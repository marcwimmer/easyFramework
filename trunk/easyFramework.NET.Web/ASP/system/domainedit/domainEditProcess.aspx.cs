using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;


namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     domainEditProcess

	//--------------------------------------------------------------------------------'
	//Module:    domainEditProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   store the content of domains
	//--------------------------------------------------------------------------------'
	//Created:   24.05.2004 18:14:36
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class domainEditProcess : efProcessPage
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


			string sNewInternalValue = oRequest.sGetValue("new_internalvalue", "");
			string sNewKeyCaption = oRequest.sGetValue("new_caption", "");
			string sDom_id = oRequest.sGetValue("dom_id", "");

			if (Functions.IsEmptyString(sDom_id))
			{
				throw (new efException("Parameter \"dom_id\" required!"));
			}

			//-----check dom-id------
			Recordset rsDomain = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDomains", "*", "dom_id=" + sDom_id, "", "", "");
			if (rsDomain.EOF)
			{
				throw (new efException("Invalid dom_id \"" + sDom_id + "\"!"));
			}
			int lDom_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(sDom_id, 0);


			//-----------start transaction - delete all db-values - insert new values - store transaction ---
			oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);

			try
			{
	
	
				//-----insert new domain-value, if exists--------
				if (sNewInternalValue != "")
				{
					Domains.gInsertDomainValue(oClientInfo, lDom_id, sNewInternalValue, sNewKeyCaption, -1);
				}
	
				//-------update existing domains---------
				XmlNodeList nlKeys = oRequest.selectNodes("//*[starts-with(name(), 'dvl_id_')]");
				for (int i = 0; i <= nlKeys.lCount - 1; i++)
				{
		
					string sDvl_id;
					string sCaption;
					string sInternalValue;
		
					sDvl_id = nlKeys[i].sName;
					if (Functions.Len(sDvl_id) - Functions.Len("dvl_id_") > 0)
					{
						sDvl_id = Functions.Right(sDvl_id, Functions.Len(sDvl_id) - Functions.Len("dvl_id_"));
					}
		
					sCaption = oRequest.sGetValue("dvl_caption_" + sDvl_id, "");
					sInternalValue = oRequest.sGetValue("dvl_internalvalue_" + sDvl_id, "");
					Domains.gInsertDomainValue(oClientInfo, lDom_id, sInternalValue, sCaption, easyFramework.Sys.ToolLib.DataConversion.glCInt(sDvl_id, 0));
		
				}
	
	
				//-----------remove deleted domain-values----------
				XmlNodeList nlDeleted = oRequest.selectNodes("//*[starts-with(name(), 'dvl_deleted_id_')]");
				for (int i = 0; i <= nlDeleted.lCount - 1; i++)
				{
					string sDvl_id;
					sDvl_id = nlDeleted[i].sName;
					sDvl_id = Functions.Right(sDvl_id, Functions.Len(sDvl_id) - Functions.Len("dvl_deleted_id_"));
					if (nlDeleted[i].sText == "1")
					{
						Domains.gRemoveDomainValue(oClientInfo, lDom_id, sDvl_id);
					}
				}
	
	
	
	
				oClientInfo.CommitTrans();
	
			}
			catch (Exception ex)
			{
	
				oClientInfo.RollbackTrans();
	
				throw (ex);
	
			}


			return "SUCCESS";


		}



	}

}
