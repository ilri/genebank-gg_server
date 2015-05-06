 /***********************************************/
/************** Index Definitions **************/
/***********************************************/

/************ No index definitions exist for app_setting *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table app_setting') as Action from dual;
/************ 6 Index Definitions for app_user_item_list *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_auil_c for table app_user_item_list ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AUIL_C  ON GRINGLOBAL.APP_USER_ITEM_LIST (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_auil_created for table app_user_item_list ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AUIL_CREATED  ON GRINGLOBAL.APP_USER_ITEM_LIST (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_auil_modified for table app_user_item_list ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AUIL_MODIFIED  ON GRINGLOBAL.APP_USER_ITEM_LIST (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_auil_owned for table app_user_item_list ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AUIL_OWNED  ON GRINGLOBAL.APP_USER_ITEM_LIST (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uil_group for table app_user_item_list ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_UIL_GROUP  ON GRINGLOBAL.APP_USER_ITEM_LIST (cooperator_id, list_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uil_tab for table app_user_item_list ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_UIL_TAB  ON GRINGLOBAL.APP_USER_ITEM_LIST (cooperator_id, tab_name, list_name)
/


/************ 1 Index Definitions for sec_db *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdb for table sec_db ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDB  ON GRINGLOBAL.SEC_DB (migration_number)
/


/************ 1 Index Definitions for sec_db_migration *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdbm for table sec_db_migration ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDBM  ON GRINGLOBAL.SEC_DB_MIGRATION (migration_number, sort_order)
/


/************ 1 Index Definitions for sec_db_migration_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdbml for table sec_db_migration_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDBML  ON GRINGLOBAL.SEC_DB_MIGRATION_LANG (sec_db_migration_id, language_iso_639_3_code)
/


/************ 1 Index Definitions for sec_file *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sf for table sec_file ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SF  ON GRINGLOBAL.SEC_FILE (virtual_file_path)
/


/************ 1 Index Definitions for sec_file_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sfg for table sec_file_group ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SFG  ON GRINGLOBAL.SEC_FILE_GROUP (group_name)
/


/************ 1 Index Definitions for sec_file_group_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_sfgm for table sec_file_group_map ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_SFGM  ON GRINGLOBAL.SEC_FILE_GROUP_MAP (sec_file_group_id, sec_file_id)
/


/************ No index definitions exist for sec_group *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table sec_group') as Action from dual;
/************ 5 Index Definitions for sec_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sl_created for table sec_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SL_CREATED  ON GRINGLOBAL.SEC_LANG (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sl_modified for table sec_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SL_MODIFIED  ON GRINGLOBAL.SEC_LANG (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sl_owned for table sec_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SL_OWNED  ON GRINGLOBAL.SEC_LANG (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sl_code for table sec_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SL_CODE  ON GRINGLOBAL.SEC_LANG (iso_639_3_code)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sl_tag for table sec_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SL_TAG  ON GRINGLOBAL.SEC_LANG (ietf_tag)
/


/************ 7 Index Definitions for cooperator *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_co_full_name for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_CO_FULL_NAME  ON GRINGLOBAL.COOPERATOR (last_name, first_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_co_org_code for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_CO_ORG_CODE  ON GRINGLOBAL.COOPERATOR (organization_code)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_c_created for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_C_CREATED  ON GRINGLOBAL.COOPERATOR (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_c_cur_c for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_C_CUR_C  ON GRINGLOBAL.COOPERATOR (current_cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_c_modified for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_C_MODIFIED  ON GRINGLOBAL.COOPERATOR (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_c_owned for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_C_OWNED  ON GRINGLOBAL.COOPERATOR (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_c_sl for table cooperator ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_C_SL  ON GRINGLOBAL.COOPERATOR (sec_lang_id)
/


/************ 4 Index Definitions for cooperator_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cg_created for table cooperator_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CG_CREATED  ON GRINGLOBAL.COOPERATOR_GROUP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cg_modified for table cooperator_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CG_MODIFIED  ON GRINGLOBAL.COOPERATOR_GROUP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cg_owned for table cooperator_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CG_OWNED  ON GRINGLOBAL.COOPERATOR_GROUP (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_cg_name for table cooperator_group ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_CG_NAME  ON GRINGLOBAL.COOPERATOR_GROUP (name)
/


/************ 5 Index Definitions for cooperator_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cm_c for table cooperator_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CM_C  ON GRINGLOBAL.COOPERATOR_MAP (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cm_cg for table cooperator_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CM_CG  ON GRINGLOBAL.COOPERATOR_MAP (cooperator_group_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cm_created for table cooperator_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CM_CREATED  ON GRINGLOBAL.COOPERATOR_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cm_modified for table cooperator_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CM_MODIFIED  ON GRINGLOBAL.COOPERATOR_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cm_owned for table cooperator_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CM_OWNED  ON GRINGLOBAL.COOPERATOR_MAP (owned_by)
/


/************ 4 Index Definitions for crop *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cr_created for table crop ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CR_CREATED  ON GRINGLOBAL.CROP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cr_modified for table crop ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CR_MODIFIED  ON GRINGLOBAL.CROP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cr_owned for table crop ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CR_OWNED  ON GRINGLOBAL.CROP (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_tcr_name for table crop ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_TCR_NAME  ON GRINGLOBAL.CROP (name)
/


/************ 5 Index Definitions for crop_trait *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ct_cr for table crop_trait ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CT_CR  ON GRINGLOBAL.CROP_TRAIT (crop_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ct_created for table crop_trait ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CT_CREATED  ON GRINGLOBAL.CROP_TRAIT (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ct_modified for table crop_trait ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CT_MODIFIED  ON GRINGLOBAL.CROP_TRAIT (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ct_owned for table crop_trait ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CT_OWNED  ON GRINGLOBAL.CROP_TRAIT (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ct for table crop_trait ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_CT  ON GRINGLOBAL.CROP_TRAIT (short_name, crop_id)
/


/************ 4 Index Definitions for crop_trait_code *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tct_created for table crop_trait_code ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCT_CREATED  ON GRINGLOBAL.CROP_TRAIT_CODE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tct_modified for table crop_trait_code ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCT_MODIFIED  ON GRINGLOBAL.CROP_TRAIT_CODE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tct_owned for table crop_trait_code ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCT_OWNED  ON GRINGLOBAL.CROP_TRAIT_CODE (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tct_tr for table crop_trait_code ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCT_TR  ON GRINGLOBAL.CROP_TRAIT_CODE (crop_trait_id)
/


/************ 6 Index Definitions for crop_trait_code_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctcl_created for table crop_trait_code_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTCL_CREATED  ON GRINGLOBAL.CROP_TRAIT_CODE_LANG (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctcl_modified for table crop_trait_code_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTCL_MODIFIED  ON GRINGLOBAL.CROP_TRAIT_CODE_LANG (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctcl_owned for table crop_trait_code_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTCL_OWNED  ON GRINGLOBAL.CROP_TRAIT_CODE_LANG (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctcl_sl for table crop_trait_code_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTCL_SL  ON GRINGLOBAL.CROP_TRAIT_CODE_LANG (sec_lang_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctcl_tc for table crop_trait_code_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTCL_TC  ON GRINGLOBAL.CROP_TRAIT_CODE_LANG (crop_trait_code_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ctcl for table crop_trait_code_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_CTCL  ON GRINGLOBAL.CROP_TRAIT_CODE_LANG (crop_trait_code_id, sec_lang_id)
/


/************ 5 Index Definitions for crop_trait_qualifier *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ctq_name for table crop_trait_qualifier ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_CTQ_NAME  ON GRINGLOBAL.CROP_TRAIT_QUALIFIER (name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctq_created for table crop_trait_qualifier ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTQ_CREATED  ON GRINGLOBAL.CROP_TRAIT_QUALIFIER (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctq_ct for table crop_trait_qualifier ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTQ_CT  ON GRINGLOBAL.CROP_TRAIT_QUALIFIER (crop_trait_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctq_modified for table crop_trait_qualifier ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTQ_MODIFIED  ON GRINGLOBAL.CROP_TRAIT_QUALIFIER (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctq_owned for table crop_trait_qualifier ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTQ_OWNED  ON GRINGLOBAL.CROP_TRAIT_QUALIFIER (owned_by)
/


/************ 5 Index Definitions for genetic_marker *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gm_created for table genetic_marker ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GM_CREATED  ON GRINGLOBAL.GENETIC_MARKER (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gm_modified for table genetic_marker ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GM_MODIFIED  ON GRINGLOBAL.GENETIC_MARKER (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gm_owned for table genetic_marker ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GM_OWNED  ON GRINGLOBAL.GENETIC_MARKER (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gm_tcr for table genetic_marker ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GM_TCR  ON GRINGLOBAL.GENETIC_MARKER (crop_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_gm_crop for table genetic_marker ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_GM_CROP  ON GRINGLOBAL.GENETIC_MARKER (crop_id, name)
/


/************ 3 Index Definitions for inventory_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ig_created for table inventory_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IG_CREATED  ON GRINGLOBAL.INVENTORY_GROUP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ig_modified for table inventory_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IG_MODIFIED  ON GRINGLOBAL.INVENTORY_GROUP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ig_owned for table inventory_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IG_OWNED  ON GRINGLOBAL.INVENTORY_GROUP (owned_by)
/


/************ 4 Index Definitions for inventory_maint_policy *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_im_co for table inventory_maint_policy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IM_CO  ON GRINGLOBAL.INVENTORY_MAINT_POLICY (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_im_created for table inventory_maint_policy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IM_CREATED  ON GRINGLOBAL.INVENTORY_MAINT_POLICY (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_im_modified for table inventory_maint_policy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IM_MODIFIED  ON GRINGLOBAL.INVENTORY_MAINT_POLICY (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_im_owned for table inventory_maint_policy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IM_OWNED  ON GRINGLOBAL.INVENTORY_MAINT_POLICY (owned_by)
/


/************ 3 Index Definitions for literature *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_l_created for table literature ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_L_CREATED  ON GRINGLOBAL.LITERATURE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_l_modified for table literature ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_L_MODIFIED  ON GRINGLOBAL.LITERATURE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_l_owned for table literature ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_L_OWNED  ON GRINGLOBAL.LITERATURE (owned_by)
/


/************ 10 Index Definitions for order_request *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_created for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_CREATED  ON GRINGLOBAL.ORDER_REQUEST (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_final_c for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_FINAL_C  ON GRINGLOBAL.ORDER_REQUEST (final_recipient_cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_modified for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_MODIFIED  ON GRINGLOBAL.ORDER_REQUEST (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_original_or for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_ORIGINAL_OR  ON GRINGLOBAL.ORDER_REQUEST (original_order_request_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_owned for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_OWNED  ON GRINGLOBAL.ORDER_REQUEST (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_requestor_c for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_REQUESTOR_C  ON GRINGLOBAL.ORDER_REQUEST (requestor_cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_ship_to_c for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_SHIP_TO_C  ON GRINGLOBAL.ORDER_REQUEST (ship_to_cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_or_source_c for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_OR_SOURCE_C  ON GRINGLOBAL.ORDER_REQUEST (source_cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_or_local for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_OR_LOCAL  ON GRINGLOBAL.ORDER_REQUEST (site_code, local_number)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_or_obtained for table order_request ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_OR_OBTAINED  ON GRINGLOBAL.ORDER_REQUEST (order_obtained_via)
/


/************ 4 Index Definitions for order_request_action *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ora_created for table order_request_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORA_CREATED  ON GRINGLOBAL.ORDER_REQUEST_ACTION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ora_modified for table order_request_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORA_MODIFIED  ON GRINGLOBAL.ORDER_REQUEST_ACTION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ora_or for table order_request_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORA_OR  ON GRINGLOBAL.ORDER_REQUEST_ACTION (order_request_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ora_owned for table order_request_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORA_OWNED  ON GRINGLOBAL.ORDER_REQUEST_ACTION (owned_by)
/


/************ No index definitions exist for order_request_image *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table order_request_image') as Action from dual;
/************ 4 Index Definitions for plant_introduction *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_pi_created for table plant_introduction ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_PI_CREATED  ON GRINGLOBAL.PLANT_INTRODUCTION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_pi_modified for table plant_introduction ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_PI_MODIFIED  ON GRINGLOBAL.PLANT_INTRODUCTION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_pi_owned for table plant_introduction ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_PI_OWNED  ON GRINGLOBAL.PLANT_INTRODUCTION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_pi_year for table plant_introduction ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_PI_YEAR  ON GRINGLOBAL.PLANT_INTRODUCTION (plant_introduction_year_date)
/


/************ 4 Index Definitions for region *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_r_created for table region ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_R_CREATED  ON GRINGLOBAL.REGION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_r_modified for table region ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_R_MODIFIED  ON GRINGLOBAL.REGION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_r_owned for table region ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_R_OWNED  ON GRINGLOBAL.REGION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_re for table region ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_RE  ON GRINGLOBAL.REGION (continent, subcontinent)
/


/************ 4 Index Definitions for sec_dataview *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sr_created for table sec_dataview ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SR_CREATED  ON GRINGLOBAL.SEC_DATAVIEW (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sr_modified for table sec_dataview ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SR_MODIFIED  ON GRINGLOBAL.SEC_DATAVIEW (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sr_owned for table sec_dataview ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SR_OWNED  ON GRINGLOBAL.SEC_DATAVIEW (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_dataview for table sec_dataview ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_DATAVIEW  ON GRINGLOBAL.SEC_DATAVIEW (dataview_name)
/


/************ 1 Index Definitions for sec_dataview_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdl for table sec_dataview_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDL  ON GRINGLOBAL.SEC_DATAVIEW_LANG (sec_dataview_id, sec_lang_id)
/


/************ 5 Index Definitions for sec_dataview_param *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srp_created for table sec_dataview_param ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRP_CREATED  ON GRINGLOBAL.SEC_DATAVIEW_PARAM (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srp_modified for table sec_dataview_param ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRP_MODIFIED  ON GRINGLOBAL.SEC_DATAVIEW_PARAM (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srp_owned for table sec_dataview_param ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRP_OWNED  ON GRINGLOBAL.SEC_DATAVIEW_PARAM (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srp_sr for table sec_dataview_param ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRP_SR  ON GRINGLOBAL.SEC_DATAVIEW_PARAM (sec_dataview_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdp for table sec_dataview_param ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDP  ON GRINGLOBAL.SEC_DATAVIEW_PARAM (sec_dataview_id, param_name)
/


/************ 1 Index Definitions for sec_dataview_sql *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sds for table sec_dataview_sql ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDS  ON GRINGLOBAL.SEC_DATAVIEW_SQL (sec_dataview_id, db_engine_code)
/


/************ 1 Index Definitions for sec_file_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sfl for table sec_file_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SFL  ON GRINGLOBAL.SEC_FILE_LANG (sec_file_id, sec_lang_id)
/


/************ 1 Index Definitions for sec_group_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sgl for table sec_group_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SGL  ON GRINGLOBAL.SEC_GROUP_LANG (sec_group_id, sec_lang_id)
/


/************ 4 Index Definitions for sec_table *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_st_created for table sec_table ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ST_CREATED  ON GRINGLOBAL.SEC_TABLE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_st_modified for table sec_table ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ST_MODIFIED  ON GRINGLOBAL.SEC_TABLE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_st_owned for table sec_table ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ST_OWNED  ON GRINGLOBAL.SEC_TABLE (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_st for table sec_table ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_ST  ON GRINGLOBAL.SEC_TABLE (table_name)
/


/************ 6 Index Definitions for sec_table_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_stf_cdgrp for table sec_table_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_STF_CDGRP  ON GRINGLOBAL.SEC_TABLE_FIELD (group_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_stf_created for table sec_table_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_STF_CREATED  ON GRINGLOBAL.SEC_TABLE_FIELD (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_stf_modified for table sec_table_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_STF_MODIFIED  ON GRINGLOBAL.SEC_TABLE_FIELD (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_stf_owned for table sec_table_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_STF_OWNED  ON GRINGLOBAL.SEC_TABLE_FIELD (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_stf_st for table sec_table_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_STF_ST  ON GRINGLOBAL.SEC_TABLE_FIELD (sec_table_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_stf for table sec_table_field ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_STF  ON GRINGLOBAL.SEC_TABLE_FIELD (sec_table_id, field_name)
/


/************ No index definitions exist for sec_table_field_lang *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table sec_table_field_lang') as Action from dual;
/************ 1 Index Definitions for sec_table_relationship *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_str for table sec_table_relationship ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_STR  ON GRINGLOBAL.SEC_TABLE_RELATIONSHIP (sec_table_field_id, relationship_type_code, other_table_field_id)
/


/************ 5 Index Definitions for sec_user *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_su_co for table sec_user ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SU_CO  ON GRINGLOBAL.SEC_USER (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_su_created for table sec_user ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SU_CREATED  ON GRINGLOBAL.SEC_USER (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_su_modified for table sec_user ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SU_MODIFIED  ON GRINGLOBAL.SEC_USER (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_su_owned for table sec_user ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SU_OWNED  ON GRINGLOBAL.SEC_USER (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_su_name for table sec_user ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SU_NAME  ON GRINGLOBAL.SEC_USER (user_name)
/


/************ 1 Index Definitions for sec_user_cart *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_suc for table sec_user_cart ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SUC  ON GRINGLOBAL.SEC_USER_CART (sec_user_id)
/


/************ 1 Index Definitions for sec_user_cart_item *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_suci for table sec_user_cart_item ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SUCI  ON GRINGLOBAL.SEC_USER_CART_ITEM (sec_user_cart_id, item_name)
/


/************ No index definitions exist for site *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table site') as Action from dual;
/************ 4 Index Definitions for taxonomy_author *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ta_created for table taxonomy_author ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TA_CREATED  ON GRINGLOBAL.TAXONOMY_AUTHOR (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ta_modified for table taxonomy_author ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TA_MODIFIED  ON GRINGLOBAL.TAXONOMY_AUTHOR (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ta_owned for table taxonomy_author ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TA_OWNED  ON GRINGLOBAL.TAXONOMY_AUTHOR (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ta_name for table taxonomy_author ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_TA_NAME  ON GRINGLOBAL.TAXONOMY_AUTHOR (short_name_expanded_diacritic)
/


/************ 5 Index Definitions for taxonomy_family *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tf_created for table taxonomy_family ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TF_CREATED  ON GRINGLOBAL.TAXONOMY_FAMILY (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tf_cur_tf for table taxonomy_family ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TF_CUR_TF  ON GRINGLOBAL.TAXONOMY_FAMILY (current_taxonomy_family_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tf_modified for table taxonomy_family ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TF_MODIFIED  ON GRINGLOBAL.TAXONOMY_FAMILY (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tf_owned for table taxonomy_family ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TF_OWNED  ON GRINGLOBAL.TAXONOMY_FAMILY (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_fa for table taxonomy_family ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_FA  ON GRINGLOBAL.TAXONOMY_FAMILY (family_name, author_name, subfamily, tribe, subtribe)
/


/************ 7 Index Definitions for taxonomy_genus *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index fk_tg_cur_tgt for table taxonomy_genus ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.FK_TG_CUR_TGT  ON GRINGLOBAL.TAXONOMY_GENUS (current_taxonomy_genus_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tg_created for table taxonomy_genus ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TG_CREATED  ON GRINGLOBAL.TAXONOMY_GENUS (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tg_modified for table taxonomy_genus ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TG_MODIFIED  ON GRINGLOBAL.TAXONOMY_GENUS (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tg_owned for table taxonomy_genus ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TG_OWNED  ON GRINGLOBAL.TAXONOMY_GENUS (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tg_tf for table taxonomy_genus ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TG_TF  ON GRINGLOBAL.TAXONOMY_GENUS (taxonomy_family_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_tg_name for table taxonomy_genus ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_TG_NAME  ON GRINGLOBAL.TAXONOMY_GENUS (common_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_tg for table taxonomy_genus ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_TG  ON GRINGLOBAL.TAXONOMY_GENUS (genus_name, genus_authority, subgenus_name, section_name, series_name, subseries_name, subsection_name)
/


/************ 4 Index Definitions for taxonomy_genus_type *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gt_tf for table taxonomy_genus_type ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GT_TF  ON GRINGLOBAL.TAXONOMY_GENUS_TYPE (taxonomy_family_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgt_created for table taxonomy_genus_type ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGT_CREATED  ON GRINGLOBAL.TAXONOMY_GENUS_TYPE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgt_modified for table taxonomy_genus_type ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGT_MODIFIED  ON GRINGLOBAL.TAXONOMY_GENUS_TYPE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgt_owned for table taxonomy_genus_type ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGT_OWNED  ON GRINGLOBAL.TAXONOMY_GENUS_TYPE (owned_by)
/


/************ 3 Index Definitions for url *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_u_created for table url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_U_CREATED  ON GRINGLOBAL.URL (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_u_modified for table url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_U_MODIFIED  ON GRINGLOBAL.URL (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_u_owned for table url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_U_OWNED  ON GRINGLOBAL.URL (owned_by)
/


/************ 3 Index Definitions for accession_group *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ag_created for table accession_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AG_CREATED  ON GRINGLOBAL.ACCESSION_GROUP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ag_modified for table accession_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AG_MODIFIED  ON GRINGLOBAL.ACCESSION_GROUP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ag_owned for table accession_group ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AG_OWNED  ON GRINGLOBAL.ACCESSION_GROUP (owned_by)
/


/************ 4 Index Definitions for app_resource *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_are_created for table app_resource ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ARE_CREATED  ON GRINGLOBAL.APP_RESOURCE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_are_modified for table app_resource ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ARE_MODIFIED  ON GRINGLOBAL.APP_RESOURCE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_are_owned for table app_resource ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ARE_OWNED  ON GRINGLOBAL.APP_RESOURCE (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_are_sl for table app_resource ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ARE_SL  ON GRINGLOBAL.APP_RESOURCE (sec_lang_id)
/


/************ 5 Index Definitions for app_user_gui_setting *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sugs_co for table app_user_gui_setting ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUGS_CO  ON GRINGLOBAL.APP_USER_GUI_SETTING (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sugs_created for table app_user_gui_setting ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUGS_CREATED  ON GRINGLOBAL.APP_USER_GUI_SETTING (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sugs_modified for table app_user_gui_setting ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUGS_MODIFIED  ON GRINGLOBAL.APP_USER_GUI_SETTING (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sugs_owned for table app_user_gui_setting ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUGS_OWNED  ON GRINGLOBAL.APP_USER_GUI_SETTING (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sugs for table app_user_gui_setting ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SUGS  ON GRINGLOBAL.APP_USER_GUI_SETTING (cooperator_id, app_name, form_name, resource_name, resource_key)
/


/************ 4 Index Definitions for citation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ci_created for table citation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CI_CREATED  ON GRINGLOBAL.CITATION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ci_l for table citation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CI_L  ON GRINGLOBAL.CITATION (literature_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ci_modified for table citation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CI_MODIFIED  ON GRINGLOBAL.CITATION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ci_owned for table citation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CI_OWNED  ON GRINGLOBAL.CITATION (owned_by)
/


/************ 4 Index Definitions for code_value *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdval_cdgrp for table code_value ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDVAL_CDGRP  ON GRINGLOBAL.CODE_VALUE (group_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdval_created for table code_value ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDVAL_CREATED  ON GRINGLOBAL.CODE_VALUE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdval_modified for table code_value ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDVAL_MODIFIED  ON GRINGLOBAL.CODE_VALUE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdval_owned for table code_value ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDVAL_OWNED  ON GRINGLOBAL.CODE_VALUE (owned_by)
/


/************ 6 Index Definitions for code_value_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cvl_created for table code_value_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CVL_CREATED  ON GRINGLOBAL.CODE_VALUE_LANG (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cvl_cv for table code_value_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CVL_CV  ON GRINGLOBAL.CODE_VALUE_LANG (code_value_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cvl_modified for table code_value_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CVL_MODIFIED  ON GRINGLOBAL.CODE_VALUE_LANG (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cvl_owned for table code_value_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CVL_OWNED  ON GRINGLOBAL.CODE_VALUE_LANG (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cvl_sl for table code_value_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CVL_SL  ON GRINGLOBAL.CODE_VALUE_LANG (sec_lang_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_cvl for table code_value_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_CVL  ON GRINGLOBAL.CODE_VALUE_LANG (code_value_id, sec_lang_id)
/


/************ 5 Index Definitions for genetic_marker_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gmc_ci for table genetic_marker_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GMC_CI  ON GRINGLOBAL.GENETIC_MARKER_CITATION_MAP (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gmc_created for table genetic_marker_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GMC_CREATED  ON GRINGLOBAL.GENETIC_MARKER_CITATION_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gmc_gm for table genetic_marker_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GMC_GM  ON GRINGLOBAL.GENETIC_MARKER_CITATION_MAP (genetic_marker_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gmc_modified for table genetic_marker_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GMC_MODIFIED  ON GRINGLOBAL.GENETIC_MARKER_CITATION_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_gmc_owned for table genetic_marker_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GMC_OWNED  ON GRINGLOBAL.GENETIC_MARKER_CITATION_MAP (owned_by)
/


/************ No index definitions exist for geography *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table geography') as Action from dual;
/************ No index definitions exist for georeference *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table georeference') as Action from dual;
/************ 5 Index Definitions for method *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_m_created for table method ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_M_CREATED  ON GRINGLOBAL.METHOD (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_m_g for table method ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_M_G  ON GRINGLOBAL.METHOD (geography_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_m_modified for table method ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_M_MODIFIED  ON GRINGLOBAL.METHOD (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_m_owned for table method ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_M_OWNED  ON GRINGLOBAL.METHOD (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_m for table method ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_M  ON GRINGLOBAL.METHOD (name)
/


/************ 5 Index Definitions for method_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mcm_ci for table method_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MCM_CI  ON GRINGLOBAL.METHOD_CITATION_MAP (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mcm_created for table method_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MCM_CREATED  ON GRINGLOBAL.METHOD_CITATION_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mcm_m for table method_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MCM_M  ON GRINGLOBAL.METHOD_CITATION_MAP (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mcm_modified for table method_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MCM_MODIFIED  ON GRINGLOBAL.METHOD_CITATION_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mcm_owned for table method_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MCM_OWNED  ON GRINGLOBAL.METHOD_CITATION_MAP (owned_by)
/


/************ 5 Index Definitions for method_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mm_c for table method_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MM_C  ON GRINGLOBAL.METHOD_MAP (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mm_created for table method_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MM_CREATED  ON GRINGLOBAL.METHOD_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mm_m for table method_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MM_M  ON GRINGLOBAL.METHOD_MAP (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mm_modified for table method_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MM_MODIFIED  ON GRINGLOBAL.METHOD_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_mm_owned for table method_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_MM_OWNED  ON GRINGLOBAL.METHOD_MAP (owned_by)
/


/************ 1 Index Definitions for sec_datatrigger *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sd for table sec_datatrigger ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SD  ON GRINGLOBAL.SEC_DATATRIGGER (sec_dataview_id, sec_table_id, fully_qualified_class_name)
/


/************ 6 Index Definitions for sec_dataview_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srf_created for table sec_dataview_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRF_CREATED  ON GRINGLOBAL.SEC_DATAVIEW_FIELD (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srf_modified for table sec_dataview_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRF_MODIFIED  ON GRINGLOBAL.SEC_DATAVIEW_FIELD (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srf_owned for table sec_dataview_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRF_OWNED  ON GRINGLOBAL.SEC_DATAVIEW_FIELD (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srf_sr for table sec_dataview_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRF_SR  ON GRINGLOBAL.SEC_DATAVIEW_FIELD (sec_dataview_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srf_stf for table sec_dataview_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRF_STF  ON GRINGLOBAL.SEC_DATAVIEW_FIELD (sec_table_field_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdf for table sec_dataview_field ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDF  ON GRINGLOBAL.SEC_DATAVIEW_FIELD (sec_dataview_id, sec_table_field_id, field_name)
/


/************ 6 Index Definitions for sec_dataview_field_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srfl_created for table sec_dataview_field_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRFL_CREATED  ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srfl_modified for table sec_dataview_field_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRFL_MODIFIED  ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srfl_owned for table sec_dataview_field_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRFL_OWNED  ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srfl_sl for table sec_dataview_field_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRFL_SL  ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG (sec_lang_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_srfl_srf for table sec_dataview_field_lang ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SRFL_SRF  ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG (sec_dataview_field_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sdfl for table sec_dataview_field_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SDFL  ON GRINGLOBAL.SEC_DATAVIEW_FIELD_LANG (sec_dataview_field_id, sec_lang_id)
/


/************ 1 Index Definitions for sec_group_user_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sgum for table sec_group_user_map ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SGUM  ON GRINGLOBAL.SEC_GROUP_USER_MAP (sec_group_id, sec_user_id)
/


/************ 5 Index Definitions for sec_index *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_si_created for table sec_index ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SI_CREATED  ON GRINGLOBAL.SEC_INDEX (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_si_modified for table sec_index ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SI_MODIFIED  ON GRINGLOBAL.SEC_INDEX (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_si_owned for table sec_index ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SI_OWNED  ON GRINGLOBAL.SEC_INDEX (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_si_st for table sec_index ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SI_ST  ON GRINGLOBAL.SEC_INDEX (sec_table_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_si for table sec_index ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SI  ON GRINGLOBAL.SEC_INDEX (index_name)
/


/************ 6 Index Definitions for sec_index_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sif_created for table sec_index_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SIF_CREATED  ON GRINGLOBAL.SEC_INDEX_FIELD (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sif_modified for table sec_index_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SIF_MODIFIED  ON GRINGLOBAL.SEC_INDEX_FIELD (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sif_owned for table sec_index_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SIF_OWNED  ON GRINGLOBAL.SEC_INDEX_FIELD (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sif_si for table sec_index_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SIF_SI  ON GRINGLOBAL.SEC_INDEX_FIELD (sec_index_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sif_stf for table sec_index_field ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SIF_STF  ON GRINGLOBAL.SEC_INDEX_FIELD (sec_table_field_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sif for table sec_index_field ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SIF  ON GRINGLOBAL.SEC_INDEX_FIELD (sec_index_id, sec_index_field_id)
/


/************ 3 Index Definitions for sec_perm *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sp_created for table sec_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SP_CREATED  ON GRINGLOBAL.SEC_PERM (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sp_modified for table sec_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SP_MODIFIED  ON GRINGLOBAL.SEC_PERM (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sp_owned for table sec_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SP_OWNED  ON GRINGLOBAL.SEC_PERM (owned_by)
/


/************ 1 Index Definitions for sec_perm_field *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_spf for table sec_perm_field ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SPF  ON GRINGLOBAL.SEC_PERM_FIELD (sec_perm_id, sec_dataview_field_id, sec_table_field_id, compare_mode, compare_operator, parent_compare_operator)
/


/************ 1 Index Definitions for sec_perm_lang *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_spl for table sec_perm_lang ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SPL  ON GRINGLOBAL.SEC_PERM_LANG (sec_perm_id, sec_lang_id)
/


/************ 6 Index Definitions for sec_user_perm *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sup_created for table sec_user_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUP_CREATED  ON GRINGLOBAL.SEC_USER_PERM (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sup_modified for table sec_user_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUP_MODIFIED  ON GRINGLOBAL.SEC_USER_PERM (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sup_owned for table sec_user_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUP_OWNED  ON GRINGLOBAL.SEC_USER_PERM (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sup_sp for table sec_user_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUP_SP  ON GRINGLOBAL.SEC_USER_PERM (sec_perm_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_sup_su for table sec_user_perm ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_SUP_SU  ON GRINGLOBAL.SEC_USER_PERM (sec_user_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sup for table sec_user_perm ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SUP  ON GRINGLOBAL.SEC_USER_PERM (sec_perm_id, sec_user_id)
/


/************ 10 Index Definitions for taxonomy *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_c for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_C  ON GRINGLOBAL.TAXONOMY (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_created for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_CREATED  ON GRINGLOBAL.TAXONOMY (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_cur_t for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_CUR_T  ON GRINGLOBAL.TAXONOMY (current_taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_modified for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_MODIFIED  ON GRINGLOBAL.TAXONOMY (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_owned for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_OWNED  ON GRINGLOBAL.TAXONOMY (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_tcr for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_TCR  ON GRINGLOBAL.TAXONOMY (crop_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_t_tg for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_T_TG  ON GRINGLOBAL.TAXONOMY (taxonomy_genus_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ta_site1 for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_TA_SITE1  ON GRINGLOBAL.TAXONOMY (priority_site_1)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ta_site2 for table taxonomy ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_TA_SITE2  ON GRINGLOBAL.TAXONOMY (priority_site_2)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ta for table taxonomy ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_TA  ON GRINGLOBAL.TAXONOMY (name, name_authority)
/


/************ 5 Index Definitions for taxonomy_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcm_ci for table taxonomy_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCM_CI  ON GRINGLOBAL.TAXONOMY_CITATION_MAP (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcm_created for table taxonomy_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCM_CREATED  ON GRINGLOBAL.TAXONOMY_CITATION_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcm_modified for table taxonomy_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCM_MODIFIED  ON GRINGLOBAL.TAXONOMY_CITATION_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcm_owned for table taxonomy_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCM_OWNED  ON GRINGLOBAL.TAXONOMY_CITATION_MAP (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcm_t for table taxonomy_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCM_T  ON GRINGLOBAL.TAXONOMY_CITATION_MAP (taxonomy_id)
/


/************ 6 Index Definitions for taxonomy_common_name *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_cn_name for table taxonomy_common_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_CN_NAME  ON GRINGLOBAL.TAXONOMY_COMMON_NAME (name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_cn_simplified_name for table taxonomy_common_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_CN_SIMPLIFIED_NAME  ON GRINGLOBAL.TAXONOMY_COMMON_NAME (simplified_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcn_created for table taxonomy_common_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCN_CREATED  ON GRINGLOBAL.TAXONOMY_COMMON_NAME (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcn_modified for table taxonomy_common_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCN_MODIFIED  ON GRINGLOBAL.TAXONOMY_COMMON_NAME (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcn_owned for table taxonomy_common_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCN_OWNED  ON GRINGLOBAL.TAXONOMY_COMMON_NAME (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tcn_t for table taxonomy_common_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TCN_T  ON GRINGLOBAL.TAXONOMY_COMMON_NAME (taxonomy_id)
/


/************ 5 Index Definitions for taxonomy_distribution *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_td_created for table taxonomy_distribution ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TD_CREATED  ON GRINGLOBAL.TAXONOMY_DISTRIBUTION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_td_g for table taxonomy_distribution ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TD_G  ON GRINGLOBAL.TAXONOMY_DISTRIBUTION (geography_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_td_modified for table taxonomy_distribution ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TD_MODIFIED  ON GRINGLOBAL.TAXONOMY_DISTRIBUTION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_td_owned for table taxonomy_distribution ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TD_OWNED  ON GRINGLOBAL.TAXONOMY_DISTRIBUTION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_td_t for table taxonomy_distribution ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TD_T  ON GRINGLOBAL.TAXONOMY_DISTRIBUTION (taxonomy_id)
/


/************ No index definitions exist for taxonomy_family_cit_map *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table taxonomy_family_cit_map') as Action from dual;
/************ 5 Index Definitions for taxonomy_genus_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgc_ci for table taxonomy_genus_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGC_CI  ON GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgc_created for table taxonomy_genus_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGC_CREATED  ON GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgc_modified for table taxonomy_genus_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGC_MODIFIED  ON GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgc_owned for table taxonomy_genus_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGC_OWNED  ON GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgc_tg for table taxonomy_genus_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGC_TG  ON GRINGLOBAL.TAXONOMY_GENUS_CITATION_MAP (taxonomy_genus_id)
/


/************ 4 Index Definitions for taxonomy_germination_rule *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgr_created for table taxonomy_germination_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGR_CREATED  ON GRINGLOBAL.TAXONOMY_GERMINATION_RULE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgr_modified for table taxonomy_germination_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGR_MODIFIED  ON GRINGLOBAL.TAXONOMY_GERMINATION_RULE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgr_owned for table taxonomy_germination_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGR_OWNED  ON GRINGLOBAL.TAXONOMY_GERMINATION_RULE (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tgr_t for table taxonomy_germination_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TGR_T  ON GRINGLOBAL.TAXONOMY_GERMINATION_RULE (taxonomy_id)
/


/************ 7 Index Definitions for taxonomy_url *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_created for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_CREATED  ON GRINGLOBAL.TAXONOMY_URL (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_modified for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_MODIFIED  ON GRINGLOBAL.TAXONOMY_URL (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_owned for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_OWNED  ON GRINGLOBAL.TAXONOMY_URL (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_t for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_T  ON GRINGLOBAL.TAXONOMY_URL (taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_tf for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_TF  ON GRINGLOBAL.TAXONOMY_URL (taxonomy_family_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_tg for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_TG  ON GRINGLOBAL.TAXONOMY_URL (taxonomy_genus_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tu_u for table taxonomy_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TU_U  ON GRINGLOBAL.TAXONOMY_URL (url_id)
/


/************ 5 Index Definitions for taxonomy_use *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tus_created for table taxonomy_use ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TUS_CREATED  ON GRINGLOBAL.TAXONOMY_USE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tus_modified for table taxonomy_use ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TUS_MODIFIED  ON GRINGLOBAL.TAXONOMY_USE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tus_owned for table taxonomy_use ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TUS_OWNED  ON GRINGLOBAL.TAXONOMY_USE (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_tus_t for table taxonomy_use ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_TUS_T  ON GRINGLOBAL.TAXONOMY_USE (taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_tu_usage for table taxonomy_use ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_TU_USAGE  ON GRINGLOBAL.TAXONOMY_USE (economic_usage)
/


/************ 6 Index Definitions for accession *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_a_created for table accession ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_A_CREATED  ON GRINGLOBAL.ACCESSION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_a_modified for table accession ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_A_MODIFIED  ON GRINGLOBAL.ACCESSION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_a_owned for table accession ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_A_OWNED  ON GRINGLOBAL.ACCESSION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_a_pi for table accession ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_A_PI  ON GRINGLOBAL.ACCESSION (plant_introduction_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_a_t for table accession ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_A_T  ON GRINGLOBAL.ACCESSION (taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ac for table accession ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_AC  ON GRINGLOBAL.ACCESSION (accession_prefix, accession_number, accession_suffix)
/


/************ 6 Index Definitions for accession_action *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aa_a for table accession_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AA_A  ON GRINGLOBAL.ACCESSION_ACTION (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aa_c for table accession_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AA_C  ON GRINGLOBAL.ACCESSION_ACTION (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aa_created for table accession_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AA_CREATED  ON GRINGLOBAL.ACCESSION_ACTION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aa_m for table accession_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AA_M  ON GRINGLOBAL.ACCESSION_ACTION (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aa_modified for table accession_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AA_MODIFIED  ON GRINGLOBAL.ACCESSION_ACTION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aa_owned for table accession_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AA_OWNED  ON GRINGLOBAL.ACCESSION_ACTION (owned_by)
/


/************ 5 Index Definitions for accession_citation_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_acm_a for table accession_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ACM_A  ON GRINGLOBAL.ACCESSION_CITATION_MAP (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_acm_ci for table accession_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ACM_CI  ON GRINGLOBAL.ACCESSION_CITATION_MAP (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_acm_created for table accession_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ACM_CREATED  ON GRINGLOBAL.ACCESSION_CITATION_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_acm_modified for table accession_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ACM_MODIFIED  ON GRINGLOBAL.ACCESSION_CITATION_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_acm_owned for table accession_citation_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ACM_OWNED  ON GRINGLOBAL.ACCESSION_CITATION_MAP (owned_by)
/


/************ 4 Index Definitions for accession_habitat *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ah_a for table accession_habitat ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AH_A  ON GRINGLOBAL.ACCESSION_HABITAT (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ah_created for table accession_habitat ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AH_CREATED  ON GRINGLOBAL.ACCESSION_HABITAT (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ah_modified for table accession_habitat ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AH_MODIFIED  ON GRINGLOBAL.ACCESSION_HABITAT (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ah_owned for table accession_habitat ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AH_OWNED  ON GRINGLOBAL.ACCESSION_HABITAT (owned_by)
/


/************ 10 Index Definitions for accession_ipr *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ar_a for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AR_A  ON GRINGLOBAL.ACCESSION_IPR (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ar_ac for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AR_AC  ON GRINGLOBAL.ACCESSION_IPR (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ar_c for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AR_C  ON GRINGLOBAL.ACCESSION_IPR (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ar_created for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AR_CREATED  ON GRINGLOBAL.ACCESSION_IPR (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ar_modified for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AR_MODIFIED  ON GRINGLOBAL.ACCESSION_IPR (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ar_owned for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AR_OWNED  ON GRINGLOBAL.ACCESSION_IPR (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ipr_crop for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_IPR_CROP  ON GRINGLOBAL.ACCESSION_IPR (crop_name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ipr_number for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_IPR_NUMBER  ON GRINGLOBAL.ACCESSION_IPR (accession_ipr_number)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ipr_prefix for table accession_ipr ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_IPR_PREFIX  ON GRINGLOBAL.ACCESSION_IPR (accession_ipr_prefix)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ipr for table accession_ipr ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_IPR  ON GRINGLOBAL.ACCESSION_IPR (accession_id, assigned_type, accession_ipr_prefix)
/


/************ 4 Index Definitions for accession_narrative *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ana_a for table accession_narrative ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ANA_A  ON GRINGLOBAL.ACCESSION_NARRATIVE (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ana_created for table accession_narrative ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ANA_CREATED  ON GRINGLOBAL.ACCESSION_NARRATIVE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ana_modified for table accession_narrative ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ANA_MODIFIED  ON GRINGLOBAL.ACCESSION_NARRATIVE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ana_owned for table accession_narrative ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ANA_OWNED  ON GRINGLOBAL.ACCESSION_NARRATIVE (owned_by)
/


/************ 5 Index Definitions for accession_pedigree *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ap_a for table accession_pedigree ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AP_A  ON GRINGLOBAL.ACCESSION_PEDIGREE (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ap_ac for table accession_pedigree ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AP_AC  ON GRINGLOBAL.ACCESSION_PEDIGREE (citation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ap_created for table accession_pedigree ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AP_CREATED  ON GRINGLOBAL.ACCESSION_PEDIGREE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ap_modified for table accession_pedigree ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AP_MODIFIED  ON GRINGLOBAL.ACCESSION_PEDIGREE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ap_owned for table accession_pedigree ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AP_OWNED  ON GRINGLOBAL.ACCESSION_PEDIGREE (owned_by)
/


/************ 5 Index Definitions for accession_quarantine *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aq_a for table accession_quarantine ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AQ_A  ON GRINGLOBAL.ACCESSION_QUARANTINE (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aq_c for table accession_quarantine ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AQ_C  ON GRINGLOBAL.ACCESSION_QUARANTINE (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aq_created for table accession_quarantine ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AQ_CREATED  ON GRINGLOBAL.ACCESSION_QUARANTINE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aq_modified for table accession_quarantine ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AQ_MODIFIED  ON GRINGLOBAL.ACCESSION_QUARANTINE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aq_owned for table accession_quarantine ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AQ_OWNED  ON GRINGLOBAL.ACCESSION_QUARANTINE (owned_by)
/


/************ 1 Index Definitions for accession_source *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_src for table accession_source ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SRC  ON GRINGLOBAL.ACCESSION_SOURCE (accession_id, type_code, step_date_qualifier, step_date)
/


/************ 5 Index Definitions for accession_source_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_asm_as for table accession_source_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ASM_AS  ON GRINGLOBAL.ACCESSION_SOURCE_MAP (accession_source_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_asm_c for table accession_source_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ASM_C  ON GRINGLOBAL.ACCESSION_SOURCE_MAP (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_asm_created for table accession_source_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ASM_CREATED  ON GRINGLOBAL.ACCESSION_SOURCE_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_asm_modified for table accession_source_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ASM_MODIFIED  ON GRINGLOBAL.ACCESSION_SOURCE_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_asm_owned for table accession_source_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ASM_OWNED  ON GRINGLOBAL.ACCESSION_SOURCE_MAP (owned_by)
/


/************ 4 Index Definitions for code_rule *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdrule_created for table code_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDRULE_CREATED  ON GRINGLOBAL.CODE_RULE (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdrule_cv for table code_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDRULE_CV  ON GRINGLOBAL.CODE_RULE (code_value_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdrule_modified for table code_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDRULE_MODIFIED  ON GRINGLOBAL.CODE_RULE (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cdrule_owned for table code_rule ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CDRULE_OWNED  ON GRINGLOBAL.CODE_RULE (owned_by)
/


/************ 8 Index Definitions for crop_trait_url *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_cr for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_CR  ON GRINGLOBAL.CROP_TRAIT_URL (crop_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_created for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_CREATED  ON GRINGLOBAL.CROP_TRAIT_URL (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_ct for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_CT  ON GRINGLOBAL.CROP_TRAIT_URL (crop_trait_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_m for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_M  ON GRINGLOBAL.CROP_TRAIT_URL (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_modified for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_MODIFIED  ON GRINGLOBAL.CROP_TRAIT_URL (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_owned for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_OWNED  ON GRINGLOBAL.CROP_TRAIT_URL (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ctu_u for table crop_trait_url ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTU_U  ON GRINGLOBAL.CROP_TRAIT_URL (url_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ctu for table crop_trait_url ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_CTU  ON GRINGLOBAL.CROP_TRAIT_URL (url_type, sequence_number, crop_id, crop_trait_id, code)
/


/************ 6 Index Definitions for genetic_annotation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ga_created for table genetic_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GA_CREATED  ON GRINGLOBAL.GENETIC_ANNOTATION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ga_gm for table genetic_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GA_GM  ON GRINGLOBAL.GENETIC_ANNOTATION (marker_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ga_m for table genetic_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GA_M  ON GRINGLOBAL.GENETIC_ANNOTATION (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ga_modified for table genetic_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GA_MODIFIED  ON GRINGLOBAL.GENETIC_ANNOTATION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ga_owned for table genetic_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GA_OWNED  ON GRINGLOBAL.GENETIC_ANNOTATION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ga for table genetic_annotation ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_GA  ON GRINGLOBAL.GENETIC_ANNOTATION (marker_id, method_id)
/


/************ 12 Index Definitions for inventory *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_a for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_A  ON GRINGLOBAL.INVENTORY (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_backup_i for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_BACKUP_I  ON GRINGLOBAL.INVENTORY (backup_inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_c for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_C  ON GRINGLOBAL.INVENTORY (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_created for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_CREATED  ON GRINGLOBAL.INVENTORY (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_im for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_IM  ON GRINGLOBAL.INVENTORY (inventory_maint_policy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_modified for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_MODIFIED  ON GRINGLOBAL.INVENTORY (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_owned for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_OWNED  ON GRINGLOBAL.INVENTORY (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_i_parent_i for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_I_PARENT_I  ON GRINGLOBAL.INVENTORY (parent_inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_inv_location for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_INV_LOCATION  ON GRINGLOBAL.INVENTORY (site_code, location_section_1, location_section_2, location_section_3, location_section_4)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_inv_number for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_INV_NUMBER  ON GRINGLOBAL.INVENTORY (inventory_number)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_inv_prefix for table inventory ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_INV_PREFIX  ON GRINGLOBAL.INVENTORY (inventory_prefix)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_inv for table inventory ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_INV  ON GRINGLOBAL.INVENTORY (inventory_prefix, inventory_number, inventory_suffix, inventory_type_code)
/


/************ 6 Index Definitions for inventory_action *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ia_c for table inventory_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IA_C  ON GRINGLOBAL.INVENTORY_ACTION (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ia_created for table inventory_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IA_CREATED  ON GRINGLOBAL.INVENTORY_ACTION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ia_i for table inventory_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IA_I  ON GRINGLOBAL.INVENTORY_ACTION (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ia_m for table inventory_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IA_M  ON GRINGLOBAL.INVENTORY_ACTION (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ia_modified for table inventory_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IA_MODIFIED  ON GRINGLOBAL.INVENTORY_ACTION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ia_owned for table inventory_action ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IA_OWNED  ON GRINGLOBAL.INVENTORY_ACTION (owned_by)
/


/************ 6 Index Definitions for inventory_group_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_igm_created for table inventory_group_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IGM_CREATED  ON GRINGLOBAL.INVENTORY_GROUP_MAP (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_igm_i for table inventory_group_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IGM_I  ON GRINGLOBAL.INVENTORY_GROUP_MAP (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_igm_ig for table inventory_group_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IGM_IG  ON GRINGLOBAL.INVENTORY_GROUP_MAP (inventory_group_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_igm_modified for table inventory_group_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IGM_MODIFIED  ON GRINGLOBAL.INVENTORY_GROUP_MAP (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_igm_owned for table inventory_group_map ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IGM_OWNED  ON GRINGLOBAL.INVENTORY_GROUP_MAP (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_igm for table inventory_group_map ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_IGM  ON GRINGLOBAL.INVENTORY_GROUP_MAP (inventory_id, inventory_group_id, site_code)
/


/************ No index definitions exist for inventory_image *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table inventory_image') as Action from dual;
/************ 6 Index Definitions for inventory_quality_status *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iqs_created for table inventory_quality_status ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IQS_CREATED  ON GRINGLOBAL.INVENTORY_QUALITY_STATUS (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iqs_i for table inventory_quality_status ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IQS_I  ON GRINGLOBAL.INVENTORY_QUALITY_STATUS (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iqs_modified for table inventory_quality_status ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IQS_MODIFIED  ON GRINGLOBAL.INVENTORY_QUALITY_STATUS (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iqs_owned for table inventory_quality_status ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IQS_OWNED  ON GRINGLOBAL.INVENTORY_QUALITY_STATUS (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_iqs_test for table inventory_quality_status ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_IQS_TEST  ON GRINGLOBAL.INVENTORY_QUALITY_STATUS (test_type, pathogen_code)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_iqs for table inventory_quality_status ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_IQS  ON GRINGLOBAL.INVENTORY_QUALITY_STATUS (inventory_id, test_type, pathogen_code, finished_date)
/


/************ 5 Index Definitions for inventory_viability *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iv_created for table inventory_viability ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IV_CREATED  ON GRINGLOBAL.INVENTORY_VIABILITY (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iv_i for table inventory_viability ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IV_I  ON GRINGLOBAL.INVENTORY_VIABILITY (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iv_m for table inventory_viability ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IV_M  ON GRINGLOBAL.INVENTORY_VIABILITY (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iv_modified for table inventory_viability ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IV_MODIFIED  ON GRINGLOBAL.INVENTORY_VIABILITY (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_iv_owned for table inventory_viability ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_IV_OWNED  ON GRINGLOBAL.INVENTORY_VIABILITY (owned_by)
/


/************ 8 Index Definitions for order_request_item *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ori_created for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORI_CREATED  ON GRINGLOBAL.ORDER_REQUEST_ITEM (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ori_i for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORI_I  ON GRINGLOBAL.ORDER_REQUEST_ITEM (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ori_modified for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORI_MODIFIED  ON GRINGLOBAL.ORDER_REQUEST_ITEM (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ori_or for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORI_OR  ON GRINGLOBAL.ORDER_REQUEST_ITEM (order_request_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ori_owned for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORI_OWNED  ON GRINGLOBAL.ORDER_REQUEST_ITEM (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_ori_t for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_ORI_T  ON GRINGLOBAL.ORDER_REQUEST_ITEM (taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_ori_item for table order_request_item ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_ORI_ITEM  ON GRINGLOBAL.ORDER_REQUEST_ITEM (order_request_id, name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_ori for table order_request_item ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_ORI  ON GRINGLOBAL.ORDER_REQUEST_ITEM (order_request_id, sequence_number)
/


/************ 1 Index Definitions for sec_group_perm_map *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_sgpm for table sec_group_perm_map ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_SGPM  ON GRINGLOBAL.SEC_GROUP_PERM_MAP (sec_group_id, sec_perm_id)
/


/************ No index definitions exist for site_inventory_nc7 *************/

select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table site_inventory_nc7') as Action from dual;
/************ 9 Index Definitions for accession_annotation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_c for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_C  ON GRINGLOBAL.ACCESSION_ANNOTATION (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_created for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_CREATED  ON GRINGLOBAL.ACCESSION_ANNOTATION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_i for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_I  ON GRINGLOBAL.ACCESSION_ANNOTATION (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_modified for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_MODIFIED  ON GRINGLOBAL.ACCESSION_ANNOTATION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_or for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_OR  ON GRINGLOBAL.ACCESSION_ANNOTATION (order_request_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_owned for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_OWNED  ON GRINGLOBAL.ACCESSION_ANNOTATION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_t_new for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_T_NEW  ON GRINGLOBAL.ACCESSION_ANNOTATION (new_taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_aan_t_old for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AAN_T_OLD  ON GRINGLOBAL.ACCESSION_ANNOTATION (old_taxonomy_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_al_site for table accession_annotation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AL_SITE  ON GRINGLOBAL.ACCESSION_ANNOTATION (site_code)
/


/************ 9 Index Definitions for accession_name *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_an_name for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_AN_NAME  ON GRINGLOBAL.ACCESSION_NAME (name)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_a for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_A  ON GRINGLOBAL.ACCESSION_NAME (accession_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_ag for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_AG  ON GRINGLOBAL.ACCESSION_NAME (accession_group_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_c for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_C  ON GRINGLOBAL.ACCESSION_NAME (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_created for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_CREATED  ON GRINGLOBAL.ACCESSION_NAME (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_i for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_I  ON GRINGLOBAL.ACCESSION_NAME (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_modified for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_MODIFIED  ON GRINGLOBAL.ACCESSION_NAME (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_an_owned for table accession_name ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AN_OWNED  ON GRINGLOBAL.ACCESSION_NAME (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_an for table accession_name ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_AN  ON GRINGLOBAL.ACCESSION_NAME (accession_id, name, accession_group_id)
/


/************ 5 Index Definitions for accession_voucher *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_av_c for table accession_voucher ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AV_C  ON GRINGLOBAL.ACCESSION_VOUCHER (cooperator_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_av_created for table accession_voucher ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AV_CREATED  ON GRINGLOBAL.ACCESSION_VOUCHER (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_av_i for table accession_voucher ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AV_I  ON GRINGLOBAL.ACCESSION_VOUCHER (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_av_modified for table accession_voucher ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AV_MODIFIED  ON GRINGLOBAL.ACCESSION_VOUCHER (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_av_owned for table accession_voucher ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_AV_OWNED  ON GRINGLOBAL.ACCESSION_VOUCHER (owned_by)
/


/************ 8 Index Definitions for crop_trait_observation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_created for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_CREATED  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_ct for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_CT  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (crop_trait_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_ctc for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_CTC  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (crop_trait_code_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_ctq for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_CTQ  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (crop_trait_qualifier_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_i for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_I  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_m for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_M  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (method_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_modified for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_MODIFIED  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_cto_owned for table crop_trait_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_CTO_OWNED  ON GRINGLOBAL.CROP_TRAIT_OBSERVATION (owned_by)
/


/************ 6 Index Definitions for genetic_observation *************/
select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_go_created for table genetic_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GO_CREATED  ON GRINGLOBAL.GENETIC_OBSERVATION (created_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_go_ga for table genetic_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GO_GA  ON GRINGLOBAL.GENETIC_OBSERVATION (genetic_annotation_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_go_i for table genetic_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GO_I  ON GRINGLOBAL.GENETIC_OBSERVATION (inventory_id)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_go_modified for table genetic_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GO_MODIFIED  ON GRINGLOBAL.GENETIC_OBSERVATION (modified_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_fk_go_owned for table genetic_observation ...') as Action from dual;
CREATE  INDEX GRINGLOBAL.NDX_FK_GO_OWNED  ON GRINGLOBAL.GENETIC_OBSERVATION (owned_by)
/


select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index ndx_uniq_go for table genetic_observation ...') as Action from dual;
CREATE UNIQUE INDEX GRINGLOBAL.NDX_UNIQ_GO  ON GRINGLOBAL.GENETIC_OBSERVATION (genetic_annotation_id, inventory_id, individual)
/


