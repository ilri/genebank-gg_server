using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business.SqlParser {
    public class MissingJoinException : Exception {
        public ITable Table;
        public List<Join> ExistingJoins;
        public MissingJoinException(string message, ITable table, List<Join> existingJoins)
            : base(message) {
            Table = table;
            ExistingJoins = existingJoins;
        }
    }
}
