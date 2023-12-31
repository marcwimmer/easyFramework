using System;

namespace easyFramework.Sys.Data.Table
{
	//================================================================================
//Class:     RuleException
	
	//--------------------------------------------------------------------------------'
//Module:    Rules.vb
	//--------------------------------------------------------------------------------'
//Purpose:   If a rule-exception has been detected, then the infos are stored
	//           here
	//--------------------------------------------------------------------------------'
//Created:   08.04.2004 10:33:41
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	public class TableRuleException
	{
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private string msFieldName;
		private string msMsg;
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		
		//================================================================================
//Property:  sFieldName
		//--------------------------------------------------------------------------------'
//Purpose:   the fieldname, which caused the error (db-name)
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   08.04.2004 10:35:10
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public string sFieldName
		{
			get{
				return msFieldName;
			}
			set
			{
				msFieldName = value;
			}
		}
		
		//================================================================================
//Property:  sMsg
		//--------------------------------------------------------------------------------'
//Purpose:   the error-description
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   08.04.2004 10:35:00
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public string sMsg
		{
			get{
				return msMsg;
			}
			set
			{
				msMsg = value;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		public TableRuleException(string sMsg, string sFieldName) {
			msFieldName = sFieldName;
		}
		public TableRuleException() {
			
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
