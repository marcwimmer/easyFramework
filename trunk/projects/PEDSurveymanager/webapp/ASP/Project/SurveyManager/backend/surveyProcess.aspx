<%@ Page Language="cs" AutoEventWireup="false" Src="surveyProcess.aspx.cs" Inherits="easyFramework.Project.SurveyManager.surveyProcess" %>
<%@ import namespace="easyFramework.Sys.ToolLib" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools" %>
<%

	if (!Functions.IsEmptyString(sBorderHtmlBegin))
	{
		Response.Write(sBorderHtmlBegin);
	}
	else {
%>
<html>
  <head>
    <title><%=sSurveyTitle%></title>
    <link rel="stylesheet" type="text/css" href="<%=sCssOfSurvey%>"/>
  </head>
  <body>
 <%
	}
 %>

	<%Response.Write(gsListItems());%> 
	
	
	
  <%if (!Functions.IsEmptyString(sBorderHtmlEnd)) {
		Response.Write(sBorderHtmlEnd);
	} else {
  %>
  </body>
</html>
<%
	}
%>
