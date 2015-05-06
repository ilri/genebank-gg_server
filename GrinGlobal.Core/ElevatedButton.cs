using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace GrinGlobal.Core {
    public class ElevatedButton : Button {

        public ElevatedButton() {
            FlatStyle = FlatStyle.System;
            try {
                if (!Toolkit.IsProcessElevated()) ShowShield();
            } catch { }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private uint BCM_SETSHIELD = 0x0000160C;


        private void ShowShield() {
            IntPtr wParam = new IntPtr(0);
            IntPtr lParam = new IntPtr(1);
            SendMessage(new HandleRef(this, Handle), BCM_SETSHIELD, wParam, lParam);
        }

    }

}
