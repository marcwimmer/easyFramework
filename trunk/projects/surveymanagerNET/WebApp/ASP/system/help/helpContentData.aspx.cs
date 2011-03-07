using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     helpContentData

	//--------------------------------------------------------------------------------'
	//Module:    helpContentData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   loads the content of the help
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 21:58:27
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class helpContentData : efDataPage
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

		public override string sGetData(ClientInfo oClientInfo, easyFramework.Sys.Xml.XmlDocument oRequest)
		{
	
	
			//---------------get hlp_id-------------------------------
			string sHlp_id = oRequest.sGetValue("hlp_id", "");
			string sToc_id = oRequest.sGetValue("toc_id", "");
			string sCustom_id = oRequest.sGetValue("custom_id", "");
	
			if (Functions.IsEmptyString(sHlp_id) & Functions.IsEmptyString(sToc_id)
				& Functions.IsEmptyString(sCustom_id))
			{
				return "Parameter \"hlp_id\", \"toc_id\" or \"custom_id\" is required!";
			}
	
	
	
			Recordset rsHelpChapters;
	
			if (sHlp_id != "")
			{
				rsHelpChapters = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_heading, hlp_body", "hlp_id='" + sHlp_id + "'");
			}
			else if (sToc_id != "")
			{
				rsHelpChapters = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_heading, hlp_body", "hlp_toc_id='" + sToc_id + "'");
			}
			else
			{
				rsHelpChapters = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_heading, hlp_body", "hlp_customid='" + sCustom_id + "'");
			}
	
	
			if (rsHelpChapters.EOF == true)
			{
				return "OK-||-no heading-||-no body";
			}
			else
			{
				return "OK-||-" + rsHelpChapters["hlp_heading"].sValue + "-||-" + rsHelpChapters["hlp_body"].sValue;
			}
	
		}
	}

}
