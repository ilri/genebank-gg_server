using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.Core;

namespace GrinGlobal.Business.DataTriggers {
    public class InventoryDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args)
        {
            // maint policy function moved to additional data trigger
            //if (args.SaveMode == SaveMode.Insert)
            //{
                //fillMaintenancePolicyValues(args);
            //}
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Auto-populates inventory data with corresponding default values from associated inventory_maint_policy data.  Does not override custom-filled values provided by the user.  Applies only on insert";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Defaults for inventory fields based on the associated inventory maintenance policy";
        }

        private void fillMaintenancePolicyValues(ISaveDataTriggerArgs args) {
            var h = args.Helper;

            var policyID = Toolkit.ToInt32(h.GetValue("inventory_maint_policy_id", -1, true), -1);
            if (policyID > 0){
                var dt = args.ReadData(@"
SELECT inventory_maint_policy_id
      ,maintenance_name
      ,form_type_code
      ,on_hand_unit_code
      ,web_availability_note
      ,is_auto_deducted
      ,distribution_default_form_code
      ,distribution_default_quantity
      ,distribution_unit_code
      ,distribution_critical_quantity
      ,regeneration_critical_quantity
      ,regeneration_method_code 
from 
    inventory_maint_policy 
where 
    inventory_maint_policy_id = " + policyID);
                if (dt.Rows.Count > 0) {
                    var dr = dt.Rows[0];

                    if (h.FieldExists("form_type_code") && h.IsValueEmpty("form_type_code")) {
                        h.SetValue("form_type_code", dr["form_type_code"], dr.Table.Columns["form_type_code"].DataType, false);
                    }

                    if (h.FieldExists("on_hand_unit_code") && h.IsValueEmpty("on_hand_unit_code")) {
                        h.SetValue("on_hand_unit_code", dr["on_hand_unit_code"], dr.Table.Columns["on_hand_unit_code"].DataType, false);
                    }
                    if (h.FieldExists("web_availability_note") && h.IsValueEmpty("web_availability_note")) {
                        h.SetValue("web_availability_note", dr["web_availability_note"], dr.Table.Columns["web_availability_note"].DataType, false);
                    }
                    if (h.FieldExists("is_auto_deducted") && h.IsValueEmpty("is_auto_deducted")) {
                        h.SetValue("is_auto_deducted", dr["is_auto_deducted"], dr.Table.Columns["is_auto_deducted"].DataType, false);
                    }
                    if (h.FieldExists("distribution_default_form_code") && h.IsValueEmpty("distribution_default_form_code")) {
                        h.SetValue("distribution_default_form_code", dr["distribution_default_form_code"], dr.Table.Columns["distribution_default_form_code"].DataType, false);
                    }
                    if (h.FieldExists("distribution_default_quantity") && h.IsValueEmpty("distribution_default_quantity")) {
                        h.SetValue("distribution_default_quantity", dr["distribution_default_quantity"], dr.Table.Columns["distribution_default_quantity"].DataType, false);
                    }

                    if (h.FieldExists("distribution_unit_code") && h.IsValueEmpty("distribution_unit_code")) {
                        h.SetValue("distribution_unit_code", dr["distribution_unit_code"], dr.Table.Columns["distribution_unit_code"].DataType, false);
                    }
                    if (h.FieldExists("distribution_critical_quantity") && h.IsValueEmpty("distribution_critical_quantity")) {
                        h.SetValue("distribution_critical_quantity", dr["distribution_critical_quantity"], dr.Table.Columns["distribution_critical_quantity"].DataType, false);
                    }
                    if (h.FieldExists("regeneration_critical_quantity") && h.IsValueEmpty("regeneration_critical_quantity"))
                    {
                        h.SetValue("regeneration_critical_quantity", dr["regeneration_critical_quantity"], dr.Table.Columns["regeneration_critical_quantity"].DataType, false);
                    }

                    // comment out for now, wait for gap analysis.
                    //if (h.FieldExists("pollination_method_code") && h.IsValueEmpty("pollination_method_code"))
                    //{
                    //    h.SetValue("pollination_method_code", dr["regeneration_method_code"], dr.Table.Columns["regeneration_method_code"].DataType, false);
                    //}

                }
            }
        }

    }
}
