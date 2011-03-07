INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntitySelect', 'Surveys', 'SQL', -100, 'exec spGetAllowedSurveys @usr_id=$usr_id')

GO

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL', -100, 'DELETE FROM tdSurveyGroupsTsUsers WHERE svg_usr_usr_id=$1')

GO

GO
UPDATE tsRecordIds SET MaxId=(SELECT ISNULL(Max(eme_id),0) FROM tsEntityMemos )
WHERE tablename='tsEntityMemos'

DECLARE @maxid INT 
EXEC @maxid = spGetNextRecordId 'tsEntityMemos'
INSERT INTO tsEntityMemos(eme_id, eme_caption, eme_ety_name, eme_fieldname, eme_index)
VALUES(@maxid, 'Rahmen HTML Anfang', 'Surveys', 'svy_border_html_begin', 10)

EXEC @maxid =spGetNextRecordId 'tsEntityMemos'
INSERT INTO tsEntityMemos(eme_id, eme_caption, eme_ety_name, eme_fieldname, eme_index)
VALUES(@maxid, 'Rahmen HTML Ende', 'Surveys', 'svy_border_html_begin', 10)

EXEC @maxid =spGetNextRecordId 'tsEntityMemos'
INSERT INTO tsEntityMemos(eme_id, eme_caption, eme_ety_name, eme_fieldname, eme_index)
VALUES(@maxid, 'Rahmen HTML Anfang', 'SurveyGroups', 'svg_border_html_begin', 10)

EXEC @maxid =spGetNextRecordId 'tsEntityMemos'
INSERT INTO tsEntityMemos(eme_id, eme_caption, eme_ety_name, eme_fieldname, eme_index)
VALUES(@maxid, 'Rahmen HTML Ende', 'SurveyGroups', 'svg_border_html_begin', 10)

GO


/*-------------------------
use of html-editor
--------------------------*/
UPDATE tsEntityMemos
SET eme_extendedHtmlEditor = 0
WHERE eme_caption='Rahmen HTML Anfang' or eme_caption='Rahmen HTML Ende'
GO

/*-------------------------
correction
--------------------------*/
UPDATE tsEntityMemos
SET eme_fieldname = 'svy_border_html_end'
WHERE eme_caption='Rahmen HTML Ende' and eme_ety_name='Surveys'
UPDATE tsEntityMemos
SET eme_fieldname = 'svg_border_html_end'
WHERE eme_caption='Rahmen HTML Ende' and eme_ety_name='Surveygroups'
GO

/*-------------------------
menue for slots
--------------------------*/
DELETE FROM tsMainMenue WHERE mnu_id = 'Slots'
INSERT INTO tsMainMenue(mnu_id, mnu_parentid, mnu_title, mnu_command, mnu_isFolder, 
mnu_icon_normal, mnu_icon_opened, mnu_index)
VALUES('Slots', NULL, 'Slots', '', 1, 'treeview_folder', 'treeview_folder_open', 200)
GO
DELETE FROM tsMainMenue WHERE mnu_id = 'EditSlots'
INSERT INTO tsMainMenue(mnu_id, mnu_parentid, mnu_title, mnu_command, mnu_isFolder, 
mnu_icon_normal, mnu_icon_opened, mnu_index)
VALUES('EditSlots', 'Slots', 'Slots bearbeiten', '/ASP/system/entityedit/entityedit.aspx?entity=Slots', 0, 'treeview_item', 'treeview_item', 0)
GO

/*-------------------------
entities for slots
--------------------------*/
DELETE FROM tsEntities WHERE ety_name = 'Slots'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml, 
	ety_tableDefxml, ety_searchfields, ety_primarySearchField, ety_ToStringFields, ety_cols, ety_colwidths, ety_colcaptions)
VALUES('Slots', 'Slot', '/ASP/system/entityedit/entityedit.aspx?entity=Slots&slt_id=$1',
	'/asp/Project/SurveyManager/baseedit/slots/Dialog.xml', '/asp/Project/SurveyManager/baseedit/slots/TableDef.xml', 
	'slt_name;slt_externalid', 'slt_name', 'ISNULL(slt_name,'''')',
	'slt_name;slt_externalid', '100px;100px', 'Slot;ext. Web-ID')
GO
DELETE FROM tsEntityTabs WHERE tab_entity = 'Slots' AND tab_tabid='link'
INSERT INTO tsEntityTabs(tab_entity, tab_tabid, tab_tabtext, tab_url, tab_index)
VALUES('Slots', 'link', 'Link', '/ASP/Project/SurveyManager/baseedit/slots/entitytabs/tab_link.aspx?slt_id=$1', 200)
GO

/*-------------------------
entity-popup placeholders
--------------------------*/
GO
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption, epp_url, epp_index)
VALUES('Surveys', 'HTML Platzhalter','/ASP/Project/SurveyManager/surveyplaceholders/placeholder.aspx?svy_id=$1',500)
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption, epp_url, epp_index)
VALUES('SurveyGroups', 'HTML Platzhalter','/ASP/Project/SurveyManager/surveyplaceholders/placeholder.aspx?svg_id=$1',500)
