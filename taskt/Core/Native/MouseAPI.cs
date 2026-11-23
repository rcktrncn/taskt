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
        public static void SendMouseClick(string clickType)
        {
            switch (clickType.ToLower())
            {
                case "double left click":
                    SendLeftDoubleClick();
                    return;

                case "double middle click":
                    SendMiddleDoubleClick();
                    return;

                case "double right click":
                    SendRightDoubleClick();
                    return;

                case "left click":
                    SendLeftClick();
                    return;

                case "right click":
                    SendRightClick();
                    return;

                case "middle click":
                    SendMiddleClick();
                    return;

                case "left down":
                    SendLeftDown();
                    return;

                case "right down":
                    SendRightDown();
                    return;

                case "middle down":
                    SendMiddleDown();
                    return;

                case "left up":
                    SendLeftUp();
                    return;

                case "right up":
                    SendRightUp();
                    return;

                case "middle up":
                    SendMiddleUp();
                    return;

                default:
                    break;
            }
        }

        /// <summary>
        /// send mouse click
        /// </summary>
        /// <param name="clickType"></param>
        /// <param name="xMousePosition"></param>
        /// <param name="yMousePosition"></param>
        //public static void SendMouseClick(string clickType, int xMousePosition, int yMousePosition)
        //{
        //    var actions = new List<MouseEvents>();

        //    switch (clickType.ToLower())
        //    {
        //        case "double left click":
        //            actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP, MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
        //            break;
        //            SendLeftDoubleClick();
        //            return;

        //        case "left click":
        //            actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_LEFTDOWN, MouseEvents.MOUSEEVENTF_LEFTUP });
        //            break;
        //            SendLeftClick();
        //            return;

        //        case "right click":
        //            actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_RIGHTDOWN, MouseEvents.MOUSEEVENTF_RIGHTUP });
        //            break;
        //            SendRightClick();
        //            return;

        //        case "middle click":
        //            actions.AddRange(new MouseEvents[] { MouseEvents.MOUSEEVENTF_MIDDLEDOWN, MouseEvents.MOUSEEVENTF_MIDDLEUP });
        //            break;
        //            SendMiddleClick();
        //            return;

        //        case "left down":
        //            actions.Add(MouseEvents.MOUSEEVENTF_LEFTDOWN);
        //            break;
        //            SendLeftDown();
        //            return;

        //        case "right down":
        //            actions.Add(MouseEvents.MOUSEEVENTF_RIGHTDOWN);
        //            break;
        //            SendRightDown();
        //            return;

        //        case "middle down":
        //            actions.Add(MouseEvents.MOUSEEVENTF_MIDDLEDOWN);
        //            break;
        //            SendMiddleDown();
        //            return;

        //        case "left up":
        //            actions.Add(MouseEvents.MOUSEEVENTF_LEFTUP);
        //            break;
        //            SendLeftUp();
        //            return;

        //        case "right up":
        //            actions.Add(MouseEvents.MOUSEEVENTF_RIGHTUP);
        //            break;
        //            SendRightUp();
        //            return;

        //        case "middle up":
        //            actions.Add(MouseEvents.MOUSEEVENTF_MIDDLEUP);
        //            break;
        //            SendMiddleUp();
        //            return;

        //        default:
        //            break;
        //    }

        //    foreach (var mouse in actions)
        //    {
        //        mouse_event((uint)mouse, xMousePosition, yMousePosition, 0, 0);
        //    }
        //}

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

        /// <summary>
        /// send mouse right down
        /// </summary>
        public static void SendRightDown()
        {
            SendMouseActions(CreateMouseInputs(MouseEvents.MOUSEEVENTF_RIGHTDOWN));
        }

        /// <summary>
        /// send mouse right up
        /// </summary>
        public static void SendRightUp()
        {
            SendMouseActions(CreateMouseInputs(MouseEvents.MOUSEEVENTF_RIGHTUP));
        }

        /// <summary>
        /// send mouse right click
        /// </summary>
        /// <param name="pressDuration">ms</param>
        public static void SendRightClick(int pressDuration = 50)
        {
            if (pressDuration < 0)
            {
                pressDuration = 50;
            }

            SendRightDown();
            Thread.Sleep(pressDuration);
            SendRightUp();
        }

        /// <summary>
        /// send mouse middle down
        /// </summary>
        public static void SendMiddleDown()
        {
            SendMouseActions(CreateMouseInputs(MouseEvents.MOUSEEVENTF_MIDDLEDOWN));
        }

        /// <summary>
        /// send mouse middle up
        /// </summary>
        public static void SendMiddleUp()
        {
            SendMouseActions(CreateMouseInputs(MouseEvents.MOUSEEVENTF_MIDDLEUP));
        }

        /// <summary>
        /// send mouse middle click
        /// </summary>
        /// <param name="pressDuration">ms</param>
        public static void SendMiddleClick(int pressDuration = 50)
        {
            if (pressDuration < 0)
            {
                pressDuration = 50;
            }

            SendMiddleDown();
            Thread.Sleep(pressDuration);
            SendMiddleUp();
        }

        /// <summary>
        /// send left double-click
        /// </summary>
        /// <param name="pressDuration"></param>
        /// <param name="intervalSecondClick"></param>
        public static void SendLeftDoubleClick(int pressDuration = 50, int intervalSecondClick = 100)
        {
            if (pressDuration < 0)
            {
                pressDuration = 50;
            }
            if (intervalSecondClick < 0)
            {
                intervalSecondClick = 50;
            }

            SendLeftClick(pressDuration);
            Thread.Sleep(intervalSecondClick);
            SendLeftClick(pressDuration);
        }

        /// <summary>
        /// send right double-click
        /// </summary>
        /// <param name="pressDuration"></param>
        /// <param name="intervalSecondClick"></param>
        public static void SendRightDoubleClick(int pressDuration = 50, int intervalSecondClick = 100)
        {
            if (pressDuration < 0)
            {
                pressDuration = 50;
            }
            if (intervalSecondClick < 0)
            {
                intervalSecondClick = 50;
            }

            SendRightClick(pressDuration);
            Thread.Sleep(intervalSecondClick);
            SendRightClick(pressDuration);
        }

        /// <summary>
        /// send middle double-click
        /// </summary>
        /// <param name="pressDuration"></param>
        /// <param name="intervalSecondClick"></param>
        public static void SendMiddleDoubleClick(int pressDuration = 50, int intervalSecondClick = 100)
        {
            if (pressDuration < 0)
            {
                pressDuration = 50;
            }
            if (intervalSecondClick < 0)
            {
                intervalSecondClick = 50;
            }

            SendMiddleClick(pressDuration);
            Thread.Sleep(intervalSecondClick);
            SendMiddleClick(pressDuration);
        }
    }
}
