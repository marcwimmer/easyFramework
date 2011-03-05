using System;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;

namespace easyFramework.Frontend.ASP.ComplexObjects
{
	//================================================================================
	//Class:     MultiStructureHelper

	//--------------------------------------------------------------------------------'
	//Module:    MultiStructureHelper.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   helper functions for interaction with multi-structure class
	//--------------------------------------------------------------------------------'
	//Created:   03.05.2004 16:26:56
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class MultiStructureHelper
	{



		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Sub:       gStoreMultiStructureInDatabase
		//--------------------------------------------------------------------------------'
		//Purpose:   stores the complete content of a dialog, which uses the multi-
		//           structure component in the database;
		//           new entities are inserted and exisiting are updated or deleted
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 16:32:47
		//--------------------------------------------------------------------------------'
		//Changed:   16.05.2004 - transactions added, Marc
		//--------------------------------------------------------------------------------'
		public static void gStoreMultiStructureInDatabase (ClientInfo oClientInfo, XmlDocument oXmlDialogData, System.Web.HttpRequest oRequest, System.Web.HttpApplicationState oApp, System.Web.HttpServerUtility oServer)
		{
	
	
			//-----------------load xml-document---------------------------------
			string sMultiXmlFilename;
			XmlDocument oMultiXml;
	
	
			if (oXmlDialogData.selectSingleNode("//multixmlfile") == null)
			{
				throw (new efException("The hidden-field \"multixmlfile\" must hold the name of " + "the used multi-xml dialog"));
			}
			else
			{
		
				sMultiXmlFilename = oXmlDialogData.selectSingleNode("//multixmlfile").sText;
				sMultiXmlFilename = oRequest.MapPath(sMultiXmlFilename);
				oMultiXml = new XmlDocument();
				oMultiXml.gLoad(sMultiXmlFilename);
		
			}
	
			//------------ start transaction and start try-block ---------------------
			oClientInfo.BeginTrans(efTransaction.efEnumIsolationLevels.efReadUncommitted);
	
			try
			{
		
				//-------------iterate every node and update the entity--------------------
				XmlNodeList oLevelNodes = oMultiXml.selectNodes("//level");
				for (int i = 0; i <= oLevelNodes.lCount - 1; i++)
				{
					mHandleLevelNode(oClientInfo, oLevelNodes[i], oRequest, oApp, oServer, oXmlDialogData);
				}
		
				//------------commit transaction--------------
				if (oClientInfo.bHasErrors)
				{
					oClientInfo.RollbackTrans();
				}
				else
				{
					oClientInfo.CommitTrans();
				}
		
			}
			catch (System.Exception ex)
			{
		
				oClientInfo.RollbackTrans();
		
				throw (ex);
		
			}
	
	
		}




		//================================================================================
		//Private Methods:
		//================================================================================


		//================================================================================
		//Sub:       mHandleLevelNode
		//--------------------------------------------------------------------------------'
		//Purpose:   stores the items of this node
		//--------------------------------------------------------------------------------'
		//Params:    the levelnode
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 17:44:12
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static void mHandleLevelNode (ClientInfo oClientInfo, XmlNode oLevelNode, System.Web.HttpRequest oRequest, System.Web.HttpApplicationState oApp, System.Web.HttpServerUtility oServer, XmlDocument oDialogOutput)
		{
	
			XmlNode oNode;
	
	
			//-------------get the entity-name and default-entity --------------------------
			string sEntityName;
			if (oLevelNode.selectSingleNode("entity") == null)
			{
				throw (new efException("Entity-node is missing. Please insert the entity-name, " + "otherwise the data cannot be stored!"));
			}
			else
			{
				sEntityName = oLevelNode.selectSingleNode("entity").sText;
			}
	
	
	
			//-------------collect the data from dialog--------------------------------------
			efArrayList aoDialogEntityData = maoGetElementValuesByLevel(oDialogOutput, oLevelNode);
	
			if (aoDialogEntityData == null)
			{
				return;
			}
	
			for (int i = 0; i <= aoDialogEntityData.Count - 1; i++)
			{
		
				efArrayList oFields = ((efArrayList)(aoDialogEntityData[i]));
		
				DefaultEntity oEntity = null;
				oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntityName);
		
				//---------get the level-praefix----------------------------------------------------------
				string sLevelPraefix = msGetLevelPraefix(((XmlNode)(oFields[0])).sName);
		
				//---------search for key-field, to either create a new entity or load an existing or delete the existing and the sub-elements--------
				oNode = moGetDialogNode(sLevelPraefix, oEntity.sKeyFieldName, oDialogOutput);
				if (oNode == null)
				{
					throw (new efException("Key-field \"" + oEntity.sKeyFieldName + "\" wasn't found in dialog."));
				}
		
				//-------------now either store or create a new survey--------------------
				if (Functions.IsEmptyString(oNode.sText))
				{
					oEntity.gNew(oClientInfo, "");
				}
				else
				{
					oEntity.gLoad(oClientInfo, oNode.sText);
				}
		
		
				//------------check if the entity is to delete; if so then remove all sub-nodes from the dialogoutput, so that those entities are not created-----
				if (oDialogOutput.selectSingleNode("//" + sLevelPraefix + "deleted") == null == false)
				{
			
					//delete the entity, but only, if it has a primary-key; otherweise it doesn't exist in the database
					if (!Functions.IsEmptyString(oEntity.oKeyField.sValue))
					{
				
						oEntity.gDelete(oClientInfo);
				
				
						XmlNodeList nlLevelNodes = oDialogOutput.selectNodes("//*");
				
						for (int iRemove = 0; iRemove <= nlLevelNodes.lCount - 1; iRemove++)
						{
					
							if (Functions.Left(nlLevelNodes[iRemove].sName, Functions.Len(sLevelPraefix)) == sLevelPraefix)
							{
						
								nlLevelNodes[iRemove].gRemoveFromParent();
						
							}
					
						}
					}
			
				}
				else
				{
			
			
			
					//------------now apply the other fields to the entity--------------------
					easyFramework.Sys.RecordsetObjects.Field oField;
					for (int z = 0; z <= oEntity.oFields.Count - 1; z++)
					{
						oField = oEntity.oFields[z];
				
						oNode = moGetDialogNode(sLevelPraefix, oEntity.oFields[z].sName, oDialogOutput);
						if (oNode != null)
						{
							oField.sValue = oNode.sText;
						}
				
					}
			
			
					//----------------if there is a sort-field, then put the sort-value into that field, because
					//           the field was updated via javascript -----------------------------
					if (oLevelNode.selectSingleNode("sortfield") == null == false)
					{
						string sSortField = oLevelNode.selectSingleNode("sortfield").sText;
				
						oNode = moGetDialogNode(sLevelPraefix, "__sortvalue", oDialogOutput);
				
						oEntity.oFields[sSortField].sValue = oNode.sText;
				
					}
			
			
					//------------fill-up the relations field ----------------------------
					if (oLevelNode.selectSingleNode("relation") == null == false)
					{
				
						//----------get relation-info-----------------
						string sParentLevelKeyFieldName;
						string sParentLevelKeyFieldValue;
						string sThisKeyFieldName;
				
						if (oLevelNode.selectSingleNode("relation/toplevel") == null)
						{
							throw (new efException("Relation-node requires element \"toplevel\"!"));
						}
						else
						{
							sParentLevelKeyFieldName = oLevelNode.selectSingleNode("relation/toplevel").sText;
						}
				
						if (oLevelNode.selectSingleNode("relation/thislevel") == null)
						{
							throw (new efException("Relation-node requires element \"thislevel\"!"));
						}
						else
						{
							sThisKeyFieldName = oLevelNode.selectSingleNode("relation/thislevel").sText;
						}
				
				
						//------------retrieve parent key-field values-------------------------
				
						if (mlGetLevelHierarchy(oLevelNode) == 1) //top-levels are exceptions:
						{
					
							//------------handle top-level element-------------------
							if (oDialogOutput.selectSingleNode("//" + sParentLevelKeyFieldName) == null)
							{
								throw (new efException("The value of top-most element \"" + sParentLevelKeyFieldName + "\" " + "wasn't found! If the web-component \"MultiStructure\" is used, then the property \"" + "sTopMostElementName\" has to be set."));
							}
							else
							{
								sParentLevelKeyFieldValue = oDialogOutput.selectSingleNode("//" + sParentLevelKeyFieldName).sText;
							}
					
							//------------set the key-field value in the entity-------------------
							if (oEntity.oFields.gbIsField(sThisKeyFieldName) == false)
							{
								throw (new efException("The field \"" + sThisKeyFieldName + "\" in the relation wasn't found in the entity!"));
							}
					
							oEntity.oFields[sThisKeyFieldName].sValue = sParentLevelKeyFieldValue;
					
					
						}
						else
						{
							//------------handle all other sub-elements-------------------
					
							oNode = moGetDialogNode(msMoveLevelUp(sLevelPraefix), sParentLevelKeyFieldName, oDialogOutput);
							if (oNode == null)
							{
								throw (new efException("The value of parent element \"" + sParentLevelKeyFieldName + "\" " + "wasn't found! Please make sure, that the fields, that are listed in the relation-" + "section of the multistructure-file exist in the parent-element and in the current-element."));
							}
							else
							{
								sParentLevelKeyFieldValue = oNode.sText;
							}
					
							//------------set the key-field value in the entity-------------------
							if (oEntity.oFields.gbIsField(sThisKeyFieldName) == false)
							{
								throw (new efException("The field \"" + sThisKeyFieldName + "\" in the relation wasn't found in the entity. "));
							}
					
							oEntity.oFields[sThisKeyFieldName].sValue = sParentLevelKeyFieldValue;
					
						}
				
						//------------now store the entity in the database--------------------
						bool bSaveResult = oEntity.gSave(oClientInfo);
						if (oClientInfo.bHasErrors)
						{
							return;
						}
						if (! bSaveResult)
						{
							oClientInfo.gAddError(oEntity.sName + " wasn't saved.");
							return;
						}
				
				
						//------------put the entity-id back into the dialog-xml, so that other elements can be appended------
						moGetDialogNode(sLevelPraefix, oEntity.oKeyField.sName, oDialogOutput).sText = oEntity.oKeyField.sValue;
				
				
					} //entity is deleted
			
				}
		
			}
	
		}



		//================================================================================
		//Function:  mbIsTopLevelNode
		//--------------------------------------------------------------------------------'
		//Purpose:   determines, if it is the top-most level
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 17:45:23
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static bool mbIsTopLevelNode(XmlNode oLevelNode)
		{
	
			if (oLevelNode.selectSingleNode("ancestor::level") == null)
			{
				return true;
			}
			else
			{
				return false;
			}
	
		}


		//================================================================================
		//Function:  mlGetLevelHierarchy
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the number of the hierarchy of the level
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   e.g. 1 for the top-level, 3 for the third level
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 18:12:36
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static int mlGetLevelHierarchy(XmlNode oLevelNode)
		{
			return oLevelNode.selectNodes("ancestor::level").lCount + 1;
		}

		//================================================================================
		//Function:  maoGetElementValuesByLevel
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the elements of the html-form of the given level-hierarchy
		//           grouped by each entry. the different values of an entry a collected
		//           in each nodelist.
		//           if you have three records e.g. then you have an array of three
		//           nodelists, where every nodelist contains the field of the record
		//--------------------------------------------------------------------------------'
		//Params:    oLevelNode - the levelnode, for which to get the data
		//--------------------------------------------------------------------------------'
		//Returns:   array of node-arrays; a nodelist contains the field of an entity record
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 17:54:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static efArrayList maoGetElementValuesByLevel(XmlDocument oDialogOutput, XmlNode oLevelNode)
		{
	
			efHashTable oHandledLevels = new efHashTable();
	
			XmlNodeList nlFlatAllNodes = oDialogOutput.selectNodes("//*");
	
			int lHierarchy = mlGetLevelHierarchy(oLevelNode);
	
			efArrayList oResult = new efArrayList();
	
			for (int i = 0; i <= nlFlatAllNodes.lCount - 1; i++)
			{
		
		
				//check if node matches hierarchy:
				if (mbNodeNameBelongsToHierarchy(nlFlatAllNodes[i].sName, lHierarchy))
				{
					string sLevelPraefix;
					sLevelPraefix = msExtractLevelOfCurrentLevelNode(nlFlatAllNodes[i].sName, lHierarchy);
			
			
					if (oHandledLevels.ContainsKey(sLevelPraefix) == false)
					{
				
						efArrayList oXmlNodes;
						oXmlNodes = new efArrayList();
				
						oHandledLevels.Add(sLevelPraefix, oXmlNodes);
				
						//-----------add to result-collection-----------
						oResult.Add(oXmlNodes);
				
					}
					else
					{
						efArrayList oXmlNodes = ((efArrayList)(oHandledLevels[sLevelPraefix]));
						oXmlNodes.Add(nlFlatAllNodes[i]);
				
					}
			
			
				}
		
		
		
			}
	
			return oResult;
	
		}


		//================================================================================
		//Function:  mbNodeNameBelongsToHierarchy
		//--------------------------------------------------------------------------------'
		//Purpose:   tests if the given node-name belongs to the level;
		//           the node-name looks like "LV_1_4_2_txtLoginName". this example-node has
		//           3 levels. so if hierarchy is 2 or 4 or 5 the node-name doesn't belong
		//           to the hierarchy
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 18:24:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static bool mbNodeNameBelongsToHierarchy(string sNodeName, int lHierarchy)
		{
	
			if (Functions.Left(sNodeName, 3) != "LV_")
			{
				return false;
			}
	
			string[] asSplitted = Functions.Split(sNodeName, "_", lHierarchy + 2);
	
			if (lHierarchy == 1)
			{
		
				if (asSplitted.Length < 2)
				{
					return false;
				}
		
				if (Functions.IsNumeric(asSplitted[1]))
				{
					return true;
				}
				else
				{
					return false;
				}
		
			}
			else
			{
				if (Functions.InStr(sNodeName, "_") == 0)
				{
					return false;
				}
		
				if (Functions.UBound(asSplitted) < lHierarchy)
				{
					return false;
				}
		
				if (! Functions.IsNumeric(asSplitted[lHierarchy - 1]))
				{
					return false;
				}
		
				if (asSplitted.Length >= lHierarchy)
				{
			
					if (Functions.IsNumeric(asSplitted[lHierarchy]))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
		
			}
	
	
		}



		//================================================================================
		//Function:  msExtractLevelOfCurrentLevelNode
		//--------------------------------------------------------------------------------'
		//Purpose:   if the node belongs to the current level, then the current level
		//           praefix is extracted
		//--------------------------------------------------------------------------------'
		//Params:    e.g. "LV_1_2_2_5", level 3
		//--------------------------------------------------------------------------------'
		//Returns:   e.g. "LV_1_2_2"
		//--------------------------------------------------------------------------------'
		//Created:   03.05.2004 18:47:26
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static string msExtractLevelOfCurrentLevelNode(string sNodeName, int lHierarchy)
		{
	
			string[] asSplitted = Functions.Split(sNodeName, "_");
			string sResult = "";
			for (int I = 0; I <= lHierarchy; I++)
			{
		
				sResult += asSplitted[I] + "_";
		
			}
	
			return sResult;
	
		}




		//================================================================================
		//Function:  moGetDialogNode
		//--------------------------------------------------------------------------------'
		//Purpose:   gets a node with data from the dialog-output; the praefixes txt,
		//           cbo, chk...are all tested
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   05.05.2004 08:52:25
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static XmlNode moGetDialogNode(string sLevelPraefix, string sFieldName, XmlDocument oDialogOutput)
		{
	
			if ( oDialogOutput.selectSingleNode("//" + sLevelPraefix + "txt" + sFieldName) != null)
			{
				return oDialogOutput.selectSingleNode("//" + sLevelPraefix + "txt" + sFieldName);
			}
			else if (oDialogOutput.selectSingleNode("//" + sLevelPraefix + "cbo" + sFieldName) != null)
			{
				return oDialogOutput.selectSingleNode("//" + sLevelPraefix + "cbo" + sFieldName);
			}
			else if ( oDialogOutput.selectSingleNode("//" + sLevelPraefix + "chk" + sFieldName) != null)
			{
				return oDialogOutput.selectSingleNode("//" + sLevelPraefix + "chk" + sFieldName);
			}
			else if ( oDialogOutput.selectSingleNode("//" + sLevelPraefix + "lbl" + sFieldName) != null)
			{
				return oDialogOutput.selectSingleNode("//" + sLevelPraefix + "lbl" + sFieldName);
			}
			else if ( oDialogOutput.selectSingleNode("//" + sLevelPraefix + "cmd" + sFieldName) != null)
			{
				return oDialogOutput.selectSingleNode("//" + sLevelPraefix + "cmd" + sFieldName);
			}
			else if (oDialogOutput.selectSingleNode("//" + sLevelPraefix + sFieldName) != null)
			{
				return oDialogOutput.selectSingleNode("//" + sLevelPraefix + sFieldName);
			}
			else
			{
				return null;
			}
	
	
		}



		//================================================================================
		//Function:  msGetLevelPraefix
		//--------------------------------------------------------------------------------'
		//Purpose:   gets "LV_1_1" from "LV_1_1_txtAns_id"
		//--------------------------------------------------------------------------------'
		//Params:    "LV_1_1_txtAns_id"
		//--------------------------------------------------------------------------------'
		//Returns:   "LV_1_1"
		//--------------------------------------------------------------------------------'
		//Created:   05.05.2004 09:00:27
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static string msGetLevelPraefix(string sNodeName)
		{
	
			if (Functions.InStr(sNodeName, "_") == 0)
			{
				throw (new efException("Invalid node-name " + sNodeName));
			}
	
			string[] asSplitted = Functions.Split(sNodeName, "_");
			string sResult = "";
	
			for (int i = 0; i <= asSplitted.Length - 1; i++)
			{
		
				switch (Functions.LCase(Functions.Left(asSplitted[i], 3)))
				{
					case "txt":
						return sResult;
				
					case "cbo":
						return sResult;
				
					case "lbl":
						return sResult;
				
					case "cmd":
						return sResult;
				
					case "chk":
				
						return sResult;
					default:
				
						sResult += asSplitted[i] + "_";
						break;
				}
		
			}
	
			return sResult;
	
		}

		//================================================================================
		//Function:  msMoveLevelUp
		//--------------------------------------------------------------------------------'
		//Purpose:   to move up a level
		//--------------------------------------------------------------------------------'
		//Params:    "LV_1_2_5_6_"
		//--------------------------------------------------------------------------------'
		//Returns:   "LV_1_1_5_"
		//--------------------------------------------------------------------------------'
		//Created:   05.05.2004 09:00:27
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private static string msMoveLevelUp(string sLevelPraefix)
		{
	
			string[] asSplitted = Functions.Split(sLevelPraefix, "_");
			string sResult = "";
			for (int i = 0; i <= asSplitted.Length - 3; i++)
			{
				sResult += asSplitted[i] + "_";
			}
			return sResult;
	
	
		}
	}























}
