using System;
using System.Web;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.OnlineHelp;
using easyFramework.Sys.Security;
using easyFramework.Sys.SysEvents;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;						   
using easyFramework.Frontend.ASP.WebApp;
using System.IO;

namespace easyFramework.Project.Default
{											
	//================================================================================
	//Class:     Global

	//--------------------------------------------------------------------------------'
	//Module:    Global.asax.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Beteuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   global.asa - init of the web-project
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 17:23:22
	//--------------------------------------------------------------------------------'
	//Changed:

	//================================================================================
	//Imports:
	//================================================================================

	//--------------------------------------------------------------------------------'
	public class Global : System.Web.HttpApplication
	{

		#region " Vom Component Designer generierter Code "

		public Global() 
		{

			// Dieser Aufruf ist fr den Komponenten-Designer erforderlich.
			InitializeComponent();

			// Initialisierungen nach dem Aufruf InitializeComponent() hinzufgen

		}

		// Fr Komponenten-Designer erforderlich
		private System.ComponentModel.Container components = null;

		//HINWEIS: Die folgende Prozedur ist fr den Komponenten-Designer erforderlich
		//Sie kann mit dem Komponenten-Designer modifiziert werden.
		//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion

		
		public void Application_AuthenticateRequest (object sender, EventArgs e)
		{
			AppEventHandler.Application_AuthenticateRequest(Application, Server, Request);
		}

		public void Application_BeginRequest (object sender, EventArgs e)
		{
			AppEventHandler.Application_BeginRequest(Application, Server, Request);
		}

		public void Application_End (object sender, EventArgs e)
		{
			AppEventHandler.Application_End(Application, Server, Request);
		}

		public void Application_Error (object sender, EventArgs e)
		{
			AppEventHandler.Application_Error(Application, Server, Request, this.Context);
		}

		public void Application_Start (object sender, EventArgs e)
		{
			AppEventHandler.Application_Start(Application, Server);
		}

		public void Session_End (object sender, EventArgs e)
		{
			AppEventHandler.Session_End(Application, Server, Request);
		}

		public void Session_Start (object sender, EventArgs e)
		{
			AppEventHandler.Session_Start(Application, Server, Request);
		}

		

	}

}
