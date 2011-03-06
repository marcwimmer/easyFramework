<%@ Page language="cs" src="newPackageDlg.aspx.cs" AutoEventWireup="false" Inherits="easyFramework.Project.Default.ASP.tasks.distribution.newPackageDlg" %>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
  <HEAD>
    <ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
  </HEAD>
  <ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
  <script language="javascript">
		
    function mOnLoad() {      
      gResizeTo(800,240);
      gPosWindowDefault();
    }

    function mOnSubmit() {
      //get xml-string from dialog-controls:
      var sParams = "";gsXMLDlgOutput(document.forms["frmMain"], "", true);
		
      //pass the xml-string to the the processing page and get the string-result:
      var sResult = gsCallServerMethod("newPackageProcess.aspx", sParams);
	
	  
      //if ok then close window, otherwise alert the error:
      if (sResult.substring(0, 7) == "SUCCESS") {
		efMsgBox("Paket wurde erstellt. Sie können das Paket nun verteilen.", "INFO");
        window.close();
      }
      else {
		efMsgBox(sResult, "ERROR");
	  }
      
    }
	
  </script>
  <!--#include file="../../system/defaultheader.aspx.inc"-->
  <body onload="mOnLoad()">
    <form id="frmMain" onsubmit="mOnSubmit();">
      <table class="bordertable">
        <tr>
          <td>
            <ef:efXmlDialog id="EfXmlDialog1" runat="server" sDefinitionFile="newPackageDlg.xml"></ef:efXmlDialog>
          </td>
        </tr>
      </table>
    </form>
  </body>
</HTML>
