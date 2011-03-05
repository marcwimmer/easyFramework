using System;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     MIME

	//--------------------------------------------------------------------------------'
	//Module:    MIME.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   helper for MIME-handling
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 10:15:51
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public enum efEnumSupportedMIME
	{
		
		efBrowserStandard, //let the browser choose
		
		efExcel,
		efMSWord,
		efPDF,
		efTextHtml,
		efZip
		
	}
	
	
	public class MIME
	{
		
		
		//================================================================================
//Function:  gsGetResponseContentType
		//--------------------------------------------------------------------------------'
//Purpose:   returns the string for response("Content-Type")
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   11.05.2004 10:16:43
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsGetResponseContentType(efEnumSupportedMIME mime)
		{
			switch (mime)
			{
				case efEnumSupportedMIME.efBrowserStandard:
					
					return "application/octet-stream";
					
					
				case efEnumSupportedMIME.efExcel:
					
					return "application/vnd.ms-excel";
				case efEnumSupportedMIME.efMSWord:
					
					return "application/msword";
				case efEnumSupportedMIME.efPDF:
					
					return "application/pdf";
				case efEnumSupportedMIME.efTextHtml:
					
					return "text/html";
				case efEnumSupportedMIME.efZip:
					
					return "application/x-zip-compressed";
				default:
					
					throw (new efException("unhandled mime: " + mime));
					break;
			}
		}
		
		
		//================================================================================
//Function:  gsGetResponseContentDisposition
		//--------------------------------------------------------------------------------'
//Purpose:   returns the string for response("Content-Disposition")
		//           useful, if you want to give a user-readable name to the content,
		//           which is being downloaded, e.g. "downloadfile.xls"
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   11.05.2004 10:16:43
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsGetResponseContentType(string sFileName)
		{
			
			return "inline;filename=" + sFileName;
			
		}
		
		
	}
	
}
