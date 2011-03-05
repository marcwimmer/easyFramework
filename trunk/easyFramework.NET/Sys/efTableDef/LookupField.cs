using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Sys.Data.Table
{
	//================================================================================
//Class:     LookupField
	//--------------------------------------------------------------------------------'
//Module:    LookupField.vb
	//--------------------------------------------------------------------------------'
//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
//Purpose:   special Field for lookups, so that it doesn't disturb the real
	//           table-fields
	//           lookup-fields are for example:
	//           adp_id, adp_f1, (select lookupvalue from t2 where t2.f1 = thisTable.fx) as adp_lookupvalue
	//           (the statement in brackets is the lookup-value)
	//--------------------------------------------------------------------------------'
//Created:   20.08.2004 12:44:14
	//--------------------------------------------------------------------------------'
//Changed:
	//--------------------------------------------------------------------------------'
	
	//================================================================================
//Imports:
	//================================================================================
	
	public class TableDefLookupField : TableDefField
	{
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		protected string msLookupSql;
		protected bool mbIsReadOnly;
		
		//================================================================================
		//Public Consts:
		//================================================================================
		
		//================================================================================
		//Public Properties:
		//================================================================================
		public string sLookupSql
		{
			get{
				return msLookupSql;
			}
		}
		public bool bIsReadOnly
		{
			get{
				return mbIsReadOnly;
			}
		}
		//================================================================================
		//Public Events:
		//================================================================================
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		
		//================================================================================
//Function:  sGetAsSQL
		//--------------------------------------------------------------------------------'
//Purpose:   returns sql-representation of the lookup-field
		//--------------------------------------------------------------------------------'
//Params:    bWithAsClause - true: includes as clause;
		//                           you usually need "false" when using the lookup
		//                           in a where-clause
		//--------------------------------------------------------------------------------'
//Returns:
		//--------------------------------------------------------------------------------'
//Created:   20.08.2004 12:55:33
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public string sGetAsSQL(bool bWithAsClause)
		{
			
			string sRet;
			
			
			sRet = "(" + msLookupSql + ") ";
			
			if (bWithAsClause)
			{
				sRet += " AS " + msName;
			}
			
			
			return sRet;
			
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
		public override void gInitFromXml (Xml.XmlNode oXmlNode)
		{
			
			msName = oXmlNode.selectSingleNode("name").sText;
			mbIsReadOnly = true;
			if (!Functions.IsEmptyString(oXmlNode.sGetValue("desc")))
			{
				msDesc = oXmlNode.selectSingleNode("desc").sText;
			}
			else
			{
				msDesc = msName;
			}
			menType = menCTableDefFieldType(oXmlNode.selectSingleNode("type").sText);
			mbPrimaryKey = false;
			if (oXmlNode.selectSingleNode("sql") == null)
			{
				throw (new efDataException("sql-Element missing in \"" + oXmlNode.sXml + "\"."));
			}
			else
			{
				msLookupSql = oXmlNode.selectSingleNode("sql").sText;
				
			}
			
			if (oXmlNode.selectSingleNode("type") == null)
			{
				throw (new TableDefParseException("Node \"type\" for field not found!"));
			}
			else
			{
				menType = menCTableDefFieldType(oXmlNode.selectSingleNode("type").sText);
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
		}
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Private Consts:
		//================================================================================
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
		//================================================================================
		//Private Methods:
		//================================================================================
		
		
		
	}
	
}
