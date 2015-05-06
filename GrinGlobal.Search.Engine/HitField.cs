using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Core;
using System.Runtime.Serialization;

namespace GrinGlobal.Search.Engine {

	/// <summary>
	/// Used to store a value from the database in the index itself.  Think foreign key lookup.
	/// </summary>
    [DataContract(Namespace = "http://www.grin-global.org")]
    [KnownType(typeof(System.DBNull))]
    public struct HitField : IExternalSortable<HitField> {

		/// <summary>
		/// The ordinal of the field within the database row. (Name is not stored for size and performance purposes.  Lookup Name via IndexedFields[HitField.FieldOrdinal].Name)
		/// </summary>
        [DataMember]
        public int FieldOrdinal;

		/// <summary>
		/// The value from the database row.  Usually a foreign key id.
		/// </summary>
        [DataMember]
        public int Value;

		internal static readonly int SIZE = sizeof(int) * 2; // = 8

		public HitField(BinaryReader rdr) {
			FieldOrdinal = -1;
			Value = -1;
			Read(rdr, true);
		}

		public override string ToString() {
			return "FieldOrdinal=" + FieldOrdinal + ", Value=" + Value;
		}

		#region IPersistable<HitField> Members


        /// <summary>
        /// Not to be used.  Always throws NotImplementedException.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
		public IEnumerable<HitField> Sort(IEnumerable<HitField> items) {
			throw new NotImplementedException();
		}

        /// <summary>
        /// Loads the current object with data from the given BinaryReader as a source.  includeSortKey is ignored.
        /// </summary>
        /// <param name="rdr"></param>
        /// <param name="includeSortKey"></param>
		public void Read(BinaryReader rdr, bool includeSortKey) {
			FieldOrdinal = rdr.ReadInt32();
			Value = rdr.ReadInt32();
		}

        /// <summary>
        /// Persists the current object to the given BinaryWriter.  includeSortKey is ignored.
        /// </summary>
        /// <param name="wtr"></param>
        /// <param name="includeSortKey"></param>
		public void Write(BinaryWriter wtr, bool includeSortKey) {
			wtr.Write(FieldOrdinal);
			wtr.Write(Value);
		}

        /// <summary>
        /// Returns -1 if current object's Value property is less than the 'other' object's Value property.  Zero if they match.  +1 otherwise.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public int CompareTo(HitField other) {
			if (this.Value < other.Value) {
				return -1;
			} else if (this.Value == other.Value) {
				return 0;
			} else {
				return 1;
			}
		}

        /// <summary>
        /// Returns True if current object's FieldOrdinal property has the same value as the 'other' object's FieldOrdinal property.  Returns False otherwise
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
		public bool IsInSameGroup(HitField other) {
			return this.FieldOrdinal == other.FieldOrdinal;
		}

		//public void ReadOnlySortKey(BinaryReader rdr) {
		//    // do nothing, no such thing as a sort key
		//}

		//public void WriteOnlySortKey(BinaryWriter wtr) {
		//    // do nothing, no such thing as a sort key
		//}

		#endregion
	}
}
