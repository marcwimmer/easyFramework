'================================================================================
'Class:     certifiedBrowsers

'--------------------------------------------------------------------------------'
'Module:    certifiedBrowsers.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   displays a list of browsers, which can be used
'--------------------------------------------------------------------------------'
'Created:   13.06.2004 13:49:47 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework


Public Class certifiedBrowsers
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region


    Private Sub Page_Load(ByVal oXmlRequest As Sys.Xml.XmlDocument) Handles MyBase.Load

        '-------------js, css, title----------------
        sTitle = "webbrowser-check"
        gAddScriptLink("../../../js/efStandard.js", True)
        gAddScriptLink("../../../js/efWindow.js", True)
        gAddScriptLink("../../../js/efDlgParams.js", True)
        gAddScriptLink("../../../js/efServerProcess.js", True)
        gAddScriptLink("../../../js/efTreeview.js", True)
        gAddScriptLink("../../../js/efDataTable.js", True)
        gAddScriptLink("../../../js/efTabDialog.js", True)
        gAddCss("../../../css/efstyledefault.css", True)
        gAddCss("../../../css/efstyledialogtable.css", True)
        gAddCss("../../../css/efstyledatatable.css", True)
        gAddCss("../../../css/efstyletabdlg.css", True)

    End Sub
End Class
