using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows.Forms;
using taskt.Core.IO;

namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class frmHTMLBuilder : Form
    {
        public frmHTMLBuilder()
        {
            InitializeComponent();
            
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;

            //rtbHTML.Enabled = false;
            //webBrowserHTML.CoreWebView2InitializationCompleted += this.webBrowserHTML_InitializationCompleted;

            //var udfPath = Path.Combine(Folders.GetSettingsFolderPath(), "webview2");
            //webBrowserHTML.EnsureCoreWebView2Async(null);
        }

        private async void frmHTMLBuilder_Load(object sender, EventArgs e)
        {
            rtbHTML.Enabled = false;

            SupplementFormsEvents.SupplementFormLoad(this);

            var udfPath = Path.Combine(Folders.GetSettingsFolderPath(), "webview2");
            var webView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: udfPath);
            await webBrowserHTML.EnsureCoreWebView2Async(webView2Environment);
            webBrowserHTML.NavigateToString(rtbHTML.Text);
        }

        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void rtbHTML_TextChanged(object sender, EventArgs e)
        {
            //webBrowserHTML.ScriptErrorsSuppressed = true;
            //webBrowserHTML.DocumentText = rtbHTML.Text;

            webBrowserHTML.NavigateToString(rtbHTML.Text);
        }

        private void webBrowserHTML_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            rtbHTML.Enabled = false;
        }

        private void webBrowserHTML_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            rtbHTML.Enabled = true;
        }

        private async void frmHTMLBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            await webBrowserHTML.CoreWebView2.Profile.ClearBrowsingDataAsync();
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            webBrowserHTML.NavigateToString(rtbHTML.Text);
        }

        //private void webBrowserHTML_InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        //{
        //    if (e.IsSuccess)
        //    {
        //        // ok!
        //        rtbHTML.Enabled = true;
        //        webBrowserHTML.NavigateToString(rtbHTML.Text);
        //    }
        //    else
        //    {
        //        throw new Exception("WebView2 Initialize Error!");
        //    }
        //}
    }
}
