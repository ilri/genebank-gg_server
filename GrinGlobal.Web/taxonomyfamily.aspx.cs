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
    public partial class TaxonomyFamily : System.Web.UI.Page
    {
        string _type;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Request.QueryString["type"] != null)
                    ViewState["type"] = this.Request.QueryString["type"].ToString();
                bindData(Toolkit.ToInt32(Request.QueryString["id"], 0));
                
            }
        }
        private void bindData(int taxonomyfamilyID)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                bindTaxonomy(sd, taxonomyfamilyID);
                bindReferences(sd, taxonomyfamilyID);
                bindOtherReferences();
                if (_type == "family")
                {
                    bindCheckOther(sd, taxonomyfamilyID);
                    bindSynonyms(sd, taxonomyfamilyID);
                }
                bindSubdivisions(sd, taxonomyfamilyID);
                bindImages(sd, taxonomyfamilyID);
            }
        }

        private void bindTaxonomy(SecureData sd, int taxonomyfamilyID)
        {
            string value = "";
            var dt = sd.GetData("web_taxonomyfamily_summary", ":taxonomyfamilyid=" + taxonomyfamilyID, 0, 0).Tables["web_taxonomyfamily_summary"];
            dvFamily.DataSource = dt;
            dvFamily.DataBind();

            if (ViewState["type"] != null)
            {
                switch (ViewState["type"].ToString())
                {
                    case "subfamily":
                        ViewState["name"] = dt.Rows[0]["subfamily"];
                        _type = "subfamily";
                        value = dt.Rows[0]["family_short_name"] + " subfam. " + dt.Rows[0]["subfamily"];
                        Page.DataBind(); //do bind before setting visible property
                        dvFamily.FindControl("pnlSubFamily").Visible = true;
                        break;
                    case "tribe":
                        ViewState["name"] = dt.Rows[0]["tribe"];
                        _type = "tribe";
                        value = dt.Rows[0]["family_short_name"] + " tribe " + dt.Rows[0]["tribe"];
                        Page.DataBind();
                        if (dt.Rows[0]["subfamily"] != DBNull.Value)
                            dvFamily.FindControl("pnlSubFamily").Visible = true;
                        dvFamily.FindControl("pnlTribe").Visible = true;
                        break;
                    case "subtribe": 
                        ViewState["name"] = dt.Rows[0]["subtribe"];
                        _type = "subtribe";
                        value = dt.Rows[0]["family_short_name"] + " subtribe " + dt.Rows[0]["subtribe"];
                        Page.DataBind();
                        if (dt.Rows[0]["subfamily"] != DBNull.Value)
                            dvFamily.FindControl("pnlSubFamily").Visible = true;
                        if (dt.Rows[0]["tribe"] != DBNull.Value)
                            dvFamily.FindControl("pnlTribe").Visible = true;
                        dvFamily.FindControl("pnlSubTribe").Visible = true;
                        break;
                }
            }
            else
            {
                ViewState["name"] = dt.Rows[0]["family_short_name"];
                _type = "family";
                value = dt.Rows[0]["family_short_name"].ToString();
                Page.DataBind();
            }

            if (dt.Rows[0]["altfamily"] == DBNull.Value)
                dvFamily.FindControl("tr_alternatename").Visible = false;
            if (dt.Rows[0]["genus_type"] == DBNull.Value)
                dvFamily.FindControl("tr_typegenus").Visible = false;
            if (dt.Rows[0]["note"] == DBNull.Value)
                dvFamily.FindControl("tr_comments").Visible = false;

            if (_type == "family")
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["subfamily"].ToString()))
                    dvFamily.FindControl("pnlSubFamily1").Visible = true;
                if (!String.IsNullOrEmpty(dt.Rows[0]["tribe"].ToString()))
                    dvFamily.FindControl("pnlTribe1").Visible = true;
                if (!String.IsNullOrEmpty(dt.Rows[0]["subtribe"].ToString()))
                    dvFamily.FindControl("pnlSubTribe1").Visible = true;
            }

            if (_type == "subfamily")
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["tribe"].ToString()))
                    dvFamily.FindControl("pnlTribe1").Visible = true;
                if (!String.IsNullOrEmpty(dt.Rows[0]["subtribe"].ToString()))
                    dvFamily.FindControl("pnlSubTribe1").Visible = true;
            }
            if (_type == "tribe")
            {
                if (!String.IsNullOrEmpty(dt.Rows[0]["subtribe"].ToString()))
                    dvFamily.FindControl("pnlSubTribe1").Visible = true;
            }


            hlRecordlist.NavigateUrl = "taxonomylist.aspx?category=genera&type=" + _type + "&value=" + value + "&id=" + taxonomyfamilyID;

        }

        private void bindReferences(SecureData sd, int taxonomyfamilyID) 
        {
            var dt = sd.GetData("web_taxonomyfamily_references", ":taxonomyfamilyid=" + taxonomyfamilyID, 0, 0).Tables["web_taxonomyfamily_references"];
            if (dt.Rows.Count > 0)
            {
                pnlReference.Visible = false;
                rptReferences.DataSource = dt;
                rptReferences.DataBind();
            }
            else
            {
                pnlReference.Visible = true;
            }
        }

        private void bindOtherReferences()
        {
            if (ViewState["type"] == null)
            {
                pnlMore.Visible = true;
                string gName = ViewState["name"].ToString();
                hlKBD.NavigateUrl = "http://www.kew.org/kbd/advancedsearch.do?keywords=" + gName;
                txtGoogle.Text = gName;
                btnGoogle.OnClientClick = "javascript:window.open(" + "\"" + "http://scholar.google.com/scholar?q=" + gName + "\"" + ")";
            }
        }

        private void bindSynonyms(SecureData sd, int taxonomyfamilyID)
        {
            var dt = sd.GetData("web_taxonomyfamily_synonyms", ":taxonomyfamilyid=" + taxonomyfamilyID, 0, 0).Tables["web_taxonomyfamily_synonyms"];
            if (dt.Rows.Count > 0)
            {
                pnlSynonyms.Visible = true;
                rptSynonyms.DataSource = dt;
                rptSynonyms.DataBind();
            }
        }


        private void bindCheckOther(SecureData sd, int taxonomyfamilyID)//temp query now
        {
            var dt = sd.GetData("web_taxonomyfamily_checkother", ":taxonomyfamilyid=" + taxonomyfamilyID, 0, 0).Tables["web_taxonomyfamily_checkother"];
            if (dt.Rows.Count > 0)
            {
                pnlCheckOther.Visible = true;
                rptCheckOther.DataSource = dt;
                rptCheckOther.DataBind();
            }
        }

        private void bindSubdivisions(SecureData sd, int taxonomyfamilyID) 
        {
            DataTable dt = null;

            if (ViewState["type"] != null)
            {
                switch (ViewState["type"].ToString())
                {
                    case "subfamily":
                        dt = sd.GetData(
                            "web_taxonomyfamily_subdivisions",
                            ":columntype=" + "subfamily_name" +
                            ";:taxonomyfamilyid=" + taxonomyfamilyID
                            , 0, 0).Tables["web_taxonomyfamily_subdivisions"];
                        break;
                    case "tribe":
                        dt = sd.GetData(
                            "web_taxonomyfamily_subdivisions",
                            ":columntype=" + "tribe_name" +
                            ";:taxonomyfamilyid=" + taxonomyfamilyID
                            , 0, 0).Tables["web_taxonomyfamily_subdivisions"];
                        break;
                    case "subtribe":
                        dt = sd.GetData(
                            "web_taxonomyfamily_subdivisions",
                            ":columntype=" + "subtribe_name" +
                            ";:taxonomyfamilyid=" + taxonomyfamilyID
                            , 0, 0).Tables["web_taxonomyfamily_subdivisions"];
                        break;
                }
            }
            else
                dt = sd.GetData(
                    "web_taxonomyfamily_subdivisions",
                    ":columntype=" + "family_name" +
                    ";:taxonomyfamilyid=" + taxonomyfamilyID
                    , 0, 0).Tables["web_taxonomyfamily_subdivisions"];

            if (dt.Rows.Count > 0)
            {
                pnlSubdivisons.Visible = true;
                gvSubdivisions.DataSource = dt;
                gvSubdivisions.DataBind();
            }
        }

        private void bindImages(SecureData sd, int taxonomyfamilyID) //temp query now
        {
            var dt = sd.GetData("web_taxonomyfamily_images", ":taxonomyfamilyid=" + taxonomyfamilyID, 0, 0).Tables["web_taxonomyfamily_images"];
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
