/*-------------------------------------------------------------------------------
		Entity-Popups for Users / Groups
-------------------------------------------------------------------------------*/


DELETE FROM tsEntityPopups WHERE epp_ety_name='Users' AND epp_caption='Rollen zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES('Users', 'Rollen zuordnen','/ASP/system/usergroups2user/usergroups2user.aspx?usr_id=$1', 100)


DELETE FROM tsEntityPopups WHERE epp_ety_name='Usergroups' AND epp_caption='Benutzer zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES('Usergroups', 'Benutzer zuordnen','/ASP/system/users2usergroup/users2usergroup.aspx?grp_id=$1', 100)


DELETE FROM tsEntityPopups WHERE epp_ety_name='Usergroups' AND epp_caption='Menüeinträge zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES('Usergroups', 'Menüeinträge zuordnen','/ASP/system/usergroups2menue/usergroups2menue.aspx?grp_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Users' AND epp_caption='Menüeinträge zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES('Users', 'Menüeinträge zuordnen','/ASP/system/users2menue/users2menue.aspx?usr_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Usergroups' AND epp_caption='Popups zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES( 'Usergroups', 'Popups zuordnen','/ASP/system/usergroups2entityPopups/usergroups2entityPopups.aspx?grp_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Users' AND epp_caption='Popups zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES( 'Users', 'Popups zuordnen','/ASP/system/users2entityPopups/users2entityPopups.aspx?usr_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Usergroups' AND epp_caption='Tabreiter zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES( 'Usergroups', 'Tabreiter zuordnen','/ASP/system/usergroups2entityTabs/usergroups2entityTabs.aspx?grp_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Users' AND epp_caption='Tabreiter zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES( 'Users', 'Tabreiter zuordnen','/ASP/system/users2entityTabs/users2entityTabs.aspx?usr_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Usergroups' AND epp_caption='Policies zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES( 'Usergroups', 'Policies zuordnen','/ASP/system/usergroups2policies/usergroups2policies.aspx?grp_id=$1', 100)

DELETE FROM tsEntityPopups WHERE epp_ety_name='Users' AND epp_caption='Policies zuordnen'
INSERT INTO tsEntityPopups(epp_ety_name, epp_caption,epp_url, epp_index)
VALUES( 'Users', 'Policies zuordnen','/ASP/system/users2policies/users2policies.aspx?usr_id=$1', 100)



GO
UPDATE tsUsers 
SET usr_supervisor = 1
WHERE usr_login = 'sa'