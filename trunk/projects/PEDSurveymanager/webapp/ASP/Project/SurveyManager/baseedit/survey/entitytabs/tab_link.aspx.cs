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
	//Class:     tab_publications

	//--------------------------------------------------------------------------------'
	//Module:    tab_publications.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   list the publications of a survey an offer to download results;
	//           a new publication may also be created
	//--------------------------------------------------------------------------------'
	//Created:   10.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================


	public class tab_link : efDialogPage
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
	
	
		protected string msSurveyWeb_id;
		protected string msHref;
	
		public override void CustomInit(XmlDocument oXmlRequest)
		{
			base.CustomInit ();
		
			string sSurvey_id = oXmlRequest.sGetValue("svy_id", "");
		
			if (sSurvey_id == "")
			{
				return;
			}
		
		
			string sQry;
		
			sQry = "pub_svy_id=" + sSurvey_id;
		
			msSurveyWeb_id = DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tdSurveys", "svy_externalid", "svy_id=" + sSurvey_id, "");
		
		
			//-----------get the base-url of the survey---------------
			string sBaseUrl;
			sBaseUrl = "http://" + Request["HTTP_HOST"] + Request.ApplicationPath + "/ASP/Project/SurveyManager/backend/survey.aspx?id=";
		
			msHref = sBaseUrl + msSurveyWeb_id;
		
		}
	
	
	}

}
