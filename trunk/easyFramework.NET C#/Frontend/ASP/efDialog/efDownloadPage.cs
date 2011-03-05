using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.ASPTools;
using System.Web;

namespace easyFramework.Frontend.ASP.Dialog
{
	//================================================================================
	//Class:     efDownloadMIME

	//--------------------------------------------------------------------------------'
	//Module:    efDownloadMIME.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH, 2004
	//--------------------------------------------------------------------------------'
	//Purpose:   helps downloading file-contents
	//--------------------------------------------------------------------------------'
	//Created:   11.05.2004 10:08:09
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	//================================================================================
	//Imports:
	//================================================================================



	public class efDownloadPage : efPage
	{
		public efDownloadPage()
		{
			menMimeType = efEnumSupportedMIME.efBrowserStandard;
			mbTransferBinary = true;
			mbMimeTypeSet = false;
			msFileName = "";
			mbAsAttachment = true;
		}



		//================================================================================
		//Private Fields:
		//================================================================================
		private efEnumSupportedMIME menMimeType;
		private bool mbTransferBinary;
		private bool mbMimeTypeSet;
		private string msFileName;
		private bool mbAsAttachment;

		//================================================================================
		//Public Properties:
		//================================================================================

		//================================================================================
		//Property:  sFileName
		//--------------------------------------------------------------------------------'
		//Purpose:   the file-name, which is added to the response-header;
		//           so the downloader has a good filename at once
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 10:38:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public string sFileName
		{
			get
			{
				return msFileName;
			}
			set
			{
				msFileName = value;
			}
		}



		//================================================================================
		//Property:  bAsAttachment
		//--------------------------------------------------------------------------------'
		//Purpose:   if as a attachment, then the file is downloaded; if inline, the
		//           file is opened within the browser window
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 13:18:16
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bAsAttachment
		{
			get
			{
				return mbAsAttachment;
			}
			set
			{
				mbAsAttachment = value;
			}
		}


		//================================================================================
		//Property:  enMimeType
		//--------------------------------------------------------------------------------'
		//Purpose:   the mime-type of the download-file
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 10:38:51
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public efEnumSupportedMIME enMimeType
		{
			get
			{
				return menMimeType;
			}
			set
			{
				menMimeType = value;
				mbMimeTypeSet = true;
			}
		}


		//================================================================================
		//Property:  bTransferBinary
		//--------------------------------------------------------------------------------'
		//Purpose:   sets the transfer-mode; for text e.g. should be used "FALSE"
		//           for excel, word-documents should be used "TRUE"
		//--------------------------------------------------------------------------------'
		//Params:
		//--------------------------------------------------------------------------------'
		//Created:   11.05.2004 10:42:10
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public bool bTransferBinary
		{
			get
			{
				return mbTransferBinary;
			}
			set
			{
				mbTransferBinary = value;
			}
		}

		//================================================================================
		//Public Methods:
		//================================================================================


		//================================================================================
		//Function:  abGetData
		//--------------------------------------------------------------------------------'
		//Purpose:   used to retrieve the data of the process;
		//           you can do, what you want here: you can return complete
		//           recordsets-xmls, sXml, boolean etc....
		//--------------------------------------------------------------------------------'
		//Params:    clientinfo, the requeststring of GET or POST
		//--------------------------------------------------------------------------------'
		//Returns:   string-data as result
		//--------------------------------------------------------------------------------'
		//Created:   23.03.2004 01:46:02
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		public virtual byte[] abGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{

			throw (new efException("Please override the abGetData-Method!"));

		}



		protected override void Render (System.Web.UI.HtmlTextWriter writer)
		{
			try
			{
	
				writer.Flush();
	
	
				//-----------------------------get buffer---------------------------
				XmlDocument oRequest = moRequestTosXml(Request);
				byte[] abBuffer = abGetData(oClientInfo, oRequest);
				if (abBuffer == null)
				{
					throw (new efException("You should not return nothing in abGetData!"));
				}
	
	
				//-----------------------------error check---------------------------
				if (mbMimeTypeSet == false)
				{
					throw (new efException("Please give a MIME-type!"));
				}
				if (Functions.IsEmptyString(msFileName))
				{
					throw (new efException("Please give a MIME-filename!"));
				}
	
	
				//-------------------------set headers------------------------------
				Response.ClearContent();
				Response.ClearHeaders();
				Response.AddHeader("Content-Type", MIME.gsGetResponseContentType(menMimeType));
				if (mbAsAttachment == true)
				{
					Response.AddHeader("Content-Disposition", "attachment;filename=" + msFileName);
				}
				else
				{
					Response.AddHeader("Content-Disposition", "inline;filename=" + msFileName);
				}
				if (mbTransferBinary == true)
				{
					Response.AddHeader("Content-Transfer-Encoding", "binary");
				}
				Response.AddHeader("Content-Length", abBuffer.Length.ToString());
				Response.Expires = -1;
	
	
				//-------------------------output the content-----------------------------
				Response.BinaryWrite(abBuffer);
				Response.Flush();
				Response.End();
	
			}
			catch (efException ex)
			{
	
				writer.Write(ex.Message);
	
				efEnvironment.goGetEnvironment(Application).goLogger.gLog(ASPTools.Logging.efEnumLogTypes.efError, ex.Message, oClientInfo);
	
	
	
			}

		}
	}

}
