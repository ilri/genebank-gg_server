using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using GrinGlobal.Business;
using System.Security.Principal;
using System.Web.Security;
using GrinGlobal.Core;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace GrinGlobal.Web {
    public class UserManager
    {
        //public const int SYSTEMID = 0;  // TODO: look up the "SYSTEM" cooperatorid from cooperator instead of hardcoding it!!!

        public static int GetSystemCooperatorID()
        {
            using (var sd = GetSecureData(true))
            {
                return sd.GetCooperatorIDForSystem();
            }
        }

        public static bool IsAnonymousUser(string userName)
        {
            return String.Compare(GetAnonymousUserName(), userName, true) == 0;
        }

        public static void Authenticate()
        {

            // if the user isn't logged in,
            // try to pull login token from the forms auth cookie
            var ctx = HttpContext.Current;
            if (ctx != null && ctx.User == null && ctx.Request != null)
            {
                var cookie = ctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                }
            }
        }

        public static string GetAnonymousUserName()
        {
            string defaultUserName = Toolkit.GetSetting("AnonymousUserName", "guest");
            return defaultUserName;
        }

        //[DebuggerStepThrough()]
        //public static SecureData GetSecureData() {
        //    return new SecureData(false, UserManager.GetLoginToken(false));
        //}

        [DebuggerStepThrough()]
        public static SecureData GetSecureData(bool useAnonymousIfEmptyLoginToken)
        {
            return new SecureData(false, UserManager.GetLoginToken(useAnonymousIfEmptyLoginToken));
        }

        [DebuggerStepThrough()]
        public static AdminData GetAdminData(bool useAnonymousIfEmptyLoginToken)
        {
            return new AdminData(false, UserManager.GetLoginToken(useAnonymousIfEmptyLoginToken));
        }

        /// <summary>
        /// Gets the login token for the default one as created by specifying the AnonymousUserName and AnonymousPassword values in the web.config file.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough()]
        public static string GetLoginToken()
        {
            return GetLoginToken(true);
        }

        /// <summary>
        /// Gets the login token for the current user.  If there isn't one, the default one as created by specifying the AnonymousUserName and AnonymousPassword values in the web.config file will be used.
        /// </summary>
        /// <param name="useDefaultIfEmpty">True to use default if login token is empty, false to return null if login token is empty</param>
        /// <returns></returns>
        [DebuggerStepThrough()]
        public static string GetLoginToken(bool useAnonymousIfEmpty)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null && ctx.Request != null)
            {
                var ck = ctx.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (ck != null)
                {
                    var ticket = FormsAuthentication.Decrypt(ck.Value);
                    if (ticket != null)
                    {
                        return ticket.UserData;
                    }
                }
            }

            // we get here, we might need to load the default token
            if (useAnonymousIfEmpty)
            {
                return getAnonymousLoginToken();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Pulls all languages from sys_lang table and caches them so this method can be called very frequently and efficiently
        /// </summary>
        /// <returns></returns>
        public static DataTable ListLanguages()
        {
            var cm = CacheManager.Get("GenericSettings");
            var dt = cm["languageList"] as DataTable;
            if (dt == null)
            {
                // fill language list
                using (var sd = UserManager.GetAdminData(true))
                {
                    dt = sd.ListLanguages(-1).Tables["list_languages"];
                    cm["languageList"] = dt;
                }
            }
            return dt;
        }

        private static string getAnonymousLoginToken()
        {
            var anonymousLoginToken = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                anonymousLoginToken = HttpContext.Current.Session["anonymous_login"] as string;
            }
            if (String.IsNullOrEmpty(anonymousLoginToken))
            {
                // no default login token yet.
                // call the login code to generate one if they specified a default user/pw in the web.config
                string anonymousUserName = GetAnonymousUserName();
                string anonymousPassword = Toolkit.GetSetting("AnonymousPassword", "");
                if (!String.IsNullOrEmpty(anonymousUserName) && !String.IsNullOrEmpty(anonymousPassword))
                {
                    anonymousLoginToken = SecureData.WebLogin(anonymousUserName, anonymousPassword, false, true);
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session.Add("anonymous_login", anonymousLoginToken);
                    }
                }
            }
            return anonymousLoginToken;
        }

        /// <summary>
        /// On successful login, creates the login token and writes it to the 'gringlobal' cookie. Optionally makes the cookie persistent.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        public static string ValidateLogin(string username, string password, bool ignorePassword, bool appliedEncryption)
        {

            string loginToken = SecureData.WebLogin(username, password, ignorePassword, appliedEncryption);
            return loginToken;
        }

        public static void SaveLoginCookieAndRedirect(string userName, string loginToken, bool isPersistent, string destinationUrl, CartItem[] accessions)
        {
            // if persistent, by default,  make it valid for 5 days, otherwise for 20 minutes. Both can be changed per configrartions.
            int sessionRegular = Toolkit.GetSetting("SessionExpireRegular", 20);
            int sessionLong = Toolkit.GetSetting("SessionExpireLong", 7200);

            //DateTime expiration = (isPersistent ? DateTime.Now.AddDays(5) : DateTime.Now.AddMinutes(20));
            DateTime expiration = (isPersistent ? DateTime.Now.AddMinutes(sessionLong) : DateTime.Now.AddMinutes(sessionRegular));

            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, expiration, isPersistent, loginToken);
            var hash = FormsAuthentication.Encrypt(ticket);

            var ctx = HttpContext.Current;

            if (Toolkit.GetSetting("AllowCookies", true))
            {
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                cookie.Expires = expiration;

                ctx.Response.Cookies.Clear();
                ctx.Response.Cookies.Add(cookie);
            }

            mergeCartItems(loginToken, accessions);

            checkExpiredPassword(userName, loginToken, ctx);

            updateLoginTime(userName, loginToken);

            if (!String.IsNullOrEmpty(destinationUrl))
            {
                ctx.Response.Clear();
                ctx.Response.Redirect(destinationUrl);
                ctx.Response.End();
            }
        }

        /// <summary>
        /// Logs out the current user and tells the browser to delete the 'gringlobal' cookie by setting the expiration date in the distant past
        /// </summary>
        public static void Logout()
        {
            // logout the current user
            FormsAuthentication.SignOut();
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Abandon();
            }

            var ctx = HttpContext.Current;
            ctx.Response.Clear();
            ctx.Response.Redirect("~/Login.aspx");

        }

        /// <summary>
        /// Returns user name of currently logged in user.  If user is not logged in, returns user specified in web.config appSetting of "AnonymousUserName", or "" if appSetting doesn't exist.
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            string ret = null;
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
            {
                ret = HttpContext.Current.User.Identity.Name;
            }

            if (String.IsNullOrEmpty(ret))
            {
                ret = GetAnonymousUserName();
            }

            return ret;
        }

        /// <summary>
        /// Save logged in user preference.
        /// </summary>
        /// <param name="coopid"></param>
        /// <param name="prefKey"></param>
        /// <param name="prefValue"></param>
        /// <returns></returns>
        public static void SaveUserPref(string prefKey, string prefValue)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    int wuserid = sd.WebUserID;
                    int affected = dm.Write(@"
                        update web_user_preference
                        set
                            preference_value = :prefvalue,
                            modified_date = :now,
                            modified_by   = :modifiedby
                        where
                            web_user_id = :webuserid
                            and preference_name = :prefname
                        ", new DataParameters(
                        ":prefvalue", prefValue,
                        ":webuserid", wuserid, DbType.Int32,
                        ":now", DateTime.UtcNow, DbType.DateTime2,
                        ":modifiedby", wuserid, DbType.Int32,
                        ":prefname", prefKey, DbType.String
                        ));

                    if (affected == 0)
                    {
                        dm.Write(@"
                        insert into web_user_preference
                        (web_user_id, preference_name, preference_value, created_date, created_by, owned_date, owned_by)
                        values
                        (:webuserid, :prefname, :prefvalue, :now1, :webuserid2, :now2, :webuserid3)
                        ", new DataParameters(
                        ":webuserid", wuserid, DbType.Int32,
                        ":prefname", prefKey, DbType.String,
                        ":prefvalue", prefValue, DbType.String,
                        ":now1", DateTime.UtcNow, DbType.DateTime2,
                        ":webuserid2", wuserid, DbType.Int32,
                        ":now2", DateTime.UtcNow, DbType.DateTime2,
                        ":webuserid3", wuserid, DbType.Int32
                        ));
                    }
                }
            }
        }

        public static void SaveUserLanguage(int langid)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    dm.Write(@"
                        update web_user
                        set
                            sys_lang_id = :seclangid,
                            modified_date = :modifieddate
                        where
                            web_user_id = :webuserid
                        ", new DataParameters(
                        ":seclangid", langid,
                        ":webuserid", sd.WebUserID, DbType.Int32,
                        ":modifieddate", DateTime.UtcNow, DbType.DateTime2
                        ));
                }
            }
        }

        /// <summary>
        /// Get user's preference from the database.
        /// </summary>
        /// <param name="coopid"></param>
        /// <param name="prefKey"></param>
        /// <returns></returns>
        public static string GetUserPref(string prefKey)
        {
            string ret = null;

            using (SecureData sd = GetSecureData(true))
            {
                //                using (DataManager dm = sd.BeginProcessing(true))
                //                {
                //                    ret = (dm.ReadValue(@"
                //                            select 
                //                                preference_value
                //                            from 
                //                                web_user_preference
                //                            where 
                //                                web_user_id = :userid
                //                                and preference_name = :prefname
                //                            ", new DataParameters(
                //                            ":userid", sd.WebUserID , DbType.Int32,
                //                            ":prefname", prefKey, DbType.String))).ToString();
                //                }
                var dt = sd.GetData("web_user_preference", ":userid=" + sd.WebUserID + ";:prefname=" + prefKey, 0, 0).Tables["web_user_preference"];
                ret = dt.ToScalarValue();
            }
            return ret;
        }

        private static void updateLoginTime(string webUserName, string loginToken) // TODO: need to move these sql/functions/methods to secureData.cs later
        {
            using (SecureData sd = new SecureData(true, loginToken, null))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    dm.Write(@"
                    update web_user
                    set    
                        last_login_date = :lastlogindate
                    where
                        user_name = :wusername
                    ", new DataParameters(
                    ":lastlogindate", DateTime.UtcNow, DbType.DateTime2,
                    ":wusername", webUserName, DbType.String));
                }
            }
        }

        public static bool HasWebAddress()
        {
            using (SecureData sd = GetSecureData(true))
            {
                //                using (DataManager dm = sd.BeginProcessing(true))
                //                {
                //                    DataTable dt = dm.Read(@"
                //                    select * 
                //                    from web_user_shipping_address
                //                    where web_user_id = :wuserid",
                //                    new DataParameters
                //                    (":wuserid", sd.WebUserID, DbType.Int32
                //                    ));

                DataTable dt = sd.GetData("web_user_shipping_address", ":wuserid=" + sd.WebUserID, 0, 0).Tables["web_user_shipping_address"];

                if (dt.Rows.Count > 1)
                    return true;
                else
                    return false;
                //}
            }
        }

        public static DataTable GetDefaultShippingAddress()
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    return dm.Read(@"
                        select distinct wusa.*, g.country_code, g.adm1 as state_name, cvl.title as country_name
                        from web_user_shipping_address wusa join geography g 
                        on wusa.geography_id = g.geography_id 
                        join code_value cv 
                        on g.country_code = cv.value 
                        join code_value_lang cvl on cv.code_value_id = cvl.code_value_id 
                        where cvl.sys_lang_id = :languageid
                        and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE'
                        and wusa.web_user_id = :wuserid
                        and wusa.is_default = 'Y'",
                        new DataParameters(
                        ":wuserid", sd.WebUserID, DbType.Int32,
                        ":languageid", sd.LanguageID, DbType.Int32
                        ));
                }
            }
        }

        public static DataTable GetShippingAddress(int addressid)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    return dm.Read(@"
                        select distinct wusa.*, g.country_code, g.adm1 as state_name from web_user_shipping_address wusa join geography g 
                        on wusa.geography_id = g.geography_id 
                        where wusa.web_user_id = :wuserid
                        and wusa.web_user_shipping_address_id = :addressid",
                        new DataParameters(
                        ":wuserid", sd.WebUserID, DbType.Int32,
                        ":addressid", addressid, DbType.Int32
                        ));
                }
            }
        }

        public static DataTable GetShippingAddress()
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    return dm.Read(@"
                        select distinct wusa.*, g.country_code, g.adm1 as state_name, cvl.title as country_name
                        from web_user_shipping_address wusa join geography g 
                        on wusa.geography_id = g.geography_id 
                        join code_value cv 
                        on g.country_code = cv.value 
                        join code_value_lang cvl on cv.code_value_id = cvl.code_value_id 
                        where cvl.sys_lang_id = :languageid
                        and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE'
                        and wusa.web_user_id = :wuserid"
                        ,
                        new DataParameters(
                        ":wuserid", sd.WebUserID, DbType.Int32,
                        ":languageid", sd.LanguageID, DbType.Int32
                        ));
                }
            }
        }

        public static void SaveShippingAddress(string addrName, string addr1, string addr2, string addr3, string city, string zip, int geographyid, bool isDefault)
        {
            int addrid = SaveShippingAddress(addrName, addr1, addr2, addr3, city, zip, geographyid);
            if (isDefault) MarkAddressDefault(addrid);
        }

        public static int SaveShippingAddress(string addrName, string addr1, string addr2, string addr3, string city, string zip, int geographyid)
        {
            int addressid = 0;
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    addressid = Toolkit.ToInt32(dm.ReadValue(@"
                            select 
                                web_user_shipping_address_id
                            from 
                                web_user_shipping_address
                            where 
                                web_user_id = :webuserid
                                and address_name = :addressname
                            ", new DataParameters(":webuserid", sd.WebUserID, DbType.Int32
                                 , ":addressname", addrName, DbType.String)), -1);

                    if (addressid < 0)
                    {
                        addressid = dm.Write(@"
                        insert into web_user_shipping_address
                        (web_user_id, address_name, address_line1, address_line2, address_line3, city, postal_index, geography_id, created_date, created_by, owned_date, owned_by)
                        values
                        (:wuserid, :addressname, :address1, :address2, :address3, :city, :zip, :geographyid, :createddate, :createdby, :owneddate, :ownedby)
                        ", true, "web_user_shipping_address_id", new DataParameters(
                        ":wuserid", sd.WebUserID, DbType.Int32,
                        ":addressname", addrName, DbType.String,
                        ":address1", addr1, DbType.String,
                        ":address2", addr2, DbType.String,
                        ":address3", addr3, DbType.String,
                        ":city", city, DbType.String,
                        ":zip", zip, DbType.String,
                        ":geographyid", geographyid, DbType.Int32,
                        ":createddate", DateTime.UtcNow, DbType.DateTime2,
                        ":createdby", sd.WebUserID, DbType.Int32,
                        ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                        ":ownedby", sd.WebUserID, DbType.Int32
                        ));
                    }
                    else
                    {
                        dm.Write(@"
                        update web_user_shipping_address
                        set    
                            address_line1 = :address1,
                            address_line2 = :address2,
                            address_line3 = :address3,
                            city = :city,
                            postal_index = :zip,
                            geography_id = :geographyid,
                            modified_date = :modifieddate,
                            modified_by   = :modifiedby
                         where
                            web_user_id = :wuserid and
                            address_name = :addressname
                            ", new DataParameters(
                            ":addressname", addrName, DbType.String,
                            ":address1", addr1, DbType.String,
                            ":address2", addr2, DbType.String,
                            ":address3", addr3, DbType.String,
                            ":city", city, DbType.String,
                            ":zip", zip, DbType.String,
                            ":geographyid", geographyid, DbType.Int32,
                            ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                            ":modifiedby", sd.WebUserID,
                            ":wuserid", sd.WebUserID
                            ));
                    }
                    return addressid;
                }
            }
        }

        public static void MarkAddressDefault(int addrid)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    dm.Write(@"
                        update web_user_shipping_address
                        set    
                            is_default = 'Y',
                            modified_date = :modifieddate,
                            modified_by = :modifiedby
                         where
                            web_user_shipping_address_id = :addrid 
                         ", new DataParameters(
                            ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                            ":modifiedby", sd.WebUserID, DbType.Int32,
                            ":addrid", addrid, DbType.Int32));

                    dm.Write(@"
                        update web_user_shipping_address
                        set    
                            is_default = 'N',
                            modified_date = :modifieddate,
                            modified_by   = :modifiedby
                         where
                            web_user_id = :wuserid and
                            web_user_shipping_address_id not in (:addrid)
                            ", new DataParameters(
                         ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                         ":modifiedby", sd.WebUserID, DbType.Int32,
                         ":wuserid", sd.WebUserID, DbType.Int32,
                         ":addrid", addrid, DbType.Int32
                         ));
                }
            }
        }

        public static void DeleteAddress(int addrid)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    dm.Write(@"
                        delete from 
                            web_user_shipping_address
                         where
                            web_user_shipping_address_id = :addrid 
                         ", new DataParameters(
                            ":addrid", addrid, DbType.Int32));
                }
            }
        }

        public static bool IsDefaultAddress(int addrid)
        {
            string defaultStatus;
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    defaultStatus = dm.ReadValue(@"
                                select 
                                    is_default
                                from 
                                    web_user_shipping_address
                                where 
                                    web_user_id = :webuserid
                                    and web_user_shipping_address_id = :addrid
                                ", new DataParameters(
                                    ":webuserid", sd.WebUserID, DbType.Int32,
                                    ":addrid", addrid, DbType.Int32
                                     )).ToString();

                    return (defaultStatus == "Y") ? true : false;
                }
            }
        }

        public static DataTable GetShippingAddressForOrder(int worderid)
        {
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    return dm.Read(@"
                        select distinct wora.*, g.country_code, g.adm1 as state_name, cvl.title as country_name
                        from web_order_request_address wora join geography g 
                        on wora.geography_id = g.geography_id 
                        join code_value cv 
                        on g.country_code = cv.value 
                        join code_value_lang cvl on cv.code_value_id = cvl.code_value_id 
                        where cvl.sys_lang_id = :languageid
                        and wora.web_order_request_id = :worderid
                        and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' 
                        ", new DataParameters(
                        ":worderid", worderid, DbType.Int32,
                        ":languageid", sd.LanguageID, DbType.Int32
                        ));
                }
            }
        }

        public static string GetAccessionIds(string sql)
        {
            string s = "";
            using (SecureData sd = GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    DataTable dtID = dm.Read(sql);
                    List<string> IDs = new List<string>();
                    if (dtID != null)
                    {
                        foreach (DataRow dr in dtID.Rows)
                        {
                            IDs.Add(dr[0].ToString());
                        }
                        if (IDs.Count > 0)
                            s = " @accession.accession_id in (" + String.Join(",", IDs.ToArray()) + ")";
                    }
                }
            }

            return s;
        }

        private static void checkExpiredPassword(string webUserName, string loginToken, HttpContext ctx)
        {
            int days = Toolkit.GetSetting("PasswordExpireDays", 0);
            if (days > 0)
            {
                using (SecureData sd = new SecureData(true, loginToken, null))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        string lastModified = (dm.ReadValue(@"select modified_date from web_user where user_name = :username
                                        ", new DataParameters(":username", webUserName, DbType.String))).ToString();

                        if (lastModified == "") // level 1 user
                            lastModified = (dm.ReadValue(@"select created_date from web_user where user_name = :username
                                        ", new DataParameters(":username", webUserName, DbType.String))).ToString();

                        if (lastModified != "")
                        {
                            DateTime date = Toolkit.ToDateTime(lastModified);
                            DateTime currentDate = DateTime.UtcNow;
                            if ((currentDate - date).Days > days)
                            {
                                ctx.Response.Clear();
                                ctx.Response.Redirect("~/userAcct.aspx?action=expirePwd");
                                ctx.Response.End();
                            }
                        }
                    }
                }
            }
        }


        private static void mergeCartItems(string loginToken, CartItem[] accessions)
        {
            if (accessions != null)
            {
                if (accessions.Length > 0)
                {
                    using (SecureData sd = new SecureData(true, loginToken, null))
                    {
                        Cart c = Cart.Current;
                        c.FillDB(loginToken);

                        int added = 0;
                        int id = 0;
                        string distributionFormCode = "";
                        foreach (CartItem ci in accessions)
                        {
                            id = ci.ItemID;
                            distributionFormCode = ci.DistributionFormCode;

                            added += c.AddAccession(id, distributionFormCode);
                        }
                        c.Save(loginToken);
                    }
                }
            }
        }
     }
}
