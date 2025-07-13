namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has show folder processes
         */

        private void ShowScriptFolderProcess()
        {
            System.Diagnostics.Process.Start(appSettings.ClientSettings.RootFolder + "\\My Scripts");
        }

        private void ShowLogFolderProcess()
        {
            System.Diagnostics.Process.Start(appSettings.ClientSettings.RootFolder + "\\Logs");
        }

    }
}
