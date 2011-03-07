using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;


namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     DBConn

	//--------------------------------------------------------------------------------'
	//Module:    DBConn.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   for getting the connection-string to the result-database
	//--------------------------------------------------------------------------------'
	//Created:   24.05.2004 01:37:22
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================
	namespace Submission
	{
	
	
		public class DBConn
		{
		
		
		
			//================================================================================
			//Function:  gsGetConnStr_ResultDatabase
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the connection-string to the result-database
			//--------------------------------------------------------------------------------'
			//Params:    connection-string to the surveymanager database
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   24.05.2004 01:38:06
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static string gsGetConnStr_ResultDatabase(string sConnstr_Svm)
			{
			
				string sQry = "cfg_name='$1'";
			
				string sCatalogName = DataMethods.gsGetDBValue(sConnstr_Svm, "tdConfig", "cfg_value", Functions.Replace(sQry, "$1", "ResultDB_CatalogName"), "");
				string sDataSource = DataMethods.gsGetDBValue(sConnstr_Svm, "tdConfig", "cfg_value", Functions.Replace(sQry, "$1", "ResultDB_DataSource"), "");
				string sPassword = DataMethods.gsGetDBValue(sConnstr_Svm, "tdConfig", "cfg_value", Functions.Replace(sQry, "$1", "ResultDB_Password"), "");
				string sUserName = DataMethods.gsGetDBValue(sConnstr_Svm, "tdConfig", "cfg_value", Functions.Replace(sQry, "$1", "ResultDB_UserName"), "");
			
				return "data source=" + sDataSource + ";Initial Catalog=" + sCatalogName + ";" + "UID=" + sUserName + ";PWD=" + sPassword;
			
			
			}
		
		}
	}
}
