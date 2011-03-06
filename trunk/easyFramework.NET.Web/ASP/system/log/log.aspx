<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="log.aspx.cs" Inherits="easyFramework.Project.Default.logEntries" %>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader2" runat="server"></ef:efPageHeader>
		<ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		function mOnLoad() {
			gResizeTo(800,560);
			gPosWindowDefault();	
			
		}
				
		function mOnOk() {
			alert(gsXMLDlgOutput(frmMain, "", true));
				//window.close();
		}
					
		function mOnAbort() {
			window.close();
		}
		
		function gOnDataTableItemClick(sDataID) {
			
		}
		
		function mClearLog() {
			var sResult = gsCallServerMethod("clearLogProcess.aspx");
			
			if (sResult != "OK!") {
				efMsgBox(sResult, "ERROR");
				return
			}
			goFindDataTable("EfDataTable1").moveFirst();
		}
							
		</script>
		<!--#include file="../defaultheader.aspx.inc"-->
	</HEAD>
	<BODY onload="mOnLoad()">
		<div>
		<ef:efDataTable id="EfDataTable1" runat="server" Width="100%" Height="90%" sDataPage="logDataTable.aspx"
			sOnItemClick="" lRowsPerPage="25" Left="00%" Top="0px" Overflow="efScroll"></ef:efDataTable>
		</div>
		<div style="position:absolute; left:60%; top:90%; width:40%; height:80px" align="right">
		<ef:efButton id="EfButton1" runat="server" sText="Log $leeren" sOnClick="mClearLog();"></ef:efButton>
		<ef:efButton id="Efbutton2" runat="server" sText="S$chließen" sOnClick="window.close();"></ef:efButton>
		</div>
	</BODY>
</HTML>
