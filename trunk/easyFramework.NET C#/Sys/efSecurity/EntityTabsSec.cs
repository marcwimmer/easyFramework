using System;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
	
namespace easyFramework.Sys.Security
{
	//================================================================================
	//Class:     EntityTabSec

	//--------------------------------------------------------------------------------'
	//Module:    EntityTabSec.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   says, wether the client has access to which tab of an entity
	//--------------------------------------------------------------------------------'
	//Created:   18.05.2004 23:37:31
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class EntityTabSec
	{
		public EntityTabSec()
		{
			moEntityTabs = new efHashTable();
		}
		
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private efHashTable moEntityTabs; //cache, so the database isn't queried all
		//                                        the time
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		
		//================================================================================
		//Sub:       gClearCache
		//--------------------------------------------------------------------------------'
		//Purpose:   if something of the user-rights was changed, then call this
		//           method. the cache is cleared and every access check is done again
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.05.2004 13:22:08
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gClearCache ()
		{
			moEntityTabs = new efHashTable();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		
		//================================================================================
//Function:  gbHasAccessFromCache
		//--------------------------------------------------------------------------------'
//Purpose:   returns true, if the user has access to the entity-tab or not;
		//           data is read from the cache. if there are changes in the database call
		//           gClearCache.
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   18.05.2004 23:38:39
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbHasAccessFromCache(ClientInfo oClientinfo, int lTab_id)
		{
			
			
			if (oClientinfo.rsLoggedInUser["usr_supervisor"].bValue == true)
			{
				return true;
			}
			
			//----------let's see, wether there is an entry for the entity-tab-item---------
			if (! moEntityTabs.ContainsKey(lTab_id))
			{
				
				//-------create user hash, wich contains bools, which indicate access or not access----
				efHashTable oNewUserHash = new efHashTable();
				
				moEntityTabs.Add(lTab_id, oNewUserHash);
				
			}
			
			//-------get the user-hash; if it doesn't exist yet, create a new one------
			efHashTable oUserHash = ((efHashTable)(moEntityTabs[lTab_id]));
			string sUserId = oClientinfo.rsLoggedInUser["usr_id"].sValue;
			
			if (oUserHash.ContainsKey(sUserId) == false)
			{
				
				bool bHasAccess = gbHasUserAccessFromDB(oClientinfo, lTab_id);
				oUserHash.Add(sUserId, bHasAccess);
				
				
			}
			
			//------get the bool------
			return ((bool)(oUserHash[sUserId]));
			
		}
		
		
		
		//================================================================================
//Function:  gbHasUserAccessFromDB
		//--------------------------------------------------------------------------------'
//Purpose:   checks the access by reading from database
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   21.05.2004 14:01:22
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbHasUserAccessFromDB(ClientInfo oClientinfo, int lTab_id)
		{
			
			string sUsr_id = oClientinfo.rsLoggedInUser["usr_id"].sValue;
			
			
			//-------first check, if there is explicit access given-------
			Recordset rstsEntityTabAccessTsUsers = easyFramework.Sys.Data.DataMethodsClientInfo.gRsGetTable(oClientinfo, "tsEntityTabAccessTsUsers", "*", "etu_usr_id=" + sUsr_id + " AND etu_Tab_id='" + DataTools.SQLString(DataConversion.gsCStr(lTab_id)) + "'", "", "", "");
			
			if (! rstsEntityTabAccessTsUsers.EOF)
			{
				
				return rstsEntityTabAccessTsUsers["etu_explicit_access"].bValue;
				
			}
			else
			{
				
				return gbHasAnyUserGroupAccessFromDB(oClientinfo, lTab_id, DataConversion.glCInt(sUsr_id, 0));
				
			}
			
			
		}
		
		
		//================================================================================
//Function:  gbHasAnyUserGroupAccessFromDB
		//--------------------------------------------------------------------------------'
//Purpose:   checks the access by reading from database of the most
		//           access-rich group
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   21.05.2004 14:01:22
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbHasAnyUserGroupAccessFromDB(ClientInfo oClientinfo, int lTab_id, int lUsr_id)
		{
			
			//-------have a look in the user-groups of the user, if at least one has access--------
			string sSqlWhere;
			
			sSqlWhere = "etg_grp_id in (select usr_grp_grp_id from tsUsersTsUserGroups where " + "usr_grp_usr_id=$1) And etg_tab_id='$2'";
			sSqlWhere = Functions.Replace(sSqlWhere, "$1", DataConversion.gsCStr(lUsr_id));
			sSqlWhere = Functions.Replace(sSqlWhere, "$2", DataTools.SQLString(DataConversion.gsCStr(lTab_id)));
			
			
			int lCount = easyFramework.Sys.Data.DataMethodsClientInfo.glDBCount(oClientinfo, "tsEntityTabAccessTsUserGroups", "*", sSqlWhere, "");
			
			if (lCount > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
			
			
		}
		
		
	}
	
}
