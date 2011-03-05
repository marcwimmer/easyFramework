using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;

namespace easyFramework.Sys.Data
{
	//================================================================================
	//Class:     DataMethodsClientInfo
	
	//--------------------------------------------------------------------------------'
	//Module:    DataMethodsClientInfo.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   gives access to the datamethods via the client-info
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 09:40:38
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	
	//================================================================================
	//Imports:
	//================================================================================
	
	
	public class DataMethodsClientInfo : easyFramework.Sys.Data.DataMethods
	{
		
		
		public static string gsGetDBValue(ClientInfo oClientInfo, string sTable, string sField, string sWhereClause, string sOnEOF)
		{
			
			return gsGetDBValue(new efTransactionClientInfo(oClientInfo), sTable, sField, sWhereClause, sOnEOF);
			
		}
		
		public static string gsGetDBValue(ClientInfo oClientInfo, string sTable, string sField, string sWhereClause, string sOnEOF, string sOrderBy)
		{
			
			return gsGetDBValue(new efTransactionClientInfo(oClientInfo), sTable, sField, sWhereClause, sOnEOF, sOrderBy);
			
		}
		
		
		public static int glGetDBValue(ClientInfo oClientInfo, string sTable, string sField, string sWhereClause, int sOnEOF)
		{
			
			
			return glGetDBValue(new efTransactionClientInfo(oClientInfo), sTable, sField, sWhereClause, sOnEOF);
			
		}
		
		
		public static bool gbGetDBValue(ClientInfo oClientInfo, string sTable, string sField, string sWhereClause, bool sOnEOF)
		{
			
			return gbGetDBValue(new efTransactionClientInfo(oClientInfo), sTable, sField, sWhereClause, sOnEOF);
			
		}
		
		
		public static decimal gdGetDBValue(ClientInfo oClientInfo, string sTable, string sField, string sWhereClause, decimal dOnEOF)
		{
			
			return gdGetDBValue(new efTransactionClientInfo(oClientInfo), sTable, sField, sWhereClause, dOnEOF);
		}
		
		
		public static DateTime gdtGetDBValue(ClientInfo oClientInfo, string sTable, string sField, string sWhereClause, DateTime dtOnEOF)
		{
			
			return gdtGetDBValue(new efTransactionClientInfo(oClientInfo), sTable, sField, sWhereClause, dtOnEOF);
			
		}
		
		
		public static int glDBCount(ClientInfo oClientInfo, string sTableName, string sFieldList, string sWhereClause, string sGroupClause)
		{
			
			return glDBCount(new efTransactionClientInfo(oClientInfo), sTableName, sFieldList, sWhereClause, sGroupClause);
			
		}
		
		public static Recordset gRsGetTable(ClientInfo oClientInfo, string sTableName, string sFieldList, string sWhereClause)
		{
			return gRsGetTable(oClientInfo,sTableName,sFieldList,sWhereClause,"","","");
		}
		public static Recordset gRsGetTable(ClientInfo oClientInfo, string sTableName, string sFieldList, string sWhereClause, string sGroupClause, string sHavingClause, string sOrderByClause)
		{
			
			return gRsGetTable(new efTransactionClientInfo(oClientInfo), sTableName, sFieldList, sWhereClause, sGroupClause, sHavingClause, sOrderByClause);
			
		}
		
		
		public static Recordset gRsGetDirect(ClientInfo oClientInfo, string sQry)
		{
			
			return gRsGetDirect(new efTransactionClientInfo(oClientInfo), sQry);
			
		}
		
		public static Recordset gRsGetDirectPaged(ClientInfo oClientInfo, string sQry, int lPageNumber, int lPageSize, ref int lTotalRecordcount)
		{
			
			return gRsGetDirectPaged(new efTransactionClientInfo(oClientInfo), sQry, lPageNumber, lPageSize, ref lTotalRecordcount);
			
		}
		
		public static void gExecuteQuery (ClientInfo oClientInfo, string sQry)
		{
			gExecuteQuery(new efTransactionClientInfo(oClientInfo), sQry);
		}
		
		public static void gUpdateTable (ClientInfo oClientInfo, string sTableName, string sSetClause, string sWhereClause)
		{
			
			gUpdateTable(new efTransactionClientInfo(oClientInfo), sTableName, sSetClause, sWhereClause);
			
		}
		
		public static void gInsertTable (ClientInfo oClientInfo, string sTableName, string sFieldList, string sValueList)
		{
			
			gInsertTable(new efTransactionClientInfo(oClientInfo), sTableName, sFieldList, sValueList);
			
		}
		
		public static void gDeleteTable (ClientInfo oClientInfo, string sTableName, string sWhereClause)
		{
			
			gDeleteTable(new efTransactionClientInfo(oClientInfo), sTableName, sWhereClause);
			
			
		}

		public static void gTruncateTable(ClientInfo oClientInfo, string sTableName) 
		{
			gTruncateTable(new efTransactionClientInfo(oClientInfo), sTableName);
		}
		
		public static void gUpdateMemo (ClientInfo oClientInfo, string sTable, string sIDField, string sIDValue, string sMemoField, string sMemo)
		{
			
			gUpdateMemo(new efTransactionClientInfo(oClientInfo), sTable, sIDField, sIDValue, sMemoField, sMemo);
			
		}
		
		public static string gsDBValue(ClientInfo oClientInfo, string sQry, string sOnEOF, string sOnNull)
		{
			
			
			return gsDBValue(new efTransactionClientInfo(oClientInfo), sQry, sOnEOF, sOnNull);
			
		}
		
		public static bool gbExists(ClientInfo oClientInfo, string sFromClause, string sWhereClause, string sFields)
		{
			
			return gbExists(new efTransactionClientInfo(oClientInfo), sFromClause, sWhereClause, sFields);
			
		}
		
		public static int glDBValue(ClientInfo oClientInfo, string sQry, int sOnEOF, int sOnNull)
		{
			
			return glDBValue(new efTransactionClientInfo(oClientInfo), sQry, sOnEOF, sOnNull);
			
		}
		
		
		public static Recordset gRsGetTablePaged(ClientInfo oClientInfo, string sTableName, int lPageNumber, int lPageSize, string sKeyField, string sFieldList, string sWhereClause, string sGroupClause, string sHavingClause, string sOrderByClause, ref int lTotalRecordcount)
		{
			
			return gRsGetTablePaged(new efTransactionClientInfo(oClientInfo), sTableName, lPageNumber, lPageSize, sKeyField, sFieldList, sWhereClause, sGroupClause, sHavingClause, sOrderByClause, ref lTotalRecordcount);
			
		}
	}
	
}
