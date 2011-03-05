insert into tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
values('EntityInsert', 'AnswerValues', 'CLASS', -2000, 'easyFramework.Project.Surveymanager.answerValueRules', 'svmBusinessTasks')
GO
insert into tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
values('EntityUpdate', 'AnswerValues', 'CLASS', -2000, 'easyFramework.Project.Surveymanager.answerValueRules', 'svmBusinessTasks')
GO

insert into tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
values('EntityInsert', 'AnswerValues', 'CLASS', 2000, 'easyFramework.Project.Surveymanager.answerValueRules', 'svmBusinessTasks')
GO
insert into tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
values('EntityUpdate', 'AnswerValues', 'CLASS', 2000, 'easyFramework.Project.Surveymanager.answerValueRules', 'svmBusinessTasks')
GO
insert into tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
values('EntityInsert', 'Answers', 'CLASS', 2000, 'easyFramework.Project.Surveymanager.answerRules', 'svmBusinessTasks')
GO
insert into tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task, sys_assembly)
values('EntityUpdate', 'Answers', 'CLASS', 2000, 'easyFramework.Project.Surveymanager.answerRules', 'svmBusinessTasks')
GO