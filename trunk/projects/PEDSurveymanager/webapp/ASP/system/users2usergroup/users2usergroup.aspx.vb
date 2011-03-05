'================================================================================
'Class:     users2usergroup  
'--------------------------------------------------------------------------------'
'Module:    users2usergroup.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   setting the roles of a user
'--------------------------------------------------------------------------------'
'Created:   19.05.2004 22:30:47 
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
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys

Public Class users2usergroup
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents efBtnSave As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents efBtnAbort As easyFramework.Frontend.ASP.WebComponents.efButton

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Protected lGrp_id As Integer

    Protected oUserGroupEntity As IEntity

    Protected rsUsers As Recordset

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        sTitle = "Benutzer zuordnen"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efPopupMenue.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)

        '-----------get the group-id-------------
        If oXmlRequest.sGetValue("grp_id") = "" Then
            Throw New efException("Parameter ""grp_id"" is required!")
        Else
            lGrp_id = glCInt(oXmlRequest.sGetValue("grp_id"))
        End If

        '-------------get users-----------------
        rsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, _
            "tsUsers", , , , , "usr_lastname, usr_firstname, usr_login")

        '---------load usergroup entity-------------------
        oUserGroupEntity = easyFramework.Frontend.ASP.AspTools.EntityLoader.goLoadEntity(oClientInfo, "UserGroups")
        oUserGroupEntity.gLoad(oClientInfo, gsCStr(lGrp_id))


    End Sub


    Public Function gbIsUsr_id(ByVal lUsr_id As Integer) As Boolean

        If DataMethodsClientInfo.glDBCount(oClientInfo, "tsUsersTsUserGroups", _
            , "usr_grp_usr_id=" & lUsr_id & " And usr_grp_grp_id=" & lGrp_id) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
