using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Diagnostics;
using System.IO;
using System.Data;

namespace GrinGlobal.DatabaseInspector.PostgreSql {
    public class PostgreSqlTableInfo :TableInfo {


        internal PostgreSqlTableInfo(DataConnectionSpec dcs)
            : base(dcs) {
            _delimiter = "";
        }

        protected override void makeFieldNullable(FieldInfo fi, bool nullable) {
            throw new NotImplementedException();
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
                        case "INTEGER":
                            fi.DataType = typeof(int);
                            break;
                        case "CHARACTER":
                        case "CHARACTER VARYING":
                        case "TEXT":
                            fi.DataType = typeof(string);
                            break;
                        case "DECIMAL":
                        case "NUMERIC":
                            fi.DataType = typeof(decimal);
                            break;
                        case "DATETIME":
                        case "DATETIME2":
                        case "TIMESTAMP WITHOUT TIME ZONE":
                            fi.DataType = typeof(DateTime);
                            break;
                        case "BIGINT":
                            fi.DataType = typeof(long);
                            break;
                        default:
                            throw new NotImplementedException(getDisplayMember("fillFromDatabase", "Datatype='{0}' not implemented in PostgreSqlTableInfo.fillFromDatabase.", datatype));
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

                    fi.IsAutoIncrement = dr["column_default"].ToString().ToLower().Contains("nextval(");

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
                    // now get index information.  See http://www.alberton.info/postgresql_meta_info.html
                    var dtIndex = dm.Read(@"
select
    indexname,
    indexdef
from
    pg_indexes
where
    tablename = :tableName
", new DataParameters(":tableName", TableName, DbType.String));
                    if (dtIndex != null) {
                        foreach (DataRow drIndex in dtIndex.Rows) {



                            string desc = drIndex["indexdef"].ToString().ToUpper().Trim();
                            if (!desc.Contains(TableName.ToUpper() + "_PKEY")) {
                                string indexName = drIndex["indexname"].ToString();
                                var idx = IndexInfo.GetInstance(this);
                                idx.TableName = TableName;
                                idx.IndexName = indexName;
                                if (desc.Contains("CREATE UNIQUE INDEX")) {
                                    idx.IndexKind = "UNIQUE";
                                }

                                var dtIndexFieldOrdinals = dm.Read(@"
SELECT 
    indkey as field_ordinals
FROM 
    pg_class, 
    pg_index
WHERE 
    pg_class.oid = pg_index.indexrelid
    AND pg_class.oid IN (
        SELECT 
            indexrelid
        FROM 
            pg_index, pg_class
        WHERE 
            pg_class.relname=:tableName
            AND pg_class.oid=pg_index.indrelid
--            AND indisunique != 't'
            AND indisprimary != 't'
    )
    AND relname = :indexName
", new DataParameters(":tableName", TableName, DbType.String, ":indexName", indexName, DbType.String));

                                if (dtIndexFieldOrdinals.Rows.Count > 0) {

                                    var ords = Toolkit.ToIntList(dtIndexFieldOrdinals.Rows[0]["field_ordinals"].ToString());
                                    foreach (var ord in ords) {
                                        var dtFieldName = dm.Read(@"
SELECT 
    a.attname as field_name
FROM 
    pg_index c
    INNER JOIN pg_class i
        on c.indexrelid = i.oid
    LEFT JOIN pg_class t
        ON c.indrelid  = t.oid
    LEFT JOIN pg_attribute a
        ON a.attrelid = t.oid
        AND a.attnum = ANY(indkey)
WHERE 
    t.relname = :tableName
    AND i.relname = :indexName
    AND a.attnum = :ord
", new DataParameters(":tableName", TableName, DbType.String, ":indexName", indexName, DbType.String, ":ord", ord, DbType.Int32));

                                        foreach (DataRow drFieldName in dtFieldName.Rows) {
                                            var fn = drFieldName["field_name"].ToString().ToUpper().Trim();
                                            foreach (FieldInfo fi2 in Fields) {
                                                if (fi2.Name.ToUpper().Trim() == fn) {
                                                    idx.Fields.Add(fi2);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                Indexes.Add(idx);

                            }
                            //string[] fieldNames = drIndex["index_keys"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            ////					int sequenceInIndex = Toolkit.ToInt32(drTemp["seq_in_index"].ToString(), 0);
                            //if (indexName.ToUpper().Trim().StartsWith("PK")) {
                            //    // skip.  primary keys are built as part of create table.
                            //} else {
                            //    // new index.
                            //    IndexInfo idx = IndexInfo.GetInstance(this);
                            //    idx.TableName = TableName;
                            //    idx.IndexName = indexName;
                            //    if (desc.Contains("UNIQUE")) {
                            //        idx.IndexKind = "UNIQUE";
                            //    }
                            //    idx.IndexType = null;
                            //    Indexes.Add(idx);

                            //    foreach (string fn in fieldNames) {
                            //        string fn1 = fn.ToUpper().Trim();
                            //        foreach (FieldInfo fi2 in Fields) {
                            //            string fn2 = fi2.Name.ToUpper().Trim();
                            //            if (fn2 == fn1) {
                            //                idx.Fields.Add(fi2);
                            //            }
                            //        }
                            //    }
                            //}
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

                        ConstraintInfo ci = new PostgreSqlConstraintInfo(this);
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
                                tiRef = new PostgreSqlTableInfo(DataConnectionSpec);
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
	information_schema.key_column_usage
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

        internal override string generateCreateSql() {
            PostgreSqlFieldInfo autoInc = null;
			string ret = generateCreateTableSql(ref autoInc) + ";\r\n";
			if (autoInc != null) {
                // postgresql requires sequence before table creation because of how we're making the create table (with nextval('<sequenc_name>') in primary key field)
                ret = generateCreateSequenceSql(autoInc) + ";\r\n" + ret;
			}
			return ret;

        }

        private string generateCreateTableSql(ref PostgreSqlFieldInfo autoIncrementField){
			/*
			 *create table accessoin
             * ( accession_id       integer   PRIMARY KEY DEFAULT nextval('sq_<tablename>'),
             *   accession_number_part1   varchar(50) NOT NULL,
             *   ...
             * );
         	 */

			autoIncrementField = null;
			StringBuilder sb = new StringBuilder();
			StringBuilder sbPKs = new StringBuilder();
			sb.Append("CREATE TABLE ")
				.Append(TableName)
				.AppendLine(" (");

			foreach (PostgreSqlFieldInfo fi in Fields) {
				sb.Append(fi.GenerateCreateSql());
				if (fi.IsPrimaryKey) {
					if (sbPKs.Length > 0) {
						sbPKs.Append(", ");
					}
					sbPKs.Append(fi.Name);
				}
                if (fi.IsAutoIncrement && autoIncrementField == null) {
                    // remember for later so we can create a sequence to match the primary key (if any)
                    autoIncrementField = fi;
                }
            }
            sb.Length -= 4;
            sb.Append(")");

			string ret = sb.ToString();
			return ret;

        }

        private string generateCreateSequenceSql(FieldInfo autoIncField) {

            // if we have an auto-increment field, create a sequence and a trigger as well.
            StringBuilder sb = new StringBuilder();
            // add a sequence...
            sb.Append(String.Format("CREATE SEQUENCE sq_{0} INCREMENT BY 1 MINVALUE 1 MAXVALUE {1} START WITH 1", TableName, int.MaxValue));

            string ret = sb.ToString();
            return ret;
        }


        protected override string bulkLoad(string inputDirectory, List<TableInfo> allTables, bool dryRun) {

            // prepare the file by simply removing the header line (postgresql has no provision for ignoring or skipping lines -- go figure as to why)

            string srcFile = inputDirectory + @"\" + TableName + ".txt";
            string tgtFile = inputDirectory + @"\" + TableName + ".postgresql";
            if (!File.Exists(srcFile)) {
                showProgress(getDisplayMember("bulkLoad{start}", "No data found for {0}.", TableName), 0, 0);
                return "";
            } else {
                try {
                    using (StreamReader sr = new StreamReader(new FileStream(srcFile, FileMode.Open, FileAccess.Read), Encoding.UTF8)) {
                        using (StreamWriter sw = new StreamWriter(new FileStream(tgtFile, FileMode.Create, FileAccess.Write), Encoding.UTF8)) {

                            while (!sr.EndOfStream) {

                                string[] vals = sr.ReadLine(true).Split(new char[] { '\t' }, StringSplitOptions.None);

                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < Fields.Count; i++) {
                                    FieldInfo fi = Fields[i];
                                    if (vals[i] == @"\N") {
                                        if (fi.IsNullable) {
                                            // null value, just put in a field delimiter
                                            sb.Append(",");
                                        } else {
                                            // file says it's null, but field is not nullable.
                                            // put in zero-length string
                                            sb.Append("\"\",");
                                        }
                                    } else {
                                        var temp = unescape(vals[i], '\t', String.Empty);
                                        // non-null value. quote it, put in a field delimiter
                                        sb.Append("\"").Append(temp.Replace("\"", "\"\"")).Append("\",");
                                    }
                                }


                                // chop off last ,
                                if (sb.Length > 0) {
                                    sb.Length--;
                                }

                                sw.WriteLine(sb.ToString());
                            }
                        }
                    }


                    using (DataManager dm = DataManager.Create(DataConnectionSpec)) {

                        // copy file into a table
                        string sql = @"COPY __TABLENAME__ FROM '__FILENAME__' WITH DELIMITER AS ',' NULL AS '' CSV  HEADER QUOTE AS '""' ESCAPE AS '""' "
                            .Replace("__TABLENAME__", TableName)
                            .Replace("__FILENAME__", (tgtFile).Replace(@"\", @"/"));

                        if (dryRun) {
                            var sqlFile = inputDirectory + @"\" + TableName + ".sql";
                            File.WriteAllText(sqlFile, sql + ";\r\nANALYZE " + TableName);
                        } else {
                            dm.DataConnectionSpec.CommandTimeout = 7200; // default to 2 hours before timing out. wowza.

                            dm.Write(sql);

                            // don't forget to ANALYZE to bring stats up to date
                            dm.Write("ANALYZE " + TableName);
                        }

                        return sql;
                    }
                } finally {
                    // don't forget to clean up the file
                    if (!dryRun) {
                        if (File.Exists(tgtFile)) {
                            File.Delete(tgtFile);
                        }
                    }
                }
            }
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "PostgreSqlTableInfo", resourceName, null, defaultValue, substitutes);
        }
    
    }
}
