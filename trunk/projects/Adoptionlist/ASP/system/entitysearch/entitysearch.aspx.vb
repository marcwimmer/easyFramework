'================================================================================
'Class:     entitySearch

'--------------------------------------------------------------------------------'
'Module:    entitySearch.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   searches entity and returns the id
'--------------------------------------------------------------------------------'
'Created:   05.04.2004 20:46:27 
'--------------------------------------------------------------------------------'
'Changed:   
'--------------------------------------------------------------------------------'

'================================================================================
'Imports:
'================================================================================
Imports easyFramework.Sys.Data
Imports easyFramework.Sys.ToolLib.DataConversion
Imports easyFramework.Sys.Data.DataTools
Imports easyFramework.Frontend.ASP.WebComponents
Imports easyFramework.Frontend.ASP.Dialog
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.ToolLib
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys


Public Class entitySearch
    Inherits efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents EfDataTable1 As easyFramework.Frontend.ASP.WebComponents.efDataTable
    Protected WithEvents efBtn_Apply As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents efBtn_Abort As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents efBtn_Search As easyFramework.Frontend.ASP.WebComponents.efButton

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

    Private msEntityName As String
    Private mlDialogWidth As Integer
    Private mlDialogHeight As Integer
    Private msImg_filter_search As String
    Private msImg_clear As String

    '================================================================================
    'Public Properties:
    '================================================================================

    '================================================================================
    'Property:  sEntityName
    '--------------------------------------------------------------------------------'
    'Purpose:   the name of the entity-type, e.g. users, usergroups; must be defined
    '           in tsEntities
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   05.04.2004 20:48:38 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Property sEntityName() As String
        Get
            Return msEntityName
        End Get
        Set(ByVal Value As String)
            msEntityName = Value
        End Set
    End Property
    ReadOnly Property sImg_filter_search() As String
        Get
            Return msImg_filter_search
        End Get

    End Property
    ReadOnly Property sImg_clear() As String
        Get
            Return msImg_clear
        End Get
    End Property

    '================================================================================
    'Private Methods:
    '================================================================================
    Private Sub Page_Load(ByVal oXmlRequest As xmldocument) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        'get the entity:
        If Request("Entity") = "" Then
            Throw New efException("entity-type is missing, e.g. ?entity=user")
        End If
        sEntityName = Request("Entity")

        Dim rsEntity As Recordset = DataMethodsClientInfo.gRsGetTable(Me.oClientInfo, _
            "tsEntities", "*", "ety_name='" & SQLString(sEntityName) & "'")

        If rsEntity.EOF Then
            Throw New efException("Entity """ & sEntityName & """ wasn't found!")
        End If


        'setup datatable:
        Me.EfDataTable1.sXmlAddParams = "<entity>" & sEntityName & "</entity>"



        '-------get image-url--------------
        msImg_filter_search = _
            Images.sGetImageURL(oClientInfo, "filter_search", _
                Request.ApplicationPath)
        msImg_clear = _
            Images.sGetImageURL(oClientInfo, "clear", _
                Request.ApplicationPath)



        '-------------js, css, title----------------
        sTitle = rsEntity("ety_title").sValue
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddScriptLink("../../js/efDataTable.js", True)
        gAddScriptLink("../../js/efTabDialog.js", True)
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddCss("../../css/efstyledatatable.css", True)
        gAddCss("../../css/efstyletabdlg.css", True)




    End Sub



End Class
