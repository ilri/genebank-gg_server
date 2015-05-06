using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// Represents the method for resolution of an ID in a Resolver. Valid values: None, PrimaryKey, ForeignKey, Sql 
    /// </summary>
	public enum ResolutionMethod {
        /// <summary>
        /// Invalid method defined.  Used for default valid only, not actually a valid resolution method.
        /// </summary>
        Invalid,
        /// <summary>
        /// No resolution defined.
        /// </summary>
		None,
        /// <summary>
        /// Primary Key from the Index will be used
        /// </summary>
		PrimaryKey,
        /// <summary>
        /// A Foreign Key stored in the Index will be used
        /// </summary>
		ForeignKey,
        /// <summary>
        /// The ID will be looked up via Sql (possibly cached in a B+ tree if CacheEnabled = True)
        /// </summary>
		Sql,
	}
}
