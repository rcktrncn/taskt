using System;
using System.Drawing;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has lstScriptActions drag&drop event handlers
         */

        private void lstScriptActions_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.currentEditAction = CommandEditAction.Move;
            lstScriptActions.DoDragDrop(lstScriptActions.SelectedItems, DragDropEffects.Move);
        }

        private void lstScriptActions_DragOver(object sender, DragEventArgs e)
        {
            if (this.currentEditAction == CommandEditAction.Move)
            {
                var cp = lstScriptActions.PointToClient(new Point(e.X, e.Y));
                var dragToItem = lstScriptActions.HitTest(cp.X, cp.Y);
                this.DnDIndex = (dragToItem.Item == null) ? -1 : dragToItem.Item.Index;
            }
            lstScriptActions.Invalidate();
        }

        private void lstScriptActions_DragEnter(object sender, DragEventArgs e)
        {
            //int len = e.Data.GetFormats().Length - 1;
            if (e.Data.GetFormats()[0] == "System.Windows.Forms.ListView+SelectedListViewItemCollection")
            {
                e.Effect = DragDropEffects.Move;
                this.currentEditAction = CommandEditAction.Move;
            }
            else
            {
                //Console.WriteLine(formatType);
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if ((e.KeyState & 12) != 0) // Shift or Ctrl
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                    lstScriptActions.BackColor = Color.LightGray;
                    this.currentEditAction = CommandEditAction.Normal;
                }
            }
        }

        private void lstScriptActions_DragLeave(object sender, EventArgs e)
        {
            lstScriptActions.BackColor = Color.WhiteSmoke;
        }

        private void lstScriptActions_DragDrop(object sender, DragEventArgs e)
        {
            lstScriptActions.BackColor = Color.WhiteSmoke;

            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if ((fileNames != null) && (fileNames.Length > 0))
            {
                var targetFile = fileNames[0];
                if (System.IO.Path.GetExtension(targetFile).ToLower() == ".xml")
                {
                    if ((e.KeyState & 12) != 0) // Shift or Ctrl
                    {
                        ImportScriptFromFilePath(targetFile);
                    }
                    else
                    {
                        OpenScriptFromFilePath(targetFile, true);
                    }
                }
                else
                {
                    using (var fm = new General.frmDialog("This file type can not open.", "File Open Error", General.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                }
            }
            else if (lstScriptActions.SelectedItems.Count > 0)
            {
                MoveCommands(e);
            }
        }

        private void lstScriptActions_MouseMove(object sender, MouseEventArgs e)
        {
            lstScriptActions.Invalidate();
        }

        private void MoveCommands(DragEventArgs e)
        {
            // Returns the location of the mouse pointer in the ListView control.
            var cp = lstScriptActions.PointToClient(new Point(e.X, e.Y));

            // Obtain the item that is located at the specified location of the mouse pointer.
            var dragToItem = lstScriptActions.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }

            CreateUndoSnapshot();

            lstScriptActions.BeginUpdate();

            // drag and drop for sequence
            if ((dragToItem.Tag is SequenceCommand sequence) && (appSettings.ClientSettings.EnableSequenceDragDrop))
            {
                // sequence command for drag drop
                //var sequence = (SequenceCommand)dragToItem.Tag;

                // add command to script actions
                for (int i = 0; i <= lstScriptActions.SelectedItems.Count - 1; i++)
                {
                    var command = (ScriptCommand)lstScriptActions.SelectedItems[i].Tag;
                    sequence.v_scriptActions.Add(command);
                }

                // remove originals
                int[] indices = new int[lstScriptActions.SelectedIndices.Count];
                lstScriptActions.SelectedIndices.CopyTo(indices, 0);
                for (int i = indices.Length - 1; i >= 0; i--)
                {
                    lstScriptActions.Items.RemoveAt(indices[i]);
                }
            }
            else
            {
                // Obtain the index of the item at the mouse pointer.
                int dragIndex = dragToItem.Index;

                var sel = new ListViewItem[lstScriptActions.SelectedItems.Count];
                for (int i = 0; i <= lstScriptActions.SelectedItems.Count - 1; i++)
                {
                    sel[i] = lstScriptActions.SelectedItems[i];
                }
                for (int i = 0; i < sel.GetLength(0); i++)
                {
                    // Obtain the ListViewItem to be dragged to the target location.
                    var dragItem = sel[i];
                    int itemIndex = dragIndex;
                    if (itemIndex == dragItem.Index)
                    {
                        //return;
                        break;
                    }
                    if (dragItem.Index < itemIndex)
                    {
                        itemIndex++;
                    }
                    else
                    {
                        itemIndex = dragIndex + i;
                    }

                    // Insert the item at the mouse pointer.
                    var insertItem = (ListViewItem)dragItem.Clone();

                    var command = (ScriptCommand)insertItem.Tag;
                    command.IsDontSavedCommand = true;
                    command.IsNewInsertedCommand = true;

                    lstScriptActions.Items.Insert(itemIndex, insertItem);
                    // Removes the item from the initial location while
                    // the item is moved to the new location.
                    lstScriptActions.Items.Remove(dragItem);
                }
            }
            this.currentEditAction = CommandEditAction.Normal;

            IndentListViewItems();  // update indent

            ChangeSaveState(true);

            lstScriptActions.EndUpdate();
            lstScriptActions.Invalidate();
        }
    }
}
