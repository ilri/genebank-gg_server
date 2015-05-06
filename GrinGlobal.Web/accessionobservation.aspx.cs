using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;

namespace GrinGlobal.Web
{
    public partial class AccessionObservation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = Toolkit.ToInt32(Request.QueryString["id"], 0);

                bindData(id);
            }
        }

        private void bindData(int id)
        {
            DataTable dth = null, dt = null, dtg = null;
            string category = "", newCategory = "";

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) 
            {
                dth = sd.GetData("web_accession_maps_header", ":accessionid=" + id, 0, 0).Tables["web_accession_maps_header"];
                dt = sd.GetData("web_accessiondetail_observation_phenotype", ":accessionid=" + id, 0, 0).Tables["web_accessiondetail_observation_phenotype"];
                dtg = sd.GetData("web_accessiondetail_observation_genotype", ":accessionid=" + id, 0, 0).Tables["web_accessiondetail_observation_genotype"];
            }
            lblPINumber.Text = dth.Rows[0].ItemArray[0].ToString();

            HtmlTable tblCropTrait = this.tblCropTrait;
            foreach (DataRow dr in dt.Rows)
            {
                newCategory = dr["category_code"].ToString();
                if (newCategory != category)
                 {
                    HtmlTableCell tc = new HtmlTableCell("TH");
                    tc.InnerHtml = newCategory + " Descriptors";
                    tc.Attributes.Add("colspan", "4");

                    HtmlTableRow tr = new HtmlTableRow();
                    tr.Cells.Add(tc);
                    tblCropTrait.Rows.Add(tr);
                    category = newCategory;
                }
                HtmlTableCell tc1 = new HtmlTableCell();
                tc1.InnerHtml = "<a href='descriptordetail.aspx?id=" + dr["crop_trait_id"].ToString() + "' title='" + dr["trait_desc"].ToString() + "'>" + dr["trait_name"].ToString() + "</a>";

                HtmlTableCell tc2 = new HtmlTableCell();
                tc2.InnerHtml = dr["trait_value"].ToString();

                //HtmlTableCell tc3 = new HtmlTableCell();
                //tc3.InnerHtml = dr["qualifier_name"].ToString();

                HtmlTableCell tc4 = new HtmlTableCell();
                tc4.InnerHtml = "<a href='method.aspx?id=" + dr["method_id"].ToString() + "' title='" + dr["materials_and_methods"].ToString() + "'>" + dr["method_name"].ToString() + "</a>"; ;

                //Don't show default inventory ID
                HtmlTableCell tc5 = new HtmlTableCell();
                if (dr["form_type_code"].ToString() == "**")
                    tc5.InnerHtml = "";
                else
                    tc5.InnerHtml = dr["inventory_id"].ToString();

                HtmlTableRow tr_data = new HtmlTableRow();
                tr_data.Cells.Add(tc1);
                tr_data.Cells.Add(tc2);
                //tr_data.Cells.Add(tc3);
                tr_data.Cells.Add(tc4);
                tr_data.Cells.Add(tc5);
                tblCropTrait.Rows.Add(tr_data);
            }
            gv.DataSource = dt;
            gv.DataBind();

            if (dtg.Rows.Count > 0)
            {
                divGeno.Visible = true;
                gvGeno.DataSource = dtg;
                gvGeno.DataBind();
            }
        }

        protected void btnExportPheno_Click(object sender, EventArgs e)
        {
            /*
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Phenotype.xls");
            Response.Charset = "";

            // If you want the option to open the Excel file without saving then
            // comment out the line below
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            gv.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
            */

            Utils.ExportToExcel(HttpContext.Current, gv, "phenotype", "phenotype data for " + lblPINumber.Text + ",");
        }

        protected void btnExportGeno_Click(object sender, EventArgs e)
        {
            //exportToExcel(gvGeno, "genotype");
            Utils.ExportToExcel(HttpContext.Current, gvGeno, "genotype", "genotype data for " + lblPINumber.Text + ",");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        //private void exportToExcel(GridView gv, string fName)
        //{
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fName));
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.csv";

        //    StringBuilder sb = new StringBuilder();
        //    System.IO.StringWriter sWriter = new System.IO.StringWriter(sb);

        //    sWriter.WriteLine(fName + " data for " + lblPINumber.Text + ",");
        //    sWriter.WriteLine(",");

        //    for (int k = 0; k < gv.Columns.Count; k++)
        //    {
        //        sWriter.Write(gv.HeaderRow.Cells[k].Text + ",");
        //    }
        //    sWriter.WriteLine(",");

        //    string sData;
        //    for (int i = 0; i < gv.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < gv.Columns.Count; j++)
        //        {
        //            sData = (gv.Rows[i].Cells[j].Text.ToString());
        //            if (sData == "&nbsp;") sData = "";
        //            sData = "\"" + sData + "\"" + ",";
        //            sWriter.Write(sData);
        //        }
        //        sWriter.WriteLine();
        //    }

        //    sWriter.Close();
        //    Response.Write(sb.ToString());
        //    Response.End();
        //}
    }
}
