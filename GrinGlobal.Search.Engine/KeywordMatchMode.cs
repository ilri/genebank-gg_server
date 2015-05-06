using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
	public enum KeywordMatchMode {
		ExactMatch = 0,
		StartsWith = 1,
		EndsWith = 2,
		Contains = 3,
	}
}
