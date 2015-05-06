using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.DatabaseInspector;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector.SqlServer {
	public class SqlServerFieldInfo : FieldInfo {

        internal SqlServerFieldInfo(TableInfo parent)
			: base(parent) {
		}

		protected override string getAutoIncrementString() {
			return IsAutoIncrement ? "IDENTITY(1,1)" : "";
		}

		protected override string getDefaultValueString() {
			if (DefaultValue == null) {
				return "";
			} else if (DefaultValue == DBNull.Value || DefaultValue.ToString() == "{DBNull.Value}") {
				//if (IsNullable) {
				//    return "DEFAULT NULL";
				//} else {
					return "";
				//}
			} else {
                string ret = "";
				if (DataType == typeof(string)) {
                    if (!String.IsNullOrEmpty(DefaultValue as string)) {
                        ret = "DEFAULT (('" + ((string)DefaultValue).Replace("\"", "").Replace("'", "") + "'))";
                    }
                } else if (DataType == typeof(long)) {
                    ret = "DEFAULT ((" + Toolkit.ToInt32(DefaultValue, null) + "))";
                } else if (DataType == typeof(int)) {
                    ret = "DEFAULT ((" + Toolkit.ToInt32(DefaultValue, null) + "))";
                } else if (DataType == typeof(DateTime)) {
                    ret = "DEFAULT ((" + Toolkit.ToDateTime(DefaultValue, null) + "))";
				} else if (DataType == typeof(float)) {
                    ret = "DEFAULT ((" + Toolkit.ToFloat(DefaultValue, null) + "))";
				} else if (DataType == typeof(decimal)) {
                    ret = "DEFAULT ((" + Toolkit.ToDecimal(DefaultValue, null) + "))";
				} else if (DataType == typeof(double)) {
                    ret = "DEFAULT ((" + Toolkit.ToDouble(DefaultValue, null) + "))";
				} else if (DataType == typeof(UInt32)) {
                    ret = "DEFAULT ((" + Toolkit.ToUInt32(DefaultValue, null) + "))";
				} else if (DataType == typeof(UInt16)) {
                    ret = "DEFAULT ((" + Toolkit.ToUInt16(DefaultValue, null) + "))";
				} else if (DataType == typeof(UInt64)) {
                    ret = "DEFAULT ((" + Toolkit.ToUInt64(DefaultValue, null) + "))";
				} else {
                    ret = "DEFAULT ((" + DefaultValue + "))";
				}


                if (ret == "DEFAULT (())") {
                    return "";
                } else {
                    return ret;
                }

			}

		}

		protected override string getNullString() {
			return IsNullable ? "NULL" : "NOT NULL";
		}

		protected override string getUnsignedString() {
			return "";
		}

		protected override string getZerofillString() {
			return "";
		}

		protected override void parseOptions(Options options) {
			base.parseOptions(options);
		}

		protected override string getCharacterSetString() {
			//if (!String.IsNullOrEmpty(CharacterSet)) {
			//    return "CHARACTER SET " + CharacterSet + " ";
			//}
			//if (this.Table.UseCaseSensitiveCollation) {
			//    return "COLLATE SQL_Latin1_General_CP1_CS_AS";
			//} else {

			// TODO: is this the right collation?  I doubt it!
				return "COLLATE SQL_Latin1_General_CP1_CI_AS";
//			}
		}

		internal override string GetDbTypeString() {
			switch (DbType) {
				case DbType.AnsiString:
				case DbType.String:
				case DbType.AnsiStringFixedLength:
				case DbType.StringFixedLength:
					// SQL Server limits unicode data to 4000 chars (8000 bytes) per field
					// if it's bigger than that, use the MAX specification (roughly 2GB per field)
                    if (MaxLength < 1 || MaxLength > 4000) {
                        return "nvarchar(max) ";
                    } else if (MinLength == MaxLength) {
						return "nchar(" + MaxLength + ") ";
                    } else if (MinLength == 1) {
						return "nchar(1) ";
                    } else {
                        return "nvarchar(" + MaxLength + ") ";
                    }
				case DbType.Int32:
					return "int";

                case DbType.Int64:
                    return "bigint";

				case DbType.DateTime2:
				case DbType.DateTime:
					// NOTE: we use datetime2 data type here!
					//       the datetime data type supported dates from year 1752 and later.
					//       we have some dates (SRC.SRCDATE) that are in the 1400's.
					//       this datetime2 data type is only supported in sql server 2008 and higher.
					return "datetime2";
				case DbType.Decimal:
					return "decimal(" + Precision + ", " + Scale + ")";
				case DbType.Boolean:
					throw new NotImplementedException(getDisplayMember("GetDbTypeString{boolean}", "Boolean field types are not supported by this tool.  You must use int, string, date/time, or rational numbers only."));
//					return "bit";
				case DbType.Single:
					return "real";
				case DbType.Double:
					return "float";
				default:
					throw new NotImplementedException(getDisplayMember("GetDbTypeString{default}", "getDbTypeString() does not have DbType={0} mapped in SqlServerFieldInfo!", DbType.ToString()));
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
			string unsignedString = getUnsignedString();
			string zerofillString = getZerofillString();
			string nullString = getNullString();
			string autoIncrementString = getAutoIncrementString();
			string defaultValueString = getDefaultValueString();
			string characterSet =  null;
			if (typeString.Contains("char(")) {
				characterSet = getCharacterSetString();
			}

			//   `id` INTEGER IDENTITY(1,1) NOT NULL DEFAULT 'Hi',
			string output = String.Format("[{0}] {1} {2} {3} {4} {5},\r\n", Name, typeString, autoIncrementString, characterSet, nullString, defaultValueString);
			return output;
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "SqlServerFieldInfo", resourceName, null, defaultValue, substitutes);
        }
        //public override string GenerateCreateSql() {
		//}
	}
}
