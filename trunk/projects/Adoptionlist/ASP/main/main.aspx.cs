using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     main

	//--------------------------------------------------------------------------------'
	//Module:    main.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   main-form
	//--------------------------------------------------------------------------------'
	//Created:   31.03.2004 22:57:06
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class main : efDialogPage
	{

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
	
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efTreeview efMenuTree;

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

		public override void CustomInit(XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
	
			sTitle = efEnvironment.goGetEnvironment(Application).gsProjectName;
			gAddScriptLink("../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../js/efOnlineHelp.js", true, "Javascript");
			gAddCss("../css/efstyledefault.css", true);
			gAddCss("../css/efstyledialogtable.css", true);
	
			efMenuTree.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath);
			efMenuTree.sDataPage = "MainMenuData.aspx";
	
	
	
	
		}

	}

}
