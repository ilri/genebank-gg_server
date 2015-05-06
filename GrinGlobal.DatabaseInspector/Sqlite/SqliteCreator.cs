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

namespace GrinGlobal.DatabaseInspector.Sqlite {
	public class SqliteCreator : Creator {

        internal SqliteCreator(DataConnectionSpec dcs)
            : base(dcs) {
		}


		public override string NormalizeName(string name) {
			return "`" + name + "`";
		}

		public override List<string> ListTableNames(string schema) {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				DataTable dt = dm.Read("show full tables where table_type = 'BASE TABLE';");
				List<string> ret = new List<string>();
				foreach (DataRow dr in dt.Rows) {
					ret.Add(dr[0].ToString());
				}
				return ret;
			}
		}

        public override void CreateConstraints(List<TableInfo> tables, bool createForeignConstraints, bool createSelfReferentialConstraints) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);

                // MySQL creates constraints orders of magnitude faster (in InnoDB engine at least)
                // if we issue a single ALTER TABLE statement with multiple constraints on it.
                ProcessTables(tables, ti => {
                    if (ti.IsSelected) {
                        string toRun = ((SqliteTableInfo)ti).GenerateCreateAllConstraintsForTable(createForeignConstraints, createSelfReferentialConstraints);
                        showProgress(getDisplayMember("CreateConstraints{progress}", "Creating all constraints for {0}...", ti.TableName), 0, 0);
                        dm.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each table index recreation (!!!)
                        if (!String.IsNullOrEmpty(toRun)) {
                            dm.Write(toRun);
                        }
                    }
                });
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "SqliteCreator", resourceName, null, defaultValue, substitutes);
        }

	}
}
