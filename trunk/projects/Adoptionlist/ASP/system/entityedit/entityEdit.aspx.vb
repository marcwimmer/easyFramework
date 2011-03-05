'================================================================================
'Class:     entityEdit

'--------------------------------------------------------------------------------'
'Module:    entityEdit.aspx.vb
'--------------------------------------------------------------------------------'
'Copyright: Promain Software-Betreuung GmbH
'--------------------------------------------------------------------------------'
'Purpose:   loads entity infos and set ups the common entity-page
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
Imports easyFramework.sys.ToolLib
Imports easyFramework.Sys.Entities
Imports easyFramework.Frontend.ASP.WebComponents
Imports easyFramework.Frontend.ASP.AspTools
Imports easyFramework.Sys.Xml
Imports easyFramework.Sys.Security
Imports easyFramework.Sys

Public Class entityEdit
    Inherits easyFramework.Frontend.ASP.Dialog.efDialogPage

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents EfPageHeader1 As easyFramework.Frontend.ASP.WebComponents.efPageHeader
    Protected WithEvents EfScriptLinks1 As easyFramework.Frontend.ASP.WebComponents.efScriptLinks
    Protected WithEvents EfButton1 As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfButton2 As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfButton3 As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfButton4 As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfDataTable1 As easyFramework.Frontend.ASP.WebComponents.efDataTable
    Protected WithEvents EfTabDialog1 As easyFramework.Frontend.ASP.WebComponents.efTabDialog
    Protected WithEvents EfTab1 As easyFramework.Frontend.ASP.WebComponents.efTab
    Protected WithEvents EfTab2 As easyFramework.Frontend.ASP.WebComponents.efTab
    Protected WithEvents Efbutton5 As easyFramework.Frontend.ASP.WebComponents.efButton
    Protected WithEvents EfPopupMenueEntity1 As easyFramework.Frontend.ASP.WebComponents.efPopupMenueEntity

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
    Private mbFilterOnLoad As Boolean = False
    Private msEntityIdOnLoad As String
    Private msKeyFieldName As String
    Private moEntity As IEntity
    Private moEntityTabSec As EntityTabSec


    '================================================================================
    'Public Properties:
    '================================================================================
    Public ReadOnly Property oEntity() As IEntity
        Get
            Return moEntity
        End Get
    End Property

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

    ReadOnly Property sKeyFieldName() As String
        Get
            Return msKeyFieldName
        End Get
    End Property

    '================================================================================
    'Property:  bFilterOnLaod
    '--------------------------------------------------------------------------------'
    'Purpose:   true, if the dialog is loaded with a given entity-id
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   30.04.2004 00:50:47 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    ReadOnly Property bFilterOnLaod() As Boolean
        Get
            Return mbFilterOnLoad
        End Get
    End Property


    ReadOnly Property oEntityTabSec() As easyFramework.Sys.Security.EntityTabSec
        Get
            Return moEntityTabSec
        End Get
    End Property

    '================================================================================
    'Property:  sFilter
    '--------------------------------------------------------------------------------'
    'Purpose:   the value of the filter
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   30.04.2004 00:51:00 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    ReadOnly Property sEntityIdOnLoad() As String
        Get
            Return msEntityIdOnLoad
        End Get
    End Property


    '================================================================================
    'Property:  lStartDialogWidth
    '--------------------------------------------------------------------------------'
    'Purpose:   the width of the dialog at startup
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   05.04.2004 21:33:39 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Property lStartDialogWidth() As Integer
        Get
            Return mlDialogWidth
        End Get
        Set(ByVal Value As Integer)
            mlDialogWidth = Value
        End Set
    End Property


    '================================================================================
    'Property:  lStartDialogHeight
    '--------------------------------------------------------------------------------'
    'Purpose:   the height of the dialog at startup
    '--------------------------------------------------------------------------------'
    'Params:    
    '--------------------------------------------------------------------------------'
    'Created:   05.04.2004 21:33:24 
    '--------------------------------------------------------------------------------'
    'Changed:   
    '--------------------------------------------------------------------------------'
    Public Property lStartDialogHeight() As Integer
        Get
            Return mlDialogHeight
        End Get
        Set(ByVal Value As Integer)
            mlDialogHeight = Value
        End Set
    End Property

    '================================================================================
    'Private Methods:
    '================================================================================
    Private Sub Page_Load(ByVal oXmlRequest As XmlDocument) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        'get the entity:
        If Request("Entity") = "" Then
            Throw New efException("entity-type is missing, e.g. ?entity=user")
        End If
        sEntityName = Request("Entity")


        moEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName)


        Me.lStartDialogHeight = oEntity.lEditWindowHeight
        Me.lStartDialogWidth = oEntity.lEditWindowWidth


        'setup datatable:
        Me.EfDataTable1.sXmlAddParams = "<entity>" & sEntityName & "</entity>"

        'setup xml-dialog:
        'Me.EfXmlDialog1.sDefinitionFile = oEntity.sEditDialogXmlFile
        'Me.EfXmlDialog1.sDataPage = "entityDialogData.aspx"

        '-------get image-url--------------
        msImg_filter_search = _
            Images.sGetImageURL(oClientInfo, "filter_search", _
                Request.ApplicationPath)
        msImg_clear = _
            Images.sGetImageURL(oClientInfo, "clear", _
                Request.ApplicationPath)


        '---------let's see, if there is an entity to load------------
        If Request(oEntity.sKeyFieldName) <> "" Then

            mbFilterOnLoad = True
            msEntityIdOnLoad = Request(oEntity.sKeyFieldName)

        End If

        msKeyFieldName = oEntity.sKeyFieldName


        '------------load entity into popup --------------
        EfPopupMenueEntity1.gSetEntity(oClientInfo, sEntityName)
        EfPopupMenueEntity1.bWithEdit = False
        EfPopupMenueEntity1.bWithSearch = False

        '---------------js, css, title-----------
        sTitle = oEntity.sTitle
        gAddScriptLink("../../js/efStandard.js", True)
        gAddScriptLink("../../js/efWindow.js", True)
        gAddScriptLink("../../js/efDlgParams.js", True)
        gAddScriptLink("../../js/efServerProcess.js", True)
        gAddScriptLink("../../js/efTreeview.js", True)
        gAddScriptLink("../../js/efDataTable.js", True)
        gAddScriptLink("../../js/efTabDialog.js", True)
        gAddScriptLink("../../js/efPopupMenue.js", True)
        gAddScriptLink("../../js/efOnlineHelp.js", True)
        gAddScriptLink("../../js/efIESpecials.js", True, "VBScript")
        gAddCss("../../css/efstyledefault.css", True)
        gAddCss("../../css/efstyledialogtable.css", True)
        gAddCss("../../css/efstyledatatable.css", True)
        gAddCss("../../css/efstyletabdlg.css", True)


        '------------init the security-object-------------
        moEntityTabSec = Environment.goGetEnvironment(Application).goEntityTabSec

    End Sub



End Class
