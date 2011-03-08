CREATE TABLE [dbo].[tsAbonnents] (
	[abo_id] [int] NOT NULL ,
	[abo_name] [nvarchar] (128) NOT NULL ,
	[abo_host] [nvarchar] (256) NULL ,
	[abo_username] [nvarchar] (128) NULL ,
	[abo_password] [nvarchar] (128) NULL ,
	[abo_inserted] [datetime] NULL ,
	[abo_updated] [datetime] NULL ,
	[abo_insertedBy] [nvarchar] (255) NULL ,
	[abo_updatedBy] [nvarchar] (255) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tsAbonnentsPackages] (
	[dst_id] [int] NOT NULL ,
	[dst_packagename] [nvarchar] (255) NULL ,
	[dst_inserted] [datetime] NULL ,
	[dst_updated] [datetime] NULL ,
	[dst_insertedBy] [nvarchar] (50) NULL ,
	[dst_updatedBy] [nvarchar] (50) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tsAbonnentsPackagesLogs] (
	[dstlog_id] [int] NOT NULL ,
	[dstlog_dst_id] [int] NULL ,
	[dstlog_infotext] [nvarchar] (512) NULL ,
	[dstlog_succeeded] [bit] NULL ,
	[dstlog_abo_id] [int] NULL ,
	[dstlog_abo_name] [nvarchar] (255) NULL ,
	[dstlog_host] [nvarchar] (255) NULL ,
	[dstlog_inserted] [datetime] NULL ,
	[dstlog_updated] [datetime] NULL ,
	[dstlog_insertedBy] [nvarchar] (255) NULL ,
	[dstlog_updatedBy] [nvarchar] (255) NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tsAbonnents] WITH NOCHECK ADD 
	CONSTRAINT [aIdxTsAbonnents] PRIMARY KEY  CLUSTERED 
	(
		[abo_id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tsAbonnentsPackages] WITH NOCHECK ADD 
	CONSTRAINT [aIdxtsAbonnentsPackages] PRIMARY KEY  CLUSTERED 
	(
		[dst_id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tsAbonnentsPackagesLogs] WITH NOCHECK ADD 
	CONSTRAINT [aIdxtsAbonnentsPackagesLogs] PRIMARY KEY  CLUSTERED 
	(
		[dstlog_id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tsAbonnentsPackagesLogs] ADD 
	CONSTRAINT [zIdxtsAbonnentsPackagesLogs_tsAbonnents] FOREIGN KEY 
	(
		[dstlog_abo_id]
	) REFERENCES [dbo].[tsAbonnents] (
		[abo_id]
	),
	CONSTRAINT [zIdxtsAbonnentsPackagesLogs_tsAbonnentsPackages] FOREIGN KEY 
	(
		[dstlog_dst_id]
	) REFERENCES [dbo].[tsAbonnentsPackages] (
		[dst_id]
	)
GO

