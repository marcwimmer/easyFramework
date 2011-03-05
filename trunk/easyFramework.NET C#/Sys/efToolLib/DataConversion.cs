using System;
using System.Text;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.ToolLib
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    DataConversion.vb
	//--------------------------------------------------------------------------------
	// Purpose:      data-conversion utils
	//--------------------------------------------------------------------------------
	// Created:      31.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	
	
	public class DataConversion
	{
		
		//================================================================================
		//Public Consts:
		//================================================================================
		public const string efNullValue = "{DADB397C-329D-45a7-84AB-2F5192FAE3A9}";
		
		public static DateTime efNullDate
		{
			get 
			{
				return DateTime.Parse("1/1/1900");
			}
		}


		//================================================================================
		// Method:       gdtDateTime
		//--------------------------------------------------------------------------------
		// Purpose:      converts the param to date-time
		//--------------------------------------------------------------------------------
		// Parameteres:  variant-object
		//--------------------------------------------------------------------------------
		// Returns:      converted date
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static DateTime gdtCDate(object vntObject)
		{
			return gdtCDate(vntObject, DataConversion.efNullDate);
		}
		public static DateTime gdtCDate(object vntObject, DateTime onNotDate)
		{
			
			
			if (gbIsNull(vntObject))
			{
				return onNotDate;
			}
			else
			{
				
				if (vntObject is string)
				{
					//format is for string always: yyyymmdd
					//with time it is: yyyymmdd hhmmss
					
					string sValue = Convert.ToString(vntObject);
					
					if (Functions.Len(sValue) == 8)
					{
						
						DateTime dtResult;
						dtResult = new DateTime(glCInt(Functions.Left(sValue, 4), 0), glCInt(Functions.Mid(sValue, 5, 2), 0), glCInt(Functions.Mid(sValue, 7, 2), 0));
						
						return dtResult;
						
					}
					else if (Functions.Len(sValue) == 15)
					{
						DateTime dDate;
						dDate = new DateTime(glCInt(Functions.Left(sValue, 4), 0), glCInt(Functions.Mid(sValue, 5, 2), 0), glCInt(Functions.Mid(sValue, 7, 2), 0));
						
						DateTime dtResult;
						dtResult = new DateTime(dDate.Year, dDate.Month, dDate.Day, glCInt(Functions.Mid(sValue, 10, 2), 0), glCInt(Functions.Mid(sValue, 12, 2), 0), glCInt(Functions.Mid(sValue, 14, 2), 0));
						
						return dtResult;
						
					}
					else
					{
						throw (new efException("Invalid Date-String: " + sValue));
					}
					
				}
				else if (! Functions.IsDate(vntObject))
				{
					return onNotDate;
				}
				else
				{
					return Convert.ToDateTime(vntObject);
				}
				
			}
			
			
			
		}
		
		//================================================================================
		// Method:       gsCStr
		//--------------------------------------------------------------------------------
		// Purpose:      converts the param to string
		//--------------------------------------------------------------------------------
		// Parameteres:  variant-object
		//--------------------------------------------------------------------------------
		// Returns:      converted string
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static string gsCStr(object vntObject)
		{
			
			if (gbIsNull(vntObject))
			{
				return "";
			}
			else
			{
				if ((vntObject is DateTime))
				{
					DateTime dDateTime = ((DateTime)(vntObject));
					string sResult;
					
					sResult = gsCStr(dDateTime.Year);
					sResult += Functions.gs2Digit(gsCStr(dDateTime.Month));
					sResult += Functions.gs2Digit(gsCStr(dDateTime.Day));
					sResult += " ";
					sResult += Functions.gs2Digit(gsCStr(dDateTime.Hour));
					sResult += Functions.gs2Digit(gsCStr(dDateTime.Minute));
					sResult += Functions.gs2Digit(gsCStr(dDateTime.Second));
					return sResult;
					
				}
				else if (vntObject is DateTime)
				{
					DateTime dDate = ((DateTime)(vntObject));
					string sResult;
					
					sResult = gsCStr(dDate.Year);
					sResult += Functions.gs2Digit(gsCStr(dDate.Month));
					sResult += Functions.gs2Digit(gsCStr(dDate.Day));
					return sResult;
					
				}
				return Convert.ToString(vntObject);
			}
			
			
		}
		//================================================================================
		// Method:       gbCBool
		//--------------------------------------------------------------------------------
		// Purpose:      converts the param to boolean
		//--------------------------------------------------------------------------------
		// Parameteres:  variant-object
		//--------------------------------------------------------------------------------
		// Returns:      converted string
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static bool gbCBool(object vntObject)
		{
			
			if (gbIsNull(vntObject))
			{
				return false;
			}
			else
			{
				if ((vntObject is DateTime) |(vntObject is DateTime))
				{
					throw (new Exception("cannot convert date-value into bool"));
				}
				
				string sCompare = System.Convert.ToString(vntObject);
				switch (Functions.LCase(sCompare))
				{
					case "1":
						return true;
						
					case "-1":
						return true;
						
					case "true":
						return true;
						
					case "wahr":
						
						return true;
					case "0":
						return false;
						
					case "falsch":
						return false;
						
					case "false":
						
						return false;
					default:
						
						throw (new Exception("couldn't convert \"" + System.Convert.ToString(vntObject) + "\" into boolean."));
						
				}
				
			}
			
			
		}
		
		//================================================================================
		// Method:       glCInt
		//--------------------------------------------------------------------------------
		// Purpose:      converts the param to integer
		//--------------------------------------------------------------------------------
		// Parameteres:  variant-object
		//--------------------------------------------------------------------------------
		// Returns:      converted integer
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static int glCInt(object vntObject)
		{
			return glCInt(vntObject, 0);
		}
		public static int glCInt(object vntObject, int onNotNumeric)
		{
			
			if (gbIsNull(vntObject))
			{
				return onNotNumeric;
			}
			else
			{
				if (! Functions.IsNumeric(vntObject))
				{
					return onNotNumeric;
				}
				else
				{
					return Convert.ToInt32(vntObject);
				}
				
			}
			
			
		}
		
		//================================================================================
		// Method:       gdCDec
		//--------------------------------------------------------------------------------
		// Purpose:      converts the param to decimal
		//--------------------------------------------------------------------------------
		// Parameteres:  variant-object
		//--------------------------------------------------------------------------------
		// Returns:      converted decimal
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static decimal gdCDec(object vntObject, int onNotNumeric)
		{
			
			if (gbIsNull(vntObject))
			{
				return onNotNumeric;
			}
			else
			{
				if (! Functions.IsNumeric(vntObject))
				{
					return onNotNumeric;
				}
				else
				{
					return Convert.ToDecimal(vntObject);
				}
				
			}
			
			
		}
		
		//================================================================================
		// Method:       gdCDbl
		//--------------------------------------------------------------------------------
		// Purpose:      converts the param to decimal
		//--------------------------------------------------------------------------------
		// Parameteres:  variant-object
		//--------------------------------------------------------------------------------
		// Returns:      converted decimal
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static double gdCDbl(object vntObject, int onNotNumeric)
		{
			
			if (gbIsNull(vntObject))
			{
				return onNotNumeric;
			}
			else
			{
				if (! Functions.IsNumeric(vntObject))
				{
					return onNotNumeric;
				}
				else
				{
					return Convert.ToDouble(vntObject);
				}
				
			}
			
			
		}
		
		//================================================================================
		//Function:  gsSqlDate
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the formats
		//           { ts 'yyyy-mm-dd hh:mm:ss.fff] '} wie z. B.: { ts '1998-09-24 10:02:20' }
		//           { d 'yyyy-mm-dd'} wie z. B.: { d '1998-09-24' }
		//           { t 'hh:mm:ss'} wie z. B.: { t '10:02:20'}
		//--------------------------------------------------------------------------------'
		//Params:    the sql-date
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 18:56:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public enum efEnumSqlDateFormat
		{
			dfTimeStamp,
			dfDate,
			dfTime
		}
		public static string gsSqlDate(DateTime dDate, efEnumSqlDateFormat enSqlDateFormat)
		{
			
			switch (enSqlDateFormat)
			{
				
			case efEnumSqlDateFormat.dfDate:
				
				return "{ d '" + dDate.Year + "-" + Functions.gs2Digit(gsCStr(dDate.Month)) + "-" + Functions.gs2Digit(gsCStr(dDate.Day)) + "' } ";
				

			case efEnumSqlDateFormat.dfTime:
				
				return "{ t '" + Functions.gs2Digit(gsCStr(dDate.Hour)) + ":" + Functions.gs2Digit(gsCStr(dDate.Minute)) + ":" + Functions.gs2Digit(gsCStr(dDate.Second)) + "' } ";
				
				
			case efEnumSqlDateFormat.dfTimeStamp:
				
				return "{ ts '" + dDate.Year + "-" + 
					Functions.gs2Digit(gsCStr(dDate.Month)) + "-" + 
					Functions.gs2Digit(gsCStr(dDate.Day)) + " " + 
					Functions.gs2Digit(gsCStr(dDate.Hour)) + ":" + 
					Functions.gs2Digit(gsCStr(dDate.Minute)) + ":" + 
					Functions.gs2Digit(gsCStr(dDate.Second)) + "' } ";
			
			
		}
		
		throw new efException("Unknown format \"" + enSqlDateFormat + "\"");
	}
	
	
	//================================================================================
	//Function:  gsFormatDate
	//--------------------------------------------------------------------------------'
	//Purpose:   formats the date into the language-format of the current user
	//--------------------------------------------------------------------------------'
	//Params:
	//--------------------------------------------------------------------------------'
	//Returns:
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 09:02:37
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	public static string gsFormatDate(string sLanguage, DateTime dtDate)
	{
		//TODO: make language-specific:
		
		System.Globalization.DateTimeFormatInfo myDTFI = new System.Globalization.CultureInfo("de-DE", false).DateTimeFormat;
		
		
		
		return dtDate.ToString("d");
	}
	
	
	//================================================================================
	// Method:       gbIsNull
	//--------------------------------------------------------------------------------
	// Purpose:      decides, wether an object is null or not; can be used on any type
	//--------------------------------------------------------------------------------
	// Parameteres:  variant-object
	//--------------------------------------------------------------------------------
	// Returns:      a new created clientinfo-object
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	public static bool gbIsNull(object vntObject)
	{
		
		if (vntObject == null)
		{
			return true;
		}
		
		if (vntObject is DateTime)
		{
			if (((DateTime)(vntObject)).Equals( efNullDate))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		if (vntObject is DBNull)
		{
			return true;
			
		}
		
		if (vntObject is string)
		{
			if (System.Convert.ToString(vntObject)== efNullValue)
			{
				return true;
			}
		}
		
		
		return false;
		
	}
	
}


//================================================================================
//Class:     DataConversionException
//--------------------------------------------------------------------------------'
//Copyright: Promain Software-Betreung GmbH
//--------------------------------------------------------------------------------'
//Created:   31.03.2004 19:04:10
//--------------------------------------------------------------------------------'
//Changed:
//--------------------------------------------------------------------------------'
public class DataConversionException : System.Exception
{
	
	
	public DataConversionException(string sMessage) : base(sMessage) {
		
	}
	
	
}

}
