

update tsEntities
set 
ety_cols='adp_id;adp_responsible_user_value;adp_land;adp_plz;adp_ort;adp_dozent;adp_autor_titel;adp_isbn;adp_anzahl_studenten; adp_name_vorlesung; adp_price_vista_excl_vat; adp_bemerkung; adp_buchhandlung; adp_verkaufte_exemplare',
ety_colcaptions='Nr.;Verantwortlicher;Land;PLZ;Ort;Dozent;Titel;ISBN; Studenten; Vorlesung; Preis Vista excl. Vat; Bemerkung; Buchhandlung; verk. Ex.',
ety_colwidths='20px;30px;30px;30px;30px;30px;50px;30px;30px;30px;30px;30px;30px;30px'
where ety_name ='Adoptions'