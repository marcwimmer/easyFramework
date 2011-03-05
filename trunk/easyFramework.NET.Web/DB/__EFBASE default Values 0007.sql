delete from tsEntities where ety_name in ('CONFIG', 'ABONNENTS')
delete from tsMainMenue where mnu_id in ('distribution', 'distribution_edit', 'distribution_newpackage', 'distribution_packages')
delete from tsEntityTabs where tab_tabid = 'AbonnentPackages'
GO
DROP TABLE tsAbonnents
GO
ENTERPRISE_COPYPASTE:
tsEntities
	CONFIG	CONFIG		ASP/system/entityedit/entityedit.aspx?entity=Config&cfg_id=$1	/asp/system/config/dialog.xml	/asp/system/config/tableDef.xml	800	600	cfg_name;cfg_value	cfg_name		cfg_name + ISNULL(' ' + cfg_value, 'N/A')	0	cfg_name;cfg_value	Name;Wert	25%;75%
GO
ENTERPRISE_COPYPASTE:
tsEntities
	ABONNENTS	ABONNENTS		ASP/system/entityedit/entityedit.aspx?entity=ABONNENTS&abo_id=$1	/asp/system/abonnents/dialog.xml	/asp/system/abonnents/tableDef.xml	800	600	abo_name;abo_host	cfg_name		abo_name + ISNULL(' ' + abo_host, 'N/A')	0	abo_name;abo_host	Name;Host	25%;75%GO
GO
ENTERPRISE_COPYPASTE:
tsMainMenue
	distribution		Distribution	''	1	0	*	treeview_folder	treeview_folder_open	0	
GO
ENTERPRISE_COPYPASTE:
tsMainMenue
	distribution_edit	distribution	Abonnenten bearbeiten	/ASP/system/entityedit/entityedit.aspx?entity=ABONNENTS	0	0	*	treeview_item	treeview_item	0	
GO
ENTERPRISE_COPYPASTE:
tsMainMenue
	distribution_newpackage	distribution	Neues Paket erstellen	/ASP/tasks/distribution/newPackageDlg.aspx	0	0	*	treeview_item	treeview_item	1	
GO
ENTERPRISE_COPYPASTE:
tsMainMenue
	distribution_packages	distribution	Pakete anzeigen und verteilen	/ASP/system/entityedit/entityedit.aspx?entity=AbonnentPackages	0	0	*	treeview_item	treeview_item	2	
GO
ENTERPRISE_COPYPASTE:
tsEntityTabs
		AbonnentPackages	AbonnentPackages	Abonnenten			/ASP/tasks/distribution/tabs/tab_abonennten.aspx	0
GO
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

