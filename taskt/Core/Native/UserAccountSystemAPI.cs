using System.Runtime.InteropServices;

namespace taskt.Core.Native.Windows
{
    public static class UserAccountSystemAPI
    {
        /// <summary>
        /// lock user account
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool LockWorkStation();

        /// <summary>
        /// poweroff, reboot
        /// </summary>
        /// <param name="uFlags"></param>
        /// <param name="dwReason"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        /// <summary>
        /// logoff
        /// </summary>
        private const int EWX_LOGOFF = 0;

        /// <summary>
        /// shutdown
        /// </summary>
        private const int EWX_SHUTDOWN = 1;

        /// <summary>
        /// reboot
        /// </summary>
        private const int EWX_REBOOT = 2;

        /// <summary>
        /// user account lock
        /// </summary>
        public static void LockUserAccount()
        {
            LockWorkStation();
        }

        /// <summary>
        /// logoff user account
        /// </summary>
        /// <returns></returns>
        public static bool LogOffUserAccount()
        {
            return ExitWindowsEx(EWX_LOGOFF, 0);
        }
    }
}
