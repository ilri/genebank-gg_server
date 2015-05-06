using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// A trigger that is applied at the dataview level during a call to GetData.  The object that implements this interface must be stateless to behave properly.
    /// </summary>
    public interface ITableReadDataTrigger : IAsyncDataTrigger, IDataTriggerDescription, IDataResource {

        /// <summary>
        /// Called once for each Table before any rows are retrieved.  Call args.Cancel() to prevent the database query and suppress TableRowRead and TableRead callbacks (i.e. does not even generate an empty DataTable object)
        /// </summary>
        /// <param name="args"></param>
        void TableReading(IReadDataTriggerArgs args);

        /// <summary>
        /// Called once for each row in the Table after data is retrieved but before TableRead is called.  Call args.Cancel() to prevent the args.DataRow from being added to the output.
        /// </summary>
        /// <param name="args"></param>
        void TableRowRead(IReadDataTriggerArgs args);

        /// <summary>
        /// Called once for each Table after data is retrieved and after TableRowRead is called for every row, but before any rows are returned.  Call args.Cancel() in this method to exclude all rows from output but still return a DataTable object.
        /// </summary>
        /// <param name="args"></param>
        void TableRead(IReadDataTriggerArgs args);

    }
}