using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.Dataviews {
	public interface IDataviewParameter {
		string Name { get; set; }
		string TypeName { get; set; }
        object Value { get; set; }
        IDataviewParameter Clone();
        Dictionary<string, object> ExtendedProperties { get; }
	}
}
