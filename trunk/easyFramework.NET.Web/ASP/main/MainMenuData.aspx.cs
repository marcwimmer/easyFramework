using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     MainMenuData

	//--------------------------------------------------------------------------------'
	//Module:    MainMenuData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright:
	//--------------------------------------------------------------------------------'
	//Purpose:
	//--------------------------------------------------------------------------------'
	//Created:   01.04.2004 21:39:46
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class MainMenuData : efDataPage
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


		//================================================================================
		//Public Methods:
		//================================================================================



		//================================================================================
		//Function:  sGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the main-menu data for the treeview
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo, request
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.04.2004 10:01:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
	
	
			Recordset rs;
			string sQry;
			FastString oS = new FastString();
	
			sQry = "SELECT " + efsFieldList + " FROM tsMainMenue " + "WHERE mnu_parentid IS NULL ORDER BY mnu_index";
	
			rs = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
	
			while (! rs.EOF)
			{
		
				mHelperBuildString(oClientInfo, rs, oS);
		
				rs.MoveNext();
			};
	
			return "OK-||-" + oS.ToString();
	
		}

		//================================================================================
		//Private Fields:
		//================================================================================
		private const string efsFieldList = "mnu_id, mnu_parentid, mnu_title, " + "mnu_command, mnu_modalwindow, mnu_icon_normal, mnu_icon_opened, mnu_isFolder ";

		//================================================================================
		//Private Methods:
		//================================================================================

		//================================================================================
		//Sub:       helper
		//--------------------------------------------------------------------------------'
		//Purpose:   for doing iterative calls
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.04.2004 10:08:06
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private void mHelperBuildString (ClientInfo oClientInfo, Recordset rsCurrentLine, FastString sResultString)
		{
	
	
			//-----------check access---------------
			bool bHasAccess;
			easyFramework.Sys.Security.MenuTreeSec oMenuSec = efEnvironment.goGetEnvironment(Application).goMenuTreeSec;
																	   
			bHasAccess = oMenuSec.gbHasAccessFromCache(oClientInfo, rsCurrentLine["mnu_id"].sValue);
			if (! bHasAccess)
			{
				return;
			}
	
			//--------------mnu-parent--------------------
			if (Functions.IsEmptyString(rsCurrentLine["mnu_parentid"].sValue))
			{
				sResultString.Append("NULL|");
			}
			else
			{
				sResultString.Append(rsCurrentLine["mnu_parentid"].sValue + "|");
			}
	
			//mnu-id:
			sResultString.Append(rsCurrentLine["mnu_id"].sValue + "|");
	
			//mnu-text:
			sResultString.Append(rsCurrentLine["mnu_title"].sValue + "|");
	
			//mnu-command:
			string sCommand = "";
			if (rsCurrentLine["mnu_command"].sValue != "")
			{
				sCommand = "gsShowWindow('" + oClientInfo.oHttpApp.sApplicationPath() + rsCurrentLine["mnu_command"].sValue + "',";
		
				if (rsCurrentLine["mnu_modalwindow"].bValue == true)
				{
					sCommand += "true";
				}
				else
				{
					sCommand += "false";
				}
				sCommand += ");";
			}
			sResultString.Append(sCommand + "|");
	
			//is folder:
			sResultString.Append(rsCurrentLine["mnu_isFolder"].lValue + "|");
	
			//icon normal:
			sResultString.Append(Images.sGetImageURL(oClientInfo, rsCurrentLine["mnu_icon_normal"].sValue, Request.ApplicationPath) + "|");
	
	
			//icon opened:
			sResultString.Append(Images.sGetImageURL(oClientInfo, rsCurrentLine["mnu_icon_opened"].sValue, Request.ApplicationPath) + "|");
	
	
	
			//carriage return line feed
			sResultString.Append("-||-");
	
			string sQry = "SELECT " + efsFieldList + " FROM tsMainMenue " + "WHERE mnu_parentid = '" + DataTools.SQLString(rsCurrentLine["mnu_id"].sValue) + "' ORDER BY mnu_index";
	
			Recordset rsChildren = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
			while (! rsChildren.EOF)
			{
		
				mHelperBuildString(oClientInfo, rsChildren, sResultString);
		
				rsChildren.MoveNext();
			} ;
	
		}

	}

}
