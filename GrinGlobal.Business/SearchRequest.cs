using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Core; // KFE

namespace GrinGlobal.Business
{
    class SearchRequest : IDisposable
    {
        SecureData _sd;
        GrinGlobal.Core.DataManager _dm;

        public SearchRequest(SecureData sd, GrinGlobal.Core.DataManager dm)
        {
            _sd = sd;
            _dm = dm;
        }

        public DataSet FindPKeys(string pKeyType, string searchText, string defaultOperator, int searchLimit) {
            return FindPKeysDynamic(pKeyType, searchText, defaultOperator, searchLimit);
        }

        public DataSet FindPKeys(string pKeyType, string searchText, string defaultOperator, string seType, int searchLimit) {
            if (seType.ToLower() == "static") {
                return FindPKeysStatic(pKeyType, searchText, defaultOperator, searchLimit);
            } else if (seType.ToLower() == "dynamic") {
                return FindPKeysDynamic(pKeyType, searchText, defaultOperator, searchLimit);
            } else {
                return FindPKeysDynamic(pKeyType, searchText, defaultOperator, searchLimit);
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose() {
            //throw new NotImplementedException();
        }

        #endregion


        //
        // ###
        // ### STATIC Search Engine section
        // ###
        // ### Pete Cyr
        // ###
        //

        public DataSet FindPKeysStatic(string pKeyType, string searchText, string defaultOperator, int searchLimit)
        {
            DataSet searchResults = _sd.CreateReturnDataSet();

            // Set the default boolean operation if it is empty or contains incorrect values...
            defaultOperator = defaultOperator.Trim().ToUpper();
            if (string.IsNullOrEmpty(defaultOperator) ||
                (defaultOperator != "AND" &&
                defaultOperator != "OR" &&
                defaultOperator != "LIST"))
            {
                defaultOperator = "AND";
            }

            // Step 1 - check to see if we have a properly formatted query string...
            string formattedSearchStringCheck = searchText.Trim().ToUpper();
            if (formattedSearchStringCheck.StartsWith("AND ") ||
                formattedSearchStringCheck.EndsWith(" AND") ||
                formattedSearchStringCheck.StartsWith("OR ") ||
                formattedSearchStringCheck.EndsWith(" OR"))
            {
                _sd.AddExceptionToTable(new Exception("The format of the search string is invalid - cannot start or end with 'AND/OR'", null), true, searchResults);
                return searchResults;
            }
            int leftParenthesis = 0;
            int rightParenthesis = 0;
            for (int i = 0; i < formattedSearchStringCheck.Length; i++)
            {
                if (formattedSearchStringCheck[i] == '(') leftParenthesis++;
                if (formattedSearchStringCheck[i] == ')') rightParenthesis++;
            }
            if (leftParenthesis > rightParenthesis)
            {
                _sd.AddExceptionToTable(new Exception("The format of the search string is invalid - missing an ending parenthesis ')' in one of the groupings", null), true, searchResults);
                return searchResults;
            }
            else if (leftParenthesis < rightParenthesis)
            {
                _sd.AddExceptionToTable(new Exception("The format of the search string is invalid - missing a beginning parenthesis '(' in one of the groupings", null), true, searchResults);
                return searchResults;
            }

            // Step 2 - break the searchText down into formatted search tokens (ex: @accession_name.plant_name like '%B73%')...
            Stack<string> searchTokens = new Stack<string>();
            if (defaultOperator == "LIST")
            {
                searchTokens = BuildListBasedSearchTokenStack(searchText);
            }
            else
            {
                searchTokens = BuildSearchTokenStack(searchText, defaultOperator);
            }

            // Consolidate search tokens from the same table into one search token...
            searchTokens = ConsolidateSearchTokens(searchTokens);
            
            // Step 3 - resolve the formatted search strings to the target PKEY type...
            List<int> pkeys = ResolveSearchTokenStack(searchTokens, pKeyType, searchLimit);

            // Step 4 - convert the search results from a List<int> to a dataset...
            if (pkeys != null &&
                pkeys.Count > -1)
            {
                searchResults.Tables.Add("SearchResult");
                //searchResults.Tables["SearchResult"].Columns.Add(pKeyType.Trim().ToLower(), typeof(int));
                searchResults.Tables["SearchResult"].Columns.Add("ID", typeof(int));
                int stopCount = 0;
                if (searchLimit == 0)
                    stopCount = pkeys.Count;
                else
                    stopCount = Math.Min(searchLimit, pkeys.Count);
                for (int i = 0; i < stopCount; i++)
                {
                    searchResults.Tables["SearchResult"].Rows.Add(pkeys[i]);
                }
            }

            return searchResults;
        }

        private List<int> ResolveSearchTokenStack(Stack<string> searchTokens, string pKeyType, int searchLimit)
        {
            string formattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineFormattedTextPattern", @"^\s*\(*\s*@\w+\.\w+\s*");
            System.Text.RegularExpressions.Match tokenMatch;
            List<int> returnResolvedPKeys = new List<int>();

            // Bail out now if there are no tokens to process...
            if (searchTokens.Count < 1) return returnResolvedPKeys;

            do
            {
                string searchToken = searchTokens.Pop();
                tokenMatch = System.Text.RegularExpressions.Regex.Match(searchToken, formattedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

//if (searchToken.Trim().StartsWith("@") ||
//    searchToken.Trim().StartsWith("(@"))
                    if (tokenMatch.Success)
                {
                    returnResolvedPKeys.AddRange(ResolveSearchToken(searchToken.Trim(), pKeyType));
                }
                else if (searchToken.Trim().ToUpper() == "AND")
                {
                    if (searchTokens.Count > 0)
                    {
                        returnResolvedPKeys = returnResolvedPKeys.Intersect(ResolveSearchTokenStack(searchTokens, pKeyType, searchLimit)).ToList();
                    }
                }
                else if (searchToken.Trim().ToUpper() == "OR")
                {
                    if (searchTokens.Count > 0)
                    {
                        returnResolvedPKeys = returnResolvedPKeys.Union(ResolveSearchTokenStack(searchTokens, pKeyType, searchLimit)).ToList();
                    }
                }
                else if (searchToken == "(")
                {
                    // Found a grouping parenthesis start - so now find the ending parenthesis and process 
                    // everything in between as a collection for resolving to PKEYS...
                    returnResolvedPKeys.AddRange(ResolveSearchTokenStack(BuildSubGroupSearchTokenStack(searchTokens), pKeyType, searchLimit));
                }

            } while (searchTokens.Count > 0);

            return returnResolvedPKeys;
        }

        private List<int> ResolveSearchToken(string searchToken, string pKeyType)
        {
            List<int> resolvedPKeys = new List<int>();

            // Step 1 - gather all of the needed resolver SQL statements...
            DataSet resolverList = _sd.GetData("get_search_resolvers", ":resolvertype = " + pKeyType + "; :databasetype = " + _dm.DataConnectionSpec.EngineName.Trim().ToLower(), 0, 0);

            string tableName = "";
            if(searchToken.Trim().StartsWith("@"))
            {
                tableName = searchToken.Substring(1, searchToken.IndexOf('.') - 1).ToUpper();
            }
            else if(searchToken.Trim().StartsWith("(@"))
            {
                tableName = searchToken.Substring(2, searchToken.IndexOf('.') - 2).ToUpper();
            }

            // Step 2 - compare the resolver's target table against the table/field being searched to find the right resolver...
            if (!string.IsNullOrEmpty(tableName) && resolverList.Tables.Contains("get_search_resolvers"))
            {
                foreach (DataRow dr in resolverList.Tables["get_search_resolvers"].Rows)
                {
                    if (dr["table_name"].ToString().ToUpper() == tableName)
                    {
                        // Found the resolver for this table.field, so start a search on the database using the SQL 
                        // for this table.field resolver...
                        if (dr["sql_statement"].ToString().Contains("__searchcriteria__"))
                        {
                            DataSet searchResults = new DataSet();

                            string sql = BuildSearchSQLStatement(dr["sql_statement"].ToString(), "__searchcriteria__", searchToken, _dm.DataConnectionSpec.EngineName); 
                            searchResults = _dm.Read(sql, searchResults, "SearchResults");

                            if (searchResults.Tables.Count == 1 &&
                                searchResults.Tables.Contains("SearchResults") &&
                                searchResults.Tables["SearchResults"].Columns.Count == 1 /*&&
                                searchResults.Tables["SearchResults"].Columns[pKeyType].DataType == typeof(int)*/)
                            {
                                int pkey;
                                foreach (DataRow drPKey in searchResults.Tables["SearchResults"].Rows)
                                {
                                    if (int.TryParse(drPKey[pKeyType].ToString(), out pkey))
                                    {
                                        resolvedPKeys.Add(pkey);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return resolvedPKeys;
        }

        private string BuildSearchSQLStatement(string sqlStatement, string replacementString, string searchToken, string dbEngine)
        {
            string sql = "";
            switch (dbEngine.ToLower())
            {
                case "sqlserver":
                case "mysql":
                case "postgresql":
                case "unknown":
                    sql = sqlStatement.Replace(replacementString, " " + searchToken.Replace("@", "") + " ");
                    break;
                case "oracle":
                    // We need to handle the CLOB object different (because Oracle does not tranparently search large objects)...
                    // First get the collection of table fields that are CLOBS...
                    string scrubbedSearchToken = searchToken;
                    string CLOBFieldSQL = @"
SELECT 
       sys_table_field_id
      ,(select table_name from sys_table st where st.sys_table_id = stf.sys_table_id) as table_name
      ,field_name
      ,field_purpose
      ,field_type
      ,default_value
      ,is_primary_key
      ,is_foreign_key
      ,foreign_key_table_field_id
      ,foreign_key_dataview_name
      ,is_nullable
      ,gui_hint
      ,is_readonly
      ,min_length
      ,max_length
      ,numeric_precision
      ,numeric_scale
      ,is_autoincrement
      ,group_name
      ,created_date
      ,created_by
      ,modified_date
      ,modified_by
      ,owned_date
      ,owned_by
FROM sys_table_field stf
WHERE field_purpose = 'DATA'
AND field_type = 'STRING'
AND max_length = -1
";
                    DataSet CLOBFields = new DataSet();
                    CLOBFields = _dm.Read(CLOBFieldSQL, CLOBFields, "CLOBFields");
                    if (CLOBFields != null &&
                        CLOBFields.Tables.Contains("CLOBFields") &&
                        CLOBFields.Tables["CLOBFields"].Rows.Count > 0)
                    {
                        foreach (DataRow dr in CLOBFields.Tables["CLOBFields"].Rows)
                        {
                            string tableFieldName = dr["table_name"].ToString().Trim().ToLower() + "." + dr["field_name"].ToString().Trim().ToLower();
                            // We need to wrap the CLOB field in a conversion function to convert it to a string because Oracle cannot directly
                            // search a CLOB (BTW...  we only convert the first 32K of text for searching)...
                            scrubbedSearchToken = scrubbedSearchToken.Replace("@" + tableFieldName, "dbms_lob.substr(" + tableFieldName + ", 32767, 1)");
                            // Oracle does not allow the '=' comparison operator to be used on a CLOB - we must use 'like' instead (even if the search is for an exact match)...
                            scrubbedSearchToken = System.Text.RegularExpressions.Regex.Replace(scrubbedSearchToken, @"dbms_lob\.substr\(" + tableFieldName + @", 32767, 1\)\s*=\s*", @"dbms_lob.substr(" + tableFieldName + @", 32767, 1) like ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        }
                    }
                    sql = sqlStatement.Replace(replacementString, " " + scrubbedSearchToken.Replace("@", "") + " ");
                    break;
                default:
                    sql = sqlStatement.Replace(replacementString, " " + searchToken.Replace("@", "") + " ");
                    break;
            }

            // Finally before returning the SQL query, put an N in front of any quoted strings in the query so that those strings are treated as literal string...
//string quotedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineQuotedTextPattern", @"'[^\f\n\r\v]+'");
string quotedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineQuotedTextPattern", @"'[\w\s'\.,/?!#$%^&*-]+'");
            sql = System.Text.RegularExpressions.Regex.Replace(sql, quotedTextPattern, new System.Text.RegularExpressions.MatchEvaluator(InternationalizeQuotedText));
            //sql = System.Text.RegularExpressions.Regex.Replace(sql, @"'[^\f\n\r\v']+'", new System.Text.RegularExpressions.MatchEvaluator(InternationalizeQuotedText));

            return sql;
        }

        private static string InternationalizeQuotedText(System.Text.RegularExpressions.Match quotedText)
        {
            string internationalizedQuotedText = quotedText.Value;
            return "N" + internationalizedQuotedText;
        }

        private Stack<string> BuildSubGroupSearchTokenStack(Stack<string> searchTokens)
        {
            Stack<string> subgroupSearchTokens = new Stack<string>();

            int groupingNestCount = 1;
            // This should be the first token after the subgroup opening parenthesis...
            string searchToken = searchTokens.Peek();

            do
            {
                // Get the next token in the stack...
                searchToken = searchTokens.Pop();
                if (searchToken.Trim() == "(")
                {
                    // If it is another grouping, increment the grouping nest counter...
                    groupingNestCount++;
                }
                else if (searchToken.Trim() == ")")
                {
                    // If it is the end of a grouping, decrement the grouping nest counter...
                    groupingNestCount--;
                }
                // Add the token to the new stack...
                subgroupSearchTokens.Push(searchToken);
            } while (groupingNestCount > 0);
            // Consume the ending grouping parenthesis (because the beginning parenthesis is already consumed)
            subgroupSearchTokens.Pop();

            // We need to reverse the order - the easiest way to do this is to create a new stack 
            // using an enumeration of the original stack as shown below...
            subgroupSearchTokens = new Stack<string>(subgroupSearchTokens);

            return subgroupSearchTokens;
        }

        private Dictionary<string, string> BuildSearchTokenDictionary(Stack<string> searchTokens)
        {
            Dictionary<string, string> searchTokenDictionary = new Dictionary<string, string>();
            string operatorContext = "";
            bool foundEndOfSubgroup = false;
            bool foundNewSubgroup = false;

            if (searchTokens.Count > 0)
            {
                do
                {
                    string tempToken = searchTokens.Pop();
                    switch (tempToken.Trim().ToUpper())
                    {
                        case "(":
                            Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
                            searchTokens.Push("(");
                            if (!string.IsNullOrEmpty(operatorContext)) searchTokens.Push(operatorContext);
                            //operatorContext = "";
                            foundNewSubgroup = true;
                            break;
                        case ")":
                            if (string.IsNullOrEmpty(operatorContext))
                            {
                                foreach (string key in searchTokenDictionary.Keys.ToList())
                                {
                                    if (searchTokenDictionary[key].StartsWith("AND "))
                                    {
                                        searchTokenDictionary[key] = "AND (" + searchTokenDictionary[key].Substring(3).Trim() + ")";
                                    }
                                    else if (searchTokenDictionary[key].StartsWith("OR "))
                                    {
                                        searchTokenDictionary[key] = "OR (" + searchTokenDictionary[key].Substring(2).Trim() + ")";
                                    }
                                    else
                                    {
                                        searchTokenDictionary[key] = "(" + searchTokenDictionary[key] + ")";
                                    }
                                }
                                foundEndOfSubgroup = true;
                            }
                            break;
                        case "AND":
                            operatorContext += tempToken + " ";
                            break;
                        case "OR":
                            operatorContext += tempToken + " ";
                            break;
                        default:
                            string tableName = tempToken.Substring(1, tempToken.IndexOf('.') - 1).ToUpper();
                            if (searchTokenDictionary.ContainsKey(tableName))
                            {
                                searchTokenDictionary[tableName] = searchTokenDictionary[tableName] + operatorContext + tempToken + " ";
                            }
                            else
                            {
                                searchTokenDictionary.Add(tableName, operatorContext + tempToken + " ");
                            }
                            operatorContext = "";
                            break;
                    }
                } while (searchTokens.Count > 0 && !foundEndOfSubgroup && !foundNewSubgroup);
            }

            return searchTokenDictionary;
        }

        private Stack<string> ConsolidateSearchTokens(Stack<string> searchTokens)
        {
            Stack<string> consolidatedSearchTokenStack = new Stack<string>();
            Dictionary<string, string> consolidatedSearchTokenDictionary = new Dictionary<string,string>();
            string operatorContext = "";

            do
            {
                string currentToken = searchTokens.Pop().Trim();
                switch (currentToken.Trim().ToUpper())
                {
                    case "(":
                        Stack<string> subgroupStack = BuildSubGroupSearchTokenStack(searchTokens);
                        Stack<string> concatenatedStack = new Stack<string>();
                        if (consolidatedSearchTokenStack.Count > 0)
                        {
                            // Before pushing existing consolidated stack on to new (concatenating) stack - invert the 
                            // consolidated stack so that it is concatenated in the order it will eventually be processed in...
                            consolidatedSearchTokenStack = new Stack<string>(consolidatedSearchTokenStack);
                            // Now start to build the new concatenation stack... 
                            do
                            {
                                concatenatedStack.Push(consolidatedSearchTokenStack.Pop());
                            } while (consolidatedSearchTokenStack.Count > 0);
                            // Apply the boolean operator to the new grouping...
                            if (!string.IsNullOrEmpty(operatorContext))
                            {
                                concatenatedStack.Push(operatorContext);
                                operatorContext = "";
                            }
                        }
                        // Create the new sub-group stack...
                        Stack<string> concatSubgroupStack = ConsolidateSearchTokens(subgroupStack);
                        // Now concatenate it to the existing stack...
                        if (concatSubgroupStack.Count > 0)
                        {
                            concatenatedStack.Push("(");
                            foreach (string searchToken in concatSubgroupStack)
                            {
                                concatenatedStack.Push(searchToken);
                            }
                            concatenatedStack.Push(")");
                        }
                        consolidatedSearchTokenStack = concatenatedStack;
                        break;
                    //case ")":
                    //    break;
                    case "AND":
                        operatorContext = currentToken + " ";
                        break;
                    case "OR":
                        operatorContext = currentToken + " ";
                        break;
                    default:
                        // Processing for a string of multiple formatted parameters...
                        // Re-add the current token to the stack...
                        searchTokens.Push(currentToken);
                        // Send the stack to the dictionary builder (that method will stop at the end of the stack 
                        // or the next grouping begining point (aka opening paranthesis)...
                        consolidatedSearchTokenDictionary = BuildSearchTokenDictionary(searchTokens);
                        // Add the dictionary entries to the existing consolidated stack...
                        foreach (string searchToken in consolidatedSearchTokenDictionary.Values)
                        {
                            string formattedSearchToken = "";
                            if (searchToken.StartsWith("AND "))
                            {
                                consolidatedSearchTokenStack.Push("AND");
                                formattedSearchToken = searchToken.Substring(3).Trim();
                            }
                            else if (searchToken.StartsWith("OR "))
                            {
                                consolidatedSearchTokenStack.Push("OR");
                                formattedSearchToken = searchToken.Substring(2).Trim();
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(operatorContext))
                                {
                                    consolidatedSearchTokenStack.Push(operatorContext);
                                    //operatorContext = "";
                                }
                                formattedSearchToken = searchToken.Trim();
                            }

                            consolidatedSearchTokenStack.Push(formattedSearchToken);
                        }
                        break;
                }
            } while (searchTokens.Count > 0);

            // We need to reverse the order - the easiest way to do this is to create a new stack 
            // using an enumeration of the original stack as shown below...
            consolidatedSearchTokenStack = new Stack<string>(consolidatedSearchTokenStack);

            return consolidatedSearchTokenStack;
        }

        private Stack<string> BuildSearchTokenStack(string rawText, string defaultBooleanOperator)
        {
//string formattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineFormattedTextPattern", @"^\s*@\w+\.\w+\s*(?:<>|<=|>=|=|<|>)\s*(?:\d+|'[^\f\n\r\v']+')\s*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*(?:\d+|'[^\f\n\r\v']+')\s*[,|\)])*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:like|LIKE)\s+'[^\f\n\r\v']+'\s*|^\s*@\w+\.\w+\s+(?:is|IS)\s+(?:not\s+|NOT\s+)*(?:null|NULL)\s*");
string formattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineFormattedTextPattern", @"^\s*@\w+\.\w+\s*(?:<>|<=|>=|=|<|>)\s*(?:\d+|'[\w\s'\.,/?!#$%^&*-]+')\s*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*(?:\d+|'[\w\s'\.,/?!#$%^&*-]+')\s*[,|\)])*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:like|LIKE)\s+'[\w\s'\.,/?!#$%^&*-]+'\s*|^\s*@\w+\.\w+\s+(?:is|IS)\s+(?:not\s+|NOT\s+)*(?:null|NULL)\s*");
            string booleanOperatorPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineBooleanOperatorPattern", @"^AND\s+|^and\s+|^OR\s+|^or\s+");
            string unformattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineUnformattedTextPattern", @"^\s*""[^\a\b\r\v\f\n\e""]+""(\s+|$)|^\s*'[^\a\b\r\v\f\n\e]+'(\s+|$)|^\s*(%|\*)*[\S%]+(%|\*)*(\s+|$)");

            Stack<string> tempTokenList = new Stack<string>();
            System.Text.RegularExpressions.Match tokenMatch;

            // Set the default boolean operation if it is empty or contains incorrect values...
            if (string.IsNullOrEmpty(defaultBooleanOperator) &&
                defaultBooleanOperator.Trim() != "AND" &&
                defaultBooleanOperator.Trim() != "OR")
            {
                defaultBooleanOperator = "AND";
            }

            while (rawText.Length > 0)
            {
                rawText = rawText.Trim();
                // First look for grouping parenthesis' in the first position of the search string...
                if (rawText[0] == '(')
                {
                    int groupingNestCount = 0;
                    // Looks like we found the start of a grouping parenthesis so now find the ending parenthesis...
                    for (int i = 0; i < rawText.Length; i++)
                    {
                        if (rawText[i] == '(')
                        {
                            // Found another grouping paranthesis - add it to the counter...
                            groupingNestCount++;
                        }
                        else if (rawText[i] == ')')
                        {
                            if (groupingNestCount > 1)
                            {
                                // Found the ending parenthesis for an inner grouping - subtract it from the counter...
                                groupingNestCount--;
                            }
                            else
                            {
                                // Found the ending parenthesis - time to do some work...
                                // First strip off the outermost grouping parenthesis' and feed the remaining string into this routine again...
                                string groupedText = rawText.Substring(1, i - 1); // this will remove the leading and trailing paranthesis
                                rawText = rawText.Substring(i + 1).Trim();
                                // Create a child (grouping) list of tokens...
                                tempTokenList.Push("(");
                                foreach (string token in BuildSearchTokenStack(groupedText, defaultBooleanOperator))
                                {
                                    tempTokenList.Push(token);
                                }
                                tempTokenList.Push(")");
                                break;
                            }
                        }
                    }
                }

                // Next look for a searchable string token...
                // This can take two forms: @table.field = 'blah' or 'raw' unformatted text...
                
                // Look for @table.field formatted search tokens first and fail-over to unformatted text...
                tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, formattedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (tokenMatch.Success)
                {
                    // Found an @table.field token
                    string scrubbedToken = tokenMatch.Value.Trim();
                    string quotedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineQuotedTextPattern", @"'[\w\s'\.,/?!#$%^&*-]+'");
                    System.Text.RegularExpressions.Match quotedTextMatch;
                    quotedTextMatch = System.Text.RegularExpressions.Regex.Match(scrubbedToken, quotedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (quotedTextMatch.Success)
                    {
                        string tempValue = quotedTextMatch.Value.Trim().TrimStart('\'').TrimEnd('\'');
                        while(tempValue.Contains("''")) tempValue = tempValue.Replace("''", "'");
                        scrubbedToken = scrubbedToken.Replace(quotedTextMatch.Value, "'" + tempValue.Replace("'", "''") + "'");                        
                    }
                    tempTokenList.Push(scrubbedToken);
                    // Get rid of the @table.field search token (and any leading/trailing white spaces)...
                    rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();
                }
                else
                {
                    // Before consuming the next token - make sure it is not a boolean operator...
                    System.Text.RegularExpressions.Match booleanTokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, booleanOperatorPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    // This is an unformatted token - format/expand this into standard @table.field form using default search autofields...
                    tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, unformattedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (!booleanTokenMatch.Success && tokenMatch.Success)
                    {
                        // Create a child (grouping) list of formatted tokens...
                        foreach (string token in FormatRawToken(tokenMatch.Value.Trim()))
                        {
                            tempTokenList.Push(token);
                        }
                        // Get rid of the unformatted search token...
                        rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();
                    }
                }
                // Finally look for a boolean operator at the beginning of the next string (to determine how 
                // matches should be combined between @table.field search results)...
                tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, booleanOperatorPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (tokenMatch.Success)
                {
                    tempTokenList.Push(tokenMatch.Value.Trim().ToUpper());
                    // Get rid of the boolean operator...
                    rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();
                }
                else
                {
                    // No boolean token was found so use the default boolean operator...
                    tempTokenList.Push(defaultBooleanOperator);
                    rawText = rawText.Trim();
                }
            }
            // Remove the last AND/OR in the stack (typically not necessary)...
            if (tempTokenList.Count > 0 &&
                (tempTokenList.Peek().ToString().Trim().ToUpper() == "AND" ||
                tempTokenList.Peek().ToString().Trim().ToUpper() == "OR"))
            {
                tempTokenList.Pop();
            }

            // Finally we need to reverse the order of the list to load into the stack...
            Stack<string> searchTokensStack = new Stack<string>(tempTokenList);

            return searchTokensStack;
        }

        public Stack<string> BuildListBasedSearchTokenStack(string itemList)
        {
            Stack<string> tempTokenList = new Stack<string>();
            string[] items = itemList.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                string scrubbedItem = ScrubText(item);
                string[] tokens = scrubbedItem.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int number = 0;
                switch (tokens.Length)
                {
                    case 1:
                        // Assume this is an order_request_id or a xxx_part1 item...
                        number = 0;
                        if (int.TryParse(tokens[0], out number))
                        {
                            tempTokenList.Push("(@order_request.order_request_id=" + number.ToString() + ")");
                            tempTokenList.Push("OR");
                        }
                        // Format token for wildcards if necessary...
                        if (tokens[0].Contains('%'))
                        {
                            tokens[0] = " LIKE '" + tokens[0].Trim();
                        }
                        else
                        {
                            tokens[0] = " = '" + tokens[0].Trim();
                        }
                        tempTokenList.Push("(@accession.accession_number_part1" + tokens[0] + "')");
                        tempTokenList.Push("OR");
                        tempTokenList.Push("(@inventory.inventory_number_part1" + tokens[0] + "')");
                        tempTokenList.Push("OR");
                        break;
                    case 2:
                        // Assume this is a xxx_part1 and xxx_part2 item...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) &&
                            int.TryParse(tokens[1], out number))
                        {
                            // Format token[0] for wildcards if necessary...
                            if (tokens[0].Contains('%'))
                            {
                                tokens[0] = " LIKE '" + tokens[0].Trim();
                            }
                            else
                            {
                                tokens[0] = " = '" + tokens[0].Trim();
                            }
                            tempTokenList.Push("(@accession.accession_number_part1" + tokens[0] + "' AND @accession.accession_number_part2=" + number.ToString() + ")");
                            tempTokenList.Push("OR");
                            tempTokenList.Push("(@inventory.inventory_number_part1" + tokens[0] + "' AND @inventory.inventory_number_part2=" + number.ToString() + ")");
                            tempTokenList.Push("OR");
                        }
                        break;
                    case 3:
                        // Assume this is a xxx_part1, xxx_part2 and xxx_part3 item...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) &&
                            int.TryParse(tokens[1], out number))
                        {
                            // Format token[0] for wildcards if necessary...
                            if (tokens[0].Contains('%'))
                            {
                                tokens[0] = " LIKE '" + tokens[0].Trim();
                            }
                            else
                            {
                                tokens[0] = " = '" + tokens[0].Trim();
                            }
                            // Format token[2] for wildcards if necessary...
                            if (tokens[2].Contains('%'))
                            {
                                tokens[2] = " LIKE '" + tokens[2].Trim();
                            }
                            else
                            {
                                tokens[2] = " = '" + tokens[2].Trim();
                            }
                            tempTokenList.Push("(@accession.accession_number_part1" + tokens[0] + "' AND @accession.accession_number_part2=" + number.ToString() + " AND @accession.accession_number_part3" + tokens[2] + "')");
                            tempTokenList.Push("OR");
                            tempTokenList.Push("(@inventory.inventory_number_part1" + tokens[0] + "' AND @inventory.inventory_number_part2=" + number.ToString() + " AND @inventory.inventory_number_part3" + tokens[2] + "')");
                            tempTokenList.Push("OR");
                        }
                        break;
                    case 4:
                        // Assume this is a xxx_part1, xxx_part2, xxx_part3 and xxx_part4 item (which can only be an inventory)...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) &&
                            int.TryParse(tokens[1], out number))
                        {
                            // Format token[0] for wildcards if necessary...
                            if (tokens[0].Contains('%'))
                            {
                                tokens[0] = " LIKE '" + tokens[0].Trim();
                            }
                            else
                            {
                                tokens[0] = " = '" + tokens[0].Trim();
                            }
                            // Format token[2] for wildcards if necessary...
                            if (tokens[2].Contains('%'))
                            {
                                tokens[2] = " LIKE '" + tokens[2].Trim();
                            }
                            else
                            {
                                tokens[2] = " = '" + tokens[2].Trim();
                            }
                            // Format token[3] for wildcards if necessary...
                            if (tokens[3].Contains('%'))
                            {
                                tokens[3] = " LIKE '" + tokens[3].Trim();
                            }
                            else
                            {
                                tokens[3] = " = '" + tokens[3].Trim();
                            }
                            tempTokenList.Push("(@inventory.inventory_number_part1" + tokens[0] + "' AND @inventory.inventory_number_part2=" + number.ToString() + " AND @inventory.inventory_number_part3" + tokens[2] + "' AND @inventory.form_type_code" + tokens[3] + "')");
                            tempTokenList.Push("OR");
                        }
                        break;
                    default:
                        // Ignore this item...
                        break;
                }
                // Format scrubbedItem for wildcards if necessary...
                if (scrubbedItem.Contains('%'))
                {
                    tempTokenList.Push("(@accession_inv_name.plant_name LIKE '" + scrubbedItem + "')");
                }
                else
                {
                    tempTokenList.Push("(@accession_inv_name.plant_name = '" + scrubbedItem + "')");
                }
                tempTokenList.Push("OR");
            }

            // Remove the last AND/OR in the stack (typically not necessary)...
            if (tempTokenList.Count > 0 &&
                (tempTokenList.Peek().ToString().Trim().ToUpper() == "AND" ||
                tempTokenList.Peek().ToString().Trim().ToUpper() == "OR"))
            {
                tempTokenList.Pop();
            }

            // Finally we need to reverse the order of the list to load into the stack...
            Stack<string> searchTokensStack = new Stack<string>(tempTokenList);

            return searchTokensStack;
        }

        private static string ScrubText(string item)
        {
            string scrubbedItem = item.Trim();
            if (scrubbedItem.StartsWith("\"") &&
                scrubbedItem.EndsWith("\""))
            {
                scrubbedItem = scrubbedItem.TrimEnd('"');
                scrubbedItem = scrubbedItem.TrimStart('"');
            }
            if (scrubbedItem.StartsWith("'") &&
                scrubbedItem.EndsWith("'"))
            {
                scrubbedItem = scrubbedItem.TrimEnd('\'');
                scrubbedItem = scrubbedItem.TrimStart('\'');
            }
            // If the user was smart enough to escape a single quote in the text - 'un-escape' the single quotes...
            while (scrubbedItem.Contains("''")) scrubbedItem = scrubbedItem.Replace("''", "'");
            // Now that there are only single quotes in the string re-escape them...
            scrubbedItem = scrubbedItem.Replace("'", "''");
            // Finally replace the * wildcard with the % wildcard...
            scrubbedItem = scrubbedItem.Replace('*', '%');

            return scrubbedItem;
        }

        private Stack<string> FormatRawToken(string rawToken)
        {
            Stack<string> formattedTokens = new Stack<string>();
            bool isInteger = false;
            bool isDecimal = false;
            bool isDateTime = false;
            //bool isCodeValue = false;

            // See what data types this token is convertible to...
            int integerToken = 0;
            isInteger = int.TryParse(rawToken, out integerToken);
            Decimal decimalToken = 0.0M;
            isDecimal = Decimal.TryParse(rawToken, out decimalToken);
            DateTime datetimeToken = DateTime.MinValue;
            isDateTime = DateTime.TryParse(rawToken, out datetimeToken);

            // Step 1 - get the list of 'auto-fields' to expand the raw text into...
            DataSet autoFields = _sd.GetData("get_search_autofields", "", 0, 0);

            // Step 2 - build the stack of search tokens...
            if (autoFields.Tables.Count == 2 &&
                autoFields.Tables.Contains("get_search_autofields"))
            {
                formattedTokens.Push("(");
                foreach (DataRow dr in autoFields.Tables["get_search_autofields"].Rows)
                {
                    switch (dr["field_type"].ToString().ToUpper())
                    {
                        case "INTEGER":
                            if (isInteger)
                            {
                                formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "=" + integerToken.ToString());
                                formattedTokens.Push("OR");
                            }
                            break;
                        case "DECIMAL":
                            if (isDecimal)
                            {
                                formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "=" + decimalToken.ToString());
                                formattedTokens.Push("OR");
                            }
                            break;
                        case "DATETIME":
                            if (isDateTime)
                            {
                                formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "='" + datetimeToken.ToString() + "'");
                                formattedTokens.Push("OR");
                            }
                            break;
                        case "STRING":
                            string scrubbedItem = ScrubText(rawToken);

//// Check to see if the string is enclosed in double quotes - if so treat it as a literal string...
//if (rawToken.Trim().StartsWith("\"") &&
//    rawToken.Trim().EndsWith("\""))
//{
//    // Remove the double quotes (used for treating the string as a literal)...
//    formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "='" + rawToken.Trim().TrimStart('"').TrimEnd('"') + "'");
//}
//// Check to see if the string is enclosed in single quotes - if so treat it as a literal string...
//else if (rawToken.Trim().StartsWith("'") &&
//    rawToken.Trim().EndsWith("'"))
//{
//    // Remove the double quotes (used for treating the string as a literal)...
//    formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "=" + rawToken.Trim());
//}
//// Check to see if the search token has wild card characters in it (if so use the like operator instead of the = operator)...
//else if (rawToken.Contains("%") ||
//    rawToken.Contains("*") ||
//    rawToken.Contains("_"))
//{
//    // Replace the * with the % wild card character...
//    rawToken = rawToken.Replace('*', '%');

//    // Create the formatted search token...
//    formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + " LIKE '" + rawToken.Trim() + "'");
//}
//else
//{
//    formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "='" + rawToken.Trim() + "'");
//}
                            if(scrubbedItem.Contains('%'))
                            {
                                // Create the formatted search token using the LIKE operator...
                                formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + " LIKE '" + scrubbedItem.Trim() + "'");
                            }
                            else
                            {
                                // Create the formatted search token using the = operator...
                                formattedTokens.Push("@" + dr["table_name"].ToString().ToLower() + "." + dr["field_name"].ToString().ToLower() + "='" + scrubbedItem.Trim() + "'");
                            }
                            formattedTokens.Push("OR");
                            break;
                        default:
                            break;
                    }
                }

                // Remove the last AND/OR in the stack (typically not necessary)...
                if (formattedTokens.Count > 0 &&
                    (formattedTokens.Peek().ToString().Trim().ToUpper() == "AND" ||
                    formattedTokens.Peek().ToString().Trim().ToUpper() == "OR"))
                {
                    formattedTokens.Pop();
                }
                formattedTokens.Push(")");
            }

            // Finally we need to reverse the order of the list to load into the stack...
            formattedTokens = new Stack<string>(formattedTokens);

            return formattedTokens;
        }



//
// ###
// ### DYNAMIC Search Engine section
// ###
// ### Kurt Endress
// ###
//
        public class KJoin {
            public string FromTableName;
            public string FromFieldName;
            public string ToTableName;
            public string ToFieldName;
            public string ExtraJoinCode;
            public KJoin(string fromTableName, string fromFieldName, string toTableName, string toFieldName, string extraJoinCode) {
                this.FromTableName = fromTableName;
                this.FromFieldName = fromFieldName;
                this.ToTableName = toTableName;
                this.ToFieldName = toFieldName;
                this.ExtraJoinCode = extraJoinCode;
            }

            public override string ToString() {
                string returnJoin;

                string fromField = FromTableName.Trim();

                if (fromField.Contains(" ")) {
                    int pos = fromField.IndexOf(" ");
                    fromField = fromField.Substring(pos,fromField.Length-pos);
                }
                fromField += "." + FromFieldName;

                string toField = ToTableName.Trim();
                if (toField.Contains(" ")) {
                    int pos = toField.IndexOf(" ");
                    toField = toField.Substring(pos, toField.Length - pos);
                }
                toField += "." + ToFieldName;

                returnJoin = "JOIN " + ToTableName;
                returnJoin += " ON " + toField + " = ";
                returnJoin += fromField;
                if (!string.IsNullOrEmpty(ExtraJoinCode)) {
                    returnJoin += " " + ExtraJoinCode;
                }
                return returnJoin;
            }
        }

        public class KMatrix {
            public List<string> sysTableList;
            public Dictionary<string,string> alias2Full;
            public KJoin[,] joinPathArray;

            // adds a direct link join to the matrix and computes all the indirect links
            public void addLink(string fromTable, string toTable, string nextTable, string fromField, string nextField, string extraJoinCode) {
                int fromId = -1;
                int toId = -1;
                for (int i = 0; i < this.sysTableList.Count; i++) {
                    if (this.sysTableList[i] == fromTable) { fromId = i; }
                    if (this.sysTableList[i] == toTable) { toId = i; }
                }
                if (fromId < 0 || toId < 0) return;
                if (this.joinPathArray[fromId, toId] != null) return;
                this.joinPathArray[fromId, toId] = new KJoin(fromTable, fromField, nextTable, nextField, extraJoinCode);

                // If something knows the direction to from table but not the to table add that
                for (int i = 0; i < sysTableList.Count; i++) {
                    if (this.joinPathArray[i, fromId] != null && this.joinPathArray[i, toId] == null) {
                        this.joinPathArray[i, toId] = this.joinPathArray[i, fromId];
                    }
                }

                // If we know how to get somewhere from the to but not the from fix it
                for (int i = 0; i < this.sysTableList.Count; i++) {
                    if (this.joinPathArray[toId, i] != null && this.joinPathArray[fromId, i] == null) {
                        this.joinPathArray[fromId, i] = this.joinPathArray[fromId, toId];
                    }
                }

                // combine info on all the ways we know to get to from whti all the plces we know to go from to
                for (int i = 0; i < this.sysTableList.Count; i++) {
                    if (this.joinPathArray[i, fromId] != null) {
                        for (int j = 0; j < this.sysTableList.Count; j++) {
                            if (this.joinPathArray[toId, j] != null) {
                                if (this.joinPathArray[i, j] == null) {
                                    this.joinPathArray[i, j] = this.joinPathArray[i, fromId];
                                }
                            }
                        }
                    }
                }
            }


        }


        public DataSet FindPKeysDynamic(string pKeyType, string searchText, string defaultOperator, int searchLimit)
        {
            DataSet searchResults = _sd.CreateReturnDataSet();

            // Set the default boolean operation if it is empty or contains incorrect values...
            defaultOperator = defaultOperator.Trim().ToUpper();
            if (string.IsNullOrEmpty(defaultOperator) ||
                (defaultOperator != "AND" &&
                defaultOperator != "OR" &&
                defaultOperator != "LIST"))
            {
                defaultOperator = "AND";
            }

            // Step 1 - check to see if we have a properly formatted query string...
            string formattedSearchStringCheck = searchText.Trim().ToUpper();
            if (formattedSearchStringCheck.StartsWith("AND ") ||
                formattedSearchStringCheck.EndsWith(" AND") ||
                formattedSearchStringCheck.StartsWith("OR ") ||
                formattedSearchStringCheck.EndsWith(" OR"))
            {
                _sd.AddExceptionToTable(new Exception("The format of the search string is invalid - cannot end with 'AND/OR'", null), true, searchResults);
                return searchResults;
            }
            int leftParenthesis = 0;
            int rightParenthesis = 0;
            for (int i = 0; i < formattedSearchStringCheck.Length; i++)
            {
                if (formattedSearchStringCheck[i] == '(') leftParenthesis++;
                if (formattedSearchStringCheck[i] == ')') rightParenthesis++;
            }
            if (leftParenthesis > rightParenthesis)
            {
                _sd.AddExceptionToTable(new Exception("The format of the search string is invalid - missing an ending parenthesis ')' in one of the groupings", null), true, searchResults);
                return searchResults;
            }
            else if (leftParenthesis < rightParenthesis)
            {
                _sd.AddExceptionToTable(new Exception("The format of the search string is invalid - missing a beginning parenthesis '(' in one of the groupings", null), true, searchResults);
                return searchResults;
            }

            List<int> pkeys;

            // Step 2 - break the searchText down into formatted search tokens (ex: @accession_name.plant_name like '%B73%')...
            Stack<string> searchTokens = new Stack<string>();
            string freeFormQuery = "";
            if (defaultOperator == "LIST") {
                pkeys = ResolveItemListDynamically(searchText, pKeyType);

            } else {
                searchTokens = BuildSearchTokenStackKFE(searchText, defaultOperator, ref freeFormQuery);


                // Step 3 - resolve the formatted search strings to the target PKEY type...

                //if (searchTokens.Count > 50) {
                //    pkeys = ResolveSearchTokenStack(searchTokens, pKeyType, searchLimit);
                //} else {
                //}

                pkeys = ResolveBothStacks(searchTokens, freeFormQuery, pKeyType, defaultOperator, searchLimit);
            }

            // Step 4 - convert the search results from a List<int> to a dataset...
            if (pkeys != null &&
                pkeys.Count > -1)
            {
                searchResults.Tables.Add("SearchResult");
                searchResults.Tables["SearchResult"].Columns.Add("ID", typeof(int));
                int stopCount = 0;
                if (searchLimit == 0)
                    stopCount = pkeys.Count;
                else
                    stopCount = Math.Min(searchLimit, pkeys.Count);
                for (int i = 0; i < stopCount; i++)
                {
                    searchResults.Tables["SearchResult"].Rows.Add(pkeys[i]);
                }
            }

            return searchResults;
        }


        // Resolve the free form tokens seperately from the @table.field criteria. Combine results if necessary

        private List<int> ResolveBothStacks(Stack<string> searchTokens, string freeForm, string pKeyType, string defaultOperator, int searchLimit) {
            int stackSize = searchTokens.Count();
            //List<int>  freeFormPKeys = ResolveFreeFormStatic(pKeyType, freeForm, defaultOperator, searchLimit);
            List<int> freeFormPKeys = ResolveFreeFormDynamic(pKeyType, freeForm, defaultOperator, searchLimit);
            List<int>  tableFieldPKeys = ResolveSearchTokenStackAllAtOnce(searchTokens, pKeyType, searchLimit);
            if (String.IsNullOrEmpty(freeForm)) {
                return tableFieldPKeys;
            } else if (stackSize < 1) {
                return freeFormPKeys;
            } else if (defaultOperator.Trim().ToUpper() == "OR") {
                return freeFormPKeys.Union(tableFieldPKeys).ToList();
            } else {
                return freeFormPKeys.Intersect(tableFieldPKeys).ToList();
            }
        }


        private List<int> ResolveFreeFormStaticV1(string pKeyType, string freeFormSearch, string defaultOperator, int searchLimit) {
            if (freeFormSearch.Length < 1) return new List<int>();

            Stack<string> searchTokens = new Stack<string>();
            searchTokens = BuildSearchTokenStack(freeFormSearch, defaultOperator);
            return ResolveSearchTokenStack(searchTokens, pKeyType, searchLimit);
        }

        /*
        private List<int> ResolveFreeFormStatic(string pKeyType, string freeFormSearch, string defaultOperator, int searchLimit) {
            if (freeFormSearch.Trim().Length < 1 ) return new List<int>();

            // Try a list search for IDS only and return that if there's a hit
            List<int> listpkeys = ResolveItemListDynamically(freeFormSearch, pKeyType, "ID");
            if (listpkeys.Count > 0) return listpkeys;

            // Try the list search with everything to pick up the plant names containing spaces for merging with rest of results
            listpkeys = ResolveItemListDynamically(freeFormSearch, pKeyType, "ALL");

            // Do the auto field searching
            Stack<string> searchTokens = new Stack<string>();
            searchTokens = BuildSearchTokenStack(freeFormSearch, defaultOperator);
            return ResolveSearchTokenStack(searchTokens, pKeyType, searchLimit).Union(listpkeys).ToList();
        }
        */

        private List<int> ResolveFreeFormDynamic(string pKeyType, string freeFormSearch, string defaultOperator, int searchLimit) {
            //List<int> returnResolvedPKeys = new List<int>();
            if (freeFormSearch.Trim().Length < 1) return new List<int>();

            // Try a list search for IDS only and return that if there's a hit
            List<int> listpkeys = ResolveItemListDynamically(freeFormSearch, pKeyType, "ID");
            if (listpkeys.Count > 0) return listpkeys;

            // Try the list search with everything to pick up the plant names containing spaces for merging with rest of results
            listpkeys = ResolveItemListDynamically(freeFormSearch, pKeyType, "ALL");
            //if (listpkeys.Count > 0 && System.Text.RegularExpressions.Regex.Match(freeFormSearch, @"\d").Success) return listpkeys;

            // Do the auto field searching
            return ResolveFreeFormString(pKeyType, freeFormSearch, defaultOperator, searchLimit).Union(listpkeys).ToList();
        }

        private List<int> ResolveFreeFormString(string pKeyType, string rawText, string defaultOperator, int searchLimit) {
            //string unformattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineUnformattedTextPattern", @"^\s*""[^\a\b\r\v\f\n\e""]+""(\s+|$)|^\s*'[^\a\b\r\v\f\n\e']+'(\s+|$)|^\s*(%|\*)*[\S%]+(%|\*)*(\s+|$)");
            string unformattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineUnformattedTextPattern", @"^\s*""([^\a\b\r\v\f\n\e""]|"""")+""(\s+|$)|^\s*'([^\a\b\r\v\f\n\e']|'')+'(\s+|$)|^\s*(%|\*)*[\S%]+(%|\*)*(\s+|$)");
            System.Text.RegularExpressions.Match tokenMatch;
            List<int> allPKeys = new List<int>();
            KMatrix sjm = new KMatrix();
            int tokCount = 0;

            // loop through tokens
            while (rawText.Length > 0) {
                rawText = rawText.Trim();
                tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, unformattedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (tokenMatch.Success) {
                    List<int> tokPKeys = ResolveTokenByAutoFields(pKeyType, tokenMatch.Value.Trim(), sjm, searchLimit);
                    tokCount += 1;
                    if (tokCount == 1) {
                        allPKeys = tokPKeys;
                    } else  if (defaultOperator.Trim().ToUpper() == "OR") {
                        allPKeys = allPKeys.Union(tokPKeys).ToList();
                    } else {
                        allPKeys = allPKeys.Intersect(tokPKeys).ToList();
                    }
                    //throw Library.CreateBusinessException(getDisplayMember("KFE", "DEBUG RFFS count: " + allPKeys.Count.ToString()));
                    rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();
                }
            }
            return allPKeys;
        }


        private List<int> ResolveTokenByAutoFields(string pKeyType, string rawToken, KMatrix sjm, int searchLimit) {
            List<int> resultPKeys = new List<int>();
            string searchToken = requoteForSQL(rawToken);
            string pKeyTable = pKeyType.Replace("_id", "");
            DataSet autoFields = _sd.GetData("get_search_autofields", "", 0, 0);

            foreach (DataRow dr in autoFields.Tables["get_search_autofields"].Rows) {
                string afTable = dr["table_name"].ToString().ToLower();
                //throw Library.CreateBusinessException(getDisplayMember("KFE", "DEBUG ResolveTokenByAutoFields from: " + fromTable + " TO: " + afTable + " afield: " + aField ));
                string sql = "SELECT " + pKeyTable + "." + pKeyType;
                List<string> neededTables = new List<string>();
                neededTables.Add(afTable);
                sql += generateFromClause(sjm, pKeyTable, "\r\nINNER ", neededTables);


                string afType = dr["field_type"].ToString().ToUpper();
                string oper = "=";
                if (afType == "STRING") {
                    rawToken = rawToken.Replace('*', '%');
                    if (rawToken.Contains("%")) {
                        oper = " LIKE ";
                    }
                    searchToken = requoteForSQL(rawToken);

                } else if (afType == "INTEGER") {
                    int integerToken = 0;
                    if (!int.TryParse(rawToken, out integerToken)) continue;
                    searchToken = integerToken.ToString();

                } else if (afType == "DECIMAL") {
                    Decimal decimalToken = 0.0M;
                    if (!Decimal.TryParse(rawToken, out decimalToken)) continue;
                    searchToken = decimalToken.ToString();

                } else if (afType == "DATETIME") {
                    DateTime datetimeToken = DateTime.MinValue;
                    if (!DateTime.TryParse(rawToken, out datetimeToken)) continue;
                    searchToken = datetimeToken.ToString();
                } else {
                    continue;
                }

                string afField = dr["field_name"].ToString().ToLower();
                sql += "\r\nWHERE " + afTable + "." + afField + oper + searchToken;

                // Internationalize quoted strings in the SQL
                string quotedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineQuotedTextPattern", @"'([^\f\n\r\v']|'')+'");
                sql = System.Text.RegularExpressions.Regex.Replace(sql, quotedTextPattern, new System.Text.RegularExpressions.MatchEvaluator(InternationalizeQuotedText));

                resultPKeys = resultPKeys.Union(selectPKeys(_dm, pKeyType, sql)).ToList();
            }
            return resultPKeys;
        }


        // for debug messages
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "SecureData", resourceName, null, defaultValue, substitutes);
        }


        private List<int> ResolveSearchTokenStackAllAtOnce(Stack<string> searchTokens, string pKeyType, int searchLimit) {
            KMatrix sjm = new KMatrix();
            return ResolveSearchTokenStackAllAtOnce(searchTokens, pKeyType, sjm, searchLimit);
        }



        private List<int> ResolveSearchTokenStackAllAtOnce(Stack<string> searchTokens, string pKeyType, KMatrix sjm, int searchLimit) {
            List<int> returnResolvedPKeys = new List<int>();
            if (searchTokens.Count < 1) return returnResolvedPKeys;

            int originalTokenCount = searchTokens.Count; // debug

            string pKeyTable = pKeyType.Replace("_id", "");
            string sql = "SELECT DISTINCT " + pKeyTable + "." + pKeyType; // +" FROM " + pKeyTable;

            // scan for tables and build where clause
            string whereClause = "\r\nWHERE";
            string joinType = "\r\nINNER ";
            List<string> neededTables = new List<string>();
            do {
                string searchToken = searchTokens.Pop();
                if (searchToken.Trim().StartsWith("@")) {
                    string tokenTable = searchToken.Substring(1, searchToken.IndexOf('.') - 1).ToLower();
                    if (!neededTables.Contains(tokenTable)) {
                        neededTables.Add(tokenTable);
                    }
                    searchToken = searchToken.Replace("@", "");
                } else if (searchToken == "OR") {
                    joinType = "\r\nLEFT ";
                }
                whereClause += " " + searchToken;
            } while (searchTokens.Count > 0);

            // remove the from table from the list of needed tables if present
            if (neededTables.Contains(pKeyTable)) {
                neededTables.Remove(pKeyTable);
            }

            if (neededTables.Count < 1) {
                sql += " FROM " + pKeyTable;

            } else { // going to need to add some joins for the criteria tables
                sql += generateFromClause(sjm, pKeyTable, joinType, neededTables);
            }
                  
            sql = sql + whereClause;

            // Internationalize quoted strings in the SQL
            string quotedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineQuotedTextPattern", @"'([^\f\n\r\v']|'')+'");
            sql = System.Text.RegularExpressions.Regex.Replace(sql, quotedTextPattern, new System.Text.RegularExpressions.MatchEvaluator(InternationalizeQuotedText));

            //if (originalTokenCount > 1) { throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "SQL Select = " + sql));}  // KFE debug
            //throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "SQL Select = " + sql));

            // execute the SQL Select and ready the PKs
            returnResolvedPKeys = selectPKeys(_dm, pKeyType, sql);
            
            //if (returnResolvedPKeys.Count < 1 && originalTokenCount > 1) {
            //    throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "DEBUG: Nothing found with SQL Select\r\n" + sql));
            //}

            return returnResolvedPKeys;
        }


        private string generateFromClause(KMatrix sjm, string fromTable, string joinType, List<string> joinTables) {
            string sql = " from " + fromTable;

            if (sjm == null || sjm.joinPathArray == null) initJoinPathArray(sjm);

            // Add joins for each of the tables 
            List<string> joinedTables = new List<string>();
            joinedTables.Add(fromTable);
            foreach (string nTable in joinTables) {
                string needed = nTable;
                if (sjm.alias2Full.ContainsKey(needed)) { needed = sjm.alias2Full[needed]; }
                if (!joinedTables.Contains(needed)) {
                    List<KJoin> joinPath = joinPathToTable(sjm, fromTable, needed);
                    // add each join in the path to the table unless it's already joined
                    foreach (KJoin nextJoin in joinPath) {
                        if (!joinedTables.Contains(nextJoin.ToTableName)) {
                            sql += joinType + nextJoin.ToString();
                            joinedTables.Add(nextJoin.ToTableName);
                        }
                    }
                }
            }

            return sql;
        }


        // executes a SQL select statement to creat a list of integer primary keys

        private List<int> selectPKeys(DataManager dm, string pKeyType, string sqlStatement) {
            List<int> resolvedPKeys = new List<int>();
            DataSet searchResults = new DataSet();
            searchResults = dm.Read(sqlStatement, searchResults, "SearchResults");
            if (searchResults.Tables.Count == 1 &&
                searchResults.Tables.Contains("SearchResults") &&
                searchResults.Tables["SearchResults"].Columns.Count == 1) {
                int pkey;
                foreach (DataRow drPKey in searchResults.Tables["SearchResults"].Rows) {
                    if (int.TryParse(drPKey[pKeyType].ToString(), out pkey)) {
                        resolvedPKeys.Add(pkey);
                    }
                }
            }
            return resolvedPKeys;
        }

        // executes a SQL select statement to creat a list of integer primary keys

        private List<int> addSelectPKeys(DataManager dm, List<int> pKeysList, string pKeyType, string sqlStatement) {
            DataSet searchResults = new DataSet();
            searchResults = dm.Read(sqlStatement, searchResults, "SearchResults");
            if (searchResults.Tables.Count == 1 &&
                searchResults.Tables.Contains("SearchResults") &&
                searchResults.Tables["SearchResults"].Columns.Count == 1) {
                int pkey;
                foreach (DataRow drPKey in searchResults.Tables["SearchResults"].Rows) {
                    if (int.TryParse(drPKey[pKeyType].ToString(), out pkey)) {
                        pKeysList.Add(pkey);
                    }
                }
            }
            return pKeysList;
        }

        private string requoteForSQL(string rawToken) {
		if (rawToken.Trim().StartsWith("\"") && rawToken.Trim().EndsWith("\"")) {
			rawToken = rawToken.Trim().TrimStart('"').TrimEnd('"').Replace("\"\"", "\"");
		} else if (rawToken.Trim().StartsWith("'") && rawToken.Trim().EndsWith("'")){
			rawToken = rawToken.TrimStart('\'').TrimEnd('\'').Replace("''", "'");
		}
		rawToken = "'" + rawToken.Replace("'", "''") + "'";
		return rawToken;
	}



        // new init join step matrix from dataview containing spanning tree joins

        private void initJoinPathArray(KMatrix sjm) {

            // pull in the prefered join links from dataview (including all extra links)
            DataSet spanningTreeJoinList = _sd.GetData("sys_matrix_input", "" + _dm.DataConnectionSpec.EngineName.Trim().ToLower(), 0, 0);
            if (!spanningTreeJoinList.Tables.Contains("sys_matrix_input")) return;
            
            sjm.sysTableList = new List<string>();
            sjm.alias2Full = new Dictionary<string,string>();
            foreach (DataRow dr in spanningTreeJoinList.Tables["sys_matrix_input"].Rows) {
                string parentTable = dr["parent_table"].ToString().Trim().ToLower();
                string childTable = dr["child_table"].ToString().Trim().ToLower();
                if (!sjm.sysTableList.Contains(parentTable)) {
                    sjm.sysTableList.Add(parentTable);
                    if (parentTable.Contains(" ")) {
                        string alias = parentTable.Substring(parentTable.IndexOf(" ")+1);
                        //throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "DEBUG: adding alias '" + alias + "' = '" + parentTable + "'"));
                        sjm.alias2Full.Add(alias,parentTable);
                    }
                }
                if (!sjm.sysTableList.Contains(childTable)) {
                    sjm.sysTableList.Add(childTable);
                    if (childTable.Contains(" ")) {
                        string alias = childTable.Substring(childTable.IndexOf(" ")+1);
                        sjm.alias2Full.Add(alias, childTable);
                    }
                }
            }

            sjm.joinPathArray = new KJoin[sjm.sysTableList.Count, sjm.sysTableList.Count];
            foreach (DataRow dr in spanningTreeJoinList.Tables["sys_matrix_input"].Rows) {
                string childTable = dr["child_table"].ToString().Trim().ToLower();
                string parentTable = dr["parent_table"].ToString().Trim().ToLower();
                string childField = dr["child_field"].ToString().Trim().ToLower();
                if (string.IsNullOrEmpty(childField)) { childField = parentTable + "_id"; }
                string parentField = dr["parent_field"].ToString().Trim().ToLower();
                if (string.IsNullOrEmpty(parentField)) {parentField = parentTable + "_id";}
                string extra = dr["extra_join_code"].ToString().Trim().ToLower();
                string command = dr["command"].ToString().Trim().ToLower();
                if (command == "map") {
                    sjm.addLink(parentTable, childTable, childTable, parentField, childField, extra);
                    sjm.addLink(childTable, parentTable, parentTable, childField, parentField, extra);
                }
                if (command == "replace") {
                    string destTable = dr["dest_table"].ToString().Trim().ToLower();
                    if (string.IsNullOrEmpty(destTable)) { destTable = parentTable; }
                    replaceStep(sjm.sysTableList, sjm.joinPathArray, childTable, destTable, parentTable, childField, parentField);
                    if (destTable == parentTable) {
                        replaceStep(sjm.sysTableList, sjm.joinPathArray, parentTable, childTable, childTable, parentField, childField);
                    }
                }
            }
        }


        private void addParentChildStep(KMatrix sjm, string parentTable, string childTable, string fieldName) {
            sjm.addLink(parentTable, childTable, childTable, fieldName, fieldName, null);
            sjm.addLink(childTable, parentTable, parentTable, fieldName, fieldName, null);
        }
        private void addParentChildStep(KMatrix sjm, string parentTable, string childTable, string parentField, string childField) {
            sjm.addLink(parentTable, childTable, childTable, parentField, childField, null);
            sjm.addLink(childTable, parentTable, parentTable, childField, parentField, null);
        }


        private void replaceStep(List<string> sysTableArray, KJoin[,] joinPathArray, string fromTable, string toTable, string nextTable, string fromField, string nextField) {
            int fromId = -1;
            int toId = -1;
            for (int i = 0; i < sysTableArray.Count; i++) {
                if (sysTableArray[i] == fromTable) { fromId = i; }
                if (sysTableArray[i] == toTable) { toId = i; }
            }
            //throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "DEBUG replacestep fromT: " + fromTable + " fID: " + fromId.ToString() + " tT: " + toTable + " tID: " + toId.ToString() + " nextT: " + nextTable + " fromF: " + fromField + " nextF: " + nextField));  // KFE debug
            if (fromId < 0 || toId < 0) return;
            joinPathArray[fromId, toId] = new KJoin(fromTable, fromField, nextTable, nextField, null);
        }

        //private List<KJoin> joinPathToTable(string[] sysTableArray, KJoin[,] joinPathArray, string fromTable, string toTable) {
        private List<KJoin> joinPathToTable(KMatrix sjm, string fromTable, string toTable) {
            List<KJoin> joinResult = new List<KJoin>();
            string sofar = fromTable;
            do {
                KJoin nextStep = nextJoinInPath(sjm, sofar, toTable);
                if (nextStep == null) break;
                joinResult.Add(nextStep);
                sofar = nextStep.ToTableName;
            } while (sofar != toTable);
            return joinResult;
        }


        private KJoin nextJoinInPath(KMatrix sjm, string fromTable, string toTable) {
            int fromId = -1;
            int toId = -1;
            for (int i = 0; i < sjm.sysTableList.Count; i++) {
                if (sjm.sysTableList[i] == fromTable) { fromId = i; }
                if (sjm.sysTableList[i] == toTable) { toId = i; }
            }
            if (fromId < 0 || toId < 0) return null;
            return sjm.joinPathArray[fromId, toId];
        }


        // new KFE parser
        private Stack<string> BuildSearchTokenStackKFE(string rawText, string defaultBooleanOperator, ref string freeFormJunk) {
            //string formattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineFormattedTextPattern", @"^\s*@\w+\.\w+\s*(?:<>|<=|>=|=|<|>)\s*(?:\d+|'[^\f\n\r\v']+')\s*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*(?:\d+|'[^\f\n\r\v']+')\s*[,|\)])*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:like|LIKE)\s+'[^\f\n\r\v']+'\s*|^\s*@\w+\.\w+\s+(?:is|IS)\s+(?:not\s+|NOT\s+)*(?:null|NULL)\s*");
            string formattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineFormattedTextPattern", @"^\s*@\w+\.\w+\s*(?:<>|<=|>=|=|<|>)\s*(?:\d+|'([^\f\n\r\v']|'')+')\s*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*(?:\d+|'([^\f\n\r\v']|'')+')\s*[,|\)])*|^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:like|LIKE)\s+'([^\f\n\r\v']|'')+'\s*|^\s*@\w+\.\w+\s+(?:is|IS)\s+(?:not\s+|NOT\s+)*(?:null|NULL)\s*");
            string booleanOperatorPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineBooleanOperatorPattern", @"^AND\s+|^and\s+|^OR\s+|^or\s+");
            string unformattedTextPattern = GrinGlobal.Core.Toolkit.GetSetting("SearchEngineUnformattedTextPattern", @"^\s*""[^\a\b\r\v\f\n\e""]+""(\s+|$)|^\s*'[^\a\b\r\v\f\n\e']+'(\s+|$)|^\s*(%|\*)*[\S%]+(%|\*)*(\s+|$)");

            Stack<string> tempTokenList = new Stack<string>();
            System.Text.RegularExpressions.Match tokenMatch;

            // If completely free form lets get out of here
            if (!rawText.Contains("@")) {
                freeFormJunk = rawText;
                return tempTokenList;
            }

            // Set the default boolean operation if it is empty or contains incorrect values...
            if (string.IsNullOrEmpty(defaultBooleanOperator) &&
                defaultBooleanOperator.Trim() != "AND" &&
                defaultBooleanOperator.Trim() != "OR") {
                defaultBooleanOperator = "AND";
            }

            while (rawText.Length > 0) {
                rawText = rawText.Trim();
                // First look for grouping parenthesis' in the first position of the search string...
                if (rawText[0] == '(') {
                    int groupingNestCount = 0;
                    // Looks like we found a the start of a grouping parenthesis so now find the ending parenthesis...
                    for (int i = 0; i < rawText.Length; i++) {
                        if (rawText[i] == '(') {
                            // Found another grouping paranthesis - add it to the counter...
                            groupingNestCount++;
                        } else if (rawText[i] == ')') {
                            if (groupingNestCount > 1) {
                                // Found the ending parenthesis for an inner grouping - subtract it from the counter...
                                groupingNestCount--;
                            } else {
                                // Found the ending parenthesis - time to do some work...
                                // First strip off the outermost grouping parenthesis' and feed the remaining string into this routine again...
                                string groupedText = rawText.Substring(1, i - 1); // this will remove the leading and trailing paranthesis
                                rawText = rawText.Substring(i + 1).Trim();
                                // Create a child (grouping) list of tokens...
                                tempTokenList.Push("(");
                                foreach (string token in BuildSearchTokenStack(groupedText, defaultBooleanOperator)) {
                                    tempTokenList.Push(token);
                                }
                                tempTokenList.Push(")");
                                break;
                            }
                        }
                    }
                }

                // Next look for a searchable string token...
                // This can take two forms: @table.field = 'blah' or 'raw' unformatted text...

                // Look for @table.field formatted search tokens first and fail-over to unformatted text...
                //tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, @"^\s*@\w+\.\w*\s*(?:=|<|>|<>|<=|>=)\s*\S*\s*|^\s*@\w+\.\w*\s+(?:in|IN|not in|NOT IN)\s*\((?:\s*\S*\s*[,|\)])*|^\s*@\w+\.\w*\s+(?:like|LIKE|not like|NOT LIKE)\s+\S*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, formattedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                int junkcnt = 0;
                if (tokenMatch.Success) {
                    // Found an @table.field token
                    tempTokenList.Push(tokenMatch.Value.Trim());
                    // Get rid of the @table.field search token (and any leading/trailing white spaces)...
                    rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();
                    //throw Library.CreateBusinessException(getDisplayMember("DEBUGKFE", "found this @token: " + tokenMatch.Value.Trim()));  // KFE debug
                } else {
                    // Before consuming the next token - make sure it is not a boolean operator...
                    System.Text.RegularExpressions.Match booleanTokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, booleanOperatorPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    // This is an unformatted token - format/expand this into standard @table.field form using default search autofields...
                    tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, unformattedTextPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (!booleanTokenMatch.Success && tokenMatch.Success) {
                        //throw Library.CreateBusinessException(getDisplayMember("hasPermission{nomask}", "found this rawtoken " + tokenMatch.Value.Trim()));  // KFE debug

                        //Let's throw this on the trash heap
                        if (String.IsNullOrEmpty(freeFormJunk)) {
                            freeFormJunk += tokenMatch.Value.Trim();
                        } else {
                            freeFormJunk += ' ' + tokenMatch.Value.Trim();
                        }
                        junkcnt += 1;
                        // Create a child (grouping) list of formatted tokens...
                        //foreach (string token in FormatRawToken(tokenMatch.Value.Trim())) {
                        //    tempTokenList.Push(token);
                        //}
                        // Get rid of the unformatted search token...
                        rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();


                        //continue; // jump to next item
                    }
                }
                // Finally look for a boolean operator at the beginning of the next string (to determine how 
                // matches should be combined between @table.field search results)...
                tokenMatch = System.Text.RegularExpressions.Regex.Match(rawText, booleanOperatorPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                string boolOp = defaultBooleanOperator;
                if (tokenMatch.Success) {
                    boolOp = tokenMatch.Value.Trim().ToUpper();
                    // Get rid of the boolean operator...
                    rawText = rawText.Remove(0, tokenMatch.Index + tokenMatch.Length).Trim();
                }
                // only add operator if this wasn't a free form token
                if (junkcnt < 1) {
                    tempTokenList.Push(boolOp);
                    rawText = rawText.Trim();
                }

            }
            // Remove the last AND/OR in the stack (typically not necessary)...
            if (tempTokenList.Count > 0 &&
                (tempTokenList.Peek().ToString().Trim().ToUpper() == "AND" ||
                tempTokenList.Peek().ToString().Trim().ToUpper() == "OR")) {
                tempTokenList.Pop();
            }

            // Finally we need to reverse the order of the list to load into the stack...
            Stack<string> searchTokensStack = new Stack<string>(tempTokenList);

            return searchTokensStack;
        }

        List<int> ResolveItemListDynamically(string itemList, string pKeyType) {
            return ResolveItemListDynamically(itemList, pKeyType, "ALL");
        }

        List<int> ResolveItemListDynamically(string itemList, string pKeyType, string searchType) {
            List<int> acids = new List<int>();
            List<int> ivids = new List<int>();
            List<int> pnids = new List<int>();
            List<int> orids = new List<int>();
            string sql;

            // split on and loop thru the lines
            string[] items = itemList.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items) {

                int preCaseCount = acids.Count + ivids.Count + orids.Count;
                // splitting into tokens
                string[] tokens = item.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int number = 0;
                switch (tokens.Length) {
                    case 1:
                        // Assume this is an order_request_id
                        number = 0;
                        if (int.TryParse(tokens[0], out number) && searchType != "ID") {
                            sql = "SELECT order_request_id FROM order_request WHERE order_request.order_request_id=" + number.ToString() + " ";
                            orids = addSelectPKeys(_dm, orids, "order_request_id", sql);
                        }
                        break;
                    case 2:
                        // Assume this is a xxx_part1 and xxx_part2 item...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) && int.TryParse(tokens[1], out number)) {
                            tokens[0] = requoteForSQL(tokens[0]);
                            sql = "SELECT accession_id FROM accession WHERE accession.accession_number_part1=" + tokens[0] + " AND accession.accession_number_part2=" + number.ToString();
                            acids = addSelectPKeys(_dm, acids, "accession_id", sql);
                            sql = "SELECT inventory_id FROM inventory WHERE inventory.inventory_number_part1=" + tokens[0] + " AND inventory.inventory_number_part2=" + number.ToString();
                            ivids = addSelectPKeys(_dm, ivids, "inventory_id", sql);
                        }
                        break;
                    case 3:
                        // Assume this is a xxx_part1, xxx_part2 and xxx_part3 item...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) && int.TryParse(tokens[1], out number)) {
                            tokens[0] = requoteForSQL(tokens[0]);
                            tokens[2] = requoteForSQL(tokens[2]);
                            sql = "SELECT accession_id FROM accession WHERE accession.accession_number_part1=" + tokens[0] + " AND accession.accession_number_part2=" + number.ToString() + " AND accession.accession_number_part3=" + tokens[2];
                            acids = addSelectPKeys(_dm, acids, "accession_id", sql);
                            sql = "SELECT inventory_id FROM inventory WHERE inventory.inventory_number_part1=" + tokens[0] + " AND inventory.inventory_number_part2=" + number.ToString() + " AND inventory.inventory_number_part3=" + tokens[2];
                            ivids = addSelectPKeys(_dm, ivids, "inventory_id", sql);
                        }
                        break;
                    case 4:
                        // Assume this is a xxx_part1, xxx_part2, xxx_part3 and xxx_part4 item (which can only be an inventory)...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) && int.TryParse(tokens[1], out number)) {
                            sql = "SELECT inventory_id FROM inventory WHERE inventory.inventory_number_part1=" + requoteForSQL(tokens[0])
                                + " AND inventory.inventory_number_part2=" + number.ToString()
                                + " AND inventory.inventory_number_part3=" + requoteForSQL(tokens[2])
                                + " AND inventory.form_type_code=" + requoteForSQL(tokens[3]);
                            ivids = addSelectPKeys(_dm, ivids, "inventory_id", sql);
                        }
                        break;
                    default:
                        // Ignore this item...
                        break;
                }
                int postCaseCount = acids.Count + ivids.Count + orids.Count;
                if (postCaseCount == preCaseCount && searchType != "ID") {
                    // maybe the entire line is a plant name
                    string quotedLine = requoteForSQL(item);
                    //throw Library.CreateBusinessException(getDisplayMember("KFE", "DEBUG list looking for lant_name: " + quotedLine));
                    sql = "SELECT accession_inv_name_id FROM accession_inv_name WHERE plant_name = " + quotedLine;
                    pnids = addSelectPKeys(_dm, pnids, "accession_inv_name_id", sql);
                }
            }

            // Convert to resolve pKeyType
            List<int> pKeyIds = new List<int>();
            if (pnids.Count() > 0) {
                Stack<string> singleStack = new Stack<string>();
                singleStack.Push("@accession_inv_name.accession_inv_name_id IN (" + idListToString(pnids) + ")");
                List<int> pnpkeys = ResolveSearchTokenStackAllAtOnce(singleStack, pKeyType, 2);
                pKeyIds = pKeyIds.Union(pnpkeys).ToList();
            }

            if (acids.Count() > 0) {
                Stack<string> singleStack = new Stack<string>();
                singleStack.Push("@accession.accession_id IN (" + idListToString(acids) + ")");
                List<int> acpkeys = ResolveSearchTokenStackAllAtOnce(singleStack, pKeyType, 2);
                pKeyIds = pKeyIds.Union(acpkeys).ToList();
            }
            if (ivids.Count() > 0) {
                Stack<string> singleStack = new Stack<string>();
                singleStack.Push("@inventory.inventory_id IN (" + idListToString(ivids) + ")");
                List<int> ivpkeys = ResolveSearchTokenStackAllAtOnce(singleStack, pKeyType, 2);
                pKeyIds = pKeyIds.Union(ivpkeys).ToList();
            }
            if (orids.Count() > 0) {
                Stack<string> singleStack = new Stack<string>();
                singleStack.Push("@order_request.order_request_id IN (" + idListToString(orids) + ")");
                //throw Library.CreateBusinessException(getDisplayMember("KFE", "DEBUG generated order token = " + singleStack.Peek().ToString()));
                List<int> orpkeys = ResolveSearchTokenStackAllAtOnce(singleStack, pKeyType, 2);
                pKeyIds = pKeyIds.Union(orpkeys).ToList();
            }

            return pKeyIds;
        }


        static string idListToString(List<int> idList) {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < idList.Count; i++) {
                builder.Append(idList[i].ToString());
                if (i != idList.Count - 1) builder.Append(",");
            }
            return builder.ToString();
        }

    }
}
