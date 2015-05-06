using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;

namespace GrinGlobal.Web
{
    public partial class cropdescriptor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["type"] != null)
                    {
                        string type = Request.QueryString["type"].ToString();
                        switch (type)
                        {
                            case "descriptor":
                                pnlDescriptor.Visible = true;
                                pnlMarker.Visible = false;
                                pnlSpecies.Visible = false;
                                pnlCitation.Visible = false;
                                bindDescriptor();
                                break;

                            case "marker":
                                pnlDescriptor.Visible = false;
                                pnlMarker.Visible = true;
                                pnlSpecies.Visible = false;
                                pnlCitation.Visible = false;
                                bindMarker();
                                break;

                            case "species":
                                pnlDescriptor.Visible = false;
                                pnlMarker.Visible = false;
                                pnlSpecies.Visible = true;
                                pnlCitation.Visible = false;
                                bindSpecies();
                                break;

                            case "citation":
                                pnlDescriptor.Visible = false;
                                pnlMarker.Visible = false;
                                pnlSpecies.Visible = false;
                                pnlCitation.Visible = true;
                                bindCitation();
                                break;

                            case "frequency":
                                break;

                            default:
                                pnlDescriptor.Visible = true;
                                pnlMarker.Visible = false;
                                pnlSpecies.Visible = false;
                                pnlCitation.Visible = false;
                                bindDescriptor();
                                break;

                        }
                    }
                }
                catch { }
            }
        }

        int id = 0;
        private void bindDescriptor()
        {
            
            if (Request.QueryString["id"] != null)
                id = Toolkit.ToInt32(Request.QueryString["id"], 0);

            using (var sd = UserManager.GetSecureData(true))
            {
                DataTable dt = null;

                dt = sd.GetData("web_crop", ":cropid=" + id, 0, 0).Tables["web_crop"];
                if (dt.Rows.Count > 0)
                {
                    string name = dt.Rows[0]["name"].ToString();
                    lblDesc.Text = "Descriptors for " + name + ":";


                    dt = sd.GetData("web_descriptorbrowse_trait_category", ":langid=" + sd.LanguageID + ";:cropid=" + id, 0, 0).Tables["web_descriptorbrowse_trait_category"];

                    if (dt.Rows.Count > 0)
                    {
                        rptDesc.DataSource = dt;
                        rptDesc.ItemDataBound += new RepeaterItemEventHandler(rptDesc_ItemDataBound);
                        rptDesc.DataBind();
                    }
                    else
                        lblDesc.Text = "No descriptor data found for " + name + ".";    
                }
            }
        }

        private void bindSpecies()
        {

            if (Request.QueryString["id"] != null)
                id = Toolkit.ToInt32(Request.QueryString["id"], 0);

            using (var sd = UserManager.GetSecureData(true))
            {
                DataTable dt = null;

                dt = sd.GetData("web_crop", ":cropid=" + id, 0, 0).Tables["web_crop"];
                if (dt.Rows.Count > 0)
                {
                    string name = dt.Rows[0]["name"].ToString();
                    lblDesc.Text = "Species included in descriptor list for " + dt.Rows[0]["name"].ToString() + ":";

                    dt = sd.GetData("web_crop_species", ":cropid=" + id, 0, 0).Tables["web_crop_species"];

                    if (dt.Rows.Count > 0)
                    {
                        rptSpecies.DataSource = dt;
                        rptSpecies.DataBind();
                    }
                    else
                        lblDesc.Text = "No species data found for " + name + ".";
                }
            }
        }


        void rptDesc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex > -1)
            {
                string cat = (string)((DataRowView)e.Item.DataItem)["category_code"];
                Repeater rptDescDetail = e.Item.FindControl("rptDescDetail") as Repeater;

                using (var sd = UserManager.GetSecureData(true))
                {
                    var dt = sd.GetData("web_crop_descriptor", ":langid=" + sd.LanguageID + ";:cat=" + cat + ";:cropid=" + id, 0, 0).Tables["web_crop_descriptor"];
                    rptDescDetail.DataSource = dt;
                    rptDescDetail.DataBind();
                }
            }
        }



        //private void bindHeader(int id)
        //{
        //    using (var sd = UserManager.GetSecureData(true))
        //    {
        //        DataTable dt = null;

        //        dt = sd.GetData("web_crop", ":cropid=" + id, 0, 0).Tables["web_crop"];
        //        if (dt.Rows.Count > 0)
        //            lblDesc.Text = dt.Rows[0]["name"].ToString();
        //    }
        //}

        private void bindCitation()
        {

            if (Request.QueryString["id"] != null)
                id = Toolkit.ToInt32(Request.QueryString["id"], 0);

            using (var sd = UserManager.GetSecureData(true))
            {
                DataTable dt = null;

                dt = sd.GetData("web_crop", ":cropid=" + id, 0, 0).Tables["web_crop"];
                if (dt.Rows.Count > 0)
                {
                    string name = dt.Rows[0]["name"].ToString();
                    lblDesc.Text = "Publications referencing accessions for " + name + ":";

                    dt = sd.GetData("web_crop_citation", ":cropid=" + id, 0, 0).Tables["web_crop_citation"];

                    if (dt.Rows.Count > 0)
                    {
                        rptCitation.DataSource = dt;
                        rptCitation.DataBind();
                    }
                    else
                        lblDesc.Text = "No citation data found for " + name + ".";

                }
            }
        }

        private void bindMarker()
        {

            if (Request.QueryString["id"] != null)
                id = Toolkit.ToInt32(Request.QueryString["id"], 0);

            using (var sd = UserManager.GetSecureData(true))
            {
                DataTable dt = null;

                dt = sd.GetData("web_crop", ":cropid=" + id, 0, 0).Tables["web_crop"];
                if (dt.Rows.Count > 0)
                {
                    string name = dt.Rows[0]["name"].ToString();
                    lblDesc.Text = "Markers for " + name + ":";


                    dt = sd.GetData("web_crop_marker", ":cropid=" + id, 0, 0).Tables["web_crop_marker"];

                    if (dt.Rows.Count > 0)
                    {
                        gvMarker.DataSource = dt;
                        gvMarker.DataBind();

                        hlView.NavigateUrl = "cropmarkerview.aspx?cropid=" + id;
                        hlView.Visible = true;

                        lbDownload.Visible = true;
                        lblS.Visible = true;
                        lblView.Text += name;
                        lblView.Visible = true;
                        hf.Value = id.ToString();
                    }
                    else
                    {
                        lblDesc.Text = "No genetic marker data found for " + name + ".";
                        hlView.Visible = false;
                        lbDownload.Visible = false;
                        lblS.Visible = false;
                        lblView.Visible = false;
                    }
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            using (var sd = UserManager.GetSecureData(true))
            {
                string id = hf.Value;
                var dt = sd.GetData("web_crop_marker_alldata", ":cropid=" + id, 0, 0).Tables["web_crop_marker_alldata"];

                if (dt.Rows.Count > 0)
                {
                    var dt2 = dt.Transform(new string[] { "crop", "method_name", "acp", "acNumber", "acs", "ivp", "ivNumber", "ivs", "ivType" }, "name", "name", "value");

                    gvAll.DataSource = dt2;
                    gvAll.DataBind();

                    Utils.ExportToExcel(HttpContext.Current, gvAll, "marker-crop" + id, "Marker data for crop ID = " + id);
                }
            }

        }


    }
}
