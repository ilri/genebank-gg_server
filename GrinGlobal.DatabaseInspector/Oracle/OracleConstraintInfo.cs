using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.DatabaseInspector.Oracle {
	public class OracleConstraintInfo : ConstraintInfo {

		public override string TableName {
			get {
				return _tableName;
			}
			set {
                _tableName = value;
			}
		}

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
			throw new NotImplementedException();
		}

		public override DataRow FillRow(DataRow dr) {
			throw new NotImplementedException();
		}

        internal OracleConstraintInfo(TableInfo parent)
			: base(parent) {
		}

		public override string ToString() {
			return base.ToString();
		}


		protected override string generateDropSql() {
            return "ALTER TABLE " + FullTableName.ToUpper() + " DROP CONSTRAINT " + FullConstraintName.ToUpper();
        }


        public string GeneratePartialCreateScript() {

            StringBuilder sb = new StringBuilder();

            sb.Append(" ADD CONSTRAINT `")
                .Append(this.FullConstraintName.ToUpper())
                .Append("` FOREIGN KEY `NDX_")
                .Append(this.ConstraintName.ToUpper())
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

			/*
			 * Oracle> ALTER TABLE aact ADD CONSTRAINT FK_AACT_ACID FOREIGN KEY (ACID) REFERENCES ACC (ACID)
			 */

			StringBuilder sb = new StringBuilder();

			sb.Append("ALTER TABLE ")
                .Append(FullTableName.ToUpper())
				.Append(@" ADD CONSTRAINT ")
                .Append(ConstraintName.ToUpper())  // oracle doesn't like the schema name put here, so we use the constraint name only
				.Append(@" FOREIGN KEY (");

			foreach (FieldInfo fi in this.SourceFields) {
				sb.Append(fi.Name).Append(@", ");
			}
			sb.Remove(sb.Length - 2, 2);

			sb.Append(@") REFERENCES ")
                .Append(this.ReferencesFullTableName.ToUpper())
				.Append(@" (");

			foreach (FieldInfo fi2 in this.ReferencesFields) {
				sb.Append(fi2.Name).Append(", ");
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append(")"); // ON DELETE RESTRICT ON UPDATE RESTRICT)");


			string ret = sb.ToString();
			return ret;
		}
	}
}
