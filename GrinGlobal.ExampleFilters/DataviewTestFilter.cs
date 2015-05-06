using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Filters;

namespace GrinGlobal.ExampleFilters {
    /// <summary>
    /// Example ad-hoc dataview filter.  Implements both interfaces for reading (IDataViewReadFilter) and writing (IDataViewSaveFilter) tables.
    /// </summary>
    public class DataviewTestFilter : IDataViewReadFilter, IDataViewSaveFilter {

        #region IDataViewReadFilter Members

        public void DataViewReading(IReadFilterArgs args) {
            // called once before any rows are read from the database.  Do initialization here.
        }

        public void DataViewRead(IReadFilterArgs args) {
            // called once for each row read from the database.
        }

        public void DataViewRowRead(IReadFilterArgs args) {
            // called once after all rows have been read from the database.  Do tear down here.
            throw new NotImplementedException();
        }

        #endregion

        #region IDataViewSaveFilter Members

        public void DataViewSaving(ISaveFilterArgs args) {
            // called one time before any rows have been read 
            // i.e. a request-level 'pre' hook
        }

        public void DataViewRowSaving(ISaveFilterArgs args) {
            // called one time for each row before it has been written to the database
            // i.e. a row-level 'pre' hook
        }

        public void DataViewRowSaved(ISaveFilterArgs args, bool saveWasSuccessful) {
            // called one time for each row after it has been attempted to be written to the database
            // i.e. a row-level 'post' hook
        }

        public void DataViewSaved(ISaveFilterArgs args, int successfulSaveCount, int failedSaveCount) {
            // called one time after all rows have been read
            // i.e. a request-level 'post' hook
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

    }
}
