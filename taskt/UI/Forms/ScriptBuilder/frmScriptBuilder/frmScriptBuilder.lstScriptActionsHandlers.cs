using System;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has lstScriptActions event handlers
         */

        private void lstScriptActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            miniMapLoadingDelayTimer.Stop();
            miniMapLoadingDelayTimer.Start();
        }

        private void miniMapLoadingDelayTimer_Tick(object sender, EventArgs e)
        {
            miniMapLoadingDelayTimer.Stop();
            lstScriptActions_SelectedIndexChangedProcess();
        }

        private void lstScriptActions_SelectedIndexChangedProcess()
        {
            if (!appSettings.ClientSettings.InsertCommandsInline)
            {
                return;
            }

            // check to see if an item has been selected last
            if (lstScriptActions.SelectedItems.Count > 0)
            {
                selectedIndex = lstScriptActions.SelectedItems[0].Index;
            }
            else
            {
                // nothing is selected
                selectedIndex = -1;
            }

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                CreateMiniMap();
            }
        }

        private void lstScriptActions_KeyDown(object sender, KeyEventArgs e)
        {
            // delete from listview if required
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRows();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                // if user presses enter simulate double click event
                lstScriptActions_DoubleClick(null, null);
            }
            else if ((e.Control) && (e.KeyCode == Keys.X))
            {
                CutRows();
            }
            else if ((e.Control) && (e.KeyCode == Keys.C))
            {
                CopyRows();
            }
            else if ((e.Control) && (e.KeyCode == Keys.V))
            {
                PasteRows();
            }
            //else if ((e.Control) && (e.KeyCode == Keys.Z))
            //{
            //    UndoChange();
            //}
            //else if ((e.Control) && (e.KeyCode == Keys.R))
            //{
            //    RedoChange();
            //}
            else if ((e.Control) && (e.Shift) && (e.KeyCode == Keys.E))
            {
                lstScriptActions_DoubleClick(null, null);
            }
            else if ((e.Control) && (e.KeyCode == Keys.E))
            {
                SetSelectedCodeToCommented(false);
            }
            else if ((e.Control) && (e.KeyCode == Keys.D))
            {
                SetSelectedCodeToCommented(true);
            }
            else if ((e.Control) && (e.KeyCode == Keys.A))
            {
                SelectAllRows();
            }
            else if ((e.Control) && (e.Shift) && (e.KeyCode == Keys.F))
            {
                // highlight this command
                HighlightAllCurrentSelectedCommand();
            }
            else if (e.KeyCode == Keys.F1)
            {
                if (lstScriptActions.SelectedItems.Count > 0)
                {
                    ShowThisCommandHelp((ScriptCommand)lstScriptActions.SelectedItems[0].Tag);
                }
            }
        }

        private void lstScriptActions_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lstScriptActions.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    if (lstScriptActions.SelectedIndices.Count == 0)
                    {
                        // no selected command
                        insertCommentToolStripMenuItem.Visible = false;
                    }
                    else if (lstScriptActions.SelectedIndices[0] == 0)
                    {
                        // when select line 1, can not insert comment line 0
                        aboveHereToolStripMenuItem.Enabled = false;
                        insertCommentToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        aboveHereToolStripMenuItem.Enabled = true;
                        insertCommentToolStripMenuItem.Visible = true;
                    }

                    if ((ScriptCommand)lstScriptActions.SelectedItems[0].Tag is EnterKeysCommand)
                    {
                        multiSendKeystrokesEditToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        multiSendKeystrokesEditToolStripMenuItem.Visible = false;
                    }

                    lstScriptActionsContextStrip.Show(Cursor.Position);
                }
            }
        }
        private void lstScriptActions_DoubleClick(object sender, EventArgs e)
        {
            EditSelectedCommand();
        }
    }
}
