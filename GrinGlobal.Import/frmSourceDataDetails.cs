using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.Import {
    public partial class frmSourceDataDetails : Form {
        public frmSourceDataDetails() {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            this.Close();
        }

        public DataGridView SourceGridView;

        private void frmSourceDataDetails_Load(object sender, EventArgs e) {
            var colors = new List<Color>();
            var names = new List<string>();

            // copy the gridview structure and looks the one we're going to display
            var dt = (SourceGridView.DataSource as DataTable).Clone();
            dgvExample.DataSource = dt;
            frmImport.ColorizeGridView(dgvExample);


            for(var i=0;i<dgvExample.Columns.Count;i++){
                var dgvc = dgvExample.Columns[i];

                var cur = dgvc.HeaderCell.Style.BackColor;
                if (!colors.Contains(cur)) {
                    names.Add(dt.Columns[i].ExtendedProperties["table_name"].ToString());
                    colors.Add(cur);
                }
            }


            // fill the legend info
            lvTables.Items.Clear();
            for (var i = 0; i < colors.Count; i++) {
                var lvi = new ListViewItem(names[i]);
                lvi.BackColor = colors[i];
                lvTables.Items.Add(lvi);
            }

        }
    }
}
