using System;
using easyFramework.Sys;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Entities;
using easyFramework.Frontend.ASP.ASPTools;
using System.Web.UI.WebControls;

namespace easyFramework.Frontend.ASP.ComplexObjects
{
	//================================================================================
	//Class:     XmlDialogRenderer
	//--------------------------------------------------------------------------------'
	//Module:    XmlDialogRenderer.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   Renders the dialog-xml files to html
	//
	//           Important: this codes doesn't produce javascript-tags directly.
	//           instead a div with name "<xmldialog-id>_javascripts" is created. its
	//           string-content are the javascript-functions
	//           reason: the xml-dialog can be rendered dynamically for example into
	//           a tab-dialog tab. if the innerhtml-property is set to the rendering
	//           result, then the script-tags are not parsed. but: if the tab
	//           creates a script tag by javascript (document.createElement("script"))
	//           and adds the string-content of the javascript-div, then
	//           the javascript can be executed.
	//           so every container has to load the content of the javascript-div
	//           into a created script-element.
	//
	//
	//
	//--------------------------------------------------------------------------------'
	//Created:   23.03.2004 15:56:34
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class XmlDialogRenderer
	{
		
		
		//================================================================================
		//Public Consts:
		//================================================================================
		public static string efsCss_dialogTable = "dialogTable";
		public static string efsCssTrDarkGray = "trDarkGray";
		public static string efsCssTrDark = "trDark";
		public static string efsCssTrBackground = "trBackground";
		public static string efsCssTddlgField = "dlgField";
		public static string efsCssTdentryField = "entryField";
		public static string efsCssTdborderField = "borderField";
		public static string efsCssTdcaptionField = "captionField";
		public static string efsCssInputTxtLabelField = "txtLabelField";
		
		public static string efsDialogTypeHiddenField = "__dialogtype";
		public static string efsDialogPageSizeHiddenField = "__dialogpagesize";
		public static string efsDialogPageDirtyFlag = "__dirty";
		public static string efsDialogType_MultiRow = "multirow";
		public static string efsDialogType_SingleRow = "singlerow";
		public static string efsEntityToStringValue = "_EntityToStringValue";
		public static string efsEntityToStringValueOnFocus = "_onFocusEntityToStringValue";
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		//================================================================================
		//Function:  sRender
		//--------------------------------------------------------------------------------'
		//Purpose:   transforms the xml to html via the html-renderer library
		//--------------------------------------------------------------------------------'
		//Params:    oXmlDefinition - the xmldialog-definition-file
		//           oXmlData - the data of the dialog
		//           sNamePraefix - a praefix, which is to the very beginning of the id
		//
		//--------------------------------------------------------------------------------'
		//Returns:   the rendered html
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 00:21:50
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsRender(ClientInfo oClientInfo, XmlDocument oXmlDefinition, XmlDocument oXmlData, string sHtmlFormName, string sXmlDialogID, string sNamePraefix, string sDataPage)
		{
			
			//---------------------------------basic checks--------------------------------------
			if (oXmlDefinition.selectSingleNode("/efDialogPage") == null)
			{
				throw (new RenderException("Invalid Dialogdefinition: " + oXmlDefinition.sXml));
			}
			
			//-----------------check, wether all elements have a name element---------------------
			XmlNodeList oNlField = oXmlDefinition.selectNodes("//efDialogField[count(NAME) = 0]");
			if (oNlField.lCount > 0)
			{
				throw (new RenderException("each dialog-field needs a name-element"));
			}
			
			
			
			//----------------------resolve include-instructions, recursivley--------------------
			oXmlDefinition = new XmlDocument(mResolveIncludeNodes(oXmlDefinition.oChildren[oXmlDefinition.oChildren.lCount - 1], oClientInfo));
			
			
			
			//----------------------some checks-------------------------------------
			mCheckForUniqueNames(oXmlDefinition);
			
			//---------------------------determine MULTIROW--------------------------
			bool bMultiRow;
			if (oXmlDefinition.selectSingleNode("/efDialogPage")["MULTIROW"].sText == "1")
			{
				bMultiRow = true;
			}
			else
			{
				bMultiRow = false;
			}
			
			//------------------------check the data------------------------
			if (oXmlData == null)
			{
				oXmlData = new XmlDocument("<DIALOGINPUT/>");
			}
			if (oXmlData.selectSingleNode("/DIALOGINPUT") == null)
			{
				throw (new RenderException("Invalid Dialogdata: " + oXmlData.sXml + "\n" + "Should start with \"<DIALOGINPUT>\""));
			}
			if (bMultiRow)
			{
				if (oXmlDefinition.selectSingleNode("/efDialogPage/@DIALOGSIZE") == null)
				{
					throw (new RenderException("Invalid XmlDialogDefinition-Data - attribute /efDialogPage/@DIALOGSIZE is missing: " + oXmlData.sXml));
				}
				
			}
			
			efHtmlTable oHtmlTable = new efHtmlTable();
			oHtmlTable["WIDTH"].sValue = "100%";
			
			//the hidden-field __dialogtype is created, which contains the values multirow
			//or single-row:
			HTMLRenderer.efHtmlInput oHtmlHidden = new HTMLRenderer.efHtmlInput(oHtmlTable);
			oHtmlHidden["TYPE"].sValue = "HIDDEN";
			oHtmlHidden["NAME"].sValue = efsDialogTypeHiddenField;
			if (bMultiRow)
			{
				oHtmlHidden["VALUE"].sValue = efsDialogType_MultiRow;
			}
			else
			{
				oHtmlHidden["VALUE"].sValue = efsDialogType_SingleRow;
			}
			
			//the hidden-field __dialogpage is created, which contains the amount of lines
			//of each page:
			if (bMultiRow)
			{
				oHtmlHidden = new HTMLRenderer.efHtmlInput(oHtmlTable);
				oHtmlHidden["TYPE"].sValue = "HIDDEN";
				oHtmlHidden["NAME"].sValue = efsDialogPageSizeHiddenField;
				oHtmlHidden["VALUE"].sValue = oXmlDefinition.selectSingleNode("/efDialogPage/@DIALOGSIZE").sText;
			}
			
			//-----------the hidden-field __dirty for the dirty-flag-------------------------
			oHtmlHidden = new HTMLRenderer.efHtmlInput(oHtmlTable);
			oHtmlHidden["TYPE"].sValue = "HIDDEN";
			oHtmlHidden["NAME"].sValue = efsDialogPageDirtyFlag + sXmlDialogID;
			oHtmlHidden["VALUE"].sValue = "0";
			
			
			XmlDocument oXmlCurrentData = new XmlDocument();
			
			//add the <ROW/>, if single-row dialog  to the data-nodes
			if (bMultiRow == false)
			{
				
				//send for each dialog-row-in-a-row the xml-data;
				//the method handleDialogRow expects the root-element
				//row, so if we don't have it yet at this non multi-row-dialog,
				//we add it:
				
				if (oXmlData.selectSingleNode("/DIALOGINPUT/ROW") == null)
				{
					oXmlCurrentData = new XmlDocument("<DIALOGINPUT><ROW NUMBER=\"1\">" + oXmlData.selectSingleNode("/DIALOGINPUT").sInnerXml + "</ROW></DIALOGINPUT>");
				}
				else
				{
					oXmlCurrentData = new XmlDocument(oXmlData.selectSingleNode("/DIALOGINPUT").sXml);
				}
			}
			
			
			
			XmlNodeList nlDefRows = oXmlDefinition.selectNodes("/efDialogPage/efDialogRow");
			int lPageSize;
			if (bMultiRow == false)
			{
				lPageSize = 1;
			}
			else
			{
				if (oXmlDefinition.selectSingleNode("/efDialogPage/@DIALOGSIZE") == null)
				{
					throw (new RenderException("Attribute DIALOGSIZE is missing!"));
				}
				lPageSize = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlDefinition.selectSingleNode("/efDialogPage/@DIALOGSIZE").sText, 0);
			}
			if (bMultiRow == false)
			{
				lPageSize = 1;
			}
			
			
			
			//---------------create the javascript-div-container----------------------
			FastString ofsJavaScripts = new FastString();
			
			
			//--------------------------determine the first element to focus:------------------
			string sElementNameToFocus = ""; //name of the first element in the dialog, which gets the focus
			
			for (int lItPageSize = 0; lItPageSize <= lPageSize - 1; lItPageSize++)
			{
				
				XmlNode oXmlDataNode = oXmlCurrentData.selectSingleNode("/DIALOGINPUT/ROW[" + lItPageSize + 1 + "]");
				
				for (int lItDefinedRows = 0; lItDefinedRows <= nlDefRows.lCount - 1; lItDefinedRows++)
				{
					
					mHandleDialogRow(oClientInfo, oHtmlTable, nlDefRows[lItDefinedRows], oXmlDataNode, bMultiRow, lItPageSize, ref sElementNameToFocus, sHtmlFormName, sNamePraefix, ofsJavaScripts, ref sElementNameToFocus, sXmlDialogID);
				}
				
				
			}
			
			//--------------register the dialog per javascript----------
			ofsJavaScripts.Append("\n" + "RegisterXmlDialog('" + sXmlDialogID + "','" + sHtmlFormName + "'" + ",'" + sElementNameToFocus + "'," + "'" + sDataPage + "');");
			
			
			
			
			FastString oStringBuilder = new FastString();
			
			//---------------CSS-formatting-------------------
			oHtmlTable["class"].sValue = efsCss_dialogTable;
			
			
			//--------------render the input-elements----------------
			oHtmlTable.gRender(oStringBuilder, 1);
			
			
			
			
			//--------------create the javascript-container----------
			efHtmlDiv oHtmlJavascriptContainer = new efHtmlDiv();
			oHtmlJavascriptContainer["style"].sValue = "visibility: hidden; position:absolute;";
			efHtmlTextarea oHtmlTextArea_JavaScriptContainer = new efHtmlTextarea(oHtmlJavascriptContainer);
			oHtmlTextArea_JavaScriptContainer["name"].sValue = sXmlDialogID + "_javascripts";
			oHtmlTextArea_JavaScriptContainer["id"].sValue = sXmlDialogID + "_javascripts";
			oHtmlTextArea_JavaScriptContainer.sText = ofsJavaScripts.ToString();
			oHtmlJavascriptContainer.gRender(oStringBuilder, 1);
			
			
			
			return oStringBuilder.ToString();
			
			
		}
		
		
		
		//================================================================================
		//Private Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       handleDialogRow
		//--------------------------------------------------------------------------------'
		//Purpose:   creates out of the dialogdefinition a row in the dialog;
		//--------------------------------------------------------------------------------'
		//Params:    the parent html-table
		//           the dialog-definition-node of the row
		//           the dialog-data; in a multi-step dialog it is one of several nodes
		//           bMultiRow - if multirow dialog the true
		//           lRowNumber - the current row-number
		//           sNameOfElementToFocus - here is the name of the first element returned
		//           sFormName - the name of the form-tag
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 00:51:07
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static void mHandleDialogRow (ClientInfo oClientInfo, efHtmlTable oParentTable, XmlNode oDialogDefRowNode, XmlNode oDialogDataRowNode, 
			bool bMultiRow, int lRowNumber, ref string sNameOfElementToFocus, string sFormName, string sNamePraefix, FastString oJavaScriptContainer, 
			ref string sFirstElementToFocus, string sXmlDialogId)
		{
			
			efHtmlTr oHtmlTr = new efHtmlTr(oParentTable);
			XmlNodeList nlFields = oDialogDefRowNode.selectNodes("efDialogField");
			
			//css-formatting:
			oHtmlTr["class"].sValue = efsCssTrDarkGray;
			
			
			
			
			for (int i = 0; i <= nlFields.lCount - 1; i++)
			{
				
				string sDialogFieldValue = "";
				string sFieldName = "";
				sFieldName = nlFields[i].selectSingleNode("NAME").sText;
				if (  oDialogDataRowNode != null)
				{
					if ( oDialogDataRowNode.selectSingleNode(sFieldName) != null)
					{
						sDialogFieldValue = oDialogDataRowNode.selectSingleNode(sFieldName).sText;
					}
				}
				
				
				
				
				mHandleDialogField(oClientInfo, oHtmlTr, nlFields[i], sDialogFieldValue, bMultiRow, lRowNumber, sFormName, sNamePraefix, oJavaScriptContainer, sXmlDialogId, ref sFirstElementToFocus);

				

			
			}

			
		}
		
		
		//================================================================================
		//Sub:       handleDialogRow
		//--------------------------------------------------------------------------------'
		//Purpose:   creates out of the dialogdefinition a field in a row
		//--------------------------------------------------------------------------------'
		//Params:    the parent html-row (oParentRow)
		//           the dialog-definition-node of the row (oDialogDefFieldNode)
		//           the dialog-data-value (sDialogDataFieldValue)
		//           bMultiRow - if multirow-dialog
		//           lRowNumber - if multi-row dialog, then the row-number here
		//           sNameOfElementToFocus - here is the name of the first element returned
		//--------------------------------------------------------------------------------'
		//Created:   24.03.2004 00:53:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static void mHandleDialogField (ClientInfo oClientInfo, efHtmlTr oParentRow, XmlNode oDialogDefFieldNode, 
				string sDialogDataFieldValue, bool bMultiRow, int lRowNumber, string sFormName, 
				string sNamePraefix, FastString oJavaScriptContainer, string sXmlDialogId, ref string sElementNameToFocus)
		{
			
			
			//--------------check for CSS-Class--------------
			string sCss = "";
			if (oDialogDefFieldNode.selectSingleNode("CLASS") == null == false)
			{
				sCss = oDialogDefFieldNode.selectSingleNode("CLASS").sText;
			}
			
			
			//if there is a description, then ad a field for description:
			efHtmlLabel oHtmlDescriptionLabel = null;
			if (oDialogDefFieldNode.selectSingleNode("DESC") == null == false)
			{
				if (!Functions.IsEmptyString(oDialogDefFieldNode.selectSingleNode("DESC").sText))
				{
					
					efHtmlTd oHtmlTdDescription = new efHtmlTd(oParentRow);
					oHtmlDescriptionLabel = new efHtmlLabel(oHtmlTdDescription);
					oHtmlTdDescription["class"].sValue = efsCssTdcaptionField;
					
					oHtmlDescriptionLabel.sText = oDialogDefFieldNode.selectSingleNode("DESC").sText;
					
					if (Functions.InStr(oHtmlDescriptionLabel.sText, "$HOT$") > 0)
					{
						//replace $HOT$ with the default-amp:
						
						//replace all later $HOT$ chars:
						oHtmlDescriptionLabel.sText = Functions.Replace(oHtmlDescriptionLabel.sText, "$HOT$", "&", 1, 1);
						oHtmlDescriptionLabel.sText = Functions.Replace(oHtmlDescriptionLabel.sText, "$HOT$", "");
						
					}
					else if (Functions.InStr(oHtmlDescriptionLabel.sText, "$") > 0)
					{
						//replace $ with the default-amp:
						
						
						//if it is not $$ then replace it:
						if (Functions.InStr(oHtmlDescriptionLabel.sText, "$") + 1 <= oHtmlDescriptionLabel.sText.Length)
						{
							if (oHtmlDescriptionLabel.sText.Substring(Functions.InStr(oHtmlDescriptionLabel.sText, "$"), 1) != "$")
							{
								//replace all later $HOT$ chars:
								oHtmlDescriptionLabel.sText = Functions.Replace(oHtmlDescriptionLabel.sText, "$", "&");
								
								oHtmlDescriptionLabel.sText = oHtmlDescriptionLabel.sText.Replace("$$", "$");
							}
							else
							{
								oHtmlDescriptionLabel.sText = oHtmlDescriptionLabel.sText.Replace("$$", "$");
								
							}
						}
						
						
						
					}
					
					//set the for-id later, after the input control is rendered; later the
					//id of the control is determined
					
					
					
					//----------------------check for COLSPANLABEL----------------------
					string sColSpanLabel;
					sColSpanLabel = oDialogDefFieldNode["COLSPANLABEL"].sText;
					if (!Functions.IsEmptyString(sColSpanLabel))
					{
						oHtmlTdDescription["COLSPAN"].sValue = sColSpanLabel;
					}
					
					//----------------------check for WIDTHLABEL----------------------
					string sWidthLabel;
					sWidthLabel = oDialogDefFieldNode["WIDTHLABEL"].sText;
					if (!Functions.IsEmptyString(sWidthLabel))
					{
						oHtmlTdDescription["WIDTH"].sValue = sWidthLabel;
					}
					else
					{
						oHtmlTdDescription["WIDTH"].sValue = "20%"; //default-value
					}
					
					//----------------------check for NOWRAP------------------
					if (!Functions.IsEmptyString(oDialogDefFieldNode["NOWRAP"].sText))
					{
						oHtmlTdDescription["NOWRAP"].sValue = oDialogDefFieldNode["NOWRAP"].sText;
					}
					
					
				}
				
				
				
				
			}
			
			
			
			
			
			
			
			efHtmlTd oHtmlTd = new efHtmlTd(oParentRow);
			oHtmlTd["class"].sValue = efsCssTdentryField;
			oHtmlTd["NOWRAP"].sValue = "true";
			
			
			//---------------------check for COLSPANFIELD------------------
			string sColSpanField;
			sColSpanField = oDialogDefFieldNode["COLSPANFIELD"].sText;
			if (!Functions.IsEmptyString(sColSpanField))
			{
				oHtmlTd["COLSPAN"].sValue = sColSpanField;
			}
			
			//----------------------check for WIDTHFIELD------------------
			string sWidthField;
			sWidthField = oDialogDefFieldNode["WIDTHFIELD"].sText;
			if (!Functions.IsEmptyString(sWidthField))
			{
				oHtmlTd["WIDTH"].sValue = sWidthField;
			}
			else
			{
				oHtmlTd["WIDTH"].sValue = "30%";
			}
			
			
			//----------------------check for NOWRAP------------------
			if (!Functions.IsEmptyString(oDialogDefFieldNode["NOWRAP"].sText))
			{
				oHtmlTd["NOWRAP"].sValue = oDialogDefFieldNode["NOWRAP"].sText;
			}
			
			
			//--------------------check for readonly-------------------------
			bool bReadonly = false;
			if (oDialogDefFieldNode.selectSingleNode("READONLY") == null == false)
			{
				bReadonly = easyFramework.Sys.ToolLib.DataConversion.gbCBool(oDialogDefFieldNode.selectSingleNode("READONLY").sText);
			}
			
			//--------------------check for cols and rows-------------------------
			string sCols = "";
			string sRows = "";
			if (oDialogDefFieldNode.selectSingleNode("COLS") == null == false)
			{
				sCols = easyFramework.Sys.ToolLib.DataConversion.gsCStr(oDialogDefFieldNode.selectSingleNode("COLS").sText);
			}
			if (oDialogDefFieldNode.selectSingleNode("ROWS") == null == false)
			{
				sRows = easyFramework.Sys.ToolLib.DataConversion.gsCStr(oDialogDefFieldNode.selectSingleNode("ROWS").sText);
			}
			
			
			//-----------check for name attribute---------------------------
			if (oDialogDefFieldNode.selectSingleNode("NAME") == null)
			{
				throw (new RenderException("Invalid field-definition - NAME-element is missing: " + oDialogDefFieldNode.sXml));
			}
			
			
			//-----------check type attribute-----------------
			if (oDialogDefFieldNode.selectSingleNode("TYPE") == null)
			{
				throw (new RenderException("Invalid field-definition - TYPE-element is missing: " + oDialogDefFieldNode.selectSingleNode("NAME").sText));
			}
			
			//-----------check if entity-----------------
			bool bIsEntity = false;
			IEntity oEntity = null;
			if (oDialogDefFieldNode.selectSingleNode("ENTITY") == null == false)
			{
				
				//load entity
				oEntity = EntityLoader.goLoadEntity(oClientInfo, oDialogDefFieldNode.selectSingleNode("ENTITY").sText);
				bIsEntity = true;
				
			}
			
			
			//----------init selementname-------------------
			string sElementName = ""; //could be adapated, so store
			
			
			//----------if there is no sdialogvalue but a value tag, then
			//           overwrite sDialogValue with the content from value ----------------
			if (oDialogDefFieldNode.selectSingleNode("VALUE") != null & Functions.IsEmptyString(sDialogDataFieldValue))
			{
				sDialogDataFieldValue = msResolveValueTag(oClientInfo, oDialogDefFieldNode.selectSingleNode("VALUE").sInnerXml);
			}
			
			//---------------default eigenschaft--------------
			bool bDefault = false;
			if (oDialogDefFieldNode.selectSingleNode("DEFAULT") != null)
				bDefault = oDialogDefFieldNode.sGetValue("DEFAULT", "false", false).ToLower().Equals("true");
			
			
			//----------create html-controls----------------
			
			string sInputType = Functions.UCase(oDialogDefFieldNode.selectSingleNode("TYPE").sText);
			switch (sInputType)
			{
				case "INPUT":
					DialogFieldHandler.Render_INPUT(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
					
				case "DISABLED":
					DialogFieldHandler.Render_DISABLED(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "DATE":
					DialogFieldHandler.Render_DATE(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "NUMBER":
					DialogFieldHandler.Render_NUMBER(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
										
				case "LABEL":
					DialogFieldHandler.Render_LABEL(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "INPUTFIRST":
					DialogFieldHandler.Render_INPUTFIRST(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "PASSWORD":
					
					DialogFieldHandler.Render_PASSWORD(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "HIDDEN":
					
					DialogFieldHandler.Render_HIDDEN(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;

				case "TEXTAREA":
					
					DialogFieldHandler.Render_TEXTAREA(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;


					
				case "CHECKBOX":
					
					DialogFieldHandler.Render_CHECKBOX(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;

					
				case "LISTCOMBO":
					DialogFieldHandler.Render_LISTCOMBO(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "DATACOMBO":
					DialogFieldHandler.Render_DATACOMBO(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "DATACOMBON":
					DialogFieldHandler.Render_DATACOMBON(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss);
					break;
					
				case "BUTTON":
					DialogFieldHandler.Render_BUTTON(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss, bDefault);
					break;

				case "BUTTOND":
					DialogFieldHandler.Render_BUTTON(oClientInfo, bIsEntity, bReadonly, sElementName,
						oParentRow, oHtmlTd, lRowNumber, bMultiRow, sNamePraefix, sDialogDataFieldValue, 
						oEntity, oDialogDefFieldNode, sXmlDialogId, sFormName, sInputType, oJavaScriptContainer, 
						ref sElementNameToFocus, sRows, sCols, sCss, bDefault);
					break;
			}
						
			
						
			//if there is a description element, then now set the for-id of the description,
			//so that pressing the alt+??? key, selects the input:
			if (oHtmlDescriptionLabel != null)
			{
				oHtmlDescriptionLabel.sFor = sElementName;
			}
						
			//for every input, there shall be a hidden-field, which contains the original-value
			//of each field:
			if (!Functions.IsEmptyString(sElementName))
			{
				efHtmlInput oHtmlOriginalValue = new efHtmlInput(oParentRow);
				oHtmlOriginalValue["TYPE"].sValue = "hidden";
				oHtmlOriginalValue["NAME"].sValue = sElementName + "_old";
				oHtmlOriginalValue["VALUE"].sValue = sDialogDataFieldValue;
			}
						
						
		}   

		
						
						
						

		//================================================================================
		//Function:  sGetElementNamePraefix
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the name of an element; use this function to construct the
		//           elements name
		//--------------------------------------------------------------------------------'
		//Params:    the user-defined name (in xmldefinition for example)
		//           bMultiRow - is element part of a multi-row dialog
		//           lRowNumber - if part of a multirow-dialog, then here is the rownumber
		//           sGeneralNamePraefix - which always stands in front of the name
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 02:28:13
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		internal static string msGetElementNamePraefix(string sName, bool bMultiRow, int lRowNumber, string sGeneralNamePraefix)
		{
							
			if (! bMultiRow)
			{
				return sGeneralNamePraefix + sName;
			}
			else
			{
				return sGeneralNamePraefix + "ROW" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lRowNumber) + "_" + sName;
			}
							
		}
						
						
		//================================================================================
		//Sub:       mBuildEventHandlers
		//--------------------------------------------------------------------------------'
		//Purpose:   looks into the dialog-def node and checks, wether there are events
		//--------------------------------------------------------------------------------'
		//Params:    the htmle-element;
		//           the xml-definition node <efDialogField>
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 10:22:06
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		internal static void mBuildEventHandlers (efHTMLElement oHtmlElement, XmlNode oDialogDefNode, FastString oJavaScriptContainer)
		{
							
			string[] asEvents;

			asEvents = new string[14];

			asEvents[0] = "ONCLICK";
			asEvents[1] = "ONDBLCLICK";
			asEvents[2] = "ONMOUSEDOWN";
			asEvents[3] = "ONMOUSEUP";
			asEvents[4] = "ONMOUSEOVER";
			asEvents[5] = "ONMOUSEREMOVE"; 
			asEvents[6] = "ONMOUSEOUT"; 
			asEvents[7] = "ONKEYPRESS"; 
			asEvents[8] = "ONKEYDOWN"; 
			asEvents[9] = "ONKEYUP"; 
			asEvents[10] = "ONBLUR"; 
			asEvents[11] = "ONFOCUS"; 
			asEvents[12] = "ONCHANGE"; 
			asEvents[13] = "ONSELECT";

							
			for (int i = 0; i < asEvents.Length; i++)
			{
								
				if (  oDialogDefNode.selectSingleNode(asEvents[i]) != null)
				{
					mAddJavascriptEventHandlers(oHtmlElement, asEvents[i], oDialogDefNode.selectSingleNode(asEvents[i]).sText, oJavaScriptContainer);
				}
			}
							
		}
						
						
		//================================================================================
		//Sub:       mAddJavascriptEventHandlers
		//--------------------------------------------------------------------------------'
		//Purpose:   if there are javascript-eventhandlers defined, such as onclick etc.
		//           then this function Sub makes sure, that a script-block is generated
		//           with a unique-functionname. then input-element then calls this
		//           unique function name. the advantage is, that the script can be designed
		//           as wanted, with "-characters and many lines
		//--------------------------------------------------------------------------------'
		//Params:    the html-element with the event
		//           sEventName - the event name, like "onclick"
		//           sEventCode - the script-code
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 10:04:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static void mAddJavascriptEventHandlers (efHTMLElement oHtmlElement, string sEventName, string sEventCode, FastString oJavaScriptContainer)
		{
							
							
			string sUniqueFunctionName;
			sUniqueFunctionName = sEventName + System.Guid.NewGuid().ToString();
			sUniqueFunctionName = Functions.Replace(sUniqueFunctionName, "}", "");
			sUniqueFunctionName = Functions.Replace(sUniqueFunctionName, "{", "");
			sUniqueFunctionName = Functions.Replace(sUniqueFunctionName, "-", "");
							
			oHtmlElement[sEventName].sValue = sUniqueFunctionName + "(); return false;";
							
			oJavaScriptContainer.Append("\n" + "function " + sUniqueFunctionName + "() {" + "\n" + sEventCode + "\n" + "}");
							
		}
						
						
		//================================================================================
		//Sub:       mCheckForUniqueNames
		//--------------------------------------------------------------------------------'
		//Purpose:   checks the given xml-document, wether the elements have unique names or not:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 15:42:22
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		internal static void mCheckForUniqueNames (XmlDocument oXml)
		{
							
			efHashTable oHash = new efHashTable();
			XmlNodeList oNl = oXml.selectNodes("//NAME");
			for (int i = 0; i <= oNl.lCount - 1; i++)
			{
				if (Functions.IsEmptyString(oNl[i].sText))
				{
					throw (new RenderException("not allowed empty-space in name-element: \"" + oXml.sXml));
									
				}
				else if (oHash.ContainsKey(oNl[i].sText) == false)
				{
					oHash.Add(oNl[i].sText, "1");
									
				}
				else
				{
					throw (new RenderException("names must be unique in dialog: \"" + oNl[i].sText + "\" is found more than one time"));
				}
								
			}
							
							
							
		}
						
						
						
		//================================================================================
		//Function:  msResolveValueTag
		//--------------------------------------------------------------------------------'
		//Purpose:   resolves the content of a value-tag
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//           the text of the value-tag
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   07.05.2004 23:05:22
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		internal static string msResolveValueTag(ClientInfo oClientInfo, string sValueTagText)
		{
							
			if (Functions.InStr(Functions.LCase(sValueTagText), "$query$") == 1)
			{
				string sQry = Functions.Right(sValueTagText, Functions.Len(sValueTagText) - Functions.Len("$query$"));
								
				Recordset rs = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
				if (rs.EOF)
				{
					return "";
				}
				else
				{
					return rs.oFields[0].sValue;
				}
								
								
			}
			else
			{
				return sValueTagText;
			}
							
							
							
		}
						
						
						
						
						
		//================================================================================
		//Function:  msGetOnEntityInputBlur
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the javascript-code for the onblur-function of an input
		//           for entities
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 15:43:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		internal static string msGetOnEntityInputBlur(string sWebpageRoot, string sFormName, string sDialogFieldName, string sEntityName)
		{
							
			string sResult;
							
			sResult = "" + "if (document.forms['" + sFormName + "'].elements['" + sDialogFieldName + efsEntityToStringValue + "']" + ".value == " + "document.forms['" + sFormName + "'].elements['" + sDialogFieldName + efsEntityToStringValueOnFocus + "']" + ".value) {return;};  " + "if (document.forms['" + sFormName + "'].elements['" + sDialogFieldName + efsEntityToStringValue + "'].value == '') {" + "document.forms['" + sFormName + "'].elements['" + sDialogFieldName + "'].value = ''; " + "return;" + "}; " + "gsSearchForKeyValue('" + sFormName + "','" + sDialogFieldName + "','" + sEntityName + "','" + sWebpageRoot + "');";
							
			return sResult;
		}
						
		//================================================================================
		//Function:  msGetOnEntityInputFocus
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the javascript-code for the onfocus-function of an input
		//           for entities
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 15:43:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		internal static string msGetOnEntityInputFocus(string sWebpageRoot, string sFormName, string sDialogFieldName, string sEntityName)
		{
							
			string sResult;
							
			sResult = "document.forms['" + sFormName + "'].elements['" + sDialogFieldName + efsEntityToStringValueOnFocus + "']" + ".value =  " + "document.forms['" + sFormName + "'].elements['" + sDialogFieldName + efsEntityToStringValue + "']" + ".value;";
							
			return sResult;
		}
						
						
		//================================================================================
		//Function:  msGetJavaFunc_GetEntityId
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the javascript of getting the id of the current entity
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 15:42:43
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string msGetJavaFunc_GetEntityId(string sFormName, string sDialogFieldName)
		{
							
			string sGetEntityID = "document.forms['" + sFormName + "'].elements['" + sDialogFieldName + "'].value";
			return sGetEntityID;
							
		}
						
						
		//================================================================================
		//Function:  msGetJavaFunc_AfterSearchEntity
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the javascript of the function, which is called,
		//           after the entity-search is done.
		//           in the global variable "oModalResult" the search-result is stored
		//           If the dialog was aborted, then the given function is not called
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   25.04.2004 15:43:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string msGetJavaFunc_AfterSearchEntity(string sWebpageRoot, IEntity oDefaultEntity, string sFormName, string sDialogFieldName, efHTMLElement oParentHtmlElement)
		{
							
			string sResult;
			string sHref = oDefaultEntity.sSearchDialog;
			sHref = Functions.Replace(sWebpageRoot + sHref, "//", "/");
							
			sResult = "" + "gAfterModalEntitySearchDlg(" + "'" + sFormName + "'," + "'" + sDialogFieldName + "'," + "'" + oDefaultEntity.sName + "'); " + "";
							
							
			return sResult;
							
		}
						
						

		//================================================================================
		//Sub:       mResolveIncludeNodes
		//--------------------------------------------------------------------------------'
		//Purpose:   puts at the place, where an include node is, the content of the file
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   13.08.2004 13:20:23
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static string mResolveIncludeNodes(XmlNode oNodeToParse, ClientInfo oClientInfo)
		{
			string returnValue;
							
			//----------------------resolve include-files---------------------------
			XmlNodeList onlIncludeNodes = oNodeToParse.selectNodes("//include");
			string sResultXml = oNodeToParse.sXml;
							
							
			if (onlIncludeNodes.lCount == 0)
			{
				return sResultXml;
			}
							
			for (int i = 0; i <= onlIncludeNodes.lCount - 1; i++)
			{
				string sXml;
								
				string sFileName;
				if (Functions.IsEmptyString(onlIncludeNodes[i]["file"].sText))
				{
					throw (new efException("Filename missing at include-node: \"" + onlIncludeNodes[i].sXml + "\""));
				}
				sFileName = onlIncludeNodes[i]["file"].sText;
								
				//----------make filename out of url-name-----------
				sFileName = oClientInfo.oHttpServer.sMapPath(easyFramework.Sys.ToolLib.DataConversion.gsCStr(oClientInfo.oHttpApp.oHttpApplication.Contents["sWebpageRoot"]) + sFileName);
								
				if (! System.IO.File.Exists(sFileName) ) 
				{
					
					if (onlIncludeNodes[i]["noerror_when_file_not_exists"].sText != "1")
					{
						throw (new efException("File not found of include node: \"" + onlIncludeNodes[i].sXml + "\""));
					}
					//-------------replace the include-instruction by white-space-----------
					sResultXml = Functions.Replace(sResultXml, onlIncludeNodes[i].sXml, "");
				}
				else	
				{			
					//--------------------------load file-------------------------------
					System.IO.StreamReader oStream = System.IO.File.OpenText(sFileName);
					sXml = oStream.ReadToEnd();
					oStream.Close();
								
								
					//-------------replace in xml, as it was text-----------
					sResultXml = Functions.Replace(sResultXml, onlIncludeNodes[i].sXml, sXml);
				}
			}
							
							
			//------reload the document, to check, if there are further sub-includes and resolve it------
			XmlDocument oNewXmlDocument = new XmlDocument(sResultXml);
			sResultXml = mResolveIncludeNodes(oNewXmlDocument.oChildren[oNewXmlDocument.oChildren.lCount - 1], oClientInfo);
							
			returnValue = sResultXml;
							
			return returnValue;
		}
						
	}
					
					
					
	//================================================================================
	//Class:     RenderException
	//--------------------------------------------------------------------------------'
	//Module:    DialogRenderer.vb
	//--------------------------------------------------------------------------------'
	//Purpose:   special exception
	//--------------------------------------------------------------------------------'
	//Created:   24.03.2004 00:39:15
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	public class RenderException : efException
	{
						
						
		public RenderException(string sMessage) : base(sMessage) 
		{
		}
	}
					
}
