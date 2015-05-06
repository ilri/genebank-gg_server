using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.DatabaseInspector.Sqlite {
	public class SqliteConstraintInfo : ConstraintInfo {

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
			throw new NotImplementedException();
		}

		public override DataRow FillRow(DataRow dr) {
			throw new NotImplementedException();
		}

        internal SqliteConstraintInfo(TableInfo parent)
			: base(parent) {
		}

		public override string ToString() {
			return base.ToString();
		}


		protected override string generateDropSql() {
			return "ALTER TABLE " + FullTableName + " DROP FOREIGN KEY `" + ConstraintName + "`";
		}

		public string GeneratePartialCreateScript() {

			StringBuilder sb = new StringBuilder();

			sb.Append(" ADD CONSTRAINT `")
				.Append(this.ConstraintName)
				.Append("` FOREIGN KEY `ndx_")
				.Append(this.ConstraintName)
				.Append("` (");

			foreach (FieldInfo fi in this.SourceFields) {
				sb.Append("`").Append(fi.Name).Append("`, ");
			}
			sb.Remove(sb.Length - 2, 2);

			sb.Append(") REFERENCES ")
				.Append(this.ReferencesTable.FullTableName)
				.Append(" (");

			foreach (FieldInfo fi2 in this.ReferencesFields) {
				sb.Append(Delimiter)
					.Append(fi2.Name)
					.Append(Delimiter).Append(", ");
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append(") ON DELETE RESTRICT ON UPDATE RESTRICT");


			string ret = sb.ToString();
			return ret;
		}

		internal override string generateCreateSql() {

			/* ALTER TABLE 'table name' ADD FOREIGN KEY 'constraint name' (fieldlist1) REFERENCES 'table2' (fieldlist2) */
			/*
			 * mysql> ALTER TABLE `prod`.`aact` ADD CONSTRAINT `FK_AACT_ACID` FOREIGN KEY `FK_AACT_ACID` (`ACID`) REFERENCES `ACC` (`ACID`) ON DELE
TE RESTRICT ON UPDATE RESTRICT;
			 */

			string srcTbl = "`" + TableName + "`";

			StringBuilder sb = new StringBuilder();

			sb.Append("ALTER TABLE ")
				.Append(Table.FullTableName)
				.Append(GeneratePartialCreateScript());
			string ret = sb.ToString();
			return ret;
		}
	}
}
