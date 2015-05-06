using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Web.UI.HtmlControls;

namespace GrinGlobal.Web
{
    public partial class cropmarker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
            }
        }

        private void bindData(int markerID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                dt = sd.GetData("web_crop_marker_data", ":markerid=" + markerID, 0, 0).Tables["web_crop_marker_data"];

                if (dt.Rows.Count > 0)
                {
                    gvMarker.DataSource = dt;
                    gvMarker.DataBind();
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        string mName = dm.ReadValue(@"select name from genetic_marker where genetic_marker_id = :markerid", new DataParameters(":markerid", markerID, DbType.Int32)).ToString();
                        gvMarker.HeaderRow.Cells[gvMarker.HeaderRow.Cells.Count - 1].Text = mName;
                    }


                    hlView.NavigateUrl = "cropmarkerview.aspx?markerid=" + markerID;
                    hlView.Visible = true;

                    lbDownload.Visible = true;
                    lblS.Visible = true;
                    lblView.Visible = true;

                    lblMarker.Text = markerID.ToString();
                }


                dt = sd.GetData("web_crop_marker_detail1", ":markerid=" + markerID, 0, 0).Tables["web_crop_marker_detail1"];
                if (dt.Rows.Count > 0)
                {
                    List<string>  header = new List<string> ();
                    foreach(DataColumn column in dt.Columns)
                    {
                         header.Add(column.ColumnName.Replace("_", " "));
                    }
  
                    DataRow dr = dt.Rows[0]; 
                    lblMarker1.Text = "Marker Details for " + dr["name"] + ":";
                    HtmlTable tblMarker1 = this.tblMarker1;
                    for (int i = 1; i < dt.Columns.Count; i++)
                    {

                        if (!String.IsNullOrEmpty(dr.ItemArray[i].ToString()))
                        {
                            HtmlTableCell tc1 = new HtmlTableCell("TH");
                            tc1.InnerHtml = header[i];

                            HtmlTableCell tc2 = new HtmlTableCell();
                            tc2.InnerHtml = dr.ItemArray[i].ToString();

                            HtmlTableRow tr = new HtmlTableRow();
                            tr.Cells.Add(tc1);
                            tr.Cells.Add(tc2);
                            tblMarker1.Rows.Add(tr);
                        }
                    }
                }

                dt = sd.GetData("web_crop_marker_detail2", ":markerid=" + markerID, 0, 0).Tables["web_crop_marker_detail2"];
                if (dt.Rows.Count > 0)
                {
                    List<string> header = new List<string>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        header.Add(column.ColumnName.Replace("_", " "));
                    }

                    DataRow dr = dt.Rows[0];
                    lblMarker2.Text = "Marker Citation Detail:";
                    HtmlTable tblMarker2 = this.tblMarker2;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        if (!String.IsNullOrEmpty(dr.ItemArray[i].ToString()))
                        {
                            HtmlTableCell tc1 = new HtmlTableCell("TH");
                            tc1.InnerHtml = header[i];

                            HtmlTableCell tc2 = new HtmlTableCell();
                            tc2.InnerHtml = dr.ItemArray[i].ToString();

                            HtmlTableRow tr = new HtmlTableRow();
                            tr.Cells.Add(tc1);
                            tr.Cells.Add(tc2);
                            tblMarker2.Rows.Add(tr);
                        }
                    }
                }

                dt = sd.GetData("web_crop_marker_detail3", ":markerid=" + markerID, 0, 0).Tables["web_crop_marker_detail3"];
                rptAssay.DataSource = dt;
                rptAssay.DataBind();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Utils.ExportToExcel(HttpContext.Current, gvMarker, "marker" + lblMarker.Text, "Marker data for marker ID = " + lblMarker.Text);
        }
    }
}
