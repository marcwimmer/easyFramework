<%@ Page Language="vb" AutoEventWireup="false" src="multistructure.aspx.vb" Inherits="multistructureDemo"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>

	<HEAD>
		<ef:efPageHeader id="EfPageHeader2" runat="server"></ef:efPageHeader>
		<ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		function mOnLoad() {
			//gResizeTo(800,560);
			//gPosWindowDefault();	
			
		}
				
		function mOnOk() {
			alert(gsXMLDlgOutput(frmMain, "", true));
				//window.close();
		}
					
		function mOnAbort() {
			window.close();
		}

		
							
		</script>
		<!--#include file="../system/defaultheader.aspx"-->
		
	</HEAD>
	<BODY onload="mOnLoad()" onkeypress="gsStatus();">
		<form name="frmMain">
			<ef:efMultiStructure sFormName="frmMain" id="EfMultiStructure1" runat="server"></ef:efMultiStructure>
		</form>
	</BODY>
</HTML>