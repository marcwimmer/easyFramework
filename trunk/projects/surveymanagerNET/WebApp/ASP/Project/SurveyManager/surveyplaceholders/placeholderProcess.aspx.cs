using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Entities;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     placeholderProcess

	//--------------------------------------------------------------------------------'
	//Module:    placeholderProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   stores the edited placeholders
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 14:50:39
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================
	public class placeholderProcess : efProcessPage
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
		
			int lSvg_id = -1;
			int lsvy_id = -1;
		
			//---------get url params--------------
			if (oRequest.sGetValue("svg_id", "") == "" & oRequest.sGetValue("svy_id", "") == "")
			{
				throw (new efException("Please provide either the parameter \"svg_id\" or \"svy_id\"!"));
			}
			if (oRequest.sGetValue("svg_id", "") != "")
			{
				lSvg_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("svg_id", ""), 0);
			}
			if (oRequest.sGetValue("svy_id", "") != "")
			{
				lsvy_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("svy_id", ""), 0);
			}
		
			//--------transform dialog-output to a recordset-------------
			Recordset rsData = Tools.grsDialogOutput2Recordset(oRequest.selectSingleNode("//DIALOGOUTPUT").sXml);
		
			//----------get entity----------------
			IEntity oEntity = EntityLoader.goLoadEntity(oClientInfo, "PlaceHolders");
		
			//------start trans---------
			oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
			try
			{
			
				do
				{
				
				
					if (rsData["plc_name"].sValue == "" & rsData["plc_id"].sValue != "")
					{
						//delete
						oEntity.gLoad(oClientInfo, rsData["plc_id"].sValue);
						if (! oEntity.gDelete(oClientInfo))
						{
							throw (new efException(oClientInfo.gsErrors()));
						}
					}
					else if (rsData["plc_id"].sValue != "")
					{
						oEntity.gLoad(oClientInfo, rsData["plc_id"].sValue);
					
						oEntity.oFields["plc_name"].sValue = rsData["plc_name"].sValue;
						oEntity.oFields["plc_value"].sValue = rsData["plc_value"].sValue;
					
						if (lsvy_id > 0)
						{
							oEntity.oFields["plc_svy_id"].lValue = lsvy_id;
						}
						if (lSvg_id > 0)
						{
							oEntity.oFields["plc_svg_id"].lValue = lSvg_id;
						}
					
						if (! oEntity.gSave(oClientInfo))
						{
							throw (new efException(oClientInfo.gsErrors()));
						}
					}
					else if (rsData["plc_id"].sValue == "" & rsData["plc_name"].sValue != "")
					{
						oEntity.gNew(oClientInfo, "");
					
						oEntity.oFields["plc_name"].sValue = rsData["plc_name"].sValue;
						oEntity.oFields["plc_value"].sValue = rsData["plc_value"].sValue;
						if (lsvy_id > 0)
						{
							oEntity.oFields["plc_svy_id"].lValue = lsvy_id;
						}
						if (lSvg_id > 0)
						{
							oEntity.oFields["plc_svg_id"].lValue = lSvg_id;
						}
					
						if (! oEntity.gSave(oClientInfo))
						{
							throw (new efException(oClientInfo.gsErrors()));
						}
					
					}
				
				
					rsData.MoveNext();
				} while (! rsData.EOF);
			
			
			
				//-------------------- return ---------------
				oClientInfo.CommitTrans();
				return "SUCCESS";
			
			}
			catch (Exception ex)
			{
			
				oClientInfo.RollbackTrans();
			
				throw (ex);
			
			}
		
		}
	
	}

}
