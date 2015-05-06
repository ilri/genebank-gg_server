using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface {
    public static class Extensions {

        /// <summary>
        /// Assumes given delimiter can be escaped using a backslash.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] Split2(this string source, string delimiter, bool removeEmptyEntries) {
            StringSplitOptions sso = StringSplitOptions.None;
            if (removeEmptyEntries) {
                sso = StringSplitOptions.RemoveEmptyEntries;
            }
            if (!source.Contains(@"\" + delimiter)) {
                return source.Split(new string[] { delimiter }, sso);
            } else {
                string[] arr = source.Split(new string[] { delimiter }, sso);
                List<string> ret = new List<string>();
                for (int i = 0; i < arr.Length; i++) {
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

        /// <summary>
        /// Splits string on the given delimiters, optionally retaining the delimiter as an entry instead of throwing it out.  Optionally can exclude Quoted sections (meaning delimiters are ignored within quoted substrings).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiters"></param>
        /// <param name="excludeQuotedSections"></param>
        /// <returns></returns>
        public static List<string> SplitRetain(this string source, char[] delimiters, bool excludeQuotedSections, bool retainDelimiter) {
            List<string> todo = null;
            if (excludeQuotedSections) {
                todo = source.SplitOnQuotes(true, true, false);
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
                    for (int j = 0; j < todo[i].Length; j++) {
                        char c = todo[i][j];
                        sb.Append(c);
                        for (int k = 0; k < delimiters.Length; k++) {
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

                        if (!isEscaped && doubleCharEscapes && cur == delim) {
                            // see if next char is same as current one
                            if (i < text.Length - 1) {
                                isEscaped = cur == text[i + 1] && inQuotes;
                                if (!isEscaped && i > quoteStartPos + 1) {
                                    isEscaped = prev == delim;
                                }
                            }
                        }

                        if (isEscaped) {
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
                if (sb.Length > 0) {
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

            if (String.IsNullOrEmpty(source)) {
                return false;
            } else {
                return (source.StartsWith("'") && source.EndsWith("'"))
                || (source.StartsWith(@"""") && source.EndsWith(@""""));
            }

        }

    }
}
