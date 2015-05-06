using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Configuration;
using System.Diagnostics;

namespace GrinGlobal.Core.DataManagers {
#if !DEBUGDATAMANAGER
		[DebuggerStepThrough()]
#endif
	internal class OdbcDataManager : DataManager {
        internal OdbcDataManager(DataConnectionSpec dcs)
            : base(dcs, "Uid", "Pwd", DataVendor.ODBC) {
        }

        protected override void mungeConnectionSpec(DataConnectionSpec dcs) {
            // nothing to do yet
        }

		protected internal override void clearPool(IDbConnection conn) {
			OdbcConnection.ReleaseObjectPool();
		}

		protected internal override IDbCommand createCommand(string text) {
            return new OdbcCommand(text);
        }

		protected internal override IDbDataParameter createParam(IDbCommand cmd, string name, object val, DbType dbType) {
            var prm = new OdbcParameter(name, val);
            prm.DbType = dbType;
            return prm;
        }

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
            return new OdbcDataAdapter((OdbcCommand)cmd);
        }

		protected internal override IDbConnection createConn(string connString) {
            return new OdbcConnection(connString);
        }

		public override string Concatenate(string[] values) {
			StringBuilder sb = new StringBuilder();
			foreach (string v in values) {
				sb.Append("coalesce(")
					.Append(v)
					.Append(",'') + ");
			}
			if (sb.Length > 3) {
				sb.Remove(sb.Length - 3, 3);
			}
			return sb.ToString();
		}


		protected internal override void applyLimit(IDbCommand cmd, int limit, int offset) {
			string sel = cmd.CommandText.Substring(0, 6);
			cmd.CommandText = sel + " TOP " + limit + " " + cmd.CommandText.Substring(6);
		}

		protected internal override void mungeParameter(IDbCommand cmd, IDbDataParameter prm) {
			// do nothing -- yet
			string txt = cmd.CommandText;
			txt = txt.Replace(prm.ParameterName, "?");
			prm.ParameterName = "?";
			cmd.CommandText = txt;
		}

		protected internal override void mungeSql(IDbCommand cmd) {
			// replace all parameter names with ?
		}

		protected internal override void enableReturnId(IDbCommand cmd, string nameOfSequenceOrPrimaryKey, ref DataParameters dps) {
			if (cmd.CommandType == CommandType.Text) {
				cmd.CommandText += "; select scope_identity();";
			}
		}

		protected internal override object executeInsert(IDbCommand cmd) {
			object ret = cmd.ExecuteScalar();
			return ret;
		}

        //protected override void initConnectionString(DataConnectionSpec dcs, string serverName, string databaseName) {
        //    string connString = "Data Source=" + serverName + ";";

        //    if (!String.IsNullOrEmpty(databaseName)) {
        //        connString += "Initial Catalog=" + databaseName + ";";
        //    }

        //    if (String.IsNullOrEmpty(dcs.UserName)) {
        //        connString += "Integrated Security=SSPI;";
        //    } else {
        //        connString += dcs.UserNameMoniker + "=;" + dcs.PasswordMoniker + "=;";
        //        connString = base.replaceNameValuePair(connString, dcs.UserNameMoniker, dcs.UserName);
        //        connString = base.replaceNameValuePair(connString, dcs.PasswordMoniker, dcs.Password);
        //    }

        //    dcs.ConnectionString = connString;
        //}

    }
}
