using System.Data;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

using System.IO;
using System.Web.UI;
using System.Web;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;

namespace GrinGlobal.Core {
	public static class Extensions {

		/// <summary>
		/// Writes csv-based output to the given file. First row is column names.
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="fileName"></param>
		public static void WriteCSV(this DataTable dt, string fileName) {

			using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))) {

				int cols = dt.Columns.Count;

				// output column names on first line
				for (int i=0; i < cols; i++) {
					DataColumn dc = dt.Columns[i];
					sw.Write("\"" + dc.ColumnName + "\"");
					if (i < cols - 1) {
						sw.Write(",");
					} else {
						sw.WriteLine();
					}
				}

				// output data on all subsequent lines
				for (int j=0; j < dt.Rows.Count; j++) {
					DataRow dr = dt.Rows[j];
					for (int k=0; k < cols; k++) {
						string data = "\"" + dr[k].ToString().Replace("\"", "\"\"") + "\"";
						sw.Write(data);
						if (k < cols - 1) {
							sw.Write(",");
						} else {
							sw.WriteLine();
						}
					}
				}
			}
		}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rdr"></param>
        ///// <param name="mustBeCrLf"></param>
        ///// <returns></returns>
        //public static string ReadLine(this StreamReader rdr, bool lineTerminatorMustBeCRLF) {
        //    // default StreamReader.Readline() method accepts \r, \n, or \r\n as a line terminator.
        //    // sometimes we need to enforce \r\n only...

        //    if (!lineTerminatorMustBeCRLF) {
        //        return rdr.ReadLine();
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    bool foundBackslash = false;
        //    bool foundCR = false;
        //    while (!rdr.EndOfStream) {
        //        char ch = (char)rdr.Read();
        //        if (foundCR){
        //            if ((ch == '\n')) {
        //                // we're done. note we don't append CR or LF.
        //                break;
        //            } else {
        //                // CR was found, LF was not.
        //                // append CR to output (since we didn't earlier in case we found LF)
        //                sb.Append('\r');
        //            }
        //        }

        //        foundBackslash = (ch == '\\');
        //        if (foundBackslash) {
        //            // escape the next char
        //            sb.Append('\\').Append((char)rdr.Read());
        //            continue;
        //        }

        //        foundCR = (ch == '\r');
        //        if (!foundCR) {
        //            sb.Append(ch);
        //        }
        //    }
        //    string ret = sb.ToString();
        //    return ret;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rdr"></param>
        /// <param name="mustBeCrLf"></param>
        /// <returns></returns>
        public static string ReadLine(this TextReader rdr, bool lineTerminatorMustBeCRLF) {
            // default StreamReader.Readline() method accepts \r, \n, or \r\n as a line terminator.
            // sometimes we need to enforce \r\n only...

            if (!lineTerminatorMustBeCRLF) {
                return rdr.ReadLine();
            }

            StringBuilder sb = new StringBuilder();
            bool foundBackslash = false;
            bool foundCR = false;
            while (rdr.Peek() > -1) {
                char ch = (char)rdr.Read();
                if (foundCR) {
                    if ((ch == '\n')) {
                        // we're done. note we don't append CR or LF.
                        break;
                    } else {
                        // CR was found, LF was not.
                        // append CR to output (since we didn't earlier in case we found LF)
                        sb.Append('\r');
                    }
                }

                foundBackslash = (ch == '\\');
                if (foundBackslash) {
                    // escape the next char
                    sb.Append('\\').Append((char)rdr.Read());
                    continue;
                }

                foundCR = (ch == '\r');
                if (!foundCR) {
                    sb.Append(ch);
                }
            }
            string ret = sb.ToString();
            return ret;

        }


		/// <summary>
		/// Makes a string like "THIS IS an exaMPLE" return as a new string like "This Is An Example" using the current thread's culture info
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string ToTitleCase(this string sToTitlize) {
			return sToTitlize.ToTitleCase(true);
		}

		/// <summary>
		/// Makes a string like "THIS IS an exaMPLE" return as a new string like "This Is An Example". If useThreadCultureInfo is false, the 'en-US' culture is used.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="useThreadCultureInfo"></param>
		/// <returns></returns>
		public static string ToTitleCase(this string sToTitlize, bool useThreadCultureInfo) {
			if (useThreadCultureInfo){
				return sToTitlize.ToTitleCase(Thread.CurrentThread.CurrentCulture);
			} else {
				return sToTitlize.ToTitleCase(new CultureInfo("en-US", false));
			}
		}

		/// <summary>
		/// Makes a string like "THIS IS an exaMPLE" return as a new string like "This Is An Example" using the given CultureInfo.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="ci"></param>
		/// <returns></returns>
		public static string ToTitleCase(this string sToTitlize, CultureInfo ci) {
			if (String.IsNullOrEmpty(sToTitlize)) {
				return sToTitlize;
			} else {
				return ci.TextInfo.ToTitleCase(sToTitlize.ToLower());
			}
		}

		/// <summary>
		/// Essentially a synonym for String.Join(separator, source)
		/// </summary>
		/// <param name="separator"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string Join(this string separator, string[] source) {
			return separator.Join(source, null, null);
		}

		/// <summary>
		/// Returns the given array separated by separator and adds prepend and append to ends of final string. Think {"hi","bye"} => 'hi', 'bye'.
		/// </summary>
		/// <param name="separator"></param>
		/// <param name="source"></param>
		/// <param name="prepend"></param>
		/// <param name="append"></param>
		/// <returns></returns>
		public static string Join(this string separator, string[] source, string prepend, string append) {
			return prepend + String.Join(separator, source) + append;
		}

		public static string ToString(this List<object> items) {
			StringBuilder sb = new StringBuilder();
			foreach (object t in items) {
				sb.Append(t.ToString());
			}
			return sb.ToString();
		}

		/// <summary>
		/// Returns true if any of given delimiters exist in the current string
		/// </summary>
		/// <param name="source"></param>
		/// <param name="delimiters"></param>
		/// <returns></returns>
		public static bool ContainsAny(this string source, string[] delimiters) {
			foreach (string d in delimiters) {
				if (source.Contains(d)) {
					return true;
				}
			}
			return false;
		}

		public static bool ContainsAny(this string source, List<string> delimiters) {
			foreach (string d in delimiters) {
				if (source.Contains(d)) {
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Assumes given delimiter can be escaped using a backslash.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static string[] Split(this string source, char delimiter, bool removeEmptyEntries) {
			return source.Split(delimiter.ToString(), removeEmptyEntries);
		}

		/// <summary>
		/// Assumes given delimiter can be escaped using a backslash.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static string[] Split(this string source, string delimiter, bool removeEmptyEntries) {
			StringSplitOptions sso = StringSplitOptions.None;
			if (removeEmptyEntries) {
				sso = StringSplitOptions.RemoveEmptyEntries;
			}
			if (!source.Contains(@"\" + delimiter)) {
				return source.Split(new string[] { delimiter }, sso);
			} else {
				string[] arr = source.Split(new string[] { delimiter }, sso);
				List<string> ret = new List<string>();
				for(int i=0;i<arr.Length;i++){
					string a = arr[i];
					if (a.EndsWith(@"\")) {
						// this should not have been split on.
						// Join them back together (if there's another entry, append it and skip it 
						// on the next iteration
						a = a + delimiter + (i < arr.Length - 1 ? arr[++i] : "");
					}
					ret.Add(a);
				}
				return ret.ToArray();
			}
		}

		public static string[] Split(this string source, string[] delimiters, bool removeEmptyEntries) {
			
			if (source.Length == 0) {
				return new string[0];
			}

			StringSplitOptions sso = StringSplitOptions.None;
			if (removeEmptyEntries) {
				sso = StringSplitOptions.RemoveEmptyEntries;
			}
			List<string> output = new List<string>();
			bool escaped = false;
			foreach (string delimiter in delimiters) {
				if (source.Contains(@"\" + delimiter)){
					escaped = true;
					break;
				}
			}

			if (!escaped) {
				// we can do normal processing, they're not escaping anything
				return source.Split(delimiters, sso);
			} else {
				// split on the escape char
				List<string> ret = new List<string>();
				string[] escapes = source.Split(@"\", true);
				foreach (string esc in escapes) {
					ret.AddRange(esc.Split(delimiters, true));
				}
				return ret.ToArray();
			}
		}

        public static string[] Split(this string source, char[] delimiters, bool removeEmptyEntries) {

            if (source.Length == 0) {
                return new string[0];
            }

            StringSplitOptions sso = StringSplitOptions.None;
            if (removeEmptyEntries) {
                sso = StringSplitOptions.RemoveEmptyEntries;
            }
            List<string> output = new List<string>();
            bool escaped = false;
            foreach (char delimiter in delimiters) {
                if (source.Contains(@"\" + delimiter)) {
                    escaped = true;
                    break;
                }
            }

            if (!escaped) {
                // we can do normal processing, they're not escaping anything
                return source.Split(delimiters, sso);
            } else {
                // split on the escape char
                List<string> ret = new List<string>();
                string[] escapes = source.Split(@"\", true);
                foreach (string esc in escapes) {
                    ret.AddRange(esc.Split(delimiters, true));
                }
                return ret.ToArray();
            }
        }


        /// <summary>
        /// Returns a list of items split by quotes.  so:  fred "likes" to and he "doesn't \"care\"" 'why they''re here'   will return (pipe signifies new item, all other text within {} is literal): {fred |"likes"| to and he |"doesn't "care""| |'why they''re here'}
        /// </summary>
        /// <param name="text"></param>
        /// <param name="backslashIsEscape"></param>
        /// <param name="doubleIsEscape"></param>
        /// <returns></returns>
        public static List<string> SplitOnQuotes(this string text, bool backslashIsEscapeChar, bool doubleCharEscapes, bool interpretUnescapedAsEscaped) {

            List<string> values = new List<string>();
            if (String.IsNullOrEmpty(text)) {
                return values;
            } else if (!text.Contains(@"""") && !text.Contains("'")) {
                values.Add(text);
            } else {

                char prev = '\0';
                char delim = '\0';
                bool inQuotes = false;
                int quoteStartPos = int.MaxValue;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < text.Length; i++) {
                    char cur = text[i];
                    if (delim == '\0' && (cur == '"' || cur == '\'')) {
                        // start of a quoted string.
                        delim = cur;
                        quoteStartPos = i;
                    }

                    if (cur != delim) {
                        // we know it's a normal character (not ' or ")
                        sb.Append(cur);

                    } else {

                        // this is a ' or ".  if it's escaped, just append it to our current output.
                        // otherwise we need to monkey with it a bit

                        bool isEscaped = backslashIsEscapeChar && prev == '\\' && cur == delim;

                        if (!isEscaped && doubleCharEscapes && cur == delim){
                            // see if next char is same as current one
                            if (i < text.Length-1){
                                isEscaped = cur == text[i + 1] && inQuotes;
                                if (!isEscaped && i > quoteStartPos + 1) {
                                    isEscaped = prev == delim;
                                }
                            }
                        }

                        if (isEscaped){
                            // this is an escaped ' or ". add it to the current list item's output.
                            sb.Append(cur);

                        } else {

                            if (!inQuotes) {
                                // start of quoted string. add what we have so far, flag us as being in quotes
                                values.Add(sb.ToString());
                                sb = new StringBuilder();
                                sb.Append(cur);
                                inQuotes = true;

                            } else {
                                // end of quoted string. add what we have, flag as being out of quotes
                                sb.Append(cur);
                                values.Add(sb.ToString());
                                sb = new StringBuilder();
                                delim = '\0';
                                inQuotes = false;
                                quoteStartPos = int.MaxValue;
                            }
                        }
                    }
                    prev = cur;
                }
                if (sb.Length > 0){
                    if (inQuotes && interpretUnescapedAsEscaped) {
                        // special case: opened quote, never closed it. (think the following:   don't  )
                        values[values.Count - 1] = values[values.Count - 1] + sb.ToString();
                    } else {
                        values.Add(sb.ToString());
                    }
                }
            }
            return values;
        }


        public static bool IsQuoted(this string source) {

            if (String.IsNullOrEmpty(source)){
                return false;
            } else {
                return (source.StartsWith("'") && source.EndsWith("'"))
                || (source.StartsWith(@"""") && source.EndsWith(@""""));
            }

        }

        /// <summary>
        /// Splits string on the given delimiters, optionally retaining the delimiter as an entry instead of throwing it out.  Optionally can exclude Quoted sections (meaning delimiters are ignored within quoted substrings).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiters"></param>
        /// <param name="excludeQuotedSections"></param>
        /// <returns></returns>
        public static List<string> SplitRetain(this string source, char[] delimiters, bool excludeQuotedSections, bool retainDelimiter, bool interpretUnescapedQuoteAsValid) {
            List<string> todo = null;
            if (excludeQuotedSections) {
                todo = source.SplitOnQuotes(true, true, interpretUnescapedQuoteAsValid);
            } else {
                todo = new List<string>();
                todo.Add(source);
            }

            List<string> ret = new List<string>();

            for (int i = 0; i < todo.Count; i++) {
                if (excludeQuotedSections && todo[i].StartsWith("'") || todo[i].StartsWith(@"""")) {
                    // quoted section, ignore any embedded delimiters
                    ret.Add(todo[i]);
                } else {
                    StringBuilder sb = new StringBuilder();
                    for(int j=0;j<todo[i].Length;j++){
                        char c = todo[i][j];
                        sb.Append(c);
                        for (int k=0;k<delimiters.Length;k++){
                            if (c == delimiters[k]) {
                                // need to split here (knock off the last char, as it was a delimiter)
                                sb.Length--;
                                if (sb.Length > 0) {
                                    ret.Add(sb.ToString());
                                }
                                if (retainDelimiter) {
                                    ret.Add(c.ToString());
                                }
                                sb = new StringBuilder();
                                break;
                            }
                        }
                    }
                    if (sb.Length > 0) {
                        ret.Add(sb.ToString());
                    }
                }
            }


            return ret;

        }




		/// <summary>
		/// Removes count chars from the end of the StringBuilder.  If count exceeds the length of the StringBuilder, the StringBuilder is simply emptied to contain a zero-length string (as if count = StringBuilder.Length).
		/// </summary>
		/// <param name="src"></param>
		/// <param name="charsToRemove"></param>
		/// <returns></returns>
		public static StringBuilder Truncate(this StringBuilder src, int count) {
			if (src.Length <= count) {
				count = src.Length;
			}
			return src.Remove(src.Length - count, count);
		}

		public static string ToString(this Exception ex, bool emitDataItems) {
			if (!emitDataItems) {
				return ex.ToString();
			}

			StringBuilder sb = new StringBuilder(ex.ToString());
			sb.Append("\r\n ;; \r\nData => \r\n");
			if (ex.Data != null) {
				foreach (string key in ex.Data.Keys) {
					sb.Append(key)
						.Append("=")
						.Append(ex.Data[key])
						.Append("; \r\n");
				}
			}
			string ret = sb.ToString();
			return ret;
		}

		public static byte[] Slice(this byte[] src, int startIndex) {
			return src.Slice(startIndex, src.Length - startIndex);
		}

		public static byte[] Slice(this byte[] src, int startIndex, int length) {
			byte[] dest = new byte[length];
			Array.Copy(src, startIndex, dest, 0, length);
			return dest;
		}

        /// <summary>
        /// Returns the name of the first column in the PrimaryKey array.  Returns String.Empty if PrimaryKey is empty or null.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string PrimaryKeyName(this DataTable dt) {
            if (dt.PrimaryKey != null && dt.PrimaryKey.Length > 0) {
                return dt.PrimaryKey[0].ColumnName;
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns an array of all the column names in the PrimaryKey array.  Returns empty array if PrimaryKey is empty or null.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string[] PrimaryKeyNames(this DataTable dt) {
            List<string> names = new List<string>();
            if (dt.PrimaryKey != null) {
                foreach (DataColumn dc in dt.PrimaryKey) {
                    names.Add(dc.ColumnName);
                }
            }
            return names.ToArray();
        }


        private static void appendValue(DataColumn dc, StringBuilder tgt, string value) {

            if (dc.DataType == typeof(bool)) {
                tgt.Append(Toolkit.ToBoolean(value, false) ? "true" : "false").Append(",");
            } else if (dc.DataType == typeof(string)) {
                // string or boolean
                if (dc.ColumnName.StartsWith("is")) {
                    // force to boolean boolean
                    if (String.IsNullOrEmpty(value) || value != "Y") {
                        tgt.Append("false").Append(",");
                    } else {
                        tgt.Append("true").Append(",");
                    }
                } else {
                    // string
                    tgt.Append("'").Append(sanitizeForJson(value)).Append("',");
                }
            } else if (dc.DataType == typeof(DateTime)) {
                // date value
                tgt.Append("'").Append(value).Append("',");
            } else {
                // assume integer or float
                tgt.Append(value).Append(",");
            }

        }

        public static string ToJson(this DataSet ds) {

            StringBuilder tables = new StringBuilder();
            foreach (DataTable dt in ds.Tables) {
                tables.Append("'")
                    .Append(dt.TableName)
                    .Append("' : ")
                    .Append(dt.ToJson())
                    .Append(",");
            }
            if (tables.Length > 0) {
                tables.Length--;
            }

            string ret = String.Format(@"
{{
  {0}
}}
", tables);

            return ret;

        }

        private static string sanitizeForJson(string data) {
            return data.Replace("'", "\\'");
        }

        public static string ToJson(this DataTable dt, string primaryKeyName, string alternateKeyName, ITemplate header, ITemplate item, ITemplate altItem, ITemplate footer) {

            // kicks out json for the datatable.
            // table named 'table0' with columns 'col0' and 'col1' with 3 rows:
            //
            // { "table0" : 
            //       "primaryKeyName" : 'pk_field_name_here',
            //       "alternateKeyName" : 'alt_key_field_name_here',
            //       "columns" :
            //            [
            //              {  
            //                 "columnName"   : "col0",
            //                 "caption"      : "Column 0",
            //          /* extended properties */
            //                 "isPrimaryKey" : true,
            //                 "isHidden"     : true,
            //                 ...
            //                 
            //              },
            //              {  
            //                 "columnName"   : "col1",
            //                 "caption"      : "Column 1",
            //          /* extended properties */
            //                 "isPrimaryKey" : true,
            //                 "isHidden"     : true,
            //                 ...
            //                 
            //              },
            //              { ... }
            //            ],
            //       "rows" :
            //            [
            //              ["r0c0", "r0c1"],
            //              ["r1c0", "r1c1"],
            //              ["r2c0", "r2c1"]
            //            ]
            //    }
            // };

            string pkName = Toolkit.Coalesce(primaryKeyName, dt.PrimaryKeyName(), "") as string;


            StringBuilder cols = new StringBuilder();
            foreach (DataColumn dc in dt.Columns) {
                cols.Append("{");
                cols.Append("'columnName':'").Append(sanitizeForJson(dc.ColumnName)).Append("',");
                cols.Append("'caption':'").Append(sanitizeForJson(dc.Caption)).Append("',");
                cols.Append("'ordinal':").Append(dc.Ordinal.ToString()).Append(",");


                string dataType = "string";
                if (dc.DataType == typeof(DateTime)) {
                    dataType = "date";
                } else if (dc.DataType == typeof(int) || dc.DataType == typeof(decimal) || dc.DataType == typeof(float)) {
                    dataType = "number";
                } else if (dc.ColumnName.ToLower().StartsWith("is_")) {
                    dataType = "bool";
                }

                cols.Append("'dataType':'").Append(dataType).Append("',");

                StringBuilder props = new StringBuilder();
                foreach (string name in dc.ExtendedProperties.Keys) {
                    string jsPropName = name.Replace("_", " ").ToTitleCase().Replace(" ", "");
                    jsPropName = jsPropName.Substring(0, 1).ToLower() + jsPropName.Substring(1);

                    object temp = dc.ExtendedProperties[name];
                    string value = String.Empty;
                    if (temp != null) {
                        value = temp.ToString();
                    }

                    props.Append("'").Append(jsPropName).Append("':");

                    if (name.StartsWith("is_")) {
                        if (value == "Y" || value.ToLower() == "true") {
                            props.Append("true,");
                        } else {
                            props.Append("false,");
                        }
                    } else if (String.IsNullOrEmpty(value)) {
                        props.Append("null,");
                    } else {
                        int? intVal = Toolkit.ToInt32(value, (int?)null);
                        if (intVal != null) {
                            props.Append(intVal.ToString()).Append(",");
                        } else {
                            decimal? decVal = Toolkit.ToDecimal(value, (decimal?)null);
                            if (decVal != null) {
                                props.Append(decVal.ToString()).Append(",");
                            } else {
                                props.Append("'").Append(value.Replace("'", "\\'")).Append("',");
                            }
                        }
                    }

                }
                
                cols.Append(props.ToString());

                if (cols.Length > 0 && cols[cols.Length - 1] == ',') {
                    cols.Length--;
                }

                cols.Append("},");
            }
            if (cols.Length > 0 && cols[cols.Length - 1] == ',') {
                cols.Length--;
            }

            StringBuilder rows = new StringBuilder();
            for(int i=0;i<dt.Rows.Count;i++){
                DataRow dr = dt.Rows[i];
                StringBuilder row = new StringBuilder();
                for (int j = 0; j < dt.Columns.Count; j++) {
                    appendValue(dt.Columns[j], row, dr[j].ToString());
                }
                if (row.Length > 0) {
                    row.Length--;
                }

                string pkValue = "null";
                if (!String.IsNullOrEmpty(pkName)){
                    pkValue = dr[pkName].ToString();
                }

                string outputRow = String.Format("{{ values: [{2}] }},", i.ToString(), pkValue, row.ToString());

                rows.Append(outputRow);
            }

            if (rows.Length > 0 && rows[rows.Length-1] == ',') {
                rows.Length--;
            }



            string ret = String.Format(@"
{{ 
  'primaryKeyName' : '{0}',
  'alternateKeyName' : '{1}',
  'columns' : [{2}],
  'rows'    : [{3}]
}}
", pkName, Toolkit.Coalesce(alternateKeyName, dt.PrimaryKeyName(), ""), cols.ToString(), rows.ToString());


            return ret;


        }

        /// <summary>
        /// Returns all values for the given column
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="uniqueValuesOnly"></param>
        /// <returns></returns>
        public static List<string> ListColumnValues(this DataTable dt, string columnName, bool uniqueValuesOnly) {
            var rv = new List<string>();
            if (uniqueValuesOnly) {
                foreach (DataRow dr in dt.Rows) {
                    if (!rv.Contains(dr[columnName].ToString())) {
                        rv.Add(dr[columnName].ToString());
                    }
                }
            } else {
                foreach (DataRow dr in dt.Rows) {
                    rv.Add(dr[columnName].ToString());
                }
            }
            return rv;
        }

        public static string ToJson(this DataTable dt, string primaryKeyName, string alternateKeyName) {
            return dt.ToJson(primaryKeyName, alternateKeyName, null, null, null, null);
        }

        public static string ToJson(this DataTable dt) {
            return dt.ToJson(null, null, null, null, null, null);
        }

        public static DataTable Transform(this DataTable dt, string[] groupByColumns, string sourceColumnForNames, string sourceColumnForCaptions, string sourceColumnForValues) {

            DataTable ret = new DataTable(dt.TableName);

            // ok, we need to create a new datatable with columns of:
            // 1. those listed in groupByColumns
            foreach (string s in groupByColumns) {
                DataColumn groupByCol = new DataColumn(s, typeof(string));
                DataColumn existing = dt.Columns[s];
                if (existing != null){
                    groupByCol.Caption = existing.Caption;
                }
                ret.Columns.Add(groupByCol);
            }
            // 2. unique values from sourceColumnForNames (use value in same row for captions from sourceColumnForCaptions
            foreach (DataRow dr in dt.Rows) {
                string newColName = dr[sourceColumnForNames].ToString();
                if (!String.IsNullOrEmpty(newColName)) {
                    if (!ret.Columns.Contains(newColName)) {
                        DataColumn newCol = new DataColumn(newColName, typeof(string));
                        if (!String.IsNullOrEmpty(sourceColumnForCaptions)) {
                            string newColCaption = dr[sourceColumnForCaptions].ToString();
                            if (!String.IsNullOrEmpty(newColCaption)) {
                                newCol.Caption = newColCaption;
                            }
                        }
                        ret.Columns.Add(newCol);
                    }
                }
            }


            // then, we need to create a row for each unique value in the groupByColumns and copy over the
            // corresponding value from sourceColumnForValues into appropriate new column name 
            // we could do this with a dt.Select, but it is significantly slower than using the List<string>
            // since we more than likely have multiple columns we're grouping by

            List<string> uniqueGroupByValues = new List<string>();
            foreach (DataRow dr in dt.Rows) {
                StringBuilder sb = new StringBuilder();
                foreach(string s in groupByColumns){
                    sb.Append(dr[s].ToString());
                }

                string groupByVal = sb.ToString();
                DataRow row = null;
                int rowNumber = uniqueGroupByValues.FindIndex(r => {
                    return r == groupByVal;
                });
                if (rowNumber == -1){
                    // need to create and add a new row to the output table
                    row = ret.NewRow();
                    foreach (string s in groupByColumns) {
                        row[s] = dr[s];
                    }
                    ret.Rows.Add(row);
                    // remember the unique values
                    uniqueGroupByValues.Add(groupByVal);
                } else {
                    row = ret.Rows[rowNumber];
                }

                // assign the value to the proper 'new' column
                string newColName = dr[sourceColumnForNames].ToString();
                string newColValue = dr[sourceColumnForValues].ToString();
                if (!String.IsNullOrEmpty(newColName)){
                    //row[newColName] = dr[sourceColumnForValues]; 
                    if (row[newColName].ToString() == "")
                        row[newColName] = dr[sourceColumnForValues];
                    else
                    {
                        if (!(row[newColName].ToString().IndexOf(newColValue) > -1))  // only add when it's not duplicate value
                            row[newColName] += "; " + dr[sourceColumnForValues];
                    }
                }
            }

            return ret;


        }

        public static void AddList<T>(this DataSet ds, List<T> list, string name) {
            var dt = ds.Tables.Add(name);
            dt.Columns.Add("Value", typeof(T));
            foreach (var val in list) {
                dt.Rows.Add(new object[] { val });
            }
        }

        public static string ToJson(this DataTable dt, bool needDetail)
        {
            StringBuilder JsonString = new StringBuilder();

            if (!needDetail)
            {
                JsonString.Append("{ ");
                JsonString.Append("\"table\":[{ ");
                JsonString.Append("\"row\":[ ");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    JsonString.Append("\"col\":[ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("{" + "\"data\":\"" + dt.Rows[i][j].ToString().Replace("\"", "\'") + "\"},");
                            //JsonString.Append("{" + "\"data\":\"" + dt.Rows[i][j] + "\"},");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("{" + "\"data\":\"" + dt.Rows[i][j].ToString().Replace("\"", "\' ") + "\"}");
                            //JsonString.Append("{" + "\"data\":\"" + dt.Rows[i][j].ToString() + "\"}");
                        }
                    }

                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("]} ");
                    }
                    else
                    {
                        JsonString.Append("]}, ");
                    }
                }
                JsonString.Append("]}]}");
                return JsonString.ToString();
            }
            else
                return ToJson(dt);
        }

        public static int GetGrinGlobalErrorNumber(this Exception ex) {
            if (ex.Data != null && ex.Data.Contains("GrinGlobalErrorNumber")) {
                return Toolkit.ToInt32(ex.Data["GrinGlobalErrorNumber"], 0);
            } else {
                return 0;
            }
        }

        public static string GetGrinGlobalErrorMessage(this Exception ex) {
            if (ex.Data != null && ex.Data.Contains("GrinGlobalErrorMessage")) {
                return ex.Data["GrinGlobalErrorMessage"] + string.Empty;
            } else {
                return null;
            }
        }

        public delegate DataTable BindExCallback(DataColumn column, string dataviewName);

        /// <summary>
        /// Binds ExtendedProperties and columns to the gridview as well as the data.
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dt"></param>
        public static void BindEx(this DataGridView dgv, DataTable dt, bool allowSorting, bool removeAutoAppendedFields, bool ignoreReadOnly, BindExCallback smallSelectDataSourceCallback, BindExCallback largeSelectDataSourceCallback) {
            if (dt != null) {

                dgv.AutoGenerateColumns = false;
                dgv.Columns.Clear();

                if (removeAutoAppendedFields){
                    for (var i = 0; i < dt.Columns.Count; i++) {
                        var dc = dt.Columns[i];
                        if (dc.ExtendedProperties.Count == 0 || !dc.ExtendedProperties.ContainsKey("gui_hint")){
                            // the save process may auto-append columns, so remove them here if the caller says to
                            dt.Columns.Remove(dc);
                            i--;
                        }
                    }
                }

                foreach(DataColumn dc in dt.Columns){

                    var nullable = dc.ExtendedProperties.ContainsKey("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString().ToUpper() == "Y";
                    if (dc.ExtendedProperties.ContainsKey("gui_hint")) {

                        var guiHint = dc.ExtendedProperties["gui_hint"].ToString().ToUpper();

                        DataGridViewColumn dgvc = null;

                        switch (guiHint) {
                            case "TOGGLE_CONTROL":
                                var dgvchk = new DataGridViewCheckBoxColumn(false);
                                dgvchk.TrueValue = "Y";
                                dgvchk.FalseValue = "N";
                                dgvc = dgvchk;
                                break;
                            case "LARGE_SINGLE_SELECT_CONTROL":
                                var dgvcbo1 = new DataGridViewComboBoxColumn();
                                dgvcbo1.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                                dgvcbo1.ValueMember = "value_member";
                                dgvcbo1.DisplayMember = "display_member";
                                if (largeSelectDataSourceCallback != null) {
                                    dgvcbo1.DataSource = largeSelectDataSourceCallback(dc, dc.ExtendedProperties["foreign_key_dataview_name"].ToString());
                                }
                                dgvc = dgvcbo1;
                                break;
                            case "SMALL_SINGLE_SELECT_CONTROL":
                                var dgvcbo2 = new DataGridViewComboBoxColumn();
                                dgvcbo2.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                                dgvcbo2.ValueMember = "value_member";
                                dgvcbo2.DisplayMember = "display_member";
                                dgvcbo2.FlatStyle = FlatStyle.Popup;
                                //dgvcbo2.DefaultCellStyle.ForeColor = Color.Black;
                                //dgvcbo2.DefaultCellStyle.BackColor = Color.White;
                                if (smallSelectDataSourceCallback != null) {
                                    var gn = dc.ExtendedProperties["group_name"].ToString();
                                    var dtCbo = smallSelectDataSourceCallback(dc, gn);
                                    if (nullable) {
                                        var empty = dtCbo.NewRow();
                                        empty["value_member"] = DBNull.Value;
                                        empty["display_member"] = "(None)";
                                        dtCbo.Rows.InsertAt(empty, 0);
                                        dtCbo.AcceptChanges();
                                    }
                                    var vals = new List<string>();
                                    foreach (DataRow dr in dtCbo.Rows) {
                                        vals.Add(dr["value_member"].ToString());
                                    }
                                    dgvcbo2.Tag = "Valid codes for " + gn + ":\r\n" + String.Join("\r\n", vals.ToArray());
                                    dgvcbo2.DataSource = dtCbo;
                                }
                                dgvc = dgvcbo2;
                                break;
                            case "DATE_CONTROL":
                                var dgvcdate = new DataGridViewCalendarColumn();
                                dgvc = dgvcdate;
                                break;
                            default:
                                var dgvctxt = new DataGridViewTextBoxColumn();
                                var maxLength = Toolkit.ToInt32(dc.ExtendedProperties["max_length"], 32767);
                                if (maxLength == -1) {
                                    maxLength = 32767;
                                }

                                if (guiHint == "INTEGER_CONTROL") {
                                    maxLength = 10;
                                } else if (guiHint == "DECIMAL_CONTROL") {
                                    maxLength = Toolkit.ToInt32(dc.ExtendedProperties["numeric_precision"], 18) + 1;
                                }

                                dgvctxt.MaxInputLength = maxLength;
                                dgvc = dgvctxt;
                                break;
                        }


                        // TODO: lots of more binding stuff here

                        // NOTE: we explicity do NOT want to set the AllowDBNull value here!
                        //       this makes pasting data in considerably harder and error handling ugly
                        //       we do it manually later.
                        //if (!nullable) {
                        //    dc.AllowDBNull = false;
                        //}

                        dgvc.Name = dc.ColumnName;
                        dgvc.DataPropertyName = dc.ColumnName;
                        dgvc.HeaderText = dc.Caption;
                        dgvc.ToolTipText = dc.ExtendedProperties["description"].ToString();
                        if (ignoreReadOnly) {
                            dc.ReadOnly = false;
                            //dc.ExtendedProperties["is_readonly"] = "N";
                            dgvc.ReadOnly = false;
                        } else {
                            dgvc.ReadOnly = dc.ExtendedProperties["is_readonly"].ToString().ToUpper() == "Y";
                        }
                        dgvc.Visible = dc.ExtendedProperties["is_visible"].ToString().ToUpper() == "Y";
                        dgvc.SortMode = allowSorting ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable;
                        dgv.ShowCellToolTips = true;

                        dgv.Columns.Add(dgvc);
                    }
                }
                dgv.CellFormatting -= new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
                dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
                dgv.DataSource = dt;

            }
        }

        static void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            if (e.RowIndex > -1 && e.ColumnIndex > -1) {
                var dgv = sender as DataGridView;
                var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = dgv.Columns[e.ColumnIndex].Tag as string;
            }
        }

        public static int ImportData(this DataGridView dgv, IEnumerable<List<string>> dataSource) {
            var offsets = new List<int>();

            var dt = dgv.DataSource as DataTable;

            var cols = new List<DataGridViewColumn>();

            var rows = new List<DataRow>();

            var rowNumber = 0;

            var errors = 0;

            dgv.DefaultCellStyle.DataSourceNullValue = DBNull.Value;
            dgv.DefaultCellStyle.NullValue = "[null]";

            //// make sure all combobox columns are non-nullable
            //for (var i = 0; i < dgv.Columns.Count; i++) {
            //    if (dgv.Columns[i] is DataGridViewComboBoxColumn) {
            //        dt.Columns[i].AllowDBNull = false;
            //    }
            //}
            var currow = 0;
            foreach (var values in dataSource) {
                currow++;
                if (currow % 500 == 0) {
                    Application.DoEvents();
                }
                if (offsets.Count == 0) {
                    foreach (var val in values) {
                        var added = false;
                        for (var i = 0; i < dgv.Columns.Count; i++) {
                            var col = dgv.Columns[i];
                            if (String.Compare(col.Name.Trim().Replace(" *", ""), val, true) == 0 || String.Compare(col.HeaderText.Trim().Replace(" *", ""), val, true) == 0) {
                                offsets.Add(i);
                                cols.Add(col);
                                added = true;
                                break;
                            }
                        }
                        if (!added) {
                            offsets.Add(-1);
                            cols.Add(null);
                        }
                    }
                } else {

                    rowNumber++;
                    var totalCharLength = 0;
                    if (dt != null) {
                        var row = dt.NewRow();
                        for (var i = 0; i < values.Count; i++) {
                            if (i >= cols.Count) {
                                // their input file has values past what their columns are defined as.  Do not error, just silently ignore them.
                                Debug.WriteLine("more values than for defined columns at row=" + rowNumber);
                            } else if (cols[i] != null && !cols[i].ReadOnly) {
                                var invalidCodeValue = false;
                                if (cols[i] is DataGridViewCheckBoxColumn) {
                                    var chkCol = ((DataGridViewCheckBoxColumn)cols[i]);
                                    if (String.Compare(chkCol.TrueValue.ToString().Trim(), values[i].Trim(), true) == 0
                                        || String.Compare(values[i].Trim(), "TRUE", true) == 0
                                        || values[i].Trim() == "1") {
                                        // either the true value or the literal text "TRUE" or "1". assume true.
                                        values[i] = chkCol.TrueValue.ToString();
                                    } else {
                                        // not the true value, assume the false value
                                        values[i] = chkCol.FalseValue.ToString();
                                    }
                                } else if (cols[i] is DataGridViewTextBoxColumn) {
                                    var txtCol = cols[i] as DataGridViewTextBoxColumn;
                                    if (values[i].Length > txtCol.MaxInputLength) {
                                        row.SetColumnError(dt.Columns[cols[i].DataPropertyName], "The value '" + values[i] + "' exceeds the maximum length for this column.  Data has been truncated to fit.");
                                        row.RowError = "Error in " + cols[i].HeaderText + ": Value '" + values[i] + "' is exceeds the maximum length.  Data has been truncated to fit.";
                                        values[i] = Toolkit.Cut(values[i], 0, txtCol.MaxInputLength);
                                        errors++;
                                    }
                                    //} else if (cols[i] is DataGridViewCalendarColumn) {
                                    //    var calCol = cols[i] as DataGridViewCalendarColumn;

                                } else if (cols[i] is DataGridViewComboBoxColumn) {
                                    var cboCol = cols[i] as DataGridViewComboBoxColumn;
                                    var dtCol = cboCol.DataSource as DataTable;
                                    var found = false;
                                    foreach (DataRow dr in dtCol.Rows) {
                                        if (string.Compare(dr[cboCol.ValueMember].ToString().Trim(), values[i].Trim(), true) == 0
                                            || string.Compare(dr[cboCol.DisplayMember].ToString().Trim(), values[i].Trim(), true) == 0) {
                                            found = true;
                                            values[i] = dr[cboCol.ValueMember].ToString();
                                            break;
                                        }
                                    }
                                    if (!found) {
                                        invalidCodeValue = true;
                                    }
                                }

                                totalCharLength += values[i].ToString().Trim().Length;
                                if (String.IsNullOrEmpty(values[i])) {
                                    row[cols[i].DataPropertyName] = DBNull.Value;
                                } else {
                                    var tableColumn = row.Table.Columns[cols[i].DataPropertyName];

                                    if (invalidCodeValue) {
                                        row.SetColumnError(tableColumn, "'" + values[i] + "' is not a valid value. Please select one from the drop down.");
                                        row.RowError = "Error in " + cols[i].HeaderText + ": Value '" + values[i] + "' is not a valid value.";
                                        errors++;
                                    } else {
                                        object val = null;
                                        if (tableColumn.DataType == typeof(string)) {
                                            var maxlen = tableColumn.MaxLength;
                                            maxlen = maxlen < 0 ? 32767 : maxlen;
                                            if (values[i].Length > maxlen) {
                                                row.SetColumnError(tableColumn, "The value '" + values[i] + "' exceeds the maximum length for this column.  Data has been truncated to fit.");
                                                row.RowError = "Error in " + cols[i].HeaderText + ": Value '" + values[i] + "' is exceeds the maximum length.  Data has been truncated to fit.";
                                                errors++;
                                            }
                                            val = Toolkit.Cut(values[i], 0, maxlen);
                                        } else {
                                            val = values[i];
                                        }
                                        try {
                                            row[cols[i].DataPropertyName] = val;
                                        } catch (Exception ex) {
                                            row.SetColumnError(tableColumn, ex.Message);
                                            row.RowError = "Error in " + cols[i].HeaderText + ": Value '" + val + "' is invalid.  Detail: " + ex.Message;
                                            errors++;
                                        }

                                    }
                                }
                            }
                        }
                        if (totalCharLength > 0) {
                            rows.Add(row);
                            //for (var i = 0; i < dgv.Columns.Count; i++) {
                            //    dgv.Rows[dgv.Rows.Count - 1].Cells[i].Style.NullValue = "[null]";
                            //}

                        } else {
                            Debug.WriteLine("No data to import, skipping");
                        }

                    } else {
                        // not databound, push directly into datagridview row objects
                        var newRow = dgv.Rows.Add();
                        DataGridViewRow row = dgv.Rows[newRow];
                        for (var i = 0; i < values.Count; i++) {
                            if (offsets[i] > -1) {
                                totalCharLength += values[i].ToString().Trim().Length;
                                row.Cells[offsets[i]].Value = values[i];
                                dgv.Rows[newRow].Cells[i].Style.NullValue = "[null]";
                            }
                        }

                        if (totalCharLength == 0) {
                            Debug.WriteLine("Removing empty row");
                            dgv.Rows.Remove(row);
                        }
                    }
                }
            }



            // if data bound, we queued up the rows.
            if (dt != null) {

                dgv.DataSource = null;
                dgv.Rows.Clear();
                var rowi = 0;
                foreach (var r in rows) {
                    rowi++;
                    if (rowi % 500 == 0) {
                        Application.DoEvents();
                    }
                    // r["__importid__"] = rowi-1;
                    dt.Rows.Add(r);
                }
                dgv.DataSource = dt;

            }
            offsets.Clear();
            rows.Clear();
            cols.Clear();

            return errors;
        }

        public static void StartDragIfNeeded(this DataGridView dgv, DataGridViewCellMouseEventArgs e, bool shiftClickUsesActualNames, Encoding encoding) {
            // begin drag and drop if at least 1 row is selected
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && dgv.Rows[e.RowIndex].Selected && e.Button == MouseButtons.Left) {
                if (dgv.SelectedRows.Count > 0) {
                    // export data from the gridview control
                    var gridviewData = dgv.ExportData(shiftClickUsesActualNames, encoding);

                    // ...and finally start the drag event
                    DataObject dat = new DataObject();
                    dat.SetData(typeof(string), gridviewData);
                    dgv.DoDragDrop(dat, DragDropEffects.Copy);

                }
            }
        }

        public static string ExportData(this DataGridView dgv, bool controlClickUsesActualNames, Encoding encoding){
            using (new AutoCursor(dgv.FindForm())) {
                // grab header text (or actual column names if they hit shift)
                var useActualNames = controlClickUsesActualNames && (System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control;
                var names = new List<string>();
                foreach (DataGridViewColumn dgvc in dgv.Columns) {
                    var nameToUse = (useActualNames ? dgvc.Name : dgvc.HeaderText).Replace(" *", "");
                    names.Add(nameToUse);
                }

                // set up some streams to manipulate data into a tab-delimited string
                using (var ms = new MemoryStream()) {
                    encoding = encoding ?? Encoding.UTF8;
                    using (var sr = new StreamReader(ms, encoding)) {
                        using (var sw = new StreamWriter(ms, encoding)) {

                            // output column info
                            Toolkit.OutputTabbedHeader(names.ToArray(), sw);

                            // output data from selected rows
                            var values = new List<object>();
                            var rows = new List<DataGridViewRow>();
                            foreach (DataGridViewRow dgvr in dgv.SelectedRows) {
                                rows.Add(dgvr);
                            }
                            rows.Reverse();
                            foreach (DataGridViewRow dgvr in rows) {
                                values.Clear();
                                if (!dgvr.IsNewRow) {
                                    foreach (DataGridViewCell cell in dgvr.Cells) {
                                        //if (cell is DataGridViewCheckBoxCell) {
                                        //    values.Add(cell.Value);
                                        //} else {
                                        values.Add(cell.Value);
                                        //}
                                    }
                                    Toolkit.OutputTabbedData(values.ToArray(), sw, false);
                                }
                            }

                            // retrieve contents from the memory stream
                            sw.Flush();
                            sr.BaseStream.Position = 0;
                            var s = sr.ReadToEnd();
                            return s;

                        }
                    }
                }
            }
        }


        private static List<Form> __synchronizedForms;
        /// <summary>
        /// Used to suppress GUI synchronization and event handling when an event is in the process of being handled.  (aka prevent controls from cross-firing events on all event handlers that start with a call to this method.  See GrinGlobal.Import.frmImport.ddlDataType_SelectedIndexChanged and frmImport_Load for an example)
        /// </summary>
        /// <param name="callback"></param>
        public static void Synchronize(this Form f, bool showCursor, Toolkit.VoidCallback callback) {
            if (__synchronizedForms == null) {
                __synchronizedForms = new List<Form>();
            }

            if (!__synchronizedForms.Contains(f)) {
                __synchronizedForms.Add(f);
                try {
                    if (showCursor) {
                        using (new AutoCursor(f)) {
                            callback();
                        }
                    } else {
                        callback();
                    }
                } finally {
                    __synchronizedForms.Remove(f);
                }
            }
        }

        public static object GetValue(this ComboBox cbo, object defaultValue) {

            if (cbo.SelectedIndex > -1) {
                var dt = cbo.DataSource as DataTable;
                if (dt != null) {
                    return dt.Rows[cbo.SelectedIndex][cbo.ValueMember];
                }
            }

            return defaultValue;
        }

        public delegate void BackgroundWorkerCallback(BackgroundWorker worker);
        public delegate void BackgroundWorkerProgress(BackgroundWorker worker, ProgressChangedEventArgs e);
        public delegate void BackgroundWorkerCompleted(BackgroundWorker worker, RunWorkerCompletedEventArgs e);

        //protected BackgroundWorker processInBackground(BackgroundWorkerCallback callback, BackgroundWorkerProgress progress, BackgroundWorkerCompleted completed) {
        //    var bw = new BackgroundWorker();
        //    bw.WorkerReportsProgress = true;
        //    bw.WorkerSupportsCancellation = true;
        //    if (callback != null) {
        //        bw.DoWork += new DoWorkEventHandler((sender, e) => {
        //            callback(bw);
        //        });
        //        if (progress != null) {
        //            bw.ProgressChanged += new ProgressChangedEventHandler((sender, e) => {
        //                progress(bw, e);
        //            });
        //        }
        //        if (completed != null) {
        //            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) => {
        //                completed(bw, e);
        //            });
        //        }
        //        bw.RunWorkerAsync();
        //    }
        //    return bw;
        //}

        /// <summary>
        /// callback: wrk => {}, progresss: (wrk, progress) => {}, completed: (wrk, result) => {}
        /// </summary>
        /// <param name="f"></param>
        /// <param name="callback"></param>
        /// <param name="progress"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public static BackgroundWorker ProcessInBackground(this Form f, BackgroundWorkerCallback callback, BackgroundWorkerProgress progress, BackgroundWorkerCompleted completed) {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            if (callback != null) {
                bw.DoWork += new DoWorkEventHandler((sender, e) => {
                    callback(bw);
                });
                if (progress != null) {
                    bw.ProgressChanged += new ProgressChangedEventHandler((sender, e) => {
                        progress(bw, e);
                    });
                }
                if (completed != null) {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender, e) => {
                        completed(bw, e);
                    });
                }
                bw.RunWorkerAsync();
            }
            return bw;
        }

        public static void EnableColumnSorting(this ListView lv) {
            var lvh = new ListViewHandler();
            lv.ListViewItemSorter = new ListViewColumnSorter();
            lv.ColumnClick += new ColumnClickEventHandler(lvh.OnColumnClick);
            lv.KeyUp += new KeyEventHandler(lvh.OnKeyUp);
        }

        private class ListViewHandler {
            internal void OnKeyUp(object sender, KeyEventArgs e) {
                if ((e.Modifiers & Keys.Control) == Keys.Control && e.KeyCode == Keys.A) {
                    var lv = (ListView)sender;
                    foreach (ListViewItem lvi in lv.Items) {
                        lvi.Selected = true;
                    }
                }
            }
            internal void OnColumnClick(object sender, ColumnClickEventArgs e) {
                var lv = ((ListView)sender);
                var lvcs = lv.ListViewItemSorter as ListViewColumnSorter;

                // Determine if clicked column is already the column that is being sorted.
                if (e.Column == lvcs.SortColumn) {
                    // Reverse the current sort direction for this column.
                    if (lvcs.Order == SortOrder.Ascending) {
                        lvcs.Order = SortOrder.Descending;
                    } else {
                        lvcs.Order = SortOrder.Ascending;
                    }
                } else {
                    // Set the column number that is to be sorted; default to ascending.
                    lvcs.SortColumn = e.Column;
                    lvcs.Order = SortOrder.Ascending;
                }

                lvcs.DetermineComparer(lv);

                // Perform the sort with these new sort options.
                lv.Sort();
            }
        }

        public static string ToScalarValue(this DataTable dt)
        {
            string ret = "";
            if (dt.Rows.Count > 0)
                ret = dt.Rows[0].ItemArray[0].ToString();
            return ret;
        }
    }
}
