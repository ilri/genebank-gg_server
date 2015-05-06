using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Xml;
using GrinGlobal.Core.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;
using System.Text.RegularExpressions;
using System.Threading;

using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine {
	public class Index<TKey, TValue> : IDisposable 
	where TKey : ISearchable<TKey>,  new()
	where TValue : IPersistable<TValue>, new()
	{

		#region Supporting Classes

		private class MainDataProcessingArguments {
			public long HitID;
			public SortedDictionary<TKey, long> KeywordHash;
			public BinaryReader HitMapReader;
			public BinaryWriter HitMapWriter;
			public BinaryReader KeywordReader;
			public BinaryWriter KeywordWriter;
		}

		#endregion Supporting Classes

		#region Properties

		public delegate void ProgressEventHandler(object sender, ProgressEventArgs pea);

		public event ProgressEventHandler OnProgress;

		private object lockForReading = new object();

        /// <summary>
        /// Gets the Indexer object that essentially hosts this object
        /// </summary>
        public Indexer<TKey, TValue> Indexer { get; private set; }

		/// <summary>
		/// Gets the location of the data.  If InSeparateFile, data is stored in a .data file.  If InLeaf, data is stored directly in the leaf itself.  Default is InSeparateFile.
		/// </summary>
		public DataLocation DataLocation { get; private set; }

		/// <summary>
		/// Gets or sets if the index is enabled for searching (allows you to disable an index w/o dropping all the configuration info from the config file)
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Returns true if this index is loaded and ready for searching
		/// </summary>
		public bool Loaded {
			get {
				return this.State == IndexState.Open;
			}
		}

		/// <summary>
		/// Gets the status of the Index.  Opening, Open, Closing, Closed.
		/// </summary>
		public IndexState State { get; private set; }

        /// <summary>
        /// Returns true if the index is configured properly, false otherwise.
        /// </summary>
        public bool IsConfigurationValid { get; private set; }

		/// <summary>
		/// Name of the folder the files this index requires are stored in.  /Indexes/FolderName
		/// </summary>
		public string FolderName { get; set; }

		/// <summary>
		/// Number of keywords stored per node in the index (B+ tree) files.  Default is 100.  100-250 is a common value, but anything 3 or greater is functional.  Higher FanoutSize means fewer disk reads, but more list comparisons done per node. Maximum Fanout is Int16.MaxValue.  /Indexes/@FanoutSize
		/// </summary>
		public short FanoutSize { get; set; }

		/// <summary>
		/// The encoding used to store character data in the files needed by the search engine.  /Indexes/Index/@Encoding
		/// </summary>
		public Encoding Encoding {get; set; }

		/// <summary>
		/// Average number of bytes per keyword.  Larger number results in fewer node splits / relocations, but possibly wasted space within the node.  Default is 10.  /Indexes/Index/@AverageKeywordSize
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

		private string _name;
		/// <summary>
		/// The name of the index.  /Indexes/Index/@Name
		/// </summary>
		public string Name { 
			get {
				return _name;
			} 
			set {
				_name = (value == null ? string.Empty : value.ToLower());
			}
		}

        /// <summary>
        /// Where the SQL is defined that is associated with this index.  Can be Internal (gringlobal.search.config -> /Indexes/Index/Sql), External ({vendorname}_queries.xml -> /{indexname}/Sql), or Dataview (stored in sys_dataview table with a dataview named "search_{indexname}")
        /// </summary>
        public SqlSource SqlSource { get; private set; }

        /// <summary>
        /// Gets or sets the name of the dataview
        /// </summary>
        public string DataviewName { get; set; }

		/// <summary>
		/// SQL to execute to create this index.  /Indexes/Index/Sql
		/// </summary>
		public string Sql { 
            get {
                var dbDown = false;
                return GetSqlByEngine(Indexer.DataConnectionSpec.ProviderName, ref dbDown);
            }
            set {
                _sqlStatements[Indexer.DataConnectionSpec.ProviderName] = value;
            }
        }

        /// <summary>
        /// Returns SQL corresponding to the given engine name.
        /// </summary>
        /// <param name="engineName"></param>
        /// <returns></returns>
        internal string GetSqlByEngine(string engineName, ref bool isDatabaseDown) {
            string val = "";
            if (_sqlStatements.Count == 0 && this.SqlSource == SqlSource.Dataview) {
                // hasn't been filled yet.  fill it now.
                loadSqlFromDataview(this, ref isDatabaseDown);
            }
            _sqlStatements.TryGetValue(engineName, out val);
            return val;
        }

        private static string loadSqlFromDataview(Index<TKey, TValue> idx, ref bool isDatabaseDown) {
            var msg = "";
            // look in dataview definition table (sys_dataview)
            if (!isDatabaseDown) {
                try {
                    using (var dm = DataManager.Create(idx.Indexer.DataConnectionSpec)) {
                        var dt = dm.Read(@"
select 
    sql_statement,
    database_engine_tag
from 
    sys_dataview sd left join sys_dataview_sql sds
    on sd.sys_dataview_id = sds.sys_dataview_id
where
    sd.dataview_name = :dvname
", new DataParameters(
         ":dvname", idx.DataviewName));

                        if (dt.Rows.Count == 0) {
                            msg = getDisplayMember("loadSqlFromDataview{nodata}", "Configuration error in Index={0}: SqlSource is defined as Dataview, but no dataview named '{1}' could be located in table sys_dataview.\n", idx.Name , idx.DataviewName);
                            idx.showProgress(msg, EventLogEntryType.Error, true);
                        } else {
                            var found = false;
                            foreach (DataRow dr in dt.Rows) {

                                idx.appendSqlStatement(dr["database_engine_tag"].ToString().Trim(), dr["sql_statement"].ToString().Trim(), true);

                                if (dr["database_engine_tag"].ToString().ToLower() == idx.Indexer.DataConnectionSpec.ProviderName.ToLower()) {
                                    found = true;
                                }

                            }

                            if (!found) {
                                msg = getDisplayMember("loadSqlFromDataview{notfound}", "Configuration error in Index={0}: SqlSource is defined as Dataview, but the dataview named '{1}' does not contain sql in table sys_dataview_sql for database_engine_tag = '{2}'.", idx.Name, idx.DataviewName, dm.DataConnectionSpec.ProviderName);
                                idx.showProgress(msg, EventLogEntryType.Error, true);
                            }
                        }
                    }
                } catch (Exception exDB) {
                    msg = getDisplayMember("loadSqlFromDataview{failed}", "Configuration error in Index={0}: SqlSource is defined as Dataview, but an error occurred pulling the sys_dataview_sql.sql_statement value from the database: {1}", idx.Name, exDB.Message);
                    idx.showProgress(msg, EventLogEntryType.Error, true);
                    isDatabaseDown = true;
                }
            }
            return msg;
        }

        internal static void PullSqlFromDataviewIntoDataSet(DataRow dr, DataConnectionSpec dcs, string providerName, string dataviewName, ref bool isDatabaseDown) {
            if (!isDatabaseDown) {
                try {
                    using (var dm = DataManager.Create(dcs)) {
                        var dt = dm.Read(@"
select 
    sql_statement 
from 
    sys_dataview sd inner join sys_dataview_sql sds
    on sd.sys_dataview_id = sds.sys_dataview_id
    and sds.database_engine_tag = :dbenginecode
where
    sd.dataview_name = :dvname
", new DataParameters(
     ":dbenginecode", providerName,
     ":dvname", dataviewName));

                        if (dt.Rows.Count == 0 || String.IsNullOrEmpty(dt.Rows[0]["sql_statement"].ToString())) {
                            //msg = "Configuration error in Index=" + idx.Name + ": SqlSource is defined as Dataview, but no dataview named 'search_" + idx.Name + "' could be located in sys_dataview and sys_dataview_sql for database_engine_tag = '" + dm.DataConnectionSpec.ProviderName + "'.";
                            //idx.showProgress(msg, EventLogEntryType.Error, true);
                        } else {
                             dr[providerName + "_sql_statement"] = dt.Rows[0]["sql_statement"].ToString().Trim();
                        }
                    }
                } catch (Exception exDB) {
                    //msg = "Configuration error in Index=" + idx.Name + ": SqlSource is defined as Dataview, but an error occurred pulling the sys_dataview_sql.sql_statement value from the database: " + exDB.Message;
                    //idx.showProgress(msg, EventLogEntryType.Error, true);
                    Debug.WriteLine(exDB.ToString());
                    isDatabaseDown = true;
                    // throw;
                }
            }
        }

//        private static void loadSqlSettingsFromDataview(Dictionary<string, string> settings, string category, DataConnectionSpec dcs, string providerName, string dataviewName, ref bool isDatabaseDown) {
//            if (!isDatabaseDown) {
//                try {
//                    using (var dm = DataManager.Create(dcs)) {
//                        var dt = dm.Read(@"
//select 
//    sql_statement 
//from 
//    sys_dataview sd inner join sys_dataview_sql sds
//    on sd.sys_dataview_id = sds.sys_dataview_id
//    and sds.database_engine_tag = :dbenginecode
//where
//    sd.dataview_name = :dvname
//", new DataParameters(
//     ":dbenginecode", providerName,
//     ":dvname", dataviewName));

//                        if (dt.Rows.Count == 0 || String.IsNullOrEmpty(dt.Rows[0]["sql_statement"].ToString())) {
//                            //msg = "Configuration error in Index=" + idx.Name + ": SqlSource is defined as Dataview, but no dataview named 'search_" + idx.Name + "' could be located in sys_dataview and sys_dataview_sql for database_engine_tag = '" + dm.DataConnectionSpec.ProviderName + "'.";
//                            //idx.showProgress(msg, EventLogEntryType.Error, true);
//                        } else {
//                            Lib.AddSettingAsNeeded(settings, category, "SqlFor-" + providerName, dt.Rows[0]["sql_statement"].ToString().Trim(), true);
//                        }
//                    }
//                } catch (Exception exDB) {
//                    //msg = "Configuration error in Index=" + idx.Name + ": SqlSource is defined as Dataview, but an error occurred pulling the sys_dataview_sql.sql_statement value from the database: " + exDB.Message;
//                    //idx.showProgress(msg, EventLogEntryType.Error, true);
//                    Debug.WriteLine(exDB.ToString());
//                    isDatabaseDown = true;
//                    // throw;
//                }
//            }
//        }

        /// <summary>
        /// Sets given SQL in given engineName
        /// </summary>
        /// <param name="engineName"></param>
        /// <param name="sql"></param>
        internal void SetSqlByEngine(string engineName, string sql) {
            _sqlStatements[engineName] = sql;
        }

		/// <summary>
		/// When sorting data during creation, this is the maximum number of MB to allow in RAM at a given point in time.  /Indexes/Index/@MaxSortSizeInMB
		/// </summary>
		public int MaxSortSizeInMB { get; set; }

		/// <summary>
		/// List of all fields in the database associated with this index
		/// </summary>
		public List<Field> DatabaseFields { get; private set; }

        /// <summary>
        /// List of all fields in this index that point to another index (to tunnel back from Resolvers during updates)
        /// </summary>
        public List<Field> ForeignKeyFields { get; private set; }

		/// <summary>
		/// List of all fields that are searchable by this index
		/// </summary>
		internal List<Field> SearchableFields { get; private set; }

		/// <summary>
		/// List of all fields that are found in the config file for this index.  /Indexes/Index/Fields
		/// </summary>
		internal List<Field> ConfiguredFields { get; private set; }

		/// <summary>
		/// List of fields whose values are stored directly in the index
		/// </summary>
		public List<Field> IndexedFields { get; private set; }

        private Field _pkField;

		/// <summary>
		/// Resolvers defined for this index.  /Indexes/Index/Resolvers
		/// </summary>
		public Dictionary<string, Resolver<TKey, TValue>> Resolvers { get; private set; }

		/// <summary>
		/// List of characters to interpret as separators for keywords at index creation time -- will never appear in a keyword.   /Indexes/Index/@Separators
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
		/// If true, HTML will be decoded before being added to the index.  This means HTML elements are removed and HTML entities are expanded to their textual equivalent.   e.g. "&lt;i&gt;Mike &amp;amp; Ike&lt;i&gt;" will be indexed as three unique keywords -- "Mike", "&amp;", "Ike" -- assuming &amp; is not considered a separator.  /Indexes/Index/@StripHtml
		/// </summary>
		internal bool StripHtml { get; private set; }

        /// <summary>
        /// If false, only fields which are explicitly defined in /Indexes/Index/Fields will be indexed (i.e. searchable).  Otherwise fields which store string data will be automatically indexed.  /Indexes/Index/@AutoIndexStringFields
        /// </summary>
        public bool AutoIndexStringFields { get; set; }

        /// <summary>
        /// Required.  Gets or sets the name of the primary key field for this index.  Must name a field that is a unique signed 32-bit integer.
        /// </summary>
        public string PrimaryKeyField { get; set; }

		/// <summary>
		/// Comma or line-separated list of stopwords, contained in the Index/StopWords node (defaults to values from Indexes/StopWords node)
		/// </summary>
		internal List<string> StopWords { get; private set; }

		// these are the offsets within the .dat file of the associate word (to recreate IndexItem objects from)
		private BPlusTree<TKey, TValue> _keywordTree;

		private BinaryReader _dataReader;
        private BinaryWriter _dataWriter;

		private static string[] EMPTY_STRING_ARRAY = new string[0];
		private static Regex __htmlTagRemover = new Regex(@"<([/A-Z][A-Z0-9]*)\b[^>]*/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Gets the temporary ID associated with this particular instance of the Index object.  Used to associates Hit objects with this object cheaply (i.e. think of it as a really lightweight WeakReference -- one that simply allows us to see if two Hit objects are from the same Index object)
        /// </summary>
        public short TemporaryID { get; internal set; }

		#endregion Properties



		internal Index(Indexer<TKey, TValue> indexer) {
            Indexer = indexer;
			State = IndexState.Closed;
			DatabaseFields = new List<Field>();
            ForeignKeyFields = new List<Field>();
			SearchableFields = new List<Field>();
			IndexedFields = new List<Field>();
			ConfiguredFields = new List<Field>();
			Resolvers = new Dictionary<string, Resolver<TKey, TValue>>();
            _pkField = null;
		}

        internal static void FillDataSetFromConfig(DataSet ds, string indexName, string configFilePath, string indexerFolderName, string providerName, string connectionString, string onlyThisIndex, string onlyThisResolver, ref bool isDatabaseDown) {
            var xd = new XmlDocument();
            xd.Load(configFilePath);
            var ndIdx = xd.SelectSingleNode("/Indexes/Index[@Name='" + indexName + "']");
            if (ndIdx != null) {

                var drIndexer = ds.Tables["indexer"].Rows[0];

                var dtIndex = ds.Tables["index"];
                var drIndex = dtIndex.NewRow();
                dtIndex.Rows.Add(drIndex);

                drIndex["index_name"] = indexName;

                var dtField = ds.Tables["index_field"];
                var dtResolver = ds.Tables["resolver"];

                string xmlFile = Toolkit.ResolveFilePath(indexerFolderName + @"\" + indexName + ".xml", false);

                // try to load the index-specific xml file if possible

                var databaseFields = new Dictionary<string, Field>();
                if (!String.IsNullOrEmpty(xmlFile) && File.Exists(xmlFile)) {
                    // if the xml definition file exists, use it as the base for fields collections
                    Document doc = new Document();
                    doc.Load(xmlFile);
                    var autoIndexStringFields = Toolkit.GetAttValue(ndIdx, "AutoIndexStringFields", "");
                    if (String.IsNullOrEmpty(autoIndexStringFields)) {
                        autoIndexStringFields = drIndexer["auto_index_string_fields"].ToString();
                    }
                    var autoIndex = Toolkit.ToBoolean(autoIndexStringFields, false);

                    var nodeList = doc.Root.Nodes["Fields"].Nodes;
                    for (var i = 0; i < nodeList.Count; i++) {

                        var nd = nodeList[i];

                        Field fld = Field.FromDefinitionFileNode(nd);
                        databaseFields.Add(fld.Name, fld);
                        fld.FillDataSet(ds, indexName, false);

                    }
                }

                // use values from xml file specifically for this index node if given, default to indexer values otherwise
                var fanoutSize = Toolkit.ToInt32(Toolkit.GetAttValue(ndIdx, "FanoutSize", ""), 0);
                if (fanoutSize < 1) {
                    fanoutSize = Toolkit.ToInt32(drIndexer["fanout_size"].ToString(), 133);
                }
                drIndex["fanout_size"] = fanoutSize;

                var averageKeywordSize = Toolkit.ToInt32(Toolkit.GetAttValue(ndIdx, "AverageKeywordSize", ""), 0);
                if (averageKeywordSize < 1) {
                    averageKeywordSize = Toolkit.ToInt32(drIndexer["average_keyword_size"].ToString(), 5);
                }
                drIndex["average_keyword_size"] = averageKeywordSize;


                var nodeCacheSize = Toolkit.ToInt32(Toolkit.GetAttValue(ndIdx, "NodeCacheSize", ""), 0);
                if (nodeCacheSize < 1) {
                    nodeCacheSize = Toolkit.ToInt32(drIndexer["node_cache_size"], 10);
                }
                drIndex["node_cache_size"] = nodeCacheSize;

                var keywordCacheSize = Toolkit.ToInt32(Toolkit.GetAttValue(ndIdx, "KeywordCacheSize", ""), 0);
                if (keywordCacheSize < 1) {
                    keywordCacheSize = Toolkit.ToInt32(drIndexer["keyword_cache_size"], 10);
                }
                drIndex["keyword_cache_size"] = keywordCacheSize;


                drIndex["Encoding"] = Toolkit.GetAttValue(ndIdx, "Encoding", "");


                var maxSortSizeInMB = Toolkit.ToInt32(Toolkit.GetAttValue(ndIdx, "MaxSortSizeInMB", "0"), 0);
                if (maxSortSizeInMB < 1) {
                    maxSortSizeInMB = Toolkit.ToInt32(drIndexer["max_sort_size_in_mb"], 16);
                }
                drIndex["max_sort_size_in_mb"] = maxSortSizeInMB;

                drIndex["enabled"] = Toolkit.ToBoolean(Toolkit.GetAttValue(ndIdx, "Enabled", "false"), false);

                var sqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), Toolkit.GetAttValue(ndIdx, "SqlSource", ""), (int)SqlSource.Unknown));
                if (sqlSource == SqlSource.Unknown) {
                    sqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), drIndexer["sql_source"].ToString(), (int)SqlSource.Unknown));
                }
                drIndex["sql_source"] = sqlSource.ToString();

                var dataviewName = Toolkit.GetAttValue(ndIdx, "DataviewName", "");
                drIndex["dataview_name"] = dataviewName;


                // default to all separators defined @ indexer level (which includes the hardcoded ones)
                var separators = new List<char>();
                var indexerSeps = drIndexer["separators"].ToString().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in indexerSeps) {
                    separators.AddRange(s.ToCharArray());
                }

                // add to that list all separators defined in config file specifically for this index
                string tempSeps = ("" + Toolkit.GetAttValue(ndIdx, "Separators", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                foreach (char c in tempSeps) {
                    if (!separators.Contains(c)) {
                        separators.Add(c);
                    }
                }
                drIndex["separators"] = Toolkit.Join(separators.ToArray(), "\t", "");





                // default to all punctuation defined @ indexer level (which includes the hardcoded ones)
                var puncs = new List<char>();
                var indexerPuncs = drIndexer["punctuation"].ToString().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in indexerPuncs) {
                    puncs.AddRange(s.ToCharArray());
                }

                // add to that list all separators defined in config file specifically for this index
                string tempPunc = ("" + Toolkit.GetAttValue(ndIdx, "Punctuation", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                foreach (char c in tempPunc) {
                    if (!puncs.Contains(c)) {
                        puncs.Add(c);
                    }
                }
                drIndex["punctuation"] = Toolkit.Join(puncs.ToArray(), "\t", "");




                // default to all whitespace defined @ indexer level
                var whitespaces = new List<char>();
                var indexerWhitespaces = Toolkit.GetAttValue(ndIdx, "Whitespace", "").Split(new string[] { "=!=!=" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in indexerWhitespaces) {
                    whitespaces.AddRange(s.ToCharArray());
                }

                // add to that list all separators defined in config file specifically for this index
                string tempWhitespace = ("" + Toolkit.GetAttValue(ndIdx, "Whitespace", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                foreach (char c in tempWhitespace) {
                    if (!whitespaces.Contains(c)) {
                        whitespaces.Add(c);
                    }
                }
                drIndex["whitespace"] = Toolkit.Join(whitespaces.ToArray(), "=!=!=", "");


                var indexerStripHtml = drIndexer["strip_html"].ToString();
                drIndex["strip_html"] = Toolkit.Coalesce(Toolkit.GetAttValue(ndIdx, "StripHtml", ""), indexerStripHtml).ToString();

                var indexerAutoIndexStringFields = drIndexer["auto_index_string_fields"].ToString();
                drIndex["auto_index_string_fields"] = Toolkit.Coalesce(Toolkit.GetAttValue(ndIdx, "AutoIndexStringFields", ""), indexerAutoIndexStringFields).ToString();

                drIndex["primary_key_field"] = Toolkit.GetAttValue(ndIdx, "PrimaryKeyField", "");


                // default to all separators defined @ indexer level (which includes the hardcoded ones)
                var stopwords = new List<string>();
                var indexerStopWords = drIndexer["stop_words"].ToString().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                // add to that list all separators defined in config file specifically for this index
                string tempStops = ("" + Toolkit.GetAttValue(ndIdx, "StopWords", "")).Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
                foreach (var isw in indexerStopWords) {
                    if (!stopwords.Contains(isw)) {
                        stopwords.Add(isw);
                    }
                }
                drIndex["stop_words"] = Toolkit.Join(stopwords.ToArray(), "\t", "");


                var configuredFields = ndIdx.SelectNodes("./Fields/Field");
                foreach(XmlNode ndConfigField in configuredFields){
                    var fieldName = Toolkit.GetAttValue(ndConfigField, "Name", "");
                    Field dbField = null;
                    if (databaseFields.TryGetValue(fieldName, out dbField)) {
                        var f = Field.FromXmlNode(ndConfigField, dbField);
                        f.FillDataSet(ds, indexName, false);
                    }
                }





                var dcs = new DataConnectionSpec(providerName, null, null, connectionString);


                switch (sqlSource) {
                    case SqlSource.Unknown:
                    default:
                        //msg = "Configuration error in Index=" + indexName + ": SqlSource is not defined properly.";
                        //idx.showProgress(msg, EventLogEntryType.Error, true);
                        break;
                    //case SqlSource.Xml:
                    //    // look in config file itself
                    //    var sqlNodes = ndIdx.SelectNodes("./Sql");
                    //    foreach (XmlNode sqlNode in sqlNodes) {
                    //        if (sqlNode == null || String.IsNullOrEmpty(sqlNode.Value)) {
                    //            //msg = "Configuration error in Index=" + indexName + ": SqlSource is defined as Xml, but /Indexes/Index[@Name=" + idx.Name + "]/Sql is not provided.";
                    //            //idx.showProgress(msg, EventLogEntryType.Error, true);
                    //        } else {
                    //            var eng = Toolkit.GetAttValue(sqlNode, "engine", "");
                    //            if (String.IsNullOrEmpty(eng)) {
                    //                eng = providerName;
                    //            }

                    //            var sql = sqlNode.Value.Trim();

                    //            if (eng == "*") {
                    //                // special case...
                    //                Lib.AddSettingAsNeeded(settings, category, "SqlFor-mysql", sql, true);
                    //                Lib.AddSettingAsNeeded(settings, category, "SqlFor-postgresql", sql, true);
                    //                Lib.AddSettingAsNeeded(settings, category, "SqlFor-sqlserver", sql, true);
                    //                Lib.AddSettingAsNeeded(settings, category, "SqlFor-oracle", sql, true);
                    //            } else {
                    //                // general case
                    //                Lib.AddSettingAsNeeded(settings, category, "SqlFor-" + eng, sql, true);
                    //            }
                    //        }
                    //    }
                    //    break;
                    case SqlSource.Dataview:
                        // look in dataview definition table (sys_dataview)
                        // (only if they're loading just this index though, since it is expensive)
                        if (!String.IsNullOrEmpty(onlyThisIndex)) {
                            PullSqlFromDataviewIntoDataSet(drIndex, dcs, providerName, dataviewName, ref isDatabaseDown);
                        }
                        break;
                }

                foreach(XmlNode res in ndIdx.SelectNodes("./Resolvers/Resolver")){
                    var resName = Toolkit.GetAttValue(res, "Name", "");
                    if (String.IsNullOrEmpty(onlyThisResolver) || onlyThisResolver.ToLower() == resName.ToLower()) {
                        Resolver<TKey, TValue>.FillDataSetFromConfig(ds, indexName, res, resName, dcs, onlyThisIndex, onlyThisResolver, ref isDatabaseDown);
                    }
                }

                ValidateConfigurationByDataRow(drIndex, ds, providerName, onlyThisIndex, onlyThisResolver, ref isDatabaseDown);

                // read stats
                var statFile = Toolkit.ResolveFilePath(indexerFolderName + @"\" + indexName + ".stats", false);
                if (File.Exists(statFile)){
                    var stats = SerializableDictionary.Load(statFile);
                    foreach (string key in stats.Keys) {
                        switch (key.ToLower()) {
                            case "lastrebuildbegin":
                                drIndex["last_rebuild_begin_date"] = stats[key];
                                break;
                            case "lastrebuildend":
                                drIndex["last_rebuild_end_date"] = stats[key];
                                break;
                            case "errorssincerebuild":
                                drIndex["errors_since_last_rebuild"] = stats[key];
                                break;
                            case "erroronrebuild":
                                drIndex["error_on_rebuild"] = stats[key];
                                break;
                        }
                    }
                }
            }
        }

        internal static int getSystemCooperatorID(DataConnectionSpec dsc) {
            using (var dm = DataManager.Create(dsc)) {
                return Toolkit.ToInt32(dm.ReadValue("select cooperator_id from cooperator where last_name = 'SYSTEM'"), -1);
            }
        }


        internal static void saveSqlToDataview(string dataviewName, DataConnectionSpec dcs, Dictionary<string, string> sqlStatements) {

            // TODO: get the right coop id??
            var coopID = getSystemCooperatorID(dcs);

            // write sql to database.  grab/insert sys_dataview_id as needed.  update/insert sys_dataview_sql record as needed.
            using (var dm = DataManager.Create(dcs)) {
                var dvID = Toolkit.ToInt32(dm.ReadValue(@"select sys_dataview_id from sys_dataview where dataview_name = :dvname",
                    new DataParameters(":dvname", dataviewName.ToLower())), -1);
                if (dvID < 0) {

                    // dataview doesn't exist yet.  create it.
//insert into
//    sys_dataview
//(dataview_name, is_enabled, is_readonly, is_system, is_property_suppressed, is_user_visible, is_web_visible, category_name, is_transform, created_date, created_by, owned_date, owned_by)
//values
//(:dvname, 'Y', 'Y', 'Y', 'Y', 'N', 'N', 'search', 'N', :now1, :who1, :now2, :who2)
                    dvID = Toolkit.ToInt32(dm.Write(@"
insert into
    sys_dataview
(dataview_name, is_enabled, is_readonly, category_code, is_transform, created_date, created_by, owned_date, owned_by)
values
(:dvname, 'Y', 'Y', 'Search Engine', 'N', :now1, :who1, :now2, :who2)
", true, "sys_dataview_id", new DataParameters(
                        ":dvname", dataviewName, DbType.String,
                        ":now1", DateTime.UtcNow, DbType.DateTime2,
                        ":who1", coopID, DbType.Int32,
                        ":now2", DateTime.UtcNow, DbType.DateTime2,
                        ":who2", coopID, DbType.Int32)), -1);
                }

                foreach (var eng in sqlStatements.Keys) {

                    var sqlID = Toolkit.ToInt32(dm.ReadValue(@"
select 
    sys_dataview_sql_id 
from 
    sys_dataview_sql 
where 
    sys_dataview_id = :dvid
    and database_engine_tag = :engine
", new DataParameters(
                        ":dvid", dvID, DbType.Int32,
                        ":engine", eng, DbType.String
                        )), -1);

                    if (sqlID < 0) {
                        // need to create a new dataview sql record
                        sqlID = Toolkit.ToInt32(dm.Write(@"
insert into 
    sys_dataview_sql
(sys_dataview_id, database_engine_tag, sql_statement, created_date, created_by, owned_date, owned_by)
values
(:dvid, :engine, :sql, :now1, :who1, :now2, :who2)
", true, "sys_dataview_sql_id", new DataParameters(
                            ":dvid", dvID, DbType.Int32,
                            ":engine", eng, DbType.String,
                            ":sql", sqlStatements[eng] + "", DbType.String,
                            ":now1", DateTime.UtcNow, DbType.DateTime2,
                            ":who1", coopID, DbType.Int32,
                            ":now2", DateTime.UtcNow, DbType.DateTime2,
                            ":who2", coopID, DbType.Int32)), -1);

                    } else {
                        // need to update the existing dataview sql record
                        dm.Write(@"
update
    sys_dataview_sql
set
    sql_statement = :sql,
    modified_date = :now1,
    modified_by = :who1
where
    sys_dataview_sql_id = :sqlid
", new DataParameters(
                            ":sql", sqlStatements[eng] + "", DbType.String,
                            ":now1", DateTime.UtcNow, DbType.DateTime2,
                            ":who1", coopID, DbType.Int32,
                            ":sqlid", sqlID, DbType.Int32));
                    }
                }

            }
        }

        private SerializableDictionary _stats;
        private string _statFile;
        private string _errorFile;

        private object getStat(string name, object defaultValue) {
            if (_stats != null) {
                object ret = null;
                if (_stats.TryGetValue(name, out ret)) {
                    return ret;
                }
            }
            return defaultValue;
        }

        private void loadStats() {

            lock (_singleLocker) {
                _stats = SerializableDictionary.Load(_statFile);
                foreach (string key in _stats.Keys) {
                    switch (key.ToLower()) {
                        case "lastrebuildbegin":
                            LastRebuildBegin = Toolkit.ToDateTime(_stats[key], new DateTime(1900, 1, 1));
                            break;
                        case "lastrebuildend":
                            LastRebuildEnd = Toolkit.ToDateTime(_stats[key], new DateTime(1900, 1, 1));
                            break;
                        case "errorssincerebuild":
                            _errorsSinceRebuild = Toolkit.ToInt32(_stats[key], 0);
                            break;
                        case "erroronrebuild":
                            ErrorOnRebuild = _stats[key] as string;
                            break;
                    }
                }
            }
        }

        private void logError(string errorMessage) {
            try {
                lock (_singleLocker) {

                    if (!String.IsNullOrEmpty(_errorFile)) {
                        // log the error info to file
                        File.AppendAllText(_errorFile, DateTime.UtcNow.ToString("yyyy/MM/dd hh:mm:ss tt") + " -- " + errorMessage.Replace("\r", "\\r").Replace("\n", "\\n"));
                    }

                    // up the count since load
                    Interlocked.Add(ref _errorsSinceLoad, 1);
                    // up the total count since rebuild
                    Interlocked.Add(ref _errorsSinceRebuild, 1);
                    // save that count
                    saveStat("ErrorsSinceRebuild", _errorsSinceRebuild);

                }
            } catch (Exception ex) {
                // eat it...for whatever reason we're bombing hard on 
            }
        }

        private object _singleLocker = new object();

        private void saveStat(string name, object value) {
            lock (_singleLocker) {
                _stats[name] = value;
                _stats.Save(_statFile);
            }
        }

        /// <summary>
        /// Returns the UTC date/time of when the index finished its last rebuild from the database
        /// </summary>
        public DateTime LastRebuildEnd { get; private set; }

        /// <summary>
        /// Returns the UTC date/time of when the index began its last rebuild from the database
        /// </summary>
        public DateTime LastRebuildBegin { get; private set; }

        /// <summary>
        /// Gets the error text that occured during the last rebuild.  If no error occurred, will return string.Empty.
        /// </summary>
        public string ErrorOnRebuild { get; private set; }

        /// <summary>
        /// Returns the UTC date/time of when the index was last loaded from disk
        /// </summary>
        public DateTime LastLoaded { get; private set; }

        private int _errorsSinceRebuild;
        /// <summary>
        /// Returns the number of errors that have occurred since the index was rebuilt from the database
        /// </summary>
        public int ErrorsSinceRebuild {
            get { return _errorsSinceRebuild; }
        }

        private int _errorsSinceLoad;
        /// <summary>
        /// Returns number of errors that have occurred since the index was loaded into memory
        /// </summary>
        public int ErrorsSinceLoad {
            get { return _errorsSinceLoad; }
        }

        /// <summary>
        /// Fills index properties with values from given navigator.
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="indexer"></param>
        /// <returns></returns>
        internal static Index<TKey, TValue> FillObjectFromDataSet(Indexer<TKey, TValue> indexer, DataSet ds, string indexName, ref bool isDatabaseDown, ProgressEventHandler handler) {

            Index<TKey, TValue> idx = new Index<TKey, TValue>(indexer);
            idx.Name = indexName;
            idx.FolderName = indexer.FolderName;

            idx._errorFile = Toolkit.ResolveFilePath(indexer.FolderName + @"\" + idx.Name + ".errors", false);
            idx._statFile = Toolkit.ResolveFilePath(indexer.FolderName + @"\" + idx.Name + ".stats", false);
            idx.loadStats();

            if (handler != null) {
                idx.OnProgress += handler;
            }
            var drs = ds.Tables["index"].Select("index_name = '" + indexName + "'");
            if (drs.Length > 0) {
                var dr = drs[0];

                // use values from xml file specifically for this index node if given, default to indexer values otherwise
                idx.FanoutSize = Toolkit.ToInt16(dr["fanout_size"], indexer.FanoutSize);
                idx.AverageKeywordSize = Toolkit.ToInt16(dr["average_keyword_size"], indexer.AverageKeywordSize);
                idx.NodeCacheSize = Toolkit.ToInt32(dr["node_cache_size"], indexer.NodeCacheSize);
                idx.KeywordCacheSize = Toolkit.ToInt32(dr["keyword_cache_size"], indexer.KeywordCacheSize);
                idx.Encoding = Indexer<TKey, TValue>.ParseEncoding(dr["encoding"].ToString());
                idx.MaxSortSizeInMB = Toolkit.ToInt32(dr["max_sort_size_in_mb"], indexer.MaxSortSizeInMB);
                idx.Enabled = Toolkit.ToBoolean(dr["enabled"], true);

                idx.SqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), dr["sql_source"].ToString(), (int)indexer.SqlSource));

                idx.DataviewName = dr["dataview_name"].ToString();

                // default to all separators defined @ indexer level (which includes the hardcoded ones)
                var separators = new List<char>();
                separators.AddRange(indexer.Separators);

                // add to that list all separators defined in config file specifically for this index
                string tempSeps = dr["separators"].ToString();
                foreach (char c in tempSeps) {
                    if (!separators.Contains(c)) {
                        separators.Add(c);
                    }
                }
                idx.Separators = separators.ToArray();




                // default to all punctuation defined @ indexer level (which includes the hardcoded ones)
                var puncs = new List<char>();
                puncs.AddRange(indexer.Punctuation);

                // add to that list all separators defined in config file specifically for this index
                string tempPunc = dr["punctuation"].ToString();
                foreach (char c in tempPunc) {
                    if (!puncs.Contains(c)) {
                        puncs.Add(c);
                    }
                }
                idx.Punctuation = puncs.ToArray();




                // default to all whitespace defined @ indexer level
                var whitespaces = new List<char>();
                whitespaces.AddRange(indexer.Whitespace);

                // add to that list all whitespace defined in config file specifically for this index

                string tempWhitespaces = dr["whitespace"].ToString();
                foreach (char c in tempWhitespaces) {
                    if (!whitespaces.Contains(c)) {
                        whitespaces.Add(c);
                    }
                }
                idx.Whitespace = whitespaces.ToArray();


                idx.StripHtml = Toolkit.ToBoolean(dr["strip_html"], indexer.StripHtml);

                idx.AutoIndexStringFields = Toolkit.ToBoolean(dr["auto_index_string_fields"].ToString(), indexer.AutoIndexStringFields);
                idx.PrimaryKeyField = dr["primary_key_field"].ToString();


                idx._sqlStatements.Clear();
                var msg = "";
                var sqlFilePath = "";
                switch (idx.SqlSource) {
                    case SqlSource.Unknown:
                    default:
                        msg = getDisplayMember("FillObjectFromDataSet{unknown}", "Configuration error in Index={0}: SqlSource is not defined properly.", idx.Name);
                        idx.showProgress(msg, EventLogEntryType.Error, true);
                        break;
                    case SqlSource.Xml:
                        // look in config file itself
                        idx.appendSqlStatement("sqlserver", dr["sqlserver_sql_statement"].ToString(), true);
                        idx.appendSqlStatement("mysql", dr["mysql_sql_statement"].ToString(), true);
                        idx.appendSqlStatement("postgresql", dr["postgresql_sql_statement"].ToString(), true);
                        idx.appendSqlStatement("oracle", dr["oracle_sql_statement"].ToString(), true);
                        break;
                    case SqlSource.Dataview:

                        if (String.IsNullOrEmpty(idx.DataviewName)) {
                            msg = getDisplayMember("FillObjectFromDataSet{dataview}", "Configuration error in Index={0}: SqlSource is defined as Dataview, but the DataviewName is not specified.", idx.Name);
                            idx.showProgress(msg, EventLogEntryType.Error, true);
                        } else {

                            // 2010-08-02 Brock Weaver brock@circaware.com
                            //            moved sql loading for dataviews to method because 
                            //            if the db is unavailable (such as at boot time) it was causing the service to not start properly.

                            loadSqlFromDataview(idx, ref isDatabaseDown);

                        }
                        break;
                }


                string xmlFile = Toolkit.ResolveFilePath(indexer.FolderName + @"\" + idx.Name + ".xml", false);

                // try to load the index-specific xml file if possible
                var fileCreateWorked = true;
                if (!File.Exists(xmlFile)) {
                    // this index has never been created yet, meaning the definition file doesn't exist.
                    // just create stubs so the definition file is created properly.
                    // (this will fill the index DatabaseFields property et al so the index can be altered via admin tool)
                    fileCreateWorked = idx.createFiles(true);
                }

                if (!String.IsNullOrEmpty(xmlFile) && File.Exists(xmlFile)) {
                    // if the xml file exists, use it as the base for fields collections
                    Document doc = new Document();
                    doc.Load(xmlFile);
                    foreach (Node nd in doc.Root.Nodes["Fields"].Nodes) {
                        Field fld = Field.FromDefinitionFileNode(nd);
                        idx.DatabaseFields.Add(fld);
                        if (indexer.AutoIndexStringFields && fld.DataType == typeof(string)) {
                            idx.SearchableFields.Add(fld);
                        }
                        if (fld.StoredInIndex) {
                            idx.IndexedFields.Add(fld);
                        }
                        if (!String.IsNullOrEmpty(fld.ForeignKeyTable) && !String.IsNullOrEmpty(fld.ForeignKeyField)) {
                            idx.ForeignKeyFields.Add(fld);
                        }
                    }
                }


                idx.IsConfigurationValid = fileCreateWorked;

                idx.StopWords = indexer.StopWords;
                var stopwords = dr["stop_words"].ToString().Split(new string[] { "=!=!=" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var sw in stopwords) {
                    if (!idx.StopWords.Contains(sw)) {
                        idx.StopWords.Add(sw);
                    }
                }

                var drFields = ds.Tables["index_field"].Select("index_name = '" + indexName + "'");
                if (drFields.Length > 0) {
                    foreach (DataRow drField in drFields) {

                        Field f = Field.FillObjectFromDataSet(drField);
                        for (var i = 0; i < idx.DatabaseFields.Count; i++) {
                            var dbf = idx.DatabaseFields[i];
                            if (String.Compare(dbf.Name, f.Name, true) == 0) {
                                // this field already exists in db fields list.
                                // let the configuration file settings override the defintion file settings for this field
                                idx.DatabaseFields[i] = f;
                            }
                        }

                        idx.ConfiguredFields.Add(f);


                        if (f.Searchable) {
                            Field sf = idx.SearchableFields.Find(fld => {
                                return fld.Name.ToLower() == f.Name.ToLower();
                            });
                            if (sf != null) {
                                // remove the one added by the table xml file, replace with the configured one 
                                // (as it is based on the table xml file, but includes the config tweaks)
                                idx.SearchableFields.Remove(sf);
                            }
                            idx.SearchableFields.Add(f);
                        }

                        if (f.StoredInIndex) {
                            Field idxFld = idx.IndexedFields.Find(fld => {
                                return fld.Name.ToLower() == f.Name.ToLower();
                            });
                            if (idxFld != null) {
                                idx.IndexedFields.Remove(idxFld);
                            }
                            idx.IndexedFields.Add(f);
                        }

                        if (!String.IsNullOrEmpty(f.ForeignKeyTable) && !String.IsNullOrEmpty(f.ForeignKeyField)) {
                            Field fkFld = idx.ForeignKeyFields.Find(fld => {
                                return fld.Name.ToLower() == f.Name.ToLower();
                            });
                            if (fkFld != null) {
                                idx.ForeignKeyFields.Remove(fkFld);
                            }
                            idx.ForeignKeyFields.Add(f);
                        }
                    }
                }
            }

            var drResolvers = ds.Tables["resolver"].Select("index_name = '" + indexName + "'");
            foreach (DataRow drR in drResolvers) {
                Resolver<TKey, TValue> resolver = Resolver<TKey, TValue>.FillObjectFromDataSet(drR, indexName, drR["resolver_name"].ToString(), null, idx, ref isDatabaseDown);
                idx.Resolvers.Add(resolver.Name, resolver);
            }

            idx.ValidateConfiguration(true, ref isDatabaseDown);

            return idx;

        }

        private static bool ValidateConfigurationByDataRow(DataRow dr, DataSet ds, string providerName, string onlyThisIndex, string onlyThisResolver, ref bool isDatabaseDown) {

            var sb = new StringBuilder();

            var indexName = dr["index_name"].ToString();
            var sql = dr[providerName + "_sql_statement"].ToString();
            var sqlSource = (SqlSource)Toolkit.ToEnum(typeof(SqlSource), dr["sql_source"].ToString(), (int)SqlSource.Unknown);
            var dataviewName = dr["dataview_name"].ToString();

            switch (sqlSource) {
                case SqlSource.None:
                    break;
                case SqlSource.Dataview:
                    if (String.IsNullOrEmpty(dataviewName)){
                        var msg = "Index " + indexName + " is defined as using a Dataview for its sql source, but no DataviewName is given.";
                        sb.AppendLine(msg);
                    } else {
                        if (String.IsNullOrEmpty(sql)) {
                            if (!isDatabaseDown) {
                                if (!String.IsNullOrEmpty(onlyThisIndex)) {
                                    // we don't verify the sql when loading the full list because it takes a LONG time
                                    var msg = "Index " + indexName + " is defined as using Dataview '" + dataviewName + "' for its source, but no sql for the current database engine (" + providerName + ") could be found.";
                                    sb.AppendLine(msg);
                                }
                            }
                        }
                    }
                    break;
                case SqlSource.Xml:
                    if (String.IsNullOrEmpty(sql)) {

                        // sql is not defined for current dataconnection.
                        var msg = "Index " + indexName + " has no sql defined for the current database engine (" + providerName + ")";
                        sb.AppendLine(msg);
                    }
                    break;
                case SqlSource.Unknown:
                    throw new InvalidOperationException(getDisplayMember("ValidateConfigurationByDataRow", "SqlSource = {0} not implemented in Index.ValidateConfigurationByDataRow", sqlSource.ToString()) );
            }

            var drResolvers = ds.Tables["resolver"].Select("index_name = '" + indexName + "'");
            foreach (DataRow drR in drResolvers) {
                if (!Resolver<TKey, TValue>.ValidateConfigurationByDataRow(drR, ds, providerName, onlyThisIndex, onlyThisResolver, ref isDatabaseDown)) {
                    sb.AppendLine(drR["invalid_reason"].ToString());
                }
            }

            dr["is_valid"] = sb.Length == 0;
            dr["invalid_reason"] = sb.ToString();

            return sb.Length == 0;
        }


        internal string ValidateConfiguration(bool logIt, ref bool isDatabaseDown) {

            var sb = new StringBuilder();

            // assume everything is ok, change if needed
            IsConfigurationValid = true;

            if (String.IsNullOrEmpty(GetSqlByEngine(Indexer.DataConnectionSpec.ProviderName, ref isDatabaseDown))) {

                if (SqlSource == SqlSource.Dataview && isDatabaseDown) {
                    // could not reach database to determine sql, assume valid and continue.
                } else {

                    // sql is not defined for current dataconnection.
                    var msg = getDisplayMember("ValidateConfiguration{nosql}", "Index {0} has no sql defined for the current database engine ({1})", Name, Indexer.DataConnectionSpec.ProviderName);
                    sb.AppendLine(msg);
                    if (logIt) {
                        showProgress(msg, EventLogEntryType.Error, false);
                    }
                    IsConfigurationValid = false;
                }
            }

            foreach (var key in Resolvers.Keys) {
                var resolver = Resolvers[key];
                if (!resolver.IsConfigurationValid) {
                    var msg = resolver.ValidateConfiguration(false, isDatabaseDown);
                    sb.AppendLine(msg);
                    IsConfigurationValid = false;
                }
            }

            return sb.ToString();
        }

        void idx_OnProgress(object sender, ProgressEventArgs pea) {
            showProgress(pea);
        }


        private Dictionary<string, string> _sqlStatements = new Dictionary<string, string>();
        private int appendExternalSqlStatement(string primarySqlFileName, string engineSqlFileName, string engineName) {
            var setAsSql = primarySqlFileName.ToLower().EndsWith(engineSqlFileName.ToLower());
            var path = Toolkit.ResolveFilePath(Path.GetDirectoryName(primarySqlFileName) + @"\" + engineSqlFileName, false);
            if (File.Exists(path)) {
                var nav = new XPathDocument(path).CreateNavigator();
                var ndSql = nav.SelectSingleNode("/queries/" + Name + "/sql");
                if (ndSql != null) {
                    var ret = (ndSql.Value + "").Trim();
                    appendSqlStatement(engineName, ret, setAsSql);
                    return 1;
                }
            }
            return 0;
        }

        private void appendSqlStatement(string engineName, string sql, bool setAsSql) {
            _sqlStatements[engineName] = sql;
            if (Indexer.DataConnectionSpec != null && String.Compare(engineName, Indexer.DataConnectionSpec.EngineName, true) == 0) {
                Sql = sql;
            }
        }

		public override string ToString() {
			return this.Name;
		}

		private string calcFileName(string extension) {
			return this.Name + extension;
		}

		private string calcFilePath(string extension, bool deleteFileIfExists, bool throwExceptionIfFileIsMissing, bool useRotationFolder) {
			string path = Toolkit.ResolveFilePath(FolderName + @"\" + (useRotationFolder ? @"rotation\" : "") + this.Name + extension, true);
			if (deleteFileIfExists) {
				if (File.Exists(path)) {
					File.Delete(path);
				}
			} else if (throwExceptionIfFileIsMissing) {
				if (!File.Exists(path)) {
					throw new InvalidOperationException(getDisplayMember("calcFilePath", "Error processing Index {0}: {1} file is missing.", Name , path));
				}
			}
			return path;
		}

		internal void showProgress(string message, EventLogEntryType logType, bool writeToEventLog) {
            showProgress(new ProgressEventArgs(message, logType, writeToEventLog));
		}

        private void showProgress(ProgressEventArgs pea) {

            // if it's an error, log it to the error log
            if (pea.LogType == EventLogEntryType.Error) {

                logError(pea.Message);

            }


            if (OnProgress != null) {
                OnProgress(this, pea);
            }
        }

		private void checkFolder() {
			string dir = Toolkit.ResolveDirectoryPath(FolderName, true);
			if (!Directory.Exists(dir)) {
				throw new InvalidOperationException(getDisplayMember("checkFolder", "Folder '{0}' that is supposed to hold files for index '{1}' does not exist", dir, this.Name));
			}
		}

		#region Methods for Creation

        private void copyFilesToRotationFolder() {
            string fromFolder = Toolkit.ResolveDirectoryPath(FolderName, false);
            string toFolder = Toolkit.ResolveDirectoryPath(FolderName + "./rotation/", false);

            // copy existing files to rotation folder
            foreach (string name in Directory.GetFiles(fromFolder, this.Name + ".*", SearchOption.TopDirectoryOnly)) {
                FileInfo fi = new FileInfo(name);
                // stats file is never copied...
                if (!fi.Name.ToLower().EndsWith(".stats")) {
                    File.Copy(name, Toolkit.ResolveFilePath(toFolder + "./" + fi.Name, false));
                }
            }

        }

		public void Rotate(bool recreateBeforeRotate) {

			showProgress(getDisplayMember("Rotate{start}", "Rotating index {0}...", Name), EventLogEntryType.Information, false);

			// create indexes if needed (always to the rotation folder)
			if (recreateBeforeRotate) {
				Rebuild(true);
			}

            string rotationFolder = FolderName + "./rotation/";

            if (_keywordTree != null) {
                // wait for existing requests to finish, then call our rotation code
                // _keywordTree.GetExclusiveLock(processRotate, new object[] { rotationFolder, FolderName });
                _keywordTree.Lock(delegate() {
                    processRotate(null, new object[] { rotationFolder, FolderName });
                });
            } else {
                // either the index is disabled or brand new.  just rotate the files in (no need to get lock from tree)
                rotate(rotationFolder, FolderName );
            }

			// restart the index
            var dbDown = false;
            this.Load(ref dbDown);

			showProgress(getDisplayMember("Rotate{success}", "Successfully rotated index {0}.", Name), EventLogEntryType.Information, false);
		}

		private void rotate(string fromFolder, string toFolder) {
			// trickle-down delete all files used by this index (Dispose is called as needed)
            DeleteFiles();

			fromFolder = Toolkit.ResolveDirectoryPath(fromFolder, false);
			toFolder = Toolkit.ResolveDirectoryPath(toFolder, false);

			// move new files from rotation folder to main folder
			foreach (string name in Directory.GetFiles(fromFolder, this.Name + ".*", SearchOption.TopDirectoryOnly)) {
				FileInfo fi = new FileInfo(name);
                var tgt = Toolkit.ResolveFilePath(toFolder + "./" + fi.Name, true);
                if (File.Exists(tgt)){
                    File.Delete(tgt);
                }
                File.Move(name, tgt);
			}
		}


		private BPlusTreeNode<TKey, TValue> processRotate(BinaryWriter unusedWriter, object[] arguments) {

			string fromFolder = (string)arguments[0];
			string toFolder = (string)arguments[1];

			this.State = IndexState.Closed;
			this.Enabled = false;

			rotate(fromFolder, toFolder);

			return null;

		}
		
		/// <summary>
		/// Scans the database and (re)builds the index files and stores them in the folder specified in the Index.FolderName property
		/// </summary>
		public void Rebuild(bool inRotationFolder) {

            string before = FolderName;

            try {

                showProgress(getDisplayMember("Rebuild{start}", "Creating index {0}. SQL={1} ...", Name ,this.Sql), EventLogEntryType.Information, false);

                if (inRotationFolder) {
                    FolderName += "./rotation/";
                }

                // make sure the folder exists
                checkFolder();

                // close the data/index file references, if any.
                Dispose();

                // delete all associated files
                DeleteFiles();

                // flag the stats file to show that we're currently rebuilding it
                saveStat("LastRebuildBegin", DateTime.UtcNow);
                saveStat("LastRebuildEnd", DateTime.MinValue);


                // recreate the index
                createFiles(false);

                // log when we created it
                saveStat("LastRebuildEnd", DateTime.UtcNow);
                saveStat("ErrorsSinceLastRebuild", 0);
                saveStat("ErrorOnRebuild", "");

                showProgress(getDisplayMember("Rebuild{success}", "Successfully created index {0}.", Name), EventLogEntryType.Information, false);
            } catch (Exception ex){
                showProgress(getDisplayMember("Rebuild{failed}", "Failed creating index {0}: {1}.  {2}", this.Name, ex.Message, ex.ToString(true)), EventLogEntryType.Error, true);
                saveStat("LastRebuildEnd", DateTime.UtcNow);
                saveStat("ErrorOnRebuild", ex.ToString(true));
                saveStat("ErrorsSinceLastRebuild", -1);
                throw;
            } finally {
                if (inRotationFolder) {
                    FolderName = before;
                }
            }
		}

		private bool createFiles(bool stubsOnly) {


			// NOTE: we use binary files simply because they save so much space for numeric data (which is mostly what we're storing)
			// Here's the overall conceptual flow:
			//
			// 1. Copy data as needed from the database
			//    For each row in the database
			//       For each field in the row
			//          For each 'word' in the field
			//             Lookup its HitID. If it doesn't have one yet, create one.
			//             Save 'word'->HitID to the tree (for quick lookup on subsequent passes) -- .index
			//             Save HitID + 'word' to a flat file (for quick reverse lookup after we've sorted results) -- .hitmap
			//             Save all hit data (HitID, PrimaryKeyID, field offset, word offset, indexed fields, etc) -- .tmp*
			// 2. Sort hit data
			//    Sort each .tmp* file into a .sort* file by HitID, PrimaryKeyID
			// 3. Merge sorted data
			//    This consumes .sort* files and generates a single .data file
			// 4. Update tree
			//    The tree's HitID values are replaced with the corresponding location (file offset) in the .data file
			//    (We could not do this initially because the data was not sorted by HitID and PrimaryKeyID and we therefore didn't know the true file offset)
			// 5. Resolve IDs
			//    Since search results should return ID's that are not necessarily the PrimaryKeyID's for the table in which
			//    the keyword is found, we offer the ability to resolve a PrimaryKeyID from one table to one or more PrimaryKeyIDs 
			//    of another table (think of 'joe' being found in an address table, and you want information about the company joe works for)
			//    This step is entirely optional and is based on the config file. Simply do not add Resolver nodes to the config file to skip this step.
			//    Or, on the Resolve node, set attribute "CacheEnabled" to "False".  This will cause the resolver to look up 
			//    the ID's to resolve in real-time from the database using the Sql specified in the Resolve node.









			// Below is the pseudocode initially used to implement the creation of this index.  IT IS MERELY A GUIDELINE.
			// This consists of 5 parts:
			// 1. Initialization
			//		Create an empty dictionary S
			//		Create an empty temp file on disk
			// 2. Process Text and write Temp File
			//		For each record R:
			//			For each field F in R:
			//				a. Read F, parsing it into index terms T
			//				For each T:
			//					1. Search S for T
			//					2. If T is not in S, insert it
			//					3. Write (t,r,f) where t = termID, r = primary key id of record R, f = field offset in of field F in R
			// 3. Internal sorting to make runs
			//		Determine number of records K that can be held in memory
			//		Read K records from temp file
			//		Sort into nondecreasing t order, and for equal values of t, nondecreasing r order
			//		Write sorted run back to the temporary file
			//		Repeat until there are no more runs to be sorted
			// 4. Merging
			//		Pairwise merge runs in the temp file until it is one sorted run
			// 5. Output inverted file
			//		For each term t:
			//			Start a new inverted file entry
			//			Read all triples (t,r,f) from temp file and compose the inverted file entry for term t
			//			Append inverted file entry to the inverted file


			int fieldCount = 0;

			string definitionFileName = calcFilePath(".xml", true, false, false);

            string queueFileName = calcFilePath(".queue", true, false, false);

            string targetFileName = calcFilePath(".data", true, false, false);

            string treeFileName = calcFilePath(".index", true, false, false);

			// delete any files leftover from previous (possibly failed) creations
			string[] tempFiles = Directory.GetFiles(Toolkit.ResolveDirectoryPath(FolderName, false), calcFileName(".data.temp_") + "*");
			foreach (string tf in tempFiles) {
				File.Delete(tf);
			}
			string[] sortFiles = Directory.GetFiles(Toolkit.ResolveDirectoryPath(FolderName, false), calcFileName(".data.sort_") + "*");
			foreach (string sf in sortFiles) {
				File.Delete(sf);
			}


			using (DataManager dm = DataManager.Create(this.Indexer.DataConnectionSpec)) {

                var alteredSql = Sql;
                if (stubsOnly) {
                    // we don't want to pull the entire data content -- just run sql to get the proper database offsets and create stub files
                    //  (note this works even if using the 'old style' /*_ _*/ replacement approach)
                    alteredSql = (string.Empty + alteredSql).Replace("/*_", "").Replace("_*/", "").Replace(":idlist", "0").Replace(":readall", "0");
                } else {
                    // we DO want to pull the entire data content, make :readall = 1 (note this works even if using the 'old style' /*_ _*/ replacement approach)
                    alteredSql = (string.Empty + alteredSql).Replace(":idlist", "0").Replace(":readall", "1");

                }
                try {
                    using (IDataReader idr = dm.Stream(alteredSql)) {

                        // 1. Initialization
                        fieldCount = idr.FieldCount;
                        DatabaseFields.Clear();
                        SearchableFields.Clear();
                        IndexedFields.Clear();

                        //_createdDateFieldOrdinal = -1;
                        //_maxCreatedDate = DateTime.MinValue;

                        //_modifiedDateFieldOrdinal = -1;
                        //_maxModifiedDate = DateTime.MinValue;

                        for (int i = 0; i < fieldCount; i++) {
                            Field dbField = new Field { Name = idr.GetName(i), DataType = idr.GetFieldType(i), Ordinal = i };
                            DatabaseFields.Add(dbField);

                            //if (dbField.Name.ToLower() == "created_date") {
                            //    _createdDateFieldOrdinal = i;
                            //} else if (dbField.Name.ToLower() == "modified_date") {
                            //    _modifiedDateFieldOrdinal = i;
                            //}

                            if (dbField.Name.ToLower().Trim() == PrimaryKeyField.ToLower().Trim()) {
                                dbField.IsPrimaryKey = true;
                                _pkField = dbField;
                            }

                            // default string fields to searchable if the config says to
                            if (this.AutoIndexStringFields) {
                                if (dbField.DataType == typeof(string)) {
                                    dbField.Searchable = true;
                                }
                            }

                            Field configuredField = null;
                            foreach (Field cf in ConfiguredFields) {
                                if (cf.Name.ToLower() == dbField.Name.ToLower()) {
                                    configuredField = cf.Clone();
                                    break;
                                }
                            }

                            if (configuredField == null) {
                                // not configured. use defaults.
                                if (dbField.DataType == typeof(DateTime)) {
                                    // default to always storing just the date portion in ASCII-friendly sort order
                                    dbField.Format = "yyyy-MM-dd";
                                }
                            } else {
                                // configured.
                                dbField.Searchable = configuredField.Searchable;
                                dbField.StoredInIndex = configuredField.StoredInIndex;
                                dbField.Format = configuredField.Format;
                                if (dbField.DataType == typeof(DateTime) && String.IsNullOrEmpty(dbField.Format)) {
                                    // no format specified for a date field, assume ASCII-friendly sort order
                                    dbField.Format = "yyyy-MM-dd";
                                }

                                // dbField always wins out on the ordinal, since the configuredField should have no knowledge of the field ordinal within the database (matching is always done based on field name)
                                configuredField.Ordinal = dbField.Ordinal;
                            }


                            if (dbField.Searchable) {
                                SearchableFields.Add(dbField);
                            }

                            if (dbField.StoredInIndex) {
                                IndexedFields.Add(dbField);
                            }

                        }

                        // save definition to file (for mapping info when index is loaded for reading -- not used during creation)
                        writeDefinitionFile(definitionFileName);

                        // pull data from database and put data into files as needed
                        // This method produces the following files:
                        //   * A hit->keyword mapping file (.hitmap)      -- contains reverse information as our keyword tree (needed later when updating the tree)
                        //   * A keyword->data file location (.queue)     -- contains data that will be bulk inserted into our tree
                        //   * A data file (.data)                        -- file that contains hitid/pkid/fieldindex/wordindex/hitfield(s) info.  Main data file for the index.
                        //   * A log file (.log)                          -- file that contains the largest created / modified date from the data source for the latest run
                        processDatabaseInfo(idr, queueFileName, targetFileName, IndexedFields.Count);

                    }
                } catch (Exception ex) {
                    Logger.LogText("Error creating files for index " + this.Name + ": " + ex.Message);
                    if (!stubsOnly) {
                        throw;
                    }
                }
			}


			// by this point, we have a data file that is sorted by keyword, primary key.
			// we want to create our tree so each keyword points at the proper offset in that data file
			// This method produces the following files:
			//   * A b+ tree (.index)                                 -- the file that contains all keywords with pointers to the .data file.  Main index file for the index.
			var created = createTree(queueFileName, treeFileName);

			// now, using our list of primary keys, we generate all resolved primary key id's.
			// note each resolver results in two files:
			//   * A data file containing all the resolved primary key id's (.data)
			//   * A b+ tree containing the pk id's as keywords which point to the corresponding offsets in that data file (.index)

            // 2009-08-07 Brock Weaver brock@circaware.com
            // optimization:
            // also create a resolver file with keyword as the key, not pk id's.
            // we lose the ability to restrict by field name and that ability to do quoted searches, but
            // search performance is 2-3 orders of magnitude greater due to the minimization of processing required
            // at run time (trading disk space for access time)
            // i.e. create the following to files:
            //   * A data file containing all the resolved primary key id's (.data)
            //   * A b+ tree containing the same set of keywords as the 'main' index file which point to the corresponding offsets in that data file (.index)
            
            generateResolverFiles(stubsOnly);

            return created;


		}

		private void processDatabaseInfo(IDataReader idr, string queueFileName, string targetFileName, int fieldCount) {
			// This method produces the following files:
			//   * A sorted file 
			//   * A keyword->hit mapping file (.keywordmap)  -- the keyword hash stored as a file
			//   * A hit->keyword mapping file (.hitmap)      -- contains reverse information as our keyword tree (needed later when updating the tree)



			// this external sorter takes care of several things:
			//  Pulling data from database
			//  Sorting that data using an external (file-based) sort
			//  outputting the final results as a single, sorted file

			DataFileProcessor<Hit> dataFileProcessor = new DataFileProcessor<Hit>();
			SortedDictionary<TKey, long> keywordHash = new SortedDictionary<TKey, long>();

            string keywordListFileName = calcFilePath(".keywords", true, false, false);
            string hitMapFileName = calcFilePath(".hitmap", true, false, false);
//            string logFileName = calcFilePath(".log", false, false);


			// open a hitmap file and a reader/writer for it
			using(FileStream fsHitmap = new FileStream(hitMapFileName, FileMode.Create, FileAccess.ReadWrite)){
				using (BinaryWriter bwHitMap = new BinaryWriter(fsHitmap)) {
					using (BinaryReader brHitMap = new BinaryReader(fsHitmap)) {

						// open a keyword file and a reader/writer for it
						using (FileStream fsKeywords = new FileStream(keywordListFileName, FileMode.Create, FileAccess.ReadWrite)) {
							using (BinaryWriter bwKeywords = new BinaryWriter(fsKeywords)) {
								using (BinaryReader brKeywords = new BinaryReader(fsKeywords)) {


                                    //// create the log file if needed
                                    //if (!File.Exists(logFileName)) {
                                    //    File.WriteAllText(logFileName, "logged\tcreated\tmodified\r\n");
                                    //}


									// tell the processor to do its thing, calling back to us as needed
									dataFileProcessor.Sort(idr, this.MaxSortSizeInMB, targetFileName, processDatabaseInfoRowCallback, false, processDatabaseInfoGroupSortedCallback,
										new MainDataProcessingArguments {
											HitID = -1,
											HitMapWriter = bwHitMap,
											HitMapReader = brHitMap,
											KeywordWriter = bwKeywords,
											KeywordReader = brKeywords,
											KeywordHash = keywordHash
									});

                                    //// log the max created / updated dates we found (so it can be used later if we need to do a partial index update)
                                    //File.AppendAllText(logFileName, DateTime.Now.ToString() + "\t" + _maxCreatedDate.ToString() + "\t" + _maxModifiedDate.ToString() + "\r\n");

								}
							}
						}
					}
				}
			}

			// and finally open a write-only file that will be used for bulk inserting the keywordhash contents into the tree
			using (BinaryWriter bwQueueFile = new BinaryWriter(new FileStream(queueFileName, FileMode.Create, FileAccess.Write))) {

				switch (DataLocation) {
					case DataLocation.InLeaf:

						using (BinaryReader brData = new BinaryReader(new FileStream(targetFileName, FileMode.Open, FileAccess.Read))) {
							// read from the data file and copy into the leaf itself
							foreach (TKey key in keywordHash.Keys) {
								key.Write(bwQueueFile);

								brData.BaseStream.Position = keywordHash[key];

								int count = brData.ReadInt32();
								for (int i=0; i < count; i++) {
									TValue val = new TValue();
									val.Read(brData);
									val.Write(bwQueueFile);
								}

							}
						}

						// all the data from the data file is now in the queue file.
						// get rid of the data file.
						File.Delete(targetFileName);

						break;

					case DataLocation.InSeparateFile:

						// the .data file will keep all the data -- meaning we just need to 
						// store List<long> as the value in the tree (so if data is added to 
						// the .data file later, we can add a pointer in the tree to that new 
						// data without losing the pointer to the preexisting data already in the .data file)

						foreach (TKey key in keywordHash.Keys) {
							// write the key
							key.Write(bwQueueFile);

							// then grab the value and write it out
							long val = keywordHash[key];
							BPlusListLong bpll = new BPlusListLong();
							bpll.Value.Add(val);
							bpll.Write(bwQueueFile);
						}
						break;
					default:
						throw new NotImplementedException(getDisplayMember("processDatabaseInfo", "processDatabaseInfo is missing a case in switch(DataLocation)"));
				}
			}
				

			// we get here, queue file contains everything we need. get rid of the hitmap/keyword files as they're no longer needed
			File.Delete(hitMapFileName);
			File.Delete(keywordListFileName);

		}


		private List<Hit> processDatabaseInfoRowCallback(IDataReader idr, object additionalData) {


			MainDataProcessingArguments args = (MainDataProcessingArguments)additionalData;


			// NOTE: this method is called once per row in the database.
			// Since we track more than just the hits, we have a lot of extra work to do

			List<Hit> hits = new List<Hit>();

			// first field is always an integer and is the primary key -- if not, they haven't defined their Sql node
			// properly in the Index config file.
            if (_pkField == null) {
                throw new InvalidOperationException(getDisplayMember("processDatabaseInfoRowCallback{nopkfield}", "The '{0}' index defines field '{1}' as its Primary Key, but that field name was not found in the results from running the SQL statement configured for that index.  The SQL is : {2}", Name, this.PrimaryKeyField, this.Sql));
            }
			int pkID = Toolkit.ToInt32(idr.GetValue(_pkField.Ordinal), -1);
			if (pkID == -1) {
				throw new InvalidOperationException(getDisplayMember("processDatabaseInfoRowCallback{nopkid}", "Invalid sql defined for index {0}.  First column must always be the primary key for the table, and the primary key must be a 4-byte signed integer.", this.Name));
			}

			Hit hitTemplate = new Hit(0, 0, 0, 0, IndexedFields);

			// first, get all values of IndexedFields so we can store them with each hit
			for (int i=0; i < IndexedFields.Count; i++) {
				// NOTE: assumes value must be an int, as this is all we can store in HitField.
                var idxField = IndexedFields[i];
                if (idxField.IsBoolean) {
                    hitTemplate.Fields[i].Value = idr.GetValue(idxField.Ordinal).ToString() == idxField.TrueValue.ToString() ? 1 : 0;
                } else {
                    hitTemplate.Fields[i].Value = Toolkit.ToInt32(idr.GetValue(idxField.Ordinal), 0);
                }
			}

			int searchableFieldCount = SearchableFields.Count;
			for (int i=0; i < searchableFieldCount; i++) {

				// note we only inspect the searchable fields (all string fields are searchable by default)
				int fieldOrdinal = SearchableFields[i].Ordinal;
				object value = idr.GetValue(fieldOrdinal);
				string[] keywords = Tokenize(value, SearchableFields[i]);

				for (int keywordIndex=0; keywordIndex < keywords.Length; keywordIndex++) {

					string text = keywords[keywordIndex];

					// if an item returned tokenize contained only html tags, it will be empty if StripHtml is enabled.
					// so this is an extra check to make sure we don't put in null/zero-length keywords
					if (!String.IsNullOrEmpty(text)) {

						// also, make sure it's not in the list of words to exclude
						if (!StopWords.Contains(text)) {

							long foundHitID = -1;

							TKey key = new TKey().Parse(text);

							// remember the keyword to the hit id
							if (!args.KeywordHash.TryGetValue(key, out foundHitID)) {

								// this is a new keyword.  add it to the hash, map it to hit id, write it to file for bulk insert later
								args.HitID++;

								// map keyword to hit id
								args.KeywordHash[key] = args.HitID;

								// to do efficient reverse lookups (given hitid, find keyword), we're going to write the keywords to one file,
								// and the file location of each keyword to another file (called the hitmap file).
								// since hitid's are ordered, we can assume hitid 0 -> first keyword, hitid 1 -> second keyword, etc.
								// since we store the file location of the keyword (instead of the keyword itself) in the map file (aka fixed length records),
								// we can do simple math to determine the file location of the keyword to read later on after we've sorted teh data file and
								// we're trying to resolve hitid back to keyword.
								args.HitMapWriter.Write(args.KeywordWriter.BaseStream.Position);
								args.KeywordWriter.Write(text);

								foundHitID = args.HitID;
							}

							Hit newHit = hitTemplate.CloneOnlyFields((int)foundHitID, pkID, fieldOrdinal, keywordIndex);
							// NOTE: All the h.Fields were initialized before we got in this tighter loop, so we just copy them here
							// (considerably quicker to copy from Hit object than from idr.GetValue())
							hits.Add(newHit);
						}
					}
				}
			}

			return hits;
		}

		private void processDatabaseInfoGroupSortedCallback(Hit lastItemInGroup, long dataFileLocation, int groupCount, object additionalData) {
			MainDataProcessingArguments mdpa = (MainDataProcessingArguments)additionalData;

			// ok, all the keywords (and their hit ids) are still in RAM.
			// this method is called when we find a new hit id.
			// what we're trying to get to is a file that contains the Keyword followed by the data file location.
			// so we need to use the keyword hash to find which keyword maps to which hit id.
			// since it's stored in the opposite fashion (keyword->hitid, not hitid->keyword),
			// 

			// calculate what the file location for the keyword is
			mdpa.HitMapReader.BaseStream.Position = lastItemInGroup.ID * sizeof(long);
			long keywordFileLocation = mdpa.HitMapReader.ReadInt64();

			// read the keyword from the file
			mdpa.KeywordReader.BaseStream.Position = keywordFileLocation;
			string keyword = mdpa.KeywordReader.ReadString();



			// ok, we have the keyword (from the hitmap/keyword files) and the actual dataFileLocation (passed into this method)
			// update our keyword hash with that value (don't write it out yet -- we want to order by keyword, not hit id)
			mdpa.KeywordHash[new TKey().Parse(keyword)] = dataFileLocation;


		}


		private bool createTree(string queueFileName, string treeFileName) {


			// queue file contains all the data we need to bulk insert into our tree...
            if (File.Exists(queueFileName)) {
                using (Stream treeStream = new FileStream(treeFileName, FileMode.Create, FileAccess.ReadWrite)) {
                    using (var tree = BPlusTree<BPlusString, BPlusListLong>.Create(treeStream, this.Encoding, this.FanoutSize, this.AverageKeywordSize, this.NodeCacheSize, this.KeywordCacheSize)) {
                        using (Stream queueStream = new FileStream(queueFileName, FileMode.Open, FileAccess.Read)) {
                            tree.BulkInsert(queueStream, 1.0M);
                        }
                    }
                }
            }

            // the queue file is no longer needed (its data is now in our trees)
            File.Delete(queueFileName);

            return File.Exists(treeFileName);

		}

		private void generateResolverFiles(bool stubsOnly) {

			// resolvers are always stored with their key as an int and their data as a list of longs
			List<Resolver<TKey, TValue>> resolversToProcess = new List<Resolver<TKey, TValue>>();

            var dbDown = false;
            foreach (string name in Resolvers.Keys) {
				Resolver<TKey, TValue> r = Resolvers[name];
				if (r.Enabled) {
                    r.KeywordCacheSize = this.KeywordCacheSize;
                    r.NodeCacheSize = this.NodeCacheSize;

                    if (!Loaded && r.CacheMode == ResolverCacheMode.IdAndKeyword) {
                        // we must load the index to enable looking up by keyword
                        // but skip loading the resolvers since we're in the process of creating them
                        load(false, ref dbDown);
                    }

                    r.Create(stubsOnly);
				}
			}

            // unload if we loaded it to fill the resolver trees
            if (Loaded) {
                Unload();
            }
		}


		private void writeDefinitionFile(string fileName) {

			Document doc = new Document("Index");
			doc.Root.Attributes.SetValue("Name", this.Name);
			Node fieldNodes = new Node("Fields");
			fieldNodes.Attributes.SetValue("Count", DatabaseFields.Count.ToString());
			doc.Root.Nodes.Add(fieldNodes);

			for (int i=0; i < DatabaseFields.Count; i++) {
				Node nd = DatabaseFields[i].ToXmlNode();
				fieldNodes.Nodes.Add(nd);
			}

			Node resolverNodes = new Node("Resolvers");
			resolverNodes.Attributes.SetValue("Count", Resolvers.Count.ToString());
			foreach (string name in Resolvers.Keys) {
                var nd = Resolvers[name].ToXmlNode();
				resolverNodes.Nodes.Add(nd);
			}

			string xmlFilePath = fileName;
			doc.Save(xmlFilePath);


		}

		internal string RemoveHtml(string text) {
			
			string output = __htmlTagRemover.Replace(text, "");

			output = HttpUtility.HtmlDecode(output);

			return output;

			//MatchCollection mc = __htmlTagRemover.Matches(text);
			//if (mc.Count > 0) {
			//    foreach (Match m in mc) {
			//        output[i] = stripHtmlTags(m.Groups["content"].Value);
			//    }
			//}
		}

//		private static Regex __htmlTagRemover = new Regex(@"(<(?<tagname>[A-Z][A-Z0-9]*)\b[^>]*>(?<content>.*?)</\k<tagname>>|([A-Z][A-Z0-9]*)\b[^>]*/>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
//		private static Regex __htmlTagRemover = new Regex(@"<(?<tagname>[A-Z][A-Z0-9]*)\b[^>]*>(?<content>.*?)</\k<tagname>>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

		public string[] Tokenize(object value, Field f) {

			if (value == null || value == DBNull.Value) {
				return EMPTY_STRING_ARRAY;
			}

			if (f.DataType == typeof(string)) {
				if (String.IsNullOrEmpty((string)value)) {
					return EMPTY_STRING_ARRAY;
				} else {

					string text = (string)value;
					if (StripHtml) {
						// scrub out html tags and decode html entities
						text = RemoveHtml(text);
					}

                    // break on whitespace, separators, punctuation as defined by the config file
                    var temp = ParseKeywords(text);

                    return temp;
				}
			} else if (f.DataType == typeof(DateTime)) {
				return new string[1] { Toolkit.ToDateTime(value).ToString(f.Format) };
			} else if (f.DataType == typeof(decimal)) {
				return new string[1] { Toolkit.ToDecimal(value, 0.0M).ToString(f.Format) };
			} else if (f.DataType == typeof(int)) {
				return new string[1] { Toolkit.ToInt32(value, 0).ToString(f.Format) };
			} else if (f.DataType == typeof(float)) {
				return new string[1] { Toolkit.ToFloat(value, 0.0f).ToString(f.Format) };
			} else if (f.DataType == typeof(double)) {
				return new string[1] { Toolkit.ToDouble(value, 0.0d).ToString(f.Format) };
			} else {
				throw new NotImplementedException(getDisplayMember("Tokenize", "Type '{0}' not mapped in Index.tokenize()", f.DataType.ToString()));
			}
		}


        internal string[] ParseKeywords(string phrase) {

            // split on whitespace and separators
            var temp = phrase.Split(this.Whitespace.Concat(this.Separators).ToArray(), true);

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


		#endregion Methods for Creation


		#region Methods for Reading

		public void Unload() {
			showProgress(getDisplayMember("Unload{start}", "Unloading index {0}...", Name), EventLogEntryType.Information, false);
			Dispose();
			showProgress(getDisplayMember("Unload{done}", "Successfully unloaded index {0}...", Name), EventLogEntryType.Information, false);
		}

		public void Reload() {
			Unload();
            var dbDown = false;
			Load(ref dbDown);
		}

        private void loadTreeAndDataFiles() {
            checkFolder();

            // load definition from xml file
            string definitionFileName = calcFilePath(".xml", false, true, false);
            readDefinitionFile(definitionFileName);


            // init the keyword b+ tree for searching
            string keywordTreePath = calcFilePath(".index", false, false, false);
            if (!File.Exists(keywordTreePath)) {

                // index file doesn't exist. remove all associated files (in case there's some stragglers), then recreate stub files for everything.
                // stub files essentially create the valid xml and index files, but does not load index or data files with any data.
                DeleteFiles();
                createFiles(true);


                //// assume none of the relevant files exist.  create blank ones.
                //using (var temp = BPlusTree<TKey, TValue>.Create(keywordTreePath, this.Encoding, this.FanoutSize, this.AverageKeywordSize, this.NodeCacheSize, this.KeywordCacheSize)) {
                //    // nothing to do, just making sure Dispose is called.
                //}

                //createEmptyDataFileIfNeeded();

            }

            _keywordTree = BPlusTree<TKey, TValue>.Load(keywordTreePath, false, this.NodeCacheSize, this.KeywordCacheSize);

            // open data file for quick access during searches
            openDataFile();

        }

		/// <summary>
		/// Loads the necessary index files from Index.FolderName and prepares the index for querying.
		/// </summary>
		public void Load(ref bool isDatabaseDown) {
            load(true, ref isDatabaseDown);
        }

        private void load(bool includeResolvers, ref bool isDatabaseDown){

			try {

				//showProgress("Loading index " + Name + "...", EventLogEntryType.Information, false);
                var reason = this.ValidateConfiguration(false, ref isDatabaseDown);
                if (!String.IsNullOrEmpty(reason)){
                    throw new InvalidOperationException(reason);
                }

				this.State = IndexState.Opening;

                loadTreeAndDataFiles();

                if (includeResolvers) {
                    // init all the resolver b+ trees and data files for searching
                    // we should only skip these when creating the resolvers themselves,
                    // which if one or more are resolving by keyword will need the index loaded
                    // before the resolver is created.
                    foreach (Resolver<TKey, TValue> r in Resolvers.Values) {
                        if (r.Enabled) {
//                            r.CreateEmptyFilesIfNeeded();
                            r.Open();
                        }
                    }
                }

				this.State = IndexState.Open;

                if (includeResolvers) {
                    var keywordCount = this._keywordTree.GetKeywordCount(true);
                    //var resolverCount = 0;
                    //foreach (Resolver<TKey, TValue> r in Resolvers.Values) {
                    //    resolverCount += r.GetApproximateIDCount();
                    //}
                    showProgress(getDisplayMember("load{done}", "{0} index loaded.  Contains at most {1} keywords.", Name ,keywordCount.ToString("###,###,###,##0")), EventLogEntryType.Information, false);
                    //showProgress("Successfully loaded index " + Name + ".", EventLogEntryType.Information, false);

                    LastLoaded = DateTime.UtcNow;

                    // mark us as having no errors since we just loaded it
                    Interlocked.Exchange(ref _errorsSinceLoad, 0);


                }

			} catch {
				// something bombed.  clean up, then bubble the exception
				Dispose();
				throw;
			}

		}

        //private void createEmptyDataFileIfNeeded() {
        //    string dataFilePath = calcFilePath(".data", false, false);
        //    if (!File.Exists(dataFilePath)) {
        //        File.Create(dataFilePath).Close();
        //    }

        //    var xmlFilePath = calcFilePath(".xml", false, false);
        //    if (!File.Exists(xmlFilePath)) {
        //        writeDefinitionFile(xmlFilePath);
        //    }


        //}

        private void openDataFile() {
            string dataFilePath = calcFilePath(".data", false, true, false);
            FileStream fs = new FileStream(dataFilePath, FileMode.Open, FileAccess.ReadWrite);
            _dataReader = new BinaryReader(fs);
            _dataWriter = new BinaryWriter(fs);
        }

        private void closeDataFile() {
            if (_dataReader != null) {
                _dataReader.Close();
                _dataReader = null;
            }
            if (_dataWriter != null) {
                _dataWriter.Close();
                _dataWriter = null;
            }
        }

        public List<int> SearchAndResolve(string keyword, KeywordMatchMode matchMode, bool ignoreCase, string resolverName, SearchOptions opts) {
            var resolver = this.Resolvers[resolverName.ToLower()];
            var ret = resolver.GetResolvedIDsViaIdAndKeyword(keyword, matchMode, ignoreCase, opts);
            return ret;
        }

        public List<KeywordMatch<TKey, TValue>> Search(string keyword, KeywordMatchMode matchMode, bool ignoreCase) {
            if (_keywordTree == null) {
                return null;
            } else {
                return _keywordTree.Search(keyword, matchMode, ignoreCase);
            }
        }
        public List<KeywordMatch<TKey, TValue>> Search(TKey keyword, KeywordMatchMode matchMode, bool ignoreCase) {
            if (_keywordTree == null) {
                return null;
            } else {
                return _keywordTree.Search(keyword, matchMode, ignoreCase);
            }
        }

        public KeywordMatch<TKey, TValue> InsertIntoTree(string keyword, TValue newValue) {
            return _keywordTree.Insert(keyword, newValue);
        }
        public KeywordMatch<TKey, TValue> InsertIntoTree(TKey keyword, TValue newValue) {
            return _keywordTree.Insert(keyword, newValue);
        }

        public void UpdateTree(KeywordMatch<TKey, TValue> match, TValue newValue, UpdateMode updateMode){
            match.Value.Update(newValue, updateMode);
            match.Node.Write();
        }

        public List<KeywordMatch<TKey, TValue>> UpdateTree(string keyword, KeywordMatchMode matchMode, TValue newValue, UpdateMode updateMode) {
            return _keywordTree.Update(keyword, matchMode, newValue, updateMode);
        }
        public List<KeywordMatch<TKey, TValue>> UpdateTree(TKey keyword, KeywordMatchMode matchMode, TValue newValue, UpdateMode updateMode) {
            return _keywordTree.Update(keyword, matchMode, newValue, updateMode);
        }


        /// <summary>
        /// Called once for each leaf in the tree during a Defrag() call.
        /// </summary>
        /// <param name="leaf">Current node to process.  If this is to be written back to the tree (i.e. it was changed during this callback), return true to tell the index to update the tree.  Otherwise return false.</param>
        /// <param name="existingData">A BinaryReader pointing at the existing data file for reading fragmented data</param>
        /// <param name="defraggedData">A BinaryWriter pointing at the output data file for writing defragmented data</param>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        public delegate bool ProcessDefrag(BPlusTreeLeafNode<TKey, TValue> leaf, BinaryReader existingData, BinaryWriter defraggedData);

        public void Defrag(ProcessDefrag callback) {

            // defragging an index...
            // 1) the keyword tree itself never becomes fragmented, so there is no need to do any work on it -- other than updating data file pointers.
            // 2) the data file DOES become fragmented as realtime updates occur.  i.e. one hit in the tree
            //    may point to multiple positions in the data file.
            // 3) each hit in the tree needs to correspond to one location in the data file after defragging.
            // 4) all data associated with a given hit in the tree needs to be contained in a single spot in the data file after defragging.


            if (callback == null){
                return;
            }

            object[] args = new object[1] { callback };

            showProgress(getDisplayMember("Defrag{start}", "Defragging index {0}...", Name), EventLogEntryType.Information, false);

            if (_keywordTree != null) {
                // wait for existing requests to finish, then call our defrag code
                _keywordTree.Lock(delegate() {
                    copyFilesToRotationFolder();
                });

                // defrag
                processDefragCall(null, args);

                // rotate back in
                _keywordTree.Lock(delegate() {
                    rotate(FolderName + "./rotation/", FolderName);
                });

            } else {
                // either the index is disabled or brand new.  just defrag the files (no need to get a lock)
                if (_dataReader != null) {
                    processDefragCall(null, args);
                }
            }
            showProgress(getDisplayMember("Defrag{done}", "Defrag of index {0} complete.", Name), EventLogEntryType.Information, false);

        }

        private BPlusTreeNode<TKey, TValue> processDefragCall(BinaryWriter unusedWriter, object[] args) {

            ProcessDefrag callback = args[0] as ProcessDefrag;

            // create a new data file that we can dump contents of the current data file into
            _dataReader.BaseStream.Position = 0;
            string defragDataFilePath = calcFilePath(".data.defrag", true, false, true);
            using (BinaryWriter wtr = new BinaryWriter(new FileStream(defragDataFilePath, FileMode.OpenOrCreate, FileAccess.Write))){
                foreach (BPlusTreeLeafNode<TKey, TValue> leaf in _keywordTree.GetAllLeaves()) {
                    // give the caller the callback info so they can do  what they need to the leaf
                    if (callback(leaf, _dataReader, wtr)) {
                        leaf.Write();
                    }
                }
            }

            // by this point, the data file has been defragged into the defrag file.
            // we need to have our index start looking at the defragged file instead
            closeDataFile();
            string dataFilePath = calcFilePath(".data", true, false, false);  // true means delete the file if it exists, which it should exist and be deleted
            File.Move(defragDataFilePath, dataFilePath);

            string defragIndexFilePath = calcFilePath(".index", false, false, true);
            string indexFilePath = calcFilePath(".index", true, false, false);
            File.Move(defragIndexFilePath, indexFilePath);
            openDataFile();

            return null;

        }







		/// <summary>
		/// Traverses the tree and inspects each node to validate it is formed properly -- no blank keywords, proper child pointers, etc.  If checkLeaves is true, this will take a very long time to execute.  If it is false, will return considerably quicker, but may still take quite awhile to execute.  If there are any invalid nodes, it will throw an exception detailing information about each one.
		/// </summary>
		public void VerifyIntegrity(bool checkTreeStructure, bool checkLeaves) {
			StringBuilder sb = new StringBuilder();

			// showProgress("Verifying index " + Name + " ... ", EventLogEntryType.Information, false);

            // populate the _pkField if needed (or possible!)
            if (_pkField == null || _pkField.Ordinal < 0) {
                foreach (Field f in DatabaseFields) {
                    if (f.Name.ToLower() == this.PrimaryKeyField.ToLower()) {
                        _pkField = f;
                        break;
                    }
                }
            }

            if (_pkField == null || _pkField.Ordinal < 0) {
                sb.AppendLine(getDisplayMember("VerifyIntegrity{nopkfield}", "PrimaryKeyField is undefined or defined improperly.  Please check the value for /Indexes/Index[@Name={0}]/@PrimaryKeyField in the config file.", this.Name));
            }

			// first validate the configured sql for the index and all its resolvers are all correct.
            using (DataManager dm = DataManager.Create(this.Indexer.DataConnectionSpec)) {

				try {
					// run the index sql, but only grab 1 row at most (where should make sure it's 0 rows -- we're just making sure)
					dm.Limit = 1;
                    string sql = this.Sql.Replace("/*_", "").Replace("_*/", "").Replace(":idlist", "0").Replace(":readall", "0");
                    dm.Read(sql, "table1");
				} catch (Exception ex1) {
					sb.AppendLine(getDisplayMember("VerifyIntegrity{invalidsql}", "Invalid Sql for index '{0}': {1}.\r\nSql={2}", this.Name, ex1.Message, this.Sql));
				}

				foreach (string name in this.Resolvers.Keys) {
					Resolver<TKey, TValue> r = Resolvers[name];
					if (r != null){
                        switch(r.Method){
                            case ResolutionMethod.Sql:
						        try {
							        // run the resolver sql, but only grab 1 row at most (where should make sure it's 0 rows -- we're just making sure)
							        dm.Limit = 1;
                                    string sql = r.Sql.Replace("/*_", "").Replace("_*/", "").Replace(":idlist", "0").Replace(":readall", "0");
                                    //if (!sql.Contains(" where 1 = 0 ")) {
                                    //    sql += " where 1 = 0 ";
                                    //}
							        dm.Read(sql, "resTable");
						        } catch (Exception ex1) {
							        sb.AppendLine(getDisplayMember("VerifyIntegrity{invalidresolversql}", "Invalid Sql for resolver '{0}' in index '{1}': {2}.\r\nSql={3}", r.Name, this.Name, ex1.Message, r.Sql));
						        }
                                break;

                            case ResolutionMethod.ForeignKey:
                                if (String.IsNullOrEmpty(r.ForeignKeyField)) {
                                    sb.AppendLine(getDisplayMember("VerifyIntegrity{noresolverfk}", "Resolver {0} for Index {1} is defined as using Method='ForeignKey' but the /Indexes/Index[@Name={2}]/Resolvers/Resolver[@Name={3}]/@ForeignKey value is empty or missing.", r.Name, r.Index.Name, r.Index.Name, r.Name));
                                } else {
                                    // make sure the fk field exists in the index itself, because the resolver requires it to be there to function properly
                                    var offset = GetFieldIndexOrdinal(r.ForeignKeyField);
                                    if (offset < 0 || offset >= IndexedFields.Count) {
                                        sb.AppendLine(getDisplayMember("VerifyIntegrity{badfkordinal}", "Resolver {0} for Index {1} is defined as using Method='ForeignKey' and the foreign key field '{2}', but that field is not stored in the index.  Please set the config /Indexes/Index[@Name={3}]/Fields/Field[@Name={4}]/@StoredInIndex='True'", r.Name, r.Index.Name, r.Index.Name, r.Index.Name, r.ForeignKeyField));
                                    }
                                }
                                break;

                            default:
                                // nothing to check
                                break;
                        }
					}
				}
			}

			try {
//				if (this.Enabled) {
					if (this.State == IndexState.Open) {
						foreach (BPlusTreeNode<TKey, TValue> node in _keywordTree.GetAllMalformedNodes(checkLeaves)) {
							if (sb.Length == 0) {
								sb.AppendLine(getDisplayMember("VerifyIntegrity{badnode}", "\r\nThe tree contains the following malformed nodes: "));
							}
							sb.AppendLine(node.ToString());
						}
					}
//				}
			} catch (Exception ex2) {
				sb.AppendLine(getDisplayMember("VerifyIntegrity{fatal}", "Fatal error verifying integrity of index '{0}': {1}", this.Name, ex2.Message));
			}
			if (sb.Length > 0) {
				throw new InvalidOperationException(sb.ToString());
			}

			// showProgress("Successfully verified index " + Name + ".", EventLogEntryType.Information, false);

		}

        private void readDefinitionFile(string fileName) {

            //this.DatabaseFields.Clear();
            //this.IndexedFields.Clear();
            //this.SearchableFields.Clear();

            using (Document doc = new Document()) {
                doc.Load(fileName);
                foreach (Node nd in doc.Root.Nodes["Fields"].Nodes) {
                    Field f = Field.FromDefinitionFileNode(nd);
                    if (f.IsPrimaryKey) {
                        _pkField = f;
                    }
                    //DatabaseFields.Add(f);
                    //if (f.Searchable) {
                    //    SearchableFields.Add(f);
                    //}
                    //if (f.StoredInIndex) {
                    //    IndexedFields.Add(f);
                    //}
                }
            }
        }

        private Dictionary<string, Field> _dbFieldsByName;

        internal Field GetField(string fieldName) {
            if (!String.IsNullOrEmpty(fieldName)) {
                if (_dbFieldsByName == null) {
                    _dbFieldsByName = new Dictionary<string, Field>();
                    foreach (var f in DatabaseFields) {
                        _dbFieldsByName.Add(f.Name, f);
                    }
                }

                Field fld = null;
                if (_dbFieldsByName.TryGetValue(fieldName, out fld)) {
                    return fld;
                }

            }

            // we get here, no field by that name was found.
            return null;
        }

		/// <summary>
		/// Given a field name, returns its corresponding field ordinal in the database.  Since the ordinal is the only thing stored in the index, this is required to restrict hits within a specific field.
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		internal int GetFieldDatabaseOrdinal(string fieldName) {
            if (!String.IsNullOrEmpty(fieldName)) {

                if (_dbFieldsByName == null) {
                    _dbFieldsByName = new Dictionary<string, Field>();
                    foreach (var f in DatabaseFields) {
                        _dbFieldsByName.Add(f.Name, f);
                    }
                }

                Field fld = null;
                if (_dbFieldsByName.TryGetValue(fieldName, out fld)) {
                    return fld.Ordinal;
                } else {
                    // invalid field ordinal (i.e. they specified a field name which doesn't exist)
                    // return maxvalue so when doing searches we guarantee the Ordinal doesn't match (but doesn't say 'do not restrict by any field' like -1 does)
                    return int.MaxValue;
                }

                //foreach (Field f in DatabaseFields) {
                //    if (f.Name.ToLower() == fieldName.ToLower()) {
                //        return f.Ordinal;
                //    }
                //}
            } else {
                // tell them they gave us an empty field by returning -1.  useful when doing searches and hits should not be restricted to a particular field
                return -1;
            }
        }


        private Dictionary<int, Field> _dbFieldsByOrdinal;

        /// <summary>
        /// Given the database ordinal, returns the name of the field
        /// </summary>
        /// <param name="ordinal"></param>
        /// <returns></returns>
        internal string GetFieldName(int ordinal) {

            if (_dbFieldsByOrdinal == null){
                _dbFieldsByOrdinal = new Dictionary<int, Field>();
                foreach(var f in DatabaseFields){
                    _dbFieldsByOrdinal.Add(f.Ordinal, f);
                }
            }

            return _dbFieldsByOrdinal[ordinal].Name;

            //return DatabaseFields.Find(f => {
            //    return f.Ordinal == ordinal;
            //}).Name;
        }

        private Dictionary<string, int> _searchableFieldOffsetsByName;

        /// <summary>
        /// Given a field name, returns its corresponding field ordinal in the searchable fields collection.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        internal int GetFieldSearchableOffset(string fieldName) {
            if (!String.IsNullOrEmpty(fieldName)) {

                if (_searchableFieldOffsetsByName == null) {
                    _searchableFieldOffsetsByName = new Dictionary<string, int>();
                    for(var i=0;i<SearchableFields.Count;i++){
                        _searchableFieldOffsetsByName.Add(SearchableFields[i].Name, i);
                    }
                }

                return _searchableFieldOffsetsByName[fieldName];

                //for(int i=0;i<SearchableFields.Count;i++){
                //    Field f = SearchableFields[i];
                //    if (f.Name.ToLower() == fieldName.ToLower()) {
                //        return i;
                //    }
                //}
            }
            // -1 here so they know it's an invalid ordinal (ordinals must always be non-negative)
            return -1;
        }

        private Dictionary<string, int> _indexedFieldOffsetsByName;

		/// <summary>
		/// Given a field name, returns its corresponding field ordinal in the index.  This allows us to determine which Hit.Fields[] value to pull by the fieldName.
		/// </summary>
		/// <param name="fieldName"></param>
		/// <returns></returns>
		internal int GetFieldIndexOrdinal(string fieldName) {
			if (!String.IsNullOrEmpty(fieldName)) {

                if (_indexedFieldOffsetsByName == null) {
                    _indexedFieldOffsetsByName = new Dictionary<string, int>();
                    for (var i = 0; i < IndexedFields.Count; i++) {
                        _indexedFieldOffsetsByName.Add(IndexedFields[i].Name, i);
                    }
                }

                var rv = -1;
                if (_indexedFieldOffsetsByName.TryGetValue(fieldName, out rv)) {
                    return rv;
                }
                
                //for (int i = 0; i < IndexedFields.Count; i++) {
                //    if (IndexedFields[i].Name.ToLower() == fieldName.ToLower()) {
                //        return i;
                //    }
                //}
			}
			return -1;
		}


		/// <summary>
		/// Returns all keywords known by this index.  Result could be extremely large and take a long time to process.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TKey> GetAllKeywords() {
			if (_keywordTree != null) {
				foreach (TKey keyword in _keywordTree.GetAllKeywords()) {
					yield return keyword;
				}
			}
		}

        /// <summary>
        /// Returns all leaves in this index.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<BPlusTreeLeafNode<TKey, TValue>> GetAllLeaves() {
            if (_keywordTree != null) {
                foreach (var leaf in _keywordTree.GetAllLeaves()) {
                    yield return leaf;
                }
            }
        }

        internal IEnumerable<int> GetAllPrimaryKeyIDs(long dataLocation) {
            return Hit.ReadAllPrimaryKeyIDs(_dataReader, dataLocation);
        }

        internal IEnumerable<Hit> GetAllHits(long dataLocation) {
            return Hit.ReadAll(_dataReader, dataLocation, null, -1, this.TemporaryID);
        }


		/// <summary>
		/// Returns all hits for the given keyword.  Can optionally be an inexact match (startswith / endswith / contains) or be case insensitive.
		/// </summary>
		/// <param name="cmd">A command detailing the search parameters.</param>
		/// <returns></returns>
        public IEnumerable<Hit> GetHits(SearchCommand<TKey, TValue> cmd, DataSet ds, SearchOptions opts) {


            List<Hit> hits = new List<Hit>();

            cmd.ReadFromDatabase = false;
            
            // index isn't ready yet, just throw back empty set
			if (this.State != IndexState.Open) {
                return hits;
			}

            if (!String.IsNullOrEmpty(cmd.RestrictToIndex) && this.Name.ToLower() != cmd.RestrictToIndex.ToLower()) {
                // not in correct index according to field definition. just throw back empty set
                //cmd.HitList = hits;
                return hits;
            }

            var field = GetField(cmd.FieldName);
            var ordinal = field == null ? -1 : field.Ordinal;

            if (!String.IsNullOrEmpty(cmd.FieldName)) {

                if (cmd.PassthruLevel == DatabasePassthruLevel.Always) {
                    // ignore search engine indexes, go straight to database
                    getHitsFromDatabase(cmd, hits, field, ds);
                    if (hits.Count > 0) return hits;
                } else {
                    if (field != null && field.Searchable) {
                        // field is in our index and is searchable.
                        if (cmd.IsComparisonQuery()) {
                            // command is doing < > !=, can't search for that!
                            if (cmd.IsAllowedToQueryComparisons()) {
                                return getHitsFromDatabase(cmd, hits, field, ds);
                            }
                        }
                    } else {
                        // field is not defined in our index or is not searchable in our index, don't bother checking the search engine.  But we may need to query the database...
                        if (cmd.IsAllowedToQueryNonIndexedFields()) {
                            return getHitsFromDatabase(cmd, hits, field, ds);
                        }
                    }
                }

                if (cmd.Keyword.ToString().ToUpper() == "NULL") {
                    // can't search for null in a search engine.  do a database lookup.
                    if (cmd.PassthruLevel != DatabasePassthruLevel.None) {
                        getHitsFromDatabase(cmd, hits, field, ds);
                    }
                    return hits;
                }

            }

            // we make it here, we need to actually search the indexes instead of doing a database query...

            using (var hpt = new HighPrecisionTimer("indexsearch", true)) {
                List<KeywordMatch<TKey, TValue>> kms = _keywordTree.Search(cmd.Keyword, cmd.MatchMode, cmd.IgnoreCase);
                if (kms.Count > 0) {
                    lock (this.lockForReading) {

                        switch (DataLocation) {
                            case DataLocation.InLeaf:
                                throw new NotImplementedException(getDisplayMember("GetHits{inleaf}", "GetHits() isn't implemented for DataLocation.InLeaf"));
                            //foreach (KeywordMatch<TKey, TValue> km in kms) {
                            //    Hit hit = km.Value as Hit;
                            //}
                            //break;

                            case DataLocation.InSeparateFile:


                                // if they want to perform auto-lookups on coded values, we may have multiple ordinals to compare against...
                                var ordinals = new List<int>();
                                ordinals.Add(ordinal);  // first add the code field itself (or -1 if no field specified, meaning match regardless of field)

                                var fldName = (cmd.FieldName + "_name_").ToLower();
                                if (cmd.LanguageID < 1) {
                                    foreach (var f in DatabaseFields) {
                                        if (f.Name.ToLower().StartsWith(fldName)) {
                                            ordinals.Add(f.Ordinal);
                                        }
                                    }
                                } else {
                                    foreach (var f in DatabaseFields) {
                                        if (f.Name.ToLower() == fldName + cmd.LanguageID) {
                                            ordinals.Add(f.Ordinal);
                                            break;
                                        }
                                    }
                                }

                                var ords = ordinals.ToArray();

                                foreach (KeywordMatch<TKey, TValue> km in kms) {
                                    BPlusListLong bpll = km.Value as BPlusListLong;
                                    foreach (long location in bpll.Value) {
                                        hits.AddRange(Hit.ReadAll(_dataReader, location, ords, cmd.KeywordOffset, this.TemporaryID));
                                    }
                                }

                                if (hits.Count == 0)
                                {
                                    if ((cmd.IsAllowedToQueryOnZeroHits()))
                                    {
                                        // nothing found in index.  user specified to look in database if nothing was found.
                                        getHitsFromDatabase(cmd, hits, field, ds);
                                    }
                                }
                                break;
                            default:
                                throw new NotImplementedException(getDisplayMember("GetHits{datalocation}", "GetHits() is missing a case statement in switch(DataLocation) for value={0}", DataLocation.ToString()));
                        }
                    }
                }
                Debug.WriteLine("Reading index " + this.Name + " took " + hpt.ElapsedMilliseconds + " ms");
            }
            //cmd.HitList = hits;
            return hits;
		}

        private List<Hit> getHitsFromDatabase(SearchCommand<TKey, TValue> cmd, List<Hit> hits, Field field, DataSet ds) {

            // try to query the database to fulfill this search query
            var dc = cmd.GenerateSqlCommand(this, field);
            var hitCount = 0;
            if (dc != null) {
                try {
                    using (var dm = DataManager.Create(this.Indexer.DataConnectionSpec)) {

                        using (IDataReader idr = dm.Stream(dc)) {

                            var ord = GetFieldDatabaseOrdinal(cmd.FieldName);

                            while (idr.Read()) {
                                Hit hit = new Hit(0, 0, ord, 0, IndexedFields);
                                // GenerateSqlCommand always returns first column as pk, indexed fields as subsequent ones.
                                hit.PrimaryKeyID = idr.GetInt32(0);

                                // first, get all values of IndexedFields so we can store them with each hit
                                for (int i = 0; i < IndexedFields.Count; i++) {
                                    // NOTE: assumes value must be an int, as this is all we can store in HitField.
                                    var idxField = IndexedFields[i];
                                    if (idxField.IsBoolean) {
                                        hit.Fields[i].Value = idr.GetValue(i + 1).ToString() == idxField.TrueValue.ToString() ? 1 : 0;
                                    } else {
                                        hit.Fields[i].Value = Toolkit.ToInt32(idr.GetValue(i + 1), 0);
                                    }
                                }
                                hits.Add(hit);
                                hitCount++;
                            }

                            // mark command as reading from database since the sql was valid (even if no hits)
                            // this will prevent it from being cached so subsequent searches correctly re-query the database for info.
                            cmd.ReadFromDatabase = true;

                            if (ds != null) {
                                ds.Tables["SqlStatement"].Rows.Add(new object[] { "SUCCESS", this.Name, cmd.FieldName, dc.ToString(), "", hitCount });
                            }
                        }
                    }
                } catch (Exception ex) {
                    if (ds != null) {
                        ds.Tables["SqlStatement"].Rows.Add(new object[] { "ERROR", cmd.RestrictToIndex, cmd.FieldName, dc.ToString(), ex.Message, hitCount });
                    } else {
                        logError(ex.ToString(true));
                    }
                }
            }
            return hits;
        }

        /// <summary>
        /// Returns all pk IDs for the given keyword.  Can optionally be an inexact match (startswith / endswith / contains) or be case insensitive.
        /// </summary>
        /// <param name="cmd">A command detailing the search parameters.</param>
        /// <returns></returns>
        internal IEnumerable<int> GetPrimaryKeyIDs(SearchCommand<TKey, TValue> cmd, DataSet ds, SearchOptions opts) {


            var ret = new List<int>();

            // index isn't ready yet, just throw back empty set
            if (this.State != IndexState.Open) {
                return ret;
            }


            if (!String.IsNullOrEmpty(cmd.RestrictToIndex) && this.Name.ToLower() != cmd.RestrictToIndex.ToLower()) {
                // not in correct index according to field definition. just throw back empty set
                return ret;
            }





            var hits = GetHits(cmd, ds, opts);
            foreach (var h in hits) {
                ret.Add(h.PrimaryKeyID);
            }

            return ret;

            //// TODO: add passthru-sql here!

            //List<KeywordMatch<TKey, TValue>> kms = _keywordTree.Search(cmd.Keyword, cmd.MatchMode, cmd.IgnoreCase);
            //if (kms.Count > 0) {
            //    lock (this.lockForReading) {

            //        switch (DataLocation) {
            //            case DataLocation.InLeaf:
            //                throw new NotImplementedException("GetPrimaryKeyIDs() isn't implemented for DataLocation.InLeaf");
            //            //foreach (KeywordMatch<TKey, TValue> km in kms) {
            //            //    Hit hit = km.Value as Hit;
            //            //}
            //            //break;

            //            case DataLocation.InSeparateFile:
            //                // if ordinal == int.MaxValue, it means they specified a field that doesn't exist in this index.
            //                // therefore, we know all hits we find will be in the wrong field, so no need to even check the index.
            //                // if ordinal < int.MaxValue, they either didn't specify a field at all or they specified one that 
            //                // does exist in this index.  So we need to check it.
            //                var ordinal = cmd.GetFieldOrdinal(this);
            //                if (ordinal < int.MaxValue) {
            //                    foreach (KeywordMatch<TKey, TValue> km in kms) {
            //                        BPlusListLong bpll = km.Value as BPlusListLong;
            //                        foreach (long location in bpll.Value) {
            //                            ret.AddRange(Hit.ReadAllPrimaryKeyIDs(_dataReader, location, ordinal, cmd.KeywordOffset, this.TemporaryID));
            //                            // hits.AddRange(Hit.ReadAll(_dataReader, location, cmd.GetFieldOrdinal(this), cmd.KeywordIndex, this.TemporaryID));
            //                        }
            //                    }
            //                }
            //                break;
            //            default:
            //                throw new NotImplementedException("GetPrimaryKeyIDs() is missing a case statement in switch(DataLocation)");
            //        }
            //    }
            //}
            //return ret;
        }



        private DataCommand createDataCommand(int pkID) {
            // enable the where clause be uncommenting it...
            var cmd = new DataCommand(this.Sql.Replace("/*_", "").Replace("_*/", ""));
            cmd.DataParameters.Add(new DataParameter(":idlist", new int[] { pkID }, DbPseudoType.IntegerCollection));
            cmd.DataParameters.Add(new DataParameter(":readall", 0, DbType.Int32));
            return cmd;
        }

        public Hit CreateNewHit(string fieldName, int primaryKeyID, int keywordOffset){

            int ordinal = GetFieldDatabaseOrdinal(fieldName);

            Hit h = new Hit(-1, primaryKeyID, ordinal, keywordOffset, IndexedFields);

            if (IndexedFields.Count > 0) {

                // now, we must fill the IndexedFields with proper values
                // so foreign keys are populated correctly and resolving by index id
                // (i.e. keyword -> idx.idList -> resolver.idList)
                // works as intended.

                using (var dm = DataManager.Create()) {

                    // retrieve current values for all indexed fields so we can store them in the index
                    var cmd = this.createDataCommand(primaryKeyID);
                    using (var idr = dm.Stream(cmd)) {
                        if (idr.Read()) {
                            for (int i = 0; i < IndexedFields.Count; i++) {
                                // NOTE: assumes value must be an int, as this is all we can store in HitField.
                                h.Fields[i].Value = Toolkit.ToInt32(idr.GetValue(IndexedFields[i].Ordinal), 0);
                            }
                        }
                    }
                }
            }

            return h;

        }

		/// <summary>
		/// Returns all hits for the given keyword.  Can optionally be an inexact match (startswith / endswith / contains) or be case insensitive.
		/// </summary>
		/// <param name="keyword">The text to search for.</param>
		/// <param name="fieldName">Name of field to restrict hits to.  Pass -1 to disable this restriction</param>
		/// <param name="keywordIndex">Index of keyword within a field to restrict hits to.  Used for phrase searches.  Pass -1 to disable this restriction.</param>
		/// <param name="matchMode">Specifies what to consider a hit -- exact keyword or conatins keyword somehow (starts with keyword, ends with keyword, keyword anywhere in the word) </param>
		/// <param name="ignoreCase">If true, keyword comparisons are case-insensitive.  Case-sensitive otherwise.</param>
		/// <returns></returns>
        public IEnumerable<Hit> GetHits(string keyword, string fieldName, int keywordIndex, KeywordMatchMode matchMode, bool ignoreCase, DataSet ds, SearchOptions opts) {
			TKey key = new TKey().Parse(keyword);
			return GetHits(new SearchCommand<TKey, TValue> { Keyword = key, KeywordOffset = keywordIndex, MatchMode = matchMode, IgnoreCase = ignoreCase, FieldName = fieldName }, ds, opts);
		}

        internal long AppendHitToData(Hit h) {
            long ret = _dataWriter.BaseStream.Position = _dataWriter.BaseStream.Length;
            // write a length of 1
            _dataWriter.Write((int)1);
            h.Write(_dataWriter, false);
            _dataWriter.Flush();
            return ret;
        }

        /// <summary>
        /// Returns # of hits zeroed out.  out variable contains total # of hits.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="primaryKeyID"></param>
        /// <param name="totalHits"></param>
        /// <returns></returns>
        internal int ZeroOutHit(long startPosition, int primaryKeyID, out int totalHits) {
            long previousPosition = startPosition;
            int count = 0;
            totalHits = 0;
            foreach (Hit h in Hit.ReadHits(_dataReader, startPosition, null, -1, -1)) {
                totalHits++;
                if (h.PrimaryKeyID == primaryKeyID) {
                    _dataWriter.BaseStream.Position = previousPosition;
                    Hit h2 = h.CloneOnlyFields(-1, -1, h.FieldOrdinal, h.KeywordIndex);
                    h2.Write(_dataWriter, false);
                    _dataWriter.Flush();
                    count++;
                }
                previousPosition = _dataReader.BaseStream.Position;
            }
            return count;
        }

        ///// <summary>
        ///// Given a keyword, returns all hits associated with it.  Keyword must match exactly (including case)
        ///// </summary>
        ///// <param name="keyword"></param>
        ///// <returns></returns>
        //public List<Hit> this[string keyword] {
        //    get {
        //        return GetHits(keyword, null, -1, KeywordMatchMode.ExactMatch, false).ToList();
        //    }
        //}

        public List<int> GetResolvedIDs(Hit h, string resolverName, SearchOptions opts) {
            return GetResolvedIDs(new Hit[] { h }, resolverName, opts);
        }

		/// <summary>
		/// For times when we've already pulled the list of hits
		/// </summary>
		/// <param name="hits"></param>
        /// <param name="resolverName"></param>
		/// <returns></returns>
        public List<int> GetResolvedIDs(IEnumerable<Hit> hits, string resolverName, SearchOptions opts) {

			// to get resolved ids from a keyword, we must do the following:
			//   Get all hits for that keyword (given to us already)
			//      For each primary key id in the hit
			//         look up resolved ids

			List<int> resolvedIDs = new List<int>();

			// index isn't ready for querying yet, just return empty set
			if (this.State != IndexState.Open) {
				return resolvedIDs;
			}

			Resolver<TKey, TValue> res = null;

            if (String.IsNullOrEmpty(resolverName)){
                // they may not be resolving. just return empty list.
                return resolvedIDs;
            }

			resolverName = resolverName.ToLower();

			if (!Resolvers.TryGetValue(resolverName, out res)) {
                // no resolver by that name exists on this index. jump out.
				return resolvedIDs;
			} else {


                // if caller opts to ignore resolver cache, we still need to pull resolvers from PrimaryKey and ForeignKey from 
                // the hit information anyway.  So that option is only applicable to the Sql mode, which it handles below using the GetResolvedIDsViaSql() method.
                // if caller opted for Always as a passthru level, that means the hits we found were not from the index caches but from
                // the database.  They will therefore contain the latest, non-cached data, meaning so will the resolved id's they have in them.



				// the hit itself may contain the data we need.
				// if Sql = :primarykeyid, use the h.PrimaryKeyID.
				// if Sql = :(name of fk field here), use the h.IndexedFields[(name of fk field here)].Value
				// else, use the h.PrimaryKeyID to resolve it using the cache file.
				switch (res.Method) {
					case ResolutionMethod.PrimaryKey:
						// use hit.PrimaryKeyID
						foreach(Hit h in hits){
							// should never have zeroes for primary key id.
							resolvedIDs.Add(h.PrimaryKeyID);
						}
						break;
					case ResolutionMethod.ForeignKey:
						// use indexed field
						int indexedFieldOrdinal = GetFieldIndexOrdinal(res.ForeignKeyField);
						if (indexedFieldOrdinal == -1) {
							throw new InvalidOperationException(getDisplayMember("GetResolvedIDs{resolverfk}", "The indexer config file for index '{0}' and resolver '{1}' has the Sql node as resolving the id via the index field named '{2}'.  However, this field is not stored in the index.  Add the following node: /Index/Fields/Field[@Name='{3}', StoredInIndex='true'] and recreate the index.", this.Name, res.Name, res.ForeignKeyField, res.ForeignKeyField));
                        } else {
                            foreach (Hit h in hits) {
                                if (indexedFieldOrdinal >= h.Fields.Length) {
                                    throw new InvalidOperationException(getDisplayMember("GetResolvedIDs{resolverordinal}", "The indexer config file for index '{0}' and resolver '{1}' has the Sql node as resolving the id via the index field name '{2}', but that field is not stored in the index.  Add the following node: /Index/Fields/Field[@Name='{3}', StoredInIndex='true'] and recreate the index.", this.Name, res.Name, res.ForeignKeyField, res.ForeignKeyField));
                                } else {
                                    int val = h.Fields[indexedFieldOrdinal].Value;
                                    if (val > 0) {
                                        // foreign keys that are null show up as zeroes.  exclude them.
                                        resolvedIDs.Add(val);
                                    }
                                }
                            }
                        }
						break;
					case ResolutionMethod.Sql:

						// we may need to pull from data file or database, depending on opts
                        // either way, we should create a big list of pk ids before calling into the resolver method in case we do hit the database
                        // and multiple pk ids need to be sent to the db at one time

                        var pkIDs = new List<int>();
						foreach (Hit h in hits) {
                            pkIDs.Add(h.PrimaryKeyID);
                        }

                        resolvedIDs.AddRange(res.GetResolvedIDsViaSql(pkIDs, opts));

						break;
					default:
						throw new NotImplementedException(getDisplayMember("GetResolvedIDs{default}", "Method '{0}' not implemented in Index.GetResolvedIDs()", res.Method.ToString()));
				}
			}

			return resolvedIDs;
		}

		/// <summary>
		/// For times when we want to go straight through to the resolved ids
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
        public List<int> GetResolvedIDsViaPrimaryOrForeignKey(SearchCommand<TKey, TValue> cmd, DataSet ds, SearchOptions opts) {

            //if (!String.IsNullOrEmpty(cmd.RestrictToIndex) && this.Name.ToLower() != cmd.RestrictToIndex.ToLower()){
            //    return new List<int>();
            //}

            //if (cmd.KeywordIndex < 0 
            //    && this.Resolvers.ContainsKey(cmd.ResolverName) 
            //    && this.Resolvers[cmd.ResolverName].EnableByKeyword) {
            //    // we can bypass hit lookup and go directly to the resolver tree
            //    return this.Resolvers[cmd.ResolverName].GetResolvedIDs(cmd.Keyword.ToString(), cmd.MatchMode, cmd.IgnoreCase);
            //} else {
                IEnumerable<Hit> hits = GetHits(cmd, ds, opts);
                return GetResolvedIDs(hits, cmd.ResolverName, opts);
            //}
		}

        /// <summary>
        /// Used internally by search engine for realtime updating.  Uses Index hits and (if Method==Sql) id-based resolver tree for resolving ids.  Does NOT use keyword-based resolver tree.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="resolverName"></param>
        /// <returns></returns>
        internal List<int> getResolvedIDs(string keyword, string resolverName, DataSet ds, SearchOptions opts) {
            IEnumerable<Hit> hits = GetHits(keyword, null, -1, KeywordMatchMode.ExactMatch, false, ds, opts);
            return GetResolvedIDs(hits, resolverName, opts);
        }

        /// <summary>
        /// Used internally by search engine for realtime updating.  Uses Index hits and (if Method==Sql) id-based resolver tree for resolving ids.  PrimaryKeyID must match given value.  Does NOT use keyword-based resolver tree.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="primaryKeyID"></param>
        /// <param name="resolverName"></param>
        /// <returns></returns>
        internal List<int> getResolvedIDs(string keyword, int primaryKeyID, string resolverName, DataSet ds, SearchOptions opts) {
            IEnumerable<Hit> hits = GetHits(keyword, null, -1, KeywordMatchMode.ExactMatch, false, ds, opts);
            var h2 = new List<Hit>();
            foreach (var h in hits) {
                if (h.PrimaryKeyID == primaryKeyID) {
                    h2.Add(h);
                }
            }
            return GetResolvedIDs(h2, resolverName, opts);
        }

        /// <summary>
        /// Used internally by search engine for realtime updating
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="fieldName"></param>
        /// <param name="resolverName"></param>
        /// <returns></returns>
        internal List<int> getPrimaryKeyIDs(string keyword, DataSet ds, SearchOptions opts) {
            IEnumerable<Hit> hits = GetHits(keyword, null, -1, KeywordMatchMode.ExactMatch, false, ds, opts);
            var rv = new List<int>();
            foreach (var h in hits) {
                rv.Add(h.PrimaryKeyID);
            }
            return rv;
        }

		#endregion Methods for Reading

		/// <summary>
		/// Disposes this index then deletes all associates files (including all resolvers and all their files)
		/// </summary>
		private void DeleteFiles() {

			Dispose();

			if (_keywordTree != null) {
				_keywordTree.Dispose();
				_keywordTree = null;
			}

			// delete any/all files in the target Folder that start with this index's name (includes resolver files too!)
            string dir = Toolkit.ResolveDirectoryPath(FolderName, false);
			foreach (string name in Directory.GetFiles(dir, this.Name + ".*", SearchOption.TopDirectoryOnly)) {
                if (!name.ToLower().EndsWith(".stats")) {
                    // the stats file should not be overwritten!
                    File.Delete(name);
                }
			}

			//calcFilePath(".xml", true, false);
			//calcFilePath(".index", true, false);
			//calcFilePath(".data", true, false);

			//foreach (string name in Resolvers.Keys) {
			//    Resolver<TKey, TValue> r = Resolvers[name];
			//    if (r != null) {
			//        r.DeleteFiles();
			//    }
			//}

		}

        internal void SaveEnabledToConfig() {
            // write the new enabled setting to the config file
            var doc = new XmlDocument();
            doc.Load(Indexer.ConfigFilePath);
            var ndIdx = doc.SelectSingleNode("/Indexes/Index[@Name='" + this.Name + "']");
            if (ndIdx != null) {
                var val = Toolkit.GetAttValue(ndIdx, "Enabled", null);
                if (val == null) {
                    var att = doc.CreateAttribute("Enabled");
                    att.Value = this.Enabled.ToString();
                    ndIdx.Attributes.Append(att);
                } else {
                    ndIdx.Attributes["Enabled", ""].Value = this.Enabled.ToString();
                }
                doc.Save(Indexer.ConfigFilePath);
            }
        }

        private DataTable stripNotInIndex(DataTable dt, string name) {
            // strip off all rows that are not part of this index definition
            name = name.ToLower();
            for (var i = 0; i < dt.Rows.Count; i++) {
                var dr = dt.Rows[i];
                if (dr["index_name"].ToString().ToLower() != name) {
                    dt.Rows.Remove(dr);
                    i--;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        internal void SaveConfig(DataSet ds, DataRow dr) {

            if (dr != null && ds != null) {

                // pluck values out of given parameters, save to xml file.
                var doc = new XmlDocument();

                doc.Load(Indexer.ConfigFilePath);

                var nd = doc.SelectSingleNode("/Indexes/Index[@Name='" + this.Name + "']");
                if (nd == null) {
                    nd = doc.CreateElement("Index");
                    doc.DocumentElement.AppendChild(nd);
                    Toolkit.SetAttValue(nd, "Name", this.Name);
                }

                // enabled affects current object
                var enabled = dr["enabled"].ToString();
                Toolkit.SetAttValue(nd, "Enabled", enabled);
                this.Enabled = Toolkit.ToBoolean(enabled, this.Enabled);

                // as does sql...
                var providerName = Indexer.DataConnectionSpec.ProviderName.ToLower();
                this.Sql = dr[providerName + "_sql_statement"].ToString();

                Toolkit.SetAttValue(nd, "FanoutSize", dr["fanout_size"].ToString());
                Toolkit.SetAttValue(nd, "AverageKeywordSize", dr["average_keyword_size"].ToString());
                Toolkit.SetAttValue(nd, "NodeCacheSize", dr["node_cache_size"].ToString());
                Toolkit.SetAttValue(nd, "Encoding", dr["encoding"].ToString());
                Toolkit.SetAttValue(nd, "StripHtml", dr["strip_html"].ToString());
                Toolkit.SetAttValue(nd, "PrimaryKeyField", dr["primary_key_field"].ToString());
                Toolkit.SetAttValue(nd, "AutoIndexStringFields", dr["auto_index_string_fields"].ToString());
                Toolkit.SetAttValue(nd, "SqlSource", dr["sql_source"].ToString());
                Toolkit.SetAttValue(nd, "DataviewName", dr["dataview_name"].ToString());
                Toolkit.SetAttValue(nd, "KeywordCacheSize", dr["keyword_cache_size"].ToString());
                Toolkit.SetAttValue(nd, "MaxSortSizeInMB", dr["max_sort_size_in_mb"].ToString());

                // save sql here

                // save fields here
                var fields = nd.SelectSingleNode("Fields");
                if (fields == null) {
                    fields = doc.CreateElement("Fields");
                    nd.AppendChild(fields);
                }

                var dtField = ds.Tables["index_field"];

                foreach (DataRow drField in dtField.Rows) {
                    var name = drField["field_name"].ToString();
                    var field = fields.SelectSingleNode("Field[@Name='" + name + "']");
                    if (field == null) {
                        field = doc.CreateElement("Field");
                        fields.AppendChild(field);
                    }
                    Toolkit.SetAttValue(field, "Name", name);
                    Toolkit.SetAttValue(field, "IsPrimaryKey", drField["is_primary_key"].ToString());
                    Toolkit.SetAttValue(field, "StoredInIndex", drField["is_stored_in_index"].ToString());
                    Toolkit.SetAttValue(field, "Searchable", drField["is_searchable"].ToString());
                    Toolkit.SetAttValue(field, "Format", drField["format"].ToString());
                    Toolkit.SetAttValue(field, "Type", drField["type"].ToString());
                    Toolkit.SetAttValue(field, "IsBoolean", drField["is_boolean"].ToString());
                    Toolkit.SetAttValue(field, "TrueValue", drField["true_value"].ToString());
                    Toolkit.SetAttValue(field, "ForeignKeyTable", drField["foreign_key_table"].ToString());
                    Toolkit.SetAttValue(field, "ForeignKeyField", drField["foreign_key_field"].ToString());
                    Toolkit.SetAttValue(field, "Calculation", drField["calculation"].ToString());
                }


                doc.Save(Indexer.ConfigFilePath);


            }

        }

        /// <summary>
        /// Disposes this object and deletes all associated files.
        /// </summary>
        public void Delete() {

            // make sure nobody is currently using this index
            Dispose();

            // delete any files that this index may have created
            deleteFile(_statFile);
            deleteFile(_errorFile);

            // delete any temp files that are created during index rebuild (in case the delete call came in during a build)
            string[] tempFiles = Directory.GetFiles(Toolkit.ResolveDirectoryPath(FolderName, false), calcFileName(".data.temp_") + "*");
            foreach (string tf in tempFiles) {
                try {
                    File.Delete(tf);
                } catch { }
            }
            string[] sortFiles = Directory.GetFiles(Toolkit.ResolveDirectoryPath(FolderName, false), calcFileName(".data.sort_") + "*");
            foreach (string sf in sortFiles) {
                try {
                    File.Delete(sf);
                } catch { }
            }


            deleteFile(calcFilePath(".xml", false, false, false));
            deleteFile(calcFilePath(".queue", false, false, false));
            deleteFile(calcFilePath(".data", false, false, false));
            deleteFile(calcFilePath(".index", false, false, false));

            // finally remove the definition from the config file
            var doc = new XmlDocument();
            doc.Load(Indexer.ConfigFilePath);
            var ndIdx = doc.SelectSingleNode("/Indexes/Index[@Name='" + Name + "']");
            if (ndIdx != null) {
                doc.DocumentElement.RemoveChild(ndIdx);
            }
            doc.Save(Indexer.ConfigFilePath);
        }

        private void deleteFile(string fileName) {
            if (File.Exists(fileName)) {
                try {
                    File.Delete(fileName);
                } catch { }
            }
        }

		#region IDisposable Members

		public void Dispose() {

			this.State = IndexState.Closing;

            closeDataFile();

            if (_keywordTree != null) {
				_keywordTree.Dispose();
				_keywordTree = null;
			}

			this.State = IndexState.Closed;

			foreach (string name in Resolvers.Keys) {
				Resolver<TKey, TValue> r = Resolvers[name];
				if (r != null) {
					r.Dispose();
					r = null;
				}
			}
		}

		#endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "Index", resourceName, null, defaultValue, substitutes);
        }
    
    }
}
