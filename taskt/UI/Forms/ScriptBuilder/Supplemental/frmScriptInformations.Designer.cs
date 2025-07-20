
namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    partial class frmScriptInformations
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptInformations));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtScriptRevision = new System.Windows.Forms.Label();
            this.lblScriptRevision = new System.Windows.Forms.Label();
            this.txtLastRun = new System.Windows.Forms.Label();
            this.lblLastRun = new System.Windows.Forms.Label();
            this.txtRunTimes = new System.Windows.Forms.Label();
            this.lblRunTimes = new System.Windows.Forms.Label();
            this.txtTasktVersion = new System.Windows.Forms.Label();
            this.lblTastkVersion = new System.Windows.Forms.Label();
            this.lblScriptDescription = new System.Windows.Forms.Label();
            this.txtScriptDescription = new System.Windows.Forms.TextBox();
            this.lblScriptVersion = new System.Windows.Forms.Label();
            this.txtScriptVersion = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.txtScriptAuthor = new System.Windows.Forms.TextBox();
            this.pnlFooterButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.btnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlFooterButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblMainLogo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnlFooterButtons, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(588, 442);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(4, 0);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(350, 54);
            this.lblMainLogo.TabIndex = 16;
            this.lblMainLogo.Text = "Script Informations";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.txtScriptRevision);
            this.panel1.Controls.Add(this.lblScriptRevision);
            this.panel1.Controls.Add(this.txtLastRun);
            this.panel1.Controls.Add(this.lblLastRun);
            this.panel1.Controls.Add(this.txtRunTimes);
            this.panel1.Controls.Add(this.lblRunTimes);
            this.panel1.Controls.Add(this.txtTasktVersion);
            this.panel1.Controls.Add(this.lblTastkVersion);
            this.panel1.Controls.Add(this.lblScriptDescription);
            this.panel1.Controls.Add(this.txtScriptDescription);
            this.panel1.Controls.Add(this.lblScriptVersion);
            this.panel1.Controls.Add(this.txtScriptVersion);
            this.panel1.Controls.Add(this.lblAuthor);
            this.panel1.Controls.Add(this.txtScriptAuthor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 73);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 296);
            this.panel1.TabIndex = 18;
            // 
            // txtScriptRevision
            // 
            this.txtScriptRevision.AutoSize = true;
            this.txtScriptRevision.BackColor = System.Drawing.Color.Transparent;
            this.txtScriptRevision.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtScriptRevision.ForeColor = System.Drawing.Color.SlateGray;
            this.txtScriptRevision.Location = new System.Drawing.Point(13, 411);
            this.txtScriptRevision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtScriptRevision.Name = "txtScriptRevision";
            this.txtScriptRevision.Size = new System.Drawing.Size(80, 28);
            this.txtScriptRevision.TabIndex = 22;
            this.txtScriptRevision.Text = "revision";
            // 
            // lblScriptRevision
            // 
            this.lblScriptRevision.AutoSize = true;
            this.lblScriptRevision.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptRevision.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptRevision.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptRevision.Location = new System.Drawing.Point(13, 390);
            this.lblScriptRevision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScriptRevision.Name = "lblScriptRevision";
            this.lblScriptRevision.Size = new System.Drawing.Size(118, 23);
            this.lblScriptRevision.TabIndex = 21;
            this.lblScriptRevision.Text = "Script Revision";
            // 
            // txtLastRun
            // 
            this.txtLastRun.AutoSize = true;
            this.txtLastRun.BackColor = System.Drawing.Color.Transparent;
            this.txtLastRun.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtLastRun.ForeColor = System.Drawing.Color.SlateGray;
            this.txtLastRun.Location = new System.Drawing.Point(12, 469);
            this.txtLastRun.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtLastRun.Name = "txtLastRun";
            this.txtLastRun.Size = new System.Drawing.Size(76, 28);
            this.txtLastRun.TabIndex = 20;
            this.txtLastRun.Text = "last run";
            // 
            // lblLastRun
            // 
            this.lblLastRun.AutoSize = true;
            this.lblLastRun.BackColor = System.Drawing.Color.Transparent;
            this.lblLastRun.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastRun.ForeColor = System.Drawing.Color.SlateGray;
            this.lblLastRun.Location = new System.Drawing.Point(12, 448);
            this.lblLastRun.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastRun.Name = "lblLastRun";
            this.lblLastRun.Size = new System.Drawing.Size(143, 23);
            this.lblLastRun.TabIndex = 19;
            this.lblLastRun.Text = "Last run date time";
            // 
            // txtRunTimes
            // 
            this.txtRunTimes.AutoSize = true;
            this.txtRunTimes.BackColor = System.Drawing.Color.Transparent;
            this.txtRunTimes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtRunTimes.ForeColor = System.Drawing.Color.SlateGray;
            this.txtRunTimes.Location = new System.Drawing.Point(12, 352);
            this.txtRunTimes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtRunTimes.Name = "txtRunTimes";
            this.txtRunTimes.Size = new System.Drawing.Size(93, 28);
            this.txtRunTimes.TabIndex = 18;
            this.txtRunTimes.Text = "run times";
            // 
            // lblRunTimes
            // 
            this.lblRunTimes.AutoSize = true;
            this.lblRunTimes.BackColor = System.Drawing.Color.Transparent;
            this.lblRunTimes.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunTimes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblRunTimes.Location = new System.Drawing.Point(12, 331);
            this.lblRunTimes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunTimes.Name = "lblRunTimes";
            this.lblRunTimes.Size = new System.Drawing.Size(224, 23);
            this.lblRunTimes.TabIndex = 17;
            this.lblRunTimes.Text = "Number of srcript run time(s)";
            // 
            // txtTasktVersion
            // 
            this.txtTasktVersion.AutoSize = true;
            this.txtTasktVersion.BackColor = System.Drawing.Color.Transparent;
            this.txtTasktVersion.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTasktVersion.ForeColor = System.Drawing.Color.SlateGray;
            this.txtTasktVersion.Location = new System.Drawing.Point(12, 298);
            this.txtTasktVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtTasktVersion.Name = "txtTasktVersion";
            this.txtTasktVersion.Size = new System.Drawing.Size(122, 28);
            this.txtTasktVersion.TabIndex = 16;
            this.txtTasktVersion.Text = "taskt version";
            // 
            // lblTastkVersion
            // 
            this.lblTastkVersion.AutoSize = true;
            this.lblTastkVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblTastkVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTastkVersion.ForeColor = System.Drawing.Color.SlateGray;
            this.lblTastkVersion.Location = new System.Drawing.Point(12, 276);
            this.lblTastkVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTastkVersion.Name = "lblTastkVersion";
            this.lblTastkVersion.Size = new System.Drawing.Size(103, 23);
            this.lblTastkVersion.TabIndex = 15;
            this.lblTastkVersion.Text = "taskt Version";
            // 
            // lblScriptDescription
            // 
            this.lblScriptDescription.AutoSize = true;
            this.lblScriptDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptDescription.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptDescription.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptDescription.Location = new System.Drawing.Point(12, 136);
            this.lblScriptDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScriptDescription.Name = "lblScriptDescription";
            this.lblScriptDescription.Size = new System.Drawing.Size(141, 23);
            this.lblScriptDescription.TabIndex = 13;
            this.lblScriptDescription.Text = "Script Description";
            // 
            // txtScriptDescription
            // 
            this.txtScriptDescription.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptDescription.Location = new System.Drawing.Point(16, 158);
            this.txtScriptDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtScriptDescription.Multiline = true;
            this.txtScriptDescription.Name = "txtScriptDescription";
            this.txtScriptDescription.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtScriptDescription.Size = new System.Drawing.Size(533, 105);
            this.txtScriptDescription.TabIndex = 14;
            // 
            // lblScriptVersion
            // 
            this.lblScriptVersion.AutoSize = true;
            this.lblScriptVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptVersion.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptVersion.Location = new System.Drawing.Point(12, 75);
            this.lblScriptVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScriptVersion.Name = "lblScriptVersion";
            this.lblScriptVersion.Size = new System.Drawing.Size(111, 23);
            this.lblScriptVersion.TabIndex = 11;
            this.lblScriptVersion.Text = "Script Version";
            // 
            // txtScriptVersion
            // 
            this.txtScriptVersion.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptVersion.Location = new System.Drawing.Point(16, 96);
            this.txtScriptVersion.Margin = new System.Windows.Forms.Padding(4);
            this.txtScriptVersion.Name = "txtScriptVersion";
            this.txtScriptVersion.Size = new System.Drawing.Size(533, 34);
            this.txtScriptVersion.TabIndex = 12;
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.ForeColor = System.Drawing.Color.SlateGray;
            this.lblAuthor.Location = new System.Drawing.Point(12, 12);
            this.lblAuthor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(107, 23);
            this.lblAuthor.TabIndex = 9;
            this.lblAuthor.Text = "Script Author";
            // 
            // txtScriptAuthor
            // 
            this.txtScriptAuthor.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptAuthor.Location = new System.Drawing.Point(16, 34);
            this.txtScriptAuthor.Margin = new System.Windows.Forms.Padding(4);
            this.txtScriptAuthor.Name = "txtScriptAuthor";
            this.txtScriptAuthor.Size = new System.Drawing.Size(533, 34);
            this.txtScriptAuthor.TabIndex = 10;
            // 
            // pnlFooterButtons
            // 
            this.pnlFooterButtons.Controls.Add(this.btnCancel);
            this.pnlFooterButtons.Controls.Add(this.btnOK);
            this.pnlFooterButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooterButtons.Location = new System.Drawing.Point(0, 373);
            this.pnlFooterButtons.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFooterButtons.Name = "pnlFooterButtons";
            this.pnlFooterButtons.Size = new System.Drawing.Size(588, 69);
            this.pnlFooterButtons.TabIndex = 19;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCancel.DisplayText = "Cancel";
            this.btnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::taskt.Properties.Resources.various_cancel_button;
            this.btnCancel.IsMouseOver = false;
            this.btnCancel.Location = new System.Drawing.Point(67, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 48);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOK.DisplayText = "OK";
            this.btnOK.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnOK.Image = global::taskt.Properties.Resources.various_ok_button;
            this.btnOK.IsMouseOver = false;
            this.btnOK.Location = new System.Drawing.Point(13, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(48, 48);
            this.btnOK.TabIndex = 0;
            this.btnOK.TabStop = false;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmScriptInformations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScriptInformations";
            this.Text = "Script Informations";
            this.Load += new System.EventHandler(this.frmScriptInformations_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlFooterButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.TextBox txtScriptAuthor;
        private System.Windows.Forms.Label lblScriptDescription;
        private System.Windows.Forms.TextBox txtScriptDescription;
        private System.Windows.Forms.Label lblScriptVersion;
        private System.Windows.Forms.TextBox txtScriptVersion;
        private System.Windows.Forms.Label txtTasktVersion;
        private System.Windows.Forms.Label lblTastkVersion;
        private System.Windows.Forms.Label txtRunTimes;
        private System.Windows.Forms.Label lblRunTimes;
        private System.Windows.Forms.Label txtLastRun;
        private System.Windows.Forms.Label lblLastRun;
        private System.Windows.Forms.Label txtScriptRevision;
        private System.Windows.Forms.Label lblScriptRevision;
        private System.Windows.Forms.Panel pnlFooterButtons;
        private CustomControls.UIPictureButton btnCancel;
        private CustomControls.UIPictureButton btnOK;
    }
}
