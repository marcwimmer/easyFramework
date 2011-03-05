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
	//Class:     memoEdit
	//--------------------------------------------------------------------------------'
	//Module:    memoEdit.aspx.vb
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

	public class memoEdit : efDialogPage
	{

		const bool efbDefaultUseHtmlEditor = true; //use extended html-editor per default

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
	
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnOk;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnAbort;
		protected easyFramework.Frontend.ASP.WebComponents.efXmlDialog EfXmlDialog1;
		protected easyFramework.Frontend.ASP.WebComponents.efXmlDialog EfXmlDialog2;
		public FreeTextBoxControls.FreeTextBox FreeTextBox1;

		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;

		#endregion


		protected string msTableName;
		protected string msPrimaryFieldName;
		protected string msPrimaryFieldValue;
		protected string msMemoFieldName;
		protected string msMemoFieldValue;


		public override void CustomInit (XmlDocument oXmlRequest)
		{
	
			//--------------------------read parameters either from
			//       request-oject or from table tsEntityMemos -----------------------------------
			bool bDefaultUseOfHtmlEditor;
	
			if (oXmlRequest.sGetValue("eme_id", "") != "")
			{
		
				//-------------------------get values from entity--------------------
		
				Recordset rsEntityMemos = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityMemos", "*", "eme_id=" + oXmlRequest.sGetValue("eme_id", ""), "", "", "");
		
				IEntity oEntity = EntityLoader.goLoadEntity(oClientInfo, rsEntityMemos["eme_ety_name"].sValue);
		
				oEntity.gLoad(oClientInfo, oXmlRequest.sGetValue("ety_id", ""));
		
				msTableName = oEntity.sTableName;
				msPrimaryFieldName = oEntity.sKeyFieldName;
				msPrimaryFieldValue = oEntity.oKeyField.sValue;
				msMemoFieldName = rsEntityMemos["eme_fieldname"].sValue;
				msMemoFieldValue = oEntity.oFields[msMemoFieldName].sValue;
				bDefaultUseOfHtmlEditor = rsEntityMemos["eme_extendedHtmlEditor"].bValue;
				sTitle = rsEntityMemos["eme_caption"].sValue;
		
			}
			else
			{
				bDefaultUseOfHtmlEditor = efbDefaultUseHtmlEditor;
				if (Functions.IsEmptyString(oXmlRequest.sGetValue("Table", "")))
				{
					throw (new efException("Parameter \"Table\" required."));
				}
				else
				{
					msTableName = oXmlRequest.sGetValue("Table", "");
				}
		
				if (Functions.IsEmptyString(oXmlRequest.sGetValue("PKField", "")))
				{
					throw (new efException("Parameter \"PKField\" (primary key field) required."));
				}
				else
				{
					msPrimaryFieldName = oXmlRequest.sGetValue("PKField", "");
				}
		
				if (Functions.IsEmptyString(oXmlRequest.sGetValue("PKValue", "")))
				{
					throw (new efException("Parameter \"PKValue\" (primary key value) required."));
				}
				else
				{
					msPrimaryFieldValue = oXmlRequest.sGetValue("PKValue", "");
				}
		
				if (Functions.IsEmptyString(oXmlRequest.sGetValue("MemoField", "")))
				{
					throw (new efException("Parameter \"MemoField\" (column name of memo-field) required."));
				}
				else
				{
					msMemoFieldName = oXmlRequest.sGetValue("MemoField", "");
				}
		
				//-------------------------get memo-value---------------------------------------
				Recordset rs = DataMethodsClientInfo.gRsGetTable(oClientInfo, msTableName, msMemoFieldName, msPrimaryFieldName + "='" + DataTools.SQLString(msPrimaryFieldValue) + "'", "", "", "");
		
				if (rs.EOF)
				{
					throw (new efException("Recordset with primary-key value \"" + msPrimaryFieldValue + "\" doesn't exist!"));
				}
		
		
				msMemoFieldValue = rs[msMemoFieldName].sValue;
		
				sTitle = "Memotext";
		
			}
	
	
	
			//---------------------build xml for xml-dialog--------------------
			XmlDocument oXmlData = new XmlDocument("<DIALOGINPUT/>");
	
			oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("Table", true).sText = msTableName;
			oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("PrimaryFieldName", true).sText = msPrimaryFieldName;
			oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("PrimaryFieldValue", true).sText = msPrimaryFieldValue;
			oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("MemoFieldName", true).sText = msMemoFieldName;
			oXmlData.selectSingleNode("/DIALOGINPUT").AddNode("MemoFieldValue", true).sText = msMemoFieldValue;
	
			EfXmlDialog1.sXmlValues = oXmlData.sXml;
	
	
			//-------set up extended html editor------------
			FreeTextBox1.SupportFolder = "../freeTextBox/";
	
			//--------decide textarea or html-editor------
			if (mbUseExtendedHtmlEditor(oClientInfo, bDefaultUseOfHtmlEditor))
			{
				EfXmlDialog2.Visible = false;
				EfXmlDialog2.sXmlValues = "";
		
				FreeTextBox1.Text = msMemoFieldValue;
		
			}
			else
			{
				//------EfXmlDialog2 contains the textarea-memo-edit-------
				EfXmlDialog2.sXmlValues = oXmlData.sXml;
				FreeTextBox1.Visible = false;
		
			}
	
			//---------------------inits--------------------
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../freeTextBox/FreeTextBox-mainScript.js", false, "Javascript");
			gAddScriptLink("../freeTextBox/FreeTextBox-ToolbarItemsScript.js", false, "Javascript");
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
	
	
	
	
	
		}


		//================================================================================
		//Function:  mbUseExtendedHtmlEditor
		//--------------------------------------------------------------------------------'
		//Purpose:   decides, wether to use the extended html-editor or not
		//--------------------------------------------------------------------------------'
		//Params:    bDefaultUseOfHtmlEditor - value of field eme_extendedHtmlEditor
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.05.2004 14:12:21
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected bool mbUseExtendedHtmlEditor(ClientInfo oClientInfo, bool bDefaultUseOfHtmlEditor)
		{
	
			if (oClientInfo.rsLoggedInUser["usr_not_extendedHtmlEditor"].bValue == true)
			{
				return false;
			}
			else
			{
				return bDefaultUseOfHtmlEditor;
			}
	
	
		}

	}

}
