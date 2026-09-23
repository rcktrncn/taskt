using System;
using System.Collections.Generic;
using taskt.Core.Native.Windows;
using static taskt.Core.Native.Windows.HookAPI;

namespace taskt.Core.Hook
{
    /// <summary>
    /// mouse events listener (hook)
    /// </summary>
    public class MouseEventListener: IDisposable
    {
        /// <summary>
        /// hook procedure ids
        /// </summary>
        private List<IntPtr> hookIDs;

        /// <summary>
        /// inner hook procedures body
        /// </summary>
        private List<MouseHookActionDelegate> innerActions = new List<MouseHookActionDelegate>();

        /// <summary>
        /// hook procedures
        /// </summary>
        private List<LowLevelMouseProcDelegate> hookProcs;

        /// <summary>
        /// hook start state
        /// </summary>
        private bool isHookNow = false;

        public MouseEventListener(MouseHookActionDelegate hookAction) 
        {
            this.AddHookAction(hookAction);
        }

        public MouseEventListener(List<MouseHookActionDelegate> hookActions)
        {
            this.AddHookAction(hookActions);
        }

        ~MouseEventListener()
        {
            StopHook();
        }

        /// <summary>
        /// add hook action
        /// </summary>
        /// <param name="hookAction"></param>
        /// <returns></returns>
        public bool AddHookAction(MouseHookActionDelegate hookAction)
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
        public bool AddHookAction(List<MouseHookActionDelegate> hookActions)
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
                this.hookProcs = new List<LowLevelMouseProcDelegate>();
                foreach (var act in this.innerActions)
                {
                    var proc = HookAPI.CreateMouseHookProcess(act);
                    this.hookProcs.Add(proc);
                    this.hookIDs.Add(HookAPI.SetMouseHook(proc));
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
                foreach(var id in this.hookIDs)
                {
                    HookAPI.RemoveKeyboardMouseHook(id);
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
