using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GrinGlobal.Core;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business {
    /// <summary>
    /// Represents a field definition in its entirety.  The field may be part of a table mapping or a dataview definition.  Note not all properties are set at all times (table mappings do not set DataviewName, for instance, because it makes no sense in that context)
    /// </summary>
	public class Field : IField {

        public int DataviewID { get; set; }
        public int DataviewFieldID { get; set; }
        public string DataviewName { get; set; }
		public string DataviewFieldName { get; set; }

		public int TableFieldID { get; set; }
		public string TableName { get; set; }
		public string TableFieldName { get; set; }

        public Dictionary<string, object> ExtendedProperties { get; private set; }

		public ITable Table { get; set; }

		public string FieldPurpose { get; set; }

		public string DataTypeString { get; set; }
		public Type DataType { get; set; }

		public bool IsAutoIncrement { get; set; }
		public bool IsForeignKey { get; set; }
		public bool IsNullable { get; set; }
		public bool IsPrimaryKey { get; set; }
		public bool IsReadOnly { get; set; }
        public bool IsReadOnlyOnInsert { get; set; }
        public bool IsTransform { get; set; }
        public bool IsVisible { get; set; }
        /// <summary>
        /// Gets or sets ConfigurationOptions.  Currently used only to store "join_children", which the Import Wizard dataviews use to associate a subset of fields with another particular subset of fields in the same dataview.  Example is the import_taxonomy_species dataview, which may have cooperator table joined in multiple
        /// times.  To be able to say "the geography information for this subset of fields is associated with the c1 alias, not the c2 alias", the join_children will be set appropriately to specify this.
        /// </summary>
        public string ConfigurationOptions { get; set; }

		public string ForeignKeyTableName { get; set; }
		public string ForeignKeyTableFieldName { get; set; }

        private Dictionary<int, string> _dataViewFriendlyFieldNames;
        public Dictionary<int, string> DataviewFriendlyFieldNames {
            get {
                if (_dataViewFriendlyFieldNames == null) {
                    _dataViewFriendlyFieldNames = new Dictionary<int, string>();
                }
                return _dataViewFriendlyFieldNames;
            }
        }
		public string GuiHint { get; set; }

		public string ForeignKeyDataviewName { get; set; }
		public string ForeignKeyDataviewParam { get; set; }

		public string GroupName { get; set; }

		public int MinimumLength { get; set; }
		public int MaximumLength { get; set; }
		public int Precision { get; set; }
		public int Scale { get; set; }

		public object DefaultValue { get; set; }

		private string _friendlyFieldName;
		/// <summary>
		/// Gets or sets the friendly field name (language-specific text)
		/// </summary>
		public string FriendlyFieldName { 
			get {
				/// Gets or sets the friendly field name.  If it is null or empty, replaces underscores with spaces in the DatabaseFieldName and TitleCases it and returns it instead.
				//if (String.IsNullOrEmpty(_friendlyFieldName) && !String.IsNullOrEmpty(DatabaseFieldName)){
				//    _friendlyFieldName = DatabaseFieldName.Replace("_", " ").ToTitleCase();
				//}
				return _friendlyFieldName;
			}
			set {
				_friendlyFieldName = value;
			}
		}

        /// <summary>
        /// Returns the full field name (table.fieldname), using the table alias if applicable (so site.site_id will return as s1.site_id assuming s1 is its alias).  i.e. if site is not aliased at all, returns site.site_id.
        /// </summary>
        public string AliasedTableFieldName {
            get {
                if (Table != null) {
                    if (String.IsNullOrEmpty(Table.AliasName)) {
                        return Table.TableName + "." + TableFieldName;
                    } else {
                        return Table.AliasName + "." + TableFieldName;
                    }
                } else {
                    return TableFieldName;
                }
            }
        }

        public string FriendlyDescription { get; set; }


		//public bool IsPrimaryKeyField {
		//    get {
		//        return FieldPurpose == "PRIMARY_KEY";
		//    }
		//}

		public bool IsCreatedOrModifiedDate {
			get {
				return FieldPurpose.StartsWith("AUTO_DATE");
			}
		}

		public bool IsCreatedBy {
			get {
				return FieldPurpose == "AUTO_ASSIGN_CREATE";
			}
		}

		public bool IsCreatedDate {
			get {
				return FieldPurpose == "AUTO_DATE_CREATE";
			}
		}

		public bool IsModifiedBy {
			get {
				return FieldPurpose == "AUTO_ASSIGN_MODIFY";
			}
		}

		public bool IsModifiedDate {
			get {
				return FieldPurpose == "AUTO_DATE_MODIFY";
			}
		}

		public bool IsCreatedOrModifiedBy {
			get {
				return FieldPurpose.StartsWith("AUTO_ASSIGN");
			}
		}

		public bool IsOwnedBy {
			get {
				return FieldPurpose == "AUTO_ASSIGN_OWN";
			}
		}

		public bool IsOwnedDate {
			get {
				return FieldPurpose == "AUTO_DATE_OWN";
			}
		}

		//public bool _markedAsReadOnly;
		//public void MakeReadOnly(bool readOnly) {
		//    _markedAsReadOnly = readOnly;
		//}
		//public bool IsReadOnly {
		//    get {
		//        //return IsAutoIncrement ||
		//        //    IsPrimaryKeyField ||
		//        //    IsAuditField ||
		//        //    _markedAsReadOnly ||
		//        //    FieldPurpose == "READ_ONLY";
		//    }
		//}


		public bool IsAudit {
			get {
				return IsCreatedBy ||
					IsCreatedDate ||
					IsModifiedBy ||
					IsModifiedDate ||
					IsOwnedBy ||
					IsOwnedDate;
			}
        }

        public Field() {
            ExtendedProperties = new Dictionary<string, object>();
            this.DataviewFieldID = -1;
            this.DataviewID = -1;
            this.TableFieldID = -1;
            this.IsVisible = true;
        }

		public override string ToString() {
			return "Dataview: " + this.DataviewName + "." + this.DataviewFieldName + " ; Table (" + (this.Table == null ? "-1/-" : this.Table.TableID + "/" + this.Table.AliasName) + "): " + this.TableName + "." + this.TableFieldName + " (" + this.TableFieldID + "); Friendly: " + FriendlyFieldName + " ; Default Value: " + DefaultValue;
		}

        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        /// <param name="newTable"></param>
        /// <returns></returns>
		public IField Clone(ITable newTable) {
			Field fm = new Field();
			fm.GroupName = this.GroupName;
			fm.DataType = this.DataType;
			fm.DataTypeString = this.DataTypeString;
			fm.DefaultValue = this.DefaultValue;
			fm.FieldPurpose = this.FieldPurpose;
			fm.ForeignKeyTableFieldName = this.ForeignKeyTableFieldName;
			fm.ForeignKeyDataviewName = this.ForeignKeyDataviewName;
			fm.ForeignKeyDataviewParam = this.ForeignKeyDataviewParam;
			fm.ForeignKeyTableName = this.ForeignKeyTableName;
			fm.FriendlyFieldName = this.FriendlyFieldName;
			fm.GuiHint = this.GuiHint;
            foreach (var key in this.DataviewFriendlyFieldNames.Keys) {
                fm.DataviewFriendlyFieldNames[key] = this.DataviewFriendlyFieldNames[key];
            }

			fm.IsAutoIncrement = this.IsAutoIncrement;
			fm.IsForeignKey = this.IsForeignKey;
			fm.IsNullable = this.IsNullable;
			fm.IsPrimaryKey = this.IsPrimaryKey;
			fm.IsReadOnly = this.IsReadOnly;
            fm.IsReadOnlyOnInsert = this.IsReadOnlyOnInsert;

            fm.IsVisible = this.IsVisible;
            fm.ConfigurationOptions = this.ConfigurationOptions;

			fm.MaximumLength = this.MaximumLength;
			fm.MinimumLength = this.MinimumLength;
			fm.Precision = this.Precision;
			fm.DataviewFieldName = this.DataviewFieldName;
			fm.DataviewName = this.DataviewName;
			fm.Scale = this.Scale;
			fm.Table = newTable; // don't copy the reference! use given parameter
			fm.TableFieldID = this.TableFieldID;
			fm.TableFieldName = this.TableFieldName;
			fm.TableName = this.TableName;
			return fm;
		}


        #region IFieldMapping Members


        #endregion
    }
}
