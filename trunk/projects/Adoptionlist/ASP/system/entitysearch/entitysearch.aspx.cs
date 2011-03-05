using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.SqlTools;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     entitySearch

	//--------------------------------------------------------------------------------'
	//Module:    entitySearch.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   searches entity and returns the id
	//--------------------------------------------------------------------------------'
	//Created:   05.04.2004 20:46:27
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class entitySearch : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efDataTable EfDataTable1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtn_Apply;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtn_Abort;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtn_Search;
	
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
	
		//================================================================================
		//Public Properties:
		//================================================================================
	
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
	
		//================================================================================
		//Private Methods:
		//================================================================================
		public override void CustomInit (XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			//get the entity:
			if (Functions.IsEmptyString(Request["Entity"]))
			{
				throw (new efException("entity-type is missing, e.g. ?entity=user"));
			}
			sEntityName = Request["Entity"];
		
			Recordset rsEntity = DataMethodsClientInfo.gRsGetTable(this.oClientInfo, "tsEntities", "*", "ety_name='" + DataTools.SQLString(sEntityName) + "'", "", "", "");
		
			if (rsEntity.EOF)
			{
				throw (new efException("Entity \"" + sEntityName + "\" wasn't found!"));
			}
		
		
			//setup datatable:
			this.EfDataTable1.sXmlAddParams = "<entity>" + sEntityName + "</entity>";
		
		
		
			//-------get image-url--------------
			msImg_filter_search = Images.sGetImageURL(oClientInfo, "filter_search", Request.ApplicationPath);
			msImg_clear = Images.sGetImageURL(oClientInfo, "clear", Request.ApplicationPath);
		
		
		
			//-------------js, css, title----------------
			sTitle = rsEntity["ety_title"].sValue;
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../js/efTabDialog.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
			gAddCss("../../css/efstyledatatable.css", true);
			gAddCss("../../css/efstyletabdlg.css", true);
		
		
		
		
		}
	
	
	
	}

}
