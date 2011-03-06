<%@ Page Language="cs" AutoEventWireup="false" Src="optionDialog.aspx.cs" Inherits="easyFramework.Project.Default.optionDialog"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<HEAD>
		<ef:efpageheader id="EfPageHeader1" runat="server"></ef:efpageheader><ef:efscriptlinks id="EfScriptLinks1" runat="server"></ef:efscriptlinks>
	</HEAD>
	<!--DIVMODALDIALOG is searched via indexOf, and the content starting at this comment is 
		copied into a local div-->
	<!--DIVMODALDIALOG BEGIN-->

	<!--#include file="../defaultheader.aspx.inc"-->
	<body onload="OPTDIALOG_mOnLoad();">
		<form id="OPTDIALOG_frmOptionDialog" method="post" runat="server">
			<table class="borderTable" width="400px" height="200px">
				<tr>
					<td class="captionField">
					<%=sText%>
					</td>
				</tr>
				<tr>
					<td class="entryField">
						<%
							for (int i = 0; i < aoOptions.Length; i++) {
						%>	
							<input type="radio" name="OPTDIALOG_option" ondblclick="OPTDIALOG_mOnOk();" value="<%=aoOptions[i].sValue%>">
								<a href="#" onclick="gSetRadioValue('OPTDIALOG_option', '<%=aoOptions[i].sValue%>', document.forms['OPTDIALOG_frmOptionDialog']); OPTDIALOG_mOnOk(); return false;"><%=aoOptions[i].sCaption%></a></br>
						<%	
							}
						%>
					</td>
				</tr>
				<tr>
					<td>
						<ef:efButton id="OPTDIALOG_btnOk" runat="server" sText="$OK" sOnClick="OPTDIALOG_mOnOk();"></ef:efButton>
						<ef:efButton id="OPTDIALOG_btnAbort" runat="server" sText="$Abbruch" sOnClick="OPTDIALOG_mOnAbort();"></ef:efButton>
					</td>
				</tr>
			</table>
			<textarea name="javascriptcontainerModalDialog">
				function OPTDIALOG_mOnLoad() {  	
					gResizeTo(400,240);	
					gPosWindowDefault();
					
				}

				function OPTDIALOG_mOnOk() {
					
					var sOptValue = gsRadioValue("OPTDIALOG_option", document.forms["OPTDIALOG_frmOptionDialog"]);
					
					if (sOptValue == "" || sOptValue == null) {
						efMsgBox("Bitte wählen Sie eine Option aus!", "ERROR");
						return
					}
					
					gReturnModalResult(sOptValue);
					
				}
				
				function OPTDIALOG_mOnAbort() {
					gReturnNoModalResult();
				}

				
			</textarea>
		</form>
		<!--DIVMODALDIALOG END-->
	</body>

</html>
