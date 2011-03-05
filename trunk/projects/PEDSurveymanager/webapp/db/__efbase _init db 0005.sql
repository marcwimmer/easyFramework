if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fktsHelpLinks_tsHelpChapters]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[tsHelpLinks] DROP CONSTRAINT fktsHelpLinks_tsHelpChapters
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fktsHelpChapters_tsHelpToc]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[tsHelpChapters] DROP CONSTRAINT fktsHelpChapters_tsHelpToc
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tsHelpChapters]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tsHelpChapters]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tsHelpLinks]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tsHelpLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tsHelpToc]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tsHelpToc]
GO

CREATE TABLE [dbo].[tsHelpChapters] (
	[hlp_id] [uniqueidentifier] NOT NULL ,
	[hlp_parentid] [uniqueidentifier] NULL ,
	[hlp_toc_id] [uniqueidentifier] NULL ,
	[hlp_heading] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL ,
	[hlp_body] [ntext] COLLATE Latin1_General_CI_AS NULL ,
	[hlp_inserted] [datetime] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tsHelpLinks] (
	[lnk_id] [uniqueidentifier] NOT NULL ,
	[lnk_hlp_id] [uniqueidentifier] NOT NULL ,
	[lnk_entity] [nvarchar] (50) COLLATE Latin1_General_CI_AS NULL ,
	[lnk_url] [nvarchar] (400) COLLATE Latin1_General_CI_AS NULL ,
	[lnk_inserted] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tsHelpToc] (
	[toc_id] [uniqueidentifier] NOT NULL ,
	[toc_parentid] [uniqueidentifier] NULL ,
	[toc_title] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL ,
	[toc_index] [int] IDENTITY (1, 1) NOT NULL ,
	[toc_inserted] [datetime] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tsHelpChapters] WITH NOCHECK ADD 
	CONSTRAINT [aIdxtsHelpContents] PRIMARY KEY  CLUSTERED 
	(
		[hlp_id]
	)  
GO

ALTER TABLE [dbo].[tsHelpLinks] WITH NOCHECK ADD 
	CONSTRAINT [aIdxtsHelpLinks] PRIMARY KEY  CLUSTERED 
	(
		[lnk_id]
	)  
GO

ALTER TABLE [dbo].[tsHelpToc] WITH NOCHECK ADD 
	CONSTRAINT [aIdxtsHelpToc] PRIMARY KEY  CLUSTERED 
	(
		[toc_id]
	)  
GO

ALTER TABLE [dbo].[tsHelpChapters] ADD 
	CONSTRAINT [fktsHelpChapters_tsHelpToc] FOREIGN KEY 
	(
		[hlp_toc_id]
	) REFERENCES [dbo].[tsHelpToc] (
		[toc_id]
	)
GO

ALTER TABLE [dbo].[tsHelpLinks] ADD 
	CONSTRAINT [fktsHelpLinks_tsHelpChapters] FOREIGN KEY 
	(
		[lnk_hlp_id]
	) REFERENCES [dbo].[tsHelpChapters] (
		[hlp_id]
	)
GO

ALTER TABLE dbo.tsHelpChapters ADD
	hlp_customid nvarchar(255) NULL
