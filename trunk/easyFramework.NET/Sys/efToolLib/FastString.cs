using System;
using System.Text;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.ToolLib
{
	
	//================================================================================
	//Class:     FastString

	//--------------------------------------------------------------------------------'
	//Module:    FastString.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   like StringBuilder
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 01:02:52
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class FastString
	{
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		StringBuilder moStringBuilder;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		public int lLength
		{
			get{
				return moStringBuilder.Length;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		public FastString() {
			moStringBuilder = new StringBuilder();
		}
		
		
		//================================================================================
		//Sub:       gAppend
		//--------------------------------------------------------------------------------'
		//Purpose:   append to string
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:06:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void Append (string sValue)
		{
			moStringBuilder.Append(sValue);
			
		}
		
		public override string ToString()
		{
			return moStringBuilder.ToString();
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
