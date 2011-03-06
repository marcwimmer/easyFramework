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
	/// <summary>

	//================================================================================
	//Class:	<Distributor>
	//--------------------------------------------------------------------------------
	//Module:	Distributor.cs
	//--------------------------------------------------------------------------------
	//Copyright:Promain Software-Betreuung GmbH, 2004	
	//--------------------------------------------------------------------------------
	//Purpose:	collects all files of the easyframework and uploads it to the
	//			specified abonnent
	//--------------------------------------------------------------------------------
	//Created:	09.09.2004 00:57:40 Marc Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	//Changed:	
	//--------------------------------------------------------------------------------

	/// </summary>
	public class Packager
	{
		public Packager()
		{

		}

		/// <summary>
		/// starts the distribution; creates a package for distribution (xmas package)
		/// </summary>
		/// <param name="oClientInfo"></param>
		/// <param name="oXmlFileList">the file-list xml (see efZLIBWrapper for details)</param>
		public static void gMakePackage(ClientInfo oClientInfo, XmlDocument oXmlFileList) 
		{
			
			//---------make absolute paths--------------
			mResolveVirtualPaths(oClientInfo, ref oXmlFileList);

			//---------create the package directory---------------
			string sPackDir = Convert.ToString(oClientInfo.oHttpApp.oGet("distribution_made_package_folder"));
			if (!Directory.Exists(sPackDir)) 
				Directory.CreateDirectory(sPackDir);


			//--------create zip-file-----------
			string sZipFileName = "";
			sZipFileName += "efPackage_";
			sZipFileName +=	String.Format("{0:0000}", Convert.ToInt32(Functions.Year(Functions.Now())));
			sZipFileName +=	"_";
			sZipFileName +=	String.Format("{0:00}", Convert.ToInt32(Functions.Month(Functions.Now())));
			sZipFileName +=	"_";
			sZipFileName +=	String.Format("{0:00}", Convert.ToInt32(Functions.Day(Functions.Now())));
			sZipFileName +=	"_";
			sZipFileName +=	String.Format("{0:00}", Convert.ToInt32(Functions.Hour(Functions.Now())));
			sZipFileName +=	"_";
			sZipFileName +=	String.Format("{0:00}", Convert.ToInt32(Functions.Minute(Functions.Now())));
			sZipFileName +=	"_";
			sZipFileName +=	String.Format("{0:00}", Convert.ToInt32(Functions.Second(Functions.Now())));
			sZipFileName += ".zip";

			efZLib.gCreateZipFile(sPackDir + "\\" + sZipFileName, oXmlFileList, 9, 
				oClientInfo.oHttpServer.sMapPath(oClientInfo.oHttpApp.sApplicationPath()), true);

			//---------write package info into database------------------
			DefaultEntity oEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "AbonnentPackages");
			oEntity.gNew(oClientInfo);
			oEntity.oFields["dst_packagename"].sValue = sZipFileName;
			oEntity.gSave(oClientInfo);

		}

	
		/// <summary>
		/// iterates the directories and files given in the xml and makes absolute-filepaths out
		/// of them
		/// </summary>
		/// <param name="oXmlFileList"></param>
		private static void mResolveVirtualPaths(ClientInfo oClientInfo, ref XmlDocument oXmlFileList) 
		{
			XmlNodeList nlFiles = oXmlFileList.selectNodes("//file");
			XmlNodeList nlDirectories = oXmlFileList.selectNodes("//directory");

			for (int i = 0; i < nlFiles.lCount; i++) 
			{
				string sFilename = nlFiles[i].sText;

				sFilename = oClientInfo.oHttpServer.sMapPath(oClientInfo.oHttpApp.sApplicationPath() + sFilename);

				nlFiles[i].sText = sFilename;
			}

			for (int i = 0; i < nlDirectories.lCount; i++) 
			{
				string sDirectoryName = nlDirectories[i].sText;

				sDirectoryName = oClientInfo.oHttpServer.sMapPath(oClientInfo.oHttpApp.sApplicationPath() + sDirectoryName);

				nlDirectories[i].sText = sDirectoryName;
				
			}
		}


	}
}
