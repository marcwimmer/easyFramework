'================================================================================
'Class:     entitySearchSelectField

'--------------------------------------------------------------------------------'
'Module:    entitySearchSelectField.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   the user can select a field and enter a search-string here;
'           the modal-result is
'           FIELDNAME;search-string
'--------------------------------------------------------------------------------'
'Created:   25.04.2004 16:52:21 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.sys.ToolLib.Functions
Imports easyFramework.Frontend.ASP.WebComponents
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools.EntityLoader
Imports easyFramework.Sys.Entities
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys


Public Class entitySearchSelectField
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents EfDataTable1 As easyFramework.Frontend.ASP.WebComponents.efDataTable
    Protected WithEvents efBtn_Ok As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents efBtn_Abort As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfXmlDialog1 As easyFramework.Frontend.ASP.WebComponents.efXmlDialog

    'HINWEIS: Die folgende Platzhalterdeklaration ist für den Web Form-Designer erforderlich.
    'Nicht löschen oder verschieben.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Dieser Methodenaufruf ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        '------------get the entity------------------------
        If Request("Entity") = "" Then
            Throw New efException("entity-type is missing, e.g. ?entity=user")
        End If
        Dim sEntityName As String = Request("Entity")

        Dim oEntity As DefaultEntity = goLoadEntity(oClientInfo, sEntityName)


        '-------------setup dialog-xml --------------------
        Dim oXml As New XmlDocument("<efDialogPage MULTIROW=""0"" DIALOGSIZE=""1"">" & _
            "<efDialogRow>" & _
          "<efDialogField>" & _
            "<DESC>Suchbe$griff</DESC>" & _
           "<NAME>searchphrase</NAME>" & _
           "<TYPE>INPUT</TYPE>" & _
          "</efDialogField></efDialogRow><efDialogRow>" & _
          "<efDialogField COLSPANFIELD=""1"">" & _
           "<DESC>Such$feld</DESC>" & _
           "<NAME>searchfield</NAME>" & _
           "<TYPE>LISTCOMBO</TYPE>" & _
           "<SRC></SRC>" & _
           "<DATA></DATA>" & _
          "</efDialogField>" & _
         "</efDialogRow>" & _
         "</efDialogPage>")

        For I As Integer = 0 To oEntity.asSearchFields.Length - 1



            oXml.selectSingleNode("//SRC").sText += _
                oEntity.oTableDef.gsGetFieldDescription(oEntity.asSearchFields(I).sName) & _
            gsCStr(IIf(I < oEntity.asSearchFields.Length - 1, ";", ""))


            oXml.selectSingleNode("//DATA").sText += _
                oEntity.asSearchFields(I).sName & _
                gsCStr(IIf(I < oEntity.asSearchFields.Length - 1, ";", ""))

        Next
        EfXmlDialog1.sDefinitionFile = oXml.sXml


        '----------js, css, title----------
        sTitle = oEntity.sTitle
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddScriptLink("../../js/efDataTable.js", True)
        gAddScriptLink("../../js/efTabDialog.js", True)
        gAddScriptLink("../../js/efClientData.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddCss("../../css/efstyledatatable.css", True)
        gAddCss("../../css/efstyletabdlg.css", True)

    End Sub

End Class
