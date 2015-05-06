using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.IO;

namespace GrinGlobal.Web {
	public partial class Login : System.Web.UI.Page {
        string _action = string.Empty;

		protected void Page_Load(object sender, EventArgs e) {

            if (Session["FailedLoginCount"] != null) 
            {
                if (Convert.ToInt16(Session["FailedLoginCount"].ToString()) >= 3)
                {
                    Login1.Enabled = false;
                }
            }

            Page.Form.DefaultFocus = Login1.ClientID;

            if (Request.QueryString["action"] != null)
            {
                _action = Request.QueryString["action"].ToString();
            }

            if (!Page.IsPostBack)
            {
                if (_action == "checkout")
                    hlNot.NavigateUrl = "~/userinfor.aspx?action=checkout&level=nologin";
            }

            if (_action == "checkout" || _action == "menu" || _action == "addfav")
            {
                tr_header.Visible = true;
                td_new.Visible = true;
                td_vline.Visible = true;
                if (_action == "checkout") tr_footer.Visible = true;
            }
            else
            {
                tr_header.Visible = false;
                td_new.Visible = false;
                td_vline.Visible = false;
                tr_footer.Visible = false;
            }

 		}

        string _token = null;

		protected void Login1_Authenticate(object sender, AuthenticateEventArgs e) {
			try {
                _token = UserManager.ValidateLogin(Login1.UserName, Login1.Password, false, false);
                e.Authenticated = !String.IsNullOrEmpty(_token);

            } catch (Exception ex) {
                // NOTE: if the exception happened because of a configuration error, we want to know exactly why.
                //       if it's just a logon failure for the credentials this sender gave, the
                //       exception will always be the generic error of "Invalid user name / password combination."
                //       See GrinGlobal.Business.SecureData.doLogin() to see why.
                Login1.FailureText = ex.Message;
				e.Authenticated = false;
                if (ex.Message == "Invalid user name / password combination.") {
                    Session["FailedLoginCount"] = Convert.ToInt16(Session["FailedLoginCount"]) + 1;  // Actually just temp lockout, per session. Need DB change to track and make this lockout permanent
                    if (Convert.ToInt16(Session["FailedLoginCount"].ToString()) >= 3)
                    {
                        Login1.Enabled = false;
                        Login1.FailureText = "Your account has been locked.";
                    }
                }
			}
		}

        protected void Login1_LoggedIn(object sender, EventArgs e) {

            string returnUrl = Request.QueryString["ReturnUrl"];

            if (_action == "checkout")
            {
                returnUrl = "userinfor.aspx?action=checkout";
            }
            else
            {
                if (_action == "menu")  
                    returnUrl = "~/userinfor.aspx?action=menu";
                 else
                 {
                    if (returnUrl == null)
                        returnUrl = Page.ResolveUrl(Login1.DestinationPageUrl);
                    else
                    {
                        if (returnUrl.Contains("menu"))
                            returnUrl = "~/userinfor.aspx?action=menu";
                        else
                        {
                            if (returnUrl.Contains("login.aspx"))
                                returnUrl = Page.ResolveUrl(Login1.DestinationPageUrl);
                        }
                    }
                 }
            }

            Cart c = Cart.Current;
            //List<int> ids = c.AccessionIDs;  
            CartItem[] accessions = c.Accessions;

            // now that we're logged in, initialize the session info
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Abandon();
            }

            // and finally redirect
            returnUrl = preventOpenRedirect(returnUrl);
            UserManager.SaveLoginCookieAndRedirect(Login1.UserName, _token, Login1.RememberMeSet, returnUrl, accessions);
        }

        protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e) {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
             Response.Redirect("~/useracct.aspx");
        }

        private string preventOpenRedirect(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                string folder = Page.MapPath("~/");
                bool isValid = false;

                if (Directory.Exists(folder))
                {
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        FileInfo fi = new FileInfo(file);

                        if (returnUrl.Contains(fi.Name))
                        {
                            isValid = true;
                            break;
                        }
                    }
                }

                if (!isValid)
                {
                    folder = Page.MapPath("~/admin");
                    if (Directory.Exists(folder))
                    {
                        foreach (string file in Directory.GetFiles(folder))
                        {
                            FileInfo fi = new FileInfo(file);

                            if (returnUrl.Contains(fi.Name))
                            {
                                isValid = true;
                                break;
                            }
                        }
                    }
                }

                if (!isValid)
                {
                    folder = Page.MapPath("~/query");
                    if (Directory.Exists(folder))
                    {
                        foreach (string file in Directory.GetFiles(folder))
                        {
                            FileInfo fi = new FileInfo(file);

                            if (returnUrl.Contains(fi.Name))
                            {
                                isValid = true;
                                break;
                            }
                        }
                    }
                }

                if (!isValid)
                    returnUrl = "~/error.aspx";
                else
                {
                    string s = returnUrl.Substring(0, 1);
                    if ((s != "~") && (s != "/"))
                        returnUrl = "~/" + returnUrl;
                }
            }
            return returnUrl;
            
        }

	}
}
