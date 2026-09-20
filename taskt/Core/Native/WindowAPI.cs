using System;
using System.Runtime.InteropServices;
using System.Text;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Native.Windows
{
    /// <summary>
    /// Window API methods
    /// </summary>
    static public class WindowAPI
    {
        /// <summary>
        /// set window state
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// set window state async
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// set window is ForeGround
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// window state is normal
        /// </summary>
        private const int NORMAL = 1;

        /// <summary>
        ///  window maximize value
        /// </summary>
        private const int MAXIMIZE = 3;

        /// <summary>
        /// window minimize value
        /// </summary>
        private const int MINIMIZE = 6;

        /// <summary>
        /// window restore value
        /// </summary>
        private const int RESTORE = 9;

        /// <summary>
        /// window states
        /// </summary>
        public enum WindowState
        {
            NORMAL = 1,
            MAXIMIZE = 3,

            MINIMIZE = 6,

            RESTORE = 9,
        }

        /// <summary>
        /// for close window by whnd
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// close value
        /// </summary>
        private static readonly UInt32 WM_CLOSE = 0x0010;

        /// <summary>
        /// get destkop window handle
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// check window handle exists
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// get window title length
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowTextLengthW(IntPtr hWnd);

        /// <summary>
        /// get window title
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextW(IntPtr hWnd, StringBuilder text, int count);

        /// <summary>
        /// check window is minimized
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// check window is maximized
        /// </summary>
        /// <param name="hWhnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWhnd);

        /// <summary>
        /// get active window handle
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// enum window delegate
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        /// <summary>
        /// enum all windows
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        /// <summary>
        /// check window is visible
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// get window rect
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// move or resize window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

        private struct WINDOWPLACEMENT
        {
            uint length;
            uint flags;
            public uint showCmd;
            System.Drawing.Point ptMinPosition;
            System.Drawing.Point ptMaxPosition;
            RECT rcNormalPosition;
            RECT rcDevice;
        }

        /// <summary>
        /// get window state
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpwndpl"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        /// <summary>
        /// set window state
        /// </summary>
        /// <param name="whnd"></param>
        /// <param name="state"></param>
        public static void SetWindowState(IntPtr whnd, WindowState state)
        {
            ShowWindow(whnd, (int)state);
        }

        /// <summary>
        /// set window to normal
        /// </summary>
        /// <param name="whnd"></param>
        public static void SetWindowNormal(IntPtr whnd)
        {
            SetWindowState(whnd, WindowState.NORMAL);
        }

        /// <summary>
        /// set window to miximize
        /// </summary>
        /// <param name="whnd"></param>
        public static void SetWindowMaximize(IntPtr whnd)
        {
            SetWindowState(whnd, WindowState.MAXIMIZE);
        }

        /// <summary>
        /// set window to minimize
        /// </summary>
        /// <param name="whnd"></param>
        public static void SetWindowMinimize(IntPtr whnd)
        {
            SetWindowState(whnd, WindowState.MINIMIZE);
        }

        /// <summary>
        /// set window to restore
        /// </summary>
        /// <param name="whnd"></param>
        public static void SetWindowRestore(IntPtr whnd)
        {
            SetWindowState(whnd, WindowState.RESTORE);
        }

        /// <summary>
        /// check window is minimized
        /// </summary>
        /// <param name="whnd"></param>
        /// <returns></returns>
        public static bool IsWindowMinimized(IntPtr whnd)
        {
            return IsIconic(whnd);
        }

        /// <summary>
        /// activate window
        /// </summary>
        /// <param name="whnd"></param>
        public static void ActivateWindow(IntPtr whnd)
        {
            if (IsWindowMinimized(whnd))
            {
                SetWindowNormal(whnd);
            }
            SetForegroundWindow(whnd);
        }
    }
}
