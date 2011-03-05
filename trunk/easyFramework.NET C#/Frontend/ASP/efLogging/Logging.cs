using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     Logging
	//--------------------------------------------------------------------------------'
	//Module:    Logging.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   for error-logging in File; informing admin via Mail etc.
	//           it is a state-class
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 17:12:24
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class Logging
	{
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private string[] masMailRecipients;
		private string msConnStr;
		private string msMailServer;
		private string msMailSender; //email of the sender
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		//================================================================================
		//Public Consts:
		//================================================================================
		public enum efEnumLogTypes
		{
			efError = 1,
			efWarning = 15,
			efInformation = 30,
			efDebug = 50
		}
		
		
		//================================================================================
		//Public Properties:
		//================================================================================
		
		//================================================================================
		//Property:  Mailserver
		//--------------------------------------------------------------------------------'
		//Purpose:   set the mailservername or ip-address here
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:49:19
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string Mailserver
		{
			get{
				return msMailServer;
			}
			set
			{
				msMailServer = value;
			}
		}
		
		
		//================================================================================
		//Property:  Mailsender
		//--------------------------------------------------------------------------------'
		//Purpose:   set up the sender, for example "error@easyFramework.de"
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:49:21
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string Mailsender
		{
			get{
				return msMailSender;
			}
			set
			{
				msMailSender = value;
			}
		}
		//================================================================================
		//Public Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       New
		//--------------------------------------------------------------------------------'
		//Purpose:   Inits the component
		//--------------------------------------------------------------------------------'
		//Params:    Database-connectionstring; log-object logs into database tables
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:26:15
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Logging(string sConnStrDB) {
			masMailRecipients = null;
			
			msConnStr = sConnStrDB;
		}
		
		
		//================================================================================
		//Sub:       gLog
		//--------------------------------------------------------------------------------'
		//Purpose:   Does Logentry
		//--------------------------------------------------------------------------------'
		//Params:    enLogtype - Warning;Info;
		//           sMessage - Messagestring
		//           optional oClientInfo; can be nothing; not an optional Parameter, so that
		//                                 you don`t forget to mention it explicitly not to
		//                                 be
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:28:49
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gLog (efEnumLogTypes enLogType, string sMessage, ClientInfo oClientInfo)
		{
			
			
			
			if (oClientInfo != null)
			{
				mWriteDatabaseLog(enLogType, sMessage, oClientInfo.sClientID, oClientInfo["Username"]);
			}
			else
			{
				mWriteDatabaseLog(enLogType, sMessage, "N/A", "N/A");
			}
			
			
			//try to send mail, if error was logged:
			if (enLogType == efEnumLogTypes.efError & gbIsMailServerConfigured() & masMailRecipients != null)
			{
				
				string sSubject = "Fehler aufgetreten easyFramework " + Functions.Now();
				string sBody = "Dies ist eine automatisch erstellte Mail" + "\n" + sMessage;
				
				try
				{
					easyFramework.Frontend.ASP.ASPTools.Mail.gSendMail(msMailServer, msMailSender, masMailRecipients, sSubject, sBody);
					
				}
				catch (efException ex)
				{
					
					mWriteDatabaseLog(efEnumLogTypes.efError, "eMail versand fehlgeschlagen: " + ex.Message, "N/A", "N/A");
					
				}
				
				
			}
			
			
			
			
		}
		
		
		//================================================================================
		//Function:  gGetLogEntries
		//--------------------------------------------------------------------------------'
		//Purpose:   retrieves all the log-entries from the database
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//--------------------------------------------------------------------------------'
		//Returns:   recordset
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 23:00:40
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public Recordset gGetLogEntries(ClientInfo oClientInfo, int lTop)
		{
			
			return DataMethodsClientInfo.gRsGetDirect(oClientInfo, "SELECT TOP " + DataConversion.gsCStr(lTop) + " * " + "FROM tsLog ORDER BY log_id DESC");
			
		}
		
		
		//================================================================================
		//Sub:       gAddMailRecipient
		//--------------------------------------------------------------------------------'
		//Purpose:   adds an email, which will be informed, when an error happens;
		//           the configuration of mailserver is needed before
		//--------------------------------------------------------------------------------'
		//Params:    sEMail - the mail address
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:44:52
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public void gAddMailRecipient (string sEMail)
		{
			if (! gbIsMailServerConfigured())
			{
				throw (new efException("please configure mailserver and mailsender before"));
			}
			
			if (masMailRecipients == null)
			{
				masMailRecipients = new string[1];
			}
			else
			{
				String[] newArray = new String[masMailRecipients.Length + 1];
				masMailRecipients.CopyTo(newArray, 0);
				masMailRecipients = newArray;

			}
			
			masMailRecipients[masMailRecipients.Length - 1] = sEMail;
			
		}
		
		
		//================================================================================
		//Function:  gsLogType2Str
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the logtype to string
		//--------------------------------------------------------------------------------'
		//Params:    the logtype(info, warning, debug, error)
		//--------------------------------------------------------------------------------'
		//Returns:   the string
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:38:32
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static string gsLogType2Str(efEnumLogTypes enLogType)
		{
			
			switch (enLogType)
			{
				case efEnumLogTypes.efDebug:
					
					return "Debug";
				case efEnumLogTypes.efError:
					
					return "Error";
				case efEnumLogTypes.efInformation:
					
					return "Info";
				case efEnumLogTypes.efWarning:
					
					return "Warning";
				default:
					
					throw (new efException("Unknown logtype: " + enLogType));
			}
			
		}
		
		
		//================================================================================
		//Function:  gbIsMailServerConfigured
		//--------------------------------------------------------------------------------'
		//Purpose:   checks, wether everything is ok to send mails
		//--------------------------------------------------------------------------------'
		//Params:      -
		//--------------------------------------------------------------------------------'
		//Returns:   bool
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 17:47:22
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool gbIsMailServerConfigured()
		{
			if (!Functions.IsEmptyString(msMailSender) & !Functions.IsEmptyString(msMailServer))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		//================================================================================
		//Private Methods:
		//================================================================================
		private void mWriteDatabaseLog (efEnumLogTypes enLogType, string sMessage, string sClientID, string sUserName)
		{
			
			System.Data.SqlClient.SqlConnection c = new System.Data.SqlClient.SqlConnection(msConnStr);
			System.Data.SqlClient.SqlCommand sCmd = new System.Data.SqlClient.SqlCommand("INSERT INTO tsLog( " + "log_message, log_date, log_type, log_clientid, log_username) " + "VALUES(@log_message, @log_date, @log_type, @log_clientid, @log_username)", c);
			
			
			sCmd.Parameters.Add("@log_message", System.Data.SqlDbType.NVarChar, 3000);
			sCmd.Parameters["@log_message"].Value = easyFramework.Sys.ToolLib.DataConversion.gsCStr(sMessage);
			
			sCmd.Parameters.Add("@log_date", System.Data.SqlDbType.DateTime, 3000);
			sCmd.Parameters["@log_date"].Value = Functions.Now();
			
			sCmd.Parameters.Add("@log_type", System.Data.SqlDbType.NVarChar, 25);
			sCmd.Parameters["@log_type"].Value = gsLogType2Str(enLogType);
			
			sCmd.Parameters.Add("@log_clientid", System.Data.SqlDbType.NVarChar, 50);
			sCmd.Parameters["@log_clientid"].Value = easyFramework.Sys.ToolLib.DataConversion.gsCStr(sClientID);
			
			sCmd.Parameters.Add("@log_username", System.Data.SqlDbType.NVarChar, 255);
			sCmd.Parameters["@log_username"].Value = easyFramework.Sys.ToolLib.DataConversion.gsCStr(sUserName);
			
			c.Open();
			sCmd.Prepare();
			sCmd.ExecuteNonQuery();
			
			c.Close();
			
		}
		
	}
	
}
