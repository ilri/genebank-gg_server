using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrinGlobal.Core;
using GrinGlobal.Core.Xml;
using System.Text;
using System.Data;


namespace GrinGlobal.Web
{
    public partial class UserFavorite : System.Web.UI.Page
    {
        DataTable dtUseCategory = null;
        protected void Page_Init(object sender, EventArgs e)
        {
            if (UserManager.IsAnonymousUser(UserManager.GetUserName()))
                Response.Redirect("~/login.aspx?action=menu");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                bindData(true);
        }

        private void bindData(bool viewOnly) 
        {
            dtUseCategory = Utils.GetCodeValue("WEB_FAVORITE_USAGE", "");

            DataTable dt = null;
            using (var sd = UserManager.GetSecureData(true))
            {
                int wuserid = sd.WebUserID;
                dt = sd.GetData("web_favorites_view", ":wuserid=" + wuserid + ";:accessionid=" + 0, 0, 0).Tables["web_favorites_view"];
            }

            // using tree view
            //tvFavorite.Nodes.Clear();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    var child = new TreeNode(dr["pi_number"].ToString(), dr["accession_id"].ToString());
            //        tvFavorite.Nodes.Add(child);
            //}

            
            if (dt != null)
            {
                int cnt = dt.Rows.Count;
                if (cnt > 0) lblCnt.Text = " (" + cnt.ToString() + (cnt ==1? " item)": " items)");
            }

            // using gridview
            if (viewOnly)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Note"].ToString().Contains("\r\n"))
                        dr["Note"] = dr["Note"].ToString().Replace("\r\n", "<br />");
                }
            }

            gvFavorite.DataSource = dt;
            gvFavorite.DataBind();

            gv.DataSource = dt;
            gv.DataBind();
        }

        protected void tvFavorite_SelectedNodeChanged(object sender, EventArgs e)
        {
            var nd = tvFavorite.SelectedNode;
            Response.Redirect("~/accessiondetail.aspx?id=" + nd.Value);
            Response.End();
        }

        protected void gvFavorite_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Toolkit.ToInt32(gvFavorite.DataKeys[e.RowIndex].Value, 0);
            Favorite.Current.RemoveAccession(id, false);
            Favorite.Current.Save();
            bindData(true);
            string msg = Page.GetDisplayMember("UserFavorite", "favoriteRemoved", "Item removed from your favorites.");
            Master.ShowMessage(msg);
        }

        protected void btnAddSelected_Click(object sender, EventArgs e)
        {
            int id = 0;
            Cart c = Cart.Current;

            foreach (GridViewRow row in gvFavorite.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelect")).Checked)
                {
                    id = Toolkit.ToInt32(gvFavorite.DataKeys[row.DataItemIndex][0].ToString(), 0);
                    c.AddAccession(id, null);
                }
            }
            c.Save();
            bindData(true);
            string msg = Page.GetDisplayMember("UserFavorite", "addedToOrder", "Selected item(s) added to your order.");
            Master.ShowMessage(msg);
        }

        protected void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            int id = 0;
            Favorite f = Favorite.Current;

            foreach (GridViewRow row in gvFavorite.Rows)
            {
                if (((CheckBox)row.FindControl("chkSelect")).Checked)
                {
                    id = Toolkit.ToInt32(gvFavorite.DataKeys[row.DataItemIndex][0].ToString(), 0);
                    f.RemoveAccession(id, false);
                }
            }
            f.Save();
            bindData(true);
            string msg = Page.GetDisplayMember("UserFavorite", "favoriteRemoved", "Selected item(s) removed from your favorites.");
            Master.ShowMessage(msg);
        }

        protected void gvFavorite_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int id = Toolkit.ToInt32(gvFavorite.DataKeys[e.NewEditIndex].Value, 0);
            gvFavorite.EditIndex = e.NewEditIndex;
            bindData(false);
        }

        protected void gvFavorite_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvFavorite.EditIndex = -1;
            gvFavorite.SelectedIndex = -1;
            bindData(true);
        }

        protected void gvFavorite_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Toolkit.ToInt32(gvFavorite.DataKeys[e.RowIndex].Value, 0);

            TextBox txtNote = (TextBox)gvFavorite.Rows[e.RowIndex].FindControl("txtNote");
            string note = txtNote.Text;

            using (var sd = UserManager.GetSecureData(true))
            {
                int wuserid = sd.WebUserID;
                DataTable dt = sd.GetData("web_favorites_view", ":wuserid=" + 0 + ";:accessionid=" + id, 0, 0).Tables["web_favorites_view"];

                if (dt.Rows.Count > 0)
                {
                    //dt.Rows[0]["note"] = note;
                    //var ds_saved = sd.SaveData(dt.DataSet, true, ""); // Don't use SaveData() for web table for now

                    int itemId = Toolkit.ToInt32(dt.Rows[0]["web_user_cart_item_id"].ToString(), 0);
                    using (DataManager dm = sd.BeginProcessing(true))
                    {
                        dm.Write(@"
                            update web_user_cart_item
                            set note = :note, 
                                modified_date = :modifieddate,
                                modified_by   = :modifiedby
                             where
                                web_user_cart_item_id = :itemId
                                ", new DataParameters(
                                ":note", note, DbType.String,
                                ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                                ":modifiedby", wuserid,
                                ":itemId", itemId
                                ));
                    }
                }

                gvFavorite.EditIndex = -1;
                bindData(true);
                string msg = Page.GetDisplayMember("UserFavorite", "cannotChangeItem", "Item comment changed.");
                Master.ShowMessage(msg);
            }
        }

        protected void gvFavorite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            if (e.CommandName == "Add")
            {
                int accession_id = int.Parse(id);

                int i = Cart.Current.AddAccession(accession_id, null);
                string msg = "";
                if (i == 1)
                {
                    Cart.Current.Save();
                    msg = Page.GetDisplayMember("UserFavorite", "itemAddedToOrder", "Item added to your order.");
                }
                else
                    msg = Page.GetDisplayMember("CartView", "cartItemExisted", "That item was already in your cart.");

                Master.ShowMessage(msg);
            }
        }

        protected void changedUseCategory(object sender, EventArgs ea)
        {
            var ddl = (DropDownList)sender;
            var gvr = (GridViewRow)(ddl.NamingContainer);
            var id = Toolkit.ToInt32(gvFavorite.DataKeys[gvr.RowIndex][0], -1);
            var useCategory = ddl.SelectedValue;

            using (var sd = UserManager.GetSecureData(true))
            {
                int wuserid = sd.WebUserID;
                DataTable dt = sd.GetData("web_favorites_view", ":wuserid=" + 0 + ";:accessionid=" + id, 0, 0).Tables["web_favorites_view"];

                if (dt.Rows.Count > 0)
                {
                    int itemId = Toolkit.ToInt32(dt.Rows[0]["web_user_cart_item_id"].ToString(), 0);

                    using (DataManager dm = sd.BeginProcessing(true))
                    {
                        dm.Write(@"
                                update web_user_cart_item
                                set usage_code = :usecode, 
                                    modified_date = :modifieddate,
                                    modified_by   = :modifiedby
                                 where
                                    web_user_cart_item_id = :itemId
                                    ", new DataParameters(
                                ":usecode", useCategory, DbType.String,
                                ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
                                ":modifiedby", wuserid,
                                ":itemId", itemId
                                ));
                    }
                }
            }
         }

        protected void gvFavorite_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItemIndex > -1)
            {
                var accessionID = Toolkit.ToInt32(((DataRowView)e.Row.DataItem)["accession_id"], -1);
                var useCatagory = ((DataRowView)e.Row.DataItem)["usage_code"].ToString();

                var ddl = e.Row.FindControl("ddlUseCategory") as DropDownList;

                ddl.DataSource = dtUseCategory;
                ddl.DataBind();

                var item = ddl.Items.FindByValue(useCatagory);
                ddl.SelectedIndex = ddl.Items.IndexOf(item);

                var avail = ((DataRowView)e.Row.DataItem)["availability"].ToString();
                if (avail == "Not Available")
                {
                    var lbtn = e.Row.FindControl("btnAdd") as LinkButton;
                    lbtn.Visible = false;

                    var lbl = e.Row.FindControl("lblNotAvailable") as Label;
                    lbl.Visible = true;

                }

                string statusCode = "", statusNote = "";
                using (var sd = UserManager.GetSecureData(false))
                {
                    var dtAvail = sd.GetData("web_lookup_availability", ":accessionid=" + accessionID, 0, 0).Tables["web_lookup_availability"];

                    foreach (DataRow dr in dtAvail.Rows)
                    {
                        statusCode = dr["availability_status_code"].ToString();
                        statusNote = dr["availability_status_note"].ToString();

                        if (statusCode == "WEED")
                        {
                            //var siteName = ((DataRowView)e.Row.DataItem)["site_code"].ToString();
                            //var siteID = ((DataRowView)e.Row.DataItem)["site_id"].ToString();

                            string note = "This accession is a restricted noxious weed, contact site for availability.";

                            ImageButton ib = e.Row.FindControl("btnInfor") as ImageButton;
                            ib.Visible = true;
                            ib.ToolTip = note;
                            break;
                        }
                    }
                }
             }
        }

        protected void btnCheckAll_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            if (lb.Text == "Select All")
            {
                lb.Text = "Select None";
                foreach (GridViewRow row in gvFavorite.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Checked = true;
                }
            }
            else
            {
                lb.Text = "Select All";
                foreach (GridViewRow row in gvFavorite.Rows)
                {
                    ((CheckBox)row.FindControl("chkSelect")).Checked = false;
                }
            }
        }

        protected void btnRemoveAll_Click(object sender, EventArgs e)
        {
            Favorite.Current.Empty();
            bindData(true);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Utils.ExportToExcel(HttpContext.Current, gv, "myfavorites", "My Favorites List:");
        }

        //protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(txtSql.Text.Trim()))
        //        bindGrid();
        //}

        //protected void gvFavorite_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    bindData(true);
        //    gvFavorite.PageIndex = e.NewPageIndex;
        //    gvFavorite.DataBind();
        //}

        //protected void gvFavorite_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    bindData(true);

        //    DataTable dt = (DataTable)gvFavorite.DataSource;

        //    if (dt != null)
        //    {
        //        DataView dv = new DataView(dt);
        //        dv.Sort = e.SortExpression + " " + (e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        //        gvFavorite.DataSource = dv;
        //        gvFavorite.DataBind();
        //    }
        //}

        
    }     
}
