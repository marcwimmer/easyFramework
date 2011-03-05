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

Public Class usergroups2user
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

    Protected lUsr_id As Integer

    Protected oUserEntity As IEntity

    Protected rsUserGroups As recordset

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        sTitle = "Gruppen zuordnen"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efPopupMenue.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)

        '-----------get the user-id-------------
        If oXmlRequest.sGetValue("usr_id") = "" Then
            Throw New efException("Parameter ""usr_id"" is required!")
        Else
            lUsr_id = glCInt(oXmlRequest.sGetValue("usr_id"))
        End If

        '-------------get user-groups-----------------
        rsUserGroups = DataMethodsClientInfo.gRsGetTable(oClientInfo, _
            "tsUserGroups", , , , , "grp_name")

        '---------load user entity-------------------
        oUserEntity = easyFramework.Frontend.ASP.AspTools.EntityLoader.goLoadEntity(oClientInfo, "Users")
        oUserEntity.gLoad(oClientInfo, gsCStr(lUsr_id))


    End Sub


    Public Function gbIsUsrgrp_id(ByVal lGrp_id As Integer) As Boolean

        If DataMethodsClientInfo.glDBCount(oClientInfo, "tsUsersTsUserGroups", _
            , "usr_grp_usr_id=" & lUsr_id & " And usr_grp_grp_id=" & lGrp_id) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
