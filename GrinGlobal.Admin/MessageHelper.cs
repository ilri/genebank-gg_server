using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
namespace GrinGlobal.Admin {
    public class MessageHelper {
        [DllImport("User32.dll")]
        private static extern int RegisterWindowMessage(string lpString);

        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(int hWnd);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam);

        public const int WM_COPYDATA = 0x4A;
        public const int WM_USER = 0x400;

        //Used for WM_COPYDATA for string messages
        private struct COPYDATASTRUCT {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public static string ReceivedWindowsStringMessage(Message m) {
            COPYDATASTRUCT mystr = new COPYDATASTRUCT();
            Type mytype = mystr.GetType();
            mystr = (COPYDATASTRUCT)m.GetLParam(mytype);
            return mystr.lpData;
        }

        public static int SendAck(IntPtr toWindow, IntPtr fromWindow) {
            int rv = 0;
            if (toWindow != IntPtr.Zero) {
                rv = SendMessage(toWindow, WM_USER, fromWindow, 1);
            }
            return rv;
        }

        public static int SendWindowsStringMessage(IntPtr toWindow, IntPtr fromWindow, string msg) {
            int result = 0;

            if (toWindow != IntPtr.Zero){
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = len + 1;
                result = SendMessage(toWindow, WM_COPYDATA, fromWindow, ref cds);
            }

            return result;
        }
    }

}