'================================================================================
'Class:     loginProcess

'--------------------------------------------------------------------------------'
'Module:    loginProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: 
'--------------------------------------------------------------------------------'
'Purpose:   
'--------------------------------------------------------------------------------'
'Created:   23.03.2004 01:42:26 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports System.Web.HttpApplication
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.data
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys

Public Class loginProcess
    Inherits efProcessPage

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

    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        Try

            Dim sUsername As String
            Dim sPassword As String
            sUsername = oRequest.selectSingleNode("//txtusername").sText
            sPassword = oRequest.selectSingleNode("//txtpassword").sText

            oClientInfo = _
                ClientInfo.goGetNewClientInfo( _
                    easyFramework.Frontend.ASP.AspTools.Environment.goGetEnvironment( _
                    Application).gsConnStr, sUsername, sPassword)




        Catch ex As efException
            Return ex.Message

        Finally

        End Try


        Return "SUCCESS_" & oClientInfo.sClientID

    End Function


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
        '
    End Sub
End Class
