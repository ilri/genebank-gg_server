using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace GrinGlobal.Search.Engine {

    /// <summary>
    /// This object represents the values of the current state of a Resolver object
    /// </summary>
    [DataContract(Namespace = "http://www.grin-global.org")]
    [KnownType(typeof(System.DBNull))]
    public class ResolverReport {

        /// <summary>
        /// Gets the name of the resolver
        /// </summary>
		[DataMember]
		public string Name;

        /// <summary>
        /// Returns True if the resolver is enabled, False otherwise
        /// </summary>
		[DataMember]
		public bool IsEnabled;

        /// <summary>
        /// Returns True if the resolver is currently loaded, False otherwise
        /// </summary>
		[DataMember]
		public bool IsLoaded;

        /// <summary>
        /// Returns either Sql, PrimaryKey, or ForeignKey.  Dictates how to look up an identifier -- either we created an entry in a b+ tree (created from the Sql), use the PrimaryKeyID field from the hit (PrimaryKey), or pull it from one of the Hit.HitField values (ForeignKey)
        /// </summary>
		[DataMember]
		public string Method;

        /// <summary>
        /// Gets the name of the Foreign Key Field to lookup foreign key values from.  Corresponds to the list of Fields defined for an Index in the search engine's config file
        /// </summary>
		[DataMember]
		public string ForeignKeyField;

        /// <summary>
        /// Gets the Sql to run to generate the B+ tree to lookup parent identifiers -- the search engine's way of quickly 'bubbling up' the relationships to get to the target ancestor's primary key identifier.
        /// </summary>
		[DataMember]
		public string Sql;

        /// <summary>
        /// Gets the name of the primary key field from the resolver's source table
        /// </summary>
		[DataMember]
		public string PrimaryKeyField;

        /// <summary>
        /// Gets the name of the target ancestor table's primary key field
        /// </summary>
		[DataMember]
		public string ResolvedPrimaryKeyField;

	}
}
