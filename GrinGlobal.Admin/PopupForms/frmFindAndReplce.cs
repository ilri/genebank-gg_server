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
    public partial class frmFindAndReplace : GrinGlobal.Admin.ChildForms.frmBase {
        public frmFindAndReplace() {
            InitializeComponent();  tellBaseComponents(components);
        }

        public TextBox TargetTextBox { get; set; }


        private void btnAdd_Click(object sender, EventArgs e) {
            var pos = TargetTextBox.SelectionStart + TargetTextBox.Text.Length;
            doFind(true, 1, true);
        }

        public void doReplaceAll() {
            var rv = doFind(true, int.MaxValue, false);
            if (rv.Count > 0){
                TargetTextBox.Text = TargetTextBox.Text.Replace(txtFind.Text, txtReplace.Text);
                MessageBox.Show(this, getDisplayMember("doReplaceAll{body}", "Replaced {0} occurrences of '{1}'.", rv.Count.ToString(), txtFind.Text), 
                    getDisplayMember("doReplaceAll{title}", "Text Replaced"));
            }

        }

        private void doReplace(bool promptOnNotFound) {
            var result = doFind(promptOnNotFound, 1, false);
            if (result.Count > 0){
                // focus on the found text
                var startPos = result[0];
                TargetTextBox.SelectionStart = startPos;
                TargetTextBox.SelectionLength = txtFind.Text.Length;

                // replace it
                TargetTextBox.SelectedText = txtReplace.Text;

                // focus on the newly inserted text
                TargetTextBox.SelectionStart = startPos;
                TargetTextBox.SelectionLength = txtReplace.Text.Length;
                TargetTextBox.ScrollToCaret();
            }

        }

        /// <summary>
        /// Returns 1 on found, 0 if never found, -1 if found after cycling back to the beginning
        /// </summary>
        /// <param name="promptOnNotFound"></param>
        /// <returns></returns>
        public List<int> doFind(bool promptOnNotFound, int maxHitCount, bool displayFirstHit){

            var rv = new List<int>();
            if (txtFind.Text.Trim().Length == 0) {
                MessageBox.Show(this, getDisplayMember("doFind{noword_body}", "Please enter the word you want to find."), 
                    getDisplayMember("doFind{noword_title}", "Please Enter Word"));
                txtFind.SelectAll();
                txtFind.Focus();
                return rv;
            }

            rv = findText(TargetTextBox, txtFind.Text, chkIgnoreCase.Checked, maxHitCount);
            if (rv.Count == 0) {
                if (promptOnNotFound) {
                    MessageBox.Show(this, getDisplayMember("doFind{notfound_body}", "'{0}' was not found.", txtFind.Text),
                        getDisplayMember("doFind{notfound_title}", "Text Not Found"));
                }
            } else if (displayFirstHit) {
                TargetTextBox.SelectionStart = rv[0];
                TargetTextBox.SelectionLength = txtFind.Text.Length;
                TargetTextBox.ScrollToCaret();
            }

            return rv;

        }

        private List<int> findText(TextBox tgt, string textToFind, bool ignoreCase, int maxHitCount) {
            var beginPos = tgt.SelectionStart + tgt.SelectionLength;
            var hits = new List<int>();
            var done = false;
            var cycled = false;
            var hitPos = beginPos;

            var sourceText = (ignoreCase ? tgt.Text.ToLower() : tgt.Text);
            var searchText = (ignoreCase ? textToFind.ToLower() : textToFind);

            while (!done && hits.Count < maxHitCount) {

                // get next hit
                hitPos = sourceText.IndexOf(searchText, hitPos);

                if (cycled) {
                    // already cycled around once.
                    if (hitPos == -1 || hitPos > beginPos) {
                        // it's not found at all, or the hit is after our beginning position, we're done
                        done = true;
                    } else {
                        // add the hit to the output
                        hits.Add(hitPos);
                        hitPos += searchText.Length;
                    }
                } else {
                    if (hitPos == -1) {
                        // went past the end of the text, cycle around to the front.
                        cycled = true;
                        hitPos = 0;
                    } else {
                        // add the hit to the output
                        hits.Add(hitPos);
                        hitPos += searchText.Length;
                    }
                }
            }
            return hits;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Hide();
            Owner.Focus();
        }

        private void frmAddWord_Load(object sender, EventArgs e) {
            MarkClean();
        }

        private void txtWord_TextChanged(object sender, EventArgs e) {
            CheckDirty();

        }

        private void frmFindAndReplace_Activated(object sender, EventArgs e) {
            txtFind.Focus();
        }

        private void btnReplaceAll_Click(object sender, EventArgs e) {
            doReplaceAll();
        }

        private void btnReplace_Click(object sender, EventArgs e) {
            doReplace(true);
        }

        private void frmFindAndReplace_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            if (e.KeyCode == Keys.F3) {
                doFind(true, 1, true);
            }
        }

        private void txtReplace_Enter(object sender, EventArgs e) {
            txtReplace.SelectionStart = 0;
            txtReplace.SelectionLength = txtReplace.Text.Length;
        }

        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "AdminTool", "frmFindAndReplace", resourceName, null, defaultValue, substitutes);
        }
    }
}
