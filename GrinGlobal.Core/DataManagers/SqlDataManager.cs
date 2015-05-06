using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GrinGlobal.Core.DataManagers {
#if !DEBUGDATAMANAGER
		[DebuggerStepThrough()]
#endif
	internal class SqlDataManager : DataManager {
        internal SqlDataManager(DataConnectionSpec dcs)
			: base(dcs, "User Id", "Password", DataVendor.SqlServer) {
        }

		protected internal override void clearPool(IDbConnection conn) {
			SqlConnection.ClearPool((SqlConnection)conn);
		}

		protected internal override IDbCommand createCommand(string text) {
            return new SqlCommand(text);
        }

		protected internal override IDbDataParameter createParam(IDbCommand cmd, string name, object val, DbType dbType) {
			string newName = name.Replace(':', '@').Replace('?', '@');
			cmd.CommandText = cmd.CommandText.Replace(name, newName);
			//if (val is DateTime && ((DateTime)val).Year < 1753) {
			//    val = DBNull.Value;
			//}
            var prm = new SqlParameter(newName, val);
            prm.DbType = dbType;
            return prm;
        }

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
            return new SqlDataAdapter((SqlCommand)cmd);
        }

		protected internal override IDbConnection createConn(string connString)	 {
            return new SqlConnection(connString);
        }

		protected internal override void mungeParameter(IDbCommand cmd, IDbDataParameter prm) {
			// do nothing -- yet
		}

		protected internal override void mungeSql(IDbCommand cmd) {

            if (String.IsNullOrEmpty(cmd.CommandText)) {
                return;
            }

            string lc = cmd.CommandText.ToLower();
            if (!lc.Contains("concat") && !lc.Contains("trim")) {
                // no need to inspect further, don't need to munge CONCAT() or TRIM().
                return;
            }


            List<string> parsed = cmd.CommandText.SplitRetain(new char[] { ' ', '(', ')', '\r', '\n', '\t', ',' }, true, true, false);


            // so "select * from concat(coalesce(col1,''), trim('  hi '))"
            // should come out as:
            //    { "select", " ", "*", " ", "from", " ", 
            //      "concat", "(", "coalesce", "(", "col1", ",", 
            //      "''", ")", ",", " ", "trim", "(", "'  hi '", 
            //      ")", ")" }


            StringBuilder sb = new StringBuilder();

            Stack<int> concatMatchEnds = new Stack<int>();
            Stack<int> concatParens = new Stack<int>();

            Stack<int> trimMatchEnds = new Stack<int>();
//            Stack<int> trimParens = new Stack<int>();

            int parenDepth = 0;

            for (int i=0;i<parsed.Count;i++){

                string s = parsed[i];
                string lower = s.ToLower();

                if (s == "(") {
                    parenDepth++;
                }
                
                if (lower.StartsWith("'") || lower.StartsWith(@"""")) {
                    // quoted value, ignore / add to sb
                    sb.Append(s);
                } else if (lower == ("concat")) {
                    // convert CONCAT('hi', 'there') into 'hi' + 'there'
                    // skip until parsed[i] = '(' (only happens if whitespace is between concat and (, such as "concat   ("
                    int parenPos = i;
                    while (parenPos < parsed.Count && parsed[parenPos] != "(") {
                        parenPos++;
                    }
                    if (parenPos < parsed.Count) {
                        int concat = findMatchingParen(parsed, parenPos);
                        if (parenPos < concat) {
                            concatMatchEnds.Push(concat);
                            concatParens.Push(parenDepth + 1);
                        }
                    }
                } else if (lower == ("trim")){
                    // convert TRIM('  hi ') into RTRIM(LTRIM('  hi '))
                    // skip until parsed[i] = '(' (only happens if whitespace is between concat and (, such as "concat   ("
                    int parenPos = i;
                    while (parenPos < parsed.Count && parsed[parenPos] != "(") {
                        parenPos++;
                    }
                    if (parenPos < parsed.Count) {
                        int trim = findMatchingParen(parsed, parenPos);
                        if (parenPos < trim) {
                            sb.Append("RTRIM(LTRIM");
                            trimMatchEnds.Push(trim);
//                            trimParens.Push(parenDepth + 1);
                        }
                    }
                } else {

                    bool next = false;
                    if (concatMatchEnds.Count > 0) {
                        int concatMatch = concatMatchEnds.Peek();
                        int concatDepth = concatParens.Peek();
                        if (i < concatMatch && parenDepth == concatDepth) {
                            // we're inside a concat, but not too deep to be in another sub function.
                            if (parsed[i] == ",") {
                                sb.Append(" + ");
                                i++;
                            } else {
                                sb.Append(s);
                            }
                            next = true;
                        } else if (i == concatMatch){
                            concatMatchEnds.Pop();
                            concatParens.Pop();
                            sb.Append(")");
                            next = true;
                        }
                    }
                    if (!next) {
                        if (trimMatchEnds.Count > 0) {
                            int trimMatch = trimMatchEnds.Peek();
                            if (i == trimMatch) {
                                trimMatchEnds.Pop();
                                sb.Append("))");
                                next = true;
                            }
                        }
                    }

                    if (!next){
                        // no text found that needs munging, just copy it over
                        sb.Append(s);
                    }
                }

                if (s == ")") {
                    parenDepth--;
                }

            }

            cmd.CommandText = sb.ToString();

		}

		public override string Concatenate(string[] values) {
			StringBuilder sb = new StringBuilder();
			foreach (string v in values) {
				sb.Append("coalesce(")
					.Append(v)
					.Append(",'') + ");
			}
			if (sb.Length > 3) {
				sb.Remove(sb.Length - 3, 3);
			}
			return sb.ToString();
		}

        // match:
        // select           --  only at beginning of sql statement
        // select distinct  --  only at beginning of sql statement
        // union select     --  anywhere in sql statement
        // union select     --  distinct anywhere in sql statement
        private static Regex __SELECT = new Regex(@"(^[\r\n\t ]*select[\r\n\t ]+(distinct|all)?|union[\r\n\t ]+select[\r\n\t ]+(distinct|all)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex __TOP_TOP = new Regex(@"(top \(__LIMIT__\) top)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		protected internal override void applyLimit(IDbCommand cmd, int limit, int offset) {
			if (offset < 1) {
                string sql = __SELECT.Replace(cmd.CommandText, "$1 TOP (" + limit + ") ");
                if (sql.ToLower().Contains(" top (" + limit + ") top")) {
                    sql = new Regex(@"(top \(" + limit + @"\) top)", RegexOptions.IgnoreCase).Replace(sql, "top");
                }
                cmd.CommandText = "select * from ( " + sql + " ) as table__0";
//                cmd.CommandText = "select * from ( select TOP (" + limit + ") " + cmd.CommandText.Trim().Substring(6) + " ) as table__0 ";

                //int orderByPos = cmd.CommandText.ToLower().IndexOf("order by");
                //if (orderByPos > -1) {
                //    cmd.CommandText = "select TOP (" + limit + ") * from ( " + cmd.CommandText.Trim().Insert(orderByPos, " ) as table__0 ");
                //} else {
                //    cmd.CommandText = "select TOP (" + limit + ") * from ( " + cmd.CommandText.Trim() + " ) as table__0 ";
                //}
			} else {
				// HACK: sql server supports only TOP (aka limit) so we need to do some creative sql to emulate offset...

                // we essentially do triple-nested select (i.e. 3 common table expressions).  However, if they have an order by specified, we
                // will get the following error if we do not specify a TOP in the innermost common table expression.
                //
                // Msg 1033, Level 15, State 1, Line 26
                // The ORDER BY clause is invalid in views, inline functions, derived tables, subqueries, and common table expressions, unless TOP or FOR XML is also specified.
                //
                // So instead of trying to detect this, we just always put an extra common table expression with the proper TOP clause to satisfy SQL Server -- so we end up with 4 common table expressions:
                // TOP (limit + offset) in original order by
                // TOP (limit + offset) order by 1st column
                // TOP (limit) order by 1st column desc (to chop off the offset rows)
                // TOP (limit) order by 1st column asc (to put back into original order by order)
                //
                // NOTE: to properly order rows using limit and offset in sql server via DataManager, the original sql MUST use the first column in the output as its primary order by!!!!

                string sql = __SELECT.Replace(cmd.CommandText, "$1 TOP (" + (offset + limit) + ") ");


                cmd.CommandText = @"
select TOP (" + limit + @") * from (
	select TOP (" + limit + @") * from (
				select  TOP (" + (offset + limit) + @") * from ( " + sql + @") as table__0 order by 1 asc
	) as table__1 order by 1 desc
) as table__2 order by 1 asc";

//                int orderByPos = cmd.CommandText.ToLower().IndexOf("order by");
//                if (orderByPos > -1) {
//                    cmd.CommandText = @"
//select * from (
//	select top (" + limit + @") * from (
//				select TOP (" + (offset + limit) + @") * from ( " + cmd.CommandText.Trim().Insert(orderByPos, " ) as table__0 ") + @"
//	) as table__1 order by 1 desc
//) as table__2 order by 1 asc
//";
//                } else {
//                    cmd.CommandText = @"
//select * from (
//	select top (" + limit + @") * from (
//				select TOP (" + (offset + limit) + @") * from (" + cmd.CommandText.Trim() + @") as table__0 order by 1 asc
//	) as table__1 order by 1 desc
//) as table__2 order by 1 asc
//";
//                }

                
			}
		}

		protected internal override void enableReturnId(IDbCommand cmd, string nameOfSequenceOrPrimaryKey, ref DataParameters dps) {
			if (cmd.CommandType == CommandType.Text) {
				cmd.CommandText += "; select scope_identity();";
			}
		}

		protected internal override object executeInsert(IDbCommand cmd) {
			object ret = cmd.ExecuteScalar();
			return ret;
		}

//        protected override void initConnectionString(DataConnectionSpec dcs, string serverName, string databaseName) {

//            string connString = "Data Source=" + serverName + ";";

//            if (!String.IsNullOrEmpty(databaseName)) {
//                connString += "Initial Catalog=" + databaseName + ";";
//            }

//            if (dcs.UseWindowsAuthentication) {
////            if (dcs.UseWindowsAuthentication || String.IsNullOrEmpty(dcs.UserName)) {
////                connString += "Integrated Security=SSPI;";
//                connString += "Trusted_Connection=True;";
//            } else {
//                connString += dcs.UserNameMoniker + "=;" + dcs.PasswordMoniker + "=;";
//                connString = base.replaceNameValuePair(connString, dcs.UserNameMoniker, dcs.UserName);
//                connString = base.replaceNameValuePair(connString, dcs.PasswordMoniker, dcs.Password);
//            }

//            dcs.ConnectionString = connString;
//        }

        protected override void mungeConnectionSpec(DataConnectionSpec dcs) {
            if (String.IsNullOrEmpty(dcs.UserName) && !String.IsNullOrEmpty(dcs.ConnectionString)) {
                //if (!dcs.ConnectionString.ToLower().Contains("integrated security=sspi")) {
                //    dcs.ConnectionString = dcs.ConnectionString += ";Integrated Security=SSPI";
                //}
                if (!dcs.ConnectionString.ToLower().Contains("trusted_connection=true")) {
                    dcs.ConnectionString = dcs.ConnectionString += ";Trusted_Connection=True";
                }
            }
        }
    }
}
