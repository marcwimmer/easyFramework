using System;
using easyFramework.Sys.Data;

namespace easyFramework.Sys.SysEvents
{
	//================================================================================
//Class:     ISysEvents
	
	//--------------------------------------------------------------------------------'
//Module:    ISysEvents.vb
	//--------------------------------------------------------------------------------'
//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
//Purpose:   for implementing a class for field "TASK" in tsSysEvents
	//--------------------------------------------------------------------------------'
//Created:   06.05.2004 23:12:14
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	
	//================================================================================
//Imports:
	//================================================================================
	
	
	namespace Interfaces
	{
		
		public interface ISysEvents
		{
			
			
			void gHandleBefore (ClientInfo oClientInfo, object oParam, ref object oReturn, ref bool bCancel);
			
			
			void gHandleAfter (ClientInfo oClientInfo, object oParam, ref object oReturn, ref bool bRollback);
			
		}
		
	}
	
	
	
}
