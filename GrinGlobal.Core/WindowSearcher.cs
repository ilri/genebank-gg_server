using System.Runtime.InteropServices;
using System.Text;
using System;
namespace GrinGlobal.Core {
    public class WindowSearcher {
        public static IntPtr SearchForWindow(string wndclass, string title, IntPtr notThisHandle) {
            SearchData sd = new SearchData { Wndclass = wndclass, Title = title, NotThishWnd = notThisHandle };
            EnumWindows(new EnumWindowsProc(EnumProc), ref sd);
            return sd.hWnd;
        }

        public static bool EnumProc(IntPtr hWnd, ref SearchData data) {
            // Check classname and title 
            // This is different from FindWindow() in that the code below allows partial matches
            StringBuilder sb = new StringBuilder(1024);
            GetClassName(hWnd, sb, sb.Capacity);
            if (sb.ToString().StartsWith(data.Wndclass)) {
                sb = new StringBuilder(1024);
                GetWindowText(hWnd, sb, sb.Capacity);
                if (sb.ToString().StartsWith(data.Title) && data.NotThishWnd.ToInt64() != hWnd.ToInt64()) {
                    data.hWnd = hWnd;
                    return false;    // Found the wnd, halt enumeration
                }
            }
            return true;
        }

        public class SearchData {
            // You can put any vars in here...
            public string Wndclass;
            public string Title;
            public IntPtr hWnd;
            public IntPtr NotThishWnd;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}