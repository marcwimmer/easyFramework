
/*-------------------------
border-html
--------------------------*/
ALTER TABLE tdSurveys
ADD svy_border_html_begin NTEXT, svy_border_html_end NTEXT
GO
ALTER TABLE tdSurveyGroups
ADD svg_border_html_begin NTEXT, svg_border_html_end NTEXT
GO
