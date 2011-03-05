using easyFramework.Sys;
using System;
using easyFramework;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;

namespace easyFramework.Project.Default
{
	//================================================================================
	//Class:     renderAllLevelForReload

	//--------------------------------------------------------------------------------'
	//Module:    renderAllLevelForReload.aspx.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   optimizes the loading of a multistructure
	//           usually each level is rendered via the renderLevel.aspx
	//           if the multistructure has to load 10 items with several
	//           sub-items, then each time a web-access to the renderlevel.aspx
	//           has to be done;
	//           this asp renders the html of all elements in a multistructure
	//           and returns it.
	//--------------------------------------------------------------------------------'
	//Created:   02.05.2004 18:56:53
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	//================================================================================
	//Imports:
	//================================================================================

	public class renderAllLevelForReload : efDataPage
	{
		public renderAllLevelForReload()
		{
			oXmlDefinition = new XmlDocument();
			oDataIdToLevelId = new efHashTable();
			oLevelIds = new efArrayList();
		}
	
	
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
	
		protected string sXmlFilename;
		protected string sXmlDialogPraefix;
		protected string sHtmlFormName;
		protected XmlDocument oXmlDefinition;
	
		protected efHashTable oDataIdToLevelId; //stores the key in the data-stream and the corresponding level-id
	
	
		public override string sGetData(ClientInfo oClientInfo, Sys.Xml.XmlDocument oRequest)
		{
		
		
			sHtmlFormName = oRequest.sGetValue("formname");
			sXmlFilename = oRequest.sGetValue("multixml");
			string sData = oRequest.sGetValue("data");
			sXmlDialogPraefix = oRequest.sGetValue("xmldialogpraefix");
		
		
			//-------------------load the xml-definition of the struct----------------------
			if (Functions.Left(sXmlFilename, 1) != "/")
			{
				throw (new efException("XML-structure filename must be absolute for calling multi-structure ondemand renderer."));
			}
		
		
			sXmlFilename = Server.MapPath(sXmlFilename);
			oXmlDefinition.gLoad(sXmlFilename);
		
		
		
			//-----------------render and return html------------------------------
			MultistructureRenderer oMultiStructure = new MultistructureRenderer();
		
			FastString oResultHtml = new FastString();
			if (Functions.Left(sData, 6) == "OK-||-")
			{
				sData = Functions.Right(sData, Functions.Len(sData) - 6);
			}
			string[] asSplittedDatalines = Functions.Split(sData, "-||-");
		
		
			gBuildHtml(ref oResultHtml, ref asSplittedDatalines, 0, oMultiStructure, "");
		
		
		
			return "OK-||-" + oResultHtml.ToString();
		
		
		}
	
	
		//================================================================================
		//Sub:       gBuildHtml
		//--------------------------------------------------------------------------------'
		//Purpose:   makes HTML out of a line of the data; steps through the data-lines and
		//           calls itself iterativley
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   17.06.2004 16:02:57
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gBuildHtml (ref FastString oResult, ref string[] asSplittedDatalines, int lDataLine, MultistructureRenderer oMultiStructure, string sParentLevelId)
		{
		
		
			string sLevelName;
			string sId;
			string sParentId;
		
			//-----------get values from data-line-------------------
			for (int i = lDataLine; i <= asSplittedDatalines.Length - 2; i++)
			{
			
				string[] asDataLine = Functions.Split(asSplittedDatalines[i], ".|.", -1);
			
				sLevelName = asDataLine[0];
				sId = asDataLine[2];
			
				//-----transform the parent-id to a level-id--------------
				sParentId = asDataLine[1];
				sParentId = msResolveIdToLevelId(sParentId);
			
				string sLevelId = msGetNextLevelId(sParentId);
				mAddNewLevelIdPair(sLevelId, sId);
			
			
				//------------------get the data, to be added to the dialog---------------
				XmlDocument oXmlData = moXmlGetDataForDialogInput(asSplittedDatalines[i]);
			
			
				//-------------------build result html-----------------------
			
				oResult.Append(sLevelId);
			
				oResult.Append("||||||--------||||||");
			
				oResult.Append(oMultiStructure.gsRenderSpecificLevel(oClientInfo, oXmlDefinition, sHtmlFormName, sXmlDialogPraefix + sLevelId, sLevelId, sLevelName, Request, Application, Server, oXmlData));
			
			
			
				oResult.Append("||||||*****||||||");
			
			
			}
		
		
		}
	
	
		//================================================================================
		//Function:  msGetNextLevelId
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the next-level-id
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   17.06.2004 16:21:38
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected efArrayList oLevelIds; //for optimized storing of the level-ids
		protected string msGetNextLevelId(string sParentLevelId)
		{
		
			if (Functions.IsEmptyString(sParentLevelId))
			{
			
				string sLevelId = "";
				for (int i = oLevelIds.Count - 1; i >= 0; i--)
				{
				
					sLevelId = easyFramework.Sys.ToolLib.DataConversion.gsCStr(oLevelIds[i]);
				
					if (Functions.Split(sLevelId, "_").Length == 3)
					{
						break;
					}
				
				}
			
				string sNextLevelId;
				if (Functions.IsEmptyString(sLevelId))
				{
					sNextLevelId = "LV_1_";
				}
				else
				{
					int lNumber = easyFramework.Sys.ToolLib.DataConversion.glCInt(Functions.Split(sLevelId, "_", -1)[1], 0);
					lNumber += 1;
				
					sNextLevelId = "LV_" + lNumber + "_";
				
				}
			
				oLevelIds.Add(sNextLevelId);
			
				return sNextLevelId;
			
			}
			else
			{
			
				string sLastLevelId = "";
				string sLevelId = "";
				int lHierarchyParent = Functions.Split(sParentLevelId, "_", -1).Length;
				for (int i = oLevelIds.Count - 1; i >= 0; i--)
				{
				
					sLevelId = easyFramework.Sys.ToolLib.DataConversion.gsCStr(oLevelIds[i]);
				
					if (Functions.Left(sLevelId, Functions.Len(sParentLevelId)) == sParentLevelId)
					{
					
						if (Functions.Split(sLevelId, "_", -1).Length - 1 == lHierarchyParent)
						{
							sLastLevelId = sLevelId;
							break;
						}
					
					}
				
				}
			
				string sNextLevelId;
				if (Functions.IsEmptyString(sLastLevelId))
				{
					sNextLevelId = sParentLevelId + "1_";
				}
				else
				{
					int lNumber = DataConversion.glCInt(
						Functions.Split(sLastLevelId, "_")
						[Functions.Split(sLastLevelId, "_").Length - 2], 0);
					lNumber += 1;
				
					sNextLevelId = sParentLevelId + lNumber + "_";
				
				}
			
				oLevelIds.Add(sNextLevelId);
			
				return sNextLevelId;
			
			}
		
		}
	
	
	
		//================================================================================
		//Function:  msResolveIdToLevelId
		//--------------------------------------------------------------------------------'
		//Purpose:   searches, wether for then given data-id a level-id exists
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   17.06.2004 16:24:10
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected string msResolveIdToLevelId(string sId)
		{
		
			if (sId != "")
			{
			
				if (oDataIdToLevelId.ContainsKey(sId))
				{
					return DataConversion.gsCStr(oDataIdToLevelId[sId]);
				}
			}

			return "";
		
		}
	
	
		//================================================================================
		//Sub:       mAddNewLevelIdPair
		//--------------------------------------------------------------------------------'
		//Purpose:   adds a new pair to the hash-table
		//--------------------------------------------------------------------------------'
		//Params:    sLevelId - the id of the level
		//           sDataId - the data-value
		//--------------------------------------------------------------------------------'
		//Created:   17.06.2004 16:29:45
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected void mAddNewLevelIdPair (string sLevelId, string sDataId)
		{
		
			if (! oDataIdToLevelId.ContainsKey(sDataId))
			{
				oDataIdToLevelId.Add(sDataId, sLevelId);
			}
		
		}
	
	
		//================================================================================
		//Function:  mlGetNextSortValueOfLevel
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   18.06.2004 00:51:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected int mlGetNextSortValueOfLevel(string sLevelId)
		{
		
			int sLastNumber = easyFramework.Sys.ToolLib.DataConversion.glCInt(Functions.Split(sLevelId, "_")
				[Functions.UBound(Functions.Split(sLevelId, "_")) - 1], 0);
			return sLastNumber;
		
		}
	
		//================================================================================
		//Function:  moXmlGetDataForDialogInput
		//--------------------------------------------------------------------------------'
		//Purpose:   transforms an data-line of multistructure, to a dialog-input
		//           xml-document
		//--------------------------------------------------------------------------------'
		//Params:    the line of data of the multistructure
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   17.06.2004 23:46:24
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected XmlDocument moXmlGetDataForDialogInput(string sDataLine)
		{
		
			string[] asSplitted = Functions.Split(sDataLine, ".|.", -1);
		
			XmlDocument oXmlResult = new XmlDocument("<DIALOGINPUT/>");
		
			for (int i = 3; i <= asSplitted.Length - 1; i += 2)
			{
				if (asSplitted[i] != "")
				{
					oXmlResult.selectSingleNode("/DIALOGINPUT").AddNode(asSplitted[i], true).sText = asSplitted[i + 1];
				}
			}
		
			return oXmlResult;
		
		}
	
	}

}
