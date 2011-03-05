using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;
using easyFramework.Sys;
using easyFramework.Project.SurveyManager;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     configDlg
	//--------------------------------------------------------------------------------'
	//Module:    config.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   Configuration-Dialog
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 21:06:13
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'



	//================================================================================
	//Imports:
	//================================================================================

	public class configProcess : efProcessPage
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
		
			XmlNodeList oNodes = oRequest.selectNodes("//*");
			for (int i = 0; i <= oNodes.lCount - 1; i++)
			{
			
			
			
				if (Functions.Left(oNodes[i].sName, 3) == "txt" & Functions.Right(oNodes[i].sName, 4) != "_old")
				{
				
					string sName;
					sName = Functions.Right(oNodes[i].sName, Functions.Len(oNodes[i].sName) - 3);
				
					Config.gSetValue(oClientInfo, sName, oNodes[i].sText);
				
				}
			
			}
		
		
			return "SUCCESS";
		
		}
	
	
	
	
	}

}
