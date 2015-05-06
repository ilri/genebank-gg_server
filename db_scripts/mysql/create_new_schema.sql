 /***********************************************/
/******** Turn Off Unique Checks       *********/
/***********************************************/
SET UNIQUE_CHECKS=0;
/***********************************************/
/*************** Table Definitions *************/
/***********************************************/

/************ Table Definition for dev5.accession *************/
select 'creating table dev5.accession ...' as Action;
CREATE TABLE `dev5`.`accession` (
`accession_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_prefix` varchar(4)    NOT NULL  ,
`accession_number` int(11)    NOT NULL  ,
`accession_suffix` varchar(4)    NULL  ,
`accession_name_id` int(11)    NULL  ,
`site_code` varchar(8)    NULL  ,
`inactive_site_code_reason` varchar(10)    NULL  ,
`is_core` char(1)    NOT NULL  ,
`is_at_alternate_site` char(1)    NOT NULL  ,
`life_form` varchar(10)    NULL  ,
`level_of_improvement_code` varchar(10)    NULL  ,
`reproductive_uniformity` varchar(10)    NULL  ,
`initial_material_type` varchar(2)    NULL  ,
`initial_recieved_date` datetime    NOT NULL  ,
`initial_received_date_format` varchar(10)    NULL  ,
`taxonomy_id` int(11)    NOT NULL  ,
`plant_introduction_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_action *************/
select 'creating table dev5.accession_action ...' as Action;
CREATE TABLE `dev5`.`accession_action` (
`accession_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`action_name` varchar(10)    NOT NULL  ,
`occurred_date` datetime    NULL  ,
`occurred_date_format` varchar(10)    NULL  ,
`completed_date` datetime    NULL  ,
`completed_date_format` varchar(10)    NULL  ,
`is_visible_from_web` char(1)    NOT NULL  ,
`narrative` varchar(2000)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`evaluation_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_action_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_annotation *************/
select 'creating table dev5.accession_annotation ...' as Action;
CREATE TABLE `dev5`.`accession_annotation` (
`accession_annotation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`action_name` varchar(10)    NOT NULL  ,
`action_date` datetime    NOT NULL  ,
`accession_id` int(11)    NOT NULL  ,
`site_code` varchar(8)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`inventory_id` int(11)    NULL  ,
`order_entry_id` int(11)    NULL  ,
`old_taxonomy_id` int(11)    NULL  ,
`new_taxonomy_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_annotation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_citation *************/
select 'creating table dev5.accession_citation ...' as Action;
CREATE TABLE `dev5`.`accession_citation` (
`accession_citation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`literature_id` int(11)    NULL  ,
`title` varchar(240)    NULL  ,
`author_name` varchar(240)    NULL  ,
`citation_year_date` datetime    NULL  ,
`reference` varchar(60)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_citation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_group *************/
select 'creating table dev5.accession_group ...' as Action;
CREATE TABLE `dev5`.`accession_group` (
`accession_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_group_code` varchar(20)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`site_code` varchar(8)    NULL  ,
`url` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_group_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_habitat *************/
select 'creating table dev5.accession_habitat ...' as Action;
CREATE TABLE `dev5`.`accession_habitat` (
`accession_habitat_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`latitude_degrees` int(11)    NULL  ,
`latitude_minutes` int(11)    NULL  ,
`latitude_seconds` int(11)    NULL  ,
`latitude_hemisphere` char(1)    NULL  ,
`longitude_degrees` int(11)    NULL  ,
`longitude_minutes` int(11)    NULL  ,
`longitude_seconds` int(11)    NULL  ,
`longitude_hemisphere` char(1)    NULL  ,
`elevation_in_meters` int(11)    NULL  ,
`quantity_collected` int(11)    NULL  ,
`unit_of_quantity_collected` varchar(2)    NULL  ,
`form_material_collected_code` varchar(2)    NULL  ,
`plant_sample_count` int(11)    NULL  ,
`locality` varchar(240)    NULL  ,
`habitat_name` varchar(240)    NULL  ,
`note` varchar(240)    NULL  ,
`collection_coordinate_system` varchar(10)    NULL  ,
`gstype` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_habitat_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_name *************/
select 'creating table dev5.accession_name ...' as Action;
CREATE TABLE `dev5`.`accession_name` (
`accession_name_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`category` varchar(10)    NOT NULL  ,
`name` varchar(40)    NOT NULL  ,
`accession_group_id` int(11)    NULL  ,
`inventory_id` int(11)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_name_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_narrative *************/
select 'creating table dev5.accession_narrative ...' as Action;
CREATE TABLE `dev5`.`accession_narrative` (
`accession_narrative_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`type_code` varchar(10)    NULL  ,
`narrative_body` varchar(4000)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_narrative_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_pedigree *************/
select 'creating table dev5.accession_pedigree ...' as Action;
CREATE TABLE `dev5`.`accession_pedigree` (
`accession_pedigree_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`released_date` datetime    NULL  ,
`released_date_format` varchar(10)    NULL  ,
`citation_id` int(11)    NULL  ,
`description` varchar(2000)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_pedigree_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_quarantine *************/
select 'creating table dev5.accession_quarantine ...' as Action;
CREATE TABLE `dev5`.`accession_quarantine` (
`accession_quarantine_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`quarantine_type` varchar(10)    NOT NULL  ,
`progress_status_code` varchar(10)    NULL  ,
`cooperator_id` int(11)    NOT NULL  ,
`entered_date` datetime    NULL  ,
`established_date` datetime    NULL  ,
`expected_release_date` datetime    NULL  ,
`released_date` datetime    NULL  ,
`note` varchar(240)    NULL  ,
`site_code` varchar(8)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_quarantine_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_right *************/
select 'creating table dev5.accession_right ...' as Action;
CREATE TABLE `dev5`.`accession_right` (
`accession_right_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`assigned_type` varchar(10)    NOT NULL  ,
`right_prefix` varchar(40)    NULL  ,
`right_number` int(11)    NULL  ,
`crop_name` varchar(60)    NULL  ,
`full_name` varchar(240)    NULL  ,
`issued_date` datetime    NULL  ,
`expired_date` datetime    NULL  ,
`cooperator_id` int(11)    NULL  ,
`citation_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`site_code` varchar(8)    NULL  ,
`accepted_date` datetime    NULL  ,
`expected_date` datetime    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_right_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_source *************/
select 'creating table dev5.accession_source ...' as Action;
CREATE TABLE `dev5`.`accession_source` (
`accession_source_id` int(11)    NOT NULL AUTO_INCREMENT ,
`type_code` varchar(10)    NOT NULL  ,
`step_date` datetime    NULL  ,
`step_date_format` varchar(10)    NULL  ,
`is_origin_step` char(1)    NOT NULL  ,
`accession_id` int(11)    NOT NULL  ,
`geography_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`source_qualifier` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_source_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_source_member *************/
select 'creating table dev5.accession_source_member ...' as Action;
CREATE TABLE `dev5`.`accession_source_member` (
`accession_source_member_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_source_id` int(11)    NOT NULL  ,
`accession_id` int(11)    NOT NULL  ,
`cooperator_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_source_member_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_voucher *************/
select 'creating table dev5.accession_voucher ...' as Action;
CREATE TABLE `dev5`.`accession_voucher` (
`accession_voucher_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`voucher_type` varchar(10)    NOT NULL  ,
`inventory_id` int(11)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`vouchered_date` datetime    NULL  ,
`vouchered_date_format` varchar(10)    NULL  ,
`collector_identifier` varchar(40)    NULL  ,
`storage_location` varchar(200)    NOT NULL  ,
`sample_contents` varchar(100)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_voucher_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.accession_voucher_image *************/
select 'creating table dev5.accession_voucher_image ...' as Action;
CREATE TABLE `dev5`.`accession_voucher_image` (
`accession_voucher_image_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_voucher_id` int(11)    NOT NULL  ,
`relative_image_path` varchar(500)    NULL  ,
`relative_thumbnail_path` varchar(500)    NULL  ,
`relative_image_url` varchar(500)    NULL  ,
`relative_thumbnail_url` varchar(500)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_voucher_image_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.app_resource *************/
select 'creating table dev5.app_resource ...' as Action;
CREATE TABLE `dev5`.`app_resource` (
`app_resource_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_lang_id` int(11)    NOT NULL  ,
`app_resource_name` varchar(100)    NOT NULL  ,
`display_member` varchar(4000)    NOT NULL  ,
`value_member` varchar(4000)    NOT NULL  ,
`sort_order` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`app_resource_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.app_user_item_list *************/
select 'creating table dev5.app_user_item_list ...' as Action;
CREATE TABLE `dev5`.`app_user_item_list` (
`app_user_item_list_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`tab_name` varchar(100)    NOT NULL  ,
`list_name` varchar(100)    NOT NULL  ,
`id_number` int(11)    NOT NULL  ,
`id_type` varchar(100)    NOT NULL  ,
`friendly_name` varchar(1000)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`app_user_item_list_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.code_group *************/
select 'creating table dev5.code_group ...' as Action;
CREATE TABLE `dev5`.`code_group` (
`code_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(50)    NOT NULL  ,
`site_code` varchar(30)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`code_group_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.code_rule *************/
select 'creating table dev5.code_rule ...' as Action;
CREATE TABLE `dev5`.`code_rule` (
`code_rule_id` int(11)    NOT NULL AUTO_INCREMENT ,
`table_name` varchar(30)    NOT NULL  ,
`column_name` varchar(30)    NOT NULL  ,
`code_value_id` int(11)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`max_length` int(11)    NULL  ,
`function_name` varchar(7)    NULL  ,
`is_standard` char(1)    NOT NULL  ,
`is_by_category` char(1)    NOT NULL  ,
`cateogry_number` int(11)    NULL  ,
`category_note` varchar(240)    NULL  ,
`form_name` varchar(30)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`code_rule_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.code_value *************/
select 'creating table dev5.code_value ...' as Action;
CREATE TABLE `dev5`.`code_value` (
`code_value_id` int(11)    NOT NULL AUTO_INCREMENT ,
`code_group_id` int(11)    NOT NULL  ,
`value` varchar(10)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`is_standard` char(1)    NOT NULL  ,
`category_name` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`code_value_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.code_value_friendly *************/
select 'creating table dev5.code_value_friendly ...' as Action;
CREATE TABLE `dev5`.`code_value_friendly` (
`code_value_friendly_id` int(11)    NOT NULL AUTO_INCREMENT ,
`code_value_id` int(11)    NOT NULL  ,
`sec_lang_id` int(11)    NOT NULL  ,
`friendly_name` varchar(50)    NOT NULL  ,
`friendly_description` varchar(4000)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`code_value_friendly_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.cooperator *************/
select 'creating table dev5.cooperator ...' as Action;
CREATE TABLE `dev5`.`cooperator` (
`cooperator_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_cooperator_id` int(11)    NULL  ,
`site_code` varchar(8)    NULL  ,
`last_name` varchar(40)    NULL  ,
`title` varchar(5)    NULL  ,
`first_name` varchar(30)    NULL  ,
`job` varchar(40)    NULL  ,
`organization` varchar(60)    NULL  ,
`organization_code` varchar(10)    NULL  ,
`address_line1` varchar(60)    NULL  ,
`address_line2` varchar(60)    NULL  ,
`address_line3` varchar(60)    NULL  ,
`admin_1` varchar(20)    NULL  ,
`admin_2` varchar(10)    NULL  ,
`geography_id` int(11)    NULL  ,
`primary_phone` varchar(30)    NULL  ,
`secondary_phone` varchar(30)    NULL  ,
`fax` varchar(30)    NULL  ,
`email` varchar(100)    NULL  ,
`is_active` char(1)    NOT NULL  ,
`category_code` varchar(4)    NULL  ,
`ars_region` varchar(3)    NULL  ,
`discipline` varchar(10)    NULL  ,
`initials` varchar(6)    NULL  ,
`full_name` varchar(100)    NULL  ,
`note` varchar(240)    NULL  ,
`sec_lang_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`cooperator_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.cooperator_group *************/
select 'creating table dev5.cooperator_group ...' as Action;
CREATE TABLE `dev5`.`cooperator_group` (
`cooperator_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(60)    NULL  ,
`site_code` varchar(8)    NULL  ,
`is_historical` char(1)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`cooperator_group_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.cooperator_member *************/
select 'creating table dev5.cooperator_member ...' as Action;
CREATE TABLE `dev5`.`cooperator_member` (
`cooperator_member_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`cooperator_group_id` int(11)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`localid` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`cooperator_member_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.crop *************/
select 'creating table dev5.crop ...' as Action;
CREATE TABLE `dev5`.`crop` (
`crop_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(20)    NOT NULL  ,
`note` varchar(2000)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.evaluation *************/
select 'creating table dev5.evaluation ...' as Action;
CREATE TABLE `dev5`.`evaluation` (
`evaluation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(100)    NOT NULL  ,
`site_code` varchar(8)    NULL  ,
`geography_id` int(11)    NULL  ,
`material_or_method_used` varchar(4000)    NULL  ,
`study_reason` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`evaluation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.evaluation_citation *************/
select 'creating table dev5.evaluation_citation ...' as Action;
CREATE TABLE `dev5`.`evaluation_citation` (
`evaluation_citation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`evaluation_id` int(11)    NOT NULL  ,
`literature_id` int(11)    NULL  ,
`title` varchar(240)    NULL  ,
`author_name` varchar(240)    NULL  ,
`citation_year_date` datetime    NULL  ,
`reference` varchar(240)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`evaluation_citation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.evaluation_member *************/
select 'creating table dev5.evaluation_member ...' as Action;
CREATE TABLE `dev5`.`evaluation_member` (
`evaluation_member_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`evaluation_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`evaluation_member_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.family *************/
select 'creating table dev5.family ...' as Action;
CREATE TABLE `dev5`.`family` (
`family_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_family_id` int(11)    NULL  ,
`faimly_name` varchar(25)    NOT NULL  ,
`author_name` varchar(100)    NULL  ,
`alternate_name` varchar(25)    NULL  ,
`subfamily` varchar(25)    NULL  ,
`tribe` varchar(25)    NULL  ,
`subtribe` varchar(25)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`family_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genomic_annotation *************/
select 'creating table dev5.genomic_annotation ...' as Action;
CREATE TABLE `dev5`.`genomic_annotation` (
`genomic_annotation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`marker_id` int(11)    NOT NULL  ,
`evaluation_id` int(11)    NOT NULL  ,
`method` varchar(2000)    NULL  ,
`scoring_method` varchar(1000)    NULL  ,
`control_values` varchar(1000)    NULL  ,
`observation_alleles_count` int(11)    NULL  ,
`max_gob_alleles` int(11)    NULL  ,
`size_alleles` varchar(100)    NULL  ,
`unusual_alleles` varchar(100)    NULL  ,
`note` varchar(4000)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genomic_annotation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genomic_marker *************/
select 'creating table dev5.genomic_marker ...' as Action;
CREATE TABLE `dev5`.`genomic_marker` (
`genomic_marker_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_id` int(11)    NOT NULL  ,
`site_code` varchar(10)    NOT NULL  ,
`name` varchar(100)    NOT NULL  ,
`synonyms` varchar(200)    NULL  ,
`repeat_motif` varchar(100)    NULL  ,
`primers` varchar(200)    NULL  ,
`assay_conditions` varchar(4000)    NULL  ,
`range_products` varchar(60)    NULL  ,
`known_standards` varchar(300)    NULL  ,
`genebank_number` varchar(20)    NULL  ,
`map_location` varchar(100)    NULL  ,
`position` varchar(1000)    NULL  ,
`note` varchar(4000)    NULL  ,
`poly_type` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genomic_marker_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genomic_marker_citation *************/
select 'creating table dev5.genomic_marker_citation ...' as Action;
CREATE TABLE `dev5`.`genomic_marker_citation` (
`genomic_marker_citation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`genomic_marker_id` int(11)    NOT NULL  ,
`literature_id` int(11)    NULL  ,
`title` varchar(240)    NULL  ,
`author_name` varchar(240)    NULL  ,
`citation_year_date` datetime    NULL  ,
`reference` varchar(60)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genomic_marker_citation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genomic_observation *************/
select 'creating table dev5.genomic_observation ...' as Action;
CREATE TABLE `dev5`.`genomic_observation` (
`genomic_observation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`genomic_annotation_id` int(11)    NOT NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`individual` int(11)    NULL  ,
`value` varchar(1000)    NOT NULL  ,
`genebank_url` varchar(200)    NULL  ,
`image_url` varchar(200)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NOT NULL  ,
`modified_by` int(11)    NOT NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genomic_observation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genus *************/
select 'creating table dev5.genus ...' as Action;
CREATE TABLE `dev5`.`genus` (
`genus_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_genus_id` int(11)    NULL  ,
`qualifying_code` varchar(2)    NULL  ,
`is_hybrid` char(1)    NOT NULL  ,
`genus_name` varchar(30)    NOT NULL  ,
`genus_authority` varchar(100)    NULL  ,
`subgenus_name` varchar(30)    NULL  ,
`section_name` varchar(30)    NULL  ,
`series_name` varchar(30)    NULL  ,
`subseries_name` varchar(30)    NULL  ,
`family_id` int(11)    NULL  ,
`alternate_family` varchar(100)    NULL  ,
`common_name` varchar(30)    NULL  ,
`note` varchar(240)    NULL  ,
`subsection_name` varchar(30)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genus_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genus_citation *************/
select 'creating table dev5.genus_citation ...' as Action;
CREATE TABLE `dev5`.`genus_citation` (
`genus_citation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`genus_id` int(11)    NOT NULL  ,
`literature_id` int(11)    NULL  ,
`title` varchar(240)    NULL  ,
`author_name` varchar(240)    NULL  ,
`citation_year_date` datetime    NULL  ,
`reference` varchar(60)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genus_citation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.genus_type *************/
select 'creating table dev5.genus_type ...' as Action;
CREATE TABLE `dev5`.`genus_type` (
`genus_type_id` int(11)    NOT NULL AUTO_INCREMENT ,
`family_id` int(11)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genus_type_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.geography *************/
select 'creating table dev5.geography ...' as Action;
CREATE TABLE `dev5`.`geography` (
`geography_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_geography_id` int(11)    NULL  ,
`country_name` varchar(30)    NOT NULL  ,
`state_name` varchar(20)    NULL  ,
`country_iso_full_name` varchar(100)    NULL  ,
`country_iso_short_name` varchar(60)    NULL  ,
`state_full_name` varchar(60)    NULL  ,
`iso_3_char_country_code` varchar(3)    NULL  ,
`iso_2_char_country_code` varchar(2)    NULL  ,
`state_code` varchar(3)    NULL  ,
`is_valid` char(1)    NOT NULL  ,
`latitude_hemisphere` char(1)    NULL  ,
`longitude_hemisphere` char(1)    NULL  ,
`region_id` int(11)    NULL  ,
`changed_date` datetime    NULL  ,
`previous_name` varchar(100)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`geography_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory *************/
select 'creating table dev5.inventory ...' as Action;
CREATE TABLE `dev5`.`inventory` (
`inventory_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_prefix` varchar(4)    NOT NULL  ,
`inventory_number` int(11)    NOT NULL  ,
`inventory_suffix` varchar(8)    NULL  ,
`inventory_type_code` varchar(2)    NOT NULL  ,
`inventory_maintenance_id` int(11)    NULL  ,
`site_code` varchar(8)    NULL  ,
`is_distributable` char(1)    NOT NULL  ,
`location_section_1` varchar(10)    NULL  ,
`location_section_2` varchar(10)    NULL  ,
`location_section_3` varchar(10)    NULL  ,
`location_section_4` varchar(10)    NULL  ,
`quantity_on_hand` int(11)    NULL  ,
`unit_of_quantity_on_hand` varchar(2)    NULL  ,
`is_debit` char(1)    NOT NULL  ,
`distribution_default_form` varchar(2)    NULL  ,
`standard_distribution_quantity` int(11)    NULL  ,
`unit_of_distribution` varchar(2)    NULL  ,
`distribution_critical_amount` int(11)    NULL  ,
`replenishment_critical_amount` int(11)    NULL  ,
`pathogen_status` varchar(10)    NULL  ,
`availability_status` varchar(10)    NULL  ,
`status_note` varchar(60)    NULL  ,
`accession_id` int(11)    NOT NULL  ,
`parent_inventory_id` int(11)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`backup_inventory_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory_action *************/
select 'creating table dev5.inventory_action ...' as Action;
CREATE TABLE `dev5`.`inventory_action` (
`inventory_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`action_name` varchar(10)    NOT NULL  ,
`occurred_date` datetime    NULL  ,
`occurred_date_format` varchar(10)    NULL  ,
`quantity` int(11)    NULL  ,
`unit_of_quantity` varchar(2)    NULL  ,
`form_involved` varchar(2)    NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`cooperator_id` int(11)    NULL  ,
`evaluation_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`qualifier` varchar(10)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_action_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory_group *************/
select 'creating table dev5.inventory_group ...' as Action;
CREATE TABLE `dev5`.`inventory_group` (
`inventory_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`group_name` varchar(100)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_group_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory_group_maintenance *************/
select 'creating table dev5.inventory_group_maintenance ...' as Action;
CREATE TABLE `dev5`.`inventory_group_maintenance` (
`inventory_group_maintenance_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`inventory_group_id` int(11)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_group_maintenance_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory_maintenance *************/
select 'creating table dev5.inventory_maintenance ...' as Action;
CREATE TABLE `dev5`.`inventory_maintenance` (
`inventory_maintenance_id` int(11)    NOT NULL AUTO_INCREMENT ,
`maintenance_name` varchar(20)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`inventory_default_form` varchar(2)    NOT NULL  ,
`unit_of_maintenance` varchar(2)    NULL  ,
`is_debit` char(1)    NOT NULL  ,
`distribution_default_form` varchar(2)    NOT NULL  ,
`standard_distribution_quantity` int(11)    NULL  ,
`unit_of_distribution` varchar(2)    NULL  ,
`distribution_critical_amount` int(11)    NULL  ,
`replenishment_critical_amount` int(11)    NULL  ,
`regeneration_method` varchar(10)    NULL  ,
`standard_pathogen_test_count` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_maintenance_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory_pathogen_test *************/
select 'creating table dev5.inventory_pathogen_test ...' as Action;
CREATE TABLE `dev5`.`inventory_pathogen_test` (
`inventory_pathogen_test_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`test_type` varchar(10)    NOT NULL  ,
`pathogen_code` varchar(10)    NOT NULL  ,
`started_date` datetime    NULL  ,
`finished_date` datetime    NULL  ,
`test_results` varchar(10)    NULL  ,
`needed_count` int(11)    NULL  ,
`started_count` int(11)    NULL  ,
`completed_count` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_pathogen_test_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.inventory_viability *************/
select 'creating table dev5.inventory_viability ...' as Action;
CREATE TABLE `dev5`.`inventory_viability` (
`inventory_viability_id` int(11)    NOT NULL AUTO_INCREMENT ,
`tested_date` datetime    NOT NULL  ,
`tested_date_format` varchar(10)    NULL  ,
`percent_normal` int(11)    NULL  ,
`percent_abnormal` int(11)    NULL  ,
`percent_dormant` int(11)    NULL  ,
`percent_viable` int(11)    NULL  ,
`vigor_rating` varchar(10)    NULL  ,
`sample_count` int(11)    NULL  ,
`replication_count` int(11)    NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`evaluation_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_viability_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.literature *************/
select 'creating table dev5.literature ...' as Action;
CREATE TABLE `dev5`.`literature` (
`literature_id` int(11)    NOT NULL AUTO_INCREMENT ,
`abbreviation` varchar(20)    NOT NULL  ,
`standard_abbreviation` varchar(240)    NULL  ,
`reference_title` varchar(240)    NULL  ,
`author_editor_name` varchar(240)    NULL  ,
`note` varchar(240)    NULL  ,
`site_code` varchar(8)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`literature_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.order_entry *************/
select 'creating table dev5.order_entry ...' as Action;
CREATE TABLE `dev5`.`order_entry` (
`order_entry_id` int(11)    NOT NULL AUTO_INCREMENT ,
`original_order_entry_id` int(11)    NULL  ,
`site_code` varchar(8)    NULL  ,
`local_number` int(11)    NULL  ,
`order_type` varchar(2)    NULL  ,
`ordered_date` datetime    NULL  ,
`status` varchar(10)    NULL  ,
`is_completed` char(1)    NOT NULL  ,
`acted_date` datetime    NULL  ,
`source_cooperator_id` int(11)    NULL  ,
`requestor_cooperator_id` int(11)    NULL  ,
`ship_to_cooperator_id` int(11)    NULL  ,
`final_recipient_cooperator_id` int(11)    NOT NULL  ,
`order_obtained_via` varchar(10)    NULL  ,
`is_supply_low` char(1)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`special_instruction` varchar(900)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_entry_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.order_entry_action *************/
select 'creating table dev5.order_entry_action ...' as Action;
CREATE TABLE `dev5`.`order_entry_action` (
`order_entry_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`action_name` varchar(10)    NOT NULL  ,
`acted_date` datetime    NOT NULL  ,
`action_for_id` varchar(40)    NULL  ,
`order_entry_id` int(11)    NOT NULL  ,
`site_code` varchar(8)    NULL  ,
`note` varchar(240)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_entry_action_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.order_entry_item *************/
select 'creating table dev5.order_entry_item ...' as Action;
CREATE TABLE `dev5`.`order_entry_item` (
`order_entry_item_id` int(11)    NOT NULL AUTO_INCREMENT ,
`order_entry_id` int(11)    NULL  ,
`item_sequence_number` int(11)    NULL  ,
`item_name` varchar(40)    NULL  ,
`quantity_shipped` int(11)    NULL  ,
`unit_of_shipped` varchar(2)    NULL  ,
`distribution_form` varchar(2)    NULL  ,
`ipr_restriction` varchar(10)    NULL  ,
`item_status` varchar(10)    NULL  ,
`acted_date` datetime    NULL  ,
`cooperator_id` int(11)    NULL  ,
`inventory_id` int(11)    NULL  ,
`accession_id` int(11)    NULL  ,
`taxonomy_id` int(11)    NULL  ,
`external_taxonomy` varchar(100)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_entry_item_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.plant_introduction *************/
select 'creating table dev5.plant_introduction ...' as Action;
CREATE TABLE `dev5`.`plant_introduction` (
`plant_introduction_id` int(11)    NOT NULL AUTO_INCREMENT ,
`plant_introduction_year_date` datetime    NOT NULL  ,
`lowest_pi_number` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`plant_introduction_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.region *************/
select 'creating table dev5.region ...' as Action;
CREATE TABLE `dev5`.`region` (
`region_id` int(11)    NOT NULL AUTO_INCREMENT ,
`continent` varchar(20)    NOT NULL  ,
`subcontinent` varchar(30)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`region_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_lang *************/
select 'creating table dev5.sec_lang ...' as Action;
CREATE TABLE `dev5`.`sec_lang` (
`sec_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`iso_639_3_code` varchar(5)    NOT NULL  ,
`ietf_tag` varchar(30)    NULL  ,
`language_name` varchar(100)    NOT NULL  ,
`description` varchar(500)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_lang_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_perm *************/
select 'creating table dev5.sec_perm ...' as Action;
CREATE TABLE `dev5`.`sec_perm` (
`sec_perm_id` int(11)    NOT NULL AUTO_INCREMENT ,
`table_name` varchar(50)    NOT NULL  ,
`sec_perm_template_id` int(11)    NULL  ,
`is_enabled` char(1)    NOT NULL  ,
`perm_type` char(1)    NULL  ,
`perm_value` char(1)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_perm_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_perm_field *************/
select 'creating table dev5.sec_perm_field ...' as Action;
CREATE TABLE `dev5`.`sec_perm_field` (
`sec_perm_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_perm_id` int(11)    NOT NULL  ,
`field_name` varchar(50)    NOT NULL  ,
`field_type` varchar(20)    NOT NULL  ,
`compare_operator` varchar(20)    NOT NULL  ,
`compare_value` varchar(100)    NULL  ,
`compare_field_name` varchar(150)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_perm_field_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_perm_template *************/
select 'creating table dev5.sec_perm_template ...' as Action;
CREATE TABLE `dev5`.`sec_perm_template` (
`sec_perm_template_id` int(11)    NOT NULL AUTO_INCREMENT ,
`template_name` varchar(100)    NOT NULL  ,
`is_enabled` char(1)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_perm_template_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_perm_template_map *************/
select 'creating table dev5.sec_perm_template_map ...' as Action;
CREATE TABLE `dev5`.`sec_perm_template_map` (
`sec_perm_template_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_perm_template_id` int(11)    NOT NULL  ,
`sec_perm_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_perm_template_map_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_rs *************/
select 'creating table dev5.sec_rs ...' as Action;
CREATE TABLE `dev5`.`sec_rs` (
`sec_rs_id` int(11)    NOT NULL AUTO_INCREMENT ,
`rs_name` varchar(100)    NOT NULL  ,
`sql_statement` varchar(4000)    NULL  ,
`is_enabled` char(1)    NOT NULL  ,
`is_updateable` char(1)    NOT NULL  ,
`description` varchar(500)    NULL  ,
`is_system` char(1)    NOT NULL  ,
`suppress_properties` char(1)    NOT NULL  ,
`is_user_visible` char(1)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_rs_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_rs_field *************/
select 'creating table dev5.sec_rs_field ...' as Action;
CREATE TABLE `dev5`.`sec_rs_field` (
`sec_rs_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_rs_id` int(11)    NOT NULL  ,
`field_name` varchar(50)    NOT NULL  ,
`sec_table_field_id` int(11)    NULL  ,
`is_updateable` char(1)    NOT NULL  ,
`sort_order` int(11)    NOT NULL  ,
`foreign_key_resultset_name` varchar(50)    NULL  ,
`description` varchar(500)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_rs_field_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_rs_field_friendly *************/
select 'creating table dev5.sec_rs_field_friendly ...' as Action;
CREATE TABLE `dev5`.`sec_rs_field_friendly` (
`sec_rs_field_friendly_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_rs_field_id` int(11)    NOT NULL  ,
`sec_lang_id` int(11)    NOT NULL  ,
`friendly_field_name` varchar(100)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_rs_field_friendly_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_rs_param *************/
select 'creating table dev5.sec_rs_param ...' as Action;
CREATE TABLE `dev5`.`sec_rs_param` (
`sec_rs_param_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_rs_id` int(11)    NOT NULL  ,
`param_name` varchar(50)    NOT NULL  ,
`param_type` varchar(50)    NULL  ,
`sort_order` int(11)    NULL  ,
`description` varchar(100)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_rs_param_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_table *************/
select 'creating table dev5.sec_table ...' as Action;
CREATE TABLE `dev5`.`sec_table` (
`sec_table_id` int(11)    NOT NULL AUTO_INCREMENT ,
`table_name` varchar(50)    NOT NULL  ,
`is_enabled` char(1)    NOT NULL  ,
`is_updateable` char(1)    NOT NULL  ,
`audits_created` char(1)    NOT NULL  ,
`audits_modified` char(1)    NOT NULL  ,
`audits_owned` char(1)    NOT NULL  ,
`database_area` varchar(100)    NULL  ,
`description` varchar(500)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_table_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_table_field *************/
select 'creating table dev5.sec_table_field ...' as Action;
CREATE TABLE `dev5`.`sec_table_field` (
`sec_table_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_table_id` int(11)    NOT NULL  ,
`field_name` varchar(50)    NOT NULL  ,
`field_purpose` varchar(50)    NOT NULL  ,
`field_type` varchar(50)    NOT NULL  ,
`description` varchar(500)    NULL  ,
`is_primary_key` char(1)    NOT NULL  ,
`is_foreign_key` char(1)    NOT NULL  ,
`foreign_key_field_id` int(11)    NULL  ,
`foreign_key_resultset_name` varchar(50)    NULL  ,
`is_nullable` char(1)    NOT NULL  ,
`gui_hint` varchar(50)    NOT NULL  ,
`is_readonly` char(1)    NOT NULL  ,
`min_length` int(11)    NOT NULL  ,
`max_length` int(11)    NOT NULL  ,
`numeric_precision` int(11)    NOT NULL  ,
`numeric_scale` int(11)    NOT NULL  ,
`is_autoincrement` char(1)    NOT NULL  ,
`code_group_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_table_field_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_user *************/
select 'creating table dev5.sec_user ...' as Action;
CREATE TABLE `dev5`.`sec_user` (
`sec_user_id` int(11)    NOT NULL AUTO_INCREMENT ,
`user_name` varchar(50)    NOT NULL  ,
`password` varchar(255)    NOT NULL  ,
`is_enabled` char(1)    NOT NULL  ,
`cooperator_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_user_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_user_gui_setting *************/
select 'creating table dev5.sec_user_gui_setting ...' as Action;
CREATE TABLE `dev5`.`sec_user_gui_setting` (
`sec_user_gui_setting_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`resource_name` varchar(100)    NOT NULL  ,
`resource_key` varchar(100)    NOT NULL  ,
`resource_value` varchar(4000)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_user_gui_setting_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.sec_user_perm *************/
select 'creating table dev5.sec_user_perm ...' as Action;
CREATE TABLE `dev5`.`sec_user_perm` (
`sec_user_perm_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sec_user_id` int(11)    NOT NULL  ,
`sec_perm_id` int(11)    NOT NULL  ,
`is_enabled` char(1)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sec_user_perm_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy *************/
select 'creating table dev5.taxonomy ...' as Action;
CREATE TABLE `dev5`.`taxonomy` (
`taxonomy_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_taxonomy_id` int(11)    NULL  ,
`is_interspecific_hybrid` char(1)    NOT NULL  ,
`species` varchar(30)    NOT NULL  ,
`species_authority` varchar(100)    NULL  ,
`is_intraspecific_hybrid` char(1)    NOT NULL  ,
`subspecies` varchar(30)    NULL  ,
`subspecies_authority` varchar(100)    NULL  ,
`is_intervarietal_hybrid` char(1)    NOT NULL  ,
`variety` varchar(30)    NULL  ,
`variety_authority` varchar(100)    NULL  ,
`is_subvarietal_hybrid` char(1)    NOT NULL  ,
`subvariety` varchar(30)    NULL  ,
`subvariety_authority` varchar(100)    NULL  ,
`is_forma_hybrid` char(1)    NOT NULL  ,
`forma` varchar(30)    NULL  ,
`forma_authority` varchar(100)    NULL  ,
`genus_id` int(11)    NOT NULL  ,
`crop_id` int(11)    NULL  ,
`priority_site_1` varchar(8)    NULL  ,
`priority_site_2` varchar(8)    NULL  ,
`restriction` varchar(10)    NULL  ,
`life_form` varchar(10)    NULL  ,
`common_fertilization` varchar(10)    NULL  ,
`is_name_pending` char(1)    NOT NULL  ,
`synonym_code` varchar(6)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`name_verified_date` datetime    NULL  ,
`name` varchar(100)    NULL  ,
`name_authority` varchar(100)    NULL  ,
`protologue` varchar(240)    NULL  ,
`note` varchar(2000)    NULL  ,
`site_note` varchar(240)    NULL  ,
`alternate_name` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_author *************/
select 'creating table dev5.taxonomy_author ...' as Action;
CREATE TABLE `dev5`.`taxonomy_author` (
`taxonomy_author_id` int(11)    NOT NULL AUTO_INCREMENT ,
`short_name` varchar(30)    NOT NULL  ,
`full_name` varchar(100)    NULL  ,
`short_name_diacritic` varchar(30)    NULL  ,
`full_name_diacritic` varchar(100)    NULL  ,
`short_name_expanded_diacritic` varchar(30)    NULL  ,
`full_name_expanded_diacritic` varchar(100)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_author_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_citation *************/
select 'creating table dev5.taxonomy_citation ...' as Action;
CREATE TABLE `dev5`.`taxonomy_citation` (
`taxonomy_citation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_id` int(11)    NOT NULL  ,
`literature_id` int(11)    NULL  ,
`title` varchar(240)    NULL  ,
`author_name` varchar(240)    NULL  ,
`citation_year_date` datetime    NULL  ,
`reference` varchar(60)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_citation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_common_name *************/
select 'creating table dev5.taxonomy_common_name ...' as Action;
CREATE TABLE `dev5`.`taxonomy_common_name` (
`taxonomy_common_name_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_id` int(11)    NOT NULL  ,
`name` varchar(50)    NOT NULL  ,
`source` varchar(20)    NULL  ,
`note` varchar(240)    NULL  ,
`simplified_name` varchar(50)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_common_name_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_distribution *************/
select 'creating table dev5.taxonomy_distribution ...' as Action;
CREATE TABLE `dev5`.`taxonomy_distribution` (
`taxonomy_distribution_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_id` int(11)    NOT NULL  ,
`geography_id` int(11)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_distribution_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_germination_rule *************/
select 'creating table dev5.taxonomy_germination_rule ...' as Action;
CREATE TABLE `dev5`.`taxonomy_germination_rule` (
`taxonomy_germination_rule_id` int(11)    NOT NULL AUTO_INCREMENT ,
`substrata` varchar(70)    NULL  ,
`temperature_range` varchar(30)    NULL  ,
`requirements` varchar(500)    NULL  ,
`author_name` varchar(20)    NULL  ,
`category` varchar(10)    NULL  ,
`days` varchar(20)    NULL  ,
`taxonomy_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_germination_rule_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_url *************/
select 'creating table dev5.taxonomy_url ...' as Action;
CREATE TABLE `dev5`.`taxonomy_url` (
`taxonomy_url_id` int(11)    NOT NULL AUTO_INCREMENT ,
`url_type` varchar(10)    NOT NULL  ,
`family_id` int(11)    NOT NULL  ,
`genus_id` int(11)    NULL  ,
`taxonomy_id` int(11)    NULL  ,
`caption` varchar(240)    NULL  ,
`url` varchar(100)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_url_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.taxonomy_use *************/
select 'creating table dev5.taxonomy_use ...' as Action;
CREATE TABLE `dev5`.`taxonomy_use` (
`taxonomy_use_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_id` int(11)    NOT NULL  ,
`economic_usage` varchar(10)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`usage_type` varchar(250)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_use_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.trait *************/
select 'creating table dev5.trait ...' as Action;
CREATE TABLE `dev5`.`trait` (
`trait_id` int(11)    NOT NULL AUTO_INCREMENT ,
`short_name` varchar(10)    NOT NULL  ,
`name` varchar(30)    NULL  ,
`is_cgc_approved` char(1)    NOT NULL  ,
`category_code` varchar(10)    NULL  ,
`data_type` varchar(10)    NOT NULL  ,
`is_coded` char(1)    NOT NULL  ,
`max_length` int(11)    NULL  ,
`numeric_format` varchar(15)    NULL  ,
`numeric_max` int(11)    NULL  ,
`numeric_min` int(11)    NULL  ,
`original_value_type` varchar(10)    NULL  ,
`original_value_format` varchar(15)    NULL  ,
`crop_id` int(11)    NOT NULL  ,
`site_code` varchar(8)    NULL  ,
`definition` varchar(240)    NULL  ,
`note` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`trait_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.trait_code *************/
select 'creating table dev5.trait_code ...' as Action;
CREATE TABLE `dev5`.`trait_code` (
`trait_code_id` int(11)    NOT NULL AUTO_INCREMENT ,
`trait_id` int(11)    NOT NULL  ,
`code` varchar(30)    NOT NULL  ,
`definition` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`trait_code_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.trait_code_friendly *************/
select 'creating table dev5.trait_code_friendly ...' as Action;
CREATE TABLE `dev5`.`trait_code_friendly` (
`trait_code_friendly_id` int(11)    NOT NULL AUTO_INCREMENT ,
`trait_code_id` int(11)    NOT NULL  ,
`sec_lang_id` int(11)    NOT NULL  ,
`friendly_name` varchar(50)    NOT NULL  ,
`friendly_description` varchar(4000)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`trait_code_friendly_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.trait_observation *************/
select 'creating table dev5.trait_observation ...' as Action;
CREATE TABLE `dev5`.`trait_observation` (
`trait_observation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`trait_id` int(11)    NOT NULL  ,
`trait_code_id` int(11)    NOT NULL  ,
`accession_id` int(11)    NOT NULL  ,
`evaluation_id` int(11)    NOT NULL  ,
`qualifier_id` int(11)    NULL  ,
`inventory_id` int(11)    NULL  ,
`original_value` varchar(30)    NULL  ,
`frequency` decimal(18, 5)    NULL  ,
`mean_value` decimal(18, 5)    NULL  ,
`maximum_value` decimal(18, 5)    NULL  ,
`minimum_value` decimal(18, 5)    NULL  ,
`standard_deviation` decimal(18, 5)    NULL  ,
`sample_size` decimal(18, 5)    NULL  ,
`note` varchar(500)    NULL  ,
`rank` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`trait_observation_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.trait_qualifier *************/
select 'creating table dev5.trait_qualifier ...' as Action;
CREATE TABLE `dev5`.`trait_qualifier` (
`trait_qualifier_id` int(11)    NOT NULL AUTO_INCREMENT ,
`trait_qualifier_name` varchar(30)    NOT NULL  ,
`trait_id` int(11)    NOT NULL  ,
`definition` varchar(240)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`trait_qualifier_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/************ Table Definition for dev5.trait_url *************/
select 'creating table dev5.trait_url ...' as Action;
CREATE TABLE `dev5`.`trait_url` (
`trait_url_id` int(11)    NOT NULL AUTO_INCREMENT ,
`url_type` varchar(10)    NOT NULL  ,
`sequence_number` int(11)    NOT NULL  ,
`crop_id` int(11)    NOT NULL  ,
`trait_id` int(11)    NULL  ,
`code` varchar(30)    NULL  ,
`caption` varchar(60)    NULL  ,
`url` varchar(100)    NOT NULL  ,
`site_code` varchar(8)    NOT NULL  ,
`note` varchar(240)    NULL  ,
`evaluation_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`trait_url_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;
;

/***********************************************/
/************** Index Definitions **************/
/***********************************************/

/************ 7 Index Definitions for accession *************/
select 'creating index ndx_uniq_ac for table accession ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ac`  ON `dev5`.`accession` (`accession_prefix`, `accession_number`, `accession_suffix`);

select 'creating index ndx_fk_a_an for table accession ...' as Action;
CREATE  INDEX `ndx_fk_a_an`  ON `dev5`.`accession` (`accession_name_id`);

select 'creating index ndx_fk_a_t for table accession ...' as Action;
CREATE  INDEX `ndx_fk_a_t`  ON `dev5`.`accession` (`taxonomy_id`);

select 'creating index ndx_fk_a_pi for table accession ...' as Action;
CREATE  INDEX `ndx_fk_a_pi`  ON `dev5`.`accession` (`plant_introduction_id`);

select 'creating index ndx_fk_a_created for table accession ...' as Action;
CREATE  INDEX `ndx_fk_a_created`  ON `dev5`.`accession` (`created_by`);

select 'creating index ndx_fk_a_modified for table accession ...' as Action;
CREATE  INDEX `ndx_fk_a_modified`  ON `dev5`.`accession` (`modified_by`);

select 'creating index ndx_fk_a_owned for table accession ...' as Action;
CREATE  INDEX `ndx_fk_a_owned`  ON `dev5`.`accession` (`owned_by`);

/************ 6 Index Definitions for accession_action *************/
select 'creating index ndx_fk_aa_a for table accession_action ...' as Action;
CREATE  INDEX `ndx_fk_aa_a`  ON `dev5`.`accession_action` (`accession_id`);

select 'creating index ndx_fk_aa_c for table accession_action ...' as Action;
CREATE  INDEX `ndx_fk_aa_c`  ON `dev5`.`accession_action` (`cooperator_id`);

select 'creating index ndx_fk_aa_e for table accession_action ...' as Action;
CREATE  INDEX `ndx_fk_aa_e`  ON `dev5`.`accession_action` (`evaluation_id`);

select 'creating index ndx_fk_aa_created for table accession_action ...' as Action;
CREATE  INDEX `ndx_fk_aa_created`  ON `dev5`.`accession_action` (`created_by`);

select 'creating index ndx_fk_aa_modified for table accession_action ...' as Action;
CREATE  INDEX `ndx_fk_aa_modified`  ON `dev5`.`accession_action` (`modified_by`);

select 'creating index ndx_fk_aa_owned for table accession_action ...' as Action;
CREATE  INDEX `ndx_fk_aa_owned`  ON `dev5`.`accession_action` (`owned_by`);

/************ 12 Index Definitions for accession_annotation *************/
select 'creating index ndx_uniq_al for table accession_annotation ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_al`  ON `dev5`.`accession_annotation` (`action_name`, `action_date`, `accession_id`);

select 'creating index ndx_fk_aan_a for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_a`  ON `dev5`.`accession_annotation` (`accession_id`);

select 'creating index ndx_fk_al_site for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_al_site`  ON `dev5`.`accession_annotation` (`site_code`);

select 'creating index ndx_fk_aan_c for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_c`  ON `dev5`.`accession_annotation` (`cooperator_id`);

select 'creating index ndx_fk_aan_i for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_i`  ON `dev5`.`accession_annotation` (`inventory_id`);

select 'creating index ndx_fk_aan_oe for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_oe`  ON `dev5`.`accession_annotation` (`order_entry_id`);

select 'creating index ndx_fk_aan_t_old for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_t_old`  ON `dev5`.`accession_annotation` (`old_taxonomy_id`);

select 'creating index ndx_fk_aan_t_new for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_t_new`  ON `dev5`.`accession_annotation` (`new_taxonomy_id`);

select 'creating index ndx_fk_al_oldtax for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_al_oldtax`  ON `dev5`.`accession_annotation` (`new_taxonomy_id`);

select 'creating index ndx_fk_aan_created for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_created`  ON `dev5`.`accession_annotation` (`created_by`);

select 'creating index ndx_fk_aan_modified for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_modified`  ON `dev5`.`accession_annotation` (`modified_by`);

select 'creating index ndx_fk_aan_owned for table accession_annotation ...' as Action;
CREATE  INDEX `ndx_fk_aan_owned`  ON `dev5`.`accession_annotation` (`owned_by`);

/************ 5 Index Definitions for accession_citation *************/
select 'creating index ndx_fk_ac_a for table accession_citation ...' as Action;
CREATE  INDEX `ndx_fk_ac_a`  ON `dev5`.`accession_citation` (`accession_id`);

select 'creating index ndx_fk_ac_l for table accession_citation ...' as Action;
CREATE  INDEX `ndx_fk_ac_l`  ON `dev5`.`accession_citation` (`literature_id`);

select 'creating index ndx_fk_ac_created for table accession_citation ...' as Action;
CREATE  INDEX `ndx_fk_ac_created`  ON `dev5`.`accession_citation` (`created_by`);

select 'creating index ndx_fk_ac_modified for table accession_citation ...' as Action;
CREATE  INDEX `ndx_fk_ac_modified`  ON `dev5`.`accession_citation` (`modified_by`);

select 'creating index ndx_fk_ac_owned for table accession_citation ...' as Action;
CREATE  INDEX `ndx_fk_ac_owned`  ON `dev5`.`accession_citation` (`owned_by`);

/************ 3 Index Definitions for accession_group *************/
select 'creating index ndx_fk_ag_created for table accession_group ...' as Action;
CREATE  INDEX `ndx_fk_ag_created`  ON `dev5`.`accession_group` (`created_by`);

select 'creating index ndx_fk_ag_modified for table accession_group ...' as Action;
CREATE  INDEX `ndx_fk_ag_modified`  ON `dev5`.`accession_group` (`modified_by`);

select 'creating index ndx_fk_ag_owned for table accession_group ...' as Action;
CREATE  INDEX `ndx_fk_ag_owned`  ON `dev5`.`accession_group` (`owned_by`);

/************ 4 Index Definitions for accession_habitat *************/
select 'creating index ndx_fk_ah_a for table accession_habitat ...' as Action;
CREATE  INDEX `ndx_fk_ah_a`  ON `dev5`.`accession_habitat` (`accession_id`);

select 'creating index ndx_fk_ah_created for table accession_habitat ...' as Action;
CREATE  INDEX `ndx_fk_ah_created`  ON `dev5`.`accession_habitat` (`created_by`);

select 'creating index ndx_fk_ah_modified for table accession_habitat ...' as Action;
CREATE  INDEX `ndx_fk_ah_modified`  ON `dev5`.`accession_habitat` (`modified_by`);

select 'creating index ndx_fk_ah_owned for table accession_habitat ...' as Action;
CREATE  INDEX `ndx_fk_ah_owned`  ON `dev5`.`accession_habitat` (`owned_by`);

/************ 9 Index Definitions for accession_name *************/
select 'creating index ndx_fk_an_a for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_a`  ON `dev5`.`accession_name` (`accession_id`);

select 'creating index ndx_uniq_an for table accession_name ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_an`  ON `dev5`.`accession_name` (`accession_id`, `name`, `accession_group_id`);

select 'creating index ndx_an_name for table accession_name ...' as Action;
CREATE  INDEX `ndx_an_name`  ON `dev5`.`accession_name` (`name`);

select 'creating index ndx_fk_an_ag for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_ag`  ON `dev5`.`accession_name` (`accession_group_id`);

select 'creating index ndx_fk_an_i for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_i`  ON `dev5`.`accession_name` (`inventory_id`);

select 'creating index ndx_fk_an_c for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_c`  ON `dev5`.`accession_name` (`cooperator_id`);

select 'creating index ndx_fk_an_created for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_created`  ON `dev5`.`accession_name` (`created_by`);

select 'creating index ndx_fk_an_modified for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_modified`  ON `dev5`.`accession_name` (`modified_by`);

select 'creating index ndx_fk_an_owned for table accession_name ...' as Action;
CREATE  INDEX `ndx_fk_an_owned`  ON `dev5`.`accession_name` (`owned_by`);

/************ 4 Index Definitions for accession_narrative *************/
select 'creating index ndx_fk_ana_a for table accession_narrative ...' as Action;
CREATE  INDEX `ndx_fk_ana_a`  ON `dev5`.`accession_narrative` (`accession_id`);

select 'creating index ndx_fk_ana_created for table accession_narrative ...' as Action;
CREATE  INDEX `ndx_fk_ana_created`  ON `dev5`.`accession_narrative` (`created_by`);

select 'creating index ndx_fk_ana_modified for table accession_narrative ...' as Action;
CREATE  INDEX `ndx_fk_ana_modified`  ON `dev5`.`accession_narrative` (`modified_by`);

select 'creating index ndx_fk_ana_owned for table accession_narrative ...' as Action;
CREATE  INDEX `ndx_fk_ana_owned`  ON `dev5`.`accession_narrative` (`owned_by`);

/************ 5 Index Definitions for accession_pedigree *************/
select 'creating index ndx_fk_ap_a for table accession_pedigree ...' as Action;
CREATE  INDEX `ndx_fk_ap_a`  ON `dev5`.`accession_pedigree` (`accession_id`);

select 'creating index ndx_fk_ap_ac for table accession_pedigree ...' as Action;
CREATE  INDEX `ndx_fk_ap_ac`  ON `dev5`.`accession_pedigree` (`citation_id`);

select 'creating index ndx_fk_ap_created for table accession_pedigree ...' as Action;
CREATE  INDEX `ndx_fk_ap_created`  ON `dev5`.`accession_pedigree` (`created_by`);

select 'creating index ndx_fk_ap_modified for table accession_pedigree ...' as Action;
CREATE  INDEX `ndx_fk_ap_modified`  ON `dev5`.`accession_pedigree` (`modified_by`);

select 'creating index ndx_fk_ap_owned for table accession_pedigree ...' as Action;
CREATE  INDEX `ndx_fk_ap_owned`  ON `dev5`.`accession_pedigree` (`owned_by`);

/************ 5 Index Definitions for accession_quarantine *************/
select 'creating index ndx_fk_aq_a for table accession_quarantine ...' as Action;
CREATE  INDEX `ndx_fk_aq_a`  ON `dev5`.`accession_quarantine` (`accession_id`);

select 'creating index ndx_fk_aq_c for table accession_quarantine ...' as Action;
CREATE  INDEX `ndx_fk_aq_c`  ON `dev5`.`accession_quarantine` (`cooperator_id`);

select 'creating index ndx_fk_aq_created for table accession_quarantine ...' as Action;
CREATE  INDEX `ndx_fk_aq_created`  ON `dev5`.`accession_quarantine` (`created_by`);

select 'creating index ndx_fk_aq_modified for table accession_quarantine ...' as Action;
CREATE  INDEX `ndx_fk_aq_modified`  ON `dev5`.`accession_quarantine` (`modified_by`);

select 'creating index ndx_fk_aq_owned for table accession_quarantine ...' as Action;
CREATE  INDEX `ndx_fk_aq_owned`  ON `dev5`.`accession_quarantine` (`owned_by`);

/************ 10 Index Definitions for accession_right *************/
select 'creating index ndx_fk_ar_a for table accession_right ...' as Action;
CREATE  INDEX `ndx_fk_ar_a`  ON `dev5`.`accession_right` (`accession_id`);

select 'creating index ndx_uniq_ipr for table accession_right ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ipr`  ON `dev5`.`accession_right` (`accession_id`, `assigned_type`, `right_prefix`);

select 'creating index ndx_ipr_prefix for table accession_right ...' as Action;
CREATE  INDEX `ndx_ipr_prefix`  ON `dev5`.`accession_right` (`right_prefix`);

select 'creating index ndx_ipr_number for table accession_right ...' as Action;
CREATE  INDEX `ndx_ipr_number`  ON `dev5`.`accession_right` (`right_number`);

select 'creating index ndx_ipr_crop for table accession_right ...' as Action;
CREATE  INDEX `ndx_ipr_crop`  ON `dev5`.`accession_right` (`crop_name`);

select 'creating index ndx_fk_ar_c for table accession_right ...' as Action;
CREATE  INDEX `ndx_fk_ar_c`  ON `dev5`.`accession_right` (`cooperator_id`);

select 'creating index ndx_fk_ar_ac for table accession_right ...' as Action;
CREATE  INDEX `ndx_fk_ar_ac`  ON `dev5`.`accession_right` (`citation_id`);

select 'creating index ndx_fk_ar_created for table accession_right ...' as Action;
CREATE  INDEX `ndx_fk_ar_created`  ON `dev5`.`accession_right` (`created_by`);

select 'creating index ndx_fk_ar_modified for table accession_right ...' as Action;
CREATE  INDEX `ndx_fk_ar_modified`  ON `dev5`.`accession_right` (`modified_by`);

select 'creating index ndx_fk_ar_owned for table accession_right ...' as Action;
CREATE  INDEX `ndx_fk_ar_owned`  ON `dev5`.`accession_right` (`owned_by`);

/************ 5 Index Definitions for accession_source *************/
select 'creating index ndx_fk_as_a for table accession_source ...' as Action;
CREATE  INDEX `ndx_fk_as_a`  ON `dev5`.`accession_source` (`accession_id`);

select 'creating index ndx_fk_as_g for table accession_source ...' as Action;
CREATE  INDEX `ndx_fk_as_g`  ON `dev5`.`accession_source` (`geography_id`);

select 'creating index ndx_fk_as_created for table accession_source ...' as Action;
CREATE  INDEX `ndx_fk_as_created`  ON `dev5`.`accession_source` (`created_by`);

select 'creating index ndx_fk_as_modified for table accession_source ...' as Action;
CREATE  INDEX `ndx_fk_as_modified`  ON `dev5`.`accession_source` (`modified_by`);

select 'creating index ndx_fk_as_owned for table accession_source ...' as Action;
CREATE  INDEX `ndx_fk_as_owned`  ON `dev5`.`accession_source` (`owned_by`);

/************ 6 Index Definitions for accession_source_member *************/
select 'creating index ndx_fk_asm_as for table accession_source_member ...' as Action;
CREATE  INDEX `ndx_fk_asm_as`  ON `dev5`.`accession_source_member` (`accession_source_id`);

select 'creating index ndx_fk_asm_a for table accession_source_member ...' as Action;
CREATE  INDEX `ndx_fk_asm_a`  ON `dev5`.`accession_source_member` (`accession_id`);

select 'creating index ndx_fk_asm_c for table accession_source_member ...' as Action;
CREATE  INDEX `ndx_fk_asm_c`  ON `dev5`.`accession_source_member` (`cooperator_id`);

select 'creating index ndx_fk_asm_created for table accession_source_member ...' as Action;
CREATE  INDEX `ndx_fk_asm_created`  ON `dev5`.`accession_source_member` (`created_by`);

select 'creating index ndx_fk_asm_modified for table accession_source_member ...' as Action;
CREATE  INDEX `ndx_fk_asm_modified`  ON `dev5`.`accession_source_member` (`modified_by`);

select 'creating index ndx_fk_asm_owned for table accession_source_member ...' as Action;
CREATE  INDEX `ndx_fk_asm_owned`  ON `dev5`.`accession_source_member` (`owned_by`);

/************ 7 Index Definitions for accession_voucher *************/
select 'creating index ndx_fk_av_a for table accession_voucher ...' as Action;
CREATE  INDEX `ndx_fk_av_a`  ON `dev5`.`accession_voucher` (`accession_id`);

select 'creating index ndx_uniq_vo for table accession_voucher ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_vo`  ON `dev5`.`accession_voucher` (`accession_id`, `voucher_type`, `inventory_id`, `cooperator_id`, `vouchered_date`, `collector_identifier`, `storage_location`);

select 'creating index ndx_fk_av_i for table accession_voucher ...' as Action;
CREATE  INDEX `ndx_fk_av_i`  ON `dev5`.`accession_voucher` (`inventory_id`);

select 'creating index ndx_fk_av_c for table accession_voucher ...' as Action;
CREATE  INDEX `ndx_fk_av_c`  ON `dev5`.`accession_voucher` (`cooperator_id`);

select 'creating index ndx_fk_av_created for table accession_voucher ...' as Action;
CREATE  INDEX `ndx_fk_av_created`  ON `dev5`.`accession_voucher` (`created_by`);

select 'creating index ndx_fk_av_modified for table accession_voucher ...' as Action;
CREATE  INDEX `ndx_fk_av_modified`  ON `dev5`.`accession_voucher` (`modified_by`);

select 'creating index ndx_fk_av_owned for table accession_voucher ...' as Action;
CREATE  INDEX `ndx_fk_av_owned`  ON `dev5`.`accession_voucher` (`owned_by`);

/************ 4 Index Definitions for accession_voucher_image *************/
select 'creating index ndx_fk_avi_av for table accession_voucher_image ...' as Action;
CREATE  INDEX `ndx_fk_avi_av`  ON `dev5`.`accession_voucher_image` (`accession_voucher_id`);

select 'creating index ndx_fk_img_created for table accession_voucher_image ...' as Action;
CREATE  INDEX `ndx_fk_img_created`  ON `dev5`.`accession_voucher_image` (`created_by`);

select 'creating index ndx_fk_img_modified for table accession_voucher_image ...' as Action;
CREATE  INDEX `ndx_fk_img_modified`  ON `dev5`.`accession_voucher_image` (`modified_by`);

select 'creating index ndx_fk_img_owned for table accession_voucher_image ...' as Action;
CREATE  INDEX `ndx_fk_img_owned`  ON `dev5`.`accession_voucher_image` (`owned_by`);

/************ 4 Index Definitions for app_resource *************/
select 'creating index ndx_fk_are_sl for table app_resource ...' as Action;
CREATE  INDEX `ndx_fk_are_sl`  ON `dev5`.`app_resource` (`sec_lang_id`);

select 'creating index ndx_fk_are_created for table app_resource ...' as Action;
CREATE  INDEX `ndx_fk_are_created`  ON `dev5`.`app_resource` (`created_by`);

select 'creating index ndx_fk_are_modified for table app_resource ...' as Action;
CREATE  INDEX `ndx_fk_are_modified`  ON `dev5`.`app_resource` (`modified_by`);

select 'creating index ndx_fk_are_owned for table app_resource ...' as Action;
CREATE  INDEX `ndx_fk_are_owned`  ON `dev5`.`app_resource` (`owned_by`);

/************ 6 Index Definitions for app_user_item_list *************/
select 'creating index ndx_fk_auil_c for table app_user_item_list ...' as Action;
CREATE  INDEX `ndx_fk_auil_c`  ON `dev5`.`app_user_item_list` (`cooperator_id`);

select 'creating index ndx_uil_group for table app_user_item_list ...' as Action;
CREATE  INDEX `ndx_uil_group`  ON `dev5`.`app_user_item_list` (`cooperator_id`, `list_name`);

select 'creating index ndx_uil_tab for table app_user_item_list ...' as Action;
CREATE  INDEX `ndx_uil_tab`  ON `dev5`.`app_user_item_list` (`cooperator_id`, `tab_name`, `list_name`);

select 'creating index ndx_fk_auil_created for table app_user_item_list ...' as Action;
CREATE  INDEX `ndx_fk_auil_created`  ON `dev5`.`app_user_item_list` (`created_by`);

select 'creating index ndx_fk_auil_modified for table app_user_item_list ...' as Action;
CREATE  INDEX `ndx_fk_auil_modified`  ON `dev5`.`app_user_item_list` (`modified_by`);

select 'creating index ndx_fk_auil_owned for table app_user_item_list ...' as Action;
CREATE  INDEX `ndx_fk_auil_owned`  ON `dev5`.`app_user_item_list` (`owned_by`);

/************ 4 Index Definitions for code_group *************/
select 'creating index ndx_uniq_cdgrp for table code_group ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_cdgrp`  ON `dev5`.`code_group` (`name`);

select 'creating index ndx_fk_cdgrp_created for table code_group ...' as Action;
CREATE  INDEX `ndx_fk_cdgrp_created`  ON `dev5`.`code_group` (`created_by`);

select 'creating index ndx_fk_cdgrp_modified for table code_group ...' as Action;
CREATE  INDEX `ndx_fk_cdgrp_modified`  ON `dev5`.`code_group` (`modified_by`);

select 'creating index ndx_fk_cdgrp_owned for table code_group ...' as Action;
CREATE  INDEX `ndx_fk_cdgrp_owned`  ON `dev5`.`code_group` (`owned_by`);

/************ 4 Index Definitions for code_rule *************/
select 'creating index ndx_fk_cdrule_cv for table code_rule ...' as Action;
CREATE  INDEX `ndx_fk_cdrule_cv`  ON `dev5`.`code_rule` (`code_value_id`);

select 'creating index ndx_fk_cdrule_created for table code_rule ...' as Action;
CREATE  INDEX `ndx_fk_cdrule_created`  ON `dev5`.`code_rule` (`created_by`);

select 'creating index ndx_fk_cdrule_modified for table code_rule ...' as Action;
CREATE  INDEX `ndx_fk_cdrule_modified`  ON `dev5`.`code_rule` (`modified_by`);

select 'creating index ndx_fk_cdrule_owned for table code_rule ...' as Action;
CREATE  INDEX `ndx_fk_cdrule_owned`  ON `dev5`.`code_rule` (`owned_by`);

/************ 3 Index Definitions for code_value *************/
select 'creating index ndx_fk_cdval_created for table code_value ...' as Action;
CREATE  INDEX `ndx_fk_cdval_created`  ON `dev5`.`code_value` (`created_by`);

select 'creating index ndx_fk_cdval_modified for table code_value ...' as Action;
CREATE  INDEX `ndx_fk_cdval_modified`  ON `dev5`.`code_value` (`modified_by`);

select 'creating index ndx_fk_cdval_owned for table code_value ...' as Action;
CREATE  INDEX `ndx_fk_cdval_owned`  ON `dev5`.`code_value` (`owned_by`);

/************ 5 Index Definitions for code_value_friendly *************/
select 'creating index ndx_fk_cvf_cv for table code_value_friendly ...' as Action;
CREATE  INDEX `ndx_fk_cvf_cv`  ON `dev5`.`code_value_friendly` (`code_value_id`);

select 'creating index ndx_fk_cvf_sl for table code_value_friendly ...' as Action;
CREATE  INDEX `ndx_fk_cvf_sl`  ON `dev5`.`code_value_friendly` (`sec_lang_id`);

select 'creating index ndx_fk_tcf_created for table code_value_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_created`  ON `dev5`.`code_value_friendly` (`created_by`);

select 'creating index ndx_fk_tcf_modified for table code_value_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_modified`  ON `dev5`.`code_value_friendly` (`modified_by`);

select 'creating index ndx_fk_tcf_owned for table code_value_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_owned`  ON `dev5`.`code_value_friendly` (`owned_by`);

/************ 8 Index Definitions for cooperator *************/
select 'creating index ndx_fk_c_cur_c for table cooperator ...' as Action;
CREATE  INDEX `ndx_fk_c_cur_c`  ON `dev5`.`cooperator` (`current_cooperator_id`);

select 'creating index ndx_co_full_name for table cooperator ...' as Action;
CREATE  INDEX `ndx_co_full_name`  ON `dev5`.`cooperator` (`last_name`, `first_name`);

select 'creating index ndx_co_org_code for table cooperator ...' as Action;
CREATE  INDEX `ndx_co_org_code`  ON `dev5`.`cooperator` (`organization_code`);

select 'creating index ndx_uniq_co for table cooperator ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_co`  ON `dev5`.`cooperator` (`address_line1`, `admin_1`, `geography_id`, `full_name`);

select 'creating index ndx_fk_c_sl for table cooperator ...' as Action;
CREATE  INDEX `ndx_fk_c_sl`  ON `dev5`.`cooperator` (`sec_lang_id`);

select 'creating index ndx_fk_c_created for table cooperator ...' as Action;
CREATE  INDEX `ndx_fk_c_created`  ON `dev5`.`cooperator` (`created_by`);

select 'creating index ndx_fk_c_modified for table cooperator ...' as Action;
CREATE  INDEX `ndx_fk_c_modified`  ON `dev5`.`cooperator` (`modified_by`);

select 'creating index ndx_fk_c_owned for table cooperator ...' as Action;
CREATE  INDEX `ndx_fk_c_owned`  ON `dev5`.`cooperator` (`owned_by`);

/************ 4 Index Definitions for cooperator_group *************/
select 'creating index ndx_uniq_cg for table cooperator_group ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_cg`  ON `dev5`.`cooperator_group` (`name`);

select 'creating index ndx_fk_cg_created for table cooperator_group ...' as Action;
CREATE  INDEX `ndx_fk_cg_created`  ON `dev5`.`cooperator_group` (`created_by`);

select 'creating index ndx_fk_cg_modified for table cooperator_group ...' as Action;
CREATE  INDEX `ndx_fk_cg_modified`  ON `dev5`.`cooperator_group` (`modified_by`);

select 'creating index ndx_fk_cg_owned for table cooperator_group ...' as Action;
CREATE  INDEX `ndx_fk_cg_owned`  ON `dev5`.`cooperator_group` (`owned_by`);

/************ 5 Index Definitions for cooperator_member *************/
select 'creating index ndx_fk_cm_c for table cooperator_member ...' as Action;
CREATE  INDEX `ndx_fk_cm_c`  ON `dev5`.`cooperator_member` (`cooperator_id`);

select 'creating index ndx_fk_cm_cg for table cooperator_member ...' as Action;
CREATE  INDEX `ndx_fk_cm_cg`  ON `dev5`.`cooperator_member` (`cooperator_group_id`);

select 'creating index ndx_fk_cm_created for table cooperator_member ...' as Action;
CREATE  INDEX `ndx_fk_cm_created`  ON `dev5`.`cooperator_member` (`created_by`);

select 'creating index ndx_fk_cm_modified for table cooperator_member ...' as Action;
CREATE  INDEX `ndx_fk_cm_modified`  ON `dev5`.`cooperator_member` (`modified_by`);

select 'creating index ndx_fk_cm_owned for table cooperator_member ...' as Action;
CREATE  INDEX `ndx_fk_cm_owned`  ON `dev5`.`cooperator_member` (`owned_by`);

/************ 4 Index Definitions for crop *************/
select 'creating index ndx_cr_name for table crop ...' as Action;
CREATE UNIQUE INDEX `ndx_cr_name`  ON `dev5`.`crop` (`name`);

select 'creating index ndx_fk_crop_created for table crop ...' as Action;
CREATE  INDEX `ndx_fk_crop_created`  ON `dev5`.`crop` (`created_by`);

select 'creating index ndx_fk_crop_modified for table crop ...' as Action;
CREATE  INDEX `ndx_fk_crop_modified`  ON `dev5`.`crop` (`modified_by`);

select 'creating index ndx_fk_crop_owned for table crop ...' as Action;
CREATE  INDEX `ndx_fk_crop_owned`  ON `dev5`.`crop` (`owned_by`);

/************ 5 Index Definitions for evaluation *************/
select 'creating index ndx_uniq_ev for table evaluation ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ev`  ON `dev5`.`evaluation` (`name`);

select 'creating index ndx_fk_e_g for table evaluation ...' as Action;
CREATE  INDEX `ndx_fk_e_g`  ON `dev5`.`evaluation` (`geography_id`);

select 'creating index ndx_fk_e_created for table evaluation ...' as Action;
CREATE  INDEX `ndx_fk_e_created`  ON `dev5`.`evaluation` (`created_by`);

select 'creating index ndx_fk_e_modified for table evaluation ...' as Action;
CREATE  INDEX `ndx_fk_e_modified`  ON `dev5`.`evaluation` (`modified_by`);

select 'creating index ndx_fk_e_owned for table evaluation ...' as Action;
CREATE  INDEX `ndx_fk_e_owned`  ON `dev5`.`evaluation` (`owned_by`);

/************ 5 Index Definitions for evaluation_citation *************/
select 'creating index ndx_fk_ec_e for table evaluation_citation ...' as Action;
CREATE  INDEX `ndx_fk_ec_e`  ON `dev5`.`evaluation_citation` (`evaluation_id`);

select 'creating index ndx_fk_ec_l for table evaluation_citation ...' as Action;
CREATE  INDEX `ndx_fk_ec_l`  ON `dev5`.`evaluation_citation` (`literature_id`);

select 'creating index ndx_fk_ec_created for table evaluation_citation ...' as Action;
CREATE  INDEX `ndx_fk_ec_created`  ON `dev5`.`evaluation_citation` (`created_by`);

select 'creating index ndx_fk_ec_modified for table evaluation_citation ...' as Action;
CREATE  INDEX `ndx_fk_ec_modified`  ON `dev5`.`evaluation_citation` (`modified_by`);

select 'creating index ndx_fk_ec_owned for table evaluation_citation ...' as Action;
CREATE  INDEX `ndx_fk_ec_owned`  ON `dev5`.`evaluation_citation` (`owned_by`);

/************ 5 Index Definitions for evaluation_member *************/
select 'creating index ndx_fk_em_c for table evaluation_member ...' as Action;
CREATE  INDEX `ndx_fk_em_c`  ON `dev5`.`evaluation_member` (`cooperator_id`);

select 'creating index ndx_fk_em_e for table evaluation_member ...' as Action;
CREATE  INDEX `ndx_fk_em_e`  ON `dev5`.`evaluation_member` (`evaluation_id`);

select 'creating index ndx_fk_em_created for table evaluation_member ...' as Action;
CREATE  INDEX `ndx_fk_em_created`  ON `dev5`.`evaluation_member` (`created_by`);

select 'creating index ndx_fk_em_modified for table evaluation_member ...' as Action;
CREATE  INDEX `ndx_fk_em_modified`  ON `dev5`.`evaluation_member` (`modified_by`);

select 'creating index ndx_fk_em_owned for table evaluation_member ...' as Action;
CREATE  INDEX `ndx_fk_em_owned`  ON `dev5`.`evaluation_member` (`owned_by`);

/************ 5 Index Definitions for family *************/
select 'creating index ndx_fk_f_cur_f for table family ...' as Action;
CREATE  INDEX `ndx_fk_f_cur_f`  ON `dev5`.`family` (`current_family_id`);

select 'creating index ndx_uniq_fa for table family ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_fa`  ON `dev5`.`family` (`faimly_name`, `author_name`, `subfamily`, `tribe`, `subtribe`);

select 'creating index ndx_fk_f_created for table family ...' as Action;
CREATE  INDEX `ndx_fk_f_created`  ON `dev5`.`family` (`created_by`);

select 'creating index ndx_fk_f_modified for table family ...' as Action;
CREATE  INDEX `ndx_fk_f_modified`  ON `dev5`.`family` (`modified_by`);

select 'creating index ndx_fk_f_owned for table family ...' as Action;
CREATE  INDEX `ndx_fk_f_owned`  ON `dev5`.`family` (`owned_by`);

/************ 6 Index Definitions for genomic_annotation *************/
select 'creating index ndx_fk_ga_gm for table genomic_annotation ...' as Action;
CREATE  INDEX `ndx_fk_ga_gm`  ON `dev5`.`genomic_annotation` (`marker_id`);

select 'creating index ndx_uniq_ga for table genomic_annotation ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ga`  ON `dev5`.`genomic_annotation` (`marker_id`, `evaluation_id`);

select 'creating index ndx_fk_ga_e for table genomic_annotation ...' as Action;
CREATE  INDEX `ndx_fk_ga_e`  ON `dev5`.`genomic_annotation` (`evaluation_id`);

select 'creating index ndx_fk_ga_created for table genomic_annotation ...' as Action;
CREATE  INDEX `ndx_fk_ga_created`  ON `dev5`.`genomic_annotation` (`created_by`);

select 'creating index ndx_fk_ga_modified for table genomic_annotation ...' as Action;
CREATE  INDEX `ndx_fk_ga_modified`  ON `dev5`.`genomic_annotation` (`modified_by`);

select 'creating index ndx_fk_ga_owned for table genomic_annotation ...' as Action;
CREATE  INDEX `ndx_fk_ga_owned`  ON `dev5`.`genomic_annotation` (`owned_by`);

/************ 5 Index Definitions for genomic_marker *************/
select 'creating index ndx_fk_gm_crop for table genomic_marker ...' as Action;
CREATE  INDEX `ndx_fk_gm_crop`  ON `dev5`.`genomic_marker` (`crop_id`);

select 'creating index ndx_ma_crop for table genomic_marker ...' as Action;
CREATE UNIQUE INDEX `ndx_ma_crop`  ON `dev5`.`genomic_marker` (`crop_id`, `name`);

select 'creating index ndx_fk_gm_created for table genomic_marker ...' as Action;
CREATE  INDEX `ndx_fk_gm_created`  ON `dev5`.`genomic_marker` (`created_by`);

select 'creating index ndx_fk_gm_modified for table genomic_marker ...' as Action;
CREATE  INDEX `ndx_fk_gm_modified`  ON `dev5`.`genomic_marker` (`modified_by`);

select 'creating index ndx_fk_gm_owned for table genomic_marker ...' as Action;
CREATE  INDEX `ndx_fk_gm_owned`  ON `dev5`.`genomic_marker` (`owned_by`);

/************ 5 Index Definitions for genomic_marker_citation *************/
select 'creating index ndx_fk_gmc_gm for table genomic_marker_citation ...' as Action;
CREATE  INDEX `ndx_fk_gmc_gm`  ON `dev5`.`genomic_marker_citation` (`genomic_marker_id`);

select 'creating index ndx_fk_gmc_l for table genomic_marker_citation ...' as Action;
CREATE  INDEX `ndx_fk_gmc_l`  ON `dev5`.`genomic_marker_citation` (`literature_id`);

select 'creating index ndx_fk_gmc_created for table genomic_marker_citation ...' as Action;
CREATE  INDEX `ndx_fk_gmc_created`  ON `dev5`.`genomic_marker_citation` (`created_by`);

select 'creating index ndx_fk_gmc_modified for table genomic_marker_citation ...' as Action;
CREATE  INDEX `ndx_fk_gmc_modified`  ON `dev5`.`genomic_marker_citation` (`modified_by`);

select 'creating index ndx_fk_gmc_owned for table genomic_marker_citation ...' as Action;
CREATE  INDEX `ndx_fk_gmc_owned`  ON `dev5`.`genomic_marker_citation` (`owned_by`);

/************ 6 Index Definitions for genomic_observation *************/
select 'creating index ndx_fk_go_ga for table genomic_observation ...' as Action;
CREATE  INDEX `ndx_fk_go_ga`  ON `dev5`.`genomic_observation` (`genomic_annotation_id`);

select 'creating index ndx_uniq_go for table genomic_observation ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_go`  ON `dev5`.`genomic_observation` (`genomic_annotation_id`, `inventory_id`, `individual`);

select 'creating index ndx_fk_go_i for table genomic_observation ...' as Action;
CREATE  INDEX `ndx_fk_go_i`  ON `dev5`.`genomic_observation` (`inventory_id`);

select 'creating index ndx_fk_go_created for table genomic_observation ...' as Action;
CREATE  INDEX `ndx_fk_go_created`  ON `dev5`.`genomic_observation` (`created_by`);

select 'creating index ndx_fk_go_modified for table genomic_observation ...' as Action;
CREATE  INDEX `ndx_fk_go_modified`  ON `dev5`.`genomic_observation` (`modified_by`);

select 'creating index ndx_fk_go_owned for table genomic_observation ...' as Action;
CREATE  INDEX `ndx_fk_go_owned`  ON `dev5`.`genomic_observation` (`owned_by`);

/************ 7 Index Definitions for genus *************/
select 'creating index ndx_fk_gen_cur_gen for table genus ...' as Action;
CREATE  INDEX `ndx_fk_gen_cur_gen`  ON `dev5`.`genus` (`current_genus_id`);

select 'creating index ndx_uniq_gen for table genus ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_gen`  ON `dev5`.`genus` (`genus_name`, `genus_authority`, `subgenus_name`, `section_name`, `series_name`, `subseries_name`, `subsection_name`);

select 'creating index ndx_fk_gen_f for table genus ...' as Action;
CREATE  INDEX `ndx_fk_gen_f`  ON `dev5`.`genus` (`family_id`);

select 'creating index ndx_gen_name for table genus ...' as Action;
CREATE  INDEX `ndx_gen_name`  ON `dev5`.`genus` (`common_name`);

select 'creating index ndx_fk_gen_created for table genus ...' as Action;
CREATE  INDEX `ndx_fk_gen_created`  ON `dev5`.`genus` (`created_by`);

select 'creating index ndx_fk_gen_modified for table genus ...' as Action;
CREATE  INDEX `ndx_fk_gen_modified`  ON `dev5`.`genus` (`modified_by`);

select 'creating index ndx_fk_gen_owned for table genus ...' as Action;
CREATE  INDEX `ndx_fk_gen_owned`  ON `dev5`.`genus` (`owned_by`);

/************ 5 Index Definitions for genus_citation *************/
select 'creating index ndx_fk_gc_gen for table genus_citation ...' as Action;
CREATE  INDEX `ndx_fk_gc_gen`  ON `dev5`.`genus_citation` (`genus_id`);

select 'creating index ndx_fk_gc_l for table genus_citation ...' as Action;
CREATE  INDEX `ndx_fk_gc_l`  ON `dev5`.`genus_citation` (`literature_id`);

select 'creating index ndx_fk_gc_created for table genus_citation ...' as Action;
CREATE  INDEX `ndx_fk_gc_created`  ON `dev5`.`genus_citation` (`created_by`);

select 'creating index ndx_fk_gc_modified for table genus_citation ...' as Action;
CREATE  INDEX `ndx_fk_gc_modified`  ON `dev5`.`genus_citation` (`modified_by`);

select 'creating index ndx_fk_gc_owned for table genus_citation ...' as Action;
CREATE  INDEX `ndx_fk_gc_owned`  ON `dev5`.`genus_citation` (`owned_by`);

/************ 4 Index Definitions for genus_type *************/
select 'creating index ndx_fk_gt_f for table genus_type ...' as Action;
CREATE  INDEX `ndx_fk_gt_f`  ON `dev5`.`genus_type` (`family_id`);

select 'creating index ndx_fk_gt_created for table genus_type ...' as Action;
CREATE  INDEX `ndx_fk_gt_created`  ON `dev5`.`genus_type` (`created_by`);

select 'creating index ndx_fk_gt_modified for table genus_type ...' as Action;
CREATE  INDEX `ndx_fk_gt_modified`  ON `dev5`.`genus_type` (`modified_by`);

select 'creating index ndx_fk_gt_owned for table genus_type ...' as Action;
CREATE  INDEX `ndx_fk_gt_owned`  ON `dev5`.`genus_type` (`owned_by`);

/************ 10 Index Definitions for geography *************/
select 'creating index ndx_fk_g_cur_g for table geography ...' as Action;
CREATE  INDEX `ndx_fk_g_cur_g`  ON `dev5`.`geography` (`current_geography_id`);

select 'creating index ndx_uniq_ge_name for table geography ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ge_name`  ON `dev5`.`geography` (`country_name`, `state_name`);

select 'creating index ndx_ge_state for table geography ...' as Action;
CREATE  INDEX `ndx_ge_state`  ON `dev5`.`geography` (`state_name`);

select 'creating index ndx_ge_full_state for table geography ...' as Action;
CREATE  INDEX `ndx_ge_full_state`  ON `dev5`.`geography` (`country_iso_full_name`, `state_full_name`);

select 'creating index ndx_uniq_ge_code for table geography ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ge_code`  ON `dev5`.`geography` (`iso_3_char_country_code`, `state_code`);

select 'creating index ndx_ge_state_code for table geography ...' as Action;
CREATE  INDEX `ndx_ge_state_code`  ON `dev5`.`geography` (`state_code`);

select 'creating index ndx_fk_g_re for table geography ...' as Action;
CREATE  INDEX `ndx_fk_g_re`  ON `dev5`.`geography` (`region_id`);

select 'creating index ndx_fk_g_created for table geography ...' as Action;
CREATE  INDEX `ndx_fk_g_created`  ON `dev5`.`geography` (`created_by`);

select 'creating index ndx_fk_g_modified for table geography ...' as Action;
CREATE  INDEX `ndx_fk_g_modified`  ON `dev5`.`geography` (`modified_by`);

select 'creating index ndx_fk_g_owned for table geography ...' as Action;
CREATE  INDEX `ndx_fk_g_owned`  ON `dev5`.`geography` (`owned_by`);

/************ 12 Index Definitions for inventory *************/
select 'creating index ndx_uniq_inv for table inventory ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_inv`  ON `dev5`.`inventory` (`inventory_prefix`, `inventory_number`, `inventory_suffix`, `inventory_type_code`);

select 'creating index ndx_inv_prefix for table inventory ...' as Action;
CREATE  INDEX `ndx_inv_prefix`  ON `dev5`.`inventory` (`inventory_prefix`);

select 'creating index ndx_inv_number for table inventory ...' as Action;
CREATE  INDEX `ndx_inv_number`  ON `dev5`.`inventory` (`inventory_number`);

select 'creating index ndx_fk_i_im for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_im`  ON `dev5`.`inventory` (`inventory_maintenance_id`);

select 'creating index ndx_inv_location for table inventory ...' as Action;
CREATE  INDEX `ndx_inv_location`  ON `dev5`.`inventory` (`site_code`, `location_section_1`, `location_section_2`, `location_section_3`, `location_section_4`);

select 'creating index ndx_fk_i_a for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_a`  ON `dev5`.`inventory` (`accession_id`);

select 'creating index ndx_fk_i_parent_i for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_parent_i`  ON `dev5`.`inventory` (`parent_inventory_id`);

select 'creating index ndx_fk_i_c for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_c`  ON `dev5`.`inventory` (`cooperator_id`);

select 'creating index ndx_fk_i_backup_i for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_backup_i`  ON `dev5`.`inventory` (`backup_inventory_id`);

select 'creating index ndx_fk_i_created for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_created`  ON `dev5`.`inventory` (`created_by`);

select 'creating index ndx_fk_i_modified for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_modified`  ON `dev5`.`inventory` (`modified_by`);

select 'creating index ndx_fk_i_owned for table inventory ...' as Action;
CREATE  INDEX `ndx_fk_i_owned`  ON `dev5`.`inventory` (`owned_by`);

/************ 6 Index Definitions for inventory_action *************/
select 'creating index ndx_fk_ia_i for table inventory_action ...' as Action;
CREATE  INDEX `ndx_fk_ia_i`  ON `dev5`.`inventory_action` (`inventory_id`);

select 'creating index ndx_fk_ia_c for table inventory_action ...' as Action;
CREATE  INDEX `ndx_fk_ia_c`  ON `dev5`.`inventory_action` (`cooperator_id`);

select 'creating index ndx_fk_ia_e for table inventory_action ...' as Action;
CREATE  INDEX `ndx_fk_ia_e`  ON `dev5`.`inventory_action` (`evaluation_id`);

select 'creating index ndx_fk_ia_created for table inventory_action ...' as Action;
CREATE  INDEX `ndx_fk_ia_created`  ON `dev5`.`inventory_action` (`created_by`);

select 'creating index ndx_fk_ia_modified for table inventory_action ...' as Action;
CREATE  INDEX `ndx_fk_ia_modified`  ON `dev5`.`inventory_action` (`modified_by`);

select 'creating index ndx_fk_ia_owned for table inventory_action ...' as Action;
CREATE  INDEX `ndx_fk_ia_owned`  ON `dev5`.`inventory_action` (`owned_by`);

/************ 3 Index Definitions for inventory_group *************/
select 'creating index ndx_fk_ig_created for table inventory_group ...' as Action;
CREATE  INDEX `ndx_fk_ig_created`  ON `dev5`.`inventory_group` (`created_by`);

select 'creating index ndx_fk_ig_modified for table inventory_group ...' as Action;
CREATE  INDEX `ndx_fk_ig_modified`  ON `dev5`.`inventory_group` (`modified_by`);

select 'creating index ndx_fk_ig_owned for table inventory_group ...' as Action;
CREATE  INDEX `ndx_fk_ig_owned`  ON `dev5`.`inventory_group` (`owned_by`);

/************ 6 Index Definitions for inventory_group_maintenance *************/
select 'creating index ndx_fk_igm_i for table inventory_group_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_igm_i`  ON `dev5`.`inventory_group_maintenance` (`inventory_id`);

select 'creating index ndx_uniq_igm for table inventory_group_maintenance ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_igm`  ON `dev5`.`inventory_group_maintenance` (`inventory_id`, `inventory_group_id`, `site_code`);

select 'creating index ndx_fk_igm_ig for table inventory_group_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_igm_ig`  ON `dev5`.`inventory_group_maintenance` (`inventory_group_id`);

select 'creating index ndx_fk_igm_created for table inventory_group_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_igm_created`  ON `dev5`.`inventory_group_maintenance` (`created_by`);

select 'creating index ndx_fk_igm_modified for table inventory_group_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_igm_modified`  ON `dev5`.`inventory_group_maintenance` (`modified_by`);

select 'creating index ndx_fk_igm_owned for table inventory_group_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_igm_owned`  ON `dev5`.`inventory_group_maintenance` (`owned_by`);

/************ 4 Index Definitions for inventory_maintenance *************/
select 'creating index ndx_fk_im_co for table inventory_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_im_co`  ON `dev5`.`inventory_maintenance` (`cooperator_id`);

select 'creating index ndx_fk_im_created for table inventory_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_im_created`  ON `dev5`.`inventory_maintenance` (`created_by`);

select 'creating index ndx_fk_im_modified for table inventory_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_im_modified`  ON `dev5`.`inventory_maintenance` (`modified_by`);

select 'creating index ndx_fk_im_owned for table inventory_maintenance ...' as Action;
CREATE  INDEX `ndx_fk_im_owned`  ON `dev5`.`inventory_maintenance` (`owned_by`);

/************ 6 Index Definitions for inventory_pathogen_test *************/
select 'creating index ndx_fk_ipt_i for table inventory_pathogen_test ...' as Action;
CREATE  INDEX `ndx_fk_ipt_i`  ON `dev5`.`inventory_pathogen_test` (`inventory_id`);

select 'creating index ndx_uniq_pt for table inventory_pathogen_test ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_pt`  ON `dev5`.`inventory_pathogen_test` (`inventory_id`, `test_type`, `pathogen_code`, `finished_date`);

select 'creating index ndx_pt_test for table inventory_pathogen_test ...' as Action;
CREATE  INDEX `ndx_pt_test`  ON `dev5`.`inventory_pathogen_test` (`test_type`, `pathogen_code`);

select 'creating index ndx_fk_ipt_created for table inventory_pathogen_test ...' as Action;
CREATE  INDEX `ndx_fk_ipt_created`  ON `dev5`.`inventory_pathogen_test` (`created_by`);

select 'creating index ndx_fk_ipt_modified for table inventory_pathogen_test ...' as Action;
CREATE  INDEX `ndx_fk_ipt_modified`  ON `dev5`.`inventory_pathogen_test` (`modified_by`);

select 'creating index ndx_fk_ipt_owned for table inventory_pathogen_test ...' as Action;
CREATE  INDEX `ndx_fk_ipt_owned`  ON `dev5`.`inventory_pathogen_test` (`owned_by`);

/************ 5 Index Definitions for inventory_viability *************/
select 'creating index ndx_fk_iv_i for table inventory_viability ...' as Action;
CREATE  INDEX `ndx_fk_iv_i`  ON `dev5`.`inventory_viability` (`inventory_id`);

select 'creating index ndx_fk_iv_e for table inventory_viability ...' as Action;
CREATE  INDEX `ndx_fk_iv_e`  ON `dev5`.`inventory_viability` (`evaluation_id`);

select 'creating index ndx_fk_iv_created for table inventory_viability ...' as Action;
CREATE  INDEX `ndx_fk_iv_created`  ON `dev5`.`inventory_viability` (`created_by`);

select 'creating index ndx_fk_iv_modified for table inventory_viability ...' as Action;
CREATE  INDEX `ndx_fk_iv_modified`  ON `dev5`.`inventory_viability` (`modified_by`);

select 'creating index ndx_fk_iv_owned for table inventory_viability ...' as Action;
CREATE  INDEX `ndx_fk_iv_owned`  ON `dev5`.`inventory_viability` (`owned_by`);

/************ 3 Index Definitions for literature *************/
select 'creating index ndx_fk_l_created for table literature ...' as Action;
CREATE  INDEX `ndx_fk_l_created`  ON `dev5`.`literature` (`created_by`);

select 'creating index ndx_fk_l_modified for table literature ...' as Action;
CREATE  INDEX `ndx_fk_l_modified`  ON `dev5`.`literature` (`modified_by`);

select 'creating index ndx_fk_l_owned for table literature ...' as Action;
CREATE  INDEX `ndx_fk_l_owned`  ON `dev5`.`literature` (`owned_by`);

/************ 11 Index Definitions for order_entry *************/
select 'creating index ndx_fk_oe_original_oe for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_original_oe`  ON `dev5`.`order_entry` (`original_order_entry_id`);

select 'creating index ndx_oe_local for table order_entry ...' as Action;
CREATE  INDEX `ndx_oe_local`  ON `dev5`.`order_entry` (`site_code`, `local_number`);

select 'creating index ndx_fk_oe_source_c for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_source_c`  ON `dev5`.`order_entry` (`source_cooperator_id`);

select 'creating index ndx_oe_source for table order_entry ...' as Action;
CREATE  INDEX `ndx_oe_source`  ON `dev5`.`order_entry` (`source_cooperator_id`);

select 'creating index ndx_fk_oe_requestor_c for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_requestor_c`  ON `dev5`.`order_entry` (`requestor_cooperator_id`);

select 'creating index ndx_fk_oe_ship_to_c for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_ship_to_c`  ON `dev5`.`order_entry` (`ship_to_cooperator_id`);

select 'creating index ndx_fk_oe_final_c for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_final_c`  ON `dev5`.`order_entry` (`final_recipient_cooperator_id`);

select 'creating index ndx_oe_obtained for table order_entry ...' as Action;
CREATE  INDEX `ndx_oe_obtained`  ON `dev5`.`order_entry` (`order_obtained_via`);

select 'creating index ndx_fk_oe_created for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_created`  ON `dev5`.`order_entry` (`created_by`);

select 'creating index ndx_fk_oe_modified for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_modified`  ON `dev5`.`order_entry` (`modified_by`);

select 'creating index ndx_fk_oe_owned for table order_entry ...' as Action;
CREATE  INDEX `ndx_fk_oe_owned`  ON `dev5`.`order_entry` (`owned_by`);

/************ 4 Index Definitions for order_entry_action *************/
select 'creating index ndx_fk_oea_oe for table order_entry_action ...' as Action;
CREATE  INDEX `ndx_fk_oea_oe`  ON `dev5`.`order_entry_action` (`order_entry_id`);

select 'creating index ndx_fk_oea_created for table order_entry_action ...' as Action;
CREATE  INDEX `ndx_fk_oea_created`  ON `dev5`.`order_entry_action` (`created_by`);

select 'creating index ndx_fk_oea_modified for table order_entry_action ...' as Action;
CREATE  INDEX `ndx_fk_oea_modified`  ON `dev5`.`order_entry_action` (`modified_by`);

select 'creating index ndx_fk_oea_owned for table order_entry_action ...' as Action;
CREATE  INDEX `ndx_fk_oea_owned`  ON `dev5`.`order_entry_action` (`owned_by`);

/************ 11 Index Definitions for order_entry_item *************/
select 'creating index ndx_fk_oei_oe for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_oe`  ON `dev5`.`order_entry_item` (`order_entry_id`);

select 'creating index ndx_uniq_oi for table order_entry_item ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_oi`  ON `dev5`.`order_entry_item` (`order_entry_id`, `item_sequence_number`);

select 'creating index ndx_oi_item for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_oi_item`  ON `dev5`.`order_entry_item` (`order_entry_id`, `item_name`);

select 'creating index ndx_oi_aio for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_oi_aio`  ON `dev5`.`order_entry_item` (`order_entry_id`, `inventory_id`, `accession_id`);

select 'creating index ndx_fk_oei_c for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_c`  ON `dev5`.`order_entry_item` (`cooperator_id`);

select 'creating index ndx_fk_oei_i for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_i`  ON `dev5`.`order_entry_item` (`inventory_id`);

select 'creating index ndx_fk_oei_a for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_a`  ON `dev5`.`order_entry_item` (`accession_id`);

select 'creating index ndx_fk_oei_t for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_t`  ON `dev5`.`order_entry_item` (`taxonomy_id`);

select 'creating index ndx_fk_oei_created for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_created`  ON `dev5`.`order_entry_item` (`created_by`);

select 'creating index ndx_fk_oei_modified for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_modified`  ON `dev5`.`order_entry_item` (`modified_by`);

select 'creating index ndx_fk_oei_owned for table order_entry_item ...' as Action;
CREATE  INDEX `ndx_fk_oei_owned`  ON `dev5`.`order_entry_item` (`owned_by`);

/************ 4 Index Definitions for plant_introduction *************/
select 'creating index ndx_pi_year for table plant_introduction ...' as Action;
CREATE  INDEX `ndx_pi_year`  ON `dev5`.`plant_introduction` (`plant_introduction_year_date`);

select 'creating index ndx_fk_pi_created for table plant_introduction ...' as Action;
CREATE  INDEX `ndx_fk_pi_created`  ON `dev5`.`plant_introduction` (`created_by`);

select 'creating index ndx_fk_pi_modified for table plant_introduction ...' as Action;
CREATE  INDEX `ndx_fk_pi_modified`  ON `dev5`.`plant_introduction` (`modified_by`);

select 'creating index ndx_fk_pi_owned for table plant_introduction ...' as Action;
CREATE  INDEX `ndx_fk_pi_owned`  ON `dev5`.`plant_introduction` (`owned_by`);

/************ 4 Index Definitions for region *************/
select 'creating index ndx_uniq_re for table region ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_re`  ON `dev5`.`region` (`continent`, `subcontinent`);

select 'creating index ndx_fk_r_created for table region ...' as Action;
CREATE  INDEX `ndx_fk_r_created`  ON `dev5`.`region` (`created_by`);

select 'creating index ndx_fk_r_modified for table region ...' as Action;
CREATE  INDEX `ndx_fk_r_modified`  ON `dev5`.`region` (`modified_by`);

select 'creating index ndx_fk_r_owned for table region ...' as Action;
CREATE  INDEX `ndx_fk_r_owned`  ON `dev5`.`region` (`owned_by`);

/************ 5 Index Definitions for sec_lang *************/
select 'creating index ndx_uniq_sl_code for table sec_lang ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_sl_code`  ON `dev5`.`sec_lang` (`iso_639_3_code`);

select 'creating index ndx_uniq_sl_tag for table sec_lang ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_sl_tag`  ON `dev5`.`sec_lang` (`ietf_tag`);

select 'creating index ndx_fk_sl_created for table sec_lang ...' as Action;
CREATE  INDEX `ndx_fk_sl_created`  ON `dev5`.`sec_lang` (`created_by`);

select 'creating index ndx_fk_sl_modified for table sec_lang ...' as Action;
CREATE  INDEX `ndx_fk_sl_modified`  ON `dev5`.`sec_lang` (`modified_by`);

select 'creating index ndx_fk_sl_owned for table sec_lang ...' as Action;
CREATE  INDEX `ndx_fk_sl_owned`  ON `dev5`.`sec_lang` (`owned_by`);

/************ 4 Index Definitions for sec_perm *************/
select 'creating index ndx_fk_sp_spt for table sec_perm ...' as Action;
CREATE  INDEX `ndx_fk_sp_spt`  ON `dev5`.`sec_perm` (`sec_perm_template_id`);

select 'creating index ndx_fk_sp_created for table sec_perm ...' as Action;
CREATE  INDEX `ndx_fk_sp_created`  ON `dev5`.`sec_perm` (`created_by`);

select 'creating index ndx_fk_sp_modified for table sec_perm ...' as Action;
CREATE  INDEX `ndx_fk_sp_modified`  ON `dev5`.`sec_perm` (`modified_by`);

select 'creating index ndx_fk_sp_owned for table sec_perm ...' as Action;
CREATE  INDEX `ndx_fk_sp_owned`  ON `dev5`.`sec_perm` (`owned_by`);

/************ 4 Index Definitions for sec_perm_field *************/
select 'creating index ndx_fk_spf_sp for table sec_perm_field ...' as Action;
CREATE  INDEX `ndx_fk_spf_sp`  ON `dev5`.`sec_perm_field` (`sec_perm_id`);

select 'creating index ndx_fk_spf_created for table sec_perm_field ...' as Action;
CREATE  INDEX `ndx_fk_spf_created`  ON `dev5`.`sec_perm_field` (`created_by`);

select 'creating index ndx_fk_spf_modified for table sec_perm_field ...' as Action;
CREATE  INDEX `ndx_fk_spf_modified`  ON `dev5`.`sec_perm_field` (`modified_by`);

select 'creating index ndx_fk_spf_owned for table sec_perm_field ...' as Action;
CREATE  INDEX `ndx_fk_spf_owned`  ON `dev5`.`sec_perm_field` (`owned_by`);

/************ 3 Index Definitions for sec_perm_template *************/
select 'creating index ndx_fk_spt_created for table sec_perm_template ...' as Action;
CREATE  INDEX `ndx_fk_spt_created`  ON `dev5`.`sec_perm_template` (`created_by`);

select 'creating index ndx_fk_spt_modified for table sec_perm_template ...' as Action;
CREATE  INDEX `ndx_fk_spt_modified`  ON `dev5`.`sec_perm_template` (`modified_by`);

select 'creating index ndx_fk_spt_owned for table sec_perm_template ...' as Action;
CREATE  INDEX `ndx_fk_spt_owned`  ON `dev5`.`sec_perm_template` (`owned_by`);

/************ 5 Index Definitions for sec_perm_template_map *************/
select 'creating index ndx_fk_sptm_spt for table sec_perm_template_map ...' as Action;
CREATE  INDEX `ndx_fk_sptm_spt`  ON `dev5`.`sec_perm_template_map` (`sec_perm_template_id`);

select 'creating index ndx_fk_sptm_sp for table sec_perm_template_map ...' as Action;
CREATE  INDEX `ndx_fk_sptm_sp`  ON `dev5`.`sec_perm_template_map` (`sec_perm_id`);

select 'creating index ndx_fk_sptm_created for table sec_perm_template_map ...' as Action;
CREATE  INDEX `ndx_fk_sptm_created`  ON `dev5`.`sec_perm_template_map` (`created_by`);

select 'creating index ndx_fk_sptm_modified for table sec_perm_template_map ...' as Action;
CREATE  INDEX `ndx_fk_sptm_modified`  ON `dev5`.`sec_perm_template_map` (`modified_by`);

select 'creating index ndx_fk_sptm_owned for table sec_perm_template_map ...' as Action;
CREATE  INDEX `ndx_fk_sptm_owned`  ON `dev5`.`sec_perm_template_map` (`owned_by`);

/************ 3 Index Definitions for sec_rs *************/
select 'creating index ndx_fk_sr_created for table sec_rs ...' as Action;
CREATE  INDEX `ndx_fk_sr_created`  ON `dev5`.`sec_rs` (`created_by`);

select 'creating index ndx_fk_sr_modified for table sec_rs ...' as Action;
CREATE  INDEX `ndx_fk_sr_modified`  ON `dev5`.`sec_rs` (`modified_by`);

select 'creating index ndx_fk_sr_owned for table sec_rs ...' as Action;
CREATE  INDEX `ndx_fk_sr_owned`  ON `dev5`.`sec_rs` (`owned_by`);

/************ 5 Index Definitions for sec_rs_field *************/
select 'creating index ndx_fk_srf_sr for table sec_rs_field ...' as Action;
CREATE  INDEX `ndx_fk_srf_sr`  ON `dev5`.`sec_rs_field` (`sec_rs_id`);

select 'creating index ndx_fk_srf_stf for table sec_rs_field ...' as Action;
CREATE  INDEX `ndx_fk_srf_stf`  ON `dev5`.`sec_rs_field` (`sec_table_field_id`);

select 'creating index ndx_fk_srf_created for table sec_rs_field ...' as Action;
CREATE  INDEX `ndx_fk_srf_created`  ON `dev5`.`sec_rs_field` (`created_by`);

select 'creating index ndx_fk_srf_modified for table sec_rs_field ...' as Action;
CREATE  INDEX `ndx_fk_srf_modified`  ON `dev5`.`sec_rs_field` (`modified_by`);

select 'creating index ndx_fk_srf_owned for table sec_rs_field ...' as Action;
CREATE  INDEX `ndx_fk_srf_owned`  ON `dev5`.`sec_rs_field` (`owned_by`);

/************ 5 Index Definitions for sec_rs_field_friendly *************/
select 'creating index ndx_fk_srff_srf for table sec_rs_field_friendly ...' as Action;
CREATE  INDEX `ndx_fk_srff_srf`  ON `dev5`.`sec_rs_field_friendly` (`sec_rs_field_id`);

select 'creating index ndx_fk_srff_sl for table sec_rs_field_friendly ...' as Action;
CREATE  INDEX `ndx_fk_srff_sl`  ON `dev5`.`sec_rs_field_friendly` (`sec_lang_id`);

select 'creating index ndx_fk_srff_created for table sec_rs_field_friendly ...' as Action;
CREATE  INDEX `ndx_fk_srff_created`  ON `dev5`.`sec_rs_field_friendly` (`created_by`);

select 'creating index ndx_fk_srff_modified for table sec_rs_field_friendly ...' as Action;
CREATE  INDEX `ndx_fk_srff_modified`  ON `dev5`.`sec_rs_field_friendly` (`modified_by`);

select 'creating index ndx_fk_srff_owned for table sec_rs_field_friendly ...' as Action;
CREATE  INDEX `ndx_fk_srff_owned`  ON `dev5`.`sec_rs_field_friendly` (`owned_by`);

/************ 4 Index Definitions for sec_rs_param *************/
select 'creating index ndx_fk_srp_sr for table sec_rs_param ...' as Action;
CREATE  INDEX `ndx_fk_srp_sr`  ON `dev5`.`sec_rs_param` (`sec_rs_id`);

select 'creating index ndx_fk_srp_created for table sec_rs_param ...' as Action;
CREATE  INDEX `ndx_fk_srp_created`  ON `dev5`.`sec_rs_param` (`created_by`);

select 'creating index ndx_fk_srp_modified for table sec_rs_param ...' as Action;
CREATE  INDEX `ndx_fk_srp_modified`  ON `dev5`.`sec_rs_param` (`modified_by`);

select 'creating index ndx_fk_srp_owned for table sec_rs_param ...' as Action;
CREATE  INDEX `ndx_fk_srp_owned`  ON `dev5`.`sec_rs_param` (`owned_by`);

/************ 3 Index Definitions for sec_table *************/
select 'creating index ndx_fk_st_created for table sec_table ...' as Action;
CREATE  INDEX `ndx_fk_st_created`  ON `dev5`.`sec_table` (`created_by`);

select 'creating index ndx_fk_st_modified for table sec_table ...' as Action;
CREATE  INDEX `ndx_fk_st_modified`  ON `dev5`.`sec_table` (`modified_by`);

select 'creating index ndx_fk_st_owned for table sec_table ...' as Action;
CREATE  INDEX `ndx_fk_st_owned`  ON `dev5`.`sec_table` (`owned_by`);

/************ 5 Index Definitions for sec_table_field *************/
select 'creating index ndx_fk_stf_st for table sec_table_field ...' as Action;
CREATE  INDEX `ndx_fk_stf_st`  ON `dev5`.`sec_table_field` (`sec_table_id`);

select 'creating index ndx_fk_stf_cgr for table sec_table_field ...' as Action;
CREATE  INDEX `ndx_fk_stf_cgr`  ON `dev5`.`sec_table_field` (`code_group_id`);

select 'creating index ndx_fk_stf_created for table sec_table_field ...' as Action;
CREATE  INDEX `ndx_fk_stf_created`  ON `dev5`.`sec_table_field` (`created_by`);

select 'creating index ndx_fk_stf_modified for table sec_table_field ...' as Action;
CREATE  INDEX `ndx_fk_stf_modified`  ON `dev5`.`sec_table_field` (`modified_by`);

select 'creating index ndx_fk_stf_owned for table sec_table_field ...' as Action;
CREATE  INDEX `ndx_fk_stf_owned`  ON `dev5`.`sec_table_field` (`owned_by`);

/************ 5 Index Definitions for sec_user *************/
select 'creating index ndx_uniq_su_name for table sec_user ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_su_name`  ON `dev5`.`sec_user` (`user_name`);

select 'creating index ndx_fk_su_co for table sec_user ...' as Action;
CREATE  INDEX `ndx_fk_su_co`  ON `dev5`.`sec_user` (`cooperator_id`);

select 'creating index ndx_fk_su_created for table sec_user ...' as Action;
CREATE  INDEX `ndx_fk_su_created`  ON `dev5`.`sec_user` (`created_by`);

select 'creating index ndx_fk_su_modified for table sec_user ...' as Action;
CREATE  INDEX `ndx_fk_su_modified`  ON `dev5`.`sec_user` (`modified_by`);

select 'creating index ndx_fk_su_owned for table sec_user ...' as Action;
CREATE  INDEX `ndx_fk_su_owned`  ON `dev5`.`sec_user` (`owned_by`);

/************ 4 Index Definitions for sec_user_gui_setting *************/
select 'creating index ndx_fk_sugs_co for table sec_user_gui_setting ...' as Action;
CREATE  INDEX `ndx_fk_sugs_co`  ON `dev5`.`sec_user_gui_setting` (`cooperator_id`);

select 'creating index ndx_fk_sugs_created for table sec_user_gui_setting ...' as Action;
CREATE  INDEX `ndx_fk_sugs_created`  ON `dev5`.`sec_user_gui_setting` (`created_by`);

select 'creating index ndx_fk_sugs_modified for table sec_user_gui_setting ...' as Action;
CREATE  INDEX `ndx_fk_sugs_modified`  ON `dev5`.`sec_user_gui_setting` (`modified_by`);

select 'creating index ndx_fk_sugs_owned for table sec_user_gui_setting ...' as Action;
CREATE  INDEX `ndx_fk_sugs_owned`  ON `dev5`.`sec_user_gui_setting` (`owned_by`);

/************ 5 Index Definitions for sec_user_perm *************/
select 'creating index ndx_fk_sup_su for table sec_user_perm ...' as Action;
CREATE  INDEX `ndx_fk_sup_su`  ON `dev5`.`sec_user_perm` (`sec_user_id`);

select 'creating index ndx_fk_sup_sp for table sec_user_perm ...' as Action;
CREATE  INDEX `ndx_fk_sup_sp`  ON `dev5`.`sec_user_perm` (`sec_perm_id`);

select 'creating index ndx_fk_sup_created for table sec_user_perm ...' as Action;
CREATE  INDEX `ndx_fk_sup_created`  ON `dev5`.`sec_user_perm` (`created_by`);

select 'creating index ndx_fk_sup_modified for table sec_user_perm ...' as Action;
CREATE  INDEX `ndx_fk_sup_modified`  ON `dev5`.`sec_user_perm` (`modified_by`);

select 'creating index ndx_fk_sup_owned for table sec_user_perm ...' as Action;
CREATE  INDEX `ndx_fk_sup_owned`  ON `dev5`.`sec_user_perm` (`owned_by`);

/************ 10 Index Definitions for taxonomy *************/
select 'creating index ndx_fk_t_cur_t for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_cur_t`  ON `dev5`.`taxonomy` (`current_taxonomy_id`);

select 'creating index ndx_fk_t_gen for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_gen`  ON `dev5`.`taxonomy` (`genus_id`);

select 'creating index ndx_fk_t_crop for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_crop`  ON `dev5`.`taxonomy` (`crop_id`);

select 'creating index ndx_ta_site1 for table taxonomy ...' as Action;
CREATE  INDEX `ndx_ta_site1`  ON `dev5`.`taxonomy` (`priority_site_1`);

select 'creating index ndx_ta_site2 for table taxonomy ...' as Action;
CREATE  INDEX `ndx_ta_site2`  ON `dev5`.`taxonomy` (`priority_site_2`);

select 'creating index ndx_fk_t_c for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_c`  ON `dev5`.`taxonomy` (`cooperator_id`);

select 'creating index ndx_uniq_ta for table taxonomy ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ta`  ON `dev5`.`taxonomy` (`name`, `name_authority`);

select 'creating index ndx_fk_t_created for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_created`  ON `dev5`.`taxonomy` (`created_by`);

select 'creating index ndx_fk_t_modified for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_modified`  ON `dev5`.`taxonomy` (`modified_by`);

select 'creating index ndx_fk_t_owned for table taxonomy ...' as Action;
CREATE  INDEX `ndx_fk_t_owned`  ON `dev5`.`taxonomy` (`owned_by`);

/************ 4 Index Definitions for taxonomy_author *************/
select 'creating index ndx_ta_name for table taxonomy_author ...' as Action;
CREATE  INDEX `ndx_ta_name`  ON `dev5`.`taxonomy_author` (`short_name_expanded_diacritic`);

select 'creating index ndx_fk_ta_created for table taxonomy_author ...' as Action;
CREATE  INDEX `ndx_fk_ta_created`  ON `dev5`.`taxonomy_author` (`created_by`);

select 'creating index ndx_fk_ta_modified for table taxonomy_author ...' as Action;
CREATE  INDEX `ndx_fk_ta_modified`  ON `dev5`.`taxonomy_author` (`modified_by`);

select 'creating index ndx_fk_ta_owned for table taxonomy_author ...' as Action;
CREATE  INDEX `ndx_fk_ta_owned`  ON `dev5`.`taxonomy_author` (`owned_by`);

/************ 5 Index Definitions for taxonomy_citation *************/
select 'creating index ndx_fk_tc_t for table taxonomy_citation ...' as Action;
CREATE  INDEX `ndx_fk_tc_t`  ON `dev5`.`taxonomy_citation` (`taxonomy_id`);

select 'creating index ndx_fk_tc_l for table taxonomy_citation ...' as Action;
CREATE  INDEX `ndx_fk_tc_l`  ON `dev5`.`taxonomy_citation` (`literature_id`);

select 'creating index ndx_fk_tc_created for table taxonomy_citation ...' as Action;
CREATE  INDEX `ndx_fk_tc_created`  ON `dev5`.`taxonomy_citation` (`created_by`);

select 'creating index ndx_fk_tc_modified for table taxonomy_citation ...' as Action;
CREATE  INDEX `ndx_fk_tc_modified`  ON `dev5`.`taxonomy_citation` (`modified_by`);

select 'creating index ndx_fk_tc_owned for table taxonomy_citation ...' as Action;
CREATE  INDEX `ndx_fk_tc_owned`  ON `dev5`.`taxonomy_citation` (`owned_by`);

/************ 6 Index Definitions for taxonomy_common_name *************/
select 'creating index ndx_fk_tcn_t for table taxonomy_common_name ...' as Action;
CREATE  INDEX `ndx_fk_tcn_t`  ON `dev5`.`taxonomy_common_name` (`taxonomy_id`);

select 'creating index ndx_cn_name for table taxonomy_common_name ...' as Action;
CREATE  INDEX `ndx_cn_name`  ON `dev5`.`taxonomy_common_name` (`name`);

select 'creating index ndx_cn_simplified_name for table taxonomy_common_name ...' as Action;
CREATE  INDEX `ndx_cn_simplified_name`  ON `dev5`.`taxonomy_common_name` (`simplified_name`);

select 'creating index ndx_fk_tcn_created for table taxonomy_common_name ...' as Action;
CREATE  INDEX `ndx_fk_tcn_created`  ON `dev5`.`taxonomy_common_name` (`created_by`);

select 'creating index ndx_fk_tcn_modified for table taxonomy_common_name ...' as Action;
CREATE  INDEX `ndx_fk_tcn_modified`  ON `dev5`.`taxonomy_common_name` (`modified_by`);

select 'creating index ndx_fk_tcn_owned for table taxonomy_common_name ...' as Action;
CREATE  INDEX `ndx_fk_tcn_owned`  ON `dev5`.`taxonomy_common_name` (`owned_by`);

/************ 5 Index Definitions for taxonomy_distribution *************/
select 'creating index ndx_fk_td_t for table taxonomy_distribution ...' as Action;
CREATE  INDEX `ndx_fk_td_t`  ON `dev5`.`taxonomy_distribution` (`taxonomy_id`);

select 'creating index ndx_fk_td_g for table taxonomy_distribution ...' as Action;
CREATE  INDEX `ndx_fk_td_g`  ON `dev5`.`taxonomy_distribution` (`geography_id`);

select 'creating index ndx_fk_td_created for table taxonomy_distribution ...' as Action;
CREATE  INDEX `ndx_fk_td_created`  ON `dev5`.`taxonomy_distribution` (`created_by`);

select 'creating index ndx_fk_td_modified for table taxonomy_distribution ...' as Action;
CREATE  INDEX `ndx_fk_td_modified`  ON `dev5`.`taxonomy_distribution` (`modified_by`);

select 'creating index ndx_fk_td_owned for table taxonomy_distribution ...' as Action;
CREATE  INDEX `ndx_fk_td_owned`  ON `dev5`.`taxonomy_distribution` (`owned_by`);

/************ 4 Index Definitions for taxonomy_germination_rule *************/
select 'creating index ndx_fk_tgr_t for table taxonomy_germination_rule ...' as Action;
CREATE  INDEX `ndx_fk_tgr_t`  ON `dev5`.`taxonomy_germination_rule` (`taxonomy_id`);

select 'creating index ndx_fk_tgr_created for table taxonomy_germination_rule ...' as Action;
CREATE  INDEX `ndx_fk_tgr_created`  ON `dev5`.`taxonomy_germination_rule` (`created_by`);

select 'creating index ndx_fk_tgr_modified for table taxonomy_germination_rule ...' as Action;
CREATE  INDEX `ndx_fk_tgr_modified`  ON `dev5`.`taxonomy_germination_rule` (`modified_by`);

select 'creating index ndx_fk_tgr_owned for table taxonomy_germination_rule ...' as Action;
CREATE  INDEX `ndx_fk_tgr_owned`  ON `dev5`.`taxonomy_germination_rule` (`owned_by`);

/************ 6 Index Definitions for taxonomy_url *************/
select 'creating index ndx_fk_tu_f for table taxonomy_url ...' as Action;
CREATE  INDEX `ndx_fk_tu_f`  ON `dev5`.`taxonomy_url` (`family_id`);

select 'creating index ndx_fk_tu_gen for table taxonomy_url ...' as Action;
CREATE  INDEX `ndx_fk_tu_gen`  ON `dev5`.`taxonomy_url` (`genus_id`);

select 'creating index ndx_fk_tu_t for table taxonomy_url ...' as Action;
CREATE  INDEX `ndx_fk_tu_t`  ON `dev5`.`taxonomy_url` (`taxonomy_id`);

select 'creating index ndx_fk_tu_created for table taxonomy_url ...' as Action;
CREATE  INDEX `ndx_fk_tu_created`  ON `dev5`.`taxonomy_url` (`created_by`);

select 'creating index ndx_fk_tu_modified for table taxonomy_url ...' as Action;
CREATE  INDEX `ndx_fk_tu_modified`  ON `dev5`.`taxonomy_url` (`modified_by`);

select 'creating index ndx_fk_tu_owned for table taxonomy_url ...' as Action;
CREATE  INDEX `ndx_fk_tu_owned`  ON `dev5`.`taxonomy_url` (`owned_by`);

/************ 5 Index Definitions for taxonomy_use *************/
select 'creating index ndx_fk_tus_t for table taxonomy_use ...' as Action;
CREATE  INDEX `ndx_fk_tus_t`  ON `dev5`.`taxonomy_use` (`taxonomy_id`);

select 'creating index ndx_tu_usage for table taxonomy_use ...' as Action;
CREATE  INDEX `ndx_tu_usage`  ON `dev5`.`taxonomy_use` (`economic_usage`);

select 'creating index ndx_fk_tus_created for table taxonomy_use ...' as Action;
CREATE  INDEX `ndx_fk_tus_created`  ON `dev5`.`taxonomy_use` (`created_by`);

select 'creating index ndx_fk_tus_modified for table taxonomy_use ...' as Action;
CREATE  INDEX `ndx_fk_tus_modified`  ON `dev5`.`taxonomy_use` (`modified_by`);

select 'creating index ndx_fk_tus_owned for table taxonomy_use ...' as Action;
CREATE  INDEX `ndx_fk_tus_owned`  ON `dev5`.`taxonomy_use` (`owned_by`);

/************ 5 Index Definitions for trait *************/
select 'creating index ndx_uniq_de for table trait ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_de`  ON `dev5`.`trait` (`short_name`, `crop_id`);

select 'creating index ndx_fk_tr_crop for table trait ...' as Action;
CREATE  INDEX `ndx_fk_tr_crop`  ON `dev5`.`trait` (`crop_id`);

select 'creating index ndx_fk_tr_created for table trait ...' as Action;
CREATE  INDEX `ndx_fk_tr_created`  ON `dev5`.`trait` (`created_by`);

select 'creating index ndx_fk_tr_modified for table trait ...' as Action;
CREATE  INDEX `ndx_fk_tr_modified`  ON `dev5`.`trait` (`modified_by`);

select 'creating index ndx_fk_tr_owned for table trait ...' as Action;
CREATE  INDEX `ndx_fk_tr_owned`  ON `dev5`.`trait` (`owned_by`);

/************ 4 Index Definitions for trait_code *************/
select 'creating index ndx_fk_tc_tr for table trait_code ...' as Action;
CREATE  INDEX `ndx_fk_tc_tr`  ON `dev5`.`trait_code` (`trait_id`);

select 'creating index ndx_fk_tc_created for table trait_code ...' as Action;
CREATE  INDEX `ndx_fk_tc_created`  ON `dev5`.`trait_code` (`created_by`);

select 'creating index ndx_fk_tc_modified for table trait_code ...' as Action;
CREATE  INDEX `ndx_fk_tc_modified`  ON `dev5`.`trait_code` (`modified_by`);

select 'creating index ndx_fk_tc_owned for table trait_code ...' as Action;
CREATE  INDEX `ndx_fk_tc_owned`  ON `dev5`.`trait_code` (`owned_by`);

/************ 5 Index Definitions for trait_code_friendly *************/
select 'creating index ndx_fk_tcf_tc for table trait_code_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_tc`  ON `dev5`.`trait_code_friendly` (`trait_code_id`);

select 'creating index ndx_fk_tct_sl for table trait_code_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tct_sl`  ON `dev5`.`trait_code_friendly` (`sec_lang_id`);

select 'creating index ndx_fk_tcf_created for table trait_code_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_created`  ON `dev5`.`trait_code_friendly` (`created_by`);

select 'creating index ndx_fk_tcf_modified for table trait_code_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_modified`  ON `dev5`.`trait_code_friendly` (`modified_by`);

select 'creating index ndx_fk_tcf_owned for table trait_code_friendly ...' as Action;
CREATE  INDEX `ndx_fk_tcf_owned`  ON `dev5`.`trait_code_friendly` (`owned_by`);

/************ 10 Index Definitions for trait_observation *************/
select 'creating index ndx_fk_to_tr for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_tr`  ON `dev5`.`trait_observation` (`trait_id`);

select 'creating index ndx_uniq_ob for table trait_observation ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_ob`  ON `dev5`.`trait_observation` (`trait_id`, `trait_code_id`, `accession_id`, `evaluation_id`, `qualifier_id`, `inventory_id`);

select 'creating index ndx_fk_to_tc for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_tc`  ON `dev5`.`trait_observation` (`trait_code_id`);

select 'creating index ndx_fk_to_a for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_a`  ON `dev5`.`trait_observation` (`accession_id`);

select 'creating index ndx_fk_to_e for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_e`  ON `dev5`.`trait_observation` (`evaluation_id`);

select 'creating index ndx_fk_to_tq for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_tq`  ON `dev5`.`trait_observation` (`qualifier_id`);

select 'creating index ndx_fk_to_i for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_i`  ON `dev5`.`trait_observation` (`inventory_id`);

select 'creating index ndx_fk_to_created for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_created`  ON `dev5`.`trait_observation` (`created_by`);

select 'creating index ndx_fk_to_modified for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_modified`  ON `dev5`.`trait_observation` (`modified_by`);

select 'creating index ndx_fk_to_owned for table trait_observation ...' as Action;
CREATE  INDEX `ndx_fk_to_owned`  ON `dev5`.`trait_observation` (`owned_by`);

/************ 5 Index Definitions for trait_qualifier *************/
select 'creating index ndx_dq_name for table trait_qualifier ...' as Action;
CREATE  INDEX `ndx_dq_name`  ON `dev5`.`trait_qualifier` (`trait_qualifier_name`);

select 'creating index ndx_fk_tq_tr for table trait_qualifier ...' as Action;
CREATE  INDEX `ndx_fk_tq_tr`  ON `dev5`.`trait_qualifier` (`trait_id`);

select 'creating index ndx_fk_tq_created for table trait_qualifier ...' as Action;
CREATE  INDEX `ndx_fk_tq_created`  ON `dev5`.`trait_qualifier` (`created_by`);

select 'creating index ndx_fk_tq_modified for table trait_qualifier ...' as Action;
CREATE  INDEX `ndx_fk_tq_modified`  ON `dev5`.`trait_qualifier` (`modified_by`);

select 'creating index ndx_fk_tq_owned for table trait_qualifier ...' as Action;
CREATE  INDEX `ndx_fk_tq_owned`  ON `dev5`.`trait_qualifier` (`owned_by`);

/************ 7 Index Definitions for trait_url *************/
select 'creating index ndx_uniq_du for table trait_url ...' as Action;
CREATE UNIQUE INDEX `ndx_uniq_du`  ON `dev5`.`trait_url` (`url_type`, `sequence_number`, `crop_id`, `trait_id`, `code`);

select 'creating index ndx_fk_tur_crop for table trait_url ...' as Action;
CREATE  INDEX `ndx_fk_tur_crop`  ON `dev5`.`trait_url` (`crop_id`);

select 'creating index ndx_fk_tur_tr for table trait_url ...' as Action;
CREATE  INDEX `ndx_fk_tur_tr`  ON `dev5`.`trait_url` (`trait_id`);

select 'creating index ndx_fk_tur_e for table trait_url ...' as Action;
CREATE  INDEX `ndx_fk_tur_e`  ON `dev5`.`trait_url` (`evaluation_id`);

select 'creating index ndx_fk_tur_created for table trait_url ...' as Action;
CREATE  INDEX `ndx_fk_tur_created`  ON `dev5`.`trait_url` (`created_by`);

select 'creating index ndx_fk_tur_modified for table trait_url ...' as Action;
CREATE  INDEX `ndx_fk_tur_modified`  ON `dev5`.`trait_url` (`modified_by`);

select 'creating index ndx_fk_tur_owned for table trait_url ...' as Action;
CREATE  INDEX `ndx_fk_tur_owned`  ON `dev5`.`trait_url` (`owned_by`);

/***********************************************/
/******** Migration Table Definitions **********/
/***********************************************/

/** Key Mapping Table Definition for accession **/
select 'creating migration table __accession ...' as Action;
CREATE TABLE `dev5`.`__accession` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_action **/
select 'creating migration table __accession_action ...' as Action;
CREATE TABLE `dev5`.`__accession_action` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_action` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_annotation **/
select 'creating migration table __accession_annotation ...' as Action;
CREATE TABLE `dev5`.`__accession_annotation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_annotation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_citation **/
select 'creating migration table __accession_citation ...' as Action;
CREATE TABLE `dev5`.`__accession_citation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_citation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_group **/
select 'creating migration table __accession_group ...' as Action;
CREATE TABLE `dev5`.`__accession_group` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_group` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_habitat **/
select 'creating migration table __accession_habitat ...' as Action;
CREATE TABLE `dev5`.`__accession_habitat` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_habitat` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_name **/
select 'creating migration table __accession_name ...' as Action;
CREATE TABLE `dev5`.`__accession_name` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_name` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_narrative **/
select 'creating migration table __accession_narrative ...' as Action;
CREATE TABLE `dev5`.`__accession_narrative` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_narrative` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_pedigree **/
select 'creating migration table __accession_pedigree ...' as Action;
CREATE TABLE `dev5`.`__accession_pedigree` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_pedigree` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_quarantine **/
select 'creating migration table __accession_quarantine ...' as Action;
CREATE TABLE `dev5`.`__accession_quarantine` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_quarantine` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_right **/
select 'creating migration table __accession_right ...' as Action;
CREATE TABLE `dev5`.`__accession_right` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_right` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_source **/
select 'creating migration table __accession_source ...' as Action;
CREATE TABLE `dev5`.`__accession_source` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_source` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_source_member **/
select 'creating migration table __accession_source_member ...' as Action;
CREATE TABLE `dev5`.`__accession_source_member` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_source_member` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_voucher **/
select 'creating migration table __accession_voucher ...' as Action;
CREATE TABLE `dev5`.`__accession_voucher` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_voucher` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for accession_voucher_image **/
select 'creating migration table __accession_voucher_image ...' as Action;
CREATE TABLE `dev5`.`__accession_voucher_image` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_accession_voucher_image` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for app_resource **/
select 'creating migration table __app_resource ...' as Action;
CREATE TABLE `dev5`.`__app_resource` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_app_resource` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for app_user_item_list **/
select 'creating migration table __app_user_item_list ...' as Action;
CREATE TABLE `dev5`.`__app_user_item_list` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_app_user_item_list` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for code_group **/
select 'creating migration table __code_group ...' as Action;
CREATE TABLE `dev5`.`__code_group` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_code_group` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for code_rule **/
select 'creating migration table __code_rule ...' as Action;
CREATE TABLE `dev5`.`__code_rule` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_code_rule` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for code_value **/
select 'creating migration table __code_value ...' as Action;
CREATE TABLE `dev5`.`__code_value` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_code_value` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for code_value_friendly **/
select 'creating migration table __code_value_friendly ...' as Action;
CREATE TABLE `dev5`.`__code_value_friendly` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_code_value_friendly` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for cooperator **/
select 'creating migration table __cooperator ...' as Action;
CREATE TABLE `dev5`.`__cooperator` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_cooperator` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for cooperator_group **/
select 'creating migration table __cooperator_group ...' as Action;
CREATE TABLE `dev5`.`__cooperator_group` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_cooperator_group` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for cooperator_member **/
select 'creating migration table __cooperator_member ...' as Action;
CREATE TABLE `dev5`.`__cooperator_member` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_cooperator_member` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for crop **/
select 'creating migration table __crop ...' as Action;
CREATE TABLE `dev5`.`__crop` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_crop` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for evaluation **/
select 'creating migration table __evaluation ...' as Action;
CREATE TABLE `dev5`.`__evaluation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_evaluation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for evaluation_citation **/
select 'creating migration table __evaluation_citation ...' as Action;
CREATE TABLE `dev5`.`__evaluation_citation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_evaluation_citation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for evaluation_member **/
select 'creating migration table __evaluation_member ...' as Action;
CREATE TABLE `dev5`.`__evaluation_member` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_evaluation_member` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for family **/
select 'creating migration table __family ...' as Action;
CREATE TABLE `dev5`.`__family` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_family` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genomic_annotation **/
select 'creating migration table __genomic_annotation ...' as Action;
CREATE TABLE `dev5`.`__genomic_annotation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genomic_annotation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genomic_marker **/
select 'creating migration table __genomic_marker ...' as Action;
CREATE TABLE `dev5`.`__genomic_marker` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genomic_marker` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genomic_marker_citation **/
select 'creating migration table __genomic_marker_citation ...' as Action;
CREATE TABLE `dev5`.`__genomic_marker_citation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genomic_marker_citation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genomic_observation **/
select 'creating migration table __genomic_observation ...' as Action;
CREATE TABLE `dev5`.`__genomic_observation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genomic_observation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genus **/
select 'creating migration table __genus ...' as Action;
CREATE TABLE `dev5`.`__genus` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genus` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genus_citation **/
select 'creating migration table __genus_citation ...' as Action;
CREATE TABLE `dev5`.`__genus_citation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genus_citation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for genus_type **/
select 'creating migration table __genus_type ...' as Action;
CREATE TABLE `dev5`.`__genus_type` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_genus_type` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for geography **/
select 'creating migration table __geography ...' as Action;
CREATE TABLE `dev5`.`__geography` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_geography` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory **/
select 'creating migration table __inventory ...' as Action;
CREATE TABLE `dev5`.`__inventory` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory_action **/
select 'creating migration table __inventory_action ...' as Action;
CREATE TABLE `dev5`.`__inventory_action` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory_action` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory_group **/
select 'creating migration table __inventory_group ...' as Action;
CREATE TABLE `dev5`.`__inventory_group` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory_group` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory_group_maintenance **/
select 'creating migration table __inventory_group_maintenance ...' as Action;
CREATE TABLE `dev5`.`__inventory_group_maintenance` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory_group_maintenance` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory_maintenance **/
select 'creating migration table __inventory_maintenance ...' as Action;
CREATE TABLE `dev5`.`__inventory_maintenance` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory_maintenance` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory_pathogen_test **/
select 'creating migration table __inventory_pathogen_test ...' as Action;
CREATE TABLE `dev5`.`__inventory_pathogen_test` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory_pathogen_test` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for inventory_viability **/
select 'creating migration table __inventory_viability ...' as Action;
CREATE TABLE `dev5`.`__inventory_viability` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_inventory_viability` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for literature **/
select 'creating migration table __literature ...' as Action;
CREATE TABLE `dev5`.`__literature` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_literature` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for order_entry **/
select 'creating migration table __order_entry ...' as Action;
CREATE TABLE `dev5`.`__order_entry` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_order_entry` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for order_entry_action **/
select 'creating migration table __order_entry_action ...' as Action;
CREATE TABLE `dev5`.`__order_entry_action` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_order_entry_action` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for order_entry_item **/
select 'creating migration table __order_entry_item ...' as Action;
CREATE TABLE `dev5`.`__order_entry_item` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_order_entry_item` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for plant_introduction **/
select 'creating migration table __plant_introduction ...' as Action;
CREATE TABLE `dev5`.`__plant_introduction` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_plant_introduction` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for region **/
select 'creating migration table __region ...' as Action;
CREATE TABLE `dev5`.`__region` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_region` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_lang **/
select 'creating migration table __sec_lang ...' as Action;
CREATE TABLE `dev5`.`__sec_lang` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_lang` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_perm **/
select 'creating migration table __sec_perm ...' as Action;
CREATE TABLE `dev5`.`__sec_perm` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_perm` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_perm_field **/
select 'creating migration table __sec_perm_field ...' as Action;
CREATE TABLE `dev5`.`__sec_perm_field` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_perm_field` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_perm_template **/
select 'creating migration table __sec_perm_template ...' as Action;
CREATE TABLE `dev5`.`__sec_perm_template` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_perm_template` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_perm_template_map **/
select 'creating migration table __sec_perm_template_map ...' as Action;
CREATE TABLE `dev5`.`__sec_perm_template_map` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_perm_template_map` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_rs **/
select 'creating migration table __sec_rs ...' as Action;
CREATE TABLE `dev5`.`__sec_rs` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_rs` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_rs_field **/
select 'creating migration table __sec_rs_field ...' as Action;
CREATE TABLE `dev5`.`__sec_rs_field` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_rs_field` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_rs_field_friendly **/
select 'creating migration table __sec_rs_field_friendly ...' as Action;
CREATE TABLE `dev5`.`__sec_rs_field_friendly` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_rs_field_friendly` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_rs_param **/
select 'creating migration table __sec_rs_param ...' as Action;
CREATE TABLE `dev5`.`__sec_rs_param` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_rs_param` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_table **/
select 'creating migration table __sec_table ...' as Action;
CREATE TABLE `dev5`.`__sec_table` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_table` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_table_field **/
select 'creating migration table __sec_table_field ...' as Action;
CREATE TABLE `dev5`.`__sec_table_field` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_table_field` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_user **/
select 'creating migration table __sec_user ...' as Action;
CREATE TABLE `dev5`.`__sec_user` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_user` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_user_gui_setting **/
select 'creating migration table __sec_user_gui_setting ...' as Action;
CREATE TABLE `dev5`.`__sec_user_gui_setting` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_user_gui_setting` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for sec_user_perm **/
select 'creating migration table __sec_user_perm ...' as Action;
CREATE TABLE `dev5`.`__sec_user_perm` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_sec_user_perm` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy **/
select 'creating migration table __taxonomy ...' as Action;
CREATE TABLE `dev5`.`__taxonomy` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_author **/
select 'creating migration table __taxonomy_author ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_author` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_author` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_citation **/
select 'creating migration table __taxonomy_citation ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_citation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_citation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_common_name **/
select 'creating migration table __taxonomy_common_name ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_common_name` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_common_name` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_distribution **/
select 'creating migration table __taxonomy_distribution ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_distribution` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_distribution` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_germination_rule **/
select 'creating migration table __taxonomy_germination_rule ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_germination_rule` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_germination_rule` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_url **/
select 'creating migration table __taxonomy_url ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_url` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_url` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for taxonomy_use **/
select 'creating migration table __taxonomy_use ...' as Action;
CREATE TABLE `dev5`.`__taxonomy_use` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_taxonomy_use` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for trait **/
select 'creating migration table __trait ...' as Action;
CREATE TABLE `dev5`.`__trait` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_trait` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for trait_code **/
select 'creating migration table __trait_code ...' as Action;
CREATE TABLE `dev5`.`__trait_code` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_trait_code` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for trait_code_friendly **/
select 'creating migration table __trait_code_friendly ...' as Action;
CREATE TABLE `dev5`.`__trait_code_friendly` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_trait_code_friendly` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for trait_observation **/
select 'creating migration table __trait_observation ...' as Action;
CREATE TABLE `dev5`.`__trait_observation` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_trait_observation` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for trait_qualifier **/
select 'creating migration table __trait_qualifier ...' as Action;
CREATE TABLE `dev5`.`__trait_qualifier` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_trait_qualifier` (previous_id)
) Engine=MyISAM ;

/** Key Mapping Table Definition for trait_url **/
select 'creating migration table __trait_url ...' as Action;
CREATE TABLE `dev5`.`__trait_url` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_trait_url` (previous_id)
) Engine=MyISAM ;

select 'Disabling index updates on dev5.accession ...' as Action;
ALTER TABLE dev5.accession DISABLE KEYS;

select 'Disabling index updates on dev5.accession_action ...' as Action;
ALTER TABLE dev5.accession_action DISABLE KEYS;

select 'Disabling index updates on dev5.accession_annotation ...' as Action;
ALTER TABLE dev5.accession_annotation DISABLE KEYS;

select 'Disabling index updates on dev5.accession_citation ...' as Action;
ALTER TABLE dev5.accession_citation DISABLE KEYS;

select 'Disabling index updates on dev5.accession_group ...' as Action;
ALTER TABLE dev5.accession_group DISABLE KEYS;

select 'Disabling index updates on dev5.accession_habitat ...' as Action;
ALTER TABLE dev5.accession_habitat DISABLE KEYS;

select 'Disabling index updates on dev5.accession_name ...' as Action;
ALTER TABLE dev5.accession_name DISABLE KEYS;

select 'Disabling index updates on dev5.accession_narrative ...' as Action;
ALTER TABLE dev5.accession_narrative DISABLE KEYS;

select 'Disabling index updates on dev5.accession_pedigree ...' as Action;
ALTER TABLE dev5.accession_pedigree DISABLE KEYS;

select 'Disabling index updates on dev5.accession_quarantine ...' as Action;
ALTER TABLE dev5.accession_quarantine DISABLE KEYS;

select 'Disabling index updates on dev5.accession_right ...' as Action;
ALTER TABLE dev5.accession_right DISABLE KEYS;

select 'Disabling index updates on dev5.accession_source ...' as Action;
ALTER TABLE dev5.accession_source DISABLE KEYS;

select 'Disabling index updates on dev5.accession_source_member ...' as Action;
ALTER TABLE dev5.accession_source_member DISABLE KEYS;

select 'Disabling index updates on dev5.accession_voucher ...' as Action;
ALTER TABLE dev5.accession_voucher DISABLE KEYS;

select 'Disabling index updates on dev5.accession_voucher_image ...' as Action;
ALTER TABLE dev5.accession_voucher_image DISABLE KEYS;

select 'Disabling index updates on dev5.app_resource ...' as Action;
ALTER TABLE dev5.app_resource DISABLE KEYS;

select 'Disabling index updates on dev5.app_user_item_list ...' as Action;
ALTER TABLE dev5.app_user_item_list DISABLE KEYS;

select 'Disabling index updates on dev5.code_group ...' as Action;
ALTER TABLE dev5.code_group DISABLE KEYS;

select 'Disabling index updates on dev5.code_rule ...' as Action;
ALTER TABLE dev5.code_rule DISABLE KEYS;

select 'Disabling index updates on dev5.code_value ...' as Action;
ALTER TABLE dev5.code_value DISABLE KEYS;

select 'Disabling index updates on dev5.code_value_friendly ...' as Action;
ALTER TABLE dev5.code_value_friendly DISABLE KEYS;

select 'Disabling index updates on dev5.cooperator ...' as Action;
ALTER TABLE dev5.cooperator DISABLE KEYS;

select 'Disabling index updates on dev5.cooperator_group ...' as Action;
ALTER TABLE dev5.cooperator_group DISABLE KEYS;

select 'Disabling index updates on dev5.cooperator_member ...' as Action;
ALTER TABLE dev5.cooperator_member DISABLE KEYS;

select 'Disabling index updates on dev5.crop ...' as Action;
ALTER TABLE dev5.crop DISABLE KEYS;

select 'Disabling index updates on dev5.evaluation ...' as Action;
ALTER TABLE dev5.evaluation DISABLE KEYS;

select 'Disabling index updates on dev5.evaluation_citation ...' as Action;
ALTER TABLE dev5.evaluation_citation DISABLE KEYS;

select 'Disabling index updates on dev5.evaluation_member ...' as Action;
ALTER TABLE dev5.evaluation_member DISABLE KEYS;

select 'Disabling index updates on dev5.family ...' as Action;
ALTER TABLE dev5.family DISABLE KEYS;

select 'Disabling index updates on dev5.genomic_annotation ...' as Action;
ALTER TABLE dev5.genomic_annotation DISABLE KEYS;

select 'Disabling index updates on dev5.genomic_marker ...' as Action;
ALTER TABLE dev5.genomic_marker DISABLE KEYS;

select 'Disabling index updates on dev5.genomic_marker_citation ...' as Action;
ALTER TABLE dev5.genomic_marker_citation DISABLE KEYS;

select 'Disabling index updates on dev5.genomic_observation ...' as Action;
ALTER TABLE dev5.genomic_observation DISABLE KEYS;

select 'Disabling index updates on dev5.genus ...' as Action;
ALTER TABLE dev5.genus DISABLE KEYS;

select 'Disabling index updates on dev5.genus_citation ...' as Action;
ALTER TABLE dev5.genus_citation DISABLE KEYS;

select 'Disabling index updates on dev5.genus_type ...' as Action;
ALTER TABLE dev5.genus_type DISABLE KEYS;

select 'Disabling index updates on dev5.geography ...' as Action;
ALTER TABLE dev5.geography DISABLE KEYS;

select 'Disabling index updates on dev5.inventory ...' as Action;
ALTER TABLE dev5.inventory DISABLE KEYS;

select 'Disabling index updates on dev5.inventory_action ...' as Action;
ALTER TABLE dev5.inventory_action DISABLE KEYS;

select 'Disabling index updates on dev5.inventory_group ...' as Action;
ALTER TABLE dev5.inventory_group DISABLE KEYS;

select 'Disabling index updates on dev5.inventory_group_maintenance ...' as Action;
ALTER TABLE dev5.inventory_group_maintenance DISABLE KEYS;

select 'Disabling index updates on dev5.inventory_maintenance ...' as Action;
ALTER TABLE dev5.inventory_maintenance DISABLE KEYS;

select 'Disabling index updates on dev5.inventory_pathogen_test ...' as Action;
ALTER TABLE dev5.inventory_pathogen_test DISABLE KEYS;

select 'Disabling index updates on dev5.inventory_viability ...' as Action;
ALTER TABLE dev5.inventory_viability DISABLE KEYS;

select 'Disabling index updates on dev5.literature ...' as Action;
ALTER TABLE dev5.literature DISABLE KEYS;

select 'Disabling index updates on dev5.order_entry ...' as Action;
ALTER TABLE dev5.order_entry DISABLE KEYS;

select 'Disabling index updates on dev5.order_entry_action ...' as Action;
ALTER TABLE dev5.order_entry_action DISABLE KEYS;

select 'Disabling index updates on dev5.order_entry_item ...' as Action;
ALTER TABLE dev5.order_entry_item DISABLE KEYS;

select 'Disabling index updates on dev5.plant_introduction ...' as Action;
ALTER TABLE dev5.plant_introduction DISABLE KEYS;

select 'Disabling index updates on dev5.region ...' as Action;
ALTER TABLE dev5.region DISABLE KEYS;

select 'Disabling index updates on dev5.sec_lang ...' as Action;
ALTER TABLE dev5.sec_lang DISABLE KEYS;

select 'Disabling index updates on dev5.sec_perm ...' as Action;
ALTER TABLE dev5.sec_perm DISABLE KEYS;

select 'Disabling index updates on dev5.sec_perm_field ...' as Action;
ALTER TABLE dev5.sec_perm_field DISABLE KEYS;

select 'Disabling index updates on dev5.sec_perm_template ...' as Action;
ALTER TABLE dev5.sec_perm_template DISABLE KEYS;

select 'Disabling index updates on dev5.sec_perm_template_map ...' as Action;
ALTER TABLE dev5.sec_perm_template_map DISABLE KEYS;

select 'Disabling index updates on dev5.sec_rs ...' as Action;
ALTER TABLE dev5.sec_rs DISABLE KEYS;

select 'Disabling index updates on dev5.sec_rs_field ...' as Action;
ALTER TABLE dev5.sec_rs_field DISABLE KEYS;

select 'Disabling index updates on dev5.sec_rs_field_friendly ...' as Action;
ALTER TABLE dev5.sec_rs_field_friendly DISABLE KEYS;

select 'Disabling index updates on dev5.sec_rs_param ...' as Action;
ALTER TABLE dev5.sec_rs_param DISABLE KEYS;

select 'Disabling index updates on dev5.sec_table ...' as Action;
ALTER TABLE dev5.sec_table DISABLE KEYS;

select 'Disabling index updates on dev5.sec_table_field ...' as Action;
ALTER TABLE dev5.sec_table_field DISABLE KEYS;

select 'Disabling index updates on dev5.sec_user ...' as Action;
ALTER TABLE dev5.sec_user DISABLE KEYS;

select 'Disabling index updates on dev5.sec_user_gui_setting ...' as Action;
ALTER TABLE dev5.sec_user_gui_setting DISABLE KEYS;

select 'Disabling index updates on dev5.sec_user_perm ...' as Action;
ALTER TABLE dev5.sec_user_perm DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy ...' as Action;
ALTER TABLE dev5.taxonomy DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_author ...' as Action;
ALTER TABLE dev5.taxonomy_author DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_citation ...' as Action;
ALTER TABLE dev5.taxonomy_citation DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_common_name ...' as Action;
ALTER TABLE dev5.taxonomy_common_name DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_distribution ...' as Action;
ALTER TABLE dev5.taxonomy_distribution DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_germination_rule ...' as Action;
ALTER TABLE dev5.taxonomy_germination_rule DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_url ...' as Action;
ALTER TABLE dev5.taxonomy_url DISABLE KEYS;

select 'Disabling index updates on dev5.taxonomy_use ...' as Action;
ALTER TABLE dev5.taxonomy_use DISABLE KEYS;

select 'Disabling index updates on dev5.trait ...' as Action;
ALTER TABLE dev5.trait DISABLE KEYS;

select 'Disabling index updates on dev5.trait_code ...' as Action;
ALTER TABLE dev5.trait_code DISABLE KEYS;

select 'Disabling index updates on dev5.trait_code_friendly ...' as Action;
ALTER TABLE dev5.trait_code_friendly DISABLE KEYS;

select 'Disabling index updates on dev5.trait_observation ...' as Action;
ALTER TABLE dev5.trait_observation DISABLE KEYS;

select 'Disabling index updates on dev5.trait_qualifier ...' as Action;
ALTER TABLE dev5.trait_qualifier DISABLE KEYS;

select 'Disabling index updates on dev5.trait_url ...' as Action;
ALTER TABLE dev5.trait_url DISABLE KEYS;

/***********************************************/
/********* Insert Key Mapping Data *************/
/***********************************************/

/* accession */
select 'mapping old id to new id for accession ...' as Action;
insert into dev5.__accession (previous_id) select acid from dev2.acc;

/* accession_action */
select 'mapping old id to new id for accession_action ...' as Action;
insert into dev5.__accession_action (previous_id) select aactno from dev2.aact;

/* accession_annotation */
select 'mapping old id to new id for accession_annotation ...' as Action;
insert into dev5.__accession_annotation (previous_id) select alno from dev2.al;

/* accession_citation */
select 'mapping old id to new id for accession_citation ...' as Action;
insert into dev5.__accession_citation (previous_id) select citno from dev2.acit;

/* accession_group */
select 'mapping old id to new id for accession_group ...' as Action;
insert into dev5.__accession_group (previous_id) select agid from dev2.ag;

/* accession_habitat */
select 'mapping old id to new id for accession_habitat ...' as Action;
insert into dev5.__accession_habitat (previous_id) select srcno from dev2.hab;

/* accession_name */
select 'mapping old id to new id for accession_name ...' as Action;
insert into dev5.__accession_name (previous_id) select anno from dev2.an;

/* accession_narrative */
select 'mapping old id to new id for accession_narrative ...' as Action;
insert into dev5.__accession_narrative (previous_id) select narr_id from dev2.narr;

/* accession_pedigree */
select 'mapping old id to new id for accession_pedigree ...' as Action;
insert into dev5.__accession_pedigree (previous_id) select pedid from dev2.ped;

/* accession_quarantine */
select 'mapping old id to new id for accession_quarantine ...' as Action;
insert into dev5.__accession_quarantine (previous_id) select quarid from dev2.quar;

/* accession_right */
select 'mapping old id to new id for accession_right ...' as Action;
insert into dev5.__accession_right (previous_id) select iprid_int from dev2.ipr;

/* accession_source */
select 'mapping old id to new id for accession_source ...' as Action;
insert into dev5.__accession_source (previous_id) select srcno from dev2.src;

/* accession_source_member */
select 'mapping old id to new id for accession_source_member ...' as Action;
insert into dev5.__accession_source_member (previous_id) select smbrid from dev2.smbr;

/* accession_voucher */
select 'mapping old id to new id for accession_voucher ...' as Action;
insert into dev5.__accession_voucher (previous_id) select vno from dev2.vou;

/* app_resource */
select 'mapping old id to new id for app_resource ...' as Action;
insert into dev5.__app_resource (previous_id) select app_resource_id from dev2.app_resource;

/* app_user_item_list */
select 'mapping old id to new id for app_user_item_list ...' as Action;
insert into dev5.__app_user_item_list (previous_id) select groups_id from dev2.groups;

/* code_group */
select 'mapping old id to new id for code_group ...' as Action;
insert into dev5.__code_group (previous_id) select code_group_id from dev2.code_group;

/* code_rule */
select 'mapping old id to new id for code_rule ...' as Action;
insert into dev5.__code_rule (previous_id) select code_rule_id from dev2.code_rule;

/* code_value */
select 'mapping old id to new id for code_value ...' as Action;
insert into dev5.__code_value (previous_id) select code_value_id from dev2.code_value;

/* cooperator */
select 'mapping old id to new id for cooperator ...' as Action;
insert into dev5.__cooperator (previous_id) select cno from dev2.coop;

/* cooperator_group */
select 'mapping old id to new id for cooperator_group ...' as Action;
insert into dev5.__cooperator_group (previous_id) select cgid_int from dev2.cg;

/* cooperator_member */
select 'mapping old id to new id for cooperator_member ...' as Action;
insert into dev5.__cooperator_member (previous_id) select mbrid from dev2.mbr;

/* crop */
select 'mapping old id to new id for crop ...' as Action;
insert into dev5.__crop (previous_id) select cropno from dev2.crop;

/* evaluation */
select 'mapping old id to new id for evaluation ...' as Action;
insert into dev5.__evaluation (previous_id) select eno from dev2.eval;

/* evaluation_citation */
select 'mapping old id to new id for evaluation_citation ...' as Action;
insert into dev5.__evaluation_citation (previous_id) select citno from dev2.ecit;

/* evaluation_member */
select 'mapping old id to new id for evaluation_member ...' as Action;
insert into dev5.__evaluation_member (previous_id) select embrid from dev2.embr;

/* family */
select 'mapping old id to new id for family ...' as Action;
insert into dev5.__family (previous_id) select famno from dev2.fam;

/* genomic_annotation */
select 'mapping old id to new id for genomic_annotation ...' as Action;
insert into dev5.__genomic_annotation (previous_id) select gano from dev2.ga;

/* genomic_marker */
select 'mapping old id to new id for genomic_marker ...' as Action;
insert into dev5.__genomic_marker (previous_id) select mrkno from dev2.mrk;

/* genomic_marker_citation */
select 'mapping old id to new id for genomic_marker_citation ...' as Action;
insert into dev5.__genomic_marker_citation (previous_id) select citno from dev2.mcit;

/* genomic_observation */
select 'mapping old id to new id for genomic_observation ...' as Action;
insert into dev5.__genomic_observation (previous_id) select gobno from dev2.gob;

/* genus */
select 'mapping old id to new id for genus ...' as Action;
insert into dev5.__genus (previous_id) select gno from dev2.gn;

/* genus_citation */
select 'mapping old id to new id for genus_citation ...' as Action;
insert into dev5.__genus_citation (previous_id) select citno from dev2.gcit;

/* genus_type */
select 'mapping old id to new id for genus_type ...' as Action;
insert into dev5.__genus_type (previous_id) select gno from dev2.gnt;

/* geography */
select 'mapping old id to new id for geography ...' as Action;
insert into dev5.__geography (previous_id) select geono from dev2.geo;

/* inventory */
select 'mapping old id to new id for inventory ...' as Action;
insert into dev5.__inventory (previous_id) select ivid from dev2.iv;

/* inventory_action */
select 'mapping old id to new id for inventory_action ...' as Action;
insert into dev5.__inventory_action (previous_id) select iactno from dev2.iact;

/* inventory_group */
select 'mapping old id to new id for inventory_group ...' as Action;
insert into dev5.__inventory_group (previous_id) select igid from dev2.ig;

/* inventory_group_maintenance */
select 'mapping old id to new id for inventory_group_maintenance ...' as Action;
insert into dev5.__inventory_group_maintenance (previous_id) select igmid from dev2.igm;

/* inventory_maintenance */
select 'mapping old id to new id for inventory_maintenance ...' as Action;
insert into dev5.__inventory_maintenance (previous_id) select imid from dev2.im;

/* inventory_pathogen_test */
select 'mapping old id to new id for inventory_pathogen_test ...' as Action;
insert into dev5.__inventory_pathogen_test (previous_id) select ptid from dev2.pt;

/* inventory_viability */
select 'mapping old id to new id for inventory_viability ...' as Action;
insert into dev5.__inventory_viability (previous_id) select viano from dev2.via;

/* literature */
select 'mapping old id to new id for literature ...' as Action;
insert into dev5.__literature (previous_id) select litid from dev2.lit;

/* order_entry */
select 'mapping old id to new id for order_entry ...' as Action;
insert into dev5.__order_entry (previous_id) select orno from dev2.ord;

/* order_entry_action */
select 'mapping old id to new id for order_entry_action ...' as Action;
insert into dev5.__order_entry_action (previous_id) select oactno from dev2.oact;

/* order_entry_item */
select 'mapping old id to new id for order_entry_item ...' as Action;
insert into dev5.__order_entry_item (previous_id) select itno from dev2.oi;

/* plant_introduction */
select 'mapping old id to new id for plant_introduction ...' as Action;
insert into dev5.__plant_introduction (previous_id) select pivol from dev2.pi;

/* region */
select 'mapping old id to new id for region ...' as Action;
insert into dev5.__region (previous_id) select regno from dev2.reg;

/* sec_lang */
select 'mapping old id to new id for sec_lang ...' as Action;
insert into dev5.__sec_lang (previous_id) select sec_lang_id from dev2.sec_lang;

/* sec_perm */
select 'mapping old id to new id for sec_perm ...' as Action;
insert into dev5.__sec_perm (previous_id) select sec_perm_id from dev2.sec_perm;

/* sec_perm_field */
select 'mapping old id to new id for sec_perm_field ...' as Action;
insert into dev5.__sec_perm_field (previous_id) select sec_perm_field_id from dev2.sec_perm_field;

/* sec_perm_template */
select 'mapping old id to new id for sec_perm_template ...' as Action;
insert into dev5.__sec_perm_template (previous_id) select sec_perm_template_id from dev2.sec_perm_template;

/* sec_perm_template_map */
select 'mapping old id to new id for sec_perm_template_map ...' as Action;
insert into dev5.__sec_perm_template_map (previous_id) select sec_perm_template_map_id from dev2.sec_perm_template_map;

/* sec_rs */
select 'mapping old id to new id for sec_rs ...' as Action;
insert into dev5.__sec_rs (previous_id) select sec_rs_id from dev2.sec_rs;

/* sec_rs_field */
select 'mapping old id to new id for sec_rs_field ...' as Action;
insert into dev5.__sec_rs_field (previous_id) select sec_rs_field_id from dev2.sec_rs_field;

/* sec_rs_field_friendly */
select 'mapping old id to new id for sec_rs_field_friendly ...' as Action;
insert into dev5.__sec_rs_field_friendly (previous_id) select sec_rs_field_friendly_id from dev2.sec_rs_field_friendly;

/* sec_rs_param */
select 'mapping old id to new id for sec_rs_param ...' as Action;
insert into dev5.__sec_rs_param (previous_id) select sec_rs_param_id from dev2.sec_rs_param;

/* sec_table */
select 'mapping old id to new id for sec_table ...' as Action;
insert into dev5.__sec_table (previous_id) select sec_table_id from dev2.sec_table;

/* sec_table_field */
select 'mapping old id to new id for sec_table_field ...' as Action;
insert into dev5.__sec_table_field (previous_id) select sec_table_field_id from dev2.sec_table_field;

/* sec_user */
select 'mapping old id to new id for sec_user ...' as Action;
insert into dev5.__sec_user (previous_id) select sec_user_id from dev2.sec_user;

/* sec_user_gui_setting */
select 'mapping old id to new id for sec_user_gui_setting ...' as Action;
insert into dev5.__sec_user_gui_setting (previous_id) select sec_user_gui_settings_id from dev2.sec_user_gui_settings;

/* sec_user_perm */
select 'mapping old id to new id for sec_user_perm ...' as Action;
insert into dev5.__sec_user_perm (previous_id) select sec_user_perm_id from dev2.sec_user_perm;

/* taxonomy */
select 'mapping old id to new id for taxonomy ...' as Action;
insert into dev5.__taxonomy (previous_id) select taxno from dev2.tax;

/* taxonomy_author */
select 'mapping old id to new id for taxonomy_author ...' as Action;
insert into dev5.__taxonomy_author (previous_id) select tautid from dev2.taut;

/* taxonomy_citation */
select 'mapping old id to new id for taxonomy_citation ...' as Action;
insert into dev5.__taxonomy_citation (previous_id) select citno from dev2.tcit;

/* taxonomy_common_name */
select 'mapping old id to new id for taxonomy_common_name ...' as Action;
insert into dev5.__taxonomy_common_name (previous_id) select cnid_int from dev2.cn;

/* taxonomy_distribution */
select 'mapping old id to new id for taxonomy_distribution ...' as Action;
insert into dev5.__taxonomy_distribution (previous_id) select distno from dev2.dist;

/* taxonomy_germination_rule */
select 'mapping old id to new id for taxonomy_germination_rule ...' as Action;
insert into dev5.__taxonomy_germination_rule (previous_id) select ruleno from dev2.germrule;

/* taxonomy_url */
select 'mapping old id to new id for taxonomy_url ...' as Action;
insert into dev5.__taxonomy_url (previous_id) select turlno from dev2.turl;

/* taxonomy_use */
select 'mapping old id to new id for taxonomy_use ...' as Action;
insert into dev5.__taxonomy_use (previous_id) select usesid from dev2.uses;

/* trait */
select 'mapping old id to new id for trait ...' as Action;
insert into dev5.__trait (previous_id) select dno from dev2.dsc;

/* trait_code */
select 'mapping old id to new id for trait_code ...' as Action;
insert into dev5.__trait_code (previous_id) select cdid from dev2.cd;

/* trait_observation */
select 'mapping old id to new id for trait_observation ...' as Action;
insert into dev5.__trait_observation (previous_id) select obno from dev2.ob;

/* trait_qualifier */
select 'mapping old id to new id for trait_qualifier ...' as Action;
insert into dev5.__trait_qualifier (previous_id) select qno from dev2.qual;

/* trait_url */
select 'mapping old id to new id for trait_url ...' as Action;
insert into dev5.__trait_url (previous_id) select durlid from dev2.durl;



/***********************************************/
/********* Insert Real Data ********************/
/***********************************************/

/* accession */
select 'copying data to accession ...' as Action;
insert into dev5.accession (accession_prefix, accession_number, accession_suffix, accession_name_id, site_code, inactive_site_code_reason, is_core, is_at_alternate_site, life_form, level_of_improvement_code, reproductive_uniformity, initial_material_type, initial_recieved_date, initial_received_date_format, taxonomy_id, plant_introduction_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select acp, acno, acs, (select new_id from dev5.__accession_name where previous_id = (select an_orig.anno from dev2.an an_orig where an_orig.acid = dev2.acc.acid order by coalesce(an_orig.topname,'Z'), an_orig.idrank limit 1)), site, whynull, coalesce(core,'N'), coalesce(backup,'N'), lifeform, acimpt, uniform, acform, received, datefmt, (select new_id from dev5.__taxonomy where previous_id = dev2.acc.taxno), (select new_id from dev5.__plant_introduction where previous_id = dev2.acc.pivol), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.acc.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.acc.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.acc.userid)) from dev2.acc;

/* accession_action */
select 'copying data to accession_action ...' as Action;
insert into dev5.accession_action (accession_id, action_name, occurred_date, occurred_date_format, completed_date, completed_date_format, is_visible_from_web, narrative, cooperator_id, evaluation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.aact.acid), action, occurred, fmtoccurred, completed, fmtcompleted, coalesce(showweb,'N'), narr, (select new_id from dev5.__cooperator where previous_id = dev2.aact.cno), (select new_id from dev5.__evaluation where previous_id = dev2.aact.eno), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.aact.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.aact.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.aact.userid)) from dev2.aact;

/* accession_annotation */
select 'copying data to accession_annotation ...' as Action;
insert into dev5.accession_annotation (action_name, action_date, accession_id, site_code, cooperator_id, inventory_id, order_entry_id, old_taxonomy_id, new_taxonomy_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select action, acted, (select new_id from dev5.__accession where previous_id = dev2.al.acid), site, (select new_id from dev5.__cooperator where previous_id = dev2.al.cno), (select new_id from dev5.__inventory where previous_id = dev2.al.ivid), (select new_id from dev5.__order_entry where previous_id = dev2.al.orno), (select new_id from dev5.__taxonomy where previous_id = dev2.al.oldtaxno), (select new_id from dev5.__taxonomy where previous_id = dev2.al.newtaxno), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.al.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.al.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.al.userid)) from dev2.al;

/* accession_citation */
select 'copying data to accession_citation ...' as Action;
insert into dev5.accession_citation (accession_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.acit.acid), (select new_id from dev5.__literature where previous_id = (select lit_orig.litid from dev2.lit lit_orig where lit_orig.abbr = dev2.acit.abbr)), cittitle, author, STR_TO_DATE(CONCAT(cityr, '0101'), '%Y%m%d'), citref, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.acit.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.acit.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.acit.userid)) from dev2.acit;

/* accession_group */
select 'copying data to accession_group ...' as Action;
insert into dev5.accession_group (accession_group_code, note, site_code, url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select agname, cmt, site, url, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ag.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ag.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ag.userid)) from dev2.ag;

/* accession_habitat */
select 'copying data to accession_habitat ...' as Action;
insert into dev5.accession_habitat (accession_id, latitude_degrees, latitude_minutes, latitude_seconds, latitude_hemisphere, longitude_degrees, longitude_minutes, longitude_seconds, longitude_hemisphere, elevation_in_meters, quantity_collected, unit_of_quantity_collected, form_material_collected_code, plant_sample_count, locality, habitat_name, note, collection_coordinate_system, gstype, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.hab.acid), latd, latm, lats, lath, lond, lonm, lons, lonh, elev, quant, units, cform, plants, locality, habitat, cmt, gctype, gstype, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.hab.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.hab.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.hab.userid)) from dev2.hab;

/* accession_name */
select 'copying data to accession_name ...' as Action;
insert into dev5.accession_name (accession_id, category, name, accession_group_id, inventory_id, cooperator_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.an.acid), idtype, plantid, (select new_id from dev5.__accession_name where previous_id = (select ag_orig.agid from dev2.ag ag_orig where ag_orig.agname = dev2.an.agname)), (select new_id from dev5.__inventory where previous_id = dev2.an.ivid), (select new_id from dev5.__cooperator where previous_id = dev2.an.cno), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.an.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.an.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.an.userid)) from dev2.an;

/* accession_narrative */
select 'copying data to accession_narrative ...' as Action;
insert into dev5.accession_narrative (accession_id, type_code, narrative_body, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.narr.acid), ntype, narr, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.narr.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.narr.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.narr.userid)) from dev2.narr;

/* accession_pedigree */
select 'copying data to accession_pedigree ...' as Action;
insert into dev5.accession_pedigree (accession_id, released_date, released_date_format, citation_id, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.ped.acid), released, datefmt, (select new_id from dev5.__accession_citation where previous_id = dev2.ped.citno), pedigree, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ped.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ped.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ped.userid)) from dev2.ped;

/* accession_quarantine */
select 'copying data to accession_quarantine ...' as Action;
insert into dev5.accession_quarantine (accession_id, quarantine_type, progress_status_code, cooperator_id, entered_date, established_date, expected_release_date, released_date, note, site_code, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.quar.acid), qtype, status, (select new_id from dev5.__cooperator where previous_id = dev2.quar.cno), entered, establish, expected, released, cmt, site, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.quar.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.quar.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.quar.userid)) from dev2.quar;

/* accession_right */
select 'copying data to accession_right ...' as Action;
insert into dev5.accession_right (accession_id, assigned_type, right_prefix, right_number, crop_name, full_name, issued_date, expired_date, cooperator_id, citation_id, note, site_code, accepted_date, expected_date, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.ipr.acid), iprtype, iprid, iprno, iprcrop, iprname, issued, expired, (select new_id from dev5.__cooperator where previous_id = dev2.ipr.cno), (select new_id from dev5.__accession_citation where previous_id = dev2.ipr.citno), cmt, site, accepted, expected, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ipr.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ipr.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ipr.userid)) from dev2.ipr;

/* accession_source */
select 'copying data to accession_source ...' as Action;
insert into dev5.accession_source (type_code, step_date, step_date_format, is_origin_step, accession_id, geography_id, note, source_qualifier, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select srctype, srcdate, datefmt, coalesce(origin,'N'), (select new_id from dev5.__accession where previous_id = dev2.src.acid), (select new_id from dev5.__geography where previous_id = dev2.src.geono), cmt, srcqual, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.src.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.src.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.src.userid)) from dev2.src;

/* accession_source_member */
select 'copying data to accession_source_member ...' as Action;
insert into dev5.accession_source_member (accession_source_id, accession_id, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession_source where previous_id = dev2.smbr.srcno), (select new_id from dev5.__accession where previous_id = dev2.smbr.acid), (select new_id from dev5.__cooperator where previous_id = dev2.smbr.cno), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.smbr.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.smbr.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.smbr.userid)) from dev2.smbr;

/* accession_voucher */
select 'copying data to accession_voucher ...' as Action;
insert into dev5.accession_voucher (accession_id, voucher_type, inventory_id, cooperator_id, vouchered_date, vouchered_date_format, collector_identifier, storage_location, sample_contents, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__accession where previous_id = dev2.vou.acid), vtype, (select new_id from dev5.__inventory where previous_id = dev2.vou.ivid), (select new_id from dev5.__cooperator where previous_id = dev2.vou.cno), vouchered, datefmt, collid, vloc, vcontent, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.vou.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.vou.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.vou.userid)) from dev2.vou;

/* accession_voucher_image **************** No previous definition **************** */

/* app_resource */
select 'copying data to app_resource ...' as Action;
insert into dev5.app_resource (sec_lang_id, app_resource_name, display_member, value_member, sort_order, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select coalesce((select new_id from dev5.__sec_lang where previous_id = (select sl_orig.sec_lang_id from dev2.sec_lang sl_orig where sl_orig.iso_639_3_code = dev2.app_resource.language_code)), 1), app_resource_name, display_member, value_member, sort_order, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.app_resource;

/* app_user_item_list */
select 'copying data to app_user_item_list ...' as Action;
insert into dev5.app_user_item_list (cooperator_id, tab_name, list_name, id_number, id_type, friendly_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__cooperator where previous_id = dev2.groups.cno), tabname, groupname, idno, idnotype, friendlyname, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.groups;

/* code_group */
select 'copying data to code_group ...' as Action;
insert into dev5.code_group (name, site_code, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select name, site_code, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.code_group;

/* code_rule */
select 'copying data to code_rule ...' as Action;
insert into dev5.code_rule (table_name, column_name, code_value_id, site_code, max_length, function_name, is_standard, is_by_category, cateogry_number, category_note, form_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select table_name, column_name, (select new_id from dev5.__code_value where previous_id = dev2.code_rule.code_no), site, maxlen, function, coalesce(std,'N'), coalesce(cat_flag,'N'), cat_no, cat_cmt, form_name, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.code_rule.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.code_rule.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.code_rule.userid)) from dev2.code_rule;

/* code_value */
select 'copying data to code_value ...' as Action;
insert into dev5.code_value (code_group_id, value, site_code, is_standard, category_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select code_no, code, site, coalesce(std,'N'), cat, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.code_value.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.code_value.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.code_value.userid)) from dev2.code_value;

/* code_value_friendly **************** No previous definition **************** */

select 'copying language-specific code_value data into code_value_friendly' as Action;

insert into dev5.code_value_friendly
	(code_value_id, sec_lang_id, friendly_name, friendly_description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	(select new_id from dev5.__code_value where previous_id = cv_orig.code_value_id),
	(select sec_lang_id from dev5.sec_lang where iso_639_3_code = 'ENG'),
	cv_orig.def,
	cv_orig.cmt,
	(select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),
	'20090106',
	(select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),
	'20090106',
	(select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),
	'20090106'
from
	dev2.code_value cv_orig

/* cooperator */
select 'copying data to cooperator ...' as Action;
insert into dev5.cooperator (current_cooperator_id, site_code, last_name, title, first_name, job, organization, organization_code, address_line1, address_line2, address_line3, admin_1, admin_2, geography_id, primary_phone, secondary_phone, fax, email, is_active, category_code, ars_region, discipline, initials, full_name, note, sec_lang_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__cooperator where previous_id = dev2.coop.validcno), site, lname, title, fname, job, org, orgid, add1, add2, add3, city, zip, geono, phone1, phone2, fax, email, coalesce(active,'N'), cat, arsregion, discipline, initials, coop, cmt, coalesce((select new_id from dev5.__sec_lang where previous_id = (select sl_orig.sec_lang_id from dev2.sec_lang sl_orig where sl_orig.iso_639_3_code = dev2.coop.language_code)), 1), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.coop.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.coop.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.coop.userid)) from dev2.coop;

/* cooperator_group */
select 'copying data to cooperator_group ...' as Action;
insert into dev5.cooperator_group (name, site_code, is_historical, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select cgname, site, coalesce(historical,'N'), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cg.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cg.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cg.userid)) from dev2.cg;

/* cooperator_member */
select 'copying data to cooperator_member ...' as Action;
insert into dev5.cooperator_member (cooperator_id, cooperator_group_id, note, localid, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__cooperator where previous_id = dev2.mbr.cno), (select new_id from dev5.__cooperator_group where previous_id = (select cg_orig.cgid_int from dev2.cg cg_orig where cg_orig.cgid = dev2.mbr.cgid)), cmt, localid, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mbr.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mbr.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mbr.userid)) from dev2.mbr;

/* crop */
select 'copying data to crop ...' as Action;
insert into dev5.crop (name, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select crop, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.crop.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.crop.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.crop.userid)) from dev2.crop;

/* evaluation */
select 'copying data to evaluation ...' as Action;
insert into dev5.evaluation (name, site_code, geography_id, material_or_method_used, study_reason, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select ename, site, (select new_id from dev5.__geography where previous_id = dev2.eval.geono), methods, studytype, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.eval.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.eval.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.eval.userid)) from dev2.eval;

/* evaluation_citation */
select 'copying data to evaluation_citation ...' as Action;
insert into dev5.evaluation_citation (evaluation_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__evaluation where previous_id = dev2.ecit.eno), (select new_id from dev5.__literature where previous_id = (select lit_orig.litid from dev2.lit lit_orig where lit_orig.abbr = dev2.ecit.abbr)), cittitle, author, STR_TO_DATE(CONCAT(cityr, '0101'), '%Y%m%d'), citref, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ecit.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ecit.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ecit.userid)) from dev2.ecit;

/* evaluation_member */
select 'copying data to evaluation_member ...' as Action;
insert into dev5.evaluation_member (cooperator_id, evaluation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__cooperator where previous_id = dev2.embr.cno), (select new_id from dev5.__evaluation where previous_id = dev2.embr.eno), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.embr.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.embr.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.embr.userid)) from dev2.embr;

/* family */
select 'copying data to family ...' as Action;
insert into dev5.family (current_family_id, faimly_name, author_name, alternate_name, subfamily, tribe, subtribe, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__family where previous_id = dev2.fam.validfamno), family, famauthor, altfamily, subfamily, tribe, subtribe, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.fam.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.fam.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.fam.userid)) from dev2.fam;

/* genomic_annotation */
select 'copying data to genomic_annotation ...' as Action;
insert into dev5.genomic_annotation (marker_id, evaluation_id, method, scoring_method, control_values, observation_alleles_count, max_gob_alleles, size_alleles, unusual_alleles, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__genomic_marker where previous_id = dev2.ga.mrkno), (select new_id from dev5.__evaluation where previous_id = dev2.ga.eno), method, scoring_method, control_values, no_obs_alleles, max_gob_alleles, size_alleles, unusual_alleles, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ga.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ga.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ga.userid)) from dev2.ga;

/* genomic_marker */
select 'copying data to genomic_marker ...' as Action;
insert into dev5.genomic_marker (crop_id, site_code, name, synonyms, repeat_motif, primers, assay_conditions, range_products, known_standards, genebank_number, map_location, position, note, poly_type, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__crop where previous_id = dev2.mrk.cropno), site, marker, synonyms, repeat_motif, primers, assay_conditions, range_products, known_standards, genbank_no, map_location, position, cmt, poly_type, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mrk.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mrk.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mrk.userid)) from dev2.mrk;

/* genomic_marker_citation */
select 'copying data to genomic_marker_citation ...' as Action;
insert into dev5.genomic_marker_citation (genomic_marker_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__genomic_marker where previous_id = dev2.mcit.mrkno), (select new_id from dev5.__literature where previous_id = (select lit_orig.litid from dev2.lit lit_orig where lit_orig.abbr = dev2.mcit.abbr)), cittitle, author, STR_TO_DATE(CONCAT(cityr, '0101'), '%Y%m%d'), citref, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mcit.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mcit.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.mcit.userid)) from dev2.mcit;

/* genomic_observation */
select 'copying data to genomic_observation ...' as Action;
insert into dev5.genomic_observation (genomic_annotation_id, inventory_id, individual, value, genebank_url, image_url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__genomic_annotation where previous_id = dev2.gob.gano), (select new_id from dev5.__inventory where previous_id = dev2.gob.ivid), indiv, gob, genbank_link, image_link, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gob.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gob.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gob.userid)) from dev2.gob;

/* genus */
select 'copying data to genus ...' as Action;
insert into dev5.genus (current_genus_id, qualifying_code, is_hybrid, genus_name, genus_authority, subgenus_name, section_name, series_name, subseries_name, family_id, alternate_family, common_name, note, subsection_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__genus where previous_id = dev2.gn.validgno), qual, coalesce(ghybrid,'N'), genus, gauthor, subgenus, section, series, subseries, (select new_id from dev5.__family where previous_id = dev2.gn.famno), othfamily, cname, cmt, subsection, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gn.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gn.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gn.userid)) from dev2.gn;

/* genus_citation */
select 'copying data to genus_citation ...' as Action;
insert into dev5.genus_citation (genus_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__genus where previous_id = dev2.gcit.gno), (select new_id from dev5.__literature where previous_id = (select lit_orig.litid from dev2.lit lit_orig where lit_orig.abbr = dev2.gcit.abbr)), cittitle, author, STR_TO_DATE(CONCAT(cityr, '0101'), '%Y%m%d'), citref, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gcit.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gcit.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gcit.userid)) from dev2.gcit;

/* genus_type */
select 'copying data to genus_type ...' as Action;
insert into dev5.genus_type (family_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__family where previous_id = dev2.gnt.famno), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gnt.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gnt.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.gnt.userid)) from dev2.gnt;

/* geography */
select 'copying data to geography ...' as Action;
insert into dev5.geography (current_geography_id, country_name, state_name, country_iso_full_name, country_iso_short_name, state_full_name, iso_3_char_country_code, iso_2_char_country_code, state_code, is_valid, latitude_hemisphere, longitude_hemisphere, region_id, changed_date, previous_name, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__geography where previous_id = dev2.geo.validgeono), country, state, isofull, isoshort, statefull, iso3, iso2, st, coalesce(cflag,'N'), lath, lonh, (select new_id from dev5.__region where previous_id = dev2.geo.regno), changed, oldname, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.geo.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.geo.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.geo.userid)) from dev2.geo;

/* inventory */
select 'copying data to inventory ...' as Action;
insert into dev5.inventory (inventory_prefix, inventory_number, inventory_suffix, inventory_type_code, inventory_maintenance_id, site_code, is_distributable, location_section_1, location_section_2, location_section_3, location_section_4, quantity_on_hand, unit_of_quantity_on_hand, is_debit, distribution_default_form, standard_distribution_quantity, unit_of_distribution, distribution_critical_amount, replenishment_critical_amount, pathogen_status, availability_status, status_note, accession_id, parent_inventory_id, cooperator_id, backup_inventory_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select ivp, ivno, ivs, ivt, (select new_id from dev5.__inventory_maintenance where previous_id = (select im_orig.imid from dev2.im im_orig where im_orig.imname = dev2.iv.imname and im_orig.site = dev2.iv.SITE)), site, coalesce(distribute,'N'), loc1, loc2, loc3, loc4, onhand, munits, coalesce(debit,'N'), dform, dquant, dunits, dcritical, rcritical, pstatus, status, statcmt, (select new_id from dev5.__accession where previous_id = dev2.iv.acid), (select new_id from dev5.__inventory where previous_id = dev2.iv.parent), (select new_id from dev5.__cooperator where previous_id = dev2.iv.cno), (select new_id from dev5.__inventory where previous_id = dev2.iv.backupiv), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.iv.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.iv.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.iv.userid)) from dev2.iv;

/* inventory_action */
select 'copying data to inventory_action ...' as Action;
insert into dev5.inventory_action (action_name, occurred_date, occurred_date_format, quantity, unit_of_quantity, form_involved, inventory_id, cooperator_id, evaluation_id, note, qualifier, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select action, occurred, datefmt, quant, units, iform, (select new_id from dev5.__inventory where previous_id = dev2.iact.ivid), (select new_id from dev5.__cooperator where previous_id = dev2.iact.cno), (select new_id from dev5.__evaluation where previous_id = dev2.iact.eno), cmt, iactqual, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.iact.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.iact.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.iact.userid)) from dev2.iact;

/* inventory_group */
select 'copying data to inventory_group ...' as Action;
insert into dev5.inventory_group (group_name, site_code, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select igname, site, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ig.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ig.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ig.userid)) from dev2.ig;

/* inventory_group_maintenance */
select 'copying data to inventory_group_maintenance ...' as Action;
insert into dev5.inventory_group_maintenance (inventory_id, inventory_group_id, site_code, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__inventory where previous_id = dev2.igm.ivid), (select new_id from dev5.__inventory_group where previous_id = (select ig_orig.igid from dev2.ig ig_orig where ig_orig.igname = dev2.igm.igname and ig_orig.site = dev2.igm.SITE)), site, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.igm.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.igm.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.igm.userid)) from dev2.igm;

/* inventory_maintenance */
select 'copying data to inventory_maintenance ...' as Action;
insert into dev5.inventory_maintenance (maintenance_name, site_code, inventory_default_form, unit_of_maintenance, is_debit, distribution_default_form, standard_distribution_quantity, unit_of_distribution, distribution_critical_amount, replenishment_critical_amount, regeneration_method, standard_pathogen_test_count, note, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select imname, site, ivt, munits, coalesce(debit,'N'), dform, dquant, dunits, dcritical, rcritical, regen, ptests, cmt, (select new_id from dev5.__cooperator where previous_id = dev2.im.cno), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.im.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.im.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.im.userid)) from dev2.im;

/* inventory_pathogen_test */
select 'copying data to inventory_pathogen_test ...' as Action;
insert into dev5.inventory_pathogen_test (inventory_id, test_type, pathogen_code, started_date, finished_date, test_results, needed_count, started_count, completed_count, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__inventory where previous_id = dev2.pt.ivid), pttype, ptcode, began, finished, results, needed, started, completed, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.pt.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.pt.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.pt.userid)) from dev2.pt;

/* inventory_viability */
select 'copying data to inventory_viability ...' as Action;
insert into dev5.inventory_viability (tested_date, tested_date_format, percent_normal, percent_abnormal, percent_dormant, percent_viable, vigor_rating, sample_count, replication_count, inventory_id, evaluation_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select tested, datefmt, norm, abnorm, dormant, viable, vigor, sample, reps, (select new_id from dev5.__inventory where previous_id = dev2.via.ivid), (select new_id from dev5.__evaluation where previous_id = dev2.via.eno), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.via.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.via.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.via.userid)) from dev2.via;

/* literature */
select 'copying data to literature ...' as Action;
insert into dev5.literature (abbreviation, standard_abbreviation, reference_title, author_editor_name, note, site_code, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select abbr, stdabbr, reftitle, editor, cmt, site, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.lit.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.lit.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.lit.userid)) from dev2.lit;

/* order_entry */
select 'copying data to order_entry ...' as Action;
insert into dev5.order_entry (original_order_entry_id, site_code, local_number, order_type, ordered_date, status, is_completed, acted_date, source_cooperator_id, requestor_cooperator_id, ship_to_cooperator_id, final_recipient_cooperator_id, order_obtained_via, is_supply_low, note, special_instruction, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__order_entry where previous_id = dev2.ord.origno), site, localno, ortype, ordered, status, coalesce(done,'N'), acted, (select new_id from dev5.__cooperator where previous_id = dev2.ord.source), (select new_id from dev5.__cooperator where previous_id = dev2.ord.orderer), (select new_id from dev5.__cooperator where previous_id = dev2.ord.shipto), (select new_id from dev5.__cooperator where previous_id = dev2.ord.final), reqref, coalesce(supplylow,'N'), cmt, request, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ord.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ord.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ord.userid)) from dev2.ord;

/* order_entry_action */
select 'copying data to order_entry_action ...' as Action;
insert into dev5.order_entry_action (action_name, acted_date, action_for_id, order_entry_id, site_code, note, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select action, acted, actid, (select new_id from dev5.__order_entry where previous_id = dev2.oact.orno), site, cmt, cno, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.oact.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.oact.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.oact.userid)) from dev2.oact;

/* order_entry_item */
select 'copying data to order_entry_item ...' as Action;
insert into dev5.order_entry_item (order_entry_id, item_sequence_number, item_name, quantity_shipped, unit_of_shipped, distribution_form, ipr_restriction, item_status, acted_date, cooperator_id, inventory_id, accession_id, taxonomy_id, external_taxonomy, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__order_entry where previous_id = dev2.oi.orno), oino, item, quant, units, dform, rest, status, acted, (select new_id from dev5.__cooperator where previous_id = dev2.oi.cno), (select new_id from dev5.__inventory where previous_id = dev2.oi.ivid), (select new_id from dev5.__accession where previous_id = dev2.oi.acid), (select new_id from dev5.__taxonomy where previous_id = dev2.oi.taxno), taxon, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.oi.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.oi.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.oi.userid)) from dev2.oi;

/* plant_introduction */
select 'copying data to plant_introduction ...' as Action;
insert into dev5.plant_introduction (plant_introduction_year_date, lowest_pi_number, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select STR_TO_DATE(CONCAT(piyear, '0101'), '%Y%m%d'), lowpi, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.pi.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.pi.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.pi.userid)) from dev2.pi;

/* region */
select 'copying data to region ...' as Action;
insert into dev5.region (continent, subcontinent, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select area, region, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.reg.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.reg.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.reg.userid)) from dev2.reg;

/* sec_lang */
select 'copying data to sec_lang ...' as Action;
insert into dev5.sec_lang (iso_639_3_code, ietf_tag, language_name, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select iso_639_3_code, ietf_tag, language_name, description, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_lang;

/* sec_perm */
select 'copying data to sec_perm ...' as Action;
insert into dev5.sec_perm (table_name, sec_perm_template_id, is_enabled, perm_type, perm_value, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select table_name, (select new_id from dev5.__sec_perm_template where previous_id = dev2.sec_perm.sec_perm_template_id), coalesce(enabled,'N'), perm_type, perm_value, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_perm;

/* sec_perm_field */
select 'copying data to sec_perm_field ...' as Action;
insert into dev5.sec_perm_field (sec_perm_id, field_name, field_type, compare_operator, compare_value, compare_field_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_perm where previous_id = dev2.sec_perm_field.sec_perm_id), field_name, field_type, compare_operator, compare_value, compare_field_name, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_perm_field;

/* sec_perm_template */
select 'copying data to sec_perm_template ...' as Action;
insert into dev5.sec_perm_template (template_name, is_enabled, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select template_name, coalesce(enabled,'N'), '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_perm_template;

/* sec_perm_template_map */
select 'copying data to sec_perm_template_map ...' as Action;
insert into dev5.sec_perm_template_map (sec_perm_template_id, sec_perm_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_perm_template where previous_id = dev2.sec_perm_template_map.sec_perm_template_id), (select new_id from dev5.__sec_perm where previous_id = dev2.sec_perm_template_map.sec_perm_id), '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_perm_template_map;

/* sec_rs */
select 'copying data to sec_rs ...' as Action;
insert into dev5.sec_rs (rs_name, sql_statement, is_enabled, is_updateable, description, is_system, suppress_properties, is_user_visible, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select rs_name, sql_statement, coalesce(enabled,'N'), coalesce(updateable,'N'), description, coalesce(is_system,'N'), coalesce(suppress_props,'N'), coalesce(is_user_visible,'N'), '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_rs;

/* sec_rs_field */
select 'copying data to sec_rs_field ...' as Action;
insert into dev5.sec_rs_field (sec_rs_id, field_name, sec_table_field_id, is_updateable, sort_order, foreign_key_resultset_name, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_rs where previous_id = dev2.sec_rs_field.sec_rs_id), field_name, (select new_id from dev5.__sec_table_field where previous_id = dev2.sec_rs_field.sec_table_field_id), coalesce(updateable,'N'), sort_order, foreign_key_resultset_name, description, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_rs_field;

/* sec_rs_field_friendly */
select 'copying data to sec_rs_field_friendly ...' as Action;
insert into dev5.sec_rs_field_friendly (sec_rs_field_id, sec_lang_id, friendly_field_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_rs_field where previous_id = dev2.sec_rs_field_friendly.sec_rs_field_id), coalesce((select new_id from dev5.__sec_lang where previous_id = (select sl_orig.sec_lang_id from dev2.sec_lang sl_orig where sl_orig.iso_639_3_code = dev2.sec_rs_field_friendly.language_code)), 1), friendly_field_name, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_rs_field_friendly;

/* sec_rs_param */
select 'copying data to sec_rs_param ...' as Action;
insert into dev5.sec_rs_param (sec_rs_id, param_name, param_type, sort_order, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_rs where previous_id = dev2.sec_rs_param.sec_rs_id), param_name, param_type, sort_order, description, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_rs_param;

/* sec_table */
select 'copying data to sec_table ...' as Action;
insert into dev5.sec_table (table_name, is_enabled, is_updateable, audits_created, audits_modified, audits_owned, database_area, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select table_name, coalesce(enabled,'N'), coalesce(updateable,'N'), audits_created, audits_modified, audits_owned, database_area, description, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_table;

/* sec_table_field */
select 'copying data to sec_table_field ...' as Action;
insert into dev5.sec_table_field (sec_table_id, field_name, field_purpose, field_type, description, is_primary_key, is_foreign_key, foreign_key_field_id, foreign_key_resultset_name, is_nullable, gui_hint, is_readonly, min_length, max_length, numeric_precision, numeric_scale, is_autoincrement, code_group_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_table where previous_id = dev2.sec_table_field.sec_table_id), field_name, field_purpose, field_type, description, coalesce(is_primary_key,'N'), coalesce(is_foreign_key,'N'), foreign_key_field_id, foreign_key_resultset_name, coalesce(is_nullable,'N'), gui_hint, coalesce(is_readonly,'N'), min_length, max_length, numeric_precision, numeric_scale, coalesce(is_autoincrement,'N'), (select new_id from dev5.__code_group where previous_id = dev2.sec_table_field.lookup_code_no), '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_table_field;

/* sec_user */
select 'copying data to sec_user ...' as Action;
insert into dev5.sec_user (user_name, password, is_enabled, cooperator_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select user_name, password, coalesce(enabled,'N'), (select new_id from dev5.__cooperator where previous_id = dev2.sec_user.cno), '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_user;

/* sec_user_gui_setting */
select 'copying data to sec_user_gui_setting ...' as Action;
insert into dev5.sec_user_gui_setting (cooperator_id, resource_name, resource_key, resource_value, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__cooperator where previous_id = dev2.sec_user_gui_settings.cno), resource_name, resource_key, resource_value, '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_user_gui_settings;

/* sec_user_perm */
select 'copying data to sec_user_perm ...' as Action;
insert into dev5.sec_user_perm (sec_user_id, sec_perm_id, is_enabled, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__sec_user where previous_id = dev2.sec_user_perm.sec_user_id), (select new_id from dev5.__sec_perm where previous_id = dev2.sec_user_perm.sec_perm_id), coalesce(enabled,'N'), '20090106', 1, '20090106', 1, '20090106', 1 from dev2.sec_user_perm;

/* taxonomy */
select 'copying data to taxonomy ...' as Action;
insert into dev5.taxonomy (current_taxonomy_id, is_interspecific_hybrid, species, species_authority, is_intraspecific_hybrid, subspecies, subspecies_authority, is_intervarietal_hybrid, variety, variety_authority, is_subvarietal_hybrid, subvariety, subvariety_authority, is_forma_hybrid, forma, forma_authority, genus_id, crop_id, priority_site_1, priority_site_2, restriction, life_form, common_fertilization, is_name_pending, synonym_code, cooperator_id, name_verified_date, name, name_authority, protologue, note, site_note, alternate_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__taxonomy where previous_id = dev2.tax.validtaxno), coalesce(shybrid,'N'), species, sauthor, coalesce(ssphybrid,'N'), subsp, sspauthor, coalesce(varhybrid,'N'), var, varauthor, coalesce(svhybrid,'N'), subvar, svauthor, coalesce(fhybrid,'N'), forma, fauthor, (select new_id from dev5.__genus where previous_id = dev2.tax.gno), (select new_id from dev5.__crop where previous_id = dev2.tax.cropno), psite1, psite2, rest, lifeform, fert, coalesce(pending,'N'), qual, (select new_id from dev5.__cooperator where previous_id = dev2.tax.cno), verified, taxon, taxauthor, protologue, taxcmt, sitecmt, othname, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.tax.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.tax.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.tax.userid)) from dev2.tax;

/* taxonomy_author */
select 'copying data to taxonomy_author ...' as Action;
insert into dev5.taxonomy_author (short_name, full_name, short_name_diacritic, full_name_diacritic, short_name_expanded_diacritic, full_name_expanded_diacritic, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select shortaut, longaut, smarkaut, lmarkaut, shexpaut, lgexpaut, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.taut.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.taut.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.taut.userid)) from dev2.taut;

/* taxonomy_citation */
select 'copying data to taxonomy_citation ...' as Action;
insert into dev5.taxonomy_citation (taxonomy_id, literature_id, title, author_name, citation_year_date, reference, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__taxonomy where previous_id = dev2.tcit.taxno), (select new_id from dev5.__literature where previous_id = (select lit_orig.litid from dev2.lit lit_orig where lit_orig.abbr = dev2.tcit.abbr)), cittitle, author, STR_TO_DATE(CONCAT(cityr, '0101'), '%Y%m%d'), citref, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.tcit.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.tcit.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.tcit.userid)) from dev2.tcit;

/* taxonomy_common_name */
select 'copying data to taxonomy_common_name ...' as Action;
insert into dev5.taxonomy_common_name (taxonomy_id, name, source, note, simplified_name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__taxonomy where previous_id = dev2.cn.taxno), cname, source, cmt, cnid, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cn.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cn.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cn.userid)) from dev2.cn;

/* taxonomy_distribution */
select 'copying data to taxonomy_distribution ...' as Action;
insert into dev5.taxonomy_distribution (taxonomy_id, geography_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__taxonomy where previous_id = dev2.dist.taxno), (select new_id from dev5.__geography where previous_id = dev2.dist.geono), cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.dist.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.dist.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.dist.userid)) from dev2.dist;

/* taxonomy_germination_rule */
select 'copying data to taxonomy_germination_rule ...' as Action;
insert into dev5.taxonomy_germination_rule (substrata, temperature_range, requirements, author_name, category, days, taxonomy_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select substrata, temp, requirements, author, category, days, (select new_id from dev5.__taxonomy where previous_id = dev2.germrule.taxno), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.germrule.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.germrule.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.germrule.userid)) from dev2.germrule;

/* taxonomy_url */
select 'copying data to taxonomy_url ...' as Action;
insert into dev5.taxonomy_url (url_type, family_id, genus_id, taxonomy_id, caption, url, site_code, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select urltype, (select new_id from dev5.__family where previous_id = dev2.turl.famno), (select new_id from dev5.__genus where previous_id = dev2.turl.gno), (select new_id from dev5.__taxonomy where previous_id = dev2.turl.taxno), caption, url, site, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.turl.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.turl.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.turl.userid)) from dev2.turl;

/* taxonomy_use */
select 'copying data to taxonomy_use ...' as Action;
insert into dev5.taxonomy_use (taxonomy_id, economic_usage, note, usage_type, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__taxonomy where previous_id = dev2.uses.taxno), taxuse, cmt, usetype, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.uses.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.uses.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.uses.userid)) from dev2.uses;

/* trait */
select 'copying data to trait ...' as Action;
insert into dev5.trait (short_name, name, is_cgc_approved, category_code, data_type, is_coded, max_length, numeric_format, numeric_max, numeric_min, original_value_type, original_value_format, crop_id, site_code, definition, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select dqname, dname, coalesce(cac,'N'), dcat, obtype, coalesce(usecode,'N'), obmaxlen, obformat, obmax, obmin, orgtype, orgformat, (select new_id from dev5.__crop where previous_id = dev2.dsc.cropno), site, def, cmt, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.dsc.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.dsc.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.dsc.userid)) from dev2.dsc;

/* trait_code */
select 'copying data to trait_code ...' as Action;
insert into dev5.trait_code (trait_id, code, definition, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__trait where previous_id = dev2.cd.dno), code, def, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cd.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cd.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.cd.userid)) from dev2.cd;

/* trait_code_friendly **************** No previous definition **************** */

select 'copying language-specific trait_code data into trait_code_friendly' as Action;

insert into dev5.trait_code_friendly
	(trait_code_id, sec_lang_id, friendly_name, friendly_description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	(select new_id from dev5.__trait_code where previous_id = tc_orig.cdid),
	(select sec_lang_id from dev5.sec_lang where iso_639_3_code = 'ENG'),
	tc_orig.code,
	tc_orig.def,
	(select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),
	'20090106',
	(select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),
	'20090106',
	(select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),
	'20090106'
from
	dev2.cd tc_orig

/* trait_observation */
select 'copying data to trait_observation ...' as Action;
insert into dev5.trait_observation (trait_id, trait_code_id, accession_id, evaluation_id, qualifier_id, inventory_id, original_value, frequency, mean_value, maximum_value, minimum_value, standard_deviation, sample_size, note, rank, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select (select new_id from dev5.__trait where previous_id = dev2.ob.dno), (select new_id from dev5.__trait_code where previous_id = dev2.ob.ob), (select new_id from dev5.__accession where previous_id = dev2.ob.acid), (select new_id from dev5.__evaluation where previous_id = dev2.ob.eno), (select new_id from dev5.__trait_qualifier where previous_id = dev2.ob.qno), (select new_id from dev5.__inventory where previous_id = dev2.ob.ivid), orgvalue, freq, mean, high, low, sdev, ssize, cmt, rank, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ob.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ob.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.ob.userid)) from dev2.ob;

/* trait_qualifier */
select 'copying data to trait_qualifier ...' as Action;
insert into dev5.trait_qualifier (trait_qualifier_name, trait_id, definition, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select qual, (select new_id from dev5.__trait where previous_id = dev2.qual.dno), def, created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.qual.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.qual.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.qual.userid)) from dev2.qual;

/* trait_url */
select 'copying data to trait_url ...' as Action;
insert into dev5.trait_url (url_type, sequence_number, crop_id, trait_id, code, caption, url, site_code, note, evaluation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select urltype, seqno, (select new_id from dev5.__crop where previous_id = dev2.durl.cropno), (select new_id from dev5.__trait where previous_id = dev2.durl.dno), code, caption, url, site, cmt, (select new_id from dev5.__evaluation where previous_id = dev2.durl.eno), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.durl.userid)), modified, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.durl.userid)), created, (select new_id from dev5.__cooperator where previous_id = (select su_orig.cno from dev2.siteuser su_orig where su_orig.siteuser = dev2.durl.userid)) from dev2.durl;


/***********************************************/
/******** Turn On Unique Checks       **********/
/***********************************************/
SET UNIQUE_CHECKS=1;
/***********************************************/
/******** Converting tables to InnoDB **********/
/***********************************************/
select 'Changing engine for dev5.accession to InnoDB ...' as Action;
ALTER TABLE dev5.accession ENGINE=InnoDB;

select 'Changing engine for dev5.accession_action to InnoDB ...' as Action;
ALTER TABLE dev5.accession_action ENGINE=InnoDB;

select 'Changing engine for dev5.accession_annotation to InnoDB ...' as Action;
ALTER TABLE dev5.accession_annotation ENGINE=InnoDB;

select 'Changing engine for dev5.accession_citation to InnoDB ...' as Action;
ALTER TABLE dev5.accession_citation ENGINE=InnoDB;

select 'Changing engine for dev5.accession_group to InnoDB ...' as Action;
ALTER TABLE dev5.accession_group ENGINE=InnoDB;

select 'Changing engine for dev5.accession_habitat to InnoDB ...' as Action;
ALTER TABLE dev5.accession_habitat ENGINE=InnoDB;

select 'Changing engine for dev5.accession_name to InnoDB ...' as Action;
ALTER TABLE dev5.accession_name ENGINE=InnoDB;

select 'Changing engine for dev5.accession_narrative to InnoDB ...' as Action;
ALTER TABLE dev5.accession_narrative ENGINE=InnoDB;

select 'Changing engine for dev5.accession_pedigree to InnoDB ...' as Action;
ALTER TABLE dev5.accession_pedigree ENGINE=InnoDB;

select 'Changing engine for dev5.accession_quarantine to InnoDB ...' as Action;
ALTER TABLE dev5.accession_quarantine ENGINE=InnoDB;

select 'Changing engine for dev5.accession_right to InnoDB ...' as Action;
ALTER TABLE dev5.accession_right ENGINE=InnoDB;

select 'Changing engine for dev5.accession_source to InnoDB ...' as Action;
ALTER TABLE dev5.accession_source ENGINE=InnoDB;

select 'Changing engine for dev5.accession_source_member to InnoDB ...' as Action;
ALTER TABLE dev5.accession_source_member ENGINE=InnoDB;

select 'Changing engine for dev5.accession_voucher to InnoDB ...' as Action;
ALTER TABLE dev5.accession_voucher ENGINE=InnoDB;

select 'Changing engine for dev5.accession_voucher_image to InnoDB ...' as Action;
ALTER TABLE dev5.accession_voucher_image ENGINE=InnoDB;

select 'Changing engine for dev5.app_resource to InnoDB ...' as Action;
ALTER TABLE dev5.app_resource ENGINE=InnoDB;

select 'Changing engine for dev5.code_group to InnoDB ...' as Action;
ALTER TABLE dev5.code_group ENGINE=InnoDB;

select 'Changing engine for dev5.code_rule to InnoDB ...' as Action;
ALTER TABLE dev5.code_rule ENGINE=InnoDB;

select 'Changing engine for dev5.code_value to InnoDB ...' as Action;
ALTER TABLE dev5.code_value ENGINE=InnoDB;

select 'Changing engine for dev5.code_value_friendly to InnoDB ...' as Action;
ALTER TABLE dev5.code_value_friendly ENGINE=InnoDB;

select 'Changing engine for dev5.cooperator to InnoDB ...' as Action;
ALTER TABLE dev5.cooperator ENGINE=InnoDB;

select 'Changing engine for dev5.cooperator_group to InnoDB ...' as Action;
ALTER TABLE dev5.cooperator_group ENGINE=InnoDB;

select 'Changing engine for dev5.cooperator_member to InnoDB ...' as Action;
ALTER TABLE dev5.cooperator_member ENGINE=InnoDB;

select 'Changing engine for dev5.crop to InnoDB ...' as Action;
ALTER TABLE dev5.crop ENGINE=InnoDB;

select 'Changing engine for dev5.evaluation to InnoDB ...' as Action;
ALTER TABLE dev5.evaluation ENGINE=InnoDB;

select 'Changing engine for dev5.evaluation_citation to InnoDB ...' as Action;
ALTER TABLE dev5.evaluation_citation ENGINE=InnoDB;

select 'Changing engine for dev5.evaluation_member to InnoDB ...' as Action;
ALTER TABLE dev5.evaluation_member ENGINE=InnoDB;

select 'Changing engine for dev5.family to InnoDB ...' as Action;
ALTER TABLE dev5.family ENGINE=InnoDB;

select 'Changing engine for dev5.genomic_annotation to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_annotation ENGINE=InnoDB;

select 'Changing engine for dev5.genomic_marker to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_marker ENGINE=InnoDB;

select 'Changing engine for dev5.genomic_marker_citation to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_marker_citation ENGINE=InnoDB;

select 'Changing engine for dev5.genomic_observation to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_observation ENGINE=InnoDB;

select 'Changing engine for dev5.genus to InnoDB ...' as Action;
ALTER TABLE dev5.genus ENGINE=InnoDB;

select 'Changing engine for dev5.genus_citation to InnoDB ...' as Action;
ALTER TABLE dev5.genus_citation ENGINE=InnoDB;

select 'Changing engine for dev5.genus_type to InnoDB ...' as Action;
ALTER TABLE dev5.genus_type ENGINE=InnoDB;

select 'Changing engine for dev5.geography to InnoDB ...' as Action;
ALTER TABLE dev5.geography ENGINE=InnoDB;

select 'Changing engine for dev5.inventory to InnoDB ...' as Action;
ALTER TABLE dev5.inventory ENGINE=InnoDB;

select 'Changing engine for dev5.inventory_action to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_action ENGINE=InnoDB;

select 'Changing engine for dev5.inventory_group to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_group ENGINE=InnoDB;

select 'Changing engine for dev5.inventory_group_maintenance to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_group_maintenance ENGINE=InnoDB;

select 'Changing engine for dev5.inventory_maintenance to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_maintenance ENGINE=InnoDB;

select 'Changing engine for dev5.inventory_pathogen_test to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_pathogen_test ENGINE=InnoDB;

select 'Changing engine for dev5.inventory_viability to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_viability ENGINE=InnoDB;

select 'Changing engine for dev5.literature to InnoDB ...' as Action;
ALTER TABLE dev5.literature ENGINE=InnoDB;

select 'Changing engine for dev5.order_entry to InnoDB ...' as Action;
ALTER TABLE dev5.order_entry ENGINE=InnoDB;

select 'Changing engine for dev5.order_entry_action to InnoDB ...' as Action;
ALTER TABLE dev5.order_entry_action ENGINE=InnoDB;

select 'Changing engine for dev5.order_entry_item to InnoDB ...' as Action;
ALTER TABLE dev5.order_entry_item ENGINE=InnoDB;

select 'Changing engine for dev5.plant_introduction to InnoDB ...' as Action;
ALTER TABLE dev5.plant_introduction ENGINE=InnoDB;

select 'Changing engine for dev5.region to InnoDB ...' as Action;
ALTER TABLE dev5.region ENGINE=InnoDB;

select 'Changing engine for dev5.sec_lang to InnoDB ...' as Action;
ALTER TABLE dev5.sec_lang ENGINE=InnoDB;

select 'Changing engine for dev5.sec_perm to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm ENGINE=InnoDB;

select 'Changing engine for dev5.sec_perm_field to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm_field ENGINE=InnoDB;

select 'Changing engine for dev5.sec_perm_template to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm_template ENGINE=InnoDB;

select 'Changing engine for dev5.sec_perm_template_map to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm_template_map ENGINE=InnoDB;

select 'Changing engine for dev5.sec_rs to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs ENGINE=InnoDB;

select 'Changing engine for dev5.sec_rs_field to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs_field ENGINE=InnoDB;

select 'Changing engine for dev5.sec_rs_field_friendly to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs_field_friendly ENGINE=InnoDB;

select 'Changing engine for dev5.sec_rs_param to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs_param ENGINE=InnoDB;

select 'Changing engine for dev5.sec_table to InnoDB ...' as Action;
ALTER TABLE dev5.sec_table ENGINE=InnoDB;

select 'Changing engine for dev5.sec_table_field to InnoDB ...' as Action;
ALTER TABLE dev5.sec_table_field ENGINE=InnoDB;

select 'Changing engine for dev5.sec_user to InnoDB ...' as Action;
ALTER TABLE dev5.sec_user ENGINE=InnoDB;

select 'Changing engine for dev5.sec_user_gui_setting to InnoDB ...' as Action;
ALTER TABLE dev5.sec_user_gui_setting ENGINE=InnoDB;

select 'Changing engine for dev5.sec_user_perm to InnoDB ...' as Action;
ALTER TABLE dev5.sec_user_perm ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_author to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_author ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_citation to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_citation ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_common_name to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_common_name ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_distribution to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_distribution ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_germination_rule to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_germination_rule ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_url to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_url ENGINE=InnoDB;

select 'Changing engine for dev5.taxonomy_use to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_use ENGINE=InnoDB;

select 'Changing engine for dev5.trait to InnoDB ...' as Action;
ALTER TABLE dev5.trait ENGINE=InnoDB;

select 'Changing engine for dev5.trait_code to InnoDB ...' as Action;
ALTER TABLE dev5.trait_code ENGINE=InnoDB;

select 'Changing engine for dev5.trait_code_friendly to InnoDB ...' as Action;
ALTER TABLE dev5.trait_code_friendly ENGINE=InnoDB;

select 'Changing engine for dev5.trait_observation to InnoDB ...' as Action;
ALTER TABLE dev5.trait_observation ENGINE=InnoDB;

select 'Changing engine for dev5.trait_qualifier to InnoDB ...' as Action;
ALTER TABLE dev5.trait_qualifier ENGINE=InnoDB;

select 'Changing engine for dev5.trait_url to InnoDB ...' as Action;
ALTER TABLE dev5.trait_url ENGINE=InnoDB;

select 'Adding constraints for dev5.accession to InnoDB ...' as Action;
ALTER TABLE dev5.accession  
 ADD CONSTRAINT `fk_a_an` FOREIGN KEY `ndx_fk_a_an` (`accession_name_id`) REFERENCES `dev5`.`accession_name` (`accession_name_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_a_t` FOREIGN KEY `ndx_fk_a_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_a_pi` FOREIGN KEY `ndx_fk_a_pi` (`plant_introduction_id`) REFERENCES `dev5`.`plant_introduction` (`plant_introduction_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_a_created` FOREIGN KEY `ndx_fk_a_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_a_modified` FOREIGN KEY `ndx_fk_a_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_a_owned` FOREIGN KEY `ndx_fk_a_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_action to InnoDB ...' as Action;
ALTER TABLE dev5.accession_action  
 ADD CONSTRAINT `fk_aa_a` FOREIGN KEY `ndx_fk_aa_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aa_c` FOREIGN KEY `ndx_fk_aa_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aa_e` FOREIGN KEY `ndx_fk_aa_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aa_created` FOREIGN KEY `ndx_fk_aa_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aa_modified` FOREIGN KEY `ndx_fk_aa_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aa_owned` FOREIGN KEY `ndx_fk_aa_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_annotation to InnoDB ...' as Action;
ALTER TABLE dev5.accession_annotation  
 ADD CONSTRAINT `fk_aan_a` FOREIGN KEY `ndx_fk_aan_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_c` FOREIGN KEY `ndx_fk_aan_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_i` FOREIGN KEY `ndx_fk_aan_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_oe` FOREIGN KEY `ndx_fk_aan_oe` (`order_entry_id`) REFERENCES `dev5`.`order_entry` (`order_entry_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_t_old` FOREIGN KEY `ndx_fk_aan_t_old` (`old_taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_t_new` FOREIGN KEY `ndx_fk_aan_t_new` (`new_taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_created` FOREIGN KEY `ndx_fk_aan_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_modified` FOREIGN KEY `ndx_fk_aan_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aan_owned` FOREIGN KEY `ndx_fk_aan_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_citation to InnoDB ...' as Action;
ALTER TABLE dev5.accession_citation  
 ADD CONSTRAINT `fk_ac_a` FOREIGN KEY `ndx_fk_ac_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ac_l` FOREIGN KEY `ndx_fk_ac_l` (`literature_id`) REFERENCES `dev5`.`literature` (`literature_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ac_created` FOREIGN KEY `ndx_fk_ac_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ac_modified` FOREIGN KEY `ndx_fk_ac_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ac_owned` FOREIGN KEY `ndx_fk_ac_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_group to InnoDB ...' as Action;
ALTER TABLE dev5.accession_group  
 ADD CONSTRAINT `fk_ag_created` FOREIGN KEY `ndx_fk_ag_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ag_modified` FOREIGN KEY `ndx_fk_ag_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ag_owned` FOREIGN KEY `ndx_fk_ag_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_habitat to InnoDB ...' as Action;
ALTER TABLE dev5.accession_habitat  
 ADD CONSTRAINT `fk_ah_a` FOREIGN KEY `ndx_fk_ah_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ah_created` FOREIGN KEY `ndx_fk_ah_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ah_modified` FOREIGN KEY `ndx_fk_ah_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ah_owned` FOREIGN KEY `ndx_fk_ah_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_name to InnoDB ...' as Action;
ALTER TABLE dev5.accession_name  
 ADD CONSTRAINT `fk_an_a` FOREIGN KEY `ndx_fk_an_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_an_ag` FOREIGN KEY `ndx_fk_an_ag` (`accession_group_id`) REFERENCES `dev5`.`accession_group` (`accession_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_an_i` FOREIGN KEY `ndx_fk_an_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_an_c` FOREIGN KEY `ndx_fk_an_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_an_created` FOREIGN KEY `ndx_fk_an_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_an_modified` FOREIGN KEY `ndx_fk_an_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_an_owned` FOREIGN KEY `ndx_fk_an_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_narrative to InnoDB ...' as Action;
ALTER TABLE dev5.accession_narrative  
 ADD CONSTRAINT `fk_ana_a` FOREIGN KEY `ndx_fk_ana_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ana_created` FOREIGN KEY `ndx_fk_ana_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ana_modified` FOREIGN KEY `ndx_fk_ana_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ana_owned` FOREIGN KEY `ndx_fk_ana_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_pedigree to InnoDB ...' as Action;
ALTER TABLE dev5.accession_pedigree  
 ADD CONSTRAINT `fk_ap_a` FOREIGN KEY `ndx_fk_ap_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ap_ac` FOREIGN KEY `ndx_fk_ap_ac` (`citation_id`) REFERENCES `dev5`.`accession_citation` (`accession_citation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ap_created` FOREIGN KEY `ndx_fk_ap_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ap_modified` FOREIGN KEY `ndx_fk_ap_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ap_owned` FOREIGN KEY `ndx_fk_ap_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_quarantine to InnoDB ...' as Action;
ALTER TABLE dev5.accession_quarantine  
 ADD CONSTRAINT `fk_aq_a` FOREIGN KEY `ndx_fk_aq_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aq_c` FOREIGN KEY `ndx_fk_aq_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aq_created` FOREIGN KEY `ndx_fk_aq_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aq_modified` FOREIGN KEY `ndx_fk_aq_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_aq_owned` FOREIGN KEY `ndx_fk_aq_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_right to InnoDB ...' as Action;
ALTER TABLE dev5.accession_right  
 ADD CONSTRAINT `fk_ar_a` FOREIGN KEY `ndx_fk_ar_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ar_c` FOREIGN KEY `ndx_fk_ar_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ar_ac` FOREIGN KEY `ndx_fk_ar_ac` (`citation_id`) REFERENCES `dev5`.`accession_citation` (`accession_citation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ar_created` FOREIGN KEY `ndx_fk_ar_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ar_modified` FOREIGN KEY `ndx_fk_ar_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ar_owned` FOREIGN KEY `ndx_fk_ar_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_source to InnoDB ...' as Action;
ALTER TABLE dev5.accession_source  
 ADD CONSTRAINT `fk_as_a` FOREIGN KEY `ndx_fk_as_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_as_g` FOREIGN KEY `ndx_fk_as_g` (`geography_id`) REFERENCES `dev5`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_as_created` FOREIGN KEY `ndx_fk_as_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_as_modified` FOREIGN KEY `ndx_fk_as_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_as_owned` FOREIGN KEY `ndx_fk_as_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_source_member to InnoDB ...' as Action;
ALTER TABLE dev5.accession_source_member  
 ADD CONSTRAINT `fk_asm_as` FOREIGN KEY `ndx_fk_asm_as` (`accession_source_id`) REFERENCES `dev5`.`accession_source` (`accession_source_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_asm_a` FOREIGN KEY `ndx_fk_asm_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_asm_c` FOREIGN KEY `ndx_fk_asm_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_asm_created` FOREIGN KEY `ndx_fk_asm_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_asm_modified` FOREIGN KEY `ndx_fk_asm_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_asm_owned` FOREIGN KEY `ndx_fk_asm_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_voucher to InnoDB ...' as Action;
ALTER TABLE dev5.accession_voucher  
 ADD CONSTRAINT `fk_av_a` FOREIGN KEY `ndx_fk_av_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_av_i` FOREIGN KEY `ndx_fk_av_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_av_c` FOREIGN KEY `ndx_fk_av_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_av_created` FOREIGN KEY `ndx_fk_av_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_av_modified` FOREIGN KEY `ndx_fk_av_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_av_owned` FOREIGN KEY `ndx_fk_av_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.accession_voucher_image to InnoDB ...' as Action;
ALTER TABLE dev5.accession_voucher_image  
 ADD CONSTRAINT `fk_avi_av` FOREIGN KEY `ndx_fk_avi_av` (`accession_voucher_id`) REFERENCES `dev5`.`accession_voucher` (`accession_voucher_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_img_created` FOREIGN KEY `ndx_fk_img_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_img_modified` FOREIGN KEY `ndx_fk_img_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_img_owned` FOREIGN KEY `ndx_fk_img_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.app_resource to InnoDB ...' as Action;
ALTER TABLE dev5.app_resource  
 ADD CONSTRAINT `fk_are_sl` FOREIGN KEY `ndx_fk_are_sl` (`sec_lang_id`) REFERENCES `dev5`.`sec_lang` (`sec_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_are_created` FOREIGN KEY `ndx_fk_are_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_are_modified` FOREIGN KEY `ndx_fk_are_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_are_owned` FOREIGN KEY `ndx_fk_are_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.app_user_item_list to MyISAM ...' as Action;
ALTER TABLE dev5.app_user_item_list  
 ADD CONSTRAINT `fk_auil_c` FOREIGN KEY `ndx_fk_auil_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_auil_created` FOREIGN KEY `ndx_fk_auil_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_auil_modified` FOREIGN KEY `ndx_fk_auil_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_auil_owned` FOREIGN KEY `ndx_fk_auil_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.code_group to InnoDB ...' as Action;
ALTER TABLE dev5.code_group  
 ADD CONSTRAINT `fk_cdgrp_created` FOREIGN KEY `ndx_fk_cdgrp_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdgrp_modified` FOREIGN KEY `ndx_fk_cdgrp_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdgrp_owned` FOREIGN KEY `ndx_fk_cdgrp_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.code_rule to InnoDB ...' as Action;
ALTER TABLE dev5.code_rule  
 ADD CONSTRAINT `fk_cdrule_cv` FOREIGN KEY `ndx_fk_cdrule_cv` (`code_value_id`) REFERENCES `dev5`.`code_value` (`code_value_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdrule_created` FOREIGN KEY `ndx_fk_cdrule_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdrule_modified` FOREIGN KEY `ndx_fk_cdrule_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdrule_owned` FOREIGN KEY `ndx_fk_cdrule_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.code_value to InnoDB ...' as Action;
ALTER TABLE dev5.code_value  
 ADD CONSTRAINT `fk_cdval_created` FOREIGN KEY `ndx_fk_cdval_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdval_modified` FOREIGN KEY `ndx_fk_cdval_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cdval_owned` FOREIGN KEY `ndx_fk_cdval_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.code_value_friendly to InnoDB ...' as Action;
ALTER TABLE dev5.code_value_friendly  
 ADD CONSTRAINT `fk_cvf_cv` FOREIGN KEY `ndx_fk_cvf_cv` (`code_value_id`) REFERENCES `dev5`.`code_value` (`code_value_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cvf_sl` FOREIGN KEY `ndx_fk_cvf_sl` (`sec_lang_id`) REFERENCES `dev5`.`sec_lang` (`sec_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcf_created` FOREIGN KEY `ndx_fk_tcf_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcf_modified` FOREIGN KEY `ndx_fk_tcf_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcf_owned` FOREIGN KEY `ndx_fk_tcf_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.cooperator to InnoDB ...' as Action;
ALTER TABLE dev5.cooperator  
 ADD CONSTRAINT `fk_c_cur_c` FOREIGN KEY `ndx_fk_c_cur_c` (`current_cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_c_sl` FOREIGN KEY `ndx_fk_c_sl` (`sec_lang_id`) REFERENCES `dev5`.`sec_lang` (`sec_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_c_created` FOREIGN KEY `ndx_fk_c_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_c_modified` FOREIGN KEY `ndx_fk_c_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_c_owned` FOREIGN KEY `ndx_fk_c_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.cooperator_group to InnoDB ...' as Action;
ALTER TABLE dev5.cooperator_group  
 ADD CONSTRAINT `fk_cg_created` FOREIGN KEY `ndx_fk_cg_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cg_modified` FOREIGN KEY `ndx_fk_cg_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cg_owned` FOREIGN KEY `ndx_fk_cg_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.cooperator_member to InnoDB ...' as Action;
ALTER TABLE dev5.cooperator_member  
 ADD CONSTRAINT `fk_cm_c` FOREIGN KEY `ndx_fk_cm_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cm_cg` FOREIGN KEY `ndx_fk_cm_cg` (`cooperator_group_id`) REFERENCES `dev5`.`cooperator_group` (`cooperator_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cm_created` FOREIGN KEY `ndx_fk_cm_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cm_modified` FOREIGN KEY `ndx_fk_cm_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_cm_owned` FOREIGN KEY `ndx_fk_cm_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.crop to InnoDB ...' as Action;
ALTER TABLE dev5.crop  
 ADD CONSTRAINT `fk_crop_created` FOREIGN KEY `ndx_fk_crop_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_crop_modified` FOREIGN KEY `ndx_fk_crop_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_crop_owned` FOREIGN KEY `ndx_fk_crop_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.evaluation to InnoDB ...' as Action;
ALTER TABLE dev5.evaluation  
 ADD CONSTRAINT `fk_e_g` FOREIGN KEY `ndx_fk_e_g` (`geography_id`) REFERENCES `dev5`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_e_created` FOREIGN KEY `ndx_fk_e_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_e_modified` FOREIGN KEY `ndx_fk_e_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_e_owned` FOREIGN KEY `ndx_fk_e_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.evaluation_citation to InnoDB ...' as Action;
ALTER TABLE dev5.evaluation_citation  
 ADD CONSTRAINT `fk_ec_e` FOREIGN KEY `ndx_fk_ec_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ec_l` FOREIGN KEY `ndx_fk_ec_l` (`literature_id`) REFERENCES `dev5`.`literature` (`literature_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ec_created` FOREIGN KEY `ndx_fk_ec_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ec_modified` FOREIGN KEY `ndx_fk_ec_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ec_owned` FOREIGN KEY `ndx_fk_ec_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.evaluation_member to InnoDB ...' as Action;
ALTER TABLE dev5.evaluation_member  
 ADD CONSTRAINT `fk_em_c` FOREIGN KEY `ndx_fk_em_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_em_e` FOREIGN KEY `ndx_fk_em_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_em_created` FOREIGN KEY `ndx_fk_em_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_em_modified` FOREIGN KEY `ndx_fk_em_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_em_owned` FOREIGN KEY `ndx_fk_em_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.family to InnoDB ...' as Action;
ALTER TABLE dev5.family  
 ADD CONSTRAINT `fk_f_cur_f` FOREIGN KEY `ndx_fk_f_cur_f` (`current_family_id`) REFERENCES `dev5`.`family` (`family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_f_created` FOREIGN KEY `ndx_fk_f_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_f_modified` FOREIGN KEY `ndx_fk_f_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_f_owned` FOREIGN KEY `ndx_fk_f_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genomic_annotation to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_annotation  
 ADD CONSTRAINT `fk_ga_gm` FOREIGN KEY `ndx_fk_ga_gm` (`marker_id`) REFERENCES `dev5`.`genomic_marker` (`genomic_marker_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ga_e` FOREIGN KEY `ndx_fk_ga_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ga_created` FOREIGN KEY `ndx_fk_ga_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ga_modified` FOREIGN KEY `ndx_fk_ga_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ga_owned` FOREIGN KEY `ndx_fk_ga_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genomic_marker to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_marker  
 ADD CONSTRAINT `fk_gm_crop` FOREIGN KEY `ndx_fk_gm_crop` (`crop_id`) REFERENCES `dev5`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gm_created` FOREIGN KEY `ndx_fk_gm_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gm_modified` FOREIGN KEY `ndx_fk_gm_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gm_owned` FOREIGN KEY `ndx_fk_gm_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genomic_marker_citation to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_marker_citation  
 ADD CONSTRAINT `fk_gmc_gm` FOREIGN KEY `ndx_fk_gmc_gm` (`genomic_marker_id`) REFERENCES `dev5`.`genomic_marker` (`genomic_marker_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gmc_l` FOREIGN KEY `ndx_fk_gmc_l` (`literature_id`) REFERENCES `dev5`.`literature` (`literature_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gmc_created` FOREIGN KEY `ndx_fk_gmc_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gmc_modified` FOREIGN KEY `ndx_fk_gmc_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gmc_owned` FOREIGN KEY `ndx_fk_gmc_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genomic_observation to InnoDB ...' as Action;
ALTER TABLE dev5.genomic_observation  
 ADD CONSTRAINT `fk_go_ga` FOREIGN KEY `ndx_fk_go_ga` (`genomic_annotation_id`) REFERENCES `dev5`.`genomic_annotation` (`genomic_annotation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_go_i` FOREIGN KEY `ndx_fk_go_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_go_created` FOREIGN KEY `ndx_fk_go_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_go_modified` FOREIGN KEY `ndx_fk_go_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_go_owned` FOREIGN KEY `ndx_fk_go_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genus to InnoDB ...' as Action;
ALTER TABLE dev5.genus  
 ADD CONSTRAINT `fk_gen_cur_gen` FOREIGN KEY `ndx_fk_gen_cur_gen` (`current_genus_id`) REFERENCES `dev5`.`genus` (`genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gen_f` FOREIGN KEY `ndx_fk_gen_f` (`family_id`) REFERENCES `dev5`.`family` (`family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gen_created` FOREIGN KEY `ndx_fk_gen_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gen_modified` FOREIGN KEY `ndx_fk_gen_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gen_owned` FOREIGN KEY `ndx_fk_gen_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genus_citation to InnoDB ...' as Action;
ALTER TABLE dev5.genus_citation  
 ADD CONSTRAINT `fk_gc_gen` FOREIGN KEY `ndx_fk_gc_gen` (`genus_id`) REFERENCES `dev5`.`genus` (`genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gc_l` FOREIGN KEY `ndx_fk_gc_l` (`literature_id`) REFERENCES `dev5`.`literature` (`literature_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gc_created` FOREIGN KEY `ndx_fk_gc_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gc_modified` FOREIGN KEY `ndx_fk_gc_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gc_owned` FOREIGN KEY `ndx_fk_gc_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.genus_type to InnoDB ...' as Action;
ALTER TABLE dev5.genus_type  
 ADD CONSTRAINT `fk_gt_f` FOREIGN KEY `ndx_fk_gt_f` (`family_id`) REFERENCES `dev5`.`family` (`family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gt_created` FOREIGN KEY `ndx_fk_gt_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gt_modified` FOREIGN KEY `ndx_fk_gt_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_gt_owned` FOREIGN KEY `ndx_fk_gt_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.geography to InnoDB ...' as Action;
ALTER TABLE dev5.geography  
 ADD CONSTRAINT `fk_g_cur_g` FOREIGN KEY `ndx_fk_g_cur_g` (`current_geography_id`) REFERENCES `dev5`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_g_re` FOREIGN KEY `ndx_fk_g_re` (`region_id`) REFERENCES `dev5`.`region` (`region_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_g_created` FOREIGN KEY `ndx_fk_g_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_g_modified` FOREIGN KEY `ndx_fk_g_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_g_owned` FOREIGN KEY `ndx_fk_g_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory to InnoDB ...' as Action;
ALTER TABLE dev5.inventory  
 ADD CONSTRAINT `fk_i_im` FOREIGN KEY `ndx_fk_i_im` (`inventory_maintenance_id`) REFERENCES `dev5`.`inventory_maintenance` (`inventory_maintenance_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_a` FOREIGN KEY `ndx_fk_i_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_parent_i` FOREIGN KEY `ndx_fk_i_parent_i` (`parent_inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_c` FOREIGN KEY `ndx_fk_i_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_backup_i` FOREIGN KEY `ndx_fk_i_backup_i` (`backup_inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_created` FOREIGN KEY `ndx_fk_i_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_modified` FOREIGN KEY `ndx_fk_i_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_i_owned` FOREIGN KEY `ndx_fk_i_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory_action to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_action  
 ADD CONSTRAINT `fk_ia_i` FOREIGN KEY `ndx_fk_ia_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ia_c` FOREIGN KEY `ndx_fk_ia_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ia_e` FOREIGN KEY `ndx_fk_ia_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ia_created` FOREIGN KEY `ndx_fk_ia_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ia_modified` FOREIGN KEY `ndx_fk_ia_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ia_owned` FOREIGN KEY `ndx_fk_ia_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory_group to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_group  
 ADD CONSTRAINT `fk_ig_created` FOREIGN KEY `ndx_fk_ig_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ig_modified` FOREIGN KEY `ndx_fk_ig_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ig_owned` FOREIGN KEY `ndx_fk_ig_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory_group_maintenance to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_group_maintenance  
 ADD CONSTRAINT `fk_igm_i` FOREIGN KEY `ndx_fk_igm_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_igm_ig` FOREIGN KEY `ndx_fk_igm_ig` (`inventory_group_id`) REFERENCES `dev5`.`inventory_group` (`inventory_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_igm_created` FOREIGN KEY `ndx_fk_igm_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_igm_modified` FOREIGN KEY `ndx_fk_igm_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_igm_owned` FOREIGN KEY `ndx_fk_igm_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory_maintenance to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_maintenance  
 ADD CONSTRAINT `fk_im_co` FOREIGN KEY `ndx_fk_im_co` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_im_created` FOREIGN KEY `ndx_fk_im_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_im_modified` FOREIGN KEY `ndx_fk_im_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_im_owned` FOREIGN KEY `ndx_fk_im_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory_pathogen_test to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_pathogen_test  
 ADD CONSTRAINT `fk_ipt_i` FOREIGN KEY `ndx_fk_ipt_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ipt_created` FOREIGN KEY `ndx_fk_ipt_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ipt_modified` FOREIGN KEY `ndx_fk_ipt_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ipt_owned` FOREIGN KEY `ndx_fk_ipt_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.inventory_viability to InnoDB ...' as Action;
ALTER TABLE dev5.inventory_viability  
 ADD CONSTRAINT `fk_iv_i` FOREIGN KEY `ndx_fk_iv_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_iv_e` FOREIGN KEY `ndx_fk_iv_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_iv_created` FOREIGN KEY `ndx_fk_iv_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_iv_modified` FOREIGN KEY `ndx_fk_iv_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_iv_owned` FOREIGN KEY `ndx_fk_iv_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.literature to InnoDB ...' as Action;
ALTER TABLE dev5.literature  
 ADD CONSTRAINT `fk_l_created` FOREIGN KEY `ndx_fk_l_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_l_modified` FOREIGN KEY `ndx_fk_l_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_l_owned` FOREIGN KEY `ndx_fk_l_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.order_entry to InnoDB ...' as Action;
ALTER TABLE dev5.order_entry  
 ADD CONSTRAINT `fk_oe_original_oe` FOREIGN KEY `ndx_fk_oe_original_oe` (`original_order_entry_id`) REFERENCES `dev5`.`order_entry` (`order_entry_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_source_c` FOREIGN KEY `ndx_fk_oe_source_c` (`source_cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_requestor_c` FOREIGN KEY `ndx_fk_oe_requestor_c` (`requestor_cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_ship_to_c` FOREIGN KEY `ndx_fk_oe_ship_to_c` (`ship_to_cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_final_c` FOREIGN KEY `ndx_fk_oe_final_c` (`final_recipient_cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_created` FOREIGN KEY `ndx_fk_oe_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_modified` FOREIGN KEY `ndx_fk_oe_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oe_owned` FOREIGN KEY `ndx_fk_oe_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.order_entry_action to InnoDB ...' as Action;
ALTER TABLE dev5.order_entry_action  
 ADD CONSTRAINT `fk_oea_oe` FOREIGN KEY `ndx_fk_oea_oe` (`order_entry_id`) REFERENCES `dev5`.`order_entry` (`order_entry_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oea_created` FOREIGN KEY `ndx_fk_oea_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oea_modified` FOREIGN KEY `ndx_fk_oea_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oea_owned` FOREIGN KEY `ndx_fk_oea_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.order_entry_item to InnoDB ...' as Action;
ALTER TABLE dev5.order_entry_item  
 ADD CONSTRAINT `fk_oei_oe` FOREIGN KEY `ndx_fk_oei_oe` (`order_entry_id`) REFERENCES `dev5`.`order_entry` (`order_entry_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_c` FOREIGN KEY `ndx_fk_oei_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_i` FOREIGN KEY `ndx_fk_oei_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_a` FOREIGN KEY `ndx_fk_oei_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_t` FOREIGN KEY `ndx_fk_oei_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_created` FOREIGN KEY `ndx_fk_oei_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_modified` FOREIGN KEY `ndx_fk_oei_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_oei_owned` FOREIGN KEY `ndx_fk_oei_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.plant_introduction to InnoDB ...' as Action;
ALTER TABLE dev5.plant_introduction  
 ADD CONSTRAINT `fk_pi_created` FOREIGN KEY `ndx_fk_pi_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_pi_modified` FOREIGN KEY `ndx_fk_pi_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_pi_owned` FOREIGN KEY `ndx_fk_pi_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.region to InnoDB ...' as Action;
ALTER TABLE dev5.region  
 ADD CONSTRAINT `fk_r_created` FOREIGN KEY `ndx_fk_r_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_r_modified` FOREIGN KEY `ndx_fk_r_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_r_owned` FOREIGN KEY `ndx_fk_r_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_lang to InnoDB ...' as Action;
ALTER TABLE dev5.sec_lang  
 ADD CONSTRAINT `fk_sl_created` FOREIGN KEY `ndx_fk_sl_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sl_modified` FOREIGN KEY `ndx_fk_sl_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sl_owned` FOREIGN KEY `ndx_fk_sl_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_perm to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm  
 ADD CONSTRAINT `fk_sp_spt` FOREIGN KEY `ndx_fk_sp_spt` (`sec_perm_template_id`) REFERENCES `dev5`.`sec_perm_template` (`sec_perm_template_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sp_created` FOREIGN KEY `ndx_fk_sp_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sp_modified` FOREIGN KEY `ndx_fk_sp_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sp_owned` FOREIGN KEY `ndx_fk_sp_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_perm_field to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm_field  
 ADD CONSTRAINT `fk_spf_sp` FOREIGN KEY `ndx_fk_spf_sp` (`sec_perm_id`) REFERENCES `dev5`.`sec_perm` (`sec_perm_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_spf_created` FOREIGN KEY `ndx_fk_spf_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_spf_modified` FOREIGN KEY `ndx_fk_spf_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_spf_owned` FOREIGN KEY `ndx_fk_spf_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_perm_template to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm_template  
 ADD CONSTRAINT `fk_spt_created` FOREIGN KEY `ndx_fk_spt_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_spt_modified` FOREIGN KEY `ndx_fk_spt_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_spt_owned` FOREIGN KEY `ndx_fk_spt_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_perm_template_map to InnoDB ...' as Action;
ALTER TABLE dev5.sec_perm_template_map  
 ADD CONSTRAINT `fk_sptm_spt` FOREIGN KEY `ndx_fk_sptm_spt` (`sec_perm_template_id`) REFERENCES `dev5`.`sec_perm_template` (`sec_perm_template_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sptm_sp` FOREIGN KEY `ndx_fk_sptm_sp` (`sec_perm_id`) REFERENCES `dev5`.`sec_perm` (`sec_perm_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sptm_created` FOREIGN KEY `ndx_fk_sptm_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sptm_modified` FOREIGN KEY `ndx_fk_sptm_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sptm_owned` FOREIGN KEY `ndx_fk_sptm_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_rs to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs  
 ADD CONSTRAINT `fk_sr_created` FOREIGN KEY `ndx_fk_sr_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sr_modified` FOREIGN KEY `ndx_fk_sr_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sr_owned` FOREIGN KEY `ndx_fk_sr_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_rs_field to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs_field  
 ADD CONSTRAINT `fk_srf_sr` FOREIGN KEY `ndx_fk_srf_sr` (`sec_rs_id`) REFERENCES `dev5`.`sec_rs` (`sec_rs_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srf_stf` FOREIGN KEY `ndx_fk_srf_stf` (`sec_table_field_id`) REFERENCES `dev5`.`sec_table_field` (`sec_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srf_created` FOREIGN KEY `ndx_fk_srf_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srf_modified` FOREIGN KEY `ndx_fk_srf_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srf_owned` FOREIGN KEY `ndx_fk_srf_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_rs_field_friendly to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs_field_friendly  
 ADD CONSTRAINT `fk_srff_srf` FOREIGN KEY `ndx_fk_srff_srf` (`sec_rs_field_id`) REFERENCES `dev5`.`sec_rs_field` (`sec_rs_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srff_sl` FOREIGN KEY `ndx_fk_srff_sl` (`sec_lang_id`) REFERENCES `dev5`.`sec_lang` (`sec_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srff_created` FOREIGN KEY `ndx_fk_srff_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srff_modified` FOREIGN KEY `ndx_fk_srff_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srff_owned` FOREIGN KEY `ndx_fk_srff_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_rs_param to InnoDB ...' as Action;
ALTER TABLE dev5.sec_rs_param  
 ADD CONSTRAINT `fk_srp_sr` FOREIGN KEY `ndx_fk_srp_sr` (`sec_rs_id`) REFERENCES `dev5`.`sec_rs` (`sec_rs_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srp_created` FOREIGN KEY `ndx_fk_srp_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srp_modified` FOREIGN KEY `ndx_fk_srp_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_srp_owned` FOREIGN KEY `ndx_fk_srp_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_table to InnoDB ...' as Action;
ALTER TABLE dev5.sec_table  
 ADD CONSTRAINT `fk_st_created` FOREIGN KEY `ndx_fk_st_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_st_modified` FOREIGN KEY `ndx_fk_st_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_st_owned` FOREIGN KEY `ndx_fk_st_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_table_field to InnoDB ...' as Action;
ALTER TABLE dev5.sec_table_field  
 ADD CONSTRAINT `fk_stf_st` FOREIGN KEY `ndx_fk_stf_st` (`sec_table_id`) REFERENCES `dev5`.`sec_table` (`sec_table_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_stf_cgr` FOREIGN KEY `ndx_fk_stf_cgr` (`code_group_id`) REFERENCES `dev5`.`code_group` (`code_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_stf_created` FOREIGN KEY `ndx_fk_stf_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_stf_modified` FOREIGN KEY `ndx_fk_stf_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_stf_owned` FOREIGN KEY `ndx_fk_stf_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_user to InnoDB ...' as Action;
ALTER TABLE dev5.sec_user  
 ADD CONSTRAINT `fk_su_co` FOREIGN KEY `ndx_fk_su_co` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_su_created` FOREIGN KEY `ndx_fk_su_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_su_modified` FOREIGN KEY `ndx_fk_su_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_su_owned` FOREIGN KEY `ndx_fk_su_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_user_gui_setting to InnoDB ...' as Action;
ALTER TABLE dev5.sec_user_gui_setting  
 ADD CONSTRAINT `fk_sugs_co` FOREIGN KEY `ndx_fk_sugs_co` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sugs_created` FOREIGN KEY `ndx_fk_sugs_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sugs_modified` FOREIGN KEY `ndx_fk_sugs_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sugs_owned` FOREIGN KEY `ndx_fk_sugs_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.sec_user_perm to InnoDB ...' as Action;
ALTER TABLE dev5.sec_user_perm  
 ADD CONSTRAINT `fk_sup_su` FOREIGN KEY `ndx_fk_sup_su` (`sec_user_id`) REFERENCES `dev5`.`sec_user` (`sec_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sup_sp` FOREIGN KEY `ndx_fk_sup_sp` (`sec_perm_id`) REFERENCES `dev5`.`sec_perm` (`sec_perm_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sup_created` FOREIGN KEY `ndx_fk_sup_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sup_modified` FOREIGN KEY `ndx_fk_sup_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_sup_owned` FOREIGN KEY `ndx_fk_sup_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy  
 ADD CONSTRAINT `fk_t_cur_t` FOREIGN KEY `ndx_fk_t_cur_t` (`current_taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_t_gen` FOREIGN KEY `ndx_fk_t_gen` (`genus_id`) REFERENCES `dev5`.`genus` (`genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_t_crop` FOREIGN KEY `ndx_fk_t_crop` (`crop_id`) REFERENCES `dev5`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_t_c` FOREIGN KEY `ndx_fk_t_c` (`cooperator_id`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_t_created` FOREIGN KEY `ndx_fk_t_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_t_modified` FOREIGN KEY `ndx_fk_t_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_t_owned` FOREIGN KEY `ndx_fk_t_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_author to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_author  
 ADD CONSTRAINT `fk_ta_created` FOREIGN KEY `ndx_fk_ta_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ta_modified` FOREIGN KEY `ndx_fk_ta_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_ta_owned` FOREIGN KEY `ndx_fk_ta_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_citation to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_citation  
 ADD CONSTRAINT `fk_tc_t` FOREIGN KEY `ndx_fk_tc_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_l` FOREIGN KEY `ndx_fk_tc_l` (`literature_id`) REFERENCES `dev5`.`literature` (`literature_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_created` FOREIGN KEY `ndx_fk_tc_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_modified` FOREIGN KEY `ndx_fk_tc_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_owned` FOREIGN KEY `ndx_fk_tc_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_common_name to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_common_name  
 ADD CONSTRAINT `fk_tcn_t` FOREIGN KEY `ndx_fk_tcn_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcn_created` FOREIGN KEY `ndx_fk_tcn_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcn_modified` FOREIGN KEY `ndx_fk_tcn_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcn_owned` FOREIGN KEY `ndx_fk_tcn_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_distribution to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_distribution  
 ADD CONSTRAINT `fk_td_t` FOREIGN KEY `ndx_fk_td_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_td_g` FOREIGN KEY `ndx_fk_td_g` (`geography_id`) REFERENCES `dev5`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_td_created` FOREIGN KEY `ndx_fk_td_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_td_modified` FOREIGN KEY `ndx_fk_td_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_td_owned` FOREIGN KEY `ndx_fk_td_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_germination_rule to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_germination_rule  
 ADD CONSTRAINT `fk_tgr_t` FOREIGN KEY `ndx_fk_tgr_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tgr_created` FOREIGN KEY `ndx_fk_tgr_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tgr_modified` FOREIGN KEY `ndx_fk_tgr_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tgr_owned` FOREIGN KEY `ndx_fk_tgr_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_url to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_url  
 ADD CONSTRAINT `fk_tu_f` FOREIGN KEY `ndx_fk_tu_f` (`family_id`) REFERENCES `dev5`.`family` (`family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tu_gen` FOREIGN KEY `ndx_fk_tu_gen` (`genus_id`) REFERENCES `dev5`.`genus` (`genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tu_t` FOREIGN KEY `ndx_fk_tu_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tu_created` FOREIGN KEY `ndx_fk_tu_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tu_modified` FOREIGN KEY `ndx_fk_tu_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tu_owned` FOREIGN KEY `ndx_fk_tu_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.taxonomy_use to InnoDB ...' as Action;
ALTER TABLE dev5.taxonomy_use  
 ADD CONSTRAINT `fk_tus_t` FOREIGN KEY `ndx_fk_tus_t` (`taxonomy_id`) REFERENCES `dev5`.`taxonomy` (`taxonomy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tus_created` FOREIGN KEY `ndx_fk_tus_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tus_modified` FOREIGN KEY `ndx_fk_tus_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tus_owned` FOREIGN KEY `ndx_fk_tus_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.trait to InnoDB ...' as Action;
ALTER TABLE dev5.trait  
 ADD CONSTRAINT `fk_tr_crop` FOREIGN KEY `ndx_fk_tr_crop` (`crop_id`) REFERENCES `dev5`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tr_created` FOREIGN KEY `ndx_fk_tr_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tr_modified` FOREIGN KEY `ndx_fk_tr_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tr_owned` FOREIGN KEY `ndx_fk_tr_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.trait_code to InnoDB ...' as Action;
ALTER TABLE dev5.trait_code  
 ADD CONSTRAINT `fk_tc_tr` FOREIGN KEY `ndx_fk_tc_tr` (`trait_id`) REFERENCES `dev5`.`trait` (`trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_created` FOREIGN KEY `ndx_fk_tc_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_modified` FOREIGN KEY `ndx_fk_tc_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tc_owned` FOREIGN KEY `ndx_fk_tc_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.trait_code_friendly to InnoDB ...' as Action;
ALTER TABLE dev5.trait_code_friendly  
 ADD CONSTRAINT `fk_tcf_tc` FOREIGN KEY `ndx_fk_tcf_tc` (`trait_code_id`) REFERENCES `dev5`.`trait_code` (`trait_code_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tct_sl` FOREIGN KEY `ndx_fk_tct_sl` (`sec_lang_id`) REFERENCES `dev5`.`sec_lang` (`sec_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcf_created` FOREIGN KEY `ndx_fk_tcf_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcf_modified` FOREIGN KEY `ndx_fk_tcf_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tcf_owned` FOREIGN KEY `ndx_fk_tcf_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.trait_observation to InnoDB ...' as Action;
ALTER TABLE dev5.trait_observation  
 ADD CONSTRAINT `fk_to_tr` FOREIGN KEY `ndx_fk_to_tr` (`trait_id`) REFERENCES `dev5`.`trait` (`trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_tc` FOREIGN KEY `ndx_fk_to_tc` (`trait_code_id`) REFERENCES `dev5`.`trait_code` (`trait_code_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_a` FOREIGN KEY `ndx_fk_to_a` (`accession_id`) REFERENCES `dev5`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_e` FOREIGN KEY `ndx_fk_to_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_tq` FOREIGN KEY `ndx_fk_to_tq` (`qualifier_id`) REFERENCES `dev5`.`trait_qualifier` (`trait_qualifier_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_i` FOREIGN KEY `ndx_fk_to_i` (`inventory_id`) REFERENCES `dev5`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_created` FOREIGN KEY `ndx_fk_to_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_modified` FOREIGN KEY `ndx_fk_to_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_to_owned` FOREIGN KEY `ndx_fk_to_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.trait_qualifier to InnoDB ...' as Action;
ALTER TABLE dev5.trait_qualifier  
 ADD CONSTRAINT `fk_tq_tr` FOREIGN KEY `ndx_fk_tq_tr` (`trait_id`) REFERENCES `dev5`.`trait` (`trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tq_created` FOREIGN KEY `ndx_fk_tq_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tq_modified` FOREIGN KEY `ndx_fk_tq_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tq_owned` FOREIGN KEY `ndx_fk_tq_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

select 'Adding constraints for dev5.trait_url to InnoDB ...' as Action;
ALTER TABLE dev5.trait_url  
 ADD CONSTRAINT `fk_tur_crop` FOREIGN KEY `ndx_fk_tur_crop` (`crop_id`) REFERENCES `dev5`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tur_tr` FOREIGN KEY `ndx_fk_tur_tr` (`trait_id`) REFERENCES `dev5`.`trait` (`trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tur_e` FOREIGN KEY `ndx_fk_tur_e` (`evaluation_id`) REFERENCES `dev5`.`evaluation` (`evaluation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tur_created` FOREIGN KEY `ndx_fk_tur_created` (`created_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tur_modified` FOREIGN KEY `ndx_fk_tur_modified` (`modified_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT, 
 ADD CONSTRAINT `fk_tur_owned` FOREIGN KEY `ndx_fk_tur_owned` (`owned_by`) REFERENCES `dev5`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;

/***********************************************/
/*********** Migration Table Drops ***********/
/***********************************************/

select 'dropping migration table __accession ...' as Action;
drop table dev5.__accession;

select 'dropping migration table __accession_action ...' as Action;
drop table dev5.__accession_action;

select 'dropping migration table __accession_annotation ...' as Action;
drop table dev5.__accession_annotation;

select 'dropping migration table __accession_citation ...' as Action;
drop table dev5.__accession_citation;

select 'dropping migration table __accession_group ...' as Action;
drop table dev5.__accession_group;

select 'dropping migration table __accession_habitat ...' as Action;
drop table dev5.__accession_habitat;

select 'dropping migration table __accession_name ...' as Action;
drop table dev5.__accession_name;

select 'dropping migration table __accession_narrative ...' as Action;
drop table dev5.__accession_narrative;

select 'dropping migration table __accession_pedigree ...' as Action;
drop table dev5.__accession_pedigree;

select 'dropping migration table __accession_quarantine ...' as Action;
drop table dev5.__accession_quarantine;

select 'dropping migration table __accession_right ...' as Action;
drop table dev5.__accession_right;

select 'dropping migration table __accession_source ...' as Action;
drop table dev5.__accession_source;

select 'dropping migration table __accession_source_member ...' as Action;
drop table dev5.__accession_source_member;

select 'dropping migration table __accession_voucher ...' as Action;
drop table dev5.__accession_voucher;

select 'dropping migration table __accession_voucher_image ...' as Action;
drop table dev5.__accession_voucher_image;

select 'dropping migration table __app_resource ...' as Action;
drop table dev5.__app_resource;

select 'dropping migration table __app_user_item_list ...' as Action;
drop table dev5.__app_user_item_list;

select 'dropping migration table __code_group ...' as Action;
drop table dev5.__code_group;

select 'dropping migration table __code_rule ...' as Action;
drop table dev5.__code_rule;

select 'dropping migration table __code_value ...' as Action;
drop table dev5.__code_value;

select 'dropping migration table __code_value_friendly ...' as Action;
drop table dev5.__code_value_friendly;

select 'dropping migration table __cooperator ...' as Action;
drop table dev5.__cooperator;

select 'dropping migration table __cooperator_group ...' as Action;
drop table dev5.__cooperator_group;

select 'dropping migration table __cooperator_member ...' as Action;
drop table dev5.__cooperator_member;

select 'dropping migration table __crop ...' as Action;
drop table dev5.__crop;

select 'dropping migration table __evaluation ...' as Action;
drop table dev5.__evaluation;

select 'dropping migration table __evaluation_citation ...' as Action;
drop table dev5.__evaluation_citation;

select 'dropping migration table __evaluation_member ...' as Action;
drop table dev5.__evaluation_member;

select 'dropping migration table __family ...' as Action;
drop table dev5.__family;

select 'dropping migration table __genomic_annotation ...' as Action;
drop table dev5.__genomic_annotation;

select 'dropping migration table __genomic_marker ...' as Action;
drop table dev5.__genomic_marker;

select 'dropping migration table __genomic_marker_citation ...' as Action;
drop table dev5.__genomic_marker_citation;

select 'dropping migration table __genomic_observation ...' as Action;
drop table dev5.__genomic_observation;

select 'dropping migration table __genus ...' as Action;
drop table dev5.__genus;

select 'dropping migration table __genus_citation ...' as Action;
drop table dev5.__genus_citation;

select 'dropping migration table __genus_type ...' as Action;
drop table dev5.__genus_type;

select 'dropping migration table __geography ...' as Action;
drop table dev5.__geography;

select 'dropping migration table __inventory ...' as Action;
drop table dev5.__inventory;

select 'dropping migration table __inventory_action ...' as Action;
drop table dev5.__inventory_action;

select 'dropping migration table __inventory_group ...' as Action;
drop table dev5.__inventory_group;

select 'dropping migration table __inventory_group_maintenance ...' as Action;
drop table dev5.__inventory_group_maintenance;

select 'dropping migration table __inventory_maintenance ...' as Action;
drop table dev5.__inventory_maintenance;

select 'dropping migration table __inventory_pathogen_test ...' as Action;
drop table dev5.__inventory_pathogen_test;

select 'dropping migration table __inventory_viability ...' as Action;
drop table dev5.__inventory_viability;

select 'dropping migration table __literature ...' as Action;
drop table dev5.__literature;

select 'dropping migration table __order_entry ...' as Action;
drop table dev5.__order_entry;

select 'dropping migration table __order_entry_action ...' as Action;
drop table dev5.__order_entry_action;

select 'dropping migration table __order_entry_item ...' as Action;
drop table dev5.__order_entry_item;

select 'dropping migration table __plant_introduction ...' as Action;
drop table dev5.__plant_introduction;

select 'dropping migration table __region ...' as Action;
drop table dev5.__region;

select 'dropping migration table __sec_lang ...' as Action;
drop table dev5.__sec_lang;

select 'dropping migration table __sec_perm ...' as Action;
drop table dev5.__sec_perm;

select 'dropping migration table __sec_perm_field ...' as Action;
drop table dev5.__sec_perm_field;

select 'dropping migration table __sec_perm_template ...' as Action;
drop table dev5.__sec_perm_template;

select 'dropping migration table __sec_perm_template_map ...' as Action;
drop table dev5.__sec_perm_template_map;

select 'dropping migration table __sec_rs ...' as Action;
drop table dev5.__sec_rs;

select 'dropping migration table __sec_rs_field ...' as Action;
drop table dev5.__sec_rs_field;

select 'dropping migration table __sec_rs_field_friendly ...' as Action;
drop table dev5.__sec_rs_field_friendly;

select 'dropping migration table __sec_rs_param ...' as Action;
drop table dev5.__sec_rs_param;

select 'dropping migration table __sec_table ...' as Action;
drop table dev5.__sec_table;

select 'dropping migration table __sec_table_field ...' as Action;
drop table dev5.__sec_table_field;

select 'dropping migration table __sec_user ...' as Action;
drop table dev5.__sec_user;

select 'dropping migration table __sec_user_gui_setting ...' as Action;
drop table dev5.__sec_user_gui_setting;

select 'dropping migration table __sec_user_perm ...' as Action;
drop table dev5.__sec_user_perm;

select 'dropping migration table __taxonomy ...' as Action;
drop table dev5.__taxonomy;

select 'dropping migration table __taxonomy_author ...' as Action;
drop table dev5.__taxonomy_author;

select 'dropping migration table __taxonomy_citation ...' as Action;
drop table dev5.__taxonomy_citation;

select 'dropping migration table __taxonomy_common_name ...' as Action;
drop table dev5.__taxonomy_common_name;

select 'dropping migration table __taxonomy_distribution ...' as Action;
drop table dev5.__taxonomy_distribution;

select 'dropping migration table __taxonomy_germination_rule ...' as Action;
drop table dev5.__taxonomy_germination_rule;

select 'dropping migration table __taxonomy_url ...' as Action;
drop table dev5.__taxonomy_url;

select 'dropping migration table __taxonomy_use ...' as Action;
drop table dev5.__taxonomy_use;

select 'dropping migration table __trait ...' as Action;
drop table dev5.__trait;

select 'dropping migration table __trait_code ...' as Action;
drop table dev5.__trait_code;

select 'dropping migration table __trait_code_friendly ...' as Action;
drop table dev5.__trait_code_friendly;

select 'dropping migration table __trait_observation ...' as Action;
drop table dev5.__trait_observation;

select 'dropping migration table __trait_qualifier ...' as Action;
drop table dev5.__trait_qualifier;

select 'dropping migration table __trait_url ...' as Action;
drop table dev5.__trait_url;

