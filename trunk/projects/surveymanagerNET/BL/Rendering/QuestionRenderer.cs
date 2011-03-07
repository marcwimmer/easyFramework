using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Addons;
using easyFramework.Addons.efMathMLWrapper;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     QuestionRenderer

	//--------------------------------------------------------------------------------'
	//Module:    QuestionRenderer.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   used to render a question
	//--------------------------------------------------------------------------------'
	//Created:   07.05.2004 15:13:36
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	namespace Rendering
	{
			


		public class QuestionRenderer
		{
	
	
			//================================================================================
			//Private Fields:
			//================================================================================
	
			private Recordset mRsQuestion;
	
			//================================================================================
			//Public Consts:
			//================================================================================
			//css-Definitionen:
			public string CLASS_SVY_QUESTIONTEXT = "SVY_QUESTIONTEXT";
	
			//================================================================================
			//Public Properties:
			//================================================================================
	
			//================================================================================
			//Public Events:
			//================================================================================
	
			//================================================================================
			//Public Methods:
			//================================================================================
			public QuestionRenderer(Recordset rs) 
			{
				mRsQuestion = rs;
			}
	
	
	
			//================================================================================
			//Sub:       gRender
			//--------------------------------------------------------------------------------'
			//Purpose:   gets html of a question
			//--------------------------------------------------------------------------------'
			//Params:    rsSurvye - survey-recordset
			//           sConnstr - connstr to svm-db
			//           output - rendering target
			//           lQuestionNumber - number of the question within the survey
			//           lQuestionCount - number of questions in the survey
			//--------------------------------------------------------------------------------'
			//Created:   14.05.2004 12:42:28
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public void gRender (Recordset rsSurvey, string sConnstr, ref FastString output, int lQuestionNumber, int lQuestionCount, System.Web.HttpApplicationState oAppState)
			{
		
		
				//--------if the value "svy_layout_column" is set on survey-stage, then
				//        several columns have to be made------------------
		
				if (rsSurvey["svy_layout_columns"].lValue == 0)
				{
					output.Append("<tr>" + "\n");
				}
				else
				{
			
					//-------decide, wether to start a new row or not------------
					//if it is the first question of a row, then start a new row:
					if ((lQuestionNumber - 1) % rsSurvey["svy_layout_columns"].lValue == 0)
					{
						output.Append("<tr>" + "\n");
					}
			
				}
		
		
		
		
				if (rsSurvey["svy_svt_id"].sValue == "TEST")
				{
					output.Append("<td width=\"40\" valign=\"top\" >");
					//-----display a nice-number:----------
					output.Append("<table width=\"40\" height=\"30\"><tr><td bgcolor=\"#DDDDDD\" align=\"right\"><b><font size=\"+2\">" + lQuestionNumber + ".</font></b></td></tr></table>");
					output.Append("</td>" + "\n");
					output.Append("<td width=\"99%\" valign=\"top\">" + "\n"); //CORRESPONDING #A88282
				}
				else
				{
					output.Append("<td valign=\"top\"><p class=\"voteQuestiontext\">"); //CORRESPONDING #A88282
				}
		
		
				output.Append("<table width=\"100%\">" + "\n"); //CORRESPONDING #A000018
				output.Append("<tr>" + "\n");
		
				//----if description on the left, start a td #CORRESPONDING_A007------
				bool bDescriptionOnTheLeft = false;
				switch (mRsQuestion["qst_layout_description_on_the_left"].lValue)
				{
					case 0:
				
						bDescriptionOnTheLeft = rsSurvey["svy_layout_description_on_the_left"].bValue;
						break;
					case 1:
				
						bDescriptionOnTheLeft = true;
						break;
					case 2:
				
						bDescriptionOnTheLeft = false;
						break;
				}
		
				//-----------beginning td of question-text---------  'CORRESPONDING #A0000127
				output.Append("<td ");
				if (bDescriptionOnTheLeft == true)
				{
					output.Append("width=\"80\">");
				}
				else
				{
					output.Append(">");
				}
		
				//-----output questiontext------
				if (mRsQuestion["qst_html_before"].sValue.Length > 0)
					output.Append(mRsQuestion["qst_html_before"].sValue);
				output.Append("<span class=\"" + CLASS_SVY_QUESTIONTEXT + "\">");
				output.Append(mRsQuestion["qst_text"].sValue);
				output.Append("</span>");
				output.Append("<br/>");

				//-----------show mathematical formular, if needed-------------------
				if (mRsQuestion["qst_mathml"].sValue.Length > 0) 
				{
					string sOutputFilename = rsSurvey["svy_id"].sValue + "_" + mRsQuestion["qst_id"].sValue;
					efEnvironment oEnv = efEnvironment.goGetEnvironment(oAppState);
					string sOutputFolder = oEnv.gsTempFolder;


					efMathMLRenderer.gRenderXml(mRsQuestion["qst_mathml"].sValue, sOutputFolder, sOutputFilename, System.Drawing.Imaging.ImageFormat.Gif);

					string sUrlPathImage = oEnv.gsTempFolderUrl + sOutputFilename + ".gif";
					output.Append("<img src=\"" + sUrlPathImage + "\">");
					output.Append("<br><br>");

					
				}
		
				
				//----if description on the left, end the td #CORRESPONDING_A007------
				if (bDescriptionOnTheLeft == true)
				{
					output.Append("</td><td width=\"5\">&nbsp;</td>");
					output.Append("<td class=\"voteAnswer\" align=\"left\">" + "\n");
				}
		
		
				Recordset rsAnswers = DataMethods.gRsGetTable(sConnstr, "tdAnswers", "*", "ans_qst_id=" + mRsQuestion["qst_id"].sValue, "", "", "ans_index");
		
		
		
		
				while (! rsAnswers.EOF)
				{
			
			
					AnswerRenderer oAnswerRenderer = new AnswerRenderer(rsAnswers);
					oAnswerRenderer.gRender(sConnstr, output, oAppState);
			
			
			
					rsAnswers.MoveNext();
				}
		
				output.Append("</td>" + "\n");
				output.Append("</tr>" + "\n");
				output.Append("</table>" + "\n"); //CORRESPONDING #A000018
		
		
				//-----------insert after html---------------------
				output.Append(mRsQuestion["qst_html_after"].sValue);
		
		
				output.Append("</td>" + "\n"); //td of question CORRESPONDING #A88282
		
				if (rsSurvey["svy_layout_columns"].lValue == 0)
				{
					output.Append("</tr>" + "\n");
				}
				else
				{
			
					//-------decide, wether to end a row or not------------
					//if it is the last question of a row, then end the row:
					if ((lQuestionNumber - 1) % rsSurvey["svy_layout_columns"].lValue == rsSurvey["svy_layout_columns"].lValue - 1)
					{
						output.Append("</tr>" + "\n");
					}
			
				}
		
		
				if (rsSurvey["svy_svt_id"].sValue == "TEST")
				{
					output.Append("<tr>");
					output.Append("<td colspan=\"2\">");
					output.Append("<hr>");
					output.Append("</td>");
					output.Append("</tr>");
			
			
				}
		
		
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
