using System;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has auto save script file processes
         */

        private void autoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (this.dontSaveFlag)
            {
                // DBG
                //Console.WriteLine("now autosave");

                (var autoSavePath, var saveTime) = Script.GetAutoSaveScriptFilePath();

                // serialize script
                try
                {
                    scriptInfo.TasktVersion = Application.ProductVersion;
                    Core.Script.Script.SerializeScript(lstScriptActions.Items, scriptVariables, scriptInfo, appSettings.EngineSettings, scriptSerializer, autoSavePath);
                    Notify($"Script automatically saved. ({saveTime})");
                }
                catch (Exception ex)
                {
                    Notify($"Auto Save Error: {ex}");
                }
            }
        }

        /// <summary>
        /// set auto save state
        /// </summary>
        private void SetAutoSaveState()
        {
            if (appSettings.ClientSettings.EnabledAutoSave)
            {
                autoSaveTimer.Enabled = this.dontSaveFlag;
                if (this.dontSaveFlag)
                {
                    autoSaveTimer.Interval = appSettings.ClientSettings.AutoSaveInterval * 60000;
                    // DBG
                    //autoSaveTimer.Interval = appSettings.ClientSettings.AutoSaveInterval * 1000;
                    autoSaveTimer.Start();
                }
                else
                {
                    autoSaveTimer.Stop();
                }
            }
        }

        /// <summary>
        /// remove old auto saved files
        /// </summary>
        private void RemoveOldAutoSavedFiles()
        {
            RemoveOldScriptFiles(Core.IO.Folders.GetAutoSaveFolderPath(), appSettings.ClientSettings.RemoveAutoSaveFileDays);
        }

        /// <summary>
        /// remove old run-without-saving script files
        /// </summary>
        private void RemoveOldRunWithoutSavingScriptFiles()
        {
            RemoveOldScriptFiles(Core.IO.Folders.GetRunWithoutSavingFolderPath(), appSettings.ClientSettings.RemoveRunWithtoutSavingFileDays);
        }

        /// <summary>
        /// remove old before-converted script files
        /// </summary>
        private void RemoveOldBeforeConvertedScriptFiles()
        {
            RemoveOldScriptFiles(Core.IO.Folders.GetBeforeConvertedFolderPath(), appSettings.ClientSettings.RemoveBeforeConvertedFileDays);
        }

        /// <summary>
        /// general remove old script files
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="days"></param>
        private void RemoveOldScriptFiles(string folderPath, int days)
        {
            var files = System.IO.Directory.GetFiles(folderPath, "*.xml");
            foreach (var fp in files)
            {
                var info = new System.IO.FileInfo(fp);
                var diff = DateTime.Now - info.CreationTime;
                if (diff.TotalDays >= days)
                {
                    System.IO.File.Delete(fp);
                }
            }
        }
    }
}
