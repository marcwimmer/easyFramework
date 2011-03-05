<%@ Page language="cs" src="tab_abonennten.aspx.cs" AutoEventWireup="false" Inherits="easyFramework.Project.Default.ASP.tasks.distribution.tabs.tab_abonennten" %>
<div style="position:absolute; visibility:hidden">
	<textarea id="javacontainer">
		
		function mOnUploadPackageClick(sDst_id, sAbo_id) {
		//execute, when the user clicks the upload-button to
		//upload a package to the receiver
			
			var sParams = "<dst_id>" + sDst_id + "</dst_id><abo_id>" + sAbo_id + "</abo_id>";
			
			var sResult = gsCallServerMethod(gsGetWebPageRoot() + "/ASP/tasks/distribution/process.aspx", sParams);
			
			if (sResult != "OK") {
				efMsgBox(sResult, "ERROR");
			}
			else {
				efMsgBox("Paket erfolgreich hochgeladen!", "INFO", "OK");
			}
			
			goFindTabDialog("EfTabDialog1").goGetTab("AbonnentPackages").refreshWithLastData();
			
		}

	</textarea>
</div>
<%=this.gsWritePackageStatus()%>