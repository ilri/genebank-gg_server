﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace GrinGlobal.Web.taxon
{
    public partial class taxonomysearch : System.Web.UI.Page
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
                // Taxon family list
                var dt = sd.GetData("web_lookup_taxon_family", "", 0, 0).Tables["web_lookup_taxon_family"];

                lstFamily.DataSource = dt;
                lstFamily.DataBind();

                lstFamily.Items.Insert(0, new ListItem("ALL FAMILIES", "0"));
                lstFamily.Items.Insert(1, new ListItem("all pteridophytes", "ferns"));
                lstFamily.Items.Insert(2, new ListItem("all gymnosperms", "gymno"));
                lstFamily.Items.Insert(3, new ListItem("all angiosperms", "angio"));
                lstFamily.Items.Insert(4, new ListItem("plant pathogens", "pathogen"));
                lstFamily.SelectedIndex = 0;

                // Continent list
                dt = sd.GetData("web_lookup_continent", "", 0, 0).Tables["web_lookup_continent"];

                ddlContinent.DataSource = dt;
                ddlContinent.DataBind();

                // Country list
                dt = sd.GetData("web_lookup_country_taxon", "", 0, 0).Tables["web_lookup_country_taxon"];

                lstCountry.DataSource = dt;
                lstCountry.DataBind();
                lstCountry.Items.Insert(0, new ListItem("ALL COUNTRIES", "0:0"));
                lstCountry.SelectedIndex = 0;

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            searchTaxonomy();
        }

        protected void ddlContinent_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlContinent.SelectedValue;
            if (selected == "0")
            {
                ddlRegion.Items.Clear();
            }
            else
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    var dt = sd.GetData("web_lookup_region", ":regionid=" + Toolkit.ToInt32(selected, 0), 0, 0).Tables["web_lookup_region"];

                    ddlRegion.DataSource = dt;
                    ddlRegion.DataBind();
                }
            }

            ddlRegion.Items.Insert(0, new ListItem("ALL REGIONS", "0"));
        }

        protected void lstCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = lstCountry.SelectedValue;
            if (selected == "0:0" || lstCountry.GetSelectedIndices().Count() > 1)
            {
                ddlState.Items.Clear();
                ddlState.Visible = false;
                txtState.Text = "";
                txtState.Visible = true;
                lblState.Visible = true;
            }
            else
            {
                ddlState.Visible = true;
                txtState.Text = "";
                txtState.Visible = false;
                lblState.Visible = false;

                selected = selected.Split(':')[1];

                DataTable dt = Utils.GetStateList(selected);
                if (dt.Rows.Count > 0)
                {
                    ddlState.DataSource = dt;
                    ddlState.DataBind();
                }
                ddlState.Items.RemoveAt(0);
                ddlState.Items.Insert(0, new ListItem("ALL STATES/PROVINCES", "0"));
            }
        }

        private void searchTaxonomy()
        {
            try
            {
                string searchString = "";
                using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        string sqlbase = "";
                        string sql = "";
                        DataTable dt = null;
                        bool hasLike = false;

                        sqlbase = @"select distinct t.taxonomy_species_id as tid, t.current_taxonomy_species_id as tcid, t1.taxonomy_species_id as tid1,
                                t.name as name, t.name_authority as author, t1.name as name1, t1.name_authority as author1, t1.synonym_code
                                from taxonomy_species t join taxonomy_species t1 on t1.current_taxonomy_species_id = t.taxonomy_species_id where 1=1";

                        // accepted name
                        string sqlaccept = "";
                        if (cbAccepted.Checked)
                        {
                            sqlaccept = " and t1.current_taxonomy_species_id = t1.taxonomy_species_id";
                            searchString = "<b>accepted names</b> with ";
                        }

                        string sqlgerm = "";
                        if (cbGerm.Checked)
                        {
                            sqlgerm = " and t1.taxonomy_species_id in (select distinct taxonomy_species_id from accession)";
                            searchString += "<b>NPGS accessions</b>";
                        }

                        // genus/species 
                        string sSearchBox = txtSearch.Text.Trim() + "*";
                        string s = sSearchBox.Trim().Replace('*', '%');
                        s = Utils.Sanitize(s);
                        sSearchBox = "'" + sSearchBox + "'";

                        string sqltaxon = "";
                        if (!String.IsNullOrEmpty(txtSearch.Text.Trim()))
                        {
                            sqltaxon = " and (t1.name like :s or replace(t1.name, ' x ', ' ') like :s)";
                            searchString = (sqlgerm.Length > 0 ? searchString + " & " : searchString + " ") + "<b>genus/species</b> = <i>" + sSearchBox + "</i>";
                        }

                        // family
                        string sqlfamily = "";

                        if (lstFamily.SelectedIndex != 0)
                        {
                            var sbfamily = new StringBuilder();
                            var sbfamily2 = new StringBuilder();
                            string famDisplay = "";
                            string famid = "";
                            string famname = "";
                            foreach (int i in lstFamily.GetSelectedIndices())
                            {
                                famid = lstFamily.Items[i].Value;
                                famname = lstFamily.Items[i].Text; 
                                
                                switch (famid)
                                {
                                    case "ferns" :
                                        sbfamily.Append("f.note like 'fern%'");
                                        famDisplay += "ferns, ";
                                        break;

                                    case "gymno":
                                        if (sbfamily.Length > 0) sbfamily.Append(" or ");
                                        sbfamily.Append("f.note like 'gymno%'");
                                        famDisplay += famname.Substring(4, famname.Length - 4) + ", ";
                                        break;

                                    case "angio":
                                        if (sbfamily.Length > 0) sbfamily.Append(" or ");
                                        sbfamily.Append("replace(f.note, 'monocot', 'dicot') like 'dicot%'");
                                        famDisplay += famname.Substring(4, famname.Length - 4) + ", ";
                                        break;

                                    case "pathogen":
                                        if (sbfamily.Length > 0) sbfamily.Append(" or ");
                                        sbfamily.Append("f.note like 'bacter%' or f.note like 'vir%' or f.note like 'fungal%' or f.note like 'pathogen%' or f.note like 'graft%'");
                                        famDisplay += famname.Substring(6, famname.Length - 6) + ", ";
                                        break;

                                    default:
                                        sbfamily2.Append(Toolkit.ToInt32(famid, 0)).Append(",");    
                                        famDisplay += famname + ", ";
                                        break;
                                }
                            }

                            if (sbfamily.Length > 0)
                            {
                                if (sbfamily2.Length > 0)
                                {
                                    string sfamily2 = sbfamily2.ToString();
                                    sfamily2 = sfamily2.Substring(0, sfamily2.Length - 1);
                                    sqlfamily = " and (t1.taxonomy_genus_id in (select distinct g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id where " + sbfamily.ToString() + " ) "
                                        + " or t1.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_genus where taxonomy_family_id in (" + sfamily2 + "))"
                                        + " or t1.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_alt_family_map where taxonomy_family_id in (" + sfamily2 + ")))";
                                }
                                else
                                    sqlfamily = " and t1.taxonomy_genus_id in (select distinct g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id where " + sbfamily.ToString() + " ) ";
                            }
                            else
                            {
                                if (sbfamily2.Length > 0)
                                {
                                    string sfamily2 = sbfamily2.ToString();
                                    sfamily2 = sfamily2.Substring(0, sfamily2.Length - 1);
                                    sqlfamily = " and (t1.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_genus where taxonomy_family_id in (" + sfamily2 + ")) "
                                               + " or t1.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_alt_family_map where taxonomy_family_id in (" + sfamily2 + ")))";
                                }
                            }

                            famDisplay = famDisplay.Substring(0, famDisplay.Length - 2);
                            searchString = (sqlgerm.Length > 0 || sqltaxon.Length > 0 ? searchString + " & " : searchString + " ") + " <b>family/altfamily</b> = <i>'" + famDisplay + "'</i>";
                        }
                        else
                            searchString = (sqlgerm.Length > 0 || sqltaxon.Length > 0 ? searchString + " & " : searchString + " ") + "<b>family</b> = 'all families' ";

                        // common name
                        string sqlcommon = "";

                        sSearchBox = txtCommon.Text.Trim();
                        string scomm = sSearchBox.Trim().Replace('*', '%');
                        scomm = Utils.Sanitize(scomm);
                        hasLike = scomm.IndexOf('%') > -1;
                        sSearchBox = "'" + sSearchBox + "'";

                        if (!String.IsNullOrEmpty(txtCommon.Text.Trim()))
                        {
                            scomm = scomm.Replace(" ", "").Replace("-", "");
                            if (hasLike)
                            {
                                sqlcommon = " and t1.taxonomy_species_id in (select distinct taxonomy_species_id from taxonomy_common_name where simplified_name like :scomm)";
                            }
                            else
                            {
                                sqlcommon = " and t1.taxonomy_species_id in (select distinct taxonomy_species_id from taxonomy_common_name where simplified_name = :scomm)";
                            }

                            searchString += " & <b>common name</b> = <i>" + sSearchBox + "</i>";
                        }


                        searchString = "(for the query: " + searchString;

                        // native distribution
                        string sqlnative = "";

                        string stateid = "";
                        string statename = "";
                        if (ddlState.SelectedIndex > 0)
                        {
                            stateid= ddlState.SelectedValue;
                            statename = ddlState.SelectedItem.Text;

                            sqlnative = " and t1.taxonomy_species_id in (select distinct taxonomy_species_id from taxonomy_geography_map where geography_status_code = 'n' and geography_id = " + Toolkit.ToInt32(stateid, 0) + ") ";

                            searchString += " & <b>native country</b> = '" + lstCountry.SelectedItem.Text + "' & <b>native state</b> = '" + statename + "'"; 
                        }
                        else if (txtState.Text.Trim() != "")
                        {
                            statename = Utils.Sanitize(txtState.Text.Trim());
                            statename = Regex.Replace(statename, "or ", "", RegexOptions.IgnoreCase);
                            sqlnative = " and t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id where tgm.geography_status_code = 'n' and g.adm1 = '" + statename + "' and g.adm2 is null and g.adm3 is null and g.adm4 is null ) ";
                            searchString += " & <b>native state</b> = '" + statename + "'"; 
                        }
                        else if (lstCountry.SelectedIndex > 0)
                        {
                            foreach (int i in lstCountry.GetSelectedIndices())
                            {
                                stateid += "'" + lstCountry.Items[i].Value.Split(':')[1] + "',";
                                statename += lstCountry.Items[i].Text + ", ";
                            }

                            stateid = Utils.Sanitize(stateid.Substring(0, stateid.Length - 1));
                            sqlnative = " and t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id where tgm.geography_status_code = 'n' and g.country_code in (" + stateid + ") and adm2 is null and adm3 is null and adm4 is null ) ";

                            statename = statename.Substring(0, statename.Length - 2);
                            searchString += " & <b>native country</b> = '" + statename + "'"; 
                        }
                        else if (ddlRegion.SelectedIndex > 0)
                        {
                            stateid = ddlRegion.SelectedValue;
                            sqlnative = " and t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography_region_map grm on tgm.geography_id = grm.geography_id where tgm.geography_status_code = 'n' and grm.region_id = " + Toolkit.ToInt32(stateid, 0) + ") ";

                            statename = ddlRegion.SelectedItem.Text;

                            searchString += " & <b>native continent</b> = '" + ddlContinent.SelectedItem.Text + "' & <b>native region</b> = '" + statename + "' & <b>native country</b> = 'all countries'";
                        }
                        else if (ddlContinent.SelectedIndex > 0)
                        {
                            statename = Utils.Sanitize(ddlContinent.SelectedItem.Text);
                            sqlnative = " and t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography_region_map grm on tgm.geography_id = grm.geography_id join region r on grm.region_id = r.region_id where tgm.geography_status_code = 'n' and r.continent = '" + statename + "' ) ";

                            searchString += " & <b>native continent</b> = '" + statename + "' & <b>native country</b> = 'all countries'";
                        }
                        else
                        {
                            searchString += " & <b>native country</b> = 'all countries'"; 
                        }

                        // non-native distribution
                        string sqlnon = "";
//                        string sqlnonTemplate = @"( (geography_id is null and note like ':value')  
//                                                or (geography_id is not null and exists 
//                                                    (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm 
//                                                    join geography g on tgm.geography_id = g.geography_id
//                                                    join geography_region_map grm on g.geography_id = grm.geography_id 
//                                                    join region r on r.region_id = grm.region_id
//                                                    join code_value cv on g.country_code = cv.value 
//                                                    join code_value_lang cvl on cv.code_value_id = cvl.code_value_id where cvl.sys_lang_id =" + sd.LanguageID +
//                                                    @" and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' and g.adm1 is null and g.adm2 is null and g.adm3 is null and g.adm4 is null
//                                                    and ((country_code not like '%[0-9]%' and cvl.title like ':value' ) or r.continent like ':value' or r.subcontinent like ':value'))))";

                        string sqlnonTemplate = @" select distinct taxonomy_species_id from taxonomy_geography_map where geography_status_code != 'n' and geography_id is null and note like ':value' 
                                                 union select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm 
                                                    join geography g on tgm.geography_id = g.geography_id
                                                    join geography_region_map grm on g.geography_id = grm.geography_id 
                                                    join region r on r.region_id = grm.region_id
                                                    join code_value cv on g.country_code = cv.value 
                                                    join code_value_lang cvl on cv.code_value_id = cvl.code_value_id 
                                                    where tgm.geography_status_code != 'n' and tgm.geography_id is not null and cvl.sys_lang_id =" + sd.LanguageID +
                                                    @" and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' and g.adm1 is null and g.adm2 is null and g.adm3 is null and g.adm4 is null
                                                    and ((country_code not like '%[0-9]%' and cvl.title like ':value' ) or r.continent like ':value' or r.subcontinent like ':value') ";

                        string nonNative = txtNon.Text.Trim();
                        nonNative = Utils.Sanitize(nonNative);
                        nonNative = Regex.Replace(nonNative, "or ", "", RegexOptions.IgnoreCase);

                        if (!String.IsNullOrEmpty(nonNative))
                        {
                            var sbNon = new StringBuilder();
                            string[] nonEntry = nonNative.Split(',');

                            foreach (string str in nonEntry)
                            {
                                string s1 = str.Trim().Replace("cultivated", "cult.").Replace("naturalized", "natzd.").Replace("introduced", "introd.").Replace("incl", "including");
                                sbNon.Append(" union ").Append(sqlnonTemplate.Replace(":value", "%" + s1 + "%"));
                            }

                            sqlnon = sbNon.ToString();
                            sqlnon = sqlnon.Substring(7, sqlnon.Length - 7);

                            string op = "";
                            if (sqlnative != "")
                            {
                                sqlnative = " and ( " + sqlnative.Substring(5, sqlnative.Length - 5);
                                //sqlnative = sqlnative.Substring(0, sqlnative.Length - 2);

                                if (rblNonNative.SelectedValue == "OrNon")
                                    op = " or ";
                                else
                                    op = " and ";

                                sqlnon = op + " t1.taxonomy_species_id in (" + sqlnon + "))";
                            }
                            else
                            {
                                op = " and ";
                                sqlnon = op + " t1.taxonomy_species_id in (" + sqlnon + ")";
                            }


                            //if (op == " and ")
                            //    sqlnon = op + " t1.taxonomy_species_id in (select distinct taxonomy_species_id from taxonomy_geography_map where geography_status_code != 'n' and " + sqlnon + ")";
                            //else   
                            //{
                            //    sqlnative = sqlnative.Substring(0, sqlnative.Length - 2);
                            //    sqlnon = " union select distinct taxonomy_species_id from taxonomy_geography_map where geography_status_code != 'n' and " + sqlnon + ")";
                            //}

                            searchString += (op == " and " ? " & " : " or ") + "<b> non-native range</b> = '" + nonNative + "'"; 
                        }

                        if ((sqltaxon + sqlfamily + sqlcommon + sqlaccept + sqlgerm + sqlnative + sqlnon) != "")
                        {
                            sql = sqlbase + sqltaxon + sqlfamily + sqlcommon + sqlaccept + sqlgerm + sqlnative + sqlnon;
                            searchString += ")";

                            sql = sql + " order by name1";

                            if (sqltaxon != "")
                            {   
                                if (sqlcommon == "")
                                    dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                else
                                    dt = dm.Read(sql, new DataParameters(":s", s, DbType.String, ":scomm", scomm, DbType.String));
                            }
                            else if (sqlcommon != "")
                            {
                                dt = dm.Read(sql, new DataParameters(":scomm", scomm, DbType.String));
                            }
                            else
                                dt = dm.Read(sql);

                            //if (dt.Rows.Count == 1)
                            //    Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid1"].ToString());
                            if (dt.Rows.Count > 0)
                            {
                                pnlSearch.Visible = false;
                                pnlResult.Visible = true;

                                displayData(dt);
                            }
                            else
                            {
                                string noresult = "No data found matching your criteria. ";
                                Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                            }
                            lblCriteria.Text = searchString;
                        }
                        else
                        {
                            Master.ShowError(Page.GetDisplayMember("Search", "doSearch{noCriteria}", "Please enter search string or other search criteria and try again."));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                string noresult = "No data found matching your criteria. ";
                Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));

            }
        }

        //private void displayData(DataTable dt)
        //{
        //    string name = "";
        //    string sname = "";

        //    dt.Columns.Add("linktext", typeof(string));

        //    int idx = 1;
        //    string sidx = "";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        name = "<b><i>" + dr["name"].ToString().Replace(" f. ", " </i>f.<i> ") + "</i> " + dr["author"] + "</b>";
        //        sname = "<i>" + dr["name1"].ToString().Replace(" f. ", " </i>f.<i> ") + "</i> " + dr["author1"];
        //        sidx = String.Format("{0,4}", idx).Replace(" ", "&nbsp;");

        //        if (dr["tid"].ToString() == dr["tid1"].ToString())
        //            dr["linktext"] = sidx + ". <a href='../taxonomydetail.aspx?id=" + dr["tid"] + "'>" + name.Trim() + "</a>";
        //        else
        //            dr["linktext"] = sidx + ". <a href='../taxonomydetail.aspx?id=" + dr["tid"] + "'>" + sname.Trim() + " (=" + name.Trim() + ")</a>";

        //        idx++;
        //    }

        //    rptResult.DataSource = dt;
        //    rptResult.DataBind();
        //}

        private void displayData(DataTable dt)
        {
            string name = "";
            string sname = "";

            dt.Columns.Add("linktext", typeof(string));
            dt.Columns.Add("sorttext", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                name = "<b><i>" + dr["name"].ToString().Replace(" f. ", " </i>f.<i> ") + "</i> " + dr["author"] + "</b>";
                sname = "<i>" + dr["name1"].ToString().Replace(" f. ", " </i>f.<i> ") + "</i> " + dr["author1"];

                if (dr["tid"].ToString() == dr["tid1"].ToString())
                    dr["linktext"] = TaxonUtil.ItalicTaxon(" <a href='../taxonomydetail.aspx?id=" + dr["tid"] + "'>" + name.Trim() + "</a>");
                else
                    dr["linktext"] = TaxonUtil.ItalicTaxon(" <a href='../taxonomydetail.aspx?id=" + dr["tid"] + "'>" + sname.Trim() + " (=" + name.Trim() + ")</a>");

                dr["sorttext"] = TaxonUtil.RemoveTaxon(dr["name1"].ToString());
            }
            dt.DefaultView.Sort = "sorttext";

            rptResult.DataSource = dt;
            rptResult.DataBind();
        }
    }
}
