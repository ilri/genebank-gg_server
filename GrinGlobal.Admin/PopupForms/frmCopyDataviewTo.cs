using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Admin.ChildForms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.PopupForms {
    public partial class frmCopyDataviewTo : GrinGlobal.Admin.ChildForms.frmBase {
        public frmCopyDataviewTo() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public string OriginalDataviewName;
        public bool Copying;

        private void btnAdd_Click(object sender, EventArgs e) {
            if (txtNewDataviewName.Text.Trim().Length == 0) {
                if (Copying) {
                    MessageBox.Show(this, getDisplayMember("add{name_body}", "Please enter a name for the new dataview."), 
                        getDisplayMember("add{name_title}", "Please Enter Name"));
                } else {
                    MessageBox.Show(this, getDisplayMember("add{newname_body}", "Please enter a new name for the dataview."),
                        getDisplayMember("add{newname_title}", "Please Enter Name"));
                }
                txtNewDataviewName.SelectAll();
                txtNewDataviewName.Focus();
            } else if (String.Compare(txtNewDataviewName.Text.Trim(), OriginalDataviewName.Trim(), true) == 0) {
                MessageBox.Show(this, getDisplayMember("add{cantmatch_body}", "The new dataview name cannot match the original."), 
                    getDisplayMember("add{cantmatch_title}", "Please Enter Different Name"));
                txtNewDataviewName.SelectAll();
                txtNewDataviewName.Focus();
            } else {
                // see if that dataview already exists
                var ds = AdminProxy.GetDataViewDefinition(txtNewDataviewName.Text.Trim());
                if (ds.Tables["sys_dataview"].Rows.Count > 0) {
                    MessageBox.Show(this, getDisplayMember("add{existingdataview_body}", "A dataview with that name already exists.\nPlease enter a different name."), 
                        getDisplayMember("add{existingdataview_title}", "Please Enter Different Name"));
                    txtNewDataviewName.SelectAll();
                    txtNewDataviewName.Focus();
                } else {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public override void RefreshData() {
            // nothing to do
        }

        private void frmCopyDataviewTo_Load(object sender, EventArgs e) {
//            lblNewDataviewName.Text = "Enter name for new dataview based on '" + txtNewDataviewName.Text + "':";
            if (Copying) {
                Text = "Copy Dataview To...";
                lblNewDataviewName.Text = "Name for new dataview:";
            } else {
                Text = "Rename Dataview To...";
                lblNewDataviewName.Text = "New name for dataview:";
            }
            MarkClean();
        }

        private void frmCopyDataviewTo_Activated(object sender, EventArgs e) {
            txtNewDataviewName.Focus();

        }

        private void txtNewDataviewName_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmCopyDataviewTo", resourceName, null, defaultValue, substitutes);
        }
    }
}
