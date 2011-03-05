using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Data;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     placeholder

	//--------------------------------------------------------------------------------'
	//Module:    placeholder.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   for storing the definied placeholders
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 07:48:44
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class placeholder : efDialogPage
	{
		public placeholder()
		{
			mlSvy_id = 0;
			mlSvg_id = 0;
		}
	
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPagedXmlDialog EfPagedXmlDialog1;
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnOk;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnAbort;
	
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
	
		protected int mlSvy_id;
		protected int mlSvg_id;
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{
		
			//------css, title,scripts---------
			sTitle = "HTML-Platzhaler";
			gAddScriptLink("../../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../../js/efMultiStructure.js", true, "Javascript");
			gAddScriptLink("../../../js/efPopupMenue.js", true, "Javascript");
			gAddScriptLink("../../../js/efPagedXmlDialog.js", true, "Javascript");
			gAddScriptLink("../../../js/efIESpecials.js", true, "VBScript");
			gAddCss("../../../css/efstyledefault.css", true);
			gAddCss("../../../css/efstyledialogtable.css", true);
			gAddCss("../../../css/efstyledatatable.css", true);
		
		
			//---------setup dialog-----
			if (oXmlRequest.sGetValue("svg_id", "") == "" & oXmlRequest.sGetValue("svy_id", "") == "")
			{
				throw (new efException("Please provide either the parameter \"svg_id\" or \"svy_id\"!"));
			}
			if (oXmlRequest.sGetValue("svg_id", "") != "")
			{
				mlSvg_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlRequest.sGetValue("svg_id", ""), 0);
			}
			if (oXmlRequest.sGetValue("svy_id", "") != "")
			{
				mlSvy_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlRequest.sGetValue("svy_id", ""), 0);
			}
		
		
			//-------setup xml-dialog---------
			EfPagedXmlDialog1.sAddParams = "<svg_id>" + mlSvg_id + "</svg_id>" + "<svy_id>" + mlSvy_id + "</svy_id>";
			EfPagedXmlDialog1.oXmlDialog.sDefinitionFile = "dialog.xml";
			EfPagedXmlDialog1.oXmlDialog.sDataPage = "placeholderData.aspx";
			EfPagedXmlDialog1.oXmlDialog.sFormName = "frmMain";
		
		}
	
	}

}
