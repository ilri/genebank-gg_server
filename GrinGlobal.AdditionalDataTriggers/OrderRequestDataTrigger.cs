using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;
using System.Globalization;
using System.Threading;

namespace GrinGlobal.AdditionalDataTriggers
{
    public class OrderRequestDataTrigger : ITableSaveDataTrigger
    {
        #region ITableSaveFilter Members

        public void TableSaving(ISaveDataTriggerArgs args)
        {
            // pre-table save trigger
        }

        public void TableRowSaving(ISaveDataTriggerArgs args)
        {

        }

        public void TableRowSaved(ISaveDataTriggerArgs args)
        {
            // post-row save trigger

            if (args.SaveMode == SaveMode.Insert)
            {
                if (!args.Helper.IsValueEmpty("web_order_request_id"))
                {
                    int? worID = (int?)(args.Helper.GetValue("web_order_request_id", null, true));
                    if (worID > 0)
                    {
                        // update web_order_request table status
                        args.WriteData(@"
update 
    web_order_request
set 
    status_code = 'ACCEPTED', 
    modified_date = :modifieddate
where 
    web_order_request_id = :worID
", ":modifieddate", DateTime.UtcNow, ":worID", worID);
                    }
                }

            }
        }

        public void TableRowSaveFailed(ISaveDataTriggerArgs args)
        {
        }


        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount)
        {
            // post-table save trigger
        }

        #endregion

        #region IAsyncFilter Members

        public bool IsAsynchronous
        {
            get { return false; }
        }

        public object Clone()
        {
            return new OrderRequestDataTrigger();
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag)
        {
            return "Update web_order_request status if one order is created from it.";
        }

        public string GetTitle(string ietfLanguageTag)
        {
            return "OrderRequest Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames
        {
            get { return new string[] { "order_request" }; }
        }

        #endregion
    }


    public class OrderRequestOrderTypeCodeDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert) {
                var helper = args.Helper;

                // 2) set order type to DI if null - N
                helper.SetValueIfFieldExistsAndIsEmpty("order_type_code", "DI");
                if (helper.IsValueEmpty("order_type_code")) {
                    helper.SetValue("order_type_code", "DI", typeof(string), false);
                }

                // 3) set ordered date to today if null - N
                helper.SetValueIfFieldExistsAndIsEmpty("ordered_date", DateTime.UtcNow);
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Sets Order order_type_code and ordered_date if NULL.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "OrderRequest.order_type_code Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }


    public class OrderRequestCoopsDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert) {
                var helper = args.Helper;

                // fulls in NULL coop links from final
                if (helper.AllFieldsExist("requestor_cooperator_id", "final_recipient_cooperator_id") && helper.IsValueEmpty("requestor_cooperator_id")) {
                    helper.SetValue("requestor_cooperator_id", helper.GetValue("final_recipient_cooperator_id", DBNull.Value, true), typeof(int), false);
                }
                if (helper.AllFieldsExist("final_recipient_cooperator_id", "requestor_cooperator_id") && helper.IsValueEmpty("final_recipient_cooperator_id")) {
                    helper.SetValue("final_recipient_cooperator_id", helper.GetValue("requestor_cooperator_id", DBNull.Value, true), typeof(int), false);
                }
                if (helper.AllFieldsExist("ship_to_cooperator_id", "final_recipient_cooperator_id") && helper.IsValueEmpty("ship_to_cooperator_id")) {
                    helper.SetValue("ship_to_cooperator_id", helper.GetValue("final_recipient_cooperator_id", DBNull.Value, true), typeof(int), false);
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Sets Order coop links if NULL.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "OrderRequest.coopertor_id Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }


    public class OrderRequestCoopCatDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaving(ISaveDataTriggerArgs args) {
            // pre-row save trigger
            if (args.SaveMode == SaveMode.Insert) {
                var helper = args.Helper;

                // check that the final recipient has a category code
                var dtCoop = args.ReadData(@"SELECT category_code FROM cooperator WHERE cooperator_id = :coopid AND category_code IS NOT NULL", ":coopid", helper.GetValue("final_recipient_cooperator_id", 0, true), DbType.Int32);
                if (dtCoop.Rows.Count < 1) {
                    args.Cancel("Final recipient cooperator must have a category");
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Checks final recipient category";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "OrderRequest.category Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }


    public class OrderRequestOrigDataTrigger : TableDataTriggerAdapter {

        public override void TableRowSaved(ISaveDataTriggerArgs args) {
            // post-row save trigger

            if (args.SaveMode == SaveMode.Insert) {
                // make original_order_request_id field match the order_request_id field
                if (args.Helper.IsValueEmpty("original_order_request_id")) {
                    args.WriteData(@"
UPDATE order_request
SET original_order_request_id = :orid
WHERE order_request_id = :orid AND original_order_request_id IS NULL
", ":orid", args.NewPrimaryKeyID);
                }
            }
        }

        public override string GetDescription(string ietfLanguageTag) {
            return "Sets Order original_order_request_id if NULL.";
        }

        public override string GetTitle(string ietfLanguageTag) {
            return "OrderRequest.original_order_request_id Data Trigger";
        }

        public override string[] ResourceNames {
            get {
                return new string[] { "order_request" };
            }
        }
    }

}
