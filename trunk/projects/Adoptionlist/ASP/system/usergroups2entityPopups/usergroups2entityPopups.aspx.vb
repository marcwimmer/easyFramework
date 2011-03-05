'================================================================================
'Class:     users2menue
'--------------------------------------------------------------------------------'
'Module:    users2menue.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   setting which user has access to which menue-item
'--------------------------------------------------------------------------------'
'Created:   26.05.2004 23:18:47 
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

Public Class usergroups2entityPopups
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

    Protected oUsergroupEntity As IEntity

    Protected rsPopups As Recordset

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        sTitle = "Popup-Menüeinträge zuordnen"
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efPopupMenue.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)

        '-----------get the user-id-------------
        If oXmlRequest.sGetValue("grp_id") = "" Then
            Throw New efException("Parameter ""grp_id"" is required!")
        Else
            lGrp_id = glCInt(oXmlRequest.sGetValue("grp_id"))
        End If

        '-------------get popups-----------------
        rsPopups = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityPopups", "epp_id, epp_ety_name, epp_caption", _
            "", , , "epp_ety_name, epp_index ASC")


        '---------load user-group entity-------------------
        oUsergroupEntity = easyFramework.Frontend.ASP.AspTools.EntityLoader.goLoadEntity(oClientInfo, "UserGroups")
        oUsergroupEntity.gLoad(oClientInfo, gsCStr(lGrp_id))


    End Sub



    '================================================================================
    'Function:  gbIsPopup_id
    '--------------------------------------------------------------------------------'
    'Purpose:   checks wether the given menue-id is assigned or not
    '           by reading from table tsMenuAccessTsUserGroups
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   20.05.2004 23:35:45 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Function gbIsPopup_id(ByVal sPopup_id As String) As Boolean

        If DataMethodsClientInfo.glDBCount(oClientInfo, "tsEntityPopupAccessTsUsergroups", _
            , "epg_epp_id='" & DataTools.SQLString(sPopup_id) & "' And epg_grp_id=" & lGrp_id) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function





End Class
