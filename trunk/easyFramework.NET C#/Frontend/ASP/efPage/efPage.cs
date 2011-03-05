using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Xml;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;
using System.Web;

namespace easyFramework.Frontend.ASP.Dialog
{
	//================================================================================
	//Class:     efDialogPage
	//--------------------------------------------------------------------------------'
	//Module:    efPage.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   the base asp-page from which is inherited
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 01:07:15
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'

	public class efPage : System.Web.UI.Page
	{
		
		
		
		//================================================================================
		//Private Fields:
		//================================================================================
		private ClientInfo moClientInfo;
		private XmlDocument moXmlRequest = null; //cached after fetched, for next executions;
		private string msTitle;


		
		
		//================================================================================
		//Public Properties:
		//================================================================================
		//================================================================================
		//Property:  sTitle
		//--------------------------------------------------------------------------------'
		//Purpose:   stores the document-title
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 10:02:49
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sTitle
		{
			get
			{
				string retValue = msTitle;

				//----Server hinten dran hängen-------
				if (moClientInfo != null)
					retValue += " - " + moClientInfo.oHttpRequest.oHttpRequest.ServerVariables.Get("SERVER_NAME");

				return retValue;
			}
			set
			{
				msTitle = value;
			}
		}

		//================================================================================
		//Property:  oClientInfo
		//--------------------------------------------------------------------------------'
		//Purpose:   to achieve the client-info object
		//--------------------------------------------------------------------------------'
		//Params:    -
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 01:17:31
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public ClientInfo oClientInfo
		{
			get
			{
				return moClientInfo;
			}
		}
		
		
		//================================================================================
		//Public Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       New
		//--------------------------------------------------------------------------------'
		//Purpose:   constructor
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo
		//--------------------------------------------------------------------------------'
		//Created:   22.03.2004 01:16:48
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efPage() 
		{
						

		}
		
		
		
		
		
		//================================================================================
		//Protected Methods:
		//================================================================================
		
		//================================================================================
		//Sub:       LoadClientInfo
		//--------------------------------------------------------------------------------'
		//Purpose:   loads the clientinfo object, by getting the clientid from url;
		//           the login-dialog overrides this sub, so that no errors occur,
		//           because ther isn't any clientinfo at login-time
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   02.04.2004 01:42:18
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected virtual void LoadClientInfo ()
		{
			if (!Functions.IsEmptyString(Request["ClientID"]))
			{
				moClientInfo = new ClientInfo(Request["ClientID"].ToString(), 
					efEnvironment.goGetEnvironment(Application).gsConnStr);
				moClientInfo.oHttpRequest = new Sys.Webobjects.HttpRequest(Request);
				moClientInfo.oHttpServer = new Sys.Webobjects.HttpServer(Server);
				moClientInfo.oHttpApp = new Sys.Webobjects.HttpApp(Application);
			}
			else
			{
				throw (new efException("URL-Param \"clientID\" is missing\""));
				
			}
			
		}
		
		//================================================================================
		//Function:  moRequestTosXml
		//--------------------------------------------------------------------------------'
		//Purpose:   converts the request-parameter to xml-document
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Returns:
		//--------------------------------------------------------------------------------'
		//Created:   06.04.2004 09:33:03
		//--------------------------------------------------------------------------------'
		//Changed:   16.04.2004 - using UNICODE-conversion, Marc
		//--------------------------------------------------------------------------------'
		protected XmlDocument moRequestTosXml(HttpRequest oRequest)
		{
			
			if (moXmlRequest != null) 
				return moXmlRequest;
			else 
			{

				XmlDocument oResult = new XmlDocument("<request/>");
			
			
			
				foreach (string sKey in oRequest.QueryString)
				{
					if (!Functions.IsEmptyString(sKey))
					{
						oResult.selectSingleNode("/request").AddNode(sKey, false);
						oResult.selectSingleNode("/request/" + sKey).sText = oRequest[sKey].ToString();
					}
				}
			
				//often there is something given in the inputstream,especially then,
				//when the javascript gServerProcess method is called with arguments:
			
				if (Request.InputStream.CanRead)
				{
				
					System.IO.Stream str;
				
					int strLen;
					int strRead;
				
					System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding(false);
				
				
					// Create a Stream object.
					str = Request.InputStream;
					// Find number of bytes in stream.
					strLen = System.Convert.ToInt32(str.Length);
					// Create a byte array.
					byte[] strArr = new byte[strLen + 1];
					// Read stream into byte array.
					strRead = str.Read(strArr, 0, strLen);
				
					// Convert byte array to a text string:
					FastString oSBuilder = new FastString();
					oSBuilder.Append(enc.GetString(strArr, 0, strLen));
				
				
					//'For counter = 0 To strLen - 1
					//'    oSBuilder.Append(chr(strArr(counter).ToString()))
					//'Next counter
				
					//try, if xml-document:
					if (oSBuilder.lLength > 0)
					{
						oResult.selectSingleNode("/request").appendDocumentFragment(oSBuilder.ToString());
					}
				}
			
				//store it:
				moXmlRequest = oResult;

				//return it:
				return oResult;

			}
			
		}
		
		//================================================================================
		//Sub:       OnInit
		//--------------------------------------------------------------------------------'
		//Purpose:   load the clientinfo-object
		//           each dialog-page must have a valid client-info object;
		//           the clientinfo-object is got by the url-parameter "clientid", which
		//           must be always provided
		//--------------------------------------------------------------------------------'
		//Params:    system-arguments
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 23:11:14
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void OnInit (System.EventArgs e)
		{
			
			//------needed for everything---------
			LoadClientInfo();
			
			//-----------call the load-handler of the page--------------------
			CustomInit (moRequestTosXml(Request));
			
			
			
			base.OnInit(e);

		}
		
		/// <summary>
		///================================================================================
		///Sub:       CustomInit
		///--------------------------------------------------------------------------------'
		///Purpose:   default-page settings for response-object
		///			 OVERWRITE this function - call the base-function always,
		///			 when overriding
		///--------------------------------------------------------------------------------'
		///Params:
		///--------------------------------------------------------------------------------'
		///Created:   06.04.2004 09:33:39
		///--------------------------------------------------------------------------------'
		///Changed:
		///--------------------------------------------------------------------------------'
		/// </summary>
		public virtual void CustomInit (XmlDocument oXmlRequest)
		{
			
			//default settings:
			Response.Expires = -1;
			Response.CacheControl = "no-cache";
			Response.AddHeader("Pragma", "no-cache");
			
		}


		//================================================================================
		//Private Methods:
		//================================================================================
		

	}
	
}
