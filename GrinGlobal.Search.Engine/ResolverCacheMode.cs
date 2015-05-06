using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    public enum ResolverCacheMode {
        /// <summary>
        /// No caching is used.  SQL contained within the config file is ran against database in real-time to do necessary lookups.  Slow regardless of hit count, nothing needed to keep up-to-date
        /// </summary>
        None,
        /// <summary>
        /// Resolver has a single tree based on ID, essentially enabling foreign key lookups to resolved ID's.  Overall, good search performance with few (&lt; 1000) hits, relatively inexpensive to keep up-to-date
        /// </summary>
        Id,
        /// <summary>
        /// Resolver has two trees, one based on ID and one based on keyword.  Excellent search performance with any number of hits, extremely expensive (in both space and processing time) to keep up-to-date
        /// </summary>
        IdAndKeyword
    }
}
