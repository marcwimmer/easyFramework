using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     memoProcess
	//--------------------------------------------------------------------------------'
	//Module:    memoProcess.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   for editing a memo-field; can be used for any memo-fields, in
	//           any table
	//--------------------------------------------------------------------------------'
	//Created:   12.05.2004 22:11:27
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	public class memoProcess : efProcessPage
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
	
	
			//--------------------read parameters--------------------
			string sTableName;
			string sPrimaryKeyFieldName;
			string sPrimaryKeyFieldValue;
			string sMemoFieldName;
			string sMemoFieldValue;
	
			sTableName = oRequest.sGetValue("txtTable", "");
			sPrimaryKeyFieldName = oRequest.sGetValue("txtPrimaryFieldName", "");
			sPrimaryKeyFieldValue = oRequest.sGetValue("txtPrimaryFieldValue", "");
			sMemoFieldName = oRequest.sGetValue("txtMemoFieldName", "");
	
			//either the data is in "txtMemoFieldValue" or in "FreeTextBox1":
			sMemoFieldValue = oRequest.sGetValue("txtMemoFieldValue", "");
			if (Functions.IsEmptyString(sMemoFieldValue))
			{
				sMemoFieldValue = oRequest.sGetValue("FreeTextBox1", "");
			}
	
	
			//------------------store memo-------------------------------
			DataMethodsClientInfo.gUpdateMemo(oClientInfo, sTableName, sPrimaryKeyFieldName, sPrimaryKeyFieldValue, sMemoFieldName, sMemoFieldValue);
	
	
			return "SUCCESS";
	
	
	
		}



	}

}
