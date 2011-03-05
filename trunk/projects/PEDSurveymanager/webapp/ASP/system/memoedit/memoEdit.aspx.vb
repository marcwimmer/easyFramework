'================================================================================
'Class:     memoEdit
'--------------------------------------------------------------------------------'
'Module:    memoEdit.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   for editing a memo-field; can be used for any memo-fields, in
'           any table
'--------------------------------------------------------------------------------'
'Created:   12.05.2004 22:11:27 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Frontend.ASP.WebComponents
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys

Public Class memoEdit
    Inherits efDialogPage

    Const efbDefaultUseHtmlEditor As Boolean = True 'use extended html-editor per default

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents btnOk As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents btnAbort As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfXmlDialog1 As easyFramework.Frontend.ASP.WebComponents.efXmlDialog
    Protected WithEvents EfXmlDialog2 As easyFramework.Frontend.ASP.WebComponents.efXmlDialog
    Protected WithEvents FreeTextBox1 As FreeTextBoxControls.FreeTextBox

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region


    Protected msTableName As String
    Protected msPrimaryFieldName As String
    Protected msPrimaryFieldValue As String
    Protected msMemoFieldName As String
    Protected msMemoFieldValue As String


    Private Sub Page_Load1(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        '--------------------------read parameters either from
        '       request-oject or from table tsEntityMemos -----------------------------------
        Dim bDefaultUseOfHtmlEditor As Boolean

        If oXmlRequest.sGetValue("eme_id") <> "" Then

            '-------------------------get values from entity--------------------

            Dim rsEntityMemos As Recordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, _
                "tsEntityMemos", , "eme_id=" & oXmlRequest.sGetValue("eme_id"))

            Dim oEntity As IEntity = EntityLoader.goLoadEntity(oClientInfo, _
                rsEntityMemos("eme_ety_name").sValue)

            oEntity.gLoad(oClientInfo, oXmlRequest.sGetValue("ety_id"))

            msTableName = oEntity.sTableName
            msPrimaryFieldName = oEntity.sKeyFieldName
            msPrimaryFieldValue = oEntity.oKeyField.sValue
            msMemoFieldName = rsEntityMemos("eme_fieldname").sValue
            msMemoFieldValue = oEntity.oFields(msMemoFieldName).sValue
            bDefaultUseOfHtmlEditor = rsEntityMemos("eme_extendedHtmlEditor").bValue
            sTitle = rsEntityMemos("eme_caption").sValue

        Else
            bDefaultUseOfHtmlEditor = efbDefaultUseHtmlEditor
            If oXmlRequest.sGetValue("Table") = "" Then
                Throw New efException("Parameter ""Table"" required.")
            Else
                msTableName = oXmlRequest.sGetValue("Table")
            End If

            If oXmlRequest.sGetValue("PKField") = "" Then
                Throw New efException("Parameter ""PKField"" (primary key field) required.")
            Else
                msPrimaryFieldName = oXmlRequest.sGetValue("PKField")
            End If

            If oXmlRequest.sGetValue("PKValue") = "" Then
                Throw New efException("Parameter ""PKValue"" (primary key value) required.")
            Else
                msPrimaryFieldValue = oXmlRequest.sGetValue("PKValue")
            End If

            If oXmlRequest.sGetValue("MemoField") = "" Then
                Throw New efException("Parameter ""MemoField"" (column name of memo-field) required.")
            Else
                msMemoFieldName = oXmlRequest.sGetValue("MemoField")
            End If

            '-------------------------get memo-value---------------------------------------
            Dim rs As Recordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, msTableName, _
                msMemoFieldName, msPrimaryFieldName & "='" & DataTools.SQLString( _
                    msPrimaryFieldValue) & "'")

            If rs.EOF Then
                Throw New efException("Recordset with primary-key value """ & msPrimaryFieldValue & """ doesn't exist!")
            End If


            msMemoFieldValue = rs(msMemoFieldName).sValue

            sTitle = "Memotext"

        End If



        '---------------------build xml for xml-dialog--------------------
        Dim oXmlData As New XmlDocument("<DIALOGINPUT/>")

        oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("Table", True).sText = _
            msTableName
        oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("PrimaryFieldName", True).sText = _
            msPrimaryFieldName
        oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("PrimaryFieldValue", True).sText = _
            msPrimaryFieldValue
        oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("MemoFieldName", True).sText = _
            msMemoFieldName
        oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("MemoFieldValue", True).sText = _
            msMemoFieldValue

        EfXmlDialog1.sXmlValues = oXmlData.sXml


        '-------set up extended html editor------------
        FreeTextBox1.SupportFolder = "../freeTextBox/"

        '--------decide textarea or html-editor------
        If mbUseExtendedHtmlEditor(oClientInfo, bDefaultUseOfHtmlEditor) Then
            EfXmlDialog2.Visible = False
            EfXmlDialog2.sXmlValues = ""

            FreeTextBox1.Text = msMemoFieldValue

        Else
            '------EfXmlDialog2 contains the textarea-memo-edit-------
            EfXmlDialog2.sXmlValues = oXmlData.sXml
            FreeTextBox1.Visible = False

        End If

        '---------------------inits--------------------
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../freeTextBox/FreeTextBox-mainScript.js", False)
        gAddScriptLink("../freeTextBox/FreeTextBox-ToolbarItemsScript.js", False)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)





    End Sub


    '================================================================================
    'Function:  mbUseExtendedHtmlEditor
    '--------------------------------------------------------------------------------'
    'Purpose:   decides, wether to use the extended html-editor or not
    '--------------------------------------------------------------------------------'
    'Params:    bDefaultUseOfHtmlEditor - value of field eme_extendedHtmlEditor
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   25.05.2004 14:12:21 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Function mbUseExtendedHtmlEditor(ByVal oClientInfo As ClientInfo, _
        ByVal bDefaultUseOfHtmlEditor As Boolean) As Boolean

        If oClientInfo.rsLoggedInUser("usr_not_extendedHtmlEditor").bValue = True Then
            Return False
        Else
            Return bDefaultUseOfHtmlEditor
        End If


    End Function

End Class
