using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrinGlobal.Core {
    public class AutoCursor : IDisposable {

        private Form _form;
        public AutoCursor(Form f) {
            Application.UseWaitCursor = true;
            _form = f;
            if (_form != null) {
                _form.Cursor = Cursors.WaitCursor;
                _form.Refresh();
            }
        }



        #region IDisposable Members

        public void Dispose() {
            if (_form != null) {
                _form.Cursor = Cursors.Default;
            }
            Application.UseWaitCursor = false;
        }

        #endregion
    }
}
