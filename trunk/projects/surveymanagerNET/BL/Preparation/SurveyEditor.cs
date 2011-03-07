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
	//Class:     SurveyEditor

	//--------------------------------------------------------------------------------'
	//Module:    SurveyEditor.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   main-functions for editing a survey; everything shared
	//--------------------------------------------------------------------------------'
	//Created:   02.05.2004 11:06:10
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================



	namespace Preparation
	{
	
	
		public class SurveyEditor
		{
		
		
		
			//================================================================================
			//Function:  glCreateSurvey
			//--------------------------------------------------------------------------------'
			//Purpose:   creates a new survey. if lTemplateSurveyId isn't 0 then
			//           an existing survey is copied to the new survey
			//--------------------------------------------------------------------------------'
			//Params:    the survey-id of an existing survey
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   02.05.2004 11:07:16
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static int glCreateSurvey(ClientInfo oClientInfo, int lTemplateSurveyId, System.Web.HttpRequest oRequest, System.Web.HttpApplicationState oApp, int svy_svg_id, string svy_name, string svy_svt_id)
			{
			
				DefaultEntity oEntity = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entSurveys);
			
				int lResult;
			
				oEntity.gNew(oClientInfo, "");
				if (svy_svg_id >= 1)
				{
					oEntity.oFields["svy_svg_id"].lValue = svy_svg_id;
				}
				oEntity.oFields["svy_name"].sValue = svy_name;
				oEntity.oFields["svy_template"].bValue = false;
				oEntity.oFields["svy_svt_id"].sValue = svy_svt_id;
				oEntity.oFields["svy_sst_id"].sValue = svmEnums.efsSURVEYSTATE_DESIGN;
				if (! oEntity.gSave(oClientInfo))
				{
					throw (new efException(oClientInfo.gsErrors()));
				}
				else
				{
					lResult = oEntity.oKeyField.lValue;
				}
			
			
				if (lTemplateSurveyId >= 1)
				{
					gCopySurvey(oClientInfo, lTemplateSurveyId, oEntity.oFields["svy_id"].lValue, oApp, oRequest);
				}
			
			
				return lResult;
			
			}
		
		
			//================================================================================
			//Sub:       gCopySurvey
			//--------------------------------------------------------------------------------'
			//Purpose:   copies the questions and answers and so on from one survey to
			//           the other survey
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   18.05.2004 14:29:58
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static void gCopySurvey (ClientInfo oClientInfo, int lCopyFrom_svy_id, int lCopyTo_svy_id, System.Web.HttpApplicationState oApp, System.Web.HttpRequest oRequest)
			{
			
			
				Recordset rsQuestions = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdQuestions", "*", "qst_svy_id=" + lCopyFrom_svy_id, "", "", "");
			
			
				while (! rsQuestions.EOF)
				{
				
				
					DefaultEntity oQuestionEntity = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entQuestions);
				
				
					oQuestionEntity.gNew(oClientInfo, "");
					for (int i = 0; i <= oQuestionEntity.oFields.Count - 1; i++)
					{
						switch (Functions.LCase(oQuestionEntity.oFields[i].sName))
						{
						
							case "qst_id":
						
								break;
						
							case "qst_svy_id":
						
								oQuestionEntity.oFields[i].lValue = lCopyTo_svy_id;
								break;
							default:
						
								oQuestionEntity.oFields[i].sValue = rsQuestions.oFields[oQuestionEntity.oFields[i].sName].sValue;
								break;
						
						}
					}
			
					//------------store question-----------
					if (! oQuestionEntity.gSave(oClientInfo))
					{
						throw (new efException(oClientInfo.gsErrors()));
					}
			
			
					//-------------copy the answers-----------------
					Recordset rsAnswers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswers", "*", "ans_qst_id=" + rsQuestions["qst_id"].sValue, "", "", "");
			
					while (! rsAnswers.EOF)
					{
				
				
						DefaultEntity oAnswerEntity = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entAnswers);
				
				
						oAnswerEntity.gNew(oClientInfo, "");
						for (int i = 0; i <= oAnswerEntity.oFields.Count - 1; i++)
						{
							switch (Functions.LCase(oAnswerEntity.oFields[i].sName))
							{
						
								case "ans_id":
						
									break;
						
								case "ans_qst_id":
						
									oAnswerEntity.oFields[i].lValue = oQuestionEntity.oFields["qst_id"].lValue;
									break;
								default:
						
									oAnswerEntity.oFields[i].sValue = rsAnswers.oFields[oAnswerEntity.oFields[i].sName].sValue;
									break;
						
							}
						}
			
						//---------store answer----------
						oAnswerEntity.gSave(oClientInfo);
			
			
						//---------copy answer-values----------
						Recordset rsAnswerValues = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdAnswerValues", "*", "val_ans_id=" + rsAnswers.oFields["ans_id"].sValue, "", "", "");
			
						while (! rsAnswerValues.EOF)
						{
				
							DefaultEntity oAnswerValueEntity = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entAnswerValues);
				
				
							oAnswerValueEntity.gNew(oClientInfo, "");
							for (int i = 0; i <= oAnswerValueEntity.oFields.Count - 1; i++)
							{
								switch (Functions.LCase(oAnswerValueEntity.oFields[i].sName))
								{
						
									case "val_id":
						
										break;
						
									case "val_ans_id":
						
										oAnswerValueEntity.oFields[i].lValue = oAnswerEntity.oFields["ans_id"].lValue;
										break;
									default:
						
										oAnswerValueEntity.oFields[i].sValue = rsAnswerValues[oAnswerValueEntity.oFields[i].sName].sValue;
										break;
						
								}
							}
			
							//---------store answer----------
							oAnswerValueEntity.gSave(oClientInfo);
			
			
			
			
							rsAnswerValues.MoveNext();
						}
		
		
		
						rsAnswers.MoveNext();
					} 
	
	
	
					rsQuestions.MoveNext();
				} 


			}

			//================================================================================
			//Function:  gDeleteElement
			//--------------------------------------------------------------------------------'
			//Purpose:   löscht ein Element, wie z.B. eine Frage
			//
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   24.05.2004 23:35:48
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static void gDeleteElement(ClientInfo oClientInfo, string sType, int lEntityId)
			{
				string sEntityName = "";
				if (sType.Equals("QUESTION")) 
				{
					sEntityName = svmEnums.entQuestions;

				}
				else if (sType.Equals("ANSWER")) 
				{
					sEntityName = svmEnums.entAnswers;
				}
				else if (sType.Equals("ANSWERVALUE"))
				{
					sEntityName = svmEnums.entAnswerValues;
				}
				else throw new Exception("Invalid Element-Type: \"" + sType + "\".");


				DefaultEntity entity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);
				entity.gLoad(oClientInfo, Convert.ToString(lEntityId));

				entity.gDelete(oClientInfo);

			}

			//================================================================================
			//Function:  gsGetSurveyValue
			//--------------------------------------------------------------------------------'
			//Purpose:   gets a value from a survey; if the value doesn't exist in the
			//           survey, this function iterativley goes up all parent surveygroups
			//           and tries to get the value from the first parent, which has the value
			//           set.
			//
			//           example: gsGetSurveyValue(cl, "svy_baseurl", "svg_baseurl", svy_id)
			//
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   24.05.2004 23:35:48
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static string gsGetSurveyValue(ClientInfo oClientInfo, string sSurveyField, string sAlternativeSurveyGroupField, int lSvy_id, int lSvg_id)
			{


				return gsGetSurveyValue(oClientInfo.sConnStr, sSurveyField, sAlternativeSurveyGroupField, lSvy_id, lSvg_id);

			}

			public static string gsGetSurveyValue(string sConnStr, string sSurveyField, string sAlternativeSurveyGroupField, int lSvy_id, int lSvg_id)
			{


				if (lSvg_id == -1)
				{
	
					string sValue = DataMethods.gsGetDBValue(sConnStr, "tdSurveys", sSurveyField, "svy_id=" + lSvy_id, "");
	
					if (!Functions.IsEmptyString(sValue))
					{
						return sValue;
					}
					else
					{
		
						int svg_id = DataMethods.glGetDBValue(sConnStr, "tdSurveys", "svy_svg_id", "svy_id=" + lSvy_id, 0);
		
						return gsGetSurveyValue(sConnStr, sSurveyField, sAlternativeSurveyGroupField, lSvy_id, svg_id);
		
					}
				}
				else
				{
	
					string sValue = DataMethods.gsGetDBValue(sConnStr, "tdSurveyGroups", sAlternativeSurveyGroupField, "svg_id=" + lSvg_id, "");
	
					if (!Functions.IsEmptyString(sValue))
					{
						return sValue;
					}
					else
					{
						int lParentsvg_id = DataMethods.glGetDBValue(sConnStr, "tdSurveyGroups", "svg_parentid", "svg_id=" + lSvg_id, 0);
		
		
						if (lParentsvg_id == 0)
						{
							return "";
						}
						else
						{
							return gsGetSurveyValue(sConnStr, sSurveyField, sAlternativeSurveyGroupField, lSvy_id, lParentsvg_id);
						}
		
		
					}
	
				}



			}


			/*
			//================================================================================
			//Function:  gNewElement
			//--------------------------------------------------------------------------------'
			//Purpose:   adds a new question, answer, answervalue to a survey
			//			 default-values are set here
			//--------------------------------------------------------------------------------'
			//Params:	sType: give here:
			//			QUESTION
			//			ANSWER
			//			ANSWERVALUE
			//--------------------------------------------------------------------------------'
			//Returns:  the id of the newly created element
			//--------------------------------------------------------------------------------'
			//Created:   31.05.2005 23:35:48
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			*/
			public static int glNewElement(ClientInfo oClientInfo, string sType, int lParentId) 
			{

				if (sType.Equals("QUESTION")) 
				{

					DefaultEntity oQuestion = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entQuestions);
					oQuestion.gNew(oClientInfo);
					oQuestion.oFields["qst_text"].sValue = "Text bitte bearbeiten!";
					oQuestion.oFields["qst_svy_id"].lValue = lParentId;
					oQuestion.oFields["qst_index"].lValue = 1000;
					oQuestion.gSave(oClientInfo);
					return oQuestion.oFields["qst_id"].lValue;

				}
				else if (sType.Equals("ANSWER")) 
				{
					DefaultEntity oAnswer = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entAnswers);
					oAnswer.gNew(oClientInfo);
					oAnswer.oFields["ans_resultDbField"].sValue = "BITTE_BEARBEITEN";
					oAnswer.oFields["ans_qst_id"].lValue = lParentId;
					oAnswer.oFields["ans_aty_id"].sValue = "FREETEXT";
					oAnswer.oFields["ans_lines"].lValue = 1;
					oAnswer.oFields["ans_checked"].bValue = false;
					oAnswer.oFields["ans_multiline"].bValue = true;
					oAnswer.oFields["ans_index"].lValue = 1000;
					oAnswer.gSave(oClientInfo);
					return oAnswer.oFields["ans_id"].lValue;
				}
				else if (sType.Equals("ANSWERVALUE"))
				{
					DefaultEntity oAnswerValue = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entAnswerValues);
					oAnswerValue.gNew(oClientInfo);
					oAnswerValue.oFields["val_text"].sValue = "Text bitte bearbeiten!";
					oAnswerValue.oFields["val_ans_id"].lValue = lParentId;
					oAnswerValue.oFields["val_correct"].bValue = false;
					oAnswerValue.oFields["val_index"].lValue = 1000;
					oAnswerValue.gSave(oClientInfo);
					return oAnswerValue.oFields["val_id"].lValue;
				}
				else throw new Exception("Invalid Element-Type: \"" + sType + "\".");

			}

		}


	}
}
