using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Filters;

namespace GrinGlobal.ExampleFilters {
    /// <summary>
    /// Example ad-hoc table-level filter.  Implements both interfaces for reading (ITableReadFilter) and writing (ITableSaveFilter) tables.
    /// </summary>
    public class TableTestFilter : ITableReadFilter, ITableSaveFilter {

        // Read filter members are called when someone issues a GetData() to the middle tier.
        #region ITableReadFilter Members

        public void TableReading(IReadFilterArgs args) {
            // called one time for each database table mapped in the Dataview
            // before all rows have been read -- a request-level 'pre' hook
        }

        public void TableRowRead(IReadFilterArgs args) {
            // called once for each row -- a row-level 'post' hook
        }

        public void TableRead(IReadFilterArgs args) {
            // called one time for each database table mapped in the Dataview
            // after all rows have been read -- a request-level 'post' hook
        }


        #endregion

        #region IAsyncFilter Members

        public object Clone() {
            // used to duplicate itself, may be called by the middle tier at any time.
            // most objects will be stateless, if that is the case just return a new copy.
            return new TableTestFilter();
        }

        public bool IsAsynchronous {
            get { 
                // if this filter is to run asynchronously, return true.
                // otherwise return false.
                // Almost every single filter should return false here.
                // Exceptions would be anything that can run out-of-band,
                // such as audit filters, sending email, etc.
                return false;
            }
        }

        #endregion

        // Save filter members are called when someone calls SaveData() to the middle tier
        #region ITableSaveFilter Members

        public void TableSaving(ISaveFilterArgs args) {
            // called one time for each database table mapped in the dataview
            // before any rows are processed.  Do initialization here.
        }

        public void TableRowSaving(ISaveFilterArgs args) {
            // called one time for each row before it has been written to the database
        }

        public void TableRowSaved(ISaveFilterArgs args, bool saveWasSuccessful) {
            // called one time for each row after it has been attempted to be written to the database
        }

        public void TableSaved(ISaveFilterArgs args, int successfulSaveCount, int failedSaveCount) {
            // called one time for each database table mapped in the dataview
            // after all rows have been processed.  Do tear down here.
        }

        #endregion
    }
}
