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
    public partial class OrderHistory : System.Web.UI.Page
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
                    bindData();
            }
        }

        private void bindData()
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true, true))
                {
                    // Anonymous User order records have web_cooperator_id = -1. Cann't let level 1 user see Other anonymous user's order. 
                    int wcoopid = 0;
                    if (sd.WebCooperatorID == -1)
                        wcoopid = 0;   
                    else
                        wcoopid = sd.WebCooperatorID;

                    DataTable dt = null;
                    dt = dm.Read(@"
                        select wor.created_date as statusDate, wor.web_order_request_id as orderID, wor.status_code, wor.special_instruction 
                        from web_order_request wor  
                        where wor.web_cooperator_id = :wcoopID
                        order by wor.created_date desc
                        ", new DataParameters(":wcoopID", wcoopid));

                        gvOrders.DataSource = dt;
                        gvOrders.DataBind();
                        if (dt.Rows.Count > 0)
                        {
                            lblSummary.Text = "You have submitted <b>" + dt.Rows.Count + "</b> germplasm order(s).";
                            lblSummary.Visible = true;
                        }
                        else
                            lblSummary.Visible = false;
                }
            }
        }

        protected void gvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            int orderID = Toolkit.ToInt32(gvOrders.SelectedDataKey.Value, 0);
            this.Response.Redirect("orderhistorydetail.aspx?id=" + orderID);
        }

        protected string GetOrderStatus(object valid, object valstatus)
        {
            string ret = valstatus.ToString();
            string retid = valid.ToString();
            if (!String.IsNullOrEmpty(retid))
            {
                int orderID = Toolkit.ToInt32(retid);

                using (var sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    using (DataManager dm = sd.BeginProcessing(true, true))
                    {

                        // DataTable dtItems = sd.GetData("web_order_history_detail", ":orderrequestid=" + orderID, 0, 0).Tables["web_order_history_detail"];
                        DataTable dtItems = dm.Read(@"
                        select wor.status_code as order_status, coalesce(ori.status_code, wori.status_code) as status_code 
	                    from web_order_request_item wori left join order_request_item ori on wori.web_order_request_item_id = ori.web_order_request_item_id
                        left join web_order_request wor on wori.web_order_request_id = wor.web_order_request_id
                        where wori.web_order_request_id = :orderID ",
                            new DataParameters(":orderID", orderID));


                        if (dtItems.Rows.Count > 0)
                        {
                            ret = dtItems.Rows[0].ItemArray[0].ToString();
                            bool complete = true;
                            for (int i = 0; i < dtItems.Rows.Count; i++)
                            {
                                if (dtItems.Rows[i]["status_code"].ToString() != "SHIPPED")
                                {
                                    complete = false;
                                    break;
                                }
                            }
                            if (complete) ret = "SHIPPED";  // Any other order status calculation logic ???

                        }
                    }
                }
            }
            return ret;
        }
    }
}
