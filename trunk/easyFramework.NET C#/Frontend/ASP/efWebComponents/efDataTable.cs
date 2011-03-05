using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using easyFramework.Sys.Data;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Sys;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efDataTable

	//--------------------------------------------------------------------------------'
	//Module:    efDataTable.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   display-control for data-rows;
	//           item can be clicked - event is raised and the item is displayed
	//           as selected
	//--------------------------------------------------------------------------------'
	//Created:   05.04.2004 23:50:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	[DefaultProperty("Text"), ToolboxData("<{0}:efDataTable runat=server></{0}:efDataTable>")]public class efDataTable : efBaseElement
	{
		public efDataTable()
		{
			mlRowsPerPage = 20;
		}


		//================================================================================
		//Private Fields:
		//================================================================================
		string msOnItemClick;
		string msOnItemDblClick;
		string msOnAfterInit;
		string msDataPage;
		int mlRowsPerPage;
		string msXmlAddParams;


		//================================================================================
		//Public Consts:
		//================================================================================

		//================================================================================
		//Public Properties:
		//================================================================================



		//================================================================================
		//Property:  sOnItemClick
		//--------------------------------------------------------------------------------'
		//Purpose:   the data-page (ASPX-File)
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 00:50:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sDataPage
		{
			get
			{
				return msDataPage;
			}
	
			set
			{
				msDataPage = value;
			}
		}



		//================================================================================
		//Property:  sAddParams
		//--------------------------------------------------------------------------------'
		//Purpose:   additional-params, which are given to the data-load page;
		//           must be xml-code
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 13:03:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sXmlAddParams
		{
			get
			{
				return msXmlAddParams;
			}
	
			set
			{
				msXmlAddParams = value;
			}
		}
		//================================================================================
		//Property:  lRowsPerPage
		//--------------------------------------------------------------------------------'
		//Purpose:   the lines per page
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 12:02:42
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public int lRowsPerPage
		{
			get
			{
				return mlRowsPerPage;
			}
	
			set
			{
				mlRowsPerPage = value;
			}
		}
		//================================================================================
		//Property:  sOnAfterInit
		//--------------------------------------------------------------------------------'
		//Purpose:   the javascript-event which is invoked, when the table is initialized
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   30.04.2004 23:51:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sOnAfterInit
		{
			get
			{
				return msOnAfterInit;
			}
	
			set
			{
				msOnAfterInit = value;
			}
		}


		//================================================================================
		//Property:  sOnClick
		//--------------------------------------------------------------------------------'
		//Purpose:   the javascript-event which is invoked, when an item is clicked
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 23:51:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sOnItemClick
		{
			get
			{
				return msOnItemClick;
			}
	
			set
			{
				msOnItemClick = value;
			}
		}

		//================================================================================
		//Property:  sOnDblClick
		//--------------------------------------------------------------------------------'
		//Purpose:   the javascript-event which is invoked, when an item is double-clicked
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.04.2004 23:51:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sOnItemDblClick
		{
			get
			{
				return msOnItemDblClick;
			}
	
			set
			{
				msOnItemDblClick = value;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Sub:       Render
		//--------------------------------------------------------------------------------'
		//Purpose:   build the html-output
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 00:02:36
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
	
	
	
			efHtmlDiv oHtmlDiv = new efHtmlDiv();
			efHtmlDiv oHtmlDivTable = new efHtmlDiv(oHtmlDiv);
			efHtmlDiv oHtmlDivButtons = new efHtmlDiv(oHtmlDiv);
			efHtmlDiv ohtmldivInfoText = new efHtmlDiv(oHtmlDiv); //"Datensatz 1 von 2 Seite 3" etc.
	
	
			oHtmlDiv["style"].sValue = "position:absolute;";
	
			if (! Top.IsEmpty)
			{
				oHtmlDiv["style"].sValue += "top:" + Top.ToString() + ";";
			}
			if (! Left.IsEmpty)
			{
				oHtmlDiv["style"].sValue += "left:" + Left.ToString() + ";";
			}
			if (! Width.IsEmpty)
			{
				oHtmlDiv["style"].sValue += "width:" + Width.ToString() + ";";
			}
			if (! Height.IsEmpty)
			{
				oHtmlDiv["style"].sValue += "height: " + Height.ToString() + ";";
			}
	
			oHtmlDiv["style"].sValue += "overflow: hidden";
	
			if (Functions.IsEmptyString(this.CssClass))
			{
				oHtmlDiv["class"].sValue = "dataTableContainer";
			}
			else
			{
				oHtmlDiv["class"].sValue = this.CssClass;
			}
	
	
	
			efHtmlScript oHtmlScript = new efHtmlScript();
	
			string sVarNameOldBodyLoadEvent = easyFramework.Frontend.ASP.ASPTools.Tools.sGetUniqueString(true, 32);
			string sFunctionNameOnLoad = easyFramework.Frontend.ASP.ASPTools.Tools.sGetUniqueString(true, 32);
			string sVarName_OldColor = easyFramework.Frontend.ASP.ASPTools.Tools.sGetUniqueString(true, 32);
			string sVarName_OldBackground = easyFramework.Frontend.ASP.ASPTools.Tools.sGetUniqueString(true, 32);
	
			oHtmlScript.sCode = "   var " + sVarNameOldBodyLoadEvent + " = window.onload;" + "\n" + "   window.onload = " + sFunctionNameOnLoad + "; " + "\n" + "   function " + sFunctionNameOnLoad + "() {" + "\n" + "           fillDataTableRows('" + this.ID + "','" + this.sDataPage + "',null," + this.lRowsPerPage + ",'" + msOnItemClick + "'," + "'" + this.msXmlAddParams + "','" + msOnItemDblClick + "'," + "'" + msOnAfterInit + "'" + "); " + "\n" + "       if (" + sVarNameOldBodyLoadEvent + " != null) {" + "\n" + "           " + sVarNameOldBodyLoadEvent + "();}" + "\n" + "   }" + "\n";
	
	
	
			ClientInfo oClientInfo = ((efPage)(Parent.Page)).oClientInfo;
	
			//position table-div
			oHtmlDivTable["id"].sValue = this.ID + "_table";
			oHtmlDivTable["class"].sValue = "dataTable";
			oHtmlDivTable["style"].sValue = "position:absolute;top:0%;left:0;width:100%;height:90%;overflow:" 
				+ efHtmlConsts.Overflow(this.Overflow) + ";";
	
			//position infotext-div:
			ohtmldivInfoText["id"].sValue = this.ID + "_infotext";
			ohtmldivInfoText["class"].sValue = "dataTableInfoText";
			ohtmldivInfoText["style"].sValue = "position:absolute;top:90%;left:0;width:60%;height:10%;overflow:hidden;";
	
			//position button-div:
			oHtmlDivButtons["align"].sValue = "right";
			oHtmlDivButtons["id"].sValue = this.ID + "_buttons";
			oHtmlDivButtons["class"].sValue = "dataTableButtons";
			oHtmlDivButtons["style"].sValue = "position:absolute;top:90%;left:60%;width:40%;height:10%;overflow:hidden;";
	
	
			Images oImage = new Images();
	
			efHtmlImg oHtmlImg;
	
			//first-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_first", this.Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_first_down", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_first", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "gDataTableOnButtonClick('" + this.ID + "','first');";
			oHtmlImg["id"].sValue = this.ID + "_button_first";
	
			//prev-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_prev", this.Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_prev_down", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_prev", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "gDataTableOnButtonClick('" + this.ID + "','prev');";
			oHtmlImg["id"].sValue = this.ID + "_button_prev";
	
			//next-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_next", this.Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_next_down", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_next", this.Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "gDataTableOnButtonClick('" + this.ID + "','next');";
			oHtmlImg["id"].sValue = this.ID + "_button_next";
	
	
			//last-button:
			oHtmlImg = new efHtmlImg(oHtmlDivButtons);
			oHtmlImg["src"].sValue = Images.sGetImageURL(oClientInfo, "button_last", Parent.Page.Request.ApplicationPath);
			oHtmlImg["onmousedown"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_last_down", Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseup"].sValue = "this.src='" + Images.sGetImageURL(oClientInfo, "button_last", Parent.Page.Request.ApplicationPath) + "'";
			oHtmlImg["onmouseout"].sValue = oHtmlImg["onmouseup"].sValue;
			oHtmlImg["onclick"].sValue = "gDataTableOnButtonClick('" + this.ID + "','last');";
			oHtmlImg["id"].sValue = this.ID + "_button_last";
	
	
	
			oHtmlDiv.gRender(output, 1);
			oHtmlScript.gRender(output, 1);
	
	
		}
		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================





	}

}
