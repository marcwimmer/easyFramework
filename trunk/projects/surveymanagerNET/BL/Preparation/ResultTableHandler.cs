using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.SqlTools;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     ResultTableHandler
	//--------------------------------------------------------------------------------'
	//Module:    ResultTableHandler.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   this class synchronizes and creates the result-tables for
	//           a publication
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 22:15:22
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================
	namespace Preparation
	{
	
		public class ResultTableHandler
		{
		
		
			//================================================================================
			//Public Methods:
			//================================================================================
		
			//================================================================================
			//Sub:       gSynchronizeResultTable
			//--------------------------------------------------------------------------------'
			//Purpose:   creates the result-table, if not exist; otherwise it synchronizes the
			//           result-table with the survey
			//           if there are errors, they will be returned in the clientinfo error-
			//           collection
			//--------------------------------------------------------------------------------'
			//Params:    clientinfo
			//           publication-id
			//           entity-loader
			//--------------------------------------------------------------------------------'
			//Created:   11.05.2004 22:16:32
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static void gSynchronizeResultTable (ClientInfo oClientInfo, int lPub_id)
			{
			
			
				//--------------load entities--------------------
				Recordset rsPublication = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdPublications", "*", "pub_id=" + lPub_id, "", "", "");
				if (rsPublication.EOF)
				{
					throw (new efException("Invalid pub_id \"" + lPub_id + "\"."));
				}
			
				Recordset rsSurvey = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveys", "*", "svy_id=" + rsPublication["pub_svy_id"].sValue, "", "", "");
			
			
			
				Recordset rsQuestions = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdQuestions", "*", "qst_svy_id=" + rsPublication["pub_svy_id"].sValue, "", "", "qst_index");
			








			
				//-----------get connection to result-database----------------
				string sConnstr = Config.gsConnstrResultDB(oClientInfo);
			
			
			
				//-----------check, if table exists--------------------
				//-----------if not create table; the primary column is always sessionid
				string sTableName = gsGetResultTableName(lPub_id);
				SqlTable oSqlTable = new SqlTable(sConnstr, sTableName);
				if (oSqlTable.gbExistsTable() == false)
				{
					oSqlTable.gCreateTable("SessionID", SqlBase.efEnumSqlDbType.efInt, -1);
				}
			
			
				//-----------add a column for the survey-id and a timestamp-field, if not exists--------
				if (! oSqlTable.gbExistsColumn("svy_id"))
				{
					oSqlTable.gAddColumn("svy_id", SqlBase.efEnumSqlDbType.efInt, -1, true, "");
				}
				if (! oSqlTable.gbExistsColumn("date"))
				{
					oSqlTable.gAddColumn("date", SqlBase.efEnumSqlDbType.efDate, -1, false, "getdate()");
				}

				//--------------add a column to store the replied questions, if there is only a subset backposted---------
				if (!oSqlTable.gbExistsColumn(svmEnums.efsFieldUsedQuestions))
					oSqlTable.gAddColumn(svmEnums.efsFieldUsedQuestions, SqlBase.efEnumSqlDbType.efNVarChar, 1024, true, "");
			
			
				//-------------iterate questions and their answers and create the result-table----------
				while (! rsQuestions.EOF)
				{
				
					Recordset rsAnswers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswers", "*", "ans_qst_id=" + rsQuestions["qst_id"].sValue, "", "", "ans_index");
				
				
					while (! rsAnswers.EOF)
					{
					
						//-----------check if answer has a result-column
						string sDBFieldOfAnswer = rsAnswers["ans_resultDbField"].sValue;
					
					
						//-------don't allow spaces --> always Problems---------------
						if (Functions.InStr(sDBFieldOfAnswer, " ") > 0)
						{
							oClientInfo.gAddError("Datenbankfeld für Antwort auf Frage \"" + rsQuestions["qst_text"].sValue + "\" enthält ein Leerzeichen im Datenbankfeldnamen. Dies ist nicht zulässig.");
						}											
					
					
						if (Functions.IsEmptyString(sDBFieldOfAnswer))
						{
							oClientInfo.gAddError("Datenbankfeld für Antwort auf Frage \"" + rsQuestions["qst_text"].sValue + "\" fehlt! Ergebnistabelle kann nicht erzeugt werden. Bitte nachtragen!");
						}
						else
						{
						
							//-----------distinguish gap-text and all other answers-------------
							if (Functions.UCase(rsAnswers["ans_aty_id"].sValue) == "GAPTEXT")
							{
							
								//--------create for each gap-text box an own db-field in the
								//        form of DBFIELD_1, DBFIELD_2, DBFIELD_3....------------
							
								int lCountFields = DataMethodsClientInfo.glDBCount(oClientInfo, "tdAnswerValues", "*", "val_ans_id=" + rsAnswers["ans_id"].sValue, "");
							
								for (int i = 0; i <= lCountFields - 1; i++)
								{
								
									string sColumnName;
									sColumnName = sDBFieldOfAnswer + "_" + Convert.ToInt32(i + 1);
									if (! oSqlTable.gbExistsColumn(sColumnName))
									{
										oSqlTable.gAddColumn(sColumnName, SqlBase.efEnumSqlDbType.efNVarChar, 3800, true, "");
									}
								
								}
							
							
							}
							else
							{
								//-----------check if result-column exists; if not, then try to create
								if (! oSqlTable.gbExistsColumn(sDBFieldOfAnswer))
								{
								
									oSqlTable.gAddColumn(sDBFieldOfAnswer, SqlBase.efEnumSqlDbType.efNVarChar, 3800, true, "");
								
								}
							
							}
						
						}
					
						rsAnswers.MoveNext();
					} 
				
				
					rsQuestions.MoveNext();
				} 
			
			
			
				//------if survey is a vote, then create a culmulated-table of the single/multiple-choice questions---
				if (rsSurvey["svy_svt_id"].sValue == "VOTE")
				{
				
					string sCulmulatedTableName = gsGetResultTableNameCulmulated(lPub_id);
					oSqlTable = new SqlTable(sConnstr, sCulmulatedTableName);
				
					if (oSqlTable.gbExistsTable() == false)
					{
						oSqlTable.gCreateTable("val_text", SqlBase.efEnumSqlDbType.efNVarChar, 400);
					}
				
					if (! oSqlTable.gbExistsColumn("Counter"))
					{
						oSqlTable.gAddColumn("Counter", SqlBase.efEnumSqlDbType.efInt, -1, true, "");
					}
				
					if (! oSqlTable.gbExistsColumn("LastUpdate"))
					{
						oSqlTable.gAddColumn("LastUpdate", SqlBase.efEnumSqlDbType.efDate, -1, true, "");
					}
				
				
				}
			
			
			}
		
		
			//================================================================================
			//Private Methods:
			//================================================================================
		
		
			//================================================================================
			//Function:  gsGetResultTableName
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the name of the result-table
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   11.05.2004 22:23:26
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static string gsGetResultTableName(int lPub_id)
			{
			
				return "results_pub_" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lPub_id);
			
			}
		
			//================================================================================
			//Function:  gsGetResultTableNameCulmulated
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the name of the result-table of culmulated answers
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   11.05.2004 22:23:26
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static string gsGetResultTableNameCulmulated(int lPub_id)
			{
			
				return "results_culmulated_pub_" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lPub_id);
			
			}
		}
	}

}
