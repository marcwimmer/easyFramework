using System;

namespace easyFramework.Sys.ToolLib
{
	//================================================================================
	//Class:     efException
		
	//--------------------------------------------------------------------------------'
	//Module:    Exception.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   base-exception
	//--------------------------------------------------------------------------------'
	//Created:   19.04.2004 14:52:10
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	public class efException : System.Exception
	{
		
		public efException(string sMsg) : base(sMsg) {
		}
		public efException(Exception ex) 
		{
			string sMsg = ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace;
			
		}
		public efException() {
		}
		public string sMsg
		{
			get{
				return base.Message;
			}
		}
	}
	
}
