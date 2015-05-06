using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace GrinGlobal.Web {
    public partial class Error : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Page.Title = "Error";
            Exception ex = (Server.GetLastError());
            if (ex == null) {
                Response.Redirect("~/search.aspx");
            } else {
                if (Page.User.IsInRole("Admins"))
                {
                    pnlError.Visible = true;
                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }

                    var sb = new StringBuilder("<pre>" + ex.ToString() + "</pre>");
                    if (ex.Data != null)
                    {
                        sb.Append("<h3>Additional Data</h3><table border='1' cellpadding='1' cellspacing='1' class='grid'><tr><th>Key</th><th>Value</th></tr>");
                        foreach (var k in ex.Data.Keys)
                        {
                            var val = ex.Data[k];
                            sb.Append("<tr><td><pre>" + k + "</pre></td><td><pre>" + val + "</pre></td></tr>");
                        }
                        sb.Append("</table>");
                    }
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        sb.Append("<h3>Last Request:&nbsp;&nbsp;&nbsp;").Append(HttpContext.Current.Request.Url).Append("</h3>");
                    }

                    ErrorMessage = ex.Message;
                    ErrorString = sb.ToString();
                }
                else
                    pnlError.Visible = false;

                Server.ClearError();
            }
        }

        public string ErrorMessage;
        public string ErrorString;
    }
}
