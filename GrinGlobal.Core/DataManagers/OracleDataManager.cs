using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GrinGlobal.Core.DataManagers {
#if !DEBUGDATAMANAGER
	[DebuggerStepThrough()]
#endif
	internal class OracleDataManager : DataManager {
		internal OracleDataManager(DataConnectionSpec dcs)
//			: base(dcs, "uid", "pwd", DataVendor.Oracle) {
			: base(dcs, "User Id", "password", DataVendor.Oracle) {
		}

        protected override void mungeConnectionSpec(DataConnectionSpec dcs) {
            //// make it a privileged connection if need be
            //if (("" + dcs.UserName).ToUpper() == "SYS"){
            //    if (!dcs.ConnectionString.ToLower().Contains("dba privilege=")){
            //        dcs.ConnectionString += "; DBA Privilege=SYSDBA;";
            //    }
            //}
        }


		protected internal override void clearPool(IDbConnection conn) {
            //try {
            //    OracleConnection.ClearPool((OracleConnection)conn);
            //} catch (Exception ex) {
            //    Debug.WriteLine("Could not clear oracle connection pool: " + ex.Message);
            //    throw;
            //}
		}

		protected internal override IDbCommand createCommand(string text) {
			return new OracleCommand(text);
		}

		protected internal override IDbDataParameter createParam(IDbCommand cmd, string name, object val, DbType dbType) {
			string newName = name.Replace('@', ':').Replace('?', ':');
			cmd.CommandText = cmd.CommandText.Replace(name, newName);

			var prm = new OracleParameter();
            prm.ParameterName = name;

            switch(dbType){
                case DbType.DateTime2:
                case DbType.DateTime:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.DateTime;
                    } else {
                        prm.Value = new OracleDateTime((DateTime)val);
                    }
                    break;
                case DbType.AnsiString:
                case DbType.String:
                    if (val == null || val == DBNull.Value) {
                        prm.Value = new OracleString(null);
                    } else {
                        prm.Value = new OracleString(val + String.Empty);
                    }
                    break;
                case DbType.StringFixedLength:
                case DbType.AnsiStringFixedLength:
                    if (val == null || val == DBNull.Value) {
                        prm.Value = new OracleString(null);
                    } else {
                        prm.Value = new OracleString(val + String.Empty);
                    }
                    break;
                case DbType.Decimal:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.Number;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToDecimal(val, 0));
                    }
                    break;
                case DbType.Double:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.Double;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToDouble(val, 0.0f));
                    }
                    break;
                case DbType.Int16:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.Int16;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToInt16(val, 0));
                    }
                    break;
                case DbType.Int32:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.Int32;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToInt32(val, 0));
                    }
                    break;
                case DbType.Int64:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.Number;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToInt64(val, 0));
                    }
                    break;
                case DbType.Single:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.Float;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToFloat(val, 0.0f));
                    }
                    break;
                case DbType.UInt16:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.UInt16;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToUInt16(val, 0));
                    }
                    break;
                case DbType.UInt32:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                        prm.OracleType = OracleType.UInt32;
                    } else {
                        prm.Value = new OracleNumber(Toolkit.ToUInt32(val, 0));
                    }
                    break;
                default:
                    if (val == DBNull.Value) {
                        prm.Value = DBNull.Value;
                    } else {
                        prm.Value = val;
                    }
                    break;
            }

            return prm;
        }

		protected internal override DbDataAdapter createAdapter(IDbCommand cmd) {
			return new OracleDataAdapter((OracleCommand)cmd);
		}

		protected internal override IDbConnection createConn(string connString) {
			return new OracleConnection(connString);
		}

        //protected internal override void applyLimit(IDbCommand cmd, int limit, int offset) {
        //    string origSql = cmd.CommandText;
        //    string lowerSql = origSql.ToLower();

        //    string output = null;

        //    // oracle uses "where rownum > {offset} and rownum < {limit}" to do limits, so we need to inject the where...

        //    int iFromPos = -1;
        //    int iStartPos = -1;
            
        //    int iEndPos = lowerSql.Length;
        //    if (lowerSql.Contains("from")) {
        //        iFromPos = lowerSql.LastIndexOf("from");
        //    }

        //    if (lowerSql.Contains("where")) {
        //        var iWherePos= lowerSql.LastIndexOf("where") + 5;
        //        if (iWherePos > iFromPos) {
        //            // make sure we grab the where associated with the same from clause
        //            iStartPos = iWherePos;
        //        }
        //    }
        //    if (lowerSql.Contains("order by")) {
        //        iEndPos = lowerSql.LastIndexOf("order by");
        //    }
        //    if (lowerSql.Contains("group by")) {
        //        iEndPos = lowerSql.LastIndexOf("group by");
        //    }

        //    // they may have multiple OR, AND, etc statements in where clause.
        //    // we wrap everything in () so we don't accidentally muck up operator precedence.
        //    StringBuilder sb = new StringBuilder();
        //    if (iStartPos == -1) {
        //        // no where clause. inject one.
        //        sb.Append(Toolkit.Cut(origSql, 0, iEndPos))
        //            .Append(" WHERE ");
        //    } else {
        //        // where clause. insert everything before it
        //        sb.Append(Toolkit.Cut(origSql, 0, iStartPos));
        //    }

        //    if (offset > 0) {
        //        sb.Append(" ROWNUM > ").Append(offset);
        //    }
        //    if (limit > 0) {
        //        if (offset > 0) {
        //            sb.Append(" AND ");
        //        }
        //        sb.Append(" ROWNUM < ").Append(limit + 1).Append(" ");
        //    }

        //    if (iStartPos == -1) {
        //        sb.Append(Toolkit.Cut(origSql, iEndPos));
        //    } else {
        //        // where clause exists, tack the original where clause on the end
        //        sb.Append(" AND (")
        //            .Append(Toolkit.Cut(origSql, iStartPos, iEndPos - iStartPos))
        //            .Append(") ")
        //            .Append(Toolkit.Cut(origSql, iEndPos));

        //    }

        //    output = sb.ToString();

        //    cmd.CommandText = output;


        //}


        // match:
        // select           --  only at beginning of sql statement
        // select distinct  --  only at beginning of sql statement
        // union select     --  anywhere in sql statement
        // union select     --  distinct anywhere in sql statement
        private static Regex __SELECT = new Regex(@"(^[\r\n\t ]*select[\r\n\t ]+(distinct|all)?|union[\r\n\t ]+select[\r\n\t ]+(distinct|all)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex __TOP_TOP = new Regex(@"(top \(__LIMIT__\) top)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected internal override void applyLimit(IDbCommand cmd, int limit, int offset) {
            if (limit <= 0){
                // nothing to do, no limit applied
            } else {
                if (offset <= 0) {
                    // no offset, just grab first X records
                    
                    cmd.CommandText = String.Format(@"
select * from ({0}) WHERE ROWNUM < {1}
", cmd.CommandText, limit + 1);

                } else {
                    
                    // both offset and limit defined

                    cmd.CommandText = String.Format(@"
select * from ({0}) WHERE ROWNUM < {1} and ROWNUM > {2}
", cmd.CommandText, limit + 1, offset);

                }
            }
        }

		public override string Concatenate(string[] values) {
			StringBuilder sb = new StringBuilder();
            sb.Append("CONCAT(");
            foreach (string v in values) {
                sb.Append("NVL(")
                    .Append(v)
                    .Append(",''), ");
            }
            if (sb.Length > 2) {
                sb.Remove(sb.Length - 2, 2);
                sb.Append(")");
            }
            return sb.ToString();
		}

		protected internal override void mungeParameter(IDbCommand cmd, IDbDataParameter prm) {
			// do nothing yet
		}

		protected internal override void mungeSql(IDbCommand cmd) {

            if (String.IsNullOrEmpty(cmd.CommandText)) {
                return;
            }

            string lc = cmd.CommandText.ToLower();
            if (!lc.Contains("concat") && !lc.Contains("left") && !lc.Contains("convert") && !lc.Contains("len")) {
//            if (!lc.Contains("concat") && !lc.Contains("left") && !lc.Contains("convert") && !lc.Contains("len") && !lc.Contains("coalesce")) {
                // no need to inspect further, don't need to munge CONCAT() / LEFT() / CONVERT()
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

            Stack<int> convertMatchEnds = new Stack<int>();
            Stack<int> convertParens = new Stack<int>();

            Stack<int> leftMatchEnds = new Stack<int>();
            Stack<int> leftParens = new Stack<int>();

            Stack<int> lenMatchEnds = new Stack<int>();
            Stack<int> lenParens = new Stack<int>();

            //Stack<int> coalesceMatchEnds = new Stack<int>();
            //Stack<int> coalesceParens = new Stack<int>();
            //List<string> nvlsQueued = new List<string>();
            int parenDepth = 0;

            StringBuilder sbConvertValue = new StringBuilder();
            string convertType = null;

            for (int i = 0; i < parsed.Count; i++) {

                string s = parsed[i];
                string lower = s.ToLower();

                if (s == "(") {
                    parenDepth++;
                }

                if (lower.StartsWith("'") || lower.StartsWith(@"""")) {
                    // quoted value, ignore / add to sb
                    sb.Append(s);
                } else if (lower == "concat") {
                    // convert CONCAT('hi', 'there') into 'hi' || 'there'
                    // skip until parsed[i] = '(' (only happens if whitespace is between concat and (, such as "concat   ("
                    int parenPos = i;
                    while (parenPos < parsed.Count && parsed[parenPos] != "(") {
                        parenPos++;
                    }
                    if (parenPos < parsed.Count) {
                        int concat = findMatchingParen(parsed, parenPos);
                        if (i < concat) {
                            concatMatchEnds.Push(concat);
                            concatParens.Push(parenDepth + 1);
                        }
                    }
                } else if (lower == "left" && i < parsed.Count - 1 && parsed[i + 1] == "(") {
                    // convert left('asdf', 37) into substr('asdf', 0, 37)
                    int left = findMatchingParen(parsed, i + 1);
                    if (i < left) {
                        leftMatchEnds.Push(left);
                        leftParens.Push(parenDepth + 1);
                        sb.Append("substring");
                    }
                } else if (lower == "convert") {
                    int convert = findMatchingParen(parsed, i + 1);
                    if (i < convert) {
                        convertMatchEnds.Push(convert);
                        convertParens.Push(parenDepth + 1);
                        sb.Append("cast");
                    }
                } else if (lower == "len") {
                    int lenPos = findMatchingParen(parsed, i + 1);
                    if (i < lenPos) {
                        lenMatchEnds.Push(lenPos);
                        lenParens.Push(parenDepth + 1);
                        sb.Append("length");
                    }
                //} else if (lower == "coalesce") {
                //    int coalescePos = findMatchingParen(parsed, i + 1);
                //    if (i < coalescePos) {
                //        coalesceMatchEnds.Push(coalescePos);
                //        coalesceParens.Push(parenDepth + 1);
                //        sb.Append("NVL");
                //    }
                } else {

                    bool next = false;
                    if (concatMatchEnds.Count > 0) {
                        int concatMatch = concatMatchEnds.Peek();
                        int concatDepth = concatParens.Peek();
                        if (i < concatMatch && parenDepth == concatDepth) {
                            // we're inside a concat, but not too deep to be in another sub function.
                            if (parsed[i] == ",") {
                                sb.Append(" || ");
                            } else {
                                sb.Append(s);
                            }
                            next = true;
                        } else if (i == concatMatch) {
                            concatMatchEnds.Pop();
                            concatParens.Pop();
                            sb.Append(")");
                            next = true;
                        }
                    }
                    if (!next) {
                        if (leftMatchEnds.Count > 0) {
                            int leftMatch = leftMatchEnds.Peek();
                            int leftDepth = leftParens.Peek();
                            if (i < leftMatch && parenDepth == leftDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(", 1, ");
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == leftMatch) {
                                leftMatchEnds.Pop();
                                leftParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (convertMatchEnds.Count > 0) {
                            int convertMatch = convertMatchEnds.Peek();
                            int convertDepth = convertParens.Peek();
                            // convert(type, value) => cast(value as type)
                            if (i < convertMatch && parenDepth == convertDepth) {
                                if (parsed[i] != "(" && parsed[i] != ")") {
                                    if (convertType == null && parsed[i].Trim().Length > 0) {
                                        // we're on the type
                                        convertType = parsed[i];
                                        switch (convertType.ToLower()) {
                                            case "datetime":
                                            case "datetime2":
                                                convertType = "timestamp";
                                                break;
                                        }
                                    } else if (parsed[i] == ",") {
                                        // we're on the separator
                                    } else {
                                        // must be on the value
                                        sbConvertValue.Append(parsed[i]);
                                    }
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == convertMatch) {
                                convertMatchEnds.Pop();
                                convertParens.Pop();
                                sb.Append(sbConvertValue.ToString()).Append(" as ").Append(convertType);
                                convertType = null;
                                sbConvertValue = new StringBuilder();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    if (!next) {
                        if (lenMatchEnds.Count > 0) {
                            int lenMatch = lenMatchEnds.Peek();
                            int lenDepth = lenParens.Peek();
                            if (i < lenMatch && parenDepth == lenDepth) {
                                if (parsed[i] == ",") {
                                    sb.Append(", 1, ");
                                } else {
                                    sb.Append(s);
                                }
                                next = true;
                            } else if (i == lenMatch) {
                                lenMatchEnds.Pop();
                                lenParens.Pop();
                                sb.Append(")");
                                next = true;
                            }
                        }
                    }

                    //if (!next) {
                    //    if (coalesceMatchEnds.Count > 0) {
                    //        int coalesceMatch = coalesceMatchEnds.Peek();
                    //        int coalesceDepth = coalesceParens.Peek();
                    //        if (i < coalesceMatch && parenDepth == coalesceDepth) {
                    //            if (parsed[i] == ",") {
                    //                // nothing to do...
                    //            } else {
                    //                nvlsQueued.Add(s);
                    //                //sb.Append(s);
                    //            }
                    //            next = true;
                    //        } else if (i == coalesceMatch) {
                    //            coalesceMatchEnds.Pop();
                    //            coalesceParens.Pop();
                    //            var nvls = 0;
                    //            for (var k = 0; k < nvlsQueued.Count; k++) {
                    //                if (k > 0 && k < nvlsQueued.Count-1){
                    //                    sb.Append(", NVL(").Append(nvlsQueued[k]);
                    //                    nvls++;
                    //                } else {
                    //                    sb.Append(nvlsQueued[k]);
                    //                }
                    //            }
                    //            for(var k=0;k<nvls;k++){
                    //                sb.Append(")");
                    //            }
                    //            next = true;
                    //        }
                    //    }
                    //}

                    if (!next) {
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

		protected internal override void enableReturnId(IDbCommand cmd, string nameOfSequenceOrPrimaryKey, ref DataParameters dps) {
			if (cmd.CommandType == CommandType.Text) {
				if (dps == null){
					dps = new DataParameters();
				}
				DataParameter dp = dps.Find(new Predicate<DataParameter>(delegate(DataParameter test) {
					return test.Key == ":ggcreturnid";
				}));
				if (dp == null){
					dp = new DataParameter(":ggcreturnid", null, DbType.Int32, ParameterDirection.Output);
					dps.Add(dp);
				} else {
					dp.Value = null;
				}

				if (!cmd.CommandText.Contains(" returning " + nameOfSequenceOrPrimaryKey + " into :ggcreturnid")) {
					cmd.CommandText += " returning " + nameOfSequenceOrPrimaryKey + " into :ggcreturnid";
				}

			}
		}

        public override string TestLogin() {
            try {
                ReadValue("select 1 from DUAL").ToString();
                return null;
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return ex.Message;
            }
        }

		protected internal override object executeInsert(IDbCommand cmd) {
			cmd.ExecuteNonQuery();
			object ret = ((IDataParameter)cmd.Parameters[":ggcreturnid"]).Value;
			return ret;
		}

        //protected override void initConnectionString(DataConnectionSpec dcs, string serverName, string databaseName) {
        //    dcs.ProviderName = "oracle";
        //    var dbPriv = "";
        //    //if (("" + userName).ToUpper() == "SYS" || ("" + userName).ToUpper() == "SYSTEM") {
        //    //    dbPriv = "DBA Privilege=SYSDBA;";
        //    //}
        //    //dcs.ConnectionString = String.Format(@"Data Source=XE;User Id='{2}';Password='{3}';{4}", ServerName, Port, ("" + userName).Replace("'", "''"), ("" + password).Replace("'", "''"), dbPriv);

        //    var split = serverName.Split(':');
        //    var justServer = split[0];
        //    var port = "1521";
        //    if (split.Length > 0) {
        //        port = split[1];
        //    }

        //    dcs.ConnectionString = String.Format(@"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id='{2}';Password='{3}';{4}", justServer, port, ("" + dcs.UserName).Replace("'", "''"), ("" + dcs.Password).Replace("'", "''"), dbPriv);
        //}
    }
}
