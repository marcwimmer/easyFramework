'================================================================================
'Class:     OnSubmission

'--------------------------------------------------------------------------------'
'Module:    OnSubmission.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   handles the onsubmission-event of a survey
'--------------------------------------------------------------------------------'
'Created:   29.06.2004 08:10:00 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'
Imports easyFramework.sys
Imports easyFramework.sys.SysEvents.Interfaces
Imports easyFramework.sys.Recordset
Imports easyFramework.sys.Data.DataMethods
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.Data
Imports easyFramework.sys.Xml
Imports easyFramework.Project.SurveyManager.Submission

Public Class OnSubmission
    Implements ISysEvents



    '================================================================================
    'Public Consts:
    '================================================================================
    Public Const sFIELDNAME_EMAIL As String = "email"              'feld in result-table für email
    Public Const sFIELDNAME_FIRSTNAME As String = "firstname"      'feld in result-table für vorname
    Public Const sFIELDNAME_LASTNAME As String = "lastname"        'feld in result-table für familienname
    Public Const sFIELDNAME_SPEAKTO As String = "speakto"          'feld in result-table für anrede
    Public Const sFIELDNAME_NEWSLETTERJANEIN As String = "newsletterjanein"          'feld in result-table für anrede

    '================================================================================
    'Sub:       gHandleAfter
    '--------------------------------------------------------------------------------'
    'Purpose:   sends after submission a coupon, if activated at survey-level
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   29.06.2004 08:40:19 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Sub gHandleAfter(ByVal oClientInfo As Sys.ClientInfo, ByVal oParam As Object, _
        ByRef oReturn As Object, ByRef bRollback As Boolean) _
        Implements Sys.SysEvents.Interfaces.ISysEvents.gHandleAfter


        Dim oXmlParam As XmlDocument = CType(oParam, XmlDocument)

        Dim sResultTableName As String = oXmlParam.sGetValue("resultTableName")
        Dim sSessionID As String = oXmlParam.sGetValue("sessionid")
        Dim sConnstr_svm As String = oXmlParam.sGetValue("connstr_svm")


        Dim oConn_resultdb As efTransaction = CType(oClientInfo.oVolatileField("conn_results"), efTransaction)


        Dim rs As Recordset = DataMethods.gRsGetTable(oConn_resultdb, sResultTableName, "*", _
            "SessionID=" & sSessionID, "", "", "")

        '------------get the survey and look, if it has a coupon-----------------------
        Dim rsSurvey As Recordset = DataMethods.gRsGetTable(sConnstr_svm, "tdSurveys", "*", _
            "svy_id=" & rs("svy_id").sValue, "", "", "")


        '--------------------handle newsletter entry------------------
        If rsSurvey("svy_newsletter_active").bValue = True Then

            Dim sSubmitter_Email As String
            Dim sSubmitter_Firstname As String
            Dim sSubmitter_Lastname As String
            Dim sSubmitter_SpeakTo As String
            Dim sSubmitter_NewsletterJaNein As String

            sSubmitter_Email = rs(sFIELDNAME_EMAIL).sValue
            sSubmitter_SpeakTo = rs(sFIELDNAME_SPEAKTO).sValue
            sSubmitter_Firstname = rs(sFIELDNAME_FIRSTNAME).sValue
            sSubmitter_Lastname = rs(sFIELDNAME_LASTNAME).sValue
            sSubmitter_NewsletterJaNein = rs(sFIELDNAME_NEWSLETTERJANEIN).sValue

            If sSubmitter_NewsletterJaNein.ToLower.StartsWith("ja") Then
                '------------connection-string zur Newsletter-Datenbank zusammenbauen---------------
                Dim sConnStr_NewsletterDB As String
                sConnStr_NewsletterDB = "Data Source=" & DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tdConfig", "cfg_value", "cfg_name='NewsletterDB_DataSource'", "") & ";"
                sConnStr_NewsletterDB &= "Initial Catalog=" & DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tdConfig", "cfg_value", "cfg_name='NewsletterDB_CatalogName'", "") & ";"
                sConnStr_NewsletterDB &= "UID=" & DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tdConfig", "cfg_value", "cfg_name='NewsletterDB_UserName'", "") & ";"
                sConnStr_NewsletterDB &= "PWD=" & DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tdConfig", "cfg_value", "cfg_name='NewsletterDB_Password'", "") & ";"

                '---------check, wether email already exists-----------
                Dim sWhere As String
                Dim sNewsletterlist As String = rsSurvey("svy_newsletter_list").sValue
                sWhere = "Newsletterliste='$1' AND email='$2'"
                sWhere = Replace(sWhere, "$1", sNewsletterlist)
                sWhere = Replace(sWhere, "$2", sSubmitter_Email)
                If DataMethods.glDBCount(sConnStr_NewsletterDB, "NewsletterEmpfaenger", "", sWhere, "") = 0 Then

                    Dim sAffiliate As String
                    sAffiliate = rsSurvey("svy_name").sValue & " (SVM)"

                    Dim sValues As String
                    sValues = "'$1','$2','$3','$4','$5','$6'"
                    sValues = Replace(sValues, "$1", DataTools.SQLString(sSubmitter_SpeakTo))
                    sValues = Replace(sValues, "$2", DataTools.SQLString(sSubmitter_Firstname))
                    sValues = Replace(sValues, "$3", DataTools.SQLString(sSubmitter_Lastname))
                    sValues = Replace(sValues, "$4", DataTools.SQLString(sSubmitter_Email))
                    sValues = Replace(sValues, "$5", DataTools.SQLString(sNewsletterlist))
                    sValues = Replace(sValues, "$6", DataTools.SQLString(sAffiliate))

                    DataMethods.gInsertTable(sConnStr_NewsletterDB, "NewsletterEmpfaenger", _
                        "Anrede, Vorname, Name, email, Newsletterliste, Affiliate", _
                        sValues)


                End If

            End If

        End If




        '------------------------handle coupon---------------------
        If rsSurvey("svy_coupon").bValue = True Then


            '------------get name, email-adress from submitter of survey------------
            Dim sSubmitter_Email As String
            Dim sSubmitter_Firstname As String
            Dim sSubmitter_Lastname As String
            Dim sSubmitter_SpeakTo As String


            sSubmitter_Email = rs(sFIELDNAME_EMAIL).sValue
            sSubmitter_SpeakTo = rs(sFIELDNAME_SPEAKTO).sValue
            sSubmitter_Firstname = rs(sFIELDNAME_FIRSTNAME).sValue
            sSubmitter_Lastname = rs(sFIELDNAME_LASTNAME).sValue


            '------check, wether the eMail has submitted before or not -------------
            Dim sQryTestEMail As String = rsSurvey(sFIELDNAME_EMAIL).sValue & "='" & _
                DataTools.SQLString(rs(sFIELDNAME_EMAIL).sValue) & "'"

            '-----if more than 1 try, then ignore------
            If DataMethods.glDBCount(oConn_resultdb, sResultTableName, "", sQryTestEMail, "") > 1 Then
                Return
            End If

            '----------------create a coupon and send it----------------
            Dim sCouponSource As String
            If rsSurvey("svy_coupon_source").sValue = "" Then
                sCouponSource = "Survey " & rsSurvey("svy_name").sValue
            Else
                sCouponSource = rsSurvey("svy_coupon_source").sValue
            End If

            If sSubmitter_Email = "" Then
                oClientInfo.gAddError("eMail Adresse wird zum Verschicken des Gutscheins benötigt.")
                Return
            End If

            CouponHandler.gSendNewCoupon(oClientInfo, sCouponSource, rsSurvey("svy_coupon_price").dValue, _
               rsSurvey("svy_coupon_product_series").sValue, _
               rsSurvey("svy_coupon_bprintable").bValue, _
               "", _
               rsSurvey("svy_coupon_downloadURL").sValue, _
               rsSurvey("svy_coupon_email_sender").sValue, _
               sSubmitter_Email, sSubmitter_SpeakTo, sSubmitter_Firstname, sSubmitter_Lastname)

        End If


    End Sub

    Public Sub gHandleBefore(ByVal oClientInfo As Sys.ClientInfo, ByVal oParam As Object, ByRef oReturn As Object, ByRef bCancel As Boolean) Implements Sys.SysEvents.Interfaces.ISysEvents.gHandleBefore

    End Sub









End Class
