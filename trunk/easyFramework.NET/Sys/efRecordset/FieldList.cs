using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using System.Text;
using System.Collections;

namespace easyFramework.Sys
{
	//================================================================================
	// Class:        FieldList
	//--------------------------------------------------------------------------------
	// Purpose:      Collection of Fields; implements icollection
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================

	//================================================================================
	//Imports:
	//================================================================================


	namespace RecordsetObjects
	{
	
	
		public class FieldList : System.Collections.IEnumerator
		{
		
			//================================================================================
			//private fields:
			//================================================================================
			private Recordset moParentRecordset;
			private efHashTable moFields;
			private efArrayList moKeys; //for indexing fields (index 3 = usr_name...)
			private int mlCurrent;
		
		
			//================================================================================
			//Public Consts:
			//================================================================================
			public const string efsXmlNode_FieldList = "fieldlist";
		
		
			//================================================================================
			//public properties:
			//================================================================================
		
			//================================================================================
			// Property:     Count
			//--------------------------------------------------------------------------------
			// Purpose:      recordnode-count
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public int Count
			{
				get
				{
					return moFields.Count;
				}
			}
		
			//================================================================================
			// Property:     IsSynchronized
			//--------------------------------------------------------------------------------
			// Purpose:      IsSynchronized
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}
		

		
			//================================================================================
			// Property:     Current
			//--------------------------------------------------------------------------------
			// Purpose:      mlCurrent
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public object Current
			{
				get
				{
				
					if (!(mlCurrent >= 0 & moFields.Count >= 0))
					{
						return null;
					}
				
					Field oField;
					string sKeyValue = System.Convert.ToString(moKeys[mlCurrent]);
				
					oField = this[sKeyValue];
					return oField;
				
				}
			}
		

			//================================================================================
			//Property:  oField
			//--------------------------------------------------------------------------------'
			//Purpose:   returns a Field-object
			//--------------------------------------------------------------------------------'
			//Params:    indexname
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 20:50:41
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public Field this[string sName]
			{
				get
				{
				
					//----case insensitive----
					sName = Functions.LCase(sName);
				
					for (int i = 0; i <= moKeys.Count - 1; i++)
					{										 
						Field oCompareField = ((Field)(moFields[moKeys[i]]));
						if (Functions.LCase(oCompareField.sName) == Functions.LCase(sName))
						{
							return oCompareField;
						}
					}
				
					throw (new efException("Field \"" + sName + "\" doesn't exist."));
				
				}
			
			}
		
			//================================================================================
			//Property:  oField
			//--------------------------------------------------------------------------------'
			//Purpose:   returns a Field-object
			//--------------------------------------------------------------------------------'
			//Params:    indexname
			//--------------------------------------------------------------------------------'
			//Created:   21.03.2004 20:50:41
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public Field this[int lIndex]
			{
				get
				{
				
					ICollection oCol = moFields.Keys;
				
					string sKeyValue = System.Convert.ToString(moKeys[lIndex]);
				
					return ((Field)(moFields[sKeyValue]));
				
				
				}
			
			}
		
			//================================================================================
			//public methods:
			//================================================================================
			//================================================================================
			// Method:       New
			//--------------------------------------------------------------------------------
			// Purpose:      Constructor
			//--------------------------------------------------------------------------------
			// Parameteres:  current recordnode (<record>...</record>)
			//
			//--------------------------------------------------------------------------------
			// Returns:      a new created recordset
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public FieldList(Recordset oParentRecordset) 
			{
				moFields = new efHashTable();
				moKeys = new efArrayList();
				mlCurrent = -1;
			
			
				moParentRecordset = oParentRecordset;
			
			
			}
		
		
			//================================================================================
			//Sub:       gAppendAsXml
			//--------------------------------------------------------------------------------'
			//Purpose:   transforms the fieldliest to an xml-node and appends it
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   03.06.2004 23:21:13
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public void gAppendAsXml (XmlNode oParent)
			{
			
				XmlNode oNewNode = oParent.AddNode(efsXmlNode_FieldList, false);
				for (int i = 0; i <= moFields.Count - 1; i++)
				{
					Field oField = ((Field)(moFields[i]));
					oField.gAppendAsXml(oNewNode);
				}
			
			}
		
		

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
			public void gLoadFromXml (XmlNode oXmlNode)
			{
			
				//------------flush data----------
				mFlush();
			
				//---------check node-name--------
				if (oXmlNode.sName != efsXmlNode_FieldList)
				{
					throw (new efException("Invalid xml for recordset. Expected \"" + efsXmlNode_FieldList + "\", found \"" + oXmlNode.sName + "\"."));
				}
			
				//---------create fields-----------
				XmlNodeList onlFields = oXmlNode.oChildren;
			
				for (int i = 0; i <= onlFields.lCount - 1; i++)
				{
				
					Field oField = new Field(this, onlFields[i]);
				
				}
			
			}
		
			//================================================================================
			//Sub:       gAppendField
			//--------------------------------------------------------------------------------'
			//Purpose:   appends a new field
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   03.06.2004 23:08:44
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public void gAppendField (string sName, Field.efEnumFieldType enType, bool bOnlyFieldDef)
			{
			
			
			
				//----the key in the hash-table needs a key-value-----
				if (Functions.IsEmptyString(sName))
				{
					sName = easyFramework.Sys.ToolLib.DataConversion.gsCStr(Guid.NewGuid());
				}
			
				Field oField = new Field(this, bOnlyFieldDef, sName, enType, "");
			
				moFields.Add(sName, oField);
				moKeys.Add(sName);
			
			}
		
		
			//================================================================================
			//Function:  gbIsField
			//--------------------------------------------------------------------------------'
			//Purpose:   returns true, if the field exists in the recordset
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   09.04.2004 15:27:39
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public bool gbIsField(string sName)
			{
			
			
				for (int i = 0; i <= this.Count - 1; i++)
				{
				
					if (Functions.LCase(this[i].sName) == Functions.LCase(sName))
					{
						return true;
					}
				}
				return false;
			}
			//================================================================================
			// Method:       CopyTo
			//--------------------------------------------------------------------------------
			// Purpose:      copies arraycontent
			//--------------------------------------------------------------------------------
			// Parameteres:  array, index
			//--------------------------------------------------------------------------------
			// Returns:      -
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public void CopyTo (System.Array array, int index)
			{
			
			}
		
			//================================================================================
			// Method:       GetEnumerator
			//--------------------------------------------------------------------------------
			// Purpose:      Returns the enumerator
			//--------------------------------------------------------------------------------
			// Parameteres:  -
			//--------------------------------------------------------------------------------
			// Returns:      -
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public System.Collections.IEnumerator GetEnumerator()
			{
				mlCurrent = -1;
				return this;
			}
		
			//================================================================================
			// Method:       MoveNext
			//--------------------------------------------------------------------------------
			// Purpose:      moves current
			//--------------------------------------------------------------------------------
			// Parameteres:  -
			//--------------------------------------------------------------------------------
			// Returns:      -
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public bool MoveNext()
			{
				if (mlCurrent < Count - 1)
				{
					mlCurrent += 1;
					return true;
				}
				else
				{
					return false;
				}
			}
		
			//================================================================================
			// Method:       Reset
			//--------------------------------------------------------------------------------
			// Purpose:      resets current
			//--------------------------------------------------------------------------------
			// Parameteres:  -
			//--------------------------------------------------------------------------------
			// Returns:      -
			//--------------------------------------------------------------------------------
			// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
			//--------------------------------------------------------------------------------
			// Changed:
			//================================================================================
			public void Reset ()
			{
				mlCurrent = -1;
			}
		
		
		
			//================================================================================
			//Sub:       mFlush
			//--------------------------------------------------------------------------------'
			//Purpose:   flushes the recordset
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   04.06.2004 00:39:31
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			protected void mFlush ()
			{
				moKeys = null;
				moFields = null;
			
			}
		
		
		}
	}
}
