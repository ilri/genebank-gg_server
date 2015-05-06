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
    public partial class cooperator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
            }
        }

        private void bindData(int coopID)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                DataTable dt1 = sd.GetData("web_cooperator_accessionsource", ":cooperatorid=" + coopID, 0, 0).Tables["web_cooperator_accessionsource"];
                int i1 = dt1.Rows.Count;
                if (i1 > 0)
                {
                    lblTotal.Text = "(" + i1 + " accessions)";
                    rptAccessions.DataSource = dt1;
                    rptAccessions.DataBind();
                }
                else
                    pnlAccessions.Visible = false;

                DataTable dt2 = sd.GetData("web_cooperator_methodsource", ":cooperatorid=" + coopID, 0, 0).Tables["web_cooperator_methodsource"];
                int i2 = dt2.Rows.Count;
                if (i2 > 0)
                {
                    rptMethods.DataSource = dt2;
                    rptMethods.DataBind();
                }
                else
                    pnlMethods.Visible = false;

                if (i1 == 0 & i2 == 0)
                {
                    dvCooperator.DataSource = null;
                    dvCooperator.DataBind();
                }
                else
                {
                    DataTable dt = sd.GetData("web_cooperator", ":cooperatorid=" + coopID, 0, 0).Tables["web_cooperator"];
                    dvCooperator.DataSource = dt;
                    dvCooperator.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];

                        if ((dr["title"] == DBNull.Value) && (dr["first_name"] == DBNull.Value) && (dr["last_name"] == DBNull.Value))
                            dvCooperator.FindControl("tr_name").Visible = false;
                        if (dr["organization"] == DBNull.Value)
                            dvCooperator.FindControl("tr_organization").Visible = false;
                        if (dr["address_line1"] == DBNull.Value)
                            dvCooperator.FindControl("tr_add1").Visible = false;
                        if (dr["address_line2"] == DBNull.Value)
                            dvCooperator.FindControl("tr_add2").Visible = false;
                        if (dr["address_line3"] == DBNull.Value)
                            dvCooperator.FindControl("tr_add3").Visible = false;
                        if ((dr["city"] == DBNull.Value) && (dr["state"] == DBNull.Value))
                            dvCooperator.FindControl("tr_city").Visible = false;
                        if ((dr["country"] == DBNull.Value) && (dr["postal_index"] == DBNull.Value))
                            dvCooperator.FindControl("tr_zip").Visible = false;
                        if (dr["primary_phone"] == DBNull.Value)
                            dvCooperator.FindControl("tr_phone").Visible = false;
                        if (dr["secondary_phone"] == DBNull.Value)
                            dvCooperator.FindControl("tr_phone2").Visible = false;
                        if (dr["fax"] == DBNull.Value)
                            dvCooperator.FindControl("tr_fax").Visible = false;
                        if (dr["email"] == DBNull.Value)
                            dvCooperator.FindControl("tr_email").Visible = false;

                        string recentCoopID = "";
                        recentCoopID = dr["current_cooperator_id"].ToString();
                        if (!string.IsNullOrEmpty(recentCoopID) && (recentCoopID != coopID.ToString()))
                        {
                            hlRecent.Visible = true;
                            hlRecent.NavigateUrl = "~/cooperator.aspx?id=" + recentCoopID;
                        }
                    }
                }
            }
        }
    }
}
