delete from tsEntities where ety_name in ('CONFIG', 'ABONNENTS')
delete from tsMainMenue where mnu_id in ('distribution', 'distribution_edit', 'distribution_newpackage', 'distribution_packages')
delete from tsEntityTabs where tab_tabid = 'AbonnentPackages'
GO

insert into tsEntities(ety_name, ety_title, ety_searchdialog,ety_editdialog,ety_editdialogxml,  ety_tabledefxml, 
  ety_windowwidth, ety_windowheight, ety_searchfields,  ety_primarysearchfield, 
  ety_icon, ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths)
values('CONFIG', 'CONFIG', null, 'ASP/system/entityedit/entityedit.aspx?entity=Config&cfg_id=$1',
'/asp/system/config/dialog.xml', 
'/asp/system/config/tableDef.xml',
'800', '600',
'cfg_name;cfg_value',	'cfg_name',	null,	'cfg_name + ISNULL('' '' + cfg_value, ''N/A'')',	0,	'cfg_name;cfg_value',	'Name;Wert',	'25%;75%')

GO
insert into tsEntities(ety_name, ety_title, ety_searchdialog, ety_editdialog, ety_editdialogxml, ety_tabledefxml, 
  ety_windowwidth, ety_windowheight, ety_searchfields,  ety_primarysearchfield, 
  ety_icon, ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths)
VALUES('ABONNENTS','ABONNENTS',	null,	'ASP/system/entityedit/entityedit.aspx?entity=ABONNENTS&abo_id=$1',	
'/asp/system/abonnents/dialog.xml',
'/asp/system/abonnents/tableDef.xml',
800,600,
'abo_name;abo_host', 'abo_name',null,'abo_name + ISNULL('' '' + abo_host, ''N/A'')',	0	,'abo_name;abo_host','Name;Host','25%;75%')
GO
INSERT INTO tsMainMenue(mnu_id, mnu_command, mnu_title, mnu_parentid, mnu_isfolder, mnu_modalwindow, mnu_edition, mnu_icon_normal, mnu_icon_opened, mnu_index)
VALUES('distribution','','Distribution',null,1,0, '*', 'treeview_folder','treeview_folder_open',0)

GO
INSERT INTO tsMainMenue(mnu_id, mnu_command, mnu_title, mnu_parentid, mnu_isfolder, mnu_modalwindow, mnu_edition, mnu_icon_normal, mnu_icon_opened, mnu_index)
VALUES('distribution_edit','/ASP/system/entityedit/entityedit.aspx?entity=ABONNENTS','distribution','Abonnenten bearbeiten',0,	0,'*','treeview_item','treeview_item',0)
GO
INSERT INTO tsMainMenue(mnu_id, mnu_command, mnu_parentid, mnu_title, mnu_isfolder, mnu_modalwindow, mnu_edition, mnu_icon_normal, mnu_icon_opened, mnu_index)
VALUES('distribution_newpackage','/ASP/tasks/distribution/newPackageDlg.aspx','distribution','Neues Paket erstellen',0,	0,'*','treeview_item','treeview_item',1	)
GO

INSERT INTO tsMainMenue(mnu_id, mnu_command, mnu_parentid, mnu_title, mnu_isfolder, mnu_modalwindow, mnu_edition, mnu_icon_normal, mnu_icon_opened, mnu_index)
VALUES('distribution_packages','/ASP/system/entityedit/entityedit.aspx?entity=AbonnentPackages','distribution','Pakete anzeigen und verteilen', 0,0,'*','treeview_item','treeview_item',2)
GO
INSERT INTO tsEntityTabs(tab_entity, tab_tabid, tab_tabtext, tab_url, tab_index)
VALUES('AbonnentPackages','AbonnentPackages','Abonnenten','/ASP/tasks/distribution/tabs/tab_abonennten.aspx',0)
GO

