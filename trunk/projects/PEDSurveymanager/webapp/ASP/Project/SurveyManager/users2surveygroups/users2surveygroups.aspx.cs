using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     users2policies
	//--------------------------------------------------------------------------------'
	//Module:    users2policies.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   setting which user has access to which menue-item
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 23:18:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class users2surveygroups : efDialogPage
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
		
		protected XmlDocument oXmlSurveyGroups;
		
		public override void CustomInit (XmlDocument oXmlRequest)
		{
			
			sTitle = "Projektgruppen zuordnen";
			gAddScriptLink("../../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../../js/efPopupMenue.js", true, "Javascript");
			gAddCss("../../../css/efstyledefault.css", true);
			gAddCss("../../../css/efstyledialogtable.css", true);
			
			//-----------get the user-id-------------
			if (oXmlRequest.sGetValue("usr_id", "") == "")
			{
				throw (new efException("Parameter \"usr_id\" is required!"));
			}
			else
			{
				lUsr_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlRequest.sGetValue("usr_id", ""), 0);
			}
			
			//-------------get tabs-----------------
			oXmlSurveyGroups = goXmlGetSurveyGroupItems(oClientInfo);
			
			
			//---------load user-group entity-------------------
			oUserEntity = EntityLoader.goLoadEntity(oClientInfo, "Users");
			oUserEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id));
			
			
		}
		
		
		
		//================================================================================
//Function:  gbIsSvg_id
		//--------------------------------------------------------------------------------'
//Purpose:   checks wether the given svg-id is assigned or not
		//           by reading from table tdSurveyGroupsTsUsers
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   20.05.2004 23:35:45
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbIsSvg_id(int lSvg_id)
		{
			
			Recordset rstdSurveyGroupsTsUsers;
			
			rstdSurveyGroupsTsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveyGroupsTsUsers", "*", "svg_usr_svg_id=" + lSvg_id + " And svg_usr_usr_id=" + lUsr_id, "", "", "");
			
			if (rstdSurveyGroupsTsUsers.EOF)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		
		//================================================================================
//Function:  goXmlGetSurveyGroupItems
		//--------------------------------------------------------------------------------'
//Purpose:   returns the surveygroup-items for the dialog
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   20.05.2004 23:24:58
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static XmlDocument goXmlGetSurveyGroupItems(ClientInfo oClientInfo)
		{
			
			Recordset rsSurveyGroups;
			XmlDocument oXmlResult = new XmlDocument("<surveygroupitems/>");
			
			
			//--------start with top elements-----------
			rsSurveyGroups = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveyGroups", "svg_id, svg_name", "svg_parentid is null", "", "", "svg_name ASC");
			
			while (! rsSurveyGroups.EOF)
			{
				
				mHelpGetSurveyGroupItems(oClientInfo, ref oXmlResult, rsSurveyGroups["svg_id"].lValue, rsSurveyGroups["svg_name"].sValue);
				
				rsSurveyGroups.MoveNext();
			} ;
			
			return oXmlResult;
		}
		
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		
		
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
		protected string msGetSelectCombo(int lSvg_id)
		{
			efHtmlInput oSelect = new efHtmlInput();
			oSelect["type"].sValue = "checkbox";
			oSelect["name"].sValue = "svg_id_" + lSvg_id;
			
			if (gbIsSvg_id(lSvg_id))
			{
				oSelect["checked"].sValue = "True";
			}
			
			FastString oRenderResult = new FastString();
			oSelect.gRender(oRenderResult, 1);
			return oRenderResult.ToString();
			
		}
		
		
		
		
		//================================================================================
//Sub:       mHelpGetSurveyGroupItems
		//--------------------------------------------------------------------------------'
//Purpose:   helper for adding the menue-items
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   20.05.2004 23:31:45
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		private static void mHelpGetSurveyGroupItems (ClientInfo oClientInfo, ref XmlDocument oXmlDoc, int svg_parentid, string svg_parent_name)
		{
			
			//------------ad the parent-item---------------
			XmlNode oNode = oXmlDoc.selectSingleNode("/surveygroupitems").AddNode("item", false);
			
			oNode.AddNode("id", true).sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(svg_parentid);
			oNode.AddNode("title", true).sText = svg_parent_name;
			
			
			//-------step through the children--------------
			Recordset rsChildren = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveyGroups", "svg_id, svg_name", "svg_parentid=" + svg_parentid, "", "", "svg_name ASC");
			
			while (! rsChildren.EOF)
			{
				
				mHelpGetSurveyGroupItems(oClientInfo, ref oXmlDoc, rsChildren["svg_id"].lValue, svg_parent_name + " / " + rsChildren["svg_name"].sValue);
				
				rsChildren.MoveNext();
			} ;
			
		}
	}
	
}
