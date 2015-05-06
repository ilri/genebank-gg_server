using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

using System.IO;
using System.Management;
using System.Management.Instrumentation;
using System.Data;

using System.Collections.Specialized;

using System.Diagnostics;
using System.Runtime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Xml;

using Microsoft.Win32;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ServiceProcess;
using System.DirectoryServices;
using System.Web;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Channels;
using System.Net.Sockets;

namespace GrinGlobal.Core {
	/// <summary>
	/// Contains common methods used by almost any project
	/// </summary>
	[ComVisible(true)]
//	[DebuggerStepThrough()]
	public class Toolkit {

		#region Commonly used delegates
		/// <summary>
		/// Allows anonymous object-returning methods to be created succinctly
		/// </summary>
		public delegate object ObjectCallback();
		/// <summary>
		/// Allows anonymous boolean-returning methods to be created succinctly
		/// </summary>
		/// <returns></returns>
		public delegate bool BoolCallback();

		public delegate void VoidCallback();

		#endregion

        public static bool ActivateApplication(IntPtr windowHandle) {
            try {
                return SetForegroundWindow(windowHandle);
            } catch {
                return false;
            }
        }

        public static void Swap<T>(List<T> objects, int offsetA, int offsetB) {
            var temp = objects[offsetA];
            objects[offsetA] = objects[offsetB];
            objects[offsetB] = temp;
        }

        public static void Swap(ref object a, ref object b) {
            var temp = a;
            a = b;
            b = a;
        }

		#region String manipulation

        private static List<char> ESCAPE_CHAR_COMPARES = new List<char> {'u', 'x', 'U', '\'', '"', '\\', '0', 'a', 'b', 'f', 'n', 'r', 't', 'v'};
        private static List<char> ESCAPE_CHARS = new List<char> {'\0', '\0', '\0', '\'', '\"', '\\', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v'};
        public static char Unescape(string escapedString) {
            if (String.IsNullOrEmpty(escapedString) || escapedString[0] != '\\' || escapedString.Length < 2) {
                return '\0';
            } else {
                var c2 = escapedString[1];
                var idx = ESCAPE_CHAR_COMPARES.IndexOf(c2);
                if (idx == -1) {
                    return '\0';
                } else if (idx < 3) {
                    // unicode, more to do
                    return Regex.Unescape(escapedString)[0];
                } else {
                    return ESCAPE_CHARS[idx];
                }

                /*
                \' - single quote, needed for character literals
                \" - double quote, needed for string literals
                \\ - backslash
                \0 - Unicode character 0
                \a - Alert (character 7)
                \b - Backspace (character 8)
                \f - Form feed (character 12)
                \n - New line (character 10)
                \r - Carriage return (character 13)
                \t - Horizontal tab (character 9)
                \v - Vertical quote (character 11)
                \uxxxx - Unicode escape sequence for character with hex value xxxx
                \xn[n][n][n] - Unicode escape sequence for character with hex value nnnn (variable length version of \uxxxx)
                \Uxxxxxxxx - Unicode escape sequence for character with hex value xxxxxxxx (for generating surrogates)            }
                */
            }
        }

        /// <summary>
        /// Given string containing a comma- or whitespace-separated series of numbers, parses them into a List of ints.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<int> ToIntList(string s) {
            if (String.IsNullOrEmpty(s)) {
                return new List<int>();
            }

            string[] list = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> ret = new List<int>();
            foreach (string v in list) {
                int? val = Toolkit.ToInt32(v, (int?)null);
                if (val != null){
                    ret.Add((int)val);
                }
            }
            return ret;
        }

        /// <summary>
        /// Given string containing a comma- or whitespace-separated series of numbers, parses them into a List of decimals.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<decimal> ToDecimalList(string s) {
            if (String.IsNullOrEmpty(s)) {
                return new List<decimal>();
            }

            string[] list = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<decimal> ret = new List<decimal>();
            foreach (string v in list) {
                decimal? val = Toolkit.ToDecimal(v, (decimal?)null);
                if (val != null) {
                    ret.Add((decimal)val);
                }
            }
            return ret;
        }

        /// <summary>
        /// Given two versions (in format of 1.234.555.080), returns true if version A is greater than version B
        /// </summary>
        /// <param name="versionA"></param>
        /// <param name="versionB"></param>
        /// <returns></returns>
        public static bool IsVersionAGreater(string versionA, string versionB) {
            if (String.IsNullOrEmpty(versionA)) {
                // no version A specified, assume B is equal to or greater
                return false;
            } else if (String.IsNullOrEmpty(versionB)) {
                // no version B exists, assume version A is greater
                return true;
            } else {
                // both versions exist, compare each portion of version numbers (i.e. X.Y.Z)
                string[] a = versionA.Split(new char[] { '.' });
                string[] b = versionB.Split(new char[] { '.' });

                int i = 0;
                while (i < a.Length && i < b.Length) {
                    if (Toolkit.ToInt32(a[i], 0) > Toolkit.ToInt32(b[i], 0)) {
                        return true;
                    }
                    if (Toolkit.ToInt32(a[i], 0) < Toolkit.ToInt32(b[i], 0)) {
                        return false;
                    }
                    i++;
                    if (i >= b.Length && i < a.Length) {
                        return true;
                    }
                }

                return false;
            }

        }


        /// <summary>
        /// Given string containing a comma- or whitespace-separated series of numbers, parses them into a List of longs.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<long> ToLongList(string s) {
            if (String.IsNullOrEmpty(s)) {
                return new List<long>();
            }

            string[] list = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<long> ret = new List<long>();
            foreach (string v in list) {
                long? val = Toolkit.ToInt64(v, (long?)null);
                if (val != null) {
                    ret.Add((long)val);
                }
            }
            return ret;
        }


        /// <summary>
        /// Assumes given string, s, is in connectionstring-like format (key1=value1;key2=value2;) and removes name/value pair from the string, including the trailing ';' if any.  Handles quoted values properly.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="name"></param>
        /// <param name="defaultIfNotFound"></param>
        /// <returns></returns>
        public static string RemoveNameValuePair(string s, string name) {
            List<string> keyValuePairs = s.SplitRetain(new char[] { ';', '=' }, true, true, false);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keyValuePairs.Count; i++) {
                if (keyValuePairs[i].ToLower() == name.ToLower()) {
                    // this is the name of the key to remove
                    // next entry is the =
                    // entry after that is the (possibly quoted) User Id or Password value.
                    // that's the one we want to ignore, so slurp everything up past it

                    // skip over next 3 (moniker, =, value)
                    i += 2;
                    if (i < keyValuePairs.Count - 1) {
                        if (keyValuePairs[i + 1].Trim() == ";") {
                            i++;
                        }
                    }

                } else {
                    sb.Append(keyValuePairs[i]);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Assumes given string, s, is in connectionstring-like format (key1=value1;key2=value2;) and returns value for given name, defaulting to defaultIfNotFound.  Handles quoted values properly.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="name"></param>
        /// <param name="defaultIfNotFound"></param>
        /// <returns></returns>
        public static string GetValueFromNameValuePair(string s, string name, string defaultIfNotFound) {

            if (s == null) {
                return null;
            }
            List<string> keyValuePairs = s.SplitRetain(new char[] { ';', '=' }, true, true, false);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keyValuePairs.Count; i++) {
                if (keyValuePairs[i].ToLower() == name.ToLower()) {
                    // this is the name of the key to remove
                    // next entry is the =
                    // entry after that is the (possibly quoted) User Id or Password value.
                    // that's the one we want to ignore, so slurp everything up past it

                    // skip over next 3 (moniker, =, value)
                    i += 2;

                    if (i < keyValuePairs.Count) {
                        return keyValuePairs[i];
                    }
                }
            }

            return defaultIfNotFound;

        }


		/// <summary>
		/// Removes all chars except the given keepChars from the string
		/// </summary>
		/// <param name="value"></param>
		/// <param name="charsToKeep"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string KeepChars(string value, string charsToKeep) {
			StringBuilder sb = new StringBuilder();
			char[] origChars = value.ToCharArray();
			char[] keepChars = charsToKeep.ToCharArray();
			foreach (char c in origChars) {
				foreach (char r in keepChars) {
					if (c == r) {
						sb.Append(c);
					}
				}
			}
			return sb.ToString();
		}

		public static string ConcatPairs<T>(Dictionary<string, T> pairs) {
			StringBuilder sb = new StringBuilder();
			foreach (string key in pairs.Keys) {
				string val = pairs[key].ToString();
				if (val == null) {
					sb.Append(key).Append("=;");
				} else {
					sb.Append(key).Append("=").Append(val.Replace(";", @"\;")).Append(";");
				}
			}
			string ret = sb.ToString();
			return ret;
		}

        /// <summary>
        /// Given a string of key/value pairs delimited with = and ;  (e.g.:   key1=val1;key2=val2), returns a dictionary containing the parsed key/value pairs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delimitedKeyValuePairs"></param>
        /// <returns></returns>
		public static Dictionary<string, T> ParsePairs<T>(string delimitedKeyValuePairs) {
			Dictionary<string, T> dic = new Dictionary<string, T>();
			if (!String.IsNullOrEmpty(delimitedKeyValuePairs)) {
				List<string> kvPairs = delimitedKeyValuePairs.SplitRetain(new char[]{';', '='}, true, true, false);
				for (int i=0; i < kvPairs.Count; i++) {

                    string keyword = kvPairs[i].Trim();

                    if (keyword == ";" || keyword == "=") {
                        // just a delimiter, ignore
                    } else {

                        // keyword found.
                        // if next one is an '=', go ahead and try to parse it

                        string value = null;

                        if (i < kvPairs.Count-1) {

                            i++; // jump to the =, if any

                            if (kvPairs[i] == "=") {

                                StringBuilder sb = new StringBuilder();
                                i++; // jump to next item, if any

                                while (i < kvPairs.Count && kvPairs[i] != ";") {
                                    sb.Append(kvPairs[i]);
                                    // keep looping until we run out or we hit an item that is just a ;
                                    i++;
                                }
                                value = sb.ToString().Trim();

                                if (value.IsQuoted()) {
                                    // remove quotes from the ends
                                    value = Toolkit.Cut(value, 1, -1);
                                }

                            }

                        }

                        if (String.IsNullOrEmpty(value)) {
                            value = null;
                        }

                        dic.Add(keyword, (T)Convert.ChangeType(value, typeof(T)));

                    }

                    //string value = kvPairs[i + 1];

                    //int firstEquals = kvPairs[i].IndexOf('=');
                    //string keyword = null;
                    //string value = null;
                    //if (firstEquals > -1) {
                    //    keyword = Toolkit.Cut(kvPairs[i], 0, firstEquals);
                    //    value = Toolkit.Cut(kvPairs[i], firstEquals);

                    //    if ((value.StartsWith("'") && value.EndsWith("'"))
                    //        || (value.StartsWith(@"""") && value.EndsWith(@""""))) {
                    //        // chop off quotes from both ends
                    //        value = Toolkit.Cut(value, 1, -1);
                    //    }


                    //} else {
                    //    // no equals found. ignore.
                    //}
				}
			}
			return dic;
		}
















        /// <summary>
        /// Parses string as contents using the given encoding.  Encoding defaults to UTF8 if null.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="returnHeaderRow"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IEnumerable<List<string>> ParseDelimited(string contents, bool returnHeaderRow, Encoding encoding, char delimiter) {
            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(contents);
            using (var ms = new MemoryStream(bytes)) {
                using (var sr = new StreamReader(ms, encoding)) {
                    foreach (List<string> line in ParseDelimited(sr, returnHeaderRow, delimiter)) {
                        yield return line;
                    }
                }
            }
        }

        public static IEnumerable<List<string>> ParseDelimited(string fileName, bool returnHeaderRow, char delimiter) {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                using (var sr = new StreamReader(fs)) {
                    foreach (List<string> line in ParseDelimited(sr, returnHeaderRow, delimiter)) {
                        yield return line;
                    }
                }
            }
        }

        public static IEnumerable<List<string>> ParseDelimited(TextReader tr, bool returnHeaderRow, char delimiter) {
            if (tr.Peek() > -1) {

                List<string> cols = ParseDelimitedLine(tr, delimiter);
                if (returnHeaderRow) {
                    yield return cols;
                }
                while (tr.Peek() > -1) {
                    yield return ParseDelimitedLine(tr, delimiter);
                }
            }
        }

        public static List<string> ParseDelimitedLine(TextReader tr, char delimiter) {

            return ParseDelimitedLine(tr.ReadLine(true), delimiter);

        }

        public static List<string> ParseDelimitedLine(string line, char delimiter) {

            List<string> values = new List<string>();
            values.AddRange(line.Split(delimiter.ToString(), false)); 
            return values;

        }











        /// <summary>
        /// Parses string as contents using the given encoding.  Encoding defaults to UTF8 if null.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="returnHeaderRow"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static IEnumerable<List<string>> ParseTabDelimited(string contents, bool returnHeaderRow, Encoding encoding) {
            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(contents);
            using (var ms = new MemoryStream(bytes)){
                using (var sr = new StreamReader(ms, encoding)) {
                    foreach (List<string> line in ParseTabDelimited(sr, returnHeaderRow)) {
                        yield return line;
                    }
                }
            }
        }
        
        public static IEnumerable<List<string>> ParseTabDelimited(string fileName, bool returnHeaderRow) {
			using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                using (var sr = new StreamReader(fs)) {
                    foreach (List<string> line in ParseTabDelimited(sr, returnHeaderRow)) {
                        yield return line;
                    }
                }
			}
		}

        //public static IEnumerable<List<string>> ParseTabDelimited(StreamReader sr, bool returnHeaderRow) {
        //    if (!sr.EndOfStream) {
        //        List<string> cols = ParseTabbedLine(sr);
        //        if (returnHeaderRow) {
        //            yield return cols;
        //        }
        //        while (!sr.EndOfStream) {
        //            yield return ParseTabbedLine(sr);
        //        }
        //    }
        //}

        public static IEnumerable<List<string>> ParseTabDelimited(TextReader tr, bool returnHeaderRow) {
            if (tr.Peek() > -1){

                List<string> cols = ParseTabbedLine(tr);
                if (returnHeaderRow) {
                    yield return cols;
                }
                while (tr.Peek() > -1) {
                    yield return ParseTabbedLine(tr);
                }
            }
        }

        public static List<string> ParseTabbedLine(TextReader tr) {

            return ParseTabbedLine(tr.ReadLine(true));

		}

        public static List<string> ParseTabbedLine(string line) {

            List<string> values = new List<string>();
            values.AddRange(line.Split("\t", false));
            return values;

        }

		public static List<string> ParseCSVLine(TextReader sr) {
            char escape = '\0';
            List<string> values = new List<string>();
            StringBuilder sb = new StringBuilder();
            while (true) {
				string line = sr.ReadLine();
				for (int i=0; i < line.Length; i++) {
					char ch = line[i];
					char nextch = (i < line.Length - 1 ? line[i + 1] : '\0');
					if (escape == '\0') {
						if (ch == '"' && sb.ToString().Trim().Length == 0) {
							// we eat the escape char
							escape = ch;
							continue;
						}
					}

					if (ch == escape) {
						if (ch == nextch) {
							// they doubled up the escape char to signify it should be escaped.
							// aka "The man said ""hello!"" as he ran away"

							// just append the escape char and skip the next one
							sb.Append(ch);
							i++;

						} else {
							// end of escaped chars
							escape = '\0';
							// we eat the escape char
						}
					} else if (ch == ',' && escape == '\0') {
						// end of item
						values.Add(sb.ToString());
						sb = new StringBuilder();
					} else {

						// we get here, it's just a 'normal' char and should be tacked on.
						sb.Append(ch);
					}
				}

				if (escape == '\0') {
					break;
				} else {
					// there was an embedded newline that the ReadLine() slurped up.
					// so now we need to re-inject that.
					sb.Append("\n");
				}

			}

			values.Add(sb.ToString());
			return values;
		}

		public static List<List<string>> ParseCSV(TextReader input, bool returnHeaderRow) {
			List<List<string>> ret = new List<List<string>>();
			if (input.Peek() > -1) {
				List<string> cols = ParseCSVLine(input);
				if (returnHeaderRow) {
					ret.Add(cols);
				}
				while (input.Peek() > -1) {
					ret.Add(ParseCSVLine(input));
				}
			}
			return ret;
		}

		public static List<List<string>> ParseCSVByChunk(TextReader input, bool returnHeaderRow, int rowChunkSize) {
			List<List<string>> ret = new List<List<string>>();
			if (input.Peek() > -1) {
				List<string> cols = ParseCSVLine(input);
				if (returnHeaderRow) {
					ret.Add(cols);
				}
				int i = 0;
				while (input.Peek() > -1 && i < rowChunkSize) {
					i++;
					ret.Add(ParseCSVLine(input));
				}
			}
			return ret;
		}

		public static IEnumerable<List<string>> ParseCSVByRow(StreamReader input, bool returnHeaderRow) {
			if (!input.EndOfStream) {
				List<string> cols = ParseCSVLine(input);
				if (returnHeaderRow) {
					yield return cols;
				}
				int chunkSize = 100000;
				while (!input.EndOfStream) {
					int i = 0;
					List<List<string>> valueSet = new List<List<string>>();
					while (!input.EndOfStream && i < chunkSize) {
						valueSet.Add(ParseCSVLine(input));
						i++;
					}
					foreach (List<string> values in valueSet) {
						yield return values;
					}
				}
			}
		}

		public static IEnumerable<List<string>> ParseCSV(string fileName, bool returnHeaderRow) {
			using (StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read))) {
				if (!sr.EndOfStream) {
					List<string> cols = ParseCSVLine(sr);
					if (returnHeaderRow) {
						yield return cols;
					}
					while (!sr.EndOfStream) {
						yield return ParseCSVLine(sr);
					}
				}
			}
		}


		//public static DataTable ParseCSV(string data) {
		//    return ParseCSV(data.Split(new string[] { "\r\n" }, StringSplitOptions.None));
		//}

		public static long OutputCSVHeader(string[] columnNames, StreamWriter output) {
			long outputLength = 0;
			for (int i=0; i < columnNames.Length; i++) {
				string towrite = "\"" + columnNames[i].Replace("\"", "\"\"") + "\"";
				output.Write(towrite);
				outputLength += towrite.Length;
				if (i < columnNames.Length - 1) {
					output.Write(",");
					outputLength += 1;
				} else {
					output.WriteLine();
					outputLength += 2;
				}
			}
			return outputLength;
		}
		public static long OutputCSV(object[] data, TextWriter output, bool alwaysOutputQuotes) {
			long outputLength = 0;
			for(int i=0;i<data.Length;i++){
				string towrite = null;
				if (data[i] == null || data[i].ToString() == String.Empty){
					towrite = alwaysOutputQuotes ? "\"\"" : String.Empty;
				} else {
					towrite = data[i].ToString();
					if (alwaysOutputQuotes || towrite.Contains("\r") || towrite.Contains("\n") || towrite.Contains(",") || towrite.Contains("\"")) {
						// need to escape in double quotes
						towrite = "\"" + towrite.Replace("\"", "\"\"") + "\"";
					}
				}
				outputLength += towrite.Length;
				output.Write(towrite);

				if (i < data.Length - 1) {
					// not last field
					output.Write(",");
					outputLength += 1;
				} else {
					// last field
					output.WriteLine();
					outputLength += 2;
				}
			}
			return outputLength;
		}

		public static int OutputTabbedHeader(string[] columnNames, TextWriter output) {
			int charCount = 0;
			for (int i=0; i < columnNames.Length; i++) {
				string towrite = columnNames[i].Replace("\t", "\\t");
				output.Write(towrite);
				charCount += towrite.Length;
				if (i < columnNames.Length - 1) {
					output.Write("\t");
					charCount++;
				} else {
					output.WriteLine();
					charCount += 2;
				}
			}
			return charCount;
		}

        public static int OutputDelimitedHeader(string[] columnNames, TextWriter output, char delimiter, char escapeCharacter) {
            int charCount = 0;
            var sdelim = delimiter.ToString();
            var newdelim = @"\" + delimiter;
            for (int i = 0; i < columnNames.Length; i++) {
                string towrite = columnNames[i].Replace(sdelim, newdelim);
                output.Write(towrite);
                charCount += towrite.Length;
                if (i < columnNames.Length - 1) {
                    output.Write(sdelim);
                    charCount++;
                } else {
                    output.WriteLine();
                    charCount += 2;
                }
            }
            return charCount;
        }


        /// <summary>
        /// Writes data to the given stream in tab-delimited, assuming one row consists of fieldsPerRow (which should divide evenly into the data.Length).  If outputNullAsBackslashN, a DBNull.Value is output as "\N" (uppercase N as opposed to lowercase n for newline ("\n")).  Otherwise DBNull.Value is output as "" (zero-length string).
        /// </summary>
        /// <param name="data"></param>
        /// <param name="output"></param>
        /// <param name="fieldsPerRow"></param>
        /// <param name="outputNullAsBackslashN"></param>
        /// <returns></returns>
        public static int OutputDelimitedData(object[] data, TextWriter output, char delimiter, char escapeCharacter, int fieldsPerRow, bool outputNullAsBackslashN) {
            var sdelim = delimiter.ToString();
            var newdelim = @"\" + delimiter;
            int rowCount = data.Length / fieldsPerRow;
            int charCount = 0;
            for (int i = 0; i < rowCount; i++) {
                int rowNumber = i * fieldsPerRow;
                for (int fieldOffset = 0; fieldOffset < fieldsPerRow; fieldOffset++) {
                    string towrite = null;
                    if (data[rowNumber + fieldOffset] == null || data[rowNumber + fieldOffset] == DBNull.Value) {
                        if (outputNullAsBackslashN) {
                            towrite = @"\N";
                        } else {
                            towrite = string.Empty;
                        }
                    } else {
                        towrite = data[rowNumber + fieldOffset].ToString();
                        if (towrite.Contains(@"\")) {
                            towrite = towrite.Replace(@"\", @"\\");
                        }
                        if (towrite.Contains("\r")) {
                            towrite = towrite.Replace("\r", "\\r");
                        }
                        if (towrite.Contains("\n")) {
                            towrite = towrite.Replace("\n", "\\n");
                        }
                        if (towrite.Contains(sdelim)) {
                            towrite = towrite.Replace(sdelim, newdelim);
                        }
                    }
                    output.Write(towrite);
                    charCount += towrite.Length;

                    if (fieldOffset < fieldsPerRow - 1) {
                        // not last field for this row
                        output.Write(sdelim);
                        charCount++;
                    } else {
                        // last field for this row
                        output.WriteLine();
                        charCount += 2;
                    }
                }
            }
            return charCount;
        }


        /// <summary>
        /// Stream data to resp.  Format can be csv, tab, json, or xml.  
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="resp"></param>
        /// <param name="format"></param>
        /// <param name="noCaptions"></param>
        /// <param name="compact"></param>
        public static void OutputData(DataTable dt, HttpResponse resp, string format, bool noCaptions, bool compact) {

            resp.Clear();
            using (var sw = new StreamWriter(resp.OutputStream)) {
                switch (format) {
                    case "xml":
                        resp.ContentType = "text/xml";
                        break;
                    case "text":
                    case "txt":
                    case "tab":
                        resp.ContentType = "text/plain";
                        break;
                    case "csv":
                        resp.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", "result"));
                        resp.Charset = "";
                        resp.ContentType = "text/csv";
                        break;
                    case "json":
                        resp.ContentType = "application/json";
                        break;
                    default:
                        throw new NotSupportedException(getDisplayMember("OutputData", "format='{0}' is not a valid option in Toolkit.OutputData().", format));
                }

                Toolkit.OutputData(dt, sw, format, noCaptions, compact);
                if (format == "csv") resp.End();
            }

        }

        public static void OutputData(DataTable dt, StreamWriter sw, string format, bool noCaptions, bool compact) {
            // determine proper column names
            var cols = new List<string>();
            if (noCaptions) {
                foreach (DataColumn dc in dt.Columns) {
                    cols.Add(dc.ColumnName);
                }
            } else {
                foreach (DataColumn dc in dt.Columns) {
                    if (String.IsNullOrEmpty(dc.Caption)) {
                        cols.Add(dc.ColumnName);
                    } else {
                        cols.Add(dc.Caption);
                    }
                }
            }

            var colNames = cols.ToArray();

            switch (format) {
                case "xml":
                    dt.WriteXml(sw.BaseStream, XmlWriteMode.WriteSchema);
                    break;
                case "text":
                case "txt":
                case "tab":
                    Toolkit.OutputTabbedHeader(colNames, sw);
                    foreach (DataRow dr in dt.Rows) {
                        Toolkit.OutputTabbedData(dr.ItemArray, sw, !compact);
                    }

                    break;
                case "csv":
                    Toolkit.OutputCSVHeader(colNames, sw);
                    foreach (DataRow dr in dt.Rows) {
                        Toolkit.OutputCSV(dr.ItemArray, sw, !compact);
                    }

                    break;
                case "json":
                    Toolkit.OutputJsonData(colNames, dt, sw, !compact);
                    break;
                default:
                    throw new NotSupportedException(getDisplayMember("OutputData", "format='{0}' is not a valid option in Toolkit.OutputData().", format));
            }
        }

        /// <summary>
        /// rowAsObject = true: Output is similar to { 'dvname' : { columns : ['col1','col2'], rows : [ ['val1', 'val2'], ['val3', 'val4'] ] } }
        /// rowAsObject = false: Output is similar to { 'dvname' : { columns : ['col1','col2'], rows : [ {'col1':'val1', 'col2':'val2'}, {'col1':'val3', 'col2':'val4'} ] } }
        /// If cols is null or empty, it is determined from the DataTable itself.
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="dt"></param>
        /// <param name="sw"></param>
        public static void OutputJsonData(string[] cols, DataTable dt, StreamWriter sw, bool rowAsObject) {

            if (cols == null || cols.Length == 0) {
                cols = new string[dt.Columns.Count];
                for(var i=0;i<dt.Columns.Count;i++){
                    var dc = dt.Columns[i];
                    if (String.IsNullOrEmpty(dc.Caption)) {
                        cols[i] = dc.ColumnName;
                    } else {
                        cols[i] = dc.Caption;
                    }
                }
            }

            // output a JSON-formatted stream
            sw.WriteLine("{ \"" + dt.TableName + "\" : { ");
            sw.WriteLine(" columns : [");
            for (var i = 0; i < cols.Length; i++) {
                if (i > 0) {
                    sw.Write(@", """ + cols[i].Replace("\"", "\\\"") + @"""");
                } else {
                    sw.Write(@"""" + cols[i].Replace("\"", "\\\"") + @"""");
                }
            }

            sw.WriteLine("], rows : [ ");
            var dateColumns = new List<int>();
            for (var i = 0; i < dt.Columns.Count; i++) {
                if (dt.Columns[i].DataType == typeof(DateTime)) {
                    dateColumns.Add(i);
                }
            }

            for(var i=0;i<dt.Rows.Count;i++){
                var dr = dt.Rows[i];
                var vals = dr.ItemArray;
                foreach (var j in dateColumns) {
                    // notice we emit datetime data in the same value as it is stored,
                    // which should always be in UTC (GMT, whatever) so the client
                    // will need to convert it to local time as needed.
                    if (vals[j] != DBNull.Value) {
                        vals[j] = ((DateTime)vals[j]).ToString("yyyy/MM/dd hh:mm:ss tt");
                    }
                }

                if (i > 0) {
                    sw.Write(", ");
                }
                if (rowAsObject) {
                    sw.Write(" { ");
                } else {
                    sw.Write(" [ ");
                }

                for (var j = 0; j < vals.Length; j++) {
                    if (j > 0){
                        sw.Write(", ");
                    }
                    var towrite = string.Empty;
                    if (vals[j] != null && vals[j] != DBNull.Value) {
                        towrite = vals[j].ToString();
						if (towrite.Contains(@"\")) {
							towrite = towrite.Replace(@"\", @"\\");
						}
						if (towrite.Contains("\"")) {
							towrite = towrite.Replace("\"", "\\\"");
						}

                    }

                    if (rowAsObject) {
                        sw.Write(@"""" + cols[j] + @""":""" + towrite + @"""");
                    } else {
                        sw.Write(@"""" + towrite + @"""");
                    }


                }
                if (rowAsObject) {
                    sw.Write(" }");
                } else {
                    sw.Write(" ]");
                }
            }
            sw.WriteLine(" ] } } ");
        }

        /// <summary>
        /// Output is similar to { 'dvname' : { columns : ['col1','col2'], rows : ['col1':'val1', 'col2':'val2'] } }
        /// If cols is null or empty, it is determined from the DataTable itself.
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="dt"></param>
        /// <param name="sw"></param>
        public static void OutputJsonData(string[] columnNames, object[] data, StreamWriter output) {

			int rowCount = data.Length / columnNames.Length;
			for (int i=0; i < rowCount; i++) {
                int rowNumber = i * columnNames.Length;
                if (i > 0) {
                    output.Write(",[");
                } else {
                    output.Write("[");
                }
                for (int fieldOffset = 0; fieldOffset < columnNames.Length; fieldOffset++) {

                    
                    string towrite = null;
					if (data[rowNumber + fieldOffset] == null || data[rowNumber + fieldOffset] == DBNull.Value) {
                        towrite = String.Empty;
					} else {
						towrite = data[rowNumber + fieldOffset].ToString();
						if (towrite.Contains(@"\")) {
							towrite = towrite.Replace(@"\", @"\\");
						}
						if (towrite.Contains("\"")) {
							towrite = towrite.Replace("\"", "\\\"");
						}
					}
                    if (fieldOffset > 0) {
                        // not last field for this row
                        output.Write(", ");
                    }
                    output.Write(@"""" + columnNames[i].Replace("\"", "\\\"") + @""":""" + towrite + @"""");

				}
                output.Write("]");
            }
        }

        /// <summary>
        /// Writes data to the given stream in tab-delimited, assuming one row consists of fieldsPerRow (which should divide evenly into the data.Length).  If outputNullAsBackslashN, a DBNull.Value is output as "\N" (uppercase N as opposed to lowercase n for newline ("\n")).  Otherwise DBNull.Value is output as "" (zero-length string).
        /// </summary>
        /// <param name="data"></param>
        /// <param name="output"></param>
        /// <param name="fieldsPerRow"></param>
        /// <param name="outputNullAsBackslashN"></param>
        /// <returns></returns>
		public static int OutputTabbedData(object[] data, TextWriter output, int fieldsPerRow, bool outputNullAsBackslashN) {
			int rowCount = data.Length / fieldsPerRow;
			int charCount = 0;
			for (int i=0; i < rowCount; i++) {
				int rowNumber = i * fieldsPerRow;
				for (int fieldOffset = 0; fieldOffset < fieldsPerRow; fieldOffset++) {
					string towrite = null;
					if (data[rowNumber + fieldOffset] == null || data[rowNumber + fieldOffset] == DBNull.Value) {
						if (outputNullAsBackslashN) {
							towrite = @"\N";
						} else {
							towrite = string.Empty;
						}
					} else {
						towrite = data[rowNumber + fieldOffset].ToString();
                        if (towrite.Contains(@"\")){
                            towrite = towrite.Replace(@"\", @"\\");
                        }
						if (towrite.Contains("\r")) {
							towrite = towrite.Replace("\r", "\\r");
						}
						if (towrite.Contains("\n")) {
							towrite = towrite.Replace("\n", "\\n");
						}
						if (towrite.Contains("\t")) {
							towrite = towrite.Replace("\t", "\\t");
						}
					}
					output.Write(towrite);
					charCount += towrite.Length;

					if (fieldOffset < fieldsPerRow - 1) {
						// not last field for this row
						output.Write("\t");
						charCount++;
					} else {
						// last field for this row
						output.WriteLine();
						charCount += 2;
					}
				}
			}
			return charCount;
		}
        /// <summary>
        /// Writes data to the given stream in tab-delimited format as a single row.  If outputNullAsBackslashN, a DBNull.Value is output as "\N" (uppercase N as opposed to lowercase n for newline ("\n")).  Otherwise DBNull.Value is output as "" (zero-length string).
        /// </summary>
        /// <param name="data"></param>
        /// <param name="output"></param>
        /// <param name="outputNullAsBackslashN"></param>
        /// <returns></returns>
		public static int OutputTabbedData(object[] data, TextWriter output, bool outputNullAsBackslashN) {
			return OutputTabbedData(data, output, data.Length, outputNullAsBackslashN);
		}

        /// <summary>
        /// Writes data to the given stream in tab-delimited with nulls as a \N.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static int OutputTabbedData(object[] data, TextWriter output) {
			return OutputTabbedData(data, output, data.Length, true);
		}


		/// <summary>
		/// Given seconds, returns elapsed time in 00:00:00 format.
		/// </summary>
		/// <param name="totalSeconds"></param>
		/// <returns></returns>
		public static string FormatTimeElapsed(int totalSeconds) {
			TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);
			string elapsed = ts.Hours.ToString("00:") + ts.Minutes.ToString("00:") + ts.Seconds.ToString("00");
			return elapsed;
		}

		/// <summary>
		/// Given milliseconds, returns elapsed time in 00:00:00.000 format.
		/// </summary>
		/// <param name="totalSeconds"></param>
		/// <returns></returns>
		public static string FormatTimeElapsed(double totalMilliseconds) {
			TimeSpan ts = TimeSpan.FromMilliseconds(totalMilliseconds);
			string elapsed = ts.Hours.ToString("00:") + ts.Minutes.ToString("00:") + ts.Seconds.ToString("00") + "." + ts.Milliseconds.ToString("000");
			return elapsed;
		}

		/// <summary>
		/// Removes the given removeChars from the given string
		/// </summary>
		/// <param name="value"></param>
		/// <param name="charsToRemove"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string StripChars(string value, string charsToRemove) {
			StringBuilder sb = new StringBuilder();
			char[] origChars = value.ToCharArray();
			char[] removeChars = charsToRemove.ToCharArray();
			foreach (char c in origChars) {
				foreach (char r in removeChars) {
					if (c != r) {
						sb.Append(c);
					}
				}
			}

			return sb.ToString();

			//string temp = null;
			//foreach (char c in removeChars) {
			//    temp += value.Replace(c.ToString(), "");
			//}
			//return temp;
		}

        /// <summary>
        /// Returns first value passed that is neither null nor DBNull.Value.  Returns null otherwise (not DBNull.Value)
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static object Coalesce(params object[] values) {
            object lastVal = null;
            foreach (object v in values) {
                lastVal = v;
                if (v != null && v != DBNull.Value) {
                    if (v is String) {
                        if (v as String != String.Empty) {
                            // empty string, keep looking.
                            return v;
                        }
                    } else {
                        return v;
                    }
                }
            }
            return lastVal;
        }


        		/// <summary>
		/// Calls ToString() on each element in given array and inserts the given separator string between each element.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="separator"></param>
        /// <param name="defaultValueIfEmpty">Value to return if given array is null or contains no elements</param>
		/// <returns></returns>
        [ComVisible(true)]
        public static string Join(Array a, string separator, string defaultValueIfEmpty) {
            return Join(a, separator, defaultValueIfEmpty, null, null);
        }

        /// <summary>
        /// Calls ToString() on each row in given DataTable for given field.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fieldName"></param>
        /// <param name="separator"></param>
        /// <param name="defaultValueIfEmpty"></param>
        /// <returns></returns>
        [ComVisible(true)]
        public static string Join(DataTable dt, string fieldName, string separator, string defaultValueIfEmpty) {
            var sb = new StringBuilder();
            if (dt == null || dt.Rows.Count == 0 || String.IsNullOrEmpty(fieldName)) {
                return defaultValueIfEmpty;
            }

            foreach (DataRow dr in dt.Rows) {
                sb.Append(dr[fieldName].ToString()).Append(separator);
            }
            if (sb.Length > separator.Length) {
                sb.Length -= separator.Length;
            }

            return sb.ToString();
        }

		/// <summary>
		/// Calls ToString() on each element in given array and inserts the given separator string between each element.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="separator"></param>
        /// <param name="defaultValueIfEmpty">Value to return if given array is null or contains no elements</param>
        /// <param name="startDelimiter">Delimiter to prepend to output if array has 1 or more elements.  Pass null or empty string to ignore.</param>
        /// <param name="endDelimiter">Delimiter to append to output if array has 1 or more elements.  Pass null or empty string to ignore.</param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string Join(Array a, string separator, string defaultValueIfEmpty, string startDelimiter, string endDelimiter) {
			StringBuilder sb = new StringBuilder();
            if (a == null || a.Length == 0) {
                return defaultValueIfEmpty;
            }
			foreach (object o in a) {
				if (o != null) {
					sb.Append(o.ToString());
				}
				sb.Append(separator);
			}
			if (sb.Length > separator.Length) {
				sb.Remove(sb.Length - separator.Length, separator.Length);
			}

			string ret = sb.ToString();
            if (ret.Length == 0) {
                ret = defaultValueIfEmpty;
            }
            if (!String.IsNullOrEmpty(startDelimiter)) {
                ret = startDelimiter + ret;
            }
            if (!String.IsNullOrEmpty(endDelimiter)) {
                ret = ret + endDelimiter;
            }
			return ret;
		}

        /// <summary>
        /// Calls ToString() on each element in given array and inserts the given separator string between each element, insert delimiter on both side of each element.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="separator"></param>
        /// <param name="defaultValueIfEmpty">Value to return if given array is null or contains no elements</param>
        /// <param name="Delimiter">Delimiter to prepend and append the element.  Pass null or empty string to ignore.</param>
        /// <returns></returns>
        [ComVisible(true)]
        public static string Join(Array a, string separator, string defaultValueIfEmpty, string Delimiter)
        {
            StringBuilder sb = new StringBuilder();
            if (a == null || a.Length == 0)
            {
                return defaultValueIfEmpty;
            }
            foreach (object o in a)
            {
                if (o != null)
                {
                    sb.Append(Delimiter + o.ToString() + Delimiter);
                }
                sb.Append(separator);
            }
            if (sb.Length > separator.Length)
            {
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }

            string ret = sb.ToString();
            if (ret.Length == 0)
            {
                ret = defaultValueIfEmpty;
            }
            return ret;
        }
		/// <summary>
		/// Makes the given string safe to put into an xml stream. escapes &lt;, &gt;, &amp;, &quot, &apos;
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string XmlEncode(string value) {
			string ret = (String.IsNullOrEmpty(value) ? "" : value.Replace("&", "&amp;").Replace("<","&lt;").Replace(">","&gt;").Replace("'","&apos;").Replace("\"","&quot"));
			return ret;
		}

		/// <summary>
		/// Makes the given string non-xml formatted.  (unencodes the 5 special chars from XmlEncode)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string XmlDecode(string value) {
			string ret = (String.IsNullOrEmpty(value) ? "" : value.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&amp;", "&"));
			return ret;
		}

		/// <summary>
		/// Cuts a string at the given starting position, which can be negative.  Does not throw exceptions.
		/// </summary>
		/// <param name="value">String to cut</param>
		/// <param name="startPosition">If positive, from the left.  If negative, from the right. If > length of value, returns all chars right of start position</param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string Cut(string value, int startPosition) {
			return Cut(value, startPosition, (value == null ? 0 : value.Length));
		}

		/// <summary>
		/// Cuts a string at the given starting position for the given length of chars, both of which can be negative.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="startPosition"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string Cut(string value, int startPosition, int length) {
			if (String.IsNullOrEmpty(value)) {
				return value;
			}

			if (length != value.Length && startPosition != 0) {
				// they specified both ends.
				// recurse to resolve front end first
				value = Toolkit.Cut(value, startPosition, value.Length);
                startPosition = 0;
			}

			if (length < 0) {
				// they're saying exclude last 'length' characters.
				// so Toolkit.Cut('asdf', 0, -2) = 'as'
				length = value.Length - Math.Abs(length);
				if (length < 0) {
					// they're trying to cut off more than they have.
					// i.e. Toolkit.Cut('asdf', 0, -8) = ''
					// return zero-length string
					return string.Empty;
				}
			}
			if (length > value.Length) {
				// they're saying return more than they have.
				// limit to all there is
				// ie.. Toolkit.Cut('asdf', 0, 8) = 'asdf'
				length = value.Length;
			}




				/*
				// startPosition is actually end position.
				// substract length from startPosition to get real start position
				startPosition += length;
				// and toggle length so it's now positive
				length *= -1;
				 */
			if (startPosition < 0) {
				// pull from the right
				startPosition = value.Length + startPosition;
				// if they specified a negative start position less than the length, just pull from the beginning.
			}

			if (startPosition < 0) {
				startPosition = 0;
			}

			if (startPosition + length > value.Length) {
				// they said pull more than they have. just return whole string.
				return value.Substring(startPosition);
			} else if (startPosition + length < 0){
				// they said pull negative chars. return empty string.
				return string.Empty;
			} else {
				// they said pull a valid subset of chars.
				return value.Substring(startPosition, length);
			}

		}

		/// <summary>
		/// base64-encodes the given plain text
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public static string ToBase64(string plainText) {
			byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			string ret = Convert.ToBase64String(encodedBytes);
			return ret;
		}

		/// <summary>
		/// base64-decodes the given base64 text
		/// </summary>
		/// <param name="base64Text"></param>
		/// <returns></returns>
		public static string FromBase64(string base64Text) {
			byte[] plainBytes = Convert.FromBase64String(base64Text);
			string ret = System.Text.Encoding.UTF8.GetString(plainBytes);
			return ret;
		}

		/// <summary>
		/// Converts the given byte array to a hex string. null byte array returns null string. empty byte array returns empty string.
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static string ToHexString(byte[] bytes) {
			if (bytes == null) {
				return null;
			}
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++) {
				sb.Append(bytes[i].ToString("X").PadLeft(2, '0'));
			}
			string ret = sb.ToString();
			return ret;
		}

		public static int Max(params int[] values) {
			if (values == null || values.Length == 0) {
				return int.MaxValue;
			} else {
				int max = int.MinValue;
				for (int i=0; i < values.Length; i++) {
					if (values[i] > max) {
						max = values[i];
					}
				}
				return max;
			}
		}

		public static int Minimum(params int[] values) {
			if (values == null || values.Length == 0) {
				return int.MinValue;
			} else {
				int min = int.MaxValue;
				for (int i=0; i < values.Length; i++) {
					if (values[i] < min) {
						min = values[i];
					}
				}
				return min;
			}
		}

		[ComVisible(true)]
		public static byte[] ToByteArray(params long[] values) {
			byte[] bytes = new byte[values.Length * 8];
			for (int i=0; i < values.Length; i++) {
				Array.Copy(BitConverter.GetBytes(values[i]), 0, bytes, i * 8, 8);
			}
			return bytes;
		}

		[ComVisible(true)]
		public static byte[] ToByteArray(params int[] values) {
			byte[] bytes = new byte[values.Length * 4];
			for(int i=0;i<values.Length;i++){
				Array.Copy(BitConverter.GetBytes(values[i]), 0, bytes, i * 4, 4);
			}
			return bytes;
		}

		/// <summary>
		/// Given a hex string converts it to a byte array.  null hex string returns a null byte array. empty hex string returns a byte array of length 0.
		/// </summary>
		/// <param name="hexString"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static byte[] ToByteArray(string hexString) {
			if (hexString == null) {
				return null;
			}
			List<byte> bytes = new List<byte>();
			int size = hexString.Length / 2;
			for (int i = 0; i < size; i += 2) {
				string bs = hexString.Substring(i, 2);
				byte b = byte.Parse(bs, System.Globalization.NumberStyles.HexNumber);
				bytes.Add(b);
			}
			byte[] ret = bytes.ToArray();
			return ret;
		}

		/// <summary>
		/// Given an object that can represent a Guid (object, string, or byte[]), returns the corresponding Guid object.  Returns defaultValue if parsing fails.
		/// </summary>
		/// <param name="guidString"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Guid? ToGuid(object guid, Guid? defaultValue) {
			if (guid == null) {
				return defaultValue;
			} else if (guid is Guid) {
				return (Guid)guid;
			} else if (guid is byte[]) {
				return new Guid((byte[])guid);
			} else {
				return ToGuid(guid.ToString(), defaultValue);
			}
		}


		/// <summary>
		/// Given the string representation of a Guid, returns the corresponding Guid object.  Returns defaultValue if parsing fails.
		/// </summary>
		/// <param name="guidString"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Guid? ToGuid(string guidString, Guid? defaultValue) {
			try {	
				Guid? test = new Guid(guidString);
				return test;
			} catch {
				// couldn't parse it.  bomb!
				return defaultValue;
			}
		}

		/// <summary>
		/// Given an octet hex string (whose bytes are partially reverse ordered compared to a guid), returns the corresponding Guid value
		/// </summary>
		/// <param name="octetHexString"></param>
		/// <returns></returns>
		public static Guid ToGuid(string octetHexString) {
			byte[] octetBytes = new byte[16];
			for (int i = 0; i < 16; i++) {
				string bs = Toolkit.Cut(octetHexString, i * 2, 2);
				octetBytes[i] = byte.Parse(bs, System.Globalization.NumberStyles.HexNumber);
			}
			byte[] guidBytes = guidOctetTransfom(octetBytes);
			Guid guid = new Guid(guidBytes);
			return guid;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static string ToOctetHexString(Guid guid) {
			byte[] octetBytes = ToOctetByteArray(guid);
			StringBuilder sb = new StringBuilder();
			foreach (byte b in octetBytes) {
				sb.Append(b.ToString("X"));
			}
			string output = sb.ToString();
			return output;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static byte[] ToOctetByteArray(Guid guid) {
			if (guid == Guid.Empty) {
				guid = Guid.NewGuid();
			}
			byte[] output = guidOctetTransfom(guid.ToByteArray());
			return output;
		}

		private static byte[] guidOctetTransfom(byte[] input){
			byte[] output = new byte[16];

			// octet byte arrays are actually guid's but the higher-order bytes are reordered for
			// whatever silly reason.  here's the mapping:

			// GUID byte   0  1  2  3 - 4  5 - 6  7 - 8  9 - 10 11 12 13 14 15
			// Octet byte  3  2  1  0 - 5  4 - 7  6 - 8  9 - 10 11 12 13 14 15
			int[] mapping = new int[16] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };

			// this is a commutable transform, so the inverse is true as well

			// use the mapping to copy over the bytes
			for(int i=0;i<16;i++){
				output[i] = input[mapping[i]];
			}

			return output;

		}
		#endregion

		#region Type conversions


		/// <summary>
		/// Tries to parse the given value to a nullable short, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static short? ToInt16(object value, short? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return null;
			} else if (value is string) {
				return ToInt16((string)value, defaultValue);
			} else if (!(value is short?)) {
				return ToInt16(value.ToString(), defaultValue);
			} else {
				try {
					return (short?)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Tries to parse the given value to a non-nullable short, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static short ToInt16(object value, short defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else if (value is string) {
				return ToInt16((string)value, defaultValue);
			} else if (!(value is short)) {
				return ToInt16(value.ToString(), defaultValue);
			} else {
				try {
					return (short)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the given string to a nullable short, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static short? ToInt16(string value, short? defaultValue) {
			short ret;
			if (short.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a short, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static short ToInt16(string value, short defaultValue) {
			return (short)ToInt16(value, (short?)defaultValue);
		}





		/// <summary>
		/// Tries to parse the given value to a nullable ushort, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ushort? ToUInt16(object value, ushort? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return null;
			} else if (value is string) {
				return ToUInt16((string)value, defaultValue);
			} else if (! (value is ushort?)) {
				return ToUInt16(value.ToString(), defaultValue);
			} else {
				try {
					return (ushort?)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Tries to parse the given value to a non-nullable ushort, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ushort ToUInt16(object value, ushort defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else if (value is string) {
				return ToUInt16((string)value, defaultValue);
			} else if (!(value is ushort)) {
				return ToUInt16(value.ToString(), defaultValue);
			} else {
				try {
					return (ushort)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the given string to a nullable ushort, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ushort? ToUInt16(string value, ushort? defaultValue) {
			ushort ret;
			if (ushort.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a ushort, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ushort ToUInt16(string value, ushort defaultValue) {
			return (ushort)ToUInt16(value, (ushort?)defaultValue);
		}


		/// <summary>
		/// Tries to parse the given value into a nullable int, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static uint? ToUInt32(object value, uint? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return null;
			} else if (value is string) {
				return ToUInt32((string)value, defaultValue);
			} else if (!(value is uint?)) {
				return ToUInt32(value.ToString(), defaultValue);
			} else {
				try {
					return (uint?)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Tries to parse the given value into a non-nullable int, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static uint ToUInt32(object value, uint defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else if (value is string) {
				return ToUInt32((string)value, defaultValue);
			} else if (!(value is uint)) {
				return ToUInt32(value.ToString(), defaultValue);
			} else {
				try {
					return (uint)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the given string to a nullable int, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static uint ToUInt32(string value, uint defaultValue) {
			uint ret;
			if (uint.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}


		/// <summary>
		/// Converts the given string to a nullable int, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static uint? ToUInt32(string value, uint? defaultValue) {
			uint ret;
			if (uint.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}





		/// <summary>
		/// Tries to parse the given value ulongo a nullable ulong, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ulong? ToUInt64(object value, ulong? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return null;
			} else if (value is string) {
				return ToUInt64((string)value, defaultValue);
			} else if (!(value is ulong?)) {
				return ToUInt64(value.ToString(), defaultValue);
			} else {
				try {
					return (ulong?)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Tries to parse the given value ulongo a non-nullable ulong, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ulong ToUInt64(object value, ulong defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else if (value is string) {
				return ToUInt64((string)value, defaultValue);
			} else if (!(value is ulong)) {
				return ToUInt64(value.ToString(), defaultValue);
			} else {
				try {
					return (ulong)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the given string to a nullable ulong, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ulong? ToUInt64(string value, ulong? defaultValue) {
			ulong ret;
			if (ulong.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a ulong, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static ulong ToUInt64(string value, ulong defaultValue) {
			return (ulong)ToUInt64(value, (ulong?)defaultValue);
		}











		/// <summary>
		/// Tries to parse the given value longo a nullable long, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long? ToInt64(object value, long? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return null;
			} else if (value is string) {
				return ToInt64((string)value, defaultValue);
			} else if (!(value is long?)) {
				return ToInt64(value.ToString(), defaultValue);
			} else {
				try {
					return (long?)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Tries to parse the given value longo a non-nullable long, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToInt64(object value, long defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else if (value is string) {
				return ToInt64((string)value, defaultValue);
			} else if (!(value is long)) {
				return ToInt64(value.ToString(), defaultValue);
			} else {
				try {
					return (long)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the given string to a nullable long, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long? ToInt64(string value, long? defaultValue) {
			long ret;
			if (long.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a long, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToInt64(string value, long defaultValue) {
			return (long)ToInt64(value, (long?)defaultValue);
		}



		/// <summary>
		/// Tries to parse the given value into a nullable int, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int? ToInt32(object value, int? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return null;
			} else if (value is string) {
				return ToInt32((string)value, defaultValue);
			} else if (!(value is int?)) {
				return ToInt32(value.ToString(), defaultValue);
			} else {
				try {
					return (int?)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Tries to parse the given value into a non-nullable int, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int ToInt32(object value, int defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else if (value is string) {
				return ToInt32((string)value, defaultValue);
			} else if (!(value is int)) {
				return ToInt32(value.ToString(), defaultValue);
			} else {
				try {
					return (int)value;
				} catch {
					return defaultValue;
				}
			}
		}

		/// <summary>
		/// Converts the given string to a nullable int, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int? ToInt32(string value, int? defaultValue) {
			int ret;
			if (int.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given value to an int using the specified enumeration type to do the conversion.  Does not throw errors. If conversion fails, returns defaultValue instead.
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
        [DebuggerStepThrough()]
		public static int ToEnum(Type enumType, string value, int defaultValue) {
			if (value == null) {
				return defaultValue;
			}
			try {
				int temp = (int)Enum.Parse(enumType, value, true);
				return temp;
			} catch {
				// parsing exception. return default.
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a nullable float, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float? ToFloat(string value, float? defaultValue) {
			float ret;
			if (float.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Tries to parse the given value into a nullable float, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float? ToFloat(object value, float? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else {
				return ToFloat(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// Tries to parse the given value into a non-nullable float, Returns defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float ToFloat(object value, float defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else {
				return ToFloat(value.ToString(), defaultValue);
			}
		}


		/// <summary>
		/// Converts the given string to a nullable double, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double? ToDouble(string value, double? defaultValue) {
			double ret;
			if (double.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Tries to parse the given value into a nullable double, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double? ToDouble(object value, double? defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else {
				return ToDouble(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// Tries to parse the given value into a non-nullable decimal, returning defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double ToDouble(object value, double defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else {
				return ToDouble(value.ToString(), defaultValue);
			}
		}


		/// <summary>
		/// Converts the given string to a nullable decimal, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal? ToDecimal(string value, decimal? defaultValue) {
			decimal ret;
			if (decimal.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Tries to parse the given value into a nullable decimal, returning null if given value is null or DBNull.Value.  Returns defaultValue otherwise if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal? ToDecimal(object value, decimal? defaultValue) {
			if (value is DBNull || value == null) {
				return defaultValue;
			} else {
				return ToDecimal(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// Tries to parse the given value into a non-nullable decimal, returning defaultValue if parse fails.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal ToDecimal(object value, decimal defaultValue) {
			if (value == DBNull.Value || value == null) {
				return defaultValue;
			} else {
				return ToDecimal(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// Converts the given string to a non-nullable bool, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool ToBoolean(string value, bool defaultValue) {
			bool ret;
			if (bool.TryParse(value, out ret)) {
				return ret;
            } else {
                if (!String.IsNullOrEmpty(value)){
			        if (value == "1" || value.ToLower() == "y"){
                        return true;
                    } else if (value == "0" || value.ToLower() == "n"){
                        return false;
                    }
                }
				return defaultValue;
			}
		}


		/// <summary>
		/// Converts the given string to a nullable bool, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool? ToBoolean(string value, bool? defaultValue) {
			bool ret;
			if (bool.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given object to a nullable bool, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool? ToBoolean(object value, bool? defaultValue) {
			if (value == null) {
				return defaultValue;
			} else {
				return ToBoolean(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// Converts the given object to a non-nullable bool, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static bool ToBoolean(object value, bool defaultValue) {
			if (value == null) {
				return defaultValue;
			} else {
				return ToBoolean(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <param name="assumeLocalTime"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(string value, string format, bool assumeLocalTime, DateTime defaultValue) {
			DateTime ret;
			DateTimeStyles dts = (assumeLocalTime ? DateTimeStyles.AssumeLocal : DateTimeStyles.AssumeUniversal);

			if (DateTime.TryParseExact(value, format, null, dts, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}


		/// <summary>
		/// Converts the given string to a nullable DateTime, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime? ToDateTime(string value, DateTime? defaultValue) {
			DateTime ret;
			if (DateTime.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}


		/// <summary>
		/// Converts the given string to a non-nullable DateTime, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(string value, DateTime defaultValue) {
			DateTime ret;
			if (DateTime.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a non-nullable DateTime, returning DateTime.MinValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(string value) {
			return ToDateTime(value, DateTime.MinValue);
		}




		/// <summary>
		/// Converts the given object to a nullable DateTime, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime? ToDateTime(object value, DateTime? defaultValue) {
			if (value == null) {
				return defaultValue;
			} else {
				return ToDateTime(value.ToString(), defaultValue);
			}
		}


		/// <summary>
		/// Converts the given object to a non-nullable DateTime, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object value, DateTime defaultValue) {
			if (value == null) {
				return defaultValue;
			} else {
				return ToDateTime(value.ToString(), defaultValue);
			}
		}

		/// <summary>
		/// Converts the given object to a non-nullable DateTime, returning DateTime.MinValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object value) {
			return ToDateTime(value, DateTime.MinValue);
		}


		/// <summary>
		/// Converts the given string to a non-nullable int, returning int.MinValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToInt32(string value) {
			return ToInt32(value, int.MinValue);
		}

		/// <summary>
		/// Converts the given string to a non-nullable double, returning double.MinValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static double ToDouble(string value) {
			return ToDouble(value, double.MinValue);
		}

		/// <summary>
		/// Converts the given string to a non-nullable float, returning float.MinValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float ToFloat(string value) {
			return ToFloat(value, float.MinValue);
		}


        public static bool IsServiceInstalled(string serviceName) {
            try {
                using (var sc = new ServiceController(serviceName)) {
                    sc.Refresh();
                    // pulling the ServiceName property will throw an exception if it fails -- the actual value isn't what we're interested in
                    // (although the actual value must be non-empty to be valid)
                    return !String.IsNullOrEmpty(sc.ServiceName);
                }
            } catch (Exception ex) {
                Debug.WriteLine("Could not query service status for '" + serviceName + "': " + ex.Message);
                return false;
            }
        }


        public static bool IsServiceRunning(string serviceName) {
            try {
                using (var sc = new ServiceController(serviceName)) {
                    sc.Refresh();
                    return sc.Status == ServiceControllerStatus.Running;
                }
            } catch (Exception ex) {
                Debug.WriteLine("Could not query service status for '" + serviceName + "': " + ex.Message);
                return false;
            }
        }

        public static bool IsProcessElevated() {
            var rv = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            return rv;
        }

        /// <summary>
        /// Attempts to start the given service.  Allows 30 seconds for startup before timing out.
        /// </summary>
        /// <param name="serviceName"></param>
        public static void StartService(string serviceName) {
            using (ServiceController sc = new ServiceController(serviceName)) {
                sc.Refresh();
                if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending) {
                    sc.Start();
                }
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30.0f));
            }
        }

        /// <summary>
        /// Attempts to stop the given service.  Allows 30 seconds for service to shutdown before timing out.
        /// </summary>
        /// <param name="serviceName"></param>
        public static void StopService(string serviceName) {
            using (ServiceController sc = new ServiceController(serviceName)) {
                sc.Refresh();
                if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending) {
                    sc.Stop();
                }
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30.0f));
            }
        }


		/// <summary>
		/// Converts the given string to a non-nullable int, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static int ToInt32(string value, int defaultValue) {
			int ret;
			if (int.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a non-nullable float, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static float ToFloat(string value, float defaultValue) {
			float ret;
			if (float.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a non-nullable double, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static double ToDouble(string value, double defaultValue) {
			double ret;
			if (double.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}

		/// <summary>
		/// Converts the given string to a non-nullable decimal, returning the given defaultValue if it is inconvertible.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static decimal ToDecimal(string value, decimal defaultValue) {
			decimal ret;
			if (decimal.TryParse(value, out ret)) {
				return ret;
			} else {
				return defaultValue;
			}
		}


		#endregion

		#region File manipulations


        /// <summary>
        /// Will poll the local hard drives and removable drives for available freespace, and return a path under the first one which meets the minSizeInMB (e.g. D:\temp\gginst).  If minSizeInMB &lt; 1, returns %SYSTEMDRIVE%\temp\gginst (i.e. ignore space and just return the path on the HD Windows is installed on)
        /// </summary>
        /// <param name="minSizeInMB"></param>
        /// <returns></returns>
        [ComVisible(true)]
        public static string GetTempDirectory(int minSizeInMB) {
            // SQL Server installer does not play well with spaces in the path -- even if the path is in 8.3 format.
            // In XP the temp path may be part of the current user's Application Data path (C:\Documents and Settings\<username>\Application Data)
            // so we can't use the TEMP or TMP environment variables.
            // so we're punting and using a hardcoded path here because we have no other choice.
            // But we at least don't tie it to a specific drive.
            // this will work in Vista simply because the installer (and all msi's) run with elevated privileges.

            // default to using same HD as Windows
            var windowsDrive = Environment.GetEnvironmentVariable("SYSTEMDRIVE") ?? "C:";
            var drive = windowsDrive;

            var minBytes = ((long)minSizeInMB * 1024L * 1024L);

            if (minSizeInMB > 0) {
                try {
                    foreach (var di in DriveInfo.GetDrives()) {
                        // Optical drives will have IsReady = false if no disc is in the drive
                        if (di.IsReady) {
                            if (di.DriveType == DriveType.Fixed || di.DriveType == DriveType.Removable) {
                                // is a USB stick or a local hard drive.
                                if (di.AvailableFreeSpace > minBytes) {
                                    drive = di.Name;

                                    try {
                                        var tempPath = Toolkit.ResolveDirectoryPath(drive + @"\temp\gginst", true);
                                        if (CanWriteToFolder(tempPath)) {
                                            break;
                                        } else {
                                            // folder matches criteria, but we can't write to it.
                                            // continue checking other drives.
                                            drive = null;
                                        }
                                    } catch {
                                        // error trying to create the temp folder, assume no rights to do so and
                                        // continue checking other dirvers
                                        drive = null;
                                    }
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                    throw new InvalidOperationException(getDisplayMember("GetTempDirectory{enumfailed}", "Failed enumerating drives: {0}", ex.Message));
                }
            }

            if (String.IsNullOrEmpty(drive) || !Directory.Exists(drive)) {
                drive = windowsDrive;
            }

            var di2 = new DriveInfo(drive);
            if (di2.AvailableFreeSpace <= minBytes) {
                throw new InvalidOperationException(getDisplayMember("GetTempDirectory{toofull}", "Drive {0} does not have enough free space available (minimum is {1} MB).", drive, minSizeInMB.ToString()));
            }


            var tempDir = Toolkit.ResolveDirectoryPath(drive + @"\temp\gginst", true);
            // create a file to make sure we can write to this folder

            if (!CanWriteToFolder(tempDir)) {
                throw new InvalidOperationException(getDisplayMember("GetTempDirectory{cantwrite}", "Temporary path {0} is valid and has enough drive space, but is not writable.  Make sure you have access to write to that folder.", tempDir));
            }

            return tempDir;

        }

        public static bool CanWriteToFolder(string folderName) {
            var path = Toolkit.ResolveDirectoryPath(folderName, true);
            // create a file to make sure we can write to this folder

            try {
                var fn = path + Path.DirectorySeparatorChar.ToString() + Guid.NewGuid().ToString("N");
                Touch(fn);
                File.Delete(fn);
                return true;
            } catch {
                return false;
            }
        }


        /// <summary>
        /// Creates an empty file with the given file name (will auto-resolve filename if it is not an absolute path).
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Touch(string fileName) {
            var fn = Toolkit.ResolveFilePath(fileName, true);
            File.WriteAllBytes(fn, new byte[0] { });
            return fn;
        }

		/// <summary>
		/// Given a path to a directory, resolves it to an absolute path.  Creates directories as needed. Environment variables are resolved (e.g. "%SystemRoot%\ -> "C:\windows\") and special folders are resolved (e.g. "*ApplicationData*\" -> "C:\Documents and Settings\[current user]\Application Data\").
		/// </summary>
		/// <param name="directoryName">Root-relative, document-relative, or absolute path to a directory.  ** delimiters are used for "special" folders like - *Application Data*</param>
		/// <param name="createDirectoriesAsNeeded">If necessary, creates all folders up to root to make sure directory can be created.</param>
		/// <returns>Absolute path (parent directories guaranteed to be created) to a directory.</returns>
		[ComVisible(true)]
		public static string ResolveDirectoryPath(string directoryName, bool createDirectoriesAsNeeded) {

			directoryName = resolvePath(directoryName);

			if (String.IsNullOrEmpty(directoryName)) {
				return String.Empty;
			}

			if (!Directory.Exists(directoryName) && createDirectoriesAsNeeded) {
				Directory.CreateDirectory(directoryName);
			}

			return directoryName;
		}


        /// <summary>
        /// Given a path to a directory or file, resolves it to an absolute path, then bounces up one level and returns the parent directory.  Creates directories as needed. Environment variables are resolved (e.g. "%SystemRoot%\ -> "C:\windows\") and special folders are resolved (e.g. "*ApplicationData*\" -> "C:\Documents and Settings\[current user]\Application Data\").
        /// </summary>
        /// <param name="directoryName">Root-relative, document-relative, or absolute path to a directory.  ** delimiters are used for "special" folders like - *Application Data*</param>
        /// <param name="createDirectoriesAsNeeded">If necessary, creates all folders up to root to make sure directory can be created.</param>
        /// <returns>Absolute path (parent directories guaranteed to be created) to a directory.</returns>
        [ComVisible(true)]
        public static string ResolveParentDirectoryPath(string directoryOrFileName, bool createDirectoriesAsNeeded) {

            var dir = ResolveDirectoryPath(directoryOrFileName, createDirectoriesAsNeeded);
            return Directory.GetParent(dir).FullName;

        }


        /// <summary>
        /// Given a directory path, will make sure it is completely empty.  Does not drop then re-add directory (so permissions are not altered).
        /// </summary>
        /// <param name="directoryName"></param>
        [ComVisible(true)]
        public static void EmptyDirectory(string directoryName) {
            directoryName = ResolveDirectoryPath(directoryName, true);
            foreach (string dir in Directory.GetDirectories(directoryName)) {
                Directory.Delete(dir, true);
            }
            foreach (string fil in Directory.GetFiles(directoryName)) {
                File.Delete(fil);
            }
        }

		/// <summary>
		/// Given a path to a file, resolves it to an absolute path.  Creates directories as needed. Environment variables are resolved (e.g. "%SystemRoot%\joe.txt" -> "C:\windows\joe.txt") and special folders are resolved (e.g. "*ApplicationData*\fred.txt" -> "C:\Documents and Settings\[current user]\Application Data\fred.txt").
		/// </summary>
		/// <param name="fileName">Root-relative, document-relative, or absolute path to a file.  ** delimiters are used for "special" folders like - *Application Data*</param>
		/// <param name="createDirectoriesAsNeeded">If necessary, creates all folders up to root to make sure file can be created.</param>
		/// <returns>Absolute path (parent directories guaranteed to be created) to a file.  Does not create the file.</returns>
		[ComVisible(true)]
		public static string ResolveFilePath(string fileName, bool createDirectoriesAsNeeded) {

			fileName = resolvePath(fileName);

			if (String.IsNullOrEmpty(fileName)) {
				return String.Empty;
			}

			if (!File.Exists(fileName)) {
				FileInfo fi = new FileInfo(fileName);
				if (!Directory.Exists(fi.DirectoryName) && createDirectoriesAsNeeded) {
					Directory.CreateDirectory(fi.DirectoryName);
				}
			}

			return fileName;
		}

		private static string resolvePath(string path) {
			if (String.IsNullOrEmpty(path)) {
				// always return empty instead of null in case callers are doing string manipulation checks
				return String.Empty;
			}

			// resolve any environment variables
			path = Environment.ExpandEnvironmentVariables(path);

			// resolve any "special" folders
			if (path.Contains("*")) {
				// pluck out the ** portion
				int front = path.IndexOf("*");
				int back = path.LastIndexOf("*");
				if (front == back) {
					throw new InvalidOperationException(getDisplayMember("resolvePath{mismatchedasterisk}", "Badly formed path (missing corresponding * delimiter: '{0}'.", path));
				}

				string special = path.Substring(front + 1, back - 1).Replace(" ", "");
				special = Environment.GetFolderPath((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), special, true));

                //// yuck. Environment.GetFolderPath doesn't always report the correct user when
                //// we've launched a process as another user via pinvoke. Probably not
                //// loading the app domain properly or something, but I don't have time to
                //// research.  total hack here!
                //if (!special.Contains(Path.DirectorySeparatorChar + DomainUser.Current.UserName + Path.DirectorySeparatorChar)) {
                //    // it didn't resolve the current user properly.  Let's fudge it.
                //    string docsAndSettings = (Toolkit.IsWindowsVistaOrLater ? Path.DirectorySeparatorChar + @"Users" + Path.DirectorySeparatorChar : Path.DirectorySeparatorChar + @"Documents and Settings" + Path.DirectorySeparatorChar);
                //    int pos1Start = special.IndexOf(docsAndSettings);
                //    int pos1End = pos1Start + docsAndSettings.Length;
                //    int pos2 = special.IndexOf(Path.DirectorySeparatorChar, pos1End + 1);
                //    special = Toolkit.Cut(special, 0, pos1End) + DomainUser.Current.UserName + Toolkit.Cut(special, pos2);
                //}


				//#if DEBUG
				//                Logger.LogText("cur user=" + DomainUser.Current + "; special folder=" + special, false, true);
				//#endif

				path = path.Substring(0, front) + special + path.Substring(back + 1);
			}

			if (path.IndexOf("~") == 0) {
				path = AppDomain.CurrentDomain.BaseDirectory + path.Substring(1);
			}

			// resolve folders to base directory if not absolute
            if (path.IndexOf(@":" + Path.DirectorySeparatorChar) == -1 && path.IndexOf(Path.DirectorySeparatorChar) != 0) {
				path = AppDomain.CurrentDomain.BaseDirectory + path;
				FileInfo fi = new FileInfo(path);
                path = fi.Directory + Path.DirectorySeparatorChar.ToString() + fi.Name;
			}

            // to make sure the correct version of path separators exist, replace both '/' and '\' with the os-specific equivalent.
            path = path.Replace(@"\", Path.DirectorySeparatorChar.ToString()).Replace("/", Path.DirectorySeparatorChar.ToString());

			// resolve any relativity in the middle of the path
			path = Path.GetFullPath(path);

            //Console.WriteLine("resolved path=" + path);

			return path;
		}

		#endregion

		#region Threading

		/// <summary>
		/// Blocks caller's thread until another thread calls UnblockThread() or ThreadBlocker.Singleton.MaxWaitTime milliseconds expires, whichever comes first.
		/// </summary>
		public static void BlockThread() {
			ThreadBlocker.Singleton.Block();
		}

		/// <summary>
		/// Blocks caller's thread until another thread calls UnblockThread() or maxWaitTime milliseconds expires, whichever comes first.
		/// </summary>
		/// <param name="maxWaitTIme"></param>
		public static void BlockThread(int maxWaitTime) {
			ThreadBlocker.Singleton.Block(maxWaitTime);
		}

		/// <summary>
		/// Blocks caller's thread until given callback returns true.  Callback is invoked every maxIterationTime milliseconds or immediately when UnblockThread() is called.
		/// </summary>
		/// <param name="callback">Method that allows caller to process their code on each iteration.  Should return true to unblock the thread, false to block it again.</param>
		/// <param name="maxIterationTime">Number of milliseconds to wait for another thread to call UnblockThread() before calling the callback again.</param>
		public static void BlockThread(Toolkit.BoolCallback callback, int maxIterationTime) {
			ThreadBlocker.Singleton.BlockingLoop(callback, maxIterationTime);
		}

		/// <summary>
		/// Blocks caller's thread until given callback returns true.  Callback is invoked every ThreadBlocker.Singleton.MaxWaitTime milliseconds or immediately when UnblockThread() is called.
		/// </summary>
		/// <param name="callback"></param>
		public static void BlockThread(Toolkit.BoolCallback callback) {
			ThreadBlocker.Singleton.BlockingLoop(callback);
		}

		/// <summary>
		/// Tells all threads that are currently blocked that they can resume processing.  Applies to any BlockThread overload.
		/// </summary>
		public static void UnblockBlockedThread() {
			ThreadBlocker.Singleton.Unblock();
		}
		#endregion

		#region Registry

        public static void WriteFirewallException(string fullFilePath, string displayName, bool enabled) {
            string value = fullFilePath + ":*:" + (enabled ? "Enabled" : "Disabled") + ":" + displayName;
            Toolkit.SaveRegSetting(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\AuthorizedApplications\List", fullFilePath, value, true);
        }

        private static RegistryKey getBaseRegKey(ref string regPath) {
            var temp = regPath.ToLower();
            regPath = regPath.Substring(regPath.IndexOf('\\') + 1);
            if (temp.StartsWith("hklm") || temp.StartsWith("hkey_local_machine")) {
                return Registry.LocalMachine;
            } else if (temp.StartsWith("hkcu") || temp.StartsWith("hkey_current_user")){
                return Registry.CurrentUser;
            } else if (temp.StartsWith("hkcr") || temp.StartsWith("hkey_classes_root")){
                return Registry.ClassesRoot;
            } else if (temp.StartsWith("hkcc") || temp.StartsWith("hkey_current_config")){
                return Registry.CurrentConfig;
            } else if (temp.StartsWith("hkdd") || temp.StartsWith("hkey_dyn_data")){
                return Registry.DynData;
            } else if (temp.StartsWith("hkpd") || temp.StartsWith("hkey_performance_data")){
                return Registry.PerformanceData;
            } else if (temp.StartsWith("hku") || temp.StartsWith("hkey_users")) {
                return Registry.Users;
            } else {
                return Registry.LocalMachine;
            }
        }

        public static string[] GetRegSubKeys(string keyName) {

            string[] names = new string[] { };

            RegistryKey baseKey = getBaseRegKey(ref keyName);

            using (RegistryKey rk = baseKey.OpenSubKey(keyName)) {
                if (rk != null) {
                    names = rk.GetSubKeyNames();
                } else {
                    if (Toolkit.Is64BitOperatingSystem()) {
                        if (Toolkit.Is64BitProcess()) {
                            // running in a 64 bit environment, check the Wow6432Node if needed
                            if (keyName.ToLower().StartsWith(@"software\")) {
                                keyName = keyName.ToLower().Replace(@"software\", @"software\wow6432node\");

                                using (RegistryKey rk2 = baseKey.OpenSubKey(keyName)) {
                                    if (rk2 != null) {
                                        names = rk2.GetSubKeyNames();
                                    }
                                }

                            } else {
                                // we've already done the correct read and found nothing.
                            }

                        } else {
                            // running on a 64 bit OS, but process is 32 bit.
                            // we already looked up the value from the 32 bit section of the registry and found nothing.
                            // we need to shell out to a 64 bit process so it can check the 64 bit section of the registry
                            names = force64BitRegistryReadSubkeys(keyName);
                        }
                    } else {
                        // 32-bit OS, means process must also be 32-bit.
                        // some MS apps (aka SQL Server) put certain registry keys in the Wow6432Node even on a 32-bit OS.
                        // this is technically incorrect, but that's how SQL Server does it so we need to support it.
                        if (keyName.ToLower().StartsWith(@"software\")) {
                            keyName = keyName.ToLower().Replace(@"software\", @"software\wow6432node\");
                            using (RegistryKey rk3 = baseKey.OpenSubKey(keyName)) {
                                if (rk3 != null) {
                                    names = rk3.GetSubKeyNames();
                                }
                            }
                        }
                    }
                }
            }
            return names;
        }

		/// <summary>
		/// Gets given key/value, supplying default value if not found.  ALSO, in 64-bit environment, will check Wow6432Node as a secondary lookup if needed.
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="valueName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetRegSetting(string keyName, string valueName, string defaultValue) {

            var regPath = resolveRegName(keyName);
            string val = null;
            try {
                val = (string)Registry.GetValue(regPath, valueName, null);
                //            EventLog.WriteEntry("GRIN-Global", "Read reg key '" + keyName + @"\" + valueName + " = '" + val + "'.", EventLogEntryType.Information);
                if (val == null) {
                    // http://support.microsoft.com/kb/896459
                    if (Toolkit.Is64BitOperatingSystem()) {
                        if (Toolkit.Is64BitProcess()) {
                            // running in a 64 bit environment, check the Wow6432Node if needed
                            if (regPath.ToLower().Contains(@"\software\")) {
                                regPath = regPath.ToLower().Replace(@"\software\", @"\software\wow6432node\");
                            }
                            val = (string)Registry.GetValue(regPath, valueName, defaultValue);
                        } else {
                            // running on a 64 bit OS, but process is 32 bit.
                            // we already looked up the value from the 32 bit section of the registry and found nothing.
                            // we need to shell out to a 64 bit process so it can check the 64 bit section of the registry
                            val = force64BitRegistryRead(regPath, valueName, defaultValue, "string");
                        }
                    } else {

                        // 32-bit OS, means process must also be 32-bit.
                        // some MS apps (aka SQL Server) put certain registry keys in the Wow6432Node even on a 32-bit OS.
                        // this is technically incorrect, but that's how SQL Server does it so we need to support it.
                        if (regPath.ToLower().Contains(@"\software\")) {
                            regPath = regPath.ToLower().Replace(@"\software\", @"\software\wow6432node\");
                        }
                        val = (string)Registry.GetValue(regPath, valueName, defaultValue);
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine("Error reading registry: " + ex.Message);
            }
            return val;
        }

        private static void force64BitRegistryWrite(string regPath, string valueName, string value, string type) {
            var util64 = GetPathToUtility64Exe();
            if (File.Exists(util64)) {
                var val = Toolkit.ExecuteProcess(util64, @"--writeregkey """ + regPath + Path.DirectorySeparatorChar.ToString() + valueName + @""" --type " + type + @" --value """ + value + @"""", null);
            } else {
                throw new InvalidOperationException(getDisplayMember("force64BitRegistryWrite", "Could not find ggutil64.exe to force a write to the 64-bit section of the registry."));
            }

        }

        private static string force64BitRegistryRead(string regPath, string valueName, string defaultValue, string type) {
            var util64 = GetPathToUtility64Exe();
//            EventLog.WriteEntry("GRIN-Global", "Via ggutil64.exe, reading reg key " + regPath + @"\" + valueName + "...", EventLogEntryType.Information);
            if (File.Exists(util64)) {
                var val = Toolkit.ExecuteProcess(util64, @"--readregkey """ + regPath + Path.DirectorySeparatorChar.ToString() + valueName + @""" --type " + type + @" --value " + defaultValue, null);
                if (!String.IsNullOrEmpty(val)) {
//                    EventLog.WriteEntry("GRIN-Global", "Via ggutil64.exe, found reg key " + regPath + @"\" + valueName + " = '" + val + "'", EventLogEntryType.Information);
                    return val;
                }
            } else {
//                EventLog.WriteEntry("GRIN-Global", "Did not find ggutil64.exe at path='" + util64 + "'.", EventLogEntryType.Information);
            }

//            EventLog.WriteEntry("GRIN-Global", "Returning default value for regkey '" + regPath + @"\" + valueName + "' = '" + defaultValue + "'", EventLogEntryType.Information);
            return defaultValue;
        }

        private static string[] force64BitRegistryReadSubkeys(string keyname) {
            var util64 = GetPathToUtility64Exe();
            if (File.Exists(util64)) {
                var val = Toolkit.ExecuteProcess(util64, @"--readregsubkeys """ + keyname, null).Split(new string[]{", "}, StringSplitOptions.RemoveEmptyEntries);
                return val;
            } else {
                return new string[] { };
            }
        }

        public static string GetPathToUtility64Exe() {
            return getPathToExe("ggutil64.exe");
        }

        public static string GetPathToUACExe(){
            return getPathToExe("gguac.exe");
        }

        public static string GetPathToAdminExe() {
            return getPathToExe("GrinGlobal.Admin.exe");
        }

        private static string getPathToExe(string exeName){

            // since this method may be trying to resolve ggutil64.exe, we just want to call Registry.GetValue() instead of Toolkit.GetRegSetting()
            // so we don't potentially cause an infinite loop.  This means we almost must manually check the Wow6432Node for 64-bit friendliness.

            var p = Toolkit.ResolveFilePath("~/" + exeName, false);
            if (!File.Exists(p)) {
                // try to pull 
                p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global\Database", "InstallPath", "") as string + @"\" + exeName, false);
                if (!File.Exists(p)) {
                    p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\GRIN-Global\Database", "InstallPath", "") as string + @"\" + exeName, false);
                    if (!File.Exists(p)) {
                        p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global\Search Engine", "InstallPath", "") as string + @"\" + exeName, false);
                        if (!File.Exists(p)) {
                            p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\GRIN-Global\Search Engine", "InstallPath", "") as string + @"\" + exeName, false);
                            if (!File.Exists(p)) {
                                p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global\Updater", "InstallPath", "") as string + @"\" + exeName, false);
                                if (!File.Exists(p)) {
                                    p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\GRIN-Global\Updater", "InstallPath", "") as string + @"\" + exeName, false);
                                    if (!File.Exists(p)) {
                                        p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\GRIN-Global\Admin", "InstallPath", "") as string + @"\" + exeName, false);
                                        if (!File.Exists(p)) {
                                            p = Toolkit.ResolveFilePath(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\GRIN-Global\Admin", "InstallPath", "") as string + @"\" + exeName, false);
                                            if (!File.Exists(p)) {
                                                p = "";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return p;
        }

        /// <summary>
        /// Gets given key/value, supplying default value if not found.  ALSO, in 64-bit environment, will check Wow6432Node as a secondary lookup if needed.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string[] GetRegSetting(string keyName, string valueName, string[] defaultValue) {

            var regPath = resolveRegName(keyName);

            var val = (string[])Registry.GetValue(regPath, valueName, null);
            if (val == null) {

                // http://support.microsoft.com/kb/896459
                if (Toolkit.Is64BitOperatingSystem()) {
                    if (Toolkit.Is64BitProcess()) {
                        // running in a 64 bit environment, check the Wow6432Node if needed
                        if (regPath.ToLower().Contains(@"\software\")) {
                            regPath = regPath.ToLower().Replace(@"\software\", @"\software\wow6432node\");
                        }
                        val = (string[])Registry.GetValue(regPath, valueName, defaultValue);
                    } else {
                        // running on a 64 bit OS, but process is 32 bit.
                        val = force64BitRegistryRead(regPath, valueName, Toolkit.Join(defaultValue, ", ", null), "string[]").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    }
                } else {
                    val = defaultValue;
                }
            }
            return val;
        }

		/// <summary>
        /// Gets given key/value, supplying default value if not found.  ALSO, in 64-bit environment, will check Wow6432Node as a secondary lookup if needed.
        /// </summary>
		/// <param name="keyName"></param>
		/// <param name="valueName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static int GetRegSetting(string keyName, string valueName, int defaultValue) {


            var regPath = resolveRegName(keyName);

            var val = Toolkit.ToInt32(Registry.GetValue(regPath, valueName, int.MinValue), int.MinValue);
            if (val == int.MinValue) {
                // http://support.microsoft.com/kb/896459
                if (Toolkit.Is64BitOperatingSystem()) {
                    if (Toolkit.Is64BitProcess()) {
                        // running in a 64 bit environment, check the Wow6432Node if needed
                        if (regPath.ToLower().Contains(@"\software\")) {
                            regPath = regPath.ToLower().Replace(@"\software\", @"\software\wow6432node\");
                        }
                        val = Toolkit.ToInt32(Registry.GetValue(regPath, valueName, defaultValue), defaultValue);
                    } else {
                        // running on a 64 bit OS, but process is 32 bit.
                        val = Toolkit.ToInt32(force64BitRegistryRead(regPath, valueName, defaultValue.ToString(), "int"), defaultValue);
                    }
                } else {
                    val = defaultValue;
                }
            }
            return val;
            
            
            //object val = Registry.GetValue(resolveRegName(keyName), valueName, defaultValue);
            //if (val == null) {
            //    return defaultValue;
            //} else {
            //    return (int)val;
            //}
		}

		/// <summary>
		/// Saves the given string as a String value in the registry at the specified keyname / valuename.
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="valueName"></param>
		/// <param name="value"></param>
		[ComVisible(true)]
		public static void SaveRegSetting(string keyName, string valueName, string value, bool forceWriteTo64BitRegistry) {
            if (forceWriteTo64BitRegistry) {
                if (Toolkit.Is64BitOperatingSystem() && !Toolkit.Is64BitProcess()) {
                    force64BitRegistryWrite(resolveRegName(keyName), valueName, value, "string");
                } else {
                    Registry.SetValue(resolveRegName(keyName), valueName, value, RegistryValueKind.String);
                }
            } else {
                Registry.SetValue(resolveRegName(keyName), valueName, value, RegistryValueKind.String);
            }
		}

		/// <summary>
		/// Saves the given int as a DWord value in the registry at the specified keyname / valuename
		/// </summary>
		/// <param name="keyName"></param>
		/// <param name="valueName"></param>
		/// <param name="value"></param>
		[ComVisible(true)]
        public static void SaveRegSetting(string keyName, string valueName, int value, bool forceWriteTo64BitRegistry) {
            if (forceWriteTo64BitRegistry) {
                if (Toolkit.Is64BitOperatingSystem() && !Toolkit.Is64BitProcess()) {
                    force64BitRegistryWrite(resolveRegName(keyName), valueName, value.ToString(), "int");
                } else {
                    Registry.SetValue(resolveRegName(keyName), valueName, value, RegistryValueKind.DWord);
                }
            } else {
                Registry.SetValue(resolveRegName(keyName), valueName, value, RegistryValueKind.DWord);
            }
		}

		private static string resolveRegName(string keyName) {
			string temp = keyName.Replace("HKCU", "HKEY_CURRENT_USER");
			temp = temp.Replace("HKLM", "HKEY_LOCAL_MACHINE");
			temp = temp.Replace("HKCR", "HKEY_CLASSES_ROOT");
			temp = temp.Replace("HKU", "HKEY_USERS");
			temp = temp.Replace("HKCC", "HKEY_CURRENT_CONFIG");
			temp = temp.Replace("HKDD", "HKEY_DYN_DATA");
			temp = temp.Replace("HKPD", "HKEY_PERFORMANCE_DATA");
			return temp;
		}
		#endregion

		#region User and Process Info

		public static object LoadClass(Type type) {
			return Activator.CreateInstance(type);
		}

		/// <summary>
		/// Creates an instance of the given class.  That class must be in the same namespace as the caller's namespace.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public static object LoadClass(string className) {
			// get the caller's type via the stackframe
			StackFrame sf = new StackFrame(1);
			Type t = sf.GetMethod().ReflectedType;
			Assembly asm = Assembly.GetCallingAssembly();
			return asm.CreateInstance(t.Namespace + "." + className, true);
		}

		/// <summary>
		/// Loads the given class based on the className and from the namespace described by the type.
		/// </summary>
		/// <param name="typeOfClassInSameNamespace">Can be the same class as className parameter, but typically is another class in the same namespace.  e.g. typeof(YourClass2)</param>
		/// <param name="className">e.g. "YourClass1"</param>
		/// <returns></returns>
		public static object LoadClass(Type typeOfClassInSameNamespace, string className) {
			Assembly asm = Assembly.GetCallingAssembly();
			return asm.CreateInstance(typeOfClassInSameNamespace.Namespace + "." + className, true);
		}


		/// <summary>
		/// Returns true if this instance of the application is the only process currently running for the given user.  User defaults to current user if passed as null.
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool IsOnlyInstanceOfApplicationForUser(DomainUser user) {
			if (user == null) {
				user = DomainUser.Current;
			}
			string appName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;
			List<int> pids = Toolkit.GetProcessIds(appName, DomainUser.Current);
			return pids.Count < 2;
		}

		/// <summary>
		/// Minimizes and hides the window of the current application. (Also causes Task manager to display an apparently smaller memory footprint)
		/// </summary>
		/// <returns></returns>
		public static bool HideWindow() {
			return ShowWindow(Process.GetCurrentProcess().MainWindowHandle, (int)(SHOWWINDOW.SW_HIDE | SHOWWINDOW.SW_MINIMIZE));
		}

        /// <summary>
        /// Minimizes given window handle to the task bar
        /// </summary>
        /// <param name="hWnd"></param>
        public static void MinimizeWindow(int hWnd) {
            try {
                ShowWindow(new IntPtr(hWnd), (int)(SHOWWINDOW.SW_MINIMIZE));
            } catch {
            }
        }

        /// <summary>
        /// Restores given window handle to the screen
        /// </summary>
        /// <param name="hWnd"></param>
        public static void RestoreWindow(int hWnd) {
            try {
                ShowWindow(new IntPtr(hWnd), (int)(SHOWWINDOW.SW_RESTORE));
            } catch {
            }
        }

        public static int GetWindowHandle(string titleBarText) {
            try {
                foreach (var p in Process.GetProcesses()) {
                    if (p.MainWindowTitle.ToLower() == titleBarText.ToLower()) {
                        return p.MainWindowHandle.ToInt32();
                    }
                }
            } catch {
            }
            return 0;
        }

		/// <summary>
		/// Hides the window with the given hWnd.  Does NOT close it, just hides it.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		public static bool HideWindow(int hWnd) {
            try {
                return ShowWindow(new IntPtr(hWnd), (int)(SHOWWINDOW.SW_HIDE));
            } catch {
                return false;
            }
		}

		/// <summary>
		/// Closes the window with the given hWnd.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="async">If true, Posts message to the given window.  Otherwise Sends message.</param>
		/// <returns></returns>
		public static void CloseWindow(int hWnd, bool async) {
            try {
                if (async) {
                    PostMessageSafe(new HandleRef(new object(), new IntPtr(hWnd)), WindowsMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                } else {
                    SendMessage(new HandleRef(new object(), new IntPtr(hWnd)), WindowsMessages.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            } catch { }
		}


		/// <summary>
		/// Returns all users currently logged into the system, assuming they have at least one executable running (like explorer.exe or rundll32.exe).
		/// </summary>
		/// <param name="excludeSystemUsers"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static List<DomainUser> GetLoggedInUsers(bool excludeSystemUsers) {
			return GetProcessOwners(null, excludeSystemUsers);
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindowsXP {
            get {
                return Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindowsVista {
            get {
                return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindowsVistaOrLater {
            get {
                return Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindows7 {
            get {
                return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsNonWindowsOS {
            get {
                return Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindowsVistaHome {
            get {
                var edition = WindowsVistaEdition.ToLower();
                return edition.Contains("home") && edition.Contains("premium");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindowsVistaUltimate {
            get {
                return WindowsVistaEdition.ToLower() == "ultimate";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static bool IsWindowsVistaBusiness {
            get {
                return WindowsVistaEdition.ToLower() == "business";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [ComVisible(true)]
        public static string WindowsVistaEdition {
            get {
                return Toolkit.GetRegSetting(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "") as string;
            }
        }



		/// <summary>
		/// Launches a process, waits for it to complete, and returns the output as a string
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="args"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static string ExecuteProcess(string fullName, string args, DomainUser user) {
			string output = null;
			Process p = launch(fullName, args, true, user, true, true, ref output);

			return output;
		}

		/// <summary>
		/// Launches a process and optionally waits for it to complete.
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="waitForExit"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static Process LaunchProcess(string fullName, bool waitForExit) {
			return LaunchProcess(fullName, null, null, false, waitForExit);
		}

		/// <summary>
		/// Launches a process and optionally waits for it to complete executing.
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="arguments"></param>
		/// <param name="waitForExit"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static Process LaunchProcess(string fullName, string arguments, bool waitForExit) {
			return LaunchProcess(fullName, arguments, null, false, waitForExit);
		}

		/// <summary>
		/// Launches a process and optionally waits for it to finish.  Specifiy null DomainUser to launch under current user's account.  Password for user is not required.
		/// </summary>
		/// <param name="fullName"></param>
		/// <param name="arguments"></param>
		/// <param name="user"></param>
		/// <param name="hideWindow"></param>
		/// <param name="waitForExit"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static Process LaunchProcess(string fullName, string arguments, DomainUser user, bool hideWindow, bool waitForExit) {
			string junk = null;
			return launch(fullName, arguments, hideWindow, user, waitForExit, false, ref junk);
		}

		private static Process launch(string fullName, string arguments, bool hideWindow, DomainUser user, bool waitForExit, bool returnOutput, ref string output) {

			fullName = Toolkit.ResolveFilePath(fullName, false);

            Process p = null;

			if (DomainUser.IsNullOrEmpty(user) || user == DomainUser.Current) {
				// launch as current user
				p = new Process();
				p.StartInfo = new ProcessStartInfo(fullName, arguments);
				if (hideWindow) {
					p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.CreateNoWindow = true;
				}

				if (returnOutput) {
					p.StartInfo.RedirectStandardOutput = true;
					p.StartInfo.RedirectStandardError = true;
					p.StartInfo.UseShellExecute = false;
				}
				p.StartInfo.WorkingDirectory = Directory.GetParent(fullName).FullName;

				p.Start();

				if (waitForExit) {
					if (returnOutput) {
						// to avoid a deadlock, we have to read to end of standard output before waiting for exit!
						output = p.StandardOutput.ReadToEnd();
						if (String.IsNullOrEmpty(output)) {
							output = p.StandardError.ReadToEnd();
						}
					}
					p.WaitForExit();
				}

				return p;
			} else if (!String.IsNullOrEmpty(user.Password)) {
				// launch as a different user, but still use normal .Net launching
				p = new Process();
				p.StartInfo = new ProcessStartInfo(fullName, arguments);
				if (hideWindow) {
					p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.CreateNoWindow = true;
                }
				if (returnOutput) {
					p.StartInfo.RedirectStandardOutput = true;
					p.StartInfo.RedirectStandardError = true;
					p.StartInfo.UseShellExecute = false;
				}
				p.StartInfo.Domain = user.Domain;
				p.StartInfo.UserName = user.UserName;
				p.StartInfo.LoadUserProfile = true;
				SecureString ss = new SecureString();
				foreach (char c in user.Password) {
					ss.AppendChar(c);
				}
				p.StartInfo.Password = ss;

				p.StartInfo.UseShellExecute = false;
				p.StartInfo.WorkingDirectory = Directory.GetParent(fullName).FullName;

				p.Start();

			} else {
				// user is specified, but no password given.
				// use API tricks to accomplish this!

				int pid = pinvokeProcess(user, fullName, arguments, hideWindow);

                p = Toolkit.GetProcessById(pid);

                // we can't return data from a pinvoked process, but we can wait for exit.
                if (waitForExit) {
                    p.WaitForExit();
                }
                return p;

			}

			if (waitForExit && p != null) {
				if (returnOutput) {
					// to avoid a deadlock, we have to read to end of standard output before waiting for exit!
					output = p.StandardOutput.ReadToEnd();
					if (String.IsNullOrEmpty(output)) {
						output = p.StandardError.ReadToEnd();
					}
				}
				p.WaitForExit();
			}
			return p;

		}

		private static int pinvokeProcess(DomainUser user, string fullName, string arguments, bool hideWindow) {
			List<int> pids = GetProcessIds(null, user);
			if (pids.Count == 0) {
				// no processes exist in memory from which to obtain an access token for the given user.
				throw new InvalidOperationException(getDisplayMember("pinvokeProcess{missingpid}", "Toolkit.pinvokeProcess requires the target user to have a process executing from which to impersonate the user's access token.  There are no processes currently running for '{0}', so pinvokeProcess cannot execute code as that user.", user.ToString()));
			} else {

				int newPid = int.MinValue;

				// we have a valid pid for the user.
				// launch a new process under that user's security context
				IntPtr processHandle = Process.GetProcessById(pids[0]).Handle;
				IntPtr impersonationToken = IntPtr.Zero;
				IntPtr primaryToken = IntPtr.Zero;
				try {

					if (!OpenProcessToken(processHandle, TokenAccess.TokenAllAccess, out impersonationToken)) {
						int hr = Marshal.GetHRForLastWin32Error();
                        Exception ex = Marshal.GetExceptionForHR(hr);
						throw new InvalidOperationException(getDisplayMember("pinvokeProcess{opentokenfailed}", "Could not obtain impersonation token for user '{0}' on pid={1}. Error: {2}", user.ToString(), pids[0].ToString(), ex.Message), ex);
					}

					SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
					if (!DuplicateTokenEx(impersonationToken, 0x10000000, ref sa, SecurityImpersonationLevel.SecurityImpersonation, TokenType.TokenPrimary, out primaryToken)) {
						int hr = Marshal.GetHRForLastWin32Error();
                        Exception ex = Marshal.GetExceptionForHR(hr);
						throw new InvalidOperationException(getDisplayMember("pinvokeProcess{duplicatetokenfailed}", "Could not create a primary access token from the process token for user '{0}' on pid={1}. Error: {2}", user.ToString(), pids[0].ToString(), ex.Message), ex);
					}

					// primaryToken now contains the necessary data to give to CreateProcessAsUser.

					PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
					STARTUPINFO si = new STARTUPINFO();
					si.mb = Marshal.SizeOf(si);
					if (hideWindow) {
						// tell the application that we are setting the window display 
						si.dwFlags = (int)STARTF.STARTF_USESHOWWINDOW;
						si.wShowWindow = (int)SHOWWINDOW.SW_HIDE;
						//						si.wShowWindow = (int)(SHOWWINDOW.SW_HIDE | SHOWWINDOW.SW_MINIMIZE);
					}

					SECURITY_ATTRIBUTES saProcess = new SECURITY_ATTRIBUTES();
					SECURITY_ATTRIBUTES saThread = new SECURITY_ATTRIBUTES();

					if (!CreateProcessAsUser(primaryToken, fullName, new StringBuilder(arguments), ref saProcess, ref saThread, false, 0, IntPtr.Zero, Directory.GetParent(fullName).FullName, ref si, out pi)) {
						//						int errCode = Marshal.GetLastWin32Error();
						int hr = Marshal.GetHRForLastWin32Error();
                        Exception ex = Marshal.GetExceptionForHR(hr);
                        throw new InvalidOperationException(getDisplayMember("pinvokeProcess{createprocessfailed}", "Could not create process as user. {0} {1}. Error: {2}", fullName, arguments, ex.Message), ex);
					}

					// if we get here, we launched the process. return the pid.
					newPid = pi.dwProcessId;


				} catch (ManagementException me) {
					string info = me.Message + ":" + (me.ErrorInformation != null ? me.ErrorInformation["Description"] : null) + ":" + me.ToString();
					Logger.LogText("pinvokeProcess failed: " + info);
					throw;
				} catch (Exception ex1) {
					// close the token handle
					Logger.LogText("pinvokeProcess failed: " + ex1.ToString());
					throw;
				} finally {
					if (primaryToken != IntPtr.Zero) {
						CloseHandle(primaryToken);
					}
					if (impersonationToken != IntPtr.Zero) {
						CloseHandle(impersonationToken);
					}
				}


				return newPid;
			}
		}

		private static object impersonateViaCredentials(DomainUser user, ObjectCallback callback) {

			object output = null;

			// we have a valid pid for the user.
			// switch to that user's security context
			IntPtr tokenHandle = IntPtr.Zero;
			IntPtr tokenDuplicate = IntPtr.Zero;
			try {

				LogonUser(user.UserName, user.Domain, user.Password, LOGON_TYPE.LOGON32_LOGON_INTERACTIVE, LOGON_PROVIDER.LOGON32_PROVIDER_DEFAULT, out tokenHandle);

				if (!DuplicateToken(tokenHandle, SecurityImpersonationLevel.SecurityImpersonation, out tokenDuplicate)) {
					int hr = Marshal.GetHRForLastWin32Error();
					Marshal.ThrowExceptionForHR(hr);
				}


				WindowsImpersonationContext wic = null;
				try {

					wic = WindowsIdentity.Impersonate(tokenDuplicate);
					// Run the user's code 
					output = callback();

				} finally {
					if (wic != null) {
						wic.Undo();
						wic.Dispose();
					}
				}

			} catch (ManagementException me) {
				string info = me.Message + ":" + (me.ErrorInformation != null ? me.ErrorInformation["Description"] : null) + ":" + me.ToString();
				Logger.LogText("RunAs impersonating via credentials failed: " + info);

			} catch (Exception ex1) {
				// close the token handle
				Logger.LogText("RunAs impersonating via credentials failed: " + ex1.ToString());
				throw;
			} finally {
				if (tokenDuplicate != IntPtr.Zero) {
					CloseHandle(tokenDuplicate);
				}
				if (tokenHandle != IntPtr.Zero) {
					CloseHandle(tokenHandle);
				}
			}


			return output;
		}
		private static object impersonateViaProcess(int pid, ObjectCallback callback) {
			object output = null;

			// we have a valid pid for the user.
			// switch to that user's security context
			IntPtr tokenHandle = IntPtr.Zero;
			IntPtr processHandle = Process.GetProcessById(pid).Handle;
			try {

				if (!OpenProcessToken(processHandle, TokenAccess.TokenAllAccess, out tokenHandle)) {
					int hr = Marshal.GetHRForLastWin32Error();
					Marshal.ThrowExceptionForHR(hr);
					//						throw new InvalidOperationException("Could not obtain impersonation token for user '" + user + "' on pid=" + pids[0]);
				}

				WindowsImpersonationContext wic = null;
				try {

					wic = WindowsIdentity.Impersonate(tokenHandle);

					// Run the user's code 
					output = callback();


				} finally {
					if (wic != null) {
						wic.Undo();
						wic.Dispose();
					}
				}

			} catch (ManagementException me) {
				string info = me.Message + ":" + (me.ErrorInformation != null ? me.ErrorInformation["Description"] : null) + ":" + me.ToString();
				Logger.LogText("RunAs failed: " + info);

			} catch (Exception ex1) {
				// close the token handle
				Logger.LogText("RunAs failed: " + ex1.ToString());
				throw;
			} finally {
				if (tokenHandle != IntPtr.Zero) {
					CloseHandle(tokenHandle);
				}
			}


			return output;
		}

		/// <summary>
		/// Runs the given callback in the user space of the given user. Returns to default security context after the callback has returned or thrown an exception.  Requires the target user to have at least one process executing from which it can copy the access token and impersonate that user.  The calling code must have TOKEN_IMPERSONATE access (i.e. be LocalSystem)
		/// </summary>
		/// <param name="user"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static object RunAs(DomainUser user, ObjectCallback callback) {

			if (callback == null) {
				// don't even bother running as someone when there's nothing to do
				return null;
			}


			// given the domain\user, run the given callback under their credentials

			// null user, default to "current" user
			if (DomainUser.IsNullOrEmpty(user)) {
				// no user specified, assume current user
				user = DomainUser.Current;
			}

			if (!String.IsNullOrEmpty(user.Password)) {
				return impersonateViaCredentials(user, callback);
			} else {
				List<int> pids = GetProcessIds(null, user);
				if (pids.Count == 0) {
					// no processes exist in memory from which to obtain an access token for the given user.
					throw new InvalidOperationException(getDisplayMember("RunAs", "If the password is not specified for the DomainUser object, Toolkit.RunAs requires the target user to have a process executing from which to impersonate the user's access token.  There are no processes currently running for '{0}', so RunAs cannot execute code as that user.", user.ToString()));
				} else {
					return impersonateViaProcess(pids[0], callback);
				}
			}
		}

		/// <summary>
		/// Kills all the processes with the given name for the given user.  If process name is null, gets all processes for given user.  If user is null, assumes DomainUser.Current.
		/// </summary>
		/// <param name="processName"></param>
		/// <param name="owner"></param>
		/// <param name="tryNicelyFirst">If true, this method will post a WM_CLOSE message to the process.  If this fails, process.Kill() is called.  If false, process.Kill() is called.</param>
		/// <param name="flagPath">Resolved path where app looks for flag file(s).</param>
		[ComVisible(true)]
		public static void KillProcesses(string processName, DomainUser owner, bool tryNicelyFirst, string flagPath) {
			List<int> pids = GetProcessIds(processName, owner);
			foreach (int pid in pids) {
				KillProcess(pid, tryNicelyFirst, flagPath);
			}
		}

		/// <summary>
		/// Kills the process with the given pid.  Optionally sends a request to the app to shut itself down first with a 10 second grace period to fulfill the request before killing it forcefully.
		/// </summary>
		/// <param name="processId"></param>
		/// <param name="tryNicelyFirst">If true, this method will tell the app to shut itself down and give 10 seconds to comply, killing it forcefully after 10 seconds if the request is not met.  If false, simply kills it without requesting it to shut itself down.</param>
		/// <param name="flagPath">Resolved path where app looks for flag file(s).  If given, will create an empty file named "kill" in this path, which the process to kill should be designed to interpret and delete after use.</param>
		[ComVisible(true)]
		public static void KillProcess(int processId, bool tryNicelyFirst, string flagPath) {
			Process p = GetProcessById(processId);
			if (p != null) {
				if (tryNicelyFirst) {
					try {

						// tell app to shut itself down
						p.CloseMainWindow();

						// if flag path is given, create a kill file there.
						// this assumes the process is able to interpret the
						// kill file since the config is saying it can.
						if (!String.IsNullOrEmpty(flagPath)) {
							File.WriteAllText(flagPath + @"\kill", "");
						}

						// allow up to 10 seconds of grace period for
						// app to shutdown before we kill it.
						int i = 0;
						while (!p.HasExited && i < 10) {
							Thread.Sleep(1000);
							i++;
							p.Refresh();
						}

						// app not shutdown yet.  kill it
						if (!p.HasExited) {
							p.Kill();
						}
					} catch (Exception ex) {

						throw new InvalidOperationException(getDisplayMember("KillProcess{ex}", "Could not kill process {0}: {1}", processId.ToString(), ex.Message), ex);

						//try {
						//    if (p != null && !p.HasExited) {
						//        p.Kill();
						//    }
						//} catch {
						//    // eat any errors when we try the last-ditch Kill() call.
						//}
					}
				} else {
					// force an app to go bye-bye
					p.Kill();
				}
			}
		}

		/// <summary>
		/// Returns process by given pid.  If it does not exist, returns null.  Does not throw exceptions.
		/// </summary>
		/// <param name="pid"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static Process GetProcessById(int pid) {
			try {
				return Process.GetProcessById(pid);
			} catch (Exception ex) {
				Debug.WriteLine(ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// Gets the first process id for the given process name and domain / user
		/// </summary>
		/// <param name="processName"></param>
		/// <param name="domain"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static int GetProcessId(string processName, string domain, string userName) {
			return GetProcessId(processName, new DomainUser(domain, userName));
		}

		private static bool PostMessageSafe(HandleRef hWnd, WindowsMessages msg, IntPtr wParam, IntPtr lParam) {
			if (!PostMessage(hWnd, msg, wParam, lParam)) {
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}

			return true;
		}


		/// <summary>
		/// Gets the PID of the first process with given name for the given user
		/// </summary>
		/// <param name="processName"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static int GetProcessId(string processName, DomainUser user) {
			List<int> pids = GetProcessIds(processName, user);
			if (pids.Count > 0) {
				return pids[0];
			} else {
				return -1;
			}
		}

		/// <summary>
		/// Gets all the PIDs of the processes with given name running under any user.  If processName is null, returns all processes.
		/// </summary>
		/// <param name="processName"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static List<int> GetProcessIds(string processName) {
			return GetProcessIds(processName, null);
		}

        /// <summary>
        /// Gets the physical path the given web app corresponds to.  e.g. C:\inetpub\wwwroot\gringlobal  (no trailing backslash)
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string GetIISPhysicalPath(string appName) {
            try {
                DirectoryEntry iis = new DirectoryEntry("IIS://localhost/W3SVC");
                foreach (DirectoryEntry index in iis.Children) {
                    if (index.SchemaClassName == "IIsWebServer") {
                        int id = Convert.ToInt32(index.Name);
                        using (var site = new DirectoryEntry("IIS://localhost/W3SVC/" + id)) {
                            string siteName = site.Properties["ServerComment"].Value.ToString();
                            var dePath = "IIS://localhost/W3SVC/" + id + "/Root/" + appName;
                            using (var rootVDir = new DirectoryEntry(dePath)) {
                                try {
                                    string physicalPath = rootVDir.Properties["Path"].Value + "";
                                    return physicalPath;
                                } catch (COMException com) {
                                    // COMException means that path doesn't exist (i.e. virtual folder must be under a different web site on this machine)
                                    Debug.WriteLine("Error retrieving directory entry for '" + dePath + "': " + com.Message);
                                }
                            }
                        }
                    }
                }
            } catch {
                // cannot contact IIS... what to do?
            }

            // we get here, we couldn't determine the path to the website automatically. Look in config file...
            return Toolkit.GetSetting("PhysicalPathToWebsite", "");

        }

		/// <summary>
		/// Gets all the PIDs of the processes with given name for the given user.  If processName is null, returns all processes for that user.
		/// </summary>
		/// <param name="processName"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		[ComVisible(true)]
		public static List<int> GetProcessIds(string processName, DomainUser user) {

			//Process[] ps = Process.GetProcessesByName(processName);
			//List<int> ret = new List<int>();
			//foreach (Process p in ps) {
			//    if (p.StartInfo.UserName == user.UserName) {
			//        ret.Add(p.Id);
			//    }
			//}



			List<int> pids = new List<int>();
			SelectQuery selectQuery = new SelectQuery("select * from Win32_Process " + (processName == null ? "" : " where name='" + processName + "'"));

			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery)) {
				using (ManagementObjectCollection processes = searcher.Get()) {
					foreach (ManagementObject process in processes) {
						using (process) {
							//out argument to return user and domain
							string[] s = new String[2];
							//Invoke the GetOwner method and populate the array with the user name and domain
							process.InvokeMethod("GetOwner", (object[])s);
							if (user == null || user == new DomainUser(s[1], s[0])) {
								// this process is the first one running under the user id we're interested in.
								pids.Add(Toolkit.ToInt32(process["ProcessId"].ToString(), -1));
							}
						}
					}
				}
			}
			return pids;
		}

        [ComVisible(true)]
        public static DomainUser GetDomainUser(string processName, bool ignoreSystemUsers) {
            SelectQuery selectQuery = new SelectQuery("select * from Win32_Process where name='" + processName + "'");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery)) {
                using (ManagementObjectCollection processes = searcher.Get()) {
                    foreach (ManagementObject process in processes) {
                        using (process) {
                            //out argument to return user and domain
                            string[] s = new String[2];
                            //Invoke the GetOwner method and populate the array with the user name and domain
                            process.InvokeMethod("GetOwner", (object[])s);
                            DomainUser du = new DomainUser(s[1], s[0]);
                            if (ignoreSystemUsers && DomainUser.IsSystemAccount(du)) {
                                // keep looking
                            } else {
                                return du;
                            }
                        }
                    }
                }
            }
            return null;
        }


		/// <summary>
		/// Returns the domain\user associated with the first instance found of the given process name
		/// </summary>
		/// <param name="processName">file name of process to inspect, including extension</param>
		/// <param name="excludeSystemUsers">excludes users 'SYSTEM', 'LOCAL SERVICE', and 'NETWORK SERVICE'</param>
		/// <returns></returns>
		[ComVisible(true)]
		public static List<DomainUser> GetProcessOwners(string processName, bool excludeSystemUsers) {

			//Process[] ps = Process.GetProcesses();
			//foreach (Process p in ps) {
			//    IntPtr accessToken = IntPtr.Zero;
			//    if (OpenProcessToken(p.Handle, TokenAccess.TokenQuery, out accessToken)) {
			//        int tokenInfoLength = 0;

			//        // first get token length
			//        GetTokenInformation(accessToken, TOKEN_INFORMATION_CLASS.TokenOwner, IntPtr.Zero, tokenInfoLength, out tokenInfoLength);
			//        IntPtr tokenInfo = Marshal.AllocHGlobal(tokenInfoLength);
			//        if (GetTokenInformation(accessToken, TOKEN_INFORMATION_CLASS.TokenOwner, tokenInfo, tokenInfoLength, out tokenInfoLength)){

			//        }
			//        Marshal.FreeHGlobal(tokenInfo);
			//    }

			//}
			List<DomainUser> users = new List<DomainUser>();
			SelectQuery selectQuery = new SelectQuery("select * from Win32_Process " + (processName == null ? "" : " where name='" + processName + "'"));
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery)) {
				using (ManagementObjectCollection processes = searcher.Get()) {
					foreach (ManagementObject process in processes) {
						using (process) {
							if (Toolkit.ToInt32(process["Handle"].ToString(), 0) > 0) {
								// skip the "System Idle Process" as it belongs to nobody
								//out argument to return user and domain
								string[] s = new String[2];
								//Invoke the GetOwner method and populate the array with the user name and domain
								process.InvokeMethod("GetOwner", (object[])s);
								DomainUser du = new DomainUser(s[1], s[0]);

								if (excludeSystemUsers && DomainUser.IsSystemAccount(du)) {
									// do not add, it is a system user.
								} else if (users.Contains(du)) {
									// do not add, it is already in the list.
								} else {
									users.Add(du);
								}
							}
						}
					}
				}
			}
			return users;
		}

		#endregion

		#region Net / IO / Web

        /// <summary>
        /// Efficiently convert given DataSet to an xml string
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DataSetToXml(DataSet ds) {
            var output = "";
            if (ds != null) {
                using (var ms = new MemoryStream()) {
                    using (var sr = new StreamReader(ms)) {
                        ds.WriteXml(ms, XmlWriteMode.WriteSchema);
                        ms.Position = 0;
                        output = sr.ReadToEnd();
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Efficiently convert given xml string to a DataSet object
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataSet DataSetFromXml(string xml) {
            DataSet ds = new DataSet();
            if (!String.IsNullOrEmpty(xml)) {
                using (var sr = new StringReader(xml)) {
                    ds.ReadXml(sr);
                }
            }
            return ds;
        }


        public static string GetSearchEngineBindingUrl() {
            var url = Toolkit.GetSetting("SearchEngineBindingUrl", "");
            return url;
        }

        /// <summary>
        /// Gets the EndpointAddress for the search engine. If url is null or empty, looks up from config setting "SearchEngineBindingUrl".  Typical url values are:  http://localhost:2011/searchhost, net.pipe://localhost/searchhost, net.tcp://localhost:2012/searchhost, tcp://localhost:2012/searchhost
        /// </summary>
        /// <param name="defaultUrl"></param>
        /// <returns></returns>
        public static EndpointAddress GetSearchEngineAddress(string url) {
            if (String.IsNullOrEmpty(url)) {
                url = GetSearchEngineBindingUrl();
            }
            return new EndpointAddress(url);
        }

        public static string GetSearchEngineBindingType() {
            var bindingType = Toolkit.GetSetting("SearchEngineBindingType", "").ToLower();
            return bindingType;
        }

        public static System.ServiceModel.Channels.Binding GetSearchEngineBinding(string bindingType) {
            if (String.IsNullOrEmpty(bindingType)) {
                bindingType = GetSearchEngineBindingType();
            }

            switch (bindingType) {
                case "tcp":
                    // no longer used, TcpSearchServer now implements this (and uses bindingType, not Binding)
                    return null;

                case "http":
                default:
                    /*
                     * closeTimeout="00:01:00"
      openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:01:00"
      allowCookies="false" bypassProxyOnLocal="false" hostNameComparisonMode="StrongWildcard"
      maxBufferSize="65536" maxBufferPoolSize="524288" maxReceivedMessageSize="65536"
      messageEncoding="Text" textEncoding="utf-8" transferMode="Buffered"
      useDefaultWebProxy="true">
      <readerQuotas maxDepth="32" maxStringContentLength="8192" maxArrayLength="16384"
        maxBytesPerRead="4096" maxNameTableCharCount="16384" />
      <security mode="None">
        <transport clientCredentialType="None" proxyCredentialType="None"
          realm="" />
        <message clientCredentialType="UserName" algorithmSuite="Default" />
      </security>
                     * */
                    var httpBinding = new BasicHttpBinding();
                    httpBinding.BypassProxyOnLocal = false;
                    httpBinding.AllowCookies = false;
                    httpBinding.SendTimeout = TimeSpan.FromMinutes(Toolkit.GetSetting("SearchEngineSendTimeout", 1.0f));
                    httpBinding.ReceiveTimeout = TimeSpan.FromMinutes(Toolkit.GetSetting("SearchEngineReceiveTimeout", 10.0f));
                    httpBinding.MaxBufferSize = int.MaxValue; // 65536;
                    httpBinding.MaxBufferPoolSize = int.MaxValue; // 524288;
                    httpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                    httpBinding.MaxReceivedMessageSize = int.MaxValue; // 65536;
                    httpBinding.MessageEncoding = WSMessageEncoding.Text;
                    httpBinding.TextEncoding = UTF8Encoding.UTF8;
                    httpBinding.TransferMode = TransferMode.Buffered;
                    if (httpBinding.ReaderQuotas != null) {
                        httpBinding.ReaderQuotas.MaxDepth = 32;
                        httpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue; // 8192;
                        httpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue; // 16384;
                        httpBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue; // 4096;
                        httpBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue; // 16384;
                    }
                    if (httpBinding.Security != null) {
                        httpBinding.Security.Mode = BasicHttpSecurityMode.None;
                        if (httpBinding.Security.Transport != null) {
                            httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                            httpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                            httpBinding.Security.Transport.Realm = "";
                        }
                        if (httpBinding.Security.Message != null) {
                            httpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                            httpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
                        }
                    }
                    return httpBinding;

                case "pipe":
                    /*
                     *         <binding name="NetNamedPipeBinding_ISearchHost" closeTimeout="00:01:00"
      openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:01:00"
      transactionFlow="false" transferMode="Buffered" transactionProtocol="OleTransactions"
      hostNameComparisonMode="StrongWildcard" maxBufferPoolSize="2147483647"
      maxBufferSize="2147483647" maxConnections="10" maxReceivedMessageSize="2147483647">
      <readerQuotas maxDepth="32" maxStringContentLength="2147483647"
        maxArrayLength="2147483647" maxBytesPerRead="2147483647" maxNameTableCharCount="2147483647" />
      <security mode="Transport">
        <transport protectionLevel="EncryptAndSign" />
      </security>

                     * */

                    NetNamedPipeBinding pipeBinding = new NetNamedPipeBinding();
                    pipeBinding.CloseTimeout = new TimeSpan(0, 1, 0);
                    pipeBinding.OpenTimeout = new TimeSpan(0, 1, 0);
                    pipeBinding.ReceiveTimeout = TimeSpan.FromMinutes(Toolkit.GetSetting("SearchEngineReceiveTimeout", 10.0f));
                    pipeBinding.SendTimeout = TimeSpan.FromMinutes(Toolkit.GetSetting("SearchEngineSendTimeout", 1.0f));
                    pipeBinding.TransactionFlow = false;
                    pipeBinding.TransferMode = TransferMode.Buffered;
                    pipeBinding.TransactionProtocol = TransactionProtocol.OleTransactions;
                    pipeBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                    pipeBinding.MaxBufferPoolSize = int.MaxValue;
                    pipeBinding.MaxBufferSize = int.MaxValue;
                    pipeBinding.MaxConnections = 10;
                    pipeBinding.MaxReceivedMessageSize = int.MaxValue;
                    if (pipeBinding.ReaderQuotas != null) {
                        pipeBinding.ReaderQuotas.MaxDepth = 32;
                        pipeBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                        pipeBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;
                        pipeBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                        pipeBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
                    }
                    if (pipeBinding.Security != null) {
                        pipeBinding.Security.Mode = NetNamedPipeSecurityMode.Transport;
                        if (pipeBinding.Security.Transport != null) {
                            pipeBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                        }
                    }
                    return pipeBinding;

//                case "tcp":
//                    /*(
//                     *         <binding name="NetTcpBinding_ISearchHost" closeTimeout="00:01:00"
//      openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:01:00"
//      transactionFlow="false" transferMode="Buffered" transactionProtocol="OleTransactions"
//      hostNameComparisonMode="StrongWildcard" listenBacklog="10" maxBufferPoolSize="524288"
//      maxBufferSize="65536" maxConnections="10" maxReceivedMessageSize="65536">
//      <readerQuotas maxDepth="32" maxStringContentLength="8192" maxArrayLength="16384"
//        maxBytesPerRead="4096" maxNameTableCharCount="16384" />
//      <reliableSession ordered="true" inactivityTimeout="00:10:00"
//        enabled="false" />
//      <security mode="Transport">
//        <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
//        <message clientCredentialType="Windows" />
//      </security>
//    </binding>
//*/
//                    NetTcpBinding tcpBinding = new NetTcpBinding();
//                    tcpBinding.CloseTimeout = new TimeSpan(0, 1, 0);
//                    tcpBinding.OpenTimeout = new TimeSpan(0, 1, 0);
//                    tcpBinding.ReceiveTimeout = TimeSpan.FromMinutes(Toolkit.GetSetting("SearchEngineReceiveTimeout", 10.0f));
//                    tcpBinding.SendTimeout = TimeSpan.FromMinutes(Toolkit.GetSetting("SearchEngineSendTimeout", 1.0f));
//                    tcpBinding.TransactionFlow = false;
//                    tcpBinding.TransferMode = TransferMode.Buffered;
//                    tcpBinding.TransactionProtocol = TransactionProtocol.OleTransactions;
//                    tcpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
//                    tcpBinding.ListenBacklog = 10;
//                    tcpBinding.MaxBufferPoolSize = int.MaxValue; // 524288;
//                    tcpBinding.MaxBufferSize = int.MaxValue; // 65536;
//                    tcpBinding.MaxConnections = 10;
//                    tcpBinding.MaxReceivedMessageSize = int.MaxValue; //16384;
//                    tcpBinding.ReaderQuotas.MaxDepth = 32;
//                    tcpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue; //8192;
//                    tcpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue; //16384;
//                    tcpBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue; // 4096;
//                    tcpBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue; // 16384;
//                    tcpBinding.ReliableSession.Ordered = true;
//                    tcpBinding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);
//                    tcpBinding.ReliableSession.Enabled = false;
//                    tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
//                    tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
//                    tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;

//                    return tcpBinding;
            }

        }

        private const int CHUNK_SIZE = 4096;
        public static string ReadAllStreamText(NetworkStream stm, string endMoniker) {
            byte[] chunk = new byte[CHUNK_SIZE];
            int bytesRead = int.MaxValue;
            var message = new StringBuilder();

            while (true) {
                try {
                    //blocks until a client sends a message
                    bytesRead = stm.Read(chunk, 0, CHUNK_SIZE);
                } catch {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0) {
                    //the client has disconnected from the server
                    break;
                }

                var s = UTF8Encoding.UTF8.GetString(chunk, 0, bytesRead);
                message.Append(s);
                if (s.EndsWith(endMoniker)) {
                    // we're done!
                    break;
                }

            }
            return message.ToString();
        }

		#endregion Net / IO / Web

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "Core", "Toolkit", resourceName, null, defaultValue, substitutes);
        }

		#region Get Application Settings

		/// <summary>
		/// Gets a nullable int setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static int? GetSetting(string name, int? defaultValue) {
			return ToInt32(ConfigurationManager.AppSettings[name], defaultValue);
		}
		/// <summary>
		/// Gets a nullable float setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static float? GetSetting(string name, float? defaultValue) {
			return ToFloat(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a nullable double setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static double? GetSetting(string name, double? defaultValue) {
			return ToDouble(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a nullable decimal setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static decimal? GetSetting(string name, decimal? defaultValue) {
			return ToDecimal(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a nullable bool setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static bool? GetSetting(string name, bool? defaultValue) {
			return ToBoolean(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a nullable DateTime setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static DateTime? GetSetting(string name, DateTime? defaultValue) {
			return ToDateTime(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a string setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		//[DebuggerStepThrough]
		public static string GetSetting(string name, string defaultValue) {
			string val = ConfigurationManager.AppSettings[name];
			if (String.IsNullOrEmpty(val)) {
				return defaultValue;
			} else {
				return val;
			}
		}


		/// <summary>
		/// Returns a read-only Dictionary of all settings from the current config file
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, string> ListConfigSettings() {
			Dictionary<string, string> dic = new Dictionary<string,string>();
			NameValueCollection nvc = ConfigurationManager.AppSettings;
			foreach(string key in nvc.Keys){
				dic.Add(key, nvc[key]);
			}
			return dic;
		}

        /// <summary>
        /// Writes the value to the appropriate setting in the web.config or (application name).config file
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SaveConfigSetting(string name, string value){
            ConfigurationManager.AppSettings[name] = value;
        }


		/// <summary>
		/// Gets a non-nullable int setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static int GetSetting(string name, int defaultValue) {
			return ToInt32(ConfigurationManager.AppSettings[name], defaultValue);
		}
		/// <summary>
		/// Gets a non-nullable float setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static float GetSetting(string name, float defaultValue) {
			return ToFloat(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a non-nullable double setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static double GetSetting(string name, double defaultValue) {
			return ToDouble(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a non-nullable decimal setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static decimal GetSetting(string name, decimal defaultValue) {
			return ToDecimal(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a non-nullable bool setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static bool GetSetting(string name, bool defaultValue) {
			return ToBoolean(ConfigurationManager.AppSettings[name], defaultValue);
		}

		/// <summary>
		/// Gets a non-nullable DateTime setting from the app config file, defaulting to the given value if it does not exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public static DateTime GetSetting(string name, DateTime defaultValue) {
			return ToDateTime(ConfigurationManager.AppSettings[name], defaultValue);
		}


        ///// <summary>
        ///// Gets the list of installed applications and their version numbers.
        ///// </summary>
        ///// <returns></returns>
        //public static Dictionary<string, string> ListInstalledApplications() {
        //    var installedApps = new Dictionary<string, string>();
        //    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false)) {
        //        string[] guids = rk.GetSubKeyNames();
        //        foreach (string guid in guids) {
        //            string displayName = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "DisplayName", "") as string;
        //            string displayVersion = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "DisplayVersion", "") as string;
        //            string uninstallString = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + guid, "UninstallString", "") as string;
        //            if (!String.IsNullOrEmpty(displayName) && !installedApps.ContainsKey(displayName)) {
        //                Guid productGuid = Guid.Empty;
        //                if (guid.StartsWith("{") && guid.Contains("-") && guid.Length >= 38 && guid[37] == '}') {
        //                    productGuid = new Guid(guid.Substring(0, 38));
        //                }
        //                installedApps.Add(displayName, displayVersion);
        //            }
        //        }
        //    }

        //    return installedApps;
        //}

        ///// <summary>
        ///// Gets the version of the MSI that installed the application (as shown in Add / Remove Programs)
        ///// </summary>
        ///// <param name="installedAppName"></param>
        ///// <returns></returns>
        //public static string GetInstalledVersion(string installedAppName) {
        //    var sd = ListInstalledApplications();
        //    var ver = "";
        //    if (sd.TryGetValue(installedAppName, out ver)) {
        //        return installedAppName + " v " + ver;
        //    } else {
        //        return GetApplicationVersion(installedAppName);
        //    }
        //}

        /// <summary>
        /// returns the string "(given appName) v 1.3.3.4" representing the application's version
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationVersion(string appName) {
            Assembly asm = Assembly.GetEntryAssembly();
            if (asm == null) {
                asm = Assembly.GetCallingAssembly();
                if (asm == null) {
                    asm = Assembly.GetExecutingAssembly();
                }
                if (asm == null) {
                    return "Unknown assembly!";
                }
            }
            AssemblyName an = asm.GetName();
            return appName + " v " + an.Version.ToString();
        }

        [ComVisible(true)]
        public static bool Is64BitOperatingSystem() {

            // ProgramW6432 does not exist on Vista 64-bit, only Windows 7 64-bit...
//            var ret = Environment.GetEnvironmentVariable("ProgramW6432");
//            return !String.IsNullOrEmpty(ret);

            // PROCESSOR_ARCHITECTURE reports x86 if the current process is 32-bit (even if it's running on a 64-bit OS)
            // http://windowsitpro.com/article/articleid/102210/q-the-processor_architecture-environment-variable-isnt-returning-the-value-i-expect-whats-wrong.html
            //var ret = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            //if (String.IsNullOrEmpty(ret)) {
            //    return false;
            //} else {
            //    return ret.Contains("64");
            //}

            // HACK: other options fail, so we're using the existence of a 32-bit subsystem Program Files environment variable
            //       to detect if current system is 64-bit.  Note this will fail when Windows 64-bit operating systems stop supporting the 32-bit subsystem ... Which will not happen any time soon. (famous last words, right?)
            var ret = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            return !String.IsNullOrEmpty(ret);
        }

        [ComVisible(true)]
        public static bool Is64BitProcess() {
            return IntPtr.Size == 8;
        }

		/// <summary>
		/// returns the string "(assembly name) v 1.3.3.4" representing the application's version
		/// </summary>
		/// <returns></returns>
		public static string GetApplicationVersion() {
			Assembly asm = Assembly.GetEntryAssembly();
			if (asm == null ){
				asm = Assembly.GetCallingAssembly();
				if (asm == null){
					asm = Assembly.GetExecutingAssembly();
				}
				if (asm == null){
					return "Unknown assembly!";
				}
			}
			AssemblyName an = asm.GetName();
			return an.Name + " v " + an.Version.ToString();
		}
		#endregion

		#region xml
		/// <summary>
		/// Gets the .Value of the given node.  Returns null if node is null. Does not throw exceptions.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetNodeText(XmlNode node) {
			return GetNodeText(node, null);
		}

		/// <summary>
		/// Gets the .Value of the given node.  Returns defaultValue is node is null. Does not throw exceptions.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetNodeText(XmlNode node, string defaultValue) {
			if (node == null) {
				return defaultValue;
			} else {
				return node.InnerText;
			}
		}

		/// <summary>
		/// Gets the .Value of the attribute on the given node.  If node or attribute don't exist, returns null. Does not throw exceptions.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attName"></param>
		/// <returns></returns>
		public static string GetAttValue(XmlNode node, string attName) {
			return GetAttValue(node, attName, null);
		}

		/// <summary>
		/// Gets the .Value of the attribute on the given node.  If node or attribute don't exist, returns defaultValue. Does not throw exceptions.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="attName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string GetAttValue(XmlNode node, string attName, string defaultValue) {
			if (node == null) {
				return defaultValue;
			} else {
				XmlAttribute att = node.Attributes[attName];
				if (att == null) {
					return defaultValue;
				} else {
					return att.Value;
				}
			}
		}

        /// <summary>
        /// Sets the .Value of the given attribute.  Auto-appends a new attribute as needed.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attName"></param>
        /// <param name="attValue"></param>
        public static void SetAttValue(XmlNode node, string attName, string attValue) {
            var att = node.Attributes[attName];
            if (att == null) {
                att = node.OwnerDocument.CreateAttribute(attName);
                att.Value = attValue;
                node.Attributes.Append(att);
            } else {
                att.Value = attValue;
            }
        }



		#endregion xml

		#region Timer
		/// <summary>
		/// Return the number of milliseconds it takes to execute the given callback
		/// </summary>
		/// <param name="timerName"></param>
		/// <param name="vc"></param>
		/// <returns></returns>
		public static double TimeIt(string timerName, Toolkit.VoidCallback vc) {
			using (HighPrecisionTimer hpt = new HighPrecisionTimer(timerName, true)) {
				vc();
				return hpt.Stop();
			}
		}
		#endregion Timer

		#region P/Invokes


		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccess DesiredAccess, out IntPtr TokenHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

		[Flags]
		private enum TokenAccess : uint {
			StandardRightsRequired = 0x000F0000,
			StandardRightsRead = 0x00020000,
			TokenAssignPrimary = 0x0001,
			TokenDuplicate = 0x0002,
			TokenImpersonate = 0x0004,
			TokenQuery = 0x0008,
			TokenQuerySource = 0x0010,
			TokenAdjustPrivileges = 0x0020,
			TokenAdjustGroups = 0x0040,
			TokenAdjustDefault = 0x0080,
			TokenAdjustSessionid = 0x0100,
			TokenRead = (0x00020000 | 0x0008),
			TokenAllAccess = (0x000f0000 | 0x0001 |
				0x0002 | 0x0004 | 0x0008 | 0x0010 |
				0x0020 | 0x0040 | 0x0080 |
				0x0100)
		}

		private enum TokenType {
			TokenPrimary = 1,
			TokenImpersonation = 2
		}

		private enum SecurityImpersonationLevel {
			SecurityAnonymous,
			SecurityIdentification,
			SecurityImpersonation,
			SecurityDelegation
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern int ImpersonateLoggedOnUser(IntPtr hToken);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool RevertToSelf();

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseHandle(IntPtr hObject);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private extern static bool DuplicateTokenEx(
			IntPtr hExistingToken,
			uint dwDesiredAccess,
			ref SECURITY_ATTRIBUTES lpTokenAttributes,
			SecurityImpersonationLevel ImpersonationLevel,
			TokenType TokenType,
			out IntPtr phNewToken);


		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool CreateProcessAsUser(
			IntPtr hToken,
			string lpApplicationName,
			[In] StringBuilder lpCommandLine,
			ref SECURITY_ATTRIBUTES lpProcessAttributes,
			ref SECURITY_ATTRIBUTES lpThreadAttributes,
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);

		[StructLayout(LayoutKind.Sequential)]
		private struct SECURITY_ATTRIBUTES {
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public int bInheritHandle;
		}
		[StructLayout(LayoutKind.Sequential)]
		private struct PROCESS_INFORMATION {
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct STARTUPINFO {
			public Int32 mb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public Int32 dwX;
			public Int32 dwY;
			public Int32 dwXSize;
			public Int32 dwYSize;
			public Int32 dwXCountChars;
			public Int32 dwYCountChars;
			public Int32 dwFillAttribute;
			public Int32 dwFlags;
			public Int16 wShowWindow;
			public Int16 cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}
		private enum LOGON_TYPE {
			LOGON32_LOGON_INTERACTIVE = 2,
			LOGON32_LOGON_NETWORK,
			LOGON32_LOGON_BATCH,
			LOGON32_LOGON_SERVICE,
			LOGON32_LOGON_UNLOCK = 7,
			LOGON32_LOGON_NETWORK_CLEARTEXT,
			LOGON32_LOGON_NEW_CREDENTIALS
		}

		private enum LOGON_PROVIDER {
			LOGON32_PROVIDER_DEFAULT,
			LOGON32_PROVIDER_WINNT35,
			LOGON32_PROVIDER_WINNT40,
			LOGON32_PROVIDER_WINNT50
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool PostMessage(HandleRef hWnd, WindowsMessages msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		private static extern IntPtr SendMessage(HandleRef hWnd, WindowsMessages msg, IntPtr wParam, IntPtr lParam);

		private enum WindowsMessages : uint {
			WM_ACTIVATE = 0x6,
			WM_ACTIVATEAPP = 0x1C,
			WM_AFXFIRST = 0x360,
			WM_AFXLAST = 0x37F,
			WM_APP = 0x8000,
			WM_ASKCBFORMATNAME = 0x30C,
			WM_CANCELJOURNAL = 0x4B,
			WM_CANCELMODE = 0x1F,
			WM_CAPTURECHANGED = 0x215,
			WM_CHANGECBCHAIN = 0x30D,
			WM_CHAR = 0x102,
			WM_CHARTOITEM = 0x2F,
			WM_CHILDACTIVATE = 0x22,
			WM_CLEAR = 0x303,
			WM_CLOSE = 0x10,
			WM_COMMAND = 0x111,
			WM_COMPACTING = 0x41,
			WM_COMPAREITEM = 0x39,
			WM_CONTEXTMENU = 0x7B,
			WM_COPY = 0x301,
			WM_COPYDATA = 0x4A,
			WM_CREATE = 0x1,
			WM_CTLCOLORBTN = 0x135,
			WM_CTLCOLORDLG = 0x136,
			WM_CTLCOLOREDIT = 0x133,
			WM_CTLCOLORLISTBOX = 0x134,
			WM_CTLCOLORMSGBOX = 0x132,
			WM_CTLCOLORSCROLLBAR = 0x137,
			WM_CTLCOLORSTATIC = 0x138,
			WM_CUT = 0x300,
			WM_DEADCHAR = 0x103,
			WM_DELETEITEM = 0x2D,
			WM_DESTROY = 0x2,
			WM_DESTROYCLIPBOARD = 0x307,
			WM_DEVICECHANGE = 0x219,
			WM_DEVMODECHANGE = 0x1B,
			WM_DISPLAYCHANGE = 0x7E,
			WM_DRAWCLIPBOARD = 0x308,
			WM_DRAWITEM = 0x2B,
			WM_DROPFILES = 0x233,
			WM_ENABLE = 0xA,
			WM_ENDSESSION = 0x16,
			WM_ENTERIDLE = 0x121,
			WM_ENTERMENULOOP = 0x211,
			WM_ENTERSIZEMOVE = 0x231,
			WM_ERASEBKGND = 0x14,
			WM_EXITMENULOOP = 0x212,
			WM_EXITSIZEMOVE = 0x232,
			WM_FONTCHANGE = 0x1D,
			WM_GETDLGCODE = 0x87,
			WM_GETFONT = 0x31,
			WM_GETHOTKEY = 0x33,
			WM_GETICON = 0x7F,
			WM_GETMINMAXINFO = 0x24,
			WM_GETOBJECT = 0x3D,
			WM_GETSYSMENU = 0x313,
			WM_GETTEXT = 0xD,
			WM_GETTEXTLENGTH = 0xE,
			WM_HANDHELDFIRST = 0x358,
			WM_HANDHELDLAST = 0x35F,
			WM_HELP = 0x53,
			WM_HOTKEY = 0x312,
			WM_HSCROLL = 0x114,
			WM_HSCROLLCLIPBOARD = 0x30E,
			WM_ICONERASEBKGND = 0x27,
			WM_IME_CHAR = 0x286,
			WM_IME_COMPOSITION = 0x10F,
			WM_IME_COMPOSITIONFULL = 0x284,
			WM_IME_CONTROL = 0x283,
			WM_IME_ENDCOMPOSITION = 0x10E,
			WM_IME_KEYDOWN = 0x290,
			WM_IME_KEYLAST = 0x10F,
			WM_IME_KEYUP = 0x291,
			WM_IME_NOTIFY = 0x282,
			WM_IME_REQUEST = 0x288,
			WM_IME_SELECT = 0x285,
			WM_IME_SETCONTEXT = 0x281,
			WM_IME_STARTCOMPOSITION = 0x10D,
			WM_INITDIALOG = 0x110,
			WM_INITMENU = 0x116,
			WM_INITMENUPOPUP = 0x117,
			WM_INPUTLANGCHANGE = 0x51,
			WM_INPUTLANGCHANGEREQUEST = 0x50,
			WM_KEYDOWN = 0x100,
			WM_KEYFIRST = 0x100,
			WM_KEYLAST = 0x108,
			WM_KEYUP = 0x101,
			WM_KILLFOCUS = 0x8,
			WM_LBUTTONDBLCLK = 0x203,
			WM_LBUTTONDOWN = 0x201,
			WM_LBUTTONUP = 0x202,
			WM_MBUTTONDBLCLK = 0x209,
			WM_MBUTTONDOWN = 0x207,
			WM_MBUTTONUP = 0x208,
			WM_MDIACTIVATE = 0x222,
			WM_MDICASCADE = 0x227,
			WM_MDICREATE = 0x220,
			WM_MDIDESTROY = 0x221,
			WM_MDIGETACTIVE = 0x229,
			WM_MDIICONARRANGE = 0x228,
			WM_MDIMAXIMIZE = 0x225,
			WM_MDINEXT = 0x224,
			WM_MDIREFRESHMENU = 0x234,
			WM_MDIRESTORE = 0x223,
			WM_MDISETMENU = 0x230,
			WM_MDITILE = 0x226,
			WM_MEASUREITEM = 0x2C,
			WM_MENUCHAR = 0x120,
			WM_MENUCOMMAND = 0x126,
			WM_MENUDRAG = 0x123,
			WM_MENUGETOBJECT = 0x124,
			WM_MENURBUTTONUP = 0x122,
			WM_MENUSELECT = 0x11F,
			WM_MOUSEACTIVATE = 0x21,
			WM_MOUSEFIRST = 0x200,
			WM_MOUSEHOVER = 0x2A1,
			WM_MOUSELAST = 0x20A,
			WM_MOUSELEAVE = 0x2A3,
			WM_MOUSEMOVE = 0x200,
			WM_MOUSEWHEEL = 0x20A,
			WM_MOVE = 0x3,
			WM_MOVING = 0x216,
			WM_NCACTIVATE = 0x86,
			WM_NCCACommunicatorPluginIZE = 0x83,
			WM_NCCREATE = 0x81,
			WM_NCDESTROY = 0x82,
			WM_NCHITTEST = 0x84,
			WM_NCLBUTTONDBLCLK = 0xA3,
			WM_NCLBUTTONDOWN = 0xA1,
			WM_NCLBUTTONUP = 0xA2,
			WM_NCMBUTTONDBLCLK = 0xA9,
			WM_NCMBUTTONDOWN = 0xA7,
			WM_NCMBUTTONUP = 0xA8,
			WM_NCMOUSEHOVER = 0x2A0,
			WM_NCMOUSELEAVE = 0x2A2,
			WM_NCMOUSEMOVE = 0xA0,
			WM_NCPAINT = 0x85,
			WM_NCRBUTTONDBLCLK = 0xA6,
			WM_NCRBUTTONDOWN = 0xA4,
			WM_NCRBUTTONUP = 0xA5,
			WM_NEXTDLGCTL = 0x28,
			WM_NEXTMENU = 0x213,
			WM_NOTIFY = 0x4E,
			WM_NOTIFYFORMAT = 0x55,
			WM_NULL = 0x0,
			WM_PAINT = 0xF,
			WM_PAINTCLIPBOARD = 0x309,
			WM_PAINTICON = 0x26,
			WM_PALETTECHANGED = 0x311,
			WM_PALETTEISCHANGING = 0x310,
			WM_PARENTNOTIFY = 0x210,
			WM_PASTE = 0x302,
			WM_PENWINFIRST = 0x380,
			WM_PENWINLAST = 0x38F,
			WM_POWER = 0x48,
			WM_PRINT = 0x317,
			WM_PRINTCLIENT = 0x318,
			WM_QUERYDRAGICON = 0x37,
			WM_QUERYENDSESSION = 0x11,
			WM_QUERYNEWPALETTE = 0x30F,
			WM_QUERYOPEN = 0x13,
			WM_QUERYUISTATE = 0x129,
			WM_QUEUESYNC = 0x23,
			WM_QUIT = 0x12,
			WM_RBUTTONDBLCLK = 0x206,
			WM_RBUTTONDOWN = 0x204,
			WM_RBUTTONUP = 0x205,
			WM_RENDERALLFORMATS = 0x306,
			WM_RENDERFORMAT = 0x305,
			WM_SETCURSOR = 0x20,
			WM_SETFOCUS = 0x7,
			WM_SETFONT = 0x30,
			WM_SETHOTKEY = 0x32,
			WM_SETICON = 0x80,
			WM_SETREDRAW = 0xB,
			WM_SETTEXT = 0xC,
			WM_SETTINGCHANGE = 0x1A,
			WM_SHOWWINDOW = 0x18,
			WM_SIZE = 0x5,
			WM_SIZECLIPBOARD = 0x30B,
			WM_SIZING = 0x214,
			WM_SPOOLERSTATUS = 0x2A,
			WM_STYLECHANGED = 0x7D,
			WM_STYLECHANGING = 0x7C,
			WM_SYNCPAINT = 0x88,
			WM_SYSCHAR = 0x106,
			WM_SYSCOLORCHANGE = 0x15,
			WM_SYSCOMMAND = 0x112,
			WM_SYSDEADCHAR = 0x107,
			WM_SYSKEYDOWN = 0x104,
			WM_SYSKEYUP = 0x105,
			WM_SYSTIMER = 0x118,  // undocumented, see http://support.microsoft.com/?id=108938
			WM_TCARD = 0x52,
			WM_TIMECHANGE = 0x1E,
			WM_TIMER = 0x113,
			WM_UNDO = 0x304,
			WM_UNINITMENUPOPUP = 0x125,
			WM_USER = 0x400,
			WM_USERCHANGED = 0x54,
			WM_VKEYTOITEM = 0x2E,
			WM_VSCROLL = 0x115,
			WM_VSCROLLCLIPBOARD = 0x30A,
			WM_WINDOWPOSCHANGED = 0x47,
			WM_WINDOWPOSCHANGING = 0x46,
			WM_WININICHANGE = 0x1A,
			WM_XBUTTONDBLCLK = 0x20D,
			WM_XBUTTONDOWN = 0x20B,
			WM_XBUTTONUP = 0x20C
		}

		private enum SHOWWINDOW : uint {
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_FORCEMINIMIZE = 11,
			SW_MAX = 11,
		}

		[Flags]
		private enum STARTF : uint {
			STARTF_USESHOWWINDOW = 0x00000001,
			STARTF_USESIZE = 0x00000002,
			STARTF_USEPOSITION = 0x00000004,
			STARTF_USECOUNTCHARS = 0x00000008,
			STARTF_USEFILLATTRIBUTE = 0x00000010,
			STARTF_RUNFULLSCREEN = 0x00000020,  // ignored for non-x86 platforms
			STARTF_FORCEONFEEDBACK = 0x00000040,
			STARTF_FORCEOFFFEEDBACK = 0x00000080,
			STARTF_USESTDHANDLES = 0x00000100,
		}

		private enum TOKEN_INFORMATION_CLASS {
			TokenUser = 1,
			TokenGroups,
			TokenPrivileges,
			TokenOwner,
			TokenPrimaryGroup,
			TokenDefaultDacl,
			TokenSource,
			TokenType,
			TokenImpersonationLevel,
			TokenStatistics,
			TokenRestrictedSids,
			TokenSessionId,
			TokenGroupsAndPrivileges,
			TokenSessionReference,
			TokenSandBoxInert,
			TokenAuditPolicy,
			TokenOrigin
		}

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(
			string lpszUsername,
			string lpszDomain,
			string lpszPassword,
			LOGON_TYPE dwLogonType,
			LOGON_PROVIDER dwLogonProvider,
			out IntPtr phToken
			);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool DuplicateToken(IntPtr ExistingTokenHandle, SecurityImpersonationLevel
		   ImpersonationLevel, out IntPtr DuplicateTokenHandle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
		#endregion

	}
}