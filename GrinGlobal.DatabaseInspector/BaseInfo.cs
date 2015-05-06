using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Core;
//using GrinGlobal.Sql;
//using GrinGlobal.Business;
namespace GrinGlobal.DatabaseInspector {
	public abstract class BaseInfo {

		public BaseInfo(DataConnectionSpec dcs) {
			DataConnectionSpec = dcs;
			SyncAction = SyncAction.Create;
            _delimiter = "`";
		}

		protected string _delimiter;
		public virtual string Delimiter {
			get {
                return _delimiter;
			}
		}

		protected string _schemaName;
		public virtual string SchemaName {
			get {
				return _schemaName;
			}
			set {
				_schemaName = (value == null ? null : value.ToLower());
			}
		}

        /// <summary>
        /// Gets the fully qualified name, with proper quoting, of the table.  e.g. sql server -> 'dbo'.'accession', mysql -> `gringlobal`.`accession`, oracle -> gringlobal.accession, etc
        /// </summary>
		public string FullTableName {
			get {
				if (String.IsNullOrEmpty(SchemaName)) {
					return Delimiter + TableName + Delimiter;
				} else {
					return Delimiter + SchemaName + Delimiter + "." + Delimiter + TableName + Delimiter;
				}
			}
		}

		protected string _tableName;
		public virtual string TableName {
			get {
				return _tableName;
			}
			set {
				_tableName = (value == null ? null : value.ToLower());
			}
		}

		public SyncAction SyncAction { get; set; }


		protected abstract void fillFromDatabase(List<TableInfo> tables, bool fillRowCount, int maxRecurseDepth);

		protected abstract void fillFromDataSet(DataSet ds, DataRow dr, List<TableInfo> tables);

		protected virtual string generateIndexSelectSql() {
			throw new NotImplementedException();
		}

		protected virtual string generateInsertSelectSql() {
			throw new NotImplementedException();
		}

		protected virtual string generateMapCreateSql() {
			return "CREATE TABLE [__" + TableName + @"] (
new_id int identity(1,1),
previous_id int NOT NULL,
table_name varchar(30) NULL,
PRIMARY KEY (new_id),
);
alter table [__" + TableName + @"] add index [_MAP_PK_" + TableName + "] (previous_id, table_name) ;";
		}

		internal abstract string generateCreateSql();
		protected virtual string generateDropSql() {
			throw new NotImplementedException();
		}

		public DataConnectionSpec DataConnectionSpec { get; set; }

		public abstract DataRow FillRow(DataRow dr);

	}
}
