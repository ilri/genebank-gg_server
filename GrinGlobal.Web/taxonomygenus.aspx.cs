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
    public partial class TaxonomyGenus : System.Web.UI.Page
    {
        string _type;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                if (this.Request.QueryString["type"] != null)
                    ViewState["type"] = this.Request.QueryString["type"].ToString();
                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
            }
        }

        private void bindData(int taxonomygenusID)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                bindTaxonomy(sd, taxonomygenusID);
                bindReferences(sd, taxonomygenusID);
                bindOtherReferences();
                bindSynonyms(sd, taxonomygenusID);
                if (_type == "genus") bindCheckOther(sd, taxonomygenusID);
                bindSubdivisions(sd, taxonomygenusID);
                bindImages(sd, taxonomygenusID);
            }
        }

        private void bindTaxonomy(SecureData sd, int taxonomygenusID) {
            string value = "";
            var dt= sd.GetData("web_taxonomygenus_summary", ":taxonomygenusid=" + taxonomygenusID, 0, 0).Tables["web_taxonomygenus_summary"];
            dvGenus.DataSource = dt;
            dvGenus.DataBind();

            ViewState["genus_short_name"] = dt.Rows[0]["genus_short_name"];

            if (ViewState["type"] != null)
            {
                _type = ViewState["type"].ToString();
                switch (ViewState["type"].ToString())
                {
                    case "subgenus":
                        ViewState["name"] = dt.Rows[0]["subgenus_name"];
                        _type = "subgenus";
                        value = ViewState["genus_short_name"].ToString() + " subg. " + dt.Rows[0]["subgenus_name"];
                        Page.DataBind();
                        dvGenus.FindControl("pnlSubGenus").Visible = true;
                        break;
                    case "section":
                        ViewState["name"] = dt.Rows[0]["section_name"];
                        _type = "section";
                        value = ViewState["genus_short_name"].ToString() + " sect. " + dt.Rows[0]["section_name"];
                        Page.DataBind();
                        if (dt.Rows[0]["subgenus_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSubGenus").Visible = true;
                        dvGenus.FindControl("pnlSect").Visible = true;
                        break;
                    case "subsection":
                        ViewState["name"] = dt.Rows[0]["subsection_name"];
                        _type = "subsection";
                        value = ViewState["genus_short_name"].ToString() + " subsect. " + dt.Rows[0]["subsection_name"];
                        Page.DataBind();
                        if (dt.Rows[0]["subgenus_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSubGenus").Visible = true;
                        if (dt.Rows[0]["section_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSect").Visible = true;
                        dvGenus.FindControl("pnlSubSect").Visible = true;
                        break;
                    case "series":
                        ViewState["name"] = dt.Rows[0]["series_name"];
                        _type = "series";
                        value = ViewState["genus_short_name"].ToString() + " seri. " + dt.Rows[0]["series_name"];
                        Page.DataBind();
                        if (dt.Rows[0]["subgenus_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSubGenus").Visible = true;
                        if (dt.Rows[0]["section_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSect").Visible = true;
                        if (dt.Rows[0]["subsection_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSubSect").Visible = true;
                        dvGenus.FindControl("pnlSeri").Visible = true;
                        break;
                    case "subseries":
                        ViewState["name"] = dt.Rows[0]["subseries_name"];
                        _type = "subseries";
                        value = ViewState["genus_short_name"].ToString() + " subseri. " + dt.Rows[0]["subseries_name"];
                        Page.DataBind();
                        if (dt.Rows[0]["subgenus_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSubGenus").Visible = true;
                        if (dt.Rows[0]["section_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSect").Visible = true;
                        if (dt.Rows[0]["subsection_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSubSect").Visible = true;
                        if (dt.Rows[0]["series_name"] != DBNull.Value)
                            dvGenus.FindControl("pnlSeri").Visible = true;
                        dvGenus.FindControl("pnlSubSeri").Visible = true;
                        break;
                }
            }
            else
            {
                ViewState["name"] = dt.Rows[0]["genus_name"];
                _type = "genus";
                value = ViewState["genus_short_name"].ToString();
                Page.DataBind();
            }

            if (dt.Rows[0]["subfamily"] == DBNull.Value)
                dvGenus.FindControl("tr_subfamily").Visible = false;
            if (dt.Rows[0]["tribe"] == DBNull.Value)
                dvGenus.FindControl("tr_tribe").Visible = false;
            if (dt.Rows[0]["subtribe"] == DBNull.Value)
                dvGenus.FindControl("tr_subtribe").Visible = false;
            if (dt.Rows[0]["altfamily"] == DBNull.Value)
                dvGenus.FindControl("tr_altfamily").Visible = false;
            if (dt.Rows[0]["common_name"] == DBNull.Value)
                dvGenus.FindControl("tr_common_name").Visible = false;
            if (dt.Rows[0]["note"] == DBNull.Value)
                dvGenus.FindControl("tr_comments").Visible = false;

            hlRecordlist.NavigateUrl = "taxonomylist.aspx?category=species&type=" + _type + "&value=" + value + "&id=" + taxonomygenusID;
       }

        private void bindReferences(SecureData sd, int taxonomygenusid) {
            var dt = sd.GetData("web_taxonomygenus_references", ":taxonomygenusid=" + taxonomygenusid, 0, 0).Tables["web_taxonomygenus_references"];
            if (dt.Rows.Count > 0)
            {
                pnlReference.Visible = false;
                rptReferences.DataSource = dt;
                rptReferences.DataBind();
            }
            else
            {
                pnlReference.Visible  = true;
            }
        }

        private void bindOtherReferences()
        {
            if (ViewState["type"] == null)
            {
                pnlMore.Visible = true;
                string gName = ViewState["genus_short_name"].ToString();
                hlKBD.NavigateUrl = "http://www.kew.org/kbd/advancedsearch.do?keywords=" + gName;
                txtGoogle.Text = gName;
                btnGoogle.OnClientClick = "javascript:window.open(" + "\"" + "http://scholar.google.com/scholar?q=" + gName + "\"" + ")";
            }
        }

        private void bindSynonyms(SecureData sd, int taxonomygenusid)
        {
            var dt = sd.GetData("web_taxonomygenus_synonyms", ":taxonomygenusid=" + taxonomygenusid, 0, 0).Tables["web_taxonomygenus_synonyms"];
            if (dt.Rows.Count > 0)
            {
                pnlSynonyms.Visible = true;
                rptSynonyms.DataSource = dt;
                rptSynonyms.DataBind();
            }
        }


        private void bindCheckOther(SecureData sd, int taxonomygenusID) //temp query now
        {
            var dt = sd.GetData("web_taxonomygenus_checkother", ":taxonomygenusid=" + taxonomygenusID, 0, 0).Tables["web_taxonomygenus_checkother"];
            if (dt.Rows.Count > 0)
            {
                pnlCheckOther.Visible = true;
                rptCheckOther.DataSource = dt;
                rptCheckOther.DataBind();
            }
        }

        private void bindSubdivisions(SecureData sd, int taxonomygenusID)  
        {
            DataTable dt = null;
            string columntype = "";

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
                    "web_taxonomygenus_subdivisions",
                    ":columntype=" + columntype +
                    ";:taxonomygenusid=" + taxonomygenusID
                    , 0, 0).Tables["web_taxonomygenus_subdivisions"];


            if (dt.Rows.Count > 0)
            {
                pnlSubdivisons.Visible = true;
                gvSubdivisions.DataSource = dt;
                gvSubdivisions.DataBind();
            }
        }

        private void bindImages(SecureData sd, int taxonomygenusID) //temp query now
        {
            var dt = sd.GetData("web_taxonomygenus_images", ":taxonomygenusid=" + taxonomygenusID, 0, 0).Tables["web_taxonomygenus_images"];
            if (dt.Rows.Count > 0)
            {
                pnlImages.Visible = true;
                rptImages.DataSource = dt;
                rptImages.DataBind();
            }
        }

        protected string getName()
        {
            return ViewState["name"].ToString();
        }

        protected string getNameTitle()
        {
            return _type;
        }

        protected string DisplayNote(object note)
        {
            if (!String.IsNullOrEmpty(note.ToString()))
            {
                string note1 = note as string;

                //int i = note1.IndexOf("\\;");
                note1 = System.Text.RegularExpressions.Regex.Split(note1, "\\;")[0];
                if (note1.Length > 1)
                {
                    if (note1.Substring(note1.Length - 1, 1) == "\\")
                        note1 = note1.Substring(0, note1.Length - 1);
                }

                return note1;

            }
            return "";
        }
    }
}
