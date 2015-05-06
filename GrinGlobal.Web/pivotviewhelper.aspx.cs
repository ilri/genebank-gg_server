using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GrinGlobal.Core;
using System.IO;
using GrinGlobal.Business;

namespace GrinGlobal.Web {
    public partial class PivotViewHelper : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            string action = (Request.Params["action"] + "").ToLower();

            string fmt = (Request.Params["format"] + "").ToLower();
            if (action == "bounce" || action == "export") {

                string file = "~/uploads/exports/export_" + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                string contentType = fmt;
                // pull data from body, spit out to a file, return url to that file
                if (fmt == "csv") {
                    contentType = "text/csv";
                    file += ".csv";
                } else if (fmt == "tab") {
                    contentType = "text/plain";
                    file += ".txt";
                } else if (fmt == "") {
                    // TODO: add more cases here
                } else {
                    // assume html otherwise
                    contentType = "text/html";
                    file += ".html";
                }

                try {
                    string physicalPath = Server.MapPath(file);
                    File.WriteAllText(physicalPath, Request.Params["data"]);
                } catch (Exception ex) {
                    Response.Clear();
                    Response.Write(" { errmsg: '" + ex.Message.Replace("'", @"\'") + "'}");
                    Response.End();
                }

                Response.Clear();
                if (action == "export") {
                    Response.Redirect(Page.ResolveUrl(file));
                } else if (action == "bounce") {
                    Response.ContentType = contentType;
                    string url = Page.ResolveUrl(file);
                    Response.Write("{ url : '" + url + "' }");
                } else {
                    throw new NotImplementedException("action='" + action + "' not implemented yet.");
                }
                Response.End();


            } else {

                // cheesy web service simulator until I get the WCF / login token stuff squared around...
                string dataview = Request.QueryString["dataview"];
                if (String.IsNullOrEmpty(dataview)) {
                    dataview = "web_pivot_test2";
                }

                DataSet ds = new DataSet();

                try {

                    using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                        ds = sd.Search(
                            Request.QueryString["q"],
                            String.Compare(Request.QueryString["ic"], "true", true) == 0,
                            String.Compare(Request.QueryString["ma"], "true", true) == 0,
                            null,
                            "accession",
                            0,
                            Toolkit.ToInt32(Request.QueryString["lim"], 500),
                            0,
                            0,
                            dataview,
                            null,
                            null,
                            null);

                        DataTable dt = ds.Tables[dataview];
                        // hide all '_id' fields, assume they're needed only for internal purposes
                        foreach (DataColumn dc in dt.Columns) {
                            if (dc.ColumnName.ToLower().EndsWith("_id")) {
                                dc.ExtendedProperties["is_hidden"] = true;
                            }
                        }
                    }
                } catch (Exception ex) {
                    Response.Clear();
                    Response.Write(" { errmsg: '" + ex.Message.Replace("'", @"\'") + "'}");
                    Response.End();
                }

                Response.Clear();
                Response.Write(ds.Tables[dataview].ToJson("accession_id", "pi_number"));
                Response.End();

            }

        }
    }
}
