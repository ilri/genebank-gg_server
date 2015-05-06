using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Interface.DataTriggers {
    public class ReadOptions {
        public int? AltLanguageID;
        public bool UnescapeWhereClause;
        public ReadOptions Clone() {
            var rv = new ReadOptions(null);
            rv.AltLanguageID = this.AltLanguageID;
            return rv;

        }
        public ReadOptions(string options) {
            AltLanguageID = null;
            UnescapeWhereClause = false;
            if (!String.IsNullOrEmpty(options)) {
                var dic = Util.ParsePairs<string>(options);
                foreach (var key in dic.Keys) {
                    switch (key.ToLower()) {
                        case "altlanguageid":
                            AltLanguageID = Util.ToInt32(dic[key], null);
                            break;
                        case "unescapewhereclause":
                            UnescapeWhereClause = Util.ToBoolean(dic[key], false);
                            break;
                        default:
                            // TODO: add more options here!
                            break;
                    }
                }
            }
        }
    }

}
