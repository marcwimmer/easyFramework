using System;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data.Table
{
	
	//================================================================================
	//Class:     efTableUpdateException

	//--------------------------------------------------------------------------------'
	//Module:    efTableUpdateException.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   General Update-Exception
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 10:38:02
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class TableUpdateException : efException
	{
		
		
		public TableUpdateException(string sMsg) : base(sMsg) {
		}
	}
	
}
