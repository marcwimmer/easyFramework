using System;
using easyFramework.Sys;

namespace easyFramework.Sys.Data
{
	//================================================================================
	//Class:     InternalClientInfo

	//--------------------------------------------------------------------------------'
	//Module:    InternalClientInfo.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   internal class for accessing internal with clientinfo
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 09:57:56
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	
	public class InternalClientInfo : Internal
	{
		
		
		
		public static void gUpdateTsRecordIds (ClientInfo oClientInfo, string sTableName, string sKeyField)
		{
			
			if (oClientInfo.oCurrentTransaction == null == false)
			{
				gUpdateTsRecordIds(oClientInfo.oCurrentTransaction, sTableName, sKeyField);
			}
			else
			{
				gUpdateTsRecordIds(new efTransaction(oClientInfo.sConnStr), sTableName, sKeyField);
				
			}
			
			
		}
		
		
		
		public static int glGetNextRecordID(ClientInfo oClientInfo, string sTableName)
		{
			
			if (oClientInfo.oCurrentTransaction == null == false)
			{
				return glGetNextRecordID(oClientInfo.oCurrentTransaction, sTableName);
			}
			else
			{
				return glGetNextRecordID(new efTransaction(oClientInfo.sConnStr), sTableName);
				
			}
			
			
		}
		
	}
	
}
