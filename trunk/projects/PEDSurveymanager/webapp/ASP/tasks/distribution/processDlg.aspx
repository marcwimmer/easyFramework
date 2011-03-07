<%@ Page language="cs" Src="processDlg.aspx.cs" AutoEventWireup="false" Inherits="easyFramework.Project.Default.ASP.tasks.distribution.processDlg" %>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
  <HEAD>
    <ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
  </HEAD>
  <ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
  <script language="javascript">
		
    function mOnLoad() {      
      gResizeTo(400,240);
      gPosWindowDefault();
    }

    function mOnSubmit() {
      //get xml-string from dialog-controls:
      var sParams = gsXMLDlgOutput(document.forms["frmMain"], "", true);

      //pass the xml-string to the the processing page and get the string-result:
      var sResult = gsCallServerMethod("xxxxProcess.aspx", sParams);

      //if ok then close window, otherwise alert the error:
      if (sResult.substring(0, 8) == "SUCCESS_") {
        window.close();
      }
      else MsgBox(sResult, "ERROR");
    }
	
  </script>
  <!--#include file="../../system/defaultheader.aspx.inc"-->
  <body onload="mOnLoad()">
    <form id="frmMain" onsubmit="mOnSubmit();">
      <table class="bordertable">
        <tr>
          <td>
            <ef:efXmlDialog id="EfXmlDialog1" runat="server" sDefinitionFile="Dialog.xml"></ef:efXmlDialog>
          </td>
        </tr>
      </table>
    </form>
  </body>
</HTML>

