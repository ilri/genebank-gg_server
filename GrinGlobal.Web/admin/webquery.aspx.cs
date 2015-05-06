using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Text;

namespace GrinGlobal.Web.Admin {
	public partial class Sql : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void btnRun_Click(object sender, EventArgs e) 
        {
			bindGrid();
		}

		private void bindGrid() {
            using (var sd = new SecureData(true, UserManager.GetLoginToken())) {

                int limit = 0;
                int.TryParse(txtLimit.Text.Replace(",", ""), out limit);

                using (DataManager dm = sd.BeginProcessing(true, true)) 
                {
                    string sqlInput = txtSql.Text.Trim();
                    string sql = sqlInput.ToLower();
                    // Laura: need more cleanning work here, considering special characters, java scripts.... all these regular possible cases
                    if (sql.Contains("update ") || sql.Contains("delete ") || sql.Contains("insert") || sql.Contains("select into") || sql.Contains("create ") || sql.Contains("exec ") || sql.Contains("execute "))
                    {
                        lblError.Visible = true;
                        lblError.Text = "Only Select query is allowed.";

                        lblRetrieved.Visible = true;
                        GridView1.DataSource = null;
                    }
                    else
                    {
                        try
                        {
                            dm.Limit = limit;
                            DataTable dt = dm.Read(sqlInput);

                            lblError.Visible = false;
                            GridView1.DataSource = dt;

                            int cnt = dt.Rows.Count;
                            lblRetrieved.Visible = true;
                            lblRetrieved.Text = "Retrieved " + cnt + " rows.";
                            
                        }
                        catch (Exception ex)
                        {
                            lblError.Visible = true;
                            lblError.Text = ex.Message;

                            lblRetrieved.Visible = true;
                            GridView1.DataSource = null;
                        }
                    }
                    GridView1.PageSize = Toolkit.ToInt32(ddlPageSize.SelectedValue, 0);
                    GridView1.DataBind();
                }
            }
		}

		protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(txtSql.Text.Trim()))
                bindGrid();
		}

		protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
			bindGrid();
			GridView1.PageIndex = e.NewPageIndex;
			GridView1.DataBind();
		}

		protected void GridView1_Sorting(object sender, GridViewSortEventArgs e) {
			bindGrid();

			DataTable dt = (DataTable)GridView1.DataSource;

			if (dt != null) {
				DataView dv = new DataView(dt);
				dv.Sort = e.SortExpression + " " + (e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC");
				GridView1.DataSource = dv;
				GridView1.DataBind();
			}
		}

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count != 0)
                Utils.ExportToExcel(HttpContext.Current, GridView1, "query_result", "Query Result For Current Page");
        }

        protected void btnDownloadAll_Click(object sender, EventArgs e)
        {
            HttpContext ctx = HttpContext.Current;
            ctx.Response.Clear();
            ctx.Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.csv", "query_result"));
            ctx.Response.Charset = "";
            ctx.Response.ContentType = "application/vnd.csv";

            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sWriter = new System.IO.StringWriter(sb);

            //sWriter.WriteLine("Query Result For All Rows");
            //sWriter.WriteLine(",");

            bindGrid();
            DataTable dt = (DataTable)GridView1.DataSource;

            for (int k = 0; k < dt.Columns.Count; k++)
            {
                sWriter.Write(dt.Columns[k].Caption + ",");
            }
            sWriter.WriteLine(",");  

            string sData;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //sData = (dt.Rows[i].ItemArray[j].ToString());
                    sData = (dt.Rows[i][j].ToString());
                    if (sData == "&nbsp;") sData = "";
                    sData = "\"" + sData + "\"" + ",";
                    sWriter.Write(sData);
                }
                sWriter.WriteLine();
            }

            sWriter.Close();
            ctx.Response.Write(sb.ToString());
            ctx.Response.End();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}.txt", "webQuery"));
            Response.Charset = "";
            Response.ContentType = "text/plain";

            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sWriter = new System.IO.StringWriter(sb);

            string sData = txtSql.Text.Trim();
            sWriter.Write(sData);

            sWriter.Close();
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            string fn = System.IO.Path.GetFileName(upload1.FileName);

            try
            {
                if (fn.Trim() != "")
                {
                    int iExt = fn.LastIndexOf(".");
                    if (iExt > 0)
                    {
                        string sExt = fn.Substring(iExt + 1, fn.Length - iExt - 1).ToUpper();
                        if (sExt == "TXT" || sExt == "SQL")
                        {
                            lblError.Visible = false;

                            StringBuilder sb = new StringBuilder();
                            System.IO.StreamReader sr = new System.IO.StreamReader(upload1.PostedFile.InputStream);
                            txtSql.Text = sr.ReadToEnd();
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Only files with .sql or .txt extention are processed.";
                            return;
                        }

                    }
                }
            }
            catch (Exception err)
            {
                lblError.Visible = true;
                lblError.Text = "File cannot be processed. Please check your file and try again.";
            }

        }
	}
}
