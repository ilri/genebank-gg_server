using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Interface.Dataviews;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.Core;
using GrinGlobal.Business;

namespace GrinGlobal.Business.DataTriggers {
	public class SaveDataTriggerArgs : ISaveDataTriggerArgs {
        /// <summary>
        /// Gets the reason the trigger is cancelling the save.  Can be set by callng Cancel() method.
        /// </summary>
		public string CancelReason { get; set; }
        /// <summary>
        /// Gets if the trigger requests the save to be cancelled.  Can by set by calling Cancel() method.
        /// </summary>
		public bool IsCancelled { get; set; }
        /// <summary>
        /// Tells SecureData to cancel the save for this row.  Can be reversed by calling Uncancel().
        /// </summary>
        /// <param name="reason"></param>
		public void Cancel(string reason){
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
        /// What the row in the database currently contains.  Meaningless during TableSaving or TableSaved.
        /// </summary>
        public DataTable DataTable { get; set; }
        /// <summary>
        /// What the row in the database currently contains.  Meaningless during TableSaving or TableSaved.
        /// </summary>
        public DataRow RowInDatabase { get; set; }
        /// <summary>
        /// What the row that is to be written to the database contains.  Meaningless during TableSaving or TableSaved.
        /// </summary>
        public DataRow RowToSave { get; set; }
        /// <summary>
        /// Field mapping information for the fields in the row relevant to the table in the database.  Note the DataTable property represents all fields in the dataview, not just those pertaining to the current FieldMappingTable.
        /// </summary>
        public ITable Table { get; set; }

        /// <summary>
        /// DataView-level field mapping information for the fields in the row relevant to the table in the database.
        /// </summary>
        public IDataview Dataview { get; set; }

        /// <summary>
        /// Contains the new primary key value.  If this is an update it contains same as pk for the datarow.  If it is a a delete, it will be 0.  If it is an insert, this value will be the new pk value (and original pk field will contain original pk value)
        /// </summary>
        public int NewPrimaryKeyID { get; set; }

        /// <summary>
        /// Contains the original key value. If this is an insert, it will be 0.  Otherwise it will contain the same value as the pk for the datarow.
        /// </summary>
        public int OriginalPrimaryKeyID { get; set; }

        public DataManager DataManager;
        public SecureData SecureData;

        /// <summary>
        /// Gets the helper object for safely accessing data regardless of dataview definition
        /// </summary>
        public DataTriggerHelper Helper { get; set; }

        public int UserID { get { return SecureData.SysUserID; } }
        public int CooperatorID { get { return SecureData.CooperatorID; } }
        public int LanguageID { get { return SecureData.LanguageID; } }
        public string UserName { get { return SecureData.SysUserName; } }

        /// <summary>
        /// How we will save the data (Insert, Update, Delete, None, Unknown).  Always evaluates to SaveMode.Unknown during TableSaving or TableSaved.
        /// </summary>
        public SaveMode SaveMode { get; set; }
        /// <summary>
        /// Gets or sets a reference to an object the caller may use to pass additional information along to the event handler.
        /// </summary>
        public object AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets the options the caller specified in the call to the SaveData method
        /// </summary>
        public SaveOptions SaveOptions { get; set; }

        public DataTable ReadData(string sql, params object[] parameters) {
            if (parameters != null && parameters.Length == 1 && parameters[0] is DataParameters) {
                return DataManager.Read(sql, parameters[0] as DataParameters);
            } else {
                return DataManager.Read(sql, new DataParameters(parameters));
            }
        }
        public DataSet ReadData(string sql, string tableName, DataSet ds, params object[] parameters) {
            if (parameters != null && parameters.Length == 1 && parameters[0] is DataParameters) {
                return DataManager.Read(sql, ds, tableName, parameters[0] as DataParameters);
            } else {
                return DataManager.Read(sql, ds, tableName, new DataParameters(parameters));
            }
        }

        public object ReadValue(string sql, params object[] parameters) {
            if (parameters != null && parameters.Length == 1 && parameters[0] is DataParameters) {
                return DataManager.ReadValue(sql, parameters[0] as DataParameters);
            } else {
                return DataManager.ReadValue(sql, new DataParameters(parameters));
            }
        }


        public int WriteData(string sql, params object[] parameters) {
            if (parameters != null && parameters.Length == 1 && parameters[0] is DataParameters) {
                return DataManager.Write(sql, parameters[0] as DataParameters);
            } else {
                return DataManager.Write(sql, new DataParameters(parameters));
            }
        }
        public int WriteData(string sql, bool returnID, string nameOfSequenceOrPrimaryKeyField, params object[] parameters) {
            if (parameters != null && parameters.Length == 1 && parameters[0] is DataParameters) {
                return DataManager.Write(sql, returnID, nameOfSequenceOrPrimaryKeyField, parameters[0] as DataParameters);
            } else {
                return DataManager.Write(sql, returnID, nameOfSequenceOrPrimaryKeyField, new DataParameters(parameters));
            }
        }

        public Exception Exception { get; set; }


        public ISaveDataTriggerArgs Clone() {
            SaveDataTriggerArgs ret = new SaveDataTriggerArgs();
            ret.AdditionalData = this.AdditionalData;
            ret.CancelReason = this.CancelReason;
            ret.DataTable = this.DataTable;
            ret.DataManager = this.DataManager;
            ret.SecureData = this.SecureData;
            ret.Dataview = this.Dataview;
            ret.Table = this.Table.Clone();
            ret.IsCancelled = this.IsCancelled;
            ret.NewPrimaryKeyID = this.NewPrimaryKeyID;
            ret.OriginalPrimaryKeyID = this.OriginalPrimaryKeyID;
            ret.RowInDatabase = this.RowInDatabase;
            ret.RowToSave = this.RowToSave;
            ret.SaveMode = this.SaveMode;
            ret.SecureData = this.SecureData;
            ret.SaveOptions = this.SaveOptions.Clone();

            ret.Exception = this.Exception;

            return ret as ISaveDataTriggerArgs;
        }

}
}
