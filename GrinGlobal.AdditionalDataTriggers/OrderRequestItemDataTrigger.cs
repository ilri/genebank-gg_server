using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using System.Globalization;
using System.Threading;

namespace GrinGlobal.AdditionalDataTriggers {
    public class OrderRequestItemDataTrigger : ITableSaveDataTrigger {
        #region ITableSaveFilter Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            // pre-table save trigger
        }

        public void TableRowSaving(ISaveDataTriggerArgs args)
        {
            // pre-row save trigger

            if (args.SaveMode == SaveMode.Insert || args.SaveMode == SaveMode.Update)
            {
                var h = args.Helper;

                // 4) default status (NEW)
                // in AT default but lets make sure
                h.SetValueIfFieldExistsAndIsEmpty("status_code", "NEW");
                h.SetValueIfFieldExistsAndIsEmpty("status_date", DateTime.UtcNow);

                // 3) fill in default distribution values if empty (go ahead and retrieve a bunch of stuff and do ifs later?)
                int f = 0;
                int q = 0;
                int u = 0;

                if (h.FieldExists("distribution_form_code") && h.IsValueEmpty("distribution_form_code")) { f = 1; }
                if (h.FieldExists("quantity_shipped") && h.IsValueEmpty("quantity_shipped")) { q = 1; }
                if (h.FieldExists("quantity_shipped_unit_code") && h.IsValueEmpty("quantity_shipped_unit_code")) { u = 1; }

                if (q == 1 || u == 1 || f == 1)
                {
                    int ivid = (int)h.GetValue("inventory_id", 0, true);
                    // args.Cancel("Inventory = " + ivid);
                    // return;
                    var dt = args.ReadData(@"SELECT distribution_default_form_code,distribution_default_quantity, distribution_unit_code FROM inventory WHERE inventory_id = :ivid ", ":ivid", ivid);
                    var dr = dt.Rows[0];

                    // string dform = dtDsc.Rows[0]["distribution_default_form_code"].ToString();
                    // double dquant = System.Convert.ToDouble(dtDsc.Rows[0]["distribution_default_quantity"]);
                    // string dunits = dtDsc.Rows[0]["distribution_unit_code"].ToString();
                    //args.Cancel("dform = " + dform + "dquant = " + dquant + "units = " + dunits);
                    //return;
                    if (f == 1) { h.SetValue("distribution_form_code", dr["distribution_default_form_code"], dr.Table.Columns["distribution_default_form_code"].DataType, false); }
                    if (q == 1) { h.SetValue("quantity_shipped", dr["distribution_default_quantity"], dr.Table.Columns["distribution_default_quantity"].DataType, false); }
                    if (u == 1) { h.SetValue("quantity_shipped_unit_code", dr["distribution_unit_code"], dr.Table.Columns["distribution_unit_code"].DataType, false); }
                    //
                    // 2) fill in default iv if accession only entered - cannot be done as acid not in table
                    //if (h.FieldExists("inventory_id") && h.IsValueEmpty("inventory_id")) 
                    // {
                    //  args.Cancel("Inventory not present.");
                    //     int acid = (int)h.GetValue("accession_id", 0, true);
                    //if (acid == 0) { return; }
                    //    args.Cancel("acid = " + acid);
                    //    return;
                    //  }
                }

                // fill in name if blank from topname
                if (h.FieldExists("name") && h.IsValueEmpty("name")) 
                {
                    int ivid = (int)h.GetValue("inventory_id", 0, true);
                    var dt = args.ReadData(@"select top 1 plant_name as topname from accession_inv_name an where inventory_id in
    (select inventory_id from inventory i where accession_id in (select accession_id from inventory i2 where inventory_id=:ivid)) order by plant_name_rank", ":ivid", ivid);

                    if (dt.Rows.Count > 0)
                    {
                        var dr = dt.Rows[0];
                        // args.Cancel("topname = " + dr["topname"]);
                        // return;
                        h.SetValue("name", dr["topname"], dr.Table.Columns["topname"].DataType, false); 
                    }                    
                }

                // set taxon column if null
                if (h.FieldExists("external_taxonomy") && h.IsValueEmpty("external_taxonomy"))
                {
                    int ivid = (int)h.GetValue("inventory_id", 0, true);
                    var dt = args.ReadData(@"select name from taxonomy_species t, accession a, inventory i where t.taxonomy_species_id=a.taxonomy_species_id and
      a.accession_id=i.accession_id and i.inventory_id=:ivid;", ":ivid", ivid);
                    var dr = dt.Rows[0];
                    // args.Cancel("tax = " + dr["name"]);
                    // return;
                    h.SetValue("external_taxonomy", dr["name"], dr.Table.Columns["name"].DataType, false);               
                }

               
            }
        }
        public void TableRowSaved(ISaveDataTriggerArgs args)
        {
            // post-row save trigger
            if (args.SaveMode == SaveMode.Insert)
            {
                if (!args.Helper.IsValueEmpty("web_order_request_item_id"))
                {
                    int? woriID = (int?)(args.Helper.GetValue("web_order_request_item_id", null, true));
                    if (woriID > 0)
                    {
                        // update web_order_request_item table status
                        args.WriteData(@"
                        update
                            web_order_request_item
                        set
                            status_code = 'ACCEPTED',
                            modified_date = :modifieddate
                        where
                            status_code = 'NEW' and
                            web_order_request_item_id = :woriID
                        ", ":modifieddate", DateTime.UtcNow, ":woriID", woriID);
                    }
                }
            }
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
            return new OrderRequestItemDataTrigger();
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Set item defaults.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "OrderRequestItem Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return new string[] { "order_request_item" }; }
        }

        #endregion
    }
}
