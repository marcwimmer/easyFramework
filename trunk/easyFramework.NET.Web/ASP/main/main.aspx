<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<%@ Page Language="cs" AutoEventWireup="false" src="main.aspx.cs" Inherits="easyFramework.Project.Default.main"%>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools"%>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		function mOnLoad() {
			//gWindowMaximize();	
		}
				
		function mOnOk() {
			
		}
					
		function mOnAbort() {
			window.close();
		}
		

							
		</script>
		<!--#include file="../system/defaultheader.aspx"-->
	</HEAD>
	<body onload="mOnLoad()">
		<table height="20" width="100%">
			<tr>
				<td class="clsDarkGradientHead"><%=Server.HtmlEncode(efEnvironment.goGetEnvironment(Application).gsProjectName)%></td>
			</tr>
		</table>
		<ef:efTreeview id="efMenuTree" runat="server" lWidth="20%" lHeight="95%" lTop="5%" lLeft="1" enBorderStyle="efRidge"
			Width="20%" Height="95%" sBorderWidth="thin" BorderStyle="Groove" Left="1" Top="5%" BorderWidth="3px"></ef:efTreeview>
		<form id="frmMain" onsubmit="mOnSubmit(); return false;">
		</form>
	</body>
</HTML>
