'================================================================================
'Class:     domainEdit

'--------------------------------------------------------------------------------'
'Module:    domainEdit.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   editing the domain-values
'--------------------------------------------------------------------------------'
'Created:   24.05.2004 17:12:13 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Sys

Public Class domainEdit
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents efbtnOk As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents efbtnAbort As easyFramework.Frontend.ASP.WebComponents.efButton

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Protected moHtmlSelectDomain As ComboBox
    Protected mlDom_id As Integer
    Protected rsDomValues As Recordset
    Protected msDomainDescription As String

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load


        '---------------js, css, title-----------
        sTitle = "Domainen"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddScriptLink("../../js/efDataTable.js", True)
        gAddScriptLink("../../js/efTabDialog.js", True)
        gAddScriptLink("../../js/efPopupMenue.js", True)
        gAddScriptLink("../../js/efIESpecials.js", True, "VBScript")
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddCss("../../css/efstyledatatable.css", True)
        gAddCss("../../css/efstyletabdlg.css", True)


        '--------get all domains---------------
        Dim rsDomains As Recordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDomains", _
             "dom_id, dom_name", , , , "dom_name")

        '------get the selected domain-----------
        Dim sSelectedDomain As String = oXmlRequest.sGetValue("dom_id")
        If sSelectedDomain <> "" Then
            mlDom_id = glCInt(sSelectedDomain)
            msDomainDescription = DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tsDomains", "dom_description", _
                "dom_id=" & mlDom_id)
        End If

        '---------setup the domain-select-combo--------
        moHtmlSelectDomain = New ComboBox
        moHtmlSelectDomain.sName = "dom_id"
        Do While Not rsDomains.EOF
            moHtmlSelectDomain.gAddEntry(rsDomains("dom_id").sValue, rsDomains("dom_name").sValue)
            rsDomains.MoveNext()
        Loop

        '------get the domain-values, if there are any-------
        rsDomValues = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDomainValues", _
            , "dvl_dom_id=" & mlDom_id, , , "dvl_caption")

        If mlDom_id <> 0 Then
            moHtmlSelectDomain.sSelectedValue = gsCStr(mlDom_id)
        End If


    End Sub




End Class
