using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;

namespace GrinGlobal.CooperatorMunger {
    public partial class frmCooperatorMunger : Form {
        public frmCooperatorMunger() {
            InitializeComponent();
        }

        private DataConnectionSpec _dcs;
        private void btnTest_Click(object sender, EventArgs e) {
            string providerName = rbFromDatabaseMySQL.Checked ? "mysql" :
    rbFromDatabaseOracle.Checked ? "oracle" :
    rbFromDatabasePostgreSQL.Checked ? "postgresql" :
    "sqlserver";
            _dcs = new DataConnectionSpec { ConnectionString = cboFromConnectionString.Text, ProviderName = providerName };


            using (var dm = DataManager.Create(_dcs)) {
                var output = dm.TestLogin();
                if (String.IsNullOrEmpty(output)) {
                    MessageBox.Show(this, "Connection Succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                    btnMunge.Enabled = true;
                } else {
                    MessageBox.Show(this, "Failed: " + output, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnMunge.Enabled = false;
                }
            }

        }

        private void update(string msg) {
            textBox1.AppendText(DateTime.Now.ToString() + " - " + msg + "\r\n");
            this.Refresh();
            Application.DoEvents();
        }

        private void btnMunge_Click(object sender, EventArgs e) {
            try {
                Cursor = Cursors.WaitCursor;
                btnTest.Enabled = false;
                btnMunge.Enabled = false;
                btnCancel.Visible = true;
                _cancel = false;
                using (var dm = DataManager.Create(_dcs)) {

                    update("Setting all phone numbers / emails / addresses to bogus values...");

                    dm.BeginTran();
                    // update all phone numbers / emails / addresses / etc to bogus values
                    dm.Write(@"
update
 cooperator
set
    primary_phone = '123 555 1234',
    secondary_phone = '123 555 1235',
    fax = '123 555 1236',
    email = 'nobody@example.com',
    address_line2 = 'Apt ABC',
    address_line3 = null
");

                    update("Getting existing information...");
                    // get a list of all data we want to shuffle around
                    var dt = dm.Read(@"
select 
    cooperator_id, 
    site_code, 
    first_name, 
    last_name, 
    organization, 
    job, 
    organization_code, 
    address_line1,
    admin_1, 
    admin_2, 
    category_code, 
    discipline, 
    initials, 
    full_name,
    note  
from 
    cooperator");

                    Application.DoEvents();
                    if (_cancel) {
                        update("Cancelled");
                        MessageBox.Show(this, "Action cancelled, all updates rolled back.");
                        return;
                    }


                    var rnd = new Random(DateTime.Now.Millisecond);

                    update("Scrambling data for " + dt.Rows.Count + " cooperators...");

                    // randomly switch values in each column
                    for (var i = 0; i < dt.Rows.Count; i++) {

                        var drSrc = dt.Rows[i];

                        for (var j = 1; j < dt.Columns.Count; j++) {
                            var tgt = rnd.Next(dt.Rows.Count);
                            while (tgt == i) {
                                // make sure source and target are not the same
                                tgt = rnd.Next(dt.Rows.Count);
                            }

                            var drDest = dt.Rows[tgt];

                            var temp = drSrc[j];
                            drSrc[j] = drDest[j];
                            drDest[j] = temp;

                            // To satisfy ndx_uniq_co constraint, we randomize info here...
                            switch(dt.Columns[j].ColumnName){
                                case "address_line1":
                                    drDest[j] = rnd.Next(30000) + " NW Nowhere St.";
                                    break;
                                case "full_name":
                                    drDest[j] = "Santa Claus " + rnd.Next(30000);
                                    break;
                            }
                        }

                        Application.DoEvents();
                        if (_cancel) {
                            update("Cancelled");
                            MessageBox.Show(this, "Cancelled, all updates rolled back");
                            return;
                        }

                    }


                    // create the sql to issue
                    var sql = "update cooperator set ";
                    var prms = new DataParameters();
                    foreach (DataColumn dc in dt.Columns) {
                        if (dc.ColumnName != "cooperator_id") {
                            if (prms.Count > 0) {
                                sql += ", ";
                            }
                            sql += " " + dc.ColumnName + " = :" + dc.ColumnName;
                            prms.Add(new DataParameter(":" + dc.ColumnName, null, dc.DataType));
                        }
                    }
                    sql += " where cooperator_id = :id";
                    prms.Add(new DataParameter(":id", null, DbType.Int32));




                    for(var i=0;i<dt.Rows.Count;i++){
                        var dr = dt.Rows[i];
                        // assign values from each row into correct parameter values
                        for (var j = 1; j < dt.Columns.Count; j++) {
                            prms[j-1].Value = dr[j];
                        }
                        // :id is last instead of first, but it is first in the datarow.
                        prms[prms.Count - 1].Value = dr[0];

                        // issue the sql
                        dm.Write(sql, prms);

                        if (i % 1000 == 0) {
                            Application.DoEvents();
                            update("Updated " + i + " cooperators so far");
                            if (_cancel) {
                                update("Cancelled");
                                MessageBox.Show(this, "Cancelled, all updates rolled back");
                                return;
                            }
                        }
                    }

                    dm.Commit();

                    update("Done!");
                    MessageBox.Show(this, "Done!");
                }
            } finally {
                btnMunge.Enabled = true;
                btnCancel.Visible = false;
                Cursor = Cursors.Default;
            }
        }

        private bool _cancel = false;
        private void btnCancel_Click(object sender, EventArgs e) {
            _cancel = true;
        }
    }
}
