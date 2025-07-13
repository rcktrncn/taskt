using System;
using System.Linq;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has tvCommands context menu event handlers
         */

        private void expandRootTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            tvCommands.SelectedNode.Expand();
        }

        private void collapseRootTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            tvCommands.SelectedNode.Collapse();
        }

        private void clearRootTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            ShowAllCommands();
        }

        private void addCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            BeforeAddNewCommandProcess();
        }

        private void helpCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }
            string commandName = GetSelectedCommandFullName();
            if (commandName.Length == 0)
            {
                return;
            }
            var cmd = automationCommands.Where(t => t.FullName == commandName).FirstOrDefault();
            if (cmd != null)
            {
                ShowThisCommandHelp(cmd.Command);
            }
        }

        private void sampleThisCommandTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }
            string commandName = GetSelectedCommandFullName().Split('-')[1].Trim();
            using (var frm = new Supplemental.frmSample(commandName))
            {
                frm.ShowDialog(this);
            }
        }

        private void highlightCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }
            string commandName = GetSelectedCommandFullName();
            if (commandName.Length == 0)
            {
                return;
            }
            var cmd = automationCommands.Where(t => t.FullName == commandName).FirstOrDefault();
            if (cmd != null)
            {
                AdvancedSearchItemInCommands(((ScriptCommand)cmd.Command).SelectionName, false, false, true, false, false, false, "");
            }
        }

        private void clearCmdTVCommandMenuStrip_Click(object sender, EventArgs e)
        {
            ShowAllCommands();
        }
    }
}
