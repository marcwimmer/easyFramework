DELETE FROM tsEntities WHERE ety_name = 'Adoptions'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml,ety_tabledefxml, ety_windowwidth, ety_windowheight,ety_searchfields,
	ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, 
 	ety_colwidths, ety_primarysearchField)
VALUES('Adoptions', 'Adoptions', '/ASP/system/entityedit/entityedit.aspx?entity=Adoptions&adp_id=$1',
	'/ASP/Project/Adoptionlist/baseedit/adoptions/Dialog.xml',
	'/ASP/Project/Adoptionlist/baseedit/adoptions/TableDef.xml',
	900, 700, 'adp_dozent', 'adp_dozent', 0, 'adp_dozent;adp_plz;adp_ort;adp_autor_titel', 'Dozent;PLZ;Ort;Titel', '30px;30px;30px;50px','adp_dozent')

GO
DELETE FROM tsMainMenue WHERE Project='ADP'
GO
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('adp_main', null, 'Adoptionlist', '', 1, 0, 'treeview_folder','treeview_folder_open','ADP')
GO
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('adp_edit', 'adp_main', 'Bearbeiten', '/ASP/system/entityedit/entityedit.aspx?entity=Adoptions', 1, 0, 'treeview_item','treeview_item','ADP')
GO




