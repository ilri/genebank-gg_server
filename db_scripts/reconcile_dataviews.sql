-- determine counts of missing / invalid dataviews for each database
select 
    sum(case when db_engine_code = 'sqlserver' and sql_statement is not null then 1 else 0 end) as sql_server_count,
    sum(case when db_engine_code = 'mysql' and sql_statement is not null then 1 else 0 end) as mysql_count,
    sum(case when db_engine_code = 'postgresql' and sql_statement is not null then 1 else 0 end) as postgresql_count,
    sum(case when db_engine_code = 'oracle' and sql_statement is not null then 1 else 0 end) as oracle_count
from sec_dataview_sql 

;

-- see which specific dataviews are missing / invalid from each database (assumes sqlserver records are correct and bases results off of that)
select
    sd.sec_dataview_id,
    sd.dataview_name,
    'mysql' as missing_from,
    case when sql_statement is null then 'empty sql' else 'no record' end as missing_why

from 
    sec_dataview_sql sds inner join sec_dataview sd
        on sds.sec_dataview_id = sd.sec_dataview_id
    where sds.db_engine_code = 'sqlserver' and (sds.sql_statement is null or sds.sec_dataview_id not in 
    (select sds2.sec_dataview_id from sec_dataview_sql sds2 where sds2.db_engine_code = 'mysql'))

union

select
    sd.sec_dataview_id,
    sd.dataview_name,
    'postgresql' as missing_from,
    case when sql_statement is null then 'empty sql' else 'no record' end as missing_why

from 
    sec_dataview_sql sds inner join sec_dataview sd
        on sds.sec_dataview_id = sd.sec_dataview_id
    where sds.db_engine_code = 'sqlserver' and sds.sec_dataview_id not in 
    (select sds2.sec_dataview_id from sec_dataview_sql sds2 where sds2.db_engine_code = 'postgresql')

union

select
    sd.sec_dataview_id,
    sd.dataview_name,
    'oracle' as missing_from,
    case when sql_statement is null then 'empty sql' else 'no record' end as missing_why
from 
    sec_dataview_sql sds inner join sec_dataview sd
        on sds.sec_dataview_id = sd.sec_dataview_id
    where sds.db_engine_code = 'sqlserver' and sds.sec_dataview_id not in 
    (select sds2.sec_dataview_id from sec_dataview_sql sds2 where sds2.db_engine_code = 'oracle')

order by dataview_name

