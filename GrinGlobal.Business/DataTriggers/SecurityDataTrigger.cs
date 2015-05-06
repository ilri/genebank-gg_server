using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.DataTriggers;
using System.Data;

namespace GrinGlobal.Business.DataTriggers {
    public class SecurityDataTrigger : IDataviewSaveDataTrigger, ITableSaveDataTrigger, IDataviewReadDataTrigger {
        #region ITableSaveDataTrigger Members

        public void TableSaving(ISaveDataTriggerArgs args) {
            // TODO: check security here!!!
        }

        public void TableRowSaving(ISaveDataTriggerArgs args) {
            // TODO: check security here!!!
        }

        public void TableRowSaved(ISaveDataTriggerArgs args) {
        }

        public void TableRowSaveFailed(ISaveDataTriggerArgs args) {
        }

        public void TableSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
        }

        #endregion

        #region IDataViewSaveDataTrigger Members

        public void DataViewSaving(ISaveDataTriggerArgs args) {
        }

        public void DataViewRowSaving(ISaveDataTriggerArgs args) {

            string tableName = null;
            string pkValue = null;

            if (args.Dataview.Tables != null && args.Dataview.Tables.Count > 0) {
                tableName = args.Dataview.PrimaryKeyTableName;
            }

            var args2 = args as SaveDataTriggerArgs;

            switch (args2.SaveMode) {
                case SaveMode.Update:
                    if (!args2.SecureData.CanUpdate(args2.DataManager, args.RowToSave, args.RowInDatabase, args.Dataview)) {
                        if (args.Dataview.PrimaryKeyNames != null && args.Dataview.PrimaryKeyNames.Count > 0 && args.Dataview.Tables.Count > 0){
                            pkValue = args.RowToSave[args.Dataview.PrimaryKeyNames[0]].ToString();
                        }
                        throw Library.DataPermissionException(args2.SecureData.SysUserName, tableName, Permission.Update, new string[] { pkValue });
                    }
                    break;
                case SaveMode.Delete:
                    if (!args2.SecureData.CanDelete(args2.DataManager, args.RowToSave, args.RowInDatabase, args.Dataview)) {
                        pkValue = args.RowToSave[args.Dataview.PrimaryKeyNames[0], DataRowVersion.Original].ToString();
                        throw Library.DataPermissionException(args2.SecureData.SysUserName, tableName, Permission.Delete, new string[] { pkValue });
                    }
                    break;
                case SaveMode.Insert:
                    if (!args2.SecureData.CanCreate(args2.DataManager, args.RowToSave, args.RowInDatabase, args.Dataview)) {
                        if (args.Dataview.PrimaryKeyNames != null && args.Dataview.PrimaryKeyNames.Count > 0 && args.Dataview.Tables.Count > 0) {
                            pkValue = args.RowToSave[args.Dataview.PrimaryKeyNames[0]].ToString();
                        }
//                        pkValue = args.OriginalPrimaryKeyID.ToString();
                        throw Library.DataPermissionException(args2.SecureData.SysUserName, tableName, Permission.Create, new string[] { pkValue });
                    }
                    break;
                case SaveMode.Unknown:
                case SaveMode.None:
                default:
                    break;
            }


        }

        public void DataViewRowSaved(ISaveDataTriggerArgs args) {
        }

        public void DataViewRowSaveFailed(ISaveDataTriggerArgs args) {
        }

        public void DataViewSaved(ISaveDataTriggerArgs args, int successfulSaveCount, int failedSaveCount) {
        }

        #endregion

        #region IAsyncDataTrigger Members

        public bool IsAsynchronous {
            get {
                return false;
            }
        }

        public virtual object Clone() {
            return this;
        }

        #endregion


        #region IDataViewReadDataTrigger Members

        private Permission[] _dataviewPermissions;
        private bool _canReadAll = false;
        private bool _checkRows = false;

        public void DataViewReading(IReadDataTriggerArgs args) {

            var args2 = args as ReadDataTriggerArgs;

            var sd = args.SecureData as SecureData;
            // first, we see what kind of access they have to the entire dataview
            // if the Read permission is VariesByRow, we need to inspect each row.
            // otherwise we can juse use the dataview-level permissions.
            _dataviewPermissions = sd.GetEffectivePermissions(args.UserID, args.FieldMappingDataView.DataViewName, args.FieldMappingDataView.PrimaryKeyTableName);

            // if it's a system dataview, let them have read access regardless of security...
            //if (args.FieldMappingDataView.IsRequiredBySystem) {
            //    _dataviewPermissions[(int)PermissionAction.Read].Level = PermissionLevel.Allow;
            //}
            if (args.FieldMappingDataView.CategoryCode.ToLower().Trim() == "system") {
                _dataviewPermissions[(int)PermissionAction.Read].Level = PermissionLevel.Allow;
            }


            switch (_dataviewPermissions[(int)PermissionAction.Read].Level) {
                case PermissionLevel.Inherit:
                case PermissionLevel.Deny:
                default:
                    // has no access, flag as such

                    // has no access, prevent read altogether
                    args.Cancel("Access denied. User '" + args.UserName + "' does not have access to read data in dataview '" + args.FieldMappingDataView.DataViewName + "'.");

                    _canReadAll = false;
                    _checkRows = false;

                    break;
                case PermissionLevel.Allow:
                    // has access to all rows, flag as such
                    _canReadAll = true;
                    _checkRows = false;
                    break;
                case PermissionLevel.VariesByRow:
                    // we'll need to inspect each row and only return some of them.
                    _canReadAll = false;
                    _checkRows = true;
                    break;
            }
        }

        public void DataViewRowRead(IReadDataTriggerArgs args) {

            // NOTE: if we call args.Cancel() in this handler, the framework will remove the current row (args.DataRow) from the DataTable that will be returned (args.DataTable).

            if (_checkRows) {
                var args2 = args as ReadDataTriggerArgs;
                if (!(args2.SecureData as SecureData).CanRead(args2.DataManager, args2.DataRow, args2.DataRow, args2.FieldMappingDataView)) {
                    // user does not have access to this row, suppress it from output by cancelling
                    args.Cancel("unauthorized");
                }
            }
        }

        public void DataViewRead(IReadDataTriggerArgs args) {
            // NOTE: if we call args.Cancel() in this handler, the framework will remove all rows from the DataTable that will be returned (args.DataTable).
            if (!_canReadAll && !_checkRows) {
                args.Cancel("Access denied. User '" + args.UserName + "' does not have access to dataview '" + args.FieldMappingDataView.DataViewName + "'.");
            }
        }

        #endregion

        #region IDataTriggerDescription Members

        public string GetDescription(string ietfLanguageTag) {
            return "Implements the Security mechanism used by GRIN-Global.  Only way to disable is by editing web.config and setting DisableSecurity to 'true'.";
        }

        public string GetTitle(string ietfLanguageTag) {
            return "Security Data Trigger";
        }

        #endregion

        #region IDataResource Members

        public string[] ResourceNames {
            get { return null; }
        }

        #endregion
    }
}
