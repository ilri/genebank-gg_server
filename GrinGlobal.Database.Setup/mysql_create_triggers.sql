-- to run this script (note the --force and --delimiter options):
-- mysql -u root -p --delimiter=// --force --database gringlobal < mysql_create_triggers.sql



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
CREATE TRIGGER accession_unique_insert
BEFORE INSERT ON accession
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    accession a
  where 
    a.accession_number_part1 = NEW.accession_number_part1
    and coalesce(a.accession_number_part2,-1) = coalesce(NEW.accession_number_part2, -1)
    and coalesce(a.accession_number_part3,'') = coalesce(NEW.accession_number_part3,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      --   We try to put a string value into the integer variable on purpose!
      --   This will cause a 'Incorrect integer value' error and make the
      --   operation bomb, meaning no data is changed in the database.
      --   The string we assign it is still emitted on the error report though
      --   so we can see the root cause.
      declare pi_number int;
      select concat('Duplicate entry ''', NEW.accession_number_part1, '-', 
        coalesce(cast(NEW.accession_number_part2 as char),''), '-', 
        coalesce(new.accession_number_part3,''), ''' for key ''ndx_uniq_ac''.')
      into pi_number
      from accession
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER accession_unique_update
BEFORE UPDATE ON accession
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    accession a
  where 
    a.accession_number_part1 = NEW.accession_number_part1
    and coalesce(a.accession_number_part2,-1) = coalesce(NEW.accession_number_part2, -1)
    and coalesce(a.accession_number_part3,'') = coalesce(NEW.accession_number_part3,'')
    and a.accession_id != NEW.accession_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare pi_number int;
      select concat('Duplicate entry ''', NEW.accession_number_part1, '-', 
        coalesce(cast(NEW.accession_number_part2 as char),''), '-', 
        coalesce(new.accession_number_part3,''), ''' for key ''ndx_uniq_ac''.')
      into pi_number
      from accession
      limit 1;
    end;
  end if;
end;
//


-- --------------------------------------------------------------------
-- insert and update triggers for accession_ipr
-- --------------------------------------------------------------------
CREATE TRIGGER accession_ipr_unique_insert
BEFORE INSERT ON accession_ipr
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    accession_ipr ai
  where 
    ai.accession_id = NEW.accession_id
    and ai.type_code = NEW.type_code
    and coalesce(ai.ipr_number,'') = coalesce(NEW.ipr_number, '');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare id_type_prefix int;
      select concat('Duplicate entry ''', 
        cast(NEW.accession_id as char), 
        '-', 
        NEW.type_code,
        coalesce(new.ipr_number,''), 
        ''' for key ''ndx_uniq_ipr''.')
      into id_type_prefix
      from accession_ipr
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER accession_ipr_unique_update
BEFORE UPDATE ON accession_ipr
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    accession_ipr ai
  where 
    ai.accession_id = NEW.accession_id
    and ai.type_code = NEW.type_code
    and coalesce(ai.ipr_number,'') = coalesce(NEW.ipr_number, '')
    and ai.accession_ipr_id != NEW.accession_ipr_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare id_type_prefix int;
      select concat('Duplicate entry ''', 
        cast(NEW.accession_id as char), 
        '-', 
        NEW.type_code,
        coalesce(new.ipr_number,''), 
        ''' for key ''ndx_uniq_ipr''.')
      into id_type_prefix
      from accession_ipr
      limit 1;
    end;
  end if;
end;
//

-- --------------------------------------------------------------------
-- insert and update triggers for accession_name
-- --------------------------------------------------------------------
CREATE TRIGGER accession_name_unique_insert
BEFORE INSERT ON accession_name
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    accession_name an
  where 
    an.accession_id = NEW.accession_id
    and an.plant_name = NEW.plant_name
    and coalesce(cast(an.name_group_id as char),'') = coalesce(cast(NEW.name_group_id as char), '');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare id_name_group_id int;
      select concat('Duplicate entry ''', 
        cast(NEW.accession_id as char), 
        '-', 
        NEW.name,
        coalesce(cast(new.name_group_id as char),''), 
        ''' for key ''ndx_uniq_an''.')
      into id_name_group_id
      from accession_name
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER accession_name_unique_update
BEFORE UPDATE ON accession_name
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    accession_name an
  where 
    an.accession_id = NEW.accession_id
    and an.plant_name = NEW.plant_name
    and coalesce(cast(an.name_group_id as char),'') = coalesce(cast(NEW.name_group_id as char), '')
    and coalesce(an.category_code,'') != coalesce(NEW.category_code, '')
    and an.accession_name_id != NEW.accession_name_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare id_name_group_id int;
      select concat('Duplicate entry ''', 
        cast(NEW.accession_id as char), 
        '-', 
        NEW.plant_name,
        coalesce(cast(new.name_group_id as char),''), 
        ''' for key ''ndx_uniq_an''.')
      into id_name_group_id
      from accession_name
      limit 1;
    end;
  end if;
end;
//

-- --------------------------------------------------------------------
-- insert and update triggers for cooperator
-- --------------------------------------------------------------------
CREATE TRIGGER cooperator_unique_insert
BEFORE INSERT ON cooperator
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    cooperator co
  where 
	coalesce(co.first_name,'') = coalesce(NEW.first_name,'')
    and coalesce(co.last_name,'') = coalesce(NEW.last_name,'')
    and coalesce(co.organization,'') = coalesce(NEW.organization,'')
    and coalesce(co.city,'') = coalesce(NEW.city,'')
    and coalesce(co.geography_id,-1) = coalesce(NEW.geography_id,-1)
    and coalesce(co.primary_phone,'') = coalesce(NEW.primary_phone,'')
/*    and coalesce(co.full_name,'') = coalesce(NEW.full_name,''); */

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare address1_admin1_geography_fullname int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.first_name,''),
        '-',
        coalesce(NEW.last_name,''),
        '-',
        coalesce(NEW.organization,''),
        '-',
        coalesce(NEW.city,''),
        '-',
        coalesce(cast(NEW.geography_id as char),''),
        '-',
        coalesce(NEW.primary_phone,''),
        ''' for key ''ndx_uniq_co''.')
      into address1_admin1_geography_fullname
      from cooperator
      limit 1;
    end;
  end if;
end;
//
/*
CREATE TRIGGER cooperator_unique_update
BEFORE UPDATE ON cooperator
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    cooperator co
  where 
    coalesce(co.address_line1,'') = coalesce(NEW.address_line1,'')
    and coalesce(co.admin_1,'') = coalesce(NEW.admin_1,'')
    and coalesce(co.geography_id,-1) = coalesce(NEW.geography_id,-1)
    and coalesce(co.full_name,'') = coalesce(NEW.full_name,'')
    and co.cooperator_id != NEW.cooperator_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare address1_admin1_geography_fullname int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.address_line1,''),
        '-',
        coalesce(NEW.admin_1,''),
        '-',
        coalesce(cast(NEW.geography_id as char),''),
        '-',
        coalesce(NEW.full_name,''),
        ''' for key ''ndx_uniq_co''.')
      into address1_admin1_geography_fullname
      from cooperator
      limit 1;
    end;
  end if;
end;
//
*/


-- --------------------------------------------------------------------
-- insert and update triggers for taxonomy_family
-- --------------------------------------------------------------------
CREATE TRIGGER taxonomy_family_unique_insert
BEFORE INSERT ON taxonomy_family
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    taxonomy_family tf
  where 
    coalesce(tf.family_name,'') = coalesce(NEW.family_name,'')
    and coalesce(tf.author_name,'') = coalesce(NEW.author_name,'')
    and coalesce(tf.subfamily_name,'') = coalesce(NEW.subfamily_name,'')
    and coalesce(tf.tribe_name,'') = coalesce(NEW.tribe_name,'')
    and coalesce(tf.subtribe_name,'') = coalesce(NEW.subtribe_name,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare fam_aut_subfam_tribe_subtribe int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.family_name,''),
        '-',
        coalesce(NEW.author_name,''),
        '-',
        coalesce(NEW.subfamily_name,''),
        '-',
        coalesce(NEW.tribe_name,''),
        '-',
        coalesce(NEW.subtribe_name,''),
        ''' for key ''ndx_uniq_fa''.')
      into fam_aut_subfam_tribe_subtribe
      from taxonomy_family
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER taxonomy_family_unique_update
BEFORE UPDATE ON taxonomy_family
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    taxonomy_family tf
  where 
    coalesce(tf.family_name,'') = coalesce(NEW.family_name,'')
    and coalesce(tf.author_name,'') = coalesce(NEW.author_name,'')
    and coalesce(tf.subfamily_name,'') = coalesce(NEW.subfamily_name,'')
    and coalesce(tf.tribe_name,'') = coalesce(NEW.tribe_name,'')
    and coalesce(tf.subtribe_name,'') = coalesce(NEW.subtribe_name,'')
    and tf.taxonomy_family_id != NEW.taxonomy_family_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare fam_aut_subfam_tribe_subtribe int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.family_name,''),
        '-',
        coalesce(NEW.author_name,''),
        '-',
        coalesce(NEW.subfamily_name,''),
        '-',
        coalesce(NEW.tribe_name,''),
        '-',
        coalesce(NEW.subtribe_name,''),
        ''' for key ''ndx_uniq_fa''.')
      into fam_aut_subfam_tribe_subtribe
      from taxonomy_family
      limit 1;
    end;
  end if;
end;
//

-- --------------------------------------------------------------------
-- insert and update triggers for geography
-- --------------------------------------------------------------------
CREATE TRIGGER geography_unique_insert
BEFORE INSERT ON geography
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    geography ge
  where 
    coalesce(ge.country_code,'') = coalesce(NEW.country_code,'')
    and coalesce(ge.adm1,'') = coalesce(NEW.adm1,'');
    and coalesce(ge.adm1_type_code,'') = coalesce(NEW.adm1_type_code,'');
    and coalesce(ge.adm2,'') = coalesce(NEW.adm2,'');
    and coalesce(ge.adm2_type_code,'') = coalesce(NEW.adm2_type_code,'');
    and coalesce(ge.adm3,'') = coalesce(NEW.adm3,'');
    and coalesce(ge.adm3_type_code,'') = coalesce(NEW.adm3_type_code,'');
    and coalesce(ge.adm4,'') = coalesce(NEW.adm4,'');
    and coalesce(ge.adm4_type_code,'') = coalesce(NEW.adm4_type_code,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare country_state int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.country_code,''),
        '-',
        coalesce(NEW.adm1,''),
        '-',
        coalesce(NEW.adm1_type_code,''),
        '-',
        coalesce(NEW.adm2,''),
        '-',
        coalesce(NEW.adm2_type_code,''),
        '-',
        coalesce(NEW.adm3,''),
        '-',
        coalesce(NEW.adm3_type_code,''),
        '-',
        coalesce(NEW.adm4,''),
        '-',
        coalesce(NEW.adm4_type_code,''),
        ''' for key ''ndx_uniq_ge_name''.')
      into country_state
      from geography
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER geography_unique_update
BEFORE UPDATE ON geography
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    geography ge
  where 
    coalesce(ge.country_code,'') = coalesce(NEW.country_code,'')
    and coalesce(ge.adm1,'') = coalesce(NEW.adm1,'');
    and coalesce(ge.adm1_type_code,'') = coalesce(NEW.adm1_type_code,'');
    and coalesce(ge.adm2,'') = coalesce(NEW.adm2,'');
    and coalesce(ge.adm2_type_code,'') = coalesce(NEW.adm2_type_code,'');
    and coalesce(ge.adm3,'') = coalesce(NEW.adm3,'');
    and coalesce(ge.adm3_type_code,'') = coalesce(NEW.adm3_type_code,'');
    and coalesce(ge.adm4,'') = coalesce(NEW.adm4,'');
    and coalesce(ge.adm4_type_code,'') = coalesce(NEW.adm4_type_code,'');
    and ge.geography_id != NEW.geography_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare country_state int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.country_code,''),
        '-',
        coalesce(NEW.adm1,''),
        '-',
        coalesce(NEW.adm1_type_code,''),
        '-',
        coalesce(NEW.adm2,''),
        '-',
        coalesce(NEW.adm2_type_code,''),
        '-',
        coalesce(NEW.adm3,''),
        '-',
        coalesce(NEW.adm3_type_code,''),
        '-',
        coalesce(NEW.adm4,''),
        '-',
        coalesce(NEW.adm4_type_code,''),
        ''' for key ''ndx_uniq_ge_name''.')
      into country_state
      from geography
      limit 1;
    end;
  end if;
end;
//



-- --------------------------------------------------------------------
-- insert and update triggers for inventory
-- --------------------------------------------------------------------
CREATE TRIGGER inventory_unique_insert
BEFORE INSERT ON inventory
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    inventory inv
  where 
    coalesce(inv.inventory_number_part1,'') = coalesce(NEW.inventory_number_part1,'')
    and coalesce(inv.inventory_number_part2,-1) = coalesce(NEW.inventory_number_part2,-1)
    and coalesce(inv.inventory_number_part3,'') = coalesce(NEW.inventory_number_part3,'')
    and coalesce(inv.form_type_code,'') = coalesce(NEW.form_type_code,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare pi_number int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.inventory_number_part1,''),
        '-',
        coalesce(cast(NEW.inventory_number_part2 as char),''),
        '-',
        coalesce(NEW.inventory_number_part3,''),
        '-',
        coalesce(NEW.form_type_code,''),
        ''' for key ''ndx_uniq_go''.')
      into pi_number
      from inventory
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER inventory_unique_update
BEFORE UPDATE ON inventory
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    inventory inv
  where 
    coalesce(inv.inventory_number_part1,'') = coalesce(NEW.inventory_number_part1,'')
    and coalesce(inv.inventory_number_part2,-1) = coalesce(NEW.inventory_number_part2,-1)
    and coalesce(inv.inventory_number_part3,'') = coalesce(NEW.inventory_number_part3,'')
    and coalesce(inv.form_type_code,'') = coalesce(NEW.form_type_code,'')
    and inv.inventory_id != NEW.inventory_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare pi_number int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.inventory_number_part1,''),
        '-',
        coalesce(cast(NEW.inventory_number_part2 as char),''),
        '-',
        coalesce(NEW.inventory_number_part3,''),
        '-',
        coalesce(NEW.form_type_code,''),
        ''' for key ''ndx_uniq_go''.')
      into pi_number
      from inventory
      limit 1;
    end;
  end if;
end;
//

-- --------------------------------------------------------------------
-- insert and update triggers for inventory_quality_status
-- --------------------------------------------------------------------
CREATE TRIGGER inventory_quality_status_unique_insert
BEFORE INSERT ON inventory_quality_status
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    inventory_quality_status iqs
  where 
    coalesce(iqs.inventory_id,-1) = coalesce(NEW.inventory_id,-1)
    and coalesce(iqs.test_type,'') = coalesce(NEW.test_type,'')
    and coalesce(iqs.pathogen_code,'') = coalesce(NEW.pathogen_code,'')
    and coalesce(cast(iqs.finished_date as char),'') = coalesce(cast(NEW.finished_date as char),'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare pi_number int;
      select concat('Duplicate entry ''', 
        coalesce(cast(NEW.inventory_id as char),''),
        '-',
        coalesce(NEW.test_type,''),
        '-',
        coalesce(NEW.pathogen_code,''),
        '-',
        coalesce(cast(NEW.finished_date as char),''),
        ''' for key ''ndx_uniq_iqs''.')
      into pi_number
      from inventory_quality_status
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER inventory_quality_status_unique_update
BEFORE UPDATE ON inventory_quality_status
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    inventory_quality_status iqs
  where 
    coalesce(iqs.inventory_id,-1) = coalesce(NEW.inventory_id,-1)
    and coalesce(iqs.test_type,'') = coalesce(NEW.test_type,'')
    and coalesce(iqs.pathogen_code,'') = coalesce(NEW.pathogen_code,'')
    and coalesce(cast(iqs.finished_date as char),'') = coalesce(cast(NEW.finished_date as char),'')
    and iqs.inventory_quality_status_id != NEW.inventory_quality_status_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare pi_number int;
      select concat('Duplicate entry ''', 
        coalesce(cast(NEW.inventory_id as char),''),
        '-',
        coalesce(NEW.test_type,''),
        '-',
        coalesce(NEW.pathogen_code,''),
        '-',
        coalesce(cast(NEW.finished_date as char),''),
        ''' for key ''ndx_uniq_iqs''.')
      into pi_number
      from inventory_quality_status
      limit 1;
    end;
  end if;
end;
//


-- --------------------------------------------------------------------
-- insert and update triggers for order_request_item
-- --------------------------------------------------------------------
CREATE TRIGGER order_request_item_unique_insert
BEFORE INSERT ON order_request_item
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    order_request_item ori
  where 
    coalesce(ori.order_request_id,-1) = coalesce(NEW.order_request_id,-1)
    and coalesce(ori.sequence_number,-1) = coalesce(NEW.sequence_number,-1);

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare id_sequence int;
      select concat('Duplicate entry ''', 
        coalesce(cast(NEW.order_request_id as char),''),
        '-',
        coalesce(cast(NEW.sequence_number as char),''),
        ''' for key ''ndx_uniq_ori''.')
      into id_sequence
      from order_request_item
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER order_request_item_unique_update
BEFORE UPDATE ON order_request_item
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    order_request_item ori
  where 
    coalesce(ori.order_request_id,-1) = coalesce(NEW.order_request_id,-1)
    and coalesce(ori.sequence_number,-1) = coalesce(NEW.sequence_number,-1)
    and ori.order_request_item_id != NEW.order_request_item_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare id_sequence int;
      select concat('Duplicate entry ''', 
        coalesce(cast(NEW.order_request_id as char),''),
        '-',
        coalesce(cast(NEW.sequence_number as char),''),
        ''' for key ''ndx_uniq_ori''.')
      into id_sequence
      from order_request_item
      limit 1;
    end;
  end if;
end;
//


-- --------------------------------------------------------------------
-- insert and update triggers for region
-- --------------------------------------------------------------------
CREATE TRIGGER region_unique_insert
BEFORE INSERT ON region
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    region re
  where 
    coalesce(re.continent,'') = coalesce(NEW.continent,'')
    and coalesce(re.subcontinent,'') = coalesce(NEW.subcontinent,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare cont_subcont int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.continent,''),
        '-',
        coalesce(NEW.subcontinent,''),
        ''' for key ''ndx_uniq_re''.')
      into cont_subcont
      from region
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER region_unique_update
BEFORE UPDATE ON region
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    region re
  where 
    coalesce(re.continent,'') = coalesce(NEW.continent,'')
    and coalesce(re.subcontinent,'') = coalesce(NEW.subcontinent,'')
    and re.region_id != NEW.region_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare cont_subcont int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.continent,''),
        '-',
        coalesce(NEW.subcontinent,''),
        ''' for key ''ndx_uniq_re''.')
      into cont_subcont
      from region
      limit 1;
    end;
  end if;
end;
//

-- --------------------------------------------------------------------
-- insert and update triggers for taxonomy
-- --------------------------------------------------------------------
CREATE TRIGGER taxonomy_unique_insert
BEFORE INSERT ON taxonomy
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    taxonomy t
  where 
    coalesce(t.name,'') = coalesce(NEW.name,'')
    and coalesce(t.name_authority,'') = coalesce(NEW.name_authority,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare name_name_auth int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.name,''),
        '-',
        coalesce(NEW.name_authority,''),
        ''' for key ''ndx_uniq_ta''.')
      into name_name_auth
      from taxonomy
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER taxonomy_unique_update
BEFORE UPDATE ON taxonomy
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    taxonomy t
  where 
    coalesce(t.name,'') = coalesce(NEW.name,'')
    and coalesce(t.name_authority,'') = coalesce(NEW.name_authority,'')
    and t.taxonomy_id != NEW.taxonomy_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare name_name_auth int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.name,''),
        '-',
        coalesce(NEW.name_authority,''),
        ''' for key ''ndx_uniq_ta''.')
      into name_name_auth
      from taxonomy
      limit 1;
    end;
  end if;
end;
//

-- --------------------------------------------------------------------
-- insert and update triggers for taxonomy_genus
-- --------------------------------------------------------------------
CREATE TRIGGER taxonomy_genus_unique_insert
BEFORE INSERT ON taxonomy_genus
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    taxonomy_genus tg
  where 
    coalesce(tg.genus_name,'') = coalesce(NEW.genus_name,'')
    and coalesce(tg.genus_authority,'') = coalesce(NEW.genus_authority,'')
    and coalesce(tg.subgenus_name,'') = coalesce(NEW.subgenus_name,'')
    and coalesce(tg.section_name,'') = coalesce(NEW.section_name,'')
    and coalesce(tg.series_name,'') = coalesce(NEW.series_name,'')
    and coalesce(tg.subseries_name,'') = coalesce(NEW.subseries_name,'')
    and coalesce(tg.subsection_name,'') = coalesce(NEW.subsection_name,'');

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare name_auth_sub_sect_series_subseries_subsect int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.genus_authority,''),
        '-',
        coalesce(NEW.subgenus_name,''),
        '-',
        coalesce(NEW.section_name,''),
        '-',
        coalesce(NEW.series_name,''),
        '-',
        coalesce(NEW.subseries_name,''),
        '-',
        coalesce(NEW.subsection_name,''),
        ''' for key ''ndx_uniq_tg''.')
      into name_auth_sub_sect_series_subseries_subsect
      from taxonomy_genus
      limit 1;
    end;
  end if;
end;
//

CREATE TRIGGER taxonomy_genus_unique_update
BEFORE UPDATE ON taxonomy_genus
  FOR EACH ROW
BEGIN

  DECLARE n1 INT DEFAULT 0;

  select 
    count(*) as num
  into 
    n1
  from 
    taxonomy_genus tg
  where 
    coalesce(tg.genus_name,'') = coalesce(NEW.genus_name,'')
    and coalesce(tg.genus_authority,'') = coalesce(NEW.genus_authority,'')
    and coalesce(tg.subgenus_name,'') = coalesce(NEW.subgenus_name,'')
    and coalesce(tg.section_name,'') = coalesce(NEW.section_name,'')
    and coalesce(tg.series_name,'') = coalesce(NEW.series_name,'')
    and coalesce(tg.subseries_name,'') = coalesce(NEW.subseries_name,'')
    and coalesce(tg.subsection_name,'') = coalesce(NEW.subsection_name,'')
    and tg.taxonomy_genus_id != NEW.taxonomy_genus_id;

  if (n1 > 0) then
    begin
      -- HACK HACK HACK HACK HACK
      declare name_auth_sub_sect_series_subseries_subsect int;
      select concat('Duplicate entry ''', 
        coalesce(NEW.genus_authority,''),
        '-',
        coalesce(NEW.subgenus_name,''),
        '-',
        coalesce(NEW.section_name,''),
        '-',
        coalesce(NEW.series_name,''),
        '-',
        coalesce(NEW.subseries_name,''),
        '-',
        coalesce(NEW.subsection_name,''),
        ''' for key ''ndx_uniq_tg''.')
      into name_auth_sub_sect_series_subseries_subsect
      from taxonomy_genus
      limit 1;
    end;
  end if;
end;
//
