using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    public class TableDataTriggerAdapter : ITableSaveDataTrigger {
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

        #region IDataTriggerDescription Members

        public virtual string GetDescription(string ietfLanguageTag) {
            return "No description for " + (ietfLanguageTag ?? "(unknown language)");
        }

        public virtual string GetTitle(string ietfLanguageTag) {
            return "No title for " + (ietfLanguageTag ?? "(unknown language)");
        }

        #endregion


        #region IAsyncDataTrigger Members

        /// <summary>
        /// Returns false.  Override this method as needed.
        /// </summary>
        public virtual bool IsAsynchronous {
            get { return false; }
        }

        /// <summary>
        /// Does NOT clone this object -- simply returns this object.  If your object has state, override this method!!!!
        /// </summary>
        /// <returns></returns>
        public virtual object Clone() {
            // HACK: can't determine the 'real' object, so assume this one is stateless.
            //       Implementors
            return this;
        }

        #endregion

        #region IDataResource Members

        public virtual string[] ResourceNames {
            get { return null; }
        }

        #endregion
    }
}
