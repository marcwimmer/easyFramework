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
CREATE TABLE tsAbonnents(
abo_id INT NOT NULL,
abo_name NVARCHAR(128) NOT NULL,
abo_host NVARCHAR(256),
abo_port NVARCHAR(128),
abo_username NVARCHAR(128),
abo_password NVARCHAR(128),
abo_inserted DATETIME,
abo_updated DATETIME,
abo_insertedBy NVARCHAR(255),
abo_updatedBy NVARCHAR(255)
)
GO
ALTER TABLE tsAbonnents ADD CONSTRAINT aIdxTsAbonnents PRIMARY KEY(abo_id)
GO
