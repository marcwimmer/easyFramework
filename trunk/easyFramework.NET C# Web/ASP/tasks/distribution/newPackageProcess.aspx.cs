using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Tasks.Distribution;

namespace easyFramework.Project.Default.ASP.tasks.distribution
{
	//================================================================================
	//Class:     newPackageProcess

	//--------------------------------------------------------------------------------'
	//Module:    newPackageProcess.aspx.cs
	//--------------------------------------------------------------------------------'
	//Copyright:
	//--------------------------------------------------------------------------------'
	//Purpose:	creates with the help of the efDistributor a new package
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 01:42:26
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class newPackageProcess : efProcessPage
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
			
			XmlDocument oXmlFileList = new XmlDocument();
			oXmlFileList.gLoad(Convert.ToString(oClientInfo.oHttpApp.oGet("distribution_filelist_xml")));

			Packager.gMakePackage(oClientInfo, oXmlFileList);
			
	
	
			return "SUCCESS";
	
		}

	}

}
