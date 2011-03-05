using System;
using System.Xml;
using System.Collections;

namespace easyFramework.Sys.Xml
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    XmlNodeList.vb
	//--------------------------------------------------------------------------------
	// Purpose:      the basic xml-nodelist
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	//================================================================================
	//Imports
	//================================================================================
	
	public class XmlNodeList : System.Collections.IEnumerator
	{
		
		//================================================================================
		//private fields:
		//================================================================================
		private System.Xml.XmlNodeList moSystemXmlNodeList;
		private int mlCurrent;
		
		//================================================================================
		//public properties:
		//================================================================================
		
		//================================================================================
		// Property:     lCount
		//--------------------------------------------------------------------------------
		// Purpose:      lCount
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public int lCount
		{
			get{
				return moSystemXmlNodeList.Count;
			}
		}
		
		//================================================================================
		// Property:     Current
		//--------------------------------------------------------------------------------
		// Purpose:      Current
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public object Current
		{
			get{
				return new XmlNode(moSystemXmlNodeList[mlCurrent]);
			}
		}
		
		//================================================================================
		// Property:     Item
		//--------------------------------------------------------------------------------
		// Parameteres:  oSystemXMLNodeList
		//--------------------------------------------------------------------------------
		// Returns:      XmlNode
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNode this[int lIndex]
		{
			get{
				return new XmlNode(moSystemXmlNodeList.Item(lIndex));
			}
		}
		
		//================================================================================
		// Property:     Item
		//--------------------------------------------------------------------------------
		// Parameteres:  nodename
		//--------------------------------------------------------------------------------
		// Returns:      the first node of the given name
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNode this[string sName]
		{
			get{
				
				for (int i = 0; i <= moSystemXmlNodeList.Count - 1; i++)
				{
					if (moSystemXmlNodeList[i].Name == sName)
					{
						return new XmlNode(moSystemXmlNodeList[i]);
					}
				}
			
				return null;
			}
		}
		
		//================================================================================
		// Property:     Item
		//--------------------------------------------------------------------------------
		// Parameteres:  nodename, attributename, attributevalue
		//--------------------------------------------------------------------------------
		// Returns:      the first node of the given name with the given attribute-value
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNode this[string sName, string sAttributeName, string sAttributeValue]
		{
			
			get{
				
				for (int i = 0; i <= moSystemXmlNodeList.Count - 1; i++)
				{
					
					if (moSystemXmlNodeList[i].Name == sName)
					{
						
						for (int y = 0; y <= moSystemXmlNodeList[i].Attributes.Count - 1; y++)
						{
							
							if (moSystemXmlNodeList[i].Attributes[y].Name == sAttributeName)
							{
								if (moSystemXmlNodeList[i].Attributes[y].InnerText == sAttributeValue)
								{
									return new XmlNode(moSystemXmlNodeList[i]);
								}
							}
							
							
						}
					}
					
					
				}

				return null;
				
			}
		}
		
		
		
		//================================================================================
		//public methods:
		//================================================================================
		//================================================================================
		// Method:       New
		//--------------------------------------------------------------------------------
		// Parameteres:  oSystemXMLNodeList
		//--------------------------------------------------------------------------------
		// Returns:      XmlNode
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNodeList(System.Xml.XmlNodeList oSystemXMLNodeList) {
			moSystemXmlNodeList = oSystemXMLNodeList;
			mlCurrent = -1;
		}
		
		
		
		//================================================================================
		// Method:       GetEnumerator
		//--------------------------------------------------------------------------------
		// Parameteres:  -
		//--------------------------------------------------------------------------------
		// Returns:      IEnumerable.Enumerator
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public System.Collections.IEnumerator GetEnumerator()
		{
			return this;
		}
		
		//================================================================================
		// Method:       MoveNext
		//--------------------------------------------------------------------------------
		// Parameteres:  Boolean
		//--------------------------------------------------------------------------------
		// Returns:      True, if not eof
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public bool MoveNext()
		{
			if (mlCurrent < moSystemXmlNodeList.Count - 1)
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
		// Purpose:      for enumerator; start at first item again
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
	}
	
	
	
}
