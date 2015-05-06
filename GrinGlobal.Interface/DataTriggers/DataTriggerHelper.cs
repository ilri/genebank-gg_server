using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// Helps safely get/set values from DataRow objects -- checks for field existence before getting/setting value.  Also has generally useful functions, such as ToTitleCase and a safe Substring.  Intended to be used from within a DataTrigger callback context.
    /// </summary>
    public class DataTriggerHelper {

        public DataRow SavingRow { get; set; }
        public DataRow DatabaseRow { get; set; }
        public string AliasName { get; set; }

        public DataTriggerHelper(DataRow savingRow, DataRow databaseRow, string aliasName) {
            SavingRow = savingRow;
            DatabaseRow = databaseRow;
            AliasName = aliasName;
        }

        public void SetValueIfFieldExistsAndIsEmpty(string tableFieldName, object value) {
            var dvf = GetDataviewFieldName(tableFieldName, true);
            if (!String.IsNullOrEmpty(dvf) && IsValueEmpty(tableFieldName)) {
                setValue(dvf, tableFieldName, value, SavingRow.Table.Columns[dvf].DataType, false);
            }
        }

        /// <summary>
        /// Given the tableFieldName (and the AliasName property is set correctly), returns the corresponding ColumnName in the SavingRow or DatabaseRow depending on the fromSavingRow parameter.  Returns null if it does not exist in specified row.
        /// </summary>
        /// <param name="tableFieldName"></param>
        /// <param name="fromSavingRow"></param>
        /// <returns></returns>
        public string GetDataviewFieldName(string tableFieldName, bool fromSavingRow) {
            if (fromSavingRow) {
                if (SavingRow == null) {
                    return null;
                } else {
                    return GetDataviewFieldName(SavingRow.Table, AliasName, tableFieldName);
                }
            } else {
                if (DatabaseRow == null) {
                    return null;
                } else {
                    return GetDataviewFieldName(DatabaseRow.Table, AliasName, tableFieldName);
                }
            }
        }

        /// <summary>
        /// Returns the dataview field name restricted by both the alias and table field name.  Looks at only the "table_alias_name" and "table_field_name" extended properties on each column in given DataTable object.  Returns null if not found.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="aliasName"></param>
        /// <param name="tableFieldName"></param>
        /// <returns></returns>
        public static string GetDataviewFieldName(DataTable dt, string aliasName, string tableFieldName){
            var tfnLower = tableFieldName.ToLower();
            if (aliasName == null) {
                aliasName = string.Empty;
            }

            foreach (DataColumn dc in dt.Columns) {
                if (dc.ExtendedProperties.ContainsKey("table_alias_name") && dc.ExtendedProperties.ContainsKey("table_field_name")) {
                    var tan = dc.ExtendedProperties["table_alias_name"].ToString().ToLower();
                    var tfn = dc.ExtendedProperties["table_field_name"].ToString().ToLower();
                    // auto-appended fields will have the entire alias + field name as their table_field_name extended property, just like their ColumnName
                    // e.g. 
                    //      ColumnName = "tf1.current_taxonomy_family_id"
                    //      ExtendedProperties["table_alias_name"] = "tf1"
                    //      ExtendedProperties["table_field_name"] = "tf1.current_taxonomy_family_id"
                    if (tan == aliasName.ToLower() && (tfn == tfnLower) || (tan + "." + tfnLower == tfn)) {
                        return dc.ColumnName;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the dataview field name restricted by only the table field name.  Looks at only the "table_field_name" extended properties on each column in given DataTable object.  Returns null if not found.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="aliasName"></param>
        /// <param name="tableFieldName"></param>
        /// <returns></returns>
        public static string GetDataviewFieldName(DataTable dt, string tableFieldName) {
            var tfnLower = tableFieldName.ToLower();

            foreach (DataColumn dc in dt.Columns) {
                if (dc.ExtendedProperties.ContainsKey("table_field_name")) {
                    var tfn = dc.ExtendedProperties["table_field_name"].ToString().ToLower();
                    if (tfn == tfnLower) {
                        return dc.ColumnName;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns:  If SavingRow has the given field, returns the value from it.  If the DatabaseRow exists and has the given field, it returns the value from it.  Otherwise returns given defaultValue.  Respects AliasName property.
        /// </summary>
        /// <param name="tableFieldName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="returnDefaultIfFieldIsDBNullOrMissing"></param>
        /// <returns></returns>
        public object GetValue(string tableFieldName, object defaultValue, bool returnDefaultIfFieldIsDBNullOrMissing) {
            return GetValue(tableFieldName, defaultValue, returnDefaultIfFieldIsDBNullOrMissing, false);
        }

        /// <summary>
        /// Returns:  If SavingRow has the given field, returns the value from it.  If the DatabaseRow exists and has the given field, it returns the value from it.  Otherwise returns given defaultValue.  Respects AliasName property.  But if specified, will pull from any alias (preferring AliasName first).
        /// </summary>
        /// <param name="tableFieldName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="returnDefaultIfFieldIsDBNullOrMissing"></param>
        /// <param name="matchAnyAlias"></param>
        /// <returns></returns>
        public object GetValue(string tableFieldName, object defaultValue, bool returnDefaultIfFieldIsDBNullOrMissing, bool matchAnyAlias) {
            object rv = null;

            string dvf = GetDataviewFieldName(tableFieldName, true);
            if (!String.IsNullOrEmpty(dvf)) {
                if (SavingRow != null) {
                    rv = SavingRow[dvf];
                }
            } else {
                dvf = GetDataviewFieldName(tableFieldName, false);
                if (!String.IsNullOrEmpty(dvf)) {
                    if (DatabaseRow != null) {
                        rv = DatabaseRow[dvf];
                    }
                }
            }

            if (rv == null && matchAnyAlias) {
                if (SavingRow != null) {
                    dvf = DataTriggerHelper.GetDataviewFieldName(SavingRow.Table, tableFieldName);
                    if (!String.IsNullOrEmpty(dvf)) {
                        rv = SavingRow[dvf];
                    }
                }
                if (rv == null){
                    if (DatabaseRow != null) {
                        dvf = DataTriggerHelper.GetDataviewFieldName(DatabaseRow.Table, tableFieldName);
                        if (!String.IsNullOrEmpty(dvf)) {
                            rv = DatabaseRow[dvf];
                        }
                    }
                }
            }

            if ((rv == null || rv == DBNull.Value) && returnDefaultIfFieldIsDBNullOrMissing) {
                rv = defaultValue;
            }

            return rv;

        }

        /// <summary>
        /// Gets the DataRowVersion.Original value for given field, or defaultValue if not found.  Respects AliasName property.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object GetOriginalValue(string tableFieldName, object defaultValue) {

            string dvf = GetDataviewFieldName(tableFieldName, true);
            if (!String.IsNullOrEmpty(dvf)) {
                if (SavingRow != null && SavingRow.RowState != DataRowState.Added) {
                    return SavingRow[dvf, DataRowVersion.Original];
                }
            } else {
                dvf = GetDataviewFieldName(tableFieldName, false);
                if (!String.IsNullOrEmpty(dvf)) {
                    if (DatabaseRow != null && DatabaseRow.RowState != DataRowState.Added) {
                        return DatabaseRow[dvf, DataRowVersion.Original];
                    }
                }
            }

            return defaultValue;

        }

        /// <summary>
        /// Returns true if row[columnName] is empty string, null, or DBNull.Value.  If fieldName is not in the row, returns true.  Otherwise returns false.  Respects AliasName property.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool IsValueEmpty(string tableFieldName) {
            var val = GetValue(tableFieldName, null, true);
            return val == null || val.ToString().Trim().Length == 0;
        }

        /// <summary>
        /// Using aliasName and tableFieldName, attempts to find the most specific DataColumn that matches.  Order of specificity is: (1) dc.ExtendedProperties of "table_alias_name" and "table_field_name" match, (2) dc.ColumnName == aliasName.tableFieldName, (3) dc.ColumnName == tableFieldName. If found, returns true if the value for row[matched_column_name] is empty string, null, or DBNull.Value.  If no DataColumn is found that matches criteria, returns true.  Otherwise returns false.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="aliasName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool IsValueEmpty(DataRow dr, string aliasName, string tableFieldName) {
            var dvf = GetDataviewFieldName(dr.Table, aliasName, tableFieldName);
            if (!String.IsNullOrEmpty(dvf)) {
                return String.IsNullOrEmpty(dr[dvf].ToString());
            } else if (dr.Table.Columns.Contains(aliasName + "." + tableFieldName)){
                return String.IsNullOrEmpty(dr[aliasName + "." + tableFieldName].ToString());
            } else if (dr.Table.Columns.Contains(tableFieldName)) {
                return String.IsNullOrEmpty(dr[tableFieldName].ToString());
            } else {
                return true;
            }
        }

        /// <summary>
        /// Returns true if given field exists in SavingRow object.  Tries AliasName.fieldName first, then fieldName.  Respects AliasName property.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool FieldExists(string tableFieldName) {
            var dvf = GetDataviewFieldName(tableFieldName, true);
            return !String.IsNullOrEmpty(dvf);
        }

        /// <summary>
        /// Returns true if given field exists in SavingRow object
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private bool fieldExists(string name) {
            return SavingRow.Table.Columns.Contains(name);
        }

        /// <summary>
        /// Safely assigns value to the SavingRow.  i.e. if necessary, adds given fieldName to the SavingRow (as given fieldType) before assigning it the given value.  Respects AliasName property.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public void SetValue(string tableFieldName, object value, Type fieldType, bool addIfNeeded) {
            var dataviewFieldName = GetDataviewFieldName(tableFieldName, true);
            setValue(dataviewFieldName, tableFieldName, value, fieldType, addIfNeeded);
        }

        private void setValue(string dataviewFieldName, string tableFieldName, object value, Type fieldType, bool addIfNeeded){
            if (String.IsNullOrEmpty(dataviewFieldName)) {
                if (addIfNeeded) {
                    dataviewFieldName = AddFieldIfNeeded(tableFieldName, fieldType);
                } else {
                    // doesn't exist, they said don't add it. just exit.
                    return;
                }
            }

            // note if we try to set value on a deleted row, this will bomb since we're assuming DataRowVersion.Current
            if (SavingRow.Table.Columns[dataviewFieldName].ReadOnly) {
                try {
                    SavingRow.Table.Columns[dataviewFieldName].ReadOnly = false;
                    SavingRow[dataviewFieldName] = value;
                } finally {
                    SavingRow.Table.Columns[dataviewFieldName].ReadOnly = true;
                }
            } else {
                SavingRow[dataviewFieldName] = value;
            }
        }

        /// <summary>
        /// Adds the given field to the SavingRow.Table object if needed.  Respects AliasName property.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public string AddFieldIfNeeded(string tableFieldName, Type fieldType) {
            var dvf = GetDataviewFieldName(tableFieldName, true);
            if (String.IsNullOrEmpty(dvf)) {
                dvf = (String.IsNullOrEmpty(AliasName) ? tableFieldName : AliasName + "." + tableFieldName);
                var dc = new DataColumn(dvf, fieldType);
                dc.ExtendedProperties["table_alias_name"] = AliasName;
                dc.ExtendedProperties["table_field_name"] = tableFieldName;
                SavingRow.Table.Columns.Add(dc);
            }
            return dvf;
        }

        /// <summary>
        /// Returns true if all given fields exist in the SavingRow object.  Respects AliasName property.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public bool AllFieldsExist(params string[] tableFieldNames) {
            foreach (var s in tableFieldNames) {
                var dvf = GetDataviewFieldName(s, true);
                if (String.IsNullOrEmpty(dvf)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the given string as title cased (aka proper cased).  "fred hoiberg" becomes "Fred Hoiberg".  Respects AliasName property.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string ToTitleCase(string tableFieldName) {
            var ti = Thread.CurrentThread.CurrentCulture.TextInfo;
            return ti.ToTitleCase(GetValue(tableFieldName, "", false) + string.Empty);
        }

        /// <summary>
        /// Safely returns the substring of a string (i.e. specifying a startPos past the end of the string or a length that is too long won't cause an exception, just returns what you would expect it to
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="startPos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Substring(string tableFieldName, int startPos, int length) {
            var val = string.Empty + GetValue(tableFieldName, null, false);
            if (String.IsNullOrEmpty(val)) {
                return string.Empty;
            } else {

                if (length < 1) {
                    return string.Empty;
                }

                if (startPos < 0) {
                    startPos = 0;
                }

                if (startPos >= val.Length) {
                    return string.Empty;
                }

                if (startPos + length > val.Length) {
                    return val.Substring(startPos);
                } else {
                    return val.Substring(startPos, length);
                }
            }
        }
    }
}
