using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;


namespace easyFramework.Project.SurveyManager
{
	//================================================================================
	//Class:     HTMLPlaceHolders

	//--------------------------------------------------------------------------------'
	//Module:    HTMLPlaceHolders.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   This class replaces the placeholders in HTML; it iterates all parentgroups
	//           to get the correct values
	//
	//--------------------------------------------------------------------------------'
	//Created:   27.05.2004 00:14:27
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	//================================================================================
	//Imports:
	//================================================================================

	namespace Rendering
	{
	
		public class HTMLPlaceHolders
		{
		
		
			//================================================================================
			//Public Methods:
			//================================================================================
		
		
			//================================================================================
			//Function:  gsGetReplacedHtml
			//--------------------------------------------------------------------------------'
			//Purpose:   replaces the html with the replacement-values
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Returns:
			//--------------------------------------------------------------------------------'
			//Created:   27.05.2004 00:16:44
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			public static string gsGetReplacedHtml(string sConnStr, int lsvy_id, string sHtml)
			{
			
				XmlDocument oXmlPlaceHolders = new XmlDocument("<placeholders/>");
			
				//---get survey's placeholders--
				Recordset rsPlaceHolders = DataMethods.gRsGetTable(sConnStr, "tdPlaceHolders", "*", "plc_svy_id=" + lsvy_id, "", "", "");
				Recordset rsSurvey = DataMethods.gRsGetTable(sConnStr, "tdSurveys", "svy_svg_id", "svy_id=" + lsvy_id, "", "", "");
			
				mAppendPlaceHolders(ref oXmlPlaceHolders, rsPlaceHolders);
			
				//--------start recursion--------
				mAppendPlaceHoldersOfSurveyGroup(sConnStr, ref oXmlPlaceHolders, rsSurvey["svy_svg_id"].lValue);
			
			
				//--------replace in html----------
				XmlNodeList nlPlaceHolders = oXmlPlaceHolders.selectNodes("//placeholder");
				for (int i = 0; i <= nlPlaceHolders.lCount - 1; i++)
				{
				
					string sName = nlPlaceHolders[i].selectSingleNode("name").sText;
					string sValue = nlPlaceHolders[i].selectSingleNode("value").sText;
				
					sHtml = Functions.Replace(sHtml, sName, sValue);
				
				
				}
			
				//-------------return result
				return sHtml;
			
			}
		
			//================================================================================
			//Protected Methods:
			//================================================================================
		
		
			//================================================================================
			//Sub:       mAppendPlaceHoldersOfSurveyGroup
			//--------------------------------------------------------------------------------'
			//Purpose:   recursivley iterates all parent-surveygroups and builds the
			//           placeholders-xml
			//--------------------------------------------------------------------------------'
			//Params:
			//--------------------------------------------------------------------------------'
			//Created:   27.05.2004 00:22:06
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			protected static void mAppendPlaceHoldersOfSurveyGroup (string sConnStr, ref XmlDocument oXmlPlaceHolders, int lSvg_id)
			{
			
				Recordset rsSurveyGroup = DataMethods.gRsGetTable(sConnStr, "tdSurveyGroups", "svg_parentid", "svg_id=" + lSvg_id, "", "", "");
			
				if (! rsSurveyGroup.EOF)
				{
				
					Recordset rsPlaceHolders = DataMethods.gRsGetTable(sConnStr, "tdPlaceHolders", "*", "plc_svg_id=" + lSvg_id, "", "", "");
				
					mAppendPlaceHolders(ref oXmlPlaceHolders, rsPlaceHolders);
				
				
					if (rsSurveyGroup["svg_parentid"].lValue > 0)
					{
						mAppendPlaceHoldersOfSurveyGroup(sConnStr, ref oXmlPlaceHolders, rsSurveyGroup["svg_parentid"].lValue);
					}
				
				}
			
			}
		
			//================================================================================
			//Sub:       gAppendPlaceHolders
			//--------------------------------------------------------------------------------'
			//Purpose:   appends the placeholders to the oXmlPlaceHolders-Ref-Parameter
			//--------------------------------------------------------------------------------'
			//Params:    oXmlPlaceHolders - xml-document, where the placeholders are appended
			//           rsPlaceHolders - recordset of table tdPlaceHolders
			//--------------------------------------------------------------------------------'
			//Created:   27.05.2004 00:20:10
			//--------------------------------------------------------------------------------'
			//Changed:
			//--------------------------------------------------------------------------------'
			protected static void mAppendPlaceHolders (ref XmlDocument oXmlPlaceHolders, Recordset rsPlaceHolders)
			{
			
				XmlNode oBaseNode = oXmlPlaceHolders.selectSingleNode("/placeholders");
			
				while (! rsPlaceHolders.EOF)
				{
				
					string sName = rsPlaceHolders["plc_name"].sValue;
					string sValue = rsPlaceHolders["plc_value"].sValue;
				
					//------only add item, if it doesn't exist------
					if (oXmlPlaceHolders.selectSingleNode("//name[text()='" + sName + "']") == null)
					{
						XmlNode oNewNode = oBaseNode.AddNode("placeholder", false);
						oNewNode.AddNode("name", true).sText = rsPlaceHolders["plc_name"].sValue;
						oNewNode.AddNode("value", true).sText = rsPlaceHolders["plc_value"].sValue;
					}
				
					rsPlaceHolders.MoveNext();
				} 
			
			
			}
		
		}
	}

}
