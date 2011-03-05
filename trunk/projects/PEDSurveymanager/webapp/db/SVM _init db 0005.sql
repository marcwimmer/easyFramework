/*------------------------------------------------------------------
einfügen slots
------------------------------------------------------------------*/
CREATE TABLE tdSlots
(
slt_id INT NOT NULL,
slt_name NVARCHAR(255),
slt_assigned_svy_id INT, 
slt_externalid NVARCHAR(50),
slt_inserted DATETIME,
slt_insertedBy NVARCHAR(255),
slt_updated DATETIME,
slt_updatedBy NVARCHAR(255)
)
GO
ALTER TABLE tdSlots ADD CONSTRAINT aIdxTdSlots PRIMARY KEY (slt_id)
GO
ALTER TABLE tdSlots ADD CONSTRAINT fkTdSlotsTdSurveys FOREIGN KEY (slt_assigned_svy_id)
	REFERENCES tdSurveys(svy_id)
GO

