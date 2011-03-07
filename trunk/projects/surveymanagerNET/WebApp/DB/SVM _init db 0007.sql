SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER  procedure spGetAllowedSurveyGroups 
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

--the access to all parent-nodes must exist; otherwise the user cannot access the survey-group:
DECLARE C CURSOR FOR SELECT Allowed FROM #allowedGroups
OPEN C
FETCH NEXT FROM C INTO @svg_id
WHILE @@FETCH_STATUS = 0
BEGIN

	INSERT INTO #allowedGroups(Allowed)
	SELECT svg_parentid FROM tdSurveyGroups WHERE svg_id = @svg_id

	FETCH NEXT FROM C INTO @svg_id
	
END
CLOSE C
DEALLOCATE C


select * from #allowedGroups



drop table #allowedGroups

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


