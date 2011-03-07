using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;


namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     Config

	//--------------------------------------------------------------------------------'
	//Module:    Config.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   configurations in surveymanager
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 20:50:13
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================
	public class Config
	{
	
	
		//================================================================================
		//Function:  gsConnstrResultDB
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the connectionstring to the result-database
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 22:27:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsConnstrResultDB(ClientInfo oClientInfo)
		{
		
			string sDatasource;
			string sInitialCatalog;
			string sUsername;
			string sPassword;
		
			sDatasource = gsGetConfig(oClientInfo, "ResultDB_DataSource");
			sInitialCatalog = gsGetConfig(oClientInfo, "ResultDB_CatalogName");
			sUsername = gsGetConfig(oClientInfo, "ResultDB_UserName");
			sPassword = gsGetConfig(oClientInfo, "ResultDB_Password");
		
			if (Functions.IsEmptyString(sDatasource) | 
				Functions.IsEmptyString(sInitialCatalog) | 
				Functions.IsEmptyString(sUsername))
			{
				throw (new efException("Please config the connection-string in the config-dialog!"));
			}
		
			return "Data Source=" + sDatasource + "; Initial Catalog=" + sInitialCatalog + "; UID=" + sUsername + ";pwd=" + sPassword;
		
		
		}
	
	
		public static void gSetValue (ClientInfo oClientInfo, string sConfigName, string sConfigValue)
		{
		
			Recordset rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdConfig", "*", "cfg_name='" + DataTools.SQLString(sConfigName) + "'", "", "", "");
		
		
			//--------------if configured value does not exist, insert it-------------
			if (rs.EOF)
			{
				string sInsert = "INSERT INTO $1($2) VALUES($3)";
			
				sInsert = Functions.Replace(sInsert, "$1", "tdConfig");
				sInsert = Functions.Replace(sInsert, "$2", "cfg_name, cfg_value");
				sInsert = Functions.Replace(sInsert, "$3", "'" + sConfigName + "','" + DataTools.SQLString(sConfigValue) + "'");
			
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, sInsert);
			
				//refetch:
				rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdConfig", "*", "cfg_name='" + DataTools.SQLString(sConfigValue) + "'", "", "", "");
			
			}
		
			string sUpdate = "UPDATE $1 SET cfg_value='$2' WHERE cfg_name='$3'";
			sUpdate = Functions.Replace(sUpdate, "$1", "tdConfig");
			sUpdate = Functions.Replace(sUpdate, "$2", DataTools.SQLString(sConfigValue));
			sUpdate = Functions.Replace(sUpdate, "$3", DataTools.SQLString(sConfigName));
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, sUpdate);
		
		}
	
	
		public static string gsGetConfig(ClientInfo oClientInfo, string sConfigName)
		{
		
			Recordset rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tdConfig", "*", "cfg_name='" + DataTools.SQLString(sConfigName) + "'", "", "", "");
		
			if (rs.EOF)
			{
				return "";
			}
			else
			{
				return rs["cfg_value"].sValue;
			}
		
		}
	
	}

}
