using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data.Table
{
	//================================================================================
	//Class:     Field

	//--------------------------------------------------------------------------------'
	//Module:    Field.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   field of a table-def
	//--------------------------------------------------------------------------------'
	//Created:   19.04.2004 14:35:43
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class TableDefField
	{
		
		//================================================================================
		//Private Fields:
		//================================================================================
		protected string msName;
		protected RecordsetObjects.Field.efEnumFieldType menType;
		protected string msDesc;
		protected int mlSize;
		protected bool mbPrimaryKey;
		protected TableDefRule moRule;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		public string sName
		{
			get{
				return msName;
			}
		}
		public RecordsetObjects.Field.efEnumFieldType enType
		{
			get{
				return menType;
			}
		}
		public int lSize
		{
			get{
				return mlSize;
			}
		}
		public string sDesc
		{
			get{
				return msDesc;
			}
		}
		public TableDefRule oRule
		{
			get{
				return moRule;
			}
		}
		public bool bPrimaryKey
		{
			get{
				return mbPrimaryKey;
			}
		}
		
		
		
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		
		//================================================================================
		//Function:  goCheckValue
		//--------------------------------------------------------------------------------'
		//Purpose:   checks the given value against the rule; data-type conversion is
		//           done before
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   20.04.2004 15:09:20
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbCheckValue(ClientInfo oClientInfo, RecordsetObjects.Field oRsField)
		{
			bool bIsNull = false;
			string sValue = "";

			switch (menType)
			{
				case RecordsetObjects.Field.efEnumFieldType.efBool:
					
					
					bool bValue;
					if (oRsField == null)
					{
						bValue = false;
						bIsNull = true;
					}
					else
					{
						bValue = oRsField.bValue;
						bIsNull = oRsField.bIsNull();
					}
					
					return moRule.gbCheckValue(oClientInfo, bValue, bIsNull);
					
					
					
					
				case RecordsetObjects.Field.efEnumFieldType.efDateTime:
					
					
					DateTime dtValue = DataConversion.efNullDate;

					if (oRsField == null)
					{
						bIsNull = true;
					}
					else
					{
						
						bIsNull = oRsField.bIsNull();
						if (! oRsField.bIsNull())
						{
							dtValue = oRsField.dtValue;
						}
					}
					
					return moRule.gbCheckValue(oClientInfo, dtValue, bIsNull);
					
				case RecordsetObjects.Field.efEnumFieldType.efDecimal:
					
					
					decimal dValue = 0;
					
					if (oRsField == null)
					{
						bIsNull = true;
					}
					else
					{
						dValue = oRsField.dValue;
						bIsNull = oRsField.bIsNull();
					}
					
					return moRule.gbCheckValue(oClientInfo, dValue, bIsNull);
					
				case RecordsetObjects.Field.efEnumFieldType.efDouble:
					
					
					double dbValue = 0;
					
					if (oRsField == null)
					{
						bIsNull = true;
					}
					else
					{
						dbValue = oRsField.dbValue;
						bIsNull = oRsField.bIsNull();
					}
					
					return moRule.gbCheckValue(oClientInfo, dbValue, bIsNull);
					
					
					
				case RecordsetObjects.Field.efEnumFieldType.efInteger:
					
					
					int lValue = 0;
					if (oRsField == null)
					{
						bIsNull = true;
					}
					else
					{
						lValue = oRsField.lValue;
						bIsNull = oRsField.bIsNull();
					}
					
					return moRule.gbCheckValue(oClientInfo, lValue, bIsNull);
					
					
					
				case RecordsetObjects.Field.efEnumFieldType.efMemo:
					
					if (oRsField == null)
					{
						bIsNull = true;
					}
					else
					{
						sValue = oRsField.sValue;
						bIsNull = oRsField.bIsNull();
					}
					
					return moRule.gbCheckValue(oClientInfo, sValue, bIsNull);
					
					
					
				case RecordsetObjects.Field.efEnumFieldType.efString:
					
					if (oRsField == null)
					{
						bIsNull = true;
					}
					else
					{
						sValue = oRsField.sValue;
						bIsNull = oRsField.bIsNull();
					}
					
					return moRule.gbCheckValue(oClientInfo, sValue, bIsNull);
					
					
				default:
					
					throw (new TableDefParseException("Unhandled field-type: \"" + menType + "\""));
					
			}
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
		public virtual void gInitFromXml (XmlNode oXmlNode)
		{
			
			
			
			if (oXmlNode.selectSingleNode("name") == null)
			{
				throw (new TableDefParseException("Node \"name\" for field not found!"));
			}
			else
			{
				msName = oXmlNode.selectSingleNode("name").sText;
			}
			
			if (oXmlNode.selectSingleNode("desc") == null)
			{
				throw (new TableDefParseException("Node \"desc\" for field not found!"));
			}
			else
			{
				msDesc = oXmlNode.selectSingleNode("desc").sText;
			}
			
			if (oXmlNode.selectSingleNode("type") == null)
			{
				throw (new TableDefParseException("Node \"type\" for field not found!"));
			}
			else
			{
				menType = menCTableDefFieldType(oXmlNode.selectSingleNode("type").sText);
			}
			
			if (oXmlNode.oAttributeList["primarykey"].sText == "1")
			{
				mbPrimaryKey = true;
			}
			
			
			if (menType == RecordsetObjects.Field.efEnumFieldType.efString)
			{
				if (oXmlNode.selectSingleNode("size") == null)
				{
					throw (new TableDefParseException("Node \"size\" missing for field \"" + msName + "\"!"));
				}
				else if (! Functions.IsNumeric(oXmlNode.selectSingleNode("size").sText))
				{
					throw (new TableDefParseException("Node \"size\" is not numeric: " + oXmlNode.selectSingleNode("size").sText));
				}
				else
				{
					mlSize = System.Convert.ToInt32(oXmlNode.selectSingleNode("size").sText);
					if (mlSize <= 0)
					{
						throw (new TableDefParseException("Invalid value for size \"" + mlSize + "\""));
					}
				}
			}
			else
			{
				mlSize = -1;
			}
			
			
			if (oXmlNode.selectNodes("rules").lCount > 1)
			{
				throw (new TableDefParseException("Only one sub-node of type \"rules\" " + "is allowed for: " + msName));
			}
			else
			{
				moRule = new TableDefRule(this);
				moRule.gInitFromXml(oXmlNode.selectSingleNode("rules"));
				
				
			}
			
			
		}
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		protected RecordsetObjects.Field.efEnumFieldType menCTableDefFieldType(string sDataType)
		{
			
			sDataType = Functions.LCase(sDataType);
			switch (sDataType)
			{
				case "int":
					
					return RecordsetObjects.Field.efEnumFieldType.efInteger;
				case "string":
					
					return RecordsetObjects.Field.efEnumFieldType.efString;
				case "bool":
					return RecordsetObjects.Field.efEnumFieldType.efBool;
					
				case "boolean":
					
					return RecordsetObjects.Field.efEnumFieldType.efBool;
				case "memo":
					
					return RecordsetObjects.Field.efEnumFieldType.efMemo;
				case "decimal":
					
					return RecordsetObjects.Field.efEnumFieldType.efDecimal;
				case "double":
					
					return RecordsetObjects.Field.efEnumFieldType.efDouble;
				case "date":
					
					return RecordsetObjects.Field.efEnumFieldType.efDateTime;
				default:
					
					throw (new TableDefParseException("Invalid field-type found: \"" + sDataType + "\""));
					
					
			}
			
		}
		//================================================================================
		//Private Consts:
		//================================================================================
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
		
	}
	
}
