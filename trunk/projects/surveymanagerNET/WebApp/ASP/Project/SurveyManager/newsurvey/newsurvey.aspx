<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools"%>
<%@ Page Language="cs" AutoEventWireup="false" src="newsurvey.aspx.cs" Inherits="easyFramework.Project.SurveyManager.newsurvey"%>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		function mOnLoad() {
			gResizeTo(600, 180);
			gPosWindowDefault();	
		}
				
		function mOnAbort() {
			window.close();
		}
		
		function mOnTableClick(sId) {
					
		}
		
		function mOnMenuItemSurveyGroupClick(sGroup) {
			goFindDataTable("EfDataTable1").reload("<svg_id>"+ sGroup +"</svg_id>");
			
		}
		
		function mOnOk() {
			
			var sDialogData = gsXMLDlgOutput(document.forms["frmMain"], "", true);
			var sResult = new String(gsCallServerMethod("newsurveyProcess.aspx", sDialogData))
			
			if (sResult.substring(0,8) != "SUCCESS_") {
				efMsgBox(sResult, "ERROR");
			}
			else {
				
				var svy_id = sResult.split("_")[1];
				
				gsShowWindow("../../../system/entityedit/entityedit.aspx?entity=surveys&svy_id=" + svy_id, false);
			
				window.close();
				
			}
			
		}
		
		function mOnAbort() {
			window.close();
		}
							
		</script>
		<!--#include file="../../../system/defaultheader.aspx.inc"-->
	</HEAD>
	<body onload="mOnLoad()">
		<form name="frmMain">
		<table height="20" width="100%" class="dialogTable">
			<tr>
				<td class="clsDarkGradientHead"><ef:efXmlDialog id="EfXmlDialog1" runat="server" sDefinitionFile="newSurvey.xml"></ef:efXmlDialog></td>
			</tr>
		</table>
		<ef:efButton id="EfButton1" runat="server" sOnClick="mOnOk();" sText="OK" sClass="cmdButton" CssClass="cmdButton"></ef:efButton>
		<ef:efButton id="EfButton2" runat="server" sOnClick="mOnAbort();" sText="Abbruch" sClass="cmdButton" CssClass="cmdButton"></ef:efButton>
		</form>
	</body>
</HTML>
