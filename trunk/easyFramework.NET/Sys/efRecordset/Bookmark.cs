using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using System.Text;

namespace easyFramework.Sys
{
	//================================================================================
	//Class:     Bookmark

	//--------------------------------------------------------------------------------'
	//Module:    Recordset.vb
	//--------------------------------------------------------------------------------'
	//Purpose:   Bookmark, for storing the current position; can be applied via
	//           the bookmark-property
	//--------------------------------------------------------------------------------'
	//Created:   08.04.2004 10:48:39
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	
	namespace RecordsetObjects
	{
		
		
		
		public class Bookmark
		{
			
			
			//================================================================================
			//Private Fields:
			//================================================================================
			private int mlPos;
			//================================================================================
			//Public Consts:
			//================================================================================
			
			//================================================================================
			//Public Properties:
			//================================================================================
			//================================================================================
//Property:  lPos
			//--------------------------------------------------------------------------------'
//Purpose:   the position, which was stored
			//--------------------------------------------------------------------------------'
//Params:
			//--------------------------------------------------------------------------------'
//Created:   08.04.2004 10:49:53
			//--------------------------------------------------------------------------------'
//Changed:
			//--------------------------------------------------------------------------------'
			public int lPos
			{
				get{
					return mlPos;
				}
			}
			
			//================================================================================
			//Public Events:
			//================================================================================
			
			//================================================================================
			//Public Methods:
			//================================================================================
			
			//================================================================================
//Sub:       New
			//--------------------------------------------------------------------------------'
//Purpose:   constructor
			//--------------------------------------------------------------------------------'
//Params:
			//--------------------------------------------------------------------------------'
//Created:   08.04.2004 10:50:03
			//--------------------------------------------------------------------------------'
//Changed:
			//--------------------------------------------------------------------------------'
			public Bookmark(int lPos) {
				mlPos = lPos;
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
			
			
		}
		
	}
}