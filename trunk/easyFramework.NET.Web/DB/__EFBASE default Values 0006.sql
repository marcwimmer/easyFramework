---insert entity tsConfig
DELETE FROM tsEntities WHERE ety_name LIKE 'CONFIG'
INSERT INTO [tsEntities]([ety_name], [ety_title], [ety_searchdialog], [ety_editdialog], [ety_editdialogxml], [ety_tableDefXml], [ety_windowwidth], [ety_windowheight], [ety_searchfields], [ety_primarySearchField], [ety_icon], [ety_tostringFields], [ety_concurrentUpdates], [ety_cols], [ety_colcaptions], [ety_colwidths])
VALUES('CONFIG', 
'CONFIG', 
null, 
'ASP/system/entityedit/entityedit.aspx?entity=Config&cfg_id=$1', 
'/asp/system/config/dialog.xml', 
'/asp/system/config/tableDef.xml', 
800, 600, 'cfg_name;cfg_value', 
'cfg_name', 
NULL, 'cfg_name + ISNULL('' '' + cfg_value, ''N/A'')', 
0, 'cfg_name;cfg_value', 'Name;Wert', '25%;75%')
GO
DELETE FROM tsMainMenue WHERE mnu_id = 'admin_config'
INSERT INTO [tsMainMenue]([mnu_id], [mnu_parentid], [mnu_title], [mnu_command], [mnu_isFolder], [mnu_modalwindow], [mnu_edition], [mnu_icon_normal], [mnu_icon_opened], [mnu_index], [Project])
VALUES('admin_config', 'admin', 'Konfiguration', '/ASP/system/entityedit/entityedit.aspx?entity=CONFIG', 
0, 0, '*', 'treeview_item', 'treeview_item', 
0, null)
GO
DELETE FROM tsMainMenue WHERE mnu_id = 'distribution_edit'
GO
DELETE FROM tsMainMenue WHERE mnu_id = 'distribution'
GO

INSERT INTO [tsMainMenue]([mnu_id], [mnu_parentid], [mnu_title], [mnu_command], [mnu_isFolder], [mnu_modalwindow], [mnu_edition], [mnu_icon_normal], [mnu_icon_opened], [mnu_index], [Project])
VALUES('distribution', null, 'Distribution', 
'', 
1, 0, '*', 'treeview_folder', 'treeview_folder_open', 
0, null)
GO
INSERT INTO [tsMainMenue]([mnu_id], [mnu_parentid], [mnu_title], [mnu_command], [mnu_isFolder], [mnu_modalwindow], [mnu_edition], [mnu_icon_normal], [mnu_icon_opened], [mnu_index], [Project])
VALUES('distribution_edit', 'distribution', 'Abonnenten bearbeiten', 
'/ASP/system/entityedit/entityedit.aspx?entity=ABONNENTS', 
0, 0, '*', 'treeview_item', 'treeview_item', 
0, null)
GO
DELETE FROM tsEntities WHERE ety_name LIKE 'ABONNENTS'
INSERT INTO [tsEntities]([ety_name], [ety_title], [ety_searchdialog], [ety_editdialog], [ety_editdialogxml], [ety_tableDefXml], [ety_windowwidth], [ety_windowheight], [ety_searchfields], [ety_primarySearchField], [ety_icon], [ety_tostringFields], [ety_concurrentUpdates], [ety_cols], [ety_colcaptions], [ety_colwidths])
VALUES('ABONNENTS', 
'ABONNENTS', 
null, 
'ASP/system/entityedit/entityedit.aspx?entity=ABONNENTS&abo_id=$1', 
'/asp/system/abonnents/dialog.xml', 
'/asp/system/abonnents/tableDef.xml', 
800, 600, 'abo_name;abo_host', 
'cfg_name', 
NULL, 'abo_name + ISNULL('' '' + abo_host, ''N/A'')', 
0, 'abo_name;abo_host', 'Name;Host', '25%;75%')
GO
