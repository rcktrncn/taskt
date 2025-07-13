using System.Reflection;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has show other website process
         */

        private void ShowGitProjectPage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitProjectURL);
        }
        private void ShowGitReleasePage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitReleaseURL);
        }
        private void ShowGitIssuePage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitIssueURL);
        }
        private void ShowWikiPage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.WikiURL);
        }
        private void ShowGitterPage()
        {
            System.Diagnostics.Process.Start(Core.MyURLs.GitterURL);
        }
        private void ShowThisCommandHelp(ScriptCommand command)
        {
            string parent = ((Core.Automation.Attributes.ClassAttributes.Group)command.GetType().GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group))).groupName;
            System.Diagnostics.Process.Start(Core.MyURLs.GetWikiURL(command.SelectionName, parent));
        }
        private void BeginShowThisCommandHelpProcess()
        {
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                ShowThisCommandHelp((ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
            }
        }
    }
}
