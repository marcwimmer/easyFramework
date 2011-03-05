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
	//Class:     users2menue
	//--------------------------------------------------------------------------------'
	//Module:    users2menue.aspx.vb
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

	public class users2entityPopups : efDialogPage
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

		protected Recordset rsPopups;

		private void Page_Load (XmlDocument oXmlRequest)
		{
	
			sTitle = "Popup-Meneintrge zuordnen";
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
	
			//-------------get popups-----------------
			rsPopups = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityPopups", "epp_id, epp_ety_name, epp_caption", "", "", "", "epp_ety_name, epp_index ASC");
	
	
			//---------load user-group entity-------------------
			oUserEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "Users");
			oUserEntity.gLoad(oClientInfo, easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_id));
	
	
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
		protected string msGetGroupAccess(int lEpp_id)
		{
	
	
			if (efEnvironment.goGetEnvironment(Application).goEntityPopupSec.gbHasAnyUserGroupAccessFromDB(oClientInfo, lEpp_id, lUsr_id))
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
		protected string msGetSelectCombo(int lPopup_id)
		{
			ComboBox oSelect = new ComboBox();
			oSelect.gAddEntry("2", "wie Gruppe");
			oSelect.gAddEntry("1", "Zugriff");
			oSelect.gAddEntry("0", "kein Zugriff");
	
			oSelect.sSelectedValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(glIsPopup_id(lPopup_id));
	
			oSelect.sName = "epp_id_" + lPopup_id;
	
			return oSelect.gRender();
	
		}


		//================================================================================
		//Function:  gbIsPopup_id
		//--------------------------------------------------------------------------------'
		//Purpose:   checks wether the given menue-id is assigned or not
		//           by reading from table tsMenuAccessTsUserGroups
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.05.2004 23:35:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int glIsPopup_id(int lPopup_id)
		{
	
			Recordset rstsEntityPopupAccessTsUsers;
	
			rstsEntityPopupAccessTsUsers = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityPopupAccessTsUsers", "*", "epu_epp_id='" + DataTools.SQLString(easyFramework.Sys.ToolLib.DataConversion.gsCStr(lPopup_id)) + "' And epu_usr_id=" + lUsr_id, "", "", "");
	
			if (rstsEntityPopupAccessTsUsers.EOF)
			{
				return 2;
			}
			else
			{
				if (rstsEntityPopupAccessTsUsers["epu_explicit_access"].bValue == true)
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
	
	
		}





	}

}
