using System;

namespace easyFramework.Frontend.ASP.HTMLRenderer
{
	//================================================================================
//Class:     efHtmlDiv
	
	//--------------------------------------------------------------------------------'
//Module:    efHtmlDiv.vb
	//--------------------------------------------------------------------------------'
//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
//Purpose:   html DIV
	//--------------------------------------------------------------------------------'
//Created:   23.03.2004 18:45:07
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	public class efHtmlDiv : efHTMLElement
	{
		
		
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		//================================================================================
//Sub:       New
		//--------------------------------------------------------------------------------'
//Purpose:   constructor
		//--------------------------------------------------------------------------------'
//Params:    the parent-object
		//--------------------------------------------------------------------------------'
//Created:   23.03.2004 19:15:01
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public efHtmlDiv(efHTMLElement oParent) : base(oParent) {
		}
		public efHtmlDiv() : base(null) {
		}
		
		//================================================================================
//Function:  sGetNodeName
		//--------------------------------------------------------------------------------'
//Purpose:   gets the nodename
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:   string
		//--------------------------------------------------------------------------------'
//Created:   23.03.2004 19:14:45
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetNodeName()
		{
			return "DIV";
		}
		
		
		//================================================================================
//Function:  asAllowedParentTags
		//--------------------------------------------------------------------------------'
//Purpose:   returns the allowed parent-tags
		//--------------------------------------------------------------------------------'
//Params:    '
		//--------------------------------------------------------------------------------'
//Returns:   string-list
		//--------------------------------------------------------------------------------'
//Created:   23.03.2004 18:48:37
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public override string[] asAllowedParentTags()
		{
			string[] asResult = new string[] { "TD", "SPAN", "P", "BODY", "FORM", "TABLE", "DIV" };
			return asResult;
		}
	}
	
}
