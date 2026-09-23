using System;
using taskt.Core.Automation.Engine;
using taskt.Core.Native.Windows;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for IWindowSizeProperties Extension methods
    /// </summary>
    public static class EM_WindowSizePropertiesExtensionMethods
    {
        /// <summary>
        /// expand value or variable as Window Width
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrVariableAsWindowWidth(this IWindowSizeProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            var w = command.v_Width;
            //(var width, var height) = GetWindowSize(whnd);
            (var width, var height) = WindowAPI.GetWindowSize(whnd);

            if ((w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentSize.VariableName, engine)) ||
                 (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWidth.VariableName, engine)))
            {
                return width;
            }
            else if (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentHeight.VariableName, engine))
            {
                return height;
            }
            else
            {
                return w.ExpandValueOrUserVariableAsInteger("Window Width", engine);
            }
        }

        /// <summary>
        /// expand value or variable as Window Height
        /// </summary>
        /// <param name="command"></param>
        /// <param name="whnd"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static int ExpandValueOrVariableAsWindowHeight(this IWindowSizeProperties command, IntPtr whnd, AutomationEngineInstance engine)
        {
            var w = command.v_Height;
            //(var width, var height) = GetWindowSize(whnd);
            (var width, var height) = WindowAPI.GetWindowSize(whnd);

            if ((w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentSize.VariableName, engine)) ||
                 (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentHeight.VariableName, engine)))
            {
                return height;
            }
            else if (w == VariableNameControls.GetWrappedVariableName(SystemVariables.Window_CurrentWidth.VariableName, engine))
            {
                return width;
            }
            else
            {
                return w.ExpandValueOrUserVariableAsInteger("Window Height", engine);
            }
        }
    }
}
