 /************************************************************************/
/************************************************************************/
/*** This script can be recreated using GrinGlobal.DatabaseCopier.exe ***/
/************************************************************************/
/************************************************************************/
/***********************************************/
/*************** Table Definitions *************/
/***********************************************/

/************ Table Definition for gringlobal.accession *************/
select concat(now(), ' creating table gringlobal.accession ...') as Action;
CREATE TABLE `gringlobal`.`accession` (
`accession_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_number_part1` varchar(50)    NOT NULL  ,
`accession_number_part2` int(11)    NULL  ,
`accession_number_part3` varchar(50)    NULL  ,
`is_core` char(1)     NOT NULL  ,
`is_backed_up` char(1)     NOT NULL  ,
`backup_location1_site_id` int(11)    NULL  ,
`backup_location2_site_id` int(11)    NULL  ,
`status_code` varchar(20)    NULL  ,
`life_form_code` varchar(20)    NULL  ,
`improvement_status_code` varchar(20)    NULL  ,
`reproductive_uniformity_code` varchar(20)    NULL  ,
`initial_received_form_code` varchar(20)    NULL  ,
`initial_received_date` datetime    NULL  ,
`initial_received_date_code` varchar(20)    NULL  ,
`taxonomy_species_id` int(11)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_action *************/
select concat(now(), ' creating table gringlobal.accession_action ...') as Action;
CREATE TABLE `gringlobal`.`accession_action` (
`accession_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`action_name_code` varchar(20)    NOT NULL  ,
`started_date` datetime    NULL  ,
`started_date_code` varchar(20)    NULL  ,
`completed_date` datetime    NULL  ,
`completed_date_code` varchar(20)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`note` text     NULL  ,
`cooperator_id` int(11)    NULL  ,
`method_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_action_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_annotation *************/
select concat(now(), ' creating table gringlobal.accession_annotation ...') as Action;
CREATE TABLE `gringlobal`.`accession_annotation` (
`accession_annotation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`annotation_type_code` varchar(20)    NOT NULL  ,
`annotation_date` datetime    NOT NULL  ,
`annotation_date_code` varchar(20)    NULL  ,
`annotation_cooperator_id` int(11)    NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`order_request_id` int(11)    NULL  ,
`old_taxonomy_species_id` int(11)    NULL  ,
`new_taxonomy_species_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_annotation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_ipr *************/
select concat(now(), ' creating table gringlobal.accession_ipr ...') as Action;
CREATE TABLE `gringlobal`.`accession_ipr` (
`accession_ipr_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`type_code` varchar(20)    NOT NULL  ,
`ipr_number` varchar(50)    NULL  ,
`ipr_crop_name` varchar(100)    NULL  ,
`ipr_full_name` text     NULL  ,
`issued_date` datetime    NULL  ,
`expired_date` datetime    NULL  ,
`cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`accepted_date` datetime    NULL  ,
`expected_date` datetime    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_ipr_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_name *************/
select concat(now(), ' creating table gringlobal.accession_name ...') as Action;
CREATE TABLE `gringlobal`.`accession_name` (
`accession_name_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`category_code` varchar(20)    NOT NULL  ,
`plant_name` varchar(100)    NOT NULL  ,
`plant_name_rank` int(11)    NOT NULL  ,
`name_group_id` int(11)    NULL  ,
`name_source_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_name_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_pedigree *************/
select concat(now(), ' creating table gringlobal.accession_pedigree ...') as Action;
CREATE TABLE `gringlobal`.`accession_pedigree` (
`accession_pedigree_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`released_date` datetime    NULL  ,
`released_date_code` varchar(20)    NULL  ,
`male_accession_id` int(11)    NULL  ,
`male_external_accession` varchar(50)    NULL  ,
`female_accession_id` int(11)    NULL  ,
`female_external_accession` varchar(50)    NULL  ,
`cross_code` varchar(20)    NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_pedigree_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_quarantine *************/
select concat(now(), ' creating table gringlobal.accession_quarantine ...') as Action;
CREATE TABLE `gringlobal`.`accession_quarantine` (
`accession_quarantine_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`quarantine_type_code` varchar(20)    NOT NULL  ,
`progress_status_code` varchar(20)    NULL  ,
`custodial_cooperator_id` int(11)    NOT NULL  ,
`entered_date` datetime    NULL  ,
`established_date` datetime    NULL  ,
`expected_release_date` datetime    NULL  ,
`released_date` datetime    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_quarantine_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_source *************/
select concat(now(), ' creating table gringlobal.accession_source ...') as Action;
CREATE TABLE `gringlobal`.`accession_source` (
`accession_source_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_id` int(11)    NOT NULL  ,
`geography_id` int(11)    NULL  ,
`acquisition_source_code` varchar(20)    NULL  ,
`source_type_code` varchar(20)    NOT NULL  ,
`source_date` datetime    NULL  ,
`source_date_code` varchar(20)    NULL  ,
`is_origin` char(1)     NOT NULL  ,
`quantity_collected` int(11)    NULL  ,
`unit_quantity_collected_code` varchar(20)    NULL  ,
`collected_form_code` varchar(20)    NULL  ,
`number_plants_sampled` int(11)    NULL  ,
`elevation_meters` int(11)    NULL  ,
`collector_verbatim_locality` text     NULL  ,
`latitude` decimal(18, 8)    NULL  ,
`longitude` decimal(18, 8)    NULL  ,
`uncertainty` int(11)    NULL  ,
`formatted_locality` text     NULL  ,
`georeference_datum` varchar(10)    NULL  ,
`georeference_protocol_code` varchar(20)    NULL  ,
`georeference_annotation` text     NULL  ,
`environment_description` text     NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_source_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_source_map *************/
select concat(now(), ' creating table gringlobal.accession_source_map ...') as Action;
CREATE TABLE `gringlobal`.`accession_source_map` (
`accession_source_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`accession_source_id` int(11)    NOT NULL  ,
`cooperator_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_source_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.accession_voucher *************/
select concat(now(), ' creating table gringlobal.accession_voucher ...') as Action;
CREATE TABLE `gringlobal`.`accession_voucher` (
`accession_voucher_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NULL  ,
`collector_cooperator_id` int(11)    NULL  ,
`collector_voucher_number` varchar(40)    NULL  ,
`voucher_location` varchar(255)    NOT NULL  ,
`vouchered_date` datetime    NULL  ,
`vouchered_date_code` varchar(20)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`accession_voucher_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.app_resource *************/
select concat(now(), ' creating table gringlobal.app_resource ...') as Action;
CREATE TABLE `gringlobal`.`app_resource` (
`app_resource_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_lang_id` int(11)    NOT NULL  ,
`app_name` varchar(100)    NOT NULL  ,
`form_name` varchar(100)    NOT NULL  ,
`app_resource_name` varchar(100)    NOT NULL  ,
`title_member` text     NULL  ,
`display_member` text     NOT NULL  ,
`value_member` text     NULL  ,
`sort_order` int(11)    NULL  ,
`properties` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`app_resource_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.app_setting *************/
select concat(now(), ' creating table gringlobal.app_setting ...') as Action;
CREATE TABLE `gringlobal`.`app_setting` (
`app_setting_id` int(11)    NOT NULL AUTO_INCREMENT ,
`category_tag` varchar(200)    NULL  ,
`sort_order` int(11)    NULL  ,
`name` varchar(200)    NOT NULL  ,
`value` varchar(500)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`app_setting_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.app_user_gui_setting *************/
select concat(now(), ' creating table gringlobal.app_user_gui_setting ...') as Action;
CREATE TABLE `gringlobal`.`app_user_gui_setting` (
`app_user_gui_setting_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`app_name` varchar(50)    NULL  ,
`form_name` varchar(100)    NULL  ,
`resource_name` varchar(100)    NOT NULL  ,
`resource_key` varchar(100)    NOT NULL  ,
`resource_value` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`app_user_gui_setting_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.app_user_item_list *************/
select concat(now(), ' creating table gringlobal.app_user_item_list ...') as Action;
CREATE TABLE `gringlobal`.`app_user_item_list` (
`app_user_item_list_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`tab_name` varchar(100)    NOT NULL  ,
`list_name` varchar(300)    NOT NULL  ,
`id_number` int(11)    NULL  ,
`id_type` varchar(100)    NOT NULL  ,
`sort_order` int(11)    NULL  ,
`title` text     NOT NULL  ,
`description` text     NULL  ,
`properties` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`app_user_item_list_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.citation *************/
select concat(now(), ' creating table gringlobal.citation ...') as Action;
CREATE TABLE `gringlobal`.`citation` (
`citation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`literature_id` int(11)    NULL  ,
`citation_title` text     NULL  ,
`author_name` text     NULL  ,
`citation_year` int(11)    NULL  ,
`reference` varchar(200)    NULL  ,
`doi_reference` varchar(500)    NULL  ,
`url` varchar(500)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`citation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.citation_map *************/
select concat(now(), ' creating table gringlobal.citation_map ...') as Action;
CREATE TABLE `gringlobal`.`citation_map` (
`citation_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`citation_id` int(11)    NOT NULL  ,
`accession_id` int(11)    NULL  ,
`method_id` int(11)    NULL  ,
`taxonomy_species_id` int(11)    NULL  ,
`taxonomy_genus_id` int(11)    NULL  ,
`taxonomy_family_id` int(11)    NULL  ,
`accession_ipr_id` int(11)    NULL  ,
`accession_pedigree_id` int(11)    NULL  ,
`genetic_marker_id` int(11)    NULL  ,
`taxonomy_common_name_id` int(11)    NULL  ,
`taxonomy_use_id` int(11)    NULL  ,
`type_code` varchar(20)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`citation_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.code_value *************/
select concat(now(), ' creating table gringlobal.code_value ...') as Action;
CREATE TABLE `gringlobal`.`code_value` (
`code_value_id` int(11)    NOT NULL AUTO_INCREMENT ,
`group_name` varchar(100)    NOT NULL  ,
`value` varchar(20)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`code_value_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.code_value_lang *************/
select concat(now(), ' creating table gringlobal.code_value_lang ...') as Action;
CREATE TABLE `gringlobal`.`code_value_lang` (
`code_value_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`code_value_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`code_value_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.cooperator *************/
select concat(now(), ' creating table gringlobal.cooperator ...') as Action;
CREATE TABLE `gringlobal`.`cooperator` (
`cooperator_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_cooperator_id` int(11)    NULL  ,
`site_id` int(11)    NULL  ,
`last_name` varchar(100)    NULL  ,
`title` varchar(10)    NULL  ,
`first_name` varchar(100)    NULL  ,
`job` varchar(100)    NULL  ,
`organization` varchar(100)    NULL  ,
`organization_abbrev` varchar(10)    NULL  ,
`address_line1` varchar(100)    NULL  ,
`address_line2` varchar(100)    NULL  ,
`address_line3` varchar(100)    NULL  ,
`city` varchar(100)    NULL  ,
`postal_index` varchar(100)    NULL  ,
`geography_id` int(11)    NULL  ,
`secondary_organization` varchar(100)    NULL  ,
`secondary_organization_abbrev` varchar(10)    NULL  ,
`secondary_address_line1` varchar(100)    NULL  ,
`secondary_address_line2` varchar(100)    NULL  ,
`secondary_address_line3` varchar(100)    NULL  ,
`secondary_city` varchar(100)    NULL  ,
`secondary_postal_index` varchar(100)    NULL  ,
`secondary_geography_id` int(11)    NULL  ,
`primary_phone` varchar(30)    NULL  ,
`secondary_phone` varchar(30)    NULL  ,
`fax` varchar(30)    NULL  ,
`email` varchar(100)    NULL  ,
`secondary_email` varchar(100)    NULL  ,
`status_code` varchar(20)    NOT NULL  ,
`category_code` varchar(20)    NULL  ,
`organization_region_code` varchar(20)    NULL  ,
`discipline_code` varchar(20)    NULL  ,
`note` text     NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`cooperator_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.cooperator_group *************/
select concat(now(), ' creating table gringlobal.cooperator_group ...') as Action;
CREATE TABLE `gringlobal`.`cooperator_group` (
`cooperator_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(60)    NULL  ,
`is_group_active` char(1)     NOT NULL  ,
`site_id` int(11)    NULL  ,
`category_code` varchar(20)    NULL  ,
`group_tag` text     NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`cooperator_group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.cooperator_map *************/
select concat(now(), ' creating table gringlobal.cooperator_map ...') as Action;
CREATE TABLE `gringlobal`.`cooperator_map` (
`cooperator_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`cooperator_group_id` int(11)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`cooperator_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop *************/
select concat(now(), ' creating table gringlobal.crop ...') as Action;
CREATE TABLE `gringlobal`.`crop` (
`crop_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(20)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_attach *************/
select concat(now(), ' creating table gringlobal.crop_attach ...') as Action;
CREATE TABLE `gringlobal`.`crop_attach` (
`crop_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_id` int(11)    NOT NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`thumbnail_virtual_path` varchar(255)    NULL  ,
`sort_order` int(11)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`content_type` varchar(100)    NULL  ,
`category_code` varchar(20)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait *************/
select concat(now(), ' creating table gringlobal.crop_trait ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait` (
`crop_trait_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_id` int(11)    NOT NULL  ,
`coded_name` varchar(30)    NOT NULL  ,
`is_peer_reviewed` char(1)     NOT NULL  ,
`category_code` varchar(20)    NOT NULL  ,
`data_type_code` varchar(20)    NOT NULL  ,
`is_coded` char(1)     NOT NULL  ,
`max_length` int(11)    NULL  ,
`numeric_format` varchar(15)    NULL  ,
`numeric_maximum` int(11)    NULL  ,
`numeric_minimum` int(11)    NULL  ,
`original_value_type_code` varchar(20)    NULL  ,
`original_value_format` varchar(50)    NULL  ,
`is_archived` char(1)     NOT NULL  ,
`ontology_url` varchar(300)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_attach *************/
select concat(now(), ' creating table gringlobal.crop_trait_attach ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_attach` (
`crop_trait_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_trait_id` int(11)    NOT NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`thumbnail_virtual_path` varchar(255)    NULL  ,
`sort_order` int(11)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`content_type` varchar(100)    NULL  ,
`category_code` varchar(20)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_code *************/
select concat(now(), ' creating table gringlobal.crop_trait_code ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_code` (
`crop_trait_code_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_trait_id` int(11)    NOT NULL  ,
`code` varchar(30)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_code_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_code_attach *************/
select concat(now(), ' creating table gringlobal.crop_trait_code_attach ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_code_attach` (
`crop_trait_code_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_trait_code_id` int(11)    NOT NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`thumbnail_virtual_path` varchar(255)    NULL  ,
`sort_order` int(11)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`content_type` varchar(100)    NULL  ,
`category_code` varchar(20)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_code_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_code_lang *************/
select concat(now(), ' creating table gringlobal.crop_trait_code_lang ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_code_lang` (
`crop_trait_code_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_trait_code_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_code_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_lang *************/
select concat(now(), ' creating table gringlobal.crop_trait_lang ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_lang` (
`crop_trait_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_trait_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_observation *************/
select concat(now(), ' creating table gringlobal.crop_trait_observation ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_observation` (
`crop_trait_observation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`crop_trait_id` int(11)    NOT NULL  ,
`crop_trait_code_id` int(11)    NULL  ,
`numeric_value` decimal(18, 5)    NULL  ,
`string_value` varchar(400)    NULL  ,
`method_id` int(11)    NOT NULL  ,
`is_archived` char(1)     NOT NULL  ,
`data_quality_code` varchar(20)    NULL  ,
`original_value` varchar(30)    NULL  ,
`frequency` decimal(18, 5)    NULL  ,
`rank` int(11)    NULL  ,
`mean_value` decimal(18, 5)    NULL  ,
`maximum_value` decimal(18, 5)    NULL  ,
`minimum_value` decimal(18, 5)    NULL  ,
`standard_deviation` decimal(18, 5)    NULL  ,
`sample_size` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_observation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.crop_trait_observation_data *************/
select concat(now(), ' creating table gringlobal.crop_trait_observation_data ...') as Action;
CREATE TABLE `gringlobal`.`crop_trait_observation_data` (
`crop_trait_observation_data_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_trait_observation_id` int(11)    NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`individual` int(11)    NOT NULL  ,
`crop_trait_id` int(11)    NULL  ,
`crop_trait_code_id` int(11)    NULL  ,
`numeric_value` decimal(18, 5)    NULL  ,
`string_value` varchar(400)    NULL  ,
`method_id` int(11)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`crop_trait_observation_data_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.genetic_annotation *************/
select concat(now(), ' creating table gringlobal.genetic_annotation ...') as Action;
CREATE TABLE `gringlobal`.`genetic_annotation` (
`genetic_annotation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`genetic_marker_id` int(11)    NOT NULL  ,
`method_id` int(11)    NOT NULL  ,
`assay_method` text     NULL  ,
`scoring_method` text     NULL  ,
`control_values` text     NULL  ,
`observation_alleles_count` int(11)    NULL  ,
`max_gob_alleles` int(11)    NULL  ,
`size_alleles` varchar(100)    NULL  ,
`unusual_alleles` varchar(100)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genetic_annotation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.genetic_marker *************/
select concat(now(), ' creating table gringlobal.genetic_marker ...') as Action;
CREATE TABLE `gringlobal`.`genetic_marker` (
`genetic_marker_id` int(11)    NOT NULL AUTO_INCREMENT ,
`crop_id` int(11)    NOT NULL  ,
`name` varchar(100)    NOT NULL  ,
`synonyms` varchar(200)    NULL  ,
`repeat_motif` varchar(100)    NULL  ,
`primers` varchar(200)    NULL  ,
`assay_conditions` text     NULL  ,
`range_products` varchar(60)    NULL  ,
`genbank_number` varchar(20)    NULL  ,
`known_standards` text     NULL  ,
`map_location` varchar(100)    NULL  ,
`position` text     NULL  ,
`poly_type` varchar(10)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genetic_marker_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.genetic_observation *************/
select concat(now(), ' creating table gringlobal.genetic_observation ...') as Action;
CREATE TABLE `gringlobal`.`genetic_observation` (
`genetic_observation_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`genetic_annotation_id` int(11)    NULL  ,
`is_archived` char(1)     NOT NULL  ,
`data_quality_code` varchar(20)    NULL  ,
`frequency` decimal(18, 5)    NULL  ,
`value` text     NULL  ,
`rank` int(11)    NULL  ,
`mean_value` decimal(18, 5)    NULL  ,
`maximum_value` decimal(18, 5)    NULL  ,
`minimum_value` decimal(18, 5)    NULL  ,
`standard_deviation` decimal(18, 5)    NULL  ,
`sample_size` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genetic_observation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.genetic_observation_data *************/
select concat(now(), ' creating table gringlobal.genetic_observation_data ...') as Action;
CREATE TABLE `gringlobal`.`genetic_observation_data` (
`genetic_observation_data_id` int(11)    NOT NULL AUTO_INCREMENT ,
`genetic_observation_id` int(11)    NULL  ,
`genetic_annotation_id` int(11)    NOT NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`individual` int(11)    NULL  ,
`individual_allele_number` int(11)    NULL  ,
`value` text     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`genetic_observation_data_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.geography *************/
select concat(now(), ' creating table gringlobal.geography ...') as Action;
CREATE TABLE `gringlobal`.`geography` (
`geography_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_geography_id` int(11)    NULL  ,
`country_code` varchar(20)    NOT NULL  ,
`adm1` varchar(50)    NULL  ,
`adm1_type_code` varchar(20)    NULL  ,
`adm2` varchar(50)    NULL  ,
`adm2_type_code` varchar(20)    NULL  ,
`adm3` varchar(50)    NULL  ,
`adm3_type_code` varchar(20)    NULL  ,
`adm4` varchar(50)    NULL  ,
`adm4_type_code` varchar(20)    NULL  ,
`changed_date` datetime    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`geography_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.geography_lang *************/
select concat(now(), ' creating table gringlobal.geography_lang ...') as Action;
CREATE TABLE `gringlobal`.`geography_lang` (
`geography_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`geography_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`geography_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.geography_region_map *************/
select concat(now(), ' creating table gringlobal.geography_region_map ...') as Action;
CREATE TABLE `gringlobal`.`geography_region_map` (
`geography_region_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`geography_id` int(11)    NOT NULL  ,
`region_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`geography_region_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory *************/
select concat(now(), ' creating table gringlobal.inventory ...') as Action;
CREATE TABLE `gringlobal`.`inventory` (
`inventory_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_number_part1` varchar(50)    NOT NULL  ,
`inventory_number_part2` int(11)    NULL  ,
`inventory_number_part3` varchar(50)    NULL  ,
`form_type_code` varchar(20)    NOT NULL  ,
`inventory_maint_policy_id` int(11)    NOT NULL  ,
`is_distributable` char(1)     NOT NULL  ,
`storage_location_part1` varchar(20)    NULL  ,
`storage_location_part2` varchar(20)    NULL  ,
`storage_location_part3` varchar(20)    NULL  ,
`storage_location_part4` varchar(20)    NULL  ,
`latitude` decimal(18, 8)    NULL  ,
`longitude` decimal(18, 8)    NULL  ,
`is_available` char(1)     NOT NULL  ,
`web_availability_note` text     NULL  ,
`availability_status_code` varchar(20)    NOT NULL  ,
`availability_status_note` text     NULL  ,
`availability_start_date` datetime    NULL  ,
`availability_end_date` datetime    NULL  ,
`quantity_on_hand` int(11)    NULL  ,
`quantity_on_hand_unit_code` varchar(20)    NULL  ,
`is_auto_deducted` char(1)     NOT NULL  ,
`distribution_default_form_code` varchar(20)    NULL  ,
`distribution_default_quantity` int(11)    NULL  ,
`distribution_unit_code` varchar(20)    NULL  ,
`distribution_critical_quantity` int(11)    NULL  ,
`replenishment_critical_quantity` int(11)    NULL  ,
`pathogen_status_code` varchar(20)    NULL  ,
`accession_id` int(11)    NOT NULL  ,
`parent_inventory_id` int(11)    NULL  ,
`backup_inventory_id` int(11)    NULL  ,
`rootstock` varchar(200)    NULL  ,
`hundred_seed_weight` decimal(18, 5)    NULL  ,
`pollination_method_code` varchar(20)    NULL  ,
`pollination_vector_code` varchar(20)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_action *************/
select concat(now(), ' creating table gringlobal.inventory_action ...') as Action;
CREATE TABLE `gringlobal`.`inventory_action` (
`inventory_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`action_name_code` varchar(20)    NOT NULL  ,
`action_date` datetime    NOT NULL  ,
`action_date_code` varchar(20)    NULL  ,
`quantity` int(11)    NULL  ,
`quantity_unit_code` varchar(20)    NULL  ,
`form_code` varchar(20)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`method_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_action_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_attach *************/
select concat(now(), ' creating table gringlobal.inventory_attach ...') as Action;
CREATE TABLE `gringlobal`.`inventory_attach` (
`inventory_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`thumbnail_virtual_path` varchar(255)    NULL  ,
`sort_order` int(11)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`content_type` varchar(100)    NULL  ,
`category_code` varchar(20)    NULL  ,
`copyright_information` varchar(100)    NULL  ,
`attach_cooperator_id` int(11)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_group *************/
select concat(now(), ' creating table gringlobal.inventory_group ...') as Action;
CREATE TABLE `gringlobal`.`inventory_group` (
`inventory_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`group_name` varchar(100)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_group_map *************/
select concat(now(), ' creating table gringlobal.inventory_group_map ...') as Action;
CREATE TABLE `gringlobal`.`inventory_group_map` (
`inventory_group_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`inventory_group_id` int(11)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_group_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_maint_policy *************/
select concat(now(), ' creating table gringlobal.inventory_maint_policy ...') as Action;
CREATE TABLE `gringlobal`.`inventory_maint_policy` (
`inventory_maint_policy_id` int(11)    NOT NULL AUTO_INCREMENT ,
`maintenance_name` varchar(50)    NOT NULL  ,
`form_type_code` varchar(20)    NOT NULL  ,
`on_hand_unit_code` varchar(20)    NULL  ,
`web_availability_note` text     NULL  ,
`is_auto_deducted` char(1)     NOT NULL  ,
`distribution_default_form_code` varchar(20)    NOT NULL  ,
`distribution_default_quantity` int(11)    NULL  ,
`distribution_unit_code` varchar(20)    NULL  ,
`distribution_critical_quantity` int(11)    NULL  ,
`replenishment_critical_quantity` int(11)    NULL  ,
`regeneration_method_code` varchar(20)    NULL  ,
`curator_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_maint_policy_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_name *************/
select concat(now(), ' creating table gringlobal.inventory_name ...') as Action;
CREATE TABLE `gringlobal`.`inventory_name` (
`inventory_name_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`category_code` varchar(20)    NOT NULL  ,
`plant_name` varchar(100)    NOT NULL  ,
`plant_name_rank` int(11)    NOT NULL  ,
`name_group_id` int(11)    NULL  ,
`name_source_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_name_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_quality_status *************/
select concat(now(), ' creating table gringlobal.inventory_quality_status ...') as Action;
CREATE TABLE `gringlobal`.`inventory_quality_status` (
`inventory_quality_status_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`test_type_code` varchar(20)    NOT NULL  ,
`contaminant_code` varchar(20)    NOT NULL  ,
`test_result_code` varchar(20)    NULL  ,
`started_date` datetime    NULL  ,
`completed_date` datetime    NULL  ,
`required_replication_count` int(11)    NULL  ,
`started_count` int(11)    NULL  ,
`completed_count` int(11)    NULL  ,
`method_id` int(11)    NULL  ,
`tester_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_quality_status_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_viability *************/
select concat(now(), ' creating table gringlobal.inventory_viability ...') as Action;
CREATE TABLE `gringlobal`.`inventory_viability` (
`inventory_viability_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`inventory_viability_rule_id` int(11)    NULL  ,
`tested_date` datetime    NOT NULL  ,
`tested_date_code` varchar(20)    NULL  ,
`percent_normal` int(11)    NULL  ,
`percent_abnormal` int(11)    NULL  ,
`percent_dormant` int(11)    NULL  ,
`percent_viable` int(11)    NULL  ,
`vigor_rating_code` varchar(20)    NULL  ,
`total_tested_count` int(11)    NULL  ,
`replication_count` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_viability_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_viability_data *************/
select concat(now(), ' creating table gringlobal.inventory_viability_data ...') as Action;
CREATE TABLE `gringlobal`.`inventory_viability_data` (
`inventory_viability_data_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_viability_id` int(11)    NOT NULL  ,
`order_request_item_id` int(11)    NULL  ,
`counter_cooperator_id` int(11)    NULL  ,
`replication_number` int(11)    NOT NULL  ,
`count_number` int(11)    NOT NULL  ,
`count_date` datetime    NOT NULL  ,
`normal_count` int(11)    NOT NULL  ,
`abnormal_count` int(11)    NULL  ,
`dormant_count` int(11)    NULL  ,
`dead_count` int(11)    NULL  ,
`unknown_count` int(11)    NULL  ,
`replication_count` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_viability_data_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.inventory_viability_rule *************/
select concat(now(), ' creating table gringlobal.inventory_viability_rule ...') as Action;
CREATE TABLE `gringlobal`.`inventory_viability_rule` (
`inventory_viability_rule_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_species_id` int(11)    NOT NULL  ,
`name` varchar(100)    NULL  ,
`substrata` varchar(100)    NULL  ,
`temperature_range` varchar(30)    NULL  ,
`requirements` text     NULL  ,
`category_code` varchar(20)    NULL  ,
`count_regime_days` varchar(50)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`inventory_viability_rule_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.literature *************/
select concat(now(), ' creating table gringlobal.literature ...') as Action;
CREATE TABLE `gringlobal`.`literature` (
`literature_id` int(11)    NOT NULL AUTO_INCREMENT ,
`abbreviation` varchar(20)    NOT NULL  ,
`standard_abbreviation` text     NULL  ,
`reference_title` text     NULL  ,
`editor_author_name` text     NULL  ,
`literature_type_code` varchar(20)    NULL  ,
`publication_year` int(11)    NULL  ,
`publisher_name` text     NULL  ,
`publisher_location` text     NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`literature_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.method *************/
select concat(now(), ' creating table gringlobal.method ...') as Action;
CREATE TABLE `gringlobal`.`method` (
`method_id` int(11)    NOT NULL AUTO_INCREMENT ,
`name` varchar(100)    NOT NULL  ,
`geography_id` int(11)    NULL  ,
`elevation_meters` int(11)    NULL  ,
`latitude` decimal(18, 0)    NULL  ,
`longitude` decimal(18, 0)    NULL  ,
`uncertainty` int(11)    NULL  ,
`formatted_locality` text     NULL  ,
`georeference_datum` varchar(10)    NULL  ,
`georeference_protocol_code` varchar(20)    NULL  ,
`georeference_annotation` text     NULL  ,
`materials_and_methods` text     NULL  ,
`study_reason_code` varchar(20)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`method_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.method_map *************/
select concat(now(), ' creating table gringlobal.method_map ...') as Action;
CREATE TABLE `gringlobal`.`method_map` (
`method_cooperator_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`cooperator_id` int(11)    NOT NULL  ,
`method_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`method_cooperator_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.name_group *************/
select concat(now(), ' creating table gringlobal.name_group ...') as Action;
CREATE TABLE `gringlobal`.`name_group` (
`name_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`group_name` varchar(20)    NOT NULL  ,
`note` text     NULL  ,
`url` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`name_group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.order_request *************/
select concat(now(), ' creating table gringlobal.order_request ...') as Action;
CREATE TABLE `gringlobal`.`order_request` (
`order_request_id` int(11)    NOT NULL AUTO_INCREMENT ,
`original_order_request_id` int(11)    NULL  ,
`web_order_request_id` int(11)    NULL  ,
`local_number` int(11)    NULL  ,
`order_type_code` varchar(20)    NULL  ,
`ordered_date` datetime    NULL  ,
`intended_use_code` varchar(20)    NULL  ,
`intended_use_note` text     NULL  ,
`completed_date` datetime    NULL  ,
`requestor_cooperator_id` int(11)    NULL  ,
`ship_to_cooperator_id` int(11)    NULL  ,
`final_recipient_cooperator_id` int(11)    NOT NULL  ,
`order_obtained_via` varchar(20)    NULL  ,
`special_instruction` text     NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_request_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.order_request_action *************/
select concat(now(), ' creating table gringlobal.order_request_action ...') as Action;
CREATE TABLE `gringlobal`.`order_request_action` (
`order_request_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`order_request_id` int(11)    NOT NULL  ,
`action_name_code` varchar(20)    NOT NULL  ,
`started_date` datetime    NOT NULL  ,
`started_date_code` varchar(20)    NULL  ,
`completed_date` datetime    NULL  ,
`completed_date_code` varchar(20)    NULL  ,
`action_information` varchar(100)    NULL  ,
`action_cost` decimal(18, 5)    NULL  ,
`cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_request_action_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.order_request_attach *************/
select concat(now(), ' creating table gringlobal.order_request_attach ...') as Action;
CREATE TABLE `gringlobal`.`order_request_attach` (
`order_request_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`order_request_id` int(11)    NOT NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`thumbnail_virtual_path` varchar(255)    NULL  ,
`sort_order` int(11)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`content_type` varchar(100)    NULL  ,
`category_code` varchar(20)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`copyright_information` varchar(100)    NULL  ,
`attach_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_request_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.order_request_item *************/
select concat(now(), ' creating table gringlobal.order_request_item ...') as Action;
CREATE TABLE `gringlobal`.`order_request_item` (
`order_request_item_id` int(11)    NOT NULL AUTO_INCREMENT ,
`order_request_id` int(11)    NOT NULL  ,
`web_order_request_item_id` int(11)    NULL  ,
`sequence_number` int(11)    NULL  ,
`name` varchar(100)    NULL  ,
`quantity_shipped` int(11)    NULL  ,
`quantity_shipped_unit_code` varchar(20)    NULL  ,
`distribution_form_code` varchar(20)    NULL  ,
`status_code` varchar(20)    NULL  ,
`status_date` datetime    NULL  ,
`inventory_id` int(11)    NOT NULL  ,
`external_taxonomy` varchar(100)    NULL  ,
`source_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`web_user_note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`order_request_item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.region *************/
select concat(now(), ' creating table gringlobal.region ...') as Action;
CREATE TABLE `gringlobal`.`region` (
`region_id` int(11)    NOT NULL AUTO_INCREMENT ,
`continent` varchar(20)    NOT NULL  ,
`subcontinent` varchar(30)    NULL  ,
`sequence_number` int(11)    NULL  ,
`continent_abbreviation` varchar(20)    NULL  ,
`subcontinent_abbreviation` varchar(20)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`region_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.region_lang *************/
select concat(now(), ' creating table gringlobal.region_lang ...') as Action;
CREATE TABLE `gringlobal`.`region_lang` (
`region_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`region_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`region_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.site *************/
select concat(now(), ' creating table gringlobal.site ...') as Action;
CREATE TABLE `gringlobal`.`site` (
`site_id` int(11)    NOT NULL AUTO_INCREMENT ,
`site_short_name` varchar(20)    NOT NULL  ,
`site_long_name` varchar(100)    NOT NULL  ,
`organization_abbrev` varchar(20)    NULL  ,
`is_internal` char(1)     NOT NULL  ,
`is_distribution_site` char(1)     NOT NULL  ,
`type_code` varchar(20)    NULL  ,
`fao_institute_number` varchar(20)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`site_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.site_inventory_nc7 *************/
select concat(now(), ' creating table gringlobal.site_inventory_nc7 ...') as Action;
CREATE TABLE `gringlobal`.`site_inventory_nc7` (
`site_inventory_nc7_id` int(11)    NOT NULL AUTO_INCREMENT ,
`inventory_id` int(11)    NOT NULL  ,
`hundred_weight` decimal(18, 5)    NULL  ,
`pollination_control` varchar(10)    NULL  ,
`farm_field_identifier` varchar(10)    NULL  ,
`location_type_code` varchar(10)    NULL  ,
`location_low` varchar(10)    NULL  ,
`location_high` varchar(10)    NULL  ,
`sublocation_type_code` varchar(10)    NULL  ,
`sublocation_low` varchar(10)    NULL  ,
`sublocation_high` varchar(10)    NULL  ,
`old_inventory_identifier` varchar(30)    NULL  ,
`inventory_site_note` text     NULL  ,
`inventory_location1_latitude` decimal(18, 8)    NULL  ,
`inventory_location1_longitude` decimal(18, 8)    NULL  ,
`inventory_location1_precision` int(11)    NULL  ,
`inventory_location2_latitude` decimal(18, 8)    NULL  ,
`inventory_location2_longitude` decimal(18, 8)    NULL  ,
`inventory_location2_precision` int(11)    NULL  ,
`inventory_datum` varchar(10)    NULL  ,
`coordinates_apply_to_code` varchar(10)    NULL  ,
`coordinates_comment` text     NULL  ,
`coordinates_voucher_location` varchar(500)    NULL  ,
`irregular_inventory_location` varchar(500)    NULL  ,
`is_increase_success_flag` char(1)     NULL  ,
`reason_unsuccessfull1_code` varchar(10)    NULL  ,
`reason_unsuccessfull2_code` varchar(10)    NULL  ,
`reason_unsuccessfull3_code` varchar(10)    NULL  ,
`reason_unsuccessfull_note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`site_inventory_nc7_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_database *************/
select concat(now(), ' creating table gringlobal.sys_database ...') as Action;
CREATE TABLE `gringlobal`.`sys_database` (
`sys_database_id` int(11)    NOT NULL AUTO_INCREMENT ,
`migration_number` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_database_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_database_migration *************/
select concat(now(), ' creating table gringlobal.sys_database_migration ...') as Action;
CREATE TABLE `gringlobal`.`sys_database_migration` (
`sys_database_migration_id` int(11)    NOT NULL AUTO_INCREMENT ,
`migration_number` int(11)    NOT NULL  ,
`sort_order` int(11)    NOT NULL  ,
`action_type` varchar(50)    NOT NULL  ,
`action_up` text     NULL  ,
`action_down` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_database_migration_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_database_migration_lang *************/
select concat(now(), ' creating table gringlobal.sys_database_migration_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_database_migration_lang` (
`sys_database_migration_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_database_migration_id` int(11)    NOT NULL  ,
`language_iso_639_3_code` varchar(5)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_database_migration_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_datatrigger *************/
select concat(now(), ' creating table gringlobal.sys_datatrigger ...') as Action;
CREATE TABLE `gringlobal`.`sys_datatrigger` (
`sys_datatrigger_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_id` int(11)    NULL  ,
`sys_table_id` int(11)    NULL  ,
`virtual_file_path` varchar(255)    NULL  ,
`assembly_name` varchar(255)    NOT NULL  ,
`fully_qualified_class_name` varchar(255)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`is_system` char(1)     NOT NULL  ,
`sort_order` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_datatrigger_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_datatrigger_lang *************/
select concat(now(), ' creating table gringlobal.sys_datatrigger_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_datatrigger_lang` (
`sys_datatrigger_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_datatrigger_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_datatrigger_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_dataview *************/
select concat(now(), ' creating table gringlobal.sys_dataview ...') as Action;
CREATE TABLE `gringlobal`.`sys_dataview` (
`sys_dataview_id` int(11)    NOT NULL AUTO_INCREMENT ,
`dataview_name` varchar(100)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`is_readonly` char(1)     NOT NULL  ,
`category_name` varchar(100)    NULL  ,
`database_area` varchar(50)    NULL  ,
`database_area_sort_order` int(11)    NULL  ,
`is_transform` char(1)     NOT NULL  ,
`transform_field_for_names` varchar(50)    NULL  ,
`transform_field_for_captions` varchar(50)    NULL  ,
`transform_field_for_values` varchar(50)    NULL  ,
`configuration_options` varchar(500)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_dataview_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_dataview_field *************/
select concat(now(), ' creating table gringlobal.sys_dataview_field ...') as Action;
CREATE TABLE `gringlobal`.`sys_dataview_field` (
`sys_dataview_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_id` int(11)    NOT NULL  ,
`field_name` varchar(50)    NOT NULL  ,
`sys_table_field_id` int(11)    NULL  ,
`is_readonly` char(1)     NOT NULL  ,
`is_primary_key` char(1)     NOT NULL  ,
`is_transform` char(1)     NOT NULL  ,
`sort_order` int(11)    NOT NULL  ,
`gui_hint` varchar(100)    NULL  ,
`foreign_key_dataview_name` varchar(50)    NULL  ,
`group_name` varchar(100)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_dataview_field_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_dataview_field_lang *************/
select concat(now(), ' creating table gringlobal.sys_dataview_field_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_dataview_field_lang` (
`sys_dataview_field_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_field_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_dataview_field_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_dataview_lang *************/
select concat(now(), ' creating table gringlobal.sys_dataview_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_dataview_lang` (
`sys_dataview_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_dataview_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_dataview_param *************/
select concat(now(), ' creating table gringlobal.sys_dataview_param ...') as Action;
CREATE TABLE `gringlobal`.`sys_dataview_param` (
`sys_dataview_param_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_id` int(11)    NOT NULL  ,
`param_name` varchar(50)    NOT NULL  ,
`param_type` varchar(50)    NULL  ,
`sort_order` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_dataview_param_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_dataview_sql *************/
select concat(now(), ' creating table gringlobal.sys_dataview_sql ...') as Action;
CREATE TABLE `gringlobal`.`sys_dataview_sql` (
`sys_dataview_sql_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_id` int(11)    NOT NULL  ,
`database_engine_tag` varchar(10)    NOT NULL  ,
`sql_statement` text     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_dataview_sql_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_file *************/
select concat(now(), ' creating table gringlobal.sys_file ...') as Action;
CREATE TABLE `gringlobal`.`sys_file` (
`sys_file_id` int(11)    NOT NULL AUTO_INCREMENT ,
`is_enabled` char(1)     NOT NULL  ,
`virtual_file_path` varchar(255)    NOT NULL  ,
`file_name` varchar(255)    NULL  ,
`file_version` varchar(255)    NULL  ,
`file_size` decimal(18, 0)    NULL  ,
`display_name` varchar(255)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_file_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_file_group *************/
select concat(now(), ' creating table gringlobal.sys_file_group ...') as Action;
CREATE TABLE `gringlobal`.`sys_file_group` (
`sys_file_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`group_name` varchar(100)    NOT NULL  ,
`version_name` varchar(50)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_file_group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_file_group_map *************/
select concat(now(), ' creating table gringlobal.sys_file_group_map ...') as Action;
CREATE TABLE `gringlobal`.`sys_file_group_map` (
`sys_file_group_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_file_group_id` int(11)    NOT NULL  ,
`sys_file_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_file_group_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_file_lang *************/
select concat(now(), ' creating table gringlobal.sys_file_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_file_lang` (
`sys_file_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_file_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_file_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_group *************/
select concat(now(), ' creating table gringlobal.sys_group ...') as Action;
CREATE TABLE `gringlobal`.`sys_group` (
`sys_group_id` int(11)    NOT NULL AUTO_INCREMENT ,
`group_tag` text     NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_group_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_group_lang *************/
select concat(now(), ' creating table gringlobal.sys_group_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_group_lang` (
`sys_group_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_group_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_group_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_group_permission_map *************/
select concat(now(), ' creating table gringlobal.sys_group_permission_map ...') as Action;
CREATE TABLE `gringlobal`.`sys_group_permission_map` (
`sys_group_permission_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_group_id` int(11)    NOT NULL  ,
`sys_permission_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_group_permission_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_group_user_map *************/
select concat(now(), ' creating table gringlobal.sys_group_user_map ...') as Action;
CREATE TABLE `gringlobal`.`sys_group_user_map` (
`sys_group_user_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_group_id` int(11)    NOT NULL  ,
`sys_user_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_group_user_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_index *************/
select concat(now(), ' creating table gringlobal.sys_index ...') as Action;
CREATE TABLE `gringlobal`.`sys_index` (
`sys_index_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_table_id` int(11)    NOT NULL  ,
`index_name` varchar(50)    NOT NULL  ,
`is_unique` char(1)     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_index_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_index_field *************/
select concat(now(), ' creating table gringlobal.sys_index_field ...') as Action;
CREATE TABLE `gringlobal`.`sys_index_field` (
`sys_index_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_index_id` int(11)    NOT NULL  ,
`sys_table_field_id` int(11)    NOT NULL  ,
`sort_order` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_index_field_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_lang *************/
select concat(now(), ' creating table gringlobal.sys_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_lang` (
`sys_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`iso_639_3_tag` varchar(5)    NOT NULL  ,
`ietf_tag` varchar(30)    NULL  ,
`script_direction` varchar(3)    NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_permission *************/
select concat(now(), ' creating table gringlobal.sys_permission ...') as Action;
CREATE TABLE `gringlobal`.`sys_permission` (
`sys_permission_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_dataview_id` int(11)    NULL  ,
`sys_table_id` int(11)    NULL  ,
`permission_tag` text     NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`create_permission` char(1)     NOT NULL  ,
`read_permission` char(1)     NOT NULL  ,
`update_permission` char(1)     NOT NULL  ,
`delete_permission` char(1)     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_permission_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_permission_field *************/
select concat(now(), ' creating table gringlobal.sys_permission_field ...') as Action;
CREATE TABLE `gringlobal`.`sys_permission_field` (
`sys_permission_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_permission_id` int(11)    NOT NULL  ,
`sys_dataview_field_id` int(11)    NULL  ,
`sys_table_field_id` int(11)    NULL  ,
`field_type` varchar(20)    NULL  ,
`compare_operator` varchar(20)    NULL  ,
`compare_value` text     NULL  ,
`parent_table_field_id` int(11)    NULL  ,
`parent_field_type` varchar(20)    NULL  ,
`parent_compare_operator` varchar(20)    NULL  ,
`parent_compare_value` text     NULL  ,
`compare_mode` varchar(20)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_permission_field_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_permission_lang *************/
select concat(now(), ' creating table gringlobal.sys_permission_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_permission_lang` (
`sys_permission_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_permission_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_permission_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_table *************/
select concat(now(), ' creating table gringlobal.sys_table ...') as Action;
CREATE TABLE `gringlobal`.`sys_table` (
`sys_table_id` int(11)    NOT NULL AUTO_INCREMENT ,
`table_name` varchar(50)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`is_readonly` char(1)     NOT NULL  ,
`audits_created` char(1)     NOT NULL  ,
`audits_modified` char(1)     NOT NULL  ,
`audits_owned` char(1)     NOT NULL  ,
`database_area` varchar(100)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_table_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_table_field *************/
select concat(now(), ' creating table gringlobal.sys_table_field ...') as Action;
CREATE TABLE `gringlobal`.`sys_table_field` (
`sys_table_field_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_table_id` int(11)    NOT NULL  ,
`field_name` varchar(50)    NOT NULL  ,
`field_ordinal` int(11)    NULL  ,
`field_purpose` varchar(50)    NOT NULL  ,
`field_type` varchar(50)    NOT NULL  ,
`default_value` varchar(50)    NULL  ,
`is_primary_key` char(1)     NOT NULL  ,
`is_foreign_key` char(1)     NOT NULL  ,
`foreign_key_table_field_id` int(11)    NULL  ,
`foreign_key_dataview_name` varchar(100)    NULL  ,
`is_nullable` char(1)     NOT NULL  ,
`gui_hint` varchar(50)    NOT NULL  ,
`is_readonly` char(1)     NOT NULL  ,
`min_length` int(11)    NOT NULL  ,
`max_length` int(11)    NOT NULL  ,
`numeric_precision` int(11)    NOT NULL  ,
`numeric_scale` int(11)    NOT NULL  ,
`is_autoincrement` char(1)     NOT NULL  ,
`group_name` varchar(100)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_table_field_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_table_field_lang *************/
select concat(now(), ' creating table gringlobal.sys_table_field_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_table_field_lang` (
`sys_table_field_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_table_field_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_table_field_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_table_lang *************/
select concat(now(), ' creating table gringlobal.sys_table_lang ...') as Action;
CREATE TABLE `gringlobal`.`sys_table_lang` (
`sys_table_lang_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_table_id` int(11)    NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`title` varchar(500)    NOT NULL  ,
`description` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_table_lang_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_table_relationship *************/
select concat(now(), ' creating table gringlobal.sys_table_relationship ...') as Action;
CREATE TABLE `gringlobal`.`sys_table_relationship` (
`sys_table_relationship_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_table_field_id` int(11)    NULL  ,
`relationship_type_tag` varchar(20)    NOT NULL  ,
`other_table_field_id` int(11)    NULL  ,
`created_by` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`modified_by` int(11)    NULL  ,
`modified_date` datetime    NULL  ,
`owned_by` int(11)    NOT NULL  ,
`owned_date` datetime    NOT NULL  ,
PRIMARY KEY (`sys_table_relationship_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_user *************/
select concat(now(), ' creating table gringlobal.sys_user ...') as Action;
CREATE TABLE `gringlobal`.`sys_user` (
`sys_user_id` int(11)    NOT NULL AUTO_INCREMENT ,
`user_name` varchar(50)    NOT NULL  ,
`password` varchar(255)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`cooperator_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.sys_user_permission_map *************/
select concat(now(), ' creating table gringlobal.sys_user_permission_map ...') as Action;
CREATE TABLE `gringlobal`.`sys_user_permission_map` (
`sys_user_permission_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`sys_user_id` int(11)    NOT NULL  ,
`sys_permission_id` int(11)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`sys_user_permission_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_alt_family_map *************/
select concat(now(), ' creating table gringlobal.taxonomy_alt_family_map ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_alt_family_map` (
`taxonomy_alt_family_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_genus_id` int(11)    NOT NULL  ,
`taxonomy_family_id` int(11)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_alt_family_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_attach *************/
select concat(now(), ' creating table gringlobal.taxonomy_attach ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_attach` (
`taxonomy_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_family_id` int(11)    NULL  ,
`taxonomy_genus_id` int(11)    NULL  ,
`taxonomy_species_id` int(11)    NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`thumbnail_virtual_path` varchar(255)    NULL  ,
`sort_order` int(11)    NULL  ,
`title` varchar(500)    NULL  ,
`description` varchar(500)    NULL  ,
`content_type` varchar(100)    NULL  ,
`category_code` varchar(20)    NULL  ,
`is_web_visible` char(1)     NOT NULL  ,
`copyright_information` varchar(100)    NULL  ,
`attach_cooperator_id` int(11)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_author *************/
select concat(now(), ' creating table gringlobal.taxonomy_author ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_author` (
`taxonomy_author_id` int(11)    NOT NULL AUTO_INCREMENT ,
`short_name` varchar(30)    NOT NULL  ,
`full_name` varchar(100)    NOT NULL  ,
`short_name_expanded_diacritic` varchar(30)    NULL  ,
`full_name_expanded_diacritic` varchar(100)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_author_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_common_name *************/
select concat(now(), ' creating table gringlobal.taxonomy_common_name ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_common_name` (
`taxonomy_common_name_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_genus_id` int(11)    NULL  ,
`taxonomy_species_id` int(11)    NULL  ,
`language_description` varchar(100)    NULL  ,
`name` varchar(100)    NOT NULL  ,
`simplified_name` varchar(100)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_common_name_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_crop_map *************/
select concat(now(), ' creating table gringlobal.taxonomy_crop_map ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_crop_map` (
`taxonomy_crop_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_species_id` int(11)    NOT NULL  ,
`crop_id` int(11)    NOT NULL  ,
`alternate_crop_name` varchar(100)    NOT NULL  ,
`common_crop_name` varchar(100)    NOT NULL  ,
`is_primary_genepool` char(1)     NOT NULL  ,
`is_secondary_genepool` char(1)     NOT NULL  ,
`is_tertiary_genepool` char(1)     NOT NULL  ,
`is_quaternary_genepool` char(1)     NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_crop_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_family *************/
select concat(now(), ' creating table gringlobal.taxonomy_family ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_family` (
`taxonomy_family_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_taxonomy_family_id` int(11)    NULL  ,
`type_taxonomy_genus_id` int(11)    NULL  ,
`suprafamily_rank_code` varchar(20)    NULL  ,
`suprafamily_rank_name` varchar(100)    NULL  ,
`family_name` varchar(25)    NOT NULL  ,
`author_name` varchar(100)    NULL  ,
`alternate_name` varchar(25)    NULL  ,
`subfamily_name` varchar(25)    NULL  ,
`tribe_name` varchar(25)    NULL  ,
`subtribe_name` varchar(25)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_family_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_genus *************/
select concat(now(), ' creating table gringlobal.taxonomy_genus ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_genus` (
`taxonomy_genus_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_taxonomy_genus_id` int(11)    NULL  ,
`taxonomy_family_id` int(11)    NOT NULL  ,
`qualifying_code` varchar(20)    NULL  ,
`is_hybrid` char(1)     NOT NULL  ,
`genus_name` varchar(30)    NOT NULL  ,
`genus_authority` varchar(100)    NULL  ,
`subgenus_name` varchar(30)    NULL  ,
`section_name` varchar(30)    NULL  ,
`subsection_name` varchar(30)    NULL  ,
`series_name` varchar(30)    NULL  ,
`subseries_name` varchar(30)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_genus_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_geography_map *************/
select concat(now(), ' creating table gringlobal.taxonomy_geography_map ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_geography_map` (
`taxonomy_geography_map_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_species_id` int(11)    NOT NULL  ,
`geography_id` int(11)    NULL  ,
`geography_status_code` varchar(20)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_geography_map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_noxious *************/
select concat(now(), ' creating table gringlobal.taxonomy_noxious ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_noxious` (
`taxonomy_noxious_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_species_id` int(11)    NOT NULL  ,
`geography_id` int(11)    NOT NULL  ,
`noxious_type_code` varchar(20)    NOT NULL  ,
`noxious_level_code` varchar(20)    NULL  ,
`url` varchar(300)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_noxious_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_species *************/
select concat(now(), ' creating table gringlobal.taxonomy_species ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_species` (
`taxonomy_species_id` int(11)    NOT NULL AUTO_INCREMENT ,
`current_taxonomy_species_id` int(11)    NULL  ,
`nomen_number` int(11)    NULL  ,
`is_specific_hybrid` char(1)     NOT NULL  ,
`species_name` varchar(30)    NOT NULL  ,
`species_authority` varchar(100)    NULL  ,
`is_subspecific_hybrid` char(1)     NOT NULL  ,
`subspecies_name` varchar(30)    NULL  ,
`subspecies_authority` varchar(100)    NULL  ,
`is_varietal_hybrid` char(1)     NOT NULL  ,
`variety_name` varchar(30)    NULL  ,
`variety_authority` varchar(100)    NULL  ,
`is_subvarietal_hybrid` char(1)     NOT NULL  ,
`subvariety_name` varchar(30)    NULL  ,
`subvariety_authority` varchar(100)    NULL  ,
`is_forma_hybrid` char(1)     NOT NULL  ,
`forma_rank_type` varchar(30)    NULL  ,
`forma_name` varchar(30)    NULL  ,
`forma_authority` varchar(100)    NULL  ,
`taxonomy_genus_id` int(11)    NOT NULL  ,
`priority1_site_id` int(11)    NULL  ,
`priority2_site_id` int(11)    NULL  ,
`curator1_cooperator_id` int(11)    NULL  ,
`curator2_cooperator_id` int(11)    NULL  ,
`restriction_code` varchar(20)    NULL  ,
`life_form_code` varchar(20)    NULL  ,
`common_fertilization_code` varchar(20)    NULL  ,
`is_name_pending` char(1)     NOT NULL  ,
`synonym_code` varchar(20)    NULL  ,
`verifier_cooperator_id` int(11)    NULL  ,
`name_verified_date` datetime    NULL  ,
`name` varchar(100)    NULL  ,
`name_authority` varchar(100)    NULL  ,
`protologue` varchar(500)    NULL  ,
`note` text     NULL  ,
`site_note` text     NULL  ,
`alternate_name` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_species_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.taxonomy_use *************/
select concat(now(), ' creating table gringlobal.taxonomy_use ...') as Action;
CREATE TABLE `gringlobal`.`taxonomy_use` (
`taxonomy_use_id` int(11)    NOT NULL AUTO_INCREMENT ,
`taxonomy_species_id` int(11)    NOT NULL  ,
`economic_usage_code` varchar(20)    NOT NULL  ,
`usage_type_code` varchar(20)    NULL  ,
`plant_part_code` varchar(20)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`taxonomy_use_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_cooperator *************/
select concat(now(), ' creating table gringlobal.web_cooperator ...') as Action;
CREATE TABLE `gringlobal`.`web_cooperator` (
`web_cooperator_id` int(11)    NOT NULL AUTO_INCREMENT ,
`last_name` varchar(100)    NULL  ,
`title` varchar(10)    NULL  ,
`first_name` varchar(100)    NULL  ,
`job` varchar(100)    NULL  ,
`organization` varchar(100)    NULL  ,
`organization_code` varchar(10)    NULL  ,
`address_line1` varchar(100)    NULL  ,
`address_line2` varchar(100)    NULL  ,
`address_line3` varchar(100)    NULL  ,
`city` varchar(100)    NULL  ,
`postal_index` varchar(100)    NULL  ,
`geography_id` int(11)    NULL  ,
`primary_phone` varchar(30)    NULL  ,
`secondary_phone` varchar(30)    NULL  ,
`fax` varchar(30)    NULL  ,
`email` varchar(100)    NULL  ,
`is_active` char(1)     NOT NULL  ,
`category_code` varchar(4)    NULL  ,
`organization_region` varchar(20)    NULL  ,
`discipline` varchar(50)    NULL  ,
`initials` varchar(10)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_cooperator_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_order_request *************/
select concat(now(), ' creating table gringlobal.web_order_request ...') as Action;
CREATE TABLE `gringlobal`.`web_order_request` (
`web_order_request_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_cooperator_id` int(11)    NULL  ,
`ordered_date` datetime    NULL  ,
`intended_use_code` varchar(20)    NULL  ,
`intended_use_note` text     NULL  ,
`status_code` varchar(20)    NULL  ,
`note` text     NULL  ,
`special_instruction` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_order_request_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_order_request_action *************/
select concat(now(), ' creating table gringlobal.web_order_request_action ...') as Action;
CREATE TABLE `gringlobal`.`web_order_request_action` (
`web_order_request_action_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_order_request_id` int(11)    NOT NULL  ,
`action_code` varchar(20)    NOT NULL  ,
`acted_date` datetime    NOT NULL  ,
`action_for_id` varchar(40)    NULL  ,
`note` text     NULL  ,
`web_cooperator_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_order_request_action_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_order_request_address *************/
select concat(now(), ' creating table gringlobal.web_order_request_address ...') as Action;
CREATE TABLE `gringlobal`.`web_order_request_address` (
`web_order_request_address_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_order_request_id` int(11)    NOT NULL  ,
`address_line1` varchar(100)    NULL  ,
`address_line2` varchar(100)    NULL  ,
`address_line3` varchar(100)    NULL  ,
`city` varchar(100)    NULL  ,
`postal_index` varchar(100)    NULL  ,
`geography_id` int(11)    NULL  ,
`carrier` varchar(20)    NULL  ,
`carrier_account` varchar(50)    NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_order_request_address_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_order_request_attach *************/
select concat(now(), ' creating table gringlobal.web_order_request_attach ...') as Action;
CREATE TABLE `gringlobal`.`web_order_request_attach` (
`web_order_request_attach_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_cooperator_id` int(11)    NOT NULL  ,
`web_order_request_id` int(11)    NOT NULL  ,
`virtual_path` varchar(255)    NOT NULL  ,
`content_type` varchar(100)    NULL  ,
`title` varchar(500)    NULL  ,
`status` varchar(50)    NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_order_request_attach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_order_request_item *************/
select concat(now(), ' creating table gringlobal.web_order_request_item ...') as Action;
CREATE TABLE `gringlobal`.`web_order_request_item` (
`web_order_request_item_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_cooperator_id` int(11)    NOT NULL  ,
`web_order_request_id` int(11)    NOT NULL  ,
`sequence_number` int(11)    NOT NULL  ,
`accession_id` int(11)    NOT NULL  ,
`name` varchar(40)    NULL  ,
`quantity_shipped` int(11)    NULL  ,
`unit_of_shipped_code` varchar(20)    NULL  ,
`distribution_form_code` varchar(20)    NULL  ,
`status_code` varchar(20)    NULL  ,
`curator_note` text     NULL  ,
`user_note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_order_request_item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_user *************/
select concat(now(), ' creating table gringlobal.web_user ...') as Action;
CREATE TABLE `gringlobal`.`web_user` (
`web_user_id` int(11)    NOT NULL AUTO_INCREMENT ,
`user_name` varchar(50)    NOT NULL  ,
`password` varchar(255)    NOT NULL  ,
`is_enabled` char(1)     NOT NULL  ,
`sys_lang_id` int(11)    NOT NULL  ,
`last_login_date` datetime    NULL  ,
`web_cooperator_id` int(11)    NULL  ,
`created_date` datetime    NOT NULL  ,
`modified_date` datetime    NULL  ,
PRIMARY KEY (`web_user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_user_cart *************/
select concat(now(), ' creating table gringlobal.web_user_cart ...') as Action;
CREATE TABLE `gringlobal`.`web_user_cart` (
`web_user_cart_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_user_id` int(11)    NOT NULL  ,
`cart_type_code` varchar(20)    NOT NULL  ,
`expiration_date` datetime    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_user_cart_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_user_cart_item *************/
select concat(now(), ' creating table gringlobal.web_user_cart_item ...') as Action;
CREATE TABLE `gringlobal`.`web_user_cart_item` (
`web_user_cart_item_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_user_cart_id` int(11)    NOT NULL  ,
`accession_id` int(11)    NOT NULL  ,
`quantity` int(11)    NOT NULL  ,
`form_type_code` varchar(20)    NOT NULL  ,
`note` text     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_user_cart_item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_user_preference *************/
select concat(now(), ' creating table gringlobal.web_user_preference ...') as Action;
CREATE TABLE `gringlobal`.`web_user_preference` (
`web_user_preference_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_user_id` int(11)    NOT NULL  ,
`preference_name` varchar(100)    NOT NULL  ,
`preference_value` varchar(100)    NOT NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_user_preference_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;


/************ Table Definition for gringlobal.web_user_shipping_address *************/
select concat(now(), ' creating table gringlobal.web_user_shipping_address ...') as Action;
CREATE TABLE `gringlobal`.`web_user_shipping_address` (
`web_user_shipping_address_id` int(11)    NOT NULL AUTO_INCREMENT ,
`web_user_id` int(11)    NOT NULL  ,
`address_name` varchar(50)    NOT NULL  ,
`address_line1` varchar(100)    NOT NULL  ,
`address_line2` varchar(100)    NULL  ,
`address_line3` varchar(100)    NULL  ,
`city` varchar(100)    NOT NULL  ,
`postal_index` varchar(100)    NOT NULL  ,
`geography_id` int(11)    NOT NULL  ,
`is_default` char(1)     NULL  ,
`created_date` datetime    NOT NULL  ,
`created_by` int(11)    NOT NULL  ,
`modified_date` datetime    NULL  ,
`modified_by` int(11)    NULL  ,
`owned_date` datetime    NOT NULL  ,
`owned_by` int(11)    NOT NULL  ,
PRIMARY KEY (`web_user_shipping_address_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;



/*** Load data into table `gringlobal`.`app_resource` ***/
select concat(now(), ' Loading data into table `gringlobal`.`app_resource`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/app_resource.txt' INTO TABLE `gringlobal`.`app_resource`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`app_setting` ***/
select concat(now(), ' Loading data into table `gringlobal`.`app_setting`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/app_setting.txt' INTO TABLE `gringlobal`.`app_setting`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`code_value` ***/
select concat(now(), ' Loading data into table `gringlobal`.`code_value`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/code_value.txt' INTO TABLE `gringlobal`.`code_value`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`code_value_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`code_value_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/code_value_lang.txt' INTO TABLE `gringlobal`.`code_value_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`cooperator` ***/
select concat(now(), ' Loading data into table `gringlobal`.`cooperator`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/cooperator.txt' INTO TABLE `gringlobal`.`cooperator`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`cooperator_group` ***/
select concat(now(), ' Loading data into table `gringlobal`.`cooperator_group`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/cooperator_group.txt' INTO TABLE `gringlobal`.`cooperator_group`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`cooperator_map` ***/
select concat(now(), ' Loading data into table `gringlobal`.`cooperator_map`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/cooperator_map.txt' INTO TABLE `gringlobal`.`cooperator_map`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`geography` ***/
select concat(now(), ' Loading data into table `gringlobal`.`geography`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/geography.txt' INTO TABLE `gringlobal`.`geography`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`geography_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`geography_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/geography_lang.txt' INTO TABLE `gringlobal`.`geography_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`geography_region_map` ***/
select concat(now(), ' Loading data into table `gringlobal`.`geography_region_map`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/geography_region_map.txt' INTO TABLE `gringlobal`.`geography_region_map`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`region` ***/
select concat(now(), ' Loading data into table `gringlobal`.`region`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/region.txt' INTO TABLE `gringlobal`.`region`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`region_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`region_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/region_lang.txt' INTO TABLE `gringlobal`.`region_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`site` ***/
select concat(now(), ' Loading data into table `gringlobal`.`site`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/site.txt' INTO TABLE `gringlobal`.`site`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_database` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_database`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_database.txt' INTO TABLE `gringlobal`.`sys_database`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_database_migration` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_database_migration`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_database_migration.txt' INTO TABLE `gringlobal`.`sys_database_migration`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_database_migration_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_database_migration_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_database_migration_lang.txt' INTO TABLE `gringlobal`.`sys_database_migration_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_datatrigger` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_datatrigger`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_datatrigger.txt' INTO TABLE `gringlobal`.`sys_datatrigger`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_datatrigger_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_datatrigger_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_datatrigger_lang.txt' INTO TABLE `gringlobal`.`sys_datatrigger_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_dataview` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_dataview`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_dataview.txt' INTO TABLE `gringlobal`.`sys_dataview`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_dataview_field` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_dataview_field`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_dataview_field.txt' INTO TABLE `gringlobal`.`sys_dataview_field`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_dataview_field_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_dataview_field_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_dataview_field_lang.txt' INTO TABLE `gringlobal`.`sys_dataview_field_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_dataview_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_dataview_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_dataview_lang.txt' INTO TABLE `gringlobal`.`sys_dataview_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_dataview_param` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_dataview_param`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_dataview_param.txt' INTO TABLE `gringlobal`.`sys_dataview_param`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_dataview_sql` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_dataview_sql`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_dataview_sql.txt' INTO TABLE `gringlobal`.`sys_dataview_sql`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_file` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_file`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_file.txt' INTO TABLE `gringlobal`.`sys_file`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_file_group` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_file_group`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_file_group.txt' INTO TABLE `gringlobal`.`sys_file_group`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_file_group_map` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_file_group_map`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_file_group_map.txt' INTO TABLE `gringlobal`.`sys_file_group_map`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_file_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_file_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_file_lang.txt' INTO TABLE `gringlobal`.`sys_file_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_group` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_group`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_group.txt' INTO TABLE `gringlobal`.`sys_group`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_group_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_group_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_group_lang.txt' INTO TABLE `gringlobal`.`sys_group_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_group_permission_map` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_group_permission_map`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_group_permission_map.txt' INTO TABLE `gringlobal`.`sys_group_permission_map`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_group_user_map` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_group_user_map`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_group_user_map.txt' INTO TABLE `gringlobal`.`sys_group_user_map`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_index` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_index`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_index.txt' INTO TABLE `gringlobal`.`sys_index`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_index_field` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_index_field`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_index_field.txt' INTO TABLE `gringlobal`.`sys_index_field`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_lang.txt' INTO TABLE `gringlobal`.`sys_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_permission` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_permission`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_permission.txt' INTO TABLE `gringlobal`.`sys_permission`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_permission_field` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_permission_field`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_permission_field.txt' INTO TABLE `gringlobal`.`sys_permission_field`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_permission_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_permission_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_permission_lang.txt' INTO TABLE `gringlobal`.`sys_permission_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_table` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_table`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_table.txt' INTO TABLE `gringlobal`.`sys_table`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_table_field` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_table_field`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_table_field.txt' INTO TABLE `gringlobal`.`sys_table_field`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_table_field_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_table_field_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_table_field_lang.txt' INTO TABLE `gringlobal`.`sys_table_field_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_table_lang` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_table_lang`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_table_lang.txt' INTO TABLE `gringlobal`.`sys_table_lang`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_table_relationship` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_table_relationship`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_table_relationship.txt' INTO TABLE `gringlobal`.`sys_table_relationship`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_user` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_user`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_user.txt' INTO TABLE `gringlobal`.`sys_user`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`sys_user_permission_map` ***/
select concat(now(), ' Loading data into table `gringlobal`.`sys_user_permission_map`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/sys_user_permission_map.txt' INTO TABLE `gringlobal`.`sys_user_permission_map`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`web_cooperator` ***/
select concat(now(), ' Loading data into table `gringlobal`.`web_cooperator`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/web_cooperator.txt' INTO TABLE `gringlobal`.`web_cooperator`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`web_user` ***/
select concat(now(), ' Loading data into table `gringlobal`.`web_user`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/web_user.txt' INTO TABLE `gringlobal`.`web_user`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;

/*** Load data into table `gringlobal`.`web_user_preference` ***/
select concat(now(), ' Loading data into table `gringlobal`.`web_user_preference`') as Action;

LOAD DATA INFILE 'C:/projects/GrinGlobal_non-svn/raw_data_files/web_user_preference.txt' INTO TABLE `gringlobal`.`web_user_preference`
CHARACTER SET utf8
 FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'

IGNORE 1 LINES 
;
/***********************************************/
/************** Index Definitions **************/
/***********************************************/

/************ 5 Index Definitions for accession *************/
select concat(now(), ' creating index ndx_fk_a_created for table accession ...') as Action;
CREATE  INDEX `ndx_fk_a_created`  ON `gringlobal`.`accession` (`created_by`);

select concat(now(), ' creating index ndx_fk_a_modified for table accession ...') as Action;
CREATE  INDEX `ndx_fk_a_modified`  ON `gringlobal`.`accession` (`modified_by`);

select concat(now(), ' creating index ndx_fk_a_owned for table accession ...') as Action;
CREATE  INDEX `ndx_fk_a_owned`  ON `gringlobal`.`accession` (`owned_by`);

select concat(now(), ' creating index ndx_fk_a_t for table accession ...') as Action;
CREATE  INDEX `ndx_fk_a_t`  ON `gringlobal`.`accession` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_ac for table accession ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ac`  ON `gringlobal`.`accession` (`accession_number_part1`, `accession_number_part2`, `accession_number_part3`);

/************ 7 Index Definitions for accession_action *************/
select concat(now(), ' creating index ndx_fk_aa_a for table accession_action ...') as Action;
CREATE  INDEX `ndx_fk_aa_a`  ON `gringlobal`.`accession_action` (`accession_id`);

select concat(now(), ' creating index ndx_fk_aa_c for table accession_action ...') as Action;
CREATE  INDEX `ndx_fk_aa_c`  ON `gringlobal`.`accession_action` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_aa_created for table accession_action ...') as Action;
CREATE  INDEX `ndx_fk_aa_created`  ON `gringlobal`.`accession_action` (`created_by`);

select concat(now(), ' creating index ndx_fk_aa_m for table accession_action ...') as Action;
CREATE  INDEX `ndx_fk_aa_m`  ON `gringlobal`.`accession_action` (`method_id`);

select concat(now(), ' creating index ndx_fk_aa_modified for table accession_action ...') as Action;
CREATE  INDEX `ndx_fk_aa_modified`  ON `gringlobal`.`accession_action` (`modified_by`);

select concat(now(), ' creating index ndx_fk_aa_owned for table accession_action ...') as Action;
CREATE  INDEX `ndx_fk_aa_owned`  ON `gringlobal`.`accession_action` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_aa for table accession_action ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_aa`  ON `gringlobal`.`accession_action` (`accession_id`, `action_name_code`, `created_date`);

/************ 9 Index Definitions for accession_annotation *************/
select concat(now(), ' creating index ndx_fk_aan_c for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_c`  ON `gringlobal`.`accession_annotation` (`annotation_cooperator_id`);

select concat(now(), ' creating index ndx_fk_aan_created for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_created`  ON `gringlobal`.`accession_annotation` (`created_by`);

select concat(now(), ' creating index ndx_fk_aan_i for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_i`  ON `gringlobal`.`accession_annotation` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_aan_modified for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_modified`  ON `gringlobal`.`accession_annotation` (`modified_by`);

select concat(now(), ' creating index ndx_fk_aan_or for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_or`  ON `gringlobal`.`accession_annotation` (`order_request_id`);

select concat(now(), ' creating index ndx_fk_aan_owned for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_owned`  ON `gringlobal`.`accession_annotation` (`owned_by`);

select concat(now(), ' creating index ndx_fk_aan_t_new for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_t_new`  ON `gringlobal`.`accession_annotation` (`new_taxonomy_species_id`);

select concat(now(), ' creating index ndx_fk_aan_t_old for table accession_annotation ...') as Action;
CREATE  INDEX `ndx_fk_aan_t_old`  ON `gringlobal`.`accession_annotation` (`old_taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_al for table accession_annotation ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_al`  ON `gringlobal`.`accession_annotation` (`inventory_id`, `annotation_type_code`, `annotation_date`);

/************ 8 Index Definitions for accession_ipr *************/
select concat(now(), ' creating index ndx_fk_ar_a for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_fk_ar_a`  ON `gringlobal`.`accession_ipr` (`accession_id`);

select concat(now(), ' creating index ndx_fk_ar_c for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_fk_ar_c`  ON `gringlobal`.`accession_ipr` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_ar_created for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_fk_ar_created`  ON `gringlobal`.`accession_ipr` (`created_by`);

select concat(now(), ' creating index ndx_fk_ar_modified for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_fk_ar_modified`  ON `gringlobal`.`accession_ipr` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ar_owned for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_fk_ar_owned`  ON `gringlobal`.`accession_ipr` (`owned_by`);

select concat(now(), ' creating index ndx_ipr_crop for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_ipr_crop`  ON `gringlobal`.`accession_ipr` (`ipr_crop_name`);

select concat(now(), ' creating index ndx_ipr_number for table accession_ipr ...') as Action;
CREATE  INDEX `ndx_ipr_number`  ON `gringlobal`.`accession_ipr` (`ipr_number`);

select concat(now(), ' creating index ndx_uniq_ipr for table accession_ipr ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ipr`  ON `gringlobal`.`accession_ipr` (`accession_id`, `type_code`, `ipr_number`);

/************ 8 Index Definitions for accession_name *************/
select concat(now(), ' creating index ndx_an_name for table accession_name ...') as Action;
CREATE  INDEX `ndx_an_name`  ON `gringlobal`.`accession_name` (`plant_name`);

select concat(now(), ' creating index ndx_fk_an_a for table accession_name ...') as Action;
CREATE  INDEX `ndx_fk_an_a`  ON `gringlobal`.`accession_name` (`accession_id`);

select concat(now(), ' creating index ndx_fk_an_c for table accession_name ...') as Action;
CREATE  INDEX `ndx_fk_an_c`  ON `gringlobal`.`accession_name` (`name_source_cooperator_id`);

select concat(now(), ' creating index ndx_fk_an_created for table accession_name ...') as Action;
CREATE  INDEX `ndx_fk_an_created`  ON `gringlobal`.`accession_name` (`created_by`);

select concat(now(), ' creating index ndx_fk_an_modified for table accession_name ...') as Action;
CREATE  INDEX `ndx_fk_an_modified`  ON `gringlobal`.`accession_name` (`modified_by`);

select concat(now(), ' creating index ndx_fk_an_ng for table accession_name ...') as Action;
CREATE  INDEX `ndx_fk_an_ng`  ON `gringlobal`.`accession_name` (`name_group_id`);

select concat(now(), ' creating index ndx_fk_an_owned for table accession_name ...') as Action;
CREATE  INDEX `ndx_fk_an_owned`  ON `gringlobal`.`accession_name` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_an for table accession_name ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_an`  ON `gringlobal`.`accession_name` (`accession_id`, `plant_name`, `name_group_id`, `category_code`);

/************ 5 Index Definitions for accession_pedigree *************/
select concat(now(), ' creating index ndx_fk_ap_a for table accession_pedigree ...') as Action;
CREATE  INDEX `ndx_fk_ap_a`  ON `gringlobal`.`accession_pedigree` (`accession_id`);

select concat(now(), ' creating index ndx_fk_ap_created for table accession_pedigree ...') as Action;
CREATE  INDEX `ndx_fk_ap_created`  ON `gringlobal`.`accession_pedigree` (`created_by`);

select concat(now(), ' creating index ndx_fk_ap_modified for table accession_pedigree ...') as Action;
CREATE  INDEX `ndx_fk_ap_modified`  ON `gringlobal`.`accession_pedigree` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ap_owned for table accession_pedigree ...') as Action;
CREATE  INDEX `ndx_fk_ap_owned`  ON `gringlobal`.`accession_pedigree` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_pd for table accession_pedigree ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_pd`  ON `gringlobal`.`accession_pedigree` (`accession_id`);

/************ 6 Index Definitions for accession_quarantine *************/
select concat(now(), ' creating index ndx_fk_aq_a for table accession_quarantine ...') as Action;
CREATE  INDEX `ndx_fk_aq_a`  ON `gringlobal`.`accession_quarantine` (`accession_id`);

select concat(now(), ' creating index ndx_fk_aq_c for table accession_quarantine ...') as Action;
CREATE  INDEX `ndx_fk_aq_c`  ON `gringlobal`.`accession_quarantine` (`custodial_cooperator_id`);

select concat(now(), ' creating index ndx_fk_aq_created for table accession_quarantine ...') as Action;
CREATE  INDEX `ndx_fk_aq_created`  ON `gringlobal`.`accession_quarantine` (`created_by`);

select concat(now(), ' creating index ndx_fk_aq_modified for table accession_quarantine ...') as Action;
CREATE  INDEX `ndx_fk_aq_modified`  ON `gringlobal`.`accession_quarantine` (`modified_by`);

select concat(now(), ' creating index ndx_fk_aq_owned for table accession_quarantine ...') as Action;
CREATE  INDEX `ndx_fk_aq_owned`  ON `gringlobal`.`accession_quarantine` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_qr for table accession_quarantine ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_qr`  ON `gringlobal`.`accession_quarantine` (`accession_id`, `quarantine_type_code`);

/************ 1 Index Definitions for accession_source *************/
select concat(now(), ' creating index ndx_uniq_sr for table accession_source ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sr`  ON `gringlobal`.`accession_source` (`accession_id`, `source_type_code`, `source_date`);

/************ 6 Index Definitions for accession_source_map *************/
select concat(now(), ' creating index ndx_fk_asm_as for table accession_source_map ...') as Action;
CREATE  INDEX `ndx_fk_asm_as`  ON `gringlobal`.`accession_source_map` (`accession_source_id`);

select concat(now(), ' creating index ndx_fk_asm_c for table accession_source_map ...') as Action;
CREATE  INDEX `ndx_fk_asm_c`  ON `gringlobal`.`accession_source_map` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_asm_created for table accession_source_map ...') as Action;
CREATE  INDEX `ndx_fk_asm_created`  ON `gringlobal`.`accession_source_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_asm_modified for table accession_source_map ...') as Action;
CREATE  INDEX `ndx_fk_asm_modified`  ON `gringlobal`.`accession_source_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_asm_owned for table accession_source_map ...') as Action;
CREATE  INDEX `ndx_fk_asm_owned`  ON `gringlobal`.`accession_source_map` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_sm for table accession_source_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sm`  ON `gringlobal`.`accession_source_map` (`accession_source_id`, `cooperator_id`);

/************ 6 Index Definitions for accession_voucher *************/
select concat(now(), ' creating index ndx_fk_av_c for table accession_voucher ...') as Action;
CREATE  INDEX `ndx_fk_av_c`  ON `gringlobal`.`accession_voucher` (`collector_cooperator_id`);

select concat(now(), ' creating index ndx_fk_av_created for table accession_voucher ...') as Action;
CREATE  INDEX `ndx_fk_av_created`  ON `gringlobal`.`accession_voucher` (`created_by`);

select concat(now(), ' creating index ndx_fk_av_i for table accession_voucher ...') as Action;
CREATE  INDEX `ndx_fk_av_i`  ON `gringlobal`.`accession_voucher` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_av_modified for table accession_voucher ...') as Action;
CREATE  INDEX `ndx_fk_av_modified`  ON `gringlobal`.`accession_voucher` (`modified_by`);

select concat(now(), ' creating index ndx_fk_av_owned for table accession_voucher ...') as Action;
CREATE  INDEX `ndx_fk_av_owned`  ON `gringlobal`.`accession_voucher` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_vo for table accession_voucher ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_vo`  ON `gringlobal`.`accession_voucher` (`inventory_id`, `voucher_location`, `vouchered_date`);

/************ 4 Index Definitions for app_resource *************/
select concat(now(), ' creating index ndx_fk_are_created for table app_resource ...') as Action;
CREATE  INDEX `ndx_fk_are_created`  ON `gringlobal`.`app_resource` (`created_by`);

select concat(now(), ' creating index ndx_fk_are_modified for table app_resource ...') as Action;
CREATE  INDEX `ndx_fk_are_modified`  ON `gringlobal`.`app_resource` (`modified_by`);

select concat(now(), ' creating index ndx_fk_are_owned for table app_resource ...') as Action;
CREATE  INDEX `ndx_fk_are_owned`  ON `gringlobal`.`app_resource` (`owned_by`);

select concat(now(), ' creating index ndx_fk_are_sl for table app_resource ...') as Action;
CREATE  INDEX `ndx_fk_are_sl`  ON `gringlobal`.`app_resource` (`sys_lang_id`);

/************ No index definitions exist for app_setting *************/

select concat(now(), ' no index definitions exist for table app_setting') as Action;
/************ 5 Index Definitions for app_user_gui_setting *************/
select concat(now(), ' creating index ndx_fk_sugs_co for table app_user_gui_setting ...') as Action;
CREATE  INDEX `ndx_fk_sugs_co`  ON `gringlobal`.`app_user_gui_setting` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_sugs_created for table app_user_gui_setting ...') as Action;
CREATE  INDEX `ndx_fk_sugs_created`  ON `gringlobal`.`app_user_gui_setting` (`created_by`);

select concat(now(), ' creating index ndx_fk_sugs_modified for table app_user_gui_setting ...') as Action;
CREATE  INDEX `ndx_fk_sugs_modified`  ON `gringlobal`.`app_user_gui_setting` (`modified_by`);

select concat(now(), ' creating index ndx_fk_sugs_owned for table app_user_gui_setting ...') as Action;
CREATE  INDEX `ndx_fk_sugs_owned`  ON `gringlobal`.`app_user_gui_setting` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_sugs for table app_user_gui_setting ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sugs`  ON `gringlobal`.`app_user_gui_setting` (`cooperator_id`, `app_name`, `form_name`, `resource_name`, `resource_key`);

/************ 6 Index Definitions for app_user_item_list *************/
select concat(now(), ' creating index ndx_fk_auil_c for table app_user_item_list ...') as Action;
CREATE  INDEX `ndx_fk_auil_c`  ON `gringlobal`.`app_user_item_list` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_auil_created for table app_user_item_list ...') as Action;
CREATE  INDEX `ndx_fk_auil_created`  ON `gringlobal`.`app_user_item_list` (`created_by`);

select concat(now(), ' creating index ndx_fk_auil_modified for table app_user_item_list ...') as Action;
CREATE  INDEX `ndx_fk_auil_modified`  ON `gringlobal`.`app_user_item_list` (`modified_by`);

select concat(now(), ' creating index ndx_fk_auil_owned for table app_user_item_list ...') as Action;
CREATE  INDEX `ndx_fk_auil_owned`  ON `gringlobal`.`app_user_item_list` (`owned_by`);

select concat(now(), ' creating index ndx_uil_group for table app_user_item_list ...') as Action;
CREATE  INDEX `ndx_uil_group`  ON `gringlobal`.`app_user_item_list` (`cooperator_id`, `list_name`);

select concat(now(), ' creating index ndx_uil_tab for table app_user_item_list ...') as Action;
CREATE  INDEX `ndx_uil_tab`  ON `gringlobal`.`app_user_item_list` (`cooperator_id`, `tab_name`, `list_name`);

/************ 5 Index Definitions for citation *************/
select concat(now(), ' creating index ndx_fk_ci_created for table citation ...') as Action;
CREATE  INDEX `ndx_fk_ci_created`  ON `gringlobal`.`citation` (`created_by`);

select concat(now(), ' creating index ndx_fk_ci_l for table citation ...') as Action;
CREATE  INDEX `ndx_fk_ci_l`  ON `gringlobal`.`citation` (`literature_id`);

select concat(now(), ' creating index ndx_fk_ci_modified for table citation ...') as Action;
CREATE  INDEX `ndx_fk_ci_modified`  ON `gringlobal`.`citation` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ci_owned for table citation ...') as Action;
CREATE  INDEX `ndx_fk_ci_owned`  ON `gringlobal`.`citation` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_cit for table citation ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cit`  ON `gringlobal`.`citation` (`literature_id`, `citation_year`, `reference`);

/************ 15 Index Definitions for citation_map *************/
select concat(now(), ' creating index ndx_fk_cim_acc for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_acc`  ON `gringlobal`.`citation_map` (`accession_id`);

select concat(now(), ' creating index ndx_fk_cim_cit for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_cit`  ON `gringlobal`.`citation_map` (`citation_id`);

select concat(now(), ' creating index ndx_fk_cim_created for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_created`  ON `gringlobal`.`citation_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_cim_gm for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_gm`  ON `gringlobal`.`citation_map` (`genetic_marker_id`);

select concat(now(), ' creating index ndx_fk_cim_ip for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_ip`  ON `gringlobal`.`citation_map` (`accession_ipr_id`);

select concat(now(), ' creating index ndx_fk_cim_me for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_me`  ON `gringlobal`.`citation_map` (`method_id`);

select concat(now(), ' creating index ndx_fk_cim_modified for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_modified`  ON `gringlobal`.`citation_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cim_owned for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_owned`  ON `gringlobal`.`citation_map` (`owned_by`);

select concat(now(), ' creating index ndx_fk_cim_pe for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_pe`  ON `gringlobal`.`citation_map` (`accession_pedigree_id`);

select concat(now(), ' creating index ndx_fk_cim_ta for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_ta`  ON `gringlobal`.`citation_map` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_fk_cim_tc for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_tc`  ON `gringlobal`.`citation_map` (`taxonomy_common_name_id`);

select concat(now(), ' creating index ndx_fk_cim_tf for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_tf`  ON `gringlobal`.`citation_map` (`taxonomy_family_id`);

select concat(now(), ' creating index ndx_fk_cim_tg for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_tg`  ON `gringlobal`.`citation_map` (`taxonomy_genus_id`);

select concat(now(), ' creating index ndx_fk_cim_tu for table citation_map ...') as Action;
CREATE  INDEX `ndx_fk_cim_tu`  ON `gringlobal`.`citation_map` (`taxonomy_use_id`);

select concat(now(), ' creating index ndx_uniq_cim for table citation_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cim`  ON `gringlobal`.`citation_map` (`citation_id`, `accession_id`, `method_id`, `taxonomy_species_id`, `taxonomy_genus_id`, `taxonomy_family_id`, `accession_ipr_id`, `accession_pedigree_id`, `genetic_marker_id`, `taxonomy_common_name_id`, `taxonomy_use_id`);

/************ 5 Index Definitions for code_value *************/
select concat(now(), ' creating index ndx_fk_cdval_cdgrp for table code_value ...') as Action;
CREATE  INDEX `ndx_fk_cdval_cdgrp`  ON `gringlobal`.`code_value` (`group_name`);

select concat(now(), ' creating index ndx_fk_cdval_created for table code_value ...') as Action;
CREATE  INDEX `ndx_fk_cdval_created`  ON `gringlobal`.`code_value` (`created_by`);

select concat(now(), ' creating index ndx_fk_cdval_modified for table code_value ...') as Action;
CREATE  INDEX `ndx_fk_cdval_modified`  ON `gringlobal`.`code_value` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cdval_owned for table code_value ...') as Action;
CREATE  INDEX `ndx_fk_cdval_owned`  ON `gringlobal`.`code_value` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_cv for table code_value ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cv`  ON `gringlobal`.`code_value` (`group_name`, `value`);

/************ 6 Index Definitions for code_value_lang *************/
select concat(now(), ' creating index ndx_fk_cvl_created for table code_value_lang ...') as Action;
CREATE  INDEX `ndx_fk_cvl_created`  ON `gringlobal`.`code_value_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_cvl_cv for table code_value_lang ...') as Action;
CREATE  INDEX `ndx_fk_cvl_cv`  ON `gringlobal`.`code_value_lang` (`code_value_id`);

select concat(now(), ' creating index ndx_fk_cvl_modified for table code_value_lang ...') as Action;
CREATE  INDEX `ndx_fk_cvl_modified`  ON `gringlobal`.`code_value_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cvl_owned for table code_value_lang ...') as Action;
CREATE  INDEX `ndx_fk_cvl_owned`  ON `gringlobal`.`code_value_lang` (`owned_by`);

select concat(now(), ' creating index ndx_fk_cvl_sl for table code_value_lang ...') as Action;
CREATE  INDEX `ndx_fk_cvl_sl`  ON `gringlobal`.`code_value_lang` (`sys_lang_id`);

select concat(now(), ' creating index ndx_uniq_cvl for table code_value_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cvl`  ON `gringlobal`.`code_value_lang` (`code_value_id`, `sys_lang_id`);

/************ 8 Index Definitions for cooperator *************/
select concat(now(), ' creating index ndx_co_full_name for table cooperator ...') as Action;
CREATE  INDEX `ndx_co_full_name`  ON `gringlobal`.`cooperator` (`last_name`, `first_name`);

select concat(now(), ' creating index ndx_co_org_code for table cooperator ...') as Action;
CREATE  INDEX `ndx_co_org_code`  ON `gringlobal`.`cooperator` (`organization_abbrev`);

select concat(now(), ' creating index ndx_fk_c_created for table cooperator ...') as Action;
CREATE  INDEX `ndx_fk_c_created`  ON `gringlobal`.`cooperator` (`created_by`);

select concat(now(), ' creating index ndx_fk_c_cur_c for table cooperator ...') as Action;
CREATE  INDEX `ndx_fk_c_cur_c`  ON `gringlobal`.`cooperator` (`current_cooperator_id`);

select concat(now(), ' creating index ndx_fk_c_modified for table cooperator ...') as Action;
CREATE  INDEX `ndx_fk_c_modified`  ON `gringlobal`.`cooperator` (`modified_by`);

select concat(now(), ' creating index ndx_fk_c_owned for table cooperator ...') as Action;
CREATE  INDEX `ndx_fk_c_owned`  ON `gringlobal`.`cooperator` (`owned_by`);

select concat(now(), ' creating index ndx_fk_c_sl for table cooperator ...') as Action;
CREATE  INDEX `ndx_fk_c_sl`  ON `gringlobal`.`cooperator` (`sys_lang_id`);

select concat(now(), ' creating index ndx_uniq_co for table cooperator ...') as Action;
CREATE  INDEX `ndx_uniq_co`  ON `gringlobal`.`cooperator` (`last_name`, `first_name`, `organization`, `city`, `geography_id`, `primary_phone`);

/************ 4 Index Definitions for cooperator_group *************/
select concat(now(), ' creating index ndx_fk_cg_created for table cooperator_group ...') as Action;
CREATE  INDEX `ndx_fk_cg_created`  ON `gringlobal`.`cooperator_group` (`created_by`);

select concat(now(), ' creating index ndx_fk_cg_modified for table cooperator_group ...') as Action;
CREATE  INDEX `ndx_fk_cg_modified`  ON `gringlobal`.`cooperator_group` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cg_owned for table cooperator_group ...') as Action;
CREATE  INDEX `ndx_fk_cg_owned`  ON `gringlobal`.`cooperator_group` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_cg_name for table cooperator_group ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cg_name`  ON `gringlobal`.`cooperator_group` (`name`);

/************ 6 Index Definitions for cooperator_map *************/
select concat(now(), ' creating index ndx_fk_cm_c for table cooperator_map ...') as Action;
CREATE  INDEX `ndx_fk_cm_c`  ON `gringlobal`.`cooperator_map` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_cm_cg for table cooperator_map ...') as Action;
CREATE  INDEX `ndx_fk_cm_cg`  ON `gringlobal`.`cooperator_map` (`cooperator_group_id`);

select concat(now(), ' creating index ndx_fk_cm_created for table cooperator_map ...') as Action;
CREATE  INDEX `ndx_fk_cm_created`  ON `gringlobal`.`cooperator_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_cm_modified for table cooperator_map ...') as Action;
CREATE  INDEX `ndx_fk_cm_modified`  ON `gringlobal`.`cooperator_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cm_owned for table cooperator_map ...') as Action;
CREATE  INDEX `ndx_fk_cm_owned`  ON `gringlobal`.`cooperator_map` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_cm for table cooperator_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cm`  ON `gringlobal`.`cooperator_map` (`cooperator_id`, `cooperator_group_id`);

/************ 4 Index Definitions for crop *************/
select concat(now(), ' creating index ndx_fk_cr_created for table crop ...') as Action;
CREATE  INDEX `ndx_fk_cr_created`  ON `gringlobal`.`crop` (`created_by`);

select concat(now(), ' creating index ndx_fk_cr_modified for table crop ...') as Action;
CREATE  INDEX `ndx_fk_cr_modified`  ON `gringlobal`.`crop` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cr_owned for table crop ...') as Action;
CREATE  INDEX `ndx_fk_cr_owned`  ON `gringlobal`.`crop` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_crop for table crop ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_crop`  ON `gringlobal`.`crop` (`name`);

/************ 5 Index Definitions for crop_attach *************/
select concat(now(), ' creating index ndx_fk_c for table crop_attach ...') as Action;
CREATE  INDEX `ndx_fk_c`  ON `gringlobal`.`crop_attach` (`crop_id`);

select concat(now(), ' creating index ndx_fk_created for table crop_attach ...') as Action;
CREATE  INDEX `ndx_fk_created`  ON `gringlobal`.`crop_attach` (`created_by`);

select concat(now(), ' creating index ndx_fk_modified for table crop_attach ...') as Action;
CREATE  INDEX `ndx_fk_modified`  ON `gringlobal`.`crop_attach` (`modified_by`);

select concat(now(), ' creating index ndx_fk_owned for table crop_attach ...') as Action;
CREATE  INDEX `ndx_fk_owned`  ON `gringlobal`.`crop_attach` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ca for table crop_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ca`  ON `gringlobal`.`crop_attach` (`crop_id`, `virtual_path`);

/************ 5 Index Definitions for crop_trait *************/
select concat(now(), ' creating index ndx_fk_ct_cr for table crop_trait ...') as Action;
CREATE  INDEX `ndx_fk_ct_cr`  ON `gringlobal`.`crop_trait` (`crop_id`);

select concat(now(), ' creating index ndx_fk_ct_created for table crop_trait ...') as Action;
CREATE  INDEX `ndx_fk_ct_created`  ON `gringlobal`.`crop_trait` (`created_by`);

select concat(now(), ' creating index ndx_fk_ct_modified for table crop_trait ...') as Action;
CREATE  INDEX `ndx_fk_ct_modified`  ON `gringlobal`.`crop_trait` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ct_owned for table crop_trait ...') as Action;
CREATE  INDEX `ndx_fk_ct_owned`  ON `gringlobal`.`crop_trait` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ct for table crop_trait ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ct`  ON `gringlobal`.`crop_trait` (`coded_name`, `crop_id`);

/************ 5 Index Definitions for crop_trait_attach *************/
select concat(now(), ' creating index ndx_fk_cta_created for table crop_trait_attach ...') as Action;
CREATE  INDEX `ndx_fk_cta_created`  ON `gringlobal`.`crop_trait_attach` (`created_by`);

select concat(now(), ' creating index ndx_fk_cta_ct for table crop_trait_attach ...') as Action;
CREATE  INDEX `ndx_fk_cta_ct`  ON `gringlobal`.`crop_trait_attach` (`crop_trait_id`);

select concat(now(), ' creating index ndx_fk_cta_modified for table crop_trait_attach ...') as Action;
CREATE  INDEX `ndx_fk_cta_modified`  ON `gringlobal`.`crop_trait_attach` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cta_owned for table crop_trait_attach ...') as Action;
CREATE  INDEX `ndx_fk_cta_owned`  ON `gringlobal`.`crop_trait_attach` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_cta for table crop_trait_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_cta`  ON `gringlobal`.`crop_trait_attach` (`crop_trait_id`, `virtual_path`);

/************ 5 Index Definitions for crop_trait_code *************/
select concat(now(), ' creating index ndx_fk_tct_created for table crop_trait_code ...') as Action;
CREATE  INDEX `ndx_fk_tct_created`  ON `gringlobal`.`crop_trait_code` (`created_by`);

select concat(now(), ' creating index ndx_fk_tct_modified for table crop_trait_code ...') as Action;
CREATE  INDEX `ndx_fk_tct_modified`  ON `gringlobal`.`crop_trait_code` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tct_owned for table crop_trait_code ...') as Action;
CREATE  INDEX `ndx_fk_tct_owned`  ON `gringlobal`.`crop_trait_code` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tct_tr for table crop_trait_code ...') as Action;
CREATE  INDEX `ndx_fk_tct_tr`  ON `gringlobal`.`crop_trait_code` (`crop_trait_id`);

select concat(now(), ' creating index ndx_uniq_ctc for table crop_trait_code ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ctc`  ON `gringlobal`.`crop_trait_code` (`crop_trait_id`, `code`);

/************ 5 Index Definitions for crop_trait_code_attach *************/
select concat(now(), ' creating index ndx_fk_ctca_created for table crop_trait_code_attach ...') as Action;
CREATE  INDEX `ndx_fk_ctca_created`  ON `gringlobal`.`crop_trait_code_attach` (`created_by`);

select concat(now(), ' creating index ndx_fk_ctca_ctc for table crop_trait_code_attach ...') as Action;
CREATE  INDEX `ndx_fk_ctca_ctc`  ON `gringlobal`.`crop_trait_code_attach` (`crop_trait_code_id`);

select concat(now(), ' creating index ndx_fk_ctca_modified for table crop_trait_code_attach ...') as Action;
CREATE  INDEX `ndx_fk_ctca_modified`  ON `gringlobal`.`crop_trait_code_attach` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ctca_owned for table crop_trait_code_attach ...') as Action;
CREATE  INDEX `ndx_fk_ctca_owned`  ON `gringlobal`.`crop_trait_code_attach` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ctca for table crop_trait_code_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ctca`  ON `gringlobal`.`crop_trait_code_attach` (`crop_trait_code_id`, `virtual_path`);

/************ 6 Index Definitions for crop_trait_code_lang *************/
select concat(now(), ' creating index ndx_fk_ctcl_created for table crop_trait_code_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctcl_created`  ON `gringlobal`.`crop_trait_code_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_ctcl_modified for table crop_trait_code_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctcl_modified`  ON `gringlobal`.`crop_trait_code_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ctcl_owned for table crop_trait_code_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctcl_owned`  ON `gringlobal`.`crop_trait_code_lang` (`owned_by`);

select concat(now(), ' creating index ndx_fk_ctcl_sl for table crop_trait_code_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctcl_sl`  ON `gringlobal`.`crop_trait_code_lang` (`sys_lang_id`);

select concat(now(), ' creating index ndx_fk_ctcl_tc for table crop_trait_code_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctcl_tc`  ON `gringlobal`.`crop_trait_code_lang` (`crop_trait_code_id`);

select concat(now(), ' creating index ndx_uniq_ctcl for table crop_trait_code_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ctcl`  ON `gringlobal`.`crop_trait_code_lang` (`crop_trait_code_id`, `sys_lang_id`);

/************ 6 Index Definitions for crop_trait_lang *************/
select concat(now(), ' creating index ndx_fk_ctl_created for table crop_trait_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctl_created`  ON `gringlobal`.`crop_trait_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_ctl_modified for table crop_trait_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctl_modified`  ON `gringlobal`.`crop_trait_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ctl_owned for table crop_trait_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctl_owned`  ON `gringlobal`.`crop_trait_lang` (`owned_by`);

select concat(now(), ' creating index ndx_fk_ctl_sl for table crop_trait_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctl_sl`  ON `gringlobal`.`crop_trait_lang` (`sys_lang_id`);

select concat(now(), ' creating index ndx_fk_ctl_t for table crop_trait_lang ...') as Action;
CREATE  INDEX `ndx_fk_ctl_t`  ON `gringlobal`.`crop_trait_lang` (`crop_trait_id`);

select concat(now(), ' creating index ndx_uniq_ctl for table crop_trait_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ctl`  ON `gringlobal`.`crop_trait_lang` (`crop_trait_id`, `sys_lang_id`);

/************ 7 Index Definitions for crop_trait_observation *************/
select concat(now(), ' creating index ndx_fk_cto_created for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_created`  ON `gringlobal`.`crop_trait_observation` (`created_by`);

select concat(now(), ' creating index ndx_fk_cto_ct for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_ct`  ON `gringlobal`.`crop_trait_observation` (`crop_trait_id`);

select concat(now(), ' creating index ndx_fk_cto_ctc for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_ctc`  ON `gringlobal`.`crop_trait_observation` (`crop_trait_code_id`);

select concat(now(), ' creating index ndx_fk_cto_i for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_i`  ON `gringlobal`.`crop_trait_observation` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_cto_m for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_m`  ON `gringlobal`.`crop_trait_observation` (`method_id`);

select concat(now(), ' creating index ndx_fk_cto_modified for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_modified`  ON `gringlobal`.`crop_trait_observation` (`modified_by`);

select concat(now(), ' creating index ndx_fk_cto_owned for table crop_trait_observation ...') as Action;
CREATE  INDEX `ndx_fk_cto_owned`  ON `gringlobal`.`crop_trait_observation` (`owned_by`);

/************ No index definitions exist for crop_trait_observation_data *************/

select concat(now(), ' no index definitions exist for table crop_trait_observation_data') as Action;
/************ 6 Index Definitions for genetic_annotation *************/
select concat(now(), ' creating index ndx_fk_ga_created for table genetic_annotation ...') as Action;
CREATE  INDEX `ndx_fk_ga_created`  ON `gringlobal`.`genetic_annotation` (`created_by`);

select concat(now(), ' creating index ndx_fk_ga_gm for table genetic_annotation ...') as Action;
CREATE  INDEX `ndx_fk_ga_gm`  ON `gringlobal`.`genetic_annotation` (`genetic_marker_id`);

select concat(now(), ' creating index ndx_fk_ga_m for table genetic_annotation ...') as Action;
CREATE  INDEX `ndx_fk_ga_m`  ON `gringlobal`.`genetic_annotation` (`method_id`);

select concat(now(), ' creating index ndx_fk_ga_modified for table genetic_annotation ...') as Action;
CREATE  INDEX `ndx_fk_ga_modified`  ON `gringlobal`.`genetic_annotation` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ga_owned for table genetic_annotation ...') as Action;
CREATE  INDEX `ndx_fk_ga_owned`  ON `gringlobal`.`genetic_annotation` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ga for table genetic_annotation ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ga`  ON `gringlobal`.`genetic_annotation` (`genetic_marker_id`, `method_id`);

/************ 5 Index Definitions for genetic_marker *************/
select concat(now(), ' creating index ndx_fk_gm_cr for table genetic_marker ...') as Action;
CREATE  INDEX `ndx_fk_gm_cr`  ON `gringlobal`.`genetic_marker` (`crop_id`);

select concat(now(), ' creating index ndx_fk_gm_created for table genetic_marker ...') as Action;
CREATE  INDEX `ndx_fk_gm_created`  ON `gringlobal`.`genetic_marker` (`created_by`);

select concat(now(), ' creating index ndx_fk_gm_modified for table genetic_marker ...') as Action;
CREATE  INDEX `ndx_fk_gm_modified`  ON `gringlobal`.`genetic_marker` (`modified_by`);

select concat(now(), ' creating index ndx_fk_gm_owned for table genetic_marker ...') as Action;
CREATE  INDEX `ndx_fk_gm_owned`  ON `gringlobal`.`genetic_marker` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_gm_crop for table genetic_marker ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_gm_crop`  ON `gringlobal`.`genetic_marker` (`crop_id`, `name`);

/************ 6 Index Definitions for genetic_observation *************/
select concat(now(), ' creating index ndx_fk_go_created for table genetic_observation ...') as Action;
CREATE  INDEX `ndx_fk_go_created`  ON `gringlobal`.`genetic_observation` (`created_by`);

select concat(now(), ' creating index ndx_fk_go_ga for table genetic_observation ...') as Action;
CREATE  INDEX `ndx_fk_go_ga`  ON `gringlobal`.`genetic_observation` (`genetic_annotation_id`);

select concat(now(), ' creating index ndx_fk_go_i for table genetic_observation ...') as Action;
CREATE  INDEX `ndx_fk_go_i`  ON `gringlobal`.`genetic_observation` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_go_modified for table genetic_observation ...') as Action;
CREATE  INDEX `ndx_fk_go_modified`  ON `gringlobal`.`genetic_observation` (`modified_by`);

select concat(now(), ' creating index ndx_fk_go_owned for table genetic_observation ...') as Action;
CREATE  INDEX `ndx_fk_go_owned`  ON `gringlobal`.`genetic_observation` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_go for table genetic_observation ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_go`  ON `gringlobal`.`genetic_observation` (`inventory_id`);

/************ 7 Index Definitions for genetic_observation_data *************/
select concat(now(), ' creating index ndx_fk_god_created for table genetic_observation_data ...') as Action;
CREATE  INDEX `ndx_fk_god_created`  ON `gringlobal`.`genetic_observation_data` (`created_by`);

select concat(now(), ' creating index ndx_fk_god_ga for table genetic_observation_data ...') as Action;
CREATE  INDEX `ndx_fk_god_ga`  ON `gringlobal`.`genetic_observation_data` (`genetic_annotation_id`);

select concat(now(), ' creating index ndx_fk_god_i for table genetic_observation_data ...') as Action;
CREATE  INDEX `ndx_fk_god_i`  ON `gringlobal`.`genetic_observation_data` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_god_modified for table genetic_observation_data ...') as Action;
CREATE  INDEX `ndx_fk_god_modified`  ON `gringlobal`.`genetic_observation_data` (`modified_by`);

select concat(now(), ' creating index ndx_fk_god_ob for table genetic_observation_data ...') as Action;
CREATE  INDEX `ndx_fk_god_ob`  ON `gringlobal`.`genetic_observation_data` (`genetic_observation_id`);

select concat(now(), ' creating index ndx_fk_god_owned for table genetic_observation_data ...') as Action;
CREATE  INDEX `ndx_fk_god_owned`  ON `gringlobal`.`genetic_observation_data` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_god for table genetic_observation_data ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_god`  ON `gringlobal`.`genetic_observation_data` (`genetic_annotation_id`, `inventory_id`, `individual`, `individual_allele_number`);

/************ 5 Index Definitions for geography *************/
select concat(now(), ' creating index ndx_fk_g_created for table geography ...') as Action;
CREATE  INDEX `ndx_fk_g_created`  ON `gringlobal`.`geography` (`created_by`);

select concat(now(), ' creating index ndx_fk_g_cur_g for table geography ...') as Action;
CREATE  INDEX `ndx_fk_g_cur_g`  ON `gringlobal`.`geography` (`current_geography_id`);

select concat(now(), ' creating index ndx_fk_g_modified for table geography ...') as Action;
CREATE  INDEX `ndx_fk_g_modified`  ON `gringlobal`.`geography` (`modified_by`);

select concat(now(), ' creating index ndx_fk_g_owned for table geography ...') as Action;
CREATE  INDEX `ndx_fk_g_owned`  ON `gringlobal`.`geography` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_geo for table geography ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_geo`  ON `gringlobal`.`geography` (`country_code`, `adm1`, `adm1_type_code`, `adm2`, `adm2_type_code`, `adm3`, `adm3_type_code`, `adm4`, `adm4_type_code`);

/************ 6 Index Definitions for geography_lang *************/
select concat(now(), ' creating index ndx_fk_gl_created for table geography_lang ...') as Action;
CREATE  INDEX `ndx_fk_gl_created`  ON `gringlobal`.`geography_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_gl_g for table geography_lang ...') as Action;
CREATE  INDEX `ndx_fk_gl_g`  ON `gringlobal`.`geography_lang` (`geography_id`);

select concat(now(), ' creating index ndx_fk_gl_modified for table geography_lang ...') as Action;
CREATE  INDEX `ndx_fk_gl_modified`  ON `gringlobal`.`geography_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_gl_owned for table geography_lang ...') as Action;
CREATE  INDEX `ndx_fk_gl_owned`  ON `gringlobal`.`geography_lang` (`owned_by`);

select concat(now(), ' creating index ndx_fk_gl_sl for table geography_lang ...') as Action;
CREATE  INDEX `ndx_fk_gl_sl`  ON `gringlobal`.`geography_lang` (`sys_lang_id`);

select concat(now(), ' creating index ndx_uniq_gl for table geography_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_gl`  ON `gringlobal`.`geography_lang` (`geography_id`, `sys_lang_id`);

/************ 6 Index Definitions for geography_region_map *************/
select concat(now(), ' creating index ndx_fk_grm_created for table geography_region_map ...') as Action;
CREATE  INDEX `ndx_fk_grm_created`  ON `gringlobal`.`geography_region_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_grm_g for table geography_region_map ...') as Action;
CREATE  INDEX `ndx_fk_grm_g`  ON `gringlobal`.`geography_region_map` (`geography_id`);

select concat(now(), ' creating index ndx_fk_grm_modified for table geography_region_map ...') as Action;
CREATE  INDEX `ndx_fk_grm_modified`  ON `gringlobal`.`geography_region_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_grm_owned for table geography_region_map ...') as Action;
CREATE  INDEX `ndx_fk_grm_owned`  ON `gringlobal`.`geography_region_map` (`owned_by`);

select concat(now(), ' creating index ndx_fk_grm_r for table geography_region_map ...') as Action;
CREATE  INDEX `ndx_fk_grm_r`  ON `gringlobal`.`geography_region_map` (`region_id`);

select concat(now(), ' creating index ndx_uniq_grm for table geography_region_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_grm`  ON `gringlobal`.`geography_region_map` (`geography_id`, `region_id`);

/************ 10 Index Definitions for inventory *************/
select concat(now(), ' creating index ndx_fk_i_a for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_a`  ON `gringlobal`.`inventory` (`accession_id`);

select concat(now(), ' creating index ndx_fk_i_backup_i for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_backup_i`  ON `gringlobal`.`inventory` (`backup_inventory_id`);

select concat(now(), ' creating index ndx_fk_i_created for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_created`  ON `gringlobal`.`inventory` (`created_by`);

select concat(now(), ' creating index ndx_fk_i_im for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_im`  ON `gringlobal`.`inventory` (`inventory_maint_policy_id`);

select concat(now(), ' creating index ndx_fk_i_modified for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_modified`  ON `gringlobal`.`inventory` (`modified_by`);

select concat(now(), ' creating index ndx_fk_i_owned for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_owned`  ON `gringlobal`.`inventory` (`owned_by`);

select concat(now(), ' creating index ndx_fk_i_parent_i for table inventory ...') as Action;
CREATE  INDEX `ndx_fk_i_parent_i`  ON `gringlobal`.`inventory` (`parent_inventory_id`);

select concat(now(), ' creating index ndx_inv_number for table inventory ...') as Action;
CREATE  INDEX `ndx_inv_number`  ON `gringlobal`.`inventory` (`inventory_number_part2`);

select concat(now(), ' creating index ndx_inv_prefix for table inventory ...') as Action;
CREATE  INDEX `ndx_inv_prefix`  ON `gringlobal`.`inventory` (`inventory_number_part1`);

select concat(now(), ' creating index ndx_uniq_inv for table inventory ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_inv`  ON `gringlobal`.`inventory` (`inventory_number_part1`, `inventory_number_part2`, `inventory_number_part3`, `form_type_code`);

/************ 7 Index Definitions for inventory_action *************/
select concat(now(), ' creating index ndx_fk_ia_c for table inventory_action ...') as Action;
CREATE  INDEX `ndx_fk_ia_c`  ON `gringlobal`.`inventory_action` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_ia_created for table inventory_action ...') as Action;
CREATE  INDEX `ndx_fk_ia_created`  ON `gringlobal`.`inventory_action` (`created_by`);

select concat(now(), ' creating index ndx_fk_ia_i for table inventory_action ...') as Action;
CREATE  INDEX `ndx_fk_ia_i`  ON `gringlobal`.`inventory_action` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_ia_m for table inventory_action ...') as Action;
CREATE  INDEX `ndx_fk_ia_m`  ON `gringlobal`.`inventory_action` (`method_id`);

select concat(now(), ' creating index ndx_fk_ia_modified for table inventory_action ...') as Action;
CREATE  INDEX `ndx_fk_ia_modified`  ON `gringlobal`.`inventory_action` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ia_owned for table inventory_action ...') as Action;
CREATE  INDEX `ndx_fk_ia_owned`  ON `gringlobal`.`inventory_action` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ia for table inventory_action ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ia`  ON `gringlobal`.`inventory_action` (`inventory_id`, `action_name_code`, `action_date`, `action_date_code`);

/************ 5 Index Definitions for inventory_attach *************/
select concat(now(), ' creating index ndx_fk_iat_created for table inventory_attach ...') as Action;
CREATE  INDEX `ndx_fk_iat_created`  ON `gringlobal`.`inventory_attach` (`created_by`);

select concat(now(), ' creating index ndx_fk_iat_iid for table inventory_attach ...') as Action;
CREATE  INDEX `ndx_fk_iat_iid`  ON `gringlobal`.`inventory_attach` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_iat_modified for table inventory_attach ...') as Action;
CREATE  INDEX `ndx_fk_iat_modified`  ON `gringlobal`.`inventory_attach` (`modified_by`);

select concat(now(), ' creating index ndx_fk_iat_owned for table inventory_attach ...') as Action;
CREATE  INDEX `ndx_fk_iat_owned`  ON `gringlobal`.`inventory_attach` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_iat for table inventory_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_iat`  ON `gringlobal`.`inventory_attach` (`inventory_id`, `virtual_path`);

/************ 4 Index Definitions for inventory_group *************/
select concat(now(), ' creating index ndx_fk_ig_created for table inventory_group ...') as Action;
CREATE  INDEX `ndx_fk_ig_created`  ON `gringlobal`.`inventory_group` (`created_by`);

select concat(now(), ' creating index ndx_fk_ig_modified for table inventory_group ...') as Action;
CREATE  INDEX `ndx_fk_ig_modified`  ON `gringlobal`.`inventory_group` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ig_owned for table inventory_group ...') as Action;
CREATE  INDEX `ndx_fk_ig_owned`  ON `gringlobal`.`inventory_group` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ig for table inventory_group ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ig`  ON `gringlobal`.`inventory_group` (`group_name`);

/************ 6 Index Definitions for inventory_group_map *************/
select concat(now(), ' creating index ndx_fk_igm_created for table inventory_group_map ...') as Action;
CREATE  INDEX `ndx_fk_igm_created`  ON `gringlobal`.`inventory_group_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_igm_i for table inventory_group_map ...') as Action;
CREATE  INDEX `ndx_fk_igm_i`  ON `gringlobal`.`inventory_group_map` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_igm_ig for table inventory_group_map ...') as Action;
CREATE  INDEX `ndx_fk_igm_ig`  ON `gringlobal`.`inventory_group_map` (`inventory_group_id`);

select concat(now(), ' creating index ndx_fk_igm_modified for table inventory_group_map ...') as Action;
CREATE  INDEX `ndx_fk_igm_modified`  ON `gringlobal`.`inventory_group_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_igm_owned for table inventory_group_map ...') as Action;
CREATE  INDEX `ndx_fk_igm_owned`  ON `gringlobal`.`inventory_group_map` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_igm for table inventory_group_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_igm`  ON `gringlobal`.`inventory_group_map` (`inventory_id`, `inventory_group_id`);

/************ 5 Index Definitions for inventory_maint_policy *************/
select concat(now(), ' creating index ndx_fk_im_co for table inventory_maint_policy ...') as Action;
CREATE  INDEX `ndx_fk_im_co`  ON `gringlobal`.`inventory_maint_policy` (`curator_cooperator_id`);

select concat(now(), ' creating index ndx_fk_im_created for table inventory_maint_policy ...') as Action;
CREATE  INDEX `ndx_fk_im_created`  ON `gringlobal`.`inventory_maint_policy` (`created_by`);

select concat(now(), ' creating index ndx_fk_im_modified for table inventory_maint_policy ...') as Action;
CREATE  INDEX `ndx_fk_im_modified`  ON `gringlobal`.`inventory_maint_policy` (`modified_by`);

select concat(now(), ' creating index ndx_fk_im_owned for table inventory_maint_policy ...') as Action;
CREATE  INDEX `ndx_fk_im_owned`  ON `gringlobal`.`inventory_maint_policy` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_im for table inventory_maint_policy ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_im`  ON `gringlobal`.`inventory_maint_policy` (`maintenance_name`);

/************ 7 Index Definitions for inventory_name *************/
select concat(now(), ' creating index ndx_fk_in_c for table inventory_name ...') as Action;
CREATE  INDEX `ndx_fk_in_c`  ON `gringlobal`.`inventory_name` (`name_source_cooperator_id`);

select concat(now(), ' creating index ndx_fk_in_created for table inventory_name ...') as Action;
CREATE  INDEX `ndx_fk_in_created`  ON `gringlobal`.`inventory_name` (`created_by`);

select concat(now(), ' creating index ndx_fk_in_i for table inventory_name ...') as Action;
CREATE  INDEX `ndx_fk_in_i`  ON `gringlobal`.`inventory_name` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_in_modified for table inventory_name ...') as Action;
CREATE  INDEX `ndx_fk_in_modified`  ON `gringlobal`.`inventory_name` (`modified_by`);

select concat(now(), ' creating index ndx_fk_in_ng for table inventory_name ...') as Action;
CREATE  INDEX `ndx_fk_in_ng`  ON `gringlobal`.`inventory_name` (`name_group_id`);

select concat(now(), ' creating index ndx_fk_in_owned for table inventory_name ...') as Action;
CREATE  INDEX `ndx_fk_in_owned`  ON `gringlobal`.`inventory_name` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_in for table inventory_name ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_in`  ON `gringlobal`.`inventory_name` (`inventory_id`, `plant_name`, `name_group_id`, `category_code`);

/************ 8 Index Definitions for inventory_quality_status *************/
select concat(now(), ' creating index ndx_fk_iqs_created for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_fk_iqs_created`  ON `gringlobal`.`inventory_quality_status` (`created_by`);

select concat(now(), ' creating index ndx_fk_iqs_cur for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_fk_iqs_cur`  ON `gringlobal`.`inventory_quality_status` (`tester_cooperator_id`);

select concat(now(), ' creating index ndx_fk_iqs_i for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_fk_iqs_i`  ON `gringlobal`.`inventory_quality_status` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_iqs_me for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_fk_iqs_me`  ON `gringlobal`.`inventory_quality_status` (`method_id`);

select concat(now(), ' creating index ndx_fk_iqs_modified for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_fk_iqs_modified`  ON `gringlobal`.`inventory_quality_status` (`modified_by`);

select concat(now(), ' creating index ndx_fk_iqs_owned for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_fk_iqs_owned`  ON `gringlobal`.`inventory_quality_status` (`owned_by`);

select concat(now(), ' creating index ndx_iqs_test for table inventory_quality_status ...') as Action;
CREATE  INDEX `ndx_iqs_test`  ON `gringlobal`.`inventory_quality_status` (`test_type_code`, `contaminant_code`);

select concat(now(), ' creating index ndx_uniq_iqs for table inventory_quality_status ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_iqs`  ON `gringlobal`.`inventory_quality_status` (`inventory_id`, `test_type_code`, `contaminant_code`, `started_date`);

/************ 6 Index Definitions for inventory_viability *************/
select concat(now(), ' creating index ndx__uniq_iv for table inventory_viability ...') as Action;
CREATE UNIQUE INDEX `ndx__uniq_iv`  ON `gringlobal`.`inventory_viability` (`inventory_id`, `inventory_viability_rule_id`, `tested_date`, `tested_date_code`);

select concat(now(), ' creating index ndx_fk_iv_created for table inventory_viability ...') as Action;
CREATE  INDEX `ndx_fk_iv_created`  ON `gringlobal`.`inventory_viability` (`created_by`);

select concat(now(), ' creating index ndx_fk_iv_i for table inventory_viability ...') as Action;
CREATE  INDEX `ndx_fk_iv_i`  ON `gringlobal`.`inventory_viability` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_iv_ivr for table inventory_viability ...') as Action;
CREATE  INDEX `ndx_fk_iv_ivr`  ON `gringlobal`.`inventory_viability` (`inventory_viability_rule_id`);

select concat(now(), ' creating index ndx_fk_iv_modified for table inventory_viability ...') as Action;
CREATE  INDEX `ndx_fk_iv_modified`  ON `gringlobal`.`inventory_viability` (`modified_by`);

select concat(now(), ' creating index ndx_fk_iv_owned for table inventory_viability ...') as Action;
CREATE  INDEX `ndx_fk_iv_owned`  ON `gringlobal`.`inventory_viability` (`owned_by`);

/************ 7 Index Definitions for inventory_viability_data *************/
select concat(now(), ' creating index ndx_fk_ivd_c for table inventory_viability_data ...') as Action;
CREATE  INDEX `ndx_fk_ivd_c`  ON `gringlobal`.`inventory_viability_data` (`counter_cooperator_id`);

select concat(now(), ' creating index ndx_fk_ivd_created for table inventory_viability_data ...') as Action;
CREATE  INDEX `ndx_fk_ivd_created`  ON `gringlobal`.`inventory_viability_data` (`created_by`);

select concat(now(), ' creating index ndx_fk_ivd_iv for table inventory_viability_data ...') as Action;
CREATE  INDEX `ndx_fk_ivd_iv`  ON `gringlobal`.`inventory_viability_data` (`inventory_viability_id`);

select concat(now(), ' creating index ndx_fk_ivd_modified for table inventory_viability_data ...') as Action;
CREATE  INDEX `ndx_fk_ivd_modified`  ON `gringlobal`.`inventory_viability_data` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ivd_ori for table inventory_viability_data ...') as Action;
CREATE  INDEX `ndx_fk_ivd_ori`  ON `gringlobal`.`inventory_viability_data` (`order_request_item_id`);

select concat(now(), ' creating index ndx_fk_ivd_owned for table inventory_viability_data ...') as Action;
CREATE  INDEX `ndx_fk_ivd_owned`  ON `gringlobal`.`inventory_viability_data` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ivd for table inventory_viability_data ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ivd`  ON `gringlobal`.`inventory_viability_data` (`inventory_viability_id`, `replication_number`, `count_number`);

/************ 5 Index Definitions for inventory_viability_rule *************/
select concat(now(), ' creating index ndx_fk_ivr_created for table inventory_viability_rule ...') as Action;
CREATE  INDEX `ndx_fk_ivr_created`  ON `gringlobal`.`inventory_viability_rule` (`created_by`);

select concat(now(), ' creating index ndx_fk_ivr_modified for table inventory_viability_rule ...') as Action;
CREATE  INDEX `ndx_fk_ivr_modified`  ON `gringlobal`.`inventory_viability_rule` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ivr_owned for table inventory_viability_rule ...') as Action;
CREATE  INDEX `ndx_fk_ivr_owned`  ON `gringlobal`.`inventory_viability_rule` (`owned_by`);

select concat(now(), ' creating index ndx_fk_ivr_t for table inventory_viability_rule ...') as Action;
CREATE  INDEX `ndx_fk_ivr_t`  ON `gringlobal`.`inventory_viability_rule` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_ivr for table inventory_viability_rule ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ivr`  ON `gringlobal`.`inventory_viability_rule` (`taxonomy_species_id`, `name`);

/************ 4 Index Definitions for literature *************/
select concat(now(), ' creating index ndx_fk_l_created for table literature ...') as Action;
CREATE  INDEX `ndx_fk_l_created`  ON `gringlobal`.`literature` (`created_by`);

select concat(now(), ' creating index ndx_fk_l_modified for table literature ...') as Action;
CREATE  INDEX `ndx_fk_l_modified`  ON `gringlobal`.`literature` (`modified_by`);

select concat(now(), ' creating index ndx_fk_l_owned for table literature ...') as Action;
CREATE  INDEX `ndx_fk_l_owned`  ON `gringlobal`.`literature` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_l for table literature ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_l`  ON `gringlobal`.`literature` (`abbreviation`);

/************ 5 Index Definitions for method *************/
select concat(now(), ' creating index ndx_fk_m_created for table method ...') as Action;
CREATE  INDEX `ndx_fk_m_created`  ON `gringlobal`.`method` (`created_by`);

select concat(now(), ' creating index ndx_fk_m_g for table method ...') as Action;
CREATE  INDEX `ndx_fk_m_g`  ON `gringlobal`.`method` (`geography_id`);

select concat(now(), ' creating index ndx_fk_m_modified for table method ...') as Action;
CREATE  INDEX `ndx_fk_m_modified`  ON `gringlobal`.`method` (`modified_by`);

select concat(now(), ' creating index ndx_fk_m_owned for table method ...') as Action;
CREATE  INDEX `ndx_fk_m_owned`  ON `gringlobal`.`method` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_m for table method ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_m`  ON `gringlobal`.`method` (`name`);

/************ 6 Index Definitions for method_map *************/
select concat(now(), ' creating index ndx_fk_mm_c for table method_map ...') as Action;
CREATE  INDEX `ndx_fk_mm_c`  ON `gringlobal`.`method_map` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_mm_created for table method_map ...') as Action;
CREATE  INDEX `ndx_fk_mm_created`  ON `gringlobal`.`method_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_mm_m for table method_map ...') as Action;
CREATE  INDEX `ndx_fk_mm_m`  ON `gringlobal`.`method_map` (`method_id`);

select concat(now(), ' creating index ndx_fk_mm_modified for table method_map ...') as Action;
CREATE  INDEX `ndx_fk_mm_modified`  ON `gringlobal`.`method_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_mm_owned for table method_map ...') as Action;
CREATE  INDEX `ndx_fk_mm_owned`  ON `gringlobal`.`method_map` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_mm for table method_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_mm`  ON `gringlobal`.`method_map` (`cooperator_id`, `method_id`);

/************ 4 Index Definitions for name_group *************/
select concat(now(), ' creating index ndx_fk_ng_created for table name_group ...') as Action;
CREATE  INDEX `ndx_fk_ng_created`  ON `gringlobal`.`name_group` (`created_by`);

select concat(now(), ' creating index ndx_fk_ng_modified for table name_group ...') as Action;
CREATE  INDEX `ndx_fk_ng_modified`  ON `gringlobal`.`name_group` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ng_owned for table name_group ...') as Action;
CREATE  INDEX `ndx_fk_ng_owned`  ON `gringlobal`.`name_group` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ng for table name_group ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ng`  ON `gringlobal`.`name_group` (`group_name`);

/************ 8 Index Definitions for order_request *************/
select concat(now(), ' creating index ndx_fk_or_created for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_created`  ON `gringlobal`.`order_request` (`created_by`);

select concat(now(), ' creating index ndx_fk_or_final_c for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_final_c`  ON `gringlobal`.`order_request` (`final_recipient_cooperator_id`);

select concat(now(), ' creating index ndx_fk_or_modified for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_modified`  ON `gringlobal`.`order_request` (`modified_by`);

select concat(now(), ' creating index ndx_fk_or_original_or for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_original_or`  ON `gringlobal`.`order_request` (`original_order_request_id`);

select concat(now(), ' creating index ndx_fk_or_owned for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_owned`  ON `gringlobal`.`order_request` (`owned_by`);

select concat(now(), ' creating index ndx_fk_or_requestor_c for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_requestor_c`  ON `gringlobal`.`order_request` (`requestor_cooperator_id`);

select concat(now(), ' creating index ndx_fk_or_ship_to_c for table order_request ...') as Action;
CREATE  INDEX `ndx_fk_or_ship_to_c`  ON `gringlobal`.`order_request` (`ship_to_cooperator_id`);

select concat(now(), ' creating index ndx_or_obtained for table order_request ...') as Action;
CREATE  INDEX `ndx_or_obtained`  ON `gringlobal`.`order_request` (`order_obtained_via`);

/************ 6 Index Definitions for order_request_action *************/
select concat(now(), ' creating index ndx_fk_ora_c for table order_request_action ...') as Action;
CREATE  INDEX `ndx_fk_ora_c`  ON `gringlobal`.`order_request_action` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_ora_created for table order_request_action ...') as Action;
CREATE  INDEX `ndx_fk_ora_created`  ON `gringlobal`.`order_request_action` (`created_by`);

select concat(now(), ' creating index ndx_fk_ora_modified for table order_request_action ...') as Action;
CREATE  INDEX `ndx_fk_ora_modified`  ON `gringlobal`.`order_request_action` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ora_or for table order_request_action ...') as Action;
CREATE  INDEX `ndx_fk_ora_or`  ON `gringlobal`.`order_request_action` (`order_request_id`);

select concat(now(), ' creating index ndx_fk_ora_owned for table order_request_action ...') as Action;
CREATE  INDEX `ndx_fk_ora_owned`  ON `gringlobal`.`order_request_action` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_ora for table order_request_action ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ora`  ON `gringlobal`.`order_request_action` (`order_request_id`, `action_name_code`, `started_date`);

/************ 6 Index Definitions for order_request_attach *************/
select concat(now(), ' creating index ndx_fk_oat_or for table order_request_attach ...') as Action;
CREATE  INDEX `ndx_fk_oat_or`  ON `gringlobal`.`order_request_attach` (`order_request_id`);

select concat(now(), ' creating index ndx_fk_orat_c for table order_request_attach ...') as Action;
CREATE  INDEX `ndx_fk_orat_c`  ON `gringlobal`.`order_request_attach` (`attach_cooperator_id`);

select concat(now(), ' creating index ndx_fk_orat_created for table order_request_attach ...') as Action;
CREATE  INDEX `ndx_fk_orat_created`  ON `gringlobal`.`order_request_attach` (`created_by`);

select concat(now(), ' creating index ndx_fk_orat_modified for table order_request_attach ...') as Action;
CREATE  INDEX `ndx_fk_orat_modified`  ON `gringlobal`.`order_request_attach` (`modified_by`);

select concat(now(), ' creating index ndx_fk_orat_owned for table order_request_attach ...') as Action;
CREATE  INDEX `ndx_fk_orat_owned`  ON `gringlobal`.`order_request_attach` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_orat for table order_request_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_orat`  ON `gringlobal`.`order_request_attach` (`order_request_id`, `virtual_path`);

/************ 8 Index Definitions for order_request_item *************/
select concat(now(), ' creating index ndx_fk_ori_created for table order_request_item ...') as Action;
CREATE  INDEX `ndx_fk_ori_created`  ON `gringlobal`.`order_request_item` (`created_by`);

select concat(now(), ' creating index ndx_fk_ori_i for table order_request_item ...') as Action;
CREATE  INDEX `ndx_fk_ori_i`  ON `gringlobal`.`order_request_item` (`inventory_id`);

select concat(now(), ' creating index ndx_fk_ori_modified for table order_request_item ...') as Action;
CREATE  INDEX `ndx_fk_ori_modified`  ON `gringlobal`.`order_request_item` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ori_or for table order_request_item ...') as Action;
CREATE  INDEX `ndx_fk_ori_or`  ON `gringlobal`.`order_request_item` (`order_request_id`);

select concat(now(), ' creating index ndx_fk_ori_owned for table order_request_item ...') as Action;
CREATE  INDEX `ndx_fk_ori_owned`  ON `gringlobal`.`order_request_item` (`owned_by`);

select concat(now(), ' creating index ndx_fk_ori_sc for table order_request_item ...') as Action;
CREATE  INDEX `ndx_fk_ori_sc`  ON `gringlobal`.`order_request_item` (`source_cooperator_id`);

select concat(now(), ' creating index ndx_ori_item for table order_request_item ...') as Action;
CREATE  INDEX `ndx_ori_item`  ON `gringlobal`.`order_request_item` (`order_request_id`, `name`);

select concat(now(), ' creating index ndx_uniq_ori for table order_request_item ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ori`  ON `gringlobal`.`order_request_item` (`order_request_id`, `sequence_number`);

/************ 4 Index Definitions for region *************/
select concat(now(), ' creating index ndx_fk_r_created for table region ...') as Action;
CREATE  INDEX `ndx_fk_r_created`  ON `gringlobal`.`region` (`created_by`);

select concat(now(), ' creating index ndx_fk_r_modified for table region ...') as Action;
CREATE  INDEX `ndx_fk_r_modified`  ON `gringlobal`.`region` (`modified_by`);

select concat(now(), ' creating index ndx_fk_r_owned for table region ...') as Action;
CREATE  INDEX `ndx_fk_r_owned`  ON `gringlobal`.`region` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_re for table region ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_re`  ON `gringlobal`.`region` (`continent`, `subcontinent`);

/************ 6 Index Definitions for region_lang *************/
select concat(now(), ' creating index ndx_fk_rl_created for table region_lang ...') as Action;
CREATE  INDEX `ndx_fk_rl_created`  ON `gringlobal`.`region_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_rl_modified for table region_lang ...') as Action;
CREATE  INDEX `ndx_fk_rl_modified`  ON `gringlobal`.`region_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_rl_owned for table region_lang ...') as Action;
CREATE  INDEX `ndx_fk_rl_owned`  ON `gringlobal`.`region_lang` (`owned_by`);

select concat(now(), ' creating index ndx_fk_rl_r for table region_lang ...') as Action;
CREATE  INDEX `ndx_fk_rl_r`  ON `gringlobal`.`region_lang` (`region_id`);

select concat(now(), ' creating index ndx_fk_rl_sl for table region_lang ...') as Action;
CREATE  INDEX `ndx_fk_rl_sl`  ON `gringlobal`.`region_lang` (`sys_lang_id`);

select concat(now(), ' creating index ndx_uniq_rl for table region_lang ...') as Action;
CREATE  INDEX `ndx_uniq_rl`  ON `gringlobal`.`region_lang` (`region_id`, `sys_lang_id`);

/************ 4 Index Definitions for site *************/
select concat(now(), ' creating index ndx_fk_s_created for table site ...') as Action;
CREATE  INDEX `ndx_fk_s_created`  ON `gringlobal`.`site` (`created_by`);

select concat(now(), ' creating index ndx_fk_s_modified for table site ...') as Action;
CREATE  INDEX `ndx_fk_s_modified`  ON `gringlobal`.`site` (`modified_by`);

select concat(now(), ' creating index ndx_fk_s_owned for table site ...') as Action;
CREATE  INDEX `ndx_fk_s_owned`  ON `gringlobal`.`site` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_s for table site ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_s`  ON `gringlobal`.`site` (`site_short_name`, `organization_abbrev`);

/************ No index definitions exist for site_inventory_nc7 *************/

select concat(now(), ' no index definitions exist for table site_inventory_nc7') as Action;
/************ 1 Index Definitions for sys_database *************/
select concat(now(), ' creating index ndx_uniq_sdb for table sys_database ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdb`  ON `gringlobal`.`sys_database` (`migration_number`);

/************ 1 Index Definitions for sys_database_migration *************/
select concat(now(), ' creating index ndx_uniq_sdbm for table sys_database_migration ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdbm`  ON `gringlobal`.`sys_database_migration` (`migration_number`, `sort_order`);

/************ 1 Index Definitions for sys_database_migration_lang *************/
select concat(now(), ' creating index ndx_uniq_sdbml for table sys_database_migration_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdbml`  ON `gringlobal`.`sys_database_migration_lang` (`sys_database_migration_id`, `language_iso_639_3_code`);

/************ 1 Index Definitions for sys_datatrigger *************/
select concat(now(), ' creating index ndx_uniq_sd for table sys_datatrigger ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sd`  ON `gringlobal`.`sys_datatrigger` (`sys_dataview_id`, `sys_table_id`, `fully_qualified_class_name`);

/************ No index definitions exist for sys_datatrigger_lang *************/

select concat(now(), ' no index definitions exist for table sys_datatrigger_lang') as Action;
/************ 4 Index Definitions for sys_dataview *************/
select concat(now(), ' creating index ndx_fk_sr_created for table sys_dataview ...') as Action;
CREATE  INDEX `ndx_fk_sr_created`  ON `gringlobal`.`sys_dataview` (`created_by`);

select concat(now(), ' creating index ndx_fk_sr_modified for table sys_dataview ...') as Action;
CREATE  INDEX `ndx_fk_sr_modified`  ON `gringlobal`.`sys_dataview` (`modified_by`);

select concat(now(), ' creating index ndx_fk_sr_owned for table sys_dataview ...') as Action;
CREATE  INDEX `ndx_fk_sr_owned`  ON `gringlobal`.`sys_dataview` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_dataview for table sys_dataview ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_dataview`  ON `gringlobal`.`sys_dataview` (`dataview_name`);

/************ 6 Index Definitions for sys_dataview_field *************/
select concat(now(), ' creating index ndx_fk_srf_created for table sys_dataview_field ...') as Action;
CREATE  INDEX `ndx_fk_srf_created`  ON `gringlobal`.`sys_dataview_field` (`created_by`);

select concat(now(), ' creating index ndx_fk_srf_modified for table sys_dataview_field ...') as Action;
CREATE  INDEX `ndx_fk_srf_modified`  ON `gringlobal`.`sys_dataview_field` (`modified_by`);

select concat(now(), ' creating index ndx_fk_srf_owned for table sys_dataview_field ...') as Action;
CREATE  INDEX `ndx_fk_srf_owned`  ON `gringlobal`.`sys_dataview_field` (`owned_by`);

select concat(now(), ' creating index ndx_fk_srf_sr for table sys_dataview_field ...') as Action;
CREATE  INDEX `ndx_fk_srf_sr`  ON `gringlobal`.`sys_dataview_field` (`sys_dataview_id`);

select concat(now(), ' creating index ndx_fk_srf_stf for table sys_dataview_field ...') as Action;
CREATE  INDEX `ndx_fk_srf_stf`  ON `gringlobal`.`sys_dataview_field` (`sys_table_field_id`);

select concat(now(), ' creating index ndx_uniq_sdf for table sys_dataview_field ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdf`  ON `gringlobal`.`sys_dataview_field` (`sys_dataview_id`, `field_name`);

/************ 6 Index Definitions for sys_dataview_field_lang *************/
select concat(now(), ' creating index ndx_fk_srfl_created for table sys_dataview_field_lang ...') as Action;
CREATE  INDEX `ndx_fk_srfl_created`  ON `gringlobal`.`sys_dataview_field_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_srfl_modified for table sys_dataview_field_lang ...') as Action;
CREATE  INDEX `ndx_fk_srfl_modified`  ON `gringlobal`.`sys_dataview_field_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_srfl_owned for table sys_dataview_field_lang ...') as Action;
CREATE  INDEX `ndx_fk_srfl_owned`  ON `gringlobal`.`sys_dataview_field_lang` (`owned_by`);

select concat(now(), ' creating index ndx_fk_srfl_sl for table sys_dataview_field_lang ...') as Action;
CREATE  INDEX `ndx_fk_srfl_sl`  ON `gringlobal`.`sys_dataview_field_lang` (`sys_lang_id`);

select concat(now(), ' creating index ndx_fk_srfl_srf for table sys_dataview_field_lang ...') as Action;
CREATE  INDEX `ndx_fk_srfl_srf`  ON `gringlobal`.`sys_dataview_field_lang` (`sys_dataview_field_id`);

select concat(now(), ' creating index ndx_uniq_sdfl for table sys_dataview_field_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdfl`  ON `gringlobal`.`sys_dataview_field_lang` (`sys_dataview_field_id`, `sys_lang_id`);

/************ 1 Index Definitions for sys_dataview_lang *************/
select concat(now(), ' creating index ndx_uniq_sdl for table sys_dataview_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdl`  ON `gringlobal`.`sys_dataview_lang` (`sys_dataview_id`, `sys_lang_id`);

/************ 5 Index Definitions for sys_dataview_param *************/
select concat(now(), ' creating index ndx_fk_srp_created for table sys_dataview_param ...') as Action;
CREATE  INDEX `ndx_fk_srp_created`  ON `gringlobal`.`sys_dataview_param` (`created_by`);

select concat(now(), ' creating index ndx_fk_srp_modified for table sys_dataview_param ...') as Action;
CREATE  INDEX `ndx_fk_srp_modified`  ON `gringlobal`.`sys_dataview_param` (`modified_by`);

select concat(now(), ' creating index ndx_fk_srp_owned for table sys_dataview_param ...') as Action;
CREATE  INDEX `ndx_fk_srp_owned`  ON `gringlobal`.`sys_dataview_param` (`owned_by`);

select concat(now(), ' creating index ndx_fk_srp_sr for table sys_dataview_param ...') as Action;
CREATE  INDEX `ndx_fk_srp_sr`  ON `gringlobal`.`sys_dataview_param` (`sys_dataview_id`);

select concat(now(), ' creating index ndx_uniq_sdp for table sys_dataview_param ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sdp`  ON `gringlobal`.`sys_dataview_param` (`sys_dataview_id`, `param_name`);

/************ 1 Index Definitions for sys_dataview_sql *************/
select concat(now(), ' creating index ndx_uniq_sds for table sys_dataview_sql ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sds`  ON `gringlobal`.`sys_dataview_sql` (`sys_dataview_id`, `database_engine_tag`);

/************ 1 Index Definitions for sys_file *************/
select concat(now(), ' creating index ndx_uniq_sf for table sys_file ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sf`  ON `gringlobal`.`sys_file` (`virtual_file_path`);

/************ 1 Index Definitions for sys_file_group *************/
select concat(now(), ' creating index ndx_uniq_sfg for table sys_file_group ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sfg`  ON `gringlobal`.`sys_file_group` (`group_name`);

/************ 1 Index Definitions for sys_file_group_map *************/
select concat(now(), ' creating index ndx_sfgm for table sys_file_group_map ...') as Action;
CREATE UNIQUE INDEX `ndx_sfgm`  ON `gringlobal`.`sys_file_group_map` (`sys_file_group_id`, `sys_file_id`);

/************ 1 Index Definitions for sys_file_lang *************/
select concat(now(), ' creating index ndx_uniq_sfl for table sys_file_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sfl`  ON `gringlobal`.`sys_file_lang` (`sys_file_id`, `sys_lang_id`);

/************ No index definitions exist for sys_group *************/

select concat(now(), ' no index definitions exist for table sys_group') as Action;
/************ 1 Index Definitions for sys_group_lang *************/
select concat(now(), ' creating index ndx_uniq_sgl for table sys_group_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sgl`  ON `gringlobal`.`sys_group_lang` (`sys_group_id`, `sys_lang_id`);

/************ 1 Index Definitions for sys_group_permission_map *************/
select concat(now(), ' creating index ndx_uniq_sgpm for table sys_group_permission_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sgpm`  ON `gringlobal`.`sys_group_permission_map` (`sys_group_id`, `sys_permission_id`);

/************ 1 Index Definitions for sys_group_user_map *************/
select concat(now(), ' creating index ndx_uniq_sgum for table sys_group_user_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sgum`  ON `gringlobal`.`sys_group_user_map` (`sys_group_id`, `sys_user_id`);

/************ 5 Index Definitions for sys_index *************/
select concat(now(), ' creating index ndx_fk_si_created for table sys_index ...') as Action;
CREATE  INDEX `ndx_fk_si_created`  ON `gringlobal`.`sys_index` (`created_by`);

select concat(now(), ' creating index ndx_fk_si_modified for table sys_index ...') as Action;
CREATE  INDEX `ndx_fk_si_modified`  ON `gringlobal`.`sys_index` (`modified_by`);

select concat(now(), ' creating index ndx_fk_si_owned for table sys_index ...') as Action;
CREATE  INDEX `ndx_fk_si_owned`  ON `gringlobal`.`sys_index` (`owned_by`);

select concat(now(), ' creating index ndx_fk_si_st for table sys_index ...') as Action;
CREATE  INDEX `ndx_fk_si_st`  ON `gringlobal`.`sys_index` (`sys_table_id`);

select concat(now(), ' creating index ndx_uniq_si for table sys_index ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_si`  ON `gringlobal`.`sys_index` (`index_name`, `sys_table_id`);

/************ 6 Index Definitions for sys_index_field *************/
select concat(now(), ' creating index ndx_fk_sif_created for table sys_index_field ...') as Action;
CREATE  INDEX `ndx_fk_sif_created`  ON `gringlobal`.`sys_index_field` (`created_by`);

select concat(now(), ' creating index ndx_fk_sif_modified for table sys_index_field ...') as Action;
CREATE  INDEX `ndx_fk_sif_modified`  ON `gringlobal`.`sys_index_field` (`modified_by`);

select concat(now(), ' creating index ndx_fk_sif_owned for table sys_index_field ...') as Action;
CREATE  INDEX `ndx_fk_sif_owned`  ON `gringlobal`.`sys_index_field` (`owned_by`);

select concat(now(), ' creating index ndx_fk_sif_si for table sys_index_field ...') as Action;
CREATE  INDEX `ndx_fk_sif_si`  ON `gringlobal`.`sys_index_field` (`sys_index_id`);

select concat(now(), ' creating index ndx_fk_sif_stf for table sys_index_field ...') as Action;
CREATE  INDEX `ndx_fk_sif_stf`  ON `gringlobal`.`sys_index_field` (`sys_table_field_id`);

select concat(now(), ' creating index ndx_uniq_sif for table sys_index_field ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sif`  ON `gringlobal`.`sys_index_field` (`sys_index_id`, `sys_index_field_id`);

/************ 5 Index Definitions for sys_lang *************/
select concat(now(), ' creating index ndx_fk_sl_created for table sys_lang ...') as Action;
CREATE  INDEX `ndx_fk_sl_created`  ON `gringlobal`.`sys_lang` (`created_by`);

select concat(now(), ' creating index ndx_fk_sl_modified for table sys_lang ...') as Action;
CREATE  INDEX `ndx_fk_sl_modified`  ON `gringlobal`.`sys_lang` (`modified_by`);

select concat(now(), ' creating index ndx_fk_sl_owned for table sys_lang ...') as Action;
CREATE  INDEX `ndx_fk_sl_owned`  ON `gringlobal`.`sys_lang` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_sl_code for table sys_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sl_code`  ON `gringlobal`.`sys_lang` (`iso_639_3_tag`);

select concat(now(), ' creating index ndx_uniq_sl_tag for table sys_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sl_tag`  ON `gringlobal`.`sys_lang` (`ietf_tag`);

/************ 3 Index Definitions for sys_permission *************/
select concat(now(), ' creating index ndx_fk_sp_created for table sys_permission ...') as Action;
CREATE  INDEX `ndx_fk_sp_created`  ON `gringlobal`.`sys_permission` (`created_by`);

select concat(now(), ' creating index ndx_fk_sp_modified for table sys_permission ...') as Action;
CREATE  INDEX `ndx_fk_sp_modified`  ON `gringlobal`.`sys_permission` (`modified_by`);

select concat(now(), ' creating index ndx_fk_sp_owned for table sys_permission ...') as Action;
CREATE  INDEX `ndx_fk_sp_owned`  ON `gringlobal`.`sys_permission` (`owned_by`);

/************ 1 Index Definitions for sys_permission_field *************/
select concat(now(), ' creating index ndx_uniq_spf for table sys_permission_field ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_spf`  ON `gringlobal`.`sys_permission_field` (`sys_permission_id`, `sys_dataview_field_id`, `sys_table_field_id`, `compare_mode`, `compare_operator`, `parent_compare_operator`);

/************ 1 Index Definitions for sys_permission_lang *************/
select concat(now(), ' creating index ndx_uniq_spl for table sys_permission_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_spl`  ON `gringlobal`.`sys_permission_lang` (`sys_permission_id`, `sys_lang_id`);

/************ 4 Index Definitions for sys_table *************/
select concat(now(), ' creating index ndx_fk_st_created for table sys_table ...') as Action;
CREATE  INDEX `ndx_fk_st_created`  ON `gringlobal`.`sys_table` (`created_by`);

select concat(now(), ' creating index ndx_fk_st_modified for table sys_table ...') as Action;
CREATE  INDEX `ndx_fk_st_modified`  ON `gringlobal`.`sys_table` (`modified_by`);

select concat(now(), ' creating index ndx_fk_st_owned for table sys_table ...') as Action;
CREATE  INDEX `ndx_fk_st_owned`  ON `gringlobal`.`sys_table` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_st for table sys_table ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_st`  ON `gringlobal`.`sys_table` (`table_name`);

/************ 6 Index Definitions for sys_table_field *************/
select concat(now(), ' creating index ndx_fk_stf_cdgrp for table sys_table_field ...') as Action;
CREATE  INDEX `ndx_fk_stf_cdgrp`  ON `gringlobal`.`sys_table_field` (`group_name`);

select concat(now(), ' creating index ndx_fk_stf_created for table sys_table_field ...') as Action;
CREATE  INDEX `ndx_fk_stf_created`  ON `gringlobal`.`sys_table_field` (`created_by`);

select concat(now(), ' creating index ndx_fk_stf_modified for table sys_table_field ...') as Action;
CREATE  INDEX `ndx_fk_stf_modified`  ON `gringlobal`.`sys_table_field` (`modified_by`);

select concat(now(), ' creating index ndx_fk_stf_owned for table sys_table_field ...') as Action;
CREATE  INDEX `ndx_fk_stf_owned`  ON `gringlobal`.`sys_table_field` (`owned_by`);

select concat(now(), ' creating index ndx_fk_stf_st for table sys_table_field ...') as Action;
CREATE  INDEX `ndx_fk_stf_st`  ON `gringlobal`.`sys_table_field` (`sys_table_id`);

select concat(now(), ' creating index ndx_uniq_stf for table sys_table_field ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_stf`  ON `gringlobal`.`sys_table_field` (`sys_table_id`, `field_name`);

/************ 1 Index Definitions for sys_table_field_lang *************/
select concat(now(), ' creating index ndk_uniq_stfl for table sys_table_field_lang ...') as Action;
CREATE UNIQUE INDEX `ndk_uniq_stfl`  ON `gringlobal`.`sys_table_field_lang` (`sys_table_field_id`, `sys_lang_id`);

/************ 1 Index Definitions for sys_table_lang *************/
select concat(now(), ' creating index ndx_uniq_stl for table sys_table_lang ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_stl`  ON `gringlobal`.`sys_table_lang` (`sys_table_id`, `sys_lang_id`);

/************ 1 Index Definitions for sys_table_relationship *************/
select concat(now(), ' creating index ndx_uniq_str for table sys_table_relationship ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_str`  ON `gringlobal`.`sys_table_relationship` (`sys_table_field_id`, `relationship_type_tag`, `other_table_field_id`);

/************ 5 Index Definitions for sys_user *************/
select concat(now(), ' creating index ndx_fk_su_co for table sys_user ...') as Action;
CREATE  INDEX `ndx_fk_su_co`  ON `gringlobal`.`sys_user` (`cooperator_id`);

select concat(now(), ' creating index ndx_fk_su_created for table sys_user ...') as Action;
CREATE  INDEX `ndx_fk_su_created`  ON `gringlobal`.`sys_user` (`created_by`);

select concat(now(), ' creating index ndx_fk_su_modified for table sys_user ...') as Action;
CREATE  INDEX `ndx_fk_su_modified`  ON `gringlobal`.`sys_user` (`modified_by`);

select concat(now(), ' creating index ndx_fk_su_owned for table sys_user ...') as Action;
CREATE  INDEX `ndx_fk_su_owned`  ON `gringlobal`.`sys_user` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_su_name for table sys_user ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_su_name`  ON `gringlobal`.`sys_user` (`user_name`);

/************ 6 Index Definitions for sys_user_permission_map *************/
select concat(now(), ' creating index ndx_fk_sup_created for table sys_user_permission_map ...') as Action;
CREATE  INDEX `ndx_fk_sup_created`  ON `gringlobal`.`sys_user_permission_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_sup_modified for table sys_user_permission_map ...') as Action;
CREATE  INDEX `ndx_fk_sup_modified`  ON `gringlobal`.`sys_user_permission_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_sup_owned for table sys_user_permission_map ...') as Action;
CREATE  INDEX `ndx_fk_sup_owned`  ON `gringlobal`.`sys_user_permission_map` (`owned_by`);

select concat(now(), ' creating index ndx_fk_sup_sp for table sys_user_permission_map ...') as Action;
CREATE  INDEX `ndx_fk_sup_sp`  ON `gringlobal`.`sys_user_permission_map` (`sys_permission_id`);

select concat(now(), ' creating index ndx_fk_sup_su for table sys_user_permission_map ...') as Action;
CREATE  INDEX `ndx_fk_sup_su`  ON `gringlobal`.`sys_user_permission_map` (`sys_user_id`);

select concat(now(), ' creating index ndx_uniq_sup for table sys_user_permission_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_sup`  ON `gringlobal`.`sys_user_permission_map` (`sys_permission_id`, `sys_user_id`);

/************ 6 Index Definitions for taxonomy_alt_family_map *************/
select concat(now(), ' creating index ndx_fk_tafm_created for table taxonomy_alt_family_map ...') as Action;
CREATE  INDEX `ndx_fk_tafm_created`  ON `gringlobal`.`taxonomy_alt_family_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_tafm_f for table taxonomy_alt_family_map ...') as Action;
CREATE  INDEX `ndx_fk_tafm_f`  ON `gringlobal`.`taxonomy_alt_family_map` (`taxonomy_family_id`);

select concat(now(), ' creating index ndx_fk_tafm_g for table taxonomy_alt_family_map ...') as Action;
CREATE  INDEX `ndx_fk_tafm_g`  ON `gringlobal`.`taxonomy_alt_family_map` (`taxonomy_genus_id`);

select concat(now(), ' creating index ndx_fk_tafm_modified for table taxonomy_alt_family_map ...') as Action;
CREATE  INDEX `ndx_fk_tafm_modified`  ON `gringlobal`.`taxonomy_alt_family_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tafm_owned for table taxonomy_alt_family_map ...') as Action;
CREATE  INDEX `ndx_fk_tafm_owned`  ON `gringlobal`.`taxonomy_alt_family_map` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_tafm for table taxonomy_alt_family_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tafm`  ON `gringlobal`.`taxonomy_alt_family_map` (`taxonomy_genus_id`, `taxonomy_family_id`);

/************ 7 Index Definitions for taxonomy_attach *************/
select concat(now(), ' creating index ndx_fk_tat_created for table taxonomy_attach ...') as Action;
CREATE  INDEX `ndx_fk_tat_created`  ON `gringlobal`.`taxonomy_attach` (`created_by`);

select concat(now(), ' creating index ndx_fk_tat_modified for table taxonomy_attach ...') as Action;
CREATE  INDEX `ndx_fk_tat_modified`  ON `gringlobal`.`taxonomy_attach` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tat_owned for table taxonomy_attach ...') as Action;
CREATE  INDEX `ndx_fk_tat_owned`  ON `gringlobal`.`taxonomy_attach` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tat_tf for table taxonomy_attach ...') as Action;
CREATE  INDEX `ndx_fk_tat_tf`  ON `gringlobal`.`taxonomy_attach` (`taxonomy_family_id`);

select concat(now(), ' creating index ndx_fk_tat_tg for table taxonomy_attach ...') as Action;
CREATE  INDEX `ndx_fk_tat_tg`  ON `gringlobal`.`taxonomy_attach` (`taxonomy_genus_id`);

select concat(now(), ' creating index ndx_fk_tat_ts for table taxonomy_attach ...') as Action;
CREATE  INDEX `ndx_fk_tat_ts`  ON `gringlobal`.`taxonomy_attach` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_tat for table taxonomy_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tat`  ON `gringlobal`.`taxonomy_attach` (`taxonomy_family_id`, `taxonomy_genus_id`, `taxonomy_species_id`, `virtual_path`);

/************ 5 Index Definitions for taxonomy_author *************/
select concat(now(), ' creating index ndx_fk_ta_created for table taxonomy_author ...') as Action;
CREATE  INDEX `ndx_fk_ta_created`  ON `gringlobal`.`taxonomy_author` (`created_by`);

select concat(now(), ' creating index ndx_fk_ta_modified for table taxonomy_author ...') as Action;
CREATE  INDEX `ndx_fk_ta_modified`  ON `gringlobal`.`taxonomy_author` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ta_owned for table taxonomy_author ...') as Action;
CREATE  INDEX `ndx_fk_ta_owned`  ON `gringlobal`.`taxonomy_author` (`owned_by`);

select concat(now(), ' creating index ndx_ta_name for table taxonomy_author ...') as Action;
CREATE  INDEX `ndx_ta_name`  ON `gringlobal`.`taxonomy_author` (`short_name_expanded_diacritic`);

select concat(now(), ' creating index ndx_uniq_ta for table taxonomy_author ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ta`  ON `gringlobal`.`taxonomy_author` (`short_name`);

/************ 8 Index Definitions for taxonomy_common_name *************/
select concat(now(), ' creating index ndx_cn_name for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_cn_name`  ON `gringlobal`.`taxonomy_common_name` (`name`);

select concat(now(), ' creating index ndx_cn_simplified_name for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_cn_simplified_name`  ON `gringlobal`.`taxonomy_common_name` (`simplified_name`);

select concat(now(), ' creating index ndx_fk_tcn_created for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_fk_tcn_created`  ON `gringlobal`.`taxonomy_common_name` (`created_by`);

select concat(now(), ' creating index ndx_fk_tcn_modified for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_fk_tcn_modified`  ON `gringlobal`.`taxonomy_common_name` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tcn_owned for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_fk_tcn_owned`  ON `gringlobal`.`taxonomy_common_name` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tcn_tg for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_fk_tcn_tg`  ON `gringlobal`.`taxonomy_common_name` (`taxonomy_genus_id`);

select concat(now(), ' creating index ndx_fk_tcn_ts for table taxonomy_common_name ...') as Action;
CREATE  INDEX `ndx_fk_tcn_ts`  ON `gringlobal`.`taxonomy_common_name` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_tcn for table taxonomy_common_name ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tcn`  ON `gringlobal`.`taxonomy_common_name` (`taxonomy_genus_id`, `taxonomy_species_id`, `language_description`, `name`);

/************ 6 Index Definitions for taxonomy_crop_map *************/
select concat(now(), ' creating index ndx_fk_tcm_cr for table taxonomy_crop_map ...') as Action;
CREATE  INDEX `ndx_fk_tcm_cr`  ON `gringlobal`.`taxonomy_crop_map` (`crop_id`);

select concat(now(), ' creating index ndx_fk_tcm_created for table taxonomy_crop_map ...') as Action;
CREATE  INDEX `ndx_fk_tcm_created`  ON `gringlobal`.`taxonomy_crop_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_tcm_modfied for table taxonomy_crop_map ...') as Action;
CREATE  INDEX `ndx_fk_tcm_modfied`  ON `gringlobal`.`taxonomy_crop_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tcm_owned for table taxonomy_crop_map ...') as Action;
CREATE  INDEX `ndx_fk_tcm_owned`  ON `gringlobal`.`taxonomy_crop_map` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tcm_ts for table taxonomy_crop_map ...') as Action;
CREATE  INDEX `ndx_fk_tcm_ts`  ON `gringlobal`.`taxonomy_crop_map` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_tcm for table taxonomy_crop_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tcm`  ON `gringlobal`.`taxonomy_crop_map` (`taxonomy_species_id`, `crop_id`, `alternate_crop_name`, `common_crop_name`, `is_primary_genepool`, `is_secondary_genepool`, `is_tertiary_genepool`, `is_quaternary_genepool`);

/************ 5 Index Definitions for taxonomy_family *************/
select concat(now(), ' creating index ndx_fk_tf_created for table taxonomy_family ...') as Action;
CREATE  INDEX `ndx_fk_tf_created`  ON `gringlobal`.`taxonomy_family` (`created_by`);

select concat(now(), ' creating index ndx_fk_tf_cur_tf for table taxonomy_family ...') as Action;
CREATE  INDEX `ndx_fk_tf_cur_tf`  ON `gringlobal`.`taxonomy_family` (`current_taxonomy_family_id`);

select concat(now(), ' creating index ndx_fk_tf_modified for table taxonomy_family ...') as Action;
CREATE  INDEX `ndx_fk_tf_modified`  ON `gringlobal`.`taxonomy_family` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tf_owned for table taxonomy_family ...') as Action;
CREATE  INDEX `ndx_fk_tf_owned`  ON `gringlobal`.`taxonomy_family` (`owned_by`);

select concat(now(), ' creating index ndx_uniq_tf for table taxonomy_family ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tf`  ON `gringlobal`.`taxonomy_family` (`family_name`, `author_name`, `subfamily_name`, `tribe_name`, `subtribe_name`);

/************ 6 Index Definitions for taxonomy_genus *************/
select concat(now(), ' creating index fk_tg_cur_tg for table taxonomy_genus ...') as Action;
CREATE  INDEX `fk_tg_cur_tg`  ON `gringlobal`.`taxonomy_genus` (`current_taxonomy_genus_id`);

select concat(now(), ' creating index ndx_fk_tg_created for table taxonomy_genus ...') as Action;
CREATE  INDEX `ndx_fk_tg_created`  ON `gringlobal`.`taxonomy_genus` (`created_by`);

select concat(now(), ' creating index ndx_fk_tg_modified for table taxonomy_genus ...') as Action;
CREATE  INDEX `ndx_fk_tg_modified`  ON `gringlobal`.`taxonomy_genus` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tg_owned for table taxonomy_genus ...') as Action;
CREATE  INDEX `ndx_fk_tg_owned`  ON `gringlobal`.`taxonomy_genus` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tg_tf for table taxonomy_genus ...') as Action;
CREATE  INDEX `ndx_fk_tg_tf`  ON `gringlobal`.`taxonomy_genus` (`taxonomy_family_id`);

select concat(now(), ' creating index ndx_uniq_tg for table taxonomy_genus ...') as Action;
CREATE  INDEX `ndx_uniq_tg`  ON `gringlobal`.`taxonomy_genus` (`genus_name`, `genus_authority`, `subgenus_name`, `section_name`, `subsection_name`, `series_name`, `subseries_name`);

/************ 6 Index Definitions for taxonomy_geography_map *************/
select concat(now(), ' creating index ndx_fk_tgm_created for table taxonomy_geography_map ...') as Action;
CREATE  INDEX `ndx_fk_tgm_created`  ON `gringlobal`.`taxonomy_geography_map` (`created_by`);

select concat(now(), ' creating index ndx_fk_tgm_g for table taxonomy_geography_map ...') as Action;
CREATE  INDEX `ndx_fk_tgm_g`  ON `gringlobal`.`taxonomy_geography_map` (`geography_id`);

select concat(now(), ' creating index ndx_fk_tgm_modified for table taxonomy_geography_map ...') as Action;
CREATE  INDEX `ndx_fk_tgm_modified`  ON `gringlobal`.`taxonomy_geography_map` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tgm_owned for table taxonomy_geography_map ...') as Action;
CREATE  INDEX `ndx_fk_tgm_owned`  ON `gringlobal`.`taxonomy_geography_map` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tgm_ts for table taxonomy_geography_map ...') as Action;
CREATE  INDEX `ndx_fk_tgm_ts`  ON `gringlobal`.`taxonomy_geography_map` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_tgm for table taxonomy_geography_map ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tgm`  ON `gringlobal`.`taxonomy_geography_map` (`taxonomy_species_id`, `geography_id`, `geography_status_code`);

/************ 6 Index Definitions for taxonomy_noxious *************/
select concat(now(), ' creating index ndx_fk_tn_created for table taxonomy_noxious ...') as Action;
CREATE  INDEX `ndx_fk_tn_created`  ON `gringlobal`.`taxonomy_noxious` (`created_by`);

select concat(now(), ' creating index ndx_fk_tn_g for table taxonomy_noxious ...') as Action;
CREATE  INDEX `ndx_fk_tn_g`  ON `gringlobal`.`taxonomy_noxious` (`geography_id`);

select concat(now(), ' creating index ndx_fk_tn_modified for table taxonomy_noxious ...') as Action;
CREATE  INDEX `ndx_fk_tn_modified`  ON `gringlobal`.`taxonomy_noxious` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tn_owned for table taxonomy_noxious ...') as Action;
CREATE  INDEX `ndx_fk_tn_owned`  ON `gringlobal`.`taxonomy_noxious` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tn_ts for table taxonomy_noxious ...') as Action;
CREATE  INDEX `ndx_fk_tn_ts`  ON `gringlobal`.`taxonomy_noxious` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_uniq_tn for table taxonomy_noxious ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tn`  ON `gringlobal`.`taxonomy_noxious` (`taxonomy_species_id`, `geography_id`, `noxious_type_code`);

/************ 9 Index Definitions for taxonomy_species *************/
select concat(now(), ' creating index ndx_fk_ts_c for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_c`  ON `gringlobal`.`taxonomy_species` (`verifier_cooperator_id`);

select concat(now(), ' creating index ndx_fk_ts_created for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_created`  ON `gringlobal`.`taxonomy_species` (`created_by`);

select concat(now(), ' creating index ndx_fk_ts_cur_t for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_cur_t`  ON `gringlobal`.`taxonomy_species` (`current_taxonomy_species_id`);

select concat(now(), ' creating index ndx_fk_ts_modified for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_modified`  ON `gringlobal`.`taxonomy_species` (`modified_by`);

select concat(now(), ' creating index ndx_fk_ts_owned for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_owned`  ON `gringlobal`.`taxonomy_species` (`owned_by`);

select concat(now(), ' creating index ndx_fk_ts_s for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_s`  ON `gringlobal`.`taxonomy_species` (`priority1_site_id`);

select concat(now(), ' creating index ndx_fk_ts_s2 for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_s2`  ON `gringlobal`.`taxonomy_species` (`priority2_site_id`);

select concat(now(), ' creating index ndx_fk_ts_tg for table taxonomy_species ...') as Action;
CREATE  INDEX `ndx_fk_ts_tg`  ON `gringlobal`.`taxonomy_species` (`taxonomy_genus_id`);

select concat(now(), ' creating index ndx_uniq_ts for table taxonomy_species ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_ts`  ON `gringlobal`.`taxonomy_species` (`name`, `name_authority`);

/************ 6 Index Definitions for taxonomy_use *************/
select concat(now(), ' creating index ndx_fk_tus_created for table taxonomy_use ...') as Action;
CREATE  INDEX `ndx_fk_tus_created`  ON `gringlobal`.`taxonomy_use` (`created_by`);

select concat(now(), ' creating index ndx_fk_tus_modified for table taxonomy_use ...') as Action;
CREATE  INDEX `ndx_fk_tus_modified`  ON `gringlobal`.`taxonomy_use` (`modified_by`);

select concat(now(), ' creating index ndx_fk_tus_owned for table taxonomy_use ...') as Action;
CREATE  INDEX `ndx_fk_tus_owned`  ON `gringlobal`.`taxonomy_use` (`owned_by`);

select concat(now(), ' creating index ndx_fk_tus_ts for table taxonomy_use ...') as Action;
CREATE  INDEX `ndx_fk_tus_ts`  ON `gringlobal`.`taxonomy_use` (`taxonomy_species_id`);

select concat(now(), ' creating index ndx_tu_usage for table taxonomy_use ...') as Action;
CREATE  INDEX `ndx_tu_usage`  ON `gringlobal`.`taxonomy_use` (`economic_usage_code`);

select concat(now(), ' creating index ndx_uniq_tu for table taxonomy_use ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_tu`  ON `gringlobal`.`taxonomy_use` (`taxonomy_species_id`, `economic_usage_code`, `usage_type_code`);

/************ No index definitions exist for web_cooperator *************/

select concat(now(), ' no index definitions exist for table web_cooperator') as Action;
/************ 1 Index Definitions for web_order_request *************/
select concat(now(), ' creating index ndx_uniq_wor for table web_order_request ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wor`  ON `gringlobal`.`web_order_request` (`web_cooperator_id`, `ordered_date`);

/************ 1 Index Definitions for web_order_request_action *************/
select concat(now(), ' creating index ndx_uniq_wora for table web_order_request_action ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wora`  ON `gringlobal`.`web_order_request_action` (`web_order_request_id`, `web_cooperator_id`, `acted_date`);

/************ No index definitions exist for web_order_request_address *************/

select concat(now(), ' no index definitions exist for table web_order_request_address') as Action;
/************ 1 Index Definitions for web_order_request_attach *************/
select concat(now(), ' creating index ndx_uniq_worat for table web_order_request_attach ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_worat`  ON `gringlobal`.`web_order_request_attach` (`virtual_path`);

/************ 1 Index Definitions for web_order_request_item *************/
select concat(now(), ' creating index ndx_uniq_wori for table web_order_request_item ...') as Action;
CREATE  INDEX `ndx_uniq_wori`  ON `gringlobal`.`web_order_request_item` (`web_order_request_id`, `web_cooperator_id`, `sequence_number`);

/************ 1 Index Definitions for web_user *************/
select concat(now(), ' creating index ndx_uniq_wu for table web_user ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wu`  ON `gringlobal`.`web_user` (`user_name`);

/************ 1 Index Definitions for web_user_cart *************/
select concat(now(), ' creating index ndx_uniq_wuc for table web_user_cart ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wuc`  ON `gringlobal`.`web_user_cart` (`web_user_id`, `cart_type_code`);

/************ 1 Index Definitions for web_user_cart_item *************/
select concat(now(), ' creating index ndx_uniq_wuci for table web_user_cart_item ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wuci`  ON `gringlobal`.`web_user_cart_item` (`web_user_cart_id`, `accession_id`);

/************ 1 Index Definitions for web_user_preference *************/
select concat(now(), ' creating index ndx_uniq_wup for table web_user_preference ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wup`  ON `gringlobal`.`web_user_preference` (`web_user_id`, `preference_name`);

/************ 1 Index Definitions for web_user_shipping_address *************/
select concat(now(), ' creating index ndx_uniq_wusa for table web_user_shipping_address ...') as Action;
CREATE UNIQUE INDEX `ndx_uniq_wusa`  ON `gringlobal`.`web_user_shipping_address` (`web_user_id`, `address_name`);

/***********************************************/
/*********** Constraint Definitions ************/
/***********************************************/

/********** 6 Constraint Definitions for accession **********/
select concat(now(), ' creating constraint fk_a_created for table accession ...') as Action;
ALTER TABLE `gringlobal`.`accession` ADD CONSTRAINT `fk_a_created` FOREIGN KEY `ndx_fk_a_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_a_modified for table accession ...') as Action;
ALTER TABLE `gringlobal`.`accession` ADD CONSTRAINT `fk_a_modified` FOREIGN KEY `ndx_fk_a_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_a_owned for table accession ...') as Action;
ALTER TABLE `gringlobal`.`accession` ADD CONSTRAINT `fk_a_owned` FOREIGN KEY `ndx_fk_a_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_a_s1 for table accession ...') as Action;
ALTER TABLE `gringlobal`.`accession` ADD CONSTRAINT `fk_a_s1` FOREIGN KEY `ndx_fk_a_s1` (`backup_location1_site_id`) REFERENCES `gringlobal`.`site` (`site_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_a_s2 for table accession ...') as Action;
ALTER TABLE `gringlobal`.`accession` ADD CONSTRAINT `fk_a_s2` FOREIGN KEY `ndx_fk_a_s2` (`backup_location2_site_id`) REFERENCES `gringlobal`.`site` (`site_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_a_t for table accession ...') as Action;
ALTER TABLE `gringlobal`.`accession` ADD CONSTRAINT `fk_a_t` FOREIGN KEY `ndx_fk_a_t` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for accession_action **********/
select concat(now(), ' creating constraint fk_aa_a for table accession_action ...') as Action;
ALTER TABLE `gringlobal`.`accession_action` ADD CONSTRAINT `fk_aa_a` FOREIGN KEY `ndx_fk_aa_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aa_c for table accession_action ...') as Action;
ALTER TABLE `gringlobal`.`accession_action` ADD CONSTRAINT `fk_aa_c` FOREIGN KEY `ndx_fk_aa_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aa_created for table accession_action ...') as Action;
ALTER TABLE `gringlobal`.`accession_action` ADD CONSTRAINT `fk_aa_created` FOREIGN KEY `ndx_fk_aa_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aa_m for table accession_action ...') as Action;
ALTER TABLE `gringlobal`.`accession_action` ADD CONSTRAINT `fk_aa_m` FOREIGN KEY `ndx_fk_aa_m` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aa_modified for table accession_action ...') as Action;
ALTER TABLE `gringlobal`.`accession_action` ADD CONSTRAINT `fk_aa_modified` FOREIGN KEY `ndx_fk_aa_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aa_owned for table accession_action ...') as Action;
ALTER TABLE `gringlobal`.`accession_action` ADD CONSTRAINT `fk_aa_owned` FOREIGN KEY `ndx_fk_aa_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 8 Constraint Definitions for accession_annotation **********/
select concat(now(), ' creating constraint fk_aan_c for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_c` FOREIGN KEY `ndx_fk_aan_c` (`annotation_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_created for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_created` FOREIGN KEY `ndx_fk_aan_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_i for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_i` FOREIGN KEY `ndx_fk_aan_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_modified for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_modified` FOREIGN KEY `ndx_fk_aan_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_or for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_or` FOREIGN KEY `ndx_fk_aan_or` (`order_request_id`) REFERENCES `gringlobal`.`order_request` (`order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_owned for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_owned` FOREIGN KEY `ndx_fk_aan_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_t_new for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_t_new` FOREIGN KEY `ndx_fk_aan_t_new` (`new_taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aan_t_old for table accession_annotation ...') as Action;
ALTER TABLE `gringlobal`.`accession_annotation` ADD CONSTRAINT `fk_aan_t_old` FOREIGN KEY `ndx_fk_aan_t_old` (`old_taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for accession_ipr **********/
select concat(now(), ' creating constraint fk_ar_a for table accession_ipr ...') as Action;
ALTER TABLE `gringlobal`.`accession_ipr` ADD CONSTRAINT `fk_ar_a` FOREIGN KEY `ndx_fk_ar_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ar_c for table accession_ipr ...') as Action;
ALTER TABLE `gringlobal`.`accession_ipr` ADD CONSTRAINT `fk_ar_c` FOREIGN KEY `ndx_fk_ar_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ar_created for table accession_ipr ...') as Action;
ALTER TABLE `gringlobal`.`accession_ipr` ADD CONSTRAINT `fk_ar_created` FOREIGN KEY `ndx_fk_ar_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ar_modified for table accession_ipr ...') as Action;
ALTER TABLE `gringlobal`.`accession_ipr` ADD CONSTRAINT `fk_ar_modified` FOREIGN KEY `ndx_fk_ar_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ar_owned for table accession_ipr ...') as Action;
ALTER TABLE `gringlobal`.`accession_ipr` ADD CONSTRAINT `fk_ar_owned` FOREIGN KEY `ndx_fk_ar_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for accession_name **********/
select concat(now(), ' creating constraint fk_an_a for table accession_name ...') as Action;
ALTER TABLE `gringlobal`.`accession_name` ADD CONSTRAINT `fk_an_a` FOREIGN KEY `ndx_fk_an_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_an_c for table accession_name ...') as Action;
ALTER TABLE `gringlobal`.`accession_name` ADD CONSTRAINT `fk_an_c` FOREIGN KEY `ndx_fk_an_c` (`name_source_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_an_created for table accession_name ...') as Action;
ALTER TABLE `gringlobal`.`accession_name` ADD CONSTRAINT `fk_an_created` FOREIGN KEY `ndx_fk_an_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_an_modified for table accession_name ...') as Action;
ALTER TABLE `gringlobal`.`accession_name` ADD CONSTRAINT `fk_an_modified` FOREIGN KEY `ndx_fk_an_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_an_ng for table accession_name ...') as Action;
ALTER TABLE `gringlobal`.`accession_name` ADD CONSTRAINT `fk_an_ng` FOREIGN KEY `ndx_fk_an_ng` (`name_group_id`) REFERENCES `gringlobal`.`name_group` (`name_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_an_owned for table accession_name ...') as Action;
ALTER TABLE `gringlobal`.`accession_name` ADD CONSTRAINT `fk_an_owned` FOREIGN KEY `ndx_fk_an_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for accession_pedigree **********/
select concat(now(), ' creating constraint fk_ap_a for table accession_pedigree ...') as Action;
ALTER TABLE `gringlobal`.`accession_pedigree` ADD CONSTRAINT `fk_ap_a` FOREIGN KEY `ndx_fk_ap_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ap_a_female for table accession_pedigree ...') as Action;
ALTER TABLE `gringlobal`.`accession_pedigree` ADD CONSTRAINT `fk_ap_a_female` FOREIGN KEY `ndx_fk_ap_a_female` (`female_accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ap_a_male for table accession_pedigree ...') as Action;
ALTER TABLE `gringlobal`.`accession_pedigree` ADD CONSTRAINT `fk_ap_a_male` FOREIGN KEY `ndx_fk_ap_a_male` (`male_accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ap_created for table accession_pedigree ...') as Action;
ALTER TABLE `gringlobal`.`accession_pedigree` ADD CONSTRAINT `fk_ap_created` FOREIGN KEY `ndx_fk_ap_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ap_modified for table accession_pedigree ...') as Action;
ALTER TABLE `gringlobal`.`accession_pedigree` ADD CONSTRAINT `fk_ap_modified` FOREIGN KEY `ndx_fk_ap_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ap_owned for table accession_pedigree ...') as Action;
ALTER TABLE `gringlobal`.`accession_pedigree` ADD CONSTRAINT `fk_ap_owned` FOREIGN KEY `ndx_fk_ap_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for accession_quarantine **********/
select concat(now(), ' creating constraint fk_aq_a for table accession_quarantine ...') as Action;
ALTER TABLE `gringlobal`.`accession_quarantine` ADD CONSTRAINT `fk_aq_a` FOREIGN KEY `ndx_fk_aq_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aq_c for table accession_quarantine ...') as Action;
ALTER TABLE `gringlobal`.`accession_quarantine` ADD CONSTRAINT `fk_aq_c` FOREIGN KEY `ndx_fk_aq_c` (`custodial_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aq_created for table accession_quarantine ...') as Action;
ALTER TABLE `gringlobal`.`accession_quarantine` ADD CONSTRAINT `fk_aq_created` FOREIGN KEY `ndx_fk_aq_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aq_modified for table accession_quarantine ...') as Action;
ALTER TABLE `gringlobal`.`accession_quarantine` ADD CONSTRAINT `fk_aq_modified` FOREIGN KEY `ndx_fk_aq_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_aq_owned for table accession_quarantine ...') as Action;
ALTER TABLE `gringlobal`.`accession_quarantine` ADD CONSTRAINT `fk_aq_owned` FOREIGN KEY `ndx_fk_aq_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for accession_source **********/
select concat(now(), ' creating constraint fk_as_a for table accession_source ...') as Action;
ALTER TABLE `gringlobal`.`accession_source` ADD CONSTRAINT `fk_as_a` FOREIGN KEY `ndx_fk_as_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_as_created for table accession_source ...') as Action;
ALTER TABLE `gringlobal`.`accession_source` ADD CONSTRAINT `fk_as_created` FOREIGN KEY `ndx_fk_as_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_as_g for table accession_source ...') as Action;
ALTER TABLE `gringlobal`.`accession_source` ADD CONSTRAINT `fk_as_g` FOREIGN KEY `ndx_fk_as_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_as_modified for table accession_source ...') as Action;
ALTER TABLE `gringlobal`.`accession_source` ADD CONSTRAINT `fk_as_modified` FOREIGN KEY `ndx_fk_as_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_as_owned for table accession_source ...') as Action;
ALTER TABLE `gringlobal`.`accession_source` ADD CONSTRAINT `fk_as_owned` FOREIGN KEY `ndx_fk_as_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for accession_source_map **********/
select concat(now(), ' creating constraint fk_asm_as for table accession_source_map ...') as Action;
ALTER TABLE `gringlobal`.`accession_source_map` ADD CONSTRAINT `fk_asm_as` FOREIGN KEY `ndx_fk_asm_as` (`accession_source_id`) REFERENCES `gringlobal`.`accession_source` (`accession_source_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_asm_c for table accession_source_map ...') as Action;
ALTER TABLE `gringlobal`.`accession_source_map` ADD CONSTRAINT `fk_asm_c` FOREIGN KEY `ndx_fk_asm_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_asm_created for table accession_source_map ...') as Action;
ALTER TABLE `gringlobal`.`accession_source_map` ADD CONSTRAINT `fk_asm_created` FOREIGN KEY `ndx_fk_asm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_asm_modified for table accession_source_map ...') as Action;
ALTER TABLE `gringlobal`.`accession_source_map` ADD CONSTRAINT `fk_asm_modified` FOREIGN KEY `ndx_fk_asm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_asm_owned for table accession_source_map ...') as Action;
ALTER TABLE `gringlobal`.`accession_source_map` ADD CONSTRAINT `fk_asm_owned` FOREIGN KEY `ndx_fk_asm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for accession_voucher **********/
select concat(now(), ' creating constraint fk_av_c for table accession_voucher ...') as Action;
ALTER TABLE `gringlobal`.`accession_voucher` ADD CONSTRAINT `fk_av_c` FOREIGN KEY `ndx_fk_av_c` (`collector_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_av_created for table accession_voucher ...') as Action;
ALTER TABLE `gringlobal`.`accession_voucher` ADD CONSTRAINT `fk_av_created` FOREIGN KEY `ndx_fk_av_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_av_i for table accession_voucher ...') as Action;
ALTER TABLE `gringlobal`.`accession_voucher` ADD CONSTRAINT `fk_av_i` FOREIGN KEY `ndx_fk_av_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_av_modified for table accession_voucher ...') as Action;
ALTER TABLE `gringlobal`.`accession_voucher` ADD CONSTRAINT `fk_av_modified` FOREIGN KEY `ndx_fk_av_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_av_owned for table accession_voucher ...') as Action;
ALTER TABLE `gringlobal`.`accession_voucher` ADD CONSTRAINT `fk_av_owned` FOREIGN KEY `ndx_fk_av_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for app_resource **********/
select concat(now(), ' creating constraint fk_are_created for table app_resource ...') as Action;
ALTER TABLE `gringlobal`.`app_resource` ADD CONSTRAINT `fk_are_created` FOREIGN KEY `ndx_fk_are_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_are_modified for table app_resource ...') as Action;
ALTER TABLE `gringlobal`.`app_resource` ADD CONSTRAINT `fk_are_modified` FOREIGN KEY `ndx_fk_are_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_are_owned for table app_resource ...') as Action;
ALTER TABLE `gringlobal`.`app_resource` ADD CONSTRAINT `fk_are_owned` FOREIGN KEY `ndx_fk_are_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_are_sl for table app_resource ...') as Action;
ALTER TABLE `gringlobal`.`app_resource` ADD CONSTRAINT `fk_are_sl` FOREIGN KEY `ndx_fk_are_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for app_setting **********/
select concat(now(), ' creating constraint ndx_fk_aset_created for table app_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_setting` ADD CONSTRAINT `ndx_fk_aset_created` FOREIGN KEY `ndx_ndx_fk_aset_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_aset_modified for table app_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_setting` ADD CONSTRAINT `ndx_fk_aset_modified` FOREIGN KEY `ndx_ndx_fk_aset_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_aset_owned for table app_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_setting` ADD CONSTRAINT `ndx_fk_aset_owned` FOREIGN KEY `ndx_ndx_fk_aset_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for app_user_gui_setting **********/
select concat(now(), ' creating constraint fk_sugs_co for table app_user_gui_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_user_gui_setting` ADD CONSTRAINT `fk_sugs_co` FOREIGN KEY `ndx_fk_sugs_co` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sugs_created for table app_user_gui_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_user_gui_setting` ADD CONSTRAINT `fk_sugs_created` FOREIGN KEY `ndx_fk_sugs_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sugs_modified for table app_user_gui_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_user_gui_setting` ADD CONSTRAINT `fk_sugs_modified` FOREIGN KEY `ndx_fk_sugs_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sugs_owned for table app_user_gui_setting ...') as Action;
ALTER TABLE `gringlobal`.`app_user_gui_setting` ADD CONSTRAINT `fk_sugs_owned` FOREIGN KEY `ndx_fk_sugs_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for app_user_item_list **********/
select concat(now(), ' creating constraint fk_auil_c for table app_user_item_list ...') as Action;
ALTER TABLE `gringlobal`.`app_user_item_list` ADD CONSTRAINT `fk_auil_c` FOREIGN KEY `ndx_fk_auil_c` (`app_user_item_list_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_auil_created for table app_user_item_list ...') as Action;
ALTER TABLE `gringlobal`.`app_user_item_list` ADD CONSTRAINT `ndx_fk_auil_created` FOREIGN KEY `ndx_ndx_fk_auil_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_auil_modified for table app_user_item_list ...') as Action;
ALTER TABLE `gringlobal`.`app_user_item_list` ADD CONSTRAINT `ndx_fk_auil_modified` FOREIGN KEY `ndx_ndx_fk_auil_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_auil_owned for table app_user_item_list ...') as Action;
ALTER TABLE `gringlobal`.`app_user_item_list` ADD CONSTRAINT `ndx_fk_auil_owned` FOREIGN KEY `ndx_ndx_fk_auil_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for citation **********/
select concat(now(), ' creating constraint fk_ci_created for table citation ...') as Action;
ALTER TABLE `gringlobal`.`citation` ADD CONSTRAINT `fk_ci_created` FOREIGN KEY `ndx_fk_ci_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ci_l for table citation ...') as Action;
ALTER TABLE `gringlobal`.`citation` ADD CONSTRAINT `fk_ci_l` FOREIGN KEY `ndx_fk_ci_l` (`literature_id`) REFERENCES `gringlobal`.`literature` (`literature_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ci_modified for table citation ...') as Action;
ALTER TABLE `gringlobal`.`citation` ADD CONSTRAINT `fk_ci_modified` FOREIGN KEY `ndx_fk_ci_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ci_owned for table citation ...') as Action;
ALTER TABLE `gringlobal`.`citation` ADD CONSTRAINT `fk_ci_owned` FOREIGN KEY `ndx_fk_ci_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 14 Constraint Definitions for citation_map **********/
select concat(now(), ' creating constraint fi_cim_created for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fi_cim_created` FOREIGN KEY `ndx_fi_cim_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_acc for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_acc` FOREIGN KEY `ndx_fk_cim_acc` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_cit for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_cit` FOREIGN KEY `ndx_fk_cim_cit` (`citation_id`) REFERENCES `gringlobal`.`citation` (`citation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_gm for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_gm` FOREIGN KEY `ndx_fk_cim_gm` (`genetic_marker_id`) REFERENCES `gringlobal`.`genetic_marker` (`genetic_marker_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_ip for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_ip` FOREIGN KEY `ndx_fk_cim_ip` (`accession_ipr_id`) REFERENCES `gringlobal`.`accession_ipr` (`accession_ipr_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_me for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_me` FOREIGN KEY `ndx_fk_cim_me` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_modified for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_modified` FOREIGN KEY `ndx_fk_cim_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_owned for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_owned` FOREIGN KEY `ndx_fk_cim_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_pe for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_pe` FOREIGN KEY `ndx_fk_cim_pe` (`accession_pedigree_id`) REFERENCES `gringlobal`.`accession_pedigree` (`accession_pedigree_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_ta for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_ta` FOREIGN KEY `ndx_fk_cim_ta` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_tcn for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_tcn` FOREIGN KEY `ndx_fk_cim_tcn` (`taxonomy_common_name_id`) REFERENCES `gringlobal`.`taxonomy_common_name` (`taxonomy_common_name_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_tf for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_tf` FOREIGN KEY `ndx_fk_cim_tf` (`taxonomy_family_id`) REFERENCES `gringlobal`.`taxonomy_family` (`taxonomy_family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_tg for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_tg` FOREIGN KEY `ndx_fk_cim_tg` (`taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cim_tu for table citation_map ...') as Action;
ALTER TABLE `gringlobal`.`citation_map` ADD CONSTRAINT `fk_cim_tu` FOREIGN KEY `ndx_fk_cim_tu` (`taxonomy_use_id`) REFERENCES `gringlobal`.`taxonomy_use` (`taxonomy_use_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for code_value **********/
select concat(now(), ' creating constraint fk_cdval_created for table code_value ...') as Action;
ALTER TABLE `gringlobal`.`code_value` ADD CONSTRAINT `fk_cdval_created` FOREIGN KEY `ndx_fk_cdval_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cdval_modified for table code_value ...') as Action;
ALTER TABLE `gringlobal`.`code_value` ADD CONSTRAINT `fk_cdval_modified` FOREIGN KEY `ndx_fk_cdval_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cdval_owned for table code_value ...') as Action;
ALTER TABLE `gringlobal`.`code_value` ADD CONSTRAINT `fk_cdval_owned` FOREIGN KEY `ndx_fk_cdval_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for code_value_lang **********/
select concat(now(), ' creating constraint fk_cvl_created for table code_value_lang ...') as Action;
ALTER TABLE `gringlobal`.`code_value_lang` ADD CONSTRAINT `fk_cvl_created` FOREIGN KEY `ndx_fk_cvl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cvl_cv for table code_value_lang ...') as Action;
ALTER TABLE `gringlobal`.`code_value_lang` ADD CONSTRAINT `fk_cvl_cv` FOREIGN KEY `ndx_fk_cvl_cv` (`code_value_id`) REFERENCES `gringlobal`.`code_value` (`code_value_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cvl_modified for table code_value_lang ...') as Action;
ALTER TABLE `gringlobal`.`code_value_lang` ADD CONSTRAINT `fk_cvl_modified` FOREIGN KEY `ndx_fk_cvl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cvl_owned for table code_value_lang ...') as Action;
ALTER TABLE `gringlobal`.`code_value_lang` ADD CONSTRAINT `fk_cvl_owned` FOREIGN KEY `ndx_fk_cvl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cvl_sl for table code_value_lang ...') as Action;
ALTER TABLE `gringlobal`.`code_value_lang` ADD CONSTRAINT `fk_cvl_sl` FOREIGN KEY `ndx_fk_cvl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 9 Constraint Definitions for cooperator **********/
select concat(now(), ' creating constraint fk_c_created for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_created` FOREIGN KEY `ndx_fk_c_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_cur for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_cur` FOREIGN KEY `ndx_fk_c_cur` (`current_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_cur_c for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_cur_c` FOREIGN KEY `ndx_fk_c_cur_c` (`current_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_g for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_g` FOREIGN KEY `ndx_fk_c_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_g2 for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_g2` FOREIGN KEY `ndx_fk_c_g2` (`secondary_geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_modified for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_modified` FOREIGN KEY `ndx_fk_c_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_owned for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_owned` FOREIGN KEY `ndx_fk_c_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_s for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_s` FOREIGN KEY `ndx_fk_c_s` (`site_id`) REFERENCES `gringlobal`.`site` (`site_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_c_sl for table cooperator ...') as Action;
ALTER TABLE `gringlobal`.`cooperator` ADD CONSTRAINT `fk_c_sl` FOREIGN KEY `ndx_fk_c_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for cooperator_group **********/
select concat(now(), ' creating constraint fk_cg_created for table cooperator_group ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_group` ADD CONSTRAINT `fk_cg_created` FOREIGN KEY `ndx_fk_cg_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cg_modified for table cooperator_group ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_group` ADD CONSTRAINT `fk_cg_modified` FOREIGN KEY `ndx_fk_cg_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cg_owned for table cooperator_group ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_group` ADD CONSTRAINT `fk_cg_owned` FOREIGN KEY `ndx_fk_cg_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cg_s for table cooperator_group ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_group` ADD CONSTRAINT `fk_cg_s` FOREIGN KEY `ndx_fk_cg_s` (`site_id`) REFERENCES `gringlobal`.`site` (`site_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for cooperator_map **********/
select concat(now(), ' creating constraint fk_cm_c for table cooperator_map ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_map` ADD CONSTRAINT `fk_cm_c` FOREIGN KEY `ndx_fk_cm_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cm_cg for table cooperator_map ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_map` ADD CONSTRAINT `fk_cm_cg` FOREIGN KEY `ndx_fk_cm_cg` (`cooperator_group_id`) REFERENCES `gringlobal`.`cooperator_group` (`cooperator_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cm_created for table cooperator_map ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_map` ADD CONSTRAINT `fk_cm_created` FOREIGN KEY `ndx_fk_cm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cm_modified for table cooperator_map ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_map` ADD CONSTRAINT `fk_cm_modified` FOREIGN KEY `ndx_fk_cm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cm_owned for table cooperator_map ...') as Action;
ALTER TABLE `gringlobal`.`cooperator_map` ADD CONSTRAINT `fk_cm_owned` FOREIGN KEY `ndx_fk_cm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for crop **********/
select concat(now(), ' creating constraint fk_cr_created for table crop ...') as Action;
ALTER TABLE `gringlobal`.`crop` ADD CONSTRAINT `fk_cr_created` FOREIGN KEY `ndx_fk_cr_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cr_modified for table crop ...') as Action;
ALTER TABLE `gringlobal`.`crop` ADD CONSTRAINT `fk_cr_modified` FOREIGN KEY `ndx_fk_cr_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cr_owned for table crop ...') as Action;
ALTER TABLE `gringlobal`.`crop` ADD CONSTRAINT `fk_cr_owned` FOREIGN KEY `ndx_fk_cr_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for crop_attach **********/
select concat(now(), ' creating constraint fk_ca_c for table crop_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_attach` ADD CONSTRAINT `fk_ca_c` FOREIGN KEY `ndx_fk_ca_c` (`crop_id`) REFERENCES `gringlobal`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ca_created for table crop_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_attach` ADD CONSTRAINT `fk_ca_created` FOREIGN KEY `ndx_fk_ca_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ca_modified for table crop_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_attach` ADD CONSTRAINT `fk_ca_modified` FOREIGN KEY `ndx_fk_ca_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ca_owned for table crop_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_attach` ADD CONSTRAINT `fk_ca_owned` FOREIGN KEY `ndx_fk_ca_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for crop_trait **********/
select concat(now(), ' creating constraint fk_ct_cr for table crop_trait ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait` ADD CONSTRAINT `fk_ct_cr` FOREIGN KEY `ndx_fk_ct_cr` (`crop_id`) REFERENCES `gringlobal`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ct_created for table crop_trait ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait` ADD CONSTRAINT `fk_ct_created` FOREIGN KEY `ndx_fk_ct_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ct_modified for table crop_trait ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait` ADD CONSTRAINT `fk_ct_modified` FOREIGN KEY `ndx_fk_ct_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ct_owned for table crop_trait ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait` ADD CONSTRAINT `fk_ct_owned` FOREIGN KEY `ndx_fk_ct_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for crop_trait_attach **********/
select concat(now(), ' creating constraint fk_cta_created for table crop_trait_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_attach` ADD CONSTRAINT `fk_cta_created` FOREIGN KEY `ndx_fk_cta_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cta_ct for table crop_trait_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_attach` ADD CONSTRAINT `fk_cta_ct` FOREIGN KEY `ndx_fk_cta_ct` (`crop_trait_id`) REFERENCES `gringlobal`.`crop_trait` (`crop_trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cta_modified for table crop_trait_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_attach` ADD CONSTRAINT `fk_cta_modified` FOREIGN KEY `ndx_fk_cta_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cta_owned for table crop_trait_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_attach` ADD CONSTRAINT `fk_cta_owned` FOREIGN KEY `ndx_fk_cta_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for crop_trait_code **********/
select concat(now(), ' creating constraint fk_tct_created for table crop_trait_code ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code` ADD CONSTRAINT `fk_tct_created` FOREIGN KEY `ndx_fk_tct_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tct_modified for table crop_trait_code ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code` ADD CONSTRAINT `fk_tct_modified` FOREIGN KEY `ndx_fk_tct_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tct_owned for table crop_trait_code ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code` ADD CONSTRAINT `fk_tct_owned` FOREIGN KEY `ndx_fk_tct_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tct_tr for table crop_trait_code ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code` ADD CONSTRAINT `fk_tct_tr` FOREIGN KEY `ndx_fk_tct_tr` (`crop_trait_id`) REFERENCES `gringlobal`.`crop_trait` (`crop_trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for crop_trait_code_attach **********/
select concat(now(), ' creating constraint fk_ctca_created for table crop_trait_code_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_attach` ADD CONSTRAINT `fk_ctca_created` FOREIGN KEY `ndx_fk_ctca_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctca_ctc for table crop_trait_code_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_attach` ADD CONSTRAINT `fk_ctca_ctc` FOREIGN KEY `ndx_fk_ctca_ctc` (`crop_trait_code_id`) REFERENCES `gringlobal`.`crop_trait_code` (`crop_trait_code_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctca_modified for table crop_trait_code_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_attach` ADD CONSTRAINT `fk_ctca_modified` FOREIGN KEY `ndx_fk_ctca_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctca_owned for table crop_trait_code_attach ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_attach` ADD CONSTRAINT `fk_ctca_owned` FOREIGN KEY `ndx_fk_ctca_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for crop_trait_code_lang **********/
select concat(now(), ' creating constraint fk_ctcl_created for table crop_trait_code_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_lang` ADD CONSTRAINT `fk_ctcl_created` FOREIGN KEY `ndx_fk_ctcl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctcl_modified for table crop_trait_code_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_lang` ADD CONSTRAINT `fk_ctcl_modified` FOREIGN KEY `ndx_fk_ctcl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctcl_owned for table crop_trait_code_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_lang` ADD CONSTRAINT `fk_ctcl_owned` FOREIGN KEY `ndx_fk_ctcl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctcl_sl for table crop_trait_code_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_lang` ADD CONSTRAINT `fk_ctcl_sl` FOREIGN KEY `ndx_fk_ctcl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctcl_tc for table crop_trait_code_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_code_lang` ADD CONSTRAINT `fk_ctcl_tc` FOREIGN KEY `ndx_fk_ctcl_tc` (`crop_trait_code_id`) REFERENCES `gringlobal`.`crop_trait_code` (`crop_trait_code_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for crop_trait_lang **********/
select concat(now(), ' creating constraint fk_ctl_created for table crop_trait_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_lang` ADD CONSTRAINT `fk_ctl_created` FOREIGN KEY `ndx_fk_ctl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctl_modified for table crop_trait_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_lang` ADD CONSTRAINT `fk_ctl_modified` FOREIGN KEY `ndx_fk_ctl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctl_owned for table crop_trait_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_lang` ADD CONSTRAINT `fk_ctl_owned` FOREIGN KEY `ndx_fk_ctl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctl_sl for table crop_trait_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_lang` ADD CONSTRAINT `fk_ctl_sl` FOREIGN KEY `ndx_fk_ctl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctl_t for table crop_trait_lang ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_lang` ADD CONSTRAINT `fk_ctl_t` FOREIGN KEY `ndx_fk_ctl_t` (`crop_trait_id`) REFERENCES `gringlobal`.`crop_trait` (`crop_trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 7 Constraint Definitions for crop_trait_observation **********/
select concat(now(), ' creating constraint fk_cto_created for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_created` FOREIGN KEY `ndx_fk_cto_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cto_ct for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_ct` FOREIGN KEY `ndx_fk_cto_ct` (`crop_trait_id`) REFERENCES `gringlobal`.`crop_trait` (`crop_trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cto_ctc for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_ctc` FOREIGN KEY `ndx_fk_cto_ctc` (`crop_trait_code_id`) REFERENCES `gringlobal`.`crop_trait_code` (`crop_trait_code_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cto_i for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_i` FOREIGN KEY `ndx_fk_cto_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cto_m for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_m` FOREIGN KEY `ndx_fk_cto_m` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cto_modified for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_modified` FOREIGN KEY `ndx_fk_cto_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_cto_owned for table crop_trait_observation ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation` ADD CONSTRAINT `fk_cto_owned` FOREIGN KEY `ndx_fk_cto_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 8 Constraint Definitions for crop_trait_observation_data **********/
select concat(now(), ' creating constraint fk_ctod_created for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_created` FOREIGN KEY `ndx_fk_ctod_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_ct for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_ct` FOREIGN KEY `ndx_fk_ctod_ct` (`crop_trait_id`) REFERENCES `gringlobal`.`crop_trait` (`crop_trait_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_ctc for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_ctc` FOREIGN KEY `ndx_fk_ctod_ctc` (`crop_trait_code_id`) REFERENCES `gringlobal`.`crop_trait_code` (`crop_trait_code_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_cto for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_cto` FOREIGN KEY `ndx_fk_ctod_cto` (`crop_trait_observation_id`) REFERENCES `gringlobal`.`crop_trait_observation` (`crop_trait_observation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_i for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_i` FOREIGN KEY `ndx_fk_ctod_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_m for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_m` FOREIGN KEY `ndx_fk_ctod_m` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_modified for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_modified` FOREIGN KEY `ndx_fk_ctod_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ctod_owned for table crop_trait_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`crop_trait_observation_data` ADD CONSTRAINT `fk_ctod_owned` FOREIGN KEY `ndx_fk_ctod_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for genetic_annotation **********/
select concat(now(), ' creating constraint fk_ga_created for table genetic_annotation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_annotation` ADD CONSTRAINT `fk_ga_created` FOREIGN KEY `ndx_fk_ga_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ga_gm for table genetic_annotation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_annotation` ADD CONSTRAINT `fk_ga_gm` FOREIGN KEY `ndx_fk_ga_gm` (`genetic_marker_id`) REFERENCES `gringlobal`.`genetic_marker` (`genetic_marker_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ga_m for table genetic_annotation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_annotation` ADD CONSTRAINT `fk_ga_m` FOREIGN KEY `ndx_fk_ga_m` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ga_modified for table genetic_annotation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_annotation` ADD CONSTRAINT `fk_ga_modified` FOREIGN KEY `ndx_fk_ga_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ga_owned for table genetic_annotation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_annotation` ADD CONSTRAINT `fk_ga_owned` FOREIGN KEY `ndx_fk_ga_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for genetic_marker **********/
select concat(now(), ' creating constraint fk_gm_cr for table genetic_marker ...') as Action;
ALTER TABLE `gringlobal`.`genetic_marker` ADD CONSTRAINT `fk_gm_cr` FOREIGN KEY `ndx_fk_gm_cr` (`crop_id`) REFERENCES `gringlobal`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gm_created for table genetic_marker ...') as Action;
ALTER TABLE `gringlobal`.`genetic_marker` ADD CONSTRAINT `fk_gm_created` FOREIGN KEY `ndx_fk_gm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gm_modified for table genetic_marker ...') as Action;
ALTER TABLE `gringlobal`.`genetic_marker` ADD CONSTRAINT `fk_gm_modified` FOREIGN KEY `ndx_fk_gm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gm_owned for table genetic_marker ...') as Action;
ALTER TABLE `gringlobal`.`genetic_marker` ADD CONSTRAINT `fk_gm_owned` FOREIGN KEY `ndx_fk_gm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for genetic_observation **********/
select concat(now(), ' creating constraint fk_go_created for table genetic_observation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation` ADD CONSTRAINT `fk_go_created` FOREIGN KEY `ndx_fk_go_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_go_ga for table genetic_observation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation` ADD CONSTRAINT `fk_go_ga` FOREIGN KEY `ndx_fk_go_ga` (`genetic_annotation_id`) REFERENCES `gringlobal`.`genetic_annotation` (`genetic_annotation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_go_i for table genetic_observation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation` ADD CONSTRAINT `fk_go_i` FOREIGN KEY `ndx_fk_go_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_go_modified for table genetic_observation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation` ADD CONSTRAINT `fk_go_modified` FOREIGN KEY `ndx_fk_go_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_go_owned for table genetic_observation ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation` ADD CONSTRAINT `fk_go_owned` FOREIGN KEY `ndx_fk_go_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for genetic_observation_data **********/
select concat(now(), ' creating constraint fk_god_created for table genetic_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation_data` ADD CONSTRAINT `fk_god_created` FOREIGN KEY `ndx_fk_god_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_god_ga for table genetic_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation_data` ADD CONSTRAINT `fk_god_ga` FOREIGN KEY `ndx_fk_god_ga` (`genetic_annotation_id`) REFERENCES `gringlobal`.`genetic_annotation` (`genetic_annotation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_god_i for table genetic_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation_data` ADD CONSTRAINT `fk_god_i` FOREIGN KEY `ndx_fk_god_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_god_modified for table genetic_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation_data` ADD CONSTRAINT `fk_god_modified` FOREIGN KEY `ndx_fk_god_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_god_ob for table genetic_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation_data` ADD CONSTRAINT `fk_god_ob` FOREIGN KEY `ndx_fk_god_ob` (`genetic_observation_id`) REFERENCES `gringlobal`.`genetic_observation` (`genetic_observation_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_god_owned for table genetic_observation_data ...') as Action;
ALTER TABLE `gringlobal`.`genetic_observation_data` ADD CONSTRAINT `fk_god_owned` FOREIGN KEY `ndx_fk_god_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for geography **********/
select concat(now(), ' creating constraint fk_g_created for table geography ...') as Action;
ALTER TABLE `gringlobal`.`geography` ADD CONSTRAINT `fk_g_created` FOREIGN KEY `ndx_fk_g_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_g_cur_g for table geography ...') as Action;
ALTER TABLE `gringlobal`.`geography` ADD CONSTRAINT `fk_g_cur_g` FOREIGN KEY `ndx_fk_g_cur_g` (`current_geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_g_modified for table geography ...') as Action;
ALTER TABLE `gringlobal`.`geography` ADD CONSTRAINT `fk_g_modified` FOREIGN KEY `ndx_fk_g_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_g_owned for table geography ...') as Action;
ALTER TABLE `gringlobal`.`geography` ADD CONSTRAINT `fk_g_owned` FOREIGN KEY `ndx_fk_g_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for geography_lang **********/
select concat(now(), ' creating constraint fk_gl_created for table geography_lang ...') as Action;
ALTER TABLE `gringlobal`.`geography_lang` ADD CONSTRAINT `fk_gl_created` FOREIGN KEY `ndx_fk_gl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gl_g for table geography_lang ...') as Action;
ALTER TABLE `gringlobal`.`geography_lang` ADD CONSTRAINT `fk_gl_g` FOREIGN KEY `ndx_fk_gl_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gl_modified for table geography_lang ...') as Action;
ALTER TABLE `gringlobal`.`geography_lang` ADD CONSTRAINT `fk_gl_modified` FOREIGN KEY `ndx_fk_gl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gl_owned for table geography_lang ...') as Action;
ALTER TABLE `gringlobal`.`geography_lang` ADD CONSTRAINT `fk_gl_owned` FOREIGN KEY `ndx_fk_gl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_gl_sl for table geography_lang ...') as Action;
ALTER TABLE `gringlobal`.`geography_lang` ADD CONSTRAINT `fk_gl_sl` FOREIGN KEY `ndx_fk_gl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for geography_region_map **********/
select concat(now(), ' creating constraint fk_geography_region_map_geography_region_map for table geography_region_map ...') as Action;
ALTER TABLE `gringlobal`.`geography_region_map` ADD CONSTRAINT `fk_geography_region_map_geography_region_map` FOREIGN KEY `ndx_fk_geography_region_map_geography_region_map` (`geography_region_map_id`) REFERENCES `gringlobal`.`geography_region_map` (`geography_region_map_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_grm_created for table geography_region_map ...') as Action;
ALTER TABLE `gringlobal`.`geography_region_map` ADD CONSTRAINT `fk_grm_created` FOREIGN KEY `ndx_fk_grm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_grm_g for table geography_region_map ...') as Action;
ALTER TABLE `gringlobal`.`geography_region_map` ADD CONSTRAINT `fk_grm_g` FOREIGN KEY `ndx_fk_grm_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_grm_modified for table geography_region_map ...') as Action;
ALTER TABLE `gringlobal`.`geography_region_map` ADD CONSTRAINT `fk_grm_modified` FOREIGN KEY `ndx_fk_grm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_grm_owned for table geography_region_map ...') as Action;
ALTER TABLE `gringlobal`.`geography_region_map` ADD CONSTRAINT `fk_grm_owned` FOREIGN KEY `ndx_fk_grm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fo_grm_r for table geography_region_map ...') as Action;
ALTER TABLE `gringlobal`.`geography_region_map` ADD CONSTRAINT `fo_grm_r` FOREIGN KEY `ndx_fo_grm_r` (`region_id`) REFERENCES `gringlobal`.`region` (`region_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 7 Constraint Definitions for inventory **********/
select concat(now(), ' creating constraint fk_i_a for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_a` FOREIGN KEY `ndx_fk_i_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_i_backup_i for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_backup_i` FOREIGN KEY `ndx_fk_i_backup_i` (`backup_inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_i_created for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_created` FOREIGN KEY `ndx_fk_i_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_i_im for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_im` FOREIGN KEY `ndx_fk_i_im` (`inventory_maint_policy_id`) REFERENCES `gringlobal`.`inventory_maint_policy` (`inventory_maint_policy_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_i_modified for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_modified` FOREIGN KEY `ndx_fk_i_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_i_owned for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_owned` FOREIGN KEY `ndx_fk_i_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_i_parent_i for table inventory ...') as Action;
ALTER TABLE `gringlobal`.`inventory` ADD CONSTRAINT `fk_i_parent_i` FOREIGN KEY `ndx_fk_i_parent_i` (`parent_inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for inventory_action **********/
select concat(now(), ' creating constraint fk_ia_c for table inventory_action ...') as Action;
ALTER TABLE `gringlobal`.`inventory_action` ADD CONSTRAINT `fk_ia_c` FOREIGN KEY `ndx_fk_ia_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ia_created for table inventory_action ...') as Action;
ALTER TABLE `gringlobal`.`inventory_action` ADD CONSTRAINT `fk_ia_created` FOREIGN KEY `ndx_fk_ia_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ia_i for table inventory_action ...') as Action;
ALTER TABLE `gringlobal`.`inventory_action` ADD CONSTRAINT `fk_ia_i` FOREIGN KEY `ndx_fk_ia_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ia_m for table inventory_action ...') as Action;
ALTER TABLE `gringlobal`.`inventory_action` ADD CONSTRAINT `fk_ia_m` FOREIGN KEY `ndx_fk_ia_m` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ia_modified for table inventory_action ...') as Action;
ALTER TABLE `gringlobal`.`inventory_action` ADD CONSTRAINT `fk_ia_modified` FOREIGN KEY `ndx_fk_ia_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ia_owned for table inventory_action ...') as Action;
ALTER TABLE `gringlobal`.`inventory_action` ADD CONSTRAINT `fk_ia_owned` FOREIGN KEY `ndx_fk_ia_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for inventory_attach **********/
select concat(now(), ' creating constraint fk_iat_c for table inventory_attach ...') as Action;
ALTER TABLE `gringlobal`.`inventory_attach` ADD CONSTRAINT `fk_iat_c` FOREIGN KEY `ndx_fk_iat_c` (`attach_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iat_created for table inventory_attach ...') as Action;
ALTER TABLE `gringlobal`.`inventory_attach` ADD CONSTRAINT `fk_iat_created` FOREIGN KEY `ndx_fk_iat_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iat_iid for table inventory_attach ...') as Action;
ALTER TABLE `gringlobal`.`inventory_attach` ADD CONSTRAINT `fk_iat_iid` FOREIGN KEY `ndx_fk_iat_iid` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iat_modified for table inventory_attach ...') as Action;
ALTER TABLE `gringlobal`.`inventory_attach` ADD CONSTRAINT `fk_iat_modified` FOREIGN KEY `ndx_fk_iat_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iat_owned for table inventory_attach ...') as Action;
ALTER TABLE `gringlobal`.`inventory_attach` ADD CONSTRAINT `fk_iat_owned` FOREIGN KEY `ndx_fk_iat_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for inventory_group **********/
select concat(now(), ' creating constraint fk_ig_created for table inventory_group ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group` ADD CONSTRAINT `fk_ig_created` FOREIGN KEY `ndx_fk_ig_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ig_modified for table inventory_group ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group` ADD CONSTRAINT `fk_ig_modified` FOREIGN KEY `ndx_fk_ig_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ig_owned for table inventory_group ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group` ADD CONSTRAINT `fk_ig_owned` FOREIGN KEY `ndx_fk_ig_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for inventory_group_map **********/
select concat(now(), ' creating constraint fk_igm_created for table inventory_group_map ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group_map` ADD CONSTRAINT `fk_igm_created` FOREIGN KEY `ndx_fk_igm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_igm_i for table inventory_group_map ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group_map` ADD CONSTRAINT `fk_igm_i` FOREIGN KEY `ndx_fk_igm_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_igm_ig for table inventory_group_map ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group_map` ADD CONSTRAINT `fk_igm_ig` FOREIGN KEY `ndx_fk_igm_ig` (`inventory_group_id`) REFERENCES `gringlobal`.`inventory_group` (`inventory_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_igm_modified for table inventory_group_map ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group_map` ADD CONSTRAINT `fk_igm_modified` FOREIGN KEY `ndx_fk_igm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_igm_owned for table inventory_group_map ...') as Action;
ALTER TABLE `gringlobal`.`inventory_group_map` ADD CONSTRAINT `fk_igm_owned` FOREIGN KEY `ndx_fk_igm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for inventory_maint_policy **********/
select concat(now(), ' creating constraint fk_im_co for table inventory_maint_policy ...') as Action;
ALTER TABLE `gringlobal`.`inventory_maint_policy` ADD CONSTRAINT `fk_im_co` FOREIGN KEY `ndx_fk_im_co` (`curator_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_im_created for table inventory_maint_policy ...') as Action;
ALTER TABLE `gringlobal`.`inventory_maint_policy` ADD CONSTRAINT `fk_im_created` FOREIGN KEY `ndx_fk_im_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_im_modified for table inventory_maint_policy ...') as Action;
ALTER TABLE `gringlobal`.`inventory_maint_policy` ADD CONSTRAINT `fk_im_modified` FOREIGN KEY `ndx_fk_im_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_im_owned for table inventory_maint_policy ...') as Action;
ALTER TABLE `gringlobal`.`inventory_maint_policy` ADD CONSTRAINT `fk_im_owned` FOREIGN KEY `ndx_fk_im_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for inventory_name **********/
select concat(now(), ' creating constraint fk_in_c for table inventory_name ...') as Action;
ALTER TABLE `gringlobal`.`inventory_name` ADD CONSTRAINT `fk_in_c` FOREIGN KEY `ndx_fk_in_c` (`name_source_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_in_created for table inventory_name ...') as Action;
ALTER TABLE `gringlobal`.`inventory_name` ADD CONSTRAINT `fk_in_created` FOREIGN KEY `ndx_fk_in_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_in_i for table inventory_name ...') as Action;
ALTER TABLE `gringlobal`.`inventory_name` ADD CONSTRAINT `fk_in_i` FOREIGN KEY `ndx_fk_in_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_in_modified for table inventory_name ...') as Action;
ALTER TABLE `gringlobal`.`inventory_name` ADD CONSTRAINT `fk_in_modified` FOREIGN KEY `ndx_fk_in_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_in_ng for table inventory_name ...') as Action;
ALTER TABLE `gringlobal`.`inventory_name` ADD CONSTRAINT `fk_in_ng` FOREIGN KEY `ndx_fk_in_ng` (`name_group_id`) REFERENCES `gringlobal`.`name_group` (`name_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_in_owned for table inventory_name ...') as Action;
ALTER TABLE `gringlobal`.`inventory_name` ADD CONSTRAINT `fk_in_owned` FOREIGN KEY `ndx_fk_in_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for inventory_quality_status **********/
select concat(now(), ' creating constraint fk_iqs_created for table inventory_quality_status ...') as Action;
ALTER TABLE `gringlobal`.`inventory_quality_status` ADD CONSTRAINT `fk_iqs_created` FOREIGN KEY `ndx_fk_iqs_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iqs_cur for table inventory_quality_status ...') as Action;
ALTER TABLE `gringlobal`.`inventory_quality_status` ADD CONSTRAINT `fk_iqs_cur` FOREIGN KEY `ndx_fk_iqs_cur` (`tester_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iqs_i for table inventory_quality_status ...') as Action;
ALTER TABLE `gringlobal`.`inventory_quality_status` ADD CONSTRAINT `fk_iqs_i` FOREIGN KEY `ndx_fk_iqs_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iqs_me for table inventory_quality_status ...') as Action;
ALTER TABLE `gringlobal`.`inventory_quality_status` ADD CONSTRAINT `fk_iqs_me` FOREIGN KEY `ndx_fk_iqs_me` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iqs_modified for table inventory_quality_status ...') as Action;
ALTER TABLE `gringlobal`.`inventory_quality_status` ADD CONSTRAINT `fk_iqs_modified` FOREIGN KEY `ndx_fk_iqs_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iqs_owned for table inventory_quality_status ...') as Action;
ALTER TABLE `gringlobal`.`inventory_quality_status` ADD CONSTRAINT `fk_iqs_owned` FOREIGN KEY `ndx_fk_iqs_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for inventory_viability **********/
select concat(now(), ' creating constraint fk_inventory_viability_inventory_viability for table inventory_viability ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability` ADD CONSTRAINT `fk_inventory_viability_inventory_viability` FOREIGN KEY `ndx_fk_inventory_viability_inventory_viability` (`inventory_viability_id`) REFERENCES `gringlobal`.`inventory_viability` (`inventory_viability_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iv_created for table inventory_viability ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability` ADD CONSTRAINT `fk_iv_created` FOREIGN KEY `ndx_fk_iv_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iv_i for table inventory_viability ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability` ADD CONSTRAINT `fk_iv_i` FOREIGN KEY `ndx_fk_iv_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iv_ivr for table inventory_viability ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability` ADD CONSTRAINT `fk_iv_ivr` FOREIGN KEY `ndx_fk_iv_ivr` (`inventory_viability_rule_id`) REFERENCES `gringlobal`.`inventory_viability_rule` (`inventory_viability_rule_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iv_modified for table inventory_viability ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability` ADD CONSTRAINT `fk_iv_modified` FOREIGN KEY `ndx_fk_iv_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_iv_owned for table inventory_viability ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability` ADD CONSTRAINT `fk_iv_owned` FOREIGN KEY `ndx_fk_iv_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for inventory_viability_data **********/
select concat(now(), ' creating constraint fk_ivd_c for table inventory_viability_data ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_data` ADD CONSTRAINT `fk_ivd_c` FOREIGN KEY `ndx_fk_ivd_c` (`counter_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivd_created for table inventory_viability_data ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_data` ADD CONSTRAINT `fk_ivd_created` FOREIGN KEY `ndx_fk_ivd_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivd_iv for table inventory_viability_data ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_data` ADD CONSTRAINT `fk_ivd_iv` FOREIGN KEY `ndx_fk_ivd_iv` (`inventory_viability_id`) REFERENCES `gringlobal`.`inventory_viability` (`inventory_viability_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivd_modified for table inventory_viability_data ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_data` ADD CONSTRAINT `fk_ivd_modified` FOREIGN KEY `ndx_fk_ivd_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivd_ori for table inventory_viability_data ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_data` ADD CONSTRAINT `fk_ivd_ori` FOREIGN KEY `ndx_fk_ivd_ori` (`order_request_item_id`) REFERENCES `gringlobal`.`order_request_item` (`order_request_item_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivd_owned for table inventory_viability_data ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_data` ADD CONSTRAINT `fk_ivd_owned` FOREIGN KEY `ndx_fk_ivd_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for inventory_viability_rule **********/
select concat(now(), ' creating constraint fk_ivr_created for table inventory_viability_rule ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_rule` ADD CONSTRAINT `fk_ivr_created` FOREIGN KEY `ndx_fk_ivr_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivr_modified for table inventory_viability_rule ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_rule` ADD CONSTRAINT `fk_ivr_modified` FOREIGN KEY `ndx_fk_ivr_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivr_owned for table inventory_viability_rule ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_rule` ADD CONSTRAINT `fk_ivr_owned` FOREIGN KEY `ndx_fk_ivr_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ivr_t for table inventory_viability_rule ...') as Action;
ALTER TABLE `gringlobal`.`inventory_viability_rule` ADD CONSTRAINT `fk_ivr_t` FOREIGN KEY `ndx_fk_ivr_t` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for literature **********/
select concat(now(), ' creating constraint fk_l_created for table literature ...') as Action;
ALTER TABLE `gringlobal`.`literature` ADD CONSTRAINT `fk_l_created` FOREIGN KEY `ndx_fk_l_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_l_modified for table literature ...') as Action;
ALTER TABLE `gringlobal`.`literature` ADD CONSTRAINT `fk_l_modified` FOREIGN KEY `ndx_fk_l_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_l_owned for table literature ...') as Action;
ALTER TABLE `gringlobal`.`literature` ADD CONSTRAINT `fk_l_owned` FOREIGN KEY `ndx_fk_l_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for method **********/
select concat(now(), ' creating constraint fk_m_created for table method ...') as Action;
ALTER TABLE `gringlobal`.`method` ADD CONSTRAINT `fk_m_created` FOREIGN KEY `ndx_fk_m_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_m_g for table method ...') as Action;
ALTER TABLE `gringlobal`.`method` ADD CONSTRAINT `fk_m_g` FOREIGN KEY `ndx_fk_m_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_m_modified for table method ...') as Action;
ALTER TABLE `gringlobal`.`method` ADD CONSTRAINT `fk_m_modified` FOREIGN KEY `ndx_fk_m_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_m_owned for table method ...') as Action;
ALTER TABLE `gringlobal`.`method` ADD CONSTRAINT `fk_m_owned` FOREIGN KEY `ndx_fk_m_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for method_map **********/
select concat(now(), ' creating constraint fk_mm_c for table method_map ...') as Action;
ALTER TABLE `gringlobal`.`method_map` ADD CONSTRAINT `fk_mm_c` FOREIGN KEY `ndx_fk_mm_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_mm_created for table method_map ...') as Action;
ALTER TABLE `gringlobal`.`method_map` ADD CONSTRAINT `fk_mm_created` FOREIGN KEY `ndx_fk_mm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_mm_m for table method_map ...') as Action;
ALTER TABLE `gringlobal`.`method_map` ADD CONSTRAINT `fk_mm_m` FOREIGN KEY `ndx_fk_mm_m` (`method_id`) REFERENCES `gringlobal`.`method` (`method_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_mm_modified for table method_map ...') as Action;
ALTER TABLE `gringlobal`.`method_map` ADD CONSTRAINT `fk_mm_modified` FOREIGN KEY `ndx_fk_mm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_mm_owned for table method_map ...') as Action;
ALTER TABLE `gringlobal`.`method_map` ADD CONSTRAINT `fk_mm_owned` FOREIGN KEY `ndx_fk_mm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for name_group **********/
select concat(now(), ' creating constraint fk_ng_created for table name_group ...') as Action;
ALTER TABLE `gringlobal`.`name_group` ADD CONSTRAINT `fk_ng_created` FOREIGN KEY `ndx_fk_ng_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ng_modified for table name_group ...') as Action;
ALTER TABLE `gringlobal`.`name_group` ADD CONSTRAINT `fk_ng_modified` FOREIGN KEY `ndx_fk_ng_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ng_owned for table name_group ...') as Action;
ALTER TABLE `gringlobal`.`name_group` ADD CONSTRAINT `fk_ng_owned` FOREIGN KEY `ndx_fk_ng_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 8 Constraint Definitions for order_request **********/
select concat(now(), ' creating constraint fk_or_created for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_created` FOREIGN KEY `ndx_fk_or_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_final_c for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_final_c` FOREIGN KEY `ndx_fk_or_final_c` (`final_recipient_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_modified for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_modified` FOREIGN KEY `ndx_fk_or_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_original_or for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_original_or` FOREIGN KEY `ndx_fk_or_original_or` (`original_order_request_id`) REFERENCES `gringlobal`.`order_request` (`order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_owned for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_owned` FOREIGN KEY `ndx_fk_or_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_requestor_c for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_requestor_c` FOREIGN KEY `ndx_fk_or_requestor_c` (`requestor_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_ship_to_c for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_ship_to_c` FOREIGN KEY `ndx_fk_or_ship_to_c` (`ship_to_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_or_wor for table order_request ...') as Action;
ALTER TABLE `gringlobal`.`order_request` ADD CONSTRAINT `fk_or_wor` FOREIGN KEY `ndx_fk_or_wor` (`web_order_request_id`) REFERENCES `gringlobal`.`web_order_request` (`web_order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for order_request_action **********/
select concat(now(), ' creating constraint fk_ora_c for table order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`order_request_action` ADD CONSTRAINT `fk_ora_c` FOREIGN KEY `ndx_fk_ora_c` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ora_created for table order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`order_request_action` ADD CONSTRAINT `fk_ora_created` FOREIGN KEY `ndx_fk_ora_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ora_modified for table order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`order_request_action` ADD CONSTRAINT `fk_ora_modified` FOREIGN KEY `ndx_fk_ora_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ora_or for table order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`order_request_action` ADD CONSTRAINT `fk_ora_or` FOREIGN KEY `ndx_fk_ora_or` (`order_request_id`) REFERENCES `gringlobal`.`order_request` (`order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ora_owned for table order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`order_request_action` ADD CONSTRAINT `fk_ora_owned` FOREIGN KEY `ndx_fk_ora_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for order_request_attach **********/
select concat(now(), ' creating constraint fk_orat_c for table order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`order_request_attach` ADD CONSTRAINT `fk_orat_c` FOREIGN KEY `ndx_fk_orat_c` (`attach_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_orat_created for table order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`order_request_attach` ADD CONSTRAINT `fk_orat_created` FOREIGN KEY `ndx_fk_orat_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_orat_modified for table order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`order_request_attach` ADD CONSTRAINT `fk_orat_modified` FOREIGN KEY `ndx_fk_orat_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_orat_or for table order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`order_request_attach` ADD CONSTRAINT `fk_orat_or` FOREIGN KEY `ndx_fk_orat_or` (`order_request_id`) REFERENCES `gringlobal`.`order_request` (`order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_orat_owned for table order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`order_request_attach` ADD CONSTRAINT `fk_orat_owned` FOREIGN KEY `ndx_fk_orat_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 7 Constraint Definitions for order_request_item **********/
select concat(now(), ' creating constraint fk_ori_created for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_created` FOREIGN KEY `ndx_fk_ori_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ori_i for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_i` FOREIGN KEY `ndx_fk_ori_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ori_modified for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_modified` FOREIGN KEY `ndx_fk_ori_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ori_or for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_or` FOREIGN KEY `ndx_fk_ori_or` (`order_request_id`) REFERENCES `gringlobal`.`order_request` (`order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ori_owned for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_owned` FOREIGN KEY `ndx_fk_ori_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ori_sc for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_sc` FOREIGN KEY `ndx_fk_ori_sc` (`source_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ori_wori for table order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`order_request_item` ADD CONSTRAINT `fk_ori_wori` FOREIGN KEY `ndx_fk_ori_wori` (`web_order_request_item_id`) REFERENCES `gringlobal`.`web_order_request_item` (`web_order_request_item_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for region **********/
select concat(now(), ' creating constraint fk_r_created for table region ...') as Action;
ALTER TABLE `gringlobal`.`region` ADD CONSTRAINT `fk_r_created` FOREIGN KEY `ndx_fk_r_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_r_modified for table region ...') as Action;
ALTER TABLE `gringlobal`.`region` ADD CONSTRAINT `fk_r_modified` FOREIGN KEY `ndx_fk_r_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_r_owned for table region ...') as Action;
ALTER TABLE `gringlobal`.`region` ADD CONSTRAINT `fk_r_owned` FOREIGN KEY `ndx_fk_r_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for region_lang **********/
select concat(now(), ' creating constraint fk_rl_created for table region_lang ...') as Action;
ALTER TABLE `gringlobal`.`region_lang` ADD CONSTRAINT `fk_rl_created` FOREIGN KEY `ndx_fk_rl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_rl_modified for table region_lang ...') as Action;
ALTER TABLE `gringlobal`.`region_lang` ADD CONSTRAINT `fk_rl_modified` FOREIGN KEY `ndx_fk_rl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_rl_owned for table region_lang ...') as Action;
ALTER TABLE `gringlobal`.`region_lang` ADD CONSTRAINT `fk_rl_owned` FOREIGN KEY `ndx_fk_rl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_rl_r for table region_lang ...') as Action;
ALTER TABLE `gringlobal`.`region_lang` ADD CONSTRAINT `fk_rl_r` FOREIGN KEY `ndx_fk_rl_r` (`region_id`) REFERENCES `gringlobal`.`region` (`region_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_rl_sl for table region_lang ...') as Action;
ALTER TABLE `gringlobal`.`region_lang` ADD CONSTRAINT `fk_rl_sl` FOREIGN KEY `ndx_fk_rl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for site **********/
select concat(now(), ' creating constraint fk_s_created for table site ...') as Action;
ALTER TABLE `gringlobal`.`site` ADD CONSTRAINT `fk_s_created` FOREIGN KEY `ndx_fk_s_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_s_modified for table site ...') as Action;
ALTER TABLE `gringlobal`.`site` ADD CONSTRAINT `fk_s_modified` FOREIGN KEY `ndx_fk_s_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_s_owned for table site ...') as Action;
ALTER TABLE `gringlobal`.`site` ADD CONSTRAINT `fk_s_owned` FOREIGN KEY `ndx_fk_s_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for site_inventory_nc7 **********/
select concat(now(), ' creating constraint fk_sin_created for table site_inventory_nc7 ...') as Action;
ALTER TABLE `gringlobal`.`site_inventory_nc7` ADD CONSTRAINT `fk_sin_created` FOREIGN KEY `ndx_fk_sin_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sin_i for table site_inventory_nc7 ...') as Action;
ALTER TABLE `gringlobal`.`site_inventory_nc7` ADD CONSTRAINT `fk_sin_i` FOREIGN KEY `ndx_fk_sin_i` (`inventory_id`) REFERENCES `gringlobal`.`inventory` (`inventory_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sin_modified for table site_inventory_nc7 ...') as Action;
ALTER TABLE `gringlobal`.`site_inventory_nc7` ADD CONSTRAINT `fk_sin_modified` FOREIGN KEY `ndx_fk_sin_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sin_owned for table site_inventory_nc7 ...') as Action;
ALTER TABLE `gringlobal`.`site_inventory_nc7` ADD CONSTRAINT `fk_sin_owned` FOREIGN KEY `ndx_fk_sin_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_database **********/
select concat(now(), ' creating constraint ndx_fk_sdb_created for table sys_database ...') as Action;
ALTER TABLE `gringlobal`.`sys_database` ADD CONSTRAINT `ndx_fk_sdb_created` FOREIGN KEY `ndx_ndx_fk_sdb_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdb_modified for table sys_database ...') as Action;
ALTER TABLE `gringlobal`.`sys_database` ADD CONSTRAINT `ndx_fk_sdb_modified` FOREIGN KEY `ndx_ndx_fk_sdb_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdb_owned for table sys_database ...') as Action;
ALTER TABLE `gringlobal`.`sys_database` ADD CONSTRAINT `ndx_fk_sdb_owned` FOREIGN KEY `ndx_ndx_fk_sdb_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_database_migration **********/
select concat(now(), ' creating constraint ndx_fk_sdbm_created for table sys_database_migration ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration` ADD CONSTRAINT `ndx_fk_sdbm_created` FOREIGN KEY `ndx_ndx_fk_sdbm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdbm_modified for table sys_database_migration ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration` ADD CONSTRAINT `ndx_fk_sdbm_modified` FOREIGN KEY `ndx_ndx_fk_sdbm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdbm_owned for table sys_database_migration ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration` ADD CONSTRAINT `ndx_fk_sdbm_owned` FOREIGN KEY `ndx_ndx_fk_sdbm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for sys_database_migration_lang **********/
select concat(now(), ' creating constraint fk_sdbml_sdbm for table sys_database_migration_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration_lang` ADD CONSTRAINT `fk_sdbml_sdbm` FOREIGN KEY `ndx_fk_sdbml_sdbm` (`sys_database_migration_id`) REFERENCES `gringlobal`.`sys_database_migration` (`sys_database_migration_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdbml_created for table sys_database_migration_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration_lang` ADD CONSTRAINT `ndx_fk_sdbml_created` FOREIGN KEY `ndx_ndx_fk_sdbml_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdbml_modified for table sys_database_migration_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration_lang` ADD CONSTRAINT `ndx_fk_sdbml_modified` FOREIGN KEY `ndx_ndx_fk_sdbml_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sdbml_owned for table sys_database_migration_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_database_migration_lang` ADD CONSTRAINT `ndx_fk_sdbml_owned` FOREIGN KEY `ndx_ndx_fk_sdbml_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_datatrigger **********/
select concat(now(), ' creating constraint fk_sdt_created for table sys_datatrigger ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger` ADD CONSTRAINT `fk_sdt_created` FOREIGN KEY `ndx_fk_sdt_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdt_dv for table sys_datatrigger ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger` ADD CONSTRAINT `fk_sdt_dv` FOREIGN KEY `ndx_fk_sdt_dv` (`sys_dataview_id`) REFERENCES `gringlobal`.`sys_dataview` (`sys_dataview_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdt_modified for table sys_datatrigger ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger` ADD CONSTRAINT `fk_sdt_modified` FOREIGN KEY `ndx_fk_sdt_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdt_owned for table sys_datatrigger ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger` ADD CONSTRAINT `fk_sdt_owned` FOREIGN KEY `ndx_fk_sdt_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdt_st for table sys_datatrigger ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger` ADD CONSTRAINT `fk_sdt_st` FOREIGN KEY `ndx_fk_sdt_st` (`sys_table_id`) REFERENCES `gringlobal`.`sys_table` (`sys_table_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_datatrigger_lang **********/
select concat(now(), ' creating constraint fk_sdtl_created for table sys_datatrigger_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger_lang` ADD CONSTRAINT `fk_sdtl_created` FOREIGN KEY `ndx_fk_sdtl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdtl_modified for table sys_datatrigger_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger_lang` ADD CONSTRAINT `fk_sdtl_modified` FOREIGN KEY `ndx_fk_sdtl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdtl_owned for table sys_datatrigger_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger_lang` ADD CONSTRAINT `fk_sdtl_owned` FOREIGN KEY `ndx_fk_sdtl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdtl_sdt for table sys_datatrigger_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger_lang` ADD CONSTRAINT `fk_sdtl_sdt` FOREIGN KEY `ndx_fk_sdtl_sdt` (`sys_datatrigger_id`) REFERENCES `gringlobal`.`sys_datatrigger` (`sys_datatrigger_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdtl_sl for table sys_datatrigger_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_datatrigger_lang` ADD CONSTRAINT `fk_sdtl_sl` FOREIGN KEY `ndx_fk_sdtl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_dataview **********/
select concat(now(), ' creating constraint fk_sr_created for table sys_dataview ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview` ADD CONSTRAINT `fk_sr_created` FOREIGN KEY `ndx_fk_sr_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sr_modified for table sys_dataview ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview` ADD CONSTRAINT `fk_sr_modified` FOREIGN KEY `ndx_fk_sr_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sr_owned for table sys_dataview ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview` ADD CONSTRAINT `fk_sr_owned` FOREIGN KEY `ndx_fk_sr_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_dataview_field **********/
select concat(now(), ' creating constraint fk_srf_created for table sys_dataview_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field` ADD CONSTRAINT `fk_srf_created` FOREIGN KEY `ndx_fk_srf_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srf_modified for table sys_dataview_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field` ADD CONSTRAINT `fk_srf_modified` FOREIGN KEY `ndx_fk_srf_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srf_owned for table sys_dataview_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field` ADD CONSTRAINT `fk_srf_owned` FOREIGN KEY `ndx_fk_srf_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srf_sr for table sys_dataview_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field` ADD CONSTRAINT `fk_srf_sr` FOREIGN KEY `ndx_fk_srf_sr` (`sys_dataview_id`) REFERENCES `gringlobal`.`sys_dataview` (`sys_dataview_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srf_stf for table sys_dataview_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field` ADD CONSTRAINT `fk_srf_stf` FOREIGN KEY `ndx_fk_srf_stf` (`sys_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_dataview_field_lang **********/
select concat(now(), ' creating constraint fk_srfl_created for table sys_dataview_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field_lang` ADD CONSTRAINT `fk_srfl_created` FOREIGN KEY `ndx_fk_srfl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srfl_modified for table sys_dataview_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field_lang` ADD CONSTRAINT `fk_srfl_modified` FOREIGN KEY `ndx_fk_srfl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srfl_owned for table sys_dataview_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field_lang` ADD CONSTRAINT `fk_srfl_owned` FOREIGN KEY `ndx_fk_srfl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srfl_sl for table sys_dataview_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field_lang` ADD CONSTRAINT `fk_srfl_sl` FOREIGN KEY `ndx_fk_srfl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srfl_srf for table sys_dataview_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_field_lang` ADD CONSTRAINT `fk_srfl_srf` FOREIGN KEY `ndx_fk_srfl_srf` (`sys_dataview_field_id`) REFERENCES `gringlobal`.`sys_dataview_field` (`sys_dataview_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_dataview_lang **********/
select concat(now(), ' creating constraint fk_sdl_created for table sys_dataview_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_lang` ADD CONSTRAINT `fk_sdl_created` FOREIGN KEY `ndx_fk_sdl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdl_modified for table sys_dataview_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_lang` ADD CONSTRAINT `fk_sdl_modified` FOREIGN KEY `ndx_fk_sdl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdl_owned for table sys_dataview_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_lang` ADD CONSTRAINT `fk_sdl_owned` FOREIGN KEY `ndx_fk_sdl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdl_sd for table sys_dataview_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_lang` ADD CONSTRAINT `fk_sdl_sd` FOREIGN KEY `ndx_fk_sdl_sd` (`sys_dataview_id`) REFERENCES `gringlobal`.`sys_dataview` (`sys_dataview_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sdl_sl for table sys_dataview_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_lang` ADD CONSTRAINT `fk_sdl_sl` FOREIGN KEY `ndx_fk_sdl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for sys_dataview_param **********/
select concat(now(), ' creating constraint fk_srp_created for table sys_dataview_param ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_param` ADD CONSTRAINT `fk_srp_created` FOREIGN KEY `ndx_fk_srp_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srp_modified for table sys_dataview_param ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_param` ADD CONSTRAINT `fk_srp_modified` FOREIGN KEY `ndx_fk_srp_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srp_owned for table sys_dataview_param ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_param` ADD CONSTRAINT `fk_srp_owned` FOREIGN KEY `ndx_fk_srp_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srp_sr for table sys_dataview_param ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_param` ADD CONSTRAINT `fk_srp_sr` FOREIGN KEY `ndx_fk_srp_sr` (`sys_dataview_id`) REFERENCES `gringlobal`.`sys_dataview` (`sys_dataview_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for sys_dataview_sql **********/
select concat(now(), ' creating constraint fk_srs_createdby for table sys_dataview_sql ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_sql` ADD CONSTRAINT `fk_srs_createdby` FOREIGN KEY `ndx_fk_srs_createdby` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srs_modifiedby for table sys_dataview_sql ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_sql` ADD CONSTRAINT `fk_srs_modifiedby` FOREIGN KEY `ndx_fk_srs_modifiedby` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srs_ownedby for table sys_dataview_sql ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_sql` ADD CONSTRAINT `fk_srs_ownedby` FOREIGN KEY `ndx_fk_srs_ownedby` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_srs_sr for table sys_dataview_sql ...') as Action;
ALTER TABLE `gringlobal`.`sys_dataview_sql` ADD CONSTRAINT `fk_srs_sr` FOREIGN KEY `ndx_fk_srs_sr` (`sys_dataview_id`) REFERENCES `gringlobal`.`sys_dataview` (`sys_dataview_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_file **********/
select concat(now(), ' creating constraint ndx_fk_sf_created for table sys_file ...') as Action;
ALTER TABLE `gringlobal`.`sys_file` ADD CONSTRAINT `ndx_fk_sf_created` FOREIGN KEY `ndx_ndx_fk_sf_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sf_modified for table sys_file ...') as Action;
ALTER TABLE `gringlobal`.`sys_file` ADD CONSTRAINT `ndx_fk_sf_modified` FOREIGN KEY `ndx_ndx_fk_sf_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sf_owned for table sys_file ...') as Action;
ALTER TABLE `gringlobal`.`sys_file` ADD CONSTRAINT `ndx_fk_sf_owned` FOREIGN KEY `ndx_ndx_fk_sf_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_file_group **********/
select concat(now(), ' creating constraint ndx_fk_sfg_created for table sys_file_group ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group` ADD CONSTRAINT `ndx_fk_sfg_created` FOREIGN KEY `ndx_ndx_fk_sfg_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfg_modified for table sys_file_group ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group` ADD CONSTRAINT `ndx_fk_sfg_modified` FOREIGN KEY `ndx_ndx_fk_sfg_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfg_owned for table sys_file_group ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group` ADD CONSTRAINT `ndx_fk_sfg_owned` FOREIGN KEY `ndx_ndx_fk_sfg_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_file_group_map **********/
select concat(now(), ' creating constraint fk_sfgm_sf for table sys_file_group_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group_map` ADD CONSTRAINT `fk_sfgm_sf` FOREIGN KEY `ndx_fk_sfgm_sf` (`sys_file_id`) REFERENCES `gringlobal`.`sys_file` (`sys_file_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sfgm_sfg for table sys_file_group_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group_map` ADD CONSTRAINT `fk_sfgm_sfg` FOREIGN KEY `ndx_fk_sfgm_sfg` (`sys_file_group_id`) REFERENCES `gringlobal`.`sys_file_group` (`sys_file_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfgm_created for table sys_file_group_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group_map` ADD CONSTRAINT `ndx_fk_sfgm_created` FOREIGN KEY `ndx_ndx_fk_sfgm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfgm_modified for table sys_file_group_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group_map` ADD CONSTRAINT `ndx_fk_sfgm_modified` FOREIGN KEY `ndx_ndx_fk_sfgm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfgm_owned for table sys_file_group_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_group_map` ADD CONSTRAINT `ndx_fk_sfgm_owned` FOREIGN KEY `ndx_ndx_fk_sfgm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_file_lang **********/
select concat(now(), ' creating constraint fk_sec_file_lang_sec_file for table sys_file_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_lang` ADD CONSTRAINT `fk_sec_file_lang_sec_file` FOREIGN KEY `ndx_fk_sec_file_lang_sec_file` (`sys_file_id`) REFERENCES `gringlobal`.`sys_file` (`sys_file_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sfl_sf for table sys_file_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_lang` ADD CONSTRAINT `fk_sfl_sf` FOREIGN KEY `ndx_fk_sfl_sf` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfl_created for table sys_file_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_lang` ADD CONSTRAINT `ndx_fk_sfl_created` FOREIGN KEY `ndx_ndx_fk_sfl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfl_modified for table sys_file_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_lang` ADD CONSTRAINT `ndx_fk_sfl_modified` FOREIGN KEY `ndx_ndx_fk_sfl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sfl_owned for table sys_file_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_file_lang` ADD CONSTRAINT `ndx_fk_sfl_owned` FOREIGN KEY `ndx_ndx_fk_sfl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_group **********/
select concat(now(), ' creating constraint ndx_fk_sg_created for table sys_group ...') as Action;
ALTER TABLE `gringlobal`.`sys_group` ADD CONSTRAINT `ndx_fk_sg_created` FOREIGN KEY `ndx_ndx_fk_sg_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sg_modified for table sys_group ...') as Action;
ALTER TABLE `gringlobal`.`sys_group` ADD CONSTRAINT `ndx_fk_sg_modified` FOREIGN KEY `ndx_ndx_fk_sg_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sg_owned for table sys_group ...') as Action;
ALTER TABLE `gringlobal`.`sys_group` ADD CONSTRAINT `ndx_fk_sg_owned` FOREIGN KEY `ndx_ndx_fk_sg_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_group_lang **********/
select concat(now(), ' creating constraint fk_sgl_sg for table sys_group_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_lang` ADD CONSTRAINT `fk_sgl_sg` FOREIGN KEY `ndx_fk_sgl_sg` (`sys_group_id`) REFERENCES `gringlobal`.`sys_group` (`sys_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sgl_sl for table sys_group_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_lang` ADD CONSTRAINT `fk_sgl_sl` FOREIGN KEY `ndx_fk_sgl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgl_created for table sys_group_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_lang` ADD CONSTRAINT `ndx_fk_sgl_created` FOREIGN KEY `ndx_ndx_fk_sgl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgl_modified for table sys_group_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_lang` ADD CONSTRAINT `ndx_fk_sgl_modified` FOREIGN KEY `ndx_ndx_fk_sgl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgl_owned for table sys_group_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_lang` ADD CONSTRAINT `ndx_fk_sgl_owned` FOREIGN KEY `ndx_ndx_fk_sgl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_group_permission_map **********/
select concat(now(), ' creating constraint fk_sgpm_sg for table sys_group_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_permission_map` ADD CONSTRAINT `fk_sgpm_sg` FOREIGN KEY `ndx_fk_sgpm_sg` (`sys_group_id`) REFERENCES `gringlobal`.`sys_group` (`sys_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sgpm_sp for table sys_group_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_permission_map` ADD CONSTRAINT `fk_sgpm_sp` FOREIGN KEY `ndx_fk_sgpm_sp` (`sys_permission_id`) REFERENCES `gringlobal`.`sys_permission` (`sys_permission_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgpm_created for table sys_group_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_permission_map` ADD CONSTRAINT `ndx_fk_sgpm_created` FOREIGN KEY `ndx_ndx_fk_sgpm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgpm_modified for table sys_group_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_permission_map` ADD CONSTRAINT `ndx_fk_sgpm_modified` FOREIGN KEY `ndx_ndx_fk_sgpm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgpm_owned for table sys_group_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_permission_map` ADD CONSTRAINT `ndx_fk_sgpm_owned` FOREIGN KEY `ndx_ndx_fk_sgpm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_group_user_map **********/
select concat(now(), ' creating constraint fk_sgum_sg for table sys_group_user_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_user_map` ADD CONSTRAINT `fk_sgum_sg` FOREIGN KEY `ndx_fk_sgum_sg` (`sys_group_id`) REFERENCES `gringlobal`.`sys_group` (`sys_group_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sgum_su for table sys_group_user_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_user_map` ADD CONSTRAINT `fk_sgum_su` FOREIGN KEY `ndx_fk_sgum_su` (`sys_user_id`) REFERENCES `gringlobal`.`sys_user` (`sys_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgum_created for table sys_group_user_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_user_map` ADD CONSTRAINT `ndx_fk_sgum_created` FOREIGN KEY `ndx_ndx_fk_sgum_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgum_modified for table sys_group_user_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_user_map` ADD CONSTRAINT `ndx_fk_sgum_modified` FOREIGN KEY `ndx_ndx_fk_sgum_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_sgum_owned for table sys_group_user_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_group_user_map` ADD CONSTRAINT `ndx_fk_sgum_owned` FOREIGN KEY `ndx_ndx_fk_sgum_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for sys_index **********/
select concat(now(), ' creating constraint fk_si_created for table sys_index ...') as Action;
ALTER TABLE `gringlobal`.`sys_index` ADD CONSTRAINT `fk_si_created` FOREIGN KEY `ndx_fk_si_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_si_modified for table sys_index ...') as Action;
ALTER TABLE `gringlobal`.`sys_index` ADD CONSTRAINT `fk_si_modified` FOREIGN KEY `ndx_fk_si_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_si_owned for table sys_index ...') as Action;
ALTER TABLE `gringlobal`.`sys_index` ADD CONSTRAINT `fk_si_owned` FOREIGN KEY `ndx_fk_si_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_si_st for table sys_index ...') as Action;
ALTER TABLE `gringlobal`.`sys_index` ADD CONSTRAINT `fk_si_st` FOREIGN KEY `ndx_fk_si_st` (`sys_table_id`) REFERENCES `gringlobal`.`sys_table` (`sys_table_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_index_field **********/
select concat(now(), ' creating constraint fk_sif_created for table sys_index_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_index_field` ADD CONSTRAINT `fk_sif_created` FOREIGN KEY `ndx_fk_sif_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sif_modified for table sys_index_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_index_field` ADD CONSTRAINT `fk_sif_modified` FOREIGN KEY `ndx_fk_sif_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sif_owned for table sys_index_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_index_field` ADD CONSTRAINT `fk_sif_owned` FOREIGN KEY `ndx_fk_sif_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sif_si for table sys_index_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_index_field` ADD CONSTRAINT `fk_sif_si` FOREIGN KEY `ndx_fk_sif_si` (`sys_index_id`) REFERENCES `gringlobal`.`sys_index` (`sys_index_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sif_stf for table sys_index_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_index_field` ADD CONSTRAINT `fk_sif_stf` FOREIGN KEY `ndx_fk_sif_stf` (`sys_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_lang **********/
select concat(now(), ' creating constraint fk_sl_created for table sys_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_lang` ADD CONSTRAINT `fk_sl_created` FOREIGN KEY `ndx_fk_sl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sl_modified for table sys_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_lang` ADD CONSTRAINT `fk_sl_modified` FOREIGN KEY `ndx_fk_sl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sl_owned for table sys_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_lang` ADD CONSTRAINT `fk_sl_owned` FOREIGN KEY `ndx_fk_sl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_permission **********/
select concat(now(), ' creating constraint fk_sp_created for table sys_permission ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission` ADD CONSTRAINT `fk_sp_created` FOREIGN KEY `ndx_fk_sp_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sp_modified for table sys_permission ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission` ADD CONSTRAINT `fk_sp_modified` FOREIGN KEY `ndx_fk_sp_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sp_owned for table sys_permission ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission` ADD CONSTRAINT `fk_sp_owned` FOREIGN KEY `ndx_fk_sp_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sp_sr for table sys_permission ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission` ADD CONSTRAINT `fk_sp_sr` FOREIGN KEY `ndx_fk_sp_sr` (`sys_dataview_id`) REFERENCES `gringlobal`.`sys_dataview` (`sys_dataview_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sp_st for table sys_permission ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission` ADD CONSTRAINT `fk_sp_st` FOREIGN KEY `ndx_fk_sp_st` (`sys_table_id`) REFERENCES `gringlobal`.`sys_table` (`sys_table_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 7 Constraint Definitions for sys_permission_field **********/
select concat(now(), ' creating constraint fk_sp_srf for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_sp_srf` FOREIGN KEY `ndx_fk_sp_srf` (`sys_dataview_field_id`) REFERENCES `gringlobal`.`sys_dataview_field` (`sys_dataview_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sp_stf for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_sp_stf` FOREIGN KEY `ndx_fk_sp_stf` (`sys_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_spf_created for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_spf_created` FOREIGN KEY `ndx_fk_spf_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_spf_modified for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_spf_modified` FOREIGN KEY `ndx_fk_spf_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_spf_owned for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_spf_owned` FOREIGN KEY `ndx_fk_spf_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_spf_sp for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_spf_sp` FOREIGN KEY `ndx_fk_spf_sp` (`sys_permission_id`) REFERENCES `gringlobal`.`sys_permission` (`sys_permission_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_spf_stf for table sys_permission_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_field` ADD CONSTRAINT `fk_spf_stf` FOREIGN KEY `ndx_fk_spf_stf` (`parent_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_permission_lang **********/
select concat(now(), ' creating constraint fk_spl_sl for table sys_permission_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_lang` ADD CONSTRAINT `fk_spl_sl` FOREIGN KEY `ndx_fk_spl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_spl_sp for table sys_permission_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_lang` ADD CONSTRAINT `fk_spl_sp` FOREIGN KEY `ndx_fk_spl_sp` (`sys_permission_id`) REFERENCES `gringlobal`.`sys_permission` (`sys_permission_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_spl_created for table sys_permission_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_lang` ADD CONSTRAINT `ndx_fk_spl_created` FOREIGN KEY `ndx_ndx_fk_spl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_spl_modified for table sys_permission_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_lang` ADD CONSTRAINT `ndx_fk_spl_modified` FOREIGN KEY `ndx_ndx_fk_spl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint ndx_fk_spl_owned for table sys_permission_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_permission_lang` ADD CONSTRAINT `ndx_fk_spl_owned` FOREIGN KEY `ndx_ndx_fk_spl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for sys_table **********/
select concat(now(), ' creating constraint fk_st_created for table sys_table ...') as Action;
ALTER TABLE `gringlobal`.`sys_table` ADD CONSTRAINT `fk_st_created` FOREIGN KEY `ndx_fk_st_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_st_modified for table sys_table ...') as Action;
ALTER TABLE `gringlobal`.`sys_table` ADD CONSTRAINT `fk_st_modified` FOREIGN KEY `ndx_fk_st_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_st_owned for table sys_table ...') as Action;
ALTER TABLE `gringlobal`.`sys_table` ADD CONSTRAINT `fk_st_owned` FOREIGN KEY `ndx_fk_st_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_table_field **********/
select concat(now(), ' creating constraint fk_stf_created for table sys_table_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field` ADD CONSTRAINT `fk_stf_created` FOREIGN KEY `ndx_fk_stf_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stf_modified for table sys_table_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field` ADD CONSTRAINT `fk_stf_modified` FOREIGN KEY `ndx_fk_stf_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stf_owned for table sys_table_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field` ADD CONSTRAINT `fk_stf_owned` FOREIGN KEY `ndx_fk_stf_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stf_st for table sys_table_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field` ADD CONSTRAINT `fk_stf_st` FOREIGN KEY `ndx_fk_stf_st` (`sys_table_id`) REFERENCES `gringlobal`.`sys_table` (`sys_table_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stf_stffk for table sys_table_field ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field` ADD CONSTRAINT `fk_stf_stffk` FOREIGN KEY `ndx_fk_stf_stffk` (`foreign_key_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_table_field_lang **********/
select concat(now(), ' creating constraint fk_stfl_created for table sys_table_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field_lang` ADD CONSTRAINT `fk_stfl_created` FOREIGN KEY `ndx_fk_stfl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stfl_modified for table sys_table_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field_lang` ADD CONSTRAINT `fk_stfl_modified` FOREIGN KEY `ndx_fk_stfl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stfl_owned for table sys_table_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field_lang` ADD CONSTRAINT `fk_stfl_owned` FOREIGN KEY `ndx_fk_stfl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stfl_sl for table sys_table_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field_lang` ADD CONSTRAINT `fk_stfl_sl` FOREIGN KEY `ndx_fk_stfl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stfl_stf for table sys_table_field_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_field_lang` ADD CONSTRAINT `fk_stfl_stf` FOREIGN KEY `ndx_fk_stfl_stf` (`sys_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_table_lang **********/
select concat(now(), ' creating constraint fk_stl_created for table sys_table_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_lang` ADD CONSTRAINT `fk_stl_created` FOREIGN KEY `ndx_fk_stl_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stl_modified for table sys_table_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_lang` ADD CONSTRAINT `fk_stl_modified` FOREIGN KEY `ndx_fk_stl_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stl_owned for table sys_table_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_lang` ADD CONSTRAINT `fk_stl_owned` FOREIGN KEY `ndx_fk_stl_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stl_sl for table sys_table_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_lang` ADD CONSTRAINT `fk_stl_sl` FOREIGN KEY `ndx_fk_stl_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_stl_st for table sys_table_lang ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_lang` ADD CONSTRAINT `fk_stl_st` FOREIGN KEY `ndx_fk_stl_st` (`sys_table_id`) REFERENCES `gringlobal`.`sys_table` (`sys_table_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_table_relationship **********/
select concat(now(), ' creating constraint fk_str_created for table sys_table_relationship ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_relationship` ADD CONSTRAINT `fk_str_created` FOREIGN KEY `ndx_fk_str_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_str_modified for table sys_table_relationship ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_relationship` ADD CONSTRAINT `fk_str_modified` FOREIGN KEY `ndx_fk_str_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_str_owned for table sys_table_relationship ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_relationship` ADD CONSTRAINT `fk_str_owned` FOREIGN KEY `ndx_fk_str_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_str_stf for table sys_table_relationship ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_relationship` ADD CONSTRAINT `fk_str_stf` FOREIGN KEY `ndx_fk_str_stf` (`sys_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_str_stf_other for table sys_table_relationship ...') as Action;
ALTER TABLE `gringlobal`.`sys_table_relationship` ADD CONSTRAINT `fk_str_stf_other` FOREIGN KEY `ndx_fk_str_stf_other` (`other_table_field_id`) REFERENCES `gringlobal`.`sys_table_field` (`sys_table_field_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for sys_user **********/
select concat(now(), ' creating constraint fk_su_co for table sys_user ...') as Action;
ALTER TABLE `gringlobal`.`sys_user` ADD CONSTRAINT `fk_su_co` FOREIGN KEY `ndx_fk_su_co` (`cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_su_created for table sys_user ...') as Action;
ALTER TABLE `gringlobal`.`sys_user` ADD CONSTRAINT `fk_su_created` FOREIGN KEY `ndx_fk_su_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_su_modified for table sys_user ...') as Action;
ALTER TABLE `gringlobal`.`sys_user` ADD CONSTRAINT `fk_su_modified` FOREIGN KEY `ndx_fk_su_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_su_owned for table sys_user ...') as Action;
ALTER TABLE `gringlobal`.`sys_user` ADD CONSTRAINT `fk_su_owned` FOREIGN KEY `ndx_fk_su_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for sys_user_permission_map **********/
select concat(now(), ' creating constraint fk_sup_created for table sys_user_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_user_permission_map` ADD CONSTRAINT `fk_sup_created` FOREIGN KEY `ndx_fk_sup_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sup_modified for table sys_user_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_user_permission_map` ADD CONSTRAINT `fk_sup_modified` FOREIGN KEY `ndx_fk_sup_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sup_owned for table sys_user_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_user_permission_map` ADD CONSTRAINT `fk_sup_owned` FOREIGN KEY `ndx_fk_sup_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sup_sp for table sys_user_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_user_permission_map` ADD CONSTRAINT `fk_sup_sp` FOREIGN KEY `ndx_fk_sup_sp` (`sys_permission_id`) REFERENCES `gringlobal`.`sys_permission` (`sys_permission_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_sup_su for table sys_user_permission_map ...') as Action;
ALTER TABLE `gringlobal`.`sys_user_permission_map` ADD CONSTRAINT `fk_sup_su` FOREIGN KEY `ndx_fk_sup_su` (`sys_user_id`) REFERENCES `gringlobal`.`sys_user` (`sys_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_alt_family_map **********/
select concat(now(), ' creating constraint fk_tafm_created for table taxonomy_alt_family_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_alt_family_map` ADD CONSTRAINT `fk_tafm_created` FOREIGN KEY `ndx_fk_tafm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tafm_f for table taxonomy_alt_family_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_alt_family_map` ADD CONSTRAINT `fk_tafm_f` FOREIGN KEY `ndx_fk_tafm_f` (`taxonomy_family_id`) REFERENCES `gringlobal`.`taxonomy_family` (`taxonomy_family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tafm_g for table taxonomy_alt_family_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_alt_family_map` ADD CONSTRAINT `fk_tafm_g` FOREIGN KEY `ndx_fk_tafm_g` (`taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tafm_modified for table taxonomy_alt_family_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_alt_family_map` ADD CONSTRAINT `fk_tafm_modified` FOREIGN KEY `ndx_fk_tafm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tafm_owned for table taxonomy_alt_family_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_alt_family_map` ADD CONSTRAINT `fk_tafm_owned` FOREIGN KEY `ndx_fk_tafm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 7 Constraint Definitions for taxonomy_attach **********/
select concat(now(), ' creating constraint fk_tat_c for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_c` FOREIGN KEY `ndx_fk_tat_c` (`attach_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tat_created for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_created` FOREIGN KEY `ndx_fk_tat_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tat_modified for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_modified` FOREIGN KEY `ndx_fk_tat_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tat_owned for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_owned` FOREIGN KEY `ndx_fk_tat_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tat_tf for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_tf` FOREIGN KEY `ndx_fk_tat_tf` (`taxonomy_family_id`) REFERENCES `gringlobal`.`taxonomy_family` (`taxonomy_family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tat_tg for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_tg` FOREIGN KEY `ndx_fk_tat_tg` (`taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tat_ts for table taxonomy_attach ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_attach` ADD CONSTRAINT `fk_tat_ts` FOREIGN KEY `ndx_fk_tat_ts` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 3 Constraint Definitions for taxonomy_author **********/
select concat(now(), ' creating constraint fk_ta_created for table taxonomy_author ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_author` ADD CONSTRAINT `fk_ta_created` FOREIGN KEY `ndx_fk_ta_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ta_modified for table taxonomy_author ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_author` ADD CONSTRAINT `fk_ta_modified` FOREIGN KEY `ndx_fk_ta_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ta_owned for table taxonomy_author ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_author` ADD CONSTRAINT `fk_ta_owned` FOREIGN KEY `ndx_fk_ta_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_common_name **********/
select concat(now(), ' creating constraint fk_tcn_created for table taxonomy_common_name ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_common_name` ADD CONSTRAINT `fk_tcn_created` FOREIGN KEY `ndx_fk_tcn_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcn_modified for table taxonomy_common_name ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_common_name` ADD CONSTRAINT `fk_tcn_modified` FOREIGN KEY `ndx_fk_tcn_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcn_owned for table taxonomy_common_name ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_common_name` ADD CONSTRAINT `fk_tcn_owned` FOREIGN KEY `ndx_fk_tcn_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcn_tg for table taxonomy_common_name ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_common_name` ADD CONSTRAINT `fk_tcn_tg` FOREIGN KEY `ndx_fk_tcn_tg` (`taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcn_ts for table taxonomy_common_name ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_common_name` ADD CONSTRAINT `fk_tcn_ts` FOREIGN KEY `ndx_fk_tcn_ts` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_crop_map **********/
select concat(now(), ' creating constraint fc_tcm_cr for table taxonomy_crop_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_crop_map` ADD CONSTRAINT `fc_tcm_cr` FOREIGN KEY `ndx_fc_tcm_cr` (`crop_id`) REFERENCES `gringlobal`.`crop` (`crop_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcm_created for table taxonomy_crop_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_crop_map` ADD CONSTRAINT `fk_tcm_created` FOREIGN KEY `ndx_fk_tcm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcm_modified for table taxonomy_crop_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_crop_map` ADD CONSTRAINT `fk_tcm_modified` FOREIGN KEY `ndx_fk_tcm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcm_owned for table taxonomy_crop_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_crop_map` ADD CONSTRAINT `fk_tcm_owned` FOREIGN KEY `ndx_fk_tcm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tcm_ts for table taxonomy_crop_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_crop_map` ADD CONSTRAINT `fk_tcm_ts` FOREIGN KEY `ndx_fk_tcm_ts` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_family **********/
select concat(now(), ' creating constraint fk_tf_created for table taxonomy_family ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_family` ADD CONSTRAINT `fk_tf_created` FOREIGN KEY `ndx_fk_tf_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tf_cur_tf for table taxonomy_family ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_family` ADD CONSTRAINT `fk_tf_cur_tf` FOREIGN KEY `ndx_fk_tf_cur_tf` (`current_taxonomy_family_id`) REFERENCES `gringlobal`.`taxonomy_family` (`taxonomy_family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tf_modified for table taxonomy_family ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_family` ADD CONSTRAINT `fk_tf_modified` FOREIGN KEY `ndx_fk_tf_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tf_owned for table taxonomy_family ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_family` ADD CONSTRAINT `fk_tf_owned` FOREIGN KEY `ndx_fk_tf_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tf_tg for table taxonomy_family ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_family` ADD CONSTRAINT `fk_tf_tg` FOREIGN KEY `ndx_fk_tf_tg` (`type_taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_genus **********/
select concat(now(), ' creating constraint fk_tg_created for table taxonomy_genus ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_genus` ADD CONSTRAINT `fk_tg_created` FOREIGN KEY `ndx_fk_tg_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tg_cur_tgt for table taxonomy_genus ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_genus` ADD CONSTRAINT `fk_tg_cur_tgt` FOREIGN KEY `ndx_fk_tg_cur_tgt` (`current_taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tg_modified for table taxonomy_genus ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_genus` ADD CONSTRAINT `fk_tg_modified` FOREIGN KEY `ndx_fk_tg_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tg_owned for table taxonomy_genus ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_genus` ADD CONSTRAINT `fk_tg_owned` FOREIGN KEY `ndx_fk_tg_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tg_tf for table taxonomy_genus ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_genus` ADD CONSTRAINT `fk_tg_tf` FOREIGN KEY `ndx_fk_tg_tf` (`taxonomy_family_id`) REFERENCES `gringlobal`.`taxonomy_family` (`taxonomy_family_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_geography_map **********/
select concat(now(), ' creating constraint fk_tgm_created for table taxonomy_geography_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_geography_map` ADD CONSTRAINT `fk_tgm_created` FOREIGN KEY `ndx_fk_tgm_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tgm_g for table taxonomy_geography_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_geography_map` ADD CONSTRAINT `fk_tgm_g` FOREIGN KEY `ndx_fk_tgm_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tgm_modified for table taxonomy_geography_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_geography_map` ADD CONSTRAINT `fk_tgm_modified` FOREIGN KEY `ndx_fk_tgm_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tgm_owned for table taxonomy_geography_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_geography_map` ADD CONSTRAINT `fk_tgm_owned` FOREIGN KEY `ndx_fk_tgm_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tgm_ts for table taxonomy_geography_map ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_geography_map` ADD CONSTRAINT `fk_tgm_ts` FOREIGN KEY `ndx_fk_tgm_ts` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for taxonomy_noxious **********/
select concat(now(), ' creating constraint fk_tn_created for table taxonomy_noxious ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_noxious` ADD CONSTRAINT `fk_tn_created` FOREIGN KEY `ndx_fk_tn_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tn_g for table taxonomy_noxious ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_noxious` ADD CONSTRAINT `fk_tn_g` FOREIGN KEY `ndx_fk_tn_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tn_modified for table taxonomy_noxious ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_noxious` ADD CONSTRAINT `fk_tn_modified` FOREIGN KEY `ndx_fk_tn_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tn_owned for table taxonomy_noxious ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_noxious` ADD CONSTRAINT `fk_tn_owned` FOREIGN KEY `ndx_fk_tn_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tn_ts for table taxonomy_noxious ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_noxious` ADD CONSTRAINT `fk_tn_ts` FOREIGN KEY `ndx_fk_tn_ts` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 10 Constraint Definitions for taxonomy_species **********/
select concat(now(), ' creating constraint fk_ts_c for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_c` FOREIGN KEY `ndx_fk_ts_c` (`verifier_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_created for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_created` FOREIGN KEY `ndx_fk_ts_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_cur_t for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_cur_t` FOREIGN KEY `ndx_fk_ts_cur_t` (`current_taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_curator1 for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_curator1` FOREIGN KEY `ndx_fk_ts_curator1` (`curator1_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_curator2 for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_curator2` FOREIGN KEY `ndx_fk_ts_curator2` (`curator2_cooperator_id`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_modified for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_modified` FOREIGN KEY `ndx_fk_ts_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_owned for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_owned` FOREIGN KEY `ndx_fk_ts_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_s for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_s` FOREIGN KEY `ndx_fk_ts_s` (`priority1_site_id`) REFERENCES `gringlobal`.`site` (`site_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_s2 for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_s2` FOREIGN KEY `ndx_fk_ts_s2` (`priority2_site_id`) REFERENCES `gringlobal`.`site` (`site_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_ts_tg for table taxonomy_species ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_species` ADD CONSTRAINT `fk_ts_tg` FOREIGN KEY `ndx_fk_ts_tg` (`taxonomy_genus_id`) REFERENCES `gringlobal`.`taxonomy_genus` (`taxonomy_genus_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for taxonomy_use **********/
select concat(now(), ' creating constraint fk_tus_created for table taxonomy_use ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_use` ADD CONSTRAINT `fk_tus_created` FOREIGN KEY `ndx_fk_tus_created` (`created_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tus_modified for table taxonomy_use ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_use` ADD CONSTRAINT `fk_tus_modified` FOREIGN KEY `ndx_fk_tus_modified` (`modified_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tus_owned for table taxonomy_use ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_use` ADD CONSTRAINT `fk_tus_owned` FOREIGN KEY `ndx_fk_tus_owned` (`owned_by`) REFERENCES `gringlobal`.`cooperator` (`cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_tus_ts for table taxonomy_use ...') as Action;
ALTER TABLE `gringlobal`.`taxonomy_use` ADD CONSTRAINT `fk_tus_ts` FOREIGN KEY `ndx_fk_tus_ts` (`taxonomy_species_id`) REFERENCES `gringlobal`.`taxonomy_species` (`taxonomy_species_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for web_cooperator **********/
select concat(now(), ' creating constraint fk_wc_created for table web_cooperator ...') as Action;
ALTER TABLE `gringlobal`.`web_cooperator` ADD CONSTRAINT `fk_wc_created` FOREIGN KEY `ndx_fk_wc_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wc_g for table web_cooperator ...') as Action;
ALTER TABLE `gringlobal`.`web_cooperator` ADD CONSTRAINT `fk_wc_g` FOREIGN KEY `ndx_fk_wc_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wc_modified for table web_cooperator ...') as Action;
ALTER TABLE `gringlobal`.`web_cooperator` ADD CONSTRAINT `fk_wc_modified` FOREIGN KEY `ndx_fk_wc_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wc_owned for table web_cooperator ...') as Action;
ALTER TABLE `gringlobal`.`web_cooperator` ADD CONSTRAINT `fk_wc_owned` FOREIGN KEY `ndx_fk_wc_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for web_order_request **********/
select concat(now(), ' creating constraint fk_wor_created for table web_order_request ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request` ADD CONSTRAINT `fk_wor_created` FOREIGN KEY `ndx_fk_wor_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wor_modified for table web_order_request ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request` ADD CONSTRAINT `fk_wor_modified` FOREIGN KEY `ndx_fk_wor_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wor_owned for table web_order_request ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request` ADD CONSTRAINT `fk_wor_owned` FOREIGN KEY `ndx_fk_wor_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wor_wc for table web_order_request ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request` ADD CONSTRAINT `fk_wor_wc` FOREIGN KEY `ndx_fk_wor_wc` (`web_cooperator_id`) REFERENCES `gringlobal`.`web_cooperator` (`web_cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for web_order_request_action **********/
select concat(now(), ' creating constraint fk_wora_c for table web_order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_action` ADD CONSTRAINT `fk_wora_c` FOREIGN KEY `ndx_fk_wora_c` (`web_cooperator_id`) REFERENCES `gringlobal`.`web_cooperator` (`web_cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wora_created for table web_order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_action` ADD CONSTRAINT `fk_wora_created` FOREIGN KEY `ndx_fk_wora_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wora_modified for table web_order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_action` ADD CONSTRAINT `fk_wora_modified` FOREIGN KEY `ndx_fk_wora_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wora_owned for table web_order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_action` ADD CONSTRAINT `fk_wora_owned` FOREIGN KEY `ndx_fk_wora_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wora_wor for table web_order_request_action ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_action` ADD CONSTRAINT `fk_wora_wor` FOREIGN KEY `ndx_fk_wora_wor` (`web_order_request_id`) REFERENCES `gringlobal`.`web_order_request` (`web_order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for web_order_request_address **********/
select concat(now(), ' creating constraint fk_worad_created for table web_order_request_address ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_address` ADD CONSTRAINT `fk_worad_created` FOREIGN KEY `ndx_fk_worad_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worad_g for table web_order_request_address ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_address` ADD CONSTRAINT `fk_worad_g` FOREIGN KEY `ndx_fk_worad_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worad_owned for table web_order_request_address ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_address` ADD CONSTRAINT `fk_worad_owned` FOREIGN KEY `ndx_fk_worad_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worad_wor for table web_order_request_address ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_address` ADD CONSTRAINT `fk_worad_wor` FOREIGN KEY `ndx_fk_worad_wor` (`web_order_request_id`) REFERENCES `gringlobal`.`web_order_request` (`web_order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wurad_modified for table web_order_request_address ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_address` ADD CONSTRAINT `fk_wurad_modified` FOREIGN KEY `ndx_fk_wurad_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for web_order_request_attach **********/
select concat(now(), ' creating constraint fk_worat_created for table web_order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_attach` ADD CONSTRAINT `fk_worat_created` FOREIGN KEY `ndx_fk_worat_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worat_modified for table web_order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_attach` ADD CONSTRAINT `fk_worat_modified` FOREIGN KEY `ndx_fk_worat_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worat_owned for table web_order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_attach` ADD CONSTRAINT `fk_worat_owned` FOREIGN KEY `ndx_fk_worat_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worat_wc for table web_order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_attach` ADD CONSTRAINT `fk_worat_wc` FOREIGN KEY `ndx_fk_worat_wc` (`web_cooperator_id`) REFERENCES `gringlobal`.`web_cooperator` (`web_cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_worat_wor for table web_order_request_attach ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_attach` ADD CONSTRAINT `fk_worat_wor` FOREIGN KEY `ndx_fk_worat_wor` (`web_order_request_id`) REFERENCES `gringlobal`.`web_order_request` (`web_order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 6 Constraint Definitions for web_order_request_item **********/
select concat(now(), ' creating constraint fk_wori_a for table web_order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_item` ADD CONSTRAINT `fk_wori_a` FOREIGN KEY `ndx_fk_wori_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wori_created for table web_order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_item` ADD CONSTRAINT `fk_wori_created` FOREIGN KEY `ndx_fk_wori_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wori_modified for table web_order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_item` ADD CONSTRAINT `fk_wori_modified` FOREIGN KEY `ndx_fk_wori_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wori_owned for table web_order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_item` ADD CONSTRAINT `fk_wori_owned` FOREIGN KEY `ndx_fk_wori_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wori_wc for table web_order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_item` ADD CONSTRAINT `fk_wori_wc` FOREIGN KEY `ndx_fk_wori_wc` (`web_cooperator_id`) REFERENCES `gringlobal`.`web_cooperator` (`web_cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wori_wor for table web_order_request_item ...') as Action;
ALTER TABLE `gringlobal`.`web_order_request_item` ADD CONSTRAINT `fk_wori_wor` FOREIGN KEY `ndx_fk_wori_wor` (`web_order_request_id`) REFERENCES `gringlobal`.`web_order_request` (`web_order_request_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 2 Constraint Definitions for web_user **********/
select concat(now(), ' creating constraint fk_wu_sl for table web_user ...') as Action;
ALTER TABLE `gringlobal`.`web_user` ADD CONSTRAINT `fk_wu_sl` FOREIGN KEY `ndx_fk_wu_sl` (`sys_lang_id`) REFERENCES `gringlobal`.`sys_lang` (`sys_lang_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wu_wc for table web_user ...') as Action;
ALTER TABLE `gringlobal`.`web_user` ADD CONSTRAINT `fk_wu_wc` FOREIGN KEY `ndx_fk_wu_wc` (`web_cooperator_id`) REFERENCES `gringlobal`.`web_cooperator` (`web_cooperator_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for web_user_cart **********/
select concat(now(), ' creating constraint fk_wuc_created for table web_user_cart ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart` ADD CONSTRAINT `fk_wuc_created` FOREIGN KEY `ndx_fk_wuc_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuc_modified for table web_user_cart ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart` ADD CONSTRAINT `fk_wuc_modified` FOREIGN KEY `ndx_fk_wuc_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuc_owned for table web_user_cart ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart` ADD CONSTRAINT `fk_wuc_owned` FOREIGN KEY `ndx_fk_wuc_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuc_wu for table web_user_cart ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart` ADD CONSTRAINT `fk_wuc_wu` FOREIGN KEY `ndx_fk_wuc_wu` (`web_user_id`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for web_user_cart_item **********/
select concat(now(), ' creating constraint fk_wuci_a for table web_user_cart_item ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart_item` ADD CONSTRAINT `fk_wuci_a` FOREIGN KEY `ndx_fk_wuci_a` (`accession_id`) REFERENCES `gringlobal`.`accession` (`accession_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuci_created for table web_user_cart_item ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart_item` ADD CONSTRAINT `fk_wuci_created` FOREIGN KEY `ndx_fk_wuci_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuci_modified for table web_user_cart_item ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart_item` ADD CONSTRAINT `fk_wuci_modified` FOREIGN KEY `ndx_fk_wuci_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuci_owned for table web_user_cart_item ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart_item` ADD CONSTRAINT `fk_wuci_owned` FOREIGN KEY `ndx_fk_wuci_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wuci_wuc for table web_user_cart_item ...') as Action;
ALTER TABLE `gringlobal`.`web_user_cart_item` ADD CONSTRAINT `fk_wuci_wuc` FOREIGN KEY `ndx_fk_wuci_wuc` (`web_user_cart_id`) REFERENCES `gringlobal`.`web_user_cart` (`web_user_cart_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 4 Constraint Definitions for web_user_preference **********/
select concat(now(), ' creating constraint fk_wup_created for table web_user_preference ...') as Action;
ALTER TABLE `gringlobal`.`web_user_preference` ADD CONSTRAINT `fk_wup_created` FOREIGN KEY `ndx_fk_wup_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wup_modified for table web_user_preference ...') as Action;
ALTER TABLE `gringlobal`.`web_user_preference` ADD CONSTRAINT `fk_wup_modified` FOREIGN KEY `ndx_fk_wup_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wup_owned for table web_user_preference ...') as Action;
ALTER TABLE `gringlobal`.`web_user_preference` ADD CONSTRAINT `fk_wup_owned` FOREIGN KEY `ndx_fk_wup_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wup_wu for table web_user_preference ...') as Action;
ALTER TABLE `gringlobal`.`web_user_preference` ADD CONSTRAINT `fk_wup_wu` FOREIGN KEY `ndx_fk_wup_wu` (`web_user_id`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


/********** 5 Constraint Definitions for web_user_shipping_address **********/
select concat(now(), ' creating constraint fk_wusa_created for table web_user_shipping_address ...') as Action;
ALTER TABLE `gringlobal`.`web_user_shipping_address` ADD CONSTRAINT `fk_wusa_created` FOREIGN KEY `ndx_fk_wusa_created` (`created_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wusa_g for table web_user_shipping_address ...') as Action;
ALTER TABLE `gringlobal`.`web_user_shipping_address` ADD CONSTRAINT `fk_wusa_g` FOREIGN KEY `ndx_fk_wusa_g` (`geography_id`) REFERENCES `gringlobal`.`geography` (`geography_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wusa_modified for table web_user_shipping_address ...') as Action;
ALTER TABLE `gringlobal`.`web_user_shipping_address` ADD CONSTRAINT `fk_wusa_modified` FOREIGN KEY `ndx_fk_wusa_modified` (`modified_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wusa_owned for table web_user_shipping_address ...') as Action;
ALTER TABLE `gringlobal`.`web_user_shipping_address` ADD CONSTRAINT `fk_wusa_owned` FOREIGN KEY `ndx_fk_wusa_owned` (`owned_by`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


select concat(now(), ' creating constraint fk_wusa_wu for table web_user_shipping_address ...') as Action;
ALTER TABLE `gringlobal`.`web_user_shipping_address` ADD CONSTRAINT `fk_wusa_wu` FOREIGN KEY `ndx_fk_wusa_wu` (`web_user_id`) REFERENCES `gringlobal`.`web_user` (`web_user_id`) ON DELETE RESTRICT ON UPDATE RESTRICT;


