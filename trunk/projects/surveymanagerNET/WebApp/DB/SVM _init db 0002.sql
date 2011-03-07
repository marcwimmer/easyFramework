/*

	Author: Marc Wimmer
	Copyright: Promain Software-Betreuung GmbH
	Date:	14.04.2004

	Creates the base-Tables for Surveymanager

*/
-------------- extend Survey with memo-field for congratulations-memo --------------------
ALTER TABLE tdSurveys
ADD svy_congratulation_html NTEXT,
svy_sorry_html NTEXT
GO


---------------add mandatory for answer-----------------------
ALTER TABLE tdAnswers
ADD ans_mandatory BIT NOT NULL DEFAULT(0) 
GO

---------------add percentage required for passing a test-----------------------
ALTER TABLE tdSurveys
ADD svy_testpassed_percentage INT



/*-------------------------------------------------
stored procedure spGetAllowedSurveyGroups
--------------------------------------------------*/
GO
create procedure spGetAllowedSurveyGroups 
@usr_id int

as

SET NOCOUNT ON

/*-------------------------------------------------
get the survey-groups of the current user
--------------------------------------------------*/
create table #allowedGroups(Allowed int)

insert into #allowedGroups (Allowed)
select svg_usr_svg_id from tdSurveyGroupsTsUsers where svg_usr_usr_id=@usr_id


--get all children of each top-parent--
DECLARE C CURSOR FOR SELECT Allowed FROM #allowedGroups
OPEN C
DECLARE @svg_id INT

FETCH NEXT FROM C INTO @svg_id
WHILE @@FETCH_STATUS = 0
BEGIN

	--the inserted ids will be fetched at the next fetch; so all parents
	--can be fetched

	INSERT INTO #allowedGroups(Allowed)
	SELECT svg_id FROM tdSurveyGroups WHERE svg_parentid=@svg_id

	FETCH NEXT FROM C INTO @svg_id
	
END
CLOSE C
DEALLOCATE C



select * from #allowedGroups



drop table #allowedGroups
GO





/*--------------------------------------------------------------------------------
		TABLE tdSurveyGroupsTsUsers
--------------------------------------------------------------------------------*/


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tdSurveyGroupsTsUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tdSurveyGroupsTsUsers]
GO

CREATE TABLE [dbo].[tdSurveyGroupsTsUsers] (
	[svg_usr_id] [int] NOT NULL ,
	[svg_usr_svg_id] [int] NULL ,
	[svg_usr_usr_id] [int] NULL ,
	[svg_usr_inserted] [datetime] NOT NULL DEFAULT(getdate()),
	[svg_usr_insertedBy] [nvarchar] (255) NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tdSurveyGroupsTsUsers] WITH NOCHECK ADD 
	CONSTRAINT [aIdxTdSurveyGroupsTsUsers] PRIMARY KEY  CLUSTERED 
	(
		[svg_usr_id]
	)  ON [PRIMARY] 
GO

ALTER TABLE tdSurveyGroupsTsUsers ADD CONSTRAINT
	fkTdSurveyGroupsTsUsersTdSurveyGroups FOREIGN KEY(svg_usr_svg_id)
	REFERENCES tdSurveyGroups(svg_id)
GO
ALTER TABLE tdSurveyGroupsTsUsers ADD CONSTRAINT
	fkTdSurveyGroupsTsUsersTsUsers FOREIGN KEY(svg_usr_usr_id)
	REFERENCES tsUsers(usr_id)
GO