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
	/// Zusammenfassung für deleteElement.
	/// </summary>
	public class deleteElementProcess : efProcessPage
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


		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
			string sType = oRequest.sGetValue("type");
			string sId = oRequest.sGetValue("id");
			string sMsg = "";

			if (sId == null | sId.Equals("")) 
			{
				throw new Exception("Id missing!");
			}

			int iId = Convert.ToInt32(sId);

			SurveyEditor.gDeleteElement(oClientInfo, sType, iId);
			
			if (oClientInfo.gsErrors().Length > 0) 
				sMsg = oClientInfo.gsErrors();
			else
				sMsg = "OK!";

			return sMsg;
		}
	}
}
