using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     Login
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

	public class Login : efDialogPage
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
	
		public bool bAutoLogin = false;
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			sTitle = "Login";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efOnlineHelp.js", true, "Javascript");
			gAddScriptLink("../../js/efIESpecials.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
		
		
		
			//----------perhaps there is an autologin---------------
			string sAutologinUser = "";
			string sAutologinPassword = "";

			if (Application["autologin_user"] != null) 
			{
				sAutologinUser = Convert.ToString( Application["autologin_user"]);
				sAutologinPassword = Convert.ToString( Application["autologin_pwd"]);
			}

			if (sAutologinUser != "") 
			{
				this.EfXmlDialog1.sXmlValues = "<DIALOGINPUT>";
				this.EfXmlDialog1.sXmlValues += "<username>";
				this.EfXmlDialog1.sXmlValues += sAutologinUser;
				this.EfXmlDialog1.sXmlValues += "</username>";
					
				this.EfXmlDialog1.sXmlValues += "<password>";
				this.EfXmlDialog1.sXmlValues += sAutologinPassword;
				this.EfXmlDialog1.sXmlValues += "</password>";
					
				this.EfXmlDialog1.sXmlValues +="</DIALOGINPUT>";
			}
			else 
				this.EfXmlDialog1.sXmlValues = "<DIALOGINPUT></DIALOGINPUT>";
		
			
		
		
		
		}
	
		//================================================================================
		//Sub:       LoadClientInfo
		//--------------------------------------------------------------------------------'
		//Purpose:   override the init-event of the default-page; the reason is, that we
		//           have at this time no client-id yet. with the process of the login,
		//           the client-id is created.
		//
		//
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 23:11:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void LoadClientInfo ()
		{
			//nothing - do not load
		}
	}

}
