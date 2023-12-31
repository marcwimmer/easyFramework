using System;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.HTMLRenderer
{
	//================================================================================
	//Class:     efHtmlHr

	//--------------------------------------------------------------------------------'
	//Module:    efHtmlHr.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   html Hr
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 18:45:07
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	
	public class efHtmlHr : efHTMLElement
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
		public efHtmlHr(efHTMLElement oParent) : base(oParent) {
		}
		public efHtmlHr() : base(null) {
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
			return "HR";
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
			string[] asResult = new string[] { "APPLET", "BLOCKQUOTE", "BODY", "BUTTON", "CENTER", "DD", "DEL", "DIV", "FIELDSET", "FORM", "IFRAME", "INS", "LI", "MAP", "NOFRAMES", "NOSCRIPT", "OBJECT", "TD", "TH" };
			return asResult;
		}
		
		public override void gRenderBeginTag (FastString sHtmlBuilder, int lLevel)
		{
			for (int i = 1; i <= lLevel - 1; i++)
			{
				sHtmlBuilder.Append("\t");
			}
			
			sHtmlBuilder.Append("<" + sGetNodeName());
			
			if (moAttributeList != null)
			{
				
				foreach (efAttribute oAttr in moAttributeList)
				{
					if (oAttr.bIsEmpty == false)
					{
						
						sHtmlBuilder.Append(" " + oAttr.sName + "=\"" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Replace(oAttr.sValue, "\"", efHTML_QUOT)) + "\"");
						
					}
					
				}
			}
			
			sHtmlBuilder.Append("/>" + "\n");
			
		}
		
		public override void gRenderEndTag (FastString sHtmlBuilder, int lLevel)
		{
			
		}
	}
	
}
