using System;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Data;
using System.Data;
using System.Collections;

namespace easyFramework.Sys
{
	//================================================================================
	// Project:      easyFramework
	// Copyright:    Promain Software-Betreuung GmbH
	// Component:    DataMethods.vb
	//--------------------------------------------------------------------------------
	// Purpose:      contains the clientinfo-object, which holds informations
	//               of the current logged-in user
	//--------------------------------------------------------------------------------
	// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
	//--------------------------------------------------------------------------------
	// Changed:
	//================================================================================
	
	
	public class ClientInfo
	{
		
		//================================================================================
		//private fields:
		//================================================================================
		private string msClientID;
		private string msConnstr;
		private efArrayList moErrorList; //of efExceptions
		private efTransaction moCurrentTransaction;
		private string msLanguage;
		private string msDateFormat = "dd.MM.yyyy"; //use default; store someday in db
		private VolatileFields moVolatileFields;
		
		
		private Webobjects.HttpApp moHttpApp;
		private Webobjects.HttpRequest moHttpRequest;
		private Webobjects.HttpServer moHttpServer;
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		
		//================================================================================
		//Property:  oHttpApp
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 10:27:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Webobjects.HttpApp oHttpApp
		{
			get
			{
				return moHttpApp;
			}
			set
			{
				moHttpApp = value;
			}
		}
		
		
		//================================================================================
		//Property:  sLanguage
		//--------------------------------------------------------------------------------'
		//Purpose:   the language of the current client-connection
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 11:58:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sLanguage
		{
			get
			{
				return msLanguage;
			}
			set
			{
				msLanguage = value;
			}
		}

		/// <summary>
		/// the dateformat can be used for a correct formatting of date for the user in
		/// his language-format
		/// </summary>
		public string sDateFormat
		{
			get
			{
				return msDateFormat;
			}
			set 
			{
				msLanguage = value;
			}
		}
		
		//================================================================================
		//Property:  oHttpRequest
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 10:27:53
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Webobjects.HttpRequest oHttpRequest
		{
			get
			{
				return moHttpRequest;
			}
			set
			{
				moHttpRequest = value;
			}
		}
		
		//================================================================================
		//Property:  oHttpServer
		//--------------------------------------------------------------------------------'
		//Purpose:
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   04.06.2004 10:27:50
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Webobjects.HttpServer oHttpServer
		{
			get
			{
				return moHttpServer;
			}
			set
			{
				moHttpServer = value;
			}
		}
		
		//================================================================================
		//Property:  bHasErrors
		//--------------------------------------------------------------------------------'
		//Purpose:   returns true, if there are errors
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 00:51:08
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bHasErrors
		{
			get
			{
				if (moErrorList == null)
				{
					return false;
				}
				else
				{
					if (moErrorList.Count > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}
		
		//================================================================================
		//Property:  oCurrentTransaction
		//--------------------------------------------------------------------------------'
		//Purpose:   sets/gets the current-transaction
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:35:47
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efTransaction oCurrentTransaction
		{
			get
			{
				return moCurrentTransaction;
			}
			set
			{
				moCurrentTransaction = value;
			}
		}
		
		
		//================================================================================
		//Property:  bHasTransaction
		//--------------------------------------------------------------------------------'
		//Purpose:   returns true, false depending, if there is a transaction-object or not
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:36:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bHasTransaction
		{
			get
			{
				if (moCurrentTransaction == null == false)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		
		
		//================================================================================
		//public methods
		//================================================================================
		public ClientInfo(string sClientID, string sConnstr) 
		{
			moErrorList = new efArrayList();
			msLanguage = "de-DE";
			moVolatileFields = new VolatileFields();
			msConnstr = sConnstr;
			msClientID = sClientID;
		}
		
		
		
		//================================================================================
		//Sub:       BeginTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   begins a database-transaction
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:39:01
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void BeginTrans (efTransaction.efEnumIsolationLevels iso)
		{
			if (moCurrentTransaction == null == false)
			{
				throw (new efDataException("Already has transaction - cannot start another transaction"));
			}
			moCurrentTransaction = new efTransaction(this.sConnStr);
			moCurrentTransaction.BeginTrans(iso);
		}
		
		
		//================================================================================
		//Sub:       RollbackTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   rollbacks a transaction
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:39:57
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void RollbackTrans ()
		{
			if (moCurrentTransaction == null)
			{
				throw (new efDataException("There is no transaction to rollback."));
			}
			
			moCurrentTransaction.RollbackTrans();
			
			moCurrentTransaction = null;
			
		}
		
		
		//================================================================================
		//Sub:       RollBackTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   rollbacks a transaction to a certain save-point
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:40:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void RollbackTrans (string sSavePointName)
		{
			if (moCurrentTransaction == null)
			{
				throw (new efDataException("There is no transaction to rollback."));
			}
			
			moCurrentTransaction.RollbackTrans(sSavePointName);
			
		}
		
		
		//================================================================================
		//Sub:       CommitTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   commits a transaction
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:40:57
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void CommitTrans ()
		{
			if (moCurrentTransaction == null)
			{
				throw (new efDataException("There is no transaction to rollback."));
			}
			
			moCurrentTransaction.CommitTrans();
			moCurrentTransaction = null;
			
		}
		
		
		//================================================================================
		//Sub:       SaveTrans
		//--------------------------------------------------------------------------------'
		//Purpose:   stores a transaction's save-point
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   16.05.2004 13:41:15
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void SaveTrans (string sSavePointName)
		{
			moCurrentTransaction.Save(sSavePointName);
		}
		
		
		//================================================================================
		//Sub:       gAddError
		//--------------------------------------------------------------------------------'
		//Purpose:   add error to the error-list
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 00:49:19
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gAddError (string sMsg)
		{
			efException oE = new efException(sMsg);
			moErrorList.Add(oE);
			
		}
		
		
		//================================================================================
		//Function:  gsErrors
		//--------------------------------------------------------------------------------'
		//Purpose:   returns all errors as string
		//--------------------------------------------------------------------------------'
		//Params:    lMaxErrors - the maximum number of errors
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 01:01:59
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsErrors()
		{
			return gsErrors(10);
		}
		public string gsErrors(int lMaxErrors)
		{
			if (bHasErrors == false)
			{
				return "";
			}
			else
			{
				FastString sResult = new FastString();
				for (int I = 0; I <= moErrorList.Count - 1; I++)
				{
					if (I > lMaxErrors)
					{
						break;
					}
					
					sResult.Append(((efException)(moErrorList[I])).sMsg + "\r\n");
					
				}
				return sResult.ToString();
				
			}
		}
		
		
		//================================================================================
		//Function:  gGetOldestValue
		//--------------------------------------------------------------------------------'
		//Purpose:   pops the latest error (not a stack-pop, fifo)
		//--------------------------------------------------------------------------------'
		//Params:    if true, then the error is removed
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   21.04.2004 00:52:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		//Public Function gGetOldestValue(Optional ByVal bRemoveAfterCall As Boolean = True) As efException
		
		//    If Not bHasErrors Then Return Nothing
		
		//    Dim oResult As efException =     '        CType(moErrorList(0), efException)
		//    moErrorList.RemoveAt(0)
		
		
		//End Function
		
		//================================================================================
		// Method:       goNewClientInfo
		//--------------------------------------------------------------------------------
		// Parameters:   connstr to database
		//               the username, to log on
		//               the password of the user
		//--------------------------------------------------------------------------------
		// Returns:      a new created clientinfo-object
		//--------------------------------------------------------------------------------
		// Created:      21.03.04 M.Wimmer (mwimmer@promain-software.de)
		//--------------------------------------------------------------------------------
		// Changed:
		//================================================================================
		public static ClientInfo goGetNewClientInfo(string connstr, string sUsername, string sPassword)
		{
			
			System.Data.SqlClient.SqlConnection oConn = new System.Data.SqlClient.SqlConnection(connstr);
			oConn.Open();
			
			try
			{
				
				
				//-------check usernamme/password---------------
				
				System.Data.SqlClient.SqlCommand oCmdUser = new System.Data.SqlClient.SqlCommand("SELECT * FROM tsUsers " + " WHERE usr_login='" + DataTools.SQLString(sUsername) + "'", oConn);
				System.Data.SqlClient.SqlDataReader oReader = oCmdUser.ExecuteReader();
				bool bHasUser = false;
				string sPassword_in_DB = "";
				int lUsr_ID=-1;
				if (oReader.Read())
				{
					bHasUser = true;
					sPassword_in_DB = oReader.GetString(oReader.GetOrdinal("usr_password"));
					lUsr_ID = oReader.GetInt32(oReader.GetOrdinal("usr_id"));
				}
				oReader.Close();
				
				if (! bHasUser)
				{
					throw (new efException("Incorrect login or password \"" + sUsername + "\". Please try again!"));
				}
				else
				{
					if (Functions.LCase(sPassword_in_DB) != Functions.LCase(sPassword))
					{
						throw (new efException("Incorrect login or password \"" + sUsername + "\". Please try again!"));
					}
				}
				
				
				
				//------create client-info
				
				Functions.Randomize();
				Random r = new Random();
				r.Next(0, 10000000);
				
				string sClientID; //creates a new session-id
				sClientID = easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Year(DateTime.Now)) + easyFramework.Sys.ToolLib.Functions.gs2Digit(easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Month(Functions.Now()))) + easyFramework.Sys.ToolLib.Functions.gs2Digit(easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Day(Functions.Now()))) + easyFramework.Sys.ToolLib.Functions.gs2Digit(easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Hour(Functions.Now()))) + easyFramework.Sys.ToolLib.Functions.gs2Digit(easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Minute(Functions.Now()))) + easyFramework.Sys.ToolLib.Functions.gs2Digit(easyFramework.Sys.ToolLib.DataConversion.gsCStr(Functions.Second(Functions.Now()))) + easyFramework.Sys.ToolLib.DataConversion.gsCStr(r.Next());
				
				
				
				
				ClientInfo oResult = new ClientInfo(sClientID, connstr);
				
				string sQry = "INSERT INTO tsClientInfo(ci_id, ci_inserted, ci_loggedin, ci_loggedin_usr) " + "VALUES('$1', getdate(), $2, $3)";
				sQry = Functions.Replace(sQry, "$1", sClientID);
				sQry = Functions.Replace(sQry, "$2", easyFramework.Sys.ToolLib.DataConversion.gsCStr(easyFramework.Sys.ToolLib.DataConversion.glCInt(bHasUser, 0)));
				sQry = Functions.Replace(sQry, "$3", easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_ID));
				
				System.Data.SqlClient.SqlCommand oCmd = new System.Data.SqlClient.SqlCommand(sQry, oConn);
				oCmd.ExecuteNonQuery();
				
				
				
				return oResult;
				
			}
			finally
			{
				oConn.Close();
				
			}
			
			
		}
		//================================================================================
		//public properties
		//================================================================================
		
		
		//================================================================================
		//Property:  oField
		//--------------------------------------------------------------------------------'
		//Purpose:   retrieves the data, which is stored in the database tsClientInfo.
		//           field ci_content
		//--------------------------------------------------------------------------------'
		//Params:    the name of the property
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 23:53:50
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string this[string sName]
		{
			get
			{
				string sQry = "SELECT ci_content FROM tsClientInfo WHERE ci_id='" + msClientID + "'";
				
				Recordset rs;
				if (oCurrentTransaction == null)
				{
					rs = DataMethods.gRsGetDirect(sConnStr, sQry);
				}
				else
				{
					rs = DataMethods.gRsGetDirect(oCurrentTransaction, sQry);
				}
				
				if (rs.EOF)
				{
					throw (new Exception("Invalid Client-ID: \"" + msClientID + "\"!"));
				}
				
				if (! rs["ci_content"].bIsNull())
				{
					
					XmlDocument oXml = new XmlDocument(rs["ci_content"].sValue);
					
					XmlNode oXmlNode = oXml.selectSingleNode("//" + sName);
					
					if (oXmlNode == null)
					{
						return "";
					}
					else
					{
						return oXmlNode.sText;
					}
				}
				else return "";
			}
			set
			{
				string sQry = "SELECT ci_content FROM tsClientInfo WHERE ci_id='" + msClientID + "'";
				
				Recordset rs;
				if (oCurrentTransaction == null)
				{
					rs = DataMethods.gRsGetDirect(sConnStr, sQry);
				}
				else
				{
					rs = DataMethods.gRsGetDirect(oCurrentTransaction, sQry);
				}
				
				XmlDocument oXml;
				if (! rs["ci_content"].bIsNull())
				{
					
					oXml = new XmlDocument(rs["ci_content"].sValue);
				}
				else
				{
					oXml = new XmlDocument("<clientInfoFields/>");
				}
				
				XmlNode oXmlNode = oXml.selectSingleNode("//" + sName);
				
				if (oXmlNode == null)
				{
					oXmlNode = oXml.selectSingleNode("/clientInfoFields").AddNode(sName, true);
					
				}
				
				oXmlNode.sText = value;
				
				
				//---------------store the memo again------------
				if (oCurrentTransaction == null)
				{
					DataMethods.gUpdateMemo(sConnStr, "tsClientInfo", "ci_id", this.sClientID, "ci_content", oXml.sXml);
				}
				else
				{
					Data.DataMethods.gUpdateMemo(oCurrentTransaction, "tsClientInfo", "ci_id", this.sClientID, "ci_content", oXml.sXml);
				}
				
			}
			
		}
		
		
		//================================================================================
		//Property:  oVolatileField
		//--------------------------------------------------------------------------------'
		//Purpose:   stores data not in database; it is volatile data and exists
		//           as long the clientinfo-object exists
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   22.05.2004 20:40:29
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public VolatileFields oVolatileField 
		{
			get 
			{
				return moVolatileFields;
			}  

		}	

		//================================================================================
		//Property:  sClientID
		//--------------------------------------------------------------------------------'
		//Purpose:   returns Client-ID
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 22:41:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sClientID
		{
			get
			{
				return msClientID;
			}
		}
		
		//================================================================================
		//Property:  bLoggedIn
		//--------------------------------------------------------------------------------'
		//Purpose:   true, when a user from tsUsers is logged in;
		//           use sub "logon" to logon a user
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 22:41:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bLoggedIn
		{
			get
			{
				
				if (oCurrentTransaction == null)
				{
					return DataMethods.gbGetDBValue(sConnStr, "tsClientInfo", "ci_loggedin", "ci_id='" + this.sClientID + "'");
				}
				else
				{
					return DataMethods.gbGetDBValue(oCurrentTransaction, "tsClientInfo", "ci_loggedin", "ci_id='" + this.sClientID + "'");
				}
				
				
			}
		}
		
		//================================================================================
		//Property:  rsLoggedInUser
		//--------------------------------------------------------------------------------'
		//Purpose:   nothing, if no user is logged in;
		//           otherwise data from table tsUsers is returned
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 22:41:03
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Recordset rsLoggedInUser
		{
			get
			{
				if (bLoggedIn == false)
				{
					return null;
				}
				else
				{
					
					string sQry = "SELECT ci_loggedin_usr FROM tsClientInfo WHERE ci_id=$1";
					
					sQry = Functions.Replace(sQry, "$1", "'" + Functions.Replace(this.sClientID, "'", "''") + "'");
					int lUsr_ID;
					if (oCurrentTransaction == null)
					{
						lUsr_ID = DataMethods.glDBValue(sConnStr, sQry, 0, 0);
					}
					else
					{
						lUsr_ID = DataMethods.glDBValue(oCurrentTransaction, sQry, 0, 0);
					}
					
					
					if (oCurrentTransaction == null)
					{
						return DataMethods.gRsGetTable(sConnStr, "tsUsers", "*", "usr_id=" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_ID), "", "", "");
					}
					else
					{
						return DataMethods.gRsGetTable(oCurrentTransaction, "tsUsers", "*", "usr_id=" + easyFramework.Sys.ToolLib.DataConversion.gsCStr(lUsr_ID), "", "", "");
					}
					
				}
				
			}
		}
		
		
		//================================================================================
		//Property:  sConnStr
		//--------------------------------------------------------------------------------'
		//Purpose:   Connection-string
		//--------------------------------------------------------------------------------'
		//Created:   21.03.2004 22:40:38
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sConnStr
		{
			get
			{
				return msConnstr;
			}
		}
		
		
		//================================================================================
		//Function:  gsGetUsername
		//--------------------------------------------------------------------------------'
		//Purpose:   returns the user-name, e.g. for storing in the last-update-columns
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   13.04.2004 00:16:34
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string gsGetUsername()
		{
			if (rsLoggedInUser == null)
			{
				return "aN/A";
			}
			else
			{
				string sResult;
				
				sResult = rsLoggedInUser["usr_firstname"].sValue;
				sResult += Functions.Trim(" " + rsLoggedInUser["usr_lastname"].sValue);
				if (!Functions.IsEmptyString(rsLoggedInUser["usr_login"].sValue))
				{
					sResult += " (" + rsLoggedInUser["usr_login"].sValue + ")";
				}
				
				return sResult;
				
			}
			
		}

		/// <summary>
		/// returns a correctly formatted date in the users language
		/// </summary>
		/// <param name="dtValue"></param>
		/// <returns></returns>
		public string gsFormatDate(DateTime dtValue) 
		{
			return Functions.Format(dtValue, sDateFormat);
		}
		
		//================================================================================
		//private methods
		//================================================================================
		
		~ClientInfo()
		{
			
			
			//----------rollback transaction, if there is any, if not cleared
			try
			{
				if (moCurrentTransaction == null == false)
				{
					moCurrentTransaction.RollbackTrans();
				}
			}
			catch
			{
			}
			
		}
	}
	
	
}
