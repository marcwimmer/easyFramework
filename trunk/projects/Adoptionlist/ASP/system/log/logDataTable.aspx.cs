using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     entityDataTable

	//--------------------------------------------------------------------------------'
	//Module:    entityDataTable.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   data for the data-table
	//--------------------------------------------------------------------------------'
	//Created:   05.04.2004 23:54:16
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class logDataTable : easyFramework.Frontend.ASP.Dialog.efDataPage
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
	
		//================================================================================
		//Function:  sGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the data for the datatable-control
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 23:54:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
		
			int lPage;
			int lPageSize;
			if (oRequest.selectSingleNode("//page") == null)
			{
				lPage = 1;
			}
			else
			{
				lPage = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.selectSingleNode("//page").sText, 0);
				if (lPage < 1)
				{
					lPage = 1;
				}
			}
			if (oRequest.selectSingleNode("//rowsperpage") == null)
			{
				lPageSize = 20;
			}
			else
			{
				lPageSize = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.selectSingleNode("//rowsperpage").sText, 0);
				if (lPageSize < 1)
				{
					lPageSize = 1;
				}
			}
		
			int lRecordcount = 0;
			Recordset rsLogEntries = DataMethodsClientInfo.gRsGetTablePaged(oClientInfo, "tsLog", lPage, lPageSize, "*", "*", "", "", "", "", ref lRecordcount);
			
		
			FastString oSBuilder = new FastString();
			oSBuilder.Append("OK-||-");
			oSBuilder.Append(easyFramework.Sys.ToolLib.DataConversion.gsCStr(lRecordcount) + "|" + "-||-");
			oSBuilder.Append("Typ|Nachricht|Datum|Client-ID|Username|-||-");
			oSBuilder.Append("5%|45%|5%|7%|15%|-||-");
		
			while (!rsLogEntries.EOF)
			{
			
				oSBuilder.Append(rsLogEntries["log_id"].sValue + "|" + 
					rsLogEntries["log_type"].sValue + "|");
				
				//the log-entry
					string sMessage = rsLogEntries["log_message"].sValue;
					sMessage = Functions.Replace(sMessage, "|", "");
					sMessage = Functions.Replace(sMessage, "\n", "<br>");

					oSBuilder.Append(sMessage + "|");


				oSBuilder.Append(
					DataConversion.gsFormatDate(oClientInfo.sLanguage, rsLogEntries["log_date"].dtValue) + "|" + 
					rsLogEntries["log_clientid"].sValue + "|" + 
					rsLogEntries["log_username"].sValue + "|-||-");
				rsLogEntries.MoveNext();
			} 
		
		
			return oSBuilder.ToString();
		
		
		}
	}

}
