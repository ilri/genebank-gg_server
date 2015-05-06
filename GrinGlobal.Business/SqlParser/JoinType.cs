using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Business.SqlParser {
    public enum JoinType {
        Unknown,
        None,
        Inner,
        Left,
        Right,
        Outer,
        Full
    }
}
