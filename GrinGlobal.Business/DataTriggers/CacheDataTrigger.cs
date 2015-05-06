
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Web;
using GrinGlobal.Core;

namespace GrinGlobal.Business.DataTriggers {
    public class CacheDataTrigger : ITableSaveDataTrigger {

        #region ITableSaveDataTrigger Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void TableRowSaving(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void TableRowSaved(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {

            var name = args.Table.TableName.ToLower();
            if (name.StartsWith("sys_") || name == "app_resource") {
                // need to make sure the caches are cleared if data is saved these tables
                // note this temporarily slows things down because each new request coming in will
                // require a database hit, so be careful what tables you add here.
                CacheManager.ClearAll();
            }

        }

        #endregion

        #region IAsyncDataTrigger Members

        public bool IsAsynchronous {
            get { return false; }
        }

        public object Clone() {
            // no state to clone, just return a new object
            return new CacheDataTrigger();
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Clears server-side caches as needed when data is changed in tables required by the GRIN-Global system (sys_* tables).";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Cache Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return null; }
        }

        #endregion
    }
}
