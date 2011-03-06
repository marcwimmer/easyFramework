<%@ Page Language="cs" AutoEventWireup="false" src="multistructure.aspx.cs" Inherits="easyFramework.Project.Default.multistructureDemo"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.ComplexObjects" Assembly="efASPFrontend" %>
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