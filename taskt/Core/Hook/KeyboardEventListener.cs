using System;
using System.Collections.Generic;
using taskt.Core.Native.Windows;
using static taskt.Core.Native.Windows.HookAPI;

namespace taskt.Core.Hook
{
    /// <summary>
    /// keyboard events listener (hook)
    /// </summary>
    public class KeyboardEventListener: IDisposable
    {
        /// <summary>
        /// hook procedure ids
        /// </summary>
        private List<IntPtr> hookIDs;

        /// <summary>
        /// inner hook procedures body
        /// </summary>
        private List<KeyboardHookActionDelegate> innerActions = new List<KeyboardHookActionDelegate>();

        /// <summary>
        /// hook procedures
        /// </summary>
        private List<LowLevelKeyboardProcDelegate> hookProcs;

        /// <summary>
        /// hook start state
        /// </summary>
        private bool isHookNow = false;

        public KeyboardEventListener(KeyboardHookActionDelegate hookAction) 
        {
            this.AddHookAction(hookAction);
        }

        public KeyboardEventListener(List<KeyboardHookActionDelegate> hookActions)
        {
            this.AddHookAction(hookActions);
        }

        ~KeyboardEventListener()
        {
            StopHook();
        }

        /// <summary>
        /// add hook action
        /// </summary>
        /// <param name="hookAction"></param>
        /// <returns></returns>
        public bool AddHookAction(KeyboardHookActionDelegate hookAction)
        {
            if (!isHookNow)
            {
                this.innerActions.Add(hookAction);
            }
            return !isHookNow;
        }

        /// <summary>
        /// add hook actions
        /// </summary>
        /// <param name="hookActions"></param>
        /// <returns></returns>
        public bool AddHookAction(List<KeyboardHookActionDelegate> hookActions)
        {
            if (!isHookNow)
            {
                this.innerActions.AddRange(hookActions);
            }
            return !isHookNow;
        }

        public void Dispose()
        {
            StopHook();
        }

        /// <summary>
        /// start hook
        /// </summary>
        /// <returns></returns>
        public bool StartHook()
        {
            if (!isHookNow)
            {
                this.hookIDs = new List<IntPtr>();
                this.hookProcs = new List<LowLevelKeyboardProcDelegate>();
                foreach (var act in this.innerActions)
                {
                    var proc = HookAPI.CreateKeyboardHookProcess(act);
                    this.hookProcs.Add(proc);
                    this.hookIDs.Add(HookAPI.SetKeyboardHook(proc));
                }
                isHookNow = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// stop hook
        /// </summary>
        /// <returns></returns>
        public bool StopHook()
        {
            if (isHookNow)
            {
                if (this.hookIDs != null)
                {
                    foreach (var id in this.hookIDs)
                    {
                        HookAPI.RemoveKeyboardMouseHook(id);
                    }
                }
                this.hookIDs = null;
                this.hookProcs = null;
                isHookNow = false;
                return true;
            }
            else
            {
                this.hookIDs = null;
                this.hookProcs = null;
                return false;
            }
        }
    }
}
