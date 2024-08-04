﻿using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptEngine.Supplemental
{
    public partial class frmRemoteDesktopViewer : Form
    {
        public event EventHandler<LoginResultArgs> LoginUpdateEvent;

        public frmRemoteDesktopViewer(string machineName, string userName, string password, bool supportCreadSsp, int totalWidth, int totalHeight, bool hideDisplay, bool minimizeOnStart, int keyboardHookMode = 2)
        {
            InitializeComponent();

            //detect if we should attempt to hide the display
            if (hideDisplay)
            {
                pnlCover.Dock = DockStyle.Fill;
            }
            else
            {
                pnlCover.Hide();
            }

            //set text and form properties
            this.Text = $"Remote Desktop - Machine: '{machineName}' | User: '{userName}'"; 
            this.Width = totalWidth;
            this.Height = totalHeight;

            //declare credentials
            axRDP.Server = machineName;
            axRDP.UserName = userName;

            axRDP.AdvancedSettings9.ClearTextPassword = password;
            axRDP.AdvancedSettings9.EnableCredSspSupport = supportCreadSsp;

            axRDP.AdvancedSettings9.RedirectDrives = true;
            axRDP.AdvancedSettings9.RedirectPrinters = false;
            axRDP.AdvancedSettings9.RedirectClipboard = false;

            axRDP.SecuredSettings3.KeyboardHookMode = keyboardHookMode;

            //defaults to false
            //axRDP.AdvancedSettings7.RedirectDrives = false;
            //axRDP.AdvancedSettings7.RedirectPrinters = false;
            //axRDP.AdvancedSettings7.RedirectClipboard = false;

            //initiate connection
            axRDP.Connect();

            //declare timeout
            tmrLoginFailure.Enabled = true;

            if (minimizeOnStart)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void frmRemoteDesktopViewer_Load(object sender, EventArgs e)
        {
        }

        private void axRDP_OnDisconnected(object sender, AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent e)
        {
            //codes https://social.technet.microsoft.com/wiki/contents/articles/37870.rds-remote-desktop-client-disconnect-codes-and-reasons.aspx
            if (e.discReason != 3)
            {
                LoginUpdateEvent?.Invoke(this, new LoginResultArgs(axRDP.Server, LoginResultArgs.LoginResultCode.Failed, e.discReason.ToString()));
            }

            this.Close();
        }

        private void axRDP_OnLoginComplete(object sender, EventArgs e)
        {
           tmrLoginFailure.Enabled = false;
           LoginUpdateEvent?.Invoke(this, new LoginResultArgs(axRDP.Server, LoginResultArgs.LoginResultCode.Success, ""));
        }

        private void tmrLoginFailure_Tick(object sender, EventArgs e)
        {
            tmrLoginFailure.Enabled = false;
            LoginUpdateEvent?.Invoke(this, new LoginResultArgs(axRDP.Server, LoginResultArgs.LoginResultCode.Failed, "Timeout"));
        }

        private void pnlCover_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pnlCover.Hide();
        }

        private void axRDP_OnLogonError(object sender, AxMSTSCLib.IMsTscAxEvents_OnLogonErrorEvent e)
        {
            //MessageBox.Show(e.ToString(), "Logon Error");
        }
    }

    public class LoginResultArgs
    {
        public enum LoginResultCode
        {
            Success,
            Failed
        }

        public LoginResultCode Result;
        public string MachineName { get; set; }
        public string AdditionalDetail { get; set; }

        public LoginResultArgs(string userName, LoginResultCode result, string additionalDetail)
        {
            this.MachineName = userName;
            this.Result = result;
            this.AdditionalDetail = additionalDetail;
        }
    }
}
