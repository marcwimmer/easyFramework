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
	//Class:     users2usergroup
	//--------------------------------------------------------------------------------'
	//Module:    users2usergroup.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   setting the roles of a user
	//--------------------------------------------------------------------------------'
	//Created:   19.05.2004 22:30:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class usergroups2user : efDialogPage
	{

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
	
		}
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtnSave;
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtnAbort;

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

		protected int lUsr_id;

		protected IEntity oUserEntity;

		protected Recordset rsUserGroups;

		public override void CustomInit (XmlDocument oXmlRequest)
		{
	
			sTitle = "Gruppen zuordnen";
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efPopupMenue.js", true, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
	
			//-----------get the user-id-------------
			if (Functions.IsEmptyString(oXmlRequest.sGetValue("usr_id", "")))
			{
				throw (new efException("Parameter \"usr_id\" is required!"));
			}
			else
			{
				lUsr_id = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlRequest.sGetValue("usr_id", ""), 0);
			}
	
			//-------------get user-groups-----------------
			rsUserGroups = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsUserGroups", "*", "", "", "", "grp_name");
	
			//---------load user entity-------------------
			oUserEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "Users");
			oUserEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id));
	
	
		}


		public bool gbIsUsrgrp_id(int lGrp_id)
		{
	
			if (DataMethodsClientInfo.glDBCount(oClientInfo, "tsUsersTsUserGroups", "*", "usr_grp_usr_id=" + lUsr_id + " And usr_grp_grp_id=" + lGrp_id, "") > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
	
		}

	}

}
