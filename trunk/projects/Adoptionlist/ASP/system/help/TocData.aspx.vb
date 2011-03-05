'================================================================================
'Class:     TocData

'--------------------------------------------------------------------------------'
'Module:    TocData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: 
'--------------------------------------------------------------------------------'
'Purpose:   
'--------------------------------------------------------------------------------'
'Created:   04.06.2004 15:41:54 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

Imports easyFramework.Sys
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml
Imports easyFramework.sys.ToolLib
Imports easyFramework.sys.ToolLib.Functions
Imports easyFramework.sys.Data.DataMethodsClientInfo
Imports easyFramework.sys.Data.DataTools
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Frontend.ASP.AspTools.Environment

Public Class TocData
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



    '================================================================================
    'Function:  sGetData
    '--------------------------------------------------------------------------------'
    'Purpose:   get the data for the TOC of the help
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   04.06.2004 15:43:45 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        '--------build the toc------------

        Dim rs As Recordset
        Dim sResult As String
        Dim sQry As String
        Dim oS As New FastString

        sQry = "SELECT " & efsFieldList & " FROM tsHelpToc " & _
            "WHERE toc_parentid IS NULL ORDER BY toc_index"

        rs = gRsGetDirect(oClientInfo, sQry)

        Do While Not rs.EOF

            mHelperBuildString(oClientInfo, rs, oS)

            rs.MoveNext()
        Loop

        Return "OK-||-" & oS.ToString
    End Function


    '================================================================================
    'Private Fields:
    '================================================================================
    Private Const efsFieldList As String = "toc_id, toc_parentid, toc_title"


    '================================================================================
    'Private Methods:
    '================================================================================

    '================================================================================
    'Sub:       helper
    '--------------------------------------------------------------------------------'
    'Purpose:   for doing iterative calls
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   02.04.2004 10:08:06 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Private Sub mHelperBuildString(ByVal oClientInfo As ClientInfo, _
        ByVal rsCurrentLine As Recordset, _
        ByVal sResultString As FastString)

        '------get children now, because then not a folder-image but a textitem-image is displayed----
        Dim sQry As String = _
        "SELECT " & efsFieldList & " FROM tsHelpToc " & _
        "WHERE toc_parentid = '" & _
        SQLString(rsCurrentLine("toc_id").sValue) & _
        "' ORDER BY toc_index"

        Dim rsChildren As Recordset = gRsGetDirect(oClientInfo, sQry)


        '--------------mnu-parent--------------------
        If rsCurrentLine("toc_parentid").sValue = "" Then
            sResultString.Append("NULL|")
        Else
            sResultString.Append(rsCurrentLine("toc_parentid").sValue & "|")
        End If

        'mnu-id:
        sResultString.Append(rsCurrentLine("toc_id").sValue & "|")

        '---------------------------mnu-text---------------------------
        sResultString.Append(rsCurrentLine("toc_title").sValue & "|")

        '---------------------------mnu-command---------------------------
        Dim sCommand As String
        sCommand = "mLoadContent(null, '" & rsCurrentLine("toc_id").sValue & "');"

        sResultString.Append(sCommand & "|")

        '---------------------------is folder---------------------------
        sResultString.Append("0" & "|")

        '---------------------------icon normal---------------------------
        Dim sImage As String
        If rsChildren.EOF Then
            sImage = "treeview_item"
        Else
            sImage = "treeview_folder"
        End If

        sResultString.Append( _
            Images.sGetImageURL(oClientInfo, sImage, _
            Request.ApplicationPath) & _
            "|")


        '-------------------------icon opened---------------------------
        If rsChildren.EOF Then
            sImage = "treeview_item"
        Else
            sImage = "treeview_folder_open"
        End If
        sResultString.Append( _
            Images.sGetImageURL(oClientInfo, sImage, _
            Request.ApplicationPath) & _
            "|")



        '---------------------------carriage return line feed---------------------------
        sResultString.Append("-||-")

        Do While Not rsChildren.EOF

            mHelperBuildString(oClientInfo, rsChildren, sResultString)

            rsChildren.MoveNext()
        Loop

    End Sub

End Class
