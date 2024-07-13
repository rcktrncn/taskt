namespace taskt.Core.Automation.Commands
{
    public static class InternalKeywordsControls
    {
        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariable(string txt, Engine.AutomationEngineInstance engine)
        {
            txt = VariableNameControls.ReplaceKeywordsToSystemVariable(txt, engine);
            txt = WindowControls.ReplaceKeywordsToSystemVariable(txt, engine);
            txt = ExcelControls.ReplaceKeywordsToSystemVariable(txt, engine);
            return txt;
        }

        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariable(string txt, ApplicationSettings settings)
        {
            txt = VariableNameControls.ReplaceKeywordsToSystemVariable(txt, settings);
            txt = WindowControls.ReplaceKeywordsToSystemVariable(txt, settings);
            txt = ExcelControls.ReplaceKeywordsToSystemVariable(txt, settings);
            return txt;
        }
    }
}
