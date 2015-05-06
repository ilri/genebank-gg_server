using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using GrinGlobal.Interface.Dataviews;

namespace GrinGlobal.Business.SqlParser {
    /// <summary>
    /// Represents an entire SQL statement, possibly including multiple queries UNIONed together
    /// </summary>
    public class Query : SqlBase {

        public List<Select> Selects { get; private set; }

        public Query(string sql, DataConnectionSpec dcs, int languageID, bool errorOnMissingRelationship, bool errorOnMultipleRelationships)
            : base(sql, dcs, languageID, errorOnMissingRelationship, errorOnMultipleRelationships) {
        }

        /// <summary>
        /// Given a char index, returns the Select object to which it corresponds
        /// </summary>
        /// <param name="charIndex"></param>
        /// <returns></returns>
        public Select GetStatement(int charIndex) {
            foreach (var sel in Selects) {
                if (charIndex > sel.StartsAtCharIndex && charIndex < sel.EndsAtCharIndex) {
                    return sel;
                }
            }
            return null;
        }

        /// <summary>
        /// Determines which Select is the proper one, then adds the table to it.  Also adds to Query Tables collection if needed.  Optionally errors if no relationship exists between existing tables and the new one.
        /// </summary>
        /// <param name="charIndex"></param>
        /// <param name="fmt"></param>
        /// <param name="errorOnMissingRelationship"></param>
        /// <returns></returns>
        public string AddTable(int charIndex, ITable fmt, bool errorOnMissingRelationship, bool errorOnMultipleRelationships, Join joinToUse) {
            var stmt = GetStatement(charIndex);
            if (stmt == null) {
                stmt = new Select(null, DataConnectionSpec, LanguageID, errorOnMissingRelationship, errorOnMultipleRelationships);
                Selects.Add(stmt);
            }
            var tbl = stmt.AddTable(fmt, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse);
            if (GetTable(Toolkit.Coalesce(tbl.AliasName, tbl.TableName) as string) == null) {
                this.Tables.Add(tbl);
            }
            if (stmt == Selects[0]) {
                foreach (var fm in stmt.Fields) {
                    if (GetField(fm.DataviewFieldName) == null) {
                        this.Fields.Add(fm);
                    }
                }
            }

            return Regenerate(errorOnMissingRelationship, errorOnMultipleRelationships);
        }


        /// <summary>
        /// Determines which Select is the proper one, then adds the field to it, and corresponding table if need be. Also adds to Query Fields collection if needed.  Errors if no relationship exists between existing tables and the one to which the new field belongs
        /// </summary>
        /// <param name="charIndex"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        public string AddField(int charIndex, IField fm, bool errorOnMissingRelationship, bool errorOnMultipleRelationships, Join joinToUse) {
            var stmt = GetStatement(charIndex);
            if (stmt == null) {
                stmt = new Select(null, DataConnectionSpec, LanguageID, errorOnMissingRelationship, errorOnMultipleRelationships);
                Selects.Add(stmt);
            }
            var fld = stmt.AddField(fm, errorOnMissingRelationship, errorOnMultipleRelationships, joinToUse);

            if (GetTable(fld.Table.TableName) == null) {
                this.Tables.Add(fld.Table);
            }
            if (Selects[0] == stmt) {
                // only add fields from first select
                if (GetField(fld.DataviewFieldName) == null) {
                    this.Fields.Add(fld);
                }
            }
            return Regenerate(errorOnMissingRelationship, errorOnMultipleRelationships);
        }

        public override void Parse(List<string> tokens, bool isTopLevelTokens, bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {

            if (isTopLevelTokens) {
                throw new InvalidOperationException(getDisplayMember("Parse{toplevel}", "Query.Parse must be given all tokens, not just toplevel ones"));
            }

            // re-init collections
            Selects = new List<Select>();
            Tables = new List<ITable>();
            Fields = new List<IField>();

            // parse out all parameters, save as a single collection for all UNION clauses
            Parameters = parseParameters(tokens);

            // Parse out all UNION clauses
            var unions = parseUnions(tokens);
            for(var i=0;i<unions.Count;i++){
                var unionInfo = unions[i];
                // for each UNION clause, parse the select statement
                var stmt = new Select(null, DataConnectionSpec, LanguageID, errorOnMissingRelationship, errorOnMultipleRelationships);
                stmt.StartsAtCharIndex = unionInfo.StartOffset;
                stmt.EndsAtCharIndex = unionInfo.EndOffset;
                stmt.QueryOffset = Selects.Count;
                stmt.Parse(unionInfo.Tokens, true, errorOnMissingRelationship, errorOnMultipleRelationships);

                // remember all tables as a single collection
                Tables.AddRange(stmt.Tables);

                if (Fields.Count == 0) {
                    // first union clause fills the Fields collection
                    foreach (var f in stmt.Fields) {
                        Fields.Add(f);
                    }
                }

                // always copy verbatim_text to union-specific spot
                for (var j = 0; j < stmt.Fields.Count; j++) {
                    if (j < this.Fields.Count) {
                        Fields[j].ExtendedProperties["text_union_" + i] = stmt.Fields[j].ExtendedProperties["text"];
//                        Fields[j].ExtendedProperties["verbatim_name_union_" + i] = stmt.Fields[j].ExtendedProperties["verbatim_name"];
                    }
                }

                Selects.Add(stmt);
            }
        }

        public override string Regenerate(bool errorOnMissingRelationship, bool errorOnMultipleRelationships) {
            var sb = new StringBuilder();

            foreach(var sel in Selects){
                if (sb.Length > 0){
                    sb.AppendLine(" UNION ");
                }
                sb.AppendLine(sel.regenerate(sb.Length, this.Parameters, errorOnMissingRelationship, errorOnMultipleRelationships));
            }

            SQL = sb.ToString();
            return SQL;
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "Query", resourceName, null, defaultValue, substitutes);
        }

    }
}
