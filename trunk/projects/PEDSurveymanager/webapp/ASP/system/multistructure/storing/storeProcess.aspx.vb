'================================================================================
'Class:     storeProcess       
'--------------------------------------------------------------------------------'
'Module:    storeProcess.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH, 2004
'--------------------------------------------------------------------------------'
'Purpose:   stores the content of the multistructure-edit
'
'           returns "SUCCESS" or the error-string
'--------------------------------------------------------------------------------'
'Created:   03.05.2004 16:41:48 
'--------------------------------------------------------------------------------'
'Changed:   16.05.2004 - transactions added, Marc
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Frontend.ASP.ComplexObjects
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys

Public Class storeProcess
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


    Public Overrides Function sGetData(ByVal oClientInfo As ClientInfo, _
        ByVal oRequest As XmlDocument) As String


        If oClientInfo.bHasErrors Then
            Return oClientInfo.gsErrors
        End If


        MultiStructureHelper.gStoreMultiStructureInDatabase(oClientInfo, oRequest, Request, Application, Server)


        If oClientInfo.bHasErrors Then
            Return oClientInfo.gsErrors
        Else
            Return "SUCCESS"
        End If

    End Function



End Class
