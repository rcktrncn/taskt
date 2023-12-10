﻿using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmCommandList : ThemedForm
    {
        private Core.ApplicationSettings appSettings;

        private TreeNode[] treeAllCommands;
        private ImageList treeAllCommandsImage;

        private string firstGroup;
        private string firstCommand;

        public frmCommandList(Core.ApplicationSettings appSettings,TreeNode[] commands, ImageList commandsImage, string selectedCommand)
        {
            InitializeComponent();

            this.appSettings = appSettings;
            treeAllCommands = (TreeNode[])commands.Clone();
            treeAllCommandsImage = commandsImage;

            var spt = selectedCommand.Split('-');
            if (spt.Length == 2)
            {
                firstGroup = spt[0].Trim();
                firstCommand = spt[1].Trim();
            }
            else
            {
                firstGroup = "";
                firstCommand = "";
            }
        }
        private void frmCommandList_Load(object sender, EventArgs e)
        {
            tvCommands.SuspendLayout();
            tvCommands.BeginUpdate();

            tvCommands.Nodes.Clear();
            foreach (TreeNode command in treeAllCommands)
            {
                tvCommands.Nodes.Add((TreeNode)command.Clone());
            }

            //taskt.Core.CommandsTreeControls.ShowCommandsTree(tvCommands, treeAllCommands);

            tvCommands.ImageList = treeAllCommandsImage;

            tvCommands.EndUpdate();
            tvCommands.ResumeLayout();

            if ((firstGroup != "") && (firstCommand != ""))
            {
                Core.CommandsTreeControls.FocusCommand(firstGroup, firstCommand, tvCommands);
            }
        }

        #region footer buttons
        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region tvCommands Events
        private void tvCommands_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvCommands.SelectedNode != null)
            {
                if (tvCommands.SelectedNode.Nodes.Count == 0)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void tvCommands_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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
            }
        }
        #endregion

        #region Commands Search
        private void txtSearchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchCommandProcess();
            }
        }

        private void picCommandSearch_Click(object sender, EventArgs e)
        {
            SearchCommandProcess();
        }

        private void picCommandClear_Click(object sender, EventArgs e)
        {
            txtSearchKeyword.Text = "";
            SearchCommandProcess();
        }

        private void SearchCommandProcess()
        {
            string keyword = txtSearchKeyword.Text.Trim().ToLower();
            if (keyword == "")
            {
                Core.CommandsTreeControls.ShowCommandsTree(tvCommands, treeAllCommands);
            }
            else
            {
                var filterdCommands = Core.CommandsTreeControls.FilterCommands(keyword, treeAllCommands, appSettings.ClientSettings);
                Core.CommandsTreeControls.ShowCommandsTree(tvCommands, filterdCommands, true);
            }
        }
        #endregion

        public string FullCommandName
        {
            get
            {
                return Core.CommandsTreeControls.GetSelectedFullCommandName(tvCommands);
            }
        }

    }
}
