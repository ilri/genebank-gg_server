using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Interface.DataTriggers {
    public interface IReadDataTriggerArgs {
        /// <summary>
        /// Gets the reason the trigger is cancelling the read.  Can be set by callng Cancel() method.
        /// </summary>
        string CancelReason { get; set; }
        /// <summary>
        /// Gets if the trigger requests the read to be cancelled.  Can by set by calling Cancel() method.
        /// </summary>
        bool IsCancelled { get; set; }
        /// <summary>
        /// Tells SecureData to cancel returning any data.  Can be reversed by calling Uncancel().
        /// </summary>
        /// <param name="reason"></param>
        void Cancel(string reason);
        /// <summary>
        /// Marks the row as no longer being cancelled (i.e. clears CancelReason and sets IsCancelled = false)
        /// </summary>
        void Uncancel();

        /// <summary>
        /// The DataTable generated from the database read.  Null during DataViewReading.
        /// </summary>
        DataTable DataTable { get; set; }

        /// <summary>
        /// The current DataRow generated from the database read.  Only valid during DataViewRowRead.
        /// </summary>
        DataRow DataRow { get; set; }

        /// <summary>
        /// Gets the helper object for accessing data safely regardless of dataview definition
        /// </summary>
        DataTriggerHelper Helper { get; set; }

        /// <summary>
        /// Gets or sets a reference to the SecureData object that caused this trigger to execute.
        /// </summary>
        object SecureData { get; set; }

        /// <summary>
        /// DataView-level field mapping information for the fields in the row relevant to the table in the database.
        /// </summary>
        IDataview FieldMappingDataView { get; set; }

        /// <summary>
        /// Gets or sets a reference to an object the caller may use to pass additional information along to the event handler.
        /// </summary>
        object AdditionalData { get; set; }

        /// <summary>
        /// Gets the ID used for security purposes for the current user
        /// </summary>
        int UserID { get; }
        /// <summary>
        ///  Gets the ID used for auditing and tracking purposes for the current user
        /// </summary>
        int CooperatorID { get; }
        /// <summary>
        /// Gets the ID representing the current user's language
        /// </summary>
        int LanguageID { get; }
        /// <summary>
        /// Gets the login name of the current user
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Gets or sets the DataManager currently used by SecureData
        /// </summary>
        //        public DataManager DataManager;

        IReadDataTriggerArgs Clone();
    }
}
