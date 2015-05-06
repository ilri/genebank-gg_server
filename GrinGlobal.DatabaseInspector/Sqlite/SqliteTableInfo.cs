using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
//using GrinGlobal.Sql;
//using GrinGlobal.Business; 
using GrinGlobal.Interface.Dataviews;
using System.IO;

namespace GrinGlobal.DatabaseInspector.Sqlite {
	public class SqliteTableInfo : TableInfo {

        internal SqliteTableInfo(DataConnectionSpec dcs)
			: base(dcs) {
		}

		protected override string generateDropSql() {
			return "DROP TABLE " + FullTableName;
		}

		public override string ToString() {
			return base.ToString();
		}

		protected override string generateInsertSelectSql() {
			StringBuilder sb = new StringBuilder();
			sb.Append("select ");
			foreach (FieldInfo fi in Fields) {
				if (!fi.IsForeignKey) {
					sb.Append("`")
						.Append(fi.Name)
						.Append("`, ");
				}
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append(" from ").Append(FullTableName).Append(" order by 1");

			string ret = sb.ToString();
			return ret;
		}

		internal override string generateCreateSql() {

			/*
			 * CREATE TABLE `acc` (
  `ACID` int(8) unsigned NOT NULL,
  `ACP` varchar(4) character set latin1 NOT NULL,
  `ACNO` int(7) unsigned NOT NULL,
  `ACS` varchar(4) character set latin1 default NULL,
  `SITE` varchar(8) character set latin1 default NULL,
  `WHYNULL` varchar(10) character set latin1 default NULL,
  `CORE` char(1) character set latin1 default NULL,
  `BACKUP` char(1) character set latin1 default NULL,
  `LIFEFORM` varchar(10) character set latin1 default NULL,
  `ACIMPT` varchar(10) character set latin1 default NULL,
  `UNIFORM` varchar(10) character set latin1 default NULL,
  `ACFORM` char(2) character set latin1 default NULL,
  `RECEIVED` datetime NOT NULL,
  `DATEFMT` varchar(10) character set latin1 default NULL,
  `TAXNO` int(8) unsigned NOT NULL,
  `PIVOL` int(3) unsigned default NULL,
  `USERID` char(10) character set latin1 default NULL,
  `CREATED_AT` datetime default NULL,
  `MODIFIED_AT` datetime default NULL,
  `CREATED_BY` int(10) unsigned default NULL,
  `MODIFIED_BY` int(10) unsigned default NULL,
  `OWNED_BY` int(10) unsigned default NULL,
  `OWNED_AT` datetime default NULL,
  PRIMARY KEY  (`ACID`),
  UNIQUE KEY `UNIQ_ACC` (`ACP`,`ACNO`,`ACS`),
  KEY `NDX_FK_ACC_SITE` (`SITE`),
  KEY `NDX_FK_ACC_TAX` (`TAXNO`),
  KEY `NDX_FK_ACC_PI` (`PIVOL`),
  CONSTRAINT `FK_ACC_PI` FOREIGN KEY (`PIVOL`) REFERENCES `pi` (`PIVOL`),
  CONSTRAINT `FK_ACC_SITE` FOREIGN KEY (`SITE`) REFERENCES `main`.`site` (`SITE`),
  CONSTRAINT `FK_ACC_TAX` FOREIGN KEY (`TAXNO`) REFERENCES `tax` (`TAXNO`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin |			 * */

			StringBuilder sb = new StringBuilder();
			StringBuilder sbPKs = new StringBuilder();
			sb.Append("CREATE TABLE ")
				.Append(FullTableName)
				.AppendLine(" (");

			foreach (FieldInfo fi in Fields) {
				sb.Append(fi.GenerateCreateSql());
				if (fi.IsPrimaryKey) {
					if (sbPKs.Length > 0) {
						sbPKs.Append(", ");
					}
					sbPKs.Append("`")
						.Append(fi.Name)
						.Append("`");
				}
			}

			if (sbPKs.Length > 0) {
			    sb.Append("PRIMARY KEY");

			    if (!String.IsNullOrEmpty(PrimaryKeySortType)) {
			        sb.Append(" USING ").Append(PrimaryKeySortType);
			    }
			    sb.Append(" (");

			    sb.Append(sbPKs.ToString())
			        .AppendLine(")");
			} else {
				sb.Remove(sb.Length - 3, 3).AppendLine();
			}

			sb.Append(") ");

			// default engine to InnoDB if not specified
			if (String.IsNullOrEmpty(Engine)) {
				Engine = "InnoDB";
			}
			sb.Append("ENGINE=")
				.Append(Engine)
				.AppendLine(" DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci ;");

			//if (UseCaseSensitiveCollation) {
			//    sb.Append(" DEFAULT CHARSET=utf8 COLLATE=utf8_bin");
			//} else if (!String.IsNullOrEmpty(CharacterSet) || !String.IsNullOrEmpty(Collation)) {
			//    sb.Append(" DEFAULT ");
			//    if (!String.IsNullOrEmpty(CharacterSet)) {
			//        sb.Append(" CHARSET=").Append(CharacterSet);
			//    }
			//    if (!String.IsNullOrEmpty(Collation)) {
			//        sb.Append(" COLLATE=").Append(Collation);
			//    }
			//}

			string ret = sb.ToString();
			return ret;
		}


		protected override void makeFieldNullable(FieldInfo fi, bool nullable) {
			using(DataManager dm = DataManager.Create(DataConnectionSpec)){
				fi.IsNullable = nullable;
				string fieldDef = fi.GenerateCreateSql();
				dm.Write(String.Format("ALTER TABLE {0} MODIFY COLUMN {2}", FullTableName, fieldDef));
			}
		}

		protected override void beginLoading(DataManager dm, DataCommand cmd) {
			dm.BeginTran();
		}

		protected override void endLoadingFailure(DataManager dm) {
			dm.Rollback();
		}

		protected override void endLoadingSuccess(DataManager dm) {
			dm.Commit();
		}

		protected override string bulkLoad(string inputDirectory, List<TableInfo> allTables, bool dryRun) {

			showProgress(getDisplayMember("bulkLoad{start}", "Bulk inserting data for {0}...", TableName), 0, 0);
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				dm.DataConnectionSpec.CommandTimeout = 7200; // default to 2 hours before timing out. wowza.
				string filename = inputDirectory.Replace(@"\", @"/") + "/" + TableName + ".txt";
                var sql = String.Format(@"
LOAD DATA INFILE '{0}' INTO TABLE {1}
CHARACTER SET utf8
{2}
IGNORE 1 LINES 
", filename, FullTableName, Creator.INFILE_OPTIONS);

                if (dryRun) {
                    File.WriteAllText(inputDirectory + @"\" + TableName + ".sql", sql);
                } else {
                    dm.Write(sql);
                }

                return sql;
			}

		}

        protected override void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth) {

			if (tables.Exists(t => {
				return t.TableName.ToUpper().Trim() == TableName.ToUpper().Trim();
			})) {
				// this table has already been added and loaded.
				return;
			}


			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {

				// first determine options for the table itself

                DataTable dtTable = null;
                try {
                    dtTable = dm.Read(String.Format("show create table {0}", FullTableName));
                } catch (Exception ex) {
                    if (ex.Message.ToLower().Contains(" doesn't exist")) {
                        // this table isn't defined -- a FK is pointing at a non-existent table.
                        // just jump out and continue processing.
                        return;
                    }
                }

				if (dtTable.Rows.Count > 0) {
					string createTable = dtTable.Rows[0]["Create Table"].ToString();

					Regex reEngine = new Regex(@"ENGINE=([^\s]*)", RegexOptions.Multiline);
					Match m = reEngine.Match(createTable);
					if (m.Success) {
						Engine = m.Groups[1].Value;
					}
					Regex reCharset = new Regex(@"CHARSET=([^\s]*)", RegexOptions.Multiline);
					m = reCharset.Match(createTable);
					if (m.Success){
						CharacterSet = m.Groups[1].Value;
					}

					Regex reCollate = new Regex(@"COLLATE=([^\s]*)", RegexOptions.Multiline);
					m = reCollate.Match(createTable);
					if (m.Success) {
						Collation = m.Groups[1].Value;
					}
					Regex rePkType = new Regex(@"USING ([^\s]*) ", RegexOptions.Multiline);
					m = rePkType.Match(createTable);
					if (m.Success) {
						PrimaryKeySortType = m.Groups[1].Value;
					}
				}



				// now fill in the fields


				DataTable dt = dm.Read(String.Format("describe {0};", FullTableName));

				Fields = new List<FieldInfo>();

				foreach (DataRow dr in dt.Rows) {
					// create a field object based on the datarow information
					FieldInfo fi = null;

					Match m = Regex.Match(dr["Type"].ToString(), @"([^(]*)(?:\(([0-9]*),?([0-9]*)?\))?");
					int minlength = 0;
					int maxlength = 0;
					int scale = 0;
					int precision = 0;
					string fieldType = null;
					if (m.Success) {
						fieldType = m.Groups[1].Value.ToString().ToLower();
						precision = maxlength = Toolkit.ToInt32(m.Groups[2].Value.ToString(), 0);
						scale = Toolkit.ToInt32(m.Groups[3].Value.ToString(), 0);
					}

					string fieldName = dr["Field"].ToString().ToUpper();

					bool nullable = dr["Null"].ToString().ToLower().Trim() != "no";
					bool primaryKey = dr["Key"].ToString().ToLower().Trim() == "pri";

					// TODO: Mysql's describe statement does not kick out character set info!
					//       How are we going to get that for fields that have different char set
					//       than the default for its table?

					bool unsigned = dr["Type"].ToString().ToLower().Contains("unsigned");
					bool zerofill = dr["Type"].ToString().ToLower().Contains("zerofill");
					bool autoIncrement = dr["Extra"].ToString().ToLower().Contains("auto_increment");
					bool createdDate = fieldName == "CREATED" || fieldName == "CREATED_AT" || fieldName == "CREATED_DATE";
					bool createdBy = fieldName == "CREATED_BY";
					bool modifiedDate = fieldName == "MODIFIED" || fieldName == "MODIFIED_AT" || fieldName == "MODIFIED_DATE";
					bool modifiedBy = fieldName == "MODIFIED_BY";
					bool ownedDate = fieldName == "OWNED" || fieldName == "OWNED_AT" || fieldName == "OWNED_DATE";
					bool ownedBy = fieldName == "OWNED_BY";

					fi = new SqliteFieldInfo(this);

					switch (fieldType) {
						case "int":
							fi.DataType = typeof(int);
							break;
                        case "bigint":
                        case "long":
                            fi.DataType = typeof(long);
                            break;
						case "varchar":
						case "nvarchar":
							fi.DataType = typeof(string);
							break;
						case "char":
						case "nchar":
							minlength = maxlength;
							fi.DataType = typeof(string);
							break;
						case "datetime2":
						case "datetime":
							fi.DataType = typeof(DateTime);
							break;
						case "float":
							fi.DataType = typeof(float);
							break;
						case "decimal":
							fi.DataType = typeof(Decimal);
							break;
						case "double":
							fi.DataType = typeof(Double);
							break;
						default:
                            throw new InvalidOperationException(getDisplayMember("fillFromDatabase{default}", "Fields of type '{0}' are not supported.\nYou must change the type for {1}.{2} before continuing or add the code to support that type to GrinGlobal.DatabaseInspector.\nSupported types are:\nint\nvarchar\nchar\nnvarchar\nnchar\ndatetime\nfloat\ndouble\ndecimal\n", fieldType, TableName, fieldName));
					}


					fi.AddOptions("name", dr["Field"].ToString(),
							"nullable", nullable,
							"minlength", minlength,
							"maxlength", maxlength,
							"scale", scale,
							"precision", precision,
							"primarykey", primaryKey,
							"unsigned", unsigned,
							"zerofill", zerofill,
							"autoincrement", autoIncrement,
							"createdby", createdBy,
							"createdat", createdDate,
							"modifiedby", modifiedBy,
							"modifiedat", modifiedDate,
							"ownedby", ownedBy,
							"ownedat", ownedDate);

					object defaultVal = dr["default"];
					if (defaultVal != null) {
						if (defaultVal == DBNull.Value) {
							fi.AddOptions("defaultvalue", "{DBNull.Value}");
						} else if (defaultVal.GetType().ToString().ToLower() == "system.string") {
							fi.AddOptions("defaultvalue", @"""" + defaultVal + @"""");
						} else {
							fi.AddOptions("defaultvalue", defaultVal);
						}
					}


					Fields.Add(fi);
				}

				// now get index information
				DataTable dtTemp = dm.Read(String.Format("SHOW INDEX FROM {0}", FullTableName));
				string prevKeyName = null;
				IndexInfo idx = null;
				foreach (DataRow drTemp in dtTemp.Rows) {
					bool unique = drTemp["non_unique"].ToString().Trim() == "0";
					string keyName = drTemp["key_name"].ToString();
					string typeName = drTemp["index_type"].ToString();
//					int sequenceInIndex = Toolkit.ToInt32(drTemp["seq_in_index"].ToString(), 0);
					if (keyName == "PRIMARY") {
					    // skip.  primary keys are built as part of create table.
					} else {
						if (keyName != prevKeyName) {
							// new index.
							idx = IndexInfo.GetInstance(this);
							idx.TableName = TableName;
							idx.IndexName = keyName;
							if (unique){
								idx.IndexKind = "UNIQUE";
							}
							idx.IndexType = typeName;
							Indexes.Add(idx);
							prevKeyName = keyName;
						}

						FieldInfo fi = Fields.Find(fi2 => {
							return fi2.Name.ToLower() == drTemp["column_name"].ToString().ToLower();
						});

						if (fi != null) {
							idx.Fields.Add(fi);
						}
					}
				}



				// we've defined the table as well as all its fields and indexes.
				// add it to the list BEFORE resolving constraints.
				// this lets us sidestep recursion black holes due to circular referential integrity constraints
				// (aka self-referential or t1 -> t2 -> t1, or t1 -> t2 -> t3 -> t1
				if (!tables.Exists(t => {
					return this.TableName.ToUpper().Trim() == t.TableName.ToUpper().Trim();
				})) {
					tables.Add(this);
				}





				// now get constraint information
				// (this is already in the dtTable from CREATE TABLE, just parse it here)
				if (dtTable.Rows.Count > 0) {
					string createTable = dtTable.Rows[0]["Create Table"].ToString();

					//   CONSTRAINT `FK_ACC_PI` FOREIGN KEY (`PIVOL`) REFERENCES `pi` (`PIVOL`),
					//              name                     src fields           tgt   tgt fields

					Regex reConstraint = new Regex(@"CONSTRAINT[\s]*([^\s]*) FOREIGN KEY \(([^\s,]*)*\) REFERENCES ([^\s]*) \(([^\s,]*)*\)", RegexOptions.Multiline);
					MatchCollection mc = reConstraint.Matches(createTable);
					if (mc.Count > 0){
						foreach (Match m in mc) {
							ConstraintInfo ci = new SqliteConstraintInfo(this);
							ci.ConstraintName = m.Groups[1].Value.Replace("`", "").ToUpper();
							ci.TableName = this.TableName;
							ci.ReferencesTableName = m.Groups[3].Value.Replace("`", "").ToUpper();

							// determine which field(s) this foreign key points at
							TableInfo tiRef = null;
							if (this.TableName.ToUpper().Trim() == ci.ReferencesTableName.ToUpper().Trim()) {
								// self-referential.
								tiRef = this;
							} else {
								// see if it's pointing at a table we've already resolved
								foreach (TableInfo ti in tables) {
									if (ti.TableName.ToUpper().Trim() == ci.ReferencesTableName.ToUpper().Trim()) {
										tiRef = ti;
										break;
									}
								}
								// see if
							}


							if (tiRef == null) {
								// the current table references one that isn't loaded yet.
								// load it (and all of its referenced tables)
								tiRef = new SqliteTableInfo(DataConnectionSpec);
								tiRef.Fill(SchemaName, ci.ReferencesTableName, tables, fillRowCount, maxRecurseDepth-1);
								if (!tables.Exists(te => {
									return te.TableName.ToUpper().Trim() == tiRef.TableName.ToUpper().Trim();
								})) {
									tables.Add(tiRef);
								}
							}

							foreach (Capture cap2 in m.Groups[4].Captures) {
								if (cap2.Length > 0) {
									string refFieldName = cap2.Value.Replace("`", "").ToUpper();
									foreach (FieldInfo fi2 in tiRef.Fields) {
										if (fi2.Name.ToUpper().Trim() == refFieldName.ToUpper().Trim()) {
											ci.ReferencesFields.Add(fi2);
											break;
										}
									}
								}
							}

							// tell each field in our table if they're a foreign key elsewhere
							for(int i=0;i<m.Groups[2].Captures.Count;i++){
								Capture cap = m.Groups[2].Captures[i];
								if (cap.Length > 0) {
									string srcFieldName = cap.Value.Replace("`", "").ToUpper();
									foreach (FieldInfo fi in this.Fields) {
										if (fi.Name.ToUpper().Trim() == srcFieldName.ToUpper().Trim()) {
											fi.IsForeignKey = true;
											fi.ForeignKeyTableName = ci.ReferencesTableName;
                                            if (ci.ReferencesFields.Count > i) {
                                                fi.ForeignKeyFieldName = ci.ReferencesFields[i].Name;
                                            }
											ci.SourceFields.Add(fi);
											break;
										}
									}
								}
							}
							Constraints.Add(ci);
						}
					}
				}

				// fill in rowcount
				if (fillRowCount) {
					RowCount = Toolkit.ToInt32(dm.ReadValue(String.Format("select count(1) from {0}", FullTableName)), 0);
				}
			}
		}

        internal string GenerateCreateAllIndexesForTable() {
            StringBuilder sb = new StringBuilder();

            if (this.Indexes.Count > 0) {
                sb.Append("ALTER TABLE ").Append(FullTableName).Append(" ");
                foreach (IndexInfo ii in this.Indexes) {
                    string indexCreate = ii.generateCreateSql().Replace("CREATE ", " ADD ").Replace(" ON " + this.FullTableName, "");
                    sb.Append(indexCreate);
                    sb.AppendLine(", ");
                }
                //sb.Length -= 4; // 4 = last ", \r\n"
            }

            string output = sb.ToString().Trim('\r', '\n', ' ', ',');
            return output;

        }

        internal string GenerateRebuildAllIndexesForTable() {
            return "ALTER TABLE " + this.FullTableName + " ENGINE=" + this.Engine;
        }

        internal string GenerateCreateAllConstraintsForTable(bool createForeignConstraints, bool createSelfReferentialConstraints) {
            StringBuilder sb = new StringBuilder();

            if (this.Indexes.Count > 0) {
                sb.Append("ALTER TABLE ").Append(FullTableName).AppendLine(" ");
                bool foundOne = false;
                foreach (ConstraintInfo ci in this.Constraints) {
                    if ((!ci.IsSelfReferential && createForeignConstraints)
                        || (ci.IsSelfReferential && createSelfReferentialConstraints)) {
                        string constraintCreate = ci.generateCreateSql().Replace("CREATE ", " ADD ").Replace(" ON " + this.FullTableName, "").Replace("ALTER TABLE " + this.FullTableName, "");
                        sb.Append(constraintCreate);
                        sb.AppendLine(", ");
                        foundOne = true;
                    }
                }
                if (!foundOne) {
                    // no constraints found matching the given parameters. return nothing.
                    sb = new StringBuilder();
                //} else {
                //    sb.Length -= 4; // 4 = last ", \r\n"
                }
            }

            string output = sb.ToString().Trim('\r', '\n', ' ', ',');
            return output;

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "SqliteTableInfo", resourceName, null, defaultValue, substitutes);
        }

	}
}
