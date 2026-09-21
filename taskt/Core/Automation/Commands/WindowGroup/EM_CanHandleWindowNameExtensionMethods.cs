namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleWindowNameExtensionMethods
    {
        /// <summary>
        /// check text is current window name keyword
        /// </summary>
        /// <param name="str"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsCurrentWindowNameKeyword(string str, Engine.AutomationEngineInstance engine)
        {
            return (str == VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, engine));
        }
    }
}
