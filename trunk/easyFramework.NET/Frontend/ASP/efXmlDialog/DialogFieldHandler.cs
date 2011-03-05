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
	/// <summary>

	//================================================================================
	//Class:	DialogFieldHandler
	//--------------------------------------------------------------------------------
	//Module:	DialogFieldHandler.cs
	//--------------------------------------------------------------------------------
	//Copyright:Promain Software-Betreuung GmbH, 2004	
	//--------------------------------------------------------------------------------
	//Purpose:	renders a dialog-field of a dialog
	//--------------------------------------------------------------------------------
	//Created:	04.09.2004 18:24:25 Marc Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	//Changed:	
	//--------------------------------------------------------------------------------

	/// </summary>
	public class DialogFieldHandler
	{




		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Class:	<DialogFieldHandler>
		//--------------------------------------------------------------------------------
		//Module:	DialogFieldHandler.cs
		//--------------------------------------------------------------------------------
		//Copyright:Promain Software-Betreuung GmbH, 2004	
		//--------------------------------------------------------------------------------
		//Purpose:	constructor
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:26:19 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public DialogFieldHandler()
		{
			

		}



		//================================================================================
		//Function:		<Render_INPUT>
		//--------------------------------------------------------------------------------
		//Purpose:		renders an INPUT-element
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:25:33 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_INPUT(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer,
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{

			//check, if there is a field-element which shall have the focus set:
			if (Functions.IsEmptyString(sElementNameToFocus) & !oDialogDefFieldNode.sGetValue("TYPE").Equals("HIDDEN"))
			{
				sElementNameToFocus = sElementName;
			}

			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "TEXT";
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
					
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
			if (bIsEntity)
			{
				sElementName += XmlDialogRenderer.efsEntityToStringValue;
			}
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["ID"].sValue = sElementName;
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;

			if (oDialogDefFieldNode.selectSingleNode("MAXLENGTH") != null) 
			{
				oHtmlInput["MAXLENGTH"].sValue = oDialogDefFieldNode.selectSingleNode("MAXLENGTH").sText;
			}
					
			if (sInputType == "DISABLED")
			{
				oHtmlInput["DISABLED"].sValue = "True";
			}
					
			//----------handle events----------
			XmlDialogRenderer.mBuildEventHandlers(oHtmlInput, oDialogDefFieldNode, oJavaScriptContainer);

					
					
					
			//-----------handle entity---------------------
			if (bIsEntity == false)
			{
				oHtmlInput["STYLE"].sValue = "width:100%";
			}
			else
			{
				oHtmlInput["STYLE"].sValue = "width:85%";
						
				string sElementNameKeyValue = Functions.Left(sElementName, Functions.Len(sElementName) - Functions.Len(XmlDialogRenderer.efsEntityToStringValue));
						
						
				//------create wrapper function: gUpdateEntityValue_ElementName --------
				//------this one is called by efDlgParams.js, when loading values into
				//------the dialog. the toString-value is updated also
				oJavaScriptContainer.Append("\n" + "function gUpdateEntityValue_" + sElementNameKeyValue + "() {" + "\n" + "   gUpdateEntityValue(\"" + sFormName + "\",\"" + sElementNameKeyValue + "\",\"" + oEntity.sName + "\",\"" + oClientInfo.oHttpApp.sApplicationPath() + "\");" + "\n" + "}");
						
						
				//--------render popup-------------
				//TODO render popup
				string sPopupMenueID = "PopupMenue" + sElementNameKeyValue;
				EntityPopupMenueRenderer oPopupmenue = new EntityPopupMenueRenderer(oEntity, oClientInfo.oHttpApp.sApplicationPath());
						
				oPopupmenue.sJavaFuncAfterSearch = XmlDialogRenderer.msGetJavaFunc_AfterSearchEntity(oClientInfo.oHttpApp.sApplicationPath(), oEntity, sFormName, sElementNameKeyValue, oHtmlTd);
				oPopupmenue.sJavaFuncToGetEntityId =XmlDialogRenderer.msGetJavaFunc_GetEntityId(sFormName, sElementNameKeyValue);
				oPopupmenue.oEntity = oEntity;
						
						
				FastString oFastString = new FastString();
						
				oJavaScriptContainer.Append("\n" + oPopupmenue.Render(oClientInfo, sPopupMenueID, Unit.Parse("140px"), true));
						
				//--------create hidden-element for storing the entity-id----------
				efHtmlInput oHtmlHiddenEntityID = new efHtmlInput(oHtmlTd);
				oHtmlHiddenEntityID["TYPE"].sValue = "hidden";
				oHtmlHiddenEntityID["NAME"].sValue = sElementNameKeyValue;
				oHtmlHiddenEntityID["ONCHANGE"].sValue = "gHandleEntityHiddenChange(this);";
						
						
				//-----display small-button--------
				efHtmlButton oHtmlBtn = new efHtmlButton(oHtmlTd);
				oHtmlBtn.sText = "...";
				oHtmlBtn["onclick"].sValue = "gShowPopup_" + sPopupMenueID + "();";
				oHtmlBtn["type"].sValue = "button";
				oHtmlBtn["class"].sValue = "cmdButtonSmall";
						
				//-----store the original-value of key--------
				efHtmlInput oHtmlOriginalValueOfEntity = new efHtmlInput(oParentRow);
				oHtmlOriginalValueOfEntity["TYPE"].sValue = "hidden";
				oHtmlOriginalValueOfEntity["NAME"].sValue = sElementNameKeyValue + "_old";
				oHtmlOriginalValueOfEntity["VALUE"].sValue = sDialogDataFieldValue;
						
				//-----value-on-focus-flag, if changed--------
				efHtmlInput oHtmlEntityChanged = new efHtmlInput(oParentRow);
				oHtmlEntityChanged["TYPE"].sValue = "hidden";
				oHtmlEntityChanged["NAME"].sValue = sElementNameKeyValue + XmlDialogRenderer.efsEntityToStringValueOnFocus;
				oHtmlEntityChanged["VALUE"].sValue = "";
						
				//-----apply onBlur-event----------------
				oHtmlInput["onblur"].sValue = XmlDialogRenderer.msGetOnEntityInputBlur(oClientInfo.oHttpApp.sApplicationPath(), sFormName, sElementNameKeyValue, oEntity.sName);
						
				//----apply onFocus-event---------------
				oHtmlInput["onfocus"].sValue = XmlDialogRenderer.msGetOnEntityInputFocus(oClientInfo.oHttpApp.sApplicationPath(), sFormName, sElementNameKeyValue, oEntity.sName);
						
			}
					
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}

					
			//------------set dirty-flag-handler--------------
			string	sOnChange = oHtmlInput["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = "";
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlInput["ONCHANGE"].sValue = sOnChange;


		}


		//================================================================================
		//Function:		<Render_DISABLED>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:36:03 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_DISABLED(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "TEXT";
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
					
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
			if (bIsEntity)
			{
				sElementName += XmlDialogRenderer.efsEntityToStringValue;
			}
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["ID"].sValue = sElementName;
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;
					
			if (sInputType == "DISABLED")
			{
				oHtmlInput["DISABLED"].sValue = "True";
			}
					
					
					
					
					
			//-----------handle entity---------------------
			if (bIsEntity == false)
			{
				oHtmlInput["STYLE"].sValue = "width:100%";
			}
			else
			{
				oHtmlInput["STYLE"].sValue = "width:85%";
						
				string sElementNameKeyValue = Functions.Left(sElementName, Functions.Len(sElementName) - Functions.Len(XmlDialogRenderer.efsEntityToStringValue));
						
						
				//------create wrapper function: gUpdateEntityValue_ElementName --------
				//------this one is called by efDlgParams.js, when loading values into
				//------the dialog. the toString-value is updated also
				oJavaScriptContainer.Append("\n" + "function gUpdateEntityValue_" + sElementNameKeyValue + "() {" + "\n" + "   gUpdateEntityValue(\"" + sFormName + "\",\"" + sElementNameKeyValue + "\",\"" + oEntity.sName + "\",\"" + oClientInfo.oHttpApp.sApplicationPath() + "\");" + "\n" + "}");
						
						
				//--------render popup-------------
				//TODO render popup
				string sPopupMenueID = "PopupMenue" + sElementNameKeyValue;
				EntityPopupMenueRenderer oPopupmenue = new EntityPopupMenueRenderer(oEntity, oClientInfo.oHttpApp.sApplicationPath());
						
				oPopupmenue.sJavaFuncAfterSearch = XmlDialogRenderer.msGetJavaFunc_AfterSearchEntity(oClientInfo.oHttpApp.sApplicationPath(), oEntity, sFormName, sElementNameKeyValue, oHtmlTd);
				oPopupmenue.sJavaFuncToGetEntityId = XmlDialogRenderer.msGetJavaFunc_GetEntityId(sFormName, sElementNameKeyValue);
				oPopupmenue.oEntity = oEntity;
						
						
				FastString oFastString = new FastString();
						
				oJavaScriptContainer.Append("\n" + oPopupmenue.Render(oClientInfo, sPopupMenueID, Unit.Parse("140px"), true));
						
				//--------create hidden-element for storing the entity-id----------
				efHtmlInput oHtmlHiddenEntityID = new efHtmlInput(oHtmlTd);
				oHtmlHiddenEntityID["TYPE"].sValue = "hidden";
				oHtmlHiddenEntityID["NAME"].sValue = sElementNameKeyValue;
				oHtmlHiddenEntityID["ONCHANGE"].sValue = "gHandleEntityHiddenChange(this);";
						
						
				//-----display small-button--------
				efHtmlButton oHtmlBtn = new efHtmlButton(oHtmlTd);
				oHtmlBtn.sText = "...";
				oHtmlBtn["onclick"].sValue = "gShowPopup_" + sPopupMenueID + "();";
				oHtmlBtn["type"].sValue = "button";
				oHtmlBtn["class"].sValue = "cmdButtonSmall";
						
				//-----store the original-value of key--------
				efHtmlInput oHtmlOriginalValueOfEntity = new efHtmlInput(oParentRow);
				oHtmlOriginalValueOfEntity["TYPE"].sValue = "hidden";
				oHtmlOriginalValueOfEntity["NAME"].sValue = sElementNameKeyValue + "_old";
				oHtmlOriginalValueOfEntity["VALUE"].sValue = sDialogDataFieldValue;
						
				//-----value-on-focus-flag, if changed--------
				efHtmlInput oHtmlEntityChanged = new efHtmlInput(oParentRow);
				oHtmlEntityChanged["TYPE"].sValue = "hidden";
				oHtmlEntityChanged["NAME"].sValue = sElementNameKeyValue + XmlDialogRenderer.efsEntityToStringValueOnFocus;
				oHtmlEntityChanged["VALUE"].sValue = "";
						
				//-----apply onBlur-event----------------
				oHtmlInput["onblur"].sValue = XmlDialogRenderer.msGetOnEntityInputBlur(oClientInfo.oHttpApp.sApplicationPath(), sFormName, sElementNameKeyValue, oEntity.sName);
						
				//----apply onFocus-event---------------
				oHtmlInput["onfocus"].sValue = XmlDialogRenderer.msGetOnEntityInputFocus(oClientInfo.oHttpApp.sApplicationPath(), sFormName, sElementNameKeyValue, oEntity.sName);
						
			}
					
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
					
			//----------add dirty-flag-eventhandler------------
			oHtmlInput["ONCHANGE"].sValue = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);";
		}


		//================================================================================
		//Function:		<Render_DATE>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:36:13 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_DATE(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "TEXT";
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
					
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
			if (bIsEntity)
			{
				sElementName += XmlDialogRenderer.efsEntityToStringValue;
			}
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["ID"].sValue = sElementName;
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;
					
			if (sInputType == "DISABLED")
			{
				oHtmlInput["DISABLED"].sValue = "True";
			}
					

			oHtmlInput["ONBLUR"].sValue = "formatDate(this);";
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlInput["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlInput["ONCHANGE"].sValue = sOnChange;

		}


		//================================================================================
		//Function:		<Render_NUMBER>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:53:00 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_NUMBER(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{

			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "TEXT";
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
					
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
			if (bIsEntity)
			{
				sElementName += XmlDialogRenderer.efsEntityToStringValue;
			}
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["ID"].sValue = sElementName;
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;
					
			if (sInputType == "DISABLED")
			{
				oHtmlInput["DISABLED"].sValue = "True";
			}
					
			oHtmlInput["ONBLUR"].sValue = "this.value=checkNumber(this.value);";
					
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlInput["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
		
			oHtmlInput["ONCHANGE"].sValue = sOnChange;

		}




		//================================================================================
		//Function:		<Render_LABEL>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:36:31 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_LABEL(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlSpan oSpan = new efHtmlSpan(oHtmlTd);
			oSpan["CLASS"].sValue = sCss;
			efHtmlTextNode oHtmlText = new efHtmlTextNode(oSpan);
			oHtmlText.sText = sDialogDataFieldValue;
			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "HIDDEN";
					
			sElementName = "lbl" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;

		}


		//================================================================================
		//Function:		<Render_INPUTFIRST>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:36:59 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_INPUTFIRST(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "TEXT";
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
																							   
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["ID"].sValue = sElementName;
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlInput["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlInput["ONCHANGE"].sValue = sOnChange;

		}



		//================================================================================
		//Function:		<Render_PASSWORD>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:36:49 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_PASSWORD(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "PASSWORD";
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
			
			//----------handle events----------
			XmlDialogRenderer.mBuildEventHandlers(oHtmlInput, oDialogDefFieldNode, oJavaScriptContainer);


			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;
			oHtmlInput["ID"].sValue = sElementName;
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlInput["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlInput["ONCHANGE"].sValue = sOnChange;

		

		}


		//================================================================================
		//Function:		<Render_HIDDEN>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:36:40 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_HIDDEN(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlInput oHtmlInput = new efHtmlInput(oParentRow);
			oHtmlTd.gDetachFromParent(); //not needed
			oHtmlInput["TYPE"].sValue = "HIDDEN";
					
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlInput["NAME"].sValue = sElementName;
			oHtmlInput["ID"].sValue = sElementName;
					
			oHtmlInput["VALUE"].sValue = sDialogDataFieldValue;
			
		}


		//================================================================================
		//Function:		<Render_TEXTAREA>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:37:09 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_TEXTAREA(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlTextarea oHtmlTextArea = new efHtmlTextarea(oHtmlTd);
					
			sElementName = "txt" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			if (!Functions.IsEmptyString(sCols))
			{
				oHtmlTextArea["COLS"].sValue = sCols;
			}
			if (!Functions.IsEmptyString(sRows))
			{
				oHtmlTextArea["ROWS"].sValue = sRows;
			}
					
					
			oHtmlTextArea["NAME"].sValue = sElementName;
			oHtmlTextArea.sText = sDialogDataFieldValue;
			if (bReadonly)
			{
				oHtmlTextArea["DISABLED"].sValue = "true";
			}
			if (Functions.IsEmptyString(oHtmlTextArea["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlTextArea["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlTextArea["ONCHANGE"].sValue = sOnChange;

		}


		//================================================================================
		//Function:		<Render_CHECKBOX>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:37:17 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_CHECKBOX(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlInput oHtmlInput = new efHtmlInput(oHtmlTd);
			oHtmlInput["TYPE"].sValue = "CHECKBOX";
					
			sElementName = "chk" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlInput["ID"].sValue = sElementName;
					
			oHtmlInput["NAME"].sValue = sElementName;
					
			if (bReadonly)
			{
				oHtmlInput["DISABLED"].sValue = "true";
			}
					
			if (sDialogDataFieldValue != "0")
			{
				oHtmlInput["CHECKED"].sValue = "True";
				sDialogDataFieldValue = "1"; //normalize bool-value
						
			}
			if (Functions.IsEmptyString(oHtmlInput["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlInput["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlInput["ONCHANGE"].sValue = sOnChange;


		}


		//================================================================================
		//Function:		<Render_LISTCOMBO>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:37:25 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_LISTCOMBO(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{

			efHtmlSelect oHtmlSelect = new efHtmlSelect(oHtmlTd);
					
			sElementName = "cbo" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlSelect["NAME"].sValue = sElementName;
			oHtmlSelect["ID"].sValue = sElementName;
					
			if (bReadonly)
			{
				oHtmlSelect["DISABLED"].sValue = "true";
			}
					
			//check, wether the src and data-field also exist:
			if (oDialogDefFieldNode.selectSingleNode("SRC") == null)
			{
				throw (new RenderException("Invalid field-definition - SRC-element is missing for " + oHtmlSelect["NAME"].sValue));
			}
			if (oDialogDefFieldNode.selectSingleNode("DATA") == null)
			{
				throw (new RenderException("Invalid field-definition - DATA-element is missing for " + oHtmlSelect["NAME"].sValue));
			}
					
			string sDataValues = oDialogDefFieldNode.selectSingleNode("DATA").sText;
			string sSrcValues = oDialogDefFieldNode.selectSingleNode("SRC").sText;
					
			if (Functions.InStr(sDataValues, ";") == 0)
			{
				if (Functions.InStr(sSrcValues, ";") > 0)
				{
					throw (new RenderException("DATA and SRC arguments do not match for" + oHtmlSelect["NAME"].sValue));
				}
						
				oHtmlSelect.gAddOption(sDataValues, sSrcValues);
						
			}
			else
			{
				if (Functions.InStr(sSrcValues, ";") == 0)
				{
					throw (new RenderException("DATA and SRC arguments do not match for" + oHtmlSelect["NAME"].sValue));
				}
				else
				{
					if (Functions.Split(sSrcValues, ";").Length != Functions.Split(sDataValues, ";").Length)
					{
						throw (new RenderException("DATA and SRC arguments do not match for" + oHtmlSelect["NAME"].sValue));
								
					}
				}
						
				for (int i = 0; i < Functions.Split(sSrcValues, ";").Length; i++)
				{
							
					oHtmlSelect.gAddOption(Functions.Split(sDataValues, ";")[i], Functions.Split(sSrcValues, ";")[i]);
							
				}
						
						
			}
					
			//-------------set the selected-value, if given------------------
			if (!Functions.IsEmptyString(sDialogDataFieldValue))
			{
				oHtmlSelect.sSelectedValue = sDialogDataFieldValue;
			}
					
			//------------styling-------------------------
			oHtmlSelect["style"].sValue = "width:100%";
					
			//------------is it first-element to focus?-------------------------
			if (Functions.IsEmptyString(oHtmlSelect["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlSelect["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlSelect["ONCHANGE"].sValue = sOnChange;

		}


		//================================================================================
		//Function:		<Render_DATACOMBO>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:37:35 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_DATACOMBO(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{
			efHtmlSelect oHtmlSelect = new efHtmlSelect(oHtmlTd);
		
			sElementName = "cbo" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlSelect["NAME"].sValue = sElementName;
			oHtmlSelect["ID"].sValue = sElementName;
					
			string sSource = "";
			string sDomain = "";
			Recordset rsData = null;
			

			if (bReadonly)
			{
				oHtmlSelect["DISABLED"].sValue = "true";
			}

			//----------handle events----------
			XmlDialogRenderer.mBuildEventHandlers(oHtmlSelect, oDialogDefFieldNode, oJavaScriptContainer);


			//-------------check, wether the src a also exist--------------
			if (oDialogDefFieldNode.selectSingleNode("SRC") == null & oDialogDefFieldNode.selectSingleNode("DOMAIN") == null)
			{
				throw (new RenderException("Invalid field-definition - SRC and DOMAIN-element is missing for " + oHtmlSelect["NAME"].sValue + ". Please specify either an SRC or DOMAIN!"));
			}
					
			if (oDialogDefFieldNode.selectSingleNode("SRC") == null == false)
			{
				sSource = oDialogDefFieldNode.selectSingleNode("SRC").sText;
			}
			if (oDialogDefFieldNode.selectSingleNode("DOMAIN") == null == false)
			{
				sDomain = oDialogDefFieldNode.selectSingleNode("DOMAIN").sText;
			}
					
			//--------------get the data into rsData ----------------------
			if (oClientInfo == null)
			{
				throw (new RenderException("Client-Info-Object needed, when using DATACOMBO. Please " + "set the property!"));
			}
					
					
			if (!Functions.IsEmptyString(sSource))
			{
						
				Recordset rsDataCombo = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDataCombo", "dc_qry", "dc_name='" + DataTools.SQLString(sSource) + "'", "", "", "");
						
				if (rsDataCombo.EOF)
				{
					throw (new RenderException("The datasource \"" + sSource + "\" wasn't found in tsDataCombos."));
				}
						
				string sQry = rsDataCombo["dc_qry"].sValue;
				if (Functions.IsEmptyString(sQry))
				{
					throw (new RenderException("Query not set for " + sSource + ". Please set in tsDataCombos!"));
				}
						
				rsData = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
						
				if (rsData.oFields.Count != 2)
				{
					throw (new RenderException("Invalid select-statement for datacombo " + sSource + ". There have to be exactly 2 fields: the first-one contains " + " a key-value, the second-one contains the display-value. Please correct!"));
				}
						
			}
			else if (!Functions.IsEmptyString(sDomain))
			{
						
				rsData = Domains.grsGetDomainsForComboBox(oClientInfo, sDomain);
						
			}
					
			//------------build options--------------------
					
			if (sInputType == "DATACOMBON")
			{
				oHtmlSelect.gAddOption("", "");
			}
					
			while (! rsData.EOF)
			{
						
				oHtmlSelect.gAddOption(rsData.oFields[0].sValue, rsData.oFields[1].sValue);
						
				rsData.MoveNext();
						
			};
					
			//----------set the selected-value, if given-----------------------
			if (!Functions.IsEmptyString(sDialogDataFieldValue))
			{
				oHtmlSelect.sSelectedValue = sDialogDataFieldValue;
			}
					
					
			//------------styling--------------------
			oHtmlSelect["style"].sValue = "width:100%";
					
			//------------is it first-element to focus?-------------------------
			if (Functions.IsEmptyString(oHtmlSelect["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlSelect["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlSelect["ONCHANGE"].sValue = sOnChange;
		}


		//================================================================================
		//Function:		<Render_DATACOMBON>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:37:45 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_DATACOMBON(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss) 
		{

			efHtmlSelect oHtmlSelect = new efHtmlSelect(oHtmlTd);
			
			sElementName = "cbo" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
			oHtmlSelect["NAME"].sValue = sElementName;
			oHtmlSelect["ID"].sValue = sElementName;
					
			if (bReadonly)
			{
				oHtmlSelect["DISABLED"].sValue = "true";
			}

			//----------handle events----------
			XmlDialogRenderer.mBuildEventHandlers(oHtmlSelect, oDialogDefFieldNode, oJavaScriptContainer);


			//-------------check, wether the src a also exist--------------
			if (oDialogDefFieldNode.selectSingleNode("SRC") == null & oDialogDefFieldNode.selectSingleNode("DOMAIN") == null)
			{
				throw (new RenderException("Invalid field-definition - SRC and DOMAIN-element is missing for " + oHtmlSelect["NAME"].sValue + ". Please specify either an SRC or DOMAIN!"));
			}
					
					
			string sSource = "";
			string sDomain = "";
			Recordset rsData = null;

			if (oDialogDefFieldNode.selectSingleNode("SRC") == null == false)
			{
				sSource = oDialogDefFieldNode.selectSingleNode("SRC").sText;
			}
			if (oDialogDefFieldNode.selectSingleNode("DOMAIN") == null == false)
			{
				sDomain = oDialogDefFieldNode.selectSingleNode("DOMAIN").sText;
			}
					
			//--------------get the data into rsData ----------------------
			if (oClientInfo == null)
			{
				throw (new RenderException("Client-Info-Object needed, when using DATACOMBO. Please " + "set the property!"));
			}
					
					
			if (!Functions.IsEmptyString(sSource))
			{
						
				Recordset rsDataCombo = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsDataCombo", "dc_qry", "dc_name='" + DataTools.SQLString(sSource) + "'", "", "", "");
						
				if (rsDataCombo.EOF)
				{
					throw (new RenderException("The datasource \"" + sSource + "\" wasn't found in tsDataCombos."));
				}
						
				string sQry = rsDataCombo["dc_qry"].sValue;
				if (Functions.IsEmptyString(sQry))
				{
					throw (new RenderException("Query not set for " + sSource + ". Please set in tsDataCombos!"));
				}
						
				rsData = DataMethodsClientInfo.gRsGetDirect(oClientInfo, sQry);
						
				if (rsData.oFields.Count != 2)
				{
					throw (new RenderException("Invalid select-statement for datacombo " + sSource + ". There have to be exactly 2 fields: the first-one contains " + " a key-value, the second-one contains the display-value. Please correct!"));
				}
						
			}
			else if (!Functions.IsEmptyString(sDomain))
			{
						
				rsData = Domains.grsGetDomainsForComboBox(oClientInfo, sDomain);
						
			}
					
			//------------build options--------------------
					
			if (sInputType == "DATACOMBON")
			{
				oHtmlSelect.gAddOption("", "");
			}
					
			while (! rsData.EOF)
			{
						
				oHtmlSelect.gAddOption(rsData.oFields[0].sValue, rsData.oFields[1].sValue);
						
				rsData.MoveNext();
						
			};
					
			//----------set the selected-value, if given-----------------------
			if (!Functions.IsEmptyString(sDialogDataFieldValue))
			{
				oHtmlSelect.sSelectedValue = sDialogDataFieldValue;
			}
					
					
			//------------styling--------------------
			oHtmlSelect["style"].sValue = "width:100%";
					
			//------------is it first-element to focus?-------------------------
			if (Functions.IsEmptyString(oHtmlSelect["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}
					
			//------------set dirty-flag-handler--------------
			string sOnChange = oHtmlSelect["ONCHANGE"].sValue;
			if (sOnChange == null) sOnChange = ""; 
			sOnChange = "gXmlDialogSetDirty('" + sXmlDialogId + "', true);" + "\n" + sOnChange;
			oHtmlSelect["ONCHANGE"].sValue = sOnChange;
			
		}


		//================================================================================
		//Function:		<Render_BUTTON>
		//--------------------------------------------------------------------------------
		//Purpose:	
		//--------------------------------------------------------------------------------
		//Params:	
		//--------------------------------------------------------------------------------
		//Returns:	
		//--------------------------------------------------------------------------------
		//Created:	04.09.2004 18:37:54 Marc Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		//Changed:	
		//--------------------------------------------------------------------------------
		public static void Render_BUTTON(ClientInfo oClientInfo, bool bIsEntity, bool bReadonly, 
			string sElementName, efHtmlTr oParentRow, efHtmlTd oHtmlTd, int lRowNumber, bool bMultiRow, string sNamePraefix,
			string sDialogDataFieldValue, IEntity oEntity, XmlNode oDialogDefFieldNode, string sXmlDialogId, string sFormName, 
			string sInputType, FastString oJavaScriptContainer, 
			ref string sElementNameToFocus, string sRows, string sCols, string sCss, bool bDefault) 
		{

			efHtmlButton oHtmlButton = new efHtmlButton(oHtmlTd);
					
			sElementName = "cmd" + oDialogDefFieldNode.selectSingleNode("NAME").sText;
			sElementName = XmlDialogRenderer.msGetElementNamePraefix(sElementName, bMultiRow, lRowNumber, sNamePraefix);
					
					
			oHtmlButton["ID"].sValue = sElementName;
			oHtmlButton["TYPE"].sValue = "button";
					
			if (oDialogDefFieldNode.selectSingleNode("VALUE") != null)
			{
				oHtmlButton.sText = oDialogDefFieldNode.selectSingleNode("VALUE").sText;
			}
					
			//disabled button, if wanted:
			if (sInputType == "BUTTOND" | bReadonly)
			{
				oHtmlButton["DISABLED"].sValue = "True";
			}
					
			//css-style:
			string sCssStyle;
			if (oDialogDefFieldNode.selectSingleNode("BUTTONSTYLE") == null)
			{
				sCssStyle = "cmdButton";
			}
			else
			{
						
				switch (oDialogDefFieldNode.selectSingleNode("BUTTONSTYLE").sText)
				{
					case "cmdButton":
						sCssStyle = oDialogDefFieldNode.selectSingleNode("BUTTONSTYLE").sText;
						break;
								
					case "cmdButtonTriple":
						sCssStyle = oDialogDefFieldNode.selectSingleNode("BUTTONSTYLE").sText;
						break;
								
					case "cmdButtonDouble":
						sCssStyle = oDialogDefFieldNode.selectSingleNode("BUTTONSTYLE").sText;
						break;
								
					case "cmdButtonSmall":
						sCssStyle = oDialogDefFieldNode.selectSingleNode("BUTTONSTYLE").sText;
						break;

					default:
						throw (new RenderException("invalid button-type: " + oDialogDefFieldNode.selectSingleNode("BUTTONTYPE").sText + "for element " + sElementName));
				}
			}

			oHtmlButton["CLASS"].sValue = sCssStyle;
		

			//----------handle events----------
			XmlDialogRenderer.mBuildEventHandlers(oHtmlButton, oDialogDefFieldNode, oJavaScriptContainer);


			//-----if it is an submit or reset button then call form.submit and form.reset() on click-------
			string sButtonType;
			if (oDialogDefFieldNode.selectSingleNode("BUTTONTYPE") == null)
				sButtonType = "User";
			else
				sButtonType = oDialogDefFieldNode.selectSingleNode("BUTTONTYPE").sText;
                
			if (sButtonType == "User" | sButtonType == "Submit" | sButtonType == "Reset")
			{
			
				if (sButtonType == "Submit" | sButtonType == "Reset") 
				{

					//--store the old click event in a variable; call it if not null--
					//and do the submit:
					string sVarNameOldOnClick = Tools.sGetUniqueString();
					string sFunctionOnClick = Tools.sGetUniqueString();
				
					string sFunction = "";
					if (sButtonType == "Submit") 
					{

						//don't call the real submit-function, because the onsubmit
						//shall be done before; so just call the event:
						sFunction = "if (document.forms['" + sFormName + "'].onsubmit != null) " +
							"document.forms['" + sFormName + "']" + ".onsubmit()";
					}
					else 
					{
						if (sButtonType == "Reset")
							sFunction = "document.forms['" + sFormName + "']" + ".reset()";
					}

					oJavaScriptContainer.Append("\r\n" +
						"var " + sVarNameOldOnClick + " = " +
						"document.forms['" + sFormName + "']" + ".elements['" + sElementName + "'].onclick;" + "\r\n" +
						"document.forms['" + sFormName + "']" + ".elements['" + sElementName + "'].onclick=" + sFunctionOnClick + ";" + "\r\n" +
						"function " + sFunctionOnClick + "() {" + "\r\n" +
						"   if (" + sVarNameOldOnClick + " != null) " + sVarNameOldOnClick + "();" + "\r\n"+ 
						"   " + sFunction + "; " + "\r\n" +
						"}" + "\r\n");


				}
			


			}

			if (Functions.IsEmptyString(oHtmlButton["DISABLED"].sValue))
			{
				if (Functions.IsEmptyString(sElementNameToFocus))
				{
					sElementNameToFocus = sElementName;
				}
			}


			if (bDefault) 
			{
				//if enter is pressed, then this button is to be executed;
				efHtmlScript s = new efHtmlScript();
				s.sCode = "gRegisterXmlDialogDefaultButton('$1', '$2');";
				s.sCode = s.sCode.Replace("$1", sXmlDialogId);
				s.sCode = s.sCode.Replace("$2", oHtmlButton["ID"].sValue);

				oJavaScriptContainer.Append("\r\n");
				oJavaScriptContainer.Append(s.sCode);
			}
					

		}
	}

}
