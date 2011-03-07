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
	//Class:     surveyProcess

	//--------------------------------------------------------------------------------'
	//Module:    surveyProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   storing the results of a survey
	//--------------------------------------------------------------------------------'
	//Created:   12.05.2004 09:26:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class surveyProcess : System.Web.UI.Page
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
	
		protected efArrayList aoErrorlist;
	
		protected string sCssOfSurvey;
	
		protected string sSurveyTitle;
	
		protected string sResultHtml;
	
		protected string sBorderHtmlBegin;
		protected string sBorderHtmlEnd;

		public string gsListItems() 
		{
			FastString oResult = new FastString();
            if (aoErrorlist.Count > 0) {
				oResult.Append("<ul>");
				
				for (int i = 0; i < this.aoErrorlist.Count; i++) {
					oResult.Append("<li>");
					oResult.Append(Convert.ToString(this.aoErrorlist[i]));
					oResult.Append("</li>");
			}

			oResult.Append("</ul>");
		
			oResult.Append("<button type=\"button\" onclick=\"history.back()\">Eingabe wiederholen</button>");
			
			} else {
				oResult.Append(sResultHtml);
			}

			return oResult.ToString();
		}
	
	
		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
            /* //TODO reactivate
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			string sConnstr_Svm;
			string sConnstr_Results;
		
		
			sConnstr_Svm = efEnvironment.goGetEnvironment(Application).gsConnStr;
			sConnstr_Results = Submission.DBConn.gsGetConnStr_ResultDatabase(sConnstr_Svm);
		
		
			XmlDocument oXmlResult;
		
			int lSessionID = Submission.Submissioner.glSubmit(Request, Application, sConnstr_Svm, sConnstr_Results, ref aoErrorlist);
		
			if (aoErrorlist.Count > 0)
			{
				return;
			}
		
		
			//------------------------get css------------------------
			Recordset rsSurvey;
			Recordset rsSurveyGroup;
			int lSvy_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(Request["svy_id"], 0);
			rsSurvey = DataMethods.gRsGetTable(sConnstr_Svm, "tdSurveys", "svy_id, svy_name, svy_css, svy_svg_id, svy_svt_id", "svy_id=" + lSvy_id, "", "", "");
			if (! rsSurvey.EOF)
			{
				rsSurveyGroup = DataMethods.gRsGetTable(sConnstr_Svm, "tdSurveyGroups", "svg_css", "svg_id=" + rsSurvey["svy_svg_id"].sValue, "", "", "");
			}
			lSvy_id = rsSurvey["svy_id"].lValue;
		
			//---------get css------------
			sCssOfSurvey = Preparation.SurveyEditor.gsGetSurveyValue(sConnstr_Svm, "svy_css", "svg_css", lSvy_id, -1);
		
		
			//----------------get border-html -----------------------
			sBorderHtmlBegin = Preparation.SurveyEditor.gsGetSurveyValue(sConnstr_Svm, "svy_border_html_begin", "svg_border_html_begin", lSvy_id, -1);
			sBorderHtmlEnd = Preparation.SurveyEditor.gsGetSurveyValue(sConnstr_Svm, "svy_border_html_end", "svg_border_html_end", lSvy_id, -1);
		
			//---------replace placeholders in border-html---------------
			sBorderHtmlBegin = Rendering.HTMLPlaceHolders.gsGetReplacedHtml(sConnstr_Svm, lSvy_id, sBorderHtmlBegin);
			sBorderHtmlEnd = Rendering.HTMLPlaceHolders.gsGetReplacedHtml(sConnstr_Svm, lSvy_id, sBorderHtmlEnd);
		
			//------------------survey title----------------------
			sSurveyTitle = rsSurvey["svy_name"].sValue;
		
		
		
			//----------------- get result html ----------------------
			int lPub_id = Preparation.Publications.goCurrentActivePublication(sConnstr_Svm, lSvy_id)["pub_id"].lValue;
			switch (Functions.UCase(rsSurvey["svy_svt_id"].sValue))
			{
				case "PROFITPLAY":
				
					sResultHtml = msProfitPlayResultToHtml(lSvy_id, sConnstr_Svm);
					break;
				case "TEST":
				
					oXmlResult = Analysis.goXmlGetTestResults(sConnstr_Svm, sConnstr_Results, lPub_id, lSessionID);
					sResultHtml = msTestResultToHtml(Analysis.goXmlGetTestResults(sConnstr_Svm, sConnstr_Results, lPub_id, lSessionID), rsSurvey["svy_id"].lValue, sConnstr_Svm);
					break;
				case "VOTE":
				
					sResultHtml = msVoteResultToHtml(Analysis.goXmlGetCulmulativeResults(sConnstr_Svm, sConnstr_Results, lPub_id), lSvy_id, sConnstr_Svm);
					break;
			}
		
		
			//-------------- specific html, depending on request----------
			if (Request["closeButton"] == "1")
			{
				sResultHtml += "<button type=\"button\" onclick=\"window.close()\">Schlie&szlig;en</button>";
			}
		*/
		
		}
	
	
	
		//================================================================================
		//Function:  msTestResultToHtml
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the result-xml of a test into a html
		//--------------------------------------------------------------------------------'
		//Params:    the xml
		//--------------------------------------------------------------------------------'
		//Returns:   html
		//--------------------------------------------------------------------------------'
		//Created:   14.05.2004 00:01:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msTestResultToHtml(XmlDocument oXmlResult, int lSvy_id, string sConnstr_svm)
		{
		
			
			
			FastString oResult = new FastString();
		
		
			bool bPassed;
			bPassed = easyFramework.Sys.ToolLib.DataConversion.gbCBool(oXmlResult.selectSingleNode("/testresults/passed").sText);
		
			if (bPassed)
			{
				oResult.Append(DataMethods.gsGetDBValue(sConnstr_svm, "tdSurveys", "svy_congratulation_html", "svy_id=" + lSvy_id, ""));
			}
			else
			{
				oResult.Append(DataMethods.gsGetDBValue(sConnstr_svm, "tdSurveys", "svy_sorry_html", "svy_id=" + lSvy_id, ""));
			}
		
			double dAchievedPercents = easyFramework.Sys.ToolLib.DataConversion.gdCDbl(oXmlResult.sGetValue("achievedPercents", ""), 0);
			oResult.Append("<br><br>erreichte Prozent: " + 
				Functions.FormatNumber(dAchievedPercents, 2, 
				Functions.efEnumTriState.useDefault, 
				Functions.efEnumTriState.useDefault, 
				Functions.efEnumTriState.useDefault) + "%");
		
		
			//---------render the corrected answers-------------------
			XmlNodeList nlQuestions = oXmlResult.selectNodes("//question");
		
			for (int i = 0; i <= nlQuestions.lCount - 1; i++)
			{
			
				oResult.Append("<hr>");
				string sNumber = easyFramework.Sys.ToolLib.DataConversion.gsCStr(i + 1);
			
				oResult.Append("<table width=\"100%\"><tr><td width=\"1\">");
				oResult.Append("<table width=\"40\" height=\"30\"><tr><td bgcolor=\"#DDDDDD\" align=\"right\"><b><font size=\"+2\">" + sNumber + ".</font></b></td></tr></table>");
				oResult.Append("</td><td>");
				oResult.Append(nlQuestions[i].selectSingleNode("questiontext").sText);
				oResult.Append("</td>");
				oResult.Append("</tr>");
				oResult.Append("</table>");
			
			
			
				oResult.Append("<br>");
			
				//------------show the gap-text, if there is a gap-text------------
				if (nlQuestions[i].selectSingleNode("gaptext") != null)
				{
					oResult.Append(nlQuestions[i].selectSingleNode("gaptext").sText + "<br><br>");
				}
			
				//show the free answer and suggestion:
				if (nlQuestions[i].selectSingleNode("answer[@type='FREETEXT']") != null)
				{
					oResult.Append("<b>Ihre Antwort:</b><br>");
					oResult.Append(nlQuestions[i].selectSingleNode("answer[@type='FREETEXT']/texts/yourValue").sText + "<br><br>");
					oResult.Append("<b>Lösungsvorschlag:</b><br>");
					oResult.Append(nlQuestions[i].selectSingleNode("answer[@type='FREETEXT']/texts/suggestion").sText + "<br><br>");
				}

				
				XmlNodeList nlAnswers = nlQuestions[i].selectNodes("answer");
			
				for (int iAnswer = 0; iAnswer <= nlAnswers.lCount - 1; iAnswer++)
				{
				
					//--------good answers---------
					XmlNodeList nlGoodAnswers = nlAnswers[iAnswer].selectNodes("good");
					for (int y = 0; y <= nlGoodAnswers.lCount - 1; y++)
					{
					
						oResult.Append("<font color=\"green\">");
						oResult.Append("<b>Richtig: </b>");
						oResult.Append(nlGoodAnswers[y].selectSingleNode("yourValue").sText);
						//Hinweistext anzeigen, falls vorhanden:
						if (nlGoodAnswers[y].sGetValue("notetext") != "") oResult.Append("<br/>");
						oResult.Append(nlGoodAnswers[y].sGetValue("notetext"));
						oResult.Append("</font>");
					
						oResult.Append("<br>");
					}
				
					//--------false answers---------
					XmlNodeList nlFalseAnswers = nlAnswers[iAnswer].selectNodes("false");
					for (int y = 0; y <= nlFalseAnswers.lCount - 1; y++)
					{
					
						string sYourValue = "";
						string sCorrectValue = "";
						sYourValue = nlFalseAnswers[y].selectSingleNode("yourValue").sText;
						if (nlFalseAnswers[y].selectSingleNode("correctValue") == null == false)
						{
							sCorrectValue = nlFalseAnswers[y].selectSingleNode("correctValue").sText;
						}
					
					
						oResult.Append("<font color=\"red\">");
						oResult.Append("<b>Falsch: </b>");
						oResult.Append(sYourValue);
						//Hinweistext anzeigen, falls vorhanden:
						if (nlFalseAnswers[y].sGetValue("notetext") != "") oResult.Append("<br/>");
						oResult.Append(nlFalseAnswers[y].sGetValue("notetext"));
						oResult.Append("</font>");
					
						if (sCorrectValue != "")
						{
							oResult.Append("&nbsp;&nbsp;<font color=\"green\">");
							oResult.Append("<b>richtig wäre gewesen: </b>");
							oResult.Append(sCorrectValue);
							oResult.Append("</font>");
						
						}
					
						oResult.Append("<br>");
					}
				
					//--------missing answers---------
					XmlNodeList nlMissingAnswers = nlAnswers[iAnswer].selectNodes("missing");
					for (int y = 0; y <= nlMissingAnswers.lCount - 1; y++)
					{
					
					
						string sCorrectValue = "";
						sCorrectValue = nlMissingAnswers[y].selectSingleNode("correctValue").sText;
					
						oResult.Append("<font color=\"red\">");
						oResult.Append("<b>Fehlt: </b>");
						oResult.Append(sCorrectValue);
						//Hinweistext anzeigen, falls vorhanden:
						if (nlMissingAnswers[y].sGetValue("notetext") != "") oResult.Append("<br/>");
						oResult.Append(nlMissingAnswers[y].sGetValue("notetext"));
						oResult.Append("</font>");
					
						oResult.Append("<br>");
					}
				
				}
			
			
			}
		
		
		
		
		
		
			return oResult.ToString();
		
		
		}
	
		//================================================================================
		//Function:  msVoteResultToHtml
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the result-xml of a Vote into a html
		//--------------------------------------------------------------------------------'
		//Params:    the xml
		//--------------------------------------------------------------------------------'
		//Returns:   html
		//--------------------------------------------------------------------------------'
		//Created:   14.05.2004 00:01:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msVoteResultToHtml(XmlDocument oXmlResult, int lSvy_id, string sConnstr_svm)
		{
		
		
			//----------------------include a default-vote-css-----------------
		
			const int lMaxImageWidth = 300;
		
			FastString oResult = new FastString();
		
			string[] asColors = new string[6] { "#FF0000", "#0000FF", "#FFFF00", "#FF00FF", "#009900", "#00FFFF" };
		
		
			oResult.Append("<p class=\"voteQuestiontext\">" + oXmlResult.selectSingleNode("//questiontext").sText + "</p>");
		
			oResult.Append("<table class=\"voteTable\" width=\"100%\" border=\"0\">");
		
			XmlNodeList nlNodes = oXmlResult.selectNodes("//valresult");
			for (int i = 0; i <= nlNodes.lCount - 1; i++)
			{
			
			
			
				string sBgColor = asColors[i %(Functions.UBound(asColors) + 1)];
				string sPercent;
				int lPercent;
				string sValText;
				string sPosAnzahl;
			
			
				//-------------set values----------
				sValText = nlNodes[i].selectSingleNode("text").sText;
				sPercent = nlNodes[i].selectSingleNode("spercent").sText;
				sPosAnzahl = nlNodes[i].selectSingleNode("counter").sText;
				lPercent = easyFramework.Sys.ToolLib.DataConversion.glCInt(nlNodes[i].selectSingleNode("lpercent").sText, 0);
			
			
			
				//-------------get html------------------
				oResult.Append("<tr>");
				oResult.Append("<td height=\"24\" width=\"35%\" class=\"voteField\">" + sValText + "</td>");
			
				oResult.Append("<td width=\"60%\" height=\"24\" class=\"voteField\">");
				oResult.Append("<img src=\"images/votebalken_m.gif\" height=\"13\" width=\"" + lPercent / 100 * lMaxImageWidth + "\"/>");
				oResult.Append("</td>");
			
				oResult.Append("<td height=\"24\" width=\"5%\" class=\"voteField\" align=\"right\">" + sPosAnzahl + "&nbsp;</td>");
				oResult.Append("<td height=\"24\" width=\"5%\" class=\"voteFieldPercent\" align=\"right\">" + sPercent + "</td>");
			
			
				oResult.Append("</tr>");
			
			}
		
			oResult.Append("<tr>");
			oResult.Append("<td class=\"voteBottomField\" align=\"right\">&nbsp;</td>");
			oResult.Append("<td class=\"voteBottomField\" align=\"right\">Gesamtbeteiligung:</td>");
			oResult.Append("<td class=\"voteBottomField\" align=\"right\">" + oXmlResult.selectSingleNode("/culmulativeresults")["sumCounter"].sText + "&nbsp;</td>");
			oResult.Append("<td class=\"voteBottomField\" align=\"right\">&nbsp;</td>");
			oResult.Append("</tr>");
		
			oResult.Append("</table>");
		
			oResult.Append("<br><br>");
		
		
			oResult.Append(DataMethods.gsGetDBValue(sConnstr_svm, "tdSurveys", "svy_congratulation_html", "svy_id=" + lSvy_id, ""));
		
		
		
			return oResult.ToString();
		
		}
	
		//================================================================================
		//Function:  msProfitPlayResultToHtml
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the result-xml of a profit-play into a html
		//--------------------------------------------------------------------------------'
		//Params:    the xml
		//--------------------------------------------------------------------------------'
		//Returns:   html
		//--------------------------------------------------------------------------------'
		//Created:   14.05.2004 00:01:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msProfitPlayResultToHtml(int lSvy_id, string sConnstr_svm)
		{
		
			return DataMethods.gsGetDBValue(sConnstr_svm, "tdSurveys", "svy_congratulation_html", "svy_id=" + lSvy_id, "");
		
		}
	
	}

}
