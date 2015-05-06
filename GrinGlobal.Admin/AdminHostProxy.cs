using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.ServiceModel;
using System.Diagnostics;
using System.IO;
using GrinGlobal.InstallHelper;
using System.Net;
using GrinGlobal.Interface.Dataviews;
using System.Security.Authentication;
using System.Windows.Forms;

namespace GrinGlobal.Admin {
    /// <summary>
    /// Since having a reference to a dll directly and a reference to the same code via a web service reference result in two completely unrelated interfaces, we have to meld them together in this class.  Its primary/only responsibility is to be the entry point for calling either one of the two interfaces.
    /// </summary>
    public class AdminHostProxy : IDisposable {


        private AdminData _direct;
        private ConnectionInfo _conn;
        public ConnectionInfo Connection {
            get {
                return _conn;
            }
        }

        /// <summary>
        /// If the GrinGlobalLoginToken property is empty, creates a proxy object and calls the Login method using the GrinGlobal Username and Password properties to validate them and generate a new token, which is stored in GrinGlobalLoginToken property of the given ConnectionInfo object for future use.
        /// </summary>
        /// <returns></returns>
        public static bool AutoLogin(ConnectionInfo ci) {
            if (ci == null) {
                // no connection info at all.
                return false;
            } else if (!String.IsNullOrEmpty(ci.GrinGlobalLoginToken)) {
                // already have a login token
                return true;
            } else {
                if (ci.UseWindowsAuthentication || (!String.IsNullOrEmpty(ci.DatabaseEngineUserName) && ci.DatabaseEngineRememberPassword && !String.IsNullOrEmpty(ci.DatabaseEnginePassword))){
                    // probably valid database credentials.
                    if (!String.IsNullOrEmpty(ci.GrinGlobalUserName) && !String.IsNullOrEmpty(ci.GrinGlobalPassword)) {

                        // we have proper gringlobal login capability.

                        var ahs = new AdminHostProxy(ci);
                        string login = ahs.Login(ci.GrinGlobalUserName, ci.GrinGlobalHashedPassword);
                        if (!String.IsNullOrEmpty(login)) {
                            ci.GrinGlobalLoginToken = login;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private delegate DataSet DataSetCallback();


        private DataSet loginIfNeeded(DataSetCallback callback) {
            try {
                return callback();
            } catch (InvalidCredentialException ice){
                // bad login, prompt for new...
                Debug.WriteLine(ice.Message);
                mdiParent.Current.ClearLoginToken();
                mdiParent.Current.SelectConnectionNode();
                return null;
            }
        }

        public AdminHostProxy(ConnectionInfo ci) : this(ci, false) {
        }
        public AdminHostProxy(ConnectionInfo ci, bool errorOnBadCredentials) {
            if (ci != null) {
                // use it as a direct dll reference (aka connect via database only, no web services involved)
                var dcs = new DataConnectionSpec {
                    ProviderName = ci.DatabaseEngineProviderName,
                    ServerName = ci.DatabaseEngineServerName,
                    DatabaseName = ci.DatabaseEngineDatabaseName,
                    UserName = ci.DatabaseEngineUserName,
                    Password = ci.DatabaseEnginePassword,
                    UseWindowsAuthentication = ci.UseWindowsAuthentication,
                    SID = ci.DatabaseEngineSID
                };
                string token = null;
                try {
                    token = AdminData.Login(ci.GrinGlobalUserName, ci.GrinGlobalHashedPassword, dcs);
                    var direct = new AdminData(false, token, dcs);
                    _direct = direct;
                    LanguageID = _direct.LanguageID;
                } catch (Exception ex) {
                    if (errorOnBadCredentials) {
                        mdiParent.Current.ClearLoginToken();
                        throw;
                        //var direct = new SecureData(false, token, dcs);
                        //_direct = direct;
                        //LanguageID = _direct.LanguageID;
                    }
                }

                _conn = ci;
            } else {
                throw new InvalidOperationException(getDisplayMember("constructor", "No connection object was given to AdminHostProxy"));
            }
        }

        /// <summary>
        /// Gets or sets the language id for the current user.
        /// </summary>
        public int LanguageID {
            get {
                return _direct != null ? _direct.LanguageID : 1;
            }
            set {
                if (_direct != null) {
                    _direct.LanguageID = value;
                }
            }
        }

        private void clearLocalCache(params string[] cacheNameStartsWith) {
            var cm = CacheManager.Get(Connection.ServerName);
            if (cacheNameStartsWith == null || cacheNameStartsWith.Length == 0) {
                // clear all local caches period
                cm.Clear();
            } else {
                // clear all local caches that start with the given name
                var keys = cm.Keys.ToList();
                foreach (var key in keys) {
                    foreach (var s in cacheNameStartsWith) {
                        if (key.ToLower().StartsWith(s.ToLower())) {
                            cm.Remove(key);
                        }
                    }
                }
            }
        }

        private bool clearWebCache(string cacheName){
            clearWebCacheForReal(cacheName, true);
            return clearWebCacheForReal(cacheName, false);
        }

        private bool clearWebCacheForReal(string cacheName, bool isDebug){
            try {

                var gui = new GuiExtended();
                gui.WebRequestCreatedCallback = delegate(HttpWebRequest req) {
                    Utility.InitProxySettings(req);
                };
                string hostName = null;
                var parsedHost = _conn.DatabaseEngineServerName.Split(',', ':', '\\', '/');
                if (parsedHost.Length == 0){
                    return false;
                } else {
                    hostName = parsedHost[0];

                    // for debugging locally only...
                    if (isDebug) {
                        if (hostName == "127.0.0.1" || hostName == "localhost" || hostName == "." || hostName == "(local)") {
                            hostName = "localhost:2600";
                        }
                    }
                }

                gui.Url = "http://" + hostName + "/gringlobal/gui.asmx";
                // do the request asynchronously???
                gui.ClearCacheAsync(false, _conn.GrinGlobalUserName, _conn.GrinGlobalHashedPassword, cacheName);
                //gui.ClearCache(false, _conn.GrinGlobalUserName, _conn.GrinGlobalPassword, cacheName);
                return true;
            } catch (Exception ex) {
                Debug.WriteLine("Exception clearing cache '" + cacheName + "': " + ex.Message);
                // eat all errors for now...
                return false;
            }

        }

        private const string EMPTY_OBJECT_MESSAGE = "An object must be given in the constructor for AdminHostProxy that implements either GrinGlobal.Admin.ActualWebService.IWebService or GrinGlobal.Web.IWebService.";

        public DataSet DeleteDataViewDefinition(string dataviewName, bool forceDelete) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteDataViewDefinition(dataviewName, forceDelete);

                clearLocalCache(null);
                //clearLocalCache("dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet GetTableFieldDependencies(string tableName, string fieldName) {
            return loginIfNeeded(delegate() {
                var ret = _direct.GetTableFieldDependencies(tableName, fieldName);
                return ret;
            });
        }
        public DataSet GetDataViewDependencies(string dataviewName) {
            return loginIfNeeded(delegate() {
                var ret = _direct.GetDataViewDependencies(dataviewName);
                return ret;
            });
        }

        public DataSet GetDataViewDefinition(string dataviewName) {
            return loginIfNeeded(delegate() {
                var ret = _direct.GetDataViewDefinition(dataviewName);
                return ret;
            });
        }

        public DataSet GetData(string dataviewName, string delimitedParamList) {
            return loginIfNeeded(delegate() {
                var ret = _direct.GetData(dataviewName, delimitedParamList, 0, 0);
                return ret;
            });
        }

        public DataSet TransferOwnership(DataSet ds, int newOwnerCooperatorID, bool includeDescendents) {
            return loginIfNeeded(delegate() {
                var ret = _direct.TransferOwnership(ds, newOwnerCooperatorID, includeDescendents);
                return ret;
            });
        }

        //public DataSet ResolveUniqueKeys(DataSet ds, string options) {
        //    var ret = _direct.ResolveUniqueKeys(ds, options);
        //    return ret;
        //}

        public DataSet CopyDataViewDefinition(string dataviewName, string copyToDataViewName) {
            return loginIfNeeded(delegate() {
                var ret = _direct.CopyDataViewDefinition(dataviewName, copyToDataViewName);
                clearLocalCache(null);
                //clearLocalCache("dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RenameDataViewDefinition(string dataviewName, string newDataViewName) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RenameDataViewDefinition(dataviewName, newDataViewName);
                clearLocalCache(null);
                //clearLocalCache("dataview");
                clearWebCache(null);
                return ret;
            });
        }

        //public DataSet CreateDataViewDefinition(string dataviewName, bool readOnly, bool suppressProps, bool isSystem, bool userVisible, bool webVisible, string formAssemblyName, string formFullyQualifiedName, string executableName, string categoryName, string databaseArea, int databaseAreaSortOrder, bool transformable, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, DataSet dsInfo) {
        public DataSet CreateDataViewDefinition(string dataviewName, bool readOnly, string categoryName, string databaseArea, int databaseAreaSortOrder, bool transformable, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, string configurationOptions, DataSet dsInfo) {
            //var ret = _direct.CreateDataViewDefinition(dataviewName, readOnly, suppressProps, isSystem, userVisible, webVisible, formAssemblyName, formFullyQualifiedName, executableName, categoryName, databaseArea, databaseAreaSortOrder, transformable, transformFieldForNames, transformFieldForCaptions, transformFieldForValues, dsInfo);
            return loginIfNeeded(delegate() {
                var ret = _direct.CreateDataViewDefinition(dataviewName, readOnly, categoryName, databaseArea, databaseAreaSortOrder, transformable, transformFieldForNames, transformFieldForCaptions, transformFieldForValues, configurationOptions, dsInfo);
                clearLocalCache(null);
                //clearLocalCache("dataview");
                clearWebCache(null);
                return ret;
            });
        }

        //public DataSet GetData(string dataviewNameOrSql, string delimitedParameterList, int offset, int limit) {
        //    if (_actual != null) {
        //        return _actual.GetData(false, Connection.GrinGlobalLoginToken, dataviewNameOrSql, delimitedParameterList, offset, limit);
        //    } else {
        //        return _direct.GetData(dataviewNameOrSql, delimitedParameterList, offset, limit);
        //    }
        //}

        //public string GetDataJson(string dataviewNameOrSql, string delimitedParameterList, int offset, int limit) {
        //    if (_actual != null) {
        //        return _actual.GetDataJson(false, Connection.GrinGlobalLoginToken, dataviewNameOrSql, delimitedParameterList, offset, limit);
        //    } else {
        //        return _direct.GetDataJson(dataviewNameOrSql, delimitedParameterList, offset, limit);
        //    }
        //}

        //public DataSet GetDataParameterTemplate(string dataviewName) {
        //    if (_actual != null) {
        //        return _actual.GetDataParameterTemplate(false, Connection.GrinGlobalLoginToken, dataviewName);
        //    } else {
        //        return _direct.GetDataParameterTemplate(dataviewName);
        //    }
        //}

        public bool ClearCache(string cacheName) {
            bool rv = false;
            loginIfNeeded(delegate() {
                _direct.ClearCache(cacheName, false);
                clearLocalCache(cacheName);
                rv = clearWebCache(cacheName);
                return null;
            });
            return rv;
        }

        //public DataSet LogUsage(string usageData) {
        //    if (_actual != null) {
        //        return _actual.LogUsage(false, Connection.GrinGlobalLoginToken, usageData);
        //    } else {
        //        return _direct.LogUsage(false, Connection.GrinGlobalLoginToken, usageData);
        //    }
        //}

        //public DataSet Search(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit) {
        //    if (_actual != null) {
        //        return _actual.Search(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit);
        //    } else {
        //        return _direct.Search(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit);
        //    }
        //}

        //public DataSet SearchThenGetData(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName) {
        //    if (_actual != null) {
        //        return _actual.SearchThenGetData(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
        //    } else {
        //        return _direct.SearchThenGetData(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
        //    }
        //}

        //public string SearchThenGetDataJson(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName) {
        //    if (_actual != null) {
        //        return _actual.SearchThenGetDataJson(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
        //    } else {
        //        return _direct.SearchThenGetDataJson(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
        //    }
        //}

        //public DataSet CreateUser(string newUserUserName, string newUserPassword, bool newUserEnabled) {
        //    if (_actual != null) {
        //        return _actual.CreateUser(false, Connection.GrinGlobalLoginToken, newUserUserName, newUserPassword, newUserEnabled);
        //    } else {
        //        return _direct.CreateUser(false, Connection.GrinGlobalLoginToken, newUserUserName, newUserPassword, newUserEnabled);
        //    }
        //}

        public string Login() {
            return Login(Connection.GrinGlobalUserName, Connection.GrinGlobalHashedPassword);
        }

        public string Login(string userName, string password) {
            if (!IsValid) {
                return null;
            } else {
                return AdminData.Login(userName, password, _direct.DataConnectionSpec);
            }
        }

        public DataSet ChangeWebPassword(string webUserName, string newWebPassword) {
            return loginIfNeeded(delegate() {
                return _direct.ChangeWebPassword(webUserName, newWebPassword);
            });
        }

        public DataSet ChangePassword(int targetUserID, string newPassword) {
            return loginIfNeeded(delegate() {
                return _direct.ChangePassword(targetUserID, newPassword);
            });
        }

        public DataSet ListResources(string appName, int languageID) {
            return loginIfNeeded(delegate() {
                return _direct.ListResources(appName, languageID);
            });
        }

        //public DataSet RenameList(string existingGroupName, string newGroupName, string existingTabName, string newTabName, int cooperatorID) {
        //    if (_actual != null) {
        //        return _actual.RenameList(false, Connection.GrinGlobalLoginToken, existingGroupName, newGroupName, existingTabName, newTabName, cooperatorID);
        //    } else {
        //        return _direct.RenameList(false, Connection.GrinGlobalLoginToken, existingGroupName, newGroupName, existingTabName, newTabName, cooperatorID);
        //    }
        //}

        //public DataSet RenameTab(string existingTabName, string newTabName, int cooperatorID) {
        //    if (_actual != null) {
        //        return _actual.RenameTab(false, Connection.GrinGlobalLoginToken, existingTabName, newTabName, cooperatorID);
        //    } else {
        //        return _direct.RenameTab(false, Connection.GrinGlobalLoginToken, existingTabName, newTabName, cooperatorID);
        //    }
        //}

        //public DataSet ChangeLanguage(int newLanguageID) {
        //    if (_actual != null) {
        //        return _actual.ChangeLanguage(false, Connection.GrinGlobalLoginToken, newLanguageID);
        //    } else {
        //        return _direct.ChangeLanguage(false, Connection.GrinGlobalLoginToken, newLanguageID);
        //    }
        //}

        //public string GetVersion() {
        //    if (_actual != null) {
        //        return _actual.GetVersion();
        //    } else {
        //        return _direct.GetVersion();
        //    }
        //}

        //public DataSet DeleteList(string groupName, string tabName, int cooperatorID) {
        //    if (_actual != null) {
        //        return _actual.DeleteList(false, Connection.GrinGlobalLoginToken, groupName, tabName, cooperatorID);
        //    } else {
        //        return _direct.DeleteList(false, Connection.GrinGlobalLoginToken, groupName, tabName, cooperatorID);
        //    }
        //}

        //public DataSet SaveDataSet(DataSet ds) {
        //    if (_actual != null) {
        //        return _actual.SaveDataSet(false, Connection.GrinGlobalLoginToken, ds);
        //    } else {
        //        return _direct.SaveDataSet(false, Connection.GrinGlobalLoginToken, ds);
        //    }
        //}

        public DataSet ImportDataViewDefinitions(DataSet dsDefintions, List<string> dataviewNamesToImport, bool ignoreMissingFieldMappings, bool includeFieldsAndParameters, bool includeLanguageData, bool useInTableMappings) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ImportDataViewDefinitions(dsDefintions, dataviewNamesToImport, ignoreMissingFieldMappings, includeFieldsAndParameters, includeLanguageData, useInTableMappings);
                clearLocalCache(null);
                //clearLocalCache("dataview", "table", "field");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet ExportDataViewDefinitions(List<string> dataviewNamesToExport) {
            return loginIfNeeded(delegate() {
                return _direct.ExportDataViewDefinitions(dataviewNamesToExport);
            });
        }

        public string TestDatabaseConnection() {
            return AdminData.TestDatabaseConnection(_direct.DataConnectionSpec);
        }

        public DataSet DeleteUser(int userID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteUser(userID);
                clearLocalCache(null);
                //clearLocalCache("user");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DisableUser(int userID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DisableUser(userID);
                clearLocalCache(null);
                //clearLocalCache("user");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet EnableUser(int userID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.EnableUser(userID);
                clearLocalCache(null);
                //clearLocalCache("user");
                clearWebCache(null);
                return ret;
            });
        }

        public List<string> GetRoles(int userID) {
            List<string> rv = null;
            loginIfNeeded(delegate() {
                rv = _direct.GetRoles(Connection.GrinGlobalUserName, userID);
                return null;
            });
            return rv;
        }

        public DataSet GetCooperatorInfo(int cooperatorID) {
            return loginIfNeeded(delegate() {
                return _direct.GetCooperatorInfo(cooperatorID);
            });
        }

        public DataSet GetWebCooperatorInfo(int webCooperatorID) {
            return loginIfNeeded(delegate() {
                return _direct.GetWebCooperatorInfo(webCooperatorID);
            });
        }

        public DataSet SearchCooperators(string query) {
            return loginIfNeeded(delegate() {
                return _direct.SearchCooperators(query);
            });
        }

        public DataSet SearchWebCooperators(string query) {
            return loginIfNeeded(delegate() {
                return _direct.SearchWebCooperators(query);
            });
        }

        public DataSet SearchGeographies(string query) {
            return loginIfNeeded(delegate() {
                return _direct.SearchGeographies(query);
            });
        }

        public DataSet GetUserInfo(int userID) {
            return loginIfNeeded(delegate() {
                return _direct.GetUserInfo(userID);
            });
        }


        public DataSet ListUsers(int userID) {
            return loginIfNeeded(delegate() {
                return _direct.ListUsers(userID);
            });
        }

        public DataSet ListPermissions(int permID, List<int> excludePermIDs, bool enabledOnly) {
            return loginIfNeeded(delegate() {
                return _direct.ListPermissions(permID, excludePermIDs, enabledOnly);
            });
        }






        public DataSet DeletePermission(int permID, bool forceDelete) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeletePermission(permID, forceDelete);
                clearLocalCache(null);
                //clearLocalCache("permission");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DisablePermission(int permID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DisablePermission(permID);
                clearLocalCache(null);
                //clearLocalCache("permission");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet EnablePermission(int permID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.EnablePermission(permID);
                clearLocalCache(null);
                //clearLocalCache("permission");
                clearWebCache(null);
                return ret;
            });
        }

        public int SavePermission(int permID, string code, string name, string description, int dataviewID, int tableID, string createPerm, string readPerm, string updatePerm, string deletePerm, bool isEnabled) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SavePermission(permID, code, name, description, dataviewID, tableID, createPerm, readPerm, updatePerm, deletePerm, isEnabled);
                clearLocalCache(null);
                //clearLocalCache("permission");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public DataSet DeletePermissionField(int permFieldID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeletePermissionField(permFieldID);
                clearLocalCache(null);
                //clearLocalCache("permission");
                clearWebCache(null);
                return ret;
            });
        }


        public DataSet ListLookupDataViews() {
            return loginIfNeeded(delegate() {
                return _direct.ListLookupDataViews();
            });
        }

        public DataSet ListDataViews(int dataviewID, string fromCategory, bool orderByCategory, string fromDatabaseArea, bool orderByDatabaseArea) {
            return loginIfNeeded(delegate() {
                return _direct.ListDataViews(dataviewID, fromCategory, orderByCategory, fromDatabaseArea, orderByDatabaseArea);
            });
        }

        public DataSet ListTables(int tableID) {
            return loginIfNeeded(delegate() {
                return _direct.ListTables(tableID);
            });
        }

        public DataSet ListTables(string tableName) {
            return loginIfNeeded(delegate() {
                return _direct.ListTables(tableName);
            });
        }

        public DataSet ListTables(int dataviewID, int tableID) {
            return loginIfNeeded(delegate() {
                return _direct.ListTables(dataviewID, tableID);
            });
        }

        public DataSet ListEffectivePermissions(int userID, string dataviewName, string tableName) {
            return loginIfNeeded(delegate() {
                return _direct.ListEffectivePermissions(userID, dataviewName, tableName);
            });
        }

        public DataSet AddPermissionsToUser(int userID, List<int> permIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.AddPermissionsToUser(userID, permIDs);
                clearLocalCache(null);
                //clearLocalCache("permission", "user");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RemovePermissionsFromUser(int userID, List<int> permIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RemovePermissionsFromUser(userID, permIDs);
                clearLocalCache(null);
                //clearLocalCache("permission", "user");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet ApplyPermissionTemplatesToUser(int userID, List<int> permTemplateIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ApplyPermissionTemplatesToUser(userID, permTemplateIDs);
                clearLocalCache(null);
                //clearLocalCache("permissiontemplate", "permission", "user");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet ListPermissionFields(int permID, int permFieldID) {
            return loginIfNeeded(delegate() {
                return _direct.ListPermissionFields(permID, permFieldID);
            });
        }

        public DataSet ListRelatedTables(int dataviewID, int tableID, int parentTableFieldID, string[] relationshipTypes) {
            return loginIfNeeded(delegate() {
                return _direct.ListRelatedTables(dataviewID, tableID, parentTableFieldID, relationshipTypes);
            });
        }

        public DataSet ListTableFields(int tableID, int tableFieldID, bool includeRelationshipAndIndexData) {
            return loginIfNeeded(delegate() {
                return _direct.ListTableFields(tableID, tableFieldID, includeRelationshipAndIndexData);
            });
        }

        public DataSet ListTableFields(int tableID, int tableFieldID, bool onlyPrimaryKeyFields, bool onlyForeignKeyFields) {
            return loginIfNeeded(delegate() {
                return _direct.ListTableFields(tableID, tableFieldID, onlyPrimaryKeyFields, onlyForeignKeyFields);
            });
        }

        public int SaveTableRelationship(int relationshipID, int fromFieldID, string relationshipTypeCode, int toFieldID) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveTableRelationship(relationshipID, fromFieldID, relationshipTypeCode, toFieldID);
                clearLocalCache(null);
                //clearLocalCache("table", "relationship", "dataview");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public DataSet ListTableRelationships(int fromTableID, int relationshipID) {
            return loginIfNeeded(delegate() {
                return _direct.ListTableRelationships(fromTableID, relationshipID);
            });
        }

        public DataSet ListDataViewFields(int dataviewID, int dataviewFieldID) {
            return loginIfNeeded(delegate() {
                return _direct.ListDataViewFields(dataviewID, dataviewFieldID);
            });
        }

        public DataSet ListDataViewCategories() {
            return loginIfNeeded(delegate() {
                return _direct.ListDataviewCategories();
            });
        }

        public DataSet ListDataViewDatabaseAreas() {
            return loginIfNeeded(delegate() {
                return _direct.ListDataviewDatabaseAreas();
            });
        }

        public int SavePermissionField(int permFieldID, int permID, int dataviewFieldID, int tableFieldID, 
            string valueType, string compareOperator, string value,
            int parentTableFieldID, string parentValueType, string parentCompareOperator, string parentValue,
            string compareMode) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SavePermissionField(permFieldID, permID, dataviewFieldID, tableFieldID, valueType, compareOperator, value, parentTableFieldID, parentValueType, parentCompareOperator, parentValue, compareMode);
                clearLocalCache(null);
                //clearLocalCache("permission", "field");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public int SaveUser(int userID, string userName, bool enabled, int cooperatorID, int currentCooperatorID, int webCooperatorID, string title, string firstName, string lastName, string job,
            string discipline, string organization, string organizationAbbreviation, int languageID, bool isActive, string addressLine1, string addressLine2, string addressLine3, string admin1, string admin2, string email,
            string primaryPhone, string secondaryPhone, string fax, int siteID, string regionCode, string categoryCode, int geographyID, string note){

            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveUser(userID, userName, enabled, cooperatorID, currentCooperatorID, webCooperatorID, title, firstName, lastName, job,
                         discipline, organization, organizationAbbreviation, languageID, isActive, addressLine1, addressLine2, addressLine3, admin1, admin2, email,
                         primaryPhone, secondaryPhone, fax, siteID, regionCode, categoryCode, geographyID, note);
                clearLocalCache(null);
                //clearLocalCache("user");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public DataSet ListSites(string siteCode) {
            return loginIfNeeded(delegate() {
                return _direct.ListSites(siteCode);
            });
        }

        public DataSet ListGeographies(int geographyID) {
            return loginIfNeeded(delegate() {
                return _direct.ListGeographies(geographyID);
            });
        }

        public DataSet ListLanguages(int languageID) {
            return loginIfNeeded(delegate() {
                return _direct.ListLanguages(languageID);
            });
        }

        public void RenameCodeGroup(string originalGroupName, string newGroupName) {
            loginIfNeeded(delegate() {
                _direct.RenameCodeGroup(originalGroupName, newGroupName);
                clearLocalCache(null);
                //clearLocalCache("code");
                clearWebCache(null);
                return null;
            });
        }
        public int SaveCodeValue(int ID, string groupName, string value, DataTable dtLang, List<string> touchedTables) {
            int rv = -1;
            loginIfNeeded(delegate() {
                var ret = _direct.SaveCodeValue(ID, groupName, value, dtLang, touchedTables);
                clearLocalCache(null);
                //clearLocalCache("code");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public DataSet ListCodeValues(string groupName, int? valueID, int? languageID) {
            return loginIfNeeded(delegate() {
                return _direct.ListCodeValues(groupName, valueID, languageID);
            });
        }

        public DataSet ListTablesAndDataviewsByCodeGroup(string groupName) {
            return loginIfNeeded(delegate() {
                return _direct.ListTablesAndDataviewsByCodeGroup(groupName);
            });
        }

        public void DeleteCodeValue(int codeValueId, string groupName) {
            loginIfNeeded(delegate() {
                _direct.DeleteCodeValue(codeValueId, groupName);
                return null;
            });
        }

        public int GetCodeValueUsageCount(string value, string tableName, string fieldName) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.GetCodeValueUsageCount(value, tableName, fieldName);
                return null;
            });
            return rv;
        }

        public void DeleteCodeGroup(string groupName) {
            loginIfNeeded(delegate() {
                _direct.DeleteCodeGroup(groupName);
                return null;
            });
        }
        public DataSet ListCodeGroups(string groupName) {
            return loginIfNeeded(delegate() {
                return _direct.ListCodeGroups(groupName);
            });
        }



        public int GetGroupIDForTag(string tag) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.GetSysGroupIDForTag(tag);
                return null;
            });
            return rv;
        }

        public bool IsReservedGroupTagValue(string value) {
            bool rv = false;
            loginIfNeeded(delegate() {
                rv = _direct.IsReservedGroupTagValue(value);
                return null;
            });
            return rv;
        }

        public DataSet ListGroups(int groupID) {
            return loginIfNeeded(delegate() {
                return _direct.ListGroups(groupID);
            });
        }

        public DataSet DeleteGroup(int groupID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteGroup(groupID);
                clearLocalCache(null);
                //clearLocalCache("group");
                clearWebCache(null);
                return ret;
            });
        }

        public int SaveGroup(int groupID, string code, string name, string description) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveGroup(groupID, code, name, description);
                clearLocalCache(null);
                //clearLocalCache("group");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public DataSet AddPermissionsToGroup(int groupID, List<int> permIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.AddPermissionsToGroup(groupID, permIDs);
                clearLocalCache(null);
                //clearLocalCache("group", "permission");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RemovePermissionsFromGroup(int groupID, List<int> permIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RemovePermissionsFromGroup(groupID, permIDs);
                clearLocalCache(null);
                //clearLocalCache("group", "permission");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet AddUsersToGroup(int groupID, List<int> userIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.AddUsersToGroup(groupID, userIDs);
                clearLocalCache(null);
                //clearLocalCache("group", "user");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RemoveUsersFromGroup(int groupID, List<int> userIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RemoveUsersFromGroup(groupID, userIDs);
                clearLocalCache(null);
                //clearLocalCache("group", "user");
                clearWebCache(null);
                return ret;
            });
        }



        public DataSet ListPermissionTemplates(int permTemplateID) {
            return loginIfNeeded(delegate() {
                return _direct.ListPermissionTemplates(permTemplateID);
            });
        }

        public DataSet DeletePermissionTemplate(int permTemplateID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeletePermissionTemplate(permTemplateID);
                clearLocalCache(null);
                //clearLocalCache("permissiontemplate");
                clearWebCache(null);
                return ret;
            });
        }

        public int SavePermissionTemplate(int permTemplateID, string name, string description) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SavePermissionTemplate(permTemplateID, name, description);
                clearLocalCache(null);
                //clearLocalCache("permissiontemplate");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public DataSet AddPermissionsToTemplate(int permTemplateID, List<int> permIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.AddPermissionsToTemplate(permTemplateID, permIDs);
                clearLocalCache(null);
                //clearLocalCache("permissiontemplate", "permission");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RemovePermissionsFromTemplate(int permTemplateID, List<int> permIDs) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RemovePermissionsFromTemplate(permTemplateID, permIDs);
                clearLocalCache(null);
                //clearLocalCache("permissiontemplate", "permission");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet ListTableNames(string schemaName)
        {
            return loginIfNeeded(delegate() {
                return _direct.ListTableNames(schemaName);
            });
        }

        public ITable MapTable(string tableName) {
            ITable rv = null;
            loginIfNeeded(delegate() {
                rv = Table.Map(tableName, _direct.DataConnectionSpec, _direct.LanguageID, true); // brock
                return null;
            });
            return rv;
        }

        public DataSet RecreateTableMappings(List<string> tableNames){
            return loginIfNeeded(delegate() {
                var ret = _direct.RecreateTableMappings(tableNames);
                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RecreateTableRelationships(List<string> tableNames) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RecreateTableRelationships(tableNames);
                clearLocalCache(null);
                //clearLocalCache("table", "relationship", "dataview", "field");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DeleteOrphanedMappings() {
            return loginIfNeeded(delegate() {
                var ds = _direct.ListOrphanedMappings();

                var missing = ds.Tables["list_orphaned_mappings"];
                foreach (DataRow dr in missing.Rows) {
                    _direct.DeleteTableMapping(Toolkit.ToInt32(dr["sys_table_id"], -1), true);
                }

                clearLocalCache(null);
                //clearLocalCache("table", "relationship", "dataview");
                clearWebCache(null);
                return ds;
            });
        }

        public DataSet RecreateTableIndexes(List<string> tableNames) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RecreateTableIndexes(tableNames);
                clearLocalCache(null);
                //clearLocalCache("table", "index");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet GetSearchEngineInfo(bool enabledOnly) {
            return loginIfNeeded(delegate() {
                return _direct.GetSearchEngineInfo(enabledOnly, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet GetSearchEngineInfoEx(bool enabledOnly, string onlyThisIndex, string onlyThisResolver) {
            return loginIfNeeded(delegate() {
                return _direct.GetSearchEngineInfoEx(enabledOnly, onlyThisIndex, onlyThisResolver, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet GetSearchEngineStatus() {
            return loginIfNeeded(delegate() {
                return _direct.GetSearchEngineStatus(Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet SaveSearchEngineResolver(DataSet ds) {
            return loginIfNeeded(delegate() {
                return _direct.SaveSearchEngineResolver(ds, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet SaveSearchEngineIndexer(DataSet ds) {
            return loginIfNeeded(delegate() {
                return _direct.SaveSearchEngineIndexer(ds, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet SaveSearchEngineIndex(DataSet ds) {
            return loginIfNeeded(delegate() {
                return _direct.SaveSearchEngineIndex(ds, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet GetSearchEngineLog() {
            return loginIfNeeded(delegate() {
                return _direct.GetSearchEngineLog(Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet DisableSearchEngineResolver(string indexName, string resolverName) {
            return loginIfNeeded(delegate() {
                return _direct.DisableSearchEngineResolver(indexName, resolverName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet EnableSearchEngineResolver(string indexName, string resolverName) {
            return loginIfNeeded(delegate() {
                return _direct.EnableSearchEngineResolver(indexName, resolverName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet RebuildSearchEngineIndex(string indexName) {
            return loginIfNeeded(delegate() {
                return _direct.RebuildSearchEngineIndex(indexName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet RebuildAllSearchEngineIndexes() {
            return loginIfNeeded(delegate() {
                return _direct.RebuildAllEnabledSearchEngineIndexes(Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public List<string> VerifySearchEngineIndexes(List<string> indexNames) {
            List<string> rv = null;
            loginIfNeeded(delegate() {
                rv = _direct.VerifySearchEngineIndexes(indexNames, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
                return null;
            });
            return rv;
        }

        public void ReloadSearchEngineIndexes(List<string> indexNames) {
            loginIfNeeded(delegate() {
                _direct.ReloadSearchEngineIndexes(indexNames, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
                return null;
            });
        }

        public DataSet DefragSearchEngineIndex(string indexName) {
            return loginIfNeeded(delegate() {
                return _direct.DefragSearchEngineIndex(indexName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet DeleteSearchEngineIndex(string indexName) {
            return loginIfNeeded(delegate() {
                return _direct.DeleteSearchEngineIndexes(indexName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet EnableSearchEngineIndex(string indexName) {
            return loginIfNeeded(delegate() {
                return _direct.EnableSearchEngineIndex(indexName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet DisableSearchEngineIndex(string indexName) {
            return loginIfNeeded(delegate() {
                return _direct.DisableSearchEngineIndex(indexName, Connection.SearchEngineBindingType, Connection.SearchEngineBindingUrl);
            });
        }

        public DataSet DeleteTableMapping(int tableID, bool forceDelete) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteTableMapping(tableID, forceDelete);
                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DisableTableMapping(int tableID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleTableMappingEnabled(tableID, false);
                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet EnableTableMapping(int tableID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleTableMappingEnabled(tableID, true);
                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet ListUnmappedTables() {
            return loginIfNeeded(delegate() {
                return _direct.ListUnmappedTables();
            });
        }

        public DataSet DeleteTableRelationship(int relationshipID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteTableRelationship(relationshipID);
                clearLocalCache(null);
                //clearLocalCache("table", "relationship", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DeleteTableIndex(int indexID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteTableIndex(indexID);
                clearLocalCache(null);
                //clearLocalCache("table", "index", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DeleteTableFieldMapping(int fieldID, bool forceDelete) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteTableFieldMapping(fieldID, forceDelete);
                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public int UpdateTableMapping(int tableID, string databaseArea, bool enabled) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.UpdateTableMapping(tableID, databaseArea, enabled);
                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return null;
            });
            return rv;
        }

        public int SaveTableFieldMapping(int fieldID, int tableID, string fieldName, string purpose, string type, string defaultValue, bool isPrimaryKey,
            bool isForeignKey, int foreignKeyTableFieldID, string foreignKeyDataview, bool isNullable, string guiHint, bool isReadOnly,
            int minLength, int maxLength, int precision, int scale, bool autoIncrement, string codeGroupCode, DataTable languageInfo) {

            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveTableFieldMapping(fieldID, tableID, fieldName, purpose, type, defaultValue, isPrimaryKey,
                isForeignKey, foreignKeyTableFieldID, foreignKeyDataview, isNullable, guiHint, isReadOnly,
                minLength, maxLength, precision, scale, autoIncrement, codeGroupCode, languageInfo);

                clearLocalCache(null);
                //clearLocalCache("table", "field", "dataview");
                clearWebCache(null);
                return null;
            });
            return rv;
        }


        public DataConnectionSpec GetDataConnectionSpec() {
            return _direct.DataConnectionSpec;
        }

        public DataSet TestDataview(string sql, List<IDataviewParameter> parameters, string dataviewName, int languageID, int offset, int limit) {
            return loginIfNeeded(delegate() {
                return _direct.GetData(sql, parameters, dataviewName, languageID, offset, limit);
            });
        }

        /// <summary>
        /// Verifies the dataview by specifying default values for parameters.  Returns error string on error, otherwise null.
        /// </summary>
        /// <param name="dataviewName"></param>
        /// <returns></returns>
        public string VerifyDataview(string dataviewName, bool unescapeWhereClause) {
            string rv = null;
            loginIfNeeded(delegate() {

                var ds = _direct.GetDataViewDefinition(dataviewName);
                DataTable dt = ds.Tables["sys_dataview"];
                if (dt.Rows.Count > 0) {

                    int dvID = Toolkit.ToInt32(dt.Rows[0]["sys_dataview_id"], -1);
                    string paramList = null;
                    DataSet dsParam = _direct.GetData("get_dataview_parameters", ":dataview=" + dataviewName, 0, 0);
                    var issueWarning = false;
                    foreach (DataRow drParam in dsParam.Tables["get_dataview_parameters"].Rows) {
                        if (drParam["param_type"].ToString() == "STRINGREPLACEMENT") {
                            paramList += drParam["param_name"] + "=;";
                            issueWarning = true;
                        } else {
                            paramList += drParam["param_name"] + "=0;";
                        }
                    }

                    try {
                        var dtGet = _direct.GetData(dataviewName, paramList, 0, 1, "unescapewhereclause=" + unescapeWhereClause).Tables[dataviewName];
                        
                        // we get here, the sql worked.
                        // make sure we field definitions match columns in both number and name.
                        // However, if this dataview is transformed, we cannot perform this check.
                        if (dt.Rows[0]["is_transform"].ToString().ToUpper() == "Y") {
                            issueWarning = true;
                            throw new InvalidOperationException("This dataview has transform enabled so field and column mismatches cannot be verified.");
                        } else {
                            var dtFields = ds.Tables["dv_field_info"];

                            var sbMissingColumns = new StringBuilder();
                            var sbMissingFields = new StringBuilder();

                            var invalid = false;
                            var sb = new StringBuilder();
                            if (dtFields.Rows.Count < dtGet.Columns.Count) {
                                sb.Append("SQL emits more columns than the number of defined dataview fields.");
                                invalid = true;
                            } else if (dtFields.Rows.Count > dtGet.Columns.Count) {
                                sb.Append("SQL emits fewer columns than the number of defined dataview fields.");
                                invalid = true;
                            } else {
                                // mark the text to assume it's wrong, but don't set the invalid flag because we don't know yet.
                                sb.Append("SQL columns and defined dataview fields are mismatched.");
                            }

                            // determine which columns are missing
                            var foundColumns = new List<string>();
                            foreach (DataRow drF in dtFields.Rows) {
                                var fieldName = drF["dv_field_name"].ToString().ToLower();
                                if (!dtGet.Columns.Contains(fieldName)) {
                                    sbMissingColumns.Append(fieldName + ", ");
                                    invalid = true;
                                } else {
                                    foundColumns.Add(fieldName);
                                }
                            }

                            // determine which fields are missing
                            foreach (DataColumn dc in dtGet.Columns) {
                                var colName = dc.ColumnName.ToLower();
                                if (!foundColumns.Contains(colName)) {
                                    sbMissingFields.Append(colName + ", ");
                                    invalid = true;
                                }
                            }

                            // generate an error and format the message somewhat nicely
                            if (invalid) {
                                if (sbMissingFields.Length > 2) {
                                    sbMissingFields.Length -= 2;
                                    sb.Append("  Missing dataview field definitions: " + sbMissingFields.ToString());
                                }
                                if (sbMissingColumns.Length > 2) {
                                    sbMissingColumns.Length -= 2;
                                    sb.Append("  Missing SQL columns: " + sbMissingColumns.ToString());
                                }

                                throw new InvalidOperationException(sb.ToString());
                            }
                        }

                        rv = null;
                    } catch (Exception ex) {
                        if (issueWarning) {
                            rv = "WARNING: " + ex.Message;
                        } else {
                            rv = "FAILED: " + ex.Message;
                        }
                        //Response.Write("<b><span style='color:red'>FAILED: </span>" + Server.HtmlEncode(ex.Message) + "</b>&nbsp;&nbsp;&nbsp;&nbsp;<a href='#' onclick='javascript:showWindow(\"" + dvName + "\");'>Edit '" + dvName + "'</a><br />");
                        //if (foundStringRelacement) {
                        //    Response.Write("<p><b>NOTE: dataview <span style='color:green'>" + dvName + "</span> contains one or more parameters of type STRINGREPLACEMENT, so a valid default value cannot be automatically determined, meaning it may be valid SQL.  Please inspect it carefully to make sure.</b></p>");
                        //}
                    }
                } else {
                    rv = null;
                }
                return null;
            });
            return rv;
        }

        public void UpconvertDataviewForImport(DataSet ds) {
            loginIfNeeded(delegate() {
                AdminData.UpconvertDataviewForImport(ds);
                return null;
            });
        }

        public bool PingSearchEngine(string bindingType, string bindingUrl) {
            bool rv = false;
            loginIfNeeded(delegate() {
                rv = _direct.PingSearchEngine(bindingType ?? Connection.SearchEngineBindingType, bindingUrl ?? Connection.SearchEngineBindingUrl);
                return null;
            });
            return rv;
        }

        /// <summary>
        /// Returns true if connection was succesfully established, false otherwise
        /// </summary>
        public bool IsValid {
            get {
                return _direct != null;
            }
        }

        #region File Groups / Files


        public DataSet ListFilesByGroup(int groupID, bool assignedToGroup) {
            return loginIfNeeded(delegate() {
                return _direct.ListFilesByGroup(groupID, assignedToGroup);
            });
        }

        public DataSet ListFileGroups(int id) {
            return loginIfNeeded(delegate() {
                return _direct.ListFileGroups(id, false);
            });
        }

        public int SaveFileGroup(int id, string name, string version, bool enabled) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveFileGroup(id, name, version, enabled);
                return null;
            });
            return rv;
        }

        public DataSet ListFiles(int id) {
            return loginIfNeeded(delegate() {
                return _direct.ListFiles(id, false);
            });
        }


        public DataSet DeleteFileGroup(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteFileGroup(id);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet AddFileToGroup(int groupID, int fileID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.AddFileToGroup(groupID, fileID);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet RemoveFileFromGroup(int groupID, int fileID) {
            return loginIfNeeded(delegate() {
                var ret = _direct.RemoveFileFromGroup(groupID, fileID);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DisableFileGroup(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleFileGroupEnabled(id, false);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet EnableFileGroup(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleFileGroupEnabled(id, true);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DisableFile(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleFileEnabled(id, false);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet EnableFile(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleFileEnabled(id, true);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }

        public int SaveFile(int id, string displayName, string fileName, string virtualPath, string physicalPath, bool enabled) {
            // copy file from physical path to IIS virtual path
            var dest = Connection.WebAppPhysicalPath; // Toolkit.GetIISPhysicalPath("gringlobal");

            dest += (@"\" + virtualPath).Replace("~/", @"\").Replace(@"\\", @"\").Replace("/", @"\");

            if (File.Exists(dest)) {
                File.Delete(dest);
            }
            File.Copy(physicalPath, dest);

            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveFile(id, displayName, fileName, virtualPath, dest, enabled);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return null;
            });

            return rv;


        }

        public DataSet DeleteFile(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteFile(id);
                clearLocalCache(null);
                //clearLocalCache("filegroup", "file");
                clearWebCache(null);
                return ret;
            });
        }
        
        #endregion

        #region Triggers
        public DataSet ListTriggers(int id) {
            return loginIfNeeded(delegate() {
                return _direct.ListTriggers(id, false);
            });
        }


        public DataSet DeleteTrigger(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.DeleteTrigger(id);
                clearLocalCache(null);
                //clearLocalCache("trigger", "table", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet DisableTrigger(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleTriggerEnabled(id, false);
                clearLocalCache(null);
                //clearLocalCache("trigger", "table", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public DataSet EnableTrigger(int id) {
            return loginIfNeeded(delegate() {
                var ret = _direct.ToggleTriggerEnabled(id, true);
                clearLocalCache(null);
                //clearLocalCache("trigger", "table", "dataview");
                clearWebCache(null);
                return ret;
            });
        }

        public int SaveTrigger(int triggerID, int dataviewID, int tableID, string virtualFilePath, string assemblyName, string className, string title, string description, bool enabled, bool system) {
            int rv = -1;
            loginIfNeeded(delegate() {
                rv = _direct.SaveTrigger(triggerID, dataviewID, tableID, virtualFilePath, assemblyName, className, title, description, enabled, system);
                clearLocalCache(null);
                //clearLocalCache("trigger", "table", "dataview");
                clearWebCache(null);
                return null;
            });
            return rv;
        }
        #endregion

        #region Application Settings
        public void DeleteConnectionString(string name, string configFilePath) {
            _direct.DeleteConnectionString(name, configFilePath);
        }

        public void SaveConnectionString(string name, string connectionString, string providerName, string configFilePath) {
            _direct.SaveConnectionString(name, connectionString, providerName, configFilePath);
        }

        public void DeleteApplicationSetting(string name, string configFilePath) {
            _direct.DeleteApplicationSetting(name, configFilePath);
        }

        public void SaveApplicationSetting(string name, string value, string configFilePath) {
            _direct.SaveApplicationSetting(name, value, configFilePath);
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (_direct != null) {
                _direct.Dispose();
                _direct = null;
            }
        }

        #endregion

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "AdminHostProxy", resourceName, null, defaultValue, substitutes);
        }
    }
}
