using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Interface.Dataviews;
using System.Diagnostics;
using GrinGlobal.Business;
using GrinGlobal.Core;
using GrinGlobal.Admin.PopupForms;
using GrinGlobal.Business.SqlParser;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmDataview2 : GrinGlobal.Admin.ChildForms.frmBase {

        private enum FieldColumnOffsets {
            DataviewFieldName = 0,
            TableName = 1,
            TableFieldName = 2,
            Required = 3,
            ReadOnly = 4,
            PrimaryKey = 5,
            TransformBy = 6,
            UserInterface = 7,
            DropDownGroupName = 8,
            LookupDataviewName = 9,
            LastHardcoded = 9
        }


        public frmDataview2() {

            InitializeComponent();  tellBaseComponents(components);

            // HACK: rich text box does not natively support Drag events from the Properties panel in VS.NET.
            //       http://support.microsoft.com/kb/814309
            //initRichTextBox(rtbSQLServer);
            //initRichTextBox(rtbPostgreSQL);
            //initRichTextBox(rtbMySQL);
            //initRichTextBox(rtbOracle);
            PrimaryTabControl = tcBottom;
            //SecondaryTabControl = tcSql;


        }

        private class FieldRowTag {
            internal int RowIndex;
            internal ITable Table;
            internal IField Field;
        }

        private string _origDataviewName;

        private void initDropDowns() {

            Debug.WriteLine("initializing drop downs");

            // preview tab
            var dt = initLanguageDropDown(ddlViewInLanguage, true, null);

            // Fields tab
            foreach (DataRow dr in dt.Rows) {
                if (!dgvFields.Columns.Contains(dr["title"].ToString())) {
                    var dgc = new DataGridViewTextBoxColumn();
                    dgc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgc.HeaderText = dr["title"].ToString();
                    dgc.Tag = dr["sys_lang_id"].ToString() + "|" + dr["script_direction"].ToString();
                    if (dr["script_direction"].ToString() == "RTL") {
                        // this language is right to left, adjust accordingly.
                        dgc.HeaderText = RLE_CHAR + dr["title"].ToString();
                        if (dgc.HeaderCell.ToolTipText != string.Empty) {
                            dgc.HeaderCell.ToolTipText = RLE_CHAR + dgc.HeaderCell.ToolTipText;
                        }
                        dgc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
                    }
                    dgvFields.Columns.Add(dgc);
                }
            }

            //var dgddField = dgvFields.Columns[2] as DataGridViewComboBoxColumn;
            //initTableFieldDropDown(dgddField, 0, true, "(None)");

            var dgddGroupName = dgvFields.Columns[(int)FieldColumnOffsets.DropDownGroupName] as DataGridViewComboBoxColumn;
            initCodeGroupDropDown(dgddGroupName, false, "(None)");

            var dgddFKDataview = dgvFields.Columns[(int)FieldColumnOffsets.LookupDataviewName] as DataGridViewComboBoxColumn;
            initDataViewLookupDropDown(dgddFKDataview, "(None)");
            //initDataViewDropDown(dgddFKDataview, false, "(None)", -1);


            // parameters tab
            // (nothing to do)

            // properties tab
            initDataViewCategoryDropDown(cboCategoryNameCode);
            initDataViewDatabaseAreaDropDown(cboDatabaseAreaCode);


        }

        public string DataviewName;

        private List<ITable> _allTables;
        private Dictionary<string, int> _sqlIDs;

        private void initTreeViews() {
            // get 2 lists of all table names, one sorted alphabetically and one sorted by dependencies

            Debug.WriteLine("initializing tree views");
            _allTables = initTableFieldTreeView(tvByName, false);
            initTableFieldTreeView(tvByRelationship, true);

        }

        private DataTable _dtLang;

        private void initTransformDropDown(ComboBox ddl, List<IField> fields, string defaultText) {
            ddl.Items.Clear();
            ddl.Items.Add(""); // make an empty one
            var idx = -1;
            if (fields != null) {
                for (var i = 0; i < fields.Count; i++) {
                    ddl.Items.Add(fields[i].DataviewFieldName);
                    if (String.Compare(fields[i].DataviewFieldName, defaultText, true) == 0) {
                        idx = i+1;
                    }
                }
            }

            if (idx > -1) {
                ddl.SelectedIndex = idx;
            }
        }

        public override void RefreshData() {
            try {
                refreshData(false);
            } catch (NotImplementedException nie) {
                MessageBox.Show(this, nie.Message, "Error Refreshing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                refreshData(true);
            }
        }

        private bool _refreshing;
        private void refreshData(bool ignoreErrors){

            Sync(true, delegate() {
                _refreshing = true;
                if (tvByName.Nodes.Count == 0) {
                    initDropDowns();
                    initTreeViews();
                }

                // make sure we don't pull in a null dataviewname as that causes everything to load (i.e. really bad)
                DataviewName = String.IsNullOrEmpty(DataviewName) ? @"__This is a very!!!! unlikely name for a dataview....!@#$)*(%&*()" : DataviewName;

                Debug.WriteLine("refreshing data");

                _sqlIDs = new Dictionary<string, int>();

                for (var i = 0; i < ddlViewInLanguage.Items.Count; i++) {
                    if (ddlViewInLanguage.Items[i].ToString() == AdminProxy.LanguageID.ToString()) {
                        ddlViewInLanguage.SelectedIndex = i;
                    }
                }
                if (ddlViewInLanguage.SelectedIndex < 0) {
                    ddlViewInLanguage.SelectedIndex = 0;
                }

                this.Text = "Dataview - " + MainFormCurrentNodeText("New") + " - " + AdminProxy.Connection.DatabaseEngineServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

                var ds = AdminProxy.GetDataViewDefinition(this.DataviewName);

                ID = -1;

                _sqlIDs["sqlserver"] = -1;
                _sqlIDs["mysql"] = -1;
                _sqlIDs["postgresql"] = -1;
                _sqlIDs["oracle"] = -1;

                var dtSqls = ds.Tables["sys_dataview_sql"];
                //foreach (DataRow drS in dtSqls.Rows) {
                //    switch (drS["database_engine_tag"].ToString()) {
                //        case "mysql":
                //            txtMySQL.Text = drS["sql_statement"].ToString();
                //            _sqlIDs["mysql"] = Toolkit.ToInt32(drS["sys_dataview_sql_id"], -1);
                //            break;
                //        case "sqlserver":
                //            txtSQLServer.Text = drS["sql_statement"].ToString();
                //            _sqlIDs["sqlserver"] = Toolkit.ToInt32(drS["sys_dataview_sql_id"], -1);
                //            break;
                //        case "postgresql":
                //            txtPostgreSQL.Text = drS["sql_statement"].ToString();
                //            _sqlIDs["postgresql"] = Toolkit.ToInt32(drS["sys_dataview_sql_id"], -1);
                //            break;
                //        case "oracle":
                //            txtOracle.Text = drS["sql_statement"].ToString();
                //            _sqlIDs["oracle"] = Toolkit.ToInt32(drS["sys_dataview_sql_id"], -1);
                //            break;
                //        default:
                //            throw new InvalidOperationException("Unknown database engine '" + drS["database_engine_tag"].ToString() + "' for sql='" + drS["sql_statement"]);
                //    }
                //}


                // read in the SQL to create a default query object to populate with database values
                //_query = new Query(getRichTextBox(true).Text, AdminProxy.GetDataConnectionSpec(), AdminProxy.LanguageID, !ignoreErrors);
                _query = new Query("", AdminProxy.GetDataConnectionSpec(), AdminProxy.LanguageID, !ignoreErrors);

                var fields = new List<IField>();

                _dtLang = ds.Tables["sys_lang"];

                var dtFields = ds.Tables["dv_field_info"];
                if (_query.Selects.Count > 0) {

                    var validFields = new List<IField>();
                    for(var i=0;i<dtFields.Rows.Count;i++){
                        var drF = dtFields.Rows[i];
                        IField f = _query.Selects[0].GetField(drF["dv_field_name"].ToString());
                        if (f == null) {
                            // TODO: there's a field defined that does not exist in the SQL.  i.e. it's an orphaned mapping.
                            //       ignoring for now, but should we prompt the user?
                            Debug.WriteLine("what to do?");
                            f = new Field();
                        }

                        // since the SQL parsing may not coincide with what the database says the definition is (think about when the dataview is saved using SQL Server then imported to PostgreSQL)
                        // we will manually override the Fields collection in the sql parser.
                        // when things are in sync this will net zero change in the fields collection.
                        validFields.Add(f);

                        f.TableFieldName = drF["table_field_name"].ToString();
                        f.TableFieldID = Toolkit.ToInt32(drF["sys_table_field_id"], -1);
                        f.DataviewFieldName = drF["dv_field_name"].ToString();
                        f.DataviewFieldID = Toolkit.ToInt32(drF["sys_dataview_field_id"], -1);
                        f.TableName = drF["table_name"].ToString();
                        f.Table = _allTables.Find(t => String.Compare(t.TableName, f.TableName, true) == 0);

                        _query.Selects[0].AddTableIfNeeded(f.TableName, drF["dv_table_alias_name"].ToString(), _allTables, f.Table);

                        f.IsNullable = drF["table_field_nullable"].ToString() == "Y";
                        f.IsReadOnly = drF["dv_readonly"].ToString() == "Y" || drF["table_readonly"].ToString() == "Y" || drF["table_field_readonly"].ToString() == "Y" || drF["dv_field_readonly"].ToString() == "Y";
                        f.IsReadOnlyOnInsert = drF["table_readonly"].ToString() == "I" || drF["table_field_readonly"].ToString() == "I";
                        f.IsPrimaryKey = drF["dv_field_primary_key"].ToString() == "Y";
                        f.IsTransform = drF["dv_field_transform"].ToString() == "Y";
                        f.GuiHint = drF["gui_hint"].ToString();
                        f.ForeignKeyDataviewName = drF["foreign_key_dataview_name"].ToString();
                        f.GroupName = drF["dv_group_name"].ToString();

                        foreach (DataRow drLang in _dtLang.Rows) {
                            var langID = Toolkit.ToInt32(drLang["sys_lang_id"], -1);
                            var val = drF["friendly_name_for_" + langID].ToString();
                            f.DataviewFriendlyFieldNames[langID] = val;
                        }

                        //fields.Add(f.Clone(f.Table == null ? null : f.Table.Clone()));

                    }

                    _query.Selects[0].Fields = validFields;

                    //for (var i = 0; i < _query.Selects[0].Fields.Count; i++) {
                    //    var fld = _query.Selects[0].Fields[i];
                    //    if (!validFieldNames.Contains(fld.DataviewFieldName)) {
                    //        _query.Selects[0].Fields.Remove(fld);
                    //        i--;
                    //    }
                    //}
                }

                //if (_query.Selects.Count > 0) {
                //    foreach (var gf in _query.Selects[0].Fields) {
                //        // copy database field values in as needed
                //        //bool found = false;
                //        foreach (var dbf in fields) {
                //            if (String.Compare(dbf.DataviewFieldName, gf.DataviewFieldName, true) == 0) {

                //                gf.DataType = dbf.DataType;
                //                gf.DataTypeString = dbf.DataTypeString;
                //                gf.DataviewFieldID = dbf.DataviewFieldID;
                //                gf.DataviewFieldName = dbf.DataviewFieldName;
                //                foreach (var key in dbf.DataviewFriendlyFieldNames.Keys) {
                //                    gf.DataviewFriendlyFieldNames[key] = dbf.DataviewFriendlyFieldNames[key];
                //                }
                //                gf.DataviewID = dbf.DataviewID;
                //                gf.DataviewName = dbf.DataviewName;
                //                gf.DefaultValue = dbf.DefaultValue;
                //                foreach (var key in dbf.ExtendedProperties.Keys) {
                //                    gf.ExtendedProperties[key] = dbf.ExtendedProperties[key];
                //                }
                //                gf.FieldPurpose = dbf.FieldPurpose;
                //                gf.ForeignKeyDataviewName = dbf.ForeignKeyDataviewName;
                //                gf.ForeignKeyDataviewParam = dbf.ForeignKeyDataviewParam;
                //                gf.ForeignKeyTableFieldName = dbf.ForeignKeyTableFieldName;
                //                gf.ForeignKeyTableName = dbf.ForeignKeyTableName;
                //                gf.FriendlyDescription = dbf.FriendlyDescription;
                //                gf.FriendlyFieldName = dbf.FriendlyFieldName;
                //                gf.GroupName = dbf.GroupName;
                //                gf.GuiHint = dbf.GuiHint;
                //                gf.MaximumLength = dbf.MaximumLength;
                //                gf.MinimumLength = dbf.MinimumLength;
                //                gf.Precision = dbf.Precision;
                //                gf.Scale = dbf.Scale;
                //                gf.Table = dbf.Table;
                //                gf.TableFieldID = dbf.TableFieldID;
                //                gf.TableFieldName = dbf.TableFieldName;
                //                gf.TableName = dbf.TableName;
                //                //gf.IsAudit = dbf.IsAudit;
                //                gf.IsAutoIncrement = dbf.IsAutoIncrement;
                //                //gf.IsCreatedBy = dbf.IsCreatedBy;
                //                //gf.IsCreatedDate = dbf.IsCreatedDate;
                //                //gf.IsCreatedOrModifiedBy = dbf.IsCreatedOrModifiedBy;
                //                //gf.IsCreatedOrModifiedDate = dbf.IsCreatedOrModifiedDate;
                //                gf.IsForeignKey = dbf.IsForeignKey;
                //                //gf.IsModifiedBy = dbf.IsModifiedBy;
                //                //gf.IsModifiedDate = dbf.IsModifiedDate;
                //                gf.IsNullable = dbf.IsNullable;
                //                //gf.IsOwnedBy = dbf.IsOwnedBy;
                //                //gf.IsOwnedDate = dbf.IsOwnedDate;
                //                gf.IsPrimaryKey = dbf.IsPrimaryKey;
                //                gf.IsReadOnly = dbf.IsReadOnly;
                //                gf.IsReadOnlyOnInsert = dbf.IsReadOnlyOnInsert;
                //                gf.IsTransform = dbf.IsTransform;
                //                //found = true;
                //                break;
                //            }
                //        }
                //        //if (!found) {
                //        //    fields.Add(gf);
                //        //}
                //    }
                //    _query.Selects[0].Fields = fields;
                //}

                // wipe out parameters and repopulate with database values
                var prms = new List<IDataviewParameter>();
                var dtParams = ds.Tables["sys_dataview_param"];
                foreach (DataRow drP in dtParams.Rows) {
                    IDataviewParameter p = null;
                    p = _query.GetParameter(drP["param_name"].ToString());

                    if (p == null) {
                        p = new DataviewParameter();
                    }
                    p.Name = drP["param_name"].ToString();
                    p.TypeName = drP["param_type"].ToString();
                    p.Value = null;
                    if (!String.IsNullOrEmpty(p.Name)) {
                        prms.Add(p);
                    }
                }
                _query.Parameters = prms;

                // now re-sync everything, using the previous generator as the master
                syncParameters(true);
                syncFields(true);

                var dtDV = ds.Tables["sys_dataview"];
                foreach (DataRow dr in dtDV.Rows) {
                    chkIsTransform.Checked = dr["is_transform"].ToString() == "Y";
                    chkIsReadOnly.Checked = dr["is_readonly"].ToString() == "Y";

                    if (chkIsTransform.Checked) {
                        // transforms are read-only no matter what!!!
                        chkIsReadOnly.Checked = true;
                    }

                    //chkIsSystem.Checked = dr["is_system"].ToString() == "Y";
                    //chkIsUserVisible.Checked = dr["is_user_visible"].ToString() == "Y";
                    //chkIsWebVisible.Checked = dr["is_web_visible"].ToString() == "Y";

                    cboCategoryNameCode.Text = dr["category_code"].ToString();
                    cboDatabaseAreaCode.Text = dr["database_area_code"].ToString();
                    txtDatabaseAreaOrder.Text = dr["database_area_code_sort_order"].ToString();
                    _origDataviewName = dr["dataview_name"].ToString();
                    txtDataviewName.Text = _origDataviewName;
                    txtConfigurationOptions.Text = dr["configuration_options"].ToString();

                    ID = Toolkit.ToInt32(dr["sys_dataview_id"].ToString(), -1);

                    if (_query.Selects.Count > 0) {

                        initTransformDropDown(ddlFieldCaptionSource, _query.Selects[0].Fields, dr["transform_field_for_captions"].ToString());
                        initTransformDropDown(ddlFieldValueSource, _query.Selects[0].Fields, dr["transform_field_for_values"].ToString());
                        initTransformDropDown(ddlFieldNameSource, _query.Selects[0].Fields, dr["transform_field_for_names"].ToString());
                    } else {
                        initTransformDropDown(ddlFieldCaptionSource, null, dr["transform_field_for_captions"].ToString());
                        initTransformDropDown(ddlFieldValueSource, null, dr["transform_field_for_values"].ToString());
                        initTransformDropDown(ddlFieldNameSource, null, dr["transform_field_for_names"].ToString());
                    }

                }


                var dtTitle = ds.Tables["sys_dataview_lang"];

                dgvTitles.Rows.Clear();

                foreach (DataRow drLang in _dtLang.Rows) {
                    var langID = Toolkit.ToInt32(drLang["sys_lang_id"], -1);
                    var drTitles = dtTitle.Select("sys_lang_id = " + langID);
                    var title = "";
                    var desc = "";
                    var dataviewLangID = -1;
                    if (drTitles.Length > 0) {
                        var drt = drTitles[0];
                        dataviewLangID = Toolkit.ToInt32(drt["sys_dataview_lang_id"], -1);
                        title = drt["title"].ToString();
                        desc = drt["description"].ToString();
                    }
                    var values = new object[] { drLang["title"].ToString(), title, desc };
                    var newRowIndex = dgvTitles.Rows.Add(values);
                    dgvTitles.Rows[newRowIndex].Tag = dataviewLangID + "|" + langID;
                }


                //dgvPropertiesTitleDescription.AutoGenerateColumns = false;
                //dgvPropertiesTitleDescription.Columns[0].DataPropertyName = "language_name";
                //dgvPropertiesTitleDescription.Columns[1].DataPropertyName = "title";
                //dgvPropertiesTitleDescription.Columns[2].DataPropertyName = "description";
                //dgvPropertiesTitleDescription.DataSource = dtTitle;

                if (dtSqls.Rows.Count == 0) {
                    // must be a new dataview, default to making it visible in CT but not in web
                    chkIsUserVisible.Checked = true;
                    chkIsWebVisible.Checked = false;
                }

                MarkClean();
                _refreshing = false;
            });

        }

        private Query _query;

        //private void hideSqlTabsAsNeeded(TabPage currentTab, bool showAll) {
        //    try {
        //        tcSql.SuspendLayout();
        //        if (showAll) {
        //            if (!tcSql.TabPages.Contains(tpSqlServer)) {
        //                tcSql.TabPages.Add(tpSqlServer);
        //            }
        //            if (!tcSql.TabPages.Contains(tpPostgreSQL)) {
        //                tcSql.TabPages.Add(tpPostgreSQL);
        //            }
        //            if (!tcSql.TabPages.Contains(tpMySQL)) {
        //                tcSql.TabPages.Add(tpMySQL);
        //            }
        //            if (!tcSql.TabPages.Contains(tpOracle)) {
        //                tcSql.TabPages.Add(tpOracle);
        //            }
        //        } else {
        //            for (var i = 0; i < tcSql.TabPages.Count; i++) {
        //                if (tcSql.TabPages[i].Name != currentTab.Name) {
        //                    tcSql.TabPages.RemoveAt(i);
        //                    i--;
        //                }
        //            }
        //            if (!tcSql.TabPages.Contains(currentTab)) {
        //                tcSql.TabPages.Add(currentTab);
        //            }
        //        }
        //    } finally {
        //        tcSql.ResumeLayout();
        //    }
        //}

        //private IEnumerable<TextBox> getRichTextBoxes() {
        //    yield return txtSQLServer;
        //    yield return txtPostgreSQL;
        //    yield return txtMySQL;
        //    yield return txtOracle;
        //}

        ///// <summary>
        ///// Gets the active RichTextBox -- unless overridden by the useActualDatabaseConnection flag, which will then return the one for the current database connection
        ///// </summary>
        ///// <param name="useActualDatabaseConnection"></param>
        ///// <returns></returns>
        //private TextBox getRichTextBox(bool useActualDatabaseConnection) {
        //    if (useActualDatabaseConnection) {
        //        switch (AdminProxy.Connection.DatabaseEngineProviderName.ToLower()) {
        //            case "sqlserver":
        //                //return rtbSQLServer;
        //                return txtSQLServer;
        //            case "postgresql":
        //                //return rtbPostgreSQL;
        //                return txtPostgreSQL;
        //            case "mysql":
        //                //return rtbMySQL;
        //                return txtMySQL;
        //            case "oracle":
        //                //return rtbOracle;
        //                return txtOracle;
        //            default:
        //                throw new InvalidOperationException("Could not determine which TextBox to retrieve, invalid provider name: " + AdminProxy.GetDataConnectionSpec().ProviderName);
        //        }
        //    } else {
        //        foreach (var c in tcSql.SelectedTab.Controls) {
        //            if (c is TextBox) {
        //                return c as TextBox;
        //            }
        //        }
        //    }
        //    return null;
        //}

        private enum QueryMasterSource {
            None,
            QueryObject = 1,
            SqlEditorPane = 2,
            Gridviews = 3
        }

        private void syncGui(QueryMasterSource source) {
            try {
                syncGui(source, false);
            } catch (NotImplementedException nie) {
                MessageBox.Show(this, nie.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                syncGui(source, true);
            }
        }

        private void syncGui(QueryMasterSource source, bool ignoreErrors){

            Sync(true, delegate() {

                Debug.WriteLine("syncing gui");

                tmrQueryText.Stop();

                //switch (AdminProxy.Connection.DatabaseEngineProviderName) {
                //    case "sqlserver":
                //        hideSqlTabsAsNeeded(tpSqlServer, _showAllEngines);
                //        break;
                //    case "mysql":
                //        hideSqlTabsAsNeeded(tpMySQL, _showAllEngines);
                //        break;
                //    case "postgresql":
                //        hideSqlTabsAsNeeded(tpPostgreSQL, _showAllEngines);
                //        break;
                //    case "oracle":
                //        hideSqlTabsAsNeeded(tpOracle, _showAllEngines);
                //        break;
                //}


                // always parse the SQL of the 'real' engine -- the one that is for the current connection
                switch (source) {
                    case QueryMasterSource.None:
                        // gui sync that is completely unrelated to query parsing.
                        // do nothing with sql editor / gridviews / query object.
                        break;
                    case QueryMasterSource.QueryObject:
                        // query object is already properly filled. sync all GUI items to it.

                        syncParameters(true);
                        syncFields(true);
                        //getRichTextBox(true).Text = _query.Regenerate(!ignoreErrors);
                        _query.Regenerate(!ignoreErrors);

                        break;
                    case QueryMasterSource.SqlEditorPane:

                        // sql pane is master. parse its sql to fill query object appropriately, sync with gridviews
                        // _query.Parse(getRichTextBox(true).Text, !ignoreErrors);
                        _query.Parse("", !ignoreErrors);

                        // make gridview data match what's in the generator
                        syncParameters(true);
                        syncFields(true);

                        break;
                    case QueryMasterSource.Gridviews:
                        // gridviews is master.  fill query object appropriate, then regen and push into sql pane.
                        syncParameters(false);
                        syncFields(false);
                        // and assign it to the text box tied to the 'real' engine (i.e. we don't auto-update ones for engines that aren't the current one)
                        //getRichTextBox(true).Text = _query.Regenerate(!ignoreErrors);
                        _query.Regenerate(!ignoreErrors);

                        break;
                    default:
                        throw new NotImplementedException("QueryMasterSource = " + source.ToString() + " not defined in syncGui()");
                }

                CheckDirty();
                lblError.Visible = false;

            });
        }

        private void syncFields(bool queryObjectIsMaster) {

            Debug.WriteLine("syncing fields, queryObjectIsMaster=" + queryObjectIsMaster);

            var dgv = dgvFields;
            try {
                dgv.SuspendLayout();
                if (queryObjectIsMaster) {


                    if (_query.Selects.Count == 0) {
                        // wipe out gridview rows
                        dgv.Rows.Clear();
                    } else {
                        // query object does not carry the certain information (readonly, primary key, user interface, language), so copy it out from the gridview before wiping it out.

                        var stmt = _query.Selects[0];
                        for (var i = 0; i < stmt.Fields.Count; i++) {

                            var row = (DataGridViewRow)null;
                            for (var j = 0; j < dgv.Rows.Count; j++) {
                                var currow = dgv.Rows[j];
                                var tag = currow.Tag as FieldRowTag;
                                if (tag != null) {
                                    var offset = stmt.GetFieldOffset(tag.Field.DataviewFieldName);
                                    if (offset == i){
                                        row = currow;
                                        break;
                                    }
                                }
                            }
                            
                            if (row != null) {

                                var tag = row.Tag as FieldRowTag;
                                if (tag != null) {
                                    stmt.Fields[i].Table = tag.Table;
                                    if (stmt.Fields[i].Table != null) {
                                        stmt.Fields[i].TableFieldID = tag.Field.TableFieldID;
                                        stmt.Fields[i].TableName = tag.Table.TableName;
                                        stmt.Fields[i].TableFieldName = tag.Field.TableFieldName;
                                    }
                                }
                                stmt.Fields[i].IsNullable = !Toolkit.ToBoolean(row.Cells["colFieldRequired"].Value, true);
                                stmt.Fields[i].IsReadOnly = Toolkit.ToBoolean(row.Cells["colFieldReadOnly"].Value, true);
                                stmt.Fields[i].IsPrimaryKey = Toolkit.ToBoolean(row.Cells["colFieldPrimaryKey"].Value, true);
                                stmt.Fields[i].IsTransform = Toolkit.ToBoolean(row.Cells["colFieldTransform"].Value, true);
                                var guiHint = row.Cells["colFieldGuiHint"].Value + "";
                                if (String.Compare(guiHint, "(none)", true) == 0){
                                    guiHint = "";
                                }
                                stmt.Fields[i].GuiHint = guiHint;

                                var gn = row.Cells["colFieldGroupName"].Value + "";
                                if (String.Compare(gn, "(none)", true) == 0) {
                                    gn = "";
                                }
                                stmt.Fields[i].GroupName = gn;


                                var fkdv = row.Cells["colFieldForeignKey"].Value + "";
                                if (String.Compare(fkdv, "(none)", true) == 0) {
                                    fkdv = "";
                                //} else if (guiHint != "LARGE_SINGLE_SELECT_CONTROL") {
                                //    // only fields marked as having a Lookup Picker should have a dataview name associated with them.
                                //    // since the select may pull the one from the table, we need to manually override here as needed.
                                //    fkdv = "";
                                }
                                stmt.Fields[i].ForeignKeyDataviewName = fkdv; 

                                // language-specific stuff here
                                for (var j = (int)FieldColumnOffsets.LastHardcoded; j < dgv.Columns.Count; j++) {
                                    var dgvc = dgvFields.Columns[j];
                                    if (("" + dgvc.Tag).Contains("|")) {
                                        var split = dgvc.Tag.ToString().Split('|');
                                        stmt.Fields[i].DataviewFriendlyFieldNames[Toolkit.ToInt32(split[0], -1)] = row.Cells[j].Value + "";
                                    }
                                }
                            }
                        }


                        // wipe out gridview rows
                        dgv.Rows.Clear();

                        // re-init the table dropdown (which is driven by tables available in the SQL query
                        var dgddTable = dgvFields.Columns[(int)FieldColumnOffsets.TableName] as DataGridViewComboBoxColumn;
                        initTableDropDown(dgddTable, true, "(None)", _query.Selects[0].Tables);



                        for (var i = 0; i < stmt.Fields.Count; i++) {
                            var f = stmt.Fields[i];
                            var tableID = -1;
                            if (f.Table != null) {
                                tableID = f.Table.TableID;
                                if (!stmt.Tables.Exists(t => t.TableID == f.Table.TableID)) {
                                    // they may have mapped an invalid table (i.e. one that is not in the gen.Tables collection)
                                    // zero it out so we don't get an error.
                                    tableID = -1;
                                    f.Table = null;
                                }
                            }
                            var fieldID = f.TableFieldID > -1 ? f.TableFieldID : -1;
                            var guiHint = Toolkit.Coalesce(f.GuiHint, "") as string;
                            var fkdvnm = String.IsNullOrEmpty(f.ForeignKeyDataviewName) ? "(None)" : f.ForeignKeyDataviewName;
                            //if (guiHint != "LARGE_SINGLE_SELECT_CONTROL") {
                            //    // fk dataview names only apply to fields with lookup picker as the gui
                            //    fkdvnm = "(None)";
                            //}

                            var gn = String.IsNullOrEmpty(f.GroupName) ? "(None)" : f.GroupName;

                            var values = new List<object>(new object[] { f.DataviewFieldName, tableID, f.TableFieldID, !f.IsNullable, f.IsReadOnly, f.IsPrimaryKey, f.IsTransform,  guiHint, gn, fkdvnm });

                            // add language-specific columns...
                            foreach (DataRow drLang in _dtLang.Rows) {
                                var langVal = (string)null;
                                var langID = Toolkit.ToInt32(drLang["sys_lang_id"], -1);
                                f.DataviewFriendlyFieldNames.TryGetValue(langID, out langVal);
                                values.Add(langVal);
                            }

                            var index = dgv.Rows.Add(values.ToArray());
                            var row = dgv.Rows[index];
                            row.Tag = new FieldRowTag { Field = f, Table = f.Table, RowIndex = index };

                            // re-init the field dropdown (which is driven by the table dropdown)
                            initTableFieldDropDown(row.Cells[(int)FieldColumnOffsets.TableFieldName] as DataGridViewComboBoxCell, tableID, true, "(None)");

                            initGuiHintDropdown(row.Cells[(int)FieldColumnOffsets.UserInterface] as DataGridViewComboBoxCell, guiHint);

                            //((row.Cells[(int)FieldColumnOffsets.ForeignKeyDataviewName] as DataGridViewComboBoxCell).

                            // if (guiHint == "LARGE_SINGLE_SELECT_CONTROL") ;

                            //// sync the gui hint dropdown
                            //var ddlCellGuiHint = row.Cells[5] as DataGridViewComboBoxCell;
                            //var ddlDt = ddlCellGuiHint.DataSource as DataTable;
                            //for (var k = 0; k < ddlCellGuiHint.Items.Count; k++) {
                            //    var txt = ddlCellGuiHint.Items[k].ToString();
                            //    if (String.Compare(txt, guiHint, true) == 0){
                            //        ddlCellGuiHint.Value = txt;
                            //    }
                            //}

                        }


                        initTransformDropDown(ddlFieldCaptionSource, stmt.Fields, ddlFieldCaptionSource.Text);
                        initTransformDropDown(ddlFieldValueSource, stmt.Fields, ddlFieldValueSource.Text);
                        initTransformDropDown(ddlFieldNameSource, stmt.Fields, ddlFieldNameSource.Text);


                    }
                } else {
                    // sync fields collection to what's in the gridview
                    var newFields = new List<IField>();

                    var prevRowIndexes = new List<int>();
                    for (var i = 0; i < dgv.Rows.Count; i++) {
                        DataGridViewRow row = dgv.Rows[i];
                        if (!row.IsNewRow) {
                            // we skip the 'new row' row as it shouldn't have anything in it we care about
                            var tagValue = row.Tag as FieldRowTag;
                            if (tagValue == null) {
                                tagValue = new FieldRowTag();
                                tagValue.RowIndex = i;
                            }

                            prevRowIndexes.Add(tagValue.RowIndex);
                            tagValue.RowIndex = i;

                            var name = row.Cells[(int)FieldColumnOffsets.DataviewFieldName].Value + string.Empty;
                            var tableID = Toolkit.ToInt32(row.Cells[(int)FieldColumnOffsets.TableName].Value, -1);
                            var fieldID = Toolkit.ToInt32(row.Cells[(int)FieldColumnOffsets.TableFieldName].Value, -1);
                            
                            var chkReadOnly = ((DataGridViewCheckBoxCell)row.Cells[(int)FieldColumnOffsets.ReadOnly]);
                            var readOnly = Toolkit.ToBoolean(chkReadOnly.Value, false);

                            var chkRequired = ((DataGridViewCheckBoxCell)row.Cells[(int)FieldColumnOffsets.Required]);
                            var required = !Toolkit.ToBoolean(chkRequired.Value, true);
                            
                            var chkPrimaryKey = ((DataGridViewCheckBoxCell)row.Cells[(int)FieldColumnOffsets.PrimaryKey]);
                            var primaryKey = Toolkit.ToBoolean(chkPrimaryKey.Value, false);

                            var chkTransform = ((DataGridViewCheckBoxCell)row.Cells[(int)FieldColumnOffsets.TransformBy]);
                            var transform = Toolkit.ToBoolean(chkTransform.Value, false);

                            var guiHint = row.Cells[(int)FieldColumnOffsets.UserInterface].Value + string.Empty;
                            var groupName = row.Cells[(int)FieldColumnOffsets.DropDownGroupName].Value + string.Empty;
                            var foreignKeyDataviewName = row.Cells[(int)FieldColumnOffsets.LookupDataviewName].Value + string.Empty;


                            var tbl = null as ITable;
                            if (tableID > -1) {
                                foreach (var t in _query.Tables) {
                                    if (t.TableID == tableID) {
                                        tbl = t;
                                        break;
                                    }
                                }
                            }
                            tagValue.Table = tbl;

                            if (tagValue.Field == null) {

                                if (fieldID > -1 && tbl != null) {
                                    foreach (var f in tbl.Mappings) {
                                        if (fieldID == f.TableFieldID) {
                                            tagValue.Field = f;
                                            //field = f.Clone(tbl.Clone());
                                            break;
                                        }
                                    }
                                }

                                if (tagValue.Field == null) {
                                    tagValue.Field = new Field();
                                }
                            }

                            tagValue.Field.DataviewFieldName = name;
                            tagValue.Field.IsNullable = !required;
                            tagValue.Field.IsReadOnly = readOnly;
                            tagValue.Field.IsPrimaryKey = primaryKey;
                            tagValue.Field.IsTransform = transform;
                            tagValue.Field.GuiHint = String.IsNullOrEmpty(guiHint) ? "(None)" : guiHint;
                            tagValue.Field.GroupName = String.IsNullOrEmpty(groupName) ? "(None)" : groupName;
                            tagValue.Field.ForeignKeyDataviewName = String.IsNullOrEmpty(foreignKeyDataviewName) ? "(None)" : foreignKeyDataviewName;

                            // TODO: language-specific stuff here
                            for (var j = (int)FieldColumnOffsets.LastHardcoded; j < dgvFields.Columns.Count; j++) {
                                var dgvc = dgvFields.Columns[j];
                                if (("" + dgvc.Tag).Contains("|")) {
                                    var split = dgvc.Tag.ToString().Split('|');
                                    tagValue.Field.DataviewFriendlyFieldNames[Toolkit.ToInt32(split[0], -1)] = row.Cells[j].Value + "";
                                }
                            }

                            newFields.Add(tagValue.Field);
                        }
                    }
                    if (_query.Selects.Count > 0) {
                        _query.Selects[0].Fields = newFields;
                        // rearrange all the UNION select fields 
                        if (_query.Selects.Count > 1) {
                            for (var i = 1; i < _query.Selects.Count; i++) {
                                var sel = _query.Selects[i];
                                var fields = new List<IField>();
                                for (var j = 0; j < sel.Fields.Count; j++) {
                                    var fld = sel.Fields[prevRowIndexes[j]];
                                    // dataview field name should always match that of the first select statement
                                    fld.DataviewFieldName = _query.Selects[0].Fields[j].DataviewFieldName;
                                    fields.Add(sel.Fields[prevRowIndexes[j]]);
                                }
                                sel.Fields = fields;
                            }
                        }
                    }


                }
            } finally {
                dgv.ResumeLayout();
            }
        }

        private void syncParameters(bool queryObjectIsMaster) {
            Debug.WriteLine("syncing parameters, queryObjectIsMaster=" + queryObjectIsMaster);
            var dgv = dgvParameters;

            if (queryObjectIsMaster) {

                // sync gridview values to what's in the parameters collection

                // first, copy in any types from the previous generator if possible
                foreach (var p in _query.Parameters) {
                    var found = false;
                    foreach (var pp in _query.Parameters) {
                        if (pp.Name.ToLower() == p.Name.ToLower()) {
                            p.TypeName = pp.TypeName;
                            found = true;
                        }
                    }
                    if (!found) {
                        p.TypeName = "String";
                    }
                }

                // wipe out gridview rows
                try {
                    dgv.Rows.Clear();

                    for (var i = 0; i < _query.Parameters.Count; i++) {
                        var p = _query.Parameters[i];
                        var values = new object[2] { p.Name, p.TypeName };
                        dgv.Rows.Add(values);
                    }
                } catch (Exception ex) {
                    // eat this error, happens when they go from editing one field to the next
                    Debug.WriteLine(ex.Message);
                }
            } else {

                // sync parameters collection from gridview values
                var newParams = new List<IDataviewParameter>();

                foreach (DataGridViewRow row in dgv.Rows) {
                    if (!row.IsNewRow) {
                        var name = row.Cells[0].Value + string.Empty;
                        var type = (row.Cells[1] as DataGridViewComboBoxCell).Value + string.Empty;

                        var p = _query.GetParameter(name);
                        if (p != null) {
                            p.TypeName = type;
                            var found = false;
                            foreach (var np in newParams) {
                                if (String.Compare(np.Name, p.Name, true) == 0) {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                newParams.Add(p);
                            }
                        } else {
                            newParams.Add(new DataviewParameter { Name = name, TypeName = type });
                        }
                    }
                }

                _query.Parameters = newParams;

            }
        }

        internal override void FullScreenToggled(bool fullScreenEnabled) {
            if (fullScreenEnabled) {
                MessageBox.Show("Now in full screen");
            } else {
                MessageBox.Show("Now in 'normal' screen");
            }
        }

        private void initRichTextBox(RichTextBox rtb) {
            rtb.EnableAutoDragDrop = false;
            rtb.AllowDrop = true;
            rtb.DragEnter += new DragEventHandler(sqlQuery_DragEnter);
            rtb.DragDrop += new DragEventHandler(sqlQuery_DragDrop);
        }

        private bool _showAllEngines;

        private void frmDataview_Load(object sender, EventArgs e) {

            lblDropAs.Text = "";

            var parent = chkIsTransform.Parent;
            while (parent != null && parent.BackColor == Color.Transparent) {
                parent = parent.Parent;
            }
            chkIsTransform.BackColor = parent == null ?  this.BackColor : parent.BackColor;

            var dt = new DataTable("param_types");
            dt.Columns.Add("value_member", typeof(string));
            dt.Columns.Add("display_member", typeof(string));
            dt.Rows.Add("STRING", "String");
            dt.Rows.Add("INTEGER", "Integer");
            dt.Rows.Add("DATETIME", "DateTime");
            dt.Rows.Add("DOUBLE", "Double");
            dt.Rows.Add("DECIMAL", "Decimal");
            dt.Rows.Add("INTEGERCOLLECTION", "IntegerCollection");
            dt.Rows.Add("STRINGCOLLECTION", "StringCollection");
            dt.Rows.Add("STRINGREPLACEMENT", "StringReplacement");
            dt.Rows.Add("DECIMALCOLLECTION", "DecimalCollection");

            var colType = dgvParameters.Columns[1] as DataGridViewComboBoxColumn;
            colType.ValueMember = "value_member";
            colType.DisplayMember = "display_member";
            colType.DataSource = dt;

            if (Modal) {
                ControlBox = true;
                MaximizeBox = true;
                MinimizeBox = true;
                tcBottom.SelectedTab = tpProperties;
                txtDataviewName.Focus();
            }

            // read settings from registry, prevent gui from gyrating while we do so
            Sync(false, delegate() {
                chkAutoSynchronize.Checked = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "DataviewAutoSynchronize", "true")).ToLower() == "true";
                _showAllEngines = ("" + Toolkit.GetRegSetting(mdiParent.HIVE, "DataviewShowAllEngines", "false")).ToLower() == "true";

                //if (_showAllEngines) {
                //    // make sure our engine is the default one
                //    switch (AdminProxy.Connection.DatabaseEngineProviderName) {
                //        case "sqlserver":
                //            tcSql.SelectedTab = tpSqlServer;
                //            break;
                //        case "mysql":
                //            tcSql.SelectedTab = tpMySQL;
                //            break;
                //        case "postgresql":
                //            tcSql.SelectedTab = tpPostgreSQL;
                //            break;
                //        case "oracle":
                //            tcSql.SelectedTab = tpOracle;
                //            break;
                //    }
                //}

            });


            // we already manually synced everything from the RefreshData() call
            syncGui(QueryMasterSource.None, true);

            MarkClean();

        }

        private void tvByName_ItemDrag(object sender, ItemDragEventArgs e) {
            base.startDrag(sender, (TreeNode)e.Item);
        }

        DragDropObject _ddo;
        private void sqlQuery_DragEnter(object sender, DragEventArgs e) {
            var tb = (TextBox)sender;
            var ddo = getDragDropObject(e);
            if (ddo != null) {
                _ddo = ddo;
                if (ddo.SourceNode.Name.StartsWith("Table|") || ddo.SourceNode.Name.StartsWith("Field|") || ddo.SourceNode.Name.StartsWith("Folder|")) {
                    e.Effect = DragDropEffects.Copy;
                    Debug.WriteLine("drag enter, mouse = " + e.X + ", " + e.Y);
                    updateDropAsText(ddo, e.X, e.Y, tb);

                }
            }
        }

        private void sqlQuery_DragDrop(object sender, DragEventArgs e) {
            var rtb = (TextBox)sender;

            var ddo = getDragDropObject(e);
            if (ddo != null) {
                try {
                    var charIndex = rtb.GetCharIndexFromPosition(rtb.PointToClient(new Point { X = e.X, Y = e.Y }));
                    if (ddo.SourceNode.Name.StartsWith("Table|")) {
                        lblDropAs.Text = "";
                        rtb.Text = _query.AddTable(charIndex, (ITable)ddo.SourceNode.Tag, true, true);
                        rtb.SelectionStart = charIndex;
                        rtb.SelectionLength = 0;
                        rtb.ScrollToCaret();
                        rtb.Focus();
                        syncGui(QueryMasterSource.QueryObject);
                    } else if (ddo.SourceNode.Name.StartsWith("Folder|")) {
                        lblDropAs.Text = "";
                        rtb.Text = _query.AddTable(charIndex, (ITable)ddo.SourceNode.Tag, true, true);
                        rtb.SelectionStart = charIndex;
                        rtb.SelectionLength = 0;
                        rtb.ScrollToCaret();
                        rtb.Focus();
                        syncGui(QueryMasterSource.QueryObject);
                    } else if (ddo.SourceNode.Name.StartsWith("Field|")) {
                        lblDropAs.Text = "";
                        rtb.Text = _query.AddField(charIndex, (IField)ddo.SourceNode.Tag, true, true);
                        rtb.SelectionStart = charIndex;
                        rtb.SelectionLength = 0;
                        rtb.ScrollToCaret();
                        rtb.Focus();
                        syncGui(QueryMasterSource.QueryObject);
                    }
                } catch (NotImplementedException nie) {
                    MessageBox.Show(this, nie.Message, "No Relationship Defined", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            _ddo = null;

        }

        private Point getPointInForm(Control c, Point p) {
            var pt = c.Location;
            pt.Offset(p.X, p.Y);

            var parent = c.Parent;
            while (!(parent is Form)) {
                pt.Offset(parent.Location.X, parent.Location.Y);
                parent = parent.Parent;
            }
            return pt;
        }

        private static string CANCEL_CHARS = " \r\n\t=<>!@#$%^&*()-+/?.,`~\\|'\"";
        private static string WHITESPACE = " \r\n\t";

        private frmFindAndReplace _findAndReplace;
        private int _previousPeriodPosition;
        private void sqlQuery_KeyUp(object sender, KeyEventArgs e) {

            // grab previous word from textbox
            TextBox tb = null; // getRichTextBox(false);
            if (tb == null) {
                return;
            }




            var curPos = tb.SelectionStart;
            var txt = tb.Text;

            //Debug.WriteLine("KeyValue=" + e.KeyValue + ", KeyData=" + e.KeyData + ", KeyCode=" + e.KeyCode);
            if (e.KeyCode == Keys.OemPeriod){

                if (curPos > 2 && txt[curPos - 2] == '.') {
                    // cancel the suggestion listbox
                    lbSuggest.Visible = false;
                } else {

                    // display the suggestion listbox if we can
                    // but only use tables from the correct UNION clause

                    var pos = curPos - 2;
                    while (pos > 0 && CANCEL_CHARS.IndexOf(txt[pos]) < 0) {
                        pos--;
                    }
                    var tableName = txt.Substring(pos + 1, curPos - pos - 2);

                    // determine which UNION clause we're in...
                    if (_query.Selects.Count > 0) {
                        var stmt = _query.Selects[0];
                        var i=0;
                        while (i < _query.Selects.Count - 1 && pos > _query.Selects[i].EndsAtCharIndex) {
                            i++;
                            stmt = _query.Selects[i];
                        }

                        // grab table that matches that alias, if any
                        ITable table = null;
                        foreach (var t in stmt.Tables) {
                            if (String.Compare(t.AliasName, tableName, true) == 0 || String.Compare(t.TableName, tableName, true) == 0) {
                                table = t;
                                break;
                            }
                        }

                        if (table != null) {
                            if (lbSuggest.Tag == table) {
                                lbSuggest.Visible = true;
                                _previousPeriodPosition = curPos;
                            } else {
                                lbSuggest.Items.Clear();
                                foreach (var f in table.Mappings) {
                                    lbSuggest.Items.Add(f.TableFieldName);
                                }
                                if (lbSuggest.Items.Count > 0) {
                                    lbSuggest.Tag = table;
                                }
                            }

                            if (lbSuggest.Items.Count > 0) {
                                lbSuggest.SelectedIndex = 0;
                                var p = tb.GetPositionFromCharIndex(tb.SelectionStart);
                                var p2 = getPointInForm(tb, p);
                                lbSuggest.Left = p2.X;
                                lbSuggest.Top = p2.Y + lbSuggest.ItemHeight;
                                lbSuggest.BringToFront();
                                lbSuggest.Visible = true;
                                _previousPeriodPosition = curPos;
                            }

                        }
                    }
                }
            } else if (tb.SelectionStart == _previousPeriodPosition - 1 && (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)){
                // they deleted the period, hide the suggestion listbox
                lbSuggest.Visible = false;

            } else if (e.KeyCode == Keys.Escape){
                // they hit escape, hide the suggestion listbox if needed
                lbSuggest.Visible = false;
            } else {

                if (lbSuggest.Visible) {
                    var pos = _previousPeriodPosition;
                    while (pos < txt.Length && WHITESPACE.IndexOf(txt[pos]) < 0) {
                        pos++;
                    }

                    if (pos > _previousPeriodPosition) {

                        var curWord = txt.Substring(_previousPeriodPosition, pos - _previousPeriodPosition).ToLower();

                        switch (e.KeyCode) {
                            case Keys.Enter:
                            case Keys.Tab:
                            case Keys.Space:
                            case Keys.Up:
                            case Keys.Down:
                            case Keys.Right:
                            case Keys.PageUp:
                            case Keys.PageDown:
                                lbSuggest.Visible = false;
                                break;
                            default:
                                // auto-select one that closest matches current word
                                var found = false;
                                for (var i = 0; i < lbSuggest.Items.Count; i++) {
                                    var it = lbSuggest.Items[i].ToString();
                                    if (it.ToLower().StartsWith(curWord.ToLower())) {
                                        lbSuggest.SelectedIndex = i;
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found) {
                                    lbSuggest.Visible = false;
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void sqlQuery_KeyDown(object sender, KeyEventArgs e) {


            if ((Control.ModifierKeys & Keys.Control) == Keys.Control) {
                // show the find and replace dialog box
                if (e.KeyCode == Keys.F || e.KeyCode == Keys.H) {

                    TextBox tb = null; //getRichTextBox(false);
                    if (_findAndReplace == null) {
                        _findAndReplace = new frmFindAndReplace();
                        _findAndReplace.FormClosed += new FormClosedEventHandler(_findAndReplace_FormClosed);
                    }
                    _findAndReplace.TargetTextBox = tb;
                    _findAndReplace.txtFind.Text = tb.SelectedText;
                    _findAndReplace.Show(this);

                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    return;
                }
            }

            if (e.KeyCode == Keys.F3) {
                // find next, assuming find and replace is displaying
                if (_findAndReplace != null) {
                    _findAndReplace.doFind(true, 1, true);
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }


            if ((e.Modifiers & Keys.Control) == Keys.Control && e.KeyCode == Keys.A) {
                ((TextBox)sender).SelectAll();
                e.SuppressKeyPress = true;
            }

            if (lbSuggest.Visible) {
                switch (e.KeyCode) {
                    case Keys.Up:
                        if (lbSuggest.SelectedIndex > 0) {
                            lbSuggest.SelectedIndex = lbSuggest.SelectedIndex - 1;
                        }
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.PageUp:
                        if (lbSuggest.SelectedIndex > 4) {
                            lbSuggest.SelectedIndex = lbSuggest.SelectedIndex - 5;
                        } else {
                            lbSuggest.SelectedIndex = 0;
                        }
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Down:
                        if (lbSuggest.SelectedIndex < lbSuggest.Items.Count - 1) {
                            lbSuggest.SelectedIndex = lbSuggest.SelectedIndex + 1;
                        }
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.PageDown:
                        if (lbSuggest.SelectedIndex < lbSuggest.Items.Count - 5) {
                            lbSuggest.SelectedIndex = lbSuggest.SelectedIndex + 5;
                        } else {
                            lbSuggest.SelectedIndex = lbSuggest.Items.Count - 1;
                        }
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Enter:
                    case Keys.Space:
                    case Keys.Tab:
                        if (lbSuggest.SelectedItem != null) {
                            var tb = (TextBox)sender;
                            var firstText = Toolkit.Cut(tb.Text, 0, _previousPeriodPosition) + lbSuggest.SelectedItem.ToString();
                            tb.Text = firstText + Toolkit.Cut(tb.Text, tb.SelectionStart);
                            tb.SelectionStart = firstText.Length;
                            tb.SelectionLength = 0;
                        }
                        lbSuggest.Visible = false;
                        if (e.KeyCode != Keys.Space) {
                            e.SuppressKeyPress = true;
                        }
                        break;
                    case Keys.Escape:
                        lbSuggest.Visible = false;
                        e.SuppressKeyPress = true;
                        break;
                }
            }



        }

        void _findAndReplace_FormClosed(object sender, FormClosedEventArgs e) {
            _findAndReplace = null;
        }

        private void chkShowAllEngines_CheckedChanged(object sender, EventArgs e) {
            syncGui(QueryMasterSource.SqlEditorPane);
        }

        private void tcSql_DragEnter(object sender, DragEventArgs e) {
        }

        private void btnCancel_Click(object sender, EventArgs e) {

            if (IsDirty()) {
                var dr = MessageBox.Show(this, "You have changes that have not yet been saved.\nDo you want to save them now?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (dr) {
                    case DialogResult.Yes:
                        // save it
                        btnSave.PerformClick();
                        return;
                    case DialogResult.No:
                        // they want to ignore changes, continue closing
                        MainFormSelectParentTreeNode();
                        return;
                    case DialogResult.Cancel:
                        // they asked to cancel the cancel, so do nothing
                        DialogResult = DialogResult.None;
                        return;
                    default:
                        break;
                }
            } else {
                MainFormSelectParentTreeNode();
            }
            
            DialogResult = DialogResult.Cancel;


        }

        private void btnSave_Click(object sender, EventArgs e) {

            var dvName = txtDataviewName.Text.Trim();

            // TODO: scrub input!
            if (dvName.Length == 0) {
                MessageBox.Show(this, "You must provide a name for the dataview.", "Provide Name");
                tcBottom.SelectedTab = tpProperties;
                txtDataviewName.Focus();
                DialogResult = DialogResult.None;
                return;
            }


            // 'lookup' dataviews must have value_member and display_member fields, and value_member must be an int and a primary key.
            if (String.Compare(cboDatabaseAreaCode.Text, "lookup table", true) == 0 ) {
                var sel = _query.Selects[0];
                var unmapped = new List<string>();
                var foundValueMember = false;
                var foundDisplayMember = false;
                var pkCount = 0;
                var pkIsInteger = false;
                foreach (var f in sel.Fields) {
                    if (f.IsPrimaryKey) {
                        pkCount++;
                        pkIsInteger = f.DataType == typeof(int) && f.Table != null;
                    }
                    if (f.DataviewFieldName.ToLower() == "value_member") {
                        foundValueMember = true;
                        // invalidValueMemberField = f.DataType != typeof(int) || f.Table == null;
                        //if (!invalidValueMemberField) {
                        //    invalidValueMemberField = !f.IsPrimaryKey;
                        //}
                    } else if (f.DataviewFieldName.ToLower() == "display_member") {
                        foundDisplayMember = true;
                    }
                }

                if (!foundDisplayMember || !foundValueMember) {
                    MessageBox.Show(this, "For a lookup dataview, a field named 'value_member' and 'display_member' must be provided.", "Missing Dataview Field");
                    return;
                } else if (pkCount != 1 || !pkIsInteger) {
                    MessageBox.Show(this, "For a lookup dataview, a primary key must be specified and it must mapped to a single table/field with an integer data type.", "Invalid Primary Key Field Mapping");
                    return;
                }
                //} else if (invalidValueMemberField) {
                //    //MessageBox.Show(this, "For a lookup dataview, the field named 'value_member' must be mapped to a specific table/field with an integer data type.\n  It must also be marked as the dataview's primary key.", "Invalid value_member Field Mapping");
                //    MessageBox.Show(this, "For a lookup dataview, the field named 'value_member' must be mapped to a specific table/field with an integer data type.", "Invalid value_member Field Mapping");
                //    return;
                //}
            }

            // 'import' dataviews must meet a specific set of requirements so the import wizard works properly
            if (String.Compare(cboCategoryNameCode.Text, "import wizard", true) == 0) {
                var sel = _query.Selects[0];
                var unmapped = new List<string>();
                var pks = new List<string>();
                var noUniqueKeyFields = new List<string>();
                var missingUniqueKeyFields = new List<string>();
                var missingRequiredTables = new List<string>();
                var mismappedForeignKeys = new List<string>();
                foreach (var f in sel.Fields) {
                    if (f.Table == null) {
                        // field doesn't map to a table
                        unmapped.Add(f.DataviewFieldName);
                    } else if (f.Table.GetField(f.TableFieldName).IsPrimaryKey) {
                        // field is a primary key
                        pks.Add(f.DataviewFieldName);
                    } else if (String.IsNullOrEmpty(f.Table.UniqueKeyFields)) {
                        // a table is mapped that does not have any unique key fields defined on it
                        noUniqueKeyFields.Add(f.TableName + " (via the " + f.DataviewFieldName + " field)");
                    } else {
                        // see if all unique key fields for the table exist (they may have mapped some but not all)
                        var ukFields = f.Table.UniqueKeyFields.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ukf in ukFields) {
                            var found = false;
                            foreach(IField sf in sel.Fields){
                                if (String.Compare(sf.TableName, f.TableName, true) == 0 && String.Compare(sf.TableFieldName, ukf, true) == 0) {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                var tf = f.Table.GetField(ukf);
                                if (tf.IsPrimaryKey) {
                                    // missing primary keys is ok, that's the point of importing! :)
                                } else {
                                    // but missing foreign keys that point to tables not in the dataview are not
                                    if (tf.IsForeignKey && tf.DataType == typeof(int)) {

                                        if (String.IsNullOrEmpty(tf.ForeignKeyTableName)) {
                                            mismappedForeignKeys.Add(tf.TableName + "." + tf.TableFieldName);
                                        } else {

                                            var foundFKTable = false;
                                            foreach (IField sf2 in sel.Fields) {
                                                if (string.Compare(sf2.TableName, tf.ForeignKeyTableName, true) == 0) {
                                                    foundFKTable = true;
                                                }
                                            }
                                            if (!foundFKTable) {
                                                if (tf.ForeignKeyTableName != "sys_lang") {
                                                    // sys_lang table is a special case in that the import wizard uses a dropdown on the first screen to handle it
                                                    if (!missingRequiredTables.Contains(tf.TableName)) {
                                                        missingRequiredTables.Add(tf.ForeignKeyTableName + " (required by the " + tf.TableName + " table)");
                                                    }
                                                }
                                            }
                                        }
                                    } else {
                                        // neither is missing non-foreign key fields
                                        missingUniqueKeyFields.Add(ukf + " (for table " + f.TableName + ")");
                                    }
                                }
                            }
                        }
                    }
                }
                if (unmapped.Count > 0) {
                    MessageBox.Show(this, "For an import dataview to work properly, all fields must be mapped to a table.\nThe following fields are not:\n\n" + String.Join("\r\n", unmapped.ToArray()));
                    return;
                } else if (pks.Count > 0) {
                    MessageBox.Show(this, "For an import dataview to work properly, no primary key fields may be included.\nThe following fields are primary keys:\n\n" + String.Join("\r\n", pks.ToArray()));
                    return;
                } else if (noUniqueKeyFields.Count > 0) {
                    MessageBox.Show(this, "For an import dataview to work properly, all fields must be mapped to a table with a unique key defined.\nThe following tables do not define a unique key:\n\n" + String.Join("\n", noUniqueKeyFields.ToArray()));
                    return;
                } else if (missingUniqueKeyFields.Count > 0) {
                    MessageBox.Show(this, "For an import dataview to work properly, all unique key fields for a mapped table must exist in the dataview.\nThe following unique key fields are missing:\n\n" + String.Join("\n", missingUniqueKeyFields.ToArray()));
                    return;
                } else if (mismappedForeignKeys.Count > 0){
                    MessageBox.Show(this, "For an import dataview to work properly, all foriegn key fields must be mapped to a valid table.\nThe following foreign key fields are not properly mapped to tables:\n\n" + String.Join("\n", mismappedForeignKeys.ToArray()));
                    return;
                } else if (missingRequiredTables.Count > 0){
                    MessageBox.Show(this, "For an import dataview to work properly, all tables that comprise a unique key must be given.\nThe following tables are missing:\n\n" + String.Join("\r\n", missingRequiredTables.ToArray()));
                    return;
                }
            }

            if (String.Compare(cboCategoryNameCode.Text, "search engine", true) == 0) {

                // TODO: add search engine restriction checks here.  i.e. make sure they have a /*_ WHERE blah=blah2 _*/ clause, 2 fields for resolvers, etc

            }




            DialogResult = DialogResult.OK;


            if (!String.IsNullOrEmpty(_origDataviewName) && String.Compare(_origDataviewName, txtDataviewName.Text.Trim(), true) != 0) {
                // since dataviews are sync'd by dataview name, we must do an explicit rename if they changed its name
                AdminProxy.RenameDataViewDefinition(_origDataviewName, txtDataviewName.Text.Trim());
            }

            // get a bogus custom view just for the datatable structures
            DataSet dsInfo = AdminProxy.GetDataViewDefinition("__This is a very!!!! unlikely name for a dataview....!@#$)*(%&*()");
            DataTable dtParam = dsInfo.Tables["sys_dataview_param"];
            DataTable dtFriendly = dsInfo.Tables["dv_field_info"];
            DataTable dtSql = dsInfo.Tables["sys_dataview_sql"];
            DataTable dtTitle = dsInfo.Tables["sys_dataview_lang"];

            // NOTE: since the UI allows them to enter data at various places for the same information (e.g. edit SQL or edit gridview(s))
            //       we don't pull info directly from the GUI.  We pull it from the last-synchronized SqlGenerator object.  So this save
            //       code relies on that object being up to date with whatever is in the GUI.

            // add parameter info
            foreach (var prm in _query.Parameters) {
                DataRow drParam = dtParam.NewRow();
                drParam["sys_dataview_id"] = ID;
                drParam["param_name"] = prm.Name;
                drParam["param_type"] = prm.TypeName.ToUpper();
                dtParam.Rows.Add(drParam);
            }

            // add friendly field info
            if (_query.Selects.Count > 0) {
                for (var i = 0; i < _query.Selects[0].Fields.Count; i++) {
                    var fld = _query.Selects[0].Fields[i];

                    DataRow drFriendly = dtFriendly.NewRow();
                    drFriendly["sys_dataview_field_id"] = fld.DataviewFieldID; // note this field is optional, but required for doing deletes!
                    drFriendly["dv_field_sort_order"] = i;
                    drFriendly["dv_field_transform"] = fld.IsTransform;
                    drFriendly["dv_field_name"] = fld.DataviewFieldName;
                    drFriendly["dv_field_readonly"] = fld.IsReadOnly ? "Y" : fld.IsReadOnlyOnInsert ? "I" : "N";
                    drFriendly["dv_field_primary_key"] = fld.IsPrimaryKey ? "Y" : "N";
                    drFriendly["dv_field_transform"] = fld.IsTransform ? "Y" : "N";

                    var fkdv = fld.ForeignKeyDataviewName;
                    if (String.Compare(fkdv, "(none)", true) == 0) {
                        fkdv = "";
                    }
                    drFriendly["foreign_key_dataview_name"] = fkdv;

                    var gn = fld.GroupName;
                    if (String.Compare(gn, "(none)", true) == 0) {
                        gn = "";
                    }
                    drFriendly["dv_group_name"] = gn;

                    drFriendly["table_name"] = fld.TableName;
                    drFriendly["table_field_name"] = fld.TableFieldName;
                    drFriendly["gui_hint"] = fld.GuiHint;
                    foreach (DataRow drLang in _dtLang.Rows) {
                        var langID = Toolkit.ToInt32(drLang["sys_lang_id"], -1);
                        string friendlyName = null;
                        if (fld.DataviewFriendlyFieldNames.TryGetValue(langID, out friendlyName)) {
                            drFriendly["friendly_name_for_" + langID] = friendlyName;
                        } else {
                            drFriendly["friendly_name_for_" + langID] = null;
                        }
                    }

                    dtFriendly.Rows.Add(drFriendly);

                }
            }

            // add sql info
            string[] engines = new string[] { "sqlserver", "mysql", "oracle", "postgresql" };
            for (int j = 0; j < engines.Length; j++) {
                var engine = engines[j];
                string sql = null;
                //switch (engine) {
                //    case "sqlserver":
                //        sql = txtSQLServer.Text;
                //        break;
                //    case "mysql":
                //        sql = txtMySQL.Text;
                //        break;
                //    case "oracle":
                //        sql = txtOracle.Text;
                //        break;
                //    case "postgresql":
                //        sql = txtPostgreSQL.Text;
                //        break;
                //}
                if (!String.IsNullOrEmpty(sql)) {
                    DataRow drSql = dtSql.NewRow();
                    drSql["sys_dataview_sql_id"] = _sqlIDs[engine];
                    drSql["sys_dataview_id"] = ID;
                    drSql["database_engine_tag"] = engine;
                    drSql["sql_statement"] = sql;
                    if (!String.IsNullOrEmpty(sql)) {
                        dtSql.Rows.Add(drSql);
                    }
                }
            }

            // since the titles and descriptions have nothing to sync with, we pull their values
            // directly from the gridview we display them in

            // add title/description info
            foreach (DataGridViewRow row in dgvTitles.Rows) {

                string title = row.Cells[1].Value + string.Empty;
                string description = row.Cells[2].Value + string.Empty;

                if (!String.IsNullOrEmpty(title) || !String.IsNullOrEmpty(description)) {
                    var split = (string.Empty + row.Tag).Split('|');
                    var drTitle = dtTitle.NewRow();
                    drTitle["sys_dataview_lang_id"] = Toolkit.ToInt32(split[0], -1);
                    drTitle["sys_dataview_id"] = ID;
                    drTitle["sys_lang_id"] = split.Length > 1 ? Toolkit.ToInt32(split[1], -1) : -1;
                    drTitle["title"] = title;
                    drTitle["description"] = description;
                    dtTitle.Rows.Add(drTitle);
                }
            }

            //AdminProxy.CreateDataViewDefinition(txtDataviewName.Text.Trim(), chkIsReadOnly.Checked, false, chkIsSystem.Checked, chkIsUserVisible.Checked, chkIsWebVisible.Checked, "", "", "", cboCategoryName.Text, cboDatabaseArea.Text, Toolkit.ToInt32(txtDatabaseAreaOrder.Text, 0), chkIsTransform.Checked, ddlFieldNameSource.Text, ddlFieldCaptionSource.Text, ddlFieldValueSource.Text, dsInfo);
            AdminProxy.CreateDataViewDefinition(txtDataviewName.Text.Trim(), chkIsReadOnly.Checked, cboCategoryNameCode.Text, cboDatabaseAreaCode.Text, Toolkit.ToInt32(txtDatabaseAreaOrder.Text, 0), chkIsTransform.Checked, ddlFieldNameSource.Text, ddlFieldCaptionSource.Text, ddlFieldValueSource.Text, txtConfigurationOptions.Text, dsInfo);
            lblError.Visible = false;

            MarkClean();

            DataviewName = txtDataviewName.Text;

            if (!Modal) {
                // don't close the form
                DialogResult = DialogResult.None;
            } else {
                // let the form close, we're already on the appropriate tree node.
                if (MainFormIsCurrentNodeDescendentOf("ndDataviews")) {
                    MainFormRefreshData();
                }
            }
            MainFormUpdateStatus("Saved " + dvName + " ", true);

        }

        private void dgvParameters_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
            if (e.ColumnIndex == 0) {
                var row = dgvParameters.Rows[e.RowIndex];
                var name = e.FormattedValue + "";

                if (e.RowIndex == dgvParameters.NewRowIndex) {
                    // it's the new row, just ignore
                } else {

                    if (name.Trim().Length == 0) {
                        // they blanked out the name, assume they're removing the parameter.
                    } else {

                        if (!name.StartsWith(":") && !name.StartsWith("@") && !name.StartsWith("?")) {
                            MessageBox.Show(this, "Parameter names must begin with one of the following:\n:\n@\n?", "Invalid Parameter Name", MessageBoxButtons.OK);
                            e.Cancel = true;
                        }
                    }
                }
            }
        }

        void tmrQueryText_Tick(object sender, EventArgs e) {
            if (chkAutoSynchronize.Checked) {
                //if (dgvFields.Focused || dgvParameters.Focused){
                //    // gridviews have focus, assume they're master
                //    syncGui(QueryMasterSource.Gridviews);
                //} else {
                    // sql editor has focus, assume it's master
                    syncGui(QueryMasterSource.SqlEditorPane);
                //}
            }
            tmrQueryText.Stop();
        }

        private void sqlQuery_TextChanged(object sender, EventArgs e) {
            // toggle the timer so the previous event handler does not trigger syncGui to be called
            // (i.e. only do it roughly 1 second after they quit typing, not every single time they enter a character)
            var txt = ((TextBox)sender).Text;
            if (chkAutoSynchronize.Checked) {
                if (txt.Length == 0) {
                    // immediately refresh since we know it's empty
                    tmrQueryText.Stop();
                    syncGui(QueryMasterSource.SqlEditorPane);
                } else {
                    Sync(false, delegate() {
                        // restart the timer
                        tmrQueryText.Stop();
                        tmrQueryText.Start();
                    });
                }
            } else {
                CheckDirty();
            }
        }

        private void dgvParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex > -1) {
                var row = dgvParameters.Rows[e.RowIndex];
                var cell = dgvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var cellValue = string.Empty + cell.Value;
                var f = dgvParameters.Rows[e.RowIndex].Tag as IField;

                // add to top-level parameters collection as needed...
                while (_query.Parameters.Count <= e.RowIndex) {
                    _query.Parameters.Add(new DataviewParameter { Name = dgvParameters.Rows[e.RowIndex].Cells[0].Value + "", TypeName = "String" });
                }

                //// and statement-level parameters collection as needed...
                //for (var i = 0; i < _query.Selects.Count; i++) {
                //    var stmt = _query.Selects[i];
                //    while (stmt.Parameters.Count <= e.RowIndex) {
                //        stmt.Parameters.Add(new DataviewParameter { Name = dgvParameters.Rows[e.RowIndex].Cells[0].Value + "", TypeName = "String" });
                //    }
                //}

                switch (e.ColumnIndex) {
                    case 0:
                        // they changed the parameter name that should be in the sql. sync the sql.

                        var prevName = _query.Parameters[e.RowIndex].Name;
                        var prm = _query.Parameters[e.RowIndex];

                        if (cellValue.Trim().Length == 0) {
                            _query.Parameters.Remove(prm);
                            dgvParameters.Rows.RemoveAt(e.RowIndex);
                        } else {

                            // change it for the entire query...
                            prm.ExtendedProperties["previous_name"] = prevName;
                            prm.Name = cellValue;

                            //// and all its statements as well...
                            //for (var i = 0; i < _query.Selects.Count; i++) {
                            //    var stmt = _query.Selects[i];
                            //    prevName = stmt.Parameters[e.RowIndex].Name;
                            //    prm = stmt.Parameters[e.RowIndex];
                            //    prm.ExtendedProperties["previous_name"] = prevName;
                            //    prm.Name = cellValue;
                            //}
                        }
                        syncGui(QueryMasterSource.QueryObject);
                        break;
                    case 1:
                        // they changed the parameter type.  this is not in the sql, so no need to sync.
                        _query.Parameters[e.RowIndex].TypeName = cellValue;
                        // and all its statements as well...
                        //for (var i = 0; i < _query.Selects.Count; i++) {
                        //    var stmt = _query.Selects[i];
                        //    stmt.Parameters[e.RowIndex].TypeName = cellValue;
                        //}
                        CheckDirty();
                        break;
                }
            }
        }

        private IField getTableField(ITable table, int fieldID) {
            if (table != null) {
                foreach (IField fld in table.Mappings) {
                    if (fld.TableFieldID == fieldID) {
                        return fld;
                    }
                }
            }
            return null;
        }

        private void dgvFields_CellEndEdit(object sender, DataGridViewCellEventArgs e) {


            if (e.RowIndex > -1){
                var row = dgvFields.Rows[e.RowIndex];
                var cell = dgvFields.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var cellValue = string.Empty + cell.Value;
                var rowTagValue = dgvFields.Rows[e.RowIndex].Tag as FieldRowTag;
                var colTagValue = dgvFields.Columns[e.ColumnIndex].Tag as string;

                switch (e.ColumnIndex) {
                    case (int)FieldColumnOffsets.ReadOnly:
                        // changed readonly flag
                        var readOnly = Toolkit.ToBoolean(cellValue, false);
                        if (!readOnly) {
                            var tableFieldID = Toolkit.ToInt32(dgvFields.Rows[e.RowIndex].Cells[(int)FieldColumnOffsets.TableFieldName].Value, -1);
                            var tableField = getTableField(rowTagValue.Table, tableFieldID);
                            if (tableField != null) {
                                if (tableField.IsPrimaryKey || tableField.IsReadOnly) {
                                    // this table defines itself as never being able to write to this field.
                                    MessageBox.Show(this, "This field is marked as read only in the table mapping for " + rowTagValue.Table.TableName + ".\r\nThis cannot be overridden in the dataview, so it will remain read only.", "Table Mapping Is Read Only");
                                    readOnly = true;
                                    cell.Value = readOnly;
                                } else {
                                    if (rowTagValue.Field.GuiHint == "LARGE_SINGLE_SELECT_CONTROL" && (!tableField.IsForeignKey || String.IsNullOrEmpty(tableField.ForeignKeyTableFieldName))){
                                        MessageBox.Show(this, "This field is not defined as a foreign key in the table mappings.\r\n\r\nPlease edit the table mappings for " + rowTagValue.Table.TableName + "." + tableField.TableFieldName + " as follows:\r\n\r\n - Put a check next to 'Is Foreign Key'\r\n - Specify a valid field for 'Foreign Key Detail'\r\n", "Missing Foreign Key Mapping");
                                        readOnly = true;
                                        cell.Value = readOnly;
                                    }
                                }
                            }
                        }
                        rowTagValue.Field.IsReadOnly = readOnly;
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.PrimaryKey:
                        // changed primary key flag
                        rowTagValue.Field.IsPrimaryKey = Toolkit.ToBoolean(cellValue, false);
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.UserInterface:
                        // changed user interface value
                        rowTagValue.Field.GuiHint = cellValue; // convertFieldGuiHint(cellValue);

                        switch(rowTagValue.Field.GuiHint.ToString()){
                            case "LARGE_SINGLE_SELECT_CONTROL":
                                row.Cells[(int)FieldColumnOffsets.DropDownGroupName].ReadOnly = true;
                                row.Cells[(int)FieldColumnOffsets.LookupDataviewName].ReadOnly = false;
                                break;
                            case "SMALL_SINGLE_SELECT_CONTROL":
                                row.Cells[(int)FieldColumnOffsets.DropDownGroupName].ReadOnly = false;
                                row.Cells[(int)FieldColumnOffsets.LookupDataviewName].ReadOnly = true;
                                break;
                            default:
                                // neither, disable both
                                row.Cells[(int)FieldColumnOffsets.DropDownGroupName].ReadOnly = false;
                                row.Cells[(int)FieldColumnOffsets.LookupDataviewName].ReadOnly = false;
                                break;
                        }

                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.DropDownGroupName:
                        rowTagValue.Field.GroupName = cellValue;
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.LookupDataviewName:
                        // changed foreign key dataview name
                        rowTagValue.Field.ForeignKeyDataviewName = cellValue.ToString();
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.TransformBy:
                        // changed transform by flag
                        rowTagValue.Field.IsTransform = Toolkit.ToBoolean(cellValue, false);
                        if (!rowTagValue.Field.IsReadOnly) {
                            dgvFields.Rows[e.RowIndex].Cells[(int)FieldColumnOffsets.ReadOnly].Value = true;
                            MessageBox.Show(this, "Transforming on a field requires it to be read only as well, so the Read Only flag has been set.", "Read Only Required");
                        }
                        CheckDirty();
                        break;

                    default:
                        // edited the friendly name (language specific one)

                        var split = colTagValue.Split('|');
                        var lid = Toolkit.ToInt32(split[0], -1);

                        rowTagValue.Field.DataviewFriendlyFieldNames[lid] = cellValue.ToString();

                        // editing a value which is not shown in the SQL, but is important to remember.
                        // nothing to do except update the 
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.DataviewFieldName:

                        if (_query.Selects.Count > 0) {
                            var field = _query.Selects[0].Fields[e.RowIndex];

                            var nameChanged = true;
                            if (field.DataviewFieldName == cellValue) {
                                nameChanged = false;
                            }
                            if (nameChanged) {
                                // they changed the field name that should be in the sql. sync the sql.
                                field.ExtendedProperties["previous_name"] = field.DataviewFieldName;
                                field.DataviewFieldName = cellValue;
                                rowTagValue.Field = field;
                                //if (chkAutoSynchronize.Checked) {
                                //    syncGui(QueryMasterSource.Gridviews);
                                //}
                            }
                        }
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.TableName:
                        // they changed the table.
                        // sync the field dropdown as needed

                        var tableChanged = true;
                        var val = Toolkit.ToInt32(cellValue, -1);
                        if (rowTagValue.Table != null) {
                            if (rowTagValue.Table.TableID == val) {
                                tableChanged = false;
                            }
                        }
                        if (tableChanged) {
                            Sync(true, delegate() {
                                var fieldCell = row.Cells[(int)FieldColumnOffsets.TableFieldName] as DataGridViewComboBoxCell;
                                var rows = (fieldCell.DataSource as DataTable).Select("sys_table_field_id = " + fieldCell.Value);
                                var fieldName = "";
                                if (rows.Length > 0){
                                    fieldName = "" + rows[0]["field_name"];
                                }
                                //var tableField = getTableField(rowTagValue.Table, Toolkit.ToInt32(fieldCell.Value, -1));

                                var dt = initTableFieldDropDown(fieldCell, val, true, "(None)");

                                rows = (fieldCell.DataSource as DataTable).Select("field_name = '" + fieldName.Replace("'", "''") + "'");
                                if (rows.Length > 0) {
                                    fieldCell.Value = Toolkit.ToInt32(rows[0]["sys_table_field_id"], -1);
                                } else {
                                    fieldCell.Value = 0;
                                }

                                rowTagValue.Table = _query.AddTableIfNeeded(cellValue, null, _allTables, null);
                                rowTagValue.Field.Table = rowTagValue.Table;
                                rowTagValue.Field.TableFieldID = (int)fieldCell.Value;
                                rowTagValue.Field.TableFieldName = fieldName;
                                if (rowTagValue.Field.Table == null) {
                                    rowTagValue.Field.TableName = "";
                                    rowTagValue.Field.ExtendedProperties["text"] = "";
                                } else {
                                    rowTagValue.Field.TableName = rowTagValue.Field.Table.TableName;
                                    var alias = rowTagValue.Table.AliasName;
                                    if (!String.IsNullOrEmpty(alias + "")) {
                                        rowTagValue.Field.ExtendedProperties["text"] = rowTagValue.Table.AliasName + "." + rowTagValue.Field.TableFieldName;
                                    } else {
                                        rowTagValue.Field.ExtendedProperties["text"] = rowTagValue.Field.TableFieldName;
                                    }
                                }

                                //var text = fieldCell.FormattedValue + string.Empty;
                                //foreach (DataRow dr in dt.Rows) {
                                //    if (String.Compare(dr["field_name"].ToString(), text, true) == 0) {
                                //        fieldCell.Value = Toolkit.ToInt32(dr["sys_table_field_id"], -1);
                                //    }
                                //}
                            });
                            //if (chkAutoSynchronize.Checked) {
                            //    syncGui(QueryMasterSource.Gridviews);
                            //}
                        }
                        CheckDirty();
                        break;
                    case (int)FieldColumnOffsets.TableFieldName:
                        // they changed the table.field this dataview field maps to.
                        // add it, save it in our Tag object for this row

                        var fieldChanged = true;
                        if (rowTagValue.Field.DataviewFieldID.ToString() == cellValue) {
                            fieldChanged = false;
                        }

                        if (fieldChanged) {
                            var cboCell = cell as DataGridViewComboBoxCell;
                            var cellTableFieldID = "";
                            foreach (DataRowView rv in cboCell.Items) {
                                if (rv[cboCell.ValueMember].ToString() == cellValue) {
                                    cellTableFieldID = rv[cboCell.ValueMember].ToString();
                                    break;
                                }
                            }

                            var newField = _query.AddFieldIfNeeded(cellTableFieldID, rowTagValue.Table, null);
                            if (newField == null) {
                                rowTagValue.Field.TableFieldName = null;
                                rowTagValue.Field.TableFieldID = -1;
                                rowTagValue.Field.Table = null;
                                rowTagValue.Field.IsReadOnly = true;
                                // TODO: since there is no field to map to, make the field readonly?

                            } else {
                                rowTagValue.Field.TableFieldName = newField.TableFieldName;
                                rowTagValue.Field.TableFieldID = newField.TableFieldID;
                                rowTagValue.Field.Table = newField.Table;
                            }

                            // and sync the sql to our new data
                            //if (chkAutoSynchronize.Checked) {
                            //    syncGui(QueryMasterSource.Gridviews);
                            //}
                            CheckDirty();
                        } else {
                            CheckDirty();
                        }

                        break;
                }
            }
        }

        private void reportError(string message) {
            lblError.Text = message;
            lblError.Visible = true;
            Application.DoEvents();
            lblError.Refresh();
        }

        private void dgvFields_DataError(object sender, DataGridViewDataErrorEventArgs e) {
//            MessageBox.Show(this, "DataError\n Row " + e.RowIndex + ", Column " + e.ColumnIndex + " : " + e.Exception.Message);
            if (e.ColumnIndex == (int)FieldColumnOffsets.LookupDataviewName && e.RowIndex > -1 && !dgvFields.CurrentRow.IsNewRow) {
                if (_query.Selects.Count > 0 && _query.Selects[0].Fields.Count > e.RowIndex) {
                    reportError("Field '" + dgvFields.Rows[e.RowIndex].Cells[(int)FieldColumnOffsets.DataviewFieldName].Value + "' has an invalid Lookup Picker Source of '" + _query.Selects[0].Fields[e.RowIndex].ForeignKeyDataviewName + "'.");
                } else {
                    reportError("Field '" + dgvFields.Rows[e.RowIndex].Cells[(int)FieldColumnOffsets.DataviewFieldName].Value + "' has an invalid Lookup Picker Source.");
                }
            } else {
                MainFormReportError("DataError\n Row " + e.RowIndex + ", Column " + e.ColumnIndex + ", value='" + dgvFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "': " + e.Exception.Message);
            }
        }

        private string getFieldFriendlyName(string fieldName, int langID) {
            foreach (DataGridViewRow row in dgvFields.Rows) {
                if (row.Cells[(int)FieldColumnOffsets.DataviewFieldName].Value + "" == fieldName) {
                    for(var i=0;i<dgvFields.Columns.Count;i++){
                        var col = dgvFields.Columns[i];
                        if (col.Tag is string && ("" + col.Tag).Contains("|")) {
                            var split = col.Tag.ToString().Split('|');
                            var lid = Toolkit.ToInt32(split[0], -1);
                            if (lid == langID) {
                                return row.Cells[i].Value + "";
                            }
                        }
                    }
                }
            }
            return "";
        }

        private frmDataviewParameters _fparams;
        private void btnGo_Click(object sender, EventArgs e) {
//            var gen = new SqlGenerator(getRichTextBox(false).Text, AdminProxy.GetDataConnectionSpec());

            // prompt them for parameter values

            if (_fparams == null) {
                _fparams = new frmDataviewParameters();
            } else {
                _fparams.ResetActivated();
            }
            _fparams.Parameters = new List<IDataviewParameter>(_query.Parameters.ToArray());

            if (_query.Parameters.Count == 0 || DialogResult.OK == _fparams.ShowDialog(this)) {
                var limit = Toolkit.ToInt32(chkLimitRows.Checked ? txtLimitRows.Text : "0", 0);
                var dvName = MainFormCurrentNodeText("");
                using (new AutoCursor(this)) {
                    try {
                        var langID = Toolkit.ToInt32(ddlViewInLanguage.SelectedValue, -1);
                        //var ds = AdminProxy.TestDataview(getRichTextBox(false).Text, _query.Parameters, dvName, langID, 0, limit);
                        var ds = AdminProxy.TestDataview("", _query.Parameters, dvName, langID, 0, limit);
                        var dt = ds.Tables[dvName];
                        if (dt == null) {
                            MessageBox.Show(this, "SQL was issued, but no valid DataTable was returned.\nNotify your system administrator.", "Data could not be retrieved");
                        } else {

                            if (chkIsTransform.Checked && ddlFieldNameSource.SelectedIndex > -1 && ddlFieldValueSource.SelectedIndex > -1) {

                                if (_query.Selects.Count > 0) {
                                    var stmt = _query.Selects[0];
                                    var transformFields = new List<string>();
                                    for (var i = 0; i < stmt.Fields.Count; i++) {
                                        if (stmt.Fields[i].IsTransform) {
                                            transformFields.Add(stmt.Fields[i].DataviewFieldName);
                                        }
                                    }

                                    dt = dt.Transform(transformFields.ToArray(), ddlFieldNameSource.Text, ddlFieldCaptionSource.Text, ddlFieldValueSource.Text);

                                }
                            }

                            // set datasource to null so columns and rows are cleared out (in case they changed ordering of columns)
                            dgvPreview.DataSource = null;

                            // then rebind it with the new data
                            dgvPreview.DataSource = dt;


                            for (var i = 0; i < dt.Columns.Count; i++) {
                                dgvPreview.Columns[i].HeaderText = dt.Columns[i].Caption;

                                var friendlyName = getFieldFriendlyName(dt.Columns[i].ColumnName, langID);
                                if (!String.IsNullOrEmpty(friendlyName)) {
                                    dgvPreview.Columns[i].HeaderText = friendlyName;

                                    if (_query.Selects.Count > 0) {
                                        var fld = _query.Selects[0].GetField(dt.Columns[i].ColumnName);
                                        if (fld != null) {
                                            fld.DataviewFriendlyFieldNames[langID] = friendlyName;
                                            //if (fld.DataviewFriendlyFieldNames.TryGetValue(langID, out friendly)) {
                                            //    dgvPreview.Columns[i].HeaderText = friendly;
                                            //}
                                        }
                                    }
                                }

                            }
                            switch (dt.Rows.Count) {
                                case 1:
                                    lblPreviewCount.Text = "Retrieved 1 row.";
                                    break;
                                default:
                                    lblPreviewCount.Text = "Retrieved " + dt.Rows.Count + " rows.";
                                    break;
                            }
                        }
                    } catch (Exception ex) {
                        MessageBox.Show(this, "The following SQL error occurred:\n" + ex.Message, "Invalid SQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private bool _activated;
        private void frmDataview_Activated(object sender, EventArgs e) {
            if (!_activated) {

                _activated = true;

            }
        }

        private void chkIsTransform_CheckedChanged(object sender, EventArgs e) {
            if (chkIsTransform.Checked) {
                chkIsReadOnly.Checked = true;
            }
            syncGui(QueryMasterSource.None);
        }

        private void dgvFields_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
        }

        private void dgvFields_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.ColumnIndex > -1 && e.RowIndex > -1) {
                var col = (DataGridViewColumn)dgvFields.Columns[e.ColumnIndex];
                var langIDAndDirection = (string.Empty + col.Tag).Split('|');
                if (langIDAndDirection.Length > 1 && e.RowIndex >= 0) {
                    var dir = langIDAndDirection[1];
                    if (dir == "RTL") {
                        e.PaintBackground(e.CellBounds, true);
                        TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(), e.CellStyle.Font, e.CellBounds, e.CellStyle.ForeColor, TextFormatFlags.RightToLeft | TextFormatFlags.Right);
                        e.Handled = true;
                    }
                }
            }

        }

        private void dgvFields_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            if (dgvFields.CurrentCell.ColumnIndex > -1 && dgvFields.CurrentCell.RowIndex > -1) {
                if (e.Control != null) {
                    var col = dgvFields.Columns[dgvFields.CurrentCell.ColumnIndex];
                    var langIDAndDirection = (string.Empty + col.Tag).ToString().Split('|');
                    if (langIDAndDirection.Length > 1) {
                        var dir = langIDAndDirection[1];
                        if (dir == "RTL") {
                            e.Control.RightToLeft = RightToLeft.Yes;
                        }
                    }
                }
            }

        }

        private const char RLE_CHAR = (char)0x202B;// RLE embedding Unicode control character

        private void dgvFields_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e) {
            if (e.ColumnIndex > -1 && e.RowIndex > -1) {
                var col = (DataGridViewColumn)dgvFields.Columns[e.ColumnIndex];
                var langIDAndDirection = (string.Empty + col.Tag).ToString().Split('|');
                if (langIDAndDirection.Length > 1) {
                    var dir = langIDAndDirection[1];
                    if (dir == "RTL") {
                        if (e.ToolTipText != string.Empty) {
                            e.ToolTipText = RLE_CHAR + e.ToolTipText;
                        }
                    }
                }
            }
        }


        // used to suppress dialog box showing multiple times when they cancel
        //private bool _alreadyPromptedForClose;
        private void frmDataview_FormClosing(object sender, FormClosingEventArgs e) {
            //if (!_alreadyPromptedForClose) {
            //    _alreadyPromptedForClose = true;
                if (Modal) {
                    if (IsDirty()) {
                        var dr = MessageBox.Show(this, "You have changes that have not yet been saved.\nDo you want to save them now?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        switch(dr){
                            case DialogResult.No:
                                if (!Modal) {
                                    if (MainFormCurrentNode().Tag.ToString() != this.DataviewName) {
                                        MainFormSelectPreviousNode();
                                    }
                                }
                                return;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                return;
                            case DialogResult.Yes:
                                e.Cancel = true;
                                btnSave.PerformClick();
                                return;
                            default:
                                break;
                        }
                    }
                }


            if (_findAndReplace != null){
                try {
                    _findAndReplace.Close();
                } catch { }
                _findAndReplace = null;
            }

                //if (!Modal) {
                //    MainFormSelectParentTreeNode();
                //}
           //}
        }


        private Rectangle _dgvFieldsMouseDownDragBox;
        private int _dgvFieldsMouseDownRowIndex;
        private int _dgvFieldsMouseDropRowIndex;

        private class DataGridViewDragDrop {
            public List<DataGridViewRow> Rows;
            public DataGridViewDragDrop() {
                Rows = new List<DataGridViewRow>();
            }
        }

        private void dgvFields_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                // If the mouse moves outside the rectangle, start the drag.
                if (_dgvFieldsMouseDownDragBox != Rectangle.Empty 
                    && !_dgvFieldsMouseDownDragBox.Contains(e.X, e.Y)) {

                    // Proceed with the drag and drop, passing in the list item.       
                    var dnd = new DataGridViewDragDrop();
                    foreach(DataGridViewRow row in dgvFields.SelectedRows){
                        dnd.Rows.Add(row);
                    }
                    //var dropEffect = dgvFields.DoDragDrop(dgvFields.Rows[_dgvFieldsMouseDownRowIndex], DragDropEffects.Move);
                    var dropEffect = dgvFields.DoDragDrop(dnd, DragDropEffects.Move);
                }
            }
        }

        private void dgvFields_MouseDown(object sender, MouseEventArgs e) {
            // Get the index of the item the mouse is below.
            _dgvFieldsMouseDownRowIndex = dgvFields.HitTest(e.X, e.Y).RowIndex;
            if (_dgvFieldsMouseDownRowIndex != -1) {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.               
                var dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dgvFieldsMouseDownDragBox = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                                        dragSize);
            } else {
                // Reset the rectangle if the mouse is not over a row.
                _dgvFieldsMouseDownDragBox = Rectangle.Empty;
            }
        }

        private void dgvFields_DragOver(object sender, DragEventArgs e) {
            // make sure they're dragging a row, not a table or something else
            if (e.Data.GetDataPresent(typeof(DataGridViewDragDrop))){
                var dnd = e.Data.GetData(typeof(DataGridViewDragDrop)) as DataGridViewDragDrop;

                // The mouse locations are relative to the screen, so they must be
                // converted to client coordinates.
                var clientPoint = dgvFields.PointToClient(new Point(e.X, e.Y));

                // Get the row index of the item the mouse is below.
                var rowIndex = dgvFields.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (rowIndex < 0) {
                    e.Effect = DragDropEffects.None;
                } else {
                    e.Effect = DragDropEffects.Move;
                }
            } else if (e.Data.GetDataPresent(typeof(string))){
                var data = e.Data.GetData(typeof(string)) as string;
                if (data != null) {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void dgvFields_DragDrop(object sender, DragEventArgs e) {

            var dnd = e.Data.GetData(typeof(DataGridViewDragDrop)) as DataGridViewDragDrop;

            if (dnd != null) {

                // The mouse locations are relative to the screen, so they must be
                // converted to client coordinates.
                var clientPoint = dgvFields.PointToClient(new Point(e.X, e.Y));

                // Get the row index of the item the mouse is below.
                _dgvFieldsMouseDropRowIndex = dgvFields.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (_dgvFieldsMouseDropRowIndex > -1) {

                    // If the drag operation was a move then remove and insert the row(s).
                    if (e.Effect == DragDropEffects.Move) {
                        foreach (DataGridViewRow row in dnd.Rows) {
                            dgvFields.Rows.Remove(row);
                        }
                        dgvFields.Rows.InsertRange(_dgvFieldsMouseDropRowIndex, dnd.Rows.ToArray());

                        //// swap the fields in the generator's fields collection too
                        //var temp = _generator.Fields[_dgvFieldsMouseDownRowIndex];
                        //_generator.Fields[_dgvFieldsMouseDownRowIndex] = _generator.Fields[_dgvFieldsMouseDropRowIndex];
                        //_generator.Fields[_dgvFieldsMouseDropRowIndex] = temp;
                        dgvFields.CurrentCell = dgvFields.Rows[_dgvFieldsMouseDropRowIndex].Cells[0];
                        syncGui(QueryMasterSource.Gridviews);
                    }
                }
            } else {
                var data = e.Data.GetData(typeof(string)) as string;
                MessageBox.Show(data);
            }
        }

        private void dgvFields_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {
            //var needToRefresh = false;
            //Sync(true, delegate {
            //    var dgv = (DataGridView)sender;
            //    var endRow = e.RowIndex + e.RowCount;
            //    if (endRow > 0 && e.RowIndex > -1) {
            //        for (var i = e.RowIndex; i < endRow; i++) {
            //            var row = dgv.Rows[i];
            //            _generator.RemoveField(row.Cells[0].Value as string);
            //        }
            //        needToRefresh = true;
            //        dgv.Refresh();
            //    }
            //});
            //if (needToRefresh) {
            //    syncGui(false);
            //}
        }

        private void chkAutoSynchronizeGridviews_CheckedChanged(object sender, EventArgs e) {
            if (chkAutoSynchronize.Checked) {

                TextBox tb = null; // getRichTextBox(false);

                if (IsControlDirty(dgvFields) && IsControlDirty(tb)) {
                    Clipboard.SetText(tb.Text, TextDataFormat.Text);
                    MessageBox.Show(this, "You have altered both the Fields and the SQL directly.  The information in Fields will be used, meaning your SQL changes may be lost.\nYour SQL has been copied to the clipboard in case this happens.", "Conflicting Edits");
                } else {
                    if (IsControlDirty(dgvFields)) {
                        syncGui(QueryMasterSource.Gridviews);
                    } else if (IsControlDirty(tb)) {
                        syncGui(QueryMasterSource.SqlEditorPane);
                    }
                }


            }
        }

        private void tvByRelationship_ItemDrag(object sender, ItemDragEventArgs e) {
            base.startDrag(sender, (TreeNode)e.Item);

        }

        private void dgvFields_KeyPress(object sender, KeyPressEventArgs e) {
        }

        private void dgvFields_KeyUp(object sender, KeyEventArgs e) {
        }

        private void deleteSelectedFieldRows(){
            var rowsToDelete = new List<DataGridViewRow>();;
            if (dgvFields.SelectedRows.Count > 0) {
                var removedCount = 0;
                for (var i = 0; i < dgvFields.Rows.Count; i++) {
                    if (dgvFields.Rows[i].Selected) {
                        rowsToDelete.Add(dgvFields.Rows[i]);
                        for (var j = 0; j < _query.Selects.Count; j++) {
                            _query.Selects[j].Fields.RemoveAt(i-removedCount);
                        }
                        removedCount++;
                    } else {
                        var frt = dgvFields.Rows[i].Tag as FieldRowTag;
                        if (frt != null) {
                            frt.RowIndex -= rowsToDelete.Count;
                        }
                    }
                }

                foreach (var row in rowsToDelete) {
                    dgvFields.Rows.Remove(row);
                }

                syncGui(QueryMasterSource.Gridviews);
            }
        }

        private void cmiFieldsDelete_Click(object sender, EventArgs e) {
            deleteSelectedFieldRows();
        }

        private void cmiFieldsMoveUp_Click(object sender, EventArgs e) {

            if (dgvFields.SelectedRows.Count > 0) {
                var total = dgvFields.SelectedRows.Count;
                var firstRowIndex = Toolkit.Minimum(dgvFields.SelectedRows[0].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex, dgvFields.SelectedRows[dgvFields.SelectedRows.Count - 1].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                var lastRowIndex = Toolkit.Max(dgvFields.SelectedRows[0].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex, dgvFields.SelectedRows[dgvFields.SelectedRows.Count - 1].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                if (firstRowIndex > 0) {
                    var rows = new List<DataGridViewRow>();
                    var indexes = new List<int>();

                    // remember which rows to move
                    foreach (DataGridViewRow row in dgvFields.SelectedRows) {
                        rows.Add(row);
                        indexes.Add(row.Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                    }

                    // remove them
                    for (var i = 0; i < rows.Count; i++) {
                        dgvFields.Rows.Remove(rows[i]);
                    }
                    // mark all rows as unselected
                    foreach (DataGridViewRow dgvr in dgvFields.Rows) {
                        dgvr.Selected = false;
                    }


                    // reverse lists if needed so insertion code works properly
                    if (indexes[0] > indexes[indexes.Count - 1]) {
                        rows.Reverse();
                        indexes.Reverse();
                    }


                    // insert them, mark as selected
                    for (var i = 0; i < rows.Count; i++) {
                        dgvFields.Rows.Insert(indexes[i] - 1, rows[i]);
                        rows[i].Selected = true;
                    }

                    // re-highlight the row, sync gui
                    //dgvFields.CurrentCell = dgvFields.Rows[firstRowIndex - 1].Cells[0];
                    syncGui(QueryMasterSource.Gridviews);
                }
            }
        }

        private void cmFields_Opening(object sender, CancelEventArgs e) {
            if (dgvFields.SelectedRows.Count > 0) {
                var firstRowIndex = Toolkit.Minimum(dgvFields.SelectedRows[(int)FieldColumnOffsets.DataviewFieldName].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex, dgvFields.SelectedRows[dgvFields.SelectedRows.Count - 1].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                var lastRowIndex = Toolkit.Max(dgvFields.SelectedRows[(int)FieldColumnOffsets.DataviewFieldName].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex, dgvFields.SelectedRows[dgvFields.SelectedRows.Count - 1].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                cmiFieldsMoveUp.Enabled = firstRowIndex > 0;
                cmiFieldsMoveDown.Enabled = lastRowIndex < dgvFields.Rows.Count - 1;
                cmiFieldsDelete.Enabled = true;
            } else {
                cmiFieldsMoveDown.Enabled = false;
                cmiFieldsMoveUp.Enabled = false;
                cmiFieldsDelete.Enabled = false;
            }

        }

        private void cmiFieldsMoveDown_Click(object sender, EventArgs e) {
            if (dgvFields.SelectedRows.Count > 0) {
                var total = dgvFields.SelectedRows.Count;
                var firstRowIndex = Toolkit.Minimum(dgvFields.SelectedRows[0].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex, dgvFields.SelectedRows[dgvFields.SelectedRows.Count - 1].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                var lastRowIndex = Toolkit.Max(dgvFields.SelectedRows[0].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex, dgvFields.SelectedRows[dgvFields.SelectedRows.Count - 1].Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                if (lastRowIndex < dgvFields.Rows.Count - 1) {
                    var rows = new List<DataGridViewRow>();
                    var indexes = new List<int>();

                    // remember which rows to move
                    foreach (DataGridViewRow row in dgvFields.SelectedRows) {
                        rows.Add(row);
                        indexes.Add(row.Cells[(int)FieldColumnOffsets.DataviewFieldName].RowIndex);
                    }
                    // remove them
                    for (var i = 0; i < rows.Count; i++) {
                        dgvFields.Rows.Remove(rows[i]);
                    }

                    // mark all rows as unselected
                    foreach (DataGridViewRow dgvr in dgvFields.Rows) {
                        dgvr.Selected = false;
                    }

                    // reverse lists if needed so insertion code works properly
                    if (indexes[0] > indexes[indexes.Count - 1]) {
                        rows.Reverse();
                        indexes.Reverse();
                    }

                    // insert them, mark as selected
                    for (var i = 0; i < rows.Count; i++) {
                        dgvFields.Rows.Insert(indexes[i]+1, rows[i]);
                        rows[i].Selected = true;
                    }

                    // re-highlight the row, sync gui
                    //dgvFields.CurrentCell = dgvFields.Rows[firstRowIndex + 1].Cells[0];
                    syncGui(QueryMasterSource.Gridviews);
                }
            }

            //if (_dgvFieldsMouseDownRowIndex < dgvFields.Rows.Count - 1) {
            //    var rowToMove = dgvFields.Rows[_dgvFieldsMouseDownRowIndex];
            //    dgvFields.Rows.RemoveAt(_dgvFieldsMouseDownRowIndex);
            //    dgvFields.Rows.Insert(_dgvFieldsMouseDropRowIndex + 1, rowToMove);
            //}

        }

        private void dgvFields_KeyDown(object sender, KeyEventArgs e) {
            if (dgvFields.EditingControl == null) {
                if (e.KeyCode == Keys.Delete) {
                    e.Handled = true;
                    deleteSelectedFieldRows();
                }
            }
        }

        private void txtDataviewName_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void chkIssys_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void chkIsReadOnly_CheckedChanged(object sender, EventArgs e) {
            if (!chkIsReadOnly.Checked && chkIsTransform.Checked) {
                MessageBox.Show(this, "If a Transform is applied to a dataview, it must be marked read only.", "Dataview Forced to Read Only");
                chkIsReadOnly.Checked = true;
                return;
            }
            chkIsReadOnlyOnInsert.Enabled = !chkIsReadOnly.Checked;
            CheckDirty();

        }

        private void chkIsReadOnlyOnInsert_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void chkIsVisible_CheckedChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlFieldNameSource_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlFieldValueSource_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void ddlFieldCaptionSource_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cboCategoryName_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void txtCategoryOrder_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void dgvTitles_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            CheckDirty();

        }

        private void frmDataview_DragOver(object sender, DragEventArgs e) {
        }

        private void dgvFields_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
        }

        private void dgvFields_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
            if (e.RowIndex > -1 && e.ColumnIndex == (int)FieldColumnOffsets.ReadOnly) {
                var cellValue = dgvFields.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                var chk = Toolkit.ToBoolean(cellValue, false);

                if (chk) {
                    if (chkIsReadOnly.Checked) {
                        if (chkIsTransform.Checked) {
                            MessageBox.Show(this, "A transform is applied to this dataview, so Read Only is required.\nTo make it writable, you must remove the transform.");
                            e.Cancel = true;
                        } else {
                            if (DialogResult.Yes == MessageBox.Show(this, "This entire dataview is marked as read only.\nDo you want to mark it as writable so this field can be made writable?", "Make Dataview Writable?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                                chkIsReadOnly.Checked = false;
                            } else {
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
        }

        private void chkLimitRows_CheckedChanged(object sender, EventArgs e) {
            txtLimitRows.Enabled = chkLimitRows.Checked;
        }

        private void lnkHelpNaming_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            new frmHelpDataviewCategory().ShowDialog(this);
        }

        private void txtDataviewName_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                System.Console.Beep();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void cboCategoryName_SelectedIndexChanged_1(object sender, EventArgs e) {
            CheckDirty();
        }

        private void cboDatabaseArea_SelectedIndexChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cboDatabaseArea_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void cboCategoryName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void sqlQuery_DragLeave(object sender, EventArgs e) {
            _ddo = null;
            updateDropAsText(_ddo, 0, 0, (TextBox)sender);
        }

        private void sqlQuery_MouseMove(object sender, MouseEventArgs e) {
            //Debug.WriteLine("mouse moved, point=" + e.X + ", " + e.Y);
            //updateDropAsText(_ddo, e.X, e.Y, (TextBox)sender);
        }

        private void updateDropAsText(DragDropObject ddo, int x, int y, TextBox tb){

            if (ddo == null) {
                lblDropAs.Text = "";
            } else {
                var pt = tb.PointToClient(new Point { X = x, Y = y });
                pt.X = (pt.X < 0 ? 0 : pt.X);
                pt.Y = (pt.Y < 0 ? 0 : pt.Y);
                var charIndex = tb.GetCharIndexFromPosition(pt);

                Debug.WriteLine("char = " + charIndex + ", mouse = " + x + ", " + y + ", point to client = " + pt.X + ", " + pt.Y);


                var joinText = "";
                var unionText = "";

                if (_ddo.SourceNode.Name.StartsWith("Table|") || ddo.SourceNode.Name.StartsWith("Field|") || ddo.SourceNode.Name.StartsWith("Folder|")) {
                    var sn = ddo.SourceNode.Name.Split('|');
                    if (sn[0] == "Table" || sn[0] == "Folder") {
                        joinText = "Join in all fields from '" + sn[1] + "'";
                        unionText = "Create a new UNION SELECT clause for '" + sn[1] + "'";
                    } else {
                        joinText = "Join in field '" + sn[1] + "'";
                        unionText = "Create a new UNION SELECT clause for field '" + sn[1] + "'";
                    }

                }

                lblDropAs.Text = "";
                foreach (var sel in _query.Selects) {
                    if (sel.EndsAtCharIndex > charIndex) {
                        lblDropAs.Text = joinText;
                        break;
                    }
                }

                if (lblDropAs.Text == "") {
                    if (_query.Selects.Count == 0) {
                        unionText = unionText.Replace("UNION SELECT", "SELECT");
                    }
                    lblDropAs.Text = unionText;
                }
            }
        }

        private void sqlQuery_DragOver(object sender, DragEventArgs e) {
            Debug.WriteLine("DragOver, mouse = " + e.X + ", " + e.Y);
            var ddo = getDragDropObject(e);
            if (ddo != null) {
                e.Effect = DragDropEffects.Copy;
                updateDropAsText(ddo, e.X, e.Y, (TextBox)sender);
            }

        }

        private void txtConfigurationOptions_TextChanged(object sender, EventArgs e) {
            CheckDirty();
        }

        private void dgvFields_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
            // 2010-08-10 Brock Weaver brock@circaware.com
            // this wasn't working for calculated fields (such as results from a subquery) so I'm taking out this restriction for now.

            //if (!_refreshing){
            //    if (e.RowIndex > -1 && e.ColumnIndex == ((int)FieldColumnOffsets.UserInterface))
            //    {
            //        var value = e.FormattedValue + string.Empty;

            //        if (value.ToLower() == "large_single_select_control" || value.ToLower() == "lookup picker")
            //        {
            //            var fld = ((FieldRowTag)(dgvFields.Rows[e.RowIndex].Tag)).Field as IField;
            //            if (fld != null)
            //            {
            //                if (fld.Table != null)
            //                {
            //                    if (String.IsNullOrEmpty(fld.ForeignKeyTableName) || String.IsNullOrEmpty(fld.ForeignKeyTableFieldName))
            //                    {
            //                        MessageBox.Show(this, "Field '" + fld.DataviewFieldName + "' is not mapped as being a foreign key.\nYou must either choose a different User Interface or map the '" + fld.TableName + "." + fld.TableFieldName + "' field to a foreign key field using the Table Mappings editor.", "Not a Foreign Key");
            //                        e.Cancel = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

        }

        private void pnlDrop_DragDrop(object sender, DragEventArgs e) {
            var ddo = getDragDropObject(e);
            if (ddo != null) {
                try {
                    //var charIndex = rtb.GetCharIndexFromPosition(rtb.PointToClient(new Point { X = e.X, Y = e.Y }));
                    if (ddo.SourceNode.Name.StartsWith("Table|")) {
                        lblDropAs.Text = "";

                        //var tst = new test();
                        //tst.MdiParent = this;
                        //tst.Show();

                        var t = new TableControl();
                        t.Table = (ITable)ddo.SourceNode.Tag;
                        var pt = pnlDrop.PointToClient(new Point { X = e.X, Y = e.Y });
                        t.Left = pt.X;
                        t.Top = pt.Y;
                        pnlDrop.Controls.Add(t);

                        //rtb.Text = _query.AddTable(charIndex, (ITable)ddo.SourceNode.Tag, true);
                        //rtb.SelectionStart = charIndex;
                        //rtb.SelectionLength = 0;
                        //rtb.ScrollToCaret();
                        //rtb.Focus();
                        //syncGui(QueryMasterSource.QueryObject);
                    //} else if (ddo.SourceNode.Name.StartsWith("Folder|")) {
                    //    lblDropAs.Text = "";
                    //    rtb.Text = _query.AddTable(charIndex, (ITable)ddo.SourceNode.Tag, true);
                    //    rtb.SelectionStart = charIndex;
                    //    rtb.SelectionLength = 0;
                    //    rtb.ScrollToCaret();
                    //    rtb.Focus();
                    //    syncGui(QueryMasterSource.QueryObject);
                    //} else if (ddo.SourceNode.Name.StartsWith("Field|")) {
                    //    lblDropAs.Text = "";
                    //    rtb.Text = _query.AddField(charIndex, (IField)ddo.SourceNode.Tag, true);
                    //    rtb.SelectionStart = charIndex;
                    //    rtb.SelectionLength = 0;
                    //    rtb.ScrollToCaret();
                    //    rtb.Focus();
                    //    syncGui(QueryMasterSource.QueryObject);
                    }
                } catch (NotImplementedException nie) {
                    MessageBox.Show(this, nie.Message, "No Relationship Defined", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            _ddo = null;
        }

        private void pnlDrop_DragEnter(object sender, DragEventArgs e) {
            var tb = (Panel)sender;
            var ddo = getDragDropObject(e);
            if (ddo != null) {
                _ddo = ddo;
                if (ddo.SourceNode.Name.StartsWith("Table|") || ddo.SourceNode.Name.StartsWith("Field|") || ddo.SourceNode.Name.StartsWith("Folder|")) {
                    e.Effect = DragDropEffects.Copy;
                    Debug.WriteLine("drag enter, mouse = " + e.X + ", " + e.Y);
                    // updateDropAsText(ddo, e.X, e.Y, tb);
                }
            }
        }

        private void pnlDrop_DragOver(object sender, DragEventArgs e) {
            Debug.WriteLine("DragOver, mouse = " + e.X + ", " + e.Y);
            var ddo = getDragDropObject(e);
            if (ddo != null) {
                e.Effect = DragDropEffects.Copy;
                //updateDropAsText(ddo, e.X, e.Y, (TextBox)sender);
            }

        }
    }
}
