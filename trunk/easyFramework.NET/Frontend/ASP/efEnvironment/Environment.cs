using System;
using System.Web;
using easyFramework.Frontend.ASP;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     Environment

	//--------------------------------------------------------------------------------'
	//Module:    environment.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   contains some environmental functions
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 02:21:26
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class efEnvironment
	{
	
	
		//================================================================================
		//Private Fields:
		//================================================================================
		private HttpApplicationState moWebApp;
	
		//================================================================================
		//Public Properties:
		//================================================================================
	
	
		//================================================================================
		//Property:  gsConnStr
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the connection-string
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 01:56:00
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsConnStr
		{
			get
			{
				return Convert.ToString(moWebApp["ConnStr"]);
			}
		}
	
	
		//================================================================================
		//Property:  oHelpSystem
		//--------------------------------------------------------------------------------'
		//Purpose:   gets the loaded help-system
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 18:24:25
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public OnlineHelp.HelpSystem goHelpSystem
		{
			get
			{
				return ((OnlineHelp.HelpSystem)(moWebApp["helpsystem"]));
			}
		}
	
	


		//================================================================================
		//Property:  gsTempFolder
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the temporary-folder (phiyscal)
		//           e.g. "c:\inetpub\wwwroot\app\tempFolder"
		//           (no ending backslash, no matter what in easyFramework.config)
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 12:26:35
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsTempFolder
		{
			get
			{
				string sResult = Convert.ToString(moWebApp["tempFolder"]);
			
				if (Functions.Right(sResult, 1) != "\\")
				{
					sResult += "\\";
				}

				//create folder, if not exists
				string sDirWithoutEndingSlash = sResult.Substring(0, sResult.Length - 1);
				try 
				{
					if (!System.IO.Directory.Exists(sDirWithoutEndingSlash)) 
					{
						System.IO.Directory.CreateDirectory(sDirWithoutEndingSlash);
					}
				}
				finally 
				{}
			
				return sResult;
			
			}
		}

		/// <summary>
		/// returns the tempfolder as url like: "http://www.host.com/virtual_path/"
		/// 
		/// </summary>
		public string gsTempFolderUrl 
		{
			get 
			{
				string sResult = Convert.ToString(moWebApp["tempFolderUrl"]);
			
				if (Functions.Right(sResult, 1) != "/")
				{
					sResult += "/";
				}
			
				return sResult;

			}
		}

	
		//================================================================================
		//Property:  gsMainPage
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the main-page, like /main/main.aspx
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 01:56:00
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsMainPage
		{
			get
			{
				return Convert.ToString(moWebApp["mainPage"]);
			}
		}
	
		//================================================================================
		//Property:  gsProjectName
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the title of the project
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 01:56:00
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsProjectName
		{
			get
			{
				return Convert.ToString(moWebApp["projectName"]);
			}
		}
	
	
	
	
		//================================================================================
		//Property:  goLogger
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the logging-object out of the application-context
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 18:54:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Logging goLogger
		{
			get
			{
				return ((Logging)(moWebApp["oLog"]));
			}
		}
	
	
		//================================================================================
		//Property:  goMenuTreeSec
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the menu-tree security-object, which says, which
		//           user has access to which menu-tree; if it doesn't exist, it
		//           is created on demand
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 18:54:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public easyFramework.Sys.Security.MenuTreeSec goMenuTreeSec
		{
			get
			{
			
				easyFramework.Sys.Security.MenuTreeSec oResult;
			
				if (moWebApp["oMenuTreeSec"] == null)
				{
					oResult = new easyFramework.Sys.Security.MenuTreeSec();
					moWebApp["oMenuTreeSec"] = oResult;
				}
			
			
				return ((easyFramework.Sys.Security.MenuTreeSec)(moWebApp["oMenuTreeSec"]));
			}
		}
	
		//================================================================================
		//Property:  goEntityPopupSec
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the entity-popup security-object, which says, which
		//           user has access to which entity-popup; if it doesn't exist, it
		//           is created on demand
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 18:54:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public easyFramework.Sys.Security.EntityPopupSec goEntityPopupSec
		{
			get
			{
			
				easyFramework.Sys.Security.EntityPopupSec oResult;
			
				if (moWebApp["oEntityPopupSec"] == null)
				{
					oResult = new easyFramework.Sys.Security.EntityPopupSec();
					moWebApp["oEntityPopupSec"] = oResult;
				}
			
			
				return ((easyFramework.Sys.Security.EntityPopupSec)(moWebApp["oEntityPopupSec"]));
			}
		}
	
		//================================================================================
		//Property:  goEntityTabSec
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the entity-tab security-object, which says, which
		//           user has access to which tab-popup; if it doesn't exist, it
		//           is created on demand
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 18:54:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public easyFramework.Sys.Security.EntityTabSec goEntityTabSec
		{
			get
			{
			
				easyFramework.Sys.Security.EntityTabSec oResult;
			
				if (moWebApp["oEntityTabSec"] == null)
				{
					oResult = new easyFramework.Sys.Security.EntityTabSec();
					moWebApp["oEntityTabSec"] = oResult;
				}
			
			
				return ((easyFramework.Sys.Security.EntityTabSec)(moWebApp["oEntityTabSec"]));
			}
		}
	
		//================================================================================
		//Property:  goPolicySec
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the policy security-object, which says, which
		//           user has access to which policy; if it doesn't exist, it
		//           is created on demand
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 18:54:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public easyFramework.Sys.Security.PolicySec goPolicySec
		{
			get
			{
			
				easyFramework.Sys.Security.PolicySec oResult;
			
				if (moWebApp["oPolicySec"] == null)
				{
					oResult = new easyFramework.Sys.Security.PolicySec();
					moWebApp["oPolicySec"] = oResult;
				}
			
			
				return ((easyFramework.Sys.Security.PolicySec)(moWebApp["oPolicySec"]));
			}
		}

		//================================================================================
		//Function:		<goGetConfig>
		//--------------------------------------------------------------------------------
		//Purpose:		returns the configuration-object
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	08.09.2004 23:52:25 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public easyFramework.Frontend.ASP.Config goConfig
		{

			get 
			{
				easyFramework.Frontend.ASP.Config oResult = 
					(easyFramework.Frontend.ASP.Config) moWebApp["EZCONFIG"];

				return oResult;
			}

		}
	
		//================================================================================
		//Public Methods:
		//================================================================================
	
		//================================================================================
		//Sub:       Constructor
		//--------------------------------------------------------------------------------'
		//Purpose:   Stores the reference to Application-object
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 02:24:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efEnvironment(HttpApplicationState App) 
		{
			moWebApp = null;
		
			moWebApp = App;
		
		}
	
	
		//================================================================================
		//Function:  oGetEnvironment
		//--------------------------------------------------------------------------------'
		//Purpose:   retrieves the environment class out of the application-context
		//--------------------------------------------------------------------------------'
		//Params:    the application-context
		//--------------------------------------------------------------------------------'
		//Returns:   environment-class
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 01:52:11
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static easyFramework.Frontend.ASP.ASPTools.efEnvironment goGetEnvironment(HttpApplicationState App)
		{
		
			easyFramework.Frontend.ASP.ASPTools.efEnvironment oResult;
			oResult = ((easyFramework.Frontend.ASP.ASPTools.efEnvironment)(App["oEnv"]));
		
			if (oResult == null)
			{
				oResult = new easyFramework.Frontend.ASP.ASPTools.efEnvironment(App);
				App["oEnv"] = oResult;
			}
			return oResult;
		
		}
		public static easyFramework.Frontend.ASP.ASPTools.efEnvironment goGetEnvironment(ClientInfo oClientInfo)
		{
		
			return goGetEnvironment(oClientInfo.oHttpApp.oHttpApplication);

		}	

		
	
	}

}
