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
    public partial class descriptordetail : System.Web.UI.Page
    {
        static int traitID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                traitID = Toolkit.ToInt32(Request.QueryString["id"], 0);
                if (traitID > 0)
                    bindData(traitID);
            }
        }

        private void bindData(int descriptorID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_descriptor_detail", ":langid=" + sd.LanguageID + ";:descriptorid=" + descriptorID, 0, 0).Tables["web_descriptor_detail"];
                dvDescriptor.DataSource = dt;
                dvDescriptor.DataBind();

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    if (dr["data_format"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_nformat").Visible = false;
                    if (dr["numeric_minimum"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_nminimum").Visible = false;
                    if (dr["numeric_maximum"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_nmaximum").Visible = false;
                    if (dr["original_value_type_code"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_original_type").Visible = false;
                    if (dr["original_value_format"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_original_format").Visible = false;
                    if (dr["ontology_url"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_ontology").Visible = false;
                    if (dr["note"] == DBNull.Value)
                        dvDescriptor.FindControl("tr_note").Visible = false;

                    dt = sd.GetData("web_descriptor_detail_method", ":descriptorid=" + descriptorID, 0, 0).Tables["web_descriptor_detail_method"];
                    if (dt.Rows.Count > 0)
                    {
                        plMethod.Visible = true;
                        rptMethod.DataSource = dt;
                        rptMethod.DataBind();
                    }

                    string descriptor = dr["descriptor_sname"].ToString();
                    hf1.Value = dr["descriptor_sname"].ToString();


                    string coded = dr["is_coded"].ToString();
                    string dataCode = dr["data_type_code"].ToString();
                    string attachCnt = "0";

                    if (coded == "Y")
                    {
                        lblName.Text = "Distribution of Values for " + descriptor;
                        dt = sd.GetData("web_descriptor_detail_values1", ":descriptorid=" + descriptorID, 0, 0).Tables["web_descriptor_detail_values1"];

                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            attachCnt = dm.ReadValue(@"select COUNT(ctca.crop_trait_code_attach_id) from crop_trait_code_attach ctca join crop_trait_code ctc on ctca.crop_trait_code_id = ctc.crop_trait_code_id where ctc.crop_trait_id = :ctid", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();
                        }
                    }
                    else if (dataCode == "CHAR" || dataCode == "UPPER")
                    {
                        lblName.Text = "Distinct Values for " + descriptor;
                        dt = sd.GetData("web_descriptor_detail_values2", ":descriptorid=" + descriptorID, 0, 0).Tables["web_descriptor_detail_values2"];
                    }
                    else
                    {
                        lblName.Text = "Distribution of Values for " + descriptor;
                       
                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            decimal min = Toolkit.ToDecimal(dm.ReadValue(@"select MIN(numeric_value) from crop_trait_observation where crop_trait_id = :ctid", new DataParameters(":ctid", descriptorID, DbType.Int32)).ToString(), 0);
                            decimal max = Toolkit.ToDecimal(dm.ReadValue(@"select MAX(numeric_value) from crop_trait_observation where crop_trait_id = :ctid", new DataParameters(":ctid", descriptorID, DbType.Int32)).ToString(), 0);

                            decimal step = (max - min) / 10;

                            if (step == 0 && min == 0)
                                dt = null;
                            else
                            {
                                dt = new DataTable();
                                DataColumn range = new DataColumn("Range", typeof(string));
                                //range.Caption = "Range";
                                dt.Columns.Add(range);
                                DataColumn number = new DataColumn("Number of Accessions", typeof(string));
                                //number.Caption = "Number of Accessions";
                                dt.Columns.Add(number);
                                string cell1 = "", cell2 = "";

                                for (int i = 0; i < 10; i++)
                                {
                                    decimal low = min + i * step;
                                    decimal up = min + i * step + step;
                                    DataTable dtCnt = sd.GetData("web_descriptor_detail_values3", ":descriptorid=" + descriptorID + ";:value1=" + low + ";:value2=" + up, 0, 0).Tables["web_descriptor_detail_values3"];
                                    int cnt = Toolkit.ToInt32(dtCnt.Rows[0][0].ToString(), 0);

//                                    int cnt = Toolkit.ToInt32(dm.ReadValue(@"select COUNT(distinct a.accession_id) from crop_trait_observation cto join inventory i on cto.inventory_id =  i.inventory_id 
//                                            join accession a on a.accession_id = i.accession_id
//                                            where cto.crop_trait_id = :ctid and cto.numeric_value >= :value1 and cto.numeric_value <= :value2",
//                                                new DataParameters(":ctid", descriptorID, DbType.Int32, ":value1", low, DbType.Decimal, ":value2", up, DbType.Decimal)).ToString(), 0);

                                    cell1 = low.ToString() + " - " + up.ToString();
                                    if (cnt == 0)
                                        cell2 = "0";
                                    else
                                        cell2 = "<a href='descriptoraccession.aspx?id1=" + descriptorID + "&id2=" + low + ";" + up + "&type=3'>" + cnt + "</a>";

                                    DataRow row = null;
                                    row = dt.NewRow();
                                    row[0] = cell1;
                                    row[1] = cell2;
                                    dt.Rows.Add(row);
                                }
                            }
                        }
                    }

                    gvCodeValue.DataSource = dt;
                    gvCodeValue.DataBind();

                    if (coded == "Y")
                    {
                        for (int i = 0; i < gvCodeValue.Rows.Count; i++)
                        {
                            gvCodeValue.Rows[i].Cells[3].Visible = false;

                            if (attachCnt == "0")
                                gvCodeValue.Rows[i].Cells[4].Visible = false;
                        }
                        if (attachCnt == "0")
                            gvCodeValue.HeaderRow.Cells[4].Visible = false;
                     }     

                    bindAttachment(descriptorID);
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DataTable dt = null;

            if (traitID > 0)
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        string traitName = dm.ReadValue(@"select coded_name from crop_trait where crop_trait_id = :ctid", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();

                        dt = sd.GetData("web_descriptor_detail_download", ":traitid=" + traitID + ";:methodid=0", 0, 0).Tables["web_descriptor_detail_download"];
                        gv1.DataSource = dt;
                        gv1.DataBind();

                        Utils.ExportToExcel(HttpContext.Current, gv1, "grin", "Accession evaluated for " + traitName);
                    }
                }
            }
        }

        protected void gvCodeValue_RowDataBound(object sender, GridViewRowEventArgs e)
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

                if (e.Row.Cells.Count > 3)
                {
                    e.Row.Cells[3].Visible = false;
                    
                    string category = e.Row.Cells[3].Text;
                    string path = e.Row.Cells[4].Text;
                    bool hasData = false;
                    string cellTx = "";
                    if (category.ToUpper() == "LINK")
                    {
                        if (path != "")
                        {
                            cellTx = "<a href='" + path + "' target='_blank'> Code Image </a>";
                            hasData = true;
                        }
                    }
                    else if (category.ToUpper() == "IMAGE")
                    {
                        if (path != "")
                        {
                            cellTx = "<a href='" + path + "' target='_blank'><img src='" + path + "' alt='' height='60'></a>";
                            hasData = true;
                        }
                    }

                    if (hasData)
                        e.Row.Cells[4].Text = cellTx; 
                }
            }
        }

        protected void gvCodeValue_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
               for (int i = 0; i < e.Row.Cells.Count; i++)
               {
                    string header = e.Row.Cells[i].Text;

                    if (header.ToLower().Contains("virtual"))
                        header = "";

                    if (header.ToLower() == "category_code")
                        e.Row.Cells[i].Visible = false;

                    header = header.Replace("_", " ");
                    e.Row.Cells[i].Text = header;
               } 
            }
        }

        private void bindAttachment(int id)
        {
            DataTable dt = null;

            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                dt = sd.GetData("web_descriptor_detail_attach", ":traitid=" + id + ";:categorycode='IMAGE'", 0, 0).Tables["web_descriptor_detail_attach"];

                if (dt.Rows.Count > 0)
                {
                    imagePreviewer.DataSource = dt;
                    imagePreviewer.DataBind();
                }
                else
                {
                    imagePreviewer.Visible = false;
                }

                dt = sd.GetData("web_descriptor_detail_attach", ":traitid=" + id + ";:categorycode='LINK'", 0, 0).Tables["web_descriptor_detail_attach"];
                if (dt.Rows.Count > 0)
                {
                    plLink.Visible = true;
                    rptLink.DataSource = dt;
                    rptLink.DataBind();
                }
            }
        }
    }
}
