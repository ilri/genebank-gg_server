using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class AccessionInvAnnotationDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) on type ID-CHECK require cooperator link
                // GC check if cno is filled on an ID-CHECK
                if (helper.AllFieldsExist("annotation_type_code", "annotation_cooperator_id") && helper.GetValue("annotation_type_code", "", true).ToString().ToUpper() == "ID-CHECK" && helper.IsValueEmpty("annotation_cooperator_id")) {
                    args.Cancel("Cooperator must be supplied for ID-CHECK.");
                }

                // 2) Null out order number if type NOM-CHANGE
                if (helper.AllFieldsExist("annotation_type_code", "order_request_id") && helper.GetValue("annotation_type_code", "", true).ToString().ToUpper() == "NOM-CHANGE" && !helper.IsValueEmpty("order_request_id")) {
                    helper.SetValue("order_request_id", DBNull.Value, typeof(int), false);
                }

                // 3) set oldtaxno to current acc species name when type = 'Re-IDENT' and oldtaxno is null
                if (helper.AllFieldsExist("annotation_type_code", "old_taxonomy_species_id") && helper.GetValue("annotation_type_code", "", true).ToString().ToUpper() == "RE-IDENT" && helper.IsValueEmpty("old_taxonomy_species_id")) {
                    int ivid = (int)helper.GetValue("inventory_id", 0, true);                
                    var dtAcc = args.ReadData(@"SELECT taxonomy_species_id FROM accession a INNER JOIN inventory i ON a.accession_id = i.accession_id WHERE inventory_id = :ivid",
                    ":ivid", ivid, DbType.Int32);
                    if (dtAcc.Rows.Count > 0) {
                        helper.SetValueIfFieldExistsAndIsEmpty("old_taxonomy_species_id", dtAcc.Rows[0]["taxonomy_species_id"]);
                    }
                }

                //4) newtaxno must be a valid taxon on RE-IDENT action
                if (helper.AllFieldsExist("annotation_type_code", "new_taxonomy_species_id")
                    && helper.GetValue("annotation_type_code", "", true).ToString().ToUpper() == "RE-IDENT"
                    && !helper.IsValueEmpty("new_taxonomy_species_id")) {

                    int tsid = (int)helper.GetValue("new_taxonomy_species_id", 0, true);
                    var dtTax = args.ReadData(@"SELECT current_taxonomy_species_id, synonym_code FROM taxonomy_species WHERE taxonomy_species_id = :tsid AND taxonomy_species_id = current_taxonomy_species_id",
                        ":tsid", tsid, DbType.Int32);
                    if (dtTax.Rows.Count < 1) {
                        args.Cancel("New taxonomy must be valid for RE-IDENT.");
                    }
                }

                // 5) on type ID-CHECK set oldtaxno to current acc taxon
                if (helper.AllFieldsExist("annotation_type_code", "old_taxonomy_species_id")
                    && helper.GetValue("annotation_type_code", "", true).ToString().ToUpper() == "ID-CHECK") {

                    int ivid = (int)helper.GetValue("inventory_id", 0, true);
                    var dtAcc = args.ReadData(@"SELECT taxonomy_species_id FROM accession a INNER JOIN inventory i ON a.accession_id = i.accession_id WHERE inventory_id = :ivid",
                    ":ivid", ivid, DbType.Int32);
                    if (dtAcc.Rows.Count > 0) {
                        helper.SetValueIfFieldExistsAndIsEmpty("old_taxonomy_species_id", dtAcc.Rows[0]["taxonomy_species_id"]);
                    }
                }

            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Accession Inv Annotation.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "AccessionInvAnnotation Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "acession_inv_annotation" };
            }
        }
    }
}