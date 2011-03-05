using System;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;


namespace easyFramework.OnlineHelp
{
	//================================================================================
	//Class:     DatabaseUpdate

	//--------------------------------------------------------------------------------'
	//Module:    DatabaseUpdate.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   This class updates the database. It puts the xml-file into the db.
	//--------------------------------------------------------------------------------'
	//Created:   04.06.2004 18:34:36
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	public class DatabaseUpdate
	{
		
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		
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
		
		//================================================================================
//Sub:       gUpdate
		//--------------------------------------------------------------------------------'
//Purpose:   starts the database-update
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   04.06.2004 18:35:42
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		public static void gUpdate (ClientInfo oClientInfo)
		{
			
			
			//---------search for main-xml-file---------------
			string sMainXmlFile;
			HelpSystem oHelpSystem = HelpSystem.goGetFromClientInfo(oClientInfo);
			
			sMainXmlFile = oHelpSystem.sHelpDir + "\\help.xml";
			
			if (! System.IO.File.Exists(sMainXmlFile))
			{
				throw (new efException("File \"" + sMainXmlFile + "\" doesn't exist!"));
			}
			
			//----------load xml-document-----------
			XmlDocument oXml = new XmlDocument();
			oXml.gLoad(sMainXmlFile);
			
			//--------------delete current entries------------
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, "DELETE FROM tsHelpLinks");
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, "DELETE FROM tsHelpChapters");
			DataMethodsClientInfo.gExecuteQuery(oClientInfo, "DELETE FROM tsHelpToc");
			
			//--------iterate chapters----------
			XmlNodeList onlChapters = oXml.selectNodes("/help/chapter");
			mStoreChapters(oClientInfo, onlChapters);
			
			
			
		}
		//================================================================================
		//Protected Properties:
		//================================================================================
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       mStoreChapters
		//--------------------------------------------------------------------------------'
		//Purpose:   stores the given chapter-nodes
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 19:20:40
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected static void mStoreChapters (ClientInfo oClientInfo, XmlNodeList onlChapters)
		{
			
			if (onlChapters.lCount == 0)
			{
				return;
			}
			
			for (int i = 0; i <= onlChapters.lCount - 1; i++)
			{
				
				XmlNode oChapterNode = onlChapters[i];
				
				
				//------------get nodes and check obligatory fields------------------
				XmlNode oHeadingNode;
				XmlNode oBodyNode;
				XmlNode oLinkNode;
				XmlNode oParentChapter;
				
				oChapterNode = onlChapters[i];
				oHeadingNode = oChapterNode.selectSingleNode("heading");
				oBodyNode = oChapterNode.selectSingleNode("body");
				oLinkNode = oChapterNode.selectSingleNode("link");
				oParentChapter = oChapterNode.selectSingleNode("parent::chapter");
				
				
				if (oHeadingNode == null)
				{
					throw (new efException("Chapter-element requires \"heading\"-element."));
				}
				
				if (oBodyNode == null)
				{
					throw (new efException("Chapter-element requires \"body\"-element."));
				}
				
				if (Functions.Len(oHeadingNode.sText) > 255)
				{
					throw (new efException("The heading is limited to 255 characters: \"" + oHeadingNode.sText + "\""));
				}
				
				
				//------------set internal guid, to identify chapter------------
				Guid oGuid = Guid.NewGuid();
				oChapterNode.AddNode("guid", true).sText = oGuid.ToString();
				
				
				
				//------------insert toc into tsHelpToc-----------------
				Recordset rsToc = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpToc", "toc_id, toc_parentid, toc_title, toc_inserted", "1=2", "", "", "");
				rsToc.AddNew();
				rsToc["toc_id"].sValue = oGuid.ToString();
				if (oParentChapter == null == false)
				{
					rsToc["toc_parentid"].sValue = oParentChapter.selectSingleNode("guid").sText;
				}
				rsToc["toc_title"].sValue = oHeadingNode.sText;
				rsToc["toc_inserted"].dtValue = DateTime.Now;
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, rsToc.gsSqlInsert("tsHelpToc"));
				
				//------------insert toc into tsHelpChapters-----------------
				Recordset rsContent = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpChapters", "hlp_id, hlp_toc_id, hlp_parentid, hlp_heading, hlp_body, hlp_inserted, hlp_customid", "1=2", "", "", "");
				rsContent.AddNew();
				rsContent["hlp_id"].sValue = oGuid.ToString();
				rsContent["hlp_toc_id"].sValue = oGuid.ToString();
				if (oParentChapter == null == false)
				{
					rsContent["hlp_parentid"].sValue = oParentChapter.selectSingleNode("guid").sText;
				}
				rsContent["hlp_heading"].sValue = oHeadingNode.sText;
				mAdaptBodyNode(oClientInfo, oBodyNode);
				rsContent["hlp_body"].sValue = oBodyNode.sInnerXml;
				rsContent["hlp_inserted"].dtValue = DateTime.Now;
				if (oChapterNode.selectSingleNode("customid") == null == false)
				{
					rsContent["hlp_customid"].sValue = oChapterNode.selectSingleNode("customid").sText;
				}
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, rsContent.gsSqlInsert("tsHelpChapters"));
				
				
				//------------insert links, if there are any----------------
				if (oLinkNode != null)
				{
					for (int y = 0; y <= oLinkNode.oChildren.lCount - 1; y++)
					{
						
						XmlNode oLinkChild = oLinkNode.oChildren[y];
						
						Recordset rsLink = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsHelpLinks", "*", "1=2", "", "", "");
						rsLink.AddNew();
						rsLink["lnk_id"].sValue = Guid.NewGuid().ToString();
						rsLink["lnk_hlp_id"].sValue = rsContent["hlp_id"].sValue;
						if (oLinkChild.sName == "entity")
						{
							if (Functions.Len(oLinkChild.sText) > 50)
							{
								throw (new efException("Length of entity is limited to 50: \"" + oLinkChild.sText + "\""));
							}
							rsLink["lnk_entity"].sValue = oLinkChild.sText;
						}
						else if (oLinkChild.sName == "url")
						{
							if (Functions.Len(oLinkChild.sText) > 400)
							{
								throw (new efException("Length of url is limited to 400: \"" + oLinkChild.sText + "\""));
							}
							rsLink["lnk_url"].sValue = oLinkChild.sText;
						}
						rsLink["lnk_inserted"].dtValue = DateTime.Now;
						DataMethodsClientInfo.gExecuteQuery(oClientInfo, rsLink.gsSqlInsert("tsHelpLinks"));
						
						
					}
				}
				
				
				//-------get children and append them--------------
				XmlNodeList nlSubChapters = oChapterNode.selectNodes("chapter");
				mStoreChapters(oClientInfo, nlSubChapters);
				
			}
			
		}
		
		
		//================================================================================
//Sub:       mRenderBodyNode
		//--------------------------------------------------------------------------------'
//Purpose:   does some changes in the body-xml, like
		//           - appending the web-page root to the src-tags of images
		//           - changing the hrefs
		//--------------------------------------------------------------------------------'
//Params:
		//--------------------------------------------------------------------------------'
//Created:   06.06.2004 21:25:44
		//--------------------------------------------------------------------------------'
//Changed:
		//--------------------------------------------------------------------------------'
		protected static void mAdaptBodyNode (ClientInfo oClientInfo, XmlNode oXmlBodyNode)
		{
			
			
			//--------Adapt image-sources-------------
			XmlNodeList nlImages = oXmlBodyNode.selectNodes("descendant::img");
			for (int i = 0; i <= nlImages.lCount - 1; i++)
			{
				
				if (!Functions.IsEmptyString(nlImages[i].oAttributeList["src"].sText))
				{
					nlImages[i].oAttributeList["src"].sText = oClientInfo.oHttpApp.sApplicationPath() + nlImages[i].oAttributeList["src"].sText;
					
				}
				
			}
			
			//--------Adapt hrefs-------------
			XmlNodeList nlA = oXmlBodyNode.selectNodes("descendant::a");
			for (int i = 0; i <= nlA.lCount - 1; i++)
			{
				
				if (!Functions.IsEmptyString(nlA[i].oAttributeList["href"].sText) & nlA[i].oAttributeList["href"].sText != null)
				{
					
					string sCustomId = nlA[i].oAttributeList["href"].sText;
					
					if (!Functions.IsEmptyString(sCustomId))
					{
						nlA[i].oAttributeList["href"].sText = "#";
						nlA[i].oAttributeList["onclick"].sText = "mLoadContent('', '', '" + sCustomId + "'); return false;";
					}
					else
					{
						nlA[i].oAttributeList["href"].sText = "#";
						nlA[i].oAttributeList["onclick"].sText = "return false;";
						
					}
					
				}
				
			}
			
		}
		
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
