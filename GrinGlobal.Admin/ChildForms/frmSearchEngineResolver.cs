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
    public partial class frmSearchEngineResolver : GrinGlobal.Admin.ChildForms.frmBase {
        public frmSearchEngineResolver() {
            InitializeComponent();  tellBaseComponents(components);
            PrimaryTabControl = tcResolver;

        }

        public string IndexName;
        public string ResolverName;

        public DataTable _dtResolver;

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

        private DataTable stripNotInResolver(DataTable dt, string indexName, string resolverName) {
            // strip off all dtResolver rows that are not part of this index definition
            for (var i = 0; i < dt.Rows.Count; i++) {
                var dr = dt.Rows[i];
                if (dr["index_name"].ToString() != IndexName || dr["resolver_name"].ToString() != resolverName){
                    dt.Rows.Remove(dr);
                    i--;
                }
            }
            dt.AcceptChanges();
            return dt;
        }

        private string mapDropdownValue(string input) {
            switch ((String.Empty + input).ToLower()) {
                    // ResolutionMethod enum...
                case "id":
                    return "Id";
                case "id and keyword":
                    return "IdAndKeyword";
                case "idandkeyword":
                    return "Id And Keyword";

                    // ResolverCacheMode enum...
                case "primarykey":
                    return "Index's Primary Key";
                case "index's primary key":
                    return "PrimaryKey";
                case "foreignkey":
                    return "Foreign Key";
                case "foreign key":
                    return "ForeignKey";
                case "sql":
                    return "Sql";

                    // Both / neither enum...
                case "none":
                    return "None";
                default:
                    return input;
            }
        }

        public override void RefreshData() {

            this.Text = "Search Engine Resolver - " + MainFormCurrentNodeText("") + " - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            IndexName = ("" + MainFormCurrentNode().Parent.Tag).ToString();
            ResolverName = ("" + MainFormCurrentNode().Tag).ToString();
            string providerName = "";

            // set defaults...

            txtName.Text = "";
            ddlMethod.SelectedIndex = 0;
            ddlCacheMode.SelectedIndex = 0;
            txtPrimaryKeyField.Text = "";
            txtNodeCacheSize.Text = "100";
            txtKeywordCacheSize.Text = "100";
            txtFanoutSize.Text = "100";
            txtEncoding.Text = "UTF8";
            txtAverageKeywordSize.Text = "11";
            chkEnabled.Checked = true;
            chkAllowRealtimeUpdates.Checked = true;
            chkEnabled.Checked = true;

            //txtMySQLSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";
            //txtOracleSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";
            //txtPostgreSQLSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";
            //txtSQLServerSQL.Text = "select * from TABLE /*_ WHERE src.pk_id in (:idlist) _*/";

            this.Text = "Search Engine Resolver - (New Resolver) for Index " + IndexName + " - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            if (!String.IsNullOrEmpty(IndexName)) {
                // DataTables returned are: indexer, index, index_field, resolver, and status.
                var ds = AdminProxy.GetSearchEngineInfoEx(true, IndexName, ResolverName);
                _dtResolver = stripNotInResolver(ds.Tables["resolver"], IndexName, ResolverName);

                //initHooksForMdiParent(dtResolver, "resolver_name", "resolver_name");

                if (_dtResolver.Rows.Count > 0) {
                    var dr = _dtResolver.Rows[0];

                    this.Text = "Search Engine Resolver - " + ResolverName + " for Index " + IndexName + " - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

                    // load general tab
                    txtName.Text = dr["resolver_name"].ToString();
                    txtForeignKeyField.Text = dr["foreign_key_field"].ToString();
                    txtPrimaryKeyField.Text = dr["primary_key_field"].ToString();
                    txtResolvedPrimaryKeyField.Text = dr["resolved_primary_key_field"].ToString();

                    ddlMethod.SelectedIndex = ddlMethod.FindStringExact(mapDropdownValue(dr["method"].ToString()));

                    initDataViewDropDown(ddlSearchDataviews, false, "(None)", -1, "Search Engine", null);
                    ddlSearchDataviews.SelectedIndex = ddlSearchDataviews.FindString(dr["dataview_name"].ToString());


                    chkEnabled.Checked = dr["enabled"].ToString().ToUpper() == "TRUE";
                    chkAllowRealtimeUpdates.Checked = dr["allow_realtime_updates"].ToString().ToUpper() == "TRUE";

                    // load advanced tab
                    ddlCacheMode.SelectedIndex = ddlCacheMode.FindStringExact(mapDropdownValue(dr["cache_mode"].ToString()));
                    txtNodeCacheSize.Text = dr["node_cache_size"].ToString();
                    txtKeywordCacheSize.Text = dr["keyword_cache_size"].ToString();
                    txtFanoutSize.Text = dr["fanout_size"].ToString();
                    txtEncoding.Text = dr["encoding"].ToString();
                    txtAverageKeywordSize.Text = dr["average_keyword_size"].ToString();

                }

            }

            MarkClean();
            syncGUI();

        }

        private bool _syncing;
        private void syncGUI() {
            if (!_syncing) {
                try {
                    _syncing = true;


                    if (ddlCacheMode.Text.Contains("Keyword")) {
                        if (chkAllowRealtimeUpdates.Checked) {
                            chkAllowRealtimeUpdates.Checked = false;
                            MessageBox.Show(this, getDisplayMember("syncGUI{norealtime_body}",  "Realtime updates can not be used when a method of 'Id And Keyword' is used, and has been disabled."), 
                                getDisplayMember("syncGUI{norealtime_title}", "Realtime Updates Unavailable"));
                        }
                    }



                    // toggle items related to Method dropdown
                    lblForeignKeyField.Visible = false;
                    txtForeignKeyField.Visible = false;
                    lblPrimaryKeyField.Visible = false;
                    txtPrimaryKeyField.Visible = false;
                    lblResolvedPrimaryKeyField.Visible = false;
                    txtResolvedPrimaryKeyField.Visible = false;

                    lblDataviewName.Visible = false;
                    ddlSearchDataviews.Visible = false;
                    btnNewDataview.Visible = false;
                    btnEditDataview.Visible = false;

                    var val = mapDropdownValue(ddlMethod.Text);
                    switch (val) {
                        case "None":
                            break;
                        case "ForeignKey":
                            lblForeignKeyField.Visible = true;
                            txtForeignKeyField.Visible = true;
                            break;
                        case "PrimaryKey":
                            break;
                        case "Sql":
                            lblPrimaryKeyField.Visible = true;
                            txtPrimaryKeyField.Visible = true;
                            lblDataviewName.Visible = true;
                            ddlSearchDataviews.Visible = true;
                            btnNewDataview.Visible = true;
                            btnEditDataview.Visible = true;
                            lblResolvedPrimaryKeyField.Visible = true;
                            txtResolvedPrimaryKeyField.Visible = true;
                            break;
                    }


                } finally {
                    _syncing = false;
                }
            }
            CheckDirty();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            save(!Modal);
        }

        private void save(bool showParent){

            DataRow dr = null;
            if (_dtResolver.Rows.Count == 0) {
                dr = _dtResolver.NewRow();
                _dtResolver.Rows.Add(dr);
            } else {
                dr = _dtResolver.Rows[0];
            }

            dr["index_name"] = IndexName;
            dr["resolver_name"] = txtName.Text;
            dr["average_keyword_size"] = txtAverageKeywordSize.Text;
            dr["encoding"] = txtEncoding.Text;
            dr["fanout_size"] = txtFanoutSize.Text;
            dr["foreign_key_field"] = txtForeignKeyField.Text;
            dr["keyword_cache_size"] = txtKeywordCacheSize.Text;
            dr["node_cache_size"] = txtNodeCacheSize.Text;
            dr["sql_source"] = ddlMethod.Text == "Sql" ? "Dataview" : "None";
            dr["primary_key_field"] = txtPrimaryKeyField.Text;
            dr["resolved_primary_key_field"] = txtResolvedPrimaryKeyField.Text;
            dr["cache_mode"] = mapDropdownValue(ddlCacheMode.Text);
            dr["method"] = mapDropdownValue(ddlMethod.Text);
            dr["dataview_name"] = ddlSearchDataviews.SelectedValue.ToString();

            dr["enabled"] = chkEnabled.Checked ? "TRUE" : "FALSE";
            dr["allow_realtime_updates"] = chkAllowRealtimeUpdates.Checked ? "TRUE" : "FALSE";



            AdminProxy.SaveSearchEngineResolver(_dtResolver.DataSet);


            var result = MessageBox.Show(this, getDisplayMember("save{delayed_body}", "Your changes have been saved, but they will not take effect until the '{0}' index is rebuilt.\nWould you like to rebuild it now?", IndexName), 
                getDisplayMember("save{delayed_title}", "Save Successful - Rebuild Index?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes){

                AdminProxy.RebuildSearchEngineIndex(IndexName);
            }

            MarkClean();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            if (Modal) {
                this.Close();
            } else {
                MainFormSelectParentTreeNode();
            }
        }

        private void showProperties(string resolverName) {
            if (String.IsNullOrEmpty(resolverName)) {
//                MainFormPopupNewItemForm(new frmResolver());
            } else {
                MainFormSelectChildTreeNode(resolverName);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e) {
            MainFormRefreshData();
        }


        private void ddlMethod_SelectedIndexChanged(object sender, EventArgs e) {
            syncGUI();

        }
        private void ddlCacheMode_SelectedIndexChanged(object sender, EventArgs e) {
            syncGUI();

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

        private void frmSearchEngineResolver_Load(object sender, EventArgs e) {
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void chkAllowRealtimeUpdates_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtForeignKeyField_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtPrimaryKeyField_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtResolvedPrimaryKeyField_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtNodeCacheSize_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtKeywordCacheSize_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtAverageKeywordSize_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtFanoutSize_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtEncoding_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtSQLServerSQL_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtMySQLSQL_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtPostgreSQLSQL_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtOracleSQL_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void btnNewDataview_Click(object sender, EventArgs e) {
            editDataview("");
        }

        private void editDataview(string dataviewName) {
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

        private void btnEditDataview_Click(object sender, EventArgs e) {
            editDataview(ddlSearchDataviews.SelectedValue.ToString());
        }

        private void ddlSearchDataviews_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmSearchEngineResolver", resourceName, null, defaultValue, substitutes);
        }
    }
}
