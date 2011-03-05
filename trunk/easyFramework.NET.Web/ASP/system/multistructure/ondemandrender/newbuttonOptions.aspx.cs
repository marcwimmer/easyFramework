using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     newbuttonOptions

	//--------------------------------------------------------------------------------'
	//Module:    newbuttonOptions.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   retrieves the valid options for a new button
	//--------------------------------------------------------------------------------'
	//Created:   31.05.2004 00:14:15
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class newbuttonOptions : efDataPage
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
	
	
	
	
			string sXmlFilename = oRequest.selectSingleNode("//multixml").sText;
			string sStartLevel = oRequest.selectSingleNode("//startlevel").sText;
			string sThisSubLevel = oRequest.selectSingleNode("//levelhierarchy").sText; //either "this" or "sub"
			MultistructureRenderer.efEnumLevel enLevel;
	
			switch (Functions.LCase(sThisSubLevel))
			{
				case "sub":
			
					enLevel = MultistructureRenderer.efEnumLevel.sublevel;
					break;
				case "this":
			
					enLevel = MultistructureRenderer.efEnumLevel.thisLevel;
					break;
				default:
			
					return "Please Provide either \"this\" or \"sub\" in parameter \"levelhierachy\", when calling \"newbuttonOptions.aspx\".";
			}
	
			//-------------------load the xml-definition of the struct----------------------
			if (Functions.Left(sXmlFilename, 1) != "/")
			{
				throw (new efException("XML-structure filename must be absolute for calling multi-structure ondemand renderer."));
			}
	
	
			sXmlFilename = Server.MapPath(sXmlFilename);
			XmlDocument oXmlDefinition = new XmlDocument();
			oXmlDefinition.gLoad(sXmlFilename);
	
			//----------get result-string-----------
			MultistructureRenderer oMultiStructure = new MultistructureRenderer();
			string sResult;
			sResult = "SUCCESS" + oMultiStructure.gsRenderOptionValuesForNewButton(oClientInfo, oXmlDefinition, sStartLevel, enLevel);
	
			return sResult;
	
	
	
		}



	}

}
