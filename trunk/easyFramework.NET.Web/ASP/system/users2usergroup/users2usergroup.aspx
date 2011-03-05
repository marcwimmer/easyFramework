<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="users2usergroup.aspx.cs" Inherits="easyFramework.Project.Default.users2usergroup" %>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<!--#include file="../defaultheader.aspx"-->
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
			
			var sResult = gsCallServerMethod("users2usergroupProcess.aspx", sParams);
			
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
					Benutzer von <b>
						<%=oUserGroupEntity.gsToString(oClientInfo)%>
					</b>
				</td>
			</tr>
			<tr>
				<td class="dlgField" valign="top">
					<form id="frmMain" name="frmMain" method="post">
						<input type="hidden" name="grp_id" value="<%=lGrp_id%>">
						<table class="dialogTable">
							<%
			while (!rsUsers.EOF) 
			{
		%>
							<tr class="trBackground">
								<td class="entryField"><%=rsUsers["usr_firstname"].sValue%> <%=rsUsers["usr_lastname"].sValue%> (Login: <%=rsUsers["usr_login"].sValue%>)</td>
								<td class="entryField"><input type="checkbox" name="usr_id_<%=rsUsers["usr_id"].sValue%>" <%if (gbIsUsr_id(rsUsers["usr_id"].lValue)) {%>checked<%}%>></td>
							</tr>
							<%
		
				rsUsers.MoveNext();
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
