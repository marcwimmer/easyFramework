using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Frontend.ASP;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Security;
using easyFramework.Sys.Entities;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     newsurveyProcess

	//--------------------------------------------------------------------------------'
	//Module:    newsurveyProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   creates a new survey either depending on a template-survey
	//           or from scratch
	//--------------------------------------------------------------------------------'
	//Created:   02.05.2004 10:58:41
	//--------------------------------------------------------------------------------'
	//Changed:


	//================================================================================
	//Imports:
	//================================================================================


	//--------------------------------------------------------------------------------'
	public class newsurveyProcess : efProcessPage
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
		
			
			int lTemplateSurveyID;
			string svy_name = "test";
			int svy_svg_id;
			string svy_svt_id;
		
			Recordset rsDlgData = Tools.grsDialogOutput2Recordset(oRequest.sXml);
		
		
			lTemplateSurveyID = rsDlgData["template_svy_id"].lValue;
			svy_name = rsDlgData["svy_name"].sValue;
			svy_svg_id = rsDlgData["svy_svg_id"].lValue;
			svy_svt_id = rsDlgData["svy_svt_id"].sValue;
		
			try
			{
				int lResult = easyFramework.Project.SurveyManager.Preparation.
					SurveyEditor.glCreateSurvey(oClientInfo, lTemplateSurveyID, 
					Request, Application, svy_svg_id, svy_name, svy_svt_id);
			
				return "SUCCESS_" + easyFramework.Sys.ToolLib.DataConversion.
					gsCStr(lResult);
			
			}
			catch (efException ex)
			{
			
				return ex.Message;
			
			}
		
		
		}
	
	}

}
