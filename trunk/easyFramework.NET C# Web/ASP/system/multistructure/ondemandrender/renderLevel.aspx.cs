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
	//Class:     renderLevel

	//--------------------------------------------------------------------------------'
	//Module:    renderLevel.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   used by the multistructure-component, if a new-button is clicked,
	//           then this aspx-page calls the dialog-renderer to generate the input
	//
	//
	//           required parameters:
	//
	//           -current-level-id; "" if it is a new top-level in "levelid"
	//           -path to multistructure-xml-file in "multixml"
	//           -html-name of the form, which contains the structs "formname"
	//           -xml-dialog-id: a unique id of the dialog; for making the refresh-function
	//            for example in "xmldialogid"
	//           -namepraefix: the global name-praefix for all input-elements
	//            usually the level-id in "namepraefix"
	//
	//
	//           returns:
	//           html for input-boxes
	//
	//--------------------------------------------------------------------------------'
	//Created:   02.05.2004 18:56:53
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================



	public class renderLevel : efDataPage
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
	
	
			string sNamePraefix = oRequest.selectSingleNode("//namepraefix").sText;
			string sXmlDialogID = oRequest.selectSingleNode("//xmldialogid").sText;
			string sHtmlFormName = oRequest.selectSingleNode("//formname").sText;
			string sXmlFilename = oRequest.selectSingleNode("//multixml").sText;
			string sStartLevel = oRequest.selectSingleNode("//startlevel").sText;
	
			//-------------------load the xml-definition of the struct----------------------
			if (Functions.Left(sXmlFilename, 1) != "/")
			{
				throw (new efException("XML-structure filename must be absolute for calling multi-structure ondemand renderer."));
			}
	
	
			sXmlFilename = Server.MapPath(sXmlFilename);
			XmlDocument oXmlDefinition = new XmlDocument();
			oXmlDefinition.gLoad(sXmlFilename);
	
	
	
			//-----------------render and return html------------------------------
			MultistructureRenderer oMultiStructure = new MultistructureRenderer();
			string sResult;
			sResult = oMultiStructure.gsRenderSpecificLevel(oClientInfo, oXmlDefinition, sHtmlFormName, sXmlDialogID, sNamePraefix, sStartLevel, Request, Application, Server, null);
	
			return sResult;
	
		}



	}

}
