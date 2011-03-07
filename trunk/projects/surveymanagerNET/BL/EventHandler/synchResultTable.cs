using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     handleUpdatePublication

	//--------------------------------------------------------------------------------'
	//Module:    handleUpdatePublication.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   fired on update of a survey and a publication;
	//           synchronizes the result-table
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 22:53:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class synchResultTable_PublicationUpdate : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
	
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{
		
			IEntity oEntity_Publications = ((IEntity)(oParam));
			Preparation.ResultTableHandler.gSynchronizeResultTable(oClientInfo, oEntity_Publications.oFields["pub_id"].lValue);
		
			if (!Functions.IsEmptyString(oClientInfo.gsErrors()))
			{
				bRollback = true;
			}
		
		}
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
		}
	
	}



	public class synchResultTable_SurveyUpdate : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{
		
			IEntity oEntity_Surveys = ((IEntity)(oParam));
		
			Recordset rsActivePublication = Preparation.Publications.goCurrentActivePublication(oClientInfo.sConnStr, oEntity_Surveys.oFields["svy_id"].lValue);
		
			if (rsActivePublication.EOF == false)
			{
				Preparation.ResultTableHandler.gSynchronizeResultTable(oClientInfo, rsActivePublication["pub_id"].lValue);
			}
		
		}
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
		}
	
	}


	public class synchResultTable_AnswerInsertUpdate : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{
		
			IEntity oEntity_Answer = ((IEntity)(oParam));
		
			//get the answers survey:
			string sSvy_id = DataMethodsClientInfo.gsDBValue(oClientInfo, "SELECT qst_svy_id FROM tdQuestions WHERE qst_id=" + "(SELECT ans_qst_id FROM tdAnswers WHERE ans_id=" + oEntity_Answer.oKeyField.sValue + ")", "", "");
		
		
		
			Recordset rsActivePublication = Preparation.Publications.goCurrentActivePublication(oClientInfo.sConnStr, easyFramework.Sys.ToolLib.DataConversion.glCInt(sSvy_id, 0));
		
			if (rsActivePublication.EOF == false)
			{
				Preparation.ResultTableHandler.gSynchronizeResultTable(oClientInfo, rsActivePublication["pub_id"].lValue);
			}
		
		}
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
		}
	
	}
}
