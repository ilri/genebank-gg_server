using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// The current state of the index
    /// </summary>
	public enum IndexState {
        /// <summary>
        /// Index is in the process of opening
        /// </summary>
		Opening,
        /// <summary>
        /// Index is open and ready to be queried
        /// </summary>
		Open,
        /// <summary>
        /// Index is in the process of closing
        /// </summary>
		Closing,
        /// <summary>
        /// Index is closed and cannot be queried
        /// </summary>
		Closed,
	}
}
