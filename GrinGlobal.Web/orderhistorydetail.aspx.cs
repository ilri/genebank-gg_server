using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;

namespace GrinGlobal.Web
{
    public partial class OrderHistoryDetail : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                Response.Redirect("~/login.aspx?action=menu");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                   // Only display what login user's order
                    string id = Request.QueryString["id"].ToString();
                    using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
                    {
                        using (DataManager dm = sd.BeginProcessing(true, true))
                        {
                            DataTable dt = dm.Read(@"
                                select web_order_request_id from web_order_request where web_cooperator_id = :wcoopID ",
                                new DataParameters(":wcoopID", sd.WebCooperatorID));

                            bool isMine = false;
                            foreach (DataRow dr in dt.Rows) {
                                if (dr["web_order_request_id"].ToString() == id)
                                {
                                    isMine = true;
                                    bindDisplayData((Int32.Parse(id)));
                                    break;
                                }
                             }
                             if (!isMine) Response.Redirect("orderhistory.aspx");
                        }
                    }
                }
            }
        }

        private void bindDisplayData(int orderID)
        {
            Requestor r = Requestor.Current;
            DataTable dtItems = null;
            DataTable dtActions = null;

            lblOrderIDs.Text = orderID.ToString();
            Uploader1.RecordID = orderID;
            Uploader1.UserID = r.Webuserid;
            Uploader1.RecordType = "WebOrder";
            bool isRestriction = false;

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
//                    dtItems = dm.Read(@"
//                        select
//                        a.accession_id, wori.status_code, wori.distribution_form_code,
//                        concat(coalesce(a.accession_number_part1,''), ' ', coalesce(convert(varchar, a.accession_number_part2),''), ' ', coalesce(a.accession_number_part3,'')) as pi_number,
//                        s.site_short_name as site,
//                        (select TOP 1 plant_name from accession_name an where a.accession_id = an.accession_id and plant_name_rank = (select MIN(plant_name_rank) from accession_name an2 where a.accession_id = an2.accession_id)) as top_name,
//					    a.taxonomy_species_id, (tg.genus_name + ' ' + t.species_name) as taxonomy_name, wori.user_note as note 
//                        from web_order_request_item wori
//                        join accession a on a.accession_id = wori.accession_id
//                        left join taxonomy_species t on a.taxonomy_species_id = t.taxonomy_species_id 
//                        left join taxonomy_genus tg on t.taxonomy_genus_id = tg.taxonomy_genus_id
//                        left join web_order_request wor on wori.web_order_request_id = wor.web_order_request_id
//                        left join cooperator c on a.owned_by = c.cooperator_id
//						left join site s on c.site_id = s.site_id
//                        where wori.web_order_request_id = :orderrequesetid
//                        order by accession_id asc
//                        ", new DataParameters(":orderrequesetid", orderID));

                    dtItems = sd.GetData("web_order_history_detail", ":orderrequestid=" + orderID, 0, 0).Tables["web_order_history_detail"];

                    if (dtItems.Rows.Count > 0)
                    {
                        lblOrderStatus.Text = dtItems.Rows[0].ItemArray[1].ToString();

                        for (int i = 0; i < dtItems.Rows.Count; i++)
                        {
                            if (dtItems.Rows[i]["type_code"].ToString().StartsWith("MTA-"))
                            {
                                isRestriction = true;
                                break;
                            }
                        }

                        DataTable dtItemstatus = sd.GetData("web_order_history_detail_itemstatus", ":orderrequestid=" + orderID, 0, 0).Tables["web_order_history_detail_itemstatus"];
                        bool complete = true;
                        if (dtItemstatus.Rows.Count > 0)
                        {
                            string itemStatus = "";
                            int woriID = 0;
                            dtItems.Columns["status_code"].ReadOnly = false;

                            foreach (DataRow drItem in dtItems.Rows)
                            {
                                woriID = Toolkit.ToInt32(drItem["web_order_request_item_id"].ToString(), 0);
                                DataRow[] foundRows = dtItemstatus.Select("web_order_request_item_id = " + woriID);

                                if (foundRows.Count() > 0)
                                {
                                    itemStatus = foundRows[0].ItemArray[1].ToString();

                                    if (itemStatus != "SHIPPED") complete = false;
                                    
                                    drItem["status_code"] = itemStatus;
                                }
                            }
                        }
                        else
                            complete = false;

                        if (complete) // Any other order status calculation logic ???
                        {
                            lblOrderStatus.Text = "SHIPPED";
                            Uploader1.Enabled = false;
                        }
                        else
                            Uploader1.Enabled = true;
                    }

                    dtActions = sd.GetData("web_order_history_action_detail", ":orderrequestid=" + orderID, 0, 0).Tables["web_order_history_action_detail"];

                    DataTable dt2 = dm.Read(@"
                        select intended_use_code, intended_use_note, special_instruction from web_order_request
                        where web_order_request_id = :orderID ",
                        new DataParameters(":orderID", orderID));

                    if (dt2.Rows.Count > 0)
                    {
                        lblIntended.Text = dt2.Rows[0].ItemArray[0].ToString() + ":" + dt2.Rows[0].ItemArray[1].ToString();
                        lblSpecial.Text = dt2.Rows[0].ItemArray[2].ToString();
                    }
                }
            }

            if (dtItems != null)
            {
                gvOrderItems.DataSource = dtItems;
                gvOrderItems.DataBind();
                gvOrderItems.Columns[6].Visible = isRestriction;

                int cnt = gvOrderItems.Rows.Count;
                lblCnt.Text = " (" + cnt.ToString() + (cnt == 1 ? " item)" : " items)");
            }

            gvOrderActions.DataSource = dtActions;
            gvOrderActions.DataBind();

            lblName.Text = r.Firstname + " " + r.Lastname;
            lblOrganization.Text = r.Organization;
            lblPhone.Text = "PHONE: " + r.Phone;
            lblFax.Text = "  FAX: " + r.Fax;

            DataTable dt = UserManager.GetShippingAddressForOrder(orderID);
            foreach (DataRow dr in dt.Rows)
            {
                lblAdd1.Text = dr["address_line1"].ToString();
                lblAdd2.Text = dr["address_line2"].ToString();
                lblAdd3.Text = dr["address_line3"].ToString();
                lblAdd4.Text = dr["city"].ToString() + ", " + dr["state_name"].ToString() + " " + dr["postal_index"].ToString() + ",  " + dr["country_name"].ToString();
            }
        }
        protected string GetDisplayText(object formcode)
        {
            if (formcode is string && !String.IsNullOrEmpty(formcode as string))
            {
                string text = formcode as string;
                text = CartItem.GetMaterialDescription(text);
                return text;
             }
            else
            {
                return "";
            }

        }
    }
}
