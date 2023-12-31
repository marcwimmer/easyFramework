<%@ Page Language="cs" AutoEventWireup="false" src="surveyedit.aspx.cs" Inherits="easyFramework.Project.SurveyManager.surveyedit" %>
<%@ Register TagPrefix="mul" Namespace="easyFramework.Frontend.ASP.ComplexObjects" Assembly="efASPFrontend" %>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		var svy_id = ""; 		
		var sEntityType = "";
		var sEntityId = "";
		var sLastTreeview = "";
		var sLastQst_id = "";
		var sLastAns_id = "";
		
		function mOnLoad() {
		
			//gWindowMaximize();	
			gResizeTo(1000, 600);
			svy_id = getSearchParam("svy_id");
			
		}
		
		function mOnSave() {
			 
			efStatus("Daten werden gespeichert...");
		
			var sDlgData = gsXMLDlgOutput(document.forms["frmMain"], "", true);
			sDlgData = sDlgData.replace("<DIALOGOUTPUT>", "<DIALOGOUTPUT><entity>" + sEntityType + "</entity>");
			sDlgData = sDlgData.replace("<DIALOGOUTPUT>", "<DIALOGOUTPUT><entityId>" + sEntityId + "</entityId>");
			
			var sResult = gsCallServerMethod("../../../system/entityedit/EntityProcess.aspx", sDlgData);
			
			if (sResult.substring(0, 7) == "SUCCESS") {
				
				gXmlDialogSetDirty("editDialog", false);
				efStatus("erfolgreich gespeichert");
				
				reloadTreeview(sLastTreeview, null, true)
			}
			else {
				efStatus("Fehler!");
				alert(sResult);
			}
		}
					
		function mOnAbort() {
		
			window.close();
		}
		
		function mOnMenuItemSurveyGroupClick(sGroup) {
			goFindDataTable("EfDataTable1").reload("<svg_id>"+ sGroup +"</svg_id>");
			
		}
					
		function mOnPreview() {
		
			gsShowWindow("../preview/preview.aspx?svy_id="+svy_id, false);
				
		}		
		
		function msOnAfterAskToSave() {
			//saved after paging:
			var sDlgData = gsXMLDlgOutput(document.forms["frmMain"], "", true);
			
			var sResult = gsCallServerMethod("../../../system/multistructure/storing/storeProcess.aspx", sDlgData);
			
			if (sResult == "SUCCESS") {
				
			}
			else {
				alert(sResult);
			}
		
		}
		
		function mLoadQuestion(qst_id) {
			mHandleSaveChanges();
		
			//in Answer-Treeview alle Antworten laden:
			var sParams = "<qst_id>" + qst_id + "</qst_id>";
			sEntityId = qst_id;
			sEntityType = "questions";
			reloadTreeview("efMenuTree_Answers", sParams);
			sLastTreeview = "efMenuTree_Questions";
			sLastQst_id = qst_id;
			sLastAns_id = ""; //reset
			
			//Frage in Xml-Dialog laden:
			var oDiv = document.getElementById("editDialog");
			var sXmlDialogDefinitionFile = "/ASP/Project/SurveyManager/baseedit/question/Dialog.xml";
			var sXmlDialogDataPage = "../../../system/entityedit/entityDialogData.aspx";
			var sXmlFormName = "frmMain";
			var sXmlDialogId = "editDialog";
			var sXmlDataParams = sParams + "<entity>" + sEntityType + "</entity><entityId>" + sEntityId + "</entityId>";
			gXmlDialog2Div(oDiv, sXmlDialogDefinitionFile, sXmlDialogDataPage, sXmlFormName, sXmlDialogId, sXmlDataParams)
			gXmlDialogFetchData(sXmlDialogId, sXmlDataParams);


			//in AnswerValue-Treeview alle Antwortwerten laden:
			var ans_id = -1;
			var sParams = "<ans_id>" + ans_id + "</ans_id>";
			reloadTreeview("efMenuTree_AnswerValues", sParams);
			sLastAns_id = ans_id;
			
			
		}
		
		
		function mLoadAnswer(ans_id) {
			mHandleSaveChanges();
		
		
			if (ans_id == null) ans_id = sLastAns_id;
		
			//in AnswerValue-Treeview alle Antwortwerten laden:
			var sParams = "<ans_id>" + ans_id + "</ans_id>";
			sEntityId = ans_id;
			sEntityType = "answers";
			reloadTreeview("efMenuTree_AnswerValues", sParams);
			sLastTreeview = "efMenuTree_Answers";
			sLastAns_id = ans_id;
			
			//Frage in Xml-Dialog laden:
			
			//Besonderheit: 
			//Abh�ngig vom Fragentyp (ans_aty_id) gibt es verschiedene XML-Dialoge, die geladen werden m�ssen:
			var sSqlParams = "<field>ans_aty_id</field><from>tdAnswers</from><where>ans_id=" + ans_id + "</where>";
			var sAnswerType = gsCallServerMethod("fieldValueData.aspx", sSqlParams);
			
			var sSqlParams = "<field>aty_xmldialog_path</field><from>tdAnswers inner join tdAnswerTypes on tdAnswertypes.aty_id = ans_aty_id</from><where>ans_id=" + ans_id + "</where>";
			var sDialogXml = gsCallServerMethod("fieldValueData.aspx", sSqlParams);
			
			var oDiv = document.getElementById("editDialog");
			var sXmlDialogDefinitionFile = sDialogXml;
			var sXmlDialogDataPage = "../../../system/entityedit/entityDialogData.aspx";
			var sXmlFormName = "frmMain";
			var sXmlDialogId = "editDialog";
			var sXmlDataParams = sParams + "<entity>" + sEntityType + "</entity><entityId>" + sEntityId + "</entityId>";
			gXmlDialog2Div(oDiv, sXmlDialogDefinitionFile, sXmlDialogDataPage, sXmlFormName, sXmlDialogId, sXmlDataParams)
			gXmlDialogFetchData(sXmlDialogId, sXmlDataParams);
		}

		function mLoadAnswerValue(val_id) {
			mHandleSaveChanges();
			
			//in AnswerValue-Treeview alle Antwortwerten laden:
			var sParams = "<val_id>" + val_id + "</val_id>";
			sEntityId = val_id;
			sEntityType = "answervalues";
			sLastTreeview = "efMenuTree_AnswerValues";
			sLastVal_id = val_id;
			
			//Frage in Xml-Dialog laden:
			var oDiv = document.getElementById("editDialog");
			var sXmlDialogDefinitionFile = "/ASP/Project/SurveyManager/baseedit/answervalue/Dialog.xml";
			var sXmlDialogDataPage = "../../../system/entityedit/entityDialogData.aspx";
			var sXmlFormName = "frmMain";
			var sXmlDialogId = "editDialog";
			var sXmlDataParams = sParams + "<entity>" + sEntityType + "</entity><entityId>" + sEntityId + "</entityId>";
			gXmlDialog2Div(oDiv, sXmlDialogDefinitionFile, sXmlDialogDataPage, sXmlFormName, sXmlDialogId, sXmlDataParams)
			gXmlDialogFetchData(sXmlDialogId, sXmlDataParams);
		}
		
		function mAddQuestion() {
		
			var sType = "QUESTION";
			var sParentId = svy_id;
			var sParams = "type=" + sType + "&parentid=" + sParentId;
			var sResult = gsCallServerMethod("insertElementProcess.aspx?" + sParams, "");
			if (sResult.substring(0, 3) != "OK!") { efMsgBox(sResult, "ERROR"); return; }
			reloadTreeview("efMenuTree_Questions", "<svy_id>" + svy_id + "</svy_id>");
					
			var sNewId = sResult.split("_")[1];
			mLoadQuestion(sNewId);		
			
			gTreeviewSelectNode("efMenuTree_Questions", sNewId);
		}
		
		function mAddAnswer() {
		
			if (sLastQst_id == "" | sLastQst_id == null) {
				efMsgBox("Bitte w�hlen Sie eine Frage aus!", "INFO");
				return;
			}
		
			var sType = "ANSWER";
			var sParentId = sLastQst_id;
			var sParams = "type=" + sType + "&parentid=" + sParentId;
			var sResult = gsCallServerMethod("insertElementProcess.aspx?" + sParams, "");
			if (sResult.substring(0, 3) != "OK!") { efMsgBox(sResult, "ERROR"); return; }
			reloadTreeview("efMenuTree_Answers", "<qst_id>" + sLastQst_id + "</qst_id>");
			var sNewId = sResult.split("_")[1];
			mLoadAnswer(sNewId);		

			gTreeviewSelectNode("efMenuTree_Answers", sNewId);
					
		}		

		function mAddAnswerValue() {
		
			if (sLastAns_id == "" | sLastAns_id == null) {
				efMsgBox("Bitte w�hlen Sie eine Antwort aus!", "INFO");
				return;
			}
		
			var sType = "ANSWERVALUE";
			var sParentId = sLastAns_id;
			var sParams = "type=" + sType + "&parentid=" + sParentId;
			var sResult = gsCallServerMethod("insertElementProcess.aspx?" + sParams, "");
			if (sResult.substring(0, 3) != "OK!") { efMsgBox(sResult, "ERROR"); return; }
			reloadTreeview("efMenuTree_AnswerValues", "<ans_id>" + sLastAns_id + "</ans_id>");
			var sNewId = sResult.split("_")[1];
			mLoadAnswerValue(sNewId);		
			
			gTreeviewSelectNode("efMenuTree_AnswerValues", sNewId);
			
		}		
		
		function mDeleteQuestion() {
			if (sLastQst_id == "" | sLastQst_id == null) {
				efMsgBox("Bitte w�hlen Sie eine Frage aus!", "INFO");
				return;
			}
			
			if (!confirm("Wirklich l�schen?")) return;
			
			var sType = "QUESTION";
			var sParams = "<type>" + sType + "</type><id>" + sLastQst_id + "</id>"
			var sResult = gsCallServerMethod("deleteElementProcess.aspx", sParams);
			if (sResult != "OK!") { efMsgBox(sResult, "ERROR"); return; }
			reloadTreeview("efMenuTree_Questions", "<svy_id>" + svy_id + "</svy_id>");
			reloadTreeview("efMenuTree_Answers", "<qst_id>" + sLastQst_id + "</qst_id>");
			reloadTreeview("efMenuTree_AnswerValues", "<val_id>" + sLastAns_id + "</val_id>");
			
			sLastQst_id = "";
			sLastAns_id = "";
			sLastVal_id = "";
			
			document.getElementById("editDialog").innerHTML = "";
		}
		
		function mDeleteAnswer() {
			if (sLastAns_id == "" | sLastAns_id == null) {
				efMsgBox("Bitte w�hlen Sie eine Antwort aus!", "INFO");
				return;
			}
			
			if (!confirm("Wirklich l�schen?")) return;
			
			var sType = "ANSWER";
			var sParams = "<type>" + sType + "</type><id>" + sLastAns_id + "</id>"
			var sResult = gsCallServerMethod("deleteElementProcess.aspx", sParams);
			if (sResult != "OK!") { efMsgBox(sResult, "ERROR"); return; }
			reloadTreeview("efMenuTree_Answers", "<qst_id>" + sLastQst_id + "</qst_id>");
			reloadTreeview("efMenuTree_AnswerValues", "<val_id>" + sLastAns_id + "</val_id>");

			sLastAns_id = "";
			sLastVal_id = "";
		
			document.getElementById("editDialog").innerHTML = "";
		}
		
		function mDeleteAnswerValue() {
			if (sLastVal_id == "" | sLastVal_id == null) {
				efMsgBox("Bitte w�hlen Sie einen Antwortwert aus!", "INFO");
				return;
			}
			
			if (!confirm("Wirklich l�schen?")) return;
			
			var sType = "ANSWERVALUE";
			var sParams = "<type>" + sType + "</type><id>" + sLastVal_id + "</id>"
			var sResult = gsCallServerMethod("deleteElementProcess.aspx", sParams);
			if (sResult != "OK!") { efMsgBox(sResult, "ERROR"); return; }
			reloadTreeview("efMenuTree_AnswerValues", "<val_id>" + sLastAns_id + "</val_id>");
		
			sLastVal_id = "";

			document.getElementById("editDialog").innerHTML = "";
		}

		function mHandleSaveChanges() {
		
			if (gbAnyRegisteredXmlDialogDirty()) {
				if (efMsgBox("�nderungen speichern?", "QUESTION", "YESNO") == "YES") {
					mOnSave();
				}
			}
		
		
		}		
	
		</script>
		<!--#include file="../../../system/defaultheader.aspx.inc"-->
	</HEAD>
	<body onload="mOnLoad();">
		
		
		<!--Fragen-->
		<div style="LEFT:0px; OVERFLOW:hidden; WIDTH:50%; POSITION:absolute; TOP:0px; HEIGHT:5%">
			<h1>Fragen</h1>
		</div>
		<ef:efTreeview id="efMenuTree_Questions" runat="server" enBorderStyle="efRidge"
			Width="50%" Height="48%" sBorderWidth="thin" BorderStyle="Groove" Left="0%" Top="5%" BorderWidth="3px" sOverflow="Scroll" 
			sSelectBackgroundColor="#AAC4E6"></ef:efTreeview>
		<div style="LEFT:0px; OVERFLOW:hidden; Width:50%; POSITION:absolute;TOP:54%; HEIGHT:5%">
			<ef:efButton id="EfButton_AddQuestion" runat="server" sText="Neue Frage" sOnClick="mAddQuestion();"></ef:efButton>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<ef:efButton id="EfButton_DeleteQuestion" runat="server" sText="L�schen" sOnClick="mDeleteQuestion();"></ef:efButton>
		</div>

		<!--Antworten-->
		<div style="LEFT:50%; OVERFLOW:hidden; WIDTH:30%; POSITION:absolute; TOP:0px; HEIGHT:5%">
			<h1>Antworten</h1>
		</div>
		<ef:efTreeview id="efMenuTree_Answers" runat="server" enBorderStyle="efRidge"
			Width="30%" Height="48%" sBorderWidth="thin" BorderStyle="Groove" Left="50%" Top="5%" BorderWidth="3px"
			sSelectBackgroundColor="#AAC4E6"></ef:efTreeview>
		<div style="LEFT:50%; OVERFLOW:hidden; Width:30%; POSITION:absolute;TOP:54%; HEIGHT:5%">
			<ef:efButton id="EfButton_AddAnswer" runat="server" sText="Neue Antw." sOnClick="mAddAnswer();"></ef:efButton>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<ef:efButton id="EfButton_DeleteAnswer" runat="server" sText="L�schen" sOnClick="mDeleteAnswer();"></ef:efButton>
		</div>
		
		<!--Antwortwerte-->
		<div style="LEFT:80%; OVERFLOW:hidden; WIDTH:20%; POSITION:absolute; TOP:0px; HEIGHT:5%">
			<h1>Antwortwerte</h1>
		</div>
		<ef:efTreeview id="efMenuTree_AnswerValues" runat="server" enBorderStyle="efRidge"
			Width="20%" Height="48%" sBorderWidth="thin" BorderStyle="Groove" Left="80%" Top="5%" BorderWidth="3px"
			sSelectBackgroundColor="#AAC4E6"></ef:efTreeview>
		<div style="LEFT:80%; OVERFLOW:hidden; Width:20%; POSITION:absolute;TOP:54%; HEIGHT:5%">
			<ef:efButton id="EfButton_AddAnswerValue" runat="server" sText="Neuer Wert" sOnClick="mAddAnswerValue();"></ef:efButton>
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			<ef:efButton id="EfButton_DeleteAnswerValue" runat="server" sText="L�schen" sOnClick="mDeleteAnswerValue();"></ef:efButton>
		</div>

					
		<!--editier-Dialog (XML-Dialog-->
		<form name="frmMain" id="frmMain">
			<div id="editDialogSpace" style="background: #AAC4E6; LEFT:0px; OVERFLOW:hidden; WIDTH:100%; POSITION:absolute; TOP:59%; HEIGHT:1%">
			
			</div>
			<div id="editDialog" style="background: #AAC4E6; LEFT:0px; OVERFLOW:hidden; WIDTH:100%; POSITION:absolute; TOP:60%; HEIGHT:30%">
			bitte auf einen Eintrag klicken!
			</div>
			<div id="editDialogBtn" style="background: #AAC4E6; LEFT:0px; OVERFLOW:hidden; WIDTH:100%; POSITION:absolute; TOP: 90%; HEIGHT: 10%" align="right">
				<ef:efButton id="EfButton1" runat="server" sText="$Speichern" sOnClick="mOnSave();"></ef:efButton>
				<ef:efButton id="EfButton3" runat="server" sText="$Preview" sOnClick="mOnPreview();"></ef:efButton>			
				<ef:efButton id="EfButton2" runat="server" sText="S$chlie�en" sOnClick="mOnAbort();"></ef:efButton>
			</div>
				
			
		</form>
		
		
		
		
	</body>
</HTML>
