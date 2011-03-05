'================================================================================
'Class:     main

'--------------------------------------------------------------------------------'
'Module:    main.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   main-form   
'--------------------------------------------------------------------------------'
'Created:   31.03.2004 22:57:06 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools.Environment
Imports easyFramework.Sys.Xml


Public Class main
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents efMenuTree As easyFramework.Frontend.ASP.WebComponents.efTreeview

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal oXmlRequest As xmldocument) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        sTitle = goGetEnvironment(Application).gsProjectName
        gAddScriptLink("../js/efStandard.js", True)
        gAddScriptLink("../js/efWindow.js", True)
        gAddScriptLink("../js/efDlgParams.js", True)
        gAddScriptLink("../js/efServerProcess.js", True)
        gAddScriptLink("../js/efTreeview.js", True)
        gAddScriptLink("../js/efOnlineHelp.js", True)
        gAddCss("../css/efstyledefault.css", True)
        gAddCss("../css/efstyledialogtable.css", True)

        efMenuTree.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath)
        efMenuTree.sDataPage = "MainMenuData.aspx"




    End Sub

End Class
