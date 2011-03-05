INSERT INTO tsEntities(ety_name, ety_title, ety_searchdialog, ety_editdialog, ety_editdialogxml, ety_tabledefxml, 
ety_windowwidth, ety_windowheight, ety_searchfields, ety_primarysearchfield, ety_icon, ety_tostringfields, ety_concurrentupdates, ety_cols, ety_colcaptions, ety_colwidths)
VALUES('AbonnentPackages', 'Abonnement Pakete', null, 
'/ASP/system/entityedit/entityedit.aspx?entity=AbonnentPackages&dst_id=$1',
'/asp/tasks/distribution/entities/AbonnentsPackagesDlg.xml', 
'/asp/tasks/distribution/entities/AbonnentsPackagesTableDef.xml', 
'800', '600',  'dst_packagename', 'dst_packagename', null, 'ISNULL(dst_packagename, '''')', 1,
'dst_packagename', 'Name', '100%')