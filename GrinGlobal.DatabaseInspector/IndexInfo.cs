using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using GrinGlobal.Core;
using GrinGlobal.DatabaseInspector.MySql;
using GrinGlobal.DatabaseInspector.SqlServer;
using GrinGlobal.DatabaseInspector.Oracle;
using GrinGlobal.DatabaseInspector.PostgreSql;

namespace GrinGlobal.DatabaseInspector {
	public abstract class IndexInfo : BaseInfo {

		public static IndexInfo GetInstance(TableInfo parent) {
			DataVendor vendor = DataManager.GetVendor(parent.DataConnectionSpec);
			switch (vendor) {
				case DataVendor.MySql:
					return new MySqlIndexInfo(parent);
				case DataVendor.SqlServer:
					return new SqlServerIndexInfo(parent);
				case DataVendor.Oracle:
				case DataVendor.ODBC:
					return new OracleIndexInfo(parent);
                case DataVendor.PostgreSql:
                    return new PostgreSqlIndexInfo(parent);
			}

			throw new NotImplementedException(getDisplayMember("GetInstance", "IndexInfo class for {0} is not mapped through IndexInfo.GetInstance().",  vendor.ToString()));

		}


		public TableInfo Table { get; set; }

		private string _indexName;
		public string IndexName {
			get {
				return _indexName;
			}
			set {
				_indexName = (value == null ? null : value.ToLower());
			}
		}

        public string FullIndexName {
            get {
                if (String.IsNullOrEmpty(SchemaName)) {
                    return Delimiter + IndexName + Delimiter;
                } else {
                    return Delimiter + SchemaName + Delimiter + "." + Delimiter + IndexName + Delimiter;
                }
            }
        }

		/// <summary>
		/// UNIQUE | FULLTEXT | SPATIAL
		/// </summary>
		public string IndexKind { get; set; }
		/// <summary>
		/// BTREE | HASH | RTREE
		/// </summary>
		public string IndexType { get; set; }

		public List<FieldInfo> Fields { get; set; }

		public IndexInfo(TableInfo parent)
			: base(parent.DataConnectionSpec) {
			Table = parent;
			Fields = new List<FieldInfo>();
            if (parent != null) {
                _delimiter = parent.Delimiter;
                _schemaName = parent.SchemaName;
            }
        }

		protected override void fillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables) {

			// pull info from datarow
			this.TableName = dr["table_name"].ToString().ToLower();
			this.IndexKind = dr["index_kind"].ToString();
			this.IndexName = dr["index_name"].ToString().ToLower();
			this.IndexType = dr["index_type"].ToString();

            DataTable dtIndexField = ds.Tables["IndexFieldInfo"];
            DataRow[] drFields = dtIndexField.Select("parent_id = " + dr["id"]);
            foreach (DataRow drIndexField in drFields) {
                Fields.Add(this.Table.Fields.Find(idxField => {
                    return idxField.Name.ToUpper().Trim() == drIndexField["field_name"].ToString().ToUpper().Trim();
                }));
            }

            //// look up all fields associated with this table
            //foreach (DataRow dr2 in ds.Tables["IndexFieldInfo"].Rows) {
            //    if (dr2["table_name"].ToString().ToUpper().Trim() == TableName.ToUpper().Trim()) {
            //        FieldInfo fi = FieldInfo.GetInstance(this.Table);
            //        fi.FillFromDataSet(dr2, null, tables);
            //        Fields.Add(fi);
            //    }
            //}

		}

		public void FillFromDataSet(DataSet ds, DataRow dr) {
			fillFromDataSet(ds, dr, null);
		}

		public string GenerateCreateScript() {
			return generateCreateSql();
		}

		public void RunCreateScript() {
			string sql = generateCreateSql();
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				dm.Write(sql);
			}
		}

        protected abstract string generateRebuildSql();

        public void RunRebuildScript() {
            string sql = generateRebuildSql();
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                dm.Write(sql);
            }
        }

		public void RunDropScript(bool eatErrors) {
			string sql = generateDropSql();
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				try {
					dm.Write(sql);
				} catch {
					if (!eatErrors) {
						throw;
					}
				}
			}
		}

		public void RunScript() {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				switch (SyncAction) {
					case SyncAction.Nothing:
						return;
					case SyncAction.Alter:
						RunDropScript(true);
						RunCreateScript();
						break;
					case SyncAction.Create:
						RunCreateScript();
						break;
					case SyncAction.Drop:
						RunDropScript(false);
						break;
				}
			}
		}

		public IndexInfo Clone(TableInfo newParent) {
			IndexInfo ii = IndexInfo.GetInstance(newParent);
			ii.DataConnectionSpec = this.DataConnectionSpec;
			ii.IndexKind = IndexKind;
			ii.IndexName = IndexName;
			ii.IndexType = IndexType;
			ii.SyncAction = SyncAction;
			ii.TableName = newParent.TableName;
			foreach (FieldInfo fi in Fields) {
				ii.Fields.Add(fi.Clone(newParent));
			}
			return ii;
		}


		public override string ToString() {
			return this.TableName + "." + this.IndexName;
		}

		public DataRow FillRow(int parentID, DataRow dr) {
			dr["id"] = dr.Table.Rows.Count;
			dr["parent_id"] = parentID;
			dr["table_name"] = TableName.ToLower();
			dr["index_name"] = IndexName.ToLower();
			dr["index_kind"] = IndexKind;
			dr["index_type"] = IndexType;
			return dr;
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "IndexInfo", resourceName, null, defaultValue, substitutes);
        }
    }
}
