using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using System.Text;
using GrinGlobal.Business;
using System.Web.Services;

namespace GrinGlobal.Web {
    public partial class TaxonomyBrowse : System.Web.UI.Page {

        bool _orderByAccessionCount;
        string _viewBy;
        bool _excludeZeros;
        string _genus;
        string _species;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Toolkit.IsNonWindowsOS) {
                ClientScript.RegisterClientScriptInclude("jquery.ui.autocomplete", Page.ResolveClientUrl("/gringlobal/scripts/jquery.autocomplete-1.0.2.js"));
            } else {
                ScriptManager.RegisterClientScriptInclude(this, typeof(string), "jquery.ui.autocomplete", Page.ResolveClientUrl("~/scripts/jquery.autocomplete-1.0.2.js"));
            }
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
            
            _orderByAccessionCount = Request["orderby"] == "accessioncount" || rblOrderBy.SelectedValue == "accessioncount";
            _viewBy = rblViewBy.SelectedValue + Request["view"];
            _excludeZeros = chkExclude.Checked || Request["excludeempty"] == "true";
            _genus = getNameRestriction(Utils.Sanitize(txtGenus.Text), ddlGenusCompare.SelectedValue);
            _species = getNameRestriction(Utils.Sanitize(txtSpecies.Text), ddlSpeciesCompare.SelectedValue);
        }

        private string getNameRestriction(string value, string compareType) 
        {
            string ret = "";
            if (value == "") {
                ret = "%";
            } else {
                switch (compareType) {
                    case "endwith":
                        ret = "%" + value.Replace('*', '%');
                        break;
                    case "startwith":
                    default:
                        ret = value.Replace('*', '%') + "%";
                        break;
                    case "contain":
                        ret = "%" + value.Replace('*', '%') + "%";
                        break;
                    case "exactlymatch":
                        // nothing to do
                        ret = value;
                        break;
                }
            }
            return ret;
        }

        private DataTable getTreeData(int? ID, int depth)
        {
            // depth is given as current level, we're drilling down one. so increment it.
            depth++;

            string cacheKey = _viewBy + ID + _excludeZeros + _orderByAccessionCount + _genus + ddlGenusCompare.SelectedValue + _species + ddlSpeciesCompare.SelectedValue + depth;
            CacheManager cm = CacheManager.Get("Taxonomy");
            if (cm[cacheKey] == null) 
            {
                using (var sd = UserManager.GetSecureData(true)) 
                {
                    string[] matrix;
                    //string level = "";

                    switch (_viewBy) {
                        case "family":
                        default:
                            matrix = new string[] { "family", "genus", "species" };
                            //if (_species == "%") level = "_level";
                            break;
                        case "genus":
                            matrix = new string[] { "genus", "species", "species" };
                           //if (_species == "%") level = "_level";
                            break;
                        case "species":
                            matrix = new string[] { "species", "species", "species" };
                            break;
                    }

                    
                    //cm[cacheKey] = sd.GetData(
                    //    "web_taxonomybrowse_" + matrix[depth] + level,
                    //    ":orderby=" + (_orderByAccessionCount ? "count(distinct a.accession_id) desc," : "") +
                    //    ";:jointype=" + (_excludeZeros ? "inner" : "left") +
                    //    ";:id=" + ID +
                    //    ";:genus=" + _genus +
                    //    ";:species=" + _species
                    //    , 0, 0).Tables["web_taxonomybrowse_" + matrix[depth] + level];
                    cm[cacheKey] = sd.GetData(
                        "web_taxonomybrowse_" + matrix[depth],
                        ":orderby=" + (_orderByAccessionCount ? "count(distinct a.accession_id) desc," : "") +
                        ";:jointype=" + (_excludeZeros ? "inner" : "left") +
                        ";:id=" + ID +
                        ";:genus=" + _genus +
                        ";:species=" + _species
                        , 0, 0).Tables["web_taxonomybrowse_" + matrix[depth]];

                }
            }

            return cm[cacheKey] as DataTable;

        }

        private void fillTreeNode(TreeNode parent, DataTable dt)
        {
            bool notEmpty = chkExclude.Checked;

            if (parent != null) {
                parent.ChildNodes.Clear();
            }
            var expandable = dt.Columns.Contains("genus_name") || dt.Columns.Contains("family_name");

            foreach(DataRow dr in dt.Rows){
                string display = "";
                if (!expandable)
                {
                    if (!String.IsNullOrEmpty(dr["synonym"].ToString()))
                        if (notEmpty)
                            display = dr["name"].ToString() + " (=" + dr["synonym"].ToString() + ") (" + dr["accession_count"].ToString() + ")";
                        else
                            display = dr["name"].ToString() + " (=" + dr["synonym"].ToString() + ")";
                    else
                        if (notEmpty)
                            display = dr["display_text"].ToString();
                        else
                            display = dr["name"].ToString();
                }
                else
                    display = dr["display_text"].ToString();

                var child = new TreeNode(display, dr["value"].ToString());
                child.PopulateOnDemand = expandable;
                if (parent == null){
                    tvTaxonomy.Nodes.Add(child);
                } else {
                    parent.ChildNodes.Add(child);
                }
            }
        }

        private void bindData() 
        {
            tvTaxonomy.Nodes.Clear();
            var dt = getTreeData(null, -1);
            fillTreeNode(null, dt);
        }

        private void populate(TreeNode nd) 
        {
            nd.Expanded = true;
            nd.Selected = true;

            var dt = getTreeData(Toolkit.ToInt32(nd.Value, 0), nd.Depth);
            fillTreeNode(nd, dt);

        }


        protected void tvTaxonomy_SelectedNodeChanged(object sender, EventArgs e) 
        {
            var nd = tvTaxonomy.SelectedNode;
            if (nd.PopulateOnDemand || nd.ChildNodes.Count > 0) {
                nd.Expand();
            } else {
                Response.Redirect("~/taxonomydetail.aspx?id=" + nd.Value);
                Response.End();
            }
        }

        protected void tvTaxonomy_TreeNodePopulate(object sender, TreeNodeEventArgs e) 
        {
            populate(e.Node);
        }

        protected void tvTaxonomy_TreeNodeExpanded(object sender, TreeNodeEventArgs e) 
        {
//            populate(e.Node);
        }

        protected void btnGo_Click(object sender, EventArgs e) 
        {
            bindData();
        }

        //protected string GetGenusList()
        //{
        //    string cacheKey = "genus_list";
        //    CacheManager cm = CacheManager.Get("Taxonomy");
        //    if (cm[cacheKey] == null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
        //        {
        //            using (DataManager dm = sd.BeginProcessing(true))
        //            {
        //                DataTable dt = dm.Read(@"select distinct genus_name from taxonomy_genus");
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    sb.Append(dr["genus_name"].ToString()).Append("|");
        //                }
        //            }
        //        }

        //        cm[cacheKey] = sb.ToString();
        //    }
        //    return cm[cacheKey] as string;
        //}

        //protected string GetSpeciesList()
        //{
        //    string cacheKey = "species_list";
        //    CacheManager cm = CacheManager.Get("Taxonomy");
        //    if (cm[cacheKey] == null)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
        //        {
        //            using (DataManager dm = sd.BeginProcessing(true))
        //            {
        //                DataTable dt = dm.Read(@"select distinct species_name from taxonomy_species where species_name not like '%''%'");
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    sb.Append(dr["species_name"].ToString()).Append("|");
        //                }
        //            }
        //        }

        //        cm[cacheKey] = sb.ToString();
        //    }
        //    return cm[cacheKey] as string;
        //}

        [WebMethod]
        public static string[] GetGenusList(string searchText)
        {
            List<string> names = new List<string>();
            if (searchText != "")
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        DataTable dt = dm.Read(@"select top 1 genus_name from taxonomy_genus where genus_name like '" + searchText + "%' order by genus_name asc"); // top 1, or top 10?
                       foreach (DataRow dr in dt.Rows)
                       {
                           names.Add((dr["genus_name"].ToString()));
                       }
                    }
                }
            }
            return names.ToArray();
        }

        [WebMethod]
        public static string[] GetSpeciesList(string searchText)
        {
            List<string> names = new List<string>();
            if (searchText != "")
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        DataTable dt = dm.Read(@"select top 1 species_name from taxonomy_species where species_name like '" + searchText + "%' order by species_name asc");
                        foreach (DataRow dr in dt.Rows)
                        {
                            names.Add((dr["species_name"].ToString()));
                        }
                    }
                }
            }
            return names.ToArray();
        }
    }
}
