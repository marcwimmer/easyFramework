using System;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.ToolLib
{
	/// <summary>
	/// Zusammenfassung für HigherFunctions.
	/// </summary>
	public class efHigherFunctions
	{
		public efHigherFunctions()
		{
			
		}


		//================================================================================
		//Sub:       sHtml2Text
		//--------------------------------------------------------------------------------'
		//Purpose:   removes html-tags from a text and leaves only the text inside
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:06:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsHtml2Text(string sHtml) 
		{

			if (sHtml.IndexOf("<") < 0 & sHtml.IndexOf(">") < 0) 
				return sHtml;

			while  (sHtml.IndexOf("<") > 0 & sHtml.IndexOf(">") > 0) 
			{
				int lPos_Start = sHtml.IndexOf("<");
				int lPos_End = sHtml.IndexOf(">");
				sHtml = sHtml.Substring(0, lPos_Start) + sHtml.Substring(lPos_End + 1);
				
			};

			return sHtml;

		}

	}
}
