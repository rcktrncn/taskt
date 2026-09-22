using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Native.Windows
{
    public static class ClipboardAPI
    {
        /// <summary>
        /// get clipboard data in a specified format
        /// </summary>
        /// <param name="uFormat"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        /// <summary>
        /// set clipboard data in a specified format
        /// </summary>
        /// <param name="uFormat"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetClipboardData(uint uFormat, IntPtr data);

        /// <summary>
        /// empty
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();

        /// <summary>
        /// check clipboard contains data in a specified fomat
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint format);

        /// <summary>
        /// open
        /// </summary>
        /// <param name="hWndNewOwner"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// close
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseClipboard();

        /// <summary>
        /// lock
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        /// <summary>
        /// unlock
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// clipboard data format
        /// https://learn.microsoft.com/en-us/windows/win32/dataxchg/standard-clipboard-formats
        /// </summary>
        private const uint CF_UNICODETEXT = 13;

        /// <summary>
        /// set text to clipboard
        /// </summary>
        /// <param name="textToSet"></param>
        public static void SetClipboardText(string textToSet)
        {
            OpenClipboard(IntPtr.Zero);
            EmptyClipboard();
            var ptr = Marshal.StringToHGlobalUni(textToSet);
            SetClipboardData(CF_UNICODETEXT, ptr);
            CloseClipboard();
        }

        /// <summary>
        /// clear clipboard value
        /// </summary>
        public static void ClearClipboard()
        {
            OpenClipboard(IntPtr.Zero);
            EmptyClipboard();
            CloseClipboard();
        }

        /// <summary>
        /// get text from clipboard
        /// </summary>
        /// <returns></returns>
        public static string GetClipboardText()
        {
            if (!IsClipboardFormatAvailable(CF_UNICODETEXT))
            {
                return null;
            }

            if (!OpenClipboard(IntPtr.Zero))
            {
                return null;
            }

            string data = null;
            var hGlobal = GetClipboardData(CF_UNICODETEXT);
            if (hGlobal != IntPtr.Zero)
            {
                var lpwcstr = GlobalLock(hGlobal);
                if (lpwcstr != IntPtr.Zero)
                {
                    data = Marshal.PtrToStringUni(lpwcstr);
                    GlobalUnlock(lpwcstr);
                }
            }
            CloseClipboard();

            return data;
        }
    }
}
