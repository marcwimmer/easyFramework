'================================================================================
'Class:     EntityProcess

'--------------------------------------------------------------------------------'
'Module:    EntityProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   stores/updates the entity
'--------------------------------------------------------------------------------'
'Created:   08.04.2004 23:31:28 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Data.Table
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataMethods
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys
Imports System.Text

Public Class EntityProcess
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


    '================================================================================
    'Private Fields:
    '================================================================================

    '================================================================================
    'Public Consts:
    '================================================================================

    '================================================================================
    'Public Properties:
    '================================================================================

    '================================================================================
    'Public Events:
    '================================================================================

    '================================================================================
    'Public Methods:
    '================================================================================
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String

        'transform the dialogdata to a recordset:
        Dim rsDialogData As Recordset = _
            easyFramework.Frontend.ASP.AspTools.Tools.grsDialogOutput2Recordset( _
                oRequest.selectSingleNode("//DIALOGOUTPUT").sXml)

        'get the entity-name:
        Dim sEntityName As String
        If oRequest.selectSingleNode("//entity") Is Nothing Then
            Throw New efException("entity-type missing.")
        Else
            sEntityName = oRequest.selectSingleNode("//entity").sText
        End If

        'load the entity-object:
        Dim oEntity As DefaultEntity
        oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName)


        If oRequest.selectSingleNode("//deleteEntity[@value='1']") Is Nothing = False Then

            oEntity.gLoad(oClientInfo, rsDialogData(oEntity.sKeyFieldName).sValue)
            oEntity.gDelete(oClientInfo)

            If oClientInfo.bHasErrors Then
                Return oClientInfo.gsErrors
            Else
                Return "SUCCESS"

            End If

        Else

            'apply the rule-file:
            Dim bResult As Boolean
            bResult = oEntity.oTableDef.gbCheckRecordset(oClientInfo, rsDialogData)

            'if rule errors exist, return the errors as string:
            If Not bResult Then
                Return oClientInfo.gsErrors()
            End If

            '----------check key-field in dialog--------------
            If Not rsDialogData.oFields.gbIsField(oEntity.sKeyFieldName) Then
                Throw New efException("The key-field must be at least a hidden-field in the dialog-xml, " & _
                    "so that the recordset can be updated!")
            End If

            'store the data:
            If rsDialogData(oEntity.sKeyFieldName).sValue = "" Then
                oEntity.gNew(oClientInfo)
            Else
                oEntity.gLoad(oClientInfo, rsDialogData(oEntity.sKeyFieldName).sValue)
            End If

            'apply the new values:
            For i As Integer = 0 To rsDialogData.oFields.Count - 1
                If oEntity.oFields.gbIsField(rsDialogData.oFields(i).sName) Then
                    oEntity.oFields(rsDialogData.oFields(i).sName).sValue = _
                        rsDialogData.oFields(i).sValue
                End If
            Next

            'store the entity:
            oEntity.gSave(oClientInfo)

            If oClientInfo.bHasErrors Then
                Return oClientInfo.gsErrors
            Else
                'return the id:
                Return "SUCCESS_" & oEntity.oKeyField.sValue

            End If


        End If




    End Function

    '================================================================================
    'Protected Properties:
    '================================================================================

    '================================================================================
    'Protected Methods:
    '================================================================================

    '================================================================================
    'Private Consts:
    '================================================================================

    '================================================================================
    'Private Fields:
    '================================================================================



End Class
