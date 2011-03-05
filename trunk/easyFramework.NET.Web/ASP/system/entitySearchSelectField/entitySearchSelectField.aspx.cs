using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     entitySearchSelectField

	//--------------------------------------------------------------------------------'
	//Module:    entitySearchSelectField.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   the user can select a field and enter a search-string here;
	//           the modal-result is
	//           FIELDNAME;search-string
	//--------------------------------------------------------------------------------'
	//Created:   25.04.2004 16:52:21
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class entitySearchSelectField : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efDataTable EfDataTable1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtn_Ok;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtn_Abort;
		protected easyFramework.Frontend.ASP.WebComponents.efXmlDialog EfXmlDialog1;
	
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
	
		public override void CustomInit (XmlDocument oXmlRequest)
		{
			// Hier Benutzercode zur Seiteninitialisierung einfgen
		
			//------------get the entity------------------------
			if (Functions.IsEmptyString(Request["Entity"]))
			{
				throw (new efException("entity-type is missing, e.g. ?entity=user"));
			}
			string sEntityName = Request["Entity"];
		
			DefaultEntity oEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, sEntityName);
		
		
			//-------------setup dialog-xml --------------------
			XmlDocument oXml = new XmlDocument("<efDialogPage MULTIROW=\"0\" DIALOGSIZE=\"1\">" + "<efDialogRow>" + "<efDialogField>" + "<DESC>Suchbe$griff</DESC>" + "<NAME>searchphrase</NAME>" + "<TYPE>INPUT</TYPE>" + "</efDialogField></efDialogRow><efDialogRow>" + "<efDialogField COLSPANFIELD=\"1\">" + "<DESC>Such$feld</DESC>" + "<NAME>searchfield</NAME>" + "<TYPE>LISTCOMBO</TYPE>" + "<SRC></SRC>" + "<DATA></DATA>" + "</efDialogField>" + "</efDialogRow>" + "</efDialogPage>");
		
			for (int i = 0; i <= oEntity.asSearchFields.Length - 1; i++)
			{
			
			
			
				oXml.selectSingleNode("//SRC").sText += oEntity.oTableDef.gsGetFieldDescription(oEntity.asSearchFields[i].sName) + easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.IIf(i < oEntity.asSearchFields.Length - 1, ";", ""));
			
			
				oXml.selectSingleNode("//DATA").sText += oEntity.asSearchFields[i].sName + 
					easyFramework.Sys.ToolLib.DataConversion.gsCStr(
						Functions.IIf(i < oEntity.asSearchFields.Length - 1, ";", ""));
			
			}
			EfXmlDialog1.sDefinitionFile = oXml.sXml;
		
		
			//----------js, css, title----------
			sTitle = oEntity.sTitle;
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../js/efTabDialog.js", true, "Javascript");
			gAddScriptLink("../../js/efClientData.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
			gAddCss("../../css/efstyledatatable.css", true);
			gAddCss("../../css/efstyletabdlg.css", true);
		
		}
	
	}

}
