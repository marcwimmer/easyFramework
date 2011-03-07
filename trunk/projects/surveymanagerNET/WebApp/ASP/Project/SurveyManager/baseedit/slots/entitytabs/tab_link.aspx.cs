using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Entities;
using easyFramework.Project.SurveyManager.Preparation;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     tab_slotlink

	//--------------------------------------------------------------------------------'
	//Module:    tab_slotlink.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   returns the link to a slot
	//--------------------------------------------------------------------------------'
	//Created:   10.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================


	public class tab_slotlink : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtnNewPublication;
	
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
	
	
		protected string msSlot_id;
		protected string msHref;
	
		private void Page_Load (XmlDocument oXmlRequest)
		{
		
			string sSlot_id = oXmlRequest.sGetValue("slt_id", "");
		
			if (sSlot_id == "" | sSlot_id == "undefined")
			{
				return;
			}
		
		
			msSlot_id = DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tdSlots", "slt_externalid", "slt_id=" + sSlot_id, "");
		
		
			//-----------get the base-url of the survey---------------
			string sBaseUrl;
		
			sBaseUrl = "http://" + Request["HTTP_HOST"] + Request.ApplicationPath + "/ASP/Project/SurveyManager/backend/slot.aspx?id=";
		
			msHref = sBaseUrl + msSlot_id;
		
		}
	
	
	}

}
