using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efTab

	//--------------------------------------------------------------------------------'
	//Module:    efTa.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   tab-control (for tab-dialogs)
	//--------------------------------------------------------------------------------'
	//Created:   13.04.2004 02:06:46
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Class:     efTab

	//--------------------------------------------------------------------------------'
	//Module:    efTabDialog.vb
	//--------------------------------------------------------------------------------'
	//Purpose:   a tab of the dialog
	//--------------------------------------------------------------------------------'
	//Created:   13.04.2004 10:23:00
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	[DefaultProperty("ID"), ToolboxData("<{0}:efTab runat=server></{0}:efTab>"), ParseChildren(false)]public class efTab : System.Web.UI.WebControls.Panel, INamingContainer
	{

		//================================================================================
		//Private Fields:
		//================================================================================
		private string msCaption;
		private Unit moTabLeft;
		private Unit moTabWidth;
		private Unit moTabHeight;
		private efTabDialog moDesignTimeParent;
		private string msXmlDialogDefinitionFile;
		private string msXmlDataPage;
		private string msXmlDialogFormName;
		private string msUrl;

		//================================================================================
		//Public Consts:
		//================================================================================

		//================================================================================
		//Public Properties:
		//================================================================================
		[Bindable(true), Category("Appearance")]public efTabDialog oParent
		{
			get
			{
				return moDesignTimeParent;
			}
			set
			{
				moDesignTimeParent = value;
			}
		}

		//================================================================================
		//Property:  sCaption
		//--------------------------------------------------------------------------------'
		//Purpose:   the caption of the tab
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]public string sCaption
		{
			get
			{
				return msCaption;
			}
			set
			{
				msCaption = value;
			}
		}
		//================================================================================
		//Property:  sXmlFormName
		//--------------------------------------------------------------------------------'
		//Purpose:   name of the html-form
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]public string sXmlDialogFormName
		{
			get
			{
				return msXmlDialogFormName;
			}
			set
			{
				msXmlDialogFormName = value;
			}
		}
		//================================================================================
		//Property:  sXmlDialog
		//--------------------------------------------------------------------------------'
		//Purpose:   if the content is an xml-dialog, then this is the xml-dialog
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]public string sXmlDialogDefinitionFile
		{
			get
			{
				return msXmlDialogDefinitionFile;
			}
			set
			{
				msXmlDialogDefinitionFile = value;
			}
		}
		//================================================================================
		//Property:  sXmlDialogDataPage
		//--------------------------------------------------------------------------------'
		//Purpose:   this is the data-page of an xml-dialog
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]public string sXmlDialogDataPage
		{
			get
			{
				return msXmlDataPage;
			}
			set
			{
				msXmlDataPage = value;
			}
		}

		//================================================================================
		//Property:  sUrl
		//--------------------------------------------------------------------------------'
		//Purpose:   if the content is from a url, this is the url
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]public string sUrl
		{
			get
			{
				return msUrl;
			}
			set
			{
				msUrl = value;
			}
		}
		//================================================================================
		//Property:  oTabLeft
		//--------------------------------------------------------------------------------'
		//Purpose:   the left-property of the head of the tab
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:59:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]public Unit oTabLeft
		{
			get
			{
				return moTabLeft;
			}
			set
			{
				moTabLeft = value;
			}
		}

		//================================================================================
		//Property:  lTabHeight
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the height as integer pixels
		//--------------------------------------------------------------------------------'
		//Params:    pixels
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:59:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public int lTabHeight
		{
			get
			{
				string sReplaced = Functions.Replace(Functions.LCase(this.oTabHeight.ToString()), "px", "");
				if (! Functions.IsNumeric(sReplaced) & !Functions.IsEmptyString(sReplaced))
				{
					throw (new Exception("Cannot get pixel-height, because height-value of " + "tab is not pixel: " + this.oTabHeight.ToString()));
				}
		
				return easyFramework.Sys.ToolLib.DataConversion.glCInt(sReplaced, 0);
			}
		}

		//================================================================================
		//Property:  oTabWidth
		//--------------------------------------------------------------------------------'
		//Purpose:   the width of the tab
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]
		public Unit oTabWidth
		{
			get
			{
				if (moTabWidth.IsEmpty)
				{
					return Unit.Pixel(80);
				}
				else
				{
					return moTabWidth;
				}
		
			}
			set
			{
				moTabWidth = value;
			}
		}
		//================================================================================
		//Property:  oTabHeight
		//--------------------------------------------------------------------------------'
		//Purpose:   the height of the tab
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:21:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance")]
		public Unit oTabHeight
		{
			get
			{
				if (moTabHeight.IsEmpty)
				{
					return Unit.Pixel(20);
				}
				else
				{
					return moTabHeight;
				}
		
			}
			set
			{
				moTabHeight = value;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================
		//================================================================================
		//Sub:       gRender
		//--------------------------------------------------------------------------------'
		//Purpose:   renders the tab
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:44:05
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gRender (efHtmlDiv oParentDiv, string sParentID, int lTopPixel)
		{
	
			//render the top-element:
			efHtmlDiv oDiv = new efHtmlDiv(oParentDiv);
			oDiv["id"].sValue = ID + "_tab";
			oDiv["class"].sValue = "clsTab";
			oDiv["style"].sValue = "position:absolute; " + "height:" + oTabHeight.ToString() + "; " + "width:" + oTabWidth.ToString() + ";" + "left:" + oTabLeft.ToString() + ";" + "top:0px;" + "z-index:5;";
			efHtmlA oHtmlA = new efHtmlA(oDiv);
			oHtmlA["class"].sValue = "clsTabText";
			oHtmlA["id"].sValue = ID + "_captiontext";
			oHtmlA["href"].sValue = "javascript:gHandleTabItemClick('" + sParentID + "','" + ID + "'); ";
			efHtmlTextNode oText = new efHtmlTextNode(oHtmlA);
			oText.sText = sCaption;
	
			//click on the div shall be the same like clicking on the label:
			oDiv["onclick"].sValue = oHtmlA["href"].sValue;
	
			//render the content:
			efHtmlDiv oDivContent = new efHtmlDiv(oParentDiv);
			oDivContent["id"].sValue = ID + "_content";
			oDivContent["style"].sValue = "position:absolute; " + "height:98%; " + "width:100%;" + "left:0px;" + "top:" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lTopPixel) + "px;" + "overflow:scroll;" + "visibility:hidden;";
	
	
			System.Text.StringBuilder oSB = new System.Text.StringBuilder();
			System.Web.UI.HtmlTextWriter output;
			System.IO.TextWriter writer = new System.IO.StringWriter(oSB);
			output = new System.Web.UI.HtmlTextWriter(writer);
	
	
			if (Functions.IsEmptyString(msUrl) & Functions.IsEmptyString(msXmlDialogDefinitionFile))
			{
				this.RenderChildren(output);
				efHtmlUnparsedHtml oHmtlUnparsed = new efHtmlUnparsedHtml(oDivContent);
				oHmtlUnparsed.sHtml = oSB.ToString();
		
			}
			else
			{
		
				//create a function, which is on a refresh called;
				//this function fills the content of the tab-item:
		
		
		
			}
	
	
	
	
	
		}
		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Sub:       Render
		//--------------------------------------------------------------------------------'
		//Purpose:   do nothing, because inherited from panel
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   14.04.2004 00:09:46
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void Render (System.Web.UI.HtmlTextWriter writer)
		{
			//do nothing
		}


		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================


	}


}
