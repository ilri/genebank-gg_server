using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GrinGlobal.Interface {
    public class Util {

        /// <summary>
        /// Given a string of key/value pairs delimited with = and ;  (e.g.:   key1=val1;key2=val2), returns a dictionary containing the parsed key/value pairs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delimitedKeyValuePairs"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ParsePairs<T>(string delimitedKeyValuePairs) {
            Dictionary<string, T> dic = new Dictionary<string, T>();
            if (!String.IsNullOrEmpty(delimitedKeyValuePairs)) {
                List<string> kvPairs = delimitedKeyValuePairs.SplitRetain(new char[] { ';', '=' }, true, true);
                for (int i = 0; i < kvPairs.Count; i++) {

                    string keyword = kvPairs[i].Trim();

                    if (keyword == ";" || keyword == "=") {
                        // just a delimiter, ignore
                    } else {

                        // keyword found.
                        // if next one is an '=', go ahead and try to parse it

                        string value = null;

                        if (i < kvPairs.Count - 1) {

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
                                    value = Util.Cut(value, 1, -1);
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
        /// Converts the given string to a non-nullable int, returning int.MinValue if it is inconvertible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(string value) {
            return ToInt32(value, int.MinValue);
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
        /// Cuts a string at the given starting position, which can be negative.  Does not throw exceptions.
        /// </summary>
        /// <param name="value">String to cut</param>
        /// <param name="startPosition">If positive, from the left.  If negative, from the right. If > length of value, returns all chars right of start position</param>
        /// <returns></returns>
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
        public static string Cut(string value, int startPosition, int length) {
            if (String.IsNullOrEmpty(value)) {
                return value;
            }

            if (length != value.Length && startPosition != 0) {
                // they specified both ends.
                // recurse to resolve front end first
                value = Util.Cut(value, startPosition, value.Length);
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
            } else if (startPosition + length < 0) {
                // they said pull negative chars. return empty string.
                return string.Empty;
            } else {
                // they said pull a valid subset of chars.
                return value.Substring(startPosition, length);
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
                if (!String.IsNullOrEmpty(value)) {
                    if (value == "1" || value.ToLower() == "y") {
                        return true;
                    } else if (value == "0" || value.ToLower() == "n") {
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


        public static List<string> ParseDelimitedLine(string line, char delimiter) {

            List<string> values = new List<string>();
            values.AddRange(line.Split2(delimiter.ToString(), false));
            return values;

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


    }
}
