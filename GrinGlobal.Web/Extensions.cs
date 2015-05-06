using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GrinGlobal.Core;
using GrinGlobal.Business;

namespace GrinGlobal.Web {
    public static class Extensions {

        public static string GetDisplayMember(this Page pg, string pageName, string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "WebTool", pageName, resourceName, null, defaultValue, substitutes);
        }

        public static string GetDisplayMember(this Page pg, string pageName, DataConnectionSpec dcs, int languageId, string resourceName, string defaultValue, params string[] substitutes)
        {
            return ResourceHelper.GetDisplayMember(dcs, "WebTool", pageName, resourceName, languageId, defaultValue, substitutes);
        }

        public static bool ToBoolean(this MasterPage mp, object o) {
            return Toolkit.ToBoolean(o, false);
        }
        public static bool ToBoolean(this MasterPage mp, object o, bool defaultValue) {
            return Toolkit.ToBoolean(o, defaultValue);
        }

        public static string DisplayText(this Page pg, string controlID, string defaultValue) {
            try {
                return Site1.DisplayText(controlID, defaultValue).ToString();
            } catch {
                return defaultValue;
            }
        }

        public static string DisplayText(this Page pg, string controlID, string defaultValue, params string[] substitutes) {
            try {
                return Site1.DisplayText(controlID, defaultValue, substitutes).ToString();
            }
            catch {
                return defaultValue;
            }
        }

        public static string DescriptionText(this Page pg, string controlID, string defaultValue) {
            try {
                return Site1.DescriptionText(controlID, defaultValue).ToString();
            } catch {
                return defaultValue;
            }
        }

        public static void ApplyCaptions(this GridView gv) {
            if (gv.DataSource is DataTable) {

                DataTable dt = gv.DataSource as DataTable;

                foreach (DataControlField dcf in gv.Columns) {
                    if (dt.Columns.Contains(dcf.HeaderText)) {
                        dcf.HeaderText = dt.Columns[dcf.HeaderText].Caption;
                    }
                }
            }
        }

        public static void SetTitle(this Page pg, string title) {
            //if (String.IsNullOrEmpty(title)) {
            //    pg.Title = "GRIN-Global Web v " + HttpContext.Current.Application["VERSION"] + " " + pg.Title;
            //} else {
            //    pg.Title = title + " - GRIN-Global Web v " + HttpContext.Current.Application["VERSION"] + " " + pg.Title; // + Toolkit.GetApplicationVersion("GRIN-Global Web");
            //}
            // try specific title first
            if (String.IsNullOrEmpty(pg.Title))
            {
                pg.Title = pg.Title + " GRIN-Global Web v " + HttpContext.Current.Application["VERSION"] ;
            }
            else
            {
                pg.Title = pg.Title + " - GRIN-Global Web v " + HttpContext.Current.Application["VERSION"];
            }

        }

        public static CartControl GetCartControl(this MasterPage mp) {
            CartControl cc = mp.FindControl("ggCart") as CartControl;
            return cc;
        }

        public static void ShowError(this MasterPage mp, string error) {
            Label lbl = mp.FindControl("lblError") as Label;
            if (lbl != null) {
                lbl.Text = error;
                lbl.Visible = !String.IsNullOrEmpty(lbl.Text);
            }
        }

        public static void ShowMessage(this MasterPage mp, string message) {
            Label lbl = mp.FindControl("lblMessage") as Label;
            if (lbl != null) {
                lbl.Text = message;
                lbl.Visible = !String.IsNullOrEmpty(lbl.Text);
            }
        }

        public static void ShowMessageRed(this MasterPage mp, string message)
        {
            Label lbl = mp.FindControl("lblMessageRed") as Label;
            if (lbl != null)
            {
                lbl.Text = message;
                lbl.Visible = !String.IsNullOrEmpty(lbl.Text);
            }
        }

        public static void ShowMore(this MasterPage mp, string message)
        {
            Label lbl = mp.FindControl("lblMore") as Label;
            if (lbl != null)
            {
                lbl.Text = message;
                lbl.Visible = !String.IsNullOrEmpty(lbl.Text);
            }
        }

        public static void FlashMessage(this MasterPage mp, string message) {
            FlashMessage(mp, message, null, null, null);
        }
        public static void FlashMessage(this MasterPage mp, string message, string queryString) {
            FlashMessage(mp, message, null, queryString, null);
        }

        public static void FlashMessage(this MasterPage mp, string message, string destinationPage, string queryString) {
            FlashMessage(mp, message, destinationPage, queryString, null);
        }

        public static void FlashMessage(this MasterPage mp, string message, string destinationPage, string queryString, string javascriptToRunAfterRedirect) {


            HttpContext ctx = HttpContext.Current;

            if (ctx != null && ctx.Session != null && ctx.Request != null) {

                ctx.Session["msg"] = message;
                ctx.Session["jsAfterRedirect"] = javascriptToRunAfterRedirect;

                if (String.IsNullOrEmpty(destinationPage)) {
                    destinationPage = ctx.Request.Url.AbsolutePath;
                }
                if (!String.IsNullOrEmpty(queryString)) {
                    if (queryString.StartsWith("?")) {
                        destinationPage += queryString;
                    } else {
                        destinationPage += "?" + queryString;
                    }
                }

                ctx.Response.Clear();
                ctx.Response.Redirect(destinationPage);
                ctx.Response.End();
            }
        }

        public static void FlashError(this MasterPage mp, string error) {
            FlashError(mp, error, null, null);
        }
        public static void FlashError(this MasterPage mp, string error, string queryString) {
            FlashError(mp, error, queryString, null);
        }
        public static void FlashError(this MasterPage mp, string error, string queryString, string javascriptToRunAfterRedirect) {

            HttpContext ctx = HttpContext.Current;
            if (ctx != null && ctx.Session != null && ctx.Request != null) {

                ctx.Session["err"] = error;
                ctx.Session["jsAfterRedirect"] = javascriptToRunAfterRedirect;

                string dest = ctx.Request.Url.AbsolutePath;
                if (!String.IsNullOrEmpty(queryString)) {
                    if (queryString.StartsWith("?")) {
                        dest += queryString;
                    } else {
                        dest += "?" + queryString;
                    }
                }

                ctx.Response.Clear();
                ctx.Response.Redirect(dest);
                ctx.Response.End();
            }
        }

        public static void ShowFlashError(this MasterPage mp, Label lbl) {

            lbl.Text = mp.GetFlashError();
            lbl.Visible = !String.IsNullOrEmpty(lbl.Text);

            string jsToRun = mp.GetJavascriptToRun();
            if (!String.IsNullOrEmpty(jsToRun)) {
                mp.Page.ClientScript.RegisterStartupScript(mp.GetType(), "jsAfterRedirect", jsToRun, true);
            }

        }


        public static void ShowFlashMessage(this MasterPage mp, Label lbl) {

            lbl.Text = mp.GetFlashMessage();
            lbl.Visible = !String.IsNullOrEmpty(lbl.Text);

            string jsToRun = mp.GetJavascriptToRun();
            if (!String.IsNullOrEmpty(jsToRun)) {
                mp.Page.ClientScript.RegisterStartupScript(mp.GetType(), "jsAfterRedirect", jsToRun, true);
            }

        }

        public static string GetFlashMessage(this MasterPage pg) {
            string msg = "";
            HttpContext ctx = HttpContext.Current;
            if (ctx != null & ctx.Session != null) {
                msg = ctx.Session["msg"] as string;
                ctx.Session["msg"] = null;
            }

            return msg;
        }

        public static string GetJavascriptToRun(this MasterPage pg) {
            string js  = "";
            HttpContext ctx = HttpContext.Current;
            if (ctx != null & ctx.Session != null) {
                js = ctx.Session["jsAfterRedirect"] as string;
                ctx.Session["jsAfterRedirect"] = null;
            }
            return js;
        }

        public static string GetFlashError(this MasterPage pg) {
            string err = "";
            HttpContext ctx = HttpContext.Current;
            if (ctx != null & ctx.Session != null) {
                err = ctx.Session["err"] as string;
                ctx.Session["err"] = null;
            }

            return err;
        }

        // Shows a client-side JavaScript alert in the browser.
        public static void Show(string message)
        {
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Extensions), "alert", script);
            }
        }

        /// <summary>
        /// Lists all values within the given code group
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="ddl"></param>
        /// <param name="groupName"></param>
        /// <param name="sd"></param>
        public static void FillCodeGroupDropDown(this MasterPage mp, DropDownList ddl, string groupName, SecureData sd) {
            ddl.DataSource = null;
            ddl.DataTextField = "display_member";
            ddl.DataValueField = "value_member";

            var dt = sd.GetData("importlist_code_value", ":groupname=" + groupName + ";:langid=" + sd.LanguageID, 0, 0).Tables["importlist_code_value"];
            ddl.DataSource = dt;
            ddl.DataBind();

        }

        /// <summary>
        /// Lists all code groups
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="ddl"></param>
        /// <param name="sd"></param>
        public static void FillCodeGroupDropDown(this MasterPage mp, DropDownList ddl, SecureData sd) {
            ddl.DataSource = null;
            ddl.DataTextField = "display_member";
            ddl.DataValueField = "value_member";

            var dt = sd.GetData("group_name_lookup", null, 0, 0).Tables["group_name_lookup"];
            ddl.DataSource = dt;
            ddl.DataBind();

        }

        public static void FillLookupDropDown(this MasterPage mp, DropDownList ddl, SecureData sd) {
            ddl.DataSource = null;
            ddl.DataTextField = "title";
            ddl.DataValueField = "dataview_name";

            var dt = sd.GetData("get_lookup_table_list", null, 0, 0).Tables["get_lookup_table_list"];
            ddl.DataSource = dt;
            ddl.DataBind();

        }

        public static void FillWebLookupDropDown(this MasterPage mp, DropDownList ddl, SecureData sd) {
            ddl.DataSource = null;
            ddl.DataTextField = "title";
            ddl.DataValueField = "dataview_name";

            var dt = sd.GetData("web_lookup_list", null, 0, 0).Tables["web_lookup_list"];
            ddl.DataSource = dt;
            ddl.DataBind();

        }

    }
}
