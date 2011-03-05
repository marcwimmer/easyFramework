using System;
using System.Data.SqlClient;

namespace easyFramework.Sys.SqlTools
{
	//================================================================================
	//Class:     SqlTable

	//--------------------------------------------------------------------------------'
	//Module:    SqlTable.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   a field in the database
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 08:28:20
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class SqlField : SqlBase
	{
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private SqlTable moParentTable;
		private SqlBase.efEnumSqlDbType menSqlType;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		public efEnumSqlDbType enSqlType
		{
			get{
				return menSqlType;
			}
			set
			{
				menSqlType = value;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		public SqlField(string sConnstr, string sName, SqlTable oParentTable) : base(sConnstr, sName) {
			msConnstr = sConnstr;
			msName = sName;
			moParentTable = oParentTable;
		}
		
		public override void Sync ()
		{
			
			SqlConnection c = goGetConnection();
			c.Open();
			
			SqlDataReader dr;
			SqlCommand cmd;
			
			
			
			cmd = new SqlCommand("SELECT * FROM syscolumns WHERE ID=" + sSqlSafe_String(moParentTable.sDbId) + " AND name=" + sSqlSafe_String(sName), c);
			
			dr = cmd.ExecuteReader();
			
			dr.Read();
			
			this.enSqlType = this.sXtypeToSqlType(dr.GetInt32(dr.GetOrdinal("xtype")));
			
			
			
			c.Close();
			
			
		}
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		protected efEnumSqlDbType sXtypeToSqlType(int xtype)
		{
			switch (xtype)
			{
				case 104:
					
					return SqlBase.efEnumSqlDbType.efBit;
				case 106:
					
					return SqlBase.efEnumSqlDbType.efDecimal;
				case 62:
					
					return SqlBase.efEnumSqlDbType.efFloat;
				case 56:
					
					return SqlBase.efEnumSqlDbType.efInt;
				case 60:
					
					return SqlBase.efEnumSqlDbType.efMoney;
				case 99:
					
					return SqlBase.efEnumSqlDbType.efNText;
				case 231:
					
					return SqlBase.efEnumSqlDbType.efNVarChar;
				default:
					
					throw (new Exception("Unhandled x-type: " + xtype));
					
					
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
		
		
		
	}
	
}
