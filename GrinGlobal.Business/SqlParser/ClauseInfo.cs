using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Business.SqlParser {
    public class ClauseInfo {

        public int Offset;
        public Clause Clause;
        public List<string> Tokens;

        public ClauseInfo() {
            Clause = Clause.Unknown;
            Tokens = new List<string>();
        }
    }
}
