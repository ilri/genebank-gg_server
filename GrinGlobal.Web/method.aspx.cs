using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web
{
    public partial class method : System.Web.UI.Page
    {
        static int _methodid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _methodid = Toolkit.ToInt32(Request.QueryString["id"], 0);
                bindData(_methodid);
            }
        }

        private void bindData(int methodID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_method", ":methodid=" + methodID, 0, 0).Tables["web_method"];
                dvMethod.DataSource = dt;
                dvMethod.DataBind();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    if (string.IsNullOrEmpty(dr["state_name"].ToString()) && string.IsNullOrEmpty(dr["country_name"].ToString()))
                        dvMethod.FindControl("tr_location").Visible = false;
                    if (dr["methods"] == DBNull.Value)
                        dvMethod.FindControl("tr_method").Visible = false;
                }

                dt = sd.GetData("web_citation", ":methodid=" + methodID + ";:iprid=0;:pedigreeid=0;:markerid=0", 0, 0).Tables["web_citation"];
                if (dt.Rows.Count > 0)
                {
                    plCitations.Visible = true;
                    rptCitations.DataSource = dt;
                    rptCitations.DataBind();
                }

                dt = sd.GetData("web_method_cooperator", ":methodid=" + methodID, 0, 0).Tables["web_method_cooperator"];
                if (dt.Rows.Count > 0)
                {
                    plResearcher.Visible = true;
                    rptCoop.DataSource = dt;
                    rptCoop.DataBind();
                }

                dt = sd.GetData("web_method_descriptor", ":methodid=" + methodID, 0, 0).Tables["web_method_descriptor"];
                if (dt.Rows.Count > 0)
                {
                    plTrait.Visible = true;
                    rptAccession.DataSource = dt;
                    rptAccession.DataBind();
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_method_download", ":methodid=" +_methodid, 0, 0).Tables["web_method_download"];

                dt = dt.Transform(new string[] { "crop", "method_name", "taxon", "acp", "acno", "acs" }, "coded_name", "coded_name", "observation_value");
                gv1.DataSource = dt;
                gv1.DataBind();

                Utils.ExportToExcel(HttpContext.Current, gv1, "method-trait", "Query Result");
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/display.aspx?methodid=" + _methodid);
            Server.Transfer("~/display.aspx?methodid=" + _methodid);
        }

    }
}
