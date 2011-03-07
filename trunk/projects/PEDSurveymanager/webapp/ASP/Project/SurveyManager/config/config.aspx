<%@ Page Language="cs" AutoEventWireup="false" Src="config.aspx.cs" Inherits="easyFramework.Project.SurveyManager.configDlg"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<script language="javascript">
	
			function mOnLoad() {
			 gResizeTo(600, 510);
			 gPosWindowDefault();
			 gFocusFirstElement("frmMain");
			}
			
			function mOnAbort() {
				gReturnModalResult(false);
			}
			
			function mOnOk() {
			
				var sDlgOutput = gsXMLDlgOutput(document.forms["frmMain"], "", true)
			
				var sResult = gsCallServerMethod("configProcess.aspx", sDlgOutput);
			
				if (sResult == "SUCCESS") 
					gReturnModalResult(true);
				else
					efMsgBox(sResult, "ERROR");
			
			}
		</script>
		<!--#include file="../../../system/defaultheader.aspx.inc"-->
	</HEAD>
	<body onload="mOnLoad()">
		<form id="frmMain" method="post" runat="server">
			<table class="bordertable" width="100%">
				<tr>
					<td>
						<ef:efXmlDialog id="EfXmlDialog1" runat="server" sDefinitionFile="config.xml" Width="368px"></ef:efXmlDialog>
					</td>
				</tr>
				<tr>
					<td>
						<ef:efButton id="btnOk" runat="server" sText="OK" sOnClick="mOnOk();"></ef:efButton>
						<ef:efButton id="btnAbort" runat="server" sText="Abbruch" sOnClick="mOnAbort();"></ef:efButton>
						</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
