<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="tab_publications.aspx.cs" Inherits="easyFramework.Project.SurveyManager.tab_publications"%>
<%@ import namespace="easyFramework.Sys.ToolLib" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools" %>
<div style="VISIBILITY:hidden; POSITION:absolute">
	<textarea id="javacontainer">
		
		function mOnNewPublication() {
			
			var sResult = gsCallServerMethod(gsGetWebPageRoot() + 
				"/ASP/Project/Surveymanager/baseedit/survey/entitytabs/tab_publicationsProcess.aspx", 
				"<svy_id>" + msGetEntityIdForOptions() + "</svy_id>");	//lastEntityId from entity-edit-dialog;
			
			if (sResult != "SUCCESS") 	
				efMsgBox(sResult, "ERROR");
			else {
				refreshPublicationsTab();	
			}
		}
		
		function mUnpublishSurvey(svy_id) {
		
		
		
		}
		
		function refreshPublicationsTab() {
			
			var sParams = "<svy_id>" + msGetEntityIdForOptions() + "</svy_id>";
			goFindTabDialog("EfTabDialog1").goGetTab("publications").fetchHtml();
		}

	</textarea>
</div>
<%

	string sLogoExcel = Images.sGetImageURL(oClientInfo, 
		"excel_logo", Request.ApplicationPath);

%>
<table class="dialogTable">
	<tr>
		<td class="captionField">Datum</td>
		<td class="captionField">Ergebnisse</td>
	</tr>
		<%if (mrsPublications != null) {
	while (!mrsPublications.EOF) {%>
	<tr>
		<td class="entryField"><%=DataConversion.gsFormatDate(oClientInfo.sLanguage, mrsPublications["pub_inserted"].dtValue)%></td>
		<td class="entryField"><a href="javascript:gsShowWindow(gsGetWebPageRoot() + '/ASP/Project/SurveyManager/excelresults/downloadPublicationResults.aspx?pub_id=<%=mrsPublications["pub_id"].sValue%>' , false);"><img alt="Download Excel" border="0" src="<%=sLogoExcel%>"> Download</a></td>
	</tr>
	<%
			mrsPublications.MoveNext();
		}
	}
	
%>
</table>
<ef:efButton id="efBtnNewPublication" runat="server" sOnClick="mOnNewPublication();" sText="Neu Publizierung"
	CssClass="cmdButtonDouble"></ef:efButton>
