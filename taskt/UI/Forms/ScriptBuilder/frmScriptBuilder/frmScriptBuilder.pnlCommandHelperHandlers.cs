using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has Welcome Screen event handlers
         */

        #region Link Labels
        private void GenerateRecentFiles()
        {
            flwRecentFiles.Controls.Clear();

            var scriptPath = Core.IO.Folders.GetScriptsFolderPath();

            if (!System.IO.Directory.Exists(scriptPath))
            {
                lblRecentFiles.Text = "Script Folder does not exist";
                lblFilesMissing.Text = "Directory Not Found: " + scriptPath;
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
                return;
            }

            var directory = new System.IO.DirectoryInfo(scriptPath);

            var recentFiles = directory.GetFiles("*.xml")
                .OrderByDescending(file => file.LastWriteTime).Select(f => f.Name);


            if (recentFiles.Count() == 0)
            {
                lblRecentFiles.Text = "No Recent Files Found";
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
            }
            else
            {
                flwRecentFiles.SuspendLayout();
                foreach (var fil in recentFiles)
                {
                    if (flwRecentFiles.Controls.Count == 7)
                    {
                        break;
                    }

                    LinkLabel newFileLink = new LinkLabel
                    {
                        Text = fil,
                        AutoSize = true,
                        LinkColor = Color.AliceBlue,
                        Font = lnkGitIssue.Font,
                        Margin = new Padding(0, 0, 0, 0)
                    };
                    newFileLink.LinkClicked += NewFileLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newFileLink);
                }
                flwRecentFiles.ResumeLayout();
            }
        }

        private void lnkStartEdit_Click(object sender, EventArgs e)
        {
            pnlCommandHelper.Visible = false;
        }

        private void lnkGitProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowGitProjectPage();
        }

        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowGitReleasePage();
        }

        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowGitIssuePage();
        }

        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowWikiPage();
        }

        private void linkGitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowGitterPage();
        }

        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            var targetScriptPath = System.IO.Path.Combine(Core.IO.Folders.GetScriptsFolderPath(), senderLink.Text);
            OpenScriptFromFilePath(targetScriptPath, true);
        }
        #endregion

        #region Drag&Drop script file
        private void pnlCommandHelper_DragDrop(object sender, DragEventArgs e)
        {
            pnlCommandHelper.BackColor = Color.FromArgb(59, 59, 59);
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileNames.Length > 0)
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
        }

        private void pnlCommandHelper_DragEnter(object sender, DragEventArgs e)
        {
            pnlCommandHelper.BackColor = Color.FromArgb(59, 59, 128);
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
            }
        }

        private void pnlCommandHelper_DragLeave(object sender, EventArgs e)
        {
            pnlCommandHelper.BackColor = Color.FromArgb(59, 59, 59);
        }
        #endregion

        #region welcome screen
        private void picRecentFiles_Click(object sender, EventArgs e)
        {
            BeginOpenScriptProcess();
        }
        #endregion
    }
}
