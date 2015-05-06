using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;

using System.Data;
using System.Diagnostics;
using GrinGlobal.Business.DataTriggers;
using GrinGlobal.Interface.DataTriggers;
using System.Web;
using GrinGlobal.Interface;
using System.Reflection;
//using GrinGlobal.Interface.Forms;
using GrinGlobal.Interface.Dataviews;
using System.IO;

namespace GrinGlobal.Business {
    /// <summary>
    /// Represents a dataview definition in its entirety, including sql, parameters, fields, language-specific names and titles, and related table mappings.
    /// </summary>
	public class Dataview : IDataview {

        public Dictionary<string, object> ExtendedProperties { get; private set; }

        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        /// <returns></returns>
        public IDataview Clone() {

            var ret = new Dataview();
            ret.IsEnabled = this.IsEnabled;
            ret.IsReadOnly = this.IsReadOnly;
            //ret.IsPropertySuppressed = this.IsPropertySuppressed;
            //ret.IsRequiredBySystem = this.IsRequiredBySystem;
            //ret.IsUserVisible = this.IsUserVisible;
            //ret.IsWebVisible = this.IsWebVisible;
            ret.DataViewName = this.DataViewName;
            foreach (var key in this.SqlStatements.Keys) {
                ret.SqlStatements[key] = this.SqlStatements[key];
            }

            ret.Title = this.Title;
            ret.Description = this.Description;

            ret.IsTransform = this.IsTransform;
            ret.TransformFieldForCaptions = this.TransformFieldForCaptions;
            ret.TransformFieldForNames = this.TransformFieldForNames;
            ret.TransformFieldForValues = this.TransformFieldForValues;

            //ret.FormAssemblyName = this.FormAssemblyName;
            //ret.FormFullyQualifiedName = this.FormFullyQualifiedName;
            //ret.FormAssemblyVersion = this.FormAssemblyVersion;
            //ret.ExecutableName = this.ExecutableName;
            ret.TransformByFields = this.TransformByFields;
            ret.CategoryCode = this.CategoryCode;
            ret.DatabaseAreaCode = this.DatabaseAreaCode;
            ret.DatabaseAreaSortOrder = this.DatabaseAreaSortOrder;
            ret.ConfigurationOptions = this.ConfigurationOptions;

            ret._sqlStatementForCurrentEngine = this.SqlStatementForCurrentEngine;

            foreach (var prm in this.Parameters) {
                ret.Parameters.Add(prm.Clone());
            }
            foreach (var fmt in Tables) {
                var fmtClone = fmt.Clone();
                ret.Tables.Add(fmtClone);
                //foreach (var fm in fmt.Mappings) {
                //    ret.Mappings.Add(fm.Clone(fmtClone));
                //}
            }

            // clone trigger objects (in case they're stateful)
            foreach (var trigger in this.SaveDataTriggers) {
                ret.SaveDataTriggers.Add((IDataviewSaveDataTrigger)trigger.Clone());
            }

            foreach (var trigger in this.ReadDataTriggers) {
                ret.ReadDataTriggers.Add((IDataviewReadDataTrigger)trigger.Clone());
            }

            foreach (string pk in this.PrimaryKeyNames) {
                ret.PrimaryKeyNames.Add(pk);
            }

            return ret;
        }


        private List<ITable> _tables;
		/// <summary>
		/// Gets the fields mapped to tables that the dataview represents. for a dataview that is a direct select * from table1, contains 1 mapping which is exactly the same as the Mappings property on the FieldMappingResultSet object.
		/// </summary>
        public List<ITable> Tables {
			get {
				return _tables;
			}
		}

        /// <summary>
        /// Gets or sets the data triggers associated with this dataview in sys_datatrigger and that implement the IDataViewSaveDataTrigger interface
        /// </summary>
        public List<IDataviewSaveDataTrigger> SaveDataTriggers { get; set; }
        /// <summary>
        /// Gets or sets the data triggers associated with this dataview in sys_datatrigger and that implement the IDataViewReadDataTrigger interface
        /// </summary>
        public List<IDataviewReadDataTrigger> ReadDataTriggers { get; set; }

        public ITable GetParentTable(IField fm) {
            ITable fmt = _tables.Find(delegate(ITable test) {
				return test.TableName == fm.TableName;
			});
			return fmt;
		}

        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsTransform { get; set; }
        public string FormAssemblyName { get; set; }
        public string FormFullyQualifiedName { get; set; }
        public string FormAssemblyVersion { get; set; }
        public string ExecutableName { get; set; }
        public string CategoryCode { get; set; }
        public string DatabaseAreaCode { get; set; }
        public int DatabaseAreaSortOrder { get; set; }
        public string[] TransformByFields { get; set; }
        public string TransformFieldForNames { get; set; }
        public string TransformFieldForCaptions { get; set; }
        public string TransformFieldForValues { get; set; }
        public string ConfigurationOptions { get; set; }

        private string _sqlStatementForCurrentEngine;
        /// <summary>
        /// Gets the SQL statement for this dataview that corresponds to the database engine the system uses.
        /// </summary>
        public string SqlStatementForCurrentEngine {
            get {
                return _sqlStatementForCurrentEngine;
            }
        }

        public string PrimaryKeyTableName { get; set; }
        public List<string> PrimaryKeyNames { get; set; }

		private List<IField> _mappings;
		/// <summary>
		/// Gets the fields mapped specifically for this dataview via sys_dataview_field.  i.e. lets you look up a field without knowing which table it belongs to
		/// </summary>
		public List<IField> Mappings {
			get {
				return _mappings;
			}
		}

		private List<IDataviewParameter> _parameters;
        /// <summary>
        /// Gets all the parameters defined in the sys_dataview_param table for this dataview.
        /// </summary>
		public List<IDataviewParameter> Parameters {
			get {
				return _parameters;
			}
		}

        //private string _sqlStatement;
        //public string SqlStatement {
        //    get {
        //        return _sqlStatement;
        //    }
        //}

        private Dictionary<string, string> _sqlStatements;
        /// <summary>
        /// Gets all SQL statements defined in sys_dataview_sql for this dataview.  Each database engine has its own SQL statement.
        /// </summary>
        public Dictionary<string, string> SqlStatements {
            get {
                return _sqlStatements;
            }
        }

        public List<string> TableNames {
            get {
                var names = new List<string>();
                foreach (var tbl in this.Tables) {
                    names.Add(tbl.TableName);
                }
                return names;
            }
        }

		private string _dataviewName;
		public string DataViewName {
			get {
				return _dataviewName;
			}
            set {
                _dataviewName = value;
            }
		}

		public bool IsRequiredBySystem { get; set; }
		public bool IsPropertySuppressed { get; set; }

		public bool IsUserVisible { get; set; }
        public bool IsWebVisible { get; set; }

		public Dataview() {
			_tables = new List<ITable>();

            _mappings = new List<IField>();
            _parameters = new List<IDataviewParameter>();
            PrimaryKeyNames = new List<string>();
            SaveDataTriggers = new List<IDataviewSaveDataTrigger>();
            ReadDataTriggers = new List<IDataviewReadDataTrigger>();
            _sqlStatements = new Dictionary<string, string>();
			_dataviewName = null;
            ExtendedProperties = new Dictionary<string, object>();
		}

		public static Type ParseDataType(string typeName){
			string type = typeName.ToLower();
			switch (type) {
				case "integer":
				case "int":
				case "int32":
					type = "System.Int32";
					break;
				case "string":
                case "stringreplacement":
				case "varchar":
				case "varchar2":
					type = "System.String";
					break;
				case "datetime2":
				case "datetime":
				case "date":
					type = "System.DateTime";
					break;
				case "integercollection":
				case "integerarray":
				case "intcollection":
				case "intarray":
				case "int32collection":
				case "int32array":
				case "int[]":
				case "int32[]":
					type = "System.Int32[]";
					break;
                case "decimalcollection":
                case "decimalarray":
                case "decimal[]":
                    type = "System.Decimal[]";
                    break;
                default:
					type = "System." + type.ToTitleCase();
					break;
			}

			return Type.GetType(type);
		}

        /// <summary>
        /// Finds the field in the Mappings collection which matches the given sysTableID, aliasName, and sysTableFieldID.  Returns null if not found.
        /// </summary>
        /// <param name="sysTableID"></param>
        /// <param name="aliasName"></param>
        /// <param name="sysTableFieldID"></param>
        /// <returns></returns>
        private IField findField(int sysTableID, string aliasName, int sysTableFieldID) {
			foreach (Table fmt in this.Tables) {
				if (fmt.TableID == sysTableID && (String.IsNullOrEmpty(aliasName) || fmt.AliasName.ToLower() == aliasName.ToLower()) ) {
					foreach (Field fm in fmt.Mappings) {
						if (fm.TableFieldID == sysTableFieldID) {
							return fm;
						}
					}
				}
			}
			return null;
		}

        /// <summary>
        /// Returns an array of DataColumns that are defined in the PrimaryKeyNames list for this dataview.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataColumn[] GetPrimaryKeyColumns(DataTable dt) {
            List<DataColumn> cols = new List<DataColumn>();
            foreach (string kn in PrimaryKeyNames) {
                if (dt.Columns.Contains(kn)) {
                    cols.Add(dt.Columns[kn]);
                }
            }
            if (cols.Count == 0) {
                return null;
            } else {
                return cols.ToArray();
            }
        }

		public static void ClearCache() {
			CacheManager.Get("FieldMappingDataViewCache").Clear();
            Table.ClearCache();
        }

		/// <summary>
		/// Maps the given dataview name (aka table or view name) and fields to the corresponding actual database table and field names
		/// </summary>
		/// <param name="dataviewName"></param>
		/// <param name="languageID">The language we are to return</param>
		/// <returns></returns>
		public static IDataview Fill(string dataviewName, int languageID, DataManager dm) {

			dataviewName = dataviewName.ToLower();

            string cacheName = "FieldMappingDataViewCache";
			string cacheKey = dataviewName + "__" + languageID;
			CacheManager cache = CacheManager.Get(cacheName);
			var fmdv = (Dataview)(cache[cacheKey]);
			if (fmdv != null) {
				// already exists, just return it
				return fmdv;

			} else {

				int dvID = -1;
				// first, get info at the dataview level
				DataTable dtDV = dm.Read(@"
select
    sdl.title,
    sdl.description,
    sd.sys_dataview_id
      ,sd.dataview_name
      ,sd.is_enabled
      ,sd.is_readonly
      ,sd.category_code
      ,sd.database_area_code
      ,sd.database_area_code_sort_order
      ,sd.is_transform
      ,sd.transform_field_for_names
      ,sd.transform_field_for_captions
      ,sd.transform_field_for_values
      ,sd.configuration_options
      ,sd.created_date
      ,sd.created_by
      ,sd.modified_date
      ,sd.modified_by
      ,sd.owned_date
from
	sys_dataview sd left join sys_dataview_lang sdl
        on sd.sys_dataview_id = sdl.sys_dataview_id
        and sdl.sys_lang_id = :langid 
where
	sd.dataview_name = :dataview
", new DataParameters(":dataview", dataviewName, ":langid", languageID, DbType.Int32));

				if (dtDV.Rows.Count == 0) {
					// dataview not found. just return null, let caller decide what to do
					return null;
				} 


				DataRow drDV = dtDV.Rows[0];
				fmdv = new Dataview();

				dvID = Toolkit.ToInt32(drDV["sys_dataview_id"], -1);

				fmdv.IsEnabled = drDV["is_enabled"].ToString().ToUpper() == "Y";
				fmdv.IsReadOnly = drDV["is_readonly"].ToString().ToUpper() == "Y";
                //fmdv.IsRequiredBySystem = drDV["is_system"].ToString().ToUpper() == "Y";
                //fmdv.IsPropertySuppressed = drDV["is_property_suppressed"].ToString().ToUpper() == "Y";
                //fmdv.IsUserVisible = drDV["is_user_visible"].ToString().ToUpper() == "Y";
                //fmdv.IsWebVisible = drDV["is_web_visible"].ToString().ToUpper() == "Y";
                fmdv.IsTransform = drDV["is_transform"].ToString().ToUpper() == "Y";
                fmdv.DataViewName = drDV["dataview_name"].ToString().ToLower();
                fmdv.TransformFieldForCaptions = drDV["transform_field_for_captions"].ToString();
                fmdv.TransformFieldForNames = drDV["transform_field_for_names"].ToString();
                fmdv.TransformFieldForValues = drDV["transform_field_for_values"].ToString();
                fmdv.CategoryCode = drDV["category_code"].ToString();
                fmdv.DatabaseAreaCode = drDV["database_area_code"].ToString();
                fmdv.DatabaseAreaSortOrder = Toolkit.ToInt32(drDV["database_area_code_sort_order"], 0);
                fmdv.Title = drDV["title"].ToString();
                fmdv.Description = drDV["description"].ToString();
                fmdv.ConfigurationOptions = drDV["configuration_options"].ToString();

                DataTable dtSqls = dm.Read(@"
select
    sys_dataview_sql_id
      ,sys_dataview_id
      ,database_engine_tag
      ,sql_statement
      ,created_date
      ,created_by
      ,modified_date
      ,modified_by
      ,owned_date
      ,owned_by
from
    sys_dataview_sql
where
    sys_dataview_id = :id
", new DataParameters(":id", dvID, DbType.Int32));

                foreach (DataRow sqlRow in dtSqls.Rows) {
                    string sql = sqlRow["sql_statement"].ToString().Trim();
                    if (String.IsNullOrEmpty(sql)) {
                        sql = "select * from " + fmdv.DataViewName;
                    }
                    fmdv.SqlStatements.Add(sqlRow["database_engine_tag"].ToString(), sql);
                }

                addSystemLevelDataviewDataTriggers(fmdv);

                addAdHocDataviewDataTriggers(fmdv, dm, dvID);

				// now, we need to determine which table(s) are associated with this dataview and map in their fields
				// (0 or many is valid)


                var tableList = new List<ITable>();

                var dtFields = dm.Read(@"
select
	st.table_name,
	stf.sys_table_field_id,
	stf.sys_table_field_id,
	sdf.sys_dataview_field_id,
	stf.field_ordinal,
    sdf.table_alias_name,
	sdf.sort_order
from 
	sys_table_field stf inner join sys_table st
		on stf.sys_table_id = st.sys_table_id
	inner join sys_dataview_field sdf
		on stf.sys_table_field_id = sdf.sys_table_field_id
	inner join sys_dataview sd
		on sdf.sys_dataview_id = sd.sys_dataview_id
where
	dataview_name = :dvname
order by sdf.sort_order
", new DataParameters(":dvname", dataviewName, DbType.String));

                var aliasNames = new List<string>();
                var tblNames = new List<string>();
                foreach (DataRow drF in dtFields.Rows) {
                    var tableName = drF["table_name"].ToString();
                    var addTable = false;
                    var alias = drF["table_alias_name"].ToString();
                    if (!String.IsNullOrEmpty(alias) && !aliasNames.Contains(alias)){
                        // table name is already in our list, but the specific alias is not
                        addTable = true;
                    } else if (!tblNames.Contains(tableName + ":::" + alias)) {
                        addTable = true;
                    }
                    if (addTable) {
                        var fmt = Table.Map(tableName, dm.DataConnectionSpec, languageID, false); // brock
                        if (fmt != null) {
                            // create a clone, as we'll possibly be tweaking values in it and we don't want to affect the original values (that may be cached and not refreshed from the db) 
                            fmt = fmt.Clone();
                            fmt.AliasName = alias;
                            tableList.Add(fmt);
                            tblNames.Add(tableName + ":::" + alias);
                            if (!String.IsNullOrEmpty(alias)) {
                                aliasNames.Add(alias);
                            }
                        }
                    }
                }

                fmdv._tables = tableList;


				// we get here, we know all the tables this dataview touches.
				// we also know all the fields that belong to each of those tables (regardless of this dataview)
				// now, we need to add the dataview-specific data to those fields
				// and the list of fields that apply to this dataview
				// (i.e. a dataview may contain only a subset of the fields from a table --
				//  but we have to track all fields in case we need to provide default values.)


                var transformByFields = new List<string>();

				DataTable dtDVFields = dm.Read(@"
select
	srf.sys_table_field_id,
	case 
        when srf.is_readonly = 'Y' then 'Y'
        when stf.is_readonly = 'Y' then 'Y'
        else 'N' 
    end as is_readonly,
	case 
        when srf.is_readonly = 'I' then 'Y'
        when stf.is_readonly = 'I' then 'Y'
        else 'N' 
    end as is_readonly_on_insert,
    srf.is_primary_key,
    stf.is_primary_key as is_table_primary_key,
    srf.is_transform,
	coalesce(stf.sys_table_id, -1) as sys_table_id,
	srf.field_name as dataview_field_name,
    srf.is_transform as dv_field_transform,
	coalesce(srf.foreign_key_dataview_name, stf.foreign_key_dataview_name) as foreign_key_dataview_name,
    coalesce(srf.gui_hint, stf.gui_hint) as gui_hint,
    coalesce(srf.group_name, stf.group_name) as group_name,
    srf.table_alias_name as table_alias_name,
    srf.is_visible as dv_is_visible,
    srf.configuration_options as dv_options,
	coalesce(srfl.title, stfl.title, srf.field_name) as friendly_field_name,
    coalesce(srfl.description, stfl.description) as friendly_field_description,
    st.table_name as primary_key_table_name
from
	sys_dataview_field srf left join sys_dataview_field_lang srfl
      on srfl.sys_dataview_field_id = srf.sys_dataview_field_id
      and coalesce(srfl.sys_lang_id,:langid) = :langid
	left join sys_table_field stf 
		on stf.sys_table_field_id = srf.sys_table_field_id
    left join sys_table_field_lang stfl
        on stfl.sys_table_field_id = stf.sys_table_field_id
        and coalesce(stfl.sys_lang_id,:langid) = :langid
    left join sys_table st
        on stf.sys_table_id = st.sys_table_id
	left join sys_table_field stf2
		on stf.foreign_key_table_field_id = stf2.sys_table_field_id
	left join sys_table st2
		on st2.sys_table_id = stf2.sys_table_id
where
	srf.sys_dataview_id = :dvid
order by
	srf.sort_order
", new DataParameters(":dvid", dvID, DbType.Int32, ":langid", languageID, DbType.Int32));

                var tablesWithPrimaryKeyInDataView = new List<string>();
                var aliases = new List<string>();

				foreach (DataRow drDVFields in dtDVFields.Rows) {

					// find corresponding mapping in one of the tables we reference
                    var sysTableID = Toolkit.ToInt32(drDVFields["sys_table_id"], -1);
                    var sysFieldID = Toolkit.ToInt32(drDVFields["sys_table_field_id"], -1);
                    var aliasName = drDVFields["table_alias_name"].ToString();
					IField fm = fmdv.findField(sysTableID, aliasName, sysFieldID);
					if (fm == null) {
						// this field has no corresponding table.
						// we should still add it (but as read only)
						fm = new Field();
						fm.IsReadOnly = true;
                        fm.IsReadOnlyOnInsert = true;
					} else {

                        // 2010/10/19 Brock Weaver brock@circaware.com
                        //            To address the problem of the same table being joined in 2 or more times with the same table
                        //            as their parent (e.g. taxonomy_family's priority_site1 and priority_site2), we clone here as needed.
                        //            Note we're not cloning the table though... still working on that.
                        if (fm.Table != null && !String.IsNullOrEmpty(fm.Table.AliasName)){
                            if (!aliases.Contains(fm.Table.AliasName)) {
                                //fm = fm.Clone(fm.Table);
                                aliases.Add(fm.Table.AliasName);
                            }
                        }

						// if the table says it's read only, a dataview cannot override it.
						// if the table says it isn't read only, a dataview CAN override it.
						if (!fm.IsReadOnly) {
							fm.IsReadOnly = drDVFields["is_readonly"].ToString().ToUpper() == "Y";
						}
                        if (!fm.IsReadOnlyOnInsert) {
                            fm.IsReadOnlyOnInsert = drDVFields["is_readonly_on_insert"].ToString().ToUpper() == "Y";
                        }
					}
					if (fmdv.IsReadOnly) {
						// if the entire dataview is marked as read only, mark all fields as read only
						fm.IsReadOnly = true;
                        fm.IsReadOnlyOnInsert = true;
					}

                    fm.ConfigurationOptions = drDVFields["dv_options"].ToString();

                    fm.IsTransform = drDVFields["dv_field_transform"].ToString() == "Y";

                    fm.IsVisible = drDVFields["dv_is_visible"].ToString() == "Y";

                    fm.DataviewName = dataviewName;
                    fm.DataviewFieldName = drDVFields["dataview_field_name"].ToString();

                    if (drDVFields["is_table_primary_key"].ToString().ToUpper() == "Y") {
                        // remember which table(s) have primary key fields in the dataview
                        var tbl = drDVFields["primary_key_table_name"].ToString().ToLower();
                        if (!tablesWithPrimaryKeyInDataView.Contains(tbl)) {
                            tablesWithPrimaryKeyInDataView.Add(tbl);
                        }
                        // and remember the field that is the pk (for easy reference later)
                        if (fm.Table != null) {
                            fm.Table.PrimaryKeyDataViewFieldName = fm.DataviewFieldName;
                        }
                    }

                    if (drDVFields["is_transform"].ToString().ToUpper() == "Y"){
                        transformByFields.Add(fm.DataviewFieldName);
                    }

                    if (drDVFields["is_primary_key"].ToString().ToUpper() == "Y") {
                        fmdv.PrimaryKeyNames.Add(fm.DataviewFieldName);
                        fmdv.PrimaryKeyTableName = drDVFields["primary_key_table_name"].ToString();
                    }

					// note this changes the FieldMapping object in the FieldMappingTable itself
					// so when we do an insert we can simply spin through the table's mappings
					// it also means each dataview gets its own copy of the table's mappings!
					fm.FriendlyFieldName = drDVFields["friendly_field_name"].ToString();
                    fm.FriendlyDescription = drDVFields["friendly_field_description"].ToString();

					if (drDVFields["foreign_key_dataview_name"].ToString().Trim() != String.Empty) {
                        // only set this if the dataview level is overriding the table level one.
						fm.ForeignKeyDataviewName = drDVFields["foreign_key_dataview_name"].ToString().Trim();
                        fm.ForeignKeyDataviewParam = Dataview.DetermineForeignKeyDataviewParams(dm, fm.ForeignKeyDataviewName);
                        if (String.IsNullOrEmpty(fm.ForeignKeyTableName) || String.IsNullOrEmpty(fm.ForeignKeyTableFieldName))
                        {
                            // 2010-07-07 Brock Weaver brock@circaware.com
                            // HACK! Force CT to display text even if it's not mapped properly here.
                            // We do this by putting in a non-empty table and field name for the foreign key.
                            // We also will force it to be readonly so updates are never wrongfully attempted.
                            // If this causes problems, just comment out the following 3 lines to go back to
                            // previous behavior.
                            fm.ForeignKeyTableName = "brock_change_this";
                            fm.ForeignKeyTableFieldName = "brock_change_this_too";
                            fm.IsReadOnly = true;
                        }
                    }


                    // these exist in both sys_table_field and sys_dataview_field, but we're coalescing them
                    // so non-null ones in sys_dataview_field override those in sys_table_field.
                    fm.GuiHint = drDVFields["gui_hint"].ToString();
                    fm.GroupName = drDVFields["group_name"].ToString();

                    // table values we shouldn't overwrite (because we don't know what they are -- rely on FieldMappingTable to figure it out
                    //fm.ForeignKeyTableName = drDVFields["foreign_key_table_name"].ToString();
                    //fm.ForeignKeyTableFieldName = drDVFields["foreign_key_table_field_name"].ToString();
                    //fm.IsNullable = drDVFields["is_nullable"].ToString().ToUpper().Trim() == "Y";
                    ////fm.DataType = 
                    //fm.DataTypeString = drDVFields["field_type"].ToString();
                    //fm.DefaultValue = drDVFields["default_value"];
                    //fm.FieldPurpose = drDVFields["field_purpose"].ToString();
                    //fm.MaximumLength = Toolkit.ToInt32(drDVFields["max_length"], 0);
                    //fm.MinimumLength = Toolkit.ToInt32(drDVFields["min_length"], 0);
                    //fm.Precision = Toolkit.ToInt32(drDVFields["numeric_precision"], 0);
                    //fm.Scale = Toolkit.ToInt32(drDVFields["numeric_scale"], 0);



					fmdv.Mappings.Add(fm);

				}

                //2010-01-08 brock@circaware.com
                // Now that we've gathered up all fields and which tables they belong to,
                // we need to determine which table(s) have their PK field in the dataview.
                // If a field is marked as writable but the PK field of the table it bleongs to
                // is not in the dataview, we cannot allow that field to be writable.
                // So even if the field is marked as writable in the dataview, it can only be writable
                // if its PK field is also part of the dataview.
                // 2010-03-26 brock@circaware.com
            //     Per Pete / Mark, removed this logic for get_crop_trait_observation
            //     to allow user to change crop to change crop_trait dropdown
                //foreach (var fld in fmdv.Mappings) {
                //    if (!fld.IsReadOnly) {
                //        if (fld.Table == null // not mapped to a table
                //            || !tablesWithPrimaryKeyInDataView.Contains(("" + fld.TableName).ToLower())) { // mapped to a table, but that table's PK does not exist in this dataview
                //            fld.IsReadOnly = true;
                //            fld.IsReadOnlyOnInsert = true;
                //        }
                //    }
                //}

//                    // pull in the primary key defined for this dataview, if any
//                    DataTable dtKeys = dm.Read(@"
//select
//    srf.field_name as dv_field_name,
//    stf.field_name as table_field_name
//from
//    sys_dataview_key srk inner join sys_dataview_field srf
//        on srk.sys_dataview_field_id = srf.sys_dataview_field_id
//    inner join sys_table_field stf
//        on srf.sys_table_field_id = stf.sys_table_field_id
//where
//    srk.sys_dataview_id = :id
//order by
//    srk.sort_order
//", new DataParameters(":id", dvID));

//                    foreach (DataRow drKey in dtKeys.Rows) {
//                        fmrs.KeyNames.Add(drKey["dv_field_name"].ToString());
//                    }


                fmdv.TransformByFields = transformByFields.ToArray();

                string sqlForEngine = null;
                if (fmdv.SqlStatements.TryGetValue(dm.DataConnectionSpec.EngineName, out sqlForEngine)){
                    fmdv._sqlStatementForCurrentEngine = sqlForEngine;
                }


				// now, we need to pull sql parameters (not used for mapping data,
				// but is still associated with a resulset so we should pull it and cache it for later)
				DataTable dtParams = dm.Read(@"
select
	sys_dataview_param_id
      ,sys_dataview_id
      ,param_name
      ,param_type
      ,sort_order
      ,created_date
      ,created_by
      ,modified_date
      ,modified_by
      ,owned_date
      ,owned_by
from
	sys_dataview_param
where
	sys_dataview_id in (
		select sys_dataview_id from sys_dataview where dataview_name = :name
	)
", new DataParameters(":name", dataviewName));

				foreach (DataRow dr in dtParams.Rows) {
                    fmdv.Parameters.Add(new DataviewParameter { Name = dr["param_name"].ToString(), TypeName = dr["param_type"].ToString() });
				}

                // 2010-05-20 brock@circaware.com
                // since adding optional capability to do multi-table updates, 
                // we need to have the table list be in dependency order so the 'top' table
                // data is written first and its PK retrieved in case it is needed for a insert on a child table
                fmdv.initializeTableLists(tableList, dm, languageID);


				// add it to the cache for next time
				cache[cacheKey] = fmdv;

				return (IDataview)cache[cacheKey];
			}
		}

        /// <summary>
        /// Adds all data triggers absolutely required by the system for proper functionality.  Namely SecurityDataTrigger.
        /// </summary>
        /// <param name="fmdv"></param>
        private static void addSystemLevelDataviewDataTriggers(IDataview fmdv) {
            // the SECURITY trigger is ALWAYS added
            fmdv.SaveDataTriggers.Add(new SecurityDataTrigger());
            fmdv.ReadDataTriggers.Add(new SecurityDataTrigger());

        }

        /// <summary>
        /// Adds all data triggers defined in the sys_datatrigger table with is_enabled = 'Y' for this dataview.
        /// </summary>
        /// <param name="fmdv"></param>
        /// <param name="dm"></param>
        /// <param name="dataviewID"></param>
        private static void addAdHocDataviewDataTriggers(IDataview fmdv, DataManager dm, int dataviewID) {

            // TODO: implement object pooling so every time we Map a dataview doesn't necessarily 
            //       mean we have to load class(es) from .dll file(s)

            // now determine which trigger(s) apply to this dataview
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
	and (:dvid = sdt.sys_dataview_id or sdt.sys_dataview_id is null)
order by
	sdt.is_system desc,
	sdt.sort_order
", new DataParameters(":dvid", dataviewID, DbType.Int32));

            var iis = HttpContext.Current == null ? Toolkit.GetIISPhysicalPath("gringlobal") : string.Empty;

            foreach (DataRow drTrigger in dtTrigger.Rows) {
                // create an instance of the ISaveDataTrigger object to cache for later
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
                            if (trigger is IDataviewSaveDataTrigger) {
                                fmdv.SaveDataTriggers.Add((IDataviewSaveDataTrigger)trigger);
                            } else if (trigger is IDataviewReadDataTrigger) {
                                fmdv.ReadDataTriggers.Add((IDataviewReadDataTrigger)trigger);
                            } else if (trigger is ITableReadDataTrigger || trigger is ITableSaveDataTrigger) {
                                // do not load -- we're at the dataview level, not the table level.
                            } else {
                                throw new InvalidOperationException(getDisplayMember("addAdHocDataviewDataTriggers{nointerface}", "The class '{0}' in assembly '{1}' does not implement the GrinGlobal.Business.DataTriggers.IDataViewSaveDataTrigger or IDataViewReadDataTrigger interface.  Either the class is coded incorrectly or it should be configured as a Table DataTrigger, not a DataView DataTrigger.", fqcn, asmName));
                            }
                        }
                    } catch (FileNotFoundException fnf) {
                        if (HttpContext.Current != null) {
                            throw new InvalidOperationException(getDisplayMember("addAdHocDataviewDataTriggers{notfound}", "A data trigger is mapped in the sys_datatrigger table which points to '{0}' but that file does not exist.  Source: {1}", drTrigger["virtual_file_path"].ToString(), fnf.Message));
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
        /// Returns the field from the Mappings collection that has both the same tableFieldID and aliasName. If not found returns null.
        /// </summary>
        /// <param name="tableFieldID"></param>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public IField GetField(int tableFieldID, string aliasName) {
            var aliasLow = (string.Empty + aliasName).ToLower();
            foreach (var f in Mappings) {
                if (f.TableFieldID == tableFieldID) {
                    if (String.IsNullOrEmpty(aliasLow) || (f.Table != null && f.Table.AliasName.ToLower() == aliasLow)) {
                        return f;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Reorders the given tableList in dependency order.  Handles self-referencing and multiply aliased tables properly.  For Import Wizard Support.
        /// </summary>
        /// <param name="tableList"></param>
        /// <param name="dm"></param>
        /// <param name="languageID"></param>
        private void initializeTableLists(List<ITable> tableList, DataManager dm, int languageID) {
            if (tableList == null || tableList.Count < 2) {
                return;
            }

            var sorted = new List<ITable>();
            sorted.AddRange(tableList);

            var selfReferences = new List<string>();
            if (sorted.Count > 1) {
                // selection sort all tables to get proper hierarchical ordering
                for (var i = 0; i < sorted.Count; i++) {
                    for (var j = i + 1; j < sorted.Count; j++) {
                        if (needToFlipTables(sorted[i], sorted[j], selfReferences)) {
                            Toolkit.Swap<ITable>(sorted, i, j);
                        }
                    }
                }


            }


            this._tables = sorted;

        }

        /// <summary>
        /// Returns true if t1 needs to follow t2 in the dependency hierarchy.  otherwise returns false.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="selfReferences"></param>
        /// <returns></returns>
        private bool needToFlipTables(ITable t1, ITable t2, List<string> selfReferences) {
            var fk1 = getFieldNameFromJoinChildren(t2.AliasName, t1.AliasName);
            if (fk1 == null) {
                // t1 does not point at t2, so no dependency exists.
                // do not flip.
                return false;
            } else {

                // t1 points at t2.  

                // see if t1 is the same table name as t2.
                var fkAlias = fk1.Split('.')[0];

                if (t1.TableName.ToLower() == t2.TableName.ToLower()) {
                    // self-referential... Add it to the list for later processing. Do not flip.
                    if (fkAlias.ToLower() == t2.AliasName.ToLower()) {
                        // self-referential and its join children are saying they are in the correct order. do not flip
                        return false;
                    } else {
                        // self-referential, but in the wrong order
                        // flip them.
                        return true;
                    }
                } else {
                    // t2 may also point at t1.
                    //  e.g. taxonomy_family.type_taxonomy_genus_id -> taxonomy_genus
                    //       taxonomy_genus.taxonomy_family_id -> taxonomy_family
                    //  ALWAYS report the one that is part of the unique key list as being the 'real' child.
                    var fk2 = getFieldNameFromJoinChildren(t1.AliasName, t2.AliasName);
                    if (fk2 == null) {
                        // t2 does not point at t1.
                        // assume t1 and t2 need to flip.
                        return true;
                    } else {
                        // t1 points at t2.  t2 also points at t1.
                        if (t2.UniqueKeyFields.Split(',').Contains(fk2)) {
                            // t2's pointer to t1 is part of the unique key.
                            // so t2 is the 'real' child. Do not flip.
                            return false;
                        } else {
                            // t2's pointer to t1 is NOT part of the unique key.
                            // so t1 is the 'real' child.  Flip.
                            return true;
                        }
                    }
                }
            }
        }

        private string getFieldNameFromJoinChildren(string childAliasName, string parentAliasName) {
            var rv = new string[0];
            foreach (var m in _mappings) {
                if (m.Table != null && m.Table.AliasName == childAliasName && !String.IsNullOrEmpty(m.ConfigurationOptions)) {
                    // if this field says one of its join children matches the exact alias, assume this is the right one.
                    if (!String.IsNullOrEmpty(m.ConfigurationOptions)) {
                        var jclist = Toolkit.ParsePairs<string>(m.ConfigurationOptions);
                        string jcarray = null;
                        if (jclist.TryGetValue("join_children", out jcarray)) {
                            var jcs = jcarray.Split(',');
                            foreach (var jc in jcs) {
                                if (jc.StartsWith(parentAliasName + ".")) {
                                    return jc;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Not currently used.
        /// </summary>
        /// <param name="table1ID"></param>
        /// <param name="table2ID"></param>
        /// <param name="dm"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        private static ITable QueryForJoinTable(int table1ID, int table2ID, DataManager dm, int languageID) {

            var name = dm.ReadValue(@"
select
	stfk.table_name
from 
	sys_table_field stf 
	left join sys_table_field stffk
		on stffk.foreign_key_table_field_id = stf.sys_table_field_id
	left join sys_table stfk
		on stffk.sys_table_id = stfk.sys_table_id
where 
	stf.sys_table_id in (:table1, :table2)
	and (stf.is_primary_key = 'Y')
group by stfk.table_name
having COUNT(1) = 2
", new DataParameters(":table1", table1ID, DbType.Int32, ":table2", table2ID, DbType.Int32)) + string.Empty;

            if (!String.IsNullOrEmpty(name) && name.ToLower().EndsWith("_map")) {
                var rv = Table.Map(name, dm, languageID, true); // brock
                if (rv != null) {
                    rv = rv.Clone();
                }
                return rv;
            }

            return null;

        }

        /// <summary>
        /// Not currently used.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sorted"></param>
        /// <param name="unsorted"></param>
        /// <returns></returns>
        private static ITable FindJoinTable(ITable target, List<ITable> sorted, List<ITable> unsorted) {

            // so what we do first is spin through the existing tables and see if we can find it.

            // example:
            // target   = citation
            // sorted   = accession, cooperator
            // unsorted = citation, citation_map, literature

            var candidates = new List<ITable>();

            // grab all tables from the unsorted list that are a parent of our target (i.e. target points at it)
            foreach (var t2 in unsorted) {
                if (String.Compare(t2.TableName, target.TableName, true) != 0) {
                    // 2010-08-11 Brock Weaver brock@circaware.com
                    //            Added the restriction of the 'many to many' table requiring to end with '_map'
                    //            Though not the best approach, it is the most consistent one I can find.
                    //            Without this restriction, several tables are wrongly identified as being 'map' tables
                    //            so this makes it much more efficient for SaveData() processing yet still works for
                    //            all tables following our naming convention.
                    if (t2.TableName.ToLower().EndsWith("_map")) {
                        if (target.IsParentOf(t2)) {
                            candidates.Add(t2);
                        }
                    }
                }
            }

            // now, in our example:
            // candidates = citation_map


            if (candidates.Count > 0) {
                // now, all the ones in the candidates list are parents of our target.
                // for us to pick the correct one, the candidate must also be a parent of one of the tables
                // in the sorted list as well.
                foreach (var t1 in sorted) {
                    if (String.Compare(t1.TableName, target.TableName, true) != 0) {
                        foreach (var c in candidates) {
                            if (t1.IsParentOf(c)) {
                                return c;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Not currently used.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int? FindIntValue(DataTriggerHelper helper, string columnName) {
            columnName = columnName.ToLower();
            int? rv = Toolkit.ToInt32(helper.GetValue(columnName, null, false), (int?)null);
            if (rv == null) {
                var found = false;
                foreach (IField fld in this.Mappings) {
                    if (fld.TableFieldName == columnName) {
                        rv = (int?)(helper.GetValue(fld.DataviewFieldName, rv, false));
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    // not in field mappings, but may have been appended by earlier processing...
                    for (var i = this.Mappings.Count; i < helper.SavingRow.Table.Columns.Count; i++) {
                        var dc = helper.SavingRow.Table.Columns[i];

                        if (dc.ColumnName.ToLower().EndsWith("." + columnName)) {
                            rv = Toolkit.ToInt32(helper.SavingRow[dc], rv);
                        }

                        string children = null;
                        if (dc.ExtendedProperties.ContainsKey("join_children")) {
                            children = dc.ExtendedProperties["join_children"] as string;
                        }

                        if (children != null) {
                            var clist = children.Split(',');
                            foreach (var c in clist) {
                                if (c.ToLower().EndsWith("." + columnName)) {
                                    rv = Toolkit.ToInt32(helper.SavingRow[dc], rv);
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            return rv;
        }


        /// <summary>
        /// Returns the DataviewName property.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return this.DataViewName;
        }

        /// <summary>
        /// Given a dataviewname, returns a string containing all defined parameters which is in the proper format to pass to GetData().  Example return value is: ":field1=__field1__;:field2=__field2__"
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="foreignKeyDataviewName"></param>
        /// <returns></returns>
        public static string DetermineForeignKeyDataviewParams(DataManager dm, string foreignKeyDataviewName) {
            if (String.IsNullOrEmpty(foreignKeyDataviewName)) {
                return String.Empty;
            } else {

                var cm = CacheManager.Get("dataview_params");

                var cachedParams = cm[foreignKeyDataviewName] as string;

                if (String.IsNullOrEmpty(cachedParams)) {

                    // need to pull all parameters associated with the foreign key dataview name
                    StringBuilder sb = new StringBuilder();
                    DataTable rsParams = dm.Read(@"
select 
	param_name
from 
	sys_dataview_param srp left join sys_dataview sr 
		on srp.sys_dataview_id = sr.sys_dataview_id 
where 
	sr.dataview_name = :fkrsname 
order by 
	srp.sort_order
", new DataParameters(":fkrsname", foreignKeyDataviewName));
                    //DataTable rsParams = dm.Read("select * from sys_dataview_param where sys_dataview_id = :id order by sort_order", new DataParameters(":id", dr["sys_dataview_id"]));
                    for (int k = 0; k < rsParams.Rows.Count; k++) {
                        DataRow drParam = rsParams.Rows[k];
                        sb.Append(drParam["param_name"].ToString())
                            .Append("=__")
                            .Append(drParam["param_name"].ToString().Replace(":", "").Replace("@", "").Replace("?", ""))
                            .Append("__");
                        if (k < rsParams.Rows.Count - 1) {
                            sb.Append(";");
                        }
                    }
                    // param looks like ":field1=__field1__;:field2=__field2__"
                    cachedParams = sb.ToString();
                    cm[foreignKeyDataviewName] = cachedParams;
                }
                return cachedParams;
            }
        }



        /// <summary>
        /// Calls Dataview.Fill(), then returns the first Table object in the Tables collection for that dataview
        /// </summary>
        /// <param name="dataviewName"></param>
        /// <param name="languageID"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
		public static ITable MapFirst(string dataviewName, int languageID, DataManager dm) {
			var fmdv = Dataview.Fill(dataviewName, languageID, dm);
			if (fmdv.Tables.Count > 0) {
				return fmdv.Tables[0];
			} else {
				return null;
			}
		}

        /// <summary>
        /// Simply calls Dataview.Fill() and returns its value.
        /// </summary>
        /// <param name="dataviewName"></param>
        /// <param name="languageID"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
		public static IDataview Map(string dataviewName, int languageID, DataManager dm){
			var fmdv = Dataview.Fill(dataviewName, languageID, dm);
            return fmdv;
		}

        /// <summary>
        /// Returns language-friendly messages / errors / text / etc.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="substitutes"></param>
        /// <returns></returns>
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "Dataview", resourceName, null, defaultValue, substitutes);
        }
    }
}
