using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static taskt.Core.Native.DEF_POINT;

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
        public delegate IntPtr HookProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

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
        /// keyboard messages for LowLevelKeyboardProc
        /// https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keyup
        /// etc
        /// </summary>
        public enum KeyboardMessages
        {
            WM_KEYDOWN = 0x0100,    // key down
            WM_KEYUP = 0x0101,  // key up
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
        }

        /// <summary>
        /// low level keyboard input event information struct
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-kbdllhookstruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
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
        /// mouse messages
        /// https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-lbuttondown
        /// etc ...
        /// </summary>
        public enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,    // left down
            WM_LBUTTONUP = 0x0202,  // left up
            WM_MOUSEMOVE = 0x0200,  // move
            WM_MOUSEWHEEL = 0x020A, // wheel
            WM_RBUTTONDOWN = 0x0204,    // right down
            WM_RBUTTONUP = 0x0205,   // right up
            WM_XBUTTONDOWN = 0x020b,
            WM_XBUTTONUP = 0x020c,
        }

        /// <summary>
        /// low level mouse input event information struct
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msllhookstruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
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
        /// WinEvents
        /// https://learn.microsoft.com/en-us/windows/win32/winauto/event-constants?redirectedfrom=MSDN
        /// </summary>
        public enum SystemEvents
        {
            EVENT_MIN = 0x00000001,       // MIN
            EVENT_MAX = 0x7FFFFFFF,          // MAX
            EVENT_SYSTEM_FOREGROUND = 0x3,  // The foreground window has changed. The system sends this event even if the foreground window has changed to another window in the same thread. Server applications never send this event.
            MINIMIZE_END = 0x0017, // A window object is about to be restored. This event is sent by the system, never by servers.
            MINIMIZE_START = 0x0016 // A window object is about to be minimized. This event is sent by the system, never by servers.
        }

        /// <summary>
        /// call back for WinEvents
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nc-winuser-wineventproc
        /// </summary>
        /// <param name="hWinEventHook"></param>
        /// <param name="event"></param>
        /// <param name="hwnd"></param>
        /// <param name="idObject"></param>
        /// <param name="idChild"></param>
        /// <param name="dwEventThread"></param>
        /// <param name="dwmsEventTime"></param>
        public delegate void SystemEventHandlerDelegate(IntPtr hWinEventHook, SystemEvents @event, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        /// <summary>
        /// sets an event hook function for a range of events (WinEvents)
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwineventhook
        /// </summary>
        /// <param name="eventMin">hook event lowest value</param>
        /// <param name="eventMax">hook event highest value</param>
        /// <param name="hmodWinEventProc"></param>
        /// <param name="lpfnWinEventProc">callback hook procedure</param>
        /// <param name="idProcess"></param>
        /// <param name="idThread"></param>
        /// <param name="dwFlags"></param>
        /// <returns>event hook instance</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(SystemEvents eventMin, SystemEvents eventMax, IntPtr hmodWinEventProc, SystemEventHandlerDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        /// <summary>
        /// remove event hook function created by SetWinEventHook (WinEvents)
        /// https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwinevent
        /// </summary>
        /// <param name="hWinEventHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

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

        /// <summary>
        /// set window event hook
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public static IntPtr SetWindowHook(SystemEventHandlerDelegate ev)
        {
            return SetWinEventHook(SystemEvents.EVENT_MIN, SystemEvents.EVENT_MAX, IntPtr.Zero, ev, 0, 0, 0);
        }

        /// <summary>
        /// remove window event hook
        /// </summary>
        /// <param name="hhk"></param>
        public static void RemoveWindowHook(IntPtr hhk)
        {
            UnhookWindowsHookEx(hhk);
        }

        /// <summary>
        /// create keyboad hook procedure
        /// </summary>
        /// <param name="hookId"></param>
        /// <returns></returns>
        public static LowLevelKeyboardProcDelegate CreateKeyboadHookProcess(IntPtr hookId, Action<int, KeyboardMessages, KBDLLHOOKSTRUCT> hookAction)
        {
            LowLevelKeyboardProcDelegate ret = (nCode, wParam, lParam) =>
            {
                var keyboadMessage = (KeyboardMessages)wParam;
                var hookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                hookAction(nCode, keyboadMessage, hookStruct);

                return CallNextHookEx(hookId, nCode, wParam, lParam);
            };
            return ret;
        }
    }
}
