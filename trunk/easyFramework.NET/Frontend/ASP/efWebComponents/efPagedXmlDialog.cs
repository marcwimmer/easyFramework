using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using easyFramework.Sys.Data;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.Dialog;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efPagedXmlDialog

	//--------------------------------------------------------------------------------'
	//Module:    efPagedXmlDialog.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   pages an xml-dialog, so that a lot of data is parted into
	//           several pages
	//           with first, last, next, prev button
	//--------------------------------------------------------------------------------'
	//Created:   26.05.2004 07:31:42
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	//================================================================================
	//Imports:
	//================================================================================


	[DefaultProperty("Text"), ToolboxData("<{0}:efPagedXmlDialog runat=server></{0}:efPagedXmlDialog>")]public class efPagedXmlDialog : System.Web.UI.WebControls.WebControl
	{
		public efPagedXmlDialog()
		{
			moXmlDialog = new efXmlDialog();
		}


		//================================================================================
		//Private Fields:
		//================================================================================

		efXmlDialog moXmlDialog;
		string msAddParams;
		string msOnSaveChanges;


		//================================================================================
		//Public Properties:
		//================================================================================


		//================================================================================
		//Property:  sOnSaveChanges
		//--------------------------------------------------------------------------------'
		//Purpose:   this javascript-function is called, when the user is asked,
		//           if he wants to save the changes
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   26.05.2004 23:07:17
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sOnSaveChanges
		{
			get
			{
				return msOnSaveChanges;
			}
			set
			{
				msOnSaveChanges = value;
			}
		}


		//================================================================================
		//Property:  sAddParams
		//--------------------------------------------------------------------------------'
		//Purpose:   additional parameters which are given to the datapage;
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   26.05.2004 13:22:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sAddParams
		{
			get
			{
				return msAddParams;
			}
			set
			{
				msAddParams = value;
			}
		}

		//================================================================================
		//Property:  oXmlDialog
		//--------------------------------------------------------------------------------'
		//Purpose:   the xml-dialog, which is displayed
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   26.05.2004 07:39:57
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efXmlDialog oXmlDialog
		{
			get
			{
				return moXmlDialog;
			}
			set
			{
				moXmlDialog = value;
			}
		}


		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================

		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
	
	
			efHtmlImg oHtmlImg;
			Images oImage = new Images();
			oXmlDialog.ID = this.ID + "_xmldialog";
	
			ClientInfo oClientInfo = ((efPage)(Parent.Page)).oClientInfo;
	
			efHtmlDiv oHtmlDivButtons = new efHtmlDiv();
			oHtmlDivButtons["align"].sValue = "right";
	
			//-----------__PAGE-field and XmlPagedDialogRecordcount-field
			efHtmlInput oHtmlInputPage = new efHtmlInput();
			efHtmlInput oHtmlInputRecordcount = new efHtmlInput();
			oHtmlInputRecordcount["name"].sValue = "XmlPagedDialogRecordcount";
			oHtmlInputRecordcount["type"].sValue = "hidden";
			oHtmlInputPage["name"].sValue = "__Page";
			oHtmlInputPage["type"].sValue = "hidden";
			oHtmlInputPage["value"].sValue = "1";
	
			//first-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_first", this.Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_first_down", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_first", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "PXML_gPagedXmlDialogClick('" + this.ID + "','first');";
			oHtmlImg["id"].sValue = this.ID + "_button_first";
	
			//prev-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_prev", this.Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_prev_down", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_prev", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "PXML_gPagedXmlDialogClick('" + this.ID + "','prev');";
			oHtmlImg["id"].sValue = this.ID + "_button_prev";
	
			//next-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_next", this.Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_next_down", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_next", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "PXML_gPagedXmlDialogClick('" + this.ID + "','next');";
			oHtmlImg["id"].sValue = this.ID + "_button_next";
	
	
			//last-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_last", Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_last_down", Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_last", Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "PXML_gPagedXmlDialogClick('" + this.ID + "','last');";
			oHtmlImg["id"].sValue = this.ID + "_button_last";
	
			//-----------javascript for first fecth--------
			efHtmlScript oScript = new efHtmlScript();
			oScript.sCode += "PXML_registerPagedDialog('" + this.ID + "','" + this.sAddParams + "');";
			if (!Functions.IsEmptyString(msOnSaveChanges))
			{
				oScript.sCode += "PXML_goGetDialog('" + this.ID + "').sOnSaveChanges=" + msOnSaveChanges + ";";
			}
			oScript.sCode += "PXML_goGetDialog('" + this.ID + "').fetchData();";
	
	
	
			//---------------render------------
			oHtmlDivButtons.gRender(output, 1);
			oHtmlInputPage.gRender(output, 1);
			oHtmlInputRecordcount.gRender(output, 1);
			oXmlDialog.Render(output, this.Parent);
			oHtmlDivButtons.gRender(output, 1);
			oScript.gRender(output, 1);
	
		}

	}

}
