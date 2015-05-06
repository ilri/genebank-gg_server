using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.DatabaseInspector;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector.Oracle {
	public class OracleFieldInfo : FieldInfo {

        internal OracleFieldInfo(TableInfo parent)
            : base(parent) {
		}

		public override string TableName {
			get {
				return _tableName;
			}
			set {
                _tableName = value;
			}
		}


		protected override string getAutoIncrementString() {
			return "";
//			return IsAutoIncrement ? "AUTO_INCREMENT" : "";
		}

		protected override string getDefaultValueString() {
			if (DefaultValue == null) {
				return "";
			} else if (DefaultValue == DBNull.Value || DefaultValue.ToString() == "{DBNull.Value}" || DefaultValue.ToString() == string.Empty) {
				//if (IsNullable) {
				//    return "DEFAULT NULL";
				//} else {
					return "";
				//}
			} else {
				//return "DEFAULT " + DefaultValue.ToString().Replace("\"", "'") + " ";
                return "DEFAULT '" + DefaultValue.ToString().Replace("\"", "") + "' ";
			}
		}

		protected override string getNullString() {
			return IsNullable ? "NULL " : "NOT NULL ";
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
				return "CHARACTER SET UTF16 ";
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
                    if (MaxLength < 1 || MaxLength > 2000) {
                        //return "NVARCHAR2(2000) ";
                        MaxLength = -1;
                        return "CLOB ";
                        //return "VARCHAR2(2000) ";
                        //                        return "TODO: LONG TEXT TYPE IN ORACLE???";
                    } else if (MinLength == MaxLength) {
                        return "CHAR(" + MaxLength + " CHAR) ";
                    } else if (MaxLength == 1){
                        return "CHAR(1 CHAR) ";
                    } else {
                        return "VARCHAR2(" + MaxLength + " CHAR) ";
                    }
				case DbType.Int32:
					return "NUMBER(9) ";
                case DbType.Int64:
                    return "NUMBER(19) ";
				case DbType.Boolean:
					throw new NotImplementedException(getDisplayMember("GetDbTypeString{boolean}", "Boolean field types are not supported by this tool.  You must use int, string, date/time, or rational numbers only."));
				case DbType.DateTime:
				case DbType.DateTime2:
					return "DATE ";
				case DbType.Decimal:
					return "DECIMAL(" + Precision + ", " + Scale + ") ";
				case DbType.Double:
					return "FLOAT ";
				case DbType.Single:
					return "REAL ";

				default:
					throw new NotImplementedException(getDisplayMember("GetDbTypeString{default}", "getDbTypeString() does not have DbType={0} mapped in OracleFieldInfo!", DbType.ToString()));
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
			string characterSet =  getCharacterSetString();

			//   `id` INTEGER DEFAULT 'Hi' NOT NULL,
			string output = String.Format("{0} {1}{2}{3},\r\n", Name, typeString, defaultValueString, nullString);
			return output;
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "OracleFieldInfo", resourceName, null, defaultValue, substitutes);
        }
        //public override string GenerateCreateSql() {
		//}
	}
}
