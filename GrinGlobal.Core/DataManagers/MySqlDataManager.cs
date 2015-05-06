using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace brockweaver.DataManagers {
	internal class MySqlDataManager : DataManager {
		internal MySqlDataManager(ConnectionStringSettings css)
			: base(css) {
		}

		protected internal override IDbCommand createCommand(string text) {
			return new MySqlCommand(text);
		}

		protected internal override IDataParameter createParam(string name, object val) {
			return new MySqlParameter(name, val);
		}

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
			return new MySqlDataAdapter((MySqlCommand)cmd);
		}

		protected internal override IDbConnection createConn(string connString) {
			return new MySqlConnection(connString);
		}
	}
}
