using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.Text;

namespace GrinGlobal.DatabaseInspector.Sqlite {
    public class SqliteIndexInfo : IndexInfo {




        internal SqliteIndexInfo(TableInfo parent)
			: base(parent) {
		}

		protected override string generateDropSql() {
			// ALTER TABLE tablename DROP INDEX indexname
			return "ALTER TABLE " + Table.FullTableName + " DROP INDEX `" + IndexName + "`";
		}

        protected override string generateRebuildSql() {
            throw new NotImplementedException();
        }

		internal override string generateCreateSql() {

			/*
			 * CREATE [UNIQUE|FULLTEXT|SPATIAL] INDEX index_name
		[index_type]
		ON tbl_name (index_col_name,...)
		[index_type]
	*/

			StringBuilder sb = new StringBuilder();
			StringBuilder sbPKs = new StringBuilder();
			string idx = "`" + IndexName + "`";
			sb.Append("CREATE ")
				.Append(IndexKind)
				.Append(" INDEX ")
				.Append(idx)
				.Append(" ");
			if (!String.IsNullOrEmpty(IndexType)){
				sb.Append("USING ").Append(IndexType);
			}
			sb.Append(" ON ")
				.Append(Table.FullTableName)
				.Append(" (");

			foreach (FieldInfo fi in Fields) {
				sb.Append("`").Append(fi.Name);
				//if (fi.DataType == typeof(string)) {
				//    sb.Append("(")
				//        .Append(fi.MaxLength)
				//        .Append(")");
				//}
				sb.Append("`, ");
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
