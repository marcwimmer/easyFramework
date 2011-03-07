using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     handleUpdatePublication

	//--------------------------------------------------------------------------------'
	//Module:    handleUpdatePublication.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   fired on update of a survey; some rules are
	//           defined here, like in votings shall only be choice-questions;
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 22:53:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class checkQuestionsValidity
	{
	
		public static bool gCheckAnswerForSurvey(ClientInfo oClientInfo, string ans_aty_id, string svy_svt_id)
		{
		
		
			if (Functions.UCase(svy_svt_id) == "VOTE" & Functions.UCase(ans_aty_id) != "SINGLECHOICE" & Functions.UCase(ans_aty_id) != "MULTIPLECHOICE")
			{
			
				oClientInfo.gAddError("Abstimmungen dürfen nur Single/Multipl-choice antworten enthalten!");
			
				return false;
			
			}
		
			if (Functions.UCase(svy_svt_id) == "FREETEXT" & Functions.UCase(svy_svt_id) == "TEST" & Functions.UCase(ans_aty_id) != "GAPTEXT" & Functions.UCase(ans_aty_id) != "SINGLECHOICE" & Functions.UCase(ans_aty_id) != "MULTIPLECHOICE")
			{
			
				oClientInfo.gAddError("Tests dürfen nur Freetext/Gaptext/Single/Multipl-choice antworten enthalten!");
			
				return false;
			
			}		
		
			return true;
		
		}
	
	}


	public class surveyRules : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{
		
			IEntity oSurveyEntity = ((IEntity)(oParam));
		
			//--------if there are more than 1 question in a vote, then error-------
			if (DataMethodsClientInfo.glDBCount(oClientInfo, "tdQuestions", "*", "qst_svy_id=" + oSurveyEntity.oFields["svy_id"].sValue, "") > 1 & oSurveyEntity.oFields["svy_svt_id"].sValue == "VOTE")
			{
				bRollback = true;
				oClientInfo.gAddError("Abstimmungen drfen nicht mehr als eine Frage haben!");
			}
		
		
		
		}
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
			IEntity oSurveyEntity = ((IEntity)(oParam));
		
			if (!Functions.IsEmptyString(oSurveyEntity.oKeyField.sValue))
			{
			
				Recordset rsAnswers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswers", "ans_aty_id", "ans_qst_id in (SELECT qst_id FROM tdQuestions WHERE qst_svy_id=" + oSurveyEntity.oFields["svy_id"].sValue + ")", "", "", "");
			
				while (! rsAnswers.EOF)
				{
				
					if (! checkQuestionsValidity.gCheckAnswerForSurvey(oClientInfo, 
						rsAnswers["ans_aty_id"].sValue, 
						oSurveyEntity.oFields["svy_svt_id"].sValue))
					{
						Cancel = true;
					}
					else
					{
						Cancel = false;
					}
				
					rsAnswers.MoveNext();
				}
			
			}
		}
	}


	public class answerRules : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{

			IEntity oAnswerEntity = ((IEntity)(oParam));
			
			//---------------------durchnummerieren der Fragen-----------------------------
			//gewünschten Wert übernehmen, damit Frage dort ist:
			DataMethodsClientInfo.gUpdateTable(oClientInfo, "tdAnswers", "ans_index=" + Convert.ToString(oAnswerEntity.oFields["ans_index"].lValue - 1), "ans_id=" + oAnswerEntity.oFields["ans_id"].lValue);

			Recordset rsSiblings = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswers", "ans_id, ans_index", "ans_qst_id=" + oAnswerEntity.oFields["ans_qst_id"].sValue, "", "", "ans_index");
			int Counter = 0;
			while (!rsSiblings.EOF) 
			{	
				DataMethodsClientInfo.gUpdateTable(oClientInfo, "tdAnswers", "ans_index=" + Counter, "ans_id=" + rsSiblings["ans_id"].lValue);
				if (oAnswerEntity.oFields["ans_id"].lValue == rsSiblings["ans_id"].lValue)
					oAnswerEntity.oFields["ans_index"].lValue = Counter;
				rsSiblings.MoveNext();
				Counter++;
			}
		}
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
			IEntity oAnswerEntity = ((IEntity)(oParam));
			Recordset rsQuestion = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdQuestions", "*", "qst_id=" + oAnswerEntity.oFields["ans_qst_id"].sValue, "", "", "");
		
			//-------------wenn das Datenbankfeld noch "BITTE_BEARBEITEN" heisst, dann den Benutzer darauf aufmerksam machen-----------
			/*if (oAnswerEntity.oFields["ans_resultDbField"].sValue.Equals("BITTE_BEARBEITEN"))
			{
				oClientInfo.gAddError("Bitte geben Sie ein eindeutigen Namen für das Datenbankfeld ein.");
				Cancel = false;
				return;
			}
			*/

			//-------------wenn ein Datenbankfeldname mehrfach vergeben ist, dann meckern-----------
			string sql = "ans_resultDbField='$1' AND ans_qst_id IN (SELECT qst_id FROM tdQuestions WHERE qst_svy_id =$2) AND ans_id <> $3";
			sql = sql.Replace("$1", oAnswerEntity.oFields["ans_resultDbField"].sValue.Replace("'", "''"));
			sql = sql.Replace("$2", rsQuestion["qst_svy_id"].sValue);
			if (oAnswerEntity.oFields["ans_id"].sValue.Length > 0 )
				sql = sql.Replace("$3", oAnswerEntity.oFields["ans_id"].sValue);
			else 
				sql = sql.Replace("$3", "-1");
			Recordset rsDoppelteFelder = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswers INNER JOIN tdQuestions ON qst_id = ans_qst_id", "ans_resultDbField, qst_text", sql, "", "", "");
			if (rsDoppelteFelder.lRecordcount > 0) 
			{
				string sResultDbName = oAnswerEntity.oFields["ans_resultDbField"].sValue;
				string sQstText = rsDoppelteFelder["qst_text"].sValue;
				oClientInfo.gAddError("Die Datenbankfeldnamen müssen eindeutig sein. Der Name \"" + sResultDbName + "\" ist bereichts vergeben an die Frage: \"" + sQstText + "\".");
				Cancel = true;
				return;
			}
		
			//-------------if not saved yet, then exit -----------------
			if (Functions.IsEmptyString(oAnswerEntity.oFields["ans_id"].sValue))
			{
				return;
			}
			if (Functions.IsEmptyString(oAnswerEntity.oFields["ans_qst_id"].sValue))
			{
				return;
			}
		
			//-------check question------
			
			Recordset rsSurvey = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveys", "svy_svt_id", "svy_id=" + rsQuestion["qst_svy_id"].sValue, "", "", "");
		
		
			if (! checkQuestionsValidity.gCheckAnswerForSurvey(oClientInfo, oAnswerEntity.oFields["ans_aty_id"].sValue, rsSurvey["svy_svt_id"].sValue))
			{
				Cancel = true;
			}
			else
			{
				Cancel = false;
			}
		
		}
	
	
	}


	public class questionRules : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{
		
			IEntity oQuestionEntity = ((IEntity)(oParam));
		
			Recordset rsOtherQuestions = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdQuestions", "*", "qst_svy_id=" + oQuestionEntity.oFields["qst_svy_id"].sValue, "", "", "");
		
			Recordset rsSurvey = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveys", "svy_svt_id", "svy_id=" + oQuestionEntity.oFields["qst_svy_id"].sValue, "", "", "");
		
			//--------if there are more than 1 question in a vote, then error-------
			if (rsOtherQuestions.lRecordcount > 1 & rsSurvey["svy_svt_id"].sValue == "VOTE")
			{
				bRollback = true;
				oClientInfo.gAddError("Abstimmungen dürfen maximal eine Frage haben.");
			}

			
		
			//---------------------durchnummerieren der Fragen-----------------------------
			//gewünschten Wert übernehmen, damit Frage dort ist:
			DataMethodsClientInfo.gUpdateTable(oClientInfo, "tdQuestions", "qst_index=" + Convert.ToString(oQuestionEntity.oFields["qst_index"].lValue - 1), "qst_id=" + oQuestionEntity.oFields["qst_id"].lValue);

			Recordset rsSiblings = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdQuestions", "qst_id, qst_index", "qst_svy_id=" + oQuestionEntity.oFields["qst_svy_id"].sValue, "", "", "qst_index");
			int Counter = 0;
			while (!rsSiblings.EOF) 
			{	
				DataMethodsClientInfo.gUpdateTable(oClientInfo, "tdQuestions", "qst_index=" + Counter, "qst_id=" + rsSiblings["qst_id"].lValue);
				if (oQuestionEntity.oFields["qst_id"].lValue == rsSiblings["qst_id"].lValue)
					oQuestionEntity.oFields["qst_index"].lValue = Counter;
				rsSiblings.MoveNext();
				Counter++;
			}
		
		
		}
	
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
		
		
		
		}
	
	
	}


	public class answerValueRules : easyFramework.Sys.SysEvents.Interfaces.ISysEvents
	{
	
		public void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool bRollback)
		{

			IEntity oAnswerValueEntity = ((IEntity)(oParam));
			
			//---------------------durchnummerieren der Fragen-----------------------------
			//gewünschten Wert übernehmen, damit Frage dort ist:
			DataMethodsClientInfo.gUpdateTable(oClientInfo, "tdAnswerValues", "val_index=" + Convert.ToString(oAnswerValueEntity.oFields["val_index"].lValue - 1), "val_id=" + oAnswerValueEntity.oFields["val_id"].lValue);
				
			Recordset rsSiblings = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswerValues", "val_id, val_for_db, val_ans_id, val_index", "val_ans_id=" + oAnswerValueEntity.oFields["val_ans_id"].sValue, "", "", "val_index");
			int Counter = 0;
			while (!rsSiblings.EOF) 
			{	
				DataMethodsClientInfo.gUpdateTable(oClientInfo, "tdAnswerValues", "val_index=" + Counter, "val_id=" + rsSiblings["val_id"].lValue);
				if (oAnswerValueEntity.oFields["val_id"].lValue == rsSiblings["val_id"].lValue)
					oAnswerValueEntity.oFields["val_index"].lValue = Counter;
				rsSiblings.MoveNext();
				Counter++;
			}

			//----------prüfen, dass val-for-db für die Antwort eindeutig ist-------------------
			if (oAnswerValueEntity.oFields["val_for_db"].sValue.Length > 0) 
			{
				string sSql = "val_ans_id=$1 AND val_for_db='$2' AND val_id <> $3";
				sSql = Functions.Replace(sSql, "$1", oAnswerValueEntity.oFields["val_ans_id"].sValue);
				sSql = Functions.Replace(sSql, "$2", Functions.Replace(oAnswerValueEntity.oFields["val_for_db"].sValue, "'", "''"));
				sSql = Functions.Replace(sSql, "$3", oAnswerValueEntity.oFields["val_id"].sValue);
				Recordset rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswerValues", "val_id", sSql, "", "", "");
				if (!rs.EOF) 
				{
					oClientInfo.gAddError("Der interne Antwortwert \"" + oAnswerValueEntity.oFields["val_for_db"].sValue + "\" ist nicht eindeutig.");
				}
			}

		}
	
		public void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturnObject, ref bool Cancel)
		{
		
			IEntity oAnswerValueEntity = ((IEntity)(oParam));

		}

	}
}
