DELETE FROM tsMainMenue WHERE mnu_id = 'adp_excel'
GO
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command], mnu_isFolder, [mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], Project)
VALUES('adp_excel', 'adp_main', 'Excel abrufen', '/ASP/Project/Adoptionlist/getexcel/getexcel.aspx', 1, 0, 'treeview_item','treeview_item','ADP')
GO



CREATE VIEW dbo.vwAdoptions
AS
SELECT     dbo.tdAdoptions.adp_id AS [Adoption Nummer], dbo.tsUsers.usr_firstname + N' ' + dbo.tsUsers.usr_lastname AS Verantwortlicher, 
                      dbo.tdAdoptions.adp_plz AS PLZ, dbo.tdAdoptions.adp_ort AS Ort, dbo.tdAdoptions.adp_land AS Land, dbo.tdAdoptions.adp_dozent AS Dozent, 
                      dbo.tdAdoptions.adp_autor_titel AS AutorTitel, dbo.tdAdoptions.adp_isbn AS ISBN, dbo.tdAdoptions.adp_anzahl_studenten AS [Anzahl Studenten], 
                      dbo.tdAdoptions.adp_name_vorlesung AS [Name Vorlesung], dbo.tdAdoptions.adp_price_vista_excl_vat AS [Price Vista Excl Vat], 
                      dbo.tdAdoptions.adp_bemerkung AS Bemerkung, dbo.tdAdoptions.adp_buchhandlung AS Buchhandlung, 
                      dbo.tdAdoptions.adp_verkaufte_exemplare AS [Verkaufte Exemplare], dbo.tdAdoptions.adp_inserted AS Eingefügt, 
                      dbo.tdAdoptions.adp_updated AS Bearbeitet
FROM         dbo.tdAdoptions LEFT OUTER JOIN
                      dbo.tsUsers ON dbo.tdAdoptions.adp_responsible_user_id = dbo.tsUsers.usr_id

