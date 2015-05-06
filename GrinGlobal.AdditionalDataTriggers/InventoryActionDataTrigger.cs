using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using System.Globalization;
using System.Threading;

namespace GrinGlobal.AdditionalDataTriggers {
    public class InventoryActionDataTrigger : ITableSaveDataTrigger {
        #region ITableSaveFilter Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            // pre-table save trigger
        }

        public void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger

            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) require units if quantity filled in
                if (helper.AllFieldsExist("quantity", "quantity_unit_code") && !helper.IsValueEmpty("quantity") && helper.IsValueEmpty("quantity_unit_code")) {
                    args.Cancel("must have quantity_unit_code if quantity entered");
                }
            }
        }

        public void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger
        }

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
        }


        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            // post-table save trigger
        }

        #endregion

        #region IAsyncFilter Members

        public bool IsAsynchronous {
            get { return false; }
        }

        public object Clone() {
            return new InventoryActionDataTrigger();
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Require Inventory Action units if quantity filled in.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "InventoryAction Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return new string[] { "inventory_action" }; }
        }

        #endregion
    }
}
