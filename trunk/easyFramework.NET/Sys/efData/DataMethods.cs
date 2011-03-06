using System.Diagnostics;
using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys;

namespace easyFramework.Sys.Data
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    DataMethods.vb
	//--------------------------------------------------------------------------------
	// Purpose:      contains db-relevant methods
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	//================================================================================
	//Const
	//================================================================================
	
	//================================================================================
	//Imports
	//================================================================================
	
	
	public class DataMethods
	{
		
		
		//================================================================================
		//Function:  gsGetDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the value of the first field in the first dataset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:51:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static string gsGetDBValue(string sConnstr, string sTable, string sField, string sWhereClause, string sOnEOF)
		{
			
			return gsGetDBValue(new efTransaction(sConnstr), sTable, sField, sWhereClause, sOnEOF);
			
		}
		public static string gsGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause, string sOnEOF)
		{
			return gsGetDBValue(oTransaction, sTable, sField, sWhereClause, sOnEOF, "");
		}
		public static string gsGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause, string sOnEOF, string sOrderBy)
		{
			
			
			Recordset rs = DataMethods.gRsGetTable(oTransaction, sTable, sField, sWhereClause, "", "", sOrderBy);
			if (rs.EOF)
			{
				return sOnEOF;
			}
			else
			{
				return rs.oFields[0].sValue;
			}
			
		}
		
		//================================================================================
		//Function:  glGetDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the value of the first field in the first dataset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:51:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static int glGetDBValue(string sConnstr, string sTable, string sField, string sWhereClause, int sOnEOF)
		{
			return glGetDBValue(new efTransaction(sConnstr), sTable, sField, sWhereClause, sOnEOF);
			
		}
		public static int glGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause, int sOnEOF)
		{
			
			Recordset rs = DataMethods.gRsGetTable(oTransaction, sTable, sField, sWhereClause, "", "", "");
			if (rs.EOF)
			{
				return sOnEOF;
			}
			else
			{
				return rs.oFields[0].lValue;
			}
			
		}
		
		//================================================================================
		//Function:  gbGetDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the value of the first field in the first dataset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:51:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static bool gbGetDBValue(string sConnstr, string sTable, string sField, string sWhereClause, bool bOnEOF)
		{
			
			return gbGetDBValue(new efTransaction(sConnstr), sTable, sField, sWhereClause, bOnEOF);
			
		}		
		public static bool gbGetDBValue(string sConnstr, string sTable, string sField, string sWhereClause)
		{
	
			return gbGetDBValue(new efTransaction(sConnstr), sTable, sField, sWhereClause, false);
	
		}
		public static bool gbGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause)
		{
			return gbGetDBValue(oTransaction, sTable, sField, sWhereClause, false);
		}
		public static bool gbGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause, bool bOnEOF)
		{
			
			Recordset rs = DataMethods.gRsGetTable(oTransaction, sTable, sField, sWhereClause, "", "", "");
			if (rs.EOF)
			{
				return bOnEOF;
			}
			else
			{
				return rs.oFields[0].bValue;
			}
			
		}
		//================================================================================
		//Function:  gdGetDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the value of the first field in the first dataset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:51:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static decimal gdGetDBValue(string sConnstr, string sTable, string sField, string sWhereClause, decimal dOnEOF)
		{
			
			return gdGetDBValue(new efTransaction(sConnstr), sTable, sField, sWhereClause, dOnEOF);
		}
		public static decimal gdGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause, decimal dOnEOF)
		{
			
			Recordset rs = DataMethods.gRsGetTable(oTransaction, sTable, sField, sWhereClause, "", "", "");
			if (rs.EOF)
			{
				return dOnEOF;
			}
			else
			{
				return rs.oFields[0].dValue;
			}
			
		}
		
		//================================================================================
		//Function:  gdtGetDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the value of the first field in the first dataset
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:51:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static DateTime gdtGetDBValue(string sConnstr, string sTable, string sField, string sWhereClause, DateTime dtOnEOF)
		{
			
			return gdtGetDBValue(new efTransaction(sConnstr), sTable, sField, sWhereClause, dtOnEOF);
			
		}
		public static DateTime gdtGetDBValue(efTransaction oTransaction, string sTable, string sField, string sWhereClause, DateTime dtOnEOF)
		{
			
			Recordset rs = DataMethods.gRsGetTable(oTransaction, sTable, sField, sWhereClause, "", "", "");
			if (rs.EOF)
			{
				return dtOnEOF;
			}
			else
			{
				return rs.oFields[0].dtValue;
			}
			
		}
		
		//================================================================================
		//Function:  glCount
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the count(*) of the table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   the count of lines
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:45:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static int glDBCount(string sConnStr, string sTableName, string sFieldList, string sWhereClause, string sGroupClause)
		{
			
			return glDBCount(new efTransaction(sConnStr), sTableName, sFieldList, sWhereClause, sGroupClause);
			
		}
		
		public static int glDBCount(efTransaction oTransaction, string sTableName, string sFieldList, string sWhereClause, string sGroupClause)
		{
			
			
			string sQry;
			sQry = "SELECT Count($1) AS Count FROM $2  " + "$3 " + "$4 ";
			
			if (Functions.IsEmptyString(sFieldList)) sFieldList = "*";
			
			sQry = Functions.Replace(sQry, "$1", sFieldList);
            sQry = Functions.Replace(sQry, "$2", sTableName);
			if (!Functions.IsEmptyString(sWhereClause))
			{
                sQry = Functions.Replace(sQry, "$3", " WHERE " + sWhereClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$3", "");
			}
			if (!Functions.IsEmptyString(sGroupClause))
			{
				sQry = Functions.Replace(sQry, "$4", " GROUP BY  " + sGroupClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$4", "");
			}
			
			
			
			return gRsGetDirect(oTransaction, sQry).oFields[0].lValue;
			
			
			
		}
		
		//================================================================================
		//Sub:   gRsGetTable
		//--------------------------------------------------------------------------------'
		//Purpose:   gets a recordset by several params
		//--------------------------------------------------------------------------------'
		//Params:    'Params:    conn-string
		//           Tablename
		//           optional fieldlist
		//           optional Where-clause (e.g. "f1='anton'")
		//           optional group-clause (e.g. "group by value")
		//           optional having-clause (e.g. "group by value")
		//           optional order by-clause (e.g. "group by value")
		//--------------------------------------------------------------------------------'
		//Returns:   Recordset
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:41:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static Recordset gRsGetTable(string sConnStr, string sTableName, string sFieldList, string sWhereClause, string sGroupClause, string sHavingClause, string sOrderByClause)
		{
			
			return gRsGetTable(new efTransaction(sConnStr), sTableName, sFieldList, sWhereClause, sGroupClause, sHavingClause, sOrderByClause);
			
		}
		
		
		public static Recordset gRsGetTable(efTransaction oTransaction, string sTableName, string sFieldList, string sWhereClause, string sGroupClause, string sHavingClause, string sOrderByClause)
		{
			
			
			
			string sQry;
			sQry = "SELECT $1 FROM $2  " + "$3 " + "$4 " + "$5 " + "$6 ";


            sQry = Functions.Replace(sQry, "$1", sFieldList);
            sQry = Functions.Replace(sQry, "$2", sTableName);
			if (!Functions.IsEmptyString(sWhereClause))
			{
				sQry = Functions.Replace(sQry, "$3", " WHERE " + sWhereClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$3", "");
			}
			if (!Functions.IsEmptyString(sGroupClause))
			{
				sQry = Functions.Replace(sQry, "$4", " GROUP BY  " + sGroupClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$4", "");
			}
			if (!Functions.IsEmptyString(sHavingClause))
			{
                sQry = Functions.Replace(sQry, "$5", " HAVING  " + sHavingClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$5", "");
			}
			if (!Functions.IsEmptyString(sOrderByClause))
			{
                sQry = Functions.Replace(sQry, "$6", " ORDER BY  " + sOrderByClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$6", "");
			}
			
			
			return gRsGetDirect(oTransaction, sQry);
			
		}
		
		
		//================================================================================
		//Sub:   gRsGetTablePaged
		//--------------------------------------------------------------------------------'
		//Purpose:   gets a recordset by several params
		//--------------------------------------------------------------------------------'
		//Params:    'Params:    oClientinfo
		//           sTablename
		//           lPageNumber - the number of the current page
		//           lPageSize - the number of how many records are in a page
		//           optional fieldlist
		//           optional Where-clause (e.g. "f1='anton'")
		//           optional group-clause (e.g. "group by value")
		//           optional having-clause (e.g. "group by value")
		//           optional order by-clause (e.g. "group by value")
		//--------------------------------------------------------------------------------'
		//Returns:   Recordset
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:41:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static Recordset gRsGetTablePaged(
			string sConnstr, 
			string sTableName, 
			int lPageNumber, 
			int lPageSize, 
			string sKeyField, 
			string sFieldList, 
			string sWhereClause, 
			string sGroupClause, 
			string sHavingClause, 
			string sOrderByClause, 
			ref int lTotalRecordcount)
		{
			
			return gRsGetTablePaged( 
				new efTransaction(sConnstr), 
				sTableName, 
				lPageNumber, 
				lPageSize, 
				sKeyField, 
				sFieldList, 
				sWhereClause, 
				sGroupClause, 
				sHavingClause, 
				sOrderByClause, 
				ref lTotalRecordcount);
			
		}
		
		public static Recordset gRsGetTablePaged(
			efTransaction oTransaction, 
			string sTableName, 
			int lPageNumber, 
			int lPageSize, 
			string sKeyField, 
			string sFieldList, 
			string sWhereClause, 
			string sGroupClause, 
			string sHavingClause, 
			string sOrderByClause, 
			ref int lTotalRecordcount)
		{
			
			string sQry;
			sQry = "SELECT $1 FROM $2  " + "$3 " + "$4 " + "$5 " + "$6 ";

            sQry = Functions.Replace(sQry, "$1", sFieldList);
			sQry = Functions.Replace(sQry, "$2", sTableName);
			if (!Functions.IsEmptyString(sWhereClause))
			{
                sQry = Functions.Replace(sQry, "$3", " WHERE " + sWhereClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$3", "");
			}
			if (!Functions.IsEmptyString(sGroupClause))
			{
                sQry = Functions.Replace(sQry, "$4", " GROUP BY  " + sGroupClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$4", "");
			}
			if (!Functions.IsEmptyString(sHavingClause))
			{
                sQry = Functions.Replace(sQry, "$5", " HAVING  " + sHavingClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$5", "");
			}
			if (!Functions.IsEmptyString(sOrderByClause))
			{
                sQry = Functions.Replace(sQry, "$6", " ORDER BY  " + sOrderByClause);
			}
			else
			{
                sQry = Functions.Replace(sQry, "$6", "");
			}
			
				 
			Recordset rs = gRsGetDirectPaged(oTransaction, sQry, lPageNumber, lPageSize, ref lTotalRecordcount);
			
			
			
			
			return rs;
			
			
		}
		//================================================================================
		//Function:  gRsGetDirect
		//--------------------------------------------------------------------------------'
		//Purpose:   returns a recordset by executing the query
		//--------------------------------------------------------------------------------'
		//Params:    oClientinfo
		//           sQry
		//--------------------------------------------------------------------------------'
		//Returns:   recordset
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 22:39:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static Recordset gRsGetDirect(string sConnStr, string sQry)
		{
			
			return gRsGetDirect(new efTransaction(sConnStr), sQry);
			
		}
		
		public static Recordset gRsGetDirect(efTransaction oTransaction, string sQry)
		{
			
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			
			System.Data.SqlClient.SqlDataReader dr;
			System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sQry, c, oTransaction.oTransaction);
			
			dr = cmd.ExecuteReader();
			
			Recordset rsResult = new Recordset();
			
			//felder erzeugen:
			mAppendFieldsFromDataReader(dr, ref rsResult);
			
			
			while (dr.Read())
			{
				
				int i = 0;
				rsResult.AddNew();
				
				foreach (easyFramework.Sys.RecordsetObjects.Field f in rsResult.oFieldDefList)
				{
					
					
					if (DataConversion.gbIsNull(dr.GetValue(i)))
					{
						rsResult.oFields[f.sName].sValue = easyFramework.Sys.ToolLib.DataConversion.efNullValue;
					}
					else
					{
						rsResult.oFields[f.sName].sValue = DataConversion.gsCStr(dr.GetValue(i));
						
					}
					i += 1;
				}
				
			} ;
			dr.Close();
			
			rsResult.MoveFirst();
			
			if (bCloseConn)
			{
				c.Close();
			}
			
			return rsResult;
			
		}
		
		//================================================================================
		//Function:  gRsGetDirectPaged
		//--------------------------------------------------------------------------------'
		//Purpose:   returns a recordset by executing the query; it is optimized for
		//           paging for multi-row dialogs or in any paging
		//--------------------------------------------------------------------------------'
		//Params:    oClientinfo
		//           sQry
		//           lPageNumber - the number of the current page
		//           lPageSize - the number of how many records are in a page
		//--------------------------------------------------------------------------------'
		//Returns:   recordset
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 22:39:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static Recordset gRsGetDirectPaged(
			string sConnstr, 
			string sQry, 
			int lPageNumber, 
			int lPageSize, 
			ref int lTotalRecordcount)
		{

			
			return gRsGetDirectPaged(
				new efTransaction(sConnstr), 
				sQry, 
				lPageNumber, 
				lPageSize, 
				ref lTotalRecordcount);
			
		}
		
		public static Recordset gRsGetDirectPaged(
			efTransaction oTransaction, 
			string sQry, 
			int lPageNumber, 
			int lPageSize, 
			ref int lTotalRecordcount)
		{
			
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			int lStartRec;
			int lEndRec;
			lStartRec = DataConversion.glCInt(lPageSize *(lPageNumber - 1), 0);
			lEndRec = DataConversion.glCInt(lPageSize *(lPageNumber - 1), 0) + lPageSize - 1;
			
			
			//-----------get recordcount---------
			string sQryForCount = sQry;
			//es darf keine order-by anweisung vorhanden sein, wenn eine top anweisung vorhanden ist:
			if (sQryForCount.ToLower().IndexOf("order by") > 0)
			{
				sQryForCount = sQryForCount.Substring(0, sQryForCount.ToLower().IndexOf("order by"));
			}
			string sSqlCount = "SELECT COUNT(*) AS Count FROM ($1) AS queryresult";
            sSqlCount = Functions.Replace(sSqlCount, "$1", sQryForCount);
			lTotalRecordcount = glDBValue(oTransaction, sSqlCount, 0, 0);
			
			
			
			System.Data.SqlClient.SqlDataReader dr;
			System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sQry, c, oTransaction.oTransaction);
			
			dr = cmd.ExecuteReader();
			
			Recordset rsResult = new Recordset();
			
			
			
			//felder erzeugen:
			mAppendFieldsFromDataReader(dr, ref rsResult);
			
			int lCounter = -1;
			while (dr.Read())
			{
				
				lCounter += 1;
				
				if (lCounter >= lStartRec & lCounter <= lEndRec)
				{
					
					int i = 0;
					rsResult.AddNew();
					
					foreach (easyFramework.Sys.RecordsetObjects.Field f in rsResult.oFieldDefList)
					{
						
						
						if (DataConversion.gbIsNull(dr.GetValue(i)))
						{
							rsResult.oFields[f.sName].sValue = easyFramework.Sys.ToolLib.DataConversion.efNullValue;
						}
						else
						{
							rsResult.oFields[f.sName].sValue = DataConversion.gsCStr(dr.GetValue(i));
							
						}
						i += 1;
					}
				}
			} ;
			dr.Close();
			
			rsResult.MoveFirst();
			
			if (bCloseConn)
			{
				c.Close();
			}
			
			
			return rsResult;
			
		}
		
		//================================================================================
		//Sub:   gExecuteQuery
		//--------------------------------------------------------------------------------'
		//Purpose:   just executes sql-statement
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo, sql-query
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:07:25
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static void gExecuteQuery (string sConnstr, string sQry)
		{
			gExecuteQuery(new efTransaction(sConnstr), sQry);
		}
		public static void gExecuteQuery (efTransaction oTransaction, string sQry)
		{
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sQry, c, oTransaction.oTransaction);
			
			try 
			{
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex) 
			{
				throw ex;
			}
			finally 
			{ 

				if (bCloseConn)
				{
					c.Close();
				}
			}
			
		}
		
		
		//================================================================================
		//Sub:   gUpdateTable
		//--------------------------------------------------------------------------------'
		//Purpose:   builds simple update-statement for database
		//--------------------------------------------------------------------------------'
		//Params:    oClientinfo
		//           Tablename
		//           Set-Clause (e.g. "f1 = 2, F3='a'")
		//           optional Where-clause (e.g. "f1='anton'")
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:10:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static void gUpdateTable (string sConnstr, string sTableName, string sSetClause, string sWhereClause)
		{
			
			gUpdateTable(new efTransaction(sConnstr), sTableName, sSetClause, sWhereClause);
			
		}
		
		public static void gUpdateTable (efTransaction oTransaction, string sTableName, string sSetClause, string sWhereClause)
		{
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			
			string sQry;
			sQry = "UPDATE [$1] " + "SET $2 " + "$3";

            sQry = Functions.Replace(sQry, "$1", sTableName);
            sQry = Functions.Replace(sQry, "$2", sSetClause);
			if (!Functions.IsEmptyString(sWhereClause))
			{
                sQry = Functions.Replace(sQry, "$3", " WHERE " + sWhereClause);
			}
			
			
			
			SqlCommand cmd = new SqlCommand(sQry, c, oTransaction.oTransaction);
			cmd.ExecuteNonQuery();
			if (bCloseConn)
			{
				c.Close();
			}
			
		}
		
		//================================================================================
		//Sub:   gInsertTable
		//--------------------------------------------------------------------------------'
		//Purpose:   builds simple insert-statement for database
		//--------------------------------------------------------------------------------'
		//Params:    oClientinfo
		//           Tablename
		//           sFieldList, like: "SessionID, Name1, Name2"
		//           sValueList, like: "1,'Marc','Wimmer'
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:10:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static void gInsertTable (string sConnstr, string sTableName, string sFieldList, string sValueList)
		{
			
			gInsertTable(new efTransaction(sConnstr), sTableName, sFieldList, sValueList);
			
		}
		
		public static void gInsertTable (efTransaction oTransaction, string sTableName, string sFieldList, string sValueList)
		{
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			
			string sQry;
			sQry = "INSERT INTO [$1]($2) VALUES($3) ";
			
			sQry = Functions.Replace(sQry, "$1", sTableName);
            sQry = Functions.Replace(sQry, "$2", sFieldList);
            sQry = Functions.Replace(sQry, "$3", sValueList);
			
			
			
			SqlCommand cmd = new SqlCommand(sQry, c, oTransaction.oTransaction);
			cmd.ExecuteNonQuery();
			if (bCloseConn)
			{
				c.Close();
			}
			
			
		}
		
		//================================================================================
		//Sub:   gDeleteTable
		//--------------------------------------------------------------------------------'
		//Purpose:   builds simple delete-statement for database
		//--------------------------------------------------------------------------------'
		//Params:    oClientinfo
		//           Tablename
		//           optional Where-clause (e.g. "f1='anton'")
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:10:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static void gDeleteTable (string sConnstr, string sTableName, string sWhereClause)
		{
			
			gDeleteTable(new efTransaction(sConnstr), sTableName, sWhereClause);
			
			
		}
		
		public static void gDeleteTable (efTransaction oTransaction, string sTableName, string sWhereClause)
		{
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			
			string sQry;
			sQry = "DELETE FROM [$1] " + "$2";

            sQry = Functions.Replace(sQry, "$1", sTableName);
			if (!Functions.IsEmptyString(sWhereClause))
			{
                sQry = Functions.Replace(sQry, "$2", " WHERE " + sWhereClause);
			}
			
			
			SqlCommand cmd = new SqlCommand(sQry, c, oTransaction.oTransaction);
			cmd.ExecuteNonQuery();
			
			if (bCloseConn)
			{
				c.Close();
			}
			
		}
		
		public static void gTruncateTable (string sConnstr, string sTableName)
		{
			
			gTruncateTable(new efTransaction(sConnstr), sTableName);
			
			
		}
		
		public static void gTruncateTable (efTransaction oTransaction, string sTableName)
		{
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			
			string sQry;
			sQry = "TRUNCATE TABLE [$1] ";

            sQry = Functions.Replace(sQry, "$1", sTableName);
			
			SqlCommand cmd = new SqlCommand(sQry, c, oTransaction.oTransaction);
			cmd.ExecuteNonQuery();
			
			if (bCloseConn)
			{
				c.Close();
			}
			
		}
		
		//================================================================================
		//Sub:   gUpdateMemo
		//--------------------------------------------------------------------------------'
		//Purpose:   updates long text
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//           sTable - tablename
		//           sIDField - the name of the id-field
		//           sIDValue - value of the id-column
		//           sMemoField - name of the memo-field
		//           sMemo - the string-content
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 00:42:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static void gUpdateMemo (string sConnstr, string sTable, string sIDField, string sIDValue, string sMemoField, string sMemo)
		{
			
			gUpdateMemo(new efTransaction(sConnstr), sTable, sIDField, sIDValue, sMemoField, sMemo);
			
		}
		
		
		public static void gUpdateMemo (efTransaction oTransaction, string sTable, string sIDField, string sIDValue, string sMemoField, string sMemo)
		{
			
			SqlConnection c;
			bool bCloseConn = false;
			if (oTransaction.oConnection.State == ConnectionState.Open)
			{
				c = oTransaction.oConnection;
			}
			else
			{
				c = new SqlConnection(oTransaction.oConnection.ConnectionString);
				c.Open();
				bCloseConn = true;
			}
			
			
			string sQry;
			sQry = "UPDATE $1 SET $2 = @memocontent WHERE $3 = '$4'";
            sQry = Functions.Replace(sQry, "$1", sTable);
            sQry = Functions.Replace(sQry, "$2", sMemoField);
            sQry = Functions.Replace(sQry, "$3", sIDField);
            sQry = Functions.Replace(sQry, "$4", sIDValue);
			
			SqlCommand oCommand = new SqlCommand(sQry, c, oTransaction.oTransaction);
			oCommand.Parameters.Add("@memocontent", SqlDbType.NText);
			oCommand.Parameters["@memocontent"].Value = sMemo;
			
			
			
			oCommand.ExecuteNonQuery();
			
			if (bCloseConn)
			{
				c.Close();
			}
			
			
			
		}
		
		
		
		//================================================================================
		//Function:  gsDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the first value of the first field of the first record as
		//           string
		
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo and the sql-query
		//           default-values for null and eof
		//--------------------------------------------------------------------------------'
		//Returns:   string
		//--------------------------------------------------------------------------------'
		//Created:   02.04.2004 01:29:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		
		public static string gsDBValue(string sConnstr, string sQry, string sOnEOF, string sOnNull)
		{
			
			
			return gsDBValue(new efTransaction(sConnstr), sQry, sOnEOF, sOnNull);
			
		}
		
		public static string gsDBValue(efTransaction oTransaction, string sQry, string sOnEOF, string sOnNull)
		{
			
			Recordset rs = gRsGetDirect(oTransaction, sQry);
			if (rs.EOF)
			{
				return sOnEOF;
			}
			else if (rs.oFields[0].bIsNull())
			{
				return sOnNull;
			}
			else
			{
				return rs.oFields[0].sValue;
			}
		}
		
		
		//================================================================================
		//Function:  gbExists
		//--------------------------------------------------------------------------------'
		//Purpose:   returns true, if any entry exists
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 21:11:25
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static bool gbExists(string sConnStr, string sFromClause, string sWhereClause, string sFields)
		{
			
			return gbExists(new efTransaction(sConnStr), sFromClause, sWhereClause, sFields);
			
		}
		public static bool gbExists(efTransaction oTransaction, string sFromClause, string sWhereClause, string sFields)
		{
			
			string sQry;
			sQry = "SELECT CASE WHEN EXISTS(SELECT $1 FROM $2 WHERE $3) THEN 1 ELSE 0 END AS bExists";

            sQry = Functions.Replace(sQry, "$1", sFields);
            sQry = Functions.Replace(sQry, "$2", sFromClause);
            sQry = Functions.Replace(sQry, "$3", sWhereClause);
			
			int lValue = glDBValue(oTransaction, sQry, 0, 0);
			
			if (lValue == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
			
		}
		
		
		//================================================================================
		//Function:  glDBValue
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the first value of the first field of the first record as
		//           integer

		//--------------------------------------------------------------------------------'
		//Params:    clientinfo and the sql-query
		//           default-values for null and eof
		//--------------------------------------------------------------------------------'
		//Returns:   integer
		//--------------------------------------------------------------------------------'
		//Created:   02.04.2004 01:29:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		
		public static int glDBValue(string sConnstr, string sQry, int sOnEOF, int sOnNull)
		{
			
			return glDBValue(new efTransaction(sConnstr), sQry, sOnEOF, sOnNull);
			
		}
		public static int glDBValue(efTransaction oTransaction, string sQry, int sOnEOF, int sOnNull)
		{
			
			Recordset rs = gRsGetDirect(oTransaction, sQry);
			if (rs.EOF)
			{
				return sOnEOF;
			}
			else if (rs.oFields[0].bIsNull())
			{
				return sOnNull;
			}
			else
			{
				return rs.oFields[0].lValue;
			}
		}
		
		//================================================================================
		//Private Methods:
		//================================================================================
		
		
		
		
		//================================================================================
		//Function:  mAppendFieldsFromDataReader
		//--------------------------------------------------------------------------------'
		//Purpose:   creates the field-defs from an sql-data-reader;
		//--------------------------------------------------------------------------------'
		//Params:    - the opened datareader for getting the fields
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 23:21:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static void mAppendFieldsFromDataReader (SqlDataReader oSqlReader, 
			ref easyFramework.Sys.Recordset oRs)
		{
			
			//felder erzeugen, if there is no name, then create a name:
			for (int i = 0; i <= oSqlReader.FieldCount - 1; i++)
			{
				
				if (Functions.IsEmptyString(oSqlReader.GetName(i)))
				{
					throw (new Exception("each column in the sql-statement must have a name"));
				}
				
				oRs.AppendField(oSqlReader.GetName(i), easyFramework.Sys.RecordsetObjects.Field.gEnSqlType2efType(oSqlReader.GetDataTypeName(i)));
			}
			
			
		}
		
		
		
	}
	
}
