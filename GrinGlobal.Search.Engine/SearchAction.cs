using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
	internal enum SearchAction {
		Literal,
		And,
		Or,
		Not,
		BeginGrouping,
		EndGrouping,
		FieldDefinition,
		Processed,
        Then
	}
}
