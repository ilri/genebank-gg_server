using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Dataviews;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Diagnostics;
using System.Data;

namespace GrinGlobal.Business.SqlParser {
    /// <summary>
    /// Primary entry point for parsing and generating SQL statements that are for querying data, but not manipulating data (i.e. SELECT statements only; not UPDATE, INSERT, DELETE, DROP, CREATE, GRANT, etc)
    /// </summary>
    public class Generator {

        private DataConnectionSpec _dataConnectionSpec;

        public Generator(DataConnectionSpec dcs) {
            _dataConnectionSpec = dcs;
        }

        public Query Query { get; private set; }

        public Query Parse(string sql) {

            // parsing sql is not an easy task, so we break it down into various passes:
            // 1. Parse out all parameters -- since these are global to the query (regardless of join / subquery / union / etc)
            //                                we can blindly check for anything starting with parameter monikers
            // 2. Parse out all UNION clauses -- the tables/fields/aliases are specific to each UNION clause, so this must be done next
            // 3. For 1st UNION clause only, determine all tables
            // 4. For 1st UNION clause only, determine all fields

            Query = new Query(sql, _dataConnectionSpec);
            Query.Parse();

            return Query;

        }


    }
}
