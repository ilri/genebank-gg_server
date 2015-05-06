using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrinGlobal.Core;
using System.Threading;

namespace GrinGlobal.InstallHelper {
    public partial class frmSplash : Form, IDisposable {
        public frmSplash() {
            InitializeComponent();
        }

        public frmSplash(string labelText, bool showCancelButton, IWin32Window owner, bool disableOwner) :this() {
            Show(labelText, showCancelButton, owner, disableOwner);
        }

        public frmSplash(string labelText, bool showCancelButton, IWin32Window owner) 
            : this(labelText, showCancelButton, owner, false) {
        }

        private bool _disableOwner;
        public bool DisableOwner {
            get {
                return _disableOwner;
            }
            set {
                _disableOwner = value;
                if (_disableOwner && Owner != null) {
                    Owner.Enabled = false;
                }
            }
        }

        private BackgroundWorker _worker;
        public BackgroundWorker Worker {
            get {
                return _worker;
            }
            set {
                _worker = value;
                btnCancel.Visible = _worker != null;
                Application.DoEvents();
            }
        }

        public bool UserCancelled { get; private set; }

        private delegate void formShower(string labelText, bool showCancelButton, IWin32Window owner);

        public void Show(string labelText, bool showCancelButton, IWin32Window owner, bool disableOwner) {
            showForm(labelText, showCancelButton, owner, disableOwner);
        }
        public void Show(string labelText, bool showCancelButton, IWin32Window owner) {
            Show(labelText, showCancelButton, owner, false);
            //if (Thread.CurrentThread.IsBackground) {
            //    var result = this.BeginInvoke(new formShower(showForm), labelText, showCancelButton, owner);
            //    while (!result.IsCompleted) {
            //        Thread.Sleep(50);
            //        Application.DoEvents();
            //    }
            //    this.EndInvoke(result);
            //} else {
            //}
        }

        private void showForm(string labelText, bool showCancelButton, IWin32Window owner, bool disableOwner){
            lblText.Text = labelText;
            btnCancel.Visible = showCancelButton && _worker != null && _worker.WorkerSupportsCancellation;
            this.Cursor = Cursors.WaitCursor;
            if (owner != null) {
                this.Show(owner);
            } else {
                this.Show();
            }
            DisableOwner = disableOwner;

            this.Refresh();
            Application.DoEvents();
            Toolkit.ActivateApplication(this.Handle);
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 400;
            timer1.Enabled = true;
        }

        void timer1_Tick(object sender, EventArgs e) {
            var i = progressBar1.Value + progressBar1.Step;
            if (i > progressBar1.Maximum) {
                i = progressBar1.Minimum;
            }
            progressBar1.Value = i;
            this.Refresh();
            Application.DoEvents();
        }

        private delegate void labelTextChanger(string newLabelText);


        public void ChangeTitle(string newTitle) {
            if (!this.Visible) {
                this.Show(lblText.Text, false, null);
            }
            changeTitle(newTitle);
        }

        public void ChangeText(string newLabelText) {
            if (!this.Visible) {
                this.Show(newLabelText, false, null);
            } else {
                //if (Thread.CurrentThread.IsBackground) {
                //    var result = this.BeginInvoke(new labelTextChanger(changeLabelText), newLabelText);
                //    while (!result.IsCompleted) {
                //        Thread.Sleep(50);
                //        Application.DoEvents();
                //    }
                //    this.EndInvoke(result);
                //} else {
                changeLabelText(newLabelText);
                //}
            }
        }

        private void changeLabelText(string newLabelText) {
            lblText.Text = newLabelText;
            if (UserCancelled && btnCancel.Enabled ) {
                btnCancel.Enabled = false;
            }
            lblText.Refresh();
            this.Refresh();
            //Application.DoEvents();
        }

        private void changeTitle(string newTitle) {
            this.Text = newTitle;
            this.Refresh();
            Application.DoEvents();
        }

        private void frmSplash_Load(object sender, EventArgs e) {

        }

        private void lblText_Click(object sender, EventArgs e) {

        }



        #region IDisposable Members

        void IDisposable.Dispose() {
            enableOwnerIfNeeded();
            this.Close();
        }

        #endregion

        private void enableOwnerIfNeeded() {
            if (Owner != null) {
                Owner.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (_worker.WorkerSupportsCancellation) {
                UserCancelled = true;
                changeLabelText("Cancelling...");
                _worker.CancelAsync();
            }
        }

        private void frmSplash_FormClosing(object sender, FormClosingEventArgs e) {
            enableOwnerIfNeeded();
        }

        private void frmSplash_FormClosed(object sender, FormClosedEventArgs e) {
            enableOwnerIfNeeded();
        }
    }
}
