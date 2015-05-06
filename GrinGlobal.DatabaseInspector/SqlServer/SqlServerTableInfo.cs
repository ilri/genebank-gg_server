using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.IO;

namespace GrinGlobal.DatabaseInspector.SqlServer {
	public class SqlServerTableInfo : TableInfo {

		internal SqlServerTableInfo(DataConnectionSpec dcs)
			: base(dcs) {
		}

		protected override string generateDropSql() {
			return "DROP TABLE [" + TableName + "]";
		}

		public override string ToString() {
			return base.ToString();
		}


		public override string Delimiter {
			get {
				return "'";
			}
		}

        internal string GenerateRebuildAllIndexesForTable(){
            return "ALTER INDEX ALL ON " + this.TableName + " REBUILD ";
        }

		protected override void beginLoading(DataManager dm, DataCommand cmd) {
//			dm.BeginTran();
		}

		protected override void endLoadingFailure(DataManager dm) {
//			dm.Rollback();
		}

		protected override void endLoadingSuccess(DataManager dm) {
//			dm.Commit();
		}



		protected override string bulkLoad(string inputDirectory, List<TableInfo> allTables, bool dryRun) {

			// SQL Server is a little touchy when it comes to its input stream for BULK INSERT.
			// So we make a copy of the .txt file and tweak its contents as we go.
			// Basically, we change the mysql-specific \N char to empty string and un-escape
			// any embedded CR and LF characters in the stream.

			showProgress(getDisplayMember("bulkLoad{start}", "Preparing {0} source file for import...", TableName ), 0, 0);

			string fieldTerminator = "&}^";
			string rowTerminator = "+-}|\r\n";

			// first prepare the file by replacing \N with null values
			int linesOfData = 0;
			string srcFile = inputDirectory + @"\" + TableName + ".txt";
			string tgtFile = inputDirectory + @"\" + TableName + ".sqlserver";
            string heading = string.Empty;
			if (File.Exists(srcFile)) {
				using (StreamReader sr = new StreamReader(new FileStream(srcFile, FileMode.Open, FileAccess.Read), Encoding.UTF8)) {
					using (StreamWriter sw = new StreamWriter(new FileStream(tgtFile, FileMode.Create, FileAccess.Write), Encoding.Unicode)) {
						if (!sr.EndOfStream) {
							// first line is always column info
							// we don't need it, just eat it
							heading = sr.ReadLine(true);
						}
						while (!sr.EndOfStream) {
							string line = unescape(sr.ReadLine(true), '\t', fieldTerminator);
							sw.Write(line + rowTerminator);
							linesOfData++;
						}
					}
				}
			}

			if (linesOfData == 0) {

                if (heading != string.Empty && heading.IndexOf('\r') < 0 && heading.IndexOf('\n') > -1) {
                    // file has at least one LF but no CR's whatsoever.
                    throw new InvalidOperationException(getDisplayMember("bulkLoad{nocr}", "Input file ({0}) does not appear to be properly formatted: line endings are required to be carriage return/line feed (i.e. CRLF, aka 0x0D 0x0A)", tgtFile));
                } else {

                    showProgress(getDisplayMember("bulkLoad{nodata}", "No data found for {0}.",TableName), 0, 0);
                    if (File.Exists(tgtFile)) {
                        try {
                            File.Delete(tgtFile);
                        } catch { }
                    }
                    return "";
                }
			}

			showProgress(getDisplayMember("bulkLoad{progress}", "Bulk inserting data for {0}...", TableName), 0, 0);

            var sql = "";

			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				dm.DataConnectionSpec.CommandTimeout = 7200; // default to 2 hours before timing out. wowza.
                sql = String.Format(@"
BULK INSERT {0}
FROM '{1}'
WITH (
 FIELDTERMINATOR = '{2}',
 ROWTERMINATOR = '{3}',
 DATAFILETYPE = 'widechar',
 KEEPIDENTITY,
 KEEPNULLS
)
", TableName, tgtFile, fieldTerminator, rowTerminator);

                if (dryRun){
                    // write to a .sql file for later
                    var sqlFile = inputDirectory + @"\" + TableName + ".sql";
                    File.WriteAllText(sqlFile, sql);
                } else {
    				dm.Write(sql);
                }
			}

			// clean up the temp file after we're done with it (dry run leaves the file there so we can use it later)
            if (!dryRun) {
                try {
                    File.Delete(tgtFile);
                } catch {
                    // if deleting fails, just ignore. someone might be looking at it
                }
            }

            return sql;
		}


		protected override string generateInsertSelectSql() {
			/*
			 * 				copyTable(dest, sourceDatabaseName,
					"tblPayMethod", 
					new string[]{"PayMethodID", "PayMethod", "1 as IsActive"},
					"PaymentMethod",
					new string[]{"ID", "Name", "IsActive"}
					);
*/
			StringBuilder sb = new StringBuilder();

			sb.Append("copyTable(dest, sourceDatabaseName,\r\n\"" + TableName + "\",\r\n");
			sb.Append("new string[]{");
			foreach (FieldInfo fi in Fields) {
				if (!fi.IsForeignKey) {
					//sb.Append("[");
					sb.Append("\"" + fi.Name + "\"");
					//sb.Append("], ");
					sb.Append(", ");
				}
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append("},\r\n");
			sb.Append("\"TARGETTABLE\",\r\n");
			sb.Append("new string[]{");
			foreach (FieldInfo fi in Fields) {
				if (!fi.IsForeignKey) {
					//sb.Append("[");
					sb.Append("\"" + fi.Name + "\"");
					//sb.Append("], ");
					sb.Append(", ");
				}
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append("}\r\n);");

//			sb.Append(" from ").Append(TableName).Append(" order by 1");


			//sb.Append("select ");
			//foreach (FieldInfo fi in Fields) {
			//    if (!fi.IsForeignKey) {
			//        //sb.Append("[");
			//        sb.Append(fi.Name);
			//        //sb.Append("], ");
			//        sb.Append(", ");
			//    }
			//}
			//sb.Remove(sb.Length - 2, 2);
			//sb.Append(" from ").Append(TableName).Append(" order by 1");

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
			string tbl = ("[" + TableName + "]");
			sb.Append("CREATE TABLE ")
				.Append(tbl)
				.Append(" (\r\n");

			foreach (FieldInfo fi in Fields) {
				sb.Append(fi.GenerateCreateSql());
				if (fi.IsPrimaryKey) {
					if (sbPKs.Length > 0) {
						sbPKs.Append(", ");
					}
					sbPKs.Append("[")
						.Append(fi.Name)
						.Append("] ASC");
				}
			}

			if (sbPKs.Length > 0) {
			    sb.Append("CONSTRAINT [PK_");
			    sb.Append(TableName)
			        .Append("] PRIMARY KEY CLUSTERED (");

			    sb.Append(sbPKs.ToString())
			        .Append(")\r\n");
			} else {
				sb.Remove(sb.Length - 3, 3).Append("\r\n");
			}

			sb.Append(") ON [PRIMARY] ");

			string ret = sb.ToString();
			return ret;
		}

		protected override void makeFieldNullable(FieldInfo fi, bool nullable) {
			string tbl = "`" + TableName + "`";
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				fi.IsNullable = nullable;
				string fieldDef = fi.GenerateCreateSql();
				dm.Write("ALTER TABLE " + tbl + " MODIFY COLUMN " + fieldDef);
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

                // now fill in the fields

                DataTable dt = dm.Read("select * from information_schema.columns where table_name = '" + TableName + "' order by ORDINAL_POSITION");

                Fields = new List<FieldInfo>();

                foreach (DataRow dr in dt.Rows) {
                    // create a field object based on the datarow information
                    FieldInfo fi = FieldInfo.GetInstance(this);
                    fi.TableName = dr["table_name"].ToString();
                    fi.Name = dr["column_name"].ToString();
                    string fieldname = fi.Name.ToUpper().Trim();
                    string datatype = dr["data_type"].ToString().ToUpper().Trim();
                    //					fi.CharacterSet = dr["character_set_name"].ToString();
                    switch (datatype) {
                        case "INT":
                            fi.DataType = typeof(int);
                            break;
                        case "NVARCHAR":
                        case "NCHAR":
                        case "VARCHAR":
                        case "CHAR":
                            fi.DataType = typeof(string);
                            break;
                        case "DECIMAL":
                        case "NUMERIC":
                            fi.DataType = typeof(decimal);
                            break;
                        case "DATETIME":
                        case "DATETIME2":
                            fi.DataType = typeof(DateTime);
                            break;
                        case "BIGINT":
                            fi.DataType = typeof(long);
                            break;
                        default:
                            throw new NotImplementedException(getDisplayMember("fillFromDatabase{default}", "Datatype='{0}' not implemented in SqlServerTableInfo.fillFromDatabase.", datatype));
                    }
                    //fi.DbType = null;

                    object defaultVal = dr["column_default"];
                    if (defaultVal != null) {
                        if (defaultVal == DBNull.Value || defaultVal.ToString() == "(NULL)") {
                            fi.DefaultValue = "{DBNull.Value}";
                            //} else if (defaultVal.GetType().ToString().ToLower() == "system.string") {
                            //    fi.DefaultValue = @"""" + defaultVal.ToString().Replace("((", "").Replace("))", ""); +@"""";
                        } else {
                            fi.DefaultValue = defaultVal.ToString().Replace("((", "").Replace("))", "");
                            fi.DefaultValue = fi.DefaultValue.ToString().Replace("('", "").Replace("')", "");
                        }
                    }

                    int ret = Toolkit.ToInt32(dm.ReadValue("SELECT COLUMNPROPERTY( OBJECT_ID(:tablename),:columnname,'IsIdentity')", new DataParameters(":tablename", fi.TableName, ":columnname", fi.Name)), 0);
                    fi.IsAutoIncrement = ret == 1;

                    //fi.FieldType = null;
                    //fi.ForeignKeyFieldName = null;
                    //fi.ForeignKeyTableName = null;
                    //fi.IsAutoIncrement = null;
                    fi.IsCreatedDate = fieldname == "CREATED" || fieldname == "CREATED_AT" || fieldname == "CREATED_DATE";
                    fi.IsCreatedBy = fieldname == "CREATED_BY";
                    //fi.IsForeignKey = null;
                    fi.IsModifiedDate = fieldname == "MODIFIED" || fieldname == "MODIFIED_AT" || fieldname == "MODIFIED_DATE";
                    fi.IsModifiedBy = fieldname == "MODIFIED_BY";
                    fi.IsNullable = dr["IS_NULLABLE"].ToString().ToUpper().Trim() == "YES";
                    fi.IsOwnedDate = fieldname == "OWNED" || fieldname == "OWNED_AT" || fieldname == "OWNED_DATE";
                    fi.IsOwnedBy = fieldname == "OWNED_BY";

                    fi.IsPrimaryKey = 0 < Toolkit.ToInt32(dm.ReadValue(@"
select count(1) 
from 
	information_schema.table_constraints tc
	inner join information_schema.constraint_column_usage ccu
		on tc.constraint_name = ccu.constraint_name
where 
	tc.table_name = :tableName
	and tc.constraint_type = 'PRIMARY KEY'
	and ccu.column_name = :fieldName
", new DataParameters(":tableName", fi.TableName, ":fieldName", fi.Name)), 0);

                    //fi.IsUnsigned = null;
                    //fi.IsZeroFill = null;
                    fi.MaxLength = Toolkit.ToInt32(dr["character_maximum_length"], 0);
                    //fi.MinLength = null;
                    fi.Precision = Toolkit.ToInt32(dr["numeric_precision"], 0);
                    //fi.Purpose = null;
                    fi.Scale = Toolkit.ToInt32(dr["numeric_scale"], 0);
                    //fi.Value = null;

                    Fields.Add(fi);

                }

                if (dt.Rows.Count > 0) {
                    // now get index information
                    DataSet dsHelpInfo = new DataSet();
                    dsHelpInfo = dm.Read("sp_help [" + TableName + "]", dsHelpInfo, "schema", null);
                    DataTable dtIndex = null;
                    // index table isn't always spit out int he same order (if no idexes, no table)
                    // so we have to probe for it by column names.
                    foreach (DataTable dtHelp in dsHelpInfo.Tables) {
                        if (dtHelp.Columns.Contains("index_name") && dtHelp.Columns.Contains("index_description")) {
                            dtIndex = dtHelp;
                            break;
                        }
                    }
                    if (dtIndex != null) {
                        foreach (DataRow drIndex in dtIndex.Rows) {
                            string indexName = drIndex["index_name"].ToString();
                            string desc = drIndex["index_description"].ToString().ToUpper().Trim();
                            string[] fieldNames = drIndex["index_keys"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            //					int sequenceInIndex = Toolkit.ToInt32(drTemp["seq_in_index"].ToString(), 0);
                            if (indexName.ToUpper().Trim().StartsWith("PK")) {
                                // skip.  primary keys are built as part of create table.
                            } else {
                                // new index.
                                IndexInfo idx = IndexInfo.GetInstance(this);
                                idx.TableName = TableName;
                                idx.IndexName = indexName;
                                if (desc.Contains("UNIQUE")) {
                                    idx.IndexKind = "UNIQUE";
                                }
                                idx.IndexType = null;
                                Indexes.Add(idx);

                                foreach (string fn in fieldNames) {
                                    string fn1 = fn.ToUpper().Trim();
                                    foreach (FieldInfo fi2 in Fields) {
                                        string fn2 = fi2.Name.ToUpper().Trim();
                                        if (fn2 == fn1) {
                                            idx.Fields.Add(fi2);
                                        }
                                    }
                                }
                            }
                        }
                    }


                    // fill in rowcount
                    if (fillRowCount) {
                        RowCount = Toolkit.ToInt32(dm.ReadValue("select count(1) from [" + TableName + "]"), 0);
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


                    DataTable dtConstraints = dm.Read(@"
select 
	tc.TABLE_NAME as source_table,
	tc.CONSTRAINT_NAME as source_constraint,
	tc2.TABLE_NAME as refers_to_table,
	rc.UNIQUE_CONSTRAINT_NAME as refers_to_constraint
	
from 
	information_schema.table_constraints tc
	inner join information_schema.referential_constraints rc
		on tc.constraint_name = rc.constraint_name
	inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2
		on rc.UNIQUE_CONSTRAINT_NAME = tc2.CONSTRAINT_NAME
where 
	tc.table_name = :tableName
order by
	tc.constraint_name", new DataParameters(":tableName", TableName));


                    // pull source info
                    foreach (DataRow drC in dtConstraints.Rows) {

                        ConstraintInfo ci = new SqlServerConstraintInfo(this);
                        ci.ConstraintName = drC["source_constraint"].ToString();
                        ci.TableName = TableName;
                        ci.Table = this;
                        ci.ReferencesTableName = drC["refers_to_table"].ToString();
                        // needed for lazy evaluation
                        ci.TableList = tables;

                        // determine which field(s) this foreign key points at
                        TableInfo tiRef = null;
                        if (this.TableName.ToLower().Trim() == ci.ReferencesTableName.ToLower().Trim()) {
                            // self-referential.
                            tiRef = this;
                        } else {
                            // see if it's pointing at a table we've already resolved
                            foreach (TableInfo ti in tables) {
                                if (ti.TableName.ToLower().Trim() == ci.ReferencesTableName.ToLower().Trim()) {
                                    tiRef = ti;
                                    break;
                                }
                            }
                        }


                        if (tiRef == null) {

                            // if (maxRecurseDepth > 0) {

                            // the current table references one that isn't loaded yet.
                            // load it (and all of its referenced tables)
                            if (!tables.Exists(te => {
                                return te.TableName.ToLower().Trim() == ci.ReferencesTableName.ToLower().Trim();
                            })) {
                                tiRef = new SqlServerTableInfo(DataConnectionSpec);
                                tiRef.Fill(SchemaName, ci.ReferencesTableName, tables, fillRowCount, maxRecurseDepth - 1);
                                if (!tables.Exists(t => {
                                    return tiRef.TableName.ToUpper().Trim() == t.TableName.ToUpper().Trim();
                                })) {
                                    tables.Add(tiRef);
                                }
                            }
                            //} else {
                            //    // there's more FK relationships, but they specified not to follow them.
                            //    // we already filled in the ci.ReferencesTableName so we can do lazy evaluation on it later if need be.
                            //    // the lazy evaluation will essentially redo everything we did above, but get past this step to fill the constraint info.
                            //    Constraints.Add(ci);

                            //    continue;

                            //}
                        }

                        ci.ReferencesTable = tiRef;

                        DataTable dtRefersTo = dm.Read(@"
select
	*
from
	information_schema.constraint_column_usage
where
	constraint_name = :constraintName
order by
    column_name
", new DataParameters(":constraintName", drC["refers_to_constraint"].ToString()));

                        foreach (DataRow drRef in dtRefersTo.Rows) {
                            string refFieldName = drRef["COLUMN_NAME"].ToString().ToLower().Trim();
                            foreach (FieldInfo fiRef in tiRef.Fields) {
                                if (fiRef.Name.ToLower().Trim() == refFieldName) {
                                    ci.ReferencesFields.Add(fiRef);
                                    break;
                                }
                            }
                        }


                        DataTable dtSource = dm.Read(@"
select
	*
from
	information_schema.constraint_column_usage
where
	constraint_name = :constraintName
order by
    column_name
", new DataParameters(":constraintName", drC["source_constraint"].ToString()));

                        foreach (DataRow drSrc in dtSource.Rows) {
                            string srcFieldName = drSrc["COLUMN_NAME"].ToString().ToUpper().Trim();
                            foreach (FieldInfo fi in this.Fields) {
                                if (fi.Name.ToUpper().Trim() == srcFieldName) {
                                    ci.SourceFields.Add(fi);
                                    fi.IsForeignKey = true;
                                    fi.ForeignKeyConstraintName = ci.ConstraintName;
                                    fi.ForeignKeyTableName = ci.ReferencesTableName;
                                    fi.ForeignKeyTable = ci.ReferencesTable;
                                    if (ci.ReferencesFields.Count > 0) {
                                        fi.ForeignKeyFieldName = ci.ReferencesFields[0].Name;
                                    }
                                    break;
                                }
                            }
                        }
                        Constraints.Add(ci);
                    }
                }
            }
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "SqlServerTableInfo", resourceName, null, defaultValue, substitutes);
        }

	}
}
