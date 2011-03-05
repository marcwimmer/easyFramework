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
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Frontend.ASP.AspTools.Environment
Imports easyFramework.Sys

Public Class users2menue
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
        If oXmlRequest.sGetValue("usr_id") = "" Then
            Throw New efException("Parameter ""usr_id"" is required!")
        Else
            lUsr_id = glCInt(oXmlRequest.sGetValue("usr_id"))
        End If

        '-------------get user-groups-----------------
        oXmlMenueItems = goXmlGetMenueItems(oClientInfo)


        '---------load user-group entity-------------------
        oUserEntity = easyFramework.Frontend.ASP.AspTools.EntityLoader.goLoadEntity(oClientInfo, "Users")
        oUserEntity.gLoad(oClientInfo, gsCStr(lUsr_id))


    End Sub



    '================================================================================
    'Function:  gbIsMnu_id
    '--------------------------------------------------------------------------------'
    'Purpose:   checks wether the given menue-id is assigned or not
    '           by reading from table tsMenuAccessTsUserGroups
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   2 - like group
    '           1 - access
    '           0 - no access
    '--------------------------------------------------------------------------------'
    'Created:   20.05.2004 23:35:45 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Function glIsMnu_id(ByVal sMnu_id As String) As Integer

        Dim rstsMenuAccessTsUsers As Recordset

        rstsMenuAccessTsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsMenuAccessTsUsers", _
            , "msu_mnu_id='" & DataTools.SQLString(sMnu_id) & "' And msu_usr_id=" & lUsr_id)

        If rstsMenuAccessTsUsers.EOF Then
            Return 2
        Else
            If rstsMenuAccessTsUsers("msu_explicit_access").bValue = True Then
                Return 1
            Else
                Return 0
            End If
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
    'Protected Methods:
    '================================================================================

    '================================================================================
    'Function:  msGetGroupAccess
    '--------------------------------------------------------------------------------'
    'Purpose:   returns the access of the users-groups
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   21.05.2004 13:59:27 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Function msGetGroupAccess(ByVal sMnu_id As String) As String


        If goGetEnvironment(Application).goMenuTreeSec.gbHasAnyUserGroupAccessFromDB(oClientInfo, _
            sMnu_id, lUsr_id) Then
            Return "Zugriff"
        Else
            Return "kein Zugriff"
        End If

    End Function

    '================================================================================
    'Function:  msGetSelectCombo
    '--------------------------------------------------------------------------------'
    'Purpose:   gets the select-input-element for assigning the user-rights;
    '           the correct-value is already chosen
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   21.05.2004 13:45:59 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Protected Function msGetSelectCombo(ByVal sMnu_id As String) As String
        Dim oSelect As New ComboBox
        oSelect.gAddEntry("2", "wie Gruppe")
        oSelect.gAddEntry("1", "Zugriff")
        oSelect.gAddEntry("0", "kein Zugriff")

        oSelect.sSelectedValue = gsCStr(glIsMnu_id(sMnu_id))

        oSelect.sName = "mnu_id_" & sMnu_id

        Return oSelect.gRender
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
