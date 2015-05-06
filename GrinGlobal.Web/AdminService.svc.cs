using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using GrinGlobal.Business;
using GrinGlobal.Core;
using GrinGlobal.Business.SearchSvc;
using System.IO;

namespace GrinGlobal.Web {
    // NOTE: If you change the class name "AdminService" here, you must also update the reference to "AdminService" in Web.config.
	public class AdminService : IAdminService {


        // NOTE: This property is NOT exposed via the web.
        //       It is used to transparently support the admin tool
        //       connecting via a web service or a direct dll reference.
        //       It allows SecureData to override which database it points at
        //       (contrast the new SecureData() constructor used in this class against the one used in GUI.asmx)
        // public DataConnectionSpec DataConnectionSpec = null;

        public DataSet DeleteDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DeleteDataViewDefinition(dataviewName);
			}
		}

        public DataSet CreateDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName, bool readOnly, bool suppressProps, bool isSystem, bool userVisible, string formAssemblyName, string formFullyQualifiedName, string executableName, string categoryName, int categorySortOrder, bool transformable, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, DataSet dsInfo) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.CreateDataViewDefinition(dataviewName, readOnly, suppressProps, isSystem, userVisible, formAssemblyName, formFullyQualifiedName, executableName, categoryName, categorySortOrder, transformable, transformFieldForNames, transformFieldForCaptions, transformFieldForValues, dsInfo);
			}
		}

        public DataSet CopyDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName, string copyToDataViewName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.CopyDataViewDefinition(dataviewName, copyToDataViewName);
            }
        }

        public DataSet RenameDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName, string newDataViewName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.RenameDataViewDefinition(dataviewName, newDataViewName);
            }
        }

        public DataSet ImportDataViewDefinitions(bool suppressExceptions, string loginToken, DataSet dsDefinitions, List<string> dataviewNamesToImport) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ImportDataViewDefinitions(dsDefinitions, dataviewNamesToImport);
            }
        }

        public DataSet ExportDataViewDefinitions(bool suppressExceptions, string loginToken, List<string> dataviewNamesToExport) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ExportDataViewDefinitions(dataviewNamesToExport);
            }
        }
        public DataSet ChangePassword(bool suppressExceptions, string loginToken, int targetUserID, string newPassword) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ChangePassword(targetUserID, newPassword);
            }
        }

        public DataSet GetDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.GetDataViewDefinition(dataviewName);
            }
        }


		public DataSet ClearCache(bool suppressExceptions, string loginToken, string cacheName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ClearCache(cacheName);
			}
		}

		public string Login(string userName, string password) {
            return SecureData.Login(userName, password);
		}

		public string GetVersion() {
			return Toolkit.GetApplicationVersion();
		}

        public string TestDatabaseConnection() {
            return SecureData.TestDatabaseConnection(null);
        }

        public DataSet DeleteUser(bool suppressExceptions, string loginToken, int userID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DeleteUser(userID);
            }
        }

        public DataSet DisableUser(bool suppressExceptions, string loginToken, int userID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DisableUser(userID);
            }
        }

        public DataSet EnableUser(bool suppressExceptions, string loginToken, int userID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.EnableUser(userID);
            }
        }

        public List<string> GetRoles(string loginToken, int userID) {
            using (var sd = new SecureData(false, loginToken)) {
                return sd.GetRoles(null, 0);
            }
        }

        public DataSet GetUserInfo(bool suppressExceptions, string loginToken, int userID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.GetUserInfo(userID);
            }
        }

        public DataSet GetCooperatorInfo(bool suppressExceptions, string loginToken, int cooperatorID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.GetCooperatorInfo(cooperatorID);
            }
        }

        public DataSet SearchCooperators(bool suppressExceptions, string loginToken, string query) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.SearchCooperators(query);
            }
        }

        public DataSet ListUsers(bool suppressExceptions, string loginToken, int userID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListUsers(userID);
            }
        }

        public DataSet ListPermissions(bool suppressExceptions, string loginToken, int permID, List<int> excludePermIDs, bool enabledOnly) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListPermissions(permID, excludePermIDs, enabledOnly);
            }
        }

        public DataSet DeletePermission(bool suppressExceptions, string loginToken, int permID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DeletePermission(permID);
            }
        }

        public DataSet DisablePermission(bool suppressExceptions, string loginToken, int permID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DisablePermission(permID);
            }
        }

        public DataSet EnablePermission(bool suppressExceptions, string loginToken, int permID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.EnablePermission(permID);
            }
        }

        public int SavePermission(bool suppressExceptions, string loginToken, int permID, string name, string description, int dataviewID, int tableID, string createPerm, string readPerm, string updatePerm, string deletePerm, bool isEnabled) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.SavePermission(permID, name, description, dataviewID, tableID, createPerm, readPerm, updatePerm, deletePerm, isEnabled);
            }
        }

        public DataSet ListDataViews(bool suppressExceptions, string loginToken, int dataviewID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListDataViews(dataviewID);
            }
        }

        public DataSet ListTables(bool suppressExceptions, string loginToken, int tableID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListTables(tableID);
            }
        }

        public DataSet ListEffectivePermissions(bool suppressExceptions, string loginToken, int userID, string dataviewName, string tableName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListEffectivePermissions(userID, dataviewName, tableName);
            }
        }


        public DataSet AddPermissionsToUser(bool suppressExceptions, string loginToken, int userID, List<int> permIDs) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.AddPermissionsToUser(userID, permIDs);
            }
        }

        public DataSet RemovePermissionsFromUser(bool suppressExceptions, string loginToken, int userID, List<int> permIDs) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.RemovePermissionsFromUser(userID, permIDs);
            }
        }

        public DataSet ListPermissionFields(bool suppressExceptions, string loginToken, int permID, int permFieldID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListPermissionFields(permID, permFieldID);
            }
        }

        public DataSet ListRelatedTables(bool suppressExceptions, string loginToken, int dataviewID, int tableID, int parentTableFieldID, string[] relationshipTypes) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListRelatedTables(dataviewID, tableID, parentTableFieldID, relationshipTypes);
            }
        }

        public DataSet ListTableFields(bool suppressExceptions, string loginToken, int tableID, int tableFieldID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListTableFields(tableID, tableFieldID);
            }
        }

        public DataSet ListDataViewFields(bool suppressExceptions, string loginToken, int dataviewID, int dataviewFieldID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListDataViewFields(dataviewID, dataviewFieldID);
            }
        }

        public int SavePermissionField(bool suppressExceptions, string loginToken, int permFieldID, int permID, int dataviewFieldID, int tableFieldID, string valueType, string compareOperator, string value, int parentTableFieldID, string parentValueType, string parentCompareOperator, string parentValue, string compareMode) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.SavePermissionField(permFieldID, permID, dataviewFieldID, tableFieldID, valueType, compareOperator, value, parentTableFieldID, parentValueType, parentCompareOperator, parentValue, compareMode);
            }
        }

        public DataSet DeletePermissionField(bool suppressExceptions, string loginToken, int permFieldID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DeletePermissionField(permFieldID);
            }
        }

        public int SaveUser(bool suppressExceptions, string loginToken, int userID, string userName, bool enabled, int cooperatorID, int currentCooperatorID, string title, string firstName, string initials, string lastName, string fullName, string job,
            string discipline, string organization, string organizationCode, int languageID, bool isActive, string addressLine1, string addressLine2, string addressLine3, string admin1, string admin2, string email,
            string primaryPhone, string secondaryPhone, string fax, string siteCode, string regionCode, string categoryCode, int geographyID, string note){
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.SaveUser(userID, userName, enabled, cooperatorID, currentCooperatorID, title, firstName, initials, lastName, fullName, job,
                    discipline, organization, organizationCode, languageID, isActive, addressLine1, addressLine2, addressLine3, admin1, admin2, email,
                    primaryPhone, secondaryPhone, fax, siteCode, regionCode, categoryCode, geographyID, note);
            }
        }

        public DataSet ListSites(bool suppressExceptions, string loginToken, string siteCode) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListSites(siteCode);
            }
        }

        public DataSet ListGeographies(bool suppressExceptions, string loginToken, int geographyID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListGeographies(geographyID);
            }
        }

        public DataSet ListLanguages(bool suppressExceptions, string loginToken, int languageID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListLanguages(languageID);
            }
        }

        public DataSet ListPermissionTemplates(bool suppressExceptions, string loginToken, int permTemplateID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ListPermissionTemplates(permTemplateID);
            }
        }

        public DataSet DeletePermissionTemplate(bool suppressExceptions, string loginToken, int permTemplateID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.DeletePermissionTemplate(permTemplateID);
            }
        }

        public int SavePermissionTemplate(bool suppressExceptions, string loginToken, int permTemplateID, string name, string description) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.SavePermissionTemplate(permTemplateID, name, description);
            }
        }

        public DataSet AddPermissionsToTemplate(bool suppressExceptions, string loginToken, int permTemplateID, List<int> permIDs) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.AddPermissionsToTemplate(permTemplateID, permIDs);
            }
        }

        public DataSet RemovePermissionsFromTemplate(bool suppressExceptions, string loginToken, int permTemplateID, List<int> permIDs) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.RemovePermissionsFromTemplate(permTemplateID, permIDs);
            }
        }

        public DataSet ApplyPermissionTemplatesToUser(bool suppressExceptions, string loginToken, int userID, List<int> permTemplateIDs) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ApplyPermissionTemplatesToUser(userID, permTemplateIDs);
            }
        }

        public DataSet ListTableNames(bool suppressExceptions, string loginToken, string schemaName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken))
            {
                return sd.ListTableNames(schemaName);
            }
        }

        public DataSet RecreateTableMappings(bool suppressExceptions, string loginToken, List<string> tableNames) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken))
            {
                return sd.RecreateTableMappings(tableNames);
            }
        }
    }
}
