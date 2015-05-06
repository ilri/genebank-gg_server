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
    public partial class Maps : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["taxonomyid"] != null)
                {
                    int id = Toolkit.ToInt32(Request.QueryString["taxonomyid"], 0);
                    bindTaxonData(id);
                }
                else
                {
                    int id = Toolkit.ToInt32(Request.QueryString["id"], 0);
                    bindData(id);
                }
            }
        }

        private void bindData(int id)
        {
            bindHeaderFooter(id);

            bindMapControl(id);
        }

        private DataTable getDataViewData(string dvName, int id)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                return sd.GetData(dvName, ":accessionid=" + id, 0, 0).Tables[dvName];
            }
        }

        private void bindHeaderFooter(int id)
        {
            DataTable dt = getDataViewData("web_accession_maps_header", id);

            object[] dtItems = dt.Rows[0].ItemArray;
            lblTaxonomy.Text = dtItems[1].ToString();
            lblPINumber.Text = "<b>" + dtItems[0].ToString() + "</b>" + " ( Mapped accessions = " + dtItems[2].ToString() + ")";
            lblIDinside.Text = dtItems[0].ToString();
        }

        private void bindMapControl(int id)
        {
            DataTable dt = getDataViewData("web_accession_maps", id);

            mc1.DataSource  = dt;
            mc1.DataBind();
        }

        private void bindTaxonData(int id)
        {
            bindTaxonHeaderFooter(id);

            bindTaxonMapControl(id);
        }

        private void bindTaxonHeaderFooter(int id)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                string dvName = "web_taxonomyspecies_view_accessionmaps_header";
                DataTable dt = sd.GetData(dvName, ":taxonomy_species_id=" + id, 0, 0).Tables[dvName];

                object[] dtItems = dt.Rows[0].ItemArray;
                lblTaxonomy.Text = dtItems[0].ToString();
                lblPINumber.Text = "Mapped accessions = " + dtItems[1].ToString() + " (Total Accessions: " + dtItems[2].ToString() + ")";
                lblIDinside.Text = "";
            }
        }

        private void bindTaxonMapControl(int id)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                string dvName = "web_taxonomyspecies_view_accessionmaps";
                DataTable dt = sd.GetData(dvName, ":taxonomy_species_id=" + id, 0, 0).Tables[dvName];

                mc1.DataSource = dt;
                mc1.DataBind();
            }
        }


    }
}
