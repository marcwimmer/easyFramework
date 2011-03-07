using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;


namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     SurveyRenderer

	//--------------------------------------------------------------------------------'
	//Module:    SurveyRenderer.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   used to get the end-user HTML
	//--------------------------------------------------------------------------------'
	//Created:   07.05.2004 14:11:01
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	namespace Rendering
	{

		public class SurveyRenderer
		{
	
			//================================================================================
			//Private Fields:
			//================================================================================
			private int mlsvy_id;
	
			//================================================================================
			//Public Consts:
			//================================================================================
	
			//================================================================================
			//Public Properties:
			//================================================================================
			public int lsvy_id
			{
				get
				{
					return mlsvy_id;
				}
				set
				{
					mlsvy_id = value;
				}
			}
			//================================================================================
			//Public Events:
			//================================================================================
	
			//================================================================================
			//Public Methods:
			//================================================================================
	
			//================================================================================
			//Function:  gRender
			//--------------------------------------------------------------------------------'
			//Purpose:   gets the result HTML
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   07.05.2004 14:13:30
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public string gRender(string sConnStr, bool bIsPreview, System.Web.HttpApplicationState oAppState)
			{
		
		
				Recordset rsSurvey = DataMethods.gRsGetTable(sConnStr, "tdSurveys", "*", "svy_id=" + mlsvy_id, "", "", "");
		
				if (rsSurvey.EOF)
				{
					throw (new efException("Survey with svy_id=\"" + mlsvy_id + "\" not found!"));
				}
		
				Recordset rsQuestions = DataMethods.gRsGetTable(sConnStr, "tdQuestions", "*", "qst_svy_id=" + mlsvy_id, "", "", "qst_index");
		
				FastString oResult = new FastString();


				//----------------------------------if there is a limit of questions, then randomly remove questions---------------------------
				if (rsSurvey["svy_zufallsfragen_poolgroesse"].lValue > 0 && !bIsPreview) 
				{
					int maxFragen = rsSurvey["svy_zufallsfragen_poolgroesse"].lValue;
					Random r = new Random();
					while (rsQuestions.lRecordcount > maxFragen ) 
					{
						
						int lRandomNumber = r.Next(rsQuestions.lRecordcount);

						rsQuestions.MoveFirst();
						rsQuestions.Move(lRandomNumber);
						rsQuestions.Delete();


					}
				}
				rsQuestions.MoveFirst();
		
		
				//------------------output the introduction-memo-----------------
				oResult.Append(rsSurvey.oFields["svy_intro_html"].sValue);
		
		
				oResult.Append("<table border=\"0\">");
		

				//-----------alle verwendeten Fragen-Ids in einem hidden-field speichern, damit
				//dann auch nur diese Fragen später ausgewertet werden-------------------
				string sQuestionIdsString = "";

				int lQuestionNumber = 0;
				while (! rsQuestions.EOF)
				{
					lQuestionNumber += 1;
			
					QuestionRenderer oQuestRenderer = new QuestionRenderer(rsQuestions);
					

					oQuestRenderer.gRender(rsSurvey, sConnStr, ref oResult, lQuestionNumber, rsQuestions.lRecordcount, oAppState);
			
					sQuestionIdsString += rsQuestions["qst_id"].sValue + ",";
			
			
					rsQuestions.MoveNext();
				}
				if (sQuestionIdsString.Length > 0) sQuestionIdsString = sQuestionIdsString.Substring(0, sQuestionIdsString.Length -1);
		
				oResult.Append("</table>");
		
				//------------------output the extroduction-memo-----------------
				oResult.Append(rsSurvey["svy_extro_html"].sValue);
		
		
				//----------------------add a hidden-field for the survey-id-----------------
				efHtmlInput oHtmlHidden = new efHtmlInput();
				oHtmlHidden["type"].sValue = "hidden";
				oHtmlHidden["name"].sValue = "svy_id";
				oHtmlHidden["value"].sValue = DataConversion.gsCStr(lsvy_id);
				oHtmlHidden.gRender(oResult, 1);

		
				//----------------------add a hidden-field for the used questions-----------------
				efHtmlInput oHtmlQuestions = new efHtmlInput();
				oHtmlQuestions["type"].sValue = "hidden";
				oHtmlQuestions["name"].sValue = svmEnums.efsFieldUsedQuestions;
				oHtmlQuestions["value"].sValue = sQuestionIdsString;
				oHtmlQuestions.gRender(oResult, 1);

				//--------------------return result----------------------
				return oResult.ToString();
		
			}
			//================================================================================
			//Sub:       New
			//--------------------------------------------------------------------------------'
			//Purpose:   constructor
			//--------------------------------------------------------------------------------'
			//Params:    the survey-id
			//--------------------------------------------------------------------------------'
			//Created:   07.05.2004 14:11:47
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public SurveyRenderer(int lsvy_id) 
			{
				mlsvy_id = lsvy_id;
			}
			public SurveyRenderer(string sConnstr, string sExternalSurveyID) 
			{
		
				string sSvy_id = DataMethods.gsGetDBValue(sConnstr, "tdSurveys", "svy_id", "svy_externalid='" + DataTools.SQLString(sExternalSurveyID) + "'", "");
		
				if (Functions.IsEmptyString(sSvy_id))
				{
					throw (new efException("Survey with id \"" + sExternalSurveyID + "\" wasn't found!"));
				}
		
				mlsvy_id = DataConversion.glCInt(sSvy_id, 0);
		
			}
	
			//================================================================================
			//Protected Properties:
			//================================================================================
	
			//================================================================================
			//Protected Methods:
			//================================================================================
	
			//================================================================================
			//Private Consts:
			//================================================================================
	
			//================================================================================
			//Private Fields:
			//================================================================================
	
			//================================================================================
			//Private Methods:
			//================================================================================
	
	
	
		}
	}


}
