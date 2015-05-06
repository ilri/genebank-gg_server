using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Import {
    public partial class frmChooseDelimiter : Form {
        public frmChooseDelimiter() {
            InitializeComponent();


            Sync(true, delegate() {
                var dt = new DataTable();
                dt.Columns.Add("value_member", typeof(string));
                dt.Columns.Add("display_member", typeof(string));
                dt.Rows.Add('\t', "Tab");
                dt.Rows.Add('|', "Pipe (|)");
                dt.Rows.Add('\0', "Custom...");
                dt.AcceptChanges();

                ddlColumnSeparator.ValueMember = "value_member";
                ddlColumnSeparator.DisplayMember = "display_member";
                ddlColumnSeparator.DataSource = dt;
            });
        }


        private bool _syncing;
        protected bool IsSyncing {
            get {
                return _syncing;
            }
        }
        public delegate void VoidCallback();
        /// <summary>
        /// Used to suppress GUI synchronization and event handling when an event is in the process of being handled.  (aka prevent controls from cross-firing events)
        /// </summary>
        /// <param name="callback"></param>
        protected void Sync(bool showCursor, VoidCallback callback) {
            if (!_syncing) {
                try {
                    _syncing = true;
                    if (showCursor) {
                        using (new AutoCursor(this)) {
                            callback();
                        }
                    } else {
                        callback();
                    }
                } finally {
                    _syncing = false;
                }
            }
        }



        private void frmChooseDelimiter_Load(object sender, EventArgs e) {

        }

        public void Init(char defaultDelimiter, string fileContents){
            FileContents = fileContents;
            ColumnDelimiter = defaultDelimiter;
            var dt = ddlColumnSeparator.DataSource as DataTable;
            for (var i = 0; i < dt.Rows.Count; i++) {
                if (dt.Rows[i]["value_member"].ToString() == ColumnDelimiter.ToString()) {
                    ddlColumnSeparator.SelectedIndex = i;
                    break;
                }
            }
            if (ddlColumnSeparator.SelectedIndex == -1) {
                ddlColumnSeparator.SelectedIndex = ddlColumnSeparator.Items.Count - 1;
                txtCustomColumnDelimiter.Text = ColumnDelimiter.ToString();
            }

            parseData(ColumnDelimiter.ToString());

        }

        private void ddlSeparator_SelectedIndexChanged(object sender, EventArgs e) {
            Sync(true, delegate() {
                if (ddlColumnSeparator.SelectedValue.ToString() == "\0") {
                    txtCustomColumnDelimiter.Visible = true;
                    txtCustomColumnDelimiter.Focus();
                    txtCustomColumnDelimiter.Select();
                } else {
                    txtCustomColumnDelimiter.Visible = false;
                    parseData(ddlColumnSeparator.SelectedValue.ToString());
                }
            });
        }

        private void txtCustomDelimiter_TextChanged(object sender, EventArgs e) {
            if (txtCustomColumnDelimiter.Text.StartsWith(@"\")) {
                txtCustomColumnDelimiter.MaxLength = 2;
            } else {
                txtCustomColumnDelimiter.MaxLength = 1;
            }

            Sync(true, delegate() {
                parseData(txtCustomColumnDelimiter.Text);
            });
        }


        public string FileContents;
        public char ColumnDelimiter;

        private void parseData(string delim) {
            lv.Items.Clear();
            lv.Columns.Clear();
            if (!String.IsNullOrEmpty(delim)) {
                if (delim[0] == '\\' && delim.Length > 1) {
                    // TODO: smash "\b" into '\b' (string to character)
                    ColumnDelimiter = Toolkit.Unescape(delim);
                } else {
                    ColumnDelimiter = delim[0];
                }
                foreach (var line in Toolkit.ParseDelimited(FileContents, true, Encoding.UTF8, ColumnDelimiter)) {
                    if (lv.Columns.Count == 0) {
                        foreach (var c in line) {
                            lv.Columns.Add(c, 80);
                        }
                    } else {
                        var lvi = new ListViewItem();
                        for(var i=0;i<line.Count;i++){
                            if (i == 0) {
                                lvi.Text = line[i];
                            } else {
                                lvi.SubItems.Add(line[i]);
                            }
                        }
                        lv.Items.Add(lvi);
                    }
                }
            } else {
                ColumnDelimiter = '\0';
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
