using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Search.Engine {
    internal class Lib {

        internal static string GetSetting(Dictionary<string, string> dic, string category, string name, string defaultValue) {
            if (dic.ContainsKey(category + "--" + name)) {
                return dic[category + "--" + name];
            } else {
                return defaultValue;
            }
        }

        internal static void AddSettingAsNeeded(Dictionary<string, string> dic, string category, string name, string value, bool overwriteExisting) {
            if (!dic.ContainsKey(category + "--" + name) || overwriteExisting) {
                dic[category + "--" + name] = value;
            }
        }

        internal static void AddSettingAsNeeded(Dictionary<string, string> dic, string category, string name, string value) {
            AddSettingAsNeeded(dic, category, name, value, false);
        }

        /// <summary>
        /// Returns all entries from given dictionary that start with the given category.  Category is removed from the key in the returned dictionary.
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> GetAllSettingsByCategory(Dictionary<string, string> dic, string category) {
            var rv = new Dictionary<string, string>();

            foreach (var key in dic.Keys) {
                if (key.StartsWith(category + "--")) {
                    rv.Add(key.Replace(category + "--", ""), dic[key]);
                }
            }

            return rv;

        }


    }
}
