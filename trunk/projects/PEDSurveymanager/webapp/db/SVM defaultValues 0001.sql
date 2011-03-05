GO
INSERT INTO tdSurveyGroups(svg_id, svg_name, svg_parentid) VALUES(1, 'Tests', NULL)
INSERT INTO tdSurveyGroups(svg_id, svg_name, svg_parentid) VALUES(2, 'Gewinnspiele', NULL)
INSERT INTO tdSurveyGroups(svg_id, svg_name, svg_parentid) VALUES(3, 'Markt+Technik', 2)
GO
INSERT INTO tsRecordIds(TableName, MaxID) VALUES('tdSurveyGroups',3)
GO
---------------------entities------------------------
DELETE FROM tsEntities WHERE ety_name = 'SurveyGroups'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths, ety_primarysearchField)
VALUES('SurveyGroups', 'Gruppe', '/ASP/system/entityedit/entityedit.aspx?entity=SurveyGroups&svg_id=$1',
	'/ASP/Project/SurveyManager/baseedit/surveygroup/Dialog.xml',
	'/ASP/Project/SurveyManager/baseedit/surveygroup/TableDef.xml',
	600, 480, 'svg_name', 'svg_name', 0, 'svg_name', 'Gruppe','100px','svg_name')

GO
DELETE FROM tsEntityPopups WHERE epp_ety_name = 'Surveys'
DELETE FROM tsEntities WHERE ety_name = 'Surveys'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths, ety_primarysearchField)
VALUES('Surveys', 'Survey', '/ASP/system/entityedit/entityedit.aspx?entity=Surveys&svy_id=$1',
	'/ASP/Project/SurveyManager/baseedit/survey/Dialog.xml',
	'/ASP/Project/SurveyManager/baseedit/survey/TableDef.xml',
	600, 480, 'svy_name', 'svy_name', 0, 'svy_name', 'Survey','100px','svy_name')

GO
DELETE FROM tsEntityPopups WHERE epp_ety_name = 'Questions'
DELETE FROM tsEntities WHERE ety_name = 'Questions'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths, ety_primarysearchField)
VALUES('Questions', 'Frage', '/ASP/system/entityedit/entityedit.aspx?entity=Questions&qst_id=$1',
	'/ASP/Project/SurveyManager/baseedit/question/Dialog.xml',
	'/ASP/Project/SurveyManager/baseedit/question/TableDef.xml',
	600, 480, 'qst_name', 'qst_name', 0, 'qst_name', 'Frage','100px','qst_name')

GO
DELETE FROM tsEntityPopups WHERE epp_ety_name = 'Answers'
DELETE FROM tsEntities WHERE ety_name = 'Answers'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths, ety_primarysearchField)
VALUES('Answers', 'Antwort', '/ASP/system/entityedit/entityedit.aspx?entity=SurveyGroups&svg_id=$1',
	'/ASP/Project/SurveyManager/baseedit/answer/Dialog.xml',
	'/ASP/Project/SurveyManager/baseedit/answer/TableDef.xml',
	600, 480, 'ans_resultDbField', 'ans_resultDbField', 0, 'ans_resultDbField', 'Antwort','100px','ans_resultDbField')

GO
DELETE FROM tsEntityPopups WHERE epp_ety_name = 'AnswerValues'
DELETE FROM tsEntities WHERE ety_name = 'AnswerValues'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths, ety_primarysearchField)
VALUES('AnswerValues', 'Antwortwert', '/ASP/system/entityedit/entityedit.aspx?entity=AnswerValues&svg_id=$1',
	'/ASP/Project/SurveyManager/baseedit/answervalue/Dialog.xml',
	'/ASP/Project/SurveyManager/baseedit/answervalue/TableDef.xml',
	600, 480, 'val_value', 'val_value', 0, 'val_value', 'Antwortwert','100px','val_value')

GO
DELETE FROM tsEntityPopups WHERE epp_ety_name = 'Publications'
DELETE FROM tsEntities WHERE ety_name = 'Publications'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths, ety_primarysearchField)
VALUES('Publications', 'Gruppe', '/ASP/system/entityedit/entityedit.aspx?entity=Publications&svg_id=$1',
	'/ASP/Project/SurveyManager/baseedit/publications/Dialog.xml',
	'/ASP/Project/SurveyManager/baseedit/publications/TableDef.xml',
	600, 480, 'pub_name', 'pub_name', 0, 'pub_name', 'Publikation','100px','pub_name')

GO
---------------------tsMenuItems--------------------
DELETE FROM tsMainMenue WHERE Project='SVM'
GO
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('surveygroups', null, 'Gruppen', '', 1, 0, 'treeview_folder','treeview_folder_open','SVM')
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('editsurveygroups', 'admin', 'Gruppen bearbeiten', '/ASP/system/entityedit/entityedit.aspx?entity=SurveyGroups', 1, 0, 'treeview_item','treeview_item','SVM')
GO
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('actions_new_survey', null, 'Neues Survey erstellen', '/ASP/Project/SurveyManager/newsurvey/newsurvey.aspx', 1, 0, 'treeview_item','treeview_item','SVM')
GO
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('config', 'admin', 'Konfiguration', '/ASP/Project/SurveyManager/config/config.aspx', 1, 1, 'treeview_item','treeview_item','SVM')

GO
--------------------tsDataCombo--------------------
DELETE FROM tsDataCombo WHERE Project = 'SVM'
INSERT INTO tsDataCombo(dc_name, dc_qry, Project)
VALUES('SurveyGroups', 'SELECT svg_id, svg_name FROM tdSurveyGroups ORDER BY 2', 'SVM')

INSERT INTO tsDataCombo(dc_name, dc_qry, PROJECT)
VALUES('SurveyStates','SELECT sst_id, sst_name FROM tdSurveyStates ORDER BY 2','SVM')
GO
INSERT INTO tsDataCombo(dc_name, dc_qry, PROJECT)
VALUES('SurveyTypes','SELECT svt_id, svt_name FROM tdSurveyTypes ORDER BY 2','SVM')
GO
INSERT INTO tsDataCombo(dc_name, dc_qry, PROJECT)
VALUES('AnswerTypes','SELECT aty_id, aty_name FROM tdAnswerTypes ORDER BY 2','SVM')
GO

GO

--------------------tdSurveytypes------------------
INSERT INTO tdSurveyTypes(svt_id, svt_name)
VALUES('PROFITPLAY', 'Gewinnspiel')
GO
INSERT INTO tdSurveyTypes(svt_id, svt_name)
VALUES('TEST', 'Test / Prüfungsvorbereitung')
GO
INSERT INTO tdSurveyTypes(svt_id, svt_name)
VALUES('VOTE', 'Abstimmung')
GO
-------------------tdSurveyStates-----------------
INSERT INTO tdSurveyStates(sst_id, sst_name)
VALUES('DESIGN', 'In Entwicklung')
GO
INSERT INTO tdSurveyStates(sst_id, sst_name)
VALUES('READYTOPUBLISH', 'Wartet auf Publikation')
GO
INSERT INTO tdSurveyStates(sst_id, sst_name)
VALUES('ONLINE', 'Online')
GO
INSERT INTO tdSurveyStates(sst_id, sst_name)
VALUES('OFFLINE', 'Offline')
GO
-------------------tdAnswertypes---------------
INSERT tdAnswertypes(aty_id, aty_name)
VALUES('FREETEXT', 'Freitext')
GO
INSERT tdAnswertypes(aty_id, aty_name)
VALUES('MULTIPLECHOICE', 'multiple-choice')
GO
INSERT tdAnswertypes(aty_id, aty_name)
VALUES('SINGLECHOICE', '1 aus x')
GO
INSERT tdAnswertypes(aty_id, aty_name)
VALUES('GAPTEXT', 'Lückentext')
GO
------------------tsEntityPopups---------------
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption, epp_url, epp_index)
VALUES('Surveys', 'Fragen bearbeiten', '/ASP/Project/SurveyManager/surveyedit/surveyedit.aspx?svy_id=$1',0)
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption, epp_url, epp_index)
VALUES('Surveys', 'Preview', '/ASP/Project/SurveyManager/preview/preview.aspx?svy_id=$1',0)
GO

------------------tsEntityMemos---------------

INSERT INTO tsEntityMemos(eme_id,eme_caption,eme_ety_name,eme_fieldname,eme_index)
VALUES(2, 'Einleitung HTML', 'Surveys', 'svy_intro_html',1)
GO
INSERT INTO tsEntityMemos(eme_id,eme_caption,eme_ety_name,eme_fieldname,eme_index)
VALUES(3, 'Ausleitung HTML', 'Surveys', 'svy_extro_html',2)
GO
UPDATE tsRecordIds SET MaxId = (SELECT Max(eme_id) FROM tsEntityMemos) WHERE TableName='tsEntityMemos'
GO

----------------tsRecordIds---------------------
if not exists(select * from tsRecordIds where tablename = 'tsEntityPopups')
insert into tsRecordIDs(TableName, MaxId) values('tsEntityPopups',0)
go
update tsRecordIDs
set MaxId = isnull((Select Max(epp_id) from tsEntityPopups), 0)
where tablename='tsEntityPopups'




-----------------tsSysEvents---------------------
DELETE FROM tsSysEvents


INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Answers', 'SQL',-1000, 'DELETE FROM tdAnswerValues WHERE val_ans_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Questions', 'SQL',-1000, 'DELETE FROM tdAnswerValues WHERE val_ans_id IN (SELECT ans_id FROM tdAnswers WHERE ans_qst_id = $1)')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Questions', 'SQL',-500, 'DELETE FROM tdAnswers WHERE ans_qst_id=$1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Surveys', 'SQL',-500, 'DELETE FROM tdQuestions WHERE qst_svy_id=$1')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Surveys', 'SQL',-1000, 'DELETE FROM tdAnswers WHERE ans_qst_id IN (SELECT qst_id FROM tdQuestions WHERE qst_svy_id=$1)')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Surveys', 'SQL',-1000, 'DELETE FROM tdAnswerValues WHERE val_ans_id IN (SELECT ans_id FROM tdAnswers WHERE ans_qst_id IN (SELECT qst_id FROM tdQuestions WHERE qst_svy_id=$1))')

	
GO
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Publications', 'CLASS', 100, 'easyFramework.Project.Surveymanager.synchResultTable_PublicationUpdate', 'svmBusinessTasks')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Surveys', 'CLASS', 100, 'easyFramework.Project.Surveymanager.synchResultTable_SurveyUpdate', 'svmBusinessTasks')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Publications', 'CLASS', 100, 'easyFramework.Project.Surveymanager.synchResultTable_PublicationUpdate', 'svmBusinessTasks')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Surveys', 'CLASS', 100, 'easyFramework.Project.Surveymanager.synchResultTable_SurveyUpdate', 'svmBusinessTasks')
GO
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Answers', 'CLASS', 100, 'easyFramework.Project.Surveymanager.synchResultTable_AnswerInsertUpdate', 'svmBusinessTasks')
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Answers', 'CLASS', 100, 'easyFramework.Project.Surveymanager.synchResultTable_AnswerInsertUpdate', 'svmBusinessTasks')

GO

-----------------tsEntityTabs---------------------
DELETE FROM tsEntityTabs


INSERT INTO tsEntityTabs(tab_entity, tab_tabid, tab_tabtext, tab_url, tab_index)
VALUES('Surveys', 'publications', 'Publikationen', '/ASP/Project/SurveyManager/baseedit/survey/entitytabs/tab_publications.aspx?svy_id=$1', 300)

