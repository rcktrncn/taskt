using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
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

        // TODO: update to SendInput
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
        private static extern void mouse_event(uint dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        /// <summary>
        /// mouse events
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event
        /// </summary>
        private enum MouseEvents : uint
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
        /// get mouse cursor position
        /// </summary>
        /// <returns>System.Drawing.Point</returns>
        public static System.Drawing.Point GetCursorPositionAsSystemDrawingPoint()
        {
            (var x, var y) = GetCursorPosition();
            return new System.Drawing.Point(x, y);
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
                    //actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
                    //break;
                    SendLeftClick();
                    return;

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
                mouse_event((uint)mouse, xMousePosition, yMousePosition, 0, 0);
            }
        }

        /// <summary>
        /// send mouse actions use InputAPI.SendInput
        /// </summary>
        /// <param name="inputs"></param>
        private static void SendMouseActions(InputAPI.INPUT[] inputs)
        {
            InputAPI.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(InputAPI.INPUT)));
        }

        /// <summary>
        /// create Mouse Inputs for InputAPI.SendInput
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static InputAPI.INPUT[] CreateMouseInputs(MouseEvents e)
        {
            return new InputAPI.INPUT[]
            {
                new InputAPI.INPUT
                {
                     type = InputAPI.InputType.INPUT_MOUSE,
                    U = new InputAPI.InputUnion
                    {
                        mi = new InputAPI.MOUSEINPUT
                        {
                            dwFlags = (uint)e,
                        }
                    }
                }
            };
        }

        /// <summary>
        /// send mouse left down
        /// </summary>
        public static void SendLeftDown()
        {
            SendMouseActions(CreateMouseInputs(MouseEvents.MOUSEEVENTF_LEFTDOWN));
        }

        /// <summary>
        /// send mouse left up
        /// </summary>
        public static void SendLeftUp()
        {
            SendMouseActions(CreateMouseInputs(MouseEvents.MOUSEEVENTF_LEFTUP));
        }

        /// <summary>
        /// send mouse left click
        /// </summary>
        /// <param name="pressDuration">ms</param>
        public static void SendLeftClick(int pressDuration = 50)
        {
            if (pressDuration < 0)
            {
                pressDuration = 50;
            }

            SendLeftDown();
            Thread.Sleep(pressDuration);
            SendLeftUp();
        }
    }
}
