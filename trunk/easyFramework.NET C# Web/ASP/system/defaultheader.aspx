<%
	//Copyright Promain GmbH 2004
	//
	//Default-Header to include in all asp-pages; refer to the documentation for more
	//information
	
	Response.Expires = -1;
	
%>						
<script language="javascript">
	var msClientID;
	msClientID = getSearchParam("clientid")
	if (msClientID == null || msClientID == "") msClientID = "NOCLIENTID";
	
	//declared in standard.js
	sWebPageRoot = "<%=easyFramework.Sys.ToolLib.DataConversion.gsCStr(Application["sWebPageRoot"])%><%//Don't use ""oClientInfo.oHttpApp.sApplicationPath"", because at loginscreen we don't have a clientinfo%>";
	
</script>
