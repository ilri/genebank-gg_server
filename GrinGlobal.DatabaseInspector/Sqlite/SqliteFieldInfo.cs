using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.DatabaseInspector;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector.Sqlite {
    public class SqliteFieldInfo : FieldInfo {

        internal SqliteFieldInfo(TableInfo parent)
            : base(parent) {
		}

		protected override string getAutoIncrementString() {
			return IsAutoIncrement ? "AUTOINCREMENT" : "";
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
				return "DEFAULT '" + DefaultValue.ToString().Replace("\"","") + "'";
			}
		}

		protected override string getNullString() {
			return IsNullable ? "NULL" : "";
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
			//    // this is handled at the table level
			//    return "";
			//} else if (!String.IsNullOrEmpty(CharacterSet)) {
			//    return "CHARACTER SET " + CharacterSet + " ";
			//}
			return "";
		}

		internal override string GetDbTypeString() {
			switch (DbType) {
				case DbType.AnsiString:
				case DbType.String:
				case DbType.AnsiStringFixedLength:
				case DbType.StringFixedLength:
				case DbType.DateTime:
				case DbType.DateTime2:
					return "TEXT";
				case DbType.Int32:
                case DbType.Int64:
                    return "INTEGER";
				case DbType.Decimal:
                case DbType.Double:
                case DbType.Single:
                    return "REAL";
				case DbType.Boolean:
					throw new NotImplementedException(getDisplayMember("GetDbTypeString{boolean}", "Boolean field types are not supported by this tool.  You must use int, string, date/time, or rational numbers only."));
//					return "bit";
				default:
					throw new NotImplementedException(getDisplayMember("GetDbTypeString{default}", "getDbTypeString() does not have DbType={0} mapped in MySqlFieldInfo!", DbType.ToString()));
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
			string characterSet =  getCharacterSetString();

			//   `id` INTEGER NULL AUTO_INCREMENT DEFAULT 'Hi',
			string output = String.Format("{0} {1} {2} {3} {4} {5},\r\n", Name, typeString, characterSet, nullString, autoIncrementString, defaultValueString);
			return output;
		}

		//public override string GenerateCreateSql() {
		//}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "SqliteFieldInfo", resourceName, null, defaultValue, substitutes);
        }
    }
}
