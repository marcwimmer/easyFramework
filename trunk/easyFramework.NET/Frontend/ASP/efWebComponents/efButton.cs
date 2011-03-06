using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.HTMLRenderer;
//using easyFramework.sys.ToolLib.Functions;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efButton
	//--------------------------------------------------------------------------------'
	//Module:    efButton.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   button-control-element
	//--------------------------------------------------------------------------------'
	//Created:   24.03.2004 15:23:16
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	[DefaultProperty("Text"), ToolboxData("<{0}:efButton runat=server></{0}:efButton>")]public class efButton : System.Web.UI.WebControls.WebControl
	{


		//================================================================================
		//Private Fields:
		//================================================================================
		string msOnClick;
		string mstext;

		//================================================================================
		//Public Consts:
		//================================================================================
		public const string efsCssDefault = "cmdButton";


		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  sOnClick
		//--------------------------------------------------------------------------------'
		//Purpose:   the on-click event
		//--------------------------------------------------------------------------------'
		//Params:    '
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 15:24:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string @sOnClick
		{
			get
			{
				return msOnClick;
			}
	
			set
			{
				msOnClick = value;
			}
		}

		//================================================================================
		//Property:  [sText]
		//--------------------------------------------------------------------------------'
		//Purpose:   the button-caption
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 15:24:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string @sText
		{
			get
			{
				return mstext;
			}
	
			set
			{
				mstext = value;
			}
		}





		//================================================================================
		//Public Methods:
		//================================================================================


		//================================================================================
		//Sub:       RenderBeginTag
		//--------------------------------------------------------------------------------'
		//Purpose:   renders button element by using then htmle-element-class
		//--------------------------------------------------------------------------------'
		//Params:    html-writer
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 15:22:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override void RenderBeginTag (System.Web.UI.HtmlTextWriter writer)
		{
			efHtmlButton oHtmlButton = new efHtmlButton();
			FastString oS = new FastString();
	
			oHtmlButton["onClick"].sValue = sOnClick;
			if (Functions.IsEmptyString(CssClass))
			{
				oHtmlButton["class"].sValue = efsCssDefault;
			}
			else
			{
				oHtmlButton["class"].sValue = CssClass;
			}
	
			oHtmlButton["type"].sValue = "button";
	
			//no submit-button per default; is important for netscape; ie doesn't matter:
			oHtmlButton["type"].sValue = "button";
	
	
			//-------if there is a $-sign, then replace it---------
			string sAccessKey = "";
			if (Functions.InStr(sText, "$") > 0)
			{
                sAccessKey = Functions.Mid(sText, Functions.InStr(sText, "$") + 1, 1);
                sText = Functions.Left(sText, Functions.InStr(sText, "$") - 1) + "<u>" + sAccessKey + "</u>" + Functions.Right(sText, Functions.Len(sText) - Functions.InStr(sText, "$") - 1);
			}
	
	
			oHtmlButton.sText = this.sText;
	
			if (!Functions.IsEmptyString(sAccessKey))
			{
				oHtmlButton["AccessKey"].sValue = sAccessKey;
			}
	
	
			oHtmlButton.gRender(oS, 1);
			writer.Write(oS);
	
	
		}

		//================================================================================
		//Sub:       RenderEndTag
		//--------------------------------------------------------------------------------'
		//Purpose:   renders button element by using then htmle-element-class
		//--------------------------------------------------------------------------------'
		//Params:    html-writer
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 15:22:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public override void RenderEndTag (System.Web.UI.HtmlTextWriter writer)
		{
			//nothing
	
		}
	}

}
