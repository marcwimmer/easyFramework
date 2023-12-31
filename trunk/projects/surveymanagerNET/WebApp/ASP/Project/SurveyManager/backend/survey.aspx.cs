using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Project.SurveyManager;
using easyFramework.Sys;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     survey

	//--------------------------------------------------------------------------------'
	//Module:    survey.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   displaying the content of a survey
	//--------------------------------------------------------------------------------'
	//Created:   12.05.2004 09:26:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================


	public class survey : System.Web.UI.Page
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
	
		protected int lSvy_id;
	
		protected string sSurveyHtml;
	
		protected string sCssOfSurvey;
	
		protected string sSurveyTitle;
	
		protected string sSurveyType;
	
		protected string sBorderHtmlBegin;
		protected string sBorderHtmlEnd;
	
		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			if (Request["id"] == "")
			{
				throw (new efException("ID required!"));
			}
			string sConnstr_Svm;
			string sConnstr_Results;
		
		
			sConnstr_Svm = efEnvironment.goGetEnvironment(Application).gsConnStr;
			sConnstr_Results = Submission.DBConn.gsGetConnStr_ResultDatabase(sConnstr_Svm);
		
		
		
			Rendering.SurveyRenderer oSurveyRenderer = new Rendering.SurveyRenderer(sConnstr_Svm, easyFramework.Sys.ToolLib.DataConversion.gsCStr(Request["id"]));
			sSurveyHtml = oSurveyRenderer.gRender(sConnstr_Svm, false, this.Application);
		
		
		
		
			//------------------------get css------------------------
			Recordset rsSurvey;
			Recordset rsSurveyGroup;
			rsSurvey = DataMethods.gRsGetTable(sConnstr_Svm, "tdSurveys", "svy_svt_id, svy_id, svy_name, svy_css, svy_svg_id", "svy_externalid='" + DataTools.SQLString(Request["id"]) + "'", "", "", "");
			if (! rsSurvey.EOF)
			{
				rsSurveyGroup = DataMethods.gRsGetTable(sConnstr_Svm, "tdSurveyGroups", "svg_css", "svg_id=" + rsSurvey["svy_svg_id"].sValue, "", "", "");
			}
			lSvy_id = rsSurvey["svy_id"].lValue;
		
			//---------get css------------
			sCssOfSurvey = Preparation.SurveyEditor.gsGetSurveyValue(sConnstr_Svm, "svy_css", "svg_css", lSvy_id, -1);
		
			//--------------set the survey-type-----------
			sSurveyType = rsSurvey["svy_svt_id"].sValue;
		
			//------------------survey title----------------------
			sSurveyTitle = rsSurvey["svy_name"].sValue;
		
			//----------------get border-html -----------------------
			sBorderHtmlBegin = Preparation.SurveyEditor.gsGetSurveyValue(sConnstr_Svm, "svy_border_html_begin", "svg_border_html_begin", lSvy_id, -1);
			sBorderHtmlEnd = Preparation.SurveyEditor.gsGetSurveyValue(sConnstr_Svm, "svy_border_html_end", "svg_border_html_end", lSvy_id, -1);
		
			//---------replace placeholders in border-html---------------
			sBorderHtmlBegin = Rendering.HTMLPlaceHolders.gsGetReplacedHtml(sConnstr_Svm, lSvy_id, sBorderHtmlBegin);
			sBorderHtmlEnd = Rendering.HTMLPlaceHolders.gsGetReplacedHtml(sConnstr_Svm, lSvy_id, sBorderHtmlEnd);
		
		}
	
	
	}

}
