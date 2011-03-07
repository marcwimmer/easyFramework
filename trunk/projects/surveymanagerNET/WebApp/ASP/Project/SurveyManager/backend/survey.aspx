<%@ Page Language="cs" AutoEventWireup="false" Src="survey.aspx.cs" Inherits="easyFramework.Project.SurveyManager.survey" %>
<%@ import namespace="easyFramework.Sys.ToolLib" %>
<%@ import namespace="easyFramework.Frontend.ASP.ASPTools" %>
<%

	if (!Functions.IsEmptyString(sBorderHtmlBegin))
	{
		Response.Write(sBorderHtmlBegin);
	}
	else
	{
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
	<form id="Form1" method="post" action="surveyProcess.aspx">
	<%=sSurveyHtml%>
	<hr>
	<%if (sSurveyType == "VOTE") {%>
	<input type="submit" value="Abstimmen">
	<%} else {%>
	<input type="submit" value="Abschicken">
	<%}%>
    </form>

  <%if (!Functions.IsEmptyString(sBorderHtmlEnd))
	{
		Response.Write(sBorderHtmlEnd);
		
	} else {
  %>
  </body>
</html>
<%
}
%>