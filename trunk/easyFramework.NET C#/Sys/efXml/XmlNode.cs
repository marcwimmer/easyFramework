using System;
using System.Xml;

namespace easyFramework.Sys.Xml
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    XmlNodeList.vb
	//--------------------------------------------------------------------------------
	// Purpose:      the basic xml-node
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	public class XmlNode
	{
		
		//================================================================================
		//private fields:
		//================================================================================
		private System.Xml.XmlNode moSystemXmlNode;
		
		//================================================================================
		//public properties:
		//================================================================================
		
		//================================================================================
		// Property:     oAttributeList
		//--------------------------------------------------------------------------------
		// Purpose:      attribute-list
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlAttributeList oAttributeList
		{
			get
			{
				return new XmlAttributeList(moSystemXmlNode.Attributes, moSystemXmlNode);
			}
		}
		//================================================================================
		// Property:     oAttribute
		//--------------------------------------------------------------------------------
		// Purpose:      default attribute
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlAttribute this[string sName]
		{
			get
			{
				return oAttributeList[sName];
				
			}
		}
		
		//================================================================================
		// Property:     sName
		//--------------------------------------------------------------------------------
		// Purpose:      the name of the node
		//--------------------------------------------------------------------------------
		// Created:      09.04.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public string sName
		{
			get
			{
				return moSystemXmlNode.Name;
			}
		}
		
		//================================================================================
		// Property:     sText
		//--------------------------------------------------------------------------------
		// Purpose:      inner-text as cdata; if cdata does not exist, it is created
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public string sText
		{
			get
			{
				
				if (! mbIsDataNode())
				{
					throw (new XmlException("node is not a data-node, because it contains other elements. " + "the sText-property is not available"));
				}
				
				System.Xml.XmlCDataSection oCDataNode=null;
				
				if (moSystemXmlNode.ChildNodes.Count > 0)
				{
					if (moSystemXmlNode.ChildNodes[0].NodeType == XmlNodeType.CDATA)
					{
						oCDataNode = ((System.Xml.XmlCDataSection)(moSystemXmlNode.ChildNodes[0]));
					}
				}
				
				if (oCDataNode != null)
				{
					return oCDataNode.Value;
				}
				else
				{
					return moSystemXmlNode.InnerText;
				}
				
				
			}
			set
			{
				if (! mbIsDataNode())
				{
					throw (new XmlException("node is not a data-node, because it contains other elements. " + "the sText-property is not available"));
				}
				
				System.Xml.XmlCDataSection oCDataNode = null;
				
				if (moSystemXmlNode.ChildNodes.Count > 0)
				{
					if (moSystemXmlNode.ChildNodes[0].NodeType == XmlNodeType.CDATA)
					{
						oCDataNode = ((System.Xml.XmlCDataSection)(moSystemXmlNode.ChildNodes[0]));
					}
				}
				
				if (oCDataNode == null)
				{
					oCDataNode = moSystemXmlNode.OwnerDocument.CreateCDataSection("");

					//-----------remove existing text; it could be, that there was no CDATA before---
					if (moSystemXmlNode.ChildNodes.Count == 1 && moSystemXmlNode.ChildNodes[0].NodeType == XmlNodeType.Text)
						moSystemXmlNode.InnerText = "";

					moSystemXmlNode.AppendChild(oCDataNode);
				}
				
				oCDataNode.Value = value;
			}
		}
		
		//================================================================================
		// Property:     sXml
		//--------------------------------------------------------------------------------
		// Purpose:      sXml
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public string sXml
		{
			get
			{
				return moSystemXmlNode.OuterXml;
			}
			
		}
		
		//================================================================================
		// Property:     sInnerXml
		//--------------------------------------------------------------------------------
		// Purpose:      sInnerXml
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public string sInnerXml
		{
			get
			{
				return moSystemXmlNode.InnerXml;
			}
			set
			{
				moSystemXmlNode.InnerXml = value;
			}
			
		}
		
		//================================================================================
		//Property:  oParent
		//--------------------------------------------------------------------------------'
		//Purpose:   gets the parent
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 18:40:38
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public XmlNode oParent
		{
			get
			{
				
				XmlNode oResult = new XmlNode(moSystemXmlNode.ParentNode);
				
				return oResult;
				
			}
			
		}
		
		//================================================================================
		//Property:  oChildren
		//--------------------------------------------------------------------------------'
		//Purpose:   gets the children
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 18:40:38
		//--------------------------------------------------------------------------------'
		//--------------------------------------------------------------------------------'
		//Changed:
		public XmlNodeList oChildren
		{
			get
			{
				
				XmlNodeList oResult = new XmlNodeList(moSystemXmlNode.ChildNodes);
				
				return oResult;
				
			}
			
		}
		
		
		//================================================================================
		//public methods:
		//================================================================================
		//================================================================================
		// Method:       New
		//--------------------------------------------------------------------------------
		// Purpose:      constructor
		//--------------------------------------------------------------------------------
		// Parameteres:  oSystemXMLNode
		//--------------------------------------------------------------------------------
		// Returns:      -
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNode(System.Xml.XmlNode oSystemXMLNode) 
		{
			moSystemXmlNode = oSystemXMLNode;
			
		}
		
		//================================================================================
		// Method:       sGetValue
		//--------------------------------------------------------------------------------
		// Parameteres:  makes a select and returns the value, if a node was
		//               found;
		//--------------------------------------------------------------------------------
		// Returns:      String
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public string sGetValue(string sNodeName)
		{ 
			return sGetValue(sNodeName, "", true); 
		}

		public string sGetValue(string sNodeName, string onNoNodeFound, bool bSearchTree)
		{
			
			
			string sQry;
			if (bSearchTree == false)
			{
				sQry = sNodeName;
			}
			else
			{
				sQry = ".//" + sNodeName;
			}
			
			XmlNode oNode = selectSingleNode(sNodeName);
			if (oNode == null)
			{
				return onNoNodeFound;
			}
			else
			{
				return oNode.sText;
				
			}
			
			
		}
		
		//================================================================================
		//Function:  AddNode
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a new node
		//--------------------------------------------------------------------------------'
		//Params:    sname - the node name
		//           bIsDataNode - true: data is stored in CDATA-section; otherwise
		//           the node contains only other elements
		//--------------------------------------------------------------------------------'
		//Returns:   the created node
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 20:02:40
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public XmlNode AddNode(string sName, bool bIsDataNode)
		{
			
			if (sName.IndexOf(" ") >= 0)
			{
				throw new Exception("An XML-Node name mustn't contain a space.");
			}

			System.Xml.XmlNode xNode = moSystemXmlNode.OwnerDocument.CreateNode(XmlNodeType.Element, sName, "");
			
			moSystemXmlNode.AppendChild(xNode);
			
			if (bIsDataNode == true)
			{
				xNode.AppendChild(moSystemXmlNode.OwnerDocument.CreateCDataSection(""));
				
			}
			
			return new XmlNode(xNode);
			
		}
		
		
		//================================================================================
		//Sub:       gRemoveFromParent
		//--------------------------------------------------------------------------------'
		//Purpose:   removes the node from its parent
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   06.05.2004 15:32:13
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gRemoveFromParent ()
		{
			
			if (moSystemXmlNode.ParentNode == null == false)
			{
				moSystemXmlNode.ParentNode.RemoveChild(moSystemXmlNode);
			}
			
		}
		
		
		//================================================================================
		// Method:       selectNodes
		//--------------------------------------------------------------------------------
		// Parameteres:  sXPath - XPath expression
		//--------------------------------------------------------------------------------
		// Returns:      XmlNodeList
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNodeList selectNodes(string sXPath)
		{
			
			System.Xml.XmlNodeList oXmlNodeList;
			oXmlNodeList = moSystemXmlNode.SelectNodes(sXPath);
			
			if (oXmlNodeList == null)
			{
				return null;
			}
			return new XmlNodeList(oXmlNodeList);
			
		}
		
		//================================================================================
		// Method:       selectSingleNode
		//--------------------------------------------------------------------------------
		// Parameteres:  sXPath - XPath expression
		//--------------------------------------------------------------------------------
		// Returns:      XmlNode
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public XmlNode selectSingleNode(string sXPath)
		{
			
			System.Xml.XmlNode oXmlNode;
			oXmlNode = moSystemXmlNode.SelectSingleNode(sXPath);
			
			if (oXmlNode == null)
			{
				return null;
			}
			return new XmlNode(oXmlNode);
			
		}
		
		
		
		//================================================================================
		//Sub:       appendDocumentFragment
		//--------------------------------------------------------------------------------'
		//Purpose:   appends a string as new xml to the existing xml-document
		//--------------------------------------------------------------------------------'
		//Params:    the docfragment-string
		//--------------------------------------------------------------------------------'
		//Created:   09.04.2004 00:47:08
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void appendDocumentFragment (string sXmlDocFragment)
		{
			
			XmlDocumentFragment oDocFragment;
			oDocFragment = moSystemXmlNode.OwnerDocument.CreateDocumentFragment();
			
			oDocFragment.InnerXml = sXmlDocFragment;
			
			moSystemXmlNode.AppendChild(oDocFragment);
			
			
		}
		
		
		
		//================================================================================
		//Private Methods:
		//================================================================================
		
		//================================================================================
		//Function:  mbIsDataNode
		//--------------------------------------------------------------------------------'
		//Purpose:   decides, wether this node contains other elements and is by this way
		//           not a data node;
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Returns:   true: only data is stored; false: other elements are stored
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 00:32:15
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private bool mbIsDataNode()
		{
			
			for (int i = 0; i <= moSystemXmlNode.ChildNodes.Count - 1; i++)
			{
				if (moSystemXmlNode.ChildNodes[i].NodeType == XmlNodeType.Element)
				{
					return false;
				}
			}
			return true;
		}
		
	}
	
}
