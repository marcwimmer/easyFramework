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
	//Class:     efProcessPage
	//--------------------------------------------------------------------------------'
	//Module:    efProcessPage.vb
	//--------------------------------------------------------------------------------'
	//Copyright: Promain Software-Betreuung GmbH
	//--------------------------------------------------------------------------------'
	//Purpose:   you will often need to store the result of a dialog. this is done
	//           by an ASP-process page
	//--------------------------------------------------------------------------------'
	//Created:   22.03.2004 01:14:10
	//--------------------------------------------------------------------------------'
	//Changed:
	//--------------------------------------------------------------------------------'
	//================================================================================
	//Imports:
	//================================================================================


	public class efProcessPage : efPage
	{


		//================================================================================
		//Public Methods:
		//================================================================================

		//================================================================================
		//Function:  sGetData
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
		public virtual string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{

			return "you should override the sGetData-Method";

		}

		//================================================================================
		//Protected Methods:
		//================================================================================

		//================================================================================
		//Sub:       Render
		//--------------------------------------------------------------------------------'
		//Purpose:   get the content of the function sGetData and display it
		//--------------------------------------------------------------------------------'
		//Params:    the html-writer
		//--------------------------------------------------------------------------------'
		//Created:   31.03.2004 17:56:56
		//--------------------------------------------------------------------------------'
		//Changed:
		//--------------------------------------------------------------------------------'
		protected override void Render (System.Web.UI.HtmlTextWriter writer)
		{
			try
			{
	
				XmlDocument oRequest = moRequestTosXml(Request);
				writer.Write(sGetData(oClientInfo, oRequest));
			}
			catch (efException ex)
			{
	
				writer.Write(ex.Message);
	
				efEnvironment.goGetEnvironment(Application).goLogger.gLog(ASPTools.Logging.efEnumLogTypes.efError, ex.Message, oClientInfo);
	
	
	
			}


		}


		//================================================================================
		//Private Methods:
		//================================================================================




	}

}
