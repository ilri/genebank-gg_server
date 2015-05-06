using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Core {
    public enum DbPseudoType {
        /// <summary>
        /// Used as a placeholder to detect when pseudo type hasn't been set properly
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// For representing a series of integers that will be passed as part of the SQL itself.  Used when variables are needed where database engines do not allow them.  e.g. IN (:integer_list_here)
        /// </summary>
        IntegerCollection = 1,
        /// <summary>
        /// For replacing the value as part of the SQL itself instead of an actual Parameter to the databse engine.  Used when variables are needed where database engines do not allow them.  e.g. select * from :table_name_here
        /// </summary>
        StringReplacement = 2,
        /// <summary>
        /// For representing a series of strings that will be passed as part of the SQL itself.  Used when variables are needed where database engines do not allow them.  e.g. IN (:string_list_here).
        /// </summary>
        StringCollection = 3,
        /// <summary>
        /// For representing a series of integers that will be passed as part of the SQL itself.  Used when variables are needed where database engines do not allow them.  e.g. IN (:decimal_list_here)
        /// </summary>
        DecimalCollection = 4
    }
}
