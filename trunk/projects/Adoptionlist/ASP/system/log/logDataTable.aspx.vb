'================================================================================
'Class:     entityDataTable

'--------------------------------------------------------------------------------'
'Module:    entityDataTable.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   data for the data-table 
'--------------------------------------------------------------------------------'
'Created:   05.04.2004 23:54:16 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.Entities
Imports easyFramework.sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys

Public Class logDataTable
    Inherits easyFramework.Frontend.ASP.Dialog.efDataPage

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
    'Purpose:   returns the data for the datatable-control
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   05.04.2004 23:54:01 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        Dim sResult As String

        Dim lPage As Integer
        Dim lPageSize As Integer
        If oRequest.selectSingleNode("//page") Is Nothing Then
            lPage = 1
        Else
            lPage = glCInt(oRequest.selectSingleNode("//page").sText)
            If lPage < 1 Then lPage = 1
        End If
        If oRequest.selectSingleNode("//rowsperpage") Is Nothing Then
            lPageSize = 20
        Else
            lPageSize = glCInt(oRequest.selectSingleNode("//rowsperpage").sText)
            If lPageSize < 1 Then lPageSize = 1
        End If

        Dim rsLogEntries As Recordset = _
            DataMethodsClientInfo.gRsGetTablePaged(oClientInfo, "tsLog", _
                lPage, lPageSize, "*")
        Dim lRecordcount As Integer = DataMethodsClientInfo.glDBCount(oClientInfo, "tsLog")


        Dim oSBuilder As New FastString
        oSBuilder.Append("OK-||-")
        oSBuilder.Append(gsCStr(lRecordcount) & "-||-")
        oSBuilder.Append("Typ|Nachricht|Datum|Client-ID|Username|-||-")
        oSBuilder.Append("5%|45%|5%|7%|15%|-||-")

        Do While Not rsLogEntries.EOF

            oSBuilder.Append( _
                rsLogEntries("log_id").sValue & "|" & _
                rsLogEntries("log_type").sValue & "|" & _
                Replace(rsLogEntries("log_message").sValue, "|", "") & "|" & _
                DataConversion.gsFormatDate(oClientInfo.sLanguage, rsLogEntries("log_date").dtValue) & "|" & _
                rsLogEntries("log_clientid").sValue & "|" & _
                rsLogEntries("log_username").sValue & "|-||-" _
            )
            rsLogEntries.MoveNext()
        Loop


        Return oSBuilder.ToString


    End Function
End Class
