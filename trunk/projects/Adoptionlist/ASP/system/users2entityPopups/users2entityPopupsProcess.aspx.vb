'================================================================================
'Class:     users2menueProcess

'--------------------------------------------------------------------------------'
'Module:    users2menueProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   store the usergroups/menueitem assignments
'--------------------------------------------------------------------------------'
'Created:   20.05.2004 00:07:30 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys
Imports System

Public Class users2entityPopupsProcess
    Inherits efProcessPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'HINWEIS: Die folgende Platzhalterdeklaration ist f�r den Web Form-Designer erforderlich.
    'Nicht l�schen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region


    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, ByVal oRequest As XmlDocument) As String


        Try
            oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted)


            Dim lUsr_id As Integer
            lUsr_id = glCInt(oRequest.sGetValue("usr_id"))

            If lUsr_id = 0 Then
                Throw New efException("Param ""usr_id"" missing!")
            End If

            '--------delete all current assignments of user to groups-----------
            DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsEntityPopupAccessTsUsers", _
                "epu_usr_id=" & lUsr_id)

            '---------append all groups----------------------
            Dim nlGroups As XmlNodeList = oRequest.selectNodes("//*[starts-with(name(), 'epp_id_')]")
            For i As Integer = 0 To nlGroups.lCount - 1

                Dim sEpp_id As String = Right(nlGroups(i).sName, Len(nlGroups(i).sName) - Len("epp_id_"))


                If nlGroups(i).sText <> "2" Then
                    Dim sExplicit_Access As String = nlGroups(i).sText

                    DataMethodsClientInfo.gInsertTable(oClientInfo, "tsEntityPopupAccessTsUsers", _
                        "epu_epp_id,epu_usr_id,epu_explicit_access, epu_inserted,epu_insertedby", _
                        "'" & DataTools.SQLString(sEpp_id) & "', " & _
                        gsCStr(lUsr_id) & "," & _
                        sExplicit_Access & "," & _
                        "getdate(), " & _
                        "'" & oClientInfo.gsGetUsername() & "'")
                End If

            Next


            oClientInfo.CommitTrans()

            Return "SUCCESS"

        Catch ex As Exception


            oClientInfo.RollbackTrans()

            Throw ex

        End Try

    End Function



End Class
