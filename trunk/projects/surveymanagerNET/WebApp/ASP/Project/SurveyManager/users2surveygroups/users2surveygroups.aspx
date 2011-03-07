<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="users2surveygroups.aspx.cs" Inherits="easyFramework.Project.SurveyManager.users2surveygroups" %>
<%@ Import namespace="easyFramework.Sys.Xml"%>
<%@ Import namespace="easyFramework.Sys.ToolLib"%>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<!--#include file="../../../system/defaultheader.aspx.inc"-->
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
			
			var sResult = gsCallServerMethod("users2surveygroupsProcess.aspx", sParams);
			
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
					Benutzergruppen von Benutzer<b>
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
								<td class="captionField"><b>Projektordner</b></td>
								<td class="captionField"><b>Benutzerzugriff</b></td>
							</tr>
							<%
			XmlNodeList nlSurveyGroups = oXmlSurveyGroups.selectNodes("//item");
			for (int i = 0; i < nlSurveyGroups.lCount; i++) 
			{
		%>
							<tr class="trBackground">
								<td class="entryField"><%=nlSurveyGroups[i].selectSingleNode("title").sText%></td>
								<td class="entryField">
								<%=msGetSelectCombo(DataConversion.glCInt(nlSurveyGroups[i].selectSingleNode("id").sText))%></td>
							</tr>
							<%
		
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
