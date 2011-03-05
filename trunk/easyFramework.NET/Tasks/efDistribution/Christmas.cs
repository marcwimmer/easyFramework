using System;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP;
using easyFramework.Sys.Entities;
using System.IO;
using easyFramework.AddOns.efICSharpZLibWrapper;
using easyFramework.Frontend.ASP.ASPTools;


namespace easyFramework.Tasks.Distribution
{
	//================================================================================
	//Class:	<Christmas>
	//--------------------------------------------------------------------------------
	//Module:	Christmas.cs
	//--------------------------------------------------------------------------------
	//Copyright:Promain Software-Betreuung GmbH, 2004	
	//--------------------------------------------------------------------------------
	//Purpose:	gets the package, created by packager, and 
	//			puts the files to the place, where they belong to
	//--------------------------------------------------------------------------------
	//Created:	09.09.2004 00:57:40 Marc Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	//Changed:	
	//--------------------------------------------------------------------------------
	public class Christmas
	{
		public Christmas()
		{
			
		}

		/// <summary>
		/// extracts the files from a package (made by the packager class)
		/// </summary>
		public static void gOpenPackage(string sZipFileName, 
			byte[] abZipFileContent, 
			string sReceivedPackageFile, 
			string sRootDir) 
		{
			//store the data in the received package folder
			if (sReceivedPackageFile.Substring(sReceivedPackageFile.Length - 1, 1) != "\\")
				sReceivedPackageFile += "\\" + sZipFileName;
			string sPath = Path.GetDirectoryName(sReceivedPackageFile);
			if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

			
			FileStream fs = new FileStream(sReceivedPackageFile,  FileMode.OpenOrCreate, FileAccess.Write,
				FileShare.None);
			
			fs.Write(abZipFileContent, 0, abZipFileContent.Length);
			fs.Close();


			efZLib.gExtractFromZipFile(sReceivedPackageFile, sRootDir, true, true);

		}
	}
}
