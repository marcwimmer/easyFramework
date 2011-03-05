using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     TocData

	//--------------------------------------------------------------------------------'
	//Module:    TocData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright:
	//--------------------------------------------------------------------------------'
	//Purpose:
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 15:41:54
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	public class TocData : efDataPage
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



		//================================================================================
		//Function:  sGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   get the data for the TOC of the help
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 15:43:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
	
			//--------build the toc------------
	
			Recordset rs;
			string sQry;
			FastString oS = new FastString();
	
			sQry = "SELECT " + efsFieldList + " FROM tsHelpToc " + "WHERE toc_parentid IS NULL ORDER BY toc_index";
	
			rs = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
	
			while (! rs.EOF)
			{
		
				mHelperBuildString(oClientInfo, rs, oS);
		
				rs.MoveNext();
			} ;
	
			return "OK-||-" + oS.ToString();
		}


		//================================================================================
		//Private Fields:
		//================================================================================
		private const string efsFieldList = "toc_id, toc_parentid, toc_title";


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
	
			//------get children now, because then not a folder-image but a textitem-image is displayed----
			string sQry = "SELECT " + efsFieldList + " FROM tsHelpToc " + "WHERE toc_parentid = '" + DataTools.SQLString(rsCurrentLine["toc_id"].sValue) + "' ORDER BY toc_index";
	
			Recordset rsChildren = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
	
	
			//--------------mnu-parent--------------------
			if (Functions.IsEmptyString(rsCurrentLine["toc_parentid"].sValue))
			{
				sResultString.Append("NULL|");
			}
			else
			{
				sResultString.Append(rsCurrentLine["toc_parentid"].sValue + "|");
			}
	
			//mnu-id:
			sResultString.Append(rsCurrentLine["toc_id"].sValue + "|");
	
			//---------------------------mnu-text---------------------------
			sResultString.Append(rsCurrentLine["toc_title"].sValue + "|");
	
			//---------------------------mnu-command---------------------------
			string sCommand;
			sCommand = "mLoadContent(null, '" + rsCurrentLine["toc_id"].sValue + "');";
	
			sResultString.Append(sCommand + "|");
	
			//---------------------------is folder---------------------------
			sResultString.Append("0" + "|");
	
			//---------------------------icon normal---------------------------
			string sImage;
			if (rsChildren.EOF)
			{
				sImage = "treeview_item";
			}
			else
			{
				sImage = "treeview_folder";
			}
	
			sResultString.Append(Images.sGetImageURL(oClientInfo, sImage, Request.ApplicationPath) + "|");
	
	
			//-------------------------icon opened---------------------------
			if (rsChildren.EOF)
			{
				sImage = "treeview_item";
			}
			else
			{
				sImage = "treeview_folder_open";
			}
			sResultString.Append(Images.sGetImageURL(oClientInfo, sImage, Request.ApplicationPath) + "|");
	
	
	
			//---------------------------carriage return line feed---------------------------
			sResultString.Append("-||-");
	
			while (! rsChildren.EOF)
			{
		
				mHelperBuildString(oClientInfo, rsChildren, sResultString);
		
				rsChildren.MoveNext();
			} ;
	
		}

	}

}
