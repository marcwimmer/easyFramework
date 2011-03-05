Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml

Public Class multistructureDemo
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfMultiStructure1 As easyFramework.Frontend.ASP.ComplexObjects.efMultiStructure

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


        Me.EfMultiStructure1.sDefinitionFile = "xmlMultiStructure.xml"
        Me.EfMultiStructure1.sTopMostElementValue = oClientInfo.rsLoggedInUser("usr_id").sValue
        Me.EfMultiStructure1.sTopMostElementName = "any_id0"

        '---------------js, css, title-----------
        sTitle = "Demo Multistructure"
        gAddScriptLink("../js/efStandard.js", True)
        gAddScriptLink("../js/efWindow.js", True)
        gAddScriptLink("../js/efDlgParams.js", True)
        gAddScriptLink("../js/efServerProcess.js", True)
        gAddScriptLink("../js/efTreeview.js", True)
        gAddScriptLink("../js/efDataTable.js", True)
        gAddScriptLink("../js/efTabDialog.js", True)
        gAddScriptLink("../js/efPopupMenue.js", True)
        gAddScriptLink("../js/efMultiStructure.js", True)
        gAddCss("../css/efstyledefault.css", True)
        gAddCss("../css/efstyledialogtable.css", True)
        gAddCss("../css/efstyledatatable.css", True)
        gAddCss("../css/efstyletabdlg.css", True)

    End Sub

End Class
