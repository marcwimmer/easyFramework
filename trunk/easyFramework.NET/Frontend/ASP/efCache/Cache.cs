using System;
using easyFramework.Sys.ToolLib;
using easyFramework.Interfaces;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     Cache

	//--------------------------------------------------------------------------------'
	//Module:    Cache.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   global cache object, where often used objects can be stored
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 15:02:09
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	
	public class Cache
	{
		public Cache()
		{
			moContainer = new efHashTable();
		}
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private efHashTable moContainer;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       gAddObject
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a new object
		//--------------------------------------------------------------------------------'
		//Params:    dtExpires -
		//--------------------------------------------------------------------------------'
		//Created:   03.06.2004 00:37:28
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gAddObject (string sUniqueName, ICachedItem obj) 
		{
			int lExpiresEveryXXXSeconds = 60 * 60 * 24;
			DateTime dDate = DataConversion.efNullDate;
			gAddObject(sUniqueName, obj, dDate, lExpiresEveryXXXSeconds);
		}
		public void gAddObject (string sUniqueName, ICachedItem obj, DateTime dtDefinitleyExpires, int lExpiresEveryXXXSeconds)
		{
			CacheObject oCacheItem = new CacheObject();
			oCacheItem.dtCreated = Functions.Now();
			oCacheItem.dtDefinitleyExpires = dtDefinitleyExpires;
			oCacheItem.dtLastUpdated = Functions.Now();
			oCacheItem.obj = obj;
			oCacheItem.gExpiresEveryXXXXSeconds = lExpiresEveryXXXSeconds;
			
			moContainer.Add(sUniqueName, oCacheItem);
			
			//--------set new expiration-date, because it just came updated in here
			oCacheItem.gSetNewExpirationDate();
			
		}
		
		
		//================================================================================
//Function:  gHasObject
		//--------------------------------------------------------------------------------'
//Purpose:   returns true, if the object already exists
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   03.06.2004 00:49:00
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public bool gHasObject(string sUniqueName)
		{
			return moContainer.ContainsKey(sUniqueName);
			
		}
		
		//================================================================================
//Function:  gGetObject
		//--------------------------------------------------------------------------------'
//Purpose:   returns the given object
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   03.06.2004 00:37:47
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public object gGetObject(string sUniqueName)
		{
			
			CacheObject oCachedItem;
			oCachedItem = ((CacheObject)(moContainer[sUniqueName]));
			
			if (oCachedItem.bHasExpired())
			{
				oCachedItem.obj.gHandleExpiration();
				
				if (! oCachedItem.gSetNewExpirationDate())
				{
					oCachedItem.obj.gHandleCacheDeath();
				}
				
			}
			
			return oCachedItem.obj;
			
		}
		
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Private Consts:
		//================================================================================
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
		//================================================================================
		//Private Methods:
		//================================================================================
		
		
		//================================================================================
//Class:     CacheObject
		
		//--------------------------------------------------------------------------------'
//Module:    Cache.vb
		//--------------------------------------------------------------------------------'
//Purpose:   a cached-item
		//--------------------------------------------------------------------------------'
//Created:   21.04.2004 15:05:47
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		private class CacheObject
		{
			public CacheObject()
			{
				dtCreated = DateTime.Parse("1/1/1900");
				dtLastUpdated = DateTime.Parse("1/1/1900");
				dtDefinitleyExpires = DateTime.Parse("1/1/1900");
				dtExpires = DateTime.Parse("1/1/1900");
			}
			
			public DateTime dtCreated;
			public DateTime dtLastUpdated;
			public DateTime dtDefinitleyExpires;
			public DateTime dtExpires;
			
			public ICachedItem obj;
			
			public int gExpiresEveryXXXXSeconds;
			
			public bool bHasExpired()
			{
				if (dtExpires != DateTime.Parse("1/1/1900"))
				{
					if (dtExpires < dtLastUpdated)
					{
						return true;
					}
				}
				return false;
			}
			
			public bool gSetNewExpirationDate()
			{
				
				//-------if passed the definitley-expired range, then no new expiration can be set--------
				if (dtDefinitleyExpires != DateTime.Parse("1/1/1900"))
				{
					if (Functions.Now() > dtDefinitleyExpires)
					{
						return false;
					}
				}
				
				dtLastUpdated = Functions.Now();
				dtExpires = Functions.DateAdd(Functions.efEnumDateInterval.Second, gExpiresEveryXXXXSeconds, dtLastUpdated);
				return true;
			}
			
		}
		
	}
	
}
