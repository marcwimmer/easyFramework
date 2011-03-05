'================================================================================
'Class:     renderLevel

'--------------------------------------------------------------------------------'
'Module:    renderLevel.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   used to render an xml-dialog content on demand; 
'           is e.g. called from a tab-dialog
'
'           needs the follogwing url-parameters:
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
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Sys.Xml
Imports easyFramework.Frontend.ASP.AspTools.Tools
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.ToolLib.Functions
Imports easyFramework.Sys

Public Class renderXmlDlg
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



        '------------load xml-definition------------
        Dim oXmlDef As New XmlDocument
        oXmlDef.gLoad(Tools.sWebToAbsoluteFilename(Request, oRequest.sGetValue("xmldeffile"), False))


        Dim oXmlDlg As New xmlDialogRenderer


        Return oXmlDlg.gsRender(oClientInfo, oXmlDef, Nothing, _
            oRequest.sGetValue("xmlformname"), oRequest.sGetValue("xmldialogid"), "", _
            oRequest.sGetValue("xmldatapage"))


    End Function




End Class
