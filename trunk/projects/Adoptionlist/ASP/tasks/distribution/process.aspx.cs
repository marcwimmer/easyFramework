using System;
using System.IO;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;
using easyFramework.Sys;
using easyFramework.Tasks.Distribution;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.ASPTools;

namespace easyFramework.Project.Default.ASP.tasks.distribution
{
	/// <summary>
	/// handles the upload of the package to the abonnent.
	/// </summary>
	public class process : efProcessPage
	{
		
		#region Vom Web Form-Designer generierter Code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist für den ASP.NET Web Form-Designer erforderlich.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{    
			
		}
		#endregion

		override public  string  sGetData(ClientInfo oClientInfo, XmlDocument oXmlRequest) 
		{

			//-----------------get abonnent and package-----------------
			string sAbo_id = oXmlRequest.sGetValue("abo_id");
			string sDst_id = oXmlRequest.sGetValue("dst_id");

			string sWhere = "abo_id=" + sAbo_id;
			Recordset rsAbonnent = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsAbonnents", 
				"*", sWhere, null, null, null);

			sWhere = "dst_id=" + sDst_id;
			Recordset rsAbonnentsPackages = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsAbonnentsPackages", 
				"*", sWhere, null, null, null);


			string sResult = "";
			try 
			{


				if (rsAbonnent.EOF) 
					throw new efException("Abonnent with id " + sAbo_id + " wasn't found.");
				if (rsAbonnentsPackages.EOF) 
					throw new efException("Abonnent-Package with dst_id " + sDst_id + " wasn't found.");

				string sHost = rsAbonnent["abo_host"].sValue;
				string sURL_To_Call = sHost;
				if (sURL_To_Call.Substring(sURL_To_Call.Length - 2, 1) == "/") 
					sURL_To_Call = sURL_To_Call.Substring(0, sURL_To_Call.Length - 1);
				sURL_To_Call += Convert.ToString(oClientInfo.oHttpApp.oGet("distribution_receiving_asp"));

				//open the zip-archive as byte array
				string sZipFileName = rsAbonnentsPackages["dst_packagename"].sValue;
				string sFileName = oClientInfo.oHttpApp.oGet("distribution_made_package_folder").ToString() +
					"\\" +
					sZipFileName;
				sFileName = sFileName.Replace("\\\\", "\\");

				if (!File.Exists(sFileName))
					throw new efException("The package-file \"" + sFileName + "\" doesn't exist anymore. Distribution failed.");

				FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

				//read complete array into memory at first; later, we could send in smaller packages
				byte[] abFileContent = new byte[fs.Length];
				fs.Read(abFileContent, 0, Convert.ToInt32(fs.Length));

				//make a call to the web-page
				XmlDocument oXmlDataToPost = new XmlDocument("<package></package>");
				oXmlDataToPost.selectSingleNode("/package").AddNode("content", true).sText = 
					Envelope.gsEnvelope(ref abFileContent);

				oXmlDataToPost.selectSingleNode("/package").AddNode("filename", true).sText = 
					sZipFileName;

				sResult = Tools.gsPostXmlData(oClientInfo, sURL_To_Call, oXmlDataToPost);


			}
			catch (Exception ex) 
			{
				sResult = ex.Message;
			}
			finally
			{


				//enter the successful distribution into the database
				bool bSucceeded;
				if (sResult == "OK") 
					bSucceeded = true;
				else
					bSucceeded = false;
			
				Recordset rsInsertLog = new Recordset();
				rsInsertLog = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsAbonnentsPackagesLogs", 
					"*", "1=2", null, null, null);
				rsInsertLog.AddNew();
				rsInsertLog["dstlog_id"].lValue = DataMethodsClientInfo.glDBValue(oClientInfo, 
					"SELECT (ISNULL(MAX(dstlog_id), 0) + 1) AS MAXVALUE FROM tsAbonnentsPackagesLogs", 1, 1);
				rsInsertLog["dstlog_dst_id"].lValue = rsAbonnentsPackages["dst_id"].lValue;
				rsInsertLog["dstlog_infotext"].sValue = sResult;
				rsInsertLog["dstlog_succeeded"].bValue = bSucceeded;
				rsInsertLog["dstlog_abo_id"].lValue = rsAbonnent["abo_id"].lValue;
				rsInsertLog["dstlog_abo_name"].sValue = rsAbonnent["abo_name"].sValue;
				rsInsertLog["dstlog_host"].sValue = rsAbonnent["abo_host"].sValue;
				rsInsertLog["dstlog_inserted"].dtValue = Functions.Now();
				rsInsertLog["dstlog_insertedBy"].sValue = oClientInfo.gsGetUsername();
				DataMethodsClientInfo.gExecuteQuery(oClientInfo, rsInsertLog.gsSqlInsert("tsAbonnentsPackagesLogs"));
			}
			//return result
			return sResult;

		}
	}


}
