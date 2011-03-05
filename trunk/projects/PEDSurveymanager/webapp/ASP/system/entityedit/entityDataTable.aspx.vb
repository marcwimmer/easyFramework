'================================================================================
'Class:     entityDataTable

'--------------------------------------------------------------------------------'
'Module:    entityDataTable.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   data for the data-table 
'--------------------------------------------------------------------------------'
'Created:   05.04.2004 23:54:16 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.AspTools
Imports easyframework.Frontend.ASP.AspTools.Tools
Imports easyFramework.Sys

Public Class entityDataTable
    Inherits easyFramework.Frontend.ASP.Dialog.efDataPage

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
    'Function:  sGetData
    '--------------------------------------------------------------------------------'
    'Purpose:   returns the data for the datatable-control
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   05.04.2004 23:54:01 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        Dim sResult As String

        Dim lPage As Integer
        Dim lPageSize As Integer
        If oRequest.selectSingleNode("//page") Is Nothing Then
            lPage = 1
        Else
            lPage = glCInt(oRequest.selectSingleNode("//page").sText)
            If lPage < 1 Then lPage = 1
        End If
        If oRequest.selectSingleNode("//rowsperpage") Is Nothing Then
            lPageSize = 20
        Else
            lPageSize = glCInt(oRequest.selectSingleNode("//rowsperpage").sText)
            If lPageSize < 1 Then lPageSize = 1
        End If


        Dim sEntityName As String = oRequest.selectSingleNode("//entity").sText

        '---------------get where-clause-------------------
        Dim sClause As String
        sClause = ""



        '----------------------load the entity---------------------------
        Dim oEntity As DefaultEntity
        oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName)
        


        '---------------handle the search-values-----------------------
        If Not oRequest.selectSingleNode("//searchdlg") Is Nothing Then

            Dim sField As String
            Dim sValue As String
            Dim sSearchDlg As String = oRequest.selectSingleNode("//searchdlg").sText

            If InStr(sSearchDlg, ";") > 0 Then
                sField = Split(sSearchDlg, ";")(0)
                sValue = Split(sSearchDlg, ";", 2)(1)

                sValue = Replace(sValue, "*", "%")

                '------------handle the cases, when the field is a sub-query or the
                '           the field is real table field--------------------------

                If TypeOf (oEntity.oTableDef.goGetField(sField)) Is easyFramework.Sys.Data.Table.TableDefLookupField Then

                    Dim oLookupField As easyFramework.Sys.Data.Table.TableDefLookupField _
                         = CType(oEntity.oTableDef.goGetField(sField), easyFramework.Sys.Data.Table.TableDefLookupField)

                    sClause = "(" & oLookupField.sGetAsSQL(False) & ") LIKE '" & SQLString(sValue) & "'"

                Else
                    sClause = "[" & sField & "] LIKE '" & SQLString(sValue) & "'"

                End If



            End If


            End If





        Dim lRecordcount As Integer
        Dim rsEntityValues As Recordset = _
            oEntity.gRsSearch(oClientInfo, sClause, , lPage, lPageSize, lRecordcount)
       

        Return gsEntityToDataTableString(oEntity, rsEntityValues, lRecordcount)


    End Function
End Class
