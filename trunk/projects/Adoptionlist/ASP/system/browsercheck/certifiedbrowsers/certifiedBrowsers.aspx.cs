using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     certifiedBrowsers

	//--------------------------------------------------------------------------------'
	//Module:    certifiedBrowsers.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   displays a list of browsers, which can be used
	//--------------------------------------------------------------------------------'
	//Created:   13.06.2004 13:49:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================
	
	
	public class certifiedBrowsers : efDialogPage
	{
		
		#region " Vom Web Form Designer generierter Code "
		
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
			
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		
//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;
		
		private void Page_Init (System.Object sender, System.EventArgs e)
		{
//CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
			//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
			InitializeComponent();
		}
		
		#endregion
		
		public override void CustomInit (XmlDocument oXmlRequest)
		{
			
			//-------------js, css, title----------------
			sTitle = "webbrowser-check";
			gAddScriptLink("../../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../../js/efTabDialog.js", true, "Javascript");
			gAddCss("../../../css/efstyledefault.css", true);
			gAddCss("../../../css/efstyledialogtable.css", true);
			gAddCss("../../../css/efstyledatatable.css", true);
			gAddCss("../../../css/efstyletabdlg.css", true);
			
		}
	}
	
}
