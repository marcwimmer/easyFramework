/*


	Autor: Marc Wimmer
	Date:  2004/06/29

	Initialization for pearson-specific adaptions of the surveymanager
*/
DELETE FROM tsEntityTabs WHERE tab_tabid='coupon'
INSERT INTO tsEntityTabs(tab_entity, tab_tabid, tab_tabtext,
tab_xmldialogdefinitionfile, tab_index)
VALUES('Surveys', 'coupon', 'Gutschein', '/ASP/Project/SurveyManagerPED/baseedit/survey/Dialog_Coupon.xml',500)

GO

DELETE FROM tsSysEvents WHERE sys_category = 'Survey' and sys_name='Submission'
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('Survey', 'Submission', 'CLASS', 100, 
'easyFramework.Project.Surveymanager.Pearson.OnSubmission', 'PearsonSVMBL')

GO
DELETE FROM tsSysEvents WHERE sys_category = 'global.asax' and sys_name='Application_Start'
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('global.asax', 'Application_Start', 'CLASS', 100, 
'easyFramework.Project.Surveymanager.Pearson.OnApplicationStart', 'PearsonSVMBL')

GO
DELETE FROM tsDomains WHERE dom_name ='Coupon_Download_URLs'
INSERT INTO tsDomains(dom_name, dom_description)
VALUES('Coupon_Download_URLs', 'Download URLs, wenn das eBook runtergeladen wird.')

GO

DELETE FROM tsDomains WHERE dom_name ='Produktreihe'
INSERT INTO tsDomains(dom_name, dom_description)
VALUES('Produktreihe', 'Reihen der Bücher')

GO

UPDATE tsEntities
SET ety_tableDefXml = '/ASP/Project/SurveyManagerPED/baseedit/survey/TableDef.xml'
WHERE ety_name = 'Surveys'