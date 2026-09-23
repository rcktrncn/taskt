using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static taskt.Core.Native.Windows.HookAPI;

namespace taskt.Core.Native.Windows
{
    public static class KeyboardAPI
    {
        /// <summary>
        /// get the status of the specified virtual key (up, down)
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getkeystate
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        /// <summary>
        /// convert virtual-key code and keystate to unicode
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-tounicode
        /// </summary>
        /// <param name="virtualKeyCode">virtual keycode to be translated</param>
        /// <param name="scanCode">hardware scancode to be translated</param>
        /// <param name="keyboardState">265 byte array</param>
        /// <param name="receivingBuffer">translated character UTF-16</param>
        /// <param name="bufferSize">receivingBuffer size</param>
        /// <param name="flags">behavior of function</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, StringBuilder receivingBuffer, int bufferSize, uint flags);

        /// <summary>
        /// key states
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.keystates?view=windowsdesktop-10.0
        /// </summary>
        [Flags]
        private enum KeyStates
        {
            /// <summary>
            /// not pressed
            /// </summary>
            None = 0,
            /// <summary>
            /// pressed
            /// </summary>
            Down = 1,
            /// <summary>
            /// toggled
            /// </summary>
            Toggled = 2
        }

        /// <summary>
        /// get key states
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static KeyStates GetKeyStates(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            // If the high-order bit is 1, the key is down
            // otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
            {
                state |= KeyStates.Down;
            }

            // If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
            {
                state |= KeyStates.Toggled;
            }

            return state;
        }

        /// <summary>
        /// check keystate is down
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key)
        {
            return KeyStates.Down == (GetKeyStates(key) & KeyStates.Down);
        }

        /// <summary>
        /// check keystate is toggled
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyToggled(Keys key)
        {
            return KeyStates.Toggled == (GetKeyStates(key) & KeyStates.Toggled);
        }

        /// <summary>
        /// check current keyboard state is UpperCase
        /// </summary>
        /// <returns></returns>
        public static bool IsUpperCase()
        {
            bool toUpperCase = false;

            // determine if casing is needed
            if (IsKeyDown(Keys.ShiftKey) && IsKeyToggled(Keys.Capital))
            {
                toUpperCase = false;
            }
            else if (!IsKeyDown(Keys.ShiftKey) && IsKeyToggled(Keys.Capital))
            {
                toUpperCase = true;
            }
            else if (IsKeyDown(Keys.ShiftKey) && !IsKeyToggled(Keys.Capital))
            {
                toUpperCase = true;
            }
            else if (!IsKeyDown(Keys.ShiftKey) && !IsKeyToggled(Keys.Capital))
            {
                toUpperCase = false;
            }

            return toUpperCase;
        }

        /// <summary>
        /// convert Virtual-Key to string
        /// </summary>
        /// <param name="kbd"></param>
        /// <param name="toUpperCase"></param>
        /// <returns></returns>
        public static string ConvertVirtualKeyToString(KBDLLHOOKSTRUCT kbd)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];

            if (IsUpperCase())
            {
                keyboardState[(int)Keys.ShiftKey] = 0xff;
            }

            var key = (Keys)kbd.vkCode;
            ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);

            return buf.ToString();
        }
    }
}
