using System.Collections.Generic;
using System.Runtime.InteropServices;
using static taskt.Core.Native.DEF_POINT;

namespace taskt.Core.Native.Windows
{
    public static class MouseAPI
    {
        /// <summary>
        /// get mouse cursor position
        /// </summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        /// <summary>
        /// set mouse cursoro position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        /// <summary>
        /// execute mouse event (old api)
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="cButtons"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        /// <summary>
        /// mouse events
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event
        /// </summary>
        private enum MouseEvents
        {
            MOUSEEVENTF_LEFTDOWN = 0x02,
            MOUSEEVENTF_LEFTUP = 0x04,
            MOUSEEVENTF_RIGHTDOWN = 0x08,
            MOUSEEVENTF_RIGHTUP = 0x10,
            MOUSEEVENTF_MIDDLEDOWN = 0x20,
            MOUSEEVENTF_MIDDLEUP = 0x40
        }

        /// <summary>
        /// get mouse cursor position
        /// </summary>
        /// <returns>(x, y)</returns>
        public static (int, int) GetCursorPosition()
        {
            GetCursorPos(out POINT p);
            return (p.x, p.y);
        }

        /// <summary>
        /// set mouse cursor position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool SetCursorPosition(int x, int y)
        {
            return SetCursorPos(x, y);
        }

        /// <summary>
        /// send mouse click
        /// </summary>
        /// <param name="clickType"></param>
        /// <param name="xMousePosition"></param>
        /// <param name="yMousePosition"></param>
        public static void SendMouseClick(string clickType, int xMousePosition, int yMousePosition)
        {
            var actions = new List<MouseEvents>();

            switch (clickType.ToLower())
            {
                case "double left click":
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP, MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
                    break;

                case "left click":
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
                    break;

                case "right click":
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_RIGHTDOWN, MouseEvents.MOUSEEVENTF_RIGHTUP });
                    break;

                case "middle click":
                    actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_MIDDLEDOWN, MouseEvents.MOUSEEVENTF_MIDDLEUP });
                    break;

                case "left down":
                    actions.Add(MouseEvents.MOUSEEVENTF_LEFTDOWN);
                    break;

                case "right down":
                    actions.Add(MouseEvents.MOUSEEVENTF_RIGHTDOWN);
                    break;

                case "middle down":
                    actions.Add(MouseEvents.MOUSEEVENTF_MIDDLEDOWN);
                    break;

                case "left up":
                    actions.Add(MouseEvents.MOUSEEVENTF_LEFTUP);
                    break;

                case "right up":
                    actions.Add(MouseEvents.MOUSEEVENTF_RIGHTUP);
                    break;

                case "middle up":
                    actions.Add(MouseEvents.MOUSEEVENTF_MIDDLEUP);
                    break;

                default:
                    break;
            }

            foreach (var mouse in actions)
            {
                mouse_event((int)mouse, xMousePosition, yMousePosition, 0, 0);
            }
        }
    }
}
