using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Npgsql;
using System.Configuration;

namespace brockweaver.DataManagers {
	internal class PostgreSqlDataManager : DataManager {
		internal PostgreSqlDataManager(ConnectionStringSettings css)
			: base(css) {
		}

		protected internal override IDbCommand createCommand(string text) {
			return new NpgsqlCommand(text);
		}

		protected internal override IDataParameter createParam(string name, object val) {
			return new NpgsqlParameter(name, val);
		}

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
			return new NpgsqlDataAdapter((NpgsqlCommand)cmd);
		}

		protected internal override IDbConnection createConn(string connString) {
			return new NpgsqlConnection(connString);
		}
	}
}
