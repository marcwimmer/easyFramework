<%@ Page Language="cs" AutoEventWireup="false" Src="certifiedBrowsers.aspx.cs" Inherits="easyFramework.Project.Default.certifiedBrowsers"%>
<%@ Register TagPrefix="ef" Namespace="easyFramework.Frontend.ASP.WebComponents" Assembly="efASPFrontend" %>
<HTML>
	<HEAD>
		<ef:efPageHeader id="EfPageHeader1" runat="server"></ef:efPageHeader>
	</HEAD>
	<ef:efScriptLinks id="EfScriptLinks1" runat="server"></ef:efScriptLinks>
	<!--#include file="../../defaultheader.aspx.inc"-->
	<body>
		<table class="borderTable" width="100%" height="100%">
			<tr>
				<td valign="top">
					<b>browser-check</b>
					<p>The easyFramework-system for web-application-development uses javascripts and 
						certain javascript-objects to create the dialog-frontend. There are 
						user-friendly components like popup-menues, paged-data-tables and so. 
						<br>
						Make sure:
					<ul>
						<li>
							<p><b> turn on javascript</b>
						</li>
						<li>
							<p><b> try to use one of the certified browsers of the list below</b>
						</li>
					</ul>
					<p>Following browsers are supported by the easyFramework-System (state: 06/13/2004)
						<table>
							<tr>
								<td class="captionField"><b>Operating System</b></td>
								<td class="captionField"><b>Browser</b></td>
								<td class="captionField"><b>Verified</b></td>
							</tr>
							<tr>
								<td class="entryField">Windows XP Prof./2000 Prof./Server./Adv.Server</td>
								<td class="entryField">Internet Explorer 6.0</td>
								<td class="entryField">OK</td>
							</tr>
							<tr>
								<td class="entryField">Windows XP Prof./2000 Prof./Server./Adv.Server</td>
								<td class="entryField">Netscape Navigator 7.1</td>
								<td class="entryField">OK</td>
							</tr>
							<tr>
								<td class="entryField">Windows XP Prof./2000 Prof./Server./Adv.Server</td>
								<td class="entryField">Netscape Navigator 6.x</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Windows XP Prof./2000 Prof./Server./Adv.Server</td>
								<td class="entryField">Netscape Navigator 4.x</td>
								<td class="entryField">incompatible</td>
							</tr>
							<tr>
								<td class="entryField">Windows XP Prof./2000 Prof./Server./Adv.Server</td>
								<td class="entryField">Mozilla 1.7</td>
								<td class="entryField">OK</td>
							</tr>
							<tr>
								<td class="entryField">Mac OS 10.3</td>
								<td class="entryField">Internet Explorer 5.2</td>
								<td class="entryField">incompatible</td>
							</tr>
							<tr>
								<td class="entryField">Mac OS 10.3</td>
								<td class="entryField">Safari</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Mac OS 10.3</td>
								<td class="entryField">Netscape Navigator</td>
								<td class="entryField">OK (some minor incompatibilities)</td>
							</tr>
							<tr>
								<td class="entryField">Mac OS 10.3</td>
								<td class="entryField">Mozilla 1.5</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">other Mac OS (like 9.x)</td>
								<td class="entryField">all browsers</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Linux-platforms</td>
								<td class="entryField">Mozilla 1.7</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Linux-platforms</td>
								<td class="entryField">Netscape Navigator 7.1</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Linux-platforms</td>
								<td class="entryField">Mozilla 1.7</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Linux-platforms</td>
								<td class="entryField">Netscape Navigator 7.1</td>
								<td class="entryField">verification ongoing</td>
							</tr>
							<tr>
								<td class="entryField">Linux-platforms</td>
								<td class="entryField">other browsers</td>
								<td class="entryField">verification ongoing</td>
							</tr>
						</table>
						<div align="center">
						<br><button type="button" onclick="window.close();">Close</button></div>
				</td>
			</tr>
		</table>
		
	</body>
</HTML>
