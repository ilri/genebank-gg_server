using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GrinGlobal.Core {
    public class FreezeWindow : IDisposable {

        public FreezeWindow(IntPtr ptr) {
            try {
                LockWindowUpdate(ptr);
            } catch {
                // eat all errors here, as this is non-essential to functionality of the app (32-bit vs 64-bit may bomb here)
            }
        }

        [DllImport("user32.dll")]
        static extern bool LockWindowUpdate(IntPtr hWndLock);

        public void Dispose() {
            try {
                LockWindowUpdate(IntPtr.Zero);
            } catch {
                // eat all errors here, as this is non-essential to functionality of the app (32-bit vs 64-bit may bomb here)
            }
        }
    }
}
