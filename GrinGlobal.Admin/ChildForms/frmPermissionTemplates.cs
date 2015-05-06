using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.Admin.ChildForms {
    public partial class frmPermissionTemplates : GrinGlobal.Admin.ChildForms.frmBase {
        public frmPermissionTemplates() {
            InitializeComponent();  tellBaseComponents(components);
        }


        public List<int> AssignedIDs = new List<int>();
        public List<int> SelectedIDs = new List<int>();


        public override void RefreshData() {

            //lvPerms.MultiSelect = !Modal;

            this.Text = "Permission Templates - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");
            var tags = rememberSelectedTags(lvPermissionTemplates);

            // this form has multiple uses, as does the underlying ListPermissions method.
            // it can:
            // List all permissions (0,[0])
            // List permission info for a single permission (37,[0])
            // List permissions that are NOT in a list (0, [2,3,4,5,6])


            var ds = AdminProxy.ListPermissionTemplates(ID);

            initHooksForMdiParent(ds.Tables["list_permission_templates"], "template_name", "sys_perm_template_id");
            lvPermissionTemplates.Items.Clear();
            foreach (DataRow dr in ds.Tables["list_permission_templates"].Rows) {
                var lvi = new ListViewItem(dr["template_name"].ToString(), 0);
                lvi.Tag = Toolkit.ToInt32(dr["sys_perm_template_id"], -1);
                DataRow[] drPerms = ds.Tables["permissions_by_template"].Select("sys_perm_template_id = " + dr["sys_perm_template_id"]);
                var perms = new List<string>();
                foreach (DataRow drp in drPerms) {
                    perms.Add(drp["perm_name"].ToString());
                }
                lvi.SubItems.Add(Toolkit.Join(perms.ToArray(), ", ", "-"));
                lvi.Tag = dr["sys_perm_template_id"];
                lvPermissionTemplates.Items.Add(lvi);
            }

            selectRememberedTags(lvPermissionTemplates, tags);

            MainFormUpdateStatus("Refreshed Permission Templates ", false);
        }

        private void showProperties() {
            if (lvPermissionTemplates.SelectedItems.Count == 1) {
                MainFormSelectChildTreeNode(lvPermissionTemplates.SelectedItems[0].Tag.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (lvPermissionTemplates.SelectedItems.Count == 0) {
                MessageBox.Show("You must select at least one permission template to add.");
            } else {
                SelectedIDs.Clear();
                foreach (ListViewItem lvi in lvPermissionTemplates.SelectedItems) {
                    SelectedIDs.Add(Toolkit.ToInt32(lvi.Tag, -1));
                }
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();

        }

        private void frmPermissionTemplates_Load(object sender, EventArgs e) {
            MakeListViewSortable(lvPermissionTemplates);
            if (!Modal) {
                btnOK.Visible = false;
                btnCancel.Visible = false;
                lvPermissionTemplates.Dock = DockStyle.Fill;
            }
        }

        private void defaultPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            showProperties();

        }

        private void refreshPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            RefreshData();

        }

        private void deletePermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            promptToDelete();

        }

        private void promptToDelete() {
            if (lvPermissionTemplates.SelectedItems.Count > 0) {
                if (DialogResult.Yes == MessageBox.Show(this, "Are you sure you want to delete permission template(s)?\n\nThis will not delete any permissions or alter any user permissions in any way.", "Delete Permission Template(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) {
                    foreach (ListViewItem lvi in lvPermissionTemplates.SelectedItems) {
                        AdminProxy.DeletePermissionTemplate(Toolkit.ToInt32(lvi.Tag, -1));
                    }
                    MainFormUpdateStatus("Deleted " + lvPermissionTemplates.SelectedItems.Count + " permission templates.", true);
                    RefreshData();
                }
            }
        }

        private void ctxMenuNodePermission_Opening(object sender, CancelEventArgs e) {
            cmiDelete.Enabled = lvPermissionTemplates.SelectedItems.Count > 0;
            cmiProperties.Enabled = lvPermissionTemplates.SelectedItems.Count == 1;

        }

        private void newPermissionTemplateMenuItem_Click(object sender, EventArgs e) {
            MainFormPopupNewItemForm(new frmPermissionTemplate());
        }

        private void lvPermissionTemplates_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (Modal) {
                btnOK.PerformClick();
            } else {
                showProperties();
            }

        }

        private void lvPermissionTemplates_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                promptToDelete();
            }

        }

        private void lvPermissionTemplates_KeyPress(object sender, KeyPressEventArgs e) {
            if ((Keys)e.KeyChar == Keys.Enter) {
                btnOK.PerformClick();
            }

        }

        private void lvPermissionTemplates_ItemDrag(object sender, ItemDragEventArgs e) {
            startDrag(sender);
        }

        private void cmiTemplateExportList_Click(object sender, EventArgs e) {
            ExportListView(lvPermissionTemplates);

        }
    
    }
}
