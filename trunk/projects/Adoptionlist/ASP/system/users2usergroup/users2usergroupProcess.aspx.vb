'================================================================================
'Class:     usergroups2userProcess

'--------------------------------------------------------------------------------'
'Module:    usergroups2userProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   store the usergroups/users assignments
'--------------------------------------------------------------------------------'
'Created:   20.05.2004 00:07:30 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys
Imports System

Public Class users2usergroupProcess
    Inherits efProcessPage

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


        Try
            oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted)


            Dim lGrp_id As Integer
            lGrp_id = glCInt(oRequest.sGetValue("grp_id"))

            If lGrp_id = 0 Then
                Throw New efException("Param ""grp_id"" missing!")
            End If

            '--------delete all current assignments of user to groups-----------
            DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsUsersTsUserGroups", _
                "usr_grp_grp_id=" & lGrp_id)

            '---------append all groups----------------------
            Dim nlGroups As XmlNodeList = oRequest.selectNodes("//*[starts-with(name(), 'usr_id_')]")
            For i As Integer = 0 To nlGroups.lCount - 1

                Dim sUsr_id As String = Right(nlGroups(i).sName, Len(nlGroups(i).sName) - Len("usr_id_"))

                Dim lUsr_grp_id As Integer = _
                    InternalClientInfo.glGetNextRecordID(oClientInfo, "tsUsersTsUserGroups")

                If nlGroups(i).sText <> "0" Then

                    DataMethodsClientInfo.gInsertTable(oClientInfo, "tsUsersTsUserGroups", _
                        "usr_grp_id,usr_grp_grp_id,usr_grp_usr_id", _
                        lUsr_grp_id & "," & lGrp_id & "," & sUsr_id)
                End If

                InternalClientInfo.gUpdateTsRecordIds(oClientInfo, "tsUsersTsUserGroups", "usr_grp_id")

            Next


            oClientInfo.CommitTrans()

            Return "SUCCESS"

        Catch ex As Exception


            oClientInfo.RollbackTrans()

            Throw ex

        End Try

    End Function



End Class
