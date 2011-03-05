using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP
{
	/// <summary>
	//================================================================================
	//Class:	Config
	//--------------------------------------------------------------------------------
	//Module:	Config.cs
	//--------------------------------------------------------------------------------
	//Copyright:Promain Software-Betreuung GmbH, 2004	
	//--------------------------------------------------------------------------------
	//Purpose:	allows to store configuration settings in Table
	//			tsConfig as string values
	//--------------------------------------------------------------------------------
	//Created:	08.09.2004 21:00:56 Marc Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	//Changed:	
	//--------------------------------------------------------------------------------
	/// </summary>
	public class Config		
	{

		private string msConnstr;
		private easyFramework.Sys.Entities.DefaultEntity moEntity;

		public Config(ClientInfo oClientInfo, string sConnstr)
		{
			msConnstr = sConnstr;

			moEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "CONFIG");

		}


		public string sGet(ClientInfo oClientInfo, string sConfigName)
		{

			string sQry = "cfg_name LIKE '" + DataTools.SQLString(sConfigName) + "'";

			Recordset rs = moEntity.gRsSearch(oClientInfo, sQry, "cfg_name");

			if (rs.EOF)
				return null;
			else
				return rs["cfg_value"].sValue;

		}

		public DateTime dtGet(ClientInfo oClientInfo, string sConfigName)
		{

			return DataConversion.gdtCDate(
				sGet(oClientInfo, sConfigName));
		}

		public int lGet(ClientInfo oClientInfo, string sConfigName)
		{

			return DataConversion.glCInt(
				sGet(oClientInfo, sConfigName));
		}

		public void Set(ClientInfo oClientInfo, string sConfigName, string sConfigValue)
		{

			string sQry = "cfg_name LIKE '" + DataTools.SQLString(sConfigName) + "'";

			Recordset rs = moEntity.gRsSearch(oClientInfo, sQry, "cfg_name");

			if (rs.EOF) 
			{
				moEntity.gNew(oClientInfo, "");
				moEntity.oFields["cfg_name"].sValue = sConfigName;
			}
			moEntity.oFields["cfg_value"].sValue = sConfigValue;
			moEntity.gSave(oClientInfo);

		}
		public void Set(ClientInfo oClientInfo, string sConfigName, int lConfigValue)
		{

			Set(oClientInfo, sConfigName, DataConversion.gsCStr(lConfigValue));
		}
		public void Set(ClientInfo oClientInfo, string sConfigName, DateTime dtConfigValue)
		{

			Set(oClientInfo, sConfigName, DataConversion.gsCStr(dtConfigValue));
		}
	}
}
