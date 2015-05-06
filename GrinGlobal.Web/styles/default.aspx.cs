using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.IO;

namespace GrinGlobal.Web.styles {
    public partial class _default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            var theme = "default";
            if (Toolkit.GetSetting("AllowCookies", true)) {
                var cookie = Request.Cookies["theme"];
                if (cookie != null) {
                    theme = cookie.Value;
                }
            }
            var prm = Request.QueryString["theme"];
            if (!String.IsNullOrEmpty(prm)) {
                // they're passing a querystring to override the cookie, so write it as a new cookie
                theme = prm;
                streamCSS(theme, true);
            } else {
                streamCSS(theme, false);
            }

        }


        private void streamCSS(string theme, bool writeCookie) {

            // read from file, resolve app relative paths
            if (!File.Exists(Server.MapPath("~/styles/" + theme + ".css"))) {
                // if they specify an invalid theme, use default
                theme = "default";
            }

            string css = File.ReadAllText(Server.MapPath("~/styles/" + theme + ".css"));
            string appRoot = Page.ResolveUrl("~/");
            css = css.Replace("/gringlobal/", appRoot).Replace("//", "/");



            Response.Clear();
            
            // tell browser we're giving it css, not html
            Response.ContentType = "text/css";

            // we don't need to set any other caching properties because
            // we're doing that with the @Output directive in the .aspx file

            Response.Write(css);

            // and write a cookie to use this theme on subsequent requests
            if (writeCookie) {
                if (Toolkit.GetSetting("AllowCookies", true)) {
                    var cookie = new HttpCookie("theme", theme);
                    cookie.Expires = DateTime.UtcNow.AddYears(10);
                    Response.Cookies.Add(cookie);
                }
            }

            Response.End();

        }
    }
}
