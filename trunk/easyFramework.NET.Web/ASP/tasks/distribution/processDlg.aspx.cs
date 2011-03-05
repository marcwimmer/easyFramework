using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default.ASP.tasks.distribution
{
	//================================================================================
	//Class:     processDlg
	//--------------------------------------------------------------------------------'
	//Module:    Login.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   logins user
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 01:36:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class processDlg : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist für den ASP.NET Web Form-Designer erforderlich.
			//

			
			base.OnInit(e);

			
		}

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough()
		]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efXmlDialog EfXmlDialog1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnOk;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnAbort;
	
		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;
	
		
		#endregion
	
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			sTitle = "Verteilung";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efOnlineHelp.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
		
		
		
		
		
		
			this.EfXmlDialog1.sXmlValues = "<DIALOGINPUT></DIALOGINPUT>";
		
		
		
		}
	
	}

}
