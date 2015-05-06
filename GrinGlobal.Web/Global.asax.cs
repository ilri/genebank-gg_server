using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using GrinGlobal.Core;
using System.Text;
using System.Diagnostics;
using GrinGlobal.Business;

namespace GrinGlobal.Web {
    public class Global : System.Web.HttpApplication {

        protected void Application_Start(object sender, EventArgs e) {

            // wipe out all caches to make sure an app restart causes a clean slate
            CacheManager.ClearAll();


            // clean up the export folder so it doesn't get huge over time
            // this will also alert us up front if there's problems being able to write to the exports folder
            // instead of exports just failing whenever they are requested
            string exportFolder = Server.MapPath("~/uploads/exports/");
            if (Directory.Exists(exportFolder)) {
                foreach (string f in Directory.GetFiles(exportFolder)) {
                    if (!f.ToLower().EndsWith("placeholder.txt")) {
                        File.Delete(f);
                    }
                }
            }

            // look up the version # of the gringlobal.web.dll so we can show it on the website
            var path = Server.MapPath("~/bin/GrinGlobal.Web.dll");
            if (File.Exists(path)) {
                var fvi = FileVersionInfo.GetVersionInfo(path);
                Application["VERSION"] = fvi.FileVersion;
            }


            // begin monitoring for emails to send
            EmailQueue.BeginMonitoring();

        }

        protected void Session_Start(object sender, EventArgs e) {
            //Session.Timeout = 1;
            //HttpContext ctx = HttpContext.Current;
            //StringBuilder sb = new StringBuilder();
            //foreach (string key in ctx.Request.Cookies) {
            //    HttpCookie ck = ctx.Request.Cookies[key];
            //    sb.AppendLine(ck.Name + "=" + ck.Value + ", expires=" + ck.Expires.ToString());
            //}
            //Logger.LogTextForcefully("session starting " + DateTime.Now.ToString() + "\r\n" + sb.ToString());
        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {
            //UserManager.Authenticate();
        }

        protected void Application_Error(object sender, EventArgs e) {
            if (!Toolkit.GetSetting("DisableFriendlyErrors", false)) {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null) {
                    Exception ex = ctx.Server.GetLastError();
                    try {
                        Logger.LogTextForcefully("Application error: " + ex.Message, ex);
                        HttpContext.Current.Server.Transfer("~/error.aspx");
                    } catch (Exception ex2) {
                        // prevent an infinite loop should the error page ... error :)
                        Logger.LogTextForcefully("Application error: " + ex2.Message, ex2);
                        Debug.WriteLine(ex2.Message);
                    }
                }
            }
        }

        protected void Session_End(object sender, EventArgs e) {
//            UserManager.Logout();
            //HttpContext ctx = HttpContext.Current;
            //StringBuilder sb = new StringBuilder();
            //foreach (HttpCookie ck in ctx.Request.Cookies) {
            //    sb.AppendLine(ck.Name + "=" + ck.Value + ", expires=" + ck.Expires.ToString());
            //}
            //Logger.LogTextForcefully("session ending " + DateTime.Now.ToString() + "\r\n" + sb.ToString());

        }

        protected void Application_End(object sender, EventArgs e) {
            EmailQueue.StopMonitoring();
        }
    }
}