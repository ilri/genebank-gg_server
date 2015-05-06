-- create a default inventory record for every accession
insert into inventory (
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
	GETDATE(),
	1,
	GETDATE(),
	1
from
	accession a 
where
	a.accession_id not in 
	(select distinct accession_id from inventory where inventory_type_code = '**');

-- select inventory_id, count(inventory_id) from inventory where inventory_type_code = '**' group by inventory_id having COUNT(inventory_id) > 1

update crop_trait_observation
set
	inventory_id = (select i.inventory_id from inventory i where i.inventory_type_code = '**' and i.accession_id = crop_trait_observation.accession_id),
	modified_date = GETDATE()
where
	accession_id is not null
	and inventory_id is null

update order_request_item
set
	inventory_id = (select i.inventory_id from inventory i where i.inventory_type_code = '**' and i.accession_id = order_request_item.accession_id),
	modified_date = GETDATE()
where
	accession_id is not null
	and inventory_id is null
	

-- select * from inventory where inventory_type_code = '**'
-- delete from inventory where inventory_type_code = '**' and inventory_id not in (1659339)