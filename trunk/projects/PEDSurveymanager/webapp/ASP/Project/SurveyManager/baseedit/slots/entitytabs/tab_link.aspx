<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<%@ Page Language="vb" AutoEventWireup="false" Src="tab_link.aspx.vb" Inherits="tab_slotlink" CodeBehind="tab_link.aspx.vb" %>
<%@ import namespace="easyFramework.sys.ToolLib.dataconversion" %>
<%@ import namespace="easyFramework.frontend.asp.asptools" %>
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
		<td class="captionField"><a href="<%=msHref%>" target="_blank"><%=Server.HTMLEncode(msHref)%></a></td>
	</tr>
	<tr>
		<td class="borderField"><button type="button" onclick="refreshLinkTab();">Aktualisieren</button></td>
	</tr>
</table>

