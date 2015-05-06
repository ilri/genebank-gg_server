/***********************************************/
/********* Create temp table for holding all userids while the migration is processing ********************/
/***********************************************/

create global temporary table gringlobal.migration_tempuser (
	tempusername varchar2(50 char) not null,
	tempsite varchar2(50 char) null,
	tempcno integer not null,
	tempenabled char(1 char) not null,
	tempcreated timestamp null,
	tempmodified timestamp null
) on commit delete rows;


/* pull in existing site users, enable them */
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) 
select siteuser, site, coalesce(cno, 0), 'Y', created, modified from main.siteuser;

/* we don't know created/modified for users not in main.siteuser, so just use a known bogus one, which is 0 (cooperator_id for SYSTEM) */

/* pull in old, bogus users, disable them */
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSLYJG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPGCAW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPGCJE', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PALMDI', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('COTEP', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DAVAC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DAVAS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DAVJS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMUBN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMUEB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMUSS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSHOGR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSPIAL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSZEJF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NACG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NAEH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NASB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7AG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7BB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7DL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7GC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7MA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7MC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7MR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7SB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T5', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7TL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_SRK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSBJF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLGK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLJF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$CORBC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$CORDC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$COTEP', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUBN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUEB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUGE', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUJE', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUMB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUQS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUTH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENDD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENST', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$HILOCR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$HILOMM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MIAHD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MIARS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MIAWW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NACB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NACG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NASB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7BF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7BR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7DB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7KR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7LB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7LM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7MM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7MW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7RA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7RJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7RL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7TL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9ST', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9TF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NR6JS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSGCGR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSGCHB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLFT', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLGK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLJF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLJG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLJR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLXX', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PEODH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PEOMG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PGQOJB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PIOBN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PIODH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PIOVB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$RIVSJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$S9LC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$S9MS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$S9PK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLBL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLCR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLJW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLMJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLTA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLVB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SOYJH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$TOBVS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6AL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6DS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6ME', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6SS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6WK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PALMGR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PARLMM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PGQOMA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('RIVLR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('RIVSJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9PK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9TB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBMLJC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBMLMS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBMLPG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SOYJH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SOYJW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('TOBVS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('W6AL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('W6PL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('W6PR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('W6SS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUGJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DAVMD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7MB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NGRLAS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PEOJD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PEOML', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$S9VB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6RH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PALMAN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PGQORO', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSLYJF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSTRGR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7MB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NGRLAS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSBGK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DAVBP', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DAVIL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GSHOGR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NAAS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7DK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7T2', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NGRLJC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6CW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6SG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PGQOOH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PROD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMUMC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMUJM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('HILOMM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NGRLNR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLWG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$CORBB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUJM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENDF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENDJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENDS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENEW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENJK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENTF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$HILOFZ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MAYBA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MAYFV', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MAYMR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NAKT', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9DD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9DF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9DJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9DS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9EW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NR6JB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SOYDM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PARLAC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PARLMJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PEOML', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PGQOEP', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PGQORM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9DK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9VB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMU', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBML', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7AO', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7SW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7JR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUEA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PEOKW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$PIOML', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('CAET', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DBMUJB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('FORMUSER', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('FRASJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GENSK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSHOKS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSTRCB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('GSTRKS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('MAIN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('MAYOG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7AS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7BL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7CN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7PB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_ALM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_AMW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_APP', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_CTB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_DAS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_DNB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_EZI', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_JJH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_JLW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_MAF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_MLE', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_RCO', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_SDM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_SMH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_SRD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_TDM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_TMB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T_ZMM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSGCKW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLLD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OIGMW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OIGRP', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPGCDT', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPGCPS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PARLAB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PARLJD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PARLJM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('PEOAB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9GG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9MN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('S9SB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBMLKO', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('TOBJL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('W6EJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('W6WK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NANG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T1', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T2', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T3', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7T4', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLLT', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7AO', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7DF', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7IL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7MA', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7SJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7SS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7T1', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7T3', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7T4', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7T5', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7TC', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6RJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7CL', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('FLAXMM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLMD', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUSS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLFT', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUKE', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$GENSK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$MIAGM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7RS', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NE9SK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NSSLWG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SOYRN', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6AH', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6PR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DAVCW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$DBMUDW', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$NC7SM', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$W6BG', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('DAVLR', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('OPS$SBMLSI', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBMLSI', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('SBMLMJ', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NC7LK', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));
insert into gringlobal.migration_tempuser (tempusername, tempsite, tempcno, tempenabled, tempcreated, tempmodified) values ('NSSLCB', '---', 0, 'N', to_date('1900-01-01', 'YYYY-MM-DD'), to_date('1900-01-01', 'YYYY-MM-DD'));



/***********************************************/
/********* Insert Boilerplate Data *************/
/***********************************************/


/* create bogus 'SYSTEM' row for cooperator = 0 (we can't use 1 as it's already taken and we're preserving previous id's) */
insert into GRINGLOBAL.cooperator 
	(cooperator_id, last_name, is_active, full_name, note, created_date, created_by, owned_date, owned_by)
values
	(0, 'SYSTEM', 'N', 'SYSTEM', 'Default SYSTEM user required by GRIN-Global', current_timestamp, 0, current_timestamp, 0);

/* create bogus sec_user 'SYSTEM' row sec_user_id = 0 */
insert into GRINGLOBAL.sec_user
	(sec_user_id, user_name, password, is_enabled, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
values
	(0, 'SYSTEM', 'BOGUS', 'N', null, current_timestamp, 0, null, null, current_timestamp, 0);

/* create sec_user rows as needed using the temp table we created above */
insert into GRINGLOBAL.sec_user
	(user_name, password, is_enabled, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
    tempusername, 'BOGUS', tempenabled, tempcno, tempcreated, 0, tempmodified, 0, tempcreated, 0 
from gringlobal.migration_tempuser;

/***********************************************/
/********* Insert Real Data ********************/
/***********************************************/

/* creating real rows for cooperator, we use all as English (1) since that's all GC users had available */
/* cooperator */
insert into GRINGLOBAL.cooperator (cooperator_id, current_cooperator_id, site_code, last_name, title, first_name, job, organization, organization_code, address_line1, address_line2, address_line3, admin_1, admin_2, geography_id, primary_phone, secondary_phone, fax, email, is_active, category_code, organization_region, discipline, initials, full_name, note, sec_lang_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select cno, validcno, site, lname, title, fname, job, org, orgid, add1, add2, add3, city, zip, geono, phone1, phone2, fax, email, coalesce(case when active = 'X' then 'Y' else active end, 'N'), cat, arsregion, discipline, initials, coop, cmt, 
1, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.coop.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.coop.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.coop.userid), 0)
from PROD.coop;


/* code_column */
/* not used...
insert into GRINGLOBAL.code_column (owner, table_name, column_name, code_no, site, userid, created, modified, master, form_name)
select owner, table_name, column_name, code_no, site, userid, created, modified, master, form_name from PROD.code_column;
*/


/***********************************************/
/********* Special Case Code *******************/
/***********************************************/
/* GC uses cgid (which is a varchar(20) field) as its PK.
   GG uses cooperator_group_id (which is an int field).
   There is no provision for storing GC's prod.cg.cgid in the GG schema
   so we create a temp table here to hold it while we migrate data.
   */

DROP TABLE GRINGLOBAL.migration_cg;
DROP SEQUENCE GRINGLOBAL.sq_migration_cg;
DROP TRIGGER GRINGLOBAL.tg_migration_cg;

create table GRINGLOBAL.migration_cg (
	gg_cooperator_group_id integer not null,
	gc_cgid varchar2(20 char) null
);
CREATE SEQUENCE GRINGLOBAL.sq_migration_cg minvalue 1 start with 1 increment by 1;
CREATE OR REPLACE TRIGGER GRINGLOBAL.tg_migration_cg BEFORE INSERT ON GRINGLOBAL.migration_cg FOR EACH ROW BEGIN IF :NEW.gg_cooperator_group_id IS NULL THEN SELECT GRINGLOBAL.sq_migration_cg.NEXTVAL INTO :NEW.gg_cooperator_group_id FROM DUAL; END IF; END;
/

insert into gringlobal.migration_cg (gc_cgid)
select prod.cg.cgid from prod.cg;

/* cooperator_group */
insert into GRINGLOBAL.cooperator_group (cooperator_group_id, name, site_code, is_historical, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select gringlobal.migration_cg.gg_cooperator_group_id, cgname, site, coalesce(case when historical = 'X' then 'Y' else historical end, 'N'), cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.cg.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.cg.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.cg.userid), 0)
 from PROD.cg inner join gringlobal.migration_cg on prod.cg.cgid = gringlobal.migration_cg.gc_cgid;

 
/* cooperator_map */
insert into GRINGLOBAL.cooperator_map (cooperator_id, cooperator_group_id, note, localid, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select cno, (select gg_cooperator_group_id from gringlobal.migration_cg where gc_cgid = prod.mbr.cgid), cmt, localid, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.mbr.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.mbr.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.mbr.userid), 0)
 from PROD.mbr;

/* crop */
insert into GRINGLOBAL.crop (crop_id, name, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select cropno, crop, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.crop.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.crop.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.crop.userid), 0)
 from PROD.crop;

/* crop_trait */
insert into GRINGLOBAL.crop_trait (crop_trait_id, short_name, name, is_cgc_approved, category_code, data_type, is_coded, max_length, numeric_format, numeric_max, numeric_min, original_value_type, original_value_format, crop_id, site_code, definition, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select dno, dqname, dname, coalesce(case when cac = 'X' then 'Y' else cac end, 'N'), dcat, obtype, coalesce(case when usecode = 'X' then 'Y' else usecode end, 'N'), obmaxlen, obformat, obmax, obmin, orgtype, orgformat, cropno, site, def, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.dsc.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.dsc.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.dsc.userid), 0)
 from PROD.dsc;

/* crop_trait_code */
insert into GRINGLOBAL.crop_trait_code (crop_trait_id, code, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select dno, code, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.cd.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.cd.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.cd.userid), 0)
 from PROD.cd;

/* crop_trait_qualifier */
insert into GRINGLOBAL.crop_trait_qualifier (crop_trait_qualifier_id, crop_trait_id, name, definition, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select qno, dno, qual, def, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.qual.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.qual.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.qual.userid), 0)
from PROD.qual;

/* genetic_marker */
insert into GRINGLOBAL.genetic_marker (genetic_marker_id, crop_id, site_code, name, synonyms, repeat_motif, primers, assay_conditions, range_products, known_standards, genebank_number, map_location, position, note, poly_type, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select mrkno, cropno, site, marker, synonyms, repeat_motif, primers, assay_conditions, range_products, known_standards, genbank_no, map_location, position, cmt, poly_type, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.mrk.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.mrk.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.mrk.userid), 0)
 from PROD.mrk;

/* inventory_group */
insert into GRINGLOBAL.inventory_group (group_name, site_code, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select igname, site, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ig.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ig.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ig.userid), 0)
from PROD.ig;

/* inventory_maint_policy */
insert into GRINGLOBAL.inventory_maint_policy (maintenance_name, site_code, inventory_default_form, unit_of_maintenance, is_debit, distribution_default_form, standard_distribution_quantity, unit_of_distribution, distribution_critical_amount, replenishment_critical_amount, regeneration_method, standard_pathogen_test_count, note, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select imname, site, ivt, munits, coalesce(case when debit = 'X' then 'Y' else debit end, 'N'), dform, dquant, dunits, dcritical, rcritical, regen, ptests, cmt, cno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.im.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.im.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.im.userid), 0)
 from PROD.im;

/* literature */
insert into GRINGLOBAL.literature (abbreviation, standard_abbreviation, reference_title, author_editor_name, note, site_code, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select abbr, stdabbr, reftitle, editor, cmt, site, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.lit.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.lit.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.lit.userid), 0)
 from PROD.lit;



/* order_request */
insert into GRINGLOBAL.order_request (order_request_id, original_order_request_id, site_code, local_number, order_type, ordered_date, status, is_completed, acted_date, source_cooperator_id, requestor_cooperator_id, ship_to_cooperator_id, final_recipient_cooperator_id, order_obtained_via, is_supply_low, note, special_instruction, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select orno, origno, site, localno, ortype, ordered, status, coalesce(case when done = 'X' then 'Y' else done end, 'N'), acted, source, orderer, shipto, final, reqref, coalesce(case when supplylow = 'X' then 'Y' else supplylow end, 'N'), cmt, request, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ord.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ord.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ord.userid), 0)
 from PROD.ord;


/* order_request_action */
insert into GRINGLOBAL.order_request_action (order_request_action_id, action_name, acted_date, action_for_id, order_request_id, site_code, note, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select oactno, action, acted, actid, orno, site, cmt, cno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.oact.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.oact.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.oact.userid), 0)
 from PROD.oact;

/* plant_introduction */
insert into GRINGLOBAL.plant_introduction (plant_introduction_id, plant_introduction_year_date, lowest_pi_number, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select pivol, to_date(to_char(piyear) || '-01-01', 'YYYY-MM-DD'), lowpi, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.pi.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.pi.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.pi.userid), 0)
 from PROD.pi;

/* region */
insert into GRINGLOBAL.region (region_id, continent, subcontinent, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select regno, area, region, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.reg.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.reg.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.reg.userid), 0)
 from PROD.reg;

/* site */
insert into GRINGLOBAL.site (site_code, site_name, region_code, note, is_distributable, type_code, institution_code, contact_cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select site, coalesce(sitename, 'ADMIN'), arsregion, cmt, coalesce(case when distribute = 'X' then 'Y' else distribute end, 'N'), case site.clonal when 'Y' then 'CLNL' when 'M' then 'BOTH' else 'SEED' end, instcode, cno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.site.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.site.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.site.userid), 0)
 from PROD.site;

/* taxonomy_author */
insert into GRINGLOBAL.taxonomy_author (short_name, full_name, short_name_diacritic, full_name_diacritic, short_name_expanded_diacritic, full_name_expanded_diacritic, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select shortaut, longaut, smarkaut, lmarkaut, shexpaut, lgexpaut, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.taut.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.taut.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.taut.userid), 0)
 from PROD.taut;

/* taxonomy_family */
insert into GRINGLOBAL.taxonomy_family (taxonomy_family_id, current_taxonomy_family_id, family_name, author_name, alternate_name, subfamily, tribe, subtribe, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select famno, validfamno, family, famauthor, altfamily, subfamily, tribe, subtribe, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.fam.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.fam.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.fam.userid), 0)
 from PROD.fam;

/* taxonomy_genus */
insert into GRINGLOBAL.taxonomy_genus (taxonomy_genus_id, current_taxonomy_genus_id, taxonomy_family_id, qualifying_code, is_hybrid, genus_name, genus_authority, subgenus_name, section_name, series_name, subseries_name, subsection_name, alternate_family, common_name, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select gno, validgno, famno, qual, coalesce(case when ghybrid = 'X' then 'Y' else ghybrid end, 'N'), genus, gauthor, subgenus, section, series, subseries, subsection, othfamily, cname, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.gn.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.gn.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.gn.userid), 0)
 from PROD.gn;

/* taxonomy_genus_type */
insert into GRINGLOBAL.taxonomy_genus_type (taxonomy_genus_id, taxonomy_family_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select gno, famno, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.gnt.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.gnt.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.gnt.userid), 0)
 from PROD.gnt;

/* url **************** No previous definition **************** */

/* accession_group */
insert into GRINGLOBAL.accession_group (accession_group_code, note, site_code, url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select agname, cmt, site, url, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ag.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ag.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ag.userid), 0)
 from PROD.ag;

/* citation **************** No previous definition **************** */

/* code_group **************** No previous definition **************** */

/* code_value */
insert into GRINGLOBAL.code_value (code_group_code, value, site_code, is_standard, category_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select code_no, code, site, coalesce(case when std = 'X' then 'Y' else std end, 'N'), cat, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = MAIN.code_value.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = MAIN.code_value.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = MAIN.code_value.userid), 0)
 from MAIN.code_value;

/* code_value_lang **************** No previous definition **************** */

/* crop_trait_code_lang **************** No previous definition **************** */

/* genetic_marker_citation_map */


/***********************************************/
/********* Special Case Code *******************/
/***********************************************/
/* GC uses a separate table for each citation type (mcit, acit, tcit, etc)
   GG uses a single citation table for all citation types, then mapping tables
   to tie them to the proper taxonomy / accession / genetic_marker / etc.
   
   This means the GC citno can not match the GG citno as there might be overlap.
   So we create a temporary table here to hold the values while we migrate data.
   */

DROP TABLE GRINGLOBAL.migration_cit;
DROP SEQUENCE GRINGLOBAL.sq_migration_cit;
DROP TRIGGER GRINGLOBAL.tg_migration_cit;

create table GRINGLOBAL.migration_cit (
	gg_citation_id integer not null,
	gc_citno integer not null,
	gc_tablename varchar2(10 char)
);
CREATE SEQUENCE GRINGLOBAL.sq_migration_cit minvalue 1 start with 1 increment by 1;
CREATE OR REPLACE TRIGGER GRINGLOBAL.tg_migration_cit BEFORE INSERT ON GRINGLOBAL.migration_cit FOR EACH ROW BEGIN IF :NEW.gg_citation_id IS NULL THEN SELECT GRINGLOBAL.sq_migration_cit.NEXTVAL INTO :NEW.gg_citation_id FROM DUAL; END IF; END;
/

/* fill temp table as needed */
insert into GRINGLOBAL.migration_cit (gc_citno, gc_tablename)
select citno, 'mcit' from PROD.mcit;

/* fill citation table with all items related to genetic_marker */
insert into GRINGLOBAL.citation (citation_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.mcit.citno and gc_tablename = 'mcit'),
	(select lit.literature_id from gringlobal.literature lit where lit.abbreviation = PROD.mcit.abbr), 
	cittitle, author, case when cityr is null then null else to_date(to_char(cityr) || '-01-01', 'YYYY-MM-DD') end, citref, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.mcit.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.mcit.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.mcit.userid), 0)
 from PROD.mcit;
 
 
 
 

/* fill map table between citation and genetic_marker */
insert into GRINGLOBAL.genetic_marker_citation_map 
	(genetic_marker_id, citation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	mrkno,
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.mcit.citno and gc_tablename = 'mcit'),
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.mcit.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.mcit.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.mcit.userid), 0)
from
	PROD.mcit;

/* geography */
/*
*/
insert into GRINGLOBAL.geography (
	geography_id, current_geography_id, 
	region_id, country_code, 
	adm1, adm1_type_code, 
	adm2, adm2_type_code, 
	adm3, adm3_type_code, 
	adm4, adm4_type_code, 
	is_valid, 
	changed_date, 
	previous_geography_id, 
	note, 
	created_date, created_by, 
	modified_date, modified_by, 
	owned_date, owned_by)
select 
	geono, validgeono, 
	regno, '???', 
	state, null,
	country, null,
	null, null,
	null, null,
	coalesce(case when cflag = 'X' then 'Y' else cflag end, 'N'), 
	changed, 
	null, 
	cmt, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.geo.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.geo.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.geo.userid), 0)
from PROD.geo;

/* method */
insert into GRINGLOBAL.method (method_id, name, site_code, geography_id, material_or_method_used, study_reason, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	eno, ename, site, geono, methods, studytype, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.eval.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.eval.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.eval.userid), 0)
from PROD.eval;


/* method_citation_map */


/* fill temp table as needed */
insert into GRINGLOBAL.migration_cit (gc_citno, gc_tablename)
select citno, 'ecit' from PROD.ecit;

insert into GRINGLOBAL.citation (citation_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.ecit.citno and gc_tablename = 'ecit'),
	(select lit.literature_id from gringlobal.literature lit where lit.abbreviation = PROD.ecit.abbr), 
	cittitle, author, case when cityr is null then null else to_date(to_char(cityr) || '-01-01', 'YYYY-MM-DD') end, citref, cmt, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ecit.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ecit.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ecit.userid), 0)
 from PROD.ecit;


 
insert into GRINGLOBAL.method_citation_map 
	(method_id, citation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	eno,
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.ecit.citno and gc_tablename = 'ecit'),
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ecit.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ecit.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ecit.userid), 0)
from
	PROD.ecit ;

/* method_map */
insert into GRINGLOBAL.method_map (cooperator_id, method_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select cno, eno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.embr.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.embr.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.embr.userid), 0)
 from PROD.embr;

/* taxonomy */
insert into GRINGLOBAL.taxonomy (taxonomy_id, current_taxonomy_id, is_interspecific_hybrid, species, species_authority, is_intraspecific_hybrid, subspecies, subspecies_authority, is_intervarietal_hybrid, variety, variety_authority, is_subvarietal_hybrid, subvariety, subvariety_authority, is_forma_hybrid, forma, forma_authority, taxonomy_genus_id, crop_id, priority_site_1, priority_site_2, restriction, life_form, common_fertilization, is_name_pending, synonym_code, cooperator_id, name_verified_date, name, name_authority, protologue, note, site_note, alternate_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select taxno, validtaxno, coalesce(case when shybrid = 'X' then 'Y' else shybrid end, 'N'), species, sauthor, coalesce(case when ssphybrid = 'X' then 'Y' else ssphybrid end, 'N'), subsp, sspauthor, coalesce(case when varhybrid = 'X' then 'Y' else varhybrid end, 'N'), var, varauthor, coalesce(case when svhybrid = 'X' then 'Y' else svhybrid end, 'N'), subvar, svauthor, coalesce(case when fhybrid = 'X' then 'Y' else fhybrid end, 'N'), forma, fauthor, gno, cropno, psite1, psite2, rest, lifeform, fert, coalesce(case when pending = 'X' then 'Y' else pending end, 'N'), qual, cno, verified, taxon, taxauthor, protologue, taxcmt, sitecmt, othname, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.tax.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.tax.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.tax.userid), 0)
from PROD.tax;

/* taxonomy_citation_map */

/* fill temp table as needed */
insert into GRINGLOBAL.migration_cit (gc_citno, gc_tablename)
select citno, 'tcit' from PROD.ecit;

insert into GRINGLOBAL.citation (citation_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.tcit.citno and gc_tablename = 'tcit'),
	(select lit.literature_id from gringlobal.literature lit where lit.abbreviation = PROD.tcit.abbr),
	cittitle, author, case when cityr is null then null else to_date(to_char(cityr) || '-01-01', 'YYYY-MM-DD') end, citref, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.tcit.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.tcit.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.tcit.userid), 0)
 from PROD.tcit;


insert into GRINGLOBAL.taxonomy_citation_map 
	(taxonomy_id, citation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	taxno,
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.tcit.citno and gc_tablename = 'tcit'),
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.tcit.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.tcit.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.tcit.userid), 0)
from
	PROD.tcit;

/* taxonomy_common_name */
insert into GRINGLOBAL.taxonomy_common_name (taxonomy_id, name, source, note, simplified_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select taxno, cname, source, cmt, cnid, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.cn.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.cn.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.cn.userid), 0)
 from PROD.cn;

/* taxonomy_distribution */
insert into GRINGLOBAL.taxonomy_distribution (taxonomy_distribution_id, taxonomy_id, geography_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select distno, taxno, geono, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.dist.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.dist.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.dist.userid), 0)
 from PROD.dist;

/* taxonomy_genus_citation_map */

/* fill temp table as needed */
insert into GRINGLOBAL.migration_cit (gc_citno, gc_tablename)
select citno, 'gcit' from PROD.gcit;

insert into GRINGLOBAL.citation (citation_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.gcit.citno and gc_tablename = 'gcit'),
	(select lit.literature_id from gringlobal.literature lit where lit.abbreviation = PROD.gcit.abbr), 
	cittitle, author, case when cityr is null then null else to_date(to_char(cityr) || '-01-01', 'YYYY-MM-DD') end, citref, cmt, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.gcit.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.gcit.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.gcit.userid), 0)
from PROD.gcit;


insert into GRINGLOBAL.taxonomy_genus_citation_map 
	(taxonomy_genus_id, citation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	gno,
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.gcit.citno and gc_tablename = 'gcit'),
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.gcit.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.gcit.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.gcit.userid), 0)
from
	PROD.gcit;

/* taxonomy_germination_rule */
insert into GRINGLOBAL.taxonomy_germination_rule (taxonomy_germination_rule_id, substrata, temperature_range, requirements, author_name, category, days, taxonomy_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select ruleno, substrata, temp, requirements, author, category, days, taxno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.germrule.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.germrule.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.germrule.userid), 0)
 from PROD.germrule;

/* taxonomy_url */

insert into GRINGLOBAL.url 
(caption, url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	caption, 
	url, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.turl.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.turl.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.turl.userid), 0)
from PROD.turl;


insert into GRINGLOBAL.taxonomy_url
	(taxonomy_url_id, url_type, taxonomy_family_id, taxonomy_genus_id, taxonomy_id, url_id, site_code, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	turlno,
	urltype,
	famno,
	gno,
	taxno,
	(select url_id from gringlobal.url url1 where coalesce(url1.caption,'-') = coalesce(prod.turl.caption,'-') and coalesce(url1.url,'-') = coalesce(prod.turl.url,'-')),
	site,
	cmt,
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.turl.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.turl.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.turl.userid), 0)
from
	PROD.turl;

/* taxonomy_use */
insert into GRINGLOBAL.taxonomy_use (taxonomy_id, economic_usage, note, usage_type, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select taxno, taxuse, cmt, usetype, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.uses.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.uses.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.uses.userid), 0)
 from PROD.uses;

    exit;

/* accession_action */
insert into GRINGLOBAL.accession_action (accession_action_id, accession_id, action_name, occurred_date, occurred_date_format, completed_date, completed_date_format, is_visible_from_web, narrative, cooperator_id, method_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select aactno, acid, action, occurred, fmtoccurred, completed, fmtcompleted, coalesce(case when showweb = 'X' then 'Y' else showweb end, 'N'), narr, cno, eno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.aact.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.aact.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.aact.userid), 0)
 from PROD.aact;

/* accession_citation_map */

/* fill temp table as needed */
insert into GRINGLOBAL.migration_cit (gc_citno, gc_tablename)
select citno, 'acit' from PROD.acit;

insert into GRINGLOBAL.citation (citation_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.acit.citno and gc_tablename = 'acit'),
	(select lit.literature_id from gringlobal.literature lit where lit.abbreviation = PROD.acit.abbr), cittitle, author, 
	case when cityr is null then null else to_date(to_char(cityr) || '-01-01', 'YYYY-MM-DD') end, citref, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.acit.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.acit.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.acit.userid), 0)
 from PROD.acit;


insert into GRINGLOBAL.accession_citation_map 
	(accession_id, citation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	acid,
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.acit.citno and gc_tablename = 'acit'),
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.acit.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.acit.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.acit.userid), 0)
from
	PROD.acit;

/* accession_habitat */
insert into GRINGLOBAL.accession_habitat (accession_habitat_id, accession_id, latitude_degrees, latitude_minutes, latitude_seconds, latitude_hemisphere, longitude_degrees, longitude_minutes, longitude_seconds, longitude_hemisphere, elevation_in_meters, quantity_collected, unit_of_quantity_collected, form_material_collected_code, plant_sample_count, locality, habitat_name, note, collection_coordinate_system, gstype, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select srcno, acid, latd, latm, lats, lath, lond, lonm, lons, lonh, elev, quant, units, cform, plants, locality, habitat, cmt, gctype, gstype, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.hab.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.hab.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.hab.userid), 0)
 from PROD.hab;

/* accession_ipr */
insert into GRINGLOBAL.accession_ipr (accession_id, assigned_type, accession_ipr_prefix, accession_ipr_number, crop_name, full_name, issued_date, expired_date, cooperator_id, citation_id, note, site_code, accepted_date, expected_date, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	acid, iprtype, iprid, iprno, iprcrop, iprname, issued, expired, cno, 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.acit.citno and gc_tablename = 'acit'),
	cmt, site, accepted, expected, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ipr.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ipr.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ipr.userid), 0)
 from PROD.ipr;

/* accession_narrative */
insert into GRINGLOBAL.accession_narrative (accession_id, type_code, narrative_body, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select acid, ntype, narr, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.narr.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.narr.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.narr.userid), 0)
 from PROD.narr;

/* accession_pedigree */
insert into GRINGLOBAL.accession_pedigree (accession_id, released_date, released_date_format, citation_id, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	acid, released, datefmt, 
	(select gg_citation_id from gringlobal.migration_cit where gc_citno = prod.acit.citno and gc_tablename = 'acit'), 
	pedigree, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ped.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ped.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ped.userid), 0)
 from PROD.ped;

/* accession_quarantine */
insert into GRINGLOBAL.accession_quarantine (accession_id, quarantine_type, progress_status_code, cooperator_id, entered_date, established_date, expected_release_date, released_date, note, site_code, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select acid, qtype, status, cno, entered, establish, expected, released, cmt, site, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.quar.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.quar.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.quar.userid), 0)
 from PROD.quar;

/* accession_source */
insert into GRINGLOBAL.accession_source (accession_source_id, accession_id, type_code, step_date, step_date_format, is_origin_step, geography_id, note, source_qualifier, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select srcno, acid, srctype, srcdate, datefmt, coalesce(case when origin = 'X' then 'Y' else origin end, 'N'), geono, cmt, srcqual, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.src.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.src.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.src.userid), 0)
 from PROD.src;

/* accession_source_map */
insert into GRINGLOBAL.accession_source_map (accession_source_id, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select srcno, cno, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.smbr.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.smbr.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.smbr.userid), 0)
 from PROD.smbr;

/* code_rule */
insert into GRINGLOBAL.code_rule (code_value_id, site_code, max_length, function_name, is_standard, is_by_category, cateogry_number, category_note, form_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select code_no, site, maxlen, function, coalesce(case when std = 'X' then 'Y' else std end, 'N'), coalesce(case when cat_flag = 'X' then 'Y' else cat_flag end, 'N'), cat_no, cat_cmt, form_name, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.code_rule.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.code_rule.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.code_rule.userid), 0)
 from PROD.code_rule;

/* crop_trait_url */

insert into GRINGLOBAL.url 
(caption, url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	caption, 
	url, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.durl.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.durl.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.durl.userid), 0)
from PROD.durl;


insert into GRINGLOBAL.crop_trait_url
	(url_type, sequence_number, crop_id, crop_trait_id, code, url_id, site_code, note, method_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	urltype,
	seqno,
	cropno,
	dno,
	code,
	(select url_id from gringlobal.url url1 where coalesce(url1.caption,'-') = coalesce(prod.durl.caption,'-') and coalesce(url1.url,'-') = coalesce(prod.durl.url,'-')),
	site,
	cmt,
	eno,
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.durl.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.durl.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.durl.userid), 0)
from
	PROD.durl;

/* genetic_annotation */
insert into GRINGLOBAL.genetic_annotation (genetic_annotation_id, marker_id, method_id, method, scoring_method, control_values, observation_alleles_count, max_gob_alleles, size_alleles, unusual_alleles, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select gano, mrkno, eno, method, scoring_method, control_values, no_obs_alleles, max_gob_alleles, size_alleles, unusual_alleles, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ga.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ga.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ga.userid), 0)
 from PROD.ga;

 
/* accession */
insert into GRINGLOBAL.accession (accession_id, accession_prefix, accession_number, accession_suffix, site_code, inactive_site_code_reason, is_core, is_backed_up, backup_location, life_form, level_of_improvement_code, reproductive_uniformity, initial_material_type, initial_received_date, initial_received_date_format, taxonomy_id, plant_introduction_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select acid, acp, acno, acs, site, whynull, coalesce(case when core = 'X' then 'Y' else core end, 'N'), coalesce(case when backup = 'X' then 'Y' else backup end, 'N'), case acc.backup when 'X' then 'NSSL' when 'D' then 'Domestic non-NPGS' when 'F' then 'Foreign' when 'L' then 'Local on-site' when 'S' then 'Other NPGS site' else null end, lifeform, acimpt, uniform, acform, received, datefmt, taxno, pivol, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.acc.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.acc.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.acc.userid), 0)
 from PROD.acc;


/* inventory */
insert into GRINGLOBAL.inventory (inventory_id, inventory_prefix, inventory_number, inventory_suffix, inventory_type_code, inventory_maint_policy_id, site_code, is_distributable, location_section_1, location_section_2, location_section_3, location_section_4, quantity_on_hand, unit_of_quantity_on_hand, is_debit, distribution_default_form, standard_distribution_quantity, unit_of_distribution, distribution_critical_amount, replenishment_critical_amount, pathogen_status, availability_status, status_note, accession_id, parent_inventory_id, cooperator_id, backup_inventory_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select ivid, ivp, ivno, ivs, ivt, (select new_id from GRINGLOBAL.__inventory_maint_policy where previous_id = (select im_orig.imid from PROD.im im_orig where im_orig.imname = PROD.iv.imname and im_orig.site = PROD.iv.SITE)), site, coalesce(case when distribute = 'X' then 'Y' else distribute end, 'N'), loc1, loc2, loc3, loc4, onhand, munits, coalesce(case when debit = 'X' then 'Y' else debit end, 'N'), dform, dquant, dunits, dcritical, rcritical, pstatus, status, statcmt, acid, parent, cno, backupiv, cmt,
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.iv.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.iv.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.iv.userid), 0)
 from PROD.iv;
 
 
/***********************************************/
/********* Special Case Code *******************/
/***********************************************/
/* GC schema has both accession_id and inventory_id in certain tables.
   GG schema has only inventory_id in these same tables, and relies on 
   a default inventory record existing for all accession records.
   
   This was done to:
   1) enforce integrity (so a given record could not indirectly point at 2 separate accessions)
   2) make reporting sql easier to write and perform better
   3) make resolving a given record to its ancestor accession record only have a single route
   
   To enable this, we insert a 'dummy' inventory record for each accession record giving the inventory record a type code of '**'
   
*/
insert into gringlobal.inventory (
	inventory_prefix,
	inventory_number,
	inventory_suffix,
	inventory_type_code,
	is_distributable,
	is_debit,
	accession_id,
	note,
	created_date,
	created_by,
	owned_date,
	owned_by
)
select
	a.accession_prefix,
	a.accession_number,
	a.accession_suffix,
	'**',
	'N',
	'N',
	a.accession_id,
	'Default Association Record for Accession -> Inventory',
	current_timestamp,
	0,
	current_timestamp,
	0
	current_timestamp,
	0
from
	gringlobal.accession a 
where
	a.accession_id not in 
	(select distinct accession_id from gringlobal.inventory where inventory_type_code = '**');


 
 
 
 
 

/* inventory_action */
insert into GRINGLOBAL.inventory_action (inventory_action_id, action_name, occurred_date, occurred_date_format, quantity, unit_of_quantity, form_involved, inventory_id, cooperator_id, method_id, note, qualifier, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select iactno, action, occurred, datefmt, quant, units, iform, ivid, cno, eno, cmt, iactqual, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.iact.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.iact.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.iact.userid), 0)
 from PROD.iact;

/* inventory_group_map */
insert into GRINGLOBAL.inventory_group_map (inventory_id, inventory_group_id, site_code, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select ivid, (select ig_orig.igid from PROD.ig ig_orig where ig_orig.igname = PROD.igm.igname and ig_orig.site = PROD.igm.SITE), site, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.igm.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.igm.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.igm.userid), 0)
 from PROD.igm;

/* inventory_quality_status */
insert into GRINGLOBAL.inventory_quality_status (inventory_id, test_type, pathogen_code, started_date, finished_date, test_results, needed_count, started_count, completed_count, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select ivid, pttype, ptcode, began, finished, results, needed, started, completed, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.pt.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.pt.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.pt.userid), 0)
 from PROD.pt;

/* inventory_viability */
insert into GRINGLOBAL.inventory_viability (inventory_viability_id, tested_date, tested_date_format, percent_normal, percent_abnormal, percent_dormant, percent_viable, vigor_rating, sample_count, replication_count, inventory_id, method_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select viano, tested, datefmt, norm, abnorm, dormant, viable, vigor, sample, reps, ivid, eno, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.via.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.via.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.via.userid), 0)
 from PROD.via;

/* accession_annotation */
insert into GRINGLOBAL.accession_annotation (accession_annotation_id, action_name, action_date, accession_id, site_code, cooperator_id, inventory_id, order_request_id, old_taxonomy_id, new_taxonomy_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select alno, action, acted, acid, site, cno, 
	coalesce(ivid, (select inventory_id from gringlobal.inventory gg_inv where gg_inv.accession_id = prod.al.acid and gg_inv.inventory_type_code = '**')), 
	orno, oldtaxno, newtaxno, cmt, 
	coalesce(created, current_timestamp), 
	coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.al.userid), 0),
	coalesce(modified, current_timestamp), 
	coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.al.userid), 0),
	coalesce(created, current_timestamp), 
	coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.al.userid), 0)
from PROD.al;

/* accession_voucher */
insert into GRINGLOBAL.accession_voucher (accession_voucher_id, accession_id, voucher_type, inventory_id, cooperator_id, vouchered_date, vouchered_date_format, collector_identifier, caption, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select vno, acid, vtype, ivid, cno, vouchered, datefmt, collid, vcontent, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.vou.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.vou.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.vou.userid), 0)
 from PROD.vou;

/* genetic_observation */
insert into GRINGLOBAL.genetic_observation (genetic_observation_id, genetic_annotation_id, inventory_id, individual, value, genebank_url, image_url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select gobno, gano, ivid, indiv, gob, genbank_link, image_link, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.gob.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.gob.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.gob.userid), 0)
 from PROD.gob;
 
 
 /*****  The following ones are all > 1 million rows... *****/

 /* accession_name */
insert into GRINGLOBAL.accession_name (accession_name_id, accession_id, category, name, name_rank, accession_group_id, inventory_id, cooperator_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select anno, acid, idtype, plantid, idrank, (select ag_orig.agid from PROD.ag ag_orig where ag_orig.agname = PROD.an.agname)), ivid, cno, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.an.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.an.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.an.userid), 0)
 from PROD.an;


 /* order_request_item */
insert into GRINGLOBAL.order_request_item (order_request_item_id, order_request_id, sequence_number, name, quantity_shipped, unit_of_shipped, distribution_form, ipr_restriction, status_code, acted_date, cooperator_id, inventory_id, taxonomy_id, external_taxonomy, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select itno, oino, item, quant, units, dform, rest, status, acted, cno, ivid, taxno, taxon, cmt, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.oi.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.oi.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.oi.userid), 0)
 from PROD.oi;

/* crop_trait_observation */
insert into GRINGLOBAL.crop_trait_observation (crop_trait_id, crop_trait_code_id, numeric_value, string_value, method_id, crop_trait_qualifier_id, inventory_id, original_value, frequency, mean_value, maximum_value, minimum_value, standard_deviation, sample_size, note, rank, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select dno, 
	case when (select count(1) from PROD.dsc where PROD.dsc.dno = PROD.ob.dno and PROD.dsc.usecode = 'X') > 0 then
		(select ctc.crop_trait_code_id from GRINGLOBAL.crop_trait_code ctc where ctc.crop_trait_id = PROD.ob.dno and ctc.code = PROD.ob.ob) 
	else null end,
	null, ob, eno, qno, 
	coalesce(ivid, (select inventory_id from gringlobal.inventory gg_inv where gg_inv.accession_id = prod.ob.acid and gg_inv.inventory_type_code = '**')), 
	orgvalue, freq, mean, high, low, sdev, ssize, cmt, rank, 
coalesce(created, current_timestamp), 
coalesce((select cooperator_id from gringlobal.sec_user su1 where su1.user_name = PROD.ob.userid), 0),
coalesce(modified, current_timestamp), 
coalesce((select su2.cooperator_id from gringlobal.sec_user su2 where su2.user_name = PROD.ob.userid), 0),
coalesce(created, current_timestamp), 
coalesce((select su3.cooperator_id from gringlobal.sec_user su3 where su3.user_name = PROD.ob.userid), 0)
 from PROD.ob;


/***********************************************/
/********* Utility Scripts         *************/
/***********************************************/

/* create table to map old table/column to new table/field */
CREATE TABLE GRINGLOBAL.__old_to_new (
__old_to_new_id INTEGER NOT NULL,
old_table_name varchar2(100) NOT NULL  ,
old_field_name varchar2(100) NOT NULL  ,
new_table_name varchar2(100) NOT NULL  ,
new_field_name varchar2(100) NOT NULL  ,
sec_table_id INTEGER null,
sec_table_field_id int(11) null,
CONSTRAINT pk___old_to_new PRIMARY KEY (__old_to_new_id)
USING INDEX PCTREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.sq___old_to_new minvalue 1 start with 1 increment by 1;
CREATE OR REPLACE TRIGGER GRINGLOBAL.tg___old_to_new BEFORE INSERT ON GRINGLOBAL.__old_to_new FOR EACH ROW BEGIN IF :NEW.__old_to_new_id IS NULL THEN SELECT GRINGLOBAL.sq___old_to_new.NEXTVAL INTO :NEW.__old_to_new_id FROM DUAL; END IF; END;
/


CREATE  INDEX GRINGLOBAL.ndx_otn_old  ON GRINGLOBAL.__old_to_new (old_table_name, old_field_name);
CREATE  INDEX GRINGLOBAL.ndx_otn_new  ON GRINGLOBAL.__old_to_new (new_table_name, new_field_name);
CREATE  INDEX GRINGLOBAL.ndx_otn_st  ON GRINGLOBAL.__old_to_new (sec_table_id);
CREATE  INDEX GRINGLOBAL.ndx_otn_stf  ON GRINGLOBAL.__old_to_new (sec_table_field_id);


/* insert statements for mapping old tablenames to new tablenames */
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'citno', 'citation', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'abbr', 'citation', 'literature_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'cittitle', 'citation', 'title');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'author', 'citation', 'author_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'cityr', 'citation', 'citation_year_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'citref', 'citation', 'reference');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'cmt', 'citation', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'userid', 'citation', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'created', 'citation', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', '', 'citation', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', 'modified', 'citation', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', '', 'citation', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', '', 'citation', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('<this must be at the top of this file!>', '', 'citation', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'aactno', 'accession_action', 'accession_action_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'acid', 'accession_action', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'action', 'accession_action', 'action_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'occurred', 'accession_action', 'occurred_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'fmtoccurred', 'accession_action', 'occurred_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'completed', 'accession_action', 'completed_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'fmtcompleted', 'accession_action', 'completed_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'showweb', 'accession_action', 'is_visible_from_web');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'narr', 'accession_action', 'narrative');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'cno', 'accession_action', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'eno', 'accession_action', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'userid', 'accession_action', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'created', 'accession_action', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', '', 'accession_action', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', 'modified', 'accession_action', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', '', 'accession_action', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', '', 'accession_action', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('aact', '', 'accession_action', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'acid', 'accession', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'acp', 'accession', 'accession_prefix');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'acno', 'accession', 'accession_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'acs', 'accession', 'accession_suffix');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', '', 'accession', 'accession_name_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'site', 'accession', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'whynull', 'accession', 'inactive_site_code_reason');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'core', 'accession', 'is_core');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'backup', 'accession', 'is_backed_up');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', '', 'accession', 'backup_location');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'lifeform', 'accession', 'life_form');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'acimpt', 'accession', 'level_of_improvement_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'uniform', 'accession', 'reproductive_uniformity');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'acform', 'accession', 'initial_material_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'received', 'accession', 'initial_received_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'datefmt', 'accession', 'initial_received_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'taxno', 'accession', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'pivol', 'accession', 'plant_introduction_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'userid', 'accession', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'created', 'accession', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', '', 'accession', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', 'modified', 'accession', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', '', 'accession', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', '', 'accession', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acc', '', 'accession', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'citno', 'accession_citation_map', 'accession_citation_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'acid', 'accession_citation_map', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'abbr', 'accession_citation_map', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'cittitle', 'accession_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'author', 'accession_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'cityr', 'accession_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'citref', 'accession_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'cmt', 'accession_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'userid', 'accession_citation_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'created', 'accession_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', '', 'accession_citation_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', 'modified', 'accession_citation_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', '', 'accession_citation_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', '', 'accession_citation_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', '', 'accession_citation_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('acit', '', 'accession_citation_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'agid', 'accession_group', 'accession_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'agname', 'accession_group', 'accession_group_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'cmt', 'accession_group', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'site', 'accession_group', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'userid', 'accession_group', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'url', 'accession_group', 'url');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'created', 'accession_group', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', '', 'accession_group', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', 'modified', 'accession_group', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', '', 'accession_group', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', '', 'accession_group', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ag', '', 'accession_group', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'alno', 'accession_annotation', 'accession_annotation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'action', 'accession_annotation', 'action_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'acted', 'accession_annotation', 'action_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'acid', 'accession_annotation', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'site', 'accession_annotation', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'cno', 'accession_annotation', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'ivid', 'accession_annotation', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'orno', 'accession_annotation', 'order_request_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'oldtaxno', 'accession_annotation', 'old_taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'newtaxno', 'accession_annotation', 'new_taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'cmt', 'accession_annotation', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'userid', 'accession_annotation', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'created', 'accession_annotation', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', '', 'accession_annotation', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', 'modified', 'accession_annotation', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', '', 'accession_annotation', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', '', 'accession_annotation', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('al', '', 'accession_annotation', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'anno', 'accession_name', 'accession_name_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'acid', 'accession_name', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'idtype', 'accession_name', 'category');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'idrank', 'accession_name', '<no longer needed>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'topname', 'accession_name', '<absorbed by name field>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'plantid', 'accession_name', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'searchid', 'accession_name', '<no longer needed>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'agname', 'accession_name', 'accession_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'ivid', 'accession_name', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'cno', 'accession_name', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'cmt', 'accession_name', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'userid', 'accession_name', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'created', 'accession_name', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', '', 'accession_name', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', 'modified', 'accession_name', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', '', 'accession_name', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', '', 'accession_name', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('an', '', 'accession_name', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', 'app_resource_id', 'app_resource', 'app_resource_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', 'language_code', 'app_resource', 'sec_lang_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', 'app_resource_name', 'app_resource', 'app_resource_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', 'display_member', 'app_resource', 'display_member');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', 'value_member', 'app_resource', 'value_member');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', 'sort_order', 'app_resource', 'sort_order');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', '', 'app_resource', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', '', 'app_resource', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', '', 'app_resource', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', '', 'app_resource', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', '', 'app_resource', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('app_resource', '', 'app_resource', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'cdid', 'crop_trait_code', 'crop_trait_code_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'dno', 'crop_trait_code', 'crop_trait_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'code', 'crop_trait_code', 'code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'def', 'crop_trait_code', '<moved to crop_trait_code_lang>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'userid', 'crop_trait_code', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'created', 'crop_trait_code', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', '', 'crop_trait_code', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', 'modified', 'crop_trait_code', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', '', 'crop_trait_code', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', '', 'crop_trait_code', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cd', '', 'crop_trait_code', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'cgid_int', 'cooperator_group', 'cooperator_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'cgid', 'cooperator_group', 'code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'cgname', 'cooperator_group', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'site', 'cooperator_group', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'historical', 'cooperator_group', 'is_historical');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'cmt', 'cooperator_group', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'userid', 'cooperator_group', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'created', 'cooperator_group', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', '', 'cooperator_group', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', 'modified', 'cooperator_group', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', '', 'cooperator_group', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', '', 'cooperator_group', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cg', '', 'cooperator_group', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'cnid_int', 'taxonomy_common_name', 'taxonomy_common_name_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'taxno', 'taxonomy_common_name', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'cname', 'taxonomy_common_name', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'source', 'taxonomy_common_name', 'source');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'cmt', 'taxonomy_common_name', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'userid', 'taxonomy_common_name', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'cnid', 'taxonomy_common_name', 'simplified_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'created', 'taxonomy_common_name', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', '', 'taxonomy_common_name', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', 'modified', 'taxonomy_common_name', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', '', 'taxonomy_common_name', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', '', 'taxonomy_common_name', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('cn', '', 'taxonomy_common_name', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'code_rule_id', 'code_rule', 'code_rule_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'owner', 'code_rule', '<no longer needed>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'table_name', 'code_rule', '<no longer needed>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'column_name', 'code_rule', '<no longer needed>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'code_no', 'code_rule', 'code_value_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'site', 'code_rule', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'maxlen', 'code_rule', 'max_length');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'function', 'code_rule', 'function_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'std', 'code_rule', 'is_standard');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'cat_flag', 'code_rule', 'is_by_category');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'cat_no', 'code_rule', 'cateogry_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'cat_cmt', 'code_rule', 'category_note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'userid', 'code_rule', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'form_name', 'code_rule', 'form_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'created', 'code_rule', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', '', 'code_rule', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', 'modified', 'code_rule', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', '', 'code_rule', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', '', 'code_rule', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_rule', '', 'code_rule', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'code_value_id', 'code_value', 'code_value_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'code_no', 'code_value', 'code_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'code', 'code_value', 'value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'site', 'code_value', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'std', 'code_value', 'is_standard');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'cat', 'code_value', 'category_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'def', 'code_value', '<moved to code_value_lang table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'cmt', 'code_value', '<moved to code_value_lang table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'userid', 'code_value', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'created', 'code_value', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', '', 'code_value', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', 'modified', 'code_value', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', '', 'code_value', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', '', 'code_value', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_value', '', 'code_value', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'cno', 'cooperator', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'validcno', 'cooperator', 'current_cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'site', 'cooperator', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'lname', 'cooperator', 'last_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'title', 'cooperator', 'title');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'fname', 'cooperator', 'first_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'job', 'cooperator', 'job');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'org', 'cooperator', 'organization');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'orgid', 'cooperator', 'organization_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'add1', 'cooperator', 'address_line1');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'add2', 'cooperator', 'address_line2');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'add3', 'cooperator', 'address_line3');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'city', 'cooperator', 'admin_1');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'zip', 'cooperator', 'admin_2');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'geono', 'cooperator', 'geography_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'phone1', 'cooperator', 'primary_phone');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'phone2', 'cooperator', 'secondary_phone');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'fax', 'cooperator', 'fax');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'email', 'cooperator', 'email');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'active', 'cooperator', 'is_active');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'cat', 'cooperator', 'category_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'arsregion', 'cooperator', 'ars_region');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'discipline', 'cooperator', 'discipline');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'initials', 'cooperator', 'initials');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'coop', 'cooperator', 'full_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'cmt', 'cooperator', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'userid', 'cooperator', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'language_code', 'cooperator', 'sec_lang_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'created', 'cooperator', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', '', 'cooperator', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', 'modified', 'cooperator', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', '', 'cooperator', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', '', 'cooperator', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('coop', '', 'cooperator', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', 'cropno', 'crop', 'crop_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', 'crop', 'crop', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', 'cmt', 'crop', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', 'userid', 'crop', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', 'created', 'crop', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', '', 'crop', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', 'modified', 'crop', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', '', 'crop', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', '', 'crop', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('crop', '', 'crop', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'distno', 'taxonomy_distribution', 'taxonomy_distribution_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'taxno', 'taxonomy_distribution', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'geono', 'taxonomy_distribution', 'geography_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'cmt', 'taxonomy_distribution', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'userid', 'taxonomy_distribution', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'created', 'taxonomy_distribution', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', '', 'taxonomy_distribution', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', 'modified', 'taxonomy_distribution', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', '', 'taxonomy_distribution', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', '', 'taxonomy_distribution', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dist', '', 'taxonomy_distribution', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'dno', 'crop_trait', 'crop_trait_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'dqname', 'crop_trait', 'short_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'dname', 'crop_trait', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'cac', 'crop_trait', 'is_cgc_approved');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'dcat', 'crop_trait', 'category_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'obtype', 'crop_trait', 'data_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'usecode', 'crop_trait', 'is_coded');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'obmaxlen', 'crop_trait', 'max_length');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'obformat', 'crop_trait', 'numeric_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'obmax', 'crop_trait', 'numeric_max');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'obmin', 'crop_trait', 'numeric_min');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'orgtype', 'crop_trait', 'original_value_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'orgformat', 'crop_trait', 'original_value_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'cropno', 'crop_trait', 'crop_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'site', 'crop_trait', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'def', 'crop_trait', 'definition');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'cmt', 'crop_trait', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'userid', 'crop_trait', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'created', 'crop_trait', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', '', 'crop_trait', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', 'modified', 'crop_trait', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', '', 'crop_trait', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', '', 'crop_trait', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('dsc', '', 'crop_trait', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'durlid', 'crop_trait_url', 'crop_trait_url_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'urltype', 'crop_trait_url', 'url_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'seqno', 'crop_trait_url', 'sequence_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'cropno', 'crop_trait_url', 'crop_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'dno', 'crop_trait_url', 'crop_trait_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'code', 'crop_trait_url', 'code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'caption', 'crop_trait_url', '<moved to url table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'url', 'crop_trait_url', 'url_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'site', 'crop_trait_url', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'cmt', 'crop_trait_url', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'userid', 'crop_trait_url', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'eno', 'crop_trait_url', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'created', 'crop_trait_url', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', '', 'crop_trait_url', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', 'modified', 'crop_trait_url', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', '', 'crop_trait_url', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', '', 'crop_trait_url', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('durl', '', 'crop_trait_url', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'citno', 'method_citation_map', 'method_citation_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'eno', 'method_citation_map', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', '', 'method_citation_map', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'abbr', 'method_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'cittitle', 'method_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'author', 'method_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'cityr', 'method_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'citref', 'method_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'cmt', 'method_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'userid', 'method_citation_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'created', 'method_citation_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', '', 'method_citation_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', 'modified', 'method_citation_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', '', 'method_citation_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', '', 'method_citation_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ecit', '', 'method_citation_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', 'embrid', 'method_map', 'method_cooperator_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', 'cno', 'method_map', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', 'eno', 'method_map', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', 'userid', 'method_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', 'created', 'method_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', '', 'method_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', 'modified', 'method_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', '', 'method_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', '', 'method_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('embr', '', 'method_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'eno', 'method', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'ename', 'method', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'site', 'method', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'geono', 'method', 'geography_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'userid', 'method', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'methods', 'method', 'material_or_method_used');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'studytype', 'method', 'study_reason');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'created', 'method', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', '', 'method', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', 'modified', 'method', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', '', 'method', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', '', 'method', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('eval', '', 'method', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'famno', 'taxonomy_family', 'taxonomy_family_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'validfamno', 'taxonomy_family', 'current_taxonomy_family_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'family', 'taxonomy_family', 'family_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'famauthor', 'taxonomy_family', 'author_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'altfamily', 'taxonomy_family', 'alternate_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'subfamily', 'taxonomy_family', 'subfamily');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'tribe', 'taxonomy_family', 'tribe');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'subtribe', 'taxonomy_family', 'subtribe');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'cmt', 'taxonomy_family', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'userid', 'taxonomy_family', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'created', 'taxonomy_family', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', '', 'taxonomy_family', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', 'modified', 'taxonomy_family', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', '', 'taxonomy_family', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', '', 'taxonomy_family', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('fam', '', 'taxonomy_family', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'gano', 'genetic_annotation', 'genetic_annotation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'mrkno', 'genetic_annotation', 'marker_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'eno', 'genetic_annotation', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'method', 'genetic_annotation', 'method');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'scoring_method', 'genetic_annotation', 'scoring_method');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'control_values', 'genetic_annotation', 'control_values');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'no_obs_alleles', 'genetic_annotation', 'observation_alleles_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'max_gob_alleles', 'genetic_annotation', 'max_gob_alleles');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'size_alleles', 'genetic_annotation', 'size_alleles');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'unusual_alleles', 'genetic_annotation', 'unusual_alleles');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'cmt', 'genetic_annotation', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'userid', 'genetic_annotation', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'created', 'genetic_annotation', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', '', 'genetic_annotation', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', 'modified', 'genetic_annotation', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', '', 'genetic_annotation', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', '', 'genetic_annotation', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ga', '', 'genetic_annotation', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'citno', 'taxonomy_genus_citation_map', 'taxonomy_genus_citation_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'gno', 'taxonomy_genus_citation_map', 'taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'abbr', 'taxonomy_genus_citation_map', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'cittitle', 'taxonomy_genus_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'author', 'taxonomy_genus_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'cityr', 'taxonomy_genus_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'citref', 'taxonomy_genus_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'cmt', 'taxonomy_genus_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'userid', 'taxonomy_genus_citation_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'created', 'taxonomy_genus_citation_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', '', 'taxonomy_genus_citation_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', 'modified', 'taxonomy_genus_citation_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', '', 'taxonomy_genus_citation_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', '', 'taxonomy_genus_citation_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gcit', '', 'taxonomy_genus_citation_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'geono', 'geography', 'geography_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'validgeono', 'geography', 'current_geography_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'country', 'geography', 'country_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'state', 'geography', 'state_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'isofull', 'geography', 'country_iso_full_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'isoshort', 'geography', 'country_iso_short_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'statefull', 'geography', 'state_full_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'iso3', 'geography', 'iso_3_char_country_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'iso2', 'geography', 'iso_2_char_country_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'st', 'geography', 'state_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'cflag', 'geography', 'is_valid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'lath', 'geography', 'latitude_hemisphere');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'lonh', 'geography', 'longitude_hemisphere');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'regno', 'geography', 'region_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'changed', 'geography', 'changed_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'oldname', 'geography', 'previous_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'cmt', 'geography', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'userid', 'geography', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'created', 'geography', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', '', 'geography', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', 'modified', 'geography', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', '', 'geography', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', '', 'geography', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('geo', '', 'geography', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'ruleno', 'taxonomy_germination_rule', 'taxonomy_germination_rule_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'substrata', 'taxonomy_germination_rule', 'substrata');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'temp', 'taxonomy_germination_rule', 'temperature_range');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'requirements', 'taxonomy_germination_rule', 'requirements');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'author', 'taxonomy_germination_rule', 'author_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'category', 'taxonomy_germination_rule', 'category');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'days', 'taxonomy_germination_rule', 'days');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'taxno', 'taxonomy_germination_rule', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'userid', 'taxonomy_germination_rule', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'created', 'taxonomy_germination_rule', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', '', 'taxonomy_germination_rule', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', 'modified', 'taxonomy_germination_rule', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', '', 'taxonomy_germination_rule', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', '', 'taxonomy_germination_rule', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('germrule', '', 'taxonomy_germination_rule', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'gno', 'taxonomy_genus', 'taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'validgno', 'taxonomy_genus', 'current_taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'famno', 'taxonomy_genus', 'taxonomy_family_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'qual', 'taxonomy_genus', 'qualifying_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'ghybrid', 'taxonomy_genus', 'is_hybrid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'genus', 'taxonomy_genus', 'genus_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'gauthor', 'taxonomy_genus', 'genus_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'subgenus', 'taxonomy_genus', 'subgenus_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'section', 'taxonomy_genus', 'section_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'series', 'taxonomy_genus', 'series_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'subseries', 'taxonomy_genus', 'subseries_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'subsection', 'taxonomy_genus', 'subsection_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'othfamily', 'taxonomy_genus', 'alternate_family');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'cname', 'taxonomy_genus', 'common_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'cmt', 'taxonomy_genus', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'userid', 'taxonomy_genus', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'created', 'taxonomy_genus', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', '', 'taxonomy_genus', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', 'modified', 'taxonomy_genus', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', '', 'taxonomy_genus', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', '', 'taxonomy_genus', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gn', '', 'taxonomy_genus', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'gnid', 'taxonomy_genus_type', 'taxonomy_genus_type_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'gno', 'taxonomy_genus_type', 'taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'famno', 'taxonomy_genus_type', 'taxonomy_family_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'cmt', 'taxonomy_genus_type', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'userid', 'taxonomy_genus_type', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'created', 'taxonomy_genus_type', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', '', 'taxonomy_genus_type', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', 'modified', 'taxonomy_genus_type', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', '', 'taxonomy_genus_type', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', '', 'taxonomy_genus_type', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gnt', '', 'taxonomy_genus_type', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'gobno', 'genetic_observation', 'genetic_observation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'gano', 'genetic_observation', 'genetic_annotation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'ivid', 'genetic_observation', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'indiv', 'genetic_observation', 'individual');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'gob', 'genetic_observation', 'value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'genbank_link', 'genetic_observation', 'genebank_url');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'image_link', 'genetic_observation', 'image_url');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'userid', 'genetic_observation', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'created', 'genetic_observation', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', '', 'genetic_observation', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', 'modified', 'genetic_observation', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', '', 'genetic_observation', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', '', 'genetic_observation', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('gob', '', 'genetic_observation', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'groups_id', 'app_user_item_list', 'app_user_item_list_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'cno', 'app_user_item_list', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'tabname', 'app_user_item_list', 'tab_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'groupname', 'app_user_item_list', 'list_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'idno', 'app_user_item_list', 'id_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'idnotype', 'app_user_item_list', 'id_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', 'friendlyname', 'app_user_item_list', 'friendly_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', '', 'app_user_item_list', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', '', 'app_user_item_list', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', '', 'app_user_item_list', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', '', 'app_user_item_list', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', '', 'app_user_item_list', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('groups', '', 'app_user_item_list', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'srcno', 'accession_habitat', 'accession_habitat_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'acid', 'accession_habitat', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'latd', 'accession_habitat', 'latitude_degrees');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'latm', 'accession_habitat', 'latitude_minutes');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'lats', 'accession_habitat', 'latitude_seconds');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'lath', 'accession_habitat', 'latitude_hemisphere');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'lond', 'accession_habitat', 'longitude_degrees');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'lonm', 'accession_habitat', 'longitude_minutes');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'lons', 'accession_habitat', 'longitude_seconds');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'lonh', 'accession_habitat', 'longitude_hemisphere');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'elev', 'accession_habitat', 'elevation_in_meters');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'quant', 'accession_habitat', 'quantity_collected');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'units', 'accession_habitat', 'unit_of_quantity_collected');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'cform', 'accession_habitat', 'form_material_collected_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'plants', 'accession_habitat', 'plant_sample_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'locality', 'accession_habitat', 'locality');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'habitat', 'accession_habitat', 'habitat_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'cmt', 'accession_habitat', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'userid', 'accession_habitat', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'gctype', 'accession_habitat', 'collection_coordinate_system');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'gstype', 'accession_habitat', 'gstype');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'created', 'accession_habitat', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', '', 'accession_habitat', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', 'modified', 'accession_habitat', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', '', 'accession_habitat', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', '', 'accession_habitat', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('hab', '', 'accession_habitat', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'iactno', 'inventory_action', 'inventory_action_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'action', 'inventory_action', 'action_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'occurred', 'inventory_action', 'occurred_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'datefmt', 'inventory_action', 'occurred_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'quant', 'inventory_action', 'quantity');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'units', 'inventory_action', 'unit_of_quantity');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'iform', 'inventory_action', 'form_involved');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'ivid', 'inventory_action', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'cno', 'inventory_action', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'eno', 'inventory_action', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'cmt', 'inventory_action', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'userid', 'inventory_action', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'iactqual', 'inventory_action', 'qualifier');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'created', 'inventory_action', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', '', 'inventory_action', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', 'modified', 'inventory_action', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', '', 'inventory_action', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', '', 'inventory_action', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iact', '', 'inventory_action', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'igid', 'inventory_group', 'inventory_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'igname', 'inventory_group', 'group_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'site', 'inventory_group', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'cmt', 'inventory_group', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'userid', 'inventory_group', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'created', 'inventory_group', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', '', 'inventory_group', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', 'modified', 'inventory_group', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', '', 'inventory_group', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', '', 'inventory_group', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ig', '', 'inventory_group', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'igmid', 'inventory_group_map', 'inventory_group_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'ivid', 'inventory_group_map', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'igname', 'inventory_group_map', 'inventory_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'site', 'inventory_group_map', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'cmt', 'inventory_group_map', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'userid', 'inventory_group_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'created', 'inventory_group_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', '', 'inventory_group_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', 'modified', 'inventory_group_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', '', 'inventory_group_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', '', 'inventory_group_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('igm', '', 'inventory_group_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'imid', 'inventory_maint_policy', 'inventory_maint_policy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'imname', 'inventory_maint_policy', 'maintenance_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'site', 'inventory_maint_policy', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'ivt', 'inventory_maint_policy', 'inventory_default_form');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'munits', 'inventory_maint_policy', 'unit_of_maintenance');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'debit', 'inventory_maint_policy', 'is_debit');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'dform', 'inventory_maint_policy', 'distribution_default_form');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'dquant', 'inventory_maint_policy', 'standard_distribution_quantity');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'dunits', 'inventory_maint_policy', 'unit_of_distribution');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'dcritical', 'inventory_maint_policy', 'distribution_critical_amount');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'rcritical', 'inventory_maint_policy', 'replenishment_critical_amount');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'regen', 'inventory_maint_policy', 'regeneration_method');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'ptests', 'inventory_maint_policy', 'standard_pathogen_test_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'cmt', 'inventory_maint_policy', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'userid', 'inventory_maint_policy', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'cno', 'inventory_maint_policy', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'created', 'inventory_maint_policy', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', '', 'inventory_maint_policy', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', 'modified', 'inventory_maint_policy', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', '', 'inventory_maint_policy', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', '', 'inventory_maint_policy', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('im', '', 'inventory_maint_policy', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'iprid_int', 'accession_ipr', 'accession_ipr_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'acid', 'accession_ipr', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'iprtype', 'accession_ipr', 'assigned_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'iprid', 'accession_ipr', 'accession_ipr_prefix');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'iprno', 'accession_ipr', 'accession_ipr_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'iprcrop', 'accession_ipr', 'crop_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'iprname', 'accession_ipr', 'full_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'issued', 'accession_ipr', 'issued_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'expired', 'accession_ipr', 'expired_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'cno', 'accession_ipr', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'citno', 'accession_ipr', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'cmt', 'accession_ipr', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'site', 'accession_ipr', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'userid', 'accession_ipr', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'accepted', 'accession_ipr', 'accepted_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'expected', 'accession_ipr', 'expected_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'created', 'accession_ipr', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', '', 'accession_ipr', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', 'modified', 'accession_ipr', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', '', 'accession_ipr', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', '', 'accession_ipr', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ipr', '', 'accession_ipr', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'ivid', 'inventory', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'ivp', 'inventory', 'inventory_prefix');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'ivno', 'inventory', 'inventory_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'ivs', 'inventory', 'inventory_suffix');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'ivt', 'inventory', 'inventory_type_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'imname', 'inventory', 'inventory_maint_policy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'site', 'inventory', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'distribute', 'inventory', 'is_distributable');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'loc1', 'inventory', 'location_section_1');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'loc2', 'inventory', 'location_section_2');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'loc3', 'inventory', 'location_section_3');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'loc4', 'inventory', 'location_section_4');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'onhand', 'inventory', 'quantity_on_hand');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'munits', 'inventory', 'unit_of_quantity_on_hand');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'debit', 'inventory', 'is_debit');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'dform', 'inventory', 'distribution_default_form');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'dquant', 'inventory', 'standard_distribution_quantity');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'dunits', 'inventory', 'unit_of_distribution');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'dcritical', 'inventory', 'distribution_critical_amount');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'rcritical', 'inventory', 'replenishment_critical_amount');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'pstatus', 'inventory', 'pathogen_status');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'status', 'inventory', 'availability_status');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'statcmt', 'inventory', 'status_note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'acid', 'inventory', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'parent', 'inventory', 'parent_inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'cno', 'inventory', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'backupiv', 'inventory', 'backup_inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'cmt', 'inventory', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'userid', 'inventory', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'created', 'inventory', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', '', 'inventory', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', 'modified', 'inventory', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', '', 'inventory', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', '', 'inventory', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('iv', '', 'inventory', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'litid', 'literature', 'literature_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'abbr', 'literature', 'abbreviation');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'stdabbr', 'literature', 'standard_abbreviation');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'reftitle', 'literature', 'reference_title');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'editor', 'literature', 'author_editor_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'cmt', 'literature', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'site', 'literature', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'userid', 'literature', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'created', 'literature', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', '', 'literature', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', 'modified', 'literature', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', '', 'literature', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', '', 'literature', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('lit', '', 'literature', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'mbrid', 'cooperator_map', 'cooperator_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'cno', 'cooperator_map', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'cgid', 'cooperator_map', 'cooperator_group_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'cmt', 'cooperator_map', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'userid', 'cooperator_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'localid', 'cooperator_map', 'localid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'created', 'cooperator_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', '', 'cooperator_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', 'modified', 'cooperator_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', '', 'cooperator_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', '', 'cooperator_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mbr', '', 'cooperator_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'citno', 'genetic_marker_citation_map', 'genetic_marker_citation_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'mrkno', 'genetic_marker_citation_map', 'genetic_marker_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'abbr', 'genetic_marker_citation_map', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'cittitle', 'genetic_marker_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'author', 'genetic_marker_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'cityr', 'genetic_marker_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'citref', 'genetic_marker_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'cmt', 'genetic_marker_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'userid', 'genetic_marker_citation_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'created', 'genetic_marker_citation_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', '', 'genetic_marker_citation_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', 'modified', 'genetic_marker_citation_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', '', 'genetic_marker_citation_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', '', 'genetic_marker_citation_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mcit', '', 'genetic_marker_citation_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'mrkno', 'genetic_marker', 'genetic_marker_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'cropno', 'genetic_marker', 'crop_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'site', 'genetic_marker', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'marker', 'genetic_marker', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'synonyms', 'genetic_marker', 'synonyms');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'repeat_motif', 'genetic_marker', 'repeat_motif');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'primers', 'genetic_marker', 'primers');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'assay_conditions', 'genetic_marker', 'assay_conditions');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'range_products', 'genetic_marker', 'range_products');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'known_standards', 'genetic_marker', 'known_standards');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'genbank_no', 'genetic_marker', 'genebank_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'map_location', 'genetic_marker', 'map_location');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'position', 'genetic_marker', 'position');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'cmt', 'genetic_marker', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'poly_type', 'genetic_marker', 'poly_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'userid', 'genetic_marker', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'created', 'genetic_marker', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', '', 'genetic_marker', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', 'modified', 'genetic_marker', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', '', 'genetic_marker', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', '', 'genetic_marker', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('mrk', '', 'genetic_marker', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'narr_id', 'accession_narrative', 'accession_narrative_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'acid', 'accession_narrative', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'ntype', 'accession_narrative', 'type_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'userid', 'accession_narrative', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'narr', 'accession_narrative', 'narrative_body');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'created', 'accession_narrative', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', '', 'accession_narrative', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', 'modified', 'accession_narrative', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', '', 'accession_narrative', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', '', 'accession_narrative', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('narr', '', 'accession_narrative', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'oactno', 'order_request_action', 'order_request_action_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'action', 'order_request_action', 'action_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'acted', 'order_request_action', 'acted_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'actid', 'order_request_action', 'action_for_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'orno', 'order_request_action', 'order_request_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'site', 'order_request_action', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'cmt', 'order_request_action', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'userid', 'order_request_action', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'coop', 'order_request_action', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'cno', 'order_request_action', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'created', 'order_request_action', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', '', 'order_request_action', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', 'modified', 'order_request_action', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', '', 'order_request_action', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', '', 'order_request_action', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oact', '', 'order_request_action', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'obno', 'crop_trait_observation', 'crop_trait_observation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'dno', 'crop_trait_observation', 'crop_trait_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', '', 'crop_trait_observation', 'crop_trait_code_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', '', 'crop_trait_observation', 'numeric_value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'ob', 'crop_trait_observation', 'string_value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'acid', 'crop_trait_observation', '<no longer used. dummy inventory records created instead.>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'eno', 'crop_trait_observation', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'qno', 'crop_trait_observation', 'crop_trait_qualifier_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'ivid', 'crop_trait_observation', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'orgvalue', 'crop_trait_observation', 'original_value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'freq', 'crop_trait_observation', 'frequency');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'mean', 'crop_trait_observation', 'mean_value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'high', 'crop_trait_observation', 'maximum_value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'low', 'crop_trait_observation', 'minimum_value');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'sdev', 'crop_trait_observation', 'standard_deviation');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'ssize', 'crop_trait_observation', 'sample_size');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'cmt', 'crop_trait_observation', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'userid', 'crop_trait_observation', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'rank', 'crop_trait_observation', 'rank');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'created', 'crop_trait_observation', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', '', 'crop_trait_observation', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', 'modified', 'crop_trait_observation', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', '', 'crop_trait_observation', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', '', 'crop_trait_observation', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ob', '', 'crop_trait_observation', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'itno', 'order_request_item', 'order_request_item_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'orno', 'order_request_item', 'order_request_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'oino', 'order_request_item', 'sequence_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'item', 'order_request_item', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'quant', 'order_request_item', 'quantity_shipped');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'units', 'order_request_item', 'unit_of_shipped');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'dform', 'order_request_item', 'distribution_form');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'rest', 'order_request_item', 'ipr_restriction');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'status', 'order_request_item', 'status_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'acted', 'order_request_item', 'acted_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'cno', 'order_request_item', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'ivid', 'order_request_item', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'acid', 'order_request_item', '<no longer used. dummy inventory records created instead.>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'taxno', 'order_request_item', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'taxon', 'order_request_item', 'external_taxonomy');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'cmt', 'order_request_item', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'userid', 'order_request_item', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'created', 'order_request_item', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', '', 'order_request_item', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', 'modified', 'order_request_item', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', '', 'order_request_item', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', '', 'order_request_item', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('oi', '', 'order_request_item', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'orno', 'order_request', 'order_request_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'origno', 'order_request', 'original_order_request_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'site', 'order_request', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'localno', 'order_request', 'local_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'ortype', 'order_request', 'order_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'ordered', 'order_request', 'ordered_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'status', 'order_request', 'status');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'done', 'order_request', 'is_completed');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'acted', 'order_request', 'acted_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'source', 'order_request', 'source_cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'orderer', 'order_request', 'requestor_cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'shipto', 'order_request', 'ship_to_cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'final', 'order_request', 'final_recipient_cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'reqref', 'order_request', 'order_obtained_via');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'supplylow', 'order_request', 'is_supply_low');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'cmt', 'order_request', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'userid', 'order_request', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'request', 'order_request', 'special_instruction');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'created', 'order_request', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', '', 'order_request', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', 'modified', 'order_request', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', '', 'order_request', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', '', 'order_request', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ord', '', 'order_request', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'pedid', 'accession_pedigree', 'accession_pedigree_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'acid', 'accession_pedigree', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'released', 'accession_pedigree', 'released_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'datefmt', 'accession_pedigree', 'released_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'citno', 'accession_pedigree', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'userid', 'accession_pedigree', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'pedigree', 'accession_pedigree', 'description');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'created', 'accession_pedigree', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', '', 'accession_pedigree', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', 'modified', 'accession_pedigree', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', '', 'accession_pedigree', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', '', 'accession_pedigree', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('ped', '', 'accession_pedigree', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', 'pivol', 'plant_introduction', 'plant_introduction_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', 'piyear', 'plant_introduction', 'plant_introduction_year_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', 'lowpi', 'plant_introduction', 'lowest_pi_number');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', 'userid', 'plant_introduction', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', 'created', 'plant_introduction', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', '', 'plant_introduction', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', 'modified', 'plant_introduction', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', '', 'plant_introduction', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', '', 'plant_introduction', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pi', '', 'plant_introduction', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'ptid', 'inventory_quality_status', 'inventory_quality_status_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'ivid', 'inventory_quality_status', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'pttype', 'inventory_quality_status', 'test_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'ptcode', 'inventory_quality_status', 'pathogen_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'began', 'inventory_quality_status', 'started_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'finished', 'inventory_quality_status', 'finished_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'results', 'inventory_quality_status', 'test_results');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'needed', 'inventory_quality_status', 'needed_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'started', 'inventory_quality_status', 'started_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'completed', 'inventory_quality_status', 'completed_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'cmt', 'inventory_quality_status', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'userid', 'inventory_quality_status', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'created', 'inventory_quality_status', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', '', 'inventory_quality_status', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', 'modified', 'inventory_quality_status', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', '', 'inventory_quality_status', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', '', 'inventory_quality_status', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('pt', '', 'inventory_quality_status', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'qno', 'crop_trait_qualifier', 'crop_trait_qualifier_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'dno', 'crop_trait_qualifier', 'crop_trait_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'qual', 'crop_trait_qualifier', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'def', 'crop_trait_qualifier', 'definition');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'userid', 'crop_trait_qualifier', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'created', 'crop_trait_qualifier', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', '', 'crop_trait_qualifier', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', 'modified', 'crop_trait_qualifier', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', '', 'crop_trait_qualifier', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', '', 'crop_trait_qualifier', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('qual', '', 'crop_trait_qualifier', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'quarid', 'accession_quarantine', 'accession_quarantine_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'acid', 'accession_quarantine', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'qtype', 'accession_quarantine', 'quarantine_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'status', 'accession_quarantine', 'progress_status_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'cno', 'accession_quarantine', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'entered', 'accession_quarantine', 'entered_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'establish', 'accession_quarantine', 'established_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'expected', 'accession_quarantine', 'expected_release_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'released', 'accession_quarantine', 'released_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'cmt', 'accession_quarantine', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'site', 'accession_quarantine', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'userid', 'accession_quarantine', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'created', 'accession_quarantine', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', '', 'accession_quarantine', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', 'modified', 'accession_quarantine', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', '', 'accession_quarantine', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', '', 'accession_quarantine', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('quar', '', 'accession_quarantine', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'regno', 'region', 'region_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'area', 'region', 'continent');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'region', 'region', 'subcontinent');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'cmt', 'region', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'userid', 'region', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'created', 'region', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', '', 'region', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', 'modified', 'region', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', '', 'region', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', '', 'region', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('reg', '', 'region', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'smbrid', 'accession_source_map', 'accession_source_map_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'srcno', 'accession_source_map', 'accession_source_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'acid', 'accession_source_map', '<moved to accession_source table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'cno', 'accession_source_map', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'userid', 'accession_source_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'created', 'accession_source_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', '', 'accession_source_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', 'modified', 'accession_source_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', '', 'accession_source_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', '', 'accession_source_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('smbr', '', 'accession_source_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'srcno', 'accession_source', 'accession_source_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'acid', 'accession_source', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'srctype', 'accession_source', 'type_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'srcdate', 'accession_source', 'step_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'datefmt', 'accession_source', 'step_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'origin', 'accession_source', 'is_origin_step');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'geono', 'accession_source', 'geography_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'cmt', 'accession_source', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'userid', 'accession_source', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'srcqual', 'accession_source', 'source_qualifier');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'created', 'accession_source', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', '', 'accession_source', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', 'modified', 'accession_source', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', '', 'accession_source', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', '', 'accession_source', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('src', '', 'accession_source', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'tautid', 'taxonomy_author', 'taxonomy_author_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'shortaut', 'taxonomy_author', 'short_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'longaut', 'taxonomy_author', 'full_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'smarkaut', 'taxonomy_author', 'short_name_diacritic');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'lmarkaut', 'taxonomy_author', 'full_name_diacritic');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'shexpaut', 'taxonomy_author', 'short_name_expanded_diacritic');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'lgexpaut', 'taxonomy_author', 'full_name_expanded_diacritic');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'cmt', 'taxonomy_author', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'userid', 'taxonomy_author', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'created', 'taxonomy_author', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', '', 'taxonomy_author', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', 'modified', 'taxonomy_author', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', '', 'taxonomy_author', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', '', 'taxonomy_author', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taut', '', 'taxonomy_author', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'taxno', 'taxonomy', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'validtaxno', 'taxonomy', 'current_taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'shybrid', 'taxonomy', 'is_interspecific_hybrid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'species', 'taxonomy', 'species');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'sauthor', 'taxonomy', 'species_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'ssphybrid', 'taxonomy', 'is_intraspecific_hybrid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'subsp', 'taxonomy', 'subspecies');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'sspauthor', 'taxonomy', 'subspecies_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'varhybrid', 'taxonomy', 'is_intervarietal_hybrid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'var', 'taxonomy', 'variety');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'varauthor', 'taxonomy', 'variety_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'svhybrid', 'taxonomy', 'is_subvarietal_hybrid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'subvar', 'taxonomy', 'subvariety');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'svauthor', 'taxonomy', 'subvariety_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'fhybrid', 'taxonomy', 'is_forma_hybrid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'forma', 'taxonomy', 'forma');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'fauthor', 'taxonomy', 'forma_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'gno', 'taxonomy', 'taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'cropno', 'taxonomy', 'crop_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'psite1', 'taxonomy', 'priority_site_1');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'psite2', 'taxonomy', 'priority_site_2');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'rest', 'taxonomy', 'restriction');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'lifeform', 'taxonomy', 'life_form');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'fert', 'taxonomy', 'common_fertilization');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'pending', 'taxonomy', 'is_name_pending');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'qual', 'taxonomy', 'synonym_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'cno', 'taxonomy', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'verified', 'taxonomy', 'name_verified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'taxon', 'taxonomy', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'taxauthor', 'taxonomy', 'name_authority');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'protologue', 'taxonomy', 'protologue');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'taxcmt', 'taxonomy', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'sitecmt', 'taxonomy', 'site_note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'userid', 'taxonomy', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'othname', 'taxonomy', 'alternate_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'created', 'taxonomy', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', '', 'taxonomy', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', 'modified', 'taxonomy', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', '', 'taxonomy', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', '', 'taxonomy', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tax', '', 'taxonomy', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'taxtorcid', 'taxonomy_??????', 'taxonomy_???_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'taxno', 'taxonomy_??????', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'dno', 'taxonomy_??????', 'crop_trait_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'ct', 'taxonomy_??????', 'total_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'taxon', 'taxonomy_??????', '<not in new database>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'crop', 'taxonomy_??????', '<not in new database>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'cropno', 'taxonomy_??????', 'crop_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'genus', 'taxonomy_??????', 'taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'created', 'taxonomy_??????', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', '', 'taxonomy_??????', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', 'modified', 'taxonomy_??????', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', '', 'taxonomy_??????', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', '', 'taxonomy_??????', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('taxtorc', '', 'taxonomy_??????', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'citno', 'taxonomy_citation_map', 'taxonomy_citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'taxno', 'taxonomy_citation_map', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'abbr', 'taxonomy_citation_map', 'citation_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'cittitle', 'taxonomy_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'author', 'taxonomy_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'cityr', 'taxonomy_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'citref', 'taxonomy_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'cmt', 'taxonomy_citation_map', '<moved to citation table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'userid', 'taxonomy_citation_map', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'created', 'taxonomy_citation_map', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', '', 'taxonomy_citation_map', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', 'modified', 'taxonomy_citation_map', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', '', 'taxonomy_citation_map', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', '', 'taxonomy_citation_map', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('tcit', '', 'taxonomy_citation_map', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'turlno', 'taxonomy_url', 'taxonomy_url_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'urltype', 'taxonomy_url', 'url_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'famno', 'taxonomy_url', 'taxonomy_family_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'gno', 'taxonomy_url', 'taxonomy_genus_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'taxno', 'taxonomy_url', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'caption', 'taxonomy_url', '<moved to url table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'url', 'taxonomy_url', 'url_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'site', 'taxonomy_url', 'site_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'cmt', 'taxonomy_url', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'userid', 'taxonomy_url', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'created', 'taxonomy_url', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', '', 'taxonomy_url', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', 'modified', 'taxonomy_url', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', '', 'taxonomy_url', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', '', 'taxonomy_url', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('turl', '', 'taxonomy_url', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'usesid', 'taxonomy_use', 'taxonomy_use_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'taxno', 'taxonomy_use', 'taxonomy_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'taxuse', 'taxonomy_use', 'economic_usage');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'cmt', 'taxonomy_use', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'userid', 'taxonomy_use', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'usetype', 'taxonomy_use', 'usage_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'created', 'taxonomy_use', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', '', 'taxonomy_use', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', 'modified', 'taxonomy_use', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', '', 'taxonomy_use', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', '', 'taxonomy_use', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('uses', '', 'taxonomy_use', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'viano', 'inventory_viability', 'inventory_viability_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'tested', 'inventory_viability', 'tested_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'datefmt', 'inventory_viability', 'tested_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'norm', 'inventory_viability', 'percent_normal');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'abnorm', 'inventory_viability', 'percent_abnormal');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'dormant', 'inventory_viability', 'percent_dormant');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'viable', 'inventory_viability', 'percent_viable');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'vigor', 'inventory_viability', 'vigor_rating');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'sample', 'inventory_viability', 'sample_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'reps', 'inventory_viability', 'replication_count');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'ivid', 'inventory_viability', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'eno', 'inventory_viability', 'method_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'cmt', 'inventory_viability', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'userid', 'inventory_viability', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'created', 'inventory_viability', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', '', 'inventory_viability', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', 'modified', 'inventory_viability', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', '', 'inventory_viability', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', '', 'inventory_viability', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('via', '', 'inventory_viability', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'vno', 'accession_voucher', 'accession_voucher_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'acid', 'accession_voucher', 'accession_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'vtype', 'accession_voucher', 'voucher_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'ivid', 'accession_voucher', 'inventory_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'cno', 'accession_voucher', 'cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'vouchered', 'accession_voucher', 'vouchered_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'datefmt', 'accession_voucher', 'vouchered_date_format');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'collid', 'accession_voucher', 'collector_identifier');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'vloc', 'accession_voucher', '<moved to image table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'vcontent', 'accession_voucher', 'caption');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'cmt', 'accession_voucher', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'userid', 'accession_voucher', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'thumbnail', 'accession_voucher', '<moved to image table>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'created', 'accession_voucher', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', '', 'accession_voucher', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', 'modified', 'accession_voucher', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', '', 'accession_voucher', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', '', 'accession_voucher', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('vou', '', 'accession_voucher', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', '', 'site', 'site_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'site', 'site', 'code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'sitename', 'site', 'name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'arsregion', 'site', 'region');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'cmt', 'site', 'note');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'userid', 'site', '<absorbed by 3 amigos>');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'distribute', 'site', 'can_distribute');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'clonal', 'site', 'site_type');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'instcode', 'site', 'institution_code');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'cno', 'site', 'contact_cooperator_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'created', 'site', 'created_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', '', 'site', 'created_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', 'modified', 'site', 'modified_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', '', 'site', 'modified_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', '', 'site', 'owned_date');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('site', '', 'site', 'owned_by');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'code_column_id', 'code_column', 'code_column_id');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'owner', 'code_column', 'owner');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'table_name', 'code_column', 'table_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'column_name', 'code_column', 'column_name');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'code_no', 'code_column', 'code_no');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'site', 'code_column', 'site');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'userid', 'code_column', 'userid');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'created', 'code_column', 'created');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'modified', 'code_column', 'modified');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'master', 'code_column', 'master');
INSERT INTO GRINGLOBAL.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES ('code_column', 'form_name', 'code_column', 'form_name');

