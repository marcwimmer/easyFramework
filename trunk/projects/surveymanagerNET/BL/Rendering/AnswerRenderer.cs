using System;
using System.Web;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Addons.efMathMLWrapper;


namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     AnswerRenderer

	//--------------------------------------------------------------------------------'
	//Module:    AnswerRenderer.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   used to render an answer
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
	
		public class AnswerRenderer
		{
		
			//================================================================================
			//Private Fields:
			//================================================================================
			private Recordset mRsAnswer;
		
			//================================================================================
			//Public Consts:
			//================================================================================
			//css-Definitionen:
			public string CLASS_SVY_QUESTIONTEXT = "svy_questiontext";
	
			//================================================================================
			//Public Properties:
			//================================================================================
		
			//================================================================================
			//Public Events:
			//================================================================================
		
			//================================================================================
			//Public Methods:
			//================================================================================
			public AnswerRenderer(Recordset rsAnswer) 
			{
			
				mRsAnswer = rsAnswer;
			}
		
			public void gRender (string sConnstr, FastString Output, HttpApplicationState oAppState)
			{
			
				string sAnswerType = mRsAnswer["ans_aty_id"].sValue;
			
				//---------falls ein HTML-before angegeben ist, dieses zeigen---------------
				if (mRsAnswer["ans_html_before"].sValue.Length > 0) 
				{
						Output.Append(mRsAnswer["ans_html_before"].sValue);
				}

				
				//------------------individuell Antworttyp rendern-----------------------
				switch (Functions.UCase(sAnswerType))
				{
				
					case "FREETEXT":
				
						mRender_FreetextAnswer(sConnstr, Output);
						break;
				
					case "SINGLECHOICE":
				
						mRender_SingleChoiceAnswer(sConnstr, Output, oAppState);
						break;
				
					case "MULTIPLECHOICE":
				
						mRender_MultipleChoiceAnswer(sConnstr, Output, oAppState);
						break;
				
					case "GAPTEXT":
				
						mRender_GapTextAnswer(sConnstr, Output);
						break;
				
					case "CHECKBOX":
						mRender_CheckBoxAnswer(sConnstr, Output);
						break;

					default:
				
						throw (new efException("Unhandled answer-type \"" + sAnswerType + "\"."));
						
				
				}


				//---------falls ein HTML-after angegeben ist, dieses zeigen---------------
				if (mRsAnswer["ans_html_after"].sValue.Length > 0) 
				{
					Output.Append(mRsAnswer["ans_html_after"].sValue);
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
			private void mRender_FreetextAnswer (string sConnstr, FastString output)
			{
		
		
				if (mRsAnswer["ans_multiline"].bValue == true)
				{
			
					efHtmlTextarea efHtmlInput = new efHtmlTextarea();
			
					efHtmlInput["name"].sValue = PublicEnums.efInputName_Answer + mRsAnswer["ans_id"].sValue;
					efHtmlInput["cols"].sValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(50);
					efHtmlInput["rows"].sValue = mRsAnswer["ans_lines"].sValue;
			
					efHtmlInput.gRender(output, 1);
			
				}
				else
				{
			
					efHtmlInput oHtmlInput = new efHtmlInput();
					oHtmlInput["name"].sValue = PublicEnums.efInputName_Answer + mRsAnswer["ans_id"].sValue;
					oHtmlInput["type"].sValue = "text";
			
					oHtmlInput.gRender(output, 1);
			
				}
		
		
		
		
			}
	
			private void mRender_CheckBoxAnswer (string sConnstr, FastString output)
			{
		
				Recordset rsAnswer = DataMethods.gRsGetTable(sConnstr, "tdAnswers", "*", "ans_id=" + mRsAnswer["ans_id"].sValue, "", "", "");
				
				efHtmlInput oHtmlInput = new efHtmlInput();
				oHtmlInput["type"].sValue = "checkbox";
				oHtmlInput["name"].sValue = PublicEnums.efInputName_Answer + mRsAnswer["ans_id"].sValue;

				if (rsAnswer["ans_checked"].bValue) 
					oHtmlInput["checked"].sValue = "true";
		
				oHtmlInput.gRender(output, 1);

			}


			private void mRender_SingleChoiceAnswer (string sConnstr, FastString output, HttpApplicationState oAppState)
			{
		
				Recordset rsAnswerValues = DataMethods.gRsGetTable(sConnstr, "tdAnswerValues", "*", "val_ans_id=" + mRsAnswer["ans_id"].sValue, "", "", "val_index");
		
		
				if (mRsAnswer["ans_multiline"].bValue == true)
				{
			
					while (! rsAnswerValues.EOF)
					{
				
						efHtmlInput oHtmlRadioButton = new efHtmlInput();
						oHtmlRadioButton["type"].sValue = "RADIO";
						oHtmlRadioButton["name"].sValue = PublicEnums.efInputName_Answer + mRsAnswer["ans_id"].sValue;
						oHtmlRadioButton["value"].sValue = PublicEnums.efInputName_AnswerValue + rsAnswerValues["val_id"].sValue;
						if (rsAnswerValues["val_default"].bValue) 
							oHtmlRadioButton["checked"].sValue = "true";
				
				
						efHtmlBr oHtmlBr = new efHtmlBr();
				
						oHtmlRadioButton.gRender(output, 1);
						if (!Functions.IsEmptyString(rsAnswerValues["val_html"].sValue))
						{
					
							output.Append(rsAnswerValues["val_html"].sValue);
							mRenderMathFormula(output, rsAnswerValues, oAppState);
					
						}
						else
						{
							efHtmlTextNode oHtmlText = new efHtmlTextNode();
							oHtmlText.sText = rsAnswerValues["val_text"].sValue;
							oHtmlText.gRender(output, 1);
							mRenderMathFormula(output, rsAnswerValues, oAppState);
							oHtmlBr.gRender(output, 1);
						}
				
			
						rsAnswerValues.MoveNext();
					} 
			
				}
				else
				{
			
					efHtmlSelect oHtmlSelect = new efHtmlSelect();
					oHtmlSelect.gAddOption("", "");
			
					oHtmlSelect["name"].sValue = PublicEnums.efInputName_Answer + mRsAnswer["ans_id"].sValue;
					if (!Functions.IsEmptyString(mRsAnswer["ans_lines"].sValue))
					{
						oHtmlSelect["size"].sValue = mRsAnswer["ans_lines"].sValue;
					}
			
					while (! rsAnswerValues.EOF)
					{
				
						oHtmlSelect.gAddOption(PublicEnums.efInputName_AnswerValue + rsAnswerValues["val_id"].sValue, rsAnswerValues["val_text"].sValue, rsAnswerValues["val_default"].bValue);
				
						rsAnswerValues.MoveNext();
					}
			
					oHtmlSelect.gRender(output, 1);
				}
		
		
			}
	
			private void mRender_MultipleChoiceAnswer (string sConnstr, FastString output, HttpApplicationState oAppState)
			{
		
				Recordset rsAnswerValues = DataMethods.gRsGetTable(sConnstr, "tdAnswerValues", "*", "val_ans_id=" + mRsAnswer["ans_id"].sValue, "", "", "val_index");
		
		
				//If mRsAnswer["ans_multiline"].bValue = True Then ---- ignore multiline
		
		
		
				while (! rsAnswerValues.EOF)
				{
			
					efHtmlInput oHtmlCheckbox = new efHtmlInput();
					oHtmlCheckbox["type"].sValue = "CHECKBOX";
					oHtmlCheckbox["name"].sValue = PublicEnums.efInputName_Answer + mRsAnswer["ans_id"].sValue;
					oHtmlCheckbox["value"].sValue = PublicEnums.efInputName_AnswerValue + rsAnswerValues["val_id"].sValue;
					if (rsAnswerValues["val_default"].bValue) 
						oHtmlCheckbox["checked"].sValue = "true";
				
			
					oHtmlCheckbox.gRender(output, 1);
			
					if (!Functions.IsEmptyString(rsAnswerValues["val_html"].sValue))
					{
				
						output.Append(rsAnswerValues["val_html"].sValue);
				
					}
					else
					{
						efHtmlTextNode oHtmlText = new efHtmlTextNode();
						oHtmlText.sText = rsAnswerValues["val_text"].sValue;
						oHtmlText.gRender(output, 1);
						
				
					}


					mRenderMathFormula(output, rsAnswerValues, oAppState);

					if (Functions.IsEmptyString(rsAnswerValues["val_html"].sValue) )
					{
						efHtmlBr oHtmlBr = new efHtmlBr();
						oHtmlBr.gRender(output, 1);
					}

					rsAnswerValues.MoveNext();
				} 
		
		
			}
	
			private void mRender_GapTextAnswer (string sConnstr, FastString output)
			{
		
				//the amount of gaps and answervalues have to match:
				Recordset rsAnswerValues = DataMethods.gRsGetTable(sConnstr, "tdAnswerValues", "*", "val_ans_id=" + mRsAnswer["ans_id"].sValue, "", "", "val_index");
		
		
		
		
				efHtmlTextNode oHtmlTextNode = new efHtmlTextNode();
				string sGapText = mRsAnswer["ans_gap_txt"].sValue;
		
				if (rsAnswerValues.EOF)
				{
					gRenderPreviewErrorMessage("Es wird mindestens ein Antwortwert bentigt!", output);
					return;
				}
		
		
				int lCounter = 1;
				while (! rsAnswerValues.EOF)
				{
			
					efHtmlInput oHtmlInput = new efHtmlInput();
					oHtmlInput["type"].sValue = "text";
					oHtmlInput["name"].sValue = PublicEnums.efInputName_GapTextField + rsAnswerValues["val_id"].sValue;
			
					string sParameter = "...";
			
					if (Functions.InStr(sGapText, sParameter) > 0)
					{
				
						FastString oFastString = new FastString();
						oHtmlInput.gRender(oFastString, 1);
				
						sGapText = Functions.Replace(sGapText, sParameter, oFastString.ToString(), 1, 1);
					}
					else
					{
						gRenderPreviewErrorMessage("Anzahl Antwortwerte und Parameter stimmen nicht berein! Bitte berprfen.", output);
						return;
					}
			
			
					lCounter += 1;
					rsAnswerValues.MoveNext();
				} 
		
				output.Append(sGapText);
		
		
		
		
			}
	

			/// <summary>
			/// renders the math-formula of an answer-value 
			/// </summary>
			private void mRenderMathFormula(FastString output, Recordset rsAnswerValue, HttpApplicationState oAppState) 
			{
				//-----------show mathematical formular, if needed-------------------
				if (rsAnswerValue["val_mathml"].sValue.Length > 0) 
				{
					string sOutputFilename = "ans_" + rsAnswerValue["val_ans_id"].sValue + "_" + rsAnswerValue["val_id"].sValue;
					efEnvironment oEnv = efEnvironment.goGetEnvironment(oAppState);
					string sOutputFolder = oEnv.gsTempFolder;

					try 
					{

						efMathMLRenderer.gRenderXml(rsAnswerValue["val_mathml"].sValue, sOutputFolder, sOutputFilename, System.Drawing.Imaging.ImageFormat.Gif);
					}
					catch (Exception ex) 
					{
						string sMsg = "Error at rendering Math-Formular for Answer-Value \"" + rsAnswerValue["val_text"].sValue + "\" " + ex.Message;
						throw new efException(sMsg);
					}
					string sUrlPathImage = oEnv.gsTempFolderUrl + sOutputFilename + ".gif";
					output.Append("<img src=\"" + sUrlPathImage + "\">");
				
					
				}

			}
			
	
	
			//================================================================================
			//Sub:       gRenderPreviewErrorMessage
			//--------------------------------------------------------------------------------'
			//Purpose:   displays an error-message in the result-html
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   08.05.2004 09:15:04
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			private void gRenderPreviewErrorMessage (string sMsg, FastString oOutput)
			{
		
		
				efHtmlUnparsedHtml oHtmlUnparsed = new efHtmlUnparsedHtml();
		
				oHtmlUnparsed.sHtml = "<font color=\"red\" size=\"+2\">" + sMsg + "</font>";
		
				oHtmlUnparsed.gRender(oOutput, 1);
		
			}
	
		}



	}

}
