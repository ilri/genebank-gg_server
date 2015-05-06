using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    public enum KeywordCompareMode {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanEqualTo,
        LessThan,
        LessThanEqualTo,
        Between,
        BetweenAnd
    }
}
