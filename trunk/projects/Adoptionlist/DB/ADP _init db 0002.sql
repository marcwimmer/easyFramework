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
CREATE TABLE dbo.Tmp_tdAdoptions
	(
	adp_id int NOT NULL,
	adp_responsible_user_id int NULL,
	adp_plz nvarchar(50) NULL,
	adp_ort nvarchar(64) NULL,
	adp_dozent nvarchar(128) NULL,
	adp_autor_titel nvarchar(128) NULL,
	adp_isbn nvarchar(64) NULL,
	adp_anzahl_studenten int NULL,
	adp_name_vorlesung nvarchar(128) NULL,
	adp_price_vista_excl_vat money NULL,
	adp_bemerkung nvarchar(2048) NULL,
	adp_buchhandlung nvarchar(128) NULL,
	adp_verkaufte_exemplare int NULL,
	adp_inserted datetime NULL,
	adp_updated datetime NULL,
	adp_insertedby nvarchar(255) NULL,
	adp_updatedby nvarchar(255) NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.tdAdoptions)
	 EXEC('INSERT INTO dbo.Tmp_tdAdoptions (adp_id, adp_responsible_user_id, adp_plz, adp_ort, adp_dozent, adp_autor_titel, adp_isbn, adp_anzahl_studenten, adp_name_vorlesung, adp_price_vista_excl_vat, adp_bemerkung, adp_buchhandlung, adp_verkaufte_exemplare, adp_inserted, adp_updated, adp_insertedby, adp_updatedby)
		SELECT adp_id, CONVERT(int, adp_responsible_user_id), adp_plz, adp_ort, adp_dozent, adp_autor_titel, adp_isbn, adp_anzahl_studenten, adp_name_vorlesung, adp_price_vista_excl_vat, adp_bemerkung, adp_buchhandlung, adp_verkaufte_exemplare, adp_inserted, adp_updated, adp_insertedby, adp_updatedby FROM dbo.tdAdoptions TABLOCKX')
GO
DROP TABLE dbo.tdAdoptions
GO
EXECUTE sp_rename N'dbo.Tmp_tdAdoptions', N'tdAdoptions', 'OBJECT'
GO
ALTER TABLE dbo.tdAdoptions ADD CONSTRAINT
	aIdxTdAdoptions PRIMARY KEY CLUSTERED 
	(
	adp_id
	) ON [PRIMARY]

GO
COMMIT
GO

ALTER TABLE tdAdoptions
ADD adp_land nvarchar(64)
GO



