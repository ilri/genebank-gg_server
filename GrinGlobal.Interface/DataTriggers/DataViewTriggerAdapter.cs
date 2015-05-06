using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    public class DataViewTriggerAdapter : IDataviewSaveDataTrigger, IDataviewReadDataTrigger {

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

        public virtual void DataViewSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
            // do nothing
        }

        public virtual void DataViewRowSaveFailed(ISaveDataTriggerArgs args) {
            // do nothing
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

        #region IDataTriggerDescription Members

        public virtual string GetDescription(string ietfLanguageTag) {
            return "No description for " + (ietfLanguageTag ?? "(unknown language)");
        }

        public virtual string GetTitle(string ietfLanguageTag) {
            return "No title for " + (ietfLanguageTag ?? "(unknown language)");
        }

        #endregion

        #region IDataResource Members

        public virtual string[] ResourceNames {
            get {
                return null;
            }
        }

        #endregion
    }
}
