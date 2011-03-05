using System;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using System.Web.UI.WebControls;

namespace easyFramework.Frontend.ASP.ComplexObjects
{
	//================================================================================
	//Class:     PopMenueRenderer

	//--------------------------------------------------------------------------------'
	//Module:    PopMenueRenderer.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:
	//--------------------------------------------------------------------------------'
	//Created:   25.04.2004 01:58:26
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class PopMenueRenderer
	{
		public PopMenueRenderer()
		{
			moEntries = new efArrayList();
			mlItemHeight = 18;
		}
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private efArrayList moEntries; //of Entry
		private int mlItemHeight; //could be a property later
		
		
		
		//================================================================================
		//Public Methods:
		//================================================================================
		public void gAddOption (string sUrl, string sJavascript, bool bModal, string sAfterModalFunc, string sCaption)
		{
			
			Entry oEnt = new Entry();
			oEnt = new Entry();
			oEnt.bIsSeperator = false;
			oEnt.sCaption = sCaption;
			oEnt.sUrl = sUrl;
			oEnt.sJavascript = sJavascript;
			oEnt.bModal = bModal;
			oEnt.sModalAfterFunc = sAfterModalFunc;
			moEntries.Add(oEnt);
		}
		
		public void gAddSpacer ()
		{
			
			Entry oEnt = new Entry();
			oEnt = new Entry();
			oEnt.bIsSeperator = true;
			moEntries.Add(oEnt);
			
		}
		
		public void gClear ()
		{
			
			moEntries.Clear();
			
		}
		
		
		//================================================================================
		//Sub:       Render
		//--------------------------------------------------------------------------------'
		//Purpose:   creates the result-html
		//--------------------------------------------------------------------------------'
		//Params:
		//           sId -  id - of the popup-menue e.g. "Popup1"; must be unique in the
		//               resulting html
		//           oWidth - Unit/width of the popup-menue
		//           bJavaCodeToJavaContainer - if true, then only the content of the
		//               script-tag is rendered; useful, if javascript-codes shall be
		//               transported within a text-area
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 02:10:58
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public virtual string Render(string sId, Unit oWidth, bool bJavaCodeToJavaContainer)
		{
			
			efHtmlScript oHtmlScript = new efHtmlScript();
			
			//menue-height depends on item-count:
			int lDivHeight = mlItemHeight * moEntries.Count - 1;
			int lWidth;
			
			if (oWidth.IsEmpty)
			{
				lWidth = 140;
			}
			else
			{
				lWidth = DataConversion.glCInt(Functions.Replace(oWidth.ToString(), "px", ""), 0);
			}
			
			
			oHtmlScript.sCode = "" + "\n" + "" + "\n" + "function gShowPopup_" + sId + "() {" + "\n";
			
			
			//--------create popup-entries per javascript-functions
			oHtmlScript.sCode += "gCreatePopupMenue('" + sId + "',\"" + lWidth + "\");" + "\n";
			
			oHtmlScript.sCode += "var oPopupMenue = goFindPopupMenue('" + sId + "');";
			
			
			for (int i = 0; i <= moEntries.Count - 1; i++)
			{
				Entry oEntry = ((Entry)(moEntries[i]));
				if (oEntry.bIsSeperator)
				{
					oHtmlScript.sCode += "\n" + "oPopupMenue.AddSpacer();" + "\n";
				}
				else
				{
					
					oHtmlScript.sCode += "var s1 = new String(\"" + oEntry.sUrl + "\");" + "\n";
					oHtmlScript.sCode += "var s2 = new String(\"" + oEntry.sJavascript + "\");" + "\n";
					oHtmlScript.sCode += "var s3 = new String(\"" + DataConversion.glCInt(oEntry.bModal, 0) + "\");" + "\n";
					oHtmlScript.sCode += "var s4 = new String(\"" + Functions.Replace(oEntry.sModalAfterFunc, "\"", "82857177JFHUENME") + "\");" + "\n";
					oHtmlScript.sCode += "var s5 = new String(\"" + oEntry.sCaption + "\");" + "\n";
					oHtmlScript.sCode += "var s6 = new String(\"" + mlItemHeight + "\");" + "\n";
					
					oHtmlScript.sCode += "oPopupMenue.AddEntry(s1, s2, s3, s4, s5, s6);" + "\n" + "\n";
					
				}
			}
			
			//--------display popup---------
			oHtmlScript.sCode += "   gShowPopupMenue('" + sId + "');" + "\n" + "}" + "" + "\n" + "\n";
			
			
			if (bJavaCodeToJavaContainer)
			{
				return oHtmlScript.sCode;
			}
			else
			{
				FastString oTemp = new FastString();
				
				oHtmlScript.gRender(oTemp, 1);
				return oTemp.ToString();
			}
			
			
			
		}
		
		
		//================================================================================
		//Private Classes:
		//================================================================================
		private class Entry
		{
			public string sCaption;
			public bool bIsSeperator;
			public string sUrl;
			public string sJavascript;
			public bool bModal;
			public string sModalAfterFunc;
		}
		
		
	}
	
}
