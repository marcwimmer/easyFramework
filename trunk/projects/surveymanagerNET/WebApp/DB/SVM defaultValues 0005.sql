/*


	Autor: Marc Wimmer
	Date:  2004/08/16

	Initialization for pearson-specific adaptions of the surveymanager
*/
DELETE FROM tsEntityTabs WHERE tab_Entity = 'Surveys' AND tab_tabID='Layout'
INSERT INTO tsEntityTabs(tab_Entity, tab_tabid, tab_tabtext, 
tab_xmldialogdefinitionfile)
VALUES('Surveys', 'Layout', 'Layout', 
'/ASP/Project/SurveyManager/baseedit/survey/entitytabs/tab_Layout.xml')

GO
