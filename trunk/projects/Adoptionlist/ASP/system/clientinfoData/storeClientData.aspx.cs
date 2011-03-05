using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     storeClientData

	//--------------------------------------------------------------------------------'
	//Module:    storeClientData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   stores client-data to the database
	//--------------------------------------------------------------------------------'
	//Created:   20.08.2004 00:32:32
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class storeClientData : easyFramework.Frontend.ASP.Dialog.efProcessPage
	{

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{

		}

		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;

		private void Page_Init (System.Object sender, System.EventArgs e)
		{
			//CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
			//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
			InitializeComponent();
		}

		#endregion

		//================================================================================
		//Function:  sGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   retrieves the value of the stored information from the clientinfo
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.08.2004 00:33:39
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, easyFramework.Sys.Xml.XmlDocument oRequest)
		{

			string sName;
			string sValue;

			sName = oRequest.sGetValue("name", "");
			sValue = oRequest.sGetValue("value", "");

			oClientInfo[sName] = sValue;

			return "OK";

		}



	}

}
