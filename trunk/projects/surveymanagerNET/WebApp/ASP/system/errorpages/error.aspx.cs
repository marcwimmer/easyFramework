using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default.ASP.system.errorpages
{
	/// <summary>
	/// Zusammenfassung f�r error.
	/// </summary>
	public class error : efDialogPage
	{
		

		#region Vom Web Form-Designer generierter Code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist f�r den ASP.NET Web Form-Designer erforderlich.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		public override void CustomInit(XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
	
			sTitle = efEnvironment.goGetEnvironment(Application).gsProjectName;
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../js/efOnlineHelp.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
	
	
	
		}
	}


	
}
