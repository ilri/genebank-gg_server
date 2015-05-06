using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.DatabaseInspector;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector.PostgreSql {
    public class PostgreSqlFieldInfo : FieldInfo {

        internal PostgreSqlFieldInfo(TableInfo parent)
            : base(parent) {
            _delimiter = "";
        }

        protected override string getAutoIncrementString() {
            return IsAutoIncrement ? " DEFAULT nextval('sq_" + this.Table.TableName + "') " : "";
        }

        protected override string getDefaultValueString() {
            if (DefaultValue == null || IsAutoIncrement) {
                return "";
            } else if (DefaultValue == DBNull.Value || DefaultValue.ToString() == "{DBNull.Value}") {
                //if (IsNullable) {
                //    return "DEFAULT NULL";
                //} else {
                return "";
                //}
            } else {
                return "DEFAULT " + DefaultValue.ToString().Replace("\"", "'") + " ";
            }
        }

        protected override string getNullString() {
            if (IsAutoIncrement) {
                return "";
            } else {
                return IsNullable ? " NULL " :  "NOT NULL ";
            }
        }

        protected override string getUnsignedString() {
            return "";
            //			return IsUnsigned ? "UNSIGNED" : "";
        }

        protected override string getZerofillString() {
            return "";
            //			return IsZeroFill ? "ZEROFILL" : "";
        }

        protected override void parseOptions(Options options) {
            base.parseOptions(options);
        }

        protected override string getCharacterSetString() {
            //if (this.Table.UseCaseSensitiveCollation) {
            return "";
            //} else {
            //    if (!String.IsNullOrEmpty(CharacterSet)) {
            //        return "CHARACTER SET " + CharacterSet + " ";
            //    }
            //}
            //return "";
        }

        internal override string GetDbTypeString() {
            switch (DbType) {
                case DbType.AnsiString:
                case DbType.String:
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    if (MaxLength < 1 || MaxLength > 4000){
                        return "text ";
                    } else if (MinLength == MaxLength){
                        return "char(" + MaxLength + ") ";
                    } else if (MaxLength == 1){
                        return "char(1) ";
                    } else {
                        return "varchar(" + MaxLength + ") ";
                    }
                case DbType.Int32:
                    return "integer ";
                case DbType.Int64:
                    return "bigint ";
                case DbType.Boolean:
                    throw new NotImplementedException(getDisplayMember("GetDbTypeString{boolean}", "Boolean field types are not supported by this tool.  You must use int, string, date/time, or rational numbers only."));
                case DbType.DateTime:
                case DbType.DateTime2:
                    return "timestamp ";
                case DbType.Decimal:
                    return "decimal(" + Precision + ", " + Scale + ") ";
                case DbType.Double:
                    return "double precision ";
                case DbType.Single:
                    return "real ";

                default:
                    throw new NotImplementedException(getDisplayMember("GetDbTypeString{default}", "getDbTypeString() does not have DbType={0} mapped in PostgreSqlFieldInfo!", DbType.ToString()));
            }
        }

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
            throw new NotImplementedException();
        }

        protected override void fillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables) {
            throw new NotImplementedException();
        }

        public override DataRow FillRow(DataRow dr) {
            throw new NotImplementedException();
        }

        public override string GenerateCreateSql() {
            return generateCreateSql();
        }

        internal override string generateCreateSql() {
            string typeString = GetDbTypeString();
            string nullString = getNullString();
            string autoIncrementString = getAutoIncrementString();
            string defaultValueString = getDefaultValueString();

            //  id   integer NOT NULL PRIMARY KEY DEFAULT nextval('sq_tablename'),
            string output = String.Format("{0} {1} {2} {3} {4} {5},\r\n", Name, typeString, nullString, (IsPrimaryKey ? "PRIMARY KEY " : ""), autoIncrementString, defaultValueString);
            return output;
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "PostgreSqlFieldInfo", resourceName, null, defaultValue, substitutes);
        }

    }
}
