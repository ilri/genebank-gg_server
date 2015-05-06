using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Resources;

using GrinGlobal.DatabaseInspector;

namespace GrinGlobal.DatabaseInspector.SqlServer {
	public class SqlServerCreator : Creator {

        internal SqlServerCreator(DataConnectionSpec dcs)
			: base(dcs) {
		}

		public override string NormalizeName(string name) {
			return "[" + name + "]";
		}

		public override List<string> ListTableNames(string schema) {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				DataTable dt = dm.Read("select * from information_schema.tables where table_name <> 'sysdiagrams' order by table_name");
				List<string> ret = new List<string>();
				foreach (DataRow dr in dt.Rows) {
					ret.Add(dr["table_name"].ToString());
				}
				return ret;
			}
		}

        public override void RebuildIndexes(List<TableInfo> tables) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);

                // MySQL creates indexes orders of magnitude faster (in InnoDB engine at least)
                // if we issue a single ALTER TABLE statement with multiple indexes on it.
                ProcessTables(tables, ti => {
                    if (ti.IsSelected) {
                        string toRun = ((SqlServerTableInfo)ti).GenerateRebuildAllIndexesForTable();
                        showProgress(getDisplayMember("RebuildIndexes{progress}", "Rebuilding all indexes for {0}...", ti.TableName), 0, 0);
                        dm.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each table index recreation (!!!)
                        dm.Write(toRun);
                    }
                });
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "SqlServerCreator", resourceName, null, defaultValue, substitutes);
        }

	}
}
