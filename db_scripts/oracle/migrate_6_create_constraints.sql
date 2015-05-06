 /***********************************************/
/*********** Constraint Definitions ************/
/***********************************************/

/************ No constraint definitions exist for app_setting *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table app_setting') as Action from dual;
/************ No constraint definitions exist for app_user_item_list *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table app_user_item_list') as Action from dual;
/************ No constraint definitions exist for sec_db *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table sec_db') as Action from dual;
/************ No constraint definitions exist for sec_db_migration *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table sec_db_migration') as Action from dual;
/********** 1 Constraint Definitions for sec_db_migration_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sdbml_sdbm for table sec_db_migration_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DB_MIGRATION_LANG ADD CONSTRAINT FK_SDBML_SDBM FOREIGN KEY (sec_db_migration_id) REFERENCES GRINGLOBAL.SEC_DB_MIGRATION (sec_db_migration_id)
/

/************ No constraint definitions exist for sec_file *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table sec_file') as Action from dual;
/************ No constraint definitions exist for sec_file_group *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table sec_file_group') as Action from dual;
/********** 2 Constraint Definitions for sec_file_group_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sfgm_sf for table sec_file_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_FILE_GROUP_MAP ADD CONSTRAINT FK_SFGM_SF FOREIGN KEY (sec_file_id) REFERENCES GRINGLOBAL.SEC_FILE (sec_file_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sfgm_sfg for table sec_file_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_FILE_GROUP_MAP ADD CONSTRAINT FK_SFGM_SFG FOREIGN KEY (sec_file_group_id) REFERENCES GRINGLOBAL.SEC_FILE_GROUP (sec_file_group_id)
/

/************ No constraint definitions exist for sec_group *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table sec_group') as Action from dual;
/************ No constraint definitions exist for sec_lang *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table sec_lang') as Action from dual;
/********** 5 Constraint Definitions for cooperator **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_c_created for table cooperator ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR ADD CONSTRAINT FK_C_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_c_cur_c for table cooperator ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR ADD CONSTRAINT FK_C_CUR_C FOREIGN KEY (current_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_c_modified for table cooperator ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR ADD CONSTRAINT FK_C_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_c_owned for table cooperator ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR ADD CONSTRAINT FK_C_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_c_sl for table cooperator ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR ADD CONSTRAINT FK_C_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

/********** 3 Constraint Definitions for cooperator_group **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cg_created for table cooperator_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_GROUP ADD CONSTRAINT FK_CG_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cg_modified for table cooperator_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_GROUP ADD CONSTRAINT FK_CG_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cg_owned for table cooperator_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_GROUP ADD CONSTRAINT FK_CG_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for cooperator_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cm_c for table cooperator_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_MAP ADD CONSTRAINT FK_CM_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cm_cg for table cooperator_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_MAP ADD CONSTRAINT FK_CM_CG FOREIGN KEY (cooperator_group_id) REFERENCES GRINGLOBAL.COOPERATOR_GROUP (cooperator_group_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cm_created for table cooperator_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_MAP ADD CONSTRAINT FK_CM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cm_modified for table cooperator_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_MAP ADD CONSTRAINT FK_CM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cm_owned for table cooperator_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.COOPERATOR_MAP ADD CONSTRAINT FK_CM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for crop **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cr_created for table crop ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP ADD CONSTRAINT FK_CR_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cr_modified for table crop ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP ADD CONSTRAINT FK_CR_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cr_owned for table crop ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP ADD CONSTRAINT FK_CR_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for crop_trait **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ct_cr for table crop_trait ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT ADD CONSTRAINT FK_CT_CR FOREIGN KEY (crop_id) REFERENCES GRINGLOBAL.CROP (crop_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ct_created for table crop_trait ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT ADD CONSTRAINT FK_CT_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ct_modified for table crop_trait ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT ADD CONSTRAINT FK_CT_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ct_owned for table crop_trait ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT ADD CONSTRAINT FK_CT_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for crop_trait_code **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tct_created for table crop_trait_code ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE ADD CONSTRAINT FK_TCT_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tct_modified for table crop_trait_code ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE ADD CONSTRAINT FK_TCT_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tct_owned for table crop_trait_code ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE ADD CONSTRAINT FK_TCT_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tct_tr for table crop_trait_code ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE ADD CONSTRAINT FK_TCT_TR FOREIGN KEY (crop_trait_id) REFERENCES GRINGLOBAL.CROP_TRAIT (crop_trait_id)
/

/********** 5 Constraint Definitions for crop_trait_code_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctcl_created for table crop_trait_code_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE_LANG ADD CONSTRAINT FK_CTCL_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctcl_modified for table crop_trait_code_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE_LANG ADD CONSTRAINT FK_CTCL_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctcl_owned for table crop_trait_code_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE_LANG ADD CONSTRAINT FK_CTCL_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctcl_sl for table crop_trait_code_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE_LANG ADD CONSTRAINT FK_CTCL_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctcl_tc for table crop_trait_code_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_CODE_LANG ADD CONSTRAINT FK_CTCL_TC FOREIGN KEY (crop_trait_code_id) REFERENCES GRINGLOBAL.CROP_TRAIT_CODE (crop_trait_code_id)
/

/********** 4 Constraint Definitions for crop_trait_qualifier **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctq_created for table crop_trait_qualifier ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_QUALIFIER ADD CONSTRAINT FK_CTQ_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctq_ct for table crop_trait_qualifier ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_QUALIFIER ADD CONSTRAINT FK_CTQ_CT FOREIGN KEY (crop_trait_id) REFERENCES GRINGLOBAL.CROP_TRAIT (crop_trait_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctq_modified for table crop_trait_qualifier ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_QUALIFIER ADD CONSTRAINT FK_CTQ_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctq_owned for table crop_trait_qualifier ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_QUALIFIER ADD CONSTRAINT FK_CTQ_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for genetic_marker **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gm_created for table genetic_marker ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER ADD CONSTRAINT FK_GM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gm_modified for table genetic_marker ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER ADD CONSTRAINT FK_GM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gm_owned for table genetic_marker ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER ADD CONSTRAINT FK_GM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gm_tcr for table genetic_marker ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER ADD CONSTRAINT FK_GM_TCR FOREIGN KEY (crop_id) REFERENCES GRINGLOBAL.CROP (crop_id)
/

/********** 3 Constraint Definitions for inventory_group **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ig_created for table inventory_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP ADD CONSTRAINT FK_IG_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ig_modified for table inventory_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP ADD CONSTRAINT FK_IG_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ig_owned for table inventory_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP ADD CONSTRAINT FK_IG_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for inventory_maint_policy **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_im_co for table inventory_maint_policy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_MAINT_POLICY ADD CONSTRAINT FK_IM_CO FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_im_created for table inventory_maint_policy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_MAINT_POLICY ADD CONSTRAINT FK_IM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_im_modified for table inventory_maint_policy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_MAINT_POLICY ADD CONSTRAINT FK_IM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_im_owned for table inventory_maint_policy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_MAINT_POLICY ADD CONSTRAINT FK_IM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for literature **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_l_created for table literature ...') as Action from dual;
ALTER TABLE GRINGLOBAL.LITERATURE ADD CONSTRAINT FK_L_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_l_modified for table literature ...') as Action from dual;
ALTER TABLE GRINGLOBAL.LITERATURE ADD CONSTRAINT FK_L_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_l_owned for table literature ...') as Action from dual;
ALTER TABLE GRINGLOBAL.LITERATURE ADD CONSTRAINT FK_L_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 8 Constraint Definitions for order_request **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_created for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_final_c for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_FINAL_C FOREIGN KEY (final_recipient_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_modified for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_original_or for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_ORIGINAL_OR FOREIGN KEY (original_order_request_id) REFERENCES GRINGLOBAL.ORDER_REQUEST (order_request_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_owned for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_requestor_c for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_REQUESTOR_C FOREIGN KEY (requestor_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_ship_to_c for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_SHIP_TO_C FOREIGN KEY (ship_to_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_or_source_c for table order_request ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST ADD CONSTRAINT FK_OR_SOURCE_C FOREIGN KEY (source_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for order_request_action **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ora_created for table order_request_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ACTION ADD CONSTRAINT FK_ORA_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ora_modified for table order_request_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ACTION ADD CONSTRAINT FK_ORA_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ora_or for table order_request_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ACTION ADD CONSTRAINT FK_ORA_OR FOREIGN KEY (order_request_id) REFERENCES GRINGLOBAL.ORDER_REQUEST (order_request_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ora_owned for table order_request_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ACTION ADD CONSTRAINT FK_ORA_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for order_request_image **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_orimg_created for table order_request_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_IMAGE ADD CONSTRAINT FK_ORIMG_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_orimg_modified for table order_request_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_IMAGE ADD CONSTRAINT FK_ORIMG_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_orimg_orimgid for table order_request_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_IMAGE ADD CONSTRAINT FK_ORIMG_ORIMGID FOREIGN KEY (order_request_id) REFERENCES GRINGLOBAL.ORDER_REQUEST (order_request_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_orimg_owned for table order_request_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_IMAGE ADD CONSTRAINT FK_ORIMG_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for plant_introduction **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_pi_created for table plant_introduction ...') as Action from dual;
ALTER TABLE GRINGLOBAL.PLANT_INTRODUCTION ADD CONSTRAINT FK_PI_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_pi_modified for table plant_introduction ...') as Action from dual;
ALTER TABLE GRINGLOBAL.PLANT_INTRODUCTION ADD CONSTRAINT FK_PI_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_pi_owned for table plant_introduction ...') as Action from dual;
ALTER TABLE GRINGLOBAL.PLANT_INTRODUCTION ADD CONSTRAINT FK_PI_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for region **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_r_created for table region ...') as Action from dual;
ALTER TABLE GRINGLOBAL.REGION ADD CONSTRAINT FK_R_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_r_modified for table region ...') as Action from dual;
ALTER TABLE GRINGLOBAL.REGION ADD CONSTRAINT FK_R_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_r_owned for table region ...') as Action from dual;
ALTER TABLE GRINGLOBAL.REGION ADD CONSTRAINT FK_R_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for sec_dataview **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sr_created for table sec_dataview ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW ADD CONSTRAINT FK_SR_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sr_modified for table sec_dataview ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW ADD CONSTRAINT FK_SR_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sr_owned for table sec_dataview ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW ADD CONSTRAINT FK_SR_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for sec_dataview_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srl_created for table sec_dataview_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_LANG ADD CONSTRAINT FK_SRL_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srl_modified for table sec_dataview_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_LANG ADD CONSTRAINT FK_SRL_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srl_owned for table sec_dataview_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_LANG ADD CONSTRAINT FK_SRL_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srl_sr for table sec_dataview_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_LANG ADD CONSTRAINT FK_SRL_SR FOREIGN KEY (sec_dataview_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW (sec_dataview_id)
/

/********** 4 Constraint Definitions for sec_dataview_param **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srp_created for table sec_dataview_param ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_PARAM ADD CONSTRAINT FK_SRP_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srp_modified for table sec_dataview_param ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_PARAM ADD CONSTRAINT FK_SRP_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srp_owned for table sec_dataview_param ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_PARAM ADD CONSTRAINT FK_SRP_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srp_sr for table sec_dataview_param ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_PARAM ADD CONSTRAINT FK_SRP_SR FOREIGN KEY (sec_dataview_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW (sec_dataview_id)
/

/********** 4 Constraint Definitions for sec_dataview_sql **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srs_createdby for table sec_dataview_sql ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_SQL ADD CONSTRAINT FK_SRS_CREATEDBY FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srs_modifiedby for table sec_dataview_sql ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_SQL ADD CONSTRAINT FK_SRS_MODIFIEDBY FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srs_ownedby for table sec_dataview_sql ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_SQL ADD CONSTRAINT FK_SRS_OWNEDBY FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srs_sr for table sec_dataview_sql ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_SQL ADD CONSTRAINT FK_SRS_SR FOREIGN KEY (sec_dataview_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW (sec_dataview_id)
/

/********** 2 Constraint Definitions for sec_file_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sec_file_lang_sec_file for table sec_file_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_FILE_LANG ADD CONSTRAINT FK_SEC_FILE_LANG_SEC_FILE FOREIGN KEY (sec_file_id) REFERENCES GRINGLOBAL.SEC_FILE (sec_file_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sfl_sf for table sec_file_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_FILE_LANG ADD CONSTRAINT FK_SFL_SF FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

/********** 2 Constraint Definitions for sec_group_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sgl_sg for table sec_group_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_GROUP_LANG ADD CONSTRAINT FK_SGL_SG FOREIGN KEY (sec_group_id) REFERENCES GRINGLOBAL.SEC_GROUP (sec_group_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sgl_sl for table sec_group_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_GROUP_LANG ADD CONSTRAINT FK_SGL_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

/********** 3 Constraint Definitions for sec_table **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_st_created for table sec_table ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE ADD CONSTRAINT FK_ST_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_st_modified for table sec_table ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE ADD CONSTRAINT FK_ST_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_st_owned for table sec_table ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE ADD CONSTRAINT FK_ST_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for sec_table_field **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stf_created for table sec_table_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD ADD CONSTRAINT FK_STF_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stf_modified for table sec_table_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD ADD CONSTRAINT FK_STF_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stf_owned for table sec_table_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD ADD CONSTRAINT FK_STF_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stf_st for table sec_table_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD ADD CONSTRAINT FK_STF_ST FOREIGN KEY (sec_table_id) REFERENCES GRINGLOBAL.SEC_TABLE (sec_table_id)
/

/********** 5 Constraint Definitions for sec_table_field_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stfl_created for table sec_table_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD_LANG ADD CONSTRAINT FK_STFL_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stfl_modified for table sec_table_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD_LANG ADD CONSTRAINT FK_STFL_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stfl_owned for table sec_table_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD_LANG ADD CONSTRAINT FK_STFL_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stfl_sl for table sec_table_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD_LANG ADD CONSTRAINT FK_STFL_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_stfl_stf for table sec_table_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_FIELD_LANG ADD CONSTRAINT FK_STFL_STF FOREIGN KEY (sec_table_field_id) REFERENCES GRINGLOBAL.SEC_TABLE_FIELD (sec_table_field_id)
/

/********** 5 Constraint Definitions for sec_table_relationship **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_str_created for table sec_table_relationship ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_RELATIONSHIP ADD CONSTRAINT FK_STR_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_str_modified for table sec_table_relationship ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_RELATIONSHIP ADD CONSTRAINT FK_STR_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_str_owned for table sec_table_relationship ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_RELATIONSHIP ADD CONSTRAINT FK_STR_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_str_stf for table sec_table_relationship ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_RELATIONSHIP ADD CONSTRAINT FK_STR_STF FOREIGN KEY (sec_table_field_id) REFERENCES GRINGLOBAL.SEC_TABLE_FIELD (sec_table_field_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_str_stf_other for table sec_table_relationship ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_TABLE_RELATIONSHIP ADD CONSTRAINT FK_STR_STF_OTHER FOREIGN KEY (other_table_field_id) REFERENCES GRINGLOBAL.SEC_TABLE_FIELD (sec_table_field_id)
/

/********** 4 Constraint Definitions for sec_user **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_su_co for table sec_user ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER ADD CONSTRAINT FK_SU_CO FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_su_created for table sec_user ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER ADD CONSTRAINT FK_SU_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_su_modified for table sec_user ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER ADD CONSTRAINT FK_SU_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_su_owned for table sec_user ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER ADD CONSTRAINT FK_SU_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for sec_user_cart **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suc_created for table sec_user_cart ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART ADD CONSTRAINT FK_SUC_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suc_modified for table sec_user_cart ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART ADD CONSTRAINT FK_SUC_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suc_owned for table sec_user_cart ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART ADD CONSTRAINT FK_SUC_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suc_su for table sec_user_cart ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART ADD CONSTRAINT FK_SUC_SU FOREIGN KEY (sec_user_id) REFERENCES GRINGLOBAL.SEC_USER (sec_user_id)
/

/********** 4 Constraint Definitions for sec_user_cart_item **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suci_created for table sec_user_cart_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART_ITEM ADD CONSTRAINT FK_SUCI_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suci_modified for table sec_user_cart_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART_ITEM ADD CONSTRAINT FK_SUCI_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suci_owned for table sec_user_cart_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART_ITEM ADD CONSTRAINT FK_SUCI_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_suci_suc for table sec_user_cart_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_CART_ITEM ADD CONSTRAINT FK_SUCI_SUC FOREIGN KEY (sec_user_cart_id) REFERENCES GRINGLOBAL.SEC_USER_CART (sec_user_cart_id)
/

/********** 4 Constraint Definitions for site **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_s_contact for table site ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE ADD CONSTRAINT FK_S_CONTACT FOREIGN KEY (contact_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_s_created for table site ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE ADD CONSTRAINT FK_S_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_s_modified for table site ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE ADD CONSTRAINT FK_S_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_s_owned for table site ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE ADD CONSTRAINT FK_S_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for taxonomy_author **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ta_created for table taxonomy_author ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_AUTHOR ADD CONSTRAINT FK_TA_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ta_modified for table taxonomy_author ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_AUTHOR ADD CONSTRAINT FK_TA_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ta_owned for table taxonomy_author ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_AUTHOR ADD CONSTRAINT FK_TA_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for taxonomy_family **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tf_created for table taxonomy_family ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY ADD CONSTRAINT FK_TF_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tf_cur_tf for table taxonomy_family ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY ADD CONSTRAINT FK_TF_CUR_TF FOREIGN KEY (current_taxonomy_family_id) REFERENCES GRINGLOBAL.TAXONOMY_FAMILY (taxonomy_family_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tf_modified for table taxonomy_family ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY ADD CONSTRAINT FK_TF_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tf_owned for table taxonomy_family ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY ADD CONSTRAINT FK_TF_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for taxonomy_genus **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tg_created for table taxonomy_genus ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS ADD CONSTRAINT FK_TG_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tg_cur_tgt for table taxonomy_genus ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS ADD CONSTRAINT FK_TG_CUR_TGT FOREIGN KEY (current_taxonomy_genus_id) REFERENCES GRINGLOBAL.TAXONOMY_GENUS (taxonomy_genus_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tg_modified for table taxonomy_genus ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS ADD CONSTRAINT FK_TG_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tg_owned for table taxonomy_genus ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS ADD CONSTRAINT FK_TG_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tg_tf for table taxonomy_genus ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS ADD CONSTRAINT FK_TG_TF FOREIGN KEY (taxonomy_family_id) REFERENCES GRINGLOBAL.TAXONOMY_FAMILY (taxonomy_family_id)
/

/********** 4 Constraint Definitions for taxonomy_genus_type **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gt_tf for table taxonomy_genus_type ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_TYPE ADD CONSTRAINT FK_GT_TF FOREIGN KEY (taxonomy_family_id) REFERENCES GRINGLOBAL.TAXONOMY_FAMILY (taxonomy_family_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgt_created for table taxonomy_genus_type ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_TYPE ADD CONSTRAINT FK_TGT_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgt_modified for table taxonomy_genus_type ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_TYPE ADD CONSTRAINT FK_TGT_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgt_owned for table taxonomy_genus_type ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_TYPE ADD CONSTRAINT FK_TGT_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for url **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_u_created for table url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.URL ADD CONSTRAINT FK_U_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_u_modified for table url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.URL ADD CONSTRAINT FK_U_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_u_owned for table url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.URL ADD CONSTRAINT FK_U_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for accession_group **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ag_created for table accession_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_GROUP ADD CONSTRAINT FK_AG_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ag_modified for table accession_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_GROUP ADD CONSTRAINT FK_AG_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ag_owned for table accession_group ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_GROUP ADD CONSTRAINT FK_AG_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for app_resource **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_are_created for table app_resource ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_RESOURCE ADD CONSTRAINT FK_ARE_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_are_modified for table app_resource ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_RESOURCE ADD CONSTRAINT FK_ARE_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_are_owned for table app_resource ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_RESOURCE ADD CONSTRAINT FK_ARE_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_are_sl for table app_resource ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_RESOURCE ADD CONSTRAINT FK_ARE_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

/********** 4 Constraint Definitions for app_user_gui_setting **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sugs_co for table app_user_gui_setting ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_USER_GUI_SETTING ADD CONSTRAINT FK_SUGS_CO FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sugs_created for table app_user_gui_setting ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_USER_GUI_SETTING ADD CONSTRAINT FK_SUGS_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sugs_modified for table app_user_gui_setting ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_USER_GUI_SETTING ADD CONSTRAINT FK_SUGS_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sugs_owned for table app_user_gui_setting ...') as Action from dual;
ALTER TABLE GRINGLOBAL.APP_USER_GUI_SETTING ADD CONSTRAINT FK_SUGS_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for citation **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ci_created for table citation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CITATION ADD CONSTRAINT FK_CI_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ci_l for table citation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CITATION ADD CONSTRAINT FK_CI_L FOREIGN KEY (literature_id) REFERENCES GRINGLOBAL.LITERATURE (literature_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ci_modified for table citation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CITATION ADD CONSTRAINT FK_CI_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ci_owned for table citation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CITATION ADD CONSTRAINT FK_CI_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 3 Constraint Definitions for code_value **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdval_created for table code_value ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE ADD CONSTRAINT FK_CDVAL_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdval_modified for table code_value ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE ADD CONSTRAINT FK_CDVAL_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdval_owned for table code_value ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE ADD CONSTRAINT FK_CDVAL_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for code_value_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cvl_created for table code_value_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE_LANG ADD CONSTRAINT FK_CVL_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cvl_cv for table code_value_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE_LANG ADD CONSTRAINT FK_CVL_CV FOREIGN KEY (code_value_id) REFERENCES GRINGLOBAL.CODE_VALUE (code_value_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cvl_modified for table code_value_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE_LANG ADD CONSTRAINT FK_CVL_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cvl_owned for table code_value_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE_LANG ADD CONSTRAINT FK_CVL_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cvl_sl for table code_value_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_VALUE_LANG ADD CONSTRAINT FK_CVL_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

/********** 5 Constraint Definitions for genetic_marker_citation_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gmc_ci for table genetic_marker_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER_CITATION_MAP ADD CONSTRAINT FK_GMC_CI FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gmc_created for table genetic_marker_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER_CITATION_MAP ADD CONSTRAINT FK_GMC_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gmc_gm for table genetic_marker_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER_CITATION_MAP ADD CONSTRAINT FK_GMC_GM FOREIGN KEY (genetic_marker_id) REFERENCES GRINGLOBAL.GENETIC_MARKER (genetic_marker_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gmc_modified for table genetic_marker_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER_CITATION_MAP ADD CONSTRAINT FK_GMC_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_gmc_owned for table genetic_marker_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_MARKER_CITATION_MAP ADD CONSTRAINT FK_GMC_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for geography **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_g_created for table geography ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOGRAPHY ADD CONSTRAINT FK_G_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_g_cur_g for table geography ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOGRAPHY ADD CONSTRAINT FK_G_CUR_G FOREIGN KEY (current_geography_id) REFERENCES GRINGLOBAL.GEOGRAPHY (geography_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_g_modified for table geography ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOGRAPHY ADD CONSTRAINT FK_G_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_g_owned for table geography ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOGRAPHY ADD CONSTRAINT FK_G_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_g_re for table geography ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOGRAPHY ADD CONSTRAINT FK_G_RE FOREIGN KEY (region_id) REFERENCES GRINGLOBAL.REGION (region_id)
/

/********** 5 Constraint Definitions for georeference **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_georef_c for table georeference ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOREFERENCE ADD CONSTRAINT FK_GEOREF_C FOREIGN KEY (georeference_cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_georef_cit for table georeference ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOREFERENCE ADD CONSTRAINT FK_GEOREF_CIT FOREIGN KEY (georeference_citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_georef_created for table georeference ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOREFERENCE ADD CONSTRAINT FK_GEOREF_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_georef_modified for table georeference ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOREFERENCE ADD CONSTRAINT FK_GEOREF_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_georef_owned for table georeference ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GEOREFERENCE ADD CONSTRAINT FK_GEOREF_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for method **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_m_created for table method ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD ADD CONSTRAINT FK_M_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_m_g for table method ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD ADD CONSTRAINT FK_M_G FOREIGN KEY (geography_id) REFERENCES GRINGLOBAL.GEOGRAPHY (geography_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_m_modified for table method ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD ADD CONSTRAINT FK_M_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_m_owned for table method ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD ADD CONSTRAINT FK_M_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for method_citation_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mcm_ci for table method_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_CITATION_MAP ADD CONSTRAINT FK_MCM_CI FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mcm_created for table method_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_CITATION_MAP ADD CONSTRAINT FK_MCM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mcm_m for table method_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_CITATION_MAP ADD CONSTRAINT FK_MCM_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mcm_modified for table method_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_CITATION_MAP ADD CONSTRAINT FK_MCM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mcm_owned for table method_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_CITATION_MAP ADD CONSTRAINT FK_MCM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for method_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mm_c for table method_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_MAP ADD CONSTRAINT FK_MM_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mm_created for table method_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_MAP ADD CONSTRAINT FK_MM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mm_m for table method_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_MAP ADD CONSTRAINT FK_MM_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mm_modified for table method_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_MAP ADD CONSTRAINT FK_MM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_mm_owned for table method_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.METHOD_MAP ADD CONSTRAINT FK_MM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for sec_datatrigger **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sdt_created for table sec_datatrigger ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATATRIGGER ADD CONSTRAINT FK_SDT_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sdt_dv for table sec_datatrigger ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATATRIGGER ADD CONSTRAINT FK_SDT_DV FOREIGN KEY (sec_dataview_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW (sec_dataview_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sdt_modified for table sec_datatrigger ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATATRIGGER ADD CONSTRAINT FK_SDT_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sdt_owned for table sec_datatrigger ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATATRIGGER ADD CONSTRAINT FK_SDT_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sdt_st for table sec_datatrigger ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATATRIGGER ADD CONSTRAINT FK_SDT_ST FOREIGN KEY (sec_table_id) REFERENCES GRINGLOBAL.SEC_TABLE (sec_table_id)
/

/********** 5 Constraint Definitions for sec_dataview_field **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srf_created for table sec_dataview_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD ADD CONSTRAINT FK_SRF_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srf_modified for table sec_dataview_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD ADD CONSTRAINT FK_SRF_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srf_owned for table sec_dataview_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD ADD CONSTRAINT FK_SRF_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srf_sr for table sec_dataview_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD ADD CONSTRAINT FK_SRF_SR FOREIGN KEY (sec_dataview_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW (sec_dataview_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srf_stf for table sec_dataview_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD ADD CONSTRAINT FK_SRF_STF FOREIGN KEY (sec_table_field_id) REFERENCES GRINGLOBAL.SEC_TABLE_FIELD (sec_table_field_id)
/

/********** 5 Constraint Definitions for sec_dataview_field_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srfl_created for table sec_dataview_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG ADD CONSTRAINT FK_SRFL_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srfl_modified for table sec_dataview_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG ADD CONSTRAINT FK_SRFL_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srfl_owned for table sec_dataview_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG ADD CONSTRAINT FK_SRFL_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srfl_sl for table sec_dataview_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG ADD CONSTRAINT FK_SRFL_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_srfl_srf for table sec_dataview_field_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG ADD CONSTRAINT FK_SRFL_SRF FOREIGN KEY (sec_dataview_field_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW_FIELD (sec_dataview_field_id)
/

/********** 2 Constraint Definitions for sec_group_user_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sgum_sg for table sec_group_user_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_GROUP_USER_MAP ADD CONSTRAINT FK_SGUM_SG FOREIGN KEY (sec_group_id) REFERENCES GRINGLOBAL.SEC_GROUP (sec_group_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sgum_su for table sec_group_user_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_GROUP_USER_MAP ADD CONSTRAINT FK_SGUM_SU FOREIGN KEY (sec_user_id) REFERENCES GRINGLOBAL.SEC_USER (sec_user_id)
/

/********** 4 Constraint Definitions for sec_index **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_si_created for table sec_index ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX ADD CONSTRAINT FK_SI_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_si_modified for table sec_index ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX ADD CONSTRAINT FK_SI_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_si_owned for table sec_index ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX ADD CONSTRAINT FK_SI_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_si_st for table sec_index ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX ADD CONSTRAINT FK_SI_ST FOREIGN KEY (sec_table_id) REFERENCES GRINGLOBAL.SEC_TABLE (sec_table_id)
/

/********** 5 Constraint Definitions for sec_index_field **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sif_created for table sec_index_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX_FIELD ADD CONSTRAINT FK_SIF_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sif_modified for table sec_index_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX_FIELD ADD CONSTRAINT FK_SIF_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sif_owned for table sec_index_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX_FIELD ADD CONSTRAINT FK_SIF_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sif_si for table sec_index_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX_FIELD ADD CONSTRAINT FK_SIF_SI FOREIGN KEY (sec_index_id) REFERENCES GRINGLOBAL.SEC_INDEX (sec_index_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sif_stf for table sec_index_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_INDEX_FIELD ADD CONSTRAINT FK_SIF_STF FOREIGN KEY (sec_table_field_id) REFERENCES GRINGLOBAL.SEC_TABLE_FIELD (sec_table_field_id)
/

/********** 5 Constraint Definitions for sec_perm **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_created for table sec_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM ADD CONSTRAINT FK_SP_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_modified for table sec_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM ADD CONSTRAINT FK_SP_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_owned for table sec_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM ADD CONSTRAINT FK_SP_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_sr for table sec_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM ADD CONSTRAINT FK_SP_SR FOREIGN KEY (sec_dataview_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW (sec_dataview_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_st for table sec_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM ADD CONSTRAINT FK_SP_ST FOREIGN KEY (sec_table_id) REFERENCES GRINGLOBAL.SEC_TABLE (sec_table_id)
/

/********** 6 Constraint Definitions for sec_perm_field **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_srf for table sec_perm_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_FIELD ADD CONSTRAINT FK_SP_SRF FOREIGN KEY (sec_dataview_field_id) REFERENCES GRINGLOBAL.SEC_DATAVIEW_FIELD (sec_dataview_field_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sp_stf for table sec_perm_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_FIELD ADD CONSTRAINT FK_SP_STF FOREIGN KEY (sec_table_field_id) REFERENCES GRINGLOBAL.SEC_TABLE_FIELD (sec_table_field_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_spf_created for table sec_perm_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_FIELD ADD CONSTRAINT FK_SPF_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_spf_modified for table sec_perm_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_FIELD ADD CONSTRAINT FK_SPF_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_spf_owned for table sec_perm_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_FIELD ADD CONSTRAINT FK_SPF_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_spf_sp for table sec_perm_field ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_FIELD ADD CONSTRAINT FK_SPF_SP FOREIGN KEY (sec_perm_id) REFERENCES GRINGLOBAL.SEC_PERM (sec_perm_id)
/

/********** 2 Constraint Definitions for sec_perm_lang **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_spl_sl for table sec_perm_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_LANG ADD CONSTRAINT FK_SPL_SL FOREIGN KEY (sec_lang_id) REFERENCES GRINGLOBAL.SEC_LANG (sec_lang_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_spl_sp for table sec_perm_lang ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_PERM_LANG ADD CONSTRAINT FK_SPL_SP FOREIGN KEY (sec_perm_id) REFERENCES GRINGLOBAL.SEC_PERM (sec_perm_id)
/

/********** 5 Constraint Definitions for sec_user_perm **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sup_created for table sec_user_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_PERM ADD CONSTRAINT FK_SUP_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sup_modified for table sec_user_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_PERM ADD CONSTRAINT FK_SUP_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sup_owned for table sec_user_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_PERM ADD CONSTRAINT FK_SUP_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sup_sp for table sec_user_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_PERM ADD CONSTRAINT FK_SUP_SP FOREIGN KEY (sec_perm_id) REFERENCES GRINGLOBAL.SEC_PERM (sec_perm_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sup_su for table sec_user_perm ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_USER_PERM ADD CONSTRAINT FK_SUP_SU FOREIGN KEY (sec_user_id) REFERENCES GRINGLOBAL.SEC_USER (sec_user_id)
/

/********** 7 Constraint Definitions for taxonomy **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_c for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_created for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_cur_t for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_CUR_T FOREIGN KEY (current_taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_modified for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_owned for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_tcr for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_TCR FOREIGN KEY (crop_id) REFERENCES GRINGLOBAL.CROP (crop_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_t_tg for table taxonomy ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY ADD CONSTRAINT FK_T_TG FOREIGN KEY (taxonomy_genus_id) REFERENCES GRINGLOBAL.TAXONOMY_GENUS (taxonomy_genus_id)
/

/********** 5 Constraint Definitions for taxonomy_citation_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcm_ci for table taxonomy_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_CITATION_MAP ADD CONSTRAINT FK_TCM_CI FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcm_created for table taxonomy_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_CITATION_MAP ADD CONSTRAINT FK_TCM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcm_modified for table taxonomy_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_CITATION_MAP ADD CONSTRAINT FK_TCM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcm_owned for table taxonomy_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_CITATION_MAP ADD CONSTRAINT FK_TCM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcm_t for table taxonomy_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_CITATION_MAP ADD CONSTRAINT FK_TCM_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 4 Constraint Definitions for taxonomy_common_name **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcn_created for table taxonomy_common_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_COMMON_NAME ADD CONSTRAINT FK_TCN_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcn_modified for table taxonomy_common_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_COMMON_NAME ADD CONSTRAINT FK_TCN_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcn_owned for table taxonomy_common_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_COMMON_NAME ADD CONSTRAINT FK_TCN_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tcn_t for table taxonomy_common_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_COMMON_NAME ADD CONSTRAINT FK_TCN_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 5 Constraint Definitions for taxonomy_distribution **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_td_created for table taxonomy_distribution ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_DISTRIBUTION ADD CONSTRAINT FK_TD_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_td_g for table taxonomy_distribution ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_DISTRIBUTION ADD CONSTRAINT FK_TD_G FOREIGN KEY (geography_id) REFERENCES GRINGLOBAL.GEOGRAPHY (geography_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_td_modified for table taxonomy_distribution ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_DISTRIBUTION ADD CONSTRAINT FK_TD_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_td_owned for table taxonomy_distribution ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_DISTRIBUTION ADD CONSTRAINT FK_TD_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_td_t for table taxonomy_distribution ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_DISTRIBUTION ADD CONSTRAINT FK_TD_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 5 Constraint Definitions for taxonomy_family_cit_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tfcm_ci for table taxonomy_family_cit_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY_CIT_MAP ADD CONSTRAINT FK_TFCM_CI FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tfcm_created for table taxonomy_family_cit_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY_CIT_MAP ADD CONSTRAINT FK_TFCM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tfcm_modified for table taxonomy_family_cit_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY_CIT_MAP ADD CONSTRAINT FK_TFCM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tfcm_owned for table taxonomy_family_cit_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY_CIT_MAP ADD CONSTRAINT FK_TFCM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tfcm_tf for table taxonomy_family_cit_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_FAMILY_CIT_MAP ADD CONSTRAINT FK_TFCM_TF FOREIGN KEY (taxonomy_family_id) REFERENCES GRINGLOBAL.TAXONOMY_FAMILY (taxonomy_family_id)
/

/********** 5 Constraint Definitions for taxonomy_genus_citation_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgc_ci for table taxonomy_genus_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP ADD CONSTRAINT FK_TGC_CI FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgc_created for table taxonomy_genus_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP ADD CONSTRAINT FK_TGC_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgc_modified for table taxonomy_genus_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP ADD CONSTRAINT FK_TGC_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgc_owned for table taxonomy_genus_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP ADD CONSTRAINT FK_TGC_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgc_tg for table taxonomy_genus_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP ADD CONSTRAINT FK_TGC_TG FOREIGN KEY (taxonomy_genus_id) REFERENCES GRINGLOBAL.TAXONOMY_GENUS (taxonomy_genus_id)
/

/********** 4 Constraint Definitions for taxonomy_germination_rule **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgr_created for table taxonomy_germination_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GERMINATION_RULE ADD CONSTRAINT FK_TGR_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgr_modified for table taxonomy_germination_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GERMINATION_RULE ADD CONSTRAINT FK_TGR_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgr_owned for table taxonomy_germination_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GERMINATION_RULE ADD CONSTRAINT FK_TGR_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tgr_t for table taxonomy_germination_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_GERMINATION_RULE ADD CONSTRAINT FK_TGR_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 7 Constraint Definitions for taxonomy_url **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_created for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_modified for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_owned for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_t for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_tf for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_TF FOREIGN KEY (taxonomy_family_id) REFERENCES GRINGLOBAL.TAXONOMY_FAMILY (taxonomy_family_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_tg for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_TG FOREIGN KEY (taxonomy_genus_id) REFERENCES GRINGLOBAL.TAXONOMY_GENUS (taxonomy_genus_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tu_u for table taxonomy_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_URL ADD CONSTRAINT FK_TU_U FOREIGN KEY (url_id) REFERENCES GRINGLOBAL.URL (url_id)
/

/********** 4 Constraint Definitions for taxonomy_use **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tus_created for table taxonomy_use ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_USE ADD CONSTRAINT FK_TUS_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tus_modified for table taxonomy_use ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_USE ADD CONSTRAINT FK_TUS_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tus_owned for table taxonomy_use ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_USE ADD CONSTRAINT FK_TUS_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_tus_t for table taxonomy_use ...') as Action from dual;
ALTER TABLE GRINGLOBAL.TAXONOMY_USE ADD CONSTRAINT FK_TUS_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 5 Constraint Definitions for accession **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_a_created for table accession ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION ADD CONSTRAINT FK_A_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_a_modified for table accession ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION ADD CONSTRAINT FK_A_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_a_owned for table accession ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION ADD CONSTRAINT FK_A_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_a_pi for table accession ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION ADD CONSTRAINT FK_A_PI FOREIGN KEY (plant_introduction_id) REFERENCES GRINGLOBAL.PLANT_INTRODUCTION (plant_introduction_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_a_t for table accession ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION ADD CONSTRAINT FK_A_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 6 Constraint Definitions for accession_action **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aa_a for table accession_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ACTION ADD CONSTRAINT FK_AA_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aa_c for table accession_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ACTION ADD CONSTRAINT FK_AA_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aa_created for table accession_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ACTION ADD CONSTRAINT FK_AA_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aa_m for table accession_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ACTION ADD CONSTRAINT FK_AA_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aa_modified for table accession_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ACTION ADD CONSTRAINT FK_AA_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aa_owned for table accession_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ACTION ADD CONSTRAINT FK_AA_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for accession_citation_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_acm_a for table accession_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_CITATION_MAP ADD CONSTRAINT FK_ACM_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_acm_ci for table accession_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_CITATION_MAP ADD CONSTRAINT FK_ACM_CI FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_acm_created for table accession_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_CITATION_MAP ADD CONSTRAINT FK_ACM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_acm_modified for table accession_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_CITATION_MAP ADD CONSTRAINT FK_ACM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_acm_owned for table accession_citation_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_CITATION_MAP ADD CONSTRAINT FK_ACM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for accession_habitat **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ah_a for table accession_habitat ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_HABITAT ADD CONSTRAINT FK_AH_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ah_created for table accession_habitat ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_HABITAT ADD CONSTRAINT FK_AH_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ah_modified for table accession_habitat ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_HABITAT ADD CONSTRAINT FK_AH_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ah_owned for table accession_habitat ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_HABITAT ADD CONSTRAINT FK_AH_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 6 Constraint Definitions for accession_ipr **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ar_a for table accession_ipr ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_IPR ADD CONSTRAINT FK_AR_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ar_ac for table accession_ipr ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_IPR ADD CONSTRAINT FK_AR_AC FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ar_c for table accession_ipr ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_IPR ADD CONSTRAINT FK_AR_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ar_created for table accession_ipr ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_IPR ADD CONSTRAINT FK_AR_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ar_modified for table accession_ipr ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_IPR ADD CONSTRAINT FK_AR_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ar_owned for table accession_ipr ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_IPR ADD CONSTRAINT FK_AR_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for accession_narrative **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ana_a for table accession_narrative ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NARRATIVE ADD CONSTRAINT FK_ANA_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ana_created for table accession_narrative ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NARRATIVE ADD CONSTRAINT FK_ANA_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ana_modified for table accession_narrative ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NARRATIVE ADD CONSTRAINT FK_ANA_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ana_owned for table accession_narrative ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NARRATIVE ADD CONSTRAINT FK_ANA_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for accession_pedigree **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ap_a for table accession_pedigree ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_PEDIGREE ADD CONSTRAINT FK_AP_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ap_ac for table accession_pedigree ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_PEDIGREE ADD CONSTRAINT FK_AP_AC FOREIGN KEY (citation_id) REFERENCES GRINGLOBAL.CITATION (citation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ap_created for table accession_pedigree ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_PEDIGREE ADD CONSTRAINT FK_AP_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ap_modified for table accession_pedigree ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_PEDIGREE ADD CONSTRAINT FK_AP_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ap_owned for table accession_pedigree ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_PEDIGREE ADD CONSTRAINT FK_AP_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for accession_quarantine **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aq_a for table accession_quarantine ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_QUARANTINE ADD CONSTRAINT FK_AQ_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aq_c for table accession_quarantine ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_QUARANTINE ADD CONSTRAINT FK_AQ_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aq_created for table accession_quarantine ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_QUARANTINE ADD CONSTRAINT FK_AQ_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aq_modified for table accession_quarantine ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_QUARANTINE ADD CONSTRAINT FK_AQ_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aq_owned for table accession_quarantine ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_QUARANTINE ADD CONSTRAINT FK_AQ_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 6 Constraint Definitions for accession_source **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_src_a for table accession_source ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE ADD CONSTRAINT FK_SRC_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_src_created for table accession_source ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE ADD CONSTRAINT FK_SRC_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_src_g for table accession_source ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE ADD CONSTRAINT FK_SRC_G FOREIGN KEY (geography_id) REFERENCES GRINGLOBAL.GEOGRAPHY (geography_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_src_gr for table accession_source ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE ADD CONSTRAINT FK_SRC_GR FOREIGN KEY (georeference_id) REFERENCES GRINGLOBAL.GEOREFERENCE (georeference_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_src_modified for table accession_source ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE ADD CONSTRAINT FK_SRC_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_src_owned for table accession_source ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE ADD CONSTRAINT FK_SRC_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for accession_source_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_asm_as for table accession_source_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE_MAP ADD CONSTRAINT FK_ASM_AS FOREIGN KEY (accession_source_id) REFERENCES GRINGLOBAL.ACCESSION_SOURCE (accession_source_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_asm_c for table accession_source_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE_MAP ADD CONSTRAINT FK_ASM_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_asm_created for table accession_source_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE_MAP ADD CONSTRAINT FK_ASM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_asm_modified for table accession_source_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE_MAP ADD CONSTRAINT FK_ASM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_asm_owned for table accession_source_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_SOURCE_MAP ADD CONSTRAINT FK_ASM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for code_rule **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdrule_created for table code_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_RULE ADD CONSTRAINT FK_CDRULE_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdrule_cv for table code_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_RULE ADD CONSTRAINT FK_CDRULE_CV FOREIGN KEY (code_value_id) REFERENCES GRINGLOBAL.CODE_VALUE (code_value_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdrule_modified for table code_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_RULE ADD CONSTRAINT FK_CDRULE_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cdrule_owned for table code_rule ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CODE_RULE ADD CONSTRAINT FK_CDRULE_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 7 Constraint Definitions for crop_trait_url **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_cr for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_CR FOREIGN KEY (crop_id) REFERENCES GRINGLOBAL.CROP (crop_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_created for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_ct for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_CT FOREIGN KEY (crop_trait_id) REFERENCES GRINGLOBAL.CROP_TRAIT (crop_trait_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_m for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_modified for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_owned for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ctu_u for table crop_trait_url ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_URL ADD CONSTRAINT FK_CTU_U FOREIGN KEY (url_id) REFERENCES GRINGLOBAL.URL (url_id)
/

/********** 5 Constraint Definitions for genetic_annotation **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ga_created for table genetic_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_ANNOTATION ADD CONSTRAINT FK_GA_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ga_gm for table genetic_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_ANNOTATION ADD CONSTRAINT FK_GA_GM FOREIGN KEY (marker_id) REFERENCES GRINGLOBAL.GENETIC_MARKER (genetic_marker_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ga_m for table genetic_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_ANNOTATION ADD CONSTRAINT FK_GA_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ga_modified for table genetic_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_ANNOTATION ADD CONSTRAINT FK_GA_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ga_owned for table genetic_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_ANNOTATION ADD CONSTRAINT FK_GA_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 8 Constraint Definitions for inventory **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_a for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_backup_i for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_BACKUP_I FOREIGN KEY (backup_inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_c for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_created for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_im for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_IM FOREIGN KEY (inventory_maint_policy_id) REFERENCES GRINGLOBAL.INVENTORY_MAINT_POLICY (inventory_maint_policy_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_modified for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_owned for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_i_parent_i for table inventory ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY ADD CONSTRAINT FK_I_PARENT_I FOREIGN KEY (parent_inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

/********** 6 Constraint Definitions for inventory_action **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ia_c for table inventory_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_ACTION ADD CONSTRAINT FK_IA_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ia_created for table inventory_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_ACTION ADD CONSTRAINT FK_IA_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ia_i for table inventory_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_ACTION ADD CONSTRAINT FK_IA_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ia_m for table inventory_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_ACTION ADD CONSTRAINT FK_IA_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ia_modified for table inventory_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_ACTION ADD CONSTRAINT FK_IA_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ia_owned for table inventory_action ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_ACTION ADD CONSTRAINT FK_IA_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for inventory_group_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_igm_created for table inventory_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP_MAP ADD CONSTRAINT FK_IGM_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_igm_i for table inventory_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP_MAP ADD CONSTRAINT FK_IGM_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_igm_ig for table inventory_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP_MAP ADD CONSTRAINT FK_IGM_IG FOREIGN KEY (inventory_group_id) REFERENCES GRINGLOBAL.INVENTORY_GROUP (inventory_group_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_igm_modified for table inventory_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP_MAP ADD CONSTRAINT FK_IGM_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_igm_owned for table inventory_group_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_GROUP_MAP ADD CONSTRAINT FK_IGM_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for inventory_image **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ii_created for table inventory_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_IMAGE ADD CONSTRAINT FK_II_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ii_iid for table inventory_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_IMAGE ADD CONSTRAINT FK_II_IID FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ii_modified for table inventory_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_IMAGE ADD CONSTRAINT FK_II_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ii_owned for table inventory_image ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_IMAGE ADD CONSTRAINT FK_II_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 4 Constraint Definitions for inventory_quality_status **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iqs_created for table inventory_quality_status ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_QUALITY_STATUS ADD CONSTRAINT FK_IQS_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iqs_i for table inventory_quality_status ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_QUALITY_STATUS ADD CONSTRAINT FK_IQS_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iqs_modified for table inventory_quality_status ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_QUALITY_STATUS ADD CONSTRAINT FK_IQS_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iqs_owned for table inventory_quality_status ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_QUALITY_STATUS ADD CONSTRAINT FK_IQS_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for inventory_viability **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iv_created for table inventory_viability ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_VIABILITY ADD CONSTRAINT FK_IV_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iv_i for table inventory_viability ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_VIABILITY ADD CONSTRAINT FK_IV_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iv_m for table inventory_viability ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_VIABILITY ADD CONSTRAINT FK_IV_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iv_modified for table inventory_viability ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_VIABILITY ADD CONSTRAINT FK_IV_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_iv_owned for table inventory_viability ...') as Action from dual;
ALTER TABLE GRINGLOBAL.INVENTORY_VIABILITY ADD CONSTRAINT FK_IV_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 6 Constraint Definitions for order_request_item **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ori_created for table order_request_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ITEM ADD CONSTRAINT FK_ORI_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ori_i for table order_request_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ITEM ADD CONSTRAINT FK_ORI_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ori_modified for table order_request_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ITEM ADD CONSTRAINT FK_ORI_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ori_or for table order_request_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ITEM ADD CONSTRAINT FK_ORI_OR FOREIGN KEY (order_request_id) REFERENCES GRINGLOBAL.ORDER_REQUEST (order_request_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ori_owned for table order_request_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ITEM ADD CONSTRAINT FK_ORI_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_ori_t for table order_request_item ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ORDER_REQUEST_ITEM ADD CONSTRAINT FK_ORI_T FOREIGN KEY (taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 2 Constraint Definitions for sec_group_perm_map **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sgpm_sg for table sec_group_perm_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_GROUP_PERM_MAP ADD CONSTRAINT FK_SGPM_SG FOREIGN KEY (sec_group_id) REFERENCES GRINGLOBAL.SEC_GROUP (sec_group_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sgpm_sp for table sec_group_perm_map ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SEC_GROUP_PERM_MAP ADD CONSTRAINT FK_SGPM_SP FOREIGN KEY (sec_perm_id) REFERENCES GRINGLOBAL.SEC_PERM (sec_perm_id)
/

/********** 4 Constraint Definitions for site_inventory_nc7 **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sin_created for table site_inventory_nc7 ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE_INVENTORY_NC7 ADD CONSTRAINT FK_SIN_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sin_i for table site_inventory_nc7 ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE_INVENTORY_NC7 ADD CONSTRAINT FK_SIN_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sin_modified for table site_inventory_nc7 ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE_INVENTORY_NC7 ADD CONSTRAINT FK_SIN_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_sin_owned for table site_inventory_nc7 ...') as Action from dual;
ALTER TABLE GRINGLOBAL.SITE_INVENTORY_NC7 ADD CONSTRAINT FK_SIN_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 8 Constraint Definitions for accession_annotation **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_c for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_created for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_i for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_modified for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_or for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_OR FOREIGN KEY (order_request_id) REFERENCES GRINGLOBAL.ORDER_REQUEST (order_request_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_owned for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_t_new for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_T_NEW FOREIGN KEY (new_taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_aan_t_old for table accession_annotation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_ANNOTATION ADD CONSTRAINT FK_AAN_T_OLD FOREIGN KEY (old_taxonomy_id) REFERENCES GRINGLOBAL.TAXONOMY (taxonomy_id)
/

/********** 7 Constraint Definitions for accession_name **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_a for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_A FOREIGN KEY (accession_id) REFERENCES GRINGLOBAL.ACCESSION (accession_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_ag for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_AG FOREIGN KEY (accession_group_id) REFERENCES GRINGLOBAL.ACCESSION_GROUP (accession_group_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_c for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_created for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_i for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_modified for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_an_owned for table accession_name ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_NAME ADD CONSTRAINT FK_AN_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for accession_voucher **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_av_c for table accession_voucher ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_VOUCHER ADD CONSTRAINT FK_AV_C FOREIGN KEY (cooperator_id) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_av_created for table accession_voucher ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_VOUCHER ADD CONSTRAINT FK_AV_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_av_i for table accession_voucher ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_VOUCHER ADD CONSTRAINT FK_AV_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_av_modified for table accession_voucher ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_VOUCHER ADD CONSTRAINT FK_AV_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_av_owned for table accession_voucher ...') as Action from dual;
ALTER TABLE GRINGLOBAL.ACCESSION_VOUCHER ADD CONSTRAINT FK_AV_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 8 Constraint Definitions for crop_trait_observation **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_created for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_ct for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_CT FOREIGN KEY (crop_trait_id) REFERENCES GRINGLOBAL.CROP_TRAIT (crop_trait_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_ctc for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_CTC FOREIGN KEY (crop_trait_code_id) REFERENCES GRINGLOBAL.CROP_TRAIT_CODE (crop_trait_code_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_ctq for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_CTQ FOREIGN KEY (crop_trait_qualifier_id) REFERENCES GRINGLOBAL.CROP_TRAIT_QUALIFIER (crop_trait_qualifier_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_i for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_m for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_M FOREIGN KEY (method_id) REFERENCES GRINGLOBAL.METHOD (method_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_modified for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_cto_owned for table crop_trait_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.CROP_TRAIT_OBSERVATION ADD CONSTRAINT FK_CTO_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

/********** 5 Constraint Definitions for genetic_observation **********/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_go_created for table genetic_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_OBSERVATION ADD CONSTRAINT FK_GO_CREATED FOREIGN KEY (created_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_go_ga for table genetic_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_OBSERVATION ADD CONSTRAINT FK_GO_GA FOREIGN KEY (genetic_annotation_id) REFERENCES GRINGLOBAL.GENETIC_ANNOTATION (genetic_annotation_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_go_i for table genetic_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_OBSERVATION ADD CONSTRAINT FK_GO_I FOREIGN KEY (inventory_id) REFERENCES GRINGLOBAL.INVENTORY (inventory_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_go_modified for table genetic_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_OBSERVATION ADD CONSTRAINT FK_GO_MODIFIED FOREIGN KEY (modified_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint fk_go_owned for table genetic_observation ...') as Action from dual;
ALTER TABLE GRINGLOBAL.GENETIC_OBSERVATION ADD CONSTRAINT FK_GO_OWNED FOREIGN KEY (owned_by) REFERENCES GRINGLOBAL.COOPERATOR (cooperator_id)
/

