/*
-- clean up database
delete from inventory
delete from inventory_maint_policy
delete from accession_pedigree
delete from accession
delete from taxonomy_species
delete from taxonomy_genus
delete from taxonomy_family
delete from cooperator where last_name not like 'SYSTEM%'
delete from site where site_short_name != 'SYS'
delete from geography where geography_id not in (select geography_id from cooperator where geography_id is not null)
delete from region
*/

-- check counts
select COUNT(*) as inventory from inventory
select COUNT(*) as inventory_maint_policy from inventory_maint_policy
select COUNT(*) as accession_pedigree from accession_pedigree
select COUNT(*) as accession from accession
select COUNT(*) as taxonomy_species from taxonomy_species
select COUNT(*) as taxonomy_genus from taxonomy_genus
select COUNT(*) as taxonomy_family from taxonomy_family
select COUNT(*) as cooperator from cooperator where last_name not like 'SYSTEM%'
select COUNT(*) as site from site where site_short_name != 'SYS'
select COUNT(*) as geography from geography
select COUNT(*) as region from region
select COUNT(*) as crop_trait_observation from crop_trait_observation

-- check if auto-creating parent is working
select * from inventory where inventory_maint_policy_id not in (select inventory_maint_policy_id from inventory_maint_policy where maintenance_name = 'SYS')

-- check if multiple aliases are working
select * from accession where backup_location1_site_id is not null or backup_location2_site_id is not null
select * from accession_pedigree where male_accession_id is not null or female_accession_id is not null

-- check if self-ref keys are working
select * from inventory where parent_inventory_id is not null or backup_inventory_id is not null
select * from taxonomy_family where current_taxonomy_family_id != taxonomy_family_id
