using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.IO;
using GrinGlobal.InstallHelper;
using System.Web.Services.Protocols;
using System.Threading;

namespace GrinGlobal.Import {
    public partial class frmImport : Form {
        public frmImport() {
            InitializeComponent();
        }

        private void tc_SelectedIndexChanged(object sender, EventArgs e) {
            //switch (tc.SelectedTab.Name) {
            //    case "tpDataType":
            //        lblProgress.Text = "Step 1 of 3: Choose the type of data to import";
            //        break;
            //    case "tpSource":
            //        lblProgress.Text = "Step 2 of 3: Specify data to import";
            //        if (dgvSource.Columns.Count == 0) {
            //            bindGridView();
            //        }
            //        break;
            //    case "tpPreview":
            //        lblProgress.Text = "Step 3 of 4: Preview import actions";
            //        break;
            //    case "tpResults":
            //        lblProgress.Text = "Step 3 of 3: Results of import";
            //        break;
            //}

            lblDataType.Font = _origFont;
            lblSource.Font = _origFont;
            lblResults.Font = _origFont;

            switch (tc.SelectedIndex) {
                case 0:
                    lblDataType.Font = _boldFont;
                    updateStatusInfo();
                    break;
                case 1:
                    lblSource.Font = _boldFont;
                    updateStatusInfo();
                    break;
                case 2:
                    lblResults.Font = _boldFont;
                    updateStatusInfo();
                    break;
            }
            _tabValidated = false;
            pb.Value = (tc.SelectedIndex + 1) * 33 + 1;
        }

        private void moveToNextTab(){
            _tabValidated = true;
            //switch (tc.SelectedTab.Name) {
            //    case "tpDataType":
            //        if (!tc.TabPages.Contains(tpSource)) {
            //            tc.TabPages.Add(tpSource);
            //        }
            //        break;
            //    case "tpSource":
            //        if (!tc.TabPages.Contains(tpPreview)) {
            //            tc.TabPages.Add(tpPreview);
            //        }
            //        break;
            //    case "tpPreview":
            //        if (!tc.TabPages.Contains(tpResults)) {
            //            tc.TabPages.Add(tpResults);
            //        }
            //        break;
            //}

            if (tc.SelectedIndex < tc.TabPages.Count - 1) {
                tc.SelectedIndex++;
            }
        }


        private void moveToPreviousTab() {
            if (tc.SelectedIndex > 0){
                _tabValidated = true;
                tc.SelectedIndex--;
            }
        }

        private void btnNextSource_Click(object sender, EventArgs e) {
            HostProxy.ClearCache();
            updateJumpErrorLink();
            bindGridView(null);
            moveToNextTab();
        }

        private ListViewItem generateListViewItem(DataRow dr, List<string> columns, int rowIndex, ref int errors) {

            var result = dr["SavedStatus"].ToString();

            var lvi = new ListViewItem();
            lvi.UseItemStyleForSubItems = true;
            lvi.BackColor = (rowIndex % 2 == 0 ? Color.White : Color.SeaShell);
            if (result == "Failure") {
                lvi.ForeColor = Color.Red;
                lvi.Text = result + " - " + dr["ExceptionMessage"].ToString();
                errors++;
            } else {
                lvi.ForeColor = Color.Black;
                lvi.Text = result;
            }
            lvi.SubItems.Add(rowIndex.ToString());
            lvi.SubItems.Add(dr["TableName"].ToString());
            // lvi.SubItems.Add(dr["NewPrimaryKeyID"].ToString());

            for (var i = 0; i < columns.Count; i++) {
                if (dr.Table.Columns[i].ExtendedProperties["table_name"].ToString() == dr["TableName"].ToString()) {
                    lvi.SubItems.Add(dr[i].ToString());
                } else {
                    lvi.SubItems.Add("");
                }
            }
            return lvi;
        }

        private DataGridViewRow generateGridViewRow(DataRow dr, List<string> columns, int rowIndex, ref int errors) {

            var result = dr["SavedStatus"].ToString();

            var row = new DataGridViewRow();
            row.DefaultCellStyle.BackColor = (rowIndex % 2 == 0 ? Color.White : Color.SeaShell);

            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells.Add(new DataGridViewTextBoxCell());
            row.Cells.Add(new DataGridViewTextBoxCell());

            if (result == "Failure") {
                row.DefaultCellStyle.ForeColor = Color.Red;
                row.Cells[0].Value = result + " - " + dr["ExceptionMessage"].ToString();
                errors++;
            } else {
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.Cells[0].Value = result;
            }
            row.Cells[1].Value = rowIndex.ToString();
            row.Cells[2].Value = dr["TableName"].ToString();

            for (var i = 0; i < columns.Count; i++) {
                row.Cells.Add(new DataGridViewTextBoxCell());
                if (dr.Table.Columns[i].ExtendedProperties["table_name"].ToString() == dr["TableName"].ToString()) {
                    row.Cells[2 + i].Value = dr[i].ToString();
                } else {
                    row.Cells[2 + i].Value = "";
                }
            }
            return row;
        }

        private void generateResultRow(DataRow dr, List<string> columns, int rowIndex, ref int errors) {

            var result = dr["SavedStatus"].ToString();

            if (result == "Failure") {
                dr["Results"] = result + " - " + dr["ExceptionMessage"].ToString();
                dr.RowError = dr["ExceptionMessage"].ToString();
                errors++;
            } else {
                dr["Results"] = result + " - " + dr["SavedAction"].ToString();
            }
            dr["Row Index"] = rowIndex;

            var tblName = dr["TableName"].ToString();
            var aliasName = dr["AliasName"].ToString();

            dr["Table (Alias)"] = tblName + (String.IsNullOrEmpty(aliasName) ? "" : " (" + aliasName + ")");


            DataColumn dc = null;
            for (var i = 0; i < columns.Count; i++) {
                dc = dr.Table.Columns[i];
                var extTblName = dc.ExtendedProperties["table_name"].ToString();
                var extAliasName = dc.ExtendedProperties["table_alias_name"].ToString();
                if (tblName != extTblName || aliasName != extAliasName){
                    dr[i] = DBNull.Value;
                }
            }

            dr.AcceptChanges();
        }

        private void btnNextResults_Click(object sender, EventArgs e) {
            moveToNextTab();

        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnPreviousDataType_Click(object sender, EventArgs e) {
            moveToPreviousTab();
        }

        private void btnLoadFromCSV_Click(object sender, EventArgs e) {
            var dr = DialogResult.Cancel;
            try {
                ofdSource.Title = getDisplayMember("loadFromFile{open}", "Open source file for {0}", lblDataviewTitle.Text);
                dr = ofdSource.ShowDialog(this);
            } catch (Exception ex) {
                MessageBox.Show(this, getDisplayMember("loadFromFile{failed_body}", "Error opening file: {0}", ex.Message), 
                    getDisplayMember("loadFromFile{failed_title}", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dr == DialogResult.OK) {
                using (var splash = new frmSplash(getDisplayMember("loadFromFile{start}", "Parsing file..."), false, this, true)) {
                    splash.DisableOwner = true;
                    try {
                        splash.ChangeText(getDisplayMember("loadFromFile{start}", "Parsing file..."));
                        IEnumerable<List<string>> rows = null;
                        if (ofdSource.FileName.ToLower().EndsWith(".csv")) {
                            rows = Toolkit.ParseCSV(ofdSource.FileName, true);
                        } else {

                            var delim = '\t';
                            var truncated = new StringBuilder();
                            using (var sr = new StreamReader(File.OpenRead(ofdSource.FileName))) {
                                var line1 = sr.ReadLine();
                                var tabCount = line1.Split('\t').Length;
                                var pipeCount = line1.Split('|').Length;
                                if (pipeCount > 0) {
                                    if (pipeCount > tabCount) {
                                        delim = '|';
                                    }
                                }
                                truncated.AppendLine(line1);
                                try {
                                    for (var i = 0; i < 50; i++) {
                                        truncated.AppendLine(sr.ReadLine());
                                    }
                                } catch {
                                    // eat any errors when trying to auto-parse
                                }
                            }

                            // show dialog to let them pick it, default to the given delimiter
                            var fc = new frmChooseDelimiter();
                            fc.Init(delim, truncated.ToString());
                            if (DialogResult.OK == fc.ShowDialog(this)) {
                                rows = Toolkit.ParseDelimited(ofdSource.FileName, true, fc.ColumnDelimiter);
                            } else {
                                return;
                            }


                        //} else {
                        //    throw new InvalidOperationException("Files of type '" + Path.GetExtension(ofdSource.FileName) + "' are not supported.");
                        }

                        _validationErrorCount = dgvSource.ImportData(rows);

                        splash.ChangeText(getDisplayMember("loadFromFile{populating}", "Populating user interface..."));

                        updateStatusInfo();
                        syncImportButton();
                        jumpToNextError(true);

                        splash.ChangeText(getDisplayMember("loadFromFile{cleaningup}", "Cleaning up..."));
                        GC.Collect();
                    } catch (Exception ex) {
                        splash.Close();
                        MessageBox.Show(this, getDisplayMember("loadFromFile{failed2_body}", "Error opening file: {0}", ex.Message),
                            getDisplayMember("loadFromFile{failed2_title}", "Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private Font _origFont;
        private Font _boldFont;
        internal HostProxy HostProxy;
        private void frmImport_Load(object sender, EventArgs e) {
            initGui();
        }

        private void initGui() {

            HostProxy.ClearCache();
            pb.Top = tc.Top;
            pb.Left = tc.Left;

            lblDataviewTitle.Text = " -- Select One -- ";

            using (var splash = new frmSplash(getDisplayMember("initGui{init_body}", "Initializing Import Wizard, please be patient..."), false, this, true)) {
                splash.ChangeTitle(getDisplayMember("initGui{init_title}", "Initializing Import Wizard..."));
                _origFont = lblDataType.Font;
                _boldFont = new Font(_origFont, FontStyle.Bold);
                lblDataType.Font = _boldFont;

                // for now, get rid of preview
                tc.TabPages.Remove(tpPreview);

                lnkWhyNoDragDrop.Visible = false;
                this.Text = Toolkit.GetApplicationVersion("GRIN-Global Import Wizard");
                if (!Toolkit.IsWindowsXP && Toolkit.IsProcessElevated()) {
                    this.Text += " - [Administrator]";
                    lnkWhyNoDragDrop.Visible = true;
                }

                this.Synchronize(true, delegate() {

                    splash.ChangeText(getDisplayMember("initGui{start}", "Getting list of import dataviews..."));
                    ddlDataType.ValueMember = "value_member";
                    ddlDataType.DisplayMember = "display_member";
                    var dt = HostProxy.GetData("importlist_dataview", ":langid=" + HostProxy.LanguageID, HostProxy.LanguageID).Tables["importlist_dataview"];
                    var row = dt.NewRow();
                    row["value_member"] = "";
                    row["display_member"] = "-- Select One --";
                    dt.Rows.InsertAt(row, 0);
                    ddlDataType.DataSource = dt;

                    lblServerName.Text = HostProxy.Connection.ServerName;


                    splash.ChangeText(getDisplayMember("initGui{listlanguages}", "Getting list of languages..."));
                    var dtLang = HostProxy.GetData("get_sys_language", "", HostProxy.LanguageID).Tables["get_sys_language"];
                    ddlLanguage.ValueMember = "sys_lang_id";
                    ddlLanguage.DisplayMember = "title";
                    ddlLanguage.DataSource = dtLang;

                    for (var i = 0; i < dtLang.Rows.Count; i++) {
                        DataRow dr = dtLang.Rows[i];
                        if (Toolkit.ToInt32(dr["sys_lang_id"], -1) == HostProxy.LanguageID) {
                            ddlLanguage.SelectedIndex = i;
                        }
                    }

                    if (HostProxy.IsAdministrator) {
                        splash.ChangeText(getDisplayMember("initGui{listowners}", "Getting list of available owners..."));
                        var dtUsers = HostProxy.GetData("get_sys_user", "", HostProxy.LanguageID).Tables["get_sys_user"];
                        var dtEnabledUsers = dtUsers.Clone();
                        var en = dtUsers.Select("is_enabled = 'Y'");
                        foreach (DataRow drUser in en) {
                            dtEnabledUsers.Rows.Add(drUser.ItemArray);
                        }

                        ddlOwnedBy.ValueMember = "cooperator_id";
                        ddlOwnedBy.DisplayMember = "user_name";
                        ddlOwnedBy.DataSource = dtEnabledUsers;
                    } else {
                        lblOwnedBy.Visible = false;
                        ddlOwnedBy.Visible = false;
                    }

                });
            }
        }

        private void ddlDataType_SelectedIndexChanged(object sender, EventArgs e) {
            if (ddlDataType.SelectedIndex > 0) {
                bindGridView(null);
                btnNext.Enabled = true;
            } else {
                btnNext.Enabled = false;
            }
        }

        private void bindGridView(DataTable dtData){
            this.Synchronize(true, delegate() {

                using (new FreezeWindow(this.Handle)) {

                    // get dataview name from form
                    var dtDataType = ddlDataType.DataSource as DataTable;
                    var dvName = dtDataType.Rows[ddlDataType.SelectedIndex]["value_member"].ToString();
                    if (String.IsNullOrEmpty(dvName)) {
                        lblDataviewTitle.Text = "-";
                        return;
                    }

                    try {
                        lblDataviewTitle.Text = dtDataType.Rows[ddlDataType.SelectedIndex]["title"].ToString();
                    } catch {
                        // safe fallback if importlist_dataview doesn't have title column...
                        lblDataviewTitle.Text = dtDataType.Rows[ddlDataType.SelectedIndex]["display_member"].ToString();
                    }

                    // lookup parameters for that dataview
                    //var dtParams = HostProxy.GetData("get_dataview_parameters", ":dataview=" + dvName).Tables["get_dataview_parameters"];
                    //var sb = new StringBuilder();
                    //foreach (DataRow dr in dtParams.Rows) {
                    //    sb.Append(dr["param_name"].ToString())
                    //        .Append("=;");
                    //}
                    //var prms = sb.ToString();

                    var emptyPrms = ":createddate=;:displaymember=;:modifieddate=;:startpkey=;:stoppkey=;:valuemember=;";

                    var altLanguageID = Toolkit.ToInt32(ddlLanguage.GetValue(HostProxy.LanguageID), HostProxy.LanguageID);
                    var prms = emptyPrms + ":langid=" + altLanguageID + ";:viewallrows=" + (chkViewAllExistingData.Checked ? "1" : "0");

                    // pull an empty set of data from that dataview
                    if (dtData == null) {
                        dtData = HostProxy.GetData(dvName, prms, altLanguageID).Tables[dvName];
                    }


                    //// just so we can match up DataGridViewRow and DataRow objects nicely, we add a fake pk column...
                    //var dcPK = new DataColumn("__fakepk__", typeof(int));
                    //dcPK.ExtendedProperties["is_visible"] = "N";  // flag as invisible so it's not displayed in the gridview by BindEx()
                    //dtData.Columns.Add(dcPK); 
                    //dtData.PrimaryKey = new DataColumn[] { dcPK };
                    //for(var i=0;i<dtData.Rows.Count;i++){
                    //    var dr = dtData.Rows[i];
                    //    dr["__fakepk__"] = i;
                    //}
                    //dtData.AcceptChanges();  // mark all rows as being in sync after we changed the fake pk field



                    // if a table exists with only a single column, make it 

                    // adjust for right-to-left languages
                    dgvSource.RightToLeft = (dtData.ExtendedProperties["script_direction"].ToString().ToUpper() == "RTL" ? RightToLeft.Yes : RightToLeft.No);


                    //// tack on a fake primary key field that the import wizard uses to sync up DataTable and DataGridView
                    //// without causing a major memory hit...
                    //var dcImportID = new DataColumn("__importid__", typeof(int));
                    //dcImportID.ExtendedProperties["is_visible"] = "N";
                    //dcImportID.ExtendedProperties["gui_hint"] = "INTEGER_CONTROL";
                    //dcImportID.ExtendedProperties["description"] = "Auto-appended field used by Import Wizard only";
                    //dcImportID.ExtendedProperties["is_readonly"] = "N";
                    //dcImportID.ExtendedProperties["is_nullable"] = "N";

                    //dtData.Columns.Add(dcImportID);
                    //for(var i=0;i<dtData.Rows.Count;i++){
                    //    dtData.Rows[i]["__importid__"] = i;
                    //}
                    //dtData.AcceptChanges();
                    //dtData.PrimaryKey = new DataColumn[] { dcImportID };
                    //dtData.TableNewRow += new DataTableNewRowEventHandler(dtData_TableNewRow);



                    try {
                        // pull list of group_names from code_value table (for filling comboboxes as needed)
                        //var dtCodeGroup = HostProxy.GetData("group_name_lookup", "").Tables["group_name_lookup"];
                        dgvSource.BindEx(dtData, true, true, true,
                            delegate(DataColumn dc, string groupName) {
                                if (String.IsNullOrEmpty(groupName)) {
                                    throw new InvalidOperationException(getDisplayMember("bindGridView{badgroup}", "{0}.{1} has its user interface defined as Drop Down but no Drop Down Source is specified for that column.", dvName, dc.ColumnName));
                                }
                                var rv = HostProxy.GetData("importlist_code_value", ":langid=" + altLanguageID + ";:groupname=" + groupName, altLanguageID).Tables["importlist_code_value"];

                                // HACK: auto-append ** for germplasm_form group...
                                if (!String.IsNullOrEmpty(groupName) && groupName.ToLower() == "germplasm_form") {
                                    var dr = rv.NewRow();
                                    dr["value_member"] = "**";
                                    dr["display_member"] = "(System)";
                                    rv.Rows.Add(dr);
                                }

                                return rv;
                            },
                            delegate(DataColumn dc, string dataviewName) {
                                if (String.IsNullOrEmpty(dataviewName)) {
                                    throw new InvalidOperationException(getDisplayMember("bindGridView{badlookup}", "{0}.{1} has its user interface is defined as Lookup Picker but no Lookup Picker Source is specified for that column.", dvName, dc.ColumnName));
                                }
                                var rv = HostProxy.GetData(dataviewName, emptyPrms + ":langid=" + altLanguageID, altLanguageID).Tables[dataviewName];
                                return rv;
                            }
                        );

                        ColorizeGridView(dgvSource);

                        _validationErrorCount = 0;
                        lblImportErrors.Text = "";
                        lblImportResults.Text = "";
                        syncImportButton();
                        updateJumpErrorLink();
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }

                }
            });

        }

        void dtData_TableNewRow(object sender, DataTableNewRowEventArgs e) {
            // e.Row["__importid__"] = e.Row.Table.Rows.Count;
        }

        public static void ColorizeGridView(DataGridView dgv){
            dgv.EnableHeadersVisualStyles = false;
            //var colors = new List<Color> { Color.Beige, Color.AliceBlue, Color.SeaShell, Color.Honeydew, Color.SkyBlue, Color.Ivory, Color.PeachPuff, Color.LightYellow };
            var colors = new List<Color> { Color.LightSteelBlue, Color.Gold, Color.SkyBlue, Color.LightCoral, Color.CornflowerBlue, Color.GreenYellow, Color.Khaki, Color.Lavender };
            var curColor = -1;
            var color = Color.Transparent;
            var dt = dgv.DataSource as DataTable;
            var italicFont = new Font(dgv.Font, FontStyle.Italic);

            var tableColors = new Dictionary<string, Color>();



            for (var i = 0; i < dgv.Columns.Count; i++) {
                var dgvc = dgv.Columns[i];
                var tableAndAliasName = (dt.Columns[i].ExtendedProperties["table_name"] + ":::" + dt.Columns[i].ExtendedProperties["table_alias_name"]);
                if (!tableColors.TryGetValue(tableAndAliasName, out color)) {
                    color = colors[(++curColor) % colors.Count];
                    tableColors[tableAndAliasName] = color;
                }

                if (dt.Columns[i].ExtendedProperties["is_nullable"].ToString().ToUpper() != "Y") {
                    dgvc.HeaderCell.Style.Font = italicFont;
                    dgvc.HeaderText += " *";
                //} else {
                //    dgvc.HeaderCell.Style.Font = _origFont;
                }
                dgvc.HeaderCell.Style.BackColor = color;
            }
        }

        private void cancel() {
            this.Close();
        }


        private void btnCancel4_Click(object sender, EventArgs e) {



            cancel();
        }

        private void btnCancel3_Click(object sender, EventArgs e) {
            cancel();
        }

        private void btnCancel2_Click(object sender, EventArgs e) {
            cancel();
        }

        private void btnCancel1_Click(object sender, EventArgs e) {

            //if ((Control.ModifierKeys & Keys.Control) == Keys.Control) {
            //    HostProxy.RebuildSearchEngineIndexes(new List<string> { "accession" });
            //}
            cancel();
        }

        private void dgvSource_DragDrop(object sender, DragEventArgs e) {

            try {
                using (var splash = new frmSplash(getDisplayMember("sourceDragDrop{copying}", "Copying, please be patient..."), false, this, true)) {
                    string data = null;
                    if (e.Data.GetDataPresent(typeof(string))) {
                        data = e.Data.GetData(typeof(string)) as string;
                    }
                    if (data != null) {
                        splash.ChangeText(getDisplayMember("sourceDragDrop{populating}", "Populating view..."));
                        _validationErrorCount = dgvSource.ImportData(Toolkit.ParseTabDelimited(data, true, Encoding.UTF8));
                        syncImportButton();
                        updateStatusInfo();
                        jumpToNextError(true);
                    } else {
                        MessageBox.Show(this, getDisplayMember("sourceDragDrop{nodata_body}", "No valid data could be located."), 
                            getDisplayMember("sourceDragDrop{nodata_title}", "Invalid Data"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                //frmSplash splash = new frmSplash("Populating view, please be patient...", false, this);
                //this.ProcessInBackground(wrk => {
                //    _validationErrorCount = dgvSource.EndDropIfNeeded(e, Encoding.UTF8);
                //},
                //(wrk, progress) => {
                //    // nada
                //}, 
                //(wrk, result) => {
                //    splash.Close();
                //    updateStatusInfo();
                //    jumpToNextError(true);

                //});

            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, getDisplayMember("sourceDragDrop{failed_title}", "Error In Data"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSource_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {

            dgvSource.StartDragIfNeeded(e, true, Encoding.UTF8);

        }

        private void dgvSource_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(string))) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnPreviousSource_Click(object sender, EventArgs e) {
            moveToPreviousTab();
        }

        private void frmImport_FormClosing(object sender, FormClosingEventArgs e) {
            if (dgvSource.Rows.Count > 1 && tc.SelectedTab != tpResults) {
                if (DialogResult.Yes != MessageBox.Show(this, getDisplayMember("formclosing{start_body}", "You have unsaved changes.\nDo you want to ignore them and exit anyway?"), 
                    getDisplayMember("formclosing{start_title}", "Ignore Changes and Exit?"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) {
                    DialogResult = DialogResult.None;
                    e.Cancel = true;
                    return;
                }
            }

            //if (_outdatedSearchEngineIndexes.Count > 0) {
            //    var res = MessageBox.Show(this, getDisplayMember("formclosing{seoutdated_body}", "Some of your search engine indexes may now be outdated.\nWould you like to rebuild the outdated ones now?"),
            //        getDisplayMember("formclosing{seoutdated_title}", "Rebuild Search Engine Indexes?"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //    if (res == DialogResult.Yes) {

            //        using (var splash = new frmSplash("Starting search index rebuilds...", false, this, true)) {
            //            HostProxy.RebuildSearchEngineIndexes(_outdatedSearchEngineIndexes);
            //            DialogResult = DialogResult.OK;
            //        }

            //    } else if (res == DialogResult.Cancel) {

            //        DialogResult = DialogResult.None;
            //        e.Cancel = true;
            //        return;

            //    }
            //}

            //e.Cancel = false;
        }

        private void btnImportMore_Click(object sender, EventArgs e) {
            this.Synchronize(false, delegate() {
                var dt = dgvSource.DataSource as DataTable;
                if (dt != null) {
                    dt.Rows.Clear();
                    GC.Collect();
                }
                //ddlDataType.SelectedIndex = 0;
                _tabValidated = true;
                tc.SelectedIndex = 0;
            });
        }

        private void dgvSource_KeyDown(object sender, KeyEventArgs e) {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control) {
                if (e.KeyCode == Keys.V) {

                    using (var splash = new frmSplash(getDisplayMember("sourceKeyDown{start}", "Copying from clipboard..."), false, this, true)) {
                        var text = Clipboard.GetText();
                        splash.ChangeText(getDisplayMember("sourceKeyDown{populating}", "Populating view..."));
                        if (!String.IsNullOrEmpty(text)) {
                            handlePastedData(text);
                        }
                        syncImportButton();
                    }
                    e.Handled = true;


                    //frmSplash splash = splash = new frmSplash("Copying from clipboard...", false, this);
                    //e.Handled = true;
                    //this.ProcessInBackground(wrk => {
                    //    var text = Clipboard.GetText();
                    //    wrk.ReportProgress(10, "Populating view...");
                    //    if (!String.IsNullOrEmpty(text)) {
                    //        handlePastedData(text);
                    //    }
                    //},
                    //(wrk, progress) => {
                    //    splash.ChangeText(progress.UserState as string);
                    //},
                    //(wrk, result) => {
                    //    splash.Close();
                    //});
                } else if (e.KeyCode == Keys.C) {

                    using (var splash = new frmSplash(getDisplayMember("sourceKeyDown{copying}", "Copying to clipboard, please be patient..."), false, this, true)) {
                        var s = dgvSource.ExportData(true, UTF8Encoding.UTF8);
                        Clipboard.SetText(s);
                    }

                    e.Handled = true;
                } 
            }
        }

        private string exportGridviewData(DataGridView dgv, bool controlClickUsesActualNames, Encoding encoding) {
            using (new AutoCursor(dgv.FindForm())) {
                // grab header text (or actual column names if they hit shift)
                var useActualNames = controlClickUsesActualNames && (System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control;
                var names = new List<string>();
                foreach (DataGridViewColumn dgvc in dgv.Columns) {
                    var nameToUse = (useActualNames ? dgvc.Name : dgvc.HeaderText).Replace(" *", "");
                    names.Add(nameToUse);
                }

                // set up some streams to manipulate data into a tab-delimited string
                using (var ms = new MemoryStream()) {
                    encoding = encoding ?? Encoding.UTF8;
                    using (var sr = new StreamReader(ms, encoding)) {
                        using (var sw = new StreamWriter(ms, encoding)) {

                            // output column info
                            Toolkit.OutputTabbedHeader(names.ToArray(), sw);

                            // output data from selected rows
                            var values = new List<object>();
                            foreach (DataGridViewRow dgvr in dgv.SelectedRows) {
                                values.Clear();
                                foreach (DataGridViewCell cell in dgvr.Cells) {
                                    //if (cell is DataGridViewCheckBoxCell) {
                                    //    values.Add(cell.Value);
                                    //} else {
                                    values.Add(cell.Value);
                                    //}
                                }
                                Toolkit.OutputTabbedData(values.ToArray(), sw, false);
                            }

                            // retrieve contents from the memory stream
                            sw.Flush();
                            sr.BaseStream.Position = 0;
                            var s = sr.ReadToEnd();

                            return s;

                        }
                    }
                }
            }
        }

        private void handlePastedData(string text) {
            if (text.Contains("\t")) {

                _validationErrorCount = dgvSource.ImportData(Toolkit.ParseTabDelimited(text, true, Encoding.UTF8));
                updateStatusInfo();

                jumpToNextError(true);

            } else {
                // they're pasting just a single column.
                foreach (DataGridViewCell cell in dgvSource.SelectedCells) {
                    cell.Value = text;
                }
            }
        }

        private void lnkWhyNoDragDrop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show(this, getDisplayMember("whyNoDragDrop{start_body}", "The Import Wizard is running with elevated privileges on a Windows Vista or Windows 7 operating system.\nIn this situation, Windows will sometimes prevent drag-and-drop operations for the sake of security.\n\nHowever, copy / paste still works properly, so that may be used in its place."));
        }

        private void lnkRequired_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            var f = new frmSourceDataDetails();
            f.SourceGridView = this.dgvSource;
            f.ShowDialog();
        }

        /// <summary>
        /// returns true if link is being displayed, false otherwise
        /// </summary>
        /// <returns></returns>
        private bool updateJumpErrorLink() {
            if (_validationErrorCount <= 0) {
                _validationErrorCount = 0;
                lnkNextError.Visible = false;
            } else {
                lnkNextError.Visible = true;
                lnkNextError.Text = "Jump to next error (" + _validationErrorCount.ToString("###,###,##0") + ")";
            }
            return lnkNextError.Visible;
        }

        private void dgvSource_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex > -1) {

                //var dr = (dgvSource.Rows[e.RowIndex].DataBoundItem as DataRowView).Row;
                //if (!dr.HasErrors){
                //    _validationErrorCount--;
                //}


                var txt = ("" + dgvSource.CurrentCell.Value).ToString();

                if (txt.Contains("\t")) {
                    dgvSource.CurrentCell.Value = "";
                    using (var splash = new frmSplash(getDisplayMember("sourceCellEndEdit{populating}", "Populating view, please be patient..."), false, this, true)) {
                        handlePastedData(txt);
                    }
                }

                syncImportButton();
                //var dt = dgvSource.DataSource as DataTable;
                //var dr = dt.Rows[e.RowIndex];
                //if (dr.HasErrors) {
                //    var col = dr.Table.Columns[e.ColumnIndex];
                //    var errCols = dr.GetColumnsInError();
                //    if (errCols.Contains(col)) {

                //    }
                //}
                //dt.Rows[e.RowIndex].HasErrors();
            }
        }

        private void dgvSource_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e) {
        }

        private void dgvSource_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e) {
        }

        private void syncImportButton() {
            btnImport.Enabled = dgvSource.Rows.Count > 1; // 1 because the 'new' row is always visible
        }

        private List<string> _outdatedSearchEngineIndexes = new List<string>();

        private List<string> getChildrenAliases(DataColumn dc) {
            var rv = new List<string>();
            if (dc.ExtendedProperties.ContainsKey("join_children")) {
                var children = dc.ExtendedProperties["join_children"].ToString().Split(',');
                foreach (var c in children) {
                    rv.Add(c.Split('.')[0]);
                }
            }
            return rv;
        }

        private string getTableAlias(DataColumn dc) {
            return dc.ExtendedProperties.ContainsKey("table_alias_name") ? dc.ExtendedProperties["table_alias_name"].ToString() : null;
        }

        /// <summary>
        /// Given a column, inspect ExtendedProperties throughout other columns to determine all tables that directly reference the table the given column belongs to
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private List<string> getParentTables(DataTable dt, string columnName) {
            var rv = new List<string>();
            var myAlias = getTableAlias(dt.Columns[columnName]); // tg
            if (!String.IsNullOrEmpty(myAlias)) {
                foreach (DataColumn dc in dt.Columns) {
                    var otherAlias = getTableAlias(dc);
                    // not in the same table as the given column, not already in our list...
                    if (otherAlias != myAlias && !rv.Contains(otherAlias)) {
                        // get all aliases 
                        var aliases = getChildrenAliases(dc);
                        if (aliases.Contains(myAlias)) {
                            rv.Add(otherAlias);
                        }
                    }
                }
            }
            return rv;
        }

        private int _validationErrorCount;

        private void btnImport_Click(object sender, EventArgs e) {
            var switchTabs = true;
            var hardError = false;
            try {
                GC.Collect();
                using (var splash = new frmSplash(getDisplayMember("import{preparing}", "Preparing data, please be patient..."), false, this, true)) {
                    splash.DisableOwner = true;
                    this.Synchronize(true, delegate() {

                        // copy the datatable so it's essentially disassociated from the gridview datasource
                        var dtData = dgvSource.DataSource as DataTable;

                        try {

                            // disassociate the gridview so it's not firing a bunch of events
                            dgvResults.DataSource = null;

                            splash.ChangeText(getDisplayMember("import{validating}", "Validating data to import..."));
                            var tables = new Dictionary<string, List<DataColumn>>();

                            // group columns by their tables
                            for (var i = 0; i < dtData.Columns.Count; i++) {
                                var col = dtData.Columns[i];
                                var tableName = col.ExtendedProperties["table_name"] + "/" + col.ExtendedProperties["table_alias_name"];

                                List<DataColumn> list = null;
                                if (tables.TryGetValue(tableName, out list)) {
                                    list.Add(col);
                                } else {
                                    list = new List<DataColumn>();
                                    list.Add(col);
                                    tables.Add(tableName, list);
                                }
                            }

                            // scrub data as best we can prior to import
                            _validationErrorCount = scrubImportData(dtData, tables, splash);
                            if (_validationErrorCount == 0) {

                                try {

#if NOCOPY_DATATABLE
// Use this if trigger issues crop up!
// it was removed because it was causing out of memory exceptions...
                                    // keep their original data (in case a trigger overwrites it for some reason)
                                    var dtCopy = dtData.Copy();
                                    var ds = new DataSet();
                                    ds.Tables.Add(dtCopy);

                                    // actually do the import (which may cause errors since we can't scrub everything up front)
                                    var dtSaved = importData(dtCopy, splash);
#else
                                    var dtSaved = importData(dtData, splash);
#endif

                                    splash.ChangeText(getDisplayMember("import{cleaningup}", "Cleaning up..."));
#if NOCOPY_DATATABLE
                                    dtCopy.Dispose();
                                    dtCopy = null;
                                    ds.Dispose();
                                    ds = null;
#else
#endif
                                    // do an intermediary cleanup to try to minimize memory usage
                                    GC.Collect();

                                    // display the results
                                    displayResults(dtSaved, tables, splash);

                                } catch (Exception exImport) {
                                    splash.Close();

                                    var msg = exImport.Message;
                                    if (exImport is SoapException && exImport.Message.Contains("--->")) {
                                        msg = Toolkit.Cut(exImport.Message, exImport.Message.IndexOf("--->") + 4);
                                    }

                                    MessageBox.Show(this, getDisplayMember("import{failed_body}", "Fatal error during import: {0}", msg), 
                                        getDisplayMember("import{failed_title}", "Import Failed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    hardError = true;
                                    return;
                                }
                            } else {
                                switchTabs = false;
                                dgvSource.CurrentCell = null;
                                jumpToNextError(true);
                                return;
                            }
                        } finally {
                            // rebind gridview since we're done with it
                            dgvSource.DataSource = dtData;
                        }

                    });
                }
            } catch (Exception exBad) {
                MessageBox.Show(getDisplayMember("import{failed2_body}", "Fatal error: {0}", exBad.Message));
            }

            if (hardError) {
                return;
            }

            if (switchTabs) {
                using (var splash = new frmSplash(getDisplayMember("import{inspecting}", "Inspecting results..."), false, this, true)) {
                    splash.DisableOwner = true;
                    moveToNextTab();
                }
            } else {
                MessageBox.Show(this, getDisplayMember("import{missingData_body}", "Some required data is missing.\nPlease provide it and try again."), 
                    getDisplayMember("import{missingData_title}", "Missing Required Data"));
            }

        }

        private int scrubImportData(DataTable dtData, Dictionary<string, List<DataColumn>> tables, frmSplash splash) {


            var errorCount = 0;

            using (new FreezeWindow(dgvSource.Handle)) {
                // scrub for missing required fields
                for (var i = 0; i < dtData.Rows.Count; i++) {
                    if (i % 100 == 0 && i > 0) {
                        splash.ChangeText(getDisplayMember("scrubImportData{validated}", "Validated {0} rows...", i.ToString("###,###,##0")));
                    }
                    var dr = dtData.Rows[i];
                    dr.ClearErrors();
                    if (dr.RowState == DataRowState.Deleted) {
                        // we never ever EVER delete rows from IW..
                        // so we accept the changes so the middle tier sees it as a noop, if anything.
                        dr.AcceptChanges();
                    } else if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                        //var prevTableMissing = false;
                        var missingTables = new List<string>();
                        var keys = tables.Keys.ToList();
                        for (var j = 0; j < keys.Count; j++) {
                            var key = keys[j];
                            var cols = tables[key];

                            if (cols.Count > 0) {

                                var missingRequired = new List<DataColumn>();
                                var provided = new List<DataColumn>();
                                foreach (DataColumn dc in cols) {
                                    if (dc.ExtendedProperties["is_nullable"].ToString().ToUpper() == "Y") {
                                        if (dr[dc.ColumnName] != DBNull.Value) {
                                            provided.Add(dc);
                                        }
                                    } else {
                                        if (dc.ExtendedProperties["gui_hint"].ToString().ToUpper() == "TOGGLE_CONTROL") {
                                            // checkbox entries can be safely ignored, they'll get a default of N
                                            provided.Add(dc);
                                        } else if (dr[dc.ColumnName] == DBNull.Value && !dc.ReadOnly) {
                                            // required field, not given.  add it to the missing list.
                                            missingRequired.Add(dc);

                                            if (!missingTables.Contains(key)) {
                                                missingTables.Add(key);
                                            }

                                            //prevTableMissing = true;
                                        } else {
                                            // required field, not null, so it's provided.
                                            provided.Add(dc);
                                        }
                                    }
                                }

                                if (provided.Count > 0) {

                                    // they gave us at least one field that they want us to write.
                                    // if they don't give us all the required ones, we need to show that to them.

                                    if (missingRequired.Count > 0) {
                                        // if all provided columns are checkboxes (which we auto-adjust to fill in valid values)
                                        // ignore and mark them back to null.
                                        var allAreCheckboxes = true;
                                        foreach (var dc in provided) {
                                            if (dc.ExtendedProperties["gui_hint"] + string.Empty != "TOGGLE_CONTROL") {
                                                if ((dc.ExtendedProperties["is_readonly"] + string.Empty).Trim().ToUpper() != "Y") {
                                                    allAreCheckboxes = false;
                                                    break;
                                                }
                                            }
                                        }

                                        if (allAreCheckboxes) {
                                            foreach (var dc in provided) {
                                                if (!dc.ReadOnly) {
                                                    dr[dc.ColumnName] = DBNull.Value;
                                                }
                                            }
                                        } else {
                                            var writableMissing = new List<DataColumn>();
                                            foreach (var mr in missingRequired) {
                                                if ((mr.ExtendedProperties["is_readonly"] + string.Empty).Trim().ToUpper() != "Y") {
                                                    writableMissing.Add(mr);
                                                }
                                            }
                                            if (writableMissing.Count > 0) {
                                                errorCount++;
                                                foreach (var mr in writableMissing) {
                                                    dr.SetColumnError(mr, "Required field not provided");
                                                    dgvSource.Rows[i].ErrorText = "In " + mr.Caption + ": Required field not provided";
                                                }
                                            }
                                        }


                                        // if a parent table is missing, mark this row as being invalid.
                                        var parentTableIsMissing = false;
                                        var parents = getParentTables(dtData, cols[0].ColumnName);
                                        foreach (var p in parents) {
                                            if (missingTables.Contains(p)) {
                                                parentTableIsMissing = true;
                                                break;
                                            }
                                        }

                                        if (parentTableIsMissing) {
                                            errorCount++;
                                            foreach (var p in provided) {
                                                dr.SetColumnError(p, "Required fields in previous table(s) not provided");
                                                dgvSource.Rows[i].ErrorText = "In " + p.Caption + ": Required fields in previous table(s) not provided";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return errorCount;
        }

        private DataTable importData(DataTable dtData, frmSplash splash) {
            // issue the import
            splash.ChangeText(getDisplayMember("importData{start}", "Importing data, please be patient..."));

            var altLanguageID = Toolkit.ToInt32(ddlLanguage.GetValue(HostProxy.LanguageID), HostProxy.LanguageID);
            var ownerID = Toolkit.ToInt32(ddlOwnedBy.SelectedValue, -1);
            DataSet ds = null;
            DataTable dtSavedData = null;

            // disassociate datagridview so we don't get the big bad red "X"
            using (new FreezeWindow(dgvSource.Handle)) {
                dgvSource.Visible = false;
                dgvSource.DataSource = null;

                var dt = ddlDataType.DataSource as DataTable;
                var dr = dt.Rows[ddlDataType.SelectedIndex];
                var opts = dr["configuration_options"].ToString().ToLower();
                var insertOnlyLanguageData = false;
                var kv = Toolkit.ParsePairs<string>(opts);
                string temp = null;
                if (kv.TryGetValue("insertonlylanguagedata", out temp)) {
                    insertOnlyLanguageData = Toolkit.ToBoolean(temp, insertOnlyLanguageData);
                }

                var worker = this.ProcessInBackground(wrk => {
                    ds = HostProxy.SaveData(dtData.DataSet, altLanguageID, insertOnlyLanguageData, ownerID, wrk);
                    dtSavedData = ds.Tables[dtData.TableName];
                },
                (wrk, progress) => {
                    var txt = getDisplayMember("importData{processed}", "Processed {0} of {1} rows ({2}% complete)", Toolkit.ToInt32(progress.UserState, 0).ToString("###,###,##0"), dtData.Rows.Count.ToString("###,###,##0"), progress.ProgressPercentage.ToString("##0"));
                    //if (wrk.CancellationPending) {
                    //    txt = "Import has been cancelled.  Some data may have already been imported.\r\n" + txt;
                    //}
                    splash.ChangeText(txt);
                },
                (wrk, result) => {
                    if (result.Error != null) {
                        // Error filled, an exception was thrown
                        MessageBox.Show(getDisplayMember("importData{saveerror_body}", "error during save: {0}", result.Error.Message));

                        // due to a race condition in the Cancelled property, we can't use it here...
                        // http://msdn.microsoft.com/en-us/library/system.componentmodel.runworkercompletedeventhandler.aspx
                        // } else if (result.Cancelled){
                    } else if (splash.UserCancelled) {
                        // must have been cancelled.
                        MessageBox.Show(this, getDisplayMember("importData{cancelled_body}", "Import cancelled by user.  Some data may have been already imported."), 
                            getDisplayMember("importData{cancelled_title}", "Import Cancelled"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else {
                        // worked! nothing to do...
                    }
                });

                splash.Worker = worker;

                while (worker.IsBusy) {
                    Application.DoEvents();
                    Thread.Sleep(50);
                }

                // reassociate the dataview
                splash.ChangeText(getDisplayMember("importData{repopulating}", "Repopulating user interface..."));
                dgvSource.DataSource = dtData;
                dgvSource.Visible = true;
            }
            return dtSavedData;
        }

        private void displayResults(DataTable dtSavedData, Dictionary<string, List<DataColumn>> tables, frmSplash splash) {

            splash.ChangeText(getDisplayMember("displayResults{processing}", "Processing results..."));

            var errors = 0;
            var lastIndex = -1;


            //using (new FreezeWindow(dgvResults.Handle)) {
            dgvResults.DataSource = null;

            dgvResults.Columns.Clear();

            dgvResults.AutoGenerateColumns = false;

            if (!dtSavedData.Columns.Contains("Results")) {
                dtSavedData.Columns.Add("Results", typeof(string));
            }
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Results", HeaderText = "Results" });

            if (!dtSavedData.Columns.Contains("Row Index")) {
                dtSavedData.Columns.Add("Row Index", typeof(int));
            }
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Row Index", HeaderText = "Row Index" });

            if (!dtSavedData.Columns.Contains("Table (Alias)")) {
                dtSavedData.Columns.Add("Table (Alias)", typeof(string));
            }
            dgvResults.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Table (Alias)", HeaderText = "Table (Alias)" });

            var columnNames = new List<String>();
            for (var i = 0; i < dtSavedData.Columns.Count; i++) {
                var dc = dtSavedData.Columns[i];
                dc.ReadOnly = false;
                if (String.Compare(dc.ColumnName, "exceptionindex", true) == 0) {
                    break;
                } else {
                    var name = dc.ColumnName;
                    if (dc.ExtendedProperties.ContainsKey("title")) {
                        dc.Caption = dc.ExtendedProperties["title"].ToString();
                    }
                    columnNames.Add(dc.ColumnName);
                    var dgvc = new DataGridViewTextBoxColumn();
                    dgvc.HeaderText = dc.Caption;
                    dgvc.DataPropertyName = dc.ColumnName;
                    dgvResults.Columns.Add(dgvc);

                }
            }

            try {

                dtSavedData.BeginLoadData();

                if (chkDisplayOnlyErrors.Checked) {
                    // copy in only the errored rows
                    for (var rowIndex = 0; rowIndex < dtSavedData.Rows.Count; rowIndex++) {
                        var dr = dtSavedData.Rows[rowIndex];
                        lastIndex = Toolkit.ToInt32(dr["OriginalRowIndex"], -1) + 1;

                        if (lastIndex % 100 == 0 && lastIndex > 0) {
                            splash.ChangeText(getDisplayMember("displayResults{inspected}", "Inspected {0} result rows...", lastIndex.ToString("###,###,##0")));
                        }

                        var result = dr["SavedStatus"].ToString();
                        if (result == "Failure") {
                            generateResultRow(dr, columnNames, lastIndex, ref errors);
                        }
                    }
                } else {
                    // copy in all rows
                    for (var rowIndex = 0; rowIndex < dtSavedData.Rows.Count; rowIndex++) {
                        var dr = dtSavedData.Rows[rowIndex];

                        lastIndex = Toolkit.ToInt32(dr["OriginalRowIndex"], -1) + 1;

                        if (lastIndex % 100 == 0 && lastIndex > 0) {
                            splash.ChangeText(getDisplayMember("displayResults{inspected}", "Inspected {0} result rows...", lastIndex.ToString("###,###,##0")));
                        }

                        generateResultRow(dr, columnNames, lastIndex, ref errors);

                    }
                }
                dtSavedData.AcceptChanges();

            } finally {
                dtSavedData.EndLoadData();
            }


            GC.Collect();

            dgvResults.DataSource = dtSavedData;
            //foreach (DataColumn dc in dtSavedData.Columns) {
            //    dgvResults.Columns[dc.ColumnName].HeaderText = dc.Caption;
            //}
            //}
            GC.Collect();

            if (errors > 0) {
                lblImportErrors.ForeColor = Color.Red;
                lblImportErrors.Text = errors + " errors occurred importing " + lastIndex.ToString("###,###,##0") + " items.";
                dgvResults.Columns[0].Width = 300;
                btnFixErrors.Visible = true;

            } else {
                lblImportErrors.ForeColor = Color.Black;
                if (lastIndex <= 0) {
                    lblImportErrors.Text = "No items were imported because no changes were made.";
                } else {
                    lblImportErrors.Text = "Successfully imported " + lastIndex.ToString("###,###,##0") + " items.  No errors occurred.";
                }
                dgvResults.Columns[0].Width = 120;
                btnFixErrors.Visible = false;
            }

            updateStatusInfo();

            foreach (var t in tables.Keys) {
                var justTableName = t.Split('/')[0];
                if (!_outdatedSearchEngineIndexes.Contains(justTableName)) {
                    _outdatedSearchEngineIndexes.Add(justTableName);
                }
            }

        }

        private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e) {
        }

        private void dgvSource_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            var row = getDataRow(sender, e.RowIndex);
            if (row != null) {
                var dgvc = getCell(sender, e.RowIndex, e.ColumnIndex);
                if (dgvc != null){
                    var dc = row.Table.Columns[e.ColumnIndex];
                    var guiHint = ("" + dc.ExtendedProperties["gui_hint"]);
                    object defaultValue = dc.ExtendedProperties["default_value"];
                    switch(guiHint.ToUpper()){
                        case "TOGGLE_CONTROL":
                            // invalid boolean value, assume false...always!
                            var dgvcbc = ((DataGridViewCheckBoxCell)dgvc);
                            if (defaultValue + string.Empty == dgvcbc.TrueValue + string.Empty){
                                dgvc.Value = dgvcbc.TrueValue;
                            } else {
                                dgvc.Value = dgvcbc.FalseValue;
                            }
                            break;
                        case "SMALL_SINGLE_SELECT_CONTROL":
                            row.SetColumnError(dc, "Must specify a valid value from the dropdown");
                            break;
                        default:
                            row.SetColumnError(dc, e.Exception.Message);
                            break;
                    }
                }
            }
            e.Cancel = true;
        }

        private void cmiImportDelete_Click(object sender, EventArgs e) {
            deleteSelectedRows(dgvSource);
        }

        private void dgvSource_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
        }

        private void btnImportRetry_Click(object sender, EventArgs e) {
            using (new FreezeWindow(this.Handle)) {
                // find all errored rows, set them in error in the gridview
                var dt = dgvSource.DataSource as DataTable;
                var dsClone = dt.DataSet.Clone();
                var dtClone = dsClone.Tables[dt.TableName];
                var dtResults = dgvResults.DataSource as DataTable;
                if (dtResults == null) {
                } else {
                    foreach (DataRow drRes in dtResults.Rows) {
                        var res = drRes["Results"].ToString();
                        if (res.ToUpper().StartsWith("FAILURE")) {
                            var idx = Toolkit.ToInt32(drRes["Row Index"], -1);
                            if (idx > -1) {
                                var dr = dt.Rows[idx-1];
                                var drClone = dtClone.Rows.Add(dr.ItemArray);
                                drClone.RowError = res;
                            }
                        }
                    }
                }
                bindGridView(dtClone);
            }
            moveToPreviousTab();
            this.Refresh();
        }

        private void lnkNextError_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            jumpToNextError(false);
        }


        private bool jumpToNextError(bool restart) {

            if (dgvSource.Rows.Count == 0 || dgvSource.Columns.Count == 0) {
                return false;
            }

            var j = 0;
            while (j < dgvSource.Rows[0].Cells.Count && !dgvSource.Rows[0].Cells[j].Visible) {
                j++;
            }
            var firstVisibleCell = dgvSource.Rows[0].Cells[j < dgvSource.Rows[0].Cells.Count ? j : 0];


            if (restart) {
                dgvSource.CurrentCell = firstVisibleCell;
            }

            if (dgvSource.CurrentCell == null) {
                dgvSource.CurrentCell = firstVisibleCell;
            }

            // start on same row, but one column past the current position
            var curRow = dgvSource.CurrentCell.RowIndex;
            var curCol = dgvSource.CurrentCell.ColumnIndex + 1;
            var dt = (dgvSource.DataSource as DataTable);
            if (dt.HasErrors) {
                for (var i = curRow; i < dt.Rows.Count; i++) {
                    var row = dt.Rows[i];
                    if (row.HasErrors) {
                        var cols = row.GetColumnsInError();
                        if (cols.Length > 0) {
                            foreach (var c in cols) {
                                if (i > curRow || dt.Columns.IndexOf(c) > curCol) {
                                    dgvSource.FirstDisplayedScrollingRowIndex = i;
                                    var newCell = dgvSource.Rows[i].Cells[c.ColumnName];
                                    if (newCell != dgvSource.CurrentCell){
                                        dgvSource.CurrentCell = newCell;
                                        return updateJumpErrorLink();
                                    } else {
                                        updateJumpErrorLink();
                                        return false;
                                    }
                                }
                            }
                        } else {
                            // no specific column, just set it to the first one.
                            dgvSource.FirstDisplayedScrollingRowIndex = i;
                            var newCell = dgvSource.Rows[i].Cells[0];
                            if (newCell != dgvSource.CurrentCell) {
                                dgvSource.CurrentCell = newCell;
                                return updateJumpErrorLink();
                            } else {
                                updateJumpErrorLink();
                                return false;
                            }
                        }
                    }
                }

                for (var i = 0; i <= curRow && i < dt.Rows.Count; i++) {
                    var row = dt.Rows[i];
                    if (row.HasErrors) {
                        var cols = row.GetColumnsInError();
                        if (cols.Length > 0) {
                            foreach (var c in cols) {
                                dgvSource.FirstDisplayedScrollingRowIndex = i;
                                var newCell = dgvSource.Rows[i].Cells[0];
                                if (newCell != dgvSource.CurrentCell) {
                                    dgvSource.CurrentCell = newCell;
                                    return updateJumpErrorLink();
                                } else {
                                    updateJumpErrorLink();
                                    return false;
                                }
                            }
                        } else {
                            if (i != curRow) {
                                dgvSource.FirstDisplayedScrollingRowIndex = i;
                                var newCell = dgvSource.Rows[i].Cells[0];
                                if (newCell != dgvSource.CurrentCell) {
                                    dgvSource.CurrentCell = newCell;
                                    return updateJumpErrorLink();
                                } else {
                                    updateJumpErrorLink();
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            //// we get here, no errors exist.
            _validationErrorCount = 0;
            return updateJumpErrorLink();
        }

        private void dgvSource_CurrentCellChanged(object sender, EventArgs e) {
            updateStatusInfo();
        }

        private void updateStatusInfo(){
            if (!Thread.CurrentThread.IsBackground) {
                if (tc.SelectedIndex == 1) {
                    statusCount.Text = (dgvSource.Rows.Count - 1) + " rows";
                    var curRow = (dgvSource.CurrentCell == null ? -1 : dgvSource.CurrentCell.RowIndex);
                    if (curRow < 0) {
                        statusRow.Text = "-";
                        statusText.Text = "-";
                        statusTable.Text = "-";
                    } else {
                        if (dgvSource.CurrentRow.IsNewRow) {
                            statusRow.Text = "New Row";
                        } else {
                            statusRow.Text = "Row " + (curRow + 1);
                        }
                        var drv =((DataRowView)dgvSource.CurrentRow.DataBoundItem);
                        if (drv != null) {
                            var dr = drv.Row;
                            var dcErrors = dr.GetColumnsInError();
                            if (dcErrors.Length == 0) {
                                statusText.Text = dgvSource.CurrentRow.ErrorText;
                            } else {
                                statusText.Text = dgvSource.CurrentRow.Cells[dcErrors[0].ColumnName].ErrorText;
                            }
                        } else {
                        }
                    }
                } else {
                    statusCount.Text = "-";
                    statusRow.Text = "-";
                    statusText.Text = "-";
                    statusTable.Text = "-";
                }
            }
        }

        private bool _tabValidated;

        private void cmiImportCopy_Click(object sender, EventArgs e) {
            var cells = dgvSource.SelectedCells;
            if (cells.Count > 0) {
                var perRow = cells.Count;
                var prevRow = cells[0].RowIndex;
                for (var i = 0; i < cells.Count; i++) {
                    if (cells[i].RowIndex != prevRow) {
                        perRow = i + 1;
                        break;
                    }
                }

                var output = new List<string>();
                var rows = cells.Count / perRow;
                for (var i = 0; i < rows; i++) {
                    var line = new List<string>();
                    for (var j = 0; j < perRow; j++) {
                        line.Add(cells[(i * perRow) + j].Value.ToString());
                    }
                    output.Add(String.Join("\t", line.ToArray()));
                }

                Clipboard.SetText(String.Join("\r\n", output.ToArray()));
            }
        }

        private void cmiImportPaste_Click(object sender, EventArgs e) {

        }

        private void dgvSource_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            var row = getDataRow(sender, e.RowIndex);
            if (row != null){
                var cell = getCell(sender, e.RowIndex, e.ColumnIndex); // as DataGridViewComboBoxCell;
                if (cell != null) {
                    if (row.HasErrors) {
                        if (cell.Value != null && cell.Value != DBNull.Value) {
                            row.SetColumnError(e.ColumnIndex, null);
                            var errCols = row.GetColumnsInError();
                            if (errCols.Length == 0) {
                                row.ClearErrors();
                                var gr = getGridRow(sender, e.RowIndex);
                                if (gr != null) {
                                    gr.ErrorText = null;
                                }
                            }
                            _validationErrorCount--;
                            updateJumpErrorLink();
                        }
                    } else {
                        row.ClearErrors();
                        var gr = getGridRow(sender, e.RowIndex);
                        if (gr != null && gr.ErrorText != null) {
                            gr.ErrorText = null;
                            _validationErrorCount--;
                        }
                        updateJumpErrorLink();
                    }
                }
            }
        }

        private void tc_Selecting(object sender, TabControlCancelEventArgs e) {
            if (!_tabValidated) {
                e.Cancel = true;
                if (tc.SelectedTab == tpPreview) {
                    dgvSource.Focus();
                }
            }
        }


        private DataGridViewRow getGridRow(object sender, int rowIndex) {
            if (rowIndex > -1) {
                var dgr = ((DataGridView)sender).Rows[rowIndex];
                return dgr;
            }
            return null;
        }

        private DataRow getDataRow(object sender, int rowIndex) {
            if (rowIndex > -1) {
                var dgr = ((DataGridView)sender).Rows[rowIndex];
                if (dgr != null) {
                    var drv = dgr.DataBoundItem as DataRowView;
                    if (drv != null) {
                        return drv.Row;
                    }
                }
            }
            return null;
        }



        private DataGridViewCell getCell(object sender, int rowIndex, int columnIndex) {
            if (rowIndex > -1 && columnIndex > -1) {
                var rv = ((DataGridView)sender).Rows[rowIndex].Cells[columnIndex];
                return rv;
            }
            return null;
        }


        private void dgvSource_CellEnter(object sender, DataGridViewCellEventArgs e) {
            var dr = getDataRow(sender, e.RowIndex);
            if (dr != null) {
                var dc = dr.Table.Columns[e.ColumnIndex];
                statusTable.Text = "" + dc.ExtendedProperties["table_name"] + " (" + dc.ExtendedProperties["table_alias_name"] + ")";
            } else {
                var dt = dgvSource.DataSource as DataTable;
                if (dt != null && e.ColumnIndex > -1 && e.ColumnIndex < dt.Columns.Count) {
                    var dc = dt.Columns[e.ColumnIndex];
                    statusTable.Text = "" + dc.ExtendedProperties["table_name"] + " (" + dc.ExtendedProperties["table_alias_name"] + ")";
                }
            }
        }

        private void statusText_Click(object sender, EventArgs e) {
            if (statusText.Text != "-") {
                MessageBox.Show(this, getDisplayMember("statusText{body}", "Error:\n{0}", statusText.Text), 
                    getDisplayMember("statusText{title}", "Error"));
            }
        }

        private void statusText_MouseEnter(object sender, EventArgs e) {
            if (statusText.Text != "-") {
                this.Cursor = Cursors.Hand;
            }
        }

        private void statusText_MouseLeave(object sender, EventArgs e) {
            this.Cursor = Cursors.Default;
        }

        private void dgvResults_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) {
            var dt = dgvResults.DataSource as DataTable;
            if (dt != null && dt.Columns.Contains("Row Index") && e.RowIndex > -1) {
                var row = dt.Rows[e.RowIndex];
                var ri = Toolkit.ToInt32(row["Row Index"], -1);
                if (ri % 2 == 0) {
                    dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                } else {
                    dgvResults.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                //if (row["Results"].ToString().ToUpper().StartsWith("FAILURE")) {
                //    dgvResults.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //}
            }
        }

        private void frmImport_KeyPress(object sender, KeyPressEventArgs e) {
        }

        private void frmImport_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F5 && tc.SelectedTab == tpDataType) {
                initGui();
            } else if (e.KeyCode == Keys.Delete) {
                if (dgvSource.SelectedRows.Count > 0) {
                    deleteSelectedRows(dgvSource);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } else if (dgvSource.Focused && dgvSource.CurrentCell != null && !dgvSource.CurrentCell.IsInEditMode) {
                    dgvSource.CurrentCell.Value = null;
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            } else if (e.KeyCode == Keys.F3 && tc.SelectedTab == tpSource) {
                e.SuppressKeyPress = true;
                if (!jumpToNextError(false)) {
                    //if (dgvSource.CurrentCell.IsInEditMode || (String.IsNullOrEmpty(dgvSource.CurrentRow.ErrorText) && _validationErrorCount > 0)) {
                    //    var newRow = dgvSource.CurrentCell.RowIndex - 1;
                    //    if (newRow >= 0) {
                    //        dgvSource.CurrentCell = dgvSource.Rows[newRow].Cells[dgvSource.CurrentCell.ColumnIndex];
                    //    } else {
                    //        newRow = dgvSource.CurrentCell.RowIndex + 1;
                    //        if (newRow < dgvSource.Rows.Count) {
                    //            dgvSource.CurrentCell = dgvSource.Rows[newRow].Cells[dgvSource.CurrentCell.ColumnIndex];
                    //        } else {
                    //            dgvSource.EndEdit();
                    //        }
                    //    }
                    //}
                }
            }
        }

        private void dgvSource_KeyPress(object sender, KeyPressEventArgs e) {
        }

        private void deleteSelectedRows(DataGridView dgv) {
            using (var splash = new frmSplash(getDisplayMember("deleteSelectedRows{progress}", "Removing rows, please be patient..."), false, this, true)) {
                var dt = dgv.DataSource as DataTable;
                using (new AutoCursor(this)) {
                    dt.BeginLoadData();
                    try {
                        dgv.SuspendLayout();
                        dt.BeginLoadData();
                        using (new FreezeWindow(dgv.Handle)) {
                            if (dgv.SelectedRows.Count >= dt.Rows.Count) {
                                dt.Rows.Clear();
                                _validationErrorCount = 0;
                            } else {


                                var i = 0;
                                var total = dgv.SelectedRows.Count.ToString("###,###,##0");
                                foreach (DataGridViewRow row in dgv.SelectedRows) {
                                    if (!row.IsNewRow) {

                                        var dr = ((DataRowView)row.DataBoundItem).Row;
                                        if (dr.HasErrors) {
                                            _validationErrorCount--;
                                        }
                                        dt.Rows.Remove(dr);

                                        // doesn't work when sorted...
                                        // dt.Rows.RemoveAt(row.Index);

                                        i++;
                                        if (i % 100 == 0) {
                                            splash.ChangeText(getDisplayMember("deleteSelectedRows{removed}", "Removed {0} of {1}", i.ToString("###,###,##0"), total));
                                        } else if (i % 20 == 0) {
                                            Application.DoEvents();
                                        }
                                    }
                                }
                            }
                            //while (dgv.SelectedRows.Count > 0) {
                            //    if (dgv.SelectedRows[0].IsNewRow) {
                            //        if (dgv.SelectedRows.Count == 1) {
                            //            break;
                            //        } else {
                            //            dgv.Rows.RemoveAt(dgvSource.SelectedRows[1].Index);
                            //        }
                            //    } else {
                            //        dgv.Rows.RemoveAt(dgvSource.SelectedRows[0].Index);
                            //    }
                            //}
                        }
                    } finally {
                        dt.EndLoadData();
                        dgv.ResumeLayout();
                    }
                    dt.EndLoadData();
                    updateJumpErrorLink();
                    updateStatusInfo();
                }
            }
        }

        //private int _currentRowErrorCount;
        private void dgvSource_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
            //_currentRowErrorCount = 0;
            //var dr = getDataRow(sender, e.RowIndex);
            //if (dr != null && dr.HasErrors) {
            //    _currentRowErrorCount = dr.GetColumnsInError().Length;
            //}
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "ImportWizard", "frmImport", resourceName, null, defaultValue, substitutes);
        }
    }
}
