using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Entities;
using easyFramework.Project.SurveyManager;
using easyFramework.Project.SurveyManager.Preparation;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     tab_publicationsProcess

	//--------------------------------------------------------------------------------'
	//Module:    tab_publicationsProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   creates a new publications or sets the state of a current
	//           publication
	//--------------------------------------------------------------------------------'
	//Created:   10.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class tab_publicationsProcess : efProcessPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
	
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
	
	
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
		
			Publications oPublications = new Publications();
		
			string svy_id = oRequest.sGetValue("svy_id", "");
		
			if (svy_id == "")
			{
				throw (new efException("svy_id required!"));
			}
		
			Publications.gNewPublication(oClientInfo, DataConversion.glCInt(svy_id));
		
		
			if (oClientInfo.bHasErrors)
			{
				return oClientInfo.gsErrors();
			}
			else
			{
				return "SUCCESS";
			}
		
		
		}
	
	}

}
