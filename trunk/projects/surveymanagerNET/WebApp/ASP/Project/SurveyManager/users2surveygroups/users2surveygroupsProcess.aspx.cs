using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     users2policiesProcess

	//--------------------------------------------------------------------------------'
	//Module:    users2policiesProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   store the usergroups/policies assignments
	//--------------------------------------------------------------------------------'
	//Created:   20.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class users2surveygroupsProcess : efProcessPage
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
		
		
			try
			{
				oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
			
			
				int lUsr_id;
				lUsr_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("usr_id", ""), 0);
			
				if (lUsr_id == 0)
				{
					throw (new efException("Param \"usr_id\" missing!"));
				}
			
				//--------delete all current assignments of user to groups-----------
				DataMethodsClientInfo.gDeleteTable(oClientInfo, "tdSurveyGroupsTsUsers", "svg_usr_usr_id=" + lUsr_id);
			
				//---------append all groups----------------------
				XmlNodeList nlGroups = oRequest.selectNodes("//*[starts-with(name(), 'svg_id_')]");
				for (int i = 0; i <= nlGroups.lCount - 1; i++)
				{
				
					string sSvg_id = Functions.Right(nlGroups[i].sName, Functions.Len(nlGroups[i].sName) - Functions.Len("svg_id_"));
				
				
					if (nlGroups[i].sText == "1")
					{
					
						int lNextId = InternalClientInfo.glGetNextRecordID(oClientInfo, "tdSurveyGroupsTsUsers");
					
					
						DataMethodsClientInfo.gInsertTable(oClientInfo, "tdSurveyGroupsTsUsers", "svg_usr_id, svg_usr_svg_id, svg_usr_usr_id, svg_usr_inserted, svg_usr_insertedBy", lNextId + "," + "'" + DataTools.SQLString(sSvg_id) + "', " + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id) + "," + "getdate(), " + "'" + oClientInfo.gsGetUsername() + "'");
					
						InternalClientInfo.gUpdateTsRecordIds(oClientInfo, "tdSurveyGroupsTsUsers", "svg_usr_id");
					}
				
				}
			
			
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
