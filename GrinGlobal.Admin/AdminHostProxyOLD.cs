//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Data;
//using GrinGlobal.Core;
//using GrinGlobal.Business;
//using GrinGlobal.Admin.WCFAdminService;
//using System.ServiceModel;
//using System.Diagnostics;

//namespace GrinGlobal.Admin {
//    /// <summary>
//    /// Since having a reference to a dll directly and a reference to the same code via a web service reference result in two completely unrelated interfaces, we have to meld them together in this class.  Its primary/only responsibility is to be the entry point for calling either one of the two interfaces.
//    /// </summary>
//    public class AdminHostProxy : IDisposable {

//        private IAdminService _actual;
//        private SecureData _direct;
//        private ConnectionInfo _conn;
//        public ConnectionInfo Connection {
//            get {
//                return _conn;
//            }
//        }

//        //public static List<string> Scan() {
//        //    var ret = new List<string>();
//        //    GuiSvc.GUI gui = new GuiSvc.GUI();
//        //    for (int i = 0; i < 256; i++) {
//        //        using (HighPrecisionTimer hpt = new HighPrecisionTimer("connect", true)) {
//        //            gui.Url = "http://192.168.0." + i + "/gringlobal/gui.asmx";
//        //            gui.Timeout = 100;
//        //            try {
//        //                gui.ValidateLogin(false, "guest", "guest");
//        //                ret.Add("192.168.0." + i);
//        //            } catch (Exception ex) {
//        //                Debug.WriteLine("elapsed: " + hpt.ElapsedMilliseconds + " ... 192.168.0." + i + " : " + ex.Message);
//        //            }
//        //        }
//        //    }
//        //    return ret;
//        //}


//        /// <summary>
//        /// If the GrinGlobalLoginToken property is empty, creates a proxy object and calls the Login method using the GrinGlobal Username and Password properties to validate them and generate a new token, which is stored in GrinGlobalLoginToken property of the given ConnectionInfo object for future use.
//        /// </summary>
//        /// <returns></returns>
//        public static bool AutoLogin(ConnectionInfo ci) {
//            if (!String.IsNullOrEmpty(ci.GrinGlobalLoginToken)) {
//                // already have a login token
//                return true;
//            } else if (!String.IsNullOrEmpty(ci.GrinGlobalUserName) && !String.IsNullOrEmpty(ci.GrinGlobalPassword)){

//                // we have proper gringlobal login capability.

//                var ahs = new AdminHostProxy(ci);
//                string login = ahs.Login(ci.GrinGlobalUserName, ci.GrinGlobalPassword);
//                if (!String.IsNullOrEmpty(login)) {
//                    ci.GrinGlobalLoginToken = login;
//                    return true;
//                }
//            }

//            return false;
//        }

//        public AdminHostProxy(ConnectionInfo ci) {
//            if (ci.UseWebService) {
//                // use it as a webservice
//                AdminServiceClient asc = new AdminServiceClient();
//                asc.Endpoint.Address = new System.ServiceModel.EndpointAddress(ci.GrinGlobalUrl);
//                _actual = asc;
//            } else {
//                // use it as a direct dll reference (aka connect via database only, no web services involved)
//                var dcs = DataConnectionSpec.Parse(ci.DatabaseEngineProviderName, ci.DatabaseEngineServerName, ci.DatabaseEngineDatabaseName, ci.DatabaseEngineUserName, ci.DatabaseEnginePassword);
//                string token = SecureData.Login(ci.GrinGlobalUserName, ci.GrinGlobalPassword, dcs);
//                var direct = new SecureData(false, token, dcs);
//                _direct = direct;
//            }

//            _conn = ci;

//            if (_actual == null && _direct == null) {
//                throw new InvalidOperationException(EMPTY_OBJECT_MESSAGE);
//            }
//        }


//        private const string EMPTY_OBJECT_MESSAGE = "An object must be given in the constructor for AdminHostProxy that implements either GrinGlobal.Admin.ActualWebService.IWebService or GrinGlobal.Web.IWebService.";

//        public DataSet DeleteDataViewDefinition(string dataviewName) {
//            if (_actual != null) {
//                return _actual.DeleteDataViewDefinition(false, Connection.GrinGlobalLoginToken, dataviewName);
//            } else {
//                return _direct.DeleteDataViewDefinition(dataviewName);
//            }
//        }

//        public DataSet GetDataViewDefinition(string dataviewName) {
//            if (_actual != null) {
//                return _actual.GetDataViewDefinition(false, Connection.GrinGlobalLoginToken, dataviewName);
//            } else {
//                return _direct.GetDataViewDefinition(dataviewName);
//            }
//        }

//        public DataSet CopyDataViewDefinition(string dataviewName, string copyToDataViewName) {
//            if (_actual != null) {
//                return _actual.CopyDataViewDefinition(false, Connection.GrinGlobalLoginToken, dataviewName, copyToDataViewName);
//            } else {
//                return _direct.CopyDataViewDefinition(dataviewName, copyToDataViewName);
//            }
//        }

//        public DataSet RenameDataViewDefinition(string dataviewName, string newDataViewName) {
//            if (_actual != null) {
//                return _actual.RenameDataViewDefinition(false, Connection.GrinGlobalLoginToken, dataviewName, newDataViewName);
//            } else {
//                return _direct.RenameDataViewDefinition(dataviewName, newDataViewName);
//            }
//        }

//        public DataSet CreateDataViewDefinition(string dataviewName, bool readOnly, bool suppressProps, bool isSystem, bool userVisible, string formAssemblyName, string formFullyQualifiedName, string executableName, string categoryName, int categorySortOrder, bool transformable, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, DataSet dsInfo) {
//            if (_actual != null) {
//                return _actual.CreateDataViewDefinition(false, Connection.GrinGlobalLoginToken, dataviewName, readOnly, suppressProps, isSystem, userVisible, formAssemblyName, formFullyQualifiedName, executableName, categoryName, categorySortOrder, transformable, transformFieldForNames, transformFieldForCaptions, transformFieldForValues, dsInfo);
//            } else {
//                return _direct.CreateDataViewDefinition(dataviewName, readOnly, suppressProps, isSystem, userVisible, formAssemblyName, formFullyQualifiedName, executableName, categoryName, categorySortOrder, transformable, transformFieldForNames, transformFieldForCaptions, transformFieldForValues, dsInfo);
//            }
//        }

//        //public DataSet GetData(string dataviewNameOrSql, string delimitedParameterList, int offset, int limit) {
//        //    if (_actual != null) {
//        //        return _actual.GetData(false, Connection.GrinGlobalLoginToken, dataviewNameOrSql, delimitedParameterList, offset, limit);
//        //    } else {
//        //        return _direct.GetData(dataviewNameOrSql, delimitedParameterList, offset, limit);
//        //    }
//        //}

//        //public string GetDataJson(string dataviewNameOrSql, string delimitedParameterList, int offset, int limit) {
//        //    if (_actual != null) {
//        //        return _actual.GetDataJson(false, Connection.GrinGlobalLoginToken, dataviewNameOrSql, delimitedParameterList, offset, limit);
//        //    } else {
//        //        return _direct.GetDataJson(dataviewNameOrSql, delimitedParameterList, offset, limit);
//        //    }
//        //}

//        //public DataSet GetDataParameterTemplate(string dataviewName) {
//        //    if (_actual != null) {
//        //        return _actual.GetDataParameterTemplate(false, Connection.GrinGlobalLoginToken, dataviewName);
//        //    } else {
//        //        return _direct.GetDataParameterTemplate(dataviewName);
//        //    }
//        //}

//        public DataSet ClearCache(string cacheName) {
//            if (_actual != null) {
//                return _actual.ClearCache(false, Connection.GrinGlobalLoginToken, cacheName);
//            } else {
//                return _direct.ClearCache(cacheName);
//            }
//        }

//        //public DataSet LogUsage(string usageData) {
//        //    if (_actual != null) {
//        //        return _actual.LogUsage(false, Connection.GrinGlobalLoginToken, usageData);
//        //    } else {
//        //        return _direct.LogUsage(false, Connection.GrinGlobalLoginToken, usageData);
//        //    }
//        //}

//        //public DataSet Search(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit) {
//        //    if (_actual != null) {
//        //        return _actual.Search(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit);
//        //    } else {
//        //        return _direct.Search(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit);
//        //    }
//        //}

//        //public DataSet SearchThenGetData(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName) {
//        //    if (_actual != null) {
//        //        return _actual.SearchThenGetData(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
//        //    } else {
//        //        return _direct.SearchThenGetData(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
//        //    }
//        //}

//        //public string SearchThenGetDataJson(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName) {
//        //    if (_actual != null) {
//        //        return _actual.SearchThenGetDataJson(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
//        //    } else {
//        //        return _direct.SearchThenGetDataJson(false, Connection.GrinGlobalLoginToken, query, ignoreCase, andTermsTogether, indexList, resolverName, searchOffset, searchLimit, databaseOffset, databaseLimit, dataviewName);
//        //    }
//        //}

//        //public DataSet CreateUser(string newUserUserName, string newUserPassword, bool newUserEnabled) {
//        //    if (_actual != null) {
//        //        return _actual.CreateUser(false, Connection.GrinGlobalLoginToken, newUserUserName, newUserPassword, newUserEnabled);
//        //    } else {
//        //        return _direct.CreateUser(false, Connection.GrinGlobalLoginToken, newUserUserName, newUserPassword, newUserEnabled);
//        //    }
//        //}

//        public string Login() {
//            return Login(Connection.GrinGlobalUserName, Connection.GrinGlobalPassword);
//        }

//        public string Login(string userName, string password) {
//            if (_actual != null) {
//                return _actual.Login(userName, password);
//            } else {
//                return SecureData.Login(userName, password, _direct.DataConnectionSpec);
//            }
//        }

//        public DataSet ChangePassword(int targetUserID, string newPassword) {
//            if (_actual != null) {
//                return _actual.ChangePassword(false, Connection.GrinGlobalLoginToken, targetUserID, newPassword);
//            } else {
//                return _direct.ChangePassword(targetUserID, newPassword);
//            }
//        }

//        //public DataSet RenameList(string existingGroupName, string newGroupName, string existingTabName, string newTabName, int cooperatorID) {
//        //    if (_actual != null) {
//        //        return _actual.RenameList(false, Connection.GrinGlobalLoginToken, existingGroupName, newGroupName, existingTabName, newTabName, cooperatorID);
//        //    } else {
//        //        return _direct.RenameList(false, Connection.GrinGlobalLoginToken, existingGroupName, newGroupName, existingTabName, newTabName, cooperatorID);
//        //    }
//        //}

//        //public DataSet RenameTab(string existingTabName, string newTabName, int cooperatorID) {
//        //    if (_actual != null) {
//        //        return _actual.RenameTab(false, Connection.GrinGlobalLoginToken, existingTabName, newTabName, cooperatorID);
//        //    } else {
//        //        return _direct.RenameTab(false, Connection.GrinGlobalLoginToken, existingTabName, newTabName, cooperatorID);
//        //    }
//        //}

//        //public DataSet ChangeLanguage(int newLanguageID) {
//        //    if (_actual != null) {
//        //        return _actual.ChangeLanguage(false, Connection.GrinGlobalLoginToken, newLanguageID);
//        //    } else {
//        //        return _direct.ChangeLanguage(false, Connection.GrinGlobalLoginToken, newLanguageID);
//        //    }
//        //}

//        //public string GetVersion() {
//        //    if (_actual != null) {
//        //        return _actual.GetVersion();
//        //    } else {
//        //        return _direct.GetVersion();
//        //    }
//        //}

//        //public DataSet DeleteList(string groupName, string tabName, int cooperatorID) {
//        //    if (_actual != null) {
//        //        return _actual.DeleteList(false, Connection.GrinGlobalLoginToken, groupName, tabName, cooperatorID);
//        //    } else {
//        //        return _direct.DeleteList(false, Connection.GrinGlobalLoginToken, groupName, tabName, cooperatorID);
//        //    }
//        //}

//        //public DataSet SaveDataSet(DataSet ds) {
//        //    if (_actual != null) {
//        //        return _actual.SaveDataSet(false, Connection.GrinGlobalLoginToken, ds);
//        //    } else {
//        //        return _direct.SaveDataSet(false, Connection.GrinGlobalLoginToken, ds);
//        //    }
//        //}

//        public DataSet ImportDataViewDefinitions(DataSet dsDefintions, List<string> dataviewNamesToImport) {
//            if (_actual != null) {
//                return _actual.ImportDataViewDefinitions(false, Connection.GrinGlobalLoginToken, dsDefintions, dataviewNamesToImport);
//            } else {
//                return _direct.ImportDataViewDefinitions(dsDefintions, dataviewNamesToImport);
//            }
//        }

//        public DataSet ExportDataViewDefinitions(List<string> dataviewNamesToExport) {
//            if (_actual != null) {
//                return _actual.ExportDataViewDefinitions(false, Connection.GrinGlobalLoginToken, dataviewNamesToExport);
//            } else {
//                return _direct.ExportDataViewDefinitions(dataviewNamesToExport);
//            }
//        }

//        public string TestDatabaseConnection() {
//            if (_actual != null) {
//                return _actual.TestDatabaseConnection();
//            } else {
//                return SecureData.TestDatabaseConnection(_direct.DataConnectionSpec);
//            }
//        }

//        public DataSet DeleteUser(int userID) {
//            if (_actual != null) {
//                return _actual.DeleteUser(false, Connection.GrinGlobalLoginToken, userID);
//            } else {
//                return _direct.DeleteUser(userID);
//            }
//        }

//        public DataSet DisableUser(int userID) {
//            if (_actual != null) {
//                return _actual.DisableUser(false, Connection.GrinGlobalLoginToken, userID);
//            } else {
//                return _direct.DisableUser(userID);
//            }
//        }

//        public DataSet EnableUser(int userID) {
//            if (_actual != null) {
//                return _actual.EnableUser(false, Connection.GrinGlobalLoginToken, userID);
//            } else {
//                return _direct.EnableUser(userID);
//            }
//        }

//        public List<string> GetRoles(int userID) {
//            if (_actual != null) {
//                return _actual.GetRoles(Connection.GrinGlobalLoginToken, userID);
//            } else {
//                return _direct.GetRoles(Connection.GrinGlobalUserName, userID);
//            }
//        }

//        public DataSet GetCooperatorInfo(int cooperatorID) {
//            if (_actual != null) {
//                return _actual.GetCooperatorInfo(false, Connection.GrinGlobalLoginToken, cooperatorID);
//            } else {
//                return _direct.GetCooperatorInfo(cooperatorID);
//            }
//        }

//        public DataSet SearchCooperators(string query) {
//            if (_actual != null) {
//                return _actual.SearchCooperators(false, Connection.GrinGlobalLoginToken, query);
//            } else {
//                return _direct.SearchCooperators(query);
//            }
//        }

//        public DataSet GetUserInfo(int userID) {
//            if (_actual != null) {
//                return _actual.GetUserInfo(false, Connection.GrinGlobalLoginToken, userID);
//            } else {
//                return _direct.GetUserInfo(userID);
//            }
//        }


//        public DataSet ListUsers(int userID) {
//            if (_actual != null) {
//                return _actual.ListUsers(false, Connection.GrinGlobalLoginToken, userID);
//            } else {
//                return _direct.ListUsers(userID);
//            }
//        }

//        public DataSet ListPermissions(int permID, List<int> excludePermIDs, bool enabledOnly) {
//            if (_actual != null) {
//                return _actual.ListPermissions(false, Connection.GrinGlobalLoginToken, permID, excludePermIDs, enabledOnly);
//            } else {
//                return _direct.ListPermissions(permID, excludePermIDs, enabledOnly);
//            }
//        }






//        public DataSet DeletePermission(int permID) {
//            if (_actual != null) {
//                return _actual.DeletePermission(false, Connection.GrinGlobalLoginToken, permID);
//            } else {
//                return _direct.DeletePermission(permID);
//            }
//        }

//        public DataSet DisablePermission(int permID) {
//            if (_actual != null) {
//                return _actual.DisablePermission(false, Connection.GrinGlobalLoginToken, permID);
//            } else {
//                return _direct.DisablePermission(permID);
//            }
//        }

//        public DataSet EnablePermission(int permID) {
//            if (_actual != null) {
//                return _actual.EnablePermission(false, Connection.GrinGlobalLoginToken, permID);
//            } else {
//                return _direct.EnablePermission(permID);
//            }
//        }

//        public int SavePermission(int permID, string name, string description, int dataviewID, int tableID, string createPerm, string readPerm, string updatePerm, string deletePerm, bool isEnabled) {
//            if (_actual != null) {
//                return _actual.SavePermission(false, Connection.GrinGlobalLoginToken, permID, name, description, dataviewID, tableID, createPerm, readPerm, updatePerm, deletePerm, isEnabled);
//            } else {
//                return _direct.SavePermission(permID, name, description, dataviewID, tableID, createPerm, readPerm, updatePerm, deletePerm, isEnabled);
//            }
//        }

//        public DataSet DeletePermissionField(int permFieldID) {
//            if (_actual != null) {
//                return _actual.DeletePermissionField(false, Connection.GrinGlobalLoginToken, permFieldID);
//            } else {
//                return _direct.DeletePermissionField(permFieldID);
//            }
//        }


//        public DataSet ListDataViews(int dataviewID) {
//            if (_actual != null) {
//                return _actual.ListDataViews(false, Connection.GrinGlobalLoginToken, dataviewID);
//            } else {
//                return _direct.ListDataViews(dataviewID);
//            }
//        }

//        public DataSet ListTables(int tableID) {
//            if (_actual != null) {
//                return _actual.ListTables(false, Connection.GrinGlobalLoginToken, tableID);
//            } else {
//                return _direct.ListTables(tableID);
//            }
//        }

//        public DataSet ListEffectivePermissions(int userID, string dataviewName, string tableName) {
//            if (_actual != null) {
//                return _actual.ListEffectivePermissions(false, Connection.GrinGlobalLoginToken, userID, dataviewName, tableName);
//            } else {
//                return _direct.ListEffectivePermissions(userID, dataviewName, tableName);
//            }
//        }

//        public DataSet AddPermissionsToUser(int userID, List<int> permIDs) {
//            if (_actual != null) {
//                return _actual.AddPermissionsToUser(false, Connection.GrinGlobalLoginToken, userID, permIDs);
//            } else {
//                return _direct.AddPermissionsToUser(userID, permIDs);
//            }
//        }

//        public DataSet RemovePermissionsFromUser(int userID, List<int> permIDs) {
//            if (_actual != null) {
//                return _actual.RemovePermissionsFromUser(false, Connection.GrinGlobalLoginToken, userID, permIDs);
//            } else {
//                return _direct.RemovePermissionsFromUser(userID, permIDs);
//            }
//        }

//        public DataSet ApplyPermissionTemplatesToUser(int userID, List<int> permTemplateIDs) {
//            if (_actual != null) {
//                return _actual.ApplyPermissionTemplatesToUser(false, Connection.GrinGlobalLoginToken, userID, permTemplateIDs);
//            } else {
//                return _direct.ApplyPermissionTemplatesToUser(userID, permTemplateIDs);
//            }
//        }

//        public DataSet ListPermissionFields(int permID, int permFieldID) {
//            if (_actual != null) {
//                return _actual.ListPermissionFields(false, Connection.GrinGlobalLoginToken, permID, permFieldID);
//            } else {
//                return _direct.ListPermissionFields(permID, permFieldID);
//            }
//        }

//        public DataSet ListRelatedTables(int dataviewID, int tableID, int parentTableFieldID, string[] relationshipTypes) {
//            if (_actual != null) {
//                return _actual.ListRelatedTables(false, Connection.GrinGlobalLoginToken, dataviewID, tableID, parentTableFieldID, new List<string>(relationshipTypes));
//            } else {
//                return _direct.ListRelatedTables(dataviewID, tableID, parentTableFieldID, relationshipTypes);
//            }
//        }

//        public DataSet ListTableFields(int tableID, int tableFieldID) {
//            if (_actual != null) {
//                return _actual.ListTableFields(false, Connection.GrinGlobalLoginToken, tableID, tableFieldID);
//            } else {
//                return _direct.ListTableFields(tableID, tableFieldID);
//            }
//        }

//        public DataSet ListDataViewFields(int dataviewID, int dataviewFieldID) {
//            if (_actual != null) {
//                return _actual.ListDataViewFields(false, Connection.GrinGlobalLoginToken, dataviewID, dataviewFieldID);
//            } else {
//                return _direct.ListDataViewFields(dataviewID, dataviewFieldID);
//            }
//        }

//        public int SavePermissionField(int permFieldID, int permID, int dataviewFieldID, int tableFieldID, 
//            string valueType, string compareOperator, string value,
//            int parentTableFieldID, string parentValueType, string parentCompareOperator, string parentValue,
//            string compareMode) {
//            if (_actual != null) {
//                return _actual.SavePermissionField(false, Connection.GrinGlobalLoginToken, permFieldID, permID, dataviewFieldID, tableFieldID, valueType, compareOperator, value, parentTableFieldID, parentValueType, parentCompareOperator, parentValue, compareMode);
//            } else {
//                return _direct.SavePermissionField(permFieldID, permID, dataviewFieldID, tableFieldID, valueType, compareOperator, value, parentTableFieldID, parentValueType, parentCompareOperator, parentValue, compareMode);
//            }
//        }

//        public int SaveUser(int userID, string userName, bool enabled, int cooperatorID, int currentCooperatorID, string title, string firstName, string initials, string lastName, string fullName, string job,
//            string discipline, string organization, string organizationCode, int languageID, bool isActive, string addressLine1, string addressLine2, string addressLine3, string admin1, string admin2, string email,
//            string primaryPhone, string secondaryPhone, string fax, string siteCode, string regionCode, string categoryCode, int geographyID, string note){
//            if (_actual != null) {
//                return _actual.SaveUser(false, Connection.GrinGlobalLoginToken, userID, userName, enabled, cooperatorID, currentCooperatorID, title, firstName, initials, lastName, fullName, job,
//                     discipline, organization, organizationCode, languageID, isActive, addressLine1, addressLine2, addressLine3, admin1, admin2, email,
//                     primaryPhone, secondaryPhone, fax, siteCode, regionCode, categoryCode, geographyID, note);
//            } else {
//                return _direct.SaveUser(userID, userName, enabled, cooperatorID, currentCooperatorID, title, firstName, initials, lastName, fullName, job,
//                     discipline, organization, organizationCode, languageID, isActive, addressLine1, addressLine2, addressLine3, admin1, admin2, email,
//                     primaryPhone, secondaryPhone, fax, siteCode, regionCode, categoryCode, geographyID, note);
//            }
//        }

//        public DataSet ListSites(string siteCode) {
//            if (_actual != null) {
//                return _actual.ListSites(false, Connection.GrinGlobalLoginToken, siteCode);
//            } else {
//                return _direct.ListSites(siteCode);
//            }
//        }

//        public DataSet ListGeographies(int geographyID) {
//            if (_actual != null) {
//                return _actual.ListGeographies(false, Connection.GrinGlobalLoginToken, geographyID);
//            } else {
//                return _direct.ListGeographies(geographyID);
//            }
//        }

//        public DataSet ListLanguages(int languageID) {
//            if (_actual != null) {
//                return _actual.ListLanguages(false, Connection.GrinGlobalLoginToken, languageID);
//            } else {
//                return _direct.ListLanguages(languageID);
//            }
//        }












//        public DataSet ListPermissionTemplates(int permTemplateID) {
//            if (_actual != null) {
//                return _actual.ListPermissionTemplates(false, Connection.GrinGlobalLoginToken, permTemplateID);
//            } else {
//                return _direct.ListPermissionTemplates(permTemplateID);
//            }
//        }

//        public DataSet DeletePermissionTemplate(int permTemplateID) {
//            if (_actual != null) {
//                return _actual.DeletePermissionTemplate(false, Connection.GrinGlobalLoginToken, permTemplateID);
//            } else {
//                return _direct.DeletePermissionTemplate(permTemplateID);
//            }
//        }

//        public int SavePermissionTemplate(int permTemplateID, string name, string description) {
//            if (_actual != null) {
//                return _actual.SavePermissionTemplate(false, Connection.GrinGlobalLoginToken, permTemplateID, name, description);
//            } else {
//                return _direct.SavePermissionTemplate(permTemplateID, name, description);
//            }
//        }

//        public DataSet AddPermissionsToTemplate(int permTemplateID, List<int> permIDs) {
//            if (_actual != null) {
//                return _actual.AddPermissionsToTemplate(false, Connection.GrinGlobalLoginToken, permTemplateID, permIDs);
//            } else {
//                return _direct.AddPermissionsToTemplate(permTemplateID, permIDs);
//            }
//        }

//        public DataSet RemovePermissionsFromTemplate(int permTemplateID, List<int> permIDs) {
//            if (_actual != null) {
//                return _actual.RemovePermissionsFromTemplate(false, Connection.GrinGlobalLoginToken, permTemplateID, permIDs);
//            } else {
//                return _direct.RemovePermissionsFromTemplate(permTemplateID, permIDs);
//            }
//        }

//        public DataSet ListTableNames(string schemaName)
//        {
//            if (_actual != null)
//            {
//                return _actual.ListTableNames(false, Connection.GrinGlobalLoginToken, schemaName);
//            }
//            else
//            {
//                return _direct.ListTableNames(schemaName);
//            }
//        }

//        public DataSet RecreateTableMappings(List<string> tableNames)
//        {
//            if (_actual != null)
//            {
//                return _actual.RecreateTableMappings(false, Connection.GrinGlobalLoginToken, tableNames);
//            }
//            else
//            {
//                return _direct.RecreateTableMappings(tableNames);
//            }
//        }

//        public DataSet ListSearchEngineIndexes(string indexName, List<int> excludeIndexNames, bool enabledOnly) {
//            if (_actual != null) {
//                return null; // _actual.ListSearchEngineIndexes(false, Connection.GrinGlobalLoginToken, tableNames);
//            } else {
//                return _direct.ListSearchEngineIndexes(indexName, excludeIndexNames, enabledOnly);
//            }
//        }

//        public DataSet DeleteTableMapping(int tableID) {
//            if (_actual != null) {
//                return null; // _direct.DeleteTableMapping(tableID);
//            } else {
//                return _direct.DeleteTableMapping(tableID);
//            }
//        }

//        public DataSet DisableTableMapping(int tableID) {
//            if (_actual != null) {
//                return null; // _direct.DeleteTableMapping(tableID);
//            } else {
//                return _direct.ToggleTableMappingEnabled(tableID, false);
//            }
//        }

//        public DataSet EnableTableMapping(int tableID) {
//            if (_actual != null) {
//                return null; // _direct.DeleteTableMapping(tableID);
//            } else {
//                return _direct.ToggleTableMappingEnabled(tableID, true);
//            }
//        }

//        public DataSet ListUnmappedTables() {
//            if (_actual != null) {
//                return null; // _direct.DeleteTableMapping(tableID);
//            } else {
//                return _direct.ListUnmappedTables();
//            }
//        }

//        public DataSet ListTableFields(int tableID) {
//            if (_actual != null) {
//                return null; // _direct.ListTableFields(tableID);
//            } else {
//                return _direct.ListTableFields(tableID);
//            }
//        }

//        #region IDisposable Members

//        public void Dispose() {
//            if (_direct != null) {
//                _direct.Dispose();
//                _direct = null;
//            }
//        }

//        #endregion
//    }
//}
