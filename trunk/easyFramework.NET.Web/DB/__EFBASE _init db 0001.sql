/*

	Author: Marc Wimmer
	Copyright: Promain Software-Betreuung GmbH
	Date:	21.03.2004

	Creates the base-Tables for easyFramework

*/

SET NOCOUNT ON
GO
/*===========================================================
tsRecordIDs
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsRecordIDs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsRecordIDs]
GO
CREATE TABLE tsRecordIDs(
TableName NVARCHAR(255) NOT NULL,
MaxID INT NOT NULL 
)
GO
ALTER TABLE tsRecordIDs
ADD CONSTRAINT aIdxtsRecordIDs PRIMARY KEY (TableName)
GO
ALTER TABLE tsRecordIDs
ADD CONSTRAINT dftsRecordIDs_MaxID DEFAULT(0) FOR MaxID
GO

/*===========================================================
tsUsers
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[fkTsUsersTsUserGroups_TsUsers]'))
ALTER TABLE tsUsersTsUserGroups DROP CONSTRAINT fkTsUsersTsUserGroups_TsUsers
if exists (select * from dbo.sysobjects where id = object_id(N'[fkTsClientInfoTsUsers]'))
ALTER TABLE tsClientInfo DROP CONSTRAINT fkTsClientInfoTsUsers
if exists (select * from dbo.sysobjects where id = object_id(N'[tsUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsUsers]
GO
CREATE TABLE tsUsers(
usr_id INT NOT NULL,
usr_login NVARCHAR(25) NOT NULL,
usr_password NVARCHAR(50) NOT NULL, 
usr_firstname NVARCHAR(80), 
usr_lastname NVARCHAR(80), 
usr_inserted DATETIME NOT NULL,
usr_updated DATETIME NOT NULL,
usr_insertedBy NVARCHAR(255),
usr_updatedBy NVARCHAR(255),
usr_infotext NTEXT
)
GO
ALTER TABLE tsUsers
ADD CONSTRAINT aIdxtsUsers PRIMARY KEY (usr_id)
GO
ALTER TABLE tsUsers ADD CONSTRAINT dftsUsers_usr_inserted DEFAULT(getdate()) FOR usr_inserted
GO
ALTER TABLE tsUsers ADD CONSTRAINT dftsUsers_usr_updated DEFAULT(getdate()) FOR usr_updated
GO
INSERT INTO tsUsers(usr_id, usr_login, usr_password, usr_firstname, usr_lastname)
VALUES(1, 'sa', '', 'System','Admin')
DELETE FROM tsRecordIDs WHERE TableName = 'tsUsers'
INSERT INTO tsRecordIDs(TableName, MaxID) VALUES('tsUsers', 1)
GO
/*===========================================================
tsUserGroups
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[fkTsUsersTsUserGroups_TsUserGroups]'))
ALTER TABLE TsUsersTsUserGroups DROP CONSTRAINT fkTsUsersTsUserGroups_TsUserGroups
if exists (select * from dbo.sysobjects where id = object_id(N'[tsUserGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsUserGroups]
GO
CREATE TABLE tsUserGroups(
grp_id INT NOT NULL,
grp_name NVARCHAR(25) NOT NULL,
grp_inserted DATETIME NOT NULL,
grp_updated DATETIME NOT NULL,
grp_insertedBy NVARCHAR(255),
grp_updatedBy NVARCHAR(255))
GO
ALTER TABLE tsUserGroups
ADD CONSTRAINT aIdxtsUserGroups PRIMARY KEY (grp_id)
GO
ALTER TABLE tsUserGroups ADD CONSTRAINT dftsUserGroups_grp_inserted DEFAULT(getdate()) FOR grp_inserted
GO
ALTER TABLE tsUserGroups ADD CONSTRAINT dftsUserGroups_grp_updated DEFAULT(getdate()) FOR grp_updated
GO
INSERT INTO tsUserGroups(grp_id, grp_name) VALUES(1, 'Administratoren')
GO
DELETE FROM tsRecordIDs WHERE TableName = 'tsUserGroups'
INSERT INTO tsRecordIDs(TableName, MaxID) VALUES('tsUserGroups', 1)
GO
/*===========================================================
tsUsersTsUserGroups
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsUsersTsUserGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsUsersTsUserGroups]
GO
CREATE TABLE tsUsersTsUserGroups(
usr_grp_id INT NOT NULL,
usr_grp_usr_id INT NOT NULL,
usr_grp_grp_id INT NOT NULL,
usr_grp_inserted DATETIME NOT NULL,
usr_grp_updated DATETIME NOT NULL,
usr_grp_insertedBy NVARCHAR(255),
usr_grp_updatedBy NVARCHAR(255))
GO
ALTER TABLE tsUsersTsUserGroups ADD CONSTRAINT dftsUsersTsUserGroups_usr_grp_inserted DEFAULT(getdate()) FOR usr_grp_inserted
GO
ALTER TABLE tsUsersTsUserGroups ADD CONSTRAINT dftsUsersTsUserGroups_usr_grp_updated DEFAULT(getdate()) FOR usr_grp_updated
GO
ALTER TABLE tsUsersTsUserGroups
ADD CONSTRAINT aIdxtsUsersTsUserGroups PRIMARY KEY (usr_grp_id)
GO
INSERT INTO tsUsersTsUserGroups(usr_grp_id, usr_grp_usr_id, usr_grp_grp_id) VALUES(1, 1, 1)
GO
DELETE FROM tsRecordIDs WHERE TableName = 'tsUsersTsUserGroups'
INSERT INTO tsRecordIDs(TableName, MaxID) VALUES('tsUsersTsUserGroups', 1)
GO
ALTER TABLE tsUsersTsUserGroups
ADD CONSTRAINT fkTsUsersTsUserGroups_TsUsers FOREIGN KEY(usr_grp_usr_id) REFERENCES tsUsers(usr_id)
GO
ALTER TABLE tsUsersTsUserGroups
ADD CONSTRAINT fkTsUsersTsUserGroups_TsUserGroups FOREIGN KEY(usr_grp_grp_id) REFERENCES tsUserGroups(grp_id)
GO
/*===========================================================
tsClientInfo
=============================================================*/
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tsClientInfo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsClientInfo]
GO
CREATE TABLE tsClientInfo(
ci_id NVARCHAR(40) NOT NULL,
ci_content NTEXT,
ci_inserted DATETIME,
ci_updated DATETIME, 
ci_loggedin BIT NOT NULL,
ci_loggedin_usr INT
)
GO
ALTER TABLE tsClientInfo
ADD CONSTRAINT aIdxTsClientInfo PRIMARY KEY (ci_id)
GO
ALTER TABLE tsClientInfo
ADD CONSTRAINT fkTsClientInfoTsUsers FOREIGN KEY(ci_loggedin_usr) REFERENCES tsUsers(usr_id)
GO
ALTER TABLE tsClientInfo ADD CONSTRAINT dfTsClientInfo_ci_loggedin DEFAULT(0) FOR ci_loggedin
GO

/*===========================================================
tdDictionary
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tdDictionary]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdDictionary]
GO
CREATE TABLE tdDictionary(dic_id INT NOT NULL,
dic_systemmessage NVARCHAR(255), 
dic_translateto NVARCHAR(255),
dic_language NVARCHAR(25))
GO
ALTER TABLE tdDictionary
ADD CONSTRAINT aIdxtdDictionary PRIMARY KEY (dic_id)

/*===========================================================
tsLog
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsLog]
GO
CREATE TABLE tsLog(log_id INT NOT NULL IDENTITY(1,1),
log_message NVARCHAR(3000), 
log_date DATETIME NOT NULL,
log_type NVARCHAR(25), 
log_clientid NVARCHAR(50),
log_username NVARCHAR(255)
)
GO
ALTER TABLE tsLog ADD CONSTRAINT dfTsLog_log_date DEFAULT(getdate()) FOR log_date
GO
ALTER TABLE tsLog
ADD CONSTRAINT aIdxtsLog PRIMARY KEY (log_id)
GO
ALTER TABLE tsLog
ADD CONSTRAINT chk_log_type CHECK (log_type IN ('Error', 'Warning', 'Info', 'Debug'))
GO


/*===========================================================
tsDataCombos
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsDataCombo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsDataCombo]
GO
CREATE TABLE tsDataCombo(
dc_name NVARCHAR(50) NOT NULL,
dc_qry NVARCHAR(500),
Project	NVARCHAR(10)
)
GO
ALTER TABLE tsDataCombo ADD CONSTRAINT aidxTsDataCombo PRIMARY KEY (dc_name)
GO
/*===========================================================
tsIcons - for images used on the asp-pages
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[fkTsEntitiesTsIcons]'))
ALTER TABLE tsEntities DROP CONSTRAINT fkTsEntitiesTsIcons
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[fktsMainMenuetsIconOpened]'))
ALTER TABLE tsMainMenue DROP CONSTRAINT fktsMainMenuetsIconOpened
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[fktsMainMenuetsIconNormal]'))
ALTER TABLE tsMainMenue DROP CONSTRAINT fktsMainMenuetsIconNormal
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[fkTsEntitiesTsIcons]'))
ALTER TABLE tsEntities DROP CONSTRAINT fkTsEntitiesTsIcons
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tsIcons]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsIcons]
GO
CREATE TABLE tsIcons(
icon_id NVARCHAR(255) NOT NULL,
icon_relativepath NVARCHAR(255),
icon_filename NVARCHAR(255),
icon_height INT,
icon_width INT,
Project	NVARCHAR(10)
)
GO
ALTER TABLE tsIcons
ADD CONSTRAINT aIdxtsIcons PRIMARY KEY (icon_id)

GO

/*===========================================================
tsMainMenue
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsMainMenue]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsMainMenue]
GO
CREATE TABLE tsMainMenue(
mnu_id NVARCHAR(50) NOT NULL,
mnu_parentid NVARCHAR(50) NULL,
mnu_title NVARCHAR(255) NOT NULL,
mnu_command NVARCHAR(400) NOT NULL, 
mnu_isFolder BIT NOT NULL,
mnu_modalwindow BIT NOT NULL, 
mnu_edition NVARCHAR(80) NOT NULL,
mnu_icon_normal NVARCHAR(255) NOT NULL, 
mnu_icon_opened NVARCHAR(255) NOT NULL, 
mnu_index INT NULL,
Project	NVARCHAR(10))
GO
ALTER TABLE tsMainmenue ADD CONSTRAINT dfTsMainMenue_mnu_modalwindow DEFAULT(0) FOR mnu_modalwindow
GO
ALTER TABLE tsMainmenue ADD CONSTRAINT dfTsMainMenue_mnu_edition  DEFAULT('*') FOR mnu_edition
GO
ALTER TABLE tsMainMenue
ADD CONSTRAINT aIdxtsMainMenue PRIMARY KEY (mnu_id)
GO
ALTER TABLE tsMainMenue
ADD CONSTRAINT fktsMainMenuetsIconNormal FOREIGN KEY (mnu_icon_normal) REFERENCES tsIcons(icon_id)
GO
ALTER TABLE tsMainMenue
ADD CONSTRAINT fktsMainMenuetsIconOpened FOREIGN KEY (mnu_icon_opened) REFERENCES tsIcons(icon_id)
GO
ALTER TABLE tsMainMenue
ADD CONSTRAINT fktsMainMenuetsMainMenue FOREIGN KEY (mnu_parentid) REFERENCES tsMainMenue(mnu_id)
GO
/*===========================================================
tsEntities
=============================================================*/
if exists(select * From sysobjects where name = 'fkTsEntityPopupsTsEntities')
	alter table tsEntityPopups drop constraint fkTsEntityPopupsTsEntities
GO
if exists(select * From sysobjects where name = 'fkTsEntityMemosTsEntities')
	alter table tsEntityMemos drop constraint fkTsEntityMemosTsEntities
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[tsEntities]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsEntities]
GO
CREATE TABLE tsEntities(
ety_name NVARCHAR(255) NOT NULL,
ety_title NVARCHAR(255) NOT NULL,
ety_searchdialog NVARCHAR(255) NULL,
ety_editdialog NVARCHAR(400) NOT NULL,
ety_editdialogxml NVARCHAR(400) NOT NULL,
ety_tableDefXml NVARCHAR(400) NOT NULL,
ety_windowwidth INT NOT NULL, 
ety_windowheight INT NOT NULL, 
ety_searchfields NVARCHAR(255) NOT NULL,
ety_primarySearchField NVARCHAR(255) NOT NULL,
ety_icon NVARCHAR(255) NULL, 
ety_tostringFields NVARCHAR(255) NOT NULL,
ety_concurrentUpdates BIT  NOT NULL,
ety_cols NVARCHAR(255) NOT NULL,
ety_colcaptions NVARCHAR(255) NOT NULL,
ety_colwidths NVARCHAR(255) NOT NULL

)
GO
ALTER TABLE tsEntities ADD CONSTRAINT dfTsEntities_ety_windowwidth DEFAULT(640) FOR ety_windowwidth
GO
ALTER TABLE tsEntities ADD CONSTRAINT dfTsEntities_ety_windowheight DEFAULT(480) FOR ety_windowheight
GO
ALTER TABLE tsEntities ADD CONSTRAINT dfTsEntities_ety_concurrentUpdates DEFAULT(1) FOR ety_concurrentUpdates
GO
ALTER TABLE tsEntities ADD CONSTRAINT aidxTsEntities PRIMARY KEY(ety_name)
GO
ALTER TABLE tsEntities ADD CONSTRAINT fkTsEntitiesTsIcons FOREIGN KEY (ety_icon) REFERENCES tsIcons(icon_id)
GO
/*===========================================================
tsEntityPopups - (Querverweise einer Entität)
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsEntityPopups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsEntityPopups]
GO
CREATE TABLE tsEntityPopups (
epp_id INT NOT NULL,
epp_ety_name NVARCHAR(255) NOT NULL,
epp_caption NVARCHAR(255), 
epp_url NVARCHAR(512),
epp_index INT NOT NULL
)
GO
ALTER TABLE tsEntityPopups ADD CONSTRAINT aidxTsEntityPopups PRIMARY KEY(epp_id)
GO
ALTER TABLE tsEntityPopups ADD CONSTRAINT fkTsEntityPopupsTsEntities FOREIGN KEY (epp_ety_name) REFERENCES tsEntities(ety_name)
GO
ALTER TABLE tsEntityPopups ADD CONSTRAINT dfTsEntityPopups_epp_index DEFAULT(0) FOR epp_index
GO
/*===========================================================
tsEntityMemos - (Memotexte/Felder einer Entität)
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsEntityMemos]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsEntityMemos]
GO
CREATE TABLE tsEntityMemos (
eme_id INT NOT NULL,
eme_caption NVARCHAR(255) NOT NULL,
eme_ety_name NVARCHAR(255) NOT NULL,
eme_fieldname NVARCHAR(255), 
eme_index INT NOT NULL
)
GO
ALTER TABLE tsEntityMemos ADD CONSTRAINT dfTsEntityMemos_eme_index DEFAULT(0) FOR eme_index
GO
ALTER TABLE tsEntityMemos ADD CONSTRAINT aidxTsEntityMemos PRIMARY KEY(eme_id)
GO
ALTER TABLE tsEntityMemos ADD CONSTRAINT fkTsEntityMemosTsEntities FOREIGN KEY (eme_ety_name) REFERENCES tsEntities(ety_name)
GO


/*===========================================================
tsSysEvents
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsSysEvents]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsSysEvents]
GO
CREATE TABLE tsSysEvents (
sys_id INT IDENTITY(1,1) NOT NULL,
sys_category NVARCHAR(255) NOT NULL,
sys_name NVARCHAR(255) NOT NULL,
sys_actiontype NVARCHAR(15),
sys_index INT NOT NULL,
sys_task NVARCHAR(255) NOT NULL,
sys_assembly NVARCHAR(255)
)
GO
ALTER TABLE tsSysEvents ADD CONSTRAINT dfTsSysEvents_sys_index DEFAULT(100) FOR sys_index
GO
ALTER TABLE tsSysEvents ADD CONSTRAINT dfTsSysEvents_sys_task DEFAULT(0) FOR sys_task
GO
ALTER TABLE tsSysEvents ADD CONSTRAINT aidxTsSysEvents PRIMARY KEY(sys_id)
GO
ALTER TABLE tsSysEvents ADD CONSTRAINT chkTsSysEventsActionType CHECK (sys_actiontype IN ('SQL','CLASS'))
GO


/*===========================================================
tsEntityTabs
=============================================================*/
if exists (select * from dbo.sysobjects where id = object_id(N'[tsEntityTabs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tsEntityTabs]
GO
CREATE TABLE [dbo].[tsEntityTabs] (
	[tab_entity] [nvarchar] (255)  NOT NULL ,
	[tab_id] [nvarchar] (50) NOT   NULL ,
	[tab_tabtext] [nvarchar] (50) NOT  NULL ,
	[tab_xmldialogdefinitionfile] [nvarchar] (400)  NULL ,
	[tab_xmldialogdatapage] [nvarchar] (50)  NULL ,
	[tab_url] [nvarchar] (400)  NULL ,
	[tab_index] [int] NULL 
) ON [PRIMARY]
GO

GO
ALTER TABLE tsEntityTabs ADD CONSTRAINT aidxTsEntityTabss PRIMARY KEY(tab_entity, tab_id)
GO
ALTER TABLE tsEntityTabs ADD CONSTRAINT chkTsEntityTabs_UrlXmlDialog CHECK (not (not tab_xmldialogdefinitionfile is null and not tab_url is null)
	and not (tab_xmldialogdefinitionfile is null and tab_url is null))
GO