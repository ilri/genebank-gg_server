using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using System.Data;
using System.Text;
using GrinGlobal.Core;

namespace GrinGlobal.Web.taxon
{
    public partial class famgensearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlSearch.Visible = true;
                pnlResult.Visible = false;

                bindLists();
            }
        }

        private void bindLists()
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                var dt = sd.GetData("web_lookup_taxon_family", "", 0, 0).Tables["web_lookup_taxon_family"];

                ddlFamily.DataSource = dt;
                ddlFamily.DataBind();

                ddlFamily.Items.Insert(0, new ListItem(" ", "0"));
            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var sbClass = new StringBuilder();
            var sbClassDisplay = new StringBuilder();

            string searchString = "";

            if (!cbpathog.Checked)
                sbClass.Append(" and (f.note not like 'bacter%' and f.note not like 'vir%' and f.note not like 'fungal%' and f.note not like 'pathogen%' and f.note not like 'graft%')");
            else
                sbClassDisplay.Append(" or pathogen");

            if (!cbferns.Checked)
                 sbClass.Append(" and f.note not like 'fern%'");
            else
                sbClassDisplay.Append(" or pteridophyte");

            if (!cbgymno.Checked)
            {
                sbClass.Append(" and f.note not like 'gymnosperm%'");
            }
            else
                sbClassDisplay.Append("  or gymnosperm");

            if (!cbangio.Checked)
            {
                sbClass.Append(" and (f.note not like 'monocot%' and f.note not like 'dicot%')");
            }
            else
                sbClassDisplay.Append(" or angiosperm");

           
            try
            {
                searchString = "for the query: ";  

                if (!String.IsNullOrEmpty(txtGenus.Text))
                {
                    string genus = txtGenus.Text.Trim();
                    genus = Utils.Sanitize(genus);

                    using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            string sql = "";
                            DataTable dt = null;

                            sql = @"select distinct g.taxonomy_genus_id, g.genus_name, g.genus_authority,
                                    g.subgenus_name, g.section_name, g.subsection_name, g.series_name, g.subseries_name
                                    from taxonomy_genus g join taxonomy_family f
                                    on g.taxonomy_family_id = f.taxonomy_family_id
                                    and replace(g.genus_name, '-', '') like :s ";
                                 // and replace(g.genus_name, '-', '') like '" + genus.Replace("*", "%") + "%'";

                            if (genus.IndexOf("*") < 0) genus += "*";                            
                            
                            if (cbAccept.Checked)
                            {
                                sbClass.Append(" and (g.qualifying_code not like '%=%' or g.qualifying_code is null)");
                                searchString += "<b>accepted</b>";
                            }

                            if (cbinfragen.Checked)
                            {
                                sbClass.Append(" and g.subgenus_name is null and g.section_name is null and g.subsection_name is null and g.series_name is null and g.subseries_name is null");
                                searchString += " <b>generic names</b>";
                            }
                            else
                                searchString += " <b>generic and infrageneric names</b>";

                            searchString += " with" + " <b>genus</b>" + " = '" + genus + "' ";

                            if (sbClassDisplay.Length > 0)
                                searchString += " & <b>classification</b> = '" + sbClassDisplay.ToString().Substring(4, sbClassDisplay.ToString().Length - 4) + "'";

                            sql += sbClass.ToString();

                            //dt = dm.Read(sql);
                            dt = dm.Read(sql, new DataParameters(":s", genus.Replace("*", "%") + "%", DbType.String));
                            if (dt.Rows.Count == 1)
                                Response.Redirect("~/taxonomygenus.aspx?id=" + dt.Rows[0]["taxonomy_genus_id"].ToString());
                            else if (dt.Rows.Count > 0)
                            {
                                pnlSearch.Visible = false;
                                pnlResult.Visible = true;

                                displayDataGenus(dt);
                            }
                            else
                            {
                                string noresult = "No data found matching your criteria. ";
                                Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                            }
                            lblCriteria.Text = searchString;
                            lblFamGen.Text = "Generic Nomenclature in GRIN Taxonomy";
                        }
                    }
                }
                else if (!String.IsNullOrEmpty(txtFamily.Text))
                {

                    string family = txtFamily.Text.Trim();
                    family = Utils.Sanitize(family);

                    using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            string sql = "";
                            DataTable dt = null;

                            sql = @"select distinct taxonomy_family_id, family_name, family_authority, alternate_name, subfamily_name, tribe_name, subtribe_name 
                                    from taxonomy_family where family_name like :s ";

                            if (family.IndexOf("*") < 0) family += "*";

                            if (cbAccept.Checked)
                            {
                                sbClass.Append(" and taxonomy_family_id = current_taxonomy_family_id");
                                searchString += "<b>accepted</b>";
                            }

                            if (cbinfrafam.Checked)
                            {
                                sbClass.Append(" and subfamily_name is null and tribe_name is null and subtribe_name is null");
                                searchString += " <b>family names</b>";
                            }
                            else
                                searchString += " <b>family and infrafamily names</b>";

                            searchString += " with" + " <b>family</b>" + " = '" + family + "' ";

                            if (sbClassDisplay.Length > 0)
                                searchString += " & <b>classification</b> = '" + sbClassDisplay.ToString().Substring(4, sbClassDisplay.ToString().Length - 4) + "'";

                            sql += sbClass.ToString().Replace("f.", "");

                            dt = dm.Read(sql, new DataParameters(":s", family.Replace("*", "%") + "%", DbType.String));
                            if (dt.Rows.Count == 1)
                                Response.Redirect("~/taxonomyfamily.aspx?id=" + dt.Rows[0]["taxonomy_family_id"].ToString());
                            else if (dt.Rows.Count > 0)
                            {
                                pnlSearch.Visible = false;
                                pnlResult.Visible = true;

                                displayDataFamily(dt);
                            }
                            else
                            {
                                string noresult = "No data found matching your criteria. ";
                                Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                            }
                            lblCriteria.Text = searchString;
                            lblFamGen.Text = "Family Information in GRIN Taxonomy";
                        }
                    }
                }
                else
                    Master.ShowError(Page.GetDisplayMember("Search", "doSearch{noCriteria}", "No genus or family was entered. Please enter one or the other."));
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                string noresult = "No data found matching your criteria. ";
                Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
            }
        }

        private void displayDataGenus(DataTable dt)
        {
            string name = "";
            string namesort = "";

            dt.Columns.Add("linktext", typeof(string));
            dt.Columns.Add("sorttext", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                name = "<i>" + dr["genus_name"] +  "</i> " + dr["genus_authority"];
                namesort = name;

                if(!string.IsNullOrEmpty(dr["subgenus_name"].ToString()))
                {
                    name += " subg. <i>" + dr["subgenus_name"] + "</i>";
                    namesort += " " + dr["subgenus_name"]; 
}

                if(!string.IsNullOrEmpty(dr["section_name"].ToString()))
                {
                    name += " sect. <i>" + dr["section_name"] + "</i>";
                    namesort += " " + dr["section_name"]; 
                }

                if(!string.IsNullOrEmpty(dr["subsection_name"].ToString()))
                {
                    name += " subsect. <i>" + dr["subsection_name"] + "</i>";
                    namesort += " " + dr["subsection_name"]; 
                }

                if(!string.IsNullOrEmpty(dr["series_name"].ToString()))
                {
                    name += " ser. <i>" + dr["series_name"] + "</i>";
                    namesort += " " + dr["series_name"]; 
                }

                if(!string.IsNullOrEmpty(dr["subseries_name"].ToString()))
                {
                    name += " subser. <i>" + dr["subseries_name"] + "</i>";
                    namesort += " " + dr["subseries_name"]; 
                }

                dr["linktext"] = " <a href='../taxonomygenus.aspx?id=" + dr["taxonomy_genus_id"] + "'>" + name.Trim() + "</a>";

                dr["sorttext"] = namesort;
            }
            dt.DefaultView.Sort = "sorttext";

            rptResult.DataSource = dt;
            rptResult.DataBind();
        }

        private void displayDataFamily(DataTable dt)
        {
            string name = "";
            string namesort = "";

            dt.Columns.Add("linktext", typeof(string));
            dt.Columns.Add("sorttext", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                name = "<i>" + dr["family_name"] +  "</i> " + dr["family_authority"];
                namesort = name;

                if (!string.IsNullOrEmpty(dr["alternate_name"].ToString()))
                {
                    name += " (<i>" + dr["alternate_name"] + "</i>)";
                    namesort += " " + dr["alternate_name"];
                }

                if(!string.IsNullOrEmpty(dr["subfamily_name"].ToString())){
                    name += " subfam. <i>" + dr["subfamily_name"] + "</i>";
                    namesort += " " + dr["subfamily_name"];
                }

                if (!string.IsNullOrEmpty(dr["tribe_name"].ToString()))
                {
                    name += " tribe <i>" + dr["tribe_name"] + "</i>";
                    namesort += " " + dr["tribe_name"];
                }

                if (!string.IsNullOrEmpty(dr["subtribe_name"].ToString()))
                {
                    name += " subtribe <i>" + dr["subtribe_name"] + "</i>";
                    namesort += " " + dr["subtribe_name"];
                }

                dr["linktext"] = " <a href='../taxonomyfamily.aspx?id=" + dr["taxonomy_family_id"] + "'>" + name.Trim() + "</a>";
                dr["sorttext"] = namesort;
            }
            dt.DefaultView.Sort = "sorttext";

            rptResult.DataSource = dt;
            rptResult.DataBind();
        }
    }
}
