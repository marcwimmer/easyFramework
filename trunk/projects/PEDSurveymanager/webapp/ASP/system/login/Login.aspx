<%@ Page Language="cs" AutoEventWireup="false" Src="Login.aspx.cs" Inherits="easyFramework.Project.Default.Login"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<%@ Import Namespace="easyFramework.Frontend.ASP.ASPTools" %>
<HTML>
	<HEAD>
		<%
	
	string sMainPage = efEnvironment.goGetEnvironment(Application).gsMainPage;

%>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<!--#include file="../defaultheader.aspx"-->
		<script language="javascript">
		
		function mOnLoad() {
			gResizeTo(400,240);
			gPosWindowDefault();
			
			gFocusFirstXmlDialogElement("EfXmlDialog1");
			
			if (document.forms["frmMain"].elements["txtusername"].value != "") {
				mOnSubmit();
			}
		}
				
		function mOnAbort() {
			window.close();
		}
		
		function mOnSubmit() {
			
			
			var sParams = gsXMLDlgOutput(document.forms['frmMain'], "", true);
			
			var sResult = gsCallServerMethod("loginProcess.aspx", sParams);
			
			if (sResult.substring(0, 8) == "SUCCESS_") {
				
				//set client-id:
				msClientID = sResult.substring(8, sResult.length);
				
				gReturnModalResult(true);
				
				gsShowWindow("<%=sMainPage%>", false, null, window.opener);
			
			}
			else {
				alert(sResult);
			}
		
			
		}
							
		</script>
	</HEAD>
	<body onload="mOnLoad()">
		<form id="frmMain" onsubmit="mOnSubmit();">
			<table class="borderTable" width="100%" height="100%">
				<tr>
					<td align="center" valign="top">
						<ef:efXmlDialog id="EfXmlDialog1" runat="server" sDefinitionFile="loginDlg.xml"></ef:efXmlDialog>
					</td>
				</tr>
				<tr>
					<td class="entryField">			
						At the first login use:<br>
						username: <b>&quot;sa&quot;</b><br>
						password: <b>&quot;easy&quot;</b>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
