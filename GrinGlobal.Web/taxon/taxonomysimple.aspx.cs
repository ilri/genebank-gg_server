using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Business;
using System.Data;


namespace GrinGlobal.Web.taxon
{
    public partial class taxonomysimple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlSearch.Visible = true;
                pnlResult.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string s = txtSearch.Text;
            searchSimple(s);

        }

        protected void btnSearch2_Click(object sender, EventArgs e)
        {
            string s = txtSearch2.Text;
            searchSimple(s);
        }

        private void searchSimple(string search)
        {
            string s = search.Trim().Replace('*', '%');
            s = Utils.Sanitize(s);
            string sqlbase = "";
            string sql = "";
            DataTable dt = null;
            string display = "";

            if (!string.IsNullOrEmpty(search))
            {
                try
                {
                    using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        using (DataManager dm = sd.BeginProcessing(true,true))
                        {
                            // noman number
                            int tid;
                            if (int.TryParse(s, out tid))
                            {
                                sql = "select taxonomy_species_id from taxonomy_species where taxonomy_species_id = :tid";
                                dt = dm.Read(sql, new DataParameters(":tid", tid, DbType.Int32));
                                if (dt.Rows.Count == 1)
                                {
                                    //display = "(for the query: <b>taxon number</b> = " + tid + ")";
                                    //Server.Transfer("~/taxonomydetail.aspx?id=" + tid);
                                    Response.Redirect("~/taxonomydetail.aspx?id=" + tid);
                                }
                            }
                            else
                            {
                                // taxon name
                                sqlbase = @"select distinct t.taxonomy_species_id as tid, t.current_taxonomy_species_id as tcid, t1.taxonomy_species_id as tid1,
                                        t.name as name, t.name_authority as author, t1.name as name1, t1.name_authority as author1, t1.synonym_code
                                        from taxonomy_species t join taxonomy_species t1 on t1.current_taxonomy_species_id = t.taxonomy_species_id ";

                                bool hasLike = s.IndexOf('%') > -1;

                                if (hasLike)
                                {
                                    //sql = sqlbase + "where t1.name like '" + s + "' or replace(t1.name, ' x ', ' ') like '" + s + "'";
                                    sql = sqlbase + "where t1.name like :s or replace(t1.name, ' x ', ' ') like :s";
                                }
                                else
                                {
                                    //sql = sqlbase + "where t1.name = '" + s + "' or replace(t1.name, ' x ', ' ') = '" + s + "'";
                                    sql = sqlbase + "where t1.name = :s or replace(t1.name, ' x ', ' ') = :s"; 
                                }
                                sql = sql + " order by name1";
                                //dt = dm.Read(sql);
                                dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                if (dt.Rows.Count == 1)
                                    Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                else if (dt.Rows.Count > 0)
                                {
                                    display = displayData(search, dt, "taxon name");
                                }
                                else
                                {
                                    // genus name
                                    if (hasLike)
                                    {
                                        sql = sqlbase + "where t1.taxonomy_genus_id in (select taxonomy_genus_id from taxonomy_genus where genus_name like :s)";
                                    }
                                    else
                                    {
                                        sql = sqlbase + "where t1.taxonomy_genus_id in (select taxonomy_genus_id from taxonomy_genus where genus_name = :s)";
                                    }
                                    sql = sql + " order by name1";
                                    dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                    if (dt.Rows.Count == 1)
                                        Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                    else if (dt.Rows.Count > 0)
                                    {
                                        display = displayData(search, dt, "genus");
                                    }
                                    else
                                    {
                                        // common name
                                        string scomm = s.Replace(" ", "").Replace("-", "");
                                        scomm = Utils.Sanitize(scomm);
                                        if (hasLike)
                                        {
                                            sql = sqlbase + "where t1.taxonomy_species_id in (select taxonomy_species_id from taxonomy_common_name where simplified_name like :s)";
                                        }
                                        else
                                        {
                                            sql = sqlbase + "where t1.taxonomy_species_id in (select taxonomy_species_id from taxonomy_common_name where simplified_name = :s)";
                                        }
                                        sql = sql + " order by name1";
                                        dt = dm.Read(sql, new DataParameters(":s", scomm, DbType.String));
                                        if (dt.Rows.Count == 1)
                                            Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                        else if (dt.Rows.Count > 0)
                                        {
                                            display = displayData(search, dt, "common name");
                                        }
                                        else
                                        {
                                            // family name
                                            if (s.IndexOf("aceae") > -1)
                                            {
                                                if (hasLike)
                                                {
                                                    sql = sqlbase + "where t1.taxonomy_genus_id  in (select g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id and f.family_name like :s)";
                                                }
                                                else
                                                {
                                                    sql = sqlbase + "where t1.taxonomy_genus_id  in (select g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id and f.family_name = :s)";
                                                }
                                                sql = sql + " order by name1";
                                                dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                                if (dt.Rows.Count == 1)
                                                    Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                                else if (dt.Rows.Count > 0)
                                                {
                                                    display = displayData(search, dt, "family");
                                                }
                                            }
                                            else if (s.IndexOf("ae") > -1)
                                            {
                                                if (hasLike)
                                                {
                                                    sql = sqlbase + "where t1.taxonomy_genus_id  in (select g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id and f.alternate_name like :s";
                                                }
                                                else
                                                {
                                                    sql = sqlbase + "where t1.taxonomy_genus_id  in (select g.taxonomy_genus_id from taxonomy_family f join taxonomy_genus g on f.taxonomy_family_id = g.taxonomy_family_id and f.alternate_name = :s";
                                                }
                                                sql = sql + " order by name1";
                                                dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                                if (dt.Rows.Count == 1)
                                                    Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                                else if (dt.Rows.Count > 0)
                                                {
                                                    display = displayData(search, dt, "family");
                                                }
                                                else
                                                {
                                                    // country name
                                                    if (hasLike)
                                                    {
                                                        sql = sqlbase + @"where t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id join code_value cv on g.country_code = cv.value 
                                                                    join code_value_lang cvl on cv.code_value_id = cvl.code_value_id where cvl.sys_lang_id = " + sd.LanguageID +
                                                                        @" and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' and g.adm1 is null and g.adm2 is null and g.adm3 is null and g.adm4 is null
                                                                    and country_code not like '%[0-9]%' and cvl.title like :s)";
                                                    }
                                                    else
                                                    {
                                                        sql = sqlbase + @"where t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id join code_value cv on g.country_code = cv.value 
                                                                    join code_value_lang cvl on cv.code_value_id = cvl.code_value_id where cvl.sys_lang_id = " + sd.LanguageID +
                                                                        @" and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' and g.adm1 is null and g.adm2 is null and g.adm3 is null and g.adm4 is null
                                                                    and country_code not like '%[0-9]%' and cvl.title = :s)";
                                                    }
                                                    sql = sql + " order by name1";
                                                    dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                                    if (dt.Rows.Count == 1)
                                                        Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                                    else if (dt.Rows.Count > 0)
                                                    {
                                                        display = displayData(search, dt, "country");
                                                    }
                                                    else
                                                    {
                                                        //display = "No species in GRIN match your query: (" + search + ")";
                                                        lblCriteria.Text = "";
                                                        rptResult.DataSource = null;
                                                        rptResult.DataBind();
                                                        string noresult = "No data found matching your criteria. ";
                                                        Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                // country name
                                                if (hasLike)
                                                {
                                                    sql = sqlbase + @"where t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id join code_value cv on g.country_code = cv.value 
                                                                    join code_value_lang cvl on cv.code_value_id = cvl.code_value_id where cvl.sys_lang_id = " + sd.LanguageID + 
                                                                    @" and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' and g.adm1 is null and g.adm2 is null and g.adm3 is null and g.adm4 is null
                                                                    and country_code not like '%[0-9]%' and cvl.title like :s)";
                                                }
                                                else
                                                {
                                                    sql = sqlbase + @"where t1.taxonomy_species_id in (select distinct tgm.taxonomy_species_id from taxonomy_geography_map tgm join geography g on tgm.geography_id = g.geography_id join code_value cv on g.country_code = cv.value 
                                                                    join code_value_lang cvl on cv.code_value_id = cvl.code_value_id where cvl.sys_lang_id = " + sd.LanguageID + 
                                                                    @" and cv.group_name = 'GEOGRAPHY_COUNTRY_CODE' and g.adm1 is null and g.adm2 is null and g.adm3 is null and g.adm4 is null
                                                                    and country_code not like '%[0-9]%' and cvl.title = :s)";
                                                }
                                                sql = sql + " order by name1";
                                                dt = dm.Read(sql, new DataParameters(":s", s, DbType.String));
                                                if (dt.Rows.Count == 1)
                                                    Response.Redirect("~/taxonomydetail.aspx?id=" + dt.Rows[0]["tid"].ToString());
                                                else if (dt.Rows.Count > 0)
                                                {
                                                    display = displayData(search, dt, "country");
                                                }
                                                else
                                                {
                                                    //display = "No species in GRIN match your query: (" + search + ")";
                                                    lblCriteria.Text = "";
                                                    rptResult.DataSource = null;
                                                    rptResult.DataBind();
                                                    string noresult = "No data found matching your criteria. ";
                                                    Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));
                                                 }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                    //txtSearch2.Text = "New Search";
                    //lblCriteria.Text = display;
                    //lblCriteria.Focus();
                }
                catch (Exception e)
                {
                    string err = e.Message;
                    lblCriteria.Text = "";
                    rptResult.DataSource = null;
                    rptResult.DataBind();
                    string noresult = "No data found matching your criteria. ";
                    Master.ShowError(Page.GetDisplayMember("Search", noresult + "doSearch{noCriteria}", noresult + "Please enter search string or other search criteria and try again."));

                }
            }
        }

        //private string displayData(string search, DataTable dt, string level)
        //{
        //    pnlSearch.Visible = false;
        //    pnlResult.Visible = true;
        //    txtSearch2.Text = "New Search";

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
        //            dr["linktext"] = sidx + ". <a href='../taxonomydetail.aspx?id=" + dr["tid1"] + "'>" + sname.Trim() + " (=" + name.Trim() + ")</a>";

        //        //dr["linktext"] = inx + ". <a href='../taxonomydetail.aspx?id=" + dr["tid"] + "'>" + "<i>" + dr["name"].ToString().Replace(" f. ", " </i>f.<i> ") + "</i> " + dr["species_authority"] + "</a>";
        //        idx++;
        //    }

        //    rptResult.DataSource = dt;
        //    rptResult.DataBind();
        //    lblCriteria.Text = "(for the query: <b>" + level + "</b> = <i>" + search + "</i>)";
        //    lblCriteria.Focus();

        //    return "(for the query: <b>" + level + "</b> = <i>" + search + "</i>)";
        //}

        private string displayData(string search, DataTable dt, string level)
        {
            pnlSearch.Visible = false;
            pnlResult.Visible = true;
            txtSearch2.Text = "New Search";

            string name = "";
            string sname = "";

            dt.Columns.Add("linktext", typeof(string));
            dt.Columns.Add("sorttext", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                name = "<b><i>" + dr["name"].ToString() + "</i> " + dr["author"] + "</b>";
                sname = "<i>" + dr["name1"].ToString() + "</i> " + dr["author1"];

                if (dr["tid"].ToString() == dr["tid1"].ToString())
                    dr["linktext"] = TaxonUtil.ItalicTaxon(" <a href='../taxonomydetail.aspx?id=" + dr["tid"] + "'>" + name.Trim() + "</a>");
                else
                    dr["linktext"] = TaxonUtil.ItalicTaxon(" <a href='../taxonomydetail.aspx?id=" + dr["tid1"] + "'>" + sname.Trim() + " (=" + name.Trim() + ")</a>");

                //dr["sorttext"] = dr["name1"].ToString().Replace(" subsp. ", " ").Replace(" var. ", " ");
                dr["sorttext"] = TaxonUtil.RemoveTaxon(dr["name1"].ToString());
            }
            dt.DefaultView.Sort = "sorttext";

            rptResult.DataSource = dt;
            rptResult.DataBind();
            lblCriteria.Text = "(for the query: <b>" + level + "</b> = <i>" + search + "</i>)";
            lblCriteria.Focus();

            return "(for the query: <b>" + level + "</b> = <i>" + search + "</i>)";
        }
    }
}
