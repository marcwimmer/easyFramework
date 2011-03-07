using System;
using System.Collections;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ASPTools;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     storeDialogData
	//--------------------------------------------------------------------------------'
	//Module:    storeDialogData.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   used to retrieve the data for loading into the dialog
	//--------------------------------------------------------------------------------'
	//Created:   05.05.2004 11:56:25
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================


	public class storeDialogData : efDataPage
	{
	
		#region " Vom Web Form Designer generierter Code "
	
		//Dieser Aufruf ist fr den Web Form-Designer erforderlich.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
		
		}
	
		//HINWEIS: Die folgende Platzhalterdeklaration ist fr den Web Form-Designer erforderlich.
		//Nicht lschen oder verschieben.
		private System.Object designerPlaceholderDeclaration;
	
		private void Page_Init (System.Object sender, System.EventArgs e)
		{
			//CODEGEN: Dieser Methodenaufruf ist fr den Web Form-Designer erforderlich
			//Verwenden Sie nicht den Code-Editor zur Bearbeitung.
			InitializeComponent();
		}
	
		#endregion
	
		public const string efColumnSep = ".|.";
		public const string efLineEnd = "-||-";
	
	
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
		
			//------------get parameters --------------------------
			string sMultiStructFilename;
			string sTopElementName;
			string sTopElementValue;
		
			if (oRequest.selectSingleNode("//multistructurexml") == null)
			{
				throw (new efException("Request-parameter \"multistructurexml\" needed!"));
			}
			else
			{
				sMultiStructFilename = oRequest.selectSingleNode("//multistructurexml").sText;
			}
		
			if (oRequest.selectSingleNode("//topelementname") == null)
			{
				throw (new efException("Request-parameter \"multistructurexml\" needed!"));
			}
			else
			{
				sTopElementName = oRequest.selectSingleNode("//topelementname").sText;
			}
		
			if (oRequest.selectSingleNode("//topelementvalue") == null)
			{
				throw (new efException("Request-parameter \"topelementvalue\" needed!"));
			}
			else
			{
				sTopElementValue = oRequest.selectSingleNode("//topelementvalue").sText;
			}
		
			//------------load the multistruct-xml-----------------
			XmlDocument oXml = new XmlDocument();
			oXml.gLoad(Tools.sWebToAbsoluteFilename(Request, sMultiStructFilename, false));
		
		
			//-----------check multistructure for needed fields---------
			if (oXml.selectSingleNode("/multistructure/topmostelement") == null)
			{
				throw (new efException("The \"topmostelement\"-tag must be given under tag \"multistructure\"!"));
			}
			if (oXml.selectSingleNode("/multistructure/topmostelement/entity") == null)
			{
				throw (new efException("The \"topmostelement/entity\"-tag must be given under tag \"multistructure\"!"));
			}
			string sTopMostElementEntity = oXml.selectSingleNode("/multistructure/topmostelement/entity").sText;
		
		
			//-------------begin with the top-most entity---------------------------
			FastString oResult = new FastString();
		
			DefaultEntity oEntity = EntityLoader.goLoadEntity(oClientInfo, sTopMostElementEntity);
		
			oEntity.gLoad(oClientInfo, sTopElementValue);
		
			//mWriteDataLine(oResult, "", "", sTopMostElementEntity & sTopElementValue, moGetFieldsFromEntity(oEntity))
		
		
			XmlNode oLevelNode = oXml.selectSingleNode("/multistructure/level");
			if (oLevelNode == null)
			{
				throw (new efException("Multistructure-xml needs level-nodes!"));
			}
		
		
			//---------------check, if it is a paged dialog----------------------------
			int lPageSize = easyFramework.Sys.ToolLib.DataConversion.glCInt(oXml.selectSingleNode("/multistructure")["pagesize"].sText, 0);
			int lPage = easyFramework.Sys.ToolLib.DataConversion.glCInt(oRequest.sGetValue("page", "0"), 0);
			if (lPageSize == 0)
			{
				lPage = 0;
			}
		
		
			mAppendSubEntities(oXml, oResult, oEntity, null, true, lPage, lPageSize);
		
		
		
			return "OK" + efLineEnd + oResult.ToString();
		
		}
	
	
	
	
	
		//================================================================================
		//Private Methods:
		//================================================================================
	
	
		//================================================================================
		//Sub:       mAppendSubEntities
		//--------------------------------------------------------------------------------'
		//Purpose:   appends all children-entities of the entity; called recursivley
		//--------------------------------------------------------------------------------'
		//Params:    oresult: the result-faststring
		//           oTopEntity: the loaded top-entity
		//           oLevelNode:
		//           lPage, lPageSize: if paged xml-multistructre, then put the values here;
		//                             if 0 then they are ignored
		//--------------------------------------------------------------------------------'
		//Created:   05.05.2004 15:51:12
		//--------------------------------------------------------------------------------'
		//Changed:   30.05.2004 - condition-tag
		//--------------------------------------------------------------------------------'
		private class Storage 
		{
			public int test = 0;
			public DefaultEntity oEntity = null;
			public XmlNode oLevelNode = null;
			public string sParentId = null;
		}
		private void mAppendSubEntities (XmlDocument oMultiStructXml, FastString oResult, 
			IEntity oTopEntity, XmlNode oParentLevelNode, bool bIsFirstLevel, 
			int lPage, int lPageSize)
		{
		
		
			string sThisLevel;
			string sTopLevel;
			string sEntity;
			efArrayList localCollectedEntites = new efArrayList();

		
			//-----------------get all sub-level-nodes of the parent-level---------
			XmlNodeList nlSubLevelNodes;
			if (oParentLevelNode == null)
			{
				nlSubLevelNodes = oMultiStructXml.selectNodes("/multistructure/level");
			}
			else
			{
				nlSubLevelNodes = oParentLevelNode.selectNodes("level");
			}
		
		
			//-------iterate each level-node--------
			/*
			 * be intelligent:
			 * 
			 * if all sub-levelnodes are of the same entity and are sorted by the same field,
			 * the arrange the entities of the subnodes by the sort-field.
			 * 
			 * */
			string lastSortCol = "";
			string lastEntity = "";
			bool bDiffer = false;
			for (int i = 0; i < nlSubLevelNodes.lCount && !bDiffer; i++) 
			{
				string sortCol = nlSubLevelNodes[i].sGetValue("sortfield", "", false);
				if (lastSortCol == "") lastSortCol = sortCol;
				string entity = nlSubLevelNodes[i].sGetValue("entity", "", false);
				if (lastEntity == "") lastEntity = entity;

				if (!lastEntity.ToLower().Equals( entity.ToLower()) || !lastSortCol.ToLower().Equals(sortCol)) 
				{
					bDiffer=true;
				}

			}

			//----------now collect all entities and their descendants---------------
			for (int i = 0; i <= nlSubLevelNodes.lCount - 1; i++)
			{
				XmlNode oLevelNode = nlSubLevelNodes[i];
			
				if (oLevelNode.selectSingleNode("relation") == null)
				{
					throw (new efException("Node \"relation\" required in level-node: " + oLevelNode.sXml));
				}
				if (oLevelNode.selectSingleNode("relation/thislevel") == null)
				{
					throw (new efException("Node \"relation/thislevel\" required in level-node: " + oLevelNode.sXml));
				}
				else
				{
					sThisLevel = oLevelNode.selectSingleNode("relation/thislevel").sText;
				}
				if (oLevelNode.selectSingleNode("relation/toplevel") == null)
				{
					throw (new efException("Node \"relation/toplevel\" required in level-node: " + oLevelNode.sXml));
				}
				else
				{
					sTopLevel = oLevelNode.selectSingleNode("relation/toplevel").sText;
				}
				if (oLevelNode.selectSingleNode("entity") == null)
				{
					throw (new efException("Entity-element expected for level-node"));
				}
				else
				{
					sEntity = oLevelNode.selectSingleNode("entity").sText;
				}
			
				DefaultEntity oEntity = null;
				oEntity=EntityLoader.goLoadEntity(oClientInfo, sEntity);
			
			
				//------------------get sort-column-----------------
				string sSortField;
				if (oLevelNode.selectSingleNode("sortfield") != null)
				{
					sSortField = oLevelNode.selectSingleNode("sortfield").sText;
				}
				else
				{
					sSortField = "";
				}
			
				//------------------select sub-entities of main entity ----------------------------
				string sClause;
				sClause = sThisLevel + "=" + oTopEntity.oFields[sTopLevel].sGetForSQL();
			
				Recordset rsSubEntities = oEntity.gRsSearch(oClientInfo, sClause, sSortField);
			
				//---------if paged, then jump to the offset--------------------
				if (lPage > 0)
				{
					rsSubEntities.Move((lPage - 1) * lPageSize);
				}
			
				int lCounter;
				lCounter = 0;
			
				while (!rsSubEntities.EOF)
				{
				
					//-----------load entity----------------
					oEntity = EntityLoader.goLoadEntity(oClientInfo, sEntity);
					oEntity.gLoad(oClientInfo, rsSubEntities[oEntity.sKeyFieldName].sValue);
				
					//----------if there is a condition-tag, check if condition is true--------
					bool bConditionMatching = true;
					if (oLevelNode.selectSingleNode("condition") != null )
					{
						bConditionMatching = mbIsCondition(oLevelNode.selectSingleNode("condition"), oEntity);
					}
				
					if (bConditionMatching == true)
					{
					
					
						//-------append children--------
					
						string sParentId;
						if (bIsFirstLevel)
						{
							sParentId = "";
						}
						else
						{
							sParentId = oTopEntity.sName + "_" + oTopEntity.oKeyField.sValue;
						}
						Storage storage = null;
						storage = new Storage();
						storage.test = lCounter;
						storage.oEntity = oEntity;
						storage.oLevelNode = oLevelNode;
						storage.sParentId = sParentId;
						localCollectedEntites.Add(storage);
						storage = null;
						oEntity = null;
					
					}
				
					rsSubEntities.MoveNext();
				
					//-----------if paged and page-limit is reach, then stop-------------
					lCounter += 1;
					if (lCounter >= lPageSize & lPageSize > 0)
					{
						break;
					}
				
				} 
			
			}


			//---------------now sort the collected entities-------------------
			if (!bDiffer) //nur wenn überall die gleichen entities und das gleiche sortfeld, dann
			{
				
				Comparer comparer = new Comparer(lastSortCol);
				localCollectedEntites.Sort(comparer);
			}


			//------------now append the sub-entities per parent---------------------
			for (int i = 0; i < localCollectedEntites.Count; i++) 
			{
				Storage storage = (Storage)localCollectedEntites[i];
				mWriteDataLine(oResult, storage.oLevelNode["name"].sText, storage.sParentId, storage.oEntity.sName + "_" + storage.oEntity.oKeyField.sValue, 
					moGetFieldsFromEntity(storage.oEntity));
				mAppendSubEntities(oMultiStructXml, oResult, storage.oEntity, storage.oLevelNode, false, 0, 0);
			}				

		
		}
		private class Comparer  : IComparer
		{
			private string msSortColumn = "";

			public Comparer(string sSortColumn) 
			{
				msSortColumn = sSortColumn;
			}

			// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
			public  int Compare( object x, object y) 
			{

				DefaultEntity x1 = ((Storage) x).oEntity;
				DefaultEntity y1 = ((Storage) y).oEntity;

				String sValue1 = x1.oFields[msSortColumn].sValue;
				String sValue2 = y1.oFields[msSortColumn].sValue;

				if (Sys.ToolLib.Functions.IsNumeric(sValue1) && Sys.ToolLib.Functions.IsNumeric(sValue2)) 
				{
					double f1 = Convert.ToDouble(sValue1);
					double f2 = Convert.ToDouble(sValue2);

					if (f1 > f2) return 1;
					else if(f1 < f2) return -1;
					else return 0;

				}
				else 
				{
					return String.Compare(sValue1, sValue2);
				}


			}

		}

	
	
		//================================================================================
		//Sub:       mWriteDataLine
		//--------------------------------------------------------------------------------'
		//Purpose:   write a concrete line to the result-string
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   05.05.2004 15:29:48
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private void mWriteDataLine (FastString oCurrentString, string sLevelName, string sParentID, string sId, efArrayList sColumns)
		{
		
		
			oCurrentString.Append(sLevelName + efColumnSep);
		
			oCurrentString.Append(sParentID + efColumnSep);
		
			oCurrentString.Append(sId + efColumnSep);
		
			for (int i = 0; i <= sColumns.Count - 1; i++)
			{
				TField oField = ((TField)(sColumns[i]));
				oCurrentString.Append(Functions.Replace(oField.sName, efColumnSep, " ") + efColumnSep);
				oCurrentString.Append(Functions.Replace(oField.sValue, efColumnSep, " ") + efColumnSep);
			}
		
			oCurrentString.Append(efLineEnd);
		}
	
	
	
		//================================================================================
		//Function:  oGetFieldsFromEntity
		//--------------------------------------------------------------------------------'
		//Purpose:   retrieves the columns from an entity and puts it into an arraylist
		//--------------------------------------------------------------------------------'
		//Params:    the entity
		//--------------------------------------------------------------------------------'
		//Returns:   arraylist
		//--------------------------------------------------------------------------------'
		//Created:   05.05.2004 15:25:58
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private efArrayList moGetFieldsFromEntity(DefaultEntity oEntity)
		{
		
			efArrayList oResult = new efArrayList();
		
			for (int i = 0; i <= oEntity.oFields.Count - 1; i++)
			{
			
				TField oField;
				oField = new TField();
				oField.sName = oEntity.oFields[i].sName;
			
				if (oEntity.oFields[i].enType == easyFramework.Sys.RecordsetObjects.Field.efEnumFieldType.efBool)
				{
					if (oEntity.oFields[i].bValue == true)
					{
						oField.sValue = "1";
					}
					else
					{
						oField.sValue = "0";
					}
				}
				else
				{
					oField.sValue = oEntity.oFields[i].sValue;
				}
			
				oResult.Add(oField);
			
			
			}
		
			return oResult;
		}
	
	
	
		//================================================================================
		//Function:  bIsCondition
		//--------------------------------------------------------------------------------'
		//Purpose:   checks, wether the condition tag matches or not
		//--------------------------------------------------------------------------------'
		//Params:    oConditionTag - the condition tag <condition...
		//           oEntity - the entity of the condition
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   30.05.2004 18:09:44
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		private bool mbIsCondition(XmlNode oConditionTag, IEntity oEntity)
		{
		
		
			//------------------check for entity-field---------------------
			string sFieldName;
			if (oConditionTag.selectSingleNode("entityfield") == null)
			{
				throw (new efException("Tag \"entityfield\" is missing under condition-tag!"));
			}
			else
			{
				sFieldName = oConditionTag.selectSingleNode("entityfield").sText;
			}
		
			//----------------get values-------------
			XmlNodeList nlValues = oConditionTag.selectNodes("value");
			if (nlValues.lCount == 0)
			{
				throw (new efException("At least one value-tag should be supplied under the condition-tag!"));
			}
		
			//----------check field-name--------------------------
			if (! oEntity.oFields.gbIsField(sFieldName))
			{
				throw (new efException("Invalid field-name \"" + sFieldName + "\" of entity \"" + oEntity.sName + "\"."));
			}
		
			//------------------------- iterate values --------------------
			string sFieldValue = oEntity.oFields[sFieldName].sValue;
			for (int i = 0; i <= nlValues.lCount - 1; i++)
			{
			
				if (Functions.LCase(nlValues[i].sText) == Functions.LCase(sFieldValue))
				{
					return true;
				
				}
			
			}
		
			return false;
		
		}
	
	
		private class TField
		{
			public string sName;
			public string sValue;
		}
	
	
	
	}

}
