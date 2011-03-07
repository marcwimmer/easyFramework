<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="usergroups2user.aspx.cs" Inherits="easyFramework.Project.Default.usergroups2user" %>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<!--#include file="../defaultheader.aspx.inc"-->
		<script language="javascript">
		
		function mOnLoad() {
			gResizeTo(500,540);
			gPosWindowDefault();
			
			//gFocusFirstXmlDialogElement("EfXmlDialog1");
		}
				
		function mOnAbort() {
			window.close();
		}
		
		function mOnSave() {
			
			var sParams = gsXMLDlgOutput(document.forms['frmMain'], "", true);
			
			var sResult = gsCallServerMethod("usergroups2userProcess.aspx", sParams);
			
			if (sResult.substring(0, 7) == "SUCCESS") {
				
				window.close();	
			}
			else {
				alert(sResult);
			}
		
			
		}
							
		</script>
	</HEAD>
	<body onload="mOnLoad();">
		<table class="borderTable" width="100%" height="80%">
			<tr>
				<td class="dlgField" height="25px">
					Gruppen von <b>
						<%=oUserEntity.gsToString(oClientInfo)%>
					</b>
				</td>
			</tr>
			<tr>
				<td class="dlgField" valign="top">
					<form id="frmMain" name="frmMain" method="post">
						<input type="hidden" name="usr_id" value="<%=lUsr_id%>">
						<table class="dialogTable">
							<%
			while (!rsUserGroups.EOF)
			{
		%>
							<tr class="trBackground">
								<td class="entryField"><%=rsUserGroups["grp_name"].sValue%></td>
								<td class="entryField"><input type="checkbox" name="grp_id_<%=rsUserGroups["grp_id"].sValue%>" <%if (gbIsUsrgrp_id(rsUserGroups["grp_id"].lValue)) {%>checked<%}%>></td>
							</tr>
							<%
		
				rsUserGroups.MoveNext();
			}
		%>
						</table>
					</form>
				</td>
			</tr>
		</table>
		<ef:efButton id="efBtnSave" runat="server" sOnClick="mOnSave();" sText="$Speichern"></ef:efButton>
		<ef:efButton id="efBtnAbort" runat="server" sOnClick="mOnAbort();" sText="$Abbruch"></ef:efButton>
	</body>
</HTML>
