using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using System.Data;
using GrinGlobal.Core;
using GrinGlobal.Core.Xml;
using System.Text;

namespace GrinGlobal.Web {
    public partial class Browse : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
        }


        public string OutputTreeViewHtml() {

            bool orderByAccessionCount = Request["orderby"] == "accessioncount" || rblOrderBy.SelectedValue == "accessioncount";

            string viewBy = rblViewBy.SelectedValue;


            string cacheKey = "BrowseBy" + viewBy + "OrderBy" + (orderByAccessionCount ? "Count" : "Name");

            CacheManager cm = CacheManager.Get("Taxonomy");
            if (cm[cacheKey] == null){
                Document doc = null;
                switch(viewBy){
                    case "family":
                    default:
                        doc = createXmlByFamily(orderByAccessionCount);
                        break;
                    case "genus":
                        doc = createXmlByGenus(orderByAccessionCount);
                        break;
                    case "species":
                        doc = createXmlBySpecies(orderByAccessionCount);
                        break;
                }

                StringBuilder sb = new StringBuilder("<div class='treeList'><ul>");
                foreach (Node ndFamily in doc.Root.Nodes) {
                    if (ndFamily.NodeName == "Family") {
                        if (viewBy == "family") {
                            string familyID = ndFamily.Attributes.GetValue("ID", "");
                            sb.AppendLine("<li><a href='#' class='jsanchor' onclick='javascript:return toggleItem(\"family" + familyID + "\");'><img src='images/open.gif' id='family" + familyID + "Img' />" + ndFamily.Attributes.GetValue("DisplayText", "-") + "</a><br />");
                            if (ndFamily.Nodes.Count > 0) {
                                sb.AppendLine("<ul style='display:none' id='family" + familyID + "'>");
                            }
                        }
                        foreach (Node ndGenus in ndFamily.Nodes) {
                            if (ndGenus.NodeName == "Genus") {
                                if (viewBy == "family" || viewBy == "genus") {
                                    string genusID = ndGenus.Attributes.GetValue("ID", "");
                                    sb.AppendLine("<li><a href='#' class='jsanchor' onclick='javascript:return toggleItem(\"genus" + genusID + "\");'><img src='images/open.gif' id='genus" + genusID + "Img' />" + ndGenus.Attributes.GetValue("DisplayText", "-") + "</a><br />");
                                    if (ndGenus.Nodes.Count > 0) {
                                        sb.AppendLine("<ul style='display:none' id='genus" + genusID + "'>");
                                    }
                                }
                                foreach (Node ndSpecies in ndGenus.Nodes) {
                                    if (ndSpecies.NodeName == "Species") {
                                        string speciesID = ndSpecies.Attributes.GetValue("ID", "");
                                        string displayText = ndSpecies.Attributes.GetValue("DisplayText");
//                                        sb.AppendLine("<li><a href='#' onclick='javascript:document.getElementById(\"iframeSpecies\").src = \"Species.aspx?id=" + speciesID + "\";return false;'>" + ndSpecies.Attributes.GetValue("DisplayText", "-") + "</a></li>");
                                        sb.AppendLine("<li><a href='taxonomydetail.aspx?id=" + speciesID + "'>" + displayText + "</a></li>");
                                    }
                                }
                                if (ndGenus.Nodes.Count > 0) {
                                    if (viewBy == "family" || viewBy == "genus") {
                                        sb.AppendLine("</ul></li>");
                                    }
                                }
                            }
                        }
                        if (ndFamily.Nodes.Count > 0) {
                            if (viewBy == "family") {
                                sb.AppendLine("</ul></li>");
                            }
                        }
                    }
                }


                sb.Append("</ul></div>");

                cm[cacheKey] = sb.ToString();
            }

            return cm[cacheKey] as string;
        }


        private Document createXmlByFamily(bool orderByAccessionCount) {
            Document doc = new Document("TaxonomyInfo");
            using (var sd = UserManager.GetSecureData(true)) {
                using (var dm = sd.BeginProcessing(true, true)) {

                    // get all families + accession count(s)


                    var dtFamily = dm.Read(String.Format(@"
select 
	tf.taxonomy_family_id as value,
	tf.family_name,
	tf.family_name + ' ' + coalesce(tf.subfamily,'') + ' ' + coalesce(tf.tribe,'') + ' ' + coalesce(tf.subtribe,'') + ' (' +  replace(convert(nvarchar, convert(money, count(distinct a.accession_id)), 1), '.00', '') + ')' as display_text,
    COUNT(distinct a.accession_id) as accession_count
from 
	taxonomy_family tf left  join taxonomy_genus tg
		on tf.taxonomy_family_id = tg.taxonomy_family_id
	left  join taxonomy t
		on tg.taxonomy_genus_id = t.taxonomy_genus_id
	left  join accession a
		on t.taxonomy_id = a.taxonomy_id
group by 
    tf.taxonomy_family_id,
    tf.family_name,
    tf.subfamily,
    tf.tribe,
    tf.subtribe
order by
    {0}
    tf.family_name + ' ' + coalesce(tf.subfamily,'') + ' ' + coalesce(tf.tribe,'') + ' ' + coalesce(tf.subtribe,'')",
        orderByAccessionCount ?
        "count(distinct a.accession_id) desc," : ""));


                    foreach (DataRow drFamily in dtFamily.Rows) {

                        // output family-level info
                        var ndFamily = new Node { NodeName = "Family" };
                        ndFamily.Attributes.SetValue("ID", drFamily["value"].ToString());
                        ndFamily.Attributes.SetValue("Name", drFamily["family_name"].ToString());
                        ndFamily.Attributes.SetValue("DisplayText", drFamily["display_text"].ToString());
                        ndFamily.Attributes.SetValue("AccessionCount", drFamily["accession_count"].ToString());
                        doc.Root.Nodes.Add(ndFamily);

                        // get genus-level info
                        var dtGenus = dm.Read(String.Format(@"
select 
    tg.taxonomy_genus_id as value,
    tg.genus_name,
	tg.genus_name + ' ' + coalesce(tg.subgenus_name, '') + ' ' + coalesce(tg.section_name, '') + ' ' + coalesce(tg.series_name, '') + ' ' + coalesce(tg.subseries_name,'') + ' ' + coalesce(tg.subsection_name, '') + ' (' + replace(convert(nvarchar, convert(money, count(distinct a.accession_id)), 1), '.00', '') + ')' as display_text,
    COUNT(distinct a.accession_id) as accession_count
from 
	taxonomy_genus tg left  join taxonomy t
		on tg.taxonomy_genus_id = t.taxonomy_genus_id
	left  join accession a
		on t.taxonomy_id = a.taxonomy_id
where
    tg.taxonomy_family_id = :familyid
group by 
    tg.taxonomy_genus_id,
	tg.genus_name,
    tg.subgenus_name,
    tg.section_name,
    tg.series_name,
    tg.subseries_name,
    tg.subsection_name
order by
    {0}
    tg.genus_name + ' ' + coalesce(tg.subgenus_name, '') + ' ' + coalesce(tg.section_name, '') + ' ' + coalesce(tg.series_name, '') + ' ' + coalesce(tg.subseries_name,'') + ' ' + coalesce(tg.subsection_name, '')",
        orderByAccessionCount ?
        "count(distinct a.accession_id) desc," : ""
), new DataParameters(":familyid", Toolkit.ToInt32(drFamily["value"], -1), DbType.Int32));


                        foreach (DataRow drGenus in dtGenus.Rows) {

                            // output family-level info
                            var ndGenus = new Node { NodeName = "Genus" };
                            ndGenus.Attributes.SetValue("ID", drGenus["value"].ToString());
                            ndGenus.Attributes.SetValue("Name", drGenus["genus_name"].ToString());
                            ndGenus.Attributes.SetValue("DisplayText", drGenus["display_text"].ToString());
                            ndGenus.Attributes.SetValue("AccessionCount", drGenus["accession_count"].ToString());
                            ndFamily.Nodes.Add(ndGenus);

                            var dtSpecies = dm.Read(String.Format(@"
select 
    t.taxonomy_id as value,
    t.name,
	t.name + ' (' + replace(convert(nvarchar, convert(money, count(distinct a.accession_id)), 1), '.00', '') + ')' as display_text,
    COUNT(distinct a.accession_id) as accession_count
from 
	taxonomy t left  join accession a
		on t.taxonomy_id = a.taxonomy_id
where
    t.taxonomy_genus_id = :genusid
group by 
	t.taxonomy_id,
	t.name
order by
    {0}
    t.name
", orderByAccessionCount ? "count(distinct a.accession_id) desc," : ""
), new DataParameters(":genusid", Toolkit.ToInt32(drGenus["value"], -1), DbType.Int32));
    

                            foreach (DataRow drSpecies in dtSpecies.Rows) {
                                // output family-level info
                                var ndSpecies = new Node { NodeName = "Species" };
                                ndSpecies.Attributes.SetValue("ID", drSpecies["value"].ToString());
                                ndSpecies.Attributes.SetValue("Name", drSpecies["name"].ToString());
                                ndSpecies.Attributes.SetValue("DisplayText", drSpecies["display_text"].ToString());
                                ndSpecies.Attributes.SetValue("AccessionCount", drSpecies["accession_count"].ToString());
                                ndGenus.Nodes.Add(ndSpecies);
                            }
                        }

                    }
                }
            }
            return doc;

        }

        private Document createXmlByGenus(bool orderByAccessionCount) {
            Document doc = new Document("TaxonomyInfo");
            using (var sd = UserManager.GetSecureData(true)) {
                using (var dm = sd.BeginProcessing(true, true)) {

                    Node ndAllFamilies = new Node("Family");
                    doc.Root.Nodes.Add(ndAllFamilies);

                    // get genus-level info
                    var dtGenus = dm.Read(String.Format(@"
select 
tg.taxonomy_genus_id as value,
tg.genus_name,
tg.genus_name + ' ' + coalesce(tg.subgenus_name, '') + ' ' + coalesce(tg.section_name, '') + ' ' + coalesce(tg.series_name, '') + ' ' + coalesce(tg.subseries_name,'') + ' ' + coalesce(tg.subsection_name, '') + ' (' + replace(convert(nvarchar, convert(money, count(distinct a.accession_id)), 1), '.00', '') + ')' as display_text,
COUNT(distinct a.accession_id) as accession_count
from 
taxonomy_genus tg left  join taxonomy t
	on tg.taxonomy_genus_id = t.taxonomy_genus_id
left  join accession a
	on t.taxonomy_id = a.taxonomy_id
group by 
tg.taxonomy_genus_id,
tg.genus_name,
tg.subgenus_name,
tg.section_name,
tg.series_name,
tg.subseries_name,
tg.subsection_name
order by
{0}
tg.genus_name + ' ' + coalesce(tg.subgenus_name, '') + ' ' + coalesce(tg.section_name, '') + ' ' + coalesce(tg.series_name, '') + ' ' + coalesce(tg.subseries_name,'') + ' ' + coalesce(tg.subsection_name, '')",
    orderByAccessionCount ?
    "count(distinct a.accession_id) desc," : ""
));


                    foreach (DataRow drGenus in dtGenus.Rows) {

                        // output family-level info
                        var ndGenus = new Node { NodeName = "Genus" };
                        ndGenus.Attributes.SetValue("ID", drGenus["value"].ToString());
                        ndGenus.Attributes.SetValue("Name", drGenus["genus_name"].ToString());
                        ndGenus.Attributes.SetValue("DisplayText", drGenus["display_text"].ToString());
                        ndGenus.Attributes.SetValue("AccessionCount", drGenus["accession_count"].ToString());
                        ndAllFamilies.Nodes.Add(ndGenus);

                        var dtSpecies = dm.Read(String.Format(@"
select 
t.taxonomy_id as value,
t.name,
t.name + ' (' + replace(convert(nvarchar, convert(money, count(distinct a.accession_id)), 1), '.00', '') + ')' as display_text,
COUNT(distinct a.accession_id) as accession_count
from 
taxonomy t left join accession a
	on t.taxonomy_id = a.taxonomy_id
where
t.taxonomy_genus_id = :genusid
group by 
t.taxonomy_id,
t.name
order by
{0}
t.name
", orderByAccessionCount ? "count(distinct a.accession_id) desc," : ""
), new DataParameters(":genusid", Toolkit.ToInt32(drGenus["value"], -1), DbType.Int32));


                        foreach (DataRow drSpecies in dtSpecies.Rows) {
                            // output family-level info
                            var ndSpecies = new Node { NodeName = "Species" };
                            ndSpecies.Attributes.SetValue("ID", drSpecies["value"].ToString());
                            ndSpecies.Attributes.SetValue("Name", drSpecies["name"].ToString());
                            ndSpecies.Attributes.SetValue("DisplayText", drSpecies["display_text"].ToString());
                            ndSpecies.Attributes.SetValue("AccessionCount", drSpecies["accession_count"].ToString());
                            ndGenus.Nodes.Add(ndSpecies);
                        }
                    }
                }
            }
            return doc;
        }


        private Document createXmlBySpecies(bool orderByAccessionCount) {
            Document doc = new Document("TaxonomyInfo");
            using (var sd = UserManager.GetSecureData(true)) {
                using (var dm = sd.BeginProcessing(true, true)) {

                    Node ndAllFamilies = new Node("Family");
                    doc.Root.Nodes.Add(ndAllFamilies);

                    Node ndAllGenus = new Node("Genus");
                    ndAllFamilies.Nodes.Add(ndAllGenus);


                    var dtSpecies = dm.Read(String.Format(@"
select 
    t.taxonomy_id as value,
    t.name,
    t.name + ' (' + replace(convert(nvarchar, convert(money, count(distinct a.accession_id)), 1), '.00', '') + ')' as display_text,
    COUNT(distinct a.accession_id) as accession_count
from 
    taxonomy t inner join accession a
	    on t.taxonomy_id = a.taxonomy_id
group by 
    t.taxonomy_id,
    t.name
order by
    {0}
    t.name
", orderByAccessionCount ? "count(distinct a.accession_id) desc," : ""
));


                    foreach (DataRow drSpecies in dtSpecies.Rows) {
                        // output family-level info
                        var ndSpecies = new Node { NodeName = "Species" };
                        ndSpecies.Attributes.SetValue("ID", drSpecies["value"].ToString());
                        ndSpecies.Attributes.SetValue("Name", drSpecies["name"].ToString());
                        ndSpecies.Attributes.SetValue("DisplayText", drSpecies["display_text"].ToString());
                        ndSpecies.Attributes.SetValue("AccessionCount", drSpecies["accession_count"].ToString());
                        ndAllGenus.Nodes.Add(ndSpecies);
                    }
                }
            }
            return doc;
        }

        protected void rblViewBy_SelectedIndexChanged(object sender, EventArgs e) {

        }

    }
}
