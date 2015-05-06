using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;

namespace GrinGlobal.DatabaseInspector.PostgreSql {
    public class PostgreSqlIndexInfo :IndexInfo {

        internal PostgreSqlIndexInfo(TableInfo parent)
            : base(parent) {
            _delimiter = "";
        }

        protected override string generateRebuildSql() {
            return "REINDEX INDEX " + this.IndexName;
        }

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
            throw new NotImplementedException();
        }

        internal override string generateCreateSql() {

            StringBuilder sb = new StringBuilder();
            StringBuilder sbPKs = new StringBuilder();


            //// postgresql auto-creates an index when we add a unique constraint (and adding an index of kind UNIQUE doesn't work)...
            //if (IndexKind.ToUpper() == "UNIQUE") {
            //    sb.Append("ALTER TABLE ")
            //        .Append(TableName)
            //        .Append(" ADD CONSTRAINT ")
            //        .Append("uc_" + IndexName)
            //        .Append(" UNIQUE ");

            //    sb.Append(" (");

            //    foreach (FieldInfo fi in Fields) {
            //        sb.Append(fi.Name).Append(", ");
            //    }
            //    if (Fields.Count > 0) {
            //        sb.Length -= 2;
            //    }

            //    sb.Append(")");


            //} else {
                sb.Append("CREATE ")
                    .Append(IndexKind)
                    .Append(" INDEX ")
                    .Append(IndexName)
                    .Append(" ");
                sb.Append(" ON ")
                    .Append(TableName);

                if (!String.IsNullOrEmpty(IndexType)) {
                    sb.Append(" USING ").Append(IndexType);
                }

                sb.Append(" (");

                foreach (FieldInfo fi in Fields) {
                    sb.Append(fi.Name).Append(", ");
                    //if (fi.DataType == typeof(string)) {
                    //    sb.Append("(")
                    //        .Append(fi.MaxLength)
                    //        .Append(")");
                    //}
                }
                if (Fields.Count > 0) {
                    sb.Length -= 2;
                }

                sb.Append(")");

            //}

            string ret = sb.ToString();
            return ret;
        }

        public override System.Data.DataRow FillRow(System.Data.DataRow dr) {
            throw new NotImplementedException();
        }
    }
}
