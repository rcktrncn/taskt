using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Native.Windows
{
    public static class GDIAPI
    {
        /// <summary>
        /// get Device Context
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        /// <summary>
        /// release Device Context
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dc"></param>
        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        /// <summary>
        /// get device context
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static IntPtr GetDeviceContext(IntPtr hwnd)
        {
            return GetDC(hwnd);
        }

        /// <summary>
        /// get desktop device context
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetDesktopDeviceContext()
        {
            return GetDC(IntPtr.Zero);
        }

        /// <summary>
        /// release desktop device context
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static int ReleaseDesktopDeviceContext(IntPtr hwnd)
        {
            return ReleaseDC(IntPtr.Zero, hwnd);
        }
    }
}
