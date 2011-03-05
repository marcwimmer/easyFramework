/*

	Author: Marc Wimmer
	Copyright: Promain Software-Betreuung GmbH
	Date:	21.08.2004

	Creates the base-Tables for Adoptionlists

*/
-------------- Surveygroups --------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[tdAdoptions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [tdAdoptions]
GO
CREATE TABLE tdAdoptions(
adp_id INT NOT NULL,
adp_responsible_user_id NVARCHAR(255),
adp_plz NVARCHAR(50),
adp_ort NVARCHAR(64),
adp_dozent NVARCHAR(128), 
adp_autor_titel NVARCHAR(128), 
adp_isbn NVARCHAR(64),
adp_anzahl_studenten INT,
adp_name_vorlesung NVARCHAR(128),
adp_price_vista_excl_vat MONEY,
adp_bemerkung NVARCHAR(2048), 
adp_buchhandlung NVARCHAR(128),
adp_verkaufte_exemplare INT,
adp_inserted DATETIME,
adp_updated DATETIME,
adp_insertedby NVARCHAR(255),
adp_updatedby NVARCHAR(255)
)
GO
ALTER TABLE tdAdoptions ADD CONSTRAINT aIdxTdAdoptions PRIMARY KEY(adp_id)
GO
