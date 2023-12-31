using System;

namespace easyFramework.Sys
{
	//================================================================================
//Class:     HttpApp
	
	//--------------------------------------------------------------------------------'
//Module:    HttpApp.vb
	//--------------------------------------------------------------------------------'
//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
//Purpose:   wrapper for web-Server, so that it can be transported
	//--------------------------------------------------------------------------------'
//Created:   04.06.2004 10:21:33
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	
	
	namespace Webobjects
	{
		
		
		
		public class HttpServer
		{
			
			
			//================================================================================
			//Private Fields:
			//================================================================================
			private System.Web.HttpServerUtility moServer;
			//================================================================================
			//Public Consts:
			//================================================================================
			
			//================================================================================
			//Public Properties:
			//================================================================================
			
			//================================================================================
			//Public Events:
			//================================================================================
			
			//================================================================================
			//Public Methods:
			//================================================================================
			public HttpServer(System.Web.HttpServerUtility oServer) {
				moServer = oServer;
			}
			public string sMapPath(string sPath)
			{
				return moServer.MapPath(sPath);
			}
			
			public System.Web.HttpServerUtility oHttpServer
			{
				get{
					return moServer;
				}
			}
			
			//================================================================================
			//Protected Properties:
			//================================================================================
			
			//================================================================================
			//Protected Methods:
			//================================================================================
			
			//================================================================================
			//Private Consts:
			//================================================================================
			
			//================================================================================
			//Private Fields:
			//================================================================================
			
			//================================================================================
			//Private Methods:
			//================================================================================
			
			
		}
		
	}
}
