using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// Deprecated, not used by GG anymore.
    /// </summary>
    public enum DataResource {
        /// <summary>
        /// Does not apply to a specific resource -- could be either Table or Dataview
        /// </summary>
        Any = 1,
        /// <summary>
        /// Applies only to Table definitions
        /// </summary>
        Table = 2,
        /// <summary>
        /// Applies only to Dataview definitions
        /// </summary>
        Dataview = 3,
    }
}
