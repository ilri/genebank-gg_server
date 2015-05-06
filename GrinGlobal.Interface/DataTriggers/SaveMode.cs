using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    /// <summary>
    /// The save action to be taken.  None, Update, Delete, Insert, or Unknown.
    /// </summary>
	public enum SaveMode {
		None,
		Update,
		Delete,
		Insert,
        Unknown
	}
}
