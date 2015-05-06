using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GrinGlobal.Search.Engine {
	public interface ISearchable<T> : IComparable<T>, IPersistable<T> {

        object RootValue { get; set; }

        ///// <summary>
        ///// The object that is to be persistable
        ///// </summary>
        //T Value { get; set; }

		/// <summary>
		/// Should return -1 if current object is &lt; given value, 0 if it is exactly equal to, or 1 otherwise
		/// </summary>
		/// <param name="value"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		int CompareTo(T bplusValue, KeywordMatchMode matchMode, bool ignoreCase);

		/// <summary>
		/// Parse the given string into the proper type T
		/// </summary>
		/// <param name="searchTerm"></param>
		T Parse(string searchTerm);

	}
}
