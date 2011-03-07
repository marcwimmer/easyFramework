using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     configDlg
	//--------------------------------------------------------------------------------'
	//Module:    config.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   Configuration-Dialog
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 21:06:13
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'



	//================================================================================
	//Imports:
	//================================================================================

	public class configDlg : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efXmlDialog EfXmlDialog1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnOk;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnAbort;
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
		
		
		
			sTitle = "Konfiguration";
		
			gAddScriptLink("../../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../../js/efPopupMenue.js", true, "Javascript");
			gAddCss("../../../css/efstyledefault.css", true);
			gAddCss("../../../css/efstyledialogtable.css", true);
			gAddCss("../../../css/efstyledatatable.css", true);
		
		
			//------------load config--------------
			XmlDocument xmlDialogInput = new XmlDocument("<DIALOGINPUT/>");
			Recordset rsData = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdConfig", "*", "", "", "", "");
			while (! rsData.EOF)
			{
			
				xmlDialogInput.selectSingleNode("/DIALOGINPUT").AddNode(
					rsData["cfg_name"].sValue, true).sText = 
					rsData["cfg_value"].sValue;
			
			
			
				rsData.MoveNext();
			} ;
			EfXmlDialog1.sXmlValues = xmlDialogInput.sXml;
		
		}
	
	
	
	
	
	}

}
