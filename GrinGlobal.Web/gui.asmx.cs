using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using System.Collections.Generic;
using GrinGlobal.Core;
//using GrinGlobal.Sql;
//using GrinGlobal.Sql.GuiData;
using GrinGlobal.Business;

namespace GrinGlobal.Web {
	/// <summary>
	/// Summary description for GrinGUI
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class GUI : System.Web.Services.WebService {

        /// <summary>
        /// Performs a login using the given username and password.  Returns the loginToken if valid, null otherwise.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
		public string Login(string userName, string password) {
			return SecureData.Login(userName, password);
		}


		/// <summary>
		/// Returns data from the database as defined by the given dataview name (aka "resultsetNameOrSql") and parameters ("delimitedParameterList").
		/// </summary>
		/// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
		/// <param name="userName">Valid username to login with</param>
		/// <param name="password">Valid password to login with</param>
        /// <param name="dataviewName">Name of the dataview to run.</param>
		/// <param name="delimitedParameterList">pass in CSS-style format. e.g. :inventoryid=37;:accessionid=23423,7389;:cooperatorid=8987,98789,922</param>
        /// <param name="offset">Number of records to skip.  Default is zero</param>
        /// <param name="limit">Number of records to return.  Default is zero.  Anything less than 1 means return all</param>
        /// <param name="options">pass in CSS-style format"</param>
        /// <returns></returns>
		[WebMethod]
        public DataSet GetData(bool suppressExceptions, string userName, string password, string dataviewName, string delimitedParameterList, int offset, int limit, string options) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                DataSet ds = sd.GetData(dataviewName, delimitedParameterList, offset, limit, options);
                //string ua = HttpContext.Current.Request.UserAgent;
                //ds.RemotingFormat = SerializationFormat.Binary;
                return ds;
			}
		}

        /// <summary>
        /// Gets the stats required by the Curator Tool for determining if its local cache of lookup tables need to be updated.  Pertinent return table name is "get_lookup_table_stats"
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="tableName">Name of the table to return stats about</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetLookupTableStats(bool suppressExceptions, string userName, string password, string tableName) {
            using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                DataSet ds = sd.GetLookupTableStats(tableName);
                return ds;
            }
        }

        /// <summary>
        /// Gets the stats required by the Curator Tool for determining if its local cache of lookup tables need to be updated.  Pertinent return table name is "get_lookup_table_stats".  Returns data for all tables associated with 'lookup' dataviews.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetAllLookupTableStats(bool suppressExceptions, string userName, string password) {
            using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                DataSet ds = sd.GetAllLookupTableStats();
                return ds;
            }
        }


        /// <summary>
        /// Returns all parameter information that should be passed to GetData for the given dataview
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="dataviewName">Name of dataview to retrieve parameter information about</param>
        /// <returns></returns>
		[WebMethod]
        public DataSet GetDataParameterTemplate(bool suppressExceptions, string userName, string password, string dataviewName) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                return sd.GetDataParameterTemplate(dataviewName);
			}
		}

        /// <summary>
        /// Clears the given cacheName stored within the web site process memory.  If cacheName is null, clears all caches.  Username must be part of the administrators group for this call to succeed.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="cacheName">Name of cache to clear.  If null, all caches are cleared</param>
        /// <returns></returns>
        [WebMethod]
        public bool ClearCache(bool suppressExceptions, string userName, string password, string cacheName) {
            using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                return sd.ClearCache(cacheName, true);
            }
        }

        /// <summary>
        /// Transfer ownership of all given records from the cooperator associated with the username/password to the "newOwnerCooperatorID".  Checks are performed first to ensure the given username currently owns the records before ownership is transferred.  (i.e. you can only transfer your own records)
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="ds">A DataSet continaing one or more DataTable objects containing data which should change ownership</param>
        /// <param name="newOwnerCooperatorID">CooperatorID of the new owner.</param>
        /// <param name="includeDescendents">True to trickle-down ownership change, false to affect only the current table</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet TransferOwnership(bool suppressExceptions, string userName, string password, DataSet ds, int newOwnerCooperatorID, bool includeDescendents) {
            using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                return sd.TransferOwnership(ds, newOwnerCooperatorID, includeDescendents);
            }
        }


        /// <summary>
        /// Using given parameters, performs a search via the Search Engine and returns the results of that search
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="query">Search string to pass to the search engine for processing</param>
        /// <param name="ignoreCase">True if case should be ignored during the search, false if it should be considered</param>
        /// <param name="andTermsTogether">True if consecutive search terms should be and-ed together, False if they should be or-ed together</param>
        /// <param name="indexList">Names of all search engine indexes to add in the search.  Null means search all available indexes.</param>
        /// <param name="resolverName">Name of the resolver whose ID values should be returned.  Typically "accession", "inventory", "order_request", or "cooperator"</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return.  Less than 1 means return all applicable (up to whatever the search engine limit is configured as)</param>
        /// <param name="options">CSS-style key-value pairs of options.  Not yet used.</param>
        /// <returns></returns>
		[WebMethod]
		public DataSet Search(bool suppressExceptions, string userName, string password, string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit, string options) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
				return sd.Search(query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit, 0, 0, null, options, null, null);
			}
		}


        /// <summary>
        /// Validate the given username and password are correct, then return information about that user if it is
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <returns></returns>
		[WebMethod]
		public DataSet ValidateLogin(bool suppressExceptions, string userName, string password) {
            return SecureData.TestLogin(suppressExceptions, userName, password);
            //using (SecureData sd = new SecureData(suppressExceptions, null)) {
            //    // soft login is a rare case -- usually login should always throw exceptions on error. 
            //    // this guy lets the user toggle that
            //    return sd.SoftLogin(userName, password);
            //}
		}

        /// <summary>
        /// Changes the password for the given user.  Only users belonging to the administrators group can change passwords for other users.  Everybody else can change only their own password.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="targetUserName">Must match "userName" parameter for non-administrator users.</param>
        /// <param name="newPassword">Value for the user's "new" password</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet ChangePassword(bool suppressExceptions, string userName, string password, string targetUserName, string newPassword) {
            return SecureData.ChangePassword(suppressExceptions, userName, password, targetUserName, newPassword);
        }

        /// <summary>
        /// Changes the language for the given user.
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newLanguageID"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet ChangeLanguage(bool suppressExceptions, string userName, string password, int newLanguageID) {
            using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                return sd.ChangeLanguage(newLanguageID);
            }
        }

        /// <summary>
        /// Changes a list (folders in left hand treeview in the CT) from one name to another efficiently regardless of how many items that list contains.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="existingGroupName">Name of group (aka list) to change</param>
        /// <param name="newGroupName">New name of group (aka list)</param>
        /// <param name="existingTabName">Name of tab to change</param>
        /// <param name="newTabName">New name of tab</param>
        /// <param name="cooperatorID">CooperatorID who owns the list, typically the same as the one associated with the "userName" parameter</param>
        /// <returns></returns>
		[WebMethod]
		public DataSet RenameList(bool suppressExceptions, string userName, string password, string existingGroupName, string newGroupName, string existingTabName, string newTabName, int cooperatorID) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
				return sd.RenameList(existingGroupName, newGroupName, existingTabName, newTabName, cooperatorID);
			}
		}

        /// <summary>
        /// Changes a tab name (tab above the left hand treeview in the CT) from one name to another efficiently regardless of how many items that tab contains.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="existingTabName">Name of the tab to change</param>
        /// <param name="newTabName">New name of the tab</param>
        /// <param name="cooperatorID">CooperatorID who owns the tab, typically the same as the one associated with the "userName" parameter</param>
        /// <returns></returns>
		[WebMethod]
		public DataSet RenameTab(bool suppressExceptions, string userName, string password, string existingTabName, string newTabName, int cooperatorID) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
				return sd.RenameTab(existingTabName, newTabName, cooperatorID);
			}
		}

        /// <summary>
        /// Returns the version of the web service.  E.g.: GRIN-Global Web Application v 1.5.998
        /// </summary>
        /// <returns></returns>
		[WebMethod]
		public string GetVersion() {
			return Toolkit.GetApplicationVersion();
		}

        /// <summary>
        /// Efficiently deletes a list (from the left hand treeview in the CT) regardless of the number of items it contains.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="groupName">Group (aka list) to delete</param>
        /// <param name="tabName">Tab the list belongs to</param>
        /// <param name="cooperatorID">CooperatorID who owns the list, typically the same as the one associated with the "userName" parameter</param>
        /// <returns></returns>
		[WebMethod]
		public DataSet DeleteList(bool suppressExceptions, string userName, string password, string groupName, string tabName, int cooperatorID) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
				return sd.DeleteList(groupName, tabName, cooperatorID);
			}
		}

        /// <summary>
        /// Generates appropriate SQL as needed and writes changed data to the database.
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="ds">DataSet containing data to write to the database</param>
        /// <param name="options">Currently used only by the Import Wizard, this CSS-formatted set of key-value pairs allows one to change certain aspects of the logic in the SecureData.SaveData() method</param>
        /// <returns></returns>
		[WebMethod]
		public DataSet SaveData(bool suppressExceptions, string userName, string password, DataSet ds, string options) {
			using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
				return sd.SaveData(ds, true, options);
			}
		}

        private string guestLogin() {
            return Login(Toolkit.GetSetting("AnonymousUserName", "guest"), Toolkit.GetSetting("AnonymousPassword", "guest"));
        }

        /// <summary>
        /// Deletes a file from the given storage location
        /// </summary>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="imageFileName">Applciation-relative path of file to write</param>
        /// <returns></returns>
        [WebMethod]
        public bool DeleteImage(string userName, string password, string imageFileName) {
            using (SecureData sd = new SecureData(false, Login(userName, password))) {
                return sd.DeleteImage(imageFileName);
            }
        }

        /// <summary>
        /// Downloads given file as a byte stream
        /// </summary>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="appOrDocRelativePath">App-relative path (~/path/to/file.gif) of file to download</param>
        /// <returns></returns>
        [WebMethod]
        public byte[] DownloadImage(string userName, string password, string appOrDocRelativePath) {
            using(SecureData sd = new SecureData(false, Login(userName, password))){
                return sd.GetImage(appOrDocRelativePath);
            }
        }

        /// <summary>
        /// Uplaods given file to given location on the server
        /// </summary>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="appOrDocRelativePath">App-relative path (~/path/to/file.gif) where the file should be stored</param>
        /// <param name="imageBytes">Array of bytes containing raw data to write to the file</param>
        /// <param name="createThumbnail">If true, a thumbnail will try to be created from the given byte array</param>
        /// <param name="overwriteIfExists">If true, overwrite the file if the given apporDocRelativePath already exists.  If false, throw an exception</param>
        /// <returns></returns>
        [WebMethod]
        public string UploadImage(string userName, string password, string appOrDocRelativePath, byte[] imageBytes, bool createThumbnail, bool overwriteIfExists) {
            using (SecureData sd = new SecureData(false, Login(userName, password))) {
                return sd.SaveImage(appOrDocRelativePath, imageBytes, createThumbnail, overwriteIfExists);
            }
        }

        /// <summary>
        /// Inspects all files for given group in the sys_group / sys_file_group / sys_file tables and returns information about them
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="groupName">Name of group to retrieve file list from.  Corresponds to sys_group.group_name table/field</param>
        /// <param name="versionName">Name of version to retrieve.  Null means "any"</param>
        /// <param name="onlyAvailable">If True, only return files that are currently "available" (i.e. sys_file.is_enabled = 'Y')</param>
        /// <param name="onlyLatest">If True, only return information about the latest version of each file</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetFileInfo(bool suppressExceptions, string groupName, string versionName, bool onlyAvailable, bool onlyLatest) {
            using (SecureData sd = new SecureData(suppressExceptions, UserManager.GetLoginToken(true))) {
                return sd.GetFileInfo(groupName, versionName, onlyAvailable, onlyLatest);
            }
        }

        /// <summary>
        /// Get current configuration information about the search engine
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="enabledIndexesOnly">True to return information only on enabled indexes, False to return all</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetSearchEngineInfo(bool suppressExceptions, bool enabledIndexesOnly) {
            using (SecureData sd = new SecureData(suppressExceptions, UserManager.GetLoginToken(true))) {
                return sd.GetSearchEngineInfo(enabledIndexesOnly, null, null);
            }
        }

        /// <summary>
        /// Given a set of data, use the defined unique key values to determine the primary key values
        /// </summary>
        /// <param name="suppressExceptions">True if exceptions should be queued up into the "ExceptionTable" DataTable object as part of the DataSet returned from this method.  False to raise actual exceptions.</param>
        /// <param name="userName">Valid username to login with</param>
        /// <param name="password">Valid password to login with</param>
        /// <param name="ds">Data to inspect</param>
        /// <param name="options">CSS-formatted set of key-value pairs allows one to change certain aspects of the logic flow.  Not currently used.</param>
        /// <returns></returns>
        [WebMethod]
        public DataSet ResolveUniqueKeys(bool suppressExceptions, string userName, string password, DataSet ds, string options) {
            using (SecureData sd = new SecureData(suppressExceptions, Login(userName, password))) {
                return sd.ResolveUniqueKeys(ds, options);
            }
        }

	}
}