using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     entityDialogData

	//--------------------------------------------------------------------------------'
	//Module:    entityDialogData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   loads the data for the xml-dialog of the entity-edit-form
	//--------------------------------------------------------------------------------'
	//Created:   07.04.2004 21:46:11
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class entityDialogData : efDataPage
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
		//Purpose:   fetch the entity-data and return the string for the xml-dialog to
		//           display the data
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//           request-object
		//--------------------------------------------------------------------------------'
		//Returns:   entity-xmldialog-data
		//--------------------------------------------------------------------------------'
		//Created:   07.04.2004 21:47:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
	
			if (Functions.IsEmptyString(oRequest.sGetValue("entity", "")))
			{
				throw (new efException("entity-type is missing, e.g. &lt;entity&gtUsers&lt;entity&gt"));
			}
			string sEntityName = oRequest.sGetValue("entity", "");
	
			DefaultEntity oEntity;
			oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);
	
			string sEntityID;
			if (oRequest.sGetValue("entityId", "") != "")
			{
				sEntityID = oRequest.sGetValue("entityId", "");
				oEntity.gLoad(oClientInfo, sEntityID);
			}
			else
			{
				oEntity.gNew(oClientInfo, "");
			}
	
	
			return Tools.gsXmlRecordset2DialogInput(oEntity.gRsGetRecordset(), Tools.efEnumDialogInputType.efJavaScriptString, -1);
	
	
		}



	}

}
