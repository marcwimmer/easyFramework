'================================================================================
'Class:     demoTab

'--------------------------------------------------------------------------------'
'Module:    demoTab.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   Demo-Tab for the entity-edit dialog tabdialog
'--------------------------------------------------------------------------------'
'Created:   09.05.2004 21:41:06 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml



Public Class demoTab
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    
    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

    End Sub
End Class
