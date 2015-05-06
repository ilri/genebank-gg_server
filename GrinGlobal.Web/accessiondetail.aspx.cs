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
using System.Web.UI.HtmlControls;

namespace GrinGlobal.Web {
    public partial class AccessionDetail : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) 
        {
            try
            {
                int id = 0;
                if (Request.QueryString["id"] != null)
                {
                    id = Toolkit.ToInt32(Request.QueryString["id"], 0);
                    bindData(id);
                }
                else if (Request.QueryString["accid"] != null)
                {
                    string acc = Request.QueryString["accid"].ToString().Trim().Replace("%20", " ");
                    string[] accNum = acc.Split(' ');

                    string acc1 = "";
                    int acc2 = 0;
                    string acc3 = "";
                    string where = "";

                    for (int i = 0; i <= accNum.Length - 1; i++)
                    {
                        if (i == 0)
                        {
                            acc1 = accNum[i].Trim();
                            where = " accession_number_part1 = '" + acc1 + "'";
                        }
                        else if (i == 1)
                        {
                            acc2 = Toolkit.ToInt32(accNum[i], 0);
                            where += " and accession_number_part2 = " + acc2;
                        }
                        else
                        {
                            acc3 = accNum[2].Trim();
                            if (!String.IsNullOrEmpty(acc3)) where += " and accession_number_part3 = '" + acc3;
                        }
                    }

                    if (!String.IsNullOrEmpty(where))
                    {
                        using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                        {
                            using (DataManager dm = sd.BeginProcessing(true, true))
                            {
                                var dt = dm.Read(@"select accession_id from accession where " + where);
                                if (dt.Rows.Count >= 1)
                                {
                                    id = Toolkit.ToInt32(dt.Rows[0][0].ToString(), 0);
                                    bindData(id);
                                }
                            }
                        }
                    }
                }
                else if (Request.QueryString != null)
                {
                    if (Request.QueryString.Count == 1)
                    {
                        id = Toolkit.ToInt32(Request.QueryString.ToString(), 0);
                        if (id > 0) bindData(id);
                    }
                }

                if (id > 0) setPageTitle(this, id);
            }
            catch (Exception ex)
            { string msg = ex.Message; }

        }

        public string RemoveLeadingComma(object val)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                if (ret.StartsWith(","))
                {
                    if (ret.Length > 1)
                        ret = ret.Substring(2);
                    else
                        ret = "";
                }
            }
            return ret;
        }


        public string Join(string delimiter, params object[] values)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                {
                    if (values[i].ToString().Trim().Length > 0)
                    {
                        sb.Append(values[i].ToString()).Append(delimiter);
                    }
                }
            }
            if (sb.Length > delimiter.Length)
            {
                sb.Length -= delimiter.Length;
            }
            return sb.ToString();
        }


        private void bindData(int id) 
        {
            // when there's a bunch of different things to databind,
            // it's usually clearer to break them out.
            // for the mockup, let's just leave everything as DataManager.ExecRead

            ViewState["accession_id"] = id;

            // accession detail may not be allowed to displayed: is_web_visible
            bool display = true;
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    string visibility = dm.ReadValue(@"select is_web_visible from accession where accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();
                    if (visibility != "Y") display = false;
                 }
            }

            if (display)
            {
                bindHeader(id);
                
                bindAccessionSummary(id);

                bindAccessionNames(id);

                bindIntellectualPropertyRights(id);

                bindAvailability(id);

                bindAvailabilityNote(id);

                bindWebAvailabilityNote(id);

                bindNarrative(id);

                bindAnnotations(id);

                bindSourceHistory(id);

                bindCitations(id);

                bindPedigree(id);

                bindPathogen(id);

                bindQuarantine(id);

                bindObservation(id);

                bindVouchers(id);

                bindInventoryImage(id);

                bindFavorite(id);

                bindInventoryOther(id);

                bindActionNote(id);
            }
         }

        // for accession_id queries
        private DataTable getDataViewData(string dvName, int id) 
        {

            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                return sd.GetData(dvName, ":accessionid=" + id, 0, 0).Tables[dvName];
            }
        }

        // for other queries (accession_source)
        private DataTable getDataViewData(string dvName, string dataParams, int limit) 
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true))) 
            {
                return sd.GetData(dvName, dataParams, 0, limit).Tables[dvName];
            }
        }

        private void bindHeader(int id)
        {
            rptHeader.DataSource = getDataViewData("web_accessiondetail_header", id);
            rptHeader.DataBind();
        }

        private void bindAccessionSummary(int id)
        {
            bindAccessionPIBook(id);

            // pull data to fill the box
            rptBox.DataSource = getDataViewData("web_accessiondetail_summary", id);
            rptBox.DataBind();

         }

        private void bindAccessionNames(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_accessionnames", id);
            showAndBind(plAccessionNames, rptAccessionNames, dt);
        }

        private void bindIntellectualPropertyRights(int id) 
        {
            DataTable dt = getDataViewData("web_accessiondetail_ipr", id);
            //showAndBind(plAccessionIpr, rptAccessionIpr, dt);

            if (dt.Rows.Count > 0)
            {
                plAccessionIpr.Visible = true;
                rptAccessionIpr.DataSource = dt;
                rptAccessionIpr.ItemDataBound += new RepeaterItemEventHandler(rptAccessionIpr_ItemDataBound);
                rptAccessionIpr.DataBind();
            }
        }

        void rptAccessionIpr_ItemDataBound(object sender, RepeaterItemEventArgs e) 
        {
            if (e.Item.ItemIndex > -1) {
                int accessionIprId = (int)((DataRowView)e.Item.DataItem)["accession_ipr_id"];
                Repeater rptIprCitation = e.Item.FindControl("rptIprCitation") as Repeater;

                rptIprCitation.DataSource = getDataViewData("web_citation", ":methodid=0;:iprid=" + accessionIprId + ";:pedigreeid=0;:markerid=0", 0);
                rptIprCitation.DataBind();
            }
        }

        private void bindAvailability(int id) 
        {
            dtlAvailability.DataSource = getDataViewData("web_accessiondetail_availability", ":accessionid=" + id + ";:accessionid2=" + id, 1);
            dtlAvailability.DataBind();
        }

        public bool IsAvailable(object val) 
        {
            return val.ToString() == "Available";
        }

        private void bindNarrative(int id) 
        {
            DataTable dt = getDataViewData("web_accessiondetail_narrative", id);
            showAndBind(plNarrative, rptNarrative, dt);
        }

        private void bindSourceHistory(int id) 
        {

            // we need to do hierarchical databinding -- run a query for each
            // row returned from this query.  To do that, we must call some code
            // on each row.  The ItemDataBound event lets us do that.

            DataTable dt = getDataViewData("web_accessiondetail_srchistory", id);
            if (dt.Rows.Count > 0)
            {
                plSource.Visible = true;
                rptSourceHistory.DataSource = dt;
                rptSourceHistory.ItemDataBound += new RepeaterItemEventHandler(rptSourceHistory_ItemDataBound);
                rptSourceHistory.DataBind();
            }
        }

        void rptSourceHistory_ItemDataBound(object sender, RepeaterItemEventArgs e) 
        {
            // header and footer will fire this event, but have no data against which we can bind.
            // so checking against -1 prevents us from trying to (and bombing)
            if (e.Item.ItemIndex > -1) {

                // grab the accession_source_id from the associated data item
                int accessionSourceId = (int)((DataRowView)e.Item.DataItem)["accession_source_id"];

                // grab the repeater control from the repeater's row object
                Repeater rptSourceDetail = e.Item.FindControl("rptSourceDetail") as Repeater;

                // then do our query and databind, as usual
                DataTable dt = getDataViewData("web_accessiondetail_srchistory_detail", ":accessionsrcid=" + accessionSourceId, 0);
                if (dt.Rows.Count == 0)
                    rptSourceDetail.Visible = false;
                else
                {
                    rptSourceDetail.Visible = true;
                    rptSourceDetail.DataSource = dt;
                    rptSourceDetail.DataBind();
                }
            }
        }

        private void bindCitations(int id) 
        {
            DataTable dt = getDataViewData("web_accessiondetail_citations", id);
            showAndBind(plCitations, rptCitations, dt);
        }

        private void bindVouchers(int id) 
        {
            DataTable dt = getDataViewData("web_accessiondetail_vouchers", id);
            showAndBind(plVouchers, rptVouchers, dt);
        }

        protected void btnAddToOrder_Single(object sender, ImageClickEventArgs e)
        {
            int id = int.Parse(ViewState["accession_id"].ToString());

            // this accession needs to be added to the order.
            Cart.Current.AddAccession(id, null);
            Cart.Current.Save();

            string msg = this.GetDisplayMember("AccessionDetail", "cartAddItem", "Added item to your order.");
           
            Master.ShowMessage(msg);
        }
        
        
        protected void btnAddToMyFavorites_Single(object sender, ImageClickEventArgs e)
        {   
            int id = int.Parse(ViewState["accession_id"].ToString());

            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                Response.Redirect("~/login.aspx?action=addfav&ReturnUrl=accessiondetail.aspx?id=" + id);
            else
            {
                Favorite.Current.AddAccession(id, null);
                Favorite.Current.Save();
                dtlAvailability.FindControl("pnlNotFavorite").Visible = false;
                dtlAvailability.FindControl("pnlFavorite").Visible = true;

                string msg = Page.GetDisplayMember("AccessionDetail", "favoriteAddItem", "Added item to your favorite.");
                Master.ShowMessage(msg);
            }
        }

        private void bindAnnotations(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_annotations", id);

//            DataTable dt = DataManager.ExecRead(@"
//            select 
//                an.site_code,
//                an.action_name, 
//                an.action_date,
//                (case when t1.name IS NULL then ' ' else t1.name end) as old_name,
//                (case when t2.name IS NULL then ' ' else t2.name end) as new_name
//            from  accession_annotation an 
//            left join inventory i
//                on an.inventory_id = i.inventory_id and i.inventory_type_code = '**'
//            left join taxonomy t1 on an.old_taxonomy_id = t1.taxonomy_id  
//            left join taxonomy t2 on an.new_taxonomy_id = t2.taxonomy_id 
//            where i.accession_id = :accessionid
//            ", new DataParameters(":accessionid", id));

            //rptAnnotations.DataSource = dt;
            //rptAnnotations.DataBind();
            showAndBind(plAnnotations, rptAnnotations, dt);
        }

        private void bindPedigree(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_pedigree", id);

//            DataTable dt = DataManager.ExecRead(@"
//            select description from accession_pedigree where accession_id = :accessionid
//            ", new DataParameters(":accessionid", id));

            //showAndBind(plPedigree, rptPedigree, dt); 
            if (dt.Rows.Count > 0)
            {
                plPedigree.Visible = true;
                rptPedigree.DataSource = dt;
                rptPedigree.ItemDataBound += new RepeaterItemEventHandler(rptPedigree_ItemDataBound);
                rptPedigree.DataBind();
            }
        }

        void rptPedigree_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemIndex > -1)
            {
                int accessionPediId = (int)((DataRowView)e.Item.DataItem)["accession_pedigree_id"];
                Repeater rptPedigreeCitation = e.Item.FindControl("rptPedigreeCitation") as Repeater;

                rptPedigreeCitation.DataSource = getDataViewData("web_citation", ":methodid=0;:iprid=0;:pedigreeid=" + accessionPediId + ";:markerid=0", 0);
                rptPedigreeCitation.DataBind();
            }
        }


        private void showAndBind(Panel pl, Repeater  rp, DataTable dt)
        {
            if (dt.Rows.Count > 0){
                pl.Visible = true;
                rp.DataSource = dt;
                rp.DataBind();
             }
        }

        private void bindObservation(int id) 
        {
            DataTable dt = getDataViewData("web_accessiondetail_observation_phenotype", id);
            DataTable dtg = getDataViewData("web_accessiondetail_observation_genotype", id);

            if ((dt.Rows.Count > 0) || (dtg.Rows.Count > 0))
            {
                divObserve.Visible = true;
                hp.NavigateUrl = "~/AccessionObservation.aspx?id=" + id;
            }

            if (dt.Rows.Count > 0)
            {
                plObservation.Visible = true;

                HtmlTable tblCropTrait = this.tblCropTrait;

                string category = "", newCategory = "";
                string trait = "", newTrait = "";

                HtmlTableRow htr = new HtmlTableRow();  
                //htr.Attributes.Add("class", "someHeaderStyle");   //Laura todo
                HtmlTableCell tc_h = new HtmlTableCell("TH");
                tc_h.InnerHtml = "Category";
                htr.Cells.Add(tc_h);


                HtmlTableRow dtr = new HtmlTableRow();   
                HtmlTableCell tc_d = new HtmlTableCell("TH");
                tc_d.InnerHtml = "Descriptor";
                dtr.Cells.Add(tc_d);


                HtmlTableRow ctr = new HtmlTableRow();   
                HtmlTableCell tc_c = new HtmlTableCell("TH");
                tc_c.InnerHtml = "Value";
                ctr.Cells.Add(tc_c);

                //HtmlTableRow qtr = new HtmlTableRow();   
                // HtmlTableCell tc_q = new HtmlTableCell("TH");
                //tc_q.InnerHtml = "Qualifier";
                //qtr.Cells.Add(tc_q);

                HtmlTableRow str = new HtmlTableRow();   
                HtmlTableCell tc_s = new HtmlTableCell("TH");
                tc_s.InnerHtml = "Study/Environment";
                str.Cells.Add(tc_s);

                int colSpan_h = 1, colSpan_d = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    newCategory = dr["category_code"].ToString();
                    if (newCategory != category)
                    {
                        HtmlTableCell tc = new HtmlTableCell("TH");

                        if (category != "")
                        {
                            tc.InnerHtml = category;
                            tc.Attributes.Add("colspan", "" + colSpan_h + "");

                            htr.Cells.Add(tc);
                            colSpan_h = 1;
                        }
                        category = newCategory;
                    }
                    else
                    {
                        colSpan_h++;
                    }

                    newTrait = dr["trait_name"].ToString();
                    if (newTrait != trait)
                    {
                        HtmlTableCell tc = new HtmlTableCell("TH");

                        if (trait != "")
                        {
                            tc.InnerHtml = trait;
                            tc.Attributes.Add("colspan", "" + colSpan_d + "");

                            dtr.Cells.Add(tc);
                            colSpan_d = 1;
                        }
                        trait = newTrait;
                    }
                    else
                    {
                        colSpan_d++;
                    }

                    HtmlTableCell ctc = new HtmlTableCell();
                    ctc.InnerHtml = dr["trait_value"].ToString();
                    ctr.Cells.Add(ctc);

                    //HtmlTableCell qtc = new HtmlTableCell();
                    //qtc.InnerHtml = dr["qualifier_name"].ToString();
                    //qtr.Cells.Add(qtc);

                    HtmlTableCell stc = new HtmlTableCell();
                    stc.InnerHtml = dr["method_name"].ToString();   
                    str.Cells.Add(stc);
                }

                HtmlTableCell tc1 = new HtmlTableCell("TH");
                tc1.InnerHtml = newCategory;
                tc1.Attributes.Add("colspan", "" + colSpan_h + "");
                htr.Cells.Add(tc1);

                HtmlTableCell tc2 = new HtmlTableCell("TH");
                tc2.InnerHtml = newTrait;
                tc2.Attributes.Add("colspan", "" + colSpan_d + "");
                dtr.Cells.Add(tc2);

                tblCropTrait.Rows.Add(htr);
                tblCropTrait.Rows.Add(dtr);
                tblCropTrait.Rows.Add(ctr);
                //tblCropTrait.Rows.Add(qtr);
                tblCropTrait.Rows.Add(str);
            }

            if (dtg.Rows.Count > 0)
            {
                plGeno.Visible = true;
                HtmlTable tblGeno = this.tblGeno;

                HtmlTableRow trp = new HtmlTableRow();   
                HtmlTableCell tcp = new HtmlTableCell("TH");
                tcp.InnerHtml = "Poly Type";
                trp.Cells.Add(tcp);

                HtmlTableRow trm = new HtmlTableRow();   
                HtmlTableCell tcm = new HtmlTableCell("TH");
                tcm.InnerHtml = "Marker";
                trm.Cells.Add(tcm);

                HtmlTableRow trv = new HtmlTableRow();   
                HtmlTableCell tcv = new HtmlTableCell("TH");
                tcv.InnerHtml = "Value";
                trv.Cells.Add(tcv);

                HtmlTableRow tre = new HtmlTableRow();   
                HtmlTableCell tce = new HtmlTableCell("TH");
                tce.InnerHtml = "Evaluation";
                tre.Cells.Add(tce);

                HtmlTableRow trs = new HtmlTableRow();   
                HtmlTableCell tcs = new HtmlTableCell("TH");
                tcs.InnerHtml = "Study Type";
                trs.Cells.Add(tcs);

                HtmlTableRow tri = new HtmlTableRow();   
                HtmlTableCell tci = new HtmlTableCell("TH");
                tci.InnerHtml = "Inventory ID";
                tri.Cells.Add(tci);

                foreach (DataRow dr in dtg.Rows)
                {
                    HtmlTableCell tc1 = new HtmlTableCell();
                    tc1.InnerHtml = dr["Poly_Type"].ToString();
                    trp.Cells.Add(tc1);
                    HtmlTableCell tc2 = new HtmlTableCell();
                    tc2.InnerHtml = dr["Marker"].ToString();
                    trm.Cells.Add(tc2);
                    HtmlTableCell tc3 = new HtmlTableCell();
                    tc3.InnerHtml = dr["Value"].ToString();
                    trv.Cells.Add(tc3);
                    HtmlTableCell tc4 = new HtmlTableCell();
                    tc4.InnerHtml = dr["Evaluation"].ToString();
                    tre.Cells.Add(tc4);
                    HtmlTableCell tc5 = new HtmlTableCell();
                    tc5.InnerHtml = dr["Study_Type"].ToString();
                    trs.Cells.Add(tc5);
                    HtmlTableCell tc6 = new HtmlTableCell();
                    tc6.InnerHtml = dr["Inventory_ID"].ToString(); 
                    tri.Cells.Add(tc6);
                }
                tblGeno.Rows.Add(trp);
                tblGeno.Rows.Add(trm);
                tblGeno.Rows.Add(trv);
                tblGeno.Rows.Add(tre);
                tblGeno.Rows.Add(trs);
                tblGeno.Rows.Add(tri);
            }
        }

        private void bindInventoryImage(int id)
        {
            DataTable dt = null;

            using (var sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                dt = sd.GetData("web_accessiondetail_inventory_image", ":accessionid=" + id, 0, 0).Tables["web_accessiondetail_inventory_image"];
            }

            if (dt.Rows.Count > 0)
            {
                imagePreviewer.DataSource = dt;
                imagePreviewer.DataBind();
            }
            else
            {
                imagePreviewer.Visible = false;
            }
        }

        public string Resolve(object url)
        {
            if (url is string && !String.IsNullOrEmpty(url as string))
            {
                string path = url as string;
                if (path.ToUpper().IndexOf("HTTP://") > -1)
                {
                    return path;
                }
                else
                {
                    path = "~/uploads/images/" + path;
                    return Page.ResolveClientUrl(path.Replace(@"\", "/").Replace("//", "/"));// convert \ to / and resolve ~/ to /gringlobal/ ....
                }
            }
            else
            {
                return "";
            }
        }

        private void bindAvailabilityNote(int id)
        {
            string text = "";
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    string status = dm.ReadValue(@"select status_code from accession where accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();
                    if (status == "INACTIVE")
                    {
                        text = "<hr>Accession is not in the NPGS, contact the donor for availability. Historical record only. <b>Please do not request this accession.</b><hr>";
                    }
                    else
                    {
                        string site = dm.ReadValue(@"select '<a href=""site.aspx?id='+ convert(nvarchar, s.site_id)+ '"">' + s.site_short_name + '</a>' as site_code  from accession a left join cooperator c on a.owned_by = c.cooperator_id left join site s on c.site_id = s.site_id where a.accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();
                        if (site.Contains("NSSB"))
                        {
                            text = "<hr>This accession is part of the <b>non-NPGS</b> backup material, <b>please order it directly from the donor/developer.</b><hr>";
                        }
                        else if (site.Contains("PGQO"))
                        {
                            text = "<hr>This accession is still under quarantine restrictions.<hr>";
                        }
                        else
                        {
                            string restriction = dm.ReadValue(@"select t.restriction_code from taxonomy_species t join accession a on t.taxonomy_species_id = a.taxonomy_species_id and a.accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();
                            switch (restriction)
                            {
                                case "NARCOTIC":
                                    text = "<hr>This accession is part of the <b>non-NPGS</b> backup material, <b>please order it directly from the donor.</b><hr>";
                                    break;

                                case "RARE":
                                    //site = dm.ReadValue(@"select '<a href=""site.aspx?id='+ convert(nvarchar, s.site_id)+ '"">' + s.site_short_name + '</a>' as site_code  from accession a left join cooperator c on a.owned_by = c.cooperator_id left join site s on c.site_id = s.site_id where a.accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();
                                    text = "<hr>This accession is endangered or rare, contact " + site + " for availability and collection site details.<hr>";
                                    break;

                                case "WEED":
                                    //site = dm.ReadValue(@"select '<a href=""site.aspx?id='+ convert(nvarchar, s.site_id)+ '"">' + s.site_short_name + '</a>' as site_code  from accession a left join cooperator c on a.owned_by = c.cooperator_id left join site s on c.site_id = s.site_id where a.accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();
                                    text = "<hr><font color=red><b>This accession is a restricted noxious weed</b></font>, contact " + site + " for availability.<hr>";
                                    break;

                                default:
                                    var dtJPR = dm.Read(@"select aipr.* from accession_ipr aipr where type_code in ('CSR', 'JPR') and expired_date is null and (select COUNT(*) from accession_ipr where accession_id = aipr.accession_id ) = 1 and accession_id = :aid ", new DataParameters(":aid", id, DbType.Int32));
                                    if (dtJPR.Rows.Count == 1)
                                    {
                                        string reg = dtJPR.Rows[0]["type_code"].ToString();
                                        if (reg == "CSR")
                                            reg = "Crop Science Registration";
                                        else
                                            reg = "Journal of Plant Registration";

                                        text = "<b><hr>Recent " + reg + "</b>.  Preferably, seed should be requested from the developers.<hr>";
                                    }
                                    else
                                    {
                                        var dtIPR = dm.Read(@"select * from accession_ipr where expired_date is null and type_code not like 'MTA%' and accession_id = :aid", new DataParameters(":aid", id, DbType.Int32));
                                        if (dtIPR.Rows.Count > 0)
                                            text = "<hr>This accession is protected by Intellectual Property Rights, it should be requested from the donor/developer.<hr>";
                                        else
                                        {
                                            var dtInv = dm.Read(@"select * from inventory where accession_id = :aid", new DataParameters(":aid", id, DbType.Int32));
                                            if (dtInv.Rows.Count == 1) // no real inventory
                                                text = "<hr>contact " + site + " for availability status.<hr>";
                                            else
                                            {
                                                DataTable dt = sd.GetData("web_accessiondetail_action_note", ":actionName=" + "''AVAIL_CMT''" + ";:accessionid=" + id, 0, 0).Tables["web_accessiondetail_action_note"];
                                                showAndBind(plAvailabilityNote, rptAvailNote, dt);

                                                string ctCnt = dm.ReadValue(@"select COUNT(distribution_default_form_code) from inventory where accession_id = :aid and distribution_default_form_code = 'CT'", new DataParameters(":aid", id, DbType.Int32)).ToString();
                                                if (ctCnt != "0")
                                                {
                                                    text = "(<b>Note:</b> You will receive <i>Unrooted cuttings</i> not <i>Rooted plants</i> unless specific arrangements have been made with the curator.)";
                                                }
                                            }
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }
             }
            if (text != "")
            {
                plAvailabilityNote.Visible = true;
                lblAvailNote.Visible = true;
                lblAvailNote.Text = text;
            }
        }

        private void bindWebAvailabilityNote(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_availability_note", id);
            //showAndBind(plAvailabilityNote, rptAvailabilityNote, dt);
            if (dt.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    plWebAvailabilityNote.Visible = true;
                    rptWebAvailNote.DataSource = dt;
                    rptWebAvailNote.DataBind();
                }
            }
        }

        public string UpcaseFirstLetter(object val)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                ret = ret.ToLower();
                ret = ret.Substring(0, 1).ToUpper() + ret.Substring(1, ret.Length - 1);
            }
            return ret;
        }

        public string LowCaseString(object val)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                ret = ret.ToLower();
            }
            return ret;
        }

        public bool HasValue(object val)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                return true;
            }
            return false;
        }

        public bool IsVisible(object val, string category)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                if (ret == category)
                    return true;
                else
                    return false;
            }
            return false;

        }
     
       public string DisplaySourceType(object val1, object val2)
        {
            string ret1 = val1.ToString();
            if (!String.IsNullOrEmpty(ret1))
            {
                int id = Toolkit.ToInt32(ret1);
                DataTable dt = getDataViewData("web_accessiondetail_srchistory_detail", ":accessionsrcid=" + id, 0);
                if (dt.Rows.Count == 0)
                    return "<br/>";
                else
                {
                    string ret = val2 as string;
                    if (!String.IsNullOrEmpty(ret))
                    {
                        switch (ret.ToUpper())
                        {
                            case "COLLECTED":
                                ret = "Collectors:";
                                break;
                            case "DEVELOPED":
                                ret = "Developers:";
                                break;
                            case "DONATED":
                                ret = "Donors:";
                                break;
                            default:
                                ret = "Supported by:";
                                break;
                        }
                    }
                    return ret;
                }
            }
            else
                return "<br/>";
        }

       private void bindFavorite(int id)
       {
           bool isFavorite = false;

           if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
           {
               isFavorite = false;
           }
           else
           {
               DataTable dt = null;
               using (var sd = UserManager.GetSecureData(true))
               {
                   int wuserid = sd.WebUserID;
                   dt = sd.GetData("web_favorites_view", ":wuserid=" + wuserid + ";:accessionid=" + 0, 0, 0).Tables["web_favorites_view"]; 
               }

               foreach (DataRow dr in dt.Rows)
               {
                   if (dr["accession_id"].ToString() == id.ToString())
                   {
                       isFavorite = true;
                       break;
                   }
               }
           }
           dtlAvailability.FindControl("pnlNotFavorite").Visible = !isFavorite;
           dtlAvailability.FindControl("pnlFavorite").Visible = isFavorite;
       }

       protected void btnRemoverFromMyFavorites_Single(object sender, ImageClickEventArgs e)
       {
            int id = int.Parse(ViewState["accession_id"].ToString());

            Favorite.Current.RemoveAccession(id, false);
            Favorite.Current.Save();
            dtlAvailability.FindControl("pnlNotFavorite").Visible = true;
            dtlAvailability.FindControl("pnlFavorite").Visible = false;

            string msg = Page.GetDisplayMember("AccessionDetail", "favoriteRemoveItem", "Removed item from your favorite.");
            Master.ShowMessage(msg);
        }

       public bool IsMTA(object val)
       {
           string ret = val as string;
           if (!String.IsNullOrEmpty(ret))
           {
               if (ret.IndexOf("MTA") > -1)
                   return true;
               else
                   return false;
           }
           return false;
       }

        public bool IsPatent(object val)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                if (ret.IndexOf("UTILITY") > -1)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public bool IsPVP(object val)
        {
            string ret = val as string;
            if (!String.IsNullOrEmpty(ret))
            {
                if (ret.IndexOf("PVP") > -1)
                    return true;
                else
                    return false;
            }
            return false;
        }

        private void bindQuarantine(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_quarantine", id);
            showAndBind(plQuarantine, rptQuarantine, dt);
        }

        protected string DisplayDate(object date, object dateCode, bool addPeriod)
        {
            if (!String.IsNullOrEmpty(date.ToString()))
            {
                DateTime dt = Toolkit.ToDateTime(date);
                string dc = dateCode as string;
                string fmt = "";
                string prefix = "";

                if (!String.IsNullOrEmpty(dc))
                {
                    if (dc.IndexOf(' ') > 0)
                    {
                        prefix = dc.Split(' ')[0];
                        fmt = dc.Split(' ')[1];
                    }
                    else
                        fmt = dc;
                }
                else
                    fmt = "dd-MMM-yyyy";

                switch (fmt)
                {
                    case "MM/dd/yyyy":
                    case "dd/MM/yyyy":
                        fmt = "dd-MMM-yyyy";
                        break;
                    case "MM/yyyy":
                        fmt = "MMM-yyyy";
                        break;
                    default:
                        break;
                }

                string value = (prefix == "" ? "" : prefix + " ") + dt.ToString(fmt);
                return value == "" ? "" : value + (addPeriod ? "." : "");
            }
            else
                return "";
       }

        protected string GetOtherLink(object url)
        {
            if (url is string && !String.IsNullOrEmpty(url as string))
            {
                string path = url as string;
                string extra = "http://www.ars-grin.gov";
                //if (path.IndexOf(extra)== 0)
                //{
                //    path = path.Substring(extra.Length, (path.Length - extra.Length));
                //    return path;
                //}
                //else
                //{
                //    return path;  
                //}
                if (path.IndexOf("/") == 0)
                {
                    path = extra + path;
                    return path;
                }
                else
                {
                    return path;
                }

            }
            else
            {
                return "";
            }
        }

        private void bindInventoryOther(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_inventory_other", id);
            showAndBind(plOther, rptOther, dt);
        }

        private void bindActionNote(int id)
        {
            using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                DataTable dt = sd.GetData("web_accessiondetail_action_note", ":actionName=" + "select distinct action_name_code from accession_action where is_web_visible = 'Y' and action_name_code != 'AVAIL_CMT'" + ";:accessionid=" + id, 0, 0).Tables["web_accessiondetail_action_note"];
                showAndBind(plActionNote, rptActionNote, dt);
            }
        }

        protected string SourceDescriptor (object val)
        {
            string ret = val.ToString();
            if (!String.IsNullOrEmpty(ret))
            {
                int id = Toolkit.ToInt32(ret);
                DataTable dt = null;

                using (var sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    dt = sd.GetData("web_accessiondetail_srchistory_descriptor", ":accessionsrcid=" + id, 0, 0).Tables["web_accessiondetail_srchistory_descriptor"];
                }

                if (dt.Rows.Count > 0)
                {
                    string descriptor = "";
                    string value = "";
                    StringBuilder sb = new StringBuilder();

                    foreach (DataRow dr in dt.Rows)
                    {
                        descriptor = dr["trait_name"].ToString();
                        value = dr["trait_value"].ToString();
                        sb.Append(" " + descriptor + ": " + value + ".");
                    }
                    return sb.ToString();
                 }
                else
                    return "";
            }
            else
                return "";
        }

        private void bindPathogen(int id)
        {
            DataTable dt = getDataViewData("web_accessiondetail_pathogen", id);

            if (dt.Rows.Count > 0)
            {
                plPathogen.Visible = true;

                HtmlTable tblPathogen = this.tblPathogen;

                string testType = "", newTestType = "";
                string material = "", newMaterial = "";

                foreach (DataRow dr in dt.Rows)
                {
                    newTestType = dr["test_type_code"].ToString();
                    if (newTestType != testType)
                    {
                        HtmlTableRow htr = new HtmlTableRow();

                        HtmlTableCell th1 = new HtmlTableCell("TH");

                        th1.InnerHtml = dr["test_type"].ToString();
                        th1.Attributes.Add("align", "left");
                        htr.Cells.Add(th1);

                        HtmlTableCell th2 = new HtmlTableCell();
                        th2.InnerHtml = "";
                        th2.Attributes.Add("colspan", "7");
                        htr.Cells.Add(th2);

                        tblPathogen.Rows.Add(htr);
                    } 
                    testType = dr["test_type_code"].ToString(); 

                    HtmlTableRow dtr = new HtmlTableRow();
                    
                    HtmlTableCell t1 = new HtmlTableCell();
                    t1.InnerHtml = dr["test_contaminant"].ToString();
                    dtr.Cells.Add(t1);

                    HtmlTableCell t2 = new HtmlTableCell();
                    newMaterial = dr["inventory_material"].ToString();
                    if (newMaterial != material)
                    {
                        t2.InnerHtml = dr["inventory_material"].ToString();
                    }
                    else
                        t2.InnerHtml = "";
                    material = dr["inventory_material"].ToString();
                    dtr.Cells.Add(t2);

                    HtmlTableCell t3 = new HtmlTableCell();
                    //t3.InnerHtml = dr["completed_date"].ToString();
                    t3.InnerHtml = DisplayDate(dr["completed_date"], dr["completed_date_code"], false);
                    dtr.Cells.Add(t3);

                    HtmlTableCell t4 = new HtmlTableCell();
                    t4.InnerHtml = dr["test_result_code"].ToString();
                    dtr.Cells.Add(t4);

                    HtmlTableCell t5 = new HtmlTableCell();
                    t5.InnerHtml = dr["required_replication_count"].ToString();
                    dtr.Cells.Add(t5);

                    HtmlTableCell t6 = new HtmlTableCell();
                    t6.InnerHtml = dr["started_count"].ToString();
                    dtr.Cells.Add(t6);

                    HtmlTableCell t7 = new HtmlTableCell();
                    t7.InnerHtml = dr["completed_count"].ToString();
                    dtr.Cells.Add(t7);

                    HtmlTableCell t8 = new HtmlTableCell();
                    t8.InnerHtml = dr["note"].ToString();
                    dtr.Cells.Add(t8);

                    tblPathogen.Rows.Add(dtr);

                }
            }
        }

        private void setPageTitle(Page page, int id)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    var dt = dm.Read(@"select accession_number_part1, accession_number_part2, accession_number_part3 from accession where accession_id = :aid", new DataParameters(":aid", id, DbType.Int32));
                    if (dt.Rows.Count >= 1)
                        //page.Title = " - Accession: " + dt.Rows[0][0].ToString() + " " + dt.Rows[0][1].ToString() + " " + dt.Rows[0][2].ToString();
                        page.Title = "Accession: " + dt.Rows[0][0].ToString() + " " + dt.Rows[0][1].ToString() + " " + dt.Rows[0][2].ToString();
                }
            }
        }

        protected Boolean HasPIValue = false;
        protected string PIAssigned = "";
        protected string IVVolume = "";
        private void bindAccessionPIBook(int id)
        {
            try
            {
                using (var sd = new SecureData(false, UserManager.GetLoginToken(true)))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {
                        // Only US NPGS would have the value and continue processing
                        string acp = dm.ReadValue(@"select accession_number_part1 from accession where accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString();

                        if (acp.Trim() == "PI")
                        {
                            //string cnt = dm.ReadValue(@"select count(sys_table_id) from sys_table where table_name = 'plant_inventory_index'").ToString();
                            //if (cnt != "0")
                            //{
                                int pinumber = Int32.Parse(dm.ReadValue(@"select accession_number_part2 from accession where accession_id = :aid", new DataParameters(":aid", id, DbType.Int32)).ToString());

                                DataTable dt = sd.GetData("web_accessiondetail_pi", ":pinumber=" + pinumber, 0, 0).Tables["web_accessiondetail_pi"];

                                if (dt.Rows.Count > 0)
                                {
                                    HasPIValue = true;
                                    DataRow dr = dt.Rows[0];
                                    PIAssigned = dr["plant_inventory_year"].ToString();
                                    IVVolume = dr["plant_inventory_volumn"].ToString();
                                }


                                dt = sd.GetData("web_accessiondetail_piindex", ":pinumber=" + pinumber, 0, 0).Tables["web_accessiondetail_piindex"];

                                if (dt.Rows.Count > 0)
                                {
                                    lblViewPDF.Visible = true;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string PIIndex = "in_vol=" + dr["plant_inventory_volumn"].ToString() + "&in_suffix=" + dr["volumn_suffix"].ToString() + "&in_page=" + dr["page"].ToString();
                                        lblViewPDF.Text = lblViewPDF.Text.Replace("*", PIIndex);
                                    }
                                }

                                dt = sd.GetData("web_accessiondetail_pimindex", ":pinumber=" + pinumber, 0, 0).Tables["web_accessiondetail_pimindex"];

                                if (dt.Rows.Count > 0)
                                {
                                    lblViewIMPDF.Visible = true;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string PIIndex = "in_vol=" + dr["plant_immigrant_volumn"].ToString() + "&in_page=" + dr["plant_immigrant_page"].ToString();
                                        lblViewIMPDF.Text = lblViewIMPDF.Text.Replace("*", PIIndex);
                                    }
                                }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            { string msg = ex.Message; }
        }
    }

}