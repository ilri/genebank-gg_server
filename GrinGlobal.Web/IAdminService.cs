using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
//using GrinGlobal.Sql.GuiData;
//using GrinGlobal.Sql;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.ServiceModel.Web;

namespace GrinGlobal.Web {
    // NOTE: If you change the interface name "IAdminService" here, you must also update the reference to "IAdminService" in Web.config.
	//[ServiceContract(Namespace="http://www.grin-global.org")]
    [ServiceContract()]
    public interface IAdminService {

        #region DataView manipulations
        [OperationContract]
		DataSet DeleteDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName);

		[OperationContract]
		DataSet GetDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName);

        [OperationContract]
        DataSet CopyDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName, string copyToDataViewName);

        [OperationContract]
        DataSet RenameDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName, string newDataViewName);


        [OperationContract]
        DataSet CreateDataViewDefinition(bool suppressExceptions, string loginToken, string dataviewName, bool readOnly, bool suppressProps, bool isSystem, bool userVisible, string formAssemblyName, string formFullyQualifiedName, string executableName, string categoryName, int categorySortOrder, bool transformable, string transformFieldForNames, string transformFieldForCaptions, string transformFieldForValues, DataSet dsInfo);

        [OperationContract]
        DataSet ImportDataViewDefinitions(bool suppressExceptions, string loginToken, DataSet dsDefintions, List<string> dataviewNamesToImport);

        [OperationContract]
        DataSet ExportDataViewDefinitions(bool suppressExceptions, string loginToken, List<string> dataviewNamesToExport);


        #endregion

        #region User Manipulations
        [OperationContract]
        string Login(string userName, string password);

        [OperationContract]
        DataSet ChangePassword(bool suppressExceptions, string loginToken, int targetUserID, string newPassword);

        [OperationContract]
        DataSet ListUsers(bool suppressExceptions, string loginToken, int userID);

        [OperationContract]
        DataSet GetUserInfo(bool suppressExceptions, string loginToken, int userID);

        [OperationContract]
        DataSet GetCooperatorInfo(bool suppressExceptions, string loginToken, int cooperatorID);

        [OperationContract]
        DataSet SearchCooperators(bool suppressExceptions, string loginToken, string query);

        [OperationContract]
        DataSet DeleteUser(bool suppressExceptions, string loginToken, int userID);

        [OperationContract]
        DataSet DisableUser(bool suppressExceptions, string loginToken, int userID);

        [OperationContract]
        DataSet EnableUser(bool suppressExceptions, string loginToken, int userID);

        [OperationContract]
        DataSet ListEffectivePermissions(bool suppressExceptions, string loginToken, int userID, string dataviewName, string tableName);

        [OperationContract]
        DataSet AddPermissionsToUser(bool suppressExceptions, string loginToken, int userID, List<int> permIDs);

        [OperationContract]
        DataSet RemovePermissionsFromUser(bool suppressExceptions, string loginToken, int userID, List<int> permIDs);

        [OperationContract]
        List<string> GetRoles(string loginToken, int userID);

        [OperationContract]
        int SaveUser(bool suppressExceptions, string loginToken, int userID, string userName, bool enabled, int cooperatorID, int currentCooperatorID, string title, string firstName, string initials, string lastName, string fullName, string job,
            string discipline, string organization, string organizationCode, int languageID, bool isActive, string addressLine1, string addressLine2, string addressLine3, string admin1, string admin2, string email,
            string primaryPhone, string secondaryPhone, string fax, string siteCode, string regionCode, string categoryCode, int geographyID, string note);

        #endregion

        #region Permission Manipulations

        [OperationContract]
        DataSet ListPermissions(bool suppressExceptions, string loginToken, int permID, List<int> excludePermIDs, bool enabledOnly);

        [OperationContract]
        DataSet DeletePermission(bool suppressExceptions, string loginToken, int permID);

        [OperationContract]
        DataSet DisablePermission(bool suppressExceptions, string loginToken, int permID);

        [OperationContract]
        DataSet EnablePermission(bool suppressExceptions, string loginToken, int permID);

        [OperationContract]
        int SavePermission(bool suppressExceptions, string loginToken, int permID, string name, string description, int dataviewID, int tableID, string createPerm, string readPerm, string updatePerm, string deletePerm, bool isEnabled);

        [OperationContract]
        DataSet ListRelatedTables(bool suppressExceptions, string loginToken, int dataviewID, int tableID, int parentTableFieldID, string[] relationshipTypes);

        [OperationContract]
        DataSet ListPermissionFields(bool suppressExceptions, string loginToken, int permID, int permFieldID);

        [OperationContract]
        int SavePermissionField(bool suppressExceptions, string loginToken, int permFieldID, int permID, int dataviewFieldID, int tableFieldID, string valueType, string compareOperator, string value, int parentTableFieldID, string parentValueType, string parentCompareOperator, string parentValue, string compareMode);

        [OperationContract]
        DataSet DeletePermissionField(bool suppressExceptions, string loginToken, int permFieldID);



        [OperationContract]
        DataSet ListPermissionTemplates(bool suppressExceptions, string loginToken, int permTemplateID);

        [OperationContract]
        DataSet DeletePermissionTemplate(bool suppressExceptions, string loginToken, int permTemplateID);

        [OperationContract]
        int SavePermissionTemplate(bool suppressExceptions, string loginToken, int permTemplateID, string name, string description);

        [OperationContract]
        DataSet AddPermissionsToTemplate(bool suppressExceptions, string loginToken, int permTemplateID, List<int> permIDs);

        [OperationContract]
        DataSet RemovePermissionsFromTemplate(bool suppressExceptions, string loginToken, int permTemplateID, List<int> permIDs);

        [OperationContract]
        DataSet ApplyPermissionTemplatesToUser(bool suppressExceptions, string loginToken, int userID, List<int> permTemplateIDs);

        #endregion

        #region Miscellaneous

        [OperationContract]
        DataSet ListDataViews(bool suppressExceptions, string loginToken, int dataviewID);

        [OperationContract]
        DataSet ListTables(bool suppressExceptions, string loginToken, int tableID);

        [OperationContract]
        DataSet ListTableFields(bool suppressExceptions, string loginToken, int tableID, int tableFieldID);

        [OperationContract]
        DataSet ListDataViewFields(bool suppressExceptions, string loginToken, int dataviewID, int dataviewFieldID);

        [OperationContract]
        DataSet ListSites(bool suppressExceptions, string loginToken, string siteCode);


        [OperationContract]
        DataSet ListGeographies(bool suppressExceptions, string loginToken, int geographyID);

        [OperationContract]
        DataSet ListLanguages(bool suppressExceptions, string loginToken, int languageID);

        [OperationContract]
        DataSet ClearCache(bool suppressExceptions, string loginToken, string cacheName);

        [OperationContract]
        string GetVersion();

        [OperationContract]
        string TestDatabaseConnection();

        [OperationContract]   
        DataSet ListTableNames(bool suppressExceptions, string loginToken, string schemaName);

        [OperationContract]   
        DataSet RecreateTableMappings(bool suppressExceptions, string loginToken, List<string> tableNames);

        #endregion

        #region No Longer Needed
        //[OperationContract]
        //DataSet GetData(bool suppressExceptions, string loginToken, string dataviewNameOrSql, string delimitedParameterList, int offset, int limit);

        //[OperationContract]
        //string GetDataJson(bool suppressExceptions, string loginToken, string dataviewNameOrSql, string delimitedParameterList, int offset, int limit);

        //[OperationContract]
        //DataSet GetDataViaDataSet(bool suppressExceptions, string loginToken, string dataviewNameOrSql, DataSet parameterList, int offset, int limit);

        //[OperationContract]
        //DataSet GetDataParameterTemplate(bool suppressExceptions, string loginToken, string dataviewName);

        //[OperationContract]
        //DataSet LogUsage(bool suppressExceptions, string loginToken, string usageData);

        //[OperationContract]
        //DataSet Search(bool suppressExceptions, string loginToken, string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit);

        //[OperationContract]
        //DataSet SearchThenGetData(bool suppressExceptions, string loginToken, string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName);

        //[WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        //[OperationContract]
        //string SearchThenGetDataJson(bool suppressExceptions, string loginToken, string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName);

        //[OperationContract]
        //DataSet CreateUser(bool suppressExceptions, string loginToken, string newUserUserName, string newUserPassword, bool newUserEnabled);

        //[OperationContract]
        //DataSet RenameList(bool suppressExceptions, string loginToken, string existingGroupName, string newGroupName, string existingTabName, string newTabName, int cooperatorID);

        //[OperationContract]
        //DataSet RenameTab(bool suppressExceptions, string loginToken, string existingTabName, string newTabName, int cooperatorID);

        //[OperationContract]
        //DataSet ChangeLanguage(bool suppressExceptions, string loginToken, int newLanguageID);

        //[OperationContract]
        //DataSet DeleteList(bool suppressExceptions, string loginToken, string groupName, string tabName, int cooperatorID);

        //[OperationContract]
        //DataSet SaveDataSet(bool suppressExceptions, string loginToken, DataSet ds);

        //[OperationContract]
        //DataSet SaveRow(bool suppressExceptions, string loginToken, Dictionary<string, string> keys, Dictionary<string, object> oldValues, Dictionary<string, object> newValues);
        #endregion

    }
}