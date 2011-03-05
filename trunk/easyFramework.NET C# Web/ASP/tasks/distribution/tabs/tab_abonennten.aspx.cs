using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.Entities;
using easyFramework.Sys.ToolLib;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Frontend.ASP.ComplexObjects;
using easyFramework.Sys.Xml;
using easyFramework.Frontend.ASP.HTMLRenderer;


namespace easyFramework.Project.Default.ASP.tasks.distribution.tabs
{
	
	/// <summary>
	/// Zusammenfassung für tab_abonennten.
	/// </summary>
	public class tab_abonennten : efDialogPage
	{
		private int mlEntityId;

		public override void CustomInit (XmlDocument oXmlRequest)
		{
			
			mlEntityId = Convert.ToInt32(oXmlRequest.sGetValue("entityId", "-1"));
		
		
		}

		public string gsWritePackageStatus() 
		{

			
			efHtmlTable oTable = new efHtmlTable();
			oTable["class"].sValue = "borderTable";

			//---------------heading---------------
			efHtmlTr oTrHead = new efHtmlTr(oTable);
			efHtmlTd oTd = new efHtmlTd(oTrHead);
			oTd["class"].sValue = "captionField";
			efHtmlTextNode oText = new efHtmlTextNode(oTd);
			oText.sText = "Paket";

			oTd = new efHtmlTd(oTrHead);
			oTd["class"].sValue = "captionField";
			oText = new efHtmlTextNode(oTd);
			oText.sText = "Datum";

			oTd = new efHtmlTd(oTrHead);
			oTd["class"].sValue = "captionField";
			oText = new efHtmlTextNode(oTd);
			oText.sText = "Status";

			oTd = new efHtmlTd(oTrHead);
			oTd["class"].sValue = "captionField";
			oText = new efHtmlTextNode(oTd);
			oText.sText = "Host";
			
			oTd = new efHtmlTd(oTrHead);
			oTd["class"].sValue = "captionField";
			oText = new efHtmlTextNode(oTd);
			oText.sText = "Aktion";
			
			//-----------get abonnents---------------
			string sWhere = null;
			if (mlEntityId == -1) sWhere = "1=2";
			Recordset rsAbonnents = DataMethodsClientInfo.gRsGetTable(oClientInfo, "tsAbonnents", "*", 
				sWhere, null, null, "abo_name");

			while (!rsAbonnents.EOF) 
			{
				efHtmlTr oTr = new efHtmlTr(oTable);
				

				//abo-name
				oTd = new efHtmlTd(oTr);
				oTd["class"].sValue = "entryField";
				oText = new efHtmlTextNode(oTd);
				oText.sText = rsAbonnents["abo_name"].sValue;


				//get last log-entry
				sWhere = "dstlog_dst_id = '$1' AND dstlog_abo_id = '$2'";
				sWhere = sWhere.Replace("$1", Convert.ToString(mlEntityId));
				sWhere = sWhere.Replace("$2", Convert.ToString(rsAbonnents["abo_id"].sValue));
				Recordset rsLog = DataMethodsClientInfo.gRsGetTable(oClientInfo, 
					"tsAbonnentsPackagesLogs", "*", sWhere, null, null, "dstlog_id DESC");

				//date
				oTd = new efHtmlTd(oTr);
				oTd["class"].sValue = "entryField";
				oText = new efHtmlTextNode(oTd);
				if (rsLog.EOF == false)
					oText.sText = oClientInfo.gsFormatDate(rsLog["dstlog_inserted"].dtValue);

				//status
				//check wether distribution succeeded:
				
				string sErrMsg = "";
				
				Recordset rsLastLog = DataMethodsClientInfo.gRsGetTable(oClientInfo, 
					"tsAbonnentsPackagesLogs", "*", sWhere, 
					null, null, "dstlog_id DESC");
				
				sErrMsg = "";

				if (rsLastLog.EOF)   
				{
					sErrMsg = "- keine Verteilung bisher -";
				}
				else 
				{
					

					if (rsLastLog["dstlog_succeeded"].bValue)
					{
						//succeeded true
						sErrMsg = "Verteilt am " + oClientInfo.gsFormatDate(
							rsLastLog["dstlog_inserted"].dtValue);
					}
					else 
					{
						sWhere = sWhere + " AND dstlog_succeeded = 0 ";
						sErrMsg = rsLastLog["dstlog_infotext"].sValue;
					}
				}


				oTd = new efHtmlTd(oTr);
				oTd["class"].sValue = "entryField";
				oText = new efHtmlTextNode(oTd);
				oText.sText = sErrMsg;
				
				//host
				oTd = new efHtmlTd(oTr);
				oTd["class"].sValue = "entryField";
				oText = new efHtmlTextNode(oTd);
				oText.sText = "<a target=\"_blank\" href=\"" + rsAbonnents["abo_host"].sValue + "\">" + rsAbonnents["abo_host"].sValue + "</a>";

				//aktion
				oTd = new efHtmlTd(oTr);
				oTd["class"].sValue = "entryField";
				efHtmlButton oButton = new efHtmlButton(oTd);
				oButton.sText = "Upload";
				oButton["onclick"].sValue = "mOnUploadPackageClick('$1', '$2');";
				oButton["onclick"].sValue = oButton["onclick"].sValue.Replace("$1", Convert.ToString(mlEntityId));
				oButton["onclick"].sValue = oButton["onclick"].sValue.Replace("$2", rsAbonnents["abo_id"].sValue);
				
				rsAbonnents.MoveNext();
			}

			FastString oResult = new FastString();
			oTable.gRender(oResult, 3);
			return oResult.ToString();
		}

	}
}
