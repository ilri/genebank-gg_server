using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business;


namespace GrinGlobal.Web
{
    public partial class descriptorexport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string sqlWhere = "";
            //if (Session["sqlwhere"] != null)
            //{
            //    sqlWhere = Session["sqlwhere"].ToString();

            //}
            //Label1.Text = sqlWhere;
        }

        protected void btnExportS_Click(object sender, EventArgs e)
        {
            bindData(false);
            if (gvResult.Rows.Count > 0 ) 
                Utils.ExportToExcel(HttpContext.Current, gvResult, "descriptor_query", "");
        }

        private void bindData(bool exportAll)
        {
            string sqlAll = "";
            string sqlSelect = "";
            if (Session["sqlwhere"] != null)
            {
                sqlAll = Session["sqlwhere"].ToString();

                sqlSelect =  sqlAll + ") and " + sqlAll.Substring(1, sqlAll.IndexOf("))"));

                DataTable dt = null;
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        //  only selected traits
                        if (exportAll)
                            dt = sd.GetData("web_descriptorbrowse_trait_export", ":where=" + sqlAll, 0, 0).Tables["web_descriptorbrowse_trait_export"];
                        else
                            dt = sd.GetData("web_descriptorbrowse_trait_export", ":where=" + sqlSelect, 0, 0).Tables["web_descriptorbrowse_trait_export"];

                        dt.Columns.RemoveAt(0);

                        List<string> colist = new List<string>()
	                    {
	                        "accession_surfix",
	                        "plant_name",
	                        "taxon",
                            "origin",
                            "original_value",
                            "frequency",
                            "low",
                            "hign",
                            "mean",
                            "sdev",
                            "ssize",
                            "inventory_prefix",
                            "inventory_number",
                            "inventory_suffix",
                            "accession_comment"
	                    };

                        for (int i = 0; i < cblOptions.Items.Count; i++)
                        {
                            if (!cblOptions.Items[i].Selected)
                            {
                                dt.Columns.Remove(colist[i]);   
                            }
                        }
                        gvResult.DataSource = dt;
                        gvResult.DataBind();
                    }
                }
            }
        }

        protected void btnExportA_Click(object sender, EventArgs e)
        {
            bindData(true);
            if (gvResult.Rows.Count > 0)
                Utils.ExportToExcel(HttpContext.Current, gvResult, "descriptor_query", "");
        }
    }
}
