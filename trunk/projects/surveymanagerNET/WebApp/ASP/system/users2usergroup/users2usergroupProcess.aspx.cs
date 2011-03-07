using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     usergroups2userProcess

	//--------------------------------------------------------------------------------'
	//Module:    usergroups2userProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   store the usergroups/users assignments
	//--------------------------------------------------------------------------------'
	//Created:   20.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class users2usergroupProcess : efProcessPage
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
		
		
				int lGrp_id;
				lGrp_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("grp_id", ""), 0);
		
				if (lGrp_id == 0)
				{
					throw (new efException("Param \"grp_id\" missing!"));
				}
		
				//--------delete all current assignments of user to groups-----------
				DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsUsersTsUserGroups", "usr_grp_grp_id=" + lGrp_id);
		
				//---------append all groups----------------------
				XmlNodeList nlGroups = oRequest.selectNodes("//*[starts-with(name(), 'usr_id_')]");
				for (int i = 0; i <= nlGroups.lCount - 1; i++)
				{
			
					string sUsr_id = Functions.Right(nlGroups[i].sName, Functions.Len(nlGroups[i].sName) - Functions.Len("usr_id_"));
			
					int lUsr_grp_id = InternalClientInfo.glGetNextRecordID(oClientInfo, "tsUsersTsUserGroups");
			
					if (nlGroups[i].sText != "0")
					{
				
						DataMethodsClientInfo.gInsertTable(oClientInfo, "tsUsersTsUserGroups", "usr_grp_id,usr_grp_grp_id,usr_grp_usr_id", lUsr_grp_id + "," + lGrp_id + "," + sUsr_id);
					}
			
					InternalClientInfo.gUpdateTsRecordIds(oClientInfo, "tsUsersTsUserGroups", "usr_grp_id");
			
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
