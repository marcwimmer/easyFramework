using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data.Table;

namespace easyFramework.Sys.Data.Table
{
	//================================================================================
	//Class:     Rule

	//--------------------------------------------------------------------------------'
	//Module:    Rule.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   a rule in a table-def
	//--------------------------------------------------------------------------------'
	//Created:   19.04.2004 14:36:09
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class TableDefRule
	{
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private bool mbMandatory;
		private string msMandatoryMsg;
		private TableDefRuleValue[] maoValues;
		private string msValuesMsg;
		private int mlMaxLen;
		private string msMaxLenMsg;
		private string msNumberRangeMinValue;
		private string msNumberRangeMaxValue;
		private TableDefField moParentField;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		public TableDefRule(TableDefField oField) 
		{
			moParentField = oField;
			
		}
		
		
		//================================================================================
		//Function:  gCheckValue
		//--------------------------------------------------------------------------------'
		//Purpose:   checks the given value and returns error or not
		//--------------------------------------------------------------------------------'
		//Params:    the value
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.04.2004 15:07:58
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbCheckValue(ClientInfo oClientInfo, string sValue, bool bIsNull)
		{
			
			//check mandatory:
			
			if (mbMandatory == true & Functions.IsEmptyString(sValue))
			{
				oClientInfo.gAddError(msMandatoryMsg);
			}
			else
			{
				
				//check maxlen:
				if (mlMaxLen > -1 & ! bIsNull)
				{
					if (Functions.Len(sValue) > mlMaxLen)
					{
						oClientInfo.gAddError(msMaxLenMsg);
						return false;
					}
					
					//check values:
					bool bMatchesValues = false;

					if (maoValues != null & ! bIsNull)
					{
						for (int itValues = 0; 
								itValues <= maoValues.Length - 1 & bMatchesValues == false; 
								itValues++)
						{
							
							if (maoValues[itValues].bCaseSensitive == true)
							{
								if (maoValues[itValues].sValue == sValue)
								{
									bMatchesValues = true;
								}
							}
							else
							{
								if (Functions.LCase(maoValues[itValues].sValue) == Functions.LCase(sValue))
								{
									bMatchesValues = true;
								}
							}
							
						}

						if (!bMatchesValues) 
						{
						  oClientInfo.gAddError(msValuesMsg);
						}
						
					}
					
				}
				
				//----------------check numberrange--------------------------
				if (!Functions.IsEmptyString(msNumberRangeMinValue) & ! bIsNull)
				{
					
					if (! Functions.IsNumeric(sValue))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MaxValue(moParentField.sDesc));
					}
					
					
					
					if (DataConversion.gdCDec(sValue, 0) < DataConversion.gdCDec(msNumberRangeMinValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MinValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMinValue, 0)));
					}
					
				}
				
				
				if (!Functions.IsEmptyString(msNumberRangeMaxValue) & ! bIsNull)
				{
					
					if (! Functions.IsNumeric(sValue))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MaxValue(moParentField.sDesc));
					}
					
					if (DataConversion.gdCDec(sValue, 0) > DataConversion.gdCDec(msNumberRangeMaxValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MaxValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMaxValue, 0)));
						
					}
					
				}
			}
			
			return true;
			
		}
		public bool gbCheckValue(ClientInfo oClientInfo, bool bValue, bool bValueIsNull)
		{
			
			//check mandatory:
			TableRuleException oRuleException = new TableRuleException();
			oRuleException.sFieldName = moParentField.sDesc;
			
			if (mbMandatory == true & bValueIsNull)
			{
				oClientInfo.gAddError(msMandatoryMsg);
				return false;
			}
			else
			{
				
				
				
			}
			
			return true;
			
		}
		public bool gbCheckValue(ClientInfo oClientInfo, DateTime dtValue, bool bValueIsNull)
		{
			
			//check mandatory:
			TableRuleException oRuleException = new TableRuleException();
			oRuleException.sFieldName = moParentField.sDesc;
			
			if (mbMandatory == true & bValueIsNull)
			{
				oClientInfo.gAddError(msMandatoryMsg);
				return false;
			}
			else
			{
				
				
			}
			
			return true;
			
		}
		public bool gbCheckValue(ClientInfo oClientInfo, int lValue, bool bValueIsNull)
		{
			
			//check mandatory:
			
			if (mbMandatory == true & bValueIsNull)
			{
				oClientInfo.gAddError(msMandatoryMsg);
				
			}
			else
			{
				//----------------check numberrange--------------------------
				if (!Functions.IsEmptyString(msNumberRangeMinValue ) & ! bValueIsNull)
				{
					
					
					if (lValue < DataConversion.glCInt(msNumberRangeMinValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MinValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMinValue, 0)));
					}
					
				}
				
				
				if (!Functions.IsEmptyString(msNumberRangeMaxValue) & ! bValueIsNull)
				{
					
					if (DataConversion.gdCDec(lValue, 0) > DataConversion.gdCDec(msNumberRangeMaxValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MaxValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMaxValue, 0)));
					}
					
				}
			}
			
			
			return true;
			
		}
		public bool gbCheckValue(ClientInfo oClientInfo, double dbValue, bool bValueIsNull)
		{
			
			//check mandatory:
			TableRuleException oRuleException = new TableRuleException();
			oRuleException.sFieldName = moParentField.sDesc;
			
			if (mbMandatory == true & bValueIsNull)
			{
				oClientInfo.gAddError(msMandatoryMsg);
				
			}
			else
			{
				//----------------check numberrange--------------------------
				if (!Functions.IsEmptyString(msNumberRangeMinValue) & ! bValueIsNull)
				{
					
					
					if (dbValue < DataConversion.glCInt(msNumberRangeMinValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MinValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMinValue, 0)));
						
					}
					
				}
				
				
				if (!Functions.IsEmptyString(msNumberRangeMaxValue) & ! bValueIsNull)
				{
					
					if (DataConversion.gdCDec(dbValue, 0) > DataConversion.gdCDec(msNumberRangeMaxValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MaxValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMaxValue, 0)));
						
					}
					
				}
			}
			
			return true;
			
		}
		public bool gbCheckValue(ClientInfo oClientInfo, decimal dValue, bool bValueIsNull)
		{
			
			//check mandatory:
			TableRuleException oRuleException = new TableRuleException();
			oRuleException.sFieldName = moParentField.sDesc;
			
			if (mbMandatory == true & bValueIsNull)
			{
				oClientInfo.gAddError(msMandatoryMsg);
			}
			else
			{
				
				//----------------check numberrange--------------------------
				if (!Functions.IsEmptyString(msNumberRangeMinValue) & ! bValueIsNull)
				{
					
					
					if (dValue < DataConversion.glCInt(msNumberRangeMinValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MinValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMinValue, 0)));
						
					}
					
				}
				
				
				if (!Functions.IsEmptyString(msNumberRangeMaxValue) & ! bValueIsNull)
				{
					
					if (DataConversion.gdCDec(dValue, 0) > DataConversion.gdCDec(msNumberRangeMaxValue, 0))
					{
						oClientInfo.gAddError(msGetDefaultMsg_MaxValue(moParentField.sDesc, DataConversion.glCInt(msNumberRangeMaxValue, 0)));
						
					}
					
				}
				
			}
			
			return true;
			
		}
		//================================================================================
		//Sub:       gInitFromXml
		//--------------------------------------------------------------------------------'
		//Purpose:   loads from tabledef-node
		//--------------------------------------------------------------------------------'
		//Params:    the node of the table-def document
		//--------------------------------------------------------------------------------'
		//Created:   19.04.2004 14:42:22
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gInitFromXml (XmlNode oXmlNode)
		{
			
			
			//------------default values, if there are no rules at all ---------------
			if (oXmlNode == null)
			{
				mlMaxLen = -1;
				msMaxLenMsg = msGetDefaultMsg_MaxLen(moParentField.sDesc, mlMaxLen);
				msMandatoryMsg = msGetDefaultMsg_Mandatory(moParentField.sDesc);
				msValuesMsg = msGetDefaultMsg_ValueList(moParentField.sDesc, maoValues);
				return;
			}
			
			
			
			
			//-------------maxlen?----------------------------
			if (oXmlNode.selectSingleNode("maxlen") != null)
			{
				
				if (oXmlNode.selectSingleNode("maxlen/value") == null)
				{
					throw (new TableDefParseException("element \"value\" of element \"maxlen\" is missing!"));
				}
				
				if (! Functions.IsNumeric(oXmlNode.selectSingleNode("maxlen/value").sText))
				{
					throw (new TableDefParseException("must be a number: " + oXmlNode.selectSingleNode("maxlen/value").sText));
				}
				mlMaxLen = DataConversion.glCInt(oXmlNode.selectSingleNode("maxlen/value").sText, 0);
				
			}
			else
			{
				if (moParentField.enType == RecordsetObjects.Field.efEnumFieldType.efString)
				{
					mlMaxLen = moParentField.lSize;
				}
				else
				{
					mlMaxLen = -1;
				}
			}
			
			if (oXmlNode.selectSingleNode("maxlen/msg") != null)
			{
				msMaxLenMsg = oXmlNode.selectSingleNode("maxlen/msg").sText;
				msMaxLenMsg = Functions.Replace(msMaxLenMsg, "$1", DataConversion.gsCStr(mlMaxLen));
			}
			else
			{
				msMaxLenMsg = msGetDefaultMsg_MaxLen(moParentField.sDesc, mlMaxLen);
			}
			
			
			//--------------mandatory?-----------------------------
			if (oXmlNode.selectSingleNode("mandatory") != null)
			{
				mbMandatory = true;
				if ( oXmlNode.selectSingleNode("mandatory/msg") != null)
				{
					msMandatoryMsg = oXmlNode.selectSingleNode("mandatory/msg").sText;
				}
				else
				{
					msMandatoryMsg = msGetDefaultMsg_Mandatory(moParentField.sDesc);
				}
			}
			else
			{
				mbMandatory = false;
			}
			
			//--------------numberrange-----------------------------
			if (oXmlNode.selectSingleNode("numberrange") != null)
			{
				
				msNumberRangeMinValue = "";
				msNumberRangeMaxValue = "";
				if (oXmlNode.selectSingleNode("numberrange/minvalue") != null)
				{
					msNumberRangeMinValue = oXmlNode.selectSingleNode("numberrange/minvalue").sText;
				}
				if (oXmlNode.selectSingleNode("numberrange/maxvalue") != null)
				{
					msNumberRangeMaxValue = oXmlNode.selectSingleNode("numberrange/maxvalue").sText;
				}
				
			}
			
			
			//-------------string-values----------------------------
			if ( oXmlNode.selectSingleNode("values") != null)
			{
				XmlNodeList nlValues = oXmlNode.selectNodes("values/value");
				bool bCaseSensitive;
				switch (oXmlNode.selectSingleNode("values").
					oAttributeList["casesensitive"].
					sText)
				{
					
					case "":
					
						throw (new TableDefParseException("attribute \"casesensitive\" is mandatory for value-element"));

					default:
					
						bCaseSensitive = DataConversion.gbCBool(oXmlNode.selectSingleNode("values").oAttributeList["casesensitive"].sText);
						break;
					
				}
			
				maoValues = new TableDefRuleValue[0];

				for (int y = 0; y <= nlValues.lCount - 1; y++)
				{

					TableDefRuleValue oTableDefRuleValue = new TableDefRuleValue();
					
					oTableDefRuleValue.sValue = nlValues[y].sText;
					oTableDefRuleValue.bCaseSensitive = bCaseSensitive;
					maoValues.SetValue(oTableDefRuleValue, maoValues.Length);

					
				}
			
				if (oXmlNode.selectSingleNode("values/msg") != null)
				{
					msValuesMsg = oXmlNode.selectSingleNode("values/msg").sText;
				}
				else
				{
					msValuesMsg = msGetDefaultMsg_ValueList(moParentField.sDesc, maoValues);
				}
			
			
			}
		
		}
		//================================================================================
		//Protected Properties:
		//================================================================================
	
		//================================================================================
		//Protected Methods:
		//================================================================================
		//================================================================================
		//Function:  sGetDefaultMsg_MaxLen
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if maxlen is broken
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the maxlen
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_MaxLen(string sFieldDesc, int lMaxLen)
		{
		
			return "Bitte geben Sie nicht mehr als " + DataConversion.gsCStr(lMaxLen) + " Zeichen " + "fr " + sFieldDesc + " ein!";
		
		}
	
		//================================================================================
		//Function:  msGetDefaultMsg_Values
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if not a value of the value-list
		//			 was selected
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the values
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_Values(string sFieldDesc)
		{

			string sMsg = "Es ist nur ein der bestimmter Wert " + 
					" für das Feld \"" + sFieldDesc + "\" zulässig.";
		
			return sMsg;
		
		}
		//================================================================================
		//Function:  sGetDefaultMsg_MaxLen
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if maxlen is broken
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the maxlen
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_Mandatory(string sFieldDesc)
		{
			return "Bitte geben Sie einen Wert fr " + sFieldDesc + " ein!";
		}
	
	
		//================================================================================
		//Function:  sGetDefaultMsg_ValueList
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if valuelist is broken
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the maxlen
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_ValueList(string sFieldDesc, TableDefRuleValue[] aoValueList)
		{
		
			string sValues = "";

			if (aoValueList == null) return "";

			for (int i = 0; i <= aoValueList.Length - 1; i++)
			{
				sValues += aoValueList[i].sValue;
				if (i < aoValueList.Length - 1)
				{
					sValues += ",";
				}
			}
		
		
			return "Bitte geben Sie einen der folgenden Werte fr \"" + sFieldDesc + "\" ein: " + sValues;
		
		}
	
		//================================================================================
		//Function:  sGetDefaultMsg_ValueList
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if number is too small
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the minvalue
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_MinValue(string sFieldDesc, int sMinValue)
		{
		
		
			return "Der Wert von \"" + sFieldDesc + "\" darf nicht kleiner als " + sMinValue + " sein.";
		
		}
	
		//================================================================================
		//Function:  sGetDefaultMsg_ValueList
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if number is too big
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the maxvalue
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_MaxValue(string sFieldDesc, int sMaxValue)
		{
		
		
			return "Der Wert von \"" + sFieldDesc + "\" darf nicht grer als " + sMaxValue + " sein.";
		
		}
	
	
		//================================================================================
		//Function:  sGetDefaultMsg_NotANumber
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the default-error message, if number is not a number
		//--------------------------------------------------------------------------------'
		//Params:    the description of the field; the maxvalue
		//--------------------------------------------------------------------------------'
		//Returns:   error-message
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 11:09:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static string msGetDefaultMsg_MaxValue(string sFieldDesc)
		{
		
		
			return "Der Wert von \"" + sFieldDesc + "\" mu eine Zahl sein.";
		
		}
	
		//================================================================================
		//Private Consts:
		//================================================================================
	
		//================================================================================
		//Private Fields:
		//================================================================================
	
	
	}

}
