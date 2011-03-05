using System;
using easyFramework;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.AddOns.OfficeTalk;
using easyFramework.Sys.Entities;
using easyFramework.Sys;
using easyFramework.Project.SurveyManager;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     downloadPublicationResults

	//--------------------------------------------------------------------------------'
	//Module:    downloadPublicationResults.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   Download the results of a publication as excel
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 10:45:26
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class downloadPublicationResults : efDownloadPage
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
	
	
	
		public override byte[] abGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
		
			string sPub_id = oRequest.sGetValue("pub_id", "");
			if (sPub_id == "")
			{
				throw (new efException("pub_id required!"));
			}
		
			//--------------create excel-file----------------------
			ExcelTalker oExcelTalk = new ExcelTalker(efEnvironment.goGetEnvironment(Application).gsTempFolder);
			string sSql = "SELECT * FROM $1";
			sSql = Functions.Replace(sSql, "$1", Preparation.ResultTableHandler.gsGetResultTableName(DataConversion.glCInt(sPub_id, 0)));
		
			string sConnStr;
			sConnStr = Config.gsConnstrResultDB(oClientInfo);
			sConnStr = OfficeTalkerUtilities.gsGetNativeConnString(sConnStr);
		
			oExcelTalk.gFillExcelWithSQLServerTable(sConnStr, sSql);
		
		
			//------------get survey-------------------------------
			IEntity oPublication = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entPublications);
			oPublication.gLoad(oClientInfo, sPub_id);
		
			IEntity oSurvey = EntityLoader.goLoadEntity(oClientInfo, svmEnums.entSurveys);
			oSurvey.gLoad(oClientInfo, oPublication.oFields["pub_svy_id"].sValue);
		
			//---------------transfer settings---------------------
			this.bTransferBinary = true;
			this.enMimeType = efEnumSupportedMIME.efBrowserStandard;
			this.sFileName = Functions.gsValidFilename(oSurvey.oFields["svy_name"].sValue + ".xls");
		
		
		
			byte[] oResult;
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
