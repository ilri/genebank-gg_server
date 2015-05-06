using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using GrinGlobal.Core;

namespace GrinGlobal.AdditionalDataTriggers
{
    public class InventoryDataTrigger : TableDataTriggerAdapter {
        string note = "";
        string newNote = "";
        bool noteChanged;
        public override void TableSaving(ISaveDataTriggerArgs args) {
            // pre-table save trigger          
        }

        public override void TableRowSaving(ISaveDataTriggerArgs args){
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                // pre-row save trigger
                note = args.Helper.GetOriginalValue("web_availability_note", "").ToString();
                newNote = args.Helper.GetValue("web_availability_note", "", true).ToString();
                if (note != newNote) noteChanged = true;
            }
        }

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;
                //    args.WriteData("update inventory_viability set percent_viable = percent_dormant + percent_normal where inventory_viability_id = " + args.NewPrimaryKeyID);
                //    note = helper.GetOriginalValue("web_availability_note", "").ToString();
                //    newNote = helper.GetValue("web_availability_note", "", true).ToString();
                if (noteChanged)
                {
                    var dtEmails = args.ReadData(@"select distinct wu.user_name, LTRIM(RTRIM(LTRIM(COALESCE(a.accession_number_part1, '') + ' ') + LTRIM(COALESCE(convert(varchar, a.accession_number_part2), '') + ' ') + COALESCE(a.accession_number_part3, ''))) AS PI_Number
                                    from inventory i, accession a, web_user wu, web_user_cart wuc, web_user_cart_item wuci 
                                    where i.accession_id = a.accession_id 
                                    and a.accession_id = wuci.accession_id
                                    and wuci.web_user_cart_id = wuc.web_user_cart_id
                                    and wuc.web_user_id = wu.web_user_id
                                    and wuc.cart_type_code = 'favorites'
                                    and i.accession_id =:accessionid", ":accessionid", helper.GetValue("accession_id", 0, true), DbType.Int32);

                    if (dtEmails.Rows.Count > 0 ) {
                        string emailTo = "";
                        string pi = dtEmails.Rows[0]["PI_Number"].ToString();

                        foreach (DataRow dr in dtEmails.Rows)
                        {
                            emailTo += dr["user_name"].ToString() + "; ";
                        }

                        StringBuilder sb = new StringBuilder();
                        //string CRLF = "\r\n";
                        sb.Append("Web availability changed for item ").Append(pi).Append(" in your favorites list."); // TODO: need more email text here

                        try
                        {
                            // hard code 'email from' as dbmu@ars-grin.gov, need to use something to be configured
                            Email.Send(emailTo,
                                       "dbmu@ars-grin.gov",
                                        "",
                                        "",
                                        "GRIN-GLOBAL - Inventory Availabilty Change Notice",
                                        sb.ToString());
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message; // debug
                        }
                    }
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Send email when web_availability_note is updated.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Inventory Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "inventory" };
            }
        }
    }

    public class InventoryDefaultsDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            if (args.SaveMode == SaveMode.Insert) {
                fillMaintenancePolicyValues(args);
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Auto-populates inventory data with corresponding default values from associated inventory_maint_policy data.  Does not override custom-filled values provided by the user.  Applies only on insert";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Defaults for inventory fields based on the associated inventory maintenance policy";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "inventory" };
            }
        }

        private void fillMaintenancePolicyValues(ISaveDataTriggerArgs args) {
            var h = args.Helper;

            var policyID = Toolkit.ToInt32(h.GetValue("inventory_maint_policy_id", -1, true), -1);
            if (policyID > 0) {
                var dt = args.ReadData(@"
SELECT inventory_maint_policy_id
      ,maintenance_name
      ,form_type_code
      ,quantity_on_hand_unit_code
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

                    if (h.FieldExists("quantity_on_hand_unit_code") && h.IsValueEmpty("quantity_on_hand_unit_code")) {
                        h.SetValue("quantity_on_hand_unit_code", dr["quantity_on_hand_unit_code"], dr.Table.Columns["quantity_on_hand_unit_code"].DataType, false);
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
                    if (h.FieldExists("regeneration_critical_quantity") && h.IsValueEmpty("regeneration_critical_quantity")) {
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


    public class InventoryUnitsDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 2) must have units if quantity onhand entered - N
                if (helper.AllFieldsExist("quantity_on_hand", "quantity_on_hand_unit_code") && !helper.IsValueEmpty("quantity_on_hand") && helper.IsValueEmpty("quantity_on_hand_unit_code")) {
                    args.Cancel("must have quantity_on_hand_unit_code if quantity_on_hand entered");
                }

                // 3) must have dist units if default dist quantity filled in - N
                if (helper.AllFieldsExist("distribution_default_quantity", "distribution_unit_code") && !helper.IsValueEmpty("distribution_default_quantity") && helper.IsValueEmpty("distribution_unit_code")) {
                    args.Cancel("must have distribution_unit_code if distribution_default_quantity entered");
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Inventory units fields are filled out if quantities are specified.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Inventory Units Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }


    public class InventoryStatusDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 4) if debit set (auto deduct) toggle LOW/AVAIL based on dist critical - N
                // 
                //    IF :NEW.debit = 'Y' THEN
                //        IF (nvl(:NEW.onhand,0) <= nvl(:NEW.dcritical,0))
                //            AND (:NEW.status = 'AVAIL') THEN
                //            :NEW.status := 'LOW';
                //        ELSIF (nvl(:NEW.onhand,0) > nvl(:NEW.dcritical,0))
                //            AND (:NEW.status = 'LOW') THEN
                //            :NEW.status := 'AVAIL';
                //        END IF;
                //    END IF;

                if (helper.AllFieldsExist("is_auto_deducted", "quantity_on_hand", "distribution_critical_quantity") && helper.GetValue("is_auto_deducted", "N", true).ToString().ToUpper() == "Y") {
                    double onHand = Convert.ToDouble(helper.GetValue("quantity_on_hand", 0.0, true));
                    double critical = Convert.ToDouble(helper.GetValue("distribution_critical_quantity", 0.0, true));
                    if (onHand <= critical) {
                        if (helper.FieldExists("availability_status_code") && helper.GetValue("availability_status_code", "", true).ToString().ToUpper() == "AVAIL") {
                            helper.SetValue("availability_status_code", "LOW", null, false);
                            if (helper.FieldExists("is_available") && helper.GetValue("is_available", "Y", true).ToString().ToUpper() == "Y") {
                                helper.SetValue("is_available", "N", null, false);
                            }
                        }
                    } else if (onHand > critical) {
                        if (helper.FieldExists("availability_status_code") && helper.GetValue("availability_status_code", "", true).ToString().ToUpper() == "LOW") {
                            helper.SetValue("availability_status_code", "AVAIL", null, false);
                            if (helper.FieldExists("is_available") && helper.GetValue("is_available", "N", true).ToString().ToUpper() == "N") {
                                helper.SetValue("is_available", "Y", null, false);
                            }
                        }
                    }
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Adjusts inventory status based on critical.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Inventory Status Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }


    public class InventoryQuantityDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var helper = args.Helper;

                // 5) all quants >=0
                if (helper.FieldExists("quantity_on_hand") && Convert.ToDouble(helper.GetValue("quantity_on_hand", 0.0, true)) < 0.0) {
                    args.Cancel("quantity_on_hand must not be negative");
                }
                if (helper.FieldExists("distribution_critical_quantity") && Convert.ToDouble(helper.GetValue("distribution_critical_quantity", 0.0, true)) < 0.0) {
                    args.Cancel("distribution_critical_quantity must not be negative");
                }
                if (helper.FieldExists("distribution_default_quantity") && Convert.ToDouble(helper.GetValue("distribution_default_quantity", 0.0, true)) < 0.0) {
                    args.Cancel("distribution_default_quantity must not be negative");
                }
                if (helper.FieldExists("regeneration_critical_quantity") && Convert.ToDouble(helper.GetValue("regeneration_critical_quantity", 0.0, true)) < 0.0) {
                    args.Cancel("regeneration_critical_quantity must not be negative");
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks Inventory quantiy fields are no negative.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Inventory Quantity Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }

    // clear distribute flag if new one is flagged
     
      public class InventoryDistributeFlagSetDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger

            // for simplicity just check current iv on modify, for insert set to 0 so all updated just set and forget, otherwise have to use after save trigger on insert

            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update) {
                var h = args.Helper;
                if (h.FieldExists("is_distributable"))
                {
                    string dist = h.GetValue("is_distributable", "", true).ToString();
                    //args.Cancel("dist is " + dist);
                    //return;
                    if (dist == "Y")
                    {
                        int ivid ;

                        if (args.SaveMode == SaveMode.Update)
                        {ivid = (int)h.GetValue("inventory_id", 0, true);} else {ivid = 0;}

                        int acid = (int)h.GetValue("accession_id", 0, true);
                        var dt = args.ReadData(@"select coalesce(count(*),0)  as ct from inventory i where i.accession_id=:acid and is_distributable = 'Y' and inventory_id <> :ivid ;", ":acid", acid,":ivid", ivid);
                        var dr = dt.Rows[0];
                        int numiv = (int)dr["ct"];
                        
                        if (numiv > 0)
                        {
                            //args.Cancel("Iv = " + dr["ct"]);
                            //args.Cancel(" ivid = " + ivid);
                            //return;
                            args.WriteData(@"update inventory set is_distributable = 'N' where accession_id = :acid and is_distributable = 'Y' and inventory_id <> :ivid", 
                                new DataParameters(
                                  ":acid", acid, DbType.Int32,":ivid", ivid, DbType.Int32));
 
                        }
                    }

                }
                
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Makes sure only 1 Inventory is set to distribute.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "Inventory Distribute Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "inventory" };
            }
        }
    }

      // prevent insert, update, delete of system iv (insert/delete handled by internal accession datatrigger)
      public class InventoryProtectSysInvDataTrigger : TableDataTriggerAdapter
      {

          public override void TableRowSaving(ISaveDataTriggerArgs args)
          {
              // pre-row save trigger
              // no need to check mode?

              if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update || args.SaveMode == SaveMode.Delete)
              {
                  var h = args.Helper;
                  if (h.FieldExists("form_type_code"))
                  {
                      string ivt = "SD"; 
                      if (args.SaveMode != SaveMode.Delete) {ivt = h.GetValue("form_type_code", "", true).ToString(); }
                      string ivto = h.GetOriginalValue("form_type_code", "").ToString();

                      if (ivt == "**" || ivto == "**")
                      {
                         args.Cancel("You can not insert, update or delete the system inventory row.");
                         return;
                      }
                  }

              }
          }

          public override string GetDescription(string ietfLanguageTag)
          {
              return "Do not allow insert, update of system inventory.";
          }

          public override string GetTitle(string ietfLanguageTag)
          {
              return "Inventory Protect System Inventory Data Trigger";
          }

          public override string[] ResourceNames
          {
              get
              {
                  return new string[] { "inventory" };
              }
          }
      }

}
