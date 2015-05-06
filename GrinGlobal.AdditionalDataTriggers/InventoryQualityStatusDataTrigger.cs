using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class InventoryQualityStatusDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) check date order
                if (helper.AllFieldsExist("started_date", "completed_date") && !helper.IsValueEmpty("started_date") && !helper.IsValueEmpty("completed_date")) {
                    DateTime started = Convert.ToDateTime(helper.GetValue("started_date", null, true));
                    DateTime completed = Convert.ToDateTime(helper.GetValue("completed_date", null, true));
                    if (completed < started) {
                        args.Cancel("Completed date must be after started date.");
                    }
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Inventory Quality Status date order.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "InventoryQualityStatus Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "inventory_quality_status" };
            }
        }
    }
}
