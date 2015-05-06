
/*
  SQL Server script for creating missing sys_dataview_sql records for 'other' 3 db engines.  Assumes sqlserver records are correct and are the master.
 */
begin tran

/* delete from sys_dataview_sql where database_engine_tag = 'oracle' */

insert into sys_dataview_sql (sys_dataview_id, database_engine_tag, sql_statement, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select sds.sys_dataview_id, 'mysql', sds.sql_statement, GETUTCDATE(), 1, null, null, GETUTCDATE(), 1
from sys_dataview_sql sds where sds.database_engine_tag = 'sqlserver' and sds.sys_dataview_id not in (
		select sds2.sys_dataview_id from sys_dataview_sql sds2 
			where sds2.database_engine_tag = 'mysql' and sds2.sys_dataview_id = sds.sys_dataview_id
	)


insert into sys_dataview_sql (sys_dataview_id, database_engine_tag, sql_statement, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select sds.sys_dataview_id, 'postgresql', sds.sql_statement, GETUTCDATE(), 1, null, null, GETUTCDATE(), 1
from sys_dataview_sql sds where sds.database_engine_tag = 'sqlserver' and sds.sys_dataview_id not in (
		select sds2.sys_dataview_id from sys_dataview_sql sds2 
			where sds2.database_engine_tag = 'postgresql' and sds2.sys_dataview_id = sds.sys_dataview_id
	)

insert into sys_dataview_sql (sys_dataview_id, database_engine_tag, sql_statement, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select sds.sys_dataview_id, 'oracle', sds.sql_statement, GETUTCDATE(), 1, null, null, GETUTCDATE(), 1
from sys_dataview_sql sds where sds.database_engine_tag = 'postgresql' and sds.sys_dataview_id not in (
		select sds2.sys_dataview_id from sys_dataview_sql sds2 
			where sds2.database_engine_tag = 'oracle' and sds2.sys_dataview_id = sds.sys_dataview_id
	)

commit tran
