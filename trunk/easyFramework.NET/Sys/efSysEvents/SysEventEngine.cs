using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.SysEvents
{
	//================================================================================
	//Class:     SysEventEngine
	//--------------------------------------------------------------------------------'
	//Module:    SysEventEngine.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   calls the sysevents
	//--------------------------------------------------------------------------------'
	//Created:   06.05.2004 23:23:48
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class SysEventEngine
	{
		
		
		//================================================================================
		//Sub:       gRaiseBeforeEvents
		//--------------------------------------------------------------------------------'
		//Purpose:   calls the event-handler
		//--------------------------------------------------------------------------------'
		//Params:    client-info object
		//           event-category of tsSysEvents
		//           event-name of tsSysEvents
		//           sSqlValue - a Parameterobject, the $1 is replaced with sSqlValue
		//           oParamObject - a Parameterobject, which is passed to classes
		//           bCancel - here is the value return, if cancel should be done
		//           oReturnedObjects - each event can return an object; this is the
		//                               array, which holds the objects
		//--------------------------------------------------------------------------------'
		//Created:   07.05.2004 00:23:37
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gRaiseBeforeEvents (ClientInfo oClientInfo, string sEventCategory, string sEventName, string sSqlValue, object oParamObject, ref bool bCancel, ref object[] oReturnedObjects)
		{
			
			Recordset rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsSysEvents", "*", "sys_category='" + DataTools.SQLString(sEventCategory) + "' AND " + "sys_name='" + DataTools.SQLString(sEventName) + "' " + " AND sys_index < 0", "", "", "sys_index");
			
			
			bCancel = false;
			
			while (! rs.EOF)
			{
				
				try
				{
					
					while (! rs.EOF)
					{
						
						
						if (rs["sys_actiontype"].sValue == "SQL")
						{
							
							mHandleSqlEvent(oClientInfo, rs, sSqlValue, ref oReturnedObjects, true);
							
						}
						else if (rs["sys_actiontype"].sValue == "CLASS")
						{
							
							
							mHandleClassEvent(oClientInfo, rs, sSqlValue, oParamObject, ref oReturnedObjects, ref bCancel, true);
							
							
							if (bCancel == true)
							{
								return;
							}
							
						}
						
						
						
						
						rs.MoveNext();
					};
					
				}
				catch (efException ex)
				{
					
					oClientInfo.gAddError(ex.sMsg);
					bCancel = true;
					return;
					
				}
				
				
			};
			
		}
		
		//================================================================================
		//Sub:       gRaiseAfterEvents
		//--------------------------------------------------------------------------------'
		//Purpose:   calls the event-handler
		//--------------------------------------------------------------------------------'
		//Params:    client-info object
		//           event-category of tsSysEvents
		//           event-name of tsSysEvents
		//           sSqlValue - a Parameterobject, set in sql-queries
		//           oParamObject - a Parameterobject, which is passed to classes
		//           bCancel - here is the value return, if cancel should be done
		//           oReturnedObjects - each event can return an object; this is the
		//                               array, which holds the objects
		//--------------------------------------------------------------------------------'
		//Created:   07.05.2004 00:23:37
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gRaiseAfterEvents (ClientInfo oClientInfo, string sEventCategory, string sEventName, string sSqlValue, object oParamObject, ref bool bRollback, ref object[] oReturnedObjects)
		{
			
			Recordset rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsSysEvents", "*", "sys_category='" + DataTools.SQLString(sEventCategory) + "' AND " + "sys_name='" + DataTools.SQLString(sEventName) + "' " + " AND sys_index >= 0", "", "", "sys_index");
			
			
			bRollback = false;
			
			while (! rs.EOF)
			{
				
				try
				{
					
					while (! rs.EOF)
					{
						
						
						if (rs["sys_actiontype"].sValue == "SQL")
						{
							
							mHandleSqlEvent(oClientInfo, rs, sSqlValue, ref oReturnedObjects, false);
							
						}
						else if (rs["sys_actiontype"].sValue == "CLASS")
						{
							
							mHandleClassEvent(oClientInfo, rs, sSqlValue, oParamObject, ref oReturnedObjects, ref bRollback, false);
							
							if (bRollback == true)
							{
								return;
							}
						}
						
						
						rs.MoveNext();
					};
					
				}
				catch (efException ex)
				{
					
					oClientInfo.gAddError(ex.sMsg);
					return;
					
				}
				
				
			};
			
		}
		
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       mHandleSqlEvent
		//--------------------------------------------------------------------------------'
		//Purpose:   handles an sql-event
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//           rsSysEvent - recordset of the current sysevent
		//           sSqlValue - the sql-parameter which replaces the "$1"
		//           oReturnedObjects - the returned objects to the sysevent-caller
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 18:10:02
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected virtual void mHandleSqlEvent (ClientInfo oClientInfo, Recordset rsSysEvent, string sSqlValue, ref object[] oReturnedObjects, bool bIsBefore)
		{
			
			string sQry;
			sQry = rsSysEvent["sys_task"].sValue;
			sQry = Functions.Replace(sQry, "$1", sSqlValue);
			
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, sQry);
			
		}
		
		
		//================================================================================
		//Sub:       mHandleClassEvent
		//--------------------------------------------------------------------------------'
		//Purpose:   handles a class-event
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 18:10:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected virtual void mHandleClassEvent (ClientInfo oClientInfo, Recordset rsSysEvent, string sSqlValue, object oParamObject, ref object[] oReturnedObjects, ref bool bRollback, bool bIsBefore)
		{
			
			if (Functions.IsEmptyString(rsSysEvent["sys_assembly"].sValue))
			{
				throw (new efException("Assembly-name missing in tsSysEvents for " + rsSysEvent["sys_category"].sValue + "/" + rsSysEvent["sys_name"].sValue));
				
			}
			
			
			Interfaces.ISysEvents aHandler;
			System.Reflection.Assembly oAssembly;
			oAssembly = System.Reflection.Assembly.Load(rsSysEvent["sys_assembly"].sValue);
			aHandler = ((Interfaces.ISysEvents)(oAssembly.CreateInstance(rsSysEvent["sys_task"].sValue, true)));
			
			if (aHandler == null)
			{
				throw (new efException("Couldn't instantiate event-handler \"" + rsSysEvent["sys_task"].sValue + "\""));
			}
			
			object oReturnedObject = null;
			
			if (bIsBefore)
			{
				aHandler.gHandleBefore(oClientInfo, oParamObject, ref oReturnedObject, ref bRollback);
				
			}
			else
			{
				aHandler.gHandleAfter(oClientInfo, oParamObject, ref oReturnedObject, ref bRollback);
				
			}
			
			//---------store the returned object---------
			if (oReturnedObject != null)
			{
				if (oReturnedObjects == null)
				{
					
				}
				else
				{
					
				}
				oReturnedObjects[oReturnedObjects.Length-1] = oReturnedObject;
			}
			
			
			aHandler = null;
			oAssembly = null;
			
		}
		
		
	}
	
}
