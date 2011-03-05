'================================================================================
'Class:     logEntries

'--------------------------------------------------------------------------------'
'Module:    log.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   displays the information of table tsLog; lists all errors
'--------------------------------------------------------------------------------'
'Created:   05.04.2004 20:46:27 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys
Imports System.Web

Public Class logEntries
    Inherits easyFramework.Frontend.ASP.Dialog.efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader2 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents EfDataTable1 As easyFramework.Frontend.ASP.WebComponents.efDataTable

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    '================================================================================
    'Private Fields:
    '================================================================================

    Private msEntityName As String
    Private mlDialogWidth As Integer
    Private mlDialogHeight As Integer


    '================================================================================
    'Public Properties:
    '================================================================================

    '================================================================================
    'Private Methods:
    '================================================================================
    Private Sub Page_Load(ByVal oXmlRequest As xmldocument) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        Me.EfDataTable1.sXmlAddParams = ""


        sTitle = "Log"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddScriptLink("../../js/efDataTable.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddCss("../../css/efstyledatatable.css", True)

    End Sub



End Class
