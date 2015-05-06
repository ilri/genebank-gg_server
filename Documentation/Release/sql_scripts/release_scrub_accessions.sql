use gringlobal;

begin try

begin tran

create table #del_accession (
id int not null
);

/*********************************************
Alter sql below as needed to fill temp table
with accession_id's that need to be deleted
**********************************************/


insert into #del_accession (id) 
/*** CHANGE THIS SELECT STATEMENT ***/
select accession_id from accession 
where accession_id not in (
	select accession_id from accession where accession_number_part1 = 'PI' and accession_number_part2 between 500000 and 550000
)

/*********************************************
      Do not alter below this line
**********************************************/

-- select * from #del_accession
declare @count int
set @count = (select COUNT(*) from #del_accession)

-- remember all inventory items we will be deleting
create table #del_inventory (
id int not null
);
insert into #del_inventory (id)
select inventory_id from inventory where accession_id in (
	select id from #del_accession
)
--select * from #del_inventory


-- remember all order_request_items we will be deleting
create table #del_order_request (
id int not null
);
insert into #del_order_request (id)
select distinct(order_request_id) from order_request_item where inventory_id in (
	select id from #del_inventory
)
--select * from #del_order_request



--select * from order_request where order_request_id in (
--	select id from #del_order_request
--	) and order_request_id not in (select order_request_id from order_request_item
--	 where inventory_id in (select id from #del_inventory)
--	 )
	 
--	 print 1/ 0;
	 



-- order_request_item
delete from order_request_item where order_request_id in (
	select id from #del_order_request
)

-- order_request_action
delete from order_request_action where order_request_id in (
	select id from #del_order_request
)

-- crop_trait_observation
delete from crop_trait_observation where inventory_id in (
	select id from #del_inventory
)



-- order_request
delete from order_request where order_request_id in (
	select id from #del_order_request
)




-- inventory_quality_status
delete from inventory_quality_status where inventory_id in (
	select id from #del_inventory
)

-- inventory_viability
delete from inventory_viability where inventory_id in (
	select id from #del_inventory
)

-- genetic_observation
delete from genetic_observation where inventory_id in (
	select id from #del_inventory
)

-- crop_trait_observation
delete from crop_trait_observation where inventory_id in (
	select id from #del_inventory
)

-- inventory_image
delete from inventory_attach where inventory_id in (
	select id from #del_inventory
)

-- accession_voucher
delete from accession_voucher where inventory_id in (
	select id from #del_inventory
)

-- inventory_action
delete from inventory_action where inventory_id in (
	select id from #del_inventory
)

-- inventory_group_map
delete from inventory_group_map where inventory_id in (
	select id from #del_inventory
)

-- accession_annotation
delete from accession_annotation where inventory_id in (
	select id from #del_inventory
)

-- site_inventory_nc7
delete from site_inventory_nc7 where inventory_id in (
	select id from #del_inventory
)

-- inventory
delete from inventory where accession_id in (
	select id from #del_accession
)


-- accession_action
delete from accession_action where accession_id in (
	select id from #del_accession
)

-- accession_pedigree
delete from accession_pedigree where accession_id in (
	select id from #del_accession
)

-- accession_quarantine
delete from accession_quarantine where accession_id in (
	select id from #del_accession
)

-- accession_ipr
delete from accession_ipr where accession_id in (
	select id from #del_accession
)

-- accession_citation_map
delete from citation_map where accession_id in (
	select id from #del_accession
)

-- accession_source_map
delete from accession_source_map where accession_source_id in (
	select accession_source_id from accession_source where accession_id in (
		select id from #del_accession
	)
)

-- accession_source
delete from accession_source where accession_id in (
	select id from #del_accession
)


-- accession_name
delete from accession_name where accession_id in (
	select id from #del_accession
)

-- accession
delete from accession where accession_id in (
	select id from #del_accession
)


print 'SUCCESS - ' + convert(varchar, @count) + ' accessions successfully cascade deleted.'
--print 'SHOULD HAVE COMMITTED'
commit tran

end try
begin catch

print 'Error: ' + coalesce(convert(varchar, ERROR_NUMBER()), '') + 
	' sev ' + coalesce(convert(varchar, ERROR_SEVERITY()), '') +
	' state ' + coalesce(convert(varchar, ERROR_STATE()), '') +
	' proc ' + coalesce(convert(varchar, ERROR_PROCEDURE()), '') +
	' line ' + coalesce(convert(varchar, ERROR_LINE()), '') + 
	' msg ' + coalesce(ERROR_MESSAGE(), '');
    
rollback tran
print 'transaction rolled back'

end catch


drop table #del_order_request;
drop table #del_inventory;
drop table #del_accession;