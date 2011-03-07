<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="placeholder.aspx.cs" Inherits="easyFramework.Project.SurveyManager.placeholder"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		
		function mOnLoad() {
			gResizeTo(750, 580);
			gPosWindowDefault();	
			
			
		}
		
		function mOnOk() {
			
			var sDlgData = gsXMLDlgOutput(document.forms["frmMain"], PXML_goGetDialog("EfPagedXmlDialog1").sAddParams, true);
			
			var sResult = gsCallServerMethod("placeholderProcess.aspx", sDlgData);
			
			if (sResult == "SUCCESS") {
				
				//refetch-data, so that ids of new values are set
				PXML_goGetDialog("EfPagedXmlDialog1").fetchData();	
			}
			else {
				efMsgBox(sResult, "ERROR");
			}
		}
					
		function mOnAbort() {
		
			window.close();
		}
		

		</script>
		<!--#include file="../../../system/defaultheader.aspx.inc"-->
	</HEAD>
	<body onload="mOnLoad();">
		<form name="frmMain">
			<table class="borderTable" width="100%">
				<tr>
					<td>
						<ef:efPagedXmlDialog sOnSaveChanges="mOnOk" id="EfPagedXmlDialog1" runat="server"></ef:efPagedXmlDialog>
					</td>
				</tr>
			</table>
		</form>
		<ef:efButton id="btnOk" runat="server" sOnClick="mOnOk();" sText="$Speichern"></ef:efButton>
		<ef:efButton id="btnAbort" runat="server" sOnClick="mOnAbort();" sText="$Abbruch"></ef:efButton>
	</body>
</HTML>
