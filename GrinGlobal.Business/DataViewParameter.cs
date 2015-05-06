using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business {
    /// <summary>
    /// A database engine agnostic representation of a parameter sent as part of a SQL query.  Oracle refers to them as a "variable", others refer to them as "parameter".
    /// </summary>
	public class DataviewParameter : IDataviewParameter {
		public string Name { get; set; }
		public string TypeName { get; set; }
        public object Value { get; set; }
        public Dictionary<string, object> ExtendedProperties { get; private set; }

		public override string ToString() {
            return "Name=" + Name + " ; TypeName=" + TypeName + " ; Value=" + Value;
		}

        public DataviewParameter() {
            ExtendedProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a deep copy of this object.
        /// </summary>
        /// <returns></returns>
        public IDataviewParameter Clone() {
            DataviewParameter ret = new DataviewParameter();
            ret.Name = this.Name;
            ret.TypeName = this.TypeName;
            ret.Value = this.Value;
            foreach (var key in this.ExtendedProperties.Keys) {
                ret.ExtendedProperties[key] = ExtendedProperties[key];
            }
            return ret;
        }



    }
}
