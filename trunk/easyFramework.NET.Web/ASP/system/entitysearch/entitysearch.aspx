<%@ Page Language="cs" AutoEventWireup="false" src="entitySearch.aspx.cs" Inherits="easyFramework.Project.Default.entitySearch" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
		<script language="javascript">
		
		function mOnLoad() {
			gResizeTo(600,500);
			gPosWindowDefault();	
			
		}
				
		function mOnAbort() {
			gReturnModalResult("");
		}
		
		function mOnOk() {
		
			if (goFindDataTable("EfDataTable1").sCurrentSelectedItemId == null) {
				efMsgBox("Bitte wählen Sie einen Wert aus!", "ERROR");
				return;
			}
				   
			gReturnModalResult(goFindDataTable("EfDataTable1").sCurrentSelectedItemId);
		}
		
		function mClearFilter() {
			document.getElementById("img_filter").src="<%=sImg_clear%>";

			goFindDataTable("EfDataTable1").reload("<entity><%=sEntityName%></entity><searchdlg></searchdlg>");

		}
		
		function mOnSearch() {
		
			if (bNowModalDialogClosed == true) {
				gsShowWindow("../entitySearchSelectField/entitySearchSelectField.aspx" +
					"?entity=<%=sEntityName%>", true, "mOnSearch();");
				
				return;
			}
			
			goFindDataTable("EfDataTable1").reload("<entity><%=sEntityName%></entity><searchdlg><![CDATA[" + 
			oModalResult + "]]></searchdlg>");

			document.getElementById("img_filter").src="<%=sImg_filter_search%>";
			document.getElementById("img_filter").alt=new String(oModalResult);
			
		}
		
		function loadEntityIntoDialog(sEntityID) {
		}
							
		</script>
		<!--#include file="../defaultheader.aspx.inc"-->
	</HEAD>
	<BODY onload="mOnLoad()">
		<form name="frmMain" onsubmit="mOnOk(); return false;">
			<div id="filterImage" align="right" style="LEFT:0px;WIDTH:10%;POSITION:absolute;TOP:0px;HEIGHT:50px"><a href="#" onclick="mClearFilter(); return false;"><img border="0" id="img_filter" src="<%=sImg_clear%>"></a></div>
			<ef:efdatatable id="EfDataTable1" runat="server" lRowsPerPage="10" sDataPage="entitySearchDataTable.aspx"
				Height="100%" Width="75%" Left="10%" Top="0" sOnItemDblClick="mOnOk();"></ef:efdatatable>
			<div style="Z-INDEX:100; LEFT:85%; WIDTH:15%; POSITION:absolute; TOP:0px; HEIGHT:60%">
				<ef:efButton id="efBtn_Apply" runat="server" sText="Übernehmen" sOnClick="mOnOk();"></ef:efButton>
				<ef:efButton id="efBtn_Abort" runat="server" sText="Abbruch" sOnClick="mOnAbort();"></ef:efButton>
				<br>
				<br>
				<br>
				<br>
				<ef:efButton id="efBtn_Search" runat="server" sText="Suchen" sOnClick="mOnSearch();"></ef:efButton>
			</div>
			<P></P>
		</form>
	</BODY>
</HTML>
