'================================================================================
'Class:     entityDialogData

'--------------------------------------------------------------------------------'
'Module:    entityDialogData.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   loads the data for the xml-dialog of the entity-edit-form
'--------------------------------------------------------------------------------'
'Created:   07.04.2004 21:46:11 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys

Public Class entityDialogData
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
    'Function:  sGetData
    '--------------------------------------------------------------------------------'
    'Purpose:   fetch the entity-data and return the string for the xml-dialog to
    '           display the data
    '--------------------------------------------------------------------------------'
    'Params:    clientinfo
    '           request-object
    '--------------------------------------------------------------------------------'
    'Returns:   entity-xmldialog-data   
    '--------------------------------------------------------------------------------'
    'Created:   07.04.2004 21:47:14 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
            ByVal oRequest As XmlDocument) As String

        If oRequest.sGetValue("entity") = "" Then
            Throw New efException("entity-type is missing, e.g. &lt;entity&gtUsers&lt;entity&gt")
        End If
        Dim sEntityName As String = oRequest.sGetValue("entity")

        Dim oEntity As DefaultEntity
        oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName)

        Dim sEntityID As String
        If oRequest.sGetValue("entityId") <> "" Then
            sEntityID = oRequest.sGetValue("entityId")
            oEntity.gLoad(oClientInfo, sEntityID)
        Else
            oEntity.gNew(oClientInfo)
        End If


        Return Tools.gsXmlRecordset2DialogInput(oEntity.gRsGetRecordset, _
            Tools.efEnumDialogInputType.efJavaScriptString)


    End Function



End Class
