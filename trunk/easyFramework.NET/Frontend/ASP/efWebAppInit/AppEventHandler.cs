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
	/// handles the most basic events on global-asa session-start, app-start etc.
	/// is called by global-asa
	/// </summary>
	public class AppEventHandler
	{
		public AppEventHandler()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oAppTransporter"></param>
		/// <param name="oAppContainer"></param>
		public static void Application_AuthenticateRequest(HttpApplicationState oApp, HttpServerUtility oServer, HttpRequest oRequest) 
		{
			if (!WebAppInit.gbIsAppInitialized(oApp)) return;

			//------------call sysevent-engine-----------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, oServer), 
				"global.asax", "Application_AuthenticateRequest", "", oApp, ref bRollback, ref aoReturnedObjects);
		}

		/// <summary>
		/// handles the Applications Begin-Request method
		/// </summary>
		/// <param name="oAppTransporter"></param>
		/// <param name="oAppContainer"></param>
		public static void Application_BeginRequest(HttpApplicationState oApp, HttpServerUtility oServer, HttpRequest oRequest) 
		{

			if (!WebAppInit.gbIsAppInitialized(oApp))
				WebAppInit.gInitApp(oApp, oServer, oRequest);
			
			//----------watch for a changed easyFramework.Config-file and reload app, when modified------------
			CachedFile oCachedFile = ((CachedFile) oApp["easyFrameworkConfig"]);

			if (oCachedFile.gbFileChanged())
			{
				oCachedFile.gForceFetchFileContent();
				
				WebAppInit.gInitApp(oApp, oServer, oRequest);
				oApp["easyFrameworkConfig"] = oCachedFile;

			}
	
			//------------call sysevent-engine-----------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, 
				oServer), "global.asax", "Application_BeginRequest", "", 
				oApp, ref bRollback, ref aoReturnedObjects);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oAppTransporter"></param>
		/// <param name="oAppContainer"></param>
		/// 
		public static void Application_End(HttpApplicationState oApp, HttpServerUtility oServer, HttpRequest oRequest) 
		{
			if (!WebAppInit.gbIsAppInitialized(oApp)) return;


			//------------call sysevent-engine-----------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturned = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, 
				oServer), "global.asax", "Application_End", "", 
				oApp, ref bRollback, ref aoReturned);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oApplication"></param>
		/// <param name="oServer"></param>
		/// <param name="oRequest"></param>
		/// <param name="oContext"></param>
		public static void Application_Error(HttpApplicationState oApp, HttpServerUtility oServer, HttpRequest oRequest, 
			HttpContext oContext) 
		{
			if (!WebAppInit.gbIsAppInitialized(oApp)) return;

			if (oApp["oLog"] != null)
			{
				Logging logger = (Logging)(oApp["oLog"]);

				Exception exOuter = oServer.GetLastError();
				Exception exBase = exOuter.GetBaseException();
				mAppendInnerException(exBase, logger);
				
				//if there was an http-compilation error, then show the source-code (common problem)
				if (exOuter.InnerException is HttpCompileException) 
				{
					HttpCompileException hcex = (HttpCompileException) exOuter.InnerException;
					//TODO: Einfügen der Meldung, welcher Source-codezeile den Fehler erzeugt hat.
					logger.gLog(Logging.efEnumLogTypes.efError, "Errorcode: " + hcex.ErrorCode + "\n" + hcex.Message + "\n" + "Sourcecode: \n" + hcex.SourceCode, null);
				}


			}

			//------------call sysevent-engine-----------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, oServer), "global.asax", "Application_Error", "", 
				oApp, ref bRollback, ref aoReturnedObjects);

		}

		private static void mAppendInnerException(Exception outerException, Logging oLog) 
		{
			
			string sMsg = "$2\n\nStacktrace:\n$3\n\nSource:$4\n\n";
			sMsg = sMsg.Replace("$2", outerException.Message);
			sMsg = sMsg.Replace("$3", outerException.StackTrace);
			sMsg = sMsg.Replace("$4", outerException.Source);

			oLog.gLog(Logging.efEnumLogTypes.efError, sMsg, null);

			//if (outerException.InnerException != null)
				//mAppendInnerException(outerException, oLog);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oApplication"></param>
		/// <param name="oServer"></param>
		public static void Application_Start(HttpApplicationState oApp, HttpServerUtility oServer) 
		{	
			//set flag, that passed this event
			oApp["passed_application_start"] = true;


			//------------call sysevent-engine-----------
			/*
			 * don't use, because connection-strings are not initialized
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, oServer), "global.asax", "Application_Start", "", 
				oApp, ref bRollback, ref aoReturnedObjects);
			*/
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oAppTransporter"></param>
		public static void Session_Start(HttpApplicationState oApp, HttpServerUtility oServer, HttpRequest oRequest) 
		{
			// Wird ausgelst, wenn die Sitzung gestartet wird.

			//------------call sysevent-engine-----------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturnedObjects = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, oServer), "global.asax", "Session_Start", "", 
				oApp, ref bRollback, ref aoReturnedObjects);
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oApplication"></param>
		/// <param name="oServer"></param>
		/// <param name="oRequest"></param>
		public static void Session_End(HttpApplicationState oApp, HttpServerUtility oServer, HttpRequest oRequest) 
		{
			if (!WebAppInit.gbIsAppInitialized(oApp)) return;

			//------------call sysevent-engine-----------
			SysEventEngine oSysEventEngine = new SysEventEngine();
			bool bRollback = false;
			object[] aoReturned = null;
			oSysEventEngine.gRaiseAfterEvents(WebAppInit.goGetMinimumClientInfo(oApp, oServer), "global.asax", "Session_End", "", 
				oApp, ref bRollback, ref aoReturned);
			
		}
	}
}
