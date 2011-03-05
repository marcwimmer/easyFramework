using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Entities;
using easyFramework.Sys;

namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     tab_publications
	//--------------------------------------------------------------------------------'
	//Module:    tab_publications.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   list the publications of a survey an offer to download results;
	//           a new publication may also be created
	//--------------------------------------------------------------------------------'
	//Created:   10.05.2004 00:07:30
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class tab_publications : efDialogPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
		protected easyFramework.Frontend.ASP.WebComponents.efButton efBtnNewPublication;
	
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
	
	
		protected Recordset mrsPublications;
	
	
		public override void CustomInit(XmlDocument oXmlRequest)
		{
			base.CustomInit ();

			string sSurvey_id = oXmlRequest.sGetValue("svy_id", "");
		
			if (sSurvey_id == "")
			{
				return;
			}
		
			IEntity oEntity = EntityLoader.goLoadEntity(oClientInfo, "Publications");
		
		
			string sQry;
		
			sQry = "pub_svy_id=" + sSurvey_id;
		
		
			mrsPublications = oEntity.gRsSearch(oClientInfo, sQry, "");
		
		
		}
	
	
	}

}
