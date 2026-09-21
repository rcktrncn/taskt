using System.Runtime.InteropServices;

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
        /// point
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// get mouse cursor position
        /// </summary>
        /// <returns>(x, y)</returns>
        public static (int, int) GetCursorPosition()
        {
            GetCursorPos(out POINT p);
            return (p.X, p.Y);
        }
    }
}
