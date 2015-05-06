using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
    internal interface IExternalSortable<T> : IComparable<T> where T : new() {

		/// <summary>
		/// Reads a new object from the given binary stream
		/// </summary>
		/// <param name="rdr"></param>
		void Read(BinaryReader rdr, bool includeSortKey);

		/// <summary>
		/// Writes the current object to the given binary stream
		/// </summary>
		/// <param name="wtr"></param>
		void Write(BinaryWriter wtr, bool includeSortKey);

		/// <summary>
		/// Sorts the given list and return it
		/// </summary>
		/// <param name="items"></param>
		/// <returns></returns>
		IEnumerable<T> Sort(IEnumerable<T> items);

		/// <summary>
		/// Returns -1 if this is &lt; other, 0 if this == other, 1 if this &gt; other
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		int CompareTo(T other);

		/// <summary>
		/// Returns true if this is in the same group as other.  i.e. the following should return true:  this.ID == other.ID even if this.PrimaryKeyID != other.PrimaryKeyID
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		bool IsInSameGroup(T other);

	}
}
