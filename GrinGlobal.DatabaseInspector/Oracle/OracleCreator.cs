using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Resources;

using GrinGlobal.DatabaseInspector;

namespace GrinGlobal.DatabaseInspector.Oracle {
	public class OracleCreator : Creator {

        internal OracleCreator(DataConnectionSpec dcs)
			: base(dcs) {

		}

		public override string CommandSeparator {
			get {
				return ";";
			}
		}

		public override string NormalizeName(string name) {
			return name;
//			return "\"" + name + "\"";
		}

		public override List<string> ListTableNames(string schema) {
			using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
				string where = (String.IsNullOrEmpty(schema) ? "" : "where owner = '" + schema + "'");
				DataTable dt = dm.Read("select * from all_all_tables " + where);
				List<string> ret = new List<string>();
				foreach (DataRow dr in dt.Rows) {
					ret.Add(dr["table_name"].ToString());
				}
				return ret;
			}
		}

	}
}
