<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="help.aspx.cs" Inherits="easyFramework.Project.Default.help"%>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<!--#include file="../defaultheader.aspx.inc"-->
		<script language="javascript">
		
		var sHlp_id_On_Startup = "<%=sStartup_Hlp_id%>";
		
		function mOnLoad() {
			gWindowMaximize();
			
			
			if (sHlp_id_On_Startup != "") 
				mLoadContent(sHlp_id_On_Startup);
		}
		
		function mLoadContent(sHlp_id, sToc_id, sCustom_id) {
		
			if (sHlp_id == null) sHlp_id = "";
			if (sToc_id == null) sToc_id = "";
			if (sCustom_id == null) sCustom_id = "";
			
			
			var sParams = "<hlp_id>" + sHlp_id + "</hlp_id><toc_id>" + 
				sToc_id + "</toc_id><custom_id>" + sCustom_id + "</custom_id>";
		
			var sHtml = gsCallServerMethod("helpContentData.aspx", sParams);
			
			if (sHtml.substring(0, 6) != "OK-||-") {
				efMsgBox(sHtml, "ERROR");
				return
			}
			
			sHtml = sHtml.substring(6);
			
			var sHeading = sHtml.split("-||-")[0];
			var sBody = sHtml.split("-||-")[1];
			
			var oDiv = document.getElementById("content");
			
			oDiv.innerHTML = "<h1 class=\"heading\">" + sHeading + "</h1>" + sBody;
	
		
			if (sHlp_id != "") 
				gMakeNodeVisible(sHlp_id);
		
		}
		
		var sLastSearchTerm = "";
		function gSearch() {
		
			var sTerm = document.forms["frmMain"].elements["searchterm"].value;
			sLastSearchTerm = sTerm;
			
			var sHtmlResult = gsCallServerMethod("searchData.aspx?searchterm=" + sTerm);
			
			var oDiv = document.getElementById("content");
			oDiv.innerHTML = sHtmlResult;
			
			document.forms["frmMain"].elements["searchterm"].focus();
		}
	
		</script>
	</HEAD>
	<body onload="mOnLoad();">
		<table height="20" width="100%">
			<tr>
				<td class="clsDarkGradientHead">Online-Hilfe</td>
			</tr>
		</table>
		<ef:efTreeview id="EfTreeview1" runat="server" Left="0px" Top="55px" Width="30%" Height="80%" sDataPage="TocData.aspx"></ef:efTreeview>
		<div id="content" style="LEFT:30%; WIDTH:70%; POSITION:absolute; TOP:30px; HEIGHT:100%">
		</div>
		<form id="frmMain" method="post">
			<div id="divSearch" style="WIDTH:30%; POSITION:absolute; TOP:25px; overflow:hidden">
				<input type="text" name="searchterm"><ef:efButton id="efSearchBtn" runat="server" sText="$Suchen" sOnClick="gSearch();" CssClass="cmdButton"></ef:efButton>
			</div>
		</form>
	</body>
</HTML>
