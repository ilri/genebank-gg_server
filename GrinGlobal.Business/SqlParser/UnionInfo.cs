using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Business.SqlParser {
    public class UnionInfo {
        public int StartOffset;
        public int EndOffset;
        public List<string> Tokens;

        public UnionInfo() {
            StartOffset = 0;
            EndOffset = 0;
            Tokens = new List<string>();
        }
    }
}
