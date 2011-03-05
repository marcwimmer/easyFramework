using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efTreeview

	//--------------------------------------------------------------------------------'
	//Module:    efTreeview.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2002
	//--------------------------------------------------------------------------------'
	//Purpose:   treeview web-control
	//           users /js/efTreeview.js
	//                 /images/treeview
	//--------------------------------------------------------------------------------'
	//Created:   01.04.2004 21:13:36
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================


	[DefaultProperty("Text"), ToolboxData("<{0}:efTreeview runat=server></{0}:efTreeview>")]public class efTreeview : efBaseElement
	{
		public efTreeview()
		{
			mlItemHeight = 20;
		}


		string msDataPage;
		string msServerParams;
		int mlItemHeight;

		//system-images:
		string msImage_Clear;
		string msImage_f;
		string msImage_fminus;
		string msImage_fplus;
		string msImage_i;
		string msImage_l;
		string msImage_lminus;
		string msImage_lplus;
		string msImage_r;
		string msImage_rminus;
		string msImage_rplus;
		string msImage_t;
		string msImage_tminus;
		string msImage_tplus;
		string msOverflow = "hidden";
		string msSelectBackgroundColor = "AAC4E6";

		//================================================================================
		//Public Properties:
		//================================================================================
		[Bindable(true), Category("Appearance"), DefaultValue("hidden")]public string sOverflow
		{
			get
			{
				return msOverflow;
			}
	
			set
			{
				msOverflow = value;
			}
		}


		/*	
		<summary>
		Background-Color of selected node.
		</summary>
		<remarks>
		No important notes.
		</remarks>
		<seealso>
		<see cref="x.x"/>
		</seealso>
		<created date=6/2/2005" username="Marc Wimmer" userEmail="mwimmer@promain-software.de"/>
		
		*/
		[Bindable(true), Category("Appearance"), DefaultValue("hidden")]public string sSelectBackgroundColor
		{
			get
			{
				return msSelectBackgroundColor;
			}
	
			set
			{
				msSelectBackgroundColor = value;
			}
		}
		//================================================================================
		//Property:  [sDataPage]
		//--------------------------------------------------------------------------------'
		//Purpose:   the aspx-page, which provides the data for this control;
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
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
		//Property:  [sServerParams]
		//--------------------------------------------------------------------------------'
		//Purpose:   additional parameters, which are given to the datapge
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sServerParams
		{
			get
			{
				return msServerParams;
			}
	
			set
			{
				msServerParams = value;
			}
		}

		//================================================================================
		//Property:  [lItemHeight]
		//--------------------------------------------------------------------------------'
		//Purpose:   the height of an item-row of the treeview
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public int lItemHeight
		{
			get
			{
				return this.mlItemHeight;
			}
	
			set
			{
				mlItemHeight = value;
			}
		}
		//================================================================================
		//Property:  [sImage_Clear]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_Clear
		{
			get
			{
				return msImage_Clear;
			}
	
			set
			{
				msImage_Clear = value;
			}
		}

		//================================================================================
		//Property:  [sImage_f]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_f
		{
			get
			{
				return msImage_f;
			}
	
			set
			{
				msImage_f = value;
			}
		}

		//================================================================================
		//Property:  [sImage_fminus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_fminus
		{
			get
			{
				return msImage_fminus;
			}
	
			set
			{
				msImage_fminus = value;
			}
		}

		//================================================================================
		//Property:  [sImage_fplus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_fplus
		{
			get
			{
				return msImage_fplus;
			}
	
			set
			{
				msImage_fplus = value;
			}
		}

		//================================================================================
		//Property:  [sImage_i]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_i
		{
			get
			{
				return msImage_i;
			}
	
			set
			{
				msImage_i = value;
			}
		}

		//================================================================================
		//Property:  [sImage_l]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_l
		{
			get
			{
				return msImage_l;
			}
	
			set
			{
				msImage_l = value;
			}
		}

		//================================================================================
		//Property:  [sImage_lminus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_lminus
		{
			get
			{
				return msImage_lminus;
			}
	
			set
			{
				msImage_lminus = value;
			}
		}

		//================================================================================
		//Property:  [sImage_lplus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_lplus
		{
			get
			{
				return msImage_lplus;
			}
	
			set
			{
				msImage_lplus = value;
			}
		}



		//================================================================================
		//Property:  [sImage_r]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_r
		{
			get
			{
				return msImage_r;
			}
	
			set
			{
				msImage_r = value;
			}
		}


		//================================================================================
		//Property:  [sImage_rminus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_rminus
		{
			get
			{
				return msImage_rminus;
			}
	
			set
			{
				msImage_rminus = value;
			}
		}

		//================================================================================
		//Property:  [sImage_rplus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_rplus
		{
			get
			{
				return msImage_rplus;
			}
	
			set
			{
				msImage_rplus = value;
			}
		}

		//================================================================================
		//Property:  [sImage_t]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_t
		{
			get
			{
				return msImage_t;
			}
	
			set
			{
				msImage_t = value;
			}
		}

		//================================================================================
		//Property:  [sImage_tminus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_tminus
		{
			get
			{
				return msImage_tminus;
			}
	
			set
			{
				msImage_tminus = value;
			}
		}

		//================================================================================
		//Property:  [sImage_tplus]
		//--------------------------------------------------------------------------------'
		//Purpose:   for displaying the tree
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 21:19:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sImage_tplus
		{
			get
			{
				return msImage_tplus;
			}
	
			set
			{
				msImage_tplus = value;
			}
		}


		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       gSetDefaultSystemIcons
		//--------------------------------------------------------------------------------'
		//Purpose:   sets the default-system-images
		//--------------------------------------------------------------------------------'
		//Params:    the image-object
		//           the relative path of the web-application
		//--------------------------------------------------------------------------------'
		//Created:   01.04.2004 23:30:09
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gSetDefaultSystemIcons (ClientInfo oClientInfo, string sWebPageRoot)
		{
	
	
			this.sImage_Clear = Images.sGetImageURL(oClientInfo, "treeview_clear", sWebPageRoot);
			this.sImage_f = Images.sGetImageURL(oClientInfo, "treeview_f", sWebPageRoot);
			this.sImage_fminus = Images.sGetImageURL(oClientInfo, "treeview_fminus", sWebPageRoot);
			this.sImage_fplus = Images.sGetImageURL(oClientInfo, "treeview_fplus", sWebPageRoot);
			this.sImage_i = Images.sGetImageURL(oClientInfo, "treeview_i", sWebPageRoot);
			this.sImage_l = Images.sGetImageURL(oClientInfo, "treeview_l", sWebPageRoot);
			this.sImage_lminus = Images.sGetImageURL(oClientInfo, "treeview_lminus", sWebPageRoot);
			this.sImage_lplus = Images.sGetImageURL(oClientInfo, "treeview_lplus", sWebPageRoot);
			this.sImage_r = Images.sGetImageURL(oClientInfo, "treeview_r", sWebPageRoot);
			this.sImage_rminus = Images.sGetImageURL(oClientInfo, "treeview_rminus", sWebPageRoot);
			this.sImage_rplus = Images.sGetImageURL(oClientInfo, "treeview_rplus", sWebPageRoot);
			this.sImage_t = Images.sGetImageURL(oClientInfo, "treeview_t", sWebPageRoot);
			this.sImage_tminus = Images.sGetImageURL(oClientInfo, "treeview_tminus", sWebPageRoot);
			this.sImage_tplus = Images.sGetImageURL(oClientInfo, "treeview_tplus", sWebPageRoot);
	
		}

		//================================================================================
		//Protected Methods:
		//================================================================================

		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
	
			efHtmlDiv oHtmlDiv = new efHtmlDiv();
			oHtmlDiv["id"].sValue = this.ID;
	
			oHtmlDiv["style"].sValue = "position:absolute; overflow: " + msOverflow  + "; left: " + DataConversion.gsCStr(Left) + "; top: " + DataConversion.gsCStr(Top) + "; width: " + DataConversion.gsCStr(Width) + "; height: " + DataConversion.gsCStr(Height) + ";";
	
			oHtmlDiv["style"].sValue += "border-width:" + BorderWidth.ToString() + ";" + "border-style: " + BorderStyle.ToString() + ";" + "border-color:" + BorderColor.ToString() + ";";
	
	
	
	
			efHtmlScript oHtmlScript = new efHtmlScript();
	
			oHtmlScript.sCode = "loadTreeView(\"" + this.ID + "\", \"" + this.sDataPage + "\", \"" + this.msServerParams + "\", " + "\"" + msImage_i + "\", " + "\"" + msImage_r + "\", " + "\"" + msImage_l + "\", " + "\"" + msImage_lminus + "\", " + "\"" + msImage_lplus + "\", " + "\"" + msImage_t + "\", " + "\"" + msImage_tminus + "\", " + "\"" + msImage_tplus + "\", " + "\"" + msImage_Clear + "\", " + "" + DataConversion.gsCStr(mlItemHeight) + ", \"" + msSelectBackgroundColor + "\"); ";
	
			oHtmlDiv.gRender(output, 1);
			oHtmlScript.gRender(output, 1);
	
		}

	}

}
