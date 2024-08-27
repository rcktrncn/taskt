using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmSample : ThemedForm
    {
        private string samplePath;
        private frmScriptBuilder parentForm;
        private TreeNode[] bufferdSampleNodes;

        public frmSample()
        {
            InitializeComponent();
        }

        public frmSample(string searchKeyword) : this()
        {
            txtSearchBox.Text = searchKeyword;
        }

        private void frmSample_Load(object sender, EventArgs e)
        {
            this.parentForm = (frmScriptBuilder)this.Owner;

            //samplePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Samples";
            samplePath = Core.IO.Folders.GetSamplesFolderPath();

            if (!Directory.Exists(samplePath))
            {
                return;
            }

            var files = Directory.EnumerateFiles(samplePath, "*.xml", SearchOption.AllDirectories).ToList();

            //int baseLen = samplePath.Length + 1;    // (+1) is \\
            var baseLen = samplePath.Length;

            ImageList tvImageList = new ImageList();
            tvImageList.ImageSize = new Size(16, 16);
            tvImageList.Images.Add(Properties.Resources.sample_templete);
            tvImageList.Images.Add(Properties.Resources.command_group);
            tvSamples.ImageList = tvImageList;

            string oldFolder = "----";

            var tempNodes = new List<TreeNode>();
            TreeNode parentGroup = null;
            foreach(var file in files)
            {
                var absPath = file.Substring(baseLen);
                var absParts = absPath.Split('\\');
                if (absParts[0] == oldFolder)
                {
                    TreeNode newNode = new TreeNode(ConvertFileNameToTreeNode(absParts[1]));
                    parentGroup.Nodes.Add(newNode);
                }
                else
                {
                    if (oldFolder != "----")
                    {
                        tempNodes.Add(parentGroup);
                    }
                    oldFolder = absParts[0];
                    parentGroup = new TreeNode(absParts[0], 1, 1);
                    var newNode = new TreeNode(ConvertFileNameToTreeNode(absParts[1]));
                    parentGroup.Nodes.Add(newNode);
                }
            }
            tempNodes.Add(parentGroup);
            bufferdSampleNodes = tempNodes.ToArray();

            tvSamples.BeginUpdate();
            tvSamples.Nodes.AddRange(bufferdSampleNodes);

            tvSamples.EndUpdate();
            //tvSamples.ExpandAll();

            if (txtSearchBox.Text.Length > 0)
            {
                ApplyFilterProcess();
            }
        }

        private void frmSample_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        #region tvSample events
        private void tvSamples_DoubleClick(object sender, EventArgs e)
        {
            if (tvSamples.SelectedNode == null)
            {
                return;
            }
            if (tvSamples.SelectedNode.Level == 0)
            {
                return;
            }
            else
            {
                tvContextMenuStrip.Show(Cursor.Position);
            }
        }

        private void tvSamples_MouseClick(object sender, MouseEventArgs e)
        {
            if (tvSamples.SelectedNode == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (tvSamples.SelectedNode.Level == 0)
                {
                    if (tvSamples.SelectedNode.IsExpanded)
                    {
                        expandToolStripMenuItem.Visible = false;
                        collapseToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        expandToolStripMenuItem.Visible = true;
                        collapseToolStripMenuItem.Visible = false;
                    }
                    rootContextMenuStrip.Show(Cursor.Position);
                }
                else
                {
                    tvContextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void tvSamples_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvSamples.SelectedNode = e.Node;
            }
        }

        private void tvSamples_KeyDown(object sender, KeyEventArgs e)
        {
            if (tvSamples.SelectedNode == null)
            {
                return;
            }
            if (tvSamples.SelectedNode.Level == 1)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (e.Control || e.Shift)
                    {
                        ImportSampleScriptProcess();
                    }
                    else
                    {
                        OpenSampleScriptProcess();
                    }
                }
                else
                {
                    if (e.Control && (e.KeyCode == Keys.N))
                    {
                        NewTasktSampleScriptProcess();
                    }
                }
            }
        }
        #endregion

        #region footer buttons
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenSampleScriptProcess();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportSampleScriptProcess();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewTasktSampleScriptProcess();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Open/Import sample
        private string ConvertFileNameToTreeNode(string filaName)
        {
            return Path.GetFileNameWithoutExtension(filaName).Replace("_", " ");
        }

        private string ConvertTreeNodeToFileName(string treeText)
        {
            return treeText.Replace(" ", "_") + ".xml";
        }

        private string GetSelectedScriptPath()
        {
            if (tvSamples.SelectedNode.Level != 1)
            {
                return "";
            }
            else
            {
                //return samplePath + "\\" + tvSamples.SelectedNode.Parent.Text + "\\" + convertTreeNodeToFileName(tvSamples.SelectedNode.Text);
                return Path.Combine(samplePath, tvSamples.SelectedNode.Parent.Text, ConvertTreeNodeToFileName(tvSamples.SelectedNode.Text));
            }
        }

        private void OpenSampleScriptProcess()
        {
            var targetFile = GetSelectedScriptPath();
            //string fileName = Path.GetFileName(targetFile);
            if (targetFile != "")
            {
                parentForm.OpenScriptFromFilePath(targetFile);
                this.Close();
            }
        }

        private void ImportSampleScriptProcess()
        {
            var targetFile = GetSelectedScriptPath();
            //string fileName = Path.GetFileName(targetFile);
            if (targetFile != "")
            {
                parentForm.ImportScriptFromFilePath(targetFile);
                this.Close();
            }
        }

        private void NewTasktSampleScriptProcess()
        {
            var targetFile = GetSelectedScriptPath();
            //string fileName = Path.GetFileName(targetFile);
            if (targetFile != "")
            {
                System.Diagnostics.ProcessStartInfo pInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = Assembly.GetEntryAssembly().Location,
                    Arguments = "-oh \"" + GetSelectedScriptPath() + "\""
                };
                System.Diagnostics.Process.Start(pInfo);
                this.Close();
            }
        }
        #endregion

        #region tvContextMenuStrip events
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSampleScriptProcess();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportSampleScriptProcess();
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTasktSampleScriptProcess();
        }

        private void clearFilterTvContextMenuStrip_Click(object sender, EventArgs e)
        {
            //txtSearchBox.Text = "";
            ShowAllSamplesProcess();
        }
        #endregion

        #region rootContextMenuStrip events
        private void expandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvSamples.SelectedNode.ExpandAll();
        }

        private void collapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvSamples.SelectedNode.Collapse();
        }

        private void clearFilterRootContextMenuStrop_Click(object sender, EventArgs e)
        {
            //txtSearchBox.Text = "";
            ShowAllSamplesProcess();
        }
        #endregion

        #region search filter
        private void picSearch_Click(object sender, EventArgs e)
        {
            ApplyFilterProcess();
        }

        private void picClear_Click(object sender, EventArgs e)
        {
            //txtSearchBox.Text = "";
            ShowAllSamplesProcess();
        }

        private void txtSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                ApplyFilterProcess();
            }
        }

        /// <summary>
        /// tvSample update process
        /// </summary>
        /// <param name="proc"></param>
        private void tvSamplesUpdateProcess(Action proc)
        {
            tvSamples.BeginUpdate();
            proc();
            tvSamples.EndUpdate();
        }

        /// <summary>
        /// apply filter text in tvSamples
        /// </summary>
        private void ApplyFilterProcess()
        {
            var keyword = txtSearchBox.Text.ToLower().Trim();
            if (keyword.Length == 0)
            {
                ShowAllSamplesProcess();
            }
            else
            {
                FilterSamplesProcess(keyword);
            }
        }

        /// <summary>
        /// filter samples
        /// </summary>
        /// <param name="keyword"></param>
        private void FilterSamplesProcess(string keyword)
        {
            //tvSamples.BeginUpdate();
            //tvSamples.Nodes.Clear();

            //foreach(TreeNode parentNode in bufferdSampleNodes)
            //{
            //    var paNode = new TreeNode("", 1, 1);
            //    foreach(TreeNode node in parentNode.Nodes)
            //    {
            //        if (node.Text.ToLower().Contains(keyword))
            //        {
            //            paNode.Nodes.Add(node.Text);
            //        }
            //    }
            //    if (paNode.Nodes.Count > 0)
            //    {
            //        paNode.Text = parentNode.Text;
            //        tvSamples.Nodes.Add(paNode);
            //    }
            //}

            //if (tvSamples.Nodes.Count == 0)
            //{
            //    tvSamples.Nodes.Add(new TreeNode("nothing :-("));
            //}
            //tvSamples.ExpandAll();

            //tvSamples.EndUpdate();

            tvSamplesUpdateProcess(new Action(() =>
            {
                tvSamples.Nodes.Clear();

                foreach (TreeNode parentNode in bufferdSampleNodes)
                {
                    var paNode = new TreeNode("", 1, 1);
                    foreach (TreeNode node in parentNode.Nodes)
                    {
                        if (node.Text.ToLower().Contains(keyword))
                        {
                            paNode.Nodes.Add(node.Text);
                        }
                    }
                    if (paNode.Nodes.Count > 0)
                    {
                        paNode.Text = parentNode.Text;
                        tvSamples.Nodes.Add(paNode);
                    }
                }

                if (tvSamples.Nodes.Count == 0)
                {
                    tvSamples.Nodes.Add(new TreeNode("nothing :-("));
                }
                tvSamples.ExpandAll();
            }));

            clearFilterRootContextMenuStrop.Enabled = true;
            clearFilterTvContextMenuStrip.Enabled = true;
        }

        /// <summary>
        /// show all samples
        /// </summary>
        private void ShowAllSamplesProcess()
        {
            txtSearchBox.Text = "";

            tvSamplesUpdateProcess(new Action(() => {
                tvSamples.Nodes.Clear();
                tvSamples.Nodes.AddRange(bufferdSampleNodes);
            }));

            //tvSamples.BeginUpdate();

            //tvSamples.Nodes.Clear();
            //tvSamples.Nodes.AddRange(bufferdSampleNodes);

            //tvSamples.EndUpdate();

            clearFilterRootContextMenuStrop.Enabled = false;
            clearFilterTvContextMenuStrip.Enabled = false;
        }
        #endregion
    }
}
