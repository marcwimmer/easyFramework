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
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ComplexObjects;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efPopMenue

	//--------------------------------------------------------------------------------'
	//Module:    efPopMenue.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   pop-up menue
	//--------------------------------------------------------------------------------'
	//Created:   23.04.2004 08:36:20
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	[DefaultProperty("Text"), ToolboxData("<{0}:efPopupMenue runat=server></{0}:efPopupMenue>")]public class efPopupMenue : efBaseElement
	{
		public efPopupMenue()
		{
			moComplexPopup = new PopMenueRenderer();
		}




		//================================================================================
		//Private Fields:
		//================================================================================
		private PopMenueRenderer moComplexPopup;

		//================================================================================
		//Public Methods:
		//================================================================================
		public void gAddOption (string sUrl, bool bModal, string sAfterModalFunc, string sCaption)
		{
	
	
			moComplexPopup.gAddOption(sUrl, "", bModal, sAfterModalFunc, sCaption);
	
		}

		public void gAddSpacer ()
		{
	
			moComplexPopup.gAddSpacer();
		}

		//================================================================================
		//Protected Methods:
		//================================================================================
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
			//TODO: Call Complex-Popup-Object
			FastString oResult = new FastString();
	
	
			oResult.Append(moComplexPopup.Render(this.ID, this.Width, false));
	
			output.Write(oResult.ToString());
	
	
		}



	}


}
