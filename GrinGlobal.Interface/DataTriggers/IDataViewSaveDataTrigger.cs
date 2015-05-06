using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// A trigger that is applied at the dataview level during a call to SaveData.  The object that implements this interface must be stateless to behave properly.
    /// </summary>
    public interface IDataviewSaveDataTrigger : IAsyncDataTrigger, IDataTriggerDescription, IDataResource {
        /// <summary>
        /// Called once for each DataView before ANY rows are saved.
        /// </summary>
        /// <param name="args"></param>
        void DataViewSaving(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once for each row in the DataView before the row is saved.
        /// </summary>
        /// <param name="args"></param>
        void DataViewRowSaving(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once for each row in the DataView after the row is saved.  args.Exception will be null.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="saveWasSuccessful"></param>
        void DataViewRowSaved(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once for each row in the DataView if the row save fails.  args.Exception will be set to the exception which caused the failure
        /// </summary>
        /// <param name="args"></param>
        /// <param name="ex"></param>
        void DataViewRowSaveFailed(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once after the last row has been saved.  Is called even if one or more row(s) were Cancelled during a save, whether by this trigger or any other trigger.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="successfulSaveCount"></param>
        void DataViewSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount);
    }
}
