using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business.SqlParser {
    /// <summary>
    /// Represents a single UNION clause in a SQL statement.  Assumes SQL statement is a SELECT.
    /// </summary>
    public class Select : SqlBase {

        public int StartsAtCharIndex;
        public int EndsAtCharIndex;
        public int QueryOffset;

        public Select(string sql, DataConnectionSpec dcs, int languageID, bool errorOnMissingRelationship, bool errorOnMultipleRelationships)
            : base(sql, dcs, languageID, errorOnMissingRelationship, errorOnMultipleRelationships) {
        }

        public override void Parse(List<string> tokens, bool isTopLevelTokens, bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {

            if (!isTopLevelTokens){
                tokens = getTopLevelTokens(tokens, false);
            }
            // parse by major clauses (fill Clauses collection)
            Clauses = parseClauses(tokens);

            // find From clause, parse it first (fill Tables and Relationships collections)
            foreach (var clause in Clauses) {
                switch (clause.Clause) {
                    case Clause.From:
                        Joins = parseJoins(clause);
                        Tables = new List<ITable>();
                        foreach (var j in Joins) {
                            if (j.FromTable != null) {
                                if (!Tables.Contains(j.FromTable)) {
                                    Tables.Add(j.FromTable);
                                }
                            }
                            if (j.ToTable != null) {
                                if (!Tables.Contains(j.ToTable)) {
                                    Tables.Add(j.ToTable);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            // find Select clause, parse it next (fill Fields collection)
            foreach (var clause in Clauses) {
                switch (clause.Clause) {
                    case Clause.Select:
                        parseAndAssignFields(clause, Tables);
                        break;
                    default:
                        break;
                }
            }

            // make sure our SQL matches our collections
            SQL = String.Join("", tokens.ToArray());
        }

        public override string Regenerate(bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            throw new NotImplementedException();
        }

        public string regenerate(int startOffset, List<IDataviewParameter> parameters, bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            this.StartsAtCharIndex = startOffset;
            var sb = new StringBuilder();
            if ((SQL == null || SQL.Trim().Length == 0) && Tables.Count > 0) {
                // just get something in there to start with
                SQL = Tables[0].GetSQL(false);
            }


            // first, make sure all parameters are up to snuff.
            var sql = SQL;


            // replace any outdated parameter names we might have
            foreach (var prm in parameters) {
                object prevName = null;
                if (prm.ExtendedProperties.TryGetValue("previous_name", out prevName)) {
                    if (prevName != null) {
                        if (prevName.ToString() != prm.Name) {
                            var sbPrm = new StringBuilder();
                            var tokens = tokenize(sql);
                            foreach (var tok in tokens) {
                                if (tok == prevName.ToString()) {
                                    sbPrm.Append(prm.Name);
                                } else {
                                    sbPrm.Append(tok);
                                }
                            }
                            sql = sbPrm.ToString();
                        }
                    }
                }
            }

            SQL = sql;


            foreach (var ci in parseClauses(getTopLevelTokens(tokenize(SQL), false))) {
                switch (ci.Clause) {
                    case Clause.Select:
                        // ignore all tokens in the SQL -- use our Fields collection
                        sb.AppendLine("SELECT").Append("  ");
                        for (var i = 0; i < Fields.Count; i++) {
                            var f = Fields[i];
                            var txt = Fields[i].DataviewFieldName;
                            var nm = Fields[i].DataviewFieldName;
                            object val = null;
                            if (f.ExtendedProperties.TryGetValue("text", out val)){
                                txt = val as string;
                            }
                            if (txt.Trim() == "*"){
                                sb.Append(txt);
                            } else {
                                sb.Append(applyFieldAlias(txt, nm));
                            }
                            if (i < Fields.Count - 1) {
                                sb.Append(",\r\n  ");
                            }
                        }
                        sb.AppendLine("");
                        break;
                    case Clause.From:
                        // ignore all tokens in the SQL -- use our Tables collection
                        sb.AppendLine("FROM");
                        foreach(var join in Joins){
                            sb.Append(join.ToString());
                        }
                        sb.AppendLine("");
                        break;
                    default:
                        // we don't munge any of the other clauses at all -- just pass them through
                        sb.Append(ci.Clause.ToString().ToUpper());
                        sb.Append(String.Join("", ci.Tokens.ToArray()));
                        break;
                }
            }
            SQL = sb.ToString();
            EndsAtCharIndex = SQL.Length + StartsAtCharIndex;
            return SQL;
        }

    }
}
