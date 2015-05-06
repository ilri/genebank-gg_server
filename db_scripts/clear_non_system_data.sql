-- run this script repeatedly until no errors are returned (note we always skip the sysdiagrams table so we don't blow away any diagrams they mght have)
use [gringlobal]
delete from app_user_item_list;
delete from app_user_gui_setting;
delete from site_inventory_nc7
delete from sys_user_permission_map where sys_user_id in (select sys_user_id from sys_user where user_name not in ('administrator', 'guest'))
delete from sys_group_user_map where sys_user_id in (select sys_user_id from sys_user where user_name not in ('administrator', 'guest'))
delete from sys_user where user_name not in ('administrator', 'guest')

update site set site_short_name = 'SYS', site_long_name = 'SYSTEM' where site_short_name = 'ADMIN'

declare @syscoop int
select @syscoop = cooperator_id from cooperator where last_name like 'SYSTEM%'
update app_resource set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update app_setting set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update cooperator set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update site set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_user_permission_map set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_user set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_table_relationship set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_table_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_table_field_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_table_field set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_table set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_permission_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_permission_field set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_permission set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_index_field set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_index set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_group_user_map set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_group_permission_map set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_group_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_group set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_file_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_file_group_map set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_file_group set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_file set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_dataview_sql set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_dataview_param set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_dataview_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_dataview_field_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_dataview_field set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_dataview set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_datatrigger_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_datatrigger set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_database_migration_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_database_migration set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update sys_database set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update code_value set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop
update code_value_lang set created_by = @syscoop, modified_by = @syscoop, owned_by = @syscoop

update sys_user set user_name = 'administrator' where user_name = 'admin1'

delete from cooperator_map -- where cooperator_id in (select cooperator_id from cooperator where last_name not like 'SYSTEM%')
delete from cooperator where last_name not like 'SYSTEM%' and cooperator_id not in (select cooperator_id from sys_user where user_name in ('administrator', 'guest'))
delete from site where site_short_name not like 'SYS%'
--update cooperator set geography_id = null where last_name not like 'SYSTEM%'
delete from crop_trait_code_attach
delete from crop_trait_code_lang
delete from crop_trait_lang
delete from crop_trait_attach
delete from crop_trait_code
delete from crop_trait
exec sp_MSforeachtable 'if ''?'' != ''[dbo].[sysdiagrams]'' and charindex(''sys_'', ''?'', 0) = 0 and charindex(''app_'', ''?'', 0) = 0 and charindex(''cooperator'', ''?'', 0) = 0 and charindex(''code_'', ''?'', 0) = 0 and charindex(''site'', ''?'', 0) = 0 and charindex(''web_user'', ''?'', 0) = 0 and charindex(''web_cooperator'', ''?'', 0) = 0 delete from ?;'
--select CHARINDEX('sys_', 'sys_table', 0)

--select * from sys_user su inner join cooperator c on su.cooperator_id = c.cooperator_id

--select * from cooperator where cooperator_id in (48, 829)