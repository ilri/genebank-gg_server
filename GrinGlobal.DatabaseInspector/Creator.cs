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
using System.Threading;
//using GrinGlobal.Sql;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;
//using GrinGlobal.Business; 

namespace GrinGlobal.DatabaseInspector {

	public delegate void ProgressEventHandler(object sender, ProgressEventArgs pea);

	public delegate void ProcessTableCallback(TableInfo ti);
	public delegate void ProcessIndexCallback(IndexInfo ti);
	public delegate void ProcessConstraintCallback(ConstraintInfo ti);

	public abstract class Creator {

//        private static int updatedByCooperatorID = 1;

		#region Utility
		public event ProgressEventHandler OnProgress;

		public const string INFILE_OPTIONS =
@" FIELDS TERMINATED BY '\t' ENCLOSED BY ''
LINES TERMINATED BY '\r\n'
";


		public const string OUTFILE_OPTIONS = 
@" FIELDS TERMINATED BY '\t' ENCLOSED BY ''
 LINES TERMINATED BY '\r\n' ";


		public static Creator GetInstance(DataConnectionSpec dcs) {
			DataVendor vendor = DataManager.GetVendor(dcs);
			switch (vendor) {
				case DataVendor.MySql:
					return new MySqlCreator(dcs);
				case DataVendor.SqlServer:
					return new SqlServerCreator(dcs);
				case DataVendor.Oracle:
				case DataVendor.ODBC:
					return new OracleCreator(dcs);
                case DataVendor.PostgreSql:
                    return new PostgreSqlCreator(dcs);
			}

            throw new NotImplementedException(getDisplayMember("GetInstance", "Creator class not defined for vendor {0}", vendor.ToString()));

		}

        public BackgroundWorker Worker { get; set; }

		public void ProcessTables(List<TableInfo> tables, ProcessTableCallback callback) {
            var tableCopies = tables.ToList();
			if (callback != null) {
				foreach (TableInfo ti in tableCopies) {
					callback(ti);
				}
			}
		}
		public void ProcessIndexes(List<TableInfo> tables, ProcessIndexCallback callback) {
			if (callback != null) {
				ProcessTables(tables, t => {
					foreach (IndexInfo ii in t.Indexes) {
						callback(ii);
					}
				});
			}
		}
		public void ProcessConstraints(List<TableInfo> tables, ProcessConstraintCallback callback) {
			if (callback != null) {
				ProcessTables(tables, t => {
					foreach (ConstraintInfo ci in t.Constraints) {
						callback(ci);
					}
				});
			}
		}

		public virtual string CommandSeparator {
			get {
				return ";";
			}
		}

		public List<TableInfo> SortByName(List<TableInfo> tables) {
			tables.Sort(new TableInfoComparer());
			return tables;
		}

		public List<TableInfo> SortByDependencies(List<TableInfo> tables) {
			return TableInfoComparer.SortByDependencies(tables);
		}


		protected DataSet fillDataSet(List<TableInfo> tables) {
			// get the dataset we'll be saving schema info into
			DataSet ds = createSchemaDataSet();
			DataTable dtTable = ds.Tables["TableInfo"];
			DataTable dtTableField = ds.Tables["TableFieldInfo"];
			DataTable dtIndex = ds.Tables["IndexInfo"];
			DataTable dtIndexField = ds.Tables["IndexFieldInfo"];
			DataTable dtConstraint = ds.Tables["ConstraintInfo"];
			DataTable dtConstraintSourceField = ds.Tables["ConstraintSourceFieldInfo"];
			DataTable dtConstraintReferencesField = ds.Tables["ConstraintReferencesFieldInfo"];

			for (int i=0; i < tables.Count; i++) {
				TableInfo ti = tables[i];

				DataRow drTable = ti.FillRow(dtTable.NewRow());
				dtTable.Rows.Add(drTable);
				int tableID = Toolkit.ToInt32(drTable["id"], -1);

				foreach (FieldInfo fi in ti.Fields) {
					// fill each field row
					dtTableField.Rows.Add(fi.FillDataSetRow(tableID, ti.TableName, dtTableField.NewRow()));
				}

				// fill index information
				foreach (IndexInfo ii in ti.Indexes) {
					DataRow drIndex = ii.FillRow(tableID, dtIndex.NewRow());
					dtIndex.Rows.Add(drIndex);
					int indexID = Toolkit.ToInt32(drIndex["id"], -1);
					foreach (FieldInfo fi2 in ii.Fields) {
						dtIndexField.Rows.Add(fi2.FillDataSetRow(indexID, ti.TableName, dtIndexField.NewRow()));
					}
				}

				// fill constraint information
				foreach (ConstraintInfo ci in ti.Constraints) {
					DataRow drConstraint = ci.FillRow(tableID, dtConstraint.NewRow());
					dtConstraint.Rows.Add(drConstraint);
					int constraintID = Toolkit.ToInt32(drConstraint["id"], -1);
					foreach (FieldInfo fi3 in ci.SourceFields) {
						dtConstraintSourceField.Rows.Add(fi3.FillDataSetRow(constraintID, ci.TableName, dtConstraintSourceField.NewRow()));
					}
					foreach (FieldInfo fi4 in ci.ReferencesFields) {
						dtConstraintReferencesField.Rows.Add(fi4.FillDataSetRow(constraintID, ci.TableName, dtConstraintReferencesField.NewRow()));
					}
				}
			}

			return ds;

		}


		private DataTable createFieldDefinitionTable(string tableName) {
			DataTable dtField = new DataTable(tableName);
			dtField.Columns.Add("id", typeof(int));
			dtField.Columns.Add("parent_id", typeof(int));
			dtField.Columns.Add("table_name", typeof(string));
			dtField.Columns.Add("field_name", typeof(string));
			dtField.Columns.Add("field_type", typeof(string));
			dtField.Columns.Add("field_purpose", typeof(string));
			dtField.Columns.Add("charset", typeof(string));
			dtField.Columns.Add("default_value", typeof(string));
			dtField.Columns.Add("is_autoincrement", typeof(bool));
			dtField.Columns.Add("is_nullable", typeof(bool));
			dtField.Columns.Add("is_unsigned", typeof(bool));
			dtField.Columns.Add("is_zerofill", typeof(bool));
			dtField.Columns.Add("minlength", typeof(int));
			dtField.Columns.Add("maxlength", typeof(int));
			dtField.Columns.Add("precision", typeof(int));
			dtField.Columns.Add("scale", typeof(int));
			dtField.Columns.Add("comment", typeof(string));

			return dtField;
		}

		private DataSet createSchemaDataSet() {

			DataSet ds = new DataSet("DatabaseSchema");

			DataTable dtTable = new DataTable("TableInfo");
			dtTable.Columns.Add("id", typeof(int));
			dtTable.Columns.Add("table_name", typeof(string));
			dtTable.Columns.Add("engine", typeof(string));
			dtTable.Columns.Add("charset", typeof(string));
			dtTable.Columns.Add("collation", typeof(string));
			dtTable.Columns.Add("is_selected", typeof(bool));
			dtTable.Columns.Add("primary_key_sort_type", typeof(string));
			dtTable.Columns.Add("row_count", typeof(int));
			ds.Tables.Add(dtTable);

			DataTable dtTableField = createFieldDefinitionTable("TableFieldInfo");
			ds.Tables.Add(dtTableField);
			DataRelation relTableField = new DataRelation("RelTableField", dtTable.Columns["id"], dtTableField.Columns["parent_id"], true);
			ds.Relations.Add(relTableField);

			DataTable dtIndex = new DataTable("IndexInfo");
			dtIndex.Columns.Add("id", typeof(int));
			dtIndex.Columns.Add("parent_id", typeof(int));
			dtIndex.Columns.Add("table_name", typeof(string));
			dtIndex.Columns.Add("index_name", typeof(string));
			dtIndex.Columns.Add("index_kind", typeof(string));
			dtIndex.Columns.Add("index_type", typeof(string));
			ds.Tables.Add(dtIndex);
			DataRelation relTableIndex = new DataRelation("RelTableIndex", dtTable.Columns["id"], dtIndex.Columns["parent_id"], true);
			ds.Relations.Add(relTableIndex);


			DataTable dtIndexField = createFieldDefinitionTable("IndexFieldInfo");
			ds.Tables.Add(dtIndexField);
			DataRelation relIndexField = new DataRelation("RelIndexField", dtIndex.Columns["id"], dtIndexField.Columns["parent_id"], true);
			ds.Relations.Add(relIndexField);


			DataTable dtConstraint = new DataTable("ConstraintInfo");
			dtConstraint.Columns.Add("id", typeof(int));
			dtConstraint.Columns.Add("parent_id", typeof(int));
			dtConstraint.Columns.Add("table_name", typeof(string));
			dtConstraint.Columns.Add("constraint_name", typeof(string));
			dtConstraint.Columns.Add("references_table_name", typeof(string));
			dtConstraint.Columns.Add("on_delete_action", typeof(string));
			dtConstraint.Columns.Add("on_update_action", typeof(string));
			ds.Tables.Add(dtConstraint);
			DataRelation relTableConstraint = new DataRelation("RelTableConstraint", dtTable.Columns["id"], dtConstraint.Columns["parent_id"], true);
			ds.Relations.Add(relTableConstraint);


			DataTable dtConstraintSourceField = createFieldDefinitionTable("ConstraintSourceFieldInfo");
			ds.Tables.Add(dtConstraintSourceField);
			DataRelation relConstraintSourceField = new DataRelation("RelConstraintSourceField", dtConstraint.Columns["id"], dtConstraintSourceField.Columns["parent_id"], true);
			ds.Relations.Add(relConstraintSourceField);


			DataTable dtConstraintReferencesField = createFieldDefinitionTable("ConstraintReferencesFieldInfo");
			ds.Tables.Add(dtConstraintReferencesField);
			DataRelation relConstraintReferencesField = new DataRelation("RelConstraintReferencesField", dtConstraint.Columns["id"], dtConstraintReferencesField.Columns["parent_id"], true);
			ds.Relations.Add(relConstraintReferencesField);


			return ds;

		}

		private TableInfo getTableInfo(string schemaName, string tableName, List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {
			TableInfo ti = tables.Find(t => {
				return t.TableName.ToUpper().Trim() == tableName.ToUpper().Trim();
			});
			if (ti == null) {
				ti = TableInfo.GetInstance(DataConnectionSpec);
				ti.Fill(schemaName, tableName, tables, fillRowCount, maxRecurseDepth);
			}
			return ti;
		}

		/// <summary>
		/// Retrieves the list of table names in the database.  Can optionally override by passing the schemaName.
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		public virtual List<string> ListTableNames(string schemaName) {
			throw new NotImplementedException();
		}

		public DataConnectionSpec DataConnectionSpec { get; set; }

		protected Creator(DataConnectionSpec dcs) {
			DataConnectionSpec = dcs;
		}


		public abstract string NormalizeName(string name);

		#endregion Utility

		#region GUI Feedback
		protected void showProgress(string message, int itemOffset, int itemCount) {
			ProgressEventArgs pea = new ProgressEventArgs(message, itemOffset, itemCount);
            pea.Worker = this.Worker;
			if (OnProgress != null) {
				OnProgress(this, pea);
			}
		}
		void ti_OnProgress(object sender, ProgressEventArgs pea) {
			showProgress(pea.Message, pea.ItemOffset, pea.TotalItems);
		}



		#endregion GUI Feedback

		#region Load from datasource
		/// <summary>
		/// Reads the table schema from the database, optionally pulling a rowcount as well
		/// </summary>
		/// <param name="schemaName"></param>
		/// <param name="tableNames"></param>
		/// <param name="fillRowCount"></param>
		/// <returns></returns>
		public List<TableInfo> LoadTableInfo(string schemaName, List<string> tableNames, bool fillRowCount, int maxRecurseCount){
			List<TableInfo> tables = new List<TableInfo>();

			if (tableNames == null || tableNames.Count == 0) {
				tableNames = ListTableNames(schemaName);
			}


			for (int i=0; i < tableNames.Count; i++) {
				string tn = tableNames[i];
				showProgress(getDisplayMember("LoadTableInfo{parsing}", "Parsing schema for table {0} ( {1} of {2} )", tn, (i + 1).ToString(), tableNames.Count.ToString()), i, tableNames.Count);
				TableInfo ti = getTableInfo(schemaName, tn, tables, fillRowCount, maxRecurseCount);
				// mark it as selected since they explicitly asked for it
				// (this is used to determine if we need to copy data or schema for table items later)
				ti.IsSelected = true;
			}

            // spin through again and backfill constraint references we missed the first time (ones that points to tables which did not exist yet)
            //foreach (var ti in tables) {
            //    foreach (var ci in ti.Constraints) {
            //        if (ci.SourceFields.Count == 0) {
            //            foreach (var tiRef in tables) {
            //                if (tiRef.TableName == ci.ReferencesTableName) {

            //                }
            //            }
            //            Debug.WriteLine("need to fill constraint info for " + ti.TableName + " pointing to " + ci.ReferencesTableName);
            //        }
            //    }
            //}

			tables.Sort(new TableInfoComparer());
			return tables;
		}

		/// <summary>
		/// Reads table schema info from given CSV file.  If newTables is true, reads the definitions for the new schema.  Otherwise reads definition for the old schema.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="newTables"></param>
		/// <returns></returns>
		public List<TableInfo> LoadTableInfo(string fileName, bool newTables) {
			List<TableInfo> tables = new List<TableInfo>();

			string previousTableName = "__ yeah this will never match, which is good";

			TableInfo ti = null;

			Dictionary<string, IndexInfo> indexes = new Dictionary<string, IndexInfo>();
			IndexInfo ii = null;

			//int tableOffset = (newTables ? 3 : 0);
			//int indexCount = 10;

			List<string> colNames = new List<string>();
			int indexStartColumn = -1;
			int indexCount = -1;
			int fkNameColumn = -1;

			foreach (List<string> cols in Toolkit.ParseCSV(fileName, true)) {

				// Format:
				// current table name
				// current field name
				// current field type
				// new table name
				// new field name
				// new field type
				// comments
				// max length
				// foreign key name
				// foreign key table
				// foreign key field
				// index 1
				// index 2
				// ..
				// index 10

				if (colNames.Count == 0) {
					// first slurp in all the column headers
					colNames = cols;

					for (int j=0; j < colNames.Count; j++) {
						if (colNames[j].ToLower().Contains("index ")) {
							if (indexStartColumn < 0) {
								indexStartColumn = j;
							}
							indexCount++;
						}
					}

					continue;
				}


				string oldTableName = null;
				string tableName = null;  // cols[tableOffset];
				string newTableName = null;

				string fieldName = null; // cols[tableOffset + 1].ToLower();
				string fieldType = null; // cols[tableOffset + 2].ToLower();
				string comment = null;
				bool isAutoIncrement = false;
				bool isNullable = false;
				string engineName = "InnoDB";
				int maxLength = 0;
				string fkName = null;
				string fkTable = null;
				string fkField = null;


				for (int i=0; i < cols.Count; i++) {
					if (i >= colNames.Count) {
						// if they added stuff outside the headers, ignore it
						break;
					}
					string colName = colNames[i].ToLower().Trim();
					string colValue = cols[i];
					switch (colName) {
						case "current table name":
							oldTableName = colValue;
							if (!newTables){
								tableName = colValue;
							}
							break;
						case "current field name":
							if (!newTables) {
								fieldName = colValue;
							}
							break;
						case "current field type":
							if (!newTables) {
								fieldType = colValue;
							}
							break;
						case "new table name":
							newTableName = colValue;
							if (newTables) {
								tableName = colValue;
							}
							break;
						case "new field name":
							if (newTables) {
								fieldName = colValue;
							}
							break;
						case "new field type":
							if (newTables) {
								fieldType = colValue;
							}
							break;
						case "notes":
							comment = colValue.ToUpper();
							isAutoIncrement = comment.Contains("AUTO INCREMENT");
							isNullable = Toolkit.ToBoolean(comment.Contains("NULLABLE"), false);
							if (comment.Contains("MYISAM")) {
								engineName = "MYISAM";
							}
							break;
						case "max length":
							maxLength = Toolkit.ToInt32(colValue, 0);
							break;
						case "foreign key constraint name":
							fkName = colValue;
							fkNameColumn = i;
							break;
						case "foreign key table name":
							fkTable = colValue;
							break;
						case "foreign key field name":
							fkField = colValue;
							break;
						default:
							break;
					}
				}


				if (String.IsNullOrEmpty(tableName) || tableName.Contains("?") || tableName.Contains("<")) {
					// skip, bogus row
					continue;
				}

				tableName = tableName.ToLower();


				if (fieldName.Contains("<")) {
					// skip, bogus row (<deleted> field is a common one here)
					continue;
				}

				if (fkName != null && fkName.Contains("<")) {
					// HACK: ignore names with "<" as they're just comments
					fkName = null;
					fkTable = null;
					fkField = null;
				}

				if (tableName != previousTableName) {

					if (ti != null) {
						foreach (string key in indexes.Keys) {
							ti.Indexes.Add(indexes[key]);
						}
						tables.Add(ti);
					}
					previousTableName = tableName;



					ti = TableInfo.GetInstance(DataConnectionSpec);
					ti.IsSelected = true;
					ti.Engine = engineName;
					indexes = new Dictionary<string, IndexInfo>();
					ti.TableName = tableName;
				}

				FieldInfo fi = FieldInfo.GetInstance(ti);
				fi.Name = fieldName.ToLower();
				fi.TableName = ti.TableName;
				fi.ForeignKeyConstraintName = fkName;
				fi.ForeignKeyTableName = fkTable;
				fi.ForeignKeyFieldName = fkField;
				fi.IsCreatedDate = (fi.Name == "created_date");
				fi.IsCreatedBy = (fi.Name == "created_by");
				fi.IsModifiedDate = (fi.Name == "modified_date");
				fi.IsModifiedBy = (fi.Name == "modified_by");
				fi.IsOwnedDate = (fi.Name == "owned_date");
				fi.IsOwnedBy = (fi.Name == "owned_by");
				fi.IsAutoIncrement = isAutoIncrement;
				fi.IsForeignKey = (fi.ForeignKeyTableName + fi.ForeignKeyFieldName + "").Length > 0;
				fi.IsNullable = isNullable;
				fi.IsPrimaryKey = fi.IsAutoIncrement;
				fi.MaxLength = maxLength;

				switch (fieldType.ToUpper()) {
					case "INTEGER":
						fi.DataType = typeof(int);
						break;
                    case "LONG":
                        fi.DataType = typeof(long);
                        break;
					case "STRING":
						fi.DataType = typeof(string);
						break;
					case "DATETIME":
						fi.DataType = typeof(DateTime);
						break;
					case "DECIMAL":
						fi.DataType = typeof(Decimal);
						fi.Precision = 18;
						fi.Scale = 5;
						break;
					case "":
						// invalid field type, ignore it. (do not add)
						break;
					default:
						throw new NotImplementedException("Field type of " + fieldType + " not mapped in GetTableSchemaFromCSV");
				}

				if (fi.DataType != null) {
					ti.Fields.Add(fi);

					// use this for loop statement to NOT create indexes for each constraint
					// before creating the constraints themselves
//					for (int i=indexStartColumn; i < indexCount + indexStartColumn; i++) {

					// use this for loop statement to create indexes for each constraint
					// (mysql does this automatically, as does most DBMS's I believe)
					for (int i=fkNameColumn; i < indexCount + indexStartColumn && i < cols.Count; i++) {

						string idx = cols[i];
						if (i == fkNameColumn || i >= indexStartColumn) {
							if (!String.IsNullOrEmpty(idx) && !idx.Contains("<")) {
								if (i == fkNameColumn) {
									idx = "ndx_" + idx;
								}
								if (!indexes.ContainsKey(idx)) {
									ii = IndexInfo.GetInstance(ti);
									if (idx.Contains("UNIQUE ")) {
										ii.IndexKind = "UNIQUE";
									}
									ii.IndexName = idx.Replace("UNIQUE ", "");
									indexes.Add(idx, ii);
								}
								ii = indexes[idx];
								ii.TableName = ti.TableName;
								ii.Table = ti;
								ii.Fields.Add(fi);
							}
						}
					}

				}
			}

			if (newTables) {
				// we don't attempt to map references if they're pulling the old tables

				if (ti != null) {
					foreach (string key in indexes.Keys) {
						ti.Indexes.Add(indexes[key]);
					}
					tables.Add(ti);

					// Now, we have all the tables in the list.
					// spin back through and add constraints
					foreach (TableInfo t in tables) {
						Dictionary<string, ConstraintInfo> constraints = new Dictionary<string, ConstraintInfo>();
						ConstraintInfo ci = null;
						foreach (FieldInfo f in t.Fields) {
							if (f.IsForeignKey) {
								// a foreign key can only point to one table at a time
								// and we assume there are NO COMPOUND PRIMARY KEYS!!!

								ci = ConstraintInfo.GetInstance(t);
								ci.TableName = t.TableName;
								ci.ConstraintName = f.ForeignKeyConstraintName;
								constraints.Add(ci.ConstraintName, ci);

								ci.ReferencesTable = tables.Find(ts => {
									return ts.TableName == f.ForeignKeyTableName;
								});


								if (ci.ReferencesTable == null) {
									throw new InvalidOperationException(getDisplayMember("LoadTableInfo{reftable}", "Could not find table named '{0}' to reference that is a foreign key for table '{1}'.", f.ForeignKeyTableName, f.TableName));
								}
								ci.ReferencesTableName = ci.ReferencesTable.TableName;

								ci.SourceFields.Add(f);
								// we assume first field is ALWAYS primary key
								ci.ReferencesFields.Add(ci.ReferencesTable.Fields[0]);

							}
						}
						foreach (string key in constraints.Keys) {
							t.Constraints.Add(constraints[key]);
						}
					}
				}
			}

			// sort alphabetically
			tables.Sort(new TableInfoComparer());

			return tables;
		}

		/// <summary>
		/// Reads table schema info from __schema.xml stored in given fromFolderName.  Every TableInfo returned has IsSelected = true
		/// </summary>
		/// <param name="fromFolderName"></param>
		/// <returns></returns>
		public List<TableInfo> LoadTableInfo(string fromFolderName) {
			return LoadTableInfo(fromFolderName, null);
		}

		/// <summary>
		/// Reads table schema info from __schema.xml stored in given fromFolderName and sets IsSelected = true for table names listed in the given tableNamesToSelect
		/// </summary>
		/// <param name="fromFolderName"></param>
		/// <param name="tableNames"></param>
		/// <returns></returns>
		public List<TableInfo> LoadTableInfo(string fromFolderName, List<string> tableNamesToSelect) {
			string indir = Toolkit.ResolveDirectoryPath(fromFolderName, true);

			DataSet ds = new DataSet();
			ds.ReadXml(indir + Path.DirectorySeparatorChar + @"__schema.xml", XmlReadMode.ReadSchema);
			DataTable dtTable = ds.Tables["TableInfo"];
			DataTable dtConstraint = ds.Tables["ConstraintInfo"];


			// load all TableInfo objects
			List<TableInfo> tables = new List<TableInfo>();
			foreach (DataRow drTable in dtTable.Rows) {
				// we may have already loaded this table (via constraints)
				TableInfo ti = tables.Find(t => {
					return t.TableName.ToUpper().Trim() == drTable["table_name"].ToString().ToUpper().Trim();
				});

				if (ti == null) {
					ti = TableInfo.GetInstance(this.DataConnectionSpec);
					ti.FillFromDataSet(ds, drTable, tables);
					ti.IsSelected = tableNamesToSelect == null || tableNamesToSelect.Contains(ti.TableName);
					if (!tables.Contains(ti)) {
						tables.Add(ti);
					}
				}
			}

			// sort alphabetically
			tables.Sort(new TableInfoComparer());

			return tables;
		}

		#endregion Load from datasource

		#region Script generation

		/// <summary>
		/// Generates script for create tables described by the given TableInfo objects.
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="createMigrationKeyTables"></param>
		/// <returns></returns>
		public string ScriptTables(List<TableInfo> tables) {
			StringBuilder sb = new StringBuilder();
			// create all tables
			sb.Append("/***********************************************/\r\n");
			sb.Append("/*************** Table Definitions *************/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");
			ProcessTables(tables, ti => {
				if (ti.IsSelected) {
					sb.Append("/************ Table Definition for " + ti.SchemaName + "." + ti.TableName + " *************/\r\n");
                    if ((this is OracleCreator)) {
                        var s = ti.GenerateCreateScript().Trim();
                        sb.AppendLine("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating table " + ti.SchemaName + "." + ti.TableName + " ...') as Action from dual;");
                        sb.AppendLine(s);
                        sb.AppendLine("/");

                        //if (!s.EndsWith(";")) {
                        //    sb.AppendLine(";");
                        //} else {
                        //    sb.AppendLine();
                        //}

                    } else {
                        sb.Append("select concat(now(), ' creating table " + ti.SchemaName + "." + ti.TableName + " ...') as Action;\r\n");
                        sb.Append(ti.GenerateCreateScript());
                        //sb.Append(CommandSeparator);
                    }
					sb.Append("\r\n\r\n");

				}
			});
			string ret = sb.ToString();
			return ret;
		}

		/// <summary>
		/// Generates script for the __(tablenamehere) tables for migrating from old db schema to new db schema.
		/// </summary>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string ScriptMigrationTables(List<TableInfo> tables) {
			StringBuilder sb = new StringBuilder();
			// create all tables
			sb.Append("/***********************************************/\r\n");
			sb.Append("/******** Migration Table Definitions **********/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");
			ProcessTables(tables, ti => {
				if (ti.IsSelected) {
					sb.Append("/** Key Mapping Table Definition for " + ti.TableName + " **/\r\n");
                    if (this is OracleCreator) {
                        sb.Append("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating migration table __" + ti.TableName + " ...') as Action from dual ;\r\n");
                    } else {
                        sb.Append("select concat(now(), ' creating migration table __" + ti.TableName + " ...') as Action;\r\n");
                    }
					sb.Append(ti.GenerateCreateMapScript());
					sb.Append(CommandSeparator);
					sb.Append("\r\n\r\n");
				}
			});
			string ret = sb.ToString();
			return ret;
		}

		public string ScriptMigrationTableChangeEngine(string schemaName, TableInfo table) {
			if (!(this is MySqlCreator)) {
				return null;
			}

			StringBuilder sb = new StringBuilder();
            if (!(this is OracleCreator)) {
    			// NOTE: we don't have to issue an ENABLE KEYS because we're converting it to InnoDB (which enforces constraints unconditionally)
                sb.AppendLine(String.Format("select concat(now(), ' changing engine for {0}.{1} to {2} ...') as Action;", schemaName, table.TableName, table.Engine));
            }
			sb.AppendLine(String.Format("ALTER TABLE {0}.{1} ENGINE={2};", schemaName, table.TableName, table.Engine));

			string ret = sb.ToString();
			return ret;

		}

		public string ScriptMigrationTableCreateConstraints(string schemaName, TableInfo table) {
			if (!(this is MySqlCreator)) {
				return null;
			}

			StringBuilder sb = new StringBuilder();

            if (this is OracleCreator) {
                sb.AppendLine(String.Format("select (to_char(current_date, 'YYYY-DD-MM HH:MI:SS') || ' Adding constraints for {0}.{1} to {2} ...') as Action from dual ;", schemaName, table.TableName, table.Engine));
            } else {
                sb.AppendLine(String.Format("select concat(now(), ' Adding constraints for {0}.{1} to {2} ...') as Action;", schemaName, table.TableName, table.Engine));
            }
			sb.AppendLine(String.Format("ALTER TABLE {0}.{1}  ", schemaName, table.TableName, table.Engine));
			foreach (ConstraintInfo ci in table.Constraints) {
                if (ci is MySqlConstraintInfo) {
                    sb.Append(((MySqlConstraintInfo)ci).GeneratePartialCreateScript());
                    sb.AppendLine(", ");
                } else if (ci is OracleConstraintInfo) {
                    sb.Append(((OracleConstraintInfo)ci).GeneratePartialCreateScript());
                    sb.AppendLine(", ");
                }
			}
			sb.Remove(sb.Length - 4, 4);
			sb.AppendLine(";");

			string ret = sb.ToString();
			return ret;

		}

		/// <summary>
		/// Generates script for copying data from old db schema to new db schema. Only works within a given DBMS.
		/// </summary>
		/// <param name="csvFileName"></param>
		/// <param name="fromDBName"></param>
		/// <param name="toDBName"></param>
		/// <param name="tables"></param>
        public string ScriptMigration(string csvFileName, string fromDBName, string toDBName, List<TableInfo> tables, int updatedByCooperatorID) {

			string previousFromTableName = null;
			string previousToTableName = null;
			StringBuilder sbToFields = null;
			StringBuilder sbFromFields = null;


			StringBuilder sbMapFields = new StringBuilder("/* create table to map old table/column to new table/field */");
			sbMapFields.Append(String.Format(@"
CREATE TABLE {0}.__old_to_new (
__old_to_new_id INTEGER NOT NULL,
old_table_name varchar2(100) NOT NULL  ,
old_field_name varchar2(100) NOT NULL  ,
new_table_name varchar2(100) NOT NULL  ,
new_field_name varchar2(100) NOT NULL  ,
sys_table_id INTEGER null,
sys_table_field_id int(11) null,
CONSTRAINT pk___old_to_new PRIMARY KEY (__old_to_new_id)
USING INDEX PCTREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE GRINGLOBAL.sq___old_to_new minvalue 1 start with 1 increment by 1;
CREATE OR REPLACE TRIGGER GRINGLOBAL.tg___old_to_new BEFORE INSERT ON GRINGLOBAL.__old_to_new FOR EACH ROW BEGIN IF :NEW.__old_to_new_id IS NULL THEN SELECT GRINGLOBAL.sq___old_to_new.NEXTVAL INTO :NEW.__old_to_new_id FROM DUAL; END IF; END;
/


CREATE  INDEX {0}ndx_otn_old  ON {0}.__old_to_new (old_table_name, old_field_name);
CREATE  INDEX {0}ndx_otn_new  ON {0}.__old_to_new (new_table_name, new_field_name);
CREATE  INDEX {0}ndx_otn_st  ON {0}.__old_to_new (sys_table_id);
CREATE  INDEX {0}ndx_otn_stf  ON {0}.__old_to_new (sys_table_field_id);


/* insert statements for mapping old tablenames to new tablenames */
INSERT INTO {0}.__old_to_new (old_table_name, old_field_name, new_table_name, new_field_name) VALUES 
", toDBName));

			string fromPkName = null;
			string toPkName = null;

			bool containsCreatedDate = false;
			bool containsModifiedDate = false;

			string citationTableFromFields = null;
			string citationTableToFields = null;

			string urlTableFromFields = null;
			string urlTableToFields = null;

			// we use the ordering of the tables list to tell us the dependencies among objects.
			// this means all 'new' tables need to be in dependency order, as they will simply be tacked on the end.
			string[] keyInserts = new string[1000];
			string[] realInserts = new string[1000];
			int offset = -1;

			bool originalContainsUserId = false;

			foreach (List<string> cols in Toolkit.ParseCSV(csvFileName, false)) {

				// Format:
				// current table name
				// current field name
				// current field type
				// new table name
				// new field name
				// new field type
				// comments
				// max length
				// foreign key name
				// foreign key table
				// foreign key field
				// index 1
				// index 2
				// ..
				// index 10

				string fromTableName = cols[0].ToLower();
				string fromFieldName = cols[1].ToLower();
				string fromFieldType = cols[2].ToLower();
				string toTableName = cols[3].ToLower();
				string toFieldName = cols[4].ToLower();
				string toFieldType = cols[5].ToLower();
				string toFkTableName = null;
				string toFkFieldName = null;
				bool isFK = false;
				bool isAutoIncrement = false;
				bool isNullable = false;
				if (cols.Count > 6) {
					isAutoIncrement = cols[6].ToUpper().Contains("AUTO INCREMENT");
					isNullable = cols[6].ToUpper().Contains("NULLABLE");
					if (cols.Count > 8) {
						isFK = !String.IsNullOrEmpty(cols[8]) && !cols[8].Contains("<");
						if (cols.Count > 9) {
							toFkTableName = cols[9].ToLower();
							if (cols.Count > 10) {
								toFkFieldName = cols[10].ToLower();
							}
						}
					}
				}



				if (toTableName == "") {
					// empty row, ignore and continue
					continue;
				} else {

					if (!originalContainsUserId) {
						originalContainsUserId = fromFieldName == "userid";
					}
					if (!containsCreatedDate) {
						containsCreatedDate = fromFieldName == "created";
					}
					if (!containsModifiedDate) {
						containsModifiedDate = fromFieldName == "modified";
					}

//					string defaultDateValue = "STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '%Y-%m-%d %H:%i:%s')";
//					string defaultDateValue = "STR_TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', '%Y-%m-%d')";
					string defaultDateValue = "'" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00'";

					sbMapFields.AppendLine(String.Format("('{0}', '{1}', '{2}', '{3}'),", fromTableName.ToLower(), fromFieldName.ToLower(), toTableName.ToLower(), toFieldName.ToLower()));


					if (toTableName != previousToTableName) {
						// new table defined



						if (previousToTableName != null && tables.Exists(t => {
							if (t.TableName.ToLower() == previousToTableName.ToLower()) {

								if (!t.IsSelected) {
									return false;
								}
								offset = tables.IndexOf(t);
								return true;
							}
							return false;
						})) {
							// kick out the sql statement for this guy



							if (previousFromTableName == "" || previousFromTableName.Contains("<") || toTableName.ToLower().StartsWith("sys_")) {
								realInserts[offset] = "/* " + previousToTableName + " **************** No previous definition **************** */\r\n\r\n";

								if (previousToTableName.ToLower() == "code_value_lang") {

									// copy data from code_value.def and code_value.cmt fields into new records in the code_value_lang table

                                    if (this is OracleCreator) {
                                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' copying language-specific code_value data into code_value_lang') as Action from dual;\r\n";
                                    } else {
                                        realInserts[offset] += "select concat(now(), ' copying language-specific code_value data into code_value_lang') as Action;\r\n";
                                    }
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1),
	//{3},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1),
	//{2},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1)
									realInserts[offset] += String.Format(@"
insert into {0}.code_value_lang
	(code_value_id, sys_lang_id, name, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	(select new_id from {0}.__code_value where previous_id = cv_orig.code_value_id),
	(select sys_lang_id from {0}.sys_lang where iso_639_3_code = 'ENG'),
	cv_orig.def,
	cv_orig.cmt,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1)
from
	{1}.code_value cv_orig;

", toDBName, fromDBName, (containsCreatedDate ? "cv_orig.created" : defaultDateValue), (containsModifiedDate ? "cv_orig.modified" : defaultDateValue));

	

								} else if (previousToTableName.ToLower() == "crop_trait_code_lang") {

									// copy data from crop_trait_code.code and crop_trait_code.def fields into new records in the crop_trait_code_lang table

                                    if (this is OracleCreator) {
                                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' copying language-specific trait_code data into crop_trait_code_lang') as Action from dual;\r\n";
                                    } else {
                                        realInserts[offset] += "select concat(now(), ' copying language-specific trait_code data into crop_trait_code_lang') as Action;\r\n";
                                    }
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),1),
	//{3},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),1),
	//{2},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),1)
									realInserts[offset] += String.Format(@"
insert into {0}.crop_trait_code_lang
	(crop_trait_code_id, sys_lang_id, name, description, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	(select new_id from {0}.__crop_trait_code where previous_id = tc_orig.cdid),
	(select sys_lang_id from {0}.sys_lang where iso_639_3_code = 'ENG'),
	tc_orig.code,
	tc_orig.def,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = tc_orig.userid)),1)
from
	{1}.cd tc_orig;

", toDBName, fromDBName, (containsCreatedDate ? "tc_orig.created" : defaultDateValue), (containsModifiedDate ? "tc_orig.modified" : defaultDateValue));

								} else if (previousToTableName.ToLower() == "accession_voucher_image_map") {

									// first create mapping from old vou to new imaging (for thumbnails)

									keyInserts[offset] = "/* " + previousToTableName + " */\r\n";
									keyInserts[offset] += String.Format("insert into {0}.__image (previous_id, table_name) select vno, 'accession_voucher' from {1}.vou where {1}.vou.thumbnail is not null or {1}.vou.vloc is not null order by {1}.vou.vno;\r\n\r\n",
										toDBName, fromDBName);

									// copy data from vou.thumbnail as needed
                                    if (this is OracleCreator) {
                                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating image records needed for accession_voucher_image_map table to point at...') as Action from dual;\r\n";
                                    } else {
                                        realInserts[offset] += "select concat(now(), ' creating image records needed for accession_voucher_image_map table to point at...') as Action;\r\n";
                                    }
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1),
	//{3},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1),
	//{2},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1)
									realInserts[offset] += String.Format(@"
insert into {0}.image
	(virtual_path, thumbnail_virtual_path, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	av_orig.vloc,
	av_orig.thumbnail,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1)
from
	{1}.vou av_orig
where
	av_orig.thumbnail is not null or av_orig.vloc is not null
order by 
	av_orig.vno;


", toDBName, fromDBName, (containsCreatedDate ? "av_orig.created" : defaultDateValue), (containsModifiedDate ? "av_orig.modified" : defaultDateValue));

                                    if (this is OracleCreator) {
                                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating records as needed for accession_voucher_image_map table...') as Action from dual;\r\n";
                                    } else {
                                        realInserts[offset] += "select concat(now(), ' creating records as needed for accession_voucher_image_map table...') as Action;\r\n";
                                    }

	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1),
	//{3},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1),
	//{2},
	//coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1)
						realInserts[offset] += String.Format(@"
insert into {0}.accession_voucher_image_map
	(accession_voucher_id, image_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	(select new_id from {0}.__accession_voucher where previous_id = av_orig.vno and table_name = 'accession_voucher'),
	(select new_id from {0}.__image where previous_id = av_orig.vno and table_name = 'accession_voucher'),
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = av_orig.userid)),1)
from
	{1}.vou av_orig
where
	av_orig.thumbnail is not null or av_orig.vloc is not null;

", toDBName, fromDBName, (containsCreatedDate ? "av_orig.created" : defaultDateValue), (containsModifiedDate ? "av_orig.modified" : defaultDateValue));

								} else if (previousToTableName.ToLower() == "code_group"){

									keyInserts[offset] = "/* " + previousToTableName + " */\r\n";
									keyInserts[offset] += String.Format("insert into {0}.__code_group (previous_id, table_name) select distinct(code_no), 'code_group' from {1}.code_value order by code_no;\r\n\r\n",
										toDBName, fromDBName);

									// copy unique code_no's from code_value
                                    if (this is OracleCreator) {
                                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating code_group records needed for code_value table to point at...') as Action from dual;\r\n";
                                    } else {
                                        realInserts[offset] += "select concat(now(), ' creating code_group records needed for code_value table to point at...') as Action;\r\n";
                                    }

									realInserts[offset] += String.Format(@"
insert into {0}.code_group
	(name, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select
	distinct(concat('Group ', cv_orig.code_no)),
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = cv_orig.userid)),1)
from
	{1}.code_value cv_orig;
", toDBName, fromDBName, defaultDateValue, "null");

								} else {
									switch (previousToTableName.ToLower()) {
										case "citation":
											// remember these for later.
											// acit/mcit/gcit/tcit/ecit etc will need it
											citationTableFromFields = sbFromFields.ToString();
											citationTableFromFields = citationTableFromFields.Substring(0, citationTableFromFields.Length - 2);
											citationTableToFields = sbToFields.ToString();
											citationTableToFields = citationTableToFields.Substring(0, citationTableToFields.Length - 2);
											break;
										case "url":
											// remember these for later.
											// durl / turl will need them.
											urlTableFromFields = sbFromFields.ToString();
											urlTableFromFields = urlTableFromFields.Substring(0, urlTableFromFields.Length - 2);
											urlTableToFields = sbToFields.ToString();
											urlTableToFields = urlTableToFields.Substring(0, urlTableToFields.Length - 2);
											break;
										case "image":
										case "accession_image_map":
										case "inventory_image_map":
										case "accession_citation_map":
                                        case "sys_index":
                                        case "sys_index_field":
                                        case "sys_dataview_filter":
                                        case "sys_table_filter":
                                            // these have no corresponding table in the original schema, so they're ok to ignore
											realInserts[offset] = "/* Nothing to do for " + previousToTableName + ": no corresponding table in the original schema */\r\n";
                                            if (this is OracleCreator) {
                                                realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' ignoring table " + previousToTableName + " as there is no corresponding table in the original schema') as Action from dual;\r\n";
                                            } else {
                                                realInserts[offset] += "select concat(now(), ' ignoring table " + previousToTableName + " as there is no corresponding table in the original schema') as Action;\r\n";
                                            }
											break;
										default:
											// this isn't mapped right!
											//throw new InvalidOperationException("to table=" + previousToTableName + " isn't mapped properly in ScriptMigration()!");
                                            break;
									}
								}
							} else if (previousFromTableName.ToLower() == "durl"
								|| previousFromTableName.ToLower() == "turl"){

								// write current keys to "url" key table
								keyInserts[offset] = "/* " + previousToTableName + " */\r\n";
                                if (this is OracleCreator) {
                                    keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' mapping old id to new id for " + previousToTableName + " ...') as Action from dual;\r\n";
                                } else {
                                    keyInserts[offset] += "select concat(now(), ' mapping old id to new id for " + previousToTableName + " ...') as Action;\r\n";
                                }
								keyInserts[offset] += String.Format("insert into {0}.__url (previous_id, table_name) select {2}, '{1}' from {3}.{4};\r\n\r\n",
									toDBName, previousToTableName, fromPkName, fromDBName, previousFromTableName);

								if (sbFromFields.Length > 2) {
									sbFromFields.Remove(sbFromFields.Length - 2, 2);
								}
								if (sbToFields.Length > 2) {
									sbToFields.Remove(sbToFields.Length - 2, 2);
								}


								switch (previousToTableName) {



									case "taxonomy_url":

										// write current data to "url" table
										realInserts[offset] = "/* " + previousToTableName + " */\r\n";
                                        if (this is OracleCreator) {
                                            realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' copying data to url (for mapping from " + previousToTableName + ") ...') as Action from dual;\r\n";
                                        } else {
                                            realInserts[offset] += "select concat(now(), ' copying data to url (for mapping from " + previousToTableName + ") ...') as Action;\r\n";
                                        }
										realInserts[offset] +=
											String.Format(@"
insert into {0}.url 
(caption, url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	url_orig.caption, 
	url_orig.url, 
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1)
from {1}.turl url_orig;

",
												toDBName, 
												fromDBName,
												containsCreatedDate ? "coalesce(url_orig.created," + defaultDateValue + ")" : defaultDateValue,
												containsModifiedDate ? "coalesce(url_orig.modified," + defaultDateValue + ")" : defaultDateValue);


										// map current data to new data via the *_map table
                                        if (this is OracleCreator) {
                                            realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating mapping from url to " + previousToTableName + " ...') as Action from dual;\r\n";
                                        } else {
                                            realInserts[offset] += "select concat(now(), ' creating mapping from url to " + previousToTableName + " ...') as Action;\r\n";
                                        }


										realInserts[offset] += String.Format(@"
insert into {0}.taxonomy_url
	(url_type, taxonomy_family_id, taxonomy_genus_id, taxonomy_id, url_id, note, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	url_orig.urltype,
	(select new_id from {0}.__taxonomy_family where previous_id = url_orig.famno and table_name = 'taxonomy_family'),
	(select new_id from {0}.__taxonomy_genus where previous_id = url_orig.gno and table_name = 'taxonomy_genus'),
	(select new_id from {0}.__taxonomy where previous_id = url_orig.taxno and table_name = 'taxonomy'),
	(select new_id from {0}.__url where previous_id = url_orig.turlno and table_name = 'taxonomy_url'),
	url_orig.cmt,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1)
from
	{1}.turl url_orig;

",
											 toDBName,
											 fromDBName,
											 (containsCreatedDate ? "url_orig.created" : defaultDateValue),
											 (containsModifiedDate ? "url_orig.modified" : defaultDateValue));

										break;
									case "crop_trait_url":

										// write current data to "url" table
										realInserts[offset] = "/* " + previousToTableName + " */\r\n";
                                        if (this is OracleCreator) {
                                            realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' copying data to url (for mapping from " + previousToTableName + ") ...') as Action from dual;\r\n";
                                        } else {
                                            realInserts[offset] += "select concat(now(), ' copying data to url (for mapping from " + previousToTableName + ") ...') as Action;\r\n";
                                        }
										realInserts[offset] +=
											String.Format(@"
insert into {0}.url 
(caption, url, created_date, created_by, modified_date, modified_by, owned_date, owned_by)
select 
	url_orig.caption, 
	url_orig.url, 
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1)
from {1}.durl url_orig;

",
												toDBName,
												fromDBName,
												containsCreatedDate ? "coalesce(url_orig.created," + defaultDateValue + ")" : defaultDateValue,
												containsModifiedDate ? "coalesce(url_orig.modified," + defaultDateValue + ")" : defaultDateValue);


										// map current data to new data via the *_map table
                                        if (this is OracleCreator) {
                                            realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating mapping from url to " + previousToTableName + " ...') as Action from dual;\r\n";
                                        } else {
                                            realInserts[offset] += "select concat(now(), ' creating mapping from url to " + previousToTableName + " ...') as Action;\r\n";
                                        }

										realInserts[offset] += String.Format(@"
insert into {0}.crop_trait_url
	(url_type, sequence_number, crop_id, crop_trait_id, code, url_id, note, method_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	url_orig.urltype,
	url_orig.seqno,
	(select new_id from {0}.__crop where previous_id = url_orig.cropno and table_name = 'crop'),
	(select new_id from {0}.__crop_trait where previous_id = url_orig.dno and table_name = 'crop_trait'),
	url_orig.code,
	(select new_id from {0}.__url where previous_id = url_orig.durlid and table_name = 'crop_trait_url'),
	url_orig.cmt,
	(select new_id from {0}.__method where previous_id = url_orig.eno and table_name = 'method'),
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1),
	{3},
	case when {3} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1) end,
	{2},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = url_orig.userid)),1)
from
	{1}.durl url_orig;

",
											 toDBName,
											 fromDBName,
											 (containsCreatedDate ? "coalesce(url_orig.created," + defaultDateValue + ")" : defaultDateValue),
											 (containsModifiedDate ? "coalesce(url_orig.modified," + defaultDateValue + ")" : defaultDateValue));

										break;
									default:
										throw new NotImplementedException("Table '" + previousToTableName + " not mapped in switch() within ScriptMigration() for url data!");
								}

								//insert into dev5.accession_citation_map
								//    (accession_id, citation_id)
								//select
								//    (select new_id from __accession where previous_id = cit_orig.acid),
								//    (select new_id from __citation where previous_id = cit_orig.citno)
								//from
								//    dev2.acit cit_orig















							} else if (previousFromTableName.ToLower() == "gcit"
								|| previousFromTableName.ToLower() == "mcit"
								|| previousFromTableName.ToLower() == "ecit"
								|| previousFromTableName.ToLower() == "acit"
								|| previousFromTableName.ToLower() == "tcit"){

								// write current keys to "citation" key table
								// HACK: all the citation tables write to the same key table
								//       we can tell the difference by the 'table_name' field that is filled in with the 'new' table name (e.g. accession_citation_map)
								keyInserts[offset] = "/* " + previousToTableName + " */\r\n";
                                if (this is OracleCreator) {
                                    keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' mapping old id to new id for " + previousToTableName + " ...') as Action from dual;\r\n";
                                } else {
                                    keyInserts[offset] += "select concat(now(), ' mapping old id to new id for " + previousToTableName + " ...') as Action;\r\n";
                                }
								keyInserts[offset] += String.Format("insert into {0}.__citation (previous_id, table_name) select {2}, '{1}' from {3}.{4};\r\n\r\n",
									toDBName, previousToTableName, fromPkName, fromDBName, previousFromTableName);

								if (sbFromFields.Length > 2) {
									sbFromFields.Remove(sbFromFields.Length - 2, 2);
								}
								if (sbToFields.Length > 2) {
									sbToFields.Remove(sbToFields.Length - 2, 2);
								}

								// write current data to "citation" table
								realInserts[offset] = "/* " + previousToTableName + " */\r\n";
                                if (this is OracleCreator) {
                                    realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' copying data to citation (for mapping from " + previousToTableName + ") ...') as Action from dual;\r\n";
                                } else {
                                    realInserts[offset] += "select concat(now(), ' copying data to citation (for mapping from " + previousToTableName + ") ...') as Action;\r\n";
                                }
								realInserts[offset] +=
									String.Format("insert into {0}.citation ({2})\r\nselect {3} from {4}.{5};\r\n\r\n",
									toDBName, previousToTableName, citationTableToFields,
									citationTableFromFields.Replace("__REPLACE_WITH_REAL_FROM_TABLE__", previousFromTableName), fromDBName, previousFromTableName);


								string oldPK = "citno";
								string oldFK = null;
								string newFK = null;
								string oldTableName = null;
								string newTableName = null;
								switch(previousToTableName){
									case "accession_citation_map":
										oldFK = "acid";
										oldTableName = "acit";
										newTableName = "accession";
										newFK = "accession_id";
										break;
									case "method_citation_map":
										oldFK = "eno";
										oldTableName = "ecit";
										newTableName = "method";
										newFK = "method_id";
										break;
									case "genomic_marker_citation_map":
										oldFK = "mrkno";
										oldTableName = "mcit";
										newTableName = "genomic_marker";
										newFK = "genomic_marker_id";
										break;
									case "taxonomy_citation_map":
										oldFK = "taxno";
										oldTableName = "tcit";
										newTableName = "taxonomy";
										newFK = "taxonomy_id";
										break;
									case "taxonomy_genus_citation_map":
										oldFK = "gno";
										oldTableName = "gcit";
										newTableName = "taxonomy_genus";
										newFK = "taxonomy_genus_id";
										break;
									default:
										throw new NotImplementedException("Table '" + previousToTableName + " not mapped in switch() within ScriptMigration()!");
								}

								// map current data to new data via the *_map table
                                if (this is OracleCreator) {
                                    realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating mapping from citation to " + previousToTableName + " ...') as Action from dual;\r\n";
                                } else {
                                    realInserts[offset] += "select concat(now(), ' creating mapping from citation to " + previousToTableName + " ...') as Action;\r\n";
                                }
//insert into dev5.accession_citation_map
//    (accession_id, citation_id)
//select
//    (select new_id from __accession where previous_id = cit_orig.acid),
//    (select new_id from __citation where previous_id = cit_orig.citno)
//from
//    dev2.acit cit_orig
	

		
								

								realInserts[offset] += String.Format(@"
insert into {0}.{1} 
	({2}, citation_id, created_date, created_by, modified_date, modified_by, owned_date, owned_by) 
select
	(select new_id from {0}.__{3} where previous_id = cit_orig.{6} and table_name = '{3}'),
	(select new_id from {0}.__citation where previous_id = cit_orig.{7} and table_name = '{1}'),
	{8},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {4}.siteuser su_orig where su_orig.siteuser = cit_orig.userid)),1),
	{9},
	case when {9} is null then null else coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {4}.siteuser su_orig where su_orig.siteuser = cit_orig.userid)),1) end,
	{8},
	coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {4}.siteuser su_orig where su_orig.siteuser = cit_orig.userid)),1)
from
	{4}.{5} cit_orig;

", 
									 toDBName, 
									 previousToTableName, 
									 newFK, 
									 newTableName, 
									 fromDBName,
									 oldTableName, 
									 oldFK, 
									 oldPK,
									 (containsCreatedDate ? "cit_orig.created" : defaultDateValue),
									 (containsModifiedDate ? "cit_orig.modified" : defaultDateValue));


							} else {

								if (sbFromFields.Length > 2) {
									sbFromFields.Remove(sbFromFields.Length - 2, 2);
								}
								if (sbToFields.Length > 2) {
									sbToFields.Remove(sbToFields.Length - 2, 2);
								}

								keyInserts[offset] = "/* " + previousToTableName + " */\r\n";
								if (String.IsNullOrEmpty(toPkName)) {
                                    if (this is OracleCreator) {
                                        keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no mapping to do for " + previousToTableName + " because it has no primary key') as Action from dual;\r\n";
                                    } else {
                                        keyInserts[offset] += "select concat(now(), ' no mapping to do for " + previousToTableName + " because it has no primary key') as Action;\r\n";
                                    }
									keyInserts[offset] += "select concat(now(), ' no mapping to do for " + previousToTableName + " because it has no primary key') as Action;\r\n";
								} else {
									if (previousToTableName.ToLower() == "cooperator"){
										// first row we insert must be a bogus 'SYSTEM' type of row (we use 0 as the 'old' id because it won't be in the old data -- the 'new' id will be 1 though)
                                    if (this is OracleCreator){
                                        keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating SYSTEM as cooperator 1 ...') as Action from dual;\r\n";
                                    } else {
                                        keyInserts[offset] += "select concat(now(), ' creating SYSTEM as cooperator 1 ...') as Action;\r\n";
                                    }
										keyInserts[offset] += String.Format("insert into {0}.__cooperator (previous_id, table_name) values (0, 'cooperator');\r\n\r\n",
											toDBName);
									}


                                    

                                    if (this is OracleCreator){
                                        keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' mapping old id to new id for " + previousToTableName + " ...') as Action from dual;\r\n";
                                    } else {
									keyInserts[offset] += "select concat(now(), ' mapping old id to new id for " + previousToTableName + " ...') as Action;\r\n";
                                    }
									keyInserts[offset] += String.Format("insert into {0}.__{1} (previous_id, table_name) select {2}, '{1}' from {3}.{4};\r\n\r\n",
										toDBName, previousToTableName, fromPkName, fromDBName, previousFromTableName);
								}

								realInserts[offset] = "/* " + previousToTableName + " */\r\n";

								if (previousToTableName.ToLower() == "cooperator") {
									// first row we insert must be a bogus "SYSTEM" type of row
									realInserts[offset] += "/* creating bogus 'SYSTEM' row for cooperator = 1 */\r\n";
                                    if (this is OracleCreator) {
                                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating SYSTEM as cooperator 1 ...') as Action from dual;";
                                    } else {
                                        realInserts[offset] += "select concat(now(), ' creating SYSTEM as cooperator 1 ...') as Action;";
                                    }
									realInserts[offset] += String.Format(@"
insert into {0}.cooperator 
	(last_name, is_active, first_name, note, created_date, created_by, owned_date, owned_by)
values
	('SYSTEM', 'N', 'SYSTEM', 'Default SYSTEM user required by Grin-Global', {1}, {2}, {1}, {2});
", toDBName, defaultDateValue, updatedByCooperatorID);
									realInserts[offset] += "/* creating real rows for cooperator */\r\n";
								}

								realInserts[offset] += "select concat(now(), ' copying data to " + previousToTableName + " ...') as Action;\r\n";
								realInserts[offset] +=
									String.Format("insert into {0}.{1} ({2})\r\nselect {3} from {4}.{5};\r\n\r\n",
									toDBName, previousToTableName, sbToFields.ToString(),
									sbFromFields.ToString(), fromDBName, previousFromTableName);


								if (previousToTableName.ToLower() == "app_user_item_list") {

									// we need to re-map all the list records appropriately
									realInserts[offset] += String.Format(@"
update
	{0}.app_user_item_list
set
	id_number = (select __a.new_id from {0}.__accession __a where __a.previous_id = id_number)
where
	id_type IN ('ACID','accession_id');

update 
	{0}.app_user_item_list
set
	id_number = (select __i.new_id from {0}.__inventory __i where __i.previous_id = id_number)
where
	id_type IN ('IVID', 'inventory_id');

update
	{0}.app_user_item_list
set
	id_number = (select __or.new_id from {0}.__order_request __or where __or.previous_id = id_number)
where
	id_type IN ('ORNO', 'order_request_id');
", toDBName);

								}
							}
						}

						// reset all variables that change per table
						previousFromTableName = fromTableName;
						previousToTableName = toTableName;
						sbFromFields = new StringBuilder();
						sbToFields = new StringBuilder();
						fromPkName = null;
						toPkName = null;

						containsCreatedDate = false;
						containsModifiedDate = false;
						originalContainsUserId = false;
					}

					if (isAutoIncrement) {
						fromPkName = fromFieldName;
						toPkName = toFieldName;
					}



					if (!toFieldName.Contains("<") && !String.IsNullOrEmpty(toFieldType) && !isAutoIncrement) {

						if (isFK && toTableName.ToLower().Contains("_citation_map")) {
							// ignore foreign keys for citation map tables, as they are mapped in a special way handled in the table-switching code
						} else {


							sbToFields.Append(toFieldName).Append(", ");

							switch (toFieldName.ToLower()) {
								case "modified_by":
								case "owned_by":
								case "created_by":
									// special case, either doesn't exist or we need to 
									// do additional mapping on a second pass.
									if (originalContainsUserId) {

                                        sbFromFields.Append(updatedByCooperatorID + ", ");

										// lookup the cooperator from the table based on the userid
										//sbFromFields.Append(String.Format(@"coalesce((select new_id from {0}.__cooperator where previous_id = (select su_orig.cno from {1}.siteuser su_orig where su_orig.siteuser = {1}.{2}.userid)),1), ",
										//    toDBName, fromDBName, (fromTableName.Contains("<") ? "__REPLACE_WITH_REAL_FROM_TABLE__" : fromTableName)));
									} else if (toFieldName.ToLower() == "modified_by" && containsModifiedDate){
										// only add a 'default' modified_by if the modified_date is not null
										sbFromFields.Append("case when modified is null then null else 1 end, ");
									} else {
										// default to a 1 (system), as there is no data in the current grin system.
                                        sbFromFields.Append(updatedByCooperatorID + ", ");
									}
									break;
								case "created_date":
								case "created_at":
									if (containsCreatedDate) {
										sbFromFields.Append("coalesce(created, " + defaultDateValue + "), ");
									} else {
										sbFromFields.Append(defaultDateValue + ", ");
									}
									break;
								case "modified_at":
								case "modified_date":
									if (containsModifiedDate) {
										sbFromFields.Append("coalesce(modified, " + defaultDateValue + "), ");
									} else {
										sbFromFields.Append(defaultDateValue + ", ");
									}
									break;
								case "owned_date":
								case "owned_at":
									if (containsCreatedDate) {
										sbFromFields.Append("coalesce(created, " + defaultDateValue + "), ");
									} else {
										sbFromFields.Append(defaultDateValue + ", ");
									}
									break;
								default:
									if (!isFK) {
										if (toFieldName.ToLower().StartsWith("is_") || toFieldName.ToLower().StartsWith("can_")) {
											sbFromFields.Append(String.Format("coalesce(case when {0} = 'X' then 'Y' else {0} end, 'N'), ", fromFieldName));
										} else if (toFieldName.ToLower().Contains("_year_date")) {
											sbFromFields.Append("STR_TO_DATE(CONCAT(").Append(fromFieldName).Append(", '0101'), '%Y%m%d'), ");
										} else if (fromTableName.ToLower() == "acc" && toFieldName.ToLower() == "backup_location"){
											// special case for accession.backup_location
											// it is mapped based on the value of the BACKUP flag.
											sbFromFields.Append("case acc.backup when 'X' then 'NSSL' when 'D' then 'Domestic non-NPGS' when 'F' then 'Foreign' when 'L' then 'Local on-site' when 'S' then 'Other NPGS site' else null end, ");
										} else if (String.IsNullOrEmpty(fromFieldName)) {
											// should rarely happen, but if we have no source field, assume null.
											sbFromFields.Append("null, ");
										} else if (fromTableName.ToLower() == "site" && toFieldName.ToLower() == "site_type") {
											sbFromFields.Append("case site.clonal when 'Y' then 'CLNL' when 'M' then 'BOTH' else 'SEED' end, ");
										} else {
											sbFromFields.Append(fromFieldName).Append(", ");
										}
									} else {

										// map old fk to new fk via the appropriate mapping table (always start with "__")

										if (fromFieldType != toFieldType || fromFieldName == "" || fromFieldType == "") {
											Debug.WriteLine(fromTableName + "." + fromFieldName + " (" + fromFieldType + ") -> " + toTableName + "." + toFieldName + " (" + toFieldType + ")");
										}


										// we have special cases for ones that are joined on a string type in the original db
										switch (toFieldName.ToLower()) {
											case "code_group_code":
												sbFromFields.Append(String.Format("(select __cdgrp.new_id from {0}.__code_group __cdgrp where __cdgrp.previous_id = {1}.{2}.{3}), ", toDBName, fromDBName, fromTableName, (fromTableName.ToLower() == "sys_table_field" ? "lookup_code_no" : "code_no")));
												break;

											case "crop_trait_code_id":
												// map ob.ob to the fk equivalent (crop_trait_code_id)
												sbFromFields.Append(String.Format(@"
case when (select count(1) from {1}.dsc where {1}.dsc.dno = {1}.ob.dno and {1}.dsc.usecode = 'X') > 0 then
(select ctc.crop_trait_code_id from {0}.crop_trait_code ctc where 
ctc.crop_trait_id = 
	(select new_id from dev5.__crop_trait where previous_id = {1}.ob.dno and table_name = 'crop_trait')
and ctc.code = {1}.ob.ob) else null end , ",
													toDBName, fromDBName));
												break;
                                            case "numeric_value":
                                                if (toTableName.ToLower() == "crop_trait_observation") {
                                                    sbFromFields.Append(String.Format(@"
case when (select count(1) from {1}.dsc where {1}.dsc.dno = {1}.ob.dno and {1}.dsc.obtype = 'NUMERIC' and {1}.dsc.usecode is null) > 0 then
convert({1}.ob.ob, decimal(18,5)) else null end , ",
 toDBName, fromDBName));
                                                } else {
                                                    sbFromFields.Append(String.Format("{1}.{2}, ", toDBName, fromDBName, fromFieldName));
                                                }
                                                break;
                                            case "string_value":
                                                if (toTableName.ToLower() == "crop_trait_observation") {
                                                    sbFromFields.Append(String.Format(@"
case when (select count(1) from {1}.dsc where {1}.dsc.dno = {1}.ob.dno and {1}.dsc.obtype = 'CHAR' and {1}.dsc.usecode is null) > 0 then
{1}.ob.ob else null end , ",
 toDBName, fromDBName));
                                                } else {
                                                    sbFromFields.Append(String.Format("{1}.{2}, ", toDBName, fromDBName, fromFieldName));
                                                }
                                                break;
											case "accession_group_id":
												// grab agid based on agname
												sbFromFields.Append(String.Format("(select new_id from {0}.__accession_name where previous_id = (select ag_orig.agid from {1}.ag ag_orig where ag_orig.agname = {1}.{2}.{3})), ",
													toDBName, fromDBName, fromTableName, fromFieldName));
												break;
											case "sys_lang_id":
												// grab sys_lang_id baesd on iso_639_3_code
												sbFromFields.Append(String.Format("coalesce((select new_id from {0}.__sys_lang where previous_id = (select sl_orig.sys_lang_id from {1}.sys_lang sl_orig where sl_orig.iso_639_3_code = {1}.{2}.{3})), 1), ",
													toDBName, fromDBName, fromTableName, fromFieldName));
												break;
											case "literature_id":
												// grab litid based on abbr
												sbFromFields.Append(String.Format("(select new_id from {0}.__literature where previous_id = (select lit_orig.litid from {1}.lit lit_orig where lit_orig.abbr = {1}.{2}.{3})), ",
													toDBName, fromDBName, (fromTableName.Contains("<") ? "__REPLACE_WITH_REAL_FROM_TABLE__" : fromTableName), fromFieldName));
												break;
											case "inventory_maint_policy_id":
												// grab imid based on imname AND site
												// HACK: check out the "SITE" portion of the sql below
												sbFromFields.Append(String.Format("(select new_id from {0}.__inventory_maint_policy where previous_id = (select im_orig.imid from {1}.im im_orig where im_orig.imname = {1}.{2}.{3} and im_orig.site = {1}.{2}.SITE)), ",
													toDBName, fromDBName, fromTableName, fromFieldName));
												break;
											case "inventory_group_id":
												// grab igmid based on igname AND site
												// HACK: check out the "SITE" portion of the sql below
												sbFromFields.Append(String.Format("(select new_id from {0}.__inventory_group where previous_id = (select ig_orig.igid from {1}.ig ig_orig where ig_orig.igname = {1}.{2}.{3} and ig_orig.site = {1}.{2}.SITE)), ",
													toDBName, fromDBName, fromTableName, fromFieldName));
												break;
											case "cooperator_group_id":
												// grab cgid_int based on cgid (which is a string in original db)
												sbFromFields.Append(String.Format("(select new_id from {0}.__cooperator_group where previous_id = (select cg_orig.cgid_int from {1}.cg cg_orig where cg_orig.cgid = {1}.{2}.{3})), ",
													toDBName, fromDBName, fromTableName, fromFieldName));
												break;
											case "accession_name_id":
												// grab anno based on which is "primary" (i.e. idrank is lowest, preferring topname='X' records)
												sbFromFields.Append(String.Format("(select new_id from {0}.__accession_name where previous_id = (select an_orig.anno from {1}.an an_orig where an_orig.acid = {1}.{2}.acid order by an_orig.idrank limit 1)), ",
													toDBName, fromDBName, fromTableName, fromFieldName));
												break;

											default:
												// nothing special to lookup, just join to it
												// map old fk value to new fk value
												sbFromFields.Append(String.Format("(select new_id from {0}.__{1} where previous_id = {2}.{3}.{4} and table_name = '{1}'), ",
													toDBName, toFkTableName, fromDBName, fromTableName, fromFieldName));
												break;
										}

									}
									break;
							}
						}
					}
				}

			}

			if (previousToTableName != null && tables.Exists(t => {
				if (t.TableName.ToLower() == previousToTableName.ToLower()) {
					if (t.IsSelected) {
						offset = tables.IndexOf(t);
						return true;
					}
				}
				return false;
			})) {
				if (previousFromTableName == "") {
					realInserts[offset] = "/* " + previousToTableName + " **************** No previous definition **************** */\r\n\r\n";
				} else {
					// kick out the sql statement for this guy
					if (sbFromFields.Length > 2) {
						sbFromFields.Remove(sbFromFields.Length - 2, 2);
					}
					if (sbToFields.Length > 2) {
						sbToFields.Remove(sbToFields.Length - 2, 2);
					}
					keyInserts[offset] = "/* " + previousToTableName + " */\r\n";
					if (String.IsNullOrEmpty(toPkName)) {
                        if (this is OracleCreator) {
                            keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no mapping to do for " + previousToTableName + " because it has no primary key') as Action from dual;\r\n";
                        } else {
                            keyInserts[offset] += "select concat(now(), ' no mapping to do for " + previousToTableName + " because it has no primary key') as Action;\r\n";
                        }
					} else {
                        if (this is OracleCreator) {
                            keyInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' mapping old id to new id for " + previousToTableName + " ...') as Action from dual;\r\n";
                        } else {
		    				keyInserts[offset] += "select concat(now(), ' mapping old id to new id for " + previousToTableName + " ...') as Action;\r\n";
                        }
						keyInserts[offset] += String.Format("insert into {0}.__{1} (previous_id, table_name) select {2}, '{1}' from {3}.{4};\r\n\r\n",
							toDBName, previousToTableName, fromPkName, fromDBName, previousFromTableName);
					}

					realInserts[offset] = "/* " + previousToTableName + " */\r\n";
                    if (this is OracleCreator) {
                        realInserts[offset] += "select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' copying data to " + previousToTableName + " ...') as Action from dual;\r\n";
                    } else {
                        realInserts[offset] += "select concat(now(), ' copying data to " + previousToTableName + " ...') as Action;\r\n";
                    }
					realInserts[offset] +=
						String.Format("insert into {0}.{1} ({2})\r\nselect {3} from {4}.{5};\r\n\r\n",
						toDBName, previousToTableName, sbToFields.ToString(),
						sbFromFields.ToString(), fromDBName, previousFromTableName);

				}
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("/***********************************************/\r\n");
			sb.Append("/********* Insert Key Mapping Data *************/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");
			sb.Append(String.Join("", keyInserts));
			sb.Append("\r\n\r\n");
			sb.Append("/***********************************************/\r\n");
			sb.Append("/********* Insert Real Data ********************/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");
			sb.Append(String.Join("", realInserts));
			sb.Append("\r\n");
            //sb.Append("/***********************************************/\r\n");
            //sb.Append("/********* Cleanup Scripts         *************/\r\n");
            //sb.Append("/***********************************************/\r\n\r\n");

            //sb.Append(migrationCleanupScript());
            //sb.Append("\r\n");

			sb.Append("/***********************************************/\r\n");
			sb.Append("/********* Utility Scripts         *************/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");

			if (sbMapFields.Length > 3) {
				sbMapFields.Remove(sbMapFields.Length - 3, 3);
			}
			sbMapFields.AppendLine(";");

//            // also add the updates for after dataview mappings are recreated...
//            sbMapFields.AppendLine(String.Format(@"
//
///* update the sys_table_id and sys_table_field_id AFTER the dataview mapping have been recreated... */
//
//update {0}.__old_to_new otn inner join {0}.sys_table st
//  on otn.new_table_name = st.table_name
//set
//otn.sys_table_id = st.sys_table_id;
//
//update {0}.__old_to_new otn inner join {0}.sys_table_field stf
//  on otn.sys_table_id = stf.sys_table_id
//  and otn.new_field_name = stf.field_name
//set
//otn.sys_table_field_id = stf.sys_table_field_id;
//
//", toDBName));


			sb.Append(sbMapFields.ToString());


			string sql = sb.ToString();
			return sql;

		}

		private string migrationCleanupScript() {
			string script = "";

			// clean out sys_dataview* and sys_table* tables
			script += @"
/* wipe out sys_dataview* and sys_table* tables */
/*
select concat(now(), ' wiping dataview mappings...') as Action;

truncate sys_dataview_field_lang;
truncate sys_dataview_field;
truncate sys_dataview_param;
truncate sys_dataview;
truncate sys_table_field;
truncate sys_table;
*/

";



			return script;

		}

		/// <summary>
		/// Generates script for outputting 'nice' "insert into ... select" statements
		/// </summary>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string ScriptInsertSelect(List<TableInfo> tables) {
			StringBuilder sb = new StringBuilder();
			// create all tables
			foreach (TableInfo ti in tables) {
				if (ti.IsSelected) {
					sb.Append(ti.GenerateInsertSelectScript());
					sb.Append(CommandSeparator);
					sb.Append("\r\n\r\n");
				}
			}
			string ret = sb.ToString();
			return ret;
		}

		/// <summary>
		/// Generates script for creating indexes on given tables
		/// </summary>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string ScriptIndexes(List<TableInfo> tables) {
			StringBuilder sb = new StringBuilder();
			sb.Append("/***********************************************/\r\n");
			sb.Append("/************** Index Definitions **************/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");
			foreach (TableInfo ti in tables) {
				if (ti.IsSelected) {


					// create all indexes
					if (ti.Indexes.Count == 0) {
						sb.AppendLine("/************ No index definitions exist for " + ti.TableName + " *************/\r\n");
                        if (this is OracleCreator) {
                            sb.AppendLine("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no index definitions exist for table " + ti.TableName + "') as Action from dual;");
                        } else {
                            sb.Append("select concat(now(), ' no index definitions exist for table " + ti.TableName + "') as Action;\r\n");
                        }
					} else {
						sb.AppendLine("/************ " + ti.Indexes.Count + " Index Definitions for " + ti.TableName + " *************/");
						foreach (IndexInfo ii in ti.Indexes) {
							//						sb.Append("/************ Creating index " + ii.IndexName + " for table " + ti.TableName + " *************/\r\n");
                            
                            // HACK: this should be fixed at the root, not here!!!
                            ii.SchemaName = ti.SchemaName;

                            if (this is OracleCreator) {
                                sb.AppendLine("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating index " + ii.IndexName + " for table " + ti.TableName + " ...') as Action from dual;");
                                sb.AppendLine(ii.GenerateCreateScript());
                                sb.AppendLine("/");
                                sb.AppendLine("\r\n");
                            } else {
                                sb.Append("select concat(now(), ' creating index " + ii.IndexName + " for table " + ti.TableName + " ...') as Action;\r\n");
                                sb.Append(ii.GenerateCreateScript());
                                sb.Append(CommandSeparator);
                                sb.AppendLine("\r\n");
                            }
						}
					}
				}
			}
			string ret = sb.ToString();
			return ret;
		}

		/// <summary>
		/// Generates script for creating constraints on the given tables
		/// </summary>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string ScriptConstraints(List<TableInfo> tables) {
			StringBuilder sb = new StringBuilder();
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);
				sb.Append("/***********************************************/\r\n");
				sb.Append("/*********** Constraint Definitions ************/\r\n");
				sb.Append("/***********************************************/\r\n\r\n");
                ProcessTables(tables, ti => {
                    var processed = 0;
                    foreach (var ci in ti.Constraints) {
                        // HACK: this should be fixed at the root, not here!!!
                        ci.SchemaName = ti.SchemaName;
                        if (processed == 0) {
                            sb.AppendLine("/********** " + ti.Constraints.Count + " Constraint Definitions for " + ti.TableName + " **********/");
                        }
                        if (ci.Table.IsSelected) {
                            ci.DataConnectionSpec = DataConnectionSpec;
                            if (ci is OracleConstraintInfo) {
                                sb.AppendLine("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' creating constraint " + ci.ConstraintName + " for table " + ti.TableName + " ...') as Action from dual;");
                                sb.AppendLine(ci.GenerateCreateScript());
                                sb.AppendLine("/");
                            } else {
                                sb.AppendLine("select concat(now(), ' creating constraint " + ci.ConstraintName + " for table " + ci.TableName + " ...') as Action;");
                                sb.AppendLine(ci.GenerateCreateScript() + ";\r\n");
                            }
                            sb.AppendLine();
                            processed++;
                        }
                    }
                    if (processed == 0){
                        sb.AppendLine("/************ No constraint definitions exist for " + ti.TableName + " *************/\r\n");
                        if (this is OracleCreator) {
                            sb.AppendLine("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' no constraint definitions exist for table " + ti.TableName + "') as Action from dual;");
                        } else {
                            sb.Append("select concat(now(), ' no constraint definitions exist for table " + ti.TableName + "') as Action;\r\n");
                        }
                    }
                });
			//}

			string ret = sb.ToString();
			return ret;
		}

		/// <summary>
		/// Generates script for dropping the key migration tables, which are named __(tablenamehere)
		/// </summary>
		/// <param name="schemaName"></param>
		/// <param name="tables"></param>
		/// <returns></returns>
		public string ScriptMigrationTablesDrop(string schemaName, List<TableInfo> tables) {
			StringBuilder sb = new StringBuilder();
			sb.Append("/***********************************************/\r\n");
			sb.Append("/*********** Migration Table Drops *************/\r\n");
			sb.Append("/***********************************************/\r\n\r\n");
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);
				ProcessTables(tables, ti => {
					if (ti.IsSelected) {
						ti.DataConnectionSpec = DataConnectionSpec;
                        if (this is OracleCreator) {
                            sb.AppendLine("select (to_char(current_date, 'YYYY-MM-DD HH:MI:SS') || ' dropping migration table __" + ti.TableName + " ...') as Action from dual;");
                        } else {
                            sb.AppendLine("select concat(now(), ' dropping migration table __" + ti.TableName + " ...') as Action;");
                        }
						sb.AppendLine(String.Format("drop table {0}.__{1};\r\n", schemaName, ti.TableName));
					}
				});
			//}

			string ret = sb.ToString();
			return ret;
		}

		public string GenerateIndexSelectFromTableInfo(List<TableInfo> tables, DataConnectionSpec dcs) {

			this.DataConnectionSpec = dcs;

			/*
			 *   
			 * Method='' can be Sql, PrimaryKey, or ForeignKey
			 * 
			 <Index Name='taxonomy' AverageKeywordSize='11' Encoding='utf8' FanoutSize='100' MaxSortSizeInMB='16' Separators='' StripHtml='true' Enabled='true'>
    <Sql>select * from taxonomy</Sql>
    <Fields>
    </Fields>
    <Resolvers>
      <Resolver Name='Accession' CacheEnabled='true' AverageKeywordSize='11' Encoding='utf8' FanoutSize='100' Method='Sql' ForeignKeyField=''>
        <Sql PrimaryKeyField='taxonomy_id' ResolvedPrimaryKeyField='accession_id'>
          select 
            a.taxonomy_id, 
            a.accession_id 
          from 
            accession 
          where 
            taxonomy_id in (:idlist)
        </Sql>
      </Resolver>
      <Resolver Name='Inventory' CacheEnabled='true' AverageKeywordSize='11' Encoding='utf8' FanoutSize='100' Method='Sql'>
        <Sql PrimaryKeyField='taxonomy_id' ResolvedPrimaryKeyField='inventory_id'>
          select 
            a.taxonomy_id, 
            i.inventory_id 
          from 
            inventory i inner join accession a 
              on i.accession_id = a.accession_id 
          where 
            a.taxonomy_id in (:idlist)
        </Sql>
      </Resolver>
      <Resolver Name='Orders' CacheEnabled='true' AverageKeywordSize='11' Encoding='utf8' FanoutSize='100' Method='Sql'>
        <Sql PrimaryKeyField='taxonomy_id' ResolvedPrimaryKeyField='order_request_id'>
          select
          a.taxonomy_id,
          ori.order_request_id
          from
          inventory i inner join accession a
          on i.accession_id = a.accession_id
          inner join order_request_id ori
          on ori.inventory_id = i.inventory_id
          where
          a.taxonomy_id in (:idlist)
        </Sql>
      </Resolver>
    </Resolvers>
  </Index>
*/


			StringBuilder sb = new StringBuilder();
			// generate the xml for each of the selected tables
			for (int i=0; i < tables.Count; i++) {
				TableInfo ti = tables[i];
				if (ti.IsSelected) {
					FieldInfo pk = null;
					List<FieldInfo> interestingFields = new List<FieldInfo>();
					foreach (FieldInfo fi in ti.Fields) {
						if (fi.IsPrimaryKey) {
							pk = fi;
						} else if (fi.IsForeignKey) {
							if (fi.Name.ToLower() == "accession_id" || fi.Name.ToLower() == "inventory_id" || fi.Name.ToLower() == "order_request_id") {
								// store only relevant fks and boolean fields in index's data file
								interestingFields.Add(fi);
							}
                        } else if (fi.DataType == typeof(DateTime)) {
                            // make date fields searchable
                            interestingFields.Add(fi);
                        } else if (fi.DataType == typeof(string)) {
                            // strings are searchable. (is_ fields are boolean but stored as char)
                            interestingFields.Add(fi);
                        } else if (fi.Name.ToLower() == "accession_number_part2" || fi.Name.ToLower() == "inventory_number") {
                            interestingFields.Add(fi);
                        }
					}



					string[] resolvers = new string[] { 
						"accession", "accession_id", 
						"Inventory", "inventory_id", 
						"order_request", "order_request_id" };

					// method, fkname
					string[] resolverInfo = new string[]{
						"Sql", "",
						"Sql", "",
						"Sql", ""
					};

					for (int x=1; x < resolvers.Length; x += 2) {
						if (pk.Name.ToLower() == resolvers[x].ToLower()) {
							resolverInfo[x - 1] = "PrimaryKey";
							resolverInfo[x] = "";
						}
					}


					StringBuilder sbFields = new StringBuilder();

					foreach (FieldInfo fi in interestingFields) {

                        bool isBoolField = fi.Name.ToLower().StartsWith("is_");

						sbFields.Append(String.Format(@"
		<Field Name='{0}' StoredInIndex='{1}' Searchable='{2}' Format='{3}' {4} />", 
							fi.Name, // always write out name of field
							fi.DataType == typeof(int) || isBoolField,  // store ints and boolean fields in the index
							!isBoolField && !fi.IsForeignKey, // boolean fields and foreign keys are not searchable, all other 'interesting' ones are (strings, dates, integers, doesn't matter so long as they're 'interesting')
							fi.DataType == typeof(DateTime) ? "yyyyMMdd" : "",  // format dates nicely for searching (strip off time to minimize index size)
                            isBoolField ? "IsBoolean='True' TrueValue='Y' " : "" // mark as a boolean field if needed
                            )
						);

						for (int k=1; k < resolvers.Length; k += 2) {
							if (fi.Name.ToLower() == resolvers[k].ToLower()) {
								// remember this fk as the fkname!
								resolverInfo[k-1] = "ForeignKey";
								resolverInfo[k] = fi.Name.ToLower();
								break;
							}
						}
					}

					StringBuilder sbResolvers = new StringBuilder();

					bool allResolversEnabled = true;
					for (int j=0; j < resolvers.Length; j += 2) {

						string resolverSql = "";
						if (resolverInfo[j] == "Sql") {
							resolverSql = Resolver.GenerateSql(ti.TableName, pk.Name, resolvers[j]);
						}

						if (resolverInfo[j] == "Sql" && String.IsNullOrEmpty(resolverSql)) {
							allResolversEnabled = false;
						}

						sbResolvers.Append(String.Format(@"
		<Resolver Name='{0}' CacheEnabled='{1}' AverageKeywordSize='11' Encoding='utf8' FanoutSize='100' Method='{2}' ForeignKeyField='{3}'>
			<Sql PrimaryKeyField='{4}' ResolvedPrimaryKeyField='{5}'>
				{6}
			</Sql>
		</Resolver>", resolvers[j], (resolverInfo[j] != "Sql" || !String.IsNullOrEmpty(resolverSql) ? "true" : "false") , resolverInfo[j], resolverInfo[j + 1], pk.Name, resolvers[j + 1], resolverSql));
					}
					ti.DataConnectionSpec = this.DataConnectionSpec;
					string selectStatement = ti.GenerateIndexSelectScript();

					bool indexEnabled = false;
					if (!String.IsNullOrEmpty(selectStatement) 
						&& allResolversEnabled 
						&& (ti.TableName.ToLower() == "accession"
							|| ti.TableName.ToLower() == "accession_name"
							|| ti.TableName.ToLower() == "inventory"
							|| ti.TableName.ToLower() == "inventory_maint_policy"
							|| ti.TableName.ToLower() == "order_request"
							|| ti.TableName.ToLower() == "order_request_item"
							|| ti.TableName.ToLower() == "taxonomy"
							|| ti.TableName.ToLower() == "taxonomy_genus"
							|| ti.TableName.ToLower() == "cooperator"
							)
						){
						indexEnabled = true;
					}

					sb.Append(String.Format(@"
<Index Name='{0}' AverageKeywordSize='11' Encoding='utf8' FanoutSize='100' MaxSortSizeInMB='16' Separators='' StripHtml='true' Enabled='{1}' PrimaryKeyField='{2}'>
	<StopWords></StopWords>
	<Sql>
		{3}
	</Sql>
	<Fields>{4}
	</Fields>
	<Resolvers>{5}
	</Resolvers>
</Index>
", ti.TableName, indexEnabled.ToString(), pk.Name, selectStatement, sbFields.ToString(), sbResolvers.ToString()));
				}

			}
			string ret = sb.ToString();
			return ret;
		}


		#endregion Script generation

		#region Write to file(s)
		public void SaveTableInfoToXml(string toXmlFile, List<TableInfo> tables) {
			DataSet ds = fillDataSet(tables);
			string outfile = Toolkit.ResolveFilePath(toXmlFile, true);
			ds.WriteXml(outfile, XmlWriteMode.WriteSchema);
		}
		#endregion Write to file(s)

		#region DB Schema Alteration

        public virtual string CreateDatabase(string databaseName, string instanceName) {
            DatabaseEngineUtil dbutil = DatabaseEngineUtil.CreateInstance(DataConnectionSpec.EngineName, Toolkit.ResolveFilePath("~/gguac.exe", false), instanceName);
            dbutil.ServerName = DataConnectionSpec.ServerName;
            dbutil.Port = DataConnectionSpec.Port;
            var output = dbutil.CreateDatabase(DataConnectionSpec.Password, databaseName);
            return output;
        }

        public virtual string DropDatabase(string databaseName, string instanceName) {
            DatabaseEngineUtil dbutil = DatabaseEngineUtil.CreateInstance(DataConnectionSpec.EngineName, Toolkit.ResolveFilePath("~/gguac.exe", false), instanceName);
            dbutil.ServerName = DataConnectionSpec.ServerName;
            dbutil.Port = DataConnectionSpec.Port;
            var output = dbutil.DropDatabase(DataConnectionSpec.Password, databaseName);
            return output;
        }

		/// <summary>
		/// Creates given tables (whose IsSelected = true) in the database
		/// </summary>
		/// <param name="tables"></param>
		public virtual void CreateTables(List<TableInfo> tables, string schemaName) {
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);

                //// run any initialization stuff required by the database
                //if (this is OracleCreator) {
                //    dm.Write("ALTER DATABASE CHARACTER SET UTF16");
                //}

				ProcessTables(tables, ti => {
                    try {
                        if (ti.IsSelected) {
                            ti.DataConnectionSpec = DataConnectionSpec;
                            if (!String.IsNullOrEmpty(schemaName)) {
                                ti.SchemaName = schemaName;
                            }
                            showProgress(getDisplayMember("CreateTables{progress}", "Creating table {0}...", ti.FullTableName), 0, 0);
                            ti.RunCreateScript();
                        }
                    } catch (Exception ex) {
                        // RunCreateScript always puts sql as part of the error message.
                        var inner = (ex.InnerException == null ? "" : ex.InnerException.Message);
                        throw new InvalidOperationException(getDisplayMember("CreateTables{sql}", "Error creating table {0}: {1}\n{2}", ti.FullTableName, inner, ex.Message), ex);
                    }
				});
			// }
		}

		/// <summary>
		/// Drops given tables (whose IsSelected = true) from the database
		/// </summary>
		/// <param name="tables"></param>
		public virtual void DropTables(List<TableInfo> tables) {
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);
				ProcessTables(tables, ti => {
                    try {
					    if (ti.IsSelected) {
                            ti.DataConnectionSpec = DataConnectionSpec;
						    showProgress(getDisplayMember("DropTables{progress}", "Dropping table {0}.{1}...", ti.SchemaName, ti.TableName), 0, 0);
						    ti.RunDropScript();
					    }
                    } catch (Exception ex) {
                        // RunCreateScript always puts sql as part of the error message.
                        throw new InvalidOperationException(getDisplayMember("DropTables{sql}", "Error dropping table {0}: {1}", ti.TableName, ex.Message), ex);
                    }
                });
			//}
		}

		/// <summary>
		/// Creates given indexes (whose table.IsSelected = true) in the database
		/// </summary>
		/// <param name="tables"></param>
		public virtual void CreateIndexes(List<TableInfo> tables) {

            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);
                ProcessIndexes(tables, ii => {
                    if (ii.Table.IsSelected) {
                        try {
                            ii.DataConnectionSpec = DataConnectionSpec;
                            ii.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each index creation (!!!)
							showProgress(getDisplayMember("CreateIndexes{progress}", "Creating index {0}.{1}...", ii.Table.SchemaName, ii.IndexName), 0, 0);
							ii.RunCreateScript();
						} catch (Exception exIndex) {
							showProgress(getDisplayMember("CreateIndexes{failed}", "Errored creating index '{0}.{1}': {2}.  {3}", ii.Table.SchemaName, ii.IndexName, exIndex.Message, exIndex.ToString(true)), 0, 0);
						}
					}
				});

			//}
		}

        public virtual void RebuildIndexes(List<TableInfo> tables) {
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);
                ProcessIndexes(tables, ii => {
                    if (ii.Table.IsSelected) {
                        try {
                            ii.DataConnectionSpec = DataConnectionSpec;
                            ii.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each index creation (!!!)
                            showProgress(getDisplayMember("RebuildIndexes{progress}", "Rebuilding index {0}.{1}...", ii.Table.SchemaName, ii.IndexName), 0, 0);
                            ii.RunRebuildScript();
                        } catch (Exception exIndex) {
                            showProgress(getDisplayMember("RebuildIndexes{failed}", "Errored rebuilding index '{0}.{1}: {2}.  {3}", ii.Table.SchemaName, ii.IndexName, exIndex.Message, exIndex.ToString(true)), 0, 0);
                        }
                    }
                });
            //}
        }
		
		/// <summary>
		/// Creates constraints from the given TableInfo objects in the database.  Optional only foreign (points to different table) or self-referential (points to its own table).
		/// </summary>
		/// <param name="tables"></param>
		public virtual void CreateConstraints(List<TableInfo> tables, bool createForeignConstraints, bool createSelfReferentialConstraints) {
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(dm);
                ProcessConstraints(tables, ci => {
					if (ci.Table.IsSelected) {
						try {
                            ci.DataConnectionSpec = DataConnectionSpec;
                            ci.DataConnectionSpec.CommandTimeout = 3600; // allow one hour for each constraint creation (!!!)
                            if ((ci.IsSelfReferential && createSelfReferentialConstraints)
									|| (!ci.IsSelfReferential && createForeignConstraints)) {
								showProgress(getDisplayMember("CreateConstraints{progress}", "Creating constraint {0} on table {1}...", ci.FullConstraintName, ci.FullTableName), 0, 0);
								ci.RunCreateScript();
							}
						} catch (Exception exConstraint) {
							showProgress(getDisplayMember("CreateConstraints{failed}", "Errored creating constraint '{0}': {1}.  {2}", ci.FullConstraintName, exConstraint.Message, exConstraint.ToString(true)), 0, 0);
						}
					}
				});
			//}
		}

		//protected void CreateImportKeyTables() {
		//    TableInfo ti = TableInfo.GetInstance(DataConnectionSpec);
		//    ti.TableName = "ImportKeyMap";
		//    ti.AppendField("ID", true, DbType.Int32, null, false).IsPrimaryKey = true;
		//    FieldInfo srcTableField = ti.AppendField("SourceTableField", false, DbType.String, 50, false);
		//    FieldInfo srcID = ti.AppendField("SourceID", false, DbType.String, 50, false);
		//    ti.AppendField("DestinationID", false, DbType.String, 50, false);

		//    // create an index on the SourceTableField and SourceID field
		//    // as that is how we look up *everything*
		//    IndexInfo ii = IndexInfo.GetInstance(ti);
		//    ii.Fields.Add(srcTableField);
		//    ii.Fields.Add(srcID);
		//    ii.Table = ti;
		//    ii.TableName = ti.TableName;
		//    ii.IndexName = "ndx_importkeymap_stf_sid";
		//    ti.Indexes.Add(ii);

		//    ti.RunCreateScript();
		//    ii.RunCreateScript();
		//}

		//protected void DropImportKeyTables() {
		//    TableInfo ti = TableInfo.GetInstance(DataConnectionSpec);
		//    ti.TableName = "ImportKeyMap";
		//    ti.RunDropScript();
		//}

		#endregion DB Schema Alteration

		#region Data Copying
		public void CopyDataFromDatabase(string toFolderName, string schemaName, List<TableInfo> tables) {
			string outdir = Toolkit.ResolveDirectoryPath(toFolderName, true);
			showProgress(getDisplayMember("CopyDataFromDatabase{start}", "Copying data for {0} tables...", tables.Count.ToString()), 0, 0);
			int i=0;
			ProcessTables(tables, ti => {
				if (ti.IsSelected) {
					i++;
					ti.SchemaName = schemaName;
					showProgress(getDisplayMember("CopyDataFromDatabase{progress}", "Copying data for {0}...", ti.TableName), i, tables.Count);
					ti.OnProgress += new ProgressEventHandler(ti_OnProgress);
					try {
						ti.CopyDataToDelimitedFile(outdir, null);
					} catch (Exception ex) {
						showProgress(getDisplayMember("CopyDataFromDatabase{failed}", "Error while copying to {0}: {1}. . Continuing to next table...", ti.TableName, ex.Message), i, tables.Count);
					}
				}
			});
		}




		public string CopyDataToDatabase(string fromFolderName, string schemaName, List<TableInfo> tables, bool dryRun, bool onlyRequiredData) {

			string indir = Toolkit.ResolveDirectoryPath(fromFolderName, true);

            // 2010-04-05 Brock Weaver brock@circaware.com
            // everything points at cooperator, and cooperator points at sys_lang.
            // since we plow data in before applying indexes and/or constraints, I'm removing this check
            // as an end-around the boot strapping references problem for now.


            // sort by dependencies, and make a copy of it so we don't mess up the 
            // tables = SortByDependencies(tables);

			//try {
			//    DropImportKeyTables();
			//} catch {
			//    // ignore any failure during drop (probably meant it wasn't there anyway. who cares!
			//}
			//CreateImportKeyTables();

            var sb = new StringBuilder();
            //using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
            //    DataConnectionSpec dsc = DataConnectionSpec.Clone(); //dm);
                foreach (TableInfo ti in tables) {
                    if (ti.IsSelected) {
                        ti.DataConnectionSpec = DataConnectionSpec;
                        ti.SchemaName = schemaName;
                        ti.OnProgress += new ProgressEventHandler((sender, pea) => {
                            showProgress(pea.Message, pea.ItemOffset, pea.TotalItems);
                        });
                        var script = ti.CopyDataFromFiles(indir, tables, dryRun, onlyRequiredData);
                        if (!String.IsNullOrEmpty(script)) {
                            sb.AppendLine(script + ";");
                        }
                    }
                }
            //}

                return sb.ToString();

//			DropImportKeyTables();
		}

        private string[] _systemTables = new string[]{
                "sys_lang", 
                "sys_table", 
                "sys_table_field", 
                "sys_table_filter",
                "sys_dataview", 
                "sys_dataview_field", 
                "sys_dataview_field_lang", 
                "sys_dataview_filter", 
                "sys_dataview_param", 
                "sys_index", 
                "sys_index_field", 
                "sys_permission", 
                "sys_user", 
                "sys_user_permission_mapission", 
                "sys_table_relationship"
        };

        /// <summary>
        /// Copies all data needed for basic operation of the system.  All sys_* tables, the cooperator records tied to sys_user records.
        /// </summary>
        /// <param name="toFolderName"></param>
        /// <param name="schemaName"></param>
        public void ExportSystemDataFromDatabase(string toFolderName, string schemaName, bool includeNonrequiredUsers){

            List<string> tableNames = new List<string>(_systemTables);

            List<TableInfo> tables = this.LoadTableInfo(schemaName, tableNames, false, 1);
            int i = 0;
            ProcessTables(tables, ti => {
				if (ti.IsSelected) {
					i++;
					ti.SchemaName = schemaName;
					showProgress("Exporting data for " + ti.TableName, i, tables.Count);
					ti.OnProgress += new ProgressEventHandler(ti_OnProgress);
					try {
                        switch(ti.TableName.ToLower()){
                            case "sys_user":
                                if (includeNonrequiredUsers){
                                    ti.CopyDataToDelimitedFile(toFolderName, null);
                                } else {
                                    ti.CopyDataToDelimitedFile(toFolderName, " where user_name = 'System'");
                                }
                                break;
                            case "cooperator":
                                if (includeNonrequiredUsers) {
                                    ti.CopyDataToDelimitedFile(toFolderName, " where cooperator_id in (select cooperator_id from sys_user where user_name = 'System')");
                                } else {
                                    ti.CopyDataToDelimitedFile(toFolderName, " where cooperator_id in (select distinct(cooperator_id) from sys_user)");
                                }
                                break;
                            default:
                                // pull entire table into a file
        						ti.CopyDataToDelimitedFile(toFolderName, null);
                                break;
                        }
					} catch (Exception ex) {
						showProgress("Error while copying to " + ti.TableName + ": " + ex.Message + ". Continuing to next table...", i, tables.Count);
					}
				}
			});

        }

        public void ImportSystemDataToDatabase(string fromFolderName, string schemaName) {
            List<string> tableNames = new List<string>(_systemTables);

            List<TableInfo> tables = this.LoadTableInfo(schemaName, tableNames, false, 0);
            int i = 0;
            ProcessTables(tables, ti => {
                if (ti.IsSelected) {
                    i++;
                    ti.SchemaName = schemaName;
                    showProgress("Importing data for " + ti.TableName, i, tables.Count);
                    ti.OnProgress += new ProgressEventHandler(ti_OnProgress);
                    try {

                        string path = Toolkit.ResolveFilePath(fromFolderName + Path.DirectorySeparatorChar.ToString() + ti.TableName + ".txt", false);

                        string insert = null;

                        StringBuilder columnNames = new StringBuilder();
                        DataParameters prms = new DataParameters();
                        StringBuilder prmNames = new StringBuilder();
                        foreach(List<string> row in Toolkit.ParseTabDelimited(path, true)){
                            if (columnNames.Length == 0){
                                columnNames.Append("(");
                                foreach(string c in row){
                                    columnNames.Append(c).Append(", ");
                                    prmNames.Append(":").Append(c).Append(", ");
                                    prms.Add(new DataParameter(":" + c, null));
                                }
                                columnNames.Remove(columnNames.Length-2, 2).Append(")");
                                prmNames.Remove(prmNames.Length-2, 2).Append(")");
                                insert = String.Format("insert into {0} ({1}) values ({2})", ti.TableName, columnNames, prmNames);
                            } else {

                                StringBuilder vals = new StringBuilder();
                                for(int j=0;j<prms.Count;j++){
                                    prms[j].Value = row[j];
                                }
                            }

//                            ti.DataManager.Write(insert, false, 


                        }

                    } catch (Exception ex) {
                        showProgress("Error while copying to " + ti.TableName + ": " + ex.Message + ". Continuing to next table...", i, tables.Count);
                    }
                }
            });
        }

		#endregion Data Copying



		#region Dataview Mapping Generation

		public void DeleteDataviewMappings(string dataviewName) {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				string fullyQualifiedDataviewName = (dataviewName).ToUpper();

				// remove friendly field mappings
				dm.Write(@"
delete from
	sys_dataview_field_lang
where 
	sys_dataview_field_id in (
		select sys_dataview_field from sys_dataview_field where sys_dataview_id in (
				select sys_dataview_id from sys_dataview where dataview_name = :name
		)
	)
", new DataParameters(":name", fullyQualifiedDataviewName));

				// remove dataview field mappings
				dm.Write(@"
delete from 
	sys_dataview_field 
where 
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataviewName));

				// remove dataview param mappings (if any)
				dm.Write(@"
delete from
	sys_dataview_param
where
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)", new DataParameters(":name", fullyQualifiedDataviewName));

				// remove table field mappings
				dm.Write(@"
delete from 
	sys_table_field 
where 
	sys_table_id in (
		select sys_table_id from sys_table where table_name = :name
	)", new DataParameters(":name", fullyQualifiedDataviewName));


				// remove dataview mapping
				dm.Write(@"
delete from 
	sys_dataview 
where 
	dataview_name = :name", new DataParameters(":name", fullyQualifiedDataviewName));

				// remove table mapping
				dm.Write(@"
delete from 
	sys_table 
where 
	table_name = :name", new DataParameters(":name", fullyQualifiedDataviewName));



			}
		}

        public int GetSystemCooperatorID() {
            try {
                using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                    var dt = dm.Read("select cooperator_id from cooperator where last_name = 'SYSTEM'");
                    if (dt.Rows.Count < 1) {
                        return -1;
                    } else {
                        return Toolkit.ToInt32(dt.Rows[0]["cooperator_id"], -1);
                    }
                }
            } catch (Exception ex) {
                // we get here if the database doesn't exist...
                return -1;
            }
        }

		/// <summary>
		/// Creates or updates data for the given TableInfo object in the sys_table, sys_table_field, sys_dataview, sys_dataview_field, sys_index, and sys_index_field tables.
		/// </summary>
		/// <param name="ti"></param>
		/// <param name="tables"></param>
        public void CreateTableMappings(TableInfo ti, int updatedByCooperatorID, List<TableInfo> tables) {
            string fqtn = ti.TableName.ToLower();
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {

                dm.BeginTran();

                //                // create the sys_dataview record if needed
                //                DataTable dt = dm.Read("select * from sys_dataview where dataview_name = :tablename", new DataParameters(":tablename", fqtn));
                //                int dvId = 0;
                //                if (dt.Rows.Count == 0) {
                //                    dvId = dm.Write(@"
                //insert into sys_dataview (
                //	dataview_name, 
                //	is_enabled, 
                //	is_readonly, 
                //	is_system, 
                //	is_property_suppressed, 
                //	is_user_visible, 
                //    is_transform,
                //	created_date, 
                //	created_by, 
                //	owned_date, 
                //	owned_by
                //) values(
                //	:rsname, 
                //	:enabled, 
                //	:isreadonly, 
                //	:issystem, 
                //	:ispropertysuppressed, 
                //	:isuservisible, 
                //    :istransform,
                //	:createdat, 
                //	:createdby, 
                //	:ownedat, 
                //	:ownedby
                //)
                //", true, "sys_dataview_id", new DataParameters(
                //        ":rsname", fqtn,
                //        ":enabled", "Y",
                //        ":isreadonly", "N",
                //        ":issystem", "N",
                //        ":isuservisible", "Y",
                //        ":ispropertysuppressed", "N",
                //        ":istransform", "N",
                //        ":createdat", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                //        ":createdby", 1,
                //        ":ownedat", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                //        ":ownedby", 1
                //        ));
                //                } else {
                //                    dvId = Toolkit.ToInt32(dt.Rows[0]["sys_dataview_id"], -1);
                //                }

                // create the sys_table record if needed
                var dt = dm.Read("select * from sys_table where table_name = :tablename", new DataParameters(":tablename", fqtn));
                int tblId = 0;
                if (dt.Rows.Count == 0) {
                    tblId = dm.Write(@"
insert into sys_table (
	table_name, 
	is_enabled, 
	is_readonly, 
	audits_created, 
	audits_modified, 
	audits_owned, 
	created_date, 
	created_by, 
	owned_date, 
	owned_by
) values (
	:tablename, 
	:enabled, 
	:isreadonly, 
	:auditscreated, 
	:auditsmodified, 
	:auditsowned, 
	:createddate, 
	:createdby, 
	:owneddate, 
	:ownedby
)
", true, "sys_table_id", new DataParameters(
     ":tablename", fqtn,
     ":enabled", "Y",
     ":isreadonly", "N",
     ":auditscreated", "?",
     ":auditsmodified", "?",
     ":auditsowned", "?",
     ":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
     ":createdby", updatedByCooperatorID, DbType.Int32,
     ":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
     ":ownedby", updatedByCooperatorID, DbType.Int32
    ));
                } else {
                    tblId = Toolkit.ToInt32(dt.Rows[0]["sys_table_id"], -1);
                }



                bool foundCreatedField = false;
                bool foundModifiedField = false;
                bool foundOwnedField = false;

                var fieldIDs = new List<int>();

                for(var i=0;i<ti.Fields.Count;i++){
                    var fi = ti.Fields[i];

                    // remember these for later so we can tell sys_table which audit(s) are valid for this table
                    if (fi.IsCreatedBy || fi.IsCreatedDate) {
                        foundCreatedField = true;
                    } else if (fi.IsModifiedDate || fi.IsModifiedBy) {
                        foundModifiedField = true;
                    } else if (fi.IsOwnedBy || fi.IsOwnedDate) {
                        foundOwnedField = true;
                    }

                    bool readOnly = fi.IsAutoIncrement || fi.IsCreatedDate || fi.IsCreatedBy || fi.IsOwnedDate || fi.IsOwnedBy || fi.IsModifiedDate || fi.IsModifiedBy;



                    // guiHint is used to tell the front-end what it might be an appropriate control or approach for displaying the data.
                    string guiHint = determineGuiHint(fi, readOnly, dm);

                    // the default value is what is written to the database if the field is not given from a particular dataview.
                    // here we make sure required toggle control fields (aka checkboxes) are always defaulted to "N" if they don't have a different default already
                    // note: all toggle control fields should be defined as NOT NULL in the schema, but here we make sure anyway
                    string defaultValue = null;
                    if (fi.DefaultValue != null && fi.DefaultValue != DBNull.Value) {
                        defaultValue = fi.DefaultValue.ToString();
                    } else if (guiHint == "TOGGLE_CONTROL" && !fi.IsNullable) {
                        defaultValue = "N";
                    }


                    // create the sys_table_field record if needed
                    dt = dm.Read("select * from sys_table_field where sys_table_id = :tblid and field_name = :fieldname", new DataParameters(":tblid", tblId, DbType.Int32, ":fieldname", fi.Name));
                    int tblFieldId = 0;
                    //	foreign_key_table_name, 
                    //	foreign_key_field_name, 
                    if (dt.Rows.Count == 0) {
                        tblFieldId = dm.Write(@"
insert into sys_table_field
(
	sys_table_id, 
	field_name, 
    field_ordinal,
	field_purpose, 
	field_type, 
    default_value,
	is_primary_key, 
	is_foreign_key, 
	foreign_key_dataview_name,
	is_nullable, 
	gui_hint, 
	is_readonly,
	min_length,
	max_length,
	numeric_precision,
	numeric_scale,
	is_autoincrement,
	group_name,
	created_date,
	created_by,
	owned_date,
	owned_by
)
values
(
	:tblid, 
	:fieldname, 
    :fieldordinal,
	:fieldpurpose, 
	:fieldtype, 
    :defaultvalue,
	:isprimarykey, 
	:isforeignkey, 
	:foreignkeydataviewname,
	:isnullable, 
	:guihint, 
	:isreadonly,
	:minlength,
	:maxlength,
	:numericprecision,
	:numericscale,
	:isautoincrement,
	:groupname,
	:createddate,
	:createdby,
	:owneddate,
	:ownedby
)
", true, "sys_table_field_id", new DataParameters(
     ":tblid", tblId,
     ":fieldname", fi.Name.ToLower(),
     ":fieldordinal", i, DbType.Int32,
     ":fieldpurpose", fi.Purpose,
     ":fieldtype", fi.FieldType,
     ":defaultvalue", defaultValue,
     ":isprimarykey", (fi.IsPrimaryKey ? "Y" : "N"),
     ":isforeignkey", (fi.IsForeignKey ? "Y" : "N"),
     ":foreignkeydataviewname", fi.ForeignKeyDataviewName == null ? null : fi.ForeignKeyDataviewName.ToLower(),
     ":isnullable", (fi.IsNullable ? "Y" : "N"),
     ":guihint", guiHint,
     ":isreadonly", readOnly ? "Y" : "N",
     ":minlength", fi.MinLength, DbType.Int32,
     ":maxlength", fi.MaxLength, DbType.Int32,
     ":numericprecision", fi.Precision, DbType.Int32,
     ":numericscale", fi.Scale, DbType.Int32,
     ":isautoincrement", (fi.IsAutoIncrement ? "Y" : "N"),
     ":groupname", null,
     ":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
     ":createdby", updatedByCooperatorID, DbType.Int32,
     ":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
     ":ownedby", updatedByCooperatorID, DbType.Int32
     ));
                    } else {
                        tblFieldId = Toolkit.ToInt32(dt.Rows[0]["sys_table_field_id"], -1);

                        // NOTE: we never update the following fields since they cannot be auto-gleaned from schema info:
                        //       gui_hint
                        //       group_name
                        //       foreign_key_dataview_name
                        //       default_value
                        //       is_foreign_key -- this we can glean, but the user is likely to override this for site fields, so we don't change it here

                        dm.Write(@"
update
	sys_table_field
set
	field_name = :fieldname,
    field_ordinal = :fieldordinal,
	field_purpose = :fieldpurpose,
	field_type = :fieldtype,
	is_primary_key = :isprimarykey,
/*	is_foreign_key = :isforeignkey, */
	is_nullable = :isnullable,
	is_readonly = :isreadonly,
	min_length = :minlength,
	max_length = :maxlength,
	numeric_precision = :numericprecision,
	numeric_scale = :numericscale,
	is_autoincrement = :isautoincrement,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_table_field_id = :tblfid", new DataParameters(
                                     ":fieldname", fi.Name,
                                     ":fieldordinal", i, DbType.Int32,
                                     ":fieldpurpose", fi.Purpose,
                                     ":fieldtype", fi.FieldType,
                                     ":isprimarykey", (fi.IsPrimaryKey ? "Y" : "N"),
//                                     ":isforeignkey", (fi.IsForeignKey ? "Y" : "N"),
                                     ":isnullable", (fi.IsNullable ? "Y" : "N"),
                                     ":isreadonly", readOnly ? "Y" : "N",
                                     ":minlength", fi.MinLength, DbType.Int32,
                                     ":maxlength", fi.MaxLength, DbType.Int32,
                                     ":numericprecision", fi.Precision, DbType.Int32,
                                     ":numericscale", fi.Scale, DbType.Int32,
                                     ":isautoincrement", (fi.IsAutoIncrement ? "Y" : "N"),
                                     ":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                                     ":modifiedby", updatedByCooperatorID, DbType.Int32,
                                     ":tblfid", tblFieldId, DbType.Int32));

                    }

                    fieldIDs.Add(tblFieldId);


                    if (fi.IsForeignKey) {
                        // look up the foreign key id
                        int? fkID = Toolkit.ToInt32(dm.ReadValue(@"
select 
	stf.sys_table_field_id 
from 
	sys_table_field stf
	inner join sys_table st
	on st.sys_table_id = stf.sys_table_id
where 
	st.table_name = :tablename
	and stf.field_name = :fieldname
", new DataParameters(":tablename", fi.ForeignKeyTableName == null ? null : fi.ForeignKeyTableName.ToLower(),
     ":fieldname", fi.ForeignKeyFieldName == null ? null : fi.ForeignKeyFieldName.ToLower()
     )), (int?)null);

                        // if we found it, update the current row's foreign key reference
                        if (fkID != null) {
                            dm.Write(@"
update
	sys_table_field
set
	foreign_key_table_field_id = :fkid,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_table_field_id = :tblfid
", new DataParameters(
     ":fkid", fkID, DbType.Int32,
     ":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
     ":modifiedby", updatedByCooperatorID, DbType.Int32,
     ":tblfid", tblFieldId, DbType.Int32
     ));
                        }
                    }
                }

                //                    // create the sys_dataview_field record if needed
                //                    dt = dm.Read("select * from sys_dataview_field where sys_dataview_id = :rsid and field_name = :fieldname", new DataParameters(":rsid", dvId, ":fieldname", fi.Name));
                //                    int rsFieldId = 0;
                //                    if (dt.Rows.Count == 0) {

                //                        int rsFieldCount = Toolkit.ToInt32(dm.ReadValue("select count(1) from sys_dataview_field where sys_dataview_id = :rsid", new DataParameters(":rsid", dvId)), 0);

                //                        rsFieldId = dm.Write(@"
                //insert into sys_dataview_field (
                //	sys_dataview_id, 
                //	field_name, 
                //	sys_table_field_id, 
                //	is_readonly, 
                //    is_primary_key,
                //    is_transform,
                //	sort_order, 
                //	foreign_key_dataview_name, 
                //	created_date, 
                //	created_by, 
                //	owned_date, 
                //	owned_by
                //) values (
                //	:rsid, 
                //	:fieldname, 
                //	:tblfid, 
                //	:isreadonly, 
                //    :isprimarykey,
                //    :istransform,
                //    :sortorder,
                //	:fkrn, 
                //	:createddate, 
                //	:createdby, 
                //	:owneddate, 
                //	:ownedby
                //)
                //", true, "sys_dataview_id", new DataParameters(
                //     ":rsid", dvId, 
                //     ":fieldname", fi.Name.ToLower(), 
                //     ":tblfid", tblFieldId, 
                //     ":isreadonly", (fi.IsAutoIncrement || fi.IsPrimaryKey || fi.IsCreatedBy || fi.IsCreatedDate || fi.IsModifiedBy || fi.IsModifiedDate || fi.IsOwnedBy || fi.IsOwnedDate) ? "Y" : "N",
                //     ":isprimarykey", fi.IsPrimaryKey ? "Y" : "N",
                //     ":istransform", fi.IsTransform ? "Y" : "N",
                //     ":sortorder", rsFieldCount,
                //     ":fkrn", null,
                //     ":createddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                //     ":createdby", 1,
                //     ":owneddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
                //     ":ownedby", 1
                //     ));
                //                    } else {
                //                        rsFieldId = Toolkit.ToInt32(dt.Rows[0]["sys_dataview_field_id"], -1);
                //                    }


                //                }




                // now tell sys_table which audit(s) are in use
                dm.Write(@"
update
	sys_table
set
	is_enabled = :isenabled,
	is_readonly = :isreadonly,
	audits_created = :auditscreated,
	audits_modified = :auditsmodified,
	audits_owned = :auditsowned,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_table_id = :tableid
", new DataParameters(
         ":isenabled", "Y",
         ":isreadonly", "N",
         ":auditscreated", (foundCreatedField ? "Y" : "N"),
         ":auditsmodified", (foundModifiedField ? "Y" : "N"),
         ":auditsowned", (foundOwnedField ? "Y" : "N"),
         ":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2,
         ":modifiedby", updatedByCooperatorID, DbType.Int32,
         ":tableid", tblId, DbType.Int32));


                // delete all existing mappings that don't point at valid data
                var oldFields = dm.Read(@"
select
    sys_table_field_id
from
    sys_table_field
where
    sys_table_id = :tblid
    and sys_table_field_id not in (:fldids)
", new DataParameters(":tblid", tblId, DbType.Int32, ":fldids", fieldIDs, DbPseudoType.IntegerCollection));

                foreach(DataRow drFields in oldFields.Rows){

                    var tblFieldID = Toolkit.ToInt32(drFields["sys_table_field_id"], -1);
                    // delete perm field records
                    dm.Write(@"
delete from 
    sys_permission_field
where
    sys_table_field_id in (:fldid)
", new DataParameters(":fldid", tblFieldID, DbType.Int32));

                    // clear dataview field records
                    // disassociate the sys_dataview_field record from this sys_table_field record
                    dm.Write(@"
update
    sys_dataview_field
set
    gui_hint = null,
    sys_table_field_id = null,
    modified_date = :now,
    modified_by = :who
where
    sys_table_field_id = :stfid
", new DataParameters(":now", DateTime.UtcNow, DbType.DateTime2,
":who", updatedByCooperatorID, DbType.Int32,
":stfid", tblFieldID, DbType.Int32));

                    // delete index field records
                    dm.Write(@"
delete from 
    sys_index_field
where
    sys_table_field_id in (:fldid)
", new DataParameters(":fldid", tblFieldID, DbType.Int32));


                    // disassociate the sys_index_field record from this sys_table_field record
                    dm.Write(@"
delete from
    sys_table_relationship
where
    sys_table_field_id = :stfid1
    or other_table_field_id = :stfid2
", new DataParameters(
":stfid1", tblFieldID, DbType.Int32,
":stfid2", tblFieldID, DbType.Int32));


                }

                // grab all orphaned field language data
                var dtOldFieldLang = dm.Read(@"
select
    stf.sys_table_field_id
from
    sys_table_field stf
where
    sys_table_id = :tblid
    and sys_table_field_id not in (:fldids)
", new DataParameters(":tblid", tblId, DbType.Int32, ":fldids", fieldIDs, DbPseudoType.IntegerCollection));

                foreach (DataRow drOldFieldLang in dtOldFieldLang.Rows) {
                    var oldFieldID = Toolkit.ToInt32(drOldFieldLang["sys_table_field_id"], -1);
                    if (oldFieldID > -1) {

                        // delete unneeded table field language data
                        dm.Write(@"
delete from
    sys_table_field_lang
where
    sys_table_field_id = :id
", new DataParameters(":id", oldFieldID, DbType.Int32));

                    }
                }

                // finally, delete the old sys_table_field records themselves (we already filled the oldFields datatable)
                foreach (DataRow drOldField in oldFields.Rows) {
                    // delete unneeded table field mappings
                    var oldFieldID = Toolkit.ToInt32(drOldField["sys_table_field_id"], -1);
                    if (oldFieldID > -1) {
                        dm.Write(@"
delete from
    sys_table_field
where
    sys_table_field_id = :id
", new DataParameters(":id", oldFieldID, DbType.Int32));
                    }
                }

                // now map all indx info
                createTableIndexes(ti, updatedByCooperatorID, dm);

                // now create sys_table_relationship records for constraint(s) as needed
                createTableRelationships(ti, updatedByCooperatorID, dm);

                dm.Commit();
            }
        }

        public void CreateTableIndexMappings(TableInfo ti, int updatedByCooperatorID) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                createTableIndexes(ti, updatedByCooperatorID, dm);
            }
        }

        private void createTableIndexes(TableInfo ti, int updatedByCooperatorID, DataManager dm) {

            var tblId = getTableID(ti.TableName.ToLower(), dm);

            var validIndexIds = new List<int>();

            // now map across all the index info (and index field info)
            foreach (IndexInfo ii in ti.Indexes) {

                string isUnique = String.IsNullOrEmpty(ii.IndexKind) ? "N" : ii.IndexKind.ToLower().Contains("unique") ? "Y" : "N";


                // grab the sec_index record, if any
                int indexId = Toolkit.ToInt32(dm.ReadValue("select sys_index_id from sys_index where index_name = :indexname and sys_table_id = :tableid",
                    new DataParameters(":indexname", ii.IndexName.ToLower(), DbType.String, ":tableid", tblId, DbType.Int32)), -1);


                if (indexId < 0) {
                    // need to add this index
                    indexId = dm.Write(@"
insert into
	sys_index
(sys_table_id, index_name, is_unique, created_date, created_by, owned_date, owned_by)
values
(:tableid, :indexname, :isunique, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_index_id", new DataParameters(
     ":tableid", tblId, DbType.Int32,
     ":indexname", ii.IndexName.ToLower(),
     ":isunique", isUnique,
     ":createddate", DateTime.UtcNow, DbType.DateTime2,
     ":createdby", updatedByCooperatorID, DbType.Int32,
     ":owneddate", DateTime.UtcNow, DbType.DateTime2,
     ":ownedby", updatedByCooperatorID, DbType.Int32));
                } else {
                    dm.Write(@"
update 
	sys_index
set
	sys_table_id = :tableid,
	is_unique = :isunique, 
	modified_date = :modifieddate, 
	modified_by = :modifiedby
where
	sys_index_id = :indexid
", new DataParameters(
 ":tableid", tblId, DbType.Int32,
 ":isunique", isUnique,
 ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
 ":modifiedby", updatedByCooperatorID, DbType.Int32,
 ":indexid", indexId, DbType.Int32));
                }

                validIndexIds.Add(indexId);

                // now update each field info
                // note we remember modified date up front so we can delete now-bogus rows from sys_index_field if needed.
                var validFieldIDs = new List<int>();
                for (int i = 0; i < ii.Fields.Count; i++) {
                    FieldInfo fi2 = ii.Fields[i];
                    int indexFieldId = Toolkit.ToInt32(dm.ReadValue(@"
select 
	sif.sys_index_field_id 
from 
	sys_index_field sif inner join sys_index si
		on sif.sys_index_id = si.sys_index_id
	inner join sys_table_field stf
		on sif.sys_table_field_id = stf.sys_table_field_id
where
	sif.sys_index_id = :indexid
	and stf.field_name = :fieldname
", new DataParameters(":indexid", indexId, DbType.Int32, ":fieldname", fi2.Name.ToLower())), -1);

                    if (indexFieldId < 0) {

                        var tblFieldID = Toolkit.ToInt32(dm.ReadValue(@"
select
    stf.sys_table_field_id
from
 sys_table_field stf 
where 
    stf.sys_table_id = :tableid 
    and stf.field_name = :fieldname 
", new DataParameters(
     ":tableid", tblId, DbType.Int32,
     ":fieldname", fi2.Name.ToLower(), DbType.String)), -1);

                        indexFieldId = dm.Write(@"
insert into sys_index_field 
(sys_index_id, sys_table_field_id, sort_order, created_date, created_by, owned_date, owned_by)
values 
(:indexid, :fieldid, :sortorder, :createddate, :createdby, :owneddate, :ownedby)
", true, "sys_index_field_id", new DataParameters(
     ":indexid", indexId, DbType.Int32,
     ":fieldid", tblFieldID, DbType.Int32,
     ":sortorder", i, DbType.Int32,
     ":createddate", DateTime.UtcNow, DbType.DateTime2,
     ":createdby", updatedByCooperatorID, DbType.Int32,
     ":owneddate", DateTime.UtcNow, DbType.DateTime2,
     ":ownedby", updatedByCooperatorID, DbType.Int32));

                    } else {

                        dm.Write(@"
update 
	sys_index_field
set
	sys_index_id = :indexid,
	sort_order = :sortorder,
	sys_table_field_id = (select sys_table_field_id from sys_table_field where sys_table_id = :tableid and field_name = :fieldname),
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	sys_index_field_id = :indexfieldid
", new DataParameters(
 ":indexid", indexId, DbType.Int32,
 ":sortorder", i, DbType.Int32,
 ":tableid", tblId, DbType.Int32,
 ":fieldname", fi2.Name.ToLower(),
 ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
 ":modifiedby", updatedByCooperatorID, DbType.Int32,
 ":indexfieldid", indexFieldId, DbType.Int32));
                    }

                    validFieldIDs.Add(indexFieldId);


                }

                // delete any fields that are no longer part of this index
                dm.Write("delete from sys_index_field where sys_index_id = :indexid and sys_index_field_id not in (:fieldids)",
                    new DataParameters(":indexid", indexId, DbType.Int32, ":fieldids", validFieldIDs, DbPseudoType.IntegerCollection));

            }

            // pull all indexes that no longer exist on this table, delete those fields
            var dt = dm.Read(@"
select 
    sys_index_id 
from 
    sys_index 
where 
    sys_table_id = :tableid
    and sys_index_id not in (:indexids)
", new DataParameters(
                ":tableid", tblId, DbType.Int32,
                ":indexids", validIndexIds, DbPseudoType.IntegerCollection));

            foreach(DataRow dr in dt.Rows){
                // this index is no longer valid for this table.  delete all the sys_index_field records on it...
                dm.Write("delete from sys_index_field where sys_index_id = :indexid", new DataParameters(":indexid", Toolkit.ToInt32(dr["sys_index_id"], -1), DbType.Int32));

                // and the sys_idnex record itself...
                dm.Write("delete from sys_index where sys_index_id = :indexid", new DataParameters(":indexid", Toolkit.ToInt32(dr["sys_index_id"], -1), DbType.Int32));
            }




        }


        public void CreateTableRelationshipMappings(TableInfo ti, int updatedByCooperatorID) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                createTableRelationships(ti, updatedByCooperatorID, dm);
            }
        }

        private void createTableRelationships(TableInfo ti, int updatedByCooperatorID, DataManager dm){

            var valid = new List<int>();

            // create sys_table_relationship records for constraint(s) as needed
            foreach (var ci in ti.Constraints) {

                FieldInfo source = null;
                FieldInfo dest = null;
                string relType = null;

                // NOTE: assumes all constraints are single field relationships!
                if (ci.IsSelfReferential) {

                    // refers to itself, wahoo.
                    if (ci.SourceFields.Count > 0) {
                        source = ci.SourceFields[0];
                    }
                    if (ci.ReferencesFields.Count > 0) {
                        dest = ci.ReferencesFields[0];
                    }

                    relType = "SELF";

                } else {

                    // TODO: need to determine relationship type?  Hardcoded to PARENT for now

                    // e.g.: accession.taxonomy_id   PARENT  taxonomy.taxonomy_id
                    //       so accession's PARENT is taxonomy based on taxonomy_id
                    if (ci.SourceFields.Count > 0) {
                        source = ci.SourceFields[0];
                    }
                    if (ci.ReferencesFields.Count > 0) {
                        dest = ci.ReferencesFields[0];
                    }
                    relType = "PARENT";

                }

                if (source != null && dest != null) {
                    var sourceID = getFieldID(source.TableName.ToLower(), source.Name, dm);
                    var destID = getFieldID(dest.TableName.ToLower(), dest.Name, dm);
                    if (sourceID > 0 && destID > 0) {
                        valid.Add(saveTableRelationship(sourceID, relType, destID, false, updatedByCooperatorID, dm));
                    }
                }
            }

            // valid contains all valid relationship records in sys_table_relationship.  delete all invalid ones.
            // NOTE: this does not handle OWNER_PARENT records since those cannot be inferred!

            dm.Write(@"
delete from 
    sys_table_relationship 
where
    sys_table_field_id in (
        select 
            stf.sys_table_field_id 
        from
            sys_table_field stf inner join sys_table st
            on stf.sys_table_id = st.sys_table_id
        where
            st.table_name = :tablename
    )
    and
    sys_table_relationship_id not in (:isvalid)
    and relationship_type_tag <> 'OWNER_PARENT'
", new DataParameters(
     ":tablename", ti.TableName, DbType.String,
     ":isvalid", valid.ToArray(), DbPseudoType.IntegerCollection));



        }

        private int getFieldID(string tableName, string fieldName, DataManager dm) {
            return Toolkit.ToInt32(dm.ReadValue(@"
SELECT
    stf.sys_table_field_id 
FROM
    sys_table_field stf inner join sys_table st 
        on stf.sys_table_id = st.sys_table_id
WHERE 
    st.table_name = :tbl
    and stf.field_name = :fld
", new DataParameters(":tbl", tableName, ":fld", fieldName)), -1);
        }

        private int getTableID(string tableName, DataManager dm) {
            return Toolkit.ToInt32(dm.ReadValue(@"
SELECT
    st.sys_table_id
FROM
    sys_table st 
WHERE 
    st.table_name = :tbl
", new DataParameters(":tbl", tableName)), -1);
        }

        private int saveTableRelationship(int fromFieldID, string relationshipTypeCode, int toFieldID, bool overwriteIfNeeded, int updatedByCooperatorID, DataManager dm) {

            var relationshipID = Toolkit.ToInt32(dm.ReadValue(@"
select
    sys_table_relationship_id
from
    sys_table_relationship
where
    sys_table_field_id = :fromid
    and other_table_field_id = :toid
", new DataParameters(":fromid", fromFieldID, DbType.Int32, ":toid", toFieldID, DbType.Int32)), -1);

            if (relationshipID < 0) {
                relationshipID = dm.Write(@"
insert into sys_table_relationship
(sys_table_field_id, relationship_type_tag, other_table_field_id, created_date, created_by, owned_date, owned_by)
values
(:fieldID, :typeCode, :otherFieldID, :now1, :coop1, :now2, :coop2)
", true, "sys_table_relationship_id", new DataParameters(
":fieldID", fromFieldID, DbType.Int32,
":typeCode", relationshipTypeCode,
":otherFieldID", toFieldID, DbType.Int32,
":now1", DateTime.UtcNow, DbType.DateTime2,
":coop1", updatedByCooperatorID, DbType.Int32,
":now2", DateTime.UtcNow, DbType.DateTime2,
":coop2", updatedByCooperatorID, DbType.Int32
 ));
            } else if (overwriteIfNeeded){
                dm.Write(@"
update
    sys_table_relationship
set
    sys_table_field_id = :fieldID,
    relationship_type_tag = :typeCode,
    other_table_field_id = :otherFieldID,
    modified_date = :now1,
    modified_by = :coop1
where
    sys_table_relationship_id = :relationshipID
", new DataParameters(
":fieldID", fromFieldID, DbType.Int32,
":typeCode", relationshipTypeCode,
":otherFieldID", toFieldID, DbType.Int32,
":now1", DateTime.UtcNow, DbType.DateTime2,
":coop1", updatedByCooperatorID, DbType.Int32,
":relationshipID", relationshipID, DbType.Int32
));
            }



            // 2010-06-21 Brock Weaver brock@circaware.com
            // added the functionality of updating the sys_table_field table as well so 
            // it doesn't disagree with sys_table_relationship data
            if (relationshipTypeCode == "PARENT") {

                dm.Write(@"
update
    sys_table_field
set
    foreign_key_table_field_id = :fkid,
    modified_date = :now,
    modified_by = :who
where
    sys_table_field_id = :fieldid
", new DataParameters(":fkid", toFieldID, DbType.Int32,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":who", updatedByCooperatorID, DbType.Int32,
     ":fieldid", fromFieldID, DbType.Int32
     ));

            }





            return relationshipID;

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fi"></param>
		/// <param name="readOnly"></param>
		/// <param name="dm"></param>
		/// <returns></returns>
		private string determineGuiHint(FieldInfo fi, bool readOnly, DataManager dm) {


			// TOGGLE_CONTROL - should show a checkbox (or other 2-value entry control) as it is boolean data
			// SMALL_SINGLE_SELECT_CONTROL - should show a dropdown (or other single-select entry control) as it is a foreign key lookup to a relatively small domain table (think sys_TABLE_FIELD.sys_TABLE_ID)
			// LARGE_SINGLE_SELECT_CONTROL - should show a search button or form as it is a foreign key lookup to a relatively large table (think IV.ACID)
			// DATE_CONTROL - should show a date picker (or other date entry control)
			// TEXT_CONTROL - should show a textbox (or other free-form entry control) as it is string data
			// INTEGER_CONTROL - should show a masked edit box (or other restrictive entry control) that allows only numbers
			// DECIMAL_CONTROL - should show a masked edit box (or other restrictive entry control) that allows only numbers and decimal separator
			// INTEGER_RANGE_CONTROL - should show a slider (or other predefined value entry control with increments) that allows only numbers
			// READONLY_CONTROL - should show a label (or other read-only control)

			//if (readOnly) {
			//    return "READONLY_CONTROL";
			//}

			// default to generic entry
			if (fi.DataType == typeof(Boolean) || (fi.DataType == typeof(string) && fi.MaxLength == 1)) {
				// NOTE: assumes all string fields of length 1 are toggle fields.  Is this right????
				return "TOGGLE_CONTROL";
			} else if (fi.DataType == typeof(DateTime)) {
				return "DATE_CONTROL";
			} else if (fi.DataType == typeof(string)) {
                if (fi.Name.ToLower().EndsWith("_code")) {
                    // GRIN-Global specific hack... mark all "_code" fields as dropdowns (i.e. this is a coded field, so
                    // its contents are pulled from code_value table
                    return "SMALL_SINGLE_SELECT_CONTROL";
                } else {
                    if (!fi.IsForeignKey) {
                        return "TEXT_CONTROL";
                    } else {
                        // it's a foreign key. assume lookup picker.
                        return "LARGE_SINGLE_SELECT_CONTROL";
                    }
                }
			} else if (fi.DataType == typeof(int)) {
				if (!fi.IsForeignKey) {
					// TODO: integer range check here?  ugh.
					return "INTEGER_CONTROL";
				} else {

                    // coded values always show up as dropdowns, but they're also always strings.
                    // so this should be a lookup picker since it's an int.
                    return "LARGE_SINGLE_SELECT_CONTROL";

					// it's a foreign key. determine size of the table, and 
					// determine if it's "large" or "small"
					// return determineGuiHintForForeignKey(fi.ForeignKeyTableName, 500, dm);
				}
			} else if (fi.DataType == typeof(decimal) || fi.DataType == typeof(float) || fi.DataType == typeof(double)) {
				return "DECIMAL_CONTROL";
			}

			// unknown since we got here.  just let them freeform
			return "TEXT_CONTROL";
		}

        //private string determineGuiHintForForeignKey(string tableName, int cutoff, DataManager dm) {
        //    int count = Toolkit.ToInt32(dm.ReadValue("select count(1) from " + tableName), 0);
        //    if (count > cutoff) {
        //        return "LARGE_SINGLE_SELECT_CONTROL";
        //    } else {
        //        return "SMALL_SINGLE_SELECT_CONTROL";
        //    }
        //}
		#endregion Dataview Mapping Generation

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "Creator", resourceName, null, defaultValue, substitutes);
        }


	}
}
