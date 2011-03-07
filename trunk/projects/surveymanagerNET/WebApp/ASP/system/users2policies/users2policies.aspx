<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="users2policies.aspx.cs" Inherits="easyFramework.Project.Default.users2policies" %>
<%@ Import namespace="easyFramework.Sys.Xml"%>
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
			
			var sResult = gsCallServerMethod("users2policiesProcess.aspx", sParams);
			
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
					Policies von Benutzer<b>
						<%=oUserEntity.gsToString(oClientInfo)%>
					</b>
				</td>
			</tr>
			<tr>
				<td class="dlgField" valign="top">
					<form id="frmMain" name="frmMain" method="post">
						<input type="hidden" name="usr_id" value="<%=lUsr_id%>">
						<table class="dialogTable">
							<tr>
								<td class="captionField"><b>Policy</b></td>
								<td class="captionField"><b>Beschreibung</b></td>
								<td class="captionField"><b>Gruppenrichtlinie</b></td>
								<td class="captionField"><b>Benutzerzugriff</b></td>
							</tr>
							<%
			
			while (!rsPolicies.EOF)
			{
		%>
							<tr class="trBackground">
								<td class="entryField"><%=rsPolicies["pol_name"].sValue%></td>
								<td class="entryField"><%=rsPolicies["pol_description"].sValue%></td>
								<td class="entryField">
								<%=msGetGroupAccess(rsPolicies["pol_id"].lValue)%>
								</td>
								<td class="entryField">
								<%=msGetSelectCombo(rsPolicies["pol_id"].lValue)%></td>

							</tr>
							<%
		
				rsPolicies.MoveNext();
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
