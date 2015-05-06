select 
  TABLE_NAME,
  COLUMN_NAME,
  data_type,
  coalesce(convert(nvarchar, character_maximum_length), 'n/a') as max_length,
  case when IS_NULLABLE = 'NO' then 'N' else 'Y' end as is_nullable
 from INFORMATION_SCHEMA.columns 
 where TABLE_CATALOG = 'gringlobal'
 order by TABLE_NAME, ordinal_position