using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class AccessionDataTrigger : TableDataTriggerAdapter {
        int oldtsid = 0;
        int newtsid = 0;
        bool tsidChanged = false;

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                int tsid = (int)helper.GetValue("taxonomy_species_id", 0, true);
                var dtTax = args.ReadData(@"SELECT current_taxonomy_species_id, synonym_code, life_form_code FROM taxonomy_species WHERE taxonomy_species_id = :tsid",
                    ":tsid", tsid, DbType.Int32);
                if (dtTax.Rows.Count < 1) {
                    args.Cancel("Taxonomy could not be validated.");
                } else {
                    int currentid = (int)dtTax.Rows[0]["current_taxonomy_species_id"];

                    // 1) prevent link to invalid species (e.g. synonym)
                    if (tsid != currentid) {
                        oldtsid = tsid;
                        newtsid = currentid;
                        tsidChanged = true;

                        helper.SetValue("taxonomy_species_id", currentid, typeof(int), false);
                        dtTax = args.ReadData(@"SELECT current_taxonomy_species_id, synonym_code, life_form_code FROM taxonomy_species WHERE taxonomy_species_id = :tsid",
                            ":tsid", currentid, DbType.Int32);
                    }

                    // 2) set life_form_code from tax if null
                    helper.SetValueIfFieldExistsAndIsEmpty("life_form_code", dtTax.Rows[0]["life_form_code"]);

                    // 4) set received date if null (sysdate)
                    helper.SetValueIfFieldExistsAndIsEmpty("initial_received_date", DateTime.UtcNow);
                    helper.SetValueIfFieldExistsAndIsEmpty("initial_received_date_code", "MM/dd/yyyy");
                    helper.SetValueIfFieldExistsAndIsEmpty("status_code", "ACTIVE");
                }
            }
        }


        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger
            if (args.SaveMode == SaveMode.Insert && tsidChanged) {
                CreateAnnotation(args, args.NewPrimaryKeyID, oldtsid, newtsid, "RECEIVED");
            //} else if (args.SaveMode == SaveMode.Update && tsidChanged) {
            //    CreateAnnotation(args, args.NewPrimaryKeyID, oldtsid, newtsid, "RE-IDENT");
            }
        }

        public void CreateAnnotation(ISaveDataTriggerArgs args, int acid, int oldtsid, int newtsid, string annoType) {
            args.WriteData(@"
INSERT INTO accession_inv_annotation
    (annotation_type_code,
    annotation_date,
    annotation_date_code,
    annotation_cooperator_id,
    inventory_id,
    old_taxonomy_species_id,
    new_taxonomy_species_id,
    created_date,
    created_by,
    owned_date,
    owned_by)
SELECT
    @annotype,
    COALESCE(a.modified_date, a.created_date),
    'MM/dd/yyyy',
    COALESCE(a.modified_by, a.created_by),
    i.inventory_id,
    @oldtsid,
    @newtsid,
    COALESCE(a.modified_date, a.created_date),
    COALESCE(a.modified_by, a.created_by),
    COALESCE(a.modified_date, a.created_date),
    a.owned_by
FROM accession a
    INNER JOIN inventory i ON a.accession_id = i.accession_id AND i.form_type_code = '**'
WHERE a.accession_id = @acid
",
            new DataParameters("@acid", acid, DbType.Int32,
                               "@oldtsid", oldtsid, DbType.Int32,
                               "@newtsid", newtsid, DbType.Int32,
                               "@annotype", annoType, DbType.String));

        }


        public override string GetDescription(string ietfLanguageTag) {
            return "Checks accession for valid taxonomy and if null set initial_received_date and life_form_code.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Accession Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "accession" };
            }
        }
    }

    public class AccessionNonPIDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger

            if (args.SaveMode == SaveMode.Insert) {
                //args.Cancel("Got this far in non-pi trigger.");
                var helper = args.Helper;

                // 5) create an accession_inv_name where prefix is not PI
                string aiName = helper.GetValue("accession_number_part1", "", true).ToString();
                if (aiName != "PI") {
                    if (!helper.IsValueEmpty("accession_number_part2")) {
                        //aiName += " number here";
                        aiName += " " + helper.GetValue("accession_number_part2", null, true);
                    }
                    if (!helper.IsValueEmpty("accession_number_part3")) {
                        aiName += " " + helper.GetValue("accession_number_part3", null, true);
                    }

                    args.WriteData(@"
INSERT INTO accession_inv_name
    (inventory_id,
    category_code,
    plant_name,
    plant_name_rank,
    name_group_id,
    created_date,
    created_by,
    modified_date,
    modified_by,
    owned_date,
    owned_by)
SELECT
    i.inventory_id,
    'SITE' AS 'category_code',
    @name AS plant_name,
    1080 AS 'plant_name_rank',
    ng.name_group_id,
    a.created_date,
    a.created_by,
    a.modified_date,
    a.modified_by,
    a.owned_date,
    a.owned_by
FROM accession a
    INNER JOIN inventory i ON a.accession_id = i.accession_id AND i.form_type_code = '**'
    LEFT JOIN name_group ng ON a.accession_number_part1 = ng.group_name
WHERE a.accession_id = @acid
",
            new DataParameters("@acid", args.NewPrimaryKeyID, DbType.Int32,
                               "@name", aiName, DbType.String));

                // timing check
                //args.WriteData(@"UPDATE accession SET note = note + 'Additional data trigger was here.' WHERE accession_id = " + args.NewPrimaryKeyID);

                }
            }
        }

        #region IAsyncDataTrigger Members
        /*
        public bool IsAsynchronous {
            get { return false; }
        }

        public object Clone() {
            return new AccessionNonPIDataTrigger();
        }
        */
        #endregion

        public override string GetDescription(string ietfLanguageTag) {
            return "Create a accession_inv_name row for Non-PI accession inserts.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Accession Non-PI Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "accession" };
            }
        }
    }

    public class AccessionReidDataTrigger : TableDataTriggerAdapter {
        int oldtsid = 0;
        int newtsid = 0;
        bool tsidChanged = false;

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Update) {
               var helper = args.Helper;
                
                oldtsid = (int)helper.GetOriginalValue("taxonomy_species_id", 0);
                newtsid = (int)helper.GetValue("taxonomy_species_id", 0, true);

                if (oldtsid != newtsid) tsidChanged = true;
            }
        }

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger
            if (args.SaveMode == SaveMode.Update && tsidChanged) {

                // 6) Create a RE-IDENT annotation_label row if species changed
                CreateAnnotation(args, args.NewPrimaryKeyID, oldtsid, newtsid, "RE-IDENT");
            }
        }


        public override string GetDescription(string ietfLanguageTag) {
            return "Creates Accession Re-ID annotation if necessary.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Accession ReID Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "acession" };
            }
        }

        public void CreateAnnotation(ISaveDataTriggerArgs args, int acid, int oldtsid, int newtsid, string annoType) {
            args.WriteData(@"
INSERT INTO accession_inv_annotation
    (annotation_type_code,
    annotation_date,
    annotation_date_code,
    annotation_cooperator_id,
    inventory_id,
    old_taxonomy_species_id,
    new_taxonomy_species_id,
    created_date,
    created_by,
    owned_date,
    owned_by)
SELECT
    @annotype,
    COALESCE(a.modified_date, a.created_date),
    'MM/dd/yyyy',
    COALESCE(a.modified_by, a.created_by),
    i.inventory_id,
    @oldtsid,
    @newtsid,
    COALESCE(a.modified_date, a.created_date),
    COALESCE(a.modified_by, a.created_by),
    COALESCE(a.modified_date, a.created_date),
    a.owned_by
FROM accession a
    INNER JOIN inventory i ON a.accession_id = i.accession_id AND i.form_type_code = '**'
WHERE a.accession_id = @acid
",
            new DataParameters("@acid", acid, DbType.Int32,
                               "@oldtsid", oldtsid, DbType.Int32,
                               "@newtsid", newtsid, DbType.Int32,
                               "@annotype", annoType, DbType.String));

        }

    }

}
