/*
  replace all occurrences of FROM_DB with source database
  replace all occurrences of TO_DB with target database
*/
start transaction;

delete from TO_DB.sec_rs_field_friendly;
delete from TO_DB.sec_rs_field;
delete from TO_DB.sec_rs_param;
delete from TO_DB.sec_rs;

/* copy sec_rs records */
insert into TO_DB.sec_rs
select * from FROM_DB.sec_rs where rs_name not in
  (select rs_name from TO_DB.sec_rs);

/* copy sec_rs_param records */
insert into TO_DB.sec_rs_param
select * from FROM_DB.sec_rs_param where sec_rs_param_id not in
  (select sec_rs_param_id from TO_DB.sec_rs_param);

/* copy sec_table records */
insert into TO_DB.sec_table
select * from FROM_DB.sec_table where sec_table_id not in
  (select sec_table_id from TO_DB.sec_table);

/* copy sec_table_field records */
insert into TO_DB.sec_table_field
select * from FROM_DB.sec_table_field where sec_table_field_id not in
  (select sec_table_field_id from TO_DB.sec_table_field);


/* copy sec_rs_field records */
insert into TO_DB.sec_rs_field
select * from FROM_DB.sec_rs_field where sec_rs_field_id not in
  (select sec_rs_field_id from TO_DB.sec_rs_field);

/* copy sec_rs_field_friendly records */
insert into TO_DB.sec_rs_field_friendly
select * from FROM_DB.sec_rs_field_friendly where sec_rs_field_friendly_id not in
  (select sec_rs_field_friendly_id from TO_DB.sec_rs_field_friendly);