'================================================================================
'Class:     newbuttonOptions

'--------------------------------------------------------------------------------'
'Module:    newbuttonOptions.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   retrieves the valid options for a new button
'--------------------------------------------------------------------------------'
'Created:   31.05.2004 00:14:15 
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

Public Class newbuttonOptions
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



    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, ByVal oRequest As XmlDocument) As String




        Dim sXmlFilename As String = oRequest.selectSingleNode("//multixml").sText
        Dim sStartLevel As String = oRequest.selectSingleNode("//startlevel").sText
        Dim sThisSubLevel As String = oRequest.selectSingleNode("//levelhierarchy").sText 'either "this" or "sub"
        Dim enLevel As MultistructureRenderer.efEnumLevel

        Select Case LCase(sThisSubLevel)
            Case "sub"
                enLevel = MultistructureRenderer.efEnumLevel.sublevel
            Case "this"
                enLevel = MultistructureRenderer.efEnumLevel.thisLevel
            Case Else
                Return "Please Provide either ""this"" or ""sub"" in parameter ""levelhierachy"", when calling ""newbuttonOptions.aspx""."
        End Select

        '-------------------load the xml-definition of the struct----------------------
        If Left(sXmlFilename, 1) <> "/" Then
            Throw New efException("XML-structure filename must be absolute for calling multi-structure ondemand renderer.")
        End If


        sXmlFilename = Server.MapPath(sXmlFilename)
        Dim oXmlDefinition As New XmlDocument
        oXmlDefinition.gLoad(sXmlFilename)

        '----------get result-string-----------
        Dim oMultiStructure As New MultistructureRenderer
        Dim sResult As String
        sResult = "SUCCESS" & oMultiStructure.gsRenderOptionValuesForNewButton(oClientInfo, _
            oXmlDefinition, _
            sStartLevel, enLevel)

        Return sResult



    End Function



End Class
