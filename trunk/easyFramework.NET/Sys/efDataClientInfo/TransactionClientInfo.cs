using System;
using easyFramework.Sys;

namespace easyFramework.Sys.Data
{
	//================================================================================
	//Class:     TransactionClientInfo

	//--------------------------------------------------------------------------------'
	//Module:    TransactionClientInfo.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   Transaction with ClientInfo
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 09:50:54
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	
	public class efTransactionClientInfo : efTransaction
	{
		
		
		public efTransactionClientInfo(ClientInfo oClientInfo) {
			
			if (oClientInfo.bHasTransaction)
			{
				gInit(oClientInfo.oCurrentTransaction.oConnection, oClientInfo.oCurrentTransaction.oTransaction);
			}
			else
			{
				
				gInit(oClientInfo.sConnStr);
			}
			
		}
		
	}
	
}
