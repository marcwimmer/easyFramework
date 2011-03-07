<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="tab_link.aspx.cs" Inherits="easyFramework.Project.SurveyManager.tab_slotlink" CodeBehind="tab_link.aspx.cs" %>
<%@ import namespace="easyFramework.Sys.ToolLib" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools" %>
<div style="VISIBILITY:hidden; POSITION:absolute">
	<textarea id="javacontainer">
		
		function mUnpublishSurvey(svy_id) {
		
		
		
		}
		
		function refreshLinkTab() {
			
			var sParams = "<svy_id>" + msGetEntityIdForOptions() + "</svy_id>";
			goFindTabDialog("EfTabDialog1").goGetTab("link").refresh(sParams, msGetEntityIdForOptions());
			
		}

	</textarea>
</div>
<table class="dialogTable">
	<tr>
		<td class="captionField"><b>Link auf den Slot</b></td>
	</tr>
	<tr> 
		<td class="captionField"><a href="<%=msHref%>" target="_blank"><%=Server.HtmlEncode(msHref)%></a></td>
	</tr>
	<tr>
		<td class="borderField"><button type="button" onclick="refreshLinkTab();">Aktualisieren</button></td>
	</tr>
</table>

