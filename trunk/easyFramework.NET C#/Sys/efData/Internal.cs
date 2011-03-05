using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Collections;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    Internal.vb
	//--------------------------------------------------------------------------------
	// Purpose:      internal data-methods; shouldn't be used by
	//               developers of own projects
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	//================================================================================
	//Imports
	//================================================================================
	
	public class Internal
	{
		
		
		
		//================================================================================
//Sub:       gUpdateTsRecordIds
		//--------------------------------------------------------------------------------'
//Purpose:   updates the entry in table tsRecordIDs for the given table
		//--------------------------------------------------------------------------------'
//Params:    the key-field-name
		//--------------------------------------------------------------------------------'
//Created:   05.04.2004 17:43:37
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static void gUpdateTsRecordIds (efTransaction oTransaction, string sTableName, string sKeyField)
		{
			
			string sQry;
			
			if (DataMethods.glDBCount(oTransaction, "tsRecordIDs", "*", "TableName='" + DataTools.SQLString(sTableName) + "'", "") == 0)
			{
				
				sQry = "INSERT INTO tsRecordIDs(TableName, MaxID) VALUES('" + DataTools.SQLString(sTableName) + "',-1)";
				DataMethods.gExecuteQuery(oTransaction, sQry);
				
			}
			
			DataMethods.gUpdateTable(oTransaction, "tsRecordIDs", "MaxID=ISNULL((SELECT MAX([" + sKeyField + "]) FROM " + sTableName + "), 0)", "TableName='" + DataTools.SQLString(sTableName) + "'");
			
			
			
		}
		
		//================================================================================
		//Function:       glGetNextRecordID
		//--------------------------------------------------------------------------------'
		//Purpose:   updates the entry in table tsRecordIDs for the given table
		//--------------------------------------------------------------------------------'
		//Params:    the key-field-name
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:43:37
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static int glGetNextRecordID(efTransaction oTransaction, string sTableName)
		{
			
		
			
			return 1 + DataMethods.glGetDBValue(oTransaction, "tsRecordIDs", "ISNULL(MaxID, 0) As MaxId", "TableName='" + sTableName + "'", 0);
			
			
			
		}
		
	}
	
}
