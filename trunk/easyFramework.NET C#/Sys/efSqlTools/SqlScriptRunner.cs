using System;
using System.Data.SqlClient;
using Microsoft.VisualBasic;

namespace easyFramework.Sys.SqlTools
{
	//================================================================================
	//Class:     SqlScriptRunner

	//--------------------------------------------------------------------------------'
	//Module:    SqlScriptRunner.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   class for executing a sql-skript; progress is notfied by events
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 08:33:22
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class SqlScriptRunner
	{
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		//================================================================================
		//Public Events:
		//================================================================================
		public  delegate void SqlScriptProgressEventHandler(int lPos, int lMaxPos, string sMsg);
		private static SqlScriptProgressEventHandler SqlScriptProgressEvent;
		
		public static event SqlScriptProgressEventHandler SqlScriptProgress
		{
			add
			{
				SqlScriptProgressEvent = (SqlScriptProgressEventHandler) System.Delegate.Combine(SqlScriptProgressEvent, value);
			}
			remove
			{
				SqlScriptProgressEvent = (SqlScriptProgressEventHandler) System.Delegate.Remove(SqlScriptProgressEvent, value);
			}
		}
		
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		//================================================================================
//Sub:       gRun
		//--------------------------------------------------------------------------------'
//Purpose:   runs the sql-script; several entries are seperated by "GO" & vbcrlf
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   21.04.2004 08:34:24
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static void gRun (string sConnstr, string sSqlScriptData)
		{
			
			sSqlScriptData = Strings.Replace(sSqlScriptData, "go" + "\n", "go" + "\n", 1, -1, 0);
			sSqlScriptData = Strings.Replace(sSqlScriptData, "gO" + "\n", "go" + "\n", 1, -1, 0);
			sSqlScriptData = Strings.Replace(sSqlScriptData, "Go" + "\n", "go" + "\n", 1, -1, 0);
			sSqlScriptData = Strings.Replace(sSqlScriptData, "GO" + "\n", "go" + "\n", 1, -1, 0);
			
			string[] asSplitted = Strings.Split(sSqlScriptData, "go" + "\n", -1, 0);
			
			for (int i = 0; i <= asSplitted.Length - 1; i++)
			{
				
				SqlConnection c = new SqlConnection(sConnstr);
				SqlCommand cmd = new SqlCommand(asSplitted[i], c);
				cmd.ExecuteNonQuery();
				c.Close();
				SqlScriptProgressEvent (i + 1, asSplitted.Length, asSplitted[i]);
				
				
			}
			
			
		}
		
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Private Consts:
		//================================================================================
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
		//================================================================================
		//Private Methods:
		//================================================================================
		
		
	}
	
	
	
	
}
