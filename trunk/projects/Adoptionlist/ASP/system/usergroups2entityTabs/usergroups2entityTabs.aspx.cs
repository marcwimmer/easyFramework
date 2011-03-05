using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     users2menue
	//--------------------------------------------------------------------------------'
	//Module:    users2menue.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   setting which user has access to which menue-item
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 23:18:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class usergroups2entityTabs : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtnSave;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtnAbort;
	
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
	
		protected int lGrp_id;
	
		protected IEntity oUserGroupEntity;
	
		protected Recordset rsTabs;
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{
		
			sTitle = "Tabulator-Reiter zuordnen";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efPopupMenue.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
		
			//-----------get the user-id-------------
			if (Functions.IsEmptyString(oXmlRequest.sGetValue("grp_id", "")))
			{
				throw (new efException("Parameter \"grp_id\" is required!"));
			}
			else
			{
				lGrp_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlRequest.sGetValue("grp_id", ""), 0);
			}
		
			//-------------get tabs-----------------
			rsTabs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityTabs", "tab_id, tab_entity, tab_tabid", "", "", "", "tab_entity, tab_id ASC");
		
		
			//---------load user-group entity-------------------
			oUserGroupEntity = EntityLoader.goLoadEntity(oClientInfo, "UserGroups");
			oUserGroupEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lGrp_id));
		
		
		}
	
	
	
		//================================================================================
		//Function:  gbIsTab_id
		//--------------------------------------------------------------------------------'
		//Purpose:   checks wether the given tab-id is assigned or not
		//           by reading from table tsEntityTabAccessTsUsers
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:35:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbIsTab_id(int lTab_id)
		{
		
			if (DataMethodsClientInfo.glDBCount(oClientInfo, "tsEntityTabAccessTsUsergroups", "*", "etg_tab_id=" + lTab_id + " And etg_grp_id=" + lGrp_id, "") > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
	
	
	
	
	
	
	}

}
