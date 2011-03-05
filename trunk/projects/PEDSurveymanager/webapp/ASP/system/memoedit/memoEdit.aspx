<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="memoEdit.aspx.cs" Inherits="easyFramework.Project.Default.memoEdit" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
	</HEAD>
	<script language="javascript">
	function mOnLoad() {  	
		gResizeTo(700,540);	
		gPosWindowDefault();
		
	}

	function mOnOk() {
		//get xml-string from dialog-controls:
		var sParams = gsXMLDlgOutput(document.forms["frmMain"], "", true);
		
		//pass the xml-string to the the processing page and get the string-result:
		var sResult = gsCallServerMethod("memoProcess.aspx", sParams);
		
		
		//if ok then close window, otherwise alert the error:
		if (sResult.substring(0, 7) == "SUCCESS") {
			window.close();
		}
		else efMsgBox(sResult,"ERROR");
		
	}
	
	function mOnAbort() {
		window.close();
	}
	
	
	</script>
	<!--#include file="../defaultheader.aspx"-->
	<body onload="mOnLoad();">
		<form id="frmMain" method="post" runat="server">
			<ef:efXmlDialog id="EfXmlDialog1" runat="server" sDefinitionFile="memoDialog.xml"></ef:efXmlDialog>
			<ef:efXmlDialog id="EfXmlDialog2" runat="server" sDefinitionFile="memoDialogTextarea.xml"></ef:efXmlDialog>
			<FTB:FreeTextBox id="FreeTextBox1" runat="server" Width="660px" Height="450px"></FTB:FreeTextBox>
			<ef:efButton id="btnOk" runat="server" sText="$OK" sOnClick="mOnOk();"></ef:efButton>
			<ef:efButton id="btnAbort" runat="server" sText="$Abbruch" sOnClick="mOnAbort();"></ef:efButton>
		</form>
	</body>
</HTML>
