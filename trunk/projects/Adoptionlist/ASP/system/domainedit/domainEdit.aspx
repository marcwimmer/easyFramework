<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efWebComponents" %>
<%@ Page Language="cs" AutoEventWireup="false" Src="domainEdit.aspx.cs" Inherits="easyFramework.Project.Default.domainEdit"%>
<%@ Import Namespace="easyFramework.Sys.ToolLib" %>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
		<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
		<!--#include file="../defaultheader.aspx"-->
		<script language="javascript">

		function mOnLoad() {
			gResizeTo(750, 500);
			gPosWindowDefault();	
			
		}
		
		function mOnAbort() {
		
			window.close();
		}
		
		function mOnOk() {
		
			var sParams = gsXMLDlgOutput(document.forms["frmMain"], "", true);
			
			var sResult = gsCallServerMethod("domainEditProcess.aspx", sParams);
			
			if (sResult != "SUCCESS") {
				efMsgBox(sResult, "ERROR");			
			}
			else {
				var frm = document.forms["frmMain"];
				var sDom_id = frm.elements["dom_id"].value;
				
				var sNewUrl = "domainEdit.aspx?dom_id=" + sDom_id +"&ClientID=" + msClientID;
				
				location.replace(sNewUrl);
			}
			
		}
		
		function mOnDomainChange() {
			
			var frm = document.forms["frmMain"];
			var sDom_id = frm.elements["dom_id"].value;
			
			
			if (sDom_id != null && sDom_id != "") {
			
				var sNewUrl = "domainEdit.aspx?dom_id=" + sDom_id +"&ClientID=" + msClientID;
				
				location.replace(sNewUrl);
			
			
			}

		}
		
		
		function mOnDeleteDomainValue(ldvl_id, sdvl_caption) {
		
			var frm = document.forms["frmMain"];
			
			var sDeleteKeyName = "dvl_deleted_id_" + ldvl_id;
			var sId = "dvl_id_" + ldvl_id;
			var sInternalValue = "dvl_internalvalue_" + ldvl_id;
			var sCaptionName = "dvl_caption_" + ldvl_id;
			
			frm.elements[sInternalValue].value = "";
			frm.elements[sCaptionName].value = "";
			frm.elements[sDeleteKeyName].value = "1";
			
		}
		
		</script>
	</HEAD>
	<%
		moHtmlSelectDomain.oHtmlSelect["onchange"].sValue = "mOnDomainChange();";

	%>
	<body onload="mOnLoad();">
		<form id="frmMain" name="frmMain" method="post">
			<table class="borderTable" width="100%" height="100%">
				<tr height="40px">
					<td valign="top">
						<table class="dialogTable" width="400px">
							<tr>
								<td width="80px" class="dlgField">Domain:</td>
								<td width="*"><%=moHtmlSelectDomain.gRender()%></td>
							</tr>
							<%if ( !Functions.IsEmptyString(msDomainDescription)) {%>
							<tr>
								<td colspan="2" class="captionField"><%=msDomainDescription%></td>
							</tr>
							<%}%>
						</table>
						<hr>
					</td>
				</tr>
				<tr>
					<td valign="top">
						<table class="dialogTable">
							<!-- neuer domainen wert -->
							<tr>
								<td colspan="3" class="captionField">Neuer Eintrag:</td>
							</tr>
							<tr>
								<td><input type="text" size="50" name="new_internalvalue" value=""></td>
								<td><input type="text" size="50" name="new_caption" value=""></td>
								<td>&nbsp;</td>
							</tr>
						</table>
						<font size="1"><br></font>
						<table class="dialogtable">
							<tr>
								<td class="captionField">interner Wert</td>
								<td class="captionField">Beschriftung</td>
								<td class="captionField">&nbsp;</td>
							</tr>
				<%while (!rsDomValues.EOF)
				{
				%>
							<tr>
								<td>
									<input type="hidden" name="dvl_deleted_id_<%=rsDomValues["dvl_id"].sValue%>" value="0">
									<input type="hidden" size="50" name="dvl_id_<%=rsDomValues["dvl_id"].sValue%>" value="<%=rsDomValues["dvl_id"].sValue%>">
									<input type="text" size="50" name="dvl_internalvalue_<%=rsDomValues["dvl_id"].sValue%>" value="<%=rsDomValues["dvl_internalvalue"].sValue%>"></td>
								<td><input type="text" size="50" name="dvl_caption_<%=rsDomValues["dvl_id"].sValue%>" value="<%=rsDomValues["dvl_caption"].sValue%>"></td>
								<td><button type="button" style="smallbutton" onclick="mOnDeleteDomainValue('<%=rsDomValues["dvl_id"].sValue%>','<%=Functions.Replace(rsDomValues["dvl_caption"].sValue, "'", "''")%>');">x</button></td>
							</tr>
				<%
						rsDomValues.MoveNext();
					}
				%>
						</table>
					</td>
				</tr>
				
				
				<tr>
					<td valign="bottom">
						<ef:efButton id="efbtnOk" runat="server" sText="$Speichern" sOnClick="mOnOk();"></ef:efButton>
						<ef:efButton id="efbtnAbort" runat="server" sText="$Abbruch" sOnClick="mOnAbort();"></ef:efButton>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
