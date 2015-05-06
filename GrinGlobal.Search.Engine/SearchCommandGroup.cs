using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// Represents a logical group of search commands
    /// </summary>
    internal class SearchCommandGroup<TKey, TValue> {


        /*
         * zea || @number 37
         * ((zea) or (@number = 37))
         * 
         * @prefix between 'frank' and 'thomas'
         * ((@prefix > 'frank') and (@prefix < thomas))
         * 
         * @prefix (23, 748, 999)
         * @prefix IN (23, 748, 999)
         * ((@prefix = 23) or (@prefix = 748) or (@prefix = 999))
         * 
         * 
         * 
         * 
         */

        //SearchCommand<TKey, TValue> LeftCommand;
        //SearchCommand<TKey, TValue> Operator;
        //SearchCommand<TKey, TValue> RightCommand;

        internal static Queue<SearchCommandGroup<TKey, TValue>> Parse(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, DataSet ds, string options) {

            var ret = new Queue<SearchCommandGroup<TKey, TValue>>();

            string mungedSearchString = searchString;

            // HACK: if there's line breaks, put an OR between each line
            if (mungedSearchString.Contains("\n")) {
                mungedSearchString = "(" + mungedSearchString.Replace("\r\n", "\n").Replace("\n", ") or (") + ")";
                mungedSearchString = Regex.Replace(mungedSearchString, @"[) ]or\s+\(\s*\)", "");
            }

            List<string> words = mungedSearchString.SplitRetain(new char[]{' ', '\r', '\n'}, true, false, true);




            return ret;

        }


    }
}
