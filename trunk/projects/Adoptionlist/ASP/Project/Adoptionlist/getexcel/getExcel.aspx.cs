using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;
using easyFramework.AddOns.OfficeTalk;

namespace easyFramework.Project.Default
{
	/*
	'================================================================================
	'Class:     downloadPublicationResults

	'--------------------------------------------------------------------------------'
	'Module:    downloadPublicationResults.aspx.vb
	'--------------------------------------------------------------------------------'
	'Copyright: Promain Software-Betreuung GmbH, 2004
	'--------------------------------------------------------------------------------'
	'Purpose:   Download the results of a publication as excel
	'--------------------------------------------------------------------------------'
	'Created:   11.05.2004 10:45:26 
	'--------------------------------------------------------------------------------'
	'Changed:   
	'--------------------------------------------------------------------------------'
	*/

	//================================================================================
	//Imports:
	//================================================================================
	
	public class getExcel : efDownloadPage
	{

		#region " Vom Web Form Designer generierter Code "
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist f�r den ASP.NET Web Form-Designer erforderlich.
			//

			
			base.OnInit(e);

			
		}

		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		[System.Diagnostics.DebuggerStepThrough()
		]private void InitializeComponent ()
		{
		
		}
	
		#endregion



		public override byte[] abGetData(ClientInfo oClientInfo, XmlDocument oRequest) 
		{
			//--------------create excel-file----------------------
			ExcelTalker oExcelTalk = 
				new ExcelTalker(
				efEnvironment.goGetEnvironment(oClientInfo).gsTempFolder);		

			String  sSql  = "SELECT * FROM vwAdoptions";

			oExcelTalk.gFillExcelWithSQLServerTable(oClientInfo, sSql);

			//---------------transfer settings---------------------
			this.bTransferBinary = true;
			this.enMimeType = efEnumSupportedMIME.efBrowserStandard;
			this.sFileName = "adoptions.xls";


			Byte[] oResult;

			try 
			{
				oResult = oExcelTalk.goGetFileContent();
			}
			finally 
			{
	
				oExcelTalk.Dispose();
			}

			return oResult;

		

		}
        
	}

}