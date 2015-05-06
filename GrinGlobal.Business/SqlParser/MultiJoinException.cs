using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business.SqlParser {
    public class MultiJoinException : Exception {
        public ITable Table;
        public List<Join> PossibleJoins;

        public MultiJoinException(string message, ITable table, List<Join> joins) : base(message) {
            Table = table;
            PossibleJoins = joins;
        }
    }
}
