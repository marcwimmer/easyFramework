using System;

namespace easyFramework.Sys.ToolLib
{
	//================================================================================
	//Class:     Functions
	//--------------------------------------------------------------------------------'
	//Module:    Functions.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   Tool-Lib functions
	//--------------------------------------------------------------------------------'
	//Created:   19.04.2004 14:47:33
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class Functions
	{
		
		
		//================================================================================
		//Public Consts:
		//================================================================================
		public const string VbCrLf = Microsoft.VisualBasic.Constants.vbCrLf;
		public const string vbCr = Microsoft.VisualBasic.Constants.vbCr;
		public const string VbTab = Microsoft.VisualBasic.Constants.vbTab;
		
		public enum efEnumTriState
		{
			isTrue,
			isFalse,
			useDefault
		}

		public enum efEnumDateFormat
		{
			GeneralDate	,
			LongDate,
			LongTime,
			ShortDate,
			ShortTime
		}

		public enum efEnumDateInterval
		{
			Day	,
			Year,
			Month,
			Hour,
			Minute,
			Second
		}

		public enum efEnumFirstWeekOfYear 
		{
			System,
			FirstWeek,
			FirstFullWeek,
			Jan1
		}

		public enum efEnumFirstDayOfWeek 
		{
			Monday,
			Tuesday,
			Wednesday,
			Thursday,
			Friday,
			Saturday,
			Sunday
		}


		//================================================================================
		//Public Methods:
		//================================================================================
		public static bool IsEmptyString(string s) 
		{

			if (s == null | Trim(s) == "") 
			{
				return true;
			}
			else return false;
		}

		public static string Reverse(string sExpr)
		{
			
			string sResult = "";
			for (int i = Len(sExpr); i >= 1; i--)
			{
				
				sResult += Mid(sExpr, i, 1);
			}
			return sResult;
		}
		
		public static string LCase(string vnt)
		{
			return Microsoft.VisualBasic.Strings.LCase(vnt);
		}
		public static string LCase(char vnt)
		{
			return Microsoft.VisualBasic.Strings.LCase(vnt.ToString());
		}
		
		public static int Len(object vnt)
		{
			return Microsoft.VisualBasic.Strings.Len(vnt);
		}
		
		public static string UCase(string vnt)
		{
			return Microsoft.VisualBasic.Strings.UCase(vnt);
		}
		public static string UCase(char vnt)
		{
			return Microsoft.VisualBasic.Strings.UCase(vnt.ToString());
		}
		public static int UBound(Array a)
		{
			return Microsoft.VisualBasic.Information.UBound(a, 1);
		}
		public static int LBound(Array a)
		{
			return Microsoft.VisualBasic.Information.LBound(a, 1);
		}
		public static string Replace(string ReplaceIn, string Find, string ReplaceWith, int Start, int Count)
		{
			return Microsoft.VisualBasic.Strings.Replace(ReplaceIn, Find, ReplaceWith, Start, Count, 0);
		}
		public static string Replace(string ReplaceIn, string Find, string ReplaceWith, int Start)
		{
			return Microsoft.VisualBasic.Strings.Replace(ReplaceIn, Find, ReplaceWith, Start, -1, 0);
		}
		public static string Replace(string ReplaceIn, string Find, string ReplaceWith)
		{
			return Microsoft.VisualBasic.Strings.Replace(ReplaceIn, Find, ReplaceWith, 1, -1, 0);
		}
		public static char chr(int CharCode)
		{
			return Microsoft.VisualBasic.Strings.Chr(CharCode);
		}
		public static int Hour(DateTime dt)
		{
			return Microsoft.VisualBasic.DateAndTime.Hour(dt);
		}
		public static int Minute(DateTime dt)
		{
			return Microsoft.VisualBasic.DateAndTime.Minute(dt);
		}
		public static int Second(DateTime dt)
		{
			return Microsoft.VisualBasic.DateAndTime.Second(dt);
		}
		public static int Year(DateTime dt)
		{
			return Microsoft.VisualBasic.DateAndTime.Year(dt);
		}
		public static int Month(DateTime dt)
		{
			return Microsoft.VisualBasic.DateAndTime.Month(dt);
		}
		public static int Day(DateTime dt)
		{
			return Microsoft.VisualBasic.DateAndTime.Day(dt);
		}
		public static string Trim(string sValue)
		{
			return Microsoft.VisualBasic.Strings.Trim(sValue);
		}
		public static DateTime DateAdd(efEnumDateInterval Interval, double Number, DateTime dateValue)
		{
									
			Microsoft.VisualBasic.DateInterval i;

			switch (Interval) 
			{
				case efEnumDateInterval.Day:
					i = Microsoft.VisualBasic.DateInterval.Day;
					break;
				case efEnumDateInterval.Month:
					i = Microsoft.VisualBasic.DateInterval.Month;
					break;
				case efEnumDateInterval.Year:
					i = Microsoft.VisualBasic.DateInterval.Year;
					break;
				case efEnumDateInterval.Hour:
					i = Microsoft.VisualBasic.DateInterval.Hour;
					break;
				case efEnumDateInterval.Minute:
					i = Microsoft.VisualBasic.DateInterval.Minute;
					break;
				case efEnumDateInterval.Second:
					i = Microsoft.VisualBasic.DateInterval.Second;
					break;
				default:
					throw new efException("Ungültiges Datumsinterval \"" +	Interval.ToString() + "\".");

			}   
			
						

			return Microsoft.VisualBasic.DateAndTime.DateAdd(i, Number, dateValue);
			
		}
		public static DateTime DateAdd(string Interval, double Number, DateTime dateValue)
		{
			
			return Microsoft.VisualBasic.DateAndTime.DateAdd(Interval, Number, dateValue);
			
		}
		public static long DateDiff(String Interval, DateTime Date1, DateTime Date2, 
			efEnumFirstDayOfWeek FirstDayOfWeek, efEnumFirstWeekOfYear WeekOfYear)
		{
			
			
			Microsoft.VisualBasic.FirstDayOfWeek fdow = Convert_FirstDayOfWeek(FirstDayOfWeek);
			Microsoft.VisualBasic.FirstWeekOfYear fwoy =Convert_FirstWeekOfYear(WeekOfYear);


			return Microsoft.VisualBasic.DateAndTime.DateDiff(Interval, Date1, Date2, 
				fdow, fwoy);
		}
		public static bool IsNumeric(object Expression)
		{
			return Microsoft.VisualBasic.Information.IsNumeric(Expression);
		}
		public static DateTime Now()
		{
			return Microsoft.VisualBasic.DateAndTime.Now;
		}
		
		public static bool IsDate(object Expression)
		{
			return Microsoft.VisualBasic.Information.IsDate(Expression);
		}
		
		public static void Randomize ()
		{
			Microsoft.VisualBasic.VBMath.Randomize();
		}
		public static int InStr(string String1, string String2)
		{
			return Microsoft.VisualBasic.Strings.InStr(String1, String2, 0);
		}
		public static int InStr(int StartPos, string String1, string String2)
		{
			return Microsoft.VisualBasic.Strings.InStr(StartPos, String1, String2, 0);
		}
		
		public static object IIf(bool Expression, object TruePart, object FalsePart)
		{
			return Microsoft.VisualBasic.Interaction.IIf(Expression, TruePart, FalsePart);
		}
		
		public static string[] Split(string Expression, string Delimiter)
		{
			return Microsoft.VisualBasic.Strings.Split(Expression, Delimiter, -1, 0);
		}
		public static string[] Split(string Expression, string Delimiter, int Limit)
		{
			return Microsoft.VisualBasic.Strings.Split(Expression, Delimiter, Limit, 0);
		}
		
		public static string Left(string sString, int lCount)
		{
			return Microsoft.VisualBasic.Strings.Left(sString, lCount);
		}
		public static string Right(string sString, int lCount)
		{
			return Microsoft.VisualBasic.Strings.Right(sString, lCount);
		}
		public static string Mid(string sString, int lPos)
		{
			return Mid(sString, lPos, -1);
			}
		public static string Mid(string sString, int lPos, int lCount)
		{
			
			if (lCount == -1)
			{
				return Microsoft.VisualBasic.Strings.Mid(sString, lPos);
			}
			else
			{
				return Microsoft.VisualBasic.Strings.Mid(sString, lPos, lCount);
			}
			
			
		}
		
		public static string FormatNumber(object Expression, int NumDigitsAfterDecimal, efEnumTriState IncludeLeadingDigit, efEnumTriState UseParensForNegativeNumbers, efEnumTriState GroupDigits)
		{
			
			return Microsoft.VisualBasic.Strings.FormatNumber(Expression, NumDigitsAfterDecimal, TristateToMicrosoftTristate(IncludeLeadingDigit), TristateToMicrosoftTristate(UseParensForNegativeNumbers), TristateToMicrosoftTristate(GroupDigits));
			
		}
		
		public static string FormatCurrency(object Expression, int NumDigitsAfterDecimal, efEnumTriState IncludeLeadingDigit, efEnumTriState UseParensForNegativeNumbers, efEnumTriState GroupDigits)
		{
			
			return Microsoft.VisualBasic.Strings.FormatCurrency(Expression, NumDigitsAfterDecimal, TristateToMicrosoftTristate(IncludeLeadingDigit), TristateToMicrosoftTristate(UseParensForNegativeNumbers), TristateToMicrosoftTristate(GroupDigits));
		}
		
		public static string Format(object Expression, string Style)
		{
			return Microsoft.VisualBasic.Strings.Format(Expression, Style);
		}
		
		public static string FormatDateTime(DateTime Expression, efEnumDateFormat NamedFormat)
		{
			switch (NamedFormat) 
			{
				
				case efEnumDateFormat.GeneralDate:
					return Microsoft.VisualBasic.Strings.FormatDateTime(Expression, Microsoft.VisualBasic.DateFormat.GeneralDate);

				case efEnumDateFormat.LongDate:
					return Microsoft.VisualBasic.Strings.FormatDateTime(Expression, Microsoft.VisualBasic.DateFormat.LongDate);

				case efEnumDateFormat.LongTime:
					return Microsoft.VisualBasic.Strings.FormatDateTime(Expression, Microsoft.VisualBasic.DateFormat.LongTime);

				case efEnumDateFormat.ShortDate:
					return Microsoft.VisualBasic.Strings.FormatDateTime(Expression, Microsoft.VisualBasic.DateFormat.ShortDate);

				case efEnumDateFormat.ShortTime:
					return Microsoft.VisualBasic.Strings.FormatDateTime(Expression, Microsoft.VisualBasic.DateFormat.ShortTime);

			}

			throw new efException("Unknown Dateformat \"" + NamedFormat.ToString() + "\".");
			
		}
		
		public static string FormatPercent(object Expression, int NumDigitsAfterDecimal, efEnumTriState IncludeLeadingDigit, efEnumTriState UseParensForNegativeNumbers, efEnumTriState GroupDigits)
		{
			


		
			return Microsoft.VisualBasic.Strings.FormatPercent(Expression, NumDigitsAfterDecimal, TristateToMicrosoftTristate(IncludeLeadingDigit), TristateToMicrosoftTristate(UseParensForNegativeNumbers), TristateToMicrosoftTristate(GroupDigits));
		}
		
		private static Microsoft.VisualBasic.TriState TristateToMicrosoftTristate(efEnumTriState t)
		{
			if (t == efEnumTriState.isFalse)
			{
				return Microsoft.VisualBasic.TriState.False;
			}
			else if (t == efEnumTriState.isTrue)
			{
				return Microsoft.VisualBasic.TriState.True;
			}
			else
			{
				return Microsoft.VisualBasic.TriState.UseDefault;
			}
		}
		
		//================================================================================
		//Function:  String2Array
		//--------------------------------------------------------------------------------'
		//Purpose:   makes an array out of a string
		//--------------------------------------------------------------------------------'
		//Params:
		//           bDoNOTTrim - values are not trimmed
		//--------------------------------------------------------------------------------'
		//Returns:   array of strings
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:01:02
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string[] String2Array(string sString, string sSeperator)
		{
			return String2Array(sString, sSeperator, true, false);
		}
		public static string[] String2Array(string sString, string sSeperator, bool bAddEmptyLastElementIfExists, bool bDoNOTTrim)
		{

			
			if (sString == "")
			{
				return null;
			}
			else if (InStr(sString, sSeperator) == 0)
			{
				string[] asResult = ((string[])(Array.CreateInstance(typeof(string), 1)));
				asResult[0] = sString;
				return asResult;
			}
			else
			{
				
				efArrayList asResult = new efArrayList();
				string[] asSplitted = Split(sString, sSeperator, -1);
				
				for (int i = 0; i <= asSplitted.Length - 1; i++)
				{
					if (bDoNOTTrim)
					{
						asResult.Add(asSplitted[i]);
					}
					else
					{
						asResult.Add(Trim(asSplitted[i]));
					}
					
				}
				
				//remove the last element if it is empty if if the parameters says so:
				if (bAddEmptyLastElementIfExists == false)
				{
					asResult.RemoveAt(asResult.Count - 1);
				}
				string[] retValue = new string[asResult.Count-1 + 1];
				asResult.CopyTo(retValue);
				return retValue;
			}
		}
		//================================================================================
		//Function:  gs2Digit
		//--------------------------------------------------------------------------------'
		//Purpose:   makes "02" out of "2"; often used
		//--------------------------------------------------------------------------------'
		//Params:    the single-number
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 19:14:21
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gs2Digit(string sValue)
		{
			
			if (Len(sValue) == 0)
			{
				return "00";
			}
			else if (Len(sValue) == 1)
			{
				return "0" + sValue;
			}
			else
			{
				return sValue;
			}
			
		}
		
		
		
		
		//================================================================================
		//Function:  Array2String
		//--------------------------------------------------------------------------------'
		//Purpose:   makes a string out of an array
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   string
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 17:01:02
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string Array2String(string[] asArray, string sSeperator)
		{
			
			if (asArray == null)
			{
				return "";
			}
			else
			{
				string sResult = "";
				for (int i = 0; i <= asArray.Length - 1; i++)
				{
					sResult += sResult + asArray[i] + sSeperator;
				}

				return sResult;
			}
			
		}
		
		
		//================================================================================
		//Function:  gsGetRandomString
		//--------------------------------------------------------------------------------'
		//Purpose:   returns a randomstring
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 11:54:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsGetRandomString(int lLength)
		{
			
			Randomize();
			
			string sResult = "";
			
			Random r = new Random();
			
			for (int i = 0; i <= lLength - 1; i++)
			{
				
				int iRandom = r.Next(87, 97 + 25); //97=a
				
				if (iRandom >= 97)
				{
					sResult += chr(iRandom);
				}
				else
				{
					sResult += System.Convert.ToString(iRandom - 87);
					
				}
				
			}
			
			
			return sResult;
			
			
			
		}
		
		
		//================================================================================
		//Function:  gsGetTemporaryFile
		//--------------------------------------------------------------------------------'
		//Purpose:   returns something like "c:\temp\hak28gm5.xls"
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 11:53:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsGetTemporaryFile(string sTempFolder, string sExtension, int lLength)
		{
			
			string sRandomString = gsGetRandomString(lLength);
			
			
			if (Right(sTempFolder, 1) != "\\")
			{
				sTempFolder += "\\";
			}
			
			if (Left(sExtension, 1) != ".")
			{
				sExtension = "." + sExtension;
			}
			
			return sTempFolder + sRandomString + sExtension;
			
			
		}
		
		
		
		//================================================================================
		//Function:  gsValidFilename
		//--------------------------------------------------------------------------------'
		//Purpose:   removes invalid characters from a given filename
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 18:19:04
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsValidFilename(string sFilename)
		{
			
			sFilename = Replace(sFilename, "/", "", 1, -1);
			sFilename = Replace(sFilename, ":", "", 1, -1);
			sFilename = Replace(sFilename, "*", "", 1, -1);
			sFilename = Replace(sFilename, "?", "", 1, -1);
			sFilename = Replace(sFilename, "\"", "", 1, -1);
			sFilename = Replace(sFilename, "<", "", 1, -1);
			sFilename = Replace(sFilename, ">", "", 1, -1);
			sFilename = Replace(sFilename, "|", "", 1, -1);
			
			return sFilename;
		}


		//================================================================================
		//Function:  Convert_FirstWeekOfYear
		//--------------------------------------------------------------------------------'
		//Purpose:   CLS / VisualBasic Conversion
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.09.2004 18:19:04
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static Microsoft.VisualBasic.FirstWeekOfYear Convert_FirstWeekOfYear(efEnumFirstWeekOfYear value)
		{
			switch (value)
			{
				case efEnumFirstWeekOfYear.FirstFullWeek:
					return Microsoft.VisualBasic.FirstWeekOfYear.FirstFullWeek;
					
				case efEnumFirstWeekOfYear.Jan1:
					return Microsoft.VisualBasic.FirstWeekOfYear.Jan1;
					
				case efEnumFirstWeekOfYear.System:
					return Microsoft.VisualBasic.FirstWeekOfYear.System;
					
				default:
					throw new efException("Unknown WeekOfYear \"" + value.ToString()  + "\".");
			}


		}

		//================================================================================
		//Function:  Convert_FirstDayOfWeek
		//--------------------------------------------------------------------------------'
		//Purpose:   CLS / VisualBasic Conversion
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.09.2004 18:19:04
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static Microsoft.VisualBasic.FirstDayOfWeek Convert_FirstDayOfWeek
			(efEnumFirstDayOfWeek value) 
		{

			switch (value)
			{
				case efEnumFirstDayOfWeek.Friday:
					return  Microsoft.VisualBasic.FirstDayOfWeek.Friday;
					 
				case efEnumFirstDayOfWeek.Monday:
					return Microsoft.VisualBasic.FirstDayOfWeek.Monday;
					
				case efEnumFirstDayOfWeek.Saturday:
					return Microsoft.VisualBasic.FirstDayOfWeek.Saturday;
					
				case efEnumFirstDayOfWeek.Sunday:
					return Microsoft.VisualBasic.FirstDayOfWeek.Sunday;
					
				case efEnumFirstDayOfWeek.Thursday:
					return Microsoft.VisualBasic.FirstDayOfWeek.Thursday;
					
				case efEnumFirstDayOfWeek.Tuesday:
					return Microsoft.VisualBasic.FirstDayOfWeek.Tuesday;
					
				case efEnumFirstDayOfWeek.Wednesday:
					return Microsoft.VisualBasic.FirstDayOfWeek.Wednesday;
					
				default:
					throw new efException("Unknown DayOfWeek \"" + value.ToString() + "\".");
			}


		}
	}
	
	
	
}
