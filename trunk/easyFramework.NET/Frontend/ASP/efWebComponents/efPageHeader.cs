using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP;
using easyFramework.Sys.ToolLib;
using Microsoft.VisualBasic.CompilerServices;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efPageHeader

	//--------------------------------------------------------------------------------'
	//Module:    efPageHeader.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   the contains of the <head>-area
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 09:22:53
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	[DefaultProperty("Text"), ToolboxData("<{0}:efPageHeader runat=server></{0}:efPageHeader>")]public class efPageHeader : efBaseElement
	{



		//--------------------------------------------------------------------------------'
		//Purpose:   Information of Link to a CSS-File
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 09:26:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private class CSSLink
		{
			public CSSLink()
			{
				mbGlobal = true;
			}
	
	
	
			//================================================================================
			//Private Fields:
			//================================================================================
			private bool mbGlobal;
			private string msFilename;
	
	
			//================================================================================
			//Public Properties:
			//================================================================================
	
			//================================================================================
			//Property:  bGlobal
			//--------------------------------------------------------------------------------'
			//Purpose:   global css have a certain position in the framework
			//--------------------------------------------------------------------------------'
			//Params:    -
			//--------------------------------------------------------------------------------'
			//Created:   23.03.2004 09:28:15
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public bool bGlobal
			{
				get
				{
					return mbGlobal;
				}
				set
				{
					mbGlobal = value;
				}
			}
	
	
	
	
			//================================================================================
			//Property:  sFilename
			//--------------------------------------------------------------------------------'
			//Purpose:   the filename of the script (e.g. "efStandard.css");
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   23.03.2004 09:28:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public string sFilename
			{
				get
				{
					return msFilename;
				}
				set
				{
					if (! ASPTools.Tools.gbIsRelativeFilename(value))
					{
						throw (new Exception("only relative filenames are allowed for script " + "references"));
				
					}
					msFilename = value;
				}
			}
	
	
			//================================================================================
			//Public Methods:
			//================================================================================
	
			//================================================================================
			//Sub:       Render
			//--------------------------------------------------------------------------------'
			//Purpose:   outputs the script-link
			//--------------------------------------------------------------------------------'
			//Params:    html-writer
			//--------------------------------------------------------------------------------'
			//Created:   23.03.2004 09:32:05
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public void Render (System.Web.UI.HtmlTextWriter output)
			{
				output.Write("\t" + "\t" + "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + sFilename + "\"/>" + "\n");
			}
		}





		//================================================================================
		//Private Fields:
		//================================================================================
		CSSLink[] maoCssLinks;
		

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Sub:       Render
		//--------------------------------------------------------------------------------'
		//Purpose:   renders the HTML
		//--------------------------------------------------------------------------------'
		//Params:    HTML-writer
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 09:26:04
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
	
			string sTitle = "easyFramework";
	
			if (Parent != null)
			{
				if (Parent.Page != null)
				{
					if (Parent.Page.Server != null)
					{
						if (Parent.Page is efPage) 
						{
							efPage opage = (efPage)Parent.Page;
							if (!Functions.IsEmptyString(opage.sTitle))
							{
								sTitle = opage.sTitle;
							}
						}
						output.Write("\t" + "\t" + "<title>" + Parent.Page.Server.HtmlEncode(sTitle) + "</title>" + "\n");
				
					}
				}
			}
	
	
			output.Write("\t" + "\t" + "<meta name=\"vs_targetSchema\" " + "content=\"http://schemas.microsoft.com/intellisense/ie5\">" + "\n");
	
	
	
			if (maoCssLinks != null)
			{
				for (int I = 0; I <= maoCssLinks.Length - 1; I++)
				{
					maoCssLinks[I].Render(output);
				}
			}
	
	
		}


		//================================================================================
		//Public Properties:
		//================================================================================

	



		//================================================================================
		//Sub:       AddCss
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a link to a js-script-file;
		//--------------------------------------------------------------------------------'
		//Params:    sFilename, bGlobal
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 09:40:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void AddCss (string sFilename, bool bglobal)
		{
	
			CSSLink oCssLink = new CSSLink();
			oCssLink.bGlobal = bglobal;
			oCssLink.sFilename = sFilename;
	
			if (maoCssLinks == null)
			{
				maoCssLinks = new CSSLink[1];
				maoCssLinks[0] = oCssLink;
			}
			else
			{

				CSSLink[] newArray = new CSSLink[maoCssLinks.Length + 1];
				maoCssLinks.CopyTo(newArray, 0);

				maoCssLinks = newArray;
				maoCssLinks[maoCssLinks.Length - 1] = oCssLink;

			}
	
		}






	}

}
