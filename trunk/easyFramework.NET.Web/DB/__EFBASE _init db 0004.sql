/*-----------------------------
DOMAIN-CONCEPT
------------------------------*/
CREATE TABLE tsDomains(dom_id INT IDENTITY(1,1) NOT NULL,
	dom_name NVARCHAR(255) NOT NULL, 
	dom_description NVARCHAR(2048),
	dom_inserted DATETIME,
	dom_insertedBy NVARCHAR(255),
	dom_updated DATETIME,
	dom_updatedBy NVARCHAR(255)
)
GO
ALTER TABLE tsDomains ADD CONSTRAINT aIdxTsDomains PRIMARY KEY(dom_id)
GO
ALTER TABLE tsDomains ADD CONSTRAINT dfTsDomains_dom_inserted DEFAULT(getdate()) FOR dom_inserted
ALTER TABLE tsDomains ADD CONSTRAINT dfTsDomains_dom_updated DEFAULT(getdate()) FOR dom_updated
GO
CREATE TABLE tsDomainValues(
	dvl_id INT IDENTITY(1,1) NOT NULL,
	dvl_dom_id INT NOT NULL,
	dvl_internalvalue NVARCHAR(255) NOT NULL, 
	dvl_caption NVARCHAR(255) NOT NULL,
	dvl_inserted DATETIME,
	dvl_insertedBy NVARCHAR(255),
	dvl_updated DATETIME,
	dvl_updatedBy NVARCHAR(255)
)
GO
ALTER TABLE tsDomainValues ADD CONSTRAINT aIdxTsDomainValues PRIMARY KEY(dvl_id)
GO
ALTER TABLE tsDomainValues ADD CONSTRAINT dfTsDomainValues_dom_inserted DEFAULT(getdate()) FOR dvl_inserted
ALTER TABLE tsDomainValues ADD CONSTRAINT dfTsDomainValues_dom_updated DEFAULT(getdate()) FOR dvl_updated
GO
ALTER TABLE tsDomainValues ADD CONSTRAINT fkTsDomainValuesTsDomains FOREIGN KEY(dvl_dom_id) REFERENCES
	tsDomains(dom_id)
GO

/*-----------------------------
EXTENDED HTML-EDITOR
------------------------------*/
ALTER TABLE tsEntityMemos 
ADD eme_extendedHtmlEditor BIT NOT NULL DEFAULT(1)
GO
ALTER TABLE tsUsers
ADD usr_not_extendedHtmlEditor BIT NOT NULL  DEFAULT(0)
GO

