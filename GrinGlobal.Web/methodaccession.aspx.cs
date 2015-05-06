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
    public partial class methodaccession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                int traitID = Toolkit.ToInt32(Request.QueryString["id1"], 0);
                int methodID = Toolkit.ToInt32(Request.QueryString["id2"], 0);
                
                if (traitID > 0 && methodID > 0)
                    bindData(traitID, methodID);
            }
        }

        static string traitName = "";
        static string methodName = "";
        private void bindData(int traitID, int methodID)
        {
            DataTable dt = null;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    traitName = dm.ReadValue(@"select coded_name from crop_trait where crop_trait_id = :ctid", new DataParameters(":ctid", traitID, DbType.Int32)).ToString();
                    methodName = dm.ReadValue(@"select name from method where method_id = :mid", new DataParameters(":mid", methodID, DbType.Int32)).ToString();

                    if (methodName != "" & traitName != "")
                    {
                        lblDesc.Text = traitName + " in study " + methodName;

                        dt = sd.GetData("web_method_descriptor_accession", ":traitid=" + traitID + ";:methodid=" + methodID, 0, 0).Tables["web_method_descriptor_accession"];
                        gvAccession.DataSource = dt;
                        gvAccession.DataBind();

                        dt = sd.GetData("web_descriptor_detail_download", ":traitid=" + traitID + ";:methodid=" + methodID, 0, 0).Tables["web_descriptor_detail_download"];
                        gvAccession2.DataSource = dt;
                        gvAccession2.DataBind();
                    }
                }
             }
        }

         protected void gvAccession_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = traitName;
            } 
        }

         protected void gvAccession_RowDataBound(object sender, GridViewRowEventArgs e)
         {
             if (e.Row.DataItemIndex > -1)
             {
                 var min = Toolkit.ToFloat(((DataRowView)e.Row.DataItem)["mini"], -1).ToString();
                 var max = Toolkit.ToFloat(((DataRowView)e.Row.DataItem)["maxi"], -1).ToString();
                 var mean = Toolkit.ToFloat(((DataRowView)e.Row.DataItem)["mean"], -1).ToString();
                 var sdev = Toolkit.ToFloat(((DataRowView)e.Row.DataItem)["sdev"], -1).ToString();
                 var size = Toolkit.ToFloat(((DataRowView)e.Row.DataItem)["size"], -1).ToString();
                 var note = ((DataRowView)e.Row.DataItem)["note"].ToString().Trim();

                 string s =
                     (min == "-1" ? "" : "min=" + min + "; ") +
                     (max == "-1" ? "" : "max=" + max + "; ") +
                     (mean == "-1" ? "" : "mean=" + mean + "; ") +
                     (sdev == "-1" ? "" : "sdev=" + sdev + "; ") +
                     (size == "-1" ? "" : "samplesize=" + size);

                 if (!string.IsNullOrEmpty(note))
                 {
                     if (!string.IsNullOrEmpty(s.Trim()))
                         s += "; ";

                     if (note.IndexOf("<a href") > -1)
                     {
                        note = note.Replace(@"""/npgs/images/", @"""http://www.ars-grin.gov//npgs/images/");
                     }
                     s += note;
                 }
 
                 e.Row.Cells[3].Text = s;
             }
         }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Utils.ExportToExcel(HttpContext.Current, gvAccession2, "grin", "Accession evaluated for " + traitName + " in study " + methodName);
        }
    }
}
