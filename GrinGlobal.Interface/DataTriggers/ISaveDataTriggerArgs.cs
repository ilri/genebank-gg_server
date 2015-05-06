using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Interface.DataTriggers {
	public interface ISaveDataTriggerArgs {
        /// <summary>
        /// Gets the reason the trigger is cancelling the save.  Can be set by callng Cancel() method.
        /// </summary>
		string CancelReason { get; set; }
        /// <summary>
        /// Gets if the trigger requests the save to be cancelled.  Can by set by calling Cancel() method.
        /// </summary>
		bool IsCancelled { get; set; }
        /// <summary>
        /// Tells SecureData to cancel the save for this row.  Can be reversed by calling Uncancel().
        /// </summary>
        /// <param name="reason"></param>
		void Cancel(string reason);
        /// <summary>
        /// Marks the row as no longer being cancelled (i.e. clears CancelReason and sets IsCancelled = false)
        /// </summary>
		void Uncancel();
        /// <summary>
        /// The DataTable object defined by the dataview and filled by the user's editing actions.
        /// </summary>
        DataTable DataTable { get; set; }
        /// <summary>
        /// What the row in the database currently contains.  Meaningless during TableSaving or TableSaved.
        /// </summary>
        DataRow RowInDatabase { get; set; }
        /// <summary>
        /// What the row that is to be written to the database contains.  Meaningless during TableSaving or TableSaved.
        /// </summary>
        DataRow RowToSave { get; set; }

        /// <summary>
        /// Gets the helper object for accessing data safely regardless of dataview definition
        /// </summary>
        DataTriggerHelper Helper { get; set; }

        /// <summary>
        /// Field mapping information for the fields in the row relevant to the table in the database.  Note the DataTable property represents all fields in the dataview, not just those pertaining to the current FieldMappingTable.
        /// </summary>
        ITable Table { get; set; }

        /// <summary>
        /// DataView-level field mapping information for the fields in the row relevant to the table in the database.
        /// </summary>
        IDataview Dataview { get; set; }

        /// <summary>
        /// Contains the new primary key value.  If this is an update it contains same as pk for the datarow.  If it is a a delete, it will be 0.  If it is an insert, this value will be the new pk value (and original pk field will contain original pk value)
        /// </summary>
        int NewPrimaryKeyID { get; set; }

        /// <summary>
        /// Contains the original key value. If this is an insert, it will be 0.  Otherwise it will contain the same value as the pk for the datarow.
        /// </summary>
        int OriginalPrimaryKeyID { get; set; }

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
        /// How we will save the data (Insert, Update, Delete, None, Unknown).  Always evaluates to SaveMode.Unknown during TableSaving or TableSaved.
        /// </summary>
        SaveMode SaveMode { get; set; }
        /// <summary>
        /// Gets or sets a reference to an object the caller may use to pass additional information along to the event handler.
        /// </summary>
        object AdditionalData { get; set; }

        /// <summary>
        /// What options were specified when the call to SaveData() occurred
        /// </summary>
        SaveOptions SaveOptions { get; set;  }

        DataTable ReadData(string sql, params object[] parameters);
        DataSet ReadData(string sql, string tableName, DataSet ds, params object[] parameters);

        int WriteData(string sql, params object[] parameters);
        int WriteData(string sql, bool returnID, string primaryKeyFieldName, params object[] parameters);

        /// <summary>
        /// Gets or sets the exception associated with
        /// </summary>
        Exception Exception { get; set; }

        ISaveDataTriggerArgs Clone();
	}
}
