using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class AccessionPedigreeDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) Make sure at least released date or pedigree itself entered
                if (helper.AllFieldsExist("description", "released_date") && helper.IsValueEmpty("description") && helper.IsValueEmpty("released_date")) {
                    args.Cancel("Incomplete accession_pedigree, must have either released_date or description.");
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Accession Pedigree.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "AccessionPedigree Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "acession_pedigree" };
            }
        }
    }
}