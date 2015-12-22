using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrinGlobal.Interface;
using GrinGlobal.Core;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using GrinGlobal.Core.Xml;
using GrinGlobal.Interface.Dataviews;
// using wcf=GrinGlobal.Business.SearchSvc;
using wcf=GrinGlobal.Search.Engine.Service.SearchSvc;
using System.Data;

namespace GrinGlobal.Business {
    /// <summary>
    /// Proxy class for calling the search engine either using a WCF connection or a custom TCP class, which is specified by the config settings of SearchEngineBindingUrl and SearchEngineBindingType.
    /// </summary>
    public class ClientSearchEngineRequest : SearchEngineRequest {

        private string _exceptionMessage;
        private Node _data;

        private bool _useWCF;

        public ClientSearchEngineRequest(string bindingType, string bindingUrl) : base() {
            if (String.IsNullOrEmpty(bindingType)) {
                bindingType = Toolkit.GetSearchEngineBindingType();
            }
            if (String.IsNullOrEmpty(bindingUrl)) {
                bindingUrl = Toolkit.GetSearchEngineBindingUrl();
                if (String.IsNullOrEmpty(bindingUrl)) {
                    switch (bindingType.ToLower()) {
                        case "pipe":
                        default:
                            bindingUrl = "net.pipe://localhost/searchhost";
                            break;
                        case "http":
                            bindingUrl = "http://localhost:2011/searchhost";
                            break;
                        case "tcp":
                            bindingUrl = "http://localhost:2012/searchhost";
                            break;
                    }
                }
            }

            Binding = Toolkit.GetSearchEngineBinding(bindingType);
            Address = Toolkit.GetSearchEngineAddress(bindingUrl);

            _useWCF = bindingType.ToLower() != "tcp";

        }

        private wcf.SearchHostClient getWCFClient() {
            return new wcf.SearchHostClient(Binding, Address);
        }

        public bool Ping() {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    return c.Ping();
                }
            } else {
                this.MethodName = "ping";
                sendBasicTcpRequest();
                return true;
            }
        }

        private List<wcf.UpdateRow> convertToWcfRowList(List<UpdateRow> rows) {
            var svcRows = new List<wcf.UpdateRow>();
            foreach (var r in rows) {
                svcRows.Add(convertToWcfRow(r));
            }
            return svcRows;
        }

        private wcf.UpdateRow convertToWcfRow(UpdateRow row) {
            var svcRow = new wcf.UpdateRow();
            svcRow.ID = row.ID;
            svcRow.Mode = (wcf.UpdateMode)Enum.Parse(typeof(wcf.UpdateMode), row.Mode.ToString(), true);
            // svcRow.Values = new List<GrinGlobal.Business.SearchSvc.FieldValue>();
            svcRow.Values = new List<GrinGlobal.Search.Engine.Service.SearchSvc.FieldValue>();
            if (row.Values != null) {
                foreach (var fv in row.Values) {
                    svcRow.Values.Add(convertToWcfFieldValue(fv));
                }
            }
            return svcRow;
        }

        private wcf.FieldValue convertToWcfFieldValue(FieldValue fv) {
            var svcFV = new wcf.FieldValue();
            svcFV.FieldName = fv.FieldName;
            svcFV.OriginalValue = fv.OriginalValue;
            svcFV.NewValue = fv.NewValue;
            return svcFV;
        }

        public void UpdateIndex(string indexName, List<UpdateRow> rows) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    var svcRows = convertToWcfRowList(rows);
                    c.UpdateIndex(indexName, svcRows);
                }
            } else {
                this.MethodName = "UpdateIndex";
                this.Parameters.Add("indexName", indexName);
                var rowsXml = "";
                this.Parameters.Add("rows", rowsXml);

                sendBasicTcpRequest();
            }
        }

        public void SaveResolverSettings(DataSet ds) {
            var dsXml = Toolkit.DataSetToXml(ds);
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.SaveResolverSettings(dsXml);
                }
            } else {
                this.MethodName = "saveresolversettings";
                this.Parameters.Add("ds", dsXml);
                sendBasicTcpRequest();
            }
        }

        public void SaveIndexerSettings(DataSet ds) {
            var dsXml = Toolkit.DataSetToXml(ds);
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.SaveIndexerSettings(dsXml);
                }
            } else {
                this.MethodName = "saveindexersettings";
                this.Parameters.Add("ds", dsXml);
                sendBasicTcpRequest();
            }
        }

        public void SaveIndexSettings(DataSet ds) {
            var dsXml = Toolkit.DataSetToXml(ds);
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.SaveIndexSettings(dsXml);
                }
            } else {
                this.MethodName = "saveindexsettings";
                this.Parameters.Add("ds", dsXml);
                sendBasicTcpRequest();
            }
        }


        public void DisableResolver(string indexName, string resolverName) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.DisableResolver(indexName, resolverName);
                }
            } else {
                this.MethodName = "disableresolver";
                this.Parameters.Add("indexName", indexName);
                this.Parameters.Add("resolverName", resolverName);
                sendBasicTcpRequest();
            }
        }

        public void EnableResolver(string indexName, string resolverName) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.EnableResolver(indexName, resolverName);
                }
            } else {
                this.MethodName = "enableresolver";
                this.Parameters.Add("indexName", indexName);
                this.Parameters.Add("resolverName", resolverName);
                sendBasicTcpRequest();
            }
        }

        public void RebuildIndexes(List<string> indexNames, bool onlyIfEnabled, bool rotateInOnComplete) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.RebuildIndexes(indexNames, onlyIfEnabled, rotateInOnComplete);
                }
            } else {
                this.MethodName = "rebuildindexes";
                if (indexNames == null || indexNames.Count == 0) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                this.Parameters.Add("onlyIfEnabled", onlyIfEnabled.ToString());
                this.Parameters.Add("rotateInOnComplete", rotateInOnComplete.ToString());
                sendBasicTcpRequest();
            }
        }


        public void ReloadIndexes(List<string> indexNames) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.ReloadIndexes(indexNames);
                }
            } else {
                this.MethodName = "reloadindexes";
                if (indexNames == null || indexNames.Count == 0) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                sendBasicTcpRequest();
            }
        }

        public List<string> VerifyIndexes(List<string> indexNames, bool checkLeaves) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    return c.VerifyIndexes(indexNames, checkLeaves);
                }
            } else {
                this.MethodName = "verifyindexes";
                if (indexNames == null || indexNames.Count == 0) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                this.Parameters.Add("checkLeaves", checkLeaves.ToString());
                sendBasicTcpRequest();
                var indexList = _data.InnerXml();
                if (String.IsNullOrEmpty(indexList)) {
                    return new List<string>();
                } else {
                    return indexList.Split(";", true).ToList();
                }
            }
        }

        public void EnableIndexes(List<string> indexNames) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.EnableIndexes(indexNames);
                }
            } else {
                this.MethodName = "enableindexes";
                if (indexNames == null || indexNames.Count == 0) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                sendBasicTcpRequest();
            }
        }

        public void DefragIndexes(List<string> indexNames, bool rotateInOnComplete) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.DefragIndexes(indexNames, rotateInOnComplete);
                }
            } else {
                this.MethodName = "enableindexes";
                if (indexNames == null || indexNames.Count == 0) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                this.Parameters.Add("rotateInOnComplete", rotateInOnComplete.ToString());
                sendBasicTcpRequest();
            }
        }

        public DataSet Search(string searchString, bool ignoreCase, bool autoAndConsecutiveLiterals, List<string> indexNames, string resolverName, int offset, int limit, string ds, string options) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    var output = c.Search(searchString, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, offset, limit, ds, options);
                    return Toolkit.DataSetFromXml(output);
                }
            } else {
                MethodName = "search";
                Parameters.Add("searchString", searchString);
                Parameters.Add("ignoreCase", ignoreCase.ToString());
                Parameters.Add("autoAndConsecutiveLiterals", autoAndConsecutiveLiterals.ToString());
                if (indexNames == null || indexNames.Count == 0) {
                    Parameters.Add("indexNames", "");
                } else {
                    Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                Parameters.Add("resolverName", resolverName);
                Parameters.Add("offset", offset.ToString());
                Parameters.Add("limit", limit.ToString());
                Parameters.Add("ds", ds);
                Parameters.Add("options", options);
                sendBasicTcpRequest();
                var dsOut = Toolkit.DataSetFromXml(_data.InnerXml());
                return dsOut;
            }
        }

        public void DisableIndexes(List<string> indexNames) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.DisableIndexes(indexNames);
                }
            } else {
                this.MethodName = "disableindexes";
                if (indexNames == null) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                sendBasicTcpRequest();
            }
        }

        public void DeleteIndexes(List<string> indexNames) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    c.DeleteIndexes(indexNames);
                }
            } else {
                this.MethodName = "deleteindexes";
                if (indexNames == null) {
                    this.Parameters.Add("indexNames", "");
                } else {
                    this.Parameters.Add("indexNames", String.Join(";", indexNames.ToArray()));
                }
                sendBasicTcpRequest();
            }
        }

        public DataSet GetInfo(bool enabledIndexesOnly) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    var dsXml = c.GetInfo(enabledIndexesOnly);
                    return Toolkit.DataSetFromXml(dsXml);
                }
            } else {
                this.MethodName = "getinfo";
                this.Parameters.Add("enabledIndexesOnly", enabledIndexesOnly.ToString());
                sendBasicTcpRequest();
                var dsXml = _data.InnerXml();
                return Toolkit.DataSetFromXml(dsXml);
            }
        }

        public DataSet GetInfoEx(bool enabledIndexesOnly, string onlyThisIndex, string onlyThisResolver) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    var dsXml = c.GetInfoEx(enabledIndexesOnly, onlyThisIndex, onlyThisResolver);
                    return Toolkit.DataSetFromXml(dsXml);
                }
            } else {
                this.MethodName = "getinfoex";
                this.Parameters.Add("enabledIndexesOnly", enabledIndexesOnly.ToString());
                this.Parameters.Add("onlyThisIndex", onlyThisIndex);
                this.Parameters.Add("onlyThisResolver", onlyThisResolver);
                sendBasicTcpRequest();
                var dsXml = _data.InnerXml();
                return Toolkit.DataSetFromXml(dsXml);
            }
        }

        public string GetLatestMessages(int maxMessages) {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    var output = c.GetLatestMessages(maxMessages);
                    return output;
                }
            } else {
                this.MethodName = "getlatestmessages";
                this.Parameters.Add("maxMessages", maxMessages.ToString());
                sendBasicTcpRequest();
                var output = _data.InnerXml();
                return output;
            }
        }

        public DataSet GetStatus() {
            if (_useWCF) {
                using (var c = getWCFClient()) {
                    var dsXml = c.GetStatus();
                    return Toolkit.DataSetFromXml(dsXml);
                }
            } else {
                this.MethodName = "getstatus";
                sendBasicTcpRequest();
                var ds = Toolkit.DataSetFromXml(_data.InnerXml());
                return ds;
            }
        }

        
        private void sendBasicTcpRequest() {
            _exceptionMessage = null;
            try {
                using (var client = new TcpClient(Address.Uri.Host, Address.Uri.Port)) {
                    string response = null;
                    using (var stm = client.GetStream()) {
                        // write the request, send it..
                        var xml = this.ToXml();
                        var bytes = UTF8Encoding.UTF8.GetBytes(xml);
                        stm.Write(bytes, 0, bytes.Length);
                        stm.Flush();

                        response = Toolkit.ReadAllStreamText(stm, "</resp>");
                    }
                    if (!String.IsNullOrEmpty(response)) {
                        parseBasicTcpResponse(response);
                    }
                }
                if (!String.IsNullOrEmpty(_exceptionMessage)) {
                    throw new InvalidOperationException(_exceptionMessage);
                }
            } catch (Exception ex){
                _exceptionMessage = ex.Message;
                throw;
            }
        }

        private void parseBasicTcpResponse(string xml) {

            /*
             * <resp>
             *   <error>error message here</error>
             *   <data>return value here</data>
             * </resp>
            */

            _data = null;
            var doc = new Document();
            doc.LoadXml(xml);
            foreach (Node n in doc.Root.Nodes) {
                switch (n.NodeName.ToLower()) {
                    case "error":
                        _exceptionMessage = n.NodeValue;
                        break;
                    case "data":
                        _data = n;
                        break;
                    default:
                        // ignore
                        break;
                }
            }
        }

        public override string ToXml() {
            using (var sw = new StringWriter()) {
                using (var xtw = new XmlTextWriter(sw)) {
                    xtw.WriteStartElement("req");

                    xtw.WriteStartElement("method");
                    xtw.WriteAttributeString("Name", MethodName);

                    foreach (var key in Parameters.Keys) {
                        xtw.WriteStartElement("parameter");
                        xtw.WriteAttributeString("Name", key);
                        xtw.WriteString(Parameters[key]);
                        xtw.WriteEndElement();
                    }

                    // /method
                    xtw.WriteEndElement();

                    // /req
                    xtw.WriteEndElement();

                    return sw.ToString();
                }
            }
        }
    }
}
