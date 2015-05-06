using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.Dataviews {
	public interface IField {
		string DataviewName { get; set; }
		string DataviewFieldName { get; set; }

		int TableFieldID { get; set; }
		string TableName { get; set; }
		string TableFieldName { get; set; }
        string AliasedTableFieldName { get; }
		ITable Table { get; set; }

        Dictionary<string, object> ExtendedProperties { get; }

		string FieldPurpose { get; set; }

		string DataTypeString { get; set; }
		Type DataType { get; set; }

		bool IsAutoIncrement { get; set; }
		bool IsForeignKey { get; set; }
		bool IsNullable { get; set; }
		bool IsPrimaryKey { get; set; }
		bool IsReadOnly { get; set; }
        bool IsReadOnlyOnInsert { get; set; }

		string ForeignKeyTableName { get; set; }
		string ForeignKeyTableFieldName { get; set; }
		string GuiHint { get; set; }

		string ForeignKeyDataviewName { get; set; }
		string ForeignKeyDataviewParam { get; set; }

		string GroupName { get; set; }

		int MinimumLength { get; set; }
		int MaximumLength { get; set; }
		int Precision { get; set; }
		int Scale { get; set; }

		object DefaultValue { get; set; }

		string FriendlyFieldName { get; set; }
        string FriendlyDescription { get; set; }

        Dictionary<int, string> DataviewFriendlyFieldNames { get; }

        int DataviewID { get; set; }
        int DataviewFieldID { get; set; }
        bool IsTransform { get; set; }

		//public bool IsPrimaryKeyField {
		//    get {
		//        return FieldPurpose == "PRIMARY_KEY";
		//    }
		//}

		 bool IsCreatedOrModifiedDate { get; }

		 bool IsCreatedBy { get; }

		 bool IsCreatedDate { get; }

		 bool IsModifiedBy { get; }

		 bool IsModifiedDate { get; }

		bool IsCreatedOrModifiedBy { get; }

		bool IsOwnedBy { get; }

		bool IsOwnedDate { get; }

		bool IsAudit { get; }

		IField Clone(ITable newTable);

        bool IsVisible { get; set; }

        string ConfigurationOptions { get; set; }
	}
}
