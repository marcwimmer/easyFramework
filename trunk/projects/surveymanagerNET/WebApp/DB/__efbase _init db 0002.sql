/*

	Author: Marc Wimmer
	Copyright: Promain Software-Betreuung GmbH
	Date:	18.05.2004

	Creates the base-Tables for easyFramework

*/
/*------------------------------------------------------
			adapt tsEntityTabs
-----------------------------------------------------------*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.Tmp_tsEntityTabs
	(
	tab_id int NOT NULL IDENTITY (1, 1),
	tab_entity nvarchar(255) NOT NULL,
	tab_tabid nvarchar(50) NOT NULL,
	tab_tabtext nvarchar(50) NOT NULL,
	tab_xmldialogdefinitionfile nvarchar(400) NULL,
	tab_xmldialogdatapage nvarchar(50) NULL,
	tab_url nvarchar(400) NULL,
	tab_index int NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM SysObjects WHERE Name Like 'fktsEntityTabAccessTsUserGroupsEntityTab')
	ALTER TABLE tsEntityTabAccessTsUserGroups DROP CONSTRAINT fktsEntityTabAccessTsUserGroupsEntityTab
GO
IF EXISTS(SELECT * FROM SysObjects WHERE Name Like 'fktsEntityTabAccessTsUsersEntityTabs')
	ALTER TABLE tsEntityTabAccessTsUsers DROP CONSTRAINT fktsEntityTabAccessTsUsersEntityTabs
GO
SET IDENTITY_INSERT dbo.Tmp_tsEntityTabs OFF
GO
IF EXISTS(SELECT * FROM dbo.tsEntityTabs)
	 EXEC('INSERT INTO dbo.Tmp_tsEntityTabs (tab_entity, tab_tabid, tab_tabtext, tab_xmldialogdefinitionfile, tab_xmldialogdatapage, tab_url, tab_index)
		SELECT tab_entity, tab_id, tab_tabtext, tab_xmldialogdefinitionfile, tab_xmldialogdatapage, tab_url, tab_index FROM dbo.tsEntityTabs TABLOCKX')
GO
DROP TABLE dbo.tsEntityTabs
GO
EXECUTE sp_rename N'dbo.Tmp_tsEntityTabs', N'tsEntityTabs', 'OBJECT'
GO
ALTER TABLE dbo.tsEntityTabs ADD CONSTRAINT
	PK_tsEntityTabs PRIMARY KEY CLUSTERED 
	(
	tab_id
	) ON [PRIMARY]

GO
ALTER TABLE dbo.tsEntityTabs ADD CONSTRAINT
	IX_tsEntityTabs UNIQUE NONCLUSTERED 
	(
	tab_entity,
	tab_tabid
	) ON [PRIMARY]

GO
ALTER TABLE dbo.tsEntityTabs WITH NOCHECK ADD CONSTRAINT
	chkTsEntityTabs_UrlXmlDialog CHECK ((((not(((not([tab_xmldialogdefinitionfile] is null))) and ((not([tab_url] is null)))))) and ((not([tab_xmldialogdefinitionfile] is null and [tab_url] is null)))))
GO
COMMIT
GO


/*------------------------------------------------------
			tsMenuAccessTsUsers
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsMenuAccessTsUsers') DROP TABLE tsMenuAccessTsUsers
GO
CREATE TABLE dbo.tsMenuAccessTsUsers
	(
	msu_mnu_id nvarchar(50) NOT NULL,
	msu_usr_id int NOT  NULL,
	msu_explicit_access bit NOT NULL,
	msu_inserted DATETIME NOT NULL,
	msu_insertedby NVARCHAR(255)
	) 
GO
ALTER TABLE tsMenuAccessTsUsers ADD CONSTRAINT dftsMenuAccessTsUsers_msu_inserted DEFAULT(getdate()) FOR msu_inserted
GO
ALTER TABLE dbo.tsMenuAccessTsUsers ADD CONSTRAINT
	aIdxTsMenuAccess PRIMARY KEY CLUSTERED 
	(
	msu_mnu_id, msu_usr_id
	) 
GO
ALTER TABLE dbo.tsMenuAccessTsUsers ADD CONSTRAINT
	fktsMenuAccessTsUsersMainMenue FOREIGN KEY
	(
	msu_mnu_id
	) 
	REFERENCES tsMainMenue(mnu_id)
GO
ALTER TABLE dbo.tsMenuAccessTsUsers ADD CONSTRAINT
	fktsMenuAccessTsUserGroupsUsers FOREIGN KEY
	(
	msu_usr_id
	) 
	REFERENCES tsUsers(usr_id)

GO
/*------------------------------------------------------
			tsMenuAccessTsUserGroups
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsMenuAccessTsUserGroups') DROP TABLE tsMenuAccessTsUserGroups
GO
CREATE TABLE dbo.tsMenuAccessTsUserGroups
	(
	msg_mnu_id nvarchar(50) NOT NULL,
	msg_grp_id int NOT NULL,
	msg_inserted DATETIME NOT NULL DEFAULT(GETDATE()),
	msg_insertedby NVARCHAR(255)
	) 
GO
ALTER TABLE dbo.tsMenuAccessTsUserGroups ADD CONSTRAINT
	aIdxtsMenuAccessTsUserGroups PRIMARY KEY CLUSTERED 
	(
	msg_mnu_id, msg_grp_id
	) 
GO
ALTER TABLE dbo.tsMenuAccessTsUserGroups ADD CONSTRAINT
	fktsMenuAccessTsUserGroupsMainMenue FOREIGN KEY
	(
	msg_mnu_id
	) 
	REFERENCES tsMainMenue(mnu_id)

GO
ALTER TABLE dbo.tsMenuAccessTsUserGroups ADD CONSTRAINT
	fktsMenuAccessTsUserGroupsTsUserGroups FOREIGN KEY
	(
	msg_grp_id
	) 
	REFERENCES tsUserGroups(grp_id)

GO

/*------------------------------------------------------
			tsPolicies
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name = 'fktsPoliciesTsUsersTsUsers')
	ALTER TABLE tsPoliciesTsUsers DROP CONSTRAINT fktsPoliciesTsUsersTsUsers
GO
IF EXISTS(SELECT * FROM sysobjects WHERE Name = 'fktsPoliciesTsUsersTsPolicies')
	ALTER TABLE tsPoliciesTsUsers DROP CONSTRAINT fktsPoliciesTsUsersTsPolicies
GO
IF EXISTS(SELECT * FROM sysobjects WHERE Name = 'fktsPoliciesTsUserGroupsTsUserGroups')
	ALTER TABLE tsPoliciesTsUserGroups DROP CONSTRAINT fktsPoliciesTsUserGroupsTsUserGroups
GO
IF EXISTS(SELECT * FROM sysobjects WHERE Name = 'fktsPoliciesTsUserGroupsTsPolicies')
	ALTER TABLE tsPoliciesTsUserGroups DROP CONSTRAINT fktsPoliciesTsUserGroupsTsPolicies
GO

IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsPolicies') DROP TABLE tsPolicies
GO
CREATE TABLE dbo.tsPolicies
	(
	pol_id int not null IDENTITY(1,1),
	pol_name nvarchar(50) NOT NULL,
	pol_description nvarchar(255) NOT NULL
	) 
GO
ALTER TABLE dbo.tsPolicies ADD CONSTRAINT
	aIdxTsPolicies PRIMARY KEY CLUSTERED 
	(
	pol_id
	) 

GO
/*------------------------------------------------------
			tsPoliciesTsUsers
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsPoliciesTsUsers') DROP TABLE tsPoliciesTsUsers
GO
CREATE TABLE dbo.tsPoliciesTsUsers
	(
	pol_usr_id int NOT NULL IDENTITY (1, 1),
	pol_usr_pol_id int NOT NULL,
	pol_usr_usr_id int NOT NULL,
	pol_usr_explicit_access bit not null,
	pol_usr_inserted datetime not null default(getdate()),
	pol_usr_insertedBy NVARCHAR(255)
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tsPoliciesTsUsers ADD CONSTRAINT
	aIdxtsPoliciesTsUsers PRIMARY KEY CLUSTERED 
	(
	pol_usr_id,
	pol_usr_pol_id,
	pol_usr_usr_id
	) ON [PRIMARY]
GO
ALTER TABLE tsPoliciesTsUsers ADD CONSTRAINT
	fktsPoliciesTsUsersTsUsers FOREIGN KEY(pol_usr_usr_id) REFERENCES tsUsers(usr_id)
GO
ALTER TABLE tsPoliciesTsUsers ADD CONSTRAINT
	fktsPoliciesTsUsersTsPolicies FOREIGN KEY(pol_usr_pol_id) REFERENCES tsPolicies(pol_id)
GO

/*------------------------------------------------------
			tsPoliciesTsUserGroups
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsPoliciesTsUserGroups') DROP TABLE tsPoliciesTsUserGroups
GO
CREATE TABLE dbo.tsPoliciesTsUserGroups
	(
	pol_grp_id int NOT NULL IDENTITY (1, 1),
	pol_grp_pol_id int NOT NULL,
	pol_grp_grp_id int NOT NULL,
	pol_grp_inserted datetime not null default(getdate()),
	pol_grp_insertedBy NVARCHAR(255)
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tsPoliciesTsUserGroups ADD CONSTRAINT
	aIdxtsPoliciesTsUserGroups PRIMARY KEY CLUSTERED 
	(
	pol_grp_id,
	pol_grp_pol_id,
	pol_grp_grp_id
	) ON [PRIMARY]
GO
ALTER TABLE tsPoliciesTsUserGroups ADD CONSTRAINT
	fktsPoliciesTsUserGroupsTsUserGroups FOREIGN KEY(pol_grp_grp_id) REFERENCES tsUserGroups(grp_id)
GO
ALTER TABLE tsPoliciesTsUserGroups ADD CONSTRAINT
	fktsPoliciesTsUserGroupsTsPolicies FOREIGN KEY(pol_grp_pol_id) REFERENCES tsPolicies(pol_id)
GO
/*------------------------------------------------------
			stored procedure spGetNextRecordId
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE name='spGetNextRecordId') DROP PROCEDURE spGetNextRecordId
GO
CREATE PROCEDURE spGetNextRecordId

@TableName NVARCHAR(255)
AS

IF NOT EXISTS(SELECT * FROM tsRecordIds WHERE TableName = @TableName)
INSERT INTO tsRecordIds(TableName, MaxId) VALUES(@TableName, 0)

DECLARE @maxid INT
SET @maxid = (SELECT maxid FROM tsRecordIds WHERE TableName = @TableName)
SET @maxid = 1 + @maxid

UPDATE tsRecordIds SET MaxId = @maxid WHERE TableName = @tablename


RETURN @maxid
GO
/*------------------------------------------------------
			add supervisor-bit-field to users
-----------------------------------------------------------*/
IF NOT EXISTS(SELECT * FROM syscolumns WHERE Name='usr_supervisor')
	ALTER TABLE tsUsers
	ADD usr_supervisor BIT NOT NULL DEFAULT(0)
GO
/*------------------------------------------------------
			tsEntityPopupAccessTsUsers
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsEntityPopupAccessTsUsers') DROP TABLE tsEntityPopupAccessTsUsers
GO
CREATE TABLE dbo.tsEntityPopupAccessTsUsers
	(
	epu_epp_id int NOT NULL,
	epu_usr_id int NOT  NULL,
	epu_explicit_access bit NOT NULL,
	epu_inserted DATETIME NOT NULL DEFAULT(GETDATE()),
	epu_insertedby NVARCHAR(255)
	) 
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUsers ADD CONSTRAINT
	aIdxtsEntityPopupAccess PRIMARY KEY CLUSTERED 
	(
	epu_epp_id, epu_usr_id
	) 
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUsers ADD CONSTRAINT
	fktsEntityPopupAccessTsUsersMainMenue FOREIGN KEY
	(
	epu_epp_id
	) 
	REFERENCES tsEntityPopups(epp_id)
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUsers ADD CONSTRAINT
	fktsEntityPopupAccessTsUserGroupsUsers FOREIGN KEY
	(
	epu_usr_id
	) 
	REFERENCES tsUsers(usr_id)

GO
/*------------------------------------------------------
			tsEntityPopupAccessTsUserGroups
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsEntityPopupAccessTsUserGroups') DROP TABLE tsEntityPopupAccessTsUserGroups
GO
CREATE TABLE dbo.tsEntityPopupAccessTsUserGroups
	(
	epg_epp_id int NOT NULL,
	epg_grp_id int NOT NULL,
	epg_inserted DATETIME NOT NULL DEFAULT(GETDATE()),
	epg_insertedby NVARCHAR(255)
	) 
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUserGroups ADD CONSTRAINT
	aIdxtsEntityPopupAccessTsUserGroups PRIMARY KEY CLUSTERED 
	(
	epg_epp_id, epg_grp_id
	) 
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUserGroups ADD CONSTRAINT
	fktsEntityPopupAccessTsUserGroupsMainMenue FOREIGN KEY
	(
	epg_epp_id
	) 
	REFERENCES tsEntityPopups(epp_id)

GO
ALTER TABLE dbo.tsEntityPopupAccessTsUserGroups ADD CONSTRAINT
	fkTsEntityPopupAccessTsUserGroupsTsUserGroups FOREIGN KEY
	(
	epg_grp_id
	) 
	REFERENCES tsUserGroups(grp_id)

GO
/*------------------------------------------------------
			tsEntityTabAccessTsUsers
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsEntityTabAccessTsUsers') DROP TABLE tsEntityTabAccessTsUsers
GO
CREATE TABLE dbo.tsEntityTabAccessTsUsers
	(
	etu_tab_id int NOT NULL,
	etu_usr_id int NOT  NULL,
	etu_explicit_access bit NOT NULL,
	etu_inserted DATETIME NOT NULL DEFAULT(GETDATE()),
	etu_insertedby NVARCHAR(255)
	) 
GO
ALTER TABLE dbo.tsEntityTabAccessTsUsers ADD CONSTRAINT
	aIdxtsEntityTabAccessTsUsers PRIMARY KEY CLUSTERED 
	(
	etu_tab_id, etu_usr_id
	) 
GO
ALTER TABLE dbo.tsEntityTabAccessTsUsers ADD CONSTRAINT
	fktsEntityTabAccessTsUsersEntityTabs FOREIGN KEY
	(
	etu_tab_id
	) 
	REFERENCES tsEntityTabs(tab_id)
GO
ALTER TABLE dbo.tsEntityTabAccessTsUsers ADD CONSTRAINT
	fktsEntityTabAccessTsUserTsUsers FOREIGN KEY
	(
	etu_usr_id
	) 
	REFERENCES tsUsers(usr_id)

GO
/*------------------------------------------------------
			tsEntityTabAccessTsUserGroups
-----------------------------------------------------------*/
IF EXISTS(SELECT * FROM sysobjects WHERE Name='tsEntityTabAccessTsUserGroups') DROP TABLE tsEntityTabAccessTsUserGroups
GO
CREATE TABLE dbo.tsEntityTabAccessTsUserGroups
	(
	etg_tab_id int NOT NULL,
	etg_grp_id int NOT NULL,
	etg_inserted DATETIME NOT NULL DEFAULT(GETDATE()),
	etg_insertedby NVARCHAR(255)
	) 
GO
ALTER TABLE dbo.tsEntityTabAccessTsUserGroups ADD CONSTRAINT
	aIdxtsEntityTabAccessTsUserGroups PRIMARY KEY CLUSTERED 
	(
	etg_tab_id, etg_grp_id
	) 
GO
ALTER TABLE dbo.tsEntityTabAccessTsUserGroups ADD CONSTRAINT
	fktsEntityTabAccessTsUserGroupsEntityTab FOREIGN KEY
	(
	etg_tab_id
	) 
	REFERENCES tsEntityTabs(tab_id)

GO
ALTER TABLE dbo.tsEntityTabAccessTsUserGroups ADD CONSTRAINT
	fktsEntityTabAccessTsUserGroupsTsUserGroups FOREIGN KEY
	(
	etg_grp_id
	) 
	REFERENCES tsUserGroups(grp_id)

GO