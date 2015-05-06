using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class AccessionIPRDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) Make sure at least 1 of number, fullname is filled in
                if (helper.AllFieldsExist("ipr_number", "ipr_full_name") && helper.IsValueEmpty("ipr_number") && helper.IsValueEmpty("ipr_full_name")) {
                    args.Cancel("Incomplete accession_ipr, must have either ipr_number or ipr_full_name.");
                }

                // 2) do not allow expired_date > current date
                if (helper.FieldExists("expired_date") && !helper.IsValueEmpty("expired_date")
                    && Convert.ToDateTime(helper.GetValue("expired_date", null, true)) > DateTime.UtcNow) {
                    args.Cancel("Expired date cannot exceed the current date.");
                }

                // 3) do not allow issued date > expired date
                if (helper.AllFieldsExist("issued_date", "expired_date") && !helper.IsValueEmpty("issued_date") && !helper.IsValueEmpty("expired_date")) {
                    DateTime issued = Convert.ToDateTime(helper.GetValue("issued_date", null, true));
                    DateTime expired = Convert.ToDateTime(helper.GetValue("expired_date", null, true));
                    if (expired < issued) {
                        args.Cancel("Issued date cannot exceed expired date.");
                    }
                }

            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Accession IPR.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "AccessionIPR Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "acession_ipr" };
            }
        }
    }
}