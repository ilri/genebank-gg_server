 /***********************************************/
/*********** Migration Table Drops *************/
/***********************************************/

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __code_column ...') as Action from dual;
drop table GRINGLOBAL.__code_column;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __cooperator ...') as Action from dual;
drop table GRINGLOBAL.__cooperator;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __cooperator_group ...') as Action from dual;
drop table GRINGLOBAL.__cooperator_group;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __cooperator_map ...') as Action from dual;
drop table GRINGLOBAL.__cooperator_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop ...') as Action from dual;
drop table GRINGLOBAL.__crop;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait_code ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait_code;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait_qualifier ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait_qualifier;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __genomic_marker ...') as Action from dual;
drop table GRINGLOBAL.__genomic_marker;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __image ...') as Action from dual;
drop table GRINGLOBAL.__image;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_group ...') as Action from dual;
drop table GRINGLOBAL.__inventory_group;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_maint_policy ...') as Action from dual;
drop table GRINGLOBAL.__inventory_maint_policy;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __literature ...') as Action from dual;
drop table GRINGLOBAL.__literature;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __order_request ...') as Action from dual;
drop table GRINGLOBAL.__order_request;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __order_request_action ...') as Action from dual;
drop table GRINGLOBAL.__order_request_action;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __plant_introduction ...') as Action from dual;
drop table GRINGLOBAL.__plant_introduction;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __region ...') as Action from dual;
drop table GRINGLOBAL.__region;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_lang ...') as Action from dual;
drop table GRINGLOBAL.__sec_lang;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_perm_template ...') as Action from dual;
drop table GRINGLOBAL.__sec_perm_template;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_rs ...') as Action from dual;
drop table GRINGLOBAL.__sec_rs;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_rs_param ...') as Action from dual;
drop table GRINGLOBAL.__sec_rs_param;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_table ...') as Action from dual;
drop table GRINGLOBAL.__sec_table;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_user ...') as Action from dual;
drop table GRINGLOBAL.__sec_user;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_user_gui_setting ...') as Action from dual;
drop table GRINGLOBAL.__sec_user_gui_setting;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __site ...') as Action from dual;
drop table GRINGLOBAL.__site;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_author ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_author;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_family ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_family;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_genus ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_genus;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_genus_type ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_genus_type;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __url ...') as Action from dual;
drop table GRINGLOBAL.__url;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_group ...') as Action from dual;
drop table GRINGLOBAL.__accession_group;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __app_resource ...') as Action from dual;
drop table GRINGLOBAL.__app_resource;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __app_user_item_list ...') as Action from dual;
drop table GRINGLOBAL.__app_user_item_list;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __citation ...') as Action from dual;
drop table GRINGLOBAL.__citation;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __code_group ...') as Action from dual;
drop table GRINGLOBAL.__code_group;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __code_value ...') as Action from dual;
drop table GRINGLOBAL.__code_value;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __code_value_lang ...') as Action from dual;
drop table GRINGLOBAL.__code_value_lang;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait_code_lang ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait_code_lang;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __genomic_marker_citation_map ...') as Action from dual;
drop table GRINGLOBAL.__genomic_marker_citation_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __geography ...') as Action from dual;
drop table GRINGLOBAL.__geography;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __method ...') as Action from dual;
drop table GRINGLOBAL.__method;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __method_citation_map ...') as Action from dual;
drop table GRINGLOBAL.__method_citation_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __method_map ...') as Action from dual;
drop table GRINGLOBAL.__method_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_index ...') as Action from dual;
drop table GRINGLOBAL.__sec_index;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_perm ...') as Action from dual;
drop table GRINGLOBAL.__sec_perm;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_perm_field ...') as Action from dual;
drop table GRINGLOBAL.__sec_perm_field;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_perm_template_map ...') as Action from dual;
drop table GRINGLOBAL.__sec_perm_template_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_table_field ...') as Action from dual;
drop table GRINGLOBAL.__sec_table_field;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_table_filter ...') as Action from dual;
drop table GRINGLOBAL.__sec_table_filter;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_user_perm ...') as Action from dual;
drop table GRINGLOBAL.__sec_user_perm;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_citation_map ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_citation_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_common_name ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_common_name;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_distribution ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_distribution;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_genus_citation_map ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_genus_citation_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_germination_rule ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_germination_rule;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_url ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_url;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __taxonomy_use ...') as Action from dual;
drop table GRINGLOBAL.__taxonomy_use;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession ...') as Action from dual;
drop table GRINGLOBAL.__accession;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_action ...') as Action from dual;
drop table GRINGLOBAL.__accession_action;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_citation_map ...') as Action from dual;
drop table GRINGLOBAL.__accession_citation_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_habitat ...') as Action from dual;
drop table GRINGLOBAL.__accession_habitat;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_image_map ...') as Action from dual;
drop table GRINGLOBAL.__accession_image_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_ipr ...') as Action from dual;
drop table GRINGLOBAL.__accession_ipr;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_narrative ...') as Action from dual;
drop table GRINGLOBAL.__accession_narrative;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_pedigree ...') as Action from dual;
drop table GRINGLOBAL.__accession_pedigree;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_quarantine ...') as Action from dual;
drop table GRINGLOBAL.__accession_quarantine;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_source ...') as Action from dual;
drop table GRINGLOBAL.__accession_source;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_source_map ...') as Action from dual;
drop table GRINGLOBAL.__accession_source_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __code_rule ...') as Action from dual;
drop table GRINGLOBAL.__code_rule;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait_url ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait_url;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __genomic_annotation ...') as Action from dual;
drop table GRINGLOBAL.__genomic_annotation;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory ...') as Action from dual;
drop table GRINGLOBAL.__inventory;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_action ...') as Action from dual;
drop table GRINGLOBAL.__inventory_action;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_group_map ...') as Action from dual;
drop table GRINGLOBAL.__inventory_group_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_image_map ...') as Action from dual;
drop table GRINGLOBAL.__inventory_image_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_quality_status ...') as Action from dual;
drop table GRINGLOBAL.__inventory_quality_status;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __inventory_viability ...') as Action from dual;
drop table GRINGLOBAL.__inventory_viability;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __order_request_item ...') as Action from dual;
drop table GRINGLOBAL.__order_request_item;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_index_field ...') as Action from dual;
drop table GRINGLOBAL.__sec_index_field;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_rs_field ...') as Action from dual;
drop table GRINGLOBAL.__sec_rs_field;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_rs_field_lang ...') as Action from dual;
drop table GRINGLOBAL.__sec_rs_field_lang;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __sec_rs_filter ...') as Action from dual;
drop table GRINGLOBAL.__sec_rs_filter;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_annotation ...') as Action from dual;
drop table GRINGLOBAL.__accession_annotation;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_name ...') as Action from dual;
drop table GRINGLOBAL.__accession_name;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_voucher ...') as Action from dual;
drop table GRINGLOBAL.__accession_voucher;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __accession_voucher_image_map ...') as Action from dual;
drop table GRINGLOBAL.__accession_voucher_image_map;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait_observation ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait_observation;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __crop_trait_observation_raw ...') as Action from dual;
drop table GRINGLOBAL.__crop_trait_observation_raw;

select to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __genomic_observation ...') as Action from dual;
drop table GRINGLOBAL.__genomic_observation;

