using System;
using System.Web;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.OnlineHelp;
using easyFramework.Frontend.ASP;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.SysEvents;
using System.IO;

namespace easyFramework.Frontend.ASP.WebApp
{
	/// <summary>
	/// initializes all needed data in the web.
	/// 
	/// Can be called through the /system/devaid/... asp-files, to reinitialize the app
	/// </summary>
	public class WebAppInit
	{
		public WebAppInit()
		{

		}

		public const string efsApp_Up_And_Running = "APP_UP_AND_RUNNING";

		/// <summary>
		/// is called by the init global asa function and is the main initialization function
		/// </summary>
		/// <param name="oApplication"></param>
		/// <param name="oServer"></param>
		/// <param name="oRequest"></param>
		public static void gInitApp(HttpApplicationState oApplication, HttpServerUtility oServer, 
			HttpRequest oRequest) 
		{
			// Wird ausgelst, wenn die Anwendung gestartet wird.

			//set flag, that application wasn't correct loaded yet:
			oApplication[efsApp_Up_And_Running] = false;


			//--------read easyFramework.config--------------
			XmlDocument oXml = new XmlDocument();
			oXml.gLoad(msGetFilePathConfigFile(oServer));

			//--------------get connection-string-----------------
			if (oXml.selectSingleNode("/easyFramework/dbconn/provider").sText != "SQLOLEDB.1")
			{
				throw (new efException("invalid db-provider:" + oXml.selectSingleNode("/easyFramework/dbconn/provider").sText));
			}

			string sConnstr = "data source=" + oXml.selectSingleNode("/easyFramework/dbconn/datasource").sText + ";initial catalog=" + oXml.selectSingleNode("/easyFramework/dbconn/initialCatalog").sText + ";" + oXml.selectSingleNode("/easyFramework/dbconn/securityinfo").sText;
			oApplication["ConnStr"] = sConnstr;


			//store web-page root:
			string sWebPageRoot = oRequest.ApplicationPath;
			if (sWebPageRoot == "/")
			{
				sWebPageRoot = "";
			}
			oApplication["sWebpageRoot"] = sWebPageRoot;

			//store the easyframework-config file as a cachedfileobject
			CachedFile oCachedFile = new CachedFile();
			oCachedFile.sCompleteSourceFilePath = oServer.MapPath(sWebPageRoot + "/easyFramework.config");
			oCachedFile.gForceFetchFileContent();
			oApplication["easyFrameworkConfig"] = oCachedFile;

			//get startup-page:
			oApplication["startupPage"] = sWebPageRoot + oXml.selectSingleNode("/easyFramework/startupPage").sText;

			//get startup-logo:
			oApplication["startupLogo"] = sWebPageRoot + oXml.selectSingleNode("/easyFramework/startupLogo").sText;

			//get main-page:
			oApplication["mainPage"] = sWebPageRoot + oXml.selectSingleNode("/easyFramework/mainPage").sText;

			//get project-title:
			oApplication["projectName"] = oXml.selectSingleNode("/easyFramework/projectName").sText;

			//get temp-folder:
			oApplication["tempFolderUrl"] = sWebPageRoot + oXml.selectSingleNode("/easyFramework/tempFolder").sText;
			oApplication["tempFolder"] = oServer.MapPath((string) oApplication["tempFolderUrl"]);

			//get db-update scripts-folder:
			oApplication["dbUpdateScriptsFolder"] = oServer.MapPath(sWebPageRoot + oXml.selectSingleNode("/easyFramework/DBUpdateScriptsFolder").sText);

			//get distribution settings:
			oApplication["distribution_made_package_folder"] = oServer.MapPath(sWebPageRoot + oXml.selectSingleNode("/easyFramework/distribution/made_package_folder").sText);
			oApplication["distribution_received_package_folder"] = oServer.MapPath(sWebPageRoot + oXml.selectSingleNode("/easyFramework/distribution/received_package_folder").sText);
			oApplication["distribution_filelist_xml"] = oServer.MapPath(sWebPageRoot + oXml.selectSingleNode("/easyFramework/distribution/filelist_xml").sText);
			oApplication["distribution_receiving_asp"] = oXml.selectSingleNode("/easyFramework/distribution/receiving_asp").sText;


			//-------------run the db-update scripts (they are not run again, if already run)------------------
			easyFramework.Sys.AutoDBUpdate.Handler oAutoDBUpdate = new easyFramework.Sys.AutoDBUpdate.Handler(easyFramework.Sys.ToolLib.DataConversion.gsCStr(
				oApplication["dbUpdateScriptsFolder"]), sConnstr);
			oAutoDBUpdate.gRun();

			//------------------------load the help-system-object----------------------------------
			string sHelpDirectory = oXml.sGetValue("HelpFolder", "");
			sHelpDirectory = oServer.MapPath(sWebPageRoot + sHelpDirectory);
			HelpSystem oHelpSystem = new HelpSystem(sHelpDirectory);
			oApplication["helpsystem"] = oHelpSystem;

			//--------------------login-needed?------------------------------------------
			oApplication["autologin_user"] = oXml.sGetValue("autologin/username", "");
			oApplication["autologin_pwd"] = oXml.sGetValue("autologin/password", "");

			//------------------------set Application-objects----------------------------------------
			Logging oLog = new Logging(sConnstr);
			oApplication["oLog"] = oLog;
			Config oConfig = new Config(goGetMinimumClientInfo(oApplication, oServer), sConnstr);
			oApplication["EZCONFIG"] = oConfig;

			//--------------------------------call sysevent-engine-------------------------------------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseAfterEvents(goGetMinimumClientInfo(oApplication, oServer), "global.asax", "Application_Start", "", 
				oApplication, ref bRollback, ref aoReturnedObjects);



			//set flag, that application was correctly loaded:
			oApplication[efsApp_Up_And_Running] = true;

		}

		//================================================================================
		//Function:  msGetFilePathConfigFile
		//--------------------------------------------------------------------------------'
		//Purpose:   'returns the filepath of the config-file; we have limited access
		//           to application-path etc, so this function is tricky: it hangels
		//           through the folders, until it finds the config-file
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   12.04.2004 02:46:27
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static string msGetFilePathConfigFile(HttpServerUtility oServer)
		{


			const string sFileName = "easyFramework.config";

			string sPath = oServer.MapPath("");

			if (Functions.Right(sPath, 1) != "\\")
			{
				sPath += "\\";
			}								  

			while (! System.IO.File.Exists(sPath + sFileName) & sPath != "")
			{
				sPath = Directory.GetParent(sPath).Parent.FullName;
				if (Functions.Right(sPath, 1) != "\\")
				{
					sPath += "\\";
				}
	
			};				   


			if (sPath != "")
			{
				return sPath + sFileName;
			}
			else
			{
				throw (new efException("Configuration-File \"" + sFileName + "\" not found!"));
			}


		}



		//================================================================================
		//Function:		<moGetMinimumClientInfo>
		//--------------------------------------------------------------------------------
		//Purpose:		returns clientinfo with app and server object loaded
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:		clientinfo
		//--------------------------------------------------------------------------------
		//Created:	05.09.2004 16:36:20 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static ClientInfo goGetMinimumClientInfo(HttpApplicationState oApplication, 
			HttpServerUtility oServer)
		{

			ClientInfo oResult = new ClientInfo("", easyFramework.Sys.ToolLib.DataConversion.gsCStr(oApplication["ConnStr"]));

			oResult.oHttpApp = new easyFramework.Sys.Webobjects.HttpApp(oApplication);
			oResult.oHttpServer = new easyFramework.Sys.Webobjects.HttpServer(oServer);



			return oResult;

		}

		/// <summary>
		/// returns true, if the web-app is initialized
		/// </summary>
		/// <param name="oApp">the web-app</param>
		/// <returns></returns>
		public static bool gbIsAppInitialized(HttpApplicationState oApp) 
		{
			if (oApp[efsApp_Up_And_Running] == null)
				return false;

			if (!(bool) oApp[efsApp_Up_And_Running]) 
				return false;

			return true;
		}

	}
}
