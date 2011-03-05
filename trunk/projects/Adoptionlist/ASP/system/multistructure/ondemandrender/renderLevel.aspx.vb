'================================================================================
'Class:     renderLevel

'--------------------------------------------------------------------------------'
'Module:    renderLevel.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   used by the multistructure-component, if a new-button is clicked,
'           then this aspx-page calls the dialog-renderer to generate the input
'
'
'           required parameters:
'   
'           -current-level-id; "" if it is a new top-level in "levelid"
'           -path to multistructure-xml-file in "multixml"
'           -html-name of the form, which contains the structs "formname"
'           -xml-dialog-id: a unique id of the dialog; for making the refresh-function
'            for example in "xmldialogid"
'           -namepraefix: the global name-praefix for all input-elements
'            usually the level-id in "namepraefix"
'
'
'           returns:
'           html for input-boxes
'           
'--------------------------------------------------------------------------------'
'Created:   02.05.2004 18:56:53 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'


'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog

Imports easyFramework.Frontend.ASP.AspTools.Tools
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys


Public Class renderLevel
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

   

    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String


        Dim sNamePraefix As String = oRequest.selectSingleNode("//namepraefix").sText
        Dim sXmlDialogID As String = oRequest.selectSingleNode("//xmldialogid").sText
        Dim sHtmlFormName As String = oRequest.selectSingleNode("//formname").sText
        Dim sXmlFilename As String = oRequest.selectSingleNode("//multixml").sText
        Dim sStartLevel As String = oRequest.selectSingleNode("//startlevel").sText

        '-------------------load the xml-definition of the struct----------------------
        If Left(sXmlFilename, 1) <> "/" Then
            Throw New efException("XML-structure filename must be absolute for calling multi-structure ondemand renderer.")
        End If


        sXmlFilename = Server.MapPath(sXmlFilename)
        Dim oXmlDefinition As New XmlDocument
        oXmlDefinition.gLoad(sXmlFilename)



        '-----------------render and return html------------------------------
        Dim oMultiStructure As New MultistructureRenderer
        Dim sResult As String
        sResult = oMultiStructure.gsRenderSpecificLevel(oClientInfo, _
            oXmlDefinition, _
            sHtmlFormName, _
            sXmlDialogID, _
            sNamePraefix, _
            sStartLevel, _
            Request, Application, Server)

        Return sResult

    End Function



End Class
