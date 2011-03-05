/*

	Author: Marc Wimmer
	Copyright: Promain Software-Betreuung GmbH
	Date:	21.03.2004

	Creates the base-Tables for Surveymanager

*/
-------------- Surveygroups --------------------
if exists(select * From dbo.sysobjects where name='fkTdSurveysTdSurveyGroups')
	alter table tdSurveys drop constraint fkTdSurveysTdSurveyGroups
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdSurveyGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdSurveyGroups]
GO
CREATE TABLE tdSurveyGroups(
svg_id INT NOT NULL,
svg_parentid INT,
svg_name NVARCHAR(50),
svg_inserted DATETIME,
svg_updated DATETIME,
svg_insertedby NVARCHAR(255),
svg_updatedby NVARCHAR(255),
svg_css NVARCHAR(255)
)
GO
ALTER TABLE tdSurveyGroups ADD CONSTRAINT aIdxTdSurveyGroups PRIMARY KEY(svg_id)
GO
-------------- Surveytypes --------------------
if exists(select * From dbo.sysobjects where name='fkTdSurveysTdSurveytypes')
	alter table tdSurveys drop constraint fkTdSurveysTdSurveytypes
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdSurveytypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdSurveytypes]
GO
CREATE TABLE tdSurveytypes(
	svt_id NVARCHAR(20) NOT NULL,
	svt_name NVARCHAR(80),
	svt_inserted DATETIME,
	svt_updated DATETIME,
	svg_insertedby NVARCHAR(255),
	svg_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdSurveytypes ADD CONSTRAINT aIdxTdSurveytypes PRIMARY KEY(svt_id)
GO
-------------- SurveyStates --------------------
if exists(select * From dbo.sysobjects where name='fkTdSurveysTdSurveystates')
	alter table tdSurveys drop constraint fkTdSurveysTdSurveystates
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdSurveyStates]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdSurveyStates]
GO
CREATE TABLE tdSurveyStates(
	sst_id NVARCHAR(20) NOT NULL,
	sst_name NVARCHAR(80),
	sst_inserted DATETIME,
	sst_updated DATETIME,
	sst_insertedby NVARCHAR(255),
	sst_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdSurveyStates ADD CONSTRAINT aIdxTdSurveystates PRIMARY KEY(sst_id)
GO

-------------- Surveys --------------------
if exists(select * from dbo.sysobjects where name ='fkTdPublicationsTdSurveys')
	alter table tdPublications drop constraint fkTdPublicationsTdSurveys
GO
if exists(select * From dbo.sysobjects where name='fkTdQuestionsTdSurveys')
	alter table tdQuestions drop constraint fkTdQuestionsTdSurveys
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdSurveys]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdSurveys]
GO
CREATE TABLE tdSurveys(
	svy_id INT NOT NULL,
	svy_svg_id INT NOT NULL,
	svy_name NVARCHAR(80),
	svy_svt_id NVARCHAR(20),
	svy_sst_id NVARCHAR(20),
	svy_externalid NVARCHAR(20),
	svy_template BIT NOT NULL DEFAULT(0),
	svy_inserted DATETIME,
	svy_updated DATETIME,
	svy_insertedby NVARCHAR(255),
	svy_updatedby NVARCHAR(255),
	svy_intro_html NTEXT,
	svy_extro_html NTEXT,
	svy_css NVARCHAR(255)
)
GO
ALTER TABLE tdSurveys ADD CONSTRAINT aIdxTdSurveys PRIMARY KEY(svy_id)
GO
ALTER TABLE tdSurveys ADD CONSTRAINT fkTdSurveysTdSurveytypes FOREIGN KEY(svy_svt_id) REFERENCES 
	tdSurveytypes(svt_id)
GO
ALTER TABLE tdSurveys ADD CONSTRAINT fkTdSurveysTdSurveyStates FOREIGN KEY(svy_sst_id) REFERENCES 
	tdSurveyStates(sst_id)
GO
ALTER TABLE tdSurveys ADD CONSTRAINT fkTdSurveysTdSurveyGroups FOREIGN KEY(svy_svg_id) REFERENCES 
	tdSurveyGroups(svg_id)

GO
-------------- Questions --------------------
if exists(select * From dbo.sysobjects where name='fkTdAnswersTdQuestions')
	alter table tdAnswers drop constraint fkTdAnswersTdQuestions
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdQuestions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdQuestions]
GO
CREATE TABLE tdQuestions(
	qst_id INT NOT NULL,
	qst_svy_id INT NOT NULL,
	qst_text NVARCHAR(300),
	qst_index INT NOT NULL DEFAULT (0),
	qst_inserted DATETIME,
	qst_updated DATETIME,
	qst_insertedby NVARCHAR(255),
	qst_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdQuestions ADD CONSTRAINT aidxTdQuestions PRIMARY KEY(qst_id)
GO
ALTER TABLE tdQuestions ADD CONSTRAINT fkTdQuestionsTdSurveys FOREIGN KEY(qst_svy_id) REFERENCES 
	tdSurveys(svy_id)
GO
-------------- Answertypes --------------------
if exists(select * From dbo.sysobjects where name='fkTdAnswersTdAnswertypes')
	alter table tdAnswers drop constraint fkTdAnswersTdAnswertypes
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdAnswertypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdAnswertypes]
GO
CREATE TABLE tdAnswertypes(
	aty_id NVARCHAR(20) NOT NULL,
	aty_name NVARCHAR(255) NOT NULL
)
GO
ALTER TABLE tdAnswertypes ADD CONSTRAINT aidxTdAnswertypes PRIMARY KEY(aty_id)
GO
-------------- Answers --------------------
if exists(select * From dbo.sysobjects where name='fkTdAnswerValuesTdAnswers')
	alter table tdAnswerValues drop constraint fkTdAnswerValuesTdAnswers
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tdAnswers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdAnswers]
GO
CREATE TABLE tdAnswers(
	ans_id INT NOT NULL,
	ans_qst_id INT NOT NULL,
	ans_aty_id NVARCHAR(20), 
	ans_gap_txt NTEXT,
	ans_resultDbField NVARCHAR(20),
	ans_index INT NOT NULL DEFAULT(0), 
	ans_multiline BIT NOT NULL DEFAULT(0),
	ans_lines INT NOT NULL DEFAULT(1),
	ans_inserted DATETIME,
	ans_updated DATETIME,
	ans_insertedby NVARCHAR(255),
	ans_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdAnswers ADD CONSTRAINT aidxTdAnswers PRIMARY KEY(ans_id)
GO
ALTER TABLE tdAnswers ADD CONSTRAINT fkTdAnswersTdQuestions FOREIGN KEY(ans_qst_id) REFERENCES 
	tdQuestions(qst_id)
GO
ALTER TABLE tdAnswers ADD CONSTRAINT fkTdAnswersTdAnswertypes FOREIGN KEY(ans_aty_id) REFERENCES 
	tdAnswertypes(aty_id)
GO
ALTER TABLE tdAnswers ADD CONSTRAINT chkTdAnswersAns_lines CHECK (ans_lines >= 1)
GO
-------------- Answervalues (for select-answers) --------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[tdAnswerValues]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdAnswerValues]
GO
CREATE TABLE tdAnswerValues(
	val_id INT NOT NULL,
	val_ans_id INT NOT NULL,
	val_text NVARCHAR(1024),
	val_correct BIT NOT NULL DEFAULT(0),
	val_for_db NVARCHAR(10), 
	val_index INT NOT NULL DEFAULT(0),
	val_inserted DATETIME,
	val_updated DATETIME,
	val_insertedby NVARCHAR(255),
	val_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdAnswerValues ADD CONSTRAINT aidxtdAnswerValues PRIMARY KEY(val_id)
GO
ALTER TABLE tdAnswerValues ADD CONSTRAINT fkTdAnswerValuesTdAnswers FOREIGN KEY(val_ans_id) REFERENCES 
	tdAnswers(ans_id)
GO


-------------- Publications  --------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[tdPublications]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdPublications]
GO
CREATE TABLE tdPublications(
	pub_id INT NOT NULL,
	pub_svy_id INT NOT NULL,
	pub_inserted DATETIME,
	pub_updated DATETIME,
	pub_insertedby NVARCHAR(255),
	pub_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdPublications ADD CONSTRAINT aidxTdPublications PRIMARY KEY(pub_id)
GO
ALTER TABLE tdPublications ADD CONSTRAINT fkTdPublicationsTdSurveys FOREIGN KEY(pub_svy_id) REFERENCES 
	tdSurveys(svy_id)
GO


-------------- CONFIG-table  --------------------

if exists (select * from dbo.sysobjects where id = object_id(N'[tdConfig]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdConfig]
GO
CREATE TABLE tdConfig(
	cfg_name NVARCHAR(255) NOT NULL,
	cfg_value NVARCHAR(1024)
)
GO
ALTER TABLE tdConfig ADD CONSTRAINT aIdxTdConfig PRIMARY KEY (cfg_name)
GO

