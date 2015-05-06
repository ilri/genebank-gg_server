using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.Business.DataTriggers {
    public class AccessionDataTrigger :ITableSaveDataTrigger  {
        #region ITableSaveDataTrigger Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.Table != null && args.Table.TableName.ToLower() == "accession") {
                if (args.SaveMode == SaveMode.Delete) {

                    // if the only record left is the default inventory association record, we need to auto-delete it before the accession is deleted.
                    var h = args.Helper;
                    var accessionID = (int)(h.GetOriginalValue("accession_id", -1));

                    DataTable dt = args.ReadData("select form_type_code, inventory_id from inventory where accession_id = :accessionid", 
                        new DataParameters(":accessionid", accessionID, DbType.Int32));
                    int inventoryIdToDelete = -1;
                    if (dt.Rows.Count == 1) {
                        if (dt.Rows[0]["form_type_code", DataRowVersion.Original].ToString() == "**") {
                            inventoryIdToDelete = Toolkit.ToInt32(dt.Rows[0]["inventory_id", DataRowVersion.Original], -1);
                        }
                    }

                    if (inventoryIdToDelete > -1) {
                        // only 1 inventory record exists and it's a '**' (the default record we initially auto-created when the accession was created).
                        // try to delete it (if there are other records which are pointing at it, this will bomb, which is exactly what we want)
                        args.WriteData("delete from inventory where inventory_id = :inventoryid", new DataParameters(":inventoryid", inventoryIdToDelete, DbType.Int32));
                    }

                }
            }
        }

        private int getSystemCooperatorID(ISaveDataTriggerArgs args) {
            var cm = CacheManager.Get("SystemCooperatorID");
            var sysCoopID = Toolkit.ToInt32(cm["sys_cooperator_id"], -1);
            if (sysCoopID < 0) {
                var dt = args.ReadData("select cooperator_id from cooperator where last_name = 'SYSTEM'");
                if (dt.Rows.Count < 1) {
                    sysCoopID = -1;
                } else {
                    sysCoopID = Toolkit.ToInt32(dt.Rows[0]["cooperator_id"], -1);
                }
                cm["sys_cooperator_id"] = sysCoopID;
            }
            return sysCoopID;
        }

        private int getSystemMaintenancePolicyID(ISaveDataTriggerArgs args) {
            var cm = CacheManager.Get("InventoryMaintPolicy");

            var sysPolicyID = Toolkit.ToInt32(cm["sys_ID"], -1);

            if (sysPolicyID < 1) {
                var dt = args.ReadData("select inventory_maint_policy_id from inventory_maint_policy where maintenance_name = 'SYSTEM'");
                if (dt.Rows.Count == 0) {

                    var sysCoopID = getSystemCooperatorID(args);

                    // the "SYSTEM" maint policy does not exist.  create it.
                    sysPolicyID = args.WriteData(@"
insert into 
    inventory_maint_policy 
(maintenance_name, form_type_code, is_auto_deducted, distribution_default_form_code, created_date, created_by, owned_date, owned_by) 
    values 
('SYSTEM', '**', 'N', '**', :now1, :who1, :now2, :who2)
", true, "inventory_maint_policy_id", ":now1", DateTime.UtcNow, DbType.DateTime2, ":who1", sysCoopID, DbType.Int32, ":now2", DateTime.UtcNow, DbType.DateTime2, ":who2", sysCoopID, DbType.Int32);


                    // throw new InvalidOperationException("There is no record in the inventory_maint_policy table with maintenance_name = 'SYSTEM'.  This record is required by the GRIN-Global middle tier to function properly.");
                } else {
                    sysPolicyID = Toolkit.ToInt32(dt.Rows[0]["inventory_maint_policy_id"], -1);
                }
                cm["sys_ID"] = sysPolicyID;
            }

            return sysPolicyID;

        }

        public void TableRowSaved(ISaveDataTriggerArgs args) {
            if (args.Table != null && args.Table.TableName.ToLower() == "accession") {

                    // tell the taxonomy caches to refresh themselves, they're possibly out of date now
                    CacheManager.Get("Taxonomy").Clear();

                    if (args.SaveMode == SaveMode.Insert) {

                        // lookup id of 'SYSTEM' maint policy record as needed
                        var sysPolicyID = getSystemMaintenancePolicyID(args);

                        var dth = args.Helper;

                        var dtAcc = args.ReadData("select * from accession where accession_id = " + args.NewPrimaryKeyID);

                        if (dtAcc.Rows.Count > 0) {
                            var dr = dtAcc.Rows[0];

                            // we need to create a default inventory record to go along with the accession
                            args.WriteData(@"
insert into inventory
(
    inventory_number_part1,
	inventory_number_part2,
	inventory_number_part3,
	form_type_code,
    inventory_maint_policy_id,
	is_distributable,
    is_available,
    availability_status_code,
	is_auto_deducted,
	accession_id,
	note,
	created_date,
	created_by,
	owned_date,
	owned_by
) values (
    @part1,
    @part2,
    @part3,
    '**',
    @maintpolid,
    'N',
    'N',
    'NOT-SET',
    'N',
    @accessionid,
    'Default Association Record for Accession -> Inventory',
    @createddate,
    @createdby,
    @owneddate,
    @ownedby
)
", new DataParameters(
             "@part1", dr["accession_number_part1"], DbType.String,
             "@part2", dr["accession_number_part2"], DbType.Int32,
             "@part3", dr["accession_number_part3"], DbType.String,
             "@maintpolid", sysPolicyID, DbType.Int32,
             "@accessionid", args.NewPrimaryKeyID, DbType.Int32,
             "@createddate", DateTime.UtcNow, DbType.DateTime2,
             "@createdby", args.CooperatorID, DbType.Int32,
             "@owneddate", DateTime.UtcNow, DbType.DateTime2,
             "@ownedby", dr["owned_by"], DbType.Int32
         ));


                            //                        // HACK: set owned_by to curator1 or curator2 if available...
                            //                        var dtCurator = args.ReadData(@"
                            //select
                            //    coalesce(ts.curator1_cooperator_id, ts.curator2_cooperator_id, -1) as curator_id
                            //from
                            //    taxonomy_species ts inner join accession a
                            //on
                            //    ts.taxonomy_species_id = a.taxonomy_species_id
                            //where
                            //    a.accession_id = " + args.NewPrimaryKeyID);

                            //                        if (dtCurator.Rows.Count > 0) {
                            //                            var curatorID = Toolkit.ToInt32(dtCurator.Rows[0]["curator_id"], -1);
                            //                            if (curatorID > -1) {
                            //                                args.WriteData(@"
                            //update
                            //    accession
                            //set
                            //    owned_by = " + curatorID + " where accession_id = " + args.NewPrimaryKeyID);
                            //                            }
                            //                        }


                        }


                }
            }
        }

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
        }

        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            // nothing to do
        }

        #endregion

        #region IAsyncDataTrigger Members

        public bool IsAsynchronous {
            get { return false; }
        }

        public object Clone() {
            return new AccessionDataTrigger();
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Auto-creates the default inventory record (form_type_code = '**') when a new accession is created.  Auto-deletes the default inventory record when an accession is deleted and it is the only inventory record associated with the accession.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Accession Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return new string[] { "accession" }; }
        }

        #endregion
    }
}
