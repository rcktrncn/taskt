using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Native.Windows
{
    public static class InputAPI
    {
        /// <summary>
        /// input type for INPUT.type
        /// </summary>
        public enum InputType : uint
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        /// <summary>
        /// input struct
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public InputType type;
            public InputUnion U;
        }

        /// <summary>
        /// input union
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            // HARDWAREINPUT here
        }

        /// <summary>
        /// keyborad input
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// mouse input
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-mouseinput
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// send keyboard/mouse input
        /// </summary>
        /// <param name="nInputs"></param>
        /// <param name="pInputs"></param>
        /// <param name="cbSize"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);
    }
}
