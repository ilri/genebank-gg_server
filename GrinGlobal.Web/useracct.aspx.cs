using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business;
using System.Text;
using System.Security.Cryptography;

namespace GrinGlobal.Web
{
    public partial class UserAcct : System.Web.UI.Page
    {
        static string _prevPage = String.Empty;
        static string action = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _prevPage = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();

                if (Request.QueryString["action"] != null)
                {
                    action = Request.QueryString["action"];

                    switch (action)
                    {
                        case "editAcct":
                            mv1.SetActiveView(vwUpdateAcct);
                            txtNewUserName.Text = UserManager.GetUserName();
                            this.Form.DefaultButton = ((Button)mv1.FindControl("btnUpdateAcct")).UniqueID;
                            if (!Toolkit.GetSetting("RequireStrongPassword", false))
                                lblHelpPwdC.Visible = false;
                            else
                                lblHelpPwdC.Visible = true;
                            break;
                        case "forgotPassword":
                            mv1.SetActiveView(vwForgotPwd);
                            this.Form.DefaultButton = ((Button)mv1.FindControl("btnContinue")).UniqueID;
                            break;
                        case "forgotPasswordEmail":

                        case "expirePwd":
                            mv1.SetActiveView(vwUpdateAcct);
                            txtNewUserName.Text = UserManager.GetUserName();
                            this.Form.DefaultButton = ((Button)mv1.FindControl("btnUpdateAcct")).UniqueID;
                            this.lblChgPassword.Text = "Your password has expired. Please use this form to change your password now.";
                            this.txtNewUserName.Enabled = false;
                            if (!Toolkit.GetSetting("RequireStrongPassword", false))
                                lblHelpPwdC.Visible = false;
                            else
                                lblHelpPwdC.Visible = true;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    this.Form.DefaultButton = ((Button)mv1.FindControl("btnCreateAcct")).UniqueID;
                    if (!Toolkit.GetSetting("RequireStrongPassword", false))
                        lblHelpPwdR.Visible = false;
                    else
                        lblHelpPwdR.Visible = true;

                }
             }
        }

        protected void btnCancelAcct_Click(object sender, EventArgs e)
        {
            if (action == "expirePwd") UserManager.Logout();
            Response.Redirect(_prevPage);
        }

        protected void btnCreateAcct_Click(object sender, EventArgs e)
        {
            if (save(txtUserName.Text, txtPassword.Text, txtConPassword.Text))
            {
                sendEmailToNewAcct(txtUserName.Text);

                // login so to preserve cart content if any
                Session["newRegistration"] = true;
                string token = UserManager.ValidateLogin(txtUserName.Text, txtPassword.Text, false, false);
                UserManager.SaveLoginCookieAndRedirect(txtUserName.Text, token, true, "~/UserInfor.aspx", null);

                Master.ShowMessage(Page.GetDisplayMember("UserAcct", "accountCreated", "Your account has been created successfully."));
            }
        }

        private bool save(string name, string password, string conpassword)
        {
            if (!checkPassword(password)) return false;

            if (password != conpassword)
            {
                Master.ShowError(Page.GetDisplayMember("UserAcct", "mismatchedPasswords", "The two passwords you entered did not match each other. Please try again."));
                return false;
            }

            string hashedPassword = Crypto.HashText(txtPassword.Text);
            hashedPassword = Crypto.HashText(hashedPassword);  
            DataTable dt = null;

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    dm.Limit = 1;
                    dt = dm.Read(@"
                        select 
                            wu.web_user_id, wu.web_cooperator_id
                        from 
                            web_user wu 
                        where 
                            wu.user_name = :username 
                            and wu.is_enabled = 'Y'
                        ", new DataParameters(":username", name)); //Won't allow same user_name(e-mail) address


                    if (dt.Rows.Count > 0)
                    {
                        Master.ShowMessage(Page.GetDisplayMember("UserAcct", "usernameInUse", "The user name is already used.")); 
                        return false;
                    }

                    var userID = dm.Write(@"
                        insert into web_user
                        (user_name, password, is_enabled, sys_lang_id, created_date)
                        values
                        (:user_name, :password, :is_enabled, :seclangid, :createddate)
                        ", true, "web_user_id", new DataParameters(
                    ":user_name", name, DbType.String,
                    ":password", hashedPassword, DbType.String,
                    ":is_enabled", "Y", DbType.String,
                    ":seclangid", 1, DbType.Int32,
                    ":createddate", DateTime.UtcNow, DbType.DateTime2
                    ));
                }
            }
            return true;
        }

        protected void btnUpdateAcct_Click(object sender, EventArgs e)
        {
            bool changedPwd = false;
            bool changedName = false;
            string name = "";

            if (txtNewPassword.Text != "")
            {
                if (txtNewPassword.Text != txtNewConPassword.Text)
                {
                    Master.ShowError(Page.GetDisplayMember("UserAcct", "mismatchedPasswords", "The two passwords you entered did not match each other. Please try again."));
                    return;
                }
                if (!checkPassword(txtNewPassword.Text)) return;
            }

            if (txtNewUserName.Text != UserManager.GetUserName()) changedName = true;

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    string sCurPwdDB = (dm.ReadValue(@"
                                select 
                                    password 
                                from 
                                    web_user
                                where 
                                    web_user_id = :userid
                                ", new DataParameters(":userid", sd.WebUserID, DbType.Int32))).ToString();

                    string encryptedCurPassword = Crypto.HashText(txtCurPassword.Text);
                    encryptedCurPassword = Crypto.HashText(encryptedCurPassword);

                    string encryptedNewPassword = Crypto.HashText(txtNewPassword.Text);
                    encryptedNewPassword = Crypto.HashText(encryptedNewPassword);
     
                    if (encryptedCurPassword != sCurPwdDB)
                    {
                        Master.ShowError(Page.GetDisplayMember("UserAcct", "badCurrentPassword", "The current password you entered is not correct. Please try again."));
                        return;
                    }

                    if ((sCurPwdDB != encryptedNewPassword) && !String.IsNullOrEmpty(encryptedNewPassword)) changedPwd = true;
                    if (!changedName && !changedPwd)
                    {
                        Master.ShowError(Page.GetDisplayMember("UserAcct", "newEmailRequired", "Please enter a new e-mail address or a new password before clicking the \"Update Account\" button."));
                        return;
                    }

                    name = txtNewUserName.Text;
                    
                    if (String.IsNullOrEmpty(encryptedNewPassword)) encryptedNewPassword = sCurPwdDB;

                    dm.Write(@"
                            update web_user 
                            set
                                user_name = :user_name,
                                password = :password,
                                modified_date = :modifieddate
                             where
                                web_user_id = :webuserid
                                ", new DataParameters(
                                ":webuserid", sd.WebUserID, DbType.Int32,
                                ":user_name", name, DbType.String,
                                ":password", encryptedNewPassword, DbType.String,
                                ":modifieddate", DateTime.UtcNow, DbType.DateTime2
                                ));
                }
            }

            Master.ShowMessage(Page.GetDisplayMember("UserAcct", "accountUpdated", "Your account has been updated."));
            if (changedPwd)
            {
                StringBuilder sb = new StringBuilder();
                string CRLF = "\r\n";
                sb.Append("-----------------------------------------------");
                sb.Append(CRLF + "This is to notify you that you have successfully changed your GRIN-Global public website login password. " + CRLF);
                sb.Append("-----------------------------------------------");

                try
                {
                    Email.Send(name,
                                Toolkit.GetSetting("EmailFrom", ""),
                                "",
                                "",
                                "Password Change Acknowledgement",
                                sb.ToString());
                }
                catch (Exception ex)
                {
                    string s = ex.Message; 
                    Logger.LogTextForcefully("Application error: Sending email failed for Password Change Acknowledgement user name " + name + ". ", ex);
                }

                if (action == "expirePwd") _prevPage = "~/search.aspx";
            }
            Response.Redirect(_prevPage);
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            string newTempPW;
            string userName = (txtForgotPwdName.Text).Trim();
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    string sCurPwdDB = (dm.ReadValue(@"
                                select 
                                    password 
                                from 
                                    web_user
                                where 
                                    user_name = :username
                                ", new DataParameters(":username", userName, DbType.String))).ToString();

                    if (String.IsNullOrEmpty(sCurPwdDB))
                    {
                        Master.ShowError(Page.GetDisplayMember("UserAcct", "badLogin", " The email address entered does not match any accounts on record. Please try again."));
                        return;
                    }
                    else
                    {
                        newTempPW = CreateRandomPassword(8, 1); // Length 8 for now
                        string userPassword = Crypto.HashText(newTempPW);
                        userPassword = Crypto.HashText(userPassword);

                        dm.Write(@"
                            update web_user 
                            set
                                password = :password,
                                modified_date = :modifieddate
                             where
                                user_name = :username
                                ", new DataParameters(
                        ":username", userName, DbType.String,
                        ":password", userPassword, DbType.String,
                        ":modifieddate", DateTime.UtcNow, DbType.DateTime2
                        ));
                    }
                }
            }
 
            StringBuilder sb = new StringBuilder();
            string CRLF = "\r\n";
            sb.Append("-----------------------------------------------");
            sb.Append(CRLF + "Your new temporary password is: " + newTempPW + CRLF);
            sb.Append(CRLF + "You can change the password (Edit Account), after logging into the system."  + CRLF);
            sb.Append("-----------------------------------------------");

            try
            {
                Email.Send(userName,
                            Toolkit.GetSetting("EmailFrom", ""),
                            "",
                            "",
                            "GRIN-GLOBAL - Forgot Password",
                            sb.ToString());
            }
            catch (Exception ex)
            {
                // debug, nothing we can/need to do if mail failed to send.
                string s = ex.Message;
                Logger.LogTextForcefully("Application error: Sending email failed for password forgotten for " + userName + ". The new  temp password is " + newTempPW + " ", ex);
            }
            finally
            {
                mv1.SetActiveView(vwForgotPwdEmail);
                lblEmail.Text = userName;
            }
        }

        private void sendEmailToNewAcct(string emailToaddress)
        {
            //emailToaddress = "laura.gu@ars.usda.gov"; // laura test

            StringBuilder sb = new StringBuilder();
            string CRLF = "\r\n";
            sb.Append("You have created the account with us. You can log into your online account at anytime to: ");
            sb.Append(CRLF + "- View your up to date shopping cart information");
            sb.Append(CRLF + "- View your user preferences");
            sb.Append(CRLF + "- And More!");

            try
            {
                Email.Send(emailToaddress,
                            Toolkit.GetSetting("EmailFrom", ""),
                            "",
                            "",
                            "GRIN-GLOBAL - Your online account has been created",
                            sb.ToString());
            }
            catch (Exception ex)
            {
                // debug, nothing we can/need to do if mail failed to send.
                string s = ex.Message;
                Logger.LogTextForcefully("Application error: Sending email failed for new account " + emailToaddress + ". ", ex);
            }
            finally { }
        }

        private string CreateRandomPassword(int PasswordLength, int numberOfNonAlphanumericCharacters)
        {
            String _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789_@#$%&*";
            String _specChars = "_@#$%&*";
            char[] checkChar = new char[PasswordLength];
            int countSpecChar = 0;
            Byte[] randomBytes = new Byte[PasswordLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                //checkChar[1] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                //if (_specChars.IndexOf(checkChar[1]) > 0)
                //{
                //    countSpecChar++;
                //    if (countSpecChar == 1)
                //    {
                //        chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                //    }
                //}
                //else
                //{
                //    chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                //}

                // allow more than one special character
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                if (_specChars.IndexOf(chars[i]) > 0) countSpecChar++;
            }

            if (countSpecChar < 1)
            {
                Random rndNumber = new Random();
                chars[rndNumber.Next(1, PasswordLength - 1)] = _specChars[rndNumber.Next(0, _specChars.Length)];

            }
            return new string(chars);
        }

 
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                string s = Request.QueryString["action"];
                if (s == "expirePwd") this.MasterPageFile = "Site2.Master";
            }
        }

        private bool checkPassword(string strPassword)
        {
            if (!Toolkit.GetSetting("RequireStrongPassword", false)) return true;
            int minLength = Toolkit.GetSetting("PasswordMinLength", 8);
            int maxLength = Toolkit.GetSetting("PasswordMaxLength", 12);

            if (strPassword.Length < minLength)
            {
                Master.ShowError(Page.GetDisplayMember("UserAcct", "length8", "Password must be a minimum of eight characters."));
                return false;
            }

            if (strPassword.Length > maxLength)
            {
                Master.ShowError(Page.GetDisplayMember("UserAcct", "length12", "Password must be between eight and twelve characters."));
                return false;
            }

            //string pwd = Toolkit.GetSetting("PasswordCharacterPatten1", @"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$");
            string pwd = Toolkit.GetSetting("PasswordCharacterPatten1", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).+$");
            System.Text.RegularExpressions.Regex passExp = new System.Text.RegularExpressions.Regex(pwd);
            bool boolValidPass = passExp.IsMatch(strPassword);
            if (boolValidPass)
                return true;
            else
            {
                Master.ShowError(Page.GetDisplayMember("UserAcct", "passwordInvalid", "Password must contain at least one lower case, one upper case, one numeric character, and one special character.  Passwords are case-sensitive."));
                return false;
            }
        } 
    }
}
