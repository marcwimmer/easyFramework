using System;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data.Table
{
	//================================================================================
	//Class:     efTableUpdateConflictException

	//--------------------------------------------------------------------------------'
	//Module:    efTableUpdateConflictException.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   thrown at method UpdateDatabase, if the recordset was changed
	//           in the mean-time
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 08:55:18
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class TableUpdateConflictException : efException
	{
		
		
		public TableUpdateConflictException(ClientInfo oClientInfo, string sTableName, string sUpdatedBy, DateTime dtUpdated) : base(msFormatMsg(oClientInfo, sTableName, sUpdatedBy, dtUpdated)) {
			
			
		}
		
		private static string msFormatMsg(ClientInfo oclientInfo, string sTableName, string sUpdatedBy, DateTime dtUpdated)
		{
			string sMsg;
			
			sMsg = "The record was changed in the mean-time by user $1 at $2";
			sMsg = Functions.Replace(sMsg, "$1", sUpdatedBy);
			sMsg = Functions.Replace(sMsg, "$2", DataConversion.gsFormatDate(oclientInfo.sLanguage, dtUpdated));
			
			return sMsg;
		}
		
	}
	
}
