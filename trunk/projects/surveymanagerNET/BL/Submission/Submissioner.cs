using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Project.SurveyManager;
using easyFramework.Project.SurveyManager.Preparation;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     Submissioner

	//--------------------------------------------------------------------------------'
	//Module:    Submissioner.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   for submitting a survey
	//--------------------------------------------------------------------------------'
	//Created:   12.05.2004 11:35:01
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	namespace Submission
	{
	
	
		public class Submissioner
		{
		
		
		
			//================================================================================
			//Function:  glSubmit
			//--------------------------------------------------------------------------------'
			//Purpose:   stores the survey in db;
			//           if there are any errors, then the errors are listed in asErrorList
			//--------------------------------------------------------------------------------'
			//Params:    the request object of the fulfilled survey
			//           sConnstr_svm: connection-string to surveymanager admin-database
			//           sConnstr_resultdb: connection-string to surveymanager result-database
			//           ref-parameter asErrorList: here are the erros returned, if there are
			//           any
			//
			//           returns the sessionid
			//--------------------------------------------------------------------------------'
			//Created:   12.05.2004 11:35:54
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static int glSubmit(System.Web.HttpRequest oRequest, System.Web.HttpApplicationState oApplication, string sConnstr_svm, string sConnstr_resultdb, ref efArrayList asErrorList)
			{
			
				asErrorList = new efArrayList();
			
				//-----------------------get survey--------------------------------
				string sSvy_id = oRequest["svy_id"];
				if (Functions.IsEmptyString(sSvy_id))
				{
					asErrorList.Add("Angabe des Surveys fehlt!");
					return 0;
				}
				Recordset rsSurvey = DataMethods.gRsGetTable(sConnstr_svm, "tdSurveys", "*", "svy_id=" + sSvy_id, "", "", "");
			
				//-------------------------get publication-------------------------
			
				Recordset rsPublication = Preparation.Publications.goCurrentActivePublication(sConnstr_svm, DataConversion.glCInt(sSvy_id, 0));
			
				if (rsPublication.EOF)
				{
					asErrorList.Add("Keine Publikation für die Umfrage/Test/Gewinnspiel vorhanden!");
					return 0;
				}
			
			
				//--------------------check result-table----------------------------
				string sResultTableName = Preparation.ResultTableHandler.gsGetResultTableName(rsPublication["pub_id"].lValue);
			
			
				int lSessionID = -1;
			
				//------------------- start transaction-------------------
				efTransaction conn_svm = new efTransaction(sConnstr_svm);
				efTransaction conn_results = new efTransaction(sConnstr_resultdb);
			
				conn_results.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
			
				//-------------------create mini clientinfo-object--------------------
				easyFramework.Sys.ClientInfo oClientInfo = new easyFramework.Sys.ClientInfo("", sConnstr_svm);
				oClientInfo.oHttpApp = new easyFramework.Sys.Webobjects.HttpApp(oApplication);
				oClientInfo.oHttpRequest = new easyFramework.Sys.Webobjects.HttpRequest(oRequest);
				oClientInfo.oVolatileField["conn_results"] = conn_results;
			
			
			
				try
				{
				
				
					//--------------------store the answers-----------------------------------
					Recordset rsAllDefinedAnswers = DataMethods.gRsGetTable(sConnstr_svm, "tdAnswers", "*", "ans_qst_id IN (SELECT qst_id FROM tdQuestions WHERE qst_svy_id=" + sSvy_id + ")", "", "", "");
				
					//----------falls nicht alle Fragen, sondern nur eine Teilmenge verwendet wurde, dann alle nicht benötigten Antworten rausschmeißen:---------
					string sUsedQuestions = oClientInfo.oHttpRequest.oHttpRequest[svmEnums.efsFieldUsedQuestions];
					gRemoveQuestionsNotInSubset(ref rsAllDefinedAnswers, sUsedQuestions, "ans_qst_id");



					XmlDocument oXmlAllResultsForDb = new XmlDocument("<results/>");
					XmlDocument oXmlAllChoiceAnswerValueTexts = new XmlDocument("<valtexts/>"); //extra list of choice-answers
				
					while (! rsAllDefinedAnswers.EOF)
					{
					
						string sValueInRequest;
						XmlNode oNode;
						string[] asSplittedByComma;
								
						switch (rsAllDefinedAnswers["ans_aty_id"].sValue)
						{
						
							case "FREETEXT":
						
						
								
								oNode = oXmlAllResultsForDb.selectSingleNode("/results").AddNode(rsAllDefinedAnswers["ans_resultDBField"].sValue, true);
						
								sValueInRequest = oRequest["ANSWER_" + rsAllDefinedAnswers["ans_id"].sValue];
						
								//-----trim-----
								sValueInRequest = Functions.Trim(sValueInRequest);
						
								oNode.sText = sValueInRequest;
								break;
						
							case "GAPTEXT":
						
						
								Recordset rsAnswerValues = DataMethods.gRsGetTable(sConnstr_svm, "tdAnswerValues", "*", "val_ans_id=" + rsAllDefinedAnswers["ans_id"].sValue, "", "", "val_index");
						
								int lCounter = 0;
								while (! rsAnswerValues.EOF)
								{
									lCounter += 1;
							
									
									string sColumnName = rsAllDefinedAnswers["ans_resultDBField"].sValue + "_" + lCounter;
									oNode = oXmlAllResultsForDb.selectSingleNode("/results").AddNode(sColumnName, true);
							
							
									sValueInRequest = oRequest["GAPTEXT_" + rsAnswerValues["val_id"].sValue];
							
									//-----trim-----
									sValueInRequest = Functions.Trim(sValueInRequest);
							
									oNode.sText = sValueInRequest;
							
							
									rsAnswerValues.MoveNext();
								}
								break;
						
						
						
							case "SINGLECHOICE":
						
								oNode = oXmlAllResultsForDb.selectSingleNode("/results").AddNode(rsAllDefinedAnswers["ans_resultDBField"].sValue, true);
						
								sValueInRequest = oRequest["ANSWER_" + rsAllDefinedAnswers["ans_id"].sValue];
						
								//----------get the text of the answer-value-----------
								if (Functions.InStr(sValueInRequest, ",") == 0)
								{
									asSplittedByComma = new string[1];
									asSplittedByComma[0] = sValueInRequest;
								}
								else
								{
									asSplittedByComma = Functions.Split(sValueInRequest, ",");
								}
						
								for (int i = 0; i <= asSplittedByComma.Length - 1; i++)
								{
							
									string sValue = Functions.Trim(asSplittedByComma[i]);
							
									efArrayList oValueTexts;
									oValueTexts = new efArrayList();
							
									if (!Functions.IsEmptyString(sValue))
									{
								
										int lval_id = DataConversion.glCInt(Functions.Right(sValue, Functions.Len(sValue) - Functions.Len("ANSVAL_")), 0);
								
										Recordset rsAnswervalue = DataMethods.gRsGetTable(sConnstr_svm, "tdAnswerValues", "val_text, val_for_db", "val_id=" + lval_id, "", "", "");
										if (!Functions.IsEmptyString(rsAnswervalue["val_for_db"].sValue))
										{
											oNode.sText += rsAnswervalue["val_for_db"].sValue;
											oValueTexts.Add(rsAnswervalue["val_for_db"].sValue);
										}
										else
										{
											oNode.sText += rsAnswervalue["val_text"].sValue;
											oValueTexts.Add(rsAnswervalue["val_text"].sValue);
										}
								
										if (i < asSplittedByComma.Length - 1)
										{
											oNode.sText += ", ";
										}
								
								
									}
							
									//append the single-answervalues to the result-node-text
									for (int y = 0; y <= oValueTexts.Count - 1; y++)
									{
										oXmlAllChoiceAnswerValueTexts.selectSingleNode("/valtexts").AddNode("valtext", true).sText = System.Convert.ToString(oValueTexts[y]);
									}
								}
								break;
						
							case "CHECKBOX":
								
								oNode = oXmlAllResultsForDb.selectSingleNode("/results").AddNode(rsAllDefinedAnswers["ans_resultDBField"].sValue, true);
								try 
								{
									string sId = "ANSWER_" + rsAllDefinedAnswers["ans_id"].sValue;
									sValueInRequest =Convert.ToString(oRequest[sId]);
								}
								catch 
								{
									sValueInRequest = "";
								}
								//bei checkbox soll entweder 0 oder 1 gespeichert werden
								string valueInResultNode = "0";
								if (sValueInRequest != "" & sValueInRequest != null) 
								{
									valueInResultNode = "1";
								}

								oNode.sText = valueInResultNode;
								
								//throw new Exception(sValueInRequest);
								break;
							case "MULTIPLECHOICE":
						
						
								
								oNode = oXmlAllResultsForDb.selectSingleNode("/results").AddNode(rsAllDefinedAnswers["ans_resultDBField"].sValue, true);
						
								sValueInRequest = oRequest["ANSWER_" + rsAllDefinedAnswers["ans_id"].sValue];
						
								//----------get the text of the answer-value-----------
								if (Functions.InStr(sValueInRequest, ",") == 0)
								{
									asSplittedByComma = new string[1];
									asSplittedByComma[0] = sValueInRequest;
								}
								else
								{
									asSplittedByComma = Functions.Split(sValueInRequest, ",");
								}
						
								for (int i = 0; i <= asSplittedByComma.Length - 1; i++)
								{
							
									string sValue = Functions.Trim(asSplittedByComma[i]);
							
									efArrayList oValueTexts;
									oValueTexts = new efArrayList();
							
									if (!Functions.IsEmptyString(sValue))
									{
								
										int lval_id = DataConversion.glCInt(Functions.Right(sValue, Functions.Len(sValue) - Functions.Len("ANSVAL_")), 0);
								
										Recordset rsAnswervalue = DataMethods.gRsGetTable(sConnstr_svm, "tdAnswerValues", "val_text, val_for_db", "val_id=" + lval_id, "", "", "");
										if (!Functions.IsEmptyString(rsAnswervalue["val_for_db"].sValue))
										{
											oNode.sText += rsAnswervalue["val_for_db"].sValue;
											oValueTexts.Add(rsAnswervalue["val_for_db"].sValue);
										}
										else
										{
											oNode.sText += rsAnswervalue["val_text"].sValue;
											oValueTexts.Add(rsAnswervalue["val_text"].sValue);
										}
								
										if (i < asSplittedByComma.Length - 1)
										{
											oNode.sText += ", ";
										}
								
								
									}
							
									//append the single-answervalues to the result-node-text
									for (int y = 0; y <= oValueTexts.Count - 1; y++)
									{
										oXmlAllChoiceAnswerValueTexts.selectSingleNode("/valtexts").AddNode("valtext", true).sText = System.Convert.ToString(oValueTexts[y]);
									}
								}
								break;
						
						
							default:
						
								asErrorList.Add("Unbehandelter Antworttyp \"" + rsAllDefinedAnswers["ans_aty_id"].sValue + "\".");
								break;
						
						}
				
				
				
				
				
				
				
				
						rsAllDefinedAnswers.MoveNext();
					} 
			
					//--------------------create a new session in the result-table------------
					lSessionID = 1 + DataMethods.glGetDBValue(conn_results, sResultTableName, "max(SessionID) AS maxID", "", 0);
			
					//--------------------store the session in the table----------------------
					DataMethods.gInsertTable(conn_results, sResultTableName, "SessionID", DataConversion.gsCStr(lSessionID));
			
					//--------------------store the survey-id in the table-------------------
					DataMethods.gUpdateTable(conn_results, sResultTableName, "svy_id=" + sSvy_id, "sessionid=" + lSessionID);
			
					//--------------------store the temporary xml in the database----------------------
					XmlNodeList nlNodeList = oXmlAllResultsForDb.selectNodes("/results/*");
					for (int i = 0; i <= nlNodeList.lCount - 1; i++)
					{
						string sQry = "$1 = '$2'";
						sQry = sQry.Replace("$1", DataTools.SQLField(nlNodeList[i].sName));
						sQry = sQry.Replace("$2", DataTools.SQLString(nlNodeList[i].sText));
						DataMethods.gUpdateTable(conn_results, sResultTableName,  sQry, "SessionID=" + lSessionID);
						
					}

					//-------------store the postback-question-ids----------------------------
					DataMethods.gUpdateTable(conn_results, sResultTableName, svmEnums.efsFieldUsedQuestions + "='" + sUsedQuestions + "'", "SessionID=" + lSessionID);
			
			
					//-----------------if it is a vote, then store the fix-answers in the culmulative-table------
					if (rsSurvey["svy_svt_id"].sValue == "VOTE")
					{
				
						XmlNodeList nlValueTexts = oXmlAllChoiceAnswerValueTexts.selectNodes("//valtext");
						for (int i = 0; i <= nlValueTexts.lCount - 1; i++)
						{
					
							string sCulmulativeTableName = Preparation.ResultTableHandler.gsGetResultTableNameCulmulated(DataConversion.glCInt(rsPublication["pub_id"].lValue, 0));
					
					
					
							if (DataMethods.glDBCount(conn_results, sCulmulativeTableName, "*", "val_text='" + DataTools.SQLString(nlValueTexts[i].sText) + "'", "") == 0)
							{
						
								DataMethods.gInsertTable(conn_results, sCulmulativeTableName, "val_text, counter", "'" + DataTools.SQLString(nlValueTexts[i].sText) + "', 0");
						
							}
					
							DataMethods.gUpdateTable(conn_results, sCulmulativeTableName, "lastupdate=getdate(), counter = counter + 1", "val_text='" + DataTools.SQLString(nlValueTexts[i].sText) + "'");
					
					
					
						}
					}
			
			
					//-------------------------call the sysevent-engine----------------------
			
					SysEventEngine oSysEventEngine = new SysEventEngine();
					bool bRollback = false;
					XmlDocument oXmlParam = new XmlDocument("<param/>");
					oXmlParam.selectSingleNode("/param").AddNode("sessionid", true).sText = DataConversion.gsCStr(lSessionID);
					oXmlParam.selectSingleNode("/param").AddNode("resultTableName", true).sText = DataConversion.gsCStr(sResultTableName);
					oXmlParam.selectSingleNode("/param").AddNode("connstr_svm", true).sText = DataConversion.gsCStr(sConnstr_svm);
					//oXmlParam.selectSingleNode("/param").AddNode("connstr_resultdb", True).sText = gsCStr(sConnstr_resultdb) use volatile object conn_results
					object[] aoReturnedObjects = null;
					oSysEventEngine.gRaiseAfterEvents(oClientInfo, "Survey", "Submission", DataConversion.gsCStr(lSessionID), oXmlParam, ref bRollback, ref aoReturnedObjects);
			
			
					//---throw errors as exception, if there are any---------
					if (oClientInfo.bHasErrors)
					{
						conn_results.RollbackTrans();
						asErrorList.Add(oClientInfo.gsErrors());
				
					}
					else
					{
						conn_results.CommitTrans();
					}
			
			
				}
				catch (System.Exception ex)
				{
			
					conn_results.RollbackTrans();
			
					if (oClientInfo.bHasErrors)
					{
						asErrorList.Add(oClientInfo.gsErrors());
					}
					
							

					throw ex;
			
				}
				finally
				{
			
				}
		
		
		
				return lSessionID;
		
			}
	
			/// <summary>
			/// Hier werden alle Fragen entfernt, die nicht in das ausgewählte Subset passen, z.B.
			/// wenn 20 Fragen zufällig aus 100 ausgewählt wurden.
			/// </summary>
			/// <param name="?"></param>
			/// <param name="sUsedQuestions"></param>
			public static void gRemoveQuestionsNotInSubset(ref Recordset rsQuestions, string sUsedQuestions, string sCompareFieldName) 
			{

				if (sUsedQuestions != null)
				{
					if (sUsedQuestions.Length > 0 ) 
					{
						string[] splitted = Functions.Split(sUsedQuestions, ",");
						while (!rsQuestions.EOF) 
						{
							bool bFound = false;
							string sQst_id = rsQuestions[sCompareFieldName].sValue;
							for (int i = 0; i < splitted.Length & !bFound; i++) 
							{
								if (sQst_id == splitted[i]) 
								{
									bFound = true;
								}
							}
							if (!bFound)  
							{
								rsQuestions.Delete();
								
							}
							else 
							{
								rsQuestions.MoveNext();
							}
						}

					}

				}
				rsQuestions.MoveFirst();

			}
	
		}



	}

}
