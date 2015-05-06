using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;


namespace GrinGlobal.Web.query
{
    public partial class weborderdetail : System.Web.UI.Page
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
                    // Only display the report to CT users.
                    string id = Request.QueryString["id"].ToString();
                    if (Page.User.IsInRole("ALLUSERS"))  bindDisplayData((Int32.Parse(id)));
                }
            }
        }

        private void bindDisplayData(int orderID)
        {
            //Requestor r = Requestor.Current;
            DataTable dtItems = null;
            DataTable dtActions = null;

            lblOrderIDs.Text = orderID.ToString();
            Uploader1.RecordID = orderID;
            //Uploader1.UserID = r.Webuserid;
            Uploader1.RecordType = "WebOrder";
            Uploader1.Enabled = false;   // Report
            bool isRestriction = false;

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
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
                            //Uploader1.Enabled = false;    // Report
                        }
                        //else
                        //    Uploader1.Enabled = true;     // Report
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

                    // get Requestor information 
                    DataTable dtr = dm.Read(@"
                            select wc.*, wu.web_user_id, g.adm1 as state, cvl.title as country 
                            from 
                                web_user wu join web_cooperator wc
                                    on wu.web_cooperator_id = wc.web_cooperator_id
                                join web_order_request wor 
									on wc.web_cooperator_id = wor.web_cooperator_id 
                                left join geography g
                                    on wc.geography_id = g.geography_id
                                join code_value cv 
                                    on g.country_code = cv.value 
                                join code_value_lang cvl 
                                    on cv.code_value_id = cvl.code_value_id
                            where 
                                wor.web_order_request_id = :worid and cvl.sys_lang_id = :langid
                            ", new DataParameters(":worid", orderID, DbType.Int32, ":langid", sd.LanguageID, DbType.Int32
                            ));

                    foreach (DataRow dr in dtr.Rows)
                    {
                        string _firstname = dr["first_name"].ToString();
                        string _lastname = dr["last_name"].ToString();
                        string _organization = dr["organization"].ToString();
                        string _phone = dr["primary_phone"].ToString();
                        string _fax = dr["fax"].ToString();

                        lblName.Text = _firstname + " " + _lastname;
                        lblOrganization.Text = _organization;
                        lblPhone.Text = "PHONE: " + _phone;
                        lblFax.Text = "  FAX: " + _fax;

                        Uploader1.UserID = int.Parse(dr["web_user_id"].ToString());
                    }

                    DataTable dt = UserManager.GetShippingAddressForOrder(orderID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        lblAdd1.Text = dr["address_line1"].ToString();
                        lblAdd2.Text = dr["address_line2"].ToString();
                        lblAdd3.Text = dr["address_line3"].ToString();
                        lblAdd4.Text = dr["city"].ToString() + ", " + dr["state_name"].ToString() + " " + dr["postal_index"].ToString() + ",  " + dr["country_name"].ToString();
                    }
                }
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
