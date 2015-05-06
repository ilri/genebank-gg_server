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
    public partial class UserAddress : System.Web.UI.Page
    {
        string _action = string.Empty;
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

            if (!IsPostBack)
            {
                showViews();
            }
        }

        private void showViews()
        {
            switch (_action)
            {
                case "addNew":
                    addNewAddressView();
                    break;
                case "EditAddress":
                    if (Request.QueryString["addrid"] != null)
                    {
                        editAddressView(Toolkit.ToInt32(Request.QueryString["addrid"].ToString(),0));
                    }
                     break;
                default:
                     displayAddressView();
                     break;
            }
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
            }
        }

        private void populateCountryList(string selectdCountry)
        {
            populateCountryList();
            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByText(selectdCountry));
        }

        private void populateStateList()
        {
            string countryCode = ddlCountry.SelectedValue;
            DataTable dt = Utils.GetStateList(countryCode);

            if (dt.Rows.Count > 1)
            {
                {
                    ddlState.Visible = true;
                    ddlState.DataValueField = "gid";
                    ddlState.DataTextField = "stateName";
                    ddlState.DataSource = dt;
                    ddlState.DataBind();
                }
            }
            else
                ddlState.Visible = false;
        }

        private void addNewAddressView()
        {
            mvAddress.SetActiveView(vwAddEdit);
            lblAddEdit.Text = "Add a new shipping address";
            txtAddrName.Text = "";
            txtAddr1.Text = "";
            txtAddr2.Text = "";
            txtAddr3.Text = "";
            txtAddrCity.Text = "";
            txtAddrZip.Text = "";            
            populateCountryList("United States");
            populateStateList();
        }

        private void editAddressView(int addrid)
        {
            mvAddress.SetActiveView(vwAddEdit);
            lblAddEdit.Text = "Edit the shipping address";
            DataTable dt = UserManager.GetShippingAddress(addrid);
            foreach (DataRow dr in dt.Rows)
            {
                txtAddrName.Text = dr["address_name"].ToString();
                txtAddr1.Text = dr["address_line1"].ToString();
                txtAddr2.Text = dr["address_line2"].ToString();
                txtAddr3.Text = dr["address_line3"].ToString();
                txtAddrCity.Text = dr["city"].ToString();
                txtAddrZip.Text = dr["postal_index"].ToString();

                string countrycode = dr["country_code"].ToString();
                string statename = dr["state_name"].ToString();
                populateCountryList();
                ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(countrycode));
                populateStateList();
                ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByText(statename));
            }
        }

        private void displayAddressView()
        {
            mvAddress.SetActiveView(vwDisplay);
            DataTable dt = UserManager.GetShippingAddress();
            gvAddress.DataSource = dt;
            gvAddress.DataBind();
        }
        
        protected void btnEnterNew_Click(object sender, EventArgs e)
        {
            addNewAddressView();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateStateList();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvAddress.SetActiveView(vwDisplay);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string addrName = txtAddrName.Text.Trim();
            string addr1 = txtAddr1.Text.Trim();
            string addr2 = txtAddr2.Text.Trim();
            string addr3 = txtAddr3.Text.Trim();
            string city = txtAddrCity.Text.Trim();
            string zip = txtAddrZip.Text.Trim();
            int geographyid = Int32.Parse(ddlState.SelectedValue);
            if (geographyid == 0) geographyid = Utils.GetGeographyID(ddlCountry.SelectedValue);
            UserManager.SaveShippingAddress(addrName, addr1, addr2, addr3, city, zip, geographyid);
            displayAddressView();
            Master.ShowMessage(Page.GetDisplayMember("UserAddress", "addressSaved", "Address '{0}' is saved. '", addrName ));
        }

        protected string MarkDefaultText(object defaultStatus)
        {
            if (defaultStatus is string && !String.IsNullOrEmpty(defaultStatus as string))
            {
                string status = defaultStatus as string;
                if (status == "Y")
                    return "Default Address";
                else
                    return "";
            }
            else
                return "";
        }

        protected bool MarkDefault(object defaultStatus)
        {
            if (defaultStatus is string && !String.IsNullOrEmpty(defaultStatus as string))
            {
                string status = defaultStatus as string;
                if (status == "Y")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        protected void gvAddress_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int addrid = Toolkit.ToInt32(e.CommandArgument.ToString(), 0);
            switch (e.CommandName)
            {
                case "Edit":
                    editAddressView(addrid);
                    break;
                case "MarkDefault":
                    UserManager.MarkAddressDefault(addrid);
                    displayAddressView();
                    break;
                case "Delete":
                    if (UserManager.IsDefaultAddress(addrid))
                        Master.ShowError(Page.GetDisplayMember("UserAddress", "deleteAddress", "You can't delete default address. "));
                    else
                    {
                        UserManager.DeleteAddress(addrid);
                        displayAddressView();
                    }
                    break;
            }
        }

        protected void gvAddress_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void gvAddress_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }
    }
}
