using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.DatabaseInspector.SqlServer {
	public class SqlServerConstraintInfo : ConstraintInfo {


        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
			throw new NotImplementedException();
		}

		public override DataRow FillRow(DataRow dr) {
			throw new NotImplementedException();
		}

		internal SqlServerConstraintInfo(TableInfo parent)
			: base(parent) {
		}
		public override string ToString() {
			return base.ToString();
		}

		protected override string generateDropSql() {
			return "ALTER TABLE [" + TableName + "] DROP CONSTRAINT [" + ConstraintName + "]";
		}

		internal override string generateCreateSql() {

			/* ALTER TABLE [table name] WITH CHECK ADD CONSTRAINT [constraint name] FOREIGN KEY (fieldlist1) REFERENCES [table2] (fieldlist2) */

			string srcTbl = "[" + TableName + "]";

			StringBuilder sb = new StringBuilder();

			sb.Append("ALTER TABLE ")
				.Append(srcTbl)
				.Append(" WITH CHECK ADD CONSTRAINT [")
				.Append(this.ConstraintName)
				.Append("] FOREIGN KEY (");

			foreach (FieldInfo fi in this.SourceFields) {
				sb.Append("[").Append(fi.Name).Append("], ");
			}
			sb.Remove(sb.Length - 2, 2);

			sb.Append(") REFERENCES [")
				.Append(this.ReferencesTableName)
				.Append("] (");

			foreach (FieldInfo fi2 in this.ReferencesFields) {
				sb.Append("[")
					.Append(fi2.Name)
					.Append("], ");
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append(")");

//			sb.Append(" GO ALTER TABLE ").Append(srcTbl).Append(" CHECK CONSTRAINT [").Append(this.ConstraintName).Append("]");


			string ret = sb.ToString();
			return ret;
		}
	}
}
