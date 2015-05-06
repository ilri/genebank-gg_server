using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;

namespace GrinGlobal.Web {
    public partial class StreamLogin : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            Response.Clear();
            Response.ContentType = "text/plain";

            var un = Request["username"];
            var pw = Request["password"];

            if (String.IsNullOrEmpty(un) || String.IsNullOrEmpty(pw)) {
                invalid(null);
            } else {
                try {
                    var hash = SecureData.Login(un, pw);
                    Response.Write(hash);
                } catch (Exception ex) {
                    invalid("Login failed: " + ex.Message);
                }
            }
        }

        private void invalid(string message) {
            Response.Write("***Error*** ");
            if (String.IsNullOrEmpty(message)) {
                Response.Write("\r\n\r\n");
                Response.Write("To create a login token to use with StreamData.aspx, values for both 'username' and 'password' must be sent via an HTTP GET or POST request to this page.\r\n");
                Response.Write("\r\n\r\n");
            } else {
                Response.Write(message);
            }
            Response.End();
        }
    }
}
