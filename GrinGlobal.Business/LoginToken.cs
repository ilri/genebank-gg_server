using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using GrinGlobal.Core;
using System.Diagnostics;

namespace GrinGlobal.Business {
    /// <summary>
    /// An encrypted representation of a user's (system or web) login.  Does not store password, but does store user/cooperator id's, language id, remote ip, groups (Roles), etc.
    /// </summary>
    internal class LoginToken {

        public int UserID;
        public int CooperatorID;
        public string UserName;

        public int WebUserID;
        public int WebCooperatorID;
        public string WebUserName;

        public int LanguageID;
        public string LanguageDirection;

        public long CreatedAt;
        public string RemoteIP;
        public string[] Roles;

        public LoginToken(int userID, int cooperatorID, string userName, int webUserID, int webCooperatorID, string webUserName, int languageID, string languageDirection, string[] roles) {

            this.UserID = userID;
            this.CooperatorID = cooperatorID;
            this.UserName = userName;

            this.WebUserID = webUserID;
            this.WebCooperatorID = webCooperatorID;
            this.WebUserName = webUserName;

            this.LanguageID = languageID;
            this.LanguageDirection = languageDirection;

            this.CreatedAt = DateTime.UtcNow.Ticks;
            this.Roles = roles;

            this.RemoteIP = getRemoteIP();


        }

        /// <summary>
        /// determine their remote IP, default to 127.0.0.1 if not found (i.e. no HttpContext).  If an http proxy header is found (HTTP_X_FORWARDED_FOR), will return that value.
        /// </summary>
        /// <returns></returns>
        private string getRemoteIP() {

            var remoteIP = "";
            if (HttpContext.Current != null) {

                if (HttpContext.Current.Request != null) {
                    remoteIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                    // if they're using an HTTP proxy, let's try to grab their 'real' IP address if possible...
                    string remoteIPBehindProxy = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!String.IsNullOrEmpty(remoteIPBehindProxy)) {
                        remoteIP = remoteIPBehindProxy;
                    }
                }
            }

            if (String.IsNullOrEmpty(remoteIP)) {
                remoteIP = "127.0.0.1";
            }

            return remoteIP;
        }

        public override string ToString() {
            return Crypto.EncryptText(String.Concat(
                UserID, "|", 
                CooperatorID, "|", 
                UserName, "|",
                WebUserID, "|", 
                WebCooperatorID, "|", 
                WebUserName, "|",
                LanguageID, "|", 
                LanguageDirection, "|",
                CreatedAt, "|", 
                RemoteIP, "|", 
                String.Join("~", Roles)));
        }

        /// <summary>
        /// Parses the encrypted login token and returns embedded values.  Will throw exception on encryption failure, IP mismatch, or expired token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static LoginToken Parse(string token) {

            if (String.IsNullOrEmpty(token)) {
                // empty token, don't try to decrypt
                return null;
            }

            string cipherText = string.Empty;
            try {
                cipherText = Crypto.DecryptText(token);
            } catch (Exception ex) {
                Debug.WriteLine("decryption failed: " + ex.Message);
            }
            if (String.IsNullOrEmpty(cipherText)) {
                // bad decryption, trash token
                return null;
            }

            string[] values = cipherText.Split('|');

            LoginToken tok = null;

            try {
                tok = new LoginToken(
                    Toolkit.ToInt32(values[0], -1),
                    Toolkit.ToInt32(values[1], -1),
                    values[2],
                    Toolkit.ToInt32(values[3], -1),
                    Toolkit.ToInt32(values[4], -1),
                    values[5],
                    Toolkit.ToInt32(values[6], -1),
                    values[7], 
                    values[10].Split('~'));

                long.TryParse(values[8], out tok.CreatedAt);
                tok.RemoteIP = values[9];


            } catch (Exception ex) {
                // any errors, assume invalid token
                // throw Library.CreateBusinessException("Error parsing loginToken: " + ex.Message, ex);
                Debug.WriteLine(ex.Message);
                return null;
            }

            var actualRemoteIP = tok.getRemoteIP();
            if (actualRemoteIP != tok.RemoteIP) {
                // IP mismatch, trash the token.
                // throw Library.CreateBusinessException("Token is invalid -- IP mismatch");
                return null;
            }

            // TODO: we're ignoring the login token timeout for now, relying on forms ticket auth to handle that.
            //var ts = new TimeSpan(Math.Abs(DateTime.UtcNow.Ticks - tok.CreatedAt));
            //if (ts.Minutes > Toolkit.GetSetting("LoginTokenTimeout", 20)) {
            //    // token expired, trash it.
            //    //throw Library.CreateBusinessException("Token has expired.");
            //    return null;
            //}



            // we get here, everything loaded successfully. let them through!
            return tok;

        }
    }
}
