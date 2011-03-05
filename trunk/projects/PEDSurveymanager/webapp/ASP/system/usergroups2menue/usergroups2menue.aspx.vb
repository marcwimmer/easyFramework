'================================================================================
'Class:     usergroups2menue
'--------------------------------------------------------------------------------'
'Module:    usergroups2menue.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   setting which usergroup has access to which menue-item
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

Public Class usergroups2menue
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

    Protected oXmlMenueItems As XmlDocument

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        sTitle = "Menüeintrage zuordnen"
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

        '-------------get user-groups-----------------
        oXmlMenueItems = goXmlGetMenueItems(oClientInfo)

        
        '---------load user-group entity-------------------
        oUserGroupEntity = easyFramework.Frontend.ASP.AspTools.EntityLoader.goLoadEntity(oClientInfo, _
            "Usergroups")
        oUserGroupEntity.gLoad(oClientInfo, gsCStr(lGrp_id))


    End Sub



    '================================================================================
    'Function:  gbIsMnu_id
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
    Public Function gbIsMnu_id(ByVal sMnu_id As String) As Boolean

        If DataMethodsClientInfo.glDBCount(oClientInfo, "tsMenuAccessTsUserGroups", _
            , "msg_mnu_id='" & DataTools.SQLString(sMnu_id) & "' And msg_grp_id=" & lGrp_id) > 0 Then
            Return True
        Else
            Return False
        End If

    End Function



    '================================================================================
    'Function:  moXmlGetMenueItems
    '--------------------------------------------------------------------------------'
    'Purpose:   returns the menue-items for the dialog
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   20.05.2004 23:24:58 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Shared Function goXmlGetMenueItems(ByVal oClientInfo As ClientInfo) As XmlDocument

        Dim rsMenueItems As Recordset
        Dim oXmlResult As New XmlDocument("<menueitems/>")


        '--------start with top elements-----------
        rsMenueItems = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsMainMenue", "mnu_id, mnu_title", _
            "mnu_parentid is null", , , "mnu_index ASC")

        Do While Not rsMenueItems.EOF

            mHelpGetMenueItems(oClientInfo, oXmlResult, rsMenueItems("mnu_id").sValue, _
                rsMenueItems("mnu_title").sValue)

            rsMenueItems.MoveNext()
        Loop

        Return oXmlResult
    End Function

    '================================================================================
    'Private Methods:
    '================================================================================


    '================================================================================
    'Sub:       mHelpGetMenueItems
    '--------------------------------------------------------------------------------'
    'Purpose:   helper for adding the menue-items
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   20.05.2004 23:31:45 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Private Shared Sub mHelpGetMenueItems(ByVal oClientInfo As ClientInfo, ByRef oXmlDoc As XmlDocument, ByVal mnu_parentid As String, _
        ByVal mnu_parent_title As String)

        '------------ad the parent-item---------------
        Dim oNode As XmlNode = oXmlDoc.selectSingleNode("/menueitems").AddNode( _
            "item", False)

        oNode.AddNode("id", True).sText = mnu_parentid
        oNode.AddNode("title", True).sText = mnu_parent_title


        '-------step through the children--------------
        Dim rsChildren As Recordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsMainMenue", _
            "mnu_id, mnu_title", "mnu_parentid='" & DataTools.SQLString(mnu_parentid) & "'", _
            , , "mnu_index ASC")

        Do While Not rsChildren.EOF

            mHelpGetMenueItems(oClientInfo, oXmlDoc, rsChildren("mnu_id").sValue, mnu_parent_title & " / " & rsChildren("mnu_title").sValue)

            rsChildren.MoveNext()
        Loop

    End Sub

End Class
