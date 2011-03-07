<%@ Page Language="cs" AutoEventWireup="false" src="preview.aspx.cs" Inherits="easyFramework.Project.SurveyManager.preview"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader>
	</HEAD>
	<ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
	<script language="javascript">
	
	function mOnLoad() {
		gWindowMaximize();	
	}
		
	function mOnCloseClick() {
	
		window.close();
	}					
	</script>
	<!--#include file="../../../system/defaultheader.aspx.inc"-->
	<body onload="mOnLoad();">
		<div align="right">
		<ef:efButton id="EfButton1" runat="server" CssClass="cmdButton" sOnClick="mOnCloseClick();" sText="$Schließen"></ef:efButton>
		</div><hr>
		<br><br><br>
		
		<%=this.sSurveyHtml%>
	</body>
</HTML>
