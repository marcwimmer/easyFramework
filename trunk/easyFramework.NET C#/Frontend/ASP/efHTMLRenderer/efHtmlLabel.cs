using System;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.HTMLRenderer
{
	//================================================================================
	//Class:     efLabel

	//--------------------------------------------------------------------------------'
	//Module:    efLabel.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   html TD
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 18:45:07
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class efHtmlLabel : efHTMLElement
	{
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private string msText;
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		
		//================================================================================
		//Property:  sFor
		//--------------------------------------------------------------------------------'
		//Purpose:   the id of the element, where the label references to
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 10:28:33
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sFor
		{
			get{
				return this["for"].sValue;
			}
			set
			{
				this["for"].sValue = value;
			}
		}
		
		
		//================================================================================
//Property:  sText
		//--------------------------------------------------------------------------------'
//Purpose:   the text, which is shown in the label
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   31.03.2004 10:42:07
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public string sText
		{
			get{
				return msText;
			}
			set
			{
				msText = value;
			}
		}
		
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
		public efHtmlLabel(efHTMLElement oParent) : base(oParent) {
			
		}
		public efHtmlLabel() : base(null) {
			
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
			return "LABEL";
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
		
		
		
		
		//================================================================================
//Sub:       gRenderBeginTag
		//--------------------------------------------------------------------------------'
//Purpose:
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   31.03.2004 10:48:18
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public override void gRenderBeginTag (FastString sHtmlBuilder, int lLevel)
		{
			sHtmlBuilder.Append("<LABEL ");
			
			
			
			//set the access-key:
			string sText = this.sText;
			string sAccesskey = "";
			if (Functions.InStr(sText, "&") > 0)
			{
				sAccesskey = Functions.Mid(sText, Functions.InStr(sText, "&") + 1, 1);
				
				//make an <u> around the label:
				sText = Functions.Left(sText, Functions.InStr(sText, "&") - 1) + "<u>" + sAccesskey + "</u>" + Functions.Mid(sText, Functions.InStr(sText, "&") + 2);
			}
			
			//render attribute accesskey:
			this["accesskey"].sValue = sAccesskey;
			
			
			//render other attributes:
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
			
			
//text-content:
			sHtmlBuilder.Append(">" + sText + "</LABEL>");
			
			
		}
		
		
		//================================================================================
//Sub:       gRenderEndTag
		//--------------------------------------------------------------------------------'
//Purpose:   do nothing
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   31.03.2004 10:48:24
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public override void gRenderEndTag (FastString sHtmlBuilder, int lLevel)
		{
			//do nothing
		}
		
		
		
		//================================================================================
//Sub:       gRenderBeginTag
		//--------------------------------------------------------------------------------'
//Purpose:   overloading method
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   13.04.2004 02:53:01
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public override void gRenderBeginTag (System.Web.UI.HtmlTextWriter output, int lLevel)
		{
			base.gRenderBeginTag(output, lLevel);
			
		}
	}
	
}
