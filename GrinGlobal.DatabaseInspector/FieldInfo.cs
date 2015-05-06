using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GrinGlobal.Core;
using GrinGlobal.DatabaseInspector.MySql;
using GrinGlobal.DatabaseInspector.SqlServer;
using GrinGlobal.DatabaseInspector.Oracle;
using System.Data;
using GrinGlobal.DatabaseInspector.PostgreSql;

namespace GrinGlobal.DatabaseInspector {
	public abstract class FieldInfo : BaseInfo, IDisposable {

		public static FieldInfo GetInstance(TableInfo parent) {
			DataVendor vendor = DataManager.GetVendor(parent.DataConnectionSpec);
			switch (vendor) {
				case DataVendor.MySql:
					return new MySqlFieldInfo(parent);
				case DataVendor.SqlServer:
					return new SqlServerFieldInfo(parent);
				case DataVendor.Oracle:
				case DataVendor.ODBC:
					return new OracleFieldInfo(parent);
                case DataVendor.PostgreSql:
                    return new PostgreSqlFieldInfo(parent);
			}

			throw new NotImplementedException(getDisplayMember("GetInstance", "FieldInfo class for {0} is not mapped through FieldInfo.GetInstance().", vendor.ToString()));

		}

		public override string ToString() {
			return this.TableName.ToLower() + "." + this.Name.ToLower() + " = " + this.Value;
		}

		public DbType DbType { get; set; }
        public DbPseudoType DbPseudoType { get; set; }
        private Type _type;
		public Type DataType {

			get {
				return _type;
			}
			set {
				DbType = DataParameter.MapDbType(value);
                DbPseudoType = DataParameter.MapDbPseudoType(value);
				_type = value;
			}
		}

		public object DefaultValue { get; set; }

		private string _name;
		public virtual string Name {
			get {
				return _name;
			}
			set {
				_name = (value == null ? null : value.ToLower());
			}
		}

		protected string UnsignedString { get; set; }
		protected string NullString { get; set; }
		public object Value { get; set; }

		public TableInfo Table { get; set; }

		public string CharacterSet { get; set; }
		public int Scale { get; set; }
		public int Precision { get; set; }

		public bool IsNullable { get; set; }

		public bool IsPrimaryKey { get; set; }
        public bool IsTransform { get; set; }
        public bool IsAutoIncrement { get; set; }

		public int? CodeGroupID { get; set; }

		public bool IsUnsigned { get; set; }
		public bool IsZeroFill { get; set; }

		public bool IsCreatedBy { get; set; }
		public bool IsCreatedDate { get; set; }
		public bool IsModifiedBy { get; set; }
		public bool IsModifiedDate { get; set; }
		public bool IsOwnedBy { get; set; }
		public bool IsOwnedDate { get; set; }

		public bool IsForeignKey { get; set; }

		public TableInfo ForeignKeyTable { get; set; }

		public string ForeignKeyConstraintName { get; set; }

		private string _foreignKeyTableName;
		public string ForeignKeyTableName {
			get {
				return _foreignKeyTableName;
			}
			set {
				_foreignKeyTableName = (value == null ? null : value.ToLower());
			}
		}

		private string _foreignKeyFieldName;
		public string ForeignKeyFieldName {
			get {
				return _foreignKeyFieldName;
			}
			set {
				_foreignKeyFieldName = (value == null ? null : value.ToLower());
			}
		}

		private string _foreignKeyDataviewName;
		public string ForeignKeyDataviewName {
			get {
				return _foreignKeyDataviewName;
			}
			set {
				_foreignKeyDataviewName = (value == null ? null : value.ToLower());
			}
		}

        /// <summary>
        /// Gets or sets maxlength of field (used for strings only).  -1 means unlimited / maximum allowed by database / etc
        /// </summary>
		public int MaxLength { get; set; }
		public int MinLength { get; set; }

		/// <summary>
		/// Returns PRIMARY_KEY, AUTO_DATE_CREATE, AUTO_ASSIGN_CREATE, AUTO_DATE_MODIFY, AUTO_ASSIGN_MODIFY, AUTO_DATE_OWN, AUTO_ASSIGN_OWN, or DATA
		/// </summary>
		public string Purpose {
			get {
				if (IsPrimaryKey) {
					return "PRIMARY_KEY";
				} else {
					string nm = Name.ToUpper();
					switch (nm) {
						case "CREATED":
						case "CREATED_AT":
						case "CREATED_DATE":
							return "AUTO_DATE_CREATE";
						case "CREATED_BY":
							return "AUTO_ASSIGN_CREATE";
						case "MODIFIED":
						case "MODIFIED_AT":
						case "MODIFIED_DATE":
							return "AUTO_DATE_MODIFY";
						case "MODIFIED_BY":
							return "AUTO_ASSIGN_MODIFY";
						case "OWNED":
						case "OWNED_AT":
						case "OWNED_DATE":
							return "AUTO_DATE_OWN";
						case "OWNED_BY":
							return "AUTO_ASSIGN_OWN";
						default:
							return "DATA";
					}
				}
			}
		}

		/// <summary>
		/// Returns INTEGER, DATETIME, STRING, DECIMAL, DOUBLE, FLOAT, or LONG
		/// </summary>
		public string FieldType {
			get {
				if (DataType == typeof(int)) {
					return "INTEGER";
				} else if (DataType == typeof(DateTime)) {
					return "DATETIME";
				} else if (DataType == typeof(string)) {
					return "STRING";
				} else if (DataType == typeof(decimal)) {
					return "DECIMAL";
                } else if (DataType == typeof(long)){
                    return "LONG";
				} else if (DataType == typeof(double)) {
					return "DOUBLE";
				} else if (DataType == typeof(float)) {
					return "FLOAT";
				} else {
					return "STRING";
				}
			}
		}

		private Options _options;

//		public TableInfo Table { get; set; }

		public void AddOptions(Options options) {
			parseOptions(options);
		}

		public void AddOptions(params object[] keyValuePairs) {
			parseOptions(new Options(keyValuePairs));
		}

		protected FieldInfo(BaseInfo parent) : this (parent, null){
		}

		protected FieldInfo(BaseInfo parent, Options options)
			: base(parent.DataConnectionSpec) {
			Table = (TableInfo)parent;
			parseOptions(options);
			SyncAction = SyncAction.Create;
            if (parent != null) {
                _delimiter = parent.Delimiter;
                _schemaName = parent.SchemaName;
            }
        }


		protected virtual void parseOptions(Options options) {
			if (options == null) {
				return;
			}
			foreach (string key in options.Keys) {
				object val = options[key];
				switch (key.ToLower()) {
					case "dbtype":
					case "databasetype":
						DbType = (DbType)val;
						break;
					case "datatype":
					case "dotnetdatatype":
					case "dotnettype":
					case ".nettype":
					case "type":
						DataType = (Type)val;
						break;
					case "isprimarykey":
					case "pk":
					case "primarykey":
						IsPrimaryKey = (bool)val;
						break;
					case "isautoincrement":
					case "autoincrement":
					case "autoinc":
					case "inc":
						IsAutoIncrement = (bool)val;
						break;
                    case "transform":
                    case "istransform":
                        IsTransform = (bool)val;
                        break;
					case "isnullable":
					case "nullable":
					case "null":
					case "nullability":
						IsNullable = (bool)val;
						break;
					case "minlength":
						MinLength = (int)val;
						break;
					case "maxlength":
						MaxLength = (int)val;
						break;
					case "name":
					case "fieldname":
						Name = ((string)val).ToLower();
						parseAutoField(Name);
						break;
					case "tablename":
						TableName = (string)val;
						break;
					case "scale":
						Scale = (int)val;
						break;
					case "precision":
						Precision = (int)val;
						break;
					case "charset":
						CharacterSet = (string)val;
						break;
					case "createdat":
					case "created":
						IsCreatedDate = (bool)val;
						break;
					case "createdby":
						IsCreatedBy = (bool)val;
						break;
					case "modifiedat":
					case "modified":
						IsModifiedDate = (bool)val;
						break;
					case "modifiedby":
						IsModifiedBy = (bool)val;
						break;
					case "ownedat":
					case "owned":
						IsOwnedDate = (bool)val;
						break;
					case "ownedby":
						IsOwnedBy = (bool)val;
						break;
					case "isunsigned":
					case "unsigned":
						IsUnsigned = (bool)val;
						break;
					case "iszerofill":
					case "zerofill":
						IsZeroFill = (bool)val;
						break;
					case "defaultvalue":
						DefaultValue = val;
						break;
				}
			}
			_options += options;
		}

		private void parseAutoField(string name) {
			switch (name.ToLower()) {
				case "created_at":
				case "createdat":
				case "created_date":
				case "createddate":
				case "created":
					IsCreatedDate = true;
					break;
				case "created_by":
				case "createdby":
					IsCreatedBy = true;
					break;
				case "modified_date":
				case "modifieddate":
				case "modified_at":
				case "modifiedat":
				case "modified":
					IsModifiedDate = true;
					break;
				case "modified_by":
				case "modifiedby":
					IsModifiedBy = true;
					break;
				case "owned_at":
				case "ownedat":
				case "owned":
				case "owned_date":
				case "owneddate":
					IsOwnedDate = true;
					break;
				case "owned_by":
				case "ownedby":
					IsOwnedBy = true;
					break;
			}
		}

		protected abstract string getUnsignedString();

		protected abstract string getNullString();

		protected abstract string getZerofillString();

		protected abstract string getAutoIncrementString();

		protected abstract string getDefaultValueString();

		/// <summary>
		/// Returns the succinct type of this field.  e.g. CHAR(2), INTEGER, VARCHAR2(50), DECIMAL(9,2), etc
		/// </summary>
		/// <returns></returns>
		internal abstract string GetDbTypeString();

		protected abstract string getCharacterSetString();

		public abstract string GenerateCreateSql();


		public FieldInfo Clone(TableInfo newParent) {
			FieldInfo fi = FieldInfo.GetInstance(newParent);
			fi._name = this._name;
			fi._options = this._options;
			fi.CharacterSet = this.CharacterSet;
			fi.CodeGroupID = this.CodeGroupID;
			fi.DataConnectionSpec = this.DataConnectionSpec;
			fi.DataType = this.DataType;
			fi.DefaultValue = this.DefaultValue;
			fi.ForeignKeyTable = this.ForeignKeyTable;
			fi.ForeignKeyConstraintName = this.ForeignKeyConstraintName;
			fi.ForeignKeyFieldName = this.ForeignKeyFieldName;
			fi.ForeignKeyDataviewName = this.ForeignKeyDataviewName;
			fi.ForeignKeyTableName = this.ForeignKeyTableName;
			fi.IsAutoIncrement = this.IsAutoIncrement;
			fi.IsCreatedDate = this.IsCreatedDate;
			fi.IsCreatedBy = this.IsCreatedBy;
			fi.IsForeignKey = this.IsForeignKey;
			fi.IsModifiedDate = this.IsModifiedDate;
			fi.IsModifiedBy = this.IsModifiedBy;
			fi.IsNullable = this.IsNullable;
			fi.IsOwnedDate = this.IsOwnedDate;
			fi.IsOwnedBy = this.IsOwnedBy;
			fi.IsPrimaryKey = this.IsPrimaryKey;
            fi.IsTransform = this.IsTransform;
			fi.IsUnsigned = this.IsUnsigned;
			fi.IsZeroFill = this.IsZeroFill;
			fi.MaxLength = this.MaxLength;
			fi.MinLength = this.MinLength;
			fi.Name = this.Name;
			fi.NullString = this.NullString;
			fi.Precision = this.Precision;
			fi.Scale = this.Scale;
			fi.TableName = this.TableName;
			fi.UnsignedString = this.UnsignedString;
			fi.Value = this.Value;
			return fi;
		}

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}

			string def1 = generateCreateSql();
			string def2 = ((FieldInfo)obj).generateCreateSql();

			return def1 == def2;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

		public static bool operator ==(FieldInfo a, FieldInfo b) {
			if ((object)a == null) {
				return (object)b == null;
			} else {
				return a.Equals(b);
			}
		}

		public static bool operator !=(FieldInfo a, FieldInfo b) {
			return !(a == b);
		}


		public FieldInfo FillFromDataSet(DataRow dr, DataRow drRef, List<TableInfo> tables) {
			TableName = dr["table_name"].ToString().ToLower();

			string fieldName = dr["field_name"].ToString().ToLower();
			string fieldType = dr["field_type"].ToString().ToLower();
			object defaultValue = dr["default_value"];
			int minlength = Toolkit.ToInt32(dr["minlength"], 0);
			int maxlength = Toolkit.ToInt32(dr["maxlength"], 0);
			int precision = Toolkit.ToInt32(dr["precision"], 0);
			int scale = Toolkit.ToInt32(dr["scale"], 0);
			string characterSet = dr["charset"].ToString();
			bool nullable = Toolkit.ToBoolean(dr["is_nullable"], false);
			bool primaryKey = dr["field_purpose"].ToString().ToLower() == "primary_key";
			bool unsigned = Toolkit.ToBoolean(dr["is_unsigned"], false);
			bool zerofill = Toolkit.ToBoolean(dr["is_zerofill"], false);
			bool autoIncrement = Toolkit.ToBoolean(dr["is_autoincrement"], false);
			bool createdAt = dr["field_purpose"].ToString().ToLower() == "auto_date_create";
			bool createdBy = dr["field_purpose"].ToString().ToLower() == "auto_assign_create";
			bool modifiedAt = dr["field_purpose"].ToString().ToLower() == "auto_date_modify";
			bool modifiedBy = dr["field_purpose"].ToString().ToLower() == "auto_assign_modify";
			bool ownedAt = dr["field_purpose"].ToString().ToLower() == "auto_date_own";
			bool ownedBy = dr["field_purpose"].ToString().ToLower() == "auto_assign_own";

			switch (fieldType) {
				case "integer":
					DataType = typeof(int);
					break;
                case "long":
                    DataType = typeof(long);
                    break;
				case "string":
					DataType = typeof(string);
					break;
				case "datetime":
				case "datetime2":
					DataType = typeof(DateTime);
					break;
				case "float":
					DataType = typeof(float);
					break;
				case "decimal":
					DataType = typeof(Decimal);
					break;
				case "double":
					DataType = typeof(Double);
					break;
				default:
                    throw new InvalidOperationException(getDisplayMember("FillFromDataSet", "Fields of type '{0}' are not supported.\nYou must change the type for {1} before continuing or add the code to support that type to GrinGlobal.DatabaseInspector.\nSupported types are:\nint\nvarchar\nchar\nnvarchar\nnchar\ndatetime\nfloat\ndouble\ndecimal\n", fieldType, fieldName));
			}


			AddOptions("name", fieldName,
					"nullable", nullable,
					"minlength", minlength,
					"maxlength", maxlength,
					"primarykey", primaryKey,
					"scale", scale,
					"precision", precision,
					"charset", characterSet,
					"unsigned", unsigned,
					"zerofill", zerofill,
					"autoincrement", autoIncrement,
					"createdby", createdBy,
					"createdat", createdAt,
					"modifiedby", modifiedBy,
					"modifiedat", modifiedAt,
					"ownedby", ownedBy,
					"ownedat", ownedAt);

			if (defaultValue != null) {
				if (defaultValue.ToString() == "{DBNull.Value}") {
					AddOptions("defaultValue", DBNull.Value);
				//} else if (defaultValue.GetType().ToString().ToLower() == "system.string") {
				//    AddOptions("defaultvalue", @"""" + defaultValue + @"""");
				} else {
					AddOptions("defaultvalue", defaultValue.ToString());
				}
			}

			if (drRef != null) {
				// fill data about referencing table & field
				IsForeignKey = true;
				ForeignKeyFieldName = drRef["field_name"].ToString().ToLower();
				ForeignKeyTableName = drRef["table_name"].ToString().ToLower();
			}

			return this;
		}


		public DataRow FillDataSetRow(int parentID, string tableName, DataRow dr) {
			dr["id"] = dr.Table.Rows.Count;
			dr["parent_id"] = parentID;
			dr["table_name"] = tableName.ToLower();
			dr["field_name"] = Name.ToLower();
			dr["field_type"] = FieldType;
			dr["field_purpose"] = Purpose;
            if (DefaultValue == DBNull.Value || (string.Empty + DefaultValue) == "{DBNull.Value}" || (string.Empty + DefaultValue).Length == 0) {
                dr["default_value"] = "{DBNull.Value}";
            } else {
                dr["default_value"] = DefaultValue;
            }
			dr["charset"] = CharacterSet;
			dr["is_autoincrement"] = IsAutoIncrement;
			dr["is_nullable"] = IsNullable;
			dr["is_unsigned"] = IsUnsigned;
			dr["is_zerofill"] = IsZeroFill;
			//dr["is_created_date"] = IsCreatedAt;
			//dr["is_created_by"] = IsCreatedBy;
			//dr["is_modified_date"] = IsModifiedAt;
			//dr["is_modified_by"] = IsModifiedBy;
			//dr["is_owned_date"] = IsOwnedAt;
			//dr["is_owned_by"] = IsOwnedBy;
			dr["minlength"] = MinLength;
			dr["maxlength"] = MaxLength;
			dr["precision"] = Precision;
			dr["scale"] = Scale;
			dr["comment"] = null;
			return dr;
		}

		public void Dispose() {
		}

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "DatabaseInspector", "FieldInfo", resourceName, null, defaultValue, substitutes);
        }

	}
}
