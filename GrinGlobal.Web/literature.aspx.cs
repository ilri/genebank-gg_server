using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web {
    public partial class Literature : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) 
            {
                if (Request.QueryString["cid"] != null)
                    bindCitation(Toolkit.ToInt32(Request.QueryString["cid"], 0));
                else
                    bindLiterature(Toolkit.ToInt32(Request.QueryString["id"], 0), Request.QueryString["abbr"]);
            }

            //if (!Page.IsPostBack) {
            //    btnPrevious.NavigateUrl = Request.Params["HTTP_REFERER"];
            //}
        }

        private void bindLiterature(int literatureID, string abbr)
        {
            pnlLiterature.Visible = true;
            pnlCitation.Visible = false;

            using (SecureData sd = UserManager.GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    if (literatureID <= 0) 
                    {
                        // if they didn't give us a valid lit id, try looking it up based on abbr
                        literatureID = Toolkit.ToInt32(dm.ReadValue(@"select literature_id from literature where abbreviation = :abbr", new DataParameters(":abbr", abbr)), -1);
                    }

                    rptLiterature.DataSource = sd.GetData("web_literature", ":literature_id=" + literatureID, 0, 0).Tables["web_literature"];
                    rptLiterature.DataBind();
                }
            }
        }

        private void bindCitation(int id) 
        {
            pnlLiterature.Visible = false;
            
            using (SecureData sd = UserManager.GetSecureData(true))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    DataTable dt = dm.Read(@"
                    select c.*, l.abbreviation  from citation c left join literature l on c.literature_id = l.literature_id  where citation_id = :cid",
                    new DataParameters(":cid", id));

                    if (dt.Rows.Count > 0)
                    {
                        pnlCitation.Visible = true;

                        DataRow dr = dt.Rows[0];
                        lblCitation.Text = dr["author_name"] + "<br />" + dr["citation_title"] + "<br />" +  dr["abbreviation"] + " " + dr["reference"] + " " + dr["citation_year"] + "<br />";
                    }

                    rptCitation.DataSource = sd.GetData("web_crop_citation_accession", ":citationid=" + id, 0, 0).Tables["web_crop_citation_accession"];
                    rptCitation.DataBind();
                }
            }

        }
    }
}
