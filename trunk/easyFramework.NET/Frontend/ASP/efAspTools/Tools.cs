using Microsoft.VisualBasic;
using System;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.Entities;
using System.Text;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     Tools

	//--------------------------------------------------------------------------------'
	//Module:    Tools.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   common asp-Tools
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 16:17:37
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'


	public class Tools
	{


		//================================================================================
		//Private Fields:
		//================================================================================


		//================================================================================
		//Public Consts:
		//================================================================================
		public enum efEnumDialogInputType
		{
			efXml,
			efJavaScriptString
		}

		//================================================================================
		//Public Methods:
		//================================================================================


		//================================================================================
		//Function:  gsCallServerProcess
		//--------------------------------------------------------------------------------'
		//Purpose:   calls the given URL and returns the content of the page as string
		//           the serverxmlhttp-component is used for this
		//           the xmlhttp-componente is limited to two simultanoues connection
		//           (dr. gui, microsoft). the serverXmlHTTP can use several
		//           simultaneous connections
		//--------------------------------------------------------------------------------'
		//Params:    sURL - the URL to call
		//--------------------------------------------------------------------------------'
		//Returns:   the string or raises error, if connection fails
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 16:20:30
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsCallServerProcess(ClientInfo oClientInfo, string sURL, string sParams)
		{
	
			MSXML2.ServerXMLHTTP50Class oServerXMLHTTP = new MSXML2.ServerXMLHTTP50Class();
	
	
			//decide, wether to append clientID or not:
			string sMethod;
			if (Functions.IsEmptyString(sParams))
			{
				sParams += "?ClientID=" + oClientInfo.sClientID;
				sMethod = "GET"; //use get, whenever possible; is logged in IIS-Log;
			}
			else
			{
				if (!(Functions.InStr(sParams, "?ClientID=", 0) > 0 | Functions.InStr(sParams, "&ClientID=", 0) > 0))
				{
					sParams += "&ClientID=" + oClientInfo.sClientID;
				}
				sMethod = "POST";
			}
	
			if (sMethod == "GET")
			{
				oServerXMLHTTP.open(sMethod, sURL, false, null, null);
			}
			else if (sMethod == "POST")
			{
				oServerXMLHTTP.open(sMethod, sURL, null, null, null);
				oServerXMLHTTP.send(sParams);
			}
	
			return oServerXMLHTTP.responseText;
	
		}

		/// <summary>
		/// posts the binary data to the given url via post-method
		/// 
		/// the url may include parameters like ...aspx?param1=value1&param2=value2
		/// </summary>
		/// <param name="oClientInfo"></param>
		/// <param name="abData"></param>
		/// <param name="sData"></param>
		/// <returns>the response of the called url</returns>
		public static string gsPostStringData(ClientInfo oClientInfo, string sURL, string sData) 
		{
			MSXML2.ServerXMLHTTP50Class oServerXMLHTTP = new MSXML2.ServerXMLHTTP50Class();

			
			oServerXMLHTTP.open("POST", sURL, false, null, null);
			oServerXMLHTTP.send(sData);

			return Convert.ToString(oServerXMLHTTP.responseText);
		}
			
		/// <summary>
		/// posts the binary data to the given url via post-method
		/// 
		/// the url may include parameters like ...aspx?param1=value1&param2=value2
		/// </summary>
		/// <param name="oClientInfo"></param>
		/// <param name="abData"></param>
		/// <param name="oXmlData"></param>
		/// <param name="sURL"></param>
		/// <returns>the response of the called url</returns>
		public static string gsPostXmlData(ClientInfo oClientInfo, string sURL, XmlDocument oXmlData) 
		{
			MSXML2.ServerXMLHTTP50Class oServerXMLHTTP = new MSXML2.ServerXMLHTTP50Class();

			oServerXMLHTTP.open("POST", sURL, false, null, null);
			oServerXMLHTTP.send(oXmlData.sXml);

			return Convert.ToString(oServerXMLHTTP.responseText);
		}
		//================================================================================
		//Function:  bIsRelativeFilename
		//--------------------------------------------------------------------------------'
		//Purpose:   determines, wether the given filename is relative or absolute
		//--------------------------------------------------------------------------------'
		//Params:    sFilename (string)
		//--------------------------------------------------------------------------------'
		//Returns:   true/false
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 16:18:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static bool gbIsRelativeFilename(string sFilename)
		{
	
			if (!Functions.IsEmptyString(sFilename))
			{
				if (Strings.Left(sFilename, 1) == "/")
				{
					return false;
				}
			}
			return true;
	
		}





		//================================================================================
		//Function:  gsXmlsRecordset2DialogInput
		//--------------------------------------------------------------------------------'
		//Purpose:   transforms the record-set, so that it can be used as dialog-input
		//--------------------------------------------------------------------------------'
		//Params:    the recordset
		//           lTotalRecordcount - if the input is for a paged-xml-dialog, then
		//           enter here the total-recordcount
		//           enFormat - either the string for a javascript to decode(in a DataPage)
		//                      or for sXmlValues
		//--------------------------------------------------------------------------------'
		//Returns:   <DIALOGINPUT>
		//--------------------------------------------------------------------------------'
		//Created:   29.03.2004 02:23:00
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsXmlRecordset2DialogInput(Recordset oRs, efEnumDialogInputType enFormat, int lTotalRecordcount)
		{
	
	
	
			if (oRs.lRecordcount == 0)
			{
		
				switch (enFormat)
				{
					case efEnumDialogInputType.efJavaScriptString:
				
				
						string sResult = "";
						//-------------------- append record-count-------------------
						if (lTotalRecordcount > -1)
						{
							sResult += "__CONST|XmlPagedDialogRecordcount|" + lTotalRecordcount + "|" + "-||-";
						}
				
						sResult = "OK" + "-||-" + sResult;
				
						return sResult;
				
					case efEnumDialogInputType.efXml:
				
						return "<DIALOGINPUT/>";
				}

				throw new efException("Unknown format at gsXmlRecordset2DialogInput: " + 
					DataConversion.gsCStr(enFormat));
		
			}
			else
			{
		
		
				StringBuilder osBuilder = new StringBuilder();
				XmlDocument x = new XmlDocument();
		
		
				switch (enFormat)
				{
					case efEnumDialogInputType.efJavaScriptString:
				
						break;
				
					case efEnumDialogInputType.efXml:
				
						x.gLoad("<DIALOGINPUT/>");
						break;
				}
		
		
		
				oRs.MoveFirst();
				int lCounter = 0;
		
				osBuilder.Append("OK" + "-||-");
		
				//-------------------- append record-count-------------------
				if (lTotalRecordcount > -1)
				{
					osBuilder.Append("__CONST|XmlPagedDialogRecordcount|" + lTotalRecordcount + "|" + "-||-");
				}
		
		
				while (! oRs.EOF)
				{
			
			
					XmlNode oNodeToAppendField = null;
					if (enFormat == efEnumDialogInputType.efXml)
					{
						if (oRs.lRecordcount == 1)
						{
							oNodeToAppendField = x.selectSingleNode("/DIALOGINPUT");
						}
						else
						{
							oNodeToAppendField = x.selectSingleNode("/DIALOGINPUT").AddNode("ROW", false);
							oNodeToAppendField["NUMBER"].sText = DataConversion.gsCStr(lCounter);
						}
					}
			
					//row-number:
					osBuilder.Append("__ROW|" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lCounter) + "|" + "-||-");
			
					for (int i = 0; i <= oRs.oFieldDefList.Count - 1; i++)
					{
				
						switch (enFormat)
						{
							case efEnumDialogInputType.efXml:
						
								oNodeToAppendField.AddNode(oRs.oFieldDefList[i].sName, true).sText = oRs.oFields[i].sValue;
								break;
							case efEnumDialogInputType.efJavaScriptString:
						
						
								//name of the field:
								osBuilder.Append(oRs.oFields[i].sName + "|");
								//value of the field:
							switch (oRs.oFields[i].enType)
							{
								case easyFramework.Sys.RecordsetObjects.Field.efEnumFieldType.efBool:
								
									if (oRs.oFields[i].bValue == true)
									{
										osBuilder.Append("1|");
									}
									else
									{
										osBuilder.Append("0|");
									}
									break;
								default:
								
									osBuilder.Append(oRs.oFields[i].sValue + "|");
									break;
							}
								osBuilder.Append("-||-");
								break;
						
						}
					}
			
			
			
					oRs.MoveNext();
					lCounter += 1;
			
				} ;
		
		
				switch (enFormat)
				{
					case efEnumDialogInputType.efJavaScriptString:
				
						return osBuilder.ToString();
					case efEnumDialogInputType.efXml:
				
						return x.sXml;
				}
		
				throw new efException("Unknown format at gsXmlRecordset2DialogInput: " + 
					DataConversion.gsCStr(enFormat));
			}
	
	
		}


		//================================================================================
		//Function:  grsDialogOutput2Recordset
		//--------------------------------------------------------------------------------'
		//Purpose:   transforms the output of a dialog into a recordset; the dialog
		//           praefixes are removed;
		//--------------------------------------------------------------------------------'
		//Params:    the dialogoutput-xml
		//--------------------------------------------------------------------------------'
		//Returns:   the recordset
		//--------------------------------------------------------------------------------'
		//Created:   08.04.2004 23:34:43
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static Recordset grsDialogOutput2Recordset(string sXmlDialogoutput)
		{
	
			Recordset rsResult = new Recordset();
			XmlDocument oXmlDialogOutput = new XmlDocument(sXmlDialogoutput);
	
			XmlNode oBaseNode = oXmlDialogOutput.selectSingleNode("//DIALOGOUTPUT");
	
	
			bool bMultiRow;
			if (oBaseNode.selectSingleNode("ROW") == null)
			{
				bMultiRow = false;
			}
			else
			{
				bMultiRow = true;
			}
	
			//create fields:
			XmlNode oParentNode;
			if (bMultiRow == true)
			{
				//get the first row, to make the fields, if multirow:
				oParentNode = oBaseNode.selectSingleNode("ROW");
			}
			else
			{
				oParentNode = oBaseNode;
			}
	
			XmlNodeList nlChildNodes = oParentNode.selectNodes("*");
			efHashTable oTranslatedFields = new efHashTable();
	
			for (int i = 0; i <= nlChildNodes.lCount - 1; i++)
			{
		
				string sOldFieldName = nlChildNodes[i].sName;
				string sNewFieldName;
				if (Strings.LCase(Strings.Left(sOldFieldName, 3)) == "txt")
				{
					sNewFieldName = Strings.Right(sOldFieldName, Strings.Len(sOldFieldName) - 3);
				}
				else if (Strings.LCase(Strings.Left(sOldFieldName, 3)) == "lbl")
				{
					sNewFieldName = Strings.Right(sOldFieldName, Strings.Len(sOldFieldName) - 3);
				}
				else if (Strings.LCase(Strings.Left(sOldFieldName, 3)) == "chk")
				{
					sNewFieldName = Strings.Right(sOldFieldName, Strings.Len(sOldFieldName) - 3);
				}
				else if (Strings.LCase(Strings.Left(sOldFieldName, 3)) == "cmd")
				{
					sNewFieldName = Strings.Right(sOldFieldName, Strings.Len(sOldFieldName) - 3);
				}
				else if (Strings.LCase(Strings.Left(sOldFieldName, 3)) == "cbo")
				{
					sNewFieldName = Strings.Right(sOldFieldName, Strings.Len(sOldFieldName) - 3);
				}
				else
				{
					sNewFieldName = sOldFieldName;
				}
		
				oTranslatedFields.Add(sOldFieldName, sNewFieldName);
		
				rsResult.AppendField(sNewFieldName, easyFramework.Sys.RecordsetObjects.Field.efEnumFieldType.efUndefined);
		
		
			}
	
	
			//iterate rows and add values:
			XmlNodeList nlRowNodes;
			if (bMultiRow == true)
			{
				nlRowNodes = oBaseNode.selectNodes("ROW");
			}
			else
			{
				nlRowNodes = oBaseNode.selectNodes(".");
			}
	
			for (int i = 0; i <= nlRowNodes.lCount - 1; i++)
			{
				rsResult.AddNew();
		
				nlChildNodes = nlRowNodes[i].selectNodes("*");
				for (int y = 0; y <= nlChildNodes.lCount - 1; y++)
				{
					if (oTranslatedFields[nlChildNodes[y].sName] != null)
					{
						rsResult.oFields[DataConversion.gsCStr(oTranslatedFields[nlChildNodes[y].sName])].sValue = nlChildNodes[y].sText;
					}
			
			
				}
			}
	
	
			//---------move first-------------
			rsResult.MoveFirst();
	
	
			//----------return-----------------
			return rsResult;
	
		}

		//================================================================================
		//Function:  GetUniqueString
		//--------------------------------------------------------------------------------'
		//Purpose:   returns a string, that is unique; without any special characters like
		//           { } -
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:   string
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 16:15:49
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string sGetUniqueString()
		{
			return sGetUniqueString(true, 32);
		}
		public static string sGetUniqueString(bool bNoNumbers, int lMaxLength)
		{
	
			string s;
			s = System.Guid.NewGuid().ToString() + System.Guid.NewGuid().ToString() + System.Guid.NewGuid().ToString();
			s = Strings.Replace(s, "{", "", 1, -1, 0);
			s = Strings.Replace(s, "}", "", 1, -1, 0);
			s = Strings.Replace(s, "-", "", 1, -1, 0);
	
			if (bNoNumbers)
			{
				for (int i = 0; i <= 9; i++)
				{
					s = Strings.Replace(s, DataConversion.gsCStr(i), "", 1, -1, 0);
				}
			}
	
			return s;
	
		}



		//================================================================================
		//Function:  sEntityToDataTableString
		//--------------------------------------------------------------------------------'
		//Purpose:   transforms the entity, so that it can be shown in the
		//           data-table
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 13:09:59
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsEntityToDataTableString(IEntity sEntity, Recordset rsEntities, int lTotalRecordcount)
		{
	
			StringBuilder oSBuilder = new StringBuilder();
	
			oSBuilder.Append("OK-||-");
			oSBuilder.Append(easyFramework.Sys.ToolLib.DataConversion.gsCStr(lTotalRecordcount) + "|");
			oSBuilder.Append("-||-");
	
	
			//Captions:
			for (int i = 0; i <= sEntity.asDataTableCaptions.Length - 1; i++)
			{
				oSBuilder.Append(sEntity.asDataTableCaptions[i] + "|");
			}
			oSBuilder.Append("-||-");
	
			//ColWidths:
			for (int i = 0; i <= sEntity.asDataTableWidths.Length - 1; i++)
			{
				oSBuilder.Append(sEntity.asDataTableWidths[i] + "|");
			}
			oSBuilder.Append("-||-");
	
	
			//data:
			rsEntities.MoveFirst();
			while (! rsEntities.EOF)
			{
		
				//keyfield:
				oSBuilder.Append(rsEntities.oFields[sEntity.sKeyFieldName].sValue + "|");
		
				//datafields:
				for (int i = 0; i <= sEntity.asDataTableColumns.Length - 1; i++)
				{
			
					oSBuilder.Append(rsEntities.oFields[sEntity.asDataTableColumns[i]].sValue + "|");
			
				}
				oSBuilder.Append("-||-");
		
				rsEntities.MoveNext();
			};
	
			return oSBuilder.ToString();
	
		}

		//================================================================================
		//Function:  sRsToDataTableString
		//--------------------------------------------------------------------------------'
		//Purpose:   transforms the rs, so that it can be shown in the
		//           data-table
		//--------------------------------------------------------------------------------'
		//Params:    asCaptions          - the column header
		//           rs                  - the data-recordset
		//           sKeyField           - the name of the key-field
		//           lTotalRecordcount   - for getting the page-size
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 13:09:59
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsRsToDataTableString(string[] asCaptions, string[] asWidths, Recordset rs, string sKeyField, int lTotalRecordcount)
		{
	
			StringBuilder oSBuilder = new StringBuilder();
	
			oSBuilder.Append("OK-||-");
			oSBuilder.Append(easyFramework.Sys.ToolLib.DataConversion.gsCStr(lTotalRecordcount) + "|");
			oSBuilder.Append("-||-");
	
	
			//Captions:
			for (int i = 0; i <= asCaptions.Length - 1; i++)
			{
				oSBuilder.Append(asCaptions[i] + "|");
			}
			oSBuilder.Append("-||-");
	
			//ColWidths:
			for (int i = 0; i <= asWidths.Length - 1; i++)
			{
				oSBuilder.Append(asWidths[i] + "|");
			}
			oSBuilder.Append("-||-");
	
	
			//data:
			rs.MoveFirst();
			while (! rs.EOF)
			{
		
				//keyfield:
				oSBuilder.Append(rs.oFields[sKeyField].sValue + "|");
		
				//datafields:
				for (int i = 0; i <= rs.oFields.Count - 1; i++)
				{
					if (Strings.LCase(rs.oFields[i].sName) != Strings.LCase(sKeyField))
					{
						oSBuilder.Append(rs.oFields[i].sValue + "|");
					}
			
				}
				oSBuilder.Append("-||-");
		
				rs.MoveNext();
			} ;
	
			return oSBuilder.ToString();
	
		}

		//================================================================================
		//Function:  sWebToAbsoluteFilename
		//--------------------------------------------------------------------------------'
		//Purpose:   makes an absolute filename from a relative-filename
		//--------------------------------------------------------------------------------'
		//Params:    the current request-object which contains the pathinfo to the
		//           actual path
		//           the relative-filename; can also be virtual (starting with a "/"-character)
		//           bForWeb - True: web-path; False: file-system-path
		//--------------------------------------------------------------------------------'
		//Returns:   the absolutefilename like 'e:\inetpub\wwwroot\project\file1.xml'
		//           (where the input was just "../../file1.xml")
		//--------------------------------------------------------------------------------'
		//Created:   09.04.2004 01:36:34
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string sWebToAbsoluteFilename(System.Web.HttpRequest oRequest, string sRelativeFilename, bool bForWeb)
		{
	
			if (Strings.Left(sRelativeFilename, 1) == "/" & oRequest != null)
			{
				string sAppPath = oRequest.ApplicationPath;
				if (Strings.Right(sAppPath, 1) == "/")
				{
					sAppPath = Strings.Left(sAppPath, Strings.Len(sAppPath) - 1);
				}
				if (Strings.LCase(Strings.Left(sRelativeFilename, Strings.Len(sAppPath))) != Strings.LCase(sAppPath))
				{
					sRelativeFilename = sAppPath + sRelativeFilename;
				}
			}
			else if (oRequest != null)
			{
				string sPath = oRequest.Path;
				sPath = easyFramework.Sys.ToolLib.Functions.Reverse(Strings.Mid(easyFramework.Sys.ToolLib.Functions.Reverse(sPath), Strings.InStr(easyFramework.Sys.ToolLib.Functions.Reverse(sPath), "/", 0) + 1));
				while (Strings.Left(sRelativeFilename, 3) == "../")
				{
					sPath = easyFramework.Sys.ToolLib.Functions.Reverse(Strings.Right(easyFramework.Sys.ToolLib.Functions.Reverse(sPath), Strings.Len(sPath) - Strings.InStr(easyFramework.Sys.ToolLib.Functions.Reverse(sPath), "/", 0)));
					sRelativeFilename = Strings.Right(sRelativeFilename, Strings.Len(sRelativeFilename) - Strings.Len("../"));
				};
				sRelativeFilename = sPath + "/" + sRelativeFilename;
		
			}
	
			if (Strings.Left(sRelativeFilename, 2) == "//")
			{
				sRelativeFilename = Strings.Right(sRelativeFilename, Strings.Len(sRelativeFilename) - 1);
			}
	
	
			if (bForWeb | oRequest == null)
			{
				return sRelativeFilename;
			}
			else
			{
				return oRequest.MapPath(sRelativeFilename);
			}
	
	
		}



		//================================================================================
		//Function:  sJavaScriptBool
		//--------------------------------------------------------------------------------'
		//Purpose:   converts to a bool in javascript
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   04.05.2004 13:18:41
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string sJavaScriptBool(bool b)
		{
			if (b)
			{
				return "true";
			}
			else
			{
				return "false";
			}
		}

	}

}
