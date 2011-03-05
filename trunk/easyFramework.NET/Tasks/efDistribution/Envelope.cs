using System;
using easyFramework.Sys.ToolLib;
using System.Security.Cryptography;

namespace easyFramework.Tasks.Distribution
{
	/// <summary>
	/// The envelope class is used to transfer binary data as string-data
	/// to other websites, where they can be deevenloped. 
	/// 
	/// Some checks with MD5-Parity could be done here.
	/// </summary>
	public class Envelope
	{
		
		/// <summary>
		/// used to fill up the header
		/// </summary>
		public static string efHEADER_FILL_CHARACTER = " ";

		/// <summary>
		/// converts a byte to a string
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static string gsByteToString(byte b) 
		{
			return String.Format("{0:000}", Convert.ToInt32(b));
		}

		/// <summary>
		/// converts the string to a byte
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static byte gbStringToByte(string s) 
		{
			if (s.Length != 3) 
				throw new efException("String must have length of 3, to be converted to " +
					"byte. Invalid: \"" + s + "\".");

			return Convert.ToByte(s);
		}

		/// <summary>
		/// constructor
		/// </summary>
		public Envelope()
		{
		}

		/// <summary>
		/// makes a "letter". string data is returned which can be
		/// returned by the function gsDeEnvelope()
		/// </summary>
		/// <param name="abData">the data to be enveloped</param>
		/// <returns></returns>
		public static string gsEnvelope(ref byte[] abData) 
		{
			FastString oResult = new FastString();
			FastString oHeader = new FastString();
			
			//create a header (use the first 100 bytes for the md5)
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] abMd5 = md5.ComputeHash(abData);
			for (int i = 0; i < abMd5.Length; i++) 
			{
				string sValue = gsByteToString(abMd5[i]);
				oHeader.Append(sValue);
			}

			while (oHeader.lLength < 100)
				oHeader.Append(efHEADER_FILL_CHARACTER);
			oResult.Append(oHeader.ToString());

			
			//convert the byte-array to a string
			for (int i = 0; i < abData.Length; i++) 
			{
				string sValue = gsByteToString(abData[i]);
				oResult.Append(sValue);

			}



			return oResult.ToString();

		}

		/// <summary>
		/// makes a binary-data out of the string-file; with
		/// md5-comparision built in.
		/// compatible to gsEnvelope
		/// </summary>
		/// <param name="sData"></param>
		/// <returns></returns>
		public static byte[] gabDeEnvelope(ref string sData) 
		{
			byte[] abResult;
			string sHeader;
			string sMD5;
			efArrayList oData = new efArrayList();

			//get the header
			sHeader = sData.Substring(0, 100);

			//remove the fill-char
			while (sHeader.Substring(sHeader.Length - 1, 1) == efHEADER_FILL_CHARACTER) 
			{
				sHeader = sHeader.Substring(0, sHeader.Length - 1);
			}
			sMD5 = sHeader;

			//extract the data
			//1. put the 3-bytes pieces into an arraylist.
			//2. transform the array-list into a byte-array
			//3. get the md5
			
			sData = sData.Substring(100, sData.Length - 100);

			for (int i = 0; i < sData.Length; i+=3) 
			{
				string sPart = sData.Substring(i, 3);
				byte bValue = gbStringToByte(sPart);

				oData.Add(bValue);
			}

			abResult = (byte[])(oData.ToArray(typeof (System.Byte)));

			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] abMd5 = md5.ComputeHash(abResult, 0, abResult.Length);

			FastString oMD5FromDataString = new FastString();
			for (int i = 0; i < abMd5.Length; i++) 
			{
				oMD5FromDataString.Append(gsByteToString(abMd5[i]));
			}

			//compare md5
			if (oMD5FromDataString.ToString().CompareTo(sMD5) != 0) 
				throw new EnvelopeMD5MatchException();


			return abResult;
		}
	}
}
