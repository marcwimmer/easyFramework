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
	//Class:     efTabDialog

	//--------------------------------------------------------------------------------'
	//Module:    efTabDialog.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   tab-dialog-control
	//--------------------------------------------------------------------------------'
	//Created:   13.04.2004 02:06:46
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	[DefaultProperty("ID"), ToolboxData("<{0}:efTabDialog runat=server></{0}:efTabDialog>"), ParseChildren(false)]
	public class efTabDialog : efBaseElement, INamingContainer
	{


		//================================================================================
		//Private Fields:
		//================================================================================
		//Private mTop As Unit
		//Private mLeft As Unit
		private efTab[] maoTabs;
		private string msInitialSelectedTab;

		//================================================================================
		//Public Consts:
		//================================================================================

		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  sInitialSelectedTab
		//--------------------------------------------------------------------------------'
		//Purpose:   the tab which is selected at start-up
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 23:09:08
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sInitialSelectedTab
		{
			get
			{
				return msInitialSelectedTab;
			}
			set
			{
				msInitialSelectedTab = value;
			}
		}

		//================================================================================
		//Public Events:
		//================================================================================

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       gAddTab
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a tab
		//--------------------------------------------------------------------------------'
		//Params:    the tab to add
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 02:29:28
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gAddTab (efTab oTab)
		{
			if (maoTabs == null)
			{
				maoTabs = new efTab[1];
			}
			else
			{
				efTab[] newArray = new efTab[maoTabs.Length + 1];
				maoTabs.CopyTo(newArray, 0);
				maoTabs = newArray;
			}
			maoTabs[maoTabs.Length -1] = oTab;
	
	
		}


		//================================================================================
		//Protected Properties:
		//================================================================================

		//================================================================================
		//Protected Methods:
		//================================================================================
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
	
			if (maoTabs == null)
			{
				return;
			}
	
			efHtmlDiv oHtmlDiv = new efHtmlDiv();
			oHtmlDiv["id"].sValue = this.ID;
	
			oHtmlDiv["class"].sValue = "clsTabLine";
			oHtmlDiv["style"].sValue = "position:absolute;";
			oHtmlDiv["style"].sValue += "overflow:hidden;";
	
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
			if (! BorderWidth.IsEmpty)
			{
				oHtmlDiv["style"].sValue += "border-width: " + BorderWidth.ToString() + ";";
			}
			if ( !Functions.IsEmptyString(BorderStyle.ToString()))
			{
				oHtmlDiv["style"].sValue += "border-Style: " + BorderStyle.ToString() + ";";
			}
			if (! BorderColor.IsEmpty)
			{
				oHtmlDiv["style"].sValue += "border-Color: " + BorderColor.ToString() + ";";
			}
	
	
	
	
	
			//the background-color
			efHtmlDiv oDivLine = new efHtmlDiv(oHtmlDiv);
			oDivLine["class"].sValue = "clsTabTransparent";
			oDivLine["style"].sValue = "position:absolute;" + "width:100%;left:0px;top:0;z-index:2;height:" + maoTabs[0].lTabHeight + "px;";
	
	
			//set the left-values of the tabs:
			int iNextLeft = 0;
			for (int i = 0; i <= maoTabs.Length - 1; i++)
			{
				maoTabs[i].oTabLeft = Unit.Pixel(iNextLeft);
		
				string sWidth = maoTabs[i].oTabWidth.ToString();
				sWidth = Functions.Replace(Functions.LCase(sWidth), "px", "");
				if (! Functions.IsNumeric(sWidth) & !Functions.IsEmptyString(sWidth))
				{
					throw (new Exception("For widths of the tabs only \"px\"-values are allowed! " + "Invalid value: " + maoTabs[i].Width.ToString()));
				}
		
				iNextLeft += easyFramework.Sys.ToolLib.DataConversion.glCInt(sWidth, 0);
			}
	
			//render the tabs:
			if ( maoTabs != null)
			{
				for (int i = 0; i <= maoTabs.Length - 1; i++)
				{
					maoTabs[i].gRender(oHtmlDiv, this.ID, maoTabs[i].lTabHeight + 5);
				}
			}
	
	
	
			//init-javascript:
			efHtmlScript oScript = new efHtmlScript();
			oScript.sCode = "if (gInitTabDialog == null) alert(\"efTabDialog.js muss eingebunden sein!\");" + "\n";
	
	
			oScript.sCode += "gInitTabDialog('" + this.ID + "');" + "\n";
			for (int i = 0; i <= maoTabs.Length - 1; i++)
			{
		
				if (Functions.IsEmptyString(maoTabs[i].ID))
				{
					throw (new efException("Each Tab requires an ID! Not defined for tab with caption " + "\"" + maoTabs[i].sCaption + "\""));
			
				}
		
				oScript.sCode += "gAppendTab('" + ID + "', '" + maoTabs[i].ID + "'," + "'" + maoTabs[i].sUrl + "'," + "'" + maoTabs[i].sXmlDialogDefinitionFile + "'," + "'" + maoTabs[i].sXmlDialogDataPage + "'," + "'" + maoTabs[i].sXmlDialogFormName + "'" + ");" + "\n";
			}
	
			//select the first-tab:
			if (!Functions.IsEmptyString(msInitialSelectedTab))
			{
				oScript.sCode += "gHandleTabItemClick('" + this.ID + "','" + msInitialSelectedTab + "');" + "\n";
			}
	
	
			oHtmlDiv.gRender(output, 1);
			oScript.gRender(output, 1);
	
		}


		//================================================================================
		//Sub:       CreateChildControls
		//--------------------------------------------------------------------------------'
		//Purpose:   search in parent for all tabs, which have me as parent and add
		//           them
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 11:04:09
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		[System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]protected override void CreateChildControls ()
		{
	
			for (int i = 0; i <= Parent.Controls.Count - 1; i++)
			{
		
				if ((Parent.Controls[i]) is efTab)
				{
			
					this.gAddTab((efTab)(Parent.Controls[i]));
			
				}
		
			}
	
		} //CreateChildControls


		//================================================================================
		//Private Consts:
		//================================================================================

		//================================================================================
		//Private Fields:
		//================================================================================

	}
}
