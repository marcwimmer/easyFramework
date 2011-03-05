using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.HTMLRenderer
{
	//================================================================================
	//Class:     efHtmlConsts

	//--------------------------------------------------------------------------------'
	//Module:    efHtmlConsts.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   public class for handling consts in html
	//--------------------------------------------------------------------------------'
	//Created:   12.04.2004 21:15:18
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public enum efEnumBorderStyle
	{
		efNone,
		efHidden,
		efDotted,
		efDashed,
		efSolid,
		efDouble,
		efGroove,
		efRidge,
		efInset,
		efOutset
	}
	
	public enum efEnumBorderWidth
	{
		efThin,
		efMedium,
		efThick
	}
	
	public enum efEnumOverflow
	{
		efHidden,
		efVisible,
		efScroll,
		efAuto
	}
	
	
	public class efHtmlConsts
	{
		
		
		
		//================================================================================
//Function:  BorderStyle
		//--------------------------------------------------------------------------------'
//Purpose:   convert border-style enum into string
		//--------------------------------------------------------------------------------'
//Params:    border-style enum
		//--------------------------------------------------------------------------------'
//Returns:   string
		//--------------------------------------------------------------------------------'
//Created:   12.04.2004 21:17:28
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static string BorderStyle(efEnumBorderStyle enBorderStyle)
		{
			
			switch (enBorderStyle)
			{
				case efEnumBorderStyle.efHidden:
					
					return "hidden";
				case efEnumBorderStyle.efNone:
					
					return "none";
				case efEnumBorderStyle.efDashed:
					
					return "dashed";
				case efEnumBorderStyle.efDotted:
					
					return "dotted";
				case efEnumBorderStyle.efDouble:
					
					return "double";
				case efEnumBorderStyle.efGroove:
					
					return "groove";
				case efEnumBorderStyle.efInset:
					
					return "inset";
				case efEnumBorderStyle.efOutset:
					
					return "outset";
				case efEnumBorderStyle.efRidge:
					
					return "ridge";
				case efEnumBorderStyle.efSolid:
					
					return "solid";
				default:
					
					throw (new efException("invalid borderstytle: " + enBorderStyle));
			
			}
		}
		
		
		//================================================================================
//Function:  BorderWidth
		//--------------------------------------------------------------------------------'
//Purpose:   convert border-width
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   12.04.2004 21:20:40
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static string BorderWidth(efEnumBorderWidth enBorderWidth)
		{
			switch (enBorderWidth)
			{
				case efEnumBorderWidth.efMedium:
					
					return "medium";
				case efEnumBorderWidth.efThick:
					
					return "thick";
				case efEnumBorderWidth.efThin:
					
					return "thin";
				default:
					
					throw (new efException("invalid borderstytle: " + enBorderWidth));
				
			}
		}
		
		//================================================================================
//Function:  BorderWidth
		//--------------------------------------------------------------------------------'
//Purpose:   convert border-width
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   12.04.2004 21:20:40
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static string Overflow(efEnumOverflow enOverflow)
		{
			switch (enOverflow)
			{
				case efEnumOverflow.efAuto:
					
					return "auto";
				case efEnumOverflow.efHidden:
					
					return "hidden";
				case efEnumOverflow.efScroll:
					
					return "scroll";
				case efEnumOverflow.efVisible:
					
					return "visible";
				default:
					
					throw (new efException("invalid borderstytle: " + enOverflow));
		
			}
		}
		
	}
	
}
