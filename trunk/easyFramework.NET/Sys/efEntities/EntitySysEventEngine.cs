using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.SysEvents;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Entities
{
	//================================================================================
	//Class:     EntitySysEventEngine
	//--------------------------------------------------------------------------------'
	//Module:    EntitySysEventEngine.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   own sysevent-class, because there is special handling for
	//           the EntitySelect-event: ids of allowed or disallowed recordsets
	//           can be fetched there. because of optimization this class stores the
	//           allowed and not allowed ids in a temporary table. It is of course
	//           faster than building an easyFramework-XML-recordset first and
	//           compare the valid ids against the xml-recordset.
	//
	//--------------------------------------------------------------------------------'
	//Created:   22.05.2004 20:49:03
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class EntitySysEventEngine : SysEventEngine
	{
		
		
		//================================================================================
		//Public Consts:
		//================================================================================
		public const string efTEMPORARY_ENTITY_SELECT_TABLE = "TEMPORARY_ENTITY_SELECT_TABLE";
		public const string efTEMPORARY_ENTITY_SELECT_TABLE_CLOSE_TRANSACTION = "TEMPORARY_ENTITY_SELECT_TABLE_CLOSE_TRANSACTION";
		public const string efTEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS = "TEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS";
		public const string efTEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS = "TEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS";
		
		
		
		//================================================================================
//Sub:       mHandleSqlEvent
		//--------------------------------------------------------------------------------'
//Purpose:   override the sql-event to handle the select-statement of ids
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   22.05.2004 20:54:13
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		protected override void mHandleSqlEvent (ClientInfo oClientInfo, Recordset rsSysEvent, string sSqlValue, ref object[] oReturnedObjects, bool bIsBefore)
		{
			
			
			string sQry;
			sQry = rsSysEvent["sys_task"].sValue;
			sQry = Functions.Replace(sQry, "$1", sSqlValue);
			sQry = Functions.Replace(sQry, "$usr_id", oClientInfo.rsLoggedInUser["usr_id"].sValue);
			
			if (Functions.LCase(Functions.Left(sQry, 6)) == "select" & bIsBefore & rsSysEvent["sys_category"].sValue == "EntitySelect")
			{
				
				//check if both or only one field is there:
				bool bHasAllowed = false;
				bool bHasForbidden = false;
				
				if (Functions.InStr2(Functions.LCase(sQry), " as allowed") > -1)
				{
					bHasAllowed = true;
				}
				if (Functions.InStr2(Functions.LCase(sQry), " as forbidden") > -1)
				{
					bHasForbidden = true;
				}
				
				if (bHasAllowed)
				{
					oClientInfo.oVolatileField[efTEMPORARY_ENTITY_SELECT_TABLE_HAS_ALLOWED_FIELDS] = "1";
				}
				if (bHasForbidden)
				{
					oClientInfo.oVolatileField[efTEMPORARY_ENTITY_SELECT_TABLE_HAS_FORBIDDEN_FIELDS] = "1";
				}
				
				
				if (bHasAllowed == false & bHasForbidden == false)
				{
					throw (new efException("At least the \"as Allowed\" or \"as Forbidden\" is required in the select statement!"));
				}
				
				//----------create a temporary table to store the ids. Store the
				//           name of the temporary table in the clientinfo volatile-field
				//           "TEMPORARY_ENTITY_SELECT_TABLE"
				//           Redirect the select-statement to insert the values in the
				//           tempoary table (critical!)
				string sTemporaryTableName = "#" + Functions.gsGetRandomString(10);
				oClientInfo.oVolatileField[efTEMPORARY_ENTITY_SELECT_TABLE] = sTemporaryTableName;
				if (oClientInfo.bHasTransaction == false)
				{
					oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
					oClientInfo.oVolatileField[EntitySysEventEngine.efTEMPORARY_ENTITY_SELECT_TABLE_CLOSE_TRANSACTION] = "1";
				}
				gCreateTemporaryEntitySelectTable(oClientInfo, sTemporaryTableName);
				
//TODO: redirect the select into the temporary table
				string sSqlInsert = "INSERT INTO $1($2) $3";
				string sInsertFields = "";
				
				if (bHasAllowed)
				{
					sInsertFields += "allowedIds,";
				}
				if (bHasForbidden)
				{
					sInsertFields += "forbiddenIds,";
				}
				sInsertFields = Functions.Left(sInsertFields, Functions.Len(sInsertFields) - 1);
				
				sSqlInsert = Functions.Replace(sSqlInsert, "$1", sTemporaryTableName);
				sSqlInsert = Functions.Replace(sSqlInsert, "$2", sInsertFields);
				sSqlInsert = Functions.Replace(sSqlInsert, "$3", sQry);
				
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, sSqlInsert);
				
			}
			else if (Functions.LCase(Functions.Left(sQry, 5)) == "exec ")
			{
				
				Recordset rsResult = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
				
				
				//---------store the returned object---------
				if (rsResult != null)
				{
					if (oReturnedObjects == null)
					{
						oReturnedObjects = new object[1];
					}
					else
					{
						object[] newArray = new object[oReturnedObjects.Length + 1];
						oReturnedObjects.CopyTo(newArray, 0);
						oReturnedObjects = newArray;
					}
					oReturnedObjects[oReturnedObjects.Length - 1] = rsResult;
				}
				
				
			}
			else
			{
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, sQry);
			}
		}
		
		
		
		//================================================================================
//Sub:       gCreateTemporaryEntitySelectTable
		//--------------------------------------------------------------------------------'
//Purpose:   creates the temporary table for storing the ids
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   22.05.2004 21:01:18
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public void gCreateTemporaryEntitySelectTable (ClientInfo oClientInfo, string sTableName)
		{
			
			if (Functions.Left(sTableName, 1) != "#")
			{
				throw (new efException("Temporary tables must have the \"#\"-sign as the " + "first character."));
			}
			
			string sTempTableName = sTableName;
			string sCreateTempTable = "CREATE TABLE $1 (allowedIds NVARCHAR(100), forbiddenIds NVARCHAR(100))";
			sCreateTempTable = Functions.Replace(sCreateTempTable, "$1", sTempTableName);
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, sCreateTempTable);
			
		}
		
		
		
	}
	
}
