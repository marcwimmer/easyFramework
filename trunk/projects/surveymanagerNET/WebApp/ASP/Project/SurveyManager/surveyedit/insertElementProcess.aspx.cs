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
	/// Zusammenfassung f�r insertElement.
	/// </summary>
	public class insertElementProcess : efProcessPage
	{
	

		#region Vom Web Form-Designer generierter Code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: Dieser Aufruf ist f�r den ASP.NET Web Form-Designer erforderlich.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{    
			
		}
		#endregion


		public override string sGetData(ClientInfo oClientInfo, XmlDocument oRequest)
		{
			string sType = oRequest.sGetValue("type");
			string sParentId = oRequest.sGetValue("parentid");
			string sMsg = "";

			if (sParentId == null | sParentId.Equals("")) 
			{
				throw new Exception("Parent-ID missing!");
			}

			int iParentId = Convert.ToInt32(sParentId);
			int iNewId = 0;
			try 
			{

				iNewId = SurveyEditor.glNewElement(oClientInfo, sType, iParentId);
			}
			catch (Exception ex) 
			{
				sMsg = "Fehler aufgetreten: " + "\n" + ex.Message + "\n" + ex.StackTrace;
				oClientInfo.gAddError(sMsg);

				throw ex;
				
			}

			if (oClientInfo.gsErrors().Length > 0) 
				sMsg = oClientInfo.gsErrors();
			else
				sMsg = "OK!" + "_" + Convert.ToString(iNewId);
			return sMsg;
		}
	}
}
