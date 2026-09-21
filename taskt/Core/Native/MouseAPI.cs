using System.Runtime.InteropServices;
using static taskt.Core.Native.DEF_POINT;

namespace taskt.Core.Native.Windows
{
    public static class MouseAPI
    {
        /// <summary>
        /// get mouse cursor point
        /// </summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        /// <summary>
        /// get mouse cursor position
        /// </summary>
        /// <returns>(x, y)</returns>
        public static (int, int) GetCursorPosition()
        {
            GetCursorPos(out POINT p);
            return (p.x, p.y);
        }
    }
}
