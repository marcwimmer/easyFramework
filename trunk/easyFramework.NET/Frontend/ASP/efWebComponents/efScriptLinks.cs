using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.HTMLRenderer;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
	//Class:     efScriptLinks

	//--------------------------------------------------------------------------------'
	//Module:    efScriptLinks.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   contains links to js-scripts
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 09:47:52
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	[DefaultProperty("Text"), ToolboxData("<{0}:efScriptLinks runat=server></{0}:efScriptLinks>")]public class efScriptLinks : System.Web.UI.WebControls.WebControl
	{


		//================================================================================
		//Private Fields:
		//================================================================================
		private ScriptLink[] maoScriptLinks;


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
			if (maoScriptLinks != null)
			{
				for (int i = 0; i <= maoScriptLinks.Length - 1; i++)
				{
					maoScriptLinks[i].Render(output);
				}
			}
		}

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       AddScriptLink
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a link to a js-script-file
		//--------------------------------------------------------------------------------'
		//Params:    sFilename, bGlobal
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 09:40:55
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void AddScriptLink (string sFilename, bool bGlobal, string sLanguage)
		{
	
			ScriptLink oScriptLink = new ScriptLink();
			oScriptLink.bGlobal = bGlobal;
			oScriptLink.sFilename = sFilename;
			oScriptLink.sLanguage = sLanguage;
	
			if (maoScriptLinks == null)
			{
				maoScriptLinks = new ScriptLink[1];
				maoScriptLinks[0] = oScriptLink;
			}
			else
			{

				ScriptLink[] newArray = new ScriptLink[maoScriptLinks.Length + 1];
				maoScriptLinks.CopyTo(newArray, 0);
				maoScriptLinks = newArray;

				newArray[newArray.Length - 1] = oScriptLink;

			}
	
		}
		//================================================================================
		//Private Classes:
		//================================================================================
		//================================================================================
		//Class:     ScriptLink
		//--------------------------------------------------------------------------------'
		//Purpose:   Information of Link to a JScript-File
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 09:26:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private class ScriptLink
		{
			public ScriptLink()
			{
				mbGlobal = true;
			}
	
	
	
			//================================================================================
			//Private Fields:
			//================================================================================
			private bool mbGlobal;
			private string msFilename;
			private string msLanguage;
	
	
			//================================================================================
			//Public Properties:
			//================================================================================
	
			//================================================================================
			//Property:  bGlobal
			//--------------------------------------------------------------------------------'
			//Purpose:   global scripts have a certain position in the framework
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
			//Purpose:   the filename of the script (e.g. "efWord.js");
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
					if (! Tools.gbIsRelativeFilename(value))
					{
						throw (new Exception("only relative filenames are allowed for script " + "references"));
				
					}
			
					msFilename = value;
				}
			}

			//================================================================================
			//Property:  sLanguage
			//--------------------------------------------------------------------------------'
			//Purpose:   like "VBScript", "JavaScript"
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   18.05.2004 22:23:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public string sLanguage
			{
				get
				{
					return msLanguage;
				}
				set
				{
					msLanguage = value;
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
				efHtmlScript oHtmlScript = new efHtmlScript();
				oHtmlScript.sCode = "";
				oHtmlScript["LANGUAGE"].sValue = sLanguage;
				oHtmlScript["SRC"].sValue = sFilename;
		
				oHtmlScript.gRender(output, 2);
		
			}
		}

	}

}
