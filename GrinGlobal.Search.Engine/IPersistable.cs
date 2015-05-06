using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// An interface that allows reading/writing/updating of row-based data in a binary format
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IPersistable<T> {

        /// <summary>
        /// Called when an Update needs to take place against the object
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="updateMode"></param>
        void Update(T newValue, GrinGlobal.Interface.UpdateMode updateMode);

		/// <summary>
		/// Reads a new object from the given binary stream
		/// </summary>
		/// <param name="rdr"></param>
		void Read(BinaryReader rdr);

		/// <summary>
		/// Writes the current object to the given binary stream
		/// </summary>
		/// <param name="wtr"></param>
		void Write(BinaryWriter wtr);

	}
}
