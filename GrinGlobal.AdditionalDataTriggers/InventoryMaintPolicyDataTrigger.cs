using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.AdditionalDataTriggers {
    public class InventoryMaintPolicyDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 1) must have default dist quantity if autodeduct (debit set)
                if (helper.AllFieldsExist("is_auto_deducted", "distribution_default_quantity")
                    && helper.GetValue("is_auto_deducted", "", true).ToString().ToUpper() == "Y"
                    && helper.IsValueEmpty("distribution_default_quantity")) {
                    args.Cancel("must have distribution_default_quantity if is_auto_deducted");
                }

                // 2) must have default critical dist cutoff if autodeduct
                if (helper.AllFieldsExist("is_auto_deducted", "distribution_critical_quantity")
                    && helper.GetValue("is_auto_deducted", "", true).ToString().ToUpper() == "Y"
                    && helper.IsValueEmpty("distribution_critical_quantity")) {
                    args.Cancel("must have distribution_critical_quantity if is_auto_deducted");
                }
                // 3) if default form is null set it to form_type
                if (helper.AllFieldsExist("distribution_default_form_code", "form_type_code")) {
                    helper.SetValueIfFieldExistsAndIsEmpty("distribution_default_form_code", helper.GetValue("form_type_code", "", true).ToString());
                }

                // 4) must have default dist units if dist quant and vice versa
                if (helper.AllFieldsExist("distribution_default_quantity", "distribution_unit_code") && !helper.IsValueEmpty("distribution_default_quantity") && helper.IsValueEmpty("distribution_unit_code")) {
                    args.Cancel("Must have distribution_unit_code if distribution_default_quantity is not null.");
                }

                // 5) all quants >= 0
                if (helper.FieldExists("distribution_default_quantity") && Convert.ToDouble(helper.GetValue("distribution_default_quantity", 0.0, true)) < 0.0) {
                    args.Cancel("distribution_default_quantity must not be negative");
                }
                if (helper.FieldExists("distribution_critical_quantity") && Convert.ToDouble(helper.GetValue("distribution_critical_quantity", 0.0, true)) < 0.0) {
                    args.Cancel("distribution_critical_quantity must not be negative");
                }
                if (helper.FieldExists("regeneration_critical_quantity") && Convert.ToDouble(helper.GetValue("regeneration_critical_quantity", 0.0, true)) < 0.0) {
                    args.Cancel("regeneration_critical_quantity must not be negative");
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "InventoryMaintPolicy.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "InventoryMaintPolicy Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "InventoryMaintPolicy" };
            }
        }
    }
}