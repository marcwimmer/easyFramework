'================================================================================
'Class:     entityDialogLoad

'--------------------------------------------------------------------------------'
'Module:    entityDialogLoad.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH 2004
'--------------------------------------------------------------------------------'
'Purpose:   this data-page is used for loading the entity-data into
'           a rendered dialog
'--------------------------------------------------------------------------------'
'Created:   01.04.2004 21:39:46 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Frontend.ASP.AspTools.Environment
Imports easyFramework.Sys.data
Imports easyFramework.sys.Entities
Imports easyFramework.Sys.data.DataMethods
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys
Imports System.Text

Public Class entityDialogLoad
    Inherits efDataPage

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
    'Public Methods:
    '================================================================================



    '================================================================================
    'Function:  sGetData
    '--------------------------------------------------------------------------------'
    'Purpose:   returns the main-menu data for the treeview 
    '--------------------------------------------------------------------------------'
    'Params:    clientinfo, request
    '--------------------------------------------------------------------------------'
    'Returns:   
    '--------------------------------------------------------------------------------'
    'Created:   02.04.2004 10:01:16 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        Dim sEntityName As String
        Dim sEntityID As String
        Dim sEntitySearchPhrase As String

        Dim oEntity As DefaultEntity



        If oRequest.selectSingleNode("//entity") Is Nothing Then
            Return "Entity not given."
            Exit Function
        Else
            sEntityName = oRequest.selectSingleNode("//entity").sText
        End If

        If Not oRequest.selectSingleNode("//keyvalue") Is Nothing Then
            sEntityID = oRequest.selectSingleNode("//keyvalue").sText
        End If

        If Not oRequest.selectSingleNode("//value") Is Nothing Then
            sEntitySearchPhrase = oRequest.selectSingleNode("//value").sText
        End If

        oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName)


        If sEntitySearchPhrase <> "" And sEntityID = "" Then

            Dim rsResults As Recordset = _
                oEntity.gRsSearchMainField(oClientInfo, sEntitySearchPhrase)

            If rsResults.EOF Then
                Return "SUCCESS;"
            Else
                Dim sResult As String

                oEntity.gLoad(oClientInfo, rsResults(oEntity.sKeyFieldName).sValue)

                Return "SUCCESS;" & oEntity.oFields(oEntity.sKeyFieldName).sValue & ";" & _
                    oEntity.gsToString(oClientInfo)


            End If



        ElseIf sEntityID <> "" Then
            oEntity.gLoad(oClientInfo, sEntityID)

            Return "SUCCESS;" & sEntityID & ";" & oEntity.gsToString(oClientInfo)

        End If




    End Function


End Class
