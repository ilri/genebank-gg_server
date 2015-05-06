using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace GrinGlobal.InstallHelper
{
    public partial class frmDisclaimer : Form
    {
        private string[] lines;
        
        public frmDisclaimer()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmDisclaimer_Load(object sender, EventArgs e)
        {
            txtMessage.SelectionStart = 0;
            txtMessage.SelectionLength = 0;
            if (this.Owner != null)
            {
                this.Icon = this.Owner.Icon;
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                e.KeyCode == Keys.A) {
                txtMessage.SelectionStart = 0;
                txtMessage.SelectionLength = txtMessage.Text.Length;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            char[] param = {'\n'};
             
            lines = txtMessage.Text.Split(param);
              
            int i = 0;
            char[] trimParam = {'\r'};
            foreach (string s in lines)
            {
              lines[i++] = s.TrimEnd(trimParam);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            Brush brush = new SolidBrush(txtMessage.ForeColor);

            int linesPrinted = 0;
            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString (lines[linesPrinted++],
                txtMessage.Font, brush, x, y);
                y += 15;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
         
            e.HasMorePages = false;
        }
    }
}
