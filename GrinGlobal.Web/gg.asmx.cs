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
    /// Summary description for gg
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class gg : System.Web.Services.WebService {

		public string Login(string userName, string password) {
			return SecureData.Login(userName, password);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="suppressExceptions"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="dataviewName"></param>
		/// <param name="delimitedParameterList">pass in CSS-style format. e.g. ivid=37;acid=23423;acp=AMA;</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="options">pass in CSS-style format. Currently only valid option is "altlanguageid=(language id here)"</param>
        /// <returns></returns>
		[WebMethod]
        public DataSet GetData(bool suppressExceptions, string loginToken, string dataviewName, string delimitedParameterList, int offset, int limit, string options) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                DataSet ds = sd.GetData(dataviewName, delimitedParameterList, offset, limit, options);
                //string ua = HttpContext.Current.Request.UserAgent;
                //ds.RemotingFormat = SerializationFormat.Binary;
                return ds;
			}
		}

        /// <summary>
        /// Gets the stats required by the Curator Tool for determining if its local cache of lookup tables need to be updated.  Pertinent return table name is "get_lookup_table_stats"
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetLookupTableStats(bool suppressExceptions, string loginToken, string tableName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                DataSet ds = sd.GetLookupTableStats(tableName);
                return ds;
            }
        }

        /// <summary>
        /// Gets the stats required by the Curator Tool for determining if its local cache of lookup tables need to be updated.  Pertinent return table name is "get_lookup_table_stats".  Returns data for all tables associated with 'lookup' dataviews.
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetAllLookupTableStats(bool suppressExceptions, string loginToken) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                DataSet ds = sd.GetAllLookupTableStats();
                return ds;
            }
        }



		[WebMethod]
        public DataSet GetDataParameterTemplate(bool suppressExceptions, string loginToken, string dataviewName) {
			using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.GetDataParameterTemplate(dataviewName);
			}
		}

        [WebMethod]
        public bool ClearCache(bool suppressExceptions, string loginToken, string cacheName) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ClearCache(cacheName, true);
            }
        }

        [WebMethod]
        public DataSet TransferOwnership(bool suppressExceptions, string loginToken, DataSet ds, int newOwnerCooperatorID, bool includeDescendents) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.TransferOwnership(ds, newOwnerCooperatorID, includeDescendents);
            }
        }


		[WebMethod]
		public DataSet Search(bool suppressExceptions, string loginToken, string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit, string options) {
			using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
				return sd.Search(query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit, 0, 0, null, options, null, null);
			}
		}

		[WebMethod]
		public DataSet ValidateLogin(bool suppressExceptions, string userName, string password) {
            // TestLogin is a rare case -- usually login should always throw exceptions on error. 
            // this guy lets the user toggle that
            return SecureData.TestLogin(suppressExceptions, userName, password);
		}

		[WebMethod]
		public DataSet RenameList(bool suppressExceptions, string loginToken, string existingGroupName, string newGroupName, string existingTabName, string newTabName, int cooperatorID) {
			using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
				return sd.RenameList(existingGroupName, newGroupName, existingTabName, newTabName, cooperatorID);
			}
		}

		[WebMethod]
		public DataSet RenameTab(bool suppressExceptions, string loginToken, string existingTabName, string newTabName, int cooperatorID) {
			using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
				return sd.RenameTab(existingTabName, newTabName, cooperatorID);
			}
		}

		[WebMethod]
		public string GetVersion() {
			return Toolkit.GetApplicationVersion();
		}

		[WebMethod]
		public DataSet DeleteList(bool suppressExceptions, string loginToken, string groupName, string tabName, int cooperatorID) {
			using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
				return sd.DeleteList(groupName, tabName, cooperatorID);
			}
		}

		[WebMethod]
		public DataSet SaveData(bool suppressExceptions, string loginToken, DataSet ds, string options) {
			using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
				return sd.SaveData(ds, true, options);
			}
		}

        private string guestLogin() {
            return Login(Toolkit.GetSetting("AnonymousUserName", "guest"), Toolkit.GetSetting("AnonymousPassword", "guest"));
        }

        [WebMethod]
        public bool DeleteImage(string loginToken, string imageFileName) {
            using (SecureData sd = new SecureData(false, loginToken)) {
                return sd.DeleteImage(imageFileName);
            }
        }

        [WebMethod]
        public byte[] DownloadImage(string loginToken, string appOrDocRelativePath) {
            using(SecureData sd = new SecureData(false, loginToken)){
                return sd.GetImage(appOrDocRelativePath);
            }
        }

        [WebMethod]
        public string UploadImage(string loginToken, string appOrDocRelativePath, byte[] imageBytes, bool createThumbnail, bool overwriteIfExists) {
            using (SecureData sd = new SecureData(false, loginToken)) {
                return sd.SaveImage(appOrDocRelativePath, imageBytes, createThumbnail, overwriteIfExists);
            }
        }

        [WebMethod]
        public DataSet GetFileInfo(bool suppressExceptions, string groupName, string versionName, bool onlyAvailable, bool onlyLatest) {
            using (SecureData sd = new SecureData(suppressExceptions, UserManager.GetLoginToken(true))) {
                return sd.GetFileInfo(groupName, versionName, onlyAvailable, onlyLatest);
            }
        }

        [WebMethod]
        public DataSet GetSearchEngineInfo(bool suppressExceptions, bool enabledIndexesOnly) {
            using (SecureData sd = new SecureData(suppressExceptions, UserManager.GetLoginToken(true))) {
                return sd.GetSearchEngineInfo(enabledIndexesOnly, null, null);
            }
        }

        [WebMethod]
        public DataSet ResolveUniqueKeys(bool suppressExceptions, string loginToken, DataSet ds, string options) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ResolveUniqueKeys(ds, options);
            }
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
        public DataSet ChangePassword(bool suppressExceptions, string loginToken, string targetUserName, string newPassword) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ChangePassword(targetUserName, newPassword);
            }
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
        public DataSet ChangeLanguage(bool suppressExceptions, string loginToken, int newLanguageID) {
            using (SecureData sd = new SecureData(suppressExceptions, loginToken)) {
                return sd.ChangeLanguage(newLanguageID);
            }
        }

    }
}