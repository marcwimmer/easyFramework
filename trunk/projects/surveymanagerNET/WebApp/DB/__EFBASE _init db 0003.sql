/*------------make entity-popup identity-field--------------*/

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
GO
BEGIN TRANSACTION
ALTER TABLE dbo.tsEntityPopups
	DROP CONSTRAINT fkTsEntityPopupsTsEntities
GO
COMMIT
GO
BEGIN TRANSACTION
GO
ALTER TABLE dbo.tsEntityPopups
	DROP CONSTRAINT dfTsEntityPopups_epp_index
GO
CREATE TABLE dbo.Tmp_tsEntityPopups
	(
	epp_id int NOT NULL IDENTITY (1, 1),
	epp_ety_name nvarchar(255) NOT NULL,
	epp_caption nvarchar(255) NULL,
	epp_url nvarchar(512) NULL,
	epp_index int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tsEntityPopups ADD CONSTRAINT
	dfTsEntityPopups_epp_index DEFAULT (0) FOR epp_index
GO
SET IDENTITY_INSERT dbo.Tmp_tsEntityPopups ON
GO
IF EXISTS(SELECT * FROM dbo.tsEntityPopups)
	 EXEC('INSERT INTO dbo.Tmp_tsEntityPopups (epp_id, epp_ety_name, epp_caption, epp_url, epp_index)
		SELECT epp_id, epp_ety_name, epp_caption, epp_url, epp_index FROM dbo.tsEntityPopups TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tsEntityPopups OFF
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUsers
	DROP CONSTRAINT fktsEntityPopupAccessTsUsersMainMenue
GO
ALTER TABLE dbo.tsEntityPopupAccessTsUserGroups
	DROP CONSTRAINT fktsEntityPopupAccessTsUserGroupsMainMenue
GO
DROP TABLE dbo.tsEntityPopups
GO
EXECUTE sp_rename N'dbo.Tmp_tsEntityPopups', N'tsEntityPopups', 'OBJECT'
GO
ALTER TABLE dbo.tsEntityPopups ADD CONSTRAINT
	aidxTsEntityPopups PRIMARY KEY CLUSTERED 
	(
	epp_id
	) ON [PRIMARY]

GO
ALTER TABLE dbo.tsEntityPopups WITH NOCHECK ADD CONSTRAINT
	fkTsEntityPopupsTsEntities FOREIGN KEY
	(
	epp_ety_name
	) REFERENCES dbo.tsEntities
	(
	ety_name
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tsEntityPopupAccessTsUserGroups WITH NOCHECK ADD CONSTRAINT
	fktsEntityPopupAccessTsUserGroupsMainMenue FOREIGN KEY
	(
	epg_epp_id
	) REFERENCES dbo.tsEntityPopups
	(
	epp_id
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tsEntityPopupAccessTsUsers WITH NOCHECK ADD CONSTRAINT
	fktsEntityPopupAccessTsUsersMainMenue FOREIGN KEY
	(
	epu_epp_id
	) REFERENCES dbo.tsEntityPopups
	(
	epp_id
	)
GO
COMMIT

GO
