using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.Text;

namespace GrinGlobal.DatabaseInspector.Oracle {
	public class OracleIndexInfo : IndexInfo {


		public override string TableName {
			get {
				return _tableName;
			}
			set {
                _tableName = value;
			}
		}


        internal OracleIndexInfo(TableInfo parent)
			: base(parent) {
		}

		protected override string generateDropSql() {
			// DROP INDEX tablename.indexname
			return "DROP INDEX " + FullIndexName;
		}

        protected override string generateRebuildSql() {
            return "ALTER INDEX " + FullIndexName + " REBUILD";
        }

		internal override string generateCreateSql() {

			/*
			 * CREATE [UNIQUE] INDEX index_name ON
table_name(column_name[, column_name...])
TABLESPACE table_space;
			 */

			StringBuilder sb = new StringBuilder();
			StringBuilder sbPKs = new StringBuilder();
			
			sb.Append("CREATE ")
				.Append((IndexKind == null ? "" : IndexKind))
				.Append(" INDEX ")
				.Append((IndexName == null ? "" : FullIndexName.ToUpper()))
				.Append(" ");
			sb.Append(" ON ")
				.Append(FullTableName.ToUpper())
				.Append(" (");

			foreach (FieldInfo fi in Fields) {
//				sb.Append(@"""").Append(fi.Name).Append(@""", ");
				sb.Append(fi.Name).Append(", ");
			}
			if (Fields.Count > 0) {
				sb.Remove(sb.Length - 2, 2);
			}

			sb.Append(")");

			string ret = sb.ToString();
			return ret;
		}


        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
			throw new NotImplementedException();
		}

		public override DataRow FillRow(DataRow dr) {
			throw new NotImplementedException();
		}


	}
}
