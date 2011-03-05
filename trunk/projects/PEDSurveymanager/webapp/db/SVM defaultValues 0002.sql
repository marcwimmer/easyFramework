----------------insert event-handler for survey-check-------------
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Surveys', 'CLASS', -2000, 'easyFramework.Project.Surveymanager.SurveyRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Surveys', 'CLASS', -2000, 'easyFramework.Project.Surveymanager.SurveyRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Answers', 'CLASS', -2000, 'easyFramework.Project.Surveymanager.AnswerRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Answers', 'CLASS', -2000, 'easyFramework.Project.Surveymanager.AnswerRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Questions', 'CLASS', 500, 'easyFramework.Project.Surveymanager.questionRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Questions', 'CLASS', 500, 'easyFramework.Project.Surveymanager.questionRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityInsert', 'Surveys', 'CLASS', 50, 'easyFramework.Project.Surveymanager.surveyRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
VALUES('EntityUpdate', 'Surveys', 'CLASS', 50, 'easyFramework.Project.Surveymanager.surveyRules', 'svmBusinessTasks')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntitySelect', 'SurveyGroups', 'SQL', -100, 'exec spGetAllowedSurveyGroups @usr_id=$usr_id')

INSERT INTO tsEntityPopups(epp_ety_name, epp_caption, epp_url, epp_index)
VALUES('Users',	'Projektordner zuordnen','/ASP/Project/SurveyManager/users2surveygroups/users2surveygroups.aspx?usr_id=$1',500)
GO
INSERT INTO tsEntityTabs(tab_entity, tab_tabid, tab_tabtext, tab_url, tab_index)
VALUES('Surveys', 'link', 'Link', '/ASP/Project/SurveyManager/baseedit/survey/entitytabs/tab_link.aspx?svy_id=$1', 200)
GO



INSERT INTO tsEntityMemos(eme_id, eme_caption, eme_ety_name, eme_fieldname, eme_index)
SELECT TOP 1 ISNULL(MAX(eme_id),0)+1, 'Glückwunsch HTML', 'surveys', 'svy_congratulation_html',5
FROM tsEntityMemos
GO
INSERT INTO tsEntityMemos(eme_id, eme_caption, eme_ety_name, eme_fieldname, eme_index)
SELECT TOP 1 ISNULL(MAX(eme_id),0)+1, 'Nicht geschafft HTML', 'surveys', 'svy_sorry_html',5
FROM tsEntityMemos
GO
