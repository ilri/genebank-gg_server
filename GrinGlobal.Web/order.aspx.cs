using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace GrinGlobal.Web {
    public partial class Order : System.Web.UI.Page {
       
        static string _action = string.Empty;

        protected string DVItemComment = "hide";
        protected string DVSelectUse = "hide";
        protected string DVSelectSubUse = "hide";
        protected string DVPlannedUse = "hide";

        private Dictionary<string, string> _itemNotes = new Dictionary<string, string>();
        private bool hasFootNote = false;
        private bool hasComments = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Toolkit.IsNonWindowsOS) {
                ClientScript.RegisterClientScriptInclude("jquery.ui", Page.ResolveClientUrl("/gringlobal/scripts/jquery-ui-1.7.2.custom.min.js"));
            } else {
                ScriptManager.RegisterClientScriptInclude(this, typeof(string), "jquery.ui", Page.ResolveClientUrl("~/scripts/jquery-ui-1.7.2.custom.min.js"));
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            Page.Master.GetCartControl().Visible = false;
            if (!Page.IsPostBack) {
                if (Request.QueryString["action"] != null)
                {
                    _action = Request.QueryString["action"];  
                }
                
                if (_action == "checkout")
                {   
                    pnlPlaceOrder.Visible = true;
                    populateDropDown(ddlUse, "ORDER_INTENDED_USE");
                    bindOrderData();
                }
                else //view
                {
                    pnlDisplayOrder.Visible = true;
                    bindDisplayData(false);
                }
            } 
        }

        private void bindOrderData() {
            Cart c = Cart.Current;
            Requestor r = Requestor.Current;

            bool isSMTA = false;
            bool isRestriction = false;
            string SMTAIds = "";

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken())) {
                var ds = sd.GetData("web_cartview", ":idlist=" + Toolkit.Join(c.AccessionIDs.ToArray(), ",", ""), 0, 0);

                var dt = ds.Tables["web_cartview"];

                for(int i=0;i<dt.Rows.Count;i++)
                {
                    CartItem ci = c.FindByAccessionID(dt.Rows[i], "accession_id");
                    if (ci != null) {
                        //dt.Rows[i]["quantity"] = ci.Quantity; //Laura Temp
                    }

                    if (dt.Rows[i]["type_code"].ToString() == "MTA-SMTA")
                    {
                        isSMTA = true;
                        SMTAIds += ", <a href='accessiondetail.aspx?id=" + ci.ItemID + "'>" + dt.Rows[i]["pi_number"].ToString() + "</a>";
                    }

                    if (dt.Rows[i]["type_code"].ToString().StartsWith("MTA-"))
                        isRestriction = true;
                }
                gvCart.DataSource = dt;
                gvCart.DataBind();

                int cnt = gvCart.Rows.Count;
                lblCnt.Text = " (" + cnt.ToString() + (cnt == 1 ? " item)" : " items)");

            }

            pnlSMTA.Visible = isSMTA;
            if (isSMTA)
            {
                SMTAIds = SMTAIds.Substring(2);
                lblSMTAIds.Text = "(The items above flagged as SMTA are " + SMTAIds + ")";
            }

            gvCart.Columns[7].Visible = isRestriction;

            lblShippingName.Text = r.Firstname + " " + r.Lastname;
            lblShipping1.Text = r.Addr1_s;
            lblShipping2.Text = r.Addr2_s;
            if (lblShipping2.Text.Trim() == "")lblShipping2.Visible = false; else lblShipping2.Text = lblShipping2.Text + "<br />";
            lblShipping3.Text = r.Addr3_s;
            if (lblShipping3.Text.Trim() == "") lblShipping3.Visible = false; else lblShipping3.Text = lblShipping3.Text + "<br />";
            lblShipping4.Text = r.City_s + ", " + r.State_s + " " + r.PostalIndex_s + ",  " + r.Country_s;
            lblShippingPhone.Text = r.Phone;
            lblShippingAltPhone.Text = r.AltPhone;
            if (lblShippingAltPhone.Text.Trim() == "") tr_altPhone.Visible = false;
            lblShippingFax.Text = r.Fax;
            if (lblShippingFax.Text.Trim() == "") tr_fax.Visible = false;

            if (r.CarrierAcct.Trim() != "")
            {
                lblCarrier.Visible = true;
                lblCarrier.Text = "<br /><b>" + "Your account for expediating this order is: Carrier - " + r.Carrier + " with account number - " + r.CarrierAcct + "</b>.<br />";
            }
            else
                lblCarrier.Visible = false;

            lblConfirmEmail.Text = "Confirmation of order will be sent to: " + r.Email;
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                lblConfirmEmail.Visible = r.EmailOrder;
            else
                lblConfirmEmail.Visible = Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false);

            for (int i = 0; i < c.AccessionIDs.Count; i++)
            {
                _itemNotes.Add(c.AccessionIDs[i].ToString(), "");
            }
            ViewState["item_notes"] = _itemNotes;
            if (!hasFootNote) lblFootNote.Visible = false;
            if (hasComments)
                pnlPlaceOrder.FindControl("ShowHideButton").Visible = true;
            else
                pnlPlaceOrder.FindControl("ShowHideButton").Visible = false;
        }

        protected void gvCart_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            int id = Toolkit.ToInt32(gvCart.DataKeys[e.RowIndex].Value, 0);
            Cart c = Cart.Current;
            c.RemoveAccession(id, false);
            c.Save();
            lblFootNote.Text = "";
            bindOrderData();
        }

        protected void btnRemoveAll_Click(object sender, EventArgs e) {
            Cart.Current.Empty();
            bindOrderData();
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (ddlUse.SelectedIndex == 0)
            {
                DVSelectUse = "";
            }
            else if (ddlUseSub.Items.Count > 0 && ddlUseSub.SelectedIndex == 0)
            {
                DVSelectSubUse = "";
            }
            else if (pnlPlanned.Visible && txtPlanned.Text.Trim() == "")
            {
                DVPlannedUse = "";
            }
            else
            {
                string coopEmails = "";
                string orderList = "";
                if (saveOrder(ref coopEmails, ref orderList))
                {
                    // display order confirmation page
                    displayOrder(true);

                    // send order confirmation e-mail
                    sendNewOrderEmail(coopEmails, orderList);
                }
            }
        }

        private bool saveOrder(ref string coopEmails, ref string orderList)
        {
            bool isAnonymous = UserManager.IsAnonymousUser(UserManager.GetUserName());

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true)))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    string ordertype = "DI"; // TODO: user could have a empty cart, only wants to know the information, "IO"

                    Cart c = Cart.Current;
                    Requestor r = Requestor.Current;
                    int wuserid = sd.WebUserID;

                    setItemNotes();
                    //string intended_code = ddlUse.SelectedItem.Text;
                    string intended_code = ddlUse.SelectedValue;
                    string intended_note = intendedUseNoteSave();
                    int webOrderID = 0;

                    //if (!isAnonymous)
                    //{
                        webOrderID = dm.Write(@"
                        insert into web_order_request
                        (web_cooperator_id, ordered_date, intended_use_code, intended_use_note, status_code,special_instruction, created_date, created_by, owned_date, owned_by)
                        values
                        (:wcoopid, :ordered_date, :intended_use_code, :intended_use_note, :status, :special, :createddate, :createdby, :owneddate, :ownedby)
                        ", true, "web_order_request_id", new DataParameters(
                         ":wcoopid", sd.WebCooperatorID, DbType.Int32,
                         ":ordered_date", DateTime.UtcNow, DbType.DateTime2,
                         ":intended_use_code", intended_code, DbType.String,
                         ":intended_use_note", intended_note, DbType.String,
                         ":status", "SUBMITTED", DbType.String,
                         ":special", txtSpecial.Text, DbType.String,
                         ":createddate", DateTime.UtcNow, DbType.DateTime2,
                         ":createdby", wuserid, DbType.Int32,
                         ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                         ":ownedby", wuserid, DbType.Int32));
                    //}

                    // The Email will combine all order items infor together, even though the specific order item may not be for one site.
                    int numbers = 1;
                    StringBuilder sb = new StringBuilder();
                    string CRLF = "\r\n";

                    var ds = sd.GetData("web_order_request_item_list", ":idlist=" + Toolkit.Join(c.AccessionIDs.ToArray(), ",", ""), 0, 0);
                    DataTable dt = ds.Tables["web_order_request_item_list"];
                    
                    int accessionid = 0;
                    string sitecode = "";
                    int inventoryid = 0;
                    string email = "";
                    string note = "";
                    string unitShip = "";
                    string distributionForm = "", orderedForm = "";
                    string externaltaxon = "";
                    string pi_number = "";
                    string restriction = "";
                    bool isSMTA = false;
                    string webOrderItemIDs = "";
                    int inventoryid_done = 0;  // multiple cooperators may need receive email notification at some site
                    int accessionid_done = 0;
                    string distributionForm_done = "";

                    foreach (DataRow dr in dt.Rows)
                    {
                        sitecode = dr["site"].ToString();
                        accessionid = Toolkit.ToInt32(dr["accession_id"].ToString(), 0);
                        inventoryid = Toolkit.ToInt32(dr["inventory_id"].ToString(), 0);
                        email = dr["email"].ToString();
                        unitShip = dr["distribution_unit_code"].ToString();
                        distributionForm = dr["form_type_code"].ToString();
                        orderedForm = c.FindByAccessionID(accessionid).DistributionFormCode; 
                        note = getItemNote(accessionid.ToString());
                        externaltaxon = dr["name"].ToString();
                        pi_number = dr["pi_number"].ToString();
                        restriction = dr["type_code"].ToString();
                        int webOrderItemID = 0;

                        if (distributionForm == orderedForm && inventoryid != inventoryid_done && !((accessionid == accessionid_done) && (distributionForm == distributionForm_done)))  // One material type per order
                        {
                            //if (!isAnonymous)
                            //{
                            webOrderItemID = dm.Write(@"
                            insert into web_order_request_item
                            (web_cooperator_id, web_order_request_id, sequence_number, accession_id, unit_of_shipped_code, distribution_form_code, status_code, user_note, created_date, created_by, owned_date, owned_by)
                            values
                            (:wcoopid, :weborderrequestid, :sequencenumber, :accessionid, :unitshipped, :distributionform, :statuscode, :usernote, :createddate, :wuserid, :owneddate, :wuserid2)
                            ", true, "web_order_request_item_id", new DataParameters(
                                ":wcoopid", sd.WebCooperatorID, DbType.Int32,
                                ":weborderrequestid", webOrderID, DbType.Int32,
                                ":sequencenumber", numbers, DbType.Int32,
                                ":accessionid", accessionid, DbType.Int32,
                                ":unitshipped", unitShip, DbType.String,
                                ":distributionform", distributionForm, DbType.String,
                                ":statuscode", "NEW", DbType.String,
                                ":usernote", note, DbType.String,
                                ":createddate", DateTime.UtcNow, DbType.DateTime2,
                                ":wuserid", wuserid, DbType.Int32,
                                ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                                ":wuserid2", wuserid, DbType.Int32));
                            //}

                            if (restriction == "MTA-SMTA")
                            {
                                isSMTA = true;
                                webOrderItemIDs = webOrderItemIDs + "; " + webOrderItemID.ToString();
                                sb.Append(" ").Append(numbers).Append("  ").Append(pi_number).Append(" - ").Append(sitecode).Append(" - ").Append(externaltaxon).Append(" (SMTA Material)").Append(CRLF);
                            }
                            else
                            {
                                sb.Append(" ").Append(numbers).Append("  ").Append(pi_number).Append(" - ").Append(sitecode).Append(" - ").Append(externaltaxon).Append(CRLF);
                            }
                            //if (!coopEmails.Contains(email))
                            //    coopEmails += email + ";";
                            numbers++;
                            inventoryid_done = inventoryid;
                            accessionid_done = accessionid;
                            distributionForm_done = distributionForm;
                        }
                        if (!coopEmails.Contains(email))
                            coopEmails += email + ";";
                    }
                    orderList = sb.ToString();
                    ViewState["webOrdersID"] = webOrderID;

                    if (isSMTA)
                    {
                        if (webOrderItemIDs.Length > 0) webOrderItemIDs = webOrderItemIDs.Remove(0, 2);
                        dm.Write(@"
                                insert into web_order_request_action
                                (web_order_request_id, action_code, acted_date, action_for_id, web_cooperator_id, created_date, created_by, owned_date, owned_by)
                                values
                                (:weborderrequestid, :actioncode, :acteddate, :actionforid, :wcoopid, :createddate, :wuserid, :owneddate, :wuserid2)
                                ", new DataParameters(
                                    ":weborderrequestid", webOrderID, DbType.Int32,
                                    ":actioncode", "SMTAACCEPT", DbType.String,
                                    ":acteddate", DateTime.UtcNow, DbType.DateTime2,
                                    ":actionforid", webOrderItemIDs, DbType.String,
                                    ":wcoopid", sd.WebCooperatorID, DbType.Int32,
                                    ":createddate", DateTime.UtcNow, DbType.DateTime2,
                                    ":wuserid", wuserid, DbType.Int32,
                                    ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                                    ":wuserid2", wuserid, DbType.Int32));
                    }

                     //if(!isAnonymous) 
                         dm.Write(@"
                            insert into web_order_request_address
                            (web_order_request_id,  address_line1, address_line2, address_line3, city, postal_index, geography_id, created_date, created_by, owned_date, owned_by)
                            values
                            (:worderid, :address1, :address2, :address3, :city, :postal, :geographyid, :createddate, :wuserid, :owneddate, :wuserid2)
                            ", new DataParameters(
                            ":worderid", webOrderID, DbType.Int32,
                            ":address1", r.Addr1_s, DbType.String,
                            ":address2", r.Addr2_s, DbType.String,
                            ":address3", r.Addr3_s, DbType.String,
                            ":city", r.City_s, DbType.String,
                            ":postal", r.PostalIndex_s, DbType.String,
                            ":geographyid", r.Geographyid_s, DbType.Int32,
                            ":createddate", DateTime.UtcNow, DbType.DateTime2,
                            ":wuserid", wuserid, DbType.Int32,
                            ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                            ":wuserid2", wuserid, DbType.Int32));

                    return true;
                }
            }
        }

        private void displayOrder(bool clearCart)
        {
            pnlPlaceOrder.Visible = false;
            pnlDisplayOrder.Visible = true;
            bindDisplayData(clearCart);
        }

        private void bindDisplayData(bool clearCart)
        {
            Requestor r = Requestor.Current;
            DataTable dt = null;
            Cart c = null;
            bool isRestriction = false;

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true))) 
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    if (clearCart)
                    {
                        //clear the cart and cart items records for the user if any
                        dm.Write(@"
                        delete from 
                            web_user_cart_item
                        where
                            web_user_cart_id in
                            (select web_user_cart_id from web_user_cart where cart_type_code = 'order items' and web_user_id = :wuseid)
                        ", new DataParameters(":wuseid", r.Webuserid, DbType.Int32));

                        dm.Write(@"
                        delete from 
                            web_user_cart 
                        where 
                            cart_type_code = 'order items' and web_user_id = :wuseid
                        ", new DataParameters(":wuseid", r.Webuserid, DbType.Int32));
                    }

                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        c = HttpContext.Current.Session["cart"] as Cart;
                        if (c != null) //view order confirmation, infor from session, so non-register can still see
                        {
                            dt = sd.GetData("web_order_request_item_display", ":orderrequestid=" + Toolkit.ToInt32((ViewState["webOrdersID"].ToString()), 0), 0, 0).Tables["web_order_request_item_display"];
                            lblOrderIDs.Text = ViewState["webOrdersID"].ToString();
                            // lblEmail.Text = r.Email;

                            // if (clearCart) Session["cart"] = null;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["type_code"].ToString().StartsWith("MTA-"))
                                {
                                    isRestriction = true;
                                    break;
                                }
                            }
                        }
                        else 
                        {
                        }
                    }

                    DataTable dt2 = dm.Read(@"
                    select intended_use_code, intended_use_note, special_instruction from web_order_request
                    where web_order_request_id = :orderrequestid",
                    new DataParameters(":orderrequestid", Int32.Parse(ViewState["webOrdersID"].ToString())
                    ));

                    if (dt2.Rows.Count > 0)
                    {
                        lblIntended.Text = dt2.Rows[0].ItemArray[0].ToString() + ": " + dt2.Rows[0].ItemArray[1].ToString();
                        lblSpecial.Text = dt2.Rows[0].ItemArray[2].ToString();
                    }

                    if (r.CarrierAcct.Trim() != "")
                    {
                        lblCarrierConfirm.Visible = true;
                        lblCarrierConfirm.Text = "<br /><b>" + "Your account for expediating this order:</b> <br/>" + "Carrier - " + r.Carrier + ", Account number - " + r.CarrierAcct;
                    }
                    else
                        lblCarrierConfirm.Visible = false;
                }
            }

            gvOrderItems.DataSource = dt;
            gvOrderItems.DataBind();
            gvOrderItems.Columns[6].Visible = isRestriction;
            if (clearCart) Session["cart"] = null;

            lblName.Text = r.Firstname + " " + r.Lastname;
            lblOrganization.Text = r.Organization;
            lblPhone.Text = "Phone: " + r.Phone;
            lblFax.Text = "  FAX: " + r.Fax;
            lblAdd1.Text = r.Addr1_s;
            lblAdd2.Text = r.Addr2_s;
            lblAdd3.Text = r.Addr3_s;
            lblAdd4.Text = r.City_s + ", " + r.State_s + " " + r.PostalIndex_s + ",  " + r.Country_s;

            lblEmail.Text = "Email confirmation of this order has been sent to: <b>" + r.Email + "</b>" + ". ";
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                lblEmail.Visible = r.EmailOrder;
            else
                lblEmail.Visible = Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false);
        }

        private void sendNewOrderEmail(string emailTo, string orders)
        {
            // construct e-mail content.
            Requestor r = Requestor.Current;

            string orderID = "";
            if (ViewState["webOrdersID"] != null)
                orderID = ViewState["webOrdersID"].ToString();

            StringBuilder sb = new StringBuilder();
            string CRLF = "\r\n";

            string custLine = Page.DisplayText("newOrderEmailCustLine", "");
            if (custLine != "") sb.Append(custLine).Append(CRLF).Append(CRLF);
           
            sb.Append("New order from the web (shopping cart).").Append(CRLF).Append(CRLF).Append(CRLF);
            sb.Append("Germplasm Request -  Order ID: ").Append(orderID).Append(CRLF).Append(CRLF);
            sb.Append("NAME: ").Append(r.Firstname).Append(" ").Append(r.Lastname).Append(CRLF);
            sb.Append(" ORG: ").Append(r.Organization).Append(CRLF);
            sb.Append("ADDR: ").Append(r.Addr1).Append(CRLF);
            if (!String.IsNullOrEmpty(r.Addr2)) sb.Append("      ").Append(r.Addr2).Append(CRLF);
            if (!String.IsNullOrEmpty(r.Addr3)) sb.Append("      ").Append(r.Addr3).Append(CRLF);
            sb.Append("      ").Append(r.City).Append(",").Append(r.State).Append("  ").Append(r.PostalIndex).Append(CRLF);
            if (r.Country != "United States") sb.Append("      ").Append(r.Country).Append(CRLF);
            sb.Append("PHONE: ").Append(r.Phone).Append(CRLF);
            if (!String.IsNullOrEmpty(r.Fax)) sb.Append(" FAX: ").Append(r.Fax).Append(CRLF);
            sb.Append("EMAIL: ").Append(r.Email).Append(CRLF).Append(CRLF);
            if (txtSpecial.Text.Trim() != "")
            {
                sb.Append("Instructions:").Append(CRLF);
                sb.Append("      ").Append(txtSpecial.Text.Trim()).Append(CRLF).Append(CRLF);
            }
            sb.Append("Intended use of material:").Append(CRLF);
            sb.Append("      ").Append(intendedUse()).Append(CRLF).Append(CRLF);   
            sb.Append("Items:").Append(CRLF);
            sb.Append(orders).Append(CRLF);
         
            if (r.CarrierAcct.Trim() != "")
            {
                sb.Append("User account for expediating this order:").Append(CRLF);
                sb.Append("      ").Append("Carrier - ").Append(r.Carrier).Append(", Account number - ").Append(r.CarrierAcct).Append(CRLF);
            }

            sb.Append(CRLF).Append("Order Received: ").Append(DateTime.Now);

            string cc = "";
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                cc = r.EmailOrder? r.Email: "";
            else
                cc = (Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false)) ? r.Email : "";
           
            string eSubject = Page.DisplayText("newOrderEmailSubject", "GRIN-GLOBAL - Public Order from Web");
            eSubject += " (Web Order Number " + orderID + ")";
            string adminEmailTo = Toolkit.GetSetting("EmailOrderTo", "");
            if (!String.IsNullOrEmpty(adminEmailTo)) emailTo = adminEmailTo;

            try
            {
                // 1. To curator(s)
                // 2. If requested, cc e-mail to the requestor
                Email.Send(emailTo,
                            Toolkit.GetSetting("EmailFrom", ""),
                            cc,
                            "",
                            eSubject,
                            sb.ToString());
            }
            catch (Exception ex)
            {
                string s = ex.Message; // debug
                Logger.LogTextForcefully("Application error: Sending email failed for new web order " + orderID + ". ", ex);
            }
        }

        protected void ddlUse_SelectedIndexChanged(object sender, EventArgs e) // TODO: need to use code_value table to hold value
        {
            //string selectedText = ddlUse.SelectedItem.Text;
            string selectedText = ddlUse.SelectedValue.ToLower();

            if (selectedText == "other")
            {
                txtOther.Visible = true;
                lblOther.Visible = true;
                ddlUseSub.Items.Clear();
                ddlUseSub.Visible = false;
                pnlPlanned.Visible = true;
                proceedToOrder();
            }
            else
            {
                txtOther.Text = "";
                txtOther.Visible = false;
                lblOther.Visible = false;
                switch (selectedText)
                {
                    case "- select -":
                        ddlUseSub.Visible = false;
                        DVSelectUse = "";
                        pnlPlanned.Visible = true;
                        break;
                    case "research":
                        ddlUseSub.Items.Clear();
                        ddlUseSub.Visible = true;
                        string[] listResearch = {"- Select -", "Botanical/taxonomic investigations",
                                        "Genetic studies", 
                                        "Entomological investigations", 
                                        "Plant Pathological investigations", 
                                        "Chemistry", 
                                        "Varietal Development", 
                                        "Bioremediation", 
                                        "Weed Science", 
                                        "Historical, cultural and anthropological research"};

                        for (int i = 0; i <= listResearch.Length - 1; i++)
                            ddlUseSub.Items.Add(listResearch[i]);
                        pnlPlanned.Visible = true;
                        break;
                    case "education":
                        ddlUseSub.Items.Clear();
                        ddlUseSub.Visible = true;
                        string[] listEducation = {"- Select -", "Public education, demonstrations",
                                        "Class instruction"};
                        for (int i = 0; i <= listEducation.Length - 1; i++)
                            ddlUseSub.Items.Add(listEducation[i]);
                        pnlPlanned.Visible = true;
                        break;
                    case "repatriation":
                        ddlUseSub.Items.Clear();
                        ddlUseSub.Visible = false;
                        pnlPlanned.Visible = false;
                        break;
                    case "home":
                        ddlUseSub.Items.Clear();
                        ddlUseSub.Visible = false;
                        string message = "The NPGS provides germplasm to support research and education objectives. Due to the intensive effort and resources required to ensure availability of germplasm for this purpose, we are unable to distribute it for home gardening or other purposes that can utilize readily available commercial cultivars.";
                        Extensions.Show(message);
                        pnlPlanned.Visible = false;
                        break;
                    default:
                        break;
                }

                if (selectedText == "home" || selectedText == "repatriation") proceedToOrder();
            }
        }

        public string getItemNote(string accessionID) 
        {
            string ret = "";
            if (_itemNotes.ContainsKey(accessionID))
            {
                ret = _itemNotes[accessionID];
            }
            return ret;
         }

        public void setItemNote(string accessionID, string value)
        {
            if (_itemNotes.ContainsKey(accessionID))
            {
                _itemNotes[accessionID] = value;
            }
        }

        private void setItemNotes()
        {
            var notes = hfItemNotes.Value;
            
            if (notes != null)
            {
                notes = notes.ToString().Replace("|||", "\t");
                string[] itemNotes = Regex.Split(notes.ToString(), "\t");

                string oneNote = "",noteKey = "", noteValue = "";
                int j = -1;
                for (int i = 0; i < itemNotes.Length-1; i++)
                {
                    oneNote = itemNotes[i];
                    j = oneNote.IndexOf(":");
                    noteKey = oneNote.Substring(3, j-3);
                    noteValue = oneNote.Substring(j, oneNote.Length - j).Remove(0,1);
                    _itemNotes.Add(noteKey, noteValue);
                }
            }
        }

        private string intendedUse()
        {
            string useType = ddlUse.SelectedItem.Text;
            return  useType + ": " + intendedUseNote();
        }

        private string intendedUseNote()
        {
            string useType = ddlUse.SelectedValue;
            string intended = ((ddlUseSub.Items.Count != 0) ? ddlUseSub.SelectedItem.Text : "") + (useType.ToUpper() == "OTHER" ? txtOther.Text : "");
            
            if (useType != "HOME" && useType != "REPATRIATION")
            {
                StringBuilder sb = new StringBuilder();
                string CRLF = "\r\n";

                sb.Append(CRLF).Append("Research use notes:").Append(CRLF);
                sb.Append("      ").Append(txtPlanned.Text);

                intended += sb.ToString();
            }

            return intended;
        }

        private string intendedUseNoteSave()
        {
            string useType = ddlUse.SelectedValue;
            string intended = ((ddlUseSub.Items.Count != 0) ? ddlUseSub.SelectedItem.Text : "") + (useType.ToUpper() == "OTHER" ? txtOther.Text : "");

            if (useType != "HOME" && useType != "REPATRIATION")
            {
                StringBuilder sb = new StringBuilder();
                //string CRLF = "\r\n";

                sb.Append(". ").Append("Research use notes - ").Append(txtPlanned.Text);

                intended += sb.ToString();
            }

            return intended;
        }

        bool isCu = false;
        bool isWeed = false;
        protected void gvCart_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex > -1)
            {

                var accessionID = Toolkit.ToInt32(((DataRowView)e.Row.DataItem)["accession_id"], -1);
                var lbl = e.Row.FindControl("lblFormDistributed") as Label;
                var c = Cart.Current;
                string distributionForm = c.FindByAccessionID(accessionID).DistributionFormCode;
                lbl.Text = CartItem.GetMaterialDescription(distributionForm);

                var lblSign1 = e.Row.FindControl("lblNoteSign1") as Label;
                var lblSign2 = e.Row.FindControl("lblNoteSign2") as Label;

                var lblAvailCmt = e.Row.FindControl("lblAvailCmt") as Label;
                var ibtnPlus = e.Row.FindControl("btnPlus") as ImageButton ;
                var ibtnMinus = e.Row.FindControl("btnMinus") as ImageButton;

                var lblFormD2 = e.Row.FindControl("lblFormD2") as Label;
                var ibtnPlusF = e.Row.FindControl("btnPlusF") as ImageButton;
                var ibtnMinusF = e.Row.FindControl("btnMinusF") as ImageButton;

                // display footnote
                //if (distributionForm == "CT")
                //{
                //    hasFootNote = true;
                //    string note = " You will receive <i>Unrooted cuttings</i> not <i>Rooted plants</i> unless specific arrangements have been made with the curator.";

                //     lblSign1.Visible = true;
                //    //lblSign1.ToolTip = note.Replace("<i>", "").Replace("</i>", "");

                //    lblFootNote.Visible = true;
                //    if (!isCu)
                //    {
                //        lblFootNote.Text += "&nbsp;<font color='red'><b><small>! </b></small></font>" + note + "<br />";
                //        isCu = true;
                //    }
                //}

                string noteCutting = "You will receive <i>Unrooted cuttings</i> not <i>Rooted plants</i> unless specific arrangements have been made with the curator.";
                if (distributionForm == "CT")
                {
                    hasComments = true;
                    lblFormD2.Visible = true;
                    e.Row.Cells[5].Width = 165;
                    lblFormD2.Text =  "<br />" + noteCutting;
                    ibtnMinusF.Visible = true;
                }

                //string statusCode = "", statusNote = "";
                //using (var sd = UserManager.GetSecureData(false))
                //{
                //    var dtAvail = sd.GetData("web_lookup_availability", ":accessionid=" + accessionID, 0, 0).Tables["web_lookup_availability"];

                //    foreach (DataRow dr in dtAvail.Rows)
                //    {
                //        statusCode = dr["availability_status_code"].ToString();
                //        statusNote = dr["availability_status_note"].ToString();

                //        if (statusCode == "WEED")
                //        {
                //            hasFootNote = true;
                //            var siteName = ((DataRowView)e.Row.DataItem)["site_code"].ToString();
                //            var siteID = ((DataRowView)e.Row.DataItem)["site_id"].ToString();

                //            string note = " <font color='red'><b>This accession is a restricted noxious weed</b></font>, contact <a href='site.aspx?id=" + siteID + "'>" + siteName + "</a> for availability.";

                //            lblSign2.Visible = true;
                //            lblFootNote.Visible = true;
                //            if (!isWeed)
                //            {
                //                lblFootNote.Text += "&nbsp;<font color='red'><b><small>+ </b></small></font>" + note + "<br />";
                //                isWeed = true;
                //            }
                //            break;
                //        }
                //    }
                //}

                using (var sd = UserManager.GetSecureData(true))
                {
                    var dtAvilCmt = sd.GetData("web_accessiondetail_action_note", ":actionName=" + "''AVAIL_CMT', 'SMTA''" + ";:accessionid=" + accessionID, 0, 0).Tables["web_accessiondetail_action_note"];

                     if (dtAvilCmt.Rows.Count > 0)
                    {
                        hasComments = true;
                        lblAvailCmt.Visible = true;
                        e.Row.Cells[1].Width = 200;
                        lblAvailCmt.Text = "<br />" + dtAvilCmt.Rows[0]["note"].ToString();
                        ibtnMinus.Visible = true;
                    }
                }

                if (e.Row.Cells[7].Text  == "MTA-SMTA")
                    e.Row.Cells[7].BackColor = System.Drawing.Color.Yellow;
            }
        }

        protected void gvOrderItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex > -1)
            {

                var accessionID = Toolkit.ToInt32(((DataRowView)e.Row.DataItem)["accession_id"], -1);
                var lbl = e.Row.FindControl("lblFormDistributed") as Label;
                var c = Cart.Current;
                //lbl.Text = c.FindByAccessionID(accessionID).DistributionFormCode;
                lbl.Text = CartItem.GetMaterialDescription(c.FindByAccessionID(accessionID).DistributionFormCode);
            }
        }

        protected void cbStatement_CheckedChanged(object sender, EventArgs e)
        {
            //if (cbStatement.Checked == true)
            //{
            //    lblReadStatement.Visible = false;
            //}
            //else
            //{
            //    lblReadStatement.Visible = true;
            //}

            //proceedToOrder();
        }

        private void populateDropDown(DropDownList ddl, string groupName)
        {
            DataTable dt = Utils.GetCodeValue(groupName, "- Select -");

            if (dt.Rows.Count > 0)
            {
                ddl.DataValueField = "Value";
                ddl.DataTextField = "Text";
                ddl.DataSource = dt;
                ddl.DataBind();
            }
        }

        private void proceedToOrder()
        {
            if ((cbStatement.Checked) && (rblSMTA.SelectedIndex != -1 || !pnlSMTA.Visible))
            {
                btnProcess.Enabled = true;
                btnProcess.ToolTip = "";
            }
            else
            {
                btnProcess.Enabled = false;
                btnProcess.ToolTip = Page.GetDisplayMember("Order", "answer", "Please answer all questions on the form in order to proceed");
            }
        }

        protected void rblSMTA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblSMTA.SelectedIndex == -1)
            {
                lblSMTA.Visible = true;
            }
            else
            {
                lblSMTA.Visible = false;
                if (rblSMTA.SelectedValue == "NO")
                { // Delete the SMTA accession from the cart/order
                    Cart c = Cart.Current;
                    Favorite f = Favorite.Current;
                    using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                    {
                        var ds = sd.GetData("web_cartview", ":idlist=" + Toolkit.Join(c.AccessionIDs.ToArray(), ",", ""), 0, 0);
                        var dt = ds.Tables["web_cartview"];

                        bool changed = false;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int id = Toolkit.ToInt32(dt.Rows[i]["accession_id"].ToString(), 0);
                            
                            if (dt.Rows[i]["type_code"].ToString() == "MTA-SMTA")
                            {
                                c.RemoveAccession(id, false);
                                f.AddAccession(id, null);

                                changed = true;
                            }
                        }
                        if (changed)
                        {
                            c.Save();
                            f.Save();

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int id = Toolkit.ToInt32(dt.Rows[i]["accession_id"].ToString(), 0);

                                if (dt.Rows[i]["type_code"].ToString() == "MTA-SMTA")
                                {
                                    // Special hidden usage 
                                    using (DataManager dm = sd.BeginProcessing(true))
                                    {
                                        int wuserid = sd.WebUserID;
                                        int cartId = 0;
                                        cartId = Toolkit.ToInt32(dm.ReadValue(@"
                                        select web_user_cart_id  from web_user_cart where cart_type_code = 'favorites' and web_user_id = :wuserid",
                                            new DataParameters(":wuserid", wuserid, DbType.Int32)), 0);

                                        dm.Write(@"update web_user_cart_item set usage_code = 'SMTA-NO' where web_user_cart_id = :cartid and accession_id = :aid",
                                            new DataParameters(":cartid", cartId, DbType.Int32, ":aid", id, DbType.Int32));
                                    }
                                }
                            }


                            bindOrderData();

                            if (c.AccessionIDs.Count == 0) btnProcess.Visible = false;
                        }

                    }
                }
            }
            proceedToOrder();
        }

        protected void ddlUseSub_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string selectedText = ddlUseSub.SelectedItem.Text;

            if (selectedText == "- Select -")
            {
                DVSelectSubUse = "";
            }
            else
            {
                DVSelectSubUse = "hide";
                proceedToOrder();
            }
        }

        protected void gvCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument.ToString() != "")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvCart.Rows[index];

                var lblAvailCmt = row.FindControl("lblAvailCmt") as Label;
                var ibtnPlus = row.FindControl("btnPlus") as ImageButton;
                var ibtnMinus = row.FindControl("btnMinus") as ImageButton;

                var lblFormD2 = row.FindControl("lblFormD2") as Label;
                var ibtnPlusF = row.FindControl("btnPlusF") as ImageButton;
                var ibtnMinusF = row.FindControl("btnMinusF") as ImageButton;

                switch (e.CommandName)
                {
                    case "HideNote":
                        lblAvailCmt.Visible = false;
                        ibtnPlus.Visible = true;
                        ibtnMinus.Visible = false;
                        break;

                    case "ShowNote":
                        lblAvailCmt.Visible = true;
                        ibtnPlus.Visible = false;
                        ibtnMinus.Visible = true;
                        break;

                    case "HideNoteF":
                        lblFormD2.Visible = false;
                        ibtnPlusF.Visible = true;
                        ibtnMinusF.Visible = false;
                        break;

                    case "ShowNoteF":
                        lblFormD2.Visible = true;
                        ibtnPlusF.Visible = false;
                        ibtnMinusF.Visible = true;
                        break;

                    default:
                        break;
                }
            }
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvCart.Rows)
            {
                var lblAvailCmt = row.FindControl("lblAvailCmt") as Label;
                var ibtnPlus = row.FindControl("btnPlus") as ImageButton;
                var ibtnMinus = row.FindControl("btnMinus") as ImageButton;

                var lblFormD2 = row.FindControl("lblFormD2") as Label;
                var ibtnPlusF = row.FindControl("btnPlusF") as ImageButton;
                var ibtnMinusF = row.FindControl("btnMinusF") as ImageButton;

                if (lblAvailCmt.Text != "")
                {
                    lblAvailCmt.Visible = true;
                    ibtnPlus.Visible = false;
                    ibtnMinus.Visible = true;
                }

                if (lblFormD2.Text != "")
                {
                    lblFormD2.Visible = true;
                    ibtnPlusF.Visible = false;
                    ibtnMinusF.Visible = true;
                }
            }
        }

        protected void btnHideAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvCart.Rows)
            {
                var lblAvailCmt = row.FindControl("lblAvailCmt") as Label;
                var ibtnPlus = row.FindControl("btnPlus") as ImageButton;
                var ibtnMinus = row.FindControl("btnMinus") as ImageButton;

                var lblFormD2 = row.FindControl("lblFormD2") as Label;
                var ibtnPlusF = row.FindControl("btnPlusF") as ImageButton;
                var ibtnMinusF = row.FindControl("btnMinusF") as ImageButton;

                if (lblAvailCmt.Text != "")
                {
                    lblAvailCmt.Visible = false;
                    ibtnPlus.Visible = true;
                    ibtnMinus.Visible = false;
                }

                if (lblFormD2.Text != "")
                {
                    lblFormD2.Visible = false;
                    ibtnPlusF.Visible = true;
                    ibtnMinusF.Visible = false;
                }
            }
        }
    }
}
