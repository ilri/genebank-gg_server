using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;

namespace GrinGlobal.Search.Engine {
    public class SearchOptions {
        public static SearchOptions Parse(Dictionary<string, string> options) {

            var rv = new SearchOptions();
            if (options != null) {

                // remember our passthru setting...
                rv.PassThruLevel = DatabasePassthruLevel.NonIndexedOrComparison;
                var temp1 = string.Empty;
                if (options.TryGetValue("passthru", out temp1)) {
                    try {
                        rv.PassThruLevel = ((DatabasePassthruLevel)Enum.Parse(typeof(DatabasePassthruLevel), temp1 + "", true));
                    } catch {
                        // invalid passthru specified, ignore it and just continue to use default of NonIndexedOrComparison.
                    }
                }

                // remmeber lookup coded values setting...
                rv.LookupCodedValues = true;
                if (options.TryGetValue("lookupcodedvalues", out temp1)) {
                    rv.LookupCodedValues = Toolkit.ToBoolean(temp1, true);
                }


                // remember language id setting...
                rv.LanguageID = 0;
                if (options.TryGetValue("languageid", out temp1)) {
                    rv.LanguageID = Toolkit.ToInt32(temp1, 0);
                }

                // remember ignore cache setting
                rv.IgnoreCache = false;
                if (options.TryGetValue("ignorecache", out temp1)) {
                    rv.IgnoreCache = Toolkit.ToBoolean(temp1, false);
                }

                // remember explicit indexes
                rv.AddExplicitIndexes = true;
                if (options.TryGetValue("addexplicitindexes", out temp1)) {
                    rv.AddExplicitIndexes = Toolkit.ToBoolean(temp1, true);
                }

                rv.OrMultipleLines = false;
                if (options.TryGetValue("ormultiplelines", out temp1)) {
                    rv.OrMultipleLines = Toolkit.ToBoolean(temp1, false);
                }

                rv.ParseOnly = false;
                if (options.TryGetValue("parseonly", out temp1)) {
                    rv.ParseOnly = Toolkit.ToBoolean(temp1, false);
                }


            }

            return rv;

        }

        private SearchOptions() { }

        public DatabasePassthruLevel PassThruLevel { get; private set; }
        public bool LookupCodedValues { get; private set; }
        public bool IgnoreCache { get; private set; }
        public bool AddExplicitIndexes { get; private set; }
        public int LanguageID { get; private set; }
        public bool OrMultipleLines { get; private set; }

        public bool ParseOnly { get; private set; }
    }
}
