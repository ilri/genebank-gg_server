using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    public class DataTriggerAdapter : IDataviewSaveDataTrigger, ITableSaveDataTrigger, IDataviewReadDataTrigger {
        #region ITableSaveTrigger Members

        public virtual void TableSaving(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void TableRowSaving(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void TableRowSaved(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void TableRowSaveFailed(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            // do nothing
        }

        #endregion

        #region IDataViewSaveTrigger

        public virtual void DataViewSaving(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void DataViewRowSaving(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void DataViewRowSaved(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void DataViewRowSaveFailed(ISaveDataTriggerArgs args) {
            // do nothing
        }

        public virtual void DataViewSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            // do nothing
        }
        #endregion 

        #region IAsyncTrigger Members

        public virtual bool IsAsynchronous {
            get {
                return false;
            }
        }

        public virtual object Clone() {
            return this;
        }

        #endregion

        #region IDataTriggerDescription Members

        public virtual string GetDescription(string ietfLanguageTag) {
            return "No description for " + (ietfLanguageTag ?? "(unknown language)");
        }

        public string GetTitle(string ietfLanguageTag) {
            return "No title for " + (ietfLanguageTag ?? "(unknown language)");
        }

        #endregion


        #region IDataViewReadTrigger Members

        public virtual void DataViewReading(IReadDataTriggerArgs args) {
            // nothing to do
        }

        public virtual void DataViewRowRead(IReadDataTriggerArgs args) {
            // nothing to do
        }

        public virtual void DataViewRead(IReadDataTriggerArgs args) {
            // nothing to do
        }

        #endregion

        #region IDataResource Members

        public virtual string[] ResourceNames {
            get { return null; }
        }

        #endregion
    }
}
