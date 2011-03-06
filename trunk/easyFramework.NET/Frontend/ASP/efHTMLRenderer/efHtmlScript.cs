using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.HTMLRenderer
{
	//================================================================================
	//Class:     efTable

	//--------------------------------------------------------------------------------'
	//Module:    efTable.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   html TABLE
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 18:45:07
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class efHtmlScript : efHTMLElement
	{
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private string msCode;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		public enum efEnumScriptLanguages
		{
			efNone,
			efJavascript,
			efVBScript
		}
		
		
		//================================================================================
		//Public Properties:
		//================================================================================
		public string sCode
		{
			get{
				return msCode;
			}
			set
			{
				msCode = value;
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
		public efHtmlScript(efHTMLElement oParent) : base(oParent) {
		}
		public efHtmlScript() : base(null) {
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
			return "SCRIPT";
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
			string[] asResult = new string[] { "HTML", "TR", "TD", "TABLE", "DIV" };
			return asResult;
		}
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       gRenderBeginTag
		//--------------------------------------------------------------------------------'
		//Purpose:   make script-tag
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 21:31:46
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override void gRenderBeginTag (FastString sHtmlBuilder, int lLevel)
		{
			//no children will be rendered, so it is enough to override the begintag:
			
			
			sHtmlBuilder.Append(msGetIndent(lLevel) + "<script ");
			
			//src-attribute:
			if (this["src"].bIsEmpty == false)
			{
				sHtmlBuilder.Append("src=\"" + this["src"].sValue + "\" ");
			}
			
			//language-attribute:
			if (this["language"].bIsEmpty == false)
			{
				sHtmlBuilder.Append("language=\"" + this["language"].sValue + "\" ");
			}
			
			
			//if there is javascript-code then write it:
			sHtmlBuilder.Append(">" + "\n");
			sHtmlBuilder.Append(sCode + "\n");
			sHtmlBuilder.Append(msGetIndent(lLevel) + "</script>" + "\n");
			
			
		}
		
		//================================================================================
		//Sub:       gRenderEndTag
		//--------------------------------------------------------------------------------'
		//Purpose:   render end tag
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 21:32:02
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override void gRenderEndTag (FastString sHtmlBuilder, int lLevel)
		{
			//nothing
		}
		
		
		
		
		//================================================================================
		//Function:  sScriptLanguage2String
		//--------------------------------------------------------------------------------'
		//Purpose:   conversion
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Returns:   -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 21:33:44
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sScriptLanguage2String(efEnumScriptLanguages enLanguage)
		{
			
			switch (enLanguage)
			{
				
			case efEnumScriptLanguages.efJavascript:
				
				return "javascript";
				
			case efEnumScriptLanguages.efVBScript:
				
				return "VBScript";
				
			case efEnumScriptLanguages.efNone:
				
				return "";

		}

		throw new efException("Unknown language at sScriptLanguage2String: " +
			DataConversion.gsCStr( enLanguage ));
		
	}
	
	//================================================================================
	//Private Methods:
	//================================================================================
	
}

}