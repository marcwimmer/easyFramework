using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     usergroups2menue
	//--------------------------------------------------------------------------------'
	//Module:    usergroups2menue.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   setting which usergroup has access to which menue-item
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 23:18:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class usergroups2menue : efDialogPage
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

		protected XmlDocument oXmlMenueItems;

		public override void CustomInit (XmlDocument oXmlRequest)
		{
	
			sTitle = "Menüeintrage zuordnen";
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
	
			//-------------get user-groups-----------------
			oXmlMenueItems = goXmlGetMenueItems(oClientInfo);
	
	
			//---------load user-group entity-------------------
			oUserGroupEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "Usergroups");
			oUserGroupEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lGrp_id));
	
	
		}



		//================================================================================
		//Function:  gbIsMnu_id
		//--------------------------------------------------------------------------------'
		//Purpose:   checks wether the given menue-id is assigned or not
		//           by reading from table tsMenuAccessTsUserGroups
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:35:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbIsMnu_id(string sMnu_id)
		{
	
			if (DataMethodsClientInfo.glDBCount(oClientInfo, "tsMenuAccessTsUserGroups", "*", "msg_mnu_id='" + DataTools.SQLString(sMnu_id) + "' And msg_grp_id=" + lGrp_id, "") > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
	
		}



		//================================================================================
		//Function:  moXmlGetMenueItems
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the menue-items for the dialog
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:24:58
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static XmlDocument goXmlGetMenueItems(ClientInfo oClientInfo)
		{
	
			Recordset rsMenueItems;
			XmlDocument oXmlResult = new XmlDocument("<menueitems/>");
	
	
			//--------start with top elements-----------
			rsMenueItems = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsMainMenue", "mnu_id, mnu_title", "mnu_parentid is null", "", "", "mnu_index ASC");
	
			do
			{
		
				mHelpGetMenueItems(oClientInfo, 
					ref oXmlResult, rsMenueItems["mnu_id"].sValue, rsMenueItems["mnu_title"].sValue);
		
				rsMenueItems.MoveNext();
			} while (! rsMenueItems.EOF);
	
			return oXmlResult;
		}

		//================================================================================
		//Private Methods:
		//================================================================================


		//================================================================================
		//Sub:       mHelpGetMenueItems
		//--------------------------------------------------------------------------------'
		//Purpose:   helper for adding the menue-items
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:31:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static void mHelpGetMenueItems (ClientInfo oClientInfo, ref XmlDocument oXmlDoc, string mnu_parentid, string mnu_parent_title)
		{
	
			//------------ad the parent-item---------------
			XmlNode oNode = oXmlDoc.selectSingleNode("/menueitems").AddNode("item", false);
	
			oNode.AddNode("id", true).sText = mnu_parentid;
			oNode.AddNode("title", true).sText = mnu_parent_title;
	
	
			//-------step through the children--------------
			Recordset rsChildren = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsMainMenue", "mnu_id, mnu_title", "mnu_parentid='" + DataTools.SQLString(mnu_parentid) + "'", "", "", "mnu_index ASC");
	
			while (! rsChildren.EOF)
			{
		
				mHelpGetMenueItems(oClientInfo, ref oXmlDoc, rsChildren["mnu_id"].sValue, mnu_parent_title + " / " + rsChildren["mnu_title"].sValue);
		
				rsChildren.MoveNext();
			} ;
	
		}

	}

}
