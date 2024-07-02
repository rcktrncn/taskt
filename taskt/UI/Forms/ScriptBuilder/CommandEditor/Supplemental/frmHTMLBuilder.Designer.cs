namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    partial class frmHTMLBuilder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLBuilder));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flwAcceptIcons = new System.Windows.Forms.FlowLayoutPanel();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.btnRender = new System.Windows.Forms.Button();
            this.rtbHTML = new System.Windows.Forms.RichTextBox();
            this.webBrowserHTML = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.tableLayoutPanel1.SuspendLayout();
            this.flwAcceptIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webBrowserHTML)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flwAcceptIcons, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbHTML, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.webBrowserHTML, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1620, 643);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flwAcceptIcons
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flwAcceptIcons, 2);
            this.flwAcceptIcons.Controls.Add(this.uiBtnOK);
            this.flwAcceptIcons.Controls.Add(this.uiBtnCancel);
            this.flwAcceptIcons.Controls.Add(this.btnRender);
            this.flwAcceptIcons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwAcceptIcons.Location = new System.Drawing.Point(3, 591);
            this.flwAcceptIcons.Name = "flwAcceptIcons";
            this.flwAcceptIcons.Size = new System.Drawing.Size(1614, 49);
            this.flwAcceptIcons.TabIndex = 2;
            // 
            // uiBtnOK
            // 
            this.uiBtnOK.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOK.DisplayText = "Save";
            this.uiBtnOK.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOK.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(3, 3);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(48, 44);
            this.uiBtnOK.TabIndex = 18;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Save";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Close";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = global::taskt.Properties.Resources.various_cancel_button;
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(57, 3);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 44);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Close";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // btnRender
            // 
            this.btnRender.Location = new System.Drawing.Point(111, 3);
            this.btnRender.Name = "btnRender";
            this.btnRender.Size = new System.Drawing.Size(100, 44);
            this.btnRender.TabIndex = 20;
            this.btnRender.Text = "Render =>";
            this.btnRender.UseVisualStyleBackColor = true;
            this.btnRender.Click += new System.EventHandler(this.btnRender_Click);
            // 
            // rtbHTML
            // 
            this.rtbHTML.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHTML.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHTML.Location = new System.Drawing.Point(3, 3);
            this.rtbHTML.Name = "rtbHTML";
            this.rtbHTML.Size = new System.Drawing.Size(804, 582);
            this.rtbHTML.TabIndex = 3;
            this.rtbHTML.Text = "";
            // 
            // webBrowserHTML
            // 
            this.webBrowserHTML.AllowExternalDrop = true;
            this.webBrowserHTML.CreationProperties = null;
            this.webBrowserHTML.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webBrowserHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserHTML.Location = new System.Drawing.Point(813, 3);
            this.webBrowserHTML.Name = "webBrowserHTML";
            this.webBrowserHTML.Size = new System.Drawing.Size(804, 582);
            this.webBrowserHTML.TabIndex = 4;
            this.webBrowserHTML.ZoomFactor = 1D;
            this.webBrowserHTML.NavigationStarting += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs>(this.webBrowserHTML_NavigationStarting);
            this.webBrowserHTML.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webBrowserHTML_NavigationCompleted);
            // 
            // frmHTMLBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1620, 643);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHTMLBuilder";
            this.Text = "HTML Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHTMLBuilder_FormClosing);
            this.Load += new System.EventHandler(this.frmHTMLBuilder_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flwAcceptIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webBrowserHTML)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flwAcceptIcons;
        private CustomControls.UIPictureButton uiBtnOK;
        private CustomControls.UIPictureButton uiBtnCancel;
        public System.Windows.Forms.RichTextBox rtbHTML;
        private Microsoft.Web.WebView2.WinForms.WebView2 webBrowserHTML;
        private System.Windows.Forms.Button btnRender;
    }
}