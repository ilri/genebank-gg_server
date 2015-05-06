using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using GrinGlobal.Core;
using GrinGlobal.DatabaseInspector.MySql;
using GrinGlobal.DatabaseInspector.SqlServer;
using GrinGlobal.DatabaseInspector.Oracle;
using System.IO;
//using GrinGlobal.Sql;
//using GrinGlobal.Business; 
using GrinGlobal.DatabaseInspector.PostgreSql;
using System.Diagnostics;

namespace GrinGlobal.DatabaseInspector {
	public abstract class TableInfo : BaseInfo {

		public event ProgressEventHandler OnProgress;

		private const string DBNULL_VAL = "{DBNull.Value}";

		public static TableInfo CreateDiff(TableInfo source, TableInfo target) {
			if (source == null) {
				throw new ArgumentNullException("source");
			} else {
				// either create or alter...
				return source.CreateDiff(target);
			}
		}

		public static TableInfo GetInstance(DataConnectionSpec dcs) {

			if (dcs == null) {
				return new MySqlTableInfo(null);
			}

			DataVendor vendor = DataManager.GetVendor(dcs);
			switch (vendor) {
				case DataVendor.MySql:
					return new MySqlTableInfo(dcs);
				case DataVendor.SqlServer:
					return new SqlServerTableInfo(dcs);
				case DataVendor.Oracle:
					return new OracleTableInfo(dcs);
                case DataVendor.PostgreSql:
                    return new PostgreSqlTableInfo(dcs);
			}

			throw new NotImplementedException(getDisplayMember("GetInstance", "TableInfo class for {0} is not mapped through TableInfo.GetInstance().", vendor.ToString()));

		}



		/// <summary>
		/// Assumes given object is the slave and the current object is the source. Creates a new TableInfo object representing the source's properties overriding the target's.
		/// </summary>
		/// <param name="slave"></param>
		/// <returns></returns>
		public TableInfo CreateDiff(TableInfo target) {

			// to create a table:
			// src.SyncAction = Create

			// to Drop a table:
			// src.SyncAction = Drop

			// to drop an index:
			// src.SyncAction = Nothing,
			// src.Indexes[blah].SyncAction = Drop

			// to drop a constraint:
			// src.SyncAction = Nothing,
			// src.Constraints[blah].SyncAction = Drop


			// to create a field on an existing table:
			// src.SyncAction = Alter
			// src.Fields[blah].SyncAction = Create

			// to modify a field on an existing table:
			// src.SyncAction = Alter
			// src.Fields[blah].SyncAction = Alter

			// to drop a field on an existing table:
			// src.SyncAction = Alter
			// src.Fields[blah].SyncAction = Drop


			TableInfo diff = null;

			if (target == null) {
				// target doesn't exist. if master says to alter or create, create it.

				// if master says to drop or nothing, ignore it.
				if (SyncAction == SyncAction.Alter || SyncAction == SyncAction.Create) {
					diff = this.Clone(true, true, true);
					diff.SyncAction = SyncAction.Create;

					// fields
					foreach (FieldInfo fi in diff.Fields) {
						if (fi.SyncAction == SyncAction.Create || fi.SyncAction == SyncAction.Alter) {
							fi.SyncAction = SyncAction.Create;
						} else {
							fi.SyncAction = SyncAction.Nothing;
						}
					}

					// indexes
					foreach (IndexInfo ii in diff.Indexes) {
						if (ii.SyncAction == SyncAction.Create || ii.SyncAction == SyncAction.Alter) {
							ii.SyncAction = SyncAction.Create;
						} else {
							ii.SyncAction = SyncAction.Nothing;
						}
					}

					//constraints
					foreach (ConstraintInfo ci in diff.Constraints) {
						if (ci.SyncAction == SyncAction.Create || ci.SyncAction == SyncAction.Alter) {
							ci.SyncAction = SyncAction.Create;
						} else {
							ci.SyncAction = SyncAction.Nothing;
						}
					}


				} else {
					// if it's drop or nothing, do nothing (as it doesn't exist)
					diff = this.Clone(false, false, false);
					diff.SyncAction = SyncAction.Nothing;

				}

			} else {
				// target exists, perform a diff
				diff = this.Clone(true, true, true);
				switch (diff.SyncAction) {
					case SyncAction.Drop:
						// target exists and action is drop.
						// mark as drop for everything (constraints, indexes, table)
						diff.SyncAction = SyncAction.Drop;

						// indexes
						foreach (IndexInfo ii in diff.Indexes) {
							ii.SyncAction = SyncAction.Drop;
						}

						// constraints
						foreach (ConstraintInfo ci in Constraints) {
							ci.SyncAction = SyncAction.Drop;
						}

						break;
					case SyncAction.Nothing:
						// target exists and action is nothing.
						// 
						break;
					case SyncAction.Alter:
					case SyncAction.Create:
						// target exists, so we can't create it.
						// mark it as alter.
						diff.SyncAction = SyncAction.Alter;
						foreach (FieldInfo fiSrc in Fields) {
							// we always add the field.
							diff.Fields.Add(fiSrc);

							FieldInfo fiTgt = target.Fields.Find(f => {
								return f.Name == fiSrc.Name;
							});

							// the corresponding target field (if any) tells us the action to do against it

							if (fiTgt != null) {
								// target contains this field already.
								if (fiSrc != fiTgt) {
									// target's is different. mark it as alter
									fiSrc.SyncAction = SyncAction.Alter;
								} else {
									// field is exactly the same.
									// do nothing. (don't even add it to the Fields collection)
									fiSrc.SyncAction = SyncAction.Nothing;
								}
							} else {
								// tgt doesn't have it.
								if (fiSrc.SyncAction == SyncAction.Alter || fiSrc.SyncAction == SyncAction.Create) {
									// alter or create should show up as create (can't alter a non-existant field)
									fiSrc.SyncAction = SyncAction.Create;
								} else {
									// it's either Drop or Nothing.
									// either way we don't touch it, it already has the right SyncAction
								}
							}
						}

						// indexes
						foreach (IndexInfo ii in diff.Indexes) {
							IndexInfo iiTgt = target.Indexes.Find(i => {
								return i.IndexName == ii.IndexName;
							});
							if (iiTgt != null) {
								// index exists on target.
								switch (ii.SyncAction) {
									case SyncAction.Alter:
									case SyncAction.Create:
										iiTgt.SyncAction = SyncAction.Alter;
										break;
									case SyncAction.Drop:
										iiTgt.SyncAction = SyncAction.Drop;
										break;
									case SyncAction.Nothing:
										iiTgt.SyncAction = SyncAction.Nothing;
										break;
								}
							}
						}

						// constraints
						foreach (ConstraintInfo ci in Constraints) {
							ConstraintInfo ciNew = ci.Clone(diff);
							diff.Constraints.Add(ciNew);
							ConstraintInfo ciTgt = target.Constraints.Find(ciTemp => {
								return ciTemp.ConstraintName == ciNew.ConstraintName;
							});
							if (ciTgt != null) {
								// index exists on target.
								switch (ci.SyncAction) {
									case SyncAction.Alter:
									case SyncAction.Create:
										ciTgt.SyncAction = SyncAction.Alter;
										break;
									case SyncAction.Drop:
										ciTgt.SyncAction = SyncAction.Drop;
										break;
									case SyncAction.Nothing:
										ciTgt.SyncAction = SyncAction.Nothing;
										break;
								}
							}
						}

						break;
				}

			}

			return diff;

		}

		protected void showProgress(string message, int itemOffset, int itemCount) {
			ProgressEventArgs pea = new ProgressEventArgs(message, itemOffset, itemCount);
			if (OnProgress != null) {
				OnProgress(this, pea);
			}
		}

		public DataManager DataManager {
			set {
				DataConnectionSpec.Parent = value;
			}
		}

		public int RowCount { get; set; }

		public bool UseCaseSensitiveCollation { get; set; }

		public string Collation { get; set; }
		public string CharacterSet { get; set; }
		public string Engine { get; set; }
		public string PrimaryKeySortType { get; set; }

		public bool IsSelected { get; set; }

		public List<ConstraintInfo> Constraints { get; set; }
		public List<IndexInfo> Indexes { get; set; }
		public List<FieldInfo> Fields { get; set; }

		public FieldInfo AppendField(string fieldName, bool isAutoIncrement, DbType dbType, int? maxLength, bool isNullable){
			FieldInfo fi = FieldInfo.GetInstance(this);
			fi.Name = fieldName;
			fi.IsAutoIncrement = isAutoIncrement;
			fi.DbType = dbType;
			fi.MaxLength = maxLength == null ? 0 : (int)maxLength;
			fi.IsNullable = isNullable;
			Fields.Add(fi);
			return fi;
		}

		public FieldInfo PrimaryKey {
			get {
				foreach (FieldInfo fi in Fields) {
					if (fi.IsPrimaryKey) {
						return fi;
					}
				}
				return null;
			}
		}


		protected virtual string generateInsertSql(bool includeAutoIncrement) {
			StringBuilder sb = new StringBuilder();
			StringBuilder sbFields = new StringBuilder();
			StringBuilder sbPrms = new StringBuilder();

			foreach (FieldInfo fi in Fields) {
				if (includeAutoIncrement || !fi.IsAutoIncrement) {
					sbFields.Append(fi.Name).Append(", ");
					sbPrms.Append(":").Append(fi.Name).Append(", ");
				}
			}
			sbFields.Remove(sbFields.Length - 2, 2);
			sbPrms.Remove(sbPrms.Length - 2, 2);

			sb.Append("insert into ").Append(TableName).Append(" (");
			sb.Append(sbFields.ToString());
			sb.Append(") values (");
			sb.Append(sbPrms.ToString());
			sb.Append(")");

			return sb.ToString();
		}


		private const int MAX_ROW_THRESHHOLD = 250000;

		protected void SaveAndClearPrimaryKeys(DataManager dm) {
			int count = 0;
			using (DataCommand cmdPkInsert = new DataCommand("insert into importkeymap (SourceTableField, SourceID, DestinationID) values (@stf, @srcid, @destid)",
                        new DataParameters("@stf", null, DbType.Int32, "@srcid", null, DbType.Int32, "@destid", null, DbType.Int32))) {
				foreach (string key in _primaryKeyValues.Keys) {
					string[] keyparts = key.Split('=');
					cmdPkInsert.DataParameters[0].Value = keyparts[0];
					cmdPkInsert.DataParameters[1].Value = keyparts[1];
					cmdPkInsert.DataParameters[2].Value = _primaryKeyValues[key].ToString();
					dm.Write(cmdPkInsert);
					//dm.Write("insert into importkeymap (SourceTableField, SourceID, DestinationID) values (@stf, @srcid, @destid)",
					//    new DataParameters("@stf", keyparts[0], "@srcid", keyparts[1], "@destid", _primaryKeyValues[key].ToString()));
					count++;
					if (count % 1000 == 0) {
						showProgress("PERCENT:" + getDisplayMember("SaveAndClearPrimaryKeys{progress}", "{0} complete", (((decimal)count / (decimal)_primaryKeyValues.Keys.Count)).ToString("##0.0%")), count, _primaryKeyValues.Keys.Count);
					}
				}
			}
			_primaryKeyValues.Clear();
			_primaryKeyValues = new Dictionary<string, object>();
		}

		protected void SavePrimaryKeyValue(string keyname, string sourceValue, string destinationValue, DataManager dm){

			string fullKeyName = TableName + "." + keyname + "=" + sourceValue;
			_primaryKeyValues[fullKeyName] = destinationValue;


			// RAM is getting full, flush to table and clear the dictionary
			if (_primaryKeyValues.Count > MAX_ROW_THRESHHOLD) {
				SaveAndClearPrimaryKeys(dm);
			}

			//} else {
			//    // write to db, this table is large
			//    dm.Write("insert into importkeymap (SourceTableField, SourceID, DestinationID) values (@stf, @srcid, @destid)", new DataParameters("@stf", TableName + "." + keyname, "@srcid", sourceValue, "@destid", destinationValue));
			//}
		}

		protected object LookupPrimaryKeyValue(string keyname, string sourceValue, DataManager dm) {
			object ret = null;
			if (!_primaryKeyValues.TryGetValue(TableName + "." + keyname + "=" + sourceValue, out ret)){
				// pull from DB
				ret = dm.ReadValue("select DestinationID from importkeymap where sourcetablefield = @stf and sourceid = @srcid", new DataParameters("@stf", TableName + "." + keyname, "@srcid", sourceValue));
				if (ret != null) {
					// add it back into the cache in case we need to look it up soon
					SavePrimaryKeyValue(keyname, sourceValue, ret.ToString(), dm);
				} else {
					// could not find it. assume it is already the correct value.
					// (i.e. assume this is pointing at a non-autoincrement field, like sys_lang.iso_639_3_code)
					ret = sourceValue;
				}
			}
			return ret;
		}

		private Dictionary<string, object> _primaryKeyValues;

		public override string ToString() {
			return TableName + "; Fields=" + Fields.Count + "; Indexes=" + Indexes.Count + "; Constraints=" + Constraints.Count;
		}

		/// <summary>
		/// Adds a TableInfo object for the given tableName to the given List if it does not already exist in the list
		/// </summary>
        /// <param name="schemaName"></param>
		/// <param name="tableName"></param>
		/// <param name="tables"></param>
        /// <param name="fillRowCount"></param>
        /// <param name="maxRecurseDepth">Maximum depth of FK references to follow.  To load just the specified table, set to 0.  To fully load all dependencies, set to int.MaxValue.  Circular dependencies are handled properly.</param>
		public void Fill(string schemaName, string tableName, List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
			TableName = tableName;
			SchemaName = schemaName;
			fillFromDatabase(tables, fillRowCount, maxRecurseDepth);
		}


		public TableInfo(DataConnectionSpec dcs)
			: base(dcs) {
			Fields = new List<FieldInfo>();
			Indexes = new List<IndexInfo>();
			Constraints = new List<ConstraintInfo>();
			_primaryKeyValues = new Dictionary<string, object>();
			SyncAction = SyncAction.Create;
		}

		protected abstract void makeFieldNullable(FieldInfo fi, bool nullable);

		protected override void fillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables) {

			if (tables.Exists(t => {
				return t.TableName.ToUpper().Trim() == TableName.ToUpper().Trim();
			})) {
				// this table has already been added.
				return;
			}

			// pull info from datarow
			PrimaryKeySortType = dr["primary_key_sort_type"].ToString();
			CharacterSet = dr["charset"].ToString();
			Collation = dr["collation"].ToString();
			Engine = dr["engine"].ToString();
			IsSelected = Toolkit.ToBoolean(dr["is_selected"], false);
			RowCount = Toolkit.ToInt32(dr["row_count"], 0);

			// look up all fields associated with this table
			DataTable dtTableField = ds.Tables["TableFieldInfo"];
			DataRow[] drTableFields = dtTableField.Select("parent_id = " + dr["id"]);
			foreach (DataRow dr2 in drTableFields) {
				Fields.Add(FieldInfo.GetInstance(this).FillFromDataSet(dr2, null, tables));
			}

			// look up all indexes associated with this table
			var drIndexes = ds.Tables["IndexInfo"].Select("parent_id = " + dr["id"]);

			IndexInfo idx = null;
			foreach (DataRow dr3 in drIndexes) {
				if (idx == null || idx.IndexName.ToLower() != dr3["index_name"].ToString().ToLower()) {
					idx = IndexInfo.GetInstance(this);
                    idx.FillFromDataSet(ds, dr3);
					this.Indexes.Add(idx);
				}
			}


			DataRow[] drConstraints = ds.Tables["ConstraintInfo"].Select("parent_id = " + dr["id"]);
			ConstraintInfo ci = null;
			foreach (DataRow dr4 in drConstraints) {
				if (ci == null || ci.ConstraintName.ToLower() != dr4["constraint_name"].ToString().ToLower()) {
					ci = ConstraintInfo.GetInstance(this);
					ci.FillFromDataSet(ds, dr4, tables);
					Constraints.Add(ci);
				}
			}




		}

		public TableInfo Clone(bool includeFields, bool includeIndexes, bool includeConstraints) {
			return clone(false, includeFields, includeIndexes, includeConstraints);
		}

		protected TableInfo CloneAsErrorTable() {
			return clone(true, true, true, true);
		}

		private TableInfo clone(bool asError, bool includeFields, bool includeIndexes, bool includeConstraints) {
			TableInfo ti = TableInfo.GetInstance(DataConnectionSpec);
			ti.CharacterSet = this.CharacterSet;
			ti.Collation = this.Collation;
			ti.Engine = this.Engine;
			ti.PrimaryKeySortType = this.PrimaryKeySortType;
			ti.UseCaseSensitiveCollation = this.UseCaseSensitiveCollation;
			ti.SyncAction = this.SyncAction;

			if (asError) {
				FieldInfo fiErr = FieldInfo.GetInstance(ti);
				fiErr.DataType = typeof(string);
				fiErr.DbType = DbType.String;
				fiErr.Name = "err_message";
				fiErr.MaxLength = 300;
				fiErr.IsNullable = true;
				ti.Fields.Add(fiErr);

				ti.IsSelected = false;
				ti.TableName = this.TableName + "__importerror";
				ti.RowCount = 0;

				if (includeFields) {
					foreach (FieldInfo fi in Fields) {
						// for error table, we assume no auto-increments
						FieldInfo fiClone = fi.Clone(ti);
						fiClone.IsAutoIncrement = false;
						fiClone.IsNullable = true;
						ti.Fields.Add(fiClone);
					}
				}
			} else {
				ti.IsSelected = this.IsSelected;
				ti.TableName = this.TableName;
				ti.RowCount = this.RowCount;

				if (includeFields) {
					foreach (FieldInfo fi in Fields) {
						ti.Fields.Add(fi.Clone(ti));
					}
				}
			}

			if (includeIndexes) {
				foreach (IndexInfo ii in Indexes) {
					ti.Indexes.Add(ii.Clone(ti));
				}
			}

			if (includeConstraints) {
				foreach (ConstraintInfo ci in Constraints) {
					ti.Constraints.Add(ci.Clone(ti));
				}
			}


			return ti;
		}

		public void FillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables) {
			TableName = dr["table_name"].ToString().ToLower();
			fillFromDataSet(ds, dr, tables);
		}

		public string GenerateInsertSelectScript() {
			return generateInsertSelectSql();
		}

		public string GenerateIndexSelectScript() {
			return generateIndexSelectSql();
		}

		public string GenerateCreateMapScript() {
			return generateMapCreateSql();
		}

		public string GenerateCreateScript() {
			return generateCreateSql();
		}

		public string GenerateDropScript() {
			return generateDropSql();
		}

		public void RunAlterScript() {
			foreach (FieldInfo fi in Fields) {
				switch (fi.SyncAction) {
					case SyncAction.Nothing:
						break;
					case SyncAction.Alter:
						break;
					case SyncAction.Create:
						break;
					case SyncAction.Drop:
						break;
				}
			}
		}

        /// <summary>
        /// Generates and runs script based on SyncAction property
        /// </summary>
		public virtual void RunScript() {
			// based on SyncAction, runs the appropriate script(s)
			switch (SyncAction) {
				case SyncAction.Alter:

					RunAlterScript();

					foreach (IndexInfo ii in Indexes) {
						ii.RunScript();
					}
					foreach (ConstraintInfo ci in Constraints) {
						ci.RunScript();
					}
					break;

				case SyncAction.Drop:
					// forces a drop of all constraints and indexes as well.
					foreach (ConstraintInfo ci in Constraints) {
						ci.RunDropScript(true);
					}
					foreach (IndexInfo ii in Indexes) {
						ii.RunDropScript(true);
					}
					RunDropScript();
					break;

				case SyncAction.Create:
					// forces a create of all constraints and indexes as well.
					RunCreateScript();
					foreach (IndexInfo ii in Indexes) {
						ii.RunCreateScript();
					}
					foreach (ConstraintInfo ci in Constraints) {
						ci.RunCreateScript();
					}
					break;

				case SyncAction.Nothing:
					// inspect the indexes and constraints to see what needs to be done.
					foreach (IndexInfo ii in Indexes) {
						ii.RunScript();
					}
					foreach (ConstraintInfo ci in Constraints) {
						ci.RunScript();
					}
					break;
			}
		}

		protected virtual void beginLoading(DataManager dm, DataCommand cmd) {
		}

		protected virtual void endLoadingSuccess(DataManager dm) {
		}

		protected virtual void endLoadingFailure(DataManager dm) {
		}

        /// <summary>
        /// Runs given sql against the database.
        /// </summary>
        /// <param name="sql"></param>
        public virtual void RunScript(string sql) {
            try {
                //var txt = Toolkit.Cut("conn=" + DataConnectionSpec + "; SQL=" + sql, 0, 800);
                //EventLog.WriteEntry("GRIN-Global Database", txt);
                using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                    dm.Write(sql);
                }
            } catch (Exception ex) {
                throw new InvalidOperationException(getDisplayMember("RunScript", "Error running sql: {0}. {1}", sql, ex.Message), ex);
            }
        }
		public virtual void RunCreateScript() {
			string tableSql = generateCreateSql();
            RunScript(tableSql);
		}

		public virtual void RunDropScript() {
			string sql = generateDropSql();
            RunScript(sql);
		}



		protected virtual string bulkLoad(string inputDirectory, List<TableInfo> allTables, bool dryRun) {
            return null;
		}

		public string CopyDataFromFiles(string inputDirectory, List<TableInfo> allTables, bool dryRun, bool onlyRequiredData) {

			using (HighPrecisionTimer hptImport = new HighPrecisionTimer("import", true)) {
                if (onlyRequiredData) {
                    var tn = this.TableName.ToLower();
                    // %CABEXE% n %GG%\gringlobal.database.setup\system_data.cab %RAWDATA%\sys_*.* + %RAWDATA%\cooperator*.* + %RAWDATA%\app_*.* + %RAWDATA%\web_user.* + %RAWDATA%\web_user_preference.* + %RAWDATA%\web_cooperator.* + %RAWDATA%\site.* + %RAWDATA%\region*.* + %RAWDATA%\geo*.* + %RAWDATA%\code*.* 

                    if (tn.StartsWith("sys_")
                        || tn.StartsWith("cooperator")
                        || tn == "app_resource"
                        || tn == "app_setting"
                        || tn == "web_user"
                        || tn == "web_user_preference"
                        || tn.StartsWith("web_cooperator")
                        || tn == "site"
                        || tn.StartsWith("region")
                        || tn.StartsWith("geo")
                        || tn.StartsWith("code")) {
                        // this is a system-required table, ok to include
                    } else {
                        // not required, exclude.
                        return "";
                    }
                }

				var output = bulkLoad(inputDirectory, allTables, dryRun);
				showProgress(getDisplayMember("CopyDataFromFiles{total}", "Total import time for {0}: {1}", TableName, Toolkit.FormatTimeElapsed(((int)hptImport.Stop()) / 1000)), 0, 0);
                return output;
			}

		}


        protected virtual string unescape(string inputData, char fieldTerminatorInInput, string fieldTerminatorToOutput) {
            // example data from file (escaped):
            // \\path\\to\\something\t\NHello\\r\\nmultiline\\r\\ndata\\\t!\r\n
            // should come out as:
            // @"\path\to\something   Hello
            // multiline
            // data\    !"
            //

            // first do the easy ones -- unescape all null, new line characters
            var sb = new StringBuilder();
            var prevWasSlash = false;
            var curIsSlash = false;
            for(var i=0;i<inputData.Length;i++){
                curIsSlash = inputData[i] == '\\';

                if (prevWasSlash){
                    if (curIsSlash) {
                        // found \\
                        // append a literal backslash
                        sb.Append(@"\");
                        // reset flag so we don't continue to unescape things incorrectly
                        curIsSlash = false;
                    } else {
                        // found \r or \n or \t or something of that nature
                        switch(inputData[i]){
                            case 'n':
                                // append newline character
                                sb.Append("\n");
                                break;
                            case 'r':
                                // append carriage return character
                                sb.Append("\r");
                                break;
                            case 't':
                                sb.Append("\t");
                                break;
                            case 'N':
                                // append nothing
                                break;
                        }
                    }
                } else {
                    if (curIsSlash){
                        // found \, but previous character was not a \
                        // don't know what to do yet, just continue

                    } else {
                        // found something other than \ and previous character was not a \
                        if (inputData[i] == fieldTerminatorInInput){
                            // if it matches our field terminator, replace it with the output field terminator
                            sb.Append(fieldTerminatorToOutput);
                        } else {
                            // just append the character, it's not escaped
                            sb.Append(inputData[i]);
                        }
                    }
                }

                prevWasSlash = curIsSlash;
            }
            var output = sb.ToString();
            return output;
        }

		public void CopyDataToDelimitedFile(string outputDirectory, string whereClause) {
			string filename = outputDirectory + "\\" + TableName + ".txt";
			if (File.Exists(filename)) {
				File.Delete(filename);
			}

			string tn = TableName;
//			bool isOracle = this.GetType() == typeof(OracleTableInfo);


			using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write),Encoding.UTF8)) {
				using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
					showProgress(getDisplayMember("CopyDataToDelimitedFile{start}", "Copying data for {0}...", TableName), 0, 0);

//					if (!isOracle) {
						// use a datareader for mysql/sql server
					using (IDataReader idr = dm.Stream("select * from " + TableName + " " + whereClause)) {

						int fieldCount = idr.FieldCount;

						// get each column name
						List<string> cols = new List<string>();
						for (int i=0; i < fieldCount; i++) {
							cols.Add(idr.GetName(i));
						}
						Toolkit.OutputTabbedHeader(cols.ToArray(), sw);

						object[] data = new object[fieldCount];
						int rowProgress = 0;
						while (idr.Read()) {
							idr.GetValues(data);
							for (int i=0; i < data.Length; i++) {
								if (data[i] != null && data[i].GetType() == typeof(DateTime)) {
									data[i] = (Toolkit.ToDateTime(data[i], DateTime.MinValue)).ToString("yyyy-MM-dd HH:mm:ss");
								}
							}
							rowProgress++;
							Toolkit.OutputTabbedData(data, sw);
							if (rowProgress % 10000 == 0 && rowProgress > 0) {
								showProgress(getDisplayMember("CopyDataToDelimitedFile{progress}", "Copied {0} rows of {1} so far...", rowProgress.ToString(), this.TableName), 0, 0);
							}
						}
					}
					//} else {

					//    // datareader in oracle is *EXTREMELY* slow, so use a datatable instead
					//    // note this is slurping an entire table into RAM because datamanager's Limit property
					//    // is not entirely followed in Oracle everytime if there's an order by.  That's something to fix...
					//    DataTable dt = dm.Read("select * from " + tn);
					//    List<string> cols = new List<string>();
					//    foreach(DataColumn dc in dt.Columns){
					//        cols.Add(dc.ColumnName);
					//    }
					//    Toolkit.OutputTabbedHeader(cols.ToArray(), sw);
					//    int rowProgress = 0;
					//    foreach (DataRow dr in dt.Rows) {
					//        object[] values = dr.ItemArray;
					//        rowProgress++;
					//        Toolkit.OutputTabbedData(values, sw);
					//        if (rowProgress % 10000 == 0 && rowProgress > 0) {
					//            showProgress("Copied " + rowProgress + " rows so far...", 0, 0);
					//        }
					//    }
					//}
				}
			}
		}

		public override DataRow FillRow(DataRow dr) {
			dr["id"] = dr.Table.Rows.Count;
			dr["table_name"] = TableName.ToLower();
			dr["engine"] = Engine;
			dr["collation"] = Collation;
			dr["charset"] = CharacterSet;
			dr["primary_key_sort_type"] = PrimaryKeySortType;
			dr["is_selected"] = IsSelected;
			dr["row_count"] = RowCount;
			return dr;
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "TableInfo", resourceName, null, defaultValue, substitutes);
        }

	}
}
