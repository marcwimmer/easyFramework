/*---------------------
Entity Delete Events if usergroups / users are delete
-----------------*/

--usergroups
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'UserGroups', 'SQL',-1000, 'DELETE FROM tsEntityPopupAccessTsUserGroups WHERE epg_grp_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'UserGroups', 'SQL',-1000, 'DELETE FROM tsEntityTabAccessTsUserGroups WHERE etg_grp_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'UserGroups', 'SQL',-1000, 'DELETE FROM tsMenuAccessTsUserGroups WHERE msg_grp_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'UserGroups', 'SQL',-1000, 'DELETE FROM tsPoliciesTsUserGroups WHERE pol_grp_grp_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'UserGroups', 'SQL',-1000, 'DELETE FROM tsUsersTsUserGroups WHERE usr_grp_grp_id = $1')

--users
INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL',-1000, 'DELETE FROM tsEntityPopupAccessTsUsers WHERE epu_usr_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL',-1000, 'DELETE FROM tsEntityTabAccessTsUsers WHERE etu_usr_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL',-1000, 'DELETE FROM tsMenuAccessTsUsers WHERE msu_usr_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL',-1000, 'DELETE FROM tsPoliciesTsUsers WHERE pol_usr_usr_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL',-1000, 'DELETE FROM tsUsersTsUserGroups WHERE usr_grp_usr_id = $1')

INSERT INTO tsSysEvents(sys_category, sys_name, sys_actiontype, sys_index, sys_task)
VALUES('EntityDelete', 'Users', 'SQL',-1000, 'DELETE FROM tsClientInfo WHERE ci_loggedin_usr = $1')


---menu for domain-edit
DELETE FROM tsMainMenue WHERE mnu_id='admin_domains'
INSERT INTO [tsMainMenue]([mnu_id], mnu_parentid, [mnu_title], [mnu_command],  mnu_isFolder,[mnu_modalwindow], [mnu_icon_normal], [mnu_icon_opened], mnu_index)
VALUES('admin_domains', 'admin', 'Domains', '/ASP/system/domainedit/domainedit.aspx', 0, 0, 'treeview_item','treeview_item', 100)
