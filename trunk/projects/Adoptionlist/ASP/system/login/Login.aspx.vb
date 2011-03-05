'================================================================================
'Class:     Login  
'--------------------------------------------------------------------------------'
'Module:    Login.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   logins user
'--------------------------------------------------------------------------------'
'Created:   23.03.2004 01:36:47 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys

Public Class Login
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfXmlDialog1 As easyFramework.Frontend.ASP.WebComponents.efXmlDialog
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents btnOk As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents btnAbort As easyFramework.Frontend.ASP.WebComponents.efButton
    
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
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        sTitle = "Login"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efOnlineHelp.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)






        Me.EfXmlDialog1.sXmlValues = "<DIALOGINPUT></DIALOGINPUT>"



    End Sub

    '================================================================================
    'Sub:       LoadClientInfo
    '--------------------------------------------------------------------------------'
    'Purpose:   override the init-event of the default-page; the reason is, that we
    '           have at this time no client-id yet. with the process of the login,
    '           the client-id is created.
    '
    '       
    '--------------------------------------------------------------------------------'
    'Params:    -
    '--------------------------------------------------------------------------------'
    'Created:   31.03.2004 23:11:14 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Overrides Sub LoadClientInfo()
        'nothing - do not load
    End Sub
End Class
