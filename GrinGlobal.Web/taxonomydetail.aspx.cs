using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using System.Data;
using GrinGlobal.Business;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;

namespace GrinGlobal.Web {
    public partial class TaxonomyDetail : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            if (!Page.IsPostBack) 
            {
                int id = 0;
                if (Request.QueryString["id"] != null)
                {
                    id = Toolkit.ToInt32(Request.QueryString["id"], 0);
                    bindData(id);
                }
                else if (Request.QueryString != null)
                {
                    if (Request.QueryString.Count == 1)
                    {
                        id = Toolkit.ToInt32(Request.QueryString.ToString(), 0);
                        if (id > 0) bindData(id);
                    }
                }
            }
        }

        private void bindData(int taxonomyID) {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken())) {
                bindTaxonomy(sd, taxonomyID);
                bindConspecific(sd, taxonomyID);
                bindCommonNames(sd, taxonomyID);
                bindEconomicImportance(sd, taxonomyID);
                bindDistribution(sd, taxonomyID);
                bindReferences(sd, taxonomyID);
                bindSynonyms(sd, taxonomyID);
                bindCheckOther(sd, taxonomyID);
                bindImages(sd, taxonomyID);
            }
        }

        private void bindTaxonomy(SecureData sd, int taxonomyID) {

            DataTable dt = sd.GetData("web_taxonomyspecies_summary", ":taxonomyid=" + taxonomyID, 0, 0).Tables["web_taxonomyspecies_summary"];
            dvTaxonomy.DataSource = dt;
            dvTaxonomy.DataBind();

            if (dt.Rows.Count > 0)
            {
                ViewState["taxonomy_name"] = dt.Rows[0]["taxonomy_name"];

                if (dt.Rows[0]["subgenus_name"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_subgenus").Visible = false;
                if (dt.Rows[0]["section_name"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_section").Visible = false;
                if (dt.Rows[0]["subsection_name"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_subsection").Visible = false;
                if (dt.Rows[0]["subfamily"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_subfamily").Visible = false;
                if (dt.Rows[0]["tribe"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_tribe").Visible = false;
                if (dt.Rows[0]["subtribe"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_subtribe").Visible = false;
                if (dt.Rows[0]["note"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_comment").Visible = false;
                if (dt.Rows[0]["protologue"] == DBNull.Value)
                    dvTaxonomy.FindControl("tr_protologue").Visible = false;
                //if (dt.Rows[0]["typification"] == DBNull.Value)  //special case
                dvTaxonomy.FindControl("tr_typification").Visible = false;
            }
        }

        private void bindConspecific(SecureData sd, int taxonomyID) {
            showAndBind(sd, "web_taxonomyspecies_conspecific", taxonomyID, pnlConspecific, rptConspecific);
        }
 
        private void bindCommonNames(SecureData sd, int taxonomyID) {
            showAndBind(sd, "web_taxonomyspecies_commonnames", taxonomyID, pnlCommonNames, rptCommonNames);
        }


        private void bindEconomicImportance(SecureData sd, int taxonomyID) {
            showAndBind(sd, "web_taxonomyspecies_economic", taxonomyID, pnlEconomicImportance, rptEconomicImportance);
        }


        void rptDistributionRange_SubcontinentItemDataBound(object sender, RepeaterItemEventArgs e) {
            // header and footer will fire this event, but have no data against which we can bind.
            // so checking against -1 prevents us from trying to (and bombing)
            if (e.Item.DataItem != null) {
                
                int taxonomyID = (int)((DataRowView)e.Item.DataItem)["taxonomy_species_id"];
                string DRcontinent = ((DataRowView)e.Item.DataItem)["continent"].ToString();
                string geoStatus = ((DataRowView)e.Item.DataItem)["geography_status_code"].ToString();

                // grab the repeater control from the repeater's row object
                Repeater rptDistSubcontinent = e.Item.FindControl("rptDistSubcontinent") as Repeater;

                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    rptDistSubcontinent.DataSource = sd.GetData("web_taxonomyspecies_distribution_sub", ":taxonomyid=" + taxonomyID + ";:continent=" + DRcontinent + ";:geostatuscode=" + geoStatus, 0, 0).Tables["web_taxonomyspecies_distribution_sub"];
                    rptDistSubcontinent.ItemDataBound += new RepeaterItemEventHandler(rptDistributionRange_CountryItemDataBound);
                    rptDistSubcontinent.DataBind();
                }
            }

        }

        void rptDistributionRange_CountryItemDataBound(object sender, RepeaterItemEventArgs e) {
            // header and footer will fire this event, but have no data against which we can bind.
            // so checking against -1 prevents us from trying to (and bombing)
            if (e.Item.DataItem != null) {

                // grab the taxonomy_id and subcontinent from the associated data item
                //int taxonomyID = (int)((DataRowView)e.Item.DataItem)["taxonomy_species_id"];
                int taxonomyID = Toolkit.ToInt32(((DataRowView)e.Item.DataItem)["taxonomy_species_id"], 0);
                string DRsubcontinent = ((DataRowView)e.Item.DataItem)["subcontinent"].ToString();
                string geoStatus = ((DataRowView)e.Item.DataItem)["geography_status_code"].ToString();

                // grab the repeater control from the repeater's row object
                Repeater rptDistCountry = e.Item.FindControl("rptDistCountry") as Repeater;

               using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    rptDistCountry.DataSource = sd.GetData("web_taxonomyspecies_distribution_country", ":taxonomyid=" + taxonomyID + ";:subcontinent=" + DRsubcontinent + ";:geostatuscode=" + geoStatus, 0, 0).Tables["web_taxonomyspecies_distribution_country"];
                    rptDistCountry.ItemDataBound += new RepeaterItemEventHandler(rptDistributionRange_StateItemDataBound);
                    rptDistCountry.DataBind();
                }
            }
        }

        void rptDistributionRange_StateItemDataBound(object sender, RepeaterItemEventArgs e) {
            // header and footer will fire this event, but have no data against which we can bind.
            // so checking against -1 prevents us from trying to (and bombing)
            if (e.Item.DataItem != null) {

                // grab the taxonomy_id and subcontinent from the associated data item
                int taxonomyID = (int)((DataRowView)e.Item.DataItem)["taxonomy_species_id"];
                string DRsubcontinent = ((DataRowView)e.Item.DataItem)["subcontinent"].ToString();
                string DRcountry = ((DataRowView)e.Item.DataItem)["country_code"].ToString();
                string geoStatus = ((DataRowView)e.Item.DataItem)["geography_status_code"].ToString();

                // grab the repeater control from the repeater's row object
                Repeater rptDistState = e.Item.FindControl("rptDistState") as Repeater;

                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    var dt = sd.GetData("web_taxonomyspecies_distribution_state", ":taxonomyid=" + taxonomyID + ";:subcontinent=" + DRsubcontinent + ";:country=" + DRcountry + ";:geostatuscode=" + geoStatus, 0, 0).Tables["web_taxonomyspecies_distribution_state"];
                    rptDistState.DataSource = dt;
                    rptDistState.DataBind();
                    rptDistState.Visible = dt.Rows.Count > 0;
                }
            }
        }


        private void bindReferences(SecureData sd, int taxonomyID) {
            showAndBind(sd, "web_taxonomyspecies_references", taxonomyID, pnlReferences, rptReferences);
        }

        private void bindSynonyms(SecureData sd, int taxonomyID) {
            showAndBind(sd, "web_taxonomyspecies_synonyms", taxonomyID, pnlSynonyms, rptSynonyms);
        }

        private void bindImages(SecureData sd, int taxonomyID) {
            showAndBind(sd, "web_taxonomyspecies_images", taxonomyID, pnlImage, rptImages);
        }

        private void showAndBind(SecureData sd, string dvName, int id, Panel pl, Repeater rpt)
        {
            DataTable dt = sd.GetData(dvName, ":taxonomyid=" + id, 0, 0).Tables[dvName];
            if (dt.Rows.Count > 0)
            {
                pl.Visible = true;
                rpt.DataSource = dt;
                rpt.DataBind();
            }
        }

        protected string getName()
        {
            return ViewState["taxonomy_name"].ToString();
        }

        protected string DisplayCommonNameCitation(object id, object source)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                string litid = id.ToString();
                string litsource = source as string;
                return "(Source: <a href=\"javascript: " + litsource + "\" onclick=\"javascript:window.open('literature.aspx?id=" + litid + "' ,'','scrollbars=yes,titlebar=no,width=570,height=380')\">" + litsource + "</a>)";
            }
            else
                return "";
        }

        protected string DisplayVerified(object date, object id, object name)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                DateTime dt = Toolkit.ToDateTime(date);
                // return dt.ToString("dd-MMM-yyyy") + " by <a href='cooperator.aspx?id=" + id.ToString() + "'> " + name.ToString() + "</a>"; 
                return dt.ToString("dd-MMM-yyyy") + " by ARS Systematic Botanists. ";  // Don't show the real name per request
            }
            else
                return "NAME NOT VERIFIED.";

        }

        private void bindDistribution(SecureData sd, int taxonomyID)
        {
            DataTable dt = sd.GetData("web_taxonomyspecies_distribution", ":taxonomyid=" + taxonomyID, 0, 0).Tables["web_taxonomyspecies_distribution"];
            if (dt.Rows.Count > 0)
            {
                pnlDistributionRange.Visible = true;
                rptDistribution.DataSource = dt;
                rptDistribution.ItemDataBound += new RepeaterItemEventHandler(rptDistribution_GeoCodeItemDataBound);
                rptDistribution.DataBind();
            }
        }

        private void rptDistribution_GeoCodeItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                int taxonomyID = (int)((DataRowView)e.Item.DataItem)["taxonomy_species_id"];
                string geoStatus = ((DataRowView)e.Item.DataItem)["geography_status_code"].ToString();
                Repeater r = (Repeater)sender;

                Repeater rptDistributionRange = e.Item.FindControl("rptDistributionRange") as Repeater;
                Label lblNote = (Label)rptDistributionRange.Parent.FindControl("lblGeoNote");

                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    DataTable dt = sd.GetData("web_taxonomyspecies_distribution_continent", ":taxonomyid=" + taxonomyID + ";:geostatuscode=" + geoStatus, 0, 0).Tables["web_taxonomyspecies_distribution_continent"];

                    if (dt.Rows.Count == 1 && dt.Rows[0][0].ToString() == "")
                    {
                        
                        DataTable dt1 = sd.GetData("web_taxonomyspecies_distribution_note", ":taxonomyid=" + taxonomyID + ";:geostatuscode=" + geoStatus, 0, 0).Tables["web_taxonomyspecies_distribution_note"];
                        if (dt1.Rows.Count > 0)
                        {
                            lblNote.Visible = true;
                            lblNote.Text = "<b>.   </b>" + dt1.Rows[0]["note"].ToString();
                        }
                    }
                    else
                    {
                        lblNote.Visible = false;

                        rptDistributionRange.DataSource = dt;
                        rptDistributionRange.ItemDataBound += new RepeaterItemEventHandler(rptDistributionRange_SubcontinentItemDataBound);
                        rptDistributionRange.DataBind();
                    }
                }
            }
        }

        protected string DisplayNote(object note)
        {
            if (!String.IsNullOrEmpty(note.ToString()))
            {
                string note1 = note as string;

                //int i = note1.IndexOf("\\;");
                note1 = Regex.Split(note1, "\\;")[0];
                if (note1.Length > 1)
                {
                    if (note1.Substring(note1.Length -1, 1) == "\\" )
                        note1 = note1.Substring(0, note1.Length - 1);
                }

                return note1;

            }
            return "";
        }

        protected string DisplayCommentHead(object note)
        {
            if (!String.IsNullOrEmpty(note.ToString()))
            {
                string note1 = note as string;

                string[] comments = Regex.Split(note1, @"\\\;");

                if (comments.Length > 1)
                    return "Comments:";
                else
                    return "Comment:";
            }
            return "";
        }

        protected string DisplayComment(object note)
        {
            if (!String.IsNullOrEmpty(note.ToString()))
            {
                string note1 = note as string;

                string[] comments = Regex.Split(note1, @"\\\;");

                if (comments.Length > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul>");
                    foreach (string comment in comments)
                    {
                        sb.Append("<li>&#149 ").Append(comment).Append("</li>");
                    }
                    sb.Append("</ul>");
                    return sb.ToString();
                }
                else
                    return note1;
            }
            return "";
        }

        private void bindCheckOther(SecureData sd, int taxonomyID)
        {
            //DataTable dt = sd.GetData("web_taxonomyspecies_checkother", ":taxonomyid=" + taxonomyID, 0, 0).Tables["web_taxonomyspecies_checkother"];
            using (DataManager dm = sd.BeginProcessing(true, true))
            {
                DataTable dt = dm.Read(@"
                select tg.genus_name, ts.species_name, ts.subspecies_name, ts.variety_name, ts.name, tf.family_name 
                from taxonomy_genus tg join taxonomy_species ts on tg.taxonomy_genus_id = ts.taxonomy_genus_id
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                where ts.taxonomy_species_id = :taxonomyid",
                new DataParameters(":taxonomyid", taxonomyID ));

                if (dt.Rows.Count > 0)
                {

                    string genus_name = dt.Rows[0][0].ToString().Trim();
                    string species_name = dt.Rows[0][1].ToString().Trim();
                    string subspecies_name = dt.Rows[0][2].ToString().Trim();
                    string variety_name = dt.Rows[0][3].ToString().Trim();
                    string name = dt.Rows[0][4].ToString().Trim();
                    string family_name = dt.Rows[0][5].ToString().Trim();

                    string species_name2 = Regex.Replace(species_name, "um$", "", RegexOptions.IgnoreCase);
                    species_name2 = Regex.Replace(species_name2, "us$", "", RegexOptions.IgnoreCase);
                    species_name2 = Regex.Replace(species_name2, "a$", "", RegexOptions.IgnoreCase);

                    string name2 = Regex.Replace(name, " ", "%20");  // necessary ?
                    name2 = Regex.Replace(name2, "um$", "", RegexOptions.IgnoreCase);
                    name2 = Regex.Replace(name2, "us$", "", RegexOptions.IgnoreCase);
                    name2 = Regex.Replace(name2, "a$", "", RegexOptions.IgnoreCase);

                    DataTable otherDBs = new DataTable("otherDBs");

                    DataColumn pre = new DataColumn("otherPre", typeof(string));
                    otherDBs.Columns.Add(pre);

                    DataColumn link = new DataColumn("otherDBlink", typeof(string));
                    otherDBs.Columns.Add(link);

                    DataColumn description = new DataColumn("otherDB", typeof(string));
                    otherDBs.Columns.Add(description);

                    string olink = "";
                    string oname = "";

                    int cnt = 0;
                    // 1
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                where ts.taxonomy_species_id = ts.current_taxonomy_species_id
                and l.abbreviation  like 'F Eur%'   
                and ts.variety_name is null and ts.subvariety_name is null 
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        if (String.IsNullOrEmpty(subspecies_name))
                            olink = "<a href='http://193.62.154.38/cgi-bin/nph-readbtree.pl/feout?FAMILY_XREF=&GENUS_XREF=" + genus_name + "&SPECIES_XREF=" + species_name2 + "*&TAXON_NAME_XREF=&RANK=' target='_blank'> Flora Europaea</a>:";
                        else
                            olink = "<a href='http://193.62.154.38/cgi-bin/nph-readbtree.pl/feout?FAMILY_XREF=&GENUS_XREF=" + genus_name + "&SPECIES_XREF=" + species_name2 + "*&TAXON_NAME_XREF=" + subspecies_name + "&RANK=subsp.' target='_blank'> Flora Europaea</a>:";
                        oname = "Database of European Plants (ESFEDS)";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 2
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
			    select count(ts.taxonomy_species_id) from taxonomy_species ts
                join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography_region_map grm on tgm.geography_id = grm.geography_id
                join region r on grm.region_id = r.region_id 
                where tf.family_name = 'Asteraceae'
                and r.continent = 'Europe'               
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        if (String.IsNullOrEmpty(subspecies_name))
                            olink = "<a href='http://ww2.bgbm.org/EuroPlusMed/results.asp?name=" + genus_name + " " + species_name2 + "*' target='_blank'> Euro+Med Plantbase</a>:";
                        else
                            olink = "<a href='http://ww2.bgbm.org/EuroPlusMed/results.asp?name=" + genus_name + " " + species_name2 + "*" + " subsp. " + subspecies_name + "' target='_blank'> Euro+Med Plantbase</a>:";
                        oname = "Information Resource for Euro-Mediterranean Plant Diversity";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 3
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                where ts.taxonomy_species_id = ts.current_taxonomy_species_id
                and l.abbreviation = 'F NAmer' and tf.family_name <> 'Poaceae'
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://efloras.org/browse.aspx?flora_id=0&name_str=" + name2 + "%&submit=Search' target='_blank'> Flora of North America</a>:";
                        oname = "Collaborative Floristic Effort of North American Botanists";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 4
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
			    select count(ts.taxonomy_species_id) from taxonomy_species ts
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                where ts.subvariety_name is null
                and ts.forma_name is null
                and ts.taxonomy_species_id = ts.current_taxonomy_species_id 
                and  (g.country_code in ('USA', 'CAN','PRI', '021') 
                or (tgm.note like '%Canada%' or tgm.note like '%United States%' or tgm.note like '%North America%' or tgm.note like '%temperate%'))
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        string name3 = Regex.Replace(name2, "subsp.", "ssp.", RegexOptions.IgnoreCase);
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://plants.usda.gov/java/nameSearch?keywordquery=" + name3 + "&mode=sciname&submit.x=12&submit.y=8' target='_blank'> PLANTS</a>:";
                        oname = "USDA-NRCS Database of Plants of the United States and its Territories";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 5
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
			    select count(ts.taxonomy_species_id) from taxonomy_species ts
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                where ts.subspecies_name is null
                and ts.variety_name is null
                and ts.subvariety_name is null
                and ts.forma_name is null
                and ts.taxonomy_species_id = ts.current_taxonomy_species_id 
                and  (g.country_code in ('USA', 'CAN', '021')   
                or (tgm.note like '%Canada%' or tgm.note like '%United States%' or tgm.note like '%North America%'))
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.bonap.org/BONAPmaps2010/" + genus_name + ".html' target='_blank'> BONAP</a>";
                        oname = "<i>North American Plant Atlas</i> of the <a href=\"http://www.bonap.org/\" target=\"_blank\">Biota of North America Program</a>:";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 6
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
			    select count(ts.taxonomy_species_id) from taxonomy_species ts
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                join geography_region_map grm on tgm.geography_id = grm.geography_id
                join region r on grm.region_id = r.region_id 
                where (g.country_code = 'BRA'
                and g.adm1 in ('Parana','Santa Catarina','Rio Grande do Sul')  
                or r.subcontinent = 'Southern South America')  
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www2.darwin.edu.ar/Proyectos/FloraArgentina/Generos.asp?genus=" + genus_name + "' target='_blank'> Flora del Conosur</a>:";
                        oname = "Cat&aacute;logo de las Plantas Vasculares del Conosur";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 7
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                where ts.taxonomy_species_id = ts.current_taxonomy_species_id
                and l.abbreviation = 'F ChinaEng'                 
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://efloras.org/browse.aspx?flora_id=0&name_str=" + name2 + "%&btnSearch=Search' target='_blank'> Flora of China</a>:";
                        oname = "Online version from Harvard University";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 8
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                where ts.subspecies_name is null
                and ts.variety_name is null
                and ts.subvariety_name is null
                and ts.forma_name is null
                and  g.country_code = 'AUS'                 
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.chah.gov.au/avh/public_query.jsp' target='_blank'> AVH</a>:";
                        oname = "Australia's Virtual Herbarium";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 9
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                where ts.taxonomy_species_id = ts.current_taxonomy_species_id
                and l.abbreviation = 'F Aust'
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.anbg.gov.au/abrs/online-resources/flora/stddisplay.xsql?sn_sp=" + species_name2 + "&sn_gen=" + genus_name + "&sn_sp=' target='_blank'> ABRS</a>:";
                        oname = "Australian Biological Resources Study <i>Flora of Australia</i> online";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 10
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                join geography_region_map grm on tgm.geography_id = grm.geography_id
                join region r on grm.region_id = r.region_id 
                where r.subcontinent = 'Southern Africa'
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://sibis.sanbi.org/faces/SearchSpecies/Search.jsp?' target='_blank'> SIBIS</a>:";
                        oname = "South African National Biodiversity Institute's (SANBI) Integrated Biodiversity System";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 11
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                where g.country_code = 'ZWE' 
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.zimbabweflora.co.zw/speciesdata/utilities/utility-species-search-binomial.php?genus=" + genus_name + "&species=" + species_name + "' target='_blank'> On-line Flora of Zimbabwe</a>:";
                        oname = "";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 12
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                where l.abbreviation = 'New World Fruits'
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.bioversityinternational.org/databases/new_world_fruits_database/search.html' target='_blank'> New World Fruits Database</a>:";
                        oname = "Online database from Bioversity International";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 13
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography_region_map grm on tgm.geography_id = grm.geography_id
                join region r on grm.region_id = r.region_id 
                where ((r.continent = 'Asia-Temperate' and r.subcontinent = 'China')
                or continent in ('Northern America', 'Southern America'))
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        if (!String.IsNullOrEmpty(subspecies_name))
                            olink = "<a href='http://www.tropicos.org/NameSearch.aspx?name=" + genus_name + "+" + species_name2 + "+subsp+" + subspecies_name + "' target='_blank'> TROPICOS</a>:";
                        else if (!String.IsNullOrEmpty(variety_name))
                            olink = "<a href='http://www.tropicos.org/NameSearch.aspx?name=" + genus_name + "+" + species_name2 + "+var+" + variety_name + "' target='_blank'> TROPICOS</a>:";
                        else
                            olink = "<a href='http://www.tropicos.org/NameSearch.aspx?name=" + genus_name + "+" + species_name2 + "' target='_blank'> TROPICOS</a>:";
                        oname = "Nomenclatural and Specimen Database of the Missouri Botanical Garden";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 14
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                where tf.family_name = 'Fabaceae'
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.ildis.org/LegumeWeb?sciname=" + genus_name + "+" + species_name + "' target='_blank'> ILDIS</a>:";
                        oname = "International Legume Database & Information Service";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 15
                    if (genus_name == "Solanum")
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.nhm.ac.uk/research-curation/research/projects/solanaceaesource/taxonomy/list.jsp?searchTerm=" + genus_name + "+" + species_name2 + "' target='_blank'> Solanaceae Source</a>:";
                        oname = "A global taxonomic resource for the nightshade family";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 16
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography_region_map grm on tgm.geography_id = grm.geography_id
                join region r on grm.region_id = r.region_id 
                where tf.family_name = 'Poaceae'
                and (r.continent in ('Northern America','Southern America') or tgm.note like '%natzd.%')
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        if (!String.IsNullOrEmpty(subspecies_name))
                            olink = "<a href='http://www.tropicos.org/NameSearch.aspx?projectid=10&name=" + genus_name + "+" + species_name2 + "+subsp+" + subspecies_name + "' target='_blank'> CNWG</a>:";
                        else if (!String.IsNullOrEmpty(variety_name))
                            olink = "<a href='http://www.tropicos.org/NameSearch.aspx?projectid=10&name=" + genus_name + "+" + species_name2 + "+var+" + variety_name + "' target='_blank'> CNWG</a>:";
                        else
                            olink = "<a href='http://www.tropicos.org/NameSearch.aspx?projectid=10&name=" + genus_name + "+" + species_name2 + "' target='_blank'> CNWG</a>:";
                        oname = "Catalogue of New World Grasses Searchable Database";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 17
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
				join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
                join taxonomy_geography_map tgm on ts.taxonomy_species_id = tgm.taxonomy_species_id
                join geography g on tgm.geography_id = g.geography_id
                where ts.subvariety_name is null
                and ts.forma_name is null
                and ts.taxonomy_species_id = ts.current_taxonomy_species_id 
                and tf.family_name = 'Poaceae'
                and  (g.country_code in ('USA', 'CAN')   
                or  (tgm.geography_id is null 
                and (tgm.note like '%Canada%' or tgm.note like '%United States%' or tgm.note like '%North America%')))
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        string name3 = Regex.Replace(name, @"\s+", "_");
                        name3 = Regex.Replace(name3, "%20", "_");
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.herbarium.usu.edu/webmanual/info.asp?name=" + name3 + "&type=map' target='_blank'> Grass Manual on the Web</a>:";
                        oname = "Manual of Grasses for North America North of Mexico";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 18
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
				join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
				where ts.taxonomy_species_id = ts.current_taxonomy_species_id 
				and tf.family_name = 'Poaceae'
				and ts.subspecies_name is null and  ts.variety_name is null and  ts.subvariety_name is null and  ts.forma_name is null
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://www.kew.org/data/grasses-db/results.htm?cx=008627686291448380647%3Atswoca79bsm&q=" + name + "&sa=Search&cof=FORID%3A11#139' target='_blank'> World Grass Species-Descriptions</a>:";
                        oname = "Morphological species description from Royal Botanic Gardens, Kew";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 19
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
				join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join taxonomy_family tf on tg.taxonomy_family_id = tf.taxonomy_family_id
				where ts.taxonomy_species_id = ts.current_taxonomy_species_id 
				and ts.verifier_cooperator_id is not null
				and tg.is_hybrid = 'N'
				and tf.note like 'gymnosperm family%' 
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        string lnk1 = family_name.Substring(0, 2).ToLower() + "/";
                        string lnk2 = genus_name.Substring(0, 2).ToLower() + "/";
                        string lnk3 = species_name + ".html";

                        if (genus_name == "Araucaria" || genus_name == "Agathis" || genus_name == "Wollemia")
                        {
                            DataRow row = otherDBs.NewRow();
                            olink = "<a href='http://www.conifers.org/" + lnk1 + lnk2 + lnk3 + "' target='_blank'> Gymnosperm Database</a>";
                            oname = "of Christopher J. Earle";
                            row["otherPre"] = "";
                            row["otherDBlink"] = olink;
                            row["otherDB"] = oname;
                            otherDBs.Rows.Add(row);
                        }
                    }

                    // 20
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join taxonomy_use tu on ts.taxonomy_species_id = tu.taxonomy_species_id
		        where tu.usage_type != 'ornamental'
		        and tu.economic_usage_code not in ('CPC','FWT','FWE','CITESI','CITESII','CITESIII','GENETIC','POISON','ALTHOST','BEE','WEED')                
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://mansfeld.ipk-gatersleben.de/pls/htmldb_pgrc/f?p=185:45:0::NO::P7_BOTNAME:" + genus_name + "%20" + species_name2 + "%' target='_blank'> Mansfeld</a>:";
                        oname = "Mansfeld's World Databas of Agricultural and Horticultural Crops";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 21
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(ts.taxonomy_species_id) from taxonomy_species ts  
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                where ts.taxonomy_species_id = ts.current_taxonomy_species_id
                and l.abbreviation = 'PROTABASE'
                and ts.taxonomy_species_id = :taxonomyid",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='http://database.prota.org/search.htm" + "' target='_blank'> PROTABASE</a>:";
                        oname = "Plant Resources of Tropical Africa's (PROTA's) online resource";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 22
                    cnt = Toolkit.ToInt32(dm.ReadValue(@"
                select count(tu.taxonomy_species_id) from taxonomy_use tu 
	            where tu.usage_type like '%ornamental%' 
	            and (exists (select ts.taxonomy_species_id  from taxonomy_species ts
                join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                join literature l on c.literature_id = l.literature_id
                where l.abbreviation = 'ICRA'
                and ts.taxonomy_species_id = :taxonomyid)
                or exists (select ts.taxonomy_species_id  from taxonomy_species ts
				join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id 
                join citation c on tg.taxonomy_genus_id = c.taxonomy_genus_id 
                join literature l on c.literature_id = l.literature_id
                where l.abbreviation = 'ICRA'   
                and ts.taxonomy_species_id = :taxonomyid))",
                    new DataParameters(":taxonomyid", taxonomyID)).ToString(), 0);
                    if (cnt > 0)
                    {
                        DataTable dt2 = dm.Read(@"
                        select l.abbreviation, c.note, l.reference_title from taxonomy_species ts
                        join citation c on ts.taxonomy_species_id = c.taxonomy_species_id 
                        join literature l on c.literature_id = l.literature_id
                        where l.abbreviation = 'ICRA' 
                        and ts.taxonomy_species_id = :taxonomyid",
                            new DataParameters(":taxonomyid", taxonomyID));

                        string abbr = "";
                        string shortcmt = "";
                        string reftitle = "";
                        if (dt2.Rows.Count > 0)
                        {
                            abbr = dt2.Rows[0][0].ToString().Trim();
                            shortcmt = dt2.Rows[0][1].ToString().Trim();
                            reftitle = dt2.Rows[0][2].ToString().Trim();
                        }

                        if (shortcmt == "")
                        {
                            dt2 = dm.Read(@"
                            select l.abbreviation, c.note, l.reference_title   from taxonomy_species ts
                            join taxonomy_genus tg on ts.taxonomy_genus_id = tg.taxonomy_genus_id
                            join citation c on tg.taxonomy_genus_id = c.taxonomy_genus_id  
                            join literature l on c.literature_id = l.literature_id
                            where l.abbreviation = 'ICRA' 
                            and ts.taxonomy_species_id = :taxonomyid",
                                new DataParameters(":taxonomyid", taxonomyID));

                            if (dt2.Rows.Count > 0)
                            {
                                abbr = dt2.Rows[0][0].ToString().Trim();
                                shortcmt = dt2.Rows[0][1].ToString().Trim();
                                reftitle = dt2.Rows[0][2].ToString().Trim();
                            }
                        }

                        //shortcmt = shortcmt.Replace("http://", "");

                        DataRow row = otherDBs.NewRow();
                        olink = "<a href='" + shortcmt + "' target='_blank'> ICRA</a>:";
                        oname = reftitle + " for <i> " + name + "</i> cultivars";
                        row["otherPre"] = "";
                        row["otherDBlink"] = olink;
                        row["otherDB"] = oname;
                        otherDBs.Rows.Add(row);
                    }

                    // 23
                    DataRow row2 = otherDBs.NewRow();
                    olink = "<a href='http://epic.kew.org/searchepic/summaryquery.do?scientificName=" + genus_name + "+" + species_name2 + "*&searchAll=true' target='_blank'> ePIC</a>:";
                    oname = "Electronic Plant Information Centre of Royal Botanic Gardens, Kew";
                    row2["otherPre"] = "";
                    row2["otherDBlink"] = olink;
                    row2["otherDB"] = oname;
                    otherDBs.Rows.Add(row2);

                    // 24
                    row2 = otherDBs.NewRow();
                    oname = "<a href='http://agricola.nal.usda.gov/cgi-bin/Pwebrecon.cgi?DB=local&CNT=25&Search_Arg=" + name2 + "&Search_Code=GKEY&STARTDB=AGRIDB' target='_blank'> Article Citation Database </a> or "
                          + "<a href='http://agricola.nal.usda.gov/cgi-bin/Pwebrecon.cgi?DB=local&CNT=25&Search_Arg=" + name2 + "&Search_Code=GKEY' target='_blank'> NAL Catalog </a>  of USDA's National Agricultural Library";
                    row2["otherPre"] = "<b>AGRICOLA:</b>";
                    row2["otherDBlink"] = "";
                    row2["otherDB"] = oname;
                    otherDBs.Rows.Add(row2);

                    // 25
                    string name25 = Regex.Replace(name, @"\s+", " ");
                    name25 = name25.Replace(" ", " AND ");

                    row2 = otherDBs.NewRow();
                    olink = "<a href='http://www.ncbi.nlm.nih.gov/gquery/gquery.fcgi?term=" + name25 + "*' target='_blank'> Entrez</a>:";
                    oname = "NCBI's search engine for PubMed citations, GenBank sequences, etc.";
                    row2["otherPre"] = "";
                    row2["otherDBlink"] = olink;
                    row2["otherDB"] = oname;
                    otherDBs.Rows.Add(row2);

                    rptCheckOther.DataSource = otherDBs;
                    rptCheckOther.DataBind();
                }
            }
        }

    }
}