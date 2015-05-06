using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using GrinGlobal.Interface.DataTriggers;
using System.Web;

namespace GrinGlobal.Interface.Dataviews {
	public interface ITable {
		List<IField> Mappings { get; }

		string UniqueKeyFields { get; set; }
		int TableID { get; set; }
		string TableName { get; set; }
        string AliasName { get; set; }
		bool IsReadOnly { get; set; }
		bool AuditsCreated { get; set; }
		bool AuditsModified { get; set; }
		bool AuditsOwned { get; set; }
        string PrimaryKeyFieldName { get; set; }
        string PrimaryKeyDataViewFieldName { get; set; }

        string GetSQL(bool addAllFields);

        List<ITableSaveDataTrigger> SaveDataTriggers { get; set; }
        List<ITableReadDataTrigger> ReadDataTriggers { get; set; }

        IField GetField(string fieldName);

        List<IField> ForeignKeys { get; }

        ITable Clone();

        Dictionary<string, object> ExtendedProperties { get; }

        bool IsParentOf(ITable table);
	}
}
