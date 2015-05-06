using System;
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
    public partial class taxonomysearchcwr : System.Web.UI.Page
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
                // CWR crop list 
                var dt = sd.GetData("web_lookup_taxon_crop", "", 0, 0).Tables["web_lookup_taxon_crop"];
                lstCrop.DataSource = dt;
                lstCrop.DataBind();

                lstCrop.Items.Insert(0, new ListItem("ALL", "ALL"));
                lstCrop.SelectedIndex = 0;

                
                // Taxon family list
                dt = sd.GetData("web_lookup_taxon_family_cwr", "", 0, 0).Tables["web_lookup_taxon_family_cwr"];

                lstFamily.DataSource = dt;
                lstFamily.DataBind();

                lstFamily.Items.Insert(0, new ListItem("ALL FAMILIES", "0"));
                lstFamily.Items.Insert(1, new ListItem("all pteridophytes", "ferns"));
                lstFamily.Items.Insert(2, new ListItem("all gymnosperms", "gymno"));
                lstFamily.Items.Insert(3, new ListItem("all angiosperms", "angio"));
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

                // Repository list
                dt = sd.GetData("web_lookup_site_list", "", 0, 0).Tables["web_lookup_site_list"];
                lstRepository.DataSource = dt;
                lstRepository.DataBind();
                lstRepository.Items.Insert(0, new ListItem("ALL", "0"));
                lstRepository.SelectedIndex = 0;


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
            string searchString = "";
            if (!(cbPrimary.Checked || cbSecondary.Checked  || cbTertiary.Checked || cbGratfstock.Checked))
            {
                Master.ShowError(Page.GetDisplayMember("Search",  "doSearch{noCriteria}", "You must select a gene pool category and try again."));
                return;
            }

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    try
                    {
                        var dt = sd.GetData("web_lookup_taxon_crop", "", 0, 0).Tables["web_lookup_taxon_crop"];

                        List<string>  crops = new List<string> ();
                        foreach(DataRow dr in dt.Rows)
                        {
                             crops.Add(dr["value"].ToString().Split(':')[0]);
                        }

                        List<string> cropselect = new List<string>();
                        string searchCrop = "";
                        if (lstCrop.SelectedIndex == 0)
                        {
                            cropselect = crops;
                            searchCrop = "all";
                        }
                        else
                        {
                            foreach (int j in lstCrop.GetSelectedIndices())
                            {
                                cropselect.Add(lstCrop.Items[j].Value.Split(':')[0]);
                                searchCrop = searchCrop + ", " + lstCrop.Items[j].Value.Split(':')[1];
                            }
                            searchCrop = searchCrop.Substring(2, searchCrop.Length - 2);
                        }
                        searchCrop = " & <b>crops</b> = '" + searchCrop + "'";


                        // genus/species 
                        string sSearchBox = txtSearch.Text.Trim() + "*";
                        string s = sSearchBox.Trim().Replace('*', '%');
                        sSearchBox = "'" + sSearchBox + "'";
                        s = Utils.Sanitize(s);

                        string sqltaxon = "";
                        List<string> cropgenus = new List<string> ();
                        if (!String.IsNullOrEmpty(txtSearch.Text.Trim()))
                        {
                            searchString = "<b>genus/species</b> = <i>" + sSearchBox + "</i>";

                            sqltaxon = @"select distinct crop_id from
	                            taxonomy_crop_map tcm
	                            join taxonomy_species t 
		                            on tcm.taxonomy_species_id = t.taxonomy_species_id
	                            join taxonomy_genus tg
		                            on t.taxonomy_genus_id = tg.taxonomy_genus_id 
                                where 
                                tg.subgenus_name is null and tg.section_name is null  and  tg.series_name is null
	                            and (tg.qualifying_code not like '%=%' or tg.qualifying_code is null)
	                            and (tg.genus_name like :s) and tcm.crop_id in
	                            (select crop_id from taxonomy_crop_map tcm2 where tcm2.crop_id = tcm.crop_id and tcm2.crop_genepool_reviewers is not null)";

                            dt = dm.Read(sqltaxon, new DataParameters(":s", s, DbType.String));
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    cropgenus.Add(dr["crop_id"].ToString());
                                }
                                cropselect = cropselect.Intersect(cropgenus).ToList();
                            }
                            else
                                cropselect.Clear();

                        }

                        if (cropselect.Count != 0)
                        {
                            string sqlbase = "";
                            string sql = "";

                            sqlbase = @"select tcm.taxonomy_species_id, 
                                    t.name,
                                    '<i>' + COALESCE (tg.genus_name, '') + ' ' +
                                    COALESCE (t.species_name, '') + '</i> ' +
                                    COALESCE (t.species_authority, '') + ' ' +
                                    (case when t.subspecies_name IS NOT NULL then 'subsp. <i>' + t.subspecies_name + '</i> ' + COALESCE (t.subspecies_authority, '') + ' ' else '' end) +
                                    (case when t.variety_name IS NOT NULL then 'var. <i>' + t.variety_name + '</i> ' + COALESCE (t.variety_authority, '') + ' ' else '' end) +
                                    (case when t.forma_name IS NOT NULL then 'forma <i>' + t.forma_name + '</i> ' + COALESCE (t.forma_authority, '') + ' ' else '' end) as taxonomy_name 
                                    from 
                                        taxonomy_crop_map tcm
                                        join taxonomy_species t 
	                                        on tcm.taxonomy_species_id = t.taxonomy_species_id
                                        join taxonomy_genus tg
	                                        on t.taxonomy_genus_id = tg.taxonomy_genus_id 
                                    where 
                                        t.taxonomy_species_id = t.current_taxonomy_species_id 
                                        and tcm.alternate_crop_name != 'N/A'";

                                // genetic relative status
                                string searchPool = "";
                                if (cbPrimary.Checked) searchPool += "GR1";
                                if (cbSecondary.Checked) searchPool += ", GR2";
                                if (cbTertiary.Checked) searchPool += ", GR3";
                                if (cbGratfstock.Checked) searchPool += ", GS";

                                searchPool = cbPrimary.Checked ? searchPool : searchPool.Substring(2, searchPool.Length - 2);
                                searchPool = " & <b>genetic relative status</b> = '" + searchPool + "'";

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

                                            default:
                                                sbfamily2.Append(Toolkit.ToInt32(famid,0)).Append(",");   // make sure what's added is integer
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
                                            sqlfamily = " and (t.taxonomy_genus_id in (select distinct g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id where " + sbfamily.ToString() + " ) "
                                                + " or t.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_genus where taxonomy_family_id in (" + sfamily2 + "))"
                                                + " or t.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_alt_family_map where taxonomy_family_id in (" + sfamily2 + ")))";
                                        }
                                        else
                                            sqlfamily = " and t.taxonomy_genus_id in (select distinct g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id where " + sbfamily.ToString() + " ) ";
                                    }
                                    else
                                    {
                                        if (sbfamily2.Length > 0)
                                        {
                                            string sfamily2 = sbfamily2.ToString();
                                            sfamily2 = sfamily2.Substring(0, sfamily2.Length - 1);
                                            sqlfamily = " and (t.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_genus where taxonomy_family_id in (" + sfamily2 + ")) "
                                                       + " or t.taxonomy_genus_id in (select distinct taxonomy_genus_id from taxonomy_alt_family_map where taxonomy_family_id in (" + sfamily2 + ")))";
                                        }
                                    }

                                    famDisplay = famDisplay.Substring(0, famDisplay.Length - 2);
                                    searchString = (searchString.Length > 0 ? searchString + " & " : searchString + " ") + " <b>family/altfamily</b> = <i>'" + famDisplay + "'</i>";
                                }
                                else
                                    searchString = (searchString.Length > 0 ? searchString + " & " : searchString + " ") + "<b>family</b> = 'all families' ";

                                // distribution
                                string sqlnative = "";
                                string searchDistribution = "";
                                string stateid = "";
                                string statename = "";

                                string inStatus = cbNonnative.Checked? "('n', 'i')" : "('n')";   // non-Native distribution

                                if (ddlState.SelectedIndex > 0)
                                {
                                    stateid= ddlState.SelectedValue;
                                    statename = ddlState.SelectedItem.Text;

                                    sqlnative = " and t.taxonomy_species_id in (select distinct taxonomy_species_id from taxonomy_geography_map where geography_status_code in" + inStatus + " and geography_id = " + Toolkit.ToInt32(stateid, 0) + ") ";

                                    searchDistribution = " & <b>native country</b> = '" + lstCountry.SelectedItem.Text + "' & <b>native state</b> = '" + statename + "'"; 
                                }
                                else if (txtState.Text.Trim() != "")
                                {
                                    statename = Utils.Sanitize(txtState.Text.Trim());
                                    statename = Regex.Replace(statename, "or ", "", RegexOptions.IgnoreCase);
                                    sqlnative = " and t.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id where tgm.geography_status_code in" + inStatus + " and g.adm1 = '" + statename + "' and g.adm2 is null and g.adm3 is null and g.adm4 is null ) ";
                                    searchDistribution = " & <b>native state</b> = '" + statename + "'"; 
                                }
                                else if (lstCountry.SelectedIndex > 0)
                                {
                                    foreach (int i in lstCountry.GetSelectedIndices())
                                    {
                                        stateid += "'" + lstCountry.Items[i].Value.Split(':')[1] + "',";
                                        statename += lstCountry.Items[i].Text + ", ";
                                    }

                                    stateid = Utils.Sanitize(stateid.Substring(0, stateid.Length - 1));
                                    sqlnative = " and t.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id where tgm.geography_status_code in" + inStatus + " and g.country_code in (" + stateid + ") and adm2 is null and adm3 is null and adm4 is null ) ";

                                    statename = statename.Substring(0, statename.Length - 2);
                                    searchDistribution = " & <b>native country</b> = '" + statename + "'"; 
                                }
                                else if (ddlRegion.SelectedIndex > 0)
                                {
                                    stateid = ddlRegion.SelectedValue;
                                    sqlnative = " and t.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography_region_map grm on tgm.geography_id = grm.geography_id where tgm.geography_status_code in" + inStatus + " and grm.region_id = " + Toolkit.ToInt32(stateid, 0) + ") ";

                                    statename = ddlRegion.SelectedItem.Text;

                                    searchDistribution = " & <b>native continent</b> = '" + ddlContinent.SelectedItem.Text + "' & <b>native region</b> = '" + statename + "' & <b>native country</b> = 'all countries'";
                                }
                                else if (ddlContinent.SelectedIndex > 0)
                                {
                                    statename = Utils.Sanitize(ddlContinent.SelectedItem.Text);
                                    sqlnative = " and t.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography_region_map grm on tgm.geography_id = grm.geography_id join region r on grm.region_id = r.region_id where tgm.geography_status_code in" + inStatus + " and r.continent = '" + statename + "' ) ";

                                    searchDistribution = " & <b>native continent</b> = '" + statename + "' & <b>native country</b> = 'all countries'";
                                }
                                else
                                {
                                    searchDistribution = " & <b>native country</b> = 'all countries'"; 
                                }

                                if (cbNonnative.Checked) searchDistribution = searchDistribution + " & <b>Include non-native distribution</b> ";

                                // with germ
                                string sqlgerm = "";
                                string searchGerm = "";
                                if (cbGerm.Checked)
                                {
                                    sqlgerm = " and t.taxonomy_species_id in (select distinct taxonomy_species_id from accession)";
                                    searchGerm = " & <b>having NPGS accessions</b>";
                                }

                                // without germ
                                string sqlNgerm = "";
                                string searchNGerm = "";

                                if (cbNonGerm.Checked)
                                {
                                    sqlNgerm = " and t.taxonomy_species_id not in (select distinct taxonomy_species_id from accession)";
                                    searchNGerm = " & <b>lacking NPGS accessions</b>";
                                }

                                // NPGS Repository
                                string sqlRep = "";
                                string searchRep = "";
                                if (lstRepository.SelectedIndex == 0)
                                {
                                    searchRep = "all";
                                }
                                else
                                {
                                    //field += ddlItem.SelectedValue + " in (";
                                    StringBuilder sb = new StringBuilder();
                                    StringBuilder sbSearch = new StringBuilder();

                                    foreach (ListItem li in lstRepository.Items)
                                    {
                                        if (li.Selected)
                                        {
                                            if (sb.Length > 0)
                                            {
                                                sb.Append(", ").Append(Toolkit.ToInt32(li.Value,0)); // only take integer
                                                sbSearch.Append(", ").Append(li.Text);
                                            }
                                            else
                                            {
                                                sb.Append(li.Value);
                                                sbSearch.Append(li.Text);
                                            }
                                        }
                                    }
                                    sqlRep = @" and t.taxonomy_species_id in (select distinct taxonomy_species_id from accession a join cooperator c
                                             on a.owned_by = c.cooperator_id join site s on c.site_id = s.site_id where s.site_id in ("
                                             + sb.ToString() + "))";

                                    searchRep = sbSearch.ToString();

                                }
                                searchRep = " & <b>repositories</b> = '" + searchRep + "'";


                                sqlbase = sqlbase + sqlfamily + sqlnative + sqlRep + sqlgerm + sqlNgerm;

                                foreach (string cropid in cropselect)
                                {
                                    PlaceHolder ph = (PlaceHolder)pnlResult.FindControl("phData");

                                    cwrgeneticcontrol uc = (cwrgeneticcontrol)LoadControl("cwrgeneticcontrol.ascx");
                                    uc.Crop = cropid;
                                    uc.Primary = cbPrimary.Checked;
                                    uc.Secondary = cbSecondary.Checked;
                                    uc.Tertiary = cbTertiary.Checked;
                                    uc.Gratfstock = cbGratfstock.Checked;
                                    uc.SqlBase = sqlbase;
                                    uc.CropCount = cropselect.Count;

                                    ph.Controls.Add(uc);

                                    if (uc.HasData)
                                    {
                                        pnlResult.Visible = true;
                                        pnlSearch.Visible = false;
                                    }
                                 }

                                if (pnlResult.Visible)
                                {
                                    searchString = "(for the query: " + searchString + searchDistribution + searchCrop + searchPool + searchRep + searchGerm + searchNGerm + ")";
                                    lblCriteria.Text = searchString;
                                }
                                else
                                {
                                    string noresult = "No data found matching your criteria. ";
                                    Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                                }
                        }
                        else
                        {
                            string noresult = "No data found matching your criteria. ";
                            Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                        }
                    }
                    catch (Exception e)
                    {
                        string err = e.Message;
                        string noresult = "No data found matching your criteria. ";
                        Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                    }
                }
            }
        }
 
    }
}
