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

namespace GrinGlobal.DatabaseInspector.MySql {
	public class MySqlCreator : Creator {

        internal MySqlCreator(DataConnectionSpec dcs)
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
                        string toRun = ((MySqlTableInfo)ti).GenerateCreateAllConstraintsForTable(createForeignConstraints, createSelfReferentialConstraints);
                        showProgress(getDisplayMember("CreateConstraints{progress}", "Creating all constraints for {0}...", ti.TableName), 0, 0);
                        dm.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each table index recreation (!!!)
                        if (!String.IsNullOrEmpty(toRun)) {
                            dm.Write(toRun);
                        }
                    }
                });
            }
        }

        public override void CreateIndexes(List<TableInfo> tables) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);

                // MySQL creates indexes orders of magnitude faster (in InnoDB engine at least)
                // if we issue a single ALTER TABLE statement with multiple indexes on it.
                ProcessTables(tables, ti => {
                    string toRun = null;
                    try {
                        if (ti.IsSelected) {
                            toRun = ((MySqlTableInfo)ti).GenerateCreateAllIndexesForTable();
                            showProgress(getDisplayMember("CreateIndexes{progress}", "Creating all indexes for {0}...", ti.TableName), 0, 0);
                            dm.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each table index recreation (!!!)
                            if (!String.IsNullOrEmpty(toRun)) {
                                dm.Write(toRun);
                            }
                        }
                    } catch (Exception ex) {
                        throw new InvalidOperationException(getDisplayMember("CreateIndexes{failed}", "{0}.  SQL={1}", ex.Message, toRun), ex);
                    }
                });
            }
        }

        public override void RebuildIndexes(List<TableInfo> tables) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);

                // MySQL creates indexes orders of magnitude faster (in InnoDB engine at least)
                // if we issue a single ALTER TABLE statement with multiple indexes on it.
                ProcessTables(tables, ti => {
                    if (ti.IsSelected) {
                        string toRun = null;
                        try {
                            toRun = ((MySqlTableInfo)ti).GenerateRebuildAllIndexesForTable();
                            showProgress(getDisplayMember("RebuildIndexes{progress}", "Rebuilding all indexes for {0}...", ti.TableName), 0, 0);
                            dm.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each table index recreation (!!!)
                            if (!String.IsNullOrEmpty(toRun)) {
                                dm.Write(toRun);
                            }
                        } catch (Exception ex) {
                            throw new InvalidOperationException(ex.Message + ".  SQL=" + toRun, ex);
                        }
                    }
                });
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "MySqlCreator", resourceName, null, defaultValue, substitutes);
        }
    }
}
