using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Business.SqlParser {
    public enum Clause {
        Unknown,
        Select,
        From,
        Where,
        Group,
        Having,
        Order,
        Union,
    }
}
