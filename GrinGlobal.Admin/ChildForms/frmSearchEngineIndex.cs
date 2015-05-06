using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.Diagnostics;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmSearchEngineIndex : GrinGlobal.Admin.ChildForms.frmBase {
        public frmSearchEngineIndex() {
            InitializeComponent();  tellBaseComponents(components);
            PrimaryTabControl = tcIndex;
            //SecondaryTabControl = tcSql;

        }

        public string IndexName;

        private void frmPermission_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvFields);
            //tcSql.Height = tcSql.Height + tcSql.Top - lblDataviewName.Top;
            //tcSql.Top = lblDataviewName.Top;
            //MessageBox.Show("Not finished yet, please ignore!");
        }

        private void initDropDowns() {

        }

        private DataTable stripNotInIndex(DataTable dt, string indexName) {
            // strip off all dtResolver rows that are not part of this index definition
            for (var i = 0; i < dt.Rows.Count; i++) {
                var dr = dt.Rows[i];
                if (dr["index_name"].ToString() != IndexName) {
                    dt.Rows.Remove(dr);
                    i--;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        DataSet _dsIndex;

        public override void RefreshData() {

            this.Text = "Search Engine Index - " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            initDropDowns();

            IndexName = ("" + MainFormCurrentNode().Tag).ToString();

            // set defaults...

            txtIndexName.Text = "";
            txtPrimaryKeyField.Text = "";
            txtNodeCacheSize.Text = "100";
            txtKeywordCacheSize.Text = "100";
            txtFanoutSize.Text = "100";
            txtEncoding.Text = "UTF8";
            txtAverageKeywordSize.Text = "11";
            txtMaxSortSize.Text = "16";
            chkEnabled.Checked = true;
            chkIndexAllTextFields.Checked = true;
            chkStripHtml.Checked = true;

            //txtMySQLSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";
            //txtOracleSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";
            //txtPostgreSQLSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";
            //txtSQLServerSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";

            lvFields.Items.Clear();
            lvResolvers.Items.Clear();

            if (!String.IsNullOrEmpty(IndexName)) {
                // DataTables returned are: indexer, index, index_field, resolver, and status.
                _dsIndex = AdminProxy.GetSearchEngineInfoEx(true, IndexName, null );
                var dtIndexer = _dsIndex.Tables["indexer"];
                var dtIndex = stripNotInIndex(_dsIndex.Tables["index"], IndexName);
                var dtIndexField = stripNotInIndex(_dsIndex.Tables["index_field"], IndexName);
                var dtResolver = stripNotInIndex(_dsIndex.Tables["resolver"], IndexName);
                var dtStatus = _dsIndex.Tables["status"];

                initHooksForMdiParent(dtResolver, "resolver_name", "resolver_name");

                if (dtIndex.Rows.Count > 0) {
                    var drIndex = dtIndex.Rows[0];

                    // load general tab
                    txtIndexName.Text = drIndex["index_name"].ToString();
                    txtPrimaryKeyField.Text = drIndex["primary_key_field"].ToString();
                    txtNodeCacheSize.Text = drIndex["node_cache_size"].ToString();
                    txtKeywordCacheSize.Text = drIndex["keyword_cache_size"].ToString();
                    txtFanoutSize.Text = drIndex["fanout_size"].ToString();
                    txtEncoding.Text = drIndex["encoding"].ToString();
                    txtAverageKeywordSize.Text = drIndex["average_keyword_size"].ToString();
                    txtMaxSortSize.Text = drIndex["max_sort_size_in_mb"].ToString();
                    chkEnabled.Checked = drIndex["enabled"].ToString().ToUpper() == "TRUE";
                    chkIndexAllTextFields.Checked = drIndex["auto_index_string_fields"].ToString().ToUpper() == "TRUE";
                    chkStripHtml.Checked = drIndex["strip_html"].ToString().ToUpper() == "TRUE";

                    //// load sql tab
                    //ddlSqlSource.SelectedIndex = ddlSqlSource.FindString(drIndex["sql_source"].ToString());

                    initDataViewDropDown(ddlSearchDataviews, false, "(None)", -1, "Search Engine", null);
                    ddlSearchDataviews.SelectedIndex = ddlSearchDataviews.FindString(drIndex["dataview_name"].ToString());

                    //txtMySQLSQL.Text = drIndex["mysql_sql_statement"].ToString();
                    //txtOracleSQL.Text = drIndex["oracle_sql_statement"].ToString();
                    //txtPostgreSQLSQL.Text = drIndex["postgresql_sql_statement"].ToString();
                    //txtSQLServerSQL.Text = drIndex["sqlserver_sql_statement"].ToString();

                    txtErrorOnRebuild.Text = drIndex["error_on_rebuild"].ToString();

                    var rebuildBeginDate = drIndex["last_rebuild_begin_date"];
                    var rebuildEndDate = drIndex["last_rebuild_end_date"];
                    if (rebuildBeginDate == DBNull.Value) {
                        lblLastRebuildBeginDate.Text = "n/a";
                        lblLastRebuildEndDate.Text = "n/a";
                        txtErrorOnRebuild.Text = "n/a";
                        lblErrorsSinceLastRebuild.Text = "n/a";
                    } else {

                        var beginDate = Toolkit.ToDateTime(rebuildBeginDate, DateTime.MinValue);

                        lblLastRebuildBeginDate.Text = beginDate.ToLocalTime().ToString();

                        if (rebuildEndDate == DBNull.Value) {
                            // began, not done yet.
                            lblErrorsSinceLastRebuild.Text = "n/a";
                            txtErrorOnRebuild.Text = "n/a";
                            lblLastRebuildEndDate.Text = "(Rebuilding now)";

                        } else {

                            lblErrorsSinceLastRebuild.Text = "n/a";
                            txtErrorOnRebuild.Text = "n/a";
                            lblLastRebuildEndDate.ForeColor = Color.Black;

                            var endDate = Toolkit.ToDateTime(rebuildEndDate, DateTime.MinValue);
                            if (endDate == DateTime.MinValue) {

                                // began, not done yet
                                lblLastRebuildEndDate.Text = "(Rebuilding now)";
                            } else {

                                var errorText = drIndex["error_on_rebuild"].ToString();

                                if (!String.IsNullOrEmpty(errorText)){

                                    // rebuild failed.
                                    txtErrorOnRebuild.Text = errorText;
                                    lblLastRebuildEndDate.Text = endDate.ToLocalTime().ToString() + " *** ERROR ***";
                                    lblLastRebuildEndDate.ForeColor = Color.Red;

                                } else {

                                    // rebuild worked and is done.
                                    txtErrorOnRebuild.Text = "(none)";
                                    lblErrorsSinceLastRebuild.Text = Toolkit.ToInt32(drIndex["errors_since_last_rebuild"], 0).ToString();
                                    lblLastRebuildEndDate.Text = endDate.ToLocalTime().ToString();

                                }

                            }
                        }
                    }


                    foreach (DataRow drIndexField in dtIndexField.Rows) {
                        var lvi = new ListViewItem(drIndexField["field_name"].ToString());
                        lvi.Tag = lvi.Text;
                        lvi.SubItems.Add(drIndexField["is_stored_in_index"].ToString().ToUpper() == "TRUE" ? "Y" : "N");
                        lvi.SubItems.Add(drIndexField["is_searchable"].ToString().ToUpper() == "TRUE" ? "Y" : "N");
                        lvi.SubItems.Add(drIndexField["calculation"].ToString());
                        lvi.SubItems.Add(drIndexField["format"].ToString());
                        lvi.SubItems.Add(drIndexField["is_boolean"].ToString().ToUpper() == "TRUE" ? "Y" : "N");
                        lvi.SubItems.Add(drIndexField["true_value"].ToString());
                        lvFields.Items.Add(lvi);
                    }

                    foreach (DataRow drRes in dtResolver.Rows) {
                        var lvi = new ListViewItem(drRes["resolver_name"].ToString());
                        lvi.Tag = lvi.Text;
                        lvi.SubItems.Add(drRes["enabled"].ToString().ToUpper() == "TRUE" ? "Y" : "N");
                        lvi.SubItems.Add(drRes["method"].ToString());
                        var valid = Toolkit.ToBoolean(drRes["is_valid"], false);
                        if (valid){
                            lvi.SubItems.Add("Y");
                        } else {
                            lvi.SubItems.Add("N - " + drRes["invalid_reason"]);
                            lvi.ForeColor = Color.Red;
                        }
                        lvResolvers.Items.Add(lvi);
                    }
                }

            } else {
                initHooksForMdiParent(null, null, null);
            }

            //switch (providerName) {
            //    case "mysql":
            //        tcSql.SelectedTab = tpMySQL;
            //        break;
            //    case "oracle":
            //        tcSql.SelectedTab = tpOracle;
            //        break;
            //    case "postgresql":
            //        tcSql.SelectedTab = tpPostgreSQL;
            //        break;
            //    case "sqlserver":
            //    default:
            //        tcSql.SelectedTab = tpSqlServer;
            //        break;
            //}

            MarkClean();

        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(!Modal);
        }

        private void save(bool showParent){

            DataRow dr = null;
            var dt = _dsIndex.Tables["index"];
            if (dt.Rows.Count == 0) {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
            } else {
                dr = dt.Rows[0];
            }

            dr["index_name"] = IndexName;
//            dr["resolver_name"] = txtName.Text;
            dr["average_keyword_size"] = txtAverageKeywordSize.Text;
            dr["encoding"] = txtEncoding.Text;
            dr["fanout_size"] = txtFanoutSize.Text;
            dr["keyword_cache_size"] = txtKeywordCacheSize.Text;
            dr["node_cache_size"] = txtNodeCacheSize.Text;
            //dr["mysql_sql_statement"] = txtMySQLSQL.Text;
            //dr["oracle_sql_statement"] = txtOracleSQL.Text;
            //dr["postgresql_sql_statement"] = txtPostgreSQLSQL.Text;
            //dr["sqlserver_sql_statement"] = txtSQLServerSQL.Text;
            dr["primary_key_field"] = txtPrimaryKeyField.Text;
            dr["sql_source"] = "Dataview";
            dr["dataview_name"] = ddlSearchDataviews.Text;

            // LOTS more to do here...

            AdminProxy.SaveSearchEngineIndex(_dsIndex);

            var result = MessageBox.Show(this, getDisplayMember("save{delayed_body}", "Your changes have been saved, but they will not take effect until the '{0}' index is rebuilt.\nWould you like to rebuild it now?", IndexName ), 
                getDisplayMember("save{delayed_title}", "Save Successful - Rebuild Index?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {

                AdminProxy.RebuildSearchEngineIndex(IndexName);
            }

            MarkClean();
            MainFormUpdateStatus(getDisplayMember("save{done}", "Saved settings for index {0}", IndexName), true);

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            if (Modal) {
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void newRestrictionToolStripMenuItem_Click(object sender, EventArgs e) {


            //// we can't let the user just blindly add fields,
            //// but we can auto-generate fields for them from the index's SQL.
            //if (ddlSqlSource.SelectedIndex == 0) {
            //    // Xml data source
            //    var provider = AdminProxy.Connection.DatabaseEngineProviderName.ToLower();
            //    var sql = "";
            //    switch (provider) {
            //        case "sqlserver":
            //            sql = txtSQLServerSQL.Text;
            //            break;
            //        case "oracle":
            //            sql = txtOracleSQL.Text;
            //            break;
            //        case "mysql":
            //            sql = txtMySQLSQL.Text;
            //            break;
            //        case "postgresql":
            //            sql = txtPostgreSQLSQL.Text;
            //            break;
            //    }

            //    var dt = AdminProxy.TestDataview(sql, null, "tableABC", AdminProxy.LanguageID, 0, 1).Tables["tableABC"];

            //    foreach (DataRow dr in dt.Rows) {
            //    }

            //} else {
            //    // Dataview data source
            //}



            MessageBox.Show(this, getDisplayMember("newField{noaddfield_body}", "Fields can not be added from here.  They are created by adding them to the SQL statement.\nYou may configure the field attributes here, but to add one you must edit the SQL and rebuild the index."),
                getDisplayMember("newField{noaddfield_title}", "Cannot Create Field From Here"));

            //btnAddNewField.PerformClick();
        }

        private void showProperties(string resolverName) {
            if (String.IsNullOrEmpty(resolverName)) {
//                MainFormPopupNewItemForm(new frmResolver());
            } else {
                MainFormSelectChildTreeNode(resolverName);
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            showFieldProperties();
        }

        private void showFieldProperties() {
            if (lvFields.SelectedItems.Count == 1) {
                var f = new frmSearchEngineIndexField();
                f.IndexName = this.IndexName;
                f.FieldName = ("" + lvFields.SelectedItems[0].Tag);
                f.DataTable = _dsIndex.Tables["index_field"];
                if (DialogResult.OK == MainFormPopupForm(f, this, false)) {

                    // copy to view and behind-the-scenes datatable here!

                    var offset = lvFields.SelectedIndices[0];
                    var dtIndexField = stripNotInIndex(_dsIndex.Tables["index_field"], IndexName);
                    var drField = dtIndexField.Rows[offset];
                    var lvi = lvFields.Items[offset];

                    drField["field_name"] = lvi.SubItems[0].Text = f.txtName.Text;
                    drField["foreign_key_table"] = f.txtForeignKeyTable.Text;
                    drField["foreign_key_field"] = f.txtForeignKeyField.Text;
                    drField["format"] = f.txtFormat.Text;  // 4
                    drField["calculation"] = f.txtCalculation.Text; // 3
                    drField["true_value"] = f.txtTrueValue.Text; //6

                    // primary key flag
                    if (f.chkPrimaryKey.Checked){
                        drField["is_primary_key"] = "TRUE";
                    } else {
                        drField["is_primary_key"] = "FALSE";
                    }

                    // stored in index flag
                    if (f.chkStoredInIndex.Checked){
                        drField["is_stored_in_index"] = "TRUE";
                        lvi.SubItems[1].Text = "Y";
                    } else {
                        drField["is_stored_in_index"] = "FALSE";
                        lvi.SubItems[1].Text = "N";
                    }

                    // searchable flag
                    if (f.chkSearchable.Checked) {
                        drField["is_searchable"] = "TRUE";
                        lvi.SubItems[2].Text = "Y";
                    } else {
                        drField["is_searchable"] = "FALSE";
                        lvi.SubItems[2].Text = "N";
                    }

                    // boolean flag
                    if (f.chkIsBoolean.Checked) {
                        drField["is_boolean"] = "TRUE";
                        lvi.SubItems[5].Text = "Y";
                    } else {
                        drField["is_boolean"] = "FALSE";
                        lvi.SubItems[5].Text = "N";
                    }
                    MarkDirty();
                    syncGUI();

                }
            }
        }

        private void lvPermFields_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                //deleteToolStripMenuItem.PerformClick();
            } else if (e.KeyCode == Keys.Enter) {
                cmiFieldProperties.PerformClick();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            deleteItems(true);
        }

        private void deleteItems(bool promptFirst) {
            if (lvFields.SelectedItems.Count > 0) {
                if (promptFirst) {
                    if (DialogResult.No == MessageBox.Show(this, getDisplayMember("deleteFields{prompt_body}", "You are about to permanently delete field(s) from this index.\nDo you want to continue?"), 
                        getDisplayMember("deleteFields{prompt_title}","Delete Field(s)?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                        return;
                    }
                }

                foreach (ListViewItem lvi in lvFields.SelectedItems) {
                    MessageBox.Show("TODO: delete field...");
                }
                RefreshData();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
            //deleteToolStripMenuItem.Enabled = lvPermFields.SelectedItems.Count > 0;
            //propertiesToolStripMenuItem.Enabled = lvPermFields.SelectedItems.Count == 1;
        }

        private void lvPermFields_MouseDoubleClick(object sender, MouseEventArgs e) {
            //if (lvPermFields.SelectedItems.Count == 1) {
            //    showProperties(Toolkit.ToInt32(lvPermFields.SelectedItems[0].Tag, -1));
            //}

        }

        private void txtDescription_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            Debug.WriteLine("PreviewKeyDown: " + (e.Control ? "CTRL-" : "") + (e.Alt ? "ALT-" : "") + (e.Shift ? "SHFT-" : "") + e.KeyCode.ToString());
        }

        private void txtDescription_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e) {
            Debug.WriteLine("KeyDown: " + (e.Control ? "CTRL-" : "") + (e.Alt ? "ALT-" : "") + (e.Shift ? "SHFT-" : "") + e.KeyCode.ToString());

        }

        private void btnAddNewField_Click(object sender, EventArgs e) {
            //if (ID < 1) {
            //    if (DialogResult.Yes == MessageBox.Show(this, "You must save the permission before adding restrictions to it.\nDo you want to save it now?", "Save Permission?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
            //        save(false);
            //    } else {
            //        return;
            //    }
            //}

            var f = new frmSearchEngineIndexField();
            f.IndexName = IndexName;
            f.FieldName = null;
            f.DataTable = _dsIndex.Tables["index_field"];

            MainFormPopupNewItemForm(f);

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e) {
            if (lvResolvers.SelectedItems.Count > 0) {
                using (new AutoCursor(this)) {
                    foreach (ListViewItem lvi in lvResolvers.SelectedItems) {
                        AdminProxy.DisableSearchEngineResolver(IndexName, lvi.Tag + "");
                    }
                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("disableResolvers{done}", "Disabled {0} resolver(s) for index {1}", lvResolvers.SelectedItems.Count.ToString("###,##0"), IndexName), true);
                }
            }

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e) {
            if (lvResolvers.SelectedItems.Count > 0) {
                using (new AutoCursor(this)) {
                    foreach (ListViewItem lvi in lvResolvers.SelectedItems) {
                        AdminProxy.EnableSearchEngineResolver(IndexName, lvi.Tag + "");
                    }
                    RefreshData();
                    MainFormUpdateStatus(getDisplayMember("enable{done}", "Enabled {0} resolver(s) for index {1}", lvResolvers.SelectedItems.Count.ToString("###,##0"), IndexName), true);
                }
            }

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e) {
            MainFormRefreshData();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e) {
            if (lvResolvers.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvResolvers.SelectedItems[0].Tag + "");
            }
        }

        private void lvResolvers_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lvResolvers.SelectedItems.Count == 1) {
                showProperties(lvResolvers.SelectedItems[0].Tag + "");
            }
        }

        private void txtSQLServerSQL_KeyUp(object sender, KeyEventArgs e) {
        }

        private void txtSQLServerSQL_KeyDown(object sender, KeyEventArgs e) {
            selectAllIfNeeded(sender as TextBox, e);
        }

        private void txtMySQLSQL_KeyDown(object sender, KeyEventArgs e) {
            selectAllIfNeeded(sender as TextBox, e);
        }

        private void txtPostgreSQLSQL_KeyDown(object sender, KeyEventArgs e) {
            selectAllIfNeeded(sender as TextBox, e);

        }

        private void txtOracleSQL_KeyDown(object sender, KeyEventArgs e) {
            selectAllIfNeeded(sender as TextBox, e);

        }

        private void chkIndexAllTextFields_CheckedChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void syncGUI() {

            //if (ddlSqlSource.Text.ToLower().Contains("dataview")) {
            //    tcSql.Visible = false;
            //    lblDataviewName.Visible = true;
            //    ddlSearchDataviews.Visible = true;
            //    btnEditDataview.Visible = true;
            //    btnNewDataview.Visible = true;
            //} else {
            //    tcSql.Visible = true;
            //    lblDataviewName.Visible = false;
            //    ddlSearchDataviews.Visible = false;
            //    btnEditDataview.Visible = false;
            //    btnNewDataview.Visible = false;
            //}

            CheckDirty();
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtPrimaryKeyField_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtNodeCacheSize_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtKeywordCacheSize_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void chkStripHtml_CheckedChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtAverageKeywordSize_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtFanoutSize_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtMaxSortSize_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtEncoding_TextChanged(object sender, EventArgs e) {
            syncGUI();
    
        }

        private void txtMySQLSQL_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtPostgreSQLSQL_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void txtOracleSQL_TextChanged(object sender, EventArgs e) {
            syncGUI();

        }

        private void lvFields_MouseDoubleClick(object sender, MouseEventArgs e) {
            showFieldProperties();
        }

        private void ddlSqlSource_SelectedIndexChanged(object sender, EventArgs e) {
            syncGUI();
        }

        private void btnEditDataview_Click(object sender, EventArgs e) {
            editDataview(ddlSearchDataviews.SelectedValue.ToString());
        }

        private void btnNewDataview_Click(object sender, EventArgs e) {
            editDataview("");
        }

        private void editDataview(string dataviewName){
            using (new AutoCursor(this)) {
                var f = new frmDataview();
                f.DataviewName = dataviewName;
                if (String.IsNullOrEmpty(dataviewName)) {
                    f.ddlCategoryCode.Text = "Search Engine";
                }
                if (DialogResult.OK == MainFormPopupForm(f, this, false)) {
                    initDataViewDropDown(ddlSearchDataviews, false, "(None)", -1, "Search Engine", null);
                    ddlSearchDataviews.SelectedIndex = ddlSearchDataviews.FindString(f.DataviewName);
                    CheckDirty();
                }
            }
        }

        private void ddlSearchDataviews_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void cmiFieldRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private void cmiParameterRefresh_Click(object sender, EventArgs e) {
            RefreshData();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmSearchEngineIndex", resourceName, null, defaultValue, substitutes);
        }
    }
}
