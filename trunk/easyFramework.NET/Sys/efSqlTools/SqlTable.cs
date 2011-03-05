using System;
using System.Data.SqlClient;
using System.Collections;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.SqlTools
{
	//================================================================================
	//Class:     SqlTable

	//--------------------------------------------------------------------------------'
	//Module:    SqlTable.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   a table in the database
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 08:28:20
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	public class SqlTable : SqlBase
	{


		//================================================================================
		//Private Fields:
		//================================================================================
		private SqlField[] maoSqlFields;
		private bool mbExistsInDatabase;

		//================================================================================
		//Public Consts:
		//================================================================================

		//================================================================================
		//Public Properties:
		//================================================================================
		public SqlField[] aoSqlFields
		{
			get 
			{
				return maoSqlFields;
			}
		}

		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================
		public SqlTable(string sConnstr, string sName) : base(sConnstr, sName) 
		{
			mbExistsInDatabase = false;

		}



		//================================================================================
		//Function:  gbExistsTable
		//--------------------------------------------------------------------------------'
		//Purpose:   checks, wether a table exists
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 22:31:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbExistsTable()
		{

			SqlConnection c = goGetConnection();
			SqlCommand oCmd = new SqlCommand("SELECT COUNT(*) FROM sysobjects WHERE xtype='U' AND name=" + this.sSqlSafe_String(sName), c);

			c.Open();
			int lCount = System.Convert.ToInt32(oCmd.ExecuteScalar());
			c.Close();

			if (lCount > 0)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		//================================================================================
		//Sub:       gAddColumn
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a new column to the table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 08:31:28
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gAddColumn (string sColumnName, efEnumSqlDbType enType,  bool bNullable)
		{
			gAddColumn(sColumnName, enType, bNullable, null);
			}
		public void gAddColumn (string sColumnName, efEnumSqlDbType enType,  bool bNullable, 
			string sDefaultValue)
		{
			gAddColumn(sColumnName, enType, -1, bNullable,sDefaultValue);
			}
		
		public void gAddColumn (string sColumnName, efEnumSqlDbType enType, int lSize, bool bNullable, string sDefaultValue)
		{

			string sQry = "ALTER TABLE " + sSqlSafe_Identif(sName) + " ADD " + sSqlSafe_Identif(sColumnName) + " " + gsCSqlType(enType, lSize);

			if (bNullable == true)
			{
				sQry += " NULL ";
			}
			else
			{
				sQry += " NOT NULL ";
			}

			if (!Functions.IsEmptyString(sDefaultValue))
			{
				sQry += " DEFAULT(" + sDefaultValue + ")";
			}

			SqlConnection c = goGetConnection();
			SqlCommand cmd = new SqlCommand(sQry, c);
			c.Open();
			cmd.ExecuteNonQuery();
			c.Close();

			Sync();

		}


		//================================================================================
		//Sub:       gRenameColumn
		//--------------------------------------------------------------------------------'
		//Purpose:   renames a column in the db
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   22.04.2004 00:51:43
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gRenameColumn (string sOldColumnName, string sNewColumnName)
		{

			SqlConnection c = goGetConnection();
			SqlCommand cmd = new SqlCommand("EXEC sp_rename " + this.sSqlSafe_String(this.sSqlSafe_Identif(sName) + "." + this.sSqlSafe_Identif(sOldColumnName)) + "," + this.sSqlSafe_String(sNewColumnName) + "," + "'COLUMN'", c);
			c.Open();
			cmd.ExecuteNonQuery();
			c.Close();


		}

		//================================================================================
		//Sub:       gAddColumn
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a new table; at least, the primary-column has to be named
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 08:31:28
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gCreateTable (string sPrimaryColumnName, efEnumSqlDbType enPrimaryColumnType, int lColumnSize)
		{

			string sQry = "CREATE TABLE " + sSqlSafe_Identif(sName) + " ( " + sSqlSafe_Identif(sPrimaryColumnName) + " " + gsCSqlType(enPrimaryColumnType, lColumnSize) + " PRIMARY KEY " + ")";

			SqlConnection c = goGetConnection();
			SqlCommand cmd = new SqlCommand(sQry, c);

			c.Open();
			cmd.ExecuteNonQuery();
			c.Close();

			Sync();

		}
		//================================================================================
		//Function:  gbExistsColumn
		//--------------------------------------------------------------------------------'
		//Purpose:   returns, wether a certain column exists or not
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 08:31:40
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbExistsColumn(string sName)
		{

			SqlConnection c = goGetConnection();

			SqlCommand oCmd = new SqlCommand("SELECT COUNT(*) FROM syscolumns sc INNER JOIN sysobjects so " + "ON so.id = sc.id WHERE so.xtype='U' AND so.name=" + this.sSqlSafe_String(this.sName) + " AND sc.name=" + this.sSqlSafe_String(sName), c);

			c.Open();
			int lCount = System.Convert.ToInt32(oCmd.ExecuteScalar());
			c.Close();

			if (lCount > 0)
			{
				return true;
			}
			else
			{
				return false;
			}


		}

		public override void Sync ()
		{

			SqlConnection c = goGetConnection();

			SqlCommand oCmd = new SqlCommand("SELECT * FROM sysobjects WHERE xtype='U' AND name=" + this.sSqlSafe_String(sName), c);
			c.Open();

			SqlDataReader dr = oCmd.ExecuteReader();

			maoSqlFields = null;
			if (dr.Read())
			{
				mbExistsInDatabase = true;
				sDbId = System.Convert.ToString(dr.GetInt32(dr.GetOrdinal("id")));
			}
			else
			{
				mbExistsInDatabase = false;
				sDbId = "";
			}
			dr.Close();


			//-------------get columns----------------
			if (mbExistsInDatabase)
			{
				oCmd = new SqlCommand("SELECT * FROM syscolumns WHERE id=" + sDbId, c);
				dr = oCmd.ExecuteReader();
				ArrayList oAlFields = new ArrayList();
				while (dr.Read())
				{
		
					SqlField oSqlField = new SqlField(sConnStr, dr.GetString(dr.GetOrdinal("name")), this);
					oAlFields.Add(oSqlField);
		
		
		
		
				};
				dr.Close();
	
	
				//-------------set local columns----------------
				if (oAlFields.Count > 0)
				{
					maoSqlFields = new SqlField[oAlFields.Count-1 + 1];
					oAlFields.CopyTo(maoSqlFields);
		
				}
	
			}


			c.Close();

		}
		//================================================================================
		//Protected Properties:
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
