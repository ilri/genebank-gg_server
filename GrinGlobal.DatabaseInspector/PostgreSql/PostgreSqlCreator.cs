using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.DatabaseInspector.PostgreSql {
    public class PostgreSqlCreator : Creator {

        internal PostgreSqlCreator(DataConnectionSpec dcs)
            : base(dcs) {
        }

        public override string NormalizeName(string name) {
            return @"""" + name + @"""";
        }

        public override List<string> ListTableNames(string schemaName) {
            using (DataManager dm = DataManager.Create(DataConnectionSpec)) {
                DataTable dt = dm.Read(@"
select 
    table_name
from 
    information_schema.tables 
where 
    table_type = 'BASE TABLE' 
    and table_catalog = :schemaName
    and table_schema not ilike 'information_schema'
    and table_name not ilike 'pg_%'
order by 
	table_name
", new DataParameters(":schemaName", schemaName, DbType.String));
                List<string> ret = new List<string>();
                foreach (DataRow dr in dt.Rows) {
                    ret.Add(dr["table_name"].ToString());
                }
                return ret;
            }
        }
    }
}
