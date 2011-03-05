'================================================================================
'Class:     help

'--------------------------------------------------------------------------------'
'Module:    help.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   
'--------------------------------------------------------------------------------'
'Created:   04.06.2004 15:50:25 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.Data
Imports easyFramework.sys.ToolLib.Functions

Public Class help
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfTreeview1 As easyFramework.Frontend.ASP.WebComponents.efTreeview
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents efSearchBtn As easyFramework.Frontend.ASP.WebComponents.efButton

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
    Public msStartup_Hlp_id As String

    '================================================================================
    'Public Consts:
    '================================================================================

    '================================================================================
    'Public Properties:
    '================================================================================

    '================================================================================
    'Property:  sStartup_Hlp_id
    '--------------------------------------------------------------------------------'
    'Purpose:   the help-id which is shown on startup
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   06.06.2004 15:28:10 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public ReadOnly Property sStartup_Hlp_id() As String
        Get
            Return msStartup_Hlp_id
        End Get
    End Property

    '================================================================================
    'Public Events:
    '================================================================================

    '================================================================================
    'Public Methods:
    '================================================================================

    '================================================================================
    'Protected Properties:
    '================================================================================

    '================================================================================
    'Protected Methods:
    '================================================================================

    '================================================================================
    'Private Consts:
    '================================================================================

    '================================================================================
    'Private Fields:
    '================================================================================

    '================================================================================
    'Private Methods:
    '================================================================================
    Private Sub Page_Load1(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        sTitle = "Online Hilfe"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddCss("../../css/efstylehelp.css", True)


        '----------------update help from xml--------------
        AspTools.Environment.goGetEnvironment(Application).goHelpSystem.gUpdateDatabase(oClientInfo)

        '-------set treeview-icons---------
        EfTreeview1.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath)

        '------try to get the content for on-load-----------
        Dim sUrl As String = oXmlRequest.sGetValue("url")
        Dim sEntity As String = oXmlRequest.sGetValue("entity")
        Dim sQry As String

        If sEntity <> "" Then
            sQry = "lnk_entity='" & sEntity & "'"
        Else
            sQry = "lnk_url='" & lcase(sUrl) & "'"

        End If

        Dim sLinkId As String = DataMethodsClientInfo.gsGetDBValue(oClientInfo, _
            "tsHelpLinks", "lnk_hlp_id", sQry)
        msStartup_Hlp_id = sLinkId


    End Sub



End Class
