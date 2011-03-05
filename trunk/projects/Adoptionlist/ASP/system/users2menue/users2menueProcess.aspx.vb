'================================================================================
'Class:     users2menueProcess

'--------------------------------------------------------------------------------'
'Module:    users2menueProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   store the usergroups/menueitem assignments
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
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.AspTools.Environment
Imports easyFramework.Sys
Imports System

Public Class users2menueProcess
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


            Dim lUsr_id As Integer
            lUsr_id = glCInt(oRequest.sGetValue("usr_id"))

            If lUsr_id = 0 Then
                Throw New efException("Param ""usr_id"" missing!")
            End If

            '--------delete all current assignments of user to groups-----------
            DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsMenuAccessTsUsers", _
                "msu_usr_id=" & lUsr_id)

            '---------append all groups----------------------
            Dim nlGroups As XmlNodeList = oRequest.selectNodes("//*[starts-with(name(), 'mnu_id_')]")
            For i As Integer = 0 To nlGroups.lCount - 1

                Dim sMnu_id As String = Right(nlGroups(i).sName, Len(nlGroups(i).sName) - Len("mnu_id_"))

                If nlGroups(i).sText <> "2" Then

                    Dim sExplicit_Access As String = nlGroups(i).sText

                    DataMethodsClientInfo.gInsertTable(oClientInfo, "tsMenuAccessTsUsers", _
                        "msu_mnu_id,msu_usr_id, msu_explicit_access, msu_inserted,msu_insertedby", _
                        "'" & DataTools.SQLString(sMnu_id) & "', " & _
                        gsCStr(lUsr_id) & "," & _
                        sExplicit_Access & "," & _
                        "getdate(), " & _
                        "'" & oClientInfo.gsGetUsername() & "'")
                End If

            Next


            oClientInfo.CommitTrans()


            '-------------clear security cache------------
            goGetEnvironment(Application). _
                goMenuTreeSec.gClearCache()


            Return "SUCCESS"

        Catch ex As Exception


            oClientInfo.RollbackTrans()

            Throw ex

        End Try

    End Function



End Class
