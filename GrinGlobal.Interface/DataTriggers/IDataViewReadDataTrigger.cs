using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// A trigger that is applied at the dataview level during a call to GetData.  The object that implements this interface must be stateless to behave properly.
    /// </summary>
    public interface IDataviewReadDataTrigger : IAsyncDataTrigger, IDataTriggerDescription, IDataResource {

        /// <summary>
        /// Called once for each DataView before any rows are retrieved.  Call args.Cancel() to perform the read but return no data (to generate a valid DataTable object)
        /// </summary>
        /// <param name="args"></param>
        void DataViewReading(IReadDataTriggerArgs args);

        /// <summary>
        /// Called once for each row in the DataView after data is retrieved but before DataViewRead is called.  Call args.Cancel() to prevent the args.DataRow from being added to the output.
        /// </summary>
        /// <param name="args"></param>
        void DataViewRowRead(IReadDataTriggerArgs args);

        /// <summary>
        /// Called once for each DataView after data is retrieved and after DataViewRowRead is called for every row, but before DataTable is returned.  Call args.Cancel() in this method to exclude all rows from the DataTable but still return the DataTable object.
        /// </summary>
        /// <param name="args"></param>
        void DataViewRead(IReadDataTriggerArgs args);

    }
}