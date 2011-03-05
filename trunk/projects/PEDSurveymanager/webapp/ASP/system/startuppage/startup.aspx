<%@ Page language="cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>easyFramework</title>
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/nav4-0" name="vs_targetSchema">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="VisualStudio.HTML" name="ProgId">
		<meta content="Microsoft Visual Studio .NET 7.1" name="Originator">
	</head>
	<script language="javascript" src=../../js/efStandard.js>
	</script>
	<script language="javascript" src=../../js/efWindow.js>
	</script>
	<script language="javascript">
	
		function mOnLoad() {
			//window.moveTo(0, 0);
			//window.resizeTo(screen.width, screen.height);
		}
	
		function showStartPage() {
		
			gsShowWindow('<%=Application["startupPage"]%>', true);
			
		}
		
	</script>
	<body bgColor="#6666cc" onload="mOnLoad()">

		<table height="100%" width="100%">
			<tr>
				<td vAlign="middle" align="center"><A onclick="showStartPage(); return false;" href="#"><img src="<%=Application["startupLogo"]%>" border="0" width="502"></A>
				<br>
				<a href="../browsercheck/certifiedbrowsers/certifiedbrowsers.aspx?clientid=NA" target="_blank"><font color="white"><b>Important notes: browser prerequisites</b></font></a>
				</td>
			</tr>
		</table>
	</body>
</html>
