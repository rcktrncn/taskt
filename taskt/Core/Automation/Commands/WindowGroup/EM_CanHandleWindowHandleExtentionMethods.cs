namespace taskt.Core.Automation.Commands
{
    public class EM_CanHandleWindowHandleExtentionMethods
    {
        /// <summary>
        /// check text is current window handle keyword
        /// </summary>
        /// <param name="str"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool IsCurrentWindowHandleKeyword(string str, Engine.AutomationEngineInstance engine)
        {
            return (str == VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowHandle.VariableName, engine));
        }
    }
}
