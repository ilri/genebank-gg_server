using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data;

namespace GrinGlobal.Search.Engine {
	public class SearchCommand<TKey, TValue> : IDisposable
		where TKey : ISearchable<TKey>, new()
		where TValue : IPersistable<TValue>, new()

	{

        ///// <summary>
        ///// Gets the Hits as a result of the command being run
        ///// </summary>
        //public List<Hit> HitList { get; internal set; }


        ///// <summary>
        ///// All resolved ids for this command from all indexes that this command touches
        ///// </summary>
        //private List<int> _resolvedIDs;

        /// <summary>
        /// Resolved ids by index for this command
        /// </summary>
        private Dictionary<short, List<int>> _resolvedIDsByIndex;

        private Dictionary<short, List<Hit>> _hitsByIndex;

		/// <summary>
		/// Gets or sets the word for which we are to search
		/// </summary>
		public TKey Keyword;

		private string _fieldName;
		/// <summary>
		/// Gets or sets the field we should restrict hits to.  i.e. if FieldName='city' and we find a hit on a record in the 'last_name' field, that hit will not be returned (since it was in the wrong field)
		/// </summary>
        public string FieldName { 
            get { 
                return _fieldName; 
            } 
            set { 
                _fieldName = value;
            }
        }

        /// <summary>
        /// Gets or sets the passthru level to use when deciding if the query should call out to the database or not.
        /// </summary>
        internal DatabasePassthruLevel PassthruLevel { get; set; }

        /// <summary>
        /// Gets or sets if this command should try to auto-lookup coded values
        /// </summary>
        internal bool LookupCodedValues { get; set; }

        /// <summary>
        /// Gets or sets which language id command should use.  0 or negative means use all, positive value means use that specific value.
        /// </summary>
        internal int LanguageID { get; set; }

        /// <summary>
        /// Gets or sets if any cached data should be ignored.  Default is false.
        /// </summary>
        internal bool IgnoreCache { get; set; }

        /// <summary>
        /// Gets or sets if data was read from the database (if so, should not cache the search result!)
        /// </summary>
        internal bool ReadFromDatabase { get; set; }

        /// <summary>
        /// Used when FieldName is specified with dot notation. Example query:     @accession_name.name B73
        /// </summary>
        internal string RestrictToIndex { get; private set; }

        /// <summary>
        /// Returns true if command is allowed to query database to perform a non-equal comparison (&lt; &gt; &lt;&gt; !=)
        /// </summary>
        /// <returns></returns>
        internal bool IsAllowedToQueryComparisons() {
            return (this.PassthruLevel & DatabasePassthruLevel.Comparison) == DatabasePassthruLevel.Comparison;
        }

        /// <summary>
        /// Returns true if command is allowed to query database for fields that do not exist in the search engine index
        /// </summary>
        /// <returns></returns>
        internal bool IsAllowedToQueryNonIndexedFields() {
            return (this.PassthruLevel & DatabasePassthruLevel.NonIndexed) == DatabasePassthruLevel.NonIndexed;
        }

        /// <summary>
        /// Returns true if command is allowed to query database if no hits are found in the search engine index
        /// </summary>
        /// <returns></returns>
        internal bool IsAllowedToQueryOnZeroHits() {
            return (this.PassthruLevel & DatabasePassthruLevel.ZeroHitsFound) == DatabasePassthruLevel.ZeroHitsFound;
        }

        /// <summary>
        /// Returns true if the comparison operator for this command is searchable (i.e. =, ==, or Like.  &lt;, &gt;, &lt;&gt; are not searchable via the search engine indexes)
        /// </summary>
        /// <returns></returns>
        internal bool IsComparisonQuery() {
            switch (this.CompareMode) {
                case KeywordCompareMode.Equal:
                    return false;
                default:
                    return true;
            }
        }

        private string getComparisonOperator() {
            switch (this.CompareMode) {
                case KeywordCompareMode.Equal:
                default:
                    switch (this.MatchMode) {
                        case KeywordMatchMode.Contains:
                        case KeywordMatchMode.EndsWith:
                        case KeywordMatchMode.StartsWith:
                            return " LIKE ";
                        case KeywordMatchMode.ExactMatch:
                        default:
                            return " = ";
                    }
                    break;
                case KeywordCompareMode.GreaterThan:
                    return " > ";
                case KeywordCompareMode.GreaterThanEqualTo:
                    return " >= ";
                case KeywordCompareMode.LessThan:
                    return " < ";
                case KeywordCompareMode.LessThanEqualTo:
                    return " <= ";
                case KeywordCompareMode.NotEqual:
                    return " <> ";
            }
        }

        internal DataCommand GenerateSqlCommand(Index<TKey, TValue> index, Field field) {


            // Given 'field' object may be:
            // 1) a field stored in the index but not marked as searchable (about any foreign key, for example geography_id)
            // 2) a field unknown to the index entirely but part of the source table definition (owned_by, for example)
            // 3) ???
            // Regardless, we need to query the database and return all information that is stored in the index so we can create
            // pseudo-hits to return for subsequent processing.

            if (String.IsNullOrEmpty(this.FieldName)) {
                // can't run a sql w/o knowing which field to pull from.
                return null;
            }

            var alreadyInFieldList = String.Compare(index.PrimaryKeyField, this.FieldName, true) == 0;

            var sb = new StringBuilder("SELECT ");
            sb.Append(index.PrimaryKeyField);
            if (index.IndexedFields != null) {
                foreach (var f in index.IndexedFields) {
                    sb.Append(", ").Append(f.Name);
                    if (String.Compare(f.Name, this.FieldName, true) == 0){
                        alreadyInFieldList = true;
                    }
                }
            }
            if (field != null && !alreadyInFieldList) {
                sb.Append(", ").Append(field.Name);
            }
            sb.Append(" FROM ");
            if (String.IsNullOrEmpty(this.RestrictToIndex)) {
                sb.Append(index.Name);
            } else {
                sb.Append(this.RestrictToIndex);
            }
            sb.Append(" WHERE ").Append(this.FieldName);
            var op = getComparisonOperator();
            var needParam = true;
            if (this.Keyword.ToString().ToUpper() == "NULL") {
                if (op == " <> ") {
                    needParam = false;
                    sb.Append(" IS NOT NULL ");
                } else if (op == " = ") {
                    needParam = false;
                    sb.Append(" IS NULL ");
                } else {
                    sb.Append(op);
                    sb.Append(" :prm1 ORDER BY ").Append(index.PrimaryKeyField);
                }
            } else {
                sb.Append(op);
                sb.Append(" :prm1 ORDER BY ").Append(index.PrimaryKeyField);
            }

            DataCommand dc = null;
            if (needParam) {
                object val = null;
                var keyword = this.Keyword.RootValue;

                var tempInt = Toolkit.ToInt32(keyword, int.MinValue);
                if (tempInt == int.MinValue) {
                    var tempDec = Toolkit.ToDecimal(keyword, decimal.MinValue);
                    if (tempDec == decimal.MinValue) {
                        var tempDate = Toolkit.ToDateTime(keyword, DateTime.MinValue);
                        if (tempDate == DateTime.MinValue) {
                            val = keyword;
                        } else {
                            val = tempDate;
                        }
                    } else {
                        val = tempDec;
                    }
                } else {
                    val = tempInt;
                }


                dc = new DataCommand(sb.ToString(), new DataParameters(new DataParameter(":prm1", val)));
            } else {
                dc = new DataCommand(sb.ToString());
            }
            Debug.WriteLine(dc.ToString());
            return dc;

        }

        private Indexer<TKey, TValue> _indexer;
		/// <summary>
		/// Gets or sets the index to run this command against
		/// </summary>
		internal Indexer<TKey, TValue> Indexer {
            get {
                return _indexer;
            }
            set {
                if (_indexer != value || value == null){
                    // also clear out any ids or hits we've accumulated
                    //HitList = new List<Hit>();
                }
                _indexer = value;
            }
        }

        public int GetFieldOrdinal(Index<TKey, TValue> idx) {
            return idx.GetFieldDatabaseOrdinal(this.FieldName);
        }

        /// <summary>
        /// Clears SearchCommand term caching for given keyword in all indexes
        /// </summary>
        /// <param name="indexer"></param>
        /// <param name="keyword"></param>
        public static void ClearCache(Indexer<TKey, TValue> indexer, TKey keyword) {
            ClearCache(indexer, keyword.ToString());
        }
        /// <summary>
        /// Clears SearchCommand term caching for given keyword in all indexes
        /// </summary>
        /// <param name="indexer"></param>
        /// <param name="keyword"></param>
        public static void ClearCache(Indexer<TKey, TValue> indexer, string keyword){
            foreach (var tempID in indexer.GetIndexTemporaryIDs()) {
                var idx = indexer.GetIndex(tempID);
                ClearCache(idx, keyword);
            }
        }

        private const char DELIMITER = (char)8;  // i.e. backspace is our delimiter

        /// <summary>
        /// Clears SearchCommand term caching for given keyword in given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="keyword"></param>
        public static void ClearCache(Index<TKey, TValue> index, string keyword) {
            foreach (var rName in index.Resolvers.Keys) {
                var cm = getResolverCacheManager(index.TemporaryID, index.Resolvers[rName].Name);

                // tolist here in case the collection is modified while we process it...
                var keys = cm.Keys.ToList();
                foreach (var keyName in keys) {
                    if (keyName.ToLower().StartsWith(keyword.ToLower() + DELIMITER)) {
                        try {
                            cm.Remove(keyName);
                        } catch {
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Gets or sets the word index within a field of the keyword.  If the word does not appear at the given index, that hit is not returned.
		/// </summary>
		public int KeywordOffset;

		/// <summary>
		/// How to match on keywords.  It is possible to like via the * opeartor.  So hi* returns both hi and high.  This right-side wildcarding is fast.  The following two are slow:  *ion returns both ion and action.  *he* returns the, her, and he.
		/// </summary>
		public KeywordMatchMode MatchMode;

        /// <summary>
        /// How to compare a keyword value.  Default is Equal.  All other values can only be used with database passthru queries.  If those are disabled, the phrase containing the compare mode is ignored.
        /// </summary>
        public KeywordCompareMode CompareMode;

		/// <summary>
		/// If true, searching for "hello" will match "Hello", "HELLO", "hello", "HeLLo", etc.  If false, will only match on "hello".
		/// </summary>
		public bool IgnoreCase;

		/// <summary>
		/// Precedence of SearchActions.  Used when parsing the search string and determining what ids to output
		/// </summary>
		internal int Precedence { get; private set; }


        private string _resolverName;
		/// <summary>
		/// Name of Resolver the idlist should be filled with.  i.e. "accession" should return ID's from the "accession" table.  ResolveName must match the corresponding IndexName for it to work properly in all cases.
		/// </summary>
        public string ResolverName {
            get {
                return _resolverName;
            }
            set {
                _resolverName = (value == null ? null : value.ToLower());
            }
        }

		SearchAction _action;
		/// <summary>
		/// Gets or sets the action for the word.  Implicitly sets the Precedence when this value is changed.
		/// </summary>
		internal SearchAction Action {
			get {
				return _action;
			}
			set {
				Precedence = SearchCommand<TKey, TValue>.GetActionPrecedence(value);
				_action = value;
			}
		}


		public SearchCommand() {
			Indexer = null;
			KeywordOffset = -1;
			MatchMode = KeywordMatchMode.ExactMatch;
			IgnoreCase = true;
			//_fieldOrdinal = SearchCommandConstants.LOOKUP_FIELD_ORDINAL_FROM_INDEX_DEFINITION;
			Action = SearchAction.Literal;
            //HitList = new List<Hit>();
			Precedence = int.MinValue;
            _hitsByIndex = new Dictionary<short, List<Hit>>();
            _resolvedIDsByIndex = new Dictionary<short, List<int>>();

		}

		private static SearchCommand<TKey, TValue> createCommandFromSearchTerm(Index<TKey, TValue> index, string resolverName, bool ignoreCase, string word, bool inQuotes, KeywordMatchMode matchMode, KeywordCompareMode compareMode) {

			if (String.IsNullOrEmpty(word)) {
				return null;
			}

			word = index.RemoveHtml(word);


			if (index.StopWords.Contains(word)) {
				return null;
			}


			SearchCommand<TKey, TValue> cmd = new SearchCommand<TKey, TValue> {
				Indexer = index.Indexer,
				ResolverName = resolverName,
				IgnoreCase = ignoreCase
			};

			if (inQuotes) {
				// it's in quoted phrase, treat as a literal
				cmd.Action = SearchAction.Literal;
			} else {
				cmd.Action = SearchCommand<TKey, TValue>.ParseSearchAction(word);
			}

			if (cmd.Action == SearchAction.FieldDefinition){
				// they want to restrict the next word by a certain field.
				cmd.FieldName = Toolkit.Cut(word, 1);
			} else if (cmd.Action == SearchAction.Literal){
				TKey key = new TKey();
				// this is a literal keyword we should search for.
                if (inQuotes) {
                    // if quoted, * is treated as a literal
                    cmd.MatchMode = matchMode;
                    cmd.CompareMode = compareMode;
                    key.Parse(word);
                    cmd.Keyword = key;
                } else {

                    if (word == "*" || word == "**" || word == "%" || word == "%%") {
                        // assume its literal, as this would be the equivalent of searching for nothing.
                        cmd.MatchMode = matchMode;
                        cmd.CompareMode = compareMode;
                        key.Parse(word);
                        cmd.Keyword = key;
                    } else {
                        // if they specified a * in the search term, ignore the comparison operator and do a non-exact match...
                        if (word.StartsWith("*") || word.StartsWith("%")) {
                            if (word.EndsWith("*") || word.EndsWith("%")) {
                                cmd.MatchMode = KeywordMatchMode.Contains;
                                cmd.CompareMode = KeywordCompareMode.Equal;
                                key.Parse(Toolkit.Cut(word, 1, -1));
                                cmd.Keyword = key;
                            } else {
                                cmd.MatchMode = KeywordMatchMode.EndsWith;
                                cmd.CompareMode = KeywordCompareMode.Equal;
                                key.Parse(Toolkit.Cut(word, 1));
                                cmd.Keyword = key;
                            }
                        } else if (word.EndsWith("*") || word.EndsWith("%")) {
                            cmd.MatchMode = KeywordMatchMode.StartsWith;
                            cmd.CompareMode = KeywordCompareMode.Equal;
                            key.Parse(Toolkit.Cut(word, 0, -1));
                            cmd.Keyword = key;
                        } else {
                            // either there is no *, or it is embedded in the middle of the word.
                            // use the match/compare mode we were given (i.e. do not override)
                            cmd.MatchMode = matchMode;
                            cmd.CompareMode = compareMode;
                            key.Parse(word);
                            cmd.Keyword = key;
                        }
                    }
                }
			}

			return cmd;
		}

		/// <summary>
		/// given a word, determine its appropriate SearchAction (i.e. tokenizing the word)
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		internal static SearchAction ParseSearchAction(string word) {

			if (String.IsNullOrEmpty(word)) {
				return SearchAction.Literal;
			}

			switch (word.ToLower()) {
				case "(":
					return SearchAction.BeginGrouping;
				case ")":
					return SearchAction.EndGrouping;
				case "and":
				case "&":
				case "&&":
					return SearchAction.And;
				case "or":
				case "|":
				case "||":
					return SearchAction.Or;
				case "not":
				case "!":
					return SearchAction.Not;
				default:
					if (word.StartsWith("@")) {
						return SearchAction.FieldDefinition;
					} else {
						return SearchAction.Literal;
					}
			}

		}

		/// <summary>
		/// Given a search action, determine the precedence for it
		/// </summary>
		/// <param name="sa"></param>
		/// <returns></returns>
		internal static int GetActionPrecedence(SearchAction sa) {
			switch (sa) {
				case SearchAction.Literal:
				case SearchAction.Processed:
					return 0;
				case SearchAction.And:
				case SearchAction.Or:
				case SearchAction.Not:
					return 1;
				case SearchAction.BeginGrouping:
				case SearchAction.EndGrouping:
				case SearchAction.FieldDefinition:
					return 2;
                case SearchAction.Then:
                    return 3;
                default:
                    throw new NotImplementedException(getDisplayMember("GetActionPrecedence{default}", "SearchCommand.GetActionPrecedence() not implemented for action={0}", sa.ToString()));
			}
		}

        //internal static string TokenizeQuery(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<Index<TKey, TValue>> indexes, string resolverName) {
        //    var queue = Parse(searchString, ignoreCase, autoAndConsecutiveLiterals, indexes, resolverName);
        //    var ret = convertToString(queue);
        //    return ret;
        //}

        //private static string convertToString(Queue<SearchCommand<TKey, TValue>> cmdQueue) {
        //    StringBuilder sb = new StringBuilder();

        //    List<SearchCommand<TKey, TValue>> list = cmdQueue.ToList();
        //    foreach (SearchCommand<TKey, TValue> cmd in list) {
        //        switch (cmd.Action) {
        //            case SearchAction.Literal:
        //                sb.Append(cmd.Keyword);
        //                break;
        //            case SearchAction.And:
        //                sb.Append(" AND ");
        //                break;
        //            case SearchAction.Or:
        //                sb.Append(" OR ");
        //                break;
        //            case SearchAction.Not:
        //                sb.Append(" NOT ");
        //                break;
        //            case SearchAction.BeginGrouping:
        //                sb.Append("(");
        //                break;
        //            case SearchAction.EndGrouping:
        //                sb.Append(")");
        //                break;
        //            case SearchAction.FieldDefinition:
        //                sb.Append("@").Append(cmd.FieldName);
        //                break;
        //            case SearchAction.Processed:
        //                // should we ever get here???
        //                sb.Append(Toolkit.Join(cmd.GetResolvedIDs().ToArray(), ", ", ""));
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    string ret = sb.ToString();
        //    return ret;
        //}

        private List<int> resolveHit(Hit h, SearchOptions opts) {
            List<int> ret = new List<int>();
            Index<TKey, TValue> idx = Indexer.GetIndex(h.TemporaryIndexID);
            if (idx != null) {
                ret.AddRange(idx.GetResolvedIDs(h, this.ResolverName, opts));
            }
            return ret;
        }

        public List<int> GetResolvedIDsByIndex(short tempIndexID, SearchOptions opts) {
            List<int> ret = null;
            if (_resolvedIDsByIndex == null) {
                _resolvedIDsByIndex = new Dictionary<short, List<int>>();
            }
            if (!_resolvedIDsByIndex.TryGetValue(tempIndexID, out ret)) {
                ret = new List<int>();
                _resolvedIDsByIndex.Add(tempIndexID, ret);
            }
            return ret;
        }


        private List<Hit> GetHitsByIndex(short tempIndexID, SearchOptions opts) {
            List<Hit> ret = null;
            if (_hitsByIndex == null) {
                _hitsByIndex = new Dictionary<short, List<Hit>>();
            }
            if (!_hitsByIndex.TryGetValue(tempIndexID, out ret)) {
                ret = new List<Hit>();
                _hitsByIndex.Add(tempIndexID, ret);
            }
            return ret;
        }

        public void IntersectResolvedIDsRegardlessOfIndex(Dictionary<short, List<int>> otherResolvedIDsByIndex) {

            // first, just concat all the current resolved ids
            var ids = new List<int>().AsEnumerable();
            foreach (var value in _resolvedIDsByIndex.Values) {
                ids = ids.Concat(value);
            }

            // then intersect each of this command's resolved ids by index with all the ids from the 'other' resolved ids (regardless of index)
            foreach (var key in otherResolvedIDsByIndex.Keys) {
                _resolvedIDsByIndex[key] = otherResolvedIDsByIndex[key].Intersect(ids).ToList();
            }
            var missingKeys = _resolvedIDsByIndex.Keys.Except(otherResolvedIDsByIndex.Keys).ToList();
            foreach (var key2 in missingKeys) {
                // zeo out ones in this index list that are not present in the other.
                _resolvedIDsByIndex[key2] = new List<int>();
            }
        }

        public void IntersectResolvedIDsByIndex(Dictionary<short, List<int>> otherResolvedIDsByIndex) {
            foreach (var key in otherResolvedIDsByIndex.Keys) {
                List<int> thisIDList = null;
                if (_resolvedIDsByIndex.TryGetValue(key, out thisIDList)) {
                    _resolvedIDsByIndex[key] = otherResolvedIDsByIndex[key].Intersect(thisIDList).ToList();
                } else {
                    // nothing to intersect with, empty it out.
                    _resolvedIDsByIndex[key] = new List<int>();
                }
            }
        }

        public void IntersectHitsByIndexAndKeywordOffset(Dictionary<short, List<Hit>> otherHitsByIndex) {
            foreach (var key in otherHitsByIndex.Keys) {
                List<Hit> ret = new List<Hit>();
                List<Hit> hitList = null;
                if (_hitsByIndex.TryGetValue(key, out hitList)) {
                    ret = Hit.Intersect(hitList, otherHitsByIndex[key], SearchCommandConstants.DETERMINE_KEYWORD_INDEX_FROM_CONTEXT).ToList();
                }
                _hitsByIndex[key] = ret;
            }
        }

        public void ConcatResolvedIDsByIndex(Dictionary<short, List<int>> otherResolvedIDsByIndex) {
            foreach (var key in otherResolvedIDsByIndex.Keys) {
                List<int> thisIDList = null;
                if (_resolvedIDsByIndex.TryGetValue(key, out thisIDList)) {
                    _resolvedIDsByIndex[key] = otherResolvedIDsByIndex[key].Concat(thisIDList).ToList();
                } else {
                    _resolvedIDsByIndex[key] = otherResolvedIDsByIndex[key];
                }
            }
        }

        public void resolveHitsByIndex(SearchOptions opts) {
            foreach (var key in _hitsByIndex.Keys) {
                List<int> rids = null;
                if (!_resolvedIDsByIndex.TryGetValue(key, out rids)){
                    _resolvedIDsByIndex[key] = _indexer.GetIndex(key).GetResolvedIDs(_hitsByIndex[key], this.ResolverName, opts);
                } else {
                    _resolvedIDsByIndex[key] = _resolvedIDsByIndex[key].Intersect(_indexer.GetIndex(key).GetResolvedIDs(_hitsByIndex[key], this.ResolverName, opts)).ToList();
                }
            }
        }

        public void ExceptResolvedIDsByIndex(Dictionary<short, List<int>> otherResolvedIDsByIndex) {

            // first, just concat all the current resolved ids
            var ids = new List<int>().AsEnumerable();
            foreach (var value in otherResolvedIDsByIndex.Values) {
                ids = ids.Concat(value);
            }

            foreach (var key in otherResolvedIDsByIndex.Keys) {
                List<int> thisIDList = null;
                if (_resolvedIDsByIndex.TryGetValue(key, out thisIDList)) {
                    _resolvedIDsByIndex[key] = thisIDList.Except(ids).ToList();
                }
            }
        }


        /// <summary>
        /// Generates the resolved IDs for all hits currently contained in this command.  
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetResolvedIDs(DataSet ds, SearchOptions opts) {

            List<int> ret = null;

            if (ret == null) {
                ret = new List<int>();
            }

            if (_resolvedIDsByIndex != null && _resolvedIDsByIndex.Count > 0) {
                foreach (var resolvedKey in _resolvedIDsByIndex.Keys) {
                    ret.AddRange(_resolvedIDsByIndex[resolvedKey]);
                }
                return ret;
            }


            // each hit may come from a different index (the CurrentIndex property of this object just points to the last one we searched)
            // so for each hit, get the associated index and resolve from there.

            if (_hitsByIndex != null && _hitsByIndex.Count > 0) {
                foreach (var hitKey in _hitsByIndex.Keys) {
                    Index<TKey, TValue> idx = Indexer.GetIndex(hitKey);
                    var hits = idx.GetHits(this, ds, opts);
                    ret.AddRange(idx.GetResolvedIDs(hits, this.ResolverName, opts));
                }
            }

            //short previousTempID = -1;
            //List<Hit> groupedHits = new List<Hit>();
            //foreach (Hit h in this.HitList) {
            //    if (previousTempID == -1) {
            //        previousTempID = h.TemporaryIndexID;
            //    }

            //    if (previousTempID != h.TemporaryIndexID && groupedHits.Count > 0) {
            //        // new group, resolve this group of hits
            //        Index<TKey, TValue> idx = Indexer.GetIndex(groupedHits[0].TemporaryIndexID);
            //        if (idx != null) {
            //            ret.AddRange(idx.GetResolvedIDs(groupedHits, this.ResolverName));
            //        }
            //        groupedHits = new List<Hit>();
            //    }
            //    groupedHits.Add(h);
            //}

            //// at least one non-resolved hit after our grouping loop. 
            //if (groupedHits.Count > 0) {
            //    Index<TKey, TValue> idx = Indexer.GetIndex(groupedHits[0].TemporaryIndexID);
            //    if (idx != null) {
            //        ret.AddRange(idx.GetResolvedIDs(groupedHits, this.ResolverName));
            //    }
            //}

            return ret;


        }

        private static Queue<SearchCommand<TKey, TValue>> getSubQueue(Queue<SearchCommand<TKey, TValue>> origQueue, bool groupAlreadyStarted) {

            var subQueue = new Queue<SearchCommand<TKey, TValue>>();
            if (origQueue.Count == 0){
                
                // nothing to queue
                return subQueue;

            } else if (origQueue.Peek().Action != SearchAction.BeginGrouping && !groupAlreadyStarted) {

                // only one item in the next subqueue (i.e. not grouped)
                subQueue.Enqueue(origQueue.Dequeue());
                return subQueue;

            } else {

                // a sub group starts here. pull all of its items in (including nested groupings)
                int groupDepth = groupAlreadyStarted ? 1 : 0;
                while (true) {
                    var temp = origQueue.Dequeue();
                    if (temp.Action == SearchAction.EndGrouping) {
                        groupDepth--;
                        if (groupDepth == 0) {
                            if (!groupAlreadyStarted) {
                                subQueue.Enqueue(temp);
                            }
                            break;
                        }
                    } else if (temp.Action == SearchAction.BeginGrouping) {
                        // beginning an inner group.
                        groupDepth++;
                    }

                    subQueue.Enqueue(temp);

                    if (origQueue.Count == 0) {
                        throw new InvalidOperationException(getDisplayMember("getSubQueue{mismatchedgropuing}", "Bad search string -- grouping not ended properly, parentheses do not match"));
                    }
                }

                return subQueue;
            }
        }

        private static Queue<SearchCommand<TKey, TValue>> adjustForFieldDefinitions(Queue<SearchCommand<TKey, TValue>> queue, SearchOptions opts) {

            var ret = new Queue<SearchCommand<TKey,TValue>>();

            while (queue.Count > 0) {
                var cur = queue.Dequeue();
                if (cur.Action != SearchAction.FieldDefinition) {
                    // this is not a field definition.
                    // just queue it up
                    ret.Enqueue(cur);
                } else {
                    // this is a field definition.
                    // apply its FieldName and RestrictToIndex properties
                    // to all in the following subqueue (i.e. either just next element, or all elements in the subgroup)
                    var subqueue = getSubQueue(queue, false);
                    while (subqueue.Count > 0) {
                        var sub = subqueue.Dequeue();
                        sub.RestrictToIndex = cur.RestrictToIndex;
                        sub.FieldName = cur.FieldName;
                        if (sub.Action == SearchAction.And) {
                            // within a field definition, we or everything regardless of the default operator
                            // (i.e. always do similar to what a SQL IN() clause would)
                            sub.Action = SearchAction.Or;
                        }
                        ret.Enqueue(sub);
                    }
                }
            }

            return ret;

        }


        //private static Queue<SearchCommand<TKey, TValue>> adjustForBooleans(Queue<SearchCommand<TKey, TValue>> queue) {
        //    throw new NotImplementedException("adjustForBooleans is not finished!!!");
        //    var ret = new Queue<SearchCommand<TKey, TValue>>();

        //    var list = new List<SearchCommand<TKey, TValue>>();

        //    while (queue.Count > 0) {
        //        var cur = queue.Dequeue();
        //        if (String.IsNullOrEmpty(cur.FieldName) || !cur.FieldName.ToLower().StartsWith("is_")) {
        //            // not a boolean field
        //            // queue it up
        //            ret.Enqueue(cur);

        //        } else {

        //            var sublist = getSubQueue(queue, false).ToList();

        //            foreach (var c in sublist) {

        //            }

        //            // assume it's a boolean. we won't know for sure until we execute the command on a per-index basis
        //            // (as a field may be a boolean in one index, but not in another)
        //            // 


        //        }
        //        if (cur.Action != SearchAction.FieldDefinition) {
        //            // this is not a field definition.
        //            // just queue it up
        //            ret.Enqueue(cur);
        //        } else {
        //            // this is a field definition.
        //            // apply its FieldName and RestrictToIndex properties
        //            // to all in the following subqueue (i.e. either just next element, or all elements in the subgroup)
        //            var subqueue = getSubQueue(queue, false);
        //            while (subqueue.Count > 0) {
        //                var sub = subqueue.Dequeue();
        //                sub.RestrictToIndex = cur.RestrictToIndex;
        //                sub.FieldName = cur.FieldName;
        //                if (sub.Action == SearchAction.And) {
        //                    // within a field definition, we or everything regardless of the default operator
        //                    // (i.e. always do similar to what a SQL IN() clause would)
        //                    sub.Action = SearchAction.Or;
        //                }
        //                ret.Enqueue(sub);
        //            }
        //        }
        //    }

        //    return ret;

        //}



        internal SearchCommand<TKey, TValue> Clone() {
            var clone = new SearchCommand<TKey, TValue>();
            clone._action = this._action;
            clone._fieldName = this._fieldName;
            //clone._hitsByIndex = this._hitsByIndex;
            clone._indexer = this._indexer;
            //clone._resolvedIDsByIndex = this._resolvedIDsByIndex;
            clone.Keyword = this.Keyword;
            clone.FieldName = this.FieldName;
            clone.KeywordOffset = this.KeywordOffset;
            clone.MatchMode = this.MatchMode;
            clone.CompareMode = this.CompareMode;
            clone.Precedence = this.Precedence;
            clone.RestrictToIndex = this.RestrictToIndex;
            clone._resolverName = this._resolverName;
            clone.PassthruLevel = this.PassthruLevel;
            clone.LanguageID = this.LanguageID;
            clone.LookupCodedValues = this.LookupCodedValues;
            clone.IgnoreCache = this.IgnoreCache;
            clone.IgnoreCase = this.IgnoreCase;
            return clone;
        }

        internal int DependentCommands {
            get {
                switch (Action) {
                    case SearchAction.Literal:
                    case SearchAction.EndGrouping:
                    case SearchAction.Processed:
                    default:
                        return 0;
                    case SearchAction.And:
                    case SearchAction.Or:
                    case SearchAction.Not:
                    case SearchAction.FieldDefinition:
                    case SearchAction.Then:
                        return 1;
                    case SearchAction.BeginGrouping:
                        return 2;
                }
            }
        }


		/// <summary>
		/// Parses the search string into a queue of valid search commands.
		/// </summary>
		/// <param name="searchString"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="autoAndConsecutiveLiterals"></param>
        /// <param name="indexes"></param>
		/// <param name="resolverName"></param>
		/// <returns></returns>
        internal static Queue<SearchCommand<TKey, TValue>> Parse(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<Index<TKey, TValue>> indexes, string resolverName, DataSet ds, SearchOptions options, Indexer<TKey, TValue> indexer) {

            Queue<SearchCommand<TKey, TValue>> queue = new Queue<SearchCommand<TKey, TValue>>();

            var explicitIndexes = new List<string>();

            if (indexes != null && indexes.Count > 0) {

                var betweenCount = 0;

                var lastQueuedCommand = new SearchCommand<TKey, TValue> {
                    Action = SearchAction.BeginGrouping,
                    Indexer = indexes[0].Indexer,
                    ResolverName = resolverName
                };

                SearchCommand<TKey, TValue> lastQueuedFieldDefinition = null;

                // we always have a grouping.
                queue.Enqueue(lastQueuedCommand);

                // we inject this default opeartor when two items that will result as Processed appear consecutively
                SearchCommand<TKey, TValue> defaultLogicalOperator = new SearchCommand<TKey, TValue> {
                    Action = (autoAndConsecutiveLiterals ? SearchAction.And : SearchAction.Or),
                    ResolverName = resolverName,
                    Indexer = indexer,
                    PassthruLevel = options.PassThruLevel,
                    LookupCodedValues = options.LookupCodedValues,
                    LanguageID = options.LanguageID,
                    IgnoreCache = options.IgnoreCache
                };

                string mungedSearchString = searchString;

                // if there's line breaks and "OrMultipleLines" is enabled, put an OR between each line
                if (options.OrMultipleLines) {
                    if (mungedSearchString.Contains("\n")) {
                        mungedSearchString = "(" + mungedSearchString.Replace("\r\n", "\n").Replace("\n", ") or (") + ")";
                        mungedSearchString = Regex.Replace(mungedSearchString, @"[) ]or\s+\(\s*\)", "");
                    }
                }

                List<string> words = mungedSearchString.SplitRetain(indexes[0].Whitespace, true, false, true);

                var matchMode = KeywordMatchMode.ExactMatch;
                var compareMode = KeywordCompareMode.Equal;

                // first simply configure each command
                bool inQuotes = false;
                bool firstQuotedWord = false;
                for (int i = 0; i < words.Count; i++) {

                    // we escaped the quotes, so we have to check for quotes w/ escapes
                    if (words[i].StartsWith("'") || words[i].StartsWith("\"")) {
                        inQuotes = true;
                        firstQuotedWord = true;
                        words[i] = Toolkit.Cut(words[i], 1, -1);
                    } else {
                        inQuotes = false;
                    }

                    if (!inQuotes) {

                        switch (words[i][0]) {
                            case '\n':
                            case '\r':
                            case '\t':
                            case ' ':
                                // just ignore
                                break;
                            default:
                                // if it contains any of our special chars, we need to process that before we strip out whatever is
                                // defined as separators in the index, as that might strip out our special chars!
                                while (words[i].StartsWith("(")) {
                                    // this is a begin grouping item.

                                    //// reset to default matching mode
                                    //matchMode = KeywordMatchMode.ExactMatch;

                                    lastQueuedCommand = tweakQueueAsNeeded(queue, defaultLogicalOperator, new SearchCommand<TKey, TValue> {
                                        Action = SearchAction.BeginGrouping,
                                        Indexer = indexer,
                                        ResolverName = resolverName,
                                        MatchMode = matchMode,
                                        CompareMode = compareMode,
                                        PassthruLevel = options.PassThruLevel,
                                        LookupCodedValues = options.LookupCodedValues,
                                        LanguageID = options.LanguageID,
                                        IgnoreCache = options.IgnoreCache
                                    }, false);

                                    // remove that from our word so it's not processed 2x
                                    words[i] = Toolkit.Cut(words[i], 1);

                                }


                                if (words[i].StartsWith("!")) {

                                    if (words[i] == "!=") {
                                        // this is a 'not equal' operator
                                    } else {
                                        // this is a NOT item.

                                        // reset to default matching mode
                                        matchMode = KeywordMatchMode.ExactMatch;
                                        compareMode = KeywordCompareMode.NotEqual;

                                        lastQueuedCommand = tweakQueueAsNeeded(queue, defaultLogicalOperator, new SearchCommand<TKey, TValue> {
                                            Action = SearchAction.Not,
                                            Indexer = indexer,
                                            ResolverName = resolverName,
                                            MatchMode = matchMode,
                                            CompareMode = compareMode,
                                            PassthruLevel = options.PassThruLevel,
                                            LookupCodedValues = options.LookupCodedValues,
                                            LanguageID = options.LanguageID,
                                            IgnoreCache = options.IgnoreCache
                                        }, false);
                                        // remove that from our word so it's not processed 2x
                                        words[i] = Toolkit.Cut(words[i], 1);

                                    }

                                }

                                if (words[i].StartsWith("@")) {

                                    // this is a field definition item.

                                    // reset to default matching mode, let subsequent token parsing override
                                    matchMode = KeywordMatchMode.ExactMatch;
                                    compareMode = KeywordCompareMode.Equal;

                                    SearchCommand<TKey, TValue> fieldDef = new SearchCommand<TKey, TValue> {
                                        Action = SearchAction.FieldDefinition,
                                        Indexer = indexer,
                                        ResolverName = resolverName,
                                        MatchMode = matchMode,
                                        CompareMode = compareMode,
                                        PassthruLevel = options.PassThruLevel,
                                        LookupCodedValues = options.LookupCodedValues,
                                        LanguageID = options.LanguageID,
                                        IgnoreCache = options.IgnoreCache
                                    };

                                    int dotPosition = words[i].IndexOf(".");
                                    if (dotPosition > -1) {
                                        // table name is part of field definition. yank it out, put into keyword
                                        TKey key = new TKey().Parse(Toolkit.Cut(words[i], 1, dotPosition - 1));
                                        fieldDef.Keyword = key;
                                        fieldDef.RestrictToIndex = Toolkit.Cut(words[i], 1, dotPosition - 1);

                                        // in case there are operators at the end, we need to split those out...
                                        var ops = Toolkit.Cut(words[i], dotPosition + 1).SplitRetain(new char[] { '=', '!', '>', '<', ')' }, false, true, false);
                                        if (ops.Count > 0) {
                                            words[i] = ops[0];
                                            for (var j = 1; j < ops.Count; j++) {
                                                words.Insert(i + j, ops[j]);
                                            }
                                        }
                                        fieldDef.FieldName = words[i];


                                        // since they specified an explicit index in the query,
                                        // we may need to add that index so searches work correctly.
                                        // but only add it if the options say we can (default is that we can)

                                        if (options.AddExplicitIndexes) {
                                            var foundExplicitIndex = false;
                                            foreach (var idx in indexes) {
                                                if (String.Compare(idx.Name, fieldDef.RestrictToIndex) == 0) {
                                                    foundExplicitIndex = true;
                                                    break;
                                                }
                                            }

                                            if (!foundExplicitIndex) {
                                                var idx = indexer.GetIndex(fieldDef.RestrictToIndex);
                                                if (!indexes.Contains(idx)) {
                                                    indexes.Add(idx);
                                                }
                                            }
                                        }

                                    } else {

                                        var fieldName = Toolkit.Cut(words[i], 1);
                                        if (fieldName == "*" || fieldName == "%") {
                                            // no specific field, should probably never get here.  Probably. :)
                                        } else {
                                            fieldDef.FieldName = fieldName;
                                        }
                                    }
                                    lastQueuedFieldDefinition = fieldDef;
                                    lastQueuedCommand = tweakQueueAsNeeded(queue, defaultLogicalOperator, fieldDef, false);

                                    // we've fully processed this word
                                    words[i] = string.Empty;
                                    //var closeParenPos = words[i].IndexOf(")");
                                    //if (closeParenPos > -1){
                                    //    words[i] = Toolkit.Cut(words[i], closeParenPos);
                                    //} else {
                                    //    var eqPos = words[i].IndexOf("=");
                                    //    if (eqPos > -1) {
                                    //        words[i] = Toolkit.Cut(words[i], eqPos);
                                    //    } else {
                                    //        var gtPos = words[i].IndexOf(">");
                                    //        if (gtPos > -1) {
                                    //            words[i] = Toolkit.Cut(words[i], gtPos);
                                    //        } else {
                                    //            var ltPos = words[i].IndexOf("<");
                                    //            if (ltPos > -1) {
                                    //                words[i] = Toolkit.Cut(words[i], ltPos);
                                    //            } else {
                                    //                var bangPos = words[i].IndexOf("!");
                                    //                if (bangPos > -1) {
                                    //                    words[i] = Toolkit.Cut(words[i], bangPos);
                                    //                } else {
                                    //                    words[i] = String.Empty;
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                }

                                if (lastQueuedCommand.Action == SearchAction.FieldDefinition) {
                                    switch (words[i].ToUpper()) {
                                        case "<":
                                            compareMode = KeywordCompareMode.LessThan;
                                            words[i] = string.Empty;
                                            break;
                                        case "<=":
                                            compareMode = KeywordCompareMode.LessThanEqualTo;
                                            words[i] = string.Empty;
                                            break;
                                        case ">":
                                            compareMode = KeywordCompareMode.GreaterThan;
                                            words[i] = string.Empty;
                                            break;
                                        case ">=":
                                            compareMode = KeywordCompareMode.GreaterThanEqualTo;
                                            words[i] = string.Empty;
                                            break;
                                        case "<>":
                                        case "!=":
                                            compareMode = KeywordCompareMode.NotEqual;
                                            words[i] = string.Empty;
                                            break;
                                        case "=":
                                        case "==":
                                        case "IN":
                                        case "LIKE":
                                            compareMode = KeywordCompareMode.Equal;
                                            matchMode = KeywordMatchMode.ExactMatch;
                                            words[i] = string.Empty;
                                            break;
                                        case "BETWEEN":
                                            betweenCount++;
                                            compareMode = KeywordCompareMode.Between;
                                            matchMode = KeywordMatchMode.ExactMatch;
                                            words[i] = string.Empty;
                                            break;
                                    }
                                }

                                if (lastQueuedCommand.CompareMode == KeywordCompareMode.Between) {
                                    if (words[i].ToUpper() != "AND") {
                                        throw new InvalidOperationException(getDisplayMember("Parse{badand}", "Bad search string -- expected to find 'AND' as part of a 'BETWEEN ... AND' pharse, found '{0}'.", words[i]));
                                    } else {

                                        // first, inject an AND operator
                                        var andCmd = defaultLogicalOperator.Clone();
                                        andCmd.Action = SearchAction.And;
                                        queue.Enqueue(andCmd);

                                        // then inject the previously queued field definition as another field definition
                                        var newFieldDef = lastQueuedFieldDefinition.Clone();
                                        queue.Enqueue(newFieldDef);


                                        // change properties for next command coming out
                                        compareMode = KeywordCompareMode.LessThanEqualTo;
                                        matchMode = KeywordMatchMode.ExactMatch;
                                        words[i] = string.Empty;

                                        // change previous literal compare mode to be >= instead of between
                                        // (i.e. we use the Between compare mode simply as a flag during parsing)
                                        lastQueuedCommand.CompareMode = KeywordCompareMode.GreaterThanEqualTo;
                                        betweenCount--;
                                        break;
                                    }
                                }

                                break;
                        }
                    }

                    // we may have already done everything against this word we need to. if so, it'll be empty.
                    if (!String.IsNullOrEmpty(words[i])) {
                        // we get here, we've stripped out all special chars that mean anything to us
                        // we may still need to strip some other items (whatever is defined in the index's config file as separators)
                        string[] subwords = Indexer<TKey, TValue>.ParseKeywords(words[i], indexes[0]);

                        if (subwords.Length > 0) {

                            if (inQuotes) {

                                if (firstQuotedWord) {
                                    var prev = queue.Last();
                                    if (prev.Action == SearchAction.Literal) {
                                        lastQueuedCommand = defaultLogicalOperator.Clone();
                                        queue.Enqueue(lastQueuedCommand);
                                    }
                                }

                                // quoting implies a grouping...
                                lastQueuedCommand = new SearchCommand<TKey, TValue> {
                                    Action = SearchAction.BeginGrouping,
                                    Indexer = indexes[0].Indexer,
                                    ResolverName = resolverName
                                };
                                queue.Enqueue(lastQueuedCommand);
                            }

                            for (int j = 0; j < subwords.Length; j++) {
                                string subword = subwords[j];
                                SearchCommand<TKey, TValue> prev = null;
                                SearchCommand<TKey, TValue> cmd = createCommandFromSearchTerm(indexes[0], resolverName, ignoreCase, subword, inQuotes, matchMode, compareMode);
                                if (cmd != null) {
                                    cmd.PassthruLevel = options.PassThruLevel;
                                    cmd.LookupCodedValues = options.LookupCodedValues;
                                    cmd.LanguageID = options.LanguageID;
                                    cmd.IgnoreCache = options.IgnoreCache;
                                    // some words resolve to no commands (think StopWords), so don't queue up a nonexistent command
                                    lastQueuedCommand = tweakQueueAsNeeded(queue, defaultLogicalOperator, cmd, inQuotes);
                                    if (inQuotes) {
                                        // flag it saying the keyword index is important, but not explicitly stated
                                        cmd.KeywordOffset = SearchCommandConstants.DETERMINE_KEYWORD_INDEX_FROM_CONTEXT;
                                        if (firstQuotedWord) {
                                            // this lets us fudge the phrase "hello mom" with two searches of "hello" at any keyword offset in a given field and "mom" at the immediately following keyword offset in the exact same field in the exact same search index.
                                            firstQuotedWord = false;
                                        } else {
                                            if (prev != null) {
                                                cmd.FieldName = prev.FieldName;
                                            }
                                        }
                                        prev = cmd;
                                    }
                                }
                            }

                            if (inQuotes) {
                                // quoting implies a grouping...
                                lastQueuedCommand = new SearchCommand<TKey, TValue> {
                                    Action = SearchAction.EndGrouping,
                                    Indexer = indexer,
                                    ResolverName = resolverName,
                                    PassthruLevel = options.PassThruLevel,
                                    LookupCodedValues = options.LookupCodedValues,
                                    LanguageID = options.LanguageID,
                                    IgnoreCache = options.IgnoreCache
                                };
                                queue.Enqueue(lastQueuedCommand);
                            }

                        }
                    }

                    if (!inQuotes) {
                        while (words[i].EndsWith(")")) {
                            // this is an end grouping item.
                            lastQueuedCommand = tweakQueueAsNeeded(queue, defaultLogicalOperator, new SearchCommand<TKey, TValue> {
                                Action = SearchAction.EndGrouping,
                                Indexer = indexer,
                                ResolverName = resolverName,
                                PassthruLevel = options.PassThruLevel,
                                LookupCodedValues = options.LookupCodedValues,
                                LanguageID = options.LanguageID,
                                IgnoreCache = options.IgnoreCache
                            }, false);
                            words[i] = Toolkit.Cut(words[i], 0, -1);
                        }
                    }
                }



                // now, all of the terms have been parsed into commands.
                // we may need to tweak it to make sure field definition settings are spread properly throughout the queue.
                queue = adjustForFieldDefinitions(queue, options);

                //queue = adjustForBooleans(queue);

                // and close our group we always create
                lastQueuedCommand = new SearchCommand<TKey, TValue> {
                    Action = SearchAction.EndGrouping,
                    Indexer = indexes[0].Indexer,
                    ResolverName = resolverName,
                    PassthruLevel = options.PassThruLevel,
                    LookupCodedValues = options.LookupCodedValues,
                    LanguageID = options.LanguageID,
                    IgnoreCache = options.IgnoreCache
                };
                queue.Enqueue(lastQueuedCommand);

                if (Debugger.IsAttached) {
                    Debug.WriteLine("command queue for '" + searchString + "' follows:");
                    foreach (var cmd in queue) {
                        Debug.WriteLine(cmd.ToString());
                    }
                    Debug.WriteLine("end command queue for '" + searchString + "'");
                }

                if (betweenCount > 0) {
                    throw new InvalidOperationException(getDisplayMember("Parse{badbetween}", "Bad search string -- found 'BETWEEN' to start a 'BETWEEN ... AND' phrase, but never found corresponding 'AND'.\nIf you are trying to search for the literal keyword 'BETWEEN', you must enclose it in quotes."));
                }
            }

            // remember original query, new parsed query
            var dt = ds.Tables["SearchQuery"];
            var newSearchString = getGeneratedSearchString(queue);
            Debug.WriteLine("orig=" + searchString + " -- parsed=" + newSearchString);
            dt.Rows.Add(new object[] { searchString, ignoreCase, autoAndConsecutiveLiterals, Toolkit.Join(indexes.ToArray(), ",", ""), resolverName, newSearchString});


			// return queue to caller so they may apply it to other indexes as desired
			return queue;
		}

        private static string getGeneratedSearchString(Queue<SearchCommand<TKey, TValue>> queue) {
            var list = queue.ToList();
            var sb = new StringBuilder();
            foreach (SearchCommand<TKey, TValue> cmd in list) {
                switch (cmd.Action) {
                    case SearchAction.And:
                        sb.Append(" AND ");
                        break;
                    case SearchAction.Or:
                        sb.Append(" OR ");
                        break;
                    case SearchAction.Not:
                        sb.Append(" NOT ");
                        break;
                    case SearchAction.Then:
                        sb.Append(" THEN ");
                        break;
                    case SearchAction.BeginGrouping:
                        sb.Append("(");
                        break;
                    case SearchAction.EndGrouping:
                        sb.Append(")");
                        break;
                    case SearchAction.FieldDefinition:
                        sb.Append("@");
                        break;
                    case SearchAction.Literal:
                    case SearchAction.Processed:
                        if (!String.IsNullOrEmpty(cmd.FieldName)) {
                            sb.Append("@");
                            if (!String.IsNullOrEmpty(cmd.RestrictToIndex)) {
                                sb.Append(cmd.RestrictToIndex).Append(".");
                            }
                            sb.Append(cmd.FieldName);
                        } else {
                            sb.Append("@*");
                        }
                        switch (cmd.CompareMode) {
                            case KeywordCompareMode.Equal:
                                switch (cmd.MatchMode) {
                                    case KeywordMatchMode.StartsWith:
                                        sb.Append(" LIKE ").Append(cmd.Keyword).Append("* ");
                                        break;
                                    case KeywordMatchMode.Contains:
                                        sb.Append(" LIKE *").Append(cmd.Keyword).Append("* ");
                                        break;
                                    case KeywordMatchMode.EndsWith:
                                        sb.Append(" LIKE *").Append(cmd.Keyword).Append(" ");
                                        break;
                                    case KeywordMatchMode.ExactMatch:
                                        sb.Append(" = ").Append(cmd.Keyword).Append(" ");
                                        break;
                                }

                                break;
                            case KeywordCompareMode.NotEqual:
                                sb.Append(" != ").Append(cmd.Keyword).Append(" ");
                                break;
                            case KeywordCompareMode.GreaterThan:
                                sb.Append(" > ").Append(cmd.Keyword).Append(" ");
                                break;
                            case KeywordCompareMode.GreaterThanEqualTo:
                                sb.Append(" >= ").Append(cmd.Keyword).Append(" ");
                                break;
                            case KeywordCompareMode.LessThan:
                                sb.Append(" < ").Append(cmd.Keyword).Append(" ");
                                break;
                            case KeywordCompareMode.LessThanEqualTo:
                                sb.Append(" <= ").Append(cmd.Keyword).Append(" ");
                                break;
                            case KeywordCompareMode.Between:
                                sb.Append(" BETWEEN ").Append(cmd.Keyword).Append(" ");
                                break;
                            case KeywordCompareMode.BetweenAnd:
                                sb.Append(" AND ").Append(cmd.Keyword).Append(" ");
                                break;

                            default:
                                throw new NotImplementedException(getDisplayMember("getGeneratedSearchString{default}", "No mapping in getGeneratedSearchString() for keyword compare mode={0}", cmd.CompareMode.ToString()));
                        }
                        break;
                }
            }
            return sb.ToString();
        }


        private static SearchCommand<TKey, TValue> tweakQueueAsNeeded(Queue<SearchCommand<TKey, TValue>> queue, SearchCommand<TKey, TValue> defaultOperator, SearchCommand<TKey, TValue> newCmd, bool inQuotes) {
			if (queue.Count > 0) {
                SearchCommand<TKey, TValue> prev = queue.Last(); // HACK: a queue shouldn't allow us to grab the last one in (since by definition it's FIFO) but we need it and there's no predefined Dequeue<T> class in .NET.

				// special processing for @field support...
				//
				// fields can be defined as matching in any index or just a specific one:
				// @taxonomy.protologue fred
				// @protologue fred
				// this lets the caller use the 'easy' syntax if they want, but allows
				// them to prevent results from another index, as their search string may
				// be applied across multiple indexes in one request
				if (prev.Action == SearchAction.FieldDefinition) { // && (String.IsNullOrEmpty(prev.Keyword) || prev.Keyword.ToLower() == prev.Index.Name.ToLower()){
                    newCmd.FieldName = prev.FieldName;
                    newCmd.RestrictToIndex = prev.RestrictToIndex;
				}

				if (requiresOperator(prev, newCmd)) {
					// we have two consecutive commands that will result in literal values.
					// inject the proper operator between them.
                    if (inQuotes) {

                        // inject a 'then' operator, meaning the previous THEN the newCmd must appear
                        var cmdThen = new SearchCommand<TKey, TValue> {
                            Indexer = newCmd.Indexer,
                            Action = SearchAction.Then,
                            KeywordOffset = prev.KeywordOffset - 1,
                            FieldName = prev.FieldName
                        };

                        queue.Enqueue(cmdThen);

                    } else {
                        queue.Enqueue(defaultOperator.Clone());
                    }

                //} else if (newCmd.Action == SearchAction.And || newCmd.Action == SearchAction.Or) {
                //    // they explicitly added this and/or command.
                //    // to do things properly, auto-close and auto-begin a new group around it.
                //    var cmdEndGroup = new SearchCommand<TKey, TValue> {
                //        Indexer = newCmd.Indexer,
                //        Action = SearchAction.EndGrouping,
                //        ResolverName = prev.ResolverName,
                //        KeywordOffset = prev.KeywordOffset,
                //        FieldName = prev.FieldName,
                //        Keyword = prev.Keyword,
                //        MatchMode = prev.MatchMode,
                //        PassthruLevel = prev.PassthruLevel,
                //        LookupCodedValues = prev.LookupCodedValues,
                //        LanguageID = prev.LanguageID,
                //        IgnoreCache = prev.IgnoreCache
                //    };


                //    var cmdBeginGroup = new SearchCommand<TKey, TValue> {
                //        Indexer = newCmd.Indexer,
                //        Action = SearchAction.BeginGrouping,
                //        ResolverName = prev.ResolverName,
                //        KeywordOffset = prev.KeywordOffset,
                //        FieldName = prev.FieldName,
                //        Keyword = prev.Keyword,
                //        MatchMode = prev.MatchMode,
                //        PassthruLevel = prev.PassthruLevel,
                //        LookupCodedValues = prev.LookupCodedValues,
                //        LanguageID = prev.LanguageID,
                //        IgnoreCache = prev.IgnoreCache
                //    };

                //    queue.Enqueue(cmdEndGroup);
                //    if (String.IsNullOrEmpty(newCmd.FieldName)) {
                //        newCmd.MatchMode = KeywordMatchMode.ExactMatch;
                //    }
                //    queue.Enqueue(newCmd);
                //    queue.Enqueue(cmdBeginGroup);
                //    return cmdBeginGroup;
                
                }
			}
            //if (String.IsNullOrEmpty(newCmd.FieldName)) {
            //    newCmd.MatchMode = KeywordMatchMode.ExactMatch;
            //}
			queue.Enqueue(newCmd);
            return newCmd;
		}

        private static bool requiresOperator(SearchCommand<TKey, TValue> leftCommand, SearchCommand<TKey, TValue> rightCommand) {

			SearchAction left = leftCommand.Action;
			SearchAction right = rightCommand.Action;


			if (left == SearchAction.And || left == SearchAction.Or || left == SearchAction.FieldDefinition || left == SearchAction.Not || left == SearchAction.Then) {
				return false;
			}
			if (right == SearchAction.And || right == SearchAction.Or || right == SearchAction.Not || right == SearchAction.Then) {
				return false;
			}

            if (right == SearchAction.FieldDefinition){
                return left != SearchAction.And && left != SearchAction.Or && left != SearchAction.BeginGrouping;
            }

			// we get here, each is either Literal, Processed, BeginGrouping, or EndGrouping.
			switch(left){
				case SearchAction.BeginGrouping:
					// ()
					// ((
					// (Hi
					return false;
				case SearchAction.EndGrouping:
					if (right == SearchAction.EndGrouping){
						// ))
						return false;
					} else if (right == SearchAction.BeginGrouping){
						// )(
						return true;
					} else {
						// )Hi
						return true;
					}
				case SearchAction.Literal:
				case SearchAction.Processed:
					if (right == SearchAction.EndGrouping){
						// Hi)
						return false;
					} else if (right == SearchAction.BeginGrouping){
						// Hi(
						return true;
					} else {
						// HiHi
						return true;
					}
				default:
                    throw new NotImplementedException(getDisplayMember("requiresOperator{default}", "SearchCommand.requiresOperator()'s switch(left) is not implemented for SearchAction. {0}", left.ToString()));
			}
		}

		/// <summary>
        /// Given a queue of commands (typically created by calling SearchCommand.Parse()), executes the commands against the given indexes and returns the results.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="indexes"></param>
		/// <returns></returns>
        public static IEnumerable<int> ExecuteAndResolve(Queue<SearchCommand<TKey, TValue>> queue, List<Index<TKey, TValue>> indexes, DataSet ds, SearchOptions opts) {
            var searchedKeywords = new Dictionary<string, SearchCommand<TKey, TValue>>();
            var cmd = executeAndResolve(queue, indexes, searchedKeywords, ds, opts);
            IEnumerable<int> resolvedIDs = cmd.GetResolvedIDs(ds, opts);
            return resolvedIDs;
        }

        private void copySearchResults(SearchCommand<TKey, TValue> otherCommand, SearchOptions opts) {
            // we can do the following only because both objects are of the same type.
            // otherwise the private modifier on the member variables would prevent it
            // _resolvedIDs = otherCommand._resolvedIDs;
            _resolvedIDsByIndex = otherCommand._resolvedIDsByIndex;
            // HitList = otherCommand.HitList;
            _hitsByIndex = otherCommand._hitsByIndex;
            Action = otherCommand.Action;
        }

        private string getCacheKey() {
            // Each index + resolver has its own cache manager instance.
            // So the key only has to narrow it down based on keyword-level attributes
            if (this.Keyword == null || this.Keyword.ToString().Trim().Length == 0) {
                return null;
            } else {
                if (this.IgnoreCase) {
                    // we're using backspace character (character code 8) as the delimiter because it's impossible to type in any language and highly unlikely it would be injected into the keyword value
                    return new StringBuilder(this.Keyword.ToString().ToLower())
                        .Append(((char)8))
                        .Append(this.KeywordOffset)
                        .Append(((char)8))
                        .Append(this.MatchMode)
                        .Append(((char)8))
                        .Append(this.RestrictToIndex ?? String.Empty)
                        .Append(((char)8))
                        .Append(this.FieldName ?? String.Empty).ToString();
                } else {
                    return new StringBuilder(this.Keyword.ToString().ToLower())
                        .Append(((char)8))
                        .Append(this.KeywordOffset)
                        .Append(((char)8))
                        .Append(this.MatchMode)
                        .Append(((char)8))
                        .Append(this.RestrictToIndex ?? String.Empty)
                        .Append(((char)8))
                        .Append(this.FieldName ?? String.Empty).ToString();
                }
            }
        }


        private static CacheManager getResolverCacheManager(short temporaryIndexID, string resolverName) {
            return CacheManager.Get("SearchAndResolve" + DELIMITER + temporaryIndexID + DELIMITER + resolverName) as CacheManager;
        }
        private List<int> getCachedResolverResult(short temporaryIndexID, string resolverName, SearchOptions opts) {

            if (_indexer.TermCachingEnabled) {
                var key = getCacheKey();
                if (key != null) {
                    return getResolverCacheManager(temporaryIndexID, resolverName)[key] as List<int>;
                }
            }
            return null;
        }

        private void saveCachedResolverResult(short temporaryIndexID, string resolverName, List<int> result, SearchOptions opts) {
            // only cache it if caching is enabled and it's at least of a minimum size
            if (_indexer.TermCachingEnabled && result.Count >= _indexer.TermCachingMinimum) {
                var key = getCacheKey();
                if (key != null) {
                    getResolverCacheManager(temporaryIndexID, resolverName)[key] = result;
                }
            }
        }

        private void searchAndResolve(List<Index<TKey, TValue>> indexes, DataSet ds, SearchOptions opts) {


            var total = 0;
            foreach (var idx in indexes) {
                // idList is auto-created if need be.
                // we append to the dictionary entry for _resolvedIDsByIndex here via reference of the local variable, idList
                var idList = GetResolvedIDsByIndex(idx.TemporaryID, opts);
                Resolver<TKey, TValue> resolver = null;

                bool passedThru = false;

                // see if it's in the cache
                var ret = getCachedResolverResult(idx.TemporaryID, this.ResolverName, opts);
                if (ret != null) {
                    idList.AddRange(ret);
                } else {

                    if (idx.Resolvers.TryGetValue(this.ResolverName, out resolver)) {

                        var resCacheMode = resolver.CacheMode;

                        if (opts != null && ((opts.PassThruLevel & DatabasePassthruLevel.Resolver) == DatabasePassthruLevel.Resolver)){
                            // caller is opting to skip any cached resolver information and go directly to the database
                            resCacheMode = ResolverCacheMode.None;
                            passedThru = true;
                        }


                        switch (resCacheMode) {
                            case ResolverCacheMode.None:
                                switch (resolver.Method) {
                                    case ResolutionMethod.Sql:
                                        // go to database to resolve
                                        var resolvedIds = resolver.GetResolvedIDsFromDatabase(idx.GetPrimaryKeyIDs(this, ds, opts), opts);
                                        idList.AddRange(resolvedIds);
                                        break;
                                    case ResolutionMethod.ForeignKey:
                                    case ResolutionMethod.PrimaryKey:
                                        // need to go to index data and 
                                        // resolve via pk ids from the hits we found
                                        idList.AddRange(idx.GetResolvedIDsViaPrimaryOrForeignKey(this, ds, opts));
                                        break;
                                    case ResolutionMethod.Invalid:
                                    case ResolutionMethod.None:
                                        // nothing to do...
                                        break;
                                }
                                break;
                            case ResolverCacheMode.Id:
                                // need to go to index data and 
                                // resolve via pk ids from the hits we found
                                idList.AddRange(idx.GetResolvedIDsViaPrimaryOrForeignKey(this, ds, opts));
                                break;
                            case ResolverCacheMode.IdAndKeyword:
                                // use keyword tree to resolve
                                idList.AddRange(resolver.GetResolvedIDsViaIdAndKeyword(this.Keyword.ToString(), this.MatchMode, this.IgnoreCase, opts));
                                break;
                        }

                    }
                    if (!this.ReadFromDatabase) {
                        saveCachedResolverResult(idx.TemporaryID, this.ResolverName, idList, opts);
                    }
                }
                total += idList.Count;
            }


            this.Action = SearchAction.Processed;
        }


        private static CacheManager getHitCacheManager(short temporaryIndexID) {
            return CacheManager.Get("SearchHit" + DELIMITER + temporaryIndexID) as CacheManager;
        }
        private List<Hit> getCachedHitResult(short temporaryIndexID) {

            if (IgnoreCache) {
                return null;
            }

            if (_indexer.TermCachingEnabled) {
                var key = getCacheKey();
                if (key != null) {
                    return getHitCacheManager(temporaryIndexID)[key] as List<Hit>;
                }
            }
            return null;
        }
        private void saveCachedHitResult(short temporaryIndexID, List<Hit> result) {
            // only cache it if caching is enabled and it's at least of a minimum size
            if (_indexer.TermCachingEnabled && result.Count >= _indexer.TermCachingMinimum) {
                var key = getCacheKey();
                if (key != null) {
                    getHitCacheManager(temporaryIndexID)[key] = result;
                }
            }
        }
        private void search(List<Index<TKey, TValue>> indexes, DataSet ds, SearchOptions opts) {

            var total = 0;
            foreach (var idx in indexes) {
                // idList is auto-created if need be.
                // we append to the dictionary entry for _hitsByIndex here via reference of the local variable, idList
                var hitList = GetHitsByIndex(idx.TemporaryID, opts);

                // see if it's in the cache
                var ret = getCachedHitResult(idx.TemporaryID);
                if (ret != null) {
                    hitList.AddRange(ret);
                } else {
                    hitList.AddRange(idx.GetHits(this, ds, opts));
                }


                if (!this.ReadFromDatabase) {
                    // any queries that pulled from the database should not be cached
                    saveCachedHitResult(idx.TemporaryID, hitList);
                }
                total += hitList.Count;
            }


            this.Action = SearchAction.Processed;
        }







        private static SearchCommand<TKey, TValue> executeAndResolve(Queue<SearchCommand<TKey, TValue>> queue, List<Index<TKey, TValue>> indexes, Dictionary<string, SearchCommand<TKey, TValue>> searchedKeywords, DataSet ds, SearchOptions opts) {
            if (queue.Count == 0) {
                return new SearchCommand<TKey, TValue>();
            } else {

                // remove all disabled indexes from the search list
                for (int i = 0; i < indexes.Count; i++) {
                    if (!indexes[i].Enabled || !indexes[i].Loaded) {
                        indexes.RemoveAt(i);
                        i--;
                    }
                }

                SearchCommand<TKey, TValue> prevCmd = null;

                Queue<SearchCommand<TKey, TValue>> phase1Queue = new Queue<SearchCommand<TKey, TValue>>();



                // phase 1: gather all necessary data.  if possible, remember only ResolvedID's
                //          otherwise, remember Hits.  We have to remember Hits only if there is terms in quotes (i.e.
                //          (i.e. the hit information is needed to determine if keywordindexes are right)
                // phase 2: combine resolved id's or hits as needed
                // phase 3: process remaining hits to resolved id's if needed 
                //         (i.e. only if quoted terms are given and keyword index)

                // phase 1: gather all necessary data
                while (queue.Count > 0) {

                    // pull next cmd from process queue
                    SearchCommand<TKey, TValue> cmd = queue.Dequeue();

                    switch (cmd.Action) {
                        case SearchAction.Literal:

                            var cacheKey = cmd.getCacheKey();

                            if (searchedKeywords.ContainsKey(cacheKey)) {
                                // we've already resolved this term once in this query
                                cmd.copySearchResults(searchedKeywords[cacheKey], opts);
                            } else {

                                if (cmd.KeywordOffset == SearchCommandConstants.DETERMINE_KEYWORD_INDEX_FROM_CONTEXT) {
                                    // this command is in quotes, meaning we can't resolve anything yet.
                                    // just pull the hits and remember them.
                                    // we do this for each quoted command.
                                    // then, in the next phase, we will combine the hits across all consecutive commands
                                    // that are quoted, determine which ones match, then resolve them.
                                    cmd.search(indexes, ds, opts);
                                } else {

                                    // resolve the term
                                    cmd.searchAndResolve(indexes, ds, opts);


                                    // remember we resolved it
                                    // useful if query contains a list of similar items, like accession numbers => "PI 00000, PI 0001, PI 565555"
                                    // Indexer.TermCachingEnabled allows for caching across two different search queries and is togglable via the config.
                                    // The searchedKeywords variable is always within a given search query and is not togglable via the config.
                                    searchedKeywords[cacheKey] = cmd;
                                
                                }
                            }
                            break;

                        case SearchAction.Processed:
                            // nothing to do, this item has already been processed
                            Debug.WriteLine("Should we ever get here?");
                            break;

                        case SearchAction.BeginGrouping:

                            // create a sub queue -- all commands up to but excluding the EndGrouping (EndGrouping is eaten from the original queue)
                            Queue<SearchCommand<TKey, TValue>> groupQueue = getSubQueue(queue, true);

                            // execute the sub queue
                            var groupResult = executeAndResolve(groupQueue, indexes, searchedKeywords, ds, opts);
                            if (groupResult != null) {
                                cmd.copySearchResults(groupResult, opts);
                            }
                            cmd.Action = SearchAction.Processed;
                            break;

                        case SearchAction.Or:
                        case SearchAction.And:
                        case SearchAction.Then:
                        case SearchAction.Not:
                            // nothing to do for these in phase 1 of processing
                            break;

                        case SearchAction.FieldDefinition:

                            // should never ever get here

                            // notice this cmd is never queued up, but the next one will be.
                            // just assign the keyword as the fieldname for use later
                            // fieldName = cmd.FieldName == null ? null : cmd.FieldName.ToString();

                            throw new InvalidOperationException(getDisplayMember("executeAndResolve{fielddef}", "Bad search string parsing  -- found unexpected FieldDefinition command"));

                        default:
                            throw new NotImplementedException(getDisplayMember("executeAndResolve{default}", "Sorry, executeCommands() not yet implemented for {0}", cmd.Action.ToString()));

                    }


                    if (cmd.Action != SearchAction.FieldDefinition) {
                        phase1Queue.Enqueue(cmd);
                    }


                    prevCmd = cmd;

                }


                // phase 2: combine resolved id's or hits as needed
                var foundQuotedHits = false;
                var ret = new SearchCommand<TKey, TValue>();
                prevCmd = null;
                ret.Action = SearchAction.Processed;
                while (phase1Queue.Count > 0) {
                    var cmd = phase1Queue.Dequeue();
                    ret.ResolverName = cmd.ResolverName;
                    ret.Indexer = cmd.Indexer;
                    var nextCmd = (phase1Queue.Count == 0 ? null : phase1Queue.Peek());

                    switch (cmd.Action) {
                        case SearchAction.And:
                            if (nextCmd == null || prevCmd == null) {
                                throw new InvalidOperationException(getDisplayMember("executeAndResolve{badand}", "Bad search string parsing  -- found unexpected And command"));
                            } else {
                                ret.IntersectResolvedIDsRegardlessOfIndex(nextCmd._resolvedIDsByIndex);
                                //                                ret._resolvedIDs = ret.GetResolvedIDs().Intersect(nextCmd.GetResolvedIDs()).ToList();
                                phase1Queue.Dequeue();
                            }
                            break;
                        case SearchAction.Or:
                            if (nextCmd == null || prevCmd == null) {
                                throw new InvalidOperationException(getDisplayMember("executeAndResolve{bador}", "Bad search string parsing  -- found unexpected Or command"));
                            } else {
                                ret.ConcatResolvedIDsByIndex(nextCmd._resolvedIDsByIndex);
                                //                                ret._resolvedIDs.AddRange(nextCmd.GetResolvedIDs());
                                phase1Queue.Dequeue();
                            }
                            break;
                        case SearchAction.Not:
                            if (nextCmd == null || prevCmd == null) {
                                throw new InvalidOperationException(getDisplayMember("executeAndResolve{badnot}", "Bad search string parsing  -- found unexpected Not command"));
                            } else {
                                ret.ExceptResolvedIDsByIndex(nextCmd._resolvedIDsByIndex);
                                //                                ret._resolvedIDs = ret.GetResolvedIDs().Except(nextCmd.GetResolvedIDs()).ToList();
                                phase1Queue.Dequeue();
                            }
                            break;
                        case SearchAction.Processed:
                            if (cmd.KeywordOffset == SearchCommandConstants.IGNORE_KEYWORD_INDEX) {
                                
                                // not a quote-based item.
                                // just concat in the resolved ids
                                ret.ConcatResolvedIDsByIndex(cmd._resolvedIDsByIndex);

                            } else {

                                // we haven't resolved anything yet, we've just gathered up the hits.
                                // if there's no 'then' following the current cmd, we need to resolve them, then concat
                                foundQuotedHits = true;
                                if (ret._hitsByIndex == null || ret._hitsByIndex.Keys.Count == 0) {
                                    // first quoted word. just copy over the hit info
                                    ret._hitsByIndex = cmd._hitsByIndex;
                                } else {
                                    // next quoted word
                                    ret.IntersectHitsByIndexAndKeywordOffset(cmd._hitsByIndex);
                                }
                            }
                            break;
                        case SearchAction.Then:
                            // we should only kick out ids that all resolved from the same source index/field, as well as following a given 
                            if (prevCmd == null || nextCmd == null) {
                                throw new InvalidOperationException(getDisplayMember("executeAndResolve{badthen}", "Bad search string parsing -- found unexpected Then command"));
                            }
                            //} else {
                            //    ret.IntersectHitsByIndexAndKeywordOffset(prevCmd._hitsByIndex);
                            //    //ret.IntersectResolvedIDsByIndex(nextCmd._resolvedIDsByIndex);
                            //    foundQuotedHits = true;
                            //    phase1Queue.Dequeue();
                            //}
                            //                            throw new InvalidOperationException("Bad search string -- found unexpected Then command");
                            break;
                        default:
                            throw new NotImplementedException(getDisplayMember("executeAndResolve{badaction}", "phase 2 of executeAndResolve() has not implemented action={0}", cmd.Action.ToString()));
                    }
                    prevCmd = cmd;
                }


                // phase 3: process remaining hits to resolved id's if needed 
                if (foundQuotedHits) {
                    ret.resolveHitsByIndex(opts);
                }

                return ret;
            }
        }





























        private static int getBooleanCompareValue(string keyword) {
            if (String.IsNullOrEmpty(keyword)) {
                return 0;
            } else {
                switch (keyword.Trim().ToLower()) {
                    case "y":
                    case "true":
                    case "1":
                    case "x":
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        private IEnumerable<ResolvedHit> resolve(IEnumerable<Hit> hits, SearchOptions opts) {
            List<ResolvedHit> rhs = new List<ResolvedHit>();
            foreach (Hit h in hits) {
                ResolvedHit rh = new ResolvedHit(h, this.resolveHit(h, opts));
                rhs.Add(rh);
            }
            return rhs;
        }


        /// <summary>
        /// Returns a debugging-friendly representation of SearchCommand
        /// </summary>
        /// <returns></returns>
		public override string ToString() {
            return "Action=" + this.Action + " " + Toolkit.Coalesce(RestrictToIndex, "*") + "." + Toolkit.Coalesce(FieldName,"*") + " " + CompareMode + " " + this.Keyword + " (" + this.MatchMode.ToString() + ", IgnoreCase=" + this.IgnoreCase + ", KeywordIndex=" + this.KeywordOffset + ", Resolver=" + this.ResolverName;
		}


		#region IDisposable Members

		public void Dispose() {
			Indexer = null;
		}

		#endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "SearchCommand", resourceName, null, defaultValue, substitutes);
        }
    
    }
}
