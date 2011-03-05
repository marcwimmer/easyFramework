'================================================================================
'Class:     MainMenuData

'--------------------------------------------------------------------------------'
'Module:    MainMenuData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: 
'--------------------------------------------------------------------------------'
'Purpose:   
'--------------------------------------------------------------------------------'
'Created:   01.04.2004 21:39:46 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Frontend.ASP.AspTools.Environment
Imports easyFramework.Sys.data
Imports easyFramework.Sys.data.DataMethodsClientInfo
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys
Imports easyFramework.Sys.ToolLib
Imports System.Text

Public Class MainMenuData
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
    'Public Methods:
    '================================================================================



    '================================================================================
    'Function:  sGetData
    '--------------------------------------------------------------------------------'
    'Purpose:   returns the main-menu data for the treeview 
    '--------------------------------------------------------------------------------'
    'Params:    clientinfo, request
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   02.04.2004 10:01:16 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String


        Dim rs As Recordset
        Dim sResult As String
        Dim sQry As String
        Dim oS As New faststring

        sQry = "SELECT " & efsFieldList & " FROM tsMainMenue " & _
            "WHERE mnu_parentid IS NULL ORDER BY mnu_index"

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
    Private Const efsFieldList As String = "mnu_id, mnu_parentid, mnu_title, " & _
        "mnu_command, mnu_modalwindow, mnu_icon_normal, mnu_icon_opened, mnu_isFolder "

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


        '-----------check access---------------
        Dim bHasAccess As Boolean
        Dim oMenuSec As easyFramework.Sys.Security.MenuTreeSec = goGetEnvironment(Application).goMenuTreeSec()

        bHasAccess = oMenuSec.gbHasAccessFromCache(oClientInfo, rsCurrentLine("mnu_id").sValue)
        If Not bHasAccess Then Exit Sub

        '--------------mnu-parent--------------------
        If rsCurrentLine("mnu_parentid").sValue = "" Then
            sResultString.Append("NULL|")
        Else
            sResultString.Append(rsCurrentLine("mnu_parentid").sValue & "|")
        End If

        'mnu-id:
        sResultString.Append(rsCurrentLine("mnu_id").sValue & "|")

        'mnu-text:
        sResultString.Append(rsCurrentLine("mnu_title").sValue & "|")

        'mnu-command:
        Dim sCommand As String
        If rsCurrentLine("mnu_command").sValue <> "" Then
            sCommand = "gsShowWindow('" & _
               oClientInfo.oHttpApp.sApplicationPath & _
               rsCurrentLine("mnu_command").sValue & "',"

            If rsCurrentLine("mnu_modalwindow").bValue = True Then
                sCommand += "true"
            Else
                sCommand += "false"
            End If
            sCommand += ");"
        End If
        sResultString.Append(sCommand & "|")

        'is folder:
        sResultString.Append(rsCurrentLine("mnu_isFolder").lValue & "|")

        'icon normal:
        sResultString.Append( _
            Images.sGetImageURL(oClientInfo, rsCurrentLine("mnu_icon_normal").sValue, _
            Request.ApplicationPath) & _
            "|")


        'icon opened:
        sResultString.Append( _
            Images.sGetImageURL(oClientInfo, rsCurrentLine("mnu_icon_opened").sValue, _
            Request.ApplicationPath) & _
            "|")



        'carriage return line feed
        sResultString.Append("-||-")

        Dim sQry As String = _
            "SELECT " & efsFieldList & " FROM tsMainMenue " & _
            "WHERE mnu_parentid = '" & _
            SQLString(rsCurrentLine("mnu_id").sValue) & _
            "' ORDER BY mnu_index"

        Dim rsChildren As Recordset = gRsGetDirect(oClientInfo, sQry)
        Do While Not rsChildren.EOF

            mHelperBuildString(oClientInfo, rsChildren, sResultString)

            rsChildren.MoveNext()
        Loop

    End Sub

End Class
