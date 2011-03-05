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
	//Class:     searchData

	//--------------------------------------------------------------------------------'
	//Module:    searchData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   searches in database for the search-string
	//--------------------------------------------------------------------------------'
	//Created:   06.06.2004 16:07:02
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class searchData : efDataPage
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
	
			//-------------------some dims-----------------
			FastString oResult = new FastString();
			Recordset oRsTocs;
			string sSearchTerm = oRequest.sGetValue("searchterm", "");
	
			//----------check search term--------------
			if (Functions.IsEmptyString(sSearchTerm))
			{
				return "Please provide a search-term!";
			}
	
	
			//-----------split term by " "-------------------
			string[] sSplitTerms;
			if (Functions.InStr(sSearchTerm, " ") == 0)
			{
				sSplitTerms = new string[1];
				sSplitTerms[0] = sSearchTerm;
			}
			else
			{
				sSplitTerms = Functions.Split(sSearchTerm, " ", -1);
			}
	
			//--------------------build query--------------------
			string sQry = "";
			for (int i = 0; i <= sSplitTerms.Length - 1; i++)
			{
				sQry += "PATINDEX('%" + sSplitTerms[i] + "%', hlp_body) > 0";
				if (i < sSplitTerms.Length - 2)
				{
					sQry += " AND ";
				}
			}
	
	
			//----------------get data-------------------
			oRsTocs = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_toc_id, hlp_heading", sQry, "", "", "hlp_heading");
	
	
			//--------------build result html---------------
			if (oRsTocs.EOF)
			{
				oResult.Append("<p>Leider wurden keine Ergebnisse zu \"" + sSearchTerm + "\" gefunden.");
		
			}
			else
			{
				oResult.Append("<p>");
				oResult.Append("Suchergebnisse fr \"" + sSearchTerm + "\":");
				oResult.Append("<table class=\"borderTable\">");
		
		
				while (! oRsTocs.EOF)
				{
					oResult.Append("<tr><td class=\"dlgField\">");
			
					oResult.Append("<a href=\"#\" onclick=\"" + "mLoadContent('" + oRsTocs["hlp_toc_id"].sValue + "'); return false;\">" + oRsTocs["hlp_heading"].sValue + "</a>");
			
					oResult.Append("</td></tr>");
			
					oRsTocs.MoveNext();
				} ;
				oResult.Append("</table>");
		
			}
	
			return oResult.ToString();
	
		}



	}

}
