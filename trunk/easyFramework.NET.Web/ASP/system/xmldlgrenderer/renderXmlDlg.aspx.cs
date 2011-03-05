using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     renderLevel

	//--------------------------------------------------------------------------------'
	//Module:    renderLevel.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   used to render an xml-dialog content on demand;
	//           is e.g. called from a tab-dialog
	//
	//           needs the follogwing url-parameters:
	//
	//--------------------------------------------------------------------------------'
	//Created:   02.05.2004 18:56:53
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class renderXmlDlg : efDataPage
	{

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
	
		}

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



		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
	
	
	
			//------------load xml-definition------------
			XmlDocument oXmlDef = new XmlDocument();
			oXmlDef.gLoad(Tools.sWebToAbsoluteFilename(Request, oRequest.sGetValue("xmldeffile", ""), false));
	
	
			XmlDialogRenderer oXmlDlg = new XmlDialogRenderer();
	
	
			return XmlDialogRenderer.gsRender(oClientInfo, oXmlDef, 
				null, oRequest.sGetValue("xmlformname", ""), 
				oRequest.sGetValue("xmldialogid", ""), "", 
				oRequest.sGetValue("xmldatapage", ""));
	
	
		}




	}

}
