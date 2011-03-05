using System;
using easyFramework.Sys;
using System.Data.SqlClient;
using Microsoft.VisualBasic;

namespace easyFramework.Sys.SqlTools
{
	//================================================================================
	//Class:     SqlBase

	//--------------------------------------------------------------------------------'
	//Module:    SqlBase.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   base-object for SQL-objects
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 08:21:59
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class SqlBase
	{

		//================================================================================
		//Private Fields:
		//================================================================================


		//================================================================================
		//Public Consts:
		//================================================================================
		public enum efEnumSqlDbType
		{
			efNVarChar,
			efNText,
			efInt,
			efBit,
			efFloat,
			efMoney,
			efDecimal,
			efDate
		}
		//================================================================================
		//Public Properties:
		//================================================================================
		public string sConnStr
		{
			get
			{
				return msConnstr;
			}
			set
			{
				msConnstr = value;
			}
		}
		public string sName
		{
			get
			{
				return msName;
			}
			set
			{
				msName = value;
			}
		}
		public string sDbId
		{
			get
			{
				return msDbId;
			}
			set
			{
				msDbId = value;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================
		public SqlBase(string sConnstr, string sName) 
		{
			msName = sName;
			msConnstr = sConnstr;
		}

		public static string gsCSqlType(efEnumSqlDbType enSqlType, int lColumnSize)
		{

			switch (enSqlType)
			{
				case efEnumSqlDbType.efBit:
		
					return "bit";
				case efEnumSqlDbType.efFloat:
		
					return "float";
				case efEnumSqlDbType.efInt:
		
					return "int";
				case efEnumSqlDbType.efMoney:
		
					return "money";
				case efEnumSqlDbType.efNText:
		
					return "ntext";
				case efEnumSqlDbType.efNVarChar:
					if (lColumnSize <= 0)
						throw new Exception("NVarchar-column must have a size greater than 0!");
					return "nvarchar(" + lColumnSize + ")";
				case efEnumSqlDbType.efDecimal:
		
					return "decimal";
				case efEnumSqlDbType.efDate:
		
					return "datetime";
				default:
		
					throw (new Exception("Unknown sql-datatype \"" + enSqlType.ToString() + "\""));

		
			}

		}

		public static efEnumSqlDbType genCSqlType(string sSqlType)
		{

			sSqlType = Strings.LCase(sSqlType);

			switch (sSqlType)
			{
				case "bit":
		
					return efEnumSqlDbType.efBit;
				case "float":
		
					return efEnumSqlDbType.efFloat;
				case "int":
		
					return efEnumSqlDbType.efInt;
				case "money":
		
					return efEnumSqlDbType.efMoney;
				case "ntext":
		
					return efEnumSqlDbType.efNText;
				case "nvarchar":
		
					return efEnumSqlDbType.efNVarChar;
				case "decimal":
		
					return efEnumSqlDbType.efDecimal;
				case "datetime":
		
					return efEnumSqlDbType.efDate;
				default:
		
					throw (new Exception("Unknown sql-datatype \"" + sSqlType + "\""));
		
		
			}

		}


		//================================================================================
		//Sub:       Sync
		//--------------------------------------------------------------------------------'
		//Purpose:   synchronize with database
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 23:59:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public virtual void Sync ()
		{

		}
		//================================================================================
		//Protected Properties:
		//================================================================================
		protected string msName;
		protected string msDbId;
		protected string msConnstr;

		//================================================================================
		//Protected Methods:
		//================================================================================
		protected SqlConnection goGetConnection()
		{
			SqlConnection c = new SqlConnection(msConnstr);
			return c;
		}

		protected string sSqlSafe_Identif(string sValue)
		{
			return "[" + sValue + "]";
		}
		protected string sSqlSafe_String(string sValue)
		{
			return "'" + Microsoft.VisualBasic.Strings.Replace(sValue, "'", "''", 1, -1, 0) + "'";
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
