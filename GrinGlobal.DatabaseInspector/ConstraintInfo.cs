using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using GrinGlobal.Core;
using GrinGlobal.DatabaseInspector.MySql;
using GrinGlobal.DatabaseInspector.SqlServer;
using GrinGlobal.DatabaseInspector.Oracle;
using System.Diagnostics;
using GrinGlobal.DatabaseInspector.PostgreSql;

namespace GrinGlobal.DatabaseInspector {
	public abstract class ConstraintInfo : BaseInfo {

		public static ConstraintInfo GetInstance(TableInfo parent) {
			DataVendor vendor = DataManager.GetVendor(parent.DataConnectionSpec);
			switch (vendor) {
				case DataVendor.MySql:
					return new MySqlConstraintInfo(parent);
				case DataVendor.SqlServer:
					return new SqlServerConstraintInfo(parent);
				case DataVendor.Oracle:
					return new OracleConstraintInfo(parent);
                case DataVendor.PostgreSql:
                    return new PostgreSqlConstraintInfo(parent);
			}

            throw new NotImplementedException(getDisplayMember("GetInstance", "ConstraintInfo class for {0} is not mapped through ConstraintInfo.GetInstance().", vendor.ToString()));

		}

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
            
        }

		public TableInfo Table { get; set; }

		private string _constraintName;
		public string ConstraintName {
			get {
				return _constraintName;
			}
			set {
                _constraintName = value; // (value == null ? null : value.ToLower());
			}
		}

        public string FullConstraintName {
            get {
                if (String.IsNullOrEmpty(SchemaName)) {
                    return Delimiter + ConstraintName + Delimiter;
                } else {
                    return Delimiter + SchemaName + Delimiter + "." + Delimiter + ConstraintName + Delimiter;
                }
            }
        }

        private TableInfo _referencesTable;
        public TableInfo ReferencesTable { 
            get {
                if (_referencesTable == null) {

                    // might be already loaded, just wasn't available on the first pass. (think circular references)
                    foreach (TableInfo ti in _tableList) {
                        if (ti.TableName == ReferencesTableName) {
                            _referencesTable = ti;
                            break;
                        }
                    }

                    if (_referencesTable == null) {
                        // was not a circular reference, and has not yet been loaded.  Load it now.
                        _referencesTable = TableInfo.GetInstance(this.DataConnectionSpec);
                        if (_dsToFillFrom != null) {
                            // fill from dataset
                            _referencesTable.FillFromDataSet(_dsToFillFrom, _drToFillFrom, _tableList);
                        } else {
                            // fill from database
                            _referencesTable.Fill(SchemaName, ReferencesTableName, _tableList, false, 0);
                        }
                    }
                }
                return _referencesTable;
            } 
            set { 
                _referencesTable = value; 
            } 
        }

        internal List<TableInfo> TableList {
            get { return _tableList; }
            set { _tableList = value; }
        }

		private string _referencesTableName;
		public string ReferencesTableName {
			get {
				return _referencesTableName;
			}
			set {
				_referencesTableName = (value == null ? null : value.ToLower());
			}
		}

        public string ReferencesFullTableName {
            get {
                if (String.IsNullOrEmpty(SchemaName)) {
                    return Delimiter + ReferencesTableName + Delimiter;
                } else {
                    return Delimiter + SchemaName + Delimiter + "." + Delimiter + ReferencesTableName + Delimiter;
                }
            }
        }

        public List<string> SourceFieldNames { get; private set; }
        public List<string> ReferencesFieldNames { get; private set; }

		public List<FieldInfo> SourceFields { get; private set; }
		public List<FieldInfo> ReferencesFields { get; private set; }

		public bool IsSelfReferential {
			get {
				if (String.IsNullOrEmpty(ReferencesTableName)) {
					return false;
				} else {
					return ReferencesTableName.ToUpper() == TableName.ToUpper();
				}
			}
		}

		/// <summary>
		/// RESTRICT | CASCADE | SET NULL | NO ACTION
		/// </summary>
		public string OnDeleteAction { get; set; }
		/// <summary>
		/// RESTRICT | CASCADE | SET NULL | NO ACTION
		/// </summary>
		public string OnUpdateAction { get; set; }

		public ConstraintInfo(TableInfo parent)
			: base(parent.DataConnectionSpec) {
			Table = parent;
			SourceFields = new List<FieldInfo>();
            SourceFieldNames = new List<string>();
			ReferencesFields = new List<FieldInfo>();
            ReferencesFieldNames = new List<string>();
            if (parent != null) {
                _delimiter = parent.Delimiter;
                _schemaName = parent.SchemaName;
            }
			OnDeleteAction = "RESTRICT";
			OnUpdateAction = "RESTRICT";
		}

		public override string ToString() {
            var ret = ConstraintName + "; " + TableName;
            if (SourceFields == null || SourceFields.Count == 0) {
                ret += ".{TBD} -> ";
            } else {
                ret += "." + SourceFields[0].Name + " -> ";
            }
            ret += ReferencesTableName + ".";
            if (ReferencesFields == null || ReferencesFields.Count == 0){
                ret += "{TBD}";
            } else {
                ret += ReferencesFields[0].Name;
            }
            return ret;
		}

        private bool mapSourceFields(DataSet ds, DataRow dr) {
            DataRow[] drSourceFields = ds.Tables["ConstraintSourceFieldInfo"].Select("parent_id = " + dr["id"]);
            if (drSourceFields != null && drSourceFields.Length > 0) {
                foreach (DataRow drSrc in drSourceFields) {
                    SourceFieldNames.Add(drSrc["field_name"].ToString().ToLower());
                    foreach (FieldInfo fi in this.Table.Fields) {
                        if (fi.Name.ToUpper().Trim() == drSrc["field_name"].ToString().ToUpper().Trim()) {
                            SourceFields.Add(fi);
                            //fi.IsForeignKey = true;
                            //fi.ForeignKeyTableName = ReferencesTableName;
                            //fi.ForeignKeyFieldName = 
                            break;
                        }
                    }
                }
                return SourceFields.Count > 0;
            } else {
                throw new InvalidOperationException(getDisplayMember("mapSourceFields", "Constraint {0} is defined with parent_id={1} but no fields were found in ConstraintSourceFieldInfo for it.", dr["constraint_name"].ToString(), dr["id"].ToString()));
            }
        }

        private bool mapSourceFields(DataSet ds, TableInfo target) {

            // rescan dataset for constraint info
            DataRow[] drSourceFields = ds.Tables["ConstraintSourceFieldInfo"].Select("constraint_name = '" + target.TableName + "'");
            if (drSourceFields != null && drSourceFields.Length > 0) {
                foreach (DataRow drSrc in drSourceFields) {
                    SourceFieldNames.Add(drSrc["field_name"].ToString().ToLower());
                    foreach (FieldInfo fi in target.Fields) {
                        if (fi.Name.ToUpper().Trim() == drSrc["field_name"].ToString().ToUpper().Trim()) {
                            SourceFields.Add(fi);
                            //fi.IsForeignKey = true;
                            //fi.ForeignKeyTableName = ReferencesTableName;
                            //fi.ForeignKeyFieldName = 
                            break;
                        }
                    }
                }
            }
            return SourceFields.Count > 0;
        }


        // these variables are used when we need to auto-populate the ReferencesTable property in ConstraintInfo.
        // they simply remember which filling method was used to 
        private List<TableInfo> _tableList;
        private DataSet _dsToFillFrom;
        private DataRow _drToFillFrom;

		protected override void fillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables) {
			TableName = dr["table_name"].ToString().ToLower();
			ConstraintName = dr["constraint_name"].ToString().ToLower();
			ReferencesTableName = dr["references_table_name"].ToString().ToLower();
            _tableList = tables;
            _dsToFillFrom = ds;
            _drToFillFrom = dr;

            if (!mapSourceFields(ds, dr)) {
                // source fields not found yet. means the parent of this constraint is not mapped yet.
                Debug.Write("Constraint " + ConstraintName + " from " + TableName + " to " + ReferencesTableName + " has no source fields as " + ReferencesTableName + " has not been resolved yet.");
                Debug.WriteLine("");
            }

			// make sure the reference table is mapped
			TableInfo target = null;
			if (ReferencesTableName.ToUpper().Trim() == this.TableName.ToUpper().Trim()) {
				// self-referential
				target = this.Table;
			} else {
				foreach (TableInfo ti in tables) {
					if (ti.TableName.ToUpper().Trim() == ReferencesTableName.ToUpper().Trim()) {
						// table has already been inspected
						target = ti;
						break;
					}
				}
			}

			if (!tables.Contains(this.Table)) {
				tables.Add(this.Table);
			}

            this.SchemaName = this.Table.SchemaName;

			if (target == null) {
				// the table we are referencing hasn't been inspected yet.
				// do that now.
				DataRow[] drT = ds.Tables["TableInfo"].Select("table_name = '" + ReferencesTableName.ToLower() + "'");
				if (drT == null || drT.Length == 0){
					throw new InvalidOperationException(getDisplayMember("fillFromDataSet", "Could not find row in TableInfo table for table name={0}", ReferencesTableName.ToLower()));
				} else {
					target = TableInfo.GetInstance(this.DataConnectionSpec);
					target.FillFromDataSet(ds, drT[0], tables);

                    if (!tables.Exists(te => {
						return target.TableName.ToUpper().Trim() == te.TableName.ToUpper().Trim();
					})) {
						tables.Add(target);
					}
                }
			}

			ReferencesTable = target;


			DataRow[] drRefFields = ds.Tables["ConstraintReferencesFieldInfo"].Select("parent_id = " + dr["id"]);
			foreach (DataRow drRef in drRefFields) {
				DataRow[] drReferenceTableField = ds.Tables["TableFieldInfo"].Select("table_name = '" + ReferencesTableName.ToLower() + "' and field_name = '" + drRef["field_name"].ToString().ToLower() + "'");
				if (drReferenceTableField == null || drReferenceTableField.Length == 0) {
                    throw new InvalidOperationException(getDisplayMember("fillFromDataSet{reffield}", "Could not find field '{0}' in table '{1}' that constraint '{2}' in table '{3}' references.", drRef["field_name"].ToString().ToLower(), drRef["table_name"].ToString().ToLower(), this.ConstraintName.ToLower(), this.TableName.ToLower()));
				}
				FieldInfo fi = target.Fields.Find(f => {
					return f.Name.ToUpper().Trim() == drRef["field_name"].ToString().ToUpper().Trim();
				});
				if (fi == null) {
					throw new InvalidOperationException(getDisplayMember("fillFromDataSet{reffieldmissing}", "Could not find field named {0} in table {1}", drRef["field_name"].ToString().ToLower(), target.TableName.ToLower()));
				} else {
					SourceFields[ReferencesFields.Count].IsForeignKey = true;
					SourceFields[ReferencesFields.Count].ForeignKeyTable = fi.Table;
					SourceFields[ReferencesFields.Count].ForeignKeyTableName = fi.TableName.ToLower();
					SourceFields[ReferencesFields.Count].ForeignKeyFieldName = fi.Name.ToLower();
					ReferencesFields.Add(fi);
                    ReferencesFieldNames.Add(fi.Name.ToLower());
				}
			}
		}


		public void FillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables) {
			fillFromDataSet(ds, dr, tables);
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


		public ConstraintInfo Clone(TableInfo newParent) {
			ConstraintInfo ci = ConstraintInfo.GetInstance(newParent);
			ci.ConstraintName = ConstraintName;
			ci.DataConnectionSpec = DataConnectionSpec;
			ci.OnDeleteAction = this.OnDeleteAction;
			ci.OnUpdateAction = this.OnUpdateAction;

			// TODO: is this ok? we're not cloning the references table...
			if (ci.ReferencesTable != ci.Table) {
				// self-referential.  no need to clone it.
				Debug.WriteLine("NOT cloning ci.ReferencesTable as it is self-referential");
			} else {
				// pointing at a different table, clone it
				ci.ReferencesTable = this.ReferencesTable.Clone(true, true, true);
			}

			ci.ReferencesTableName = this.ReferencesTableName;
			ci.SyncAction = SyncAction;
			ci.Table = newParent;
			ci.TableName = newParent.TableName;
			foreach(FieldInfo fiSrc in SourceFields){
				ci.SourceFields.Add(fiSrc.Clone(newParent));
			}
            ci.SourceFieldNames = SourceFieldNames.ToList();

			foreach (FieldInfo fiRef in ReferencesFields) {
				ci.ReferencesFields.Add(fiRef.Clone(newParent));
			}
            ci.ReferencesFieldNames = ReferencesFieldNames.ToList();

			return ci;
		}


		public DataRow FillRow(int parentID, DataRow dr) {
			dr["id"] = dr.Table.Rows.Count;
			dr["parent_id"] = parentID;
			dr["table_name"] = TableName.ToLower();
			dr["constraint_name"] = ConstraintName.ToLower();
			dr["references_table_name"] = ReferencesTableName.ToLower();
			dr["on_delete_action"] = OnDeleteAction;
			dr["on_update_action"] = OnUpdateAction;

			return dr;
		}
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "ConstraintInfo", resourceName, null, defaultValue, substitutes);
        }


	}
}
