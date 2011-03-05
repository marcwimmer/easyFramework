using System;
using System.Web.Mail;
using easyFramework.Sys;
using easyFramework.Sys.ToolLib;

namespace easyFramework.Frontend.ASP.ASPTools
{
	//================================================================================
	//Class:     Mail

	//--------------------------------------------------------------------------------'
	//Module:    Mail.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   sending mails
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 17:57:13
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	
	public class Mail
	{
		
		
		//================================================================================
		//Sub:       gSendMail
		//--------------------------------------------------------------------------------'
		//Purpose:   sends mail
		//--------------------------------------------------------------------------------'
		//Params:    ByVal sMailserver As String,     '           byval sFrom as string,     '           ByVal sRecipients() As String,     '           ByVal sSubject As String,     '           ByVal sBody As String
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 18:00:34
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static void gSendMail (string sMailserver, string sFrom, string[] sRecipients, string sSubject, string sBody)
		{
			
			MailMessage oMailMessage = new MailMessage();
			
			string sRecipientsWithSemicolon = "";
			for (int i = 0; i <= sRecipients.Length - 1; i++)
			{
				sRecipientsWithSemicolon += sRecipients[i] + ";";
			}
			sRecipientsWithSemicolon = Functions.Left(sRecipientsWithSemicolon, Functions.Len(sRecipientsWithSemicolon) - 1);
			
			oMailMessage.To = sRecipientsWithSemicolon;
			oMailMessage.From = sFrom;
			oMailMessage.Subject = sSubject;
			oMailMessage.Body = sBody;
			
			SmtpMail.SmtpServer = sMailserver;
			SmtpMail.Send(oMailMessage);
			
			
		}
		
		//================================================================================
		//Sub:       gSendMail
		//--------------------------------------------------------------------------------'
		//Purpose:   sends mail
		//--------------------------------------------------------------------------------'
		//Params:    ByVal sMailserver As String,     '           ByVal sFrom As String,     '           ByVal sRecipients As String,     '           ByVal sSubject As String,     '           ByVal sBody As String
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 18:00:34
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public static void gSendMail (string sMailserver, string sFrom, string sRecipients, string sSubject, string sBody)
		{
			
			MailMessage oMailMessage = new MailMessage();
			
			sRecipients = Functions.Left(sRecipients, Functions.Len(sRecipients) - 1);
			
			oMailMessage.To = sRecipients;
			oMailMessage.From = sFrom;
			oMailMessage.Subject = sSubject;
			oMailMessage.Body = sBody;
			
			SmtpMail.SmtpServer = sMailserver;
			SmtpMail.Send(oMailMessage);
			
			
		}
		
	}
	
}
