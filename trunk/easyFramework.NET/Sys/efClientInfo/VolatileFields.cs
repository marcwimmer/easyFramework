using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using System.Collections;

namespace easyFramework.Sys
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    VolatileFields.vb
	//--------------------------------------------------------------------------------
	// Purpose:      manages the volatile-fields of a client-info object
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	public class VolatileFields
	{

		//================================================================================
		//private fields:
		//================================================================================
		private efHashTable moVolatileObjects;
		


		//================================================================================
		//Public Properties:
		//================================================================================
		public object this[string sName] 
		{
			get
			{

			
				foreach (DictionaryEntry o in moVolatileObjects)
				{
					if (Functions.LCase(DataConversion.gsCStr(o.Key)) == Functions.LCase(sName))
					{
						return o.Value;
					}
				}
			
				return "";			}

			set 
			{
				bool bFound = false;
				foreach (DictionaryEntry o in moVolatileObjects) 
				{
					if (Functions.LCase(DataConversion.gsCStr(o.Key)) == DataConversion.gsCStr(Functions.LCase(sName)))
					{
						moVolatileObjects[o.Key] = value;
						bFound = true;
						break;
					}
				}
				if (!bFound) 
				{
					moVolatileObjects.Add(sName, value);
					
				}
			}
		}

		//================================================================================
		//Public Methods:
		//================================================================================
		public VolatileFields()
		{
			
			moVolatileObjects = new efHashTable();
			
		}
	}
}
