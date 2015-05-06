SELECT sys.tables.object_id, sys.tables.name as table_name, sys.columns.name as column_name, sys.indexes.name as index_name,
sys.indexes.is_unique, sys.indexes.is_primary_key 
FROM sys.tables, sys.indexes, sys.index_columns, sys.columns 
WHERE (sys.tables.object_id = sys.indexes.object_id AND sys.tables.object_id = sys.index_columns.object_id AND sys.tables.object_id = sys.columns.object_id
AND sys.indexes.index_id = sys.index_columns.index_id AND sys.index_columns.column_id = sys.columns.column_id) 
--AND sys.tables.name = 'accession'
order by sys.tables.name, sys.indexes.name, sys.index_columns.key_ordinal