/*
CREATE placeholder Table
*/

CREATE TABLE tdPlaceHolders(
plc_id INT NOT NULL,
plc_svg_id INT NULL, 
plc_svy_id INT NULL,
plc_name NVARCHAR(50),
plc_value NVARCHAR(3400),
plc_inserted DATETIME NOT NULL DEFAULT(getdate()),
plc_insertedBy NVARCHAR(255),
plc_updated DATETIME,
plc_updatedBy NVARCHAR(255)
)
GO
ALTER TABLE tdPlaceHolders ADD CONSTRAINT aIdxTdPlaceHolders PRIMARY KEY(plc_id)
GO
ALTER TABLE tdPlaceHolders ADD CONSTRAINT fkTdPlaceHoldersTdSurveyGroups FOREIGN KEY(plc_svg_id) REFERENCES
	tdSurveyGroups(svg_id)
GO
ALTER TABLE tdPlaceHolders ADD CONSTRAINT fkTdPlaceHoldersTdSurveys FOREIGN KEY(plc_svy_id) REFERENCES
	tdSurveys(svy_id)
GO
