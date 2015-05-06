using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    [Flags]
    public enum DatabasePassthruLevel : uint {
        /// <summary>
        /// Search Command will never cause a database lookup; if field is not in index or it is a non-equal comparison (&lt;, &gt;, &lg;&gt;, !=), 0 hits will be returned.
        /// </summary>
        None = 0,
        /// <summary>
        /// Search Command will lookup via database if field is not in index.  If it is a non-equal comparison (&lt;, &gt;, &lt;&gt;, !=), 0 hits will be returned.
        /// </summary>
        NonIndexed = 1,
        /// <summary>
        /// Search Command will lookup via database if field is a non-equal comparison (&lt;, &gt;, &lt;&gt;, !=).
        /// </summary>
        Comparison = 2,
        /// <summary>
        /// Search Command will lookup via database if field is not in the index or it is a non-equal comparison (&lt;, &gt;, &lt;&gt;, !=).  Essentially NonIndexed | Comparison.
        /// </summary>
        NonIndexedOrComparison = NonIndexed | Comparison,
        /// <summary>
        /// Search Command will lookup via database if no hits are found in the search engine.
        /// </summary>
        ZeroHitsFound = 4,
        /// <summary>
        /// Search Command will ignore resolver caches completely and always query the database directly for resolved ids.  Does not affect index search behavior at all.
        /// </summary>
        Resolver = 8,
        /// <summary>
        /// Search Command will completely ignore search indexes and resolver caches and always query the database directly.
        /// </summary>
        Always = Resolver | ZeroHitsFound | NonIndexed | Comparison,
        
    }
}
