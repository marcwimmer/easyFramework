using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Frontend.ASP;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Security;
using easyFramework.Sys.Entities;

namespace easyFramework.Project.SurveyManager
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

	public class workPlaceDataTable : easyFramework.Frontend.ASP.Dialog.efDataPage
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
				lPage = DataConversion.glCInt(oRequest.selectSingleNode("//page").sText, 0);
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
				lPageSize = DataConversion.glCInt(oRequest.selectSingleNode("//rowsperpage").sText);
				if (lPageSize < 1)
				{
					lPageSize = 1;
				}
			}
		
			string sSvg_Id;
			if (oRequest.selectSingleNode("//svg_id") == null)
			{
				sSvg_Id = "";
				return "Bitte whlen Sie eine Gruppe aus! Der Inhalt wir anschlieend dargestellt.";
			}
			else
			{
				sSvg_Id = oRequest.selectSingleNode("//svg_id").sText;
			}
		
			Recordset rsSurveys = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdSurveys", "svy_id, svy_name, svy_svt_id, svy_externalid", "svy_svg_id='" + sSvg_Id + "'", "", "", "");
		
		
			int lRecordcount = DataMethodsClientInfo.glGetDBValue(oClientInfo, "tdSurveys", "COUNT(*) As Anzahl", "svy_svg_id='" + sSvg_Id + "'", 0);
		
		
			string[] asCaptions = new string[] { "Projekt", "Typ", "externe ID" };
			string[] asWidths = new string[] { "40%", "20%", "20%" };
		
		
			return Tools.gsRsToDataTableString(asCaptions, asWidths, rsSurveys, "svy_id", lRecordcount);
		
		
		}
	}

}
