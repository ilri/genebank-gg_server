using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.Text;

namespace GrinGlobal.DatabaseInspector.SqlServer {
	public class SqlServerIndexInfo : IndexInfo {




        internal SqlServerIndexInfo(TableInfo parent)
			: base(parent) {
		}

		protected override string generateDropSql() {
			// DROP INDEX tablename.indexname
			return "DROP INDEX [" + TableName + "].[" + IndexName + "]";
		}

        protected override string generateRebuildSql() {
            throw new NotImplementedException();
        }

		internal override string generateCreateSql() {
			/*
			 * CREATE NONCLUSTERED INDEX [IX_TEST_1] ON [dbo].[acc] 
(
	[ACNO] ASC,
	[ACS] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
			 */

			// OR

			/****** Object:  Index [IX_Table_1]    Script Date: 09/04/2008 17:51:43 *****
			ALTER TABLE [dbo].[Table_1] ADD  CONSTRAINT [IX_Table_1] UNIQUE NONCLUSTERED 
			(
				[test1] ASC
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
				*/

			StringBuilder sb = new StringBuilder();
			StringBuilder sbPKs = new StringBuilder();
			string idx = "[" + IndexName + "]";
			string tbl = "[" + TableName + "]";

			string type = " NONCLUSTERED INDEX ";
			if (Fields.Count == 1 && Fields[0].IsAutoIncrement) {
				// autoincrements are always clustered
				type = " CLUSTERED INDEX ";
			}


			sb.Append("CREATE ")
				.Append(IndexKind)
				.Append(type)
				.Append(idx)
				.Append(" ON ")
				.Append(tbl)
				.Append(" (");

			foreach (FieldInfo fi in Fields) {
				sb.Append("[").Append(fi.Name);
				//if (fi.DataType == typeof(string)) {
				//    sb.Append("(")
				//        .Append(fi.MaxLength)
				//        .Append(")");
				//}
				sb.Append("], ");
			}
			if (Fields.Count > 0) {
				sb.Remove(sb.Length - 2, 2);
			}

			sb.Append(") ON [PRIMARY]");

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
