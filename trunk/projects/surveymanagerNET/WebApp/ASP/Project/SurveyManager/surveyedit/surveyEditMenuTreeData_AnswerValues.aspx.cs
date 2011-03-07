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

	public class surveyEditMenuTreeData_AnswerValues : efDataPage
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
		private const string efsFieldList = "val_id, N'' as val_parent, " + 
				"val_index, val_text, val_default, val_correct, val_for_db, " +
				"0 as mnu_modalwindow, N'treeview_answervalue' as mnu_icon_normal, N'treeview_answervalue' as mnu_icon_opened, "+
				"N'0' as mnu_isFolder ";
	
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
			
			string sAns_id = oRequest.sGetValue("ans_id");
			if (sAns_id == null | sAns_id.Length == 0) sAns_id = "-1";
		
		
			//-----------Meneintrge---------
			sQry = "SELECT " + efsFieldList + " FROM tdAnswerValues WHERE val_ans_id=$1 ORDER BY val_index";
			sQry = sQry.Replace("$1", sAns_id);
		
			rs = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
		
			while (! rs.EOF)
			{
			
				mHelperBuildString(oClientInfo, rs, oS);
			
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
		
		
		
			//mnu-parent:
			sResultString.Append("NULL|");
			
		
			//qst-id:
			sResultString.Append(rsCurrentLine["val_id"].sValue + "|");
		
			string sText = "";

			//correct-image:
			if (rsCurrentLine["val_correct"].bValue)
				sText += "<img width=\"12\" src=\"../images/correct.gif\"/>";

			//mnu-text:
			if (rsCurrentLine["val_default"].bValue) sText += "<b>";
			sText += rsCurrentLine["val_text"].sValue;
			if (rsCurrentLine["val_default"].bValue) sText += "</b>";

			//append stext:
			sResultString.Append(sText + "|");

		
			//mnu-command:
			string sCommand = "mLoadAnswerValue('$1');";
			sCommand = sCommand.Replace("$1", rsCurrentLine["val_id"].sValue);
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
		
/*			string sQry = "SELECT " + efsFieldList + " FROM tsMainMenue " + 
				"WHERE mnu_parentid = '" + 
				DataTools.SQLString(rsCurrentLine["mnu_id"].sValue) + 
				"' ORDER BY mnu_index";
		
			Recordset rsChildren = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
			while (! rsChildren.EOF)
			{
			
				mHelperBuildString(oClientInfo, rsChildren, sResultString);
			
				rsChildren.MoveNext();
			};
*/		
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
