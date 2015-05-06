using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Business;
using GrinGlobal.Core;
using System.Data;
using System.Diagnostics;

namespace GrinGlobal.Web {
    public partial class CartView : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            Page.Master.GetCartControl().Visible = false;
            if (!Page.IsPostBack) {

                if (Request.QueryString["action"] == "add") {
                    List<int> ids = Toolkit.ToIntList(Request.QueryString["id"]);
                    if (ids.Count > 0) {
                        Cart c = Cart.Current;
                        int added = c.AddAccessions(ids);
                        c.Save();

                        if (added == 0) {
                            if (ids.Count == 1) {
                                Master.ShowMessage(Page.GetDisplayMember("CartView", "cartItemExisted", "That item was already in your cart."));
                            } else {
                                Master.ShowMessage(Page.GetDisplayMember("CartView", "cartItemsExsisted", "Those items were already in your cart."));
                            }
                        } else {
                            if (ids.Count > added) {
                                Master.ShowMessage(Page.GetDisplayMember("CartView", "cartItemsAddedSomeExisted", "Added {0} items to your cart.  {1} were already in your cart.", added.ToString(), (ids.Count - added).ToString()));
                            } else {
                                Master.ShowMessage(Page.GetDisplayMember("CartView", "cartItemsAddedNoneExisted", "Added {0} items to your cart.", added.ToString()));
                            }
                        }
                    }
                }

                bindData();
            } 
            lnkPrevious.NavigateUrl = ViewState["previousURL"] as string;
        }

        private void bindData() {
            Cart c = Cart.Current;

            using (SecureData sd = new SecureData(false, UserManager.GetLoginToken(true))) {
                var ds = sd.GetData("web_cartview", ":idlist=" + Toolkit.Join(c.AccessionIDs.ToArray(), ",", ""), 0, 0);

                var dt = ds.Tables["web_cartview"];

//                using (DataManager dm = sd.BeginProcessing(true)) {


//                    DataTable dt = dm.Read(
//DataManager.ExpandIntegerList(
//@"
//select
//    0 as quantity,
//    a.accession_id,
//    concat(coalesce(a.accession_number_part1,''), ' ', coalesce(convert(varchar, a.accession_number),''), ' ', coalesce(a.accession_number_part3,'')) as pi_number,
//    a.site_code,
//    (select cvl.description from code_value cv left join code_value_lang cvl on cv.code_value_id = cvl.code_value_id where cv.code_group_code = 70 and cv.value = a.initial_material_type) as distributed_type,
//    (select TOP 1 name from accession_name an where a.accession_id = an.accession_id and plant_name_rank = (select MIN(plant_name_rank) from accession_name an2 where a.accession_id = an2.accession_id)) as top_name,
//    (select top 1 i.standard_distribution_quantity from inventory i where i.accession_id = a.accession_id and i.is_distributable = 'Y') as standard_distribution_quantity,
//    t.taxonomy_id,
//    t.name as taxonomy_name
//from
//    accession a left join taxonomy t 
//		on a.taxonomy_id = t.taxonomy_id
//where
//    a.accession_id in (:idlist)
//", ":idlist", c.AccessionIDs));
                //}

                    // HACK: need to push in the quantity from the cart object
                    // we can't do this as a join between accession and sys_user_cart_item
                    // because an anonymous user won't have any records in sys_user_cart_item.

                for(int i=0;i<dt.Rows.Count;i++){
                    CartItem ci = c.FindByAccessionID(dt.Rows[i], "accession_id");
                    if (ci != null) {
                        //dt.Rows[i]["quantity"] = ci.Quantity; //Laura Temp
                    }
                }

                gvCart.DataSource = dt;
                gvCart.DataBind();

                if (gvCart.Rows.Count == 0)
                    hlCheckout.Visible = false;
                else
                {
                    if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                        hlCheckout.NavigateUrl = "~/login.aspx?action=checkout";
                    else
                        hlCheckout.NavigateUrl = "~/userinfor.aspx?action=checkout";

                    int cnt = gvCart.Rows.Count;
                    lblCnt.Text = " (" + cnt.ToString() + (cnt == 1 ? " item)" : " items)");
               }     
                
               lnkPrevious.NavigateUrl = Request.Params["HTTP_REFERER"];
               ViewState["previousURL"] = lnkPrevious.NavigateUrl;
            }
        }

        protected void gvCart_SelectedIndexChanged(object sender, EventArgs e) {

        }

        protected void btnSave_Click(object sender, EventArgs e) {
            Cart c = Cart.Current;

            // load items from the form,
            // sync up with cart
            for (int i = 0; i < gvCart.Rows.Count; i++) {
                GridViewRow gvr = gvCart.Rows[i];
                TextBox txt = gvr.FindControl("txtQuantity") as TextBox;
                if (txt != null) {
                    int quantity = Toolkit.ToInt32(txt.Text, 0);
                    int id = Toolkit.ToInt32(gvCart.DataKeys[i].Value, 0);
                    if (quantity == 0) {
                        c.RemoveAccession(id, false);
                    } else {
                        int offset = c.AccessionIDs.IndexOf(id);
                        if (offset > -1) {
                            c.Accessions[offset].Quantity = quantity;
                        } else {
                            // accession not found, nothing to do
                        }
                    }
                }
            }

            // save cart, rebind and redisplay
            c.Save();
            bindData();

        }

        protected void gvCart_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            int id = Toolkit.ToInt32(gvCart.DataKeys[e.RowIndex].Value, 0);
            Cart c = Cart.Current;
            c.RemoveAccession(id, false);
            c.Save();
            bindData();
        }

        protected void btnRemoveAll_Click(object sender, EventArgs e) {
            Cart.Current.Empty();
            bindData();
        }

        protected void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            int id = 0;
            Cart c = Cart.Current;

            foreach (GridViewRow row in gvCart.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelect")).Checked)
                {
                    id = Toolkit.ToInt32(gvCart.DataKeys[row.DataItemIndex][0].ToString(), 0);            
                    c.RemoveAccession(id, false);
                }
            }
            c.Save();
            bindData();
        }

        protected void gvCart_RowDataBound(object sender, GridViewRowEventArgs e) {

            if (e.Row.DataItemIndex > -1) {

                // pull values from current row
                var accessionID = Toolkit.ToInt32(((DataRowView)e.Row.DataItem)["accession_id"], -1);
                var formCode = ((DataRowView)e.Row.DataItem)["distributed_type"].ToString();

                var ddl = e.Row.FindControl("ddlFormDistributed") as DropDownList;
                var lbl = e.Row.FindControl("lblFormDistributed") as Label;


                // list all types for given accession
                var c = Cart.Current;
                var dt = c.ListDistributionTypes(accessionID);
                if (dt.Rows.Count == 1) {

                    // only a single one, show a label and hide the drop down
                    ddl.Visible = false;
                    lbl.Text = dt.Rows[0]["display_text"].ToString();

                } else if (dt.Rows.Count == 0){

                    // should never happen should it?  Always have to have a distributable type if is_distributable and is_available are both 'Y'.
                    ddl.Visible = false;
                    lbl.Text = "(None found)";

                } else {

                    // multiples, hide the label, show the drop down for them to choose from

                    lbl.Visible = false;


                    // bind it to the list of form types
                    ddl.DataSource = dt;
                    ddl.DataBind();

                    // NOTE: the event handler for each row's ddl is manually wired up from the markup file (.aspx)

                    // select the proper one for this row
                    formCode = c.FindByAccessionID(accessionID).DistributionFormCode;
                    var item = ddl.Items.FindByValue(formCode);
                    ddl.SelectedIndex = ddl.Items.IndexOf(item);
                }


            }
        }

        public void changedDistributionType(object sender, EventArgs ea) {
            // they changed their form type for a given row.
            // grab the dropdownlist and the row it's in, then update cart info
            var ddl = (DropDownList)sender;
            var gvr = (GridViewRow)(ddl.NamingContainer);
            var accessionID = Toolkit.ToInt32(gvCart.DataKeys[gvr.RowIndex][0], -1);
            var c = Cart.Current;
            var ci = c.FindByAccessionID(accessionID);
            if (ci != null) {
                ci.DistributionFormCode = ddl.SelectedValue;
                c.Save();
            }
        }

    }
}
