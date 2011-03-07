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
	//Class:     slot

	//--------------------------------------------------------------------------------'
	//Module:    slot.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   redirects the call to the current survey of the slot
	//--------------------------------------------------------------------------------'
	//Created:   25.05.2004 22:54:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class slot : easyFramework.Frontend.ASP.Dialog.efDialogPage
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
	
		protected string msErrors;
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{

			//----------get conn-strings----------
			string sConnstr_Svm;
			string sConnstr_Results;
			sConnstr_Svm = efEnvironment.goGetEnvironment(Application).gsConnStr;
            /* //TODO reactivate
			sConnstr_Results = easyFramework.
				Project.SurveyManager.
				Submission.DBConn.gsGetConnStr_ResultDatabase(sConnstr_Svm);
		*/
		
			//----------get url-parameters----------
			int lSlt_id;
			string sExternalId;
			if (Functions.IsEmptyString(Request["id"]))
			{
				msErrors = "Slot-ID wird bentigt!";
				return;
			}
			sExternalId = Request["id"];
		
		
		
			//-------------get recordset-slot------------
			Recordset rsSlot = DataMethods.gRsGetTable(sConnstr_Svm, "tdSlots", "*", 
				"slt_externalid='" + sExternalId + "'", "", "", "");
			if (rsSlot.EOF)
			{
				msErrors = "Ungltige Slot-ID!";
				return;
			}
			else
			{
				lSlt_id = rsSlot["slt_id"].lValue;
			}
		
		
			//-------------get survey------------
			Recordset rsSurvey = DataMethods.gRsGetTable(sConnstr_Svm, "tdSurveys", "*", 
				"svy_id=" + rsSlot["slt_assigned_svy_id"].sValue, "", "", "");
			if (rsSurvey.EOF)
			{
				msErrors = "Kein Projekt zugewiesen!";
				return;
			}
		
			//--------redirect to survey-----------
			Response.Redirect("survey.aspx?id=" + rsSurvey["svy_externalid"].sValue, true);
		}
	
	}

}
