using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     optionDialog
	//--------------------------------------------------------------------------------'
	//Module:    optionDialog.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   let's the user select, an option-value
	//
	//           this page is usually invoked by the javascript-function
	//           gsShowOptionDialog() from efStandard.js
	//--------------------------------------------------------------------------------'
	//Created:   29.05.2004 22:11:27
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class optionDialog : efDialogPage
	{

		#region " Vom Web Form Designer generierter Code "

		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
	
		}
		protected easyFramework.Frontend.ASP.WebComponents.efPageHeader EfPageHeader1;
		protected easyFramework.Frontend.ASP.WebComponents.efScriptLinks EfScriptLinks1;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnOk;
		protected easyFramework.Frontend.ASP.WebComponents.efButton btnAbort;

		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;



		private void Page_Init (System.Object sender, System.EventArgs e)
		{
			//CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
			//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
			InitializeComponent();
		}

		#endregion

		protected tOptionValues[] aoOptions;
		protected string sText;
		
		public override void CustomInit (XmlDocument oXmlRequest)
		{
	
			this.sTitle = "Auswahl";
	
			string sCaptions;
			string sValues;
	
	
			sCaptions = oXmlRequest.sGetValue("captions", "");
			sValues = oXmlRequest.sGetValue("values", "");
	
			//------seperated by "-||-"--------
			const string sSep = "-||-";
			if (Functions.InStr(sCaptions, sSep) > 0 & Functions.InStr(sValues, sSep) > 0)
			{
		
				string[] asCaptions = Functions.Split(sCaptions, sSep);
				string[] asValues = Functions.Split(sValues, sSep);
		
				for (int i = 0; i <= asCaptions.Length - 1; i++)
				{
					if (asValues[i] != "")
					{
				
				
						if (aoOptions == null)
						{
							aoOptions = new tOptionValues[1];
						}
						else
						{
							tOptionValues[] newArray = new tOptionValues[aoOptions.Length + 1];
							aoOptions.CopyTo(newArray, 0);
							aoOptions = newArray;

						}
				
						tOptionValues oOptValue = new tOptionValues();
						oOptValue.sCaption = asCaptions[i];
						oOptValue.sValue = asValues[i];
						aoOptions[Functions.UBound(aoOptions)] = oOptValue;
					}
			
				}
		
		
			}
	
			if (aoOptions == null)
			{
				throw (new efException("Please give some options!"));
			}
	
	
			//---------------get text----------
			sText = oXmlRequest.sGetValue("text", "");
	
	
			//--------css & javascript-----------
			gAddCss("../../css/efstyledefault.css", true);
			gAddCss("../../css/efstyledialogtable.css", true);
			gAddScriptLink("../../js/efStandard.js", true, "Javascript");
			gAddScriptLink("../../js/efWindow.js", true, "Javascript");
			gAddScriptLink("../../js/efDlgParams.js", true, "Javascript");
			gAddScriptLink("../../js/efServerProcess.js", true, "Javascript");
			gAddScriptLink("../../js/efTreeview.js", true, "Javascript");
			gAddScriptLink("../../js/efDataTable.js", true, "Javascript");
			gAddScriptLink("../../js/efTabDialog.js", true, "Javascript");
			gAddScriptLink("../../js/efPopupMenue.js", true, "Javascript");
			gAddScriptLink("../../js/efIESpecials.js", true, "VBScript");
			gAddScriptLink("../../js/efOptionDialog.js", true, "Javascript");
	
		}

		protected class tOptionValues
		{
			public string sValue;
			public string sCaption;
		}

	}



}
