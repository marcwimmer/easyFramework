using System;
using easyFramework.Sys.Entities;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys.Security;
using easyFramework.Sys;

namespace easyFramework.Frontend.ASP.ComplexObjects
{
	//================================================================================
	//Class:     EntityPopupMenueRenderer

	//--------------------------------------------------------------------------------'
	//Module:    EntityPopupMenueRenderer.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   creates the popup-menue for an entity-dialog
	//--------------------------------------------------------------------------------'
	//Created:   29.04.2004 21:22:16
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class EntityPopupMenueRenderer : PopMenueRenderer
	{



		//================================================================================
		//Private Methods:
		//================================================================================
		private IEntity moEntity;
		private string msWebPageRoot;
		private string msJavaFuncAfterSearch;
		private string msJavaFuncToGetEntityId;
		private bool mbWithSearch;
		private bool mbWithEdit;
		private string msMemoEditDialogPage;



		//================================================================================
		//Public Properties:
		//================================================================================
		public IEntity oEntity
		{
	
			set
			{
				moEntity = value;
		
			}
		}


		//================================================================================
		//Property:  bWithSearch
		//--------------------------------------------------------------------------------'
		//Purpose:   wether to show the search button or not
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 09:56:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bWithSearch
		{
			get
			{
				return mbWithSearch;
			}
			set
			{
				mbWithSearch = value;
			}
		}


		//================================================================================
		//Property:  bWithEdit
		//--------------------------------------------------------------------------------'
		//Purpose:   wether to show the edit-button or not
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 09:55:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bWithEdit
		{
			get
			{
				return mbWithEdit;
			}
			set
			{
				mbWithEdit = value;
			}
		}

		//================================================================================
		//Property:  sJavaFuncAfterSearch
		//--------------------------------------------------------------------------------'
		//Purpose:   the function, which is called, after a search-dialog is closed;
		//           the entity-parameter is given in the param to the function
		//           if no function is given here, then function will be called after search
		//           and no search will be available
		//
		//           you have to give exactly the java-syntax to call the function:
		//
		//           e.g. "gSetEntityValue(entityId);"
		//
		//           In the function, which is called after the search dialog is closed,
		//           you can access the oModalResult-global-variable.
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   29.04.2004 22:14:21
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sJavaFuncAfterSearch
		{
			set
			{
				msJavaFuncAfterSearch = value;
			}
		}


		//================================================================================
		//Property:  sJavaFuncToGetEntityId
		//--------------------------------------------------------------------------------'
		//Purpose:   the java-function call syntax (complete) to retrieve the
		//           entity-id
		//
		//           e.g.: "gsGetCurrentEntityID()" (no ending semi-colon!)
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   29.04.2004 22:21:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sJavaFuncToGetEntityId
		{
			set
			{
		
				if (!Functions.IsEmptyString(value))
				{
					if (Functions.Right(value, 1) == ";")
					{
						value = Functions.Left(value, Functions.Len(value) - 1);
					}
				}
				msJavaFuncToGetEntityId = value;
			}
		}


		//================================================================================
		//Public Methods:
		//================================================================================
		public EntityPopupMenueRenderer(IEntity oEntity, string sWebPageRoot) 
		{
			mbWithSearch = true;
			mbWithEdit = true;
			msMemoEditDialogPage = "/ASP/system/memoedit/memoEdit.aspx";
	
			msWebPageRoot = sWebPageRoot;
			this.oEntity = oEntity;
	
		}

		public string Render(ClientInfo oClientInfo, string sId, System.Web.UI.WebControls.Unit oWidth, bool bJavaCodeToJavaContainer)
		{
	
	
	
			mUpdateFields(oClientInfo);
	
			return base.Render(sId, oWidth, bJavaCodeToJavaContainer);
	
	
		}

		//================================================================================
		//Private Methods:
		//================================================================================

		//================================================================================
		//Sub:       mUpdateFields
		//--------------------------------------------------------------------------------'
		//Purpose:   adapts the hrefs; replacing $1 with entity-id for example
		//--------------------------------------------------------------------------------'
		//Params:    ByVal oClientInfo As ClientInfo
		//--------------------------------------------------------------------------------'
		//Created:   09.05.2004 19:40:55
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private void mUpdateFields (ClientInfo oClientInfo)
		{
	
			this.gClear();
	
			//---------------search-button----------------------------
			string sHref;
			if (!Functions.IsEmptyString(msJavaFuncAfterSearch) & mbWithSearch)
			{
				sHref = moEntity.sSearchDialog;
				sHref = Functions.Replace(msWebPageRoot + sHref, "//", "/");
		
				gAddOption(sHref, "", true, msJavaFuncAfterSearch, "Suchen");
				gAddSpacer();
			}
	
	
			//---------------edit-button----------------------------
			if (!Functions.IsEmptyString(msJavaFuncToGetEntityId) & mbWithEdit)
			{
				string sCommandEdit = msGetEntityEditOnClick(msWebPageRoot, moEntity, msJavaFuncToGetEntityId);
				gAddOption("", sCommandEdit, false, "", "Bearbeiten");
			}
	
	
			//---------------memo-fields-button----------------------------
			Recordset rsEntityMemos = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsEntityMemos", "*", "eme_ety_name='" + DataTools.SQLString(moEntity.sName) + "'", "", "", "eme_index");
			if (! rsEntityMemos.EOF)
			{
				gAddSpacer();
			}
			while (! rsEntityMemos.EOF)
			{
		
				sHref = msMemoEditDialogPage;
				sHref = Functions.Replace(msWebPageRoot + sHref, "//", "/");
				sHref += "?eme_id=" + rsEntityMemos["eme_id"].sValue;
				sHref += "&ety_id=$1";
		
				string sFunc;
				sFunc = "if (" + msJavaFuncToGetEntityId + "== '') " + "{ efMsgBox('Bitte whlen Sie einen Eintrag aus!', 'WARNING'); } else gsShowWindow('" + sHref + "', false, '');";
				sFunc = Functions.Replace(sFunc, "$1", "'+" + msJavaFuncToGetEntityId + "+'");
		
				this.gAddOption("", sFunc, false, "", rsEntityMemos["eme_caption"].sValue);
		
		
		
				rsEntityMemos.MoveNext();
			} ;
	
	
	
	
	
			//-------------------adapt popup-links with security check----------------------
			EntityPopupSec oPopupSecurity = new EntityPopupSec();
	
			TPopupLink[] aoPopupLinks = moEntity.gaoGetPopupLinks();
	
			if (aoPopupLinks.Length > 0)
			{
				gAddSpacer();
			}
	
			for (int i = 0; i <= aoPopupLinks.Length - 1; i++)
			{
		
				if (oPopupSecurity.gbHasAccessFromCache(oClientInfo, aoPopupLinks[i].lId))
				{
			
					//------replace parameter $1 with entity-id------------------
					sHref = aoPopupLinks[i].sUrl;
					sHref = msWebPageRoot + sHref;
					sHref = Functions.Replace(sHref, "//", "/");
			
					//replace the $1 with the javascript-function to get the entity:
					if (Functions.InStr(sHref, "$1") == 0)
					{
						this.gAddOption(sHref, "", false, "", aoPopupLinks[i].sCaption);
					}
					else
					{
				
						string sFunc;
						sFunc = "if (" + msJavaFuncToGetEntityId + "== '') " + "{ efMsgBox('Bitte whlen Sie einen Eintrag aus!', 'WARNING'); } else gsShowWindow('" + sHref + "', false, '');";
						sFunc = Functions.Replace(sFunc, "$1", "'+" + msJavaFuncToGetEntityId + "+'");
				
						this.gAddOption("", sFunc, false, "", aoPopupLinks[i].sCaption);
				
					}
			
				}
		
			}
	
		}


		//================================================================================
		//Function:  msGetEntityEditOnClick
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the javascript of editing the current selected entity
		//           the dialog which is stored in ety_editdialog is opened.
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 15:42:43
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static string msGetEntityEditOnClick(string sWebpageRoot, IEntity oDefaultEntity, string sJavaFuncToGetEntityId)
		{
	
			string sResult;
			string sHref = oDefaultEntity.gsEditDialogResolved("$1");
			sHref = sWebpageRoot + Functions.Replace(sHref, "$1", "");
			sHref = Functions.Replace(sHref, "//", "/");
	
			sResult = "if(" + sJavaFuncToGetEntityId + " == ''){efMsgBox('Bitte whlen Sie vorher einen Eintrag aus!','WARNING');}" + "else { gsShowWindow('" + sHref + "'+" + sJavaFuncToGetEntityId + ",false); }";
	
			return sResult;
	
		}

	}

}
