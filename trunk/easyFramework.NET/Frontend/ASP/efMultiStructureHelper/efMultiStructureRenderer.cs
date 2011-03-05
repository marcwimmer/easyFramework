using System;
using easyFramework.Frontend.ASP.HTMLRenderer;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;

namespace easyFramework.Frontend.ASP.ComplexObjects
{

	//================================================================================
	//Class:     Multistructure

	//--------------------------------------------------------------------------------'
	//Module:    Multistructure.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung Gmbh
	//--------------------------------------------------------------------------------'
	//Purpose:   for structuring several xml-dialogs;
	//           with plus/minus-button to show and hide
	//           with add-new button
	//--------------------------------------------------------------------------------'
	//Created:   02.05.2004 18:34:14
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================


	public class MultistructureRenderer
	{

		//================================================================================
		//Public Consts:
		//================================================================================

		public enum efEnumLevel
		{
			thisLevel,
			sublevel
		}

		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Function:  gsRender
		//--------------------------------------------------------------------------------'
		//Purpose:   creates the body-functions to create the single dialogs
		//--------------------------------------------------------------------------------'
		//Params:    bDefaultEmptyLine - if the input-elements of the first-line should
		//                               be rendered
		//           sDataPage - the asp, which delievers the content
		//           sTopMostElementValue - in case of a survey e.g. the survey-id; the
		//                           questions then can be added by the multi-structure
		//           sFormName - the name of the form, which contains the fields
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   02.05.2004 19:07:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsRenderInit(XmlDocument oXmlMultiStructure, string sParentDiv, string sFormName, string sMultiStructureXmlFilename, string sTopMostElementName, string sTopMostElementValue, string sDataPage, string sOnAfterAskToSave, string sRenderAllLevelsDataPage)
		{
	
	
			//-----create an add-function for each level------------------
			efHtmlScript oScript = new efHtmlScript();
	
	
			//------------------get the top-most level--------------------
			XmlNodeList oLevelNode = oXmlMultiStructure.selectNodes("//level");
			if (oLevelNode == null)
			{
				throw (new efException("At least one level needed."));
			}
	
			//------------------get level name and height ------------------------
			string sLevelName;
	
			if (oLevelNode[0]["name"].sText == "")
			{
				throw (new efException("Multi-structure node \"level\" needs attribute \"name\""));
			}
			else
			{
				sLevelName = oLevelNode[0]["name"].sText;
			}
	
	
	
			//------------------------default-empty line------------------------
	
			string sNewButtonCaption;
			if (oLevelNode[0].selectSingleNode("newbutton") == null == false)
			{
				sNewButtonCaption = oLevelNode[0].selectSingleNode("newbutton").sText;
			}
			else
			{
				sNewButtonCaption = "Neu";
			}
	
	
			//------------------if paged, get page size------------------------
			int lPageSize = 0;
			if (!Functions.IsEmptyString(oXmlMultiStructure.selectSingleNode("/multistructure")["pagesize"].sText))
			{
				lPageSize = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXmlMultiStructure.selectSingleNode("/multistructure")["pagesize"].sText, 0);
			}
	
			//------------------------generate call of init-javascript------------------------
			oScript.sCode = "msGetMultiInitHtml('" + sParentDiv + "'," + "'" + sLevelName + "'," + "'" + sFormName + "'," + "'" + sMultiStructureXmlFilename + "'," + "'" + sTopMostElementName + "'," + "'" + sTopMostElementValue + "'," + "'" + sDataPage + "'," + "'" + sNewButtonCaption + "'," + "true" + "," + lPageSize + "," + 1 + "," + sOnAfterAskToSave + "," + "'" + sRenderAllLevelsDataPage + "'" + "" + ");";
	
			FastString oResult = new FastString();
			oScript.gRender(oResult, 1);
	
	
	
			return oResult.ToString();
	
		}




		//================================================================================
		//Function:  gsRenderSpecificLevels
		//--------------------------------------------------------------------------------'
		//Purpose:   renders the concrete html of a given level
		//--------------------------------------------------------------------------------'
		//Params:
		//           sNamePraefix - usually the level-id
		//
		//--------------------------------------------------------------------------------'
		//Returns:   there is a certain format return:
		//
		//           1. the levelname of the dialog-html + ";"
		//           2. the width of the dialog-html + ";"
		//           3. the height of the dialog-html + ";"
		//           4. the html of the xml-dialog + "-||-"

		//--------------------------------------------------------------------------------'
		//Created:   02.05.2004 19:52:22
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsRenderSpecificLevel(ClientInfo oClientInfo, XmlDocument oXmlMultiStructure, string sHtmlFormName, string sXmlDialogId, string sNamePraefix, string sLevelName, System.Web.HttpRequest oRequest, System.Web.HttpApplicationState oApplication, System.Web.HttpServerUtility oServer, XmlDocument oXmlDialogData)
		{
	
	
			FastString oResult = new FastString();
	
			//-------------------------------get start node--------------------------
			XmlNode oLevelToRender;
			oLevelToRender = oXmlMultiStructure.selectSingleNode("//level[@name='" + sLevelName + "']");
	
	
			if (oLevelToRender == null)
			{
				throw (new efException("The start-level \"" + sLevelName + "\" wasn't found!"));
			}
	
			//-----------------------------load xml-definition of xml-dialog---------------------
	
			XmlDocument oXmlDlg = new XmlDocument();
			if (oLevelToRender.selectSingleNode("xmldialog") == null)
			{
				throw (new efException("Field \"xmldialog\" needed for multi-structure level \"" + sLevelName + "\"!"));
			}
			oXmlDlg.gLoad(ASPTools.Tools.sWebToAbsoluteFilename(oRequest, oLevelToRender.selectSingleNode("xmldialog").sText, false));
	
			//-----------------------------render xml-dialog-------------------------------------
			XmlDialogRenderer oDlgRenderer = new XmlDialogRenderer();
	
	
			//------------------------------return the next level's name,, width and height--------------------------
			string sLevelWidth;
			string sLevelHeight;
			string sNewButtonCaption;
	
			//------name-----------
			if (Functions.IsEmptyString(oLevelToRender["name"].sText))
			{
				throw (new efException("All level-nodes require a name-element."));
			}
			else
			{
				sLevelName = oLevelToRender["name"].sText;
			}
			//-------width----------
			if (oLevelToRender.selectSingleNode("width") == null)
			{
				throw (new efException("All level-nodes require a width-element."));
			}
			else
			{
				sLevelWidth = oLevelToRender.selectSingleNode("width").sText;
			}
			//------height----------
			if (oLevelToRender.selectSingleNode("height") == null)
			{
				throw (new efException("All level-nodes require a height-element."));
			}
			else
			{
				sLevelHeight = oLevelToRender.selectSingleNode("height").sText;
			}
			//------caption of new button----------
			if (oLevelToRender.selectSingleNode("newbutton") == null)
			{
				sNewButtonCaption = "Neu";
			}
			else
			{
				sNewButtonCaption = oLevelToRender.selectSingleNode("newbutton").sText;
			}
	
			//-------get option values----
			string sOptionDialogText = gsRenderOptionValuesForNewButton(oClientInfo, oXmlMultiStructure, sLevelName, efEnumLevel.thisLevel);
	
	
			//---------------if there are several levels in the same hierarchy, then optiondialog is mandatory-----
			XmlNodeList nlSubLevels = oLevelToRender.selectNodes("level");
			if (nlSubLevels.lCount > 1 & oLevelToRender.selectSingleNode("optiondialog") == null)
			{
				throw (new efException("If there are several levels on the same hierarchy, then there must be an option-dialog."));
			}
			string sNextSubLevelName = "";
			string sNextSubLevelNewButtonCaption = "";
			string sOptionsOfNextSubElement = "";
			if (nlSubLevels.lCount == 1)
			{
				sNextSubLevelName = nlSubLevels[0]["name"].sText;
				sOptionsOfNextSubElement = ";;";
				sNextSubLevelNewButtonCaption = nlSubLevels[0].selectSingleNode("newbutton").sText;
		
			}
			else if (nlSubLevels.lCount > 0)
			{
				sNextSubLevelName = "";
				if (oLevelToRender.selectSingleNode("optiondialog/newbutton") == null)
				{
					throw (new efException("In a top-element in the element \"option-dialog\" the element \"newbutton\" is required."));
				}
				sNextSubLevelNewButtonCaption = oLevelToRender.selectSingleNode("optiondialog/newbutton").sText;
		
				//--------get option-values of the next sub-elements, if there are severals----------
				sOptionsOfNextSubElement = gsRenderOptionValuesForNewButton(oClientInfo, oXmlMultiStructure, sLevelName, efEnumLevel.sublevel);
			}
			else if (nlSubLevels.lCount == 0)
			{
				sOptionsOfNextSubElement = ";;";
		
			}
	
			//----------------get sortfield and sort-buttons-------------------
			string sSortField = "";
			string sIconMoveUp = "";
			string sIconMoveDown = "";
			if (oLevelToRender.selectSingleNode("sortfield") != null )
			{
				XmlNode oNodeSortField = oLevelToRender.selectSingleNode("sortfield");
				sSortField = oNodeSortField.sText;
				sIconMoveUp = oNodeSortField["iconup"].sText;
				sIconMoveDown = oNodeSortField["icondown"].sText;
		
				sIconMoveUp = easyFramework.Frontend.ASP.ASPTools.Images.sGetImageURL(oClientInfo, sIconMoveUp, oClientInfo.oHttpApp.sApplicationPath());
				sIconMoveDown = easyFramework.Frontend.ASP.ASPTools.Images.sGetImageURL(oClientInfo, sIconMoveDown, oClientInfo.oHttpApp.sApplicationPath());
		
			}
	
	
			//------------------write string-results--------------------------
			oResult.Append("OK-||-");
			oResult.Append(sNamePraefix + ";");
			oResult.Append(sLevelName + ";");
			oResult.Append(sLevelHeight + ";");
			oResult.Append(sLevelWidth + ";");
			oResult.Append(Functions.Replace(sNewButtonCaption, ";", " ") + ";");
			oResult.Append(sOptionDialogText + ";"); //the semicolons are added in function  gsRenderOptionValuesForNewButton
			oResult.Append(sNextSubLevelName + ";");
			oResult.Append(Functions.Replace(sNextSubLevelNewButtonCaption, ";", "") + ";");
			oResult.Append(sOptionsOfNextSubElement + ";");
			oResult.Append(sSortField + ";");
			oResult.Append(sIconMoveUp + ";");
			oResult.Append(sIconMoveDown + ";");
			oResult.Append(XmlDialogRenderer.gsRender(oClientInfo, oXmlDlg, oXmlDialogData, sHtmlFormName, sXmlDialogId, sNamePraefix, ""));
	
	
			//-----------------increment name-praefix, so that all levels have unique-names; is
			//-----------------equivalent to the function msGetNextLevelId in efMultiStructure.js
			sNamePraefix += "1_"; //next elements always start at 1
	
	
	
			return oResult.ToString();
	
	
	
		}


		//================================================================================
		//Function:  gsRenderOptionValuesForNewButton
		//--------------------------------------------------------------------------------'
		//Purpose:   this function retrieves all possible options, which can be chosen,
		//           if a new button is clicked; it could be, that different levels
		//           can be selected
		//--------------------------------------------------------------------------------'
		//Params:    oClientInfo -
		//           oXmlMultiStructure - the xml of the multistructure-definition
		//           sLevelName - the name of the level, whose new-button shall be got
		//           enLevel - if enLevel is "thisLevel", then the new button to create
		//                     other sLevelNames on the same hierarchy is returned
		//                     if enLevel is "sublevel", then the new button of creating
		//                     a sub-element is returned. sub-elements can have some
		//                     special behaviours, because it could be, that the
		//                     sub-levels open up an option-box, from which the user
		//                     has to choose, which of the sub-levels has to be rendered
		//
		//--------------------------------------------------------------------------------'
		//Returns:   option-text, option-values, option-captions (semi-colon separated)
		//
		//           example:
		//           "Please select one of the following entries:;typeA-||-type B-||-;Class A (simple)-||-Class B-||-
		//--------------------------------------------------------------------------------'
		//Created:   31.05.2004 00:16:17
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsRenderOptionValuesForNewButton(ClientInfo oClientInfo, XmlDocument oXmlMultiStructure, string sLevelName, efEnumLevel enLevelHierarchyToCreate)
		{
	
			XmlNode oOptionDialogToRender;
			if (enLevelHierarchyToCreate == efEnumLevel.sublevel)
			{
				oOptionDialogToRender = oXmlMultiStructure.selectSingleNode("//level[@name='" + sLevelName + "']/optiondialog");
			}
			else
			{
				oOptionDialogToRender = oXmlMultiStructure.selectSingleNode("//level[@name='" + sLevelName + "']/parent::*/optiondialog");
			}
	
			if (oOptionDialogToRender == null)
			{
				return ";;";
			}
			else
			{
		
				string sOptionDialogText = "";
				string sOptionDialogCaption = ""; //the text of the level in an option-dialog
				string sOptionDialogValue = ""; //the text of the level in an option-dialog
		
				XmlNodeList nlSubOptionDialogs = oOptionDialogToRender.selectNodes("parent::*/child::*/optiondialog");
				for (int i = 0; i <= nlSubOptionDialogs.lCount - 1; i++)
				{
			
					if (oOptionDialogToRender.selectSingleNode("text") == null == false)
					{
						sOptionDialogText = oOptionDialogToRender.selectSingleNode("text").sText;
					}
			
					if (nlSubOptionDialogs[i].selectSingleNode("caption") == null)
					{
						throw (new efException("Optiondialog-element requires \"caption\"-element!"));
					}
			
					sOptionDialogValue += Functions.Replace(nlSubOptionDialogs[i].oParent["name"].sText, ";", " ");
					sOptionDialogCaption += Functions.Replace(nlSubOptionDialogs[i].selectSingleNode("caption").sText, ";", " ");
			
					if (i < nlSubOptionDialogs.lCount - 1)
					{
						sOptionDialogCaption += "-||-";
						sOptionDialogValue += "-||-";
					}
				}
		
				//-------------------------build result-string---------------------
				string sResult;
				sResult = sOptionDialogText + ";" + sOptionDialogValue + ";" + sOptionDialogCaption;
		
		
				return sResult;
			}
	
		}

	}

}
