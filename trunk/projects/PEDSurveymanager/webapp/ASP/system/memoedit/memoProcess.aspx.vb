'================================================================================
'Class:     memoProcess
'--------------------------------------------------------------------------------'
'Module:    memoProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   for editing a memo-field; can be used for any memo-fields, in
'           any table
'--------------------------------------------------------------------------------'
'Created:   12.05.2004 22:11:27 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Frontend.ASP.WebComponents
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys

Public Class memoProcess
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


        '--------------------read parameters--------------------
        Dim sTableName As String
        Dim sPrimaryKeyFieldName As String
        Dim sPrimaryKeyFieldValue As String
        Dim sMemoFieldName As String
        Dim sMemoFieldValue As String

        sTableName = oRequest.sGetValue("txtTable")
        sPrimaryKeyFieldName = oRequest.sGetValue("txtPrimaryFieldName")
        sPrimaryKeyFieldValue = oRequest.sGetValue("txtPrimaryFieldValue")
        sMemoFieldName = oRequest.sGetValue("txtMemoFieldName")

        'either the data is in "txtMemoFieldValue" or in "FreeTextBox1":
        sMemoFieldValue = oRequest.sGetValue("txtMemoFieldValue")
        If sMemoFieldValue = "" Then sMemoFieldValue = oRequest.sGetValue("FreeTextBox1")


        '------------------store memo-------------------------------
        DataMethodsClientInfo.gUpdateMemo(oClientInfo, sTableName, sPrimaryKeyFieldName, _
            sPrimaryKeyFieldValue, sMemoFieldName, sMemoFieldValue)


        Return "SUCCESS"



    End Function



End Class
