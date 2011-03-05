sp_rename 'tdQuestions.qst_text', 'qst_text_alt', 'COLUMN'
GO
alter table tdQuestions
add qst_text NVARCHAR(768)
GO
update tdQuestions
set qst_text = qst_text_alt
GO
alter table tdQuestions
drop column qst_text_alt
GO