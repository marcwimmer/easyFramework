/*-------------------------
entities for placeholders
--------------------------*/
DELETE FROM tsEntities WHERE ety_name = 'PlaceHolders'
INSERT INTO tsEntities(ety_name, ety_title, ety_editdialog, ety_editdialogxml, 
	ety_tableDefxml, ety_searchfields, ety_primarySearchField, ety_ToStringFields, ety_cols, ety_colwidths, ety_colcaptions)
VALUES('PlaceHolders', 'PlaceHolders', '/ASP/system/entityedit/entityedit.aspx?entity=PlaceHolders&plc_id=$1',
	'/asp/Project/SurveyManager/baseedit/PlaceHolders/Dialog.xml', '/asp/Project/SurveyManager/baseedit/PlaceHolders/TableDef.xml', 
	'plc_name;plc_value', 'plc_name', 'ISNULL(plc_name,'''')',
	'plc_name;plc_value', '100px;100px', 'Platzhalter;HTML')
GO
DECLARE @maxID INT
UPDATE tsRecordIds SET MaxID = (SELECT MAX(eme_id) FROM tsEntityMemos) WHERE TableName = 'tsEntityMemos'
EXEC @maxID = dbo.spGetNextRecordId 'tsEntityMemos'
INSERT INTO tsEntityMemos(eme_id,eme_caption,eme_ety_name,eme_fieldname,eme_index)
VALUES(@maxID, 'Platzhalter HTML', 'PlaceHolders', 'plc_value',2)
GO
