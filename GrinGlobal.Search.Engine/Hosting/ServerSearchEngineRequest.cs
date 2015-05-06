using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Core.Xml;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Interface;

namespace GrinGlobal.Search.Engine.Hosting {
    public class ServerSearchEngineRequest : SearchEngineRequest {

        private ISearchHost _host;

        private string _responseData;

        public ServerSearchEngineRequest(ISearchHost host) :base() {
            _host = host;
        }

        public static ServerSearchEngineRequest Parse(string xml, ISearchHost host) {

            var request = new ServerSearchEngineRequest(host);
            Document doc = new Document();
            doc.LoadXml(xml);
            foreach (Node node in doc.Root.Nodes) {
                if (node.NodeName.ToLower() == "method") {
                    var methodName = node.Name.ToLower();
                    if (!String.IsNullOrEmpty(methodName)) {
                        request.MethodName = methodName;
                        foreach (Node prm in node.Nodes) {
                            if (prm.NodeName.ToLower() == "parameter") {
                                request.Parameters.Add(prm.Name, prm.NodeValue);
                            }
                        }
                    }
                    break;
                }
            }

            return request;

        }

        public SearchEngineRequest Execute() {
            _responseData = null;
            try {
                switch (MethodName) {
                    case "search":
                        //string Search(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, int offset, int limit, string ds, string options);
                        _responseData = _host.Search(GetString("searchString"),
                            GetBool("ignoreCase"),
                            GetBool("autoAndConsecutiveLiterals"),
                            GetListString("indexNames"),
                            GetString("resolverName"),
                            GetInt32("offset"),
                            GetInt32("limit"),
                            GetString("ds"),
                            GetString("options"));
                        break;
                    case "listindexes":
                        //List<string> ListIndexes(bool enabledOnly);
                        _responseData = String.Join(";", _host.ListIndexes(GetBool("enabledOnly")).ToArray());
                        break;
                    case "getinfo":
                        //string GetInfo(bool enabledIndexesOnly);
                        _responseData = _host.GetInfo(GetBool("enabledIndexesOnly"));
                        break;
                    case "getinfoex":
                        //string GetInfoEx(bool enabledIndexesOnly, string onlyThisIndex, string onlyThisResolver);
                        _responseData = _host.GetInfoEx(GetBool("enabledIndexesOnly"), GetString("onlyThisIndex"), GetString("onlyThisResolver"));
                        break;
                    case "getstatus":
                        //string GetStatus();
                        _responseData = _host.GetStatus();
                        break;
                    case "getlatestmessages":
                        //string GetLatestMessages(int maxMessages);
                        _responseData = _host.GetLatestMessages(GetInt32("maxMessages"));
                        break;
                    case "verifyindexes":
                        //List<string> VerifyIndexes(List<string> indexNames, bool checkLeaves);
                        _responseData = String.Join(";", _host.VerifyIndexes(GetListString("indexNames"), GetBool("checkLeaves")).ToArray());
                        break;
                    case "rebuildindexes":
                        //void RebuildIndexes(List<string> indexNames, bool onlyIfEnabled, bool rotateInOnComplete);
                        _host.RebuildIndexes(GetListString("indexNames"), GetBool("onlyIfEnabled"), GetBool("rotateInOnComplete"));
                        break;
                    case "enableindexes":
                        //void EnableIndexes(List<string> indexNames);
                        _host.EnableIndexes(GetListString("indexNames"));
                        break;
                    case "disableresolver":
                        //void DisableResolver(string indexName, string reseolverName);
                        _host.DisableResolver(GetString("indexName"), GetString("resolverName"));
                        break;
                    case "enableresolver":
                        //void EnableResolver(string indexName, string reseolverName);
                        _host.EnableResolver(GetString("indexName"), GetString("resolverName"));
                        break;
                    case "saveresolversettings":
                        //void SaveResolverSettings(string ds);
                        _host.SaveResolverSettings(GetString("ds"));
                        break;
                    case "saveindexsettings":
                        //void SaveIndexSettings(string ds);
                        _host.SaveIndexSettings(GetString("ds"));
                        break;
                    case "saveindexersettings":
                        //void SaveIndexerSettings(string ds);
                        _host.SaveIndexerSettings(GetString("ds"));
                        break;
                    case "disableindexes":
                        //void DisableIndexes(List<string> indexNames);
                        _host.DisableIndexes(GetListString("indexNames"));
                        break;
                    case "deleteindexes":
                        //void DeleteIndexes(List<string> indexNames);
                        _host.DeleteIndexes(GetListString("indexNames"));
                        break;
                    case "reloadindexes":
                        //void ReloadIndexes(List<string> indexNames);
                        _host.ReloadIndexes(GetListString("indexNames"));
                        break;
                    case "ping":
                        //bool Ping();
                        _responseData = _host.Ping().ToString();
                        break;
                    case "updateindex":
                        //void UpdateIndex(string indexName, List<UpdateRow> rows);
                        _host.UpdateIndex(GetString("indexName"), GetListUpdateRow("rows"));
                        break;
                    case "updateindexrow":
                        //void UpdateIndexRow(string indexName, UpdateRow row);
                        _host.UpdateIndexRow(GetString("indexName"), GetUpdateRow("row"));
                        break;
                    case "defragindexes":
                        //void DefragIndexes(List<string> indexNames, bool rotateInOnComplete);
                        _host.DefragIndexes(GetListString("indexNames"), GetBool("rotateInOnComplete"));
                        break;
                    default:
                    case "reportonindexes":
                    case "rotateindexes":

                        //List<IndexReport> ReportOnIndexes();
                        //break;

                        //List<ResolvedHitData> SearchForHits(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, bool returnHitsWithNoResolvedIDs, int offset, int limit);
                        //void RotateIndexes(List<string> indexNames);
                        // break;

                        throw new NotImplementedException(getDisplayMember("Execute", "MethodName={0} is not implemented.", MethodName));

                }
            } catch (Exception ex) {
                Exception = ex;
            }
            return this;
        }

        //private StringBuilder _responseXml;

        //private void addToResponseXml(string xml) {
        //    if (_responseXml == null) {
        //        _responseXml = new StringBuilder();
        //    }
        //    _responseXml.Append(xml);
        //}

        public override string ToXml() {
            return String.Format(
@"<resp>
  <error>{0}</error>
  <data>{1}</data>
</resp>", (this.Exception == null ? "" : this.Exception.Message), _responseData);

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "SearchEngine", "ServerSearchEngineRequest", resourceName, null, defaultValue, substitutes);
        }
    }
}
