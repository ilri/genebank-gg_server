SELECT  
    TABLE_NAME = t.table_name,
    UNIQUE_INDEX_NAME = i.name, 
    FILE_GROUP = g.GroupName 
FROM 
	information_schema.TABLES t left join  sysindexes i 
		on t.TABLE_NAME = OBJECT_NAME(i.id)
		and (i.indid BETWEEN 1 AND 254) 
		-- leave out AUTO_STATISTICS: 
		AND (i.Status & 64)=0 
		-- leave out system tables: 
		AND OBJECTPROPERTY(i.id, 'IsMsShipped') = 0 
		and INDEXPROPERTY(i.id, i.name, 'IsUnique') = 1
		and INDEXPROPERTY(i.id, i.name, 'IsClustered') = 0
    left JOIN sysfilegroups g 
		ON i.groupid = g.groupid 
where 
	t.TABLE_NAME is not null
order by 1