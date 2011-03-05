using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.WebComponents;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     surveyedit

	//--------------------------------------------------------------------------------'
	//Module:    surveyedit.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   editing the complete survey
	//--------------------------------------------------------------------------------'
	//Created:   22.04.2004 18:39:07
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class surveyedit : efDialogPage
	{
	
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
		protected easyFramework.Frontend.ASP.ComplexObjects.efMultiStructure EfMultiStructure1;
		protected easyFramework.Frontend.ASP.WebComponents.efTreeview efMenuTree_Questions;
		protected easyFramework.Frontend.ASP.WebComponents.efTreeview efMenuTree_Answers;
		protected easyFramework.Frontend.ASP.WebComponents.efTreeview efMenuTree_AnswerValues;
		
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
	
		public string svy_id = "";

		public override void CustomInit (XmlDocument oRequest)
		{
			
			sTitle = "Survey bearbeiten";
			gAddScriptLink("../../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../../js/efMultiStructure.js", true, "Javascript");
			gAddScriptLink("../../../js/efPopupMenue.js", true, "Javascript");
			gAddScriptLink("../../../js/efOptionDialog.js", true, "Javascript");
			gAddScriptLink("../../../js/efOnlineHelp.js", true, "Javascript");
			gAddScriptLink("../../../js/efIESpecials.js", true, "VBScript");
			gAddCss("../../../css/efstyledefault.css", true);
			gAddCss("../../../css/efstyledialogtable.css", true);
			gAddCss("../../../css/efstyledatatable.css", true);
			gAddCss("styles.css", false);
		
			svy_id = oRequest.sGetValue("svy_id");
		

			efMenuTree_Questions.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath);
			efMenuTree_Questions.sDataPage = "surveyEditMenuTreeData_Questions.aspx";
			efMenuTree_Questions.sServerParams = "<svy_id>" + svy_id + "</svy_id>";
			efMenuTree_Answers.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath);
			efMenuTree_Answers.sDataPage = "surveyEditMenuTreeData_Answers.aspx";
			efMenuTree_AnswerValues.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath);
			efMenuTree_AnswerValues.sDataPage = "surveyEditMenuTreeData_AnswerValues.aspx";


			//---------------set svy-id as topmost-value for the survey-edit--------------
			if (Functions.IsEmptyString(Request["svy_id"]))
			{
				throw (new efException("The svy-id is mandatory!"));
			}
			
		
		
		}
	
	
	
	
	}

}
