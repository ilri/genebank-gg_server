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
    public partial class frmAddWord : GrinGlobal.Admin.ChildForms.frmBase {
        public frmAddWord() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public string SelectedWord { get; private set; }


        private void btnAdd_Click(object sender, EventArgs e) {
            if (txtWord.Text.Trim().Length == 0) {
                MessageBox.Show(this, getDisplayMember("add{stopword_body}",  "Please enter the word you want to add as a Stop Word."), 
                    getDisplayMember("add{stopword_title}", "Please Enter Stop Word"));
                txtWord.SelectAll();
                txtWord.Focus();
                return;
            }
            SelectedWord = txtWord.Text.Trim();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmAddWord_Load(object sender, EventArgs e) {
            MarkClean();
        }

        private void txtWord_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmAddWord", resourceName, null, defaultValue, substitutes);
        }
    }
}
