using System;
using System.Collections.Generic;
using taskt.Core.Native.Windows;
using static taskt.Core.Native.Windows.HookAPI;

namespace taskt.Core.Hook
{
    /// <summary>
    /// UI events listener (hook)
    /// </summary>
    public class UIEventListner: IDisposable
    {
        /// <summary>
        /// hook procedure ids
        /// </summary>
        private List<IntPtr> hookIDs;

        /// <summary>
        /// inner hook procedures body
        /// </summary>
        private List<SystemEventHandlerDelegate> innerActions = new List<SystemEventHandlerDelegate>();

        /// <summary>
        /// hook start state
        /// </summary>
        private bool isHookNow = false;

        public UIEventListner(SystemEventHandlerDelegate hookAction) 
        {
            this.AddHookAction(hookAction);
        }

        public UIEventListner(List<SystemEventHandlerDelegate> hookActions)
        {
            this.AddHookAction(hookActions);
        }

        ~UIEventListner()
        {
            StopHook();
        }

        /// <summary>
        /// add hook action
        /// </summary>
        /// <param name="hookAction"></param>
        /// <returns></returns>
        public bool AddHookAction(SystemEventHandlerDelegate hookAction)
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
        public bool AddHookAction(List<SystemEventHandlerDelegate> hookActions)
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
                foreach (var act in this.innerActions)
                {
                    var proc = HookAPI.SetSomeUIEventsHook(act);
                    this.hookIDs.Add(proc);
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
                isHookNow = false;
                return true;
            }
            else
            {
                this.hookIDs = null;
                return false;
            }
        }
    }
}
