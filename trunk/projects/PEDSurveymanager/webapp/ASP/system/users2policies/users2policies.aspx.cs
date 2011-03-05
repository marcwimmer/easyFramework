using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     users2policies
	//--------------------------------------------------------------------------------'
	//Module:    users2policies.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   setting which user has access to which menue-item
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 23:18:47
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class users2policies : efDialogPage
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
		

		private void Page_Init (System.Object sender, System.EventArgs e)
		{
			//CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
			//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
			InitializeComponent();
		}

		#endregion

		protected int lUsr_id;

		protected IEntity oUserEntity;

		protected Recordset rsPolicies;

		public override void CustomInit (XmlDocument oXmlRequest)
		{
	
			sTitle = "Policies zuordnen";
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
	
			//-------------get tabs-----------------
			rsPolicies = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsPolicies", "pol_id, pol_name, pol_description", "", "", "", "pol_name ASC");
	
	
			//---------load user-group entity-------------------
			oUserEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "Users");
			oUserEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id));
	
	
		}



		//================================================================================
		//Function:  gbIsPol_id
		//--------------------------------------------------------------------------------'
		//Purpose:   checks wether the given pol-id is assigned or not
		//           by reading from table tsPoliciesTsUsers
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:35:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int glIsPol_id(int lPol_id)
		{
	
			Recordset rstsPoliciesTsUsers;
	
			rstsPoliciesTsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsPoliciesTsUsers", "*", "pol_usr_pol_id='" + DataTools.SQLString(easyFramework.Sys.ToolLib.DataConversion.gsCStr(lPol_id)) + "' And pol_usr_usr_id=" + lUsr_id, "", "", "");
	
			if (rstsPoliciesTsUsers.EOF)
			{
				return 2;
			}
			else
			{
				if (rstsPoliciesTsUsers["pol_usr_explicit_access"].bValue == true)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}




		//================================================================================
		//Function:  msGetGroupAccess
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the access of the users-groups
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.05.2004 13:59:27
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msGetGroupAccess(int lPol_id)
		{
	
	
			if (efEnvironment.goGetEnvironment(Application).goPolicySec.gbHasAnyUserGroupAccessFromDB(oClientInfo, lPol_id, lUsr_id))
			{
				return "Zugriff";
			}
			else
			{
				return "kein Zugriff";
			}
	
		}

		//================================================================================
		//Function:  msGetSelectCombo
		//--------------------------------------------------------------------------------'
		//Purpose:   gets the select-input-element for assigning the user-rights;
		//           the correct-value is already chosen
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.05.2004 13:45:59
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msGetSelectCombo(int lPol_id)
		{
			ComboBox oSelect = new ComboBox();
			oSelect.gAddEntry("2", "wie Gruppe");
			oSelect.gAddEntry("1", "Zugriff");
			oSelect.gAddEntry("0", "kein Zugriff");
	
			oSelect.sSelectedValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(glIsPol_id(lPol_id));
	
			oSelect.sName = "pol_id_" + lPol_id;
	
			return oSelect.gRender();
	
		}

	}

}
