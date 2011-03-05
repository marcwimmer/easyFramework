'================================================================================
'Class:     storeClientData

'--------------------------------------------------------------------------------'
'Module:    storeClientData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   stores client-data to the database
'--------------------------------------------------------------------------------'
'Created:   20.08.2004 00:32:32 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.AspTools
Imports easyframework.Frontend.ASP.AspTools.Tools
Imports easyFramework.Sys

Public Class storeClientData
    Inherits easyFramework.Frontend.ASP.Dialog.efProcessPage

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

    '================================================================================
    'Function:  sGetData
    '--------------------------------------------------------------------------------'
    'Purpose:   retrieves the value of the stored information from the clientinfo
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   20.08.2004 00:33:39 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, ByVal oRequest As Xml.XmlDocument) As String

        Dim sName As String
        Dim sValue As String

        sName = oRequest.sGetValue("name")
        sValue = oRequest.sGetValue("value")

        oClientInfo(sName) = sValue

        Return "OK"

    End Function



End Class
