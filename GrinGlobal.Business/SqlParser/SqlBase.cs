using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using GrinGlobal.Interface.Dataviews;
using GrinGlobal.Business;
using System.Data;
using System.Diagnostics;

namespace GrinGlobal.Business.SqlParser {
    public abstract class SqlBase {

        public DataConnectionSpec DataConnectionSpec;
        public int LanguageID;

        public List<ClauseInfo> Clauses { get; protected set; }

        private List<ITable> _tables;
        public List<ITable> Tables {
            get {
                return _tables;
            }
            protected set {
                _tables = value;
            }
        }

        private List<Join> _joins;
        public List<Join> Joins {
            get {
                return _joins;
            }
            protected set {
                _joins = value;
            }
        }

        private List<IDataviewParameter> _parameters;
        public List<IDataviewParameter> Parameters {
            get {
                return _parameters;
            }
            set {
                _parameters = value;
            }
        }

        private List<IField> _fields;
        public List<IField> Fields {
            get {
                return _fields;
            }
            set {
                _fields = value;
            }
        }

        /// <summary>
        /// All the SQL_* arrays concatenated into one big list
        /// </summary>
        protected static List<string> __allSqlWords;

        /// <summary>
        /// with, select, from, where, order, group, having, union
        /// </summary>
        protected static string[] SQL_CLAUSES = new string[] {
            "with",
            "select", 
            "from", 
            "where", 
            "order", 
            "group", 
            "having",
            "union",
        };

        /// <summary>
        /// join, on
        /// </summary>
        protected static string[] SQL_JOIN_CLAUSES = new string[] {
            "join",
            "on",
        };

        /// <summary>
        /// is, null, distinct, by, top
        /// </summary>
        protected static string[] SQL_KEYWORDS = new string[] { 
            "is",
            "null",
            "distinct",
            "by", 
            "top",
            "trim",
            // brock added 2010-12-30 to address bad mapping issue when field def is:    "coalesce(field_name,-1) as field_name"
            "ltrim",
            "rtrim",
            "coalesce",
            "concat"
        };

        /// <summary>
        /// inner, outer, left, full, right
        /// </summary>
        protected static string[] SQL_JOIN_TYPES = new string[] {
            "inner", 
            "outer", 
            "left", 
            "full", 
            "right", 
        };

        /// <summary>
        /// and, or, not, like, in, =, +, ||, ...
        /// </summary>
        protected static string[] SQL_COMPARATORS = new string[]{
            "=",
            "<",
            ">",
            "<=",
            ">=",
            "*=",
            "=*",
            "like",
            "!=",
            "<>",
            "and",
            "not",
            "or",
            "in",
            "+",
            "||",
            "-",
        };

        /// <summary>
        /// and, not, or
        /// </summary>
        protected static string[] SQL_LOGICAL_OPERATORS = new string[]{
            "and",
            "not",
            "or"
        };

        public string SQL { get; protected set; }

        /// <summary>
        /// space, tab, carriage return, linefeed
        /// </summary>
        protected static char[] WHITESPACE = new char[] { ' ', '\t', '\r', '\n' };

        /// <summary>
        /// Comma, space, tab, carriage return, linefeed, left paren, right paren
        /// </summary>
        protected static char[] BASIC_TOKENS = new char[] { ',', ' ', '\t', '\r', '\n', '(', ')' };

        protected string applyFieldAlias(string fieldText, string aliasName) {

            var ret = fieldText.TrimEnd(WHITESPACE);
            var split = ret.Split('.');
            if (String.Compare(split[split.Length - 1], aliasName, true) == 0) {
                // fieldText is syntactically the same as the aliasName, so do not bother to append the alias name.
            } else {
                ret += " AS " + aliasName.TrimStart(WHITESPACE);
            }
            return ret;
        }


        public SqlBase(string sql, DataConnectionSpec dcs, int languageID, bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            Tables = new List<ITable>();
            Parameters = new List<IDataviewParameter>();
            Joins = new List<Join>();
            Fields = new List<IField>();
            DataConnectionSpec = dcs;
            LanguageID = languageID;


            if (__allSqlWords == null) {
                __allSqlWords = new List<string>();
                __allSqlWords.AddRange(SQL_CLAUSES);
                __allSqlWords.AddRange(SQL_COMPARATORS);
                __allSqlWords.AddRange(SQL_JOIN_CLAUSES);
                __allSqlWords.AddRange(SQL_JOIN_TYPES);
                __allSqlWords.AddRange(SQL_KEYWORDS);
            }

            Parse(sql, errorOnMissingRelationship, errorOnMultipleRelationships);

        }

        public void Parse(string sql, bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            SQL = sql;
            Parse(errorOnMissingRelationship, errorOnMultipleRelationships);
        }

        public abstract void Parse(List<string> tokens, bool isTopLevelTokens, bool errorOnMissingRelationship, bool errorOnMultipleRelationships);

        public void Parse(bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            var tokens = tokenize(SQL);
            Parse(tokens, false, errorOnMissingRelationship, errorOnMultipleRelationships);
        }

        protected List<string> tokenize(string text) {
            if (String.IsNullOrEmpty(text)) {
                return new List<string>();
            } else {
                return text.Trim().SplitRetain(BASIC_TOKENS, true, true, false);
            }
        }

        /// <summary>
        /// Returns all valid SQL tokens that are top-level tokens.  i.e. excludes comments and optionally whitespace.  All other non-top-level tokens are combined into a single top-level token (i.e. subselects, case statements, etc come out as a single token)
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        protected List<string> getTopLevelTokens(List<string> tokens, bool suppressWhiteSpace) {
            var ret = new List<string>();
            var inLineComment = false;
            var inMultilineComment = false;
            var parenDepth = 0;
            var caseDepth = 0;
            StringBuilder subLevelTokens = new StringBuilder();
            for (var i = 0; i < tokens.Count; i++) {
                var tok = tokens[i];

                if (tok.StartsWith("/*")) {
                    inMultilineComment = true;
                } else if (tok.EndsWith("*/")) {
                    inMultilineComment = false;
                    subLevelTokens.Append(tok);
                    ret.Add(subLevelTokens.ToString());
                    subLevelTokens = new StringBuilder();
                    continue;
                } else if (tok.StartsWith("--")) {
                    inLineComment = true;
                } else if (tok == "\n" && inLineComment) {
                    inLineComment = false;
                    subLevelTokens.Append(tok);
                    ret.Add(subLevelTokens.ToString());
                    subLevelTokens = new StringBuilder();
                    continue;
                }

                if (inMultilineComment || inLineComment) {
                    subLevelTokens.Append(tok);
                } else {
                    if (tok == "(") {
                        parenDepth++;
                    } else if (tok == ")") {
                        parenDepth--;
                        if (parenDepth == 0) {
                            subLevelTokens.Append(tok);
                            if (caseDepth == 0) {
                                ret.Add(subLevelTokens.ToString());
                                subLevelTokens = new StringBuilder();
                                continue;
                            }
                        }
                    }

                    if (parenDepth == 0) {
                        if (String.Compare(tok, "case", true) == 0) {
                            caseDepth++;
                        } else if (String.Compare(tok, "end", true) == 0 && caseDepth > 0) {
                            caseDepth--;
                            if (caseDepth == 0) {
                                ret.Add(subLevelTokens.ToString());
                                subLevelTokens = new StringBuilder();
                            }
                        }

                        if (caseDepth == 0) {
                            if (!suppressWhiteSpace || tok.Trim().Length > 0) {
                                ret.Add(tok);
                            }
                        } else {
                            if (caseDepth == 1 && tok == "case" && subLevelTokens == null) {
                                subLevelTokens = new StringBuilder(tok);
                            } else {
                                if (!suppressWhiteSpace || tok.Trim().Length > 0) {
                                    subLevelTokens.Append(tok);
                                }
                            }
                        }
                    } else {
                        if (parenDepth == 1 && tok == "(" && subLevelTokens == null) {
                            // start of a non-top-level token. start it as a new single word.
                            subLevelTokens = new StringBuilder(tok);
                        } else {
                            if (!suppressWhiteSpace || tok.Trim().Length > 0) {
                                subLevelTokens.Append(tok);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        protected bool IsSameTableAndAliasName(ITable tbl, string tableName, string aliasName) {
            return tbl != null && (String.Compare(tbl.AliasName, aliasName, true) == 0 && String.Compare(tbl.TableName, tableName, true) == 0);
        }


        protected bool IsSameTableName(ITable tbl, string tableName) {
            return tbl != null && (String.Compare(tbl.AliasName, tableName, true) == 0
                || String.Compare(tbl.TableName, tableName, true) == 0
                || String.Compare(tbl.TableID.ToString(), tableName, true) == 0);
        }

        protected bool IsSameFieldName(IField fld, string fieldName) {
            return fld != null && (String.Compare(fld.DataviewFieldName, fieldName, true) == 0
                || String.Compare(fld.TableFieldID.ToString(), fieldName, true) == 0);
        }


        /// <summary>
        /// Returns field with given dataviewFieldName OR if it previously had that dataviewFieldName it will return it. If neither, returns null.
        /// </summary>
        /// <param name="dataviewFieldName"></param>
        /// <returns></returns>
        public IField GetField(string dataviewFieldName) {

            var offset = GetFieldOffset(dataviewFieldName);
            if (offset < 0) {
                return null;
            } else {
                return Fields[offset];
            }
        }

        /// <summary>
        /// Returns offset of field with given dataviewFieldName OR if it previously had that dataviewFieldName it will return it. If neither, returns -1.
        /// </summary>
        /// <param name="dataviewFieldName"></param>
        /// <returns></returns>
        public int GetFieldOffset(string dataviewFieldName) {

            dataviewFieldName = dataviewFieldName.Replace("[", "").Replace("]", "");

            for (int i = 0; i < Fields.Count; i++) {
                var f = Fields[i];
                if (String.Compare(f.DataviewFieldName, dataviewFieldName, true) == 0) {
                    return i;
                } else {
                    var prevName = (object)null;
                    if (f.ExtendedProperties.TryGetValue("previous_name", out prevName)) {
                        if (String.Compare(prevName.ToString(), dataviewFieldName, true) == 0) {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Returns parameter with given parameterName OR if it previously had that parameter name it will set the current name to given name and return it.  If neither, returns null.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public IDataviewParameter GetParameter(string parameterName) {
            foreach (var p in Parameters) {
                if (String.Compare(p.Name, parameterName, true) == 0) {
                    return p;
                } else {
                    var prevName = (object)null;
                    if (p.ExtendedProperties.TryGetValue("previous_name", out prevName)) {
                        if (String.Compare(prevName.ToString(), parameterName, true) == 0) {
                            return p;
                        }
                    }
                }
            }
            return null;
        }

        protected IField getTableFieldMapping(string text) {

            var tableName = string.Empty;
            var fieldName = text.Trim();
            if (text.Contains(".")) {
                // is aliased on the table
                var split = text.Split('.');
                tableName = split[0].Trim();
                fieldName = split[1].Trim();
            }

            if (String.IsNullOrEmpty(fieldName)) {
                return null;
            }

            ITable table = null;
            IField field = null;

            if (!String.IsNullOrEmpty(tableName)) {
                // determine which table it belongs to
                foreach (var t in Tables) {
                    if (String.Compare(t.AliasName, tableName, true) == 0
                        || String.Compare(t.TableName, tableName, true) == 0) {

                        table = t;
                        break;

                    }
                }
            } else {
                // scan each table, use first one that matches (if any)
                foreach (var t in Tables) {
                    foreach (var tf in t.Mappings) {
                        if (String.Compare(tf.TableFieldName, fieldName, true) == 0) {
                            table = t;
                            break;
                        }
                    }
                    if (table != null) {
                        break;
                    }
                }
            }

            if (Fields != null) {
                field = Fields.Find(f2 => { return String.Compare(f2.DataviewFieldName, fieldName, true) == 0 && IsSameTableName(f2.Table, tableName); });
            }

            if (table != null) {
                // field belongs to a mapped table, so let's pull the definition from it
                // note we make a clone so we can have multiple aliases in a given query

                if (field == null) {
                    field = table.GetField(fieldName);
                    if (field != null) {
                        field = field.Clone(table);
                        field.DataviewFieldName = fieldName;
                    }
                } else {
                    field.Table = table;
                    field.TableName = table.TableName;
                }
            }

            if (field == null) {
                // could not find the table/field this one is pointing at.  create a new one that points at nothing.
                field = new Field { DataType = typeof(string), TableFieldName = fieldName, DataviewFieldName = fieldName, Table = null, TableFieldID = -1, IsReadOnly = true, IsPrimaryKey = false, ForeignKeyDataviewName = null, FriendlyFieldName = fieldName };
            }

            return field;

        }

        public ITable AddTableIfNeeded(string tableName, string aliasName, List<ITable> allTables, ITable newTable) {
            var t = Tables.Find(tbl => { return IsSameTableAndAliasName(tbl, tableName, aliasName); });
            if (t != null) {
                // table already exists in the collection, nothing to do.
            } else {
                if (allTables != null) {
                    t = allTables.Find(tbl => { return IsSameTableName(tbl, tableName); });
                }
                if (t == null) {
                    // invalid table name, use the newTable object they gave us
                    if (newTable != null) {
                        if (!Tables.Contains(newTable)) {
                            Tables.Add(newTable);
                        }
                        // also add all fields to available fields list
                        //foreach (var f in newTable.Mappings) {
                        //    var fclone = f.Clone(newTable);
                        //    AddFieldIfNeeded(fclone.TableFieldName, fclone.Table, fclone);
                        //}
                    } else {
                        // otherwise nothing to do
                    }
                } else {
                    // found the table, add a clone of it.
                    Tables.Add(t.Clone());
                }
            }
            return t;
        }


        protected List<Join> recalculateJoins() {
            // get rid of any relationships that point to/from tables we no longer have
            var ret = new List<Join>();
            foreach (var rel in Joins) {
                if (GetTable(rel.FromTable.TableName) != null
                    || GetTable(rel.ToTable.TableName) != null) {
                    ret.Add(rel);
                }
            }
            return ret;
        }


        protected List<IDataviewParameter> parseParameters(List<string> tokens) {

            var prms = new List<IDataviewParameter>();

            // tokens may have embedded period (.), so if they do, we need to split them out
            // so parameter names come out properly.
            var tempTokens = new List<string>();
            for (var i = 0; i < tokens.Count; i++) {
                var tok = tokens[i];
                if (tok.StartsWith(":") || tok.StartsWith("?") || tok.StartsWith("@")) {
                    if (tok.Contains(".")) {
                        var toks = tok.Split('.');
                        tempTokens.AddRange(toks);
                    } else {
                        tempTokens.Add(tok);
                    }
                } else {
                    tempTokens.Add(tok);
                }
            }

            for (var i = 0; i < tempTokens.Count; i++) {
                var tok = tempTokens[i];
                if (tok.StartsWith(":") || tok.StartsWith("?") || tok.StartsWith("@")) {

                    // is a parameter, add to the list
                    var dvp = new DataviewParameter();
                    dvp.Name = tok;
                    dvp.TypeName = "STRING";
                    // pull the type from the existing parameter list if possible (since we can't derive that information from the sql itself)
                    foreach (var p in Parameters) {
                        if (String.Compare(p.Name, dvp.Name, true) == 0) {
                            dvp.TypeName = p.TypeName;
                            break;
                        } else {
                            var prevName = (object)null;
                            if (p.ExtendedProperties.TryGetValue("previous_name", out prevName)) {
                                if (String.Compare(prevName.ToString(), tok, true) == 0) {
                                    dvp.TypeName = p.TypeName;
                                }
                            }
                        }

                    }
                    if (!String.IsNullOrEmpty(dvp.Name)) {
                        if (prms.Find(p2 => { return String.Compare(p2.Name, dvp.Name, true) == 0; }) == null) {
                            prms.Add(dvp);
                        }
                    }
                }
            }

            return prms;

        }

        protected List<UnionInfo> parseUnions(List<string> tokens) {
            var ret = new List<UnionInfo>();
            var unionInfo = new UnionInfo();
            var offset = 0;

            foreach (var tok in getTopLevelTokens(tokens, false)) {
                offset += tok.Length;
                if (String.Compare(tok, "union", true) == 0) {
                    unionInfo.EndOffset = offset;
                    ret.Add(unionInfo);
                    unionInfo = new UnionInfo { StartOffset = offset };
                } else {
                    unionInfo.Tokens.Add(tok);
                }
            }
            
            if (unionInfo.Tokens.Count > 0 && String.Join("", unionInfo.Tokens.ToArray()).Length > 0){
                unionInfo.EndOffset = offset;
                ret.Add(unionInfo);
            }

            return ret;
        }

        protected List<ClauseInfo> parseClauses(List<string> topLevelTokens) {
            var ret = new List<ClauseInfo>();
            var clauseInfo = new ClauseInfo();
            int offset = 0;

            foreach (var tok in topLevelTokens) {
                offset += tok.Length;
                if (SQL_CLAUSES.Contains(tok.ToLower())) {
                    // is a new major clause of the entire sql statement
                    if (clauseInfo.Clause != Clause.Unknown) {
                        ret.Add(clauseInfo);
                    }
                    clauseInfo = new ClauseInfo { Offset = offset, Clause = (Clause)Enum.Parse(typeof(Clause), tok, true) };
                } else {
                    clauseInfo.Tokens.Add(tok);
                }
            }

            if (clauseInfo.Clause != Clause.Unknown) {
                ret.Add(clauseInfo);
            }
            return ret;
        }

        private void addTextToLastTable(List<ITable> tables, string value) {
            if (tables.Count > 0) {
                var tbl = tables[tables.Count - 1];
                object text = null;
                if (tbl.ExtendedProperties.TryGetValue("text", out text)) {
                    text = ((string)text) + value;
                } else {
                    text = "\r\n\t" + value;
                }
                tbl.ExtendedProperties["text"] = text;
            }
        }

        private void addTextToLastTable(List<Join> joins, string value) {
            if (joins.Count > 0) {
                var j = joins[joins.Count - 1];
                var tbl = j.ToTable ?? j.FromTable;
                object text = null;
                if (tbl.ExtendedProperties.TryGetValue("text", out text)) {
                    text = ((string)text) + value;
                } else {
                    text = "\r\n\t" + value;
                }
                tbl.ExtendedProperties["text"] = text;
            }
        }

        private Join parseSingleJoin(List<string> topTokens, List<Join> joins, out List<string> tokensAfterOnClause){
            tokensAfterOnClause = new List<string>();
            ITable tbl = null;

            var join = new Join();

            for (var i = 0; i < topTokens.Count; i++) {
                var tok = topTokens[i];
                var tokLower = tok.ToLower();
                if (tokLower == "join" || tok.Trim().Length == 0 || tokLower == "as") {
                    // ignore, we already processed it
                } else if (SQL_JOIN_TYPES.Contains(tokLower)) {
                    join.JoinType = (JoinType)(Enum.Parse(typeof(JoinType), tok, true));
                    if (tbl != null) {
                        if (tbl.Mappings.Count > 0) {
                            Join.AddTable(tbl, joins, false, false, true);
                            joins[joins.Count - 1].JoinType = join.JoinType;
                            join = joins[joins.Count - 1];
                        }
                        tbl = null;
                        if (join.IsFull) {
                            tokensAfterOnClause = topTokens.Skip(i).ToList();
                            break;
                        }
                    }
                } else if (tokLower == ",") {
                    if (tbl != null) {
                        join.JoinType = JoinType.None;
                        if (tbl.Mappings.Count > 0) {
                            Join.AddTable(tbl, joins, false, false, true);
                            joins[joins.Count - 1].JoinType = join.JoinType;
                            join = joins[joins.Count - 1];
                        }
                        tbl = null;
                        if (join.IsFull) {
                            tokensAfterOnClause = topTokens.Skip(i).ToList();
                            break;
                        }
                    }
                } else if (tokLower == "on") {
                    // we're done inspecting for JOIN information until the next SQL_JOIN_TYPE is encountered...
                    tokensAfterOnClause = topTokens.Skip(i).ToList();
                    // HACK: an invalid table mapping will have no fields. if this is the case, ignore it...
                    if (tbl.Mappings.Count > 0) {
                        Join.AddTable(tbl, joins, false, false, true);
                        joins[joins.Count - 1].JoinType = join.JoinType;
                        join = joins[joins.Count - 1];
                    }
                    tbl = null;
                    break;
                } else {
                    // assume table name or table alias name
                    if (tbl != null) {
                        tbl.AliasName = tok;
                    } else {
                        // HACK: ignore qualified table names and SQL Server 'safe' delimiters...
                        var tblName = tok.Replace("[", "").Replace("]", "");
                        if (tblName.Contains(".")) {
                            tblName = Toolkit.Cut(tblName, tblName.LastIndexOf('.') + 1);
                        }
                        tbl = Table.Map(tblName, DataConnectionSpec, LanguageID, true); // brock
                        if (tbl != null) {
                            tbl = tbl.Clone();
                        }
                    }
                }
            }
            if (tbl != null) {
                if (join.JoinType == JoinType.Unknown) {
                    join.JoinType = JoinType.None;
                }
                if (tbl.Mappings.Count > 0) {
                    Join.AddTable(tbl, joins, false, false, true);
                    joins[joins.Count - 1].JoinType = join.JoinType;
                    join = joins[joins.Count - 1];
                }
            }
            return join;
        }

        private List<string> parseOnClauseTokens(List<string> topTokens, List<Join> joins) {

            var lastJoin = joins[joins.Count - 1];

            var operatorCount = 0;

            for (var i = 0; i < topTokens.Count; i++) {
                var tok = topTokens[i];
                var tokLower = tok.ToLower();

                if (tokLower == "on" || tok.Trim().Length == 0) {
                    // ignore
                } else if (SQL_JOIN_TYPES.Contains(tokLower)) {
                    // we're done, this is the next join type.
                    // rollback one and return it
                    return topTokens.Skip(i).ToList();
                } else if (SQL_LOGICAL_OPERATORS.Contains(tokLower)){
                    lastJoin.LastConditional.Operator = tok;
                    operatorCount++;
                } else if (SQL_COMPARATORS.Contains(tokLower)){
                    lastJoin.LastConditional.Comparator = tok;
                } else {

                    // assume this is the table.field or just field.
                    IField fld = null;
                    var tf = tok.Split('.');
                    var tbl = getTable(joins, tf[0]);
                    if (tbl != null) {
                        if (tf.Length > 1) {
                            // table.field
                            fld = tbl.GetField(tf[1]);
                        } else if (tbl == null) {
                            // just field
                            foreach (var t in Tables) {
                                fld = t.GetField(tf[0]);
                                if (fld != null) {
                                    break;
                                }
                            }
                        }
                    }

                    if (fld == null) {
                        fld = new Field { TableFieldName = tok };
                    }

                    if (lastJoin.Conditionals.Count < operatorCount + 1) {
                        lastJoin.Conditionals.Add(new Conditional { FieldA = fld });
                    } else {
                        lastJoin.LastConditional.FieldB = fld;

                        // most of the time the last table pulled in will have the FK to an existing table (i.e. last table is child of another one, which is its parent)
                        ITable parentTable = null;
                        IField childField = null;
                        foreach (var j in joins) {
                            if (j.FromTable != null) {
                                foreach (var ffk in j.FromTable.ForeignKeys) {
                                    if (ffk.AliasedTableFieldName == lastJoin.LastConditional.FieldA.AliasedTableFieldName) {
                                        parentTable = lastJoin.LastConditional.FieldB.Table;
                                        childField = ffk;
                                        break;
                                    } else if (ffk.AliasedTableFieldName == lastJoin.LastConditional.FieldB.AliasedTableFieldName) {
                                        parentTable = lastJoin.LastConditional.FieldA.Table;
                                        childField = ffk;
                                        break;
                                    }
                                }
                            }
                            if (j.ToTable != null) {
                                foreach (var tfk in j.ToTable.ForeignKeys) {
                                    if (tfk.AliasedTableFieldName == lastJoin.LastConditional.FieldA.AliasedTableFieldName) {
                                        parentTable = lastJoin.LastConditional.FieldB.Table;
                                        childField = tfk;
                                        break;
                                    } else if (tfk.AliasedTableFieldName == lastJoin.LastConditional.FieldB.AliasedTableFieldName) {
                                        parentTable = lastJoin.LastConditional.FieldA.Table;
                                        childField = tfk;
                                        break;
                                    }
                                }
                            }
                        }

                        Join.AddReferencingFieldProperty(parentTable, childField);

                        //if (lastJoin.ToTable == lastJoin.LastConditional.FieldA.Table) {
                        //    Join.AddReferencingFieldProperty(lastJoin.LastConditional.FieldB.Table, lastJoin.LastConditional.FieldA);
                        //} else {
                        //    Join.AddReferencingFieldProperty(lastJoin.LastConditional.FieldA.Table, lastJoin.LastConditional.FieldB);
                        //}
                    }
                }
            }
            return null;
        }

        protected List<Join> parseJoins(ClauseInfo fromClause) {

            // multi-pass processing...

            // treat sublevel selects as second-class citizens (i.e. don't parse them, treat them as a single token)
            var toks = getTopLevelTokens(fromClause.Tokens, false);

            var joins = new List<Join>();

            // parse everything up to the next join
            List<string> onTokens = toks;
            do {
                
                // parse the X join Y portion...
                parseSingleJoin(onTokens, joins, out onTokens);

                if (onTokens != null && onTokens.Count > 0 && onTokens[0].ToLower() == "on") {
                    // then the ON X.a = Y.a portion...
                    onTokens = parseOnClauseTokens(onTokens, joins);
                }

                // and keep doing this until we're out of tokens
            } while (onTokens != null && onTokens.Count > 0);


            return joins;
        }

        protected IField parseFieldText(string text, string lastWord) {

            // NOTE: this method was implemented hastily and horribly.  Don't try to fix it.  Please rewrite it.
            //       Contains hacks put in to parse DISTINCT and comments properly wherever they may appear.

            var fldName = text;
            if (lastWord.StartsWith("--") || lastWord.StartsWith("/*")) {
                // need to do an intermediate retokenization, the last word is a comment.
                var retokens = tokenize(text);
                if (retokens.Count > 1) {
                    lastWord = retokens[retokens.Count - 1];
                    retokens.RemoveAt(retokens.Count - 1);
                    text = String.Join("", retokens.ToArray());
                } else {
                    lastWord = "";
                    text = String.Join("", retokens.ToArray());
                }
            }

            if (lastWord.ContainsAny(new string[] { ")", "(" })) {
                text += lastWord;
                lastWord = "";
            }
            // text = "\r\n DISTINCT c.cooperator_id ", lastWord = " value_member\r\n"

            // check for:
            // DISTINCT(c.cooperator_id)
            // DISTINCT c.cooperator_id
            // DISTINCT c.cooperator_id as value_member
            // DISTINCT c.cooperator_id value_member
            // needs to emit:
            // fldName = value_member if specified, cooperator_id otherwise
            // text = "DISTINCT (c.cooperator_id)"

            // check for: TOP X
            var toks = tokenize(text.Trim());

            if (toks.Count > 1 && toks[0] == "(" && toks[1].ToLower() == "select") {
                // subselect...

                // parse the subselect so we can grab the tables.
                var subselect = new Select(String.Join("", toks.Skip(1).Take(toks.Count - 2).ToArray()), DataConnectionSpec, LanguageID, false, false);
                foreach(var t in subselect.Tables){
                    AddTableIfNeeded(t.TableName, t.AliasName, Tables, t.Clone());
                }
                fldName = lastWord;
            } else {


                var textParts = new List<string>();
                var foundAs = false;
                var nameParts = new List<string>();
                var aliasParts = new List<string>();
                var lastToken = "";
                var topNeedsNumbers = false;
                var foundRealFieldName = false;
                foreach (var tok in toks) {
                    if (SQL_KEYWORDS.Contains(tok.ToLower())) {
                        // found DISTINCT or TOP
                        textParts.Add(tok);
                        var toklower = tok.ToLower();
                        switch (toklower) {
                            case "top":
                                topNeedsNumbers = true;
                                break;
                            default:
                                break;
                        }
                    } else if (tok.Trim().Length == 0){
                        // whitespace
                        if (!foundAs){
                            textParts.Add(tok);
                        } else {
                            aliasParts.Add(tok);
                        }
                    } else if (tok.ToLower() == "as"){
                        // explicit alias, ignore
                        foundAs = true;
                    } else if (foundAs){
                        // must be the alias
                        aliasParts.Add(tok);
                    } else {
                        // brock 2010-12-30 changed because web_feedback_result_field is first dataview to have a field
                        // mapped that is a coalesce field (coalesce(frf.feedback_result_field_id, -1) feedback_result_field_id)
                        if (tok != ")" && tok != "(" && !foundRealFieldName) {
                            if (topNeedsNumbers) {
                                if (Toolkit.ToInt32(tok, -1) > -1) {
                                    // is a number and we were expecting it. (i.e. TOP statement)
                                    topNeedsNumbers = false;
                                    nameParts.Add(lastWord);
                                } else {
                                    // is not a number but we need numbers still... invalid sql???
                                }
                                textParts.Add(tok);
                                lastToken = "";
                            } else {
                                // must be the 'real' field name
                                foundRealFieldName = true;
                                nameParts.Add(tok);
                            }
                        } else {
                            textParts.Add(tok);
                            lastToken = tok;
                        }
                    }
                }



                if (textParts.Count > 1 && textParts[textParts.Count - 1] == lastToken) {
                    textParts.RemoveAt(textParts.Count - 1);
                }
                if (nameParts.Count > 1 && nameParts[nameParts.Count - 1] == lastToken) {
                    nameParts.RemoveAt(nameParts.Count - 1);
                    aliasParts.Add(lastToken);
                }
                var alias = String.Join("", aliasParts.ToArray()).Trim();
                var fieldText = String.Join("", textParts.ToArray());
                fldName = String.Join("", nameParts.ToArray()).Trim();
                if (alias.Length == 0){
                    alias = lastWord.Trim();
                    if (fldName.Length == 0) {
                        // DISTINCT a.accession_id
                        fldName = alias;
                        text += fldName;
                        alias = "";
                        lastWord = "";
                    }
                }

                if (alias.Length == 0){
                    alias = fldName;
                } else if (String.Compare(alias, fldName, true) != 0){

                }
            }


            var fld = getTableFieldMapping(fldName);
            if (fld != null) {
                fld.ExtendedProperties["text"] = text.Trim();
                //fld.ExtendedProperties["verbatim_text"] = text;
                //fld.ExtendedProperties["verbatim_name"] = lastWord;
                if (!String.IsNullOrEmpty(lastWord) && lastWord.Trim().Length > 0) {
                    fld.DataviewFieldName = lastWord.Trim();
                }
                if (String.IsNullOrEmpty(fld.DataviewFieldName)) {
                    fld.DataviewFieldName = fld.TableFieldName;
                }
            }
            return fld;
        }

        protected void parseFieldTextAndName(List<string> texts, ref string text, List<string> names, ref string name) {

            text = text.Replace("[", "").Replace("]", "");
            name = name.Replace("[", "").Replace("]", "");
            if (text.Trim() == name.Trim()) {
                // exact same text (i.e. no implicit or explicit AS clause).  e.g.: a.accession_id,
                texts.Add(text);
                names.Add(string.Empty);
            } else {
                // implicit or explicit AS clause. e.g.:   a.accession_id AS acid,
                text = Toolkit.Cut(text, 0, -1 * name.Length);

                texts.Add(text);

                //var temp = name.Split('.');
                //names.Add(temp[temp.Length - 1]);

                names.Add(name);

            }

            text = string.Empty;
            name = string.Empty;

        }

        protected void parseAndAssignFields(ClauseInfo clause, List<ITable> tables) {

            var fieldTexts = new List<string>();
            var lastText = string.Empty;

            var fieldWords = new List<string>();
            var lastWord = string.Empty;

            foreach (var tok in clause.Tokens) {
                var tokLower = tok.ToLower();
                // pluck out field name(s) as needed
                switch (tokLower) {
                    case "as":
                        // always skip the 'as' keyword, don't add to text or word.
                        break;
                    case ",":
                        // last word would have been lumped into the lastText variable, so yank it back out
                        parseFieldTextAndName(fieldTexts, ref lastText, fieldWords, ref lastWord);
                        break;
                    default:
                        // append it to the text,
                        // mark it as the last word
                        lastText += tok;
                        if (tok.Trim().Length == 0) {
                            lastWord += tok;
                        } else {
                            lastWord = tok;
                        }
                        break;
                }
            }

            if (!String.IsNullOrEmpty(lastText) && lastText.Trim().Length > 0) {
                // don't forget the very last field (in case they haven't entered a FROM clause yet)
                parseFieldTextAndName(fieldTexts, ref lastText, fieldWords, ref lastWord);
            }


            // by this point, we've gathered up all the relevant into pairs of text & word.
            // so the two lists will always be exactly the same length and consist of data like the following:
            // text                                                    word (aka dataview field name)
            // ====================================================== =========================
            // aa.accession_id                                        acid
            // concat('hi', 'from', 'dad')                            blah
            // ltrim (rtrim (a.accession_number_part1)) + 'joe'             hibbity

            Fields = new List<IField>();
            for (var i = 0; i < fieldTexts.Count; i++) {
                // parseFieldText may auto-add tables and fields from subselects so the user can select them
                // in the admin tool (think of wanting to write to a field which is calculated from a subselect)
                var field = parseFieldText(fieldTexts[i], fieldWords[i]);
                if (field != null) {
                    Fields.Add(field);
                }
            }

        }

        public ITable GetTable(string tableOrAliasNameOrID) {
            return getTable(Tables, tableOrAliasNameOrID);
        }

        public ITable getTable(List<ITable> tables, string tableOrAliasNameOrID) {
            foreach (var t in tables) {
                if (IsSameTableName(t, tableOrAliasNameOrID)) {
                    return t;
                }
            }
            return null;
        }

        public ITable getTable(List<Join> joins, string tableOrAliasNameOrID) {
            foreach (var j in joins) {
                if (IsSameTableName(j.ToTable, tableOrAliasNameOrID)) {
                    return j.ToTable;
                } else if (IsSameTableName(j.FromTable, tableOrAliasNameOrID)) {
                    return j.FromTable;
                }
            }
            return null;
        }

        internal ITable AddTable(ITable fmt, bool errorOnMissingRelationship, bool errorOnMultipleJoins, Join joinToUse) {
            var tbl = fmt.Clone();
            addTableNoClone(Joins, tbl, true, errorOnMissingRelationship, errorOnMultipleJoins, joinToUse);
            return tbl;
        }

        protected Join addTableNoClone(List<Join> joins, ITable fmt, bool addAllFields, bool errorOnMissingRelationship, bool errorOnMultipleRelationships, Join joinToUse) {
            // never called at parse time
            var t = getTable(joins, fmt.TableName);
            t = fmt;
            calculateNewAlias(t);

            if (joinToUse != null) {
                if (joins.Count == 0) {
                    joinToUse.IsFirstInClause = true;
                    joins.Add(joinToUse);
                } else {
                    // they gave us a join with no FromTable.
                    // try to combine it with the last one.  if that fails, just append it
                    if (!joins[joins.Count - 1].Combine(joinToUse)) {
                        joins.Add(joinToUse);
                    }
                }

            } else {
                Join.AddTable(t, joins, errorOnMissingRelationship, errorOnMultipleRelationships, false);
                //if (joins.Count == 0) {
                //    // first table.
                //    var newjoin = new Join();
                //    newjoin.AddTable(t, joins, errorOnMissingRelationship, errorOnMultipleRelationships);
                //    joins.Add(newjoin);
                //} else if (joins.Count == 1 && joins[0].ToTable == null) {
                //    // second table of first join
                //    joins[0].AddTable(t, joins, errorOnMissingRelationship, errorOnMultipleRelationships);
                //} else {
                //    // third or greater table.  we simply add on a new join 
                //    var newjoin = new Join();
                //    newjoin.AddTable(t, joins, errorOnMissingRelationship, errorOnMultipleRelationships);
                //    joins.Add(newjoin);
                //}
            }

            Tables.Add(t);


            if (t.Mappings.Count > 0) {
                if (addAllFields) {
                    foreach (var fm in t.Mappings) {
                        addFieldNoClone(joins, t, fm, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse, false);
                    }
                }
            }
            return (joins.Count == 0 ? null : joins[joins.Count - 1]);
        }

        public IField AddFieldIfNeeded(string tableAlias, string fieldName, ITable table, IField newField) {
            if (table == null) {
                // no table given to apply it to, assume no field exists
                return null;
            }
            var f = Fields.Find(fld => { return fld.AliasedTableFieldName == tableAlias + "." + fieldName; });
            if (f != null) {
                // field already exists in the collection, make sure the tablefieldid is assigned correctly
                foreach(IField tf in table.Mappings){
                    if (tf.TableFieldName == f.TableFieldName){
                        f.TableFieldID = tf.TableFieldID;
                        break;
                    }
                }
            } else {
                //foreach (var m in table.Mappings) {
                //    if (m.DataviewFieldName.ToLower() == fieldName.ToLower() 
                //        || m.TableFieldName.ToLower() == fieldName.ToLower()
                //        ) {
                //        f = m;
                //        break;
                //    }
                //}
                f = table.Mappings.Find(fld => { return fld.TableFieldName == fieldName; });
                if (f == null) {
                    // invalid field name, ignore
                    // use the given field object if possible
                    if (newField != null) {
                        Fields.Add(newField);
                    }
                } else {
                    // found the field, doesn't exist in dataview field list, add it.
                    Fields.Add(f.Clone(f.Table.Clone()));
                }
            }
            return f;
        }

        public void RemoveField(string dataviewFieldName) {
            for (var i = 0; i < Fields.Count; i++) {
                var fld = Fields[i];
                if (IsSameFieldName(fld, dataviewFieldName)) {
                    Fields.Remove(fld);
                    break;
                }
            }
        }

        public IField AddField(IField fm, bool errorOnMissingRelationship, bool errorOnMultipleRelationships, Join joinToUse) {
            IField rv = null;
            if (fm.Table == null) {
                rv = fm.Clone(null);
                addFieldNoClone(Joins, null, rv, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse, true);
            } else {
                var tClone = fm.Table.Clone();
                rv = fm.Clone(tClone);
                addFieldNoClone(Joins, tClone, rv, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse, true);
            }
            return rv;
        }



        protected void addFieldNoClone(List<Join> joins, ITable fmt, IField fm, bool errorOnMissingRelationship, bool errorOnMultipleRelationships, Join joinToUse, bool autoAddTableAsNeeded) {

            if (fm.Table != null) {
                var t = getTable(joins, fmt.TableName);
                if (t == null) {
                    addTableNoClone(joins, fmt, false, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse);
                }
            }

            if (String.IsNullOrEmpty(fm.DataviewFieldName)) {

                // see if another field in the collection already uses the tablefieldname.
                // if it does, set the proper alias and use it
                IField existingField = null;
                var anotherFieldHasSameName = false;
                foreach (var f in Fields) {
                    if (f.TableFieldName == fm.TableFieldName) {
                        anotherFieldHasSameName = true;
                        if (f.TableName == fm.TableName){
                            existingField = f;
                            if (existingField.Table == null) {
                                existingField.Table = GetTable(f.TableName);
                            }
                            break;
                        }
                    }
                }
                if (existingField == null) {
                    // field exists. just assume the first alias as the correct one.
                    foreach (var t in Tables) {
                        if (t.TableName == fmt.TableName) {
                            fm.Table = t;
                            break;
                        }
                    }
                    if (anotherFieldHasSameName && fm.Table != null) {
                        fm.DataviewFieldName = fm.Table.AliasName + "_" + fm.TableFieldName;
                    } else {
                        fm.DataviewFieldName = fm.TableFieldName;
                    }
                } else {

                    // they already have this field.  see if any other aliases exist for that table, use next available.
                    var found = false;
                    foreach (var t in Tables) {
                        if (t.TableName == existingField.TableName) {
                            if (t.AliasName != existingField.Table.AliasName) {
                                var useThisOne = true;
                                foreach (var f in Fields) {
                                    if (f.TableFieldName == existingField.TableFieldName && t.AliasName == f.Table.AliasName) {
                                        useThisOne = false;
                                        break;
                                    }
                                }
                                if (useThisOne) {
                                    fm.Table = t;
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (autoAddTableAsNeeded) {
                        if (!found) {
                            // if not, assume they want to join in another alias for it
                            // (think accession_pedigree -> accession, accession_pedigree has 3 keys to accession)
                            addTableNoClone(joins, fm.Table, false, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse);
                        }
                    }

                    fm.DataviewFieldName = fm.Table.AliasName + "_" + fm.TableFieldName;
                    existingField = GetField(fm.DataviewFieldName);
                    var i = 1;
                    while (existingField != null) {
                        fm.DataviewFieldName = fm.Table.AliasName + "_" + fm.TableFieldName + "_" + i;
                        i++;
                        existingField = GetField(fm.DataviewFieldName);
                    }
                }


            }

            fm.ExtendedProperties["text"] = String.Format("{0}.{1}", fm.Table.AliasName, fm.TableFieldName);

            if (!Fields.Contains(fm)) {
                Fields.Add(fm);
            }
        }


        protected void calculateNewAlias(ITable fmt) {
            var i = 0;
            string newAlias = null;
            while (newAlias == null) {
                newAlias = fmt.AliasName + (i > 0 ? i.ToString() : string.Empty);
                if (__allSqlWords.Contains(newAlias.ToLower()) || newAlias.ToLower() == "as") {
                    newAlias = null;
                    i++;
                } else {
                    foreach (var t in Tables) {
                        if (String.Compare(t.AliasName, newAlias, true) == 0) {
                            newAlias = null;
                            i++;
                            break;
                        }
                    }
                }
            }
            fmt.AliasName = newAlias;
        }

        protected void calculateFieldText(List<string> fieldTokens, Clause clause, List<string> fieldSqls) {
            // last word would have been lumped into the lastText variable, so yank it back out

            // we need to split the field text out to the sql part and the output field name part. e.g.:
            //
            // a.accession_id
            // a.accession_id as acid
            // convert(nvarchar, a.accession_number) + 'joe' + a.accession_number_part3 suff
            // accession_number_part1
            // accession_number_part1 || a.accession_number_part3 presuf

            var sb = new StringBuilder();

            IField f = null;

            if (fieldTokens.Count == 0) {
                // nothing to do
            } else {

                var foundAs = false;
                var sbText = new StringBuilder();
                var name = string.Empty;
                var sbAfterName = new StringBuilder();

                foreach (var tok in fieldTokens) {
                    if (tok.Trim().Length > 0) {
                        if (String.Compare(tok, "as", true) == 0) {
                            foundAs = true;
                            sbAfterName = new StringBuilder();
                        } else {
                            name = tok;
                        }

                    }
                    if (!foundAs) {
                        sbText.Append(tok);
                    }

                    if (tok.Trim().Length == 0) {
                        sbAfterName.Append(tok);
                    }

                }

                var fieldText = sbText.ToString();

                if (name.Trim().Length == 0) {
                    // there was no AS clause or it was invalid (did not specify a field name).
                    // just append the text as-is
                    sb.Append(fieldText);
                } else {

                    // we now have our field text and field name filled properly.

                    if (name.Contains(".")) {
                        // drop off table name as needed
                        var temp = name.Split('.');
                        name = temp[temp.Length - 1];
                    }

                    f = GetField(name);
                    if (f == null) {
                        f = new Field();
                        f.ExtendedProperties["verbatim_text"] = fieldText;
                        f.ExtendedProperties["verbatim_name"] = name;
                        f.DataviewFieldName = name.Trim();
                        Fields.Add(f);
                    }

                    object verbatimName = null;
                    string nm = null;
                    if (f.ExtendedProperties.TryGetValue("verbatim_name", out verbatimName)) {
                        nm = verbatimName.ToString().Replace(name, f.DataviewFieldName);
                    }

                    if (String.IsNullOrEmpty(nm)) {
                        // no AS clause was specified, meaning we use the DataviewFieldName if it exists.
                        if (!String.IsNullOrEmpty(f.DataviewFieldName)) {
                            nm = f.DataviewFieldName + sbAfterName.ToString();
                        } else {
                            // otherwise we just use the same name as the table field name (w/o "<table>." prefix, if any)
                            nm = name + sbAfterName.ToString();
                        }
                    }
                    f.ExtendedProperties["verbatim_name"] = nm;
                    f.ExtendedProperties["verbatim_text"] = fieldText;

                    var nameParts = nm.Split('.');
                    var justName = nameParts[nameParts.Length - 1];
                    var newText = string.Empty;
                    if (fieldText.Trim() == "*") {
                        newText = fieldText;
                    } else {
                        newText = applyFieldAlias(fieldText, justName);
                    }

                    sb.Append(newText);

                }
            }

            var inserted = false;
            for (var i = 0; i < Fields.Count; i++) {
                if (Fields[i] == f) {
                    if (i > fieldSqls.Count) {
                        fieldSqls.Add(sb.ToString());
                    } else {
                        fieldSqls.Insert(i, sb.ToString());
                    }
                    inserted = true;
                    break;
                }
            }
            if (!inserted) {
                fieldSqls.Add(sb.ToString());
            }


            fieldTokens.Clear();
        }

        protected void appendFieldCollection(StringBuilder sb, List<string> fieldSqls) {
            for (var i = 0; i < fieldSqls.Count; i++) {
                sb.Append(fieldSqls[i].TrimEnd(WHITESPACE));
                if (i < fieldSqls.Count - 1) {
                    sb.Append(",\r\n  ");
                }
            }
            if (sb.Length > 0 && !WHITESPACE.Contains(sb[sb.Length - 1])) {
                sb.AppendLine();
            }
            fieldSqls.Clear();

        }

        public abstract string Regenerate(bool errorOnMissingRelationship, bool errorOnMultipleRelationships);

    }
}
