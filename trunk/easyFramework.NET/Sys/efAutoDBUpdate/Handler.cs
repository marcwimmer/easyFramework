using System.Diagnostics;
using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.SqlTools;
using easyFramework.Sys.Xml;
using System.IO;

namespace easyFramework.Sys.AutoDBUpdate
{
	//================================================================================
	//Class:     Handler

	//--------------------------------------------------------------------------------'
	//Module:    Handler.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   looks in a given directory for sql-files; if there are sql-files,
	//           then this class look in the database at table DBSCRIPTS, if the
	//           there a files, which haven't been run yet. If so, the files are executed
	//           and the entries are written to the DBSCRIPTS-Tables
	//--------------------------------------------------------------------------------'
	//Created:   19.08.2004 10:27:56
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================


	public class Handler
	{


		//================================================================================
		//Private Fields:
		//================================================================================
		private string msWatchedDirectory;
		private string msConnectionString;

		//================================================================================
		//Public Consts:
		//================================================================================
		public const string efTABLENAME = "tsRunDBScripts";
		public const string efCOL_ID = "ID";
		public const string efCOL_DATE = "DATE";
		public const string efCOL_SCRIPTNAME = "SCRIPTNAME";
		public const string efENTERPRISE_COPYPASTE = "ENTERPRISE_COPYPASTE:";

		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  sWatchedDirectory
		//--------------------------------------------------------------------------------'
		//Purpose:   the directory, in which the sql-files are lieing
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   19.08.2004 10:30:58
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sWatchedDirectory
		{
			get
			{
				return msWatchedDirectory;
			}
			set
			{
				msWatchedDirectory = value;
			}
		}


		//================================================================================
		//Property:  sConnectionstring
		//--------------------------------------------------------------------------------'
		//Purpose:   connection-string to the database
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   19.08.2004 10:31:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sConnectionstring
		{
			get
			{
				return msConnectionString;
			}
			set
			{
				msConnectionString = value;
			}
		}

		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       New
		//--------------------------------------------------------------------------------'
		//Purpose:   constructor
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   19.08.2004 10:30:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Handler(string sWatchedDirectory, string sConnectionString) 
		{
			msWatchedDirectory = sWatchedDirectory;
			msConnectionString = sConnectionString;
		}


		//================================================================================
		//Sub:       gRun
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   19.08.2004 10:31:26
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gRun ()
		{
	
			//--------check, if table dbscripts exists or not------------
			mMakeSureDBLogTable();
	
	
	
			//-------open directory and iterate files----------
			if (! Directory.Exists(msWatchedDirectory))
			{
				throw (new efException("Directory \"" + msWatchedDirectory + "\" doesn't exist."));
			}
	
	
			//------get files--------
			string[] asFiles = Directory.GetFiles(msWatchedDirectory, "*.sql");
	
	
			//----put files in a sorted array------
	
			efArrayList oSorted = new efArrayList();
			for (int i = 0; i <= asFiles.Length - 1; i++)
			{
				oSorted.Add(asFiles[i]);
			}
			oSorted.Sort();
	
	
	
	
			//-----get filename without path---------
			for (int i = 0; i <= oSorted.Count - 1; i++)
			{
		
				string sFileName = easyFramework.Sys.ToolLib.DataConversion.gsCStr(oSorted[i]);
		
				if (Functions.InStr2(sFileName, "\\") > -1)
				{
					sFileName = Functions.Split(sFileName, "\\") [Functions.UBound (Functions.Split (sFileName ,  "\\"))];
				}
				else
				{
					throw (new efException("Backslash missing in path \"" + sFileName + "\". Probably not a dos-path."));
				}
		
				//-------check, wether script was run-----------
				string sSql;
				sSql = efCOL_SCRIPTNAME + "='" + DataTools.SQLString(sFileName) + "'";
		
				if (! DataMethods.gbExists(msConnectionString, efTABLENAME, sSql, "*"))
				{
			
					mRunScript(easyFramework.Sys.ToolLib.DataConversion.gsCStr(oSorted[i]));
			
			
					//-------------insert successful execution-------------
					string sCols;
					string sValues;
			
					sCols = "$1, $2";
					sCols = Functions.Replace(sCols, "$1", efCOL_ID);
					sCols = Functions.Replace(sCols, "$2", efCOL_SCRIPTNAME);
			
			
					long lHighestID = DataMethods.glDBValue(msConnectionString, "SELECT ISNULL(MAX(" + efCOL_ID + "), 0) AS MAXValue FROM " + efTABLENAME, 0, 0);
			
					sValues = "$1, '$2'";
					sValues = Functions.Replace(sValues, "$1", easyFramework.Sys.ToolLib.DataConversion.gsCStr(lHighestID + 1));
					sValues = Functions.Replace(sValues, "$2", DataTools.SQLString(sFileName));
			
					DataMethods.gInsertTable(msConnectionString, efTABLENAME, sCols, sValues);
			
			
				}
		
		
			}
	
	
	
	
	
		}

		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Sub:       mMakeSureDBLogTable
		//--------------------------------------------------------------------------------'
		//Purpose:   makes sure that table exists, which contains the sql-scripts, which were run
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   19.08.2004 10:33:00
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mMakeSureDBLogTable ()
		{
	
			SqlTools.SqlTable oTable = new SqlTools.SqlTable(msConnectionString, efTABLENAME);
	
			if (! oTable.gbExistsTable())
			{
		
				oTable.gCreateTable(efCOL_ID, SqlBase.efEnumSqlDbType.efInt, 0);
		
				oTable.gAddColumn(efCOL_DATE, SqlBase.efEnumSqlDbType.efDate, false, "getdate()");
				oTable.gAddColumn(efCOL_SCRIPTNAME, SqlBase.efEnumSqlDbType.efNVarChar, 255, true, null);
		
			}
	
	
		}


		//================================================================================
		//Sub:       mRunScript
		//--------------------------------------------------------------------------------'
		//Purpose:   runs the given sql-script
		//--------------------------------------------------------------------------------'
		//Params:    sDosScriptFilePath - the path to the script-filename
		//--------------------------------------------------------------------------------'
		//Created:   19.08.2004 10:49:40
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mRunScript (string sDosScriptFilePath)
		{
	
			string sFileContent;
			System.IO.StreamReader oStreamReader = File.OpenText(sDosScriptFilePath);
	
			sFileContent = oStreamReader.ReadToEnd();
			oStreamReader.Close();
	
	
			//--------the go-statements are important; they should be replace, so that
			//        all go-statments look like "GO" & vbcrlf
	
			string sGO = "GO" + "\r\n";
	
			sFileContent = Functions.Replace(sFileContent, "go" + "\r\n", sGO);
			sFileContent = Functions.Replace(sFileContent, "Go" + "\r\n", sGO);
			sFileContent = Functions.Replace(sFileContent, "gO" + "\r\n", sGO);
			sFileContent = Functions.Replace(sFileContent, "gO" + "\r\n", "\r\n" + sGO);
	
			sFileContent += "GO" + "\r\n"; //at least one
	
			string[] asStatements = Functions.Split(sFileContent, sGO);
	
	
			//----- iterate and execute statements in a transaction------------
			efTransaction oTrans = new efTransaction(msConnectionString);
			oTrans.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
	
	
			string sStatement = "";
			try
			{
		
				//----------step through the statements and execute them--------
				for (int i = 0; i <= Functions.UBound(asStatements); i++)
				{
			
					sStatement = asStatements[i];
			
					sStatement = Functions.Trim(sStatement);
			
					//--to avoid problems when:
					//GO
					//GO
					sStatement = Functions.Replace(sStatement, sGO, "");
			
					//-----and no go at eof:
					if (Functions.Right(sStatement, 2) == "GO")
					{
						sStatement = Functions.Left(sStatement, Functions.Len(sStatement) - 2);
					}

					//------now decide: could be a ENTERPRISE_COPYPASTE:----
					if (!Functions.IsEmptyString(sStatement))
					{

						if (sStatement.Length < efENTERPRISE_COPYPASTE.Length)
							DataMethods.gExecuteQuery(oTrans, sStatement);
						else 
						{
							if (sStatement.Substring(0, efENTERPRISE_COPYPASTE.Length) == efENTERPRISE_COPYPASTE) 
							{
								sStatement = sStatement.Substring(efENTERPRISE_COPYPASTE.Length, 
									sStatement.Length - efENTERPRISE_COPYPASTE.Length);
								sStatement = sStatement.Trim();

								this.mHandleEnterpriseCopyPaste(sStatement, oTrans);
							}
							else 
							{
								DataMethods.gExecuteQuery(oTrans, sStatement);
							}
						}
					}
			
				}
		
		
				//-------commit transaction------
				oTrans.CommitTrans();
		
			}
			catch (Exception ex)
			{
		
				//---if error, the rollback----
				oTrans.RollbackTrans();
		
		
				//-----rethrow exception---------
				throw (new Exception(ex.Message + "\r\n" + "at: " + sDosScriptFilePath + "\r\n" + sStatement));
		
			}
			finally
			{
		
			}
	
	
	
		}

		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================

		//================================================================================
		//Private Methods:
		//================================================================================

		/// <summary>
		/// enterprise-copy pastes are clipboard copies out of an enterprise-manager-table
		/// it is much more easier to enter this into a file, instead of forming it into
		/// a sql-statement
		/// </summary>
		/// <param name="sStatement">the tablename + chr(13) + chr(10) then the fields, separated by tab</param>
		/// <param name="oTrans">the current db-transaction</param>
		private void mHandleEnterpriseCopyPaste(string sStatement, efTransaction oTrans) 
		{
			string sTableName = "";
			string[] asValues;
			string sSqlStatement = "";
			string sInsertFields = "";
			string sInsertValues = "";

			//-------get table name----------
			sTableName = sStatement.Split(Convert.ToChar(Functions.vbCr))[0];
		
			//--------get the fields------------
			sStatement = Functions.Split(sStatement, Functions.VbCrLf, 2)[1];
			asValues = sStatement.Split(Convert.ToChar(Functions.VbTab));

			//-------------build sql----------------

			for (int i = 0; i < asValues.Length; i++) 
			{
				if (asValues[i] != "") 
					sInsertValues += "'" + DataTools.SQLString(asValues[i]) + "',";
			}
			sInsertValues = sInsertValues.Substring(0, sInsertValues.Length - 1);

			SqlTable oTable = new SqlTable(msConnectionString, sTableName);
			oTable.Sync();
			for (int i = 0; i < oTable.aoSqlFields.Length; i++) 
			{
				if (i + 1 < asValues.Length) 
					if (asValues[i + 1] != "") //there is leading tab - ignore it
						sInsertFields += "[" + oTable.aoSqlFields[i].sName + "],";
			}
			sInsertFields = sInsertFields.Substring(0, sInsertFields.Length - 1);


			sSqlStatement = "INSERT INTO $$$$1($$$$2) VALUES($$$$3)";
			sSqlStatement = sSqlStatement.Replace("$$$$1", sTableName);
			sSqlStatement = sSqlStatement.Replace("$$$$2", sInsertFields);
			sSqlStatement = sSqlStatement.Replace("$$$$3", sInsertValues);

			//---------execute sql-----------------
			DataMethods.gExecuteQuery(oTrans, sSqlStatement);
			
		}
	}

}
