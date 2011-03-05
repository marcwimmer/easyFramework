using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using easyFramework.Frontend.ASP.Dialog;
//using easyFramework.sys.ToolLib.Functions;

namespace easyFramework.Frontend.ASP.WebComponents
{
	//================================================================================
//Class:     efXmlDialog
	
	//--------------------------------------------------------------------------------'
//Module:    efXmlDialog.vb
	//--------------------------------------------------------------------------------'
//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
//Purpose:   renders definition-dialog xml to HTML
	//--------------------------------------------------------------------------------'
//Created:   22.03.2004 01:51:37
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	
	//================================================================================
//Imports:
	//================================================================================
	
	[DefaultProperty("sDialogFile"), ToolboxData("<{0}:efXmlDialog runat=server></{0}:efXmlDialog>")]public class efXmlDialog : efBaseElement
	{
		public efXmlDialog()
		{
			msFormName = "frmMain";
			mbUseDiv = false;
		}
		
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
		private string msDefinitionFile;
		private string msFormName;
		
		private string msXmlValues;
		private string msDataPage;
		
		
		bool mbUseDiv;
		
		
		//================================================================================
//Property:  sDefinitionFile
		//--------------------------------------------------------------------------------'
//Purpose:   the XML-file, which contains the dialog structure
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   22.03.2004 15:57:28
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sDefinitionFile
		{
			get{
				return msDefinitionFile;
			}
			
			set
			{
				
				msDefinitionFile = value;
				
			}
		}
		//================================================================================
//Property:  sFormName
		//--------------------------------------------------------------------------------'
//Purpose:   name of the HTML-form, in which this control is embedded
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   22.03.2004 15:57:28
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sFormName
		{
			get{
				return msFormName;
			}
			
			set
			{
				msFormName = value;
			}
		}
		//================================================================================
//Property:  sDefinitionFile
		//--------------------------------------------------------------------------------'
//Purpose:   the XML content of the values of the dialog
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   22.03.2004 15:57:28
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sXmlValues
		{
			get{
				return msXmlValues;
			}
			
			set
			{
				
				msXmlValues = value;
			}
		}
		
		
		
		
		//================================================================================
//Property:  sDataPage
		//--------------------------------------------------------------------------------'
//Purpose:   the datapage, which delievers the data to the dialog. the call to the
		//           datapage is invoked per javascript
		//--------------------------------------------------------------------------------'
//Params:    -
		//--------------------------------------------------------------------------------'
//Created:   07.04.2004 20:22:03
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public string sDataPage
		{
			get{
				return msDataPage;
			}
			
			set
			{
				msDataPage = value;
			}
		}
		
		
		//================================================================================
//Property:  bUseDiv
		//--------------------------------------------------------------------------------'
//Purpose:   if the html shall render a container div (with absolute-positioning)
		//           or not
		//           if not then sTopValue, sLeftValue, sWidth and sHeight do nothing
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   05.04.2004 23:59:58
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		[Bindable(true), Category("Appearance"), DefaultValue("")]public bool bUseDiv
		{
			get{
				return mbUseDiv;
			}
			
			set
			{
				mbUseDiv = value;
			}
		}
		
		
		
		
		
		
		
		
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
//Sub:       Render
		//--------------------------------------------------------------------------------'
//Purpose:   starts rendering of component; overridden
		//--------------------------------------------------------------------------------'
//Params:    html-writer
		//--------------------------------------------------------------------------------'
//Created:   23.03.2004 02:04:03
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		protected override void Render (System.Web.UI.HtmlTextWriter output)
		{
			Render(output, this.Parent);
			
		}
		public void Render (System.Web.UI.HtmlTextWriter output, System.Web.UI.Control oParent)
		{
			
			if (!Functions.IsEmptyString(this.msDefinitionFile))
			{
				
				XmlDocument oXml;
				
				//there could be XML-directly in the sDefinitionFile:
				if (easyFramework.Sys.ToolLib.Functions.Left(msDefinitionFile, 1) == "<")
				{
					oXml = new Sys.Xml.XmlDocument(msDefinitionFile);
				}
				else
				{
					oXml = new Sys.Xml.XmlDocument();
					oXml.gLoad(easyFramework.Frontend.ASP.ASPTools.Tools.sWebToAbsoluteFilename(oParent.Page.Request, msDefinitionFile, false));
				}
				
				
				XmlDialogRenderer oDialogRenderer = new XmlDialogRenderer();
				
				XmlDocument oXmlData = null;
				if (!Functions.IsEmptyString(msXmlValues))
				{
					oXmlData = new Sys.Xml.XmlDocument(msXmlValues);
				}
				
				FastString oSb = new FastString();
				efHtmlDiv oHtmlDiv = null;
				if (mbUseDiv)
				{
					oHtmlDiv = new efHtmlDiv();
					oHtmlDiv["style"].sValue = "position:absolute; ";
					
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
						oHtmlDiv["style"].sValue += "top:" + "width:" + Width.ToString() + ";";
					}
					if (! Height.IsEmpty)
					{
						oHtmlDiv["style"].sValue += "height: " + Height.ToString() + ";";
					}
					
					oHtmlDiv["style"].sValue += "overflow: " + efHtmlConsts.Overflow(this.Overflow);
					
					
					oHtmlDiv.gRenderBeginTag(oSb, 1);
					output.Write(oSb.ToString(), 1);
				}
				
				
				
				output.Write(
					XmlDialogRenderer.gsRender(
						oGetClientInfoFromParent(oParent), 
						oXml, 
						oXmlData, 
						msFormName, 
						this.ID, "", msDataPage)
					);
				
				if (oHtmlDiv != null)
				{
					oHtmlDiv.gRenderEndTag(oSb, 1);
					output.Write(oSb.ToString());
				}
				
				//-------------------create a javascript-tag to execute the collected javascript
				//                   in the javascript-container-----------------------------
				efHtmlScript oHtmlScript = new efHtmlScript();
				
				//append it into the javascript-container, so if the container is removed, the code is removed, too
				oHtmlScript.sCode = "var oJavaScriptContainer = document.getElementById(\"" + this.ID + "_javascripts" + "\");" + "\n" + "if (oJavaScriptContainer != null) { " + "\n" + "var oNewJavaScript = document.createElement(\"script\");" + "oNewJavaScript.text = gsResolveEntities(oJavaScriptContainer.innerHTML);" + "oJavaScriptContainer.parentNode.appendChild(oNewJavaScript);" + "}" + "\n" + "";
				
				
				oHtmlScript.gRender(output, 1);
				
				
			}
			
		}
		
		
		
		
	}
	
	
	
	
	
	
	
	
	
	
}
