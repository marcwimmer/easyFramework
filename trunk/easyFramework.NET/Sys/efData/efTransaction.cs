using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace easyFramework.Sys.Data
{
	//================================================================================
	//Class:     efTransaction

	//--------------------------------------------------------------------------------'
	//Module:    efTransaction.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   database-transactions
	//--------------------------------------------------------------------------------'
	//Created:   16.05.2004 12:40:55
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class efTransaction
	{
		private SqlTransaction moTransaction;
		private SqlConnection moConnection;
		
		
		public enum efEnumIsolationLevels
		{
			efChaos,
			efReadCommitted,
			efReadUncommitted,
			efRepeatableRead,
			efSerializable,
			efUnspecified
		}
		
		
		
		//================================================================================
		//Sub:       New
		//--------------------------------------------------------------------------------'
		//Purpose:   constructor
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:50:54
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efTransaction(SqlConnection oSqlConnection) {
			moConnection = oSqlConnection;
			
		}
		public efTransaction(string sConnstr) {
			gInit(sConnstr);
		}
		public efTransaction() {
			
		}
		
		
		//================================================================================
		//Property:  oConnection
		//--------------------------------------------------------------------------------'
		//Purpose:   gives access to connection-object
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:49:09
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public SqlConnection oConnection
		{
			get{
				return moConnection;
			}
		}
		
		
		//================================================================================
		//Property:  oTransaction
		//--------------------------------------------------------------------------------'
		//Purpose:   gives access to transaction-object
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:49:11
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public SqlTransaction oTransaction
		{
			get{
				return moTransaction;
			}
		}
		
		
		public void BeginTrans (efEnumIsolationLevels iso)
		{
			
			if ( moTransaction != null)
			{
				throw (new efDataException("There is already a transaction. Call commit or rollback before " + "starting a new transaction."));
			}
			
			if (moConnection.State == ConnectionState.Closed)
			{
				moConnection.Open();
			}
			moTransaction = moConnection.BeginTransaction(mIsoModes2(iso));
			
		}
		
		
		//================================================================================
		//Sub:       CommitTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   commits a transaction
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:48:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void CommitTrans ()
		{
			moTransaction.Commit();
			moTransaction = null;
		}
		
		
		//================================================================================
		//Sub:       RollbackTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   rolls back complete transaction
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:48:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void RollbackTrans ()
		{
			moTransaction.Rollback();
			moTransaction = null;
		}
		
		
		//================================================================================
		//Sub:       RollbackTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   rollback to save-point
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:48:36
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void RollbackTrans (string sSavePointName)
		{
			moTransaction.Rollback(sSavePointName);
		}
		
		
		//================================================================================
		//Sub:       gInitByConnStr
		//--------------------------------------------------------------------------------'
		//Purpose:   usually called by constructors; makes a connection with the
		//           given connstr
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 10:06:35
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gInit (string sConnstr)
		{
			moConnection = new SqlConnection(sConnstr);
			
		}
		public void gInit (SqlConnection oConnection, SqlTransaction oTransaction)
		{
			moConnection = oConnection;
			moTransaction = oTransaction;
			
		}
		
		//================================================================================
		//Sub:       Save
		//--------------------------------------------------------------------------------'
		//Purpose:   creates a savepoint
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 12:48:10
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void Save (string sSavePointName)
		{
			moTransaction.Save(sSavePointName);
		}
		
		
		//================================================================================
		//Function:  mIsoModes
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the system.data iso-level to ef-iso-level
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 00:31:15
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static efEnumIsolationLevels mIsoModes(System.Data.IsolationLevel isoMode)
		{
			
			switch (isoMode)
			{
				case IsolationLevel.Chaos:
					
					return efEnumIsolationLevels.efChaos;
				case IsolationLevel.ReadCommitted:
					
					return efEnumIsolationLevels.efReadCommitted;
				case IsolationLevel.ReadUncommitted:
					
					return efEnumIsolationLevels.efReadUncommitted;
				case IsolationLevel.RepeatableRead:
					
					return efEnumIsolationLevels.efRepeatableRead;
				case IsolationLevel.Serializable:
					
					return efEnumIsolationLevels.efSerializable;
				case IsolationLevel.Unspecified:
					
					return efEnumIsolationLevels.efUnspecified;
				default:
					
					throw (new efDataException("Unknown isolation-level: " + isoMode));
					
			}
			
		}
		
		//================================================================================
		//Function:  mIsoModes2
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the system.data iso-level to ef-iso-level
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 00:31:15
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static System.Data.IsolationLevel mIsoModes2(efEnumIsolationLevels isoMode)
		{
			
			switch (isoMode)
			{
				case efEnumIsolationLevels.efChaos:
					
					return IsolationLevel.Chaos;
				case efEnumIsolationLevels.efReadCommitted:
					
					return IsolationLevel.ReadCommitted;
				case efEnumIsolationLevels.efReadUncommitted:
					
					return IsolationLevel.ReadUncommitted;
				case efEnumIsolationLevels.efRepeatableRead:
					
					return IsolationLevel.RepeatableRead;
				case efEnumIsolationLevels.efSerializable:
					
					return IsolationLevel.Serializable;
				case efEnumIsolationLevels.efUnspecified:
					
					return IsolationLevel.Unspecified;
				default:
					
					throw (new efDataException("Unknown isolation-level: " + isoMode));
					
			}
			
		}
		
	}
	
}
