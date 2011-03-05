using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using System.Text;

namespace easyFramework.Sys
{
	//================================================================================
	// Class:        Field
	//--------------------------------------------------------------------------------
	// Purpose:      Field of Recordset
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	namespace RecordsetObjects
	{
		
		
		public class Field
		{
			
			//================================================================================
			//private fields:
			//================================================================================
			private bool mbIsFieldDef; //if true, then no values can be read
			private string msValue; //the value of the field; in a string, everything can be stored
			private string msName;
			private FieldList moParentFieldList;
			private efEnumFieldType menFieldType;
			
			//================================================================================
			//public const:
			//================================================================================
			public const string efsXmlNode_Field = "field";
			public const string efsXmlAttribute_Type = "type";
			public const string efsXmlAttribute_IsFieldDef = "isFielddef";
			public const string efsXmlAttribute_Name = "name";
			
			public enum efEnumFieldType
			{
				efUndefined = -1,
				efString = 0,
				efInteger = 1,
				efBool = 2,
				efBinary = 3,
				efMemo = 4,
				efDateTime = 6,
				efDecimal = 7,
				efDouble = 8,
				efUniqueIdentifier = 9,
				efDate = 10
			}
			
			public const string efsFieldType_INT = "int";
			public const string efsFieldType_NVARCHAR = "nvarchar";
			public const string efsFieldType_NTEXT = "ntext";
			public const string efsFieldType_UNIQUEIDENTIFIER = "uniqueidentifier";
			public const string efsFieldType_DATE = "date";
			public const string efsFieldType_DATETIME = "datetime";
			public const string efsFieldType_BIT = "bit";
			public const string efsFieldType_MONEY = "money";
			public const string efsFieldType_BINARY = "binary";
			
			//================================================================================
			//public properties:
			//================================================================================
			
			
			//================================================================================
			//Property:  sValue
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value as string
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:45:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public string sValue
			{
				get
				{
					if (mbIsNull() | mbIsFieldDef)
					{
						return "";
					}
					else
					{
						return msValue;
					}
					
				}
				set
				{
					msValue = value;
				}
			}
			
			//================================================================================
			//Property:  bValue
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value as boolean
			//--------------------------------------------------------------------------------'
			//Params:    the on-null value
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:45:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public bool bValue
			{
				get
				{
					if (mbIsNull() | mbIsFieldDef)
					{
						return false;
					}
					else
					{
						return easyFramework.Sys.ToolLib.DataConversion.gbCBool(msValue);
					}
				}
				set
				{
					msValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(value);
				}
			}
			
			//================================================================================
			//Property:  sName
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the name of the field
			//--------------------------------------------------------------------------------'
			//Params:    -
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 22:50:09
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public string sName
			{
				get
				{
					return msName;
				}
				
			}
			
			
			//================================================================================
			//Property:  lIndex
			//--------------------------------------------------------------------------------'
			//Purpose:   Returns the index of the field. This is the index which can be used
			//           in "oFields(2).sValue
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   22.05.2004 18:38:36
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public int lIndex
			{
				get
				{
					for (int i = 0; i <= moParentFieldList.Count - 1; i++)
					{
						if (moParentFieldList[i].sName == msName)
						{
							return i;
						}
					}
					
					throw (new efException("Internal data-error: parent-field-list not set correctly of field " + sName));
					
				}
				
			}
			//================================================================================
			//Property:  lValue
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value as integer
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:45:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public int lValue
			{
				get
				{
					if (mbIsNull() | mbIsFieldDef)
					{
						return 0;
					}
					else
					{
						return easyFramework.Sys.ToolLib.DataConversion.glCInt(msValue, 0);
					}
				}
				set
				{
					msValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(value);
				}
			}
			
			//================================================================================
			//Property:  dValue
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value as decimal
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:45:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public decimal dValue
			{
				get
				{
					if (mbIsNull() | mbIsFieldDef)
					{
						return 0;
					}
					else
					{
						return easyFramework.Sys.ToolLib.DataConversion.gdCDec(msValue, 0);
					}
					
				}
				set
				{
					msValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(value);
				}
			}
			
			//================================================================================
			//Property:  dbValue
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value as decimal
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:45:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public double dbValue
			{
				get
				{
					if (mbIsNull() | mbIsFieldDef)
					{
						return 0;
					}
					else
					{
						return easyFramework.Sys.ToolLib.DataConversion.gdCDbl(msValue, 0);
					}
					
				}
				set
				{
					msValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(value);
				}
			}
			
			//================================================================================
			//Property:  dtValue
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value as date
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:45:50
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public DateTime dtValue
			{
				get
				{
					if (mbIsNull() | mbIsFieldDef)
					{
						return easyFramework.Sys.ToolLib.DataConversion.efNullDate;
					}
					else
					{
						return easyFramework.Sys.ToolLib.DataConversion.gdtCDate(msValue, DateTime.Parse("01.01.1900"));
					}
				}
				set
				{
					msValue = easyFramework.Sys.ToolLib.DataConversion.gsCStr(value);
				}
			}
			
			//================================================================================
			//Property:  enType
			//--------------------------------------------------------------------------------'
			//Purpose:   the type of the field
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   05.04.2004 18:49:56
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public efEnumFieldType enType
			{
				get
				{
					return menFieldType;
				}
				set
				{
					menFieldType = efEnumFieldType.efBinary;
				}
			}
			
			
			
			//================================================================================
			//public methods:
			//================================================================================
			
			//================================================================================
			// Method:       New
			//--------------------------------------------------------------------------------
			// Purpose:      creates a new field-object
			//--------------------------------------------------------------------------------
			// Parameteres:  creates a new field-object
			//--------------------------------------------------------------------------------
			// Returns:      -
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:		03.06.2004 - allow no-name-fields; they can be reached by
			//                            an index then
			//================================================================================
			public Field(FieldList oParentFieldList, bool bOnlyFieldDef, string sName, efEnumFieldType enFieldType, string sValue) 
			{
				mbIsFieldDef = false;
				
				
				msName = sName;
				msValue = sValue;
				moParentFieldList = oParentFieldList;
				menFieldType = enFieldType;
				mbIsFieldDef = bOnlyFieldDef;
				
			}
			
			//================================================================================
			//Sub:       New
			//--------------------------------------------------------------------------------'
			//Purpose:   inits a field from an xml-node
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   03.06.2004 23:46:56
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public Field(FieldList oParentFieldList, XmlNode oFieldNode) 
			{
				mbIsFieldDef = false;
				
				mLoadFromXml(oFieldNode);
				moParentFieldList = oParentFieldList;
				
			}
			//================================================================================
			//Function:  IsNull
			//--------------------------------------------------------------------------------'
			//Purpose:   Determines, wether the value is null
			//--------------------------------------------------------------------------------'
			//Params:    -
			//--------------------------------------------------------------------------------'
			//Returns:   boolean
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:57:32
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public bool bIsNull()
			{
				
				return mbIsNull();
				
			}
			
			
			//================================================================================
			//Sub:   mSetNull
			//--------------------------------------------------------------------------------'
			//Purpose:   Sets the field-value to null
			//--------------------------------------------------------------------------------'
			//Params:    -
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:59:58
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public void mSetNull ()
			{
				msValue = easyFramework.Sys.ToolLib.DataConversion.efNullValue;
				
			}
			
			
			
			//================================================================================
			//Function:  sGetForSQL
			//--------------------------------------------------------------------------------'
			//Purpose:   returns the value of the field for a sql-statement, strings are
			//           for example inclusive "'"
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   05.04.2004 18:47:21
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public string sGetForSQL()
			{
				
				if (this.bIsNull())
				{
					return "NULL";
				}
				
				switch (this.enType)
				{
					case efEnumFieldType.efBinary:
						throw (new Exception("could not get binary as sql"));
						
					case efEnumFieldType.efBool:
						
						if (bValue == true)
						{
							return "1";
						}
						else
						{
							return "0";
						}
						
						
					case efEnumFieldType.efDateTime:
						
						return DataConversion.gsSqlDate(dtValue, easyFramework.Sys.ToolLib.DataConversion.efEnumSqlDateFormat.dfTimeStamp);
						
					case efEnumFieldType.efDate:
						
						return DataConversion.gsSqlDate(dtValue, easyFramework.Sys.ToolLib.DataConversion.efEnumSqlDateFormat.dfDate);
						
					case efEnumFieldType.efInteger:
						
						return sValue;
						
					case efEnumFieldType.efDecimal:
						
						return "CONVERT(MONEY, '" + Functions.Replace(sValue, ",", ".") + "')"; //because of comma, like "2,000"
						
					case efEnumFieldType.efUndefined:
						return "'" + Functions.Replace(this.sValue, "'", "''") + "'";
						
						
						
					case efEnumFieldType.efMemo:
						return "'" + Functions.Replace(this.sValue, "'", "''") + "'";
						
						
						
					case efEnumFieldType.efString:
						return "'" + Functions.Replace(this.sValue, "'", "''") + "'";
						
						
						
					case efEnumFieldType.efUniqueIdentifier:
						
						return "'" + Functions.Replace(this.sValue, "'", "''") + "'";
						
						
				}

				throw new efException("Couldn't get sSql for field \"" + this.sName + "\".");
				
			}
			
			
			//================================================================================
			//Function:  enType2Str
			//--------------------------------------------------------------------------------'
			//Purpose:   converts the type to a string (for xml)
			//--------------------------------------------------------------------------------'
			//Params:    efEnumFieldType
			//--------------------------------------------------------------------------------'
			//Returns:   string
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 20:15:15
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static string senType2Str(efEnumFieldType enType)
			{
				
				switch (enType)
				{
					case efEnumFieldType.efBinary:
						
						return "binary";
					case efEnumFieldType.efBool:
						
						return "bool";
					case efEnumFieldType.efDateTime:
						
						return "datetime";
					case efEnumFieldType.efDate:
						
						return "date";
					case efEnumFieldType.efInteger:
						
						return "int";
					case efEnumFieldType.efMemo:
						
						return "memo";
					case efEnumFieldType.efString:
						
						return "string";
					case efEnumFieldType.efUniqueIdentifier:
						
						return "uniqueidentifier";
					case efEnumFieldType.efUndefined:
						
						return "";
					default:
						
						throw (new efException("enType2Str failed for value " + enType));
						
						
				}
			}
			
			
			//================================================================================
			//Function:  enStr2Type
			//--------------------------------------------------------------------------------'
			//Purpose:   converts the string to a type
			//--------------------------------------------------------------------------------'
			//Params:    string
			//--------------------------------------------------------------------------------'
			//Returns:   efEnumFieldType
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 20:17:11
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static efEnumFieldType enStr2Type(string sType)
			{
				switch (Functions.LCase(sType))
				{
					case "binary":
						return efEnumFieldType.efBinary;

					case "bool":    						
						return efEnumFieldType.efBool;

					case "datetime":
						return efEnumFieldType.efDateTime;

					case "date":
						return efEnumFieldType.efDate;

					case "int":
						return efEnumFieldType.efInteger;

					case "memo":
						return efEnumFieldType.efMemo;

					case "string":  						
						return efEnumFieldType.efString;

					case "uniqueidentifier": 						
						return efEnumFieldType.efUniqueIdentifier;

					case "":
						return efEnumFieldType.efUndefined;

					default:
						throw (new efException("enType2Str failed for value " + sType));
						
				}
			}
			
			
			
			//================================================================================
			//Sub:       gAppendAsXml
			//--------------------------------------------------------------------------------'
			//Purpose:   transforms the field to an xml-node and appends it
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   03.06.2004 23:21:13
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public void gAppendAsXml (XmlNode oParent)
			{
				
				XmlNode oNewNode = oParent.AddNode(efsXmlNode_Field, true);
				oNewNode.sText = msValue;
				oNewNode.oAttributeList[efsXmlAttribute_IsFieldDef].sText = easyFramework.Sys.ToolLib.DataConversion.gsCStr(easyFramework.Sys.ToolLib.DataConversion.glCInt(mbIsFieldDef, 0));
				oNewNode.oAttributeList[efsXmlAttribute_Name].sText = msName;
				oNewNode.oAttributeList[efsXmlAttribute_Type].sText = senType2Str(menFieldType);
				
				
			}
			
			
			
			//================================================================================
			//Function:  gEnSqlType2efType
			//--------------------------------------------------------------------------------'
			//Purpose:
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   04.06.2004 09:33:27
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static Field.efEnumFieldType gEnSqlType2efType(string sDataTypeName)
			{
				
				switch (Functions.LCase(sDataTypeName))
				{
					
					case efsFieldType_INT:
						return Field.efEnumFieldType.efInteger;

					case efsFieldType_NVARCHAR:
						return Field.efEnumFieldType.efString;
				
					case efsFieldType_NTEXT:
						return Field.efEnumFieldType.efMemo;
				
					case efsFieldType_MONEY:
						return Field.efEnumFieldType.efDecimal;

					case efsFieldType_BIT:
						return Field.efEnumFieldType.efBool;
				
					case efsFieldType_DATETIME:
						return Field.efEnumFieldType.efDateTime;

					case efsFieldType_DATE:
						return Field.efEnumFieldType.efDate;

					case efsFieldType_UNIQUEIDENTIFIER:
						return Field.efEnumFieldType.efUniqueIdentifier;

					case efsFieldType_BINARY:
						return Field.efEnumFieldType.efBinary;

					default:					
						throw (new efException("unknown datatype: " + sDataTypeName));
						
					
				}
			
			}
		
			//================================================================================
			//Protected Properties:
			//================================================================================
		
		
			//================================================================================
			//Protected Methods:
			//================================================================================
		
			//================================================================================
			//Sub:       gLoadFromXml
			//--------------------------------------------------------------------------------'
			//Purpose:   init the field-list from the node
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   03.06.2004 23:41:13
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			protected void mLoadFromXml (XmlNode oXmlNode)
			{
			
				if (oXmlNode.sName != efsXmlNode_Field)
				{
					throw (new efException("Invalid xml for recordset. Expected \"" + efsXmlNode_Field + "\", found \"" + oXmlNode.sName + "\"."));
				}
			
				mbIsFieldDef = easyFramework.Sys.ToolLib.DataConversion.gbCBool(oXmlNode.oAttributeList[efsXmlAttribute_IsFieldDef].sText);
				menFieldType = enStr2Type(oXmlNode.oAttributeList[efsXmlAttribute_Type].sText);
				msName = oXmlNode.oAttributeList[efsXmlAttribute_Name].sText;
				msValue = oXmlNode.sText;
			
			
			}
		
			//================================================================================
			//Function:  IsNull
			//--------------------------------------------------------------------------------'
			//Purpose:   internal function for determing the null-status
			//--------------------------------------------------------------------------------'
			//Params:    -
			//--------------------------------------------------------------------------------'
			//Returns:   boolean
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 18:57:32
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			protected bool mbIsNull()
			{
			
				if (msValue == easyFramework.Sys.ToolLib.DataConversion.efNullValue)
				{
					return true;
				}
				else
				{
				
					switch (this.enType)
					{
					
						case efEnumFieldType.efBinary:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efBool:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efDateTime:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efDecimal:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efDouble:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efInteger:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efUniqueIdentifier:
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					
						case efEnumFieldType.efDate:
					
							if (Functions.IsEmptyString(msValue))
							{
								return true;
							}
							else
							{
								return false;
							}
							
					
					}
			
					return false;
				}
		
				
		
			}
	
	
		}




	}
}
