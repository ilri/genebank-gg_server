using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class AccessionQuarantineDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) make sure expected date > established date
                /* if (helper.AllFieldsExist("expected_release_date", "established_date") && !helper.IsValueEmpty("expected_release_date") && !helper.IsValueEmpty("established_date")) {
                    DateTime expected = Convert.ToDateTime(helper.GetValue("expected_release_date", null, true));
                    DateTime established = Convert.ToDateTime(helper.GetValue("established_date", null, true));
                    if (expected < established) {
                        args.Cancel("Expected date must exceed or equal the date established.");
                    }
                }
                */
                if (DateIsBefore(args, "expected_release_date", "established_date")) args.Cancel("Expected date must exceed or equal the date established.");

                // 2) make sure expected date > entered date
                if (DateIsBefore(args, "expected_release_date", "entered_date")) args.Cancel("Expected date must exceed or equal the entered date.");

                // 3) make sure released date > entered date
                if (DateIsBefore(args, "released_date", "entered_date")) args.Cancel("Released date must exceed or equal the entered date.");

                // 4) make sure released date > established date
                if (DateIsBefore(args, "released_date", "established_date")) args.Cancel("Released date must exceed or equal the established date.");

                // 5) make sure released date <= current date
                if (helper.FieldExists("released_date") && !helper.IsValueEmpty("released_date")
                    && Convert.ToDateTime(helper.GetValue("released_date", null, true)) > DateTime.UtcNow) {
                    args.Cancel("released_date cannot exceed the current date.");
                }

            }
        }

        /// <summary>
        /// Returns true if both field esist and aren't null and the first is before the second
        /// </summary>
        /// <param name="args"></param>
        /// <param name="firstDate"></param>
        /// <returns></returns>
        private bool DateIsBefore(ISaveDataTriggerArgs args, string firstDate, string secondDate) {
            var helper = args.Helper;
            if (helper.AllFieldsExist(firstDate, secondDate) && !helper.IsValueEmpty(firstDate) && !helper.IsValueEmpty(secondDate)
                && Convert.ToDateTime(helper.GetValue(firstDate, null, true)) < Convert.ToDateTime(helper.GetValue(secondDate, null, true)) ) {
                return true;
            } else {
                return false;
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Accession Quarantine Dates.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "AccessionQuarantine Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "acession_quarantine" };
            }
        }
    }
}