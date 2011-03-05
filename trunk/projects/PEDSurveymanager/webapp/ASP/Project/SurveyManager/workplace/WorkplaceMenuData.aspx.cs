using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Frontend.ASP;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Security;
using easyFramework.Sys.Entities;

namespace easyFramework.Project.SurveyManager
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

	public class WorkplaceMenuData : efDataPage
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
		//Private Fields:
		//================================================================================
		private IEntity moSurveyGroupEntity;
		private const string efsFieldList = "mnu_id, mnu_parentid, mnu_title, " + "mnu_command, mnu_modalwindow, mnu_icon_normal, mnu_icon_opened, mnu_isFolder ";
	
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
		
		
		
			//-----------Meneintrge---------
			sQry = "SELECT " + efsFieldList + " FROM tsMainMenue " + "WHERE mnu_parentid IS NULL ORDER BY mnu_index";
		
			rs = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
		
			while (! rs.EOF)
			{
			
				mHelperBuildString(oClientInfo, rs, oS);
			
				rs.MoveNext();
			};
		
		
			//----------survey groups------------------
		
			//----------load the surveygroup entity------------
			moSurveyGroupEntity = easyFramework.Frontend.ASP.ASPTools.EntityLoader.goLoadEntity(oClientInfo, "SurveyGroups");
		
		
			rs = moSurveyGroupEntity.gRsSearch(oClientInfo, 
				"svg_parentid IS NULL", "svg_name");
		
			while (! rs.EOF)
			{
			
				mHelperBuildStringSurveyGroup(oClientInfo, rs, oS);
			
				rs.MoveNext();
			};
		
		
			//----------return string------------------
		
			return "OK-||-" + oS.ToString();
		
		}
	
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
		
			MenuTreeSec oMenuSec = efEnvironment.goGetEnvironment(Application).goMenuTreeSec;
			if (! oMenuSec.gbHasAccessFromCache(oClientInfo, rsCurrentLine["mnu_id"].sValue))
			{
				return;
			}
		
		
			//mnu-parent:
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
			if (!Functions.IsEmptyString(rsCurrentLine["mnu_command"].sValue))
			{
				sCommand = "gsShowWindow('" + Request.ApplicationPath + 
					rsCurrentLine["mnu_command"].sValue + "',";
			
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
			sResultString.Append(Images.sGetImageURL(oClientInfo, 
				rsCurrentLine["mnu_icon_normal"].sValue, Request.ApplicationPath) + "|");
		
		
			//icon opened:
			sResultString.Append(Images.sGetImageURL(oClientInfo, 
				rsCurrentLine["mnu_icon_opened"].sValue, Request.ApplicationPath) + 
				"|");
		
		
		
		
			//carriage return line feed
			sResultString.Append("-||-");
		
			string sQry = "SELECT " + efsFieldList + " FROM tsMainMenue " + 
				"WHERE mnu_parentid = '" + 
				DataTools.SQLString(rsCurrentLine["mnu_id"].sValue) + 
				"' ORDER BY mnu_index";
		
			Recordset rsChildren = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
			while (! rsChildren.EOF)
			{
			
				mHelperBuildString(oClientInfo, rsChildren, sResultString);
			
				rsChildren.MoveNext();
			};
		
		}
	
		private void mHelperBuildStringSurveyGroup(ClientInfo oClientInfo, 
			Recordset rsCurrentLine, FastString sResultString)
		{
		
			//mnu-parent:
			if (Functions.IsEmptyString(rsCurrentLine["svg_parentid"].sValue))
			{
				sResultString.Append("surveygroups|");
			}
			else
			{
				sResultString.Append("svg_id" + 
					rsCurrentLine["svg_parentid"].sValue + "|");
			}
		
			//mnu-id:
			sResultString.Append("svg_id" + rsCurrentLine["svg_id"].sValue + "|");
		
			//mnu-text:
			sResultString.Append(rsCurrentLine["svg_name"].sValue + "|");
		
			//mnu-command:
			string sCommand;
		
			sCommand = "mOnMenuItemSurveyGroupClick('" + 
				rsCurrentLine["svg_id"].sValue + "');";
			sResultString.Append(sCommand + "|");
		
			//is folder:
			sResultString.Append("1|");
		
			//icon normal:
			sResultString.Append(Images.sGetImageURL(oClientInfo, 
				"treeview_folder", Request.ApplicationPath) + "|");
		
		
			//icon opened:
			sResultString.Append(Images.sGetImageURL(oClientInfo, 
				"treeview_folder_open", Request.ApplicationPath) + "|");
		
		
		
			//carriage return line feed
			sResultString.Append("-||-");
		
			Recordset rsChildren = moSurveyGroupEntity.gRsSearch(oClientInfo, 
				"svg_parentid='" + DataTools.SQLString(rsCurrentLine["svg_id"].sValue) + "'", 
				"svg_name");
			while (! rsChildren.EOF)
			{
			
				mHelperBuildStringSurveyGroup(oClientInfo, rsChildren, sResultString);
			
				rsChildren.MoveNext();
			};
		
		}
	
	}

}
