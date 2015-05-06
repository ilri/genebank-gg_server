using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Interface.Dataviews;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.Core;

namespace GrinGlobal.Business.DataTriggers {
    public class ReadDataTriggerArgs : IReadDataTriggerArgs {
        /// <summary>
        /// Gets the reason the trigger is cancelling the read.  Can be set by callng Cancel() method.
        /// </summary>
        public string CancelReason { get; set; }
        /// <summary>
        /// Gets if the trigger requests the read to be cancelled.  Can by set by calling Cancel() method.
        /// </summary>
        public bool IsCancelled { get; set; }
        /// <summary>
        /// Tells SecureData to cancel returning any data.  Can be reversed by calling Uncancel().
        /// </summary>
        /// <param name="reason"></param>
        public void Cancel(string reason) {
            CancelReason = reason;
            IsCancelled = true;
        }
        /// <summary>
        /// Marks the row as no longer being cancelled (i.e. clears CancelReason and sets IsCancelled = false)
        /// </summary>
        public void Uncancel() {
            IsCancelled = false;
            CancelReason = null;
        }

        /// <summary>
        /// The DataTable generated from the database read.  Null during DataViewReading.
        /// </summary>
        public DataTable DataTable { get; set; }

        /// <summary>
        /// The current DataRow generated from the database read.  Only valid during DataViewRowRead.
        /// </summary>
        public DataRow DataRow { get; set; }

        /// <summary>
        /// Gets the helper object for safely accessing data regardless of dataview definition
        /// </summary>
        public DataTriggerHelper Helper { get; set; }

        /// <summary>
        /// Gets or sets a reference to the SecureData object that caused this trigger to execute.
        /// </summary>
        public object SecureData { get; set; }

        /// <summary>
        /// DataView-level field mapping information for the fields in the row relevant to the table in the database.
        /// </summary>
        public IDataview FieldMappingDataView { get; set; }

        /// <summary>
        /// Gets or sets a reference to an object the caller may use to pass additional information along to the event handler.
        /// </summary>
        public object AdditionalData { get; set; }

        public int UserID { get { return (SecureData as SecureData).SysUserID; } }
        public int CooperatorID { get { return (SecureData as SecureData).CooperatorID; } }
        public int LanguageID { get { return (SecureData as SecureData).LanguageID; } }
        public string UserName { get { return (SecureData as SecureData).SysUserName; } }

        /// <summary>
        /// Gets or sets the options the caller specified in the call to the GetData method
        /// </summary>
        public ReadOptions ReadOptions { get; set; }

        /// <summary>
        /// Gets or sets the DataManager currently used by SecureData
        /// </summary>
        public DataManager DataManager { get; set; }

        public IReadDataTriggerArgs Clone() {
            ReadDataTriggerArgs ret = new ReadDataTriggerArgs();
            ret.AdditionalData = this.AdditionalData;
            ret.DataManager = this.DataManager;
            ret.DataTable = this.DataTable;
            ret.FieldMappingDataView = this.FieldMappingDataView;
            ret.SecureData = this.SecureData;

            ret.CancelReason = this.CancelReason;
            ret.IsCancelled = this.IsCancelled;
            ret.ReadOptions = this.ReadOptions.Clone();

            return ret;
        }
    }
}
