using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GrinGlobal.DatabaseInspector.PostgreSql {
    public class PostgreSqlConstraintInfo :ConstraintInfo {

        internal PostgreSqlConstraintInfo(TableInfo parent)
            : base(parent) {
            _delimiter = "";

        }

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
            throw new NotImplementedException();
        }

        internal override string generateCreateSql() {
            StringBuilder sb = new StringBuilder();

            sb.Append("ALTER TABLE ")
                .Append(TableName)
                .Append(GeneratePartialCreateScript());
            string ret = sb.ToString();
            return ret;
        }

        public string GeneratePartialCreateScript() {

            StringBuilder sb = new StringBuilder();

            sb.Append(" ADD CONSTRAINT ")
                .Append(this.ConstraintName)
                .Append(" FOREIGN KEY (");

            foreach (FieldInfo fi in this.SourceFields) {
                sb.Append(fi.Name).Append(", ");
            }
            sb.Remove(sb.Length - 2, 2);

            sb.Append(") REFERENCES ")
                .Append(this.ReferencesTable.TableName)
                .Append(" (");

            foreach (FieldInfo fi2 in this.ReferencesFields) {
                sb.Append(fi2.Name).Append(", ");
            }
            sb.Length -= 2;
            sb.Append(") ON DELETE RESTRICT ON UPDATE RESTRICT");


            string ret = sb.ToString();
            return ret;
        }

        public override DataRow FillRow(System.Data.DataRow dr) {
            throw new NotImplementedException();
        }
    }
}
