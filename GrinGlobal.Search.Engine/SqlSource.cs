using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// Specifies the source of the SQL for an index
    /// </summary>
    public enum SqlSource {
        /// <summary>
        /// Unknown source.  Never a valid value.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// SQL is defined in the gringlobal.search.config file itself, under a &lt;Sql&gt; node in the appropriate location
        /// </summary>
        Xml = 1,
        ///// <summary>
        ///// No SQL is defined.  Invalid for an Index object, but valid for Indexer or Resolver objects.
        ///// </summary>
        None = 2,
        /// <summary>
        /// SQL is defined in a dataview definition.  Valid only for databases using the GRIN-Global schema.
        /// </summary>
        Dataview = 4
    }
}
