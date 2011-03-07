using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default.ASP.system.log
{
	/// <summary>
	/// Zusammenfassung f�r clearLogProcess.
	/// </summary>
	public class clearLogProcess : easyFramework.Frontend.ASP.Dialog.efDataPage
	{
		//================================================================================
		//Function:  sGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the data for the datatable-control
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 23:54:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
		
			DataMethodsClientInfo.gTruncateTable(oClientInfo, "tsLog");

			return "OK!";
		
		
		}

	}
}
