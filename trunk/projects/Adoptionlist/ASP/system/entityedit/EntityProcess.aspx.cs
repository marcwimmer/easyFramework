using System;
using easyFramework;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     EntityProcess

	//--------------------------------------------------------------------------------'
	//Module:    EntityProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   stores/updates the entity
	//--------------------------------------------------------------------------------'
	//Created:   08.04.2004 23:31:28
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class EntityProcess : efProcessPage
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
		//Private Fields:
		//================================================================================

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
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{

			//transform the dialogdata to a recordset:
			Recordset rsDialogData = easyFramework.Frontend.ASP.ASPTools.Tools.grsDialogOutput2Recordset(oRequest.selectSingleNode("//DIALOGOUTPUT").sXml);

			//get the entity-name:
			string sEntityName;
			if (oRequest.selectSingleNode("//entity") == null)
			{
				throw (new efException("entity-type missing."));
			}
			else
			{
				sEntityName = oRequest.selectSingleNode("//entity").sText;
			}

			//load the entity-object:
			DefaultEntity oEntity;
			oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);


			if (oRequest.selectSingleNode("//deleteEntity[@value='1']") == null == false)
			{
	
				oEntity.gLoad(oClientInfo, rsDialogData[oEntity.sKeyFieldName].sValue);
				oEntity.gDelete(oClientInfo);
	
				if (oClientInfo.bHasErrors)
				{
					return oClientInfo.gsErrors();
				}
				else
				{
					return "SUCCESS";
		
				}
	
			}
			else
			{
	
				//apply the rule-file:
				bool bResult;
				bResult = oEntity.oTableDef.gbCheckRecordset(oClientInfo, rsDialogData);
	
				//if rule errors exist, return the errors as string:
				if (! bResult)
				{
					return oClientInfo.gsErrors( 10);
				}
	
				//----------check key-field in dialog--------------
				if (! rsDialogData.oFields.gbIsField(oEntity.sKeyFieldName))
				{
					throw (new efException("The key-field must be at least a hidden-field in the dialog-xml, " + "so that the recordset can be updated!"));
				}
	
				//store the data:
				if (Functions.IsEmptyString(rsDialogData[oEntity.sKeyFieldName].sValue))
				{
					oEntity.gNew(oClientInfo, "");
				}
				else
				{
					oEntity.gLoad(oClientInfo, rsDialogData[oEntity.sKeyFieldName].sValue);
				}
	
				//apply the new values:
				for (int i = 0; i <= rsDialogData.oFields.Count - 1; i++)
				{
					if (oEntity.oFields.gbIsField(rsDialogData.oFields[i].sName))
					{				   
						oEntity.oFields[rsDialogData.oFields[i].sName].sValue = rsDialogData.oFields[i].sValue;
					}
				}
	
				//store the entity:
				oEntity.gSave(oClientInfo);
	
				if (oClientInfo.bHasErrors)
				{
					return oClientInfo.gsErrors();
				}
				else
				{
					//return the id:
					return "SUCCESS_" + oEntity.oKeyField.sValue;
		
				}
	
	
			}




		}

		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================



	}

}
