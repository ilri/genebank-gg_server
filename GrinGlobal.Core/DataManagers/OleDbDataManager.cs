using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Configuration;
using System.Diagnostics;

namespace GrinGlobal.Core.DataManagers {
#if !DEBUGDATAMANAGER
		[DebuggerStepThrough()]
#endif
	internal class OleDbDataManager : DataManager {
		internal OleDbDataManager(DataConnectionSpec dcs)
			: base(dcs, "User Id", "Password", DataVendor.Unknown) {
        }

        protected override void mungeConnectionSpec(DataConnectionSpec dcs) {
            // nothing to do yet
        }


		protected internal override void clearPool(IDbConnection conn) {
			OleDbConnection.ReleaseObjectPool();
		}

		protected internal override IDbCommand createCommand(string text) {
            return new OleDbCommand(text);
        }

		protected internal override IDbDataParameter createParam(IDbCommand cmd, string name, object val, DbType dbType) {
            var prm = new OleDbParameter(name, val);
            prm.DbType = dbType;
            return prm;
        }

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
            return new OleDbDataAdapter((OleDbCommand)cmd);
        }

		protected internal override IDbConnection createConn(string connString) {
            return new OleDbConnection(connString);
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

		protected internal override void mungeParameter(IDbCommand cmd, IDbDataParameter prm) {
			// do nothing -- yet
		}

		protected internal override void mungeSql(IDbCommand cmd) {
			// do nothing -- yet
		}

		protected internal override void applyLimit(IDbCommand cmd, int limit, int offset) {
			string sel = cmd.CommandText.Substring(0, 6);
			cmd.CommandText = sel + " TOP " + limit + " " + cmd.CommandText.Substring(6);
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
        //    throw new NotImplementedException();
        //}
    }
}
