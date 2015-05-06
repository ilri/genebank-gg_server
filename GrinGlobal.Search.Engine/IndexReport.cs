using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GrinGlobal.Search.Engine {

    /// <summary>
    /// This object represents the current status of an Index object
    /// </summary>
    [DataContract(Namespace = "http://www.grin-global.org")]
    [KnownType(typeof(System.DBNull))]
    public class IndexReport {

        /// <summary>
        /// Gets the Name of the Index
        /// </summary>
		[DataMember]
		public string Name;

        /// <summary>
        /// Returns True if the Index is loaded and currently searchable.  False otherwise
        /// </summary>
		[DataMember]
		public bool IsLoaded;

        /// <summary>
        /// Returns True if the Index is enabled.  False otherwise
        /// </summary>
		[DataMember]
		public bool IsEnabled;

        /// <summary>
        /// Returns the Sql used to generate the Index contents.
        /// </summary>
		[DataMember]
		public string Sql;

        /// <summary>
        /// Returns a list of ResolverReports to reflect the current status(es) of the Index's Resolver(s)
        /// </summary>
		[DataMember]
		public List<ResolverReport> Resolvers;

		public IndexReport() {
			Resolvers = new List<ResolverReport>();
		}

	}
}
