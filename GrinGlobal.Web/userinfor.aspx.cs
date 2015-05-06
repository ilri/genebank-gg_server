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

namespace GrinGlobal.Web
{
    public partial class UserInfor : System.Web.UI.Page
    {
        string _action = string.Empty;
        string _level = string.Empty;
        static string _prevPage = String.Empty;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                Response.Redirect("~/login.aspx?action=menu");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["action"] != null)
            {
                _action = Request.QueryString["action"];  
            }
            if (Request.QueryString["level"] != null)
            {
                _level = Request.QueryString["level"];
            }

            // If user not logged in yet clicking "My Account", redirect to the login page:
            bool isAnonymous = UserManager.IsAnonymousUser(UserManager.GetUserName());
            if (_action == "menu" && isAnonymous)
                Response.Redirect("~/Login.aspx?action=menu");
            
            if( !IsPostBack )
            {
                _prevPage = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();

                if (Session["newRegistration"] != null)
                {
                    Cart c = Cart.Current;
                    c.Save();
                    Session["newRegistration"] = null;
                }
                showViews();
            }
        }

        protected void cbSameAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSameAddress.Checked)
            {
                txtShipAddr1.Text = txtAddr1.Text;
                txtShipAddr2.Text = txtAddr2.Text;
                txtShipAddr3.Text = txtAddr3.Text;
                txtShipCity.Text = txtCity.Text;

                txtShipZip.Text = txtZip.Text;
                ddlShipCountry.SelectedValue = ddlCountry.SelectedValue;
                populateStateList("ship");
                ddlShipState.SelectedValue = ddlState.SelectedValue;

                txtAddressName.Text = "Organization Address";
                txtAddressName.Enabled = false;

                //ddlShipping.SelectedIndex = 0;
            }
            else
                clearShippingAddress();        
        }

        protected void btnCooperator_Click(object sender, EventArgs e)
        {
            string title = ddlTitle.SelectedItem.Text;
            string firstname = txtFirstname.Text.Trim();
            string lastname = txtLastname.Text.Trim();
            string organization = txtOrganization.Text.Trim();
            string addr1 = txtAddr1.Text.Trim();
            string addr2 = txtAddr2.Text.Trim();
            string addr3 = txtAddr3.Text.Trim();
            string admin1 = txtCity.Text.Trim();
            string admin2 = txtZip.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string altphone = txtAltPhone.Text.Trim();
            string fax = txtFax.Text.Trim();
            string email = txtEmail.Text.Trim();
            string note = txtNote.Text.Trim();
            int geographyid = Int32.Parse(ddlState.SelectedValue);
            string state = ddlState.SelectedItem.Text;
            string country = ddlCountry.SelectedItem.Text;
            bool emailorder = cbEmailOrder.Checked;
            bool emailshipping = cbEmailShipping.Checked;
            bool emailnews = cbEmailInfor.Checked;

            int webuid = 0;
            int newCoopID = 0;
            bool needRefresh = false;
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
                using (DataManager dm = sd.BeginProcessing(true))
                {
                    if (geographyid == 0) // No State/Province needed, only country data
                        geographyid = Utils.GetGeographyID(ddlCountry.SelectedValue);

                    if (_level != "nologin")
                    {
                        webuid = sd.WebUserID;

                        int coopID = Toolkit.ToInt32(dm.ReadValue(@"
                                select 
                                    web_cooperator_id 
                                from 
                                    web_user
                                where 
                                    web_user_id = :webuserid
                                ", new DataParameters(":webuserid", webuid, DbType.Int32)), -1);

                        if (coopID < 0)
                        {
                            //insert into web_cooperator
                            newCoopID = dm.Write(@"
                            insert into web_cooperator
                            (title, last_name, first_name, organization, address_line1, address_line2, address_line3, city,postal_index, geography_id, primary_phone, secondary_phone, fax, email, is_active, note, created_date, created_by, owned_date, owned_by)
                            values
                            (:title, :lastname, :firstname, :organization, :address1, :address2, :address3, :admin1, :admin2, :geographyid, :phone, :altphone, :fax, :email, :isActive, :note, :createddate, :createdby, :owneddate, :ownedby)
                            ", true, "web_cooperator_id", new DataParameters(
                            ":title", title, DbType.String,
                            ":lastname", lastname, DbType.String,
                            ":firstname", firstname, DbType.String,
                            ":organization", organization, DbType.String,
                            ":address1", addr1, DbType.String,
                            ":address2", addr2, DbType.String,
                            ":address3", addr3, DbType.String,
                            ":admin1", admin1, DbType.String,
                            ":admin2", admin2, DbType.String,
                            ":geographyid", geographyid, DbType.Int32,
                            ":phone", phone, DbType.String,
                            ":altphone", altphone, DbType.String,
                            ":fax", fax, DbType.String,
                            ":email", email, DbType.String,
                            ":isActive", "Y", DbType.String,
                            ":note", note, DbType.String,
                            ":createddate", DateTime.UtcNow, DbType.DateTime2,
                            ":createdby", webuid, DbType.Int32,
                            ":owneddate", DateTime.UtcNow, DbType.DateTime2,
                            ":ownedby", webuid, DbType.Int32
                            ));

                            dm.Write(@"
                            update web_user
                            set    
                                web_cooperator_id = :coopid,
                                modified_date = :now
                            where
                                web_user_id = :wuserid
                            ", new DataParameters(
                            ":coopid", newCoopID, DbType.Int32,
                            ":now", DateTime.UtcNow, DbType.DateTime2,
                            ":wuserid", webuid, DbType.Int32
                            ));
                            needRefresh = true; // new token for new web coop id
                        }
                        else
                        {
                            //update
                            dm.Write(@"
                            update web_cooperator
                            set    
                                title = :title,
                                last_name = :lastname,
                                first_name = :firstname,
                                organization = :organization,
                                address_line1 = :address1,
                                address_line2 = :address2,
                                address_line3 = :address3,
                                city = :admin1,
                                postal_index = :admin2,
                                geography_id = :geographyid,
                                primary_phone = :phone,
                                secondary_phone = :altphone,
                                fax = :fax,
                                email = :email,
                                note = :note,
                                modified_date = :modifieddate,
                                modified_by   = :modifiedby
                             where
                                web_cooperator_id = :coopid
                                ", new DataParameters(
                                ":title", title, DbType.String,
                                ":lastname", lastname, DbType.String,
                                ":firstname", firstname, DbType.String,
                                ":organization", organization, DbType.String,
                                ":address1", addr1, DbType.String,
                                ":address2", addr2, DbType.String,
                                ":address3", addr3, DbType.String,
                                ":admin1", admin1, DbType.String,
                                ":admin2", admin2, DbType.String,
                                ":geographyid", geographyid, DbType.Int32,
                                ":phone", phone, DbType.String,
                                ":altphone", altphone, DbType.String,
                                ":fax", fax, DbType.String,
                                ":email", email, DbType.String,
                                ":note", note, DbType.String,
                                ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                                ":modifiedby", webuid,
                                ":coopid", coopID
                                ));
                            newCoopID = coopID;
                        }
                    }
                    else
                    {
                    }
                }
            }

            if (_level != "nologin")
            {
                UserManager.SaveUserPref("EmailOrder", emailorder.ToString());
                UserManager.SaveUserPref("EmailShipping", emailshipping.ToString());
                UserManager.SaveUserPref("EmailNews", emailnews.ToString());
                saveShippingAddress();
            }

            // Anonymous user saves some infor in session
            string addr1_s = txtShipAddr1.Text.Trim();
            string addr2_s = txtShipAddr2.Text.Trim();
            string addr3_s = txtShipAddr3.Text.Trim();
            string city_s = txtShipCity.Text.Trim();
            string postalIndex_s = txtShipZip.Text.Trim();
            int geographyid_s = Int32.Parse(ddlShipState.SelectedValue);
            string state_s = ddlShipState.SelectedItem.Text;
            string country_s = ddlShipCountry.SelectedItem.Text;
            string carrier = ddlCarrier.SelectedItem.Text;
            string carrierAcct = txtCarrierAcct.Text.Trim();

            if (geographyid_s == 0) // No State/Province needed, only country data
                geographyid_s = Utils.GetGeographyID(ddlShipCountry.SelectedValue);

            Requestor requestor = new Requestor(newCoopID, title, firstname, lastname, organization, addr1, addr2, addr3, admin1, admin2, geographyid, phone, altphone, fax, email, note, state,country, addr1_s, addr2_s, addr3_s, city_s, postalIndex_s, geographyid_s,  state_s, country_s, carrier, carrierAcct, emailorder, emailshipping, emailnews);
            requestor.Webuserid = webuid;
            Session["requestor"] = requestor;
  
            mv2.SetActiveView(vwDisplayInfor);
            displayInfor();
            if (_level == "nologin") tUserAcct2.Visible = false;

            if (needRefresh)
            {
                using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
                {
                    using (DataManager dm = sd.BeginProcessing(true))
                    {
                        string username = UserManager.GetUserName();
                        string token = UserManager.ValidateLogin(username, "string", true, false);
                        UserManager.SaveLoginCookieAndRedirect(username, token, true, "~/userinfor.aspx?action=view", null);
                    }
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (_action == "checkout" || btnOK.Text == "Continue Check Out")
            {
                Response.Redirect("~/order.aspx?action=checkout");
            }
        }
 
        private void displayUserAcct(string type)
        {
            string userName = UserManager.GetUserName();
            if (!UserManager.IsAnonymousUser(userName))
            {
                if (type == "edit")
                {
                    lblUserName.Text = userName;
                    lblPassword.Text = "****";
                }
                else
                {
                    lblUserName2.Text = userName;
                    lblPassword2.Text = "****";
                }
            }
            else
            {
                tUserAcct.Visible = false;
                tUserAcct2.Visible = false;
            }
        }

        private void displayInforEdit()
        {
            displayUserAcct("edit");

            Requestor r = Requestor.Current;
            int gid = r.Geographyid;

            populateCountryList();
            populateDropDown(ddlTitle, "COOPERATOR_TITLE");

            ddlTitle.SelectedIndex = ddlTitle.Items.IndexOf(ddlTitle.Items.FindByText(r.Title));
            txtFirstname.Text = r.Firstname;
            txtLastname.Text = r.Lastname;
            txtOrganization.Text = r.Organization;
            txtAddr1.Text = r.Addr1;
            txtAddr2.Text = r.Addr2;
            txtAddr3.Text = r.Addr3;
            txtCity.Text = r.City;
            txtZip.Text = r.PostalIndex;
            txtPhone.Text = r.Phone;
            txtAltPhone.Text = r.AltPhone;
            txtFax.Text = r.Fax;
            if (r.Email == "" || r.Email == null)
            {
                if (_level != "nologin") txtEmail.Text = UserManager.GetUserName();
            }
            else
                txtEmail.Text = r.Email;
            if (_level == "nologin") txtEmail.Enabled = true;
            txtNote.Text = r.Note;

            if (UserManager.IsAnonymousUser(UserManager.GetUserName()) || r.WebCoopid == 0)
            {
                cbEmailOrder.Checked = r.EmailOrder;
                cbEmailShipping.Checked = r.EmailShipping;
                cbEmailInfor.Checked = r.EmailNews;
            }
            else
            {
                cbEmailOrder.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false);
                cbEmailShipping.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailShipping"), false);
                cbEmailInfor.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailNews"), false);
            }

            ListItem item1 = ddlCountry.Items.FindByText(r.Country);
            if (item1 != null)
            {
                ddlCountry.SelectedItem.Selected = false;
                item1.Selected = true;
            }

            populateStateList("org");
            ListItem item2 = ddlState.Items.FindByText(r.State);
            if (item2 != null)
            {
                ddlState.SelectedItem.Selected = false;
                item2.Selected = true;
            }

            populateStateList("ship");
            if (UserManager.HasWebAddress())
            {
                lblUseAddrBook.Visible = true;
                ddlShipping.Visible = true;
                populateShippingAddressList(r.Webuserid);

                DataTable dt = UserManager.GetDefaultShippingAddress();
                if (dt.Rows.Count > 0)
                {
                    string defaultShippingID =  dt.Rows[0].ItemArray[0].ToString();
                    ListItem item3 = ddlShipping.Items.FindByValue(defaultShippingID);
                    if (item3 != null)
                    {
                        ddlShipping.SelectedItem.Selected = false;
                        item3.Selected = true;
                        fillShippingAddress(dt);
                    }
                }
            }

            ddlCarrier.SelectedValue = r.Carrier;
            txtCarrierAcct.Text = r.CarrierAcct;
            this.Form.DefaultButton = btnCooperator.UniqueID;
        }

        private void showViews()
        {
            if (_level == "nologin")
            {
                mv2.SetActiveView(vwEnterInfor);
                displayInforEdit();
                tUserAcct.Visible = false;
            }
            else
            {
                switch (_action)
                {
                    case "editInfor":
                        mv2.SetActiveView(vwEnterInfor);
                        displayInforEdit();
                        btnCooperator.Text = "Update";
                        break;
                    case "menu":
                        mv2.SetActiveView(vwDisplayInfor);
                        displayInfor();
                        break;
                    case "checkout":
                        Requestor r = Requestor.Current;
                        if (r.WebCoopid <  0) // level 1 user
                        {
                            mv2.SetActiveView(vwEnterInfor);
                            displayInforEdit();
                        }
                        else
                        {
                            mv2.SetActiveView(vwDisplayInfor);
                            displayInfor();
                            lblVerify.Visible = true;
                        }
                        break;
                    case "view":
                        mv2.SetActiveView(vwDisplayInfor);
                        displayInfor();
                        break;
                    default:
                        mv2.SetActiveView(vwEnterInfor);
                        displayInforEdit();
                        break;
                }
            }
        }
        
        private void displayInfor()
        {
            displayUserAcct("view");

            Requestor r = Requestor.Current;
            Cart c = Cart.Current;

            lblTitle.Text = r.Title;
            lblFirstName.Text = r.Firstname;
            lblLastName.Text = r.Lastname;
            lblOrganization.Text = r.Organization;
            lblAddr1.Text = r.Addr1;
            lblAddr2.Text = r.Addr2;
            lblAddr3.Text = r.Addr3;
            lblCity.Text = r.City;
            lblState.Text = r.State;
            lblZip.Text = r.PostalIndex;
            lblCountry.Text = r.Country;
            lblPhone.Text = r.Phone;
            lblAltPhone.Text = r.AltPhone;
            lblFax.Text = r.Fax;
            lblEmail.Text = r.Email;
            lblNote.Text = r.Note;

            lblShipAddr1.Text = r.Addr1_s;
            lblShipAddr2.Text = r.Addr2_s;
            lblShipAddr3.Text = r.Addr3_s;
            lblShipCity.Text = r.City_s;
            lblShipState.Text = r.State_s;
            lblShipZip.Text = r.PostalIndex_s;
            lblShipCountry.Text = r.Country_s;

            if (((c.Accessions.Length != 0) && (r.WebCoopid != 0)) ||  ((c.Accessions.Length != 0) && (_level =="nologin")))
            {
                btnOK.Text = "Continue Check Out";
                btnOK.Visible = true;
            }
            else
                btnOK.Visible = false;

            if (UserManager.IsAnonymousUser(UserManager.GetUserName()) || r.WebCoopid == 0)
            {
                cbEmail1.Checked = r.EmailOrder;
                cbShipping1.Checked = r.EmailShipping;
                cbEmail2.Checked = r.EmailNews;
            }
            else
            {
                cbEmail1.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailOrder"), false);
                cbShipping1.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailShipping"), false);
                cbEmail2.Checked = Toolkit.ToBoolean(UserManager.GetUserPref("EmailNews"), false);
            }

            lblCarrier.Text = r.Carrier;
            lblCarrierAcct.Text = r.CarrierAcct;

            //hlOrderHistory.NavigateUrl = "OrderHistory.aspx?id=" + r.WebCoopid;
            //hlUserPref.NavigateUrl = "UserPref.aspx?id=" + r.Webuserid;
        }

        private void populateCountryList()
        {
            DataTable dt = Utils.GetCountryList();

            if (dt.Rows.Count > 0)
            {
                ddlCountry.DataValueField = "countrycode";
                ddlCountry.DataTextField = "Countryname";
                ddlCountry.DataSource = dt;
                ddlCountry.DataBind();
                ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByText("United States"));

                ddlShipCountry.DataValueField = "countrycode";
                ddlShipCountry.DataTextField = "Countryname";
                ddlShipCountry.DataSource = dt;
                ddlShipCountry.DataBind();
                ddlShipCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByText("United States"));
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateStateList("org");
        }

        protected void ddlShipCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateStateList("ship");
            changeShippingAddress();
        }

        private void populateStateList(string type)
        {
            string countryCode;
            if (type == "org")
                countryCode = ddlCountry.SelectedValue;
            else
                countryCode = ddlShipCountry.SelectedValue;

            DataTable dt = Utils.GetStateList(countryCode);
            if (dt.Rows.Count > 0)
            {
                if (type == "org")
                {
                    ddlState.DataValueField = "gid";
                    ddlState.DataTextField = "stateName";
                    ddlState.DataSource = dt;
                    ddlState.DataBind();
                    ddlState.Visible = true;
                }

                if (type == "ship")
                {
                    ddlShipState.DataValueField = "gid";
                    ddlShipState.DataTextField = "stateName";
                    ddlShipState.DataSource = dt;
                    ddlShipState.DataBind();
                    ddlShipState.Visible = true;
                }
            }
            
            if (dt.Rows.Count == 1)
            {
                if (type == "org")
                     ddlState.Visible = false;
 
                if (type == "ship")
                     ddlShipState.Visible = false;
             }

          }

        private void populateStateList() 
        {
            populateStateList("org");
            populateStateList("ship");
        }

        protected void btnCoopCancel_Click(object sender, EventArgs e)
        {
            if (_prevPage.EndsWith("UserAcct.aspx")) Response.Redirect("~/Search.aspx");
            Response.Redirect(_prevPage);
        }

        private void populateShippingAddressList(int wuserid)
        {
            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken()))
            {
//                using (DataManager dm = sd.BeginProcessing(true))
//                {
//                    DataTable dt = dm.Read(@"
//                    select 0 as wusaid, '' as address_name 
//                    union select web_user_shipping_address_id as wusaid, address_name as address_name 
//                    from web_user_shipping_address
//                    where web_user_id = :wuserid 
//                    order by address_name",
//                    new DataParameters(":wuserid", wuserid
//                    ));

                    DataTable dt = sd.GetData("web_user_shipping_address", ":wuserid=" + wuserid, 0, 0).Tables["web_user_shipping_address"];

                    if (dt.Rows.Count > 0)
                    {
                        ddlShipping.DataValueField = "wusaid";
                        ddlShipping.DataTextField = "address_name";
                        ddlShipping.DataSource = dt;
                        ddlShipping.DataBind();
                    }
                //}
            }
        }

        protected void ddlShipping_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = UserManager.GetShippingAddress(Toolkit.ToInt32(ddlShipping.SelectedValue));
            fillShippingAddress(dt);
            cbSameAddress.Checked = false;
        }

        private void fillShippingAddress(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                txtShipAddr1.Text = dr["address_line1"].ToString();
                txtShipAddr2.Text = dr["address_line2"].ToString();
                txtShipAddr3.Text = dr["address_line3"].ToString();
                txtShipCity.Text = dr["city"].ToString();
                txtShipZip.Text = dr["postal_index"].ToString();
                txtAddressName.Text = dr["address_name"].ToString();

                string countrycode = dr["country_code"].ToString();
                string statename = dr["state_name"].ToString();

                ddlShipCountry.SelectedIndex = ddlShipCountry.Items.IndexOf(ddlShipCountry.Items.FindByValue(countrycode));

                populateStateList("ship");
                ddlShipState.SelectedIndex = ddlShipState.Items.IndexOf(ddlShipState.Items.FindByText(statename));

                if (dr["address_name"].ToString() == "Organization Address")
                    cbSameAddress.Checked = true;
                else
                    cbSameAddress.Checked = false;

                txtAddressName.Enabled = false;
            }
            if (dt.Rows.Count == 0) clearShippingAddress();
        }

        private void changeShippingAddress()
        {
            cbSameAddress.Checked = false;
            txtAddressName.Enabled = true;
        }

         protected void txtShipAddr1_TextChanged(object sender, EventArgs e)
        {
            changeShippingAddress();
        }

        protected void txtShipAddr2_TextChanged(object sender, EventArgs e)
        {
            changeShippingAddress();
        }

        protected void txtShipAddr3_TextChanged(object sender, EventArgs e)
        {
            changeShippingAddress();
        }

        protected void txtShipCity_TextChanged(object sender, EventArgs e)
        {
            changeShippingAddress();
        }

        protected void ddlShipState_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeShippingAddress();
        }

        protected void txtShipZip_TextChanged(object sender, EventArgs e)
        {
            changeShippingAddress();
        }

        private void saveShippingAddress()
        {
            string addrName = txtAddressName.Text.Trim();
            string addr1 = txtShipAddr1.Text.Trim();
            string addr2 = txtShipAddr2.Text.Trim();
            string addr3 = txtShipAddr3.Text.Trim();
            string city = txtShipCity.Text.Trim();
            string zip = txtShipZip.Text.Trim();
            int geographyid = Int32.Parse(ddlShipState.SelectedValue);
            string carrier = ddlCarrier.SelectedValue;
            string carrierAcct = txtCarrierAcct.Text.Trim();


            if (geographyid == 0) // No State/Province needed, only country data
                geographyid = Utils.GetGeographyID(ddlShipCountry.SelectedValue);

            UserManager.SaveShippingAddress(addrName, addr1, addr2, addr3, city, zip,geographyid, true);
        }

        private void clearShippingAddress()
        {
            txtAddressName.Text = "";
            txtAddressName.Enabled = true;
            txtShipAddr1.Text = "";
            txtShipAddr2.Text = "";
            txtShipAddr3.Text = "";
            txtShipCity.Text = "";
            ddlShipState.SelectedIndex = 0;
            txtShipZip.Text = "";
            ddlShipCountry.SelectedIndex = ddlShipCountry.Items.IndexOf(ddlShipCountry.Items.FindByText("United States"));
        }

        private void populateDropDown(DropDownList ddl, string groupName)
        {
            DataTable dt = Utils.GetCodeValue(groupName, "");

            if (dt.Rows.Count > 0)
            {
                ddl.DataValueField = "Value";
                ddl.DataTextField = "Value";
                ddl.DataSource = dt;
                ddl.DataBind();
            }
        }

    }
}
