using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using GrinGlobal.Core;
using GrinGlobal.Core.Xml;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace GrinGlobal.Business {
    /// <summary>
    /// Houses properties for connecting to either a database directly or the GRIN-Global web service / WCF service.
    /// </summary>
    [Serializable()]
    public class ConnectionInfo {

        public ConnectionInfo() {
            DatabaseEngineProviderName = "sqlserver";
            DatabaseEngineServerName = @"localhost\sqlexpress";
            DatabaseEngineDatabaseName = "gringlobal";
            DatabaseEngineSID = "";
            SearchEngineBindingType = "pipe";
            SearchEngineBindingUrl = "net.pipe://localhost/searchhost";
            WebAppPhysicalPath = Toolkit.GetIISPhysicalPath("gringlobal");
        }

        public DateTime LastUsed;

        public string DatabaseEngineServerName;
        public string DatabaseEngineProviderName;
        public string DatabaseEngineDatabaseName;
        public string DatabaseEngineSID;

        public string DatabaseEngineUserName;
        public string DatabaseEnginePassword;
        public bool DatabaseEngineRememberPassword;

        public string GrinGlobalUserName;
        public string GrinGlobalPassword;
        public bool GrinGlobalRememberPassword;
        public string GrinGlobalLoginToken;

        public string GrinGlobalHashedPassword {
            get {
                return Crypto.HashText(GrinGlobalPassword);
            }
        }

        public string SearchEngineBindingType;
        public string SearchEngineBindingUrl;
        public string WebAppPhysicalPath;

        public bool UseWebService;

        public string GrinGlobalUrl;
        public bool UseWindowsAuthentication;
        //    get {
        //        return String.IsNullOrEmpty(DatabaseEngineUserName) && DatabaseEngineProviderName.ToLower() == "sqlserver";
        //    }
        //    set {
        //        DatabaseEngineProviderName = "sqlserver";
        //        DatabaseEngineUserName = null;
        //        DatabaseEnginePassword = null;
        //    }
        //}

        public string ServerName {
            get {
                if (String.IsNullOrEmpty(GrinGlobalUrl)) {
                    return DatabaseEngineServerName;
                } else {
                    return GrinGlobalUrl;
                }
            }
        }

        public bool IsLocal {
            get {
                return DatabaseEngineServerName.ToLower().Contains("localhost")
                    || DatabaseEngineServerName.Contains("127.0.0.1")
                    || DatabaseEngineServerName.StartsWith(".")
                    || DatabaseEngineServerName.ToLower().StartsWith("(local)")
                    || DatabaseEngineServerName.ToLower().Contains(Dns.GetHostName().ToLower());
            }

        }

        public static ConnectionInfo FromXmlNode(Node nd) {
            var ci = new ConnectionInfo();
            ci.DatabaseEngineServerName = nd.Attributes.GetValue("DatabaseEngineName");
            ci.DatabaseEngineProviderName = nd.Attributes.GetValue("DatabaseEngineProvider", "sqlserver");
            ci.UseWindowsAuthentication = Toolkit.ToBoolean(nd.Attributes.GetValue("UseWindowsAuthentication", "true"), true);
            ci.DatabaseEngineRememberPassword = Toolkit.ToBoolean(nd.Attributes.GetValue("DatabaseEngineRememberPassword", "true"), true);
            ci.DatabaseEngineDatabaseName = nd.Attributes.GetValue("DatabaseEngineDatabaseName", "gringlobal");
            ci.DatabaseEngineUserName = nd.Attributes.GetValue("DatabaseEngineUserName");
            ci.DatabaseEnginePassword = Crypto.DecryptText(nd.Attributes.GetValue("DatabaseEnginePassword"));

            ci.DatabaseEngineSID = nd.Attributes.GetValue("DatabaseEngineSID", "");

            ci.GrinGlobalUserName = nd.Attributes.GetValue("GrinGlobalUserName");
            ci.GrinGlobalPassword = Crypto.DecryptText(nd.Attributes.GetValue("GrinGlobalPassword"));
            ci.GrinGlobalRememberPassword = Toolkit.ToBoolean(nd.Attributes.GetValue("GrinGlobalRememberPassword", "true"), true);
            ci.GrinGlobalUrl = nd.Attributes.GetValue("GrinGlobalUrl");

            ci.SearchEngineBindingType = nd.Attributes.GetValue("SearchEngineBindingType", "pipe");
            ci.SearchEngineBindingUrl = nd.Attributes.GetValue("SearchEngineBindingUrl", "net.pipe://localhost/searchhost");
            ci.WebAppPhysicalPath = nd.Attributes.GetValue("WebAppPhysicalPath", Toolkit.GetIISPhysicalPath("gringlobal"));

            ci.UseWebService = Toolkit.ToBoolean(nd.Attributes.GetValue("UseWebService", (String.IsNullOrEmpty(ci.GrinGlobalUrl) ? "false" : "true")), false);

            ci.LastUsed = Toolkit.ToDateTime(nd.Attributes.GetValue("LastUsed", ""), DateTime.UtcNow.AddDays(-30));
            if (ci.UseWebService) {
                return null;
            } else {
                return ci;
            }
        }

        public Node ToXmlNode() {
            var nd = new Node("Server");
            nd.Attributes.SetValue("DatabaseEngineName", DatabaseEngineServerName);
            nd.Attributes.SetValue("DatabaseEngineProvider", DatabaseEngineProviderName);
            nd.Attributes.SetValue("UseWindowsAuthentication", UseWindowsAuthentication.ToString());
            nd.Attributes.SetValue("DatabaseEngineRememberPassword", DatabaseEngineRememberPassword.ToString());
            nd.Attributes.SetValue("DatabaseEngineDatabaseName", DatabaseEngineDatabaseName);
            nd.Attributes.SetValue("DatabaseEngineUserName", DatabaseEngineUserName);
            if (DatabaseEngineRememberPassword) {
                nd.Attributes.SetValue("DatabaseEnginePassword", Crypto.EncryptText(DatabaseEnginePassword));
            }

            nd.Attributes.SetValue("DatabaseEngineSID", DatabaseEngineSID);

            nd.Attributes.SetValue("GrinGlobalUserName", GrinGlobalUserName);
            nd.Attributes.SetValue("GrinGlobalRememberPassword", GrinGlobalRememberPassword.ToString());
            if (GrinGlobalRememberPassword) {
                nd.Attributes.SetValue("GrinGlobalPassword", Crypto.EncryptText(GrinGlobalPassword));
            }
            nd.Attributes.SetValue("GrinGlobalUrl", GrinGlobalUrl);

            nd.Attributes.SetValue("SearchEngineBindingType", SearchEngineBindingType);
            nd.Attributes.SetValue("SearchEngineBindingUrl", SearchEngineBindingUrl);
            nd.Attributes.SetValue("WebAppPhysicalPath", WebAppPhysicalPath);


            nd.Attributes.SetValue("UseWebService", UseWebService.ToString());
            nd.Attributes.SetValue("LastUsed", LastUsed.ToString());
            return nd;

        }

        public ConnectionInfo(SerializationInfo info, StreamingContext ctx) {
            DatabaseEngineServerName = info.GetString("DatabaseEngineServerName");
            DatabaseEngineProviderName = info.GetString("DatabaseEngineProvider");
            UseWindowsAuthentication = info.GetBoolean("UseWindowsAuthentication");
            DatabaseEngineRememberPassword = info.GetBoolean("DatabaseEngineRememberPassword");
            DatabaseEngineDatabaseName = info.GetString("DatabaseEngineDatabaseName");
            DatabaseEngineUserName = info.GetString("DatabaseEngineUserName");
            DatabaseEnginePassword = info.GetString("DatabaseEnginePassword");
            DatabaseEngineSID = info.GetString("DatabaseEngineSID");
            GrinGlobalUserName = info.GetString("GrinGlobalUserName");
            GrinGlobalRememberPassword = info.GetBoolean("GrinGlobalRememberPassword");
            GrinGlobalPassword = info.GetString("GrinGlobalPassword");
            GrinGlobalUrl = info.GetString("GrinGlobalUrl");

            SearchEngineBindingType = info.GetString("SearchEngineBindingType");
            SearchEngineBindingUrl = info.GetString("SearchEngineBindingUrl");
            WebAppPhysicalPath = info.GetString("WebAppPhysicalPath");


            UseWebService = info.GetBoolean("UseWebService");
            try {
                LastUsed = info.GetDateTime("LastUsed");
            } catch {
                LastUsed = DateTime.UtcNow.AddDays(-30);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctx) {
            info.AddValue("DatabaseEngineServerName", DatabaseEngineServerName);
            info.AddValue("DatabaseEngineProviderName", DatabaseEngineProviderName);
            info.AddValue("UseWindowsAuthentication", UseWindowsAuthentication);
            info.AddValue("DatabaseEngineRememberPassword", DatabaseEngineRememberPassword);
            info.AddValue("DatabaseEngineDatabaseName", DatabaseEngineDatabaseName);
            info.AddValue("DatabaseEngineUserName", DatabaseEngineUserName);
            info.AddValue("DatabaseEnginePassword", DatabaseEnginePassword);
            info.AddValue("DatabaseEngineSID", DatabaseEngineSID);
            info.AddValue("GrinGlobalUserName", GrinGlobalUserName);
            info.AddValue("GrinGlobalRememberPassword", GrinGlobalRememberPassword);
            info.AddValue("GrinGlobalPassword", GrinGlobalPassword);
            info.AddValue("GrinGlobalUrl", GrinGlobalUrl);
            info.AddValue("SearchEngineBindingType", SearchEngineBindingType);
            info.AddValue("SearchEngineBindingUrl", SearchEngineBindingUrl);
            info.AddValue("WebAppPhysicalPath", WebAppPhysicalPath);

            info.AddValue("UseWebService", UseWebService);
            try {
                info.AddValue("LastUsed", LastUsed);
            } catch {
                LastUsed = DateTime.UtcNow.AddDays(-30);
            }
        }

        public DataConnectionSpec GenerateDataConnectionSpec() {
            var dcs = new DataConnectionSpec {
                ProviderName = DatabaseEngineProviderName,
                ServerName = DatabaseEngineServerName,
                DatabaseName = DatabaseEngineDatabaseName,
                UserName = DatabaseEngineUserName,
                Password = DatabaseEnginePassword,
                UseWindowsAuthentication = UseWindowsAuthentication,
                SID = DatabaseEngineSID
            };
            return dcs;
        }

        public string Serialize() {
            var ms = new MemoryStream();
            var fmtr = new BinaryFormatter();
            fmtr.Serialize(ms, this);

            var output = Convert.ToBase64String(ms.ToArray());
            return output;

        }

        public static ConnectionInfo Deserialize(string b64ConnectionInfo) {
            var ms = new MemoryStream(Convert.FromBase64String(b64ConnectionInfo));
            var fmtr = new BinaryFormatter();
            var rv = (ConnectionInfo)fmtr.Deserialize(ms);
            return rv;
        }

    }
}
