using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector {
	internal class Resolver {

		public static string GenerateSql(string tableName, string pkName, string resolverName) {
			string sql = null;
			switch (resolverName.ToLower()) {
				case "accessions":
                case "accession":
					// they're trying to resolve to an accession record
					sql = resolveAccession(tableName.ToLower().Trim());
					break;
				case "inventory":
					// they're trying to resolve to an inventory record
					sql = resolveInventory(tableName.ToLower().Trim());
					break;
				case "orders":
                case "order_request":
					// they're trying to resolve to an order_request record
					sql = resolveOrder(tableName.ToLower().Trim());
					break;
				default:
					throw new NotImplementedException(getDisplayMember("GenerateSql", "Creator.determineResolverSql() not implemented for resolver={0}", resolverName));
			}

            // NOTE: we are essentially leaving ourselves a sql-friendly marker to put a where in later via code as needed
			if (!String.IsNullOrEmpty(sql)) {
                sql = String.Format(sql, pkName, tableName.ToLower(), tableName.ToLower() + " /*_ WHERE src." + pkName + " in (:idlist) _*/ ");
			}

			return sql;

		}

		private static string resolveAccession(string tableName) {

			switch (tableName) {

				// ========================================================================
				// Tables with resolve table primary key as its primary key
				// ========================================================================
				case "accession":
					throw new NotImplementedException("Resolving an accession from table " + tableName + " makes no sense because it is the same table.  Value is in PrimaryKey.");

				// ========================================================================
				// Tables with resolve table primary key as a foreign key
				// ========================================================================
				case "inventory":
				case "accession_action":
				case "accession_annotation":
				case "accession_citation_map":
				case "accession_habitat":
				case "accession_image_map":
				case "accession_ipr":
				case "accession_name":
				case "accession_narrative":
				case "accession_pedigree":
				case "accession_quarantine":
				case "accession_source":

					throw new NotImplementedException("Resolving an accession from table " + tableName + " makes no sense because it should contain a foreign key for accession (meaning you already have the accession_id)");
				
				case "accession_source_map":
					return @"
select
	src.{0},
	asrc.accession_id
from
	accession_source asrc inner join {1} src
		on asrc.accession_source_id = src.accession_source_id
{2}
";

				case "accession_group":

					return @"
select
	src.{0},
	src.accession_id
from
	accession_name src
{2}
";


				// ========================================================================
				// Tables with Order_Request_ID as primary key
				// ========================================================================
				case "order_request":
					// order_request -> order_request_item
					// order_request_item -> accession OR inventory
					return @"
select 
	src.{0},
	coalesce(src.accession_id, i.accession_id) as accession_id
from 
	order_request_item src left join inventory i
		on src.inventory_id = i.inventory_id
{2}
";
					


				// ========================================================================
				// Tables with Inventory_ID only as foreign key
				// ========================================================================
				case "genomic_observation":
				case "inventory_action":
				case "inventory_group_map":
				case "inventory_image_map":
				case "inventory_quality_status":
				case "inventory_viability":
					// inventory -> accession

					return @"
select 
	src.{0},
	i.accession_id
from 
	{1} src inner join inventory i 
	on src.inventory_id = i.inventory_id
{2}
";
				case "inventory_maint_policy":
					return @"
select 
	src.{0},
	src.accession_id
from 
	inventory src inner join accession a 
		on src.accession_id = a.accession_id
{2}
";

				case "inventory_group":
					return @"
select
	src.{0},
	i.accession_id
from
	inventory_group_map src inner join inventory i
		on src.inventory_id = i.inventory_id
{2}
";

				// ========================================================================
				// Tables with Order_Request_ID only as foreign key
				// ========================================================================
				case "order_request_action":
					// order_action -> order_entry
					// order_entry -> order_item
					//order_item -> accession  OR  inventory
					return @"
select 
	src.{0},
	coalesce(ori.accession_id, i.accession_id) as accession_id
from 
	order_request_action src inner join order_request_item ori
		on src.order_request_id = ori.order_request_id
	left join inventory i
		on ori.inventory_id = i.inventory_id
{2}
";
					

				// ========================================================================
				// Tables with both Accession_ID and Inventory_ID as foreign key
				// ========================================================================
				case "accession_voucher":
					throw new NotImplementedException("Resolver.resolveAccession() -> both accession_id and inventory_id .... how to do it???");

				// ========================================================================
				// Tables that must "bubble up" to Accession_ID
				// ========================================================================
				case "taxonomy_common_name":
					return @"
select 
	src.{0},
	a.accession_id
from 
	accession a inner join taxonomy_common_name src
	on a.taxonomy_id = src.taxonomy_id
{2}
";
					

				case "cooperator":
					//                    // TODO: how do we join across via cooperator???
					return @"
select
	src.source_{0} as {0},
	coalesce(ori.accession_id, i.accession_id) as accession_id
from
	order_request_item ori inner join order_request src
		on ori.order_request_id = src.order_request_id
	left join inventory i 
		on ori.inventory_id = i.inventory_id
{2}
";
					

				case "cooperator_group":
					return @"
select
	src.{0},
	coalesce(ori.accession_id, i.accession_id) as accession_id
from
	cooperator_group src inner join cooperator_map cm
		on src.cooperator_group_id = cm.cooperator_group_id
	inner join order_request oreq
		on oreq.source_cooperator_id = cm.cooperator_id
	inner join order_request_item ori 
		on oreq.order_request_id = ori.order_request_id
	left join inventory i 
		on ori.inventory_id = i.inventory_id
{2}
";
					
				case "cooperator_map":
					return @"
select
	src.{0},
	coalesce(ori.accession_id, i.accession_id) as accession_id
from
	cooperator_map src inner join order_request oreq
		on oreq.source_cooperator_id = src.cooperator_id
	inner join order_request_item ori 
		on ori.order_request_id = oreq.order_request_id
	left join inventory i 
		on ori.inventory_id = i.inventory_id
{2}
";

                case "order_request_item":
                    return @"
select
    src.{0},
    i.accession_id
from
    order_request_item src inner join inventory i
        on src.inventory_id = i.inventory_id
{2}
";
                case "crop_trait_observation":
                    return @"
select
    src.{0},
    i.accession_id
from
    crop_trait_observation src inner join inventory i
        on src.inventory_id = i.inventory_id
{2}
";
					

				case "crop":
					return @"
select 
	src.{0},
	a.accession_id
from 
	accession a inner join taxonomy src
		on a.taxonomy_id = src.taxonomy_id 
{2}
";
					

				case "taxonomy_distribution":
					return @"
select 
	src.{0},
	a.accession_id
from 
	accession a inner join taxonomy_distribution src
		on a.taxonomy_id = src.taxonomy_id
{2}
";
					

				case "method":
					// method can tie to many tables, so we must search by
					// all of them.  These include any of the following:


					// accession_action
					// crop_trait_url
					// method_citation_map
					// method_map
					// genomic_annotation
					// inventory_action
					// crop_trait_observation
					// inventory_viability

					// accession_action
					string sql = @"
select
	src.{0},
	src.accession_id
from
	accession_action src
{2}
";

					// inventory_action
					sql += @"
UNION
select
	src.{0},
	i.accession_id
from
	inventory_action src inner join inventory i
	on src.inventory_id = i.inventory_id
{2}
";
					// crop_trait_observation
					//throw new NotImplementedException("implement cto!");
					//fillByAccessionOrInventory(ret, matches, tableName);

					// inventory_viability
					sql += @"
UNION
select
	src.{0},
	i.accession_id
from
	inventory_viability src inner join inventory i
		on src.inventory_id = i.inventory_id
{2}
";

					// crop_trait_url
					sql += @"
UNION
select
	src.{0},
	coalesce(cto.accession_id, i.accession_id) as accession_id
from
	crop_trait_url src inner join crop_trait_observation cto
		on src.crop_trait_id = cto.crop_trait_id
	inner join inventory i
		on cto.inventory_id = i.inventory_id
{2}
";

					// TODO: method_citation_map
					sql += @"
UNION
select
	src.{0},
	src.accession_id
from
	accession_action src
{2}
";
					sql += @"
UNION
select
	src.{0},
	i.accession_id
from
	inventory_action src inner join inventory i
		on src.inventory_id = i.inventory_id
{2}
";


					// TODO: method_map

					// TODO: genomic_annotation

					return sql;

				case "taxonomy_family":
					return @"
select
	src.{0},
	a.accession_id
from
	taxonomy_genus src inner join taxonomy t
		on src.taxonomy_genus_id = t.taxonomy_genus_id
	inner join accession a
		on t.taxonomy_id = a.taxonomy_id
{2}
";

				case "taxonomy_genus":
					return @"
select
	src.{0},
	a.accession_id
from
	accession a inner join taxonomy src
		on a.taxonomy_id = src.taxonomy_id
{2}
";

				case "taxonomy_genus_citation_map":
					// genus_citation -> genus
					// taxonomy -> genus
					// accession -> taxonomy
					return @"
select
	src.{0},
	a.accession_id
from
	accession a inner join taxonomy t
		on a.taxonomy_id = t.taxonomy_id
	inner join taxonomy_genus_citation_map src
		on t.taxonomy_genus_id = src.taxonomy_genus_id
{2}
";


				case "literature":
					// Literature_ID is found in following tables:
					// * citation
					// which of course, all these tables reference:
					// * accession_citation_map
					// * method_citation_map
					// * taxonomy_genus_citation_map
					// * genomic_marker_citation
					// * taxonomy_citation


					// accession_citation_map
					string sql2 = @"
select
	src.{0},
	acm.accession_id
from
	accession_citation_map acm inner join citation src
		on acm.citation_id = src.citation_id
{2}
";



					// taxonomy_genus_citation_map
					sql2 += @"
UNION
select
	src.{0},
	a.accession_id
from
	citation src inner join taxonomy_genus_citation_map tgcm 
		on src.citation_id = tgcm.citation_id
	inner join taxonomy t
		on tgcm.taxonomy_genus_id = t.taxonomy_genus_id
	inner join accession a
		on t.taxonomy_id = a.taxonomy_id
{2}
";

					// taxonomy_citation_map
					sql2 += @"
UNION
select
	src.{0},
	a.accession_id
from
	citation src inner join taxonomy_citation_map tcm 
		on src.citation_id = tcm.citation_id
	inner join accession a
		on a.taxonomy_id = tcm.taxonomy_id
{2}
";



					// TODO: method_citation_map


					// TODO: genomic_marker_citation_map

					return sql2;


				case "genomic_marker":
					// crop -> marker
					// trait -> crop
					// observation -> trait
					// observation -> accession OR inventory
					return @"
select
	src.{0},
	coalesce(cto.accession_id, i.accession_id) as accession_id
from
	genomic_marker src inner join crop_trait ct
		on src.crop_id = ct.crop_id
	inner join crop_trait_observation cto
		on cto.crop_trait_id = ct.crop_trait_id
	left join inventory i
		on cto.inventory_id = i.inventory_id
{2}
";

				case "genomic_marker_citation_map":
					// marker_citation -> marker
					// crop -> marker
					// trait -> crop
					// observation -> trait
					// observation -> accession OR inventory
					return @"
select
	src.{0},
	coalesce(cto.accession_id, i.accession_id) as accession_id
from
	genomic_marker gm inner join genomic_marker_citation_map src
		on gm.genomic_marker_id = src.genomic_marker_id
	inner join crop_trait ct
		on gm.crop_id = ct.crop_id
	inner join crop_trait_observation cto
		on cto.crop_trait_id = ct.crop_trait_id
	left join inventory i
		on cto.inventory_id = i.inventory_id
{2}
";

				case "plant_introduction":

					return @"
select
	src.{0},
	src.accession_id
from
	accession src
{2}
";

				case "taxonomy":
					return @"
select
	src.{0},
	src.accession_id
from
	accession src
{2}
";

				case "taxonomy_citation_map":
				case "taxonomy_url":
				case "taxonomy_use":
					return @"
select
	src.{0},
	a.accession_id
from
	accession a inner join {1} src
		on a.taxonomy_id = src.taxonomy_id
{2}
";

				case "taxonomy_author":
					Debug.WriteLine("Table " + tableName + " is not mapped through in AccessionResolver.Resolve()");
					return null;

				case "crop_trait":
					// crop_trait_observation -> crop_trait
					// crop_trait_observation -> accession OR inventory

					return @"
select
	src.{0},
	coalesce(cto.accession_id, i.accession_id) as accession_id
from
	crop_trait_observation src left join inventory i
		on src.inventory_id = i.inventory_id
{2}
";

					

				case "crop_trait_code":
				case "crop_trait_qualifier":
				case "crop_trait_url":
					return @"
select
	src.{0},
	coalesce(cto.accession_id, i.accession_id) as accession_id
from
	crop_trait_observation cto inner join {1} src
		on cto.crop_trait_id = src.crop_trait_id
	left join inventory i
		on cto.inventory_id = i.inventory_id
{2}
";

				case "citation":

					// accession_citation_map
					string sql3 = @"
select
	src.{0},
	src.accession_id
from
	accession_citation_map src
{2}
";

					// taxonomy_genus_citation_map
					sql3 += @"
UNION
select
	src.{0},
	a.accession_id
from
	taxonomy_genus_citation_map src inner join taxonomy_genus tg
		on src.taxonomy_genus_id = tg.taxonomy_genus_id
	inner join taxonomy t
		on tg.taxonomy_genus_id = t.taxonomy_genus_id
	inner join accession a
		on t.taxonomy_id = a.taxonomy_id
{2}
";
					// accession_pedigree
					sql3 += @"
UNION
select
	src.{0},
	src.accession_id
from
	accession_pedigree src
{2}
";

					// taxonomy_citation_map
					sql3 += @"
UNION
select
	src.{0},
	a.accession_id
from
	taxonomy_citation_map src inner join accession a
		on a.taxonomy_id = src.taxonomy_id
{2}
";

					// accession_ipr
					sql3 += @"
UNION
select
	src.{0},
	src.accession_id
from
	accession_ipr src
{2}
";

					return sql3;

				case "geography":
					return @"
select
	src.{0},
	asrc.accession_id
from
	geography src inner join accession_source asrc
		on src.geography_id = asrc.geography_id
{2}
";
	


				case "url":

					return @"
select
	src.{0},
	a.accession_id
from
	taxonomy_url tu inner join url src
		on tu.url_id = src.url_id
	inner join accession a
		on a.taxonomy_id = tu.taxonomy_id
{2}

UNION

select
	src.{0},
	coalesce(cto.accession_id, i.accession_id) as accession_id
from
	crop_trait_url ctu inner join url src
		on ctu.url_id = src.url_id
	inner join crop_trait_observation cto
		on ctu.crop_trait_id = cto.crop_trait_id
	left join inventory i
		on cto.inventory_id = i.inventory_id
{2}
";


				// ========================================================================
				// Tables that must "bubble up" to Inventory_ID
				// ========================================================================
				// (none for accessions)

				// ========================================================================
				// Tables that must "bubble up" to Order_Request_ID
				// ========================================================================
				// (none for accessions)

				// ========================================================================
				// Unmapped tables
				// ========================================================================

				case "accession_voucher_image_map":
				case "code_group":
				case "code_column":
				case "code_rule":
				case "code_value":
				case "code_value_lang":
				case "crop_trait_code_lang":
				case "genomic_annotation":
				case "image":
				case "method_citation_map":
				case "method_map":
				case "region":
				case "site":
				case "taxonomy_genus_type":
				case "taxonomy_germination_rule":
                case "crop_trait_observation_raw":
					Debug.WriteLine("Table " + tableName + " is not mapped for search results in AccessionResolver.Resolve()");
					//		throw new NotImplementedException("Table " + tableName + " is not mapped for search results in AccessionResolver.Resolve()");
					return null;

				case "app_resource":
				case "app_user_item_list":
				case "sys_lang":
				case "sys_perm":
				case "sys_permission_field":
				case "sys_perm_template":
				case "sys_perm_template_map":
				case "sys_dataview":
				case "sys_dataview_field":
				case "sys_dataview_field_lang":
				case "sys_dataview_param":
                case "sys_dataview_filter":
				case "sys_table":
				case "sys_table_field":
                case "sys_table_filter":
				case "sys_user":
				case "sys_user_gui_setting":
				case "sys_user_permission_map":
				case "sys_index":
				case "sys_index_field":
				case "search_index":
				case "search_index_sql":
					// explicitly ignore any of the security tables (sys_*)
					return null;
				default:
					break;
			}

			throw new NotImplementedException("Table " + tableName + " not mapped through GrinGlobal.DatabaseInspector.Resolver.resolveAccession()");
		
		}

		private static string resolveInventory(string tableName) {
			switch (tableName) {
				// ========================================================================
				// Tables with Accession_ID as primary key
				// ========================================================================
				case "accession":
					return @"
select
	src.{0},
	src.inventory_id
from
	inventory src
{2}
";

				// ========================================================================
				// Tables with Inventory_ID as primary key
				// ========================================================================
				case "inventory":
					throw new NotImplementedException("Resolving an inventory from table " + tableName + " makes no sense because it is the same table.  Value is in PrimaryKey.");

				// ========================================================================
				// Tables with Order_Request_ID as primary key
				// ========================================================================
				case "order_request":
					// order_request -> order_request_item
					// order_request_item -> accession OR inventory
					return @"
select 
	src.{0},
	coalesce(src.inventory_id, i.inventory_id) as inventory_id
from 
	order_request_item src left join inventory i 
        on src.accession_id = i.accession_id
{2}
";

				// ========================================================================
				// Tables with Accession_ID only as foreign key
				// ========================================================================
				case "accession_action":
				case "accession_annotation":
				case "accession_citation_map":
				case "accession_habitat":
				case "accession_image_map":
				case "accession_ipr":
				case "accession_name":
				case "accession_narrative":
				case "accession_pedigree":
				case "accession_quarantine":
				case "accession_source":

					return @"
select
	src.{0},
	i.inventory_id
from
	inventory i inner join {1} src
		on i.accession_id = src.accession_id
{2}
";

				case "accession_source_map":
					return @"
select
	src.{0},
	i.inventory_id
from
	accession_source asrc inner join {1} src
		on asrc.accession_source_id = src.accession_source_id
	inner join inventory i
		on asrc.accession_id = i.accession_id
{2}
";


				case "accession_group":
					return @"
select
	src.{0},
	i.inventory_id
from
	accession_name src inner join inventory i
		on i.accession_id = src.accession_id
{2}
";

				// ========================================================================
				// Tables with Inventory_ID only as foreign key
				// ========================================================================
				case "genomic_observation":
				case "inventory_action":
				case "inventory_group_map":
				case "inventory_image_map":
				case "inventory_quality_status":
				case "inventory_viability":
                case "order_request_item":
                case "crop_trait_observation":
					
					throw new NotImplementedException("Resolving an inventory from table " + tableName + " makes no sense because it should contain a foreign key for inventory (meaning you already have the inventory_id)");

				case "inventory_maint_policy":
					return @"
select
	src.{0},
	src.inventory_id
from
	inventory src 
{2}
";

				case "inventory_group":
					return @"
select
	src.{0},
	src.inventory_id
from
	inventory_group_map src
{2}
";


				// ========================================================================
				// Tables with Order_Request_ID only as foreign key
				// ========================================================================
				case "order_request_action":
					// order_action -> order_entry
					// order_entry -> order_item
					//order_item -> accession  OR  inventory
					return @"
select 
	src.{0},
	coalesce(ori.inventory_id, i.inventory_id) as inventory_id
from 
	order_request_item ori inner join order_request_action src
		on ori.order_request_id = src.order_request_id
	left join inventory i
		on ori.accession_id = i.accession_id
{2}
";

				// ========================================================================
				// Tables with both Accession_ID and Inventory_ID as foreign key
				// ========================================================================
				case "accession_voucher":
					throw new NotImplementedException("Resolving an inventory from table " + tableName + " makes no sense because it should contain a foreign key for inventory (meaning you already have the inventory_id)");


				// ========================================================================
				// Tables that must "bubble up" to Accession_ID
				// ========================================================================
				// (none for inventory)


				// ========================================================================
				// Tables that must "bubble up" to Inventory_ID
				// ========================================================================
				case "taxonomy_common_name":
					return @"
select 
	src.{0},
	i.inventory_id
from 
	accession a inner join taxonomy_common_name src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i 
		on a.accession_id = i.accession_id
{2}
";

				case "cooperator":
					//                    // TODO: how do we join across via cooperator???
					return @"
select
	src.source_{0},
	coalesce(ori.inventory_id, i.inventory_id) as inventory_id
from
	order_request_item ori inner join order_request src
		on ori.order_request_id = src.order_request_id
	left join inventory i
		on ori.accession_id = i.accession_id
{2}
";

				case "cooperator_group":
					return @"
select
	src.{0},
	coalesce(ori.inventory_id, i.inventory_id) as inventory_id
from
	cooperator_group src inner join cooperator_map cm
		on src.cooperator_group_id = cm.cooperator_group_id
	inner join order_request oreq
		on oreq.source_cooperator_id = cm.cooperator_id
	inner join order_request_item ori 
		on oreq.order_request_id = ori.order_request_id
	left join inventory i
		on ori.accession_id = i.accession_id
{2}
";
				case "cooperator_map":
					return @"
select
	src.{0},
	coalesce(ori.inventory_id, i.inventory_id) as inventory_id
from
	cooperator_map src inner join order_request oreq
		on oreq.source_cooperator_id = cm.cooperator_id
	inner join order_request_item ori 
		on ori.order_request_id = oreq.order_request_id
	left join inventory i
		on ori.accession_id = i.accession_id
{2}
";

				case "crop":
					return @"
select 
	src.{0},
	i.inventory_id
from 
	accession a inner join taxonomy src 
		on a.taxonomy_id = src.taxonomy_id 
	inner join inventory i
		on a.accession_id = i.accession_id
{2}
";

				case "taxonomy_distribution":
					return @"
select 
	src.{0},
	i.inventory_id
from 
	accession a inner join taxonomy_distribution src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
{2}
";

				case "method":
					// evaluation can tie to many tables, so we must search by
					// all of them.  These include any of the following:


					// accession_action
					// crop_trait_url
					// method_citation_map
					// method_map
					// genomic_annotation
					// inventory_action
					// crop_trait_observation
					// inventory_viability

					// accession_action
					string sql4 = @"
select
	src.{0},
	i.inventory_id
from
	accession_action src 
	inner join inventory i
		on src.accession_id = i.accession_id
{2}
";

					// inventory_action
					//					fillByForeignKey(ret, matches, "inventory_action_inventory_id_att");

					// crop_trait_observation
					//fillByInventoryOrAccession(ret, matches, tableName);

					// inventory_viability
					//        fillBySql(ret, matches, "inventory_viability_inventory_id_att", @"
					//select
					//	i.accession_id
					//from
					//	inventory i
					//where
					//	i.inventory_id in (:idlist)
					//{2}");

					// TODO: crop_trait_url
					// TODO: method_citation_map
					// TODO: method_map
					// TODO: genomic_annotation


					return sql4;

				case "taxonomy_family":
					return @"
select
	src.{0},
	i.inventory_id
from
	taxonomy_genus src inner join taxonomy t
		on src.taxonomy_genus_id = t.taxonomy_genus_id
	inner join accession a
		on t.taxonomy_id = a.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
{2}
";

				case "taxonomy_genus":
					return @"
select
	src.{0},
	i.inventory_id
from
	accession a inner join taxonomy src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
{2}
";

				case "taxonomy_genus_citation_map":
					// genus_citation -> genus
					// taxonomy -> genus
					// accession -> taxonomy
					return @"
select
	src.{0},
	i.inventory_id
from
	accession a inner join taxonomy t
		on a.taxonomy_id = t.taxonomy_id
	inner join taxonomy_genus_citation_map src
		on t.taxonomy_genus_id = src.taxonomy_genus_id
	inner join inventory i
		on a.accession_id = i.accession_id
{2}
";


				case "literature":
					// Literature_ID is found in following tables:
					// * citation
					// which of course, all these tables reference:
					// * accession_citation_map
					// * method_citation_map
					// * taxonomy_genus_citation_map
					// * genomic_marker_citation
					// * taxonomy_citation


					// accession_citation_map
					string sql = @"
select
	src.{0},
	i.inventory_id
from
	accession_citation_map acm inner join citation src
		on acm.citation_id = src.citation_id
	inner join inventory i
		on acm.accession_id = i.accession_id
{2}
";

					// TODO: method_citation_map

					// TODO: taxonomy_genus_citation_map

					// TODO: genomic_marker_citation_map

					// TODO: taxonomy_citation_map


					return sql;


				case "genomic_marker":
					// crop -> marker
					// trait -> crop
					// observation -> trait
					// observation -> accession OR inventory
					return @"
select
	src.{0},
	coalesce(cto.inventory_id, i.inventory_id) as inventory_id
from
	genomic_marker src inner join crop_trait ct
		on src.crop_id = ct.crop_id
	inner join crop_trait_observation cto
		on cto.crop_trait_id = ct.crop_trait_id
	left join inventory i
		on cto.accession_id = i.accession_id
{2}
";

				case "genomic_marker_citation_map":
					// marker_citation -> marker
					// crop -> marker
					// trait -> crop
					// observation -> trait
					// observation -> accession OR inventory
					return @"
select
	src.{0},
	coalesce(cto.inventory_id, i.inventory_id) as inventory_id
from
	genomic_marker gm inner join genomic_marker_citation_map src
		on gm.genomic_marker_id = src.genomic_marker_id
	inner join crop_trait ct
		on gm.crop_id = ct.crop_id
	inner join crop_trait_observation cto
		on cto.crop_trait_id = ct.crop_trait_id
	left join inventory i
		on cto.accession_id = i.accession_id
{2}
";

				case "plant_introduction":

					return @"
select
	src.{0},
	i.inventory_id
from
	accession src inner join inventory i
		on src.accession_id = i.accession_id
{2}
";

				case "taxonomy":
					return @"
select
	src.{0},
	i.inventory_id
from
	accession src inner join inventory i
		on src.accession_id = i.accession_id
{2}
";

				case "taxonomy_citation_map":
				case "taxonomy_author":
				case "taxonomy_url":
				case "taxonomy_use":
					return @"
select
	src.{0},
	i.inventory_id
from
	accession a inner join {1} src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
{2}";

				case "crop_trait":
					// crop_trait_observation -> crop_trait
					// crop_trait_observation -> accession OR inventory

					return @"
select
	src.{0},
	coalesce(src.inventory_id, i.inventory_id) as inventory_id
from
	crop_trait_observation src inner join inventory i
		on src.accession_id = i.accession_id
{2}
";


				case "crop_trait_code":
				case "crop_trait_qualifier":
				case "crop_trait_url":
					return @"
select
	src.{0},
	coalesce(cto.inventory_id, i.inventory_id) as inventory_id
from
	crop_trait_observation cto inner join {1} src
		on cto.crop_trait_id = src.crop_trait_id
	inner join inventory i
		on cto.accession_id = i.accession_id
{2}
";

				case "citation":
					return @"
select
	src.{0},
	i.inventory_id
from
	accession_citation_map src inner join inventory i
		on src.accession_id = i.accession_id
{2}";

				case "geography":
					return @"
select
	src.{0},
	i.inventory_id
from
	geography src inner join accession_source asrc
		on g.geography_id = asrc.geography_id
	inner join inventory i
		on asrc.accession_id = i.accession_id
{2}
";
					

				// ========================================================================
				// Tables that must "bubble up" to Order_Request_ID
				// ========================================================================
				// (none for inventory)

				// ========================================================================
				// Unmapped tables
				// ========================================================================

				case "accession_voucher_image_map":
				case "app_resource":
				case "app_user_item_list":
				case "code_group":
				case "code_column":
				case "code_rule":
				case "code_value":
				case "code_value_lang":
				case "crop_trait_code_lang":
                case "crop_trait_observation_raw":
                case "genomic_annotation":
				case "image":
				case "method_citation_map":
				case "method_map":
				case "region":
				case "site":
				case "taxonomy_genus_type":
				case "taxonomy_germination_rule":
				case "url":
					Debug.WriteLine("Table " + tableName + " is not mapped for search results in InventoryResolver.Resolve()");
					return null;
				//		throw new NotImplementedException("Table " + tableName + " is not mapped for search results in InventoryResolver.Resolve()");

				case "sys_lang":
				case "sys_perm":
				case "sys_permission_field":
				case "sys_perm_template":
				case "sys_perm_template_map":
				case "sys_dataview":
				case "sys_dataview_field":
				case "sys_dataview_field_lang":
				case "sys_dataview_param":
                case "sys_dataview_filter":
                case "sys_table":
				case "sys_table_field":
                case "sys_table_filter":
                case "sys_user":
				case "sys_user_gui_setting":
				case "sys_user_permission_map":
				case "sys_index":
				case "sys_index_field":
				case "search_index":
				case "search_index_sql":
					// explicitly ignore any of the security tables (sys_*)
					return null;
				default:
					break;

			}

			throw new NotImplementedException("Table " + tableName + " not mapped through GrinGlobal.DatabaseInspector.Resolver.resolveInventory()");
		}

		private static string resolveOrder(string tableName) {
			switch (tableName) {
				// ========================================================================
				// Tables with Accession_ID as primary key
				// ========================================================================
				case "accession":
					return @"
select
	src.{0},
	src.order_request_id
from
	order_request_item src
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join inventory src 
		on src.inventory_id = ori.inventory_id
{2}
";

				// ========================================================================
				// Tables with Inventory_ID as primary key
				// ========================================================================
				case "inventory":
					return @"
select
	src.{0},
	src.order_request_id
from
	order_request_item src
{2}";

				// ========================================================================
				// Tables with Order_Request_ID as primary key
				// ========================================================================
				case "order_request":
					// order_request -> order_request_item
					// order_request_item -> accession OR inventory
					throw new NotImplementedException("Resolving an order from table " + tableName + " makes no sense because it is the same table.  Value is in PrimaryKey.");

				// ========================================================================
				// Tables with Accession_ID only as foreign key
				// ========================================================================
				case "accession_action":
				case "accession_annotation":
				case "accession_citation_map":
				case "accession_habitat":
				case "accession_image_map":
				case "accession_ipr":
				case "accession_name":
				case "accession_narrative":
				case "accession_pedigree":
				case "accession_quarantine":
				case "accession_source":

					return @"
select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join {1} src
		on ori.accession_id = src.accession_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join {1} src
		on ori.accession_id = src.accession_id
	inner join inventory i
		on ori.inventory_id = i.inventory_id
{2}";

				case "accession_source_map":
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession_source asrc inner join {1} src
		on asrc.accession_source_id = src.accession_source_id
	inner join inventory i
		on asrc.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}
";

				case "accession_group":
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession_name src inner join inventory i	
		on src.accession_id = i.inventory_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
";

				// ========================================================================
				// Tables with Inventory_ID only as foreign key
				// ========================================================================
				case "genomic_observation":
				case "inventory_action":
				case "inventory_group_map":
				case "inventory_image_map":
				case "inventory_quality_status":
				case "inventory_viability":
					return @"
select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join {1} src
		on ori.inventory_id = src.inventory_id
{2}";

				case "inventory_maint_policy":
					return @"
select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join inventory src
		on ori.inventory_id = src.inventory_id
{2}";


	
				case "inventory_group":
					return @"
select
	src.{0},
	ori.order_request_id
from
	inventory_group_map src inner join order_request_item ori
		on src.inventory_id = ori.inventory_id
{2}
";

                case "order_request_item":
                    return @"
select
    src.{0},
    src.order_request_id
from
    order_request_item src 
{2}
";
                case "crop_trait_observation":
                    return @"
select
    src.{0},
    ori.order_request_id
from
    crop_trait_observation src inner join order_request_item ori
        on src.inventory_id = ori.inventory_id
{2}
";
					


				// ========================================================================
				// Tables with Order_Request_ID only as foreign key
				// ========================================================================
				case "order_request_action":
					// order_action -> order_entry
					// order_entry -> order_item
					//order_item -> accession  OR  inventory
					throw new NotImplementedException("Resolving an order from table " + tableName + " makes no sense because it should contain a foreign key for order_request (meaning you already have the order_request_id)");

				// ========================================================================
				// Tables with both Accession_ID and Inventory_ID as foreign key
				// ========================================================================
				case "accession_voucher":
					return @"
select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join {1} src
		on ori.accession_id = src.accession_id
{2}
UNION
select
	src.{0},
	ori.order_request_id
from
	order_request_item ori inner join {1} src
		on ori.accession_id = src.accession_id
{2}";

				// ========================================================================
				// Tables that must "bubble up" to Accession_ID
				// ========================================================================
				// (none for orders)

				// ========================================================================
				// Tables that must "bubble up" to Inventory_ID
				// ========================================================================
				// (none for orders)

				// ========================================================================
				// Tables that must "bubble up" to Order_Request_ID
				// ========================================================================

				case "taxonomy_common_name":
					return @"
select 
	src.{0},
	ori.order_request_id
from 
	accession a inner join taxonomy_common_name src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i 
		on a.accession_id = i.accession_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
UNION

select 
	src.{0},
	ori.order_request_id
from 
	accession a inner join taxonomy_common_name src
		on a.taxonomy_id = tcn.taxonomy_id
	inner join order_request_item ori
		on a.accession_id = ori.accession_id
{2}";

				case "cooperator":
					//                    // TODO: how do we join across via cooperator???
					return @"
select
	src.source_{0},
	ori.order_request_id
from
	order_request_item ori inner join order_request src
		on ori.order_request_id = src.order_request_id
{2}";

				case "cooperator_group":
					return @"
select
	src.{0},
	oreq.order_request_id
from
	cooperator_group src inner join cooperator_map cm
		on src.cooperator_group_id = cm.cooperator_group_id
	inner join order_request oreq
		on oreq.source_cooperator_id = cm.cooperator_id
{2}";

				case "cooperator_map":
					return @"
select
	src.{0},
	oreq.order_request_id
from
	cooperator_map src inner join order_request oreq
		on oreq.source_cooperator_id = src.cooperator_id
{2}";

				case "crop":
					return @"
select 
	src.{0},
	ori.order_request_id
from 
	accession a inner join taxonomy src 
		on a.taxonomy_id = src.taxonomy_id 
	inner join order_request_item ori
		on a.accession_id = ori.accession_id
{2}
UNION

select 
	src.{0},
	ori.order_request_id
from 
	accession a inner join taxonomy src 
		on a.taxonomy_id = src.taxonomy_id 
	inner join inventory i
		on a.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}";

				case "taxonomy_distribution":
					return @"
select 
	src.{0},
	ori.order_request_id
from 
	accession a inner join taxonomy_distribution src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
	inner join order_request_item ori 
		on ori.inventory_id = i.inventory_id
{2}";

				case "method":
					// evaluation can tie to many tables, so we must search by
					// all of them.  These include any of the following:


					// accession_action
					// crop_trait_url
					// method_citation_map
					// method_map
					// genomic_annotation
					// inventory_action
					// crop_trait_observation
					// inventory_viability

					// accession_action
					string sql = @"
select
	src.{0},
	ori.order_request_id
from
	accession_action src inner join inventory i
		on src.accession_id = i.accession_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	accession_action src inner join order_request_item ori
		on ori.accession_id = src.accession_id
{2}";

					// inventory_action
					//					fillByForeignKey(ret, matches, "inventory_action_inventory_id_att");

					// crop_trait_observation

					// inventory_viability
					//        fillBySql(ret, matches, "inventory_viability_inventory_id_att", @"
					//select
					//	i.accession_id
					//from
					//	inventory i
					//where
					//	i.inventory_id in (:idlist)
					//{2}");

					// TODO: crop_trait_url
					// TODO: method_citation_map
					// TODO: method_map
					// TODO: genomic_annotation

					return sql;

				case "taxonomy_family":
					return @"
select
	src.{0},
	ori.order_request_id
from
	taxonomy_genus src inner join taxonomy t
		on src.taxonomy_genus_id = t.taxonomy_genus_id
	inner join accession a
		on t.taxonomy_id = a.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	taxonomy_genus src inner join taxonomy t
		on src.taxonomy_genus_id = t.taxonomy_genus_id
	inner join accession a
		on t.taxonomy_id = a.taxonomy_id
	inner join order_request_item ori
		on ori.accession_id = a.accession_id
{2}
";

				case "taxonomy_genus":
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession a inner join taxonomy src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
";

				case "taxonomy_genus_citation_map":
					// genus_citation -> genus
					// taxonomy -> genus
					// accession -> taxonomy
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession a inner join taxonomy t
		on a.taxonomy_id = t.taxonomy_id
	inner join taxonomy_genus_citation_map src
		on t.taxonomy_genus_id = src.taxonomy_genus_id
	inner join inventory i
		on a.accession_id = i.accession_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	accession a inner join taxonomy t
		on a.taxonomy_id = t.taxonomy_id
	inner join taxonomy_genus_citation_map src
		on t.taxonomy_genus_id = src.taxonomy_genus_id
	inner join order_request_item ori
		on ori.accession_id = a.accession_id
{2}
";


				case "literature":
					// Literature_ID is found in following tables:
					// * citation
					// which of course, all these tables reference:
					// * accession_citation_map
					// * method_citation_map
					// * taxonomy_genus_citation_map
					// * genomic_marker_citation
					// * taxonomy_citation


					// accession_citation_map
					string sql2 = @"
select
	src.{0},
	ori.order_request_id
from
	accession_citation_map acm inner join citation src
		on acm.citation_id = src.citation_id
	inner join inventory i
		on acm.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	accession_citation_map acm inner join citation src
		on acm.citation_id = src.citation_id
	inner join order_request_item ori
		on ori.accession_id = acm.accession_id
{2}
";

					// TODO: method_citation_map

					// TODO: taxonomy_genus_citation_map

					// TODO: genomic_marker_citation_map

					// TODO: taxonomy_citation_map

					return sql2;

				case "genomic_marker":
					// crop -> marker
					// trait -> crop
					// observation -> trait
					// observation -> accession OR inventory
					return @"
select
	src.{0},
	ori.order_request_id
from
	genomic_marker src inner join crop_trait ct
		on src.crop_id = ct.crop_id
	inner join crop_trait_observation cto
		on cto.crop_trait_id = ct.crop_trait_id
	inner join order_request_item ori
		on (cto.accession_id = ori.accession_id or cto.inventory_id = ori.inventory_id)
{2}
";

				case "genomic_marker_citation_map":
					// marker_citation -> marker
					// crop -> marker
					// trait -> crop
					// observation -> trait
					// observation -> accession OR inventory
					return @"
select
	src.{0},
	ori.order_request_id
from
	genomic_marker gm inner join genomic_marker_citation_map src
		on gm.genomic_marker_id = src.genomic_marker_id
	inner join crop_trait ct
		on gm.crop_id = ct.crop_id
	inner join crop_trait_observation cto
		on cto.crop_trait_id = ct.crop_trait_id
	inner join order_request_item ori
		on (cto.inventory_id = ori.inventory_id or cto.accession_id = ori.accession_id)
{2}";

				case "plant_introduction":

					return @"
select
	src.{0},
	ori.order_request_id
from
	accession src inner join inventory i
		on src.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	accession src inner join order_request_item ori
		on src.accession_id = ori.accession_id
{2}";

				case "taxonomy":
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession src inner join inventory i
		on src.accession_id = i.accession_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
";


//UNION

//select
//    a.{0},
//    ori.order_request_id
//from
//    accession a inner join order_request_item ori
//        on a.accession_id = ori.accession_id
//where
//    a.taxonomy_id in (:idlist)
//{2}
//";

				case "taxonomy_citation_map":
				case "taxonomy_author":
				case "taxonomy_url":
				case "taxonomy_use":
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession a inner join {1} src
		on a.taxonomy_id = src.taxonomy_id
	inner join inventory i
		on a.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}
";

				case "crop_trait":
					// crop_trait_observation -> crop_trait
					// crop_trait_observation -> accession OR inventory

					return @"
select
	src.{0},
	ori.order_request_id
from
	crop_trait_observation src inner join order_request_item ori
		on src.inventory_id = ori.inventory_id
{2}";

				case "crop_trait_code":
				case "crop_trait_qualifier":
				case "crop_trait_url":
					return @"
select
	src.{0},
	ori.order_request_id
from
	crop_trait_observation cto inner join {1} src
		on cto.crop_trait_id = src.crop_trait_id
	inner join order_request_item ori
		on cto.inventory_id = ori.inventory_id
{2}";

				case "citation":
					return @"
select
	src.{0},
	ori.order_request_id
from
	accession_citation_map src inner join inventory i
		on src.accession_id = i.accession_id
	inner join order_request_item ori
		on ori.inventory_id = i.inventory_id
{2}
UNION

select
	src.{0},
	ori.order_request_id
from
	accession_citation_map src inner join order_request_item ori
		on ori.accession_id = src.accession_id
{2}";

				case "geography":
					return @"
select
	src.{0},
	ori.order_request_id
from
	geography src inner join accession_source asrc
		on src.geography_id = asrc.geography_id
	inner join inventory i
		on asrc.accession_id = i.accession_id
	inner join order_request_item ori
		on i.inventory_id = ori.inventory_id
{2}
";


				// ========================================================================
				// Unmapped tables
				// ========================================================================

				case "accession_voucher_image_map":
				case "app_resource":
				case "app_user_item_list":
				case "code_column":
				case "code_group":
				case "code_rule":
				case "code_value":
				case "code_value_lang":
				case "crop_trait_code_lang":
                case "crop_trait_observation_raw":
                case "genomic_annotation":
				case "image":
				case "method_citation_map":
				case "method_map":
				case "region":
				case "site":
				case "taxonomy_genus_type":
				case "taxonomy_germination_rule":
				case "url":
					Debug.WriteLine("Table " + tableName + " is not mapped for search results in OrderResolver.Resolve()");
					return null;
				//		throw new NotImplementedException("Table " + tableName + " is not mapped for search results in OrderResolver.Resolve()");
				case "sys_lang":
				case "sys_perm":
				case "sys_permission_field":
				case "sys_perm_template":
				case "sys_perm_template_map":
				case "sys_dataview":
				case "sys_dataview_field":
				case "sys_dataview_field_lang":
				case "sys_dataview_param":
                case "sys_dataview_filter":
                case "sys_table":
				case "sys_table_field":
                case "sys_table_filter":
                case "sys_user":
				case "sys_user_gui_setting":
				case "sys_user_permission_map":
				case "sys_index":
				case "sys_index_field":
				case "search_index":
				case "search_index_sql":
					// explicitly ignore any of the security tables (sys_*)
					return null;

				default:
					break;

			}

			throw new NotImplementedException("Table " + tableName + " not mapped through GrinGlobal.DatabaseInspector.Resolver.resolveOrder()");
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "Resolver", resourceName, null, defaultValue, substitutes);
        }
    }
}
