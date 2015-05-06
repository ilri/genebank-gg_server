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

namespace GrinGlobal.DatabaseInspector.MySql {
	public class MySqlTableInfo : TableInfo {

        internal MySqlTableInfo(DataConnectionSpec dcs)
			: base(dcs) {
		}

		protected override string generateDropSql() {
			return "DROP TABLE " + FullTableName;
		}

		public override string ToString() {
			return base.ToString();
		}

        //protected override string generateIndexSelectSql() {
        //    string ret = null;

        //    var fmt = FieldMappingTable.Map(this.TableName.ToLower(), this.DataConnectionSpec);

        //    switch (this.TableName.ToLower()) {
        //        case "accession":
        //            ret = generateIndexSelectSqlForAccession(fmt);
        //            break;
        //        case "inventory":
        //            ret = generateIndexSelectSqlForInventory(fmt);
        //            break;
        //        case "order_request_item":
        //            ret = generateIndexSelectSqlForOrderRequestItem(fmt);
        //            break;
        //        case "crop_trait_observation":
        //            ret = generateIndexSelectSqlForCropTraitObservation(fmt);
        //            break;
        //        default:
        //            ret = generateIndexSelectSqlForOtherTables(fmt);
        //            break;
        //    }
        //    return ret;
        //}

		private string getCodedValueNameLookupSql(IField fm, string prefixForTableWithSiteCode) {

			string fullFieldName = "src." + fm.TableFieldName.ToLower();

            StringBuilder sb = new StringBuilder();
            int max = 1;
            for (int i = 0; i < max; i++) {
                string ret = String.Format(@"(select cvl_{0}.name from code_value cv_{0} left join code_value_lang cvl_{0} on cv_{0}.code_value_id = cvl_{0}.code_value_id and cvl_{0}.sys_lang_id = {1} where {2} = cv_{0}.value and cv_{0}.code_group_code = {4}) as {0}_name_{1}"
                    , fm.TableFieldName.ToLower(), i+1, fullFieldName, !String.IsNullOrEmpty(prefixForTableWithSiteCode) ? prefixForTableWithSiteCode : "src", fm.GroupName);

                sb.Append(ret);
                if (i < max-1) {
                    sb.AppendLine(", ").Append("\t\t\t\t");
                }
            }


            return sb.ToString();

		}

        private string getCropTraitCodeValueNameLookupSql() {

            StringBuilder sb = new StringBuilder();
            int max = 1;
            for (int i = 0; i < max; i++) {
                string ret = String.Format(@"(select concat(coalesce(ctc_{0}.code, ''), ' ', coalesce(ctcl_{0}.description, '')) from crop_trait_code ctc_{0} left join crop_trait_code_lang ctcl_{0} on ctc_{0}.crop_trait_code_id = ctcl_{0}.crop_trait_code_id and ctcl_{0}.sys_lang_id = {0} where ctc_{0}.crop_trait_code_id = src.crop_trait_code_id) as crop_trait_code_description_{0}"
                    , i+1);

                sb.Append(ret);
                if (i < max - 1) {
                    sb.AppendLine(", ").Append("\t\t\t\t");
                }
            }


            return sb.ToString();

        }

        //private string generateIndexSelectSqlForOtherTables(IFieldMappingTable fmt){
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("select ");
        //    for (int i=0; i < Fields.Count; i++) {
        //        FieldInfo fi = Fields[i];

        //        if (fi.Name.ToLower().StartsWith("is_")) {
        //            sb.Append("\t\t\tcase src.")
        //                .Append(fi.Name.ToLower())
        //                .Append(" when 'Y' then 1 else 0 end as ")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        } else {
        //            sb.Append("\t\t\tsrc.")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        }


        //        FieldMapping fm =  null;
        //        foreach(FieldMapping f in fmt.Mappings){
        //            if (f.TableFieldName.ToLower() == fi.Name.ToLower()){
        //                fm = f;
        //                break;
        //            }
        //        }

        //        if (fm != null){

        //            if (!String.IsNullOrEmpty(fm.CodeGroupCode)) {
        //                sb.Append("\t\t\t")
        //                    .Append(getCodedValueNameLookupSql(fm, ""))
        //                    .AppendLine(", ");
        //            //} else if (fm.IsCreatedBy || fm.IsModifiedBy || fm.IsOwnedBy) {
        //            //    sb.Append("\t\t\t")
        //            //        .Append("(select full_name from cooperator c where c.cooperator_id = ")
        //            //        .Append(fm.TableFieldName.ToLower())
        //            //        .Append(") as ");
        //            //    if (fm.IsCreatedBy) {
        //            //        sb.AppendLine("created_by_name, ");
        //            //    } else if (fm.IsModifiedBy) {
        //            //        sb.AppendLine("modified_by_name, ");
        //            //    } else if (fm.IsOwnedBy) {
        //            //        sb.AppendLine("owned_by_name, ");
        //            //    }
        //            }
        //        }

        //    }
        //    sb.Remove(sb.Length - 4, 4);
        //    sb.AppendLine()
        //        .Append("\t\tfrom")
        //        .AppendLine().Append("\t\t\t")
        //        .Append(TableName)
        //        .Append(" src  ")
        //        .Append("\t\t/*_ WHERE src." + PrimaryKey.Name + " in (:idlist) _*/");

        //    string ret = sb.ToString();
        //    return ret;
        //}

        private string generateIndexSelectSqlForCropTraitObservation(ITable fmt) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select ");
            for (int i = 0; i < Fields.Count; i++) {
                FieldInfo fi = Fields[i];

                if (i == 1) {
                    // inject the special fields
                    sb.Append(@"
            case when src.crop_trait_code_id is not null then
                ctc.code
            when src.numeric_value is not null then
                convert(varchar, src.numeric_value)
            when src.string_value is not null then
                src.string_value
            else
                null
            end as value,
            ct.name as crop_trait_name,
            concat(coalesce(m.name, ''), ' - ', coalesce(m.material_or_method_used, '')) as method_description,
            ctq.name as crop_trait_qualifier_name,
            ");

                    sb.Append(getCropTraitCodeValueNameLookupSql());
                    sb.AppendLine(", ");
                }

                if (fi.Name.ToLower().StartsWith("is_")) {
                    sb.Append("\t\t\tcase src.")
                        .Append(fi.Name.ToLower())
                        .Append(" when 'Y' then 1 else 0 end as ")
                        .Append(fi.Name.ToLower())
                        .AppendLine(", ");
                } else {
                    sb.Append("\t\t\tsrc.")
                        .Append(fi.Name.ToLower())
                        .AppendLine(", ");
                }


                IField fm = null;
                foreach (var f in fmt.Mappings) {
                    if (f.TableFieldName.ToLower() == fi.Name.ToLower()) {
                        fm = f;
                        break;
                    }
                }

                if (fm != null && !String.IsNullOrEmpty(fm.GroupName)) {
                    sb.Append("\t\t\t")
                        .Append(getCodedValueNameLookupSql(fm, "src"))
                        .AppendLine(", ");
                }

            }
            sb.Remove(sb.Length - 4, 4);
            sb.Append(@"
        from
            crop_trait_observation src left join crop_trait ct 
                on src.crop_trait_id = ct.crop_trait_id 
            left join crop_trait_code ctc
                on src.crop_trait_code_id = ctc.crop_trait_code_id
            left join method m
                on src.method_id = m.method_id
            left join crop_trait_qualifier ctq
                on src.crop_trait_qualifier_id = ctq.crop_trait_qualifier_id
        /*_ WHERE src.crop_trait_observation_id in (:idlist) _*/
");

            string ret = sb.ToString();
            return ret;
        }

        //private string generateIndexSelectSqlForAccession(IFieldMappingTable fmt) {
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("select ");
        //    for (int i=0; i < Fields.Count; i++) {
        //        FieldInfo fi = Fields[i];

        //        if (i == 1) {
        //            // inject the special fields
        //            sb.AppendLine("\t\t\tconcat(coalesce(src.accession_number_part1, ''), coalesce(convert(varchar, src.accession_number), ''), coalesce(src.accession_number_part3, '')) as pi_number, ");
        //            sb.AppendLine("\t\t\tan.name as accession_name_name, ");
        //        }

        //        if (fi.Name.ToLower().StartsWith("is_")) {
        //            sb.Append("\t\t\tcase src.")
        //                .Append(fi.Name.ToLower())
        //                .Append(" when 'Y' then 1 else 0 end as ")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        } else {
        //            sb.Append("\t\t\tsrc.")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        }


        //        FieldMapping fm =  null;
        //        foreach (FieldMapping f in fmt.Mappings) {
        //            if (f.TableFieldName.ToLower() == fi.Name.ToLower()) {
        //                fm = f;
        //                break;
        //            }
        //        }

        //        if (fm != null && !String.IsNullOrEmpty(fm.CodeGroupCode)) {
        //            sb.Append("\t\t\t")
        //                .Append(getCodedValueNameLookupSql(fm, "src"))
        //                .AppendLine(", ");
        //        }

        //    }
        //    sb.Remove(sb.Length - 4, 4);
        //    sb.AppendLine()
        //        .Append("\t\tfrom")
        //        .AppendLine().Append("\t\t\t")
        //        .Append(TableName)
        //        .AppendLine(" src left join accession_name an on src.accession_name_id = an.accession_name_id  /*_ WHERE src.accession_id in (:idlist) _*/");

        //    string ret = sb.ToString();
        //    return ret;
        //}

        //private string generateIndexSelectSqlForInventory(IFieldMappingTable fmt) {
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("select ");
        //    for (int i=0; i < Fields.Count; i++) {
        //        FieldInfo fi = Fields[i];

        //        if (i == 1) {
        //            // inject the pre/num/suf/type
        //            sb.AppendLine("\t\t\tconcat(coalesce(src.inventory_prefix, ''), coalesce(convert(varchar, src.inventory_number), ''), coalesce(src.inventory_suffix, ''), coalesce(src.inventory_type_code, '')) as pi_number, ");
        //        }

        //        if (fi.Name.ToLower().StartsWith("is_")) {
        //            sb.Append("\t\t\tcase src.")
        //                .Append(fi.Name.ToLower())
        //                .Append(" when 'Y' then 1 else 0 end as ")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        } else {
        //            sb.Append("\t\t\tsrc.")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        }


        //        FieldMapping fm =  null;
        //        foreach (FieldMapping f in fmt.Mappings) {
        //            if (f.TableFieldName.ToLower() == fi.Name.ToLower()) {
        //                fm = f;
        //                break;
        //            }
        //        }

        //        if (fm != null && !String.IsNullOrEmpty(fm.CodeGroupCode)) {
        //            sb.Append("\t\t\t")
        //                .Append(getCodedValueNameLookupSql(fm, "src"))
        //                .AppendLine(", ");
        //        }
			
        //    }
        //    sb.Remove(sb.Length - 4, 4);
        //    sb.AppendLine()
        //        .Append("\t\tfrom")
        //        .AppendLine().Append("\t\t\t")
        //        .Append(TableName).AppendLine(" src  /*_ WHERE src.inventory_id in (:idlist) _*/");

        //    string ret = sb.ToString();
        //    return ret;
        //}

        //private string generateIndexSelectSqlForOrderRequestItem(IFieldMappingTable fmt) {
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("select ");
        //    for (int i=0; i < Fields.Count; i++) {
        //        FieldInfo fi = Fields[i];

        //        if (i == 1) {
        //            // inject the special fields
        //        }

        //        if (fi.Name.ToLower().StartsWith("is_")) {
        //            sb.Append("\t\t\tcase src.")
        //                .Append(fi.Name.ToLower())
        //                .Append(" when 'Y' then 1 else 0 end as ")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        } else {
        //            sb.Append("\t\t\tsrc.")
        //                .Append(fi.Name.ToLower())
        //                .AppendLine(", ");
        //        }

        //        FieldMapping fm =  null;
        //        foreach (FieldMapping f in fmt.Mappings) {
        //            if (f.TableFieldName.ToLower() == fi.Name.ToLower()) {
        //                fm = f;
        //                break;
        //            }
        //        }

        //        if (fm != null && !String.IsNullOrEmpty(fm.CodeGroupCode)) {
        //            sb.Append("\t\t\t")
        //                .Append(getCodedValueNameLookupSql(fm, "oreq"))
        //                .AppendLine(", ");
        //        }

        //    }
        //    sb.Remove(sb.Length - 4, 4);
        //    sb.AppendLine()
        //        .Append("\t\tfrom")
        //        .AppendLine().Append("\t\t\t")
        //        .Append(TableName)
        //        .AppendLine(" src left join order_request oreq on src.order_request_id = oreq.order_request_id  /*_ WHERE src.order_request_item_id in (:idlist) _*/");

        //    string ret = sb.ToString();
        //    return ret;
        //}



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

		protected override string generateMapCreateSql() {
			return String.Format(@"CREATE TABLE `{0}`.`__{1}` (
new_id int(11) NOT NULL AUTO_INCREMENT,
previous_id int(11) NOT NULL,
table_name varchar(30) NULL,
PRIMARY KEY (new_id),
KEY `_MAP_PK_{1}` (previous_id, table_name)
) Engine=MyISAM ", SchemaName.ToLower(), TableName.ToLower());
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
				if (fi.IsPrimaryKey || (sbPKs.Length == 0 && fi.IsAutoIncrement)) {
                    // mysql requires auto increment fields to be the pk, so we tack it into the pk if there's not one explicitly set already
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
			dm.Write("SET AUTOCOMMIT=0;");
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

            string srcFile = Toolkit.ResolveFilePath((inputDirectory + Path.DirectorySeparatorChar + TableName + ".txt").Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString(), Path.DirectorySeparatorChar.ToString()), false);
            if (!File.Exists(srcFile)) {
                showProgress(getDisplayMember("bulkLoad{nofile}", "No data found for {0}.", TableName), 0, 0);
                return "";
            } else {

                //string tgtFile = (inputDirectory + @"\" + TableName + ".mysql").Replace(@"\\", @"\");
                //// HACK: 64-bit windows and the mysql LOAD DATA INFILE command seem to think unicode means 2 completely different things.
                ////       So we force the file to UTF8 encoding so mysql is happy.
                //using (StreamReader sr = new StreamReader(new FileStream(srcFile, FileMode.Open, FileAccess.Read), Encoding.Unicode)) {
                //    using (StreamWriter sw = new StreamWriter(new FileStream(tgtFile, FileMode.Create, FileAccess.Write), Encoding.UTF8)) {
                //        sw.Write(sr.ReadToEnd());
                //    }
                //}

                //var mysqlFilename = tgtFile.Replace(@"\", @"/");

                var mysqlFilename = srcFile.Replace(@"\", @"/");

                var sql = String.Format(@"
/*** Load data into table {1} ***/
select concat(now(), ' Loading data into table {1}') as Action;

LOAD DATA INFILE '{0}' INTO TABLE {1}
CHARACTER SET utf8
{2}
IGNORE 1 LINES 
", mysqlFilename, FullTableName, Creator.INFILE_OPTIONS);

                if (dryRun) {
                    File.WriteAllText(inputDirectory + Path.DirectorySeparatorChar + TableName + ".sql", sql);
                } else {
                    using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                        dm.DataConnectionSpec.CommandTimeout = 7200; // default to 2 hours before timing out. wowza.
                        dm.Write(sql);
                    }
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

					fi = new MySqlFieldInfo(this);
                    fi.Name = dr["Field"].ToString();
                    fi.TableName = this.TableName;

					switch (fieldType) {
						case "int":
							fi.DataType = typeof(int);
							break;
                        case "bigint":
                        case "long":
                            fi.DataType = typeof(long);
                            break;
                        case "text":
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



                // fill in rowcount
                if (fillRowCount) {
                    RowCount = Toolkit.ToInt32(dm.ReadValue(String.Format("select count(1) from {0}", FullTableName)), 0);
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
							ConstraintInfo ci = new MySqlConstraintInfo(this);
							ci.ConstraintName = m.Groups[1].Value.Replace("`", "").ToUpper();
							ci.TableName = this.TableName;
							ci.ReferencesTableName = m.Groups[3].Value.Replace("`", "").ToUpper();
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
                                    tiRef = new MySqlTableInfo(DataConnectionSpec);
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
            }

            string output = sb.ToString().Trim('\r', '\n', ' ', ',');
            return output;

        }

        internal string GenerateRebuildAllIndexesForTable() {
            // issuing an alter table with same engine will cause mysql to rebuild the table (and its indexes) in one fell swoop
            if (String.IsNullOrEmpty(Engine)) {
                Engine = "InnoDB";
            }
            return "ALTER TABLE " + FullTableName + " ENGINE=" + Engine;
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

            string output = sb.ToString().Trim('\r', '\n', ' ', ','); ;
            return output;

        }
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "MySqlTableInfo", resourceName, null, defaultValue, substitutes);
        }

	}
}
