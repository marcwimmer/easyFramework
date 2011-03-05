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
	//Class:     users2menueProcess

	//--------------------------------------------------------------------------------'
	//Module:    users2menueProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   store the usergroups/menueitem assignments
	//--------------------------------------------------------------------------------'
	//Created:   20.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class users2entityPopupsProcess : efProcessPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
	
		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		

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
				DataMethodsClientInfo.gDeleteTable(oClientInfo, "tsEntityPopupAccessTsUsers", "epu_usr_id=" + lUsr_id);
			
				//---------append all groups----------------------
				XmlNodeList nlGroups = oRequest.selectNodes("//*[starts-with(name(), 'epp_id_')]");
				for (int i = 0; i <= nlGroups.lCount - 1; i++)
				{
				
					string sEpp_id = Functions.Right(nlGroups[i].sName, Functions.Len(nlGroups[i].sName) - Functions.Len("epp_id_"));
				
				
					if (nlGroups[i].sText != "2")
					{
						string sExplicit_Access = nlGroups[i].sText;
					
						DataMethodsClientInfo.gInsertTable(oClientInfo, "tsEntityPopupAccessTsUsers", "epu_epp_id,epu_usr_id,epu_explicit_access, epu_inserted,epu_insertedby", "'" + DataTools.SQLString(sEpp_id) + "', " + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id) + "," + sExplicit_Access + "," + "getdate(), " + "'" + oClientInfo.gsGetUsername() + "'");
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
