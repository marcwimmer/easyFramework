using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.Security;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     entityEdit

	//--------------------------------------------------------------------------------'
	//Module:    entityEdit.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   loads entity infos and set ups the common entity-page
	//--------------------------------------------------------------------------------'
	//Created:   05.04.2004 20:46:27
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class entityEdit : easyFramework.Frontend.ASP.Dialog.efDialogPage
	{
		public entityEdit()
		{
			mbFilterOnLoad = false;
		}


		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{

		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton EfButton1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton EfButton2;
		protected easyFramework.Frontend.ASP.WebComponents.efButton EfButton3;
		protected easyFramework.Frontend.ASP.WebComponents.efButton EfButton4;
		protected easyFramework.Frontend.ASP.WebComponents.efDataTable EfDataTable1;
		protected easyFramework.Frontend.ASP.WebComponents.efTabDialog EfTabDialog1;
		protected easyFramework.Frontend.ASP.WebComponents.efTab EfTab1;
		protected easyFramework.Frontend.ASP.WebComponents.efTab EfTab2;
		protected easyFramework.Frontend.ASP.WebComponents.efButton Efbutton5;
		protected easyFramework.Frontend.ASP.WebComponents.efPopupMenueEntity EfPopupMenueEntity1;

		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;

		private void Page_Init (System.Object sender, System.EventArgs e)
		{
			//CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
			//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
			InitializeComponent();
		}

		#endregion

		//================================================================================
		//Private Fields:
		//================================================================================

		private string msEntityName;
		private int mlDialogWidth;
		private int mlDialogHeight;
		private string msImg_filter_search;
		private string msImg_clear;
		private bool mbFilterOnLoad;
		private string msEntityIdOnLoad;
		private string msKeyFieldName;
		private IEntity moEntity;
		private EntityTabSec moEntityTabSec;


		//================================================================================
		//Public Properties:
		//================================================================================
		public IEntity oEntity
		{
			get
			{
				return moEntity;
			}
		}

		//================================================================================
		//Property:  sEntityName
		//--------------------------------------------------------------------------------'
		//Purpose:   the name of the entity-type, e.g. users, usergroups; must be defined
		//           in tsEntities
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 20:48:38
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sEntityName
		{
			get
			{
				return msEntityName;
			}
			set
			{
				msEntityName = value;
			}
		}

		public string sImg_filter_search
		{
			get
			{
				return msImg_filter_search;
			}

		}
		public string sImg_clear
		{
			get
			{
				return msImg_clear;
			}
		}

		public string sKeyFieldName
		{
			get
			{
				return msKeyFieldName;
			}
		}

		//================================================================================
		//Property:  bFilterOnLaod
		//--------------------------------------------------------------------------------'
		//Purpose:   true, if the dialog is loaded with a given entity-id
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 00:50:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bFilterOnLaod
		{
			get
			{
				return mbFilterOnLoad;
			}
		}


		public easyFramework.Sys.Security.EntityTabSec oEntityTabSec
		{
			get
			{
				return moEntityTabSec;
			}
		}

		//================================================================================
		//Property:  sFilter
		//--------------------------------------------------------------------------------'
		//Purpose:   the value of the filter
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 00:51:00
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sEntityIdOnLoad
		{
			get
			{
				return msEntityIdOnLoad;
			}
		}


		//================================================================================
		//Property:  lStartDialogWidth
		//--------------------------------------------------------------------------------'
		//Purpose:   the width of the dialog at startup
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 21:33:39
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int lStartDialogWidth
		{
			get
			{
				return mlDialogWidth;
			}
			set
			{
				mlDialogWidth = value;
			}
		}


		//================================================================================
		//Property:  lStartDialogHeight
		//--------------------------------------------------------------------------------'
		//Purpose:   the height of the dialog at startup
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 21:33:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int lStartDialogHeight
		{
			get
			{
				return mlDialogHeight;
			}
			set
			{
				mlDialogHeight = value;
			}
		}

		public override void CustomInit (XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen

			//get the entity:
			if (Functions.IsEmptyString(Request["Entity"]))
			{
				throw (new efException("entity-type is missing, e.g. ?entity=user"));
			}
			sEntityName = Request["Entity"];


			moEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);


			this.lStartDialogHeight = oEntity.lEditWindowHeight;
			this.lStartDialogWidth = oEntity.lEditWindowWidth;


			//setup datatable:
			this.EfDataTable1.sXmlAddParams = "<entity>" + sEntityName + "</entity>";

			//setup xml-dialog:
			//Me.EfXmlDialog1.sDefinitionFile = oEntity.sEditDialogXmlFile
			//Me.EfXmlDialog1.sDataPage = "entityDialogData.aspx"

			//-------get image-url--------------
			msImg_filter_search = Images.sGetImageURL(oClientInfo, "filter_search", Request.ApplicationPath);
			msImg_clear = Images.sGetImageURL(oClientInfo, "clear", Request.ApplicationPath);


			//---------let's see, if there is an entity to load------------
			if (Request[oEntity.sKeyFieldName] != "")
			{
	
				mbFilterOnLoad = true;
				msEntityIdOnLoad = Request[oEntity.sKeyFieldName];
	
			}

			msKeyFieldName = oEntity.sKeyFieldName;


			//------------load entity into popup --------------
			EfPopupMenueEntity1.gSetEntity(oClientInfo, sEntityName);
			EfPopupMenueEntity1.bWithEdit = false;
			EfPopupMenueEntity1.bWithSearch = false;

			//---------------js, css, title-----------
			sTitle = oEntity.sTitle;
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../js/efTabDialog.js", true, "Javascript");
			gAddScriptLink("../../js/efPopupMenue.js", true, "Javascript");
			gAddScriptLink("../../js/efOnlineHelp.js", true, "Javascript");
			gAddScriptLink("../../js/efIESpecials.js", true, "VBScript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
			gAddCss("../../css/efstyledatatable.css", true);
			gAddCss("../../css/efstyletabdlg.css", true);


			//------------init the security-object-------------
			moEntityTabSec = efEnvironment.goGetEnvironment(Application).goEntityTabSec;

		}

		//================================================================================
		//Private Methods:
		//================================================================================



	}

}
