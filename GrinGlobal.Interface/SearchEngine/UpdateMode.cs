using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GrinGlobal.Interface {
    /// <summary>
    /// This object specifies how an item is updated in the search engine.
    /// </summary>
    [DataContract(Namespace = "http://www.grin-global.org")]
    public enum UpdateMode {
		/// <summary>
		/// Should add or append new value to the existing value
		/// </summary>
        [EnumMember()]
		Add,
		/// <summary>
		/// Should entirely replace the existing value with the new value
		/// </summary>
        [EnumMember()]
        Replace,
		/// <summary>
		/// Should subtract or remove the new value from the existing value
		/// </summary>
        [EnumMember()]
        Subtract
	}
}
