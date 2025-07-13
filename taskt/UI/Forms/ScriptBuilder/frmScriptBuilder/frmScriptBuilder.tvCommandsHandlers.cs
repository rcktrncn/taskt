using System;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has tvCommands event handlers
         */

        private void tvCommands_DoubleClick(object sender, EventArgs e)
        {
            BeforeAddNewCommandProcess();
        }

        private void tvCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                tvCommands_DoubleClick(this, null);
            }
        }

        private void tvCommands_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvCommands.SelectedNode = e.Node;
            }
        }

        private void tvCommands_MouseClick(object sender, MouseEventArgs e)
        {
            if (tvCommands.SelectedNode == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (tvCommands.SelectedNode.Level == 0)
                {
                    if (tvCommands.SelectedNode.IsExpanded)
                    {
                        expandRootTVCommandMenuStrip.Visible = false;
                        collapseRootTVCommandMenuStrip.Visible = true;
                    }
                    else
                    {
                        expandRootTVCommandMenuStrip.Visible = true;
                        collapseRootTVCommandMenuStrip.Visible = false;
                    }
                    rootTVCommandMenuStrip.Show(Cursor.Position);
                }
                else
                {
                    if (tvCommands.SelectedNode.Nodes.Count > 0)
                    {
                        if (tvCommands.SelectedNode.IsExpanded)
                        {
                            expandRootTVCommandMenuStrip.Visible = false;
                            collapseRootTVCommandMenuStrip.Visible = true;
                        }
                        else
                        {
                            expandRootTVCommandMenuStrip.Visible = true;
                            collapseRootTVCommandMenuStrip.Visible = false;
                        }
                        rootTVCommandMenuStrip.Show(Cursor.Position);
                    }
                    else
                    {
                        cmdTVCommandMenuStrip.Show(Cursor.Position);
                    }
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                var trg = tvCommands.HitTest(e.X, e.Y);
                if (trg.Location.ToString() == "Image")
                {
                    var node = trg.Node;
                    if (node.Nodes.Count > 0)
                    {
                        if (node.IsExpanded)
                        {
                            node.Collapse();
                        }
                        else
                        {
                            node.Expand();
                        }
                    }
                }
                //Console.WriteLine(trg.Location.ToString() + ", " + trg.Node.Text);
            }
        }

        private void picCommandFilter_Click(object sender, EventArgs e)
        {
            string keyword = txtCommandFilter.Text.ToLower().Trim();
            if (keyword == "")
            {
                ShowAllCommands();
            }
            else
            {
                ShowFilterCommands(keyword);
            }
        }
        
        private void picCommandFilterClear_Click(object sender, EventArgs e)
        {
            ShowAllCommands();
        }

        private void txtCommandFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                string keyword = txtCommandFilter.Text.ToLower().Trim();
                if (keyword == "")
                {
                    ShowAllCommands();
                }
                else
                {
                    ShowFilterCommands(keyword);
                }
            }
        }

        private void ShowFilterCommands(string keyword)
        {
            TreeNode[] filterdCommands = CustomControls.CommandsTreeControls.FilterCommands(keyword, bufferedCommandList, appSettings.ClientSettings);

            CustomControls.CommandsTreeControls.ShowCommandsTree(tvCommands, filterdCommands, true);

            clearCmdTVCommandMenuStrip.Enabled = true;
            clearRootTVCommandMenuStrip.Enabled = true;
        }

        private void ShowAllCommands()
        {
            txtCommandFilter.Text = "";

            CustomControls.CommandsTreeControls.ShowCommandsTree(tvCommands, bufferedCommandList);

            clearCmdTVCommandMenuStrip.Enabled = false;
            clearRootTVCommandMenuStrip.Enabled = false;
        }

        private void SearchSelectedCommand()
        {
            if (lstScriptActions.SelectedItems.Count == 0)
            {
                return;
            }

            var command = (ScriptCommand)lstScriptActions.SelectedItems[0].Tag;
            var tp = command.GetType();
            var group = (Core.Automation.Attributes.ClassAttributes.Group)tp.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group));

            CustomControls.CommandsTreeControls.FocusCommand(group.groupName, command.SelectionName, tvCommands);
        }
    }
}
