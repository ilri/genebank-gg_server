using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class InventoryViabilityDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                if (args.Helper.IsValueEmpty("percent_viable")) {
                    args.WriteData("update inventory_viability set percent_viable = percent_dormant + percent_normal where inventory_viability_id = " + args.NewPrimaryKeyID);
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Sets percent_viable = percent_dormant + percent_normal if percent_viable is not given.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Inventory Viability Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "inventory_viability" };
            }
        }
    }
}
