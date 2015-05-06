using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.IO;
using System.Diagnostics;

namespace GrinGlobal.DatabaseInspector.Oracle {
	public class OracleTableInfo : TableInfo {

        internal OracleTableInfo(DataConnectionSpec dcs)
			: base(dcs) {
		}

		public override string Delimiter {
			get {
				return "";
			}
		}

		public override string TableName {
			get {
				return _tableName;
			}
			set {
                _tableName = value;
				//_tableName = (value == null ? null : value.ToUpper());
			}
		}

		public override string ToString() {
			return base.ToString();
		}

		public override string SchemaName {
			get {
				return _schemaName;
			}
			set {
                _schemaName = value;
				//_schemaName = (value == null ? null : value.ToUpper());
			}
		}


		protected override string generateDropSql() {
			return String.Format("DROP TABLE {0}", FullTableName);
		}

//		private const string FIELD_DELIMITER = "~|{~!";
//		private const string RECORD_DELIMITER = "!!~)}(";

		private List<string> sqlLoaderKeywordList = new List<string>(new string[] { "POSITION" });


		protected override string bulkLoad(string inputDirectory, List<TableInfo> allTables, bool dryRun) {
			// Oracle is a little touchy when it comes to loading data via SQL Loader (bulk load).
			// So we make a copy of the .txt file and tweak its contents as we go.
			// Basically, we change the mysql-specific \N char to empty string and un-escape
			// any embedded CR and LF characters in the stream.
			// Also, Oracle doesn't like escaping and expects a unique character for both field separator
			// and line ending. So we make up a relatively unlikely sequence of characters and just hope 
			// no data contains that :)

			showProgress(getDisplayMember("bulkLoad{preparing}", "Preparing {0} source file for import...", TableName), 0, 0);


//			string tgtFile = inputDirectory + @"\" + TableName + ".txt";

			string fieldTerminator = "&}^";
//			string rowTerminator = "#^%\r\n";
			string rowTerminator = "#^%";

			// first prepare the file by replacing \N with null values
			int linesOfData = 0;
			string srcFile = inputDirectory + @"\" + TableName + ".txt";
			string tgtFile = inputDirectory + @"\" + TableName + ".oracle";
			List<string> columns = new List<string>();
			if (File.Exists(srcFile)){
					// sqlldr expects UTF8 encoding for our data since it's pushed into NVARCHAR fields...
				//using (StreamReader sr = new StreamReader(new FileStream(srcFile, FileMode.Open, FileAccess.Read), Encoding.UTF8)) {
                using (StreamReader sr = new StreamReader(new FileStream(srcFile, FileMode.Open, FileAccess.Read), Encoding.UTF8)) {
					using (StreamWriter sw = new StreamWriter(new FileStream(tgtFile, FileMode.Create, FileAccess.Write), Encoding.Unicode)) {
						while (!sr.EndOfStream) {
							string line = unescape(sr.ReadLine(true), '\t', fieldTerminator);

                            string[] vals = line.Split(new string[]{fieldTerminator}, StringSplitOptions.None);

                            StringBuilder sbF = new StringBuilder();
                            for (int i = 0; i < Fields.Count; i++) {
                                FieldInfo fi = Fields[i];
                                if (vals[i] == @"\N" || vals[i].Length == 0) {
                                    // Oracle treats nulls, zero-length strings, and whitespace only strings all as NULL.
                                    // this is ok if the field is nullable, but if it's not we have to inject a single special character just
                                    // to make sure the import works.  Ye-uck.
                                    if (fi.IsNullable) {
                                        // null value, just put in a field delimiter
                                        sbF.Append(fieldTerminator);
                                    } else {
                                        // file says it's null, but field is not nullable.
                                        // just throw in a tilde for now
                                        sbF.Append("~").Append(fieldTerminator);
                                    }
                                } else {
                                    // non-null value. copy in its value and a field delimiter
                                    sbF.Append(vals[i]).Append(fieldTerminator);
                                }
                            }


                            // chop off last ,
                            if (sbF.Length > fieldTerminator.Length) {
                                sbF.Length -= fieldTerminator.Length;
                            }
                            sbF.Append(rowTerminator);
                            sw.WriteLine(sbF.ToString());

							linesOfData++;
						}
					}
				}
			}

			if (linesOfData == 0) {
			    showProgress(getDisplayMember("bulkLoad{nodata}", "No data found for {0}.", TableName), 0, 0);
				cleanUp(tgtFile);
                return "";
			}

			StringBuilder sb = new StringBuilder(String.Format(
@"OPTIONS (ROWS=1000, SKIP=1)
LOAD DATA
CHARACTERSET UTF16
LENGTH SEMANTICS CHAR
BYTEORDER LITTLE
INFILE '{0}'  ""str '{1}\r\n'""
PRESERVE BLANKS
INTO TABLE {2}
APPEND
TRAILING NULLCOLS
(
", tgtFile.Replace("\\", "\\\\"), rowTerminator, FullTableName));

			// append field names
			for (int i=0; i < Fields.Count; i++) {
				FieldInfo fi = Fields[i];
                string fieldName = fi.Name.ToUpper();
				if (sqlLoaderKeywordList.Contains(fieldName)) {
					fieldName = @"""" + fieldName + @"""";
				}
				sb.Append(fieldName);
				string term = (i < Fields.Count - 1 ? fieldTerminator : rowTerminator).Replace(@"\t", @"\\t").Replace("\r", "\\r").Replace("\n", "\\n");
				string defaultIf = (fi.IsNullable ? "NULLIF" : "DEFAULTIF");

                const int MAX_CLOB_SIZE = 32767;  // HACK:  32K ... the max the text control can display in CT .... really huge values causes out of memory problems in sqlldr, change with care.
                int fieldImportLength = fi.MaxLength;

				if (fi.DataType == typeof(DateTime)) {
					sb.Append(String.Format(@" DATE ""YYYY-MM-DD HH24:MI:SS"" TERMINATED BY ""{0}"" {1} ({2} = BLANKS)", term, defaultIf, fieldName));
				} else if (fi.DataType == typeof(string)) {
                    if (fi.MaxLength < 0 || fi.MaxLength > 2000) {

                        fieldImportLength = MAX_CLOB_SIZE; // HACK : see MAX_CLOB_SIZE definition

                        // character large object (CLOB)
                        // set max length to 2 GB
                        // CLOB fields don't let you specify sql expression (i.e. the COALESCE(:{1}... we use normally)
                        //   so we just plow in whatever is there. 
                        if (fi.IsNullable) {
                            sb.Append(String.Format(@" CHAR ({2}) TERMINATED BY ""{0}"" NULLIF ({1} = BLANKS)", term, fieldName, fieldImportLength));
                        } else {
                            sb.Append(String.Format(@" CHAR ({2}) TERMINATED BY ""{0}"" ", term, fieldName.Replace(@"""", @"\"""), fieldImportLength));
                        }
                    } else {
                        if (fi.IsNullable) {
                            sb.Append(String.Format(@" CHAR ({2}) TERMINATED BY ""{0}"" NULLIF ({1} = BLANKS)", term, fieldName, fieldImportLength));
                        } else {
                            sb.Append(String.Format(@" CHAR ({2}) TERMINATED BY ""{0}"" ""COALESCE(:{1}, ' ')""", term, fieldName.Replace(@"""", @"\"""), fieldImportLength));
                        }
                    }
				} else if (fi.DataType == typeof(int)) {
					sb.Append(String.Format(@" INTEGER EXTERNAL TERMINATED BY ""{0}"" {1} ({2} = BLANKS)", term, defaultIf, fieldName));
				} else if (fi.DataType == typeof(float) || fi.DataType == typeof(double) ){
					sb.Append(String.Format(@" FLOAT EXTERNAL TERMINATED BY ""{0}"" {1} ({2} = BLANKS)", term, defaultIf, fieldName));
				} else if (fi.DataType == typeof(decimal)) {
					sb.Append(String.Format(@" DECIMAL EXTERNAL ({3}) TERMINATED BY ""{0}"" {1} ({2} = BLANKS)", term, defaultIf, fieldName, fi.Precision));
				} else {
					sb.Append("Have no idea what kind of field this is! -> " + fieldName + " ... " + fi.GenerateCreateSql());
				}

				if (i < Fields.Count - 1) {
					sb.Append(",\r\n");
				}

			}
			sb.Append("\r\n)\r\n");

			string ctlFileContents = sb.ToString();

			string ctlFileName = tgtFile + ".ctl";
			string logFileName = tgtFile + ".log";
			string badFileName = tgtFile + ".bad";
            string outFileName = tgtFile + ".out";
            string errFileName = tgtFile + ".err";

			// delete any existing temp files (just in case previous run, if any, left remnants)
			cleanUp(ctlFileName);
			cleanUp(logFileName);
			cleanUp(badFileName);
            cleanUp(outFileName);
            cleanUp(errFileName);


			// Create the control file
			File.WriteAllText(ctlFileName, ctlFileContents);

			// we use DataManager here just to get login info
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				// run sql loader, passing appropriate values...
				string args = String.Format("userid={0}/{1} control={2} data={3} log={4} bad={5}",
                    dm.DataConnectionSpec.UserName.Replace("'", ""), dm.DataConnectionSpec.Password.Replace("'", ""), ctlFileName, tgtFile, logFileName, badFileName);

                using (_outputStream = File.AppendText(outFileName)) {
                    using (_errorStream = File.AppendText(errFileName)) {
                        showProgress(getDisplayMember("bulkLoad{start}", "Bulk inserting data for {0}...", TableName), 0, 0);

                        Process p = new Process();
                        p.StartInfo.FileName = "sqlldr.exe";
                        p.StartInfo.Arguments = args;
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardError = true;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.StartInfo.WorkingDirectory = Directory.GetParent(tgtFile).FullName;
                        p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
                        p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                        p.Start();
                        p.BeginOutputReadLine();
                        p.BeginErrorReadLine();
                        p.WaitForExit();
                    }
                }

				// inspect output and error streams, see if
				// everything worked.
				// also, if no bad file exists we can assume everything went ok.
				if (File.Exists(badFileName)) {
                    throw new InvalidOperationException(getDisplayMember("bulkLoad{badfile}", "Import failed for table {0}. See logfile='{1}' and badfile='{2}' and errfile='{3}' for details.\nCommandLine=sqlldr.exe {4}\n", TableName, logFileName, badFileName, errFileName, args));
				}
			}

			// clean up all the temp files
            cleanUp(errFileName);
            cleanUp(outFileName);
			cleanUp(logFileName);
			cleanUp(badFileName);
			cleanUp(ctlFileName);
			cleanUp(tgtFile);
            return sb.ToString();
		}

		private void cleanUp(string fileName) {
			if (File.Exists(fileName)) {
				try {
					File.Delete(fileName);
				} catch { 
					// eat all errors, as someone may be looking at it and causes us to blow
				}
			}
		}

        StreamWriter _outputStream;
        void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            _outputStream.WriteLine(e.Data);
		}

        StreamWriter _errorStream;
		void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            _errorStream.WriteLine(e.Data);
		}


		protected override string generateInsertSelectSql() {
			StringBuilder sb = new StringBuilder();
			sb.Append("select ");
			foreach (FieldInfo fi in Fields) {
				if (!fi.IsForeignKey) {
					sb.Append("\"")
						.Append(fi.Name)
						.Append("\", ");
				}
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append(" from ").Append(TableName).Append(" order by 1");

			string ret = sb.ToString();
			return ret;
		}

		public override void RunDropScript() {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {

				string triggerSql = generateDropTriggerSql();
				try {
					dm.Write(triggerSql);
				} catch {
					// eat trigger drop failures
				}

				string sequenceSql = generateDropSequenceSql();
				try {
					dm.Write(sequenceSql);
				} catch {
					// eat sequence drop failures
				}


				string tableSql = generateDropSql();
				try {
					dm.Write(tableSql);
				} catch {
					// eat table drop failures
				}

			}
		}

		//*
		public override void RunCreateScript() {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {

                FieldInfo autoInc = null;
                string tableSql = generateCreateTableSql(ref autoInc);
                dm.Write(tableSql + ";");
                if (autoInc != null) {
                    string sequenceSql = generateCreateSequenceSql(autoInc);
                    dm.Write(sequenceSql + ";");
                    string triggerSql = generateCreateTriggerSql(autoInc);
                    dm.Write(triggerSql + ";;"); // don't know why, just know we have to
                }
			}
		}
		//*/

		private string generateCreateTableSql(ref FieldInfo autoIncrementField ) {
			/*
			 * 
  CREATE TABLE PROD.BROCKTEST1
   (	ID NUMBER(10,0) NOT NULL ENABLE, 
	BROCKTEST2_ID NUMBER(10,0), 
	DESCRIPTION VARCHAR2(4000), 
	 CONSTRAINT BROCKTEST1_PK PRIMARY KEY (ID)
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE USERS ENABLE, 
	 CONSTRAINT BROCKTEST1_CON FOREIGN KEY (BROCKTEST2_ID)
	  REFERENCES PROD.BROCKTEST2 (ID) ENABLE
   ) PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT)
  TABLESPACE USERS
 	 * */

			autoIncrementField = null;
			StringBuilder sb = new StringBuilder();
			StringBuilder sbPKs = new StringBuilder();
			sb.Append("CREATE TABLE ")
				.Append(FullTableName)
				.Append(" (\r\n");

			foreach (FieldInfo fi in Fields) {
				sb.Append(fi.GenerateCreateSql());
				if (fi.IsPrimaryKey) {
					if (sbPKs.Length > 0) {
						sbPKs.Append(", ");
					}
					sbPKs.Append(fi.Name);
					if (fi.IsAutoIncrement && autoIncrementField == null) {
						// remember for later so we can create a sequence AND a trigger
						autoIncrementField = fi;
					}
				}
			}

			// create the primary key constraint if needed
			if (sbPKs.Length > 0) {
			    sb.Append("CONSTRAINT PK_")
                    .Append(TableName.ToUpper())
			        .Append(" PRIMARY KEY (")
			        .Append(sbPKs.ToString())
			        .Append(")\r\nUSING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255)");
			} else {
				if (sb.Length > 3) {
					sb.Remove(sb.Length - 3, 3);
				}
				sb.Append(")");
			}

			string ret = sb.ToString();
			return ret;

		}

		protected override void beginLoading(DataManager dm, DataCommand cmd) {
			dm.Write(String.Format("ALTER TABLE {0} NOLOGGING", FullTableName));
			cmd.ProcOrSql.Replace("insert ", "insert /*+ append */ ");
		}

		protected override void endLoadingFailure(DataManager dm) {
			dm.Write(String.Format("ALTER TABLE {0} LOGGING", FullTableName));
		}

		protected override void endLoadingSuccess(DataManager dm) {
			dm.Write(String.Format("ALTER TABLE {0} LOGGING", FullTableName));
		}

		private string generateDropSequenceSql() {
            return String.Format("DROP SEQUENCE {0}.SQ_{1}", SchemaName.ToUpper(), TableName.ToUpper());
		}

		private string generateDropTriggerSql() {
            return String.Format("DROP TRIGGER {0}.TG_{1}", SchemaName.ToUpper(), TableName.ToUpper());
		}

		private string generateCreateSequenceSql(FieldInfo autoIncField){

			// if we have an auto-increment field, create a sequence and a trigger as well.
			StringBuilder sb = new StringBuilder();
			// add a sequence...
            sb.Append(String.Format("CREATE SEQUENCE {0}.SQ_{1} MINVALUE 1 START WITH 1 INCREMENT BY 1 NOCACHE", SchemaName.ToUpper(), TableName.ToUpper()));

			string ret = sb.ToString();
			return ret;
		}

		private string generateCreateTriggerSql(FieldInfo autoIncField) {
			// and the trigger...
			StringBuilder sb = new StringBuilder();
			sb.Append(String.Format(
@"CREATE OR REPLACE TRIGGER {0}.TG_{1} BEFORE INSERT ON {0}.{1} FOR EACH ROW BEGIN IF :NEW.{2} IS NULL THEN SELECT {0}.SQ_{1}.NEXTVAL INTO :NEW.{2} FROM DUAL; END IF; END", 
				SchemaName.ToUpper(), TableName.ToUpper(), autoIncField.Name));

			string ret = sb.ToString();
			return ret;
		}


        protected override string generateMapCreateSql() {
            return String.Format(@"CREATE TABLE {0}.__{1} (
new_id INTEGER NOT NULL,
previous_id INTEGER NOT NULL,
table_name varchar2(30) NULL,
CONSTRAINT pk__{1} PRIMARY KEY (new_id)
USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255);
CREATE SEQUENCE {0}.s2_{1} minvalue 1 start with 1 increment by 1;
CREATE OR REPLACE TRIGGER {0}.t2_{1} BEFORE INSERT ON {0}.{1} FOR EACH ROW BEGIN IF :NEW.{1}_id IS NULL THEN SELECT GRINGLOBAL.s2_{1}.NEXTVAL INTO :NEW.{1}_id FROM DUAL; END IF; END
/

CREATE INDEX {0}.m2_{1} ON {0}.__{1} (previous_id, table_name)
/
", SchemaName.ToUpper(), TableName);
        }


		//*
		internal override string generateCreateSql() {
			FieldInfo autoInc = null;
			string ret = generateCreateTableSql(ref autoInc);
			if (autoInc != null) {
				ret += ";\r\n" + generateCreateSequenceSql(autoInc);
				ret += ";\r\n" + generateCreateTriggerSql(autoInc);
                ret += ";\r\n";
			}
            //EventLog.WriteEntry("OracleTableInfo", ret, EventLogEntryType.Information);
			return ret;
		}

		protected override void makeFieldNullable(FieldInfo fi, bool nullable) {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				fi.IsNullable = nullable;
				string fieldDef = fi.GenerateCreateSql();
				dm.Write(String.Format("ALTER TABLE {0} MODIFY COLUMN {1}", FullTableName, fieldDef));
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


				// load column info

				DataTable dtCols = dm.Read(@"
SELECT 
	* 
FROM 
	ALL_TAB_COLUMNS 
WHERE 
	OWNER = :owner 
	AND TABLE_NAME = :tablename
ORDER BY 
	COLUMN_ID
",
					new DataParameters(":owner", SchemaName.ToUpper(), ":tablename", this.TableName.ToUpper()));
				foreach (DataRow drCol in dtCols.Rows) {

					string columnName = drCol["COLUMN_NAME"].ToString();
					string dataType = drCol["DATA_TYPE"].ToString();
					int length = Toolkit.ToInt32(drCol["DATA_LENGTH"], 0);
					int? precision = Toolkit.ToInt32(drCol["DATA_PRECISION"], null);
					int? scale = Toolkit.ToInt32(drCol["DATA_SCALE"], null);
					bool nullable = drCol["NULLABLE"].ToString() == "Y";
                    var defaultValue = drCol["DATA_DEFAULT"];
                    switch (dataType.ToUpper()) {
                        case "VARCHAR":
                        case "VARCHAR2":
                        case "CHAR":
                        case "CLOB":
                            // for string fields, we should use the char_length value instead of data_length
                            // because data_length measures bytes instead of chars (unicode is typically 2 bytes / char)
                            length = Toolkit.ToInt32(drCol["CHAR_LENGTH"], 0);
                            break;
                    }

					FieldInfo fi = FieldInfo.GetInstance(this);
					fi.Name = columnName;
                    fi.TableName = this.TableName;

                    fi.IsPrimaryKey = 0 < Toolkit.ToInt32(dm.ReadValue(@"
SELECT 
	COUNT(1) as COUNT
FROM 
	ALL_CONSTRAINTS AC INNER JOIN ALL_CONS_COLUMNS ACC
          ON AC.CONSTRAINT_NAME = ACC.CONSTRAINT_NAME
WHERE
	AC.TABLE_NAME = :tableName AND AC.CONSTRAINT_TYPE = 'P'
       AND ACC.COLUMN_NAME = :fieldName
", new DataParameters(":tableName", fi.TableName.ToUpper(), ":fieldName", fi.Name.ToUpper())), 0);

                    // NOTE: assumes existence of a sequence for this table implies the primary key is an auto increment
                    if (fi.IsPrimaryKey) {
                        fi.IsAutoIncrement = 0 < Toolkit.ToInt32(dm.ReadValue(@"
SELECT
    COUNT(1) AS COUNT
FROM
    ALL_SEQUENCES
WHERE
    SEQUENCE_NAME = :seqName
", new DataParameters(":seqName", "SQ_" + fi.TableName.ToUpper())), 0);
                    }

					fi.Precision = (precision == null ? 0 : (int)precision);
					fi.Scale = (scale == null ? 0 : (int)scale);
                    fi.DefaultValue = defaultValue;
					fi.MaxLength = length;
					fi.IsNullable = nullable;
					fi.IsCreatedDate = columnName == "CREATED_DATE";
					fi.IsCreatedBy = columnName == "CREATED_BY";
					fi.IsModifiedDate = columnName == "MODIFIED_DATE";
					fi.IsModifiedBy = columnName == "MODIFIED_BY";
					fi.IsOwnedBy = columnName == "OWNED_BY";
					fi.IsOwnedDate = columnName == "OWNED_DATE";
					switch (dataType) {
						case "DATE":
							fi.DataType = typeof(DateTime);
                            fi.MaxLength = 0;
                            fi.MinLength = 0;
							break;
						case "VARCHAR2":
						case "CHAR":
							fi.DataType = typeof(string);
							break;
                        case "CLOB":
                            fi.MaxLength = -1;
                            fi.DataType = typeof(string);
                            break;
						case "NUMBER":
                            if (precision == null) {
                                if (scale == null) {
                                    // assume a built-in precision and scale
                                    fi.DataType = typeof(float);
                                } else {
                                    // assume an integer, as we have scale but no precision
                                    fi.DataType = typeof(int);
                                    fi.Precision = 10;
                                    fi.Scale = (int)scale;
                                    fi.MaxLength = 0;
                                }
                            } else {
                                if (scale == null) {
                                    // assume a whole number, as we have precision but no scale
                                    fi.Precision = (int)precision;
                                    fi.Scale = 0;
                                    fi.MaxLength = 0;
                                    if (fi.Precision > 10) {
                                        fi.DataType = typeof(decimal);
                                    } else {
                                        fi.DataType = typeof(int);
                                    }
                                } else {
                                    if (scale == 0) {
                                        // assume an integer
                                        fi.Precision = (int)precision;
                                        fi.Scale = 0;
                                        fi.MaxLength = 0;
                                        if (fi.Precision > 10) {
                                            fi.DataType = typeof(decimal);
                                        } else {
                                            fi.DataType = typeof(int);
                                        }
                                    } else {
                                        // assume a decimal
                                        fi.DataType = typeof(decimal);
                                        fi.Precision = (int)precision;
                                        fi.Scale = (int)scale;
                                        fi.MaxLength = 0;
                                        fi.MinLength = 0;
                                    }
                                }
                            }
								// throw new InvalidOperationException("precision/scale=" + precision + "/" + scale + " not interpretted for oracle type of 'NUMBER'");
							break;
                        default:
                            throw new NotImplementedException(getDisplayMember("fillFromDatabase", "Datatype='{0}' not implemented in OracleTableInfo.fillFromDatabase.", dataType));
                    }
					this.Fields.Add(fi);
				}


				// load index info

                var dtIndexes = dm.Read(@"
SELECT 
	* 
FROM 
	ALL_INDEXES
WHERE
	OWNER = :owner 
	AND TABLE_NAME = :tablename  
ORDER BY 
	INDEX_NAME",
                    new DataParameters(":owner", SchemaName.ToUpper(), ":tablename", this.TableName.ToUpper()));

                foreach (DataRow drIndex in dtIndexes.Rows) {

                    var indexName = drIndex["INDEX_NAME"].ToString();
                    var tableName = drIndex["TABLE_NAME"].ToString();
                    var isUnique = drIndex["UNIQUENESS"].ToString() == "UNIQUE";

                    if (!indexName.ToUpper().StartsWith("PK")) {

                        var ii = IndexInfo.GetInstance(this);
                        ii.TableName = this.TableName;
                        ii.IndexName = indexName;
                        if (isUnique){
                            ii.IndexKind = "UNIQUE";
                        }

                        var dtIndexFields = dm.Read(@"
SELECT 
	* 
FROM 
	ALL_IND_COLUMNS 
WHERE
	INDEX_OWNER = :owner 
	AND INDEX_NAME = :indexname
ORDER BY 
	COLUMN_POSITION",
                        new DataParameters(":owner", SchemaName.ToUpper(), ":indexname", indexName.ToUpper()));

                        foreach (DataRow drIndexField in dtIndexFields.Rows) {

                            FieldInfo fiIndex = Fields.Find(f => {
                                return f.Name.ToUpper() == drIndexField["COLUMN_NAME"].ToString().ToUpper();
                            });
                            if (fiIndex != null) {
                                ii.Fields.Add(fiIndex);
                            }
                        }

                        Indexes.Add(ii);

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

                // load constraint info
                var dtConstraints = dm.Read(@"
SELECT
    *
FROM
    ALL_CONSTRAINTS
WHERE
    OWNER = :owner
    AND CONSTRAINT_TYPE = 'R'
    AND TABLE_NAME = :tablename
ORDER BY
    CONSTRAINT_NAME
",
                        new DataParameters(":owner", SchemaName.ToUpper(), ":tablename", this.TableName.ToUpper()));

                foreach (DataRow drConstraint in dtConstraints.Rows) {
                    var ci = ConstraintInfo.GetInstance(this);
                    ci.ConstraintName = drConstraint["CONSTRAINT_NAME"].ToString();
                    ci.TableName = this.TableName;
                    ci.Table = this;
                    ci.TableList = tables;

                    var refersConstraintName = drConstraint["R_CONSTRAINT_NAME"].ToString();

                    var dtRefTable = dm.Read(@"
SELECT
    *
FROM
    ALL_CONSTRAINTS
WHERE
    OWNER = :owner
    AND CONSTRAINT_NAME = :referConstraintName
ORDER BY
    CONSTRAINT_NAME
",
                        new DataParameters(":owner", SchemaName.ToUpper(), ":referConstraintName", refersConstraintName.ToUpper()));

                    if (dtRefTable.Rows.Count > 0) {
                        ci.ReferencesTableName = dtRefTable.Rows[0]["TABLE_NAME"].ToString().ToUpper();

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


                        // we now know both the source table and the references table.
                        // look up fields that are involved in the constraint on both ends.


                        if (tiRef == null) {

                            // if (maxRecurseDepth > 0) {

                            // the current table references one that isn't loaded yet.
                            // load it (and all of its referenced tables)
                            if (!tables.Exists(te => {
                                return te.TableName.ToLower().Trim() == ci.ReferencesTableName.ToLower().Trim();
                            })) {
                                tiRef = new OracleTableInfo(DataConnectionSpec);
                                tiRef.Fill(SchemaName.ToUpper(), ci.ReferencesTableName.ToUpper(), tables, fillRowCount, maxRecurseDepth - 1);
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
SELECT
    *
FROM
	ALL_CONS_COLUMNS
WHERE
	OWNER = :owner
    AND CONSTRAINT_NAME = :refersToConstraint
ORDER BY
    POSITION
", new DataParameters(":owner", SchemaName.ToUpper(), ":refersToConstraint", refersConstraintName.ToUpper()));

                        foreach (DataRow drRef in dtRefersTo.Rows) {
                            string refFieldName = drRef["COLUMN_NAME"].ToString().ToLower().Trim();
                            foreach (FieldInfo fiRef in tiRef.Fields) {
                                if (fiRef.Name.ToLower().Trim() == refFieldName) {
                                    ci.ReferencesFields.Add(fiRef);
                                    break;
                                }
                            }
                        }


                        // look up source table fields
                        var dtSourceFields = dm.Read(@"
SELECT
    *
FROM
    ALL_CONS_COLUMNS
WHERE
    OWNER = :owner
    AND CONSTRAINT_NAME = :constraintName
ORDER BY
    POSITION
",
                        new DataParameters(":owner", SchemaName.ToUpper(), ":constraintName", ci.ConstraintName.ToUpper()));

                        foreach (DataRow drSourceField in dtSourceFields.Rows) {
                            var name = drSourceField["COLUMN_NAME"].ToString().ToLower().Trim();
                            var fiSource = this.Fields.Find(f => { return f.Name.ToLower().Trim() == name; });
                            if (fiSource != null) {
                                ci.SourceFields.Add(fiSource);
                                ci.SourceFieldNames.Add(name);
                                fiSource.IsForeignKey = true;
                                fiSource.ForeignKeyConstraintName = ci.ConstraintName;
                                fiSource.ForeignKeyTableName = ci.ReferencesTableName;
                                fiSource.ForeignKeyTable = ci.ReferencesTable;
                                if (ci.ReferencesFields.Count > 0) {
                                    fiSource.ForeignKeyFieldName = ci.ReferencesFields[0].Name;
                                }
                            }
                        }

                        Constraints.Add(ci);

                    }
                }
			}
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "OracleTableInfo", resourceName, null, defaultValue, substitutes);
        }
    }
}
