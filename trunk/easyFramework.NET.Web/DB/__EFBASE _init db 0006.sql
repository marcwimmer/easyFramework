CREATE TABLE tsConfig(
cfg_id INT NOT NULL,
cfg_name NVARCHAR(128) NOT NULL,
cfg_value NVARCHAR(2048),
cfg_inserted DATETIME,
cfg_updated DATETIME,
cfg_insertedBy NVARCHAR(255),
cfg_updatedBy NVARCHAR(255)
)
GO
ALTER TABLE tsConfig ADD CONSTRAINT aIdxTsConfig PRIMARY KEY(cfg_id)
GO
