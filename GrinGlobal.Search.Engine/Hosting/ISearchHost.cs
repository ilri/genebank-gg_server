using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GrinGlobal.Search.Engine;
using System.Data;
using System.ServiceModel.Web;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine.Hosting {
	// NOTE: If you change the interface name "ISearchHost" here, you must also update the reference to "ISearchHost" in App.config.
	[ServiceContract(Namespace="http://www.grin-global.org/")]
	public interface ISearchHost {

        /// <summary>
        /// Returns the resolved ID's for the given search parameters
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="autoAndConsecutiveLiterals"></param>
        /// <param name="indexNames"></param>
        /// <param name="resolverName"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
//        // [WebInvoke(UriTemplate = "search", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        string Search(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, int offset, int limit, string ds, string options);

        /// <summary>
        /// Returns the specific hit data for the given search parameters
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="autoAndConsecutiveLiterals"></param>
        /// <param name="indexNames"></param>
        /// <param name="resolverName"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "searchforhits", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        List<ResolvedHitData> SearchForHits(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, bool returnHitsWithNoResolvedIDs, int offset, int limit);

        /// <summary>
        /// Rotates the given indexes into use for search queries
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "rotateindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void RotateIndexes(List<string> indexNames);

        /// <summary>
        /// Lists all indexes in the search engine, optionally only those that are currently enabled
        /// </summary>
        /// <param name="enabledOnly"></param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "listindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        List<string> ListIndexes(bool enabledOnly);

        /// <summary>
        /// Returns information about the search engine, such as index information, status counts (# of realtime updates pending, index creations pending, etc), etc.  Optionally only those that are currently enabled
        /// </summary>
        /// <param name="enabledIndexesOnly"></param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "getinfo", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        string GetInfo(bool enabledIndexesOnly);

        /// <summary>
        /// Returns information about the search engine, such as index information, status counts (# of realtime updates pending, index creations pending, etc), etc.  Optionally only those that are currently enabled
        /// </summary>
        /// <param name="enabledIndexesOnly"></param>
        /// <param name="onlyThisIndex"></param>
        /// <param name="onlyThisResolver"></param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "getinfoex", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        string GetInfoEx(bool enabledIndexesOnly, string onlyThisIndex, string onlyThisResolver);

        /// <summary>
        /// Returns status information about the search engine
        /// </summary>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "getstatus", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        string GetStatus();


        /// <summary>
        /// Returns the latest messages from the search engine log (CSV format)
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "getlatestmessages", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        string GetLatestMessages(int maxMessages);

        /// <summary>
        /// Verifies the B+ tree and data file associated with the given indexes are valid
        /// </summary>
        /// <param name="indexNames"></param>
        /// <param name="checkLeaves"></param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "verifyindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        List<string> VerifyIndexes(List<string> indexNames, bool checkLeaves);


        /// <summary>
        /// Rebuilds given indexes by pulling data from the database.  This is typically a lengthy process.
        /// </summary>
        /// <param name="indexNames">List of indexes to create.  Passing null or list of length 0 creates all.</param>
        /// <param name="onlyIfEnabled">False to enable an index (if needed) before creating it, true to create only indexes already marked as enabled</param>
        /// <param name="rotateInOnComplete">Rotate new index files into active use after creation has completed</param>
        // [WebInvoke(UriTemplate = "rebuildindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void RebuildIndexes(List<string> indexNames, bool onlyIfEnabled, bool rotateInOnComplete);

        /// <summary>
        /// Marks given indexes as Enabled=True
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "enableindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void EnableIndexes(List<string> indexNames);

        /// <summary>
        /// Marks given resolver for given index as Enabled=false
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "disableresolver", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void DisableResolver(string indexName, string reseolverName);

        /// <summary>
        /// Marks given resolver for given index as Enabled=True
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "enableresolver", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void EnableResolver(string indexName, string reseolverName);

        /// <summary>
        /// Saves resolver settings as specified by given DataTable.  DataTable schema can be gotten via GetInfo() or GetInfoEx();
        /// </summary>
        /// <param name="dt"></param>
        // [WebInvoke(UriTemplate = "saveresolversettings", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void SaveResolverSettings(string ds);

        /// <summary>
        /// Saves index settings as specified by given DataTable.  DataTable schema can be gotten via GetInfo() or GetInfoEx();
        /// </summary>
        /// <param name="dt"></param>
        // [WebInvoke(UriTemplate = "saveindexsettings", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void SaveIndexSettings(string ds);

        /// <summary>
        /// Saves indexer settings as specified by given DataTable.  DataTable schema can be gotten via GetInfo() or GetInfoEx();
        /// </summary>
        /// <param name="dt"></param>
        // [WebInvoke(UriTemplate = "saveindexersettings", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void SaveIndexerSettings(string ds);

        /// <summary>
        /// Marks given indexes as Enabled=False
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "disableindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void DisableIndexes(List<string> indexNames);

        /// <summary>
        /// Deletes given indexes and all associated files
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "deleteindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void DeleteIndexes(List<string> indexNames);

        /// <summary>
        /// Closes and reopens given indexes
        /// </summary>
        /// <param name="indexNames"></param>
        // [WebInvoke(UriTemplate = "reloadindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void ReloadIndexes(List<string> indexNames);

        /// <summary>
        /// Returns a report of the status of all indexes
        /// </summary>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "reportonindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        List<IndexReport> ReportOnIndexes();

        /// <summary>
        /// Simply returns true.  Used for troubleshooting connections.
        /// </summary>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "ping", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        bool Ping();

        /// <summary>
        /// Performs multi-row updates on the given index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="rows"></param>
        // [WebInvoke(UriTemplate = "updateindex", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void UpdateIndex(string indexName, List<UpdateRow> rows);

        /// <summary>
        /// Performs a single-row update on the given index
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="row"></param>
        // [WebInvoke(UriTemplate = "updateindexrow", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void UpdateIndexRow(string indexName, UpdateRow row);

        ///// <summary>
        ///// Given a search query, will apply preprocessing to the string and return the tokenized string (inserts boolean operators, for example)
        ///// </summary>
        ///// <param name="searchString"></param>
        ///// <param name="ignoreCase"></param>
        ///// <param name="autoAndConsecutiveLiterals"></param>
        ///// <param name="indexNames"></param>
        ///// <param name="resolverName"></param>
        ///// <returns></returns>
        //// [WebInvoke(UriTemplate = "tokenizequery", BodyStyle = WebMessageBodyStyle.Wrapped)]
        //[OperationContract]
        //// [DynamicFormatterType]
        //string TokenizeQuery(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName);


        /// <summary>
        /// Defrags given indexes by reorganizing data in its files.  Does not touch the database.  This is typically a lengthy process.
        /// </summary>
        /// <param name="indexNames">List of indexes to create.  Passing null or list of length 0 creates all.</param>
        /// <param name="rotateInOnComplete">Rotate new index files into active use after creation has completed</param>
        /// <returns></returns>
        // [WebInvoke(UriTemplate = "defragindexes", BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        // [DynamicFormatterType]
        void DefragIndexes(List<string> indexNames, bool rotateInOnComplete);

	}
}
