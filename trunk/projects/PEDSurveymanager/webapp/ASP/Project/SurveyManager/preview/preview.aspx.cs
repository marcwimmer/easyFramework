using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Project.SurveyManager;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Data;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     preview

	//--------------------------------------------------------------------------------'
	//Module:    preview.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   for previewing a survey
	//--------------------------------------------------------------------------------'
	//Created:   07.05.2004 13:58:26
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class preview : efDialogPage
	{
	
	
		//================================================================================
		//Private Fields:
		//================================================================================
		private string msSurveyHtml;
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton EfButton1;
	
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
		//Public Properties:
		//================================================================================
	
		public string sSurveyHtml
		{
			get
			{
				return msSurveyHtml;
			}
		}
	
	
		//================================================================================
		//Private Methods:
		//================================================================================
	
		public override void CustomInit (XmlDocument oRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			int lsvy_id;
		
			lsvy_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("svy_id", ""), 0);
		
			if (lsvy_id == 0)
			{
				throw (new efException("Parameter \"svy_id\" required!"));
			}
		
		
			SurveyRenderer oSurveyRenderer = new SurveyRenderer(lsvy_id);
		
			msSurveyHtml = oSurveyRenderer.gRender(oClientInfo.sConnStr, true, this.Application);
		
		
			//-----------------css-link of survey or surveygroup------------
			Recordset rsSurvey = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveys", "svy_css, svy_svg_id", "svy_id=" + lsvy_id, "", "", "");
			Recordset rsSurveyGroup = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveyGroups", "svg_css", "svg_id=" + rsSurvey["svy_svg_id"].sValue, "", "", "");
			string sCss;
			if (rsSurvey["svy_css"].sValue == "")
			{
				sCss = rsSurveyGroup["svg_css"].sValue;
			}
			else
			{
				sCss = rsSurvey["svy_css"].sValue;
			}
		
			gAddCss(sCss, false);
		
		
			//----------------------jscripts and title----------------
			sTitle = "Preview";
			gAddScriptLink("../../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../../js/efMultiStructure.js", true, "Javascript");
		
		}
	
	}

}
