select 
	sd.dataview_name,
	sdf.field_name as dataview_field_name, 
	st.table_name + ' (' + coalesce(sdf.table_alias_name,'n/a') + ')' as table_and_alias, 
	stf.field_name as table_field_name, 
	sdf.configuration_options, 
	sdf.gui_hint,
	stf.gui_hint,
	sdf.*
from 
	sys_dataview sd inner join sys_dataview_field sdf 
		on sd.sys_dataview_id = sdf.sys_dataview_id 
	left join sys_table_field stf 
		on sdf.sys_table_field_id = stf.sys_table_field_id
	left join sys_table st 
		on stf.sys_table_id = st.sys_table_id
where 
	sd.dataview_name like 'get_accession%'
order by 
	1, sort_order