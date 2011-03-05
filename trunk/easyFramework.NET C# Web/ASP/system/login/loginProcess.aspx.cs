using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     loginProcess

	//--------------------------------------------------------------------------------'
	//Module:    loginProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright:
	//--------------------------------------------------------------------------------'
	//Purpose:
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 01:42:26
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class loginProcess : efProcessPage
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
		
				string sUsername;
				string sPassword;
				sUsername = oRequest.selectSingleNode("//txtusername").sText;
				sPassword = oRequest.selectSingleNode("//txtpassword").sText;
		
				oClientInfo = ClientInfo.goGetNewClientInfo(
					efEnvironment.goGetEnvironment(Application).gsConnStr, 
					sUsername, sPassword);
		
		
			}
			catch (efException ex)
			{
				return ex.Message;
		
			}
			finally
			{
		
			}
	
	
			return "SUCCESS_" + oClientInfo.sClientID;
	
		}


		//================================================================================
		//Sub:       LoadClientInfo
		//--------------------------------------------------------------------------------'
		//Purpose:   override the init-event of the default-page; the reason is, that we
		//           have at this time no client-id yet. with the process of the login,
		//           the client-id is created.
		//
		//
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 23:11:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void LoadClientInfo ()
		{
			//
		}
	}

}
