using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.Interface.DataTriggers {
	/// <summary>
    /// A trigger that is applied at the table level during a call to SaveData.  The object that implements this interface must be stateless to behave properly.
	/// </summary>
	public interface ITableSaveDataTrigger : IAsyncDataTrigger, IDataTriggerDescription, IDataResource  {

        /// <summary>
        /// Called once for each Table associated with a given DataView before ANY rows are saved.
        /// </summary>
        /// <param name="args"></param>
        void TableSaving(ISaveDataTriggerArgs args);
        
        /// <summary>
        /// Called once for each Table associated with a given row in the DataView before the row is saved.  So if the DataView joins 2 tables, this will be called twice for each row -- once for each table.
        /// </summary>
        /// <param name="args"></param>
        void TableRowSaving(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once for each Table associated with a given row in the DataView after the row is saved.  So if the DataView joins 2 tables, this will be called twice for each row -- once for each table.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="saveWasSuccessful"></param>
		void TableRowSaved(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once for each Table associated with a given row in the DataView after the row save has failed.  So if the DataView joins 2 tables, this will be called twice for each row -- once for each table.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="saveWasSuccessful"></param>
        void TableRowSaveFailed(ISaveDataTriggerArgs args);

        /// <summary>
        /// Called once for each Table associated with a given DataView after the last row has been saved.  Is called even if one or more row(s) were Cancelled during a save, whether by this trigger or another trigger.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="successfulSaveCount"></param>
        void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount);

	}
}
