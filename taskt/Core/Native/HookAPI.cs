using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace taskt.Core.Native.Windows
{
    static public class HookAPI
    {
        /// <summary>
        /// callback for hook
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-hookproc
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns>when (nCode < 0) please specify return value of CallNextHookEx</returns>
        private delegate IntPtr HookProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// callback for keyboard input hook
        /// https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelkeyboardproc
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or WM_SYSKEYUP</param>
        /// <param name="lParam">KBDLLHOOKSTRUCT structure
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct</param>
        /// <returns>when (nCode < 0) please specify return value of CallNextHookEx</returns>
        public delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// callback for mouse click hook
        /// https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelmouseproc
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam">WM_LBUTTONDOWN, WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_RBUTTONDOWN, WM_RBUTTONUP, WM_MBUTTONDOWN, WM_MBUTTONUP, WM_XBUTTONDOWN, or WM_XBUTTONUP</param>
        /// <param name="lParam">MSLLHOOKSTRUCT structure
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msllhookstruct</param>
        /// <returns>when (nCode < 0) please specify return value of CallNextHookEx</returns>
        public delegate IntPtr LowLevelMouseProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// low level hook
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn">call back procedure</param>
        /// <param name="hMod"></param>
        /// <param name="dwThreadId"></param>
        /// <returns>hook procedure handle</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProcDelegate lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// low level hook to keyborad
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn">call back procedure</param>
        /// <param name="hMod"></param>
        /// <param name="dwThreadId"></param>
        /// <returns>hook procedure handle</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// low level keyboard input event hook
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// non-system key is pressed
        /// https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown
        /// </summary>
        private const int WM_KEYDOWN = 0x0100;

        /// <summary>
        /// value of win hook low level mouse input event
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </summary>
        private const int WH_MOUSE_LL = 14;

        /// <summary>
        /// low level hook to mouse click
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn">call back procedure</param>
        /// <param name="hMod"></param>
        /// <param name="dwThreadId"></param>
        /// <returns>hook handle</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProcDelegate lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// remove hook
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwindowshookex
        /// </summary>
        /// <param name="hhk">hook handle</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// passes the hook informationt to the next hook procedure
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-callnexthookex
        /// </summary>
        /// <param name="hhk"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// get a module handle for the specified module
        /// https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlea
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

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

        // enums and structs


        /// <summary>
        /// mouse messages
        /// </summary>
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,    // left down
            WM_LBUTTONUP = 0x0202,  // left up
            WM_MOUSEMOVE = 0x0200,  // move
            WM_MOUSEWHEEL = 0x020A, // wheel
            WM_RBUTTONDOWN = 0x0204,    // right down
            WM_RBUTTONUP = 0x0205   // right up
        }

        /// <summary>
        /// point location struct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        /// <summary>
        /// low level keyboard input event information struct
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            /// <summary>
            /// virtual key code
            /// </summary>
            public uint vkCode;
            /// <summary>
            /// hardware scan code
            /// </summary>
            public uint scanCode;
            /// <summary>
            /// extended key flag
            /// </summary>
            public uint flags;
            /// <summary>
            /// timestamp
            /// </summary>
            public uint time;
            /// <summary>
            /// additional infomation
            /// </summary>
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// low level mouse input event information struct
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msllhookstruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            /// <summary>
            /// point x and y
            /// </summary>
            public POINT pt;
            /// <summary>
            /// mouse button message
            /// </summary>
            public uint mouseData;
            /// <summary>
            /// event injected flag
            /// </summary>
            public uint flags;
            /// <summary>
            /// timestamp
            /// </summary>
            public uint time;
            /// <summary>
            /// additional message
            /// </summary>
            public IntPtr dwExtraInfo;
        }

        /// <summary>
        /// set keyboard input hook
        /// </summary>
        /// <param name="proc">call back hook procedure</param>
        /// <returns>hook procedure handle</returns>
        public static IntPtr SetKeyboardHook(LowLevelKeyboardProcDelegate proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                            GetModuleHandle(curModule.ModuleName), 0
                        );
            }
        }

        /// <summary>
        /// set mouse input hook
        /// </summary>
        /// <param name="proc">call back hook procedure</param>
        /// <returns>hook procedure handle</returns>
        public static IntPtr SetMouseHook(LowLevelMouseProcDelegate proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                        GetModuleHandle(curModule.ModuleName), 0
                    );
            }
        }

        /// <summary>
        /// remove keyboard or mouse hook
        /// </summary>
        /// <param name="hhk"></param>
        public static void RemoveKeyboardMouseHook(IntPtr hhk)
        {
            UnhookWindowsHookEx(hhk);
        }
    }
}
