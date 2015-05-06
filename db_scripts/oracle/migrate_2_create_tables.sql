 /***********************************************/
/*************** Table Definitions *************/
/***********************************************/

/************ Table Definition for GRINGLOBAL.app_setting *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.app_setting ...') as Action from dual;
CREATE TABLE GRINGLOBAL.app_setting (
app_setting_id INTEGER NOT NULL ,
category_code VARCHAR2(50 CHAR) NULL ,
sort_order INTEGER NULL ,
name VARCHAR2(50 CHAR) NOT NULL ,
value VARCHAR2(500 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_APP_SETTING PRIMARY KEY (app_setting_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_APP_SETTING MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_APP_SETTING BEFORE INSERT ON GRINGLOBAL.APP_SETTING FOR EACH ROW BEGIN IF :NEW.app_setting_id IS NULL THEN SELECT GRINGLOBAL.SQ_APP_SETTING.NEXTVAL INTO :NEW.app_setting_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.app_user_item_list *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.app_user_item_list ...') as Action from dual;
CREATE TABLE GRINGLOBAL.app_user_item_list (
app_user_item_list_id INTEGER NOT NULL ,
cooperator_id INTEGER NOT NULL ,
tab_name VARCHAR2(100 CHAR) NOT NULL ,
list_name VARCHAR2(100 CHAR) NOT NULL ,
id_number INTEGER NOT NULL ,
id_type VARCHAR2(100 CHAR) NOT NULL ,
friendly_name VARCHAR2(1000 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_APP_USER_ITEM_LIST PRIMARY KEY (app_user_item_list_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_APP_USER_ITEM_LIST MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_APP_USER_ITEM_LIST BEFORE INSERT ON GRINGLOBAL.APP_USER_ITEM_LIST FOR EACH ROW BEGIN IF :NEW.app_user_item_list_id IS NULL THEN SELECT GRINGLOBAL.SQ_APP_USER_ITEM_LIST.NEXTVAL INTO :NEW.app_user_item_list_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_db *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_db ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_db (
sec_db_id INTEGER NOT NULL ,
migration_number INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DB PRIMARY KEY (sec_db_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DB MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DB BEFORE INSERT ON GRINGLOBAL.SEC_DB FOR EACH ROW BEGIN IF :NEW.sec_db_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DB.NEXTVAL INTO :NEW.sec_db_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_db_migration *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_db_migration ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_db_migration (
sec_db_migration_id INTEGER NOT NULL ,
migration_number INTEGER NOT NULL ,
sort_order INTEGER NOT NULL ,
action_type VARCHAR2(50 CHAR) NOT NULL ,
action_up CLOB NULL ,
action_down CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DB_MIGRATION PRIMARY KEY (sec_db_migration_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DB_MIGRATION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DB_MIGRATION BEFORE INSERT ON GRINGLOBAL.SEC_DB_MIGRATION FOR EACH ROW BEGIN IF :NEW.sec_db_migration_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DB_MIGRATION.NEXTVAL INTO :NEW.sec_db_migration_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_db_migration_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_db_migration_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_db_migration_lang (
sec_db_migration_lang_id INTEGER NOT NULL ,
sec_db_migration_id INTEGER NOT NULL ,
language_iso_639_3_code VARCHAR2(5 CHAR) NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DB_MIGRATION_LANG PRIMARY KEY (sec_db_migration_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DB_MIGRATION_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DB_MIGRATION_LANG BEFORE INSERT ON GRINGLOBAL.SEC_DB_MIGRATION_LANG FOR EACH ROW BEGIN IF :NEW.sec_db_migration_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DB_MIGRATION_LANG.NEXTVAL INTO :NEW.sec_db_migration_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_file *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_file ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_file (
sec_file_id INTEGER NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
virtual_file_path VARCHAR2(255 CHAR) NOT NULL ,
file_name VARCHAR2(255 CHAR) NULL ,
file_version VARCHAR2(255 CHAR) NULL ,
file_size DECIMAL(18, 0) NULL ,
display_name VARCHAR2(255 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_FILE PRIMARY KEY (sec_file_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_FILE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_FILE BEFORE INSERT ON GRINGLOBAL.SEC_FILE FOR EACH ROW BEGIN IF :NEW.sec_file_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_FILE.NEXTVAL INTO :NEW.sec_file_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_file_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_file_group ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_file_group (
sec_file_group_id INTEGER NOT NULL ,
group_name VARCHAR2(100 CHAR) NOT NULL ,
version_name VARCHAR2(50 CHAR) NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_FILE_GROUP PRIMARY KEY (sec_file_group_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_FILE_GROUP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_FILE_GROUP BEFORE INSERT ON GRINGLOBAL.SEC_FILE_GROUP FOR EACH ROW BEGIN IF :NEW.sec_file_group_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_FILE_GROUP.NEXTVAL INTO :NEW.sec_file_group_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_file_group_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_file_group_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_file_group_map (
sec_file_group_map_id INTEGER NOT NULL ,
sec_file_group_id INTEGER NOT NULL ,
sec_file_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_FILE_GROUP_MAP PRIMARY KEY (sec_file_group_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_FILE_GROUP_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_FILE_GROUP_MAP BEFORE INSERT ON GRINGLOBAL.SEC_FILE_GROUP_MAP FOR EACH ROW BEGIN IF :NEW.sec_file_group_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_FILE_GROUP_MAP.NEXTVAL INTO :NEW.sec_file_group_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_group ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_group (
sec_group_id INTEGER NOT NULL ,
group_code VARCHAR2(50 CHAR) NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_GROUP PRIMARY KEY (sec_group_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_GROUP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_GROUP BEFORE INSERT ON GRINGLOBAL.SEC_GROUP FOR EACH ROW BEGIN IF :NEW.sec_group_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_GROUP.NEXTVAL INTO :NEW.sec_group_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_lang (
sec_lang_id INTEGER NOT NULL ,
iso_639_3_code VARCHAR2(5 CHAR) NOT NULL ,
ietf_tag VARCHAR2(30 CHAR) NULL ,
script_direction VARCHAR2(3 CHAR) NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_LANG PRIMARY KEY (sec_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_LANG BEFORE INSERT ON GRINGLOBAL.SEC_LANG FOR EACH ROW BEGIN IF :NEW.sec_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_LANG.NEXTVAL INTO :NEW.sec_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.cooperator *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.cooperator ...') as Action from dual;
CREATE TABLE GRINGLOBAL.cooperator (
cooperator_id INTEGER NOT NULL ,
current_cooperator_id INTEGER NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
last_name VARCHAR2(100 CHAR) NULL ,
title VARCHAR2(10 CHAR) NULL ,
first_name VARCHAR2(100 CHAR) NULL ,
job VARCHAR2(100 CHAR) NULL ,
organization VARCHAR2(100 CHAR) NULL ,
organization_code VARCHAR2(10 CHAR) NULL ,
address_line1 VARCHAR2(100 CHAR) NULL ,
address_line2 VARCHAR2(100 CHAR) NULL ,
address_line3 VARCHAR2(100 CHAR) NULL ,
city VARCHAR2(100 CHAR) NULL ,
postal_index VARCHAR2(100 CHAR) NULL ,
geography_id INTEGER NULL ,
primary_phone VARCHAR2(30 CHAR) NULL ,
secondary_phone VARCHAR2(30 CHAR) NULL ,
fax VARCHAR2(30 CHAR) NULL ,
email VARCHAR2(100 CHAR) NULL ,
is_active CHAR(1 CHAR) NOT NULL ,
category_code VARCHAR2(4 CHAR) NULL ,
organization_region VARCHAR2(20 CHAR) NULL ,
discipline VARCHAR2(50 CHAR) NULL ,
initials VARCHAR2(10 CHAR) NULL ,
note CLOB NULL ,
sec_lang_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_COOPERATOR PRIMARY KEY (cooperator_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_COOPERATOR MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_COOPERATOR BEFORE INSERT ON GRINGLOBAL.COOPERATOR FOR EACH ROW BEGIN IF :NEW.cooperator_id IS NULL THEN SELECT GRINGLOBAL.SQ_COOPERATOR.NEXTVAL INTO :NEW.cooperator_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.cooperator_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.cooperator_group ...') as Action from dual;
CREATE TABLE GRINGLOBAL.cooperator_group (
cooperator_group_id INTEGER NOT NULL ,
name VARCHAR2(60 CHAR) NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
is_historical CHAR(1 CHAR) NOT NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_COOPERATOR_GROUP PRIMARY KEY (cooperator_group_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_COOPERATOR_GROUP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_COOPERATOR_GROUP BEFORE INSERT ON GRINGLOBAL.COOPERATOR_GROUP FOR EACH ROW BEGIN IF :NEW.cooperator_group_id IS NULL THEN SELECT GRINGLOBAL.SQ_COOPERATOR_GROUP.NEXTVAL INTO :NEW.cooperator_group_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.cooperator_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.cooperator_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.cooperator_map (
cooperator_map_id INTEGER NOT NULL ,
cooperator_id INTEGER NOT NULL ,
cooperator_group_id INTEGER NOT NULL ,
note CLOB NULL ,
localid VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_COOPERATOR_MAP PRIMARY KEY (cooperator_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_COOPERATOR_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_COOPERATOR_MAP BEFORE INSERT ON GRINGLOBAL.COOPERATOR_MAP FOR EACH ROW BEGIN IF :NEW.cooperator_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_COOPERATOR_MAP.NEXTVAL INTO :NEW.cooperator_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop (
crop_id INTEGER NOT NULL ,
name VARCHAR2(20 CHAR) NOT NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP PRIMARY KEY (crop_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP BEFORE INSERT ON GRINGLOBAL.CROP FOR EACH ROW BEGIN IF :NEW.crop_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP.NEXTVAL INTO :NEW.crop_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop_trait *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop_trait ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop_trait (
crop_trait_id INTEGER NOT NULL ,
short_name VARCHAR2(10 CHAR) NOT NULL ,
name VARCHAR2(30 CHAR) NULL ,
is_cgc_approved CHAR(1 CHAR) NOT NULL ,
category_code VARCHAR2(10 CHAR) NULL ,
data_type VARCHAR2(10 CHAR) NOT NULL ,
is_coded CHAR(1 CHAR) NOT NULL ,
max_length INTEGER NULL ,
numeric_format VARCHAR2(15 CHAR) NULL ,
numeric_max INTEGER NULL ,
numeric_min INTEGER NULL ,
original_value_type VARCHAR2(10 CHAR) NULL ,
original_value_format VARCHAR2(15 CHAR) NULL ,
crop_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
definition VARCHAR2(2000 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP_TRAIT PRIMARY KEY (crop_trait_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP_TRAIT MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP_TRAIT BEFORE INSERT ON GRINGLOBAL.CROP_TRAIT FOR EACH ROW BEGIN IF :NEW.crop_trait_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP_TRAIT.NEXTVAL INTO :NEW.crop_trait_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop_trait_code *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop_trait_code ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop_trait_code (
crop_trait_code_id INTEGER NOT NULL ,
crop_trait_id INTEGER NOT NULL ,
code VARCHAR2(30 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP_TRAIT_CODE PRIMARY KEY (crop_trait_code_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP_TRAIT_CODE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP_TRAIT_CODE BEFORE INSERT ON GRINGLOBAL.CROP_TRAIT_CODE FOR EACH ROW BEGIN IF :NEW.crop_trait_code_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP_TRAIT_CODE.NEXTVAL INTO :NEW.crop_trait_code_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop_trait_code_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop_trait_code_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop_trait_code_lang (
crop_trait_code_lang_id INTEGER NOT NULL ,
crop_trait_code_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP_TRAIT_CODE_LANG PRIMARY KEY (crop_trait_code_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP_TRAIT_CODE_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP_TRAIT_CODE_LANG BEFORE INSERT ON GRINGLOBAL.CROP_TRAIT_CODE_LANG FOR EACH ROW BEGIN IF :NEW.crop_trait_code_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP_TRAIT_CODE_LANG.NEXTVAL INTO :NEW.crop_trait_code_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop_trait_qualifier *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop_trait_qualifier ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop_trait_qualifier (
crop_trait_qualifier_id INTEGER NOT NULL ,
crop_trait_id INTEGER NOT NULL ,
name VARCHAR2(30 CHAR) NOT NULL ,
definition VARCHAR2(2000 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP_TRAIT_QUALIFIER PRIMARY KEY (crop_trait_qualifier_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP_TRAIT_QUALIFIER MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP_TRAIT_QUALIFIER BEFORE INSERT ON GRINGLOBAL.CROP_TRAIT_QUALIFIER FOR EACH ROW BEGIN IF :NEW.crop_trait_qualifier_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP_TRAIT_QUALIFIER.NEXTVAL INTO :NEW.crop_trait_qualifier_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.genetic_marker *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.genetic_marker ...') as Action from dual;
CREATE TABLE GRINGLOBAL.genetic_marker (
genetic_marker_id INTEGER NOT NULL ,
crop_id INTEGER NOT NULL ,
site_code VARCHAR2(10 CHAR) NOT NULL ,
name VARCHAR2(100 CHAR) NOT NULL ,
synonyms VARCHAR2(200 CHAR) NULL ,
repeat_motif VARCHAR2(100 CHAR) NULL ,
primers VARCHAR2(200 CHAR) NULL ,
assay_conditions VARCHAR2(2000 CHAR) NULL ,
range_products VARCHAR2(60 CHAR) NULL ,
known_standards CLOB NULL ,
genebank_number VARCHAR2(20 CHAR) NULL ,
map_location VARCHAR2(100 CHAR) NULL ,
position VARCHAR2(1000 CHAR) NULL ,
note CLOB NULL ,
poly_type VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_GENETIC_MARKER PRIMARY KEY (genetic_marker_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_GENETIC_MARKER MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_GENETIC_MARKER BEFORE INSERT ON GRINGLOBAL.GENETIC_MARKER FOR EACH ROW BEGIN IF :NEW.genetic_marker_id IS NULL THEN SELECT GRINGLOBAL.SQ_GENETIC_MARKER.NEXTVAL INTO :NEW.genetic_marker_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_group ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_group (
inventory_group_id INTEGER NOT NULL ,
group_name VARCHAR2(100 CHAR) NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_GROUP PRIMARY KEY (inventory_group_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_GROUP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_GROUP BEFORE INSERT ON GRINGLOBAL.INVENTORY_GROUP FOR EACH ROW BEGIN IF :NEW.inventory_group_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_GROUP.NEXTVAL INTO :NEW.inventory_group_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_maint_policy *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_maint_policy ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_maint_policy (
inventory_maint_policy_id INTEGER NOT NULL ,
maintenance_name VARCHAR2(20 CHAR) NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
inventory_default_form VARCHAR2(2 CHAR) NOT NULL ,
unit_of_maintenance VARCHAR2(2 CHAR) NULL ,
is_debit CHAR(1 CHAR) NOT NULL ,
distribution_default_form VARCHAR2(2 CHAR) NOT NULL ,
standard_distribution_quantity INTEGER NULL ,
unit_of_distribution VARCHAR2(2 CHAR) NULL ,
distribution_critical_amount INTEGER NULL ,
replenishment_critical_amount INTEGER NULL ,
regeneration_method VARCHAR2(10 CHAR) NULL ,
standard_pathogen_test_count INTEGER NULL ,
note CLOB NULL ,
cooperator_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_MAINT_POLICY PRIMARY KEY (inventory_maint_policy_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_MAINT_POLICY MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_MAINT_POLICY BEFORE INSERT ON GRINGLOBAL.INVENTORY_MAINT_POLICY FOR EACH ROW BEGIN IF :NEW.inventory_maint_policy_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_MAINT_POLICY.NEXTVAL INTO :NEW.inventory_maint_policy_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.literature *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.literature ...') as Action from dual;
CREATE TABLE GRINGLOBAL.literature (
literature_id INTEGER NOT NULL ,
abbreviation VARCHAR2(20 CHAR) NOT NULL ,
standard_abbreviation VARCHAR2(2000 CHAR) NULL ,
reference_title VARCHAR2(2000 CHAR) NULL ,
author_editor_name VARCHAR2(2000 CHAR) NULL ,
note CLOB NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_LITERATURE PRIMARY KEY (literature_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_LITERATURE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_LITERATURE BEFORE INSERT ON GRINGLOBAL.LITERATURE FOR EACH ROW BEGIN IF :NEW.literature_id IS NULL THEN SELECT GRINGLOBAL.SQ_LITERATURE.NEXTVAL INTO :NEW.literature_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.order_request *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.order_request ...') as Action from dual;
CREATE TABLE GRINGLOBAL.order_request (
order_request_id INTEGER NOT NULL ,
original_order_request_id INTEGER NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
local_number INTEGER NULL ,
order_type VARCHAR2(2 CHAR) NULL ,
ordered_date DATE NULL ,
status VARCHAR2(10 CHAR) NULL ,
is_completed CHAR(1 CHAR) NULL ,
acted_date DATE NULL ,
source_cooperator_id INTEGER NULL ,
requestor_cooperator_id INTEGER NULL ,
ship_to_cooperator_id INTEGER NULL ,
final_recipient_cooperator_id INTEGER NOT NULL ,
order_obtained_via VARCHAR2(10 CHAR) NULL ,
is_supply_low CHAR(1 CHAR) NULL ,
note CLOB NULL ,
special_instruction CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ORDER_REQUEST PRIMARY KEY (order_request_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ORDER_REQUEST MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ORDER_REQUEST BEFORE INSERT ON GRINGLOBAL.ORDER_REQUEST FOR EACH ROW BEGIN IF :NEW.order_request_id IS NULL THEN SELECT GRINGLOBAL.SQ_ORDER_REQUEST.NEXTVAL INTO :NEW.order_request_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.order_request_action *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.order_request_action ...') as Action from dual;
CREATE TABLE GRINGLOBAL.order_request_action (
order_request_action_id INTEGER NOT NULL ,
action_name VARCHAR2(10 CHAR) NOT NULL ,
acted_date DATE NOT NULL ,
action_for_id VARCHAR2(40 CHAR) NULL ,
order_request_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
note CLOB NULL ,
cooperator_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ORDER_REQUEST_ACTION PRIMARY KEY (order_request_action_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ORDER_REQUEST_ACTION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ORDER_REQUEST_ACTION BEFORE INSERT ON GRINGLOBAL.ORDER_REQUEST_ACTION FOR EACH ROW BEGIN IF :NEW.order_request_action_id IS NULL THEN SELECT GRINGLOBAL.SQ_ORDER_REQUEST_ACTION.NEXTVAL INTO :NEW.order_request_action_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.order_request_image *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.order_request_image ...') as Action from dual;
CREATE TABLE GRINGLOBAL.order_request_image (
order_request_image_id INTEGER NOT NULL ,
order_request_id INTEGER NOT NULL ,
virtual_path VARCHAR2(500 CHAR) NOT NULL ,
thumbnail_virtual_path VARCHAR2(500 CHAR) NULL ,
title VARCHAR2(500 CHAR) NULL ,
content_type VARCHAR2(100 CHAR) NULL ,
category VARCHAR2(50 CHAR) NULL ,
status_code VARCHAR2(50 CHAR) NULL ,
note VARCHAR2(2000 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ORDER_REQUEST_IMAGE PRIMARY KEY (order_request_image_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ORDER_REQUEST_IMAGE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ORDER_REQUEST_IMAGE BEFORE INSERT ON GRINGLOBAL.ORDER_REQUEST_IMAGE FOR EACH ROW BEGIN IF :NEW.order_request_image_id IS NULL THEN SELECT GRINGLOBAL.SQ_ORDER_REQUEST_IMAGE.NEXTVAL INTO :NEW.order_request_image_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.plant_introduction *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.plant_introduction ...') as Action from dual;
CREATE TABLE GRINGLOBAL.plant_introduction (
plant_introduction_id INTEGER NOT NULL ,
plant_introduction_year_date DATE NOT NULL ,
lowest_pi_number INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_PLANT_INTRODUCTION PRIMARY KEY (plant_introduction_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_PLANT_INTRODUCTION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_PLANT_INTRODUCTION BEFORE INSERT ON GRINGLOBAL.PLANT_INTRODUCTION FOR EACH ROW BEGIN IF :NEW.plant_introduction_id IS NULL THEN SELECT GRINGLOBAL.SQ_PLANT_INTRODUCTION.NEXTVAL INTO :NEW.plant_introduction_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.region *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.region ...') as Action from dual;
CREATE TABLE GRINGLOBAL.region (
region_id INTEGER NOT NULL ,
continent VARCHAR2(20 CHAR) NOT NULL ,
subcontinent VARCHAR2(30 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_REGION PRIMARY KEY (region_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_REGION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_REGION BEFORE INSERT ON GRINGLOBAL.REGION FOR EACH ROW BEGIN IF :NEW.region_id IS NULL THEN SELECT GRINGLOBAL.SQ_REGION.NEXTVAL INTO :NEW.region_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_dataview *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_dataview ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_dataview (
sec_dataview_id INTEGER NOT NULL ,
dataview_name VARCHAR2(100 CHAR) NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
is_readonly CHAR(1 CHAR) NOT NULL ,
is_system CHAR(1 CHAR) NOT NULL ,
is_property_suppressed CHAR(1 CHAR) NOT NULL ,
is_user_visible CHAR(1 CHAR) NOT NULL ,
is_web_visible CHAR(1 CHAR) NOT NULL ,
form_assembly_name VARCHAR2(255 CHAR) NULL ,
form_fully_qualified_name VARCHAR2(255 CHAR) NULL ,
executable_name VARCHAR2(255 CHAR) NULL ,
category_name VARCHAR2(50 CHAR) NULL ,
category_sort_order INTEGER NULL ,
is_transform CHAR(1 CHAR) NOT NULL ,
transform_field_for_names VARCHAR2(50 CHAR) NULL ,
transform_field_for_captions VARCHAR2(50 CHAR) NULL ,
transform_field_for_values VARCHAR2(50 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATAVIEW PRIMARY KEY (sec_dataview_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATAVIEW MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATAVIEW BEFORE INSERT ON GRINGLOBAL.SEC_DATAVIEW FOR EACH ROW BEGIN IF :NEW.sec_dataview_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATAVIEW.NEXTVAL INTO :NEW.sec_dataview_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_dataview_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_dataview_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_dataview_lang (
sec_dataview_lang_id INTEGER NOT NULL ,
sec_dataview_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATAVIEW_LANG PRIMARY KEY (sec_dataview_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATAVIEW_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATAVIEW_LANG BEFORE INSERT ON GRINGLOBAL.SEC_DATAVIEW_LANG FOR EACH ROW BEGIN IF :NEW.sec_dataview_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATAVIEW_LANG.NEXTVAL INTO :NEW.sec_dataview_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_dataview_param *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_dataview_param ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_dataview_param (
sec_dataview_param_id INTEGER NOT NULL ,
sec_dataview_id INTEGER NOT NULL ,
param_name VARCHAR2(50 CHAR) NOT NULL ,
param_type VARCHAR2(50 CHAR) NULL ,
sort_order INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATAVIEW_PARAM PRIMARY KEY (sec_dataview_param_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATAVIEW_PARAM MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATAVIEW_PARAM BEFORE INSERT ON GRINGLOBAL.SEC_DATAVIEW_PARAM FOR EACH ROW BEGIN IF :NEW.sec_dataview_param_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATAVIEW_PARAM.NEXTVAL INTO :NEW.sec_dataview_param_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_dataview_sql *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_dataview_sql ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_dataview_sql (
sec_dataview_sql_id INTEGER NOT NULL ,
sec_dataview_id INTEGER NOT NULL ,
db_engine_code VARCHAR2(10 CHAR) NOT NULL ,
sql_statement CLOB NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATAVIEW_SQL PRIMARY KEY (sec_dataview_sql_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATAVIEW_SQL MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATAVIEW_SQL BEFORE INSERT ON GRINGLOBAL.SEC_DATAVIEW_SQL FOR EACH ROW BEGIN IF :NEW.sec_dataview_sql_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATAVIEW_SQL.NEXTVAL INTO :NEW.sec_dataview_sql_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_file_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_file_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_file_lang (
sec_file_lang_id INTEGER NOT NULL ,
sec_file_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_FILE_LANG PRIMARY KEY (sec_file_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_FILE_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_FILE_LANG BEFORE INSERT ON GRINGLOBAL.SEC_FILE_LANG FOR EACH ROW BEGIN IF :NEW.sec_file_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_FILE_LANG.NEXTVAL INTO :NEW.sec_file_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_group_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_group_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_group_lang (
sec_group_lang_id INTEGER NOT NULL ,
sec_group_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_GROUP_LANG PRIMARY KEY (sec_group_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_GROUP_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_GROUP_LANG BEFORE INSERT ON GRINGLOBAL.SEC_GROUP_LANG FOR EACH ROW BEGIN IF :NEW.sec_group_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_GROUP_LANG.NEXTVAL INTO :NEW.sec_group_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_table *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_table ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_table (
sec_table_id INTEGER NOT NULL ,
table_name VARCHAR2(50 CHAR) NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
is_readonly CHAR(1 CHAR) NOT NULL ,
audits_created CHAR(1 CHAR) NOT NULL ,
audits_modified CHAR(1 CHAR) NOT NULL ,
audits_owned CHAR(1 CHAR) NOT NULL ,
database_area VARCHAR2(100 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_TABLE PRIMARY KEY (sec_table_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_TABLE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_TABLE BEFORE INSERT ON GRINGLOBAL.SEC_TABLE FOR EACH ROW BEGIN IF :NEW.sec_table_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_TABLE.NEXTVAL INTO :NEW.sec_table_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_table_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_table_field ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_table_field (
sec_table_field_id INTEGER NOT NULL ,
sec_table_id INTEGER NOT NULL ,
field_name VARCHAR2(50 CHAR) NOT NULL ,
field_ordinal INTEGER NULL ,
field_purpose VARCHAR2(50 CHAR) NOT NULL ,
field_type VARCHAR2(50 CHAR) NOT NULL ,
default_value VARCHAR2(50 CHAR) NULL ,
is_primary_key CHAR(1 CHAR) NOT NULL ,
is_foreign_key CHAR(1 CHAR) NOT NULL ,
foreign_key_table_field_id INTEGER NULL ,
foreign_key_dataview_name VARCHAR2(50 CHAR) NULL ,
is_nullable CHAR(1 CHAR) NOT NULL ,
gui_hint VARCHAR2(50 CHAR) NOT NULL ,
is_readonly CHAR(1 CHAR) NOT NULL ,
min_length INTEGER NOT NULL ,
max_length INTEGER NOT NULL ,
numeric_precision INTEGER NOT NULL ,
numeric_scale INTEGER NOT NULL ,
is_autoincrement CHAR(1 CHAR) NOT NULL ,
group_name VARCHAR2(100 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_TABLE_FIELD PRIMARY KEY (sec_table_field_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_TABLE_FIELD MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_TABLE_FIELD BEFORE INSERT ON GRINGLOBAL.SEC_TABLE_FIELD FOR EACH ROW BEGIN IF :NEW.sec_table_field_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_TABLE_FIELD.NEXTVAL INTO :NEW.sec_table_field_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_table_field_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_table_field_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_table_field_lang (
sec_table_field_lang_id INTEGER NOT NULL ,
sec_table_field_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_TABLE_FIELD_LANG PRIMARY KEY (sec_table_field_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_TABLE_FIELD_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_TABLE_FIELD_LANG BEFORE INSERT ON GRINGLOBAL.SEC_TABLE_FIELD_LANG FOR EACH ROW BEGIN IF :NEW.sec_table_field_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_TABLE_FIELD_LANG.NEXTVAL INTO :NEW.sec_table_field_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_table_relationship *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_table_relationship ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_table_relationship (
sec_table_relationship_id INTEGER NOT NULL ,
sec_table_field_id INTEGER NULL ,
relationship_type_code VARCHAR2(20 CHAR) NOT NULL ,
other_table_field_id INTEGER NULL ,
created_by INTEGER NOT NULL ,
created_date DATE NOT NULL ,
modified_by INTEGER NULL ,
modified_date DATE NULL ,
owned_by INTEGER NOT NULL ,
owned_date DATE NOT NULL ,
CONSTRAINT PK_SEC_TABLE_RELATIONSHIP PRIMARY KEY (sec_table_relationship_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_TABLE_RELATIONSHIP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_TABLE_RELATIONSHIP BEFORE INSERT ON GRINGLOBAL.SEC_TABLE_RELATIONSHIP FOR EACH ROW BEGIN IF :NEW.sec_table_relationship_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_TABLE_RELATIONSHIP.NEXTVAL INTO :NEW.sec_table_relationship_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_user *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_user ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_user (
sec_user_id INTEGER NOT NULL ,
user_name VARCHAR2(50 CHAR) NOT NULL ,
password VARCHAR2(255 CHAR) NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
cooperator_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_USER PRIMARY KEY (sec_user_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_USER MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_USER BEFORE INSERT ON GRINGLOBAL.SEC_USER FOR EACH ROW BEGIN IF :NEW.sec_user_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_USER.NEXTVAL INTO :NEW.sec_user_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_user_cart *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_user_cart ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_user_cart (
sec_user_cart_id INTEGER NOT NULL ,
sec_user_id INTEGER NOT NULL ,
expiration_date DATE NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_USER_CART PRIMARY KEY (sec_user_cart_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_USER_CART MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_USER_CART BEFORE INSERT ON GRINGLOBAL.SEC_USER_CART FOR EACH ROW BEGIN IF :NEW.sec_user_cart_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_USER_CART.NEXTVAL INTO :NEW.sec_user_cart_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_user_cart_item *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_user_cart_item ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_user_cart_item (
sec_user_cart_item_id INTEGER NOT NULL ,
sec_user_cart_id INTEGER NOT NULL ,
item_name VARCHAR2(50 CHAR) NOT NULL ,
item_value VARCHAR2(50 CHAR) NULL ,
item_quantity INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_USER_CART_ITEM PRIMARY KEY (sec_user_cart_item_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_USER_CART_ITEM MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_USER_CART_ITEM BEFORE INSERT ON GRINGLOBAL.SEC_USER_CART_ITEM FOR EACH ROW BEGIN IF :NEW.sec_user_cart_item_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_USER_CART_ITEM.NEXTVAL INTO :NEW.sec_user_cart_item_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.site *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.site ...') as Action from dual;
CREATE TABLE GRINGLOBAL.site (
site_id INTEGER NOT NULL ,
site_code VARCHAR2(10 CHAR) NOT NULL ,
site_name VARCHAR2(100 CHAR) NOT NULL ,
region_code VARCHAR2(10 CHAR) NULL ,
is_distributable CHAR(1 CHAR) NOT NULL ,
type_code VARCHAR2(4 CHAR) NOT NULL ,
institution_code VARCHAR2(50 CHAR) NULL ,
contact_cooperator_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SITE PRIMARY KEY (site_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SITE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SITE BEFORE INSERT ON GRINGLOBAL.SITE FOR EACH ROW BEGIN IF :NEW.site_id IS NULL THEN SELECT GRINGLOBAL.SQ_SITE.NEXTVAL INTO :NEW.site_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_author *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_author ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_author (
taxonomy_author_id INTEGER NOT NULL ,
short_name VARCHAR2(30 CHAR) NOT NULL ,
full_name VARCHAR2(100 CHAR) NULL ,
short_name_diacritic VARCHAR2(30 CHAR) NULL ,
full_name_diacritic VARCHAR2(100 CHAR) NULL ,
short_name_expanded_diacritic VARCHAR2(30 CHAR) NULL ,
full_name_expanded_diacritic VARCHAR2(100 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_AUTHOR PRIMARY KEY (taxonomy_author_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_AUTHOR MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_AUTHOR BEFORE INSERT ON GRINGLOBAL.TAXONOMY_AUTHOR FOR EACH ROW BEGIN IF :NEW.taxonomy_author_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_AUTHOR.NEXTVAL INTO :NEW.taxonomy_author_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_family *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_family ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_family (
taxonomy_family_id INTEGER NOT NULL ,
current_taxonomy_family_id INTEGER NULL ,
family_name VARCHAR2(25 CHAR) NOT NULL ,
author_name VARCHAR2(100 CHAR) NULL ,
alternate_name VARCHAR2(25 CHAR) NULL ,
subfamily VARCHAR2(25 CHAR) NULL ,
tribe VARCHAR2(25 CHAR) NULL ,
subtribe VARCHAR2(25 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_FAMILY PRIMARY KEY (taxonomy_family_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_FAMILY MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_FAMILY BEFORE INSERT ON GRINGLOBAL.TAXONOMY_FAMILY FOR EACH ROW BEGIN IF :NEW.taxonomy_family_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_FAMILY.NEXTVAL INTO :NEW.taxonomy_family_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_genus *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_genus ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_genus (
taxonomy_genus_id INTEGER NOT NULL ,
current_taxonomy_genus_id INTEGER NULL ,
taxonomy_family_id INTEGER NULL ,
qualifying_code VARCHAR2(2 CHAR) NULL ,
is_hybrid CHAR(1 CHAR) NOT NULL ,
genus_name VARCHAR2(30 CHAR) NOT NULL ,
genus_authority VARCHAR2(100 CHAR) NULL ,
subgenus_name VARCHAR2(30 CHAR) NULL ,
section_name VARCHAR2(30 CHAR) NULL ,
series_name VARCHAR2(30 CHAR) NULL ,
subseries_name VARCHAR2(30 CHAR) NULL ,
subsection_name VARCHAR2(30 CHAR) NULL ,
alternate_family VARCHAR2(100 CHAR) NULL ,
common_name VARCHAR2(30 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_GENUS PRIMARY KEY (taxonomy_genus_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_GENUS MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_GENUS BEFORE INSERT ON GRINGLOBAL.TAXONOMY_GENUS FOR EACH ROW BEGIN IF :NEW.taxonomy_genus_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_GENUS.NEXTVAL INTO :NEW.taxonomy_genus_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_genus_type *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_genus_type ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_genus_type (
taxonomy_genus_type_id INTEGER NOT NULL ,
taxonomy_family_id INTEGER NOT NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_GENUS_TYPE PRIMARY KEY (taxonomy_genus_type_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_GENUS_TYPE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_GENUS_TYPE BEFORE INSERT ON GRINGLOBAL.TAXONOMY_GENUS_TYPE FOR EACH ROW BEGIN IF :NEW.taxonomy_genus_type_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_GENUS_TYPE.NEXTVAL INTO :NEW.taxonomy_genus_type_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.url *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.url ...') as Action from dual;
CREATE TABLE GRINGLOBAL.url (
url_id INTEGER NOT NULL ,
caption VARCHAR2(500 CHAR) NOT NULL ,
url VARCHAR2(500 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_URL PRIMARY KEY (url_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_URL MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_URL BEFORE INSERT ON GRINGLOBAL.URL FOR EACH ROW BEGIN IF :NEW.url_id IS NULL THEN SELECT GRINGLOBAL.SQ_URL.NEXTVAL INTO :NEW.url_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_group ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_group (
accession_group_id INTEGER NOT NULL ,
accession_group_code VARCHAR2(20 CHAR) NOT NULL ,
note CLOB NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
url VARCHAR2(2000 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_GROUP PRIMARY KEY (accession_group_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_GROUP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_GROUP BEFORE INSERT ON GRINGLOBAL.ACCESSION_GROUP FOR EACH ROW BEGIN IF :NEW.accession_group_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_GROUP.NEXTVAL INTO :NEW.accession_group_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.app_resource *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.app_resource ...') as Action from dual;
CREATE TABLE GRINGLOBAL.app_resource (
app_resource_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
app_name VARCHAR2(100 CHAR) NULL ,
form_name VARCHAR2(100 CHAR) NULL ,
app_resource_name VARCHAR2(100 CHAR) NOT NULL ,
display_member VARCHAR2(2000 CHAR) NOT NULL ,
value_member VARCHAR2(2000 CHAR) NOT NULL ,
sort_order INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_APP_RESOURCE PRIMARY KEY (app_resource_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_APP_RESOURCE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_APP_RESOURCE BEFORE INSERT ON GRINGLOBAL.APP_RESOURCE FOR EACH ROW BEGIN IF :NEW.app_resource_id IS NULL THEN SELECT GRINGLOBAL.SQ_APP_RESOURCE.NEXTVAL INTO :NEW.app_resource_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.app_user_gui_setting *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.app_user_gui_setting ...') as Action from dual;
CREATE TABLE GRINGLOBAL.app_user_gui_setting (
app_user_gui_setting_id INTEGER NOT NULL ,
cooperator_id INTEGER NOT NULL ,
app_name VARCHAR2(100 CHAR) NULL ,
form_name VARCHAR2(100 CHAR) NULL ,
resource_name VARCHAR2(100 CHAR) NOT NULL ,
resource_key VARCHAR2(100 CHAR) NOT NULL ,
resource_value VARCHAR2(2000 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_APP_USER_GUI_SETTING PRIMARY KEY (app_user_gui_setting_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_APP_USER_GUI_SETTING MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_APP_USER_GUI_SETTING BEFORE INSERT ON GRINGLOBAL.APP_USER_GUI_SETTING FOR EACH ROW BEGIN IF :NEW.app_user_gui_setting_id IS NULL THEN SELECT GRINGLOBAL.SQ_APP_USER_GUI_SETTING.NEXTVAL INTO :NEW.app_user_gui_setting_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.citation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.citation ...') as Action from dual;
CREATE TABLE GRINGLOBAL.citation (
citation_id INTEGER NOT NULL ,
literature_id INTEGER NULL ,
title VARCHAR2(2000 CHAR) NULL ,
author_name VARCHAR2(2000 CHAR) NULL ,
citation_year_date DATE NULL ,
reference VARCHAR2(500 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CITATION PRIMARY KEY (citation_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CITATION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CITATION BEFORE INSERT ON GRINGLOBAL.CITATION FOR EACH ROW BEGIN IF :NEW.citation_id IS NULL THEN SELECT GRINGLOBAL.SQ_CITATION.NEXTVAL INTO :NEW.citation_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.code_value *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.code_value ...') as Action from dual;
CREATE TABLE GRINGLOBAL.code_value (
code_value_id INTEGER NOT NULL ,
group_name VARCHAR2(100 CHAR) NULL ,
value VARCHAR2(20 CHAR) NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
is_standard CHAR(1 CHAR) NOT NULL ,
category_name VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CODE_VALUE PRIMARY KEY (code_value_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CODE_VALUE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CODE_VALUE BEFORE INSERT ON GRINGLOBAL.CODE_VALUE FOR EACH ROW BEGIN IF :NEW.code_value_id IS NULL THEN SELECT GRINGLOBAL.SQ_CODE_VALUE.NEXTVAL INTO :NEW.code_value_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.code_value_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.code_value_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.code_value_lang (
code_value_lang_id INTEGER NOT NULL ,
code_value_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CODE_VALUE_LANG PRIMARY KEY (code_value_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CODE_VALUE_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CODE_VALUE_LANG BEFORE INSERT ON GRINGLOBAL.CODE_VALUE_LANG FOR EACH ROW BEGIN IF :NEW.code_value_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_CODE_VALUE_LANG.NEXTVAL INTO :NEW.code_value_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.genetic_marker_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.genetic_marker_citation_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.genetic_marker_citation_map (
genetic_marker_citation_map_id INTEGER NOT NULL ,
genetic_marker_id INTEGER NOT NULL ,
citation_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_GENETIC_MARKER_CITATION_MAP PRIMARY KEY (genetic_marker_citation_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_GENETIC_MARKER_CITATION_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_GENETIC_MARKER_CITATION_MAP BEFORE INSERT ON GRINGLOBAL.GENETIC_MARKER_CITATION_MAP FOR EACH ROW BEGIN IF :NEW.genetic_marker_citation_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_GENETIC_MARKER_CITATION_MAP.NEXTVAL INTO :NEW.genetic_marker_citation_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.geography *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.geography ...') as Action from dual;
CREATE TABLE GRINGLOBAL.geography (
geography_id INTEGER NOT NULL ,
current_geography_id INTEGER NULL ,
region_id INTEGER NULL ,
country_code VARCHAR2(3 CHAR) NOT NULL ,
adm1 VARCHAR2(50 CHAR) NULL ,
adm1_type_code VARCHAR2(10 CHAR) NULL ,
adm2 VARCHAR2(50 CHAR) NULL ,
adm2_type_code VARCHAR2(10 CHAR) NULL ,
adm3 VARCHAR2(50 CHAR) NULL ,
adm3_type_code VARCHAR2(10 CHAR) NULL ,
adm4 VARCHAR2(50 CHAR) NULL ,
adm4_type_code VARCHAR2(10 CHAR) NULL ,
is_valid CHAR(1 CHAR) NOT NULL ,
changed_date DATE NULL ,
previous_geography_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
old_geography_id INTEGER NULL ,
CONSTRAINT PK_GEOGRAPHY PRIMARY KEY (geography_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_GEOGRAPHY MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_GEOGRAPHY BEFORE INSERT ON GRINGLOBAL.GEOGRAPHY FOR EACH ROW BEGIN IF :NEW.geography_id IS NULL THEN SELECT GRINGLOBAL.SQ_GEOGRAPHY.NEXTVAL INTO :NEW.geography_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.georeference *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.georeference ...') as Action from dual;
CREATE TABLE GRINGLOBAL.georeference (
georeference_id INTEGER NOT NULL ,
elevation INTEGER NULL ,
latitude DECIMAL(18, 8) NOT NULL ,
longitude DECIMAL(18, 8) NOT NULL ,
uncertainty INTEGER NULL ,
verbatim_locality CLOB NULL ,
formatted_locality CLOB NULL ,
georeference_datum VARCHAR2(10 CHAR) NULL ,
georeference_cooperator_id INTEGER NULL ,
georeference_date DATE NULL ,
georeference_protocol_code VARCHAR2(10 CHAR) NULL ,
georeference_annotation CLOB NULL ,
georeference_citation_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_GEOREFERENCE PRIMARY KEY (georeference_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_GEOREFERENCE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_GEOREFERENCE BEFORE INSERT ON GRINGLOBAL.GEOREFERENCE FOR EACH ROW BEGIN IF :NEW.georeference_id IS NULL THEN SELECT GRINGLOBAL.SQ_GEOREFERENCE.NEXTVAL INTO :NEW.georeference_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.method *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.method ...') as Action from dual;
CREATE TABLE GRINGLOBAL.method (
method_id INTEGER NOT NULL ,
name VARCHAR2(100 CHAR) NOT NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
geography_id INTEGER NULL ,
material_or_method_used CLOB NULL ,
study_reason VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_METHOD PRIMARY KEY (method_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_METHOD MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_METHOD BEFORE INSERT ON GRINGLOBAL.METHOD FOR EACH ROW BEGIN IF :NEW.method_id IS NULL THEN SELECT GRINGLOBAL.SQ_METHOD.NEXTVAL INTO :NEW.method_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.method_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.method_citation_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.method_citation_map (
method_citation_map_id INTEGER NOT NULL ,
method_id INTEGER NOT NULL ,
citation_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_METHOD_CITATION_MAP PRIMARY KEY (method_citation_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_METHOD_CITATION_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_METHOD_CITATION_MAP BEFORE INSERT ON GRINGLOBAL.METHOD_CITATION_MAP FOR EACH ROW BEGIN IF :NEW.method_citation_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_METHOD_CITATION_MAP.NEXTVAL INTO :NEW.method_citation_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.method_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.method_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.method_map (
method_cooperator_map_id INTEGER NOT NULL ,
cooperator_id INTEGER NOT NULL ,
method_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_METHOD_MAP PRIMARY KEY (method_cooperator_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_METHOD_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_METHOD_MAP BEFORE INSERT ON GRINGLOBAL.METHOD_MAP FOR EACH ROW BEGIN IF :NEW.method_cooperator_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_METHOD_MAP.NEXTVAL INTO :NEW.method_cooperator_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_datatrigger *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_datatrigger ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_datatrigger (
sec_datatrigger_id INTEGER NOT NULL ,
sec_dataview_id INTEGER NULL ,
sec_table_id INTEGER NULL ,
virtual_file_path VARCHAR2(255 CHAR) NULL ,
assembly_name VARCHAR2(255 CHAR) NOT NULL ,
fully_qualified_class_name VARCHAR2(255 CHAR) NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
is_system CHAR(1 CHAR) NOT NULL ,
sort_order INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATATRIGGER PRIMARY KEY (sec_datatrigger_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATATRIGGER MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATATRIGGER BEFORE INSERT ON GRINGLOBAL.SEC_DATATRIGGER FOR EACH ROW BEGIN IF :NEW.sec_datatrigger_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATATRIGGER.NEXTVAL INTO :NEW.sec_datatrigger_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_dataview_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_dataview_field ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_dataview_field (
sec_dataview_field_id INTEGER NOT NULL ,
sec_dataview_id INTEGER NOT NULL ,
field_name VARCHAR2(50 CHAR) NOT NULL ,
sec_table_field_id INTEGER NULL ,
is_readonly CHAR(1 CHAR) NOT NULL ,
is_primary_key CHAR(1 CHAR) NOT NULL ,
is_transform CHAR(1 CHAR) NOT NULL ,
sort_order INTEGER NOT NULL ,
gui_hint VARCHAR2(100 CHAR) NULL ,
foreign_key_dataview_name VARCHAR2(50 CHAR) NULL ,
group_name VARCHAR2(100 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATAVIEW_FIELD PRIMARY KEY (sec_dataview_field_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATAVIEW_FIELD MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATAVIEW_FIELD BEFORE INSERT ON GRINGLOBAL.SEC_DATAVIEW_FIELD FOR EACH ROW BEGIN IF :NEW.sec_dataview_field_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATAVIEW_FIELD.NEXTVAL INTO :NEW.sec_dataview_field_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_dataview_field_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_dataview_field_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_dataview_field_lang (
sec_dataview_field_lang_id INTEGER NOT NULL ,
sec_dataview_field_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_DATAVIEW_FIELD_LANG PRIMARY KEY (sec_dataview_field_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_DATAVIEW_FIELD_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_DATAVIEW_FIELD_LANG BEFORE INSERT ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG FOR EACH ROW BEGIN IF :NEW.sec_dataview_field_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_DATAVIEW_FIELD_LANG.NEXTVAL INTO :NEW.sec_dataview_field_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_group_user_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_group_user_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_group_user_map (
sec_group_user_map_id INTEGER NOT NULL ,
sec_group_id INTEGER NOT NULL ,
sec_user_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_GROUP_USER_MAP PRIMARY KEY (sec_group_user_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_GROUP_USER_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_GROUP_USER_MAP BEFORE INSERT ON GRINGLOBAL.SEC_GROUP_USER_MAP FOR EACH ROW BEGIN IF :NEW.sec_group_user_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_GROUP_USER_MAP.NEXTVAL INTO :NEW.sec_group_user_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_index *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_index ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_index (
sec_index_id INTEGER NOT NULL ,
sec_table_id INTEGER NOT NULL ,
index_name VARCHAR2(50 CHAR) NOT NULL ,
is_unique CHAR(1 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_INDEX PRIMARY KEY (sec_index_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_INDEX MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_INDEX BEFORE INSERT ON GRINGLOBAL.SEC_INDEX FOR EACH ROW BEGIN IF :NEW.sec_index_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_INDEX.NEXTVAL INTO :NEW.sec_index_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_index_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_index_field ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_index_field (
sec_index_field_id INTEGER NOT NULL ,
sec_index_id INTEGER NOT NULL ,
sec_table_field_id INTEGER NOT NULL ,
sort_order INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_INDEX_FIELD PRIMARY KEY (sec_index_field_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_INDEX_FIELD MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_INDEX_FIELD BEFORE INSERT ON GRINGLOBAL.SEC_INDEX_FIELD FOR EACH ROW BEGIN IF :NEW.sec_index_field_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_INDEX_FIELD.NEXTVAL INTO :NEW.sec_index_field_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_perm *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_perm ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_perm (
sec_perm_id INTEGER NOT NULL ,
sec_dataview_id INTEGER NULL ,
sec_table_id INTEGER NULL ,
perm_code VARCHAR2(50 CHAR) NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
create_perm CHAR(1 CHAR) NOT NULL ,
read_perm CHAR(1 CHAR) NOT NULL ,
update_perm CHAR(1 CHAR) NOT NULL ,
delete_perm CHAR(1 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_PERM PRIMARY KEY (sec_perm_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_PERM MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_PERM BEFORE INSERT ON GRINGLOBAL.SEC_PERM FOR EACH ROW BEGIN IF :NEW.sec_perm_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_PERM.NEXTVAL INTO :NEW.sec_perm_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_perm_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_perm_field ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_perm_field (
sec_perm_field_id INTEGER NOT NULL ,
sec_perm_id INTEGER NOT NULL ,
sec_dataview_field_id INTEGER NULL ,
sec_table_field_id INTEGER NULL ,
field_type VARCHAR2(20 CHAR) NULL ,
compare_operator VARCHAR2(20 CHAR) NULL ,
compare_value CLOB NULL ,
parent_table_field_id INTEGER NULL ,
parent_field_type VARCHAR2(20 CHAR) NULL ,
parent_compare_operator VARCHAR2(20 CHAR) NULL ,
parent_compare_value CLOB NULL ,
compare_mode VARCHAR2(20 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_PERM_FIELD PRIMARY KEY (sec_perm_field_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_PERM_FIELD MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_PERM_FIELD BEFORE INSERT ON GRINGLOBAL.SEC_PERM_FIELD FOR EACH ROW BEGIN IF :NEW.sec_perm_field_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_PERM_FIELD.NEXTVAL INTO :NEW.sec_perm_field_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_perm_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_perm_lang ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_perm_lang (
sec_perm_lang_id INTEGER NOT NULL ,
sec_perm_id INTEGER NOT NULL ,
sec_lang_id INTEGER NOT NULL ,
title VARCHAR2(500 CHAR) NOT NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_PERM_LANG PRIMARY KEY (sec_perm_lang_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_PERM_LANG MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_PERM_LANG BEFORE INSERT ON GRINGLOBAL.SEC_PERM_LANG FOR EACH ROW BEGIN IF :NEW.sec_perm_lang_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_PERM_LANG.NEXTVAL INTO :NEW.sec_perm_lang_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_user_perm *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_user_perm ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_user_perm (
sec_user_perm_id INTEGER NOT NULL ,
sec_user_id INTEGER NOT NULL ,
sec_perm_id INTEGER NOT NULL ,
is_enabled CHAR(1 CHAR) NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_USER_PERM PRIMARY KEY (sec_user_perm_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_USER_PERM MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_USER_PERM BEFORE INSERT ON GRINGLOBAL.SEC_USER_PERM FOR EACH ROW BEGIN IF :NEW.sec_user_perm_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_USER_PERM.NEXTVAL INTO :NEW.sec_user_perm_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy (
taxonomy_id INTEGER NOT NULL ,
current_taxonomy_id INTEGER NULL ,
nomen_number INTEGER NULL ,
is_interspecific_hybrid CHAR(1 CHAR) NOT NULL ,
species VARCHAR2(30 CHAR) NOT NULL ,
species_authority VARCHAR2(100 CHAR) NULL ,
is_intraspecific_hybrid CHAR(1 CHAR) NOT NULL ,
subspecies VARCHAR2(30 CHAR) NULL ,
subspecies_authority VARCHAR2(100 CHAR) NULL ,
is_intervarietal_hybrid CHAR(1 CHAR) NOT NULL ,
variety VARCHAR2(30 CHAR) NULL ,
variety_authority VARCHAR2(100 CHAR) NULL ,
is_subvarietal_hybrid CHAR(1 CHAR) NOT NULL ,
subvariety VARCHAR2(30 CHAR) NULL ,
subvariety_authority VARCHAR2(100 CHAR) NULL ,
is_forma_hybrid CHAR(1 CHAR) NOT NULL ,
forma VARCHAR2(30 CHAR) NULL ,
forma_authority VARCHAR2(100 CHAR) NULL ,
taxonomy_genus_id INTEGER NOT NULL ,
crop_id INTEGER NULL ,
priority_site_1 VARCHAR2(8 CHAR) NULL ,
priority_site_2 VARCHAR2(8 CHAR) NULL ,
restriction VARCHAR2(10 CHAR) NULL ,
life_form VARCHAR2(10 CHAR) NULL ,
common_fertilization VARCHAR2(10 CHAR) NULL ,
is_name_pending CHAR(1 CHAR) NOT NULL ,
synonym_code VARCHAR2(6 CHAR) NULL ,
cooperator_id INTEGER NULL ,
name_verified_date DATE NULL ,
name VARCHAR2(100 CHAR) NULL ,
name_authority VARCHAR2(100 CHAR) NULL ,
protologue CLOB NULL ,
note CLOB NULL ,
site_note CLOB NULL ,
alternate_name VARCHAR2(2000 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY PRIMARY KEY (taxonomy_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY BEFORE INSERT ON GRINGLOBAL.TAXONOMY FOR EACH ROW BEGIN IF :NEW.taxonomy_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY.NEXTVAL INTO :NEW.taxonomy_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_citation_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_citation_map (
taxonomy_citation_id INTEGER NOT NULL ,
taxonomy_id INTEGER NOT NULL ,
citation_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_CITATION_MAP PRIMARY KEY (taxonomy_citation_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_CITATION_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_CITATION_MAP BEFORE INSERT ON GRINGLOBAL.TAXONOMY_CITATION_MAP FOR EACH ROW BEGIN IF :NEW.taxonomy_citation_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_CITATION_MAP.NEXTVAL INTO :NEW.taxonomy_citation_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_common_name *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_common_name ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_common_name (
taxonomy_common_name_id INTEGER NOT NULL ,
taxonomy_id INTEGER NOT NULL ,
name VARCHAR2(50 CHAR) NOT NULL ,
source VARCHAR2(20 CHAR) NULL ,
note CLOB NULL ,
simplified_name VARCHAR2(50 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_COMMON_NAME PRIMARY KEY (taxonomy_common_name_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_COMMON_NAME MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_COMMON_NAME BEFORE INSERT ON GRINGLOBAL.TAXONOMY_COMMON_NAME FOR EACH ROW BEGIN IF :NEW.taxonomy_common_name_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_COMMON_NAME.NEXTVAL INTO :NEW.taxonomy_common_name_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_distribution *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_distribution ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_distribution (
taxonomy_distribution_id INTEGER NOT NULL ,
taxonomy_id INTEGER NOT NULL ,
geography_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_DISTRIBUTION PRIMARY KEY (taxonomy_distribution_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_DISTRIBUTION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_DISTRIBUTION BEFORE INSERT ON GRINGLOBAL.TAXONOMY_DISTRIBUTION FOR EACH ROW BEGIN IF :NEW.taxonomy_distribution_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_DISTRIBUTION.NEXTVAL INTO :NEW.taxonomy_distribution_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_family_cit_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_family_cit_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_family_cit_map (
taxonomy_family_cit_map_id INTEGER NOT NULL ,
taxonomy_family_id INTEGER NOT NULL ,
citation_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_FAMILY_CIT_MAP PRIMARY KEY (taxonomy_family_cit_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_FAMILY_CIT_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_FAMILY_CIT_MAP BEFORE INSERT ON GRINGLOBAL.TAXONOMY_FAMILY_CIT_MAP FOR EACH ROW BEGIN IF :NEW.taxonomy_family_cit_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_FAMILY_CIT_MAP.NEXTVAL INTO :NEW.taxonomy_family_cit_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_genus_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_genus_citation_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_genus_citation_map (
taxonomy_genus_citation_map_id INTEGER NOT NULL ,
taxonomy_genus_id INTEGER NOT NULL ,
citation_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_GENUS_CITATION_MAP PRIMARY KEY (taxonomy_genus_citation_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_GENUS_CITATION_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_GENUS_CITATION_MAP BEFORE INSERT ON GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP FOR EACH ROW BEGIN IF :NEW.taxonomy_genus_citation_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_GENUS_CITATION_MAP.NEXTVAL INTO :NEW.taxonomy_genus_citation_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_germination_rule *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_germination_rule ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_germination_rule (
taxonomy_germination_rule_id INTEGER NOT NULL ,
substrata VARCHAR2(70 CHAR) NULL ,
temperature_range VARCHAR2(30 CHAR) NULL ,
requirements CLOB NULL ,
author_name VARCHAR2(20 CHAR) NULL ,
category VARCHAR2(10 CHAR) NULL ,
days VARCHAR2(20 CHAR) NULL ,
taxonomy_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_GERMINATION_RULE PRIMARY KEY (taxonomy_germination_rule_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_GERMINATION_RULE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_GERMINATION_RULE BEFORE INSERT ON GRINGLOBAL.TAXONOMY_GERMINATION_RULE FOR EACH ROW BEGIN IF :NEW.taxonomy_germination_rule_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_GERMINATION_RULE.NEXTVAL INTO :NEW.taxonomy_germination_rule_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_url *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_url ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_url (
taxonomy_url_id INTEGER NOT NULL ,
url_type VARCHAR2(10 CHAR) NOT NULL ,
taxonomy_family_id INTEGER NOT NULL ,
taxonomy_genus_id INTEGER NULL ,
taxonomy_id INTEGER NULL ,
url_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_URL PRIMARY KEY (taxonomy_url_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_URL MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_URL BEFORE INSERT ON GRINGLOBAL.TAXONOMY_URL FOR EACH ROW BEGIN IF :NEW.taxonomy_url_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_URL.NEXTVAL INTO :NEW.taxonomy_url_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.taxonomy_use *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.taxonomy_use ...') as Action from dual;
CREATE TABLE GRINGLOBAL.taxonomy_use (
taxonomy_use_id INTEGER NOT NULL ,
taxonomy_id INTEGER NOT NULL ,
economic_usage VARCHAR2(10 CHAR) NOT NULL ,
note CLOB NULL ,
usage_type VARCHAR2(250 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_TAXONOMY_USE PRIMARY KEY (taxonomy_use_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_TAXONOMY_USE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_TAXONOMY_USE BEFORE INSERT ON GRINGLOBAL.TAXONOMY_USE FOR EACH ROW BEGIN IF :NEW.taxonomy_use_id IS NULL THEN SELECT GRINGLOBAL.SQ_TAXONOMY_USE.NEXTVAL INTO :NEW.taxonomy_use_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession (
accession_id INTEGER NOT NULL ,
accession_prefix VARCHAR2(50 CHAR) NOT NULL ,
accession_number INTEGER NULL ,
accession_suffix VARCHAR2(4 CHAR) NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
inactive_site_code_reason VARCHAR2(10 CHAR) NULL ,
is_core CHAR(1 CHAR) NULL ,
is_backed_up CHAR(1 CHAR) NULL ,
backup_location VARCHAR2(50 CHAR) NULL ,
life_form VARCHAR2(10 CHAR) NULL ,
level_of_improvement_code VARCHAR2(10 CHAR) NULL ,
reproductive_uniformity VARCHAR2(10 CHAR) NULL ,
initial_material_type VARCHAR2(2 CHAR) NULL ,
initial_received_date DATE NOT NULL ,
initial_received_date_format VARCHAR2(10 CHAR) NULL ,
taxonomy_id INTEGER NOT NULL ,
plant_introduction_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION PRIMARY KEY (accession_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION BEFORE INSERT ON GRINGLOBAL.ACCESSION FOR EACH ROW BEGIN IF :NEW.accession_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION.NEXTVAL INTO :NEW.accession_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_action *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_action ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_action (
accession_action_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
action_name VARCHAR2(10 CHAR) NOT NULL ,
occurred_date DATE NULL ,
occurred_date_format VARCHAR2(10 CHAR) NULL ,
completed_date DATE NULL ,
completed_date_format VARCHAR2(10 CHAR) NULL ,
is_visible_from_web CHAR(1 CHAR) NOT NULL ,
narrative CLOB NULL ,
cooperator_id INTEGER NULL ,
method_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_ACTION PRIMARY KEY (accession_action_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_ACTION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_ACTION BEFORE INSERT ON GRINGLOBAL.ACCESSION_ACTION FOR EACH ROW BEGIN IF :NEW.accession_action_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_ACTION.NEXTVAL INTO :NEW.accession_action_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_citation_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_citation_map (
accession_citation_map_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
citation_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_CITATION_MAP PRIMARY KEY (accession_citation_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_CITATION_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_CITATION_MAP BEFORE INSERT ON GRINGLOBAL.ACCESSION_CITATION_MAP FOR EACH ROW BEGIN IF :NEW.accession_citation_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_CITATION_MAP.NEXTVAL INTO :NEW.accession_citation_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_habitat *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_habitat ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_habitat (
accession_habitat_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
latitude_degrees INTEGER NULL ,
latitude_minutes INTEGER NULL ,
latitude_seconds INTEGER NULL ,
latitude_hemisphere CHAR(1 CHAR) NULL ,
longitude_degrees INTEGER NULL ,
longitude_minutes INTEGER NULL ,
longitude_seconds INTEGER NULL ,
longitude_hemisphere CHAR(1 CHAR) NULL ,
elevation_in_meters INTEGER NULL ,
quantity_collected INTEGER NULL ,
unit_of_quantity_collected VARCHAR2(2 CHAR) NULL ,
form_material_collected_code VARCHAR2(2 CHAR) NULL ,
plant_sample_count INTEGER NULL ,
locality VARCHAR2(2000 CHAR) NULL ,
habitat_name VARCHAR2(2000 CHAR) NULL ,
note CLOB NULL ,
collection_coordinate_system VARCHAR2(10 CHAR) NULL ,
gstype VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_HABITAT PRIMARY KEY (accession_habitat_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_HABITAT MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_HABITAT BEFORE INSERT ON GRINGLOBAL.ACCESSION_HABITAT FOR EACH ROW BEGIN IF :NEW.accession_habitat_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_HABITAT.NEXTVAL INTO :NEW.accession_habitat_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_ipr *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_ipr ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_ipr (
accession_ipr_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
assigned_type VARCHAR2(10 CHAR) NOT NULL ,
accession_ipr_prefix VARCHAR2(40 CHAR) NULL ,
accession_ipr_number INTEGER NULL ,
crop_name VARCHAR2(60 CHAR) NULL ,
full_name VARCHAR2(2000 CHAR) NULL ,
issued_date DATE NULL ,
expired_date DATE NULL ,
cooperator_id INTEGER NULL ,
citation_id INTEGER NULL ,
note CLOB NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
accepted_date DATE NULL ,
expected_date DATE NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_IPR PRIMARY KEY (accession_ipr_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_IPR MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_IPR BEFORE INSERT ON GRINGLOBAL.ACCESSION_IPR FOR EACH ROW BEGIN IF :NEW.accession_ipr_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_IPR.NEXTVAL INTO :NEW.accession_ipr_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_narrative *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_narrative ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_narrative (
accession_narrative_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
type_code VARCHAR2(10 CHAR) NULL ,
narrative_body CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_NARRATIVE PRIMARY KEY (accession_narrative_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_NARRATIVE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_NARRATIVE BEFORE INSERT ON GRINGLOBAL.ACCESSION_NARRATIVE FOR EACH ROW BEGIN IF :NEW.accession_narrative_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_NARRATIVE.NEXTVAL INTO :NEW.accession_narrative_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_pedigree *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_pedigree ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_pedigree (
accession_pedigree_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
released_date DATE NULL ,
released_date_format VARCHAR2(10 CHAR) NULL ,
citation_id INTEGER NULL ,
description CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_PEDIGREE PRIMARY KEY (accession_pedigree_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_PEDIGREE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_PEDIGREE BEFORE INSERT ON GRINGLOBAL.ACCESSION_PEDIGREE FOR EACH ROW BEGIN IF :NEW.accession_pedigree_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_PEDIGREE.NEXTVAL INTO :NEW.accession_pedigree_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_quarantine *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_quarantine ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_quarantine (
accession_quarantine_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
quarantine_type VARCHAR2(10 CHAR) NOT NULL ,
progress_status_code VARCHAR2(10 CHAR) NULL ,
cooperator_id INTEGER NOT NULL ,
entered_date DATE NULL ,
established_date DATE NULL ,
expected_release_date DATE NULL ,
released_date DATE NULL ,
note CLOB NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_QUARANTINE PRIMARY KEY (accession_quarantine_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_QUARANTINE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_QUARANTINE BEFORE INSERT ON GRINGLOBAL.ACCESSION_QUARANTINE FOR EACH ROW BEGIN IF :NEW.accession_quarantine_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_QUARANTINE.NEXTVAL INTO :NEW.accession_quarantine_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_source *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_source ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_source (
accession_source_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
geography_id INTEGER NULL ,
georeference_id INTEGER NULL ,
collection_source_code VARCHAR2(10 CHAR) NULL ,
type_code VARCHAR2(10 CHAR) NOT NULL ,
step_date DATE NULL ,
step_date_format VARCHAR2(10 CHAR) NULL ,
is_origin_step CHAR(1 CHAR) NULL ,
quantity_collected INTEGER NULL ,
unit_of_quantity_collected VARCHAR2(2 CHAR) NULL ,
form_material_collected_code VARCHAR2(2 CHAR) NULL ,
plant_sample_count INTEGER NULL ,
environment_description CLOB NULL ,
collection_note CLOB NULL ,
note CLOB NULL ,
step_date_qualifier VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
old_accession_source_id INTEGER NULL ,
collection_elevation_meters INTEGER NULL ,
collector_verbatim_locality CLOB NULL ,
CONSTRAINT PK_ACCESSION_SOURCE PRIMARY KEY (accession_source_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_SOURCE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_SOURCE BEFORE INSERT ON GRINGLOBAL.ACCESSION_SOURCE FOR EACH ROW BEGIN IF :NEW.accession_source_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_SOURCE.NEXTVAL INTO :NEW.accession_source_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_source_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_source_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_source_map (
accession_source_map_id INTEGER NOT NULL ,
accession_source_id INTEGER NOT NULL ,
cooperator_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_SOURCE_MAP PRIMARY KEY (accession_source_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_SOURCE_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_SOURCE_MAP BEFORE INSERT ON GRINGLOBAL.ACCESSION_SOURCE_MAP FOR EACH ROW BEGIN IF :NEW.accession_source_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_SOURCE_MAP.NEXTVAL INTO :NEW.accession_source_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.code_rule *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.code_rule ...') as Action from dual;
CREATE TABLE GRINGLOBAL.code_rule (
code_rule_id INTEGER NOT NULL ,
code_value_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
max_length INTEGER NULL ,
function_name VARCHAR2(7 CHAR) NULL ,
is_standard CHAR(1 CHAR) NOT NULL ,
is_by_category CHAR(1 CHAR) NOT NULL ,
cateogry_number INTEGER NULL ,
category_note CLOB NULL ,
form_name VARCHAR2(30 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CODE_RULE PRIMARY KEY (code_rule_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CODE_RULE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CODE_RULE BEFORE INSERT ON GRINGLOBAL.CODE_RULE FOR EACH ROW BEGIN IF :NEW.code_rule_id IS NULL THEN SELECT GRINGLOBAL.SQ_CODE_RULE.NEXTVAL INTO :NEW.code_rule_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop_trait_url *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop_trait_url ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop_trait_url (
crop_trait_url_id INTEGER NOT NULL ,
url_type VARCHAR2(10 CHAR) NOT NULL ,
sequence_number INTEGER NOT NULL ,
crop_id INTEGER NOT NULL ,
crop_trait_id INTEGER NULL ,
code VARCHAR2(30 CHAR) NULL ,
url_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
note CLOB NULL ,
method_id INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP_TRAIT_URL PRIMARY KEY (crop_trait_url_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP_TRAIT_URL MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP_TRAIT_URL BEFORE INSERT ON GRINGLOBAL.CROP_TRAIT_URL FOR EACH ROW BEGIN IF :NEW.crop_trait_url_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP_TRAIT_URL.NEXTVAL INTO :NEW.crop_trait_url_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.genetic_annotation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.genetic_annotation ...') as Action from dual;
CREATE TABLE GRINGLOBAL.genetic_annotation (
genetic_annotation_id INTEGER NOT NULL ,
marker_id INTEGER NOT NULL ,
method_id INTEGER NOT NULL ,
method CLOB NULL ,
scoring_method VARCHAR2(1000 CHAR) NULL ,
control_values VARCHAR2(1000 CHAR) NULL ,
observation_alleles_count INTEGER NULL ,
max_gob_alleles INTEGER NULL ,
size_alleles VARCHAR2(100 CHAR) NULL ,
unusual_alleles VARCHAR2(100 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_GENETIC_ANNOTATION PRIMARY KEY (genetic_annotation_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_GENETIC_ANNOTATION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_GENETIC_ANNOTATION BEFORE INSERT ON GRINGLOBAL.GENETIC_ANNOTATION FOR EACH ROW BEGIN IF :NEW.genetic_annotation_id IS NULL THEN SELECT GRINGLOBAL.SQ_GENETIC_ANNOTATION.NEXTVAL INTO :NEW.genetic_annotation_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory (
inventory_id INTEGER NOT NULL ,
inventory_prefix VARCHAR2(50 CHAR) NOT NULL ,
inventory_number INTEGER NULL ,
inventory_suffix VARCHAR2(8 CHAR) NULL ,
inventory_type_code VARCHAR2(2 CHAR) NOT NULL ,
inventory_maint_policy_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
is_distributable CHAR(1 CHAR) NULL ,
location_section_1 VARCHAR2(10 CHAR) NULL ,
location_section_2 VARCHAR2(10 CHAR) NULL ,
location_section_3 VARCHAR2(10 CHAR) NULL ,
location_section_4 VARCHAR2(10 CHAR) NULL ,
quantity_on_hand INTEGER NULL ,
unit_of_quantity_on_hand VARCHAR2(2 CHAR) NULL ,
is_debit CHAR(1 CHAR) NULL ,
distribution_default_form VARCHAR2(2 CHAR) NULL ,
standard_distribution_quantity INTEGER NULL ,
unit_of_distribution VARCHAR2(2 CHAR) NULL ,
distribution_critical_amount INTEGER NULL ,
replenishment_critical_amount INTEGER NULL ,
pathogen_status VARCHAR2(10 CHAR) NULL ,
availability_status VARCHAR2(10 CHAR) NULL ,
status_note VARCHAR2(60 CHAR) NULL ,
accession_id INTEGER NOT NULL ,
parent_inventory_id INTEGER NULL ,
cooperator_id INTEGER NULL ,
backup_inventory_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY PRIMARY KEY (inventory_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY BEFORE INSERT ON GRINGLOBAL.INVENTORY FOR EACH ROW BEGIN IF :NEW.inventory_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY.NEXTVAL INTO :NEW.inventory_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_action *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_action ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_action (
inventory_action_id INTEGER NOT NULL ,
action_name VARCHAR2(10 CHAR) NOT NULL ,
occurred_date DATE NULL ,
occurred_date_format VARCHAR2(10 CHAR) NULL ,
quantity INTEGER NULL ,
unit_of_quantity VARCHAR2(2 CHAR) NULL ,
form_involved VARCHAR2(2 CHAR) NULL ,
inventory_id INTEGER NOT NULL ,
cooperator_id INTEGER NULL ,
method_id INTEGER NULL ,
note CLOB NULL ,
qualifier VARCHAR2(10 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_ACTION PRIMARY KEY (inventory_action_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_ACTION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_ACTION BEFORE INSERT ON GRINGLOBAL.INVENTORY_ACTION FOR EACH ROW BEGIN IF :NEW.inventory_action_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_ACTION.NEXTVAL INTO :NEW.inventory_action_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_group_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_group_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_group_map (
inventory_group_map_id INTEGER NOT NULL ,
inventory_id INTEGER NOT NULL ,
inventory_group_id INTEGER NOT NULL ,
site_code VARCHAR2(8 CHAR) NOT NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_GROUP_MAP PRIMARY KEY (inventory_group_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_GROUP_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_GROUP_MAP BEFORE INSERT ON GRINGLOBAL.INVENTORY_GROUP_MAP FOR EACH ROW BEGIN IF :NEW.inventory_group_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_GROUP_MAP.NEXTVAL INTO :NEW.inventory_group_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_image *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_image ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_image (
inventory_image_id INTEGER NOT NULL ,
inventory_id INTEGER NOT NULL ,
virtual_path VARCHAR2(500 CHAR) NOT NULL ,
thumbnail_virtual_path VARCHAR2(500 CHAR) NULL ,
title VARCHAR2(500 CHAR) NULL ,
content_type VARCHAR2(100 CHAR) NULL ,
category VARCHAR2(50 CHAR) NULL ,
status_code VARCHAR2(50 CHAR) NULL ,
note VARCHAR2(2000 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_IMAGE PRIMARY KEY (inventory_image_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_IMAGE MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_IMAGE BEFORE INSERT ON GRINGLOBAL.INVENTORY_IMAGE FOR EACH ROW BEGIN IF :NEW.inventory_image_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_IMAGE.NEXTVAL INTO :NEW.inventory_image_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_quality_status *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_quality_status ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_quality_status (
inventory_quality_status_id INTEGER NOT NULL ,
inventory_id INTEGER NOT NULL ,
test_type VARCHAR2(10 CHAR) NOT NULL ,
pathogen_code VARCHAR2(10 CHAR) NOT NULL ,
started_date DATE NULL ,
finished_date DATE NULL ,
test_results VARCHAR2(10 CHAR) NULL ,
needed_count INTEGER NULL ,
started_count INTEGER NULL ,
completed_count INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_QUALITY_STATUS PRIMARY KEY (inventory_quality_status_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_QUALITY_STATUS MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_QUALITY_STATUS BEFORE INSERT ON GRINGLOBAL.INVENTORY_QUALITY_STATUS FOR EACH ROW BEGIN IF :NEW.inventory_quality_status_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_QUALITY_STATUS.NEXTVAL INTO :NEW.inventory_quality_status_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.inventory_viability *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.inventory_viability ...') as Action from dual;
CREATE TABLE GRINGLOBAL.inventory_viability (
inventory_viability_id INTEGER NOT NULL ,
tested_date DATE NOT NULL ,
tested_date_format VARCHAR2(10 CHAR) NULL ,
percent_normal INTEGER NULL ,
percent_abnormal INTEGER NULL ,
percent_dormant INTEGER NULL ,
percent_viable INTEGER NULL ,
vigor_rating VARCHAR2(10 CHAR) NULL ,
sample_count INTEGER NULL ,
replication_count INTEGER NULL ,
inventory_id INTEGER NOT NULL ,
method_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_INVENTORY_VIABILITY PRIMARY KEY (inventory_viability_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_INVENTORY_VIABILITY MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_INVENTORY_VIABILITY BEFORE INSERT ON GRINGLOBAL.INVENTORY_VIABILITY FOR EACH ROW BEGIN IF :NEW.inventory_viability_id IS NULL THEN SELECT GRINGLOBAL.SQ_INVENTORY_VIABILITY.NEXTVAL INTO :NEW.inventory_viability_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.order_request_item *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.order_request_item ...') as Action from dual;
CREATE TABLE GRINGLOBAL.order_request_item (
order_request_item_id INTEGER NOT NULL ,
order_request_id INTEGER NOT NULL ,
sequence_number INTEGER NULL ,
name VARCHAR2(40 CHAR) NULL ,
quantity_shipped INTEGER NULL ,
unit_of_shipped VARCHAR2(2 CHAR) NULL ,
distribution_form VARCHAR2(2 CHAR) NULL ,
ipr_restriction VARCHAR2(10 CHAR) NULL ,
status_code VARCHAR2(10 CHAR) NULL ,
acted_date DATE NULL ,
inventory_id INTEGER NULL ,
taxonomy_id INTEGER NULL ,
external_taxonomy VARCHAR2(100 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ORDER_REQUEST_ITEM PRIMARY KEY (order_request_item_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ORDER_REQUEST_ITEM MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ORDER_REQUEST_ITEM BEFORE INSERT ON GRINGLOBAL.ORDER_REQUEST_ITEM FOR EACH ROW BEGIN IF :NEW.order_request_item_id IS NULL THEN SELECT GRINGLOBAL.SQ_ORDER_REQUEST_ITEM.NEXTVAL INTO :NEW.order_request_item_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.sec_group_perm_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.sec_group_perm_map ...') as Action from dual;
CREATE TABLE GRINGLOBAL.sec_group_perm_map (
sec_group_perm_map_id INTEGER NOT NULL ,
sec_group_id INTEGER NOT NULL ,
sec_perm_id INTEGER NOT NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SEC_GROUP_PERM_MAP PRIMARY KEY (sec_group_perm_map_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SEC_GROUP_PERM_MAP MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SEC_GROUP_PERM_MAP BEFORE INSERT ON GRINGLOBAL.SEC_GROUP_PERM_MAP FOR EACH ROW BEGIN IF :NEW.sec_group_perm_map_id IS NULL THEN SELECT GRINGLOBAL.SQ_SEC_GROUP_PERM_MAP.NEXTVAL INTO :NEW.sec_group_perm_map_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.site_inventory_nc7 *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.site_inventory_nc7 ...') as Action from dual;
CREATE TABLE GRINGLOBAL.site_inventory_nc7 (
site_inventory_nc7_id INTEGER NOT NULL ,
inventory_id INTEGER NOT NULL ,
hundred_weight DECIMAL(18, 5) NULL ,
pollination_control VARCHAR2(10 CHAR) NULL ,
farm_field_identifier VARCHAR2(10 CHAR) NULL ,
location_type_code VARCHAR2(10 CHAR) NULL ,
location_low VARCHAR2(10 CHAR) NULL ,
location_high VARCHAR2(10 CHAR) NULL ,
sublocation_type_code VARCHAR2(10 CHAR) NULL ,
sublocation_low VARCHAR2(10 CHAR) NULL ,
sublocation_high VARCHAR2(10 CHAR) NULL ,
old_inventory_identifier VARCHAR2(30 CHAR) NULL ,
inventory_site_note CLOB NULL ,
inventory_location1_latitude DECIMAL(18, 8) NULL ,
inventory_location1_longitude DECIMAL(18, 8) NULL ,
inventory_location1_precision INTEGER NULL ,
inventory_location2_latitude DECIMAL(18, 8) NULL ,
inventory_location2_longitude DECIMAL(18, 8) NULL ,
inventory_location2_precision INTEGER NULL ,
inventory_datum VARCHAR2(10 CHAR) NULL ,
coordinates_apply_to_code VARCHAR2(10 CHAR) NULL ,
coordinates_comment CLOB NULL ,
coordinates_voucher_location VARCHAR2(500 CHAR) NULL ,
irregular_inventory_location VARCHAR2(500 CHAR) NULL ,
is_increase_success_flag CHAR(1 CHAR) NULL ,
reason_unsuccessfull1_code VARCHAR2(10 CHAR) NULL ,
reason_unsuccessfull2_code VARCHAR2(10 CHAR) NULL ,
reason_unsuccessfull3_code VARCHAR2(10 CHAR) NULL ,
reason_unsuccessfull_note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_SITE_INVENTORY_NC7 PRIMARY KEY (site_inventory_nc7_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_SITE_INVENTORY_NC7 MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_SITE_INVENTORY_NC7 BEFORE INSERT ON GRINGLOBAL.SITE_INVENTORY_NC7 FOR EACH ROW BEGIN IF :NEW.site_inventory_nc7_id IS NULL THEN SELECT GRINGLOBAL.SQ_SITE_INVENTORY_NC7.NEXTVAL INTO :NEW.site_inventory_nc7_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_annotation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_annotation ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_annotation (
accession_annotation_id INTEGER NOT NULL ,
action_name VARCHAR2(10 CHAR) NOT NULL ,
action_date DATE NOT NULL ,
site_code VARCHAR2(8 CHAR) NULL ,
cooperator_id INTEGER NULL ,
inventory_id INTEGER NULL ,
order_request_id INTEGER NULL ,
old_taxonomy_id INTEGER NULL ,
new_taxonomy_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_ANNOTATION PRIMARY KEY (accession_annotation_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_ANNOTATION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_ANNOTATION BEFORE INSERT ON GRINGLOBAL.ACCESSION_ANNOTATION FOR EACH ROW BEGIN IF :NEW.accession_annotation_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_ANNOTATION.NEXTVAL INTO :NEW.accession_annotation_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_name *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_name ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_name (
accession_name_id INTEGER NOT NULL ,
accession_id INTEGER NOT NULL ,
category VARCHAR2(10 CHAR) NOT NULL ,
name VARCHAR2(40 CHAR) NOT NULL ,
name_rank INTEGER NULL ,
accession_group_id INTEGER NULL ,
inventory_id INTEGER NULL ,
cooperator_id INTEGER NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_NAME PRIMARY KEY (accession_name_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_NAME MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_NAME BEFORE INSERT ON GRINGLOBAL.ACCESSION_NAME FOR EACH ROW BEGIN IF :NEW.accession_name_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_NAME.NEXTVAL INTO :NEW.accession_name_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.accession_voucher *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.accession_voucher ...') as Action from dual;
CREATE TABLE GRINGLOBAL.accession_voucher (
accession_voucher_id INTEGER NOT NULL ,
voucher_type VARCHAR2(10 CHAR) NOT NULL ,
inventory_id INTEGER NULL ,
cooperator_id INTEGER NULL ,
vouchered_date DATE NULL ,
vouchered_date_format VARCHAR2(10 CHAR) NULL ,
collector_identifier VARCHAR2(40 CHAR) NULL ,
caption VARCHAR2(100 CHAR) NULL ,
note CLOB NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_ACCESSION_VOUCHER PRIMARY KEY (accession_voucher_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_ACCESSION_VOUCHER MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_ACCESSION_VOUCHER BEFORE INSERT ON GRINGLOBAL.ACCESSION_VOUCHER FOR EACH ROW BEGIN IF :NEW.accession_voucher_id IS NULL THEN SELECT GRINGLOBAL.SQ_ACCESSION_VOUCHER.NEXTVAL INTO :NEW.accession_voucher_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.crop_trait_observation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.crop_trait_observation ...') as Action from dual;
CREATE TABLE GRINGLOBAL.crop_trait_observation (
crop_trait_observation_id INTEGER NOT NULL ,
crop_trait_id INTEGER NOT NULL ,
crop_trait_code_id INTEGER NULL ,
numeric_value DECIMAL(18, 5) NULL ,
string_value VARCHAR2(500 CHAR) NULL ,
method_id INTEGER NULL ,
crop_trait_qualifier_id INTEGER NULL ,
inventory_id INTEGER NOT NULL ,
original_value VARCHAR2(30 CHAR) NULL ,
frequency DECIMAL(18, 5) NULL ,
mean_value DECIMAL(18, 5) NULL ,
maximum_value DECIMAL(18, 5) NULL ,
minimum_value DECIMAL(18, 5) NULL ,
standard_deviation DECIMAL(18, 5) NULL ,
sample_size DECIMAL(18, 5) NULL ,
note CLOB NULL ,
rank INTEGER NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NULL ,
modified_by INTEGER NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_CROP_TRAIT_OBSERVATION PRIMARY KEY (crop_trait_observation_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_CROP_TRAIT_OBSERVATION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_CROP_TRAIT_OBSERVATION BEFORE INSERT ON GRINGLOBAL.CROP_TRAIT_OBSERVATION FOR EACH ROW BEGIN IF :NEW.crop_trait_observation_id IS NULL THEN SELECT GRINGLOBAL.SQ_CROP_TRAIT_OBSERVATION.NEXTVAL INTO :NEW.crop_trait_observation_id FROM DUAL; END IF; END;
/


/************ Table Definition for GRINGLOBAL.genetic_observation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table GRINGLOBAL.genetic_observation ...') as Action from dual;
CREATE TABLE GRINGLOBAL.genetic_observation (
genetic_observation_id INTEGER NOT NULL ,
genetic_annotation_id INTEGER NOT NULL ,
inventory_id INTEGER NOT NULL ,
individual INTEGER NULL ,
value VARCHAR2(1000 CHAR) NOT NULL ,
genebank_url VARCHAR2(500 CHAR) NULL ,
image_url VARCHAR2(500 CHAR) NULL ,
created_date DATE NOT NULL ,
created_by INTEGER NOT NULL ,
modified_date DATE NOT NULL ,
modified_by INTEGER NOT NULL ,
owned_date DATE NOT NULL ,
owned_by INTEGER NOT NULL ,
CONSTRAINT PK_GENETIC_OBSERVATION PRIMARY KEY (genetic_observation_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.SQ_GENETIC_OBSERVATION MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE;
CREATE OR REPLACE TRIGGER GRINGLOBAL.TG_GENETIC_OBSERVATION BEFORE INSERT ON GRINGLOBAL.GENETIC_OBSERVATION FOR EACH ROW BEGIN IF :NEW.genetic_observation_id IS NULL THEN SELECT GRINGLOBAL.SQ_GENETIC_OBSERVATION.NEXTVAL INTO :NEW.genetic_observation_id FROM DUAL; END IF; END;
/


