using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
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

	public class users2menue : efDialogPage
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
	
		protected int lUsr_id;
	
		protected IEntity oUserEntity;
	
		protected XmlDocument oXmlMenueItems;
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{
		
			sTitle = "Meneintrage zuordnen";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efPopupMenue.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
		
			//-----------get the user-id-------------
			if (Functions.IsEmptyString(oXmlRequest.sGetValue("usr_id", "")))
			{
				throw (new efException("Parameter \"usr_id\" is required!"));
			}
			else
			{
				lUsr_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlRequest.sGetValue("usr_id", ""), 0);
			}
		
			//-------------get user-groups-----------------
			oXmlMenueItems = goXmlGetMenueItems(oClientInfo);
		
		
			//---------load user-group entity-------------------
			oUserEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "Users");
			oUserEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id));
		
		
		}
	
	
	
		//================================================================================
		//Function:  gbIsMnu_id
		//--------------------------------------------------------------------------------'
		//Purpose:   checks wether the given menue-id is assigned or not
		//           by reading from table tsMenuAccessTsUserGroups
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   2 - like group
		//           1 - access
		//           0 - no access
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:35:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int glIsMnu_id(string sMnu_id)
		{
		
			Recordset rstsMenuAccessTsUsers;
		
			rstsMenuAccessTsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsMenuAccessTsUsers", "*", "msu_mnu_id='" + DataTools.SQLString(sMnu_id) + "' And msu_usr_id=" + lUsr_id, "", "", "");
		
			if (rstsMenuAccessTsUsers.EOF)
			{
				return 2;
			}
			else
			{
				if (rstsMenuAccessTsUsers["msu_explicit_access"].bValue == true)
				{
					return 1;
				}
				else
				{
					return 0;
				}
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
			
				mHelpGetMenueItems(oClientInfo, ref oXmlResult, rsMenueItems["mnu_id"].sValue, rsMenueItems["mnu_title"].sValue);
			
				rsMenueItems.MoveNext();
			} while (! rsMenueItems.EOF);
		
			return oXmlResult;
		}
	
		//================================================================================
		//Protected Methods:
		//================================================================================
	
		//================================================================================
		//Function:  msGetGroupAccess
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the access of the users-groups
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.05.2004 13:59:27
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msGetGroupAccess(string sMnu_id)
		{
		
		
			if (efEnvironment.goGetEnvironment(Application).goMenuTreeSec.gbHasAnyUserGroupAccessFromDB(oClientInfo, sMnu_id, lUsr_id))
			{
				return "Zugriff";
			}
			else
			{
				return "kein Zugriff";
			}
		
		}
	
		//================================================================================
		//Function:  msGetSelectCombo
		//--------------------------------------------------------------------------------'
		//Purpose:   gets the select-input-element for assigning the user-rights;
		//           the correct-value is already chosen
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.05.2004 13:45:59
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msGetSelectCombo(string sMnu_id)
		{
			ComboBox oSelect = new ComboBox();
			oSelect.gAddEntry("2", "wie Gruppe");
			oSelect.gAddEntry("1", "Zugriff");
			oSelect.gAddEntry("0", "kein Zugriff");
		
			oSelect.sSelectedValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(glIsMnu_id(sMnu_id));
		
			oSelect.sName = "mnu_id_" + sMnu_id;
		
			return oSelect.gRender();
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
			Recordset rsChildren = DataMethodsClientInfo.gRsGetTable(oClientInfo, 
				"tsMainMenue", "mnu_id, mnu_title", "mnu_parentid='" + DataTools.SQLString(mnu_parentid) + "'", 
				"", "", "mnu_index ASC");
		
			while (! rsChildren.EOF)
			{
			
				mHelpGetMenueItems(oClientInfo, ref oXmlDoc, 
					rsChildren["mnu_id"].sValue, 
					mnu_parent_title + " / " + rsChildren["mnu_title"].sValue);
			
				rsChildren.MoveNext();
			} 
		
		}
	
	}

}
