using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;

namespace GrinGlobal.Business.DataTriggers {
    public class TransactionDataTrigger : IDataviewSaveDataTrigger {
        #region IDataViewSaveTrigger Members

        public void DataViewSaving(ISaveDataTriggerArgs args) {
            ((SaveDataTriggerArgs)args).DataManager.BeginTran();
        }

        public void DataViewRowSaving(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void DataViewRowSaved(ISaveDataTriggerArgs args) {
            // nothing to do
        }

        public void DataViewRowSaveFailed(ISaveDataTriggerArgs args) {
            ((SaveDataTriggerArgs)args).DataManager.Rollback();
        }

        public void DataViewSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            if (args.IsCancelled) {
                ((SaveDataTriggerArgs)args).DataManager.Rollback();
            } else {
                ((SaveDataTriggerArgs)args).DataManager.Commit();
            }
        }

        #endregion



        #region IAsyncTrigger Members

        public bool IsAsynchronous {
            get {
                return false;
            }
        }

        public virtual object Clone() {
            return this;
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Begins a database-level transaction at DataViewSaving() time, then commits if all rows saved properly.  Rolls back transaction otherwise.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Transaction Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return null; }
        }

        #endregion
    }
}
