using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GrinGlobal.Core;
using GrinGlobal.Business;

namespace GrinGlobal.Admin.ChildForms
{
    public partial class frmMappings : GrinGlobal.Admin.ChildForms.frmBase
    {
        private bool _checkdAll;

        public frmMappings()
        {
            InitializeComponent();
        }

        public override void RefreshData()
        {
            this.Text = "Table Mappings - " + this.AdminProxy.Connection.ServerName + (Toolkit.IsProcessElevated() ? " - Administrator " : "");

            //var ds = AdminProxy.ListTables(0); //Laura temp
            var ds = AdminProxy.ListTableNames(null); //Laura temp comment out  

            clbTables.Items.Clear();

            foreach (DataRow dr in ds.Tables["list_tables"].Rows)
            {
//                clbTables.Items.Add(dr["table_name"].ToString()); // brock commented out
                clbTables.Items.Add(dr["Value"].ToString());
            }

            MainFormUpdateStatus("Refreshed Table Lists ", false);
        }

        private void frmMappings_Load(object sender, EventArgs e)
        {
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            clbTables.BeginUpdate();

            for (int i = 0; i < clbTables.Items.Count; i++)
            {
                clbTables.SetItemChecked(i, !_checkdAll);
            }
            _checkdAll = !_checkdAll;

            clbTables.EndUpdate();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (clbTables.CheckedItems.Count != 0)
            {
                List<string> tableNames = new List<string>();

                for (int i = 0; i < clbTables.CheckedItems.Count; i++)
                {
                    tableNames.Add(clbTables.CheckedItems[i].ToString());
                }

                AdminProxy.RecreateTableMappings(tableNames);

                MainFormUpdateStatus("Mappings for all checked tables have been recreated ", true);
            }
            else
            {
                MessageBox.Show(this, "You must select at least one table to recreate mappings.", "Select Table(s)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void refreshTableMappingsMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void defaultTableMappingsMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
