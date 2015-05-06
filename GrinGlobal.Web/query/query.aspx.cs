using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Diagnostics;
using GrinGlobal.Core;
using GrinGlobal.Business;

namespace GrinGlobal.Web.query
{
    public partial class query : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["sql"] != null)
                    {
                        string sql = Request.QueryString["sql"].ToString();
                        showData(sql, false); 
                    }
                    else if (Request.QueryString["esql"] != null)
                    {
                        string sql = Request.QueryString["esql"].ToString();
                        showData(sql, true);
                    }
                    else
                        bindDropDowns();
                }
            }
            catch { }
        }

        private void bindDropDowns()
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                DataTable dt = sd.GetData("web_lookup_query_dataviewlist", ":langid=" + sd.LanguageID + ";:dvName=" + "web_qry_%", 0, 0).Tables["web_lookup_query_dataviewlist"];
               
                //ddlDataView.DataSource = dt;
                //ddlDataView.DataBind();

                string name = "";
                string title = "";
                string config = "";
                bool isInternal = (Page.User.IsInRole("ALLUSERS")) || Toolkit.GetSetting("MakeReportsExternal", false);  // only show internal reports to internal users, show external report to all

                foreach (DataRow dr in dt.Rows)
                {
                    name = dr["dataview_name"].ToString();
                    title = dr["title"].ToString();
                    config = dr["configuration_options"].ToString().ToLower();

                    if (isInternal || (config == "external"))
                    ddlDataView.Items.Add(new ListItem(title, name));
                }

                ddlDataView.Items.Insert(0, new ListItem("-- Select One --", ""));
                ddlDataView.SelectedIndex = 0;

                showMiscControl(true);
            }
        }

        protected void ddlDataView_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvData.DataSource = null;
            gvData.DataBind();
            pnlData.Visible = false;
            lblError.Visible = false;

            if (String.IsNullOrEmpty(ddlDataView.SelectedValue.ToString()))
            {
                gvParamValues.DataSource = null;
                gvParamValues.DataBind();
                PnlDesc.Visible = false;
                pnlParam.Visible = false;
            }
            else
            {
                //using (var admin = new AdminData(false, UserManager.GetLoginToken()))
                //{
                //    DataSet ds = admin.GetDataViewDefinition(ddlDataView.SelectedValue.ToString());

                //    gvParamValues.DataSource = ds.Tables["sys_dataview_param"];
                PnlDesc.Visible = true;
                string dataview = ddlDataView.SelectedValue;
                using (var sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        string s = dm.ReadValue(@"
                            select coalesce(sdvl.description, sdvl.title) from sys_dataview sdv left join sys_dataview_lang sdvl
                            on sdv.sys_dataview_id = sdvl.sys_dataview_id
                            and sdvl.sys_lang_id = :langid
                            where sdv.dataview_name = :dataview
                            ", new DataParameters(":langid", sd.LanguageID, DbType.Int32
                            , ":dataview", dataview, DbType.String)).ToString();

                        //s = System.Text.RegularExpressions.Regex.Replace(s, "(\\n|\\\\n|\\r|\\\\r)+", "\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        //string s1 = s.Replace("\\r\\n", "\n");
                        //txtDesc.Text = s1;

                        string s2 = s.Replace("\\r\\n", "<br />");
                        lblDesc.Text = s2;
                    }
                    
                    DataSet dsParam = sd.GetDataParameterTemplate(dataview);
                    DataTable dtParam = dsParam.Tables["dv_param_info"];

                    gvParamValues.DataSource = dtParam;
                    gvParamValues.DataBind();


                    if (dtParam.Rows.Count != 0)
                    {
                        pnlParam.Visible = true;
                    }
                    else
                    {
                        pnlParam.Visible = false;
                        btnGetData_Click(null, null);
                    }
                }
            }
        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            bindData();
        }

        private void bindData()
        {
            string rpt = ddlDataView.SelectedValue.ToString();

            if (rpt == "web_qry_web_order_detail")
            {
                var woid = ((TextBox)gvParamValues.Rows[0].FindControl("txtParamValue")).Text;
                Response.Redirect("weborderdetail.aspx?id="+ woid);
            }
            else
            {

                pnlData.Visible = true;

                DataTable dt2 = getData();
                if (dt2 == null)
                {
                    return;
                }

                //DataTable dt = dt2.DataSet.Tables[ddlDataView.SelectedValue.ToString()];
                lblRowCount.Text = "Found " + dt2.Rows.Count.ToString() + " rows";

                gvData.DataKeyNames = dt2.PrimaryKeyNames();
                gvData.DataSource = dt2;
                gvData.DataBind();
            }
        }

        private DataTable getData()
        {
            if (ddlDataView.SelectedValue == string.Empty)
            {
                return null;
            }
            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                DataSet dsParam = sd.GetDataParameterTemplate(ddlDataView.SelectedValue);
                DataTable dtParam = dsParam.Tables["dv_param_info"];

                int limit = 0;
                int.TryParse(txtLimit.Text, out limit);

                int offset = 0;
                int.TryParse(txtOffset.Text, out offset);

                StringBuilder sb = new StringBuilder();
                foreach (GridViewRow gvr in gvParamValues.Rows)
                {
                    string name = ((HiddenField)gvr.FindControl("hidParamName")).Value;
                    string val = ((TextBox)gvr.FindControl("txtParamValue")).Text;
                    if (val.Length < 1)
                    {
                        val = "";
                    }

                    sb.Append(name.Replace("=", @"\=").Replace(";", @"\;"));
                    sb.Append("=");
                    sb.Append(val.Replace("=", @"\=").Replace(";", @"\;"));
                    sb.Append(";");
                }

                string rpt = ddlDataView.SelectedValue.ToString();
                DataSet ds2 = sd.GetData(rpt, sb.ToString(), offset, limit);

                if (rpt == "web_qry_fieldbook")
                    return createFieldbook(ds2.Tables[rpt]);
                else
                    return ds2.Tables[rpt];
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //exportToExcel(gvData, "Report");
            //Utils.ExportToExcel(HttpContext.Current, gvData, "Report", "Report data for: " + ddlDataView.SelectedItem.Text + ",");
            string rpt = ddlDataView.SelectedValue.ToString();
            if (rpt == "web_qry_fieldbook")
                Utils.ExportToExcel(HttpContext.Current, gvData, "Report", "");
            else
                Utils.ExportToExcel(HttpContext.Current, gvData, "Report", "Report data for: " + ddlDataView.SelectedItem.Text + ",");

        }

        //private void exportToExcel(GridView gv, string fName)
        //{
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", fName));
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.csv";

        //    StringBuilder sb = new StringBuilder();
        //    System.IO.StringWriter sWriter = new System.IO.StringWriter(sb);

        //    sWriter.WriteLine(fName + " data for: " + ddlDataView.SelectedItem.Text + ",");
        //    sWriter.WriteLine(",");

        //    for (int k = 0; k < gv.HeaderRow.Cells.Count; k++)
        //    {
        //        sWriter.Write(gv.HeaderRow.Cells[k].Text + ",");
        //    }
        //    sWriter.WriteLine(",");

        //    string sData;
        //    for (int i = 0; i < gv.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < gv.HeaderRow.Cells.Count; j++)
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

        private void exportToText(GridView gv, string fName)  // need to export to text file so preserve the leading zero in the data
        {

        }

        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // unescape the html so that it will render as raw html
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

        private DataTable createFieldbook(DataTable dt1o)
        {
            DataTable dt1 = null;
            DataTable dt1t = null;
            DataTable dt2 = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    if (dt1o.Rows.Count > 0)
                    {
                        List<int> idlist = new List<int>();
                        int id;

                        foreach (DataRow dr in dt1o.Rows)
                        {
                            if (int.TryParse(dr[0].ToString(), out id))
                                idlist.Add(id);
                        }

                        dt1 = sd.GetData("web_descriptorbrowse_trait_fieldbook1", ":where=a.accession_id in(" + Toolkit.Join(idlist.ToArray(), ",", "") + ")", 0, 0).Tables["web_descriptorbrowse_trait_fieldbook1"];
                        dt1t = dt1.Transform(new string[] { "accession_id" }, "trait_name", "trait_name", "value");

                        int oid = 0;
                        if (dt1o.Rows.Count > 0)
                            oid = Toolkit.ToInt32(dt1o.Rows[0]["order_request_id"].ToString(), 0);

                        dt2 = sd.GetData("web_descriptorbrowse_trait_fieldbook2o", ":idlist=" + Toolkit.Join(idlist.ToArray(), ",", "") + ";:oid=" + oid, 0, 0).Tables["web_descriptorbrowse_trait_fieldbook2o"];
                        int j = dt2.Columns.Count;

                        for (int i = 1; i < dt1t.Columns.Count; i++)
                        {
                            DataColumn traitCol = new DataColumn(dt1t.Columns[i].ColumnName, typeof(string));
                            dt2.Columns.Add(traitCol);
                        }

                        DataTable dtMultiple = null;
                        foreach (DataRow dr in dt2.Rows)
                        {
                            id = Toolkit.ToInt32(dr[3].ToString(), 0);

                            // first need to add some query, so to get  0) inventory (could be many?? which to put) 1) all accession name 2) all sourcehistory, replace the blank placeholder
                            dtMultiple = sd.GetData("web_fieldbook_accessionnames", ":accessionid=" + id, 0, 0).Tables["web_fieldbook_accessionnames"];
                            dt2.Columns["IDS"].ReadOnly = false;
                            dr["IDS"] = Toolkit.Join(dtMultiple, "plantname", ";", "");

                            dtMultiple = sd.GetData("web_fieldbook_srchistory", ":accessionid=" + id, 0, 0).Tables["web_fieldbook_srchistory"];

                            var sb = new StringBuilder();
                            if (dtMultiple == null || dtMultiple.Rows.Count == 0) { }
                            else
                            {
                                dt2.Columns["HISTORY"].ReadOnly = false;
                                string acsid = "";
                                foreach (DataRow drs in dtMultiple.Rows)
                                {
                                    if (drs["accession_source_id"].ToString() != acsid)
                                    {
                                        if (sb.Length > 1) sb.Append("; ");
                                        sb.Append(drs["typecode"].ToString());

                                        if (!String.IsNullOrEmpty(drs["histdate"].ToString()))
                                            sb.Append(" ").Append(drs["histdate"].ToString());

                                        if (!String.IsNullOrEmpty(drs["state"].ToString()))
                                            sb.Append(" ").Append(drs["state"].ToString()).Append(",");


                                        if (!String.IsNullOrEmpty(drs["country"].ToString()))
                                            sb.Append(" ").Append(drs["country"].ToString());

                                    }
                                    if (!String.IsNullOrEmpty(drs["fname"].ToString()))
                                        sb.Append(" by ").Append(drs["fname"].ToString()).Append(",");

                                    if (!String.IsNullOrEmpty(drs["lname"].ToString()))
                                        sb.Append(" ").Append(drs["lname"].ToString()).Append(",");

                                    if (!String.IsNullOrEmpty(drs["organization"].ToString()))
                                        sb.Append(" ").Append(drs["organization"].ToString());

                                    acsid = drs["accession_source_id"].ToString();
                                }
                                if (sb.Length > 1) dr["HISTORY"] = sb.ToString();
                            }

                            // get trait value if there is any

                            DataRow[] foundRows = dt1t.Select("accession_id = " + id);
                            if (foundRows.Length > 0)
                            {
                                DataRow foundRow = foundRows[0];
                                for (int i = 1; i < dt1t.Columns.Count; i++)
                                {
                                    dr[j + i - 1] = foundRow[i];
                                }
                            }

                        }
                        return dt2;
                    }
                    else
                        return dt1o;
                }
            }
        }

        private void showData(string sql, bool appliedEncryption)
        {
            try
            {
                if (appliedEncryption)
                {
                    string password = Toolkit.GetSetting("StringPassword", "");
                    if (password == "")
                        sql = Crypto.DecryptText(sql.Replace(" ", "+"));
                    else
                        sql = Crypto.DecryptText(sql.Replace(" ", "+"), password);
                }
                sql = Utils.Sanitize(sql);

                if (Page.User.IsInRole("ALLUSERS") || appliedEncryption)
                {
                    if (sql.Contains("update ") || sql.Contains("delete ") || sql.Contains("insert") || sql.Contains("select into") || sql.Contains("create ") || sql.Contains("exec ") || sql.Contains("execute "))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Only Select query is allowed.";

                        bindDropDowns();
                    }
                    else
                    {
                        using (var sd = new SecureData(true, UserManager.GetLoginToken()))
                        {

                            int limit = Toolkit.ToInt32(Request.QueryString["lim"], 0);

                            if (limit == 0) int.TryParse(txtLimit.Text.Replace(",", ""), out limit);

                            using (DataManager dm = sd.BeginProcessing(true, true))
                            {
                                try
                                {
                                    dm.Limit = limit;
                                    DataTable dt = dm.Read(sql);

                                    lblError.Visible = false;
                                    lblRowCount.Text = dt.Rows.Count.ToString() + " Records";

                                    pnlData.Visible = true;
                                    gvData.DataKeyNames = dt.PrimaryKeyNames();
                                    gvData.DataSource = dt;
                                    gvData.DataBind();
                                    showMiscControl(false);

                                }
                                catch (Exception ex)
                                {
                                    lblError.Visible = true;
                                    lblError.Text = ex.Message;

                                    gvData.DataSource = null;
                                    bindDropDowns();
                                }
                            }
                        }
                    }
                }
                else
                    bindDropDowns();
            }
            catch (Exception ex) { bindDropDowns(); }
        }

        private void showMiscControl(bool regReport)
        {
            if (regReport)
            {
                //lblChoose.Visible = true;
                //ddlDataView.Visible = true;
                pnlChoose.Visible = true;
                //lblRowCount.Visible = true;
                //btnExport.Visible = true;
            }
            else
            {
                //lblChoose.Visible = false;
                //ddlDataView.Visible = false;
                pnlChoose.Visible = false;
                //lblRowCount.Visible = false;
                //btnExport.Visible = false;
            }
        }
    }
}
