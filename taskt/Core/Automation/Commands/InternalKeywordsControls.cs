namespace taskt.Core.Automation.Commands
{
    public static class InternalKeywordsControls
    {
        /// <summary>
        /// default webbrowser instance keyword
        /// </summary>
        public const string INTERNAL_DEFAULT_WEBBROWSER_INSTANCE_NAME_KEYWORD = "%kwd_default_browser_instance%";
        /// <summary>
        /// default stopwatch instance keyword
        /// </summary>
        public const string INTERNAL_DEFAULT_STOPWATCH_INSTANCE_NAME_KEYWORD = "%kwd_default_stopwatch_instance%";
        /// <summary>
        /// default excel instance keyword
        /// </summary>
        public const string INTERNAL_DEFAULT_EXCEL_INSTANCE_NAME_KEYWORD = "%kwd_default_excel_instance%";
        /// <summary>
        /// default word instance keyword
        /// </summary>
        public const string INTERNAL_DEFAULT_WORD_INSTANCE_NAME_KEYWORD = "%kwd_default_word_instance%";
        /// <summary>
        /// default db instance keyword
        /// </summary>
        public const string INTERNAL_DEFAULT_DB_INSTANCE_NAME_KEYWORD = "%kwd_default_db_instance%";
        /// <summary>
        /// default nlg instance keyword
        /// </summary>
        public const string INTERNAL_DEFAULT_NLG_INSTANCE_NAME_KEYWORD = "%kwd_default_nlg_instance%";

        /// <summary>
        /// replace keyword to default instance name
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToInstanceName(string txt, IApplicationSettings settings)
        {
            return txt.Replace(INTERNAL_DEFAULT_WEBBROWSER_INSTANCE_NAME_KEYWORD, settings.ClientSettings.DefaultBrowserInstanceName)
                    .Replace(INTERNAL_DEFAULT_STOPWATCH_INSTANCE_NAME_KEYWORD, settings.ClientSettings.DefaultStopWatchInstanceName)
                    .Replace(INTERNAL_DEFAULT_EXCEL_INSTANCE_NAME_KEYWORD, settings.ClientSettings.DefaultExcelInstanceName)
                    .Replace(INTERNAL_DEFAULT_WORD_INSTANCE_NAME_KEYWORD, settings.ClientSettings.DefaultWordInstanceName)
                    .Replace(INTERNAL_DEFAULT_DB_INSTANCE_NAME_KEYWORD, settings.ClientSettings.DefaultDBInstanceName)
                    .Replace(INTERNAL_DEFAULT_NLG_INSTANCE_NAME_KEYWORD, settings.ClientSettings.DefaultNLGInstanceName);
        }

        /// <summary>
        /// Replace Internal Keywords to SystemVariable Names (not support Variable Marker)
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
        /// Replace Internal Keywords to SystemVariable Names & Instance Name
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string ReplaceKeywordsToSystemVariableAndInstanceName(string txt, IApplicationSettings settings)
        {
            txt = ReplaceKeywordsToInstanceName(txt, settings);
            txt = VariableNameControls.ReplaceKeywordsToSystemVariable(txt, settings);
            txt = WindowControls.ReplaceKeywordsToSystemVariable(txt, settings);
            txt = ExcelControls.ReplaceKeywordsToSystemVariable(txt, settings);
            return txt;
        }
    }
}
