using System.Drawing;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has lstScriptActions drawing events processes
         */

        /// <summary>
        /// decide indent width in lstScriptActions
        /// </summary>
        private void IndentListViewItems()
        {
            int indent = 0;
            foreach (ListViewItem rowItem in lstScriptActions.Items)
            {
                if (rowItem is null)
                {
                    continue;
                }

                var cmd = (ScriptCommand)rowItem.Tag;

                if ((cmd is IHaveErrorAdditionalCommands) || (cmd is IHaveIfAdditionalCommands) || (cmd is IHaveLoopAdditionalCommands))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if (cmd is IEndOfStacturedCommand)
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if (cmd is IDelimitersOfStructuredCommands)
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent += 2;
                    if (indent < 0) indent = 0;
                }
                else
                {
                    rowItem.IndentCount = indent;
                }
            }
            AutoSizeLineNumberColumn();

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                CreateMiniMap();
            }
        }

        /// <summary>
        /// auto size line number column in lstScriptActions
        /// </summary>
        private void AutoSizeLineNumberColumn()
        {
            //auto adjust column width based on # of commands
            int columnWidth = (this.lineCharWidth * lstScriptActions.Items.Count.ToString().Length);

            lstScriptActions.Columns[0].Width = columnWidth;
            lstScriptActions.Columns[2].Width = lstScriptActions.ClientSize.Width - columnWidth - lstScriptActions.Columns[1].Width;
        }

        private void CreateMiniMap()
        {
            if (lstScriptActions.Items.Count == 0)
            {
                return;
            }

            int lvItemHeight = lstScriptActions.Items[0].Bounds.Height;
            if (lvItemHeight < 1)
            {
                return;
            }

            int commandsNum = lstScriptActions.Items.Count;
            int lvShowLines = lstScriptActions.ClientRectangle.Height / lvItemHeight;

            int miniMapHeight = lvItemHeight * lvShowLines;
            int mapItemHeight = miniMapHeight / commandsNum;
            if (mapItemHeight < 2)
            {
                mapItemHeight = 2;
            }
            else if (mapItemHeight > lvItemHeight)
            {
                mapItemHeight = lvItemHeight;
            }

            int miniMapItems = miniMapHeight / mapItemHeight;
            if (miniMapItems > commandsNum)
            {
                miniMapItems = commandsNum;
            }

            double oneItemRatio = (double)miniMapItems / commandsNum;
            int steps = 1;
            if (oneItemRatio < 1.0)
            {
                double invRatio = 1.0 / oneItemRatio;
                steps = (int)invRatio;
                if (invRatio - steps != 0.0)
                {
                    steps++;
                }

                miniMapItems = (commandsNum / steps) + (commandsNum % steps == 0 ? 0 : 1);
            }

            if ((miniMap == null) || (miniMap.GetLength(0) != miniMapItems))
            {
                miniMap = null;
                miniMap = new int[miniMapItems, 2];
            }

            if (oneItemRatio < 1.0)
            {
                for (int i = 0; i < miniMapItems; i++)
                {
                    int startIndex = i * steps;
                    int lastIndex = startIndex + steps;
                    if (lastIndex > commandsNum)
                    {
                        lastIndex = commandsNum;
                    }

                    miniMap[i, 0] = (int)MiniMapState.Normal;
                    miniMap[i, 1] = (int)MiniMapState.Normal;
                    for (int j = startIndex; j < lastIndex; j++)
                    {
                        var command = (ScriptCommand)lstScriptActions.Items[j].Tag;
                        if (command.IsDontSavedCommand)
                        {
                            miniMap[i, 0] = (int)MiniMapState.DontSave;
                            break;
                        }
                        else if (command.IsNewInsertedCommand)
                        {
                            miniMap[i, 0] = (int)MiniMapState.NewInserted;
                            break;
                        }
                    }
                    for (int j = startIndex; j < lastIndex; j++)
                    {
                        var command = (ScriptCommand)lstScriptActions.Items[j].Tag;
                        if (j == selectedIndex)
                        {
                            miniMap[i, 1] = (int)MiniMapState.Cursor;
                            break;
                        }
                        else if (command.IsMatched)
                        {
                            miniMap[i, 1] = (int)MiniMapState.Matched;
                            break;
                        }
                        else if (!command.IsValid)
                        {
                            miniMap[i, 1] = (int)MiniMapState.Error;
                            break;
                        }
                        else if (command.IsCommented || (command is CommentCommand))
                        {
                            miniMap[i, 1] = (int)MiniMapState.Comment;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < commandsNum; i++)
                {
                    var command = (ScriptCommand)lstScriptActions.Items[i].Tag;
                    if (command.IsDontSavedCommand)
                    {
                        miniMap[i, 0] = (int)MiniMapState.DontSave;
                    }
                    else if (command.IsNewInsertedCommand)
                    {
                        miniMap[i, 0] = (int)MiniMapState.NewInserted;
                    }
                    else
                    {
                        miniMap[i, 0] = (int)MiniMapState.Normal;
                    }

                    if (i == selectedIndex)
                    {
                        miniMap[i, 1] = (int)MiniMapState.Cursor;
                    }
                    else if (command.IsMatched)
                    {
                        miniMap[i, 1] = (int)MiniMapState.Matched;
                    }
                    else if (!command.IsValid)
                    {
                        miniMap[i, 1] = (int)MiniMapState.Error;
                    }
                    else if (command.IsCommented || (command is CommentCommand))
                    {
                        miniMap[i, 1] = (int)MiniMapState.Comment;
                    }
                    else
                    {
                        miniMap[i, 1] = (int)MiniMapState.Normal;
                    }
                }
            }

            if ((miniMapImg == null) || (miniMapImg.Height != lstScriptActions.Height))
            {
                miniMapImg = new Bitmap(8, lstScriptActions.Height);
            }
            using (Graphics g = Graphics.FromImage(miniMapImg))
            {
                g.DrawLine(new Pen(Color.White), 0, 0, 0, miniMapImg.Height);
                g.DrawLine(new Pen(Color.White), 4, 0, 4, miniMapImg.Height);
                for (int i = 0; i < miniMapItems; i++)
                {
                    SolidBrush co;
                    switch ((MiniMapState)miniMap[i, 0])
                    {
                        case MiniMapState.DontSave:
                            co = new SolidBrush(Core.Theme.scriptTexts["number-dontsaved"].BackColor);
                            break;
                        case MiniMapState.NewInserted:
                            co = new SolidBrush(Core.Theme.scriptTexts["number-newline"].BackColor);
                            break;
                        default:
                            co = new SolidBrush(Core.Theme.scriptTexts["normal"].BackColor);
                            break;
                    }
                    g.FillRectangle(co, 1, mapItemHeight * i, 3, mapItemHeight);

                    switch ((MiniMapState)miniMap[i, 1])
                    {
                        //case MiniMapState.Cursor:
                        //    co = new SolidBrush(taskt.Core.Theme.scriptTexts["selected-normal"].BackColor);
                        //    break;
                        case MiniMapState.Matched:
                            co = new SolidBrush(Core.Theme.scriptTexts["current-match"].BackColor);
                            break;
                        case MiniMapState.Error:
                            co = new SolidBrush(Core.Theme.scriptTexts["invalid"].FontColor);
                            break;
                        case MiniMapState.Comment:
                            co = new SolidBrush(Core.Theme.scriptTexts["comment"].FontColor);
                            break;
                        default:
                            co = new SolidBrush(Core.Theme.scriptTexts["normal"].BackColor);
                            break;
                    }
                    g.FillRectangle(co, 5, mapItemHeight * i, 3, mapItemHeight);

                    if ((MiniMapState)miniMap[i, 1] == MiniMapState.Cursor)
                    {
                        g.FillRectangle(new SolidBrush(Color.Navy), 0, mapItemHeight * i, 8, mapItemHeight);
                    }
                }
                g.FillRectangle(new SolidBrush(Color.DarkGray), 0, mapItemHeight * miniMapItems, 8, lstScriptActions.Height - (mapItemHeight * miniMapItems));
                // DBG
                //miniMapImg.Save(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\minimap.png");
            }

            //Console.WriteLine("cmdLines: " + lvCommandsNum +  ", 1itemHe: " + mapItemHeight + ", maxLines: " + maxMapItems + ", 1lineCmds: " + oneItemRatio);
        }

        private void lstScriptActions_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // switch between column index
            switch (e.ColumnIndex)
            {
                case 0:
                    // draw row number
                    DrawLineNumber(e);
                    break;
                case 1:
                    // draw command icon
                    DrawCommandIcon(e);
                    break;

                case 2:
                    // write command text
                    DrawCommandText(e);

                    break;
            }
        }

        private void DrawLineNumber(DrawListViewSubItemEventArgs e)
        {
            //AutoSizeLineNumberColumn();
            var command = (ScriptCommand)e.Item.Tag;

            Core.Theme.UIFont trg = Core.Theme.scriptTexts[DecideLineNumberText(command)];

            e.Graphics.FillRectangle(new SolidBrush(trg.BackColor), e.Bounds);
            e.Graphics.DrawString((e.ItemIndex + 1).ToString(), new Font(trg.Font, trg.FontSize, trg.Style), new SolidBrush(trg.FontColor), e.Bounds);
        }

        private void DrawCommandIcon(DrawListViewSubItemEventArgs e)
        {
            var command = (ScriptCommand)e.Item.Tag;
            var modifiedBounds = e.Bounds;
            var img = Images.GetUIImage(command.GetType().Name);
            if (img != null)
            {
                e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
            }
        }

        private void DrawCommandText(DrawListViewSubItemEventArgs e)
        {
            // get listviewitem
            ListViewItem item = e.Item;
            var command = (ScriptCommand)item.Tag;
            var modifiedBounds = e.Bounds;

            Core.Theme.UIFont trg;

            int indentWidth = appSettings.ClientSettings.IndentWidth;

            if ((debugLine > 0) && (e.ItemIndex == debugLine - 1))
            {
                trg = Core.Theme.scriptTexts["debug"];
            }
            else if ((currentScriptEditorMode == CommandEditorState.Search) || (currentScriptEditorMode == CommandEditorState.AdvencedSearch) || (currentScriptEditorMode == CommandEditorState.ReplaceSearch) || (currentScriptEditorMode == CommandEditorState.HighlightCommand))
            {
                if (command.IsMatched)
                {
                    if ((e.Item.Focused) || (e.Item.Selected))
                    {
                        trg = Core.Theme.scriptTexts["current-match"];
                    }
                    else
                    {
                        trg = Core.Theme.scriptTexts["match"];
                    }
                }
                else
                {
                    trg = Core.Theme.scriptTexts[DecideNormalCommandText(e, command)];
                }
            }
            else
            {
                trg = Core.Theme.scriptTexts[DecideNormalCommandText(e, command)];
            }

            // fille with background color
            e.Graphics.FillRectangle(new SolidBrush(trg.BackColor), modifiedBounds);

            // get indent count
            var indentPixels = (item.IndentCount * indentWidth);

            // set indented X position
            modifiedBounds.X += indentPixels;

            // draw string
            e.Graphics.DrawString(command.GetDisplayValue(),
                           new Font(trg.Font, trg.FontSize, trg.Style), new SolidBrush(trg.FontColor), modifiedBounds);

            // Emphasis Drag&Drop Line
            // DBG
            //Console.WriteLine("DRW " + this.currentEditAction + ", " + this.DnDIndex);
            if ((this.currentEditAction == CommandEditAction.Move) && (item.Index == this.DnDIndex))
            {
                int y = (lstScriptActions.SelectedItems[0].Index <= this.DnDIndex) ? (modifiedBounds.Y + modifiedBounds.Height - 1) : (modifiedBounds.Y);
                e.Graphics.DrawLine(new Pen(Color.DarkRed), modifiedBounds.X, y, modifiedBounds.X + modifiedBounds.Width, y);
            }

            // indent tab line
            if ((item.IndentCount > 0) && appSettings.ClientSettings.ShowIndentLine)
            {
                int offset;
                int i;
                if (item.IndentCount % 4 == 0)
                {
                    offset = indentWidth * 2;
                    i = item.IndentCount - 2;
                }
                else
                {
                    offset = 0;
                    i = item.IndentCount;
                }
                int bottomY = modifiedBounds.Y + modifiedBounds.Height;
                for (i = (item.IndentCount % 4 != 0 ? item.IndentCount - 2 : item.IndentCount); i > 0; i -= 4)
                {
                    int x = modifiedBounds.X - (i * indentWidth) + offset;
                    e.Graphics.DrawLine(indentDashLine, x, modifiedBounds.Y, x, bottomY);
                }

                int baseX = modifiedBounds.X - (item.IndentCount * indentWidth) + 2;
                e.Graphics.DrawLine(indentDashLine, baseX, modifiedBounds.Y, baseX, bottomY);
            }

            if (appSettings.ClientSettings.ShowScriptMiniMap)
            {
                modifiedBounds.X -= indentPixels;
                int baseX2 = modifiedBounds.Width - 8;
                e.Graphics.DrawImage(miniMapImg, new Rectangle(modifiedBounds.X + baseX2, modifiedBounds.Y, 8, modifiedBounds.Height),
                                                    new Rectangle(0, modifiedBounds.Y, 8, modifiedBounds.Height), GraphicsUnit.Pixel);
            }
        }

        private string DecideNormalCommandText(DrawListViewSubItemEventArgs e, ScriptCommand command)
        {
            string ret;

            if (command.PauseBeforeExeucution)
            {
                ret = "pause";
            }
            else if ((command is CommentCommand) || (command.IsCommented))
            {
                ret = "comment";
            }
            else if (!command.IsValid)
            {
                ret = "invalid";
            }
            else
            {
                ret = "normal";
            }

            if ((e.Item.Focused) || (e.Item.Selected))
            {
                ret = "selected-" + ret;
            }

            return ret;
        }

        private string DecideLineNumberText(ScriptCommand command)
        {
            string ret;

            if (command.IsDontSavedCommand)
            {
                ret = "dontsaved";
            }
            else if (command.IsNewInsertedCommand)
            {
                ret = "newline";
            }
            else
            {
                ret = "normal";
            }
            return "number-" + ret;
        }

        /// <summary>
        /// decide character width in lstScriptActions line number
        /// </summary>
        /// <returns></returns>
        private int DecideScriptActionsLineCharacterWidth()
        {
            var bitmap = new Bitmap(256, 256);
            var g = Graphics.FromImage(bitmap);
            var size = g.MeasureString("0", new Font(lstScriptActions.Font.FontFamily, lstScriptActions.Font.Size));

            var w = size.Width;
            if ((w - (int)w) > 0.0)
            {
                return (int)w + 1;
            }
            else
            {
                return (int)w;
            }
        }
    }
}
