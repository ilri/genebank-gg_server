using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// An object that can be used to match a keyword in a B+ Tree
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
	public struct KeywordMatch<TKey, TValue>
		where TKey : ISearchable<TKey>, new()
		where TValue : IPersistable<TValue>, new()
	{
        /// <summary>
        /// The index within the node for the current Keyword or Value. e.g. Node.Keywords[Index] == Keyword, and Node.Values[Index] == Value
        /// </summary>
		public int Index;

        /// <summary>
        /// The keyword in the search index (usually a BPlusString).  Keyword == Node.Keywords[Index]
        /// </summary>
		public TKey Keyword;

        /// <summary>
        /// The value stored in the search index for this keyword (usually a BPlusListLong).  Value == Node.Values[Index]
        /// </summary>
		public TValue Value;

        /// <summary>
        /// The leaf node associated with this keyword.
        /// </summary>
		public BPlusTreeLeafNode<TKey, TValue> Node;

        /// <summary>
        /// Returns string representation of the keyword/value/index associated with this object
        /// </summary>
        /// <returns></returns>
		public override string ToString() {
			return "Keyword=" + Keyword.ToString() + ", Value=" + Value.ToString() + ", Index=" + Index;
		}

	}
}
