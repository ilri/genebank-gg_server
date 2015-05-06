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
    public partial class frmAddSeparator : GrinGlobal.Admin.ChildForms.frmBase {
        public frmAddSeparator() {
            InitializeComponent();  tellBaseComponents(components);
            SelectedCharacter = '\0';
        }

        public char SelectedCharacter { get; private set; }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (txtWord.Text.Length != 1) {
                MessageBox.Show(this, getDisplayMember("add{missingcharacter_body}", "Please enter the character you want to add as a separator."), 
                    getDisplayMember("add{missingcharacter_title}", "Please Enter Character"));
                txtWord.Focus();
                return;
            }
            SelectedCharacter = txtWord.Text.ToCharArray()[0];

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtWord_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void frmAddSeparator_Load(object sender, EventArgs e) {
            MarkClean();
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmAddSeparator", resourceName, null, defaultValue, substitutes);
        }
    }
}
