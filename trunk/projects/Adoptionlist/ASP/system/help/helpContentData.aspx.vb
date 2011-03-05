'================================================================================
'Class:     helpContentData

'--------------------------------------------------------------------------------'
'Module:    helpContentData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   loads the content of the help
'--------------------------------------------------------------------------------'
'Created:   04.06.2004 21:58:27 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.sys.Xml
Imports easyFramework.sys.Data.DataMethodsClientInfo

Public Class helpContentData
    Inherits efDataPage

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

    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, ByVal oRequest As Xml.XmlDocument) As String


        '---------------get hlp_id-------------------------------
        Dim sHlp_id As String = oRequest.sGetValue("hlp_id")
        Dim sToc_id As String = oRequest.sGetValue("toc_id")
        Dim sCustom_id As String = oRequest.sGetValue("custom_id")

        If sHlp_id = "" And sToc_id = "" And sCustom_id = "" Then
            Return "Parameter ""hlp_id"", ""toc_id"" or ""custom_id"" is required!"
        End If



        Dim rsHelpChapters As Recordset

        If sHlp_id <> "" Then
            rsHelpChapters = gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_heading, hlp_body", _
                       "hlp_id='" & sHlp_id & "'")
        ElseIf sToc_id <> "" Then
            rsHelpChapters = gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_heading, hlp_body", _
                       "hlp_toc_id='" & sToc_id & "'")
        Else
            rsHelpChapters = gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_heading, hlp_body", _
                       "hlp_customid='" & sCustom_id & "'")
        End If


        If rsHelpChapters.EOF = True Then
            Return "OK-||-no heading-||-no body"
        Else
            Return "OK-||-" & rsHelpChapters("hlp_heading").sValue & "-||-" & rsHelpChapters("hlp_body").sValue
        End If

    End Function
End Class
