-- to run this script (note the --force and --delimiter options):
-- mysql -u root -p --force --database gringlobal < mysql_create_triggers.sql



-- HACK WARNING!!!

-- Unique indexes in MySQL 5.1 does not support uniqueness on null fields.
-- Meaning if there's a unique index on field f1, it is possible to have
-- two records with f1 = null.

-- To circumvent this shortcoming, we are using triggers to emulate the
-- desired behavior of treating null = null within a unique index for certain tables.

-- However, MySQL 5.1 does not support aborting an update/insert from a trigger,
-- so we use this major hack to get around it: assign an integer
-- variable a string value (so the trigger bombs and data is not saved)
-- http://www.brokenbuild.com/blog/2006/08/15/mysql-triggers-how-do-you-abort-an-insert-update-or-delete-with-a-trigger/




-- --------------------------------------------------------------------
-- insert and update triggers for accession
-- --------------------------------------------------------------------
drop trigger accession_unique_insert;
drop trigger accession_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for accession_ipr
-- --------------------------------------------------------------------
drop trigger accession_ipr_unique_insert;
drop trigger accession_ipr_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for accession_name
-- --------------------------------------------------------------------
drop trigger accession_name_unique_insert;
drop trigger accession_name_unique_update;
-- --------------------------------------------------------------------
-- insert and update triggers for cooperator
-- --------------------------------------------------------------------
drop trigger cooperator_unique_insert;
drop trigger cooperator_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for crop_trait_url
-- --------------------------------------------------------------------
drop trigger crop_trait_url_unique_insert;
drop trigger crop_trait_url_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for taxonomy_family
-- --------------------------------------------------------------------
drop trigger taxonomy_family_unique_insert;
drop trigger taxonomy_family_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for geography
-- --------------------------------------------------------------------
drop trigger geography_unique_insert;
drop trigger geography_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for genomic_observation
-- --------------------------------------------------------------------
drop trigger genomic_observation_unique_insert;
drop trigger genomic_observation_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for inventory
-- --------------------------------------------------------------------
drop trigger inventory_unique_insert;
drop trigger inventory_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for inventory_quality_status
-- --------------------------------------------------------------------
drop trigger inventory_quality_status_unique_insert;
drop trigger inventory_quality_status_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for order_request_item
-- --------------------------------------------------------------------
drop trigger order_request_item_unique_insert;
drop trigger order_request_item_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for region
-- --------------------------------------------------------------------
drop trigger region_unique_insert;
drop trigger region_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for taxonomy
-- --------------------------------------------------------------------
drop trigger taxonomy_unique_insert;
drop trigger taxonomy_unique_update;

-- --------------------------------------------------------------------
-- insert and update triggers for taxonomy_genus
-- --------------------------------------------------------------------
drop trigger taxonomy_genus_unique_insert;
drop trigger taxonomy_genus_unique_update;
