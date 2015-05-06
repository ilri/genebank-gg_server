using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Interface.Dataviews;
using System.Diagnostics;

namespace GrinGlobal.Admin.ChildForms {
    public partial class TableControl : UserControl {
        public TableControl() {
            InitializeComponent();
        }

        private ITable _table;
        public ITable Table {
            get {
                return _table;
            }
            set {
                _table = value;
                lblTable.Text = _table.TableName;
                lv.Items.Clear();
                foreach (IField fld in _table.Mappings) {
                    var lvi = new ListViewItem { Text = fld.TableFieldName, Tag = fld };
                    lvi.Checked = true;
                    lv.Items.Add(lvi);
                }
            }
        }

        private Point _startOffset;
        private void lblTable_MouseDown(object sender, MouseEventArgs e) {
            this.BringToFront();
            _startOffset = this.PointToScreen(new Point { X = e.X - this.Left, Y = e.Y - this.Top });
        }

        private void lblTable_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                var cur = this.PointToScreen(new Point { X = e.X, Y = e.Y });
                var pt = new Point { X = cur.X - _startOffset.X, Y = cur.Y - _startOffset.Y };
                this.Left = pt.X;
                this.Top = pt.Y;
            }
        }

        private void TableControl_MouseDown(object sender, MouseEventArgs e) {
        }

        private void TableControl_Enter(object sender, EventArgs e) {
            this.BringToFront();
        }

        private Point _startOffsetResize;
        private void statusStrip1_MouseDown(object sender, MouseEventArgs e) {
            this.BringToFront();
            _startOffsetResize = this.PointToScreen(new Point { X = e.X - this.Left, Y = e.Y - this.Top });
        }

        private void statusStrip1_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                var cur = this.PointToScreen(new Point { X = e.X, Y = e.Y });
                var pt = new Point { X = cur.X - _startOffsetResize.X, Y = cur.Y - _startOffsetResize.Y };
                this.Width = pt.X;
                this.Height = pt.Y;
            }
        }
    }
}
