/*


	Autor: Marc Wimmer
	Date:  2004/08/13

	Initialization for pearson-specific adaptions of the surveymanager
*/

INSERT INTO tsEntityTabs(tab_Entity, tab_tabid, tab_tabtext, 
tab_xmldialogdefinitionfile)
VALUES('Surveys', 'Newsletter', 'Newsletter', '/ASP/Project/SurveyManagerPED/baseedit/survey/Dialog_Newsletter.xml')

GO

INSERT INTO tsDomains(dom_name, dom_description)
VALUES('Newsletterlists', 'Newsletterlisten (Datenbank PEDCampaigns2002)')
