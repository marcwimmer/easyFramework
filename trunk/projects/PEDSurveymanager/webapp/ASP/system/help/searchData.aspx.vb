'================================================================================
'Class:     searchData

'--------------------------------------------------------------------------------'
'Module:    searchData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   searches in database for the search-string
'--------------------------------------------------------------------------------'
'Created:   06.06.2004 16:07:02 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.sys
Imports easyFramework.sys.Data
Imports easyFramework.sys.ToolLib
Imports easyFramework.sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml

Public Class searchData
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

    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, ByVal oRequest As XmlDocument) As String

        '-------------------some dims-----------------
        Dim oResult As New FastString
        Dim oRsTocs As Recordset
        Dim sSearchTerm As String = oRequest.sGetValue("searchterm")

        '----------check search term--------------
        If sSearchTerm = "" Then
            Return "Please provide a search-term!"
        End If


        '-----------split term by " "-------------------
        Dim sSplitTerms As String()
        If InStr(sSearchTerm, " ") = 0 Then
            ReDim sSplitTerms(0)
            sSplitTerms(0) = sSearchTerm
        Else
            sSplitTerms = Split(sSearchTerm, " ")
        End If

        '--------------------build query--------------------
        Dim sQry As String
        For i As Integer = 0 To sSplitTerms.Length - 1
            sQry &= "PATINDEX('%" & sSplitTerms(i) & "%', hlp_body) > 0"
            If i < sSplitTerms.Length - 2 Then
                sQry &= " AND "
            End If
        Next


        '----------------get data-------------------
        oRsTocs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_toc_id, hlp_heading", sQry, , , "hlp_heading")


        '--------------build result html---------------
        If oRsTocs.EOF Then
            oResult.Append("<p>Leider wurden keine Ergebnisse zu """ & sSearchTerm & """ gefunden.")

        Else
            oResult.Append("<p>")
            oResult.Append("Suchergebnisse für """ & sSearchTerm & """:")
            oResult.Append("<table class=""borderTable"">")


            Do While Not oRsTocs.EOF
                oResult.Append("<tr><td class=""dlgField"">")

                oResult.Append("<a href=""#"" onclick=""" & _
                    "mLoadContent('" & oRsTocs("hlp_toc_id").sValue & "'); return false;"">" & _
                    oRsTocs("hlp_heading").sValue & _
                    "</a>")

                oResult.Append("</td></tr>")

                oRsTocs.MoveNext()
            Loop
            oResult.Append("</table>")

        End If

        Return oResult.ToString

    End Function



End Class
