using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace GrinGlobal.Import {
    internal class HostProxy {

        public HostProxy(ConnectionInfo ci) {
            _conn = ci;
        }

        public HostProxy(string userName, string password, string url) {
            _conn = new ConnectionInfo { GrinGlobalUrl = url, GrinGlobalUserName = userName, GrinGlobalPassword = password, UseWebService = true, GrinGlobalRememberPassword = true };
            _gui = new GrinGlobal.Import.ggws.GUI();
        }

        public ConnectionInfo Connection {
            get {
                return _conn;
            }
        }

        ggws.GUI _gui;

        ConnectionInfo _conn;

        private DataConnectionSpec getDataConnectionSpec() {
            // use it as a direct dll reference (aka connect via database only, no web services involved)
            var dcs = new DataConnectionSpec {
                ProviderName = _conn.DatabaseEngineProviderName,
                ServerName = _conn.DatabaseEngineServerName,
                DatabaseName = _conn.DatabaseEngineDatabaseName,
                UserName = _conn.DatabaseEngineUserName,
                Password = _conn.DatabaseEnginePassword,
                UseWindowsAuthentication = _conn.UseWindowsAuthentication
            };
            //string token = SecureData.Login(_conn.GrinGlobalUserName, _conn.GrinGlobalPassword, dcs);
            return dcs;
        }

        /// <summary>
        /// Gets data as defined by the given dataview / parameter list
        /// </summary>
        /// <param name="dataviewName"></param>
        /// <param name="delimitedParameterList"></param>
        /// <returns></returns>
        public DataSet GetData(string dataviewName, string delimitedParameterList, int altLanguageID) {
            var options = "altlanguageid=" + altLanguageID;
            if (_conn.UseWebService) {
                return _gui.GetData(false, _conn.GrinGlobalUserName, _conn.GrinGlobalPassword, dataviewName, delimitedParameterList, 0, 0, options);
            } else {
                using (var sd = new SecureData(false, _conn.GrinGlobalLoginToken, _conn.GenerateDataConnectionSpec())) {
                    return sd.GetData(dataviewName, delimitedParameterList, 0, 0, options);
                }
            }


        }

        public string TestDatabaseConnection() {
            return SecureData.TestDatabaseConnection(_conn.GenerateDataConnectionSpec());
        }

        public string Login() {
            return SecureData.Login(_conn.GrinGlobalUserName, _conn.GrinGlobalPassword, _conn.GenerateDataConnectionSpec());
        }

        public bool LanguageRightToLeft;
        private int _languageID = -1;
        public int LanguageID {
            get {
                if (_languageID < 0) {
                    if (_conn.UseWebService) {
                        var ds = _gui.ValidateLogin(false, _conn.GrinGlobalUserName, _conn.GrinGlobalPassword);
                        var dt = ds.Tables["validate_login"];
                        if (dt != null) {
                            if (dt.Rows.Count > 0) {
                                _languageID = Toolkit.ToInt32(dt.Rows[0]["sys_lang_id"], -1);
                                LanguageRightToLeft = dt.Rows[0]["script_direction"].ToString().ToUpper() == "RTL";
                            }
                        }
                    } else {
                        using (var sd = new SecureData(false, _conn.GrinGlobalLoginToken, _conn.GenerateDataConnectionSpec())) {
                            _languageID = sd.LanguageID;
                            LanguageRightToLeft = sd.LanguageDirection.ToUpper() == "RTL";
                        }
                    }
                }
                return _languageID;
            }
        }

        private bool? _isAdministrator;
        public bool IsAdministrator {
            get {
                if (_isAdministrator == null) {
                    if (_conn.UseWebService) {
                    } else {
                        using (var sd = new SecureData(false, _conn.GrinGlobalLoginToken, _conn.GenerateDataConnectionSpec())) {
                            _isAdministrator = sd.IsAdministrator();
                        }
                    }
                }

                return (bool)_isAdministrator;
            }
        }

        /// <summary>
        /// Saves data to the database using unique key fields as the primary key (i.e. _id fields not needed)
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet SaveData(DataSet ds, int altLanguageID, bool insertOnlyLanguageData, int ownerID, BackgroundWorker worker) {
            var options = "useuniquekeys=true;booldefaultfalse=true;onlyifallrequiredfieldsexist=true;safeupdatesanddeletes=false;skipsearchengineupdates=true;rowprogressinterval=10;insertonlylanguagedata=" + insertOnlyLanguageData + ";altlanguageid=" + altLanguageID;
            if (IsAdministrator) {
                options += ";ownerid=" + ownerID;
            }
            if (_conn.UseWebService) {
                return _gui.SaveDataSet(true, _conn.GrinGlobalUserName, _conn.GrinGlobalPassword, ds, options); 
            } else {
                using (var sd = new SecureData(true, _conn.GrinGlobalLoginToken, _conn.GenerateDataConnectionSpec())) {
                    return sd.SaveData(ds, false, options, worker);
                }
            }
        }

        public bool RebuildSearchEngineIndexes(List<string> outdatedIndexNames) {
            if (_conn.UseWebService) {
                MessageBox.Show(
                    getDisplayMember("RebuildSearchEngineIndexes{websvc_body}", "Search engine indexes cannot be rebuilt over a web connection.\nPlease use the Admin Tool to rebuild the following search engine indexes:\r\n{0}", String.Join("\r\n", outdatedIndexNames.ToArray())),
                    getDisplayMember("RebuildSearchEngineIndexes{websvc_title}", "Manually Rebuild Indexes via Admin Tool"), MessageBoxButtons.OK);
                return false;
            } else {
                using (var ad = new AdminData(true, _conn.GrinGlobalLoginToken, _conn.GenerateDataConnectionSpec())) {
                    foreach(var idxName in outdatedIndexNames){
                        try {
                            ad.RebuildSearchEngineIndex(idxName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
                        } catch (Exception ex){
                            MessageBox.Show(getDisplayMember("RebuildSearchEngineIndexes{failed}", "Error rebuilding search engine index {0}: {1}", idxName, ex.Message));
                        }
                    }
                    return true;
                }
            }
        }

        public void ClearCache() {
            CacheManager.ClearAll();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "ImportWizard", "HostProxy", resourceName, null, defaultValue, substitutes);
        }
    }
}
