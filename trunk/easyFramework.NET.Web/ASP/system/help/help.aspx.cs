using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     help

	//--------------------------------------------------------------------------------'
	//Module:    help.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 15:50:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class help : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efTreeview EfTreeview1;
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efSearchBtn;
	
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
	
	
		//================================================================================
		//Private Fields:
		//================================================================================
		public string msStartup_Hlp_id;
	
		//================================================================================
		//Public Consts:
		//================================================================================
	
		//================================================================================
		//Public Properties:
		//================================================================================
	
		//================================================================================
		//Property:  sStartup_Hlp_id
		//--------------------------------------------------------------------------------'
		//Purpose:   the help-id which is shown on startup
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.06.2004 15:28:10
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sStartup_Hlp_id
		{
			get
			{
				return msStartup_Hlp_id;
			}
		}
	
		//================================================================================
		//Public Events:
		//================================================================================
	
		//================================================================================
		//Public Methods:
		//================================================================================
	
		//================================================================================
		//Protected Properties:
		//================================================================================
	
		//================================================================================
		//Protected Methods:
		//================================================================================
	
		//================================================================================
		//Private Consts:
		//================================================================================
	
		//================================================================================
		//Private Fields:
		//================================================================================
	
		//================================================================================
		//Private Methods:
		//================================================================================
		public override void CustomInit (XmlDocument oXmlRequest)
		{
		
			sTitle = "Online Hilfe";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
			gAddCss("../../css/efstylehelp.css", true);
		
		
			//----------------update help from xml--------------
			efEnvironment.goGetEnvironment(Application).goHelpSystem.gUpdateDatabase(oClientInfo);
		
			//-------set treeview-icons---------
			EfTreeview1.gSetDefaultSystemIcons(oClientInfo, Request.ApplicationPath);
		
			//------try to get the content for on-load-----------
			string sUrl = oXmlRequest.sGetValue("url", "");
			string sEntity = oXmlRequest.sGetValue("entity", "");
			string sQry;
		
			if (sEntity != "")
			{
				sQry = "lnk_entity='" + sEntity + "'";
			}
			else
			{
				sQry = "lnk_url='" + Functions.LCase(sUrl) + "'";
			
			}
		
			string sLinkId = DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tsHelpLinks", "lnk_hlp_id", sQry, "");
			msStartup_Hlp_id = sLinkId;
		
		
		}
	
	
	
	}

}
