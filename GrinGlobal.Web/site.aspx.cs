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
    public partial class site : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
            }
        }

        static int sid = 0;
        private void bindData(int siteid)
        {
            sid = siteid;
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_site", ":siteid=" + siteid, 0, 0).Tables["web_site"];
                dvSite.DataSource = dt;
                dvSite.DataBind();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    if (dr["organization"] == DBNull.Value)
                        dvSite.FindControl("tr_organization").Visible = false;
                    if (dr["address_line1"] == DBNull.Value)
                        dvSite.FindControl("tr_add1").Visible = false;
                    if (dr["address_line2"] == DBNull.Value)
                        dvSite.FindControl("tr_add2").Visible = false;
                    if (dr["address_line3"] == DBNull.Value)
                        dvSite.FindControl("tr_add3").Visible = false;
                    if (dr["city"] == DBNull.Value)
                        dvSite.FindControl("tr_city").Visible = false;
                    if (dr["primary_phone"] == DBNull.Value)
                        dvSite.FindControl("tr_phone").Visible = false;
                    if (dr["secondary_phone"] == DBNull.Value)
                        dvSite.FindControl("tr_phone2").Visible = false;
                    if (dr["fax"] == DBNull.Value)
                        dvSite.FindControl("tr_fax").Visible = false;
                    if (dr["email"] == DBNull.Value)
                        dvSite.FindControl("tr_email").Visible = false;
                    if (dr["note"] == DBNull.Value)
                        dvSite.FindControl("tr_site").Visible = false;
                 

                    dt = sd.GetData("web_site_curator", ":siteid=" + siteid, 0, 0).Tables["web_site_curator"];
                    if (dt.Rows.Count > 0)
                    {
                        pnlCoop.Visible = true;
                        rptCoop.DataSource = dt;
                        rptCoop.DataBind();
                    }

                    dt = sd.GetData("web_site_crop", ":siteid=" + siteid, 0, 0).Tables["web_site_crop"];
                    if (dt.Rows.Count > 0)
                    {
                        pnlCrop.Visible = true;
                        rptCrop.DataSource = dt;
                        rptCrop.DataBind();
                    }

                    dt = sd.GetData("web_site_taxon", ":siteid=" + siteid, 0, 0).Tables["web_site_taxon"];
                    if (dt.Rows.Count > 0)
                        pnlTaxon.Visible = true;
                }
            }
        }

        protected void lbSpecies_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                Label lblTaxon = (Label)pnlTaxon.FindControl("lblTaxon");
                if (rptTaxon.Visible)
                {
                    rptTaxon.Visible = false;
                    lblTaxon.Visible = false;
                }
                else
                {
                    if (rptTaxon.Items.Count == 0)
                    {
                        dt = sd.GetData("web_site_taxon", ":siteid=" + sid, 0, 0).Tables["web_site_taxon"];
                        rptTaxon.DataSource = dt;
                        rptTaxon.DataBind();

                        dt = sd.GetData("web_site_taxon_summary", ":siteid=" + sid, 0, 0).Tables["web_site_taxon_summary"];
                        if (dt.Rows.Count > 0)
                        {
                            var dr = dt.Rows[0];
                            string summary = "<br/> &nbsp;&nbsp;" + string.Format("{0:n0}", Toolkit.ToInt32(dr[0].ToString(), 0)) + " accessions";
                            summary += "<br/> &nbsp;&nbsp;" + dr[1].ToString() + (dr[1].ToString() == "1" ? " genus " : " genera");
                            summary += "<br/> &nbsp;&nbsp;" + dr[2].ToString() + " taxa of " + dr[3].ToString() + " species <br/>";

                            lblTaxon.Text = summary;
                        }
                    }

                    rptTaxon.Visible = true;
                    lblTaxon.Visible = true;
                }
            }
        }
    }
}
