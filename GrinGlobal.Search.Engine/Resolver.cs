using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core.Xml;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using GrinGlobal.Core;
using System.Data;
using System.Diagnostics;

using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine {
    /// <summary>
    /// Class for resolving a primary key id from one table to one or more primary keys in another table.  Creates multiple files as needed (b+ tree, queue, data, etc).  NOTE: Resolver is always created as a BPlusInt -> BPlusListLong.  The generic typing applies only to the type of the Index this Resolver is to be associated with.  That's why the type names are TIndexKey and TIndexValue.
    /// </summary>
    public class Resolver<TIndexKey, TIndexValue> : IDisposable  // ISearchable<BPlusInt>, IPersistable<BPlusListLong> {
        where TIndexKey : ISearchable<TIndexKey>, new()
        where TIndexValue : IPersistable<TIndexValue>, new() {


        #region Supporting Classes
        private class PrimaryKeyResolverDataProcessingArguments {
            public int PrimaryKeyFieldOrdinal;
            public int ResolvedPrimaryKeyFieldOrdinal;
        }

        private class KeywordResolverDataProcessingArguments {
            public string Keyword;
            public int ResolvedPrimaryKeyFieldOrdinal;
        }

        #endregion Supporting Classes


        #region Properties

        public bool IsConfigurationValid { get; private set; }

        /// <summary>
        /// The current position of the file pointer in the data file
        /// </summary>
        private long DataPosition {
            get {
                return _primaryKeyDataWriter != null ?
                    _primaryKeyDataWriter.BaseStream.Position :
                    _primaryKeyDataReader != null ?
                    _primaryKeyDataReader.BaseStream.Position :
                    -1;
            }
        }

        /// <summary>
        /// The current position of the file pointer in the data file
        /// </summary>
        private long KeywordDataPosition {
            get {
                return _keywordDataWriter != null ?
                    _keywordDataWriter.BaseStream.Position :
                    _keywordDataReader != null ?
                    _keywordDataReader.BaseStream.Position :
                    -1;
            }
        }

        private object lockForReading = new object();
        private object lockForWriting = new object();

        private string _name;
        /// <summary>
        /// Gets or sets the name of the resolver
        /// </summary>
        public string Name {
            get {
                return _name;
            }
            set {
                _name = (value == null ? string.Empty : value.ToLower());
            }
        }

        private const int BUFFER_SIZE = 1024 * 1024;

        /// <summary>
        /// Gets or sets the Sql used to generate the resolver data
        /// </summary>
        public string Sql { get; set; }

        public string GetSqlByEngine(string engineName){
            string val = "";
            _sqlStatements.TryGetValue(engineName, out val);
            return val;
        }


        /// <summary>
        /// Same value as Sql property, but with /*_ and _*/ removed (so any where clauses are included as part of the sql statement). Note any SQL should be munged to work with :readall existing (new style) or not (old style).
        /// </summary>
        private string _parameterizedSql;

        /// <summary>
        /// Gets or sets the method used by the Resolver to generate data
        /// </summary>
        public ResolutionMethod Method { get; set; }
        /// <summary>
        /// Gets or sets the name of the Foreign Key field used to lookup an ID
        /// </summary>
        public string ForeignKeyField { get; set; }
        /// <summary>
        /// Gets or sets the number of keywords per node in its B+ tree
        /// </summary>
        public short FanoutSize { get; set; }
        /// <summary>
        /// Gets or sets the number of bytes to use per keyword as the basis for approximating initial node size
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
        /// Gets or sets the character encoding scheme used for storing string-based data or keywords
        /// </summary>
        public string Encoding { get; set; }
        /// <summary>
        /// Gets or sets if the resolver is enabled for use, regardless of its CacheMode.  If false, resolver will always return 0 resolved IDs for any list of primary key IDs.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets if resolver can be updated with real time updates.  Default is true for CacheMode=Id, false otherwise.
        /// </summary>
        public bool AllowRealTimeUpdates { get; private set; }
        ///// <summary>
        ///// Gets if keyword-based resolution is enabled.  Defaults to true.
        ///// </summary>
        //public bool EnableByKeyword { get; private set; }
        /// <summary>
        /// Gets the level of cache.  Can be None (resolver goes to database for lookups), Id (resolver cache by ID), or IdAndKeyword (resolver cache by ID and Keyword)
        /// </summary>
        public ResolverCacheMode CacheMode { get; private set; }

        /// <summary>
        /// Gets or sets the dataview used to fill the resolver with data from the database.
        /// </summary>
        public string DataviewName { get; set; }

        /// <summary>
        /// Gets the Index object with which this Resolver is associated
        /// </summary>
        public Index<TIndexKey, TIndexValue> Index { get; private set; }
        /// <summary>
        /// Gets or sets the name of the Primary Key Field in the Index object to use for resolving an ID
        /// </summary>
        public string PrimaryKeyField { get; set; }

        /// <summary>
        /// Gets the location of the data.  If InSeparateFile, data is stored in a .data file.  If InLeaf, data is stored directly in the leaf itself.  Default is InSeparateFile.
        /// </summary>
        private DataLocation DataLocation { get; set; }

        private DataFileProcessor<ResolveIDMapping> PrimaryKeyDataFileProcessor { get; set; }
        private DataFileProcessor<ResolveKeywordMapping> KeywordDataFileProcessor { get; set; }

        private string PrimaryKeyDataFileName {
            get {
                initBaseFileName();
                return _baseFileName + ".data";
            }
        }
        private string KeywordDataFileName {
            get {
                initBaseFileName();
                return _baseFileName + ".keyword.data";
            }
        }
        private string KeywordTreeFileName {
            get {
                initBaseFileName();
                return _baseFileName + ".keyword.index";
            }
        }
        private string PrimaryKeyTreeFileName {
            get {
                initBaseFileName();
                return _baseFileName + ".index";
            }
        }

        private string QueueFileName {
            get {
                initBaseFileName();
                return _baseFileName + ".queue";
            }
        }
        /// <summary>
        /// Gets or sets the primary key field of the ancestor's table (whose ID is to be resolved)
        /// </summary>
        public string ResolvedPrimaryKeyField { get; set; }
        private string _baseFileName;
        private BPlusTree<BPlusInt, BPlusListLong> _primaryKeyTree;
        private FileStream _primaryKeyDataFileStream;
        private BinaryReader _primaryKeyDataReader;
        private BinaryWriter _primaryKeyDataWriter;

        private BPlusTree<BPlusString, BPlusListLong> _keywordTree;
        private FileStream _keywordDataFileStream;
        private BinaryReader _keywordDataReader;
        private BinaryWriter _keywordDataWriter;

        private BinaryWriter _queueWriter;

        #endregion Properties


        private Resolver() {
            PrimaryKeyDataFileProcessor = new DataFileProcessor<ResolveIDMapping>();
            KeywordDataFileProcessor = new DataFileProcessor<ResolveKeywordMapping>();
        }

        #region Statics
//        internal static Node SaveSettings(Dictionary<string, string> settings, string indexName, string resolverName, Indexer<TIndexKey, TIndexValue> indexer, Resolver<TIndexKey, TIndexValue> defaultResolver) {

//            var category = "Index-" + indexName.ToLower() + "-Resolver-" + resolverName.ToLower();

//            var defaultVal = (defaultResolver == null ? "" : null);

//            Node nd = new Node("Resolver");

//            nd.Attributes.SetValue("Name", resolverName);

//            var fanoutSize = Lib.GetSetting(settings, category, "FanoutSize", defaultVal ?? defaultResolver.FanoutSize.ToString());
//            nd.Attributes.SetValue("FanoutSize", fanoutSize);

//            var encoding = Lib.GetSetting(settings, category, "Encoding", defaultVal ?? defaultResolver.Encoding.ToString());
//            nd.Attributes.SetValue("Encoding", encoding);

//            var averageKeywordSize = Lib.GetSetting(settings, category, "AverageKeywordSize", defaultVal ?? defaultResolver.AverageKeywordSize.ToString());
//            nd.Attributes.SetValue("AverageKeywordSize", averageKeywordSize);

//            var nodeCacheSize = Lib.GetSetting(settings, category, "NodeCacheSize", defaultVal ?? defaultResolver.NodeCacheSize.ToString());
//            nd.Attributes.SetValue("NodeCacheSize", nodeCacheSize);

//            var keywordCacheSize = Lib.GetSetting(settings, category, "KeywordCacheSize", defaultVal ?? defaultResolver.KeywordCacheSize.ToString());
//            nd.Attributes.SetValue("KeywordCacheSize", keywordCacheSize);

//            var enabled = Lib.GetSetting(settings, category, "FanoutSize", defaultVal ?? defaultResolver.Enabled.ToString());
//            nd.Attributes.SetValue("Enabled", enabled);

//            var allowRealTimeUpdates = Lib.GetSetting(settings, category, "AllowRealTimeUpdates", defaultVal ?? defaultResolver.AllowRealTimeUpdates.ToString());
//            nd.Attributes.SetValue("AllowRealTimeUpdates", allowRealTimeUpdates);

//            var cacheMode = Lib.GetSetting(settings, category, "CacheMode", defaultVal ?? defaultResolver.CacheMode.ToString());
//            nd.Attributes.SetValue("CacheMode", cacheMode);
////            nd.Attributes.SetValue("EnableByKeyword", EnableByKeyword.ToString());

//            var method = Lib.GetSetting(settings, category, "Method", defaultVal ?? defaultResolver.Method.ToString());
//            nd.Attributes.SetValue("Method", method);

//            var foreignKeyField = Lib.GetSetting(settings, category, "ForeignKeyField", defaultVal ?? defaultResolver.ForeignKeyField.ToString());
//            nd.Attributes.SetValue("ForeignKeyField", foreignKeyField);

//            var dataLocation = Lib.GetSetting(settings, category, "DataLocation", defaultVal ?? defaultResolver.DataLocation.ToString());
//            nd.Attributes.SetValue("DataLocation", dataLocation);

//            var queryPrimaryKeyField = Lib.GetSetting(settings, category, "QueryPrimaryKeyField", defaultVal ?? defaultResolver.PrimaryKeyField.ToString());
//            nd.Attributes.SetValue("QueryPrimaryKeyField", queryPrimaryKeyField);

//            var queryResolvedPrimaryKeyField = Lib.GetSetting(settings, category, "QueryResolvedPrimaryKeyField", defaultVal ?? defaultResolver.ResolvedPrimaryKeyField.ToString());
//            nd.Attributes.SetValue("QueryResolvedPrimaryKeyField", queryResolvedPrimaryKeyField);

//            var dataviewName = Lib.GetSetting(settings, category, "DataviewName", defaultVal ?? defaultResolver.DataviewName.ToString());
//            if (dataviewName.ToLower() == "(none)") {
//                dataviewName = string.Empty;
//            }
//            nd.Attributes.SetValue("DataviewName", dataviewName);

//            var sqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), Lib.GetSetting(settings, category, "SqlSource", "None"), (int)SqlSource.None));

//            nd.Attributes.SetValue("SqlSource", sqlSource.ToString());

//            var engines = new string[] { "mysql", "sqlserver", "oracle", "postgresql" };
//            if (sqlSource == SqlSource.Xml) {
//                // append a Sql node for each defined engine type
//                foreach (var eng in engines) {
//                    var sqlStatement = Lib.GetSetting(settings, category, "SqlFor-" + eng, (defaultVal ?? defaultResolver.GetSqlByEngine(eng)));
//                    var ndSql = new Node("Sql");
//                    ndSql.Attributes.SetValue("engine", eng);
//                    ndSql.NodeValue = sqlStatement;
//                    nd.Nodes.Add(ndSql);
//                }
//            } else if (sqlSource == SqlSource.Dataview) {
//                // write to the database
//                var dcs = indexer.DataConnectionSpec;
//                var sqls = new Dictionary<string, string>();
//                foreach (var eng in engines) {
//                    var sqlStatement = Lib.GetSetting(settings, category, "SqlFor-" + eng, (defaultVal ?? defaultResolver.GetSqlByEngine(eng)));
//                    sqls.Add(eng, sqlStatement);
//                }
//                Index<TIndexKey, TIndexValue>.saveSqlToDataview(dataviewName, dcs, sqls);
//            } else {
//                // other cases are bogus
//            }

//            return nd;
//        }

        internal static Resolver<TIndexKey, TIndexValue> FillObjectFromDataSet(DataRow dr, string indexName, string resolverName, DataConnectionSpec dcs, Index<TIndexKey, TIndexValue> parentIndex, ref bool isDatabaseDown) {

            Resolver<TIndexKey, TIndexValue> r = new Resolver<TIndexKey, TIndexValue> {
                Name = resolverName.ToLower(),
                FanoutSize = Toolkit.ToInt16(dr["fanout_size"].ToString(), 133),
                Encoding = dr["encoding"].ToString(),
                AverageKeywordSize = Toolkit.ToInt16(dr["average_keyword_size"], 4),
                Enabled = Toolkit.ToBoolean(dr["enabled"].ToString(), true),
                CacheMode = (ResolverCacheMode)(Toolkit.ToEnum(typeof(ResolverCacheMode), dr["cache_mode"].ToString(), (int)ResolverCacheMode.Id)),
                AllowRealTimeUpdates = Toolkit.ToBoolean(dr["allow_realtime_updates"], true),

                //EnableByKeyword = Toolkit.ToBoolean(nav.GetAttribute("EnableByKeyword", ""), false),
                DataLocation = (DataLocation)(Toolkit.ToEnum(typeof(DataLocation), dr["data_location"].ToString(), (int)DataLocation.InSeparateFile)),
                ForeignKeyField = dr["foreign_key_field"].ToString(),
                PrimaryKeyField = dr["primary_key_field"].ToString(),
                ResolvedPrimaryKeyField = dr["resolved_primary_key_field"].ToString(),
                Index = parentIndex,
                DataviewName = dr["dataview_name"].ToString(),
                KeywordCacheSize = Toolkit.ToInt32(dr["keyword_cache_size"], 10),
                Method = (ResolutionMethod)(Toolkit.ToEnum(typeof(ResolutionMethod), dr["method"].ToString(), (int)ResolutionMethod.None)),
                NodeCacheSize = Toolkit.ToInt32(dr["node_cache_size"], 30)
                
            };


            // TODO: sql!

            r._sqlStatements.Clear();
            var msg = "";
            if (r.Method == ResolutionMethod.Sql) {
                switch (r.Index.SqlSource) {
                    case SqlSource.Unknown:
                    default:
                        msg = getDisplayMember("FillObjectFromDataSet{unknown}", "{0} -> {1}: SqlSource is not defined properly.", r.Index.Name, r.Name);
                        r.Index.showProgress(msg, EventLogEntryType.Error, true);
                        break;
                    //case SqlSource.Xml:
                    //    // look in settings themselves itself
                    //    r.appendSqlStatement("sqlserver", res["SqlFor-sqlserver"], true);
                    //    r.appendSqlStatement("mysql", res["SqlFor-mysql"], true);
                    //    r.appendSqlStatement("postgresql", res["SqlFor-postgresql"], true);
                    //    r.appendSqlStatement("oracle", res["SqlFor-oracle"], true);

                    //    if (!r._sqlStatements.ContainsKey(r.Index.Indexer.DataConnectionSpec.ProviderName)) {
                    //        msg = "Configuration error in Resolver=" + r.Name + " for Index=" + r.Index.Name + ": SqlSource is defined as Xml, but /Indexes/Index[@Name=" + r.Index.Name + "]/Resolver[@Name=" + r.Name + "]/Sql[@engine=" + r.Index.Indexer.DataConnectionSpec.ProviderName + "] is not provided.";
                    //        r.Index.showProgress(msg, EventLogEntryType.Error, true);
                    //    }
                    //    break;
                    case SqlSource.Dataview:
                        // look in dataview definition table (sys_dataview)
                        if (!isDatabaseDown) {
                            using (var dm = DataManager.Create(r.Index.Indexer.DataConnectionSpec)) {
                                try {
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
                 ":dbenginecode", r.Index.Indexer.DataConnectionSpec.ProviderName,
                 ":dvname", r.DataviewName));

                                    if (dt.Rows.Count == 0 || String.IsNullOrEmpty(dt.Rows[0]["sql_statement"].ToString())) {
                                        msg = getDisplayMember("FillObjectFromDataSet{nosql}", "{0} -> {1}: SqlSource is defined as Dataview, but no dataview named '{2}' could be located in sys_dataview and sys_dataview_sql for database_engine_tag = '{3}'.", r.Index.Name, r.Name, r.DataviewName, dm.DataConnectionSpec.ProviderName);
                                        r.Index.showProgress(msg, EventLogEntryType.Error, true);
                                    } else {
                                        r.appendSqlStatement(r.Index.Indexer.DataConnectionSpec.ProviderName, dt.Rows[0]["sql_statement"].ToString(), true);
                                    }
                                } catch (Exception exDB) {
                                    msg = getDisplayMember("FillObjectFromDataSet{failed}", "{0} -> {1}: SqlSource is defined as Dataview, but an error occurred pulling the sys_dataview_sql.sql_statement value from the database: {2}", r.Index.Name, r.Name, exDB.Message);
                                    r.Index.showProgress(msg, EventLogEntryType.Error, true);
                                    isDatabaseDown = true;
                                }
                            }
                        }
                        break;
                }
            }
            r.ValidateConfiguration(false, isDatabaseDown);

            return r;
        }

        internal static void FillDataSetFromConfig(DataSet ds, string indexName, XmlNode nd, string resolverName, DataConnectionSpec dcs, string onlyThisIndex, string onlyThisResolver, ref bool isDatabaseDown){

            var dtResolver = ds.Tables["resolver"];
            var drResolver = dtResolver.NewRow();
            dtResolver.Rows.Add(drResolver);

            drResolver["index_name"] = indexName;
            drResolver["resolver_name"] = resolverName;
            drResolver["fanout_size"] = Toolkit.ToInt32(Toolkit.GetAttValue(nd, "FanoutSize", ""), 133);
            drResolver["average_keyword_size"] = Toolkit.ToInt16(Toolkit.GetAttValue(nd, "AverageKeywordSize", ""), 10);
            drResolver["node_cache_size"] = Toolkit.ToInt16(Toolkit.GetAttValue(nd, "NodeCacheSize", ""), 10);
            drResolver["keyword_cache_size"] = Toolkit.ToInt16(Toolkit.GetAttValue(nd, "KeywordCacheSize", ""), 10);
            drResolver["encoding"] = Toolkit.GetAttValue(nd, "Encoding", "");
            drResolver["enabled"] = Toolkit.ToBoolean(Toolkit.GetAttValue(nd, "Enabled", ""), false);

            drResolver["method"] = Toolkit.GetAttValue(nd, "Method", "");
            var sqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), Toolkit.GetAttValue(nd, "SqlSource", ""), (int)SqlSource.Unknown));
            drResolver["sql_source"] = sqlSource.ToString();
            var dataviewName = Toolkit.GetAttValue(nd, "DataviewName", "");
            drResolver["dataview_name"] = dataviewName;
            drResolver["allow_realtime_updates"] = Toolkit.ToBoolean(Toolkit.GetAttValue(nd, "AllowRealTimeUpdates", "true"), true);
            drResolver["cache_mode"] = Toolkit.GetAttValue(nd, "CacheMode", "");
            drResolver["foreign_key_field"] = Toolkit.GetAttValue(nd, "ForeignKeyField", "");
            drResolver["primary_key_field"] = Toolkit.GetAttValue(nd, "QueryPrimaryKeyField", "");
            drResolver["resolved_primary_key_field"] = Toolkit.GetAttValue(nd, "QueryResolvedPrimaryKeyField", "");

            if (sqlSource == SqlSource.Dataview) {
                if (!String.IsNullOrEmpty(dataviewName)){
                    // don't pull it if they're not listing by a specific index as it's expensive...
                    if (!String.IsNullOrEmpty(onlyThisIndex)) {
                        // pull sql from the dataview definition...
                        Index<TIndexKey, TIndexValue>.PullSqlFromDataviewIntoDataSet(drResolver, dcs, dcs.ProviderName, dataviewName, ref isDatabaseDown);
                    }
                }
            }


            //drResolver["sql_statement"] = Toolkit.GetAttValue(nd, "SqlStatement", "");
            //drResolver["mysql_sql_statement"] = Toolkit.GetAttValue(nd, "SqlFor-mysql", "");
            //drResolver["oracle_sql_statement"] = Toolkit.GetAttValue(nd, "SqlFor-oracle", "");
            //drResolver["postgresql_sql_statement"] = Toolkit.GetAttValue(nd, "SqlFor-postgresql", "");
            //drResolver["sqlserver_sql_statement"] = Toolkit.GetAttValue(nd, "SqlFor-sqlserver", "");

            ValidateConfigurationByDataRow(drResolver, ds, dcs.ProviderName, onlyThisIndex, onlyThisResolver, ref isDatabaseDown);

        }



        private static string[] __engines = new string[]{"mysql", "oracle", "postgresql", "sqlserver"};


        #endregion Statics

        internal static bool ValidateConfigurationByDataRow(DataRow dr, DataSet ds, string providerName, string onlyThisIndex, string onlyThisResolver, ref bool isDatabaseDown) {
            var indexName = dr["index_name"].ToString();

            var indexPKField = "";
            var sqlSource = SqlSource.Unknown;
            var drIndex = ds.Tables["index"].Select("index_name = '" + indexName + "'");
            if (drIndex.Length > 0) {
                indexPKField = drIndex[0]["primary_key_field"].ToString();
                sqlSource = (SqlSource)(Toolkit.ToEnum(typeof(SqlSource), drIndex[0]["sql_source"].ToString(), (int)SqlSource.None));
            }

            var sql = dr[providerName + "_sql_statement"].ToString();
            var resolverName = dr["resolver_name"].ToString();
            var pkField = dr["primary_key_field"].ToString();
            var fkField = dr["foreign_key_field"].ToString();
            var resolvedPKField = dr["resolved_primary_key_field"].ToString();
            var dataviewName = dr["dataview_name"].ToString();
            var method = (ResolutionMethod)(Toolkit.ToEnum(typeof(ResolutionMethod), dr["method"].ToString(), (int)ResolutionMethod.None));

            var sb = new StringBuilder();
            string msg;

            switch (method) {
                case ResolutionMethod.Invalid:

                    msg = indexName + " -> " + resolverName + ": 'Method' attribute must be 'Sql', 'PrimaryKey', 'ForeignKey', or 'None'.";
                    sb.AppendLine(msg);

                    break;
                case ResolutionMethod.None:
                    // nothing to validate?
                    break;
                case ResolutionMethod.PrimaryKey:
                    // nothing to validate?
                    if (String.IsNullOrEmpty(indexPKField)) {
                        msg = indexName + " -> " + resolverName + ": Method=PrimaryKey but no PrimaryKeyField is defined on the index.  Check configuration file at /Indexes/Index[@Name=" + indexName + "]/@PrimaryKey";
                        sb.AppendLine(msg);
                    }
                    break;
                case ResolutionMethod.ForeignKey:

                    // verify foreignkeyfieldname exists
                    if (String.IsNullOrEmpty(fkField)) {
                        msg = indexName + " -> " + resolverName + ": Method=ForeignKey but the Resolver/@ForeignKeyField attribute is not set.";
                        sb.AppendLine(msg);
                    }

                    // verify the field in from foreignkeyfieldname is stored in the index
                    bool found = false;
                    var fields = ds.Tables["index_field"].Select("index_name = '" + indexName + "'");
                    foreach (DataRow drF in fields){
                        if (drF["field_name"].ToString().ToLower() == fkField.ToLower()) {
                            found = true;
                            if (!Toolkit.ToBoolean(drF["is_stored_in_index"], false)) {
                                msg = indexName + " -> " + resolverName + ": resolution method is ForeignKey with the ForeignKeyField=" + fkField + " but that field is not defined as being stored in the index.  Add it to the index config and rebuild the index.  //Index[@Name='" + indexName + "']/Fields/Field[@Name='" + fkField + "']/@StoredInIndex='true'";
                                sb.AppendLine(msg);
                            }
                        }
                    }
                    if (!found) {
                        msg = indexName + " -> " + resolverName + ": resolution method is ForeignKey with the ForeignKeyField=" + fkField + " but that field is not in the SQL statement for the index.  Add it to the SQL then set the StoredInIndex attribute to true and rebuild the index.  //Index[@Name='" + indexName + "']/Fields/Field[@Name='" + fkField + "']/@StoredInIndex='true'";
                        sb.AppendLine(msg);
                    }


                    break;
                case ResolutionMethod.Sql:

                    if (String.IsNullOrEmpty(sql)) {
                        msg = "";
                        switch (sqlSource) {
                            case SqlSource.Unknown:
                            default:
                                msg = indexName + " -> " + resolverName + ": resolution method is Sql but the SqlSource is Uknown.  Please specify a valid value for SqlSource (such as Xml, External, or Dataview) in the following node and recreate the index.  /Indexes/Index[@Name=" + indexName + "]/@SqlSource";
                                break;
                            case SqlSource.Xml:
                                msg = indexName + " -> " + resolverName + ": resolution method is Sql and SqlSource is Xml, but no Sql is defined in the config file (gringlobal.search.config).  Set the proper sql in the following node and recreate the index.  /Indexes/Index[@Name=" + indexName + "]/Resolvers/Resolver[@Name=" + resolverName + "]/Sql";
                                break;
                            case SqlSource.Dataview:
                                if (String.IsNullOrEmpty(dataviewName)) {
                                    msg = indexName + " -> " + resolverName + ": resolution method is Sql and SqlSource is Dataview, but no DataviewName is provided.  /Indexes/Index[@Name=" + indexName + "]/Resolvers/Resolver[@Name=" + resolverName + "]/@DataviewName";
                                } else {
                                    if (!isDatabaseDown) {
                                        if (!String.IsNullOrEmpty(onlyThisIndex) || !String.IsNullOrEmpty(onlyThisResolver)) {
                                            msg = indexName + " -> " + resolverName + ": resolution method is Sql and SqlSource is Dataview and DataviewName is " + dataviewName + ", but no sql could be found for the current database engine (" + providerName + ").";
                                        }
                                    }
                                }
                                break;
                        }
                        if (!String.IsNullOrEmpty(msg)) {
                            sb.AppendLine(msg);
                        }
                    }
                    if (String.IsNullOrEmpty(pkField) || String.IsNullOrEmpty(resolvedPKField)) {
                        msg = indexName + " -> " + resolverName + ": resolution method is Sql but either QueryPrimaryKeyField or QueryResolvedPrimaryKeyField is not defined in the config file.  Set those attributes to the proper values and recreate the index.  //Index[@Name='" + indexName + "']/Resolvers/Resolver[@Name='" + resolverName + "']/@QueryPrimaryKeyField and //Index[@Name='" + indexName + "']/Resolvers/Resolver[@Name='" + resolverName + "']/@QueryResolvedPrimaryKeyField.";
                        sb.AppendLine(msg);
                    }

                    break;
                default:
                    throw new NotImplementedException(getDisplayMember("ValidateConfigurationByDataRow{default}", "Method={0} is not implemented in Resolver.ValidateConfiguration().", method.ToString()));
            }

            dr["is_valid"] = sb.Length == 0;
            dr["invalid_reason"] = sb.ToString();

            return sb.Length == 0;

        }

        internal string ValidateConfiguration(bool logIt, bool isDatabaseDown) {

            // assume it works...
            IsConfigurationValid = true;

            var sb = new StringBuilder();
            string msg;


            switch (Method) {
                case ResolutionMethod.Invalid:

                    msg = getDisplayMember("ValidateConfiguration{invalid}", "Configuration error in Resolver={0} for index={1}: 'Method' attribute must be 'Sql', 'PrimaryKey', 'ForeignKey', or 'None'.", Name , Index.Name);
                    sb.AppendLine(msg);
                    if (logIt) {
                        Index.showProgress(msg, EventLogEntryType.Error, false);
                    }
                    IsConfigurationValid = false;

                    break;
                case ResolutionMethod.None:
                    // nothing to validate?
                    break;
                case ResolutionMethod.PrimaryKey:
                    // nothing to validate?
                    if (String.IsNullOrEmpty(Index.PrimaryKeyField)) {
                        msg = getDisplayMember("ValidateConfiguration{primarykey}", "Configuration error in Resolver={0} for index={1} has its Method=PrimaryKey but no PrimaryKeyField is defined on the index.  Check configuration file at /Indexes/Index[@Name={2}]/@PrimaryKey", Name, Index.Name, Index.Name);
                        sb.AppendLine(msg);
                        if (logIt) {
                            Index.showProgress(msg, EventLogEntryType.Error, false);
                        }
                        IsConfigurationValid = false;
                    }
                    break;
                case ResolutionMethod.ForeignKey:

                    // verify foreignkeyfieldname exists
                    if (String.IsNullOrEmpty(ForeignKeyField)) {
                        msg = getDisplayMember("ValidateConfiguration{foreignkey}", "Configuration error in Resolver={0} for index={1} has its Method=ForeignKey but the Resolver/@ForeignKeyField attribute is not set.", Name, Index.Name);
                        sb.AppendLine(msg);
                        if (logIt) {
                            Index.showProgress(msg, EventLogEntryType.Error, false);
                        }
                        IsConfigurationValid = false;
                    }

                    // verify the field in from foreignkeyfieldname is stored in the index
                    bool found = false;
                    foreach (Field f in Index.IndexedFields) {
                        if (f.Name.ToLower() == ForeignKeyField.ToLower()) {
                            found = true;
                            if (!f.StoredInIndex) {
                                msg = getDisplayMember("ValidateConfiguration{notinindex}", "Configuration error in Resolver={0} for index={1} defines its resolution method as ForeignKey with the ForeignKeyField={2} but that field is not defined as being stored in the index.  Add it to the index config and rebuild the index.  //Index[@Name='{3}']/Fields/Field[@Name='{4}']/@StoredInIndex='true'", Name, Index.Name, ForeignKeyField, Index.Name, ForeignKeyField);
                                sb.AppendLine(msg);
                                if (logIt) {
                                    Index.showProgress(msg, EventLogEntryType.Error, false);
                                }
                                IsConfigurationValid = false;
                            }
                        }
                    }
                    if (!found) {
                        msg = getDisplayMember("ValidateConfiguration{notfound}", "Configuration error in Resolver={0} for index={1} defines its resolution method as ForeignKey with the ForeignKeyField={2} but that field is not in the SQL statement for the index.  Add it to the SQL then set the StoredInIndex attribute to true and rebuild the index.  //Index[@Name='{3}']/Fields/Field[@Name='{4}']/@StoredInIndex='true'", Name, Index.Name, ForeignKeyField, Index.Name, ForeignKeyField) ;
                        sb.AppendLine(msg);
                        if (logIt) {
                            Index.showProgress(msg, EventLogEntryType.Error, false);
                        }
                        IsConfigurationValid = false;
                    }


                    break;
                case ResolutionMethod.Sql:

                    if (String.IsNullOrEmpty(Sql)) {
                        msg = "";
                        switch (this.Index.SqlSource) {
                            case SqlSource.Unknown:
                            default:
                                msg = getDisplayMember("ValidateConfiguration{unknown}", "Configuration error in Resolver={0} for index={1} defines its resolution method as Sql but the SqlSource is Uknown.  Please specify a valid value for SqlSource (such as Xml, External, or Dataview) in the following node and recreate the index.  /Indexes/Index[@Name={2}]/@SqlSource", Name, Index.Name, Index.Name);
                                break;
                            case SqlSource.Xml:
                                msg = getDisplayMember("ValidateConfiguration{xml}", "Configuration error in Resolver={0} for index={1} defines its resolution method as Sql and its SqlSource as Xml, but no Sql is defined in the config file (gringlobal.search.config).  Set the proper sql in the following node and recreate the index.  /Indexes/Index[@Name={2}]/Resolvers/Resolver[@Name={3}]/Sql", Name, Index.Name, Index.Name, Name);
                                break;
                            case SqlSource.Dataview:
                                if (!isDatabaseDown) {
                                    msg = getDisplayMember("ValidateConfiguration{dataview}", "Configuration error in Resolver={0} for index={1} defines its resolution method as Sql and its SqlSource as Dataview, but no dataview could be found in the sys_dataview/sys_dataview_sql tables.  Please create a dataview named '{2}' and recreate the index.", Name, Index.Name, DataviewName);
                                }
                                break;
                        }
                        if (!String.IsNullOrEmpty(msg)) {
                            sb.AppendLine(msg);
                            if (logIt) {
                                Index.showProgress(msg, EventLogEntryType.Error, false);
                            }
                            IsConfigurationValid = false;
                        }
                    }
                    if (String.IsNullOrEmpty(PrimaryKeyField) || String.IsNullOrEmpty(ResolvedPrimaryKeyField)) {
                        msg = getDisplayMember("ValidateConfiguration{missingpkorrpk}", "Configuration error in Resolver={0} for index={1} defines its resolution method as Sql but either QueryPrimaryKeyField or QueryResolvedPrimaryKeyField is not defined in the config file.  Set those attributes to the proper values and recreate the index.  //Index[@Name='{2}']/Resolvers/Resolver[@Name='{3}']/@QueryPrimaryKeyField and //Index[@Name='{4}']/Resolvers/Resolver[@Name='{5}']/@QueryResolvedPrimaryKeyField.", Name, Index.Name, Index.Name, Name, Index.Name, Name);
                        sb.AppendLine(msg);
                        if (logIt) {
                            Index.showProgress(msg, EventLogEntryType.Error, false);
                        }
                        IsConfigurationValid = false;
                    }

                    break;
                default:
                    throw new NotImplementedException(getDisplayMember("ValidateConfiguration{default}", "Method={0} is not implemented in Resolver.ValidateConfiguration().", Method.ToString()));
            }

            return sb.ToString();

        }

        private Dictionary<string, string> _sqlStatements = new Dictionary<string, string>();

        private void appendSqlStatement(string engineName, string sql, bool setAsSql) {
            Sql = sql;
            _parameterizedSql = sql.Replace("/*_", "").Replace("_*/", "");
            _sqlStatements[engineName] = sql;
        }

        #region Initialization / Generation
        /// <summary>
        /// Initializes the files necessary for initially filling the resolver.  (Does not create the tree.  You must call CreateTree() to do that)
        /// </summary>
        private void initialize(ResolverCacheMode cacheMode) {
            this.CacheMode = cacheMode;
            deleteFiles();
        }

        /// <summary>
        /// Returns approximate number of ID's that are indexed
        /// </summary>
        /// <returns></returns>
        public int GetIDCount(bool approximate) {
            switch (this.CacheMode) {
                case ResolverCacheMode.Id:
                    return _primaryKeyTree.GetKeywordCount(approximate);
                case ResolverCacheMode.None:
                default:
                    return 0;
                case ResolverCacheMode.IdAndKeyword:
                    return _keywordTree.GetKeywordCount(approximate);
            }
        }

        private void deleteFiles() {

            initBaseFileName(true);

            if (_primaryKeyTree != null) {
                _primaryKeyTree.Dispose();
                _primaryKeyTree = null;
            }
            if (_keywordTree != null) {
                _keywordTree.Dispose();
                _keywordTree = null;
            }

            if (File.Exists(PrimaryKeyDataFileName)) {
                File.Delete(PrimaryKeyDataFileName);
            }
            if (File.Exists(QueueFileName)) {
                File.Delete(QueueFileName);
            }
            if (File.Exists(PrimaryKeyTreeFileName)) {
                File.Delete(PrimaryKeyTreeFileName);
            }


            if (File.Exists(KeywordDataFileName)) {
                File.Delete(KeywordDataFileName);
            }
            if (File.Exists(KeywordTreeFileName)) {
                File.Delete(KeywordTreeFileName);
            }

            if (!String.IsNullOrEmpty(PrimaryKeyDataFileName)) {

                string partialFileName = new FileInfo(PrimaryKeyDataFileName).Name;

                // clean up any temp files from a previous run
                string[] tempFiles = Directory.GetFiles(Directory.GetParent(PrimaryKeyDataFileName).FullName, partialFileName + ".temp_*");
                foreach (string tf in tempFiles) {
                    File.Delete(tf);
                }

                // clean up any sort files from a previous run
                string[] sortFiles = Directory.GetFiles(Directory.GetParent(PrimaryKeyDataFileName).FullName, partialFileName + ".sort_*");
                foreach (string sf in sortFiles) {
                    File.Delete(sf);
                }
            }

            if (!String.IsNullOrEmpty(KeywordDataFileName)) {
                string partialFileName = new FileInfo(KeywordDataFileName).Name;

                // clean up any temp files from a previous run
                string[] tempFiles = Directory.GetFiles(Directory.GetParent(KeywordDataFileName).FullName, partialFileName + ".temp_*");
                foreach (string tf in tempFiles) {
                    File.Delete(tf);
                }

                // clean up any sort files from a previous run
                string[] sortFiles = Directory.GetFiles(Directory.GetParent(KeywordDataFileName).FullName, partialFileName + ".sort_*");
                foreach (string sf in sortFiles) {
                    File.Delete(sf);
                }
            }
        }

        private void initBaseFileName() {
            initBaseFileName(false);
        }
        private void initBaseFileName(bool forceResolveName) {
            if (forceResolveName || String.IsNullOrEmpty(_baseFileName)) {
                //if (ByKeyword) {
                //    _baseFileName = Toolkit.ResolveFilePath(Index.FolderName + @"\" + Index.Name + ".resolver." + this.Name + ".keyword", true);
                //} else {
                _baseFileName = Toolkit.ResolveFilePath(Index.FolderName + @"\" + Index.Name + ".resolver." + this.Name, true);
                //}
            }
        }
        #region Generate by Primary Key
        private void generateByPrimaryKey(bool stubsOnly) {

            this.Index.showProgress(getDisplayMember("generateByPrimaryKey{start}", "Creating resolver {0} for index {1} via primary key...", Name, Index.Name), EventLogEntryType.Information, false);

            using (DataManager dm = DataManager.Create(this.Index.Indexer.DataConnectionSpec)) {

                string sql = _parameterizedSql.Replace(":idlist", "0").Replace(":readall", "1");
                if (stubsOnly) {
                    // NOTE: if we're creating a stub, uncomment the where clause and tweak it to always return 0 rows
                    sql = _parameterizedSql.Replace(":idlist", "0").Replace(":readall", "0");
                }

                DataCommand cmd = new DataCommand(sql, null);
                cmd.Timeout = 3600; // allow one hour (!!!) for response to start per request
                using (IDataReader idr = dm.Stream(cmd)) {

                    PrimaryKeyResolverDataProcessingArguments args = new PrimaryKeyResolverDataProcessingArguments();

                    for (int j = 0; j < idr.FieldCount; j++) {
                        if (idr.GetName(j).ToLower() == PrimaryKeyField.ToLower()) {
                            args.PrimaryKeyFieldOrdinal = j;
                        } else if (idr.GetName(j).ToLower() == ResolvedPrimaryKeyField.ToLower()) {
                            args.ResolvedPrimaryKeyFieldOrdinal = j;
                        }
                    }

                    // write all the mapping pairs to temp file
                    writeToTempFile(idr, processResolverData, args);
                }
            }

            // sort those temp files and merge into a data file
            sortAndMergeTempFiles(false, generateResolverFilesMergeCallback, null);

            // finally create the tree (which uses data from the queue file and deletes it when it's done)
            createPrimaryKeyTree();
        }

        /// <summary>
        /// Writes the int keyword and the associated location in the data file to the queue file.
        /// </summary>
        /// <param name="dataLocation"></param>
        /// <param name="primaryKeyID"></param>
        private void queueKeyword(int primaryKeyID, long dataLocation) {
            if (_queueWriter == null) {
                _queueWriter = new BinaryWriter(new FileStream(QueueFileName, FileMode.Append, FileAccess.Write));
            }
            _queueWriter.Write(primaryKeyID);
            new BPlusListLong(new long[] { dataLocation }.ToList()).Write(_queueWriter);
        }

        private void generateResolverFilesMergeCallback(ResolveIDMapping lastItemInGroup, long dataFileLocation, int groupCount, object additionalData) {
            this.queueKeyword(lastItemInGroup.PrimaryKeyID, dataFileLocation);
        }


        private List<ResolveIDMapping> processResolverData(IDataReader idr, object additionalData) {

            PrimaryKeyResolverDataProcessingArguments args = (PrimaryKeyResolverDataProcessingArguments)additionalData;

            List<ResolveIDMapping> rims = new List<ResolveIDMapping>();

            // some ids may result in a null pk id ... i.e. if the resolver sql has a left join
            // don't die in this situation -- just exclude from results
            int pkID = Toolkit.ToInt32(idr.GetValue(args.PrimaryKeyFieldOrdinal), 0);
            int otherID = Toolkit.ToInt32(idr.GetValue(args.ResolvedPrimaryKeyFieldOrdinal), 0);
            if (pkID > 0 && otherID > 0) {
                rims.Add(new ResolveIDMapping { PrimaryKeyID = pkID, ResolvedPrimaryKeyID = otherID });
            }
            return rims;
        }


        /// <summary>
        /// Creates the B+ tree and bulk inserts all data queued up (via QueueKeyword). Note it does not open it after creation.  Open() must be called in order to read from the new tree.
        /// </summary>
        private void createPrimaryKeyTree() {

            if (!File.Exists(QueueFileName)) {
                throw new InvalidOperationException(getDisplayMember("createPrimaryKeyTree{noqueuefile}", "Queue file '{0}' not found.  You must call Create() before calling createTree() as Create() queues up data used by createTree().", QueueFileName));
            }

            System.Text.Encoding encodingClass = Indexer<BPlusInt, BPlusListLong>.ParseEncoding(this.Encoding);

            if (_queueWriter != null) {
                _queueWriter.Close();
                _queueWriter = null;
            }

            // notice the BPlusTree's Create method takes encoding as an actual class,
            // but we have only a string.  So we need to map it.  It really needs to be the

            using (FileStream fs = new FileStream(QueueFileName, FileMode.Open, FileAccess.Read)) {
                using (var pkTree = BPlusTree<BPlusInt, BPlusListLong>.Create(PrimaryKeyTreeFileName, encodingClass, FanoutSize, AverageKeywordSize, NodeCacheSize, KeywordCacheSize)) {
                    pkTree.BulkInsert(fs, 1.0M);
                }
            }

            // no longer need the queue file, all its data is in the tree now
            File.Delete(QueueFileName);
        }
        private void writeToTempFile(IDataReader idr, DataFileProcessor<ResolveIDMapping>.ProcessRowCallback rowCallback, object additionalData) {
            PrimaryKeyDataFileProcessor.WriteToTempFile(idr, this.Index.MaxSortSizeInMB, PrimaryKeyDataFileName, rowCallback, additionalData);
        }
        private void sortAndMergeTempFiles(bool retainSortKeyAndSuppressGroupCountHeader, DataFileProcessor<ResolveIDMapping>.ProcessGroupSortedCallback groupCallback, object additionalData) {
            PrimaryKeyDataFileProcessor.SortAndMergeTempFiles(PrimaryKeyDataFileName, retainSortKeyAndSuppressGroupCountHeader, groupCallback, additionalData);

            // initialize the queuewriter
            if (_queueWriter != null) {
                _queueWriter.Close();
            }
            _queueWriter = new BinaryWriter(new FileStream(QueueFileName, FileMode.Append, FileAccess.Write));

        }



        #endregion Generate by Primary Key

        #region Generate by Keyword
        private void generateByKeyword(bool stubsOnly) {
            // resolve to parent/child PrimaryKeyID's as needed
            if (Method != ResolutionMethod.Sql || !String.IsNullOrEmpty(Sql)) {

                try {

                    this.Index.showProgress(getDisplayMember("generateByKeyword{start}", "Creating {0} resolver {1} for index {2} via keyword...", (stubsOnly ? " stub " : ""), Name, Index.Name), EventLogEntryType.Information, false);

                    using (DataManager dm = DataManager.Create(this.Index.Indexer.DataConnectionSpec)) {
                        // lop off the where clause, we want to pull the entire table
                        //int wherePos = r.Sql.ToLower().IndexOf("where");
                        //string sql = Toolkit.Cut(r.Sql, 0, wherePos - 1);

                        int fkFieldOrdinal = this.Index.GetFieldIndexOrdinal(ForeignKeyField);
                        int resolvedPKFieldOrdinal = -2; // -2 is flag to say we haven't looked for it yet, -1 is to say not found, otherwise is ordinal of field

                        var keywordArgs = new KeywordResolverDataProcessingArguments{ ResolvedPrimaryKeyFieldOrdinal = fkFieldOrdinal };

                        if (Method == ResolutionMethod.PrimaryKey) {
                            foreach (var leaf in this.Index.GetAllLeaves()) {
                                for (int i = 0; i < leaf.Keywords.Count; i++) {
                                    var bpll = leaf.Values[i] as BPlusListLong;

                                    keywordArgs.Keyword = leaf.Keywords[i].ToString();

                                    var ids = new List<int>();
                                    foreach (long location in bpll.Value) {
                                        ids.AddRange(this.Index.GetAllPrimaryKeyIDs(location));
                                    }

                                    // write all the mapping pairs to temp file
                                    writeKeywordToTempFile(ids, processKeywordResolverData, keywordArgs);
                                }
                            }
                        } else if (Method == ResolutionMethod.ForeignKey) {
                            foreach (var leaf in this.Index.GetAllLeaves()) {
                                for (int i = 0; i < leaf.Keywords.Count; i++) {
                                    var bpll = leaf.Values[i] as BPlusListLong;

                                    keywordArgs.Keyword = leaf.Keywords[i].ToString();

                                        var hits = new List<Hit>();
                                        foreach (long location in bpll.Value) {
                                            hits.AddRange(this.Index.GetAllHits(location));
                                        }

                                        // write all the mapping pairs to temp file
                                        writeKeywordToTempFile(hits, processKeywordResolverData, keywordArgs);
                                }
                            }

                        } else if (Method == ResolutionMethod.Sql) {
                            foreach (var leaf in this.Index.GetAllLeaves()) {
                                for (int i = 0; i < leaf.Keywords.Count; i++) {
                                    var bpll = leaf.Values[i] as BPlusListLong;

                                    keywordArgs.Keyword = leaf.Keywords[i].ToString();

                                    var ids = new List<int>();
                                    foreach (long location in bpll.Value) {
                                        ids.AddRange(this.Index.GetAllPrimaryKeyIDs(location));
                                    }


                                    // ids now contains all pk ids in this index for the current keyword.

                                    // enable the where clause by uncommenting it
                                    string sql = _parameterizedSql.Replace(":readall", (stubsOnly ? "0" : "1"));


                                    var allIdsList = ids.ToList();
                                    var limit = 1000;

                                    // we do this in chunks of 1,000 so the db sql command length isn't exceeded by a bazillion id's in the :idlist section
                                    for (int j = 0; j < allIdsList.Count; j += limit) {
                                        List<int> idList = null;
                                        if (j + limit < allIdsList.Count) {
                                            idList = allIdsList.GetRange(j, limit);
                                        } else {
                                            idList = allIdsList.GetRange(j, allIdsList.Count - j);
                                        }

                                        DataCommand cmd = new DataCommand(sql, new DataParameters(":idlist", idList, DbPseudoType.IntegerCollection));
                                        // allow one hour (!!!) for response to start per request 
                                        // (i.e. one hour for db engine to create any temp tables, copy data over, then start returning results)
                                        cmd.Timeout = 3600;

                                        using (IDataReader idr = dm.Stream(cmd)) {
                                            if (keywordArgs.ResolvedPrimaryKeyFieldOrdinal < 0) {
                                                if (resolvedPKFieldOrdinal < -1) {
                                                    for (int k = 0; k < idr.FieldCount; k++) {
                                                        if (idr.GetName(k).ToLower() == ResolvedPrimaryKeyField.ToLower()) {
                                                            resolvedPKFieldOrdinal = k;
                                                            break;
                                                        }
                                                    }
                                                }
                                                keywordArgs.ResolvedPrimaryKeyFieldOrdinal = resolvedPKFieldOrdinal;
                                            }


                                            // write all the mapping pairs to temp file
                                            writeKeywordToTempFile(idr, processKeywordResolverData, keywordArgs);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // sort those temp files and merge into a data file
                    sortAndMergeKeywordTempFiles(false, generateKeywordResolverFilesMergeCallback, null);

                    // finally create the tree (which uses data from the queue file and deletes it when it's done)
                    createKeywordTree();

                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Writes the string keyword and the associated location in the data file to the queue file.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="dataLocation"></param>
        private void queueKeyword(string keyword, long dataLocation) {
            if (_queueWriter == null) {
                _queueWriter = new BinaryWriter(new FileStream(QueueFileName, FileMode.Append, FileAccess.Write));
            }
            _queueWriter.Write(keyword);
            new BPlusListLong(new long[] { dataLocation }.ToList()).Write(_queueWriter);
        }



        private void generateKeywordResolverFilesMergeCallback(ResolveKeywordMapping lastItemInGroup, long dataFileLocation, int groupCount, object additionalData) {
            this.queueKeyword(lastItemInGroup.Keyword, dataFileLocation);
        }

        private List<ResolveKeywordMapping> processKeywordResolverData(int primaryKeyID, object additionalData) {

            KeywordResolverDataProcessingArguments args = (KeywordResolverDataProcessingArguments)additionalData;

            List<ResolveKeywordMapping> rkms = new List<ResolveKeywordMapping>();

            // some ids may result in a null pk id ... i.e. if the resolver sql has a left join
            // don't die in this situation -- just exclude from results
            rkms.Add(new ResolveKeywordMapping { Keyword = args.Keyword, ResolvedPrimaryKeyID = primaryKeyID });
            return rkms;
        }

        private List<ResolveKeywordMapping> processKeywordResolverData(Hit hit, object additionalData) {

            KeywordResolverDataProcessingArguments args = (KeywordResolverDataProcessingArguments)additionalData;

            List<ResolveKeywordMapping> rkms = new List<ResolveKeywordMapping>();

            // some ids may result in a null pk id ... i.e. if the resolver sql has a left join
            // don't die in this situation -- just exclude from results
            rkms.Add(new ResolveKeywordMapping { Keyword = args.Keyword, ResolvedPrimaryKeyID = hit.Fields[args.ResolvedPrimaryKeyFieldOrdinal].Value });
            return rkms;
        }

        private List<ResolveKeywordMapping> processKeywordResolverData(IDataReader idr, object additionalData) {

            KeywordResolverDataProcessingArguments args = (KeywordResolverDataProcessingArguments)additionalData;

            List<ResolveKeywordMapping> rkms = new List<ResolveKeywordMapping>();

            // some ids may result in a null pk id ... i.e. if the resolver sql has a left join
            // don't die in this situation -- just exclude from results
            int otherID = Toolkit.ToInt32(idr.GetValue(args.ResolvedPrimaryKeyFieldOrdinal), 0);
            rkms.Add(new ResolveKeywordMapping { Keyword = args.Keyword, ResolvedPrimaryKeyID = otherID });
            return rkms;
        }

        /// <summary>
        /// Creates the B+ tree and bulk inserts all data queued up (via QueueKeyword). Note it does not open it after creation.  Open() must be called in order to read from the new tree.
        /// </summary>
        private void createKeywordTree() {

            if (!File.Exists(QueueFileName)) {
                throw new InvalidOperationException(getDisplayMember("createKeywordTree{noqueuefile}", "Queue file '{0}' not found.  You must call Create() with CacheMode=ResolverCacheMode.IdAndKeyword before calling createKeywordTree() as Create() queues up data used by createKeywordTree().", QueueFileName));
            }

            if (_queueWriter != null) {
                _queueWriter.Close();
                _queueWriter = null;
            }

            System.Text.Encoding encodingClass = Indexer<BPlusString, BPlusListLong>.ParseEncoding(this.Encoding);

            // notice the BPlusTree's Create method takes encoding as an actual class,
            // but we have only a string.  So we need to map it.  It really needs to be the

            using (FileStream fs = new FileStream(QueueFileName, FileMode.Open, FileAccess.Read)) {
                using (var tree = BPlusTree<BPlusString, BPlusListLong>.Create(KeywordTreeFileName, encodingClass, FanoutSize, AverageKeywordSize, NodeCacheSize, KeywordCacheSize)) {
                    tree.BulkInsert(fs, 1.0M);
                }
            }

            // no longer need the queue file, all its data is in the tree now
            File.Delete(QueueFileName);
        }

        private void writeKeywordToTempFile(List<int> ids, DataFileProcessor<ResolveKeywordMapping>.ProcessIDCallback idCallback, object additionalData) {
            KeywordDataFileProcessor.WriteToTempFile(ids, this.Index.MaxSortSizeInMB, KeywordDataFileName, idCallback, additionalData);
        }
        private void writeKeywordToTempFile(List<Hit> hits, DataFileProcessor<ResolveKeywordMapping>.ProcessHitCallback hitCallback, object additionalData) {
            KeywordDataFileProcessor.WriteToTempFile(hits, this.Index.MaxSortSizeInMB, KeywordDataFileName, hitCallback, additionalData);
        }
        private void writeKeywordToTempFile(IDataReader idr, DataFileProcessor<ResolveKeywordMapping>.ProcessRowCallback rowCallback, object additionalData) {
            KeywordDataFileProcessor.WriteToTempFile(idr, this.Index.MaxSortSizeInMB, KeywordDataFileName, rowCallback, additionalData);
        }
        private void sortAndMergeKeywordTempFiles(bool retainSortKeyAndSuppressGroupCountHeader, DataFileProcessor<ResolveKeywordMapping>.ProcessGroupSortedCallback groupCallback, object additionalData) {
            KeywordDataFileProcessor.SortAndMergeTempFiles(KeywordDataFileName, retainSortKeyAndSuppressGroupCountHeader, groupCallback, additionalData);

            // initialize the queuewriter
            if (_queueWriter != null) {
                _queueWriter.Close();
            }
            _queueWriter = new BinaryWriter(new FileStream(QueueFileName, FileMode.Append, FileAccess.Write));

        }



        #endregion Generate by Keyword

        internal void Create(bool stubsOnly) {

            // this is usually called right after everything is queued up.
            // this means the streams may not have been written out yet and are still open -- but in write mode only.
            // if they are, close them so we can open them for reading.
            cleanupStreams();

            if (CacheMode == ResolverCacheMode.None) {
                return;
            }


            //if (!File.Exists(QueueFileName)) {
            //    throw new InvalidOperationException("Queue file '" + QueueFileName + "' not found.  You must call Create() before calling CreateTree() as Create() queues up data used by CreateTree().");
            //}

            if (Method == ResolutionMethod.ForeignKey) {
                if (String.IsNullOrEmpty(ForeignKeyField) || ForeignKeyField.Trim() == String.Empty) {
                    throw new InvalidOperationException(getDisplayMember("Create{nofkfield}", "Resolver '{0} has its Method=ForeignKey but ForeignKeyField is empty in //Index[@Name={1}]/Resolvers/Resolver[@Name={2}]/@ForeignKeyField", Name, Name, Name));
                }
            } 
            
            if (Method == ResolutionMethod.Sql) {
                if (String.IsNullOrEmpty(Sql) || Sql.Trim() == String.Empty) {
                    switch (this.Index.SqlSource) {
                        case SqlSource.Unknown:
                        default:
                            throw new InvalidOperationException(getDisplayMember("Create{unknownsqlsource}", "Resolver '{0}' has its Method=Sql but its SqlSource=Unknown in the config file (gringlobal.search.config)", Name));
                        case SqlSource.Xml:
                            throw new InvalidOperationException(getDisplayMember("Create{xmlsqlsource}", "Resolver '{0}' has its Method=Sql and SqlSource=Xml but no Sql statement exists in the config file (gringlobal.search.config) at node //Index[@Name={1}]/Resolvers/Resolver[@Name={2}]/Sql[@engine={3}]", Name, this.Index.Name, Name, this.Index.Indexer.DataConnectionSpec.ProviderName));
                        //case SqlSource.EngineXml:
                        //    throw new InvalidOperationException("Resolver '" + Name + "' has its Method=Sql and SqlSource=EngineXml but no Sql statement exists in the database engine-specific config file (" + this.Index.DataConnectionSpec.ProviderName + "_queries.xml at node //queries/" + this.Index.Name + "/" + Name);
                        case SqlSource.Dataview:
                            throw new InvalidOperationException(getDisplayMember("Create{dataviewsqlsource}", "Resolver '{0}' has its Method=Sql and SqlSource=Dataview but no Sql statement exists in the sys_dataview table named '{1}'.", Name, DataviewName));
                    }
                } 
            }


            // prepare the resolver to be filled
            deleteFiles();

            // resolve to parent/child PrimaryKeyID's as needed
            if (Method == ResolutionMethod.Sql) {
                // resolve via primary key ID's of current index to parent / child primary key ID's
                // (only need to do this for pk tree if it's resolved via Sql)
                generateByPrimaryKey(stubsOnly);
            }

            if (CacheMode == ResolverCacheMode.IdAndKeyword) {
                // resolve via string keyword in current index to parent / child primary key id's
                // (need to do this for keyword tree regardless of how its resolved)
                generateByKeyword(stubsOnly);
            }
        }

        public void CreateEmptyFilesIfNeeded() {

            if (CacheMode == ResolverCacheMode.None) {
                // nothing to do
                return;
            } else {
                if (Method == ResolutionMethod.Sql) {
                    if (!File.Exists(PrimaryKeyTreeFileName)) {
                        System.Text.Encoding encodingClass = Indexer<BPlusInt, BPlusListLong>.ParseEncoding(this.Encoding);
                        using (var pkt = BPlusTree<BPlusInt, BPlusListLong>.Create(PrimaryKeyTreeFileName, encodingClass, FanoutSize, AverageKeywordSize, NodeCacheSize, KeywordCacheSize)) {
                            // nothing to do
                        }
                    }
                    if (!File.Exists(PrimaryKeyDataFileName)) {
                        File.Create(PrimaryKeyDataFileName).Close();
                    }
                }

                if (CacheMode == ResolverCacheMode.IdAndKeyword) {
                    if (!File.Exists(KeywordTreeFileName)) {
                        System.Text.Encoding encodingClass = Indexer<BPlusInt, BPlusListLong>.ParseEncoding(this.Encoding);
                        using (var kt = BPlusTree<BPlusString, BPlusListLong>.Create(KeywordTreeFileName, encodingClass, FanoutSize, AverageKeywordSize, NodeCacheSize, KeywordCacheSize)) {
                            // nothing to do
                        }

                    }
                    if (!File.Exists(KeywordDataFileName)) {
                        File.Create(KeywordDataFileName).Close();
                    }
                }

            }

        }

        #endregion Initialization / Generation


        #region Active Use
        /// <summary>
        /// Opens the files used by Resolver for reading.  (.index and .data files)  Requires the resolver to be fully created first.
        /// </summary>
        internal void Open() {

            initBaseFileName(true);

            if (Enabled) {

                if (CacheMode == ResolverCacheMode.None) {
                    // nothing to do, no cache
                } else {

                    // we do this for both Id and IdAndKeyword resolvers...
                    if (Method == ResolutionMethod.Sql) {
                        // pk resolver file is created only if the method is SQL.
                        // otherwise the needed data is stored in the index itself (pkid or fkid)
                        if (File.Exists(PrimaryKeyTreeFileName)) {
                            _primaryKeyTree = BPlusTree<BPlusInt, BPlusListLong>.Load(PrimaryKeyTreeFileName, false, this.NodeCacheSize, this.KeywordCacheSize);
                            _primaryKeyDataFileStream = new FileStream(PrimaryKeyDataFileName, FileMode.Open, FileAccess.ReadWrite);
                            _primaryKeyDataReader = new BinaryReader(_primaryKeyDataFileStream);
                            _primaryKeyDataWriter = new BinaryWriter(_primaryKeyDataFileStream);
                        }
                        if (_primaryKeyTree == null) {
                            throw new InvalidOperationException(getDisplayMember("Open{nopktree}", "This resolver has not yet been created, as its index file does not exist -> '{0}'.", PrimaryKeyTreeFileName));
                        }
                    }


                    // we do this only for IdAndKeyword resolvers...
                    if (CacheMode == ResolverCacheMode.IdAndKeyword) {
                        // keyword resolver files are always created regardless of method
                        // IF EnableByKeyword is set
                        if (File.Exists(KeywordTreeFileName)) {
                            _keywordTree = BPlusTree<BPlusString, BPlusListLong>.Load(KeywordTreeFileName, false, this.NodeCacheSize, this.KeywordCacheSize);
                            _keywordDataFileStream = new FileStream(KeywordDataFileName, FileMode.Open, FileAccess.ReadWrite);
                            _keywordDataReader = new BinaryReader(_keywordDataFileStream);
                            _keywordDataWriter = new BinaryWriter(_keywordDataFileStream);
                        }
                        if (_keywordTree == null) {
                            throw new InvalidOperationException(getDisplayMember("Open{nokeywordtree}", "This resolver has not yet been created, as its index file does not exist -> '{0}'.", KeywordTreeFileName));
                        }
                    }

                }


                //if (_tree == null && _keywordTree == null) {
                //    throw new InvalidOperationException("This resolver has not yet been created, as neither of its index files exist -> '" + TreeFileName + "', '" + KeywordTreeFileName + "'.");
                //}

            }

        }

        internal List<int> GetResolvedIDsFromDatabase(IEnumerable<int> pkIDs, SearchOptions opts) {
            var ret = new List<int>();
            var i = 1;
            var list = pkIDs.Take(1000);
            using (var dm = DataManager.Create(this.Index.Indexer.DataConnectionSpec)) {
                while (list != null && list.FirstOrDefault() > 0) {
                    var cmd = createDataCommand(list.ToList());
                    if (cmd == null) {
                        // Bad setup -- no sql command found, but we would loop until all in list have been processed.
                        // just jump out to make sure we don't spin indefinitely
                        break;
                    } else {
                        using (var idr = dm.Stream(cmd)) {
                            // offset should always be 1, but we look it up just in case
                            var offset = idr.GetOrdinal(this.ResolvedPrimaryKeyField);
                            while (idr.Read()) {
                                ret.Add(idr.GetInt32(offset));
                            }
                            list = pkIDs.Skip(i * 1000).Take(1000);
                            i++;
                        }
                    }
                }
            }
            return ret;
        }

        internal List<int> GetResolvedIDsViaIdAndKeyword(string keyword, KeywordMatchMode matchMode, bool ignoreCase, SearchOptions opts) {
            // this overload bypasses hit data and maps a keyword (say "wheat") directly to its resolved ids (say accession_id)
            //if (Method != ResolutionMethod.Sql) {
            //    throw new InvalidOperationException("Should never call Resolver.GetResolvedIDs() when resolution Method is configured as '" + Method.ToString() + "'.  Check Index.GetResolverIDs() method.");
            //}

            // we have enabled the cache.
            var ret = new List<int>();
            if (Enabled) {




                if (_keywordTree == null || _keywordDataReader == null) {
                    throw new InvalidOperationException(getDisplayMember("GetResolvedIDsViaIdAndKeyword{nokeywordtree}", "Resolving ID's for {0} to {1} is configured improperly.  The flag 'Enabled' is true, but the cache files do not exist or are not initialized properly.  Either set Enabled to false in the Index config file, or re-run the index creation code to generate and initialize these files.", Index.Name, Name));
                }

                // look up all the keywords associated with this primary key id
                var kms = _keywordTree.Search(keyword, matchMode, ignoreCase);
                foreach (var km in kms) {

                    // prevent others from messing with the data's file pointer while we get the data we need
                    lock (this.lockForReading) {

                        switch (DataLocation) {
                            case DataLocation.InLeaf:

                                // resolved ids are stored  in the leaf itself.
                                // no need to look in external storage file.
                                foreach (int id in km.Value.Value) {
                                    ret.Add(id);
                                }

                                break;
                            case DataLocation.InSeparateFile:

                                // each keywordmatch contains a BPlusListLong object as its Value.
                                // each BPLusListLong object contains a List<long> as its Value.
                                // that's why we need to say km.Value.Value here.
                                foreach (long loc in km.Value.Value) {
                                    // add all associated resolved ids from the data file from the data offset given by the keyword in the tree
                                    foreach (var rkm in KeywordDataFileProcessor.GetGroupOfDataItems(_keywordDataReader, loc)) {
                                        // if a key resolves down to null, it'll show up as zero.  don't add it.
                                        if (rkm.ResolvedPrimaryKeyID > 0) {
                                            ret.Add(rkm.ResolvedPrimaryKeyID);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            return ret;
        }

        //internal List<int> GetResolvedIDs(int primaryKeyID, SearchOptions opts) {
        //    return GetResolvedIDs(new List<int>(new int[] { primaryKeyID }), opts);
        //}

        internal List<int> GetResolvedIDsViaSql(List<int> primaryKeyIDs, SearchOptions opts) {
            // this overload relies on the resolver that is keyed by pkID instead of a string keyword.
            List<int> values = new List<int>();

            if (this.Enabled) {

                if (Method != ResolutionMethod.Sql) {
                    throw new InvalidOperationException(getDisplayMember("GetResolvedIDsViaSql{nonsqlmethod}", "Should never call Resolver.GetResolvedIDs() when resolution Method is configured as '{0}'.  Check Index.GetResolverIDs() method.", Method.ToString()));
                }

                if (this.CacheMode == ResolverCacheMode.None || (opts.PassThruLevel & DatabasePassthruLevel.Resolver) == DatabasePassthruLevel.Resolver) {

                    // ignore resolver files, go directly to 
                     values = GetResolvedIDsFromDatabase(primaryKeyIDs, opts);

                } else {

                    // we should be using the cache files...
                    if (_primaryKeyTree == null || _primaryKeyDataReader == null) {
                        throw new InvalidOperationException(getDisplayMember("GetResolvedIDsViaSql{pktree}", "Resolving ID's for {0} to {1} is configured improperly.  The flag 'Enabled' is true, but the cache files do not exist or are not initialized properly.  Either set Enabled to false in the Index config file, or re-run the index creation code to generate and initialize these files.", Index.Name, Name));
                    }

                    foreach (int pkID in primaryKeyIDs) {
                        // look up all the keywords associated with this primary key id
                        BPlusInt key = new BPlusInt(pkID);
                        var kms = _primaryKeyTree.Search(key, KeywordMatchMode.ExactMatch, false);

                        foreach (var km in kms) {

                            // prevent others from messing with the data's file pointer while we get the data we need
                            lock (this.lockForReading) {

                                switch (DataLocation) {
                                    case DataLocation.InLeaf:

                                        // resolved ids are stored  in the leaf itself.
                                        // no need to look in external storage file.
                                        foreach (int id in km.Value.Value) {
                                            values.Add(id);
                                        }


                                        break;
                                    case DataLocation.InSeparateFile:

                                        // each keywordmatch contains a BPlusListLong object as its Value.
                                        // each BPLusListLong object contains a List<long> as its Value.
                                        // that's why we need to say km.Value.Value here.
                                        foreach (long loc in km.Value.Value) {
                                            // add all associated resolved ids from the data file from the data offset given by the keyword in the tree
                                            foreach (ResolveIDMapping rim in PrimaryKeyDataFileProcessor.GetGroupOfDataItems(_primaryKeyDataReader, loc)) {
                                                // if a key resolves down to null, it'll show up as zero.  don't add it.
                                                if (rim.ResolvedPrimaryKeyID > 0) {
                                                    values.Add(rim.ResolvedPrimaryKeyID);
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return values;
        }


        internal DataCommand createDataCommand(int pkID) {
            return createDataCommand(new List<int>(new int[] { pkID }));
        }

        internal DataCommand createDataCommand(List<int> pkIDs) {
            if (String.IsNullOrEmpty(this.Sql)) {
                return null;
            } else {
                var cmd = new DataCommand(_parameterizedSql);
                cmd.DataParameters.Add(new DataParameter(":idlist", pkIDs, DbPseudoType.IntegerCollection));
                cmd.DataParameters.Add(new DataParameter(":readall", 0, DbType.Int32));
                return cmd;
            }
        }

        public void InsertIntoTree(string keyword, BPlusListLong newValue) {
            _keywordTree.Insert(keyword, newValue);
        }
        public void InsertIntoTree(BPlusInt primaryKey, BPlusListLong newValue) {
            _primaryKeyTree.Insert(primaryKey, newValue);
        }

        public void UpdateTree(KeywordMatch<BPlusInt, BPlusListLong> match, BPlusListLong newValue, UpdateMode updateMode) {
            match.Value.Update(newValue, updateMode);
            match.Node.Write();
        }

        public List<KeywordMatch<BPlusString, BPlusListLong>> UpdateTree(string keyword, KeywordMatchMode matchMode, BPlusListLong newValue, UpdateMode updateMode) {
            return _keywordTree.Update(keyword, matchMode, newValue, updateMode);
        }
        public List<KeywordMatch<BPlusInt, BPlusListLong>> UpdateTree(BPlusInt primaryKey, KeywordMatchMode matchMode, BPlusListLong newValue, UpdateMode updateMode) {
            return _primaryKeyTree.Update(primaryKey, matchMode, newValue, updateMode);
        }


        private BPlusListLong appendNewResolvedID(int resolvedPrimaryKeyID, BinaryWriter writer) {
            // first, create a new hit (i.e. resolved id)
            long ret = 0;
            lock (this.lockForWriting) {
                ret = writer.BaseStream.Position = writer.BaseStream.Length;
                // write a length of 1
                writer.Write((int)1);
                // write the new resolved id
                writer.Write(resolvedPrimaryKeyID);
                writer.Flush();
            }
            return new BPlusListLong(ret);


        }

        private BPlusListLong appendNewResolvedIDs(List<int> resolvedPrimaryKeyIDs, BinaryWriter writer) {
            // first, create a new hit (i.e. resolved id)
            long ret = 0;
            lock (this.lockForWriting) {
                ret = writer.BaseStream.Position = writer.BaseStream.Length;
                // write a length of 1
                writer.Write(resolvedPrimaryKeyIDs.Count);
                // write the new resolved id
                foreach (var id in resolvedPrimaryKeyIDs) {
                    writer.Write(id);
                }
                writer.Flush();
            }
            return new BPlusListLong(ret);

        }

        public void AddResolvedIDs(string keyword, List<int> resolvedPrimaryKeyIDs) {


            if (CacheMode == ResolverCacheMode.None) {
                return;
            } else {

                BPlusListLong newResolvedEntry = this.appendNewResolvedIDs(resolvedPrimaryKeyIDs, _keywordDataWriter);

                var matches = _keywordTree.Search(keyword, KeywordMatchMode.ExactMatch, false);
                if (matches.Count == 0) {
                    _keywordTree.Insert(keyword, newResolvedEntry);
                } else {
                    _keywordTree.Update(keyword, KeywordMatchMode.ExactMatch, newResolvedEntry, UpdateMode.Add);
                }
            }
        }

        public void AddResolvedIDs(int primaryKeyID, List<int> resolvedPrimaryKeyIDs) {


            if (CacheMode == ResolverCacheMode.None) {
                return;
            } else {

                BPlusListLong newResolvedEntry = this.appendNewResolvedIDs(resolvedPrimaryKeyIDs, _primaryKeyDataWriter);

                var bpi = new BPlusInt(primaryKeyID);
                var matches = _primaryKeyTree.Search(bpi, KeywordMatchMode.ExactMatch, false);

                if (matches.Count == 0) {
                    _primaryKeyTree.Insert(bpi, newResolvedEntry);
                } else {
                    _primaryKeyTree.Update(bpi, KeywordMatchMode.ExactMatch, newResolvedEntry, UpdateMode.Add);
                }
            }
        }


        public void AddResolvedID(int primaryKeyID, int resolvedPrimaryKeyID) {

            if (CacheMode == ResolverCacheMode.None) {
                return;
            } else {

                BPlusListLong newResolvedEntry = this.appendNewResolvedID(resolvedPrimaryKeyID, _primaryKeyDataWriter);

                var bpi = new BPlusInt(primaryKeyID);
                var matches = _primaryKeyTree.Search(bpi, KeywordMatchMode.ExactMatch, false);

                if (matches.Count == 0) {
                    _primaryKeyTree.Insert(bpi, newResolvedEntry);
                } else {
                    _primaryKeyTree.Update(bpi, KeywordMatchMode.ExactMatch, newResolvedEntry, UpdateMode.Add);
                }
            }
        }

        public void RemoveResolvedID(int primaryKeyID, int resolvedPrimaryKeyID) {

            if (CacheMode == ResolverCacheMode.None) {
                return;
            } else {

                var list = new List<int>();
                list.Add(resolvedPrimaryKeyID);
                RemoveResolvedIDs(primaryKeyID, list);
            }
        }

        public void RemoveResolvedIDs(int primaryKeyID, List<int> resolvedPrimaryKeyIDList) {


            if (CacheMode == ResolverCacheMode.None) {
                return;
            } else {

                var bpi = new BPlusInt(primaryKeyID);
                var matches = _primaryKeyTree.Search(bpi, KeywordMatchMode.ExactMatch, false);

                if (matches.Count == 0) {
                    // nothing to remove
                } else {
                    // TODO: lookup all data points that match, read in values, zero out those that match given resolvedPrimaryKeyID.
                    foreach (KeywordMatch<BPlusInt, BPlusListLong> match in matches) {
                        lock (this.lockForReading) {
                            foreach (long dataPos in match.Value.Value) {
                                _primaryKeyDataReader.BaseStream.Position = dataPos;
                                int length = _primaryKeyDataReader.ReadInt32();
                                for (int i = 0; i < length; i++) {
                                    var id = _primaryKeyDataReader.ReadInt32();
                                    if (resolvedPrimaryKeyIDList.Contains(id)) {
                                        lock (this.lockForWriting) {
                                            _primaryKeyDataWriter.BaseStream.Position = _primaryKeyDataReader.BaseStream.Position - sizeof(int);
                                            _primaryKeyDataWriter.Write((int)0);
                                            _primaryKeyDataWriter.Flush();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void RemoveResolvedIDs(string keyword, List<int> resolvedPrimaryKeyIDList) {


            if (CacheMode == ResolverCacheMode.None) {
                return;
            } else {

                var matches = _keywordTree.Search(keyword, KeywordMatchMode.ExactMatch, false);
                if (matches.Count == 0) {
                    // nothing to remove
                } else {
                    foreach (KeywordMatch<BPlusString, BPlusListLong> match in matches) {
                        lock (this.lockForReading) {
                            foreach (long dataPos in match.Value.Value) {
                                _keywordDataReader.BaseStream.Position = dataPos;
                                int length = _keywordDataReader.ReadInt32();
                                for (int i = 0; i < length; i++) {
                                    var id = _keywordDataReader.ReadInt32();
                                    if (resolvedPrimaryKeyIDList.Contains(id)) {
                                        lock (this.lockForWriting) {
                                            _keywordDataWriter.BaseStream.Position = _keywordDataReader.BaseStream.Position - sizeof(int);
                                            _keywordDataWriter.Write((int)0);
                                            _keywordDataWriter.Flush();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To be used only from the Indexer.writeDefinitionFile() method!!!  All other persistence is to be done via the static SaveSettings() method.
        /// </summary>
        /// <returns></returns>
        internal Node ToXmlNode() {
            Node nd = new Node("Resolver");
            nd.Attributes.SetValue("Name", Name);
            nd.Attributes.SetValue("FanoutSize", FanoutSize.ToString());
            nd.Attributes.SetValue("Encoding", Encoding);
            nd.Attributes.SetValue("AverageKeywordSize", AverageKeywordSize.ToString());
            nd.Attributes.SetValue("NodeCacheSize", NodeCacheSize.ToString());
            nd.Attributes.SetValue("KeywordCacheSize", KeywordCacheSize.ToString());
            nd.Attributes.SetValue("Enabled", Enabled.ToString());
            nd.Attributes.SetValue("AllowRealTimeUpdates", AllowRealTimeUpdates.ToString());
            nd.Attributes.SetValue("CacheMode", CacheMode.ToString());
            //            nd.Attributes.SetValue("EnableByKeyword", EnableByKeyword.ToString());
            nd.Attributes.SetValue("Method", Method.ToString());
            nd.Attributes.SetValue("ForeignKeyField", ForeignKeyField);
            nd.Attributes.SetValue("DataLocation", DataLocation.ToString());

            nd.Attributes.SetValue("QueryPrimaryKeyField", PrimaryKeyField);
            nd.Attributes.SetValue("QueryResolvedPrimaryKeyField", ResolvedPrimaryKeyField);
            nd.Attributes.SetValue("DataviewName", DataviewName);

            return nd;
        }



        internal void SaveSqlStatements(string folderPath) {

            switch (this.Index.SqlSource) {
                case SqlSource.Unknown:
                default:
                    // nothing to do
                    break;
                case SqlSource.Xml:
                    // already handled by ToXmlNode()
                    break;
                //case SqlSource.EngineXml:
                //    saveSqlToEngineXml("mysql", folderPath);
                //    saveSqlToEngineXml("oracle", folderPath);
                //    saveSqlToEngineXml("postgresql", folderPath);
                //    saveSqlToEngineXml("sqlserver", folderPath);
                //    break;
                case SqlSource.Dataview:
                    //this.Index.saveSqlToDataview("search_" + this.Index.Name.ToLower() + "__" + this.Name.ToLower(), _sqlStatements);
                    Index<BPlusInt, BPlusListLong>.saveSqlToDataview(this.DataviewName, this.Index.Indexer.DataConnectionSpec, _sqlStatements);
                    break;
            }

        }

        private void saveSqlToEngineXml(string engine, string folderPath){

            var path = Toolkit.ResolveFilePath(folderPath + @"\" + engine + "_queries.xml", false);
            if (!File.Exists(path)) {
                // xml file doesn't exist, create it.
                // should never ever get here... but anyway...
                File.WriteAllText(path, String.Format(@"<?xml version='1.0' ?>
<queries>
    <{0}>
        <{1}>
            {2}
        </{1}>
    </{0}>
</queries>", Index.Name.ToLower(), Name.ToLower(), _sqlStatements[engine]));

            } else {

                string engineSql = null;
                if (_sqlStatements.TryGetValue(engine, out engineSql)) {
                    var doc = new XmlDocument();
                    doc.Load(path);

                    // make sure parent node exists, create it if it doesn't
                    var parent = doc.SelectSingleNode("/queries/" + Index.Name.ToLower());
                    if (parent == null) {
                        parent = doc.CreateElement(Name.ToLower());
                        doc.DocumentElement.AppendChild(parent);
                    }

                    // append the sql node if needed
                    var nd = doc.SelectSingleNode("/queries/" + Index.Name.ToLower() + "/" + Name.ToLower());
                    if (nd == null) {
                        nd = doc.CreateElement(Name.ToLower());
                        parent.AppendChild(nd);
                    }
                    nd.InnerText = engineSql;
                    doc.Save(path);
                }

            }
        }

        internal void SaveEnabledToConfig() {
            // write the new enabled setting to the config file
            var doc = new XmlDocument();
            doc.Load(Index.Indexer.ConfigFilePath);
            var ndIdx = doc.SelectSingleNode("/Indexes/Index[@Name='" + Index.Name + "']/Resolvers/Resolver[@Name='" + this.Name + "']");
            if (ndIdx != null) {
                var val = Toolkit.GetAttValue(ndIdx, "Enabled", null);
                if (val == null) {
                    var att = doc.CreateAttribute("Enabled");
                    att.Value = this.Enabled.ToString();
                    ndIdx.Attributes.Append(att);
                } else {
                    ndIdx.Attributes["Enabled", ""].Value = this.Enabled.ToString();
                }
                doc.Save(Index.Indexer.ConfigFilePath);
            }
        }

        internal void SaveConfig(DataSet ds, DataRow dr) {

            var doc = new XmlDocument();
            doc.Load(this.Index.Indexer.ConfigFilePath);
            var nd = doc.SelectSingleNode("/Indexes/Index[@Name='" + dr["index_name"] + "']/Resolvers/Resolver[@Name='" + dr["resolver_name"] + "']");
            if (nd == null) {
                nd = doc.CreateElement("Resolver");
                Toolkit.SetAttValue(nd, "Name", dr["resolver_name"].ToString());
                var ndIdx = doc.SelectSingleNode("/Indexes/Index[@Name='" + dr["index_name"].ToString() + "']");
                if (ndIdx == null) {
                    throw new InvalidOperationException(getDisplayMember("SaveConfig{resolvernode}", "No Index node found in config file for index {0}, can't save resolver {1}", dr["index_name"].ToString(), dr["resolver_name"].ToString()));
                } else {
                    var ndResolvers = ndIdx.SelectSingleNode("Resolvers");
                    if (ndResolvers == null) {
                        ndResolvers = doc.CreateElement("Resolvers");
                        ndIdx.AppendChild(ndResolvers);
                    }

                    ndResolvers.AppendChild(nd);
                }
            }

            // nd is in the document now, empty it out and refill...
            nd.RemoveAll();
            Toolkit.SetAttValue(nd, "Name", dr["resolver_name"].ToString());
            Toolkit.SetAttValue(nd, "AverageKeywordSize", dr["average_keyword_size"].ToString());
            Toolkit.SetAttValue(nd, "Encoding", dr["encoding"].ToString());
            Toolkit.SetAttValue(nd, "FanoutSize", dr["fanout_size"].ToString());
            Toolkit.SetAttValue(nd, "ForeignKeyField", dr["foreign_key_field"].ToString());
            Toolkit.SetAttValue(nd, "KeywordCacheSize", dr["keyword_cache_size"].ToString());
            Toolkit.SetAttValue(nd, "NodeCacheSize", dr["node_cache_size"].ToString());
            //Toolkit.SetAttValue(nd, "SqlFor-mysql", dr["mysql_sql_statement"].ToString());
            //Toolkit.SetAttValue(nd, "SqlFor-sqlserver", dr["sqlserver_sql_statement"].ToString());
            //Toolkit.SetAttValue(nd, "SqlFor-oracle", dr["oracle_sql_statement"].ToString());
            //Toolkit.SetAttValue(nd, "SqlFor-postgresql", dr["postgresql_sql_statement"].ToString());
            Toolkit.SetAttValue(nd, "QueryPrimaryKeyField", dr["primary_key_field"].ToString());
            Toolkit.SetAttValue(nd, "QueryResolvedPrimaryKeyField", dr["resolved_primary_key_field"].ToString());
            Toolkit.SetAttValue(nd, "CacheMode", dr["cache_mode"].ToString());
            Toolkit.SetAttValue(nd, "Method", dr["method"].ToString());
            Toolkit.SetAttValue(nd, "DataviewName", dr["dataview_name"].ToString());
            Toolkit.SetAttValue(nd, "Enabled", dr["enabled"].ToString());
            Toolkit.SetAttValue(nd, "AllowRealTimeUpdates", dr["allow_realtime_updates"].ToString());
            Toolkit.SetAttValue(nd, "SqlSource", dr["sql_source"].ToString());

            doc.Save(Index.Indexer.ConfigFilePath);

        }

        #endregion Active Use

        #region Cleanup
        private void cleanupStreams() {
            if (_primaryKeyDataReader != null) {
                _primaryKeyDataReader.Close();
                _primaryKeyDataReader = null;
            }
            if (_primaryKeyDataWriter != null) {
                _primaryKeyDataWriter.Close();
                _primaryKeyDataWriter = null;
            }
            if (_primaryKeyDataFileStream != null) {
                _primaryKeyDataFileStream.Close();
                _primaryKeyDataFileStream = null;
            }

            if (_keywordDataReader != null) {
                _keywordDataReader.Close();
                _keywordDataReader = null;
            }
            if (_keywordDataWriter != null) {
                _keywordDataWriter.Close();
                _keywordDataWriter = null;
            }
            if (_keywordDataFileStream != null) {
                _keywordDataFileStream.Close();
                _keywordDataFileStream = null;
            }

            if (_queueWriter != null) {
                _queueWriter.Close();
                _queueWriter = null;
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Closes the B+ tree and all associated files
        /// </summary>
        public void Dispose() {
            if (_primaryKeyTree != null) {
                _primaryKeyTree.Dispose();
                _primaryKeyTree = null;
            }

            if (_keywordTree != null) {
                _keywordTree.Dispose();
                _keywordTree = null;
            }

            cleanupStreams();

        }

        #endregion

        #endregion Cleanup

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "Resolver", resourceName, null, defaultValue, substitutes);
        }

    }
}
