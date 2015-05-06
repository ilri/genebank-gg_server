using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GrinGlobal.Web {
    public partial class View : System.Web.UI.Page {

        public string HeaderText;

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                parseQueryString();
            }
        }


        public void PivotView_LanguageChanged(object sender, EventArgs e) {
            parseQueryString();
        }

        private void parseQueryString() {
            // there are 2 main ways to call this page:
            // 1. Specify a mode and appropriate parameters (for dataviews hardwired to this page)
            // 2. Specify a dataview name, and its required parameters

            string parameters = Request.Params["params"];

            string headerDataviewName = Request.QueryString["headerdataview"] + Request.QueryString["hdv"] + Request.QueryString["headerdv"];
            if (!String.IsNullOrEmpty(headerDataviewName)) {
                using (var sd = UserManager.GetSecureData(true)) {
                    var dt = sd.GetData(headerDataviewName, parameters, 0, 0).Tables[headerDataviewName];
                    if (dt.Rows.Count > 0) {
                        HeaderText = Server.HtmlDecode(dt.Rows[0][0].ToString());
                    }
                }
            }

            var headerTitle = Request.Params["headertitle"] + Request.Params["htitle"];
            if (!String.IsNullOrEmpty(headerTitle)) {
                HeaderText = Server.HtmlDecode(headerTitle);
            }


            string format = Request.Params["format"] + string.Empty;

            switch (format.ToLower()) {
                case "xml":
                    format = "xml";
                    break;
                case "csv":
                case "comma":
                    format = "csv";
                    break;
                case "tab":
                case "txt":
                case "text":
                    format = "tab";
                    break;
                case "json":
                    format = "json";
                    break;
                case "html":
                default:
                    format = "html";
                    break;
            }

            string dataviewName = Request.Params["dataview"] + Request.Params["dv"];  // allow either 'dataview' or 'dv' in the url

            var options = "";
            var caps = (Request.Params["nocaptions"] + string.Empty).ToLower();
            var noCaptions = false;
            if (caps == "true" || caps == "1" || caps == "y") {
                // ignore captions by sending a known bogus language id
                options = "altlanguageid=-1";
                noCaptions = true;
            }
            var cpct = (Request.Params["compact"] + string.Empty).ToLower();
            var compact = cpct == "true" || cpct == "1" || cpct == "y";

            var pkName = Request.Params["pk"] + string.Empty;
            var altKeyName = Request.Params["ak"] + string.Empty;

            string errorMessage = null;
            try {
                if (!String.IsNullOrEmpty(dataviewName)) {
                    using (var sd = UserManager.GetSecureData(true)) {
                        var dt = sd.GetData(dataviewName, parameters, ggPivotView.SkipCount, ggPivotView.Limit, options).Tables[dataviewName];

                        if (format != "html") {
                            Toolkit.OutputData(dt, Response, format, noCaptions, compact);
                        } else {

                            if (!String.IsNullOrEmpty(pkName) && !String.IsNullOrEmpty(altKeyName)) {
                                ggPivotView.AllowGrouping = true;
                                ggPivotView.PrimaryKeyName = pkName;
                                ggPivotView.AlternateKeyName = altKeyName;
                            }

                            for (int i = 0; i < dt.Columns.Count; i++) {
                                var col = dt.Columns[i].ColumnName;
                                if (col.ToUpper().LastIndexOf("_ID") == col.Length - 3) {
                                    ggPivotView.PrimaryKeyName = col;
                                    break;
                                }
                            }

                            ggPivotView.DataSource = dt;
                            ggPivotView.DataBind();

                            string caption = Request.QueryString["caption"];
                            if (!String.IsNullOrEmpty(caption)) {
                                HeaderText = caption;
                            } else if (String.IsNullOrEmpty(HeaderText)) {
                                var title = dt.ExtendedProperties["Title"] as string;
                                if (!String.IsNullOrEmpty(title)) {
                                    HeaderText = title;
                                }
                            }
                            if (!String.IsNullOrEmpty(HeaderText)) {
                                HeaderText = HeaderText.Replace("__USERNAME__", UserManager.GetUserName());
                            }
                        }
                    }
                } else {
                    if (format != "html") {
                        throw new InvalidOperationException("No dataview name provided");
                    } else {
                        ggPivotView.DataSource = null;
                        ggPivotView.DataBind();
                        HeaderText = "Please specify a dataview to load by passing in 'dv' or 'dataview' on the querystring.&nbsp;&nbsp;Example:  <br /><br />view.aspx?dv=web_accessiondetail_header&amp;params=:accessionid=382733;:inventoryid=0;orderrequestid=0";
                    }
                }
            } catch (Exception ex) {
                if (format == "html") {
                    // be sure to puke hard if major problems happen and we're emitting html (not streaming raw data)
                    throw;
                } else {
                    // ASPNET doesn't respond well to Response.End() within a try/catch, so we wait until we're out of it.
                    errorMessage = ex.Message;
                }
            }
            if (format != "html") {
                if (!String.IsNullOrEmpty(errorMessage)) {
                    Response.Write("***ERROR***: " + errorMessage);
                }
                Response.End();
            }
        }
    }
}
