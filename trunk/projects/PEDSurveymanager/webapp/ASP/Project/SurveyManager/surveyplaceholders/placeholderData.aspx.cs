using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Data;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     placeholder

	//--------------------------------------------------------------------------------'
	//Module:    placeholder.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   delivers the current placeholders
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 07:48:44
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class placeholderData : efDataPage
	{
		public placeholderData()
		{
			mlSvy_id = 0;
			mlSvg_id = 0;
		}
	
	
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
	
		protected int mlSvy_id;
		protected int mlSvg_id;
	
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
		
			//---------setup dialog-----
			if (oRequest.sGetValue("svg_id", "") == "" & oRequest.sGetValue("svy_id", "") == "")
			{
				throw (new efException("Please provide either the parameter \"svg_id\" or \"svy_id\"!"));
			}
			if (oRequest.sGetValue("svg_id", "") != "")
			{
				mlSvg_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("svg_id", ""), 0);
			}
			if (oRequest.sGetValue("svy_id", "") != "")
			{
				mlSvy_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("svy_id", ""), 0);
			}
		
			//-------------get page------------
			int lPage;
			if (oRequest.sGetValue("__Page", "") == "")
			{
				lPage = -1;
			}
			else
			{
				lPage = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("__Page", ""), 0);
			}
			//-------------get page------------
			int lPageSize;
			if (oRequest.sGetValue("__dialogpagesize", "") == "")
			{
				lPageSize = 1;
			}
			else
			{
				lPageSize = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("__dialogpagesize", ""), 0);
			}
		
			//-----------------get clause---------------
			string sClause;
			if (mlSvy_id > 0)
			{
				sClause = "plc_svy_id=" + mlSvy_id;
			}
			else
			{
				sClause = "plc_svg_id=" + mlSvg_id;
			}
		
		
			//--------------get rs----------------
			int lTotalRecordCount = 0;
			Recordset rsResult = DataMethodsClientInfo.gRsGetTablePaged(oClientInfo, "tdPlaceHolders", lPage, lPageSize, "plc_id", "*", sClause, "", "", "", ref lTotalRecordCount);
		
			string sResult = Tools.gsXmlRecordset2DialogInput(rsResult, Tools.efEnumDialogInputType.efJavaScriptString, lTotalRecordCount);
		
			return sResult;
		
		}
	}

}
