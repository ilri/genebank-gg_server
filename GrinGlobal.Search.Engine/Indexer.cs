using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Xml.XPath;

using System.Data;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Collections.Specialized;
using System.Threading;
using GrinGlobal.Core.Xml;
namespace GrinGlobal.Search.Engine {
	public class Indexer<TKey, TValue> : IDisposable
		where TKey : ISearchable<TKey>, new()
		where TValue : IPersistable<TValue>, new() {

        public static DataSet CreateDefaultDataSet() {
            var ds = new DataSet();

            var dtIndexer = new DataTable("indexer");
            dtIndexer.Columns.Add("folder_name", typeof(string));
            dtIndexer.Columns.Add("enabled", typeof(bool));
            dtIndexer.Columns.Add("sql_statement", typeof(string));
            dtIndexer.Columns.Add("average_keyword_size", typeof(int));
            dtIndexer.Columns.Add("encoding", typeof(string));
            dtIndexer.Columns.Add("fanout_size", typeof(int));
            dtIndexer.Columns.Add("max_sort_size_in_mb", typeof(int));
            dtIndexer.Columns.Add("strip_html", typeof(bool));
            dtIndexer.Columns.Add("primary_key_field", typeof(string));
            dtIndexer.Columns.Add("keyword_cache_size", typeof(int));
            dtIndexer.Columns.Add("node_cache_size", typeof(int));
            dtIndexer.Columns.Add("auto_index_string_fields", typeof(bool));
            dtIndexer.Columns.Add("connection_string", typeof(string));
            dtIndexer.Columns.Add("provider_name", typeof(string));
            dtIndexer.Columns.Add("separators", typeof(string));
            dtIndexer.Columns.Add("punctuation", typeof(string));
            dtIndexer.Columns.Add("whitespace", typeof(string));
            dtIndexer.Columns.Add("stop_words", typeof(string));
            dtIndexer.Columns.Add("term_caching_enabled", typeof(bool));
            dtIndexer.Columns.Add("term_caching_minimum", typeof(int));
            dtIndexer.Columns.Add("sql_source", typeof(string));


            ds.Tables.Add(dtIndexer);

            var dtIndex = new DataTable("index");
            dtIndex.Columns.Add("index_name", typeof(string));
            dtIndex.Columns.Add("enabled", typeof(bool));
            dtIndex.Columns.Add("is_valid", typeof(bool));
            dtIndex.Columns.Add("invalid_reason", typeof(string));
            dtIndex.Columns.Add("sql_statement", typeof(string));
            dtIndex.Columns.Add("sqlserver_sql_statement", typeof(string));
            dtIndex.Columns.Add("oracle_sql_statement", typeof(string));
            dtIndex.Columns.Add("postgresql_sql_statement", typeof(string));
            dtIndex.Columns.Add("mysql_sql_statement", typeof(string));
            dtIndex.Columns.Add("average_keyword_size", typeof(int));
            dtIndex.Columns.Add("encoding", typeof(string));
            dtIndex.Columns.Add("fanout_size", typeof(int));
            dtIndex.Columns.Add("max_sort_size_in_mb", typeof(int));
            dtIndex.Columns.Add("strip_html", typeof(bool));
            dtIndex.Columns.Add("primary_key_field", typeof(string));
            dtIndex.Columns.Add("keyword_cache_size", typeof(int));
            dtIndex.Columns.Add("node_cache_size", typeof(int));
            dtIndex.Columns.Add("auto_index_string_fields", typeof(bool));
            dtIndex.Columns.Add("connection_string", typeof(string));
            dtIndex.Columns.Add("provider_name", typeof(string));
            dtIndex.Columns.Add("sql_source", typeof(string));
            dtIndex.Columns.Add("dataview_name", typeof(string));
            dtIndex.Columns.Add("separators", typeof(string));
            dtIndex.Columns.Add("punctuation", typeof(string));
            dtIndex.Columns.Add("whitespace", typeof(string));
            dtIndex.Columns.Add("stop_words", typeof(string));
            dtIndex.Columns.Add("last_rebuild_begin_date", typeof(DateTime));
            dtIndex.Columns.Add("last_rebuild_end_date", typeof(DateTime));
            dtIndex.Columns.Add("error_on_rebuild", typeof(string));
            dtIndex.Columns.Add("errors_since_last_rebuild", typeof(int));
            ds.Tables.Add(dtIndex);

            var dtIndexField = new DataTable("index_field");
            dtIndexField.Columns.Add("field_name", typeof(string));
            dtIndexField.Columns.Add("index_name", typeof(string));
            dtIndexField.Columns.Add("is_primary_key", typeof(bool));
            dtIndexField.Columns.Add("is_stored_in_index", typeof(bool));
            dtIndexField.Columns.Add("is_searchable", typeof(bool));
            dtIndexField.Columns.Add("format", typeof(string));
            dtIndexField.Columns.Add("type", typeof(string));
            dtIndexField.Columns.Add("is_boolean", typeof(bool));
            dtIndexField.Columns.Add("true_value", typeof(string));
            dtIndexField.Columns.Add("foreign_key_table", typeof(string));
            dtIndexField.Columns.Add("foreign_key_field", typeof(string));
            dtIndexField.Columns.Add("calculation", typeof(string));
            dtIndexField.Columns.Add("ordinal", typeof(int));
            ds.Tables.Add(dtIndexField);

            var dtResolver = new DataTable("resolver");
            dtResolver.Columns.Add("resolver_name", typeof(string));
            dtResolver.Columns.Add("index_name", typeof(string));
            dtResolver.Columns.Add("enabled", typeof(bool));
            dtResolver.Columns.Add("is_valid", typeof(bool));
            dtResolver.Columns.Add("invalid_reason", typeof(string));
            dtResolver.Columns.Add("sql_statement", typeof(string));
            dtResolver.Columns.Add("sqlserver_sql_statement", typeof(string));
            dtResolver.Columns.Add("oracle_sql_statement", typeof(string));
            dtResolver.Columns.Add("postgresql_sql_statement", typeof(string));
            dtResolver.Columns.Add("mysql_sql_statement", typeof(string));
            dtResolver.Columns.Add("average_keyword_size", typeof(int));
            dtResolver.Columns.Add("encoding", typeof(string));
            dtResolver.Columns.Add("fanout_size", typeof(int));
            dtResolver.Columns.Add("keyword_cache_size", typeof(int));
            dtResolver.Columns.Add("node_cache_size", typeof(int));
            dtResolver.Columns.Add("cache_mode", typeof(string));
            dtResolver.Columns.Add("allow_realtime_updates", typeof(bool));
            dtResolver.Columns.Add("data_location", typeof(string));
            dtResolver.Columns.Add("foreign_key_field", typeof(string));
            dtResolver.Columns.Add("method", typeof(string));
            dtResolver.Columns.Add("sql_source", typeof(string));
            dtResolver.Columns.Add("dataview_name", typeof(string));
            dtResolver.Columns.Add("primary_key_field", typeof(string));
            dtResolver.Columns.Add("resolved_primary_key_field", typeof(string));
            ds.Tables.Add(dtResolver);

            var dtStatus = new DataTable("status");
            dtStatus.Columns.Add("pending_realtime_updates", typeof(int));
            dtStatus.Columns.Add("connected_clients", typeof(int));
            dtStatus.Columns.Add("processing_indexes", typeof(int));
            ds.Tables.Add(dtStatus);

            return ds;
        }

		public static Encoding ParseEncoding(string encodingClassName) {

			Encoding encodingType = UTF8Encoding.UTF8;

			if (!String.IsNullOrEmpty(encodingClassName)) {
				switch (encodingClassName.ToLower()) {
					case "ascii":
						encodingType = ASCIIEncoding.ASCII;
						break;
					case "utf8":
						encodingType = UTF32Encoding.UTF8;
						break;
					case "utf16":
					case "unicode":
						encodingType = UnicodeEncoding.Unicode;
						break;
					case "utf32":
						encodingType = UTF32Encoding.UTF32;
						break;
					case "bigendianunicode":
						encodingType = UnicodeEncoding.BigEndianUnicode;
						break;
					default:
						try {
							encodingType = (Encoding)Toolkit.LoadClass(Type.GetType(encodingClassName));
						} catch {
							// they gave an invalid encoding. just eat it, use our default of utf8
						}
						break;
				}
			}

			return encodingType;

		}

		public delegate void ProgressEventHandler(object sender, ProgressEventArgs pea);

		public event ProgressEventHandler OnProgress;
		public string FolderName { get; set; }


        private Dictionary<string, Index<TKey, TValue>> _indexes;
        private Dictionary<short, Index<TKey, TValue>> _indexesByTemporaryID;


        /// <summary>
        /// Returns the Index based on its TemporaryID
        /// </summary>
        /// <param name="temporaryID"></param>
        /// <returns></returns>
        public Index<TKey, TValue> GetIndex(short temporaryID) {
            Index<TKey, TValue> ret = null;
            _indexesByTemporaryID.TryGetValue(temporaryID, out ret);
            return ret;
        }

        /// <summary>
        /// Returns the Index based on its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Index<TKey, TValue> GetIndex(string name) {
            Index<TKey, TValue> ret = null;
            _indexes.TryGetValue(name.ToLower(), out ret);
            return ret;
        }

        /// <summary>
        /// Gets the names of all indexes
        /// </summary>
        /// <returns></returns>
        public List<string> GetIndexNames() {
            return _indexes.Keys.ToList();
        }

        /// <summary>
        /// Gets the TemporaryIDs of all indexes
        /// </summary>
        /// <returns></returns>
        public List<short> GetIndexTemporaryIDs() {
            return _indexesByTemporaryID.Keys.ToList();
        }


        /// <summary>
        /// Adds the index to the Indexer.  Updates both the Indexes dictionary and the IndexesByTempID dictionary.
        /// </summary>
        /// <param name="index"></param>
        public void AddIndex(Index<TKey, TValue> index) {
            _indexes.Add(index.Name, index);
            _indexesByTemporaryID.Add(this.AssignNewTemporaryID(index), index);
        }

        /// <summary>
        /// Gets a random ID to assign to the Index.TemporaryID this is guaranteed to not be in the given list.  Auto-appends the return value to the list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public short AssignNewTemporaryID(Index<TKey, TValue> index) {
            Random rnd = new Random(DateTime.Now.Millisecond);
            // make sure we give the index a unique temporary id
            short tempID = (short)rnd.Next(0, (int)short.MaxValue);
            while(_indexesByTemporaryID.Keys.Contains(tempID)){
                // last generated one exists, 
                tempID = (short)rnd.Next(0, (int)short.MaxValue);
            }

            index.TemporaryID = tempID;

            return tempID;

        }


		/// <summary>
		/// Full path to the config file associated with this object.  May be null if Indexer was filled with LoadConfigFromXml().
		/// </summary>
		public string ConfigFilePath { get; private set; }

		/// <summary>
		/// How to connect to the database.  Represented in the config file as 2 properties -- ConnectionString (/Indexes/@ConnectionString) and ProviderName (/Indexes/@ProviderName)
		/// </summary>
		public DataConnectionSpec DataConnectionSpec { get; private set; }

		/// <summary>
		/// Number of keywords stored per node in the index (B+ tree) files.  Default is 100.  100-250 is a common value, but anything 3 or greater is functional.  Higher FanoutSize means fewer disk reads, but more list comparisons done per node. Maximum Fanout is Int16.MaxValue.  /Indexes/@FanoutSize
		/// </summary>
		public short FanoutSize { get; set; }

		/// <summary>
		/// Average number of bytes per keyword.  Larger number results in fewer node splits / relocations, but possibly wasted space within the node.  Default is 10.  /Indexes/@AverageKeywordSize
		/// </summary>
		public short AverageKeywordSize { get; set; }

        /// <summary>
        /// Gets or sets the size of cache for tree nodes.  NodeCache be auto filled upon Load() with breadth-first traversal of nodes up to NodeCacheSize
        /// </summary>
        public int NodeCacheSize { get; set; }

        /// <summary>
        /// Gets or sets the size of cache for keywords.  KeywordCache will be filled as requests come in, up to KeywordCacheSize.
        /// </summary>
        public int KeywordCacheSize { get; set; }

		/// <summary>
		/// When sorting data during index creation, this is the maximum number of MB to allow in RAM at a given point in time.  /Indexes/@MaxSortSizeInMB
		/// </summary>
		public int MaxSortSizeInMB { get; set; }

		/// <summary>
		/// Comma or line-separated list of stopwords.   /Indexes/StopWords
		/// </summary>
		public List<string> StopWords { get; private set; }

		/// <summary>
		/// All strings to interpret as separators for keywords -- will never appear in a keyword.   /Indexes/@Separators
		/// </summary>
        internal char[] Separators { get; private set; }

        /// <summary>
        /// List of characters, which if followed by one or more whitespace characters, should be treated as whitespace.  e.g.: Assuming colon (:) is defined as a Punctuation character, applying to "Per JohnQPublic: 3:1 ratio" would result in the following keywords -- {"Per", "JohnQPublic", "3:1", "ratio"}.  /Indexes/Index/@Punctuation
        /// </summary>
        internal char[] Punctuation { get; private set; }

        /// <summary>
        /// List of characters to be treated as whitespace.  If none are given, space ( ), new line (\n), carriage return (\r), and tab (\t) are assumed.
        /// </summary>
        internal char[] Whitespace { get; private set; }

        /// <summary>
        /// Returns list of keywords that are to be automatically cached at service startup time.  /Indexes/PrecacheKeywords
        /// </summary>
        internal List<PrecacheKeyword> PrecacheKeywords { get; private set; }

        /// <summary>
        /// Returns true if caching is enabled on a per-search-term basis.  Default is false.  /Indexes/@TermCachingEnabled
        /// </summary>
        internal bool TermCachingEnabled { get; private set; }

        /// <summary>
        /// Returns lowest number of resolved ids that must match a search term for it to be cached.  Default is 1500.  /Indexes/@TermCachingMinimum
        /// </summary>
        internal int TermCachingMinimum { get; private set; }

		/// <summary>
		/// If true, HTML will be stripped before being added to the index.  This means HTML elements are removed and HTML entities are expanded to their textual equivalent.   e.g. "&lt;i&gt;Mike &amp;amp; Ike&lt;i&gt;" will be indexed as three unique keywords -- "Mike", "&amp;", "Ike" -- assuming &amp; is not considered a separator.  /Indexes/@StripHtml
		/// </summary>
		internal bool StripHtml { get; private set; }

        /// <summary>
        /// If true, fields which return string data in the SQL will be automatically added to the list of fields (in /Indexes/Index/Fields).  Otherwise, only fields which are explicitly defined will be indexed (i.e. searchable).  /Indexes/@AutoIndexStringFields
        /// </summary>
        public bool AutoIndexStringFields { get; set; }

        /// <summary>
        /// Gets the location of the SQL used when creating indexes.  Defaults to Xml.
        /// </summary>
        public SqlSource SqlSource { get; private set; }

		private Indexer() {
			_indexes = new Dictionary<string, Index<TKey, TValue>>();
            _indexesByTemporaryID = new Dictionary<short, Index<TKey, TValue>>();
			StopWords = new List<string>();
		}


		public static Indexer<TKey, TValue> InitializeFromConfig(string xmlFileName, ref bool isDatabaseDown, out List<string> loadedIndexes, ProgressEventHandler handler) {
			string fileName = null;
			if (String.IsNullOrEmpty(xmlFileName)) {
				// they didn't give us a config file.  use one from app's config file, or default to gringlobal.search.config
				fileName = Toolkit.GetSetting("SearchEngineConfigFile", "~/gringlobal.search.config");
			} else {
				// use the one they gave us
				fileName = xmlFileName;
			}


			fileName = Toolkit.ResolveFilePath(fileName, false);
            Indexer<TKey, TValue> indexer = new Indexer<TKey, TValue>();
            indexer.ConfigFilePath = fileName;
            if (handler != null) {
                indexer.OnProgress += handler;
            }

            var ds = indexer.FillDataSetFromConfig(null, false, null, null);

            indexer.fillObjectFromDataSet(ds, ref isDatabaseDown);
            loadedIndexes = indexer.LoadAllIndexes(ref isDatabaseDown);

            return indexer;
		}

        public bool DeleteIndex(string indexName) {
            var idx = GetIndex(indexName);
            if (idx == null) {
                return false;
            }

            // first disassociate it from our indexer so we ignore any new search requests coming in
            _indexesByTemporaryID.Remove(idx.TemporaryID);
            _indexes.Remove(idx.Name);

            // then delete the index (it'll clean up after itself, including the config file)
            idx.Delete();

            return true;

        }

        public DataSet FillDataSetFromConfig(DataSet ds, bool enabledIndexesOnly, string onlyThisIndex, string onlyThisResolver) {

            if (ds == null) {
                ds = CreateDefaultDataSet();
            }

            var indexList = new List<string>();

            var xml = File.ReadAllText(this.ConfigFilePath);
            var conn = "";
            var providerName = "";
            var indexerFolderName = "";

            using (var sr = new StringReader(xml)) {
                XPathDocument doc = new XPathDocument(new XmlTextReader(sr));
                XPathNavigator nav = doc.CreateNavigator();
                XPathNavigator navIndexes = nav.SelectSingleNode("/Indexes");

                List<char> separators = new List<char>();
                List<char> punctuations = new List<char>();
                List<char> whiteSpaces = new List<char>();

                var dtIndexer = ds.Tables["indexer"];

                var drIndexer = dtIndexer.NewRow();
                dtIndexer.Rows.Add(drIndexer);

                if (navIndexes != null) {
                     indexerFolderName = navIndexes.GetAttribute("FolderName", "");
                     drIndexer["folder_name"] = indexerFolderName;
                    //if (Debugger.IsAttached) {
                    //    var f = navIndexes.GetAttribute("FolderNameDebug", "");
                    //    if (!String.IsNullOrEmpty(f)) {
                    //        drIndexer["folder_name"] = f;
                    //    }
                    //}

                    drIndexer["fanout_size"] = Toolkit.ToInt16(navIndexes.GetAttribute("FanoutSize", ""), 133);
                    drIndexer["average_keyword_size"] = Toolkit.ToInt16(navIndexes.GetAttribute("AverageKeywordSize", ""), 10);
                    drIndexer["node_cache_size"] = Toolkit.ToInt32(navIndexes.GetAttribute("NodeCacheSize", ""), this.FanoutSize + (this.FanoutSize * this.FanoutSize));
                    drIndexer["keyword_cache_size"] = Toolkit.ToInt32(navIndexes.GetAttribute("KeywordCacheSize", ""), 200);
                    drIndexer["max_sort_size_in_mb"] = Toolkit.ToInt32(navIndexes.GetAttribute("MaxSortSizeInMB", ""), 16);
                    drIndexer["strip_html"] = Toolkit.ToBoolean(navIndexes.GetAttribute("StripHtml", ""), true);
                    drIndexer["auto_index_string_fields"] = Toolkit.ToBoolean(navIndexes.GetAttribute("AutoIndexStringFields", ""), false);
                    conn = navIndexes.GetAttribute("ConnectionString", "");
                    drIndexer["connection_string"] = conn;
                    providerName = navIndexes.GetAttribute("ProviderName", "");
                    drIndexer["provider_name"] = providerName;

                    drIndexer["sql_source"] = Toolkit.ToEnum(typeof(SqlSource), navIndexes.GetAttribute("SqlSource", ""), (int)SqlSource.Xml).ToString();

                    drIndexer["term_caching_enabled"] = Toolkit.ToBoolean(navIndexes.GetAttribute("TermCachingEnabled", ""), true);
                    drIndexer["term_caching_minimum"] = Toolkit.ToInt32(navIndexes.GetAttribute("TermCachingMinimum", ""), 1500);


                    var tempSeps = ("" + navIndexes.GetAttribute("Separators", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                    drIndexer["separators"] = tempSeps;
                    separators.AddRange(tempSeps.ToCharArray());

                    string tempPunc = ("" + navIndexes.GetAttribute("Punctuation", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                    drIndexer["punctuation"] = tempPunc;
                    punctuations.AddRange(tempPunc.ToCharArray());

                    string tempWhitespace = ("" + navIndexes.GetAttribute("Whitespace", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                    drIndexer["whitespace"] = tempWhitespace;
                    whiteSpaces.AddRange(tempWhitespace.ToCharArray());
                }

                var navStopWords = navIndexes.Select("StopWords/StopWord");
                var sbStopWords = new StringBuilder();
                while (navStopWords.MoveNext()) {
                    sbStopWords.Append(navStopWords.Current.Value).Append("\t");
                }
                drIndexer["stop_words"] = sbStopWords.ToString().Trim();

                if (String.IsNullOrEmpty(drIndexer["connection_string"].ToString()) || String.IsNullOrEmpty(drIndexer["provider_name"].ToString())) {
                    // they didn't give us a connection string and provider name in the custom search engine config file.  try to pull it from the default app config.
                    try {
                        using (DataManager dm = DataManager.Create()) {
                            conn = dm.DataConnectionSpec.ConnectionString;
                            drIndexer["connection_string"] = conn;

                            providerName = dm.DataConnectionSpec.ProviderName;
                            drIndexer["provider_name"] = providerName;
                        }
                    } catch {
                        throw new InvalidOperationException(getDisplayMember("FillDataSetFromConfig{connection}", "The connection string to the database must be specified in the file pointed to by the SearchServiceConfigFile app setting or within the application's configuration file itself.  In the app.config file, it must be in the <connectionstrings> node.  If in the SearchServiceConfigFile, it must be in the /Indexes/@ConnectionString attribute.  The ProviderName (sqlserver, mysql, postgresql, oracle, etc) must be specified in the /Indexes/@ProviderName attribute as well."));
                    } finally {
                    }
                }


                XPathNodeIterator itIndexes = navIndexes.Select("Index");
                while (itIndexes.MoveNext()) {
                    // loads index data from the given xml
                    // and also looks up the associated <indexname>.xml file for additional data if it exists

                    var readit = false;
                    var name = itIndexes.Current.GetAttribute("Name", "");
                    var enabled = itIndexes.Current.GetAttribute("Enabled", "");
                    if (!enabledIndexesOnly) {
                        readit = true;
                    } else {
                        if (enabled.ToLower() == "true") {
                            readit = true;
                        }
                    }
                    if (String.IsNullOrEmpty(onlyThisIndex)) {
                        readit = true;
                    } else {
                        if (name.ToLower() == onlyThisIndex.ToLower()) {
                            readit = true;
                        } else {
                            // they specified an index name and it doesn't match this one -- so ignore the enabledIndexOnly
                            readit = false;
                        }
                    }

                    if (readit) {
                        indexList.Add(name);
                    }
                }
            }

            bool isDatabaseDown = false;
            foreach (var idxName in indexList) {
                Index<TKey, TValue>.FillDataSetFromConfig(ds, idxName, ConfigFilePath, indexerFolderName, providerName, conn, onlyThisIndex, onlyThisResolver, ref isDatabaseDown);
            }
            return ds;
        }

        public void SaveConfig(DataRow dr) {
            var doc = new XmlDocument();
            doc.Load(this.ConfigFilePath);
            var nd = doc.DocumentElement;

//            Toolkit.SetAttValue(nd, "FolderName", dr["folder_name"].ToString());

//            Toolkit.SetAttValue(nd, "FanoutSize", dr["fanout_size"].ToString());
//            Toolkit.SetAttValue(nd, "AverageKeywordSize", dr["average_keyword_size"].ToString());
//            Toolkit.SetAttValue(nd, "NodeCacheSize", dr["node_cache_size"].ToString());
//            Toolkit.SetAttValue(nd, "KeywordCacheSize", dr["keyword_cache_size"].ToString());
//            Toolkit.SetAttValue(nd, "MaxSortSizeInMB", dr["max_sort_size_in_mb"].ToString());
//            Toolkit.SetAttValue(nd, "StripHtml", dr["strip_html"].ToString());
//            Toolkit.SetAttValue(nd, "AutoIndexStringFields", dr["auto_index_string_fields"].ToString());
            Toolkit.SetAttValue(nd, "ConnectionString", dr["connection_string"].ToString());
            Toolkit.SetAttValue(nd, "ProviderName", dr["provider_name"].ToString());
            Toolkit.SetAttValue(nd, "TermCachingEnabled", dr["term_caching_enabled"].ToString());
            Toolkit.SetAttValue(nd, "TermCachingMinimum", dr["term_caching_minimum"].ToString());
//            Toolkit.SetAttValue(nd, "SqlSource", dr["sql_source"].ToString());
            Toolkit.SetAttValue(nd, "Separators", dr["separators"].ToString());
//            Toolkit.SetAttValue(nd, "Punctuation", dr["punctuation"].ToString());
//            Toolkit.SetAttValue(nd, "Whitespace", dr["whitespace"].ToString());

            XmlNode ndStopWords = doc.SelectSingleNode("/Indexes/StopWords");
            if (ndStopWords == null) {
                ndStopWords = doc.CreateElement("StopWords");
                doc.DocumentElement.AppendChild(ndStopWords);
            } else {
                ndStopWords.RemoveAll();
            }

            var stopWords = dr["stop_words"].ToString().Split('\t');

            foreach (string stopWord in stopWords) {
                var ndsw = doc.CreateElement("StopWord");
                var txt = doc.CreateTextNode(stopWord);
                ndsw.AppendChild(txt);
                ndStopWords.AppendChild(ndsw);
            }

            doc.Save(this.ConfigFilePath);
        }

        /// <summary>
        /// Writes the xml file with values pulled from the given settings dictionary.  If no dictionary is given, loads one from the xml file and the current Indexer object property values override each property as needed.
        /// </summary>
        /// <param name="settings"></param>
		public void SaveConfig(Dictionary<string, string> settings) {

            var doc = new XmlDocument();
            doc.Load(this.ConfigFilePath);
            var nd = doc.DocumentElement;

            Toolkit.SetAttValue(nd, "FolderName", this.FolderName);

            Toolkit.SetAttValue(nd, "FanoutSize", this.FanoutSize.ToString());
            Toolkit.SetAttValue(nd, "AverageKeywordSize", this.AverageKeywordSize.ToString());
            Toolkit.SetAttValue(nd, "NodeCacheSize", this.NodeCacheSize.ToString());
            Toolkit.SetAttValue(nd, "KeywordCacheSize", this.KeywordCacheSize.ToString());
            Toolkit.SetAttValue(nd, "MaxSortSizeInMB", this.MaxSortSizeInMB.ToString());
            Toolkit.SetAttValue(nd, "StripHtml", this.StripHtml.ToString());
            Toolkit.SetAttValue(nd, "AutoIndexStringFields", this.AutoIndexStringFields.ToString());
            Toolkit.SetAttValue(nd, "ConnectionString", this.DataConnectionSpec.ConnectionString);
            Toolkit.SetAttValue(nd, "ProviderName", this.DataConnectionSpec.ProviderName);
            Toolkit.SetAttValue(nd, "TermCachingEnabled", this.TermCachingEnabled.ToString());
            Toolkit.SetAttValue(nd, "TermCachingMinimum", this.TermCachingMinimum.ToString());
            Toolkit.SetAttValue(nd, "SqlSource", this.SqlSource.ToString());
            Toolkit.SetAttValue(nd, "Separators", Toolkit.Join(this.Separators, "", ""));
            Toolkit.SetAttValue(nd, "Punctuation", Toolkit.Join(this.Punctuation, "", ""));
            Toolkit.SetAttValue(nd, "Whitespace", Toolkit.Join(this.Whitespace, "", ""));

            XmlNode ndStopWords = doc.SelectSingleNode("/Indexes/StopWords");
            if (ndStopWords == null) {
                ndStopWords = doc.CreateElement("StopWords");
                doc.DocumentElement.AppendChild(ndStopWords);
            } else {
                ndStopWords.RemoveAll();
            }

            foreach (string stopWord in this.StopWords) {
                var ndsw = doc.CreateElement("StopWord");
                ndsw.Value = stopWord;
                ndStopWords.AppendChild(ndsw);
            }

            doc.Save(this.ConfigFilePath);

		}

        private Indexer<TKey, TValue> fillObjectFromDataSet(DataSet ds, ref bool isDatabaseDown) {

            List<char> separators = new List<char>();
            List<char> punctuations = new List<char>();
            List<char> whiteSpaces = new List<char>();

            var dtIndexer = ds.Tables["indexer"];
            var drIndexer = dtIndexer.Rows[0];
            this.FolderName = drIndexer["folder_name"].ToString();


            this.DataConnectionSpec = new DataConnectionSpec();

            this.FanoutSize = Toolkit.ToInt16(drIndexer["fanout_size"], 133);
            this.AverageKeywordSize = Toolkit.ToInt16(drIndexer["average_keyword_size"], 10);
            this.NodeCacheSize = Toolkit.ToInt32(drIndexer["node_cache_size"], this.FanoutSize + (this.FanoutSize * this.FanoutSize));
            this.KeywordCacheSize = Toolkit.ToInt32(drIndexer["keyword_cache_size"], 200);
            this.MaxSortSizeInMB = Toolkit.ToInt32(drIndexer["max_sort_size_in_mb"], 16);
            this.StripHtml = Toolkit.ToBoolean(drIndexer["strip_html"], true);
            this.AutoIndexStringFields = Toolkit.ToBoolean(drIndexer["auto_index_string_fields"], false);
            this.DataConnectionSpec.ConnectionString = drIndexer["connection_string"].ToString();
            this.DataConnectionSpec.ProviderName = drIndexer["provider_name"].ToString();


            this.SqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), drIndexer["sql_source"].ToString(), (int)SqlSource.Xml));

            this.TermCachingEnabled = Toolkit.ToBoolean(drIndexer["term_caching_enabled"], true);
            this.TermCachingMinimum = Toolkit.ToInt32(drIndexer["term_caching_minimum"], 1500);


            string tempSeps = drIndexer["separators"].ToString();
            foreach (char c in tempSeps) {
                separators.Add(c);
            }

            string tempPunc = drIndexer["punctuation"].ToString();
            foreach (char c in tempPunc) {
                punctuations.Add(c);
            }

            string tempWhitespace = drIndexer["whitespace"].ToString();
            foreach (char c in tempWhitespace) {
                whiteSpaces.Add(c);
            }

            this.Whitespace = whiteSpaces.ToArray();
            this.Punctuation = punctuations.Except(whiteSpaces).ToArray();
            this.Separators = separators.Except(whiteSpaces).Except(punctuations).ToArray();

            this.PrecacheKeywords = new List<PrecacheKeyword>();
            //var navPrecache = navIndexes.Select("PrecacheKeywords/Keyword");
            //if (navPrecache != null) {
            //    while (navPrecache.MoveNext()) {
            //        this.PrecacheKeywords.Add(new PrecacheKeyword {
            //            Keyword = navPrecache.Current.Value,
            //            IgnoreCase = Toolkit.ToBoolean(navPrecache.Current.GetAttribute("IgnoreCase", ""), true),
            //            ResolverName = navPrecache.Current.GetAttribute("ResolverName", "")
            //        });
            //    }
            //}

            var sw = drIndexer["stop_words"].ToString();
            if (!String.IsNullOrEmpty(sw)) {
                this.StopWords.AddRange(sw.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries));
            }


            if (String.IsNullOrEmpty(this.DataConnectionSpec.ConnectionString) || String.IsNullOrEmpty(this.DataConnectionSpec.ProviderName)) {
                // they didn't give us a connection string and provider name in the custom search engine config file.  try to pull it from the default app config.
                try {
                    using (DataManager dm = DataManager.Create()) {
                        this.DataConnectionSpec = dm.DataConnectionSpec;
                    }
                } catch {
                    throw new InvalidOperationException(getDisplayMember("fillObjectFromDataSet", "The connection string to the database must be specified in the file pointed to by the SearchServiceConfigFile app setting or within the application's configuration file itself.  In the app.config file, it must be in the <connectionstrings> node.  If in the SearchServiceConfigFile, it must be in the /Indexes/@ConnectionString attribute.  The ProviderName (sqlserver, mysql, postgresql, oracle, etc) must be specified in the /Indexes/@ProviderName attribute as well."));
                } finally {
                }
            }

            var dtIndex = ds.Tables["index"];
            foreach (DataRow drIndex in dtIndex.Rows) {
                Index<TKey, TValue> index = Index<TKey, TValue>.FillObjectFromDataSet(this, ds, drIndex["index_name"].ToString(), ref isDatabaseDown, idx_OnProgress);
                this.AddIndex(index);
            }

            //var indexNames = Lib.GetAllSettingsByCategory(settings, "Indexes");

            //foreach (var key in indexNames.Keys) {
            //    //if (key.StartsWith("Index-")) {
            //    // loads index data from the given dictionary entries
            //    // and also looks up the associated <indexname>.xml file for additional data if it exists
            //    //var nm = key;
            //    //if (nm.Contains("Fields--") ||nm.Contains("-Resolvers--") || nm.Contains("-Resolver-")) {
            //    //    // skip it
            //    //} else {
            //    //    // 6 is length of "Index-"
            //    //    var len = nm.IndexOf("-Fields-") - 6;
            //    //    if (len < 0) {
            //    //        len = nm.IndexOf("--") - 6;
            //    //    }
            //    //    if (len > 0) {
            //    //var indexName = Toolkit.Cut(nm, 6, len);
            //    //if (!this._indexes.ContainsKey(indexName)) {
            //    Index<TKey, TValue> index = Index<TKey, TValue>.LoadFromSettings(this, settings, indexNames[key], ref isDatabaseDown, idx_OnProgress);
            //    this.AddIndex(index);
            //    //}
            //    //    }
            //    //}
            //    //}
            //}

            return this;

        }

		private void showProgress(ProgressEventArgs pea) {
			if (OnProgress != null) {
				OnProgress(this, pea);
			}
		}

        internal static string[] ParseKeywords(string word, Index<TKey, TValue> index) {

            return index.ParseKeywords(word);

        }

        internal string[] ParseKeywords(string word) {

            // split on whitespace and separators
            var temp = word.Split(this.Whitespace.Concat(this.Separators).ToArray(), true);

            // strip off any punctuation as needed
            var ret = new List<string>();
            foreach (var t in temp) {
                if (t.Length > 0) {
                    if (this.Punctuation.Contains(t[t.Length - 1])) {
                        // last char is a punctuation char, strip it off
                        var temp2 = t.Substring(0, t.Length - 1);
                        if (temp2 != String.Empty) {
                            ret.Add(temp2);
                        }
                    } else {
                        // doesn't end with punctuation, just add it as-is
                        ret.Add(t);
                    }
                }
            }
            // split on separators

            return ret.ToArray();
        }


		private void showProgress(string message, EventLogEntryType logType, bool writeToEventLog) {
			showProgress(new ProgressEventArgs(message, logType, writeToEventLog));
		}

		public List<string> LoadAllIndexes(ref bool isDatabaseDown) {
			int i = 0;
			List<string> indexList = new List<string>();

			foreach(string key in _indexes.Keys){
				Index<TKey, TValue> idx = _indexes[key];
				// make sure any resources it's currently tied to are released before we reload it
				idx.Dispose();
				idx.FolderName = this.FolderName;
				//idx.OnProgress += new Index<TKey, TValue>.ProgressEventHandler(idx_OnProgress);
				try {
					if (idx.Enabled && idx.IsConfigurationValid){

                        idx.Load(ref isDatabaseDown);

						indexList.Add(idx.Name);

					}
				} catch (Exception ex) {
					showProgress(getDisplayMember("LoadAllIndexes{failed}", "Could not load index {0}, continuing to next.  Reason: {1}", idx.Name, ex.Message), EventLogEntryType.Error, true);
				}
				i++;
			}
			return indexList;
		}

        internal void SwapIndexObjects(Index<TKey, TValue> oldIndex, Index<TKey, TValue> newIndex) {
            newIndex.TemporaryID = oldIndex.TemporaryID;
            _indexes[oldIndex.Name] = newIndex;
            _indexesByTemporaryID[oldIndex.TemporaryID] = newIndex;
        }

		public DataSet Search(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, string indexName, string resolverName, int offset, int limit, DataSet ds, string options) {
			List<string> names = new List<string>();
			names.Add(indexName);
			return Search(searchString, ignoreCase, autoAndConsecutiveLiterals, names, resolverName, offset, limit, ds, options);
		}

		public DataSet Search(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, int offset, int limit, DataSet ds, string options) {

			IEnumerable<int> ids = new List<int>().AsEnumerable();
			List<Index<TKey, TValue>> indexes = new List<Index<TKey, TValue>>();
			if (indexNames == null || indexNames.Count == 0) {
				// add all that are currently loaded
				foreach (string key in _indexes.Keys) {
                    if (_indexes[key].Enabled && _indexes[key].Loaded && _indexes[key].IsConfigurationValid) {
						indexes.Add(_indexes[key]);
					}
				}
			} else {
				foreach (string iname in indexNames) {

                    Index<TKey, TValue> idx = null;
                    if (_indexes.TryGetValue(iname.ToLower(), out idx)){;
					    if (idx != null) {
                            if (idx.Enabled && idx.Loaded && idx.IsConfigurationValid) {
							    indexes.Add(idx);
						    }
					    }
                    }
				}
			}

            // since the caller can specify how many items they want back,
            // we enforce an absolute upper bound so one client can't choke our server
            // by passing a really high limit
            int serverMaxLimit = Toolkit.GetSetting("MaximumItemCount", 100001);
            if (limit > serverMaxLimit || limit == 0) {
                limit = serverMaxLimit;
            }

            ds = appendTablesToSearchDataSet(ds);

            var opts = SearchOptions.Parse(Toolkit.ParsePairs<string>(("" + options).ToLower()));

            //if (Debugger.IsAttached) {
            //    opts.Add("parseonly", "true");
            //}
            
            var cmdQueue = SearchCommand<TKey, TValue>.Parse(searchString, ignoreCase, autoAndConsecutiveLiterals, indexes, resolverName, ds, opts, this);
            if (!opts.ParseOnly) {
                ids = SearchCommand<TKey, TValue>.ExecuteAndResolve(cmdQueue, indexes, ds, opts);
            }

            IEnumerable<int> uniqueIDs = ids.Distinct().OrderBy(i => i);

            var dtQuery = ds.Tables["SearchQuery"];
            if (dtQuery != null) {
                dtQuery.Rows[0]["totalFound"] = uniqueIDs.Count();
            }

            uniqueIDs = uniqueIDs.Skip(offset).Take(limit);

            var dt = ds.Tables["SearchResult"];
            foreach (var id in uniqueIDs) {
                dt.Rows.Add(id);
            }

            return ds;

		}

        private DataSet appendTablesToSearchDataSet(DataSet ds) {
            if (ds == null) {
                ds = new DataSet();
            }

            if (ds.Tables.Contains("SearchQuery")) {
                ds.Tables.Remove("SearchQuery");
            }
            // remember the query before and after parsing
            var dtQuery = new DataTable("SearchQuery");
            dtQuery.Columns.Add("searchString", typeof(string));
            dtQuery.Columns.Add("ignoreCase", typeof(bool));
            dtQuery.Columns.Add("autoAndConsecutiveLiterals", typeof(bool));
            dtQuery.Columns.Add("indexes", typeof(string));
            dtQuery.Columns.Add("resolverName", typeof(string));
            dtQuery.Columns.Add("parsedSearchString", typeof(string));
            dtQuery.Columns.Add("totalFound", typeof(int));
            ds.Tables.Add(dtQuery);

            // remember any passthru sql statement(s) we performed 
            if (ds.Tables.Contains("SqlStatement")) {
                ds.Tables.Remove("SqlStatement");
            }
            var dtSql = new DataTable("SqlStatement");
            dtSql.Columns.Add("result", typeof(string));
            dtSql.Columns.Add("tableName", typeof(string));
            dtSql.Columns.Add("fieldName", typeof(string));
            dtSql.Columns.Add("sql", typeof(string));
            dtSql.Columns.Add("message", typeof(string));
            dtSql.Columns.Add("rowCount", typeof(int));
            ds.Tables.Add(dtSql);

            // ...and of course the results
            if (ds.Tables.Contains("SearchResult")) {
                ds.Tables.Remove("SearchResult");
            }
            var dtIDs = new DataTable("SearchResult");
            dtIDs.Columns.Add("ID", typeof(int));
            ds.Tables.Add(dtIDs);

            return ds;

        }

		public void CreateAllIndexes() {
			int i = 0;
			foreach(string key in _indexes.Keys){
				Index<TKey, TValue> idx = _indexes[key];
				idx.FolderName = this.FolderName;
				//idx.OnProgress += new Index<TKey, TValue>.ProgressEventHandler(idx_OnProgress);
                if (idx.Enabled && idx.IsConfigurationValid) {
					idx.Rebuild(false);
				}
				i++;
			}
		}

		public void VerifyAllIndexes() {
			int i = 0;
			foreach (string key in _indexes.Keys) {
				Index<TKey, TValue> idx = _indexes[key];
				idx.FolderName = this.FolderName;
				//idx.OnProgress += new Index<TKey, TValue>.ProgressEventHandler(idx_OnProgress);
                if (idx.Enabled && idx.IsConfigurationValid) {
					idx.VerifyIntegrity(true, false);
				}
				i++;
			}
		}

		void idx_OnProgress(object sender, ProgressEventArgs pea) {
			showProgress(pea);
		}



		#region IDisposable Members

		public void Dispose() {
			foreach (string key in _indexes.Keys) {
				Index<TKey, TValue> idx = _indexes[key];
				if (idx != null) {
					idx.Dispose();
				}
			}
		}

		#endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "Indexer", resourceName, null, defaultValue, substitutes);
        }
    
    }
}
