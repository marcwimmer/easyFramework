update tdAnswerTypes
set aty_xmldialog_path = '/ASP/Project/SurveyManager/BaseEdit/answer/Dialog_FreeText.xml'
where aty_id = 'FREETEXT'

update tdAnswerTypes
set aty_xmldialog_path = '/ASP/Project/SurveyManager/BaseEdit/answer/Dialog_SingleChoice.xml'
where aty_id = 'SINGLECHOICE'

update tdAnswerTypes
set aty_xmldialog_path = '/ASP/Project/SurveyManager/BaseEdit/answer/Dialog_MultipleChoice.xml'
where aty_id = 'MULTIPLECHOICE'

update tdAnswerTypes
set aty_xmldialog_path = '/ASP/Project/SurveyManager/BaseEdit/answer/Dialog_GapText.xml'
where aty_id = 'GAPTEXT'

update tdAnswerTypes
set aty_xmldialog_path = '/ASP/Project/SurveyManager/BaseEdit/answer/Dialog_CheckBox.xml'
where aty_id = 'CHECKBOX'

--Bezeichnung verbessern
update tdAnswerTypes
set aty_name = '1 aus n'
where aty_id = 'SINGLECHOICE'