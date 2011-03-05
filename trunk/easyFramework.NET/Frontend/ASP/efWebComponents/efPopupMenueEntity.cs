using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.HTMLRenderer;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efPopMenueEntity

	//--------------------------------------------------------------------------------'
	//Module:    efPopMenueEntity.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   pop-up menue for entities; reads from table tsEntityPopups
	//--------------------------------------------------------------------------------'
	//Created:   23.04.2004 08:36:20
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	[DefaultProperty("Text"), ToolboxData("<{0}:efPopupMenueEntity runat=server></{0}:efPopupMenueEntity>")]public class efPopupMenueEntity : efBaseElement
	{
		public efPopupMenueEntity()
		{
			mbWithSearch = true;
			mbWithEdit = true;
		}





		//================================================================================
		//Private Fields:
		//================================================================================
		private DefaultEntity moEntity;
		private string msJavaScriptGetId;
		private EntityPopupMenueRenderer moComplexPopup;
		private bool mbWithSearch;
		private bool mbWithEdit;



		//================================================================================
		//Public Properties:
		//================================================================================

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
		//Property:  sJavaScriptGetId
		//--------------------------------------------------------------------------------'
		//Purpose:   sets the javascript-function for getting the entity-id
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   29.04.2004 07:47:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sJavaScriptGetId
		{
			get
			{
				return msJavaScriptGetId;
			}
			set
			{
				msJavaScriptGetId = value;
			}
		}

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       gSetEntity
		//--------------------------------------------------------------------------------'
		//Purpose:   loads the given entity-name
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   29.04.2004 07:45:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gSetEntity (ClientInfo oClientInfo, string sEntityName)
		{
	
			moEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);
	
		}
		//================================================================================
		//Protected Methods:
		//================================================================================
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
			//TODO: Call Complex-Popup-Object
	
			if (moEntity == null)
			{
				throw new efException("Entity in Entity-Popupmenue is missing. (ID \"" + this.ID + "\"");
			}
	
			moComplexPopup = new EntityPopupMenueRenderer(moEntity, Parent.Page.Request.ApplicationPath);
			moComplexPopup.sJavaFuncToGetEntityId = this.sJavaScriptGetId;
			moComplexPopup.bWithSearch = mbWithSearch;
			moComplexPopup.bWithEdit = mbWithEdit;
	
			FastString oResult = new FastString();
	
			ClientInfo oClientInfo = ((efPage)(this.Parent.Page)).oClientInfo;
	
			oResult.Append(moComplexPopup.Render(oClientInfo, this.ID, this.Width, false));
	
			output.Write(oResult.ToString());
	
	
		}

	}


}
