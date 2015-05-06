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
    public partial class TaxonomyList : System.Web.UI.Page
    {
       //Page for further expansion, list of names, subitems under lists of species, genera...
        string _category;
        string _type;
        string _value;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               if (this.Request.QueryString["category"] != null)
                    _category = this.Request.QueryString["category"].ToString();

                if (this.Request.QueryString["type"] != null)
                    _type = this.Request.QueryString["type"].ToString();

                if (this.Request.QueryString["value"] != null)
                    _value = this.Request.QueryString["value"].ToString();

                lblTitle.Text = ((_category == "species") ? "Species" : "Genera") + " of " + _value; //temp for only two types now.

                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
            }
        }

        private void bindData(int id)
        {
            DataTable dt = null;
            string columntype = "";

            if ( _type != "")
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    if (_category == "species")
                    {
                        switch (_type)
                        {
                            case "genus":
                                columntype = "genus_name";
                                break;
                            case "subgenus":
                                columntype = "subgenus_name";
                                break;
                            case "section":
                                columntype = "section_name";
                                break;
                            case "subsection":
                                columntype = "subsection_name";
                                break;
                            case "series":
                                columntype = "series_name";
                                break;
                            case "subseries":
                                columntype = "subseries_name";
                               break;
                        }

                        dt = sd.GetData(
                            "web_taxonomygenus_view_specieslist",
                            ":columntype=" + columntype +
                            ";:taxonomygenusid=" + id
                            , 0, 0).Tables["web_taxonomygenus_view_specieslist"];
                    }
                    else if(_category == "genera")
                    {
                        switch (_type)
                        {
                            case "family":
                                columntype = "family_name";
                                break;
                            case "subfamily":
                                columntype = "subfamily_name";
                                break;
                            case "tribe":
                                columntype = "tribe_name";
                                break;
                            case "subtribe":
                                columntype = "subtribe_name";
                                break;
                        }

                        dt = sd.GetData(
                            "web_taxonomyfamily_view_generalist",
                            ":columntype=" + columntype +
                            ";:taxonomyfamilyid=" + id
                            , 0, 0).Tables["web_taxonomyfamily_view_generalist"];
                    }
         
                }
            }

            if (dt.Rows.Count > 0)
            {
                rptRecordlist.DataSource = dt;
                rptRecordlist.DataBind();
            }
        }
    }
}
