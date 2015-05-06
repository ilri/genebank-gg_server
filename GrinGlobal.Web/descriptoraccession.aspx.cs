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
    public partial class descriptoraccession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int traitID = Toolkit.ToInt32(Request.QueryString["id1"], 0);
                
                int type = Toolkit.ToInt32(Request.QueryString["type"].ToString(), 0);

                if (type == 1)
                {
                    int codeID = Toolkit.ToInt32(Request.QueryString["id2"], 0);
                    if (traitID > 0 && codeID > 0)
                        bindData1(traitID, codeID);
                }
                else if (type == 2)
                {
                    string value = Request.QueryString["id2"].ToString();
                    if (traitID > 0 && value != "")
                        bindData2(traitID, value);
                }
                else if (type == 3)
                {
                    string value = Request.QueryString["id2"].ToString();
                    if (traitID > 0 && value.Contains(";"))
                        bindData3(traitID, value);
                }
            }
        }

        private void bindData1(int traitID, int codeID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    string traitName = dm.ReadValue(@"select coded_name from crop_trait where crop_trait_id = :ctid", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();
                    string cropName = dm.ReadValue(@"select name from crop where crop_id = (select crop_id from crop_trait where crop_trait_id = :ctid)", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();

                    string codeValue = dm.ReadValue(@"select code from crop_trait_code where crop_trait_code_id = :ctcid", new DataParameters(":ctcid", codeID, DbType.Int32)).ToString();
                    string codeDesc = dm.ReadValue(@"select description from crop_trait_code_lang where crop_trait_code_id = :ctcid and sys_lang_id = :langid", new DataParameters(":ctcid", codeID, DbType.Int32, ":langid", sd.LanguageID, DbType.Int32)).ToString();


                    if (traitName != "")
                    {
                        lblDesc.Text = cropName + " accessions with code " + codeValue  + " (" + codeDesc + ") for descriptor " + traitName;

                        dt = sd.GetData("web_descriptor_value_accession1", ":traitid=" + traitID + ";:codeid=" + codeID, 0, 0).Tables["web_descriptor_value_accession1"];
                        gvAccession.DataSource = dt;
                        gvAccession.DataBind();
                    }
                }
            }
        }

        private void bindData2(int traitID, string value)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    string traitName = dm.ReadValue(@"select coded_name from crop_trait where crop_trait_id = :ctid", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();
                    string cropName = dm.ReadValue(@"select name from crop where crop_id = (select crop_id from crop_trait where crop_trait_id = :ctid)", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();

                    if (traitName != "")
                    {
                        lblDesc.Text = cropName + " accessions with value = " + value + " for descriptor " + traitName;

                        dt = sd.GetData("web_descriptor_value_accession2", ":traitid=" + traitID + ";:value=" + value, 0, 0).Tables["web_descriptor_value_accession2"];
                        gvAccession.DataSource = dt;
                        gvAccession.DataBind();
                    }
                }
            }
        }

        private void bindData3(int traitID, string value)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    string traitName = dm.ReadValue(@"select coded_name from crop_trait where crop_trait_id = :ctid", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();
                    string cropName = dm.ReadValue(@"select name from crop where crop_id = (select crop_id from crop_trait where crop_trait_id = :ctid)", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();

                    if (traitName != "")
                    {
                        var low = value.Split(';')[0];
                        var up = value.Split(';')[1];

                        lblDesc.Text = cropName + " accessions with values between " + low + " and " + up + " for descriptor " + traitName;

                        dt = sd.GetData("web_descriptor_value_accession3", ":traitid=" + traitID + ";:low=" + low + ";:up=" + up, 0, 0).Tables["web_descriptor_value_accession3"];
                        gvAccession.DataSource = dt;
                        gvAccession.DataBind();
                    }
                }
            }
        }

        //protected void gvAccession_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        e.Row.Cells[1].Text = traitName;
        //    }
        //}

        protected void gvAccession_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].Text.ToUpper().Contains("&LT;A"))
                    {
                        e.Row.Cells[i].Text = Server.HtmlDecode(e.Row.Cells[i].Text);
                    }
                }
            }
        }

    }
}
