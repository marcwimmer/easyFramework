using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     domainEdit

	//--------------------------------------------------------------------------------'
	//Module:    domainEdit.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   editing the domain-values
	//--------------------------------------------------------------------------------'
	//Created:   24.05.2004 17:12:13
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class domainEdit : efDialogPage
	{

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
	
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efbtnOk;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efbtnAbort;

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

		protected ComboBox moHtmlSelectDomain;
		protected int mlDom_id;
		protected Recordset rsDomValues;
		protected string msDomainDescription;

		public override void CustomInit (XmlDocument oXmlRequest)
		{
	
	
			//---------------js, css, title-----------
			sTitle = "Domainen";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../js/efTabDialog.js", true, "Javascript");
			gAddScriptLink("../../js/efPopupMenue.js", true, "Javascript");
			gAddScriptLink("../../js/efIESpecials.js", true, "VBScript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
			gAddCss("../../css/efstyledatatable.css", true);
			gAddCss("../../css/efstyletabdlg.css", true);
	
	
			//--------get all domains---------------
			Recordset rsDomains = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDomains", "dom_id, dom_name", "", "", "", "dom_name");
	
			//------get the selected domain-----------
			string sSelectedDomain = oXmlRequest.sGetValue("dom_id", "");
			if (sSelectedDomain != "")
			{
				mlDom_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(sSelectedDomain, 0);
				msDomainDescription = DataMethodsClientInfo.gsGetDBValue(oClientInfo, "tsDomains", "dom_description", "dom_id=" + mlDom_id, "");
			}
	
			//---------setup the domain-select-combo--------
			moHtmlSelectDomain = new ComboBox();
			moHtmlSelectDomain.sName = "dom_id";
			while (! rsDomains.EOF)
			{
				moHtmlSelectDomain.gAddEntry(rsDomains["dom_id"].sValue, rsDomains["dom_name"].sValue);
				rsDomains.MoveNext();
			};
	
			//------get the domain-values, if there are any-------
			rsDomValues = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDomainValues", "*", "dvl_dom_id=" + mlDom_id, "", "", "dvl_caption");
	
			if (mlDom_id != 0)
			{
				moHtmlSelectDomain.sSelectedValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(mlDom_id);
			}
	
	
		}




	}

}
