using System;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Security
{
	//================================================================================
	//Class:     EntityPopupSec

	//--------------------------------------------------------------------------------'
	//Module:    EntityPopupSec.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   says, wether the client has access to which popup of an entity
	//--------------------------------------------------------------------------------'
	//Created:   18.05.2004 23:37:31
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class EntityPopupSec
	{
		public EntityPopupSec()
		{
			moEntityPopups = new efHashTable();
		}
		
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private efHashTable moEntityPopups; //cache, so the database isn't queried all
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
			moEntityPopups = new efHashTable();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		
		//================================================================================
		//Function:  gbHasAccessFromCache
		//--------------------------------------------------------------------------------'
		//Purpose:   returns true, if the user has access to the entity-popup or not;
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
		public bool gbHasAccessFromCache(ClientInfo oClientinfo, int lEpp_id)
		{
			
			
			if (oClientinfo.rsLoggedInUser["usr_supervisor"].bValue == true)
			{
				return true;
			}
			
			//----------let's see, wether there is an entry for the entity-popup-item---------
			if (! moEntityPopups.ContainsKey(lEpp_id))
			{
				
				//-------create user hash, wich contains bools, which indicate access or not access----
				efHashTable oNewUserHash = new efHashTable();
				
				moEntityPopups.Add(lEpp_id, oNewUserHash);
				
			}
			
			//-------get the user-hash; if it doesn't exist yet, create a new one------
			efHashTable oUserHash = ((efHashTable)(moEntityPopups[lEpp_id]));
			string sUserId = oClientinfo.rsLoggedInUser["usr_id"].sValue;
			
			if (oUserHash.ContainsKey(sUserId) == false)
			{
				
				bool bHasAccess = gbHasUserAccessFromDB(oClientinfo, lEpp_id);
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
		public bool gbHasUserAccessFromDB(ClientInfo oClientinfo, int lEpp_id)
		{
			
			string sUsr_id = oClientinfo.rsLoggedInUser["usr_id"].sValue;
			
			
			//-------first check, if there is explicit access given-------
			Recordset rstsEntityPopupAccessTsUsers = easyFramework.Sys.Data.DataMethodsClientInfo.gRsGetTable(oClientinfo, "tsEntityPopupAccessTsUsers", "*", "epu_usr_id=" + sUsr_id + " AND epu_epp_id='" + DataTools.SQLString(DataConversion.gsCStr(lEpp_id)) + "'", "", "", "");
			
			if (! rstsEntityPopupAccessTsUsers.EOF)
			{
				
				return rstsEntityPopupAccessTsUsers["epu_explicit_access"].bValue;
				
			}
			else
			{
				
				return gbHasAnyUserGroupAccessFromDB(oClientinfo, lEpp_id, DataConversion.glCInt(sUsr_id, 0));
				
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
		public bool gbHasAnyUserGroupAccessFromDB(ClientInfo oClientinfo, int lEpp_id, int lUsr_id)
		{
			
			//-------have a look in the user-groups of the user, if at least one has access--------
			string sSqlWhere;
			
			sSqlWhere = "epg_grp_id in (select usr_grp_grp_id from tsUsersTsUserGroups where " + "usr_grp_usr_id=$1) And epg_epp_id='$2'";
			sSqlWhere = Functions.Replace(sSqlWhere, "$1", DataConversion.gsCStr(lUsr_id));
			sSqlWhere = Functions.Replace(sSqlWhere, "$2", DataTools.SQLString(DataConversion.gsCStr(lEpp_id)));
			
			
			int lCount = DataMethodsClientInfo.glDBCount(oClientinfo, "tsEntityPopupAccessTsUserGroups", "*", sSqlWhere, "");
			
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
