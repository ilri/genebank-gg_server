using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using GrinGlobal.Core;
using GrinGlobal.Business.DataTriggers;
using GrinGlobal.Interface.DataTriggers;
using System.Web;
using GrinGlobal.Interface.Dataviews;
using System.IO;
using System.Diagnostics;

namespace GrinGlobal.Business {
    /// <summary>
    /// Represents a table as mapped in the sys_table / sys_table_field / sys_table_field_lang tables.
    /// </summary>
	public class Table : ITable {


        /// <summary>
        /// Inspects all fields in the ForeignKeys collection and returns true if one of them points at the given table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool IsParentOf(ITable table){
            foreach(var fk in table.ForeignKeys){
                if (String.Compare(fk.ForeignKeyTableName, this.TableName, true) == 0){
                    return true;
                }
            }
            return false;
        }


        private List<IField> _foreignKeyFields;
        public List<IField> ForeignKeys {
            get { return _foreignKeyFields; }
        }
       
        private List<IField> _mappings;
		public List<IField> Mappings {
			get { return _mappings; }
		}

        public Dictionary<string, object> ExtendedProperties { get; private set; }

        /// <summary>
        /// Gets or sets all fields that are part of the first unique index (excluding the primary key index) defined in sys_index / sys_index_field.
        /// </summary>
		public string UniqueKeyFields { get; set; }
		public int TableID { get; set; }
		public string TableName { get; set; }
        private string _aliasName;
        public string AliasName {
            get {
                if (_aliasName == null) {
                    var ret = new StringBuilder();
                    var t = TableName;
                    switch (t.ToLower()) {
                        case "crop":
                            _aliasName = "crop";
                            break;
                        case "taxonomy_use":
                            _aliasName = "tuse";
                            break;
                        case "genetic_observation":
                            _aliasName = "gob";
                            break;
                        case "order_request":
                            _aliasName = "oreq";
                            break;
                        case "accession_source":
                            _aliasName = "as1";
                            break;
                        default:
                            var words = t.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var w in words) {
                                ret.Append(w[0].ToString().ToLower());
                            }
                            _aliasName = ret.ToString();
                            break;
                    }
                }
                return _aliasName;
            }
            set {
                _aliasName = value;
            }
        }
		public bool IsReadOnly { get; set; }
		public bool AuditsCreated { get; set; }
		public bool AuditsModified { get; set; }
		public bool AuditsOwned { get; set; }
		public string PrimaryKeyFieldName { get; set; }
        public string PrimaryKeyDataViewFieldName { get; set; }

		public DataConnectionSpec DataConnectionSpec { get; set; }

        /// <summary>
        /// Gets or sets all data triggers defined in sys_datatrigger with is_enabled='Y' for the current table and that implement the ITableSaveDataTrigger interface.
        /// </summary>
		public List<ITableSaveDataTrigger> SaveDataTriggers { get; set; }
        /// <summary>
        /// Gets or sets all data triggers defined in sys_datatrigger with is_enabled='Y' for the current table and that implement the ITableReadDataTrigger interface.
        /// </summary>
        public List<ITableReadDataTrigger> ReadDataTriggers { get; set; }

		public Table() {
			_mappings = new List<IField>();
            _foreignKeyFields = new List<IField>();
			SaveDataTriggers = new List<ITableSaveDataTrigger>();
            ReadDataTriggers = new List<ITableReadDataTrigger>();
			TableName = null;
            TableID = -1;
            ExtendedProperties = new Dictionary<string, object>();
		}

		public override string ToString() {
			return TableName + " (" + AliasName + ") : " + _mappings.Count + " field(s) mapped";
		}

        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        /// <returns></returns>
		public ITable Clone() {
			Table fmt = new Table();
            fmt.AliasName = this.AliasName;
            fmt.AuditsCreated = this.AuditsCreated;
			fmt.AuditsModified = this.AuditsModified;
			fmt.AuditsOwned = this.AuditsOwned;
            fmt.DataConnectionSpec = this.DataConnectionSpec;

            // copy extended property values...
            foreach (var key in this.ExtendedProperties.Keys) {
                fmt.ExtendedProperties[key] = ExtendedProperties[key];
            }

            fmt.IsReadOnly = this.IsReadOnly;
            foreach (IField fm in this.Mappings) {
                fmt.Mappings.Add(fm.Clone(fmt));
            }
            foreach (IField ifm in this.ForeignKeys) {
                fmt.ForeignKeys.Add(ifm.Clone(fmt));
            }
            fmt.PrimaryKeyDataViewFieldName = this.PrimaryKeyDataViewFieldName;
            fmt.PrimaryKeyFieldName = this.PrimaryKeyFieldName;

            // clone trigger objects (in case they're stateful)
            foreach (ITableReadDataTrigger readTrigger in this.ReadDataTriggers) {
                fmt.ReadDataTriggers.Add((ITableReadDataTrigger)readTrigger.Clone());
            }
            foreach (ITableSaveDataTrigger saveTrigger in this.SaveDataTriggers) {
                fmt.SaveDataTriggers.Add((ITableSaveDataTrigger)saveTrigger.Clone());
            }

            fmt.TableID = this.TableID;
			fmt.TableName = this.TableName;

            fmt.UniqueKeyFields = this.UniqueKeyFields;

			return fmt;
		}


        //public static IFieldMappingTable Map(string tableName) {
        //    return Map(tableName, null);
        //}

        public static void ClearCache() {
            CacheManager.Get("FieldMappingTableCache").Clear();
        }

        /// <summary>
        /// Connects to the given database and returns a table object based on data in sys_table / sys_table_field / sys_table_field_lang tables for the given tableName.  Returns an empty definition if not found.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dcs"></param>
        /// <param name="languageID"></param>
        /// <param name="loadFriendlyNamesForAllLanguages"></param>
        /// <returns></returns>
        public static ITable Map(string tableName, DataConnectionSpec dcs, int languageID, bool loadFriendlyNamesForAllLanguages) {
            using (DataManager dm = (dcs == null ? DataManager.Create() : DataManager.Create(dcs))){
                return Map(tableName, dm, languageID, loadFriendlyNamesForAllLanguages);
            }
        }

        /// <summary>
        /// Returns a table object based on data in sys_table / sys_table_field / sys_table_field_lang tables for the given tableName.  Returns an empty definition if not found.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dm"></param>
        /// <param name="languageID"></param>
        /// <param name="loadFriendlyNamesForAllLanguages"></param>
        /// <returns></returns>
        public static ITable Map(string tableName, DataManager dm, int languageID, bool loadFriendlyNamesForAllLanguages) {
			string cacheName =  "FieldMappingTableCache";
            string cacheKey = tableName + languageID;
			CacheManager cache = CacheManager.Get(cacheName);
			var fmt = (ITable)(cache[cacheKey]);
			if (fmt != null) {

				// already exists, just return it
				return fmt;

			} else {



				fmt = new Table();
				fmt.TableName = tableName;

				DataTable dt = dm.Read(@"
select
	st.sys_table_id,
	st.table_name,
	st.audits_created,
	st.audits_modified,
	st.audits_owned,
	st.is_readonly as table_is_readonly,
	stf.sys_table_field_id,
    stf.field_ordinal,
	stf.field_name,
	stf.field_purpose,
	stf.field_type,
	stf.default_value,
	stf.is_primary_key,
	stf.is_foreign_key,
	stf.is_nullable,
	stf.is_autoincrement,
	stf.gui_hint,
	stf.is_readonly as field_is_readonly,
	stf.min_length,
	stf.max_length,
	stf.numeric_precision,
	stf.numeric_scale,
	stf.group_name,
    stfl.sys_lang_id,
    coalesce(stfl.title, stf.field_name) as friendly_field_name,
    stfl.description as friendly_field_description,
	stf.foreign_key_dataview_name,
    st2.table_name as foreign_key_table_name,
    stf2.field_name as foreign_key_table_field_name
from
	sys_table st inner join sys_table_field stf
		on st.sys_table_id = stf.sys_table_id
    left join sys_table_field_lang stfl
        on stf.sys_table_field_id = stfl.sys_table_field_id
        and coalesce(stfl.sys_lang_id, 0) = coalesce(:langid, stfl.sys_lang_id, 0)
	left join sys_table_field stf2
		on stf.foreign_key_table_field_id = stf2.sys_table_field_id
    left join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where
	st.table_name = :tablename 
order by
	stf.field_ordinal
", new DataParameters(":tablename", tableName, ":langid", (loadFriendlyNamesForAllLanguages ? null : (int?)languageID), DbType.Int32));

				if (dt.Rows.Count == 0) {
					return fmt;
				} else {

					// set table-level properties
					fmt.TableName = dt.Rows[0]["table_name"].ToString().ToLower();
					fmt.AuditsCreated = dt.Rows[0]["audits_created"].ToString().ToUpper() == "Y";
					fmt.AuditsModified = dt.Rows[0]["audits_modified"].ToString().ToUpper() == "Y";
					fmt.AuditsOwned = dt.Rows[0]["audits_owned"].ToString().ToUpper() == "Y";
					fmt.IsReadOnly = dt.Rows[0]["table_is_readonly"].ToString().ToUpper() == "Y";
					fmt.TableID = Toolkit.ToInt32(dt.Rows[0]["sys_table_id"], -1);
					fmt.UniqueKeyFields = determineAlternateKeyFields(dm, fmt.TableID);


                    string prevFieldName = null;
                    Field fm = null;
					// read field-level properties, add each row as a field mapping
					foreach (DataRow dr in dt.Rows) {
                        var fieldName = dr["field_name"].ToString();

                        var langID = Toolkit.ToInt32(dr["sys_lang_id"], -1);

                        if (prevFieldName != fieldName) {
                            fm = new Field();
                            fm.Table = fmt;
                            fm.TableFieldID = Toolkit.ToInt32(dr["sys_table_field_id"], -1);
                            fm.TableName = fmt.TableName;
                            fm.TableFieldName = dr["field_name"].ToString();
                            fm.FieldPurpose = dr["field_purpose"].ToString();
                            fm.DataTypeString = dr["field_type"].ToString();
                            fm.DataType = Dataview.ParseDataType(dr["field_type"].ToString());
                            fm.DefaultValue = (dr["default_value"] == DBNull.Value ? "{DBNull.Value}" : dr["default_value"].ToString());
                            //fm.DefaultValue = (dr["default_value"] == DBNull.Value ? "{DBNull.Value}" : dr["default_value"].ToString());
                            fm.IsPrimaryKey = dr["is_primary_key"].ToString().ToUpper() == "Y";
                            if (fm.IsPrimaryKey) {
                                fmt.PrimaryKeyFieldName = fm.TableFieldName;
                            }

                            if (langID == languageID) {
                                fm.FriendlyFieldName = dr["friendly_field_name"].ToString();
                                fm.FriendlyDescription = dr["friendly_field_description"].ToString();
                            }

                            fm.IsForeignKey = dr["is_foreign_key"].ToString().ToUpper() == "Y";
                            fm.ForeignKeyTableName = dr["foreign_key_table_name"].ToString();
                            fm.ForeignKeyTableFieldName = dr["foreign_key_table_field_name"].ToString();

                            fm.ForeignKeyDataviewName = dr["foreign_key_dataview_name"].ToString();
                            fm.ForeignKeyDataviewParam = Dataview.DetermineForeignKeyDataviewParams(dm, fm.ForeignKeyDataviewName);

                            fm.IsNullable = dr["is_nullable"].ToString().ToUpper() == "Y";
                            fm.GuiHint = dr["gui_hint"].ToString();
                            fm.IsReadOnly = dr["field_is_readonly"].ToString().ToUpper() == "Y";
                            fm.IsReadOnlyOnInsert = dr["field_is_readonly"].ToString().ToUpper() == "I";
                            fm.IsAutoIncrement = dr["is_autoincrement"].ToString().ToUpper() == "Y";

                            // readonly is driven by many things. it can be marked as readonly,
                            // it could be marked as autoincrement
                            if (!fm.IsReadOnly && fm.IsAutoIncrement) {
                                fm.IsReadOnly = true;
                            }


                            fm.MinimumLength = Toolkit.ToInt32(dr["min_length"], 0);
                            fm.MaximumLength = Toolkit.ToInt32(dr["max_length"], 0);
                            fm.Precision = Toolkit.ToInt32(dr["numeric_precision"], 0);
                            fm.Scale = Toolkit.ToInt32(dr["numeric_scale"], 0);
                            fm.GroupName = dr["group_name"].ToString();

                            fmt.Mappings.Add(fm);
                            if (fm.IsForeignKey) {
                                fmt.ForeignKeys.Add(fm);
                            }

                            prevFieldName = fieldName;
                        }

                        if (langID > -1) {
                            fm.DataviewFriendlyFieldNames.Add(langID, dr["friendly_field_name"].ToString());
                        }

					}

					// get all trigger(s) associated with this table


                    // certain table-level triggers are required for the system to operate properly...
                    addSystemLevelTableDataTriggers(fmt);

                    // and certain table-level triggers are optional and can be configured by the end user...
                    addAdHocTableDataTriggers(fmt, dm);

				}
			}
            cache[cacheKey] = fmt;
			return fmt;
		}

        /// <summary>
        /// Adds all data triggers absolutely required by the system for proper functionality.  Namely AccessionDataTrigger, CacheDataTrigger, and SearchDataTrigger.
        /// </summary>
        /// <param name="fmt"></param>
        private static void addSystemLevelTableDataTriggers(ITable fmt) {

            if (fmt.TableName.ToLower() == "accession") {
                fmt.SaveDataTriggers.Add(new AccessionDataTrigger());
            }

            // maint policy function moved to additional data trigger
            //if (fmt.TableName.ToLower() == "inventory")
            //{
                //fmt.SaveDataTriggers.Add(new InventoryDataTrigger());
            //}

            // Cache trigger is always applied...
            fmt.SaveDataTriggers.Add(new CacheDataTrigger());

            // Search trigger is always applied...
            //fmt.SaveDataTriggers.Add(new SearchDataTrigger());

        }

        /// <summary>
        /// Adds all data triggers defined in the sys_datatrigger table with is_enabled = 'Y' for this table.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="dm"></param>
        private static void addAdHocTableDataTriggers(ITable fmt, DataManager dm) {

            // TODO: implement object pooling so every time we Map a table doesn't necessarily 
            //       mean we have to load class(es) from .dll file(s)


//            DataTable dtFilter = dm.Read(@"
//select 
//filt.sys_filter_id
//      ,filt.sys_dataview_id
//      ,filt.sys_table_id
//      ,filt.virtual_file_path
//      ,filt.assembly_name
//      ,filt.fully_qualified_class_name
//      ,filt.is_enabled
//      ,filt.is_system
//      ,filt.sort_order
//      ,filt.created_date
//      ,filt.created_by
//      ,filt.modified_date
//      ,filt.modified_by
//      ,filt.owned_date
//      ,filt.owned_by
//from 
//	sys_filter filt
//where
//	filt.is_enabled = 'Y'
//	and (:tblid = filt.sys_table_id or filt.sys_table_id is null)
//order by
//	filt.is_system desc,
//	filt.sort_order
//", new DataParameters(":tblid", fmt.TableId, DbType.Int32));

            DataTable dtTrigger = dm.Read(@"
select 
sdt.sys_datatrigger_id
      ,sdt.sys_dataview_id
      ,sdt.sys_table_id
      ,sdt.virtual_file_path
      ,sdt.assembly_name
      ,sdt.fully_qualified_class_name
      ,sdt.is_enabled
      ,sdt.is_system
      ,sdt.sort_order
      ,sdt.created_date
      ,sdt.created_by
      ,sdt.modified_date
      ,sdt.modified_by
      ,sdt.owned_date
      ,sdt.owned_by
from 
	sys_datatrigger sdt
where
	sdt.is_enabled = 'Y'
	and (:tblid = sdt.sys_table_id or sdt.sys_table_id is null)
order by
	sdt.is_system desc,
	sdt.sort_order
", new DataParameters(":tblid", fmt.TableID, DbType.Int32));

            var iis = HttpContext.Current == null ? Toolkit.GetIISPhysicalPath("gringlobal") : string.Empty;

            foreach (DataRow drTrigger in dtTrigger.Rows) {
                // create an instance of the ISaveTrigger object to cache for later
                //string binPath = "~/";
                //if (HttpContext.Current != null) {
                //    binPath = "~/bin/";
                //}
                //string asmName = Toolkit.ResolveFilePath(binPath + drTrigger["assembly_name"].ToString().Replace(".dll", "") + ".dll", false);
                string asmName = "";
                if (HttpContext.Current != null) {
                    asmName = HttpContext.Current.Server.MapPath(drTrigger["virtual_file_path"].ToString());
                } else {
                    var path = drTrigger["virtual_file_path"].ToString();
                    //if (Debugger.IsAttached) {
                    //    // HACK: to enable debugging triggers, we load from the current path instead of IIS...
                    //    asmName = Toolkit.ResolveFilePath(path.Replace("~/bin", "~").Replace(@"~\bin", "~"), false);
                    //} else {
                        asmName = Toolkit.ResolveFilePath(path, false);
                    //}
                    if (!File.Exists(asmName)) {
                        // dll wasn't in / under current folder.  check iis path (in case this is used outside the context of the web service, such as via import wizard)
                        asmName = Toolkit.ResolveFilePath(path.Replace("~", iis).Replace(@"~", iis), false);
                    }
                }
                string fqcn = drTrigger["fully_qualified_class_name"].ToString();
                if (!String.IsNullOrEmpty(asmName) && !String.IsNullOrEmpty(fqcn)) {
                    try {
                        if (HttpContext.Current == null && !File.Exists(asmName)) {
                            // doesn't exist, not called via middle tier. skip.
                        } else {
                            object trigger = Activator.CreateInstanceFrom(asmName, fqcn).Unwrap();
                            if (trigger is ITableSaveDataTrigger) {
                                fmt.SaveDataTriggers.Add((ITableSaveDataTrigger)trigger);
                            } else if (trigger is ITableReadDataTrigger) {
                                fmt.ReadDataTriggers.Add((ITableReadDataTrigger)trigger);
                            } else if (trigger is IDataviewReadDataTrigger || trigger is IDataviewSaveDataTrigger) {
                                // do not load -- we're at the table level, not the dataview level.
                            } else {
                                throw new InvalidOperationException(getDisplayMember("addAdHocTableDataTriggers{nointerface}", "The class '{0}' in assembly '{1}' does not implement the either the GrinGlobal.Business.DataTriggers.ITableSaveDataTrigger or ITableReadDataTrigger interface.  Either the class is coded incorrectly or it should be configured as a DataView DataTrigger, not a Table DataTrigger.", fqcn , asmName));
                            }
                        }
                    } catch (FileNotFoundException fnf) {
                        if (HttpContext.Current != null) {
                            throw new InvalidOperationException(getDisplayMember("addAdHocTableDataTriggers{nofile}", "A data trigger is mapped in the sys_datatrigger table which points to '{0}' but that file does not exist.  Source:{1}", drTrigger["virtual_file_path"].ToString(), fnf.Message));
                        } else {
                            // if they're using the admin tool... we don't require all triggers to exist.
                            Debug.WriteLine("Could not load data trigger " + fqcn + " because the file " + asmName + " was not found.  Assuming they're running Admin Tool since there is not a current HttpContext");
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine(ex.Message);
                        // couldn't load that class.
                        throw;
                    }
                }
            }

        }

        /// <summary>
        /// Reads the sys_index and sys_index_field tables for the given table, and returns a comma-separated list of the fields defined on the first unique index for that table. This does not include the primary key index.
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
		private static string determineAlternateKeyFields(DataManager dm, int tableId) {

            // 2010-09-16 Brock Weaver brock@circaware.com
            //            Just grabbing 'first' unique key defined, so if there are multiples we don't accidentally
            //            pull in fields from ALL of them (crop has 2 unique keys on the same field... for whatever reason)
            var indexId = Toolkit.ToInt32(dm.ReadValue(@"
select 
	min(si.sys_index_id)
from 
	sys_index si
where
	si.sys_table_id = :tableid
	and si.is_unique = 'Y'
", new DataParameters(":tableid", tableId, DbType.Int32)), 0);

            if (indexId > 0) {


                DataTable dtAlternateKeyFields = dm.Read(@"
select 
	si.index_name,
	stf.field_name
from 
	sys_index si inner join sys_index_field sif
		on si.sys_index_id = sif.sys_index_id
	inner join sys_table_field stf
		on sif.sys_table_field_id = stf.sys_table_field_id
where
    si.sys_index_id = :indexid
order by
	sif.sort_order
", new DataParameters(":indexid", indexId, DbType.Int32));

                StringBuilder sbAlternateKeyFields = new StringBuilder();
                foreach (DataRow drAlternateKeyFields in dtAlternateKeyFields.Rows) {
                    // find the corresponding field mapping
                    sbAlternateKeyFields.Append(drAlternateKeyFields["field_name"].ToString().ToLower()).Append(", ");
                }
                if (sbAlternateKeyFields.Length > 0) {
                    sbAlternateKeyFields.Remove(sbAlternateKeyFields.Length - 2, 2);
                    return sbAlternateKeyFields.ToString();
                    //string fieldsInAlternateKey = sbAlternateKeyFields.ToString();
                    //// find the corresponding field mapping
                    //foreach (FieldMapping fm in fmt.Mappings) {
                    //    foreach (DataRow drAlternateKeyFields in dtAlternateKeyFields.Rows) {
                    //        if (fm.DataViewFieldName.ToLower() == drAlternateKeyFields["field_name"].ToString().ToLower()) {
                    //            fm.AlternateKeyFields = fieldsInAlternateKey;
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            return "";
        }

        /// <summary>
        /// Returns the field object from the Mappings collection for the given fieldname.  Returns null if not found.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IField GetField(string fieldName) {
            foreach (var m in Mappings) {
                if (String.Compare(m.TableFieldName, fieldName, true) == 0) {
                    return m;
                }
            }
            return null;
        }

        /// <summary>
        /// Generates a database engine-agnostic SELECT statement for the current table object.
        /// </summary>
        /// <param name="addAllFields"></param>
        /// <returns></returns>
        public string GetSQL(bool addAllFields) {
            var sb = new StringBuilder("SELECT\r\n  ");
            if (addAllFields) {
                for (var i = 0; i < Mappings.Count; i++) {
                    if (i > 0) {
                        sb.Append(",\r\n  ");
                    }
                    sb.Append(AliasName).Append(".").Append(Mappings[i].TableFieldName).Append(" AS ").Append(Mappings[i].TableFieldName);
                    //                sb.Append(" as ").Append(Mappings[i].TableFieldName);
                }
            }
            sb.Append("\r\nFROM\r\n  ");
            sb.Append(this.TableName);
            sb.Append(" ").Append(AliasName);
            //if (!String.IsNullOrEmpty(this.PrimaryKeyFieldName)) {
            //    sb.Append("\r\nORDER BY\r\n  ");
            //    sb.Append(AliasName).Append(".").Append(this.PrimaryKeyFieldName);
            //}
            return sb.ToString();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "Table", resourceName, null, defaultValue, substitutes);
        }

    }
}
