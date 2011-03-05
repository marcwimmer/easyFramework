using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
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

	public class entitySearchDataTable : easyFramework.Frontend.ASP.Dialog.efDataPage
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



			//------load entity-----------------------
			string sEntityName = oRequest.selectSingleNode("//entity").sText;
			DefaultEntity oEntity;
			oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);

			//---------------get where-clause-------------------
			string sClause;
			sClause = "";

			if (oRequest.selectSingleNode("//searchdlg") != null)
			{
	
				string sField;
				string sValue;
				string sSearchDlg = oRequest.selectSingleNode("//searchdlg").sText;
	
				if (Functions.InStr(sSearchDlg, ";") > 0)
				{
					sField = Functions.Split(sSearchDlg, ";", -1)[0];
					sValue = Functions.Split(sSearchDlg, ";", 2)[1];
		
					sValue = Functions.Replace(sValue, "*", "%");
					sClause = "[" + sField + "] LIKE '" + DataTools.SQLString(sValue) + "'";
		
				}
	
	
			}



			//-------------execute sql----------------------
			int lRecordcount = 0;
			Recordset rsEntityValues = oEntity.gRsSearch(oClientInfo, sClause, "", lPage, lPageSize, ref lRecordcount);


			return easyFramework.Frontend.ASP.ASPTools.Tools.gsEntityToDataTableString(oEntity, rsEntityValues, lRecordcount);


		}
	}

}
