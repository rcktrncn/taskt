using System.Linq;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has supplimental form process
         */

        private void ShowVariableManager()
        {
            using (var scriptVariableEditor = new Supplemental.frmScriptVariables(this.scriptVariables, this.appSettings))
            {
                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    CreateUndoSnapshot();
                    this.scriptVariables = scriptVariableEditor.scriptVariables.OrderBy(v => v.VariableName).ToList();
                    ChangeSaveState(true);
                }
            }
        }

        private void ShowSettingForm()
        {
            // show settings dialog
            using (var newSettings = new Supplemental.frmSettings(this))
            {
                newSettings.ShowDialog();

                // reload app settings
                //appSettings = new Core.ApplicationSettings();
                //appSettings = appSettings.GetOrCreateApplicationSettings();
                //appSettings = Core.ApplicationSettings.GetOrCreateApplicationSettings(App.Taskt_Settings_File_Path);
                appSettings = App.GetFrmScriptBuilderApplicationSettings();

                // reinit
                Core.Server.HttpServerClient.Initialize();
            }
        }

        private void ShowNewSettingForm()
        {
            using (var newSettings = new Supplemental.frmNewSettings(this))
            {
                newSettings.ShowDialog();

                // reload app settings
                //appSettings = new Core.ApplicationSettings();
                //appSettings = appSettings.GetOrCreateApplicationSettings();
                //appSettings = Core.ApplicationSettings.GetOrCreateApplicationSettings(App.Taskt_Settings_File_Path);
                appSettings = App.GetFrmScriptBuilderApplicationSettings();

                // reinit
                Core.Server.HttpServerClient.Initialize();
            }
        }

        private void ShowScriptInformationForm()
        {
            using (var frm = new Supplemental.frmScriptInformations())
            {
                CreateUndoSnapshot();
                frm.infos = scriptInfo;
                frm.ShowDialog();
                ChangeSaveState(true);
            }
        }

        private void ShowAboutForm()
        {
            using (var aboutForm = new Supplemental.frmAbout())
            {
                aboutForm.ShowDialog();
            }
        }
    }
}
