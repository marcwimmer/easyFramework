using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     entityDialogLoad

	//--------------------------------------------------------------------------------'
	//Module:    entityDialogLoad.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   this data-page is used for loading the entity-data into
	//           a rendered dialog
	//--------------------------------------------------------------------------------'
	//Created:   01.04.2004 21:39:46
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class entityDialogLoad : efDataPage
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
		//Public Methods:
		//================================================================================



		//================================================================================
		//Function:  sGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the main-menu data for the treeview
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo, request
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.04.2004 10:01:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{

			string sEntityName;
			string sEntityID = "";
			string sEntitySearchPhrase = "";

			DefaultEntity oEntity;



			if (oRequest.selectSingleNode("//entity") == null)
			{
				return "Entity not given.";
			}
			else
			{
				sEntityName = oRequest.selectSingleNode("//entity").sText;
			}

			if (oRequest.selectSingleNode("//keyvalue") != null)
			{
				sEntityID = oRequest.selectSingleNode("//keyvalue").sText;
			}

			if (oRequest.selectSingleNode("//value") != null)
			{
				sEntitySearchPhrase = oRequest.selectSingleNode("//value").sText;
			}

			oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);


			if (!Functions.IsEmptyString(sEntitySearchPhrase) & Functions.IsEmptyString(sEntityID))
			{

				Recordset rsResults = oEntity.gRsSearchMainField(oClientInfo, sEntitySearchPhrase, "");

				if (rsResults.EOF)
				{
					return "SUCCESS;";
				}
				else
				{
					oEntity.gLoad(oClientInfo, rsResults[oEntity.sKeyFieldName].sValue);
	
					return "SUCCESS;" + oEntity.oFields[oEntity.sKeyFieldName].sValue + ";" + oEntity.gsToString(oClientInfo);
	
	
				}



			}
			else if (sEntityID != "")
			{
				oEntity.gLoad(oClientInfo, sEntityID);

				return "SUCCESS;" + sEntityID + ";" + oEntity.gsToString(oClientInfo);

			}
			else if (Functions.IsEmptyString(sEntityID)) 
			{
				return "SUCCESS;";
			}


			return "FAILURE..couldn't load entity from request: " + 
				oRequest.sXml;    


		}


	}

}
