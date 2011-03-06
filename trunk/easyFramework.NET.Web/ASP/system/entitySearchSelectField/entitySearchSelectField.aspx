<%@ Page Language="cs" AutoEventWireup="false" src="entitySearchSelectField.aspx.cs" Inherits="easyFramework.Project.Default.entitySearchSelectField" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		function mOnLoad() {
			gResizeTo(500,200);
			gPosWindowDefault();	
			gFocusFirstElement("frmMain");
			
			mSetValueInDialog("frmMain", "searchfield", gsGetClientValue("LastSearchField"), "0", "");
			mSetValueInDialog("frmMain", "searchphrase", gsGetClientValue("LastSearchValue"), "0", "");
		}
				
		function mOnAbort() {
			gReturnModalResult(false);
		}
		
		function mOnOk() {
		
			var sPhrase = document.forms['frmMain'].elements['txtsearchphrase'].value;
			var sField = document.forms['frmMain'].elements['cbosearchfield'].value;
			
			var sResult = sField + ';' + sPhrase;
			
			//store the last search field, so that at start up, this field is selected again:
			gSetClientValue("LastSearchField", sField);
			gSetClientValue("LastSearchValue", sPhrase);
			
			gReturnModalResult(sResult);
			
		}
		
		
		function loadEntityIntoDialog(sEntityID) {
		}
							
		</script>
		<!--#include file="../defaultheader.aspx.inc"-->
	</HEAD>
	<BODY onload="mOnLoad()">
		<form name="frmMain" onsubmit="mOnOk(); return false;">
			<ef:efXmlDialog id="EfXmlDialog1" runat="server"></ef:efXmlDialog>
			<ef:efButton id="efBtn_Ok" runat="server" sText="Ok" sOnClick="mOnOk();"></ef:efButton>
			<ef:efButton id="efBtn_Abort" runat="server" sText="Abbruch" sOnClick="mOnAbort();"></ef:efButton>
		</form>
		
		<p>Anmerkung: verwenden Sie das Wildcard *, um nach Teilzeichenketten zu suchen, z.B. *schm* findet Eintr&auml;ge, wie z.B. "Schmidt", "Schmied", "Hammerschmidt"
	</BODY>
</HTML>
