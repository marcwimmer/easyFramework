'================================================================================
'Class:     domainEditProcess

'--------------------------------------------------------------------------------'
'Module:    domainEditProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   store the content of domains
'--------------------------------------------------------------------------------'
'Created:   24.05.2004 18:14:36 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.data.Domains
Imports easyFramework.Sys
Imports System

Public Class domainEditProcess
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


        Dim sNewInternalValue As String = oRequest.sGetValue("new_internalvalue")
        Dim sNewKeyCaption As String = oRequest.sGetValue("new_caption")
        Dim sDom_id As String = oRequest.sGetValue("dom_id")

        If sDom_id = "" Then
            Throw New efException("Parameter ""dom_id"" required!")
        End If

        '-----check dom-id------
        Dim rsDomain As Recordset = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDomains", , "dom_id=" & _
            sDom_id)
        If rsDomain.EOF Then
            Throw New efException("Invalid dom_id """ & sDom_id & """!")
        End If
        Dim lDom_id As Integer = glCInt(sDom_id)


        '-----------start transaction - delete all db-values - insert new values - store transaction ---
        oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted)

        Try


            '-----insert new domain-value, if exists--------
            If sNewInternalValue <> "" Then
                Domains.gInsertDomainValue(oClientInfo, lDom_id, _
                  sNewInternalValue, sNewKeyCaption)
            End If

            '-------update existing domains---------
            Dim nlKeys As XmlNodeList = _
                oRequest.selectNodes("//*[starts-with(name(), 'dvl_id_')]")
            For i As Integer = 0 To nlKeys.lCount - 1

                Dim sDvl_id As String
                Dim sCaption As String
                Dim sInternalValue As String

                sDvl_id = nlKeys(i).sName
                If Len(sDvl_id) - Len("dvl_id_") > 0 Then
                    sDvl_id = Right(sDvl_id, Len(sDvl_id) - Len("dvl_id_"))
                End If

                sCaption = oRequest.sGetValue("dvl_caption_" & sDvl_id)
                sInternalValue = oRequest.sGetValue("dvl_internalvalue_" & sDvl_id)
                Domains.gInsertDomainValue(oClientInfo, lDom_id, sInternalValue, sCaption, glCInt(sDvl_id))

            Next


            '-----------remove deleted domain-values----------
            Dim nlDeleted As XmlNodeList = _
                oRequest.selectNodes("//*[starts-with(name(), 'dvl_deleted_id_')]")
            For i As Integer = 0 To nlDeleted.lCount - 1
                Dim sDvl_id As String
                sDvl_id = nlDeleted(i).sName
                sDvl_id = Right(sDvl_id, Len(sDvl_id) - Len("dvl_deleted_id_"))
                If nlDeleted(i).sText = "1" Then
                    Domains.gRemoveDomainValue(oClientInfo, lDom_id, sDvl_id)
                End If
            Next




            oClientInfo.CommitTrans()

        Catch ex As Exception

            oClientInfo.RollbackTrans()

            Throw ex

        End Try


        Return "SUCCESS"


    End Function


   
End Class
