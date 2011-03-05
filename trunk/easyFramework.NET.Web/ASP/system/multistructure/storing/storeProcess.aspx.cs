using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     storeProcess
	//--------------------------------------------------------------------------------'
	//Module:    storeProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   stores the content of the multistructure-edit
	//
	//           returns "SUCCESS" or the error-string
	//--------------------------------------------------------------------------------'
	//Created:   03.05.2004 16:41:48
	//--------------------------------------------------------------------------------'
	//Changed:   16.05.2004 - transactions added, Marc
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class storeProcess : efProcessPage
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


		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
	
	
			if (oClientInfo.bHasErrors)
			{
				return oClientInfo.gsErrors();
			}
	
	
			MultiStructureHelper.gStoreMultiStructureInDatabase(oClientInfo, oRequest, Request, Application, Server);
	
	
			if (oClientInfo.bHasErrors)
			{
				return oClientInfo.gsErrors();
			}
			else
			{
				return "SUCCESS";
			}
	
		}



	}

}
