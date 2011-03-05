'================================================================================
'Class:     users2policies
'--------------------------------------------------------------------------------'
'Module:    users2policies.aspx.vb
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
Imports easyFramework.Frontend.ASP.AspTools.Environment
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Sys

Public Class users2policies
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

    Protected rsPolicies As Recordset

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load

        sTitle = "Policies zuordnen"
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

        '-------------get tabs-----------------
        rsPolicies = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsPolicies", "pol_id, pol_name, pol_description", _
            "", , , "pol_name ASC")


        '---------load user-group entity-------------------
        oUserEntity = easyFramework.Frontend.ASP.AspTools.EntityLoader.goLoadEntity(oClientInfo, "Users")
        oUserEntity.gLoad(oClientInfo, gsCStr(lUsr_id))


    End Sub



    '================================================================================
    'Function:  gbIsPol_id
    '--------------------------------------------------------------------------------'
    'Purpose:   checks wether the given pol-id is assigned or not
    '           by reading from table tsPoliciesTsUsers
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   20.05.2004 23:35:45 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Function glIsPol_id(ByVal lPol_id As Integer) As Integer

        Dim rstsPoliciesTsUsers As Recordset

        rstsPoliciesTsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsPoliciesTsUsers", _
            , "pol_usr_pol_id='" & DataTools.SQLString(gsCStr(lPol_id)) & _
            "' And pol_usr_usr_id=" & lUsr_id)

        If rstsPoliciesTsUsers.EOF Then
            Return 2
        Else
            If rstsPoliciesTsUsers("pol_usr_explicit_access").bValue = True Then
                Return 1
            Else
                Return 0
            End If
        End If
    End Function




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
    Protected Function msGetGroupAccess(ByVal lPol_id As Integer) As String


        If goGetEnvironment(Application).goPolicySec.gbHasAnyUserGroupAccessFromDB(oClientInfo, _
            lPol_id, lUsr_id) Then
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
    Protected Function msGetSelectCombo(ByVal lPol_id As Integer) As String
        Dim oSelect As New ComboBox
        oSelect.gAddEntry("2", "wie Gruppe")
        oSelect.gAddEntry("1", "Zugriff")
        oSelect.gAddEntry("0", "kein Zugriff")

        oSelect.sSelectedValue = gsCStr(glIsPol_id(lPol_id))

        oSelect.sName = "pol_id_" & lPol_id

        Return oSelect.gRender

    End Function

End Class
