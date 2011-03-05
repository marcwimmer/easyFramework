using System;

namespace easyFramework.Interfaces
{
	//================================================================================
	//interface:     ICachedItem

	//--------------------------------------------------------------------------------'
	//Module:    ICachedItem.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   Items, which are cached, must implement the ICachedItem Interface
	//--------------------------------------------------------------------------------'
	//Created:   03.06.2004 00:38:50
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	public interface ICachedItem
	{
		
		//object should reload itself, when expired
		void gHandleExpiration ();
		
		//cache-death: object is dead forever, and cannot be reinvoked
		void gHandleCacheDeath ();
		
		
	}
}
