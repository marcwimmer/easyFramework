ALTER TABLE tdSurveys
ADD svy_layout_columns INT
GO
ALTER TABLE tdAnswerValues
ADD val_html NVARCHAR(256)
GO
ALTER TABLE tdQuestions
ADD qst_html_before NVARCHAR(256), qst_html_after NVARCHAR(256)
GO

ALTER TABLE tdSurveys
ADD svy_layout_description_on_the_left BIT
GO
