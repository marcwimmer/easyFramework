using System;
using easyFramework.Sys;
using easyFramework.Sys.Data;
using easyFramework.Sys.ToolLib;
using easyFramework.Sys.Xml;
using easyFramework.Sys.SysEvents;
using easyFramework.Frontend.ASP.HTMLRenderer;


namespace easyFramework.Project.SurveyManager
{
	public class svmEnums
	{
		
		public const string entSurveys = "Surveys";
		public const string entQuestions = "Questions";
		public const string entAnswers = "Answers";
		public const string entAnswerValues = "AnswerValues";
		public const string entSurveyGroups = "SurveyGroups";
		public const string entPublications = "Publications";

		public const string efsSURVEYSTATE_DESIGN = "DESIGN";
		public const string efsSURVEYSTATE_ONLINE = "OFFLINE";
		public const string efsSURVEYSTATE_OFFLINE = "ONLINE";
		public const string efsSURVEYSTATE_READYTOPUBLISH = "READYTOPUBLISH";


		public const string efsFieldUsedQuestions = "usedQuestions"; //hidden-field, das alle benutzten Fragen eines Surveys enthält
		
	}
	
}
