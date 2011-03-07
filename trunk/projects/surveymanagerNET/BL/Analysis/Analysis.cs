using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Project.SurveyManager;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     Analysis

	//--------------------------------------------------------------------------------'
	//Module:    Analysis.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:
	//--------------------------------------------------------------------------------'
	//Created:   12.05.2004 00:21:22
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class Analysis
	{
	
	
	
		//================================================================================
		//Function:  gRsGetSurveyResultTable
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the content of a survey result-table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   data.recordset
		//--------------------------------------------------------------------------------'
		//Created:   12.05.2004 00:22:25
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static Recordset gRsGetSurveyResultTable(ClientInfo oClientInfo, int lpub_id)
		{
		
		
			string sResultTableName = Preparation.ResultTableHandler.gsGetResultTableName(lpub_id);
		
			string sConnStr = Config.gsConnstrResultDB(oClientInfo);
		
		
			return DataMethods.gRsGetTable(sConnStr, sResultTableName, "*", "", "", "", "SessionID");
		
		
		}
	
	
		//================================================================================
		//Function:  goXmlGetCulmulativeResults
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the results of a culmulative table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   17.05.2004 11:51:10
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static XmlDocument goXmlGetCulmulativeResults(string sConnstr_Svm, string sConnstr_ResultDB, int lPub_id)
		{
		
		
			//---------get records-------------
			string sResultTableName = Preparation.ResultTableHandler.gsGetResultTableNameCulmulated(lPub_id);
		
			Recordset rsResult = DataMethods.gRsGetTable(sConnstr_ResultDB, sResultTableName, "*", "", "", "", "");
		
			int lSumCounter = DataMethods.glGetDBValue(sConnstr_ResultDB, sResultTableName, "SUM(Counter) AS sumCounter", "1=1", 0);
		
		
			//--------make result xml------
			XmlDocument oXmlDoc = new XmlDocument("<culmulativeresults/>");
		
			while (! rsResult.EOF)
			{
			
				XmlNode oNode = oXmlDoc.selectSingleNode("culmulativeresults").AddNode("valresult", false);
			
				oNode.AddNode("text", true).sText = rsResult["val_text"].sValue;
				oNode.AddNode("counter", true).sText = rsResult["counter"].sValue;
				oNode.AddNode("spercent", true).sText = 
					Functions.FormatPercent(
						DataConversion.gdCDec(
							rsResult["counter"].lValue / lSumCounter, 0), 
						2, 
						Functions.efEnumTriState.useDefault, 
						Functions.efEnumTriState.useDefault, 
						Functions.efEnumTriState.useDefault);
				oNode.AddNode("lpercent", true).sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(easyFramework.Sys.ToolLib.DataConversion.glCInt(100 * rsResult["counter"].lValue / lSumCounter, 0));
			
			
				rsResult.MoveNext();
			} 
		
			//-----------add the sum-------------
			oXmlDoc.selectSingleNode("culmulativeresults")["sumCounter"].sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(lSumCounter);
		
			//-----------add the question-text again-------------
			oXmlDoc.selectSingleNode("/culmulativeresults").AddNode("questiontext", true).sText = DataMethods.gsGetDBValue(sConnstr_Svm, "tdQuestions", "qst_text", "qst_svy_id=(select " + "pub_svy_id FROM tdPublications WHERE pub_id=" + lPub_id + ")", "");
			oXmlDoc.selectSingleNode("culmulativeresults")["sumCounter"].sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(lSumCounter);
		
		
			//---------return--------
			return oXmlDoc;
		}
	
	
	
		//================================================================================
		//Function:  goXmlGetTestResults
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the result of a test as xml
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   13.05.2004 23:57:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static XmlDocument goXmlGetTestResults(string sConnStr_SurveyDb, string sConnStr_ResultDB, int lPub_id, int lSessionID)
		{
		
			//----------------get recordsets------------------
		
			Recordset rsPublication = DataMethods.gRsGetTable(sConnStr_SurveyDb, "tdPublications", "pub_svy_id", "pub_id=" + lPub_id, "", "", "");
		
			Recordset rsSurvey = DataMethods.gRsGetTable(sConnStr_SurveyDb, "tdSurveys", "*", "svy_id=" + rsPublication["pub_svy_id"].sValue, "", "", "");
		
			Recordset rsQuestions = DataMethods.gRsGetTable(sConnStr_SurveyDb, "tdQuestions", "*", "qst_svy_id=" + rsSurvey["svy_id"].sValue, "", "", "qst_index");
		
		
		
			string sResultTableName = Preparation.ResultTableHandler.gsGetResultTableName(lPub_id);


			//-------------------------if only a subset was returned, then remove all unhandled questions------------------
			string sUsedQuestions = DataMethods.gsGetDBValue(sConnStr_ResultDB, sResultTableName, svmEnums.efsFieldUsedQuestions, "SessionID=" + lSessionID, "");
			Submission.Submissioner.gRemoveQuestionsNotInSubset(ref rsQuestions, sUsedQuestions, "qst_id");



		
			//----------------init xml-------------------------
			XmlDocument oXmlResult = new XmlDocument("<testresults/>");
		
			//----------iterate answers and determine correctness-----------
			while (! rsQuestions.EOF)
			{
			
				Recordset rsAnswers = DataMethods.gRsGetTable(sConnStr_SurveyDb, "tdAnswers", "*", "ans_qst_id =" + rsQuestions["qst_id"].sValue, "", "", "ans_index");
			
				XmlNode oQuestionNode = oXmlResult.selectSingleNode("/testresults").AddNode("question", false);
			
				//-------- set question text in question-node -------------
				oQuestionNode.AddNode("questiontext", true).sText = rsQuestions["qst_text"].sValue;
			
			
				while (! rsAnswers.EOF)
				{
				
					XmlNode oNewNode = oQuestionNode.AddNode("answer", false);
					oNewNode.oAttributeList["type"].sText = Functions.UCase(rsAnswers["ans_aty_id"].sValue);
				
				
					if (Functions.UCase(rsAnswers["ans_aty_id"].sValue) == "MULTIPLECHOICE" | Functions.UCase(rsAnswers["ans_aty_id"].sValue) == "SINGLECHOICE")
					{
					

						Recordset rsAnswerValues = DataMethods.gRsGetTable(sConnStr_SurveyDb, "tdAnswerValues", "*", "val_ans_id=" + rsAnswers["ans_id"].sValue, "", "", "");
					
						string sResultStringInDb = DataMethods.gsGetDBValue(sConnStr_ResultDB, sResultTableName, DataTools.SQLField(rsAnswers["ans_resultDBField"].sValue), "SessionID=" + lSessionID, "");

						while (! rsAnswerValues.EOF)
						{
						
							//------get the answer-value-------
							string sAnswerValue;
							sAnswerValue = rsAnswerValues["val_for_db"].sValue;
							if (Functions.IsEmptyString(sAnswerValue))
							{
								sAnswerValue = rsAnswerValues["val_text"].sValue;
							}
						
							//----------test, if answer value is in result-string-------
							bool bAnswerInResultString;
							XmlNode nAnswerValueNode = null;
							
							if (Functions.InStr(", " + sResultStringInDb + ",",", " + sAnswerValue + "," ) > 0)
							{
								bAnswerInResultString = true;
							}
							else
							{
								bAnswerInResultString = false;
							}
						
						
							//-----------check if good, false, missing answer-----------
							bool bAnswerIsCorrectAnswered = false; 
							nAnswerValueNode = null;
							if (rsAnswerValues["val_correct"].bValue == true & bAnswerInResultString == false)
							{
								//Auswertung single-choice weicht von multiple-choice insofern ab, als dass ausgelassene Antworten
								//nicht als zusätzlicher Fehler gewertet werden:
								if (Functions.UCase(rsAnswers["ans_aty_id"].sValue) != "SINGLECHOICE") 
								{
									nAnswerValueNode = oNewNode.AddNode("missing", false);
									nAnswerValueNode.AddNode("correctValue", true).sText = rsAnswerValues["val_text"].sValue;
									bAnswerIsCorrectAnswered = false;
								}
							
							}
							else if (rsAnswerValues["val_correct"].bValue == false & bAnswerInResultString == true)
							{
								nAnswerValueNode = oNewNode.AddNode("false", false);
								nAnswerValueNode.AddNode("yourValue", true).sText = rsAnswerValues["val_text"].sValue;
								bAnswerIsCorrectAnswered = false;
							
							}
							else if (rsAnswerValues["val_correct"].bValue == true & bAnswerInResultString == true)
							{
								nAnswerValueNode = oNewNode.AddNode("good", false);
								nAnswerValueNode.AddNode("yourValue", true).sText = rsAnswerValues["val_text"].sValue;
								bAnswerIsCorrectAnswered = true;
							}
						
							//------------add the additional texts-----------------------
							if (nAnswerValueNode != null)
								if (bAnswerIsCorrectAnswered) 
									nAnswerValueNode.AddNode("notetext", true).sText = rsAnswerValues["val_text_on_correct"].sValue;
								else 
									nAnswerValueNode.AddNode("notetext", true).sText = rsAnswerValues["val_text_on_false"].sValue; 
						
							rsAnswerValues.MoveNext();
						} 
					}
					else if (Functions.UCase(rsAnswers["ans_aty_id"].sValue) == "GAPTEXT")
					{
					
						Recordset rsAnswerValues = DataMethods.gRsGetTable(sConnStr_SurveyDb, "tdAnswerValues", "*", "val_ans_id=" + rsAnswers["ans_id"].sValue, "", "", "");
					
						string sGapText = rsAnswers["ans_gap_txt"].sValue;
					
					
						int lCounter = 0;
						while (! rsAnswerValues.EOF)
						{
							lCounter += 1;
						
							//------get the answer-value-------
							string sAnswerValue;
							sAnswerValue = rsAnswerValues["val_for_db"].sValue;
							if (Functions.IsEmptyString(sAnswerValue))
							{
								sAnswerValue = rsAnswerValues["val_text"].sValue;
							}
						
							//----------get the user input of the answer-value-------
							string sYourValue;
							sYourValue = DataMethods.gsGetDBValue(sConnStr_ResultDB, sResultTableName, DataTools.SQLField(rsAnswers["ans_resultDBField"].sValue + "_" + lCounter), "SessionID=" + lSessionID, "");
						
						
							//-----------check if good, false, missing answer-----------
							bool bCorrect;
							if (sYourValue == sAnswerValue)
							{
								bCorrect = true;
							}
							else
							{
								bCorrect = false;
							}
						
							//-----------------add false/correct-nodes------------------------
							if (bCorrect)
							{
								XmlNode oSubNode = oNewNode.AddNode("good", false);
								oSubNode.AddNode("yourValue", true).sText = sYourValue;
								oSubNode.AddNode("correctValue", true).sText = sAnswerValue;
							}
							else
							{
								XmlNode oSubNode = oNewNode.AddNode("false", false);
								oSubNode.AddNode("yourValue", true).sText = sYourValue;
								oSubNode.AddNode("correctValue", true).sText = sAnswerValue;
							}
						
						
							//-----------------replace parameter in Gap-Text--------------------
							string sReplaceInGapText = "";
							if (bCorrect)
							{
								sReplaceInGapText = "<font color=\"green\"><b>" + sYourValue + "</b></font>";
							}
							else
							{
								sReplaceInGapText = "<font color=\"red\"><b><strike>" + sYourValue + "</strike></b></font>&nbsp;" + "<font color=\"green\"><b>" + sAnswerValue + "</b></font>";
							}
						
						
							sGapText = Functions.Replace(sGapText, "$" + lCounter, sReplaceInGapText);
						
						
						
						
						
							//---------------next answer-value----------------------
							rsAnswerValues.MoveNext();
						} 


					
						//extend the question-text with the gaptext:
						oQuestionNode.AddNode("gaptext", true).sText = sGapText;
					
					}
				
					else if (Functions.UCase(rsAnswers["ans_aty_id"].sValue) == "FREETEXT")
					{
					
						string sSuggestion = rsAnswers["ans_freetext_suggestion"].sValue; //Lösungsvorschlag

					
						
						//----------get the user input of the answer-value-------
						string sYourValue;
						sYourValue = DataMethods.gsGetDBValue(sConnStr_ResultDB, sResultTableName, DataTools.SQLField(rsAnswers["ans_resultDBField"].sValue), "SessionID=" + lSessionID, "");
						
					
						//-----------------add false/correct-nodes------------------------

						XmlNode oSubNode = oNewNode.AddNode("texts", false);
						oSubNode.AddNode("yourValue", true).sText = sYourValue;
						oSubNode.AddNode("suggestion", true).sText = sSuggestion;


					} 
				
					rsAnswers.MoveNext();
				} 
			
				rsQuestions.MoveNext();
			} 

			
		
			//-------------------check if test is passed or not-------------------------
		
			int lCount_Good = oXmlResult.selectNodes("//good").lCount;
			int lCount_Missing = oXmlResult.selectNodes("//missing").lCount;
			int lCount_False = oXmlResult.selectNodes("//false").lCount;
		
		
			decimal dPercentCorrect = 0;
			if (Convert.ToDecimal(lCount_Missing + lCount_False + lCount_Good) * 100 > 0)
				dPercentCorrect = Convert.ToDecimal( lCount_Good) /Convert.ToDecimal(lCount_Missing + lCount_False + lCount_Good) * 100;
			
		
			bool bPassed;
			bPassed = dPercentCorrect >= rsSurvey["svy_testpassed_percentage"].dValue;
		
		
		
			oXmlResult.selectSingleNode("/testresults").AddNode("passed", true).sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(easyFramework.Sys.ToolLib.DataConversion.glCInt(bPassed, 0));
			oXmlResult.selectSingleNode("/testresults").AddNode("achievedPercents", true).sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(dPercentCorrect);
		
		
		
			return oXmlResult;
		}
	
		//================================================================================
		//Function:  goXmlGetVoteResults
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the result of a vote as xml
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   13.05.2004 23:57:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static XmlDocument goXmlGetVoteResults(string sConnStr_SurveyDb, string sConnStr_ResultDB)
		{
		
			  return null;
		
		
		}


	
	}




}
