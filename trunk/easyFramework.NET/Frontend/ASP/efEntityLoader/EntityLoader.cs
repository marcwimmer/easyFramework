using System;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using easyFramework.Sys.Data.Table;
using easyFramework.Sys.Entities;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Frontend.ASP.ASPTools
	{
	//================================================================================
	//Class:     EntityLoader

	//--------------------------------------------------------------------------------'
	//Module:    EntityLoader.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   loads the specified entity; automatic load of the table-definitions
	//           is done here
	//--------------------------------------------------------------------------------'
	//Created:   21.04.2004 14:50:06
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class EntityLoader
	{
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private Cache moCache;
		
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
		
		//================================================================================
//Sub:       New
		//--------------------------------------------------------------------------------'
//Purpose:   Constructor
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   11.05.2004 08:22:29
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public EntityLoader() {
			moCache = new Cache();
			
			
		}
		//================================================================================
		//Function:  goLoadEntity
		//--------------------------------------------------------------------------------'
		//Purpose:  loads an entity; from database the table-def file is looked-up and
		//          retrieved from file-system
		//--------------------------------------------------------------------------------'
		//Params:    the clientinfo
		//           the sAppPath - the request.ApplicationPath
		//           the name of the entity
		//--------------------------------------------------------------------------------'
		//Returns:   the default-entity-object
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 15:11:42
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static DefaultEntity goLoadEntity(ClientInfo oClientInfo, string sEntityName)
		{
			
			//----------get entity-loader from application-context--------------
			
			EntityLoader oEntityLoader;
			if (oClientInfo.oHttpApp.oGet(msGetNameInWebApp()) == null)
			{
				//--------if it doesn't exist, so create it-----------
				oEntityLoader = new EntityLoader();
			}
			else
			{
				
				oEntityLoader = ((EntityLoader)(oClientInfo.oHttpApp.oGet(msGetNameInWebApp())));
			}
			
			
			return oEntityLoader.moLoadEntity(oClientInfo, sEntityName);
			
			
		}
		
		
		//================================================================================
		//Function:  goLoadEntity
		//--------------------------------------------------------------------------------'
		//Purpose:   not shared version of the function
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//-------------------------------------------------------------------r-------------'
		//Created:   11.05.2004 08:24:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected DefaultEntity moLoadEntity(ClientInfo oClientInfo, string sEntityName)
		{
			
			Recordset rsEntity = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntities", "ety_tableDefXml", "ety_name='" + easyFramework.Sys.Data.DataTools.SQLString(sEntityName) + "'", "", "", "");
			
			if (rsEntity.EOF)
			{
				throw (new efException("Entity \"" + sEntityName + "\" not found!"));
			}
			
			//-------------------reload the tabledef, if it is not cached-----------------
			efTableDefCached oTableDef;
			if (moCache.gHasObject(sEntityName) == false)
			{
				
				CachedFile oCachedFile = new CachedFile();
				oCachedFile.efTreatFileContent = efEnumFileTypes.efXml;
				
				
				string sFileName;
				sFileName = oClientInfo.oHttpApp.sApplicationPath() + "/" + rsEntity["ety_tableDefXml"].sValue;
				sFileName = Functions.Replace(sFileName, "//", "/");
				sFileName = oClientInfo.oHttpServer.sMapPath(sFileName);
				
				oCachedFile.sCompleteSourceFilePath = sFileName;
				
				oTableDef = new efTableDefCached(oCachedFile);
				moCache.gAddObject(sEntityName, oTableDef);
				
				
			}
			oTableDef = ((efTableDefCached)(moCache.gGetObject(sEntityName)));
			
			DefaultEntity oResult = new DefaultEntity(oClientInfo, sEntityName, oTableDef.oTableDef);
			
			return oResult;
			
		}
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
//Function:  msGetNameInWebApp
		//--------------------------------------------------------------------------------'
//Purpose:   returns the name of the entity-loader as a string
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   03.06.2004 00:30:43
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetNameInWebApp()
		{
			
			return "{F6D0BCA1-7A42-463f-9883-A9F71AF433CB}";
			
		}
		
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
