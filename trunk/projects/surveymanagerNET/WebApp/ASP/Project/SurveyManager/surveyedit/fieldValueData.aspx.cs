using System;
using easyFramework;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Frontend.ASP.Dialog;
using easyFramework.Sys.Xml;
using easyFramework.Project.SurveyManager.Preparation;

namespace easyFramework.Project.SurveyManager.ASP.Project.SurveyManager.surveyedit
{
	/// <summary>
	/// Zusammenfassung für insertElement.
	/// </summary>
	public class fieldValueData : efDataPage
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


		/***
		 * liefert einen Feldwert aus der Datenbank zurück
		 */
		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
			string sFrom = oRequest.sGetValue("from");
			string sWhere = oRequest.sGetValue("where");
			string sField = oRequest.sGetValue("field");

			return DataMethodsClientInfo.gsGetDBValue(oClientInfo, sFrom, sField, sWhere, "");

		}
	}
}
