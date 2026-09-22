using System;

namespace taskt.Core.Native.Windows
{
    public static class GUIAPI
    {
        /// <summary>
        /// support high dpi
        /// </summary>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// apply other visual style to window/control
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="pszSubAppName"></param>
        /// <param name="pszSubIdList"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("uxtheme.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        /// <summary>
        /// support high dpi
        /// </summary>
        public static void SupportHighDPI()
        {
            SetProcessDPIAware();
        }

        /// <summary>
        /// apply default theme style
        /// </summary>
        /// <param name="hwnd"></param>
        public static void ApplyDefaultThemeStyle(IntPtr hwnd)
        {
            SetWindowTheme(hwnd, "Explorer", null);
        }
    }
}
